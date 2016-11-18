using System;

namespace DataObjects
{
	public class SAS_DunningLetter
	{
		protected string sADL_Code;
		protected string sADL_Title;
		protected string sADL_Ref;
		protected string sADL_Message;
		protected string sADL_SignBy;
		protected string sADL_Name;
		protected DateTime? sADL_FrDate;
		protected DateTime? sADL_ToDate;
		protected string sADL_UpdatedBy;
		protected DateTime? sADL_UpdatedTime;

		public string SADL_Code
		{
			get
			{
				return this. sADL_Code;
			}
			set
			{
				this. sADL_Code = value;
			}
		}

		public string SADL_Title
		{
			get
			{
				return this. sADL_Title;
			}
			set
			{
				this. sADL_Title = value;
			}
		}

		public string SADL_Ref
		{
			get
			{
				return this. sADL_Ref;
			}
			set
			{
				this. sADL_Ref = value;
			}
		}

		public string SADL_Message
		{
			get
			{
				return this. sADL_Message;
			}
			set
			{
				this. sADL_Message = value;
			}
		}

		public string SADL_SignBy
		{
			get
			{
				return this. sADL_SignBy;
			}
			set
			{
				this. sADL_SignBy = value;
			}
		}

		public string SADL_Name
		{
			get
			{
				return this. sADL_Name;
			}
			set
			{
				this. sADL_Name = value;
			}
		}

		public DateTime? SADL_FrDate
		{
			get
			{
				return this. sADL_FrDate;
			}
			set
			{
				this. sADL_FrDate = value;
			}
		}

		public DateTime? SADL_ToDate
		{
			get
			{
				return this. sADL_ToDate;
			}
			set
			{
				this. sADL_ToDate = value;
			}
		}

		public string SADL_UpdatedBy
		{
			get
			{
				return this. sADL_UpdatedBy;
			}
			set
			{
				this. sADL_UpdatedBy = value;
			}
		}

		public DateTime? SADL_UpdatedTime
		{
			get
			{
				return this. sADL_UpdatedTime;
			}
			set
			{
				this. sADL_UpdatedTime = value;
			}
		}

	}
}
