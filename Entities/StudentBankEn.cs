using System;
using System.Collections.Generic;
using System.Text;
using System.ServiceModel;
using System.Runtime.Serialization;

namespace HTS.SAS.Entities
{
    [System.SerializableAttribute()]
    //[DataContract]
    public class StudentBankEn
    {
        private string csSASB_Code;
        private string csSASB_Desc;
        private bool cbSASB_Status;
        private string csSASB_UpdatedBy;
        private string csSASB_UpdatedDtTm;

        [System.Xml.Serialization.XmlElement]
        ////[DataMember]
        public string StudentBankCode
        {
            get { return csSASB_Code; }
            set { csSASB_Code = value; }
        }


        [System.Xml.Serialization.XmlElement]
        ////[DataMember]
        public string Description
        {
            get { return csSASB_Desc; }
            set { csSASB_Desc = value; }
        }

        [System.Xml.Serialization.XmlElement]
        ////[DataMember]
        public bool Status
        {
            get { return cbSASB_Status; }
            set { cbSASB_Status = value; }
        }


        [System.Xml.Serialization.XmlElement]
        ////[DataMember]
        public string UpdatedBy
        {
            get { return csSASB_UpdatedBy; }
            set { csSASB_UpdatedBy = value; }
        }


        [System.Xml.Serialization.XmlElement]
        ////[DataMember]
        public string UpdatedDtTm
        {
            get { return csSASB_UpdatedDtTm; }
            set { csSASB_UpdatedDtTm = value; }
        }

    }
}
