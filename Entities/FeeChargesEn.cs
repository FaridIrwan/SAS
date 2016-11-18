using System;
using System.Collections.Generic;
using System.Text;
using System.ServiceModel;
using System.Runtime.Serialization;

namespace HTS.SAS.Entities
{
    [System.SerializableAttribute()]
    //[DataContract]
    public class FeeChargesEn
    {
        private string csSAFT_Code;
        private string csSASC_Code;
        private double cdSAFS_Amount;
        private string csSASC_Desc;       
        //Author		: Anil Kumar - T-Melmax Sdn Bhd
        //Created Date	: 12/06/2015
        private double csSASC_GSTAmount;
        //Created By: Noor Aslinamona (T-Melmax)   Created Date: 23/12/2015
        private double cdLocal_Amount;
        private double cdNonLocal_Amount;
        private double cdLocal_GSTAmount;
        private double cdNonLocal_GSTAmount;
        //Created By : Jessica
        //Created Date : 19/02/16
        private double cdLocalTemp_Amount;
        private double cdNonLocalTemp_Amount;
        private string cdLocalCategory;
        private string cdNonLocalCategory;
        //Created By: Farid   Created Date: 06/04/2015
        private List<KokoEn> lstkokoenWithAmount;

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
        public double FSAmount
        {
            get { return cdSAFS_Amount; }
            set { cdSAFS_Amount = value; }
        }

        //  Author			: Anil Kumar - T-Melmax Sdn Bhd
        //Created Date	: 12/06/2015
        [System.Xml.Serialization.XmlElement]
        ////[DataMember]
        public double GSTAmount
        {
            get { return csSASC_GSTAmount; }
            set { csSASC_GSTAmount = value; }
        }

        public double LocalAmount
        {
            get { return cdLocal_Amount; }
            set { cdLocal_Amount = value; }
        }

        public double NonLocalAmount
        {
            get { return cdNonLocal_Amount; }
            set { cdNonLocal_Amount = value; }
        }

        public double LocalGSTAmount
        {
            get { return cdLocal_GSTAmount; }
            set { cdLocal_GSTAmount = value; }
        }

        public double NonLocalGSTAmount
        {
            get { return cdNonLocal_GSTAmount; }
            set { cdNonLocal_GSTAmount = value; }
        }

        public double LocalTempAmount
        {
            get { return cdLocalTemp_Amount; }
            set { cdLocalTemp_Amount = value; }
        }

        public double NonLocalTempAmount
        {
            get { return cdNonLocalTemp_Amount; }
            set { cdNonLocalTemp_Amount = value; }
        }

        public string LocalCategory
        {
            get { return cdLocalCategory; }
            set { cdLocalCategory = value; }
        }

        public string NonLocalCategory
        {
            get { return cdNonLocalCategory; }
            set { cdNonLocalCategory = value; }
        }

        public List<KokoEn> lstkokoen
        {
            get { return lstkokoenWithAmount; }
            set { lstkokoenWithAmount = value; }
        }
    }
}
