using System;
using System.Collections.Generic;
using System.Text;
using System.ServiceModel;
using System.Runtime.Serialization;

namespace HTS.SAS.Entities
{
    [System.SerializableAttribute()]
    //[DataContract]
    public class RoomTypeEn
    {
        private string csSART_Code;
        private string csSABK_Code;
        private string csSAKO_Code;
        private string csSART_Description;


        [System.Xml.Serialization.XmlElement]
        //[DataMember]
        public string SART_Code
        {
            get { return csSART_Code; }
            set { csSART_Code = value; }
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
        public string SABK_Code
        {
            get { return csSABK_Code; }
            set { csSABK_Code = value; }
        }


        [System.Xml.Serialization.XmlElement]
        //[DataMember]
        public string SART_Description
        {
            get { return csSART_Description; }
            set { csSART_Description = value; }
        }

    }
}
