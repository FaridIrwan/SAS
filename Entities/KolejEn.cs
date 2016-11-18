using System;
using System.Collections.Generic;
using System.Text;
using System.ServiceModel;
using System.Runtime.Serialization;

namespace HTS.SAS.Entities
{
    [System.SerializableAttribute()]
    //[DataContract]
    public class KolejEn
    {
        private string csSAKO_Code;
        private string csSAKO_Description;


        [System.Xml.Serialization.XmlElement]
        //[DataMember]
        public string SAKO_Code
        {
            get { return csSAKO_Code; }
            set { csSAKO_Code = value; }
        }


        [System.Xml.Serialization.XmlElement]
        //[DataMember]
        public string SAKO_Description
        {
            get { return csSAKO_Description; }
            set { csSAKO_Description = value; }
        }

    }
}
