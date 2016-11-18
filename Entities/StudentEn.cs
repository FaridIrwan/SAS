using System;
using System.Collections.Generic;
using System.Text;
using System.ServiceModel;
using System.Runtime.Serialization;

namespace HTS.SAS.Entities
{
    [System.SerializableAttribute()]
    //[DataContract]
    public class StudentEn:AccountsEn
    {
        private string csSASI_MatricNo;
        private string csSASI_Name;
        private string csSASI_PgId;
        private string csSASI_Faculty;
        private string csSASI_ICNo;
        private string csSASI_Passport;
        private string csSASI_ID;
        private string csSASI_Intake;
        private int ciSASI_CurSem;
        private string csSASI_CurSemYr;
        private string csSASI_Studytype;
        private string csSASS_Code;
        private string csSASC_Code;
        private bool cbSASI_Hostel;
        private string csSAKO_Code;
        private string csSASI_OtherID;
        private string csSABK_Code;
        private string csSASI_FloorNo;
        private string csSART_Code;
        private double cdSASI_CrditHrs;
        private double cdSASI_GPA;
        private double cdSASI_CGPA;
        private string csSASI_Add1;
        private string csSASI_Add2;
        private string csSASI_Add3;
        private string csSASI_City;
        private string csSASI_State;
        private string csSASI_Country;
        private string csSASI_Postcode;
        private string csSASI_Email;
        private string csSASI_Tel;
        private string csSASI_HP;
        private string csSASI_Bank;
        private string csSASI_AccNo;
        private string csSASI_GLCode;
        private int ciSABR_Code;
        private bool cbSASI_StatusRec;
        private bool cbSASI_AFCStatus;
        private string csSASI_UpdatedBy;
        private string csSASI_UpdatedDtTm;
        private string csSASI_MAdd1;
        private string csSASI_MAdd2;
        private string csSASI_MAdd3;
        private string csSASI_MCity;
        private string csSASI_MState;
        private string csSASI_MCountry;
        private string csSASI_MPostcode;
        private string csSASS_Sponsor;
        private int _stuIndex;
        private bool _StAuto;
        private string csSASI_FeeCat;
        private List<StudentSponEn> lstStuSponser;
        private StudentSponEn enstsponser;
        //private List<StuSponFeeTypesEn> enliststsponserfee;
        private StudentCategoryAccessEn enstcatacc;
        private ProgramInfoEn enprogram;
        private List<AccountsEn> lstaccounts;
        private double cdamountpaid;
        private double csOutstandingAmount;
        private double csLoanAmount;
        private int csIsReleased;
        private string _stManual;
        private FacultyEn enfaculty;
        private AccountsDetailsEn enAccountDetails;

        private int csNoKelompok;
        private string cskodUni;
        private string csKumpulanPelajar;
        private string csTarikhProcess;
        private string csKodBank;
        private double csAmaunTerima;
        private string csNilaiBersih;
        private string csTarikhTransaksi;
        private string csTarikhLupusWarran;
        private string stAkaunPelajar;
        private string csFiller;
        private string csStatusBayaran;
        private string csJumlahAmaun;
        private string csJumlahRekod;
        private string csKokoCode;
        private string csProgramType;
        private string csPostStatus;
        private int cssasi_reg_status;
        private double csDebitAmount;
        private double csCreditAmount;
        private string csOldPgId;
        private string csCurPgId;
        private double csOldCreditHr;        
        private double csCreditDiff;
        private double csSASS_Limit;
        private int csProgramChange;

        private string csInternal_Use;
        private string csProgramName;
        private int _StudentQty;
        private bool csSass_Type;
        private string _SponFeeCode;
        private string csSAFD_FeeBaseOn;
        private string csHostelIntake;

        //added by farid 19072016
        private double csStudentCredithours;

        [System.Xml.Serialization.XmlElement]
        ////[DataMember]
        public string ProgramType
        {
            get { return csProgramType; }
            set { csProgramType = value; }
        }

