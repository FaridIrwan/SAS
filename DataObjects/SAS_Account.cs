using System;

namespace DataObjects
{
	public class SAS_Account
	{
		protected int transID;
		protected string transTempCode;
		protected string transCode;
		protected string creditRef;
		protected string creditRef1;
		protected string debitRef;
		protected string debitRef1;
		protected string category;
		protected string subCategory;
		protected string transType;
		protected string subType;
		protected string sourceType;
		protected DateTime? transDate;
		protected DateTime? dueDate;
		protected string batchCode;
		protected string batchIntake;
		protected DateTime? batchDate;
		protected string crRef1;
		protected string crRef2;
		protected string description;
		protected string currency;
		protected float batchTotal;
		protected float tax;
		protected float discount;
		protected float taxAmount;
		protected float discountAmount;
		protected float transAmount;
		protected float paidAmount;
		protected string transStatus;
		protected float tempAmount;
		protected float tempPaidAmount;
		protected string paymentMode;
		protected string bankCode;
		protected string payeeName;
		protected DateTime? chequeDate;
		protected string chequeNo;
		protected string voucherNo;
		protected string pocketAmount;
		protected string subRef1;
		protected string subRef2;
		protected string subRef3;
		protected string postStatus;
		protected string intStatus;
		protected string createdBy;
		protected DateTime? createdDateTime;
		protected string postedBy;
		protected DateTime? postedDateTime;
		protected string intCode;
		protected string gLCode;
		protected string updatedBy;
		protected DateTime? updatedTime;
		protected string kodUniversiti;
		protected string kumpulanPelajar;
		protected string tarikhProses;
		protected string kodBank;
		protected string noKelompok;
		protected string noWarran;
		protected string amaunWarran;
		protected string noAkaun;
		protected string statusBayaran;
		protected string sb_flag;

		public int TransID
		{
			get
			{
				return this. transID;
			}
			set
			{
				this. transID = value;
			}
		}

		public string TransTempCode
		{
			get
			{
				return this. transTempCode;
			}
			set
			{
				this. transTempCode = value;
			}
		}

		public string TransCode
		{
			get
			{
				return this. transCode;
			}
			set
			{
				this. transCode = value;
			}
		}

		public string CreditRef
		{
			get
			{
				return this. creditRef;
			}
			set
			{
				this. creditRef = value;
			}
		}

		public string CreditRef1
		{
			get
			{
				return this. creditRef1;
			}
			set
			{
				this. creditRef1 = value;
			}
		}

		public string DebitRef
		{
			get
			{
				return this. debitRef;
			}
			set
			{
				this. debitRef = value;
			}
		}

