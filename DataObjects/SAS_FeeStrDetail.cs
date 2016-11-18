using System;

namespace DataObjects
{
	public class SAS_FeeStrDetail
	{
		protected string sAFS_Code;
		protected string sAFD_Type;
		protected string sAFT_Code;
		protected int sAFD_Priority;
		protected string sAFD_FeeFor;
		protected int sAFD_Sem;
        protected int _sAFS_TaxId;
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

		public int SAFD_Priority
		{
			get
			{
				return this. sAFD_Priority;
			}
			set
			{
				this. sAFD_Priority = value;
			}
		}

		public string SAFD_FeeFor
		{
			get
			{
				return this. sAFD_FeeFor;
			}
			set
			{
				this. sAFD_FeeFor = value;
			}
		}

		public int SAFD_Sem
		{
			get
			{
				return this. sAFD_Sem;
			}
			set
			{
				this. sAFD_Sem = value;
			}
		}
        public int TaxId
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
