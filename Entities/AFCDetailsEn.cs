
using System;
using System.Collections.Generic;
using System.Text;
using System.ServiceModel;
using System.Runtime.Serialization;

namespace HTS.SAS.Entities
{
    [System.SerializableAttribute()]
    //[DataContract]
    public class AFCDetailsEn
    {
        private int ciTransCode;
        private string csStudentsNo;
        private string csReferenceCode;
        private double cdTransAmount;
        private string efaculty;
        private string csSAPG_Program;
        private string csSAFC_Code;
        private string csSAFC_Semester;
        private string csSAFC_TransStatus;
        private string csSABP_Code;

        [System.Xml.Serialization.XmlElement]
        //[DataMember]
        public int TransCode
        {
            get { return ciTransCode; }
            set { ciTransCode = value; }
        }
        [System.Xml.Serialization.XmlElement]
        //[DataMember]
        public string Faculty
        {
            get { return efaculty; }
            set { efaculty = value; }
        }

        [System.Xml.Serialization.XmlElement]
        //[DataMember]
        public string NoOfStudents
        {
            get { return csStudentsNo; }
            set { csStudentsNo = value; }
        }


        [System.Xml.Serialization.XmlElement]
        //[DataMember]
        public string ProgramID
        {
            get { return csReferenceCode; }
            set { csReferenceCode = value; }
        }


        [System.Xml.Serialization.XmlElement]
        ////[DataMember]
        public double TransactionAmount
        {
            get { return cdTransAmount; }
            set { cdTransAmount = value; }
        }
        public string Program
        {
            get { return csSAPG_Program; }
            set { csSAPG_Program = value; }
        }
        public string Code
        {
            get { return csSAFC_Code; }
            set { csSAFC_Code = value; }
        }
        public string Semester
        {
            get { return csSAFC_Semester; }
            set { csSAFC_Semester = value; }
        }
        public string TransStatus
        {
            get { return csSAFC_TransStatus; }
            set { csSAFC_TransStatus = value; }
        }
        public string BidangCode
        {
            get { return csSABP_Code; }
            set { csSABP_Code = value; }
        }

    }
}
//---------------------------------------------------------------------------------

