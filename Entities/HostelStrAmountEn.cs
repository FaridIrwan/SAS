using System;
using System.Collections.Generic;
using System.Text;
using System.ServiceModel;
using System.Runtime.Serialization;

namespace HTS.SAS.Entities
{
    [System.SerializableAttribute()]
    //[DataContract]
    public class HostelStrAmountEn
    {
        private string csSAHS_Code;
        private string csSAFT_Code;
        private string csSASC_Code;
        private double cdSAHA_Amount;
        private string csSASC_Desc;
        private string csSAFT_Desc;
        private double csSAFA_Gstamount;

        [System.Xml.Serialization.XmlElement]
        ////[DataMember]
        public string HSCode
        {
            get { return csSAHS_Code; }
            set { csSAHS_Code = value; }
        }

        [System.Xml.Serialization.XmlElement]
        ////[DataMember]
        public string SCDesc
        {
            get { return csSASC_Desc; }
            set { csSASC_Desc = value; }
        }

        [System.Xml.Serialization.XmlElement]
        ////[DataMember]
        public string FTCode
        {
            get { return csSAFT_Code; }
            set { csSAFT_Code = value; }
        }


        [System.Xml.Serialization.XmlElement]
        ////[DataMember]
        public string SCCode
        {
            get { return csSASC_Code; }
            set { csSASC_Code = value; }
        }


        [System.Xml.Serialization.XmlElement]
        ////[DataMember]
        public double HAAmount
        {
            get { return cdSAHA_Amount; }
            set { cdSAHA_Amount = value; }
        }
        [System.Xml.Serialization.XmlElement]
        ////[DataMember]
        public string Description
        {
            get { return csSAFT_Desc; }
            set { csSAFT_Desc = value; }
        }

        [System.Xml.Serialization.XmlElement]
        public double GstAmount
        {
            get { return csSAFA_Gstamount; }
            set { csSAFA_Gstamount = value; }
        }

    }
}
