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
    public class MenuBAL
    {
        /// <summary>
        /// Method to Get Menus.
        /// </summary>
        /// <param name="argEn">Menu Entity is an Input.MenuID is an Input Property</param>
        /// <returns>List of Menu Entity</returns>
        public MenuEn GetMenus(MenuEn argEn)
        {
            try
            {
                MenuDAL loDs = new MenuDAL();
                return loDs.GetMenus(argEn);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
