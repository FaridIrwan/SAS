using System;

namespace DataObjects
{
	public class SAS_ReceiptHistory
	{
		protected int sARH_Id;
		protected DateTime? sARH_Date;
		protected string sARH_ReceiptNo;
		protected string sARH_Updatedby;
		protected string sARH_Deletedby;

		public int SARH_Id
		{
			get
			{
				return this. sARH_Id;
			}
			set
			{
				this. sARH_Id = value;
			}
		}

		public DateTime? SARH_Date
		{
			get
			{
				return this. sARH_Date;
			}
			set
			{
				this. sARH_Date = value;
			}
		}

		public string SARH_ReceiptNo
		{
			get
			{
				return this. sARH_ReceiptNo;
			}
			set
			{
				this. sARH_ReceiptNo = value;
			}
		}

		public string SARH_Updatedby
		{
			get
			{
				return this. sARH_Updatedby;
			}
			set
			{
				this. sARH_Updatedby = value;
			}
		}

		public string SARH_Deletedby
		{
			get
			{
				return this. sARH_Deletedby;
			}
			set
			{
				this. sARH_Deletedby = value;
			}
		}

	}
}
