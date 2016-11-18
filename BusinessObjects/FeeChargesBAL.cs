using System;
using System.Text;
using System.Collections.Generic;
using System.Transactions;
using HTS.SAS.Entities;
using HTS.SAS.DataAccessObjects;

namespace HTS.SAS.BusinessObjects
{
    /// <summary>
    /// Business Class to handle all the FeeCharges Methods.
    /// </summary>
    public class FeeChargesBAL
    {
        /// <summary>
        /// Method to Get List of FeeCharges
        /// </summary>
        /// <param name="argEn">FeeCharges Entity is an Input.FTCode is an Input Property</param>
        /// <returns>Returns List of FeeCharges</returns>
        public List<FeeChargesEn> GetFeeCharges(FeeChargesEn argEn)
        {
            try
            {
                FeeChargesDAL loDs = new FeeChargesDAL();
                return loDs.GetFeeCharges(argEn);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Method to Get List of FeeCharges
        /// </summary>
        /// <param name="argEn">FeeCharges Entity is an Input.FTCode is an Input Property</param>
        /// <returns>Returns List of FeeCharges</returns>
        public List<FeeChargesEn> GetKokoCharges(FeeChargesEn argEn)
        {
            try
            {
                FeeChargesDAL loDs = new FeeChargesDAL();
                return loDs.GetKokoCharges(argEn);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Method to Get List of FeeCharges
        /// </summary>
        /// <param name="argEn">FeeCharges Entity is an Input.FTCode is an Input Property</param>
        /// <returns>Returns List of FeeCharges</returns>
        public List<KokoEn> Getkokobaru(FeeChargesEn argEn)
        {
            try
            {
                FeeChargesDAL loDs = new FeeChargesDAL();
                return loDs.Getkokobaru(argEn);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Method to Get FeeCharges Entity
        /// </summary>
        /// <param name="argEn">FeeCharges Entity is an Input</param>
        /// <returns>Returns FeeCharges Entity</returns>
        public FeeChargesEn GetItem(FeeChargesEn argEn)
        {
            try
            {
                FeeChargesDAL loDs = new FeeChargesDAL();
                return loDs.GetItem(argEn);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Method to Insert FeeCharges 
        /// </summary>
        /// <param name="argEn">FeeCharges Entity is an Input.</param>
        /// <returns>Returns Boolean</returns>
        public bool Insert(FeeChargesEn argEn)
        {
            bool flag;
            using (TransactionScope ts = new TransactionScope())
            {
                try
                {
                    FeeChargesDAL loDs = new FeeChargesDAL();
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
        public bool UpdateKo(KokoEn argEn)
        {
            bool flag;
            using (TransactionScope ts = new TransactionScope())
            {
                try
                {
                    FeeChargesDAL loDs = new FeeChargesDAL();
                    flag = loDs.UpdateKo(argEn);
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
        /// Method to Insert FeeCharges 
        /// </summary>
        /// <param name="argEn">FeeCharges Entity is an Input.</param>
        /// <returns>Returns Boolean</returns>
        //public bool InsertKokoCharges(FeeChargesEn argEn)
        //{
        //    bool flag;
        //    using (TransactionScope ts = new TransactionScope())
        //    {
        //        try
        //        {
        //            FeeChargesDAL loDs = new FeeChargesDAL();
        //            flag = loDs.InsertKokoCharges(argEn);
        //            ts.Complete();
        //        }
        //        catch (Exception ex)
        //        {

        //            throw ex;
        //        }
        //    }
        //    return flag;
        //}
        /// <summary>
        /// Method to Update FeeCharges 
        /// </summary>
        /// <param name="argEn">FeeCharges Entity is an Input.</param>
        /// <returns>Returns Boolean</returns>
        public bool Update(FeeChargesEn argEn)
        {
            bool flag;
            using (TransactionScope ts = new TransactionScope())
            {
                try
                {
                    FeeChargesDAL loDs = new FeeChargesDAL();
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
        ///// <summary>
        ///// Method to Update FeeCharges 
        ///// </summary>
        ///// <param name="argEn">FeeCharges Entity is an Input.</param>
        ///// <returns>Returns Boolean</returns>
        //public bool UpdateKokoDetails(FeeChargesEn argEn)
        //{
        //    bool flag;
        //    using (TransactionScope ts = new TransactionScope())
        //    {
        //        try
        //        {
        //            FeeChargesDAL loDs = new FeeChargesDAL();
        //            flag = loDs.UpdateKokoDetails(argEn);
        //            ts.Complete();
        //        }
        //        catch (Exception ex)
        //        {

        //            throw ex;
        //        }
        //    }
        //    return flag;
        //}
        /// <summary>
        /// Method to Update FeeCharges 
        /// </summary>
        /// <param name="argEn">FeeCharges Entity is an Input.</param>
        /// <returns>Returns Boolean</returns>
        public bool UpdateKokoDetails(KokoEn argEn)
        {
            bool flag;
            using (TransactionScope ts = new TransactionScope())
            {
                try
                {
                    FeeChargesDAL loDs = new FeeChargesDAL();
                    flag = loDs.UpdateKokoDetails(argEn);
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
        /// Method to Delete FeeCharges 
        /// </summary>
        /// <param name="argEn">FeeCharges Entity is an Input.</param>
        /// <returns>Returns Boolean</returns>
        public bool Delete(FeeChargesEn argEn)
        {
            bool flag;
            using (TransactionScope ts = new TransactionScope())
            {
                try
                {
                    FeeChargesDAL loDs = new FeeChargesDAL();
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
        /// <param name="argEn">FeeCharges Entity is as Input.</param>
        /// <returns>Returns a Boolean</returns>
        public bool IsValid(FeeChargesEn argEn)
        {
            try
            {
                if (argEn.FTCode == null || argEn.FTCode.ToString().Length <= 0)
                    throw new Exception("FTCode Is Required!");
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }

}
