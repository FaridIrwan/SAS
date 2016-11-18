using System;

namespace DataObjects
{
	public class SAS_PayMode
	{
		protected string sAPM_Code;
		protected string sAPM_Des;
		protected bool sAPM_Status;

		public string SAPM_Code
		{
			get
			{
				return this. sAPM_Code;
			}
			set
			{
				this. sAPM_Code = value;
			}
		}

		public string SAPM_Des
		{
			get
			{
				return this. sAPM_Des;
			}
			set
			{
				this. sAPM_Des = value;
			}
		}

		public bool SAPM_Status
		{
			get
			{
				return this. sAPM_Status;
			}
			set
			{
				this. sAPM_Status = value;
			}
		}

	}
}
