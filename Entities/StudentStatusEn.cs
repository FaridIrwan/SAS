using System;
using System.Collections.Generic;
using System.Text;
using System.ServiceModel;
using System.Runtime.Serialization;

namespace HTS.SAS.Entities
{
    [System.SerializableAttribute()]
    //[DataContract]
    public class StudentStatusEn
    {
        private string csSASS_Code;
        private string csSASS_Description;
        private bool cbSASS_BlStatus;
        private int ciSABR_Code;
        private bool cbSASS_Status;
        private string csSASS_UpdatedUser;
        private string csSASS_UpdatedDtTm;
        private List<StudentCategoryAccessEn> cslstStuCatAccess;


        [System.Xml.Serialization.XmlElement]
        ////[DataMember]
        public string StudentStatusCode
        {
            get { return csSASS_Code; }
            set { csSASS_Code = value; }
        }


        [System.Xml.Serialization.XmlElement]
        ////[DataMember]
        public string Description
        {
            get { return csSASS_Description; }
            set { csSASS_Description = value; }
        }


        [System.Xml.Serialization.XmlElement]
        ////[DataMember]
        public bool BlStatus
        {
            get { return cbSASS_BlStatus; }
            set { cbSASS_BlStatus = value; }
        }


        [System.Xml.Serialization.XmlElement]
        ////[DataMember]
        public int Code
        {
            get { return ciSABR_Code; }
            set { ciSABR_Code = value; }
        }


        [System.Xml.Serialization.XmlElement]
        ////[DataMember]
        public bool Status
        {
            get { return cbSASS_Status; }
            set { cbSASS_Status = value; }
        }


        [System.Xml.Serialization.XmlElement]
        ////[DataMember]
        public string UpdatedUser
        {
            get { return csSASS_UpdatedUser; }
            set { csSASS_UpdatedUser = value; }
        }


        [System.Xml.Serialization.XmlElement]
        ////[DataMember]
        public string UpdatedDtTm
        {
            get { return csSASS_UpdatedDtTm; }
            set { csSASS_UpdatedDtTm = value; }
        }

        [System.Xml.Serialization.XmlElement]
        ////[DataMember]
        public List<StudentCategoryAccessEn> LstStudentCategoryAccess
        {
            get { return cslstStuCatAccess; }
            set { cslstStuCatAccess = value; }
        }

    }
}
