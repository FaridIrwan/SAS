using System;

namespace DataObjects
{
	public class SAS_StuSponFeeType
	{
		protected string sASI_MatricNo;
		protected string sASR_Code;
		protected string sAFT_Code;

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

		public string SASR_Code
		{
			get
			{
				return this. sASR_Code;
			}
			set
			{
				this. sASR_Code = value;
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

	}
}
