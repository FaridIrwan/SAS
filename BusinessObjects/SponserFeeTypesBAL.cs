using System;
using System.Text;
using System.Transactions;
using System.Collections.Generic;
using HTS.SAS.Entities;
using HTS.SAS.DataAccessObjects;

namespace HTS.SAS.BusinessObjects
{
    /// <summary>
    /// Business Class to handle all the SponsorFeeTypes Methods.
    /// </summary>
    public class SponsorFeeTypesBAL
    {
        /// <summary>
        /// Method to Get List of SponsorFeeTypes
        /// </summary>
        /// <param name="argEn">SponsorFeeTypes Entity as an Input.</param>
        /// <returns>Returns List of SponsorFeeTypes</returns>
        public List<SponsorFeeTypesEn> GetList(SponsorFeeTypesEn argEn)
        {
            try
            {
                SponsorFeeTypesDAL loDs = new SponsorFeeTypesDAL();
                return loDs.GetList(argEn);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Method to Get List of Sponsor FeeTypes by SponsorCode
        /// </summary>
        /// <param name="argEn">SponsorFeeTypes Entity as an Input.SponsorCode as Input Property.</param>
        /// <returns>Returns List of SponsorFeeTypes</returns>
        public List<SponsorFeeTypesEn> GetSPFeeTypeList(SponsorFeeTypesEn argEn)
        {
            try
            {
                SponsorFeeTypesDAL loDs = new SponsorFeeTypesDAL();
                return loDs.GetSPFeeTypeList(argEn);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Method to Get SponsorFeeTypes Entity
        /// </summary>
        /// <param name="argEn">SponsorFeeTypes Entity is an Input.SASR_Code as Input Property.</param>
        /// <returns>Returns SponsorFeeTypes Entity</returns>
        public SponsorFeeTypesEn GetItem(SponsorFeeTypesEn argEn)
        {
            try
            {
                SponsorFeeTypesDAL loDs = new SponsorFeeTypesDAL();
                return loDs.GetItem(argEn);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Method to Insert SponsorFeeTypes 
        /// </summary>
        /// <param name="argEn">SponsorFeeTypes Entity is an Input.</param>
        /// <returns>Returns Boolean</returns>
        public bool Insert(SponsorFeeTypesEn argEn)
        {
            bool flag;
            using (TransactionScope ts = new TransactionScope())
            {
                try
                {
                    SponsorFeeTypesDAL loDs = new SponsorFeeTypesDAL();
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
        /// Method to Insert SponsorFeeTypes 
        /// </summary>
        /// <param name="argEn">SponsorFeeTypes Entity is an Input.Sponsor Code and Feetype Code as Input Properties.</param>
        /// <returns>Returns Boolean</returns>
        public bool Update(SponsorFeeTypesEn argEn)
        {
            bool flag;
            using (TransactionScope ts = new TransactionScope())
            {
                try
                {
                    SponsorFeeTypesDAL loDs = new SponsorFeeTypesDAL();
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
        /// Method to Delete SponsorFeeTypes 
        /// </summary>
        /// <param name="argEn">SponsorFeeTypes Entity is an Input.SASR_Code as Input Property.</param>
        /// <returns>Returns Boolean</returns>
        public bool Delete(SponsorFeeTypesEn argEn)
        {
            bool flag;
            using (TransactionScope ts = new TransactionScope())
            {
                try
                {
                    SponsorFeeTypesDAL loDs = new SponsorFeeTypesDAL();
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
        /// <param name="argEn">SponsorFeeTypes Entity is as Input.</param>
        /// <returns>Returns a Boolean</returns>
        public bool IsValid(SponsorFeeTypesEn argEn)
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