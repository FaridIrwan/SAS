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
    /// Class to handle all the StudentStatus Methods.
    /// </summary>
    public class StudentStatusDAL
    {
        #region Global Declarations  

        private DbParameterCollection _DbParameterCollection = null;

        private MaxModule.DatabaseProvider _DatabaseFactory =
            new MaxModule.DatabaseProvider();

        private string DataBaseConnectionString = Helper.
           GetConnectionString();

        #endregion

        public StudentStatusDAL()
        {
        }

        #region GetStudentBlStatus 

        /// <summary>
        /// Method to Get StudentStatus by StudentStatusCode
        /// </summary>
        /// <param name="argEn">StudentStatusCode as an Input.</param>
        /// <returns>Returns StudentStatus Entity</returns>

        public StudentStatusEn GetStudentBlStatus(string argEn)
        {
            StudentStatusEn loItem = new StudentStatusEn();
            string sqlCmd = "select * from SAS_StudentStatus where SASS_Code='" + argEn + "'";
            try
            {
                if (!FormHelp.IsBlank(sqlCmd))
                {
                    using (IDataReader loReader = _DatabaseFactory.ExecuteReader(Helper.GetDataBaseType,
                        DataBaseConnectionString, sqlCmd).CreateDataReader())
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

        #region GetList 

        /// <summary>
        /// Method to Get List of StudentStatus
        /// </summary>
        /// <param name="argEn">StudentStatus Entity as an Input.</param>
        /// <returns>Returns List of StudentStatus</returns>
        public List<StudentStatusEn> GetList(StudentStatusEn argEn)
        {
            List<StudentStatusEn> loEnList = new List<StudentStatusEn>();
            string sqlCmd = "select * from SAS_StudentStatus";
            
            try
            {
                if (!FormHelp.IsBlank(sqlCmd))
                {
                    using (IDataReader loReader = _DatabaseFactory.ExecuteReader(Helper.GetDataBaseType,
                        DataBaseConnectionString, sqlCmd).CreateDataReader())
                    {
                        while (loReader.Read())
                        {
                            StudentStatusEn loItem = LoadObject(loReader);
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

        #region GetStudentStatusList

        /// <summary>
        /// Method to Get List of Active or Inactive StudentStatus
        /// </summary>
        /// <param name="argEn">StudentStatus Entity as an Input.StudentStatusCode,Description,Status and BlStatus as Input Property.</param>
        /// <returns>Returns List of StudentStatus</returns>
        public List<StudentStatusEn> GetStudentStatusList(StudentStatusEn argEn)
        {
            List<StudentStatusEn> loEnList = new List<StudentStatusEn>();
            StudentCategoryAccessDAL dobjSCAccess = new StudentCategoryAccessDAL();
            StudentCategoryAccessEn eobjSCAccess;

            argEn.StudentStatusCode = argEn.StudentStatusCode.Replace("*", "%");
            argEn.Description = argEn.Description.Replace("*", "%");
            string sqlCmd = "select SASS_Code,SASS_Description,SASS_Status,SASS_BlStatus from SAS_StudentStatus where SASS_Code <> '0'";
            if (argEn.StudentStatusCode.Length != 0) sqlCmd = sqlCmd + " and SASS_Code like '" + argEn.StudentStatusCode + "'";
            if (argEn.Description.Length != 0) sqlCmd = sqlCmd + " and SASS_Description like '" + argEn.Description + "'";
            //if (argEn.Status == true) sqlCmd = sqlCmd + " and SASS_Status =1";
            if (argEn.Status == true) sqlCmd = sqlCmd + " and SASS_Status = 'true'";
            //if (argEn.Status == false) sqlCmd = sqlCmd + " and SASS_Status =0";
            if (argEn.Status == false) sqlCmd = sqlCmd + " and SASS_Status = 'false'";
            //if (argEn.BlStatus == true) sqlCmd = sqlCmd + " and SASS_BlStatus =1";
            if (argEn.BlStatus == true) sqlCmd = sqlCmd + " and SASS_BlStatus = 'true'";
            //if (argEn.BlStatus == false) sqlCmd = sqlCmd + " and SASS_BlStatus =0";
            if (argEn.BlStatus == false) sqlCmd = sqlCmd + " and SASS_BlStatus = 'false'";
            sqlCmd = sqlCmd + " order by SASS_Code";
            try
            {
                if (!FormHelp.IsBlank(sqlCmd))
                {
                    using (IDataReader loReader = _DatabaseFactory.ExecuteReader(Helper.GetDataBaseType,
                        DataBaseConnectionString, sqlCmd).CreateDataReader())
                    {
                        while (loReader.Read())
                        {
                            StudentStatusEn loItem = LoadObject(loReader);
                            eobjSCAccess = new StudentCategoryAccessEn();
                            eobjSCAccess.StudentCategoryCode = loItem.StudentStatusCode;
                            loItem.LstStudentCategoryAccess = dobjSCAccess.GetStuCatAccessList(eobjSCAccess);
                            loEnList.Add(loItem);
                            eobjSCAccess = null;
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

        #region GetStudentStatusListAll 

        /// <summary>
        /// Method to Get List of All StudentStatus
        /// </summary>
        /// <param name="argEn">StudentStatus Entity as an Input.StudentStatusCode and Description as Input Property.</param>
        /// <returns>Returns List of StudentStatus</returns>
        public List<StudentStatusEn> GetStudentStatusListAll(StudentStatusEn argEn)
        {
            List<StudentStatusEn> loEnList = new List<StudentStatusEn>();
            StudentCategoryAccessDAL dobjSCAccess = new StudentCategoryAccessDAL();
            StudentCategoryAccessEn eobjSCAccess;

            argEn.StudentStatusCode = argEn.StudentStatusCode.Replace("*", "%");
            argEn.Description = argEn.Description.Replace("*", "%");
            string sqlCmd = "select SASS_Code,SASS_Description,SASS_Status,SASS_BlStatus from SAS_StudentStatus where SASS_Code <> '0'";
            if (argEn.StudentStatusCode.Length != 0) sqlCmd = sqlCmd + " and SASS_Code like '" + argEn.StudentStatusCode + "'";
            if (argEn.Description.Length != 0) sqlCmd = sqlCmd + " and SASS_Description like '" + argEn.Description + "'";
            sqlCmd = sqlCmd + " order by SASS_Code";
            
            try
            {
                if (!FormHelp.IsBlank(sqlCmd))
                {
                    using (IDataReader loReader = _DatabaseFactory.ExecuteReader(Helper.GetDataBaseType,
                        DataBaseConnectionString, sqlCmd).CreateDataReader())
                    {
                        while (loReader.Read())
                        {
                            StudentStatusEn loItem = LoadObject(loReader);
                            eobjSCAccess = new StudentCategoryAccessEn();
                            eobjSCAccess.StudentCategoryCode = loItem.StudentStatusCode;
                            loItem.LstStudentCategoryAccess = dobjSCAccess.GetStuCatAccessList(eobjSCAccess);
                            loEnList.Add(loItem);
                            eobjSCAccess = null;
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
        /// Method to Get StudentStatus Entity
        /// </summary>
        /// <param name="argEn">StudentStatus Entity is an Input.MatricNo as Input Property.</param>
        /// <returns>Returns StudentStatus Entity</returns>
        public StudentStatusEn GetItem(StudentStatusEn argEn)
        {
            StudentStatusEn loItem = new StudentStatusEn();
            string sqlCmd = "Select * FROM SAS_StudentStatus WHERE SASS_Code = " + 
                clsGeneric.AddQuotes(argEn.Code.ToString());
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
        /// Method to Insert StudentStatus 
        /// </summary>
        /// <param name="argEn">StudentStatus Entity is an Input.</param>
        /// <returns>Returns Boolean</returns> 
        public bool Insert(StudentStatusEn argEn)
        {
            bool lbRes = false;
            int iOut = 0;
            StudentCategoryAccessEn eobjSCAccess = new StudentCategoryAccessEn();
            StudentCategoryAccessDAL dobjSCAccess = new StudentCategoryAccessDAL();
            string sqlCmd = "Select count(*) as cnt From SAS_StudentStatus WHERE SASS_Code = @SASS_Code or SASS_Description = @SASS_Description";
            try
            {
                // Checking for Duplicates
                if (!FormHelp.IsBlank(sqlCmd))
                {
                    DbCommand cmdSel = _DatabaseFactory.GetDbCommand(Helper.GetDataBaseType, sqlCmd, DataBaseConnectionString);
                    _DatabaseFactory.AddInParameter(ref cmdSel, "@SASS_Code", DbType.String, argEn.StudentStatusCode);
                    _DatabaseFactory.AddInParameter(ref cmdSel, "@SASS_Description", DbType.String, argEn.Description);
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
                        sqlCmd = "INSERT INTO SAS_StudentStatus(SASS_Code,SASS_Description,SASS_BlStatus,SABR_Code,SASS_Status,SASS_UpdatedUser,SASS_UpdatedDtTm) VALUES (@SASS_Code,@SASS_Description,@SASS_BlStatus,@SABR_Code,@SASS_Status,@SASS_UpdatedUser,@SASS_UpdatedDtTm) ";
                        if (!FormHelp.IsBlank(sqlCmd))
                        {
                            DbCommand cmd = _DatabaseFactory.GetDbCommand(Helper.GetDataBaseType, sqlCmd, DataBaseConnectionString);
                            _DatabaseFactory.AddInParameter(ref cmd, "@SASS_Code", DbType.String, argEn.StudentStatusCode);
                            _DatabaseFactory.AddInParameter(ref cmd, "@SASS_Description", DbType.String, argEn.Description);
                            _DatabaseFactory.AddInParameter(ref cmd, "@SASS_BlStatus", DbType.Boolean, argEn.BlStatus);
                            _DatabaseFactory.AddInParameter(ref cmd, "@SABR_Code", DbType.Int32, argEn.Code);
                            _DatabaseFactory.AddInParameter(ref cmd, "@SASS_Status", DbType.Boolean, argEn.Status);
                            _DatabaseFactory.AddInParameter(ref cmd, "@SASS_UpdatedUser", DbType.String, argEn.UpdatedUser);
                            _DatabaseFactory.AddInParameter(ref cmd, "@SASS_UpdatedDtTm", DbType.String, argEn.UpdatedDtTm);
                            _DbParameterCollection = cmd.Parameters;

                            int liRowAffected = _DatabaseFactory.ExecuteNonQuery(Helper.GetDataBaseType, cmd,
                                DataBaseConnectionString, sqlCmd, _DbParameterCollection);
                            
                            int i = 0;
                            eobjSCAccess.StudentCategoryCode = argEn.StudentStatusCode;
                            //Deleting any existing StudentCategoryAccess for this studentstatus
                            dobjSCAccess.Delete(eobjSCAccess);
                            //Inserting  StudentCategoryAccess for this studentstatus
                            while (i < argEn.LstStudentCategoryAccess.Count)
                            {
                                eobjSCAccess = argEn.LstStudentCategoryAccess[i];
                                dobjSCAccess.Insert(eobjSCAccess);
                                eobjSCAccess = null;
                                i = i + 1;
                            }
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
        /// Method to Update StudentStatus 
        /// </summary>
        /// <param name="argEn">StudentStatus Entity is an Input.</param>
        /// <returns>Returns Boolean</returns> 
        public bool Update(StudentStatusEn argEn)
        {
            bool lbRes = false;
            int iOut = 0;
            StudentCategoryAccessEn eobjSCAccess = new StudentCategoryAccessEn();
            StudentCategoryAccessDAL dobjSCAccess = new StudentCategoryAccessDAL(); 
            string sqlCmd = "Select count(*) as cnt From SAS_StudentStatus WHERE SASS_Code != @SASS_Code  and SASS_Description = @SASS_Description";
            try
            {
                if (!FormHelp.IsBlank(sqlCmd))
                {
                    DbCommand cmdSel = _DatabaseFactory.GetDbCommand(Helper.GetDataBaseType, sqlCmd, DataBaseConnectionString);
                    _DatabaseFactory.AddInParameter(ref cmdSel, "@SASS_Code", DbType.String, argEn.StudentStatusCode);
                    _DatabaseFactory.AddInParameter(ref cmdSel, "@SASS_Description", DbType.String, argEn.Description);
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
                        sqlCmd = "UPDATE SAS_StudentStatus SET SASS_Code = @SASS_Code, SASS_Description = @SASS_Description, SASS_BlStatus = @SASS_BlStatus, SABR_Code = @SABR_Code, SASS_Status = @SASS_Status, SASS_UpdatedUser = @SASS_UpdatedUser, SASS_UpdatedDtTm = @SASS_UpdatedDtTm WHERE SASS_Code = @SASS_Code";
                        if (!FormHelp.IsBlank(sqlCmd))
                        {
                            DbCommand cmd = _DatabaseFactory.GetDbCommand(Helper.GetDataBaseType, sqlCmd, DataBaseConnectionString);
                            _DatabaseFactory.AddInParameter(ref cmd, "@SASS_Code", DbType.String, argEn.StudentStatusCode);
                            _DatabaseFactory.AddInParameter(ref cmd, "@SASS_Description", DbType.String, argEn.Description);
                            _DatabaseFactory.AddInParameter(ref cmd, "@SASS_BlStatus", DbType.Boolean, argEn.BlStatus);
                            _DatabaseFactory.AddInParameter(ref cmd, "@SABR_Code", DbType.Int32, argEn.Code);
                            _DatabaseFactory.AddInParameter(ref cmd, "@SASS_Status", DbType.Boolean, argEn.Status);
                            _DatabaseFactory.AddInParameter(ref cmd, "@SASS_UpdatedUser", DbType.String, argEn.UpdatedUser);
                            _DatabaseFactory.AddInParameter(ref cmd, "@SASS_UpdatedDtTm", DbType.String, argEn.UpdatedDtTm);
                            _DbParameterCollection = cmd.Parameters;

                            int liRowAffected = _DatabaseFactory.ExecuteNonQuery(Helper.GetDataBaseType, cmd,
                                DataBaseConnectionString, sqlCmd, _DbParameterCollection);
                            eobjSCAccess.StudentCategoryCode = argEn.StudentStatusCode;
                            //Deleting any existing StudentCategoryAccess for this studentstatus
                            dobjSCAccess.Delete(eobjSCAccess);
                            int i = 0;
                            //Inserting  StudentCategoryAccess for this studentstatus
                            while (i < argEn.LstStudentCategoryAccess.Count)
                            {
                                eobjSCAccess = argEn.LstStudentCategoryAccess[i];
                                dobjSCAccess.Insert(eobjSCAccess);
                                eobjSCAccess = null;
                                i = i + 1;
                            }

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
        /// Method to Insert StudentStatus 
        /// </summary>
        /// <param name="argEn">StudentStatus Entity is an Input.StudentStatusCode as Input Property.</param>
        /// <returns>Returns Boolean</returns> 
        public bool Delete(StudentStatusEn argEn)
        {
            bool lbRes = false;
            int rows = 0;
            string sqlCmd = "select count(*) as rows  from SAS_Student WHERE SASS_Code = @SASS_Code";

            try
            {
                if (!FormHelp.IsBlank(sqlCmd))
                {
                    DbCommand cmdSel = _DatabaseFactory.GetDbCommand(Helper.GetDataBaseType, sqlCmd, DataBaseConnectionString);
                 _DatabaseFactory.AddInParameter(ref cmdSel, "@SASS_Code", DbType.String, argEn.StudentStatusCode);
                 _DbParameterCollection = cmdSel.Parameters;

                 using (IDataReader dr = _DatabaseFactory.GetIDataReader(Helper.GetDataBaseType, cmdSel,
                        DataBaseConnectionString, sqlCmd, _DbParameterCollection).CreateDataReader()) 
                 {
                     if (dr.Read())
                         rows = clsGeneric.NullToInteger(dr["rows"]);
                     if (rows > 0)
                         throw new Exception("Record Already in Use");
                 }
                     if (rows == 0)
                     {
                         sqlCmd = "DELETE FROM SAS_StudentStatus WHERE SASS_Code = @SASS_Code";


                        

                         if (!FormHelp.IsBlank(sqlCmd))
                         {
                             DbCommand cmd = _DatabaseFactory.GetDbCommand(Helper.GetDataBaseType, sqlCmd, DataBaseConnectionString);
                             _DatabaseFactory.AddInParameter(ref cmd, "@SASS_Code", DbType.String, argEn.StudentStatusCode);
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
        /// Method to Load StudentStatus Entity 
        /// </summary>
        /// <param name="argReader">IDataReader Object is an Input.</param>
        /// <returns>Returns StudentStatus Entity</returns>
        private StudentStatusEn LoadObject(IDataReader argReader)
        {
            StudentStatusEn loItem = new StudentStatusEn();
            loItem.StudentStatusCode = GetValue<string>(argReader, "SASS_Code");
            loItem.Description = GetValue<string>(argReader, "SASS_Description");
            loItem.BlStatus = GetValue<bool>(argReader, "SASS_BlStatus");
            loItem.Status = GetValue<bool>(argReader, "SASS_Status");
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
