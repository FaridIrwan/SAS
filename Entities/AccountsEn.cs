using System;
using System.Collections.Generic;
using System.Text;
using System.ServiceModel;
using System.Runtime.Serialization;

namespace HTS.SAS.Entities
{
    [System.SerializableAttribute()]
    //[DataContract]
    public class AccountsEn:AccountsDetailsEn
    {
        private int ciTransID;
        private string csTransTempCode;
        private string csTransCode;
        private string csCreditRef;
        private string csCreditRef1;
        private string csDebitRef;
        private string csDebitRef1;
        private string csCategory;
        private string csSubCategory;
        private string csTransType;
        private string csSubType;
        private string csSourceType;
        private DateTime coTransDate;
        private DateTime coDueDate;
        private string csBatchCode;
        private DateTime coBatchDate;
        private string csCrRef1;
        private string csCrRef2;
        private string csDescription;
        private string csCurrency;
        private double cdBatchTotal;
        private double cdTax;
        private double cdDiscount;
        private double cdTaxAmount;
        private double cdDiscountAmount;
        private double cdTransAmount;
        private double cdPaidAmount;
        private string csTransStatus;
        private double cdTempAmount;
        private double cdTempPaidAmount;
        private string csPaymentMode;
        private string csBankCode;
        private string csPayeeName;
        private DateTime coChequeDate ;
        private string csChequeNo;
        private string csVoucherNo;
        private string csSubRef1;
        private string csSubRef2;
        private string csSubRef3;
        private string csPostStatus;
        private int csIntStatus;
        private string csCreatedBy;
        private DateTime coCreatedDateTime;
        private string csPostedBy;
        private DateTime coPostedDateTime;
        private string csIntCode;
        private string csGLCode;
        private string csUpdatedBy;
        private DateTime coUpdatedTime;
        private string csDeletedBy;
        private string csSponsor;
        private string csSponsorId;  //SponsorId
        private string StuOutStanding;  //Student Out Standing
        private string StuAccBankSlipId;  //Student Accound Bank Slip ID

        private List<AccountsDetailsEn> clAccountDetails;
        private SponsorEn ceSponsor;
        private List<SponsorEn> clistSponsorEn;
        private StudentEn ceStudent;
        private List<StudentEn> clistStudentEn;
        private List<AccountsEn> clistAccEn;
        private bool _StAuto;
        private string _stManual;
        private BankProfileEn ceBankDetails;
        private ProgramInfoEn cePrograminfo;
        private string _strMatricNo;
        private int das;
        private string csBatchIntake;
        private string csPocketAmount;
        private double csCredit;
        private double csDebit;
        private double csAllocatedAmount;
        private double cdPocketAmount;
        private string csFundCode;
        private string csKodUni;
        private string csKumpulanPelajar;
        private string csTarikhProses;
        private string csKodBank;
        private string csAutoNum;
        private string csNoKelompok_H;
        private string csNoKelompok_F;
        private double csJumlahAmaun;
        private string csJumlahRekod;
        private string csInternal_Use;

        private DateTime csReceiptDate;

        private bool cbHostel;
        private string cbKoko;
        private decimal csControlAmt;

        [System.Xml.Serialization.XmlElement]
        ////[DataMember]
        public string KumpulanPelajar
        {
            get { return csKumpulanPelajar; }
            set { csKumpulanPelajar = value; }
        }

        [System.Xml.Serialization.XmlElement]
        ////[DataMember]
        public double JumlahAmaun
        {
            get { return csJumlahAmaun; }
            set { csJumlahAmaun = value; }
        }

        [System.Xml.Serialization.XmlElement]
        ////[DataMember]
        public string JumlahRekod
        {
            get { return csJumlahRekod; }
            set { csJumlahRekod = value; }
        }

        [System.Xml.Serialization.XmlElement]
        ////[DataMember]
        public string NoKelompok_H
        {
            get { return csNoKelompok_H; }
            set { csNoKelompok_H = value; }
        }

        [System.Xml.Serialization.XmlElement]
        ////[DataMember]
        public string NoKelompok_F
        {
            get { return csNoKelompok_F; }
            set { csNoKelompok_F = value; }
        }

        [System.Xml.Serialization.XmlElement]
        ////[DataMember]
        public string AutoNum
        {
            get { return csAutoNum; }
            set { csAutoNum = value; }
        }

        [System.Xml.Serialization.XmlElement]
        ////[DataMember]
        public string KodBank
        {
            get { return csKodBank; }
            set { csKodBank = value; }
        }

        [System.Xml.Serialization.XmlElement]
        ////[DataMember]
        public string TarikhProses
        {
            get { return csTarikhProses; }
            set { csTarikhProses = value; }
        }

