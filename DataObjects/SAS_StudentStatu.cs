using System;

namespace DataObjects
{
	public class SAS_StudentStatu
	{
		protected string sASS_Code;
		protected string sASS_Description;
		protected bool sASS_BlStatus;
		protected int sABR_Code;
		protected bool sASS_Status;
		protected string sASS_UpdatedUser;
		protected string sASS_UpdatedDtTm;

		public string SASS_Code
		{
			get
			{
				return this. sASS_Code;
			}
			set
			{
				this. sASS_Code = value;
			}
		}

		public string SASS_Description
		{
			get
			{
				return this. sASS_Description;
			}
			set
			{
				this. sASS_Description = value;
			}
		}

		public bool SASS_BlStatus
		{
			get
			{
				return this. sASS_BlStatus;
			}
			set
			{
				this. sASS_BlStatus = value;
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

		public bool SASS_Status
		{
			get
			{
				return this. sASS_Status;
			}
			set
			{
				this. sASS_Status = value;
			}
		}

		public string SASS_UpdatedUser
		{
			get
			{
				return this. sASS_UpdatedUser;
			}
			set
			{
				this. sASS_UpdatedUser = value;
			}
		}

		public string SASS_UpdatedDtTm
		{
			get
			{
				return this. sASS_UpdatedDtTm;
			}
			set
			{
				this. sASS_UpdatedDtTm = value;
			}
		}

	}
}
