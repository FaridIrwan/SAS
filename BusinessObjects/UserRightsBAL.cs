using System;
using System.Text;
using System.Transactions;
using System.Collections.Generic;
using HTS.SAS.Entities;
using HTS.SAS.DataAccessObjects;

namespace HTS.SAS.BusinessObjects
{
    /// <summary>
    /// Business Class to handle all the UserRights Methods.
    /// </summary>
    public class UserRightsBAL
    {
/// <summary>
        /// Method to Get List of Menus By UserGroups
/// </summary>
        /// <param name="MenuName">MenuName as Input.</param>
        /// <param name="UserGroupID">UserGroupID  as Input.</param>
        /// <returns>Returns List of Menus</returns>
        public List<MenuMasterEn> GetMenuByUserGroup(string MenuName, int UserGroupID, string optional_params = "")
        {
            try
            {
                UserRightsDAL loDs = new UserRightsDAL();
                return loDs.GetMenuByUserGroup(MenuName, UserGroupID, optional_params);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        
        /// <summary>
        /// Method to Insert List of UserRights 
        /// </summary>
        /// <param name="argEn">List of UserRights Entity is an Input.</param>
        /// <returns>Returns Boolean</returns>
        public bool InsertUserRights(List<UserRightsEn> argEn)
        {
            bool flag;
            using (TransactionScope ts = new TransactionScope())
            {
                try
                {
                    UserRightsDAL loDs = new UserRightsDAL();
                    flag = loDs.InsertUserRights(argEn);
                    ts.Complete();
                }
                catch (Exception ex)
                {
                    
                    throw ex;
                }
            }
            return flag;
        }
        /// <summary>
        /// Method to Get List of UserRights by GroupID
        /// </summary>
        /// <param name="UserGroupID">UserGroupID  as an Input.</param>
        /// <returns>Returns List of UserRights</returns>
        public List<UserRightsEn> GetUserMenu(int UserGroupID)
        {
            try
            {
                UserRightsDAL loDs = new UserRightsDAL();
                return loDs.GetUserMenu(UserGroupID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Method to Get List of All UserRights  by UserGroupID
        /// </summary>
        /// <param name="argEn">UserGroupID  as an Input.</param>
        /// <returns>Returns List of UserRights</returns>
        public List<UserRightsEn> GetList(UserRightsEn argEn)
        {
            try
            {
                UserRightsDAL loDs = new UserRightsDAL();
                return loDs.GetList(argEn);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Method to Get UserRights Entity
        /// </summary>
        /// <param name="argEn">UserRights Entity is an Input.</param>
        /// <returns>Returns UserRights Entity</returns>
        public UserRightsEn GetItem(UserRightsEn argEn)
        {
            try
            {
                UserRightsDAL loDs = new UserRightsDAL();
                return loDs.GetItem(argEn);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Method to Insert UserRights 
        /// </summary>
        /// <param name="argEn">UserRights Entity is an Input.</param>
        /// <returns>Returns Boolean</returns>
        public bool Insert(UserRightsEn argEn)
        {
            bool flag;
            using (TransactionScope ts = new TransactionScope())
            {
                try
                {
                    UserRightsDAL loDs = new UserRightsDAL();
                    flag = loDs.Insert(argEn);
                    ts.Complete();
                }
                catch (Exception ex)
                {
                    
                    throw ex;
                }
            }
            return flag;
        }
                /// <summary>
        /// Method to Update UserRights 
        /// </summary>
        /// <param name="argEn">UserRights Entity is an Input.</param>
        /// <returns>Returns Boolean</returns>
        public bool Update(UserRightsEn argEn)
        {
            bool flag;
            using (TransactionScope ts = new TransactionScope())
            {
                try
                {
                    UserRightsDAL loDs = new UserRightsDAL();
                    flag = loDs.Update(argEn);
                    ts.Complete();
                }
                catch (Exception ex)
                {
                    
                    throw ex;
                }
            }
            return flag;
        }
        /// <summary>
        /// Method to Delete UserRights 
        /// </summary>
        /// <param name="argEn">UserRights Entity is an Input.UserGroup as Input Property.</param>
        /// <returns>Returns Boolean</returns>
        public bool Delete(UserRightsEn argEn)
        {
            bool flag;
            using (TransactionScope ts = new TransactionScope())
            {
                try
                {
                    UserRightsDAL loDs = new UserRightsDAL();
                    flag = loDs.Delete(argEn);
                    ts.Complete();
                }
                catch (Exception ex)
                {
                    
                    throw ex;
                }
            }
            return flag;
        }
        /// <summary>
        /// Method to Check Validation
        /// </summary>
        /// <param name="argEn">UserRights Entity as Input.</param>
        /// <returns>Returns a Boolean</returns>
        public bool IsValid(UserRightsEn argEn)
        {
            try
            {
                //if (argEn.UserGroup == null || argEn.UserGroup.ToString().Length <= 0)
                if (argEn.UserGroup.ToString().Length <= 0)
                    throw new Exception("UserGroup Is Required!");
                //if (argEn.MenuID == null || argEn.MenuID.ToString().Length <= 0)
                if (argEn.MenuID.ToString().Length <= 0)
                    throw new Exception("MenuID Is Required!");
                //if (argEn.IsAdd == null || argEn.IsAdd.ToString().Length <= 0)
                if (argEn.IsAdd.ToString().Length <= 0)
                    throw new Exception("IsAdd Is Required!");
                //if (argEn.IsEdit == null || argEn.IsEdit.ToString().Length <= 0)
                if (argEn.IsEdit.ToString().Length <= 0)
                    throw new Exception("IsEdit Is Required!");
                //if (argEn.IsDelete == null || argEn.IsDelete.ToString().Length <= 0)
                if (argEn.IsDelete.ToString().Length <= 0)
                    throw new Exception("IsDelete Is Required!");
                //if (argEn.IsView == null || argEn.IsView.ToString().Length <= 0)
                if (argEn.IsView.ToString().Length <= 0)
                    throw new Exception("IsView Is Required!");
                //if (argEn.IsPrint == null || argEn.IsPrint.ToString().Length <= 0)
                if (argEn.IsPrint.ToString().Length <= 0)
                    throw new Exception("IsPrint Is Required!");
                //if (argEn.IsAddModeDefault == null || argEn.IsAddModeDefault.ToString().Length <= 0)
                if (argEn.IsAddModeDefault.ToString().Length <= 0)
                    throw new Exception("IsAddModeDefault Is Required!");
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Method to Get List of Menus for Workflow Group
        /// </summary>
        /// <param name="MenuName">MenuName as Input.</param>
        /// <param name="UserGroupID">UserGroupID  as Input.</param>
        /// <returns>Returns List of Menus</returns>
        public List<MenuMasterEn> GetMenuByWorkflowGroup(string MenuName, int UserGroupID, int UserID)
        {
            try
            {
                UserRightsDAL loDs = new UserRightsDAL();
                return loDs.GetMenuByWorkflowGroup(MenuName, UserGroupID, UserID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Method to Get List of Menus By UserGroups
        /// </summary>
        /// <param name="MenuName">MenuName as Input.</param>
        /// <param name="UserGroupID">UserGroupID  as Input.</param>
        /// <returns>Returns List of Menus</returns>
        public List<UserRightsEn> GetMenuByUser(int UserID)
        {
            try
            {
                UserRightsDAL loDs = new UserRightsDAL();
                return loDs.GetMenuByUser(UserID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }

}
