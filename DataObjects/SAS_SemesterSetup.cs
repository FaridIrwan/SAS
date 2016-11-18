using System;

namespace DataObjects
{
	public class SAS_SemesterSetup
	{
		protected string sAST_Code;
		protected string sAST_Semester;
		protected string sAST_Description;
		protected bool sAST_Status;
		protected int sABR_Code;
		protected string sAST_UpdatedUser;
		protected string sAST_UpdatedDtTm;

		public string SAST_Code
		{
			get
			{
				return this. sAST_Code;
			}
			set
			{
				this. sAST_Code = value;
			}
		}

		public string SAST_Semester
		{
			get
			{
				return this. sAST_Semester;
			}
			set
			{
				this. sAST_Semester = value;
			}
		}

		public string SAST_Description
		{
			get
			{
				return this. sAST_Description;
			}
			set
			{
				this. sAST_Description = value;
			}
		}

		public bool SAST_Status
		{
			get
			{
				return this. sAST_Status;
			}
			set
			{
				this. sAST_Status = value;
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

		public string SAST_UpdatedUser
		{
			get
			{
				return this. sAST_UpdatedUser;
			}
			set
			{
				this. sAST_UpdatedUser = value;
			}
		}

		public string SAST_UpdatedDtTm
		{
			get
			{
				return this. sAST_UpdatedDtTm;
			}
			set
			{
				this. sAST_UpdatedDtTm = value;
			}
		}

	}
}
