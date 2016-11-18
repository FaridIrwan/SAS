using System;

namespace DataObjects
{
	public class SAS_CF
	{
		protected int cF_id;
		protected string cF_Code;
		protected string cF_Conn;

		public int CF_id
		{
			get
			{
				return this. cF_id;
			}
			set
			{
				this. cF_id = value;
			}
		}

		public string CF_Code
		{
			get
			{
				return this. cF_Code;
			}
			set
			{
				this. cF_Code = value;
			}
		}

		public string CF_Conn
		{
			get
			{
				return this. cF_Conn;
			}
			set
			{
				this. cF_Conn = value;
			}
		}

	}
}
