using System;

namespace DataObjects
{
	public class SAS_FeeStruct
	{
		protected string sAFS_Code;
		protected string sAPG_Code;
		protected string sAST_Code;
		protected string sAFS_EffectFm;
		protected bool sAFS_Status;
		protected string sAFS_UpdatedUser;
		protected string sAFS_UpdatedDtTm;
		protected float sAFS_CrPoint;
		protected float sAFS_TutAmt;
		protected float sAFS_CrAmt;
		protected string sAFS_Semester;
        protected string _sAFS_TaxId;

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

		public string SAPG_Code
		{
			get
			{
				return this. sAPG_Code;
			}
			set
			{
				this. sAPG_Code = value;
			}
		}

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

		public string SAFS_EffectFm
		{
			get
			{
				return this. sAFS_EffectFm;
			}
			set
			{
				this. sAFS_EffectFm = value;
			}
		}

		public bool SAFS_Status
		{
			get
			{
				return this. sAFS_Status;
			}
			set
			{
				this. sAFS_Status = value;
			}
		}

		public string SAFS_UpdatedUser
		{
			get
			{
				return this. sAFS_UpdatedUser;
			}
			set
			{
				this. sAFS_UpdatedUser = value;
			}
		}

		public string SAFS_UpdatedDtTm
		{
			get
			{
				return this. sAFS_UpdatedDtTm;
			}
			set
			{
				this. sAFS_UpdatedDtTm = value;
			}
		}

		public float SAFS_CrPoint
		{
			get
			{
				return this. sAFS_CrPoint;
			}
			set
			{
				this. sAFS_CrPoint = value;
			}
		}

		public float SAFS_TutAmt
		{
			get
			{
				return this. sAFS_TutAmt;
			}
			set
			{
				this. sAFS_TutAmt = value;
			}
		}

		public float SAFS_CrAmt
		{
			get
			{
				return this. sAFS_CrAmt;
			}
			set
			{
				this. sAFS_CrAmt = value;
			}
		}
       
		public string SAFS_Semester
		{
			get
			{
				return this. sAFS_Semester;
			}
			set
			{
				this. sAFS_Semester = value;
			}
		}
        public string TaxId
		{
			get
			{
                return this._sAFS_TaxId;
			}
			set
			{
                this._sAFS_TaxId = value;
			}
		}
	}
}
