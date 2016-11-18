using System;
using System.Collections.Generic;
using System.Text;
using System.ServiceModel;
using System.Runtime.Serialization;

namespace HTS.SAS.Entities
{
    [System.SerializableAttribute()]
    //[DataContract]
    public class WorkflowEn
    {
        private int xWorkflowId;
        private string xBatchCode;
        private DateTime xDateTime;
        private int xWorkflowStatus;
        private string xUserId;
        private string xPageName;
        private string xWorkflowRemarks;
        private AccountsEn xAccountsEn;
        private UsersEn xUsersEn;
        private MenuMasterEn xMenuMasterEn;
        private WorkflowSetupEn xWorkflowSetupEn;
        private WorkflowApproverEn xWorkflowApproverEn;

            [System.Xml.Serialization.XmlElement]
            ////[DataMember]
            public int WorkflowId
            {
                get { return xWorkflowId; }
                set { xWorkflowId = value; }
            }

            [System.Xml.Serialization.XmlElement]
            ////[DataMember]
            public string BatchCode
            {
                get { return xBatchCode; }
                set { xBatchCode = value; }
            }

            [System.Xml.Serialization.XmlElement]
            ////[DataMember]
            public DateTime DateTime
            {
                get { return xDateTime; }
                set { xDateTime = value; }
            }

            [System.Xml.Serialization.XmlElement]
            ////[DataMember]
            public int WorkflowStatus
            {
                get { return xWorkflowStatus; }
                set { xWorkflowStatus = value; }
            }

            [System.Xml.Serialization.XmlElement]
            ////[DataMember]
            public string UserId
            {
                get { return xUserId; }
                set { xUserId = value; }
            }

            [System.Xml.Serialization.XmlElement]
            ////[DataMember]
            public string PageName
            {
                get { return xPageName; }
                set { xPageName = value; }
            }

            [System.Xml.Serialization.XmlElement]
            ////[DataMember]
            public string WorkflowRemarks
            {
                get { return xWorkflowRemarks; }
                set { xWorkflowRemarks = value; }
            }

            [System.Xml.Serialization.XmlElement]
            ////[DataMember]
            public AccountsEn AccountsEn
            {
                get { return xAccountsEn; }
                set { xAccountsEn = value; }
            }

            [System.Xml.Serialization.XmlElement]
            ////[DataMember]
            public UsersEn UsersEn
            {
                get { return xUsersEn; }
                set { xUsersEn = value; }
            }

            [System.Xml.Serialization.XmlElement]
            ////[DataMember]
            public MenuMasterEn MenuMasterEn
            {
                get { return xMenuMasterEn; }
                set { xMenuMasterEn = value; }
            }

            [System.Xml.Serialization.XmlElement]
            ////[DataMember]
            public WorkflowSetupEn WorkflowSetupEn
            {
                get { return xWorkflowSetupEn; }
                set { xWorkflowSetupEn = value; }
            }

            [System.Xml.Serialization.XmlElement]
            ////[DataMember]
            public WorkflowApproverEn WorkflowApproverEn
            {
                get { return xWorkflowApproverEn; }
                set { xWorkflowApproverEn = value; }
            }

           
     }
}
