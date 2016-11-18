using System;
using System.Collections.Generic;
using System.Text;
using System.ServiceModel;
using System.Runtime.Serialization;

namespace HTS.SAS.Entities
{
    [System.SerializableAttribute()]
   // [DataContract]
    public class DegreeTypeEn
    {
        private string csSADT_Code;
        private string csSADT_Desc;
        private string csSADT_SName;
        private bool cbSADT_Status;
        private int ciSABR_Code;
        private string csSADT_UpdatedUser;
        private string csSADT_UpdatedDtTm;


        [System.Xml.Serialization.XmlElement]
     //   //[DataMember]
        public string DegreeTypeCode
        {
            get { return csSADT_Code; }
            set { csSADT_Code = value; }
        }


        [System.Xml.Serialization.XmlElement]
       // //[DataMember]
        public string Description
        {
            get { return csSADT_Desc; }
            set { csSADT_Desc = value; }
        }


        [System.Xml.Serialization.XmlElement]
        ////[DataMember]
        public string SName
        {
            get { return csSADT_SName; }
            set { csSADT_SName = value; }
        }


        [System.Xml.Serialization.XmlElement]
       // //[DataMember]
        public bool Status
        {
            get { return cbSADT_Status; }
            set { cbSADT_Status = value; }
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
        public string UpdatedUser
        {
            get { return csSADT_UpdatedUser; }
            set { csSADT_UpdatedUser = value; }
        }


        [System.Xml.Serialization.XmlElement]
        ////[DataMember]
        public string UpdatedDtTm
        {
            get { return csSADT_UpdatedDtTm; }
            set { csSADT_UpdatedDtTm = value; }
        }

    }
}
