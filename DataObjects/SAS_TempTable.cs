using System;

namespace DataObjects
{
	public class SAS_TempTable
	{
		protected string matricNo;
		protected string value_1;

		public string MatricNo
		{
			get
			{
				return this. matricNo;
			}
			set
			{
				this. matricNo = value;
			}
		}

		public string Value_1
		{
			get
			{
				return this. value_1;
			}
			set
			{
				this. value_1 = value;
			}
		}

	}
}
