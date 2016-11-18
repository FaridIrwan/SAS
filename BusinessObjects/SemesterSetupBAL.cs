using System;
using System.Text;
using System.Transactions;
using System.Collections.Generic;
using HTS.SAS.Entities;
using HTS.SAS.DataAccessObjects;

namespace HTS.SAS.BusinessObjects
{
    /// <summary>
    /// Business Class to handle all the SemesterSetup Methods.
    /// </summary>
    public class SemesterSetupBAL
    {
        /// <summary>
        /// Method to Get List of Semesters
        /// </summary>
        /// <param name="argEn">SemisterSetupCode as Input.</param>
        /// <returns>Returns List of SemesterSetup</returns>
        public List<SemesterSetupEn> GetSessionList(string argEn)
        {
            try
            {
                SemesterSetupDAL loDs = new SemesterSetupDAL();
                return loDs.GetSessionList(argEn);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        /// <summary>
        /// Method to Get List of Semesters
        /// </summary>
        /// <param name="argEn">SemisterSetupCode as Input.</param>
        /// <returns>Returns List of SemesterSetup</returns>
        public SemesterSetupEn GetItemBySession(SemesterSetupEn argEn)
        {
            try
            {
                SemesterSetupDAL loDs = new SemesterSetupDAL();
                return loDs.GetItemBySession(argEn);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        /// <summary>
        /// Method to Get List of SemesterSetup
        /// </summary>
        /// <param name="argEn">SemesterSetup Entity as an Input.</param>
        /// <returns>Returns List of SemesterSetup</returns>
        public List<SemesterSetupEn> GetListSemesterCur(SemesterSetupEn argEn)
        {
            try
            {
                SemesterSetupDAL loDs = new SemesterSetupDAL();
                return loDs.GetListSemesterCur(argEn);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Method to Get List of SemesterSetup
        /// </summary>
        /// <param name="argEn">SemesterSetup Entity as an Input.</param>
        /// <returns>Returns List of SemesterSetup</returns>
        public List<SemesterSetupEn> GetList(SemesterSetupEn argEn)
        {
            try
            {
                SemesterSetupDAL loDs = new SemesterSetupDAL();
                return loDs.GetList(argEn);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Method to Get List of SemesterSetup
        /// </summary>
        /// <param name="argEn">SemesterSetup Entity as an Input.</param>
        /// <returns>Returns List of SemesterSetup</returns>
        public List<SemesterSetupEn> GetListSemesterCode(SemesterSetupEn argEn)
        {
            try
            {
                SemesterSetupDAL loDs = new SemesterSetupDAL();
                return loDs.GetListSemesterCode(argEn);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Method to Get List of All Semesters
        /// </summary>
        /// <param name="argEn">SemesterSetup Entity as an Input.SemesterSetupCode and Description as Input Properties.</param>
        /// <returns>Returns List of SemesterSetup</returns>
        public List<SemesterSetupEn> GetSemesterSetupListAll(SemesterSetupEn argEn)
        {
            try
            {
                SemesterSetupDAL loDs = new SemesterSetupDAL();
                return loDs.GetSemesterSetupListAll(argEn);
            } 
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //modified by Hafiz @ 10/6/2016 - added ProgramId
        public string FetchTotalBatchAmount(string BatchNumber, string ProgramId)
        {
            try
            {
                SemesterSetupDAL loDs = new SemesterSetupDAL();
                return loDs.FetchTotalBatchAmount(BatchNumber, ProgramId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Method to Get List of Active or Inactive Semesters
        /// </summary>
        /// <param name="argEn">SemesterSetup Entity as an Input.SemesterSetupCode,Description and Status as Input Properties.</param>
        /// <returns>Returns List of SemesterSetup</returns>
        public List<SemesterSetupEn> GetSemesterSetupList(SemesterSetupEn argEn)
        {
            try
            {
                SemesterSetupDAL loDs = new SemesterSetupDAL();
                return loDs.GetSemesterSetupList(argEn);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Method to Get SemesterSetup Entity
        /// </summary>
        /// <param name="argEn">SemesterSetup Entity is an Input.Semester and Description as Input Property.</param>
        /// <returns>Returns SemesterSetup Entity</returns>
        public SemesterSetupEn GetItem(SemesterSetupEn argEn)
        {
            try
            {
                SemesterSetupDAL loDs = new SemesterSetupDAL();
                return loDs.GetItem(argEn);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Method to Get SemesterSetup Entity
        /// </summary>
        /// <param name="argEn">SemesterSetup Entity is an Input.SAST_Code as Input Property.</param>
        /// <returns>Returns SemesterSetup Entity</returns>
        public SemesterSetupEn GetSessionItem(string argEn)
        {
            try
            {
                SemesterSetupDAL loDs = new SemesterSetupDAL();
                return loDs.GetSessionItem(argEn);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Method to Insert SemesterSetup 
        /// </summary>
        /// <param name="argEn">SemesterSetup Entity is an Input.</param>
        /// <returns>Returns Boolean</returns>
        public bool Insert(SemesterSetupEn argEn)
        {
            bool flag;
            using (TransactionScope ts = new TransactionScope())
            {
                try
                {
                    SemesterSetupDAL loDs = new SemesterSetupDAL();
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
        /// Method to Update SemesterSetup 
        /// </summary>
        /// <param name="argEn">SemesterSetup Entity is an Input.</param>
        /// <returns>Returns Boolean</returns>
        public bool Update(SemesterSetupEn argEn)
        {
            bool flag;
            using (TransactionScope ts = new TransactionScope())
            {
                try
                {
                    SemesterSetupDAL loDs = new SemesterSetupDAL();
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
        /// Method to Delete SemesterSetup 
        /// </summary>
        /// <param name="argEn">SemesterSetup Entity is an Input.</param>
        /// <returns>Returns Boolean</returns>
        public bool Delete(SemesterSetupEn argEn)
        {
            bool flag;
            using (TransactionScope ts = new TransactionScope())
            {
                try
                {
                    SemesterSetupDAL loDs = new SemesterSetupDAL();
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
        /// <param name="argEn">SemesterSetup Entity is as Input.</param>
        /// <returns>Returns a Boolean</returns>
        public bool IsValid(SemesterSetupEn argEn)
        {
            try
            {
                if (argEn.SemisterSetupCode == null || argEn.SemisterSetupCode.ToString().Length <= 0)
                    throw new Exception("SemisterSetupCode Is Required!");
                if (argEn.Semester == null || argEn.Semester.ToString().Length <= 0)
                    throw new Exception("Semester Is Required!");
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<SemesterSetupEn> GetCurrentSessionList(SemesterSetupEn argEn)
        {
            try
            {
                SemesterSetupDAL loDs = new SemesterSetupDAL();
                return loDs.GetCurrentSessionList(argEn);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

    }

}
