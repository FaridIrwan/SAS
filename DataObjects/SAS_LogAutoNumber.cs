using System;

namespace DataObjects
{
	public class SAS_LogAutoNumber
	{
		protected int id;
		protected string batchNo;
		protected string transactionNo;
		protected string status;
		protected string category;
		protected string createdBy;
		protected DateTime? createdOn;

		public int Id
		{
			get
			{
				return this. id;
			}
			set
			{
				this. id = value;
			}
		}

		public string BatchNo
		{
			get
			{
				return this. batchNo;
			}
			set
			{
				this. batchNo = value;
			}
		}

		public string TransactionNo
		{
			get
			{
				return this. transactionNo;
			}
			set
			{
				this. transactionNo = value;
			}
		}

		public string Status
		{
			get
			{
				return this. status;
			}
			set
			{
				this. status = value;
			}
		}

		public string Category
		{
			get
			{
				return this. category;
			}
			set
			{
				this. category = value;
			}
		}

		public string CreatedBy
		{
			get
			{
				return this. createdBy;
			}
			set
			{
				this. createdBy = value;
			}
		}

		public DateTime? CreatedOn
		{
			get
			{
				return this. createdOn;
			}
			set
			{
				this. createdOn = value;
			}
		}

	}
}
