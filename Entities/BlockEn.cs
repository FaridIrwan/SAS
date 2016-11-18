using System;
using System.Collections.Generic;
using System.Text;
using System.ServiceModel;
using System.Runtime.Serialization;

namespace HTS.SAS.Entities
{
    [System.SerializableAttribute()]
    //[DataContract]
    public class BlockEn
    {
        private string csSABK_Code;
        private string csSAKO_Code;
        private string csSABK_Description;


        [System.Xml.Serialization.XmlElement]
        //[DataMember]
        public string SABK_Code
        {
            get { return csSABK_Code; }
            set { csSABK_Code = value; }
        }


        [System.Xml.Serialization.XmlElement]
        //[DataMember]
        public string SAKO_Code
        {
            get { return csSAKO_Code; }
            set { csSAKO_Code = value; }
        }


        [System.Xml.Serialization.XmlElement]
        //[DataMember]
        public string SABK_Description
        {
            get { return csSABK_Description; }
            set { csSABK_Description = value; }
        }

    }
}
