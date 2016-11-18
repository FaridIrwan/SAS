using System;
using System.Collections.Generic;
using System.Text;
using System.ServiceModel;
using System.Runtime.Serialization;

namespace HTS.SAS.Entities
{
    [System.SerializableAttribute()]
    //[DataContract]
    public class StudentSponEn:SponsorEn
    {
        private string csSASI_MatricNo;
        private string csSASS_Sponsor;
        private string csSASS_SDate;
        private string csSASS_EDate;
        private bool cbSASS_Status;
        private int ciSASS_Num;
        private bool csSASS_Type;
        private List<StuSponFeeTypesEn> lstStuSponFeetype;
        private double csSASS_Limit;

        [System.Xml.Serialization.XmlElement]
        //[DataMember]
        public string MatricNo
        {
            get { return csSASI_MatricNo; }
            set { csSASI_MatricNo = value; }
        }


        [System.Xml.Serialization.XmlElement]
        //[DataMember]
        public string Sponsor
        {
            get { return csSASS_Sponsor; }
            set { csSASS_Sponsor = value; }
        }


        [System.Xml.Serialization.XmlElement]
        //[DataMember]
        public string SDate
        {
            get { return csSASS_SDate; }
            set { csSASS_SDate = value; }
        }


        [System.Xml.Serialization.XmlElement]
        //[DataMember]
        public string EDate
        {
            get { return csSASS_EDate; }
            set { csSASS_EDate = value; }
        }


        [System.Xml.Serialization.XmlElement]
        //[DataMember]
        public bool Status
        {
            get { return cbSASS_Status; }
            set { cbSASS_Status = value; }
        }


        [System.Xml.Serialization.XmlElement]
        //[DataMember]
        public int Num
        {
            get { return ciSASS_Num; }
            set { ciSASS_Num = value; }
        }

        [System.Xml.Serialization.XmlElement]
        //[DataMember]
        public bool FullySponsered
        {
            get { return csSASS_Type; }
            set { csSASS_Type = value; }
        }

        [System.Xml.Serialization.XmlElement]
        //[DataMember]
        public List<StuSponFeeTypesEn> ListStuSponFeeTypes
        {
            get { return lstStuSponFeetype; }
            set { lstStuSponFeetype = value; }
        }

        [System.Xml.Serialization.XmlElement]
        //[DataMember]
        public double SponsorLimit
        {
            get { return csSASS_Limit; }
            set { csSASS_Limit = value; }
        }
    }
}
