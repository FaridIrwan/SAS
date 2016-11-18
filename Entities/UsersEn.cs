using System;
using System.Collections.Generic;
using System.Text;
using System.ServiceModel;
using System.Runtime.Serialization;

namespace HTS.SAS.Entities
{
    [System.SerializableAttribute()]
    //[DataContract]
    public class UsersEn:UserGroupsEn
    {
        private int ciUserID;
        private string csUserName;
        private string csPassword;
        private int ciUserGroupId;
        private string csUserStatus;
        private bool cbRecStatus;
        private string csEmail;
        private string csLastUpdatedBy;
        private DateTime coLastUpdatedDtTm;
        private string csSearchCriteria;
        private string csDepartment;
        private int ciApprovalGroup;
        private string ccApproval;
        private string csDescription;
        //Added Mona @3/8/2016
        private string csStafId;
        private string csStaffName;
        private string csJobRole;
        private DateTime coStaffExpiryDtTm;
        //Added Mona @5/8/2016
        private int ciMenuId;
        
        [System.Xml.Serialization.XmlElement]
        ////[DataMember]
        public int UserID
        {
            get { return ciUserID; }
            set { ciUserID = value; }
        }

        [System.Xml.Serialization.XmlElement]
        ////[DataMember]
        public string UserName
        {
            get { return csUserName; }
            set { csUserName = value; }
        }

        [System.Xml.Serialization.XmlElement]
        ////[DataMember]
        public string Password
        {
            get { return csPassword; }
            set { csPassword = value; }
        }

        [System.Xml.Serialization.XmlElement]
        ////[DataMember]
        public int UserGroupId
        {
            get { return ciUserGroupId; }
            set { ciUserGroupId = value; }
        }

        [System.Xml.Serialization.XmlElement]
        ////[DataMember]
        public string UserStatus
        {
            get { return csUserStatus; }
            set { csUserStatus = value; }
        }

        [System.Xml.Serialization.XmlElement]
        ////[DataMember]
        public bool RecStatus
        {
            get { return cbRecStatus; }
            set { cbRecStatus = value; }
        }

        [System.Xml.Serialization.XmlElement]
        ////[DataMember]
        public string Email
        {
            get { return csEmail; }
            set { csEmail = value; }
        }

        [System.Xml.Serialization.XmlElement]
        ////[DataMember]
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

        [System.Xml.Serialization.XmlElement]
        ////[DataMember]
        public string SearchCriteria
        {
            get { return csSearchCriteria; }
            set {csSearchCriteria = value ; }
        }

        [System.Xml.Serialization.XmlElement]
        ////[DataMember]
        public string Department
        {
            get { return csDepartment; }
            set { csDepartment = value; }
        }
        [System.Xml.Serialization.XmlElement]
        ////DataMember]
        public int WorkflowGroup
        {
            get { return ciApprovalGroup; }
            set { ciApprovalGroup = value; }
        }
        [System.Xml.Serialization.XmlElement]
        public string WorkflowRole
        {
            get { return ccApproval; }
            set { ccApproval = value; }
        }

        [System.Xml.Serialization.XmlElement]
        public string Description
        {
            get { return csDescription; }
            set { csDescription = value; }
        }
       
        [System.Xml.Serialization.XmlElement]
        ////[DataMember]
        public string StaffID
        {
            get { return csStafId; }
            set { csStafId = value; }
        }

        [System.Xml.Serialization.XmlElement]
        ////[DataMember]
        public string StaffName
        {
            get { return csStaffName; }
            set { csStaffName = value; }
        }

        [System.Xml.Serialization.XmlElement]
        ////[DataMember]
        public string JobTitle
        {
            get { return csJobRole; }
            set { csJobRole = value; }
        }
        
        [System.Xml.Serialization.XmlElement]
        ////[DataMember]
        public DateTime StaffExpiryDtTm
        {
            get { return coStaffExpiryDtTm; }
            set { coStaffExpiryDtTm = value; }
        }

        [System.Xml.Serialization.XmlElement]
        ////[DataMember]
        public int MenuID
        {
            get { return ciMenuId; }
            set { ciMenuId = value; }
        }
    }
}
