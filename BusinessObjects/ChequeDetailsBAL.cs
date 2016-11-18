using System;
using System.Text;
using System.Transactions;
using System.Collections.Generic;
using HTS.SAS.Entities;
using HTS.SAS.DataAccessObjects;

namespace HTS.SAS.BusinessObjects
{
    /// <summary>
    /// Business Class to handle all the ChequeDetails.
    /// </summary>
    public class ChequeDetailsBAL
    {
        /// <summary>
        /// Method to Get ChequeDetails 
        /// </summary>
        /// <param name="argEn">ChequeDetails Entity is an Input.ProcessID is Input Property.</param>
        /// <returns>Returns List of ChequeDetails</returns>
        public List<ChequeDetailsEn> GetList(ChequeDetailsEn argEn)
        {
            try
            {
                ChequeDetailsDAL loDs = new ChequeDetailsDAL();
                return loDs.GetList(argEn);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Method to Get ChequeDetails Item
        /// </summary>
        /// <param name="argEn">ChequeDetails Entity is an Input.</param>
        /// <returns>Returns a ChequeDetails Item</returns>

        public ChequeDetailsEn GetItem(ChequeDetailsEn argEn)
        {
            try
            {
                ChequeDetailsDAL loDs = new ChequeDetailsDAL();
                return loDs.GetItem(argEn);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Method to Insert ChequeDetails
        /// </summary>
        /// <param name="argEn">ChequeDetails Entity is the Input.</param>
        /// <returns>Returns a Boolean</returns>
        public bool Insert(ChequeDetailsEn argEn)
        {
            bool flag;
            using (TransactionScope ts = new TransactionScope())
            {
                try
                {
                    ChequeDetailsDAL loDs = new ChequeDetailsDAL();
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
        /// Method to Update ChequeDetails
        /// </summary>
        /// <param name="argEn">ChequeDetails Entity is the Input.</param>
        /// <returns>Returns a Boolean</returns>
        public bool Update(ChequeDetailsEn argEn)
        {
            bool flag;
            using (TransactionScope ts = new TransactionScope())
            {
                try
                {
                    ChequeDetailsDAL loDs = new ChequeDetailsDAL();
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
        /// Method to Delete  ChequeDetails
        /// </summary>
        /// <param name="argEn">ChequeDetails Entity is an Input.ProceddID is Property.</param>
        /// <returns>Returns a Boolean</returns>

        public bool Delete(ChequeDetailsEn argEn)
        {
            bool flag;
            using (TransactionScope ts = new TransactionScope())
            {
                try
                {
                    ChequeDetailsDAL loDs = new ChequeDetailsDAL();
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
        /// Method to Validate ChequeDetails Fields 
        /// </summary>
        /// <param name="argEn">ChequeDetails Entity is an Input</param>
        /// <returns>Returns a Boolean</returns>

        public bool IsValid(ChequeDetailsEn argEn)
        {
            try
            {
                if (argEn.ProcessId == null || argEn.ProcessId.ToString().Length <= 0)
                    throw new Exception("ProcessId Is Required!");
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }

}
