using System;
using System.Collections.Generic;
using System.Text;
using System.ServiceModel;
using System.Runtime.Serialization;

namespace HTS.SAS.Entities
{
    [System.SerializableAttribute()]
    //[DataContract]
    public class AutoNumberEn
    {
        private int ciSAAN_Code;
        private string csSAAN_Des;
        private string csSAAN_Prefix;
        private int ciSAAN_NoDigit;
        private int ciSAAN_StartNo;
        private int ciSAAN_CurNo;
        private string csSAAN_AutoNo;


        [System.Xml.Serialization.XmlElement]
        ////[DataMember]
        public int SAAN_Code
        {
            get { return ciSAAN_Code; }
            set { ciSAAN_Code = value; }
        }


        [System.Xml.Serialization.XmlElement]
        ////[DataMember]
        public string SAAN_Des
        {
            get { return csSAAN_Des; }
            set { csSAAN_Des = value; }
        }


        [System.Xml.Serialization.XmlElement]
        ////[DataMember]
        public string SAAN_Prefix
        {
            get { return csSAAN_Prefix; }
            set { csSAAN_Prefix = value; }
        }


        [System.Xml.Serialization.XmlElement]
        ////[DataMember]
        public int SAAN_NoDigit
        {
            get { return ciSAAN_NoDigit; }
            set { ciSAAN_NoDigit = value; }
        }


        [System.Xml.Serialization.XmlElement]
        ////[DataMember]
        public int SAAN_StartNo
        {
            get { return ciSAAN_StartNo; }
            set { ciSAAN_StartNo = value; }
        }


        [System.Xml.Serialization.XmlElement]
        ////[DataMember]
        public int SAAN_CurNo
        {
            get { return ciSAAN_CurNo; }
            set { ciSAAN_CurNo = value; }
        }


        [System.Xml.Serialization.XmlElement]
        ////[DataMember]
        public string SAAN_AutoNo
        {
            get { return csSAAN_AutoNo; }
            set { csSAAN_AutoNo = value; }
        }

    }
}


