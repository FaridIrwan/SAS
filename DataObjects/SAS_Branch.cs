using System;

namespace DataObjects
{
	public class SAS_Branch
	{
		protected int sABR_Code;
		protected string sABR_Desc;

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

		public string SABR_Desc
		{
			get
			{
				return this. sABR_Desc;
			}
			set
			{
				this. sABR_Desc = value;
			}
		}

	}
}