        [System.Xml.Serialization.XmlElement]
        ////[DataMember]
        public string KokoCode
        {
            get { return csKokoCode; }
            set { csKokoCode = value; }
        }

        [System.Xml.Serialization.XmlElement]
        ////[DataMember]
        public string StatusBayaran
        {
            get { return csStatusBayaran; }
            set { csStatusBayaran = value; }
        }

        [System.Xml.Serialization.XmlElement]
        ////[DataMember]
        public string AkaunPelajar
        {
            get { return stAkaunPelajar; }
            set { stAkaunPelajar = value; }
        }

        [System.Xml.Serialization.XmlElement]
        ////[DataMember]
        public string TarikhLupusWarran
        {
            get { return csTarikhLupusWarran; }
            set { csTarikhLupusWarran = value; }
        }

        [System.Xml.Serialization.XmlElement]
        ////[DataMember]
        public string TarikhTransaksi
        {
            get { return csTarikhTransaksi; }
            set { csTarikhTransaksi = value; }
        }

        [System.Xml.Serialization.XmlElement]
        ////[DataMember]
        public string NilaiBersih
        {
            get { return csNilaiBersih; }
            set { csNilaiBersih = value; }
        }

        [System.Xml.Serialization.XmlElement]
        ////[DataMember]
        public string MatricNo
        {
            get { return csSASI_MatricNo; }
            set { csSASI_MatricNo = value; }
        }

        [System.Xml.Serialization.XmlElement]
        ////[DataMember]
        public string StudentName
        {
            get { return csSASI_Name; }
            set { csSASI_Name = value; }
        }
        public string PostStatus
        {
            get { return csPostStatus; }
            set { csPostStatus = value; }
        }
        public StudentSponEn STsponsercode
        {
            get { return enstsponser; }
            set { enstsponser = value; }
        }

        public AccountsDetailsEn AccountDetailsEn
        {
            get { return enAccountDetails; }
            set { enAccountDetails = value; }
        }
       public ProgramInfoEn Programen
        {
            get { return enprogram; }
            set { enprogram = value; }
        }
        public StudentCategoryAccessEn StCategoryAcess
        {
            get { return enstcatacc; }
            set { enstcatacc = value; }
        }
        public FacultyEn FacultyEntity
        {
            get { return enfaculty; }
            set { enfaculty = value; }
        }

        [System.Xml.Serialization.XmlElement]
        ////[DataMember]
        public string ProgramID
        {
            get { return csSASI_PgId; }
            set { csSASI_PgId = value; }
        }
        
        [System.Xml.Serialization.XmlElement]
        ////[DataMember]
        public bool StAuto
        {
            get { return _StAuto; }
            set { _StAuto = value; }
        }

        [System.Xml.Serialization.XmlElement]
        ////[DataMember]
        public string SManual
        {
            get { return _stManual; }
            set { _stManual = value; }
        }

        [System.Xml.Serialization.XmlElement]
        ////[DataMember]
        public string Faculty
        {
            get { return csSASI_Faculty; }
            set { csSASI_Faculty = value; }
        }


        [System.Xml.Serialization.XmlElement]
        ////[DataMember]
        public string ICNo
        {
            get { return csSASI_ICNo; }
            set { csSASI_ICNo = value; }
        }


        [System.Xml.Serialization.XmlElement]
        ////[DataMember]
        public string Passport
        {
            get { return csSASI_Passport; }
            set { csSASI_Passport = value; }
        }


        [System.Xml.Serialization.XmlElement]
        ////[DataMember]
        public string ID
        {
            get { return csSASI_ID; }
            set { csSASI_ID = value; }
        }


        [System.Xml.Serialization.XmlElement]
        ////[DataMember]
        public string Intake
        {
            get { return csSASI_Intake; }
            set { csSASI_Intake = value; }
        }


        [System.Xml.Serialization.XmlElement]
        ////[DataMember]
        public int CurrentSemester
        {
            get { return ciSASI_CurSem; }
            set { ciSASI_CurSem = value; }
        }


