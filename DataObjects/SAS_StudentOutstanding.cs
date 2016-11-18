using System;

namespace DataObjects
{
	public class SAS_StudentOutstanding
	{
		protected int sASO_Id;
		protected string sASI_MatricNo;
		protected string sASI_Name;
		protected string sASI_PgId;
		protected int sASI_CurSem;
		protected string sASI_CurSemYr;
		protected double sASO_Outstandingamt;
		protected int sASO_IsReleased;

		public int SASO_Id
		{
			get
			{
				return this. sASO_Id;
			}
			set
			{
				this. sASO_Id = value;
			}
		}

		public string SASI_MatricNo
		{
			get
			{
				return this. sASI_MatricNo;
			}
			set
			{
				this. sASI_MatricNo = value;
			}
		}

		public string SASI_Name
		{
			get
			{
				return this. sASI_Name;
			}
			set
			{
				this. sASI_Name = value;
			}
		}

		public string SASI_PgId
		{
			get
			{
				return this. sASI_PgId;
			}
			set
			{
				this. sASI_PgId = value;
			}
		}

		public int SASI_CurSem
		{
			get
			{
				return this. sASI_CurSem;
			}
			set
			{
				this. sASI_CurSem = value;
			}
		}

		public string SASI_CurSemYr
		{
			get
			{
				return this. sASI_CurSemYr;
			}
			set
			{
				this. sASI_CurSemYr = value;
			}
		}

        public double SASO_Outstandingamt
		{
			get
			{
                return this.sASO_Outstandingamt;
			}
			set
			{
                this.sASO_Outstandingamt = value;
			}
		}

		public int SASO_IsReleased
		{
			get
			{
				return this. sASO_IsReleased;
			}
			set
			{
				this. sASO_IsReleased = value;
			}
		}

	}
}
