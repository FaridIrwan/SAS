using System;
using System.Collections.Generic;
using System.Text;
using System.ServiceModel;
using System.Runtime.Serialization;

namespace HTS.SAS.Entities
{
    [System.SerializableAttribute()]
    //[DataContract]
    public class StuSponFeeTypesEn
    {
        private string csSASI_MatricNo;
        private string csSASR_Code;
        private string csSAFT_Code;
        private string csSAFT_Desc;

        [System.Xml.Serialization.XmlElement]
        //[DataMember]
        public string MatricNo
        {
            get { return csSASI_MatricNo; }
            set { csSASI_MatricNo = value; }
        }

        [System.Xml.Serialization.XmlElement]
        //[DataMember]
        public string FeeDesc
        {
            get { return csSAFT_Desc; }
            set { csSAFT_Desc = value; }
        }

        [System.Xml.Serialization.XmlElement]
        //[DataMember]
        public string SASR_Code
        {
            get { return csSASR_Code; }
            set { csSASR_Code = value; }
        }


        [System.Xml.Serialization.XmlElement]
        //[DataMember]
        public string SAFT_Code
        {
            get { return csSAFT_Code; }
            set { csSAFT_Code = value; }
        }

    }
}
