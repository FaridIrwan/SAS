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
    /// Class to handle all the StudentCategoryAccess Methods.
    /// </summary>
    public class StudentCategoryAccessDAL
    {
        #region Global Declarations 

        private DbParameterCollection _DbParameterCollection = null;

        private MaxModule.DatabaseProvider _DatabaseFactory =
            new MaxModule.DatabaseProvider();

        private string DataBaseConnectionString = Helper.
           GetConnectionString();

        #endregion

        public StudentCategoryAccessDAL()
        {
        }

        #region GetList 

        /// <summary>
        /// Method to Get List of StudentCategoryAccess
        /// </summary>
        /// <param name="argEn">StudentCategoryAccess Entity as an Input.</param>
        /// <returns>Returns List of StudentCategoryAccess</returns>
        public List<StudentCategoryAccessEn> GetList(StudentCategoryAccessEn argEn)
        {
            List<StudentCategoryAccessEn> loEnList = new List<StudentCategoryAccessEn>();
            string sqlCmd = "select * from SAS_StudentCategoryAccess";
            
            try
            {
                if (!FormHelp.IsBlank(sqlCmd))
                {
                    using (IDataReader loReader = _DatabaseFactory.ExecuteReader(Helper.GetDataBaseType,
                        DataBaseConnectionString, sqlCmd).CreateDataReader())
                    {
                        while (loReader.Read())
                        {
                            StudentCategoryAccessEn loItem = LoadObject(loReader);                              
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

        #region GetStuCatAccessList 

        /// <summary>
        /// Method to Get List of All StudentCategoryAccess
        /// </summary>
        /// <param name="argEn">StudentCategoryAccess Entity as an Input.StudentCategoryCode as Input Property.</param>
        /// <returns>Returns List of StudentCategoryAccess</returns>
        public List<StudentCategoryAccessEn> GetStuCatAccessList(StudentCategoryAccessEn argEn)
        {
            List<StudentCategoryAccessEn> loEnList = new List<StudentCategoryAccessEn>();
            string sqlCmd = "select SCA.MenuID,MM.MenuName,MM.PageName,SCA.Status,SCA.SASC_Code" + 
                            " from SAS_StudentCategoryAccess SCA INNER JOIN UR_MenuMaster MM" +
                            " on SCA.MenuID=MM.MenuID where MM.MenuID <> 114 and SCA.SASC_Code=@SASC_Code";
            
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
                        while (loReader.Read())
                        {
                            StudentCategoryAccessEn loItem = LoadObject(loReader);
                            loItem.MenuName = GetValue<string>(loReader, "MenuName");
                            loItem.PageName = GetValue<string>(loReader, "PageName");
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

        #region GetMenuList 

        /// <summary>
        /// Method to Get List of Menus
        /// </summary>
        /// <param name="argEn">StudentCategoryAccess Entity as an Input.MenuName as Input Property.</param>
        /// <returns>Returns List of StudentCategoryAccess</returns>
        public List<StudentCategoryAccessEn> GetMenuList(StudentCategoryAccessEn argEn)
        {
            List<StudentCategoryAccessEn> loEnList = new List<StudentCategoryAccessEn>();
            string sqlCmd = "select * from UR_MenuMaster where MenuID <> 114 AND status = true AND MenuName= '" + argEn.MenuName + "'";
            try
            {
                if (!FormHelp.IsBlank(sqlCmd))
                {
                    using (IDataReader loReader = _DatabaseFactory.ExecuteReader(Helper.GetDataBaseType,
                        DataBaseConnectionString, sqlCmd).CreateDataReader())
                    {
                        while (loReader.Read())
                        {
                            StudentCategoryAccessEn loItem = LoadMenuObject(loReader);
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
        /// Method to Get StudentCategoryAccess Entity
        /// </summary>
        /// <param name="argEn">StudentCategoryAccess Entity is an Input.StudentCategoryCode as Input Property.</param>
        /// <returns>Returns StudentCategoryAccess Entity</returns>
        public StudentCategoryAccessEn GetItem(StudentCategoryAccessEn argEn)
        {
            StudentCategoryAccessEn loItem = new StudentCategoryAccessEn();
            string sqlCmd = "Select * FROM SAS_StudentCategoryAccess WHERE SASC_Code = @SASC_Code";
            
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
        /// Method to Insert StudentCategoryAccess 
        /// </summary>
        /// <param name="argEn">StudentCategoryAccess Entity is an Input.</param>
        /// <returns>Returns Boolean</returns> 
        public bool Insert(StudentCategoryAccessEn argEn)
        {
            bool lbRes = false;
            string sqlCmd = "INSERT INTO SAS_StudentCategoryAccess(SASC_Code,MenuID,Status) VALUES (@SASC_Code,@MenuID,@Status) ";
            try
            {
                if (!FormHelp.IsBlank(sqlCmd))
                {
                    DbCommand cmd = _DatabaseFactory.GetDbCommand(Helper.GetDataBaseType, sqlCmd, DataBaseConnectionString);
                    _DatabaseFactory.AddInParameter(ref cmd, "@SASC_Code", DbType.String, argEn.StudentCategoryCode);
                    _DatabaseFactory.AddInParameter(ref cmd, "@MenuID", DbType.Int32, argEn.MenuID);
                    _DatabaseFactory.AddInParameter(ref cmd, "@Status", DbType.Boolean, argEn.Status);
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

        #region Update

        /// <summary>
        /// Method to Update StudentCategoryAccess 
        /// </summary>
        /// <param name="argEn">StudentCategoryAccess Entity is an Input.StudentCategoryCode,MenuID and Status as Input Properties.</param>
        /// <returns>Returns Boolean</returns> 
        public bool Update(StudentCategoryAccessEn argEn)
        {
            bool lbRes = false;
            try
            {
                string sqlCmd = "UPDATE SAS_StudentCategoryAccess SET SASC_Code = @SASC_Code, MenuID = @MenuID, Status = @Status WHERE SASC_Code = @SASC_Code";
                
                if (!FormHelp.IsBlank(sqlCmd))
                {
                    DbCommand cmd = _DatabaseFactory.GetDbCommand(Helper.GetDataBaseType, sqlCmd, DataBaseConnectionString);
                    _DatabaseFactory.AddInParameter(ref cmd, "@SASC_Code", DbType.String, argEn.StudentCategoryCode);
                    _DatabaseFactory.AddInParameter(ref cmd, "@MenuID", DbType.Int32, argEn.MenuID);
                    _DatabaseFactory.AddInParameter(ref cmd, "@Status", DbType.Boolean, argEn.Status);
                    _DbParameterCollection = cmd.Parameters;

                    int liRowAffected = _DatabaseFactory.ExecuteNonQuery(Helper.GetDataBaseType, cmd,
                                DataBaseConnectionString, sqlCmd, _DbParameterCollection);
                    
                    if (liRowAffected > -1)
                        lbRes = true;
                    else
                        throw new Exception("Update Failed! No Row has been updated...");
                }
                //    }
                //}
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
        /// Method to Delete StudentCategoryAccess 
        /// </summary>
        /// <param name="argEn">StudentCategoryAccess Entity is an Input.StudentCategoryCode as Input Property.</param>
        /// <returns>Returns Boolean</returns> 
        public bool Delete(StudentCategoryAccessEn argEn)
        {
            bool lbRes = false;
            string sqlCmd = "DELETE FROM SAS_StudentCategoryAccess WHERE SASC_Code = @SASC_Code";
            
            try
            {
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
        /// Method to Load StudentCategoryAccess Entity 
        /// </summary>
        /// <param name="argReader">IDataReader Object is an Input.</param>
        /// <returns>Returns StudentCategoryAccess Entity</returns>

        private StudentCategoryAccessEn LoadObject(IDataReader argReader)
        {
            StudentCategoryAccessEn loItem = new StudentCategoryAccessEn();
            loItem.StudentCategoryCode = GetValue<string>(argReader, "SASC_Code");
            loItem.MenuID = GetValue<int>(argReader, "MenuID");
            loItem.Status = GetValue<bool>(argReader, "Status");

            return loItem;
        }
        /// <summary>
        /// Method to Load Menus
        /// </summary>
        /// <param name="argReader">IDataReader Object is an Input.</param>
        /// <returns>Returns StudentCategoryAccess Entity</returns>
        private StudentCategoryAccessEn LoadMenuObject(IDataReader argReader)
        {
            StudentCategoryAccessEn loItem = new StudentCategoryAccessEn();
            //loItem.StudentCategoryCode = GetValue<string>(argReader, "SASC_Code");
            loItem.MenuID = GetValue<int>(argReader, "MenuID");
            loItem.MenuName= GetValue<string>(argReader, "MenuName");
            loItem.PageName = GetValue<string>(argReader, "PageName");
            loItem.Status = false;

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
