#region NameSpaces

using System;
using System.Data;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data.Common;
using HTS.SAS.Entities;
using MaxGeneric;
using System.Diagnostics;
using System.IO;
using System.Text.RegularExpressions;
using System.Linq;

#endregion

namespace HTS.SAS.DataAccessObjects
{
    /// <summary>
    /// Class to handle all the Accounting Transactions.
    /// </summary>
    public class AccountsDAL
    {
        #region Global Declarations

        List<AccountsDetailsEn> listFirstAccDetails;

        private DbParameterCollection _DbParameterCollection = null;

        private MaxModule.DatabaseProvider _DatabaseFactory =
            new MaxModule.DatabaseProvider();

        private string DataBaseConnectionString = Helper.
           GetConnectionString();

        #endregion

        public AccountsDAL()
        {
        }
		#region GetReciptSpPockAllow

        /// <summary>
        /// Method to Get Sponsor Receipts.
        /// </summary>
        /// <param name="argEn">Sponsor Entity is an Input. Sponsor Code and  Name are the Input properties</param>
        /// <returns>List of Sponsor Entity</returns>
        public List<SponsorEn> GetReciptSpPockAll(SponsorEn argEn)
        {
            string sqlCmd;
            List<SponsorEn> loEnList = new List<SponsorEn>();

            sqlCmd = " select distinct  adts.transid,sum(adts.transamount) SpoAmount,sum(adts.tempamount) PockAmount from sas_accounts ac inner join sas_accountsdetails adts " +
                     " on ac.transid=adts.transid "; //  adts.transamount,adts.tempamount";

            //modified by Hafiz @ 17/2/2016 - START
            if (!string.IsNullOrEmpty(argEn.BatchCode))
            {
                sqlCmd = sqlCmd + " WHERE ac.batchcode='" + argEn.BatchCode + "'";
            }
            else
            {
                sqlCmd = sqlCmd + " WHERE ac.creditref1='" + argEn.TransactionCode + "'";
            }
            //modified by Hafiz @ 17/2/2016 - END

            sqlCmd = sqlCmd + " GROUP BY adts.transid";

            try
            {
                if (!FormHelp.IsBlank(sqlCmd))
                {
                    using (IDataReader loReader = _DatabaseFactory.ExecuteReader(Helper.GetDataBaseType,
                        DataBaseConnectionString, sqlCmd).CreateDataReader())
                    {
                        while (loReader.Read())
                        {
                            SponsorEn loItem = new SponsorEn();

                            loItem.TransactionAmount = GetValue<double>(loReader, "SpoAmount");
                            loItem.TempAmount = GetValue<double>(loReader, "PockAmount");
                            loEnList.Add(loItem);
                        }
                        loReader.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return loEnList;
        }

        #endregion

        #region GetReciptSpAll

        /// <summary>
        /// Method to Get Sponsor Receipts.
        /// </summary>
        /// <param name="argEn">Sponsor Entity is an Input. Sponsor Code and  Name are the Input properties</param>
        /// <returns>List of Sponsor Entity</returns>
        /// modified by Hafiz @ 10/8/2016 - add UNION the Credit Note

        public List<SponsorEn> GetReciptSpAll(SponsorEn argEn)
        {
            string sqlCmd;
            List<SponsorEn> loEnList = new List<SponsorEn>();
            argEn.SponserCode = argEn.SponserCode.Replace("*", "%");
            argEn.Name = argEn.Name.Replace("*", "%");

            sqlCmd = "( SELECT SAS_Sponsor.SASR_Code,SAS_Accounts.TransDate,SAS_Sponsor.SASR_Name,SAS_Accounts.TransAmount as StAmount,SAS_SponsorInvoice.transamount as SpAmount " +
             ",SAS_Accounts.PaidAmount,SAS_Accounts.BatchCode as TransCode,SAS_SponsorInvoice.batchcode, " +
            "SAS_Accounts.TransTempCode ,SAS_Accounts.Category,SAS_Accounts.TransType,SAS_Accounts.PostStatus,SAS_Accounts.TransStatus,SAS_Accounts.allocateamount FROM SAS_Accounts " +
                "INNER JOIN SAS_Sponsor On SAS_Accounts.CreditRef = SAS_Sponsor.SASR_Code " +
" inner join sas_sponsor_inv_rec on sas_sponsor_inv_rec.receipt_id=SAS_Accounts.transid " +
" inner join SAS_SponsorInvoice on SAS_SponsorInvoice.batchcode =sas_sponsor_inv_rec.invoice_id WHERE (SAS_Accounts.TransType = 'Credit') AND SAS_Accounts.allocateamount<>0 AND" +


                //(SAS_Accounts.Category = 'Receipt')  and (SAS_Accounts.PostStatus = 'Posted') AND (SAS_Accounts.TransStatus='Closed')
                //string sqlCmd = "SELECT SAS_Sponsor.SASR_Code,SAS_Accounts.TransDate,SAS_Sponsor.SASR_Name,SAS_Accounts.TransAmount,SAS_Accounts.PaidAmount,SAS_Accounts.BatchCode as TransCode," +
                //                 " SAS_Accounts.TransTempCode ,SAS_Accounts.Category FROM SAS_Accounts INNER JOIN SAS_Sponsor On SAS_Accounts.CreditRef = SAS_Sponsor.SASR_Code WHERE " +
                           " (SAS_Accounts.Category = '" + argEn.Category + "') " +
                           " and (SAS_Accounts.PostStatus = '" + argEn.PostStatus + "')";



            if (argEn.SponserCode.Length != 0) sqlCmd = sqlCmd + " AND SAS_Sponsor.SASR_Code LIKE '" + argEn.SponserCode + "'";
            if (argEn.Name.Length != 0) sqlCmd = sqlCmd + " AND SASR_Name LIKE '" + argEn.Name + "'";
            //commented by Hafiz @ 22/2/2016
            //no use for transstatus
            //if (argEn.TransStatus.Length != 0) sqlCmd = sqlCmd + "AND (SAS_Accounts.TransStatus='" + argEn.TransStatus + "')";

            sqlCmd = sqlCmd + " group by SAS_Sponsor.SASR_Code,SAS_Accounts.TransDate,SAS_Sponsor.SASR_Name,StAmount,SpAmount,SAS_Accounts.PaidAmount,SAS_Accounts.BatchCode," +
            "SAS_SponsorInvoice.batchcode,SAS_Accounts.TransTempCode ,SAS_Accounts.Category,SAS_Accounts.TransType,SAS_Accounts.PostStatus,SAS_Accounts.TransStatus,SAS_Accounts.allocateamount" +
            " Order by SAS_SponsorInvoice.batchcode desc )";

            sqlCmd = sqlCmd + " UNION ";

            sqlCmd = sqlCmd + "( SELECT SAS_Sponsor.SASR_Code, SAS_Accounts.TransDate, SAS_Sponsor.SASR_Name, SAS_Accounts.TransAmount as StAmount, " +
            "0 as SpAmount, SAS_Accounts.PaidAmount, SAS_Accounts.BatchCode as TransCode, '' as batchcode, SAS_Accounts.TransTempCode, SAS_Accounts.Category, " +
            "SAS_Accounts.TransType, SAS_Accounts.PostStatus, SAS_Accounts.TransStatus, SAS_Accounts.tempamount " +
            "FROM SAS_Accounts " +
            "INNER JOIN SAS_Sponsor On SAS_Accounts.CreditRef = SAS_Sponsor.SASR_Code " +
            "WHERE (SAS_Accounts.TransType = 'Credit') " +
            "AND (SAS_Accounts.Category = 'Credit Note') " +
            "AND (SAS_Accounts.Subtype = 'Sponsor') " +
            "AND (SAS_Accounts.PostStatus = 'Posted') " +
            "AND (SAS_Accounts.CreditRef1 = '') and SAS_Accounts.tempamount<>0 ) ";

            try
            {
                if (!FormHelp.IsBlank(sqlCmd))
                {
                    using (IDataReader loReader = _DatabaseFactory.ExecuteReader(Helper.GetDataBaseType,
                        DataBaseConnectionString, sqlCmd).CreateDataReader())
                    {
                        while (loReader.Read())
                        {
                            SponsorEn loItem = new SponsorEn();
                            loItem.SponserCode = GetValue<string>(loReader, "SASR_Code");
                            loItem.Name = GetValue<string>(loReader, "SASR_Name");
                            loItem.TransactionAmount = GetValue<double>(loReader, "StAmount");
                            loItem.PaidAmount = GetValue<double>(loReader, "SpAmount");
                            loItem.TransactionCode = GetValue<string>(loReader, "TransCode");
                            loItem.TempTransCode = GetValue<string>(loReader, "batchcode");
                            loItem.TransDate = GetValue<DateTime>(loReader, "TransDate");
                            loItem.Category = GetValue<string>(loReader, "Category");
                            loItem.AllocatedAmount = GetValue<double>(loReader, "allocateamount");
                            loEnList.Add(loItem);
                        }
                        loReader.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return loEnList;
        }

        #endregion

        #region GetReceiptSpAll

        /// <summary>
        /// Method to Get Sponsor Receipts.
        /// </summary>
        /// <param name="argEn">Sponsor Entity is an Input. Sponsor Code and  Name are the Input properties</param>
        /// <returns>List of Sponsor Entity</returns>
        public List<SponsorEn> GetReceiptSpAll(SponsorEn argEn)
        {
            string sqlCmd;
            List<SponsorEn> loEnList = new List<SponsorEn>();
            argEn.SponserCode = argEn.SponserCode.Replace("*", "%");
            argEn.Name = argEn.Name.Replace("*", "%");

            sqlCmd = "SELECT SAS_Sponsor.SASR_Code,SAS_Accounts.TransDate,SAS_Sponsor.SASR_Name,SAS_Accounts.TransAmount as StAmount,SAS_SponsorInvoice.transamount as SpAmount " +
             ",SAS_Accounts.PaidAmount,SAS_Accounts.BatchCode as TransCode,SAS_SponsorInvoice.batchcode, " +
            "SAS_Accounts.TransTempCode ,SAS_Accounts.Category,SAS_Accounts.TransType,SAS_Accounts.PostStatus,SAS_Accounts.TransStatus,SAS_Accounts.allocateamount, " +
            "CASE WHEN SAS_Accounts.TransAmount >= SAS_Accounts.allocateamount THEN " +
                    "SAS_Accounts.TransAmount - SAS_Accounts.allocateamount " +
                  "ELSE 0 " +
            "END AS AmountBalance " +
            "FROM SAS_Accounts " +
                "INNER JOIN SAS_Sponsor On SAS_Accounts.CreditRef = SAS_Sponsor.SASR_Code " +
" inner join sas_sponsor_inv_rec on sas_sponsor_inv_rec.receipt_id=SAS_Accounts.transid " +
" inner join SAS_SponsorInvoice on SAS_SponsorInvoice.batchcode =sas_sponsor_inv_rec.invoice_id WHERE (SAS_Accounts.TransType = 'Credit') AND SAS_Accounts.allocateamount<>0 AND (SAS_Accounts.TransAmount >= SAS_Accounts.allocateamount) AND" +
                           " (SAS_Accounts.Category = '" + argEn.Category + "') " +
                           " and (SAS_Accounts.PostStatus = '" + argEn.PostStatus + "')";


            if (argEn.SponserCode.Length != 0) sqlCmd = sqlCmd + " AND SAS_Sponsor.SASR_Code LIKE '" + argEn.SponserCode + "'";
            if (argEn.Name.Length != 0) sqlCmd = sqlCmd + " AND SASR_Name LIKE '" + argEn.Name + "'";
            //commented by Hafiz @ 22/2/2016
            //no use for transstatus
            //if (argEn.TransStatus.Length != 0) sqlCmd = sqlCmd + "AND (SAS_Accounts.TransStatus='" + argEn.TransStatus + "')";

            sqlCmd = sqlCmd + " group by SAS_Sponsor.SASR_Code,SAS_Accounts.TransDate,SAS_Sponsor.SASR_Name,StAmount,SpAmount,SAS_Accounts.PaidAmount,SAS_Accounts.BatchCode," +
            "SAS_SponsorInvoice.batchcode,SAS_Accounts.TransTempCode ,SAS_Accounts.Category,SAS_Accounts.TransType,SAS_Accounts.PostStatus,SAS_Accounts.TransStatus,SAS_Accounts.allocateamount" +
            " Order by SAS_SponsorInvoice.batchcode desc";


            try
            {
                if (!FormHelp.IsBlank(sqlCmd))
                {
                    using (IDataReader loReader = _DatabaseFactory.ExecuteReader(Helper.GetDataBaseType,
                        DataBaseConnectionString, sqlCmd).CreateDataReader())
                    {
                        while (loReader.Read())
                        {
                            SponsorEn loItem = new SponsorEn();
                            loItem.SponserCode = GetValue<string>(loReader, "SASR_Code");
                            loItem.Name = GetValue<string>(loReader, "SASR_Name");
                            loItem.TransactionAmount = GetValue<double>(loReader, "StAmount");
                            loItem.PaidAmount = GetValue<double>(loReader, "SpAmount");
                            loItem.TransactionCode = GetValue<string>(loReader, "TransCode");
                            loItem.TempTransCode = GetValue<string>(loReader, "batchcode");
                            loItem.TransDate = GetValue<DateTime>(loReader, "TransDate");
                            loItem.Category = GetValue<string>(loReader, "Category");
                            loItem.TempAmount = GetValue<double>(loReader, "allocateamount");
                            //used tempamount to stored AmountBalance - Start
                            loItem.AllocatedAmount = GetValue<double>(loReader, "AmountBalance");
                            //used tempamount to stored AmountBalance - End
                            loEnList.Add(loItem);
                        }
                        loReader.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return loEnList;
        }

        #endregion

        #region GetRecipt

        /// <summary>
        /// Method to Get Sponsor Receipts.
        /// </summary>
        /// <param name="argEn">Sponsor Entity is an Input. Sponsor Code and  Name are the Input properties</param>
        /// <returns>List of Sponsor Entity</returns>
        public List<SponsorEn> GetRecipt(SponsorEn argEn)
        {
            List<SponsorEn> loEnList = new List<SponsorEn>();
            argEn.SponserCode = argEn.SponserCode.Replace("*", "%");
            argEn.Name = argEn.Name.Replace("*", "%");
            string sqlCmd = "SELECT SAS_Sponsor.SASR_Code,SAS_Accounts.TransDate,SAS_Sponsor.SASR_Name,SAS_Accounts.TransAmount,SAS_Accounts.PaidAmount,SAS_Accounts.BatchCode as TransCode," +
                             " SAS_Accounts.TransTempCode ,SAS_Accounts.Category FROM SAS_Accounts INNER JOIN SAS_Sponsor On SAS_Accounts.CreditRef = SAS_Sponsor.SASR_Code WHERE " +
                             "(SAS_Accounts.TransType = 'Credit') AND (SAS_Accounts.Category = '" + argEn.Category + "') " +
                             " and (SAS_Accounts.PostStatus = '" + argEn.PostStatus + "')";
            if (argEn.SponserCode.Length != 0) sqlCmd = sqlCmd + " AND SAS_Sponsor.SASR_Code LIKE '" + argEn.SponserCode + "'";
            if (argEn.Name.Length != 0) sqlCmd = sqlCmd + " AND SASR_Name LIKE '" + argEn.Name + "'";
            if (argEn.TransStatus.Length != 0) sqlCmd = sqlCmd + "AND (SAS_Accounts.TransStatus='" + argEn.TransStatus + "')";

            try
            {
                if (!FormHelp.IsBlank(sqlCmd))
                {
                    using (IDataReader loReader = _DatabaseFactory.ExecuteReader(Helper.GetDataBaseType,
                        DataBaseConnectionString, sqlCmd).CreateDataReader())
                    {
                        while (loReader.Read())
                        {
                            SponsorEn loItem = new SponsorEn();
                            loItem.SponserCode = GetValue<string>(loReader, "SASR_Code");
                            loItem.Name = GetValue<string>(loReader, "SASR_Name");
                            loItem.TransactionAmount = GetValue<double>(loReader, "TransAmount");
                            loItem.PaidAmount = GetValue<double>(loReader, "PaidAmount");
                            loItem.TransactionCode = GetValue<string>(loReader, "TransCode");
                            loItem.TempTransCode = GetValue<string>(loReader, "TransTempCode");
                            loItem.TransDate = GetValue<DateTime>(loReader, "TransDate");
                            loItem.Category = GetValue<string>(loReader, "Category");
                            loEnList.Add(loItem);
                        }
                        loReader.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return loEnList;
        }

        #endregion

        #region GetListByBatch

        /// <summary>
        /// Method to Get List of MatricNo
        /// </summary>
        /// <param name="argEn">Students Entity as an Input.</param>
        /// <returns>Returns List of MatricNo</returns>
        public List<AccountsEn> GetListByBatch(string BatchCode, string Posted)
        {
            List<AccountsEn> loEnList = new List<AccountsEn>();
            BatchCode = BatchCode.Replace("*", "%");
            string sqlCmd = "select * from SAS_Accounts  WHERE BatchCode = '" + BatchCode + "'";
            sqlCmd = sqlCmd + " order by CreditRef";

            try
            {
                if (!FormHelp.IsBlank(sqlCmd))
                {
                    using (IDataReader loReader = _DatabaseFactory.ExecuteReader(Helper.GetDataBaseType,
                        DataBaseConnectionString, sqlCmd).CreateDataReader())
                    {
                        while (loReader.Read())
                        {
                            AccountsEn loItem = LoadObjectMatricNo(loReader);
                            StudentEn loItemStudEn = new StudentEn();
                            StudentDAL loStudDal = new StudentDAL();

                            loItemStudEn.MatricNo = loItem.CreditRef;
                            if (Posted == "Posted")
                            {
                                loItemStudEn.PostStatus = "2";
                            }
                            if (Posted == "Ready")
                            {
                                loItemStudEn.PostStatus = "1";
                            }
                            loEnList.Add(loItem);
                        }
                        loReader.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return loEnList;
        }

        #endregion

        #region GetSponserListByBatchID

        /// <summary>
        /// Method to Get the List of Sponsors by BatchId
        /// </summary>
        /// <param name="BatchID">BatchID is an Input Parameter.</param>
        /// <returns>Returns List of Sponsors</returns>
        public List<SponsorEn> GetSponserListByBatchID(string BatchID)
        {
            List<SponsorEn> loEnList = new List<SponsorEn>();
            string sqlCmd = " SELECT SAS_Sponsor.SASR_Code,SAS_Sponsor.SASR_Name,SAS_Sponsor.SASSR_SName,SAS_Sponsor.SASR_Address," +
                            " SAS_Sponsor.SASR_Address1,SAS_Sponsor.SASR_Address2,SAS_Sponsor.SASR_Contact,SAS_Sponsor.SASR_Phone," +
                            " SAS_Sponsor.SASR_Fax,SAS_Sponsor.SASR_Email,SAS_Sponsor.SASR_WebSite,SAS_Sponsor.SASR_Type,SAS_Sponsor.SASR_Desc," +
                            " SAS_Sponsor.SASR_GLAccount,SAS_Sponsor.SABR_Code,SAS_Sponsor.SASR_UpdatedBy,SAS_Sponsor.SASR_UpdatedDtTm," +
                            " SAS_Sponsor.SASR_Status FROM SAS_Sponsor INNER JOIN SAS_Accounts ON SAS_Sponsor.SASR_Code = SAS_Accounts.CreditRef" +
                            " WHERE SAS_Accounts.BatchCode='" + BatchID + "'";

            try
            {
                if (!FormHelp.IsBlank(sqlCmd))
                {
                    using (IDataReader loReader = _DatabaseFactory.ExecuteReader(Helper.GetDataBaseType,
                        DataBaseConnectionString, sqlCmd).CreateDataReader())
                    {
                        while (loReader.Read())
                        {
                            SponsorEn loItem = LoadSponsor(loReader);
                            loEnList.Add(loItem);
                        }
                        loReader.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return loEnList;

        }

        #endregion

        #region GetStudentLedgerList

        /// <summary>
        /// Method to Get StudentLedger list
        /// </summary>
        /// <param name="argEn">Accounts Entity is an Input.Creditref , Subtype and Poststatus are Input properties</param>
        /// <returns>Returns List of Accounts</returns>
        public List<AccountsEn> GetStudentLedgerList(AccountsEn argEn)
        {
            List<AccountsEn> loEnList = new List<AccountsEn>();
            argEn.TransType = argEn.TransType.Replace("*", "%");
            argEn.TransStatus = argEn.TransStatus.Replace("*", "%");
            //string sqlCmd = " SELECT batchcode,transcode,creditref,description,transamount,transdate,subtype,poststatus,transtype,category FROM SAS_Accounts" +
            //                " WHERE CreditRef='" + argEn.CreditRef + "' and SubType ='" + argEn.SubType + "'and PostStatus ='" + argEn.PostStatus + "'";
            //sqlCmd += " UNION select batchcode,transcode,creditref,description,transamount,transdate,subtype,poststatus,transtype,category from sas_sponsorinvoice" +
            //    " where creditref1 = '" + argEn.CreditRef + "' and SubType ='" + argEn.SubType + "'and PostStatus ='" + argEn.PostStatus + "'";

            string sqlCmd = " SELECT batchcode,transcode,creditref,description,transamount,transdate,subtype,poststatus,transtype,category FROM SAS_Accounts" +
                            " WHERE CreditRef='" + argEn.CreditRef + "' and SubType ='" + argEn.SubType + "'and PostStatus ='" + argEn.PostStatus + "'";

            //commented by farid 30062016 for sponsorinvoce record in sponsorledger
            //sqlCmd += " UNION SELECT batchcode,transcode,creditref1,description,transamount,transdate,subtype,poststatus,transtype,category" +
            //" FROM (SELECT batchcode,transcode,creditref1,description,transamount,transdate,subtype,poststatus,transtype,category, ROW_NUMBER() OVER (PARTITION BY batchcode ORDER BY batchcode) rn FROM sas_sponsorinvoice) sas_sponsorinvoice" +
            //" WHERE creditref1 = '" + argEn.CreditRef + "' and SubType ='" + argEn.SubType + "'and PostStatus ='" + argEn.PostStatus + "' and rn = 1";
            //end commented

            if (argEn.TransType.Length != 0) sqlCmd = sqlCmd + " AND TransType LIKE '" + argEn.TransType + "'";
            if (argEn.TransStatus.Length != 0) sqlCmd = sqlCmd + " AND Transstatus LIKE '" + argEn.TransStatus + "'";
            sqlCmd += " order by TransDate";

            //try
            //{
            //    if (!FormHelp.IsBlank(sqlCmd))
            //    {
            //        using (IDataReader loReader = _DatabaseFactory.ExecuteReader(Helper.GetDataBaseType,
            //            DataBaseConnectionString, sqlCmd).CreateDataReader())
            //        {
            //            while (loReader.Read())
            //            {
            //                AccountsEn loItem = LoadObject(loReader);
            //                loEnList.Add(loItem);
            //            }
            //            loReader.Close();
            //        }
            //    }
            //}
            try
            {
                if (!FormHelp.IsBlank(sqlCmd))
                {
                    using (IDataReader loReader = _DatabaseFactory.ExecuteReader(Helper.GetDataBaseType,
                        DataBaseConnectionString, sqlCmd).CreateDataReader())
                    {
                        while (loReader.Read())
                        {
                            AccountsEn loItem = new AccountsEn();
                            loItem.BatchCode = GetValue<string>(loReader, "batchcode");
                            loItem.TransactionCode = GetValue<string>(loReader, "transcode");
                            loItem.CreditRef = GetValue<string>(loReader, "creditref");
                            loItem.Description = GetValue<string>(loReader, "description");
                            loItem.TransactionAmount = GetValue<double>(loReader, "transamount");
                            loItem.TransDate = GetValue<DateTime>(loReader, "transdate");
                            loItem.SubType = GetValue<string>(loReader, "subtype");
                            loItem.PostStatus = GetValue<string>(loReader, "poststatus");
                            loItem.TransType = GetValue<string>(loReader, "transtype");
                            loItem.Category = GetValue<string>(loReader, "category");
                            loEnList.Add(loItem);
                        }
                        loReader.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return loEnList;
        }

        #endregion

        #region GetStudentLedgerDetailList

        /// <summary>
        /// Method to Get StudentLedger Details list
        /// </summary>
        /// <param name="argEn">Accounts Entity is an Input.Creditref , Subtype and Poststatus are Input properties</param>
        /// <returns>Returns List of Accounts</returns>

        //Modified by Hafiz Roslan
        //On 06/01/2016
        //Add sql CASE WHEN Category = 'Receipt'
        //Updated on 4/2/2016 - add Category = 'Loan'
        //modified on 24/3/2016 - remove SUM(acc.TransAmount) for individual records
        public List<AccountsEn> GetStudentLedgerDetailList(AccountsEn argEn)
        {
            List<AccountsEn> loEnList = new List<AccountsEn>();
            argEn.TransType = argEn.TransType.Replace("*", "%");
            //argEn.TransStatus = argEn.TransStatus.Replace("*", "%");
            ///ORIGINAL QUERY
            //            string sqlCmd = @" SELECT TransDate ,
            //                                    CASE WHEN Category = 'Receipt' THEN
            //                                      CASE
            //                                                  WHEN Description != 'CIMB CLICKS' THEN BankRecNo
            //                                                  ELSE BatchCode
            //                                      END
            //                                    ELSE TransCode
            //                                    END TransCode ,
            //                                    Description ,
            //                                    Category ,
            //                                    CASE WHEN TransType = 'Credit' THEN TransAmount
            //                                         ELSE 0
            //                                    END Credit,
            //                                    CASE WHEN TransType = 'Debit' THEN TransAmount
            //                                         ELSE 0
            //                                    END Debit ,
            //                                    TransAmount ,
            //                                    TransType,
            //                                    BatchCode
            //                             FROM   SAS_Accounts
            //                             WHERE  CreditRef = '" + argEn.CreditRef + "' AND SubType = '" + argEn.SubType + "'and PostStatus ='" + argEn.PostStatus + "'";
            ///EDITED BY JESSICA
            string sqlCmd = @"SELECT acc.TransDate,
                              CASE WHEN acc.Category = 'Receipt' THEN
                                  CASE WHEN acc.Description != 'CIMB CLICKS' THEN acc.BankRecNo
	                                  ELSE acc.TransCode
                                  END
                              ELSE
                                  CASE WHEN Category = 'Loan' THEN BatchCode
	                                  ELSE acc.TransCode
                                  END
                              END TransCode,
                        acc.Description ,
                        acc.Category ,
                        case when acc.category = 'Credit Note' or  acc.category = 'Debit Note' or acc.category = 'Invoice' then 
                            case 
                                when acc.transtype = 'Credit' Then SUM(de.transamount)
                                else 0
                            end
                        else     
                            CASE WHEN acc.TransType = 'Credit' THEN acc.TransAmount
                            ELSE 0
                            end
                            END Credit,
                        case when acc.category = 'Credit Note' or  acc.category = 'Debit Note' or acc.category = 'Invoice'  then
                            case
                                when acc.transtype = 'Debit' then SUM(de.TransAmount)
                            Else 0
                            End
                        else 
                            CASE WHEN acc.TransType = 'Debit' THEN acc.TransAmount
                            ELSE 0
                            end 
                            END Debit ,
                        Case when acc.category = 'Credit Note' or  acc.category = 'Debit Note'  or acc.category = 'Invoice'   then 
                            SUM(de.TransAmount)
                        Else
                            acc.TransAmount
                        END TransAmount,    
                        acc.TransType,
                        acc.BatchCode
                        FROM SAS_Accounts acc
                        left join sas_accountsdetails de on acc.transid = de.transid
                        WHERE  acc.CreditRef = '" + argEn.CreditRef + "' AND acc.SubType = '" + argEn.SubType + "' AND acc.PostStatus ='" + argEn.PostStatus + "' ";

            if (argEn.TransType.Length != 0) sqlCmd = sqlCmd + " AND acc.TransType LIKE '" + argEn.TransType + "' ";

            //if (argEn.TransStatus.Length != 0) sqlCmd = sqlCmd + " AND acc.Transstatus = '" + argEn.TransStatus + "' ";
            //updated by Hafiz Roslan @ 10/2/2016
            //add subcategory to identify receipt from student loan
            sqlCmd += "AND acc.subcategory NOT IN ('Loan') AND acc.TransAmount > 0 ";
            sqlCmd += "GROUP BY acc.TransDate, acc.BankRecNo, acc.Description, acc.Category, acc.TransCode, acc.TransType, acc.BatchCode, acc.TransAmount,acc.postedtimestamp ";
            sqlCmd += "ORDER BY acc.postedtimestamp";

            try
            {
                if (!FormHelp.IsBlank(sqlCmd))
                {
                    using (IDataReader loReader = _DatabaseFactory.ExecuteReader(Helper.GetDataBaseType,
                                   DataBaseConnectionString, sqlCmd).CreateDataReader())
                    {
                        while (loReader.Read())
                        {
                            AccountsEn loItem = LoadStudentLedgerObject(loReader);
                            loEnList.Add(loItem);
                        }
                        loReader.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return loEnList;
        }

        #endregion

        #region GetStudentLoanLedgerDetailList

        public List<AccountsEn> GetStudentLoanLedgerDetailList(AccountsEn argEn)
        {
            List<AccountsEn> loEnList = new List<AccountsEn>();
            argEn.TransType = argEn.TransType.Replace("*", "%");
            argEn.TransStatus = argEn.TransStatus.Replace("*", "%");
            //            string sqlCmd = @"SELECT *
            //                             FROM   (SELECT    TransDate ,
            //                                                TransCode ,
            //                                                Description ,
            //                                                Category ,
            //                                                CASE WHEN TransType = 'Credit' THEN TransAmount
            //                                                     ELSE 0
            //                                                END Debit ,
            //                                                CASE WHEN TransType = 'Debit' THEN TransAmount
            //                                                     ELSE 0
            //                                                END Credit ,
            //                                                TransAmount ,
            //                                                TransType ,
            //                                                BatchCode
            //                                      FROM      SAS_StudentLoan
            //                                      WHERE     CreditRef = '" + argEn.CreditRef + "' AND SubType = '" + argEn.SubType + "'and PostStatus ='" + argEn.PostStatus + "') A ";
            string sqlCmd = @"SELECT *
                             FROM   (SELECT    TransDate ,
                                                TransCode ,
                                                Description ,
                                                Category ,
                                                --debit
		                                        CASE WHEN Category = 'Receipt' THEN 
		                                             CASE WHEN TransType = 'Debit' THEN TransAmount
		                                             ELSE 0
		                                             END
		                                        ELSE
		                                             CASE WHEN TransType = 'Credit' THEN TransAmount
	                                                     ELSE 0
	                                                     END
		                                        END Debit ,
		                                        --credit
		                                        CASE WHEN Category = 'Receipt' THEN 
		                                             CASE WHEN TransType = 'Credit' THEN TransAmount
		                                             ELSE 0
		                                             END
		                                        ELSE
		                                             CASE WHEN TransType = 'Debit' THEN TransAmount
		                                             ELSE 0
		                                             END
		                                        END Credit ,
                                                TransAmount ,
                                                TransType ,
                                                BatchCode
                                      FROM      SAS_StudentLoan
                                      WHERE     CreditRef = '" + argEn.CreditRef + "' AND SubType = '" + argEn.SubType + "'and PostStatus ='" + argEn.PostStatus + 
                                      "' ORDER BY SAS_StudentLoan.postedtimestamp) A ";
            //WHERE     CreditRef = '" + argEn.CreditRef + "' AND SubType = '" + argEn.SubType + "'and PostStatus ='" + argEn.PostStatus + "' and Transstatus='" + argEn.TransStatus + "') A ";
            //sqlCmd += " order by A.postedtimestamp";

            try
            {
                if (!FormHelp.IsBlank(sqlCmd))
                {
                    using (IDataReader loReader = _DatabaseFactory.ExecuteReader(Helper.GetDataBaseType,
                        DataBaseConnectionString, sqlCmd).CreateDataReader())
                    {
                        while (loReader.Read())
                        {
                            AccountsEn loItem = LoadStudentLedgerObject(loReader);
                            loEnList.Add(loItem);
                        }
                        loReader.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return loEnList;
        }

        #endregion

        #region GetStudentAutoAllocation

        /// <summary>
        /// Method to Get StudentAutoAllocation list
        /// </summary>
        /// <param name="argEn">Accounts Entity is an Input.Creditref , Subtype and Poststatus are Input properties</param>
        /// <returns>Returns List of Accounts</returns>
        /// modified by Hafiz @ 24/3/2016
        public List<AccountsEn> GetStudentAutoAllocation(AccountsEn argEn)
        {
            double the_transamount = 0;
            List<AccountsEn> loEnList = new List<AccountsEn>();


            argEn.Category = argEn.Category.Replace("*", "%");


            string sqlCmd = " SELECT " +
                            " case when category IN ('Invoice','Debit Note') then" +
                            " (select SUM(transamount) from sas_accountsdetails" +
                            " where transcode = SAS_Accounts.transcode)" +
                            " else" +
                            " transamount" +
                            " END AS the_transamount," +
                            " * FROM SAS_Accounts" +
                            " WHERE CreditRef='" + argEn.CreditRef + "' and SubType ='" + argEn.SubType + "' and PostStatus ='" + argEn.PostStatus + "' and TransStatus = 'Open'";
            if (argEn.Category.Length != 0) sqlCmd = sqlCmd + " AND category IN (" + argEn.Category + ")";
            sqlCmd += "order by TransDate";

            try
            {
                if (!FormHelp.IsBlank(sqlCmd))
                {
                    using (IDataReader loReader = _DatabaseFactory.ExecuteReader(Helper.GetDataBaseType,
                        DataBaseConnectionString, sqlCmd).CreateDataReader())
                    {
                        while (loReader.Read())
                        {
                            AccountsEn loItem = LoadObject(loReader);

                            the_transamount = GetValue<double>(loReader, "the_transamount");

                            if (!double.IsNaN(the_transamount))
                            {
                                loItem.TransactionAmount = the_transamount;
                            }

                            loEnList.Add(loItem);
                        }
                        loReader.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return loEnList;
        }

        #endregion

        #region GetExportData

        /// <summary>
        /// Method to Get ExportData list
        /// </summary>
        /// <param name="argEn">Accounts Entity is an Input.Category , Subtype,FromDate and toDate are Input properties</param>
        /// <returns>Returns List of Accounts</returns>
        public List<AccountsEn> GetExportData(AccountsEn argEn)
        {
            List<AccountsEn> loEnList = new List<AccountsEn>();

            string sqlCmd = " SELECT * FROM SAS_Accounts WHERE " +

                            " PostStatus ='POSTED' ";
            if (argEn.Category.Length != 0 || argEn.Category != "All") sqlCmd += " AND Category = '" + argEn.Category + "' ";
            if (argEn.SubType.Length != 0 || argEn.SubType != "--All--") sqlCmd += " And SubType = '" + argEn.SubType + "' ";
            if (argEn.IntegrationCode == "True")
            {
                sqlCmd += " and TransDate between  '" + argEn.TransDate + "'and '" + argEn.BatchDate + "'";
            }
            if (argEn.IntegrationStatus == 0)
            {
                sqlCmd += "and IntStatus = " + argEn.IntegrationStatus + "";
            }

            sqlCmd += "order by TransDate,batchcode";

            try
            {
                if (!FormHelp.IsBlank(sqlCmd))
                {
                    using (IDataReader loReader = _DatabaseFactory.ExecuteReader(Helper.GetDataBaseType,
                        DataBaseConnectionString, sqlCmd).CreateDataReader())
                    {
                        while (loReader.Read())
                        {
                            AccountsDetailsDAL lodeatilsds = new AccountsDetailsDAL();
                            AccountsDetailsEn lodetailItem = new AccountsDetailsEn();
                            AccountsEn loItem = LoadObject(loReader);
                            lodetailItem.TransactionID = loItem.TranssactionID;
                            loItem.AccountDetailsList = lodeatilsds.GetAccountDetailList(lodetailItem);
                            loEnList.Add(loItem);
                        }
                        loReader.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            if (loEnList != null)
            {
                if (loEnList.Count != 0)
                {
                    AccountsEn loaacen;
                    System.Collections.Hashtable ht = new System.Collections.Hashtable();
                    List<AccountsEn> lohtlist = new List<AccountsEn>();
                    foreach (AccountsEn loen in loEnList)
                    {
                        if (!ht.Contains(loen.BatchCode))
                        {
                            ht.Add(loen.BatchCode, loen);
                            lohtlist.Add(loen);
                        }
                    }


                    for (int i = 0; i < lohtlist.Count; i++)
                    {
                        loaacen = new AccountsEn();
                        loaacen.BatchCode = lohtlist[i].BatchCode;
                        loaacen.UpdatedBy = argEn.UpdatedBy;
                        loaacen.UpdatedTime = argEn.UpdatedTime;
                        UpdateIntegrationStatus(loaacen);
                    }
                }
            }
            return loEnList;
        }

        #endregion

        #region GetStGLJournalExportData

        /// <summary>
        /// Method to Get GL Journal ExportData list
        /// </summary>
        /// <param name="argEn">Accounts Entity is an Input.Category , Subtype,FromDate and toDate are Input properties</param>
        /// <returns>Returns List of Accounts</returns>
        public List<AccountsEn> GetStGLJournalExportData(AccountsEn argEn)
        {
            List<AccountsEn> loEnList = new List<AccountsEn>();

            string sqlCmd = "SELECT  SAS_Program.SAPG_TI as Expr1,SAS_Program.SAPG_AD as Expr2,SAS_Accounts.* FROM SAS_Accounts INNER JOIN " +
                            " SAS_Student ON SAS_Accounts.CreditRef = SAS_Student.SASI_MatricNo INNER JOIN SAS_Program ON " +
                            " SAS_Student.SASI_PgId = SAS_Program.SAPG_Code" +
                            " WHERE (Category='Invoice' or category ='AFC'or category ='Credit Note'or category ='Debit Note')and SubType ='" + argEn.SubType + "'and  PostStatus ='POSTED' ";

            if (argEn.IntegrationCode == "True")
            {
                sqlCmd += " and TransDate between  '" + argEn.TransDate + "'and '" + argEn.BatchDate + "'";
            }
            if (argEn.IntegrationStatus == 0)
            {
                sqlCmd += " and IntStatus = " + argEn.IntegrationStatus + "";
            }
            sqlCmd += "order by TransDate,batchcode";

            try
            {
                if (!FormHelp.IsBlank(sqlCmd))
                {
                    using (IDataReader loReader = _DatabaseFactory.ExecuteReader(Helper.GetDataBaseType,
                        DataBaseConnectionString, sqlCmd).CreateDataReader())
                    {
                        while (loReader.Read())
                        {
                            AccountsDetailsDAL lodeatilsds = new AccountsDetailsDAL();
                            AccountsDetailsEn lodetailItem = new AccountsDetailsEn();
                            AccountsEn loItem = LoadObject(loReader);
                            loItem.ProgramInfo = new ProgramInfoEn();
                            loItem.ProgramInfo.Tutionacc = GetValue<string>(loReader, "Expr1");
                            loItem.ProgramInfo.Accountinfo = GetValue<string>(loReader, "Expr2");
                            loEnList.Add(loItem);
                        }
                        loReader.Close();
                    }
                }
                if (loEnList.Count != 0)
                {
                    AccountsEn loaacen;
                    System.Collections.Hashtable ht = new System.Collections.Hashtable();
                    List<AccountsEn> lohtlist = new List<AccountsEn>();
                    foreach (AccountsEn loen in loEnList)
                    {
                        if (!ht.Contains(loen.BatchCode))
                        {
                            ht.Add(loen.BatchCode, loen);
                            lohtlist.Add(loen);
                        }
                    }


                    for (int i = 0; i < lohtlist.Count; i++)
                    {
                        loaacen = new AccountsEn();
                        loaacen.BatchCode = lohtlist[i].BatchCode;
                        loaacen.UpdatedBy = argEn.UpdatedBy;
                        loaacen.UpdatedTime = argEn.UpdatedTime;
                        UpdateIntegrationStatus(loaacen);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return loEnList;
        }

        #endregion

        #region GetSponsorGLJournalExportData

        /// <summary>
        /// Method to Get Sponsor GL Journal ExportData list
        /// </summary>
        /// <param name="argEn">Accounts Entity is an Input.Category , Subtype,FromDate and toDate are Input properties</param>
        /// <returns>Returns List of Accounts</returns>
        public List<AccountsEn> GetSponsorGLJournalExportData(AccountsEn argEn)
        {
            List<AccountsEn> loEnList = new List<AccountsEn>();

            string sqlCmd = " SELECT distinct batchcode,category FROM SAS_Accounts" +
                            " WHERE (category ='Credit Note'or category ='Debit Note' or category ='Allocation') and SubType ='" + argEn.SubType + "'and  PostStatus ='POSTED'";

            if (argEn.IntegrationCode == "True")
            {
                sqlCmd += " and TransDate between  '" + argEn.TransDate + "'and '" + argEn.BatchDate + "'";
            }
            if (argEn.IntegrationStatus == 0)
            {
                sqlCmd += "and IntStatus = " + argEn.IntegrationStatus + "";
            }
            else { }


            try
            {
                if (!FormHelp.IsBlank(sqlCmd))
                {
                    using (IDataReader loReader = _DatabaseFactory.ExecuteReader(Helper.GetDataBaseType,
                        DataBaseConnectionString, sqlCmd).CreateDataReader())
                    {
                        while (loReader.Read())
                        {
                            AccountsDAL losds = new AccountsDAL();
                            AccountsEn lodItem = new AccountsEn();
                            lodItem.BatchCode = GetValue<string>(loReader, "BatchCode");
                            lodItem.Category = GetValue<string>(loReader, "Category");
                            lodItem.AccList = GetSponsorGLJournalExportDataDetails(lodItem);
                            loEnList.Add(lodItem);
                        }
                        loReader.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            if (loEnList.Count != 0)
            {
                AccountsEn loaacen;
                System.Collections.Hashtable ht = new System.Collections.Hashtable();
                List<AccountsEn> lohtlist = new List<AccountsEn>();
                foreach (AccountsEn loen in loEnList)
                {
                    if (!ht.Contains(loen.BatchCode))
                    {
                        ht.Add(loen.BatchCode, loen);
                        lohtlist.Add(loen);
                    }
                }


                for (int i = 0; i < lohtlist.Count; i++)
                {
                    loaacen = new AccountsEn();
                    loaacen.BatchCode = lohtlist[i].BatchCode;
                    loaacen.UpdatedBy = argEn.UpdatedBy;
                    loaacen.UpdatedTime = argEn.UpdatedTime;
                    UpdateIntegrationStatus(loaacen);
                }
            }
            return loEnList;
        }

        #endregion

        #region GetSponsorGLJournalExportDataStDetails

        /// <summary>
        /// Method to Get Sponsor GL Journal ExportData list
        /// </summary>
        /// <param name="argEn">Accounts Entity is an Input.Category , Subtype,FromDate and toDate are Input properties</param>
        /// <returns>Returns List of Accounts</returns>
        public List<AccountsEn> GetSponsorGLJournalExportDataStDetails(AccountsEn argEn)
        {
            List<AccountsEn> loEnList = new List<AccountsEn>();

            string sqlCmd = "SELECT SAS_Student.*, SAS_AccountsDetails.TransAmount AS Expr1, SAS_AccountsDetails.PaidAmount AS Expr2," +
                            " SAS_Program.SAPG_AD AS Expr4, SAS_AccountsDetails.TempAmount AS Expr3 FROM SAS_AccountsDetails INNER JOIN " +
                           "  SAS_Student ON SAS_AccountsDetails.RefCode = SAS_Student.SASI_MatricNo INNER JOIN SAS_Program ON" +
                           " SAS_Student.SASI_PgId = SAS_Program.SAPG_Code WHERE (SAS_AccountsDetails.TransID = '" + argEn.TranssactionID + "')";

            try
            {
                if (!FormHelp.IsBlank(sqlCmd))
                {
                    using (IDataReader loReader = _DatabaseFactory.ExecuteReader(Helper.GetDataBaseType,
                        DataBaseConnectionString, sqlCmd).CreateDataReader())
                    {
                        while (loReader.Read())
                        {
                            AccountsEn loItem = new AccountsEn();
                            StudentDAL lostudal = new StudentDAL();
                            loItem.Student = new StudentEn();
                            loItem.Student = lostudal.LoadObject(loReader);
                            loItem.Student.AccountDetailsEn = new AccountsDetailsEn();
                            loItem.Student.AccountDetailsEn.TransactionAmount = GetValue<double>(loReader, "Expr1");
                            loItem.Student.AccountDetailsEn.PaidAmount = GetValue<double>(loReader, "Expr2");
                            loItem.Student.AccountDetailsEn.TempAmount = GetValue<double>(loReader, "Expr3");
                            loItem.Student.Programen = new ProgramInfoEn();
                            loItem.Student.Programen.Accountinfo = GetValue<string>(loReader, "Expr4");
                            loEnList.Add(loItem);

                        }
                        loReader.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return loEnList;
        }

        #endregion

        #region GetSponsorGLJournalExportDataDetails

        /// <summary>
        /// Method to Get Sponsor GL Journal ExportData list
        /// </summary>
        /// <param name="argEn">Accounts Entity is an Input.Category , Subtype,FromDate and toDate are Input properties</param>
        /// <returns>Returns List of Accounts</returns>
        public List<AccountsEn> GetSponsorGLJournalExportDataDetails(AccountsEn argEn)
        {
            List<AccountsEn> loEnList = new List<AccountsEn>();

            string sqlCmd = " SELECT SAS_Accounts.*, SAS_Sponsor.* FROM SAS_Accounts INNER JOIN " +
                            " SAS_Sponsor ON SAS_Accounts.CreditRef = SAS_Sponsor.SASR_Code" +
                            " WHERE SAS_Accounts.BatchCode = '" + argEn.BatchCode + "'";


            try
            {
                if (!FormHelp.IsBlank(sqlCmd))
                {
                    using (IDataReader loReader = _DatabaseFactory.ExecuteReader(Helper.GetDataBaseType,
                        DataBaseConnectionString, sqlCmd).CreateDataReader())
                    {
                        while (loReader.Read())
                        {
                            AccountsDetailsDAL lodeatilsds = new AccountsDetailsDAL();
                            AccountsDetailsEn lodetailItem = new AccountsDetailsEn();
                            AccountsEn loItem = LoadObject(loReader);
                            loItem.Sponsor = new SponsorEn();
                            loItem.Sponsor.SponserCode = GetValue<string>(loReader, "SASR_Code");
                            loItem.Sponsor.Name = GetValue<string>(loReader, "SASR_Name");
                            loItem.Sponsor.Address = GetValue<string>(loReader, "SASR_Address");
                            loItem.Sponsor.Address1 = GetValue<string>(loReader, "SASR_Address1");
                            loItem.Sponsor.Address2 = GetValue<string>(loReader, "SASR_Address2");
                            loItem.Sponsor.Contact = GetValue<string>(loReader, "SASR_Contact");
                            loItem.Sponsor.Phone = GetValue<string>(loReader, "SASR_Phone");
                            loItem.Sponsor.GLAccount = GetValue<string>(loReader, "SASR_GLAccount");
                            if (loItem.Category == "Allocation")
                            {
                                loItem.AccList = GetSponsorGLJournalExportDataStDetails(loItem);
                            }
                            loEnList.Add(loItem);
                        }
                        loReader.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return loEnList;
        }

        #endregion

        #region GetSponsorCBReciptExportData

        /// <summary>
        /// Method to Get Sponsor GL Journal ExportData list
        /// </summary>
        /// <param name="argEn">Accounts Entity is an Input.Category , Subtype,FromDate and toDate are Input properties</param>
        /// <returns>Returns List of Accounts</returns>
        public List<AccountsEn> GetSponsorCBReciptExportData(AccountsEn argEn)
        {
            List<AccountsEn> loEnList = new List<AccountsEn>();

            string sqlCmd = " SELECT SAS_Accounts.*, SAS_Sponsor.* FROM SAS_Accounts INNER JOIN " +
                            " SAS_Sponsor ON SAS_Accounts.CreditRef = SAS_Sponsor.SASR_Code" +
                            " WHERE (Category='" + argEn.Category + "' or Category='" + argEn.SubCategory + "') and SubType ='" + argEn.SubType + "'and  PostStatus ='POSTED' ";

            if (argEn.IntegrationCode == "True")
            {
                sqlCmd += " and TransDate between  '" + argEn.TransDate + "'and '" + argEn.BatchDate + "'";
            }
            if (argEn.IntegrationStatus == 0)
            {
                sqlCmd += " and IntStatus = " + argEn.IntegrationStatus + "";
            }
            sqlCmd += "order by TransDate,batchcode";

            try
            {
                if (!FormHelp.IsBlank(sqlCmd))
                {
                    using (IDataReader loReader = _DatabaseFactory.ExecuteReader(Helper.GetDataBaseType,
                        DataBaseConnectionString, sqlCmd).CreateDataReader())
                    {
                        while (loReader.Read())
                        {
                            AccountsDetailsDAL lodeatilsds = new AccountsDetailsDAL();
                            AccountsDetailsEn lodetailItem = new AccountsDetailsEn();
                            AccountsEn loItem = LoadObject(loReader);
                            loItem.Sponsor = new SponsorEn();
                            loItem.Sponsor.SponserCode = GetValue<string>(loReader, "SASR_Code");
                            loItem.Sponsor.Name = GetValue<string>(loReader, "SASR_Name");
                            loItem.Sponsor.Address = GetValue<string>(loReader, "SASR_Address");
                            loItem.Sponsor.Address1 = GetValue<string>(loReader, "SASR_Address1");
                            loItem.Sponsor.Address2 = GetValue<string>(loReader, "SASR_Address2");
                            loItem.Sponsor.Contact = GetValue<string>(loReader, "SASR_Contact");
                            loItem.Sponsor.Phone = GetValue<string>(loReader, "SASR_Phone");
                            loItem.Sponsor.GLAccount = GetValue<string>(loReader, "SASR_GLAccount");
                            loEnList.Add(loItem);
                        }
                        loReader.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            if (loEnList.Count != 0)
            {
                AccountsEn loaacen;
                System.Collections.Hashtable ht = new System.Collections.Hashtable();
                List<AccountsEn> lohtlist = new List<AccountsEn>();
                foreach (AccountsEn loen in loEnList)
                {
                    if (!ht.Contains(loen.BatchCode))
                    {
                        ht.Add(loen.BatchCode, loen);
                        lohtlist.Add(loen);
                    }
                }


                for (int i = 0; i < lohtlist.Count; i++)
                {
                    loaacen = new AccountsEn();
                    loaacen.BatchCode = lohtlist[i].BatchCode;
                    loaacen.UpdatedBy = argEn.UpdatedBy;
                    loaacen.UpdatedTime = argEn.UpdatedTime;
                    UpdateIntegrationStatus(loaacen);
                }
            }
            return loEnList;
        }

        #endregion

        #region GetStCBPaymentExportData

        /// <summary>
        /// Method to Get CB Payment ExportData list
        /// </summary>
        /// <param name="argEn">Accounts Entity is an Input.Category , Subtype,PostStatus,FromDate and toDate are Input properties</param>
        /// <returns>Returns List of Accounts</returns>
        public List<AccountsEn> GetStCBPaymentExportData(AccountsEn argEn)
        {
            List<AccountsEn> loEnList = new List<AccountsEn>();

            string sqlCmd = " SELECT distinct batchcode,category FROM SAS_Accounts" +
                            " WHERE (Category='" + argEn.Category + "' or Category='" + argEn.SubCategory + "') and SubType ='" + argEn.SubType + "'and  PostStatus ='POSTED'";

            if (argEn.IntegrationCode == "True")
            {
                sqlCmd += " and TransDate between  '" + argEn.TransDate + "'and '" + argEn.BatchDate + "'";
            }
            if (argEn.IntegrationStatus == 0)
            {
                sqlCmd += "and IntStatus = " + argEn.IntegrationStatus + "";
            }
            else { }

            try
            {
                if (!FormHelp.IsBlank(sqlCmd))
                {
                    using (IDataReader loReader = _DatabaseFactory.ExecuteReader(Helper.GetDataBaseType,
                       DataBaseConnectionString, sqlCmd).CreateDataReader())
                    {
                        while (loReader.Read())
                        {
                            AccountsDAL losds = new AccountsDAL();
                            AccountsEn lodItem = new AccountsEn();
                            lodItem.BatchCode = GetValue<string>(loReader, "BatchCode");
                            lodItem.Category = GetValue<string>(loReader, "Category");
                            lodItem.AccList = GetStCBPaymentDetails(lodItem);
                            loEnList.Add(lodItem);
                        }
                        loReader.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            if (loEnList.Count != 0)
            {
                AccountsEn loaacen;
                System.Collections.Hashtable ht = new System.Collections.Hashtable();
                List<AccountsEn> lohtlist = new List<AccountsEn>();
                foreach (AccountsEn loen in loEnList)
                {
                    if (!ht.Contains(loen.BatchCode))
                    {
                        ht.Add(loen.BatchCode, loen);
                        lohtlist.Add(loen);
                    }
                }


                for (int i = 0; i < lohtlist.Count; i++)
                {
                    loaacen = new AccountsEn();
                    loaacen.BatchCode = lohtlist[i].BatchCode;
                    loaacen.UpdatedBy = argEn.UpdatedBy;
                    loaacen.UpdatedTime = argEn.UpdatedTime;
                    UpdateIntegrationStatus(loaacen);
                }
            }
            return loEnList;
        }

        #endregion

        #region GetStCBPaymentDetails

        /// <summary>
        /// Method to Get CB Payment Detail list
        /// </summary>
        /// <param name="argEn">Accounts Entity is an Input.Category , Subtype,FromDate and toDate are Input properties</param>
        /// <returns>Returns List of Accounts</returns>
        public List<AccountsEn> GetStCBPaymentDetails(AccountsEn argEn)
        {
            List<AccountsEn> loEnList = new List<AccountsEn>();
            string sqlCmd = "SELECT  SAS_Program.SAPG_TI, SAS_Program.SAPG_AD, SAS_Accounts.*, SAS_Student.SASI_MatricNo," +
                            " SAS_Student.SASI_Name, SAS_Student.SASI_PgId, SAS_Student.SASI_Add1,SAS_Student.SASI_Add2," +
                            " SAS_Student.SASI_Add3, SAS_Student.SASI_City, SAS_Student.SASI_State, SAS_Student.SASI_Country, SAS_Student.SASI_Postcode" +
                            " FROM SAS_Program  SAS_Program INNER JOIN SAS_Student ON SAS_Program.SAPG_Code = SAS_Student.SASI_PgId RIGHT OUTER JOIN" +
                            " SAS_Accounts ON SAS_Student.SASI_MatricNo = SAS_Accounts.CreditRef " +
                            " Where Batchcode = '" + argEn.BatchCode + "' ";

            sqlCmd += "order by TransDate,batchcode";

            try
            {
                if (!FormHelp.IsBlank(sqlCmd))
                {
                    using (IDataReader loReader = _DatabaseFactory.ExecuteReader(Helper.GetDataBaseType,
                        DataBaseConnectionString, sqlCmd).CreateDataReader())
                    {
                        while (loReader.Read())
                        {

                            AccountsEn loItem = LoadObject(loReader);

                            if (loItem.Category == "STA" || loItem.Category == "Refund" || loItem.Category == "Receipt")
                            {
                                loItem.Student = new StudentEn();
                                loItem.Student.MatricNo = GetValue<string>(loReader, "SASI_MatricNo");
                                loItem.Student.StudentName = GetValue<string>(loReader, "SASI_Name");
                                loItem.Student.ProgramID = GetValue<string>(loReader, "SASI_PgId");
                                loItem.Student.SASI_Add1 = GetValue<string>(loReader, "SASI_Add1");
                                loItem.Student.SASI_Add2 = GetValue<string>(loReader, "SASI_Add2");
                                loItem.Student.SASI_Add3 = GetValue<string>(loReader, "SASI_Add3");
                                loItem.Student.SASI_City = GetValue<string>(loReader, "SASI_City");
                                loItem.Student.SASI_State = GetValue<string>(loReader, "SASI_State");
                                loItem.Student.SASI_Country = GetValue<string>(loReader, "SASI_Country");
                                loItem.Student.SASI_Postcode = GetValue<string>(loReader, "SASI_Postcode");

                                loItem.ProgramInfo = new ProgramInfoEn();
                                loItem.ProgramInfo.Accountinfo = GetValue<string>(loReader, "SAPG_AD");

                            }
                            loEnList.Add(loItem);
                        }
                        loReader.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return loEnList;
        }

        #endregion

        #region GetStudentOutstandingAmt

        /// <summary>
        /// Method to Get Student's Outstanding Amount
        /// </summary>
        /// <param name="argEn">Student Entity is an Input</param>
        /// <returns>Returns the Outstanding Amount</returns>
        public double GetStudentOutstandingAmt(StudentEn argEn)
        {
            double OutAmt = 0.0;
            double debit = 0.0;
            double credit = 0.0;
            string sqlCmd1 = " SELECT SUM(SAS_Accounts.TransAmount) AS DebitAmount FROM SAS_Accounts INNER JOIN SAS_Student ON" +
                            " SAS_Accounts.CreditRef = SAS_Student.SASI_MatricNo WHERE (SAS_Accounts.TransType = 'Credit') AND (SAS_Accounts.poststatus = 'Posted') AND (SASI_MatricNo = '" + argEn.MatricNo + "')";
            string sqlCmd2 = " SELECT SUM(SAS_Accounts.TransAmount) AS CreditAmount FROM SAS_Accounts INNER JOIN SAS_Student ON" +
                            " SAS_Accounts.CreditRef = SAS_Student.SASI_MatricNo WHERE (SAS_Accounts.TransType = 'Debit') and (SAS_Accounts.poststatus = 'Posted') AND (SASI_MatricNo = '" + argEn.MatricNo + "')";

            try
            {
                if (!FormHelp.IsBlank(sqlCmd1))
                {
                    using (IDataReader dr = _DatabaseFactory.ExecuteReader(Helper.GetDataBaseType,
                        DataBaseConnectionString, sqlCmd1).CreateDataReader())
                    {
                        if (dr.Read())
                            debit = GetValue<double>(dr, "DebitAmount");
                    }
                }
                //get creditamount
                if (!FormHelp.IsBlank(sqlCmd2))
                {
                    using (IDataReader dr = _DatabaseFactory.ExecuteReader(Helper.GetDataBaseType,
                        DataBaseConnectionString, sqlCmd2).CreateDataReader())
                    {
                        if (dr.Read())
                            credit = GetValue<double>(dr, "CreditAmount");
                    }
                }
                //total outstanding amount
                OutAmt = debit - credit;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return OutAmt;
        }

        #endregion

        #region GetTotalAllocatedAmount

        /// <summary>
        /// Method to Get Total's Allocated Amount
        /// </summary>
        /// <param name="argEn">Student Entity is an Input</param>
        /// <returns>Returns the Outstanding Amount</returns>
        public double GetTotalAllocatedAmount(AccountsEn argEn)
        {
            double Allocated = 0.0;
            //string sqlCmd = " select Sum(transamount) As TotalAllocated from sas_accounts where (creditref1 = '" +
            //                    argEn.TransactionCode + "' and PostStatus = '" + argEn.PostStatus + "' and TransStatus = '" + argEn.TransStatus + "')";
            string sqlCmd = " select Sum(transamount) As TotalAllocated from sas_accounts where (creditref1 = '" +
                                argEn.TransactionCode + "' and PostStatus = '" + argEn.PostStatus + "' and TransStatus = '" + argEn.TransStatus + "' and Category = '" + argEn.Category
                                + "' and subtype = '" + argEn.SubType + "')";
            try
            {
                if (!FormHelp.IsBlank(sqlCmd))
                {
                    using (IDataReader dr = _DatabaseFactory.ExecuteReader(Helper.GetDataBaseType,
                        DataBaseConnectionString, sqlCmd).CreateDataReader())
                    {
                        if (dr.Read())
                            Allocated = GetValue<double>(dr, "TotalAllocated");
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Allocated;
        }

        #endregion

        #region GetTotalAllocatedAmountWithTransID

        /// <summary>
        /// Method to GetTotalAllocatedAmountWithTransID
        /// </summary>
        /// <param name="argEn">Student Entity is an Input</param>
        /// <returns>Returns the GetTotalAllocatedAmountWithTransID</returns>
        public double GetTotalAllocatedAmountWithTransID(AccountsEn argEn)
        {
            double Allocated = 0.0;
            string sqlCmd = " select Sum(transamount) As TotalAllocated from sas_accounts where (creditref1 = '" + argEn.TransactionCode + "'and PostStatus ='" + argEn.PostStatus + "'and TransID ='" + argEn.TransactionID + "')";

            try
            {
                if (!FormHelp.IsBlank(sqlCmd))
                {
                    using (IDataReader dr = _DatabaseFactory.ExecuteReader(Helper.GetDataBaseType,
                        DataBaseConnectionString, sqlCmd).CreateDataReader())
                    {
                        if (dr.Read())
                            Allocated = GetValue<double>(dr, "TotalAllocated");
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Allocated;
        }

        #endregion

        #region GetAvailableAmountBasedonTransID

        /// <summary>
        /// Method to GetAvailableAmountBasedonTransID
        /// </summary>
        /// <param name="argEn">Student Entity is an Input</param>
        /// <returns>Returns the GetAvailableAmountBasedonTransID</returns>
        public double GetAvailableAmountBasedonTransID(AccountsEn argEn)
        {
            //double AvailableAmount = 0.0;
            //double amount1 = 0.0;
            //double amount2 = 0.0;
            //string sqlCmd = " select allocateamount from sas_accounts where (creditref1 = '" + argEn.TransactionCode + "'and PostStatus ='" + argEn.PostStatus + "'and TransID ='" + argEn.TransactionID + "')";

            //try
            //{
            //    if (!FormHelp.IsBlank(sqlCmd))
            //    {
            //        using (IDataReader dr = _DatabaseFactory.ExecuteReader(Helper.GetDataBaseType,
            //            DataBaseConnectionString, sqlCmd).CreateDataReader())
            //        {
            //            if (dr.Read())
            //                AvailableAmount = GetValue<double>(dr, "allocateamount");
            //        }
            //    }
            //}
            double AvailableAmount = 0.0;
            string sqlCmd = " select Sum(allocateamount) As AvailableAmount from sas_accounts where (batchcode = '" + argEn.TransactionCode + "'and PostStatus ='" + argEn.PostStatus + "')";

            try
            {
                if (!FormHelp.IsBlank(sqlCmd))
                {
                    using (IDataReader dr = _DatabaseFactory.ExecuteReader(Helper.GetDataBaseType,
                        DataBaseConnectionString, sqlCmd).CreateDataReader())
                    {
                        if (dr.Read())
                            AvailableAmount = GetValue<double>(dr, "AvailableAmount");
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return AvailableAmount;
        }

        #endregion


        #region LoadSponsor

        /// <summary>
        /// Method to Load the Sponsor Entity Objects
        /// </summary>
        /// <param name="argReader">Idatareader is an Input</param>
        /// <returns>Returns a Sponsor Item</returns>
        private SponsorEn LoadSponsor(IDataReader argReader)
        {
            SponsorEn loItem = new SponsorEn();
            loItem.SponserCode = GetValue<string>(argReader, "SASR_Code");
            loItem.Name = GetValue<string>(argReader, "SASR_Name");
            loItem.SName = GetValue<string>(argReader, "SASSR_SName");
            loItem.Address = GetValue<string>(argReader, "SASR_Address");
            loItem.Address2 = GetValue<string>(argReader, "SASR_Address2");
            loItem.Contact = GetValue<string>(argReader, "SASR_Contact");
            loItem.Phone = GetValue<string>(argReader, "SASR_Phone");
            loItem.Fax = GetValue<string>(argReader, "SASR_Fax");
            loItem.Email = GetValue<string>(argReader, "SASR_Email");
            loItem.WebSite = GetValue<string>(argReader, "SASR_WebSite");
            loItem.Type = GetValue<string>(argReader, "SASR_Type");
            loItem.Description = GetValue<string>(argReader, "SASR_Desc");
            loItem.GLAccount = GetValue<string>(argReader, "SASR_GLAccount");
            loItem.Status = GetValue<bool>(argReader, "SASR_Status");

            return loItem;
        }

        #endregion

        #region GetList

        /// <summary>
        /// Method to Get the List Of Accounts
        /// </summary>
        /// <param name="argEn">Accounts Entity is the Input</param>
        /// <returns>Returns the List of Accounts</returns>
        public List<AccountsEn> GetList(AccountsEn argEn)
        {
            List<AccountsEn> loEnList = new List<AccountsEn>();
            string sqlCmd = "select * from SAS_Accounts";

            try
            {
                if (!FormHelp.IsBlank(sqlCmd))
                {
                    using (IDataReader loReader = _DatabaseFactory.ExecuteReader(Helper.GetDataBaseType,
                       DataBaseConnectionString, sqlCmd).CreateDataReader())
                    {
                        while (loReader.Read())
                        {
                            AccountsEn loItem = LoadObject(loReader);
                            loEnList.Add(loItem);
                        }
                        loReader.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return loEnList;
        }

        #endregion

        #region GetListStudentAccounts

        /// <summary>
        /// Method to Get the List of Student Transactions
        /// </summary>
        /// <param name="argEn">Accounts Entity is the Input.BatchCode is the Input Property</param>
        /// <returns>Returns the List OF Accounts</returns>
        public List<AccountsEn> GetListStudentAccounts(AccountsEn argEn)
        {
            List<AccountsEn> loEnList = new List<AccountsEn>();
            argEn.BatchCode = argEn.BatchCode.Replace("*", "%");
            string sqlCmd = "SELECT SAS_Accounts.* FROM SAS_Accounts where  TransType='" +
                          argEn.TransType + "'";
            if (argEn.BatchCode.Length != 0) sqlCmd = sqlCmd + " AND BatchCode LIKE '" + argEn.BatchCode + "'";

            try
            {
                if (!FormHelp.IsBlank(sqlCmd))
                {
                    using (IDataReader loReader = _DatabaseFactory.ExecuteReader(Helper.GetDataBaseType,
                        DataBaseConnectionString, sqlCmd).CreateDataReader())
                    {
                        while (loReader.Read())
                        {
                            AccountsEn loItem = LoadObject(loReader);
                            loEnList.Add(loItem);
                        }
                        loReader.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return loEnList;
        }

        #endregion

        #region GetListStudentbyBatchID

        /// <summary>
        /// Method to Get the List of Students by BatchId
        /// </summary>
        /// <param name="argEn">Student Entity is an Input Parameter.BatchCode is the Input Property</param>
        /// <returns>Returns List of Students</returns>
        public List<StudentEn> GetListStudentbyBatchID(StudentEn argEn)
        {
            List<StudentEn> loEnList = new List<StudentEn>();
            string sqlCmd = " SELECT SAS_Accounts.TransID, SAS_Accounts.CreditRef, SAS_Accounts.BatchCode," +
                            " SAS_Student.SASI_MatricNo,SAS_Student.SASI_Name, SAS_Student.SASI_PgId, SAS_Student.SASI_CurSem" +
                            " FROM SAS_Accounts INNER JOIN SAS_Student ON SAS_Accounts.CreditRef = SAS_Student.SASI_MatricNo" +
                            " WHERE SAS_Accounts.BatchCode ='" + argEn.BatchCode + "'";

            try
            {
                if (!FormHelp.IsBlank(sqlCmd))
                {
                    using (IDataReader loReader = _DatabaseFactory.ExecuteReader(Helper.GetDataBaseType,
                       DataBaseConnectionString, sqlCmd).CreateDataReader())
                    {
                        StudentDAL lods = new StudentDAL();
                        while (loReader.Read())
                        {
                            StudentEn loItem = new StudentEn();
                            loItem.TranssactionID = GetValue<int>(loReader, "TransID");
                            loItem.CreditRef = GetValue<string>(loReader, "CreditRef");
                            loItem.BatchCode = GetValue<string>(loReader, "BatchCode");
                            loItem.MatricNo = GetValue<string>(loReader, "SASI_MatricNo");
                            loItem.StudentName = GetValue<string>(loReader, "SASI_Name");
                            loItem.ProgramID = GetValue<string>(loReader, "SASI_PgId");
                            loItem.CurrentSemester = GetValue<int>(loReader, "SASI_CurSem");

                            loEnList.Add(loItem);
                        }
                        loReader.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return loEnList;
        }

        #endregion

        #region GetStudentReceiptsbyBatchID

        /// <summary>
        /// Method to Get the Student Receipts by BatchID
        /// </summary>
        /// <param name="argEn">Student Entity is the Input.BatchCode is the Input Property</param>
        /// <returns>Returns List of Students</returns>
        /// Modified by Hafiz @ 23/3/2016 - add outstandingamt to sas_accounts
        public List<StudentEn> GetStudentReceiptsbyBatchID(StudentEn argEn)
        {
            List<StudentEn> loEnList = new List<StudentEn>();
            string sqlCmd = " SELECT SAS_Accounts.TransID,SAS_Accounts.PaidAmount,SAS_Accounts.subref2, SAS_Accounts.CreditRef,SAS_Accounts.TransCode,SAS_Accounts.ChequeNo, SAS_Accounts.BatchCode,SAS_Accounts.VoucherNo," +
                            " SAS_Student.SASI_MatricNo,SAS_Student.SASI_Name, SAS_Student.SASI_PgId, SAS_Student.SASI_CurSem, SAS_Student.SASI_Faculty, SAS_Student.SASI_ICNo, SAS_Accounts.TransAmount, SAS_Accounts.ReceiptDate, " +
                            " SAS_Accounts.bankrecno, SAS_Accounts.outstandingamt FROM SAS_Accounts INNER JOIN SAS_Student ON SAS_Accounts.CreditRef = SAS_Student.SASI_MatricNo" +
                            " WHERE SAS_Accounts.BatchCode ='" + argEn.BatchCode + "'" +
                            " ORDER BY SAS_Student.SASI_MatricNo ";

            try
            {
                if (!FormHelp.IsBlank(sqlCmd))
                {
                    using (IDataReader loReader = _DatabaseFactory.ExecuteReader(Helper.GetDataBaseType,
                        DataBaseConnectionString, sqlCmd).CreateDataReader())
                    {
                        StudentDAL lods = new StudentDAL();
                        while (loReader.Read())
                        {
                            StudentEn loItem = new StudentEn();
                            loItem.TranssactionID = GetValue<int>(loReader, "TransID");
                            loItem.TransactionCode = GetValue<string>(loReader, "TransCode");
                            loItem.ChequeNo = GetValue<string>(loReader, "ChequeNo");
                            loItem.CreditRef = GetValue<string>(loReader, "CreditRef");
                            loItem.BatchCode = GetValue<string>(loReader, "BatchCode");
                            loItem.MatricNo = GetValue<string>(loReader, "SASI_MatricNo");
                            loItem.StudentName = GetValue<string>(loReader, "SASI_Name");
                            loItem.ProgramID = GetValue<string>(loReader, "SASI_PgId");
                            loItem.CurrentSemester = GetValue<int>(loReader, "SASI_CurSem");
                            loItem.VoucherNo = GetValue<string>(loReader, "VoucherNo");
                            loItem.Faculty = GetValue<string>(loReader, "SASI_Faculty");
                            loItem.ICNo = GetValue<string>(loReader, "SASI_ICNo");
                            loItem.TransactionAmount = GetValue<double>(loReader, "TransAmount");
                            loItem.PaidAmount = GetValue<double>(loReader, "PaidAmount");
                            loItem.SubReferenceTwo = GetValue<string>(loReader, "subref2");
                            loItem.BankSlipID = GetValue<string>(loReader, "bankrecno");
                            //loItem.Outstanding_Amount = MaxGeneric.clsGeneric.NullToString(GetStudentOutstandingAmount(GetValue<string>(loReader, "CreditRef")));
                            loItem.Outstanding_Amount = GetValue<string>(loReader, "outstandingamt");
                            loItem.ReceiptDate = GetValue<DateTime>(loReader, "ReceiptDate");

                            if (argEn.StuIndex == 1)
                            {
                                AccountsDetailsEn loen = new AccountsDetailsEn();
                                loen.ReferenceOne = loItem.MatricNo;
                                loen.TransactionID = loItem.TranssactionID;
                                loItem.AccountDetailsList = GetAutoStudentReceiptsbyBatchID(loen);
                                loItem.AmountPaid = loItem.PaidAmount;
                            }
                            loEnList.Add(loItem);
                        }
                        loReader.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return loEnList;
        }

        #endregion

        #region GetAutoStudentReceiptsbyBatchID

        /// <summary>
        /// Method to Get the AutoAllocation AccountDetails for Receipts 
        /// </summary>
        /// <param name="argEn">AccountDetails Entity is the Input.ReferenceOne is the Input Property</param>
        /// <returns>Returns List of AccountDetails</returns>
        public List<AccountsDetailsEn> GetAutoStudentReceiptsbyBatchID(AccountsDetailsEn argEn)
        {
            List<AccountsDetailsEn> loEnList = new List<AccountsDetailsEn>();
            string sqlCmd = " SELECT  SAS_AccountsDetails.TransID, SAS_AccountsDetails.RefCode, SAS_AccountsDetails.PaidAmount as accountspaid ,SAS_Accounts_1.TransAmount AS Expr1, SAS_Accounts_1.PaidAmount AS Expr2," +
                            " SAS_AccountsDetails.Ref1, SAS_AccountsDetails.ref2, SAS_Accounts.CreditRef, SAS_Accounts.DueDate, SAS_Accounts.TransAmount, SAS_Accounts.TransDate, SAS_Accounts.PaidAmount , SAS_Accounts_1.TransCode  " +
                            " FROM  SAS_AccountsDetails INNER JOIN SAS_Accounts ON SAS_AccountsDetails.TransID = SAS_Accounts.TransID INNER JOIN SAS_Accounts SAS_Accounts_1 ON SAS_AccountsDetails.RefCode = SAS_Accounts_1.TransCode " +
                            " WHERE (SAS_AccountsDetails.Ref1 = '" + argEn.ReferenceOne + "' and SAS_AccountsDetails.TransID = '" + argEn.TransactionID + "' )";

            try
            {
                if (!FormHelp.IsBlank(sqlCmd))
                {
                    using (IDataReader loReader = _DatabaseFactory.ExecuteReader(Helper.GetDataBaseType,
                        DataBaseConnectionString, sqlCmd).CreateDataReader())
                    {

                        while (loReader.Read())
                        {
                            AccountsDetailsEn loItem = new AccountsDetailsEn();
                            loItem.TransactionID = GetValue<int>(loReader, "TransID");
                            loItem.ReferenceTwo = GetValue<string>(loReader, "Ref2");
                            loItem.ReferenceCode = GetValue<string>(loReader, "RefCode");
                            loItem.TempPaidAmount = GetValue<double>(loReader, "accountspaid");
                            loItem.ReferenceOne = GetValue<string>(loReader, "Ref1");
                            loItem.TransactionCode = GetValue<string>(loReader, "TransCode");
                            loItem.CreditRef = GetValue<string>(loReader, "CreditRef");
                            loItem.TransDate = GetValue<DateTime>(loReader, "TransDate");
                            loItem.DueDate = GetValue<DateTime>(loReader, "DueDate");
                            loItem.PaidAmount = GetValue<double>(loReader, "Expr2");
                            loItem.TransactionAmount = GetValue<double>(loReader, "Expr1");
                            loEnList.Add(loItem);
                        }
                        loReader.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return loEnList;
        }

        #endregion

        #region GetStudentAllocationTrans

        /// <summary>
        /// Method to Get Student Allocations
        /// </summary>
        /// <param name="argEn">Student Entity is the Input.</param>
        /// <returns>Returns List of Students.</returns>
        public List<StudentEn> GetStudentAllocationTrans(StudentEn argEn)
        {
            List<StudentEn> loEnList = new List<StudentEn>();
            string sqlCmd = " SELECT SAS_Accounts.TransID,SAS_Accounts.TransCode,SAS_Accounts.Category,SAS_Accounts.ChequeNo, SAS_Accounts.CreditRef,SAS_Accounts.Description, SAS_Accounts.BatchCode," +
                            " SAS_Student.SASI_MatricNo,SAS_Student.SASI_Name, SAS_Student.SASI_PgId, SAS_Student.SASI_CurSem, SAS_Student.SASI_Faculty, SAS_Student.SASI_ICNo,SAS_Accounts.VoucherNo, SAS_Accounts.TransAmount" +
                            " FROM SAS_Accounts INNER JOIN SAS_Student ON SAS_Accounts.CreditRef = SAS_Student.SASI_MatricNo" +
                            " WHERE SAS_Accounts.BatchCode ='" + argEn.BatchCode + "' and SAS_Accounts.Category ='" + argEn.Category + "' and SAS_Accounts.Description ='" + argEn.Description + "'";

            try
            {
                if (!FormHelp.IsBlank(sqlCmd))
                {
                    using (IDataReader loReader = _DatabaseFactory.ExecuteReader(Helper.GetDataBaseType,
                        DataBaseConnectionString, sqlCmd).CreateDataReader())
                    {
                        StudentDAL lods = new StudentDAL();
                        while (loReader.Read())
                        {
                            StudentEn loItem = new StudentEn();
                            loItem.TranssactionID = GetValue<int>(loReader, "TransID");
                            loItem.TransactionCode = GetValue<string>(loReader, "TransCode");
                            loItem.ChequeNo = GetValue<string>(loReader, "ChequeNo");
                            loItem.Category = GetValue<string>(loReader, "Category");
                            loItem.Description = GetValue<string>(loReader, "Description");
                            loItem.CreditRef = GetValue<string>(loReader, "CreditRef");
                            loItem.BatchCode = GetValue<string>(loReader, "BatchCode");
                            loItem.MatricNo = GetValue<string>(loReader, "SASI_MatricNo");
                            loItem.StudentName = GetValue<string>(loReader, "SASI_Name");
                            loItem.ProgramID = GetValue<string>(loReader, "SASI_PgId");
                            loItem.CurrentSemester = GetValue<int>(loReader, "SASI_CurSem");
                            loItem.Faculty = GetValue<string>(loReader, "SASI_Faculty");
                            loItem.ICNo = GetValue<string>(loReader, "SASI_ICNo");
                            loItem.VoucherNo = GetValue<string>(loReader, "Voucherno");
                            loItem.TransactionAmount = GetValue<double>(loReader, "TransAmount");

                            loEnList.Add(loItem);
                        }
                        loReader.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return loEnList;
        }

        #endregion

        #region GetTransactions

        /// <summary>
        /// Method to Get Transactions
        /// </summary>
        /// <param name="argEn">Accounts Entity is the Input.BatchCode,Category,PostStatus and SubType are the Input Property.</param>
        /// <returns>Returns List of Accounts.</returns>
        public List<AccountsEn> GetTransactions(AccountsEn argEn)
        {
            string sqlCmd;
            List<AccountsEn> loListAccounts = new List<AccountsEn>();
            DateTime dtBatchDate = argEn.BatchDate;
            string strBatchDate = dtBatchDate.ToString();
            argEn.BatchCode = argEn.BatchCode.Replace("*", "%");
            //string sqlCmd = "SELECT DISTINCT BatchCode FROM SAS_Accounts WHERE Category='" +
            //              argEn.Category + "' AND PostStatus='" + argEn.PostStatus + "' and SubType ='" + argEn.SubType + "' ";
            //if (argEn.BatchCode.Length != 0) sqlCmd = sqlCmd + " AND BatchCode LIKE '" + argEn.BatchCode + "' ";
            //try
            //{
            //    if (argEn.BatchIntake.Length != 0) sqlCmd = sqlCmd + " AND BatchIntake = '" + argEn.BatchIntake + "' ";
            //}
            //catch (Exception bt)
            //{
            //}
            //sqlCmd = sqlCmd + "ORDER BY BatchCode DESC ";

            if (argEn.Category == "")
            {
                sqlCmd = "SELECT DISTINCT BatchCode FROM SAS_ACCOUNTS where (Category='Debit Note' OR Category='Credit Note')"
                    + " AND PostStatus='" + argEn.PostStatus + "' AND SubType ='" + argEn.SubType + "' ";
            }

            else
            {

                sqlCmd = "SELECT DISTINCT BatchCode FROM SAS_Accounts WHERE Category='" +
                              argEn.Category + "' AND PostStatus='" + argEn.PostStatus + "' and SubType ='" + argEn.SubType + "' ";
            }

            //Added by Hafiz @ 16/2/2016
            //Modified by Hafiz @ 29/2/2016
            //When Receipt to Stud Loan, seek subcategory for its  references - Start
            if (!string.IsNullOrEmpty(argEn.SubCategory))
            {
                sqlCmd = sqlCmd + "AND subcategory = '" + argEn.SubCategory + "' ";
            }
            //When Receipt to Stud Loan, seek subcategory for its  references - Stop

            if (argEn.BatchCode.Length != 0) sqlCmd = sqlCmd + " AND BatchCode LIKE '" + argEn.BatchCode + "' ";

            try
            {
                if (!string.IsNullOrEmpty(argEn.BatchIntake))
                {
                    if (argEn.BatchIntake.Length != 0) sqlCmd = sqlCmd + " AND BatchIntake = '" + argEn.BatchIntake + "' ";
                }
            }
            catch (Exception bt)
            {
                throw bt;
            }
            sqlCmd = sqlCmd + "ORDER BY BatchCode DESC ";

            try
            {
                if (!FormHelp.IsBlank(sqlCmd))
                {
                    using (IDataReader loReader = _DatabaseFactory.ExecuteReader(Helper.GetDataBaseType,
                        DataBaseConnectionString, sqlCmd).CreateDataReader())
                    {
                        while (loReader.Read())
                        {
                            argEn.BatchCode = GetValue<string>(loReader, "BatchCode");
                            loListAccounts.Add(GetItem(argEn));
                        }
                        loReader.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return loListAccounts;
        }

        #endregion

        #region GetItem

        /// <summary>
        /// Method to Get an Accounts Item
        /// </summary>
        /// <param name="argEn">Accounts Entity is the Input.BatchCode is the Input Property</param>
        /// <returns>Returns an Accounts Item</returns>
        public AccountsEn GetItem(AccountsEn argEn)
        {
            AccountsEn loItem = new AccountsEn();
            string sqlCmd = "Select * FROM SAS_Accounts WHERE BatchCode = @BatchCode";

            try
            {
                if (!FormHelp.IsBlank(sqlCmd))
                {
                    DbCommand cmd = _DatabaseFactory.GetDbCommand(Helper.GetDataBaseType, sqlCmd, DataBaseConnectionString);
                    _DatabaseFactory.AddInParameter(ref cmd, "@BatchCode", DbType.String, argEn.BatchCode);
                    _DbParameterCollection = cmd.Parameters;

                    using (IDataReader loReader = _DatabaseFactory.GetIDataReader(Helper.GetDataBaseType, cmd,
                        DataBaseConnectionString, sqlCmd, _DbParameterCollection).CreateDataReader())
                    {
                        if (loReader != null)
                        {
                            loReader.Read();
                            loItem = LoadObject(loReader);

                        }
                        loReader.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return loItem;
        }

        #endregion

        #region GetItemAllocation

        /// <summary>
        /// Method to Get an Accounts Item
        /// </summary>
        /// <param name="argEn">Accounts Entity is the Input.BatchCode is the Input Property</param>
        /// <returns>Returns an Accounts Item</returns>
        public AccountsEn GetItemAllocation(AccountsEn argEn)
        {
            AccountsEn loItem = new AccountsEn();
            string sqlCmd = "Select * FROM SAS_Accounts WHERE BatchCode = @BatchCode AND Category = 'Allocation' ";

            try
            {
                if (!FormHelp.IsBlank(sqlCmd))
                {
                    DbCommand cmd = _DatabaseFactory.GetDbCommand(Helper.GetDataBaseType, sqlCmd, DataBaseConnectionString);
                    _DatabaseFactory.AddInParameter(ref cmd, "@BatchCode", DbType.String, argEn.BatchCode);
                    _DbParameterCollection = cmd.Parameters;

                    using (IDataReader loReader = _DatabaseFactory.GetIDataReader(Helper.GetDataBaseType, cmd,
                        DataBaseConnectionString, sqlCmd, _DbParameterCollection).CreateDataReader())
                    {
                        if (loReader != null)
                        {
                            loReader.Read();
                            loItem = LoadObject(loReader);

                        }
                        loReader.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return loItem;
        }

        #endregion

        #region GetListSucceedHeader

        /// <summary>
        /// Method to Get Transactions
        /// </summary>
        /// <param name="argEn">Accounts Entity is the Input.BatchCode,Category,PostStatus and SubType are the Input Property.</param>
        /// <returns>Returns List of Accounts.</returns>
        public List<AccountsEn> GetListSucceedHeader()
        {
            List<AccountsEn> loListAccounts = new List<AccountsEn>();
            string sqlCmd = "SELECT DISTINCT * FROM SAS_SucceedTransactionHeader WHERE STH_AutoNumber <> '' ";

            try
            {
                if (!FormHelp.IsBlank(sqlCmd))
                {
                    using (IDataReader loReader = _DatabaseFactory.ExecuteReader(Helper.GetDataBaseType,
                        DataBaseConnectionString, sqlCmd).CreateDataReader())
                    {
                        while (loReader.Read())
                        {
                            AccountsEn argEn = new AccountsEn();
                            AccountsDetailsEn arDetEn = new AccountsDetailsEn();
                            argEn.AutoNum = GetValue<string>(loReader, "STH_AutoNumber");
                            argEn.NoKelompok_H = GetValue<string>(loReader, "STH_NoKelompok_H");
                            argEn.KodUniversiti = GetValue<string>(loReader, "STH_KodUni");
                            argEn.TarikhProses = GetValue<string>(loReader, "STH_TarikhProses");
                            argEn.KumpulanPelajar = GetValue<string>(loReader, "STH_KumpulanPelajar");
                            argEn.KodBank = GetValue<string>(loReader, "STH_KodBank");
                            argEn.NoKelompok_F = GetValue<string>(loReader, "STH_NoKelompok_F");
                            loListAccounts.Add(argEn);
                        }
                        loReader.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return loListAccounts;
        }

        #endregion

        #region GetPayments

        /// <summary>
        /// Method to Get Payments
        /// </summary>
        /// <param name="argEn">Accounts Entity is the Input.BatchCode,Category,PostStatus and SubType are the Input Properties.</param>
        /// <returns>Returns List of Accounts</returns>
        public List<AccountsEn> GetPayments(AccountsEn argEn)
        {
            List<AccountsEn> loListAccounts = new List<AccountsEn>();
            argEn.BatchCode = argEn.BatchCode.Replace("*", "%");
            string sqlCmd = "SELECT DISTINCT BatchCode FROM SAS_Accounts SAS_Accounts LEFT OUTER JOIN SAS_Cheque ON SAS_Accounts.TransCode = SAS_Cheque.PaymentNo WHERE Category='" +
                          argEn.Category + "' AND PostStatus='" + argEn.PostStatus + "' and SubType ='" + argEn.SubType + "' and paymentmode = '1' AND ((SAS_Cheque.PrintStatus IS NULL) OR (SAS_Cheque.PrintStatus <> N'Posted'))";
            if (argEn.BatchCode.Length != 0) sqlCmd = sqlCmd + " AND BatchCode LIKE '" + argEn.BatchCode + "'";

            try
            {
                if (!FormHelp.IsBlank(sqlCmd))
                {
                    using (IDataReader loReader = _DatabaseFactory.ExecuteReader(Helper.GetDataBaseType,
                        DataBaseConnectionString, sqlCmd).CreateDataReader())
                    {
                        while (loReader.Read())
                        {
                            argEn.BatchCode = GetValue<string>(loReader, "BatchCode");
                            loListAccounts.Add(GetItem(argEn));
                        }
                        loReader.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return loListAccounts;
        }

        #endregion

        #region GetSPAllocationTransactions

        /// <summary>
        /// Method to Get Sponsor Allocations
        /// </summary>
        /// <param name="argEn">Accounts Entity is the Input.BatchCode,TransStatus,Category,PostStatus and SubType are Input Properties.</param>
        /// <returns>Returns List of Accounts</returns>
        public List<AccountsEn> GetSPAllocationTransactions(AccountsEn argEn)
        {
            //previous codes
            //List<AccountsEn> loListAccounts = new List<AccountsEn>();
            //argEn.BatchCode = argEn.BatchCode.Replace("*", "%");
            //argEn.TransStatus = argEn.TransStatus.Replace("*", "%");
            //string sqlCmd = "SELECT * FROM SAS_Accounts WHERE Category='" +
            //              argEn.Category + "' AND PostStatus='" + argEn.PostStatus + "' and SubType ='" + argEn.SubType + "'";
            //if (argEn.BatchCode.Length != 0) sqlCmd = sqlCmd + " AND BatchCode LIKE '" + argEn.BatchCode + "'";
            //if (argEn.TransStatus.Length != 0) sqlCmd = sqlCmd + " AND TransStatus LIKE '" + argEn.TransStatus + "'";

            //try
            //{
            //    if (!FormHelp.IsBlank(sqlCmd))
            //    {
            //        using (IDataReader loReader = _DatabaseFactory.ExecuteReader(Helper.GetDataBaseType,
            //           DataBaseConnectionString, sqlCmd).CreateDataReader())
            //        {
            //            while (loReader.Read())
            //            {
            //                AccountsEn loItem = LoadObject(loReader);
            //                loListAccounts.Add(loItem);
            //            }
            //            loReader.Close();
            //        }
            //    }
            //}
            //catch (Exception ex)
            //{
            //    throw ex;
            //}
            //return loListAccounts;

            //edit by farid 15/2/2016
            List<AccountsEn> loListAccounts = new List<AccountsEn>();
            argEn.BatchCode = argEn.BatchCode.Replace("*", "%");
            string sqlCmd = "SELECT * FROM SAS_Accounts WHERE Category='" +
                          argEn.Category + "' AND PostStatus='" + argEn.PostStatus + "' and SubType ='" + argEn.SubType + "'";
            if (argEn.BatchCode.Length != 0) sqlCmd = sqlCmd + " AND BatchCode LIKE '" + argEn.BatchCode + "'";

            sqlCmd = sqlCmd + "ORDER BY BatchCode DESC ";
            try
            {
                if (!FormHelp.IsBlank(sqlCmd))
                {
                    using (IDataReader loReader = _DatabaseFactory.ExecuteReader(Helper.GetDataBaseType,
                       DataBaseConnectionString, sqlCmd).CreateDataReader())
                    {
                        while (loReader.Read())
                        {
                            AccountsEn loItem = LoadObject(loReader);
                            loListAccounts.Add(loItem);
                        }
                        loReader.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return loListAccounts;
        }

        #endregion

        #region GetSPAllocationPayments

        /// <summary>
        /// Method to Get Sponsor Allocation Payments
        /// </summary>
        /// <param name="argEn">Accounts Entity is the Input.BatchCode,TransStatus,Category,PostStatus and SubType are Input Properties.</param>
        /// <returns>Returns List of Accounts</returns>
        public List<AccountsEn> GetSPAllocationPayments(AccountsEn argEn)
        {
            List<AccountsEn> loListAccounts = new List<AccountsEn>();
            argEn.BatchCode = argEn.BatchCode.Replace("*", "%");
            argEn.TransStatus = argEn.TransStatus.Replace("*", "%");
            string sqlCmd = "SELECT * FROM SAS_Accounts LEFT OUTER JOIN SAS_Cheque ON SAS_Accounts.TransCode = SAS_Cheque.PaymentNo WHERE Category ='" +
                          argEn.Category + "' AND PostStatus='" + argEn.PostStatus + "' and SubType ='" + argEn.SubType + "'and paymentmode = '1'  AND ((SAS_Cheque.PrintStatus IS NULL) OR (SAS_Cheque.PrintStatus <> N'Posted'))";
            if (argEn.BatchCode.Length != 0) sqlCmd = sqlCmd + " AND BatchCode LIKE '" + argEn.BatchCode + "'";
            if (argEn.TransStatus.Length != 0) sqlCmd = sqlCmd + " AND TransStatus LIKE '" + argEn.TransStatus + "'";

            try
            {
                if (!FormHelp.IsBlank(sqlCmd))
                {
                    using (IDataReader loReader = _DatabaseFactory.ExecuteReader(Helper.GetDataBaseType,
                        DataBaseConnectionString, sqlCmd).CreateDataReader())
                    {
                        while (loReader.Read())
                        {
                            AccountsEn loItem = LoadObject(loReader);
                            loListAccounts.Add(loItem);
                        }
                        loReader.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return loListAccounts;
        }

        #endregion

        #region GetstAllocationAmounts

        /// <summary>
        /// Method to Get Sum of Student Allocation Amounts
        /// </summary>
        /// <param name="argEn">Accounts Entity is the Input.CreditRef,PostStatus,SubType and Description are Input Properties</param>
        /// <returns>Returns Total Student Allocation Amount</returns>
        public double GetstAllocationAmounts(AccountsEn argEn)
        {
            double allocAmount = 0.0;
            string sqlCmd = "SELECT SUM(SAS_Accounts.TransAmount) AS AllocAmount FROM SAS_Accounts WHERE CreditRef='" +
                          argEn.CreditRef + "' AND PostStatus='" + argEn.PostStatus + "' and SubType ='" + argEn.SubType + "' and Description = '" + argEn.Description + "'";


            try
            {
                if (!FormHelp.IsBlank(sqlCmd))
                {
                    using (IDataReader loReader = _DatabaseFactory.ExecuteReader(Helper.GetDataBaseType,
                        DataBaseConnectionString, sqlCmd).CreateDataReader())
                    {
                        if (loReader.Read())
                            allocAmount = GetValue<double>(loReader, "AllocAmount");

                        loReader.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return allocAmount;
        }

        #endregion

        #region GetReceiptItem

        /// <summary>
        /// Method to Get a Receipt
        /// </summary>
        /// <param name="argEn">Accounts Entity is the Input.TransactionCode is the Input Property</param>
        /// <returns>Returns a Acoounts Item</returns>
        public AccountsEn GetReceiptItem(AccountsEn argEn)
        {
            AccountsEn loItem = new AccountsEn();
            argEn.Category = argEn.Category.Replace("*", "%");
            string sqlCmd = "Select * FROM SAS_Accounts WHERE TransCode = @TransCode ";
            if (argEn.Category.Length != 0) sqlCmd = sqlCmd + " AND Category LIKE '" + argEn.Category + "'";

            try
            {
                if (!FormHelp.IsBlank(sqlCmd))
                {
                    DbCommand cmd = _DatabaseFactory.GetDbCommand(Helper.GetDataBaseType, sqlCmd, DataBaseConnectionString);
                    _DatabaseFactory.AddInParameter(ref cmd, "@TransCode", DbType.String, argEn.TransactionCode);
                    _DbParameterCollection = cmd.Parameters;

                    using (IDataReader loReader = _DatabaseFactory.GetIDataReader(Helper.GetDataBaseType, cmd,
                        DataBaseConnectionString, sqlCmd, _DbParameterCollection).CreateDataReader())
                    {
                        while (loReader.Read())
                        {
                            loItem = LoadObject(loReader);
                        }
                        loReader.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return loItem;
        }

        #endregion

        #region GetItemReceipt

        /// <summary>
        /// Method to Get Sponsor Receipt
        /// </summary>
        /// <param name="argEn">Accounts Entity is the Input.TranssactionCode is the Input Property</param>
        /// <returns>Returns an Account Item</returns>
        public AccountsEn GetItemReceipt(AccountsEn argEn)
        {
            AccountsEn loItem = new AccountsEn();
            string sqlCmd = "SELECT SAS_Sponsor.SASR_Name ,SAS_Accounts.TransAmount, SAS_Accounts.PaidAmount , " +
                            " SAS_Accounts.Category FROM SAS_Accounts INNER JOIN SAS_Sponsor ON SAS_Sponsor.SASR_Code " +
                            " =SAS_Accounts.CreditRef  WHERE SAS_Accounts.TransCode = '" +
                            argEn.TransactionCode + "' and SAS_Accounts.Category = 'Receipt'";

            try
            {
                if (!FormHelp.IsBlank(sqlCmd))
                {

                    using (IDataReader loReader = _DatabaseFactory.ExecuteReader(Helper.GetDataBaseType,
                        DataBaseConnectionString, sqlCmd).CreateDataReader())
                    {
                        while (loReader.Read())
                        {
                            loItem.Sponsor = new SponsorEn();
                            loItem.TransactionAmount = GetValue<double>(loReader, "TransAmount");
                            loItem.PaidAmount = GetValue<double>(loReader, "PaidAmount");
                            loItem.Category = GetValue<string>(loReader, "Category");
                            loItem.Sponsor.Name = GetValue<string>(loReader, "SASR_Name");
                        }
                        loReader.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return loItem;
        }

        #endregion

        #region GetItemReceiptAllow

        /// <summary>
        /// Method to Get Sponsor Receipt
        /// </summary>
        /// <param name="argEn">Accounts Entity is the Input.TranssactionCode is the Input Property</param>
        /// <returns>Returns an Account Item</returns>
        public AccountsEn GetItemReceiptAllow(AccountsEn argEn)
        {
            AccountsEn loItem = new AccountsEn();
            string sqlCmd = "SELECT SAS_Sponsor.SASR_Name ,SAS_Accounts.TransAmount, SAS_Accounts.PaidAmount , " +
                            " SAS_Accounts.Category FROM SAS_Accounts INNER JOIN SAS_Sponsor ON SAS_Sponsor.SASR_Code " +
                            " =SAS_Accounts.CreditRef  WHERE SAS_Accounts.Creditref1 = '" +
                            argEn.TransactionCode + "' and SAS_Accounts.Category = 'Allocation'";

            try
            {
                if (!FormHelp.IsBlank(sqlCmd))
                {

                    using (IDataReader loReader = _DatabaseFactory.ExecuteReader(Helper.GetDataBaseType,
                        DataBaseConnectionString, sqlCmd).CreateDataReader())
                    {
                        while (loReader.Read())
                        {
                            loItem.Sponsor = new SponsorEn();
                            loItem.TransactionAmount = GetValue<double>(loReader, "TransAmount");
                            loItem.PaidAmount = GetValue<double>(loReader, "PaidAmount");
                            loItem.Category = GetValue<string>(loReader, "Category");
                            loItem.Sponsor.Name = GetValue<string>(loReader, "SASR_Name");
                            loItem.SponsorName = GetValue<string>(loReader, "SASR_Name");
                           // loItem.AllocatedAmount = GetValue<double>(loReader, "allocateamount");
                        }
                        loReader.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return loItem;
        }

        #endregion

        #region GetItemTrans

        /// <summary>
        /// Method to Get Transaction Amount For Student
        /// </summary>
        /// <param name="argEn">Accounts Entity is the Input.Student MatricNo is the Input Property</param>
        /// <returns>Returns an Account Item</returns>
        public AccountsEn GetItemTrans(StudentEn argEn)
        {
            AccountsEn loItem = new AccountsEn();
            string sqlCmd = "select Sb_TransAmount,Sb_Name from jbit_billing inner join SAS_AFC " +
            "on jbit_billing.Sb_BatchCode = SAS_AFC.BatchCode inner join SAS_Student on jbit_billing.Sb_MatricNo = " +
            "SAS_Student.SASI_MatricNo where SAS_AFC.Semester = SAS_Student.SASI_CurSemYr " +
            "and jbit_billing.Sb_MatricNo = '" + argEn.MatricNo + "'";

            try
            {
                if (!FormHelp.IsBlank(sqlCmd))
                {
                    using (IDataReader loReader = _DatabaseFactory.ExecuteReader(Helper.GetDataBaseType,
                         DataBaseConnectionString, sqlCmd).CreateDataReader())
                    {
                        while (loReader.Read())
                        {
                            loItem.AllocatedAmount = GetValue<double>(loReader, "Sb_TransAmount");
                        }
                        loReader.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                //throw ex;
                return null;
            }
            return loItem;
        }

        #endregion

        #region GetHeaderPTPTN

        /// <summary>
        /// Method to Get Header For PTPTN Text Filer
        /// </summary>
        /// <param name="argEn">Accounts Entity is the Input.Student MatricNo is the Input Property</param>
        /// <returns>Returns an Account Item</returns>
        public AccountsEn GetHeaderPTPTN(string receiptNo)
        {
            AccountsEn loItem = new AccountsEn();
            string sqlCmd = "Select KodUniversiti,KumpulanPelajar,TarikhProses,KodBank from SAS_Accounts where CreditRef1 = '" + receiptNo +
            "' and category = 'Allocation' and PostStatus = 'Posted'";

            try
            {
                if (!FormHelp.IsBlank(sqlCmd))
                {
                    using (IDataReader loReader = _DatabaseFactory.ExecuteReader(Helper.GetDataBaseType,
                         DataBaseConnectionString, sqlCmd).CreateDataReader())
                    {
                        while (loReader.Read())
                        {
                            loItem.KodUniversiti = GetValue<string>(loReader, "KodUniversiti");
                            loItem.KumpulanPelajar = GetValue<string>(loReader, "KumpulanPelajar");
                            loItem.TarikhProses = GetValue<string>(loReader, "TarikhProses");
                            loItem.KodBank = GetValue<string>(loReader, "KodBank");
                        }
                        loReader.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                //throw ex;
                return null;
            }
            return loItem;
        }

        #endregion

        #region GetItemByTransCode

        /// <summary>
        /// Method to Get an Account Item by TransCode
        /// </summary>
        /// <param name="argEn">Accounts Entity is the Input.TransCode is the Input Property</param>
        /// <returns>Returns an Account Item</returns>
        public AccountsEn GetItemByTransCode(AccountsEn argEn)
        {
            AccountsEn loItem = new AccountsEn();
            string sqlCmd = "Select * FROM SAS_Accounts WHERE TransCode = @TransCode";

            try
            {
                if (!FormHelp.IsBlank(sqlCmd))
                {
                    DbCommand cmd = _DatabaseFactory.GetDbCommand(Helper.GetDataBaseType, sqlCmd, DataBaseConnectionString);
                    _DatabaseFactory.AddInParameter(ref cmd, "@TransCode", DbType.String, argEn.TransactionCode);
                    _DbParameterCollection = cmd.Parameters;

                    using (IDataReader loReader = _DatabaseFactory.GetIDataReader(Helper.GetDataBaseType, cmd,
                        DataBaseConnectionString, sqlCmd, _DbParameterCollection).CreateDataReader())
                    {
                        if (loReader != null)
                        {
                            loReader.Read();
                            loItem = LoadObject(loReader);

                        }
                        loReader.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return loItem;
        }

        #endregion

        #region StudentBatchInsert

        /// <summary>
        /// Method to Insert StudentBatch
        /// </summary>
        /// <param name="argEn">Accounts Entity is the Input</param>
        /// <param name="argList">List of Student Entity is the Input</param>
        /// <returns>Returns BatchCode</returns>
        /// modified by Hafiz @ 29/6/2016 - CIMB Clicks BatchCode`s related

        public string StudentBatchInsert(AccountsEn argEn, List<StudentEn> argList, bool isAutoDetails = false)
        {
            int i = 0;

            if (argEn.Description.Contains("CIMB CLICKS"))
            {
                if (!String.IsNullOrEmpty(argEn.BatchCode))
                {
                    String Temp_BatchCode = argEn.BatchCode;
                    argEn.BatchCode = Temp_BatchCode;
                }
                else
                {
                    argEn.BatchCode = GetAutoNumber("Batch");
                }
            }
            else
            {
                argEn.BatchCode = GetAutoNumber("Batch");
            }

            if (isAutoDetails)
            {
                List<string> getStudent = argList.Select(p => p.MatricNo).Distinct().ToList();

                foreach (var stu in getStudent)
                {
                    List<StudentEn> newArgList = new List<StudentEn>();
                    newArgList = argList.Where(p => p.MatricNo == stu).ToList();
                    List<AccountsDetailsEn> newlistAccDetails = new List<AccountsDetailsEn>();
                    foreach (var data in newArgList)
                    {
                        AccountsDetailsEn newAccDetails = new AccountsDetailsEn();
                        newAccDetails.TransactionAmount = data.TransactionAmount;
                        newAccDetails.ReferenceCode = data.ReferenceCode;
                        newAccDetails.TaxAmount = data.TaxAmount;
                        newAccDetails.PostStatus = argEn.PostStatus;
                        newAccDetails.TransStatus = argEn.TransStatus;
                        newAccDetails.Priority = data.Priority;
                        newAccDetails.Internal_Use = data.Internal_Use;
                        newlistAccDetails.Add(newAccDetails);
                    }
                    argEn.CreditRef = stu;// argList[i].MatricNo;
                    argEn.AccountDetailsList = newlistAccDetails;
                    try
                    {
                        //Insert Receipt Details - Start                    
                        InsertReceiptDetails(argList[i].MatricNo, argList[i].ICNo, argEn.SubReferenceOne, argList[i].TransDate, argList[i].TransactionAmount);
                        //Insert Receipt Details - End
                    }
                    catch (Exception ex)
                    {
                        MaxModule.Helper.LogError(ex.Message);
                        throw ex;
                    }

                    Insert(argEn);
                    //Updating outstanding amount
                }


            }
            else
            {
                for (i = 0; i < argList.Count; i++)
                {
                    if (argEn.Category == "Receipt")
                    {
                        argEn.TransactionAmount = argList[i].TransactionAmount;
                        argEn.AccountDetailsList = argList[i].AccountDetailsList;
                        argEn.PaidAmount = argList[i].PaidAmount;
                        argEn.StManual = argList[i].SubReferenceTwo;
                        argEn.TransactionCode = argList[i].TransactionCode;
                        argEn.ReceiptDate = argList[i].ReceiptDate;
                    }
                    if (argEn.Category == "CIMBCLICKS")
                    {
                        argEn.TransactionAmount = argList[i].TransactionAmount;
                        argEn.AccountDetailsList = argList[i].AccountDetailsList;
                        argEn.PaidAmount = argList[i].PaidAmount;
                        argEn.StManual = argList[i].SubReferenceTwo;
                        argEn.TransactionCode = argList[i].TransactionCode;
                        argEn.ReceiptDate = argList[i].ReceiptDate;
                    }
                    if (argEn.Category == "Refund")
                    {
                        argEn.TransactionAmount = argList[i].TransactionAmount;
                        //added by farid on 240216
                        argEn.CreditRef = argEn.CreditRef;
                    }
                    if (argEn.Category == "AFC")
                    {
                        argEn.TransactionAmount = argList[i].TempAmount;
                    }
                    if (argEn.Category == "Payment")
                    {
                        //edit by farid on 240216
                        argEn.CreditRef = argList[i].TransactionCode;
                        //added by farid on 240216
                        //argEn.CreditRef = argEn.CreditRef;
                    }
                    else
                    {
                        argEn.CreditRef = argList[i].MatricNo;
                    }

                    argEn.BankSlipID = argList[i].BankSlipID;
                    argEn.Outstanding_Amount = argList[i].Outstanding_Amount;

                    //if (isAutoDetails == true)
                    //{

                    //    //argEn.TransactionAmount = argList[i].TransactionAmount;
                    //    List<AccountsDetailsEn> newlistAccDetails = new List<AccountsDetailsEn>();
                    //    argEn.AccountDetailsList = newlistAccDetails;
                    //    AccountsDetailsEn newAccDetails = new AccountsDetailsEn();
                    //    newAccDetails.TransactionAmount = argList[i].TransactionAmount;
                    //    newAccDetails.ReferenceCode = argList[i].ReferenceCode;
                    //    newAccDetails.TaxAmount = argList[i].TaxAmount;
                    //    newAccDetails.PostStatus = "Ready";
                    //    newAccDetails.TransStatus = "Open";
                    //    newAccDetails.Priority = argList[i].Priority;
                    //    newAccDetails.Internal_Use = argList[i].Internal_Use;
                    //    newlistAccDetails.Add(newAccDetails);
                    //    argEn.AccountDetailsList = newlistAccDetails;
                    //}
                    try
                    {
                        //Insert Receipt Details - Start                    
                        InsertReceiptDetails(argList[i].MatricNo, argList[i].ICNo, argEn.SubReferenceOne, argList[i].TransDate, argList[i].TransactionAmount);
                        //Insert Receipt Details - End
                    }
                    catch (Exception ex)
                    {
                        MaxModule.Helper.LogError(ex.Message);
                        throw ex;
                    }

                    Insert(argEn);
                    //Updating outstanding amount

                    //tambah by farid 25022016
                    if (argEn.Category == "Payment" && argEn.PostStatus == "Ready")
                    {
                        AccountsEn loAccounts;
                        AccountsEn loen;
                        List<AccountsDetailsEn> loAccDet = new List<AccountsDetailsEn>();

                        int j = 0;
                        for (j = 0; j < argEn.AccountDetailsList.Count; j++)
                        {
                            //loAccounts = new AccountsEn();
                            loAccounts = new AccountsEn();
                            loen = new AccountsEn();

                            loAccounts.TransStatus = "Open";
                            loAccounts.PostStatus = "Ready";
                            loAccounts.PostedDateTime = DateTime.Now;
                            loAccounts.UpdatedTime = DateTime.Now;
                            loAccounts.DueDate = DateTime.Now;
                            loAccounts.CreatedDateTime = DateTime.Now;
                            loAccounts.TransDate = argEn.TransDate;
                            loAccounts.ChequeDate = argEn.ChequeDate;
                            loAccounts.BatchDate = argEn.BatchDate;
                            loAccounts.BatchCode = argEn.BatchCode;
                            loAccounts.BankCode = argEn.BankCode;
                            loAccounts.PaymentMode = argEn.PaymentMode;
                            loAccounts.CreditRefOne = argEn.TransactionCode;
                            loAccounts.SubReferenceOne = argEn.SubReferenceOne;
                            loAccounts.CreditRef = argEn.AccountDetailsList[j].ReferenceCode;
                            loAccounts.VoucherNo = argEn.AccountDetailsList[j].ReferenceOne;
                            loAccounts.Category = "STA";
                            loAccounts.TransType = "Debit";
                            loAccounts.SubType = "Student";
                            //loAccounts.TransactionAmount = argEn.AccountDetailsList[j].TransactionAmount;
                            //loAccounts.AccountDetailsList.
                            loAccounts.TempTransCode = GetAutoNumber("CN");
                            loAccounts.TransactionAmount = argEn.AccountDetailsList[j].TempAmount;
                            loAccounts.Description = "Student Pocket Amount";
                            loAccounts.TransStatus = "Open";
                            Insert(loAccounts);
                            //loen.SubReferenceOne = argEn.SubReferenceTwo;
                            //loen.CreditRef = argEn.AccountDetailsList[j].ReferenceCode;
                            //loen.PaidAmount = argEn.AccountDetailsList[j].TransactionAmount;
                            //loen.UpdatedTime = DateTime.Now;
                            //loen.UpdatedBy = "Admin";
                            //UpdatePocketAmounts(loen);
                            loAccounts = null;
                            //loen = null;
                        }
                    }
                }
            }
            return argEn.BatchCode;
        }

        #endregion

        #region InsertJBitBilling

        /// <summary>
        /// Method to Insert Data into Jbit Billing Table
        /// </summary>
        /// <param name="loItem">Batch Invoice is an Input.</param>
        /// <returns>Returns Boolean</returns>
        public bool InsertJBitBilling(AccountsEn loItem)
        {
            bool lbRes = false;
            StudentEn stud = new StudentEn();
            AccountsEn acc = new AccountsEn();
            try
            {
                string sqlString = " SELECT * FROM SAS_STUDENT inner join SAS_Accounts on SASI_MatricNo = CreditRef WHERE sasi_matricno = @MatricNo and BatchCode = @BatchCode";

                if (!FormHelp.IsBlank(sqlString))
                {
                    DbCommand cmdSel = _DatabaseFactory.GetDbCommand(Helper.GetDataBaseType, sqlString, DataBaseConnectionString);
                    _DatabaseFactory.AddInParameter(ref cmdSel, "@MatricNo", DbType.String, loItem.CreditRef);
                    _DatabaseFactory.AddInParameter(ref cmdSel, "@BatchCode", DbType.String, loItem.BatchCode);
                    _DbParameterCollection = cmdSel.Parameters;

                    using (IDataReader loReader = _DatabaseFactory.GetIDataReader(Helper.GetDataBaseType, cmdSel,
                        DataBaseConnectionString, sqlString, _DbParameterCollection).CreateDataReader())
                    {
                        if (loReader != null)
                        {
                            loReader.Read();
                            stud.StudentName = GetValue<string>(loReader, "sasi_name");
                            acc.TransactionID = GetValue<int>(loReader, "TransID");
                        }
                    }
                }

                string sqlCmd = "INSERT INTO jbit_billing(Sb_MatricNo,Sb_Name,Sb_TransID,Sb_TransDate,Sb_TransCode,Sb_Description, " +
                "Sb_Category,Sb_Transtype,Sb_TransAmount,Sb_Flag,Sb_BatchCode) VALUES (@Sb_MatricNo,@Sb_Name,@Sb_TransID,@Sb_TransDate,@Sb_TransCode,@Sb_Description, " +
                "@Sb_Category,@Sb_Transtype,@Sb_TransAmount,'0',@Sb_BatchCode)select @@identity ";

                if (!FormHelp.IsBlank(sqlCmd))
                {
                    DbCommand cmd = _DatabaseFactory.GetDbCommand(Helper.GetDataBaseType, sqlCmd, DataBaseConnectionString);
                    _DatabaseFactory.AddInParameter(ref cmd, "@Sb_MatricNo", DbType.String, loItem.CreditRef);
                    _DatabaseFactory.AddInParameter(ref cmd, "@Sb_Name", DbType.String, stud.StudentName);
                    _DatabaseFactory.AddInParameter(ref cmd, "@Sb_TransID", DbType.Int32, acc.TransactionID);
                    _DatabaseFactory.AddInParameter(ref cmd, "@Sb_TransDate", DbType.DateTime, loItem.TransDate);
                    _DatabaseFactory.AddInParameter(ref cmd, "@Sb_TransCode", DbType.String, loItem.TransactionCode);
                    _DatabaseFactory.AddInParameter(ref cmd, "@Sb_Description", DbType.String, loItem.Description);
                    _DatabaseFactory.AddInParameter(ref cmd, "@Sb_Category", DbType.String, loItem.Category);
                    _DatabaseFactory.AddInParameter(ref cmd, "@Sb_Transtype", DbType.String, loItem.TransType);
                    _DatabaseFactory.AddInParameter(ref cmd, "@Sb_TransAmount", DbType.Double, loItem.TransAmount);
                    _DatabaseFactory.AddInParameter(ref cmd, "@Sb_BatchCode", DbType.String, loItem.BatchCode);
                    _DbParameterCollection = cmd.Parameters;

                    int liRowAffected = _DatabaseFactory.ExecuteNonQuery(Helper.GetDataBaseType, cmd,
                        DataBaseConnectionString, sqlCmd, _DbParameterCollection);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return lbRes;
        }

        #endregion

        #region StudentBatchUpdate

        /// <summary>
        /// Method to Update StudentBatch
        /// </summary>
        /// <param name="argEn">Accounts Entity is the Input</param>
        /// <param name="argList">List of Student Entity is the Input</param>
        /// <returns>Returns BatchCode</returns>
        public string StudentBatchUpdate(AccountsEn argEn, List<StudentEn> argList, bool isAutoDetails = false)
        {
            int i = 0;
            BatchDelete(argEn);
            if (isAutoDetails)
            {
                List<string> getStudent = argList.Select(p => p.MatricNo).Distinct().ToList();

                foreach (var stu in getStudent)
                {
                    List<StudentEn> newArgList = new List<StudentEn>();
                    newArgList = argList.Where(p => p.MatricNo == stu).ToList();
                    List<AccountsDetailsEn> newlistAccDetails = new List<AccountsDetailsEn>();
                    foreach (var data in newArgList)
                    {
                        AccountsDetailsEn newAccDetails = new AccountsDetailsEn();
                        newAccDetails.TransactionAmount = data.TransactionAmount;
                        newAccDetails.ReferenceCode = data.ReferenceCode;
                        newAccDetails.TaxAmount = data.TaxAmount;
                        newAccDetails.PostStatus = argEn.PostStatus;
                        newAccDetails.TransStatus = argEn.TransStatus;
                        newAccDetails.Priority = data.Priority;
                        newAccDetails.Internal_Use = data.Internal_Use;
                        newlistAccDetails.Add(newAccDetails);
                    }
                    argEn.CreditRef = stu;// argList[i].MatricNo;
                    argEn.AccountDetailsList = newlistAccDetails;
                    try
                    {
                        //Insert Receipt Details - Start                    
                        InsertReceiptDetails(argList[i].MatricNo, argList[i].ICNo, argEn.SubReferenceOne, argList[i].TransDate, argList[i].TransactionAmount);
                        //Insert Receipt Details - End
                    }
                    catch (Exception ex)
                    {
                        MaxModule.Helper.LogError(ex.Message);
                        throw ex;
                    }
                    Insert(argEn);
                    //Updating outstanding amount
                }
            }
            else
            {
                for (i = 0; i < argList.Count; i++)
                {
                    if (argEn.Category == "Receipt")
                    {
                        argEn.TransactionAmount = argList[i].TransactionAmount;
                        argEn.AccountDetailsList = argList[i].AccountDetailsList;
                        argEn.PaidAmount = argList[i].PaidAmount;
                        argEn.StManual = argList[i].SubReferenceTwo;
                    }

                    if (argEn.Category == "Payment")
                    {
                        argEn.CreditRef = argList[i].TransactionCode;
                    }

                    else
                    {
                        argEn.CreditRef = argList[i].MatricNo;

                    }
                    if (argEn.Category == "Refund")
                    {
                        argEn.TransactionAmount = argList[i].TransactionAmount;
                    }

                    Insert(argEn);
                }
                //Inserting batch invoice into jbit_billing
                //InsertJBitBilling(argEn);

                //inserting Sponsor Pocket Amount & Sponsor Allocation Amount into Accounts table
                if (argEn.Category == "Payment" && argEn.PostStatus == "Posted")
                {
                    AccountsEn loAccounts;
                    AccountsEn loen;
                    List<AccountsDetailsEn> loAccDet = new List<AccountsDetailsEn>();

                    int j = 0;
                    for (j = 0; j < argEn.AccountDetailsList.Count; j++)
                    {
                        //loAccounts = new AccountsEn();
                        loAccounts = new AccountsEn();
                        loen = new AccountsEn();

                        loAccounts.TransStatus = "Closed";
                        loAccounts.PostStatus = "Posted";
                        loAccounts.PostedDateTime = DateTime.Now;
                        loAccounts.UpdatedTime = DateTime.Now;
                        loAccounts.DueDate = DateTime.Now;
                        loAccounts.CreatedDateTime = DateTime.Now;
                        loAccounts.TransDate = argEn.TransDate;
                        loAccounts.ChequeDate = argEn.ChequeDate;
                        loAccounts.BatchDate = argEn.BatchDate;
                        loAccounts.BatchCode = argEn.BatchCode;
                        loAccounts.BankCode = argEn.BankCode;
                        loAccounts.PaymentMode = argEn.PaymentMode;
                        loAccounts.CreditRefOne = argEn.TransactionCode;
                        loAccounts.SubReferenceOne = argEn.SubReferenceOne;
                        loAccounts.CreditRef = argEn.AccountDetailsList[j].ReferenceCode;
                        loAccounts.VoucherNo = argEn.AccountDetailsList[j].ReferenceOne;
                        loAccounts.Category = "STA";
                        loAccounts.TransType = "Credit";
                        loAccounts.SubType = "Student";
                        loAccounts.TransactionAmount = argEn.AccountDetailsList[j].TransactionAmount;
                        //loAccounts.AccountDetailsList.
                        loAccounts.TransactionCode = GetAutoNumber("CN");

                        loAccounts.Description = "Sponsor Pocket Amount";
                        loAccounts.TransStatus = "Closed";
                        Insert(loAccounts);
                        loen.SubReferenceOne = argEn.SubReferenceTwo;
                        loen.CreditRef = argEn.AccountDetailsList[j].ReferenceCode;
                        loen.PaidAmount = argEn.AccountDetailsList[j].TransactionAmount;
                        loen.UpdatedTime = DateTime.Now;
                        loen.UpdatedBy = "Admin";
                        UpdatePocketAmounts(loen);
                        loAccounts = null;
                        loen = null;
                    }
                }
            }
            return argEn.BatchCode;
        }

        #endregion

        #region InsertHeaderNoDetails
        //Added by Hafiz Roslan
        //Date: 11/01/2016
        //Insert/update headerno
        public bool InsertHeaderNoDetails(AccountsEn argEn, String headerNo)
        {
            bool flag = false;
            string sqlCmd = "";

            try
            {
                sqlCmd = "UPDATE sas_clicks_receiptdetails ";
                sqlCmd += " SET header_no = '" + headerNo + "'";
                sqlCmd += " WHERE receipt_no = @reportNo ";

                if (!FormHelp.IsBlank(sqlCmd))
                {
                    DbCommand cmd = _DatabaseFactory.GetDbCommand(Helper.GetDataBaseType, sqlCmd, DataBaseConnectionString);
                    _DatabaseFactory.AddInParameter(ref cmd, "@reportNo", DbType.String, argEn.SubReferenceOne);
                    _DbParameterCollection = cmd.Parameters;

                    int liRowAffected = _DatabaseFactory.ExecuteNonQuery(Helper.GetDataBaseType, cmd,
                                    DataBaseConnectionString, sqlCmd, _DbParameterCollection);

                    if (liRowAffected > -1)
                        flag = true;
                    else
                        throw new Exception("Update Failed! No Row has been updated...");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return flag;
        }
        #endregion

        #region SponsorBatchInsert

        /// <summary>
        /// Method to Insert SponsorBatch
        /// </summary>
        /// <param name="argEn">Accounts Entity is the Input</param>
        /// <param name="argList">List of Sponsor Entity is the Input</param>
        /// <returns>Returns BatchCode</returns>

        public string SponsorBatchInsert(AccountsEn argEn, List<SponsorEn> argList)
        {
            int i = 0;
            double StuAllAmt = 0.00;
            StuAllAmt = argEn.AllocatedAmount;
            argEn.BatchCode = GetAutoNumber("Batch");
            for (i = 0; i < argList.Count; i++)
            {
                argEn.CreditRef = argList[i].SponserCode;
                Insert(argEn);
               
            }
            argEn.AllocatedAmount=StuAllAmt ;
            //update receipt sponser details allocate amount
            if (argEn.SubType == "Sponsor" && argEn.Category == "Allocation")
            {
                //varaible declaretion
                string UpdateStatement;


                //Build Update Statement - Start
                UpdateStatement = "UPDATE sas_accounts SET allocateamount = '" + argEn.AllocatedAmount + "' WHERE batchcode = '" + argEn.CreditRefOne + "'";
                //Build Update Statement - Stop

                //loop thro the batch details - stop

                //if update statement successful - Start
                if (!FormHelp.IsBlank(UpdateStatement))
                {
                    if (_DatabaseFactory.ExecuteSqlStatement(Helper.GetDataBaseType,
                        DataBaseConnectionString, UpdateStatement) > -1)
                    {

                    }
                }
                //if update statement successful - Stop
            }
            //added by farid 26022016
            if (argEn.SubType == "Sponsor" && argEn.Category == "Payment")
            {
                //varaible declaretion
                string UpdateStatement;


                //Build Update Statement - Start
                UpdateStatement = "UPDATE sas_accounts SET allocateamount = '" + argEn.PaidAmount + "' WHERE batchcode = '" + argEn.CreditRefOne + "'";
                //Build Update Statement - Stop

                //loop thro the batch details - stop

                //if update statement successful - Start
                if (!FormHelp.IsBlank(UpdateStatement))
                {
                    if (_DatabaseFactory.ExecuteSqlStatement(Helper.GetDataBaseType,
                        DataBaseConnectionString, UpdateStatement) > -1)
                    {

                    }
                }
                //if update statement successful - Stop
            }
            // ended
            return argEn.BatchCode;
        }

        #endregion

        #region StudentLoanReceipt

        public string StudentLoanReceipt(AccountsEn argEn)
        {
            argEn.BatchCode = GetAutoNumber("Batch");
            Insert(argEn);
            return argEn.BatchCode;
        }

        #endregion

        #region StudentLoanReceipt_Update
        /// <summary>
        /// Method to Update Receipt
        /// </summary>
        /// Added by Hafiz Roslan
        /// Date: 06/01/2016
        //public void StudentLoanReceipt_Update(AccountsEn argEn)
        //{
        //    try
        //    {
        //        //Update Account Details - Start
        //        _Update(argEn);
        //        //Update Account Details - Stop
        //    }
        //    catch (Exception ex)
        //    {
        //        MaxModule.Helper.LogError(ex.Message);
        //        throw ex;
        //    }
        //}

        #endregion

        #region SucceedTransaction

        /// <summary>
        /// Method to Insert Succeed Transaction
        /// </summary>
        /// <param name="argEn">Accounts Entity is the Input</param>
        /// <param name="argList">List of Sponsor Entity is the Input</param>
        /// <returns>Returns BatchCode</returns>

        public bool SucceedTransaction(AccountsEn argEn)
        {
            int i = 0;
            bool res = false;
            argEn.AutoNum = GetAutoNumber("SucceedTransaction");
            res = InsertSucceedTransaction(argEn);
            return res;
            //InsertDetails(argEn)
        }

        #endregion

        #region SponsorBatchUpdate

        /// <summary>
        /// Method to Update SponsorBatch
        /// </summary>
        /// <param name="argEn">Accounts Entity is the Input</param>
        /// <param name="argList">List of Sponsor Entity is the Input</param>
        /// <returns>Returns BatchCode</returns>

        public string SponsorBatchUpdate(AccountsEn argEn, List<SponsorEn> argList)
        {
            int i = 0;
            BatchDelete(argEn);
            for (i = 0; i < argList.Count; i++)
            {
                argEn.CreditRef = argList[i].SponserCode;
                Insert(argEn);
            }
            //inserting Sponsor Pocket Amount & Sponsor Allocation Amount into Accounts table
            //if (argEn.Category == "Allocation" && argEn.PostStatus == "Posted")
            if (argEn.Category == "Allocation" && argEn.PostStatus == "Ready" && argEn.TransStatus == "Closed")
            {
                AccountsEn loAccounts;

                List<AccountsDetailsEn> loAccDet = new List<AccountsDetailsEn>();

                int j = 0;
                for (j = 0; j < argEn.AccountDetailsList.Count; j++)
                {
                    //loAccounts = new AccountsEn();
                    loAccounts = new AccountsEn();

                    loAccounts.TransStatus = "Open";
                    //loAccounts.PostStatus = "Posted";
                    //loAccounts.TransStatus = "Closed";
                    loAccounts.PostStatus = "Ready";
                    loAccounts.PostedDateTime = DateTime.Now;
                    loAccounts.UpdatedTime = DateTime.Now;
                    loAccounts.DueDate = DateTime.Now;
                    loAccounts.CreatedDateTime = DateTime.Now;
                    loAccounts.TransDate = argEn.TransDate;
                    loAccounts.ChequeDate = argEn.ChequeDate;
                    loAccounts.BatchDate = argEn.BatchDate;
                    loAccounts.BatchCode = argEn.BatchCode;
                    loAccounts.BankCode = argEn.BankCode;
                    loAccounts.PaymentMode = argEn.PaymentMode;
                    loAccounts.CreditRefOne = argEn.CreditRef;

                    loAccounts.CreditRef = argEn.AccountDetailsList[j].ReferenceCode;
                    loAccounts.Category = "SPA";
                    loAccounts.TransType = "Credit";
                    loAccounts.SubType = "Student";
                    loAccounts.TransactionAmount = argEn.AccountDetailsList[j].TransactionAmount;
                    //loAccounts.AccountDetailsList.

                    //loAccounts.TransactionCode = GetAutoNumber("DN");
                    loAccounts.TempTransCode = GetAutoNumber("SA");

                    //Inserting Allocated Amount
                    loAccounts.Description = "Sponsor Allocation Amount";
                    Insert(loAccounts);

                    //Inserting Pocket Money                    

                    loAccounts.TransactionAmount = argEn.AccountDetailsList[j].TempAmount;
                    loAccounts.TempTransCode = GetAutoNumber("SA");
                    loAccounts.Description = "Sponsor Pocket Amount";

                    Insert(loAccounts);
                    // If subtype is student then update the outstanding - by jk
                    loAccounts = null;
                }
            }
            return argEn.BatchCode;

        }

        #endregion

        #region Insert

        /// <summary>
        /// Method to Insert Accounts
        /// </summary>
        /// <param name="argEn">Accounts Entity is the Input</param>
        /// <returns>Returns Boolean</returns>
        /// Modified by Hafiz @ 23/3/2016 - add outstandingamt to sas_accounts
        /// Modified by Hafiz @ 07/6/2016 - add Control Amount to sas_accounts
        public bool Insert(AccountsEn argEn)
        {
            bool lbRes = false;
            string sqlCmd;
            double totalAll = 0.0;
            List<AccountsEn> StuTransList = new List<AccountsEn>();
            try
            {
                //string lsError = String.Empty;
                sqlCmd = "INSERT INTO SAS_Accounts(TransTempCode,TransCode,CreditRef,CreditRef1,DebitRef,DebitRef1,Category," +
                "SubCategory,TransType,SubType,SourceType,TransDate,DueDate,BatchCode,BatchIntake,BatchDate,CrRef1,CrRef2," +
                "Description,Currency,BatchTotal,Tax,Discount,TaxAmount,DiscountAmount,TransAmount,PaidAmount,TransStatus," +
                "TempAmount,TempPaidAmount,PaymentMode,BankCode,PayeeName,ChequeDate,ChequeNo,VoucherNo,PocketAmount,SubRef1," +
                "SubRef2,SubRef3,PostStatus,IntStatus,CreatedBy,CreatedTimeStamp,PostedBy,PostedTimeStamp,IntCode,GLCode," +
                "UpdatedBy,UpdatedTime,KodUniversiti,KumpulanPelajar,TarikhProses,KodBank,ReceiptDate,bankrecno,allocateamount," +
                "outstandingamt,control_amt) VALUES (@TransTempCode,@TransCode,@CreditRef,@CreditRef1,@DebitRef,@DebitRef1," +
                "@Category,@SubCategory,@TransType,@SubType,@SourceType,@TransDate,@DueDate,@BatchCode,@BatchIntake,@BatchDate," +
                "@CrRef1,@CrRef2,@Description,@Currency,@BatchTotal,@Tax,@Discount,@TaxAmount,@DiscountAmount,@TransAmount," +
                "@PaidAmount,@TransStatus,@TempAmount,@TempPaidAmount,@PaymentMode,@BankCode,@PayeeName,@ChequeDate,@ChequeNo," +
                "@VoucherNo,@PocketAmount,@SubRef1,@SubRef2,@SubRef3,@PostStatus,@IntStatus,@CreatedBy,@CreatedDateTime," +
                "@PostedBy,@PostedDateTime,@IntCode,@GLCode,@UpdatedBy,@UpdatedTime,@KodUniversiti,@KumpulanPelajar,@TarikhProses," +
                "@KodBank,@ReceiptDate,@bankrecno,@allocateamount,@outstandingamt,@control_amt);" +
                "select max(transid) from sas_accounts;";

                //select @@identity";

                if (!FormHelp.IsBlank(sqlCmd))
                {

                    if (argEn.PostStatus == "Ready")
                    {
                        #region Posting Status - Ready

                        if (argEn.Category == "Payment")
                        {
                            argEn.TempTransCode = GetAutoNumber("TSPT");
                        }
                        else if (argEn.Category == "Invoice")
                        {
                            argEn.TempTransCode = GetAutoNumber("TIn");
                        }
                        else if (argEn.Category == "Refund")
                        {
                            argEn.TempTransCode = GetAutoNumber("RD");
                        }
                        else if (argEn.Category == "Credit Note")
                        {
                            argEn.TempTransCode = GetAutoNumber("TCN");
                        }
                        else if (argEn.Category == "Debit Note")
                        {
                            argEn.TempTransCode = GetAutoNumber("TDN");
                        }
                        else if (argEn.Category == "Receipt")
                        {
                            argEn.AllocatedAmount = argEn.TransactionAmount;
                            argEn.TempTransCode = GetAutoNumber("TRT");
                        }
                        else if (argEn.Category == "Allocation")
                        {
                            argEn.AllocatedAmount = 0.00;
                            argEn.TempTransCode = GetAutoNumber("TSA");
                        }
                        else if (argEn.Category == "SPA")
                        {
                            //edit by farid on 240216
                            argEn.TempTransCode = GetAutoNumber("SA");
                        }
                        //tambah by farid on 250216
                        else if (argEn.Category == "STA")
                        {
                            //edit by farid on 240216
                            argEn.TempTransCode = GetAutoNumber("CN");
                        }
                        else if (argEn.Category == "Pre-Allocation")
                        {
                            argEn.TempTransCode = GetAutoNumber("Pre-Allocation");
                        }
                        else if (argEn.Category == "AFC")
                        {
                            argEn.TempTransCode = GetAutoNumber("TAFC");
                        }
                        else if (argEn.Category == "CIMBCLICKS")
                        {
                            argEn.TempTransCode = "T" + argEn.SubReferenceOne;
                            argEn.Category = "Receipt";
                        }

                        #endregion
                    }
                    else if (argEn.PostStatus == "Posted")
                    {
                        #region Posting Status - Posted

                        if (argEn.Category == "Payment")
                        {
                            argEn.TransactionCode = GetAutoNumber("SP");
                        }
                        else if (argEn.Category == "Invoice")
                        {
                            argEn.TransactionCode = GetAutoNumber("Inv");
                        }
                        else if (argEn.Category == "Refund")
                        {
                            argEn.TransactionCode = GetAutoNumber("RD");
                        }
                        else if (argEn.Category == "Credit Note")
                        {
                            if (argEn.SubType == "Sponsor")
                            {
                                argEn.TransactionCode = GetAutoNumber("SC");
                            }
                            else
                            {
                                listFirstAccDetails = new List<AccountsDetailsEn>();

                                argEn.TransactionCode = GetAutoNumber("CN");
                                listFirstAccDetails = GetAutoAllocationList(argEn);
                                argEn.SubReferenceTwo = "Auto";
                                AccountsEn StuTrans;

                                int k = 0;
                                int z = 0;
                                double amount = 0.0;
                                double total = 0.0;
                                for (k = 0; k < listFirstAccDetails.Count; k++)
                                {
                                    StuTrans = new AccountsEn();
                                    StuTrans.CreditRefOne = listFirstAccDetails[k].ReferenceCode;
                                    StuTrans.PaidAmount = listFirstAccDetails[k].PaidAmount;
                                    StuTrans.UpdatedBy = argEn.UpdatedBy;
                                    StuTrans.UpdatedTime = DateTime.Now;
                                    amount = listFirstAccDetails[k].PaidAmount;
                                    total += amount;
                                    StuTransList.Add(StuTrans);
                                    argEn.PaidAmount = total;
                                }

                                if (argEn.TransactionAmount == total)
                                {
                                    argEn.TransStatus = "Close";
                                }
                            }
                        }
                        else if (argEn.Category == "Debit Note")
                        {
                            if (argEn.SubType == "Sponsor")
                            {
                                argEn.TransactionCode = GetAutoNumber("SD");
                            }
                            else
                            {
                                argEn.TransactionCode = GetAutoNumber("DN");
                            }
                        }
                        else if (argEn.Category == "Receipt")
                        {
                            argEn.TransactionCode = GetAutoNumber("Rcpt");
                        }
                        else if (argEn.Category == "Allocation")
                        {
                            argEn.TransactionCode = GetAutoNumber("SA");
                        }
                        else if (argEn.Category == "Pre-Allocation")
                        {
                            argEn.TempTransCode = GetAutoNumber("Pre-Allocation");
                        }
                        else if (argEn.Category == "SPA")
                        {
                            argEn.TransactionCode = GetAutoNumber("SA");
                        }
                        //tambah by farid on 250216
                        else if (argEn.Category == "STA")
                        {
                            //tambah by farid on 250216
                            argEn.TransactionCode = GetAutoNumber("CN");
                        }
                        else if (argEn.Category == "AFC")
                        {
                            argEn.TransactionCode = GetAutoNumber("AC");
                        }

                        #endregion
                    }

                    if (argEn.Category == "AFC")
                    {
                        if (argEn.IsHostel == true)
                        {
                            #region Get Hostel Amount

                            //Start Get Amount Hostel Details
                            //string sqlCmdHostel = "select SAHA_Amount from SAS_Student SS,SAS_HostelStruct SH,SAS_HostelStrAmount SA," +
                            //    "SAS_FeeTypes SF where SASI_Hostel = '1' and SASI_MatricNo = @MatricNo and (SS.SAKO_Code = SH.SAHB_Code and SS.SABK_Code = SH.SAHB_Block " +
                            //    "and SS.SART_Code = SH.SAHB_RoomTYpe) and SH.SAHS_Code = SA.SAHS_Code and SS.SASI_FeeCat = SA.SASC_Code and SF.SAFT_Code = SA.SAFT_Code and  sasi_hostel = '1'";
                            //string sqlCmdHostel = "select SAHA_Amount from SAS_Student SS,SAS_HostelStruct SH,SAS_HostelStrAmount SA," +
                            //    "SAS_FeeTypes SF where SASI_Hostel = true and SASI_MatricNo = @MatricNo and (SS.SAKO_Code = SH.SAHB_Code and SS.SABK_Code = SH.SAHB_Block) " +
                            //    "and SH.SAHS_Code = SA.SAHS_Code and SS.SASI_FeeCat = SA.SASC_Code and SF.SAFT_Code = SA.SAFT_Code and sasi_hostel = true";
                            string sqlCmdHostel = "select SUM(SAHA_Amount) AS SAHA_Amount from SAS_Student SS,SAS_HostelStruct SH,SAS_HostelStrAmount SA, " +
                                "SAS_FeeTypes SF where SASI_Hostel = true and SASI_MatricNo = @MatricNo  and (SS.SAKO_Code = SH.SAHB_Code and SS.SABK_Code = SH.SAHB_Block) " +
                                "and SH.SAHS_Code = SA.SAHS_Code and SS.SASI_FeeCat = SA.SASC_Code and SF.SAFT_Code = SA.SAFT_Code and  sasi_hostel = true";

                            try
                            {
                                if (!FormHelp.IsBlank(sqlCmdHostel))
                                {
                                    DbCommand cmdHostel = _DatabaseFactory.GetDbCommand(Helper.GetDataBaseType, sqlCmd, DataBaseConnectionString);
                                    //_DatabaseFactory.AddInParameter(ref cmdHostel, "@MatricNo", DbType.String, argEn.MatricNo);
                                    _DatabaseFactory.AddInParameter(ref cmdHostel, "@MatricNo", DbType.String, argEn.CreditRef);
                                    _DbParameterCollection = cmdHostel.Parameters;

                                    //using (IDataReader loReaderHostel = _DatabaseFactory.GetIDataReader(Helper.GetDataBaseType, cmdHostel,
                                    //    DataBaseConnectionString, sqlCmd, _DbParameterCollection).CreateDataReader())
                                    using (IDataReader loReaderHostel = _DatabaseFactory.GetIDataReader(Helper.GetDataBaseType, cmdHostel,
                                        DataBaseConnectionString, sqlCmdHostel, _DbParameterCollection).CreateDataReader())
                                    {
                                        //loReaderHostel.Read();
                                        while (loReaderHostel.Read())
                                        {
                                            //if (loReaderHostel != null)
                                            //if (GetValue<double>(loReaderHostel, "SAHA_Amount") != 0)
                                            if (loReaderHostel.RecordsAffected != -1)
                                            {
                                                AFCEn loHostel = new AFCEn();
                                                loHostel.TransAmount = GetValue<double>(loReaderHostel, "SAHA_Amount");
                                                totalAll = loHostel.TransAmount;
                                            }
                                            else
                                                throw new Exception("No Hostel Fee for Student");
                                            //loReaderHostel.Close();
                                        }
                                        loReaderHostel.Close();
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                MaxModule.Helper.LogError(ex.Message);
                                throw ex;
                            }

                            #endregion
                        }

                        if (argEn.IsKoko != "-1")
                        {
                            #region Get Koko Amount

                            //string sqlCmdKoko = "select SK.SAKO_Code As SAKO_Code,SK.SAKO_Description as SAKO_Description,(SKD.SAKOD_FeeAmount * SK.SAKO_CreditHours) as SAHA_AMOUNT from " +
                            //"SAS_Student SS,SAS_Kokorikulum SK,SAS_KokorikulumDetails SKD where SASI_MatricNo = @MatricNo and SASI_KokoCode <> '-1' " +
                            //"and SS.SASI_FeeCat = SKD.SAKOD_CategoryCode and SS.SASI_KokoCode = SK.SAKO_Code and SKD.SAKO_Code = SK.SAKO_Code";
                            string sqlCmdKoko = "select SK.SAKO_Code As SAKO_Code,SK.SAKO_Description as SAKO_Description, " +
                                    "case when (select sasi_hostel from sas_student where sasi_matricno = @MatricNo) = true then sum (SKD.sakod_feeamount) " +
                                    "else sum (SKD.sakod_feeamountout) end as SAHA_AMOUNT from SAS_Kokorikulum SK,SAS_KokorikulumDetails SKD " +
                                    "where (select sasi_kokocode from sas_student where sasi_matricno = @MatricNo) <> '-1' " +
                                    "and SKD.SAKO_Code = SK.SAKO_Code and SKD.sako_code = (select sasi_kokocode from sas_student where sasi_matricno = @MatricNo) " +
                                    "and SKD.sakod_categorycode = (select sasc_code from sas_student where sasi_matricno = @MatricNo) " +
                                    "group by SK.SAKO_Code,SK.SAKO_Description";

                            try
                            {

                                if (!FormHelp.IsBlank(sqlCmdKoko))
                                {
                                    DbCommand cmdKoko = _DatabaseFactory.GetDbCommand(Helper.GetDataBaseType, sqlCmd, DataBaseConnectionString);
                                    //_DatabaseFactory.AddInParameter(ref cmdKoko, "@MatricNo", DbType.String, argEn.MatricNo);
                                    _DatabaseFactory.AddInParameter(ref cmdKoko, "@MatricNo", DbType.String, argEn.CreditRef);
                                    _DbParameterCollection = cmdKoko.Parameters;


                                    using (IDataReader loReaderKoko = _DatabaseFactory.GetIDataReader(Helper.GetDataBaseType, cmdKoko,
                                            DataBaseConnectionString, sqlCmdKoko, _DbParameterCollection).CreateDataReader())
                                    {
                                        while (loReaderKoko.Read())
                                        {
                                            //loReaderKoko.Read();
                                            //if (loReaderKoko != null)
                                            if (loReaderKoko.RecordsAffected != -1)
                                            {
                                                AFCEn loKoko = new AFCEn();
                                                loKoko.TransAmount = GetValue<double>(loReaderKoko, "SAHA_Amount");
                                                totalAll += loKoko.TransAmount;
                                            }
                                            else
                                                throw new Exception("No Kokorikulum Fee for Student");
                                            //loReaderKoko.Close();
                                        }
                                        loReaderKoko.Close();
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                MaxModule.Helper.LogError(ex.Message);
                                throw ex;
                            }

                            #endregion
                        }
                    }

                    //Added by Hafiz Roslan
                    //Date: 31/12/2015
                    //if (String.IsNullOrEmpty(argEn.BankSlipID) == false)
                    //{
                    //    CheckDuplicateBankSlipNo(argEn);
                    //}

                    #region Command Params

                    DbCommand cmd = _DatabaseFactory.GetDbCommand(Helper.GetDataBaseType, sqlCmd, DataBaseConnectionString);
                    _DatabaseFactory.AddInParameter(ref cmd, "@TransTempCode", DbType.String, argEn.TempTransCode);
                    _DatabaseFactory.AddInParameter(ref cmd, "@TransCode", DbType.String, argEn.TransactionCode);
                    _DatabaseFactory.AddInParameter(ref cmd, "@CreditRef", DbType.String, argEn.CreditRef);
                    _DatabaseFactory.AddInParameter(ref cmd, "@CreditRef1", DbType.String, argEn.CreditRefOne);
                    _DatabaseFactory.AddInParameter(ref cmd, "@DebitRef", DbType.String, argEn.DebitRef);
                    _DatabaseFactory.AddInParameter(ref cmd, "@DebitRef1", DbType.String, argEn.DebitRefOne);
                    _DatabaseFactory.AddInParameter(ref cmd, "@Category", DbType.String, argEn.Category);
                    _DatabaseFactory.AddInParameter(ref cmd, "@SubCategory", DbType.String, argEn.SubCategory);
                    _DatabaseFactory.AddInParameter(ref cmd, "@TransType", DbType.String, argEn.TransType);
                    _DatabaseFactory.AddInParameter(ref cmd, "@SubType", DbType.String, argEn.SubType);
                    _DatabaseFactory.AddInParameter(ref cmd, "@SourceType", DbType.String, argEn.SourceType);
                    _DatabaseFactory.AddInParameter(ref cmd, "@TransDate", DbType.DateTime, Helper.DateConversion(argEn.TransDate));
                    _DatabaseFactory.AddInParameter(ref cmd, "@DueDate", DbType.DateTime, Helper.DateConversion(argEn.DueDate));
                    _DatabaseFactory.AddInParameter(ref cmd, "@BatchCode", DbType.String, argEn.BatchCode);
                    _DatabaseFactory.AddInParameter(ref cmd, "@BatchDate", DbType.DateTime, Helper.DateConversion(argEn.BatchDate));
                    _DatabaseFactory.AddInParameter(ref cmd, "@CrRef1", DbType.String, argEn.CrRefOne);
                    _DatabaseFactory.AddInParameter(ref cmd, "@CrRef2", DbType.String, argEn.CrRefTwo);
                    _DatabaseFactory.AddInParameter(ref cmd, "@Description", DbType.String, argEn.Description);
                    _DatabaseFactory.AddInParameter(ref cmd, "@Currency", DbType.String, argEn.CurrencyUsed);
                    _DatabaseFactory.AddInParameter(ref cmd, "@BatchTotal", DbType.Double, argEn.BatchTotal);
                    _DatabaseFactory.AddInParameter(ref cmd, "@Tax", DbType.Double, argEn.TaxPercentage);
                    _DatabaseFactory.AddInParameter(ref cmd, "@Discount", DbType.Double, argEn.DiscountPercentage);
                    _DatabaseFactory.AddInParameter(ref cmd, "@TaxAmount", DbType.Double, argEn.TaxAmount);
                    _DatabaseFactory.AddInParameter(ref cmd, "@DiscountAmount", DbType.Double, argEn.DiscountAmount);
                    _DatabaseFactory.AddInParameter(ref cmd, "@TransAmount", DbType.Double, argEn.TransactionAmount + totalAll);
                    _DatabaseFactory.AddInParameter(ref cmd, "@PaidAmount", DbType.Double, argEn.PaidAmount);
                    _DatabaseFactory.AddInParameter(ref cmd, "@TransStatus", DbType.String, argEn.TransStatus);
                    _DatabaseFactory.AddInParameter(ref cmd, "@TempAmount", DbType.Double, argEn.TempAmount + totalAll);
                    _DatabaseFactory.AddInParameter(ref cmd, "@TempPaidAmount", DbType.Double, argEn.TempPaidAmount);
                    _DatabaseFactory.AddInParameter(ref cmd, "@PaymentMode", DbType.String, argEn.PaymentMode);
                    _DatabaseFactory.AddInParameter(ref cmd, "@BankCode", DbType.String, argEn.BankCode);
                    _DatabaseFactory.AddInParameter(ref cmd, "@PayeeName", DbType.String, argEn.PayeeName);
                    _DatabaseFactory.AddInParameter(ref cmd, "@ChequeDate", DbType.DateTime, Helper.DateConversion(argEn.ChequeDate));
                    _DatabaseFactory.AddInParameter(ref cmd, "@ChequeNo", DbType.String, argEn.ChequeNo);
                    _DatabaseFactory.AddInParameter(ref cmd, "@VoucherNo", DbType.String, argEn.VoucherNo);
                    _DatabaseFactory.AddInParameter(ref cmd, "@PocketAmount", DbType.String, argEn.PocketAmount);
                    _DatabaseFactory.AddInParameter(ref cmd, "@SubRef1", DbType.String, argEn.SubReferenceOne);
                    _DatabaseFactory.AddInParameter(ref cmd, "@SubRef2", DbType.String, argEn.SubReferenceTwo);
                    _DatabaseFactory.AddInParameter(ref cmd, "@SubRef3", DbType.String, argEn.SubReferenceThree);
                    _DatabaseFactory.AddInParameter(ref cmd, "@PostStatus", DbType.String, argEn.PostStatus);
                    _DatabaseFactory.AddInParameter(ref cmd, "@IntStatus", DbType.Int32, argEn.IntegrationStatus);
                    _DatabaseFactory.AddInParameter(ref cmd, "@CreatedBy", DbType.String, argEn.CreatedBy);
                    _DatabaseFactory.AddInParameter(ref cmd, "@CreatedDateTime", DbType.DateTime, Helper.DateConversion(argEn.CreatedDateTime));
                    _DatabaseFactory.AddInParameter(ref cmd, "@PostedBy", DbType.String, argEn.PostedBy);
                    _DatabaseFactory.AddInParameter(ref cmd, "@PostedDateTime", DbType.DateTime, Helper.DateConversion(argEn.PostedDateTime));
                    _DatabaseFactory.AddInParameter(ref cmd, "@IntCode", DbType.String, argEn.IntegrationCode);
                    _DatabaseFactory.AddInParameter(ref cmd, "@GLCode", DbType.String, argEn.GLCode);
                    _DatabaseFactory.AddInParameter(ref cmd, "@UpdatedBy", DbType.String, argEn.UpdatedBy);
                    _DatabaseFactory.AddInParameter(ref cmd, "@UpdatedTime", DbType.DateTime, Helper.DateConversion(argEn.UpdatedTime));
                    _DatabaseFactory.AddInParameter(ref cmd, "@BatchIntake", DbType.String, argEn.BatchIntake);
                    _DatabaseFactory.AddInParameter(ref cmd, "@KodUniversiti", DbType.String, argEn.KodUniversiti);
                    _DatabaseFactory.AddInParameter(ref cmd, "@KumpulanPelajar", DbType.String, argEn.KumpulanPelajar);
                    _DatabaseFactory.AddInParameter(ref cmd, "@TarikhProses", DbType.String, argEn.TarikhProses);
                    _DatabaseFactory.AddInParameter(ref cmd, "@KodBank", DbType.String, argEn.KodBank);
                    //_DatabaseFactory.AddInParameter(ref cmd, "@ReceiptNo", DbType.String, argEn.ReceiptNo);
                    _DatabaseFactory.AddInParameter(ref cmd, "@ReceiptDate", DbType.DateTime, Helper.DateConversion(argEn.ReceiptDate));
                    _DatabaseFactory.AddInParameter(ref cmd, "@bankrecno", DbType.String, argEn.BankSlipID);
                    _DatabaseFactory.AddInParameter(ref cmd, "@allocateamount", DbType.Double, argEn.AllocatedAmount);
                    _DatabaseFactory.AddInParameter(ref cmd, "@outstandingamt", DbType.String, argEn.Outstanding_Amount);
                    _DatabaseFactory.AddInParameter(ref cmd, "@control_amt", DbType.Decimal, clsGeneric.NullToDecimal(argEn.ControlAmt));
                    _DbParameterCollection = cmd.Parameters;

                    #endregion

                    int Result = clsGeneric.NullToInteger(_DatabaseFactory.ExecuteScalarCommand(Helper.GetDataBaseType, cmd,
                            DataBaseConnectionString, sqlCmd, _DbParameterCollection));

                    if (Result > 0)
                    {

                        #region Insert Log Auto Number

                        try
                        {
                            string sqlCmdlog = "INSERT INTO SAS_LogAutoNumber(BatchNo,TransactionNo,Status,Category,CreatedBy,Createdon) " +
                            "VALUES (@BatchNo,@TransactionNo,@Status,@Category,@CreatedBy,@Createdon)";

                            if (!FormHelp.IsBlank(sqlCmdlog))
                            {
                                DbCommand cmdlog = _DatabaseFactory.GetDbCommand(Helper.GetDataBaseType, sqlCmdlog, DataBaseConnectionString);
                                _DatabaseFactory.AddInParameter(ref cmdlog, "@BatchNo", DbType.String, argEn.BatchCode);
                                _DatabaseFactory.AddInParameter(ref cmdlog, "@Status", DbType.String, argEn.PostStatus);


                                if (argEn.PostStatus == "Ready")
                                {
                                    _DatabaseFactory.AddInParameter(ref cmdlog, "@TransactionNo", DbType.String, argEn.TempTransCode);
                                }
                                else
                                {
                                    _DatabaseFactory.AddInParameter(ref cmdlog, "@TransactionNo", DbType.String, argEn.TransactionCode);
                                }
                                _DatabaseFactory.AddInParameter(ref cmdlog, "@Category", DbType.String, argEn.Category);
                                if (string.IsNullOrEmpty(argEn.CreatedBy))
                                {
                                    _DatabaseFactory.AddInParameter(ref cmdlog, "@CreatedBy", DbType.String, argEn.UpdatedBy);
                                }
                                else
                                {
                                    _DatabaseFactory.AddInParameter(ref cmdlog, "@CreatedBy", DbType.String, argEn.CreatedBy);
                                }
                                _DatabaseFactory.AddInParameter(ref cmdlog, "@Createdon", DbType.String, DateTime.Now.ToString("yyyy/MM/dd"));
                                _DbParameterCollection = cmdlog.Parameters;

                                int liRowAffected = _DatabaseFactory.ExecuteNonQuery(Helper.GetDataBaseType, cmdlog,
                                    DataBaseConnectionString, sqlCmdlog, _DbParameterCollection);
                            }
                        }
                        catch (Exception ex)
                        {
                            MaxModule.Helper.LogError(ex.Message);
                            throw ex;
                        }

                        #endregion

                        #region Insert Account Details

                        //Inserting AccountDetails Table
                        if (argEn.AccountDetailsList != null)
                        {
                            if (argEn.AccountDetailsList.Count != 0)
                            {
                                AccountsDetailsDAL loDS = new AccountsDetailsDAL();
                                for (int i = 0; i < argEn.AccountDetailsList.Count; i++)
                                {
                                    argEn.AccountDetailsList[i].TransactionID = Result;
                                    argEn.AccountDetailsList[i].TransTempCode = argEn.TempTransCode;
                                    argEn.AccountDetailsList[i].TransactionCode = argEn.TransactionCode;
                                    loDS.Insert(argEn.AccountDetailsList[i]);
                                }
                            }
                        }

                        #endregion

                        if (argEn.PostStatus == "Posted")
                        {
                            if (argEn.Category == "Payment" && argEn.SubType == "Student")
                            {
                                //updating allocation amounts
                                UpdateAllocations(argEn);
                            }
                            if (argEn.Category == "Allocation")
                            {
                                // updating receipt paid amount
                                UpdateReceipts(argEn);
                            }
                            if (argEn.Category == "Payment" && argEn.SubType == "Sponsor")
                            {
                                // updating receipt paid amount
                                UpdateReceipts(argEn);
                            }
                            if (argEn.Category == "Receipt" && argEn.SubType == "Student")
                            {
                                // updating receipt paid amount
                                AccountsEn loen;
                                int j = 0;
                                for (j = 0; j < StuTransList.Count; j++)
                                {
                                    loen = new AccountsEn();
                                    loen = StuTransList[j];
                                    UpdateReceipts(loen);
                                }
                            }

                            #region Credit Note and Student

                            if (argEn.Category == "Credit Note" && argEn.SubType == "Student")
                            {
                                if (listFirstAccDetails != null)
                                {
                                    if (listFirstAccDetails.Count != 0)
                                    {
                                        AccountsDetailsDAL loDS = new AccountsDetailsDAL();
                                        for (int i = 0; i < listFirstAccDetails.Count; i++)
                                        {
                                            bool results;
                                            listFirstAccDetails[i].TransactionID = Result;
                                            listFirstAccDetails[i].TransTempCode = argEn.TempTransCode;
                                            listFirstAccDetails[i].TransactionCode = argEn.TransactionCode;
                                            results = loDS.Insert(listFirstAccDetails[i]);
                                            if (results == true)
                                            {
                                                //trans.Commit();

                                            }
                                            else
                                            {
                                                //trans.Rollback();
                                            }
                                        }
                                    }
                                    listFirstAccDetails = null;
                                }
                                // updating receipt paid amount
                                AccountsEn loen;
                                int j = 0;
                                for (j = 0; j < StuTransList.Count; j++)
                                {
                                    loen = new AccountsEn();
                                    loen = StuTransList[j];
                                    UpdateReceipts(loen);
                                }

                            }

                            #endregion
                        }
                    }
                    else
                    {
                        throw new Exception("Insertion Failed! No Row has been inserted...");
                    }
                }
            }
            catch (Exception ex)
            {
                MaxModule.Helper.LogError(ex.Message);
                throw ex;
            }

            return lbRes;
        }

        #endregion

        #region Check Duplicate Bank Slip No
        //Author: Hafiz Roslan
        //Date  : 31/12/2015
        //??    : Check duplicate and allow ALPHANUMERIC
        public void CheckDuplicateBankSlipNo(String bank_slip_no, String matric_no, String batch_code)
        {
            String bsn = bank_slip_no;

            //used Regex
            if (Regex.IsMatch(bsn, @"^[a-zA-Z]+$"))
            {
                //check for alphabet only

                //popup dialog box for alert
                //System.Windows.Forms.MessageBox.Show("-= Alphabet Detected =- Bank slip number MUST be [Alphanumeric]. ");
                throw new Exception("Bank slip number [" + bsn + "] cannot be all alphabet.");
            }
            else if (Regex.IsMatch(bsn, "^[0-9]+$"))
            {
                //check for numeric only

                //popup dialog box for alert
                //System.Windows.Forms.MessageBox.Show("-= Numeric Detected =- Bank slip number MUST be [Alphanumeric]. ");
                throw new Exception("Bank slip number [" + bsn + "] cannot be all numeric.");
            }
            else
            {
                //true condition - alphanumeric

                //updated by Hafiz Roslan @ 26/1/2016

                StudentEn loItem = new StudentEn();

                string sql1 = " select * from SAS_Accounts WHERE batchcode ='" + batch_code + "' AND creditref = @MatricNo";

                if (!FormHelp.IsBlank(sql1))
                {
                    DbCommand cmdSel = _DatabaseFactory.GetDbCommand(Helper.GetDataBaseType, sql1, DataBaseConnectionString);
                    _DatabaseFactory.AddInParameter(ref cmdSel, "@MatricNo", DbType.String, matric_no);
                    _DbParameterCollection = cmdSel.Parameters;

                    using (IDataReader loReader = _DatabaseFactory.GetIDataReader(Helper.GetDataBaseType, cmdSel,
                        DataBaseConnectionString, sql1, _DbParameterCollection).CreateDataReader())
                    {
                        while (loReader.Read())
                        {
                            loItem.BankSlipID = GetValue<string>(loReader, "bankrecno");
                        }
                        loReader.Close();
                    }
                }

                if (loItem.BankSlipID != null)
                {
                    //data already available in the DB
                    StudentEn loItem2 = new StudentEn();

                    string sqlCmdChkBsn = "select * from SAS_Accounts where bankrecno = @bankrecno";

                    if (!FormHelp.IsBlank(sqlCmdChkBsn))
                    {
                        DbCommand cmdchkbsn = _DatabaseFactory.GetDbCommand(Helper.GetDataBaseType, sqlCmdChkBsn, DataBaseConnectionString);
                        _DatabaseFactory.AddInParameter(ref cmdchkbsn, "@bankrecno", DbType.String, bsn);
                        _DbParameterCollection = cmdchkbsn.Parameters;

                        using (IDataReader loReader = _DatabaseFactory.GetIDataReader(Helper.GetDataBaseType, cmdchkbsn,
                            DataBaseConnectionString, sqlCmdChkBsn, _DbParameterCollection).CreateDataReader())
                        {
                            while (loReader.Read())
                            {
                                loItem2.BankSlipID = GetValue<string>(loReader, "bankrecno");

                                if (loItem.BankSlipID != loItem2.BankSlipID)
                                {
                                    //popup dialog box for alert
                                    //System.Windows.Forms.MessageBox.Show("Error! [Duplicate] Bank Slip Number.");

                                    //throw exception if got data = duplicate 
                                    throw new Exception("Bank slip number [" + bsn + "] already exists.");
                                }
                                else
                                {
                                    //do nothing - just update
                                }

                            }
                            loReader.Close();
                        }
                    }
                }
                else
                {
                    //no data found

                    string sqlCmdChkBsn = "select * from SAS_Accounts where bankrecno = @bankrecno";

                    if (!FormHelp.IsBlank(sqlCmdChkBsn))
                    {
                        DbCommand cmdchkbsn = _DatabaseFactory.GetDbCommand(Helper.GetDataBaseType, sqlCmdChkBsn, DataBaseConnectionString);
                        _DatabaseFactory.AddInParameter(ref cmdchkbsn, "@bankrecno", DbType.String, bsn);
                        _DbParameterCollection = cmdchkbsn.Parameters;

                        using (IDataReader loReader = _DatabaseFactory.GetIDataReader(Helper.GetDataBaseType, cmdchkbsn,
                            DataBaseConnectionString, sqlCmdChkBsn, _DbParameterCollection).CreateDataReader())
                        {
                            while (loReader.Read())
                            {
                                //popup dialog box for alert
                                //System.Windows.Forms.MessageBox.Show("Error! [Duplicate] Bank Slip Number.");

                                //throw exception if got data = duplicate 
                                throw new Exception("Bank slip number [" + bsn + "] already exists.");
                            }
                            loReader.Close();
                        }
                    }

                }
            }
        }

        #endregion
        
        #region InsertSucceedTransaction

        /// <summary>
        /// Method to Insert Succeed Transaction
        /// </summary>
        /// <param name="argEn">Accounts Entity is the Input</param>
        /// <returns>Returns Boolean</returns>
        protected bool InsertSucceedTransaction(AccountsEn argEn)
        {
            bool lbRes = false;
            string sqlCmd;

            try
            {
                //string lsError = String.Empty;
                sqlCmd = "Insert Into SAS_SucceedTransactionHeader(STH_AutoNumber,STH_NoKelompok_H,STH_KodUni,STH_TarikhProses,STH_KumpulanPelajar" +
                ",STH_KodBank,STH_NoKelompok_F,STH_JumlahAmaun,STH_JumlahRekod) Values (@STH_AutoNumber,@STH_NoKelompok_H,@STH_KodUni," +
                "@STH_TarikhProses,@STH_KumpulanPelajar,@STH_KodBank,@STH_NoKelompok_F,@STH_JumlahAmaun,@STH_JumlahRekod)";

                if (!FormHelp.IsBlank(sqlCmd))
                {
                    DbCommand cmd = _DatabaseFactory.GetDbCommand(Helper.GetDataBaseType, sqlCmd, DataBaseConnectionString);
                    _DatabaseFactory.AddInParameter(ref cmd, "@STH_AutoNumber", DbType.String, argEn.AutoNum);
                    _DatabaseFactory.AddInParameter(ref cmd, "@STH_NoKelompok_H", DbType.String, argEn.NoKelompok_H);
                    _DatabaseFactory.AddInParameter(ref cmd, "@STH_KodUni", DbType.String, argEn.KodUniversiti);
                    _DatabaseFactory.AddInParameter(ref cmd, "@STH_TarikhProses", DbType.String, argEn.TarikhProses);
                    _DatabaseFactory.AddInParameter(ref cmd, "@STH_KumpulanPelajar", DbType.String, argEn.KumpulanPelajar);
                    _DatabaseFactory.AddInParameter(ref cmd, "@STH_KodBank", DbType.String, argEn.KodBank);
                    _DatabaseFactory.AddInParameter(ref cmd, "@STH_NoKelompok_F", DbType.String, argEn.NoKelompok_F);
                    _DatabaseFactory.AddInParameter(ref cmd, "@STH_JumlahAmaun", DbType.Double, argEn.JumlahAmaun);
                    _DatabaseFactory.AddInParameter(ref cmd, "@STH_JumlahRekod", DbType.String, argEn.JumlahRekod);
                    _DbParameterCollection = cmd.Parameters;

                    int liRowAffected = _DatabaseFactory.ExecuteNonQuery(Helper.GetDataBaseType, cmd,
                                DataBaseConnectionString, sqlCmd, _DbParameterCollection);

                    if (liRowAffected > -1)
                        lbRes = true;

                    if (argEn.AccountDetailsList != null)
                    {
                        if (argEn.AccountDetailsList.Count != 0)
                        {
                            AccountsDetailsDAL loDS = new AccountsDetailsDAL();
                            for (int i = 0; i < argEn.AccountDetailsList.Count; i++)
                            {
                                bool result = InsertSucceedTransactionDetails(argEn.AccountDetailsList[i], argEn.AutoNum);
                                lbRes = result;
                            }
                        }
                    }
                    else
                        lbRes = false;
                    //throw new Exception("Insertion Failed! No Row has been updated...");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return lbRes;
        }

        #endregion

        #region InsertSucceedTransactionDetails

        /// <summary>
        /// Method to Insert 
        /// </summary>
        /// <param name="argEn">AccountsDetails Entity is an Input.</param>
        /// <returns>Returns Boolean</returns>
        protected bool InsertSucceedTransactionDetails(AccountsDetailsEn argEn, String AutoNum)
        {
            bool lbRes = false;
            string sqlCmd;
            try
            {
                sqlCmd = "Insert Into SAS_SucceedTransactionDetails(STH_AutoNum_H,STD_NoKelompok,STD_KumpulanPelajar," +
                "STD_NoWarran,STD_NoPelajar,STD_NoIC,STD_NamaPelajar,STD_AmaunWarran,STD_AmaunPotongan,STD_NilaiBersih," +
                "STD_TarikhTransaksi,STD_TarikhLupusWarran,STD_NoAkaunPelajar,STD_Filler,STD_StatusBayaran) " +
                "values(@STH_AutoNum_H,@STD_NoKelompok,@STD_KumpulanPelajar,@STD_NoWarran,@STD_NoPelajar,@STD_NoIC," +
                "@STD_NamaPelajar,@STD_AmaunWarran,@STD_AmaunPotongan,@STD_NilaiBersih,@STD_TarikhTransaksi," +
                "@STD_TarikhLupusWarran,@STD_NoAkaunPelajar,@STD_Filler,@STD_StatusBayaran) ";

                if (!FormHelp.IsBlank(sqlCmd))
                {
                    DbCommand cmd = _DatabaseFactory.GetDbCommand(Helper.GetDataBaseType, sqlCmd, DataBaseConnectionString);
                    _DatabaseFactory.AddInParameter(ref cmd, "@STH_AutoNum_H", DbType.String, AutoNum);
                    _DatabaseFactory.AddInParameter(ref cmd, "@STD_NoKelompok", DbType.String, argEn.NoKelompok);
                    _DatabaseFactory.AddInParameter(ref cmd, "@STD_KumpulanPelajar", DbType.String, argEn.KumpulanPelajar);
                    _DatabaseFactory.AddInParameter(ref cmd, "@STD_NoWarran", DbType.String, argEn.NoWarran);
                    _DatabaseFactory.AddInParameter(ref cmd, "@STD_NoPelajar", DbType.String, argEn.NoPelajar);
                    _DatabaseFactory.AddInParameter(ref cmd, "@STD_NoIC", DbType.String, argEn.NoIC);
                    _DatabaseFactory.AddInParameter(ref cmd, "@STD_NamaPelajar", DbType.String, argEn.NamaPelajar);
                    _DatabaseFactory.AddInParameter(ref cmd, "@STD_AmaunWarran", DbType.String, argEn.AmaunWarran);
                    _DatabaseFactory.AddInParameter(ref cmd, "@STD_AmaunPotongan", DbType.String, argEn.AmaunPotongan);
                    _DatabaseFactory.AddInParameter(ref cmd, "@STD_NilaiBersih", DbType.String, argEn.NilaiBersih);
                    _DatabaseFactory.AddInParameter(ref cmd, "@STD_TarikhTransaksi", DbType.String, argEn.TarikhTransaksi);
                    _DatabaseFactory.AddInParameter(ref cmd, "@STD_TarikhLupusWarran", DbType.String, argEn.TarikhLupusWarran);
                    _DatabaseFactory.AddInParameter(ref cmd, "@STD_NoAkaunPelajar", DbType.String, argEn.noAkaun);
                    _DatabaseFactory.AddInParameter(ref cmd, "@STD_Filler", DbType.String, argEn.Filler);
                    _DatabaseFactory.AddInParameter(ref cmd, "@STD_StatusBayaran", DbType.String, argEn.StatusBayaran);
                    _DbParameterCollection = cmd.Parameters;

                    int liRowAffected = _DatabaseFactory.ExecuteNonQuery(Helper.GetDataBaseType, cmd,
                                DataBaseConnectionString, sqlCmd, _DbParameterCollection);

                    if (liRowAffected > -1)
                        lbRes = true;
                    else
                        throw new Exception("Insertion Failed! No Row has been updated...");
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return lbRes;
        }

        #endregion

        #region GetAutoAllocationList

        /// <summary>
        /// Method to Auto Allocate Transactions
        /// </summary>
        /// <param name="argEn">Accounts Entity is the Input.TransactionAmount and CreditRef are Input Properties</param>
        /// <returns>Returns List of AccountDetails Entity.</returns>
        public List<AccountsDetailsEn> GetAutoAllocationList(AccountsEn argEn)
        {
            double TotalAmount = 0.0;
            double CurrentAmount = 0.0;
            AccountsEn loen = new AccountsEn();
            AccountsDetailsEn lodetails;
            List<AccountsEn> lolist = new List<AccountsEn>();
            List<AccountsDetailsEn> lodetaillist = new List<AccountsDetailsEn>();
            TotalAmount = argEn.TransactionAmount;
            loen.CreditRef = argEn.CreditRef;
            loen.SubType = "Student";
            loen.PostStatus = "Posted";
            loen.Category = "'Invoice','Debit Note','AFC'";
            lolist = GetStudentAutoAllocation(loen);

            int i = 0;
            for (i = 0; i < lolist.Count; i++)
            {
                double BalAmt = 0.0;
                double transAmount = 0.0;
                BalAmt = lolist[i].TransactionAmount - lolist[i].PaidAmount;
                if (BalAmt < (TotalAmount - CurrentAmount))
                {
                    lodetails = new AccountsDetailsEn();
                    transAmount = BalAmt;
                    lodetails.TransactionAmount = BalAmt;
                    lodetails.PaidAmount = BalAmt;
                    lodetails.ReferenceCode = lolist[i].TransactionCode;
                    lodetails.ReferenceOne = lolist[i].CreditRef;
                    lodetails.ReferenceTwo = "Auto";
                    lodetaillist.Add(lodetails);
                }
                else
                {
                    lodetails = new AccountsDetailsEn();
                    transAmount = (TotalAmount - CurrentAmount);
                    lodetails.TransactionAmount = (TotalAmount - CurrentAmount);
                    lodetails.PaidAmount = (TotalAmount - CurrentAmount);
                    lodetails.ReferenceCode = lolist[i].TransactionCode;
                    lodetails.ReferenceOne = lolist[i].CreditRef;
                    lodetails.ReferenceTwo = "Auto";
                    lodetaillist.Add(lodetails);

                }
                CurrentAmount += transAmount;
                if (TotalAmount == CurrentAmount)
                {
                    break;
                }
            }
            return lodetaillist;

        }

        #endregion

        #region UpdatePocketAmounts

        /// <summary>
        /// Method to Update Pocket Amounts
        /// </summary>
        /// <param name="argEn">Accounts Entity is the Input.PaidAmount,Updateby,UpdatedTime,SubreferenceOne,CreditRef are Input Properties</param>
        /// <returns>Returns Bool</returns>
        public bool UpdatePocketAmounts(AccountsEn argEn)
        {
            bool lbRes = false;
            string sqlCmd;

            try
            {
                sqlCmd = "UPDATE SAS_Accounts SET PaidAmount = '" + argEn.PaidAmount + "', UpdatedBy = '" + argEn.UpdatedBy + "', TransStatus = 'Closed', UpdatedTime = '" + argEn.UpdatedTime + "' WHERE Category = 'SPA'" +
                         " and BatchCode = '" + argEn.SubReferenceOne + "' and CreditRef = '" + argEn.CreditRef + "' and Description = 'Sponsor Pocket Amount' ";

                if (!FormHelp.IsBlank(sqlCmd))
                {
                    int liRowAffected = _DatabaseFactory.ExecuteSqlStatement(
                        Helper.GetDataBaseType, DataBaseConnectionString, sqlCmd);

                    if (liRowAffected > -1)
                        lbRes = true;
                    else
                        throw new Exception("Update Failed! No Row has been updated...");

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return lbRes;
        }

        #endregion

        #region UpdateAllocations

        /// <summary>
        /// Method to Update Allocations
        /// </summary>
        /// <param name="argEn">Accounts Entity is the Input.Trnascode,Updateby and UpdatedTime are Input Properties</param>
        /// <returns>Returns boolean</returns>
        public bool UpdateAllocations(AccountsEn argEn)
        {
            bool lbRes = false;
            string sqlCmd;

            try
            {
                sqlCmd = "UPDATE SAS_Accounts SET TransStatus = 'Closed', UpdatedBy = @UpdatedBy, UpdatedTime = @UpdatedTime WHERE TransCode = @TransCode";

                if (!FormHelp.IsBlank(sqlCmd))
                {
                    DbCommand cmd = _DatabaseFactory.GetDbCommand(Helper.GetDataBaseType, sqlCmd, DataBaseConnectionString);
                    _DatabaseFactory.AddInParameter(ref cmd, "@TransCode", DbType.String, argEn.CreditRef);
                    _DatabaseFactory.AddInParameter(ref cmd, "@UpdatedBy", DbType.String, argEn.UpdatedBy);
                    _DatabaseFactory.AddInParameter(ref cmd, "@UpdatedTime", DbType.DateTime, argEn.UpdatedTime);
                    _DbParameterCollection = cmd.Parameters;

                    int liRowAffected = _DatabaseFactory.ExecuteNonQuery(Helper.GetDataBaseType, cmd,
                                DataBaseConnectionString, sqlCmd, _DbParameterCollection);

                    if (liRowAffected > -1)
                        lbRes = true;
                    else
                        throw new Exception("Update Failed! No Row has been updated...");
                }


            }
            catch (Exception ex)
            {
                throw ex;
            }
            return lbRes;
        }

        #endregion

        #region UpdateIntegrationStatus

        /// <summary>
        /// Method to Update IntegrationStatus
        /// </summary>
        /// <param name="argEn">Accounts Entity is the Input.Batchcode  is Input Property</param>
        /// <returns>Returns boolean</returns>
        public bool UpdateIntegrationStatus(AccountsEn argEn)
        {
            bool lbRes = false;
            string sqlCmd;

            try
            {
                sqlCmd = "UPDATE SAS_Accounts SET Intstatus = Intstatus + 1, UpdatedBy = @UpdatedBy, UpdatedTime = @UpdatedTime WHERE Batchcode = @BatchCode";

                if (!FormHelp.IsBlank(sqlCmd))
                {
                    DbCommand cmd = _DatabaseFactory.GetDbCommand(Helper.GetDataBaseType, sqlCmd, DataBaseConnectionString);
                    _DatabaseFactory.AddInParameter(ref cmd, "@BatchCode", DbType.String, argEn.BatchCode);
                    _DatabaseFactory.AddInParameter(ref cmd, "@UpdatedBy", DbType.String, argEn.UpdatedBy);
                    _DatabaseFactory.AddInParameter(ref cmd, "@UpdatedTime", DbType.DateTime, argEn.UpdatedTime);
                    _DbParameterCollection = cmd.Parameters;

                    int liRowAffected = _DatabaseFactory.ExecuteNonQuery(Helper.GetDataBaseType, cmd,
                                DataBaseConnectionString, sqlCmd, _DbParameterCollection);

                    if (liRowAffected > -1)
                        lbRes = true;
                    else
                        throw new Exception("Update Failed! No Row has been updated...");
                }


            }
            catch (Exception ex)
            {
                throw ex;
            }
            return lbRes;
        }

        #endregion

        #region UpdateReceipts

        /// <summary>
        /// Method to Update Receipts
        /// </summary>
        /// <param name="argEn">Accounts Entity is the Input.Trnascode,PaidAmount,Updateby and UpdatedTime are Input Properties</param>
        /// <returns>Returns Boolean</returns>
        public bool UpdateReceipts(AccountsEn argEn)
        {
            bool lbRes = false;
            string sqlCmd;

            try
            {
                //sqlCmd = "UPDATE SAS_Accounts SET PaidAmount = PaidAmount +@PaidAmount, UpdatedBy = @UpdatedBy, UpdatedTime = @UpdatedTime WHERE TransCode = @TransCode";

                sqlCmd = "UPDATE SAS_Accounts SET PaidAmount = @PaidAmount, UpdatedBy = @UpdatedBy, UpdatedTime = @UpdatedTime WHERE TransCode = @TransCode";

                if (!FormHelp.IsBlank(sqlCmd))
                {
                    DbCommand cmd = _DatabaseFactory.GetDbCommand(Helper.GetDataBaseType, sqlCmd, DataBaseConnectionString);
                    _DatabaseFactory.AddInParameter(ref cmd, "@TransCode", DbType.String, argEn.CreditRefOne);
                    _DatabaseFactory.AddInParameter(ref cmd, "@PaidAmount", DbType.Double, argEn.PaidAmount);
                    _DatabaseFactory.AddInParameter(ref cmd, "@UpdatedBy", DbType.String, argEn.UpdatedBy);
                    _DatabaseFactory.AddInParameter(ref cmd, "@UpdatedTime", DbType.DateTime, Helper.DateConversion(argEn.UpdatedTime));
                    _DbParameterCollection = cmd.Parameters;

                    int liRowAffected = _DatabaseFactory.ExecuteNonQuery(Helper.GetDataBaseType, cmd,
                                DataBaseConnectionString, sqlCmd, _DbParameterCollection);

                    if (liRowAffected > -1)
                        lbRes = true;
                    else
                        throw new Exception("Update Failed! No Row has been updated...");

                    //Update Transaction Status

                    AccountsEn eob = new AccountsEn();
                    AccountsDAL dob = new AccountsDAL();
                    eob.TransactionCode = argEn.CreditRefOne;
                    eob.Category = "";
                    if (argEn.Category == "Receipt")
                    {
                        eob.Category = argEn.Category;
                    }
                    eob = dob.GetReceiptItem(eob);
                    if (eob.TransactionAmount == eob.PaidAmount)
                    {
                        try
                        {
                            sqlCmd = "UPDATE SAS_Accounts SET TransStatus = @TransStatus where TransCode = '" + eob.TransactionCode + "'";

                            if (!FormHelp.IsBlank(sqlCmd))
                            {
                                eob.TransStatus = "Closed";

                                DbCommand cmd1 = _DatabaseFactory.GetDbCommand(Helper.GetDataBaseType, sqlCmd, DataBaseConnectionString);
                                _DatabaseFactory.AddInParameter(ref cmd1, "@TransStatus", DbType.String, eob.TransStatus);
                                _DbParameterCollection = cmd1.Parameters;

                                int liRowAff = _DatabaseFactory.ExecuteNonQuery(Helper.GetDataBaseType, cmd,
                                    DataBaseConnectionString, sqlCmd, _DbParameterCollection);

                                if (liRowAff > -1)
                                    lbRes = true;
                                else
                                    throw new Exception("Update Failed! No Row has been updated...");
                            }
                        }
                        catch (Exception ex)
                        {
                            throw ex;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return lbRes;
        }

        #endregion

        #region Update

        /// <summary>
        /// Method to Update Accounts
        /// </summary>
        /// <param name="argEn">Accounts Entity is the Input</param>
        /// <returns>Returns Boolean</returns>
        public bool Update(AccountsEn argEn)
        {
            bool lbRes = false;
            string sqlCmd;

            try
            {
                sqlCmd = "UPDATE SAS_Accounts SET TransTempCode = @TransTempCode, TransCode = @TransCode, CreditRef = @CreditRef, CreditRef1 = @CreditRef1, DebitRef = @DebitRef, DebitRef1 = @DebitRef1, Category = @Category, SubCategory = @SubCategory, TransType = @TransType, SubType = @SubType, SourceType = @SourceType, TransDate = @TransDate, DueDate = @DueDate, BatchCode = @BatchCode, BatchDate = @BatchDate, CrRef1 = @CrRef1, CrRef2 = @CrRef2, Description = @Description, Currency = @Currency, BatchTotal = @BatchTotal, Tax = @Tax, Discount = @Discount, TaxAmount = @TaxAmount, DiscountAmount = @DiscountAmount, TransAmount = @TransAmount, PaidAmount = @PaidAmount, TransStatus = @TransStatus, TempAmount = @TempAmount, TempPaidAmount = @TempPaidAmount, PaymentMode = @PaymentMode, BankCode = @BankCode, PayeeName = @PayeeName, ChequeDate = @ChequeDate, ChequeNo = @ChequeNo, VoucherNo = @VoucherNo, SubRef1 = @SubRef1, SubRef2 = @SubRef2, SubRef3 = @SubRef3, PostStatus = @PostStatus, IntStatus = @IntStatus, CreatedBy = @CreatedBy, CreatedDateTime = @CreatedDateTime, PostedBy = @PostedBy, PostedDateTime = @PostedDateTime, IntCode = @IntCode, GLCode = @GLCode, UpdatedBy = @UpdatedBy, UpdatedTime = @UpdatedTime WHERE TransID = @TransID";

                if (!FormHelp.IsBlank(sqlCmd))
                {
                    if (argEn.Category == "Invoice" && argEn.PostStatus == "Posted")
                    {
                        if (argEn.AccountDetailsList != null)
                        {
                            argEn.TransactionCode = GetAutoNumber("Inv");
                        }
                    }
                    else if (argEn.Category == "Credit Note" && argEn.PostStatus == "Posted")
                    {
                        if (argEn.SubCategory == "WriteOff")
                        { }
                        else
                        {
                            argEn.TransactionCode = GetAutoNumber("CN");
                        }
                    }
                    else if (argEn.Category == "Debit Note" && argEn.PostStatus == "Posted")
                    {
                        argEn.TransactionCode = GetAutoNumber("DN");
                    }
                    DbCommand cmd = _DatabaseFactory.GetDbCommand(Helper.GetDataBaseType, sqlCmd, DataBaseConnectionString);
                    _DatabaseFactory.AddInParameter(ref cmd, "@TransID", DbType.Int32, argEn.TranssactionID);
                    _DatabaseFactory.AddInParameter(ref cmd, "@TransTempCode", DbType.String, argEn.TempTransCode);
                    _DatabaseFactory.AddInParameter(ref cmd, "@TransCode", DbType.String, argEn.TransactionCode);
                    _DatabaseFactory.AddInParameter(ref cmd, "@CreditRef", DbType.String, argEn.CreditRef);
                    _DatabaseFactory.AddInParameter(ref cmd, "@CreditRef1", DbType.String, argEn.CreditRefOne);
                    _DatabaseFactory.AddInParameter(ref cmd, "@DebitRef", DbType.String, argEn.DebitRef);
                    _DatabaseFactory.AddInParameter(ref cmd, "@DebitRef1", DbType.String, argEn.DebitRefOne);
                    _DatabaseFactory.AddInParameter(ref cmd, "@Category", DbType.String, argEn.Category);
                    _DatabaseFactory.AddInParameter(ref cmd, "@SubCategory", DbType.String, argEn.SubCategory);
                    _DatabaseFactory.AddInParameter(ref cmd, "@TransType", DbType.String, argEn.TransType);
                    _DatabaseFactory.AddInParameter(ref cmd, "@SubType", DbType.String, argEn.SubType);
                    _DatabaseFactory.AddInParameter(ref cmd, "@SourceType", DbType.String, argEn.SourceType);
                    _DatabaseFactory.AddInParameter(ref cmd, "@TransDate", DbType.DateTime, argEn.TransDate);
                    _DatabaseFactory.AddInParameter(ref cmd, "@DueDate", DbType.DateTime, argEn.DueDate);
                    _DatabaseFactory.AddInParameter(ref cmd, "@BatchCode", DbType.String, argEn.BatchCode);
                    _DatabaseFactory.AddInParameter(ref cmd, "@BatchDate", DbType.DateTime, argEn.BatchDate);
                    _DatabaseFactory.AddInParameter(ref cmd, "@CrRef1", DbType.String, argEn.CrRefOne);
                    _DatabaseFactory.AddInParameter(ref cmd, "@CrRef2", DbType.String, argEn.CrRefTwo);
                    _DatabaseFactory.AddInParameter(ref cmd, "@Description", DbType.String, argEn.Description);
                    _DatabaseFactory.AddInParameter(ref cmd, "@Currency", DbType.String, argEn.CurrencyUsed);
                    _DatabaseFactory.AddInParameter(ref cmd, "@BatchTotal", DbType.Double, argEn.BatchTotal);
                    _DatabaseFactory.AddInParameter(ref cmd, "@Tax", DbType.Double, argEn.TaxPercentage);
                    _DatabaseFactory.AddInParameter(ref cmd, "@Discount", DbType.Double, argEn.DiscountPercentage);
                    _DatabaseFactory.AddInParameter(ref cmd, "@TaxAmount", DbType.Double, argEn.TaxAmount);
                    _DatabaseFactory.AddInParameter(ref cmd, "@DiscountAmount", DbType.Double, argEn.DiscountAmount);
                    _DatabaseFactory.AddInParameter(ref cmd, "@TransAmount", DbType.Double, argEn.TransactionAmount);
                    _DatabaseFactory.AddInParameter(ref cmd, "@PaidAmount", DbType.Double, argEn.PaidAmount);
                    _DatabaseFactory.AddInParameter(ref cmd, "@TransStatus", DbType.String, argEn.TransStatus);
                    _DatabaseFactory.AddInParameter(ref cmd, "@TempAmount", DbType.Double, argEn.TempAmount);
                    _DatabaseFactory.AddInParameter(ref cmd, "@TempPaidAmount", DbType.Double, argEn.TempPaidAmount);
                    _DatabaseFactory.AddInParameter(ref cmd, "@PaymentMode", DbType.String, argEn.PaymentMode);
                    _DatabaseFactory.AddInParameter(ref cmd, "@BankCode", DbType.String, argEn.BankCode);
                    _DatabaseFactory.AddInParameter(ref cmd, "@PayeeName", DbType.String, argEn.PayeeName);
                    _DatabaseFactory.AddInParameter(ref cmd, "@ChequeDate", DbType.DateTime, argEn.ChequeDate);
                    _DatabaseFactory.AddInParameter(ref cmd, "@ChequeNo", DbType.String, argEn.ChequeNo);
                    _DatabaseFactory.AddInParameter(ref cmd, "@VoucherNo", DbType.String, argEn.VoucherNo);
                    _DatabaseFactory.AddInParameter(ref cmd, "@SubRef1", DbType.String, argEn.SubReferenceOne);
                    _DatabaseFactory.AddInParameter(ref cmd, "@SubRef2", DbType.String, argEn.SubReferenceTwo);
                    _DatabaseFactory.AddInParameter(ref cmd, "@SubRef3", DbType.String, argEn.SubReferenceThree);
                    _DatabaseFactory.AddInParameter(ref cmd, "@PostStatus", DbType.String, argEn.PostStatus);
                    _DatabaseFactory.AddInParameter(ref cmd, "@IntStatus", DbType.Int32, argEn.IntegrationStatus);
                    _DatabaseFactory.AddInParameter(ref cmd, "@CreatedBy", DbType.String, argEn.CreatedBy);
                    _DatabaseFactory.AddInParameter(ref cmd, "@CreatedDateTime", DbType.DateTime, argEn.CreatedDateTime);
                    _DatabaseFactory.AddInParameter(ref cmd, "@PostedBy", DbType.String, argEn.PostedBy);
                    _DatabaseFactory.AddInParameter(ref cmd, "@PostedDateTime", DbType.DateTime, argEn.PostedDateTime);
                    _DatabaseFactory.AddInParameter(ref cmd, "@IntCode", DbType.String, argEn.IntegrationCode);
                    _DatabaseFactory.AddInParameter(ref cmd, "@GLCode", DbType.String, argEn.GLCode);
                    _DatabaseFactory.AddInParameter(ref cmd, "@UpdatedBy", DbType.String, argEn.UpdatedBy);
                    _DatabaseFactory.AddInParameter(ref cmd, "@UpdatedTime", DbType.DateTime, argEn.UpdatedTime);
                    _DbParameterCollection = cmd.Parameters;

                    int liRowAffected = _DatabaseFactory.ExecuteNonQuery(Helper.GetDataBaseType, cmd,
                           DataBaseConnectionString, sqlCmd, _DbParameterCollection);

                    if (liRowAffected > -1)
                        lbRes = true;
                    else
                        throw new Exception("Update Failed! No Row has been updated...");

                    //Updating AccountsDetails table
                    if (argEn.AccountDetailsList != null)
                    {
                        AccountsDetailsDAL loDS = new AccountsDetailsDAL();
                        argEn.AccountDetailsList[0].TransactionID = argEn.TranssactionID;
                        loDS.Delete(argEn.AccountDetailsList[0]);
                        for (int i = 0; i < argEn.AccountDetailsList.Count; i++)
                        {
                            argEn.AccountDetailsList[i].TransactionID = argEn.TranssactionID;
                            argEn.AccountDetailsList[i].TransactionCode = argEn.TransactionCode;
                            loDS.Update(argEn.AccountDetailsList[i]);
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return lbRes;
        }
        #endregion

        #region ReceiptUpdate
        /// <summary>
        /// Method to Update Receipt
        /// </summary>
        /// Added by Hafiz Roslan
        /// Date: 05/01/2016
        /// modified by Hafiz @ 11/4/2016 - add outstanding amount

        public void ReceiptUpdate(AccountsEn argEn, List<StudentEn> argList)
        {
            int i = 0;
            bool flag = false;

            for (i = 0; i < argList.Count; i++)
            {
                argEn.CreditRef = argList[i].MatricNo;
                argEn.TransactionAmount = argList[i].TransactionAmount;
                argEn.BankSlipID = argList[i].BankSlipID;
                argEn.ReceiptDate = argList[i].ReceiptDate;
                argEn.AccountDetailsList = argList[i].AccountDetailsList;
                argEn.PaidAmount = argList[i].PaidAmount;
                argEn.StManual = argList[i].SubReferenceTwo;
                argEn.TransactionCode = argList[i].TransactionCode;
                argEn.Outstanding_Amount = argList[i].Outstanding_Amount;

                //updated by Hafiz Roslan @ 27/01/2016
                flag = CheckStudAvailableOrNot(argEn.CreditRef, argEn.BatchCode);

                if (flag == true)
                {
                    try
                    {
                        //Update Receipt Details - Start                    
                        UpdateReceiptDetails(argList[i].MatricNo, argList[i].ICNo, argEn.SubReferenceOne, argList[i].TransDate, argList[i].TransactionAmount);
                        //Update Receipt Details - End
                    }
                    catch (Exception ex)
                    {
                        MaxModule.Helper.LogError(ex.Message);
                        throw ex;
                    }

                    //Update Account Details - Start
                    _Update(argEn);
                    //Update Account Details - Stop
                }
                else
                {
                    try
                    {
                        //Insert Receipt Details - Start                    
                        InsertReceiptDetails(argList[i].MatricNo, argList[i].ICNo, argEn.SubReferenceOne, argList[i].TransDate, argList[i].TransactionAmount);
                        //Insert Receipt Details - End
                    }
                    catch (Exception ex)
                    {
                        MaxModule.Helper.LogError(ex.Message);
                        throw ex;
                    }

                    Insert(argEn);
                }
            }
        }

        #endregion

        #region ReceiptUpdate2
        /// <summary>
        /// Method to Update Sponsor
        /// </summary>
        /// Added by Hafiz Roslan
        /// Date: 05/01/2016

        public void ReceiptUpdate2(AccountsEn argEn, List<SponsorEn> argList)
        {
            int i = 0;
            double StuAllAmt = 0.00;

            StuAllAmt = argEn.AllocatedAmount;

            for (i = 0; i < argList.Count; i++)
            {
                argEn.CreditRef = argList[i].SponserCode;

                try
                {
                    //Update Account Details - Start
                    _Update(argEn);
                    //Update Account Details - Stop
                }
                catch (Exception ex)
                {
                    MaxModule.Helper.LogError(ex.Message);
                    throw ex;
                }
            }
            argEn.AllocatedAmount = StuAllAmt;

            //update receipt sponser details allocate amount
            if (argEn.SubType == "Sponsor" && argEn.Category == "Allocation")
            {
                //varaible declaretion
                string UpdateStatement;


                //Build Update Statement - Start
                UpdateStatement = "UPDATE sas_accounts SET allocateamount = '" + argEn.AllocatedAmount + "' WHERE batchcode = '" + argEn.CreditRefOne + "'";
                //Build Update Statement - Stop

                //loop thro the batch details - stop

                //if update statement successful - Start
                if (!FormHelp.IsBlank(UpdateStatement))
                {
                    if (_DatabaseFactory.ExecuteSqlStatement(Helper.GetDataBaseType,
                        DataBaseConnectionString, UpdateStatement) > -1)
                    {
                        //success
                    }
                }
                //if update statement successful - Stop
            }
            // ended
        }
        #endregion

        #region UpdateStudentOutstanding

        /// <summary>
        /// Method to Update Student Outstanding
        /// </summary>
        /// <param name="MatricNo">Accounts Entity is the Input.Matric No</param>
        /// <returns>Returns boolean</returns>
        public bool UpdateStudentOutstanding(string matricNo)
        {

            bool lbRes = false;
            string sqlCmd = string.Empty;
            double loanAmount = 0;
            StudentEn loItem = new StudentEn();

            // Fetch the Loan Amount for the student if exists
            loItem = GetStudentOutstanding(matricNo);
            loanAmount = loItem.LoanAmount;

            //Deletes the exising record
            try
            {
                sqlCmd = "DELETE FROM SAS_StudentOutstanding WHERE SASI_MatricNo= @MatricNo";

                if (!FormHelp.IsBlank(sqlCmd))
                {
                    DbCommand cmd = _DatabaseFactory.GetDbCommand(Helper.GetDataBaseType, sqlCmd, DataBaseConnectionString);
                    _DatabaseFactory.AddInParameter(ref cmd, "@MatricNo", DbType.String, matricNo);
                    _DbParameterCollection = cmd.Parameters;

                    int liRowAffected = _DatabaseFactory.ExecuteNonQuery(Helper.GetDataBaseType, cmd,
                                DataBaseConnectionString, sqlCmd, _DbParameterCollection);

                    if (liRowAffected > -1)
                        lbRes = true;
                    else
                        throw new Exception("Deletion Failed! No Row has been deleted...");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            //Inserts the New Outstanding amount
            try
            {
                sqlCmd = @" INSERT    INTO SAS_StudentOutstanding
                                    SELECT  *
                                    FROM    ( SELECT  DISTINCT
                                                        SS.SASI_MatricNo ,
                                                        SS.SASI_Name ,
                                                        SS.SASI_PgId ,
                                                        SS.SASI_CurSem ,
                                                        SS.SASI_CurSemYr,
                                                        ISNULL(( SELECT SUM(TransAmount) AS Amount
                                                                 FROM   SAS_Accounts
                                                                 WHERE  CreditRef = SS.SASI_MatricNo
                                                                        AND SubType = 'Student'
                                                                        AND PostStatus = 'Posted'
                                                                        AND TransType = 'Credit'
                                                               ), 0)
                                                         ISNULL(( SELECT   SUM(TransAmount) AS Amount
                                                                   FROM     SAS_Accounts
                                                                   WHERE    CreditRef = SS.SASI_MatricNo
                                                                            AND SubType = 'Student'
                                                                            AND PostStatus = 'Posted'
                                                                            AND TransType = 'Debit'
                                                                 ), 0) AS SASO_DueAmount ,
                                                        NULL AS SASO_LoanAmount ,
                                                        0 AS SASO_IsReleased
                                              FROM      SAS_Accounts SA
                                                        INNER JOIN SAS_Student SS ON SA.CreditRef = SS.SASI_MatricNo
                                                                                  AND SS.SASI_MatricNo = '" + matricNo + "' ";
                sqlCmd = sqlCmd + @"  WHERE  SA.SubType = 'Student'
                                                        AND SA.PostStatus = 'Posted'
                                                ) a
                                        WHERE   a.SASO_DueAmount > 0 ";


                if (!FormHelp.IsBlank(sqlCmd))
                {
                    int liRowAffected = _DatabaseFactory.ExecuteSqlStatement(
                        Helper.GetDataBaseType, DataBaseConnectionString, sqlCmd);

                    if (liRowAffected > -1)
                        lbRes = true;
                    else
                        throw new Exception("Update Failed! No Row has been updated...");
                }

                // Update LoanAmount in the Student Outstanding table
                if (loanAmount > 0)
                {
                    try
                    {
                        sqlCmd = @"Update SAS_StudentOutstanding SET SASO_LoanAmount =@SASO_LoanAmount where SASI_MatricNo =@SASI_MatricNo";

                        if (!FormHelp.IsBlank(sqlCmd))
                        {
                            DbCommand cmd = _DatabaseFactory.GetDbCommand(Helper.GetDataBaseType, sqlCmd, DataBaseConnectionString);
                            _DatabaseFactory.AddInParameter(ref cmd, "@SASO_LoanAmount", DbType.Double, loanAmount);
                            _DatabaseFactory.AddInParameter(ref cmd, "@SASI_MatricNo", DbType.String, matricNo);
                            _DbParameterCollection = cmd.Parameters;

                            int liRowAffected = _DatabaseFactory.ExecuteNonQuery(Helper.GetDataBaseType, cmd,
                                DataBaseConnectionString, sqlCmd, _DbParameterCollection);

                            if (liRowAffected > -1)
                                lbRes = true;
                            else
                                throw new Exception("Update Failed! No Row has been updated...");
                        }
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }

            return lbRes;
        }

        #endregion

        #region UpdateStudentOutstandingLoan

        /// <summary>
        /// Method to Update Student Outstanding Loan
        /// </summary>
        /// <param name="MatricNo">Accounts Entity is the Input.Matric No</param>
        /// <returns>Returns boolean</returns>
        public bool UpdateStudentOutstandingLoan(string matricNo)
        {

            bool lbRes = false;
            string sqlCmd = string.Empty;
            double loanAmount = 0;
            StudentEn loItem = new StudentEn();

            // Fetch the Loan Amount for the student if exists
            loItem = GetStudentOutstandingLoanAmount(matricNo);
            loanAmount = loItem.LoanAmount;
            try
            {
                // Update LoanAmount in the Student Outstanding table
                if (loanAmount > 0)
                {
                    try
                    {
                        sqlCmd = @"Update SAS_StudentOutstanding SET SASO_LoanAmount =@SASO_LoanAmount where SASI_MatricNo =@SASI_MatricNo";

                        if (!FormHelp.IsBlank(sqlCmd))
                        {
                            DbCommand cmd = _DatabaseFactory.GetDbCommand(Helper.GetDataBaseType, sqlCmd, DataBaseConnectionString);
                            _DatabaseFactory.AddInParameter(ref cmd, "@SASO_LoanAmount", DbType.Double, loanAmount);
                            _DatabaseFactory.AddInParameter(ref cmd, "@SASI_MatricNo", DbType.String, matricNo);
                            _DbParameterCollection = cmd.Parameters;

                            int liRowAffected = _DatabaseFactory.ExecuteNonQuery(Helper.GetDataBaseType, cmd,
                                DataBaseConnectionString, sqlCmd, _DbParameterCollection);

                            if (liRowAffected > -1)
                                lbRes = true;
                            else
                                throw new Exception("Update Failed! No Row has been updated...");
                        }
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }

            return lbRes;
        }

        #endregion

        #region GetStudentOutstanding
        //modified by Hafiz @ 27/5/2016

        public StudentEn GetStudentOutstanding(string matricNo)
        {
            StudentEn loItem = new StudentEn();
            string sqlCmd = string.Empty;

            try
            {
                sqlCmd = "SELECT SASO_Id,SASI_MatricNo,SASI_Name,SASI_PgId,SASI_CurSem,SASI_CurSemYr,SASO_Outstandingamt,SASO_IsReleased FROM SAS_StudentOutstanding WHERE SASI_MatricNo='" + matricNo + "'";

                if (!FormHelp.IsBlank(sqlCmd))
                {
                    DbCommand cmd = _DatabaseFactory.GetDbCommand(Helper.GetDataBaseType, sqlCmd, DataBaseConnectionString);
                    _DatabaseFactory.AddInParameter(ref cmd, "@MatricNo", DbType.String, matricNo);
                    _DbParameterCollection = cmd.Parameters;

                    if (!FormHelp.IsBlank(sqlCmd))
                    {
                        using (IDataReader loReader = _DatabaseFactory.GetIDataReader(Helper.GetDataBaseType, cmd,
                            DataBaseConnectionString, sqlCmd, _DbParameterCollection).CreateDataReader())
                        {
                            while (loReader.Read())
                            {
                                loItem.MatricNo = GetValue<string>(loReader, "SASI_MatricNo");
                                loItem.ProgramID = GetValue<string>(loReader, "SASI_PgId");
                                loItem.CurrentSemester = GetValue<int>(loReader, "SASI_CurSem");
                                loItem.CurretSemesterYear = GetValue<string>(loReader, "SASI_CurSemYr");
                                loItem.OutstandingAmount = GetValue<double>(loReader, "SASO_Outstandingamt");
                                loItem.IsReleased = GetValue<int>(loReader, "SASO_IsReleased");
                            }
                            loReader.Close();
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }

            return loItem;

        }

        #endregion

        #region GetStudentOutstandingLoanAmount

        public StudentEn GetStudentOutstandingLoanAmount(string matricNo)
        {
            StudentEn loItem = new StudentEn();
            string sqlCmd = string.Empty;

            try
            {
                sqlCmd = @"Select 
                             CreditRef,
                           (Select CASE WHEN SUM(TransAmount) IS NULL THEN 0 ELSE SUM(TransAmount) End from SAS_StudentLoan where Category ='Loan' and CreditRef=sl.CreditRef ) as Total,
                           (Select CASE WHEN SUM(TransAmount) IS NULL THEN 0 ELSE SUM(TransAmount) End from SAS_StudentLoan where Category ='Receipt' and CreditRef=sl.CreditRef ) as PaidAmount, 
                           (Select CASE WHEN SUM(TransAmount) IS NULL THEN 0 ELSE SUM(TransAmount) End from SAS_StudentLoan where Category ='Loan' and CreditRef=sl.CreditRef ) -
                           (Select CASE WHEN SUM(TransAmount) IS NULL THEN 0 ELSE SUM(TransAmount) End from SAS_StudentLoan where Category ='Receipt' and CreditRef=sl.CreditRef ) as LoanAmount  
                            FROM SAS_StudentLoan sl
                            WHERE sl.PostStatus='Posted'
                            and sl.CreditRef = " + clsGeneric.AddQuotes(matricNo);

                if (!FormHelp.IsBlank(sqlCmd))
                {
                    DbCommand cmdSel = _DatabaseFactory.GetDbCommand(Helper.GetDataBaseType, sqlCmd, DataBaseConnectionString);
                    _DatabaseFactory.AddInParameter(ref cmdSel, "@MatricNo", DbType.String, matricNo);
                    _DbParameterCollection = cmdSel.Parameters;

                    using (IDataReader loReader = _DatabaseFactory.GetIDataReader(Helper.GetDataBaseType, cmdSel,
                        DataBaseConnectionString, sqlCmd, _DbParameterCollection).CreateDataReader())
                    {
                        while (loReader.Read())
                        {
                            loItem.MatricNo = GetValue<string>(loReader, "CreditRef");
                            loItem.LoanAmount = GetValue<double>(loReader, "LoanAmount");
                        }
                        loReader.Close();
                    }
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }

            return loItem;

        }

        #endregion

        #region BatchDelete

        /// <summary>
        /// Method to Delete Batch
        /// </summary>
        /// <param name="argEn">Accounts Entity is the Input.BatchCode is an Input Property</param>
        /// <returns>Returns a Boolean</returns> 
        public bool BatchDelete(AccountsEn argEn)
        {
            bool lbRes = false;
            List<AccountsEn> loEnList = new List<AccountsEn>();
            List<AccountsDetailsEn> lodetailslist = new List<AccountsDetailsEn>();
            string sqlCmd = "select * from SAS_Accounts WHERE BatchCode = @BatchCode";

            try
            {
                //argEn.BatchCode = StudentLoanInsert(argEn);

                if (!FormHelp.IsBlank(sqlCmd))
                {
                    DbCommand cmdSel = _DatabaseFactory.GetDbCommand(Helper.GetDataBaseType, sqlCmd, DataBaseConnectionString);
                    _DatabaseFactory.AddInParameter(ref cmdSel, "@BatchCode", DbType.String, argEn.BatchCode);
                    _DbParameterCollection = cmdSel.Parameters;

                    using (IDataReader loReader = _DatabaseFactory.GetIDataReader(Helper.GetDataBaseType, cmdSel,
                       DataBaseConnectionString, sqlCmd, _DbParameterCollection).CreateDataReader())
                    {
                        AccountsDetailsDAL loObjAccountsDAL = new AccountsDetailsDAL();
                        AccountsDetailsEn loItem = null;
                        while (loReader.Read())
                        {
                            loItem = new AccountsDetailsEn();
                            loItem.TransactionID = GetValue<int>(loReader, "TransID");
                            lodetailslist.Add(loItem);
                            loItem = null;
                        }
                        loReader.Close();
                        int i = 0;
                        //deleting each item in batch
                        for (i = 0; i < lodetailslist.Count; i++)
                        {
                            loObjAccountsDAL.Delete(lodetailslist[i]);
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            Delete(argEn);
            return lbRes;
        }

        #endregion

        #region BatchDelete

        /// <summary>
        /// Method to Delete Batch
        /// </summary>
        /// <param name="argEn">Accounts Entity is the Input.BatchCode is an Input Property</param>
        /// <returns>Returns a Boolean</returns> 
        public bool BatchDelete(AccountsEn argEn, bool IsStausUpdate)
        {
            bool lbRes = false;
            List<AccountsEn> loEnList = new List<AccountsEn>();
            List<AccountsDetailsEn> lodetailslist = new List<AccountsDetailsEn>();
            string sqlCmd = "select * from SAS_Accounts WHERE BatchCode = @BatchCode";

            try
            {
                if (!FormHelp.IsBlank(sqlCmd))
                {
                    DbCommand cmdSel = _DatabaseFactory.GetDbCommand(Helper.GetDataBaseType, sqlCmd, DataBaseConnectionString);
                    _DatabaseFactory.AddInParameter(ref cmdSel, "@BatchCode", DbType.String, argEn.BatchCode);
                    _DbParameterCollection = cmdSel.Parameters;

                    using (IDataReader loReader = _DatabaseFactory.GetIDataReader(Helper.GetDataBaseType, cmdSel,
                        DataBaseConnectionString, sqlCmd, _DbParameterCollection).CreateDataReader())
                    {
                        AccountsDetailsDAL loObjAccountsDAL = new AccountsDetailsDAL();
                        AccountsDetailsEn loItem = null;
                        while (loReader.Read())
                        {
                            loItem = new AccountsDetailsEn();
                            loItem.TransactionID = GetValue<int>(loReader, "TransID");
                            loItem.CreditRef = GetValue<string>(loReader, "CreditRef");
                            lodetailslist.Add(loItem);
                            loItem = null;
                        }
                        loReader.Close();
                        int i = 0;
                        //deleting each item in batch
                        for (i = 0; i < lodetailslist.Count; i++)
                        {
                            loObjAccountsDAL.Delete(lodetailslist[i]);
                        }
                        //Updating student Staus
                        if (IsStausUpdate)
                        {
                            for (i = 0; i < lodetailslist.Count; i++)
                            {
                                loObjAccountsDAL.UpdateStudentStatus(lodetailslist[i]);
                            }
                        }


                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            Delete(argEn);
            return lbRes;
        }

        #endregion

        #region Delete

        /// <summary>
        /// Method to Delete
        /// </summary>
        /// <param name="argEn">Accounts Entity is the Input.BatchCode is an Input Property</param>
        /// <returns>Returns a Boolean</returns>
        public bool Delete(AccountsEn argEn)
        {
            bool lbRes = false;
            string sqlCmd = "DELETE FROM SAS_Accounts WHERE  BatchCode = @BatchCode";


            try
            {
                //   argEn.BatchCode =  StudentLoanInsert(argEn);

                if (!FormHelp.IsBlank(sqlCmd))
                {
                    DbCommand cmd = _DatabaseFactory.GetDbCommand(Helper.GetDataBaseType, sqlCmd, DataBaseConnectionString);
                    _DatabaseFactory.AddInParameter(ref cmd, "@BatchCode", DbType.String, argEn.BatchCode);
                    _DbParameterCollection = cmd.Parameters;

                    int liRowAffected = _DatabaseFactory.ExecuteNonQuery(Helper.GetDataBaseType, cmd,
                                DataBaseConnectionString, sqlCmd, _DbParameterCollection);

                    if (liRowAffected > -1)
                        lbRes = true;
                    else
                        throw new Exception("Insertion Failed! No Row has been updated...");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return lbRes;
        }

        #endregion

        #region DeleteLoan

        /// <summary>
        /// Method to Delete
        /// </summary>
        /// <param name="argEn">Accounts Entity is the Input.BatchCode is an Input Property</param>
        /// <returns>Returns a Boolean</returns>
        public bool DeleteLoan(AccountsEn argEn)
        {
            bool lbRes = false;
            string sqlCmd = "DELETE FROM SAS_StudentLoan WHERE  BatchCode = @BatchCode";

            try
            {
                if (!FormHelp.IsBlank(sqlCmd))
                {
                    DbCommand cmd = _DatabaseFactory.GetDbCommand(Helper.GetDataBaseType, sqlCmd, DataBaseConnectionString);
                    _DatabaseFactory.AddInParameter(ref cmd, "@BatchCode", DbType.String, argEn.BatchCode);
                    _DbParameterCollection = cmd.Parameters;

                    int liRowAffected = _DatabaseFactory.ExecuteNonQuery(Helper.GetDataBaseType, cmd,
                                DataBaseConnectionString, sqlCmd, _DbParameterCollection);

                    if (liRowAffected > -1)
                        lbRes = true;
                    else
                        throw new Exception("Deletion Failed! No Row has been updated...");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return lbRes;
        }

        #endregion

        #region DeleteSucceedTrans

        /// <summary>
        /// Method to Delete Succeed Transaction
        /// </summary>
        /// <param name="argEn">Accounts Entity is the Input.BatchCode is an Input Property</param>
        /// <returns>Returns a Boolean</returns>
        public bool DeleteSucceedTrans(AccountsEn argEn)
        {
            bool lbRes = false;
            string sqlCmd = "DELETE FROM SAS_SucceedTransactionHeader WHERE STH_AutoNumber = @STH_AutoNumber";

            try
            {
                if (!FormHelp.IsBlank(sqlCmd))
                {
                    DbCommand cmd = _DatabaseFactory.GetDbCommand(Helper.GetDataBaseType, sqlCmd, DataBaseConnectionString);
                    _DatabaseFactory.AddInParameter(ref cmd, "@STH_AutoNumber", DbType.String, argEn.AutoNum);
                    _DbParameterCollection = cmd.Parameters;

                    int liRowAffected = _DatabaseFactory.ExecuteNonQuery(Helper.GetDataBaseType, cmd,
                                DataBaseConnectionString, sqlCmd, _DbParameterCollection);

                    if (liRowAffected > -1)
                    {
                        lbRes = true;
                        lbRes = DeleteSucceedTransDetails(argEn.AutoNum);
                        if (lbRes != true)
                        {
                            throw new Exception("Delete fail for " + argEn.AutoNum + ".");
                        }
                    }
                    else
                    {
                        throw new Exception("Delete fail for " + argEn.AutoNum + ".");
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return lbRes;
        }

        #endregion

        #region DeleteSucceedTransDetails

        /// <summary>
        /// Method to Delete Succeed Transaction
        /// </summary>
        /// <param name="argEn">Accounts Entity is the Input.BatchCode is an Input Property</param>
        /// <returns>Returns a Boolean</returns>
        public bool DeleteSucceedTransDetails(String AutoNum)
        {
            bool lbRes = false;
            string sqlCmd = "DELETE FROM SAS_SucceedTransactionDetails WHERE STH_AutoNum_H = @STH_AutoNum_H";

            try
            {
                if (!FormHelp.IsBlank(sqlCmd))
                {
                    DbCommand cmd = _DatabaseFactory.GetDbCommand(Helper.GetDataBaseType, sqlCmd, DataBaseConnectionString);
                    _DatabaseFactory.AddInParameter(ref cmd, "@STH_AutoNum_H", DbType.String, AutoNum);
                    _DbParameterCollection = cmd.Parameters;

                    int liRowAffected = _DatabaseFactory.ExecuteNonQuery(Helper.GetDataBaseType, cmd,
                                DataBaseConnectionString, sqlCmd, _DbParameterCollection);

                    if (liRowAffected > -1)
                        lbRes = true;
                    else
                        throw new Exception("Delete fail for " + AutoNum + ".");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return lbRes;
        }

        #endregion
                  
        #region GetSponserStuAllocateAmount
public double GetSponserStuAllocateAmount(string BatchId)
        {
            double StuAllAmt = 0.00;
            string sqlCmd = "SELECT allocateamount FROM sas_accounts WHERE batchcode='" + BatchId +"'";

            try
            {
                if (!FormHelp.IsBlank(sqlCmd))
                {
                    using (IDataReader loReader = _DatabaseFactory.ExecuteReader(Helper.GetDataBaseType,
                        DataBaseConnectionString, sqlCmd).CreateDataReader())
                    {
                        while (loReader.Read())
                        {
                            StuAllAmt = GetValue<double>(loReader, "allocateamount"); 
                            
                        }
                        loReader.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return StuAllAmt;
        }

        #endregion

        #region Load Object

        /// <summary>
        /// Method to Load Accounts Entity
        /// </summary>
        /// <param name="argReader">Reader Object is an Input.</param>
        /// <returns>Returns Accounts Entity Object</returns>
        //public AccountsEn LoadObject(IDataReader argReader)
        //{
        //    AccountsEn loItem = new AccountsEn();

        //    loItem.TranssactionID = GetValue<int>(argReader, "TransID");
        //    loItem.TempTransCode = GetValue<string>(argReader, "TransTempCode");
        //    loItem.TransactionCode = GetValue<string>(argReader, "TransCode");
        //    loItem.CreditRef = GetValue<string>(argReader, "CreditRef");
        //    loItem.CreditRefOne = GetValue<string>(argReader, "CreditRef1");
        //    loItem.DebitRef = GetValue<string>(argReader, "DebitRef");
        //    loItem.DebitRefOne = GetValue<string>(argReader, "DebitRef1");
        //    loItem.Category = GetValue<string>(argReader, "Category");
        //    loItem.SubCategory = GetValue<string>(argReader, "SubCategory");
        //    loItem.TransType = GetValue<string>(argReader, "TransType");
        //    loItem.SubType = GetValue<string>(argReader, "SubType");
        //    loItem.SourceType = GetValue<string>(argReader, "SourceType");
        //    loItem.TransDate = GetValue<DateTime>(argReader, "TransDate");
        //    loItem.DueDate = GetValue<DateTime>(argReader, "DueDate");
        //    loItem.BatchCode = GetValue<string>(argReader, "BatchCode");
        //    loItem.BatchIntake = GetValue<string>(argReader, "BatchIntake");
        //    loItem.BatchDate = GetValue<DateTime>(argReader, "BatchDate");
        //    loItem.CrRefOne = GetValue<string>(argReader, "CrRef1");
        //    loItem.CrRefTwo = GetValue<string>(argReader, "CrRef2");
        //    loItem.Description = GetValue<string>(argReader, "Description");
        //    loItem.CurrencyUsed = GetValue<string>(argReader, "Currency");
        //    loItem.BatchTotal = GetValue<double>(argReader, "BatchTotal");
        //    loItem.TaxPercentage = GetValue<double>(argReader, "Tax");
        //    loItem.DiscountPercentage = GetValue<double>(argReader, "Discount");
        //    loItem.TaxAmount = GetValue<double>(argReader, "TaxAmount");
        //    loItem.DiscountAmount = GetValue<double>(argReader, "DiscountAmount");
        //    loItem.TransactionAmount = GetValue<double>(argReader, "TransAmount");
        //    loItem.PaidAmount = GetValue<double>(argReader, "PaidAmount");
        //    loItem.TransStatus = GetValue<string>(argReader, "TransStatus");
        //    loItem.TempAmount = GetValue<double>(argReader, "TempAmount");
        //    loItem.TempPaidAmount = GetValue<double>(argReader, "TempPaidAmount");
        //    loItem.PaymentMode = GetValue<string>(argReader, "PaymentMode");
        //    loItem.BankCode = GetValue<string>(argReader, "BankCode");
        //    loItem.PayeeName = GetValue<string>(argReader, "PayeeName");
        //    loItem.ChequeDate = GetValue<DateTime>(argReader, "ChequeDate");
        //    loItem.ChequeNo = GetValue<string>(argReader, "ChequeNo");
        //    loItem.VoucherNo = GetValue<string>(argReader, "VoucherNo");
        //    loItem.PocketAmount = GetValue<string>(argReader, "PocketAmount");
        //    loItem.SubReferenceOne = GetValue<string>(argReader, "SubRef1");
        //    loItem.SubReferenceTwo = GetValue<string>(argReader, "SubRef2");
        //    loItem.SubReferenceThree = GetValue<string>(argReader, "SubRef3");
        //    loItem.PostStatus = GetValue<string>(argReader, "PostStatus");
        //    loItem.IntegrationStatus = Convert.ToInt16(GetValue<string>(argReader, "IntStatus"));
        //    loItem.CreatedBy = GetValue<string>(argReader, "CreatedBy");
        //    loItem.CreatedDateTime = GetValue<DateTime>(argReader, "CreatedTimeStamp");
        //    loItem.PostedBy = GetValue<string>(argReader, "PostedBy");
        //    loItem.PostedDateTime = GetValue<DateTime>(argReader, "PostedTimeStamp");
        //    loItem.IntegrationCode = GetValue<string>(argReader, "IntCode");
        //    loItem.GLCode = GetValue<string>(argReader, "GLCode");
        //    loItem.UpdatedBy = GetValue<string>(argReader, "UpdatedBy");
        //    loItem.UpdatedTime = GetValue<DateTime>(argReader, "UpdatedTime");
        //    loItem.Outstanding_Amount = MaxGeneric.clsGeneric.NullToString(GetStudentOutstandingAmount(GetValue<string>(argReader, "CreditRef")));
        //    loItem.Internal_Use = GetValue<string>(argReader, "Internal_Use");
        //    loItem.ControlAmt = GetValue<decimal>(argReader, "control_amt");

        //    try
        //    {                
        //        loItem.BankSlipID = GetValue<string>(argReader, "bankrecno");
        //        loItem.AllocatedAmount = GetSponserStuAllocateAmount(GetValue<string>(argReader, "CreditRef1"));
        //    }
        //    catch (Exception ex)
        //    {
        //        return loItem;
        //    }
        //    return loItem;
        //}

    public AccountsEn LoadObject(IDataReader argReader)
{
    AccountsEn loItem = new AccountsEn();

    loItem.TranssactionID = GetValue<int>(argReader, "TransID");
    loItem.TempTransCode = GetValue<string>(argReader, "TransTempCode");
    loItem.TransactionCode = GetValue<string>(argReader, "TransCode");
    loItem.CreditRef = GetValue<string>(argReader, "CreditRef");
    loItem.CreditRefOne = GetValue<string>(argReader, "CreditRef1");
    loItem.DebitRef = GetValue<string>(argReader, "DebitRef");
    loItem.DebitRefOne = GetValue<string>(argReader, "DebitRef1");
    loItem.Category = GetValue<string>(argReader, "Category");
    loItem.SubCategory = GetValue<string>(argReader, "SubCategory");
    loItem.TransType = GetValue<string>(argReader, "TransType");
    loItem.SubType = GetValue<string>(argReader, "SubType");
    loItem.SourceType = GetValue<string>(argReader, "SourceType");
    loItem.TransDate = GetValue<DateTime>(argReader, "TransDate");
    loItem.DueDate = GetValue<DateTime>(argReader, "DueDate");
    loItem.BatchCode = GetValue<string>(argReader, "BatchCode");
    loItem.BatchIntake = GetValue<string>(argReader, "BatchIntake");
    loItem.BatchDate = GetValue<DateTime>(argReader, "BatchDate");
    loItem.CrRefOne = GetValue<string>(argReader, "CrRef1");
    loItem.CrRefTwo = GetValue<string>(argReader, "CrRef2");
    loItem.Description = GetValue<string>(argReader, "Description");
    loItem.CurrencyUsed = GetValue<string>(argReader, "Currency");
    loItem.BatchTotal = GetValue<double>(argReader, "BatchTotal");
    loItem.TaxPercentage = GetValue<double>(argReader, "Tax");
    loItem.DiscountPercentage = GetValue<double>(argReader, "Discount");
    loItem.TaxAmount = GetValue<double>(argReader, "TaxAmount");
    loItem.DiscountAmount = GetValue<double>(argReader, "DiscountAmount");
    loItem.TransactionAmount = GetValue<double>(argReader, "TransAmount");
    loItem.PaidAmount = GetValue<double>(argReader, "PaidAmount");
    loItem.TransStatus = GetValue<string>(argReader, "TransStatus");
    loItem.TempAmount = GetValue<double>(argReader, "TempAmount");
    loItem.TempPaidAmount = GetValue<double>(argReader, "TempPaidAmount");
    loItem.PaymentMode = GetValue<string>(argReader, "PaymentMode");
    loItem.BankCode = GetValue<string>(argReader, "BankCode");
    loItem.PayeeName = GetValue<string>(argReader, "PayeeName");
    loItem.ChequeDate = GetValue<DateTime>(argReader, "ChequeDate");
    loItem.ChequeNo = GetValue<string>(argReader, "ChequeNo");
    loItem.VoucherNo = GetValue<string>(argReader, "VoucherNo");
    loItem.PocketAmount = GetValue<string>(argReader, "PocketAmount");
    loItem.SubReferenceOne = GetValue<string>(argReader, "SubRef1");
    loItem.SubReferenceTwo = GetValue<string>(argReader, "SubRef2");
    loItem.SubReferenceThree = GetValue<string>(argReader, "SubRef3");
    loItem.PostStatus = GetValue<string>(argReader, "PostStatus");
    loItem.IntegrationStatus = Convert.ToInt16(GetValue<string>(argReader, "IntStatus"));
    loItem.CreatedBy = GetValue<string>(argReader, "CreatedBy");
    loItem.CreatedDateTime = GetValue<DateTime>(argReader, "CreatedTimeStamp");
    loItem.PostedBy = GetValue<string>(argReader, "PostedBy");
    loItem.PostedDateTime = GetValue<DateTime>(argReader, "PostedTimeStamp");
    loItem.IntegrationCode = GetValue<string>(argReader, "IntCode");
    loItem.GLCode = GetValue<string>(argReader, "GLCode");
    loItem.UpdatedBy = GetValue<string>(argReader, "UpdatedBy");
    loItem.UpdatedTime = GetValue<DateTime>(argReader, "UpdatedTime");
    loItem.Outstanding_Amount = MaxGeneric.clsGeneric.NullToString(GetStudentOutstandingAmount(GetValue<string>(argReader, "CreditRef")));
    loItem.Internal_Use = GetValue<string>(argReader, "Internal_Use");

    //modified by farid 20072016

    if (loItem.Category == "Invoice" && loItem.SubType == "Sponsor")
    {
    }
    else
    {
        loItem.ControlAmt = MaxGeneric.clsGeneric.NullToDecimal(GetValue<decimal>(argReader, "control_amt"));
    }

    //end modified
    try
    {
        loItem.BankSlipID = GetValue<string>(argReader, "bankrecno");
        loItem.AllocatedAmount = GetSponserStuAllocateAmount(GetValue<string>(argReader, "CreditRef1"));
    }
    catch (Exception ex)
    {
        return loItem;
    }
    return loItem;
}


        /// <summary>
        /// Method to Load Accounts Entity
        /// </summary>
        /// <param name="argReader">Reader Object is an Input.</param>
        /// <returns>Returns Accounts Entity Object</returns>
        public AccountsEn LoadObjectMatricNo(IDataReader argReader)
        {
            AccountsEn loItem = new AccountsEn();
            loItem.CreditRef = GetValue<string>(argReader, "CreditRef");
            loItem.BatchCode = GetValue<string>(argReader, "BatchCode");

            return loItem;
        }
        /// <summary>
        /// Method to Load Loan Accounts Entity
        /// </summary>
        /// <param name="argReader">Reader Object is an Input.</param>
        /// <returns>Returns Accounts Entity Object</returns>
        public AccountsEn LoadLoanObject(IDataReader argReader)
        {
            AccountsEn loItem = new AccountsEn();
            loItem.TranssactionID = GetValue<int>(argReader, "TransID");
            loItem.TempTransCode = GetValue<string>(argReader, "TransTempCode");
            loItem.TransactionCode = GetValue<string>(argReader, "TransCode");
            loItem.CreditRef = GetValue<string>(argReader, "CreditRef");
            loItem.Category = GetValue<string>(argReader, "Category");
            loItem.TransType = GetValue<string>(argReader, "TransType");
            loItem.SubType = GetValue<string>(argReader, "SubType");
            loItem.TransDate = GetValue<DateTime>(argReader, "TransDate");
            loItem.DueDate = GetValue<DateTime>(argReader, "DueDate");
            loItem.BatchCode = GetValue<string>(argReader, "BatchCode");
            loItem.BatchIntake = GetValue<string>(argReader, "BatchIntake");
            loItem.BatchDate = GetValue<DateTime>(argReader, "BatchDate");
            loItem.Description = GetValue<string>(argReader, "Description");
            loItem.BatchTotal = GetValue<double>(argReader, "BatchTotal");
            loItem.TransactionAmount = GetValue<double>(argReader, "TransAmount");
            loItem.PaidAmount = GetValue<double>(argReader, "PaidAmount");
            loItem.TransStatus = GetValue<string>(argReader, "TransStatus");
            loItem.PaymentMode = GetValue<string>(argReader, "PaymentMode");
            loItem.BankCode = GetValue<string>(argReader, "BankCode");
            loItem.PayeeName = GetValue<string>(argReader, "PayeeName");
            loItem.ChequeDate = GetValue<DateTime>(argReader, "ChequeDate");
            loItem.ChequeNo = GetValue<string>(argReader, "ChequeNo");
            loItem.VoucherNo = GetValue<string>(argReader, "VoucherNo");
            loItem.PocketAmount = GetValue<string>(argReader, "PocketAmount");
            loItem.SubReferenceOne = GetValue<string>(argReader, "SubRef1");
            loItem.SubReferenceTwo = GetValue<string>(argReader, "SubRef2");
            loItem.PostStatus = GetValue<string>(argReader, "PostStatus");
            loItem.IntegrationStatus = Convert.ToInt16(GetValue<string>(argReader, "IntStatus"));
            loItem.CreatedBy = GetValue<string>(argReader, "CreatedBy");
            loItem.CreatedDateTime = GetValue<DateTime>(argReader, "createdtimestamp");
            loItem.PostedBy = GetValue<string>(argReader, "PostedBy");
            loItem.PostedDateTime = GetValue<DateTime>(argReader, "postedtimestamp");
            loItem.UpdatedBy = GetValue<string>(argReader, "UpdatedBy");
            loItem.UpdatedTime = GetValue<DateTime>(argReader, "UpdatedTime");
            return loItem;
        }
        /// <summary>
        /// Method to Load Accounts Entity
        /// </summary>
        /// <param name="argReader">Reader Object is an Input.</param>
        /// <returns>Returns Accounts Entity Object</returns>
        public AccountsEn LoadStudentLedgerObject(IDataReader argReader)
        {
            AccountsEn loItem = new AccountsEn();
            loItem.TransactionCode = GetValue<string>(argReader, "TransCode");
            loItem.TransDate = GetValue<DateTime>(argReader, "TransDate");
            loItem.Description = GetValue<string>(argReader, "Description");
            loItem.Debit = GetValue<double>(argReader, "Debit");
            loItem.Credit = GetValue<double>(argReader, "Credit");
            loItem.Category = GetValue<string>(argReader, "Category");
            loItem.TransactionAmount = GetValue<double>(argReader, "TransAmount");
            loItem.TransType = GetValue<string>(argReader, "TransType");
            loItem.BatchCode = GetValue<string>(argReader, "BatchCode");

            return loItem;
        }
        /// <summary>
        /// Method to Get Reader Value
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="argReader">Reader Object is an Input</param>
        /// <param name="argColNm">Column Name is an Input</param>
        /// <returns></returns>
        private static T GetValue<T>(IDataReader argReader, string argColNm)
        {
            if (!argReader.IsDBNull(argReader.GetOrdinal(argColNm)))
                return (T)argReader.GetValue(argReader.GetOrdinal(argColNm));
            else
                return default(T);
        }

        #endregion

        #region GetAutoNumber

        /// <summary>
        /// Method to Get AutoNumber
        /// </summary>
        /// <param name="Description">Description as Input</param>
        /// <returns>Returns AutoNumber</returns>
        public string GetAutoNumber(string Description)
        {
            string AutoNo = "";
            int CurNo = 0;
            int NoDigit = 0;
            int AutoCode = 0;
            int i = 0;
            string SqlStr;
            SqlStr = "select * from SAS_AutoNumber where SAAN_Des='" + Description + "'";

            try
            {
                IDataReader loReader = _DatabaseFactory.ExecuteReader(Helper.GetDataBaseType,
                        DataBaseConnectionString, SqlStr).CreateDataReader();

                if (loReader.Read())
                {
                    AutoCode = Convert.ToInt32(loReader["SAAN_Code"]);
                    CurNo = Convert.ToInt32(loReader["SAAN_CurNo"]) + 1;
                    NoDigit = Convert.ToInt32(loReader["SAAN_NoDigit"]);
                    AutoNo = Convert.ToString(loReader["SAAN_Prefix"]);
                    if (CurNo.ToString().Length < NoDigit)
                    {
                        while (i < NoDigit - CurNo.ToString().Length)
                        {
                            AutoNo = AutoNo + "0";
                            i = i + 1;
                        }
                        AutoNo = AutoNo + CurNo;
                    }
                    loReader.Close();

                }

                AutoNumberEn loItem = new AutoNumberEn();
                loItem.SAAN_Code = AutoCode;
                AutoNumberDAL cods = new AutoNumberDAL();
                cods.GetItem(loItem);

                loItem.SAAN_Code = Convert.ToInt32(AutoCode);
                loItem.SAAN_CurNo = CurNo;
                loItem.SAAN_AutoNo = AutoNo;


                cods.Update(loItem);


                return AutoNo;
            }

            catch (Exception ex)
            {
                Console.Write("Error in connection : " + ex.Message);
                return ex.ToString();
            }

        }

        #endregion

        #region InsertReceiptUserAction

        /// <summary>
        /// Method to Receipt History of Updation and Deltetion 
        /// </summary>
        /// <param name="argEn">Account Entity is an Input.</param>
        /// <returns>Returns Boolean</returns>
        public bool InsertReceiptUserAction(AccountsEn argEn)
        {
            bool lbRes = false;
            try
            {
                string sqlCmd = "INSERT INTO SAS_ReceiptHistory(SARH_Date,SARH_ReceiptNo,SARH_Updatedby,SARH_Deletedby) values(@SARH_Date,@SARH_ReceiptNo,@SARH_Updatedby,@SARH_Deletedby) ";

                if (!FormHelp.IsBlank(sqlCmd))
                {
                    DbCommand cmd = _DatabaseFactory.GetDbCommand(Helper.GetDataBaseType, sqlCmd, DataBaseConnectionString);
                    _DatabaseFactory.AddInParameter(ref cmd, "@SARH_Date", DbType.DateTime, Helper.DateConversion(argEn.UpdatedTime));
                    _DatabaseFactory.AddInParameter(ref cmd, "@SARH_ReceiptNo", DbType.String, argEn.BatchCode);
                    _DatabaseFactory.AddInParameter(ref cmd, "@SARH_Updatedby", DbType.String, argEn.UpdatedBy);
                    _DatabaseFactory.AddInParameter(ref cmd, "@SARH_Deletedby", DbType.String, argEn.DeletedBy);
                    _DbParameterCollection = cmd.Parameters;

                    int liRowAffected = _DatabaseFactory.ExecuteNonQuery(Helper.GetDataBaseType, cmd,
                        DataBaseConnectionString, sqlCmd, _DbParameterCollection);

                    if (liRowAffected > -1)
                        lbRes = true;
                    else
                        throw new Exception("Insertion Failed! No Row has been inserted...");
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return lbRes;
        }

        #endregion

        #region GetReceiptHistory

        /// <summary>
        /// Method to Get Receipt History
        /// </summary>
        /// <param name="argEn">Receipt History Properties</param>
        /// <returns>Returns List of Accounts</returns>
        public List<AccountsEn> GetReceiptHistory(string fromDate, string toDate)
        {
            List<AccountsEn> loListAccounts = new List<AccountsEn>();

            string sqlCmd = @"SELECT SARH_Date ,
                                    CASE WHEN SARH_Updatedby = '' THEN 'Deleted'
                                         ELSE 'Modified'
                                    END AS 'Action' ,
                                    CASE WHEN SARH_Updatedby = '' THEN SARH_Deletedby
                                         ELSE SARH_Updatedby
                                    END AS 'User' ,
                                    SARH_ReceiptNo
                            FROM SAS_ReceiptHistory 
                            WHERE CONVERT(DATE,SARH_Date) 
                            between CONVERT(DATE,'" + fromDate + "') And CONVERT(DATE,'" + toDate + "') order by SARH_Date ";

            try
            {
                if (!FormHelp.IsBlank(sqlCmd))
                {
                    using (IDataReader loReader = _DatabaseFactory.ExecuteReader(Helper.GetDataBaseType,
                        DataBaseConnectionString, sqlCmd).CreateDataReader())
                    {
                        while (loReader.Read())
                        {
                            AccountsEn loItem = new AccountsEn();
                            loItem.UpdatedTime = GetValue<DateTime>(loReader, "SARH_Date");
                            loItem.TransactionCode = GetValue<String>(loReader, "Action");
                            loItem.UpdatedBy = GetValue<String>(loReader, "User");
                            loItem.BatchCode = GetValue<String>(loReader, "SARH_ReceiptNo");
                            loListAccounts.Add(loItem);
                        }
                        loReader.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return loListAccounts;
        }

        #endregion

        #region StudentLoanInsert

        /// <summary>
        /// Method to Insert Student Loan
        /// </summary>
        /// <param name="argEn">Accounts Entity is the Input</param>
        /// <returns>Returns BatchCode</returns>
        public string StudentLoanInsert(AccountsEn argEn)
        {
            StudentEn argStudent = new StudentEn();
            argEn.BatchCode = GetAutoNumber("Batch");

            InsertStudentLoan(argEn, argStudent);
            Insert(argEn);
            return argEn.BatchCode;
        }

        #endregion

        #region InsertStudentLoan

        /// <summary>
        /// Method to Insert Student Loan
        /// </summary>
        /// <param name="argEn">Accounts Entity is the Input</param>
        /// <returns>Returns Boolean</returns>
        public bool InsertStudentLoan(AccountsEn argEn, StudentEn argStudent)
        {
            bool lbRes = false;
            string sqlCmd;
            List<AccountsEn> StuTransList = new List<AccountsEn>();
            try
            {
                sqlCmd = "INSERT INTO SAS_StudentLoan(TransTempCode,TransCode,CreditRef,Category,TransType,SubType," +
                "TransDate,DueDate,BatchCode,BatchIntake,BatchDate,Description,BatchTotal,TransAmount,PaidAmount,TransStatus," +
                "PaymentMode,BankCode,PayeeName,ChequeDate,ChequeNo,VoucherNo,PocketAmount,SubRef1,SubRef2,PostStatus,IntStatus," +
                "CreatedBy,CreatedTimeStamp,PostedBy,PostedTimeStamp,UpdatedBy,UpdatedTime) VALUES(@TransTempCode,@TransCode,@CreditRef," +
                    "@Category,@TransType,@SubType,@TransDate,@DueDate,@BatchCode,@BatchIntake,@BatchDate,@Description,@BatchTotal," +
                    "@TransAmount,@PaidAmount,@TransStatus,@PaymentMode,@BankCode,@PayeeName,@ChequeDate,@ChequeNo,@VoucherNo,@PocketAmount," +
                    "@SubRef1,@SubRef2,@PostStatus,@IntStatus,@CreatedBy,@CreatedDateTime,@PostedBy,@PostedDateTime,@UpdatedBy,@UpdatedTime)";

                if (!FormHelp.IsBlank(sqlCmd))
                {
                    if (argEn.PostStatus == "Ready")
                    {
                        if (argEn.Category == "Loan")
                        {
                            argEn.TempTransCode = GetAutoNumber("TSL");
                        }
                        else if (argEn.Category == "Receipt")
                        {
                            argEn.TempTransCode = GetAutoNumber("TRT");
                        }

                    }
                    else if (argEn.PostStatus == "Posted")
                    {
                        if (argEn.Category == "Loan")
                        {
                            argEn.TransactionCode = GetAutoNumber("SL");
                        }
                        else if (argEn.Category == "Receipt")
                        {
                            argEn.TransactionCode = GetAutoNumber("Rcpt");
                        }
                    }

                    DbCommand cmd = _DatabaseFactory.GetDbCommand(Helper.GetDataBaseType, sqlCmd, DataBaseConnectionString);
                    _DatabaseFactory.AddInParameter(ref cmd, "@TransTempCode", DbType.String, argEn.TempTransCode);
                    _DatabaseFactory.AddInParameter(ref cmd, "@TransCode", DbType.String, argEn.TransactionCode);
                    _DatabaseFactory.AddInParameter(ref cmd, "@CreditRef", DbType.String, argEn.CreditRef);
                    _DatabaseFactory.AddInParameter(ref cmd, "@Category", DbType.String, argEn.Category);
                    _DatabaseFactory.AddInParameter(ref cmd, "@TransType", DbType.String, argEn.TransType);
                    _DatabaseFactory.AddInParameter(ref cmd, "@SubType", DbType.String, argEn.SubType);  
                    _DatabaseFactory.AddInParameter(ref cmd, "@TransDate", DbType.DateTime, Helper.DateConversion(argEn.TransDate));         
                    //_DatabaseFactory.AddInParameter(ref cmd, "@TransDate", DbType.String, argEn.TransDate.ToString("MM/dd/yyyy hh:mm:ss"));                    
                    _DatabaseFactory.AddInParameter(ref cmd, "@DueDate", DbType.DateTime, Helper.DateConversion(argEn.DueDate));         
                    //_DatabaseFactory.AddInParameter(ref cmd, "@DueDate", DbType.String, argEn.DueDate.ToString("MM/dd/yyyy hh:mm:ss"));              
                    _DatabaseFactory.AddInParameter(ref cmd, "@BatchCode", DbType.String, argEn.BatchCode);
                    _DatabaseFactory.AddInParameter(ref cmd, "@BatchIntake", DbType.String, argEn.BatchIntake);   
                    _DatabaseFactory.AddInParameter(ref cmd, "@BatchDate", DbType.DateTime, Helper.DateConversion(argEn.BatchDate)); 
                    //_DatabaseFactory.AddInParameter(ref cmd, "@BatchDate", DbType.String, argEn.BatchDate.ToString("MM/dd/yyyy hh:mm:ss"));
                    _DatabaseFactory.AddInParameter(ref cmd, "@Description", DbType.String, argEn.Description);
                    _DatabaseFactory.AddInParameter(ref cmd, "@BatchTotal", DbType.Double, argEn.BatchTotal);
                    _DatabaseFactory.AddInParameter(ref cmd, "@TransAmount", DbType.Double, argEn.TransactionAmount);
                    _DatabaseFactory.AddInParameter(ref cmd, "@PaidAmount", DbType.Double, argEn.PaidAmount);
                    _DatabaseFactory.AddInParameter(ref cmd, "@TransStatus", DbType.String, argEn.TransStatus);
                    _DatabaseFactory.AddInParameter(ref cmd, "@PaymentMode", DbType.String, argEn.PaymentMode);
                    _DatabaseFactory.AddInParameter(ref cmd, "@BankCode", DbType.String, argEn.BankCode);
                    _DatabaseFactory.AddInParameter(ref cmd, "@PayeeName", DbType.String, argEn.PayeeName);                    
                    _DatabaseFactory.AddInParameter(ref cmd, "@ChequeDate", DbType.DateTime, Helper.DateConversion(argEn.ChequeDate)); 
                    //_DatabaseFactory.AddInParameter(ref cmd, "@ChequeDate", DbType.String, argEn.ChequeDate.ToString("MM/dd/yyyy hh:mm:ss"));
                    _DatabaseFactory.AddInParameter(ref cmd, "@ChequeNo", DbType.String, argEn.ChequeNo);
                    _DatabaseFactory.AddInParameter(ref cmd, "@VoucherNo", DbType.String, argEn.VoucherNo);
                    _DatabaseFactory.AddInParameter(ref cmd, "@PocketAmount", DbType.String, argEn.PocketAmount);
                    _DatabaseFactory.AddInParameter(ref cmd, "@SubRef1", DbType.String, argEn.SubReferenceOne);
                    _DatabaseFactory.AddInParameter(ref cmd, "@SubRef2", DbType.String, argEn.SubReferenceTwo);
                    _DatabaseFactory.AddInParameter(ref cmd, "@PostStatus", DbType.String, argEn.PostStatus);
                    _DatabaseFactory.AddInParameter(ref cmd, "@IntStatus", DbType.Int32, argEn.IntegrationStatus);
                    _DatabaseFactory.AddInParameter(ref cmd, "@CreatedBy", DbType.String, argEn.CreatedBy);
                    _DatabaseFactory.AddInParameter(ref cmd, "@CreatedDateTime", DbType.DateTime, Helper.DateConversion(argEn.CreatedDateTime));                     
                    //_DatabaseFactory.AddInParameter(ref cmd, "@CreatedDateTime", DbType.String, argEn.CreatedDateTime.ToString("MM/dd/yyyy hh:mm:ss"));
                    _DatabaseFactory.AddInParameter(ref cmd, "@PostedBy", DbType.String, argEn.PostedBy);                       
                    _DatabaseFactory.AddInParameter(ref cmd, "@PostedDateTime", DbType.DateTime, Helper.DateConversion(argEn.PostedDateTime));
                    //_DatabaseFactory.AddInParameter(ref cmd, "@PostedDateTime", DbType.String, argEn.PostedDateTime.ToString("MM/dd/yyyy hh:mm:ss"));
                    _DatabaseFactory.AddInParameter(ref cmd, "@UpdatedBy", DbType.String, argEn.UpdatedBy);                          
                    _DatabaseFactory.AddInParameter(ref cmd, "@UpdatedTime", DbType.DateTime, Helper.DateConversion(argEn.UpdatedTime));
                    //_DatabaseFactory.AddInParameter(ref cmd, "@UpdatedTime", DbType.String, argEn.UpdatedTime.ToString("MM/dd/yyyy hh:mm:ss"));
                    _DbParameterCollection = cmd.Parameters;

                    int liRowAffected = _DatabaseFactory.ExecuteNonQuery(Helper.GetDataBaseType, cmd,
                        DataBaseConnectionString, sqlCmd, _DbParameterCollection);

                    if (liRowAffected > -1)
                    {
                        lbRes = true;
                        double loanAmount = 0;
                        loanAmount = argEn.TransactionAmount;
                        string matricNo = argEn.CreditRef;
                        if (argEn.PostStatus == "Posted" && argEn.Category == "Loan")
                        {
                            StudentEn loItem = new StudentEn();
                            loItem = GetStudentOutstanding(argEn.CreditRef.ToString());
                            if (string.IsNullOrEmpty(loItem.MatricNo) == false)
                            {
                                // Update LoanAmount in the Student Outstanding table
                                if (loanAmount > 0)
                                {
                                    UpdateStudentOutstanding(matricNo, loanAmount);
                                }

                            }
                            else
                            {
                                /* Inserts new record */
                                argStudent.LoanAmount = loanAmount;
                                InsertStudentOutstanding(argStudent);
                            }
                        }
                    }
                    else
                    {
                        
                        throw new Exception("Insertion Failed! No Row has been inserted...");
                    }
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return lbRes;
        }

        #endregion

        #region StudentLoanUpdate

        /// <summary>
        /// Method to Update StudentBatch
        /// </summary>
        /// <param name="argEn">Accounts Entity is the Input</param>
        /// <param name="argList">List of Student Entity is the Input</param>
        /// <returns>Returns BatchCode</returns>
        public string StudentLoanUpdate(AccountsEn argEn, StudentEn argStudent)
        {
            try
            {
                bool flag = false;

                DeleteLoan(argEn);
                InsertStudentLoan(argEn, argStudent);
                //insert acc

                //added by Hafiz Roslan @ 4/2/2016
                //added update function for updating process at sas_accounts - start
                flag = CheckStudAvailableOrNot(argEn.CreditRef, argEn.BatchCode);

                if (flag == true)
                {
                    try
                    {
                        //Update Account Details - Start
                        _Update(argEn);
                        //Update Account Details - Stop
                    }
                    catch (Exception ex)
                    {
                        MaxModule.Helper.LogError(ex.Message);
                        throw ex;
                    }
                }
                else
                {
                    try
                    {
                        //Insert Account Details - Start                    
                        Insert(argEn);
                        //Insert Account Details - End
                    }
                    catch (Exception ex)
                    {
                        MaxModule.Helper.LogError(ex.Message);
                        throw ex;
                    }
                }
                //added update function for updating process at sas_accounts - stop

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return argEn.BatchCode;
        }

        #endregion

        #region UpdateStudentOutstanding

        public bool UpdateStudentOutstanding(string matricNo, double loanAmount)
        {
            bool lbRes = false;
            string sqlCmd = @"Update SAS_StudentOutstanding SET SASO_LoanAmount= ISNULL(SASO_LoanAmount,0) + " + loanAmount + " where SASI_MatricNo ='" + matricNo + "'";

            try
            {
                if (!FormHelp.IsBlank(sqlCmd))
                {
                    int liRowAffected = _DatabaseFactory.ExecuteSqlStatement(Helper.GetDataBaseType,
                                DataBaseConnectionString, sqlCmd);

                    if (liRowAffected > -1)
                        lbRes = true;
                    else
                        throw new Exception("Updation Failed! No Row has been updated...");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return lbRes;

        }

        #endregion

        #region InsertStudentOutstanding
        //modified by Hafiz @ 27/5/2016

        public bool InsertStudentOutstanding(StudentEn argStudent)
        {
            bool lbRes = false;
            string sqlCmd = "";

            try
            {
                sqlCmd = "INSERT INTO SAS_StudentOutstanding(SASI_MatricNo,SASI_Name,SASI_PgId,SASI_CurSem,SASI_CurSemYr,SASO_Outstandingamt,SASO_IsReleased)VALUES(@SASI_MatricNo,@SASI_Name,@SASI_PgId,@SASI_CurSem,@SASI_CurSemYr,@SASO_Outstandingamt,@SASO_IsReleased)";

                if (!FormHelp.IsBlank(sqlCmd))
                {
                    DbCommand cmd = _DatabaseFactory.GetDbCommand(Helper.GetDataBaseType, sqlCmd, DataBaseConnectionString);
                    _DatabaseFactory.AddInParameter(ref cmd, "@SASI_MatricNo", DbType.String, argStudent.MatricNo);
                    _DatabaseFactory.AddInParameter(ref cmd, "@SASI_Name", DbType.String, argStudent.StudentName);
                    _DatabaseFactory.AddInParameter(ref cmd, "@SASI_PgId", DbType.String, argStudent.ProgramID);
                    _DatabaseFactory.AddInParameter(ref cmd, "@SASI_CurSem", DbType.Int16, argStudent.CurrentSemester);
                    _DatabaseFactory.AddInParameter(ref cmd, "@SASI_CurSemYr", DbType.String, argStudent.CurretSemesterYear);
                    _DatabaseFactory.AddInParameter(ref cmd, "@SASO_Outstandingamt", DbType.Double, argStudent.OutstandingAmount);
                    _DatabaseFactory.AddInParameter(ref cmd, "@SASO_IsReleased", DbType.Int16, 0);
                    _DbParameterCollection = cmd.Parameters;

                    int liRowAff = _DatabaseFactory.ExecuteNonQuery(Helper.GetDataBaseType, cmd,
                        DataBaseConnectionString, sqlCmd, _DbParameterCollection);

                    if (liRowAff > -1)
                        lbRes = true;
                    else
                        throw new Exception("Insertion Failed! No Row has been inserted...");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return lbRes;
        }

        #endregion

        #region GetLoanTransactions

        /// <summary>
        /// Method to Get Transactions
        /// </summary>
        /// <param name="argEn">Accounts Entity is the Input.BatchCode,Category,PostStatus and SubType are the Input Property.</param>
        /// <returns>Returns List of Accounts.</returns>
        public List<AccountsEn> GetLoanTransactions(AccountsEn argEn)
        {
            List<AccountsEn> loListAccounts = new List<AccountsEn>();
            argEn.BatchCode = argEn.BatchCode.Replace("*", "%");
            string sqlCmd = "SELECT DISTINCT BatchCode FROM SAS_StudentLoan WHERE Category='" +
                          argEn.Category + "' AND PostStatus='" + argEn.PostStatus + "' and SubType ='" + argEn.SubType + "'";
            if (argEn.BatchCode.Length != 0) sqlCmd = sqlCmd + " AND BatchCode LIKE '" + argEn.BatchCode + "'";

            try
            {
                if (!FormHelp.IsBlank(sqlCmd))
                {
                    using (IDataReader loReader = _DatabaseFactory.ExecuteReader(Helper.GetDataBaseType,
                        DataBaseConnectionString, sqlCmd).CreateDataReader())
                    {
                        while (loReader.Read())
                        {
                            argEn.BatchCode = GetValue<string>(loReader, "BatchCode");
                            loListAccounts.Add(GetLoanItem(argEn));
                        }
                        loReader.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return loListAccounts;
        }

        #endregion

        #region GetLoanItem

        /// <summary>
        /// Method to Get an Accounts Item
        /// </summary>
        /// <param name="argEn">Accounts Entity is the Input.BatchCode is the Input Property</param>
        /// <returns>Returns an Accounts Item</returns>
        public AccountsEn GetLoanItem(AccountsEn argEn)
        {
            AccountsEn loItem = new AccountsEn();
            string sqlCmd = "Select * FROM SAS_StudentLoan WHERE BatchCode = @BatchCode";

            try
            {
                if (!FormHelp.IsBlank(sqlCmd))
                {
                    DbCommand cmd = _DatabaseFactory.GetDbCommand(Helper.GetDataBaseType, sqlCmd, DataBaseConnectionString);
                    _DatabaseFactory.AddInParameter(ref cmd, "@BatchCode", DbType.String, argEn.BatchCode);
                    _DbParameterCollection = cmd.Parameters;

                    using (IDataReader loReader = _DatabaseFactory.GetIDataReader(Helper.GetDataBaseType, cmd,
                        DataBaseConnectionString, sqlCmd, _DbParameterCollection).CreateDataReader())
                    {
                        if (loReader != null)
                        {
                            loReader.Read();
                            loItem = LoadLoanObject(loReader);

                        }
                        loReader.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return loItem;
        }

        #endregion

        #region  SponsorInvoiceInsert

        /// <summary>
        /// Method to Insert StudentBatch
        /// </summary>
        /// <param name="argEn">Accounts Entity is the Input</param>
        /// <param name="argList">List of Student Entity is the Input</param>
        /// <returns>Returns BatchCode</returns>
        //public string SponsorInvoiceInsert(AccountsEn argEn, List<StudentEn> argList)
        //{
        //    int i = 0;
        //    argEn.BatchCode = GetAutoNumber("Batch");

        //    for (i = 0; i < argList.Count; i++)
        //    {
        //        argEn.CreditRef = argList[i].MatricNo;
        //        argEn.CreditRefOne = argList[i].SponsorCode;
        //        InsertSponserInvoice(argEn);
        //    }

        //    return argEn.BatchCode;
        //}
        public string SponsorInvoiceInsert(AccountsEn argEn, List<StudentEn> argList, bool isAutoDetails = false)
        {
            try
            {
                int i = 0;
                argEn.BatchCode = GetAutoNumber("Batch");
                if (isAutoDetails)
                {
                    List<string> getStudent = argList.Select(p => p.MatricNo).Distinct().ToList();

                    foreach (var stu in getStudent)
                    {
                        List<StudentEn> newArgList = new List<StudentEn>();
                        newArgList = argList.Where(p => p.MatricNo == stu).ToList();
                        List<AccountsDetailsEn> newlistAccDetails = new List<AccountsDetailsEn>();
                        foreach (var data in newArgList)
                        {
                            AccountsDetailsEn newAccDetails = new AccountsDetailsEn();
                            newAccDetails.TransactionAmount = data.TransactionAmount;
                            newAccDetails.ReferenceCode = data.ReferenceCode;
                            newAccDetails.TaxAmount = data.TaxAmount;
                            newAccDetails.PostStatus = argEn.PostStatus;
                            newAccDetails.TransStatus = argEn.TransStatus;
                            newAccDetails.Priority = data.Priority;
                            newAccDetails.Internal_Use = data.Internal_Use;
                            newAccDetails.ReferenceOne = data.Description;
                            newAccDetails.ReferenceThree = data.MatricNo;
                            newlistAccDetails.Add(newAccDetails);
                        }
                        argEn.AccountDetailsList = newlistAccDetails;
                        argEn.CreditRef = stu;// argList[i].MatricNo;
                        argEn.CreditRefOne = argList[0].SponsorCode;
                        argEn.Internal_Use = "auto";
                        InsertSponserInvoice(argEn);
                    }
                }
                else
                {
                    for (i = 0; i < argList.Count; i++)
                    {
                        argEn.CreditRef = argList[i].MatricNo;
                        argEn.CreditRefOne = argList[i].SponsorCode;
                        InsertSponserInvoice(argEn);
                    }
                }

                return argEn.BatchCode;
            }
            catch (Exception ex)
            {
                MaxModule.Helper.LogError(ex.Message);
                throw ex;
            }
        }

        #endregion

        #region SponsorInvoiceUpdate

        /// <summary>
        /// Method to Update StudentBatch
        /// </summary>
        /// <param name="argEn">Accounts Entity is the Input</param>
        /// <param name="argList">List of Student Entity is the Input</param>
        /// <returns>Returns BatchCode</returns>
        //public string SponsorInvoiceUpdate(AccountsEn argEn, List<StudentEn> argList)
        //{
        //    int i = 0;
        //    SponsorInvoiceDelete(argEn);
        //    for (i = 0; i < argList.Count; i++)
        //    {
        //        argEn.CreditRef = argList[i].MatricNo;
        //        argEn.CreditRefOne = argList[i].SponsorCode;
        //        InsertSponserInvoice(argEn);
        //    }
        //    return argEn.BatchCode;
        //}
        public string SponsorInvoiceUpdate(AccountsEn argEn, List<StudentEn> argList, bool isAutoDetails = false)
        {
            int i = 0;
            SponsorInvoiceDelete(argEn);
            if (isAutoDetails)
            {
                List<string> getStudent = argList.Select(p => p.MatricNo).Distinct().ToList();

                foreach (var stu in getStudent)
                {
                    List<StudentEn> newArgList = new List<StudentEn>();
                    newArgList = argList.Where(p => p.MatricNo == stu).ToList();
                    List<AccountsDetailsEn> newlistAccDetails = new List<AccountsDetailsEn>();
                    foreach (var data in newArgList)
                    {
                        AccountsDetailsEn newAccDetails = new AccountsDetailsEn();
                        newAccDetails.TransactionAmount = data.TransactionAmount;
                        newAccDetails.ReferenceCode = data.ReferenceCode;
                        newAccDetails.TaxAmount = data.TaxAmount;
                        newAccDetails.PostStatus = argEn.PostStatus;
                        newAccDetails.TransStatus = argEn.TransStatus;
                        newAccDetails.Priority = data.Priority;
                        newAccDetails.Internal_Use = data.Internal_Use;
                        newAccDetails.ReferenceThree = data.MatricNo;
                        newlistAccDetails.Add(newAccDetails);
                    }
                    argEn.AccountDetailsList = newlistAccDetails;
                    argEn.CreditRef = stu;// argList[i].MatricNo;
                    argEn.CreditRefOne = argList[0].SponsorCode;
                    argEn.Internal_Use = "auto";
                    InsertSponserInvoice(argEn);
                }
            }
            else
            {
                for (i = 0; i < argList.Count; i++)
                {
                    argEn.CreditRef = argList[i].MatricNo;
                    argEn.CreditRefOne = argList[i].SponsorCode;

                    InsertSponserInvoice(argEn);
                }
            }
            return argEn.BatchCode;
        }

        #endregion

        #region InsertSponserInvoice

        /// <summary>
        /// Method to Insert Accounts
        /// </summary>
        /// <param name="argEn">Accounts Entity is the Input</param>
        /// <returns>Returns Boolean</returns>
        public bool InsertSponserInvoice(AccountsEn argEn)
        {
            bool lbRes = false;
            string sqlCmd;
            List<AccountsEn> StuTransList = new List<AccountsEn>();
            try
            {
                sqlCmd = "INSERT INTO SAS_SponsorInvoice(TransTempCode,TransCode,CreditRef,CreditRef1," +
                "DebitRef,DebitRef1,Category,SubCategory,TransType,SubType,SourceType,TransDate,DueDate,BatchCode,BatchIntake,BatchDate,"
                + " CrRef1,CrRef2,Description,Currency,BatchTotal,Tax,Discount,TaxAmount,DiscountAmount,TransAmount," +
                "PaidAmount,TransStatus,TempAmount,TempPaidAmount,PaymentMode,BankCode,PayeeName,ChequeDate,ChequeNo,VoucherNo,"
                + "PocketAmount,SubRef1,SubRef2,SubRef3,PostStatus,IntStatus,CreatedBy,CreatedTimeStamp,PostedBy,PostedTimeStamp,IntCode,GLCode,UpdatedBy,UpdatedTime,sasr_code, Internal_Use)"
                + "VALUES (@TransTempCode,@TransCode,@CreditRef,@CreditRef1,@DebitRef,@DebitRef1,@Category,@SubCategory,@TransType,@SubType,@SourceType,"
               + " @TransDate,@DueDate,@BatchCode,@BatchIntake,@BatchDate,@CrRef1,@CrRef2,@Description,@Currency,@BatchTotal,@Tax,@Discount,@TaxAmount,"
                + "@DiscountAmount,@TransAmount,@PaidAmount,@TransStatus,@TempAmount,@TempPaidAmount,@PaymentMode,@BankCode,@PayeeName,@ChequeDate,"
                + "@ChequeNo,@VoucherNo,@PocketAmount,@SubRef1,@SubRef2,@SubRef3,@PostStatus,@IntStatus,@CreatedBy,@CreatedDateTime,@PostedBy,"
                + "@PostedDateTime,@IntCode,@GLCode,@UpdatedBy,@UpdatedTime,@SponsorId, @Internal_Use);" +
                "select max(transid) from SAS_SponsorInvoice;";

                if (!FormHelp.IsBlank(sqlCmd))
                {
                    if (argEn.PostStatus == "Ready")
                    {
                        // Difine a Autocode for Sponsor Invoice
                        argEn.TempTransCode = GetAutoNumber("TSPI");

                    }
                    else if (argEn.PostStatus == "Posted")
                    {
                        // Difine a Autocode for Sponsor Invoice
                        argEn.TransactionCode = GetAutoNumber("SPI");
                    }

                    DbCommand cmd = _DatabaseFactory.GetDbCommand(Helper.GetDataBaseType, sqlCmd, DataBaseConnectionString);
                    _DatabaseFactory.AddInParameter(ref cmd, "@TransTempCode", DbType.String, argEn.TempTransCode);
                    _DatabaseFactory.AddInParameter(ref cmd, "@TransCode", DbType.String, argEn.TransactionCode);
                    _DatabaseFactory.AddInParameter(ref cmd, "@CreditRef", DbType.String, argEn.CreditRef);
                    _DatabaseFactory.AddInParameter(ref cmd, "@CreditRef1", DbType.String, argEn.CreditRefOne);
                    _DatabaseFactory.AddInParameter(ref cmd, "@DebitRef", DbType.String, argEn.DebitRef);
                    _DatabaseFactory.AddInParameter(ref cmd, "@DebitRef1", DbType.String, argEn.DebitRefOne);
                    _DatabaseFactory.AddInParameter(ref cmd, "@Category", DbType.String, argEn.Category);
                    _DatabaseFactory.AddInParameter(ref cmd, "@SubCategory", DbType.String, argEn.SubCategory);
                    _DatabaseFactory.AddInParameter(ref cmd, "@TransType", DbType.String, argEn.TransType);
                    _DatabaseFactory.AddInParameter(ref cmd, "@SubType", DbType.String, argEn.SubType);
                    _DatabaseFactory.AddInParameter(ref cmd, "@SourceType", DbType.String, argEn.SourceType);
                    _DatabaseFactory.AddInParameter(ref cmd, "@TransDate", DbType.DateTime, Helper.DateConversion(argEn.TransDate));
                    _DatabaseFactory.AddInParameter(ref cmd, "@DueDate", DbType.DateTime, Helper.DateConversion(argEn.DueDate));
                    _DatabaseFactory.AddInParameter(ref cmd, "@BatchCode", DbType.String, argEn.BatchCode);
                    _DatabaseFactory.AddInParameter(ref cmd, "@BatchDate", DbType.DateTime, Helper.DateConversion(argEn.BatchDate));
                    _DatabaseFactory.AddInParameter(ref cmd, "@CrRef1", DbType.String, argEn.CrRefOne);
                    _DatabaseFactory.AddInParameter(ref cmd, "@CrRef2", DbType.String, argEn.CrRefTwo);
                    _DatabaseFactory.AddInParameter(ref cmd, "@Description", DbType.String, argEn.Description);
                    _DatabaseFactory.AddInParameter(ref cmd, "@Currency", DbType.String, argEn.CurrencyUsed);
                    _DatabaseFactory.AddInParameter(ref cmd, "@BatchTotal", DbType.Double, clsGeneric.NullToDecimal(argEn.BatchTotal));
                    _DatabaseFactory.AddInParameter(ref cmd, "@Tax", DbType.Double, clsGeneric.NullToDecimal(argEn.TaxPercentage));
                    _DatabaseFactory.AddInParameter(ref cmd, "@Discount", DbType.Double, clsGeneric.NullToDecimal(argEn.DiscountPercentage));
                    _DatabaseFactory.AddInParameter(ref cmd, "@TaxAmount", DbType.Double, clsGeneric.NullToDecimal(argEn.TaxAmount));
                    _DatabaseFactory.AddInParameter(ref cmd, "@DiscountAmount", DbType.Double, clsGeneric.NullToDecimal(argEn.DiscountAmount));
                    _DatabaseFactory.AddInParameter(ref cmd, "@TransAmount", DbType.Double, clsGeneric.NullToDecimal(argEn.TransactionAmount));
                    _DatabaseFactory.AddInParameter(ref cmd, "@PaidAmount", DbType.Double, clsGeneric.NullToDecimal(argEn.PaidAmount));
                    _DatabaseFactory.AddInParameter(ref cmd, "@TransStatus", DbType.String, argEn.TransStatus);
                    _DatabaseFactory.AddInParameter(ref cmd, "@TempAmount", DbType.Double, clsGeneric.NullToDecimal(argEn.TempAmount));
                    _DatabaseFactory.AddInParameter(ref cmd, "@TempPaidAmount", DbType.Double, clsGeneric.NullToDecimal(argEn.TempPaidAmount));
                    _DatabaseFactory.AddInParameter(ref cmd, "@PaymentMode", DbType.String, argEn.PaymentMode);
                    _DatabaseFactory.AddInParameter(ref cmd, "@BankCode", DbType.String, argEn.BankCode);
                    _DatabaseFactory.AddInParameter(ref cmd, "@PayeeName", DbType.String, argEn.PayeeName);
                    _DatabaseFactory.AddInParameter(ref cmd, "@ChequeDate", DbType.DateTime, Helper.DateConversion(argEn.ChequeDate));
                    _DatabaseFactory.AddInParameter(ref cmd, "@ChequeNo", DbType.String, argEn.ChequeNo);
                    _DatabaseFactory.AddInParameter(ref cmd, "@VoucherNo", DbType.String, argEn.VoucherNo);
                    _DatabaseFactory.AddInParameter(ref cmd, "@PocketAmount", DbType.String, argEn.PocketAmount);
                    _DatabaseFactory.AddInParameter(ref cmd, "@SubRef1", DbType.String, argEn.SubReferenceOne);
                    _DatabaseFactory.AddInParameter(ref cmd, "@SubRef2", DbType.String, argEn.SubReferenceTwo);
                    _DatabaseFactory.AddInParameter(ref cmd, "@SubRef3", DbType.String, argEn.SubReferenceThree);
                    _DatabaseFactory.AddInParameter(ref cmd, "@PostStatus", DbType.String, argEn.PostStatus);
                    _DatabaseFactory.AddInParameter(ref cmd, "@IntStatus", DbType.Int32, argEn.IntegrationStatus);
                    _DatabaseFactory.AddInParameter(ref cmd, "@CreatedBy", DbType.String, argEn.CreatedBy);
                    _DatabaseFactory.AddInParameter(ref cmd, "@CreatedDateTime", DbType.DateTime, Helper.DateConversion(argEn.CreatedDateTime));
                    _DatabaseFactory.AddInParameter(ref cmd, "@PostedBy", DbType.String, argEn.PostedBy);
                    _DatabaseFactory.AddInParameter(ref cmd, "@PostedDateTime", DbType.DateTime, Helper.DateConversion(argEn.PostedDateTime));
                    _DatabaseFactory.AddInParameter(ref cmd, "@IntCode", DbType.String, argEn.IntegrationCode);
                    _DatabaseFactory.AddInParameter(ref cmd, "@GLCode", DbType.String, argEn.GLCode);
                    _DatabaseFactory.AddInParameter(ref cmd, "@UpdatedBy", DbType.String, argEn.UpdatedBy);
                    _DatabaseFactory.AddInParameter(ref cmd, "@UpdatedTime", DbType.DateTime, Helper.DateConversion(argEn.UpdatedTime));
                    _DatabaseFactory.AddInParameter(ref cmd, "@BatchIntake", DbType.String, argEn.BatchIntake);
                    _DatabaseFactory.AddInParameter(ref cmd, "@SponsorId", DbType.String, argEn.SponsorID);
                    _DatabaseFactory.AddInParameter(ref cmd, "@Internal_Use", DbType.String, argEn.Internal_Use);
                    _DbParameterCollection = cmd.Parameters;


                    int lb = clsGeneric.NullToInteger(_DatabaseFactory.ExecuteScalarCommand(Helper.GetDataBaseType, cmd,
                        DataBaseConnectionString, sqlCmd, _DbParameterCollection));
                    //Inserting AccountDetails Table
                    if (argEn.AccountDetailsList != null)
                    {
                        if (argEn.AccountDetailsList.Count != 0)
                        {
                            AccountsDetailsDAL loDS = new AccountsDetailsDAL();
                            for (int i = 0; i < argEn.AccountDetailsList.Count; i++)
                            {
                                argEn.AccountDetailsList[i].TransactionID = lb;
                                argEn.AccountDetailsList[i].TransTempCode = argEn.TempTransCode;
                                argEn.AccountDetailsList[i].TransactionCode = argEn.TransactionCode;
                                loDS.InsertSponsorInvoiceDetails(argEn.AccountDetailsList[i]);
                            }
                        }
                    }
                    //if (lb > 0)
                    //{
                    //    if (argEn.Category == "Invoice" && argEn.SubType == "Sponsor")
                    //    {
                    //        //updating sponsor invoice
                    //        UpdateSponsorInvoice(argEn);
                    //    }
                    //    else
                    //    {
                    //        throw new Exception("Insertion Failed! No Row has been inserted...");
                    //    }
                    //}

                }

            }
            catch (Exception ex)
            {
                throw ex;
            }

            return lbRes;
        }

        #endregion

        #region SponsorInvoiceDelete

        /// <summary>
        /// Method to Delete SponsorInvoice
        /// </summary>
        /// <param name="argEn">Accounts Entity is the Input.BatchCode is an Input Property</param>
        /// <returns>Returns a Boolean</returns> 
        public bool SponsorInvoiceDelete(AccountsEn argEn)
        {
            bool lbRes = false;
            List<AccountsEn> loEnList = new List<AccountsEn>();
            List<AccountsDetailsEn> lodetailslist = new List<AccountsDetailsEn>();
            string sqlCmd = "select * from SAS_SponsorInvoice WHERE BatchCode = @BatchCode";

            try
            {
                if (!FormHelp.IsBlank(sqlCmd))
                {
                    DbCommand cmd = _DatabaseFactory.GetDbCommand(Helper.GetDataBaseType, sqlCmd, DataBaseConnectionString);
                    _DatabaseFactory.AddInParameter(ref cmd, "@BatchCode", DbType.String, argEn.BatchCode);
                    _DbParameterCollection = cmd.Parameters;

                    using (IDataReader loReader = _DatabaseFactory.GetIDataReader(Helper.GetDataBaseType, cmd,
                        DataBaseConnectionString, sqlCmd, _DbParameterCollection).CreateDataReader())
                    {
                        AccountsDetailsDAL loObjAccountsDAL = new AccountsDetailsDAL();
                        AccountsDetailsEn loItem = null;
                        while (loReader.Read())
                        {
                            loItem = new AccountsDetailsEn();
                            loItem.TransactionID = GetValue<int>(loReader, "TransID");
                            lodetailslist.Add(loItem);
                            loItem = null;
                        }
                        loReader.Close();
                        int i = 0;
                        //deleting each item in batch
                        for (i = 0; i < lodetailslist.Count; i++)
                        {
                            loObjAccountsDAL.DeleteSponsorInvoiceDetails(lodetailslist[i]);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            DeleteSponsorInvoice(argEn);
            return lbRes;
        }

        #endregion

        #region DeleteSponsorInvoice

        /// <summary>
        /// Method to Delete
        /// </summary>
        /// <param name="argEn">Accounts Entity is the Input.BatchCode is an Input Property</param>
        /// <returns>Returns a Boolean</returns>
        public bool DeleteSponsorInvoice(AccountsEn argEn)
        {
            bool lbRes = false;
            string sqlCmd = "DELETE FROM SAS_SponsorInvoice WHERE  BatchCode = @BatchCode";

            try
            {
                if (!FormHelp.IsBlank(sqlCmd))
                {
                    DbCommand cmd = _DatabaseFactory.GetDbCommand(Helper.GetDataBaseType, sqlCmd, DataBaseConnectionString);
                    _DatabaseFactory.AddInParameter(ref cmd, "@BatchCode", DbType.String, argEn.BatchCode);
                    _DbParameterCollection = cmd.Parameters;

                    int liRowAffected = _DatabaseFactory.ExecuteNonQuery(Helper.GetDataBaseType, cmd,
                                DataBaseConnectionString, sqlCmd, _DbParameterCollection);

                    if (liRowAffected > -1)
                        lbRes = true;
                    else
                        throw new Exception("Insertion Failed! No Row has been updated...");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return lbRes;
        }

        #endregion

        #region GetSponsorInvoiceTransactions

        /// <summary>
        /// Method to Get Sponsor Invoice Transactions
        /// </summary>
        /// <param name="argEn">Accounts Entity is the Input.BatchCode,Category,PostStatus and SubType are the Input Property.</param>
        /// <returns>Returns List of Accounts.</returns>
        public List<AccountsEn> GetSponsorInvoiceTransactions(AccountsEn argEn)
        {
            List<AccountsEn> loListAccounts = new List<AccountsEn>();
            argEn.BatchCode = argEn.BatchCode.Replace("*", "%");
            string sqlCmd = "SELECT DISTINCT BatchCode FROM SAS_SponsorInvoice WHERE Category='" +
                          argEn.Category + "' AND PostStatus='" + argEn.PostStatus + "' and SubType ='" + argEn.SubType + "'";
            if (argEn.BatchCode.Length != 0)
            {
                sqlCmd = sqlCmd + " AND BatchCode LIKE '" + argEn.BatchCode + "'";                
            }
            sqlCmd = sqlCmd + " order by BatchCode DESC";
            try
            {
                if (!FormHelp.IsBlank(sqlCmd))
                {
                    using (IDataReader loReader = _DatabaseFactory.ExecuteReader(Helper.GetDataBaseType,
                        DataBaseConnectionString, sqlCmd).CreateDataReader())
                    {
                        while (loReader.Read())
                        {
                            argEn.BatchCode = GetValue<string>(loReader, "BatchCode");
                            loListAccounts.Add(GetSponsorInvoiceItem(argEn));
                        }
                        loReader.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return loListAccounts;
        }

        #endregion

        #region GetSponsorInvoiceItem

        /// <summary>
        /// Method to Get an Accounts Item
        /// </summary>
        /// <param name="argEn">Accounts Entity is the Input.BatchCode is the Input Property</param>
        /// <returns>Returns an Accounts Item</returns>
        public AccountsEn GetSponsorInvoiceItem(AccountsEn argEn)
        {
            AccountsEn loItem = new AccountsEn();
            string sqlCmd = "Select * FROM SAS_SponsorInvoice WHERE BatchCode = @BatchCode";

            try
            {
                if (!FormHelp.IsBlank(sqlCmd))
                {
                    DbCommand cmdSel = _DatabaseFactory.GetDbCommand(Helper.GetDataBaseType, sqlCmd, DataBaseConnectionString);
                    _DatabaseFactory.AddInParameter(ref cmdSel, "@BatchCode", DbType.String, argEn.BatchCode);
                    _DbParameterCollection = cmdSel.Parameters;

                    using (IDataReader loReader = _DatabaseFactory.GetIDataReader(Helper.GetDataBaseType, cmdSel,
                        DataBaseConnectionString, sqlCmd, _DbParameterCollection).CreateDataReader())
                    {
                        if (loReader != null)
                        {
                            loReader.Read();
                            loItem = LoadObject(loReader);

                        }
                        loReader.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return loItem;
        }

        #endregion

        #region GetListStudentSponsorInvoicebyBatchID

        /// <summary>
        /// Method to Get the List of Students by BatchId
        /// </summary>
        /// <param name="argEn">Student Entity is an Input Parameter.BatchCode is the Input Property</param>
        /// <returns>Returns List of Students</returns>
        public List<StudentEn> GetListStudentSponsorInvoicebyBatchID(StudentEn argEn)
        {
            List<StudentEn> loEnList = new List<StudentEn>();
            //string sqlCmd = " SELECT SAS_SponsorInvoice.TransID, SAS_SponsorInvoice.CreditRef,SAS_SponsorInvoice.CreditRef1, SAS_SponsorInvoice.BatchCode," +
            //                " SAS_Student.SASI_MatricNo,SAS_Student.SASI_Name, SAS_Student.SASI_PgId, SAS_Student.SASI_CurSem" +
            //                " ,sapg_program FROM SAS_SponsorInvoice INNER JOIN SAS_Student ON SAS_SponsorInvoice.CreditRef = SAS_Student.SASI_MatricNo" +
            //                "  INNER JOIN sas_program ON sas_program.sapg_code=SAS_Student.SASI_PgId  WHERE SAS_SponsorInvoice.BatchCode ='" + argEn.BatchCode + "'";

            string sqlCmd = " SELECT distinct sasi_pgid,batchcode,sapg_program,sasi_cursem,SAS_SponsorInvoice.CreditRef1 FROM SAS_SponsorInvoice " +
     "INNER JOIN SAS_Student ON SAS_SponsorInvoice.CreditRef = SAS_Student.SASI_MatricNo " +
     "INNER JOIN sas_program ON sas_program.sapg_code=SAS_Student.SASI_PgId  WHERE SAS_SponsorInvoice.BatchCode='" + argEn.BatchCode + "'group by sasi_pgid,batchcode,sapg_program,sasi_cursem,SAS_SponsorInvoice.CreditRef1";

            try
            {
                if (!FormHelp.IsBlank(sqlCmd))
                {
                    using (IDataReader loReader = _DatabaseFactory.ExecuteReader(Helper.GetDataBaseType,
                        DataBaseConnectionString, sqlCmd).CreateDataReader())
                    {
                        StudentDAL lods = new StudentDAL();
                        while (loReader.Read())
                        {
                            StudentEn loItem = new StudentEn();
                            //loItem.TranssactionID = GetValue<int>(loReader, "TransID");
                            //loItem.CreditRef = GetValue<string>(loReader, "CreditRef");
                            //loItem.BatchCode = GetValue<string>(loReader, "BatchCode");
                            //loItem.MatricNo = GetValue<string>(loReader, "SASI_MatricNo");
                            //loItem.StudentName = GetValue<string>(loReader, "SASI_Name");
                            //loItem.ProgramID = GetValue<string>(loReader, "SASI_PgId");
                            //loItem.CurrentSemester = GetValue<int>(loReader, "SASI_CurSem");
                            //loItem.SponsorCode = GetValue<string>(loReader, "CreditRef1");
                            //loItem.ProgramType = GetValue<string>(loReader, "sapg_program");
                            loItem.TranssactionID = 0;
                            loItem.CreditRef = "";
                            loItem.BatchCode = GetValue<string>(loReader, "BatchCode");
                            loItem.MatricNo = "";
                            loItem.StudentName = "";
                            loItem.ProgramID = GetValue<string>(loReader, "SASI_PgId");
                            loItem.CurrentSemester = GetValue<int>(loReader, "SASI_CurSem");
                            loItem.SponsorCode = GetValue<string>(loReader, "CreditRef1");
                            loItem.ProgramType = GetValue<string>(loReader, "sapg_program");
                            loEnList.Add(loItem);
                        }
                        loReader.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return loEnList;
        }

        //Group by BatchCode Only
        public List<StudentEn> GetListStudentSponsorInvoicebyBatchID1(StudentEn argEn)
        {
            List<StudentEn> loEnList = new List<StudentEn>();
            string sqlCmd = " SELECT SAS_SponsorInvoice.TransID, SAS_SponsorInvoice.CreditRef,SAS_SponsorInvoice.CreditRef1, SAS_SponsorInvoice.BatchCode," +
                            " SAS_Student.SASI_MatricNo,SAS_Student.SASI_Name, SAS_Student.SASI_PgId, SAS_Student.SASI_CurSem" +
                            " ,sapg_program FROM SAS_SponsorInvoice INNER JOIN SAS_Student ON SAS_SponsorInvoice.CreditRef = SAS_Student.SASI_MatricNo" +
                            "  INNER JOIN sas_program ON sas_program.sapg_code=SAS_Student.SASI_PgId  WHERE SAS_SponsorInvoice.BatchCode ='" + argEn.BatchCode + "'";


            try
            {
                if (!FormHelp.IsBlank(sqlCmd))
                {
                    using (IDataReader loReader = _DatabaseFactory.ExecuteReader(Helper.GetDataBaseType,
                        DataBaseConnectionString, sqlCmd).CreateDataReader())
                    {
                        StudentDAL lods = new StudentDAL();
                        while (loReader.Read())
                        {
                            StudentEn loItem = new StudentEn();
                            loItem.TranssactionID = GetValue<int>(loReader, "TransID");
                            loItem.CreditRef = GetValue<string>(loReader, "CreditRef");
                            loItem.BatchCode = GetValue<string>(loReader, "BatchCode");
                            loItem.MatricNo = GetValue<string>(loReader, "SASI_MatricNo");
                            loItem.StudentName = GetValue<string>(loReader, "SASI_Name");
                            loItem.ProgramID = GetValue<string>(loReader, "SASI_PgId");
                            loItem.CurrentSemester = GetValue<int>(loReader, "SASI_CurSem");
                            loItem.SponsorCode = GetValue<string>(loReader, "CreditRef1");
                            loItem.ProgramType = GetValue<string>(loReader, "sapg_program");

                            loEnList.Add(loItem);
                        }
                        loReader.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return loEnList;
        }
        #endregion

        #region Get Student Outstanding Amount

        /// <summary>
        /// Method to Get an Accounts Item
        /// </summary>
        /// <param name="argEn">Accounts Entity is the Input.BatchCode is the Input Property</param>
        /// <returns>Returns an Accounts Item</returns>
        public decimal GetStudentOutstandingAmount(string StudentMatricNo)
        {
            //variable declarations
            decimal OutstandingAmount = 0; decimal CreditAmount = 0; decimal DebitAmount = 0;

            try
            {
                //build sql statement Debit Amount - Start
                string SqlStatement = " SELECT SUM(SAS_Accounts.TransAmount) AS DebitAmount FROM SAS_Accounts INNER JOIN SAS_Student ON ";
                SqlStatement += " SAS_Accounts.CreditRef = SAS_Student.SASI_MatricNo WHERE (SAS_Accounts.TransType = 'Debit') AND ";
                SqlStatement += " (SAS_Accounts.poststatus = 'Posted') AND (SASI_MatricNo = " + clsGeneric.AddQuotes(StudentMatricNo) + ")";
                //build sql statement Debit Amount - Stop

                //build sql statement Credit Amount - Start
                string SqlStatement_1 = " SELECT SUM(SAS_Accounts.TransAmount) AS CreditAmount FROM SAS_Accounts INNER JOIN SAS_Student ON";
                SqlStatement_1 += " SAS_Accounts.CreditRef = SAS_Student.SASI_MatricNo WHERE (SAS_Accounts.TransType = 'Credit') and ";
                SqlStatement_1 += " (SAS_Accounts.poststatus = 'Posted') AND (SASI_MatricNo = " + clsGeneric.AddQuotes(StudentMatricNo) + ")";
                //build sql statement Credit Amount - Stop

                if (!FormHelp.IsBlank(SqlStatement))
                {
                    DebitAmount = clsGeneric.NullToDecimal(_DatabaseFactory.ExecuteScalar(
                        Helper.GetDataBaseType, DataBaseConnectionString, SqlStatement));
                }

                //get creditamount
                if (!FormHelp.IsBlank(SqlStatement_1))
                {
                    CreditAmount = clsGeneric.NullToDecimal(_DatabaseFactory.ExecuteScalar(
                       Helper.GetDataBaseType, DataBaseConnectionString, SqlStatement_1));
                }

                //total outstanding amount - Start
                if (DebitAmount > CreditAmount)
                {
                    OutstandingAmount = DebitAmount - CreditAmount;
                }
                else
                {
                    //added by Hafiz Roslan @ 2/2/2016
                    //outstnding amt will be negetive if credit > debit
                    OutstandingAmount = (CreditAmount - DebitAmount) * -1;
                }
                //total outstanding amount - Stop
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return OutstandingAmount;
        }

        #endregion

        #region Update Posting Status

        public bool UpdatePostingStatus(string BatchCode, string UserId)
        {
            string UpdateStatement = null;
            string transstatus = "Open";

            try
            {
                //Build Update Statement - Start
                UpdateStatement += "UPDATE sas_accountsdetails AS sas_details SET transcode = substring(sas_details.transtempcode from 2 for Length(sas_details.transtempcode)),poststatus = 'Posted',transstatus=" + clsGeneric.AddQuotes(transstatus) + " ,transtempcode = '' FROM sas_accounts sas_acc WHERE sas_acc.transid = sas_details.transid AND sas_acc.batchcode = " + clsGeneric.AddQuotes(BatchCode) + ";";

                UpdateStatement += "UPDATE sas_accounts SET transcode = substring(transtempcode from 2 for Length(transtempcode)),poststatus = 'Posted',transstatus=" + clsGeneric.AddQuotes(transstatus) + " ,transtempcode = '',";

                UpdateStatement += "postedby = " + clsGeneric.AddQuotes(UserId) + clsGeneric.AddComma() + "postedtimestamp = " + clsGeneric.AddQuotes(Helper.DateConversion(DateTime.Now)) + " WHERE batchcode = " + clsGeneric.AddQuotes(BatchCode) + ";";
                //Build Update Statement - Stop

                if (!FormHelp.IsBlank(UpdateStatement))
                {
                    if (_DatabaseFactory.ExecuteSqlStatement(Helper.GetDataBaseType,
                        DataBaseConnectionString, UpdateStatement) > -1)
                    {
                        //modified by Hafiz @ 22/11/2016 - increases performance
                        string sqlCmd = "select * from SAS_Accounts WHERE BatchCode = '" + BatchCode + "'";
                        if (!FormHelp.IsBlank(sqlCmd))
                        {
                            using (IDataReader loReader = _DatabaseFactory.ExecuteReader(Helper.GetDataBaseType,
                                DataBaseConnectionString, sqlCmd).CreateDataReader())
                            {
                                while (loReader.Read())
                                {
                                    AccountsEn loItem = LoadObject(loReader);
                                    if (loItem.Category == "Receipt" || loItem.Category == "Loan")
                                    {
                                        //Category = Loan
                                        if (UpdatePostingOnStudLoan(BatchCode, UserId)) { return true; }
                                        if (UpdateStudLoanPaidAmt(BatchCode)) { return true; }
                                    }
                                    else if (loItem.Category == "Credit Note" || loItem.Category == "Debit Note")
                                    {
                                        //Category = Sponsor Debit/Note
                                        if (UpdateRecpForSponsCreditDebitNote(BatchCode)) { return true; }
                                    }

                                    //update end-result outstanding amount
                                    if (UpdateOutAmt(BatchCode)) { return true; }
                                }
                            }
                        }
                    }
                }

                return false;
            }
            catch (Exception ex)
            {
                MaxModule.Helper.LogError(ex.Message);

                return false;
            }
        }

        #endregion

        #region Track Sponsor Invoice Receipt

        public void TrackSponsorInvoiceReceipt(string InvoiceId, string BatchCode)
        {
            //variable declarations
            string SqlStatement = null; int ReceiptId = 0;

            try
            {
                //get receipt id
                ReceiptId = GetReceiptId(BatchCode);

                //build sqls statement - Start
                SqlStatement = "INSERT INTO sas_sponsor_inv_rec(receipt_id, invoice_id)";
                SqlStatement += "VALUES (" + ReceiptId + ", '" + InvoiceId + "');";
                //build sqls statement - Stop

                //Insert Data - Start
                _DatabaseFactory.ExecuteSqlStatement(Helper.GetDataBaseType,
                    DataBaseConnectionString, SqlStatement);
                //Insert Data - Stop
            }
            catch (Exception ex)
            {
                MaxModule.Helper.LogError(ex.Message);
            }
        }

        #endregion

        #region Get Receipt Id

        public int GetReceiptId(string BatchCode)
        {
            //variable declarations
            string SqlStatement = null; int ReceiptId = 0;

            try
            {
                //build sqls statement - Start
                SqlStatement = "SELECT transid AS Receipt_Id FROM sas_accounts ";
                SqlStatement += "WHERE batchcode = " + clsGeneric.AddQuotes(BatchCode);
                //build sqls statement - Stop

                //Insert Data - Start
                IDataReader _IDataReader = _DatabaseFactory.ExecuteReader(Helper.GetDataBaseType,
                    DataBaseConnectionString, SqlStatement).CreateDataReader();
                //Insert Data - Stop

                //if record exists - Start
                if (_IDataReader.Read())
                {
                    //get receipt id
                    ReceiptId = clsGeneric.NullToInteger(_IDataReader[0]);
                }
                //if record exists - Stop

                return ReceiptId;
            }
            catch (Exception ex)
            {
                MaxModule.Helper.LogError(ex.Message);
                return 0;
            }
        }

        #endregion

        #region Insert Receipt Details
        public bool InsertReceiptDetails(string sasi_matricno, string receipt_identity, string receipt_no, DateTime receipt_date, double receipt_amount)
        {
            bool lbRes = false;
            string sqlCmd = "";

            // string[] pathArr = filepath.Split('\\');
            //// string fileName = pathArr(pathArr.Length).ToString();
            // string fileName = pathArr[pathArr.Length-1];

            try
            {
                sqlCmd = "INSERT INTO sas_clicks_receiptdetails(";
                sqlCmd += "sasi_matricno, receipt_identity, receipt_no, receipt_date,receipt_amount) ";
                sqlCmd += " VALUES(@sasi_matricno, @receipt_identity, @receipt_no, to_date(@receipt_date,'DD/MM/YYYY'),@receipt_amount)";

                if (!FormHelp.IsBlank(sqlCmd))
                {
                    DbCommand cmd = _DatabaseFactory.GetDbCommand(Helper.GetDataBaseType, sqlCmd, DataBaseConnectionString);

                    _DatabaseFactory.AddInParameter(ref cmd, "@sasi_matricno", DbType.String, sasi_matricno);
                    _DatabaseFactory.AddInParameter(ref cmd, "@receipt_identity", DbType.String, receipt_identity);
                    _DatabaseFactory.AddInParameter(ref cmd, "@receipt_no", DbType.String, receipt_no);
                    _DatabaseFactory.AddInParameter(ref cmd, "@receipt_date", DbType.String, receipt_date.ToShortDateString());
                    _DatabaseFactory.AddInParameter(ref cmd, "@receipt_amount", DbType.String, receipt_amount);
                    _DbParameterCollection = cmd.Parameters;

                    int liRowAff = _DatabaseFactory.ExecuteNonQuery(Helper.GetDataBaseType, cmd,
                        DataBaseConnectionString, sqlCmd, _DbParameterCollection);

                    if (liRowAff > -1)
                        lbRes = true;
                    else
                        throw new Exception("Insertion Failed! No Row has been inserted...");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return lbRes;
        }
        #endregion

        #region Update Receipt Details
        /// <summary>
        /// Method to Update Receipt
        /// </summary>
        /// Added by Hafiz Roslan
        /// Date: 05/01/2016
        /// Modified by Hafiz @ 25/3/2016
        /// 
        public bool UpdateReceiptDetails(string sasi_matricno, string receipt_identity, string receipt_no, DateTime receipt_date, double receipt_amount)
        {
            bool lbRes = false;
            string sqlCmd = "";

            try
            {
                sqlCmd = "UPDATE sas_clicks_receiptdetails";
                sqlCmd += " SET receipt_identity = @receipt_identity, ";
                sqlCmd += " receipt_date = @receipt_date, ";
                sqlCmd += " receipt_amount = @receipt_amount ";

                if (receipt_no.Length != 0) sqlCmd += " ,receipt_no = '" + receipt_no + "' ";

                sqlCmd += "WHERE sasi_matricno = @sasi_matricno ";

                if (!FormHelp.IsBlank(sqlCmd))
                {
                    DbCommand cmd = _DatabaseFactory.GetDbCommand(Helper.GetDataBaseType, sqlCmd, DataBaseConnectionString);

                    _DatabaseFactory.AddInParameter(ref cmd, "@sasi_matricno", DbType.String, sasi_matricno);
                    _DatabaseFactory.AddInParameter(ref cmd, "@receipt_identity", DbType.String, receipt_identity);
                    _DatabaseFactory.AddInParameter(ref cmd, "@receipt_date", DbType.DateTime, Helper.DateConversion(receipt_date));
                    _DatabaseFactory.AddInParameter(ref cmd, "@receipt_amount", DbType.Double, receipt_amount);
                    _DbParameterCollection = cmd.Parameters;

                    int liRowAff = _DatabaseFactory.ExecuteNonQuery(Helper.GetDataBaseType, cmd,
                        DataBaseConnectionString, sqlCmd, _DbParameterCollection);

                    if (liRowAff > -1)
                        lbRes = true;
                    else
                        throw new Exception("Updating Failed! No Row has been updated...");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return lbRes;
        }
        #endregion

        #region _Update
        /// <summary>
        /// Method to Update Receipt&Sponsor
        /// </summary>
        /// Added by Hafiz Roslan
        /// Date: 05/01/2016
        /// modified by Hafiz @ 11/4/2016 - add outstanding amount

        public bool _Update(AccountsEn argEn)
        {
            bool lbRes = false;
            string sqlCmd;

            try
            {
                sqlCmd = "UPDATE SAS_Accounts";
                sqlCmd += " SET SubType = @SubType, ";
                sqlCmd += " PaymentMode = @PaymentMode, ";
                sqlCmd += " BatchDate = @BatchDate, ";
                sqlCmd += " BankCode = @BankCode, ";
                sqlCmd += " TransDate = @TransDate, ";
                sqlCmd += " SubRef1 = @SubRef1, ";
                sqlCmd += " TransAmount = @TransAmount, ";
                sqlCmd += " Description = @Description, ";
                sqlCmd += " UpdatedBy = @UpdatedBy, ";
                sqlCmd += " BankRecNo = @bankrecno,";
                sqlCmd += " ReceiptDate = @ReceiptDate,";
                sqlCmd += " CreditRef = @CreditRef, ";
                sqlCmd += " OutstandingAmt = @OutstandingAmount, ";
                sqlCmd += " control_amt = @control_amt ";
                sqlCmd += " WHERE batchcode ='" + argEn.BatchCode + "'";
                sqlCmd += " AND creditref ='" + argEn.CreditRef + "'";

                if (!FormHelp.IsBlank(sqlCmd))
                {
                    //Added by Hafiz Roslan
                    //Date: 31/12/2015
                    //check for duplicate bank slip no
                    //if (String.IsNullOrEmpty(argEn.BankSlipID) == false)
                    //{
                    //    CheckDuplicateBankSlipNo(argEn);
                    //}

                    DbCommand cmd = _DatabaseFactory.GetDbCommand(Helper.GetDataBaseType, sqlCmd, DataBaseConnectionString);

                    _DatabaseFactory.AddInParameter(ref cmd, "@SubType", DbType.String, argEn.SubType);
                    _DatabaseFactory.AddInParameter(ref cmd, "@PaymentMode", DbType.String, argEn.PaymentMode);
                    _DatabaseFactory.AddInParameter(ref cmd, "@BatchDate", DbType.DateTime, Helper.DateConversion(argEn.BatchDate));
                    _DatabaseFactory.AddInParameter(ref cmd, "@BankCode", DbType.String, argEn.BankCode);
                    _DatabaseFactory.AddInParameter(ref cmd, "@TransDate", DbType.DateTime, Helper.DateConversion(argEn.TransDate));
                    _DatabaseFactory.AddInParameter(ref cmd, "@SubRef1", DbType.String, argEn.SubReferenceOne);
                    _DatabaseFactory.AddInParameter(ref cmd, "@TransAmount", DbType.Double, argEn.TransactionAmount);
                    _DatabaseFactory.AddInParameter(ref cmd, "@Description", DbType.String, argEn.Description);
                    _DatabaseFactory.AddInParameter(ref cmd, "@UpdatedBy", DbType.String, argEn.UpdatedBy);
                    _DatabaseFactory.AddInParameter(ref cmd, "@bankrecno", DbType.String, argEn.BankSlipID);
                    _DatabaseFactory.AddInParameter(ref cmd, "@ReceiptDate", DbType.DateTime, Helper.DateConversion(argEn.ReceiptDate));
                    _DatabaseFactory.AddInParameter(ref cmd, "@CreditRef", DbType.String, argEn.CreditRef);
                    _DatabaseFactory.AddInParameter(ref cmd, "@OutstandingAmount", DbType.String, argEn.Outstanding_Amount);
                    _DatabaseFactory.AddInParameter(ref cmd, "@control_amt", DbType.Decimal, clsGeneric.NullToDecimal(argEn.ControlAmt));
                    _DbParameterCollection = cmd.Parameters;

                    int liRowAff = _DatabaseFactory.ExecuteNonQuery(Helper.GetDataBaseType, cmd,
                        DataBaseConnectionString, sqlCmd, _DbParameterCollection);

                    if (liRowAff > -1)
                    {
                        lbRes = true;
                    }
                    else
                        throw new Exception("Updating Failed! No Row has been updated...");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return lbRes;
        }
        #endregion

        #region DeleteStudAvailableForDG
        /// Added by Hafiz Roslan
        /// Date: 27/01/2016
        /// For : Delete student with exist in the DB - used in the onclick datagrid

        public bool DeleteStudAvailableForDG(String matricno, String batchcode)
        {
            bool lbRes = false;

            string sqlCmd = "DELETE from SAS_Accounts ";
            sqlCmd += " WHERE batchcode ='" + batchcode + "'";
            sqlCmd += " AND creditref ='" + matricno + "'";

            try
            {
                if (!FormHelp.IsBlank(sqlCmd))
                {
                    int liRowAffected = _DatabaseFactory.ExecuteSqlStatement(Helper.GetDataBaseType,
                        DataBaseConnectionString, sqlCmd);

                    if (liRowAffected > -1)
                        lbRes = true;
                    else
                        throw new Exception("Delete Failed!");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return lbRes;
        }

        #endregion

        #region CheckStudAvailableOrNot
        /// Added by Hafiz Roslan
        /// Date: 27/01/2016
        /// For : Check student availability in the sas_account for update/add

        public bool CheckStudAvailableOrNot(String matricno, String batchcode)
        {
            bool result = false;

            string sqlCmd = "select * from SAS_Accounts ";
            sqlCmd += " WHERE batchcode ='" + batchcode + "'";
            sqlCmd += " AND creditref ='" + matricno + "'";

            try
            {
                if (!FormHelp.IsBlank(sqlCmd))
                {
                    using (IDataReader loReader = _DatabaseFactory.ExecuteReader(Helper.GetDataBaseType,
                       DataBaseConnectionString, sqlCmd).CreateDataReader())
                    {
                        while (loReader.Read())
                        {
                            result = true;
                        }
                        loReader.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return result;
        }

        #endregion

        #region Update Posting Status StudentLoan

        public bool UpdatePostingStatusStudentLoan(string BatchCode, string UserId)
        {
            //create instances
            IDataReader _IDataReader = null;

            //variable declarations
            string SqlStatement = null; string UpdateStatement = null; int TransId = 0;

            try
            {
                //Build Sql Statement - Start
                SqlStatement = "SELECT transid from sas_studentloan WHERE batchcode = "
                    + clsGeneric.AddQuotes(BatchCode);
                //Build Sql Statement - Stop

                //Get Batch Details - Start
                _IDataReader = _DatabaseFactory.ExecuteReader(
                    Helper.GetDataBaseType, DataBaseConnectionString, SqlStatement).CreateDataReader();
                //Get Batch Details - Stop

                //loop thro the batch details - start
                while (_IDataReader.Read())
                {
                    //Get Transaction Id
                    TransId = clsGeneric.NullToInteger(_IDataReader[0]);
                    //Updated Mona 19/2/2016
                    string transstatus = "Open";
                    //string transstatus = "Closed";

                    //Build Update Statement - Start
                    //UpdateStatement += "UPDATE sas_accountsdetails SET transcode = substring(transtempcode from 2 for Length(transtempcode)),poststatus = 'Posted',transstatus=" + clsGeneric.AddQuotes(transstatus) + " ,transtempcode = '' WHERE transid = " + TransId + ";";
                    //UpdateStatement += "UPDATE sas_accounts SET transcode = substring(transtempcode from 2 for Length(transtempcode)),poststatus = 'Posted',transstatus=" + clsGeneric.AddQuotes(transstatus) + " ,transtempcode = '',";
                    //UpdateStatement += "postedby = " + clsGeneric.AddQuotes(UserId) + clsGeneric.AddComma() + "postedtimestamp = " + clsGeneric.AddQuotes(Helper.DateConversion(DateTime.Now)) + " WHERE transid = " + TransId + ";";

                    UpdateStatement += "UPDATE sas_studentloan SET transcode = substring(transtempcode from 2 for Length(transtempcode)),poststatus = 'Posted',transstatus=" + clsGeneric.AddQuotes(transstatus) + ",";
                    UpdateStatement += "postedby = " + clsGeneric.AddQuotes(UserId) + clsGeneric.AddComma() + "postedtimestamp = " + clsGeneric.AddQuotes(Helper.DateConversion(DateTime.Now)) + " WHERE batchcode = " + clsGeneric.AddQuotes(BatchCode) + ";";

                    //Build Update Statement - Stop
                }
                //loop thro the batch details - stop

                //if update statement successful - Start
                if (!FormHelp.IsBlank(UpdateStatement))
                {
                    if (_DatabaseFactory.ExecuteSqlStatement(Helper.GetDataBaseType,
                        DataBaseConnectionString, UpdateStatement) > -1)
                    {
                        //added by Hafiz @ 25/4/2016
                        //update outstanding amount - start
                        //string type = "loanledger";

                        //if (UpdateOutAmt(BatchCode, type)) { }
                        if (UpdateOutAmt(BatchCode)) { }
                        //update outstanding amount - end

                        return true;
                    }
                }
                //if update statement successful - Stop

                return false;
            }
            catch (Exception ex)
            {
                MaxModule.Helper.LogError(ex.Message);

                return false;
            }
        }

        #endregion

        #region SponsorBatchPost

        /// <summary>
        /// Method to Insert BatchPost
        /// </summary>
        /// <param name="argEn">Accounts Entity is the Input</param>
        /// <param name="argList">List of Sponsor Entity is the Input</param>
        /// <returns>Returns BatchPost</returns>
        //public bool SponsorBatchPost(AccountsEn argEn, List<SponsorEn> argList)
        public bool SponsorBatchPost(string BatchCode, string UserId)
        {
            int i = 0;
            double StuAllAmt = 0.00;
            double subreferenceone = 0.00;
            //BatchDelete(argEn);
            //StuAllAmt = argEn.AllocatedAmount;
            //argEn.BatchCode = GetAutoNumber("Batch");
            //variable declarations
            string SqlStatement = null; string UpdateStatement = null; int TransId = 0;

            try
            {
                //Build Sql Statement - Start
                //SqlStatement = "SELECT transid, category from sas_studentloan WHERE batchcode = " + clsGeneric.AddQuotes(BatchCode);                
                SqlStatement = "SELECT * from sas_accounts WHERE batchcode = " + clsGeneric.AddQuotes(BatchCode);
                //Build Sql Statement - Stop

                IDataReader _IDataReader = _DatabaseFactory.ExecuteReader(Helper.GetDataBaseType, DataBaseConnectionString,
                    SqlStatement).CreateDataReader();

                while (_IDataReader.Read())
                {
                    TransId = clsGeneric.NullToInteger(_IDataReader[0]);
                    string transstatus = "Closed";

                    String category = GetValue<string>(_IDataReader, "category");
                    String creditref1 = GetValue<string>(_IDataReader, "creditref1");
                    String subref1 = GetValue<string>(_IDataReader, "subref1");
                    String subtype = GetValue<string>(_IDataReader, "subtype");
                    double transamount = GetValue<double>(_IDataReader, "transamount");
                    double paidamount = GetValue<double>(_IDataReader, "paidamount");
                    double allocateamount = GetValue<double>(_IDataReader, "allocateamount");

                    StuAllAmt = paidamount - transamount;
                    if (category == "Allocation")
                    {
                        UpdateStatement += "UPDATE sas_accounts SET allocateamount = " + StuAllAmt + clsGeneric.AddComma() + "postedby = " + clsGeneric.AddQuotes(UserId) + ",";
                        UpdateStatement += "postedtimestamp = " + clsGeneric.AddQuotes(Helper.DateConversion(DateTime.Now)) + " WHERE batchcode = " + clsGeneric.AddQuotes(creditref1) + ";";
                    }
                    if (category == "Payment" && subtype == "Sponsor")
                    {
                        StuAllAmt = allocateamount + transamount;
                        subreferenceone = Convert.ToDouble(subref1);
                        if (StuAllAmt == subreferenceone)
                        {
                            StuAllAmt = 0;
                        }
                        else
                        {
                            StuAllAmt = paidamount - allocateamount;
                        }
                        UpdateStatement += "UPDATE sas_accounts SET allocateamount = " + StuAllAmt + clsGeneric.AddComma() + "postedby = " + clsGeneric.AddQuotes(UserId) + ",";
                        UpdateStatement += "postedtimestamp = " + clsGeneric.AddQuotes(Helper.DateConversion(DateTime.Now)) + " WHERE batchcode = " + clsGeneric.AddQuotes(creditref1) + ";";
                    }
                }

                if (!FormHelp.IsBlank(UpdateStatement))
                {
                    if (_DatabaseFactory.ExecuteSqlStatement(Helper.GetDataBaseType,
                        DataBaseConnectionString, UpdateStatement) > -1)
                    {
                        return true;
                    }
                }

                return false;
            }
            catch (Exception ex)
            {
                MaxModule.Helper.LogError(ex.Message);

                return false;
            }

        }
        //return argEn.BatchCode;

        #endregion

        #region GetStudentLoanOutstanding
        //added by Hafiz Roslan @ 4/2/2016
        //to get the student loan`s outstanding amount in the sas_studentloan

        public double GetStudentLoanOutstanding(StudentEn stud)
        {
            //double batchtotal = 0.0;

            //string sqlCmd = "select SUM(batchtotal) AS batchtotal from SAS_Studentloan ";
            //sqlCmd += " WHERE creditref ='" + stud.MatricNo + "'";

            //try
            //{
            //    if (!FormHelp.IsBlank(sqlCmd))
            //    {
            //        using (IDataReader loReader = _DatabaseFactory.ExecuteReader(Helper.GetDataBaseType,
            //           DataBaseConnectionString, sqlCmd).CreateDataReader())
            //        {
            //            while (loReader.Read())
            //            {
            //                batchtotal = clsGeneric.NullToInteger(loReader[0]);
            //            }
            //            loReader.Close();
            //        }
            //    }

            //    return batchtotal;
            //}
            //catch (Exception ex)
            //{
            //    MaxModule.Helper.LogError(ex.Message);
            //    return 0.0;
            //}

            ////updated by Hafiz Roslan @ 11/2/2016
            decimal batch_total = 0, paid_amt = 0;
            double loanamount = 0.0;

            try
            {
                string sqlCmd = "select SUM(batchtotal) AS batchtotal from SAS_Studentloan ";
                sqlCmd += " WHERE creditref ='" + stud.MatricNo + "' AND category NOT IN ('Receipt') ";
                sqlCmd += " AND poststatus = 'Posted';";

                string sqlCmd1 = "select SUM(transamount) AS transamount from SAS_Studentloan ";
                sqlCmd1 += " WHERE creditref ='" + stud.MatricNo + "' AND category = 'Receipt' ";
                sqlCmd1 += " AND poststatus = 'Posted';";

                if (!FormHelp.IsBlank(sqlCmd))
                {
                    batch_total = clsGeneric.NullToDecimal(_DatabaseFactory.ExecuteScalar(
                        Helper.GetDataBaseType, DataBaseConnectionString, sqlCmd));
                }

                if (!FormHelp.IsBlank(sqlCmd1))
                {
                    paid_amt = clsGeneric.NullToDecimal(_DatabaseFactory.ExecuteScalar(
                       Helper.GetDataBaseType, DataBaseConnectionString, sqlCmd1));
                }

                loanamount = Convert.ToDouble(batch_total - paid_amt);

            }
            catch (Exception ex)
            {
                MaxModule.Helper.LogError(ex.Message);

            }

            return loanamount;
        }
        #endregion

        #region UpdatePostingOnStudLoan

        //added by Hafiz Roslan @ 4/2/2016
        //Check for Category = Loan. if Yes Update table sas_studentloan
        //updated on 11/2/2015
        //change category to receipt back

        public bool UpdatePostingOnStudLoan(string BatchCode, string UserId)
        {
            //variable declarations
            string SqlStatement = null; string UpdateStatement = null; int TransId = 0;

            try
            {
                //Build Sql Statement - Start
                //SqlStatement = "SELECT transid, category from sas_studentloan WHERE batchcode = " + clsGeneric.AddQuotes(BatchCode);                
                SqlStatement = "SELECT transid, creditref, category, batchtotal from sas_studentloan WHERE batchcode = " + clsGeneric.AddQuotes(BatchCode);
                //Build Sql Statement - Stop

                IDataReader _IDataReader = _DatabaseFactory.ExecuteReader(Helper.GetDataBaseType, DataBaseConnectionString,
                    SqlStatement).CreateDataReader();

                while (_IDataReader.Read())
                {
                    TransId = clsGeneric.NullToInteger(_IDataReader[0]);
                    string transstatus = "Open";
                    
                    String category = GetValue<string>(_IDataReader, "category");
                    String matricno = GetValue<string>(_IDataReader, "creditref");
                    double batch_total = GetValue<double>(_IDataReader, "batchtotal");

                    //if (category == "Loan")
                    //{
                    //    UpdateStatement += "UPDATE sas_studentloan SET transcode = substring(transtempcode from 2 for Length(transtempcode)),poststatus = 'Posted',transstatus=" + clsGeneric.AddQuotes(transstatus) + " ,transtempcode = '',";
                    //    UpdateStatement += "postedby = " + clsGeneric.AddQuotes(UserId) + clsGeneric.AddComma() + "postedtimestamp = " + clsGeneric.AddQuotes(Helper.DateConversion(DateTime.Now)) + " WHERE transid = " + TransId + ";";
                    //}
                    if (category == "Receipt")
                    {
                        UpdateStatement += "UPDATE sas_studentloan SET transcode = substring(transtempcode from 2 for Length(transtempcode)),poststatus = 'Posted',transstatus=" + clsGeneric.AddQuotes(transstatus) + " ,transtempcode = '',";
                        UpdateStatement += "postedby = " + clsGeneric.AddQuotes(UserId) + clsGeneric.AddComma() + "postedtimestamp = " + clsGeneric.AddQuotes(Helper.DateConversion(DateTime.Now)) + " WHERE transid = " + TransId + ";";
                    }
                }

                if (!FormHelp.IsBlank(UpdateStatement))
                {
                    if (_DatabaseFactory.ExecuteSqlStatement(Helper.GetDataBaseType,
                        DataBaseConnectionString, UpdateStatement) > -1)
                    {
                        return true;
                    }
                }

                return false;
            }
            catch (Exception ex)
            {
                MaxModule.Helper.LogError(ex.Message);

                return false;
            }
        }

        #endregion

        #region AutoUpdateStudentOutstanding
        //added by Hafiz Roslan @ 5/2/2016
        //autoupdate sas_studentoutstanding with current outstanding values 
        //Modified by Hafiz Roslan @ 16/2/2016
        //Added insert if no student found
        //public void AutoUpdateStudOutstanding(String matricno, Double outstanding_amt, String rb_type)
        //Modified by Hafiz @ 10/3/2016
        //modified by Hafiz @ 31/3/2016
        //modified by Hafiz @ 25/4/2016
        //modified by Hafiz @ 27/5/2016

        public void AutoUpdateStudOutstanding(StudentEn argStudent, Double outstanding_amt)
        {
            bool lbRes = false;
            bool result = false;
            string sqlCmd = string.Empty;
            double out_amt = 0.00;
            int rows = 0;
            int flag = 0;

            try
            {
                sqlCmd = "select count(*) as rows, SASO_outstandingamt, SASO_isreleased FROM SAS_StudentOutstanding ";
                sqlCmd += "WHERE SASI_MatricNo = @MatricNo GROUP BY SASO_outstandingamt, SASO_isreleased";

                if (!FormHelp.IsBlank(sqlCmd))
                {
                    DbCommand cmdSel = _DatabaseFactory.GetDbCommand(Helper.GetDataBaseType, sqlCmd, DataBaseConnectionString);
                    _DatabaseFactory.AddInParameter(ref cmdSel, "@MatricNo", DbType.String, argStudent.MatricNo);
                    _DbParameterCollection = cmdSel.Parameters;

                    using (IDataReader dr = _DatabaseFactory.GetIDataReader(Helper.GetDataBaseType, cmdSel,
                       DataBaseConnectionString, sqlCmd, _DbParameterCollection).CreateDataReader())
                    {
                        if (dr.Read())
                        {
                            rows = clsGeneric.NullToInteger(dr["rows"]);

                            //if available - update
                            if (rows != 0)
                            {
                                //outstanding - start
                                out_amt = GetValue<double>(dr, "SASO_outstandingamt");
                                flag = GetValue<int>(dr, "SASO_isreleased");

                                if (out_amt != outstanding_amt)
                                {
                                    sqlCmd = "UPDATE SAS_StudentOutstanding SET SASO_outstandingamt ='" + outstanding_amt + "' ";
                                    sqlCmd += "WHERE SASI_MatricNo ='" + argStudent.MatricNo + "'";

                                    result = true;
                                }
                                //outstanding - end
                            }
                        }

                        //no student - insert
                        if (rows == 0)
                        {
                            sqlCmd = "INSERT INTO SAS_StudentOutstanding(SASI_MatricNo,SASI_Name,SASI_PgId,SASI_CurSem,SASI_CurSemYr,SASO_outstandingamt,SASO_IsReleased)";
                            sqlCmd += "VALUES(@SASI_MatricNo,@SASI_Name,@SASI_PgId,@SASI_CurSem,@SASI_CurSemYr,@SASO_outstandingamt,@SASO_IsReleased)";

                            if (!FormHelp.IsBlank(sqlCmd))
                            {
                                DbCommand cmd = _DatabaseFactory.GetDbCommand(Helper.GetDataBaseType, sqlCmd, DataBaseConnectionString);
                                _DatabaseFactory.AddInParameter(ref cmd, "@SASI_MatricNo", DbType.String, argStudent.MatricNo);
                                _DatabaseFactory.AddInParameter(ref cmd, "@SASI_Name", DbType.String, argStudent.StudentName);
                                _DatabaseFactory.AddInParameter(ref cmd, "@SASI_PgId", DbType.String, argStudent.ProgramID);
                                _DatabaseFactory.AddInParameter(ref cmd, "@SASI_CurSem", DbType.Int16, argStudent.CurrentSemester);
                                _DatabaseFactory.AddInParameter(ref cmd, "@SASI_CurSemYr", DbType.String, argStudent.CurretSemesterYear);
                                _DatabaseFactory.AddInParameter(ref cmd, "@SASO_IsReleased", DbType.Int16, 0);
                                _DatabaseFactory.AddInParameter(ref cmd, "@SASO_outstandingamt", DbType.Double, outstanding_amt);

                                _DbParameterCollection = cmd.Parameters;

                                //start executing
                                int liRowAffected = _DatabaseFactory.ExecuteNonQuery(Helper.GetDataBaseType, cmd,
                                    DataBaseConnectionString, sqlCmd, _DbParameterCollection);

                                if (liRowAffected > -1)
                                    lbRes = true;
                                else
                                    throw new Exception("Insertion Failed! No Row has been updated...");
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            if (result == true)
            {
                try
                {
                    if (!FormHelp.IsBlank(sqlCmd))
                    {
                        int liRowAffected = _DatabaseFactory.ExecuteSqlStatement(Helper.GetDataBaseType,
                             DataBaseConnectionString, sqlCmd);

                        if (liRowAffected > -1)
                            lbRes = true;
                        else
                            throw new Exception("Update Failed! No Row has been updated...");
                    }

                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        #endregion

        #region CheckCurSemYr
        //added by Hafiz Roslan @ 6/2/2016
        //check currentsemyear and update at sas_studentoutstanding

        public void CheckCurSemYr(string matricno, string cursemyr)
        {
            string sqlCmd = "";
            string sql_cursemyr = "";
            string res = "";

            try
            {
                sqlCmd = "select * from SAS_StudentOutstanding WHERE SASI_MatricNo ='" + matricno + "'";

                if (!FormHelp.IsBlank(sqlCmd))
                {
                    using (IDataReader loReader = _DatabaseFactory.ExecuteReader(Helper.GetDataBaseType,
                       DataBaseConnectionString, sqlCmd).CreateDataReader())
                    {
                        while (loReader.Read())
                        {
                            sql_cursemyr = GetValue<string>(loReader, "SASI_cursemyr");

                            if (sql_cursemyr != cursemyr)
                            {
                                res = "update";
                            }
                        }

                        loReader.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            if (res == "update")
            {
                try
                {
                    sqlCmd = "UPDATE SAS_StudentOutstanding SET SASI_cursemyr = '" + cursemyr + "' ";
                    sqlCmd += " WHERE SASI_MatricNo ='" + matricno + "'";

                    if (!FormHelp.IsBlank(sqlCmd))
                    {
                        int liRowAffected = _DatabaseFactory.ExecuteSqlStatement(Helper.GetDataBaseType,
                             DataBaseConnectionString, sqlCmd);

                        if (liRowAffected > -1)
                            res = "success";
                        else
                            throw new Exception("Update Failed! No Row has been updated...");
                    }

                }
                catch (Exception ex)
                {
                    throw ex;
                    res = "failed";
                }
            }
        }

        #endregion

        #region StudentOutstandingAmt

        /// <summary>
        /// Method to Get Student's Outstanding Amount
        /// </summary>
        /// <param name="argEn">Student Entity is an Input</param>
        /// <returns>Returns the Outstanding Amount</returns>
        public double StudentOutstandingAmount(StudentEn argEn)
        {
            double OutAmt = 0.0;

            string sqlCmd = @" Select Sum(Debit) - Sum(Credit) AS Amount From(
	                                    SELECT 
	                                      CASE WHEN TransType = 'Credit' THEN TransAmount
                                               ELSE 0
	                                      END Credit,
	                                      CASE WHEN TransType = 'Debit' THEN TransAmount
		                                       ELSE 0
	                                      END Debit	    
	                            FROM   SAS_Accounts
	                            WHERE  CreditRef = '" + argEn.MatricNo + "'";
            sqlCmd = sqlCmd + "AND SubType = 'Student'and PostStatus ='Posted'AND Category NOT IN ('Loan')) Result";



            try
            {
                if (!FormHelp.IsBlank(sqlCmd))
                {
                    using (IDataReader dr = _DatabaseFactory.ExecuteReader(Helper.GetDataBaseType,
                        DataBaseConnectionString, sqlCmd).CreateDataReader())
                    {
                        if (dr.Read())
                            OutAmt = GetValue<double>(dr, "amount");
                    }
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return OutAmt;
        }

        #endregion

        #region UpdateStudLoanPaidAmt
        //added by Hafiz Roslan @ 11/2/2016
        //update paid amount at the sas_studentloan

        public bool UpdateStudLoanPaidAmt(String batchcode)
        {
            List<AccountsEn> list_stud = new List<AccountsEn>();

            string SqlStatement = null, matricno = null;
            decimal tot_transamount = 0;

            try
            {
                //Build Sql Statement - Start
                SqlStatement = "SELECT * FROM sas_studentloan WHERE batchcode = '" + batchcode + "' ";
                //Build Sql Statement - Stop

                if (!FormHelp.IsBlank(SqlStatement))
                {
                    using (IDataReader loReader = _DatabaseFactory.ExecuteReader(Helper.GetDataBaseType,
                       DataBaseConnectionString, SqlStatement).CreateDataReader())
                    {
                        while (loReader.Read())
                        {
                            matricno = GetValue<string>(loReader, "creditref");

                            //get sum transamount and compare to the batchtotal of the loan
                            string sqlCmd = "select SUM(transamount) AS transamount from SAS_Studentloan ";
                            sqlCmd += " WHERE creditref ='" + matricno + "' AND category = 'Receipt' ";
                            sqlCmd += " AND poststatus = 'Posted';";

                            if (!FormHelp.IsBlank(sqlCmd))
                            {
                                tot_transamount = clsGeneric.NullToDecimal(_DatabaseFactory.ExecuteScalar(
                                   Helper.GetDataBaseType, DataBaseConnectionString, sqlCmd));

                                //compare with each of the batchtotal - START
                                string sqlCmd1 = "select * from SAS_Studentloan WHERE creditref ='" + matricno + "'";
                                sqlCmd1 += " AND category NOT IN ('Receipt') AND poststatus = 'Posted' ";

                                if (!FormHelp.IsBlank(sqlCmd1))
                                {
                                    using (IDataReader loReader1 = _DatabaseFactory.ExecuteReader(Helper.GetDataBaseType,
                                       DataBaseConnectionString, sqlCmd1).CreateDataReader())
                                    {
                                        while (loReader1.Read())
                                        {
                                            AccountsEn stud = new AccountsEn();

                                            stud.BatchTotal = GetValue<double>(loReader1, "batchtotal");
                                            stud.PaidAmount = GetValue<double>(loReader1, "paidamount");
                                            stud.TransactionID = GetValue<int>(loReader1, "transid");
                                            list_stud.Add(stud);
                                        }

                                        UpdateStudLoanPaidAmount(list_stud, Convert.ToDouble(tot_transamount));
                                    }
                                }
                                //compare with each of the batchtotal - END
                            }
                        }

                        loReader.Close();
                    }
                }

                return false;
            }
            catch (Exception ex)
            {
                MaxModule.Helper.LogError(ex.Message);

                return false;
            }

        }

        #endregion

        #region UpdateStudLoanPaidAmount
        //added by Hafiz Roslan @ 12/2/2016
        //to update the table sas_studentloan paidamount

        public bool UpdateStudLoanPaidAmount(List<AccountsEn> stud_list, double tot_transamount)
        {
            String sql_stmt = null;
            double balance = 0.0;
            int i = 0;

            try
            {
                for (i = 0; i < stud_list.Count; i++)
                {
                    if (stud_list[i].PaidAmount == stud_list[i].BatchTotal)
                    {
                        balance = tot_transamount - stud_list[i].PaidAmount;
                    }
                    else
                    {
                        if (stud_list[i].PaidAmount > 0)
                        {
                            double tots = 0.0;
                            double b = 0.0;

                            tots = stud_list[i].BatchTotal - stud_list[i].PaidAmount;

                            if (tot_transamount > tots)
                            {
                                if (tot_transamount > stud_list[i].BatchTotal)
                                {
                                    sql_stmt += "UPDATE SAS_Studentloan SET paidamount = " + stud_list[i].BatchTotal + " WHERE transid = " + stud_list[i].TransactionID + ";";

                                    balance = tot_transamount - stud_list[i].BatchTotal;
                                }
                                else
                                {
                                    sql_stmt += "UPDATE SAS_Studentloan SET paidamount = " + tot_transamount + " WHERE transid = " + stud_list[i].TransactionID + ";";

                                    balance = tot_transamount - tot_transamount;
                                }
                            }
                            else
                            {
                                sql_stmt += "UPDATE SAS_Studentloan SET paidamount = " + tot_transamount + " WHERE transid = " + stud_list[i].TransactionID + ";";

                                balance = tot_transamount - tot_transamount;
                            }
                        }
                        else
                        {
                            if (tot_transamount > stud_list[i].BatchTotal)
                            {
                                sql_stmt += "UPDATE SAS_Studentloan SET paidamount = " + stud_list[i].BatchTotal + " WHERE transid = " + stud_list[i].TransactionID + ";";

                                balance = tot_transamount - stud_list[i].BatchTotal;
                            }
                            else
                            {
                                sql_stmt += "UPDATE SAS_Studentloan SET paidamount = " + tot_transamount + " WHERE transid = " + stud_list[i].TransactionID + ";";

                                balance = tot_transamount - tot_transamount;
                            }
                        }
                    }

                    //executed query - START
                    if (!FormHelp.IsBlank(sql_stmt))
                    {
                        int liRowAffected = _DatabaseFactory.ExecuteSqlStatement(Helper.GetDataBaseType,
                             DataBaseConnectionString, sql_stmt);

                        if (liRowAffected > -1)
                        { }
                        else
                            throw new Exception("Update Failed! No Row has been updated...");
                    }
                    //executed query - END

                    if (balance >= 0)
                    {
                        tot_transamount = balance;
                    }
                    else
                    {
                        return false;
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                MaxModule.Helper.LogError(ex.Message);

                return false;
            }
        }
        #endregion

        #region GetStudentOutstandingAmtInSponsorAllocation

        /// <summary>
        /// Method to Get Student's Outstanding Amount
        /// </summary>
        /// <param name="argEn">Student Entity is an Input</param>
        /// <returns>Returns the Outstanding Amount</returns>
        public double GetStudentOutstandingAmtInSponsorAllocation(StudentEn argEn)
        {
            //AccountsEn loItem = new AccountsEn();
            List<AccountsEn> loEnList = new List<AccountsEn>();
            double OutAmt = 0.0;
            double debit = 0.0;
            double credit = 0.0;
            string sqlCmd1 = "Select Sum(Debit) As DebitAmount From(" +
            " SELECT CASE WHEN SAS_Accounts.category = 'Credit Note' or  SAS_Accounts.category = 'Debit Note' or SAS_Accounts.category = 'Invoice'" +
            " then case when SAS_Accounts.transtype = 'Debit' then SUM(de.TransAmount) Else 0 End Else SAS_Accounts.TransAmount END Debit " +
            " FROM SAS_Accounts left join sas_accountsdetails de on SAS_Accounts.transid = de.transid where " +
            " (SAS_Accounts.poststatus = 'Posted') and  (sas_accounts.subtype = 'Student') and sas_accounts.subcategory NOT IN ('Loan') and (SAS_Accounts.TransType = 'Debit')  and  " +
            " (sas_accounts.CreditRef =  '" + argEn.MatricNo + "')" +
            " group by SAS_Accounts.TransAmount,SAS_Accounts.category,SAS_Accounts.transtype ) Result ";
            string sqlCmd2 = "Select Sum(credit) As CreditAmount From(" +
          " SELECT CASE WHEN SAS_Accounts.category = 'Credit Note' or  SAS_Accounts.category = 'Debit Note' or SAS_Accounts.category = 'Invoice'" +
          //" then case when SAS_Accounts.transtype = 'Credit' then SUM(de.TransAmount) Else 0 End Else SUM(SAS_Accounts.TransAmount) END credit " +
          " then case when SAS_Accounts.transtype = 'Credit' then SUM(de.TransAmount) Else 0 End Else SAS_Accounts.TransAmount END credit " +
          " FROM SAS_Accounts left join sas_accountsdetails de on SAS_Accounts.transid = de.transid where " +
          " (SAS_Accounts.poststatus = 'Posted') and  (sas_accounts.subtype = 'Student') and sas_accounts.subcategory NOT IN ('Loan') and (SAS_Accounts.TransType = 'Credit')  and  " +
          " (sas_accounts.CreditRef =  '" + argEn.MatricNo + "')" +
          " group by SAS_Accounts.TransAmount,SAS_Accounts.category,SAS_Accounts.transtype ) Result ";

            try
            {

                if (!FormHelp.IsBlank(sqlCmd1))
                {
                    using (IDataReader dr = _DatabaseFactory.ExecuteReader(Helper.GetDataBaseType,
                        DataBaseConnectionString, sqlCmd1).CreateDataReader())
                    {
                        if (dr.Read())
                            debit = GetValue<double>(dr, "DebitAmount");
                    }
                }
                //get creditamount
                if (!FormHelp.IsBlank(sqlCmd2))
                {
                    using (IDataReader dr = _DatabaseFactory.ExecuteReader(Helper.GetDataBaseType,
                        DataBaseConnectionString, sqlCmd2).CreateDataReader())
                    {
                        if (dr.Read())
                            credit = GetValue<double>(dr, "CreditAmount");
                    }
                }

                //total outstanding amount

                OutAmt = debit - credit;


            }
            catch (Exception ex)
            {
                throw ex;
            }
            return OutAmt;
        }

        #endregion

        #region GetSPAllocationTransactionsStudentPayment

        /// <summary>
        /// Method to Get Sponsor Allocations
        /// </summary>
        /// <param name="argEn">Accounts Entity is the Input.BatchCode,TransStatus,Category,PostStatus and SubType are Input Properties.</param>
        /// <returns>Returns List of Accounts</returns>
        public List<AccountsEn> GetSPAllocationTransactionsStudentPayment(AccountsEn argEn)
        {
            //edit by farid 15/2/2016
            List<AccountsEn> loListAccounts = new List<AccountsEn>();
            argEn.BatchCode = argEn.BatchCode.Replace("*", "%");
            string sqlCmd = "SELECT * FROM SAS_Accounts WHERE Category='" +
                          argEn.Category + "' AND PostStatus='" + argEn.PostStatus + "' and SubType ='" + argEn.SubType + "' AND SubCategory='" + argEn.SubCategory + "'";
            if (argEn.BatchCode.Length != 0) sqlCmd = sqlCmd + " AND BatchCode LIKE '" + argEn.BatchCode + "'";

            try
            {
                if (!FormHelp.IsBlank(sqlCmd))
                {
                    using (IDataReader loReader = _DatabaseFactory.ExecuteReader(Helper.GetDataBaseType,
                       DataBaseConnectionString, sqlCmd).CreateDataReader())
                    {
                        while (loReader.Read())
                        {
                            AccountsEn loItem = LoadObject(loReader);
                            loListAccounts.Add(loItem);
                        }
                        loReader.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return loListAccounts;
        }

        #endregion

        #region Subcategoryupdate

        /// <summary>
        /// Method to Insert SponsorBatch
        /// </summary>
        /// <param name="argEn">Accounts Entity is the Input</param>
        /// <param name="argList">List of Sponsor Entity is the Input</param>
        /// <returns>Returns BatchCode</returns>

        public bool Subcategoryupdate(string BatchCode, string UserId)
        //public string Subcategoryupdate(AccountsEn argEn)
        {
            string SqlStatement = null; string UpdateStatement = null; int TransId = 0;

            try
            {
                //Build Sql Statement - Start
                //SqlStatement = "SELECT transid, category from sas_studentloan WHERE batchcode = " + clsGeneric.AddQuotes(BatchCode);                
                SqlStatement = "SELECT * from sas_accounts WHERE batchcode = " + clsGeneric.AddQuotes(BatchCode);
                //Build Sql Statement - Stop

                IDataReader _IDataReader = _DatabaseFactory.ExecuteReader(Helper.GetDataBaseType, DataBaseConnectionString,
                    SqlStatement).CreateDataReader();

                while (_IDataReader.Read())
                {
                    TransId = clsGeneric.NullToInteger(_IDataReader[0]);
                    string transstatus = "Closed";
                    string subcategory = " ";
                    String category = GetValue<string>(_IDataReader, "category");
                    String creditref1 = GetValue<string>(_IDataReader, "creditref1");
                    String subref1 = GetValue<string>(_IDataReader, "subref1");
                    String subref2 = GetValue<string>(_IDataReader, "subref2");
                    String subtype = GetValue<string>(_IDataReader, "subtype");
                    if (category == "Payment" && subtype == "Student")
                    {
                        UpdateStatement += "UPDATE sas_accounts SET subcategory = " + clsGeneric.AddQuotes(subcategory) + clsGeneric.AddComma() + "postedby = " + clsGeneric.AddQuotes(UserId) + ",";
                        UpdateStatement += "postedtimestamp = " + clsGeneric.AddQuotes(Helper.DateConversion(DateTime.Now)) + " WHERE batchcode = " + clsGeneric.AddQuotes(subref2) + ";";
                    }
                }
                if (!FormHelp.IsBlank(UpdateStatement))
                {
                    if (_DatabaseFactory.ExecuteSqlStatement(Helper.GetDataBaseType,
                        DataBaseConnectionString, UpdateStatement) > -1)
                    {
                        return true;
                    }
                }

                return false;
            }
            catch (Exception ex)
            {
                MaxModule.Helper.LogError(ex.Message);

                return false;
            }

        }

        #endregion

        #region GetTransactionsForAllocation

        /// <summary>
        /// Method to Get Transactions
        /// </summary>
        /// <param name="argEn">Accounts Entity is the Input.BatchCode,Category,PostStatus and SubType are the Input Property.</param>
        /// <returns>Returns List of Accounts.</returns>
        public List<AccountsEn> GetTransactionsForAllocation(AccountsEn argEn)
        {
            string sqlCmd;
            List<AccountsEn> loListAccounts = new List<AccountsEn>();
            DateTime dtBatchDate = argEn.BatchDate;
            string strBatchDate = dtBatchDate.ToString();
            argEn.BatchCode = argEn.BatchCode.Replace("*", "%");
            if (argEn.Category == "")
            {
                sqlCmd = "SELECT DISTINCT BatchCode FROM SAS_ACCOUNTS where (Category='Debit Note' OR Category='Credit Note')"
                    + " AND PostStatus='" + argEn.PostStatus + "' AND SubType ='" + argEn.SubType + "' ";
            }

            else
            {

                sqlCmd = "SELECT DISTINCT BatchCode FROM SAS_Accounts WHERE Category='" +
                              argEn.Category + "' AND PostStatus='" + argEn.PostStatus + "' and SubType ='" + argEn.SubType + "' ";
            }

            if (argEn.BatchCode.Length != 0) sqlCmd = sqlCmd + " AND BatchCode LIKE '" + argEn.BatchCode + "' ";

            try
            {
                if (!string.IsNullOrEmpty(argEn.BatchIntake))
                {
                    if (argEn.BatchIntake.Length != 0) sqlCmd = sqlCmd + " AND BatchIntake = '" + argEn.BatchIntake + "' ";
                }
            }
            catch (Exception bt)
            {
                throw bt;
            }
            sqlCmd = sqlCmd + "ORDER BY BatchCode DESC ";

            try
            {
                if (!FormHelp.IsBlank(sqlCmd))
                {
                    using (IDataReader loReader = _DatabaseFactory.ExecuteReader(Helper.GetDataBaseType,
                        DataBaseConnectionString, sqlCmd).CreateDataReader())
                    {
                        while (loReader.Read())
                        {
                            argEn.BatchCode = GetValue<string>(loReader, "BatchCode");
                            loListAccounts.Add(GetItemAllocation(argEn));
                        }
                        loReader.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return loListAccounts;
        }

        #endregion

        #region UpdateSponsorInvoice

        /// <summary>
        /// Method to Update Allocations
        /// </summary>
        /// <param name="argEn">Accounts Entity is the Input.Trnascode,Updateby and UpdatedTime are Input Properties</param>
        /// <returns>Returns boolean</returns>
        public bool UpdateSponsorInvoice(AccountsEn argEn)
        {
            bool lbRes = false;
            string sqlCmd;

            try
            {
                sqlCmd = "UPDATE sas_sponsorinvoice SET UpdatedBy = @UpdatedBy, UpdatedTime = @UpdatedTime WHERE TransCode = @TransCode";

                if (!FormHelp.IsBlank(sqlCmd))
                {
                    DbCommand cmd = _DatabaseFactory.GetDbCommand(Helper.GetDataBaseType, sqlCmd, DataBaseConnectionString);
                    _DatabaseFactory.AddInParameter(ref cmd, "@TransCode", DbType.String, argEn.CreditRef);
                    _DatabaseFactory.AddInParameter(ref cmd, "@UpdatedBy", DbType.String, argEn.UpdatedBy);
                    _DatabaseFactory.AddInParameter(ref cmd, "@UpdatedTime", DbType.DateTime, argEn.UpdatedTime);
                    _DbParameterCollection = cmd.Parameters;

                    int liRowAffected = _DatabaseFactory.ExecuteNonQuery(Helper.GetDataBaseType, cmd,
                                DataBaseConnectionString, sqlCmd, _DbParameterCollection);

                    if (liRowAffected > -1)
                        lbRes = true;
                    else
                        throw new Exception("Update Failed! No Row has been updated...");
                }


            }
            catch (Exception ex)
            {
                throw ex;
            }
            return lbRes;
        }

        #endregion

        #region Update Posting Status SponsorInvoice

        public bool UpdatePostingStatusSponsorInvoice(string BatchCode, string UserId)
        {
            //create instances
            IDataReader _IDataReader = null;

            //variable declarations
            string SqlStatement = null; string UpdateStatement = null; int TransId = 0;

            try
            {
                //Build Sql Statement - Start
                SqlStatement = "SELECT transid from sas_sponsorinvoice WHERE batchcode = "
                    + clsGeneric.AddQuotes(BatchCode);
                //Build Sql Statement - Stop

                //Get Batch Details - Start
                _IDataReader = _DatabaseFactory.ExecuteReader(
                    Helper.GetDataBaseType, DataBaseConnectionString, SqlStatement).CreateDataReader();
                //Get Batch Details - Stop

                //loop thro the batch details - start
                while (_IDataReader.Read())
                {
                    //Get Transaction Id
                    TransId = clsGeneric.NullToInteger(_IDataReader[0]);
                    //Updated Mona 19/2/2016
                    string transstatus = "Open";
                    //string transstatus = "Closed";

                    //Build Update Statement - Start
                    UpdateStatement += "UPDATE sas_sponsorinvoicedetails SET transcode = substring(transtempcode from 2 for Length(transtempcode)),poststatus = 'Posted',transstatus=" + clsGeneric.AddQuotes(transstatus) + " ,transtempcode = '' WHERE transid = " + TransId + ";";
                    UpdateStatement += "UPDATE sas_sponsorinvoice SET transcode = substring(transtempcode from 2 for Length(transtempcode)),poststatus = 'Posted',transstatus=" + clsGeneric.AddQuotes(transstatus) + " ,transtempcode = '',";
                    UpdateStatement += "postedby = " + clsGeneric.AddQuotes(UserId) + clsGeneric.AddComma() + "postedtimestamp = " + clsGeneric.AddQuotes(Helper.DateConversion(DateTime.Now)) + " WHERE transid = " + TransId + ";";

                    //UpdateStatement += "UPDATE sas_sponsorinvoice SET transcode = substring(transtempcode from 2 for Length(transtempcode)),poststatus = 'Posted',transstatus=" + clsGeneric.AddQuotes(transstatus) + ",";
                    //UpdateStatement += "postedby = " + clsGeneric.AddQuotes(UserId) + clsGeneric.AddComma() + "postedtimestamp = " + clsGeneric.AddQuotes(Helper.DateConversion(DateTime.Now)) + " WHERE batchcode = " + clsGeneric.AddQuotes(BatchCode) + ";";

                    //Build Update Statement - Stop
                }
                //loop thro the batch details - stop

                //if update statement successful - Start
                if (!FormHelp.IsBlank(UpdateStatement))
                {
                    if (_DatabaseFactory.ExecuteSqlStatement(Helper.GetDataBaseType,
                        DataBaseConnectionString, UpdateStatement) > -1)
                    {
                        return true;
                    }
                }
                //if update statement successful - Stop

                return false;
            }
            catch (Exception ex)
            {
                MaxModule.Helper.LogError(ex.Message);

                return false;
            }
        }

        #endregion

        #region UpdateReceiptAtStudInvoice
        //added by Hafiz @ 24/3/2016
        //modified by Hafiz @ 22/4/2016
        //update receipt to the invoice list

        public bool UpdateReceiptAtStudInvoice(String BatchCode)
        {
            bool lbRes = false;

            List<AccountsEn> liststud = new List<AccountsEn>();
            List<AccountsEn> studentInvoiceList = new List<AccountsEn>();

            string sqlCmd = sqlCmd = "select * from SAS_Accounts WHERE BatchCode = '" + BatchCode + "'";

            try
            {
                if (!FormHelp.IsBlank(sqlCmd))
                {
                    using (IDataReader loReader = _DatabaseFactory.ExecuteReader(Helper.GetDataBaseType,
                        DataBaseConnectionString, sqlCmd).CreateDataReader())
                    {
                        while (loReader.Read())
                        {
                            AccountsEn loItem = LoadObject(loReader);
                            liststud.Add(loItem);
                        }

                        for (int k = 0; k < liststud.Count; k++)
                        {
                            if (liststud[k].Category == "Receipt")
                            {
                                AccountsEn _AccountsEn = new AccountsEn();

                                _AccountsEn.SubType = "Student";
                                _AccountsEn.PostStatus = "Posted";
                                _AccountsEn.Category = "'Invoice','Debit Note','AFC'";
                                _AccountsEn.CreditRef = liststud[k].CreditRef;
                                _AccountsEn.TransactionAmount = liststud[k].TransactionAmount;

                                studentInvoiceList = GetStudentAutoAllocation(_AccountsEn);

                                for (int i = 0; i < studentInvoiceList.Count; i++)
                                {
                                    AccountsDetailsDAL _AccountsDetailsDAL = new AccountsDetailsDAL();
                                    AccountsDetailsEn obj = new AccountsDetailsEn();
                                    AccountsDetailsEn argEn = new AccountsDetailsEn();

                                    argEn.TransactionID = studentInvoiceList[i].TranssactionID;

                                    try
                                    {
                                        obj = _AccountsDetailsDAL.GetItem(argEn);

                                        if (obj.TransStatus != "Closed")
                                        {
                                            String stats = "";
                                            double total = 0.0;

                                            if (obj.PaidAmount == obj.TransactionAmount)
                                            {
                                                stats = "Closed";
                                                obj.PaidAmount = obj.PaidAmount;
                                            }
                                            else
                                            {
                                                total = obj.PaidAmount + _AccountsEn.TransactionAmount;

                                                if (total < obj.TransactionAmount)
                                                {
                                                    obj.PaidAmount = total;

                                                }
                                                else
                                                {
                                                    _AccountsEn.TransactionAmount = (_AccountsEn.TransactionAmount + obj.PaidAmount) - obj.TransactionAmount;

                                                    stats = "Closed";
                                                    obj.PaidAmount = obj.TransactionAmount;

                                                }
                                            }

                                            UpdateReceiptAtStudInvoiceStatus(obj.TransactionID, obj.PaidAmount, stats);
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        throw ex;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return lbRes;
        }

        #endregion

        #region UpdateReceiptAtStudInvoiceStatus
        //added by Hafiz @ 24/3/2016
        //modified by Hafiz @ 22/4/2016
        //update invoice list status to closed whenever transamount=paidamount

        public bool UpdateReceiptAtStudInvoiceStatus(int transId, double PaidAmount, String stats)
        {
            bool lbRes = false;
            String sqlCmd = "UPDATE SAS_AccountsDetails SET PaidAmount = '" + PaidAmount + "' ";

            if (stats == "Closed")
            {
                sqlCmd = sqlCmd + ",TransStatus = 'Closed' ";
            }

            sqlCmd = sqlCmd + "WHERE TransId = '" + transId + "' AND PostStatus = 'Posted' ";

            try
            {
                if (!FormHelp.IsBlank(sqlCmd))
                {
                    int liRowAffected = _DatabaseFactory.ExecuteSqlStatement(
                        Helper.GetDataBaseType, DataBaseConnectionString, sqlCmd);

                    if (liRowAffected > -1)
                        lbRes = true;
                    else
                        throw new Exception("Update Failed! No Row has been updated...");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return lbRes;
        }

        #endregion

        #region Update Posting Status - Closed

        public bool UpdatePostingStatusClosed(string BatchCode, string UserId)
        {
            //create instances
            IDataReader _IDataReader = null;

            //variable declarations
            string SqlStatement = null; string UpdateStatement = null; int TransId = 0;

            try
            {
                //Build Sql Statement - Start
                SqlStatement = "SELECT transid from sas_accounts WHERE batchcode = "
                    + clsGeneric.AddQuotes(BatchCode);
                //Build Sql Statement - Stop

                //Get Batch Details - Start
                _IDataReader = _DatabaseFactory.ExecuteReader(
                    Helper.GetDataBaseType, DataBaseConnectionString, SqlStatement).CreateDataReader();
                //Get Batch Details - Stop

                //loop thro the batch details - start
                while (_IDataReader.Read())
                {
                    //Get Transaction Id
                    TransId = clsGeneric.NullToInteger(_IDataReader[0]);
                    
                    string transstatus = "Closed";

                    //Build Update Statement - Start
                    UpdateStatement += "UPDATE sas_accountsdetails SET transcode = substring(transtempcode from 2 for Length(transtempcode)),poststatus = 'Posted',transstatus=" + clsGeneric.AddQuotes(transstatus) + " ,transtempcode = '' WHERE transid = " + TransId + ";";
                    UpdateStatement += "UPDATE sas_accounts SET transcode = substring(transtempcode from 2 for Length(transtempcode)),poststatus = 'Posted',transstatus=" + clsGeneric.AddQuotes(transstatus) + " ,transtempcode = '',";
                    UpdateStatement += "postedby = " + clsGeneric.AddQuotes(UserId) + clsGeneric.AddComma() + "postedtimestamp = " + clsGeneric.AddQuotes(Helper.DateConversion(DateTime.Now)) + " WHERE transid = " + TransId + ";";
                    //Build Update Statement - Stop
                }
                //loop thro the batch details - stop

                //if update statement successful - Start
                if (!FormHelp.IsBlank(UpdateStatement))
                {
                    if (_DatabaseFactory.ExecuteSqlStatement(Helper.GetDataBaseType,
                        DataBaseConnectionString, UpdateStatement) > -1)
                    {
                        //Added by Hafiz Roslan @ 4/2/2016 - Check for Category = Loan
                        //updated on 10/2/2016 - category change back to receipt
                        if (UpdatePostingOnStudLoan(BatchCode, UserId)) { }

                        //updated of 11/2/2015
                        if (UpdateStudLoanPaidAmt(BatchCode)) { }

                        return true;
                    }
                }
                //if update statement successful - Stop

                return false;
            }
            catch (Exception ex)
            {
                MaxModule.Helper.LogError(ex.Message);

                return false;
            }
        }

        #endregion

        #region UpdateOutAmt
        //added by Hafiz @ 25/4/2016
        //modified by Hafiz @ 21/11/2016

        public bool UpdateOutAmt(String BatchCode)
        {
            bool result = false;

            try
            {
                using (IDataReader loReader = _DatabaseFactory.Update_OutstandingAmount(BatchCode).CreateDataReader())
                {
                    if (loReader != null)
                    {
                        if (loReader.Read().Equals(true))
                        {
                            result = true;
                        }
                    }
                    loReader.Close();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return result;
        }

        #endregion

        #region UpdateNewOutstandingAmt
        //added by Hafiz @ 25/4/2016
        //modified by Hafiz @ 27/5/2016
        //update outstanding amount


        public bool UpdateNewOutstandingAmt(String matricNo, double NewAmount)
        {
            bool res = false;
            String sqlCmd = "";

            try
            {
                sqlCmd = "UPDATE SAS_StudentOutstanding SET SASO_Outstandingamt = '" + NewAmount + "' WHERE SASI_MatricNo = '" + matricNo + "' ";

                if (!FormHelp.IsBlank(sqlCmd))
                {
                    int liRowAffected = _DatabaseFactory.ExecuteSqlStatement(
                        Helper.GetDataBaseType, DataBaseConnectionString, sqlCmd);

                    if (liRowAffected > -1)
                        res = true;
                    else
                        throw new Exception("Update Failed! No Row has been updated...");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }


            return res;
        }

        #endregion

        #region UpdateOutAmtTblSASAcc
        //added by Hafiz @ 25/4/2016
        //update Receipt Outstanding Amount at SAS_Accounts

        public bool UpdateOutAmtTblSASAcc(String BatchCode, String matricNo, double outamt)
        {
            bool res = false;
            String sqlCmd = "";

            try
            {
                sqlCmd = "UPDATE SAS_Accounts SET OutstandingAmt = '" + outamt + "' WHERE BatchCode = '" + BatchCode + "' " +
                         "AND Creditref = '" + matricNo + "' ";

                if (!FormHelp.IsBlank(sqlCmd))
                {
                    int liRowAffected = _DatabaseFactory.ExecuteSqlStatement(
                        Helper.GetDataBaseType, DataBaseConnectionString, sqlCmd);

                    if (liRowAffected > -1)
                        res = true;
                    else
                        throw new Exception("Update Failed! No Row has been updated...");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }


            return res;
        }

        #endregion

        #region InsertOutstandingAmountAfterPosted
        //added by Hafiz @ 25/4/2016
        //modified by Hafiz @ 27/5/2016
        //added unavailble student into the sas_outstanding

        public bool InsertOutstandingAmountAfterPosted(AccountsEn loItem)
        {
            bool res = false;

            StudentEn stuobj = new StudentEn();
            StudentDAL _StudentDAL = new StudentDAL();
            List<StudentEn> list_acc = new List<StudentEn>();

            stuobj.MatricNo = loItem.CreditRef;

            list_acc = _StudentDAL.GetList(stuobj);

            foreach (StudentEn acc_obj in list_acc)
            {
                AccountsEn accobj = new AccountsEn();

                accobj.CreditRef = acc_obj.MatricNo;
                accobj.PostStatus = "Posted";
                accobj.SubType = "Student";
                accobj.TransType = "";
                accobj.TransStatus = "";

                double cr = 0.0, dr = 0.0;

                List<AccountsEn> ledger = GetStudentLedgerCombine(accobj);
                foreach (var ldgr in ledger)
                {
                    if (ldgr.TransType == "Credit")
                    {
                        cr = cr + ldgr.TransactionAmount;
                    }
                    else if (ldgr.TransType == "Debit")
                    {
                        if (ldgr.Category != "Receipt")
                        {
                            dr = dr + ldgr.TransactionAmount;
                        }
                        else
                        {
                            cr = cr + ldgr.TransactionAmount;
                        }
                    }
                }

                acc_obj.OutstandingAmount = dr - cr;
                if (_StudentDAL.InsStudOutstanding(acc_obj) != false) { res = true; }
            }

            return res;
        }

        #endregion

        #region GetAccList
        //added by Hafiz @ 27/5/2016

        public List<AccountsEn> GetAccList()
        {
            List<AccountsEn> loEnList = new List<AccountsEn>();
            string sqlCmd = "select * from SAS_Accounts";

            try
            {
                if (!FormHelp.IsBlank(sqlCmd))
                {
                    using (IDataReader loReader = _DatabaseFactory.ExecuteReader(Helper.GetDataBaseType,
                       DataBaseConnectionString, sqlCmd).CreateDataReader())
                    {
                        while (loReader.Read())
                        {
                            AccountsEn loItem = LoadObject(loReader);
                            loEnList.Add(loItem);
                        }
                        loReader.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return loEnList;
        }

        #endregion

        #region UpdateRecpForSponsCreditDebitNote
        //added by Hafiz @ 27/5/2016
        //update receipt amounts that affected Sponsor Debit/Note

        public bool UpdateRecpForSponsCreditDebitNote(string BatchCode)
        {
            bool result = false;
            double newamt = 0.0;

            try
            {
                string sqlCmd = "select * from SAS_Accounts WHERE BatchCode = '" + BatchCode + "'";

                if (!FormHelp.IsBlank(sqlCmd))
                {
                    using (IDataReader loReader = _DatabaseFactory.ExecuteReader(Helper.GetDataBaseType,
                        DataBaseConnectionString, sqlCmd).CreateDataReader())
                    {
                        while (loReader.Read())
                        {
                            AccountsEn accobj = LoadObject(loReader);
                            if (accobj.SubType == "Sponsor" && (accobj.Category == "Credit Note" || accobj.Category == "Debit Note"))
                            {
                                AccountsEn recpt = GetAccList().Where(x => x.BatchCode == accobj.CreditRefOne).FirstOrDefault();
                                recpt.AllocatedAmount = GetSponserStuAllocateAmount(recpt.BatchCode.ToString());

                                if (accobj.TransactionAmount > 0)
                                {
                                    //credit - increased
                                    if (accobj.TransType == "Credit")
                                    {
                                        newamt = Convert.ToDouble(recpt.AllocatedAmount) + accobj.TransactionAmount;
                                    }
                                    //debit - decreased
                                    else if (accobj.TransType == "Debit")
                                    {
                                        newamt = Convert.ToDouble(recpt.AllocatedAmount) - accobj.TransactionAmount;
                                    }
                                }

                                result = UpdateRecpAmtForSponsCreditDebitNote(recpt.TranssactionID, newamt);

                            }
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return result;
        }

        #endregion

        #region UpdateRecpAmtForSponsCreditDebitNote
        //added by Hafiz @ 27/5/2016

        public bool UpdateRecpAmtForSponsCreditDebitNote(int RcptTransId, double newAmount)
        {
            bool res = false;
            String sqlCmd = "";

            try
            {
                if (newAmount <= 0)
                {
                    sqlCmd = "UPDATE SAS_Accounts SET AllocateAmount = 0 ";
                }
                else
                {
                    sqlCmd = "UPDATE SAS_Accounts SET AllocateAmount = '" + newAmount + "' ";
                }

                sqlCmd = sqlCmd + "WHERE TransId = '" + RcptTransId + "';";

                if (!FormHelp.IsBlank(sqlCmd))
                {
                    int liRowAffected = _DatabaseFactory.ExecuteSqlStatement(
                        Helper.GetDataBaseType, DataBaseConnectionString, sqlCmd);

                    if (liRowAffected > -1)
                        res = true;
                    else
                        throw new Exception("Update Failed! No Row has been updated...");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return res;
        }

        #endregion

        #region GetStudentLedgerCombine
        //added by Hafiz @ 27/5/2016
        //Student Ledger new RA - combine Stud Ledger + Loan Ledger

        public List<AccountsEn> GetStudentLedgerCombine(AccountsEn argEn)
        {
            string sqlCmd = String.Empty;
            List<AccountsEn> loEnList = new List<AccountsEn>();
            argEn.TransType = argEn.TransType.Replace("*", "%");

            sqlCmd = "SELECT * FROM ( ";

            //Student Ledger - Start
            sqlCmd += @"(SELECT acc.PostedTimestamp, 
                            CASE WHEN acc.Category = 'Receipt' THEN
	                            ( SELECT date_time FROM SAS_workflow_status WHERE workflow_id = 
		                          (SELECT workflow_id FROM SAS_workflow
		                          WHERE batch_code=acc.BatchCode)
	                              AND workflow_status=2
	                              ORDER by date_time DESC
	                              LIMIT 1 )
	                        ELSE acc.TransDate
	                        END TransDate,
                            CASE WHEN acc.Category = 'Receipt' THEN acc.BatchCode
			                     ELSE
				                    CASE WHEN Category = 'Loan' THEN BatchCode
				                         ELSE acc.TransCode
				                    END
			                END TransCode,                            
                        acc.Description, acc.Category ,
                            CASE WHEN acc.category = 'Credit Note' or acc.category = 'Debit Note' or acc.category = 'Invoice' THEN
                                CASE
                                    WHEN acc.transtype = 'Debit' then SUM(de.TransAmount)
                                    ELSE 0
                                END
                            ELSE 
                                CASE 
                                    WHEN acc.TransType = 'Debit' THEN acc.TransAmount
                                    ELSE 0
                                END 
                            END Debit,
                            CASE WHEN acc.category = 'Credit Note' or acc.category = 'Debit Note' or acc.category = 'Invoice' THEN 
                                CASE 
                                    WHEN acc.transtype = 'Credit' Then SUM(de.transamount)
                                    ELSE 0
                                END
                            ELSE     
                                CASE 
                                    WHEN acc.TransType = 'Credit' THEN acc.TransAmount
                                    ELSE 0
                                END
                            END Credit,
                            CASE WHEN acc.category = 'Credit Note' or acc.category = 'Debit Note' or acc.category = 'Invoice' THEN 
                                SUM(de.TransAmount)
                            ELSE
                                acc.TransAmount
                            END TransAmount,    
                            CASE WHEN TransType = 'Credit' THEN 
			                     CASE WHEN Category = 'Loan' THEN 'Debit'
			                     ELSE 'Credit'
			                     END
			                ELSE 'Debit'	
			                END TransType,
                        acc.BatchCode
                    FROM SAS_Accounts acc
                    left join sas_accountsdetails de on acc.transid = de.transid
                    WHERE  acc.CreditRef = '" + argEn.CreditRef + "' AND acc.SubType = '" + argEn.SubType + "' AND acc.PostStatus ='" + argEn.PostStatus + "' ";

            if (argEn.TransType.Length != 0) sqlCmd += " AND acc.TransType LIKE '" + argEn.TransType + "'";

            sqlCmd += @"AND acc.subcategory NOT IN ('Loan') AND acc.TransAmount > 0 
                        GROUP BY acc.TransDate,  acc.ReceiptDate, acc.BankRecNo, acc.Description, acc.Category, acc.TransCode, acc.TransType, acc.BatchCode, 
                        acc.TransAmount, acc.postedtimestamp) ";
            //Student Ledger - End

            sqlCmd += "UNION ";

            //Loan Ledger - Start
            sqlCmd += @"(SELECT SAS_StudentLoan.PostedTimestamp, TransDate, TransCode, Description, Category,
                            --debit
			                CASE WHEN Category = 'Receipt' THEN 
			                     CASE WHEN TransType = 'Debit' THEN TransAmount
			                     ELSE 0
			                     END
			                ELSE
			                     CASE WHEN TransType = 'Credit' THEN TransAmount
			                     ELSE 0
			                     END
			                END Debit,
			                --credit
			                CASE WHEN Category = 'Receipt' THEN 
			                     CASE WHEN TransType = 'Credit' THEN TransAmount
			                     ELSE 0
			                     END
			                ELSE
			                     CASE WHEN TransType = 'Debit' THEN TransAmount
			                     ELSE 0
			                     END
			                END Credit,
                        TransAmount, 
                            CASE WHEN TransType = 'Credit' THEN 
			                        CASE WHEN Category = 'Loan' THEN 'Debit'
			                        ELSE 'Credit'	
			                        END
			                ELSE 'Debit'
			                END TransType,
                        BatchCode
                    FROM SAS_StudentLoan
                    WHERE CreditRef = '" + argEn.CreditRef + "' AND SubType = '" + argEn.SubType + "'and PostStatus ='" + argEn.PostStatus + "') ";
            //Loan Ledger - End

            sqlCmd += ") ledger ORDER BY ledger.PostedTimestamp ";

            try
            {
                if (!FormHelp.IsBlank(sqlCmd))
                {
                    using (IDataReader loReader = _DatabaseFactory.ExecuteReader(Helper.GetDataBaseType,
                                   DataBaseConnectionString, sqlCmd).CreateDataReader())
                    {
                        while (loReader.Read())
                        {
                            AccountsEn loItem = LoadStudentLedgerObject(loReader);
                            loEnList.Add(loItem);
                        }
                        loReader.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return loEnList;
        }

        #endregion

        #region GetAFCDetails
        //added by Hafiz @ 02/6/2016; created by Mona
        //Get AFC Details (View) used by Student Ledger

        public DataSet GetAFCDetails(string MatricNo, string CurSem, string BatchCode)
        {
            string sqlCmd = string.Empty;
            StudentDAL _StudentDAL = new StudentDAL();

            try
            {
                StudentEn studobj = _StudentDAL.GetItem(MatricNo);

                sqlCmd = "SELECT b.SAFT_Desc,a.SAFT_Code,a.SAFD_Type,a.SAFA_Amount,a.SAST_Code,a.SAFD_Sem FROM ( ";

                sqlCmd += "SELECT SAS_FeeStrAmount.SAFS_Code, SAS_FeeStrAmount.SAFA_Amount, SAS_FeeStrAmount.SAFD_Type,SAS_FeeStrAmount.SAFT_Code, " +
                          "SAS_FeeStruct.sast_code,SAS_FeeStrAmount.safd_sem FROM SAS_FeeStrAmount " +
                          "INNER JOIN SAS_FeeStrDetails ON SAS_FeeStrAmount.SAFS_Code = SAS_FeeStrDetails.SAFS_Code " +
                          "INNER JOIN SAS_FeeStruct ON SAS_FeeStrDetails.SAFS_Code = SAS_FeeStruct.SAFS_Code " +
                          "INNER JOIN SAS_FeeTypes ON SAS_FeeStrDetails.SAFT_Code = SAS_FeeTypes.SAFT_Code " +
                          "WHERE (SAS_FeeStrAmount.SASC_Code in (SELECT SASC_Code FROM sas_student where sasi_matricno = '" + MatricNo + "'))" +
                          "AND SAS_FeeStruct.sabp_code IN (SELECT sabp_code FROM sas_program WHERE sapg_code in (SELECT sasi_pgid FROM sas_student " +
                          //"where sasi_matricno = '" + MatricNo + "')) AND SAS_FeeStruct.sast_code = '" + CurSem + "' AND SAS_FeeStrAmount.SAFD_Type = 'A' " +
                          "where sasi_matricno = '" + MatricNo + "')) AND SAS_FeeStruct.sast_code = '" + studobj.Intake + "' AND SAS_FeeStrAmount.SAFD_Type = 'A' " +
                          "GROUP BY SAS_FeeStrAmount.SAFS_Code, SAS_FeeStrAmount.SAFA_Amount, SAS_FeeStrAmount.SAFD_Type,SAS_FeeStrAmount.SAFT_Code, " +
                          "SAS_FeeStruct.sast_code,SAS_FeeStrAmount.safd_sem ";
                sqlCmd += "UNION ";
                sqlCmd += "SELECT SAS_FeeStrAmount.SAFS_Code, SAS_FeeStrAmount.SAFA_Amount, SAS_FeeStrAmount.SAFD_Type,SAS_FeeStrAmount.SAFT_Code, " +
                            "SAS_FeeStruct.sast_code,SAS_FeeStrAmount.safd_sem FROM SAS_FeeStrAmount " +
                            "INNER JOIN SAS_FeeStrDetails ON SAS_FeeStrAmount.SAFS_Code = SAS_FeeStrDetails.SAFS_Code " +
                            "INNER JOIN SAS_FeeStruct ON SAS_FeeStrDetails.SAFS_Code = SAS_FeeStruct.SAFS_Code " +
                            "INNER JOIN SAS_FeeTypes ON SAS_FeeStrDetails.SAFT_Code = SAS_FeeTypes.SAFT_Code " +
                            "WHERE (SAS_FeeStrAmount.SASC_Code in (SELECT SASC_Code FROM sas_student where sasi_matricno = '" + MatricNo + "'))" +
                            "AND SAS_FeeStruct.sabp_code IN (SELECT sabp_code FROM sas_program WHERE sapg_code in (SELECT sasi_pgid FROM sas_student " +
                            //"where sasi_matricno  = '" + MatricNo + "')) AND SAS_FeeStruct.sast_code = '" + CurSem + "' AND SAS_FeeStrAmount.SAFD_Type = 'S' " +
                            "where sasi_matricno  = '" + MatricNo + "')) AND SAS_FeeStruct.sast_code = '" + studobj.Intake + "' AND SAS_FeeStrAmount.SAFD_Type = 'S' " +
                            "GROUP BY SAS_FeeStrAmount.SAFS_Code,SAS_FeeStrAmount.SAFA_Amount,SAS_FeeStrAmount.SAFD_Type, SAS_FeeStrAmount.SAFT_Code,SAS_FeeStruct.sast_code,SAS_FeeStrAmount.safd_sem ";
                sqlCmd += "UNION ";
                sqlCmd += "SELECT SAS_FeeStrAmount.SAFS_Code,CASE WHEN SAS_FeeStruct.safd_feebaseon = '1' THEN (SAS_FeeStrAmount.SAFA_Amount * " +
                            "(SELECT sasi_crdithrs FROM sas_student where sasi_matricno= '" + MatricNo + "')) ELSE SAS_FeeStrAmount.SAFA_Amount END as SAFA_Amount, " +
                            "SAS_FeeStrAmount.SAFD_Type,SAS_FeeStrAmount.SAFT_Code,SAS_FeeStruct.sast_code,(SAS_FeeStruct.safd_feebaseon:: integer) as safd_sem " +
                            "FROM SAS_FeeStrAmount INNER JOIN SAS_FeeStrDetails ON SAS_FeeStrAmount.SAFS_Code = SAS_FeeStrDetails.SAFS_Code " +
                            "INNER JOIN SAS_FeeStruct ON SAS_FeeStrDetails.SAFS_Code = SAS_FeeStruct.SAFS_Code INNER JOIN SAS_FeeTypes ON " +
                            "SAS_FeeStrDetails.SAFT_Code = SAS_FeeTypes.SAFT_Code WHERE (SAS_FeeStrAmount.SASC_Code in (SELECT SASC_Code FROM sas_student " +
                            "where sasi_matricno= '" + MatricNo + "')) and SAS_FeeStruct.sabp_code IN (SELECT sabp_code FROM sas_program WHERE sapg_code in " +
                            //"(SELECT sasi_pgid FROM sas_student where sasi_matricno = '" + MatricNo + "')) and SAS_FeeStruct.sast_code = '" + CurSem + "'" +
                            "(SELECT sasi_pgid FROM sas_student where sasi_matricno = '" + MatricNo + "')) and SAS_FeeStruct.sast_code = '" + studobj.Intake + "'" +
                            "AND SAS_FeeStrAmount.SAFD_Type = 'T' GROUP BY SAS_FeeStrAmount.SAFS_Code, SAS_FeeStrAmount.SAFA_Amount, SAS_FeeStrAmount.SAFD_Type, " +
                            "SAS_FeeStrAmount.SAFT_Code,SAS_FeeStruct.sast_code,SAS_FeeStruct.safd_feebaseon ";
                sqlCmd += "UNION ";
                sqlCmd += "SELECT SA.sahs_code as safs_code, SA.SAHA_Amount AS SAfa_Amount, 'H' as SAFD_Type, SA.saft_code, SS.sasi_cursemyr as sast_code, " +
                            "SS.sasi_cursem as safd_sem from SAS_Student SS,SAS_HostelStruct SH,SAS_HostelStrAmount SA,SAS_FeeTypes SF " +
                            "where SASI_Hostel = true and SASI_MatricNo = '" + MatricNo + "' and (SS.SAKO_Code = SH.SAHB_Code and SS.SABK_Code = SH.SAHB_Block) " +
                            "and SH.SAHS_Code = SA.SAHS_Code and SS.SASI_FeeCat = SA.SASC_Code and SF.SAFT_Code = SA.SAFT_Code and  sasi_hostel = true ";
                sqlCmd += "UNION ";
                sqlCmd += "SELECT SK.SAKO_Code As safs_code, case when (select sasi_hostel from sas_student where sasi_matricno  = '" + MatricNo + "') = true then " +
                            "sum (SKD.sakod_feeamount) else sum (SKD.sakod_feeamountout) end as SAfa_Amount, SKD.safd_type, SKD.saft_code, " +
                            "SS.sasi_cursemyr as sast_code, SS.sasi_cursem as safd_sem FROM SAS_Student SS, SAS_Kokorikulum SK,SAS_KokorikulumDetails SKD " +
                            "where sasi_matricno = '" + MatricNo + "' AND SS.sasi_kokocode <> '-1' and SKD.SAKO_Code = SK.SAKO_Code and " +
                            "SKD.sako_code = SS.sasi_kokocode and SKD.sakod_categorycode = SS.sasc_code group by SK.SAKO_Code,SKD.safd_type, " +
                            "SKD.saft_code,SS.sasi_cursemyr,SS.sasi_cursem ";

                sqlCmd += ") a INNER JOIN SAS_FeeTypes b ON a.saft_code = b.saft_code ";

                if (studobj.CurrentSemester != 1)
                {
                    sqlCmd += "WHERE a.SAFD_type != 'A' AND a.SAFD_sem IN (0,'" + studobj.CurrentSemester + "') ";
                }
                else
                {
                    sqlCmd += "WHERE a.SAFD_sem IN (0,'" + studobj.CurrentSemester + "') ";
                }

                sqlCmd = sqlCmd + "ORDER by a.SAFD_Type ";

                //execute query - start
                if (!FormHelp.IsBlank(sqlCmd))
                {
                    DataSet _DataSet = _DatabaseFactory.ExecuteDataSet(Helper.GetDataBaseType, DataBaseConnectionString, sqlCmd);
                    return _DataSet;
                }
                //execute query - end
            }
            catch (Exception ex)
            {
                MaxModule.Helper.LogError(ex.Message);

            }
            return null;
        }

        #endregion

        #region Update Posting Status For Credit Note

        public bool UpdatePostingStatusForAFCFLAGreverse(string BatchCode)
        {
            //create instances
            IDataReader _IDataReader = null;

            //variable declarations
            string SqlStatement = null; string UpdateStatement = null;

            try
            {
                //Build Sql Statement - Start
                SqlStatement = "SELECT creditref from sas_accounts WHERE batchcode = "
                    + clsGeneric.AddQuotes(BatchCode);
                //Build Sql Statement - Stop

                //Get Batch Details - Start
                _IDataReader = _DatabaseFactory.ExecuteReader(
                    Helper.GetDataBaseType, DataBaseConnectionString, SqlStatement).CreateDataReader();
                //Get Batch Details - Stop

                //loop thro the batch details - start
                while (_IDataReader.Read())
                {
                    String creditref = GetValue<string>(_IDataReader, "creditref");
                    //Updated Mona 19/2/2016
                    //string transstatus = "Open";


                    //Build Update Statement - Start
                    UpdateStatement += "UPDATE sas_student SET sasi_poststatus = '0' WHERE sasi_matricno = " + clsGeneric.AddQuotes(creditref) + ";";

                    //Build Update Statement - Stop
                }
                //loop thro the batch details - stop

                //if update statement successful - Start
                if (!FormHelp.IsBlank(UpdateStatement))
                {
                    if (_DatabaseFactory.ExecuteSqlStatement(Helper.GetDataBaseType,
                        DataBaseConnectionString, UpdateStatement) > -1)
                    {
                        return true;
                    }
                }

                return false;
                //if update statement successful - Stop
            }
            catch (Exception ex)
            {
                MaxModule.Helper.LogError(ex.Message);

                return false;
            }
        }

        #endregion

        #region GetStudentSponsorAmtInSponsorAllocation

        /// <summary>
        /// Method to Get Student's Outstanding Amount
        /// </summary>
        /// <param name="argEn">Student Entity is an Input</param>
        /// <returns>Returns the Outstanding Amount</returns>
        public double GetStudentSponsorAmtInSponsorAllocation(StudentEn argEn)
        {
            //AccountsEn loItem = new AccountsEn();
            List<AccountsEn> loEnList = new List<AccountsEn>();
            double SponAmt = 0.0;
            string sqlCmd = " select Sum(transamount) from sas_sponsorinvoicedetails where transid in " +
                "(select transid from sas_sponsorinvoice where batchcode ='" + argEn.BatchCode + "') and ref3 = '" + argEn.MatricNo + "'";
            try
            {

                if (!FormHelp.IsBlank(sqlCmd))
                {
                    using (IDataReader dr = _DatabaseFactory.ExecuteReader(Helper.GetDataBaseType,
                        DataBaseConnectionString, sqlCmd).CreateDataReader())
                    {
                        if (dr.Read())
                            SponAmt = GetValue<double>(dr, "Sum");
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return SponAmt;
        }

        #endregion

    }

}


