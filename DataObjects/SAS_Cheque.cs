using System;

namespace DataObjects
{
	public class SAS_Cheque
	{
		protected string processID;
		protected string paymentNo;
		protected string paymentType;
		protected string description;
		protected DateTime? chequeDate;
		protected DateTime? transactionDate;
		protected string printStatus;
		protected string updatedBy;
		protected DateTime? updatedTime;

		public string ProcessID
		{
			get
			{
				return this. processID;
			}
			set
			{
				this. processID = value;
			}
		}

		public string PaymentNo
		{
			get
			{
				return this. paymentNo;
			}
			set
			{
				this. paymentNo = value;
			}
		}

		public string PaymentType
		{
			get
			{
				return this. paymentType;
			}
			set
			{
				this. paymentType = value;
			}
		}

		public string Description
		{
			get
			{
				return this. description;
			}
			set
			{
				this. description = value;
			}
		}

		public DateTime? ChequeDate
		{
			get
			{
				return this. chequeDate;
			}
			set
			{
				this. chequeDate = value;
			}
		}

		public DateTime? TransactionDate
		{
			get
			{
				return this. transactionDate;
			}
			set
			{
				this. transactionDate = value;
			}
		}

		public string PrintStatus
		{
			get
			{
				return this. printStatus;
			}
			set
			{
				this. printStatus = value;
			}
		}

		public string UpdatedBy
		{
			get
			{
				return this. updatedBy;
			}
			set
			{
				this. updatedBy = value;
			}
		}

		public DateTime? UpdatedTime
		{
			get
			{
				return this. updatedTime;
			}
			set
			{
				this. updatedTime = value;
			}
		}

	}
}
