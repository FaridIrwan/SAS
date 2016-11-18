using System;
using System.Collections.Generic;
using System.Text;
using System.ServiceModel;
using System.Runtime.Serialization;

namespace HTS.SAS.Entities
{
    [System.SerializableAttribute()]
    //[DataContract]
    public class StudentCategoryEn:FeeChargesEn
    {
        private string csSASC_Code;
        private string csSASC_Desc;
        private int ciSABR_Code;
        private bool cbSASC_Status;
        private string csSASC_UpdatedBy;
        private string csSASC_UpdatedDtTm;
        private List<StudentCategoryAccessEn> cslstStuCatAccess;


        [System.Xml.Serialization.XmlElement]
        ////[DataMember]
        public string StudentCategoryCode
        {
            get { return csSASC_Code; }
            set { csSASC_Code = value; }
        }


        [System.Xml.Serialization.XmlElement]
        ////[DataMember]
        public string Description
        {
            get { return csSASC_Desc; }
            set { csSASC_Desc = value; }
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
            get { return cbSASC_Status; }
            set { cbSASC_Status = value; }
        }


        [System.Xml.Serialization.XmlElement]
        ////[DataMember]
        public string UpdatedBy
        {
            get { return csSASC_UpdatedBy; }
            set { csSASC_UpdatedBy = value; }
        }


        [System.Xml.Serialization.XmlElement]
        ////[DataMember]
        public string UpdatedDtTm
        {
            get { return csSASC_UpdatedDtTm; }
            set { csSASC_UpdatedDtTm = value; }
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

