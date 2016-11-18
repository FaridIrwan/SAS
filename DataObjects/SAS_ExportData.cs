using System;

namespace DataObjects
{
	public class SAS_ExportData
	{
		protected string interfaceID;
		protected string fileFormat;
		protected string _Interface;
		protected string frequency;
		protected string timeOfExport;
		protected string filePath;
		protected bool previousData;
		protected bool dateRange;
		protected DateTime? dateFrom;
		protected DateTime? dateTo;
		protected string lastUpdatedBy;
		protected DateTime? lastUpdatedDateTime;

		public string InterfaceID
		{
			get
			{
				return this. interfaceID;
			}
			set
			{
				this. interfaceID = value;
			}
		}

		public string FileFormat
		{
			get
			{
				return this. fileFormat;
			}
			set
			{
				this. fileFormat = value;
			}
		}

		public string Interface
		{
			get
			{
				return this._Interface;
			}
			set
			{
                this._Interface = value;
			}
		}

		public string Frequency
		{
			get
			{
				return this. frequency;
			}
			set
			{
				this. frequency = value;
			}
		}

		public string TimeOfExport
		{
			get
			{
				return this. timeOfExport;
			}
			set
			{
				this. timeOfExport = value;
			}
		}

		public string FilePath
		{
			get
			{
				return this. filePath;
			}
			set
			{
				this. filePath = value;
			}
		}

		public bool PreviousData
		{
			get
			{
				return this. previousData;
			}
			set
			{
				this. previousData = value;
			}
		}

		public bool DateRange
		{
			get
			{
				return this. dateRange;
			}
			set
			{
				this. dateRange = value;
			}
		}

		public DateTime? DateFrom
		{
			get
			{
				return this. dateFrom;
			}
			set
			{
				this. dateFrom = value;
			}
		}

		public DateTime? DateTo
		{
			get
			{
				return this. dateTo;
			}
			set
			{
				this. dateTo = value;
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

		public DateTime? LastUpdatedDateTime
		{
			get
			{
				return this. lastUpdatedDateTime;
			}
			set
			{
				this. lastUpdatedDateTime = value;
			}
		}

	}
}
