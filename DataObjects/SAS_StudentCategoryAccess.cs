using System;

namespace DataObjects
{
	public class SAS_StudentCategoryAccess
	{
		protected string sASC_Code;
		protected int menuID;
		protected bool status;

		public string SASC_Code
		{
			get
			{
				return this. sASC_Code;
			}
			set
			{
				this. sASC_Code = value;
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

	}
}
