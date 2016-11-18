using System;
using System.Text;
using System.Transactions;
using System.Collections.Generic;
using HTS.SAS.Entities;
using HTS.SAS.DataAccessObjects;

namespace HTS.SAS.BusinessObjects
{
    /// <summary>
    /// Business Class to handle all the Users Methods.
    /// </summary>
    public class UsersBAL
    {
        /// <summary>
        /// Method to Get UserRights
        /// </summary>
        /// <param name="MenuId">GroupId  as Input Property.</param>
        /// <param name="GroupId">MenuId as Input Property.</param>
        /// <returns>Returns UserRights Entity</returns>
        public UserRightsEn GetUserRights(int MenuId, int GroupId)
        {
            try
            {
                UsersDAL loDs = new UsersDAL();
                return loDs.GetUserRights(MenuId,GroupId);
            }
            catch (Exception ex)
            {
            throw ex;
            }
        }
        /// <summary>
        /// Method to Get List of Active Or Inactive Users
        /// </summary>
        /// <param name="argEn">Users Entity  as an Input.UserName,Email,UserStatus,UserGroupId and RecStatus as Input Properties.</param>
        /// <returns>Returns List of Users</returns>
        public List<UsersEn> GetUsersList(UsersEn argEn)
        {
            try
            {
                UsersDAL loDs = new UsersDAL();
                return loDs.GetUsersList(argEn);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Method to Get Login User
        /// </summary>
        /// <param name="argEn">Users Entity  as an Input.UserName and Password as Input Properties.</param>
        /// <returns>Returns Users Entity</returns>
        public UsersEn GetLoginUser(UsersEn argEn)
        {
            try
            {
                UsersDAL loDs = new UsersDAL();
                return loDs.GetLoginUser(argEn);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Method to Get List of Users
        /// </summary>
        /// <param name="argEn">Users Entity  as an Inputs.</param>
        /// <returns>Returns List of Users</returns>
        public List<UsersEn> GetList(UsersEn argEn)
        {
            try
            {
                UsersDAL loDs = new UsersDAL();
                return loDs.GetList(argEn);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Method to Get Users Entity
        /// </summary>
        /// <param name="argEn">Users Entity is an Input.UserName as Input Property.</param>
        /// <returns>Returns Users Entity</returns>
        public UsersEn GetItem(UsersEn argEn)
        {
            try
            {
                UsersDAL loDs = new UsersDAL();
                return loDs.GetItem(argEn);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Method to Insert Users 
        /// </summary>
        /// <param name="argEn">Users Entity is an Input.</param>
        /// <returns>Returns Boolean</returns>
        public bool Insert(UsersEn argEn)
        {
            bool flag;
            using (TransactionScope ts = new TransactionScope())
            {
                try
                {
                    UsersDAL loDs = new UsersDAL();
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
        /// Method to Update Users 
        /// </summary>
        /// <param name="argEn">Users Entity is an Input.</param>
        /// <returns>Returns Boolean</returns>
        public bool Update(UsersEn argEn)
        {
            bool flag;
            using (TransactionScope ts = new TransactionScope())
            {
                try
                {
                    UsersDAL loDs = new UsersDAL();
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
        /// Method to Delete Users 
        /// </summary>
        /// <param name="argEn">Users Entity is an Input.UserName as Input Property.</param>
        /// <returns>Returns Boolean</returns>
        //public bool Delete(UsersEn argEn)
        //{
        //    bool flag;
        //    using (TransactionScope ts = new TransactionScope())
        //    {
        //        try
        //        {
        //            UsersDAL loDs = new UsersDAL();
        //            flag = loDs.Delete(argEn);
        //            ts.Complete();
        //        }
        //        catch (Exception ex)
        //        {
                    
        //            throw ex;
        //        }
        //    }
        //    return flag;
        //}
        public bool Delete(List<UsersEn> argEn)
        {
            bool flag;
            using (TransactionScope ts = new TransactionScope())
            {
                try
                {
                    UsersDAL loDs = new UsersDAL();
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
        /// <param name="argEn">Users Entity as Input.</param>
        /// <returns>Returns Users Entity</returns>
        public bool IsValid(UsersEn argEn)
        {
            try
            {
                if (argEn.UserID == null || argEn.UserID.ToString().Length <= 0)
                    throw new Exception("UserID Is Required!");
                if (argEn.UserName == null || argEn.UserName.ToString().Length <= 0)
                    throw new Exception("UserName Is Required!");
                if (argEn.Password == null || argEn.Password.ToString().Length <= 0)
                    throw new Exception("Password Is Required!");
                if (argEn.UserGroupId == null || argEn.UserGroupId.ToString().Length <= 0)
                    throw new Exception("UserGroupId Is Required!");
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Method to get DataGrid
        /// </summary>
        /// <param name="argEn">Users Entity as Input.</param>
        /// <returns>Returns a Boolean</returns>
        public List<UsersEn> DataGrid(UsersEn argEn)
        {
            try
            {
                UsersDAL loDs = new UsersDAL();
                return loDs.DataGrid(argEn);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Method to get GetUser
        /// </summary>
        /// <param name="argEn">Users Entity as Input.</param>
        /// <returns>Returns a Boolean</returns>
        public List<UsersEn> GetUser(UsersEn argEn)
        {
            try
            {
                UsersDAL loDs = new UsersDAL();
                return loDs.GetUser(argEn);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int CheckDuplicateStaff(string User)
        {
            try
            {
                UsersDAL loDs = new UsersDAL();
                return loDs.CheckDuplicateStaff(User);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }

}
