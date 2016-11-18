using System;
using System.Collections.Generic;
using System.Text;
using System.ServiceModel;
using System.Runtime.Serialization;

namespace HTS.SAS.Entities
{
    [System.SerializableAttribute()]
    //[DataContract]
    public class PayModeEn
    {
        private string csSAPM_Code;
        private string csSAPM_Des;
        private bool cbSAPM_Status;


        [System.Xml.Serialization.XmlElement]
        ////[DataMember]
        public string SAPM_Code
        {
            get { return csSAPM_Code; }
            set { csSAPM_Code = value; }
        }


        [System.Xml.Serialization.XmlElement]
        ////[DataMember]
        public string SAPM_Des
        {
            get { return csSAPM_Des; }
            set { csSAPM_Des = value; }
        }


        [System.Xml.Serialization.XmlElement]
        ////[DataMember]
        public bool SAPM_Status
        {
            get { return cbSAPM_Status; }
            set { cbSAPM_Status = value; }
        }

    }
}
//---------------------------------------------------------------------------------

