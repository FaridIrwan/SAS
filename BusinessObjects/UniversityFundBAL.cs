using System;
using System.Text;
using System.Transactions;
using System.Collections.Generic;
using HTS.SAS.Entities;
using HTS.SAS.DataAccessObjects;


namespace HTS.SAS.BusinessObjects
{
    /// <summary>
    /// Business Class to handle all the UniversityFund Methods.
    /// </summary>
    public class UniversityFundBAL
    {
        /// <summary>
        /// Method to Get List of UniversityFund
        /// </summary>
        /// <param name="argEn">UniversityFund Entity as an Input.</param>
        /// <returns>Returns List of UniversityFund</returns>
        public List<UniversityFundEn> GetList(UniversityFundEn argEn)
        {
            try
            {
                UniversityFundDAL loDs = new UniversityFundDAL();
                return loDs.GetList(argEn);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Method to Get List of Active or Inactive UniversityFunds
        /// </summary>
        /// <param name="argEn">UniversityFund Entity as an Input.UniversityFundCode,Description,GLCode and Status as Input Properties.</param>
        /// <returns>Returns List of UniversityFund</returns>
        public List<UniversityFundEn> GetUniversityFundList(UniversityFundEn argEn)
        {
            try
            {
                UniversityFundDAL loDs = new UniversityFundDAL();
                return loDs.GetUniversityFundList(argEn);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Method to Get UniversityFund Entity
        /// </summary>
        /// <param name="argEn">UniversityFund Entity is an Input.UniversityFundCode as Input Property.</param>
        /// <returns>Returns UniversityFund Entity</returns>
        public UniversityFundEn GetItem(UniversityFundEn argEn)
        {
            try
            {
                UniversityFundDAL loDs = new UniversityFundDAL();
                return loDs.GetItem(argEn);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Method to Insert UniversityFund 
        /// </summary>
        /// <param name="argEn">UniversityFund Entity is an Input.</param>
        /// <returns>Returns Boolean</returns>
        public bool Insert(UniversityFundEn argEn)
        {
            bool flag;
            using (TransactionScope ts = new TransactionScope())
            {
                try
                {
                    UniversityFundDAL loDs = new UniversityFundDAL();
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
        /// Method to Update UniversityFund 
        /// </summary>
        /// <param name="argEn">UniversityFund Entity is an Input.</param>
        /// <returns>Returns Boolean</returns>
        public bool Update(UniversityFundEn argEn)
        {
            bool flag;
            using (TransactionScope ts = new TransactionScope())
            {
                try
                {
                    UniversityFundDAL loDs = new UniversityFundDAL();
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
        /// Method to Delete UniversityFund 
        /// </summary>
        /// <param name="argEn">UniversityFund Entity is an Input.UniversityFundCode as Input Property.</param>
        /// <returns>Returns Boolean</returns>
        public bool Delete(UniversityFundEn argEn)
        {
            bool flag;
            using (TransactionScope ts = new TransactionScope())
            {
                try
                {
                    UniversityFundDAL loDs = new UniversityFundDAL();
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
        /// <param name="argEn">UniversityFund Entity as Input.</param>
        /// <returns>Returns a Boolean</returns>
        public bool IsValid(UniversityFundEn argEn)
        {
            try
            {
                if (argEn.UniversityFundCode == null || argEn.UniversityFundCode.ToString().Length <= 0)
                    throw new Exception("UniversityFundCode Is Required!");
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

