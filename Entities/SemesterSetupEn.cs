using System;
using System.Collections.Generic;
using System.Text;
using System.ServiceModel;
using System.Runtime.Serialization;

namespace HTS.SAS.Entities
{
    [System.SerializableAttribute()]
    //[DataContract]
    public class SemesterSetupEn
    {
        private string csSAST_Code;
        private string csSAST_Semester;
        private string csSAST_Description;
        private bool cbSAST_Status;
        private int ciSABR_Code;
        private string csSAST_UpdatedUser;
        private string csSAST_UpdatedDtTm;
        private string csSABP_Code;
        private bool cbSAST_IsCurrentSem;
        private string csSAST_Code2;

        [System.Xml.Serialization.XmlElement]
        ////[DataMember]
        public string SemisterSetupCode
        {
            get { return csSAST_Code; }
            set { csSAST_Code = value; }
        }


        [System.Xml.Serialization.XmlElement]
        ////[DataMember]
        public string Semester
        {
            get { return csSAST_Semester; }
            set { csSAST_Semester = value; }
        }


        [System.Xml.Serialization.XmlElement]
        ////[DataMember]
        public string Description
        {
            get { return csSAST_Description; }
            set { csSAST_Description = value; }
        }


        [System.Xml.Serialization.XmlElement]
        ////[DataMember]
        public bool Status
        {
            get { return cbSAST_Status; }
            set { cbSAST_Status = value; }
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
        public string UpdatedUser
        {
            get { return csSAST_UpdatedUser; }
            set { csSAST_UpdatedUser = value; }
        }


        [System.Xml.Serialization.XmlElement]
        ////[DataMember]
        public string UpdatedDtTm
        {
            get { return csSAST_UpdatedDtTm; }
            set { csSAST_UpdatedDtTm = value; }
        }

        public string BidangCode
        {
            get { return csSABP_Code; }
            set { csSABP_Code = value; }
        }
        
        public bool CurrSem
        {
            get { return cbSAST_IsCurrentSem; }
            set { cbSAST_IsCurrentSem = value; }
        }

        public string SemisterCode2
        {
            get { return csSAST_Code2; }
            set { csSAST_Code2 = value; }
        }
    }
}
