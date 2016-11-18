using System;
using System.Text;
using System.Transactions;
using System.Collections.Generic;
using HTS.SAS.Entities;
using HTS.SAS.DataAccessObjects;

namespace HTS.SAS.BusinessObjects
{
    /// <summary>
    /// Business Class to handle all the DunningLetters.
    /// </summary>
    public class DunningLettersBAL
    {
        /// <summary>
        /// Method to Get DunningLetters 
        /// </summary>
        /// <param name="argEn">DunningLetters Entity is an Input.</param>
        /// <returns>Returns List of DunningLetters.</returns>
        public List<DunningLettersEn> GetListDunning(DunningLettersEn argEn)
        {
            try
            {
                DunningLettersDAL loDs = new DunningLettersDAL();
                return loDs.GetListDunning(argEn);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Method to Get DunningLetters 
        /// </summary>
        /// <param name="argEn">DunningLetters Entity is an Input.</param>
        /// <returns>Returns List of DunningLetters.</returns>
        public List<DunningLettersEn> CheckDunningListing(DunningLettersEn argEn, string MatricNo)
        {
            try
            {
                DunningLettersDAL loDs = new DunningLettersDAL();
                return loDs.CheckDunningListing(argEn,MatricNo);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Method to Get DunningLetters 
        /// </summary>
        /// <param name="argEn">DunningLetters Entity is an Input.</param>
        /// <returns>Returns List of DunningLetters.</returns>
        public List<DunningLettersEn> CheckListDunning(DunningLettersEn argEn)
        {
            try
            {
                DunningLettersDAL loDs = new DunningLettersDAL();
                return loDs.CheckListDunning(argEn);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Method to Get Dunning Letter 
        /// </summary>
        /// <param name="argEn">DunningLetters Entity is an Input.</param>
        /// <returns>Returns List of DunningLettersWarning.</returns>
        public List<DunningLettersEn> ListDunningWarning(DunningLettersEn argEn)
        {
            try
            {
                DunningLettersDAL loDs = new DunningLettersDAL();
                return loDs.ListDunningWarning(argEn);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Method to Get DunningLetters 
        /// </summary>
        /// <param name="argEn">DunningLetters Entity is an Input.Code and Title as Input Properties.</param>
        /// <returns>Returns List of DunningLetters.</returns>
        public List<DunningLettersEn> GetList(DunningLettersEn argEn)
        {
            try
            {
                DunningLettersDAL loDs = new DunningLettersDAL();
                return loDs.GetList(argEn);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Method to Get DunningLetters Student List
        /// </summary>
        /// <param name="argEn">DunningLetters Entity is an Input.Code and Title as Input Properties.</param>
        /// <returns>Returns List of DunningLetters.</returns>
        public List<DunningLettersEn> GetListStudent(DunningLettersEn argEn)
        {
            try
            {
                DunningLettersDAL loDs = new DunningLettersDAL();
                return loDs.GetListStudent(argEn);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Method to Get DunningLetters Warning
        /// </summary>
        /// <param name="argEn">DunningLetters Entity is an Input.Code and Title as Input Properties.</param>
        /// <returns>Returns List of DunningLetters.</returns>
        public List<DunningLettersEn> GetListWarning(DunningLettersEn argEn)
        {
            try
            {
                DunningLettersDAL loDs = new DunningLettersDAL();
                return loDs.GetListWarning(argEn);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Method to Get DunningLetters Item
        /// </summary>
        /// <param name="argEn">DunningLetters Entity is an Input.SADL_Code  as Input Property.</param>
        /// <returns>Returns a DunningLetters Item</returns>
        public DunningLettersEn GetItem(DunningLettersEn argEn)
        {
            try
            {
                DunningLettersDAL loDs = new DunningLettersDAL();
                return loDs.GetItem(argEn);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Method to Insert DunningLetters Item
        /// </summary>
        /// <param name="argEn">DunningLetters Entity is an Input.SADL_Code  as Input Property.</param>
        /// <returns></returns>
        public bool InsertDunning(DunningLettersEn argEn)
        {
            bool flag;
            using (TransactionScope ts = new TransactionScope())
            {
                try
                {
                    DunningLettersDAL loDs = new DunningLettersDAL();
                    flag = loDs.InsertDunning(argEn);
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
        /// Method to Insert DunningLetters
        /// </summary>
        /// <param name="argEn">DunningLetters Entity is the Input.</param>
        /// <returns>Returns a Boolean</returns>
        public bool Insert(DunningLettersEn argEn)
        {
            bool flag;
            using (TransactionScope ts = new TransactionScope())
            {
                try
                {
                    DunningLettersDAL loDs = new DunningLettersDAL();
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
        /// Method to Update DunningLetters
        /// </summary>
        /// <param name="argEn">DunningLetters Entity is the Input.</param>
        /// <returns>Returns a Boolean</returns>
        public bool Update(DunningLettersEn argEn)
        {
            bool flag;
            using (TransactionScope ts = new TransactionScope())
            {
                try
                {
                    DunningLettersDAL loDs = new DunningLettersDAL();
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
        ///  Method to Delete  DunningLetters 
        /// </summary>
        /// <param name="argEn">DunningLetters Entity is an Input.SADL_Code is Property.</param>
        /// <returns>Returns a Boolean</returns>
        public bool Delete(DunningLettersEn argEn)
        {
            bool flag;
            using (TransactionScope ts = new TransactionScope())
            {
                try
                {
                    DunningLettersDAL loDs = new DunningLettersDAL();
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
        /// <param name="argEn">DunningLetters Entity as Input.</param>
        /// <returns>Returns a Boolean</returns>
        public bool IsValid(DunningLettersEn argEn)
        {
            try
            {
                if (argEn.Code == null || argEn.Code.ToString().Length <= 0)
                    throw new Exception("Code Is Required!");
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }

}

