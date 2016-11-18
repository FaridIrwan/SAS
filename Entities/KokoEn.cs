using System;
using System.Collections.Generic;
using System.Text;
using System.ServiceModel;
using System.Runtime.Serialization;

namespace HTS.SAS.Entities
{
    [System.SerializableAttribute()]
    //[DataContract]
    public class KokoEn : FeeChargesEn
    {
        private int sakod_id;
        private string sakod_categorycode;
        private string sakod_noncategorycode;
        private double sakod_feeamountlocalin;
        private double sakod_feeamountlocalout;
        private double sakod_gstlocalin;
        private double sakod_gstlocalout;
        private double sakod_feeamountinterin;
        private double sakod_feeamountinterout;
        private double sakod_gstinterin;
        private double sakod_gstinterout;
        private double total_feelocalin;
        private double total_feelocalout;
        private double total_feeinterin;
        private double total_feeinterout;
        private string sako_code;
        private string sakod_categoryname;
        private string saft_code;
        private double LocalTempAmountKoko;
        private double InterTempAmount;
        private List<FeeChargesEn> cslstFeeCharges;
        private List<KokoEn> lstkokoenWithAmount;
        private int tax_mode;
        
        [System.Xml.Serialization.XmlElement]
        ////[DataMember]
        public List<FeeChargesEn> ListFeeCharges
        {
            get { return cslstFeeCharges; }
            set { cslstFeeCharges = value; }
        }
        [System.Xml.Serialization.XmlElement]
        ////[DataMember]
        public double Local_TempAmount
        {
            get { return LocalTempAmountKoko; }
            set { LocalTempAmountKoko = value; }
        }

        [System.Xml.Serialization.XmlElement]
        ////[DataMember]
        public double Inter_TempAmount
        {
            get { return InterTempAmount; }
            set { InterTempAmount = value; }
        }

        [System.Xml.Serialization.XmlElement]
        ////[DataMember]
        public int sakod_idkoko
        {
            get { return sakod_id; }
            set { sakod_id = value; }
        }


        [System.Xml.Serialization.XmlElement]
        ////[DataMember]
        public string Category
        {
            get { return sakod_categorycode; }
            set { sakod_categorycode = value; }
        }

        [System.Xml.Serialization.XmlElement]
        ////[DataMember]
        public string NonLocalCategory
        {
            get { return sakod_noncategorycode; }
            set { sakod_noncategorycode = value; }
        }


        [System.Xml.Serialization.XmlElement]
        ////[DataMember]
        public double sakodfeeamountlocalin
        {
            get { return sakod_feeamountlocalin; }
            set { sakod_feeamountlocalin = value; }
        }

        [System.Xml.Serialization.XmlElement]
        ////[DataMember]
        public double sakodfeeamountlocalout
        {
            get { return sakod_feeamountlocalout; }
            set { sakod_feeamountlocalout = value; }
        }

        [System.Xml.Serialization.XmlElement]
        ////[DataMember]
        public double sakodgstamountlocalin
        {
            get { return sakod_gstlocalin; }
            set { sakod_gstlocalin = value; }
        }

        [System.Xml.Serialization.XmlElement]
        ////[DataMember]
        public double sakodfeegstamountlocalout
        {
            get { return sakod_gstlocalout; }
            set { sakod_gstlocalout = value; }
        }

        [System.Xml.Serialization.XmlElement]
        ////[DataMember]
        public double sakodfeeamountinterin
        {
            get { return sakod_feeamountinterin; }
            set { sakod_feeamountinterin = value; }
        }

        [System.Xml.Serialization.XmlElement]
        ////[DataMember]
        public double sakodfeeamountinterout
        {
            get { return sakod_feeamountinterout; }
            set { sakod_feeamountinterout = value; }
        }

        [System.Xml.Serialization.XmlElement]
        ////[DataMember]
        public double sakodgstamountinterin
        {
            get { return sakod_gstinterin; }
            set { sakod_gstinterin = value; }
        }

        [System.Xml.Serialization.XmlElement]
        ////[DataMember]
        public double sakodgstamountinterout
        {
            get { return sakod_gstinterout; }
            set { sakod_gstinterout = value; }
        }

        [System.Xml.Serialization.XmlElement]
        ////[DataMember]
        public double totalfeelocalin
        {
            get { return total_feelocalin; }
            set { total_feelocalin = value; }
        }

        [System.Xml.Serialization.XmlElement]
        ////[DataMember]
        public double totalfeelocalout
        {
            get { return total_feelocalout; }
            set { total_feelocalout = value; }
        }

        [System.Xml.Serialization.XmlElement]
        ////[DataMember]
        public double totalfeeinterin
        {
            get { return total_feeinterin; }
            set { total_feeinterin = value; }
        }

        [System.Xml.Serialization.XmlElement]
        ////[DataMember]
        public double totalfeeinterout
        {
            get { return total_feeinterout; }
            set { total_feeinterout = value; }
        }

        [System.Xml.Serialization.XmlElement]
        ////[DataMember]
        public string Code
        {
            get { return sako_code; }
            set { sako_code = value; }
        }


        [System.Xml.Serialization.XmlElement]
        ////[DataMember]
        public string categoryname
        {
            get { return sakod_categoryname; }
            set { sakod_categoryname = value; }
        }


        [System.Xml.Serialization.XmlElement]
        ////[DataMember]
        public string Saftcode
        {
            get { return saft_code; }
            set { saft_code = value; }
        }

        [System.Xml.Serialization.XmlElement]
        ////[DataMember]
        public List<KokoEn> lstkokoen
        {
            get { return lstkokoenWithAmount; }
            set { lstkokoenWithAmount = value; }
        }

        [System.Xml.Serialization.XmlElement]
        ////[DataMember]
        public int taxmode
        {
            get { return tax_mode; }
            set { tax_mode = value; }
        }
    }
}

