using System;
using System.Text;
using System.Transactions;
using System.Collections.Generic;
using HTS.SAS.Entities;
using HTS.SAS.DataAccessObjects;

namespace HTS.SAS.BusinessObjects
{
    /// <summary>
    /// Business Class to handle all the PayMode Methods.
    /// </summary>
    public class PayModeBAL
    {
        /// <summary>
        /// Method to Get List of PayMode
        /// </summary>
        /// <param name="argEn">PayMode Entity is an Input.SAPM_Code,SAPM_Des and SAPM_Status are Input Properties.</param>
        /// <returns>Returns List of PayMode</returns>
        public List<PayModeEn> GetPaytype(PayModeEn argEn)
        {
            try
            {
                PayModeDAL loDs = new PayModeDAL();
                return loDs.GetPaytype(argEn);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Method to Get List of PayMode
        /// </summary>
        /// <param name="argEn">PayMode Entity is an Input.SAPM_Code,SAPM_Des and SAPM_Status are Input Properties.</param>
        /// <returns>Returns List of PayMode</returns>
        public List<PayModeEn> GetPaytypeAll(PayModeEn argEn)
        {
            try
            {
                PayModeDAL loDs = new PayModeDAL();
                return loDs.GetPaytypeAll(argEn);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        /// <summary>
        /// Method to Get PayMode Entity
        /// </summary>
        /// <param name="argEn">PayMode Entity is an Input.SAPM_Code as Input Property.</param>
        /// <returns>Returns PayMode Entity</returns>
        public PayModeEn GetItem(PayModeEn argEn)
        {
            try
            {
                PayModeDAL loDs = new PayModeDAL();
                return loDs.GetItem(argEn);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Method to Insert PayMode 
        /// </summary>
        /// <param name="argEn">PayMode Entity is an Input.</param>
        /// <returns>Returns Boolean</returns>
        public bool Insert(PayModeEn argEn)
        {
            bool flag;
            using (TransactionScope ts = new TransactionScope())
            {
                try
                {
                    PayModeDAL loDs = new PayModeDAL();
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
        /// Method to Update PayMode 
        /// </summary>
        /// <param name="argEn">PayMode Entity is an Input.</param>
        /// <returns>Returns Boolean</returns>
        public bool Update(PayModeEn argEn)
        {
            bool flag;
            using (TransactionScope ts = new TransactionScope())
            {
                try
                {
                    PayModeDAL loDs = new PayModeDAL();
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
        /// Method to Delete PayMode 
        /// </summary>
        /// <param name="argEn">PayMode Entity is an Input.</param>
        /// <returns>Returns Boolean</returns>
        public bool Delete(PayModeEn argEn)
        {
            bool flag;
            using (TransactionScope ts = new TransactionScope())
            {
                try
                {
                    PayModeDAL loDs = new PayModeDAL();
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
        /// <param name="argEn">PayMode Entity is as Input.</param>
        /// <returns>Returns a Boolean</returns>
        public bool IsValid(PayModeEn argEn)
        {
            try
            {
                if (argEn.SAPM_Code == null || argEn.SAPM_Code.ToString().Length <= 0)
                    throw new Exception("SAPM_Code Is Required!");
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }

}
