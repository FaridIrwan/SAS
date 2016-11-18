using System;
using System.Collections.Generic;
using System.Text;
using System.ServiceModel;
using System.Runtime.Serialization;

namespace HTS.SAS.Entities
{
   public class  MenuEn
    {
       private string cbMenuName;
       private int ciMenuId;

       public int MenuId
       {
           get { return ciMenuId; }
           set { ciMenuId = value; }
       }


       public string MenuName
       {
           get { return cbMenuName; }
           set { cbMenuName = value; }
       }
    }
}
