using System;
using System.Text;
using System.Transactions;
using System.Collections.Generic;
using HTS.SAS.Entities;
using HTS.SAS.DataAccessObjects;

namespace HTS.SAS.BusinessObjects
{
    /// <summary>
    /// Business Class to handle all the UserGroups Methods.
    /// </summary>
    public class UserGroupsBAL
    {
        /// <summary>
        /// Method to Get List of Active UserGroups
        /// </summary>
        /// <returns>Returns List of UserGroups</returns>
        public List<UserGroupsEn> GetUserGroups()
        {
            try
            {
                UserGroupsDAL loDs = new UserGroupsDAL();
                return loDs.GetUserGroups();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Method to Get List of All UserGroups
        /// </summary>
        /// <param name="argEn">UserGroups Entity as an Input.UserGroupName and Description as Input properties.</param>
        /// <returns>Returns List of UserGroups</returns>
        public List<UserGroupsEn> GetUserGroupsTypelistAll(UserGroupsEn argEn)
        {
            try
            {
                UserGroupsDAL loDs = new UserGroupsDAL();
                return loDs.GetUserGroupsTypelistAll(argEn);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Method to Get List of Active or Inative UserGroups
        /// </summary>
        /// <param name="argEn">UserGroups Entity as an Input.UserGroupName,Description and Status as Input properties.</param>
        /// <returns>Returns List of UserGroups</returns>
        public List<UserGroupsEn> GetUserGroupsTypelist(UserGroupsEn argEn)
        {
            try
            {
                UserGroupsDAL loDs = new UserGroupsDAL();
                return loDs.GetUserGroupsTypelist(argEn);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Method to Get List of UserGroups
        /// </summary>
        /// <param name="argEn">UserGroups Entity as an Input.</param>
        /// <returns>Returns List of UserGroups</returns>
        public List<UserGroupsEn> GetList(UserGroupsEn argEn)
        {
            try
            {
                UserGroupsDAL loDs = new UserGroupsDAL();
                return loDs.GetList(argEn);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Method to Get List of UserGroups
        /// </summary>
        /// <param name="argEn">UserGroups Entity as an Input.</param>
        /// <returns>Returns List of UserGroups</returns>
        /// <summary>
        public List<UserGroupsEn> GetUserGroupList(UserGroupsEn argEn)
        {
            try
            {
                UserGroupsDAL loDs = new UserGroupsDAL();
                return loDs.GetUserGroupList(argEn);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// Method to Get UserGroups Entity
        /// </summary>
        /// <param name="argEn">UserGroups Entity is an Input.UserGroupId and UserGroupName as Input Properties.</param>
        /// <returns>Returns UserGroups Entity</returns>
        public UserGroupsEn GetItem(UserGroupsEn argEn)
        {
            try
            {
                UserGroupsDAL loDs = new UserGroupsDAL();
                return loDs.GetItem(argEn);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Method to Insert UserGroups 
        /// </summary>
        /// <param name="argEn">UserGroups Entity is an Input.</param>
        /// <returns>Returns Boolean</returns>
        public bool Insert(UserGroupsEn argEn)
        {
            bool flag;
            using (TransactionScope ts = new TransactionScope())
            {
                try
                {
                    UserGroupsDAL loDs = new UserGroupsDAL();
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
        /// Method to Update UserGroups 
        /// </summary>
        /// <param name="argEn">UserGroups Entity is an Input.</param>
        /// <returns>Returns Boolean</returns>
        public bool Update(UserGroupsEn argEn)
        {
            bool flag;
            using (TransactionScope ts = new TransactionScope())
            {
                try
                {
                    UserGroupsDAL loDs = new UserGroupsDAL();
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
        /// Method to Delete UserGroups 
        /// </summary>
        /// <param name="argEn">UserGroups Entity is an Input.UserGroupId as Input Property.</param>
        /// <returns>Returns Boolean</returns>
        public bool Delete(UserGroupsEn argEn)
        {
            bool flag;
            using (TransactionScope ts = new TransactionScope())
            {
                try
                {
                    UserGroupsDAL loDs = new UserGroupsDAL();
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
        /// <param name="argEn">UserGroups Entity as Input.</param>
        /// <returns>Returns a Boolean</returns>
        public bool IsValid(UserGroupsEn argEn)
        {
            try
            {
                if (argEn.UserGroupId == null || argEn.UserGroupId.ToString().Length <= 0)
                    throw new Exception("UserGroupId Is Required!");
                if (argEn.UserGroupName == null || argEn.UserGroupName.ToString().Length <= 0)
                    throw new Exception("UserGroupName Is Required!");
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }

}
