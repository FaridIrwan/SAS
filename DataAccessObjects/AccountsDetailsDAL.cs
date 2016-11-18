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
    /// Class to handle all the AccountingDetails Transactions.
    /// </summary>
    public class AccountsDetailsDAL
    {
        #region Global Declarations

        private DbParameterCollection _DbParameterCollection = null;

        private MaxModule.DatabaseProvider _DatabaseFactory =
            new MaxModule.DatabaseProvider();

        private string DataBaseConnectionString = Helper.
           GetConnectionString();

        #endregion

        public AccountsDetailsDAL()
        {
        }

        #region GetStuDentAllocation

        /// <summary>
        /// Method to Get Student Allocations
        /// </summary>
        /// <param name="argEn">AccountDetails Entity is an Input.TransactionID is an Input Property</param>
        /// <returns>Returns List of AccountsDetails Entities</returns>
        public List<AccountsDetailsEn> GetStuDentAllocation(AccountsDetailsEn argEn)
        {
            List<AccountsDetailsEn> loEnList = new List<AccountsDetailsEn>();
            string sqlCmd = " SELECT SAS_Student.SASI_MatricNo, SAS_Student.SASI_Name, SAS_Student.SASI_PgId, SAS_Student.SASI_ICNo," +
                            " SAS_Student.SASI_CurSem, SAS_AccountsDetails.TransID, SAS_AccountsDetails.TransTempCode, SAS_AccountsDetails.TransCode," +
                            " SAS_AccountsDetails.RefCode, SAS_AccountsDetails.TransAmount, SAS_AccountsDetails.PaidAmount," +
                            " SAS_AccountsDetails.TempAmount,SAS_AccountsDetails.DiscountAmount, SAS_AccountsDetails.TempPaidAmount, SAS_AccountsDetails.TransStatus, " +
                            " SAS_AccountsDetails.PostStatus, SAS_AccountsDetails.Ref1, SAS_AccountsDetails.Ref3,SAS_AccountsDetails.NoKelompok ," +
                            " SAS_AccountsDetails.NoWarran,SAS_AccountsDetails.AmaunWarran,SAS_AccountsDetails.noAkaunPelajar,SAS_AccountsDetails.StatusBayaran " +
                            " FROM  SAS_AccountsDetails INNER JOIN SAS_Student ON" +
                            " SAS_AccountsDetails.RefCode = SAS_Student.SASI_MatricNo where SAS_AccountsDetails.TransID='" + argEn.TransactionID + "'";

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
                            loItem.Sudentacc = new StudentEn();
                            loItem.TransactionID = GetValue<int>(loReader, "TransID");
                            loItem.TransTempCode = GetValue<string>(loReader, "TransTempCode");
                            loItem.TransactionCode = GetValue<string>(loReader, "TransCode");
                            loItem.ReferenceCode = GetValue<string>(loReader, "RefCode");
                            loItem.TransactionAmount = GetValue<double>(loReader, "TransAmount");
                            loItem.PaidAmount = GetValue<double>(loReader, "PaidAmount");
                            loItem.TempAmount = GetValue<double>(loReader, "TempAmount");
                            loItem.TempPaidAmount = GetValue<double>(loReader, "TempPaidAmount");
                            loItem.Sudentacc.MatricNo = GetValue<string>(loReader, "SASI_MatricNo");
                            loItem.Sudentacc.StudentName = GetValue<string>(loReader, "SASI_Name");
                            loItem.Sudentacc.ProgramID = GetValue<string>(loReader, "SASI_PgId");
                            loItem.Sudentacc.ICNo = GetValue<string>(loReader, "SASI_ICNo");
                            loItem.ReferenceOne = GetValue<string>(loReader, "Ref1");
                            loItem.ReferenceThree = GetValue<string>(loReader, "Ref3");
                            loItem.Sudentacc.CurrentSemester = GetValue<int>(loReader, "SASI_CurSem");
                            loItem.NoKelompok = GetValue<string>(loReader, "NoKelompok");
                            loItem.NoWarran = GetValue<string>(loReader, "NoWarran");
                            loItem.AmaunWarran = GetValue<double>(loReader, "AmaunWarran");
                            loItem.noAkaun = GetValue<string>(loReader, "noAkaunPelajar");
                            loItem.DiscountAmount = GetValue<double>(loReader, "DiscountAmount");
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

        #region GetListStudentSponsorAlloc

        /// <summary>
        /// Method to Get List Of students for sponsor allocation
        /// </summary>
        /// <param name="argEn">AccountDetails Entity is an Input</param>
        /// <returns>Returns List of AccountsDetails Entities</returns>

        public List<AccountsDetailsEn> GetListStudentSponsorAlloc(string batchNo)
        {
            List<AccountsDetailsEn> loEnList = new List<AccountsDetailsEn>();
            string sqlCmd = sqlCmd = "SELECT SAS_Student.SASI_MatricNo, SAS_Student.SASI_Name, SAS_Student.SASI_PgId, SAS_Student.SASI_ICNo, " +
            "SAS_Student.SASI_CurSem, SAS_AccountsDetails.TransID, SAS_AccountsDetails.TransTempCode, " +
            "SAS_AccountsDetails.TransCode, SAS_AccountsDetails.RefCode, SAS_AccountsDetails.TransAmount, " +
            "SAS_AccountsDetails.PaidAmount, SAS_AccountsDetails.TempAmount, SAS_AccountsDetails.TempPaidAmount, " +
            "SAS_AccountsDetails.TransStatus,  SAS_AccountsDetails.PostStatus, SAS_AccountsDetails.Ref1, " +
            "SAS_AccountsDetails.Ref3,SAS_AccountsDetails.noKelompok,SAS_AccountsDetails.noWarran, " +
            "SAS_AccountsDetails.amaunWarran,SAS_AccountsDetails.noAkaunPelajar,SAS_AccountsDetails.statusBayaran " +
            "FROM SAS_AccountsDetails INNER JOIN SAS_Student ON SAS_AccountsDetails.RefCode = " +
            "SAS_Student.SASI_MatricNo inner join SAS_Accounts on SAS_Accounts.TransID = SAS_AccountsDetails.TransID " +
            "where SAS_Accounts.BatchCode = '" + batchNo + "'";


            try
            {
                if (!FormHelp.IsBlank(sqlCmd))
                {
                    using (IDataReader loReader = _DatabaseFactory.ExecuteReader(Helper.GetDataBaseType,
                       DataBaseConnectionString, sqlCmd).CreateDataReader())
                    {
                        while (loReader.Read())
                        {
                            AccountsDetailsEn loItem = LoadObjectSponAlloc(loReader);
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

        #region GetItemDetails

        /// <summary>
        /// Method to Get an Accounts Item
        /// </summary>
        /// <param name="argEn">Accounts Entity is the Input.BatchCode is the Input Property</param>
        /// <returns>Returns an Accounts Item</returns>
        public List<AccountsDetailsEn> GetItemDetails(AccountsEn argEn)
        {
            List<AccountsDetailsEn> loList = new List<AccountsDetailsEn>();
            string sqlCmd = "Select * FROM dbo.SAS_SucceedTransactionDetails WHERE STH_AutoNum_H = @STH_AutoNum_H";

            try
            {
                if (!FormHelp.IsBlank(sqlCmd))
                {
                    DbCommand cmd = _DatabaseFactory.GetDbCommand(Helper.GetDataBaseType, sqlCmd, DataBaseConnectionString);
                    _DatabaseFactory.AddInParameter(ref cmd, "@STH_AutoNum_H", DbType.String, argEn.AutoNum);
                    _DbParameterCollection = cmd.Parameters;

                    using (IDataReader loReader = _DatabaseFactory.GetIDataReader(Helper.GetDataBaseType, cmd,
                       DataBaseConnectionString, sqlCmd, _DbParameterCollection).CreateDataReader())
                    {
                        while (loReader.Read())
                        {
                            AccountsDetailsEn loItem = new AccountsDetailsEn();
                            loItem = LoadObjectSucceedDetails(loReader);
                            loList.Add(loItem);
                        }
                        loReader.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return loList;
        }

        #endregion

        #region GetList

        /// <summary>
        /// Method to Get List Of AccountDetails Entity
        /// </summary>
        /// <param name="argEn">AccountDetails Entity is an Input</param>
        /// <returns>Returns List of AccountsDetails Entities</returns>

        public List<AccountsDetailsEn> GetList(AccountsDetailsEn argEn)
        {
            List<AccountsDetailsEn> loEnList = new List<AccountsDetailsEn>();
            string sqlCmd = "select * from SAS_AccountsDetails";

            try
            {
                if (!FormHelp.IsBlank(sqlCmd))
                {
                    using (IDataReader loReader = _DatabaseFactory.ExecuteReader(Helper.GetDataBaseType,
                        DataBaseConnectionString, sqlCmd).CreateDataReader())
                    {
                        while (loReader.Read())
                        {
                            AccountsDetailsEn loItem = LoadObject(loReader);
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

        #region GetAccountDetailList

        /// <summary>
        /// Method to Get List Of AccountDetails Entity
        /// </summary>
        /// <param name="argEn">AccountDetails Entity is an Input Parameter.TransID is the Inout.</param>
        /// <returns>Returns List of AccountsDetails Entities</returns>

        public List<AccountsDetailsEn> GetAccountDetailList(AccountsDetailsEn argEn)
        {
            List<AccountsDetailsEn> loEnList = new List<AccountsDetailsEn>();
            string sqlCmd = "select SAS_AccountsDetails.*,SAS_FeeTypes.SAFT_Desc,SAS_FeeTypes.SAFT_Desc,SAFT_GLCode from SAS_AccountsDetails INNER JOIN SAS_FeeTypes ON SAS_AccountsDetails.RefCode = SAS_FeeTypes.SAFT_Code WHERE SAS_AccountsDetails.TransID = @TransID";

            try
            {
                if (!FormHelp.IsBlank(sqlCmd))
                {
                    DbCommand cmd = _DatabaseFactory.GetDbCommand(Helper.GetDataBaseType, sqlCmd, DataBaseConnectionString);
                    _DatabaseFactory.AddInParameter(ref cmd, "@TransID", DbType.Int32, argEn.TransactionID);
                    _DbParameterCollection = cmd.Parameters;

                    using (IDataReader loReader = _DatabaseFactory.GetIDataReader(Helper.GetDataBaseType, cmd,
                       DataBaseConnectionString, sqlCmd, _DbParameterCollection).CreateDataReader())
                    {
                        while (loReader.Read())
                        {
                            AccountsDetailsEn loItem = LoadObject(loReader);
                            loItem.Feetype = new FeeTypesEn();
                            loItem.Feetype.Description = GetValue<string>(loReader, "SAFT_Desc");
                            loItem.Feetype.GLCode = GetValue<string>(loReader, "SAFT_GLCode");
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

        #region GetGLJournalAccountDetailList

        /// <summary>
        /// Method to Get List Of GlJoural AccountDetails Entity
        /// </summary>
        /// <param name="argEn">AccountDetails Entity is an Input Parameter.TransID is the Inout.</param>
        /// <returns>Returns List of AccountsDetails Entities</returns>

        public List<AccountsDetailsEn> GetGLJournalAccountDetailList(AccountsDetailsEn argEn)
        {
            List<AccountsDetailsEn> loEnList = new List<AccountsDetailsEn>();
            string sqlCmd = "SELECT SAS_Student.SASI_Name, SAS_Student.SASI_Faculty, SAS_Faculty.SAFC_GlAccount, SAS_Faculty.SAFC_Desc FROM SAS_AccountsDetails INNER JOIN " +
                             " SAS_Student ON SAS_AccountsDetails.RefCode = SAS_Student.SASI_MatricNo INNER JOIN SAS_Faculty ON SAS_Student.SASI_Faculty = SAS_Faculty.SAFC_Code WHERE SAS_AccountsDetails.TransID = @TransID";

            try
            {
                if (!FormHelp.IsBlank(sqlCmd))
                {
                    DbCommand cmd = _DatabaseFactory.GetDbCommand(Helper.GetDataBaseType, sqlCmd, DataBaseConnectionString);
                    _DatabaseFactory.AddInParameter(ref cmd, "@TransID", DbType.Int32, argEn.TransactionID);
                    _DbParameterCollection = cmd.Parameters;

                    using (IDataReader loReader = _DatabaseFactory.GetIDataReader(Helper.GetDataBaseType, cmd,
                        DataBaseConnectionString, sqlCmd, _DbParameterCollection).CreateDataReader())
                    {
                        while (loReader.Read())
                        {
                            AccountsDetailsEn loItem = LoadObject(loReader);
                            loItem.Sudentacc = new StudentEn();
                            loItem.Sudentacc.FacultyEntity = new FacultyEn();
                            loItem.Sudentacc.FacultyEntity.SAFC_GlAccount = GetValue<string>(loReader, "SAFC_GlAccount");
                            loItem.Sudentacc.FacultyEntity.SAFC_Desc = GetValue<string>(loReader, "SAFC_Desc");
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

        #region GetExportDataDetailList

        /// <summary>
        /// Method to Get List Of AccountDetails Entity
        /// </summary>
        /// <param name="argEn">AccountDetails Entity is an Input Parameter.TransID is the Inout.</param>
        /// <returns>Returns List of AccountsDetails Entities</returns>

        public List<AccountsDetailsEn> GetExportDataDetailList(AccountsDetailsEn argEn)
        {
            List<AccountsDetailsEn> loEnList = new List<AccountsDetailsEn>();
            string sqlCmd = "";
            if (argEn.ReferenceCode == "Payments")
            {
                sqlCmd = "SELECT SAS_AccountsDetails.*,SAS_Student.* FROM SAS_AccountsDetails INNER JOIN SAS_Student ON SAS_AccountsDetails.RefCode = SAS_Student.SASI_MatricNo";
            }
            else if (argEn.ReferenceCode == "Invoice")
            {
                sqlCmd = "SELECT SAS_AccountsDetails.*, SAS_AccountsDetails.RefCode ,SAS_FeeTypes.* FROM SAS_AccountsDetails INNER JOIN SAS_FeeTypes ON SAS_AccountsDetails.RefCode = dbo.SAS_FeeTypes.SAFT_Code";
            }

            try
            {
                if (!FormHelp.IsBlank(sqlCmd))
                {
                    DbCommand cmd = _DatabaseFactory.GetDbCommand(Helper.GetDataBaseType, sqlCmd, DataBaseConnectionString);
                    _DatabaseFactory.AddInParameter(ref cmd, "@TransID", DbType.Int32, argEn.TransactionID);
                    _DbParameterCollection = cmd.Parameters;

                    using (IDataReader loReader = _DatabaseFactory.GetIDataReader(Helper.GetDataBaseType, cmd,
                        DataBaseConnectionString, sqlCmd, _DbParameterCollection).CreateDataReader())
                    {
                        while (loReader.Read())
                        {
                            AccountsDetailsEn loItem = LoadObject(loReader);
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

        #region GetStudentAccountsDetails

        /// <summary>
        /// Method to Get Student AccountDetails
        /// </summary>
        /// <param name="argEn">AccountsDetails Entity is an Input.TransactionId is an Input Property</param>
        /// <returns>Returns List Of AccountDetails Entities</returns>
        public List<AccountsDetailsEn> GetStudentAccountsDetails(AccountsDetailsEn argEn)
        {
            List<AccountsDetailsEn> loEnList = new List<AccountsDetailsEn>();
            string sqlCmd = "SELECT SAS_FeeTypes.SAFT_Code, SAS_FeeTypes.SAFT_Desc, SAS_FeeTypes.SAFT_FeeType, SAS_FeeTypes.SAFT_Hostel," +
                            " SAS_FeeTypes.SAFT_Priority, SAS_FeeTypes.SAFT_Remarks, SAS_FeeTypes.SAFT_GLCode, SAS_FeeTypes.SAFT_Status,SAS_AccountsDetails.TransAmount," +
                            " SAS_AccountsDetails.RefCode, SAS_AccountsDetails.TransID, SAS_AccountsDetails.TransTempCode,SAS_AccountsDetails.TransCode," +
                            " SAS_FeeCharges.safs_gstamout, SAS_FeeTypes.saft_taxmode" +
                            " FROM SAS_AccountsDetails INNER JOIN SAS_FeeTypes ON SAS_AccountsDetails.RefCode = SAS_FeeTypes.SAFT_Code " +
                            "INNER JOIN SAS_FeeCharges ON SAS_FeeTypes.SAFT_Code = SAS_FeeCharges.SAFT_Code WHERE " +
                            "SAS_AccountsDetails.TransAmount = SAS_FeeCharges.SAFS_Amount AND TransID='" + argEn.TransactionID + "'";

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
                            loItem.FeeTypeCode = GetValue<string>(loReader, "SAFT_Code");
                            loItem.Description = GetValue<string>(loReader, "SAFT_Desc");
                            loItem.FeeType = GetValue<string>(loReader, "SAFT_FeeType");
                            loItem.Hostel = GetValue<string>(loReader, "SAFT_Hostel");
                            loItem.Priority = GetValue<int>(loReader, "SAFT_Priority");
                            loItem.Remarks = GetValue<string>(loReader, "SAFT_Remarks");
                            loItem.GLCode = GetValue<string>(loReader, "SAFT_GLCode");
                            loItem.Status = GetValue<bool>(loReader, "SAFT_Status");
                            loItem.TransactionID = GetValue<int>(loReader, "TransID");
                            loItem.TransTempCode = GetValue<string>(loReader, "TransTempCode");
                            loItem.TransactionAmount = GetValue<double>(loReader, "TransAmount");
                            loItem.TransactionCode = GetValue<string>(loReader, "TransCode");
                            loItem.ReferenceCode = GetValue<string>(loReader, "RefCode");
                            loItem.GSTAmount = GetValue<double>(loReader, "safs_gstamout");
                            //loItem.TaxId = GetValue<int>(loReader, "saft_taxmode");
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

        public List<AccountsDetailsEn> GetStudentAccountsDetailsWithTaxAmount(AccountsDetailsEn argEn)
        {
            List<AccountsDetailsEn> loEnList = new List<AccountsDetailsEn>();
            string sqlCmd = "SELECT SAS_FeeTypes.SAFT_Code, SAS_FeeTypes.SAFT_Desc, SAS_FeeTypes.SAFT_FeeType, SAS_FeeTypes.SAFT_Hostel," +
                            " SAS_FeeTypes.SAFT_Priority, SAS_FeeTypes.SAFT_Remarks, SAS_FeeTypes.SAFT_GLCode, SAS_FeeTypes.SAFT_Status,SAS_AccountsDetails.TransAmount," +
                            " SAS_AccountsDetails.RefCode, SAS_AccountsDetails.TransID, SAS_AccountsDetails.TransTempCode,SAS_AccountsDetails.TransCode," +
                            " SAS_FeeTypes.saft_taxmode, SAS_AccountsDetails.TaxAmount, SAS_AccountsDetails.Tax" +
                            " FROM SAS_AccountsDetails INNER JOIN SAS_FeeTypes ON SAS_AccountsDetails.RefCode = SAS_FeeTypes.SAFT_Code " +
                            " WHERE TransID='" + argEn.TransactionID + "'";

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
                            loItem.FeeTypeCode = GetValue<string>(loReader, "SAFT_Code");
                            loItem.Description = GetValue<string>(loReader, "SAFT_Desc");
                            loItem.FeeType = GetValue<string>(loReader, "SAFT_FeeType");
                            loItem.Hostel = GetValue<string>(loReader, "SAFT_Hostel");
                            loItem.Priority = GetValue<int>(loReader, "SAFT_Priority");
                            loItem.Remarks = GetValue<string>(loReader, "SAFT_Remarks");
                            loItem.GLCode = GetValue<string>(loReader, "SAFT_GLCode");
                            loItem.Status = GetValue<bool>(loReader, "SAFT_Status");
                            loItem.TransactionID = GetValue<int>(loReader, "TransID");
                            loItem.TransTempCode = GetValue<string>(loReader, "TransTempCode");
                            loItem.TransactionAmount = GetValue<double>(loReader, "TransAmount");
                            loItem.TransactionCode = GetValue<string>(loReader, "TransCode");
                            loItem.ReferenceCode = GetValue<string>(loReader, "RefCode");
                            loItem.GSTAmount = GetValue<double>(loReader, "TaxAmount");
                            loItem.TaxAmount = GetValue<double>(loReader, "TaxAmount");
                            loItem.TaxId = GetValue<int>(loReader, "saft_taxmode");

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

        //public List<AccountsDetailsEn> GetStudentAccountsDetailsByBatchCode(AccountsEn argEn)
        //{
        //    List<AccountsDetailsEn> loEnList = new List<AccountsDetailsEn>();
        //    string sqlCmd = "SELECT sas_student.sasi_matricno, sas_student.sasi_name, sas_student.sasi_cursemyr, SAS_FeeTypes.SAFT_Code, SAS_FeeTypes.SAFT_Desc, SAS_FeeTypes.SAFT_FeeType, SAS_FeeTypes.SAFT_Hostel," +
        //                    " SAS_FeeTypes.SAFT_Priority, SAS_FeeTypes.SAFT_Remarks, SAS_FeeTypes.SAFT_GLCode, SAS_FeeTypes.SAFT_Status,SAS_AccountsDetails.TransAmount," +
        //                    " SAS_AccountsDetails.RefCode, SAS_AccountsDetails.TransID, SAS_AccountsDetails.TransTempCode,SAS_AccountsDetails.TransCode," +
        //                    " SAS_FeeTypes.saft_taxmode, SAS_AccountsDetails.TaxAmount, SAS_AccountsDetails.Tax, SAS_AccountsDetails.internal_use " +
        //                    " FROM sas_accounts inner join SAS_AccountsDetails  on "+
        //                    " sas_accounts.transcode = SAS_AccountsDetails.transcode and sas_accounts.transtempcode = SAS_AccountsDetails.transtempcode " +
        //                    " INNER JOIN SAS_FeeTypes ON SAS_AccountsDetails.RefCode = SAS_FeeTypes.SAFT_Code " +
        //                     " inner join sas_student on sas_student.sasi_matricno = sas_accounts.creditref" +
        //                     " WHERE batchcode = '" + argEn.BatchCode + "'";
        //    sqlCmd += " order by sas_student.sasi_matricno";

        //    try
        //    {
        //        if (!FormHelp.IsBlank(sqlCmd))
        //        {
        //            using (IDataReader loReader = _DatabaseFactory.ExecuteReader(Helper.GetDataBaseType,
        //                DataBaseConnectionString, sqlCmd).CreateDataReader())
        //            {
        //                while (loReader.Read())
        //                {
        //                    StudentEn getStudent = new StudentEn();
        //                    getStudent.MatricNo = GetValue<string>(loReader, "sasi_matricno");
        //                    getStudent.StudentName = GetValue<string>(loReader, "sasi_name");

        //                    AccountsDetailsEn loItem = new AccountsDetailsEn();
        //                    loItem.FeeTypeCode = GetValue<string>(loReader, "SAFT_Code");
        //                    loItem.Description = GetValue<string>(loReader, "SAFT_Desc");
        //                    loItem.FeeType = GetValue<string>(loReader, "SAFT_FeeType");
        //                    loItem.Hostel = GetValue<string>(loReader, "SAFT_Hostel");
        //                    loItem.Priority = GetValue<int>(loReader, "SAFT_Priority");
        //                    loItem.Remarks = GetValue<string>(loReader, "SAFT_Remarks");
        //                    loItem.GLCode = GetValue<string>(loReader, "SAFT_GLCode");
        //                    loItem.Status = GetValue<bool>(loReader, "SAFT_Status");
        //                    loItem.TransactionID = GetValue<int>(loReader, "TransID");
        //                    loItem.TransTempCode = GetValue<string>(loReader, "TransTempCode");
        //                    loItem.TransactionAmount = GetValue<double>(loReader, "TransAmount");
        //                    loItem.TransactionCode = GetValue<string>(loReader, "TransCode");
        //                    loItem.ReferenceCode = GetValue<string>(loReader, "RefCode");
        //                    loItem.GSTAmount = GetValue<double>(loReader, "TaxAmount");
        //                    loItem.TaxAmount = GetValue<double>(loReader, "TaxAmount");
        //                    loItem.TaxId = GetValue<int>(loReader, "saft_taxmode");
        //                    loItem.Sudentacc = getStudent;
        //                    loItem.Internal_Use = GetValue<string>(loReader, "internal_use");
        //                    loEnList.Add(loItem);
        //                }
        //                loReader.Close();
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //    return loEnList;
        //}
        public List<AccountsDetailsEn> GetStudentAccountsDetailsByBatchCode(AccountsEn argEn, String m_no)
        {
            List<AccountsDetailsEn> loEnList = new List<AccountsDetailsEn>();
            string sqlCmd = "SELECT sas_student.sasi_matricno, sas_student.sasi_name, sas_student.sasi_cursemyr, SAS_FeeTypes.SAFT_Code, SAS_FeeTypes.SAFT_Desc, SAS_FeeTypes.SAFT_FeeType, SAS_FeeTypes.SAFT_Hostel," +
                            " SAS_FeeTypes.SAFT_Priority, SAS_FeeTypes.SAFT_Remarks, SAS_FeeTypes.SAFT_GLCode, SAS_FeeTypes.SAFT_Status,SAS_AccountsDetails.TransAmount," +
                            " SAS_AccountsDetails.RefCode, SAS_AccountsDetails.TransID, SAS_AccountsDetails.TransTempCode,SAS_AccountsDetails.TransCode," +
                            " SAS_FeeTypes.saft_taxmode, SAS_AccountsDetails.TaxAmount, SAS_AccountsDetails.Tax, SAS_AccountsDetails.internal_use " +
                            " FROM sas_accounts inner join SAS_AccountsDetails  on " +
                            " sas_accounts.transcode = SAS_AccountsDetails.transcode and sas_accounts.transtempcode = SAS_AccountsDetails.transtempcode " +
                            " INNER JOIN SAS_FeeTypes ON SAS_AccountsDetails.RefCode = SAS_FeeTypes.SAFT_Code " +
                            " inner join sas_student on sas_student.sasi_matricno = sas_accounts.creditref" +
                            " WHERE batchcode = '" + argEn.BatchCode + "'";

            //added by Hafiz @ 22/3/2016
            if (m_no.Length != 0) sqlCmd = sqlCmd + " AND SAS_Student.SASI_MatricNo ='" + m_no + "'";

            sqlCmd += " order by sas_student.sasi_matricno";

            try
            {
                if (!FormHelp.IsBlank(sqlCmd))
                {
                    using (IDataReader loReader = _DatabaseFactory.ExecuteReader(Helper.GetDataBaseType,
                        DataBaseConnectionString, sqlCmd).CreateDataReader())
                    {
                        while (loReader.Read())
                        {
                            StudentEn getStudent = new StudentEn();
                            getStudent.MatricNo = GetValue<string>(loReader, "sasi_matricno");
                            getStudent.StudentName = GetValue<string>(loReader, "sasi_name");

                            AccountsDetailsEn loItem = new AccountsDetailsEn();
                            loItem.FeeTypeCode = GetValue<string>(loReader, "SAFT_Code");
                            loItem.Description = GetValue<string>(loReader, "SAFT_Desc");
                            loItem.FeeType = GetValue<string>(loReader, "SAFT_FeeType");
                            loItem.Hostel = GetValue<string>(loReader, "SAFT_Hostel");
                            loItem.Priority = GetValue<int>(loReader, "SAFT_Priority");
                            loItem.Remarks = GetValue<string>(loReader, "SAFT_Remarks");
                            loItem.GLCode = GetValue<string>(loReader, "SAFT_GLCode");
                            loItem.Status = GetValue<bool>(loReader, "SAFT_Status");
                            loItem.TransactionID = GetValue<int>(loReader, "TransID");
                            loItem.TransTempCode = GetValue<string>(loReader, "TransTempCode");
                            loItem.TransactionAmount = GetValue<double>(loReader, "TransAmount");
                            loItem.TransactionCode = GetValue<string>(loReader, "TransCode");
                            loItem.ReferenceCode = GetValue<string>(loReader, "RefCode");
                            loItem.GSTAmount = GetValue<double>(loReader, "TaxAmount");
                            loItem.TaxAmount = GetValue<double>(loReader, "TaxAmount");
                            loItem.TaxId = GetValue<int>(loReader, "saft_taxmode");
                            loItem.Sudentacc = getStudent;
                            loItem.Internal_Use = GetValue<string>(loReader, "internal_use");
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
        /// Method to Get an AccountsDetails Entity
        /// </summary>
        /// <param name="argEn">AccountDetails Entity is an Input</param>
        /// <returns>Returns AccountDetails Entity</returns>
        public AccountsDetailsEn GetItem(AccountsDetailsEn argEn)
        {
            AccountsDetailsEn loItem = new AccountsDetailsEn();
            string sqlCmd = "Select * FROM SAS_AccountsDetails WHERE TransID = @TransID";

            try
            {
                if (!FormHelp.IsBlank(sqlCmd))
                {
                    DbCommand cmd = _DatabaseFactory.GetDbCommand(Helper.GetDataBaseType, sqlCmd, DataBaseConnectionString);
                    _DatabaseFactory.AddInParameter(ref cmd, "@TransID", DbType.Int32, argEn.TransactionID);
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
        /// Method to Insert 
        /// </summary>
        /// <param name="argEn">AccountsDetails Entity is an Input.</param>
        /// <returns>Returns Boolean</returns>
        public bool Insert(AccountsDetailsEn argEn)
        {
            bool lbRes = false;
            string sqlCmd;
            try
            {
                sqlCmd = "INSERT INTO SAS_AccountsDetails(TransID,TransTempCode,TransCode,RefCode,Quantity,TransAmount," +
                    "Tax,Discount,TaxAmount,DiscountAmount,PaidAmount,TempAmount,TempPaidAmount,TransStatus,PostStatus,Ref1," +
                    "Ref2,Ref3,Priority,noKelompok,noWarran,amaunWarran,noAkaunPelajar,statusBayaran, Internal_Use) VALUES (@TransID,@TransTempCode,@TransCode,@RefCode,@Quantity,@TransAmount,@Tax," +
                    "@Discount,@TaxAmount,@DiscountAmount,@PaidAmount,@TempAmount,@TempPaidAmount,@TransStatus,@PostStatus," +
                    "@Ref1,@Ref2,@Ref3,@Priority,@noKelompok,@noWarran,@amaunWarran,@noAkaunPelajar,@statusBayaran, @Internal_Use) ";

                if (!FormHelp.IsBlank(sqlCmd))
                {
                    DbCommand cmd = _DatabaseFactory.GetDbCommand(Helper.GetDataBaseType, sqlCmd, DataBaseConnectionString);
                    _DatabaseFactory.AddInParameter(ref cmd, "@TransID", DbType.Int32, argEn.TransactionID);
                    _DatabaseFactory.AddInParameter(ref cmd, "@TransTempCode", DbType.String, argEn.TransTempCode);
                    _DatabaseFactory.AddInParameter(ref cmd, "@TransCode", DbType.String, argEn.TransactionCode);
                    _DatabaseFactory.AddInParameter(ref cmd, "@RefCode", DbType.String, argEn.ReferenceCode);
                    _DatabaseFactory.AddInParameter(ref cmd, "@Quantity", DbType.String, argEn.Quantity);
                    _DatabaseFactory.AddInParameter(ref cmd, "@TransAmount", DbType.Double, argEn.TransactionAmount);
                    _DatabaseFactory.AddInParameter(ref cmd, "@Tax", DbType.Double, argEn.TaxPercentage);
                    _DatabaseFactory.AddInParameter(ref cmd, "@Discount", DbType.Double, argEn.DiscountPercentage);
                    _DatabaseFactory.AddInParameter(ref cmd, "@TaxAmount", DbType.Double, argEn.TaxAmount);
                    _DatabaseFactory.AddInParameter(ref cmd, "@DiscountAmount", DbType.Double, argEn.DiscountAmount);
                    _DatabaseFactory.AddInParameter(ref cmd, "@PaidAmount", DbType.Double, argEn.PaidAmount);
                    _DatabaseFactory.AddInParameter(ref cmd, "@TempAmount", DbType.Double, argEn.TempAmount);
                    _DatabaseFactory.AddInParameter(ref cmd, "@TempPaidAmount", DbType.Double, argEn.TempPaidAmount);
                    _DatabaseFactory.AddInParameter(ref cmd, "@TransStatus", DbType.String, argEn.TransStatus);
                    _DatabaseFactory.AddInParameter(ref cmd, "@PostStatus", DbType.String, argEn.PostStatus);
                    _DatabaseFactory.AddInParameter(ref cmd, "@Ref1", DbType.String, argEn.ReferenceOne);
                    _DatabaseFactory.AddInParameter(ref cmd, "@Ref2", DbType.String, argEn.ReferenceTwo);
                    _DatabaseFactory.AddInParameter(ref cmd, "@Ref3", DbType.String, argEn.ReferenceThree);
                    _DatabaseFactory.AddInParameter(ref cmd, "@Priority", DbType.Int32, argEn.Priority);
                    _DatabaseFactory.AddInParameter(ref cmd, "@noKelompok", DbType.String, argEn.NoKelompok);
                    _DatabaseFactory.AddInParameter(ref cmd, "@noWarran", DbType.String, argEn.NoWarran);
                    _DatabaseFactory.AddInParameter(ref cmd, "@noAkaunPelajar", DbType.String, argEn.noAkaun);
                    _DatabaseFactory.AddInParameter(ref cmd, "@amaunWarran", DbType.String, argEn.AmaunWarran);
                    _DatabaseFactory.AddInParameter(ref cmd, "@statusBayaran", DbType.String, argEn.StatusBayaran);
                    _DatabaseFactory.AddInParameter(ref cmd, "@Internal_Use", DbType.String, argEn.Internal_Use);
                    _DbParameterCollection = cmd.Parameters;

                    int liRowAffected = _DatabaseFactory.ExecuteNonQuery(Helper.GetDataBaseType, cmd,
                        DataBaseConnectionString, sqlCmd, _DbParameterCollection);

                    if (liRowAffected > -1)
                        lbRes = true;
                    else
                        //cmd.Transaction.Rollback();
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

        #region Update

        /// <summary>
        /// Method to Update 
        /// </summary>
        /// <param name="argEn">AccountsDetails Entity is an Input.</param>
        /// <returns>Returns Boolean</returns>
        public bool Update(AccountsDetailsEn argEn)
        {
            bool lbRes = false;
            string sqlCmd;
            try
            {
                sqlCmd = "UPDATE SAS_AccountsDetails SET TransID = @TransID, TransTempCode = @TransTempCode, TransCode = @TransCode, RefCode = @RefCode, Quantity = @Quantity, TransAmount = @TransAmount, Tax = @Tax, Discount = @Discount, TaxAmount = @TaxAmount, DiscountAmount = @DiscountAmount, PaidAmount = @PaidAmount, TempAmount = @TempAmount, TempPaidAmount = @TempPaidAmount, TransStatus = @TransStatus, PostStatus = @PostStatus, Ref1 = @Ref1, Ref2 = @Ref2, Ref3 = @Ref3, Priority = @Priority WHERE TransID = @TransID";

                if (!FormHelp.IsBlank(sqlCmd))
                {
                    DbCommand cmd = _DatabaseFactory.GetDbCommand(Helper.GetDataBaseType, sqlCmd, DataBaseConnectionString);
                    _DatabaseFactory.AddInParameter(ref cmd, "@TransID", DbType.Int32, argEn.TransactionID);
                    _DatabaseFactory.AddInParameter(ref cmd, "@TransTempCode", DbType.String, argEn.TransTempCode);
                    _DatabaseFactory.AddInParameter(ref cmd, "@TransCode", DbType.String, argEn.TransactionCode);
                    _DatabaseFactory.AddInParameter(ref cmd, "@RefCode", DbType.String, argEn.ReferenceCode);
                    _DatabaseFactory.AddInParameter(ref cmd, "@Quantity", DbType.String, argEn.Quantity);
                    _DatabaseFactory.AddInParameter(ref cmd, "@TransAmount", DbType.Double, argEn.TransactionAmount);
                    _DatabaseFactory.AddInParameter(ref cmd, "@Tax", DbType.Double, argEn.TaxPercentage);
                    _DatabaseFactory.AddInParameter(ref cmd, "@Discount", DbType.Double, argEn.DiscountPercentage);
                    _DatabaseFactory.AddInParameter(ref cmd, "@TaxAmount", DbType.Double, argEn.TaxAmount);
                    _DatabaseFactory.AddInParameter(ref cmd, "@DiscountAmount", DbType.Double, argEn.DiscountAmount);
                    _DatabaseFactory.AddInParameter(ref cmd, "@PaidAmount", DbType.Double, argEn.PaidAmount);
                    _DatabaseFactory.AddInParameter(ref cmd, "@TempAmount", DbType.Double, argEn.TempAmount);
                    _DatabaseFactory.AddInParameter(ref cmd, "@TempPaidAmount", DbType.Double, argEn.TempPaidAmount);
                    _DatabaseFactory.AddInParameter(ref cmd, "@TransStatus", DbType.String, argEn.TransStatus);
                    _DatabaseFactory.AddInParameter(ref cmd, "@PostStatus", DbType.String, argEn.PostStatus);
                    _DatabaseFactory.AddInParameter(ref cmd, "@Ref1", DbType.String, argEn.ReferenceOne);
                    _DatabaseFactory.AddInParameter(ref cmd, "@Ref2", DbType.String, argEn.ReferenceTwo);
                    _DatabaseFactory.AddInParameter(ref cmd, "@Ref3", DbType.String, argEn.ReferenceThree);
                    _DatabaseFactory.AddInParameter(ref cmd, "@Priority", DbType.Int32, argEn.Priority);
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

        #region LoadObjectSucceedDetails

        /// <summary>
        /// Method to Load Succeed Details Entity
        /// </summary>
        /// <param name="argReader">Reader Object is an Input.</param>
        /// <returns>Returns Accounts Entity Object</returns>
        public AccountsDetailsEn LoadObjectSucceedDetails(IDataReader argReader)
        {
            AccountsDetailsEn loItemDet = new AccountsDetailsEn();
            loItemDet.NoKelompok = GetValue<string>(argReader, "STD_NoKelompok");
            loItemDet.KumpulanPelajar = GetValue<string>(argReader, "STD_KumpulanPelajar");
            loItemDet.NoWarran = GetValue<string>(argReader, "STD_NoWarran");
            loItemDet.NoPelajar = GetValue<string>(argReader, "STD_NoPelajar");
            loItemDet.NoIC = GetValue<string>(argReader, "STD_NoIC");
            loItemDet.NamaPelajar = GetValue<string>(argReader, "STD_NamaPelajar");
            loItemDet.AmaunWarran = GetValue<double>(argReader, "STD_AmaunWarran");
            loItemDet.AmaunPotongan = GetValue<string>(argReader, "STD_AmaunPotongan");
            loItemDet.NilaiBersih = GetValue<string>(argReader, "STD_NilaiBersih");
            loItemDet.TarikhTransaksi = GetValue<string>(argReader, "STD_TarikhTransaksi");
            loItemDet.TarikhLupusWarran = GetValue<string>(argReader, "STD_TarikhLupusWarran");
            loItemDet.noAkaun = GetValue<string>(argReader, "STD_NoAkaunPelajar");
            loItemDet.Filler = GetValue<string>(argReader, "STD_Filler");
            loItemDet.StatusBayaran = GetValue<string>(argReader, "STD_StatusBayaran");
            return loItemDet;
        }

        #endregion

        #region Delete

        /// <summary>
        /// Method to Delete 
        /// </summary>
        /// <param name="argEn">AccountsDetails Entity is an Input.</param>
        /// <returns>Returns Boolean</returns>
        public bool Delete(AccountsDetailsEn argEn)
        {
            bool lbRes = false;
            string sqlCmd = "DELETE FROM SAS_AccountsDetails WHERE TransID = @TransID";

            try
            {
                if (!FormHelp.IsBlank(sqlCmd))
                {
                    DbCommand cmd = _DatabaseFactory.GetDbCommand(Helper.GetDataBaseType, sqlCmd, DataBaseConnectionString);
                    _DatabaseFactory.AddInParameter(ref cmd, "@TransID", DbType.Int32, argEn.TransactionID);

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

        #region UpdateStudentStatus

        public bool UpdateStudentStatus(AccountsDetailsEn argEn)
        {
            bool lbRes = false;
            string sqlCmd = "UPDATE SAS_STUDENT SET SASI_POSTSTATUS=0 WHERE SASI_MATRICNO = @CREDITREF";

            try
            {
                if (!FormHelp.IsBlank(sqlCmd))
                {
                    DbCommand cmd = _DatabaseFactory.GetDbCommand(Helper.GetDataBaseType, sqlCmd, DataBaseConnectionString);
                    _DatabaseFactory.AddInParameter(ref cmd, "@CREDITREF", DbType.String, argEn.CreditRef);
                    _DbParameterCollection = cmd.Parameters;


                    int liRowAffected = _DatabaseFactory.ExecuteNonQuery(Helper.GetDataBaseType, cmd,
                                DataBaseConnectionString, sqlCmd, _DbParameterCollection);

                    if (liRowAffected > -1)
                        lbRes = true;
                    else
                        throw new Exception("Student Table Updation Failed! No Row has been updated...");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return lbRes;
        }

        #endregion

        #region Load Object

        /// <summary>
        /// Method to Load AccountDetails Entity 
        /// </summary>
        /// <param name="argReader">Reader Object is an Input.</param>
        /// <returns>Returns AccountDetails Entity</returns>
        private AccountsDetailsEn LoadObject(IDataReader argReader)
        {
            AccountsDetailsEn loItem = new AccountsDetailsEn();
            loItem.TransactionID = GetValue<int>(argReader, "TransID");
            loItem.TransTempCode = GetValue<string>(argReader, "TransTempCode");
            loItem.TransactionCode = GetValue<string>(argReader, "TransCode");
            loItem.ReferenceCode = GetValue<string>(argReader, "RefCode");
            loItem.Quantity = GetValue<string>(argReader, "Quantity");
            loItem.TransactionAmount = GetValue<double>(argReader, "TransAmount");
            loItem.TaxPercentage = GetValue<double>(argReader, "Tax");
            loItem.DiscountPercentage = GetValue<double>(argReader, "Discount");
            loItem.TaxAmount = GetValue<double>(argReader, "TaxAmount");
            loItem.DiscountAmount = GetValue<double>(argReader, "DiscountAmount");
            loItem.PaidAmount = GetValue<double>(argReader, "PaidAmount");
            loItem.TempAmount = GetValue<double>(argReader, "TempAmount");
            loItem.TempPaidAmount = GetValue<double>(argReader, "TempPaidAmount");
            loItem.TransStatus = GetValue<string>(argReader, "TransStatus");
            loItem.PostStatus = GetValue<string>(argReader, "PostStatus");
            loItem.ReferenceOne = GetValue<string>(argReader, "Ref1");
            loItem.ReferenceTwo = GetValue<string>(argReader, "Ref2");
            loItem.ReferenceThree = GetValue<string>(argReader, "Ref3");
            loItem.Priority = GetValue<int>(argReader, "Priority");

            return loItem;
        }

        /// <summary>
        /// Method to Load AccountDetails Entity 
        /// </summary>
        /// <param name="argReader">Reader Object is an Input.</param>
        /// <returns>Returns AccountDetails Entity</returns>
        private AccountsDetailsEn LoadObjectSponAlloc(IDataReader argReader)
        {
            AccountsDetailsEn loItem = new AccountsDetailsEn();
            loItem.Sudentacc = new StudentEn();
            loItem.CreditRef = GetValue<string>(argReader, "SASI_MatricNo");
            loItem.Sudentacc.ICNo = GetValue<string>(argReader, "SASI_ICNo");
            loItem.StudentName = GetValue<string>(argReader, "SASI_Name");
            loItem.TransactionAmount = GetValue<double>(argReader, "TransAmount");
            loItem.NoKelompok = GetValue<string>(argReader, "noKelompok");
            loItem.NoWarran = GetValue<string>(argReader, "noWarran");
            loItem.AmaunWarran = GetValue<double>(argReader, "amaunWarran");
            loItem.noAkaun = GetValue<string>(argReader, "noAkaunPelajar");
            //loItem.StatusBayaran = GetValue<string>(argReader, "statusBayaran");
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

        #region InsertSponsorInvoiceDetails

        /// <summary>
        /// Method to Insert SponsorInvoiceDetails
        /// </summary>
        /// <param name="argEn">AccountsDetails Entity is an Input.</param>
        /// <returns>Returns Boolean</returns>
        public bool InsertSponsorInvoiceDetails(AccountsDetailsEn argEn)
        {
            bool lbRes = false;
            string sqlCmd;
            try
            {
                sqlCmd = "INSERT INTO SAS_SponsorInvoiceDetails(TransID,TransTempCode,TransCode,RefCode,Quantity,TransAmount,Tax,Discount,TaxAmount,DiscountAmount,PaidAmount,TempAmount,TempPaidAmount,TransStatus,PostStatus,Ref1,Ref2,Ref3,Priority, Internal_Use) VALUES " +
                    "(@TransID,@TransTempCode,@TransCode,@RefCode,@Quantity,@TransAmount,@Tax,@Discount,@TaxAmount,@DiscountAmount,@PaidAmount,@TempAmount,@TempPaidAmount,@TransStatus,@PostStatus,@Ref1,@Ref2,@Ref3,@Priority, @Internal_Use) ";


                if (!FormHelp.IsBlank(sqlCmd))
                {
                    DbCommand cmd = _DatabaseFactory.GetDbCommand(Helper.GetDataBaseType, sqlCmd, DataBaseConnectionString);
                    _DatabaseFactory.AddInParameter(ref cmd, "@TransID", DbType.Int32, argEn.TransactionID);
                    _DatabaseFactory.AddInParameter(ref cmd, "@TransTempCode", DbType.String, argEn.TransTempCode);
                    _DatabaseFactory.AddInParameter(ref cmd, "@TransCode", DbType.String, argEn.TransactionCode);
                    _DatabaseFactory.AddInParameter(ref cmd, "@RefCode", DbType.String, argEn.ReferenceCode);
                    _DatabaseFactory.AddInParameter(ref cmd, "@Quantity", DbType.String, argEn.Quantity);
                    _DatabaseFactory.AddInParameter(ref cmd, "@TransAmount", DbType.Double, argEn.TransactionAmount);
                    _DatabaseFactory.AddInParameter(ref cmd, "@Tax", DbType.Double, argEn.TaxPercentage);
                    _DatabaseFactory.AddInParameter(ref cmd, "@Discount", DbType.Double, argEn.DiscountPercentage);
                    _DatabaseFactory.AddInParameter(ref cmd, "@TaxAmount", DbType.Double, argEn.TaxAmount);
                    _DatabaseFactory.AddInParameter(ref cmd, "@DiscountAmount", DbType.Double, argEn.DiscountAmount);
                    _DatabaseFactory.AddInParameter(ref cmd, "@PaidAmount", DbType.Double, argEn.PaidAmount);
                    _DatabaseFactory.AddInParameter(ref cmd, "@TempAmount", DbType.Double, argEn.TempAmount);
                    _DatabaseFactory.AddInParameter(ref cmd, "@TempPaidAmount", DbType.Double, argEn.TempPaidAmount);
                    _DatabaseFactory.AddInParameter(ref cmd, "@TransStatus", DbType.String, argEn.TransStatus);
                    _DatabaseFactory.AddInParameter(ref cmd, "@PostStatus", DbType.String, argEn.PostStatus);
                    _DatabaseFactory.AddInParameter(ref cmd, "@Ref1", DbType.String, argEn.ReferenceOne);
                    _DatabaseFactory.AddInParameter(ref cmd, "@Ref2", DbType.String, argEn.ReferenceTwo);
                    _DatabaseFactory.AddInParameter(ref cmd, "@Ref3", DbType.String, argEn.ReferenceThree);
                    _DatabaseFactory.AddInParameter(ref cmd, "@Priority", DbType.Int32, argEn.Priority);
                    _DatabaseFactory.AddInParameter(ref cmd, "@Internal_Use", DbType.String, argEn.Internal_Use);
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

        #region DeleteSponsorInvoiceDetails

        /// <summary>
        /// Method to Delete SponsorInvoiceDetails
        /// </summary>
        /// <param name="argEn">AccountsDetails Entity is an Input.</param>
        /// <returns>Returns Boolean</returns>
        public bool DeleteSponsorInvoiceDetails(AccountsDetailsEn argEn)
        {
            bool lbRes = false;
            string sqlCmd = "DELETE FROM SAS_SponsorInvoiceDetails WHERE TransID = @TransID";

            try
            {
                if (!FormHelp.IsBlank(sqlCmd))
                {
                    DbCommand cmd = _DatabaseFactory.GetDbCommand(Helper.GetDataBaseType, sqlCmd, DataBaseConnectionString);
                    _DatabaseFactory.AddInParameter(ref cmd, "@TransID", DbType.Int32, argEn.TransactionID);
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

        #region GetStudentSponsorInvoiceAccountsDetails

        /// <summary>
        /// Method to Get Student SponsorInvoice AccountDetails
        /// </summary>
        /// <param name="argEn">AccountsDetails Entity is an Input.TransactionId is an Input Property</param>
        /// <returns>Returns List Of AccountDetails Entities</returns>
        public List<AccountsDetailsEn> GetStudentSponsorInvoiceAccountsDetails(AccountsDetailsEn argEn)
        {
            List<AccountsDetailsEn> loEnList = new List<AccountsDetailsEn>();
            string sqlCmd = " SELECT SAS_FeeTypes.SAFT_Code, SAS_FeeTypes.SAFT_Desc, SAS_FeeTypes.SAFT_FeeType, SAS_FeeTypes.SAFT_Hostel," +
                            " SAS_FeeTypes.SAFT_Priority, SAS_FeeTypes.SAFT_Remarks, SAS_FeeTypes.SAFT_GLCode, SAS_FeeTypes.SAFT_Status,SAS_SponsorInvoiceDetails.TransAmount," +
                            " SAS_SponsorInvoiceDetails.RefCode, SAS_SponsorInvoiceDetails.TransID, SAS_SponsorInvoiceDetails.TransTempCode,SAS_SponsorInvoiceDetails.TransCode" +
                //" FROM SAS_SponsorInvoiceDetails INNER JOIN SAS_FeeTypes ON SAS_SponsorInvoiceDetails.RefCode = SAS_FeeTypes.SAFT_Code where  TransID='" + argEn.TransactionID + "'";
                            " FROM SAS_SponsorInvoice Inner Join SAS_SponsorInvoiceDetails on SAS_SponsorInvoice.TransCode = SAS_SponsorInvoiceDetails.TransCode" +
                            " INNER JOIN SAS_FeeTypes ON SAS_SponsorInvoiceDetails.RefCode = SAS_FeeTypes.SAFT_Code WHERE SAS_SponsorInvoice.TransID='" + argEn.TransactionID + "'";

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
                            loItem.FeeTypeCode = GetValue<string>(loReader, "SAFT_Code");
                            loItem.Description = GetValue<string>(loReader, "SAFT_Desc");
                            loItem.FeeType = GetValue<string>(loReader, "SAFT_FeeType");
                            loItem.Hostel = GetValue<string>(loReader, "SAFT_Hostel");
                            loItem.Priority = GetValue<int>(loReader, "SAFT_Priority");
                            loItem.Remarks = GetValue<string>(loReader, "SAFT_Remarks");
                            loItem.GLCode = GetValue<string>(loReader, "SAFT_GLCode");
                            loItem.Status = GetValue<bool>(loReader, "SAFT_Status");
                            loItem.TransactionID = GetValue<int>(loReader, "TransID");
                            loItem.TransTempCode = GetValue<string>(loReader, "TransTempCode");
                            loItem.TransactionAmount = GetValue<double>(loReader, "TransAmount");
                            loItem.TransactionCode = GetValue<string>(loReader, "TransCode");
                            loItem.ReferenceCode = GetValue<string>(loReader, "RefCode");
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

        /// <summary>
        /// Method to Get Student SponsorInvoice AccountDetails
        /// </summary>
        /// <param name="argEn">Accounts Entity is an Input.BatchCode is an Input Property</param>
        /// <returns>Returns List Of AccountDetails Entities</returns>
        public List<AccountsDetailsEn> GetStudentSponsorInvoiceAccountsDetailsByBatchCode(AccountsEn argEn)
        {
            List<AccountsDetailsEn> loEnList = new List<AccountsDetailsEn>();
            string sqlCmd = "";
            sqlCmd = @"select stu.sasi_matricno, stu.sasi_name, stu.sasi_cursemyr, stu.sasi_icno, prog.sapg_programbm, ft.saft_code, ft.saft_desc, ft.saft_feetype,
                        ft.saft_taxmode, sid.transamount, sid.taxamount, sid.tax, ft.SAFT_Hostel, ft.SAFT_Priority, ft.SAFT_Remarks, ft.SAFT_Status, sid.Transid,
                        sid.RefCode, sid.TransTempCode, sid.TransCode, sid.internal_use, si.creditref1, stu.sasc_code
                        from sas_sponsorInvoice si
                        inner join sas_sponsorInvoiceDetails sid on si.transcode = sid.transcode and si.transtempcode = sid.transtempcode
                        inner join sas_feetypes ft on ft.saft_code = sid.refcode
                        inner join sas_student stu on stu.sasi_matricno = si.creditref
                        left join sas_program prog on prog.sapg_code = stu.sasi_pgid
                        where si.batchcode =" + clsGeneric.AddQuotes(argEn.BatchCode);
            try
            {
                if (!FormHelp.IsBlank(sqlCmd))
                {
                    using (IDataReader loReader = _DatabaseFactory.ExecuteReader(Helper.GetDataBaseType,
                       DataBaseConnectionString, sqlCmd).CreateDataReader())
                    {
                        while (loReader.Read())
                        {
                            StudentEn getStudent = new StudentEn();
                            getStudent.MatricNo = GetValue<string>(loReader, "sasi_matricno");
                            getStudent.StudentName = GetValue<string>(loReader, "sasi_name");
                            getStudent.CurretSemesterYear = GetValue<string>(loReader, "sasi_cursemyr");
                            getStudent.ProgramName = GetValue<string>(loReader, "sapg_programbm");
                            getStudent.ICNo = GetValue<string>(loReader, "sasi_icno");
                            getStudent.CategoryCode = GetValue<string>(loReader, "sasc_code");

                            AccountsDetailsEn loItem = new AccountsDetailsEn();
                            loItem.FeeTypeCode = GetValue<string>(loReader, "SAFT_Code");
                            loItem.Description = GetValue<string>(loReader, "SAFT_Desc");
                            loItem.FeeType = GetValue<string>(loReader, "SAFT_FeeType");
                            loItem.Hostel = GetValue<string>(loReader, "SAFT_Hostel");
                            loItem.Priority = GetValue<int>(loReader, "SAFT_Priority");
                            loItem.Remarks = GetValue<string>(loReader, "SAFT_Remarks");
                            loItem.Status = GetValue<bool>(loReader, "SAFT_Status");
                            loItem.TransactionID = GetValue<int>(loReader, "TransID");
                            loItem.TransTempCode = GetValue<string>(loReader, "TransTempCode");
                            loItem.TransactionAmount = GetValue<double>(loReader, "TransAmount");
                            loItem.TransactionCode = GetValue<string>(loReader, "TransCode");
                            loItem.ReferenceCode = GetValue<string>(loReader, "RefCode");
                            loItem.TaxAmount = GetValue<double>(loReader, "taxamount");
                            loItem.GSTAmount = GetValue<double>(loReader, "taxamount");
                            loItem.TaxId = GetValue<int>(loReader, "saft_taxmode");
                            loItem.Internal_Use = GetValue<string>(loReader, "internal_use");
                            loItem.SponsorCode = GetValue<string>(loReader, "creditref1");
                            
                            loItem.Sudentacc = getStudent;
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

        #region GetStudentListBasedonReceiptNoo

        /// <summary>
        /// Method to Get GetStudentListBasedonReceiptNoo
        /// </summary>
        /// <param name="argEn">AccountDetails Entity is an Input.TransactionID is an Input Property</param>
        /// <returns>Returns List of GetStudentListBasedonReceiptNoo Entities</returns>
        public List<AccountsDetailsEn> GetStudentListBasedonReceiptNoo(AccountsDetailsEn argEn)
        {
            List<AccountsDetailsEn> loEnList = new List<AccountsDetailsEn>();
            string sqlCmd = " SELECT DISTINCT SAS_Student.SASI_MatricNo, SAS_Student.SASI_Name, SAS_Student.SASI_PgId, SAS_Student.SASI_ICNo," +
                            " SAS_Student.SASI_CurSem, SAS_AccountsDetails.TransTempCode," +
                //" SAS_AccountsDetails.RefCode, SAS_AccountsDetails.PaidAmount," +
                            " SAS_AccountsDetails.TempAmount, SAS_AccountsDetails.TempPaidAmount, " +
                            " SAS_AccountsDetails.PostStatus, SAS_AccountsDetails.Ref1,SAS_AccountsDetails.NoKelompok ," +
                            " SAS_AccountsDetails.NoWarran,SAS_AccountsDetails.AmaunWarran,SAS_AccountsDetails.noAkaunPelajar,SAS_AccountsDetails.StatusBayaran " +
                            " FROM  SAS_AccountsDetails INNER JOIN SAS_sponsorinvoice ON" +
                            " sas_sponsorinvoice.creditref1 = SAS_AccountsDetails.ref1 or sas_sponsorinvoice.creditref1 = SAS_AccountsDetails.refcode INNER JOIN sas_student ON" +
                            " sas_sponsorinvoice.creditref = sas_student.sasi_matricno where sas_sponsorinvoice.batchcode ='" + argEn.TransTempCode + "' and sas_accountsdetails.poststatus ='" + argEn.PostStatus + "'";

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
                            loItem.Sudentacc = new StudentEn();
                            //loItem.TransactionID = GetValue<int>(loReader, "TransID");
                            loItem.TransTempCode = GetValue<string>(loReader, "TransTempCode");
                            //loItem.TransactionCode = GetValue<string>(loReader, "TransCode");
                            //loItem.ReferenceCode = GetValue<string>(loReader, "RefCode");
                            //loItem.TransactionAmount = GetValue<double>(loReader, "TransAmount");
                            //loItem.PaidAmount = GetValue<double>(loReader, "PaidAmount");
                            loItem.TempAmount = GetValue<double>(loReader, "TempAmount");
                            loItem.TempPaidAmount = GetValue<double>(loReader, "TempPaidAmount");
                            loItem.Sudentacc.MatricNo = GetValue<string>(loReader, "SASI_MatricNo");
                            loItem.Sudentacc.StudentName = GetValue<string>(loReader, "SASI_Name");
                            loItem.Sudentacc.ProgramID = GetValue<string>(loReader, "SASI_PgId");
                            loItem.Sudentacc.ICNo = GetValue<string>(loReader, "SASI_ICNo");
                            loItem.ReferenceOne = GetValue<string>(loReader, "Ref1");
                            //loItem.ReferenceThree = GetValue<string>(loReader, "Ref3");
                            loItem.Sudentacc.CurrentSemester = GetValue<int>(loReader, "SASI_CurSem");
                            loItem.NoKelompok = GetValue<string>(loReader, "NoKelompok");
                            loItem.NoWarran = GetValue<string>(loReader, "NoWarran");
                            loItem.AmaunWarran = GetValue<double>(loReader, "AmaunWarran");
                            loItem.noAkaun = GetValue<string>(loReader, "noAkaunPelajar");
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

        #region GetStuDentAllocationPocketAmount

        /// <summary>
        /// Method to Get GetStuDentAllocationPocketAmount
        /// </summary>
        /// <param name="argEn">AccountDetails Entity is an Input.TransactionID is an Input Property</param>
        /// <returns>Returns List of AccountsDetails Entities</returns>
        public List<AccountsDetailsEn> GetStuDentAllocationPocketAmount(AccountsEn argEn)
        {
            List<AccountsDetailsEn> loEnList = new List<AccountsDetailsEn>();
            string sqlCmd = " SELECT Distinct SAS_Student.SASI_MatricNo, SAS_Student.SASI_Name, SAS_Student.SASI_PgId, SAS_Student.SASI_ICNo,SAS_Student.SASI_CurSem, SAS_AccountsDetails.TransID ,SAS_AccountsDetails.TransTempCode," +
                            " SAS_AccountsDetails.TransCode,SAS_AccountsDetails.RefCode, SAS_AccountsDetails.TransAmount, SAS_AccountsDetails.PaidAmount, SAS_AccountsDetails.TempAmount, SAS_AccountsDetails.TempPaidAmount," +
                            " SAS_AccountsDetails.TransStatus, SAS_AccountsDetails.PostStatus, SAS_AccountsDetails.Ref1, SAS_AccountsDetails.Ref3,SAS_AccountsDetails.NoKelompok ," +
                            " SAS_AccountsDetails.NoWarran,SAS_AccountsDetails.AmaunWarran,SAS_AccountsDetails.noAkaunPelajar,SAS_AccountsDetails.StatusBayaran " +
                            " FROM SAS_AccountsDetails INNER JOIN SAS_Student ON" +
                            " SAS_AccountsDetails.RefCode = SAS_Student.SASI_MatricNo INNER JOIN Sas_accounts ON SAS_student.SASI_MatricNo = SAS_Accounts.CreditRef where SAS_AccountsDetails.TempAmount >0 and " +
                            " SAS_accountsdetails.transid ='" + argEn.TranssactionID + "'";

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
                            loItem.Sudentacc = new StudentEn();
                            loItem.TransactionID = GetValue<int>(loReader, "TransID");
                            loItem.TransTempCode = GetValue<string>(loReader, "TransTempCode");
                            loItem.TransactionCode = GetValue<string>(loReader, "TransCode");
                            loItem.ReferenceCode = GetValue<string>(loReader, "RefCode");
                            loItem.TransactionAmount = GetValue<double>(loReader, "TransAmount");
                            loItem.PaidAmount = GetValue<double>(loReader, "PaidAmount");
                            loItem.TempAmount = GetValue<double>(loReader, "TempAmount");
                            loItem.TempPaidAmount = GetValue<double>(loReader, "TempPaidAmount");
                            loItem.Sudentacc.MatricNo = GetValue<string>(loReader, "SASI_MatricNo");
                            loItem.Sudentacc.StudentName = GetValue<string>(loReader, "SASI_Name");
                            loItem.Sudentacc.ProgramID = GetValue<string>(loReader, "SASI_PgId");
                            loItem.Sudentacc.ICNo = GetValue<string>(loReader, "SASI_ICNo");
                            loItem.ReferenceOne = GetValue<string>(loReader, "Ref1");
                            loItem.ReferenceThree = GetValue<string>(loReader, "Ref3");
                            loItem.Sudentacc.CurrentSemester = GetValue<int>(loReader, "SASI_CurSem");
                            loItem.NoKelompok = GetValue<string>(loReader, "NoKelompok");
                            loItem.NoWarran = GetValue<string>(loReader, "NoWarran");
                            loItem.AmaunWarran = GetValue<double>(loReader, "AmaunWarran");
                            loItem.noAkaun = GetValue<string>(loReader, "noAkaunPelajar");
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

        #region CheckPtptnFlag
        //added by Hafiz @ 22/3/2016
        //get ptptn flag in sas_sponsor

        public bool CheckPtptnFlag(String spon_code)
        {
            bool res = false;
            int output = 0;

            string sqlCmd = " SELECT count(*) as cnt FROM SAS_Sponsor WHERE SASR_Code = @spon_code" +
                            " AND SASR_ptptn = true ";

            try
            {
                if (!FormHelp.IsBlank(sqlCmd))
                {

                    if (string.IsNullOrEmpty(spon_code))
                    {
                        spon_code = "";
                    }

                    DbCommand cmdSel = _DatabaseFactory.GetDbCommand(Helper.GetDataBaseType, sqlCmd, DataBaseConnectionString);
                    _DatabaseFactory.AddInParameter(ref cmdSel, "@spon_code", DbType.String, spon_code);
                    _DbParameterCollection = cmdSel.Parameters;

                    using (IDataReader dr = _DatabaseFactory.GetIDataReader(Helper.GetDataBaseType, cmdSel,
                        DataBaseConnectionString, sqlCmd, _DbParameterCollection).CreateDataReader())
                    {
                        //if (dr.Read())
                        if (dr.Read() && dr.FieldCount != 0)
                        {
                            output = clsGeneric.NullToInteger(dr["cnt"]);
                        }
                    }

                    if (output == 1)
                    {
                        res = true;
                    }
                    else
                    {
                        //output=0
                        res = false;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return res;
        }

        #endregion

        #region GetStudentPaymentAllocation

        /// <summary>
        /// Method to Get Student Allocations
        /// </summary>
        /// <param name="argEn">AccountDetails Entity is an Input.TransactionID is an Input Property</param>
        /// <returns>Returns List of AccountsDetails Entities</returns>
        public List<AccountsDetailsEn> GetStudentPaymentAllocation(AccountsDetailsEn argEn)
        {
            List<AccountsDetailsEn> loEnList = new List<AccountsDetailsEn>();
            string sqlCmd = " SELECT SAS_Student.SASI_MatricNo, SAS_Student.SASI_Name, SAS_Student.SASI_PgId, SAS_Student.SASI_ICNo," +
                            " SAS_Student.SASI_CurSem, SAS_AccountsDetails.TransID, SAS_AccountsDetails.TransTempCode, SAS_AccountsDetails.TransCode," +
                            " SAS_AccountsDetails.RefCode, SAS_AccountsDetails.TransAmount, SAS_AccountsDetails.PaidAmount," +
                            " SAS_AccountsDetails.TempAmount, SAS_AccountsDetails.TempPaidAmount, SAS_AccountsDetails.TransStatus, " +
                            " SAS_AccountsDetails.PostStatus, SAS_AccountsDetails.Ref1, SAS_AccountsDetails.Ref3,SAS_AccountsDetails.NoKelompok ," +
                            " SAS_AccountsDetails.NoWarran,SAS_AccountsDetails.AmaunWarran,SAS_AccountsDetails.noAkaunPelajar,SAS_AccountsDetails.StatusBayaran " +
                            " FROM  SAS_AccountsDetails INNER JOIN SAS_Student ON" +
                            " SAS_AccountsDetails.RefCode = SAS_Student.SASI_MatricNo where SAS_AccountsDetails.TransID='" + argEn.TransactionID + "'";

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
                            loItem.Sudentacc = new StudentEn();
                            loItem.TransactionID = GetValue<int>(loReader, "TransID");
                            loItem.TransTempCode = GetValue<string>(loReader, "TransTempCode");
                            loItem.TransactionCode = GetValue<string>(loReader, "TransCode");
                            loItem.ReferenceCode = GetValue<string>(loReader, "RefCode");
                            loItem.TransactionAmount = GetValue<double>(loReader, "TransAmount");
                            loItem.PaidAmount = GetValue<double>(loReader, "PaidAmount");
                            loItem.TempAmount = GetValue<double>(loReader, "TempAmount");
                            loItem.TempPaidAmount = GetValue<double>(loReader, "TempPaidAmount");
                            loItem.Sudentacc.MatricNo = GetValue<string>(loReader, "SASI_MatricNo");
                            loItem.Sudentacc.StudentName = GetValue<string>(loReader, "SASI_Name");
                            loItem.Sudentacc.ProgramID = GetValue<string>(loReader, "SASI_PgId");
                            loItem.Sudentacc.ICNo = GetValue<string>(loReader, "SASI_ICNo");
                            loItem.ReferenceOne = GetValue<string>(loReader, "Ref1");
                            loItem.ReferenceThree = GetValue<string>(loReader, "Ref3");
                            loItem.Sudentacc.CurrentSemester = GetValue<int>(loReader, "SASI_CurSem");
                            loItem.NoKelompok = GetValue<string>(loReader, "NoKelompok");
                            loItem.NoWarran = GetValue<string>(loReader, "NoWarran");
                            loItem.AmaunWarran = GetValue<double>(loReader, "AmaunWarran");
                            loItem.noAkaun = GetValue<string>(loReader, "noAkaunPelajar");
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
    }

}


