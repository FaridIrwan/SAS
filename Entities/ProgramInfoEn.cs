using System;
using System.Collections.Generic;
using System.Text;
using System.ServiceModel;
using System.Runtime.Serialization;

namespace HTS.SAS.Entities
{
    [System.SerializableAttribute()]
    //[DataContract]
    public class ProgramInfoEn:FacultyEn
    {
        private string csSAPG_Code;
        private string csSAPG_Program;
        private string csSAPG_ProgramType;
        private string csSAPG_ProgramBM;
        private string csSAPG_SName;
        private string csSAPG_SNameBM;
        private int ciSAPG_TotalSem;
        private int ciSAPG_SemByYear;
        private string csSAPG_Desc;
        private string csSAFC_Code;
        private bool cbSAPG_Status;
        private string csSAPG_TI;
        private string csSAFC_TIDes;
        private string csSAPG_AD;
        private string csSAFC_ADDes;
        private string csSAPG_UpdatedBy;
        private string csSAPG_UpdatedDtTm;
        private int csnofostudents;
        private string csSASI_PgId;
        private string csSASI_Faculty;
        private string csSAPG_FieldStudy;
        private string csSAFC_Semester;
        private string csSAFC_BatchNo;
        private string csCode_Programe;
        private string csTranState;
        private string csSABP_Code;
        private string csSASI_CurSemYr;
        
        [System.Xml.Serialization.XmlElement]
      //  //[DataMember]
        public string ProgramCode
        {
            get { return csSAPG_Code; }
            set { csSAPG_Code = value; }
        }
        public int NoOfStudents
        {
            get { return csnofostudents; }
            set { csnofostudents = value; }
        }

        [System.Xml.Serialization.XmlElement]
        ////[DataMember]
        public string Program
        {
            get { return csSAPG_Program; }
            set { csSAPG_Program = value; }
        }
        
        [System.Xml.Serialization.XmlElement]
        ////[DataMember]
        public string ProgramBM
        {
            get { return csSAPG_ProgramBM; }
            set { csSAPG_ProgramBM = value; }
        }

        [System.Xml.Serialization.XmlElement]
        ////[DataMember]
        public string FieldStudy
        {
            get { return csSAPG_FieldStudy; }
            set { csSAPG_FieldStudy = value; }
        }
        
        [System.Xml.Serialization.XmlElement]
        ////[DataMember]
        public string SName
        {
            get { return csSAPG_SName; }
            set { csSAPG_SName = value; }
        }
        
        [System.Xml.Serialization.XmlElement]
        ////[DataMember]
        public string SNameBM
        {
            get { return csSAPG_SNameBM; }
            set { csSAPG_SNameBM = value; }
        }


        [System.Xml.Serialization.XmlElement]
        ////[DataMember]
        public int TotalSem
        {
            get { return ciSAPG_TotalSem; }
            set { ciSAPG_TotalSem = value; }
        }


        [System.Xml.Serialization.XmlElement]
        ////[DataMember]
        public int SemByYear
        {
            get { return ciSAPG_SemByYear; }
            set { ciSAPG_SemByYear = value; }
        }


        [System.Xml.Serialization.XmlElement]
        ////[DataMember]
        public string Description
        {
            get { return csSAPG_Desc; }
            set { csSAPG_Desc = value; }
        }


        [System.Xml.Serialization.XmlElement]
        ////[DataMember]
        public string Code
        {
            get { return csSAFC_Code; }
            set { csSAFC_Code = value; }
        }


        [System.Xml.Serialization.XmlElement]
        ////[DataMember]
        public bool Status
        {
            get { return cbSAPG_Status; }
            set { cbSAPG_Status = value; }
        }


        [System.Xml.Serialization.XmlElement]
        ////[DataMember]
        public string UpdatedBy
        {
            get { return csSAPG_UpdatedBy; }
            set { csSAPG_UpdatedBy = value; }
        }


        [System.Xml.Serialization.XmlElement]
        ////[DataMember]
        public string UpdatedDtTm
        {
            get { return csSAPG_UpdatedDtTm; }
            set { csSAPG_UpdatedDtTm = value; }
        }
        [System.Xml.Serialization.XmlElement]
        ////[DataMember]
        public string Tutionacc
        {
            get { return csSAPG_TI; }
            set { csSAPG_TI = value; }
        }
        [System.Xml.Serialization.XmlElement]
        ////[DataMember]
        public string TutionDes
        {
            get { return csSAFC_TIDes; }
            set { csSAFC_TIDes = value; }
        }
        [System.Xml.Serialization.XmlElement]
        ////[DataMember]
        public string Accountinfo
        {
            get { return csSAPG_AD; }
            set { csSAPG_AD = value; }
        }
        [System.Xml.Serialization.XmlElement]
        ////[DataMember]
        public string AccountDes
        {
            get { return csSAFC_ADDes; }
            set { csSAFC_ADDes = value; }
        }
        public string ProgramID
        {
            get { return csSASI_PgId; }
            set { csSASI_PgId = value; }
        }
        public string Faculty
        {
            get { return csSASI_Faculty; }
            set { csSASI_Faculty = value; }
        }

        public string ProgramType
        {
            get { return csSAPG_ProgramType; }
            set { csSAPG_ProgramType = value; }
        }

        public string Semester
        {
            get { return csSAFC_Semester; }
            set { csSAFC_Semester = value; }
        }

        public string BatchNo
        {
            get { return csSAFC_BatchNo; }
            set { csSAFC_BatchNo = value; }
        }

        public string CodeProgram
        {
            get { return csCode_Programe; }
            set { csCode_Programe = value; }
        }

        public string TransStatus
        {
            get { return csTranState; }
            set { csTranState = value; }
        }

        public string BidangCode
        {
            get { return csSABP_Code; }
            set { csSABP_Code = value; }
        }

        public string CurrentSemester
        {
            get { return csSASI_CurSemYr; }
            set { csSASI_CurSemYr = value; }
        }
        
    }
}
