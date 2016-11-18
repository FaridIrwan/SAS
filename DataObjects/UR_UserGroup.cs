using System;

namespace DataObjects
{
	public class UR_UserGroup
	{
		protected int userGroupId;
		protected string departmentID;
		protected string userGroupName;
		protected bool status;
		protected string description;
		protected string lastUpdatedBy;
		protected DateTime? lastUpdatedDtTm;

		public int UserGroupId
		{
			get
			{
				return this. userGroupId;
			}
			set
			{
				this. userGroupId = value;
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

		public string UserGroupName
		{
			get
			{
				return this. userGroupName;
			}
			set
			{
				this. userGroupName = value;
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

		public string LastUpdatedBy
		{
			get
			{
				return this. lastUpdatedBy;
			}
			set
			{
				this. lastUpdatedBy = value;
			}
		}

		public DateTime? LastUpdatedDtTm
		{
			get
			{
				return this. lastUpdatedDtTm;
			}
			set
			{
				this. lastUpdatedDtTm = value;
			}
		}

	}
}
