using System;
using System.Collections.Generic;
using System.Text;
using System.ServiceModel;
using System.Runtime.Serialization;

namespace HTS.SAS.Entities
{
   public class  ConnectionEn
    {
       private string ceConnectionStrings;
       private string ceCode;

       public string Code
       {
           get { return ceCode; }
           set { ceCode = value; }
       }


       public string ConnectionStrings
       {
           get { return ceConnectionStrings; }
           set { ceConnectionStrings = value; }
       }
    }
}
