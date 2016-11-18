using System;

namespace DataObjects
{
	public class SAS_FeeStrAmount
	{
		protected string sAFS_Code;
		protected string sAFT_Code;
		protected string sAFD_Type;
		protected string sASC_Code;
		protected float sAFA_Amount;
        protected float sAFA_GSTAmount;
		public string SAFS_Code
		{
			get
			{
				return this. sAFS_Code;
			}
			set
			{
				this. sAFS_Code = value;
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

		public string SAFD_Type
		{
			get
			{
				return this. sAFD_Type;
			}
			set
			{
				this. sAFD_Type = value;
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

		public float SAFA_Amount
		{
			get
			{
				return this. sAFA_Amount;
			}
			set
			{
				this. sAFA_Amount = value;
			}
		}
        public float GSTAmount
		{
			get
			{
                return this.sAFA_GSTAmount;
			}
			set
			{
                this.sAFA_GSTAmount = value;
			}
		}
	}
}
