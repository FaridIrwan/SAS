using System;

namespace DataObjects
{
	public class SAS_ChequeDetail
	{
		protected string processId;
		protected string chequeStartNo;
		protected string chequeEndNo;

		public string ProcessId
		{
			get
			{
				return this. processId;
			}
			set
			{
				this. processId = value;
			}
		}

		public string ChequeStartNo
		{
			get
			{
				return this. chequeStartNo;
			}
			set
			{
				this. chequeStartNo = value;
			}
		}

		public string ChequeEndNo
		{
			get
			{
				return this. chequeEndNo;
			}
			set
			{
				this. chequeEndNo = value;
			}
		}

	}
}
