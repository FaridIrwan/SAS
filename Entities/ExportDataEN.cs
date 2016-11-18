using System;
using System.Collections.Generic;
using System.Text;
using System.ServiceModel;
using System.Runtime.Serialization;

namespace HTS.SAS.Entities
{
    [System.SerializableAttribute()]

    public class ExportDataEN
    {
        private string csInterfaceID;
        private string csFileFormat;
        private string csInterface;
        private string csFrequency;
        private string csTimeofExport;
        private string csFilepath;
        private bool cbPreviousData;
        private bool cbDateRange;
        private DateTime coDateFrom;
        private DateTime coDateTo;
        private string csLastUpdatedBy;
        private DateTime coLastUpdatedDateTime;


        [System.Xml.Serialization.XmlElement]

        public string InterfaceID
        {
            get { return csInterfaceID; }
            set { csInterfaceID = value; }
        }


        [System.Xml.Serialization.XmlElement]

        public string FileFormat
        {
            get { return csFileFormat; }
            set { csFileFormat = value; }
        }


        [System.Xml.Serialization.XmlElement]

        public string Interface
        {
            get { return csInterface; }
            set { csInterface = value; }
        }


        [System.Xml.Serialization.XmlElement]

        public string Frequency
        {
            get { return csFrequency; }
            set { csFrequency = value; }
        }


        [System.Xml.Serialization.XmlElement]

        public string TimeofExport
        {
            get { return csTimeofExport; }
            set { csTimeofExport = value; }
        }


        [System.Xml.Serialization.XmlElement]

        public string Filepath
        {
            get { return csFilepath; }
            set { csFilepath = value; }
        }


        [System.Xml.Serialization.XmlElement]

        public bool PreviousData
        {
            get { return cbPreviousData; }
            set { cbPreviousData = value; }
        }

        [System.Xml.Serialization.XmlElement]

        public bool DateRange
        {
            get { return cbDateRange; }
            set { cbDateRange = value; }
        }


        [System.Xml.Serialization.XmlElement]

        public DateTime DateFrom
        {
            get { return coDateFrom; }
            set { coDateFrom = value; }
        }


        [System.Xml.Serialization.XmlElement]

        public DateTime DateTo
        {
            get { return coDateTo; }
            set { coDateTo = value; }
        }


        [System.Xml.Serialization.XmlElement]

        public string LastUpdatedBy
        {
            get { return csLastUpdatedBy; }
            set { csLastUpdatedBy = value; }
        }


        [System.Xml.Serialization.XmlElement]

        public DateTime LastUpdatedDateTime
        {
            get { return coLastUpdatedDateTime; }
            set { coLastUpdatedDateTime = value; }
        }

    }
}
//---------------------------------------------------------------------------------

