using System;
using System.Text;
using System.Transactions;
using System.Collections.Generic;
using HTS.SAS.Entities;
using HTS.SAS.DataAccessObjects;

namespace HTS.SAS.BusinessObjects
{
    /// <summary>
    /// Business Class to handle all the FeeStructureDetails Methods.
    /// </summary>
    public class FeeStrDetailsBAL
    {
        /// <summary>
        /// Method to Get List of FeeStrDetails
        /// </summary>
        /// <param name="argEn">FeeStrDetails Entity is an Input.</param>
        /// <returns>Returns List of FeeStrDetails</returns>
        public List<FeeStrDetailsEn> GetList(FeeStrDetailsEn argEn)
        {
            try
            {
                FeeStrDetailsDAL loDs = new FeeStrDetailsDAL();
                return loDs.GetList(argEn);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Method to Get FeeStrDetails Entity
        /// </summary>
        /// <param name="argEn">FeeStrDetails Entity is an Input</param>
        /// <returns>Returns FeeStrDetails Entity</returns>
        public FeeStrDetailsEn GetItem(FeeStrDetailsEn argEn)
        {
            try
            {
                FeeStrDetailsDAL loDs = new FeeStrDetailsDAL();
                return loDs.GetItem(argEn);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Method to Insert FeeStrDetails 
        /// </summary>
        /// <param name="argEn">FeeStrDetails Entity is an Input.</param>
        /// <returns>Returns Boolean</returns>
        public bool Insert(FeeStrDetailsEn argEn)
        {
            bool flag;
            using (TransactionScope ts = new TransactionScope())
            {
                try
                {
                    FeeStrDetailsDAL loDs = new FeeStrDetailsDAL();
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
        /// Method to Upate FeeStrDetails 
        /// </summary>
        /// <param name="argEn">FeeStrDetails Entity is an Input.</param>
        /// <returns>Returns Boolean</returns>
        public bool Update(FeeStrDetailsEn argEn)
        {
            bool flag;
            using (TransactionScope ts = new TransactionScope())
            {
                try
                {
                    FeeStrDetailsDAL loDs = new FeeStrDetailsDAL();
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
        /// Method to Delete FeeStrDetails 
        /// </summary>
        /// <param name="argEn">FeeStrDetails Entity is an Input.</param>
        /// <returns>Returns Boolean</returns>
        public bool Delete(FeeStrDetailsEn argEn)
        {
            bool flag;
            using (TransactionScope ts = new TransactionScope())
            {
                try
                {
                    FeeStrDetailsDAL loDs = new FeeStrDetailsDAL();
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
        /// <param name="argEn">FeeStrDetails Entity is as Input.</param>
        /// <returns>Returns a Boolean</returns>
        public bool IsValid(FeeStrDetailsEn argEn)
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
