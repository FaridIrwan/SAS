using System;
using System.Text;
using System.Transactions;
using System.Collections.Generic;
using HTS.SAS.Entities;
using HTS.SAS.DataAccessObjects;

namespace HTS.SAS.BusinessObjects
{
    /// <summary>
    /// Business Class to handle all the BankProfiles Methods.
    /// </summary>
    public class BankProfileBAL
    {
        /// <summary>
        /// Method to Get List of BankProfiles
        /// </summary>
        /// <param name="argEn">BankProfile Entity is an Input.</param>
        /// <returns>Returns List of BankProfiles</returns>
        public List<BankProfileEn> GetList(BankProfileEn argEn)
        {
            try
            {
                BankProfileDAL loDs = new BankProfileDAL();
                return loDs.GetList(argEn);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Method to Get List of All BankProfiles
        /// </summary>
        /// <param name="argEn">BankProfile Entity is an Input.BankDetailsCode,Description,ACCode and GLCode are Input Properties</param>
        /// <returns>Returns List of BankProfiles</returns>
        public List<BankProfileEn> GetBankProfileListAll(BankProfileEn argEn)
        {
            try
            {
                BankProfileDAL loDs = new BankProfileDAL();
                return loDs.GetBankProfileListAll(argEn);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Method to Get List of Active or Inactive BankProfiles
        /// </summary>
        /// <param name="argEn">BankProfile Entity is an Input.BankDetailsCode,Description,ACCode,GLCode,Status are Input Properties</param>
        /// <returns>Returns List of BankProfiles</returns>
        public List<BankProfileEn> GetBankProfileList(BankProfileEn argEn)
        {
            try
            {
                BankProfileDAL loDs = new BankProfileDAL();
                return loDs.GetBankProfileList(argEn);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Method to Get BankProfile Entity
        /// </summary>
        /// <param name="argEn">BankProfile Entity is an Input</param>
        /// <returns>Returns BankProfile Entity</returns>
        public BankProfileEn GetItem(BankProfileEn argEn)
        {
            try
            {
                BankProfileDAL loDs = new BankProfileDAL();
                return loDs.GetItem(argEn);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Method to Insert Bankprofiles
        /// </summary>
        /// <param name="argEn">Bankprofile Entity is an Input.</param>
        /// <returns>Returns Boolean</returns>
        public bool Insert(BankProfileEn argEn)
        {
            bool flag;
            using (TransactionScope ts = new TransactionScope())
            {
                try
                {
                    BankProfileDAL loDs = new BankProfileDAL();
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
        /// Method to Update Bankprofiles
        /// </summary>
        /// <param name="argEn">Bankprofile Entity is an Input.</param>
        /// <returns>Returns Boolean</returns>
        public bool Update(BankProfileEn argEn)
        {
            bool flag;
            using (TransactionScope ts = new TransactionScope())
            {
                try
                {
                    BankProfileDAL loDs = new BankProfileDAL();
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
        /// Method to Delete Bankprofiles
        /// </summary>
        /// <param name="argEn">Bankprofile Entity is an Input.</param>
        /// <returns>Returns Boolean</returns>
        public bool Delete(BankProfileEn argEn)
        {
            bool flag;
            using (TransactionScope ts = new TransactionScope())
            {
                try
                {
                    BankProfileDAL loDs = new BankProfileDAL();
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
        /// <param name="argEn">BankProfile Entity as Input.</param>
        /// <returns>Returns a Boolean</returns>
        public bool IsValid(BankProfileEn argEn)
        {
            try
            {
                if (argEn.BankDetailsCode == null || argEn.BankDetailsCode.ToString().Length <= 0)
                    throw new Exception("BankDetailsCode Is Required!");
                if (argEn.Description == null || argEn.Description.ToString().Length <= 0)
                    throw new Exception("Description Is Required!");
                if (argEn.ACCode == null || argEn.ACCode.ToString().Length <= 0)
                    throw new Exception("ACCode Is Required!");
                if (argEn.GLCode == null || argEn.GLCode.ToString().Length <= 0)
                    throw new Exception("GLCode Is Required!");
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }

}
