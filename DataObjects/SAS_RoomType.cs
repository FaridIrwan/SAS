using System;

namespace DataObjects
{
	public class SAS_RoomType
	{
		protected string sART_Code;
		protected string sABK_Code;
		protected string sART_Description;

		public string SART_Code
		{
			get
			{
				return this. sART_Code;
			}
			set
			{
				this. sART_Code = value;
			}
		}

		public string SABK_Code
		{
			get
			{
				return this. sABK_Code;
			}
			set
			{
				this. sABK_Code = value;
			}
		}

		public string SART_Description
		{
			get
			{
				return this. sART_Description;
			}
			set
			{
				this. sART_Description = value;
			}
		}

	}
}
