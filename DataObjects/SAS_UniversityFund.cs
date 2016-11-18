using System;

namespace DataObjects
{
	public class SAS_UniversityFund
	{
		protected string sAUF_Code;
		protected string sAUF_Desc;
		protected string sAUF_GLCode;
		protected int sABR_Code;
		protected bool sAUF_Status;
		protected string sASUF_UpdatedBy;
		protected string sASUF_UpdatedDtTm;

		public string SAUF_Code
		{
			get
			{
				return this. sAUF_Code;
			}
			set
			{
				this. sAUF_Code = value;
			}
		}

		public string SAUF_Desc
		{
			get
			{
				return this. sAUF_Desc;
			}
			set
			{
				this. sAUF_Desc = value;
			}
		}

		public string SAUF_GLCode
		{
			get
			{
				return this. sAUF_GLCode;
			}
			set
			{
				this. sAUF_GLCode = value;
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

		public bool SAUF_Status
		{
			get
			{
				return this. sAUF_Status;
			}
			set
			{
				this. sAUF_Status = value;
			}
		}

		public string SASUF_UpdatedBy
		{
			get
			{
				return this. sASUF_UpdatedBy;
			}
			set
			{
				this. sASUF_UpdatedBy = value;
			}
		}

		public string SASUF_UpdatedDtTm
		{
			get
			{
				return this. sASUF_UpdatedDtTm;
			}
			set
			{
				this. sASUF_UpdatedDtTm = value;
			}
		}

	}
}
