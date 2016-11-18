using System;
using System.Collections.Generic;
using System.Text;
using System.ServiceModel;
using System.Runtime.Serialization;

namespace HTS.SAS.Entities
{
    [System.SerializableAttribute()]
    //[DataContract]
    public class WorkflowSetupEn
    {
        private int xId;
        private Int64 xMenuId;
        private Int64 xTotalPreparer;
        private Int64 xTotalApprover;
        private bool xStatus;
        private string xLastUpdatedBy;
        private DateTime xLastUpdatedDtTm;
        private UsersEn xUsersEn;
        private UserGroupsEn xUserGroupsEn;
        private MenuMasterEn xMenuMasterEn;
        private WorkflowApproverEn xWorkflowApproverEn;
        private WorkflowPreparerEn xWorkflowPreparerEn;

            [System.Xml.Serialization.XmlElement]
            ////[DataMember]
            public int Id
            {
                get { return xId; }
                set { xId = value; }
            }

            [System.Xml.Serialization.XmlElement]
            ////[DataMember]
            public Int64 MenuId
            {
                get { return xMenuId; }
                set { xMenuId = value; }
            }
            
            [System.Xml.Serialization.XmlElement]
            ////[DataMember]
            public Int64 TotalPreparer
            {
                get { return xTotalPreparer; }
                set { xTotalPreparer = value; }
            }

            [System.Xml.Serialization.XmlElement]
            ////[DataMember]
            public Int64 TotalApprover
            {
                get { return xTotalApprover; }
                set { xTotalApprover = value; }
            }

            [System.Xml.Serialization.XmlElement]
            ////[DataMember]
            public bool Status
            {
                get { return xStatus; }
                set { xStatus = value; }
            }

            [System.Xml.Serialization.XmlElement]
            // //[DataMember]
            public string LastUpdatedBy
            {
                get { return xLastUpdatedBy; }
                set { xLastUpdatedBy = value; }
            }

            [System.Xml.Serialization.XmlElement]
            ////[DataMember]
            public DateTime LastUpdatedDtTm
            {
                get { return xLastUpdatedDtTm; }
                set { xLastUpdatedDtTm = value; }
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
            public UserGroupsEn UserGroupsEn
            {
                get { return xUserGroupsEn; }
                set { xUserGroupsEn = value; }
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
            public WorkflowApproverEn WorkflowApproverEn
            {
                get { return xWorkflowApproverEn; }
                set { xWorkflowApproverEn = value; }
            }

            [System.Xml.Serialization.XmlElement]
            ////[DataMember]
            public WorkflowPreparerEn WorkflowPreparerEn
            {
                get { return xWorkflowPreparerEn; }
                set { xWorkflowPreparerEn = value; }
            }
     }
}
