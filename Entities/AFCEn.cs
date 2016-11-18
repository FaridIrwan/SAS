using System;
using System.Collections.Generic;
using System.Text;
using System.ServiceModel;
using System.Runtime.Serialization;

namespace HTS.SAS.Entities
{
    [System.SerializableAttribute()]
    //[DataContract]
    public class AFCEn
    {
        private int ciTransCode;
        private string csAFCode;
        private string csSAFC_Code;
        private string csBatchCode;
        private string csDescription;
        private DateTime coBdate;
        private DateTime coTransDate;
        private DateTime coDueDate;
        private double cdBatchTotal;
        private string csPostedFor;
        private string csReference;
        private string csUpdatedby;
        private string csUpdatetime;
        private List<AFCDetailsEn> listafcdetails;
        private List<ProgramInfoEn> listprogram;
        private string csSemester;
        private string csCreditRef;
        private string csSASI_Name;
        private int csTransID;
        private string csTransCode;
        private string csDescriptionAFC;
        private string csCategory;
        private string csTransType;
        private double csTransAmount;
        private string csSAFT_Desc;
        private string csSAFT_Code;
        private string csPostStatus;
        private string csSASI_CurSemYr;

        public List<AFCDetailsEn> AFCDetailslist
        {
            get { return listafcdetails; }
            set { listafcdetails = value; }
        }
        public List<ProgramInfoEn> Programlist
        {
            get { return listprogram; }
            set { listprogram = value; }
        }

        [System.Xml.Serialization.XmlElement]
        //[DataMember]
        public string Semester
        {
            get { return csSemester; }
            set { csSemester = value; }
        }
        public string PostStatus
        {
            get { return csPostStatus; }
            set { csPostStatus = value; }
        }
        [System.Xml.Serialization.XmlElement]
        //[DataMember]
        public int TransCode
        {
            get { return ciTransCode; }
            set { ciTransCode = value; }
        }


        [System.Xml.Serialization.XmlElement]
        //[DataMember]
        public string AFCode
        {
            get { return csAFCode; }
            set { csAFCode = value; }
        }


        [System.Xml.Serialization.XmlElement]
        //[DataMember]
        public string SAFC_Code
        {
            get { return csSAFC_Code; }
            set { csSAFC_Code = value; }
        }


        [System.Xml.Serialization.XmlElement]
        //[DataMember]
        public string BatchCode
        {
            get { return csBatchCode; }
            set { csBatchCode = value; }
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
        public DateTime Bdate
        {
            get { return coBdate; }
            set { coBdate = value; }
        }


        [System.Xml.Serialization.XmlElement]
        //[DataMember]
        public DateTime TransDate
        {
            get { return coTransDate; }
            set { coTransDate = value; }
        }


        [System.Xml.Serialization.XmlElement]
        //[DataMember]
        public DateTime DueDate
        {
            get { return coDueDate; }
            set { coDueDate = value; }
        }


        [System.Xml.Serialization.XmlElement]
        //[DataMember]
        public string PostedFor
        {
            get { return csPostedFor; }
            set { csPostedFor = value; }
        }
        [System.Xml.Serialization.XmlElement]
        ////[DataMember]
        public double BatchTotal
        {
            get { return cdBatchTotal; }
            set { cdBatchTotal = value; }
        }


        [System.Xml.Serialization.XmlElement]
        //[DataMember]
        public string Reference
        {
            get { return csReference; }
            set { csReference = value; }
        }


        [System.Xml.Serialization.XmlElement]
        //[DataMember]
        public string Updatedby
        {
            get { return csUpdatedby; }
            set { csUpdatedby = value; }
        }


        [System.Xml.Serialization.XmlElement]
        //[DataMember]
        public string Updatetime
        {
            get { return csUpdatetime; }
            set { csUpdatetime = value; }
        }

        [System.Xml.Serialization.XmlElement]
        //[DataMember]
        public string CreditRef
        {
            get { return csCreditRef; }
            set { csCreditRef = value; }
        }

        [System.Xml.Serialization.XmlElement]
        //[DataMember]
        public string SASI_Name
        {
            get { return csSASI_Name; }
            set { csSASI_Name = value; }
        }

        [System.Xml.Serialization.XmlElement]
        //[DataMember]
        public int TransID
        {
            get { return csTransID; }
            set { csTransID = value; }
        }

        [System.Xml.Serialization.XmlElement]
        //[DataMember]
        public string TransCodeAFC
        {
            get { return csTransCode; }
            set { csTransCode = value; }
        }

        [System.Xml.Serialization.XmlElement]
        //[DataMember]
        public string DescriptionAFC
        {
            get { return csDescriptionAFC; }
            set { csDescriptionAFC = value; }
        }

        [System.Xml.Serialization.XmlElement]
        //[DataMember]
        public string Category
        {
            get { return csCategory; }
            set { csCategory = value; }
        }

        [System.Xml.Serialization.XmlElement]
        //[DataMember]
        public string TransType
        {
            get { return csTransType; }
            set { csTransType = value; }
        }

        [System.Xml.Serialization.XmlElement]
        //[DataMember]
        public double TransAmount
        {
            get { return csTransAmount; }
            set { csTransAmount = value; }
        }

        [System.Xml.Serialization.XmlElement]
        //[DataMember]
        public string SAFT_Code
        {
            get { return csSAFT_Code; }
            set { csSAFT_Code = value; }
        }

        [System.Xml.Serialization.XmlElement]
        //[DataMember]
        public string SAFT_Desc
        {
            get { return csSAFT_Desc; }
            set { csSAFT_Desc = value; }
        }

        [System.Xml.Serialization.XmlElement]
        //[DataMember]
        public string CurrentSemester
        {
            get { return csSASI_CurSemYr; }
            set { csSASI_CurSemYr = value; }
        }

    }
}
//---------------------------------------------------------------------------------

