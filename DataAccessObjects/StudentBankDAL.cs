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
    public class StudentBankDAL
    {
        #region Global Declarations

        private DbParameterCollection _DbParameterCollection = null;

        private MaxModule.DatabaseProvider _DatabaseFactory =
            new MaxModule.DatabaseProvider();

        private string DataBaseConnectionString = Helper.
           GetConnectionString();

        #endregion

        #region Insert

        /// <summary>
        /// Method to Insert StudentBank 
        /// </summary>
        /// <param name="argEn">StudentBank Entity is an Input.</param>
        /// <returns>Returns Boolean</returns> 
        public bool Insert(StudentBankEn argEn)
        {
            bool lbRes = false;
            int iOut = 0;

            string sqlCmd = "Select count(*) as cnt From SAS_StudentBank WHERE SASB_Code = @SASB_Code or SASB_Desc = @SASB_Desc";
            try
            {
                if (!FormHelp.IsBlank(sqlCmd))
                {
                    DbCommand cmdSel = _DatabaseFactory.GetDbCommand(Helper.GetDataBaseType, sqlCmd, DataBaseConnectionString);
                    _DatabaseFactory.AddInParameter(ref cmdSel, "@SASB_Code", DbType.String, argEn.StudentBankCode);
                    _DatabaseFactory.AddInParameter(ref cmdSel, "@SASB_Desc", DbType.String, argEn.Description);
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
                        sqlCmd = "INSERT INTO SAS_StudentBank(SASB_Code,SASB_Desc,SASB_Status,SASB_UpdatedBy,SASB_UpdatedDtTm) VALUES (@SASB_Code,@SASB_Desc,@SASB_Status,@SASB_UpdatedBy,@SASB_UpdatedDtTm) ";

                        if (!FormHelp.IsBlank(sqlCmd))
                        {
                            DbCommand cmd = _DatabaseFactory.GetDbCommand(Helper.GetDataBaseType, sqlCmd, DataBaseConnectionString);
                            _DatabaseFactory.AddInParameter(ref cmd, "@SASB_Code", DbType.String, argEn.StudentBankCode);
                            _DatabaseFactory.AddInParameter(ref cmd, "@SASB_Desc", DbType.String, argEn.Description);
                            _DatabaseFactory.AddInParameter(ref cmd, "@SASB_Status", DbType.Boolean, argEn.Status);
                            _DatabaseFactory.AddInParameter(ref cmd, "@SASB_UpdatedBy", DbType.String, argEn.UpdatedBy);
                            _DatabaseFactory.AddInParameter(ref cmd, "@SASB_UpdatedDtTm", DbType.String, argEn.UpdatedDtTm);
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
        /// Method to Update StudentBank
        /// </summary>
        /// <param name="argEn">StudentBank Entity is an Input.</param>
        /// <returns>Returns Boolean</returns> 
        public bool Update(StudentBankEn argEn)
        {
            bool lbRes = false;
            int iOut = 0;

            string sqlCmd = "Select count(*) as cnt From SAS_StudentBank WHERE SASB_Code = @SASB_Code AND SASB_Desc = @SASB_Desc AND SASB_Status = @SASB_Status";
            try
            {
                if (!FormHelp.IsBlank(sqlCmd))
                {
                    DbCommand cmdSel = _DatabaseFactory.GetDbCommand(Helper.GetDataBaseType, sqlCmd, DataBaseConnectionString);
                    _DatabaseFactory.AddInParameter(ref cmdSel, "@SASB_Code", DbType.String, argEn.StudentBankCode);
                    _DatabaseFactory.AddInParameter(ref cmdSel, "@SASB_Desc", DbType.String, argEn.Description);
                    _DatabaseFactory.AddInParameter(ref cmdSel, "@SASB_Status", DbType.Boolean, argEn.Status);
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
                        sqlCmd = "UPDATE SAS_StudentBank SET SASB_Code = @SASB_Code, SASB_Desc = @SASB_Desc, SASB_Status = @SASB_Status, SASB_UpdatedBy = @SASB_UpdatedBy, SASB_UpdatedDtTm = @SASB_UpdatedDtTm WHERE SASB_Code = @SASB_Code";
                        if (!FormHelp.IsBlank(sqlCmd))
                        {
                            DbCommand cmd = _DatabaseFactory.GetDbCommand(Helper.GetDataBaseType, sqlCmd, DataBaseConnectionString);
                            _DatabaseFactory.AddInParameter(ref cmd, "@SASB_Code", DbType.String, argEn.StudentBankCode);
                            _DatabaseFactory.AddInParameter(ref cmd, "@SASB_Desc", DbType.String, argEn.Description);
                            _DatabaseFactory.AddInParameter(ref cmd, "@SASB_Status", DbType.Boolean, argEn.Status);
                            _DatabaseFactory.AddInParameter(ref cmd, "@SASB_UpdatedBy", DbType.String, argEn.UpdatedBy);
                            _DatabaseFactory.AddInParameter(ref cmd, "@SASB_UpdatedDtTm", DbType.String, argEn.UpdatedDtTm);
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

         //<summary>
         //Method to Delete StudentBank 
         //</summary>
         //<param name="argEn">StudentBank Entity is an Input.StudentBankCode as Input Property.</param>
         //<returns>Returns Boolean</returns>
        public bool Delete(StudentBankEn argEn)
        {
            bool lbRes = false;
            int iOut = 0;


            string sqlCmd = "Select count(*) as cnt From SAS_Student WHERE SASI_Bank = @SASB_Code";

            try
            {
                if (!FormHelp.IsBlank(sqlCmd))
                {
                    DbCommand cmdSel = _DatabaseFactory.GetDbCommand(Helper.GetDataBaseType, sqlCmd, DataBaseConnectionString);
                    _DatabaseFactory.AddInParameter(ref cmdSel, "@SASB_Code", DbType.String, argEn.StudentBankCode);
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
                        sqlCmd = "DELETE FROM SAS_StudentBank WHERE SASB_Code = @SASB_Code";

                        if (!FormHelp.IsBlank(sqlCmd))
                        {
                            DbCommand cmd = _DatabaseFactory.GetDbCommand(Helper.GetDataBaseType, sqlCmd, DataBaseConnectionString);
                            _DatabaseFactory.AddInParameter(ref cmd, "@SASB_Code", DbType.String, argEn.StudentBankCode);
                            _DbParameterCollection = cmd.Parameters;

                            int liRowAffected = _DatabaseFactory.ExecuteNonQuery(Helper.GetDataBaseType, cmd,
                                DataBaseConnectionString, sqlCmd, _DbParameterCollection);

                            if (liRowAffected > -1)
                                lbRes = true;
                            else
                                throw new Exception("Delete Failed! No Row has been updated...");
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

        #region GetStudentBankTypelist

        /// <summary>
        /// Method to Get List of  Active or Inactive StudentBank
        /// </summary>
        /// <param name="argEn">StudentBank Entity as an Input.StudentBankCode,Description and Status as Input Properties.</param>
        /// <returns>Returns List of StudentBank</returns>
        public List<StudentBankEn> GetStudentBankTypelist(StudentBankEn argEn)
        {
            List<StudentBankEn> loEnList = new List<StudentBankEn>();

            argEn.StudentBankCode = argEn.StudentBankCode.Replace("*", "%");
            argEn.Description = argEn.Description.Replace("*", "%");

            string sqlCmd = "select * from SAS_StudentBank where SASB_Code <> '0'";
            if (argEn.StudentBankCode.Length != 0) sqlCmd = sqlCmd + " and SASB_Code like '" + argEn.StudentBankCode + "'";
            if (argEn.Description.Length != 0) sqlCmd = sqlCmd + " and SASB_Desc like '" + argEn.Description + "'";
            //if (argEn.Status == true) sqlCmd = sqlCmd + " and SASB_Status =1";
            if (argEn.Status == true) sqlCmd = sqlCmd + " and SASB_Status = 'true'";
            //if (argEn.Status == false) sqlCmd = sqlCmd + " and SASB_Status =0";
            if (argEn.Status == false) sqlCmd = sqlCmd + " and SASB_Status = 'false'";
            sqlCmd = sqlCmd + " order by SASB_Code";


            try
            {
                if (!FormHelp.IsBlank(sqlCmd))
                {
                    using (IDataReader loReader = _DatabaseFactory.ExecuteReader(Helper.GetDataBaseType,
                        DataBaseConnectionString, sqlCmd).CreateDataReader())
                    {
                        while (loReader.Read())
                        {
                            StudentBankEn loItem = LoadObject(loReader);
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

        #region GetStudentBankTypeListAll

        /// <summary>
        /// Method to Get List of All StudentCategory
        /// </summary>
        /// <param name="argEn">StudentBank Entity as an Input.StudentBankCode and Description as Input Properties.</param>
        /// <returns>Returns List of StudentBank</returns>
        public List<StudentBankEn> GetStudentBankTypeListAll(StudentBankEn argEn)
        {
            List<StudentBankEn> loEnList = new List<StudentBankEn>();

            argEn.StudentBankCode = argEn.StudentBankCode.Replace("*", "%");
            argEn.Description = argEn.Description.Replace("*", "%");

            string sqlCmd = "select * from SAS_StudentBank where SASB_Code <> '0'";
            if (argEn.StudentBankCode.Length != 0) sqlCmd = sqlCmd + " and SASB_Code like '" + argEn.StudentBankCode + "'";
            if (argEn.Description.Length != 0) sqlCmd = sqlCmd + " and SASB_Desc like '" + argEn.Description + "'";
            sqlCmd = sqlCmd + " order by SASB_Code";


            try
            {
                if (!FormHelp.IsBlank(sqlCmd))
                {
                    using (IDataReader loReader = _DatabaseFactory.ExecuteReader(Helper.GetDataBaseType,
                        DataBaseConnectionString, sqlCmd).CreateDataReader())
                    {
                        while (loReader.Read())
                        {
                            StudentBankEn loItem = LoadObject(loReader);
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

        #region LoadObject

        /// <summary>
        /// Method to Load StudentBank Entity 
        /// </summary>
        /// <param name="argReader">IDataReader Object is an Input.</param>
        /// <returns>Returns StudentBank Entity</returns>
        private StudentBankEn LoadObject(IDataReader argReader)
        {
            StudentBankEn loItem = new StudentBankEn();

            loItem.StudentBankCode = GetValue<string>(argReader, "SASB_Code");
            loItem.Description = GetValue<string>(argReader, "SASB_Desc");
            loItem.Status = GetValue<bool>(argReader, "SASB_Status");
            loItem.UpdatedBy = GetValue<string>(argReader, "SASB_UpdatedBy");
            loItem.UpdatedDtTm = GetValue<string>(argReader, "SASB_UpdatedDtTm");

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