        [System.Xml.Serialization.XmlElement]
        ////[DataMember]
        public string KodUniversiti
        {
            get { return csKodUni; }
            set { csKodUni = value; }
        }

        [System.Xml.Serialization.XmlElement]
        ////[DataMember]
        public string PocketAmount
        {
            get { return csPocketAmount; }
            set { csPocketAmount = value; }
        }

        [System.Xml.Serialization.XmlElement]
        ////[DataMember]
        public int TranssactionID
        {
            get { return ciTransID; }
            set { ciTransID = value; }
        }


        [System.Xml.Serialization.XmlElement]
        ////[DataMember]
        public string TempTransCode
        {
            get { return csTransTempCode; }
            set { csTransTempCode = value; }
        }


        [System.Xml.Serialization.XmlElement]
        ////[DataMember]
        public string  TransactionCode
        {
            get { return csTransCode; }
            set { csTransCode = value; }
        }


        [System.Xml.Serialization.XmlElement]
        ////[DataMember]
        public string CreditRef
        {
            get { return csCreditRef; }
            set { csCreditRef = value; }
        }


        [System.Xml.Serialization.XmlElement]
        ////[DataMember]
        public string CreditRefOne
        {
            get { return csCreditRef1; }
            set { csCreditRef1 = value; }
        }


        [System.Xml.Serialization.XmlElement]
        ////[DataMember]
        public string DebitRef
        {
            get { return csDebitRef; }
            set { csDebitRef = value; }
        }


        [System.Xml.Serialization.XmlElement]
        ////[DataMember]
        public string DebitRefOne
        {
            get { return csDebitRef1; }
            set { csDebitRef1 = value; }
        }


        [System.Xml.Serialization.XmlElement]
        ////[DataMember]
        public string Category
        {
            get { return csCategory; }
            set { csCategory = value; }
        }


        [System.Xml.Serialization.XmlElement]
        ////[DataMember]
        public string SubCategory
        {
            get { return csSubCategory; }
            set { csSubCategory = value; }
        }


        [System.Xml.Serialization.XmlElement]
        ////[DataMember]
        public string TransType
        {
            get { return csTransType; }
            set { csTransType = value; }
        }


        [System.Xml.Serialization.XmlElement]
        ////[DataMember]
        public string SubType
        {
            get { return csSubType; }
            set { csSubType = value; }
        }


        [System.Xml.Serialization.XmlElement]
        ////[DataMember]
        public string SourceType
        {
            get { return csSourceType; }
            set { csSourceType = value; }
        }


        [System.Xml.Serialization.XmlElement]
        ////[DataMember]
        public DateTime TransDate
        {
            get { return coTransDate; }
            set { coTransDate = value; }
        }


        [System.Xml.Serialization.XmlElement]
        ////[DataMember]
        public DateTime DueDate
        {
            get { return coDueDate; }
            set { coDueDate = value; }
        }


        [System.Xml.Serialization.XmlElement]
        ////[DataMember]
        public string BatchCode
        {
            get { return csBatchCode; }
            set { csBatchCode = value; }
        }


        [System.Xml.Serialization.XmlElement]
        ////[DataMember]
        public DateTime BatchDate
        {
            get { return coBatchDate; }
            set { coBatchDate = value; }
        }


        [System.Xml.Serialization.XmlElement]
        ////[DataMember]
        public string CrRefOne
        {
            get { return csCrRef1; }
            set { csCrRef1 = value; }
        }


        [System.Xml.Serialization.XmlElement]
        ////[DataMember]
        public string CrRefTwo
        {
            get { return csCrRef2; }
            set { csCrRef2 = value; }
        }


        [System.Xml.Serialization.XmlElement]
        ////[DataMember]
        public string Description
        {
            get { return csDescription; }
            set { csDescription = value; }
        }


        [System.Xml.Serialization.XmlElement]
        ////[DataMember]
        public string CurrencyUsed
        {
            get { return csCurrency; }
            set { csCurrency = value; }
        }


        [System.Xml.Serialization.XmlElement]
        ////[DataMember]
        public double BatchTotal
        {
            get { return cdBatchTotal; }
            set { cdBatchTotal = value; }
        }


        [System.Xml.Serialization.XmlElement]
        ////[DataMember]
        public double TaxPercentage
        {
            get { return cdTax; }
            set { cdTax = value; }
        }


        [System.Xml.Serialization.XmlElement]
        ////[DataMember]
        public double DiscountPercentage
        {
            get { return cdDiscount; }
            set { cdDiscount = value; }
        }


        [System.Xml.Serialization.XmlElement]
        ////[DataMember]
        public double TaxAmount
        {
            get { return cdTaxAmount; }
            set { cdTaxAmount = value; }
        }


