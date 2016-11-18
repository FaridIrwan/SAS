using System;
using System.Collections.Generic;
using System.Text;
using System.ServiceModel;
using System.Runtime.Serialization;

namespace HTS.SAS.Entities
{
    [System.SerializableAttribute()]
    //[DataContract]
    public class HostelStrDetailsEn
    {
        private string csSAHS_Code;
        private string csSAHD_Code;
        private string csSAFT_Code;
        private string csSAHD_Type;
        private int ciSAHD_Priority;
        private string csSAFT_Desc;
        private int ciSAFT_Priority;
        private int id;
        private int csSafs_Taxmode;

        private List<HostelStrAmountEn> lstHostelAmt;
        [System.Xml.Serialization.XmlElement]
        ////[DataMember]
        public string HSCode
        {
            get { return csSAHS_Code; }
            set { csSAHS_Code = value; }
        }

        [System.Xml.Serialization.XmlElement]
        ////[DataMember]
        public string HDCode
        {
            get { return csSAHD_Code; }
            set { csSAHD_Code = value; }
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
        public string HDType
        {
            get { return csSAHD_Type; }
            set { csSAHD_Type = value; }
        }

        [System.Xml.Serialization.XmlElement]
        ////[DataMember]
        public int HDPriority
        {
            get { return ciSAHD_Priority; }
            set { ciSAHD_Priority = value; }
        }

        [System.Xml.Serialization.XmlElement]
        ////[DataMember]
        public string Description
        {
            get { return csSAFT_Desc; }
            set { csSAFT_Desc = value; }
        }

        [System.Xml.Serialization.XmlElement]
        ////[DataMember]
        public int Priority
        {
            get { return ciSAFT_Priority; }
            set { ciSAFT_Priority = value; }
        }

        [System.Xml.Serialization.XmlElement]
        ////[DataMember]
        public int ObjectID
        {
            get { return id; }
            set { id = value; }
        }
        public List<HostelStrAmountEn> ListFeeAmount
        {
            get { return lstHostelAmt; }
            set { lstHostelAmt = value; }
        }

        [System.Xml.Serialization.XmlElement]
        public int TaxId
        {
            get { return csSafs_Taxmode; }
            set { csSafs_Taxmode = value; }
        }

    }
}
