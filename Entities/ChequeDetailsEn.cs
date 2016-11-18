using System;
using System.Collections.Generic;
using System.Text;
using System.ServiceModel;
using System.Runtime.Serialization;

namespace HTS.SAS.Entities
{
    [System.SerializableAttribute()]
    //[DataContract]
    public class ChequeDetailsEn
    {
        private string csProcessId;
        private string csChequeStartNo;
        private string csChequeEndNo;
        private string csNumber;

        [System.Xml.Serialization.XmlElement]
        //[DataMember]
        public string ProcessId
        {
            get { return csProcessId; }
            set { csProcessId = value; }
        }
        public string Number
        {
            get { return csNumber; }
            set { csNumber = value; }
        }

        [System.Xml.Serialization.XmlElement]
        //[DataMember]
        public string ChequeStartNo
        {
            get { return csChequeStartNo; }
            set { csChequeStartNo = value; }
        }


        [System.Xml.Serialization.XmlElement]
        //[DataMember]
        public string ChequeEndNo
        {
            get { return csChequeEndNo; }
            set { csChequeEndNo = value; }
        }

    }
}
//---------------------------------------------------------------------------------

