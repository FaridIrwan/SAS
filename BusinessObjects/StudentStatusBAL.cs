using System;
using System.Text;
using System.Transactions;
using System.Collections.Generic;
using HTS.SAS.Entities;
using HTS.SAS.DataAccessObjects;

namespace HTS.SAS.BusinessObjects
{
    /// <summary>
    /// Business Class to handle all the StudentStatus Methods.
    /// </summary>
    public class StudentStatusBAL
    {
        /// <summary>
        /// Method to Get StudentStatus by StudentStatusCode
        /// </summary>
        /// <param name="argEn">StudentStatusCode as an Input.</param>
        /// <returns>Returns StudentStatus Entity</returns>

        public StudentStatusEn GetStudentBIStatus(string argEn)
        {
            try
            {
                StudentStatusDAL loDs = new StudentStatusDAL();
                return loDs.GetStudentBlStatus(argEn);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Method to Get List of StudentStatus
        /// </summary>
        /// <param name="argEn">StudentStatus Entity as an Input.</param>
        /// <returns>Returns List of StudentStatus</returns>
        public List<StudentStatusEn> GetList(StudentStatusEn argEn)
        {
            try
            {
                StudentStatusDAL loDs = new StudentStatusDAL();
                return loDs.GetList(argEn);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Method to Get List of Active or Inactive StudentStatus
        /// </summary>
        /// <param name="argEn">StudentStatus Entity as an Input.StudentStatusCode,Description,Status and BlStatus as Input Property.</param>
        /// <returns>Returns List of StudentStatus</returns>
        public List<StudentStatusEn> GetStudentStatusList(StudentStatusEn argEn)
        {
            try
            {
                StudentStatusDAL loDs = new StudentStatusDAL();
                return loDs.GetStudentStatusList(argEn);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Method to Get List of All StudentStatus
        /// </summary>
        /// <param name="argEn">StudentStatus Entity as an Input.StudentStatusCode and Description as Input Property.</param>
        /// <returns>Returns List of StudentStatus</returns>
        public List<StudentStatusEn> GetStudentStatusListAll(StudentStatusEn argEn)
        {
            try
            {
                StudentStatusDAL loDs = new StudentStatusDAL();
                return loDs.GetStudentStatusListAll(argEn);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Method to Get StudentStatus Entity
        /// </summary>
        /// <param name="argEn">StudentStatus Entity is an Input.MatricNo as Input Property.</param>
        /// <returns>Returns StudentStatus Entity</returns>
        public StudentStatusEn GetItem(StudentStatusEn argEn)
        {
            try
            {
                StudentStatusDAL loDs = new StudentStatusDAL();
                return loDs.GetItem(argEn);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Method to Insert StudentStatus 
        /// </summary>
        /// <param name="argEn">StudentStatus Entity is an Input.</param>
        /// <returns>Returns Boolean</returns> 
        public bool Insert(StudentStatusEn argEn)
        {
            bool flag;
            using (TransactionScope ts = new TransactionScope())
            {
                try
                {
                    StudentStatusDAL loDs = new StudentStatusDAL();
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
        /// Method to Update StudentStatus 
        /// </summary>
        /// <param name="argEn">StudentStatus Entity is an Input.</param>
        /// <returns>Returns Boolean</returns> 
        public bool Update(StudentStatusEn argEn)
        {
            bool flag;
            using (TransactionScope ts = new TransactionScope())
            {
                try
                {
                    StudentStatusDAL loDs = new StudentStatusDAL();
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
        /// Method to Insert StudentStatus 
        /// </summary>
        /// <param name="argEn">StudentStatus Entity is an Input.StudentStatusCode as Input Property.</param>
        /// <returns>Returns Boolean</returns> 
        public bool Delete(StudentStatusEn argEn)
        {
            bool flag;
            using (TransactionScope ts = new TransactionScope())
            {
                try
                {
                    StudentStatusDAL loDs = new StudentStatusDAL();
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
        /// <param name="argEn">StudentStatus Entity is as Input.</param>
        /// <returns>Returns a Boolean</returns>
        public bool IsValid(StudentStatusEn argEn)
        {
            try
            {
                if (argEn.StudentStatusCode == null || argEn.StudentStatusCode.ToString().Length <= 0)
                    throw new Exception("StudentStatusCode Is Required!");
                if (argEn.Description == null || argEn.Description.ToString().Length <= 0)
                    throw new Exception("Description Is Required!");
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }

}
