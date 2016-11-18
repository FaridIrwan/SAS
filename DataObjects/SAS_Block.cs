using System;

namespace DataObjects
{
	public class SAS_Block
	{
		protected string sABK_Code;
		protected string sAKO_Code;
		protected string sABK_Description;

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

		public string SAKO_Code
		{
			get
			{
				return this. sAKO_Code;
			}
			set
			{
				this. sAKO_Code = value;
			}
		}

		public string SABK_Description
		{
			get
			{
				return this. sABK_Description;
			}
			set
			{
				this. sABK_Description = value;
			}
		}

	}
}