        [System.Xml.Serialization.XmlElement]
        ////[DataMember]
        public string CurretSemesterYear
        {
            get { return csSASI_CurSemYr; }
            set { csSASI_CurSemYr = value; }
        }


        [System.Xml.Serialization.XmlElement]
        ////[DataMember]
        public string Studytype
        {
            get { return csSASI_Studytype; }
            set { csSASI_Studytype = value; }
        }


        [System.Xml.Serialization.XmlElement]
        ////[DataMember]
        public string StudentCode
        {
            get { return csSASS_Code; }
            set { csSASS_Code = value; }
        }

        [System.Xml.Serialization.XmlElement]
        ////[DataMember]
        public int RegistrationStatus
        {
            get { return cssasi_reg_status; }
            set { cssasi_reg_status = value; }
        }

        [System.Xml.Serialization.XmlElement]
        ////[DataMember]
        public string CategoryCode
        {
            get { return csSASC_Code; }
            set { csSASC_Code = value; }
        }


        [System.Xml.Serialization.XmlElement]
        ////[DataMember]
        public bool Hostel
        {
            get { return cbSASI_Hostel; }
            set { cbSASI_Hostel = value; }
        }


        [System.Xml.Serialization.XmlElement]
        ////[DataMember]
        public string SAKO_Code
        {
            get { return csSAKO_Code; }
            set { csSAKO_Code = value; }
        }


        [System.Xml.Serialization.XmlElement]
        ////[DataMember]
        public string SASI_OtherID
        {
            get { return csSASI_OtherID; }
            set { csSASI_OtherID = value; }
        }


        [System.Xml.Serialization.XmlElement]
        ////[DataMember]
        public string SABK_Code
        {
            get { return csSABK_Code; }
            set { csSABK_Code = value; }
        }


        [System.Xml.Serialization.XmlElement]
        ////[DataMember]
        public string SASI_FloorNo
        {
            get { return csSASI_FloorNo; }
            set { csSASI_FloorNo = value; }
        }


        [System.Xml.Serialization.XmlElement]
        ////[DataMember]
        public string SART_Code
        {
            get { return csSART_Code; }
            set { csSART_Code = value; }
        }


        [System.Xml.Serialization.XmlElement]
        ////[DataMember]
        public double SASI_CrditHrs
        {
            get { return cdSASI_CrditHrs; }
            set { cdSASI_CrditHrs = value; }
        }


        [System.Xml.Serialization.XmlElement]
        ////[DataMember]
        public double SASI_GPA
        {
            get { return cdSASI_GPA; }
            set { cdSASI_GPA = value; }
        }


        [System.Xml.Serialization.XmlElement]
        ////[DataMember]
        public double SASI_CGPA
        {
            get { return cdSASI_CGPA; }
            set { cdSASI_CGPA = value; }
        }


        [System.Xml.Serialization.XmlElement]
        ////[DataMember]
        public string SASI_Add1
        {
            get { return csSASI_Add1; }
            set { csSASI_Add1 = value; }
        }


        [System.Xml.Serialization.XmlElement]
        ////[DataMember]
        public string SASI_Add2
        {
            get { return csSASI_Add2; }
            set { csSASI_Add2 = value; }
        }


        [System.Xml.Serialization.XmlElement]
        ////[DataMember]
        public string SASI_Add3
        {
            get { return csSASI_Add3; }
            set { csSASI_Add3 = value; }
        }


        [System.Xml.Serialization.XmlElement]
        ////[DataMember]
        public string SASI_City
        {
            get { return csSASI_City; }
            set { csSASI_City = value; }
        }


        [System.Xml.Serialization.XmlElement]
        ////[DataMember]
        public string SASI_State
        {
            get { return csSASI_State; }
            set { csSASI_State = value; }
        }


        [System.Xml.Serialization.XmlElement]
        ////[DataMember]
        public string SASI_Country
        {
            get { return csSASI_Country; }
            set { csSASI_Country = value; }
        }


