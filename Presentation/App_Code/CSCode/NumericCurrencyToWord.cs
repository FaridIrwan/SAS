using Microsoft.VisualBasic.CompilerServices;
using System;

	public class NumericCurrencyToWord
	
	{
		
		#region Fields
			private string[] ones;
			private string oneWords;
			private string[] tens;
			private string tenWords;
		#endregion
		
		#region Constructors
		
			public NumericCurrencyToWord ()
			
			{
				this.oneWords = ",One,Two,Three,Four,Five,Six,Seven,Eight,Nine,Ten,Eleven,Twelve,Thirteen,Fourtee"
				+ "n,Fifteen,Sixteen,Seventeen,Eighteen,Nineteen";
				char[] charArray1 = new char[] { '\u002C' };
				this.ones = this.oneWords.Split (charArray1);
				this.tenWords = ",Ten,Twenty,Thirty,Forty,Fifty,Sixty,Seventy,Eighty,Ninety";
				char[] charArray2 = new char[] { '\u002C' };
				this.tens = this.tenWords.Split (charArray2);
			}
			
		#endregion
		
		#region Methods
		
			public string Convert (string input, string CurrencyUnit, string CurrencySubUnit)
			
			{
				string string1 = null;
				string string3 = null;
				input = input.Replace ("$", "").Replace (",", "");
				if (input.Length > 12)
				{
					return "Error in Input Value";
				}
				if (input.IndexOf (".") > 0)
				{
					string3 = input.Substring (0, input.IndexOf (".")).PadLeft (9, '0');
					string1 = input.Substring (((int) (input.IndexOf (".") + 1))).PadRight (2, '0');
					if (Operators.CompareString (string1, "00", false) == 0)
					{
						string1 = "0";
					}
				}
				else
				{
					string3 = input.PadLeft (9, '0');
					string1 = "0";
				}
				int i3 = Conversions.ToInteger (string3.Substring (0, 3));
				int i4 = Conversions.ToInteger (string3.Substring (3, 3));
				int i2 = Conversions.ToInteger (string3.Substring (6, 3));
				int i1 = Conversions.ToInteger (string1);
				string string5 = this.convertHundereds (i3);
				string string7 = this.convertHundereds (i4);
				string string4 = this.convertHundereds (i2);
				string1 = this.convertHundereds (i1);
				string string6 = Conversions.ToString (((Operators.CompareString (string5.Trim (), "", false) == 0) ? "" : (string5 + " Million ")));
				string6 = Conversions.ToString (Operators.AddObject (string6, ((Operators.CompareString (string7.Trim (), "", false) == 0) ? "" : (string7 + " Thousand "))));
				string6 = Conversions.ToString (Operators.AddObject (string6, ((Operators.CompareString (string4.Trim (), "", false) == 0) ? "" : string4)));
				if (Operators.CompareString (string1, "", false) == 0)
				{
                    string6 = Conversions.ToString(((string6.Length == 0) ? ("Zero " + CurrencyUnit + "Only") : (string6 + " " + CurrencyUnit + "Only")));
                    return Conversions.ToString(((Operators.CompareString(string6, ("One " + CurrencyUnit + " Only"), false) == 0) ? ("One " + CurrencyUnit) : string6));
				}
				else
				{
					string6 = Conversions.ToString (((string6.Length == 0) ? ("Zero " + CurrencyUnit + "and ") : (string6 + " " + CurrencyUnit + "and ")));
					string6 = Conversions.ToString (((Operators.CompareString (string6, ("One " + CurrencyUnit + "and "), false) == 0) ? ("One " + CurrencyUnit + "and ") : string6));
					return Conversions.ToString (Operators.AddObject (string6, Operators.ConcatenateObject (Operators.AddObject (((Operators.CompareString (string1, "", false) == 0) ? "Zero" : string1), " "), CurrencySubUnit)));
				}
			}
			
			private string convertHundereds (int input)
			
			{
				if (input <= 99)
				{
					return this.convertTens (input);
				}
				string string2 = this.ones[((int) Math.Round (Math.Floor ((((double) input) / 100D))))];
				string2 = (string2 + " Hundred ");
				if ((((double) input) - (Math.Floor ((((double) input) / 100D)) * 100D)) == 0D)
				{
					return (string2 + "");
				}
				else
				{
					return (string2 + "" + this.convertTens (((int) Math.Round ((((double) input) - (Math.Floor ((((double) input) / 100D)) * 100D))))));
				}
			}
			
			private string convertTens (int input)
			
			{
				string string2 = null;
				if (input < 20)
				{
					string2 = this.ones[input];
					input = 0;
				}
				else
				{
					string2 = this.tens[((int) Math.Round (Math.Floor ((((double) input) / 10D))))];
					input = ((int) Math.Round ((((double) input) - (Math.Floor ((((double) input) / 10D)) * 10D))));
				}
				return Conversions.ToString (Operators.AddObject (string2, ((Operators.CompareString (this.ones[input].Trim (), "", false) == 0) ? "" : ("-" + this.ones[input]))));
			}
			
		#endregion
	}


