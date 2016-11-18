using System;

namespace DataObjects
{
	public class SAS_SponsorFeeType
	{
		protected string sASR_Code;
		protected string sAFT_Code;

		public string SASR_Code
		{
			get
			{
				return this. sASR_Code;
			}
			set
			{
				this. sASR_Code = value;
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

	}
}
