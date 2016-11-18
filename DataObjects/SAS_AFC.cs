using System;

namespace DataObjects
{
	public class SAS_AFC
	{
		protected int transCode;
		protected string aFCode;
		protected string sAFC_Code;
		protected string batchCode;
		protected string semester;
		protected string description;
		protected DateTime? bdate;
		protected DateTime? transDate;
		protected DateTime? dueDate;
		protected float batchTotal;
		protected string postedFor;
		protected string reference;
		protected string updatedby;
		protected string updatetime;

		public int TransCode
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

		public string AFCode
		{
			get
			{
				return this. aFCode;
			}
			set
			{
				this. aFCode = value;
			}
		}

		public string SAFC_Code
		{
			get
			{
				return this. sAFC_Code;
			}
			set
			{
				this. sAFC_Code = value;
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

		public string Semester
		{
			get
			{
				return this. semester;
			}
			set
			{
				this. semester = value;
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

		public DateTime? Bdate
		{
			get
			{
				return this. bdate;
			}
			set
			{
				this. bdate = value;
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

		public string PostedFor
		{
			get
			{
				return this. postedFor;
			}
			set
			{
				this. postedFor = value;
			}
		}

		public string Reference
		{
			get
			{
				return this. reference;
			}
			set
			{
				this. reference = value;
			}
		}

		public string Updatedby
		{
			get
			{
				return this. updatedby;
			}
			set
			{
				this. updatedby = value;
			}
		}

		public string Updatetime
		{
			get
			{
				return this. updatetime;
			}
			set
			{
				this. updatetime = value;
			}
		}

	}
}
