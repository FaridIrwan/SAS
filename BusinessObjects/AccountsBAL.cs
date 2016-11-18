using System;
using System.Text;
using System.Transactions;
using System.Collections.Generic;
using HTS.SAS.Entities;
using HTS.SAS.DataAccessObjects;

namespace HTS.SAS.BusinessObjects
{
    /// <summary>
    /// Business Class to handle all the Accounting Transactions.
    /// </summary>
    public class AccountsBAL
    {
        /// <summary>
        /// Method to Get an Account Item by TransCode
        /// </summary>
        /// <param name="argEn">Accounts Entity is the Input.TransCode is the Input Property</param>
        /// <returns>Returns an Account Item</returns>
        public AccountsEn GetItemByTransCode(AccountsEn argEn)
        {

            try
            {
                AccountsDAL loDs = new AccountsDAL();
                return loDs.GetItemByTransCode(argEn);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        /// <summary>
        /// Method to Get Transactions
        /// </summary>
        /// <param name="argEn">Accounts Entity is the Input.BatchCode,Category,PostStatus and SubType are the Input Property</param>
        /// <returns>Returns List of Accounts</returns>
        public List<AccountsEn> GetTransactions(AccountsEn argEn)
        {
            try
            {
                AccountsDAL loDs = new AccountsDAL();
                return loDs.GetTransactions(argEn);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Method to Get Transactions
        /// </summary>
        /// <param name="argEn">Accounts Entity is the Input.BatchCode,Category,PostStatus and SubType are the Input Property</param>
        /// <returns>Returns List of Accounts</returns>
        public bool SucceedTransaction(AccountsEn argEn)
        {
            try
            {
                AccountsDAL loDs = new AccountsDAL();
                return loDs.SucceedTransaction(argEn);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Method to Get ExportData
        /// </summary>
        /// <param name="argEn">Accounts Entity is an Input.Category , Subtype,FromDate and toDate are Input properties</param>
        /// <returns>Returns List of Accounts</returns>
        public List<AccountsEn> GetExportData(AccountsEn argEn)
        {
            try
            {
                AccountsDAL loDs = new AccountsDAL();
                return loDs.GetExportData(argEn);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Method to Get GL Journal ExportData
        /// </summary>
        /// <param name="argEn">Accounts Entity is an Input.Category , Subtype,FromDate and toDate are Input properties</param>
        /// <returns>Returns List of Accounts</returns>
        public List<AccountsEn> GetStGLJournalExportData(AccountsEn argEn)
        {
            try
            {
                AccountsDAL loDs = new AccountsDAL();
                return loDs.GetStGLJournalExportData(argEn);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Method to Get Sponsor CB Receipts ExportData
        /// </summary>
        /// <param name="argEn">Accounts Entity is an Input.Category , Subtype,FromDate and toDate are Input properties</param>
        /// <returns>Returns List of Accounts</returns>
        public List<AccountsEn> GetSponsorCBReciptExportData(AccountsEn argEn)
        {
            try
            {
                AccountsDAL loDs = new AccountsDAL();
                return loDs.GetSponsorCBReciptExportData(argEn);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Method to Get Sponsor GL Journal ExportData
        /// </summary>
        /// <param name="argEn">Accounts Entity is an Input.Category , Subtype,FromDate and toDate are Input properties</param>
        /// <returns>Returns List of Accounts</returns>
        public List<AccountsEn> GetSponsorGLJournalExportData(AccountsEn argEn)
        {
            try
            {
                AccountsDAL loDs = new AccountsDAL();
                return loDs.GetSponsorGLJournalExportData(argEn);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Method to Get CBPaymentExportData
        /// </summary>
        /// <param name="argEn">Accounts Entity is an Input.Category , Subtype,FromDate and toDate are Input properties</param>
        /// <returns>Returns List of Accounts</returns>
        public List<AccountsEn> GetStCBPaymentExportData(AccountsEn argEn)
        {
            try
            {
                AccountsDAL loDs = new AccountsDAL();
                return loDs.GetStCBPaymentExportData(argEn);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Method to Get Payments
        /// </summary>
        /// <param name="argEn">Accounts Entity is the Input.BatchCode,Category,PostStatus and SubType are the Input Property</param>
        /// <returns>Returns List of Accounts</returns>
        public List<AccountsEn> GetPayments(AccountsEn argEn)
        {
            try
            {
                AccountsDAL loDs = new AccountsDAL();
                return loDs.GetPayments(argEn);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Method to Get Sponsor Allocations Payments
        /// </summary>
        /// <param name="argEn">Accounts Entity is the Input.BatchCode,TransStatus,Category,PostStatus and SubType are Input Properties.</param>
        /// <returns>Returns List of Accounts</returns>
        public List<AccountsEn> GetSPAllocationPayments(AccountsEn argEn)
        {
            try
            {
                AccountsDAL loDs = new AccountsDAL();
                return loDs.GetSPAllocationPayments(argEn);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Method to Get Sponsor Allocations 
        /// </summary>
        /// <param name="argEn">Accounts Entity is the Input.BatchCode,TransStatus,Category,PostStatus and SubType are Input Properties.</param>
        /// <returns>Returns List of Accounts</returns>
        public List<AccountsEn> GetSPAllocationTransactions(AccountsEn argEn)
        {
            try
            {
                AccountsDAL loDs = new AccountsDAL();
                return loDs.GetSPAllocationTransactions(argEn);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Method to Get Sum of Student Allocation Amounts
        /// </summary>
        /// <param name="argEn">Accounts Entity is the Input.CreditRef,PostStatus,SubType and Description are Input Properties</param>
        /// <returns>Returns Total Student Allocation Amount</returns>
        public double GetstAllocationAmounts(AccountsEn argEn)
        {
            try
            {
                AccountsDAL loDs = new AccountsDAL();
                return loDs.GetstAllocationAmounts(argEn);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Method to Get Sponsor Receipts Allowcation.
        /// </summary>
        /// <param name="argEn">Sponsor Entity is an Input. Sponsor Code and  Name are the input properties</param>
        /// <returns>List of Sponsor Entity</returns>
        public List<SponsorEn> GetReciptSpAll(SponsorEn argEn)
        {
            try
            {
                AccountsDAL loDs = new AccountsDAL();
                return loDs.GetReciptSpAll(argEn);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Method to Get GetReceiptSpAll
        /// </summary>
        /// <param name="argEn">Sponsor Entity is an Input. Sponsor Code and  Name are the input properties</param>
        /// <returns>List of Sponsor Entity</returns>
        public List<SponsorEn> GetReceiptSpAll(SponsorEn argEn)
        {
            try
            {
                AccountsDAL loDs = new AccountsDAL();
                return loDs.GetReceiptSpAll(argEn);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Method to Get Sponsor Receipts.
        /// </summary>
        /// <param name="argEn">Sponsor Entity is an Input. Sponsor Code and  Name are the input properties</param>
        /// <returns>List of Sponsor Entity</returns>
        public List<SponsorEn> GetRecipt(SponsorEn argEn)
        {
            try
            {
                AccountsDAL loDs = new AccountsDAL();
                return loDs.GetRecipt(argEn);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Method to Get the List of Student Transactions
        /// </summary>
        /// <param name="argEn">Accounts Entity is the Input.BatchCode is the Input Property</param>
        /// <returns>Returns the List OF Accounts</returns>
        public List<AccountsEn> GetListStudentAccounts(AccountsEn argEn)
        {
            try
            {
                AccountsDAL loDs = new AccountsDAL();
                return loDs.GetListStudentAccounts(argEn);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Method to Get the List of Student Transactions
        /// </summary>
        /// <param name="argEn">Accounts Entity is the Input.BatchCode is the Input Property</param>
        /// <returns>Returns the List OF Accounts</returns>
        public List<AccountsDetailsEn> GetAutoAllocationList(AccountsEn argEn)
        {
            try
            {
                AccountsDAL loDs = new AccountsDAL();
                return loDs.GetAutoAllocationList(argEn);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Method to Get Student's Outstanding Amount
        /// </summary>
        /// <param name="argEn">Student Entity is an Input</param>
        /// <returns>Returns the Outstanding Amount</returns>
        public double GetStudentOutstandingAmt(StudentEn argEn)
        {
            try
            {
                AccountsDAL loDs = new AccountsDAL();
                return loDs.GetStudentOutstandingAmt(argEn);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Method to Get Student Allocations
        /// </summary>
        /// <param name="argEn">Student Entity is the Input.</param>
        /// <returns>Returns List of Students</returns>
        public List<StudentEn> GetStudentAllocationTrans(StudentEn argEn)
        {
            try
            {
                AccountsDAL loDs = new AccountsDAL();
                return loDs.GetStudentAllocationTrans(argEn);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Method to Get GetTotalAllocatedAmount
        /// </summary>
        /// <param name="argEn">Student Entity is an Input</param>
        /// <returns>Returns the GetTotalAllocatedAmount</returns>
        public double GetTotalAllocatedAmount(AccountsEn argEn)
        {
            try
            {
                AccountsDAL loDs = new AccountsDAL();
                return loDs.GetTotalAllocatedAmount(argEn);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Method to Get the Student Receipts by BatchID
        /// </summary>
        /// <param name="argEn">Student Entity is the Input.BatchCode is the Input Property</param>
        /// <returns>Returns List of Students</returns>
        public List<StudentEn> GetStudentReceiptsbyBatchID(StudentEn argEn)
        {
            try
            {
                AccountsDAL loDs = new AccountsDAL();
                return loDs.GetStudentReceiptsbyBatchID(argEn);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Method to Get the List of Students by BatchId
        /// </summary>
        /// <param name="argEn">Student Entity is an Input Parameter.BatchCode is the Input Property</param>
        /// <returns>Returns List of Students</returns>
        public List<StudentEn> GetListStudentbyBatchID(StudentEn argEn)
        {
            try
            {
                AccountsDAL loDs = new AccountsDAL();
                return loDs.GetListStudentbyBatchID(argEn);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #region InsertHeaderNoDetails
        //Added by Hafiz Roslan
        //Date: 11/01/2016
        //Insert/update headerno
        public void InsertHeaderNoDetails(AccountsEn argEn, String headerNo)
        {
            AccountsDAL loDs = new AccountsDAL();

            using (TransactionScope ts = new TransactionScope())
            {
                try
                {
                    loDs.InsertHeaderNoDetails(argEn, headerNo);
                    ts.Complete();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }


        #endregion
        /// <summary>
        /// Method to Insert StudentBatch
        /// </summary>
        /// <param name="argEn">Accounts Entity is the Input</param>
        /// <param name="argList">List of Student Entity is the Input</param>
        /// <param name="scArg">List of SelectionCriteria Entity is the Input (Optional)</param>
        /// <param name="isAutoDetails">Account details list will generated (Optional)</param>
        /// <returns>Returns BatchCode</returns>
        public string StudentBatchInsert(AccountsEn argEn, List<StudentEn> argList, SelectionCriteriaEn scArg = null, bool isAutoDetails = false, List<StudentEn> trackingList = null, int trackid = 0)
        {
            AccountsDAL loDs = new AccountsDAL();
            SelectionCriteriaDAL scDAL = new SelectionCriteriaDAL();
            string flag;
            using (TransactionScope ts = new TransactionScope())
            {
                try
                {

                    flag = loDs.StudentBatchInsert(argEn, argList, isAutoDetails);
                    if (scArg != null)
                    {
                        scArg.BatchCode = flag;
                        scDAL.Insert(scArg);
                    }
                    if (trackingList != null && trackid > 0)
                    {
                        StudentDAL stuDAL = new StudentDAL();
                        stuDAL.UpdateTrackingNote(trackingList, trackid);
                    }
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
        /// Method to Update Receipt For Student
        /// </summary>
        /// Added by Hafiz Roslan
        /// Date: 05/01/2016
        public void ReceiptUpdate(AccountsEn argEn, List<StudentEn> argList)
        {
            AccountsDAL loDs = new AccountsDAL();

            using (TransactionScope ts = new TransactionScope())
            {
                try
                {
                    loDs.ReceiptUpdate(argEn, argList);
                    ts.Complete();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        /// <summary>
        /// Method to Update StudentBatch
        /// </summary>
        /// <param name="argEn">Accounts Entity is the Input</param>
        /// <param name="argList">List of Student Entity is the Input</param>
        /// <returns>Returns BatchCode</returns>
        public string StudentBatchUpdate(AccountsEn argEn, List<StudentEn> argList)
        {
            string flag;
            AccountsDAL loDs = new AccountsDAL();
           using (TransactionScope ts = new TransactionScope(TransactionScopeOption.Required, new System.TimeSpan(0, 20, 0)))
            {
                try
                {

                    flag = loDs.StudentBatchUpdate(argEn, argList);                  
                    ts.Complete();

                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            //Update Student OutStanding On Invoice and Receipt
            foreach (StudentEn item in argList)
            {
                using (TransactionScope ts = new TransactionScope(TransactionScopeOption.Required, new System.TimeSpan(0, 20, 0)))
                {

                    try
                    {
                        if (argEn.SubType == "Student") //&& (argEn.Category == "Invoice" || argEn.Category == "Receipt")
                        {
                            loDs.UpdateStudentOutstanding(item.MatricNo);
                            ts.Complete();
                        }
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                }
            }


            return flag;
        }


        /// <summary>
        /// Method to Update StudentBatch
        /// </summary>
        /// <param name="argEn">Accounts Entity is the Input</param>
        /// <param name="argList">List of Student Entity is the Input</param>
        /// <param name="scArg" >SelectionCriteria Entity is the Input(Optional)</param>
        /// <returns>Returns BatchCode</returns>
        public string StudentBatchUpdateEditMode(AccountsEn argEn, List<StudentEn> argList, SelectionCriteriaEn scArg = null)
        {
            string flag;
            AccountsDAL loDs = new AccountsDAL(); 
            SelectionCriteriaDAL scDAL = new SelectionCriteriaDAL();
         
            using (TransactionScope ts = new TransactionScope(TransactionScopeOption.Required, new System.TimeSpan(0, 20, 0)))
            {
                try
                {

                    flag = loDs.StudentBatchUpdate(argEn, argList);
                    if (scArg != null)
                    {
                        scArg.BatchCode = argEn.BatchCode;
                        scDAL.Update(scArg);
                    }
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
        /// Method to Update StudentBatch
        /// </summary>
        /// <param name="argEn">Accounts Entity is the Input</param>
        /// <param name="argList">List of Student Entity is the Input</param>
        /// <param name="scArg" >SelectionCriteria Entity is the Input(Optional)</param>
        /// <param name="isAutoDetails" >Data for Account Details will base on student list(Optional)</param>
        /// <returns>Returns BatchCode</returns>
        public string StudentBatchUpdateEditMode(AccountsEn argEn, List<StudentEn> argList, SelectionCriteriaEn scArg = null, bool isAutoDetails = false)
        {
            string flag;
            AccountsDAL loDs = new AccountsDAL();
            SelectionCriteriaDAL scDAL = new SelectionCriteriaDAL();

            using (TransactionScope ts = new TransactionScope(TransactionScopeOption.Required, new System.TimeSpan(0, 20, 0)))
            {
                try
                {

                    flag = loDs.StudentBatchUpdate(argEn, argList, isAutoDetails);
                    if (scArg != null)
                    {
                        scArg.BatchCode = argEn.BatchCode;
                        scDAL.Update(scArg);
                    }
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
        /// Method to Insert SponsorBatch
        /// </summary>
        /// <param name="argEn">Accounts Entity is the Input</param>
        /// <param name="argList">List of Sponsor Entity is the Input</param>
        /// <returns>Returns BatchCode</returns>
        public string SponsorBatchInsert(AccountsEn argEn, List<SponsorEn> argList)
        {
            string flag;
            using (TransactionScope ts = new TransactionScope())
            {
                try
                {
                    AccountsDAL loDs = new AccountsDAL();
                    flag = loDs.SponsorBatchInsert(argEn, argList);
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
        /// Method to Update SponsorBatchPosting
        /// </summary>
        /// <param name="argEn">Accounts Entity is the Input</param>
        /// <param name="argList">List of Sponsor Entity is the Input</param>
        /// <returns>Returns BatchCode</returns>
        public bool SponsorBatchPost(string BatchCode, string UserId)
        {
            bool flag;
            using (TransactionScope ts = new TransactionScope())
            {
                try
                {
                    AccountsDAL loDs = new AccountsDAL();
                    flag = loDs.SponsorBatchPost(BatchCode, UserId);
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
        /// Method to Update Receipt For Receipt
        /// </summary>
        /// Added by Hafiz Roslan
        /// Date: 06/01/2016
        public void ReceiptUpdate2(AccountsEn argEn, List<SponsorEn> argList)
        {
            AccountsDAL loDs = new AccountsDAL();

            using (TransactionScope ts = new TransactionScope())
            {
                try
                {
                    loDs.ReceiptUpdate2(argEn, argList);
                    ts.Complete();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        /// <summary>
        /// Method to Update SponsorBatch
        /// </summary>
        /// <param name="argEn">Accounts Entity is the Input</param>
        /// <param name="argList">List of Sponsor Entity is the Input</param>
        /// <returns>Returns BatchCode</returns>
        public string SponsorBatchUpdate(AccountsEn argEn, List<SponsorEn> argList)
        {
            string flag;
            AccountsDAL loDs = new AccountsDAL();
            using (TransactionScope ts = new TransactionScope())
            {
                try
                {
                    flag = loDs.SponsorBatchUpdate(argEn, argList);
                    ts.Complete();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

            if (argEn.AccountDetailsList != null)
            {
                //Update Student OutStanding On Allocation
                foreach (AccountsDetailsEn item in argEn.AccountDetailsList)
                {
                    using (TransactionScope ts = new TransactionScope())
                    {
                        try
                        {
                            if (argEn.Category == "Allocation" && argEn.PostStatus == "Posted")
                            {
                                loDs.UpdateStudentOutstanding(item.ReferenceCode);
                                ts.Complete();
                            }
                        }
                        catch (Exception ex)
                        {
                            throw ex;
                        }
                    }
                }
            }

            return flag;
        }

        /// <summary>
        /// Method to Delete Batch
        /// </summary>
        /// <param name="argEn">Accounts Entity is the Input.BatchCode is an Input Property</param>
        /// <returns>Returns a Boolean</returns>
        public bool BatchDelete(AccountsEn argEn)
        {
            bool flag;
            using (TransactionScope ts = new TransactionScope())
            {
                try
                {
                    AccountsDAL loDs = new AccountsDAL();
                    flag = loDs.BatchDelete(argEn);
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
        /// Method to Get the List Of Accounts
        /// </summary>
        /// <param name="argEn">Accounts Entity is the Input</param>
        /// <returns>Returns the List of Accounts</returns>
        public List<AccountsEn> GetList(AccountsEn argEn)
        {
            try
            {
                AccountsDAL loDs = new AccountsDAL();
                return loDs.GetList(argEn);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Method to Get StudentLedger list
        /// </summary>
        /// <param name="argEn">Accounts Entity is an Input.Creditref , Subtype and Poststatus are Input properties</param>
        /// <returns>Returns List of Accounts</returns>
        public List<AccountsEn> GetStudentLedgerList(AccountsEn argEn)
        {
            try
            {
                AccountsDAL loDs = new AccountsDAL();
                return loDs.GetStudentLedgerList(argEn);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Method to Get StudentLedger list
        /// </summary>
        /// <param name="argEn">Accounts Entity is an Input.Creditref , Subtype and Poststatus are Input properties</param>
        /// <returns>Returns List of Accounts</returns>
        public List<AccountsEn> GetStudentLedgerDetailList(AccountsEn argEn)
        {
            try
            {
                AccountsDAL loDs = new AccountsDAL();
                return loDs.GetStudentLedgerDetailList(argEn);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<AccountsEn> GetStudentLoanLedgerDetailList(AccountsEn argEn)
        {
            try
            {
                AccountsDAL loDs = new AccountsDAL();
                return loDs.GetStudentLoanLedgerDetailList(argEn);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Method to Get StudentAutoAllocation list
        /// </summary>
        /// <param name="argEn">Accounts Entity is an Input.Creditref , Subtype and Poststatus are Input properties</param>
        /// <returns>Returns List of Accounts</returns>
        public List<AccountsEn> GetStudentAutoAllocation(AccountsEn argEn)
        {
            try
            {
                AccountsDAL loDs = new AccountsDAL();
                return loDs.GetStudentAutoAllocation(argEn);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Method to Get the List of Sponsors by BatchId
        /// </summary>
        /// <param name="BatchID">BatchID is an Input Parameter.</param>
        /// <returns>Returns List of Sponsors</returns>
        public List<SponsorEn> GetSponserListByBatchID(string BatchID)
        {
            try
            {
                AccountsDAL loDs = new AccountsDAL();
                return loDs.GetSponserListByBatchID(BatchID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Method to Get an Accounts Item
        /// </summary>
        /// <param name="argEn">Accounts Entity is the Input.BatchCode is the Input Property</param>
        /// <returns>Returns an Accounts Item</returns>
        public AccountsEn GetItem(AccountsEn argEn)
        {
            try
            {
                AccountsDAL loDs = new AccountsDAL();
                return loDs.GetItem(argEn);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Method to Get an Succeed Transaction Header Item
        /// </summary>
        /// <param name="argEn">Accounts Entity is the Input</param>
        /// <returns>Returns an Accounts Item</returns>
        public List<AccountsEn> GetListSucceedHeader()
        {
            try
            {
                AccountsDAL loDs = new AccountsDAL();
                return loDs.GetListSucceedHeader();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Method to Get Sponsor Receipt
        /// </summary>
        /// <param name="argEn">Accounts Entity is the Input.TranssactionCode is the Input Property</param>
        /// <returns>Returns an Account Item</returns>
        public AccountsEn GetHeaderPTPTN(string receiptNo)
        {
            try
            {
                AccountsDAL loDs = new AccountsDAL();
                return loDs.GetHeaderPTPTN(receiptNo);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Method to Get Sponsor Receipt
        /// </summary>
        /// <param name="argEn">Accounts Entity is the Input.TranssactionCode is the Input Property</param>
        /// <returns>Returns an Account Item</returns>
        public AccountsEn GetItemReceipt(AccountsEn argEn)
        {
            try
            {
                AccountsDAL loDs = new AccountsDAL();
                return loDs.GetItemReceipt(argEn);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        /// <summary>
        /// Method to Get Sponsor Receipt
        /// </summary>
        /// <param name="argEn">Accounts Entity is the Input.TranssactionCode is the Input Property</param>
        /// <returns>Returns an Account Item</returns>
        public AccountsEn GetItemReceiptAllow(AccountsEn argEn)
        {
            try
            {
                AccountsDAL loDs = new AccountsDAL();
                return loDs.GetItemReceiptAllow(argEn);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Method to Get Transaction Amount
        /// </summary>
        /// <param name="argEn">Accounts Entity is the Input.TranssactionCode is the Input Property</param>
        /// <returns>Returns an Account Item</returns>
        public AccountsEn GetItemTrans(StudentEn argEn)
        {
            try
            {
                AccountsDAL loDs = new AccountsDAL();
                return loDs.GetItemTrans(argEn);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Method to Insert Accounts
        /// </summary>
        /// <param name="argEn">Accounts Entity is the Input</param>
        /// <returns>Returns Boolean</returns>
        public bool Insert(AccountsEn argEn)
        {
            bool flag;
            using (TransactionScope ts = new TransactionScope())
            {
                try
                {
                    AccountsDAL loDs = new AccountsDAL();
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
        /// Method to Update Accounts
        /// </summary>
        /// <param name="argEn">Accounts Entity is the Input</param>
        /// <returns>Returns Boolean</returns>
        public bool Update(AccountsEn argEn)
        {
            bool flag;
            using (TransactionScope ts = new TransactionScope())
            {
                try
                {
                    AccountsDAL loDs = new AccountsDAL();
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
        /// <param name="argEn">Accounts Entity is the Input.BatchCode is an Input Property</param>
        /// <returns>Returns a Boolean</returns>
        public bool Delete(AccountsEn argEn)
        {
            bool flag;
            using (TransactionScope ts = new TransactionScope())
            {
                try
                {
                    AccountsDAL loDs = new AccountsDAL();
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
        /// Method to Delete Succeed Transaction
        /// </summary>
        /// <param name="argEn">Accounts Entity is the Input.Autonumber is an Input Property</param>
        /// <returns>Returns a Boolean</returns>
        public bool DeleteSucceedTrans(AccountsEn argEn)
        {
            bool flag;
            using (TransactionScope ts = new TransactionScope())
            {
                try
                {
                    AccountsDAL loDs = new AccountsDAL();
                    flag = loDs.DeleteSucceedTrans(argEn);
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
        /// <param name="argEn">Accounts Entity as Input.</param>
        /// <returns>Returns a Boolean</returns>

        public bool IsValid(AccountsEn argEn)
        {
            try
            {
                if (argEn.TranssactionID == null || argEn.TranssactionID.ToString().Length <= 0)
                    throw new Exception("TranssactionID Is Required!");
                if (argEn.TempTransCode == null || argEn.TempTransCode.ToString().Length <= 0)
                    throw new Exception("TempTransCode Is Required!");
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Method to Receipt History of Updation and Deltetion 
        /// </summary>
        /// <param name="argEn">Account Entity is an Input.</param>
        /// <returns>Returns Boolean</returns>
        public bool InsertReceiptUserAction(AccountsEn argEn)
        {
            bool flag;
            using (TransactionScope ts = new TransactionScope())
            {
                try
                {
                    AccountsDAL loDs = new AccountsDAL();
                    flag = loDs.InsertReceiptUserAction(argEn);
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
        /// Method to Insert Student Loan
        /// </summary>
        /// <param name="argEn">Accounts Entity is the Input</param>
        /// <returns>Returns BatchCode</returns>
        public string StudentLoanInsert(AccountsEn argEn)
        {
            AccountsDAL loDs = new AccountsDAL();
            string flag;
            using (TransactionScope ts = new TransactionScope())
            {
                try
                {
                    flag = loDs.StudentLoanInsert(argEn);
                    ts.Complete();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

            return flag;
        }

        public string StudentLoanUpdate(AccountsEn argEn, StudentEn argStudent)
        {
            string flag;
            AccountsDAL loDs = new AccountsDAL();
            using (TransactionScope ts = new TransactionScope())
            {
                try
                {

                    flag = loDs.StudentLoanUpdate(argEn, argStudent);
                    ts.Complete();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            //Update Student OutStanding Loan on Receipt
            using (TransactionScope ts = new TransactionScope())
            {
                try
                {
                    if (argEn.Category == "Receipt")
                    {
                        loDs.UpdateStudentOutstandingLoan(argStudent.MatricNo);
                        ts.Complete();
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

            return flag;
        }

        public List<AccountsEn> GetLoanTransactions(AccountsEn argEn)
        {
            try
            {
                AccountsDAL loDs = new AccountsDAL();
                return loDs.GetLoanTransactions(argEn);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Method to Delete Student loan
        /// </summary>
        /// <param name="argEn">Accounts Entity is the Input.BatchCode is an Input Property</param>
        /// <returns>Returns a Boolean</returns>
        public bool BatchLoanDelete(AccountsEn argEn)
        {
            bool flag;
            using (TransactionScope ts = new TransactionScope())
            {
                try
                {
                    AccountsDAL loDs = new AccountsDAL();
                    flag = loDs.DeleteLoan(argEn);
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
        /// Method to Get Receipt History
        /// </summary>
        /// <param name="argEn">Receipt History Properties</param>
        /// <returns>Returns List of Accounts</returns>
        public List<AccountsEn> GetReceiptHistory(string fromDate, string toDate)
        {
            try
            {
                AccountsDAL loDs = new AccountsDAL();
                return loDs.GetReceiptHistory(fromDate, toDate);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #region Sponsor Invoice
        /// <summary>
        /// Method to Insert SponsorInvoice
        /// </summary>
        /// <param name="argEn">Accounts Entity is the Input</param>
        /// <param name="argList">List of Student Entity is the Input</param>
        /// <returns>Returns BatchCode</returns>
        public string SponsorInvoiceInsert(AccountsEn argEn, List<StudentEn> argList, bool isAutoDetails = false)
        {
            AccountsDAL loDs = new AccountsDAL();
            string flag;
            using (TransactionScope ts = new TransactionScope())
            {
                try
                {

                    flag = loDs.SponsorInvoiceInsert(argEn, argList, isAutoDetails);
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
        /// Method to Update SponsorInvoice
        /// </summary>
        /// <param name="argEn">Accounts Entity is the Input</param>
        /// <param name="argList">List of Student Entity is the Input</param>
        /// <returns>Returns BatchCode</returns>
        public string SponsorInvoiceUpdate(AccountsEn argEn, List<StudentEn> argList)
        {
            string flag;
            AccountsDAL loDs = new AccountsDAL();
            using (TransactionScope ts = new TransactionScope())
            {
                try
                {

                    flag = loDs.SponsorInvoiceUpdate(argEn, argList);
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
        /// Method to Update SponsorInvoice
        /// </summary>
        /// <param name="argEn">Accounts Entity is the Input</param>
        /// <param name="argList">List of Student Entity is the Input</param>
        /// <returns>Returns BatchCode</returns>
        public string SponsorInvoiceUpdate(AccountsEn argEn, List<StudentEn> argList, bool isAutoDetails = false)
        {
            string flag;
            AccountsDAL loDs = new AccountsDAL();
            using (TransactionScope ts = new TransactionScope())
            {
                try
                {

                    flag = loDs.SponsorInvoiceUpdate(argEn, argList, isAutoDetails);
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
        /// Method to Get Sponsor Invoice Transactions
        /// </summary>
        /// <param name="argEn">Accounts Entity is the Input.BatchCode,Category,PostStatus and SubType are the Input Property</param>
        /// <returns>Returns List of Accounts</returns>
        public List<AccountsEn> GetSponsorInvoiceTransactions(AccountsEn argEn)
        {
            try
            {
                AccountsDAL loDs = new AccountsDAL();
                return loDs.GetSponsorInvoiceTransactions(argEn);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Method to Delete Sponsor Invoice
        /// </summary>
        /// <param name="argEn">Accounts Entity is the Input.BatchCode is an Input Property</param>
        /// <returns>Returns a Boolean</returns>
        public bool SponsorInvoiceDelete(AccountsEn argEn)
        {
            bool flag;
            using (TransactionScope ts = new TransactionScope())
            {
                try
                {
                    AccountsDAL loDs = new AccountsDAL();
                    flag = loDs.SponsorInvoiceDelete(argEn);
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
        /// Method to Get the List of Students SponsorInvoice by BatchId
        /// </summary>
        /// <param name="argEn">Student Entity is an Input Parameter.BatchCode is the Input Property</param>
        /// <returns>Returns List of Students</returns>
        public List<StudentEn> GetListStudentSponsorInvoicebyBatchID(StudentEn argEn)
        {
            try
            {
                AccountsDAL loDs = new AccountsDAL();
                return loDs.GetListStudentSponsorInvoicebyBatchID(argEn);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<StudentEn> GetListStudentSponsorInvoicebyBatchID1(StudentEn argEn)
        {
            try
            {
                AccountsDAL loDs = new AccountsDAL();
                return loDs.GetListStudentSponsorInvoicebyBatchID1(argEn);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion        

        /// <summary>
        /// Method to Get GetTotalAllocatedAmountWithTransID
        /// </summary>
        /// <param name="argEn">Student Entity is an Input</param>
        /// <returns>Returns the GetTotalAllocatedAmountWithTransID</returns>
        public double GetTotalAllocatedAmountWithTransID(AccountsEn argEn)
        {
            try
            {
                AccountsDAL loDs = new AccountsDAL();
                return loDs.GetTotalAllocatedAmountWithTransID(argEn);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Method to Get GetAvailableAmountBasedonTransID
        /// </summary>
        /// <param name="argEn">Student Entity is an Input</param>
        /// <returns>Returns the GetAvailableAmountBasedonTransID</returns>
        public double GetAvailableAmountBasedonTransID(AccountsEn argEn)
        {
            try
            {
                AccountsDAL loDs = new AccountsDAL();
                return loDs.GetAvailableAmountBasedonTransID(argEn);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        
        /// <summary>
        /// Method to Get Student's Outstanding Amount
        /// </summary>
        /// <param name="argEn">Student Entity is an Input</param>
        /// <returns>Returns the Outstanding Amount</returns>
        public double StudentOutstandingAmount(StudentEn argEn)
        {
            try
            {
                AccountsDAL loDs = new AccountsDAL();
                return loDs.StudentOutstandingAmount(argEn);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Method to Get Student's Outstanding Amount In Sponsor Allocation
        /// </summary>
        /// <param name="argEn">Student Entity is an Input</param>
        /// <returns>Returns the Outstanding Amount In Sponsor Allocation</returns>
        public double GetStudentOutstandingAmtInSponsorAllocation(StudentEn argEn)
        {
            try
            {
                AccountsDAL loDs = new AccountsDAL();
                return loDs.GetStudentOutstandingAmtInSponsorAllocation(argEn);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Method to Get Sponsor Allocations in Student Payment
        /// </summary>
        /// <param name="argEn">Accounts Entity is the Input.BatchCode,TransStatus,Category,PostStatus and SubType are Input Properties.</param>
        /// <returns>Returns List of Accounts</returns>
        public List<AccountsEn> GetSPAllocationTransactionsStudentPayment(AccountsEn argEn)
        {
            try
            {
                AccountsDAL loDs = new AccountsDAL();
                return loDs.GetSPAllocationTransactionsStudentPayment(argEn);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Method to Get Update Subcategory for allocation
        /// </summary>
        /// <param name="argEn">Accounts Entity is the Input.BatchCode,Category,PostStatus and SubType are the Input Property</param>
        /// <returns>Returns List of Subcategoryupdate</returns>
        public bool Subcategoryupdate(string BatchCode, string UserId)
        {
            bool flag;
            using (TransactionScope ts = new TransactionScope())
            {
                try
                {
                    AccountsDAL loDs = new AccountsDAL();
                    flag = loDs.Subcategoryupdate(BatchCode, UserId);
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
        /// Method to Get Transactions For Allocation
        /// </summary>
        /// <param name="argEn">Accounts Entity is the Input.BatchCode,Category,PostStatus and SubType are the Input Property</param>
        /// <returns>Returns List of Accounts</returns>
        public List<AccountsEn> GetTransactionsForAllocation(AccountsEn argEn)
        {
            try
            {
                AccountsDAL loDs = new AccountsDAL();
                return loDs.GetTransactionsForAllocation(argEn);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<AccountsEn> GetStudentLedgerCombine(AccountsEn argEn)
        {
            try
            {
                AccountsDAL loDs = new AccountsDAL();
                return loDs.GetStudentLedgerCombine(argEn);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Method to Get Student's Sponsor Amount In Sponsor Allocation
        /// </summary>
        /// <param name="argEn">Student Entity is an Input</param>
        /// <returns>Returns the Outstanding Amount In Sponsor Allocation</returns>
        public double GetStudentSponsorAmtInSponsorAllocation(StudentEn argEn)
        {
            try
            {
                AccountsDAL loDs = new AccountsDAL();
                return loDs.GetStudentSponsorAmtInSponsorAllocation(argEn);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }

}
