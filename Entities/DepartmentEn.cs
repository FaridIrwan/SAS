using System;
using System.Collections.Generic;
using System.Text;
using System.ServiceModel;
using System.Runtime.Serialization;

namespace HTS.SAS.Entities
{
    [System.SerializableAttribute()]
    //DataContract
    public class DepartmentEn
    {
        private int ciAutoID;
        private string csDepartmentID;
        private string csDepartment;
        private bool cbStatus;
        private string csCreatedBy;
        private DateTime coCreateDate;
        private string csModifiedBy;
        private DateTime coModifiedDate;
        private string csSqlCase;
        private string csSearchCriteria;

        [System.Xml.Serialization.XmlElement]
        ////[DataMember]
        public int AutoID
        {
            get { return ciAutoID; }
            set { ciAutoID = value; }
        }

        [System.Xml.Serialization.XmlElement]
        ////[DataMember]
        public string DepartmentID
        {
            get { return csDepartmentID; }
            set { csDepartmentID = value; }
        }

        [System.Xml.Serialization.XmlElement]
        ////[DataMember]
        public string Department
        {
            get { return csDepartment; }
            set { csDepartment = value; }
        }

        [System.Xml.Serialization.XmlElement]
        ////[DataMember]
        public bool Status
        {
            get { return cbStatus; }
            set { cbStatus = value; }
        }

        [System.Xml.Serialization.XmlElement]
        ////[DataMember]
        public string CreatedBy
        {
            get { return csCreatedBy; }
            set { csCreatedBy = value; }
        }

        [System.Xml.Serialization.XmlElement]
        ////[DataMember]
        public DateTime CreateDate
        {
            get { return coCreateDate; }
            set { coCreateDate = value; }
        }

        [System.Xml.Serialization.XmlElement]
        ////[DataMember]
        public string ModifiedBy
        {
            get { return csModifiedBy; }
            set { csModifiedBy = value; }
        }

        [System.Xml.Serialization.XmlElement]
        ////[DataMember]
        public DateTime ModifiedDate
        {
            get { return coModifiedDate; }
            set { coModifiedDate = value; }
        }

        [System.Xml.Serialization.XmlElement]
        ////[DataMember]
        public string SqlCase
        {
            get { return csSqlCase; }
            set { csSqlCase = value; }
        }

        [System.Xml.Serialization.XmlElement]
        ////[DataMember]
        public string SearchCriteria
        {
            get { return csSearchCriteria; }
            set { csSearchCriteria = value; }
        }

    }

    
}
