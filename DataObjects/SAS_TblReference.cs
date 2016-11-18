using System;

namespace DataObjects
{
	public class SAS_TblReference
	{
		protected string tblName;
		protected string fldDesc;

		public string TblName
		{
			get
			{
				return this. tblName;
			}
			set
			{
				this. tblName = value;
			}
		}

		public string FldDesc
		{
			get
			{
				return this. fldDesc;
			}
			set
			{
				this. fldDesc = value;
			}
		}

	}
}