        [System.Xml.Serialization.XmlElement]
        ////[DataMember]
        public string SASI_Postcode
        {
            get { return csSASI_Postcode; }
            set { csSASI_Postcode = value; }
        }


        [System.Xml.Serialization.XmlElement]
        ////[DataMember]
        public string SASI_Email
        {
            get { return csSASI_Email; }
            set { csSASI_Email = value; }
        }


        [System.Xml.Serialization.XmlElement]
        ////[DataMember]
        public string SASI_Tel
        {
            get { return csSASI_Tel; }
            set { csSASI_Tel = value; }
        }


        [System.Xml.Serialization.XmlElement]
        ////[DataMember]
        public string SASI_HP
        {
            get { return csSASI_HP; }
            set { csSASI_HP = value; }
        }


        [System.Xml.Serialization.XmlElement]
        ////[DataMember]
        public string SASI_Bank
        {
            get { return csSASI_Bank; }
            set { csSASI_Bank = value; }
        }


        [System.Xml.Serialization.XmlElement]
        ////[DataMember]
        public string SASI_AccNo
        {
            get { return csSASI_AccNo; }
            set { csSASI_AccNo = value; }
        }


        [System.Xml.Serialization.XmlElement]
        ////[DataMember]
        public string SASI_GLCode
        {
            get { return csSASI_GLCode; }
            set { csSASI_GLCode = value; }
        }


        [System.Xml.Serialization.XmlElement]
        ////[DataMember]
        public int SABR_Code
        {
            get { return ciSABR_Code; }
            set { ciSABR_Code = value; }
        }


        [System.Xml.Serialization.XmlElement]
        ////[DataMember]
        public bool SASI_StatusRec
        {
            get { return cbSASI_StatusRec; }
            set { cbSASI_StatusRec = value; }
        }

        [System.Xml.Serialization.XmlElement]
        ////[DataMember]
        public bool SASI_AFCStatus
        {
            get { return cbSASI_AFCStatus; }
            set { cbSASI_AFCStatus = value; }
        }


        [System.Xml.Serialization.XmlElement]
        ////[DataMember]
        public string SASI_UpdatedBy
        {
            get { return csSASI_UpdatedBy; }
            set { csSASI_UpdatedBy = value; }
        }


        [System.Xml.Serialization.XmlElement]
        ////[DataMember]
        public string SASI_UpdatedDtTm
        {
            get { return csSASI_UpdatedDtTm; }
            set { csSASI_UpdatedDtTm = value; }
        }


        [System.Xml.Serialization.XmlElement]
        ////[DataMember]
        public string SASI_MAdd1
        {
            get { return csSASI_MAdd1; }
            set { csSASI_MAdd1 = value; }
        }


        [System.Xml.Serialization.XmlElement]
        ////[DataMember]
        public string SASI_MAdd2
        {
            get { return csSASI_MAdd2; }
            set { csSASI_MAdd2 = value; }
        }


        [System.Xml.Serialization.XmlElement]
        ////[DataMember]
        public string SASI_MAdd3
        {
            get { return csSASI_MAdd3; }
            set { csSASI_MAdd3 = value; }
        }


        [System.Xml.Serialization.XmlElement]
        ////[DataMember]
        public string SASI_MCity
        {
            get { return csSASI_MCity; }
            set { csSASI_MCity = value; }
        }


        [System.Xml.Serialization.XmlElement]
        ////[DataMember]
        public string SASI_MState
        {
            get { return csSASI_MState; }
            set { csSASI_MState = value; }
        }


        [System.Xml.Serialization.XmlElement]
        ////[DataMember]
        public string SASI_MCountry
        {
            get { return csSASI_MCountry; }
            set { csSASI_MCountry = value; }
        }


        [System.Xml.Serialization.XmlElement]
        ////[DataMember]
        public string SASI_MPostcode
        {
            get { return csSASI_MPostcode; }
            set { csSASI_MPostcode = value; }
        }

