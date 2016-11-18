using System;
using System.Collections.Generic;
using System.Text;
using System.ServiceModel;
using System.Runtime.Serialization;

namespace HTS.SAS.Entities
{
    [System.SerializableAttribute()]
    //[DataContract]
    public class FeeStructEn : FeeTypesEn
    {
        private string csSAFS_Code;
        private string csSAPG_Code;
        public string csSAST_Code;
        private string csSAFS_EffectFm;
        private bool cbSAFS_Status;
        private string csSAFS_UpdatedUser;
        private string csSAFS_UpdatedDtTm;
        private double cdSAFS_CrPoint;
        private double cdSAFS_TutAmt;
        private double cdSAFS_CrAmt;
        private string csCategory;
        private string csSAFS_Semester;
        private int csSAFS_TaxId;
        private List<FeeStrDetailsEn> cslstFeeStrDetails;
        private SemesterSetupEn ensemsestersetup;
        private List<FeeStructEn> lstFeeStrDetailsWithAmount;
        private string csSABP_Code;
        private string fdFeeFor;
        private int fdSem;
        private string csSafd_FeeBaseOn;
        //added by farid 19072016
        private double cdSAFS_NonTutAmt;

        [System.Xml.Serialization.XmlElement]
        ////[DataMember]

        public SemesterSetupEn semestersetup
        {
            get { return ensemsestersetup; }
            set { ensemsestersetup = value; }
        }



        public string FeeStructureCode
        {
            get { return csSAFS_Code; }
            set { csSAFS_Code = value; }
        }


        [System.Xml.Serialization.XmlElement]
        ////[DataMember]
        public string PGCode
        {
            get { return csSAPG_Code; }
            set { csSAPG_Code = value; }
        }


        [System.Xml.Serialization.XmlElement]
        ////[DataMember]
        public string STCode
        {
            get { return csSAST_Code; }
            set { csSAST_Code = value; }
        }

        [System.Xml.Serialization.XmlElement]
        ////[DataMember]
        public string EffectFm
        {
            get { return csSAFS_EffectFm; }
            set { csSAFS_EffectFm = value; }
        }


        [System.Xml.Serialization.XmlElement]
        ////[DataMember]
        public bool Status
        {
            get { return cbSAFS_Status; }
            set { cbSAFS_Status = value; }
        }


        [System.Xml.Serialization.XmlElement]
        ////[DataMember]
        public string UpdatedUser
        {
            get { return csSAFS_UpdatedUser; }
            set { csSAFS_UpdatedUser = value; }
        }


        [System.Xml.Serialization.XmlElement]
        ////[DataMember]
        public string UpdatedDtTm
        {
            get { return csSAFS_UpdatedDtTm; }
            set { csSAFS_UpdatedDtTm = value; }
        }


        [System.Xml.Serialization.XmlElement]
        ////[DataMember]
        public double CrPoint
        {
            get { return cdSAFS_CrPoint; }
            set { cdSAFS_CrPoint = value; }
        }


        [System.Xml.Serialization.XmlElement]
        ////[DataMember]
        public double TutAmt
        {
            get { return cdSAFS_TutAmt; }
            set { cdSAFS_TutAmt = value; }
        }

        [System.Xml.Serialization.XmlElement]
        ////[DataMember]
        public double CrAmount
        {
            get { return cdSAFS_CrAmt; }
            set { cdSAFS_CrAmt = value; }
        }

        [System.Xml.Serialization.XmlElement]
        ////[DataMember]
        public string FeeCategory
        {
            get { return csCategory; }
            set { csCategory = value; }
        }

        [System.Xml.Serialization.XmlElement]
        ////[DataMember]
        public List<FeeStrDetailsEn> ListFeeStrDetails
        {
            get { return cslstFeeStrDetails; }
            set { cslstFeeStrDetails = value; }
        }

        [System.Xml.Serialization.XmlElement]
        ////[DataMember]
        public string Semester
        {
            get { return csSAFS_Semester; }
            set { csSAFS_Semester = value; }
        }
        [System.Xml.Serialization.XmlElement]
        ////[DataMember]
        public int TaxId
        {
            get { return csSAFS_TaxId; }
            set { csSAFS_TaxId = value; }
        }

        public string BidangCode
        {
            get { return csSABP_Code; }
            set { csSABP_Code = value; }
        }

        [System.Xml.Serialization.XmlElement]
        public List<FeeStructEn> lstFeeStrWithAmt
        {
            get { return lstFeeStrDetailsWithAmount; }
            set { lstFeeStrDetailsWithAmount = value; }
        }

        [System.Xml.Serialization.XmlElement]
        public string FeeFor
        {
            get { return fdFeeFor; }
            set { fdFeeFor = value; }
        }

        [System.Xml.Serialization.XmlElement]
        public int FeeDetailSem
        {
            get { return fdSem; }
            set { fdSem = value; }
        }

        [System.Xml.Serialization.XmlElement]
        public string FeeBaseOn
        {
            get { return csSafd_FeeBaseOn; }
            set { csSafd_FeeBaseOn = value; }
        }

        [System.Xml.Serialization.XmlElement]
        ////[DataMember]
        public double NonTutAmt
        {
            get { return cdSAFS_NonTutAmt; }
            set { cdSAFS_NonTutAmt = value; }
        }

    }
}
