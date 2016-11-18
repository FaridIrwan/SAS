using System;

namespace DataObjects
{
	public class SAS_AutoNumber
	{
		protected int sAAN_Code;
		protected string sAAN_Des;
		protected string sAAN_Prefix;
		protected int sAAN_NoDigit;
		protected int sAAN_StartNo;
		protected int sAAN_CurNo;
		protected string sAAN_AutoNo;

		public int SAAN_Code
		{
			get
			{
				return this. sAAN_Code;
			}
			set
			{
				this. sAAN_Code = value;
			}
		}

		public string SAAN_Des
		{
			get
			{
				return this. sAAN_Des;
			}
			set
			{
				this. sAAN_Des = value;
			}
		}

		public string SAAN_Prefix
		{
			get
			{
				return this. sAAN_Prefix;
			}
			set
			{
				this. sAAN_Prefix = value;
			}
		}

		public int SAAN_NoDigit
		{
			get
			{
				return this. sAAN_NoDigit;
			}
			set
			{
				this. sAAN_NoDigit = value;
			}
		}

		public int SAAN_StartNo
		{
			get
			{
				return this. sAAN_StartNo;
			}
			set
			{
				this. sAAN_StartNo = value;
			}
		}

		public int SAAN_CurNo
		{
			get
			{
				return this. sAAN_CurNo;
			}
			set
			{
				this. sAAN_CurNo = value;
			}
		}

		public string SAAN_AutoNo
		{
			get
			{
				return this. sAAN_AutoNo;
			}
			set
			{
				this. sAAN_AutoNo = value;
			}
		}

	}
}
