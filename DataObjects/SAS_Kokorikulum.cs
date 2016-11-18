using System;

namespace DataObjects
{
	public class SAS_Kokorikulum
	{
		protected int sAKO_Id;
		protected string sAKO_Code;
		protected string sAKO_Description;
		protected string sAKO_GLCode;
		protected bool sAKO_Status;
		protected int sAKO_CreditHours;

		public int SAKO_Id
		{
			get
			{
				return this. sAKO_Id;
			}
			set
			{
				this. sAKO_Id = value;
			}
		}

		public string SAKO_Code
		{
			get
			{
				return this. sAKO_Code;
			}
			set
			{
				this. sAKO_Code = value;
			}
		}

		public string SAKO_Description
		{
			get
			{
				return this. sAKO_Description;
			}
			set
			{
				this. sAKO_Description = value;
			}
		}

		public string SAKO_GLCode
		{
			get
			{
				return this. sAKO_GLCode;
			}
			set
			{
				this. sAKO_GLCode = value;
			}
		}

		public bool SAKO_Status
		{
			get
			{
				return this. sAKO_Status;
			}
			set
			{
				this. sAKO_Status = value;
			}
		}

		public int SAKO_CreditHours
		{
			get
			{
				return this. sAKO_CreditHours;
			}
			set
			{
				this. sAKO_CreditHours = value;
			}
		}

	}
}
