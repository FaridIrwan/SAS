using System;
using System.Text;
using System.Transactions;
using System.Collections.Generic;
using HTS.SAS.Entities;
using HTS.SAS.DataAccessObjects;

namespace HTS.SAS.BusinessObjects
{
    public class StudentBankBAL
    {

        #region Insert
        /// <summary>
        /// Method to Insert StudentBank 
        /// </summary>
        /// <param name="argEn">StudentBank Entity is an Input.</param>
        /// <returns>Returns Boolean</returns>
        public bool Insert(StudentBankEn argEn)
        {
            bool flag;
            using (TransactionScope ts = new TransactionScope())
            {
                try
                {
                    StudentBankDAL loDs = new StudentBankDAL();
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
        #endregion

        #region Update
        //<summary>
        //Method to Update StudentBank 
        //</summary>
        //<param name="argEn">StudentBank Entity is an Input.</param>
        //<returns>Returns Boolean</returns>
        public bool Update(StudentBankEn argEn)
        {
            bool flag;
            using (TransactionScope ts = new TransactionScope())
            {
                try
                {
                    StudentBankDAL loDs = new StudentBankDAL();
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

        #endregion

        #region Delete
        /// <summary>
        /// Method to Delete StudentBank 
        /// </summary>
        /// <param name="argEn">StudentBank Entity is an Input.</param>
        /// <returns>Returns Boolean</returns>
        public bool Delete(StudentBankEn argEn)
        {
            bool flag;
            using (TransactionScope ts = new TransactionScope())
            {
                try
                {
                    StudentBankDAL loDs = new StudentBankDAL();
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
        #endregion

        #region GetStudentBankTypelist
        /// <summary>
        /// Method to Get List of  Active or Inactive StudentBank
        /// </summary>
        /// <param name="argEn">StudentBank Entity as an Input.StudentBankCode,Description and Status as Input Properties.</param>
        /// <returns>Returns List of StudentBank</returns>
        public List<StudentBankEn> GetStudentBankTypelist(StudentBankEn argEn)
        {
            try
            {
                StudentBankDAL loDs = new StudentBankDAL();
                return loDs.GetStudentBankTypelist(argEn);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region GetStudentBankTypeListAll
        /// <summary>
        /// Method to Get List of All StudentCategory
        /// </summary>
        /// <param name="argEn">StudentBank Entity as an Input.StudentBankCode and Description as Input Properties.</param>
        /// <returns>Returns List of StudentBank</returns>
        public List<StudentBankEn> GetStudentBankTypeListAll(StudentBankEn argEn)
        {
            try
            {
                StudentBankDAL loDs = new StudentBankDAL();
                return loDs.GetStudentBankTypeListAll(argEn);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion
    }
}
