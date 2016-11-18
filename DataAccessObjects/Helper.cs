#region NameSpaces 

using System;
using MaxGeneric;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

#endregion

namespace HTS.SAS.DataAccessObjects
{
    public class Helper
    {
        #region Get DataBase Type 

        public static short GetDataBaseType
        {
            get
            {
               return MaxModule.Helper.GetDataBaseType;
            }
        }

        #endregion

        #region Get Connection String 

        public static string GetConnectionString()
        {
            if (GetDataBaseType == (short)(MaxModule.Helper.DataBaseType.PostGres))
            {
                return clsGeneric.NullToString(
                    ConfigurationManager.AppSettings["PG_CONNNECTION_STR"]);
            }
            else if (GetDataBaseType == (short)(MaxModule.Helper.DataBaseType.SqlServer))
            {
                return clsGeneric.NullToString(
                    ConfigurationManager.AppSettings["SQL_CONNNECTION_STR"]);
            }

            return string.Empty;
        }

        #endregion

        #region Date Conversion 

        public static string DateConversion(DateTime _DateTime)
        {
            return String.Format("{0:u}", _DateTime);  
        }

        #endregion

        #region Enumerators 

        public enum WorkflowStatus
        {
            Received = 1,
            Apporved = 2,
            Rejected = 3,
            Posted = 4,
            Failed = 5
        }

        public enum TaxMode
        {
            Exclusive = 1,
            Inclusive = 2
        }
        
        public const string StuActive = "1";
        public const int StuRegistered = 2;
        #endregion
    }
}
