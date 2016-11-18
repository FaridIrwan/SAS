using System;
using System.Collections.Generic;
using System.Text;
using System.ServiceModel;
using System.Runtime.Serialization;

namespace HTS.SAS.Entities
{
    [System.SerializableAttribute()]
    //[DataContract]

    public class BidangEn
    {
        private string csSABP_Code;
        private string csSABP_Desc;
        private bool cbSABP_Status;
        private string csSABP_UpdatedBy;
        private string csSABP_UpdatedDtTm;

        [System.Xml.Serialization.XmlElement]
        ////[DataMember]
        public string BidangCode
        {
            get { return csSABP_Code; }
            set { csSABP_Code = value; }
        }
        
        [System.Xml.Serialization.XmlElement]
        ////[DataMember]
        public string Description
        {
            get { return csSABP_Desc; }
            set { csSABP_Desc = value; }
        }
        
        [System.Xml.Serialization.XmlElement]
        ////[DataMember]
        public bool Status
        {
            get { return cbSABP_Status; }
            set { cbSABP_Status = value; }
        }

        [System.Xml.Serialization.XmlElement]
        ////[DataMember]
        public string UpdatedBy
        {
            get { return csSABP_UpdatedBy; }
            set { csSABP_UpdatedBy = value; }
        }
        
        [System.Xml.Serialization.XmlElement]
        ////[DataMember]
        public string UpdatedDtTm
        {
            get { return csSABP_UpdatedDtTm; }
            set { csSABP_UpdatedDtTm = value; }
        }

    }
}
