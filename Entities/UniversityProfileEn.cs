using System;
using System.Collections.Generic;
using System.Text;
using System.ServiceModel;
using System.Runtime.Serialization;

namespace HTS.SAS.Entities
{
    [System.SerializableAttribute()]
    //[DataContract]
    public class UniversityProfileEn
    {
        private string csSAUP_Code;
        private string csSAUP_Name;
        private string csSAUP_SName;
        private string csSAUP_Adress;
        private string csSAUP_Adress1;
        private string csSAUP_Adress2;
        private string csSAUP_City;
        private string csSAUP_State;
        private string csSAUP_Country;
        private string csSAUP_PostCode;
        private string csSAUP_Phone;
        private string csSAUP_Fax;
        private string csSAUP_Email;
        private string csSAUP_Website;
        private string csSAUP_Logo;
        private int ciSABR_Code;
        private string csSAUP_UpdatedUser;
        private string csSAUP_UpdatedDtTm;


        [System.Xml.Serialization.XmlElement]
        ////[DataMember]
        public string UniversityProfileCode
        {
            get { return csSAUP_Code; }
            set { csSAUP_Code = value; }
        }


        [System.Xml.Serialization.XmlElement]
        ////[DataMember]
        public string Name
        {
            get { return csSAUP_Name; }
            set { csSAUP_Name = value; }
        }


        [System.Xml.Serialization.XmlElement]
        ////[DataMember]
        public string SName
        {
            get { return csSAUP_SName; }
            set { csSAUP_SName = value; }
        }


        [System.Xml.Serialization.XmlElement]
        ////[DataMember]
        public string Adress
        {
            get { return csSAUP_Adress; }
            set { csSAUP_Adress = value; }
        }


        [System.Xml.Serialization.XmlElement]
        ////[DataMember]
        public string Adress1
        {
            get { return csSAUP_Adress1; }
            set { csSAUP_Adress1 = value; }
        }


        [System.Xml.Serialization.XmlElement]
        ////[DataMember]
        public string Adress2
        {
            get { return csSAUP_Adress2; }
            set { csSAUP_Adress2 = value; }
        }


        [System.Xml.Serialization.XmlElement]
        ////[DataMember]
        public string City
        {
            get { return csSAUP_City; }
            set { csSAUP_City = value; }
        }


        [System.Xml.Serialization.XmlElement]
        ////[DataMember]
        public string State
        {
            get { return csSAUP_State; }
            set { csSAUP_State = value; }
        }


        [System.Xml.Serialization.XmlElement]
        ////[DataMember]
        public string Country
        {
            get { return csSAUP_Country; }
            set { csSAUP_Country = value; }
        }


        [System.Xml.Serialization.XmlElement]
        ////[DataMember]
        public string PostCode
        {
            get { return csSAUP_PostCode; }
            set { csSAUP_PostCode = value; }
        }


        [System.Xml.Serialization.XmlElement]
        ////[DataMember]
        public string Phone
        {
            get { return csSAUP_Phone; }
            set { csSAUP_Phone = value; }
        }


        [System.Xml.Serialization.XmlElement]
        ////[DataMember]
        public string Fax
        {
            get { return csSAUP_Fax; }
            set { csSAUP_Fax = value; }
        }


        [System.Xml.Serialization.XmlElement]
        ////[DataMember]
        public string Email
        {
            get { return csSAUP_Email; }
            set { csSAUP_Email = value; }
        }


        [System.Xml.Serialization.XmlElement]
        ////[DataMember]
        public string Website
        {
            get { return csSAUP_Website; }
            set { csSAUP_Website = value; }
        }


        [System.Xml.Serialization.XmlElement]
        ////[DataMember]
        public string Logo
        {
            get { return csSAUP_Logo; }
            set { csSAUP_Logo = value; }
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
            get { return csSAUP_UpdatedUser; }
            set { csSAUP_UpdatedUser = value; }
        }


        [System.Xml.Serialization.XmlElement]
        ////[DataMember]
        public string UpdatedDtTm
        {
            get { return csSAUP_UpdatedDtTm; }
            set { csSAUP_UpdatedDtTm = value; }
        }

    }
}
