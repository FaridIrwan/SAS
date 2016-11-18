using System;
using System.Collections.Generic;
using System.Text;
using System.ServiceModel;
using System.Runtime.Serialization;

namespace HTS.SAS.Entities
{
    [System.SerializableAttribute()]
    //[DataContract]
    public class UniversityFundEn
    {
        private string csSAUF_Code;
        private string csSAUF_Desc;
        private string csSAUF_GLCode;
        private int ciSABR_Code;
        private bool cbSAUF_Status;
        private string csSASUF_UpdatedBy;
        private string csSASUF_UpdatedDtTm;


        [System.Xml.Serialization.XmlElement]
        ////[DataMember]
        public string UniversityFundCode
        {
            get { return csSAUF_Code; }
            set { csSAUF_Code = value; }
        }


        [System.Xml.Serialization.XmlElement]
        ////[DataMember]
        public string Description
        {
            get { return csSAUF_Desc; }
            set { csSAUF_Desc = value; }
        }


        [System.Xml.Serialization.XmlElement]
        ////[DataMember]
        public string GLCode
        {
            get { return csSAUF_GLCode; }
            set { csSAUF_GLCode = value; }
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
        public bool Status
        {
            get { return cbSAUF_Status; }
            set { cbSAUF_Status = value; }
        }


        [System.Xml.Serialization.XmlElement]
        ////[DataMember]
        public string UpdatedBy
        {
            get { return csSASUF_UpdatedBy; }
            set { csSASUF_UpdatedBy = value; }
        }


        [System.Xml.Serialization.XmlElement]
        ////[DataMember]
        public string UpdatedDtTm
        {
            get { return csSASUF_UpdatedDtTm; }
            set { csSASUF_UpdatedDtTm = value; }
        }

    }
}
