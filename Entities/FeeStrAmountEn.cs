using System;
using System.Collections.Generic;
using System.Text;
using System.ServiceModel;
using System.Runtime.Serialization;

namespace HTS.SAS.Entities
{
    [System.SerializableAttribute()]
    //[DataContract]
    public class FeeStrAmountEn
    {
        private string csSAFS_Code;
        private string csSAFT_Code;
        private string csSASC_Code;
        private string csSAFD_Type;
        private double cdSAFA_Amount;
        private string csSASC_Desc;
        private int id;
        private FeeStrDetailsEn enfeestrdetails;
        private FeeStructEn enfeestruct;
        private string csSAFD_feefor;
        private int csSAFD_sem;
        
        //  Author			: Anil Kumar - T-Melmax Sdn Bhd
        //Created Date	: 12/06/2015
        private double cdSAFA_GSTAmount;

        public FeeStrDetailsEn FeestruDtetails
        {
            get { return enfeestrdetails; }
            set { enfeestrdetails = value; }
        }
        public FeeStructEn Feestruct
        {
            get { return enfeestruct; }
            set { enfeestruct = value; }
        }
        [System.Xml.Serialization.XmlElement]
        ////[DataMember]
        public string FSCode
        {
            get { return csSAFS_Code; }
            set { csSAFS_Code = value; }
        }
        [System.Xml.Serialization.XmlElement]
        ////[DataMember]
        public int objectID
        {
            get { return id; }
            set { id = value; }
        }


        [System.Xml.Serialization.XmlElement]
        ////[DataMember]
        public string FTCode
        {
            get { return csSAFT_Code; }
            set { csSAFT_Code = value; }
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
        public string SCDesc
        {
            get { return csSASC_Desc; }
            set { csSASC_Desc = value; }
        }

        [System.Xml.Serialization.XmlElement]
        ////[DataMember]
        public double FeeAmount
        {
            get { return cdSAFA_Amount; }
            set { cdSAFA_Amount = value; }
        }
        [System.Xml.Serialization.XmlElement]
        ////[DataMember]
        public string Type
        {
            get { return csSAFD_Type; }
            set { csSAFD_Type = value; }
        }

        //  Author			: Anil Kumar - T-Melmax Sdn Bhd
        //Created Date	: 12/06/2015
        [System.Xml.Serialization.XmlElement]
        ////[DataMember]
        public double GSTAmount
        {
            get { return cdSAFA_GSTAmount; }
            set { cdSAFA_GSTAmount = value; }
        }

        [System.Xml.Serialization.XmlElement]
        public string FeeFor
        {
            get { return csSAFD_feefor; }
            set { csSAFD_feefor = value; }
        }

        [System.Xml.Serialization.XmlElement]
        public int FeeDetailSem
        {
            get { return csSAFD_sem; }
            set { csSAFD_sem = value; }
        }
    }
}
