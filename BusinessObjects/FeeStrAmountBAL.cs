using System;
using System.Text;
using System.Transactions;
using System.Collections.Generic;
using HTS.SAS.Entities;
using HTS.SAS.DataAccessObjects;

namespace HTS.SAS.BusinessObjects
{
    /// <summary>
    /// Business Class to handle all the FeeStructureAmount Methods.
    /// </summary>
    public class FeeStrAmountBAL
    {
        /// <summary>
        /// Method to Get List of FeeStrAmount
        /// </summary>
        /// <param name="argEn">FeeStrAmount Entity is an Input.</param>
        /// <returns>Returns List of FeeStrAmount</returns>
        public List<FeeStrAmountEn> GetList(FeeStrAmountEn argEn)
        {
            try
            {
                FeeStrAmountDAL loDs = new FeeStrAmountDAL();
                return loDs.GetList(argEn);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Method to Get FeeStrAmount Entity
        /// </summary>
        /// <param name="argEn">FeeStrAmount Entity is an Input</param>
        /// <returns>Returns FeeStrAmount Entity</returns>
        public FeeStrAmountEn GetItem(FeeStrAmountEn argEn)
        {
            try
            {
                FeeStrAmountDAL loDs = new FeeStrAmountDAL();
                return loDs.GetItem(argEn);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Method to Insert FeeStrAmount 
        /// </summary>
        /// <param name="argEn">FeeStrAmount Entity is an Input.</param>
        /// <returns>Returns Boolean</returns>
        public bool Insert(FeeStrAmountEn argEn)
        {
            bool flag;
            using (TransactionScope ts = new TransactionScope())
            {
                try
                {
                    FeeStrAmountDAL loDs = new FeeStrAmountDAL();
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
        /// Method to Update FeeStrAmount 
        /// </summary>
        /// <param name="argEn">FeeStrAmount Entity is an Input.</param>
        /// <returns>Returns Boolean</returns>
        public bool Update(FeeStrAmountEn argEn)
        {
            bool flag;
            using (TransactionScope ts = new TransactionScope())
            {
                try
                {
                    FeeStrAmountDAL loDs = new FeeStrAmountDAL();
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
        /// Method to Delete FeeStrAmount 
        /// </summary>
        /// <param name="argEn">FeeStrAmount Entity is an Input.</param>
        /// <returns>Returns Boolean</returns>
        public bool Delete(FeeStrAmountEn argEn)
        {
            bool flag;
            using (TransactionScope ts = new TransactionScope())
            {
                try
                {
                    FeeStrAmountDAL loDs = new FeeStrAmountDAL();
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
        /// <param name="argEn">FeeStrAmount Entity is as Input.</param>
        /// <returns>Returns a Boolean</returns>
        public bool IsValid(FeeStrAmountEn argEn)
        {
            try
            {
                if (argEn.FSCode == null || argEn.FSCode.ToString().Length <= 0)
                    throw new Exception("FSCode Is Required!");
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }

}
