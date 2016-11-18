using System;

namespace DataObjects
{
	public class SAS_KokorikulumDetail
	{
		protected int sAKOD_Id;
		protected string sAKOD_CategoryCode;
		protected float sAKOD_FeeAmount;
		protected string sAKO_Code;
		protected string sAKOD_CategoryName;

		public int SAKOD_Id
		{
			get
			{
				return this. sAKOD_Id;
			}
			set
			{
				this. sAKOD_Id = value;
			}
		}

		public string SAKOD_CategoryCode
		{
			get
			{
				return this. sAKOD_CategoryCode;
			}
			set
			{
				this. sAKOD_CategoryCode = value;
			}
		}

		public float SAKOD_FeeAmount
		{
			get
			{
				return this. sAKOD_FeeAmount;
			}
			set
			{
				this. sAKOD_FeeAmount = value;
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

		public string SAKOD_CategoryName
		{
			get
			{
				return this. sAKOD_CategoryName;
			}
			set
			{
				this. sAKOD_CategoryName = value;
			}
		}

	}
}
