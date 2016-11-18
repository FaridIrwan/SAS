using System;
using System.Collections.Generic;
using System.Text;
using System.ServiceModel;
using System.Runtime.Serialization;

namespace HTS.SAS.Entities
{
    [System.SerializableAttribute()]
    //[DataContract]
    public class AccountsDetailsEn : FeeTypesEn
    {
        private int ciTransID;
        private string csTransTempCode;
        private string csTransCode;
        private string csRefCode;
        private double cdTempAmount;
        private string csQuantity;
        private double cdTransAmount;
        private double cdTax;
        private double cdDiscount;
        private double cdTaxAmount;
        private double cdDiscountAmount;
        private double cdPaidAmount;
        private double cdTempPaidAmount;
        private string csTransStatus;
        private string csPostStatus;
        private string csRef1;
        private string csRef2;
        private int ciPriority;
        private List<StudentEn> enstudentlist;
        private StudentEn enstudent;
        private string csVoucherNo;
        private string csCreditRef;
        private DateTime coTransDate;
        private DateTime coDueDate;
        private FeeTypesEn enFEETYPE;
        private string csRef3;
        private float spTransAmount;
        private string csnoKelompok;
        private string csNoWarran;
        //private string csNoPelajar;
        private double csAmaunWarran;
        private string csAmaunPotongan;
        private string csNoAkaun;
        private string csStatusBayaran;
        private string csKumpulanPelajar;
        private string csNoIc;
        private string csNamaPelajar;
        private string csNilaiBersih;
        private string csTarikhTransaksi;
        private string csTarikhLupusWarran;
        private string csFiller;
        private string csNoPelajar;
        private string csInternal_Use;
        private string csSponsorCode;
        private int _StudentQty;
        //added by farid on 12042016
        private string csMatricNumber;

        [System.Xml.Serialization.XmlElement]
        ////[DataMember]
        public string NoPelajar
        {
            get { return csNoPelajar; }
            set { csNoPelajar = value; }
        }

        [System.Xml.Serialization.XmlElement]
        ////[DataMember]
        public string AmaunPotongan
        {
            get { return csAmaunPotongan; }
            set { csAmaunPotongan = value; }
        }

        [System.Xml.Serialization.XmlElement]
        ////[DataMember]
        public string Filler
        {
            get { return csFiller; }
            set { csFiller = value; }
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
        public string NamaPelajar
        {
            get { return csNamaPelajar; }
            set { csNamaPelajar = value; }
        }

        [System.Xml.Serialization.XmlElement]
        ////[DataMember]
        public string NoIC
        {
            get { return csNoIc; }
            set { csNoIc = value; }
        }

        [System.Xml.Serialization.XmlElement]
        ////[DataMember]
        public string KumpulanPelajar
        {
            get { return csKumpulanPelajar; }
            set { csKumpulanPelajar = value; }
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
        public string noAkaun
        {
            get { return csNoAkaun; }
            set { csNoAkaun = value; }
        }

        [System.Xml.Serialization.XmlElement]
        ////[DataMember]
        public double AmaunWarran
        {
            get { return csAmaunWarran; }
            set { csAmaunWarran = value; }
        }

        [System.Xml.Serialization.XmlElement]
        ////[DataMember]
        public string NoWarran
        {
            get { return csNoWarran; }
            set { csNoWarran = value; }
        }

        [System.Xml.Serialization.XmlElement]
        ////[DataMember]
        public string NoKelompok
        {
            get { return csnoKelompok; }
            set { csnoKelompok = value; }
        }
        [System.Xml.Serialization.XmlElement]
        ////[DataMember]
        public string ReferenceThree
        {
            get { return csRef3; }
            set { csRef3 = value; }
        }
        [System.Xml.Serialization.XmlElement]
        ////[DataMember]
        public int TransactionID
        {
            get { return ciTransID; }
            set { ciTransID = value; }
        }
        public StudentEn Sudentacc
        {
            get { return enstudent; }
            set { enstudent = value; }
        }

        public FeeTypesEn Feetype
        {
            get { return enFEETYPE; }
            set { enFEETYPE = value; }
        }
        public List<StudentEn> Sudentacclist
        {
            get { return enstudentlist; }
            set { enstudentlist = value; }
        }
        public string VoucherNo
        {
            get { return csVoucherNo; }
            set { csVoucherNo = value; }
        }

        [System.Xml.Serialization.XmlElement]
        ////[DataMember]
        public string TransTempCode
        {
            get { return csTransTempCode; }
            set { csTransTempCode = value; }
        }


        [System.Xml.Serialization.XmlElement]
        ////[DataMember]
        public string TransactionCode
        {
            get { return csTransCode; }
            set { csTransCode = value; }
        }


        [System.Xml.Serialization.XmlElement]
        ////[DataMember]
        public string ReferenceCode
        {
            get { return csRefCode; }
            set { csRefCode = value; }
        }


        [System.Xml.Serialization.XmlElement]
        ////[DataMember]
        public string Quantity
        {
            get { return csQuantity; }
            set { csQuantity = value; }
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
        public double PaidAmount
        {
            get { return cdPaidAmount; }
            set { cdPaidAmount = value; }
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
        public string TransStatus
        {
            get { return csTransStatus; }
            set { csTransStatus = value; }
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
        public string ReferenceOne
        {
            get { return csRef1; }
            set { csRef1 = value; }
        }


        [System.Xml.Serialization.XmlElement]
        ////[DataMember]
        public string ReferenceTwo
        {
            get { return csRef2; }
            set { csRef2 = value; }
        }


        [System.Xml.Serialization.XmlElement]
        ////[DataMember]
        public int Priority
        {
            get { return ciPriority; }
            set { ciPriority = value; }
        }

        
        public string MatricNo
        {
            get { return Sudentacc.MatricNo; }
            set { Sudentacc.MatricNo = value; }
        }
        public string StudentName
        {
            get { return Sudentacc.StudentName; }
            set { Sudentacc.StudentName = value; }
        }
        public string ICNo
        {
            get { return Sudentacc.ICNo; }
            set { Sudentacc.ICNo = value; }
        }
        public int CurrentSemester
        {
            get { return Sudentacc.CurrentSemester; }
            set { Sudentacc.CurrentSemester = value; }
        }
        public string ProgramID
        {
            get { return Sudentacc.ProgramID; }
            set { Sudentacc.ProgramID = value; }
        }
        //for accountdetails in receipts
        public string CreditRef
        {
            get { return csCreditRef; }
            set { csCreditRef = value; }
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
        public float TransAmount
        {
            get { return spTransAmount; }
            set { spTransAmount = value; }
        }

        [System.Xml.Serialization.XmlElement]
        public string Internal_Use
        {
            get { return csInternal_Use; }
            set { csInternal_Use = value; }
        }
        //end
        [System.Xml.Serialization.XmlElement]
        public string SponsorCode
        {
            get { return csSponsorCode; }
            set { csSponsorCode = value; }
        }

        [System.Xml.Serialization.XmlElement]
        public int StudentQty
        {
            get { return _StudentQty; }
            set { _StudentQty = value; }
        }

        //added by farid on 12042016
        public string MatricNumber
        {
            get { return csMatricNumber; }
            set { csMatricNumber = value; }
        }
    }
}


