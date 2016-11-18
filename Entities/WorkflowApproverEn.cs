using System;
using System.Collections.Generic;
using System.Text;
using System.ServiceModel;
using System.Runtime.Serialization;

namespace HTS.SAS.Entities
{
    [System.SerializableAttribute()]
    //[DataContract]
    public class WorkflowApproverEn:WorkflowSetupEn 
    {
        private int xWorkflowSetupId;
        private Int64 xUserId;
        private Int64 xMenuId;
        private string xLastUpdatedBy;
        private DateTime xLastUpdatedDtTm;
        private double xLowerLimit;
        private double xUpperLimit;

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

            [System.Xml.Serialization.XmlElement]
            ////[DataMember]
            public double LowerLimit
            {
                get { return xLowerLimit; }
                set { xLowerLimit = value; }
            }

            [System.Xml.Serialization.XmlElement]
            ////[DataMember]
            public double UpperLimit
            {
                get { return xUpperLimit; }
                set { xUpperLimit = value; }
            }
     }
}
