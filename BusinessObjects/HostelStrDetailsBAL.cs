using System;
using System.Text;
using System.Transactions;
using System.Collections.Generic;
using HTS.SAS.Entities;
using HTS.SAS.DataAccessObjects;

namespace HTS.SAS.BusinessObjects
{
    /// <summary>
    /// Business Class to handle all the HostelStructureDetails Methods.
    /// </summary>
    public class HostelStrDetailsBAL
    {
        /// <summary>
        /// Method to Get List of HostelStrDetails
        /// </summary>
        /// <param name="argEn">HostelStrDetails Entity is an Input.</param>
        /// <returns>Returns List of HostelStrDetails</returns>
        public List<HostelStrDetailsEn> GetList(HostelStrDetailsEn argEn)
        {
            try
            {
                HostelStrDetailsDAL loDs = new HostelStrDetailsDAL();
                return loDs.GetList(argEn);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Method to Get HostelStrDetails Entity
        /// </summary>
        /// <param name="argEn">HostelStrDetails Entity is an Input.HSCode as Input Property.</param>
        /// <returns>Returns HostelStrDetails Entity</returns>
        public HostelStrDetailsEn GetItem(HostelStrDetailsEn argEn)
        {
            try
            {
                HostelStrDetailsDAL loDs = new HostelStrDetailsDAL();
                return loDs.GetItem(argEn);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Method to Insert HostelStrDetails 
        /// </summary>
        /// <param name="argEn">HostelStrDetails Entity is an Input.</param>
        /// <returns>Returns Boolean</returns>
        public bool Insert(HostelStrDetailsEn argEn)
        {
            bool flag;
            using (TransactionScope ts = new TransactionScope())
            {
                try
                {
                    HostelStrDetailsDAL loDs = new HostelStrDetailsDAL();
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
        /// Method to Update HostelStrDetails 
        /// </summary>
        /// <param name="argEn">HostelStrDetails Entity is an Input.</param>
        /// <returns>Returns Boolean</returns>
        public bool Update(HostelStrDetailsEn argEn)
        {
            bool flag;
            using (TransactionScope ts = new TransactionScope())
            {
                try
                {
                    HostelStrDetailsDAL loDs = new HostelStrDetailsDAL();
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
        /// Method to Delete HostelStrDetails 
        /// </summary>
        /// <param name="argEn">HostelStrDetails Entity is an Input.</param>
        /// <returns>Returns Boolean</returns>
        public bool Delete(HostelStrDetailsEn argEn)
        {
            bool flag;
            using (TransactionScope ts = new TransactionScope())
            {
                try
                {
                    HostelStrDetailsDAL loDs = new HostelStrDetailsDAL();
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
        /// <param name="argEn">HostelStrDetails Entity is as Input.</param>
        /// <returns>Returns a Boolean</returns>
        public bool IsValid(HostelStrDetailsEn argEn)
        {
            try
            {
                if (argEn.HSCode == null || argEn.HSCode.ToString().Length <= 0)
                    throw new Exception("HSCode Is Required!");
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
