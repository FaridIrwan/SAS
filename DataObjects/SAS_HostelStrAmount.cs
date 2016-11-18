using System;

namespace DataObjects
{
	public class SAS_HostelStrAmount
	{
		protected string sAHS_Code;
		protected string sAFT_Code;
		protected string sASC_Code;
		protected float sAHA_Amount;

		public string SAHS_Code
		{
			get
			{
				return this. sAHS_Code;
			}
			set
			{
				this. sAHS_Code = value;
			}
		}

		public string SAFT_Code
		{
			get
			{
				return this. sAFT_Code;
			}
			set
			{
				this. sAFT_Code = value;
			}
		}

		public string SASC_Code
		{
			get
			{
				return this. sASC_Code;
			}
			set
			{
				this. sASC_Code = value;
			}
		}

		public float SAHA_Amount
		{
			get
			{
				return this. sAHA_Amount;
			}
			set
			{
				this. sAHA_Amount = value;
			}
		}

	}
}
