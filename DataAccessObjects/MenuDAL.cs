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
   public class MenuDAL
   {
       #region Global Declarations 

       private DbParameterCollection _DbParameterCollection = null;

       private MaxModule.DatabaseProvider _DatabaseFactory =
           new MaxModule.DatabaseProvider();

       private string DataBaseConnectionString = Helper.
          GetConnectionString();

       #endregion

        public MenuDAL()
        {
        }

        #region GetMenus 

        /// <summary>
        /// Method to Get Menus.
        /// </summary>
        /// <param name="argEn">Menu Entity is an Input.MenuID is an Input Property</param>
        /// <returns>List of Menu Entity</returns>
       public MenuEn GetMenus(MenuEn argEn)
        {
            MenuEn loEnList = new MenuEn();
            string sqlCmd = "select PageName from UR_MenuMaster where MenuID ='"+argEn.MenuId+"'";

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

       #region GetMenuMasterList

       public List<MenuMasterEn> GetMenuMasterList()
       {
           List<MenuMasterEn> loEnList = new List<MenuMasterEn>();

           string sqlCmd = "SELECT * FROM UR_MenuMaster";

           try
           {
               if (!FormHelp.IsBlank(sqlCmd))
               {
                   using (IDataReader loReader = _DatabaseFactory.ExecuteReader(Helper.GetDataBaseType,
                       DataBaseConnectionString, sqlCmd).CreateDataReader())
                   {
                       while (loReader.Read())
                       {
                           MenuMasterEn loItem = LoadObjectMenuMaster(loReader);
                           loEnList.Add(loItem);
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

       #region Load Object

       /// <summary>
        /// Method to Load Kolej Entity 
        /// </summary>
        /// <param name="argReader">IDataReader Object is an Input.</param>
        /// <returns>Returns Kolej Entity</returns>
        private MenuEn LoadObject(IDataReader argReader)
        {
            MenuEn loItem = new MenuEn();
            loItem.MenuName = GetValue<string>(argReader, "PageName");
            return loItem;
        }

        private MenuMasterEn LoadObjectMenuMaster(IDataReader argReader)
        {
            MenuMasterEn loItem = new MenuMasterEn();

            loItem.MenuID = GetValue<int>(argReader, "menuid");
            loItem.MenuName = GetValue<string>(argReader, "menuname");
            loItem.PageName = GetValue<string>(argReader, "pagename");
            loItem.PageDescription = GetValue<string>(argReader, "pagedescription");
            loItem.PageUrl = GetValue<string>(argReader, "pageurl");
            loItem.ImageUrl = GetValue<string>(argReader, "imageurl");
            loItem.Status = GetValue<bool>(argReader, "status");
            loItem.PageOrder = GetValue<int>(argReader, "pageorder");
            loItem.LastUpdatedBy = GetValue<string>(argReader, "lastupdatedby");
            loItem.LastUpdatedDtTm = GetValue<DateTime>(argReader, "lastupdateddttm");

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
