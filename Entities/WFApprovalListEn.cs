using System;
using System.Collections.Generic;
using System.Text;
using System.ServiceModel;
using System.Runtime.Serialization;

namespace HTS.SAS.Entities
{
    [System.SerializableAttribute()]
    //[DataContract]
    public class WFApprovalListEn
    {
        private string xBatchcode;
        private string xPagename;
        private string xUsername;
        private string xUsergroupName;
        private WorkflowSetupEn xWorkflowSetupEn;
        private WorkflowApproverEn xWorkflowApproverEn;
        private MenuMasterEn xMenuMasterEn;
        private UsersEn xUsersEn;

            [System.Xml.Serialization.XmlElement]
            ////[DataMember]
            public string Batchcode
            {
                get { return xBatchcode; }
                set { xBatchcode = value; }
            }

            [System.Xml.Serialization.XmlElement]
            ////[DataMember]
            public string Pagename
            {
                get { return xPagename; }
                set { xPagename = value; }
            }

            [System.Xml.Serialization.XmlElement]
            ////[DataMember]
            public string Username
            {
                get { return xUsername; }
                set { xUsername = value; }
            }

            [System.Xml.Serialization.XmlElement]
            ////[DataMember]
            public string UsergroupName
            {
                get { return xUsergroupName; }
                set { xUsergroupName = value; }
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

            [System.Xml.Serialization.XmlElement]
            ////[DataMember]
            public MenuMasterEn MenuMasterEn
            {
                get { return xMenuMasterEn; }
                set { xMenuMasterEn = value; }
            }

            [System.Xml.Serialization.XmlElement]
            ////[DataMember]
            public UsersEn UsersEn
            {
                get { return xUsersEn; }
                set { xUsersEn = value; }
            }
     }
}
