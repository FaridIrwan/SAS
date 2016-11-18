using System;

namespace DataObjects
{
	public class SAS_HostelStrDetail
	{
		protected string sAHS_Code;
		protected string sAHD_Code;
		protected string sAFT_Code;
		protected string sAHD_Type;
		protected int sAHD_Priority;

		public string SAHS_Code
		{
			get
			{
				return this. sAHS_Code;
			}
			set
			{
				this. sAHS_Code = value;
			}
		}

		public string SAHD_Code
		{
			get
			{
				return this. sAHD_Code;
			}
			set
			{
				this. sAHD_Code = value;
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

		public string SAHD_Type
		{
			get
			{
				return this. sAHD_Type;
			}
			set
			{
				this. sAHD_Type = value;
			}
		}

		public int SAHD_Priority
		{
			get
			{
				return this. sAHD_Priority;
			}
			set
			{
				this. sAHD_Priority = value;
			}
		}

	}
}