        [System.Xml.Serialization.XmlElement]
        ////[DataMember]
        public double DiscountAmount
        {
            get { return cdDiscountAmount; }
            set { cdDiscountAmount = value; }
        }


        [System.Xml.Serialization.XmlElement]
        ////[DataMember]
        public double TransactionAmount
        {
            get { return cdTransAmount; }
            set { cdTransAmount = value; }
        }


        [System.Xml.Serialization.XmlElement]
        ////[DataMember]
        public double PaidAmount
        {
            get { return cdPaidAmount; }
            set { cdPaidAmount = value; }
        }


        [System.Xml.Serialization.XmlElement]
        ////[DataMember]
        public string TransStatus
        {
            get { return csTransStatus; }
            set { csTransStatus = value; }
        }


        [System.Xml.Serialization.XmlElement]
        ////[DataMember]
        public double TempAmount
        {
            get { return cdTempAmount; }
            set { cdTempAmount = value; }
        }


        [System.Xml.Serialization.XmlElement]
        ////[DataMember]
        public double TempPaidAmount
        {
            get { return cdTempPaidAmount; }
            set { cdTempPaidAmount = value; }
        }


        [System.Xml.Serialization.XmlElement]
        ////[DataMember]
        public string PaymentMode
        {
            get { return csPaymentMode; }
            set { csPaymentMode = value; }
        }


        [System.Xml.Serialization.XmlElement]
        ////[DataMember]
        public string BankCode
        {
            get { return csBankCode; }
            set { csBankCode = value; }
        }


        [System.Xml.Serialization.XmlElement]
        ////[DataMember]
        public string PayeeName
        {
            get { return csPayeeName; }
            set { csPayeeName = value; }
        }


        [System.Xml.Serialization.XmlElement]
        ////[DataMember]
        public DateTime ChequeDate
        {
            get { return coChequeDate; }
            set { coChequeDate = value; }
        }


        [System.Xml.Serialization.XmlElement]
        ////[DataMember]
        public string ChequeNo
        {
            get { return csChequeNo; }
            set { csChequeNo = value; }
        }


        [System.Xml.Serialization.XmlElement]
        ////[DataMember]
        public string VoucherNo
        {
            get { return csVoucherNo; }
            set { csVoucherNo = value; }
        }


        [System.Xml.Serialization.XmlElement]
        ////[DataMember]
        public string SubReferenceOne
        {
            get { return csSubRef1; }
            set { csSubRef1 = value; }
        }


        [System.Xml.Serialization.XmlElement]
        ////[DataMember]
        public string SubReferenceTwo
        {
            get { return csSubRef2; }
            set { csSubRef2 = value; }
        }


        [System.Xml.Serialization.XmlElement]
        ////[DataMember]
        public string SubReferenceThree
        {
            get { return csSubRef3; }
            set { csSubRef3 = value; }
        }


        [System.Xml.Serialization.XmlElement]
        ////[DataMember]
        public string PostStatus
        {
            get { return csPostStatus; }
            set { csPostStatus = value; }
        }


        [System.Xml.Serialization.XmlElement]
        ////[DataMember]
        public int IntegrationStatus
        {
            get { return csIntStatus; }
            set { csIntStatus = value; }
        }


        [System.Xml.Serialization.XmlElement]
        ////[DataMember]
        public string CreatedBy
        {
            get { return csCreatedBy; }
            set { csCreatedBy = value; }
        }


        [System.Xml.Serialization.XmlElement]
        ////[DataMember]
        public DateTime CreatedDateTime
        {
            get { return coCreatedDateTime; }
            set { coCreatedDateTime = value; }
        }


        [System.Xml.Serialization.XmlElement]
        ////[DataMember]
        public string PostedBy
        {
            get { return csPostedBy; }
            set { csPostedBy = value; }
        }


        [System.Xml.Serialization.XmlElement]
        ////[DataMember]
        public DateTime PostedDateTime
        {
            get { return coPostedDateTime; }
            set { coPostedDateTime = value; }
        }


        [System.Xml.Serialization.XmlElement]
        ////[DataMember]
        public string IntegrationCode
        {
            get { return csIntCode; }
            set { csIntCode = value; }
        }


        [System.Xml.Serialization.XmlElement]
        ////[DataMember]
        public string GLCode
        {
            get { return csGLCode; }
            set { csGLCode = value; }
        }


        [System.Xml.Serialization.XmlElement]
        ////[DataMember]
        public string UpdatedBy
        {
            get { return csUpdatedBy; }
            set { csUpdatedBy = value; }
        }

        [System.Xml.Serialization.XmlElement]
        ////[DataMember]
        public string DeletedBy
        {
            get { return csDeletedBy; }
            set { csDeletedBy = value; }
        }


