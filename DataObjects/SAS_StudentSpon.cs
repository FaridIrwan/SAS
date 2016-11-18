using System;

namespace DataObjects
{
	public class SAS_StudentSpon
	{
		protected string sASI_MatricNo;
		protected string sASS_Sponsor;
		protected string sASS_SDate;
		protected string sASS_EDate;
		protected bool sASS_Status;
		protected int sASS_Num;
		protected bool sASS_Type;

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

		public string SASS_Sponsor
		{
			get
			{
				return this. sASS_Sponsor;
			}
			set
			{
				this. sASS_Sponsor = value;
			}
		}

		public string SASS_SDate
		{
			get
			{
				return this. sASS_SDate;
			}
			set
			{
				this. sASS_SDate = value;
			}
		}

		public string SASS_EDate
		{
			get
			{
				return this. sASS_EDate;
			}
			set
			{
				this. sASS_EDate = value;
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

		public int SASS_Num
		{
			get
			{
				return this. sASS_Num;
			}
			set
			{
				this. sASS_Num = value;
			}
		}

		public bool SASS_Type
		{
			get
			{
				return this. sASS_Type;
			}
			set
			{
				this. sASS_Type = value;
			}
		}

	}
}
