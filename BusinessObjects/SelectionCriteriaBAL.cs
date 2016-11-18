using System;
using System.Text;
using System.Transactions;
using System.Collections.Generic;
using HTS.SAS.Entities;
using HTS.SAS.DataAccessObjects;


namespace HTS.SAS.BusinessObjects
{
    /// <summary>
    /// Business Class to handle all the SelectionCriteria Methods.
    /// </summary>
    public class SelectionCriteriaBAL
    {
        /// <summary>
        /// Method to Get SelectionCriteria
        /// </summary>
        /// <param name="argEn">SelectionCriteria Entity is an Input.</param>
        /// <returns>Returns SelectionCriteria</returns>
        public SelectionCriteriaEn GetSC(SelectionCriteriaEn argEn)
        {
            try
            {
                SelectionCriteriaDAL loDs = new SelectionCriteriaDAL();
                return loDs.GetSCByBatchCode(argEn);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
               
        /// <summary>
        /// Method to Insert SelectionCriteria 
        /// </summary>
        /// <param name="argEn">SelectionCriteria Entity is an Input.</param>
        /// <returns>Returns Boolean</returns>
        public bool Insert(SelectionCriteriaEn argEn)
        {
            bool flag;
            using (TransactionScope ts = new TransactionScope())
            {
                try
                {
                    SelectionCriteriaDAL loDs = new SelectionCriteriaDAL();
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
        /// Method to Update SelectionCriteria 
        /// </summary>
        /// <param name="argList">SelectionCriteria Entity is an Input.</param>
        /// <returns>Returns Boolean</returns>
        public bool Update(SelectionCriteriaEn argList)
        {
            bool flag;
            using (TransactionScope ts = new TransactionScope())
            {
                try
                {
                    SelectionCriteriaDAL loDs = new SelectionCriteriaDAL();
                    flag = loDs.Update(argList);
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
        /// Method to Delete Faculty 
        /// </summary>
        /// <param name="argEn">Faculty Entity is an Input.</param>
        /// <returns>Returns Boolean</returns>
        public bool Delete(SelectionCriteriaEn argEn)
        {
            bool flag;
            using (TransactionScope ts = new TransactionScope())
            {
                try
                {
                    SelectionCriteriaDAL loDs = new SelectionCriteriaDAL();
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
//---------------------------------------------------------------------------------

//---------------------------------------------------------------------------------