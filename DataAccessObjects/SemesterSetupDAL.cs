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
    /// Class to handle all the SemesterSetup Methods.
    /// </summary>
    public class SemesterSetupDAL
    {
        #region Global Declarations 

        private DbParameterCollection _DbParameterCollection = null;

        private MaxModule.DatabaseProvider _DatabaseFactory =
            new MaxModule.DatabaseProvider();

        private string DataBaseConnectionString = Helper.GetConnectionString();

        #endregion

        public SemesterSetupDAL()
        {
        }

        #region Get Session List 

        /// <summary>
        /// Method to Get List of Semesters
        /// </summary>
        /// <param name="argEn">SemisterSetupCode as Input.</param>
        /// <returns>Returns List of SemesterSetup</returns>
        public List<SemesterSetupEn> GetSessionList(string argEn)
        {
            //create instances
            List<SemesterSetupEn> SessionList = new List<SemesterSetupEn>();

            //variable declarations
            string SqlStatement = null;

            try
            {
                //build sql statement - Start
                SqlStatement = "SELECT DISTINCT * FROM SAS_SemesterSetup WHERE SAST_Semester = ";
                SqlStatement += clsGeneric.AddQuotes(argEn);
                //build sql statement - Stop

                //Get User Login Details - Start
                IDataReader _IDataReader = _DatabaseFactory.ExecuteReader(Helper.GetDataBaseType,
                        DataBaseConnectionString, SqlStatement).CreateDataReader();
                //Get User Login Details - Stop

                 //if details available - Start
                if (_IDataReader != null)
                {
                    while (_IDataReader.Read())
                    {
                        SemesterSetupEn _SemesterSetupEn = LoadObject(_IDataReader);
                        SessionList.Add(_SemesterSetupEn);
                    }

                    _IDataReader.Close();
                    _IDataReader.Dispose();

                    return SessionList;
                }
                //if details available - Stop

                return null;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region Get List Semester Current 

        /// <summary>
        /// Method to Get List of SemesterSetup
        /// </summary>
        /// <param name="argEn">SemesterSetup Entity as an Input.</param>
        /// <returns>Returns List of SemesterSetup</returns>
        public List<SemesterSetupEn> GetListSemesterCur(SemesterSetupEn argEn)
        {
            //create instances
            List<SemesterSetupEn> SemesterList = new List<SemesterSetupEn>();

            //variable declarations
            string SqlStatement = null;

            try
            {
                //Build Sqlstatement - Start
                //SqlStatement = "SELECT Distinct SAST_Code,sast_code2,sast_semester From SAS_SemesterSetup WHERE SAST_Status = true AND sast_iscurrentsem = true";    
                SqlStatement = "SELECT Distinct SAST_Code,sast_code2,sast_semester From SAS_SemesterSetup WHERE SAST_Status = true";                
                SqlStatement += " ORDER BY SAST_Code";
                //Build Sqlstatement - Stop

                //Get User Login Details - Start
                IDataReader _IDataReader = _DatabaseFactory.ExecuteReader(Helper.GetDataBaseType,
                        DataBaseConnectionString, SqlStatement).CreateDataReader();
                //Get User Login Details - Stop

                //if details available - Start
                if (_IDataReader != null)
                {
                    while (_IDataReader.Read())
                    {
                        SemesterSetupEn _SemesterSetupEn = new SemesterSetupEn();
                        _SemesterSetupEn.Semester = GetValue<string>(_IDataReader, "sast_semester");
                        _SemesterSetupEn.SemisterSetupCode = GetValue<string>(_IDataReader, "SAST_Code");
                        _SemesterSetupEn.SemisterCode2 = GetValue<string>(_IDataReader, "sast_code2");
                        SemesterList.Add(_SemesterSetupEn);
                    }

                    _IDataReader.Close();
                    _IDataReader.Dispose();

                    return SemesterList;
                }
                //if details available - Stop

                return null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region Get List 

        /// <summary>
        /// Method to Get List of SemesterSetup
        /// </summary>
        /// <param name="argEn">SemesterSetup Entity as an Input.</param>
        /// <returns>Returns List of SemesterSetup</returns>
        public List<SemesterSetupEn> GetList(SemesterSetupEn argEn)
        {
            //create instances
            List<SemesterSetupEn> SemesterList = new List<SemesterSetupEn>();

            //variable declarations
            string SqlStatement = null;

            try
            {
                //build sql statement - Start
                SqlStatement = "Select Distinct SAST_Semester From SAS_SemesterSetup ";
                SqlStatement += " order by SAST_Semester";
                //build sql statement - Stop

                //Get User Login Details - Start
                IDataReader _IDataReader = _DatabaseFactory.ExecuteReader(Helper.GetDataBaseType,
                        DataBaseConnectionString, SqlStatement).CreateDataReader();
                //Get User Login Details - Stop

                //if details available - Start
                if (_IDataReader != null)
                {
                    while (_IDataReader.Read())
                    {
                        SemesterSetupEn _SemesterSetupEn = new SemesterSetupEn();
                        _SemesterSetupEn.Semester = GetValue<string>(_IDataReader, "SAST_Semester");
                        SemesterList.Add(_SemesterSetupEn);
                    }

                    _IDataReader.Close();
                    _IDataReader.Dispose();

                    return SemesterList;
                }
                //if details available - Stop

                return null;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region Get List Semester Code 

        /// <summary>
        /// Method to Get List of SemesterSetup
        /// </summary>
        /// <param name="argEn">SemesterSetup Entity as an Input.</param>
        /// <returns>Returns List of SemesterSetup</returns>
        public List<SemesterSetupEn> GetListSemesterCode(SemesterSetupEn argEn)
        {
            //create instances
            List<SemesterSetupEn> SemesterList = new List<SemesterSetupEn>();

            //variable declarations
            string SqlStatement = null;

            try
            {
                //Build Sql Statement - Start
                SqlStatement = "Select Distinct * From SAS_SemesterSetup ";
                SqlStatement += " order by SAST_Description, SAST_Code";
                //Build Sql Statement - Stop

                //Get User Login Details - Start
                IDataReader _IDataReader = _DatabaseFactory.ExecuteReader(Helper.GetDataBaseType,
                        DataBaseConnectionString, SqlStatement).CreateDataReader();
                //Get User Login Details - Stop

                //if details available - Start
                if (_IDataReader != null)
                {
                    while (_IDataReader.Read())
                    {
                        SemesterSetupEn _SemesterSetupEn = LoadObject(_IDataReader);
                        SemesterList.Add(_SemesterSetupEn);
                    }

                    _IDataReader.Close();
                    _IDataReader.Dispose();

                    return SemesterList;
                }
                //if details available - Stop

                return null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region Get Semester Setup List 

        /// <summary>
        /// Method to Get List of Active or Inactive Semesters
        /// </summary>
        /// <param name="argEn">SemesterSetup Entity as an Input.SemesterSetupCode,Description and Status as Input Properties.</param>
        /// <returns>Returns List of SemesterSetup</returns>
        public List<SemesterSetupEn> GetSemesterSetupList(SemesterSetupEn argEn)
        {
            //create instances
            List<SemesterSetupEn> SemseterSetupList = new List<SemesterSetupEn>();

            //variable declarations
            string SqlStatement = null;

            try
            {
                //build Sqlstatement - Start
                argEn.SemisterSetupCode = argEn.SemisterSetupCode.Replace("*", "%");
                //argEn.Description = argEn.Description.Replace("*", "%");
                argEn.SemisterCode2 = argEn.SemisterCode2.Replace("*", "%");
                SqlStatement = "select distinct * from SAS_SemesterSetup where SAST_Code <> '0'";
                if (argEn.SemisterSetupCode.Length != 0) SqlStatement = SqlStatement + " and SAST_Code like " + clsGeneric.AddQuotes(argEn.SemisterSetupCode);
                //if (argEn.Description.Length != 0) SqlStatement = SqlStatement + " and SAST_Description like " + clsGeneric.AddQuotes(argEn.Description);
                if (argEn.SemisterCode2.Length != 0) SqlStatement = SqlStatement + " and sast_code2 like " + clsGeneric.AddQuotes(argEn.SemisterCode2);
                if (argEn.Status == true) SqlStatement = SqlStatement + " and SAST_Status = true";
                if (argEn.Status == false) SqlStatement = SqlStatement + " and SAST_Status = false";
                if (argEn.CurrSem == true) SqlStatement = SqlStatement + " and sast_iscurrentsem = true";
                if (argEn.CurrSem == false) SqlStatement = SqlStatement + "  and sast_iscurrentsem = false";
                SqlStatement = SqlStatement + " order by SAST_Code";
                //build Sqlstatement - Stop

                //Get User Login Details - Start
                IDataReader _IDataReader = _DatabaseFactory.ExecuteReader(Helper.GetDataBaseType,
                        DataBaseConnectionString, SqlStatement).CreateDataReader();
                //Get User Login Details - Stop

                //if details available - Start
                if (_IDataReader != null)
                {
                    while (_IDataReader.Read())
                    {
                        SemesterSetupEn _SemesterSetupEn = LoadObject(_IDataReader);
                        SemseterSetupList.Add(_SemesterSetupEn);
                    }

                    _IDataReader.Close();
                    _IDataReader.Dispose();

                    return SemseterSetupList;
                }
                //if details available - Stop

                return null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region Get Semester Setup List All 

        /// <summary>
        /// Method to Get List of All Semesters
        /// </summary>
        /// <param name="argEn">SemesterSetup Entity as an Input.SemesterSetupCode and Description as Input Properties.</param>
        /// <returns>Returns List of SemesterSetup</returns>
        public List<SemesterSetupEn> GetSemesterSetupListAll(SemesterSetupEn argEn)
        {
            //create instances
            List<SemesterSetupEn> SemseterSetupList = new List<SemesterSetupEn>();

            //variable declarations
            string SqlStatement = null;

            try
            {
                //build Sqlstatement - Start
                argEn.SemisterSetupCode = argEn.SemisterSetupCode.Replace("*", "%");
                argEn.Description = argEn.Description.Replace("*", "%");
                SqlStatement = "select distinct * from SAS_SemesterSetup where SAST_Code <> '0'";
                if (argEn.SemisterSetupCode.Length != 0) SqlStatement = SqlStatement + " and SAST_Code like '" + clsGeneric.AddQuotes(argEn.SemisterSetupCode);
                if (argEn.Description.Length != 0) SqlStatement = SqlStatement + " and SAST_Description like '" + clsGeneric.AddQuotes(argEn.Description);
                SqlStatement = SqlStatement + " order by SAST_Code";
                //build Sqlstatement - Stop

                //Get User Login Details - Start
                IDataReader _IDataReader = _DatabaseFactory.ExecuteReader(Helper.GetDataBaseType,
                        DataBaseConnectionString, SqlStatement).CreateDataReader();
                //Get User Login Details - Stop

                //if details available - Start
                if (_IDataReader != null)
                {
                    while (_IDataReader.Read())
                    {
                        SemesterSetupEn _SemesterSetupEn = LoadObject(_IDataReader);
                        SemseterSetupList.Add(_SemesterSetupEn);
                    }

                    _IDataReader.Close();
                    _IDataReader.Dispose();

                    return SemseterSetupList;
                }
                //if details available - Stop

                return null;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region Get Item 

        /// <summary>
        /// Method to Get SemesterSetup Entity
        /// </summary>
        /// <param name="argEn">SemesterSetup Entity is an Input.Semester and Description as Input Property.</param>
        /// <returns>Returns SemesterSetup Entity</returns>
        public SemesterSetupEn GetItem(SemesterSetupEn argEn)
        {
            //create instances
            SemesterSetupEn _SemesterSetupEn = new SemesterSetupEn();

            //variable declarations
            string SqlStatement = null;

            try
            {
                //Build Sqlstatement - Start
                SqlStatement = "Select * FROM SAS_SemesterSetup WHERE SAST_Semester = ";
                SqlStatement += clsGeneric.AddQuotes(argEn.Semester) + " AND SAST_Description = ";
                SqlStatement += clsGeneric.AddQuotes(argEn.Description);
                //Build Sqlstatement - Stop

                //Get User Login Details - Start
                IDataReader _IDataReader = _DatabaseFactory.ExecuteReader(Helper.GetDataBaseType,
                        DataBaseConnectionString, SqlStatement).CreateDataReader();
                //Get User Login Details - Stop

                //if details available - Start
                if (_IDataReader != null)
                {
                    if (_IDataReader.Read())
                    {
                       _SemesterSetupEn = LoadObject(_IDataReader);
                    }

                    _IDataReader.Close();
                    _IDataReader.Dispose();

                    return _SemesterSetupEn;
                }
                //if details available - Stop

                return null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion        

        #region Get Session Item 

        /// <summary>
        /// Method to Get SemesterSetup Entity
        /// </summary>
        /// <param name="argEn">SemesterSetup Entity is an Input.SAST_Code as Input Property.</param>
        /// <returns>Returns SemesterSetup Entity</returns>
        public SemesterSetupEn GetSessionItem(string argEn)
        {
            //create instances
            SemesterSetupEn _SemesterSetupEn = new SemesterSetupEn();
            
            //variable declarations
            string SqlStatement = null;

            try
            {
                //Build Sqlstatement - Start
                SqlStatement = "Select * FROM SAS_SemesterSetup WHERE SAST_Code = ";
                SqlStatement += clsGeneric.AddQuotes(argEn);
                //Build Sqlstatement - Stop

                //Get User Login Details - Start
                IDataReader _IDataReader = _DatabaseFactory.ExecuteReader(Helper.GetDataBaseType,
                        DataBaseConnectionString, SqlStatement).CreateDataReader();
                //Get User Login Details - Stop

                //if details available - Start
                if (_IDataReader != null)
                {
                    while (_IDataReader.Read())
                    {
                        _SemesterSetupEn = LoadObject(_IDataReader);
                    }

                    _IDataReader.Close();
                    _IDataReader.Dispose();

                    return _SemesterSetupEn;
                }
                //if details available - Stop

                return null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region Get Item By Session 

        /// <summary>
        /// Method to Get SemesterSetup Entity
        /// </summary>
        /// <param name="argEn">SemesterSetup Entity is an Input.SAST_Code as Input Property.</param>
        /// <returns>Returns SemesterSetup Entity</returns>
        public SemesterSetupEn GetItemBySession(SemesterSetupEn argEn)
        {
            //create instances
            SemesterSetupEn _SemesterSetupEn = new SemesterSetupEn();

            //variable declarations
            string SqlStatement = null;

            try
            {
                //Build Sqlstatement - Start
                SqlStatement = "Select * FROM SAS_SemesterSetup WHERE SAST_Code = ";
                SqlStatement += clsGeneric.AddQuotes(argEn.SemisterSetupCode);
                //Build Sqlstatement - Stop

                //Get User Login Details - Start
                IDataReader _IDataReader = _DatabaseFactory.ExecuteReader(Helper.GetDataBaseType,
                        DataBaseConnectionString, SqlStatement).CreateDataReader();
                //Get User Login Details - Stop

                //if details available - Start
                if (_IDataReader != null)
                {
                    while (_IDataReader.Read())
                    {
                        _SemesterSetupEn = LoadObject(_IDataReader);
                    }

                    _IDataReader.Close();
                    _IDataReader.Dispose();

                    return _SemesterSetupEn;
                }
                //if details available - Stop

                return null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region Insert 

        /// <summary>
        /// Method to Insert SemesterSetup 
        /// </summary>
        /// <param name="argEn">SemesterSetup Entity is an Input.</param>
        /// <returns>Returns Boolean</returns>
        public bool Insert(SemesterSetupEn argEn)
        {
            //variable declarations - Start
            bool Result = false; string Boolvalue = "false";
            //string SqlStatement = null; int RecordsSaved = 0;
            int iOut = 0; int ResetCurrSem = 0;
            //variable declarations - Stop

            //string sqlCmd = "SELECT count(*) AS cnt FROM SAS_SemesterSetup WHERE (SAST_Code = @sast_code) OR " +
            //                "(SAST_Semester = @sast_semester AND SAST_Description = @sast_desc)";
            //string sqlCmd = "Select count(*) as cnt From SAS_SemesterSetup WHERE SAST_Code = @sast_code AND sast_code2 = @sast_code2 AND SAST_Semester = @sast_semester " +
            //                    "AND sast_status = @sast_status AND sast_iscurrentsem = @sast_currsem";
            string sqlCmd = "SELECT count(*) AS cnt FROM SAS_SemesterSetup WHERE (SAST_Code = @sast_code)";

            try
            {
                if (!FormHelp.IsBlank(sqlCmd))
                {
                    DbCommand cmdSel = _DatabaseFactory.GetDbCommand(Helper.GetDataBaseType, sqlCmd, DataBaseConnectionString);
                    _DatabaseFactory.AddInParameter(ref cmdSel, "@sast_code", DbType.String, argEn.SemisterSetupCode);
                    _DatabaseFactory.AddInParameter(ref cmdSel, "@sast_semester", DbType.String, argEn.Semester);
                    _DatabaseFactory.AddInParameter(ref cmdSel, "@sast_code2", DbType.String, argEn.SemisterCode2);                    
                    _DatabaseFactory.AddInParameter(ref cmdSel, "@sast_status", DbType.Boolean, argEn.Status);
                    _DatabaseFactory.AddInParameter(ref cmdSel, "@sast_currsem", DbType.Boolean, argEn.CurrSem);
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
                        if (argEn.Status)
                        { Boolvalue = "true"; }

                        //sqlCmd = "INSERT INTO SAS_SemesterSetup (SAST_Code,SAST_Semester,SAST_Description,SAST_Status,SABR_Code," +
                        //            "SAST_UpdatedUser,SAST_UpdatedDtTm,SAST_Code2,sast_iscurrentsem) " +
                        //            "VALUES (@sast_code,@sast_semester,@sast_desc,@sast_status,@sabr_code," +
                        //            "@sast_updatedby,@sast_updateddttm,@sast_code2,@sast_currsem) ";
                        sqlCmd = "INSERT INTO SAS_SemesterSetup (SAST_Code,SAST_Semester,SAST_Status,SABR_Code," +
                                   "SAST_UpdatedUser,SAST_UpdatedDtTm,SAST_Code2,sast_iscurrentsem) " +
                                   "VALUES (@sast_code,@sast_semester,@sast_status,@sabr_code," +
                                   "@sast_updatedby,@sast_updateddttm,@sast_code2,@sast_currsem) ";

                        if (!FormHelp.IsBlank(sqlCmd))
                        {
                            DbCommand cmd = _DatabaseFactory.GetDbCommand(Helper.GetDataBaseType, sqlCmd, DataBaseConnectionString);
                            _DatabaseFactory.AddInParameter(ref cmd, "@sast_code", DbType.String, argEn.SemisterSetupCode);
                            _DatabaseFactory.AddInParameter(ref cmd, "@sast_semester", DbType.String, argEn.Semester);
                            //_DatabaseFactory.AddInParameter(ref cmd, "@sast_desc", DbType.String, argEn.Description);
                            _DatabaseFactory.AddInParameter(ref cmd, "@sast_status", DbType.Boolean, Boolvalue);
                            _DatabaseFactory.AddInParameter(ref cmd, "@sabr_code", DbType.Int32, argEn.Code);
                            _DatabaseFactory.AddInParameter(ref cmd, "@sast_updatedby", DbType.String, argEn.UpdatedUser);
                            _DatabaseFactory.AddInParameter(ref cmd, "@sast_updateddttm", DbType.String, argEn.UpdatedDtTm);
                            _DatabaseFactory.AddInParameter(ref cmd, "@sast_code2", DbType.String, argEn.SemisterCode2);
                            _DatabaseFactory.AddInParameter(ref cmd, "@sast_currsem", DbType.Boolean, argEn.CurrSem);
                            _DbParameterCollection = cmd.Parameters;

                            int liRowAffected = _DatabaseFactory.ExecuteNonQuery(Helper.GetDataBaseType, cmd,
                                DataBaseConnectionString, sqlCmd, _DbParameterCollection);

                            if (liRowAffected > -1)
                            {     
                                Result = true;

                                if (argEn.CurrSem == true)
                                {
                                    //Reset currentsem = No for other other semester
                                    sqlCmd = "UPDATE  SAS_SemesterSetup SET sast_iscurrentsem = false " +
                                                "WHERE SAST_Code != " + clsGeneric.AddQuotes(argEn.SemisterSetupCode);                                   

                                    //Update Details to Database
                                    ResetCurrSem = _DatabaseFactory.ExecuteSqlStatement(Helper.GetDataBaseType,
                                        DataBaseConnectionString, sqlCmd);

                                    if (ResetCurrSem > -1)
                                        Result = true;
                                    else
                                        throw new Exception("Reset Current Semester Failed! No Row has been updated...");
                                }                                                                   
                            }
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

            return Result;
        }

        #endregion

        #region Update 

        /// <summary>
        /// Method to Update SemesterSetup 
        /// </summary>
        /// <param name="argEn">SemesterSetup Entity is an Input.</param>
        /// <returns>Returns Boolean</returns>
        public bool Update(SemesterSetupEn argEn)
        {
            //variable declarations - Start
            string Boolvalue = "false"; bool Result = false;
            //string SqlStatement = null; int RecordsSaved = 0;
            //variable declarations - Stop
            int iOut = 0; int ResetCurrSem = 0;
            string sqlCmd = "Select count(*) as cnt From SAS_SemesterSetup WHERE sast_code2 = @sast_code2 AND SAST_Semester = @sast_semester AND sast_status = @sast_status " +
                                "AND sast_iscurrentsem = @sast_currsem";

            try
            {
                if (!FormHelp.IsBlank(sqlCmd))
                {
                    DbCommand cmdSel = _DatabaseFactory.GetDbCommand(Helper.GetDataBaseType, sqlCmd, DataBaseConnectionString);
                    _DatabaseFactory.AddInParameter(ref cmdSel, "@sast_code2", DbType.String, argEn.SemisterCode2);
                    _DatabaseFactory.AddInParameter(ref cmdSel, "@sast_semester", DbType.String, argEn.Semester);
                    _DatabaseFactory.AddInParameter(ref cmdSel, "@sast_status", DbType.Boolean, argEn.Status);
                    _DatabaseFactory.AddInParameter(ref cmdSel, "@sast_currsem", DbType.Boolean, argEn.CurrSem);
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
                        if (argEn.Status)
                        { Boolvalue = "true"; }

                        sqlCmd = "UPDATE SAS_SemesterSetup SET SAST_Semester = @sast_semester, SAST_Status = @sast_status, " +
                                    "SAST_Code2 = @sast_code2, sast_iscurrentsem = @sast_currsem, " +
                                    "SAST_UpdatedUser = @sast_updatedby, SAST_UpdatedDtTm = @sast_updateddttm WHERE SAST_Code = @sast_code";

                        if (!FormHelp.IsBlank(sqlCmd))
                        {
                            DbCommand cmd = _DatabaseFactory.GetDbCommand(Helper.GetDataBaseType, sqlCmd, DataBaseConnectionString);
                            _DatabaseFactory.AddInParameter(ref cmd, "@sast_code", DbType.String, argEn.SemisterSetupCode);
                            _DatabaseFactory.AddInParameter(ref cmd, "@sast_semester", DbType.String, argEn.Semester);
                            _DatabaseFactory.AddInParameter(ref cmd, "@sast_status", DbType.Boolean, Boolvalue);
                            _DatabaseFactory.AddInParameter(ref cmd, "@sast_updatedby", DbType.String, argEn.UpdatedUser);
                            _DatabaseFactory.AddInParameter(ref cmd, "@sast_updateddttm", DbType.String, argEn.UpdatedDtTm);
                            _DatabaseFactory.AddInParameter(ref cmd, "@sast_code2", DbType.String, argEn.SemisterCode2);
                            _DatabaseFactory.AddInParameter(ref cmd, "@sast_currsem", DbType.Boolean, argEn.CurrSem);
                            _DbParameterCollection = cmd.Parameters;

                            int liRowAffected = _DatabaseFactory.ExecuteNonQuery(Helper.GetDataBaseType, cmd,
                                DataBaseConnectionString, sqlCmd, _DbParameterCollection);

                            if (liRowAffected > -1)
                                Result = true;

                            else
                                throw new Exception("Update Failed! No Row has been updated...");

                            if (argEn.CurrSem == true)
                            {
                                //Reset currentsem = No for other other semester
                                sqlCmd = "UPDATE  SAS_SemesterSetup SET sast_iscurrentsem = false " +
                                            "WHERE SAST_Code != " + clsGeneric.AddQuotes(argEn.SemisterSetupCode);

                                //Update Details to Database
                                ResetCurrSem = _DatabaseFactory.ExecuteSqlStatement(Helper.GetDataBaseType,
                                    DataBaseConnectionString, sqlCmd);

                                if (ResetCurrSem > -1)
                                    Result = true;
                                else
                                    throw new Exception("Reset Current Semester Failed! No Row has been updated...");
                            }

                        }
                    }
                }
            }

            catch (Exception ex)
            {
                throw ex;
            }

            return Result;
        }

        #endregion

        #region Delete 

        /// <summary>
        /// Method to Delete SemesterSetup 
        /// </summary>
        /// <param name="argEn">SemesterSetup Entity is an Input.SASR_Code as Input Property.</param>
        /// <returns>Returns Boolean</returns>
        public bool Delete(SemesterSetupEn argEn)
        {
            //variable declarations
            string SqlStatement = null; int HasRows = 0; int RecordsSaved = 0; bool Result = false;

            try
            {
                //Build Sql Statement - Start
                SqlStatement = "SELECT SUM(rows) as rows from (select count(*) as rows  from SAS_Student WHERE ";
                SqlStatement += "SASI_Intake = " + clsGeneric.AddQuotes(argEn.SemisterSetupCode) + " UNION ALL ";
                SqlStatement += " SELECT count(*) as rows from SAS_Student WHERE SASI_CurSemYr = ";
                SqlStatement += clsGeneric.AddQuotes(argEn.SemisterSetupCode) + ") AS U ";
                //Build Sql Statement - Stop

                //Get User Login Details - Start
                IDataReader _IDataReader = _DatabaseFactory.ExecuteReader(Helper.GetDataBaseType,
                        DataBaseConnectionString, SqlStatement).CreateDataReader();
                //Get User Login Details - Stop

                //if details available - Start
                if (_IDataReader != null)
                {
                    if (_IDataReader.Read())
                    {
                        HasRows = clsGeneric.NullToInteger(_IDataReader["rows"]);
                        if (HasRows > 0)
                            throw new Exception("Record Already in Use");
                    }

                    _IDataReader.Close();
                    _IDataReader.Dispose();

                    //if record not used - Start
                    if (HasRows == 0)
                    {
                        //build sql statement - Start
                        SqlStatement = "DELETE FROM SAS_SemesterSetup WHERE SAST_Code = ";
                        SqlStatement += clsGeneric.AddQuotes(argEn.SemisterSetupCode);
                        //build sql statement - Stop

                        //Save Details to Database
                        RecordsSaved = _DatabaseFactory.ExecuteSqlStatement(Helper.GetDataBaseType,
                            DataBaseConnectionString, SqlStatement);

                        //if records saved successfully - Start
                        if (RecordsSaved > -1)
                            Result = true;
                        else
                            throw new Exception("Delete Failed! No Row has been deleted...");
                        //if records saved successfully - Stop
                    }
                    //if record not used - Sto

                    return Result;
                }
                //if details available - Stop

                return false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region Fetch Total Batch Amount
        //modified by Hafiz @ 10/6/2016 - added ProgramId

        public string FetchTotalBatchAmount(string BatchNumber, string ProgramId)
        {
            //varaiable declarations
            string SqlStatement = null; string BatchTotal = null;

            try
            {
                //build SqlStatement - Start
                SqlStatement = "SELECT SUM(TransAmount) AS Total from SAS_Accounts where BatchCode = ";
                SqlStatement += clsGeneric.AddQuotes(BatchNumber) + "and CreditRef IN ";
                SqlStatement += "(SELECT SASI_matricno FROM SAS_student WHERE SASI_pgid = '" + ProgramId + "')";

                //build SqlStatement - Stop

                //Get User Login Details - Start
                IDataReader _IDataReader = _DatabaseFactory.ExecuteReader(Helper.GetDataBaseType,
                        DataBaseConnectionString, SqlStatement).CreateDataReader();
                //Get User Login Details - Stop

                //if details available - Start
                if (_IDataReader != null)
                {
                    if (_IDataReader.Read())
                    {
                        //get batch total - Start
                        BatchTotal = clsGeneric.NullToString(
                            _IDataReader["Total"]);
                        //get batch total - Stop
                    }

                    _IDataReader.Close();
                    _IDataReader.Dispose();
                }
                //if details available - Stop

                return BatchTotal;

            }
            catch (Exception err)
            {
                throw err;
            }
        }

        #endregion

        #region Load Object 

        /// <summary>
        /// Method to Load SemesterSetup Entity 
        /// </summary>
        /// <param name="argReader">IDataReader Object is an Input.</param>
        /// <returns>Returns SemesterSetup Entity</returns>
        private SemesterSetupEn LoadObject(IDataReader argReader)
        {
            SemesterSetupEn _SemesterSetupEn = new SemesterSetupEn();
            _SemesterSetupEn.SemisterSetupCode = GetValue<string>(argReader, "SAST_Code");
            _SemesterSetupEn.Semester = GetValue<string>(argReader, "SAST_Semester");
            _SemesterSetupEn.Description = GetValue<string>(argReader, "SAST_Description");
            _SemesterSetupEn.Status = GetValue<bool>(argReader, "SAST_Status");
            _SemesterSetupEn.Code = GetValue<int>(argReader, "SABR_Code");
            _SemesterSetupEn.UpdatedUser = GetValue<string>(argReader, "SAST_UpdatedUser");
            _SemesterSetupEn.UpdatedDtTm = GetValue<string>(argReader, "SAST_UpdatedDtTm");
            _SemesterSetupEn.CurrSem = GetValue<bool>(argReader, "sast_iscurrentsem");
            _SemesterSetupEn.SemisterCode2 = GetValue<string>(argReader, "SAST_Code2");

            return _SemesterSetupEn;
        }

        
        private static T GetValue<T>(IDataReader argReader, string argColNm)
        {
            if (!argReader.IsDBNull(argReader.GetOrdinal(argColNm)))
                return (T)argReader.GetValue(argReader.GetOrdinal(argColNm));
            else
                return default(T);
        }

        #endregion

        #region Get Current Session List
        //modified by Hafiz @ 07/6/2016

        public List<SemesterSetupEn> GetCurrentSessionList(SemesterSetupEn argEn)
        {
            //create instances
            List<SemesterSetupEn> SessionList = new List<SemesterSetupEn>();

            //variable declarations
            string SqlStatement = null;

            try
            {
                //build sql statement - Start
                SqlStatement = "SELECT DISTINCT * FROM SAS_SemesterSetup WHERE SAST_Status = true ORDER by SAST_Code";
                //SqlStatement += "AND sast_iscurrentsem = true;";
                //SqlStatement += clsGeneric.AddQuotes(argEn);
                //build sql statement - Stop

                //Get User Login Details - Start
                IDataReader _IDataReader = _DatabaseFactory.ExecuteReader(Helper.GetDataBaseType,
                        DataBaseConnectionString, SqlStatement).CreateDataReader();
                //Get User Login Details - Stop

                //if details available - Start
                if (_IDataReader != null)
                {
                    while (_IDataReader.Read())
                    {
                        SemesterSetupEn _SemesterSetupEn = LoadObject(_IDataReader);
                        SessionList.Add(_SemesterSetupEn);
                    }

                    _IDataReader.Close();
                    _IDataReader.Dispose();

                    return SessionList;
                }
                //if details available - Stop

                return null;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion
    }

}
