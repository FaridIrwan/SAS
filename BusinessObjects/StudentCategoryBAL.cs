using System;
using System.Text;
using System.Transactions;
using System.Collections.Generic;
using HTS.SAS.Entities;
using HTS.SAS.DataAccessObjects;

namespace HTS.SAS.BusinessObjects
{
    /// <summary>
    /// Business Class to handle all the StudentCategory Methods.
    /// </summary>
    public class StudentCategoryBAL
    {
        /// <summary>
        /// Method to Get List of StudentCategory
        /// </summary>
        /// <param name="argEn">StudentCategory Entity as an Input.</param>
        /// <returns>Returns List of StudentCategory</returns>
        public List<StudentCategoryEn> GetList(StudentCategoryEn argEn)
        {
            try
            {
                StudentCategoryDAL loDs = new StudentCategoryDAL();
                return loDs.GetList(argEn);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Method to Get List of All StudentCategory
        /// </summary>
        /// <param name="argEn">StudentCategory Entity as an Input.StudentCategoryCode and Description  as Input Properties.</param>
        /// <returns>Returns List of StudentCategory</returns>
        public List<StudentCategoryEn> GetStudentCategoryListAll(StudentCategoryEn argEn)
        {
            try
            {
                StudentCategoryDAL loDs = new StudentCategoryDAL();
                return loDs.GetStudentCategoryListAll(argEn);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Method to Get List of  Active or Inactive StudentCategory
        /// </summary>
        /// <param name="argEn">StudentCategory Entity as an Input.StudentCategoryCode,Description and Status as Input Properties.</param>
        /// <returns>Returns List of StudentCategory</returns>
        public List<StudentCategoryEn> GetStudentCategoryList(StudentCategoryEn argEn)
        {
            try
            {
                StudentCategoryDAL loDs = new StudentCategoryDAL();
                return loDs.GetStudentCategoryList(argEn);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Method to Get StudentCategory Entity
        /// </summary>
        /// <param name="argEn">StudentCategory Entity is an Input.StudentCategoryCode as Input Property.</param>
        /// <returns>Returns StudentCategory Entity</returns>
        public StudentCategoryEn GetItem(StudentCategoryEn argEn)
        {
            try
            {
                StudentCategoryDAL loDs = new StudentCategoryDAL();
                return loDs.GetItem(argEn);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Method to Insert StudentCategory 
        /// </summary>
        /// <param name="argEn">StudentCategory Entity is an Input.</param>
        /// <returns>Returns Boolean</returns> 
        public bool Insert(StudentCategoryEn argEn)
        {
            bool flag;
            using (TransactionScope ts = new TransactionScope())
            {
                try
                {
                    StudentCategoryDAL loDs = new StudentCategoryDAL();
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
        /// Method to Update StudentCategory 
        /// </summary>
        /// <param name="argEn">StudentCategory Entity is an Input.</param>
        /// <returns>Returns Boolean</returns> 
        public bool Update(StudentCategoryEn argEn)
        {
            bool flag;
            using (TransactionScope ts = new TransactionScope())
            {
                try
                {
                    StudentCategoryDAL loDs = new StudentCategoryDAL();
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
        /// Method to Delete StudentCategory 
        /// </summary>
        /// <param name="argEn">StudentCategory Entity is an Input.StudentCategoryCode as Input Property.</param>
        /// <returns>Returns Boolean</returns> 
        public bool Delete(StudentCategoryEn argEn)
        {
            bool flag;
            using (TransactionScope ts = new TransactionScope())
            {
                try
                {
                    StudentCategoryDAL loDs = new StudentCategoryDAL();
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
        /// <param name="argEn">StudentCategory Entity is as Input.</param>
        /// <returns>Returns a Boolean</returns>
        public bool IsValid(StudentCategoryEn argEn)
        {
            try
            {
                if (argEn.StudentCategoryCode == null || argEn.StudentCategoryCode.ToString().Length <= 0)
                    throw new Exception("StudentCategoryCode Is Required!");
                if (argEn.Description == null || argEn.Description.ToString().Length <= 0)
                    throw new Exception("Description Is Required!");
                if (argEn.UpdatedBy == null || argEn.UpdatedBy.ToString().Length <= 0)
                    throw new Exception("UpdatedBy Is Required!");
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }

}
