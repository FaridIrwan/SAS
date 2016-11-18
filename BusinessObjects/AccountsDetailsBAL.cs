using System;
using System.Text;
using System.Transactions;
using System.Collections.Generic;
using HTS.SAS.Entities;
using HTS.SAS.DataAccessObjects;

namespace HTS.SAS.BusinessObjects
{
    /// <summary>
    /// Business Class to handle all the AccountingDetails Transactions.
    /// </summary>
    public class AccountsDetailsBAL
    {
        /// <summary>
        /// Method to Get List Of AccountDetails Entity
        /// </summary>
        /// <param name="argEn">AccountDetails Entity is an Input</param>
        /// <returns>Returns List of AccountsDetails Entities</returns>
        public List<AccountsDetailsEn> GetList(AccountsDetailsEn argEn)
        {
            try
            {
                AccountsDetailsDAL loDs = new AccountsDetailsDAL();
                return loDs.GetList(argEn);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Method to Get Student Allocations
        /// </summary>
        /// <param name="argEn">AccountDetails Entity is an Input.TransactionID is an Input Property</param>
        /// <returns>Returns List of AccountsDetails Entities</returns>
        public List<AccountsDetailsEn> GetStuDentAllocation(AccountsDetailsEn argEn)
        {
            try
            {
                AccountsDetailsDAL loDs = new AccountsDetailsDAL();
                return loDs.GetStuDentAllocation(argEn);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        
            /// <summary>
        /// Method to Get Student AccountDetails
        /// </summary>
        /// <param name="argEn">AccountsDetails Entity is an Input.TransactionId is an Input Property</param>
        /// <returns>Returns List Of AccountDetails Entities</returns>
        public List<AccountsDetailsEn> GetListStudentSponsorAlloc(string batchNo)
        {
            try
            {
                AccountsDetailsDAL loDs = new AccountsDetailsDAL();
                AccountsDetailsEn argEn = new AccountsDetailsEn();
                return loDs.GetListStudentSponsorAlloc(batchNo);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Method to Get Succeed Transaction
        /// </summary>
        /// <param name="argEn">Accounts Entity is an Input</param>
        /// <returns>Returns List of Accounts</returns>
        public List<AccountsDetailsEn> GetItemDetails(AccountsEn argEn)
        {
            try
            {
                AccountsDetailsDAL loDs = new AccountsDetailsDAL();
                return loDs.GetItemDetails(argEn);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Method to Get Student AccountDetails
        /// </summary>
        /// <param name="argEn">AccountsDetails Entity is an Input.TransactionId is an Input Property</param>
        /// <returns>Returns List Of AccountDetails Entities</returns>
        public List<AccountsDetailsEn> GetStudentAccountsDetails(AccountsDetailsEn argEn)
        {
            try
            {
                AccountsDetailsDAL loDs = new AccountsDetailsDAL();
                return loDs.GetStudentAccountsDetails(argEn);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Method to Get Student AccountDetails
        /// </summary>
        /// <param name="argEn">AccountsDetails Entity is an Input.TransactionId is an Input Property</param>
        /// <returns>Returns List Of AccountDetails Entities</returns>
        public List<AccountsDetailsEn> GetStudentAccountsDetailsWithTaxAmount(AccountsDetailsEn argEn)
        {
            try
            {
                AccountsDetailsDAL loDs = new AccountsDetailsDAL();
                return loDs.GetStudentAccountsDetailsWithTaxAmount(argEn);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Method to Get Student AccountDetails
        /// </summary>
        /// <param name="argEn">AccountsDetails Entity is an Input.TransactionId is an Input Property</param>
        /// <returns>Returns List Of AccountDetails Entities</returns>
        //public List<AccountsDetailsEn> GetStudentAccountsDetailsByBatchCode(AccountsEn argEn)
        //{
        //    try
        //    {
        //        AccountsDetailsDAL loDs = new AccountsDetailsDAL();
        //        return loDs.GetStudentAccountsDetailsByBatchCode(argEn);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}
        public List<AccountsDetailsEn> GetStudentAccountsDetailsByBatchCode(AccountsEn argEn, String m_no)
        {
            try
            {
                AccountsDetailsDAL loDs = new AccountsDetailsDAL();
                return loDs.GetStudentAccountsDetailsByBatchCode(argEn, m_no);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Method to Get an AccountsDetails Entity
        /// </summary>
        /// <param name="argEn">AccountDetails Entity is an Input</param>
        /// <returns>Returns AccountDetails Entity</returns>
        public AccountsDetailsEn GetItem(AccountsDetailsEn argEn)
        {
            try
            {
                AccountsDetailsDAL loDs = new AccountsDetailsDAL();
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
        /// <param name="argEn">AccountsDetails Entity is an Input.</param>
        /// <returns>Returns Boolean</returns>
        public bool Insert(AccountsDetailsEn argEn)
        {
            bool flag;
            using (TransactionScope ts = new TransactionScope())
            {
                try
                {
                    AccountsDetailsDAL loDs = new AccountsDetailsDAL();
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
        /// <param name="argEn">AccountsDetails Entity is an Input.</param>
        /// <returns>Returns Boolean</returns>
        public bool Update(AccountsDetailsEn argEn)
        {
            bool flag;
            using (TransactionScope ts = new TransactionScope())
            {
                try
                {
                    AccountsDetailsDAL loDs = new AccountsDetailsDAL();
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
        /// Method to Delete 
        /// </summary>
        /// <param name="argEn">AccountsDetails Entity is an Input.</param>
        /// <returns>Returns Boolean</returns>
        public bool Delete(AccountsDetailsEn argEn)
        {
            bool flag;
            using (TransactionScope ts = new TransactionScope())
            {
                try
                {
                    AccountsDetailsDAL loDs = new AccountsDetailsDAL();
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
        /// <param name="argEn">AccountsDetails Entity as Input.</param>
        /// <returns>Returns a Boolean</returns>
        public bool IsValid(AccountsDetailsEn argEn)
        {
            try
            {
                if (argEn.TransactionID == null || argEn.TransactionID.ToString().Length <= 0)
                    throw new Exception("TransactionID Is Required!");
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        
        /// <summary>
        /// Method to Get Student Sponsor Invoice AccountsDetails
        /// </summary>
        /// <param name="argEn">AccountsDetails Entity is an Input.TransactionId is an Input Property</param>
        /// <returns>Returns List Of AccountDetails Entities</returns>
        public List<AccountsDetailsEn> GetStudentSponsorInvoiceAccountsDetails(AccountsDetailsEn argEn)
        {
            try
            {
                AccountsDetailsDAL loDs = new AccountsDetailsDAL();
                return loDs.GetStudentSponsorInvoiceAccountsDetails(argEn);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Method to Get Student Sponsor Invoice AccountsDetails
        /// </summary>
        /// <param name="argEn">AccountsDetails Entity is an Input.TransactionId is an Input Property</param>
        /// <returns>Returns List Of AccountDetails Entities</returns>
        public List<AccountsDetailsEn> GetStudentSponsorInvoiceAccountsDetailsByBatchCode(AccountsEn argEn)
        {
            try
            {
                AccountsDetailsDAL loDs = new AccountsDetailsDAL();
                return loDs.GetStudentSponsorInvoiceAccountsDetailsByBatchCode(argEn);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Method to GetStudentListBasedonReceiptNoo
        /// </summary>
        /// <param name="argEn">AccountDetails Entity is an Input.TransactionID is an Input Property</param>
        /// <returns>Returns List of GetStudentListBasedonReceiptNoo Entities</returns>
        public List<AccountsDetailsEn> GetStudentListBasedonReceiptNoo(AccountsDetailsEn argEn)
        {
            try
            {
                AccountsDetailsDAL loDs = new AccountsDetailsDAL();
                return loDs.GetStudentListBasedonReceiptNoo(argEn);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Method to GetStuDentAllocationPocketAmount
        /// </summary>
        /// <param name="argEn">AccountDetails Entity is an Input.TransactionID is an Input Property</param>
        /// <returns>Returns List of AccountsDetails Entities</returns>
        public List<AccountsDetailsEn> GetStuDentAllocationPocketAmount(AccountsEn argEn)
        {
            try
            {
                AccountsDetailsDAL loDs = new AccountsDetailsDAL();
                return loDs.GetStuDentAllocationPocketAmount(argEn);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Method to Get Student Allocations
        /// </summary>
        /// <param name="argEn">AccountDetails Entity is an Input.TransactionID is an Input Property</param>
        /// <returns>Returns List of AccountsDetails Entities</returns>
        public List<AccountsDetailsEn> GetStudentPaymentAllocation(AccountsDetailsEn argEn)
        {
            try
            {
                AccountsDetailsDAL loDs = new AccountsDetailsDAL();
                return loDs.GetStudentPaymentAllocation(argEn);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }

}