        [System.Xml.Serialization.XmlElement]
        ////[DataMember]
        public int StuIndex
        {
            get { return _stuIndex; }
            set { _stuIndex = value; }
        }
        [System.Xml.Serialization.XmlElement]
        ////[DataMember]
        public string FeeCat
        {
            get { return csSASI_FeeCat; }
            set { csSASI_FeeCat = value; }
        }

        [System.Xml.Serialization.XmlElement]
        ////[DataMember]
        public List<StudentSponEn> ListStuSponser
        {
            get { return lstStuSponser; }
            set { lstStuSponser = value; }
        }              

        public List<AccountsEn> Listtaccounts
        {
            get { return lstaccounts; }
            set { lstaccounts = value; }
        }
        public double AmountPaid
        {
            get { return cdamountpaid; }
            set { cdamountpaid = value; }
        }
        public double OutstandingAmount
        {
            get { return csOutstandingAmount; }
            set { csOutstandingAmount = value; }
        }
        public double LoanAmount
        {
            get { return csLoanAmount; }
            set { csLoanAmount = value; }
        }

        public int IsReleased
        {
            get { return csIsReleased; }
            set { csIsReleased = value; }
        }
        //[DataMember]
        public string SponsorCode
        {
            get { return csSASS_Sponsor; }
            set { csSASS_Sponsor = value; }
        }

        public double DebitAmount
        {
            get { return csDebitAmount; }
            set { csDebitAmount = value; }
        }

        public double CreditAmount
        {
            get { return csCreditAmount; }
            set { csCreditAmount = value; }
        }
        [System.Xml.Serialization.XmlElement]
        ////[DataMember]
        public string OldProgramID
        {
            get { return csOldPgId; }
            set { csOldPgId = value; }
        }
        [System.Xml.Serialization.XmlElement]
        ////[DataMember]
        public string CurProgramID
        {
            get { return csCurPgId; }
            set { csCurPgId = value; }
        }
        [System.Xml.Serialization.XmlElement]
        ////[DataMember]
        public double OldCrditHrs
        {
            get { return csOldCreditHr; }
            set { csOldCreditHr = value; }
        }
        [System.Xml.Serialization.XmlElement]
        ////[DataMember]
        public double CrditHrDiff
        {
            get { return csCreditDiff; }
            set { csCreditDiff = value; }
        }
        [System.Xml.Serialization.XmlElement]
        //[DataMember]
        public double SponsorLimit
        {
            get { return csSASS_Limit; }
            set { csSASS_Limit = value; }
        }

        [System.Xml.Serialization.XmlElement]
        public int ProgramChange
        {
            get { return csProgramChange; }
            set { csProgramChange= value; }
        }
        [System.Xml.Serialization.XmlElement]
        public string Internal_Use
        {
            get { return csInternal_Use; }
            set { csInternal_Use = value; }
        }

        [System.Xml.Serialization.XmlElement]
        public string ProgramName
        {
            get { return csProgramName; }
            set { csProgramName = value; }
        }

        [System.Xml.Serialization.XmlElement]
        public int StudentQty
        {
            get { return _StudentQty; }
            set { _StudentQty = value; }
        }

        [System.Xml.Serialization.XmlElement]
        public bool FullySponsor
        {
            get { return csSass_Type; }
            set { csSass_Type = value; }
        }

        [System.Xml.Serialization.XmlElement]
        public string SponFeeCode
        {
            get { return _SponFeeCode; }
            set { _SponFeeCode = value; }
        }

        [System.Xml.Serialization.XmlElement]
        public string FeeBaseOn
        {
            get { return csSAFD_FeeBaseOn; }
            set { csSAFD_FeeBaseOn = value; }
        }

        public string HostelIntake
        {
            get { return csHostelIntake; }
            set { csHostelIntake = value; }
        }

        [System.Xml.Serialization.XmlElement]
        ////[DataMember]
        public double StudentCredithour
        {
            get { return csStudentCredithours; }
            set { csStudentCredithours = value; }
        }

    }
}
//---------------------------------------------------------------------------------


