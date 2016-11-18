using System;
using System.Collections.Generic;
using System.Text;
using System.ServiceModel;
using System.Runtime.Serialization;

namespace HTS.SAS.Entities
{
    [System.SerializableAttribute()]
    //[DataContract]
    public class SelectionCriteriaEn
    {
        private string csSAFC_Code;
        private string csSAPG_Code; 
        private string csSASR_Code;
        private string csSAKO_Code;
        private string csBatchCode;
        private string csSASC_code;
        private string csSem;

        [System.Xml.Serialization.XmlElement]
        //[DataMember]
            ///Faculty code
        public string SAFC_Code 
        {
            get { return csSAFC_Code; }
            set { csSAFC_Code = value; }
        }


        [System.Xml.Serialization.XmlElement]
        //[DataMember]
            ///Program code
        public string SAPG_Code
        {
            get { return csSAPG_Code; }
            set { csSAPG_Code = value; }
        }


        [System.Xml.Serialization.XmlElement]
        //[DataMember]
            ///Sponsor code
        public string SASR_Code
        {
            get { return csSASR_Code; }
            set { csSASR_Code = value; }
        }


        [System.Xml.Serialization.XmlElement]
        //[DataMember]
            ///Kolej code
        public string SAKO_Code
        {
            get { return csSAKO_Code; }
            set { csSAKO_Code = value; }
        }

        [System.Xml.Serialization.XmlElement]
        //[DataMember]
        ///Kolej code
        public string BatchCode
        {
            get { return csBatchCode; }
            set { csBatchCode = value; }
        }

        [System.Xml.Serialization.XmlElement]
        //[DataMember]
        ///Student category code
        public string SASC_Code
        {
            get { return csSASC_code; }
            set { csSASC_code = value; }
        }

        [System.Xml.Serialization.XmlElement]
        //[DataMember]
        ///Semester 
        public string Sem
        {
            get { return csSem; }
            set { csSem = value; }
        }
    }
}
//---------------------------------------------------------------------------------

