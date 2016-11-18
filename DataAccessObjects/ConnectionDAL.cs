#region NameSpaces

using System;
using System.Data;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data.Common;
using HTS.SAS.Entities;
using MaxGeneric;

#endregion

namespace HTS.SAS.DataAccessObjects
{
    /// <summary>
    /// Class to handle all the Menus Methods.
    /// </summary>
   public class ConnectionDAL
    {
        #region Global Declarations 

        private DbParameterCollection _DbParameterCollection = null;

        private MaxModule.DatabaseProvider _DatabaseFactory =
            new MaxModule.DatabaseProvider();

        private string DataBaseConnectionString = Helper.
           GetConnectionString();

        #endregion

        public ConnectionDAL()
        {
        }

        #region GetConnectionString 

        /// <summary>
        /// Method to Get Menus.
        /// </summary>
        /// <param name="argEn">Menu Entity is an Input.MenuID is an Input Property</param>
        /// <returns>List of Menu Entity</returns>
        public ConnectionEn GetConnectionString(ConnectionEn argEn)
        {
            ConnectionEn loEnList = new ConnectionEn();
            string sqlCmd = "select CF_Conn from SAS_CF where CF_Code ='" + argEn.Code + "'";
            
            try
            {
                if (!FormHelp.IsBlank(sqlCmd))
                {
                    using (IDataReader loReader = _DatabaseFactory.ExecuteReader(Helper.GetDataBaseType,
                        DataBaseConnectionString, sqlCmd).CreateDataReader())
                    {
                        while (loReader.Read())
                        {
                            loEnList = LoadObject(loReader);
                        }
                        loReader.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return loEnList;
        }

        #endregion

        #region LoadObject 

        /// <summary>
        /// Method to Load Kolej Entity 
        /// </summary>
        /// <param name="argReader">IDataReader Object is an Input.</param>
        /// <returns>Returns Kolej Entity</returns>
        private ConnectionEn LoadObject(IDataReader argReader)
       {
           ConnectionEn loItem = new ConnectionEn();
           loItem.ConnectionStrings = GetValue<string>(argReader, "CF_Conn");
            return loItem;
       }
       private static T GetValue<T>(IDataReader argReader, string argColNm)
       {
           if (!argReader.IsDBNull(argReader.GetOrdinal(argColNm)))
               return (T)argReader.GetValue(argReader.GetOrdinal(argColNm));
           else
               return default(T);
       }

        #endregion
    }
}
