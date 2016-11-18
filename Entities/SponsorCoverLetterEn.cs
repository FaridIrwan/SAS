using System;
using System.Collections.Generic;
using System.Text;
using System.ServiceModel;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace HTS.SAS.Entities
{
    [SerializableAttribute()]
    public class SponsorCoverLetterEn : FeeTypesEn
    {
        private string csSASCL_Code;
        private string csSASCL_Title;
        private string csSASCL_OurRef;
        private string csSASCL_YourRef;
        private string csSASCL_Address;
        private string csSASCL_Message;
        private string csSASCL_Signby;
        private string csSASCL_Name;
        private DateTime csSASCL_FrDate;
        private DateTime csSASCL_ToDate;
        private string csSASCL_Updateby;
        private DateTime csSASCL_UpdatedTime;
        private List<StudentEn> enstudentlist;
        private StudentEn enstudent;
        private SponsorEn ensponsor;
        private List<SponsorEn> ensponsorlist;
        private string csSAPG_Program;
        private FeeTypesEn enFEETYPE;
        private double cdSAFA_Amount;

        [XmlElement]
        public string Code
        {
            get { return csSASCL_Code; }
            set { csSASCL_Code = value; }
        }

        [XmlElement]
        public string Title
        {
            get { return csSASCL_Title; }
            set { csSASCL_Title = value; }
        }

        [XmlElement]
        public string OurRef
        {
            get { return csSASCL_OurRef; }
            set { csSASCL_OurRef = value; }
        }
        [XmlElement]
        public string YourRef
        {
            get { return csSASCL_YourRef; }
            set { csSASCL_YourRef = value; }
        }
        [XmlElement]
        public string Address
        {
            get { return csSASCL_Address; }
            set { csSASCL_Address = value; }
        }
        [XmlElement]
        public string Message
        {
            get { return csSASCL_Message; }
            set { csSASCL_Message = value; }
        }
        [XmlElement]
        public string SignBy
        {
            get { return csSASCL_Signby; }
            set { csSASCL_Signby = value; }
        }

        [XmlElement]
        public string Name
        {
            get { return csSASCL_Name; }
            set { csSASCL_Name = value; }
        }

        [XmlElement]
        public DateTime FromDate
        {
            get { return csSASCL_FrDate; }
            set { csSASCL_FrDate = value; }
        }
        [XmlElement]
        public DateTime ToDate
        {
            get { return csSASCL_ToDate; }
            set { csSASCL_ToDate = value; }
        }
        [XmlElement]
        public string UpdatedBy
        {
            get { return csSASCL_Updateby; }
            set { csSASCL_Updateby = value; }
        }
        [XmlElement]
        public DateTime UpdatedTime
        {
            get { return csSASCL_UpdatedTime; }
            set { csSASCL_UpdatedTime = value; }
        }

        public StudentEn Studentacc
        {
            get { return enstudent; }
            set { enstudent = value; }
        }

        public List<StudentEn> Studentacclist
        {
            get { return enstudentlist; }
            set { enstudentlist = value; }
        }

        public string MatricNo
        {
            get { return Studentacc.MatricNo; }
            set { Studentacc.MatricNo = value; }
        }

        public string StudentName
        {
            get { return Studentacc.StudentName; }
            set { Studentacc.StudentName = value; }
        }

        public string ICNo
        {
            get { return Studentacc.ICNo; }
            set { Studentacc.ICNo = value; }
        }

        public string CurretSemesterYear
        {
            get { return Studentacc.CurretSemesterYear; }
            set { Studentacc.CurretSemesterYear = value; }
        }

        public string ProgramID
        {
            get { return Studentacc.ProgramID; }
            set { Studentacc.ProgramID = value; }
        }

        public string ProgramName
        {
            get { return csSAPG_Program; }
            set { csSAPG_Program = value; }
        }

        public SponsorEn SponsorDetails
        {
            get { return ensponsor; }
            set { ensponsor = value; }
        }

        public List<SponsorEn> SponsorDetailsList
        {
            get { return ensponsorlist; }
            set { ensponsorlist = value; }
        }

        public string SponsorCode
        {
            get { return SponsorDetails.SponserCode; }
            set { SponsorDetails.SponserCode = value; }
        }

        public string SponsorName
        {
            get { return SponsorDetails.SponsorName; }
            set { SponsorDetails.SponsorName = value; }
        }

        public string SponsorEmail
        {
            get { return SponsorDetails.Email; }
            set { SponsorDetails.Email = value; }
        }

        public bool SponsorStatus
        {
            get { return SponsorDetails.Status; }
            set { SponsorDetails.Status = value; }
        }

        public FeeTypesEn Feetype
        {
            get { return enFEETYPE; }
            set { enFEETYPE = value; }
        }

        public double FeeAmount
        {
            get { return cdSAFA_Amount; }
            set { cdSAFA_Amount = value; }
        }
    }
}