		public string DebitRef1
		{
			get
			{
				return this. debitRef1;
			}
			set
			{
				this. debitRef1 = value;
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

		public string SubCategory
		{
			get
			{
				return this. subCategory;
			}
			set
			{
				this. subCategory = value;
			}
		}

		public string TransType
		{
			get
			{
				return this. transType;
			}
			set
			{
				this. transType = value;
			}
		}

		public string SubType
		{
			get
			{
				return this. subType;
			}
			set
			{
				this. subType = value;
			}
		}

		public string SourceType
		{
			get
			{
				return this. sourceType;
			}
			set
			{
				this. sourceType = value;
			}
		}

		public DateTime? TransDate
		{
			get
			{
				return this. transDate;
			}
			set
			{
				this. transDate = value;
			}
		}

		public DateTime? DueDate
		{
			get
			{
				return this. dueDate;
			}
			set
			{
				this. dueDate = value;
			}
		}

		public string BatchCode
		{
			get
			{
				return this. batchCode;
			}
			set
			{
				this. batchCode = value;
			}
		}

		public string BatchIntake
		{
			get
			{
				return this. batchIntake;
			}
			set
			{
				this. batchIntake = value;
			}
		}

		public DateTime? BatchDate
		{
			get
			{
				return this. batchDate;
			}
			set
			{
				this. batchDate = value;
			}
		}

		public string CrRef1
		{
			get
			{
				return this. crRef1;
			}
			set
			{
				this. crRef1 = value;
			}
		}

		public string CrRef2
		{
			get
			{
				return this. crRef2;
			}
			set
			{
				this. crRef2 = value;
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

		public string Currency
		{
			get
			{
				return this. currency;
			}
			set
			{
				this. currency = value;
			}
		}

		public float BatchTotal
		{
			get
			{
				return this. batchTotal;
			}
			set
			{
				this. batchTotal = value;
			}
		}

		public float Tax
		{
			get
			{
				return this. tax;
			}
			set
			{
				this. tax = value;
			}
		}

		public float Discount
		{
			get
			{
				return this. discount;
			}
			set
			{
				this. discount = value;
			}
		}

		public float TaxAmount
		{
			get
			{
				return this. taxAmount;
			}
			set
			{
				this. taxAmount = value;
			}
		}

		public float DiscountAmount
		{
			get
			{
				return this. discountAmount;
			}
			set
			{
				this. discountAmount = value;
			}
		}

		public float TransAmount
		{
			get
			{
				return this. transAmount;
			}
			set
			{
				this. transAmount = value;
			}
		}

		public float PaidAmount
		{
			get
			{
				return this. paidAmount;
			}
			set
			{
				this. paidAmount = value;
			}
		}

		public string TransStatus
		{
			get
			{
				return this. transStatus;
			}
			set
			{
				this. transStatus = value;
			}
		}

		public float TempAmount
		{
			get
			{
				return this. tempAmount;
			}
			set
			{
				this. tempAmount = value;
			}
		}

		public float TempPaidAmount
		{
			get
			{
				return this. tempPaidAmount;
			}
			set
			{
				this. tempPaidAmount = value;
			}
		}

		public string PaymentMode
		{
			get
			{
				return this. paymentMode;
			}
			set
			{
				this. paymentMode = value;
			}
		}

		public string BankCode
		{
			get
			{
				return this. bankCode;
			}
			set
			{
				this. bankCode = value;
			}
		}

		public string PayeeName
		{
			get
			{
				return this. payeeName;
			}
			set
			{
				this. payeeName = value;
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

		public string ChequeNo
		{
			get
			{
				return this. chequeNo;
			}
			set
			{
				this. chequeNo = value;
			}
		}

		public string VoucherNo
		{
			get
			{
				return this. voucherNo;
			}
			set
			{
				this. voucherNo = value;
			}
		}

		public string PocketAmount
		{
			get
			{
				return this. pocketAmount;
			}
			set
			{
				this. pocketAmount = value;
			}
		}

		public string SubRef1
		{
			get
			{
				return this. subRef1;
			}
			set
			{
				this. subRef1 = value;
			}
		}

		public string SubRef2
		{
			get
			{
				return this. subRef2;
			}
			set
			{
				this. subRef2 = value;
			}
		}

		public string SubRef3
		{
			get
			{
				return this. subRef3;
			}
			set
			{
				this. subRef3 = value;
			}
		}

		public string PostStatus
		{
			get
			{
				return this. postStatus;
			}
			set
			{
				this. postStatus = value;
			}
		}

		public string IntStatus
		{
			get
			{
				return this. intStatus;
			}
			set
			{
				this. intStatus = value;
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

		public DateTime? CreatedDateTime
		{
			get
			{
				return this. createdDateTime;
			}
			set
			{
				this. createdDateTime = value;
			}
		}

		public string PostedBy
		{
			get
			{
				return this. postedBy;
			}
			set
			{
				this. postedBy = value;
			}
		}

		public DateTime? PostedDateTime
		{
			get
			{
				return this. postedDateTime;
			}
			set
			{
				this. postedDateTime = value;
			}
		}

		public string IntCode
		{
			get
			{
				return this. intCode;
			}
			set
			{
				this. intCode = value;
			}
		}

		public string GLCode
		{
			get
			{
				return this. gLCode;
			}
			set
			{
				this. gLCode = value;
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

		public string KodUniversiti
		{
			get
			{
				return this. kodUniversiti;
			}
			set
			{
				this. kodUniversiti = value;
			}
		}

		public string KumpulanPelajar
		{
			get
			{
				return this. kumpulanPelajar;
			}
			set
			{
				this. kumpulanPelajar = value;
			}
		}

		public string TarikhProses
		{
			get
			{
				return this. tarikhProses;
			}
			set
			{
				this. tarikhProses = value;
			}
		}

		public string KodBank
		{
			get
			{
				return this. kodBank;
			}
			set
			{
				this. kodBank = value;
			}
		}

		public string NoKelompok
		{
			get
			{
				return this. noKelompok;
			}
			set
			{
				this. noKelompok = value;
			}
		}

		public string NoWarran
		{
			get
			{
				return this. noWarran;
			}
			set
			{
				this. noWarran = value;
			}
		}

		public string AmaunWarran
		{
			get
			{
				return this. amaunWarran;
			}
			set
			{
				this. amaunWarran = value;
			}
		}

		public string NoAkaun
		{
			get
			{
				return this. noAkaun;
			}
			set
			{
				this. noAkaun = value;
			}
		}

		public string StatusBayaran
		{
			get
			{
				return this. statusBayaran;
			}
			set
			{
				this. statusBayaran = value;
			}
		}

		public string Sb_flag
		{
			get
			{
				return this. sb_flag;
			}
			set
			{
				this. sb_flag = value;
			}
		}

	}
}