        [System.Xml.Serialization.XmlElement]
        ////[DataMember]
        public DateTime UpdatedTime
        {
            get { return coUpdatedTime; }
            set { coUpdatedTime = value; }
        }

        [System.Xml.Serialization.XmlElement]
        ////[DataMember]
        public List<AccountsDetailsEn> AccountDetailsList
        {
            get { return clAccountDetails; }
            set { clAccountDetails = value; }
        }

        [System.Xml.Serialization.XmlElement]
        ////[DataMember]
        public List<SponsorEn> SponsorList
        {
            get { return clistSponsorEn; }
            set { clistSponsorEn = value; }
        }
        [System.Xml.Serialization.XmlElement]
        ////[DataMember]
        public List<AccountsEn> AccList
        {
            get { return clistAccEn; }
            set { clistAccEn = value; }
        }
        [System.Xml.Serialization.XmlElement]
        ////[DataMember]
        public SponsorEn Sponsor
        {
            get { return ceSponsor; }
            set { ceSponsor = value; }
        }
        [System.Xml.Serialization.XmlElement]
        ////[DataMember]
        public BankProfileEn BankDet
        {
            get { return ceBankDetails; }
            set { ceBankDetails = value; }
        }
        [System.Xml.Serialization.XmlElement]
        ////[DataMember]
        public ProgramInfoEn ProgramInfo
        {
            get { return cePrograminfo; }
            set { cePrograminfo = value; }
        }
        [System.Xml.Serialization.XmlElement]
        ////[DataMember]
        public List<StudentEn> StudentList
        {
            get { return clistStudentEn; }
            set { clistStudentEn = value; }
        }
        [System.Xml.Serialization.XmlElement]
        ////[DataMember]
        public StudentEn Student
        {
            get { return ceStudent; }
            set { ceStudent = value; }
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
        public string StManual
        {
            get { return _stManual; }
            set { _stManual = value; }
        }

        [System.Xml.Serialization.XmlElement]
        ////[DataMember]
        public string MatricNo
        {
            get { return _strMatricNo; }
            set { _strMatricNo = value; }
        }

        [System.Xml.Serialization.XmlElement]
        ////[DataMember]
        public string BatchIntake
        {
            get { return csBatchIntake; }
            set { csBatchIntake = value; }
        }

        [System.Xml.Serialization.XmlElement]
        ////[DataMember]
        public double Debit
        {
            get { return csDebit; }
            set { csDebit = value; }
        }

        [System.Xml.Serialization.XmlElement]
        ////[DataMember]
        public double Credit
        {
            get { return csCredit; }
            set { csCredit = value; }
        }

        [System.Xml.Serialization.XmlElement]
        ////[DataMember]
        public double AllocatedAmount
        {
            get { return csAllocatedAmount; }
            set { csAllocatedAmount = value; }
        }

        [System.Xml.Serialization.XmlElement]
        ////[DataMember]
        public double SPocketAmount
        {
            get { return cdPocketAmount; }
            set { cdPocketAmount = value; }
        }

        [System.Xml.Serialization.XmlElement]
        ////[DataMember]
        public string UniversityFundId
        {
            get { return csFundCode; }
            set { csFundCode = value; }
        }

        [System.Xml.Serialization.XmlElement]
        ////[DataMember]
        public DateTime ReceiptDate
        {
            get { return csReceiptDate; }
            set { csReceiptDate = value; }
        }

        [System.Xml.Serialization.XmlElement]
        ////[DataMember]
        public string SponsorName
        {
            get { return csSponsor; }
            set { csSponsor = value; }
        }

        [System.Xml.Serialization.XmlElement]
        ////[DataMember]
        public string SponsorID
        {
            get { return csSponsorId; }
            set { csSponsorId = value; }
        }

        [System.Xml.Serialization.XmlElement]
        ////[DataMember]
        public string Outstanding_Amount
        {
            get { return StuOutStanding; }
            set { StuOutStanding = value; }
        }

        [System.Xml.Serialization.XmlElement]
        ////[DataMember]
        public string BankSlipID
        {
            get { return StuAccBankSlipId; }
            set { StuAccBankSlipId = value; }
        }

        [System.Xml.Serialization.XmlElement]
        public string Internal_Use
        {
            get { return csInternal_Use; }
            set { csInternal_Use = value; }
        }

        [System.Xml.Serialization.XmlElement]
        ////[DataMember]
        public bool IsHostel
        {
            get { return cbHostel; }
            set { cbHostel = value; }
        }

        [System.Xml.Serialization.XmlElement]
        ////[DataMember]
        public string IsKoko
        {
            get { return cbKoko; }
            set { cbKoko = value; }
        }

        ////[DataMember]
        public decimal ControlAmt
        {
            get { return csControlAmt; }
            set { csControlAmt = value; }
        }
    }
}


