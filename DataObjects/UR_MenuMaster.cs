using System;

namespace DataObjects
{
	public class UR_MenuMaster
	{
		protected int menuID;
		protected string menuName;
		protected string pageName;
		protected string pageDescription;
		protected string pageUrl;
		protected string imageUrl;
		protected bool status;
		protected int pageOrder;
		protected string lastUpdatedBy;
		protected DateTime? lastUpdatedDtTm;

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

		public string MenuName
		{
			get
			{
				return this. menuName;
			}
			set
			{
				this. menuName = value;
			}
		}

		public string PageName
		{
			get
			{
				return this. pageName;
			}
			set
			{
				this. pageName = value;
			}
		}

		public string PageDescription
		{
			get
			{
				return this. pageDescription;
			}
			set
			{
				this. pageDescription = value;
			}
		}

		public string PageUrl
		{
			get
			{
				return this. pageUrl;
			}
			set
			{
				this. pageUrl = value;
			}
		}

		public string ImageUrl
		{
			get
			{
				return this. imageUrl;
			}
			set
			{
				this. imageUrl = value;
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

		public int PageOrder
		{
			get
			{
				return this. pageOrder;
			}
			set
			{
				this. pageOrder = value;
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
