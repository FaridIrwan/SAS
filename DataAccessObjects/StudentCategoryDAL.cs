
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
    /// Class to handle all the StudentCategory Methods.
    /// </summary>
    public class StudentCategoryDAL
    {
        #region Global Declarations 

        private DbParameterCollection _DbParameterCollection = null;

        private MaxModule.DatabaseProvider _DatabaseFactory =
            new MaxModule.DatabaseProvider();

        private string DataBaseConnectionString = Helper.
           GetConnectionString();

        #endregion

        public StudentCategoryDAL()
        {
        }

        #region GetList 

        /// <summary>
        /// Method to Get List of StudentCategory
        /// </summary>
        /// <param name="argEn">StudentCategory Entity as an Input.</param>
        /// <returns>Returns List of StudentCategory</returns>
        public List<StudentCategoryEn> GetList(StudentCategoryEn argEn)
        {
            List<StudentCategoryEn> loEnList = new List<StudentCategoryEn>();
            string sqlCmd = "select * from SAS_StudentCategory";
            
            try
            {
                if (!FormHelp.IsBlank(sqlCmd))
                {
                    using (IDataReader loReader = _DatabaseFactory.ExecuteReader(Helper.GetDataBaseType,
                        DataBaseConnectionString, sqlCmd).CreateDataReader())
                    {
                        while (loReader.Read())
                        {
                            StudentCategoryEn loItem = LoadObject(loReader);
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

        #region GetStudentCategoryList 

        /// <summary>
        /// Method to Get List of  Active or Inactive StudentCategory
        /// </summary>
        /// <param name="argEn">StudentCategory Entity as an Input.StudentCategoryCode,Description and Status as Input Properties.</param>
        /// <returns>Returns List of StudentCategory</returns>
        public List<StudentCategoryEn> GetStudentCategoryList(StudentCategoryEn argEn)
        {
            List<StudentCategoryEn> loEnList = new List<StudentCategoryEn>();       
            argEn.StudentCategoryCode = argEn.StudentCategoryCode.Replace("*", "%");
            argEn.Description = argEn.Description.Replace("*", "%");
            string sqlCmd = "select * from SAS_StudentCategory where SASC_Code <> '0'";
            if (argEn.StudentCategoryCode.Length != 0) sqlCmd = sqlCmd + " and SASC_Code like '" + argEn.StudentCategoryCode + "'";
            if (argEn.Description.Length != 0) sqlCmd = sqlCmd + " and SASC_Desc like '" + argEn.Description + "'";
            //if (argEn.Status == true) sqlCmd = sqlCmd + " and SASC_Status =1";
            if (argEn.Status == true) sqlCmd = sqlCmd + " and SASC_Status = 'true'";
            //if (argEn.Status == false) sqlCmd = sqlCmd + " and SASC_Status =0";
            if (argEn.Status == false) sqlCmd = sqlCmd + " and SASC_Status = 'false'";
            sqlCmd = sqlCmd + " order by SASC_Code";
            
            
            try
            {
                if (!FormHelp.IsBlank(sqlCmd))
                {
                    using (IDataReader loReader = _DatabaseFactory.ExecuteReader(Helper.GetDataBaseType,
                        DataBaseConnectionString, sqlCmd).CreateDataReader())
                    {
                        while (loReader.Read())
                        {
                            StudentCategoryEn loItem = LoadObject(loReader);                             
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

        #region GetStudentCategoryListAll 

        /// <summary>
        /// Method to Get List of All StudentCategory
        /// </summary>
        /// <param name="argEn">StudentCategory Entity as an Input.StudentCategoryCode and Description  as Input Properties.</param>
        /// <returns>Returns List of StudentCategory</returns>
        public List<StudentCategoryEn> GetStudentCategoryListAll(StudentCategoryEn argEn)
        {
            List<StudentCategoryEn> loEnList = new List<StudentCategoryEn>();
            argEn.StudentCategoryCode = argEn.StudentCategoryCode.Replace("*", "%");
            argEn.Description = argEn.Description.Replace("*", "%");
            string sqlCmd = "select * from SAS_StudentCategory where SASC_Code <> '0'";
            if (argEn.StudentCategoryCode.Length != 0) sqlCmd = sqlCmd + " and SASC_Code like '" + argEn.StudentCategoryCode + "'";
            if (argEn.Description.Length != 0) sqlCmd = sqlCmd + " and SASC_Desc like '" + argEn.Description + "'";
            sqlCmd = sqlCmd + " order by SASC_Code";
            

            try
            {
                if (!FormHelp.IsBlank(sqlCmd))
                {
                    using (IDataReader loReader = _DatabaseFactory.ExecuteReader(Helper.GetDataBaseType,
                        DataBaseConnectionString, sqlCmd).CreateDataReader())
                    {
                        while (loReader.Read())
                        {
                            StudentCategoryEn loItem = LoadObject(loReader);
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
        /// Method to Get StudentCategory Entity
        /// </summary>
        /// <param name="argEn">StudentCategory Entity is an Input.StudentCategoryCode as Input Property.</param>
        /// <returns>Returns StudentCategory Entity</returns>
        public StudentCategoryEn GetItem(StudentCategoryEn argEn)
        {
            StudentCategoryEn loItem = new StudentCategoryEn();
            string sqlCmd = "Select * FROM SAS_StudentCategory WHERE SASC_Code = @SASC_Code";
            
            try
            {
                if (!FormHelp.IsBlank(sqlCmd))
                {
                    DbCommand cmd = _DatabaseFactory.GetDbCommand(Helper.GetDataBaseType, sqlCmd, DataBaseConnectionString);
                    _DatabaseFactory.AddInParameter(ref cmd, "@SASC_Code", DbType.String, argEn.StudentCategoryCode);
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
        /// Method to Insert StudentCategory 
        /// </summary>
        /// <param name="argEn">StudentCategory Entity is an Input.</param>
        /// <returns>Returns Boolean</returns> 
        public bool Insert(StudentCategoryEn argEn)
        {
            bool lbRes = false;
            int iOut = 0;
            StudentCategoryAccessEn eobjSCAccess = new StudentCategoryAccessEn();
            StudentCategoryAccessDAL dobjSCAccess=new StudentCategoryAccessDAL(); 
            string sqlCmd = "Select count(*) as cnt From SAS_StudentCategory WHERE SASC_Code = @SASC_Code or SASC_Desc = @SASC_Desc";
            try
            {
                if (!FormHelp.IsBlank(sqlCmd))
                {
                    DbCommand cmdSel = _DatabaseFactory.GetDbCommand(Helper.GetDataBaseType, sqlCmd, DataBaseConnectionString);
                    _DatabaseFactory.AddInParameter(ref cmdSel, "@SASC_Code", DbType.String, argEn.StudentCategoryCode);
                    _DatabaseFactory.AddInParameter(ref cmdSel, "@SASC_Desc", DbType.String, argEn.Description);
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
                        sqlCmd = "INSERT INTO SAS_StudentCategory(SASC_Code,SASC_Desc,SABR_Code,SASC_Status,SASC_UpdatedBy,SASC_UpdatedDtTm) VALUES (@SASC_Code,@SASC_Desc,@SABR_Code,@SASC_Status,@SASC_UpdatedBy,@SASC_UpdatedDtTm) ";

                        if (!FormHelp.IsBlank(sqlCmd))
                        {
                            DbCommand cmd = _DatabaseFactory.GetDbCommand(Helper.GetDataBaseType, sqlCmd, DataBaseConnectionString);
                            _DatabaseFactory.AddInParameter(ref cmd, "@SASC_Code", DbType.String, argEn.StudentCategoryCode);
                            _DatabaseFactory.AddInParameter(ref cmd, "@SASC_Desc", DbType.String, argEn.Description);
                            _DatabaseFactory.AddInParameter(ref cmd, "@SABR_Code", DbType.Int32, argEn.Code);
                            _DatabaseFactory.AddInParameter(ref cmd, "@SASC_Status", DbType.Boolean, argEn.Status);
                            _DatabaseFactory.AddInParameter(ref cmd, "@SASC_UpdatedBy", DbType.String, argEn.UpdatedBy);
                            _DatabaseFactory.AddInParameter(ref cmd, "@SASC_UpdatedDtTm", DbType.String, argEn.UpdatedDtTm);
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
        /// Method to Update StudentCategory 
        /// </summary>
        /// <param name="argEn">StudentCategory Entity is an Input.</param>
        /// <returns>Returns Boolean</returns> 
        public bool Update(StudentCategoryEn argEn)
        {
            bool lbRes = false;
            int iOut = 0;
            StudentCategoryAccessEn eobjSCAccess = new StudentCategoryAccessEn();
            StudentCategoryAccessDAL dobjSCAccess = new StudentCategoryAccessDAL(); 
            string sqlCmd = "Select count(*) as cnt From SAS_StudentCategory WHERE SASC_Code != @SASC_Code and SASC_Desc = @SASC_Desc";
            try
            {
                if (!FormHelp.IsBlank(sqlCmd))
                {
                    DbCommand cmdSel = _DatabaseFactory.GetDbCommand(Helper.GetDataBaseType, sqlCmd, DataBaseConnectionString);
                    _DatabaseFactory.AddInParameter(ref cmdSel, "@SASC_Code", DbType.String, argEn.StudentCategoryCode);
                    _DatabaseFactory.AddInParameter(ref cmdSel, "@SASC_Desc", DbType.String, argEn.Description);
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
                        sqlCmd = "UPDATE SAS_StudentCategory SET SASC_Code = @SASC_Code, SASC_Desc = @SASC_Desc, SABR_Code = @SABR_Code, SASC_Status = @SASC_Status, SASC_UpdatedBy = @SASC_UpdatedBy, SASC_UpdatedDtTm = @SASC_UpdatedDtTm WHERE SASC_Code = @SASC_Code";
                        if (!FormHelp.IsBlank(sqlCmd))
                        {
                            DbCommand cmd = _DatabaseFactory.GetDbCommand(Helper.GetDataBaseType, sqlCmd, DataBaseConnectionString);
                            _DatabaseFactory.AddInParameter(ref cmd, "@SASC_Code", DbType.String, argEn.StudentCategoryCode);
                            _DatabaseFactory.AddInParameter(ref cmd, "@SASC_Desc", DbType.String, argEn.Description);
                            _DatabaseFactory.AddInParameter(ref cmd, "@SABR_Code", DbType.Int32, argEn.Code);
                            _DatabaseFactory.AddInParameter(ref cmd, "@SASC_Status", DbType.Boolean, argEn.Status);
                            _DatabaseFactory.AddInParameter(ref cmd, "@SASC_UpdatedBy", DbType.String, argEn.UpdatedBy);
                            _DatabaseFactory.AddInParameter(ref cmd, "@SASC_UpdatedDtTm", DbType.String, argEn.UpdatedDtTm);
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
        /// Method to Delete StudentCategory 
        /// </summary>
        /// <param name="argEn">StudentCategory Entity is an Input.StudentCategoryCode as Input Property.</param>
        /// <returns>Returns Boolean</returns> 
        public bool Delete(StudentCategoryEn argEn)
        {
            bool lbRes = false;
            int iOut = 0;
            string sqlCmd = "Select count(*) as cnt From   SAS_FeeCharges INNER JOIN SAS_StudentCategory ON SAS_FeeCharges.SASC_Code = SAS_StudentCategory.SASC_Code WHERE SAS_StudentCategory.SASC_Code = @SASC_Code ";
             try
             {
                 if (!FormHelp.IsBlank(sqlCmd))
                 {
                     DbCommand cmdSel = _DatabaseFactory.GetDbCommand(Helper.GetDataBaseType, sqlCmd, DataBaseConnectionString);
                     _DatabaseFactory.AddInParameter(ref cmdSel, "@SASC_Code", DbType.String, argEn.StudentCategoryCode);
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
                         sqlCmd = "DELETE FROM SAS_StudentCategory WHERE SASC_Code = @SASC_Code";
                         if (!FormHelp.IsBlank(sqlCmd))
                         {
                             DbCommand cmd = _DatabaseFactory.GetDbCommand(Helper.GetDataBaseType, sqlCmd, DataBaseConnectionString);
                             _DatabaseFactory.AddInParameter(ref cmd, "@SASC_Code", DbType.String, argEn.StudentCategoryCode);
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
        /// Method to Load StudentCategory Entity 
        /// </summary>
        /// <param name="argReader">IDataReader Object is an Input.</param>
        /// <returns>Returns StudentCategory Entity</returns>
        private StudentCategoryEn LoadObject(IDataReader argReader)
        {
            StudentCategoryEn loItem = new StudentCategoryEn();
            loItem.StudentCategoryCode = GetValue<string>(argReader, "SASC_Code");
            loItem.Description = GetValue<string>(argReader, "SASC_Desc");
            loItem.Code = GetValue<int>(argReader, "SABR_Code");
            loItem.Status = GetValue<bool>(argReader, "SASC_Status");
            loItem.UpdatedBy = GetValue<string>(argReader, "SASC_UpdatedBy");
            loItem.UpdatedDtTm = GetValue<string>(argReader, "SASC_UpdatedDtTm");

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
