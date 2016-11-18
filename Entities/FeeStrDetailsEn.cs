using System;
using System.Collections.Generic;
using System.Text;
using System.ServiceModel;
using System.Runtime.Serialization;

namespace HTS.SAS.Entities
{
    [System.SerializableAttribute()]
    //[DataContract]
    public class FeeStrDetailsEn
    {

        private string id;
        private string csSAFS_Code;
        private string csSAFD_Type;
        private string csSAFT_Code;
        private int ciSAFD_Priority;
        private string csSAFD_FeeFor;
        private int ciSAFD_Sem;
        private string csFeeDesc;
        private int ciSAFD_TaxId;
        private List<FeeStrAmountEn> cslstFeeStrAmount;
        private FeeStructEn enfeestucture;


        public FeeStructEn feestructure
        {
            get { return enfeestucture; }
            set { enfeestucture = value; }
        }

        public string objecttype
        {
            get { return id; }
            set { id = value; }
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
        public string FeeDesc
        {
            get { return csFeeDesc; }
            set { csFeeDesc = value; }
        }

        [System.Xml.Serialization.XmlElement]
        ////[DataMember]
        public string Type
        {
            get { return csSAFD_Type; }
            set { csSAFD_Type = value; }
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
        public int Priority
        {
            get { return ciSAFD_Priority; }
            set { ciSAFD_Priority = value; }
        }


        [System.Xml.Serialization.XmlElement]
        ////[DataMember]
        public string FeeFor
        {
            get { return csSAFD_FeeFor; }
            set { csSAFD_FeeFor = value; }
        }


        [System.Xml.Serialization.XmlElement]
        ////[DataMember]
        public int Sem
        {
            get { return ciSAFD_Sem; }
            set { ciSAFD_Sem = value; }
        }
        [System.Xml.Serialization.XmlElement]
        ////[DataMember]
        public List<FeeStrAmountEn> ListFeeAmount
        {
            get { return cslstFeeStrAmount; }
            set { cslstFeeStrAmount = value; }
        }

        [System.Xml.Serialization.XmlElement]
        ////[DataMember]
        public int _TaxId
        {
            get { return ciSAFD_TaxId; }
            set { ciSAFD_TaxId = value; }
        }
    }
}
