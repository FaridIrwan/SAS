using System;
using System.Collections.Generic;
using System.Text;
using System.ServiceModel;
using System.Runtime.Serialization;

namespace HTS.SAS.Entities
{
    [System.SerializableAttribute()]
    //[DataContract]
    public class ProgramAccountEn
    {
        private string csSAPA_Code;
        private string csSAPA_Desc;
        private bool cbSAPA_Status;
        private string csSAPG_Code;
        private string csSAPG_ProgramBM;
        private string csCode_Programe;

        [System.Xml.Serialization.XmlElement]
        ////[DataMember]
        public string ProgramAccountCode
        {
            get { return csSAPA_Code; }
            set { csSAPA_Code = value; }
        }


        [System.Xml.Serialization.XmlElement]
        ////[DataMember]
        public string ProgramAccDescription
        {
            get { return csSAPA_Desc; }
            set { csSAPA_Desc = value; }
        }


        [System.Xml.Serialization.XmlElement]
        ////[DataMember]
        public bool ProgramAccStatus
        {
            get { return cbSAPA_Status; }
            set { cbSAPA_Status = value; }
        }


        [System.Xml.Serialization.XmlElement]
        ////[DataMember]
        public string ProgramCode
        {
            get { return csSAPG_Code; }
            set { csSAPG_Code = value; }
        }

        [System.Xml.Serialization.XmlElement]
        ////[DataMember]
        public string CodeProgram
        {
            get { return csCode_Programe; }
            set { csCode_Programe = value; }
        }

        [System.Xml.Serialization.XmlElement]
        ////[DataMember]
        public string descProgram
        {
            get { return csSAPG_ProgramBM; }
            set { csSAPG_ProgramBM = value; }
        }

    }
}
//---------------------------------------------------------------------------------

