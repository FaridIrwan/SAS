using System;
using System.Collections.Generic;
using System.Text;
using System.ServiceModel;
using System.Runtime.Serialization;

namespace HTS.SAS.Entities
{
    [System.SerializableAttribute()]
    //[DataContract]
    public class PTPTNSetupEn
    {
        private int csId;
        private decimal csMin_balance;

        [System.Xml.Serialization.XmlElement]
        //[DataMember]
        public int id
        {
            get { return csId; }
            set { csId = value; }
        }

        [System.Xml.Serialization.XmlElement]
        //[DataMember]
        public decimal min_balance
        {
            get { return csMin_balance; }
            set { csMin_balance = value; }
        }
    }
}
