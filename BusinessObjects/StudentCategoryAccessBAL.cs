using System;
using System.Text;
using System.Transactions;
using System.Collections.Generic;
using HTS.SAS.Entities;
using HTS.SAS.DataAccessObjects;

namespace HTS.SAS.BusinessObjects
{
    /// <summary>
    /// Business Class to handle all the StudentCategoryAccess Methods.
    /// </summary>
    public class StudentCategoryAccessBAL
    {
        /// <summary>
        /// Method to Get List of Menus
        /// </summary>
        /// <param name="argEn">StudentCategoryAccess Entity as an Input.MenuName as Input Property.</param>
        /// <returns>Returns List of StudentCategoryAccess</returns>

        public List<StudentCategoryAccessEn> GetMenuList(StudentCategoryAccessEn argEn)
        {
            try
            {
                StudentCategoryAccessDAL loDs = new StudentCategoryAccessDAL();
                return loDs.GetMenuList(argEn);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        /// <summary>
        /// Method to Get List of StudentCategoryAccess
        /// </summary>
        /// <param name="argEn">StudentCategoryAccess Entity as an Input.</param>
        /// <returns>Returns List of StudentCategoryAccess</returns>
        public List<StudentCategoryAccessEn> GetList(StudentCategoryAccessEn argEn)
        {
            try
            {
                StudentCategoryAccessDAL loDs = new StudentCategoryAccessDAL();
                return loDs.GetList(argEn);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Method to Get List of All StudentCategoryAccess
        /// </summary>
        /// <param name="argEn">StudentCategoryAccess Entity as an Input.StudentCategoryCode as Input Property.</param>
        /// <returns>Returns List of StudentCategoryAccess</returns>
        public List<StudentCategoryAccessEn> GetStuCatAccessList(StudentCategoryAccessEn argEn)
        {
            try
            {
                StudentCategoryAccessDAL loDs = new StudentCategoryAccessDAL();
                return loDs.GetStuCatAccessList(argEn);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Method to Get StudentCategoryAccess Entity
        /// </summary>
        /// <param name="argEn">StudentCategoryAccess Entity is an Input.StudentCategoryCode as Input Property.</param>
        /// <returns>Returns StudentCategoryAccess Entity</returns>
        public StudentCategoryAccessEn GetItem(StudentCategoryAccessEn argEn)
        {
            try
            {
                StudentCategoryAccessDAL loDs = new StudentCategoryAccessDAL();
                return loDs.GetItem(argEn);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Method to Insert StudentCategoryAccess 
        /// </summary>
        /// <param name="argEn">StudentCategoryAccess Entity is an Input.</param>
        /// <returns>Returns Boolean</returns> 
        public bool Insert(StudentCategoryAccessEn argEn)
        {
            bool flag;
            using (TransactionScope ts = new TransactionScope())
            {
                try
                {
                    StudentCategoryAccessDAL loDs = new StudentCategoryAccessDAL();
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
        /// Method to Update StudentCategoryAccess 
        /// </summary>
        /// <param name="argEn">StudentCategoryAccess Entity is an Input.StudentCategoryCode,MenuID and Status as Input Properties.</param>
        /// <returns>Returns Boolean</returns> 
        public bool Update(StudentCategoryAccessEn argEn)
        {
            bool flag;
            using (TransactionScope ts = new TransactionScope())
            {
                try
                {
                    StudentCategoryAccessDAL loDs = new StudentCategoryAccessDAL();
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
        /// Method to Delete StudentCategoryAccess 
        /// </summary>
        /// <param name="argEn">StudentCategoryAccess Entity is an Input.StudentCategoryCode as Input Property.</param>
        /// <returns>Returns Boolean</returns> 
        public bool Delete(StudentCategoryAccessEn argEn)
        {
            bool flag;
            using (TransactionScope ts = new TransactionScope())
            {

                try
                {
                    StudentCategoryAccessDAL loDs = new StudentCategoryAccessDAL();
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
        /// <param name="argEn">StudentCategoryAccess Entity is as Input.</param>
        /// <returns>Returns a Boolean</returns>
        public bool IsValid(StudentCategoryAccessEn argEn)
        {
            try
            {
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }

}
