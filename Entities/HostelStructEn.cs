using System;
using System.Collections.Generic;
using System.Text;
using System.ServiceModel;
using System.Runtime.Serialization;

namespace HTS.SAS.Entities
{
    [System.SerializableAttribute()]
    //[DataContract]
    public class HostelStructEn : FeeTypesEn
    {
        private string csSAHS_Code;
        private string csSAHB_Code;
        private string csSAHB_Block;
        private string csSAHB_RoomTYpe;
        private string csSAHS_EffectFm;
        private bool cbSAFS_Status;
        private string csSAHS_UpdatedUser;
        private string csSAHS_UpdatedDtTm;
        private List<HostelStrDetailsEn> lstHostelStrDetials;
        private List<HostelStructEn> lstHostelStrDetailWithAmount;

        [System.Xml.Serialization.XmlElement]
        ////[DataMember]
        public string HostelStructureCode
        {
            get { return csSAHS_Code; }
            set { csSAHS_Code = value; }
        }


        [System.Xml.Serialization.XmlElement]
        ////[DataMember]
        public string Code
        {
            get { return csSAHB_Code; }
            set { csSAHB_Code = value; }
        }


        [System.Xml.Serialization.XmlElement]
        ////[DataMember]
        public string Block
        {
            get { return csSAHB_Block; }
            set { csSAHB_Block = value; }
        }


        [System.Xml.Serialization.XmlElement]
        ////[DataMember]
        public string RoomTYpe
        {
            get { return csSAHB_RoomTYpe; }
            set { csSAHB_RoomTYpe = value; }
        }


        [System.Xml.Serialization.XmlElement]
        ////[DataMember]
        public string EffectFm
        {
            get { return csSAHS_EffectFm; }
            set { csSAHS_EffectFm = value; }
        }


        [System.Xml.Serialization.XmlElement]
        ////[DataMember]
        public bool Status
        {
            get { return cbSAFS_Status; }
            set { cbSAFS_Status = value; }
        }


        [System.Xml.Serialization.XmlElement]
        ////[DataMember]
        public string UpdatedUser
        {
            get { return csSAHS_UpdatedUser; }
            set { csSAHS_UpdatedUser = value; }
        }


        [System.Xml.Serialization.XmlElement]
        ////[DataMember]
        public string UpdatedDtTm
        {
            get { return csSAHS_UpdatedDtTm; }
            set { csSAHS_UpdatedDtTm = value; }
        }
        [System.Xml.Serialization.XmlElement]
        ////[DataMember]
        public List<HostelStrDetailsEn> lstHFeeSD
        {
            get { return lstHostelStrDetials; }
            set { lstHostelStrDetials = value; }
        }

        [System.Xml.Serialization.XmlElement]
        ////[DataMember]
        public List<HostelStructEn> lstHFeeWithAmt
        {
            get { return lstHostelStrDetailWithAmount; }
            set { lstHostelStrDetailWithAmount = value; }
        }

    }
}
