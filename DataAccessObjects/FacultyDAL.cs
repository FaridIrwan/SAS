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
    /// Class to handle all the Faculty Methods.
    /// </summary>
    public class FacultyDAL
    {
        #region Global Declarations 

        private DbParameterCollection _DbParameterCollection = null;

        private MaxModule.DatabaseProvider _DatabaseFactory =
            new MaxModule.DatabaseProvider();

        private string DataBaseConnectionString = Helper.
           GetConnectionString();

       #endregion

        public FacultyDAL()
        {
        }

        #region GetList 

        /// <summary>
        /// Method to Get List of Faculty
        /// </summary>
        /// <param name="argEn">Faculty Entity is an Input.</param>
        /// <returns>Returns List of Faculty</returns>
        public List<FacultyEn> GetList(FacultyEn argEn)
        {
            
            List<FacultyEn> loEnList = new List<FacultyEn>();
            argEn.SAFC_Code = argEn.SAFC_Code.Replace("*", "%");
            string sqlCmd = "select * from SAS_Faculty  WHERE SAFC_CODE  <> '0'  and SAFC_Status = 't' ";
            if (argEn.SAFC_Code.Length != 0) sqlCmd = sqlCmd + " and SAFC_CODE like '" + argEn.SAFC_Code + "'";

            try
            {
                if (!FormHelp.IsBlank(sqlCmd))
                {
                    using (IDataReader loReader = _DatabaseFactory.ExecuteReader(Helper.GetDataBaseType,
                        DataBaseConnectionString, sqlCmd).CreateDataReader())
                    {
                        while (loReader.Read())
                        {
                            FacultyEn loItem = LoadObject(loReader);
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

        #region GetFacultyList 

        /// <summary>
        /// Method to Get List of all Faculty
        /// </summary>
        /// <param name="argEn">Faculty Entity is an Input.</param>
        /// <returns>Returns List of Faculty</returns>
        public List<FacultyEn> GetFacultyList(FacultyEn argEn)
        {
            List<FacultyEn> loEnList = new List<FacultyEn>();
            argEn.SAFC_Code = argEn.SAFC_Code.Replace("*", "%");
            argEn.SAFC_Desc = argEn.SAFC_Desc.Replace("*", "%");
            argEn.SAFC_SName = argEn.SAFC_SName.Replace("*", "%");
            argEn.SAFC_GlAccount = argEn.SAFC_GlAccount.Replace("*", "%"); 

            string sqlCmd = "select * from SAS_Faculty  WHERE SAFC_CODE  <> '0'";
            if (argEn.SAFC_Code.Length != 0) sqlCmd = sqlCmd + " and SAFC_CODE like '" + argEn.SAFC_Code + "'";
            if (argEn.SAFC_Desc.Length != 0) sqlCmd = sqlCmd + " and SAFC_Desc like '" + argEn.SAFC_Desc + "'";
            if (argEn.SAFC_SName.Length != 0) sqlCmd = sqlCmd + " and SAFC_SName like '" + argEn.SAFC_SName + "'";
            if (argEn.SAFC_GlAccount.Length != 0) sqlCmd = sqlCmd + " and SAFC_GlAccount like '" + argEn.SAFC_GlAccount + "'";
            if (argEn.SAFC_Status == true) sqlCmd = sqlCmd + " and SAFC_Status = true";
            if (argEn.SAFC_Status == false) sqlCmd = sqlCmd + " and SAFC_Status =false";
            sqlCmd = sqlCmd + " order by SAFC_CODE";

            try
            {
                if(!FormHelp.IsBlank(sqlCmd))
                {
                    using (IDataReader loReader = _DatabaseFactory.ExecuteReader(Helper.GetDataBaseType,
                        DataBaseConnectionString, sqlCmd).CreateDataReader())
                    {
                        while (loReader.Read())
                        {
                            FacultyEn loItem = LoadObject(loReader);
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
        /// Method to Get Faculty Entity
        /// </summary>
        /// <param name="argEn">Faculty Entity is an Input</param>
        /// <returns>Returns Faculty Entity</returns>
        public FacultyEn GetItem(FacultyEn argEn)
        {
            FacultyEn loItem = new FacultyEn();
            //string sqlCmd = "Select * FROM SAS_Faculty WHERE SAFC_SName = @SAFC_SName";
            string sqlCmd = "Select * FROM SAS_Faculty WHERE SAFC_Desc = @SAFC_SName";

           try
            {
                if(!FormHelp.IsBlank(sqlCmd))
                {
                    DbCommand cmd = _DatabaseFactory.GetDbCommand(Helper.GetDataBaseType, sqlCmd, DataBaseConnectionString);
                    //_DatabaseFactory.AddInParameter(ref cmd, "@SAFC_SName", DbType.String, argEn.SAFC_SName);
                    _DatabaseFactory.AddInParameter(ref cmd, "@SAFC_SName", DbType.String, argEn.SAFC_Desc);
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
        /// Method to Insert Faculty 
        /// </summary>
        /// <param name="argEn">Faculty Entity is an Input.</param>
        /// <returns>Returns Boolean</returns>
        public bool Insert(FacultyEn argEn)
        {
            bool lbRes = false;
            int iOut = 0;
            string sqlCmd = "Select count(*) as cnt From SAS_Faculty WHERE SAFC_Code = @SAFC_Code or SAFC_Desc = @SAFC_Desc";
            try
            {
                if(!FormHelp.IsBlank(sqlCmd))
                {
                    DbCommand cmdSel = _DatabaseFactory.GetDbCommand(Helper.GetDataBaseType, sqlCmd, DataBaseConnectionString);
                    _DatabaseFactory.AddInParameter(ref cmdSel, "@SAFC_Code", DbType.String, argEn.SAFC_Code);
                    _DatabaseFactory.AddInParameter(ref cmdSel, "@SAFC_Desc", DbType.String, argEn.SAFC_Desc);
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
                       sqlCmd = "INSERT INTO SAS_Faculty(SAFC_Code,SAFC_Desc,SAFC_SName,SAFC_Incharge,SAFC_GlAccount,SAFC_Status,SABR_Code,SAFC_UpdatedBy,SAFC_UpdatedDtTm) VALUES (@SAFC_Code,@SAFC_Desc,@SAFC_SName,@SAFC_Incharge,@SAFC_GlAccount,@SAFC_Status,@SABR_Code,@SAFC_UpdatedBy,@SAFC_UpdatedDtTm) ";

                        if(!FormHelp.IsBlank(sqlCmd))
                        {
                            DbCommand cmd = _DatabaseFactory.GetDbCommand(Helper.GetDataBaseType, sqlCmd, DataBaseConnectionString);
                            _DatabaseFactory.AddInParameter(ref cmd, "@SAFC_Code", DbType.String, argEn.SAFC_Code);
                            _DatabaseFactory.AddInParameter(ref cmd, "@SAFC_Desc", DbType.String, argEn.SAFC_Desc);
                            _DatabaseFactory.AddInParameter(ref cmd, "@SAFC_SName", DbType.String, argEn.SAFC_SName);
                            _DatabaseFactory.AddInParameter(ref cmd, "@SAFC_Incharge", DbType.String, argEn.SAFC_Incharge);
                            _DatabaseFactory.AddInParameter(ref cmd, "@SAFC_GlAccount", DbType.String, argEn.SAFC_GlAccount);
                            _DatabaseFactory.AddInParameter(ref cmd, "@SAFC_Status", DbType.Boolean, argEn.SAFC_Status);
                            _DatabaseFactory.AddInParameter(ref cmd, "@SABR_Code", DbType.Int32, argEn.SABR_Code);
                            _DatabaseFactory.AddInParameter(ref cmd, "@SAFC_UpdatedBy", DbType.String, argEn.SAFC_UpdatedBy);
                            _DatabaseFactory.AddInParameter(ref cmd, "@SAFC_UpdatedDtTm", DbType.String, argEn.SAFC_UpdatedDtTm);
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

        #region Update 

        /// <summary>
        /// Method to Update Faculty 
        /// </summary>
        /// <param name="argEn">Faculty Entity is an Input.</param>
        /// <returns>Returns Boolean</returns>
        public bool Update(FacultyEn argEn)
        {
            bool lbRes = false;
            int iOut = 0;
            
            try
            {
                string sqlCmd = "Select count(safc_code) as cnt From SAS_Faculty WHERE SAFC_Code != @SAFC_Code and SAFC_Desc = @SAFC_Desc";

                if(!FormHelp.IsBlank(sqlCmd))
                {
                    DbCommand cmdSel = _DatabaseFactory.GetDbCommand(Helper.GetDataBaseType, sqlCmd, DataBaseConnectionString);
                    _DatabaseFactory.AddInParameter(ref cmdSel, "@SAFC_Code", DbType.StringFixedLength, argEn.SAFC_Code);
                    _DatabaseFactory.AddInParameter(ref cmdSel, "@SAFC_Desc", DbType.StringFixedLength, argEn.SAFC_Desc);
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
                        sqlCmd = "UPDATE SAS_Faculty SET SAFC_Code = @SAFC_Code, SAFC_Desc = @SAFC_Desc, SAFC_SName = @SAFC_SName, SAFC_Incharge = @SAFC_Incharge, SAFC_GlAccount = @SAFC_GlAccount, SABR_Code = @SABR_Code, SAFC_UpdatedBy = @SAFC_UpdatedBy, SAFC_UpdatedDtTm = @SAFC_UpdatedDtTm,SAFC_Status = @SAFC_Status WHERE SAFC_Code = @SAFC_Code";

                        if(!FormHelp.IsBlank(sqlCmd))
                        {
                            DbCommand cmd = _DatabaseFactory.GetDbCommand(Helper.GetDataBaseType, sqlCmd, DataBaseConnectionString);
                            _DatabaseFactory.AddInParameter(ref cmd, "@SAFC_Code", DbType.String, argEn.SAFC_Code);
                            _DatabaseFactory.AddInParameter(ref cmd, "@SAFC_Desc", DbType.String, argEn.SAFC_Desc);
                            _DatabaseFactory.AddInParameter(ref cmd, "@SAFC_SName", DbType.String, argEn.SAFC_SName);
                            _DatabaseFactory.AddInParameter(ref cmd, "@SAFC_Incharge", DbType.String, argEn.SAFC_Incharge);
                            _DatabaseFactory.AddInParameter(ref cmd, "@SAFC_GlAccount", DbType.String, argEn.SAFC_GlAccount);
                            _DatabaseFactory.AddInParameter(ref cmd, "@SAFC_Status", DbType.Boolean, argEn.SAFC_Status);
                            _DatabaseFactory.AddInParameter(ref cmd, "@SABR_Code", DbType.Int32, argEn.SABR_Code);
                            _DatabaseFactory.AddInParameter(ref cmd, "@SAFC_UpdatedBy", DbType.String, argEn.SAFC_UpdatedBy);
                            _DatabaseFactory.AddInParameter(ref cmd, "@SAFC_UpdatedDtTm", DbType.String, argEn.SAFC_UpdatedDtTm);
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
        /// Method to Delete Faculty 
        /// </summary>
        /// <param name="argEn">Faculty Entity is an Input.</param>
        /// <returns>Returns Boolean</returns>
        public bool Delete(FacultyEn argEn)
        {
            bool lbRes = false;
            int iOut = 0;

            string sqlCmd = "select sum(rows) as total from(SELECT COUNT(*) AS rows FROM  SAS_Student where SASI_Faculty = @SAFC_Code union all select count(*) as rows from SAS_Program WHERE SAFC_Code = @SAFC_Code)AS U";
            try
            {
                if(!FormHelp.IsBlank(sqlCmd))
                {
                    DbCommand cmdSel = _DatabaseFactory.GetDbCommand(Helper.GetDataBaseType, sqlCmd, DataBaseConnectionString);
                    _DatabaseFactory.AddInParameter(ref cmdSel, "@SAFC_Code", DbType.String, argEn.SAFC_Code);
                    _DbParameterCollection = cmdSel.Parameters;

                    using (IDataReader dr = _DatabaseFactory.GetIDataReader(Helper.GetDataBaseType, cmdSel,
                        DataBaseConnectionString, sqlCmd, _DbParameterCollection).CreateDataReader()) 
                    {
                        if (dr.Read())
                            iOut = clsGeneric.NullToInteger(dr["total"]);
                        if (iOut > 0)
                            throw new Exception("Record Already In Use");
                    }
                    if (iOut == 0)
                    {
                        string sqlCmd1 = "DELETE FROM SAS_Faculty WHERE SAFC_Code = @SAFC_Code";

                        if(!FormHelp.IsBlank(sqlCmd1))
                        {
                            DbCommand cmd = _DatabaseFactory.GetDbCommand(Helper.GetDataBaseType, sqlCmd1, DataBaseConnectionString);
                            _DatabaseFactory.AddInParameter(ref cmd, "@SAFC_Code", DbType.String, argEn.SAFC_Code);
                            _DbParameterCollection = cmd.Parameters;

                            int liRowAffected = _DatabaseFactory.ExecuteNonQuery(Helper.GetDataBaseType, cmd,
                                DataBaseConnectionString, sqlCmd1, _DbParameterCollection);

                            if (liRowAffected > -1)
                                lbRes = true;
                            else
                                throw new Exception("Deletion Failed! No Row has been Deleted...");
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

        #region Load Object 

        /// <summary>
        /// Method to Load Faculty Entity 
        /// </summary>
        /// <param name="argReader">IDataReader Object is an Input.</param>
        /// <returns>Returns Faculty Entity</returns>
        private FacultyEn LoadObject(IDataReader argReader)
        {
            FacultyEn loItem = new FacultyEn();
            loItem.SAFC_Code = GetValue<string>(argReader, "SAFC_Code");
            loItem.SAFC_Desc = GetValue<string>(argReader, "SAFC_Desc");
            loItem.SAFC_SName = GetValue<string>(argReader, "SAFC_SName");
            loItem.SAFC_Incharge = GetValue<string>(argReader, "SAFC_Incharge");
            loItem.SAFC_GlAccount = GetValue<string>(argReader, "SAFC_GlAccount");
            loItem.SAFC_Status = GetValue<bool>(argReader, "SAFC_Status");
            loItem.SABR_Code = GetValue<int>(argReader, "SABR_Code");
            loItem.SAFC_UpdatedBy = GetValue<string>(argReader, "SAFC_UpdatedBy");
            loItem.SAFC_UpdatedDtTm = GetValue<string>(argReader, "SAFC_UpdatedDtTm");

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
    }
}


