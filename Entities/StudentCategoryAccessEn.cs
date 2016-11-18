using System;
using System.Collections.Generic;
using System.Text;
using System.ServiceModel;
using System.Runtime.Serialization;

namespace HTS.SAS.Entities
{
    [System.SerializableAttribute()]
    //[DataContract]
    public class StudentCategoryAccessEn
    {
        private string csSASC_Code;
        private string csMenuName;
        private string csPageName;
        private int ciMenuID;
        private bool cbStatus;


        [System.Xml.Serialization.XmlElement]
        ////[DataMember]
        public string StudentCategoryCode
        {
            get { return csSASC_Code; }
            set { csSASC_Code = value; }
        }


        [System.Xml.Serialization.XmlElement]
        ////[DataMember]
        public int MenuID
        {
            get { return ciMenuID; }
            set { ciMenuID = value; }
        }


        [System.Xml.Serialization.XmlElement]
        ////[DataMember]
        public bool Status
        {
            get { return cbStatus; }
            set { cbStatus = value; }
        }

        [System.Xml.Serialization.XmlElement]
        // //[DataMember]
        public string MenuName
        {
            get { return csMenuName; }
            set { csMenuName = value; }
        }

        [System.Xml.Serialization.XmlElement]
        // //[DataMember]
        public string PageName
        {
            get { return csPageName; }
            set { csPageName = value; }
        }


    }
}
