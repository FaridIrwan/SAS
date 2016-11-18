using System;
using System.Collections.Generic;
using System.Text;
using System.ServiceModel;
using System.Runtime.Serialization;

namespace HTS.SAS.Entities
{
    [System.SerializableAttribute()]
    //[DataContract]
    public class SponsorEn:AccountsEn
    {
        private string csSASR_Code;
        private string csSASR_Name;
        private string csSASSR_SName;
        private string csSASR_Address;
        private string csSASR_Address1;
        private string csSASR_Address2;
        private string csSASR_Contact;
        private string csSASR_Phone;
        private string csSASR_Fax;
        private string csSASR_Email;
        private string csSASR_WebSite;
        private string csSASR_Type;
        private string csSASR_Desc;
        private string csSASR_GLAccount;
        private int ciSABR_Code;
        private string csSASR_UpdatedBy;
        private string csSASR_UpdatedDtTm;
        private bool cbSASR_Status;
        private string Tmp_no;
        private List<SponsorFeeTypesEn> lstSponserFeeTypes;
        private bool sasr_ptptn;
       
        [System.Xml.Serialization.XmlElement]
        ////[DataMember]
        public string SponserCode
        {
            get { return csSASR_Code; }
            set { csSASR_Code = value; }
        }


        [System.Xml.Serialization.XmlElement]
        ////[DataMember]
        public string Name
        {
            get { return csSASR_Name; }
            set { csSASR_Name = value; }
        }


        [System.Xml.Serialization.XmlElement]
        ////[DataMember]
        public string SName
        {
            get { return csSASSR_SName; }
            set { csSASSR_SName = value; }
        }


        [System.Xml.Serialization.XmlElement]
        ////[DataMember]
        public string Address
        {
            get { return csSASR_Address; }
            set { csSASR_Address = value; }
        }


        [System.Xml.Serialization.XmlElement]
        ////[DataMember]
        public string Address1
        {
            get { return csSASR_Address1; }
            set { csSASR_Address1 = value; }
        }


        [System.Xml.Serialization.XmlElement]
        ////[DataMember]
        public string Address2
        {
            get { return csSASR_Address2; }
            set { csSASR_Address2 = value; }
        }


        [System.Xml.Serialization.XmlElement]
        ////[DataMember]
        public string Contact
        {
            get { return csSASR_Contact; }
            set { csSASR_Contact = value; }
        }


        [System.Xml.Serialization.XmlElement]
        ////[DataMember]
        public string Phone
        {
            get { return csSASR_Phone; }
            set { csSASR_Phone = value; }
        }


        [System.Xml.Serialization.XmlElement]
        ////[DataMember]
        public string Fax
        {
            get { return csSASR_Fax; }
            set { csSASR_Fax = value; }
        }


        [System.Xml.Serialization.XmlElement]
        ////[DataMember]
        public string Email
        {
            get { return csSASR_Email; }
            set { csSASR_Email = value; }
        }


        [System.Xml.Serialization.XmlElement]
        ////[DataMember]
        public string WebSite
        {
            get { return csSASR_WebSite; }
            set { csSASR_WebSite = value; }
        }


        [System.Xml.Serialization.XmlElement]
        ////[DataMember]
        public string Type
        {
            get { return csSASR_Type; }
            set { csSASR_Type = value; }
        }


        [System.Xml.Serialization.XmlElement]
        ////[DataMember]
        public string Description
        {
            get { return csSASR_Desc; }
            set { csSASR_Desc = value; }
        }


        [System.Xml.Serialization.XmlElement]
        ////[DataMember]
        public string GLAccount
        {
            get { return csSASR_GLAccount; }
            set { csSASR_GLAccount = value; }
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
        public string UpdatedBy
        {
            get { return csSASR_UpdatedBy; }
            set { csSASR_UpdatedBy = value; }
        }


        [System.Xml.Serialization.XmlElement]
        ////[DataMember]
        public string UpdatedDtTm
        {
            get { return csSASR_UpdatedDtTm; }
            set { csSASR_UpdatedDtTm = value; }
        }


        [System.Xml.Serialization.XmlElement]
        ////[DataMember]
        public bool Status
        {
            get { return cbSASR_Status; }
            set { cbSASR_Status = value; }
        }

        [System.Xml.Serialization.XmlElement]
        ////[DataMember]
        public List<SponsorFeeTypesEn>  LstSponserFeeTypes
        {
            get { return lstSponserFeeTypes; }
            set { lstSponserFeeTypes = value; }
        }
        public string TempSpnNo
        {
            get { return Tmp_no; }
            set { Tmp_no = value; }
        }

        public bool ptptn
        {
            get { return sasr_ptptn; }
            set { sasr_ptptn = value; }
        }

    }
}
