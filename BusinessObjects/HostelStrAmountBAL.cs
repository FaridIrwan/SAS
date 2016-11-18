using System;
using System.Text;
using System.Transactions;
using System.Collections.Generic;
using HTS.SAS.Entities;
using HTS.SAS.DataAccessObjects;

namespace HTS.SAS.BusinessObjects
{
    /// <summary>
    /// Business Class to handle all the HostelStructureAmount Methods.
    /// </summary>
    public class HostelStrAmountBAL
    {
        /// <summary>
        /// Method to Get List of HostelStructureAmount
        /// </summary>
        /// <param name="argEn">HostelStrAmount Entity is an Input.</param>
        /// <returns>Returns List of HostelStrAmounts</returns>
        public List<HostelStrAmountEn> GetList(HostelStrAmountEn argEn)
        {
            try
            {
                HostelStrAmountDAL loDs = new HostelStrAmountDAL();
                return loDs.GetList(argEn);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Method to Get FeeTypes Entity
        /// </summary>
        /// <param name="argEn">HostelStrAmount Entity is an Input.HSCode as Input Property.</param>
        /// <returns>Returns HostelStrAmount Entity</returns>
        public HostelStrAmountEn GetItem(HostelStrAmountEn argEn)
        {
            try
            {
                HostelStrAmountDAL loDs = new HostelStrAmountDAL();
                return loDs.GetItem(argEn);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Method to Insert HostelStrAmount 
        /// </summary>
        /// <param name="argEn">HostelStrAmount Entity is an Input.</param>
        /// <returns>Returns Boolean</returns>
        public bool Insert(HostelStrAmountEn argEn)
        {
            bool flag;
            using (TransactionScope ts = new TransactionScope())
            {
                try
                {
                    HostelStrAmountDAL loDs = new HostelStrAmountDAL();
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
        /// Method to Update HostelStrAmount 
        /// </summary>
        /// <param name="argEn">HostelStrAmount Entity is an Input.</param>
        /// <returns>Returns Boolean</returns>
        public bool Update(HostelStrAmountEn argEn)
        {
            bool flag;
            using (TransactionScope ts = new TransactionScope())
            {
                try
                {
                    HostelStrAmountDAL loDs = new HostelStrAmountDAL();
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
        /// Method to Delete HostelStrAmount 
        /// </summary>
        /// <param name="argEn">HostelStrAmount Entity is an Input.</param>
        /// <returns>Returns Boolean</returns>
        public bool Delete(HostelStrAmountEn argEn)
        {
            bool flag;
            using (TransactionScope ts = new TransactionScope())
            {
                try
                {
                    HostelStrAmountDAL loDs = new HostelStrAmountDAL();
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
        /// <param name="argEn">HostelStrAmount Entity is as Input.</param>
        /// <returns>Returns a Boolean</returns>
        public bool IsValid(HostelStrAmountEn argEn)
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
