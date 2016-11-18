using System;
using System.Text;
using System.Transactions;
using System.Collections.Generic;
using HTS.SAS.Entities;
using HTS.SAS.DataAccessObjects;
using System.Data.SqlClient;

namespace HTS.SAS.BusinessObjects
{
    /// <summary>
    /// Business Class to handle all the AFC Transactions.
    /// </summary>
    public class AFCBAL
    {
        /// <summary>
        /// Method to Check AFC if Exists
        /// </summary>
        /// <param name="argEn">AFC Entity is an Input.</param>
        /// <returns>Returns AFC Entity</returns>
        public AFCEn CheckAFC(AFCEn argEn)
        {
            try
            {
                AFCDAL loDs = new AFCDAL();
                return loDs.CheckAFC(argEn);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public string CheckNewStudentAFC(AFCEn argEn)
        {
            try
            {
                AFCDAL loDs = new AFCDAL();
                return loDs.CheckNewStudentAFC(argEn);
            }
            catch (Exception err)
            {
                
                throw err;
            }
        }
        /// <summary>
        /// Method to Get List of AFC
        /// </summary>
        /// <param name="argEn">AFC Entity is an Input.</param>
        /// <returns>Returns List of AFC Entities</returns>
        public List<AFCEn> GetList(AFCEn argEn)
        {
            try
            {
                AFCDAL loDs = new AFCDAL();
                return loDs.GetList(argEn);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Method to Edit AFC
        /// </summary>
        /// <param name="argEn">AFC Entity is an Input</param>
        /// <param name="RecStatus">RecStatus is an Input</param>
        /// <param name="batch">Batch is an Input</param>
        /// <returns>Returns BatchCode</returns>
        public string AFCEdit(AFCEn argEn, string RecStatus, string batch)
        {
            try
            {
                AFCDAL loDs = new AFCDAL();
                return loDs.AFCNEW(argEn, RecStatus, batch);
            }
            catch (Exception ex)
            {
                throw ex;

            }
        }
        /// <summary>
        /// Method to Generate AFC
        /// </summary>
        /// <param name="argEn">AFC Entity is an Input</param>
        /// <param name="RecStatus">RecStatus is an Input</param>
        /// <param name="batch">Batch is an Input</param>
        /// <returns>Returns BatchCode</returns>
        public string AFCNEW(AFCEn argEn, string RecStatus, string batch)
        {
            //Transaction Scope added by Solomon
            string NewAFC = string.Empty;
            //using (TransactionScope ts = new TransactionScope())
            //{
                try
                {
                    AFCDAL loDs = new AFCDAL();
                    loDs.BatchDelete(argEn, "Check");
                    NewAFC = loDs.AFCNEW(argEn, RecStatus, batch);
                    //ts.Complete();
                    //if (Transaction.Current.TransactionInformation.Status == TransactionStatus.Committed)
                    //{
                    return NewAFC;
                    //}
                    //else
                    //{
                    //    ts.Dispose();
                    //    throw new TransactionException("Transaction is lost Record is not saved");
                    //}
                }
                catch (Exception ex)
                {
                    //ts.Dispose();
                    throw ex;
                }
            //}

        }
        /// <summary>
        /// Method to Get an AFC Entity
        /// </summary>
        /// <param name="argEn">AFC Entity is an Input</param>
        /// <returns>Returns AFC Entity</returns>
        public AFCEn GetItem(AFCEn argEn)
        {
            try
            {
                AFCDAL loDs = new AFCDAL();
                return loDs.GetItem(argEn);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Method to Get an AFC Entity For Billing
        /// </summary>
        /// <param name="argEn">AFC Entity is an Input</param>
        /// <returns>Returns AFC Entity</returns>
        public AFCEn GetItemJBilling(string batch)
        {
            try
            {
                AFCDAL loDs = new AFCDAL();
                return loDs.GetItemJBilling(batch);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Method to Insert 
        /// </summary>
        /// <param name="argEn">AFC Entity is an Input.</param>
        /// <returns>Returns Boolean</returns>
        public bool Insert(AFCEn argEn)
        {
            bool flag;
            using (TransactionScope ts = new TransactionScope())
            {
                try
                {
                    AFCDAL loDs = new AFCDAL();
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
        /// Method to Insert JBilling
        /// </summary>
        /// <param name="argEn">AFC Entity is an Input.</param>
        /// <returns>Returns Boolean</returns>
        public bool InsertJBitBilling(AFCEn argEn,string batch)
        {
            bool flag;
            using (TransactionScope ts = new TransactionScope())
            {
                try
                {
                    AFCDAL loDs = new AFCDAL();
                    flag = loDs.InsertJBitBilling(argEn,batch);
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
        /// Method to Insert JBilling Details
        /// </summary>
        /// <param name="argEn">AFC Entity Details is an Input.</param>
        /// <returns>Returns Boolean</returns>
        public bool InsertJBitBillingDetails(AFCEn argEn)
        {
            bool flag;
            using (TransactionScope ts = new TransactionScope())
            {
                try
                {
                    AFCDAL loDs = new AFCDAL();
                    flag = loDs.InsertJBitBillingDetails(argEn);
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
        /// <param name="argEn">AFC Entity is an Input.</param>
        /// <returns>Returns Boolean</returns>
        public bool Update(AFCEn argEn)
        {
            bool flag;
            using (TransactionScope ts = new TransactionScope())
            {
                try
                {
                    AFCDAL loDs = new AFCDAL();
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
        /// Method to Delete AFC
        /// </summary>
        /// <param name="argEn">AFC Entity is an Input.</param>
        /// <returns>Returns Boolean</returns>
        public bool Delete(AFCEn argEn)
        {
            bool flag;
            using (TransactionScope ts = new TransactionScope())
            {
                try
                {
                    AFCDAL loDs = new AFCDAL();
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
        /// Method to Delete Batch AFC
        /// </summary>
        /// <param name="argEn">AFC Entity is an Input.</param>
        /// <returns>Returns Boolean</returns>
        public bool BatchDelete(AFCEn argEn, String Delete)
        {
            bool flag;
            using (TransactionScope ts = new TransactionScope())
            {
                try
                {
                    AFCDAL loDs = new AFCDAL();
                    flag = loDs.BatchDelete(argEn, Delete);
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
        /// <param name="argEn">AFC Entity as Input.</param>
        /// <returns>Returns a Boolean</returns>
        public bool IsValid(AFCEn argEn)
        {
            try
            {
                if (argEn.TransCode == null || argEn.TransCode.ToString().Length <= 0)
                    throw new Exception("TransCode Is Required!");
                if (argEn.AFCode == null || argEn.AFCode.ToString().Length <= 0)
                    throw new Exception("AFCode Is Required!");
                if (argEn.Bdate == null || argEn.Bdate.ToString().Length <= 0)
                    throw new Exception("Bdate Is Required!");
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string FetchBatchNumber(string Faculty,string Semester,string Programe)
        {
            AFCDAL objDAL = new AFCDAL();
            return objDAL.FetchBatchNumber(Faculty, Semester, Programe);
        }
        public string FetchBatchNumberReport(string Faculty, string Semester, string Programe)
        {
            AFCDAL objDAL = new AFCDAL();
            return objDAL.FetchBatchNumberReport(Faculty, Semester, Programe);
        }
        public string IsPosted(string Faculty, string Semester, string Programe)
        {
            AFCDAL objDAL = new AFCDAL();
            return objDAL.IsPosted(Faculty, Semester, Programe);
        }

    }

}
