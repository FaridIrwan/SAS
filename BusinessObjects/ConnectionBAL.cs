using System;
using System.Text;
using System.Collections.Generic;
using HTS.SAS.Entities;
using HTS.SAS.DataAccessObjects;

namespace HTS.SAS.BusinessObjects
{
    /// <summary>
    /// Business Class to handle all the Menus Methods.
    /// </summary>
    public class ConnectionBAL
    {
        /// <summary>
        /// Method to Get ConnectionString.
        /// </summary>
        /// <param name="argEn">Menu Entity is an Input.MenuID is an Input Property</param>
        /// <returns>List of Menu Entity</returns>
        public ConnectionEn GetConnectionString(ConnectionEn argEn)
        {
            try
            {
                ConnectionDAL loDs = new ConnectionDAL();
                return loDs.GetConnectionString(argEn);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
