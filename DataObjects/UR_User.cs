using System;

namespace DataObjects
{
	public class UR_User
	{
		protected int userID;
		protected string userName;
		protected string password;
		protected int userGroupId;
		protected string email;
		protected string designation;
		protected string department;
		protected bool recStatus;
		protected string userStatus;
		protected string lastUpdatedBy;
		protected DateTime? lastUpdatedDtTm;

		public int UserID
		{
			get
			{
				return this. userID;
			}
			set
			{
				this. userID = value;
			}
		}

		public string UserName
		{
			get
			{
				return this. userName;
			}
			set
			{
				this. userName = value;
			}
		}

		public string Password
		{
			get
			{
				return this. password;
			}
			set
			{
				this. password = value;
			}
		}

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

		public string Email
		{
			get
			{
				return this. email;
			}
			set
			{
				this. email = value;
			}
		}

		public string Designation
		{
			get
			{
				return this. designation;
			}
			set
			{
				this. designation = value;
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

		public bool RecStatus
		{
			get
			{
				return this. recStatus;
			}
			set
			{
				this. recStatus = value;
			}
		}

		public string UserStatus
		{
			get
			{
				return this. userStatus;
			}
			set
			{
				this. userStatus = value;
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
