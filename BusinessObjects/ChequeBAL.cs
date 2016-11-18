using System;
using System.Text;
using System.Transactions;
using System.Collections.Generic;
using HTS.SAS.Entities;
using HTS.SAS.DataAccessObjects;

namespace HTS.SAS.BusinessObjects
{
    /// <summary>
    /// Business Class to handle all the Cheques.
    /// </summary>
    public class ChequeBAL
    {
        /// <summary>
        /// Method to Get Cheques
        /// </summary>
        /// <param name="argEn">Cheques Entity is an Input.ProcessID is Input Property.</param>
        /// <returns>Returns List of Cheques.</returns>
        public List<ChequeEn> GetList(ChequeEn argEn)
        {
            try
            {
                ChequeDAL loDs = new ChequeDAL();
                return loDs.GetList(argEn);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Method to Get refund Cheques
        /// </summary>
        /// <param name="argEn">Cheques Entity is an Input.ProcessID is Input Property.</param>
        /// <returns>Returns List of Cheques</returns>
        public List<ChequeEn> GetRefundList(ChequeEn argEn)
        {
            try
            {
                ChequeDAL loDs = new ChequeDAL();
                return loDs.GetRefundList(argEn);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Method to Get Cheque Item
        /// </summary>
        /// <param name="argEn">Cheques Entity is an Input.ProcessID is Input Property.</param>
        /// <returns>Returns a Cheque Item</returns>
        public ChequeEn GetItem(ChequeEn argEn)
        {
            try
            {
                ChequeDAL loDs = new ChequeDAL();
                return loDs.GetItem(argEn);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Method to Insert Cheques
        /// </summary>
        /// <param name="argEn">Cheques Entity is the Input.</param>
        /// <returns>Returns a Boolean</returns>
        public string Insert(ChequeEn argEn)
        {
            String flag;
            using (TransactionScope ts = new TransactionScope())
            {
                try
                {
                    ChequeDAL loDs = new ChequeDAL();
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
        /// Method to Update Cheques
        /// </summary>
        /// <param name="argEn">Cheques Entity is the Input.</param>
        /// <returns>Returns Boolean.</returns>
        public string Update(ChequeEn argEn)
        {
            String flag;
            using (TransactionScope ts = new TransactionScope())
            {
                try
                {
                    ChequeDAL loDs = new ChequeDAL();
                    loDs.BatchDelete(argEn);
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
        /// Method to Delete Cheques Batch
        /// </summary>
        /// <param name="argEn">Cheques Entity is the Input.ProcessID is the Input Property.</param>
        /// <returns>Returns a Boolean</returns>
        public bool BatchDelete(ChequeEn argEn)
        {
            bool flag;
            using (TransactionScope ts = new TransactionScope())
            {
                try
                {
                    ChequeDAL loDs = new ChequeDAL();
                    flag = loDs.BatchDelete(argEn);
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
        /// Method to Delete Cheques
        /// </summary>
        /// <param name="argEn">Cheques Entity is the Input.ProcessID is the Input Property.</param>
        /// <returns>Returns Boolean.</returns>
        public bool Delete(ChequeEn argEn)
        {
            bool flag;
            using (TransactionScope ts = new TransactionScope())
            {
                try
                {
                    ChequeDAL loDs = new ChequeDAL();
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
/// Method to Validate Cheque Fields 
/// </summary>
        /// <param name="argEn">Cheques Entity is an Input</param>
        /// <returns>Returns a Boolean</returns>
        public bool IsValid(ChequeEn argEn)
        {
            try
            {
                if (argEn.ProcessID == null || argEn.ProcessID.ToString().Length <= 0)
                    throw new Exception("ProcessID Is Required!");
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }

}
