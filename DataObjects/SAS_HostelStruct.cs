using System;

namespace DataObjects
{
	public class SAS_HostelStruct
	{
		protected string sAHS_Code;
		protected string sAHB_Code;
		protected string sAHB_Block;
		protected string sAHB_RoomTYpe;
		protected string sAHS_EffectFm;
		protected bool sAFS_Status;
		protected string sAHS_UpdatedUser;
		protected string sAHS_UpdatedDtTm;

		public string SAHS_Code
		{
			get
			{
				return this. sAHS_Code;
			}
			set
			{
				this. sAHS_Code = value;
			}
		}

		public string SAHB_Code
		{
			get
			{
				return this. sAHB_Code;
			}
			set
			{
				this. sAHB_Code = value;
			}
		}

		public string SAHB_Block
		{
			get
			{
				return this. sAHB_Block;
			}
			set
			{
				this. sAHB_Block = value;
			}
		}

		public string SAHB_RoomTYpe
		{
			get
			{
				return this. sAHB_RoomTYpe;
			}
			set
			{
				this. sAHB_RoomTYpe = value;
			}
		}

		public string SAHS_EffectFm
		{
			get
			{
				return this. sAHS_EffectFm;
			}
			set
			{
				this. sAHS_EffectFm = value;
			}
		}

		public bool SAFS_Status
		{
			get
			{
				return this. sAFS_Status;
			}
			set
			{
				this. sAFS_Status = value;
			}
		}

		public string SAHS_UpdatedUser
		{
			get
			{
				return this. sAHS_UpdatedUser;
			}
			set
			{
				this. sAHS_UpdatedUser = value;
			}
		}

		public string SAHS_UpdatedDtTm
		{
			get
			{
				return this. sAHS_UpdatedDtTm;
			}
			set
			{
				this. sAHS_UpdatedDtTm = value;
			}
		}

	}
}
