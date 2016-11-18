using System;

namespace DataObjects
{
	public class UR_UserRight
	{
		protected int userGroupId;
		protected int menuID;
		protected bool isAdd;
		protected bool isEdit;
		protected bool isDelete;
		protected bool isView;
		protected bool isPrint;
		protected bool isPost;
		protected bool isOthers;
		protected bool defaultMode;
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

		public int MenuID
		{
			get
			{
				return this. menuID;
			}
			set
			{
				this. menuID = value;
			}
		}

		public bool IsAdd
		{
			get
			{
				return this. isAdd;
			}
			set
			{
				this. isAdd = value;
			}
		}

		public bool IsEdit
		{
			get
			{
				return this. isEdit;
			}
			set
			{
				this. isEdit = value;
			}
		}

		public bool IsDelete
		{
			get
			{
				return this. isDelete;
			}
			set
			{
				this. isDelete = value;
			}
		}

		public bool IsView
		{
			get
			{
				return this. isView;
			}
			set
			{
				this. isView = value;
			}
		}

		public bool IsPrint
		{
			get
			{
				return this. isPrint;
			}
			set
			{
				this. isPrint = value;
			}
		}

		public bool IsPost
		{
			get
			{
				return this. isPost;
			}
			set
			{
				this. isPost = value;
			}
		}

		public bool IsOthers
		{
			get
			{
				return this. isOthers;
			}
			set
			{
				this. isOthers = value;
			}
		}

		public bool DefaultMode
		{
			get
			{
				return this. defaultMode;
			}
			set
			{
				this. defaultMode = value;
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
