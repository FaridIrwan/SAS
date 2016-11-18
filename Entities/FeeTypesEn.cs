using System;
using System.Collections.Generic;
using System.Text;
using System.ServiceModel;
using System.Runtime.Serialization;

namespace HTS.SAS.Entities
{
    [System.SerializableAttribute()]
    //[DataContract]
    public class FeeTypesEn : FeeChargesEn
    {
        private string csSAFT_Code;
        private string csSAFT_Desc;
        private string csSAFT_FeeType;
        private string csSAFT_Hostel;
        private int ciSAFT_Priority;
        private int csIsTutionFee;
        private string csSAFT_Remarks;
        private string csSAFT_GLCode;
        private bool cbSAFT_Status;
        private string csSAFT_UpdatedBy;
        private string csSAFT_UpdatedDtTm;
        private double tempAmt;
        private string csSASC_Code;
        private double cdSAFS_Amount;
        private List<FeeChargesEn> cslstFeeCharges;
        private List<FacultyGLAccEn> enLstFacultyGL;
        private List<KolejGLAccEn> enLstKolejGL;
        private int csSAKO_CreditHours;
        //  Author			: Anil Kumar - T-Melmax Sdn Bhd
        //Created Date	: 12/06/2015
        private int csSAFT_taxid;
        private int csIsChangeProg;
        // added Jessica 19/2/2016
        private string csFeeCategory;
        //added Farid 6/4/2016
        private List<KokoEn> kokolstFeeCharges;

        [System.Xml.Serialization.XmlElement]
        ////[DataMember]
        public int CreditHours
        {
            get { return csSAKO_CreditHours; }
            set { csSAKO_CreditHours = value; }
        }
        [System.Xml.Serialization.XmlElement]
        // //[DataMember]
        public string FeeTypeCode
        {
            get { return csSAFT_Code; }
            set { csSAFT_Code = value; }
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
        public string FeeType
        {
            get { return csSAFT_FeeType; }
            set { csSAFT_FeeType = value; }
        }


        [System.Xml.Serialization.XmlElement]
        ////[DataMember]
        public string Hostel
        {
            get { return csSAFT_Hostel; }
            set { csSAFT_Hostel = value; }
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
        public string Remarks
        {
            get { return csSAFT_Remarks; }
            set { csSAFT_Remarks = value; }
        }


        [System.Xml.Serialization.XmlElement]
        ////[DataMember]
        public string GLCode
        {
            get { return csSAFT_GLCode; }
            set { csSAFT_GLCode = value; }
        }


        [System.Xml.Serialization.XmlElement]
        // //[DataMember]
        public bool Status
        {
            get { return cbSAFT_Status; }
            set { cbSAFT_Status = value; }
        }


        [System.Xml.Serialization.XmlElement]
        ////[DataMember]
        public string UpdatedBy
        {
            get { return csSAFT_UpdatedBy; }
            set { csSAFT_UpdatedBy = value; }
        }


        [System.Xml.Serialization.XmlElement]
        ////[DataMember]
        public string UpdatedDtTm
        {
            get { return csSAFT_UpdatedDtTm; }
            set { csSAFT_UpdatedDtTm = value; }
        }
        [System.Xml.Serialization.XmlElement]
        ////[DataMember]
        public double tempAmount
        {
            get { return tempAmt; }
            set { tempAmt = value; }
        }
        [System.Xml.Serialization.XmlElement]
        ////[DataMember]
        public string SCCode
        {
            get { return csSASC_Code; }
            set { csSASC_Code = value; }
        }
        [System.Xml.Serialization.XmlElement]
        ////[DataMember]
        public double FSAmount
        {
            get { return cdSAFS_Amount; }
            set { cdSAFS_Amount = value; }
        }

        [System.Xml.Serialization.XmlElement]
        ////[DataMember]
        public List<FeeChargesEn> ListFeeCharges
        {
            get { return cslstFeeCharges; }
            set { cslstFeeCharges = value; }
        }

        [System.Xml.Serialization.XmlElement]
        ////[DataMember]
        public List<FacultyGLAccEn> LstFacultyGL
        {
            get { return enLstFacultyGL; }
            set { enLstFacultyGL = value; }
        }

        [System.Xml.Serialization.XmlElement]
        ////[DataMember]
        public List<KolejGLAccEn> LstKolejGL
        {
            get { return enLstKolejGL; }
            set { enLstKolejGL = value; }
        }

        [System.Xml.Serialization.XmlElement]
        ////[DataMember]
        public int IsTutionFee
        {
            get { return csIsTutionFee; }
            set { csIsTutionFee = value; }
        }

        //  Author			: Anil Kumar - T-Melmax Sdn Bhd
        //Created Date	: 12/06/2015
        [System.Xml.Serialization.XmlElement]
        ////[DataMember]
        public int TaxId
        {
            get { return csSAFT_taxid; }
            set { csSAFT_taxid = value; }
        }

        public int IsChangeProgram
        {
            get { return csIsChangeProg; }
            set { csIsChangeProg = value; }
        }

        [System.Xml.Serialization.XmlElement]
        ////[DataMember]
        public string FeeCategory //Field name in database - SAFT_Hostel
        {
            get { return csFeeCategory; }
            set { csFeeCategory = value; }
        }

        [System.Xml.Serialization.XmlElement]
        ////[DataMember]
        public List<KokoEn> ListKokoCharges
        {
            get { return kokolstFeeCharges; }
            set { kokolstFeeCharges = value; }
        }
    }
}
