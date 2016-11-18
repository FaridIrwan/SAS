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
    /// Class to handle all the DegreeTypes Methods.
    /// </summary>
    public class DegreeTypeDAL
    {
        #region Global Declarations 

        private DbParameterCollection _DbParameterCollection = null;

        private MaxModule.DatabaseProvider _DatabaseFactory =
            new MaxModule.DatabaseProvider();

        private string DataBaseConnectionString = Helper.
           GetConnectionString();

        #endregion

        public DegreeTypeDAL()
        {
        }

        #region GetList 

        /// <summary>
        /// Method to Get List of DegreeTypes
        /// </summary>
        /// <param name="argEn">DegreeType Entity is an Input.</param>
        /// <returns>Returns List of DegreeTypes</returns>
        public List<DegreeTypeEn> GetList(DegreeTypeEn argEn)
        {
            List<DegreeTypeEn> loEnList = new List<DegreeTypeEn>();
            string sqlCmd = "select * from SAS_DegreeType";
            
            try
            {
                if (!FormHelp.IsBlank(sqlCmd))
                {
                    using (IDataReader loReader = _DatabaseFactory.ExecuteReader(Helper.GetDataBaseType,
                        DataBaseConnectionString, sqlCmd).CreateDataReader())
                    {
                        while (loReader.Read())
                        {
                            DegreeTypeEn loItem = LoadObject(loReader);
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

        #region GetDegreeTypeList 

        /// <summary>
        /// Method to Get List of Active or Inactive DegreeTypes
        /// </summary>
        /// <param name="argEn">DegreeType Entity is an Input.DegreeTypeCode,Description,SName and Status are Input Parameters</param>
        /// <returns>Returns List of DegreeTypes</returns>
        public List<DegreeTypeEn> GetDegreeTypeList(DegreeTypeEn argEn)
        {
            List<DegreeTypeEn> loEnList = new List<DegreeTypeEn>();
            argEn.DegreeTypeCode = argEn.DegreeTypeCode.Replace("*", "%");
            argEn.Description = argEn.Description.Replace("*", "%");
            argEn.SName = argEn.SName.Replace("*", "%");
            string sqlCmd = "select SADT_Code,SADT_Desc,SADT_SName,SADT_Status from SAS_DegreeType where SADT_Code <> '0'";
            if (argEn.DegreeTypeCode.Length != 0) sqlCmd = sqlCmd + " and SADT_Code like '" + argEn.DegreeTypeCode + "'";
            if (argEn.Description.Length != 0) sqlCmd = sqlCmd + " and SADT_Desc like '" + argEn.Description + "'";
            if (argEn.SName.Length != 0) sqlCmd = sqlCmd + " and SADT_SName like '" + argEn.SName + "'";
            //if (argEn.Status == true) sqlCmd = sqlCmd + " and SADT_Status =1";
            //if (argEn.Status == false) sqlCmd = sqlCmd + " and SADT_Status =0";
            if (argEn.Status == true) sqlCmd = sqlCmd + " and SADT_Status = true";
            if (argEn.Status == false) sqlCmd = sqlCmd + " and SADT_Status = false";
            sqlCmd = sqlCmd + " order by SADT_Code";
            
            try
            {
                if (!FormHelp.IsBlank(sqlCmd))
                {
                    using (IDataReader loReader = _DatabaseFactory.ExecuteReader(Helper.GetDataBaseType,
                        DataBaseConnectionString, sqlCmd).CreateDataReader())
                    {
                        while (loReader.Read())
                        {
                            DegreeTypeEn loItem = LoadObject(loReader);
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
        /// Method to Get DegreeType Entity
        /// </summary>
        /// <param name="argEn">DegreeType Entity is an Input</param>
        /// <returns>Returns DegreeType Entity</returns>
        public DegreeTypeEn GetItem(DegreeTypeEn argEn)
        {
            DegreeTypeEn loItem = new DegreeTypeEn();
            string sqlCmd = "Select * FROM SAS_DegreeType WHERE SADT_Code = @SADT_Code";
            
            try
            {
                if (!FormHelp.IsBlank(sqlCmd))
                {
                    using (IDataReader loReader = _DatabaseFactory.ExecuteReader(Helper.GetDataBaseType,
                        DataBaseConnectionString, sqlCmd).CreateDataReader()) 
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
        /// Method to Insert DegreeType 
        /// </summary>
        /// <param name="argEn">DegreeType Entity is an Input.</param>
        /// <returns>Returns Boolean</returns>
        public bool Insert(DegreeTypeEn argEn)
        {
            bool lbRes = false;
            int iOut = 0;
            string sqlCmd = "Select count(*) as cnt From SAS_DegreeType WHERE SADT_Code = @SADT_Code or SADT_Desc = @SADT_Desc";
            try
            {
                if (!FormHelp.IsBlank(sqlCmd))
                {
                    DbCommand cmdSel = _DatabaseFactory.GetDbCommand(Helper.GetDataBaseType, sqlCmd, DataBaseConnectionString);
                    _DatabaseFactory.AddInParameter(ref cmdSel, "@SADT_Code", DbType.String, argEn.DegreeTypeCode);
                    _DatabaseFactory.AddInParameter(ref cmdSel, "@SADT_Desc", DbType.String, argEn.Description);
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
                        sqlCmd = "INSERT INTO SAS_DegreeType(SADT_Code,SADT_Desc,SADT_SName,SADT_Status,SABR_Code,SADT_UpdatedUser,SADT_UpdatedDtTm) VALUES (@SADT_Code,@SADT_Desc,@SADT_SName,@SADT_Status,@SABR_Code,@SADT_UpdatedUser,@SADT_UpdatedDtTm) ";

                        if (!FormHelp.IsBlank(sqlCmd))
                        {
                            DbCommand cmd = _DatabaseFactory.GetDbCommand(Helper.GetDataBaseType, sqlCmd, DataBaseConnectionString);
                            _DatabaseFactory.AddInParameter(ref cmd, "@SADT_Code", DbType.String, argEn.DegreeTypeCode);
                            _DatabaseFactory.AddInParameter(ref cmd, "@SADT_Desc", DbType.String, argEn.Description);
                            _DatabaseFactory.AddInParameter(ref cmd, "@SADT_SName", DbType.String, argEn.SName);
                            _DatabaseFactory.AddInParameter(ref cmd, "@SADT_Status", DbType.Boolean, argEn.Status);
                            _DatabaseFactory.AddInParameter(ref cmd, "@SABR_Code", DbType.Int32, argEn.Code);
                            _DatabaseFactory.AddInParameter(ref cmd, "@SADT_UpdatedUser", DbType.String, argEn.UpdatedUser);
                            _DatabaseFactory.AddInParameter(ref cmd, "@SADT_UpdatedDtTm", DbType.String, argEn.UpdatedDtTm);
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
        /// Method to Update DegreeType 
        /// </summary>
        /// <param name="argEn">DegreeType Entity is an Input.</param>
        /// <returns>Returns Boolean</returns>
        public bool Update(DegreeTypeEn argEn)
        {
            bool lbRes = false;
            int iOut = 0;
            string sqlCmd = "Select count(*) as cnt From SAS_DegreeType WHERE SADT_Code != @SADT_Code and SADT_Desc = @SADT_Desc";
            try
            {
                if (!FormHelp.IsBlank(sqlCmd))
                {
                    DbCommand cmdSel = _DatabaseFactory.GetDbCommand(Helper.GetDataBaseType, sqlCmd, DataBaseConnectionString);
                    _DatabaseFactory.AddInParameter(ref cmdSel, "@SADT_Code", DbType.String, argEn.DegreeTypeCode);
                    _DatabaseFactory.AddInParameter(ref cmdSel, "@SADT_Desc", DbType.String, argEn.Description);
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
                        sqlCmd = "UPDATE SAS_DegreeType SET SADT_Code = @SADT_Code, SADT_Desc = @SADT_Desc, SADT_SName = @SADT_SName, SADT_Status = @SADT_Status, SABR_Code = @SABR_Code, SADT_UpdatedUser = @SADT_UpdatedUser, SADT_UpdatedDtTm = @SADT_UpdatedDtTm WHERE SADT_Code = @SADT_Code";

                        if (!FormHelp.IsBlank(sqlCmd))
                        {
                            DbCommand cmd = _DatabaseFactory.GetDbCommand(Helper.GetDataBaseType, sqlCmd, DataBaseConnectionString);
                            _DatabaseFactory.AddInParameter(ref cmd, "@SADT_Code", DbType.String, argEn.DegreeTypeCode);
                            _DatabaseFactory.AddInParameter(ref cmd, "@SADT_Desc", DbType.String, argEn.Description);
                            _DatabaseFactory.AddInParameter(ref cmd, "@SADT_SName", DbType.String, argEn.SName);
                            _DatabaseFactory.AddInParameter(ref cmd, "@SADT_Status", DbType.Boolean, argEn.Status);
                            _DatabaseFactory.AddInParameter(ref cmd, "@SABR_Code", DbType.Int32, argEn.Code);
                            _DatabaseFactory.AddInParameter(ref cmd, "@SADT_UpdatedUser", DbType.String, argEn.UpdatedUser);
                            _DatabaseFactory.AddInParameter(ref cmd, "@SADT_UpdatedDtTm", DbType.String, argEn.UpdatedDtTm.ToString());
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
        /// Method to Delete DegreeType 
        /// </summary>
        /// <param name="argEn">DegreeType Entity is an Input.</param>
        /// <returns>Returns Boolean</returns>
        public bool Delete(DegreeTypeEn argEn)
        {
            //Edited By Zoya @ 3/01/2016

            bool lbRes = false;
            int iOut = 0;

            string sqlCmd = "Select count(*) as cnt From SAS_Program WHERE SAPG_ProgramType = @SADT_Code";

            try
            {
                if (!FormHelp.IsBlank(sqlCmd))
                {
                    DbCommand cmdSel = _DatabaseFactory.GetDbCommand(Helper.GetDataBaseType, sqlCmd, DataBaseConnectionString);
                    _DatabaseFactory.AddInParameter(ref cmdSel, "@SADT_Code", DbType.String, argEn.DegreeTypeCode);
                    _DbParameterCollection = cmdSel.Parameters;

                    using (IDataReader dr = _DatabaseFactory.GetIDataReader(Helper.GetDataBaseType, cmdSel,
                        DataBaseConnectionString, sqlCmd, _DbParameterCollection).CreateDataReader())
                    {
                        if (dr.Read())
                            iOut = clsGeneric.NullToInteger(dr["cnt"]);
                        if (iOut > 0)
                            throw new Exception("Record Already In Use");
                    }
                    if (iOut == 0)
                    {

                        sqlCmd = "DELETE FROM SAS_DegreeType WHERE SADT_Code = @SADT_Code";


                        if (!FormHelp.IsBlank(sqlCmd))
                        {
                            DbCommand cmd = _DatabaseFactory.GetDbCommand(Helper.GetDataBaseType, sqlCmd, DataBaseConnectionString);
                            _DatabaseFactory.AddInParameter(ref cmd, "@SADT_Code", DbType.String, argEn.DegreeTypeCode);
                            _DbParameterCollection = cmd.Parameters;

                            int liRowAffected = _DatabaseFactory.ExecuteNonQuery(Helper.GetDataBaseType, cmd,
                            DataBaseConnectionString, sqlCmd, _DbParameterCollection);

                            if (liRowAffected > -1)
                                lbRes = true;
                            else
                                throw new Exception("Delete Failed! No Row has been deleted...");
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

        #region LoadObject

        /// <summary>
        /// Method to Load DegreeType Entity 
        /// </summary>
        /// <param name="argReader">IDataReader Object is an Input.</param>
        /// <returns>Returns DegreeType Entity</returns>
        private DegreeTypeEn LoadObject(IDataReader argReader)
        {
            DegreeTypeEn loItem = new DegreeTypeEn();
            loItem.DegreeTypeCode = GetValue<string>(argReader, "SADT_Code");
            loItem.Description = GetValue<string>(argReader, "SADT_Desc");
            loItem.SName = GetValue<string>(argReader, "SADT_SName");
            loItem.Status = GetValue<bool>(argReader, "SADT_Status");

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
