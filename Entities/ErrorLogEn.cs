using System;
using System.Collections.Generic;
using System.Text;

namespace HTS.SAS.Entities
{
   public class ErrorLogEn
    {
        private string csSname;
        private string csMName;
        private string csMessage;
       private DateTime codate;

       public string ScreenName
       {
           get { return csSname; }
           set { csSname = value; }
       }
       public string MethodName
       {
           get { return csMName; }
           set { csMName = value; }

       }
       public string ErrorMessage
       {
           get { return csMessage; }
           set { csMessage = value; }
       }
       public DateTime DateTime
       {
           get { return codate; }
           set { codate = value; }
       }
    }
}
