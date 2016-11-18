using System;
using System.Collections.Generic;
using System.Text;
using System.ServiceModel;
using System.Runtime.Serialization;

namespace HTS.SAS.Entities
{
    [System.SerializableAttribute()]
    //[DataContract]
    public class MenuMasterEn : UserRightsEn
    {
        private int ciMenuID;
        private string csMenuName;
        private string csPageName;
        private string csPageDescription;
        private string csPageUrl;
        private string csImageUrl;
        private bool cbStatus;
        private int ciPageOrder;
        private string csLastUpdatedBy;
        private DateTime coLastUpdatedDtTm;


        [System.Xml.Serialization.XmlElement]
        ////[DataMember]
        public int MenuID
        {
            get { return ciMenuID; }
            set { ciMenuID = value; }
        }


        [System.Xml.Serialization.XmlElement]
        ////[DataMember]
        public string MenuName
        {
            get { return csMenuName; }
            set { csMenuName = value; }
        }


        [System.Xml.Serialization.XmlElement]
        ////[DataMember]
        public string PageName
        {
            get { return csPageName; }
            set { csPageName = value; }
        }


        [System.Xml.Serialization.XmlElement]
        ////[DataMember]
        public string PageDescription
        {
            get { return csPageDescription; }
            set { csPageDescription = value; }
        }


        [System.Xml.Serialization.XmlElement]
        ////[DataMember]
        public string PageUrl
        {
            get { return csPageUrl; }
            set { csPageUrl = value; }
        }


        [System.Xml.Serialization.XmlElement]
        ////[DataMember]
        public string ImageUrl
        {
            get { return csImageUrl; }
            set { csImageUrl = value; }
        }


        [System.Xml.Serialization.XmlElement]
        ////[DataMember]
        public bool Status
        {
            get { return  cbStatus; }
            set { cbStatus = value; }
        }


        [System.Xml.Serialization.XmlElement]
        ////[DataMember]
        public int PageOrder
        {
            get { return ciPageOrder; }
            set { ciPageOrder = value; }
        }


        [System.Xml.Serialization.XmlElement]
        ////[DataMember]
        public string LastUpdatedBy
        {
            get { return csLastUpdatedBy; }
            set { csLastUpdatedBy = value; }
        }


        [System.Xml.Serialization.XmlElement]
        ////[DataMember]
        public DateTime LastUpdatedDtTm
        {
            get { return coLastUpdatedDtTm; }
            set { coLastUpdatedDtTm = value; }
        }

    }
}
//---------------------------------------------------------------------------------

