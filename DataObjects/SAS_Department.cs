using System;

namespace DataObjects
{
	public class SAS_Department
	{
		protected int autoID;
		protected string departmentID;
		protected string department;
		protected bool status;
		protected string createdBy;
		protected DateTime? createDate;
		protected string modifiedBy;
		protected DateTime? modifiedDate;

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

		public string DepartmentID
		{
			get
			{
				return this. departmentID;
			}
			set
			{
				this. departmentID = value;
			}
		}

		public string Department
		{
			get
			{
				return this. department;
			}
			set
			{
				this. department = value;
			}
		}

		public bool Status
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

		public DateTime? CreateDate
		{
			get
			{
				return this. createDate;
			}
			set
			{
				this. createDate = value;
			}
		}

		public string ModifiedBy
		{
			get
			{
				return this. modifiedBy;
			}
			set
			{
				this. modifiedBy = value;
			}
		}

		public DateTime? ModifiedDate
		{
			get
			{
				return this. modifiedDate;
			}
			set
			{
				this. modifiedDate = value;
			}
		}

	}
}
