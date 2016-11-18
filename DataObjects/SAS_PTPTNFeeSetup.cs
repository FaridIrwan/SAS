using System;

namespace DataObjects
{
	public class SAS_PTPTNFeeSetup
	{
		protected int autoID;
		protected string progCode;
		protected string progName;
		protected decimal progFee;
		protected string createdBy;
		protected DateTime? createdDate;

		public int AutoID
		{
			get
			{
				return this. autoID;
			}
			set
			{
				this. autoID = value;
			}
		}

		public string ProgCode
		{
			get
			{
				return this. progCode;
			}
			set
			{
				this. progCode = value;
			}
		}

		public string ProgName
		{
			get
			{
				return this. progName;
			}
			set
			{
				this. progName = value;
			}
		}

		public decimal ProgFee
		{
			get
			{
				return this. progFee;
			}
			set
			{
				this. progFee = value;
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

		public DateTime? CreatedDate
		{
			get
			{
				return this. createdDate;
			}
			set
			{
				this. createdDate = value;
			}
		}

	}
}
