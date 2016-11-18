using System;
using System.Collections.Generic;
using System.Text;
using System.ServiceModel;
using System.Runtime.Serialization;

namespace HTS.SAS.Entities
{
    [System.SerializableAttribute()]
    //[DataContract]
    public class StuNotesEn
    {
        private int ciSASN_Code;
        private string csSASI_MatricNo;
        private string csSASN_Remarks;
        private string csSASN_UpdatedBy;
        private string csSASN_UpdatedDtTm;


        [System.Xml.Serialization.XmlElement]
        //[DataMember]
        public int SASN_Code
        {
            get { return ciSASN_Code; }
            set { ciSASN_Code = value; }
        }


        [System.Xml.Serialization.XmlElement]
        //[DataMember]
        public string MatricNo
        {
            get { return csSASI_MatricNo; }
            set { csSASI_MatricNo = value; }
        }


        [System.Xml.Serialization.XmlElement]
        //[DataMember]
        public string Remarks
        {
            get { return csSASN_Remarks; }
            set { csSASN_Remarks = value; }
        }


        [System.Xml.Serialization.XmlElement]
        //[DataMember]
        public string SASN_UpdatedBy
        {
            get { return csSASN_UpdatedBy; }
            set { csSASN_UpdatedBy = value; }
        }


        [System.Xml.Serialization.XmlElement]
        //[DataMember]
        public string SASN_UpdatedDtTm
        {
            get { return csSASN_UpdatedDtTm; }
            set { csSASN_UpdatedDtTm = value; }
        }

    }
}
//---------------------------------------------------------------------------------

