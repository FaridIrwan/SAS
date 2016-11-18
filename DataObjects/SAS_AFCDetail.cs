using System;

namespace DataObjects
{
	public class SAS_AFCDetail
	{
		protected int transCode;
		protected string studentsNo;
		protected string programCode;
		protected float transAmount;

		public int TransCode
		{
			get
			{
				return this. transCode;
			}
			set
			{
				this. transCode = value;
			}
		}

		public string StudentsNo
		{
			get
			{
				return this. studentsNo;
			}
			set
			{
				this. studentsNo = value;
			}
		}

		public string ProgramCode
		{
			get
			{
				return this. programCode;
			}
			set
			{
				this. programCode = value;
			}
		}

		public float TransAmount
		{
			get
			{
				return this. transAmount;
			}
			set
			{
				this. transAmount = value;
			}
		}

	}
}
