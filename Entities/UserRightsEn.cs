using System;
using System.Collections.Generic;
using System.Text;
using System.ServiceModel;
using System.Runtime.Serialization;

namespace HTS.SAS.Entities
{
    [System.SerializableAttribute()]
   // [DataContract]
    public class UserRightsEn
    {
        private int ciUserGroupId;
        private int ciMenuID;
        private bool cbIsAdd;
        private bool cbIsEdit;
        private bool cbIsDelete;
        private bool cbIsView;
        private bool cbIsPrint;
        private bool cbIsPost;
        private bool cbIsOthers;
        private bool cbDefaultMode;
        private string csLastUpdatedBy;
        private DateTime coLastUpdatedDtTm;
        private string csMenuName;
        private string csPageName;
        private int ciUserId;

        [System.Xml.Serialization.XmlElement]
        ////[DataMember]
        public string PageName
        {
            get { return csPageName; }
            set { csPageName = value; }
        }
        [System.Xml.Serialization.XmlElement]
        ////[DataMember]
        public string MenuName
        {
            get { return csMenuName; }
            set { csMenuName = value; }
        }

        [System.Xml.Serialization.XmlElement]
     //   //[DataMember]
        public int UserGroup
        {
            get { return ciUserGroupId; }
            set { ciUserGroupId = value; }
        }
        
        [System.Xml.Serialization.XmlElement]
       // //[DataMember]
        public int MenuID
        {
            get { return ciMenuID; }
            set { ciMenuID = value; }
        }


        [System.Xml.Serialization.XmlElement]
       // //[DataMember]
        public bool IsAdd
        {
            get { return cbIsAdd; }
            set { cbIsAdd = value; }
        }


        [System.Xml.Serialization.XmlElement]
       // //[DataMember]
        public bool IsEdit
        {
            get { return cbIsEdit; }
            set { cbIsEdit = value; }
        }


        [System.Xml.Serialization.XmlElement]
       // //[DataMember]
        public bool IsDelete
        {
            get { return cbIsDelete; }
            set { cbIsDelete = value; }
        }


        [System.Xml.Serialization.XmlElement]
       // //[DataMember]
        public bool IsView
        {
            get { return cbIsView; }
            set { cbIsView = value; }
        }


        [System.Xml.Serialization.XmlElement]
        ////[DataMember]
        public bool IsPrint
        {
            get { return cbIsPrint; }
            set { cbIsPrint = value; }
        }


        [System.Xml.Serialization.XmlElement]
        ////[DataMember]
        public bool IsPost
        {
            get { return cbIsPost; }
            set { cbIsPost = value; }
        }


        [System.Xml.Serialization.XmlElement]
       // //[DataMember]
        public bool IsOthers
        {
            get { return cbIsOthers; }
            set { cbIsOthers = value; }
        }


        [System.Xml.Serialization.XmlElement]
        ////[DataMember]
        public bool IsAddModeDefault
        {
            get { return cbDefaultMode; }
            set { cbDefaultMode = value; }
        }



        [System.Xml.Serialization.XmlElement]
        ////[DataMember]
        public string LastUser
        {
            get { return csLastUpdatedBy; }
            set { csLastUpdatedBy = value; }
        }
        
        [System.Xml.Serialization.XmlElement]
        ////[DataMember]
        public DateTime LastDtTm
        {
            get { return coLastUpdatedDtTm; }
            set { coLastUpdatedDtTm = value; }
        }

        [System.Xml.Serialization.XmlElement]
        //   //[DataMember]
        public int UserID
        {
            get { return ciUserId; }
            set { ciUserId = value; }
        }

    }
}
//---------------------------------------------------------------------------------

