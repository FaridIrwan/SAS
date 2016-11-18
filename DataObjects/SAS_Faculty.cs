using System;

namespace DataObjects
{
	public class SAS_Faculty
	{
		protected string sAFC_Code;
		protected string sAFC_Desc;
		protected string sAFC_SName;
		protected string sAFC_Incharge;
		protected string sAFC_GlAccount;
		protected bool sAFC_Status;
		protected int sABR_Code;
		protected string sAFC_UpdatedBy;
		protected string sAFC_UpdatedDtTm;

		public string SAFC_Code
		{
			get
			{
				return this. sAFC_Code;
			}
			set
			{
				this. sAFC_Code = value;
			}
		}

		public string SAFC_Desc
		{
			get
			{
				return this. sAFC_Desc;
			}
			set
			{
				this. sAFC_Desc = value;
			}
		}

		public string SAFC_SName
		{
			get
			{
				return this. sAFC_SName;
			}
			set
			{
				this. sAFC_SName = value;
			}
		}

		public string SAFC_Incharge
		{
			get
			{
				return this. sAFC_Incharge;
			}
			set
			{
				this. sAFC_Incharge = value;
			}
		}

		public string SAFC_GlAccount
		{
			get
			{
				return this. sAFC_GlAccount;
			}
			set
			{
				this. sAFC_GlAccount = value;
			}
		}

		public bool SAFC_Status
		{
			get
			{
				return this. sAFC_Status;
			}
			set
			{
				this. sAFC_Status = value;
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

		public string SAFC_UpdatedBy
		{
			get
			{
				return this. sAFC_UpdatedBy;
			}
			set
			{
				this. sAFC_UpdatedBy = value;
			}
		}

		public string SAFC_UpdatedDtTm
		{
			get
			{
				return this. sAFC_UpdatedDtTm;
			}
			set
			{
				this. sAFC_UpdatedDtTm = value;
			}
		}

	}
}
