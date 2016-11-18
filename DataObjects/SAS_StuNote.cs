using System;

namespace DataObjects
{
	public class SAS_StuNote
	{
		protected int sASN_Code;
		protected string sASI_MatricNo;
		protected string sASN_Remarks;
		protected string sASN_UpdatedBy;
		protected string sASN_UpdatedDtTm;

		public int SASN_Code
		{
			get
			{
				return this. sASN_Code;
			}
			set
			{
				this. sASN_Code = value;
			}
		}

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

		public string SASN_Remarks
		{
			get
			{
				return this. sASN_Remarks;
			}
			set
			{
				this. sASN_Remarks = value;
			}
		}

		public string SASN_UpdatedBy
		{
			get
			{
				return this. sASN_UpdatedBy;
			}
			set
			{
				this. sASN_UpdatedBy = value;
			}
		}

		public string SASN_UpdatedDtTm
		{
			get
			{
				return this. sASN_UpdatedDtTm;
			}
			set
			{
				this. sASN_UpdatedDtTm = value;
			}
		}

	}
}
