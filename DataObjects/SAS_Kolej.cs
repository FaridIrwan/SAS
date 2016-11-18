using System;

namespace DataObjects
{
	public class SAS_Kolej
	{
		protected string sAKO_Code;
		protected string sAKO_Description;

		public string SAKO_Code
		{
			get
			{
				return this. sAKO_Code;
			}
			set
			{
				this. sAKO_Code = value;
			}
		}

		public string SAKO_Description
		{
			get
			{
				return this. sAKO_Description;
			}
			set
			{
				this. sAKO_Description = value;
			}
		}

	}
}
