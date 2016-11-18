using System;
using System.Collections.Generic;
using System.Text;
using System.ServiceModel;
using System.Runtime.Serialization;

namespace HTS.SAS.Entities
{
    [System.SerializableAttribute()]
    //[DataContract]
    public class FacultyEn
    {
        private string csSAFC_Code;
        private string csSAFC_Desc;
        private string csSAFC_SName;
        private string csSAFC_Incharge;
        private string csSAFC_GlAccount;
        private bool cbSAFC_Status;
        private int ciSABR_Code;
        private string csSAFC_UpdatedBy;
        private string csSAFC_UpdatedDtTm;


        [System.Xml.Serialization.XmlElement]
        //[DataMember]
        public string SAFC_Code
        {
            get { return csSAFC_Code; }
            set { csSAFC_Code = value; }
        }


        [System.Xml.Serialization.XmlElement]
        //[DataMember]
        public string SAFC_Desc
        {
            get { return csSAFC_Desc; }
            set { csSAFC_Desc = value; }
        }


        [System.Xml.Serialization.XmlElement]
        //[DataMember]
        public string SAFC_SName
        {
            get { return csSAFC_SName; }
            set { csSAFC_SName = value; }
        }


        [System.Xml.Serialization.XmlElement]
        //[DataMember]
        public string SAFC_Incharge
        {
            get { return csSAFC_Incharge; }
            set { csSAFC_Incharge = value; }
        }


        [System.Xml.Serialization.XmlElement]
        //[DataMember]
        public string SAFC_GlAccount
        {
            get { return csSAFC_GlAccount; }
            set { csSAFC_GlAccount = value; }
        }


        [System.Xml.Serialization.XmlElement]
        //[DataMember]
        public bool SAFC_Status
        {
            get { return cbSAFC_Status; }
            set { cbSAFC_Status = value; }
        }


        [System.Xml.Serialization.XmlElement]
        //[DataMember]
        public int SABR_Code
        {
            get { return ciSABR_Code; }
            set { ciSABR_Code = value; }
        }


        [System.Xml.Serialization.XmlElement]
        //[DataMember]
        public string SAFC_UpdatedBy
        {
            get { return csSAFC_UpdatedBy; }
            set { csSAFC_UpdatedBy = value; }
        }


        [System.Xml.Serialization.XmlElement]
        //[DataMember]
        public string SAFC_UpdatedDtTm
        {
            get { return csSAFC_UpdatedDtTm; }
            set { csSAFC_UpdatedDtTm = value; }
        }

    }
}
//---------------------------------------------------------------------------------

