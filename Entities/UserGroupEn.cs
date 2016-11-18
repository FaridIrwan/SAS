using System;
using System.Collections.Generic;
using System.Text;
using System.ServiceModel;
using System.Runtime.Serialization;

namespace HTS.SAS.Entities
{
    [System.SerializableAttribute()]
    //[DataContract]
    public class UserGroupsEn
    {
        private int ciUserGroupId;
        private string csDepartmentID;
        private string csUserGroupName;
        private bool cbStatus;
        private string csDescription;
        private string csLastUpdatedBy;
        private DateTime coLastUpdatedDtTm;


        [System.Xml.Serialization.XmlElement]
        ////[DataMember]
        public int UserGroupId
        {
            get { return ciUserGroupId; }
            set { ciUserGroupId = value; }
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
        public string UserGroupName
        {
            get { return csUserGroupName; }
            set { csUserGroupName = value; }
        }


        [System.Xml.Serialization.XmlElement]
        ////[DataMember]
        public bool Status
        {
            get { return cbStatus; }
            set { cbStatus = value; }
        }


        [System.Xml.Serialization.XmlElement]
        // //[DataMember]
        public string Description
        {
            get { return csDescription; }
            set { csDescription = value; }
        }


        [System.Xml.Serialization.XmlElement]
        // //[DataMember]
        public string LastUpdatedBy
        {
            get { return csLastUpdatedBy; }
            set { csLastUpdatedBy = value; }
        }


        [System.Xml.Serialization.XmlElement]
        ////[DataMember]
        public DateTime LastUpdatedDtTm
        {
            get { return coLastUpdatedDtTm; }
            set { coLastUpdatedDtTm = value; }
        }

    }
}
//---------------------------------------------------------------------------------

