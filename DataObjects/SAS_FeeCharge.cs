using System;

namespace DataObjects
{
	public class SAS_FeeCharge
	{
		protected string sAFT_Code;
		protected string sASC_Code;
		protected float sAFS_Amount;

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

		public float SAFS_Amount
		{
			get
			{
				return this. sAFS_Amount;
			}
			set
			{
				this. sAFS_Amount = value;
			}
		}

	}
}
