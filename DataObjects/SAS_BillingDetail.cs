using System;

namespace DataObjects
{
	public class SAS_BillingDetail
	{
		protected int sBD_ID;
		protected string sBD_BatchCode;
		protected string sAFT_Desc;
		protected string aFC_Code;
		protected string sAFT_Code;
		protected float sBD_TransAmount;

		public int SBD_ID
		{
			get
			{
				return this. sBD_ID;
			}
			set
			{
				this. sBD_ID = value;
			}
		}

		public string SBD_BatchCode
		{
			get
			{
				return this. sBD_BatchCode;
			}
			set
			{
				this. sBD_BatchCode = value;
			}
		}

		public string SAFT_Desc
		{
			get
			{
				return this. sAFT_Desc;
			}
			set
			{
				this. sAFT_Desc = value;
			}
		}

		public string AFC_Code
		{
			get
			{
				return this. aFC_Code;
			}
			set
			{
				this. aFC_Code = value;
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

		public float SBD_TransAmount
		{
			get
			{
				return this. sBD_TransAmount;
			}
			set
			{
				this. sBD_TransAmount = value;
			}
		}

	}
}
