using System;
using System.Collections.Generic;
using System.Text;
using System.ServiceModel;
using System.Runtime.Serialization;

namespace HTS.SAS.Entities
{
    [System.SerializableAttribute()]
    //[DataContract]
    public class ChequeEn:AccountsEn
    {
        private string csProcessID;
        private string csPaymentNo;
        private string csPaymentType;
        private string csDescription;
        private DateTime coChequeDate;
        private DateTime coTransactionDate;
        private string csPrintStatus;
        private string csUpdatedBy;
        private DateTime coUpdatedTime;
        private List<ChequeDetailsEn> enchequedetailslist;
        private List<AccountsEn> enaccounts;

        [System.Xml.Serialization.XmlElement]
        //[DataMember]
        public List<ChequeDetailsEn> ChequeDetailslist
        {
            get { return enchequedetailslist; }
            set { enchequedetailslist = value; }
        }
        [System.Xml.Serialization.XmlElement]
        //[DataMember]
        public List<AccountsEn> AcccountChques
        {
            get { return enaccounts; }
            set { enaccounts = value; }
        }
        [System.Xml.Serialization.XmlElement]
        //[DataMember]
        public string ProcessID
        {
            get { return csProcessID; }
            set { csProcessID = value; }
        }


        [System.Xml.Serialization.XmlElement]
        //[DataMember]
        public string PaymentNo
        {
            get { return csPaymentNo; }
            set { csPaymentNo = value; }
        }


        [System.Xml.Serialization.XmlElement]
        //[DataMember]
        public string PaymentType
        {
            get { return csPaymentType; }
            set { csPaymentType = value; }
        }


        [System.Xml.Serialization.XmlElement]
        //[DataMember]
        public string Description
        {
            get { return csDescription; }
            set { csDescription = value; }
        }


        [System.Xml.Serialization.XmlElement]
        //[DataMember]
        public DateTime ChequeDate
        {
            get { return coChequeDate; }
            set { coChequeDate = value; }
        }


        [System.Xml.Serialization.XmlElement]
        //[DataMember]
        public DateTime TransactionDate
        {
            get { return coTransactionDate; }
            set { coTransactionDate = value; }
        }


        [System.Xml.Serialization.XmlElement]
        //[DataMember]
        public string PrintStatus
        {
            get { return csPrintStatus; }
            set { csPrintStatus = value; }
        }


        [System.Xml.Serialization.XmlElement]
        //[DataMember]
        public string UpdatedBy
        {
            get { return csUpdatedBy; }
            set { csUpdatedBy = value; }
        }


        [System.Xml.Serialization.XmlElement]
        //[DataMember]
        public DateTime UpdatedTime
        {
            get { return coUpdatedTime; }
            set { coUpdatedTime = value; }
        }

    }
}


