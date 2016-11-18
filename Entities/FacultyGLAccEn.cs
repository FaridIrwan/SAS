using System;
using System.Collections.Generic;
using System.Text;
using System.ServiceModel;
using System.Runtime.Serialization;

namespace HTS.SAS.Entities
{
    [System.SerializableAttribute()]
    //[DataContract]
    public class FacultyGLAccEn
    {
        private string enSAFT_Code;
        private string enSAFC_Code;
        private string enSAFC_Desc;
        private string enGL_Account;

        [System.Xml.Serialization.XmlElement]
        //[DataMember]
        public string SAFT_Code
        {
            get { return enSAFT_Code; }
            set { enSAFT_Code = value; }
        }

        [System.Xml.Serialization.XmlElement]
        //[DataMember]
        public string SAFC_Code
        {
            get { return enSAFC_Code; }
            set { enSAFC_Code = value; }
        }

        [System.Xml.Serialization.XmlElement]
        //[DataMember]
        public string SAFC_Desc
        {
            get { return enSAFC_Desc; }
            set { enSAFC_Desc = value; }
        }

        [System.Xml.Serialization.XmlElement]
        //[DataMember]
        public string GL_Account
        {
            get { return enGL_Account; }
            set { enGL_Account = value; }
        }
    }
}

