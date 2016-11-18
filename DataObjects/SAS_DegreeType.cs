using System;

namespace DataObjects
{
	public class SAS_DegreeType
	{
		protected string sADT_Code;
		protected string sADT_Desc;
		protected string sADT_SName;
		protected bool sADT_Status;
		protected int sABR_Code;
		protected string sADT_UpdatedUser;
		protected string sADT_UpdatedDtTm;

		public string SADT_Code
		{
			get
			{
				return this. sADT_Code;
			}
			set
			{
				this. sADT_Code = value;
			}
		}

		public string SADT_Desc
		{
			get
			{
				return this. sADT_Desc;
			}
			set
			{
				this. sADT_Desc = value;
			}
		}

		public string SADT_SName
		{
			get
			{
				return this. sADT_SName;
			}
			set
			{
				this. sADT_SName = value;
			}
		}

		public bool SADT_Status
		{
			get
			{
				return this. sADT_Status;
			}
			set
			{
				this. sADT_Status = value;
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

		public string SADT_UpdatedUser
		{
			get
			{
				return this. sADT_UpdatedUser;
			}
			set
			{
				this. sADT_UpdatedUser = value;
			}
		}

		public string SADT_UpdatedDtTm
		{
			get
			{
				return this. sADT_UpdatedDtTm;
			}
			set
			{
				this. sADT_UpdatedDtTm = value;
			}
		}

	}
}
