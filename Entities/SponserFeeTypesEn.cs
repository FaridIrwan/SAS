using System;
using System.Collections.Generic;
using System.Text;
using System.ServiceModel;
using System.Runtime.Serialization;

namespace HTS.SAS.Entities
{
    [System.SerializableAttribute()]
    //[DataContract]
    public class SponsorFeeTypesEn
    {
        private string csSASR_Code;
        private string csSAFT_Code;
        private string csSAFT_Desc;


        [System.Xml.Serialization.XmlElement]
        ////[DataMember]
        public string SponserCode
        {
            get { return csSASR_Code; }
            set { csSASR_Code = value; }
        }


        [System.Xml.Serialization.XmlElement]
        ////[DataMember]
        public string FeeTypeCode
        {
            get { return csSAFT_Code; }
            set { csSAFT_Code = value; }
        }

        [System.Xml.Serialization.XmlElement]
        ////[DataMember]
        public string FeeTypeDesc
        {
            get { return csSAFT_Desc; }
            set { csSAFT_Desc = value; }
        }
    }
}
