using System;
using System.Collections.Generic;
using System.Text;
using System.ServiceModel;
using System.Runtime.Serialization;

namespace HTS.SAS.Entities
{
    [System.SerializableAttribute()]
    //[DataContract]
    public class DunningLettersEn
    {
        private string csSADL_Code;
        private string csSADL_Title;
        private string csSADL_Ref;
        private string csSADL_Message;
        private string csSADL_SignBy;
        private string csSADL_Name;
        private DateTime coSADL_FrDate;
        private DateTime coSADL_ToDate;
        private string csSADL_UpdatedBy;
        private DateTime coSADL_UpdatedTime;
        private string csSAS_MatricNo;
        private string csSADL_Semester;
        private string csSADL_Warn;
        private string csSADL_InsertBy;
        private DateTime csSADL_InsertDate;
        private string csSADL_UpdateBy;
        private DateTime csSADL_UpdateDate;
        private string csSASI_ICNo;
        private string csSASI_PgID;
        private string csSASS_Code;

        [System.Xml.Serialization.XmlElement]
        //[DataMember]
        public string MatricNo
        {
            get { return csSAS_MatricNo; }
            set { csSAS_MatricNo = value; }
        }

        [System.Xml.Serialization.XmlElement]
        //[DataMember]
        public string Code
        {
            get { return csSADL_Code; }
            set { csSADL_Code = value; }
        }


        [System.Xml.Serialization.XmlElement]
        //[DataMember]
        public string Title
        {
            get { return csSADL_Title; }
            set { csSADL_Title = value; }
        }


        [System.Xml.Serialization.XmlElement]
        //[DataMember]
        public string Reference
        {
            get { return csSADL_Ref; }
            set { csSADL_Ref = value; }
        }


        [System.Xml.Serialization.XmlElement]
        //[DataMember]
        public string Message
        {
            get { return csSADL_Message; }
            set { csSADL_Message = value; }
        }


        [System.Xml.Serialization.XmlElement]
        //[DataMember]
        public string SignBy
        {
            get { return csSADL_SignBy; }
            set { csSADL_SignBy = value; }
        }


        [System.Xml.Serialization.XmlElement]
        //[DataMember]
        public string Name
        {
            get { return csSADL_Name; }
            set { csSADL_Name = value; }
        }


        [System.Xml.Serialization.XmlElement]
        //[DataMember]
        public DateTime FromDate
        {
            get { return coSADL_FrDate; }
            set { coSADL_FrDate = value; }
        }


        [System.Xml.Serialization.XmlElement]
        //[DataMember]
        public DateTime ToDate
        {
            get { return coSADL_ToDate; }
            set { coSADL_ToDate = value; }
        }


        [System.Xml.Serialization.XmlElement]
        //[DataMember]
        public string UpdatedBy
        {
            get { return csSADL_UpdatedBy; }
            set { csSADL_UpdatedBy = value; }
        }


        [System.Xml.Serialization.XmlElement]
        //[DataMember]
        public DateTime UpdatedTime
        {
            get { return coSADL_UpdatedTime; }
            set { coSADL_UpdatedTime = value; }
        }

        [System.Xml.Serialization.XmlElement]
        //[DataMember]
        public string Warning
        {
            get { return csSADL_Warn; }
            set { csSADL_Warn = value; }
        }

        [System.Xml.Serialization.XmlElement]
        //[DataMember]
        public string Semester
        {
            get { return csSADL_Semester; }
            set { csSADL_Semester = value; }
        }

        [System.Xml.Serialization.XmlElement]
        //[DataMember]
        public string InsertBy
        {
            get { return csSADL_InsertBy; }
            set { csSADL_InsertBy = value; }
        }

        [System.Xml.Serialization.XmlElement]
        //[DataMember]
        public DateTime InsertDate
        {
            get { return csSADL_InsertDate; }
            set { csSADL_InsertDate = value; }
        }

        [System.Xml.Serialization.XmlElement]
        //[DataMember]
        public string UpdateBy
        {
            get { return csSADL_UpdateBy; }
            set { csSADL_UpdateBy = value; }
        }

        [System.Xml.Serialization.XmlElement]
        //[DataMember]
        public DateTime UpdateDate
        {
            get { return csSADL_UpdateDate; }
            set { csSADL_UpdateDate = value; }
        }

        [System.Xml.Serialization.XmlElement]
        //[DataMember]
        public string ICNo
        {
            get { return csSASI_ICNo; }
            set { csSASI_ICNo = value; }
        }

        [System.Xml.Serialization.XmlElement]
        //[DataMember]
        public string pgID
        {
            get { return csSASI_PgID; }
            set { csSASI_PgID = value; }
        }

        [System.Xml.Serialization.XmlElement]
        //[DataMember]
        public string Status
        {
            get { return csSASS_Code; }
            set { csSASS_Code = value; }
        }

    }
}
//---------------------------------------------------------------------------------