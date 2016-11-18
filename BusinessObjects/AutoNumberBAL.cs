using System;
using System.Text;
using System.Transactions;
using System.Collections.Generic;
using HTS.SAS.Entities;
using HTS.SAS.DataAccessObjects;

namespace HTS.SAS.BusinessObjects
{
    /// <summary>
    /// Business Class to handle all the AutoNumber Methods.
    /// </summary>
    public class AutoNumberBAL
    {
        /// <summary>
        /// Method to Get List of AutoNumber
        /// </summary>
        /// <param name="argEn">AutoNumber Entity is an Input.</param>
        /// <returns>Returns List of AutoNumber Entities</returns>
        public List<AutoNumberEn> GetList(AutoNumberEn argEn)
        {
            try
            {
                AutoNumberDAL loDs = new AutoNumberDAL();
                return loDs.GetList(argEn);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Method to Get List Of all AutoNumbers
        /// </summary>
        /// <param name="argEn">AutoNumber Entity is an Input.SAAN_Des,SAAN_NoDigit,SAAN_Prefix and SAAN_StartNo are Input Properties.</param>
        /// <returns>Returns List of AutoNumber Entities</returns>
        public List<AutoNumberEn> GetAutoNumberList(AutoNumberEn argEn)
        {
            try
            {
                AutoNumberDAL loDs = new AutoNumberDAL();
                return loDs.GetAutoNumberList(argEn);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Method to Get an AutoNumber Entity
        /// </summary>
        /// <param name="argEn">AutoNumber Entity is an Input</param>
        /// <returns>Returns AutoNumber Entity</returns>
        public AutoNumberEn GetItem(AutoNumberEn argEn)
        {
            try
            {
                AutoNumberDAL loDs = new AutoNumberDAL();
                return loDs.GetItem(argEn);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Method to Insert 
        /// </summary>
        /// <param name="argEn">AutoNumber Entity is an Input.</param>
        /// <returns>Returns Boolean</returns>
        public bool Insert(AutoNumberEn argEn)
        {
            bool flag;
            using (TransactionScope ts = new TransactionScope())
            {
                try
                {
                    AutoNumberDAL loDs = new AutoNumberDAL();
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
        /// Method to Update 
        /// </summary>
        /// <param name="argEn">AutoNumber Entity is an Input.</param>
        /// <returns>Returns Boolean</returns>
        public bool Update(AutoNumberEn argEn)
        {
            bool flag;
            using (TransactionScope ts = new TransactionScope())
            {
                try
                {
                    AutoNumberDAL loDs = new AutoNumberDAL();
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
        /// Method to Update AutoNumber 
        /// </summary>
        /// <param name="argEn">AutoNumber Entity is an Input.</param>
        /// <returns>Returns Boolean</returns>
        public bool UpdateAutoNumber(AutoNumberEn argEn)
        {
            bool flag;
            using (TransactionScope ts = new TransactionScope())
            {
                try
                {
                    AutoNumberDAL loDs = new AutoNumberDAL();
                    flag = loDs.UpdateAutoNumber(argEn);
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
        /// Method to Delete 
        /// </summary>
        /// <param name="argEn">AutoNumber Entity is an Input.</param>
        /// <returns>Returns Boolean</returns>
        public bool Delete(AutoNumberEn argEn)
        {
            bool flag;
            using (TransactionScope ts = new TransactionScope())
            {
                try
                {
                    AutoNumberDAL loDs = new AutoNumberDAL();
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

        


    }
}
