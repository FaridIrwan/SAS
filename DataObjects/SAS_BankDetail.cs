using System;

namespace DataObjects
{
	public class SAS_BankDetail
	{
		protected string sABD_Code;
		protected string sABD_Desc;
		protected string sABD_ACCode;
		protected string sABD_GLCode;
		protected bool sABD_Status;
		protected int sABR_Code;
		protected string sABD_UpdatedBy;
		protected string sABD_UpdatedDtTm;

		public string SABD_Code
		{
			get
			{
				return this. sABD_Code;
			}
			set
			{
				this. sABD_Code = value;
			}
		}

		public string SABD_Desc
		{
			get
			{
				return this. sABD_Desc;
			}
			set
			{
				this. sABD_Desc = value;
			}
		}

		public string SABD_ACCode
		{
			get
			{
				return this. sABD_ACCode;
			}
			set
			{
				this. sABD_ACCode = value;
			}
		}

		public string SABD_GLCode
		{
			get
			{
				return this. sABD_GLCode;
			}
			set
			{
				this. sABD_GLCode = value;
			}
		}

		public bool SABD_Status
		{
			get
			{
				return this. sABD_Status;
			}
			set
			{
				this. sABD_Status = value;
			}
		}

		public int SABR_Code
		{
			get
			{
				return this. sABR_Code;
			}
			set
			{
				this. sABR_Code = value;
			}
		}

		public string SABD_UpdatedBy
		{
			get
			{
				return this. sABD_UpdatedBy;
			}
			set
			{
				this. sABD_UpdatedBy = value;
			}
		}

		public string SABD_UpdatedDtTm
		{
			get
			{
				return this. sABD_UpdatedDtTm;
			}
			set
			{
				this. sABD_UpdatedDtTm = value;
			}
		}

	}
}
