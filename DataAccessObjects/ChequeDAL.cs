#region NameSpaces 

using System;
using System.Data;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data.Common;
using HTS.SAS.Entities;
using MaxGeneric;

#endregion

namespace HTS.SAS.DataAccessObjects
{
    /// <summary>
    /// Class to handle all the Cheque Management Methods.
    /// </summary>
    public class ChequeDAL
    {
        #region Global Declarations

        private DbParameterCollection _DbParameterCollection = null;

        private MaxModule.DatabaseProvider _DatabaseFactory =
            new MaxModule.DatabaseProvider();

        private string DataBaseConnectionString = Helper.
           GetConnectionString();

        #endregion

        public ChequeDAL()
        {
        }

        #region GetList 

        /// <summary>
        /// Method to Get Cheques
        /// </summary>
        /// <param name="argEn">Cheques Entity is an Input.ProcessID is Input Property.</param>
        /// <returns>Returns List of Cheques.</returns>
        public List<ChequeEn> GetList(ChequeEn argEn)
        {
            List<ChequeEn> loEnList = new List<ChequeEn>();
            argEn.ProcessID = argEn.ProcessID.Replace("*", "%");
            string sqlCmd = "SELECT SAS_Cheque.*, SAS_Accounts.TransCode, SAS_Cheque.PaymentNo, SAS_Accounts.BankCode,SAS_Accounts.transid,SAS_Accounts.BatchCode FROM  SAS_Cheque INNER JOIN SAS_Accounts ON SAS_Cheque.PaymentNo = SAS_Accounts.TransCode where SAS_Cheque.PaymentType = '" + argEn.PaymentType + "' and SAS_Cheque.PrintStatus = '"+argEn.PrintStatus+"'";
            if (argEn.ProcessID.Length != 0) sqlCmd = sqlCmd + " AND ProcessID LIKE '" + argEn.ProcessID + "'";
            
            try
            {
                if (!FormHelp.IsBlank(sqlCmd))
                {
                    using (IDataReader loReader = _DatabaseFactory.ExecuteReader(Helper.GetDataBaseType,
                        DataBaseConnectionString, sqlCmd).CreateDataReader())
                    {
                        while (loReader.Read())
                        {
                            ChequeDetailsDAL dscheDetials = new ChequeDetailsDAL();
                            ChequeDetailsEn locheDetials = new ChequeDetailsEn();
                            ChequeEn loItem = LoadObject(loReader);
                            locheDetials.ProcessId= loItem.ProcessID;
                            loItem.ChequeDetailslist = dscheDetials.GetList(locheDetials);
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

        #region GetRefundList 

        /// <summary>
        /// Method to Get refund Cheques
        /// </summary>
        /// <param name="argEn">Cheques Entity is an Input.ProcessID is Input Property.</param>
        /// <returns>Returns List of Cheques</returns>

        public List<ChequeEn> GetRefundList(ChequeEn argEn)
        {
            List<ChequeEn> loEnList = new List<ChequeEn>();
            argEn.ProcessID = argEn.ProcessID.Replace("*", "%");
            string sqlCmd = "SELECT SAS_Cheque.*, SAS_Accounts.TransCode, SAS_Cheque.PaymentNo, SAS_Accounts.BankCode,SAS_Accounts.transid,SAS_Accounts.BatchCode FROM  SAS_Cheque INNER JOIN SAS_Accounts ON SAS_Cheque.PaymentNo = SAS_Accounts.BatchCode where SAS_Cheque.PaymentType = '" + argEn.PaymentType + "' and SAS_Cheque.PrintStatus = '" + argEn.PrintStatus + "'";
            if (argEn.ProcessID.Length != 0) sqlCmd = sqlCmd + " AND ProcessID LIKE '" + argEn.ProcessID + "'";
            
            try
            {
                if (!FormHelp.IsBlank(sqlCmd))
                {
                    using (IDataReader loReader = _DatabaseFactory.ExecuteReader(Helper.GetDataBaseType,
                        DataBaseConnectionString, sqlCmd).CreateDataReader())
                    {
                        while (loReader.Read())
                        {
                            ChequeDetailsDAL dscheDetials = new ChequeDetailsDAL();
                            ChequeDetailsEn locheDetials = new ChequeDetailsEn();
                            ChequeEn loItem = LoadObject(loReader);
                            locheDetials.ProcessId = loItem.ProcessID;
                            loItem.ChequeDetailslist = dscheDetials.GetList(locheDetials);
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

        #region GetItem 

        /// <summary>
        /// Method to Get Cheque Item
        /// </summary>
        /// <param name="argEn">Cheques Entity is an Input.ProcessID is Input Property.</param>
        /// <returns>Returns a Cheque Item</returns>
        public ChequeEn GetItem(ChequeEn argEn)
        {
            ChequeEn loItem = new ChequeEn();
            string sqlCmd = "Select * FROM SAS_Cheque WHERE ProcessID = @ProcessID";
            
            try
            {
                if (!FormHelp.IsBlank(sqlCmd))
                {
                    DbCommand cmd = _DatabaseFactory.GetDbCommand(Helper.GetDataBaseType, sqlCmd, DataBaseConnectionString);
                    _DatabaseFactory.AddInParameter(ref cmd, "@ProcessID", DbType.String, argEn.ProcessID);
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

        #region Insert 

        /// <summary>
        /// Method to Insert Cheques
        /// </summary>
        /// <param name="argEn">Cheques Entity is the Input.</param>
        /// <returns>Returns a Boolean</returns>
        public string Insert(ChequeEn argEn)
        {
            bool lbRes = false;
            int iOut = 0;
            string batchcode = "";
            string sqlCmd = "Select count(*) as cnt From SAS_Cheque WHERE PaymentNo = @PaymentNo and printstatus = 'Ready'";
            try
            {
                if (!FormHelp.IsBlank(sqlCmd))
                {
                    DbCommand cmdsel = _DatabaseFactory.GetDbCommand(Helper.GetDataBaseType, sqlCmd, DataBaseConnectionString);
                    _DatabaseFactory.AddInParameter(ref cmdsel, "@PaymentNo", DbType.String, argEn.PaymentNo);
                    _DbParameterCollection = cmdsel.Parameters;

                    using (IDataReader dr = _DatabaseFactory.GetIDataReader(Helper.GetDataBaseType, cmdsel,
                        DataBaseConnectionString, sqlCmd, _DbParameterCollection).CreateDataReader())
                    {
                        if (dr.Read())
                            iOut = clsGeneric.NullToInteger(dr["cnt"]);
                        if (iOut > 0)
                            throw new Exception("Record Already Exist");
                    }
                    if (iOut == 0)
                    {
                                             
                        if (argEn.ProcessID == "")
                        {
                            argEn.ProcessID = GetAutoNumber("TCQ");
                        }
                        if (argEn.PrintStatus == "Posted")
                        {
                            argEn.ProcessID = GetAutoNumber("CQ");
                        }
                        batchcode = argEn.ProcessID;

                        string sqlCmd1 = "INSERT INTO SAS_Cheque(ProcessID,PaymentNo,PaymentType,Description,ChequeDate,TransactionDate,PrintStatus,UpdatedBy,UpdatedTime) VALUES (@ProcessID,@PaymentNo,@PaymentType,@Description,@ChequeDate,@TransactionDate,@PrintStatus,@UpdatedBy,@UpdatedTime)";

                        if (!FormHelp.IsBlank(sqlCmd1))
                        {
                            DbCommand cmd = _DatabaseFactory.GetDbCommand(Helper.GetDataBaseType, sqlCmd, DataBaseConnectionString);
                            _DatabaseFactory.AddInParameter(ref cmd, "@ProcessID", DbType.String, argEn.ProcessID);
                            _DatabaseFactory.AddInParameter(ref cmd, "@PaymentNo", DbType.String, argEn.PaymentNo);
                            _DatabaseFactory.AddInParameter(ref cmd, "@PaymentType", DbType.String, argEn.PaymentType);
                            _DatabaseFactory.AddInParameter(ref cmd, "@Description", DbType.String, argEn.Description);
                            _DatabaseFactory.AddInParameter(ref cmd, "@ChequeDate", DbType.DateTime, argEn.ChequeDate);
                            _DatabaseFactory.AddInParameter(ref cmd, "@TransactionDate", DbType.DateTime, argEn.TransactionDate);
                            _DatabaseFactory.AddInParameter(ref cmd, "@PrintStatus", DbType.String, argEn.PrintStatus);
                            _DatabaseFactory.AddInParameter(ref cmd, "@UpdatedBy", DbType.String, argEn.UpdatedBy);
                            _DatabaseFactory.AddInParameter(ref cmd, "@UpdatedTime", DbType.DateTime, argEn.UpdatedTime);
                            _DbParameterCollection = cmd.Parameters;

                            int liRowAffected = _DatabaseFactory.ExecuteNonQuery(Helper.GetDataBaseType, cmd,
                                DataBaseConnectionString, sqlCmd, _DbParameterCollection);

                            if (liRowAffected > -1)
                                lbRes = true;
                            else
                                throw new Exception("Update Failed! No Row has been updated...");
                            //Inserting AccountDetails Table
                            if (argEn.ChequeDetailslist != null)
                            {
                                if (argEn.ChequeDetailslist.Count != 0)
                                {
                                    ChequeDetailsDAL loDS = new ChequeDetailsDAL();
                                    for (int i = 0; i < argEn.ChequeDetailslist.Count; i++)
                                    {
                                        argEn.ChequeDetailslist[i].ProcessId = batchcode;
                                        loDS.Insert(argEn.ChequeDetailslist[i]);
                                    }
                                }
                            }
                            if (argEn.PaymentType == "Payment")
                            {
                                //updating allocations
                                UpdatePayments(argEn.AcccountChques);
                            }
                            if (argEn.PaymentType == "Refund")
                            {
                                // updating Refunds
                                UpdatePayments(argEn.AcccountChques);
                            }
                            if (argEn.PaymentType == "Sponsor Payments")
                            {
                                // updating Sponsor Payments
                                UpdatePayments(argEn.AcccountChques);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return batchcode;
        }

        #endregion

        #region UpdatePayments 

        /// <summary>
        /// Method to Update Payments
         /// </summary>
        /// <param name="arglist">List of Accounts Entity is the Input.ChequeNo,ChequeDate,Updateby and UpdatedTime are Input Properties</param>
        /// <returns>Returns Boolean.</returns>
        public bool UpdatePayments(List<AccountsEn> arglist)
        {
            AccountsDAL lods = new AccountsDAL();
            bool lbRes = false;
            string sqlCmd;

            try
            {
                int j = 0;
                for (j = 0; j < arglist.Count; j++)
                {
                    sqlCmd = "UPDATE SAS_Accounts SET ChequeNo = @ChequeNo,ChequeDate = @ChequeDate, UpdatedBy = @UpdatedBy, UpdatedTime = @UpdatedTime WHERE TransCode = '" + arglist[j].TransactionCode + "'";

                    if (!FormHelp.IsBlank(sqlCmd))
                    {
                        DbCommand cmd = _DatabaseFactory.GetDbCommand(Helper.GetDataBaseType, sqlCmd, DataBaseConnectionString);
                        _DatabaseFactory.AddInParameter(ref cmd, "@ChequeNo", DbType.String, arglist[j].ChequeNo);
                        _DatabaseFactory.AddInParameter(ref cmd, "@ChequeDate", DbType.DateTime, arglist[j].ChequeDate);
                        _DatabaseFactory.AddInParameter(ref cmd, "@UpdatedBy", DbType.String, arglist[j].UpdatedBy);
                        _DatabaseFactory.AddInParameter(ref cmd, "@UpdatedTime", DbType.DateTime, arglist[j].UpdatedTime);
                        _DbParameterCollection = cmd.Parameters;


                        int liRowAffected = _DatabaseFactory.ExecuteNonQuery(Helper.GetDataBaseType, cmd,
                                DataBaseConnectionString, sqlCmd, _DbParameterCollection);

                        if (liRowAffected > -1)
                            lbRes = true;
                        else
                            throw new Exception("Update Failed! No Row has been updated...");
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
        /// Method to Update Cheques
/// </summary>
        /// <param name="argEn">Cheques Entity is the Input.</param>
/// <returns>Returns Boolean.</returns>
        public bool Update(ChequeEn argEn)
        {
            bool lbRes = false;
            try
            {
                string sqlCmd = "UPDATE SAS_Cheque SET ProcessID = @ProcessID, PaymentNo = @PaymentNo, PaymentType = @PaymentType, Description = @Description, ChequeDate = @ChequeDate, TransactionDate = @TransactionDate, PrintStatus = @PrintStatus, UpdatedBy = @UpdatedBy, UpdatedTime = @UpdatedTime WHERE ProcessID = @ProcessID";

                if (!FormHelp.IsBlank(sqlCmd))
                {
                    DbCommand cmd = _DatabaseFactory.GetDbCommand(Helper.GetDataBaseType, sqlCmd, DataBaseConnectionString);
                    _DatabaseFactory.AddInParameter(ref cmd, "@ProcessID", DbType.String, argEn.ProcessID);
                    _DatabaseFactory.AddInParameter(ref cmd, "@PaymentNo", DbType.String, argEn.PaymentNo);
                    _DatabaseFactory.AddInParameter(ref cmd, "@PaymentType", DbType.String, argEn.PaymentType);
                    _DatabaseFactory.AddInParameter(ref cmd, "@Description", DbType.String, argEn.Description);
                    _DatabaseFactory.AddInParameter(ref cmd, "@ChequeDate", DbType.DateTime, argEn.ChequeDate);
                    _DatabaseFactory.AddInParameter(ref cmd, "@TransactionDate", DbType.DateTime, argEn.TransactionDate);
                    _DatabaseFactory.AddInParameter(ref cmd, "@PrintStatus", DbType.String, argEn.PrintStatus);
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

        #region BatchDelete 

        /// <summary>
        /// Method to Delete Cheques Batch
        /// </summary>
        /// <param name="argEn">Cheques Entity is the Input.ProcessID is the Input Property.</param>
        /// <returns>Returns a Boolean</returns>
        public bool BatchDelete(ChequeEn argEn)
        {
            bool lbRes = false;
            List<ChequeDetailsEn> loChDetails = new List<ChequeDetailsEn>();
            string sqlCmd = "Select * from SAS_Cheque WHERE ProcessID = @ProcessID";
            
            try
            {
                if (!FormHelp.IsBlank(sqlCmd))
                {
                    DbCommand cmdSel = _DatabaseFactory.GetDbCommand(Helper.GetDataBaseType, sqlCmd, DataBaseConnectionString);
                    _DatabaseFactory.AddInParameter(ref cmdSel, "@ProcessID", DbType.String, argEn.ProcessID);
                    _DbParameterCollection = cmdSel.Parameters;

                    if (!FormHelp.IsBlank(sqlCmd))
                    {
                        ChequeDetailsDAL loObjChequeDAL = new ChequeDetailsDAL();
                        ChequeDetailsEn loItem = null;
                        using (IDataReader loReader = _DatabaseFactory.GetIDataReader(Helper.GetDataBaseType, cmdSel,
                            DataBaseConnectionString, sqlCmd, _DbParameterCollection).CreateDataReader()) 
                        {
                            loItem = new ChequeDetailsEn();
                            loItem.ProcessId = GetValue<string>(loReader, "ProcessID");
                            loChDetails.Add(loItem);
                            loItem = null;
                            loReader.Close();
                        }
                        
                        int i = 0;
                        //deleting each item in batch
                        for (i = 0; i < loChDetails.Count; i++)
                        {
                            loObjChequeDAL.Delete(loChDetails[i]);
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
        /// Method to Delete Cheques
        /// </summary>
        /// <param name="argEn">Cheques Entity is the Input.ProcessID is the Input Property.</param>
        /// <returns>Returns Boolean.</returns>
        public bool Delete(ChequeEn argEn)
        {
            bool lbRes = false;
            string sqlCmd = "DELETE FROM SAS_Cheque WHERE ProcessID = @ProcessID";
            
            try
            {
                if (!FormHelp.IsBlank(sqlCmd))
                {
                    DbCommand cmdSel = _DatabaseFactory.GetDbCommand(Helper.GetDataBaseType, sqlCmd, DataBaseConnectionString);
                    _DatabaseFactory.AddInParameter(ref cmdSel, "@ProcessID", DbType.String, argEn.ProcessID);
                    _DbParameterCollection = cmdSel.Parameters;

                    int liRowAffected = _DatabaseFactory.ExecuteNonQuery(Helper.GetDataBaseType, cmdSel,
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

        #region LoadObject 

        /// <summary>
        /// Method to Load Cheques Entity
        /// </summary>
        /// <param name="argReader">Reader Object is an Input.</param>
        /// <returns>Returns Cheques Entity Object.</returns>
        private ChequeEn LoadObject(IDataReader argReader)
        {
            ChequeEn loItem = new ChequeEn();
            loItem.ProcessID = GetValue<string>(argReader, "ProcessID");
            loItem.PaymentNo = GetValue<string>(argReader, "PaymentNo");
            loItem.PaymentType = GetValue<string>(argReader, "PaymentType");
            loItem.Description = GetValue<string>(argReader, "Description");
            loItem.ChequeDate = GetValue<DateTime>(argReader, "ChequeDate");
            loItem.TransactionDate = GetValue<DateTime>(argReader, "TransactionDate");
            loItem.PrintStatus = GetValue<string>(argReader, "PrintStatus");
            loItem.UpdatedBy = GetValue<string>(argReader, "UpdatedBy");
            loItem.UpdatedTime = GetValue<DateTime>(argReader, "UpdatedTime");
            loItem.BankCode = GetValue<string>(argReader, "BankCode");
            loItem.TransactionCode = GetValue<string>(argReader, "TransCode");
            loItem.BatchCode = GetValue<string>(argReader, "BatchCode");
            loItem.TranssactionID = GetValue<int>(argReader, "TransID");
                     return loItem;
        }

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
        /// Method to get AutoNumber
        /// </summary>
        /// <param name="Description">Description as Input.</param>
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
    }

}


