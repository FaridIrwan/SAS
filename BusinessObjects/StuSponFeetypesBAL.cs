using System;
using System.Text;
using System.Transactions;
using System.Collections.Generic;
using HTS.SAS.Entities;
using HTS.SAS.DataAccessObjects;

namespace HTS.SAS.BusinessObjects
{
    /// <summary>
    /// Business Class to handle all the StudentSponsor FeeTypes Methods.
    /// </summary>
    public class StuSponFeeTypesBAL
    {
        /// <summary>
        /// Method to Get StudentSponsor FeeTypes 
        /// </summary>
        /// <param name="argEn">StuSponFeeTypes Entity as an Input.</param>
        /// <returns>Returns StuSponFeeTypes Entity</returns>
        public List<StuSponFeeTypesEn> GetList(StuSponFeeTypesEn argEn)
        {
            try
            {
                StuSponFeeTypesDAL loDs = new StuSponFeeTypesDAL();
                return loDs.GetList(argEn);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Method to Get StudentSponsor FeeTypes 
        /// </summary>
        /// <param name="argEn">StuSponFeeTypes Entity as an Input.MatricNo as Input Property.</param>
        /// <returns>Returns StuSponFeeTypes Entity</returns>
        public List<StuSponFeeTypesEn> GetStuSpnFTList(StuSponFeeTypesEn argEn)
        {
            try
            {
                StuSponFeeTypesDAL loDs = new StuSponFeeTypesDAL();
                return loDs.GetStuSponFTList(argEn);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Method to Get StudentSponsor FeeTypes Entity
        /// </summary>
        /// <param name="argEn">StudentSponFeeTypes Entity is an Input.MatricNo as Input Property.</param>
        /// <returns>Returns StudentSponFeeTypes Entity</returns>
        public StuSponFeeTypesEn GetItem(StuSponFeeTypesEn argEn)
        {
            try
            {
                StuSponFeeTypesDAL loDs = new StuSponFeeTypesDAL();
                return loDs.GetItem(argEn);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Method to Insert StudentSponsor FeeTypes
        /// </summary>
        /// <param name="argEn">StudentSponsor Entity is an Input.</param>
        /// <returns>Returns Boolean</returns> 
        public bool Insert(StuSponFeeTypesEn argEn)
        {
            bool flag;
            using (TransactionScope ts = new TransactionScope())
            {
                try
                {
                    StuSponFeeTypesDAL loDs = new StuSponFeeTypesDAL();
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
        /// Method to Update StudentSponsor FeeTypes
        /// </summary>
        /// <param name="argEn">StudentSponsor Entity is an Input.</param>
        /// <returns>Returns Boolean</returns> 
        public bool Update(StuSponFeeTypesEn argEn)
        {
            bool flag;
            using (TransactionScope ts = new TransactionScope())
            {
                try
                {
                    StuSponFeeTypesDAL loDs = new StuSponFeeTypesDAL();
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
        /// Method to Delete StudentSponsor FeeTypes 
        /// </summary>
        /// <param name="argEn">StuSponFeeTypes Entity is an Input.MatricNo as Input Property.</param>
        /// <returns>Returns Boolean</returns> 
        public bool Delete(StuSponFeeTypesEn argEn)
        {
            bool flag;
            using (TransactionScope ts = new TransactionScope())
            {
                try
                {
                    StuSponFeeTypesDAL loDs = new StuSponFeeTypesDAL();
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
        /// <param name="argEn">StuSponFeeTypes Entity is as Input.</param>
        /// <returns>Returns a Boolean</returns>
        public bool IsValid(StuSponFeeTypesEn argEn)
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
