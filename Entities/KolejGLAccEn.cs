using System;
using System.Collections.Generic;
using System.Text;
using System.ServiceModel;
using System.Runtime.Serialization;

namespace HTS.SAS.Entities
{
    [System.SerializableAttribute()]
    //[DataContract]
    public class KolejGLAccEn
    {
        private string enSAFT_Code;
        private string enSAKO_Code;
        private string enSAKO_Description;
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
        public string SAKO_Code
        {
            get { return enSAKO_Code; }
            set { enSAKO_Code = value; }
        }

        [System.Xml.Serialization.XmlElement]
        //[DataMember]
        public string SAKO_Description
        {
            get { return enSAKO_Description; }
            set { enSAKO_Description = value; }
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

