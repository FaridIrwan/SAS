using System;
using System.Collections.Generic;
using System.Text;
using System.ServiceModel;
using System.Runtime.Serialization;

namespace HTS.SAS.Entities
{
    [System.SerializableAttribute()]
    //[DataContract]
    public class WorkflowPreparerEn:WorkflowSetupEn 
    {
        private int xWorkflowSetupId;
        private Int64 xUserId;
        private Int64 xMenuId;
        private string xLastUpdatedBy;
        private DateTime xLastUpdatedDtTm;

            [System.Xml.Serialization.XmlElement]
            ////[DataMember]
            public int WorkflowSetupId
            {
                get { return xWorkflowSetupId; }
                set { xWorkflowSetupId = value; }
            }

            [System.Xml.Serialization.XmlElement]
            ////[DataMember]
            public Int64 UserId
            {
                get { return xUserId; }
                set { xUserId = value; }
            }

            [System.Xml.Serialization.XmlElement]
            ////[DataMember]
            public Int64 MenuId
            {
                get { return xMenuId; }
                set { xMenuId = value; }
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
     }
}
