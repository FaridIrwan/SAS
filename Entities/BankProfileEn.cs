using System;
using System.Collections.Generic;
using System.Text;
using System.ServiceModel;
using System.Runtime.Serialization;

namespace HTS.SAS.Entities
{
    [System.SerializableAttribute()]
    //[DataContract]
    public class BankProfileEn
    {
        private string csSABD_Code;
        private string csSABD_Desc;
        private string csSABD_ACCode;
        private string csSABD_GLCode;
        private bool cbSABD_Status;
        private int ciSABR_Code;
        private string csSABD_UpdatedBy;
        private string csSABD_UpdatedDtTm;


        [System.Xml.Serialization.XmlElement]
        ////[DataMember]
        public string BankDetailsCode
        {
            get { return csSABD_Code; }
            set { csSABD_Code = value; }
        }


        [System.Xml.Serialization.XmlElement]
        ////[DataMember]
        public string Description
        {
            get { return csSABD_Desc.Replace("'", "~"); }
            set { csSABD_Desc = value ; }
        }


        [System.Xml.Serialization.XmlElement]
        ////[DataMember]
        public string ACCode
        {
            get { return csSABD_ACCode; }
            set { csSABD_ACCode = value; }
        }


        [System.Xml.Serialization.XmlElement]
        ////[DataMember]
        public string GLCode
        {
            get { return csSABD_GLCode; }
            set { csSABD_GLCode = value; }
        }


        [System.Xml.Serialization.XmlElement]
        ////[DataMember]
        public bool Status
        {
            get { return cbSABD_Status; }
            set { cbSABD_Status = value; }
        }


        [System.Xml.Serialization.XmlElement]
        ////[DataMember]
        public int Code
        {
            get { return ciSABR_Code; }
            set { ciSABR_Code = value; }
        }


        [System.Xml.Serialization.XmlElement]
        ////[DataMember]
        public string UpdatedBy
        {
            get { return csSABD_UpdatedBy; }
            set { csSABD_UpdatedBy = value; }
        }


        [System.Xml.Serialization.XmlElement]
        ////[DataMember]
        public string UpdatedDtTm
        {
            get { return csSABD_UpdatedDtTm; }
            set { csSABD_UpdatedDtTm = value; }
        }

    }
}
