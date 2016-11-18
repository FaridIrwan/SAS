using System;

namespace DataObjects
{
	public class SAS_StudentCategory
	{
		protected string sASC_Code;
		protected string sASC_Desc;
		protected int sABR_Code;
		protected bool sASC_Status;
		protected string sASC_UpdatedBy;
		protected string sASC_UpdatedDtTm;

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

		public string SASC_Desc
		{
			get
			{
				return this. sASC_Desc;
			}
			set
			{
				this. sASC_Desc = value;
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

		public bool SASC_Status
		{
			get
			{
				return this. sASC_Status;
			}
			set
			{
				this. sASC_Status = value;
			}
		}

		public string SASC_UpdatedBy
		{
			get
			{
				return this. sASC_UpdatedBy;
			}
			set
			{
				this. sASC_UpdatedBy = value;
			}
		}

		public string SASC_UpdatedDtTm
		{
			get
			{
				return this. sASC_UpdatedDtTm;
			}
			set
			{
				this. sASC_UpdatedDtTm = value;
			}
		}

	}
}
