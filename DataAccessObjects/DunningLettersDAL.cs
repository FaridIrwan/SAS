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
    /// Class to handle all the DunningLetters.
    /// </summary>
    public class DunningLettersDAL
    {
        #region Global Declarations

        private DbParameterCollection _DbParameterCollection = null;

        private MaxModule.DatabaseProvider _DatabaseFactory =
            new MaxModule.DatabaseProvider();

        private string DataBaseConnectionString = Helper.
           GetConnectionString();

        #endregion

        public DunningLettersDAL()
        {
        }

        #region GetList

        /// <summary>
        /// Method to Get DunningLetters 
        /// </summary>
        /// <param name="argEn">DunningLetters Entity is an Input.Code and Title as Input Properties.</param>
        /// <returns>Returns List of DunningLetters.</returns>
        public List<DunningLettersEn> GetList(DunningLettersEn argEn)
        {
            List<DunningLettersEn> loEnList = new List<DunningLettersEn>();
            argEn.Code = argEn.Code.Replace("*", "%");
            argEn.Title = argEn.Title.Replace("*", "%");
            string sqlCmd = "select * from SAS_DunningLetters where SADL_Code <> '0'";
            if (argEn.Code.Length != 0) sqlCmd = sqlCmd + " and SADL_Code like '" + argEn.Code + "'";
            if (argEn.Title.Length != 0) sqlCmd = sqlCmd + " and SADL_Title like '" + argEn.Title + "'";

            try
            {
                if (!FormHelp.IsBlank(sqlCmd))
                {
                    using (IDataReader loReader = _DatabaseFactory.ExecuteReader(Helper.GetDataBaseType,
                        DataBaseConnectionString, sqlCmd).CreateDataReader())
                    {
                        while (loReader.Read())
                        {
                            DunningLettersEn loItem = LoadObject(loReader);
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

        #region ListDunningWarning

        /// <summary>
        /// Method to Get DunningLetters 
        /// </summary>
        /// <param name="argEn">DunningLetters Entity is an Input.Code and Title as Input Properties.</param>
        /// <returns>Returns List of DunningLetters.</returns>
        public List<DunningLettersEn> ListDunningWarning(DunningLettersEn argEn)
        {
            List<DunningLettersEn> loEnList = new List<DunningLettersEn>();
            //argEn.Code = argEn.Code.Replace("*", "%");
            //argEn.Title = argEn.Title.Replace("*", "%");
            //argEn.Status = argEn.Status.Replace("*", "%");
            string sqlCmd = "SELECT distinct SAS_Student.SASI_MatricNo,SAS_Student.SASI_Name," +
            "SAS_Student.SASI_PgId,SAS_Student.SASI_ICNo, SAS_Student.SASI_CurSemYr, " +
            "case WHEN SAS_dunningletterwarning.SADL_Warn is null Then 'First Time' " +
            "when SAS_dunningletterwarning.SADL_Warn = '0' then 'First Time' " +
            "when SAS_dunningletterwarning.SADL_Warn = '1' then 'Second Time' " +
            "when SAS_dunningletterwarning.SADL_Warn = '2' then 'Final Warning' " +
            "else 'overlimit' end SADL_Warn,case when SAS_dunningletterwarning.SADL_Code is null Then 'W1' " +
            "when SAS_dunningletterwarning.SADL_Code = 'W0' then 'W1' " +
            "when SAS_dunningletterwarning.SADL_Code = 'W1' then 'W2' " +
            "when SAS_dunningletterwarning.SADL_Code = 'W2' then 'W3' " +
            "else 'overlimit' end SADL_Code " +
            "FROM SAS_Program, SAS_Accounts INNER JOIN  SAS_Student ON SAS_Accounts.CreditRef = " +
            "SAS_Student.SASI_MatricNo left join SAS_dunningletterwarning on " +
            "(SAS_dunningletterwarning.SAS_MatricNo = SAS_Student.SASI_MatricNo and SAS_dunningletterwarning.SADL_Semester = SAS_Student.SASI_CurSemYr)" +
            "WHERE SAS_Program.SAPG_Code =  " +
            "SAS_Student.SASI_PgId and SAS_Accounts.SubType = 'Student' and SAS_Accounts.PostStatus = 'Posted'  " +
            "and SAS_Student.SASI_StatusRec like '%' And ((Select SUM(SAC.TransAmount) as DebitAmount from " +
            "SAS_Accounts SAC Where SAC.CreditRef = SAS_Accounts.CreditRef and SAC.TransType = 'Debit' And " +
            "SAC.poststatus = 'Posted') < (Select SUM(SAC.TransAmount) as CreditAmount from " +
            "SAS_Accounts SAC Where SAC.CreditRef = SAS_Accounts.CreditRef and SAC.TransType = 'Credit' And  " +
            "SAC.poststatus = 'Posted')) ";
            sqlCmd = sqlCmd + " Order By SAS_Student.SASI_MatricNo";

            try
            {
                if (!FormHelp.IsBlank(sqlCmd))
                {
                    using (IDataReader loReader = _DatabaseFactory.ExecuteReader(Helper.GetDataBaseType,
                        DataBaseConnectionString, sqlCmd).CreateDataReader())
                    {
                        while (loReader.Read())
                        {
                            DunningLettersEn loItem = LoadObject4(loReader);
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

        #region CheckDunningListing

        /// <summary>
        /// Method to Get DunningLetters 
        /// </summary>
        /// <param name="argEn">DunningLetters Entity is an Input.</param>
        /// <returns>Returns List of DunningLetters.</returns>
        public List<DunningLettersEn> CheckDunningListing(DunningLettersEn argEn, string MatricNo)
        {
            DunningLettersDAL lods = new DunningLettersDAL();
            List<DunningLettersEn> loEnList = new List<DunningLettersEn>();
            string sqlCmd = "SELECT distinct SAS_dunningletterwarning.SAS_MatricNo, SAS_dunningletterwarning.SADL_Semester, " +
            "SAS_dunningletterwarning.SADL_Warn,SAS_dunningletterwarning.SADL_Code " +
            "FROM SAS_dunningletterwarning WHERE SAS_dunningletterwarning.SAS_MatricNo = '" + MatricNo +
            "' Order By SAS_dunningletterwarning.SAS_MatricNo";

            try
            {
                if (!FormHelp.IsBlank(sqlCmd))
                {
                    using (IDataReader loReader = _DatabaseFactory.ExecuteReader(Helper.GetDataBaseType,
                        DataBaseConnectionString, sqlCmd).CreateDataReader())
                    {
                        while (loReader.Read())
                        {
                            DunningLettersEn loItem = LoadObject3(loReader);
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

        #region CheckListDunning

        /// <summary>
        /// Method to Get DunningLetters 
        /// </summary>
        /// <param name="argEn">DunningLetters Entity is an Input.</param>
        /// <returns>Returns List of DunningLetters.</returns>
        /// Modified by Hafiz @ 08/3/2016
        /// Fix errors when SASI_StatusRec LIKE '%' - statusRec=Boolean

        public List<DunningLettersEn> CheckListDunning(DunningLettersEn argEn)
        {
            DunningLettersDAL lods = new DunningLettersDAL();
            List<DunningLettersEn> loEnList = new List<DunningLettersEn>();
            //argEn.Status = argEn.Status.Replace("*", "%");
            string sqlCmd = "SELECT distinct SAS_Student.SASI_MatricNo, SAS_Student.SASI_CurSemYr, " +
            "SAS_dunningletterwarning.SADL_Warn,SAS_dunningletterwarning.SADL_Code " +
            "FROM SAS_Program, SAS_Accounts INNER JOIN  SAS_Student ON SAS_Accounts.CreditRef = " +
            "SAS_Student.SASI_MatricNo left join SAS_dunningletterwarning on " +
            "(SAS_dunningletterwarning.SAS_MatricNo = SAS_Student.SASI_MatricNo and SAS_dunningletterwarning.SADL_Semester = SAS_Student.SASI_CurSemYr)" +
            "WHERE SAS_Program.SAPG_Code =  " +
            "SAS_Student.SASI_PgId and SAS_Accounts.SubType = 'Student' and SAS_Accounts.PostStatus = 'Posted'  " +
            "and SAS_Student.SASI_StatusRec IN ('0','1') And ((Select SUM(SAC.TransAmount) as DebitAmount from " +
            "SAS_Accounts SAC Where SAC.CreditRef = SAS_Accounts.CreditRef and SAC.TransType = 'Debit' And " +
            "SAC.poststatus = 'Posted') < (Select SUM(SAC.TransAmount) as CreditAmount from " +
            "SAS_Accounts SAC Where SAC.CreditRef = SAS_Accounts.CreditRef and SAC.TransType = 'Credit' And  " +
            "SAC.poststatus = 'Posted')) ";
            sqlCmd = sqlCmd + " Order By SAS_Student.SASI_MatricNo";

            try
            {
                if (!FormHelp.IsBlank(sqlCmd))
                {
                    using (IDataReader loReader = _DatabaseFactory.ExecuteReader(Helper.GetDataBaseType,
                        DataBaseConnectionString, sqlCmd).CreateDataReader())
                    {
                        while (loReader.Read())
                        {
                            DunningLettersEn loItem = LoadObject2(loReader);
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

        #region GetListDunning

        /// <summary>
        /// Method to Get DunningLetters 
        /// </summary>
        /// <param name="argEn">DunningLetters Entity is an Input.</param>
        /// <returns>Returns List of DunningLetters.</returns>
        /// Modified by Hafiz @ 08/3/2016
        /// Fix errors when SASI_StatusRec LIKE '%' - statusRec=Boolean

        public List<DunningLettersEn> GetListDunning(DunningLettersEn argEn)
        {
            DunningLettersDAL lods = new DunningLettersDAL();
            List<DunningLettersEn> loEnList = new List<DunningLettersEn>();
            string sqlCmd = "SELECT distinct SAS_Student.SASI_MatricNo, SAS_Student.SASI_CurSemYr, " +
            "case WHEN SAS_dunningletterwarning.SADL_Warn is null Then '1' " +
            "when SAS_dunningletterwarning.SADL_Warn = '0' then '1' " +
            "when SAS_dunningletterwarning.SADL_Warn = '1' then '2' " +
            "when SAS_dunningletterwarning.SADL_Warn = '2' then '3' " +
            "else 'overlimit' end SADL_Warn,case when SAS_dunningletterwarning.SADL_Code is null Then 'W1' " +
            "when SAS_dunningletterwarning.SADL_Code = 'W0' then 'W1' " +
            "when SAS_dunningletterwarning.SADL_Code = 'W1' then 'W2' " +
            "when SAS_dunningletterwarning.SADL_Code = 'W2' then 'W3' " +
            "else 'overlimit' end SADL_Code " +
            "FROM SAS_Program, SAS_Accounts INNER JOIN  SAS_Student ON SAS_Accounts.CreditRef = " +
            "SAS_Student.SASI_MatricNo left join SAS_dunningletterwarning on " +
            "(SAS_dunningletterwarning.SAS_MatricNo = SAS_Student.SASI_MatricNo and SAS_dunningletterwarning.SADL_Semester = SAS_Student.SASI_CurSemYr)" +
            "WHERE SAS_Program.SAPG_Code =  " +
            "SAS_Student.SASI_PgId and SAS_Accounts.SubType = 'Student' and SAS_Accounts.PostStatus = 'Posted'  " +
            "and SAS_Student.SASI_StatusRec IN ('0','1') And ((Select SUM(SAC.TransAmount) as DebitAmount from " +
            "SAS_Accounts SAC Where SAC.CreditRef = SAS_Accounts.CreditRef and SAC.TransType = 'Debit' And " +
            "SAC.poststatus = 'Posted') < (Select SUM(SAC.TransAmount) as CreditAmount from " +
            "SAS_Accounts SAC Where SAC.CreditRef = SAS_Accounts.CreditRef and SAC.TransType = 'Credit' And  " +
            "SAC.poststatus = 'Posted')) ";
            sqlCmd = sqlCmd + " Order By SAS_Student.SASI_MatricNo";

            try
            {
                if (!FormHelp.IsBlank(sqlCmd))
                {
                    using (IDataReader loReader = _DatabaseFactory.ExecuteReader(Helper.GetDataBaseType,
                        DataBaseConnectionString, sqlCmd).CreateDataReader())
                    {
                        while (loReader.Read())
                        {
                            DunningLettersEn loItem = LoadObject2(loReader);
                            loEnList.Add(loItem);
                            lods.InsertDunning(loItem);
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

        #region GetListWarning

        /// <summary>
        /// Method to Get DunningLetters Warning
        /// </summary>
        /// <param name="argEn">DunningLetters Entity is an Input.Code and Title as Input Properties.</param>
        /// <returns>Returns List of DunningLetters.</returns>
        public List<DunningLettersEn> GetListWarning(DunningLettersEn argEn)
        {
            List<DunningLettersEn> loEnList = new List<DunningLettersEn>();
            argEn.Code = argEn.Code.Replace("*", "%");
            string sqlCmd = "SELECT SAS_DunningLetterWarning.SADL_Code " +
                                        "FROM SAS_Program, SAS_Accounts INNER JOIN  SAS_Student " +
                                        "ON SAS_Accounts.CreditRef = SAS_Student.SASI_MatricNo WHERE SAS_Program.SAPG_Code = " +
                                        "SAS_Student.SASI_PgId and SAS_Accounts.SubType = 'Student' and SAS_Accounts.PostStatus = 'Posted'  " +
                                        "and SAS_Student.SASI_StatusRec like '%' And (Select SUM(SAC.TransAmount) as DebitAmount from " +
                                        "SAS_Accounts SAC Where SAC.CreditRef = SAS_Accounts.CreditRef and SAC.TransType = 'Debit' And " +
                                        "SAC.poststatus = 'Posted') < (Select SUM(SAC.TransAmount) as CreditAmount from " +
                                        "SAS_Accounts SAC Where SAC.CreditRef = SAS_Accounts.CreditRef and SAC.TransType = 'Credit' And  " +
                                        "SAC.poststatus = 'Posted') Order By SAS_Student.SASI_PgId, SAS_Student.SASI_Name, " +
                                        "SAS_Accounts.TransDate ";

            try
            {
                if (!FormHelp.IsBlank(sqlCmd))
                {
                    DbCommand cmd = _DatabaseFactory.GetDbCommand(Helper.GetDataBaseType, sqlCmd, DataBaseConnectionString);
                    _DatabaseFactory.AddInParameter(ref cmd, "@SADL_Code", DbType.String, argEn.Code);
                    _DbParameterCollection = cmd.Parameters;

                    using (IDataReader loReader = _DatabaseFactory.GetIDataReader(Helper.GetDataBaseType, cmd,
                        DataBaseConnectionString, sqlCmd, _DbParameterCollection).CreateDataReader())
                    {
                        while (loReader.Read())
                        {
                            DunningLettersEn loItem = LoadObject1(loReader);
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

        #region GetListStudent

        /// <summary>
        /// Method to Get DunningLetters 
        /// </summary>
        /// <param name="argEn">DunningLetters Entity is an Input.Code and Title as Input Properties.</param>
        /// <returns>Returns List of DunningLetters.</returns>
        public List<DunningLettersEn> GetListStudent(DunningLettersEn argEn)
        {
            List<DunningLettersEn> loEnList = new List<DunningLettersEn>();
            string sqlCmd = "SELECT SAS_Accounts.CreditRef, SAS_Student.SASI_Name, SAS_Student.SASI_PgId, " +
                                        "SAS_Program.SAPG_Program, SAS_Student.SASI_Passport,  SAS_Student.SASI_Add1, SAS_Student.SASI_Add2," +
                                        "SAS_Student.SASI_Add3, SAS_Student.SASI_City, SAS_Student.SASS_Code,SAS_Accounts.transtype," +
                                        "CONVERT(VARCHAR(10),SAS_Accounts.TransDate,105) as Date1, SAS_Accounts.TransCode, " +
                                        "SAS_Accounts.Description, SAS_Accounts.Category,  SAS_Accounts.TransAmount, " +
                                        "SAS_Accounts.SubType , " +
                                        "(Select SUM(SAC.TransAmount) as DebitAmount from " +
                                        "SAS_Accounts SAC Where SAC.CreditRef = SAS_Accounts.CreditRef and SAC.TransType = 'Debit' And " +
                                        "SAC.poststatus = 'Posted') DebitAmount, " +
                                        "(Select SUM(SAC.TransAmount) as CreditAmount from " +
                                        "SAS_Accounts SAC Where SAC.CreditRef = SAS_Accounts.CreditRef and SAC.TransType = 'Credit' And " +
                                        "SAC.poststatus = 'Posted') CreditAmount " +
                                        "FROM SAS_Program, SAS_Accounts INNER JOIN  SAS_Student " +
                                        "ON SAS_Accounts.CreditRef = SAS_Student.SASI_MatricNo WHERE SAS_Program.SAPG_Code = " +
                                        "SAS_Student.SASI_PgId and SAS_Accounts.SubType = 'Student' and SAS_Accounts.PostStatus = 'Posted'  " +
                                        "and SAS_Student.SASI_StatusRec like '%' And (Select SUM(SAC.TransAmount) as DebitAmount from " +
                                        "SAS_Accounts SAC Where SAC.CreditRef = SAS_Accounts.CreditRef and SAC.TransType = 'Debit' And " +
                                        "SAC.poststatus = 'Posted') < (Select SUM(SAC.TransAmount) as CreditAmount from " +
                                        "SAS_Accounts SAC Where SAC.CreditRef = SAS_Accounts.CreditRef and SAC.TransType = 'Credit' And  " +
                                        "SAC.poststatus = 'Posted') Order By SAS_Student.SASI_PgId, SAS_Student.SASI_Name, " +
                                        "SAS_Accounts.TransDate ";

            try
            {
                if (!FormHelp.IsBlank(sqlCmd))
                {
                    using (IDataReader loReader = _DatabaseFactory.ExecuteReader(Helper.GetDataBaseType,
                        DataBaseConnectionString, sqlCmd).CreateDataReader())
                    {
                        while (loReader.Read())
                        {
                            DunningLettersEn loItem = LoadObject(loReader);
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
        /// Method to Get DunningLetters Item
        /// </summary>
        /// <param name="argEn">DunningLetters Entity is an Input.SADL_Code  as Input Property.</param>
        /// <returns>Returns a DunningLetters Item</returns>
        public DunningLettersEn GetItem(DunningLettersEn argEn)
        {
            DunningLettersEn loItem = new DunningLettersEn();
            string sqlCmd = "Select * FROM SAS_DunningLetters WHERE SADL_Code = @SADL_Code";

            try
            {
                if (!FormHelp.IsBlank(sqlCmd))
                {
                    DbCommand cmd = _DatabaseFactory.GetDbCommand(Helper.GetDataBaseType, sqlCmd, DataBaseConnectionString);
                    _DatabaseFactory.AddInParameter(ref cmd, "@SADL_Code", DbType.String, argEn.Code);
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
        /// Method to Insert DunningLetters
        /// </summary>
        /// <param name="argEn">DunningLetters Entity is the Input.</param>
        /// <returns>Returns a Boolean</returns>
        public bool Insert(DunningLettersEn argEn)
        {
            bool lbRes = false;
            int iOut = 0;
            string sqlCmd = "Select count(*) as cnt From SAS_DunningLetters WHERE sadl_code = @code or sadl_title = @title";
            try
            {
                if (!FormHelp.IsBlank(sqlCmd))
                {
                    DbCommand cmdSel = _DatabaseFactory.GetDbCommand(Helper.GetDataBaseType, sqlCmd, DataBaseConnectionString);
                    _DatabaseFactory.AddInParameter(ref cmdSel, "@code", DbType.String, argEn.Code);
                    _DatabaseFactory.AddInParameter(ref cmdSel, "@title", DbType.String, argEn.Title);
                    _DbParameterCollection = cmdSel.Parameters;

                    using (IDataReader dr = _DatabaseFactory.GetIDataReader(Helper.GetDataBaseType, cmdSel,
                        DataBaseConnectionString, sqlCmd, _DbParameterCollection).CreateDataReader())
                    {
                        if (dr.Read())
                            iOut = clsGeneric.NullToInteger(dr["cnt"]);
                        if (iOut > 0)
                            throw new Exception("Record Already Exist");
                        dr.Close();
                    }
                    if (iOut == 0)
                    {
                        sqlCmd = "INSERT INTO sas_dunningletters(sadl_code, sadl_title, sadl_ref, sadl_message, sadl_signby, sadl_name,sadl_frdate, sadl_todate, sadl_updatedby, sadl_updatedtime)";
                        sqlCmd += "VALUES(@SADL_Code,@SADL_Title,@SADL_Ref,@SADL_Message,@SADL_SignBy,@SADL_Name,@SADL_FrDate,@SADL_ToDate,@SADL_UpdatedBy,@SADL_UpdatedTime)";

                        if (!FormHelp.IsBlank(sqlCmd))
                        {
                            DbCommand cmd = _DatabaseFactory.GetDbCommand(Helper.GetDataBaseType, sqlCmd, DataBaseConnectionString);
                            _DatabaseFactory.AddInParameter(ref cmd, "@SADL_Code", DbType.String, MaxGeneric.clsGeneric.NullToString(argEn.Code));
                            _DatabaseFactory.AddInParameter(ref cmd, "@SADL_Title", DbType.String, MaxGeneric.clsGeneric.NullToString(argEn.Title));
                            _DatabaseFactory.AddInParameter(ref cmd, "@SADL_Ref", DbType.String, MaxGeneric.clsGeneric.NullToString(argEn.Reference));
                            _DatabaseFactory.AddInParameter(ref cmd, "@SADL_Message", DbType.String, MaxGeneric.clsGeneric.NullToString(argEn.Message));
                            _DatabaseFactory.AddInParameter(ref cmd, "@SADL_SignBy", DbType.String, MaxGeneric.clsGeneric.NullToString(argEn.SignBy));
                            _DatabaseFactory.AddInParameter(ref cmd, "@SADL_Name", DbType.String, MaxGeneric.clsGeneric.NullToString(argEn.Name));
                            _DatabaseFactory.AddInParameter(ref cmd, "@SADL_FrDate", DbType.DateTime, Helper.DateConversion(argEn.FromDate));
                            _DatabaseFactory.AddInParameter(ref cmd, "@SADL_ToDate", DbType.DateTime, Helper.DateConversion(argEn.ToDate));
                            _DatabaseFactory.AddInParameter(ref cmd, "@SADL_UpdatedBy", DbType.String, MaxGeneric.clsGeneric.NullToString(argEn.UpdatedBy));
                            _DatabaseFactory.AddInParameter(ref cmd, "@SADL_UpdatedTime", DbType.DateTime, Helper.DateConversion(argEn.UpdatedTime));
                            _DbParameterCollection = cmd.Parameters;

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
            catch (Exception ex)
            {
                throw ex;
            }
            return lbRes;
        }

        #endregion

        #region InsertDunning

        /// <summary>
        /// Method to Insert DunningLetters
        /// </summary>
        /// <param name="argEn">DunningLetters Entity is the Input.</param>
        /// <returns>Returns a Boolean</returns>
        public bool InsertDunning(DunningLettersEn argEn)
        {
            bool lbRes = false;
            int iOut = 0;
            string sqlCmd = "Select count(*) as cnt From SAS_DunningLetterWarning WHERE SAS_MatricNo = @SAS_MatricNo And SADL_Semester = @SADL_Semester";
            try
            {
                if (!FormHelp.IsBlank(sqlCmd))
                {
                    DbCommand cmdSel = _DatabaseFactory.GetDbCommand(Helper.GetDataBaseType, sqlCmd, DataBaseConnectionString);
                    _DatabaseFactory.AddInParameter(ref cmdSel, "@SAS_MatricNo", DbType.String, argEn.MatricNo);
                    _DatabaseFactory.AddInParameter(ref cmdSel, "@SADL_Semester", DbType.String, argEn.Semester);
                    _DbParameterCollection = cmdSel.Parameters;

                    using (IDataReader dr = _DatabaseFactory.GetIDataReader(Helper.GetDataBaseType, cmdSel,
                        DataBaseConnectionString, sqlCmd, _DbParameterCollection).CreateDataReader())
                    {
                        if (dr.Read())
                            iOut = clsGeneric.NullToInteger(dr["cnt"]);
                        if (iOut > 0)
                        {
                            sqlCmd = "Update SAS_DunningLetterWarning Set SADL_Code = @SADL_Code,SADL_Warn = (@SADL_Warn), " +
                            "sadl_insertby = @sadl_insertby,sadl_insertdate  = @sadl_insertdate  " +
                            "where SAS_MatricNo = @SADL_MatricNo and SADL_Semester = @SADL_Semester";

                            if (!FormHelp.IsBlank(sqlCmd))
                            {
                                DbCommand cmd = _DatabaseFactory.GetDbCommand(Helper.GetDataBaseType, sqlCmd, DataBaseConnectionString);
                                _DatabaseFactory.AddInParameter(ref cmd, "@SADL_MatricNo", DbType.String, argEn.MatricNo);
                                _DatabaseFactory.AddInParameter(ref cmd, "@SADL_Warn", DbType.String, argEn.Warning);
                                _DatabaseFactory.AddInParameter(ref cmd, "@SADL_Code", DbType.String, argEn.Code);
                                _DatabaseFactory.AddInParameter(ref cmd, "@SADL_Semester", DbType.String, argEn.Semester);
                                _DatabaseFactory.AddInParameter(ref cmd, "@SADL_insertby", DbType.String, argEn.InsertBy);
                                _DatabaseFactory.AddInParameter(ref cmd, "@SADL_insertdate", DbType.DateTime, Helper.DateConversion(argEn.InsertDate));
                                _DbParameterCollection = cmd.Parameters;

                                int liRowAffected = _DatabaseFactory.ExecuteNonQuery(Helper.GetDataBaseType, cmd,
                                    DataBaseConnectionString, sqlCmd, _DbParameterCollection);

                                if (liRowAffected > -1)
                                    lbRes = true;

                                else
                                    throw new Exception("Updating Failed! No Row has been updated...");
                                //return false;
                            }
                        }
                        else if (iOut == 0)
                        {
                            sqlCmd = "INSERT INTO sas_dunningletterwarning(sas_matricno, sadl_warn, sadl_code, sadl_semester, sadl_insertby,sadl_insertdate)";
                            sqlCmd += " VALUES (@SAS_MatricNo,@SADL_Warn,@SADL_Code,@SADL_Semester,@SADL_InsertBy,@SADL_InsertDate)";

                            if (!FormHelp.IsBlank(sqlCmd))
                            {
                                DbCommand cmd = _DatabaseFactory.GetDbCommand(Helper.GetDataBaseType, sqlCmd, DataBaseConnectionString);
                                _DatabaseFactory.AddInParameter(ref cmd, "@SAS_MatricNo", DbType.String, MaxGeneric.clsGeneric.NullToString(argEn.MatricNo));
                                _DatabaseFactory.AddInParameter(ref cmd, "@SADL_Warn", DbType.String, MaxGeneric.clsGeneric.NullToString(argEn.Warning));
                                _DatabaseFactory.AddInParameter(ref cmd, "@SADL_Code", DbType.String, MaxGeneric.clsGeneric.NullToString(argEn.Code));
                                _DatabaseFactory.AddInParameter(ref cmd, "@SADL_Semester", DbType.String, MaxGeneric.clsGeneric.NullToString(argEn.Semester));
                                _DatabaseFactory.AddInParameter(ref cmd, "@SADL_InsertBy", DbType.String, MaxGeneric.clsGeneric.NullToString(argEn.UpdatedBy));
                                _DatabaseFactory.AddInParameter(ref cmd, "@SADL_InsertDate", DbType.DateTime, Helper.DateConversion(argEn.InsertDate));
                                _DbParameterCollection = cmd.Parameters;

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
            return lbRes;
        }

        #endregion

        #region Update

        /// <summary>
        /// Method to Update DunningLetters
        /// </summary>
        /// <param name="argEn">DunningLetters Entity is the Input.</param>
        /// <returns>Returns a Boolean</returns>
        public bool Update(DunningLettersEn argEn)
        {
            bool lbRes = false;
            int iOut = 0;
            string sqlCmd = "Select count(*) as cnt From SAS_DunningLetters WHERE sadl_code != @code and sadl_title = @title";
            try
            {
                if (!FormHelp.IsBlank(sqlCmd))
                {
                    DbCommand cmdSel = _DatabaseFactory.GetDbCommand(Helper.GetDataBaseType, sqlCmd, DataBaseConnectionString);
                    _DatabaseFactory.AddInParameter(ref cmdSel, "@code", DbType.String, argEn.Code);
                    _DatabaseFactory.AddInParameter(ref cmdSel, "@title", DbType.String, argEn.Title);
                    _DbParameterCollection = cmdSel.Parameters;

                    using (IDataReader dr = _DatabaseFactory.GetIDataReader(Helper.GetDataBaseType, cmdSel,
                       DataBaseConnectionString, sqlCmd, _DbParameterCollection).CreateDataReader())
                    {
                        if (dr.Read())
                            iOut = clsGeneric.NullToInteger(dr["cnt"]);
                        if (iOut > 0)
                            throw new Exception("Record Already Exist");
                    }
                    if (iOut == 0)
                    {
                        sqlCmd = "UPDATE SAS_DunningLetters SET SADL_Code = @SADL_Code, SADL_Title = @SADL_Title, SADL_Ref = @SADL_Ref, SADL_Message = @SADL_Message, SADL_SignBy = @SADL_SignBy, SADL_Name = @SADL_Name, SADL_FrDate = @SADL_FrDate, SADL_ToDate = @SADL_ToDate, SADL_UpdatedBy = @SADL_UpdatedBy, SADL_UpdatedTime = @SADL_UpdatedTime WHERE SADL_Code = @SADL_Code";

                        if (!FormHelp.IsBlank(sqlCmd))
                        {
                            DbCommand cmd = _DatabaseFactory.GetDbCommand(Helper.GetDataBaseType, sqlCmd, DataBaseConnectionString);
                            _DatabaseFactory.AddInParameter(ref cmd, "@SADL_Code", DbType.String, argEn.Code);
                            _DatabaseFactory.AddInParameter(ref cmd, "@SADL_Title", DbType.String, argEn.Title);
                            _DatabaseFactory.AddInParameter(ref cmd, "@SADL_Ref", DbType.String, argEn.Reference);
                            _DatabaseFactory.AddInParameter(ref cmd, "@SADL_Message", DbType.String, argEn.Message);
                            _DatabaseFactory.AddInParameter(ref cmd, "@SADL_SignBy", DbType.String, argEn.SignBy);
                            _DatabaseFactory.AddInParameter(ref cmd, "@SADL_Name", DbType.String, argEn.Name);
                            _DatabaseFactory.AddInParameter(ref cmd, "@SADL_FrDate", DbType.DateTime, Helper.DateConversion(argEn.FromDate));
                            _DatabaseFactory.AddInParameter(ref cmd, "@SADL_ToDate", DbType.DateTime, Helper.DateConversion(argEn.ToDate));
                            _DatabaseFactory.AddInParameter(ref cmd, "@SADL_UpdatedBy", DbType.String, argEn.UpdatedBy);
                            _DatabaseFactory.AddInParameter(ref cmd, "@SADL_UpdatedTime", DbType.DateTime, Helper.DateConversion( argEn.UpdatedTime));
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
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return lbRes;
        }

        #endregion

        #region Delete

        /// <summary>
        ///  Method to Delete  DunningLetters 
        /// </summary>
        /// <param name="argEn">DunningLetters Entity is an Input.SADL_Code is Property.</param>
        /// <returns>Returns a Boolean</returns>
        public bool Delete(DunningLettersEn argEn)
        {
            bool lbRes = false;
            string sqlCmd = "DELETE FROM SAS_DunningLetters WHERE SADL_Code = @SADL_Code";

            try
            {
                if (!FormHelp.IsBlank(sqlCmd))
                {
                    DbCommand cmd = _DatabaseFactory.GetDbCommand(Helper.GetDataBaseType, sqlCmd, DataBaseConnectionString);
                    _DatabaseFactory.AddInParameter(ref cmd, "@SADL_Code", DbType.String, argEn.Code);
                    _DbParameterCollection = cmd.Parameters;

                    int liRowAffected = _DatabaseFactory.ExecuteNonQuery(Helper.GetDataBaseType, cmd,
                                DataBaseConnectionString, sqlCmd, _DbParameterCollection);

                    if (liRowAffected > -1)
                        lbRes = true;
                    else
                        throw new Exception("Deletion Failed! No Row has been Updated...");
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
        ///  Method to Load DunningLetters Entity
        /// </summary>
        /// <param name="argReader">Reader Object is an Input.</param>
        /// <returns>Returns DunningLetters Entity Object.</returns>
        private DunningLettersEn LoadObject(IDataReader argReader)
        {
            DunningLettersEn loItem = new DunningLettersEn();
            loItem.Code = GetValue<string>(argReader, "SADL_Code");
            loItem.Title = GetValue<string>(argReader, "SADL_Title");
            loItem.Reference = GetValue<string>(argReader, "SADL_Ref");
            loItem.Message = GetValue<string>(argReader, "SADL_Message");
            loItem.SignBy = GetValue<string>(argReader, "SADL_SignBy");
            loItem.Name = GetValue<string>(argReader, "SADL_Name");
            loItem.FromDate = GetValue<DateTime>(argReader, "SADL_FrDate");
            loItem.ToDate = GetValue<DateTime>(argReader, "SADL_ToDate");
            loItem.UpdatedBy = GetValue<string>(argReader, "SADL_UpdatedBy");
            loItem.UpdatedTime = GetValue<DateTime>(argReader, "SADL_UpdatedTime");

            return loItem;
        }
        /// <summary>
        ///  Method to Load DunningLetters Entity
        /// </summary>
        /// <param name="argReader">Reader Object is an Input.</param>
        /// <returns>Returns DunningLetters Entity Object.</returns>
        private DunningLettersEn LoadObject1(IDataReader argReader)
        {
            DunningLettersEn loItem = new DunningLettersEn();
            loItem.Code = GetValue<string>(argReader, "SADL_Code");
            return loItem;
        }
        /// <summary>
        ///  Method to Load DunningLetters
        /// </summary>
        /// <param name="argReader">Reader Object is an Input.</param>
        /// <returns>Returns DunningLetters Entity Object.</returns>
        private DunningLettersEn LoadObject2(IDataReader argReader)
        {
            DunningLettersEn loItem = new DunningLettersEn();
            loItem.MatricNo = GetValue<string>(argReader, "SASI_MatricNo");
            loItem.Semester = GetValue<string>(argReader, "SASI_CurSemYr");
            loItem.Code = GetValue<string>(argReader, "SADL_Code");
            loItem.Warning = GetValue<string>(argReader, "SADL_Warn");
            //loItem.InsertBy = Session("User");//argEn.UpdatedBy
            loItem.InsertDate = DateTime.Now.Date;
            return loItem;
        }
        /// <summary>
        ///  Method to Load DunningLetters
        /// </summary>
        /// <param name="argReader">Reader Object is an Input.</param>
        /// <returns>Returns DunningLetters Entity Object.</returns>
        private DunningLettersEn LoadObject4(IDataReader argReader)
        {
            DunningLettersEn loItem = new DunningLettersEn();
            loItem.MatricNo = GetValue<string>(argReader, "SASI_MatricNo");
            loItem.Name = GetValue<string>(argReader, "SASI_Name");
            loItem.ICNo = GetValue<String>(argReader, "SASI_ICNo");
            loItem.pgID = GetValue<String>(argReader, "SASI_PgID");
            loItem.Semester = GetValue<string>(argReader, "SASI_CurSemYr");
            loItem.Code = GetValue<string>(argReader, "SADL_Code");
            loItem.Warning = GetValue<string>(argReader, "SADL_Warn");
            return loItem;
        }
        /// <summary>
        ///  Method to Load DunningLetters
        /// </summary>
        /// <param name="argReader">Reader Object is an Input.</param>
        /// <returns>Returns DunningLetters Entity Object.</returns>
        private DunningLettersEn LoadObject3(IDataReader argReader)
        {
            DunningLettersEn loItem = new DunningLettersEn();
            loItem.MatricNo = GetValue<string>(argReader, "SAS_MatricNo");
            loItem.Semester = GetValue<string>(argReader, "SADL_Semester");
            loItem.Code = GetValue<string>(argReader, "SADL_Code");
            loItem.Warning = GetValue<string>(argReader, "SADL_Warn");
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

        #region Get Bank Details

        public DataSet BankDetails(string bankcode)
        {
            //variable declarations
            string SqlStatement = null;

            try
            {
                //Build sql statment - Start

                if (bankcode != "")
                {
                    SqlStatement += "select  * from sas_bankdetails where sabd_code='" + bankcode + "'";
                }
                else
                {
                    SqlStatement += "select * from sas_bankdetails";
                }
                //Build sql statment - Stop

                if (!FormHelp.IsBlank(SqlStatement))
                {
                    //Binding Sas Account Details - start
                    DataSet _DataSet = _DatabaseFactory.ExecuteDataSet(Helper.GetDataBaseType,
                        DataBaseConnectionString, SqlStatement);
                    //Sas Account status - Ended
                    return _DataSet;
                }

            }
            catch (Exception ex)
            {
                MaxModule.Helper.LogError(ex.Message);

            }
            return null;
        }
        #endregion

        #region GetCountDunning

        /// <summary>
        /// Method to Get DunningLetters 
        /// </summary>
        /// <param name="MatricNo,Semester">MatricNo,Semester are the Input.</param>
        /// <returns>Returns List of DunningLetters.</returns>
        public string GetCountDunning(string MatricNo, string Semester)
        {
            string Warning = null, sqlCmd = null;

            try
            {
                //build sql statement - Start
                sqlCmd = "SELECT case WHEN SAS_dunningletterwarning.SADL_Warn is null Then '1' " + 
                            "when SAS_dunningletterwarning.SADL_Warn = '0' then '1' " +
                            "when SAS_dunningletterwarning.SADL_Warn = '1' then '2' " +
                            "when SAS_dunningletterwarning.SADL_Warn = '2' then '3' " +
                            "else 'overlimit' end SADL_Warn " +
                            "FROM SAS_dunningletterwarning " +
                            "WHERE SAS_dunningletterwarning.SAS_MatricNo = " + MaxGeneric.clsGeneric.AddQuotes(MatricNo) + " and sadl_semester = " + MaxGeneric.clsGeneric.AddQuotes(Semester);                    
                //build sql statement - Stop                

                if (!FormHelp.IsBlank(sqlCmd))
                {
                    Warning = clsGeneric.NullToString(_DatabaseFactory.ExecuteScalar(
                        Helper.GetDataBaseType, DataBaseConnectionString, sqlCmd));

                    if (Warning == "")
                        Warning = "1";
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Warning;
        }

        #endregion

        #region GetState

        public string GetState(string StateCode)
        {
            //variable declarations
            string State = "";            

            try
            {
                //build sql statement - Start
                string sqlCmd = "select state_desc FROM sas_state WHERE state_code = " + MaxGeneric.clsGeneric.AddQuotes(StateCode);
                //build sql statement - Stop                

                if (!FormHelp.IsBlank(sqlCmd))
                {
                    IDataReader loReader = _DatabaseFactory.ExecuteReader(Helper.GetDataBaseType,
                        DataBaseConnectionString, sqlCmd).CreateDataReader();

                    if (loReader.Read())
                    {
                        State = GetValue<string>(loReader, "state_desc");     
                    }
                    loReader.Close();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return State;
        }

        #endregion

    }

}



