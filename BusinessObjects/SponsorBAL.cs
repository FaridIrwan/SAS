using System;
using System.Text;
using System.Transactions;
using System.Collections.Generic;
using HTS.SAS.Entities;
using HTS.SAS.DataAccessObjects;

namespace HTS.SAS.BusinessObjects
{
    /// <summary>
    /// Business Class to handle all the Sponsors Methods.
    /// </summary>
    public class SponsorBAL
    {
        /// <summary>
        /// Method to Get List of Sponsors
        /// </summary>
        /// <param name="argEn">Sponsors Entity as an Input.</param>
        /// <returns>Returns List of Sponsors</returns>
        public List<SponsorEn> GetList(SponsorEn argEn)
        {
            try
            {
                SponsorDAL loDs = new SponsorDAL();
                return loDs.GetList(argEn);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Method to Get List Active or Inactive of Sponsors
        /// </summary>
        /// <param name="argEn">Sponsors Entity as an Input.SponsorCode,Name,Type,GLAccount and Status as Input Properties.</param>
        /// <returns>Returns List of Sponsors</returns>
        public List<SponsorEn> GetSponserList(SponsorEn argEn)
        {
            try
            {
                SponsorDAL loDs = new SponsorDAL();
                return loDs.GetSponserList(argEn);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Method to Get Sponsor Entity
        /// </summary>
        /// <param name="argEn">Sponsor Entity is an Input.SASR_Code as Input Property.</param>
        /// <returns>Returns Sponsor Entity</returns>
        public SponsorEn GetItem(SponsorEn argEn)
        {
            try
            {
                SponsorDAL loDs = new SponsorDAL();
                return loDs.GetItem(argEn);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Method to Insert Sponsor 
        /// </summary>
        /// <param name="argEn">Sponsor Entity is an Input.</param>
        /// <returns>Returns Boolean</returns>
        public bool Insert(SponsorEn argEn)
        {
            bool flag;
            using (TransactionScope ts = new TransactionScope())
            {
                try
                {
                    SponsorDAL loDs = new SponsorDAL();
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
        /// Method to Update Sponsor 
        /// </summary>
        /// <param name="argEn">Sponsor Entity is an Input.</param>
        /// <returns>Returns Boolean</returns>
        public bool Update(SponsorEn argEn)
        {
            bool flag;
            using (TransactionScope ts = new TransactionScope())
            {
                try
                {
                    SponsorDAL loDs = new SponsorDAL();
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
        /// Method to Delete Sponsor 
        /// </summary>
        /// <param name="argEn">Sponsor Entity is an Input.SASR_Code as Input Property.</param>
        /// <returns>Returns Boolean</returns>
        public bool Delete(SponsorEn argEn)
        {
            bool flag;
            using (TransactionScope ts = new TransactionScope())
            {
                try
                {
                    SponsorDAL loDs = new SponsorDAL();
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
        /// <param name="argEn">Sponsor Entity is as Input.</param>
        /// <returns>Returns a Boolean</returns>
        public bool IsValid(SponsorEn argEn)
        {
            try
            {
                if (argEn.SponserCode == null || argEn.SponserCode.ToString().Length <= 0)
                    throw new Exception("SponserCode Is Required!");
                if (argEn.Name == null || argEn.Name.ToString().Length <= 0)
                    throw new Exception("Name Is Required!");
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }

}
