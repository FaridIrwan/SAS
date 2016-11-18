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
    /// Class to handle all the ProgramAccount Methods.
    /// </summary>
    public class ProgramAccountDAL
    {
        #region Global Declarations 

        private DbParameterCollection _DbParameterCollection = null;

        private MaxModule.DatabaseProvider _DatabaseFactory =
            new MaxModule.DatabaseProvider();

        private string DataBaseConnectionString = Helper.
           GetConnectionString();

        #endregion

        public ProgramAccountDAL()
        {
        }

        #region GetList 

        /// <summary>
        /// Method to Get List of ProgramAccount
        /// </summary>
        /// <param name="argEn">ProgramAccount Entity is an Input.</param>
        /// <returns>Returns List of ProgramAccount</returns>
        public List<ProgramAccountEn> GetList(ProgramAccountEn argEn)
        {
            List<ProgramAccountEn> loEnList = new List<ProgramAccountEn>();
            string sqlCmd = "select * from SAS_Program";
            
            try
            {
                if (!FormHelp.IsBlank(sqlCmd))
                {
                    using (IDataReader loReader = _DatabaseFactory.ExecuteReader(Helper.GetDataBaseType,
                        DataBaseConnectionString, sqlCmd).CreateDataReader())
                    {
                        while (loReader.Read())
                        {
                            ProgramAccountEn loItem = LoadObject(loReader);
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

        #region GetListProgram 

        /// <summary>
        /// Method to Get List of ProgramAccount
        /// </summary>
        /// <param name="argEn">ProgramAccount Entity is an Input.</param>
        /// <returns>Returns List of ProgramAccount</returns>
        public List<ProgramAccountEn> GetListProgram(ProgramAccountEn argEn)
        {
            List<ProgramAccountEn> loEnList = new List<ProgramAccountEn>();
            string sqlCmd = "select * from SAS_Program order by SAPG_Code";
            
            try
            {
                if (!FormHelp.IsBlank(sqlCmd))
                {
                    using (IDataReader loReader = _DatabaseFactory.ExecuteReader(Helper.GetDataBaseType,
                        DataBaseConnectionString, sqlCmd).CreateDataReader())
                    {
                        while (loReader.Read())
                        {
                            ProgramAccountEn loItem = LoadObject1(loReader);
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

        #region GetListProgramCombine 

        /// <summary>
        /// Method to Get List of ProgramAccount
        /// </summary>
        /// <param name="argEn">ProgramAccount Entity is an Input.</param>
        /// <returns>Returns List of ProgramAccount</returns>
        public List<ProgramAccountEn> GetListProgramCombine(ProgramAccountEn argEn)
        {
            List<ProgramAccountEn> loEnList = new List<ProgramAccountEn>();
            string sqlCmd = "select PR.SAPG_ProgramBM as descProgram from SAS_Program PR UNION ";
            sqlCmd += "Select 'AFC-' || FC.SAFC_Desc from SAS_Faculty FC UNION ";
            sqlCmd += "Select 'AFC-' || SS.SAST_Code from SAS_SemesterSetup SS";
            
            try
            {
                if (!FormHelp.IsBlank(sqlCmd))
                {
                    using (IDataReader loReader = _DatabaseFactory.ExecuteReader(Helper.GetDataBaseType,
                        DataBaseConnectionString, sqlCmd).CreateDataReader())
                    {
                        while (loReader.Read())
                        {
                            ProgramAccountEn loItem = LoadObject2(loReader);
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
        /// Method to Get ProgramAccount Entity
        /// </summary>
        /// <param name="argEn">ProgramAccount Entity is an Input.SAPA_Code as Input Property.</param>
        /// <returns>Returns ProgramAccount Entity</returns>
        public ProgramAccountEn GetItem(ProgramAccountEn argEn)
        {
            ProgramAccountEn loItem = new ProgramAccountEn();
            string sqlCmd = "Select * FROM SAS_ProgramAccount WHERE SAPA_Code = @SAPA_Code";
            
            try
            {
                if (!FormHelp.IsBlank(sqlCmd))
                {
                    DbCommand cmd = _DatabaseFactory.GetDbCommand(Helper.GetDataBaseType, sqlCmd, DataBaseConnectionString);
                    _DatabaseFactory.AddInParameter(ref cmd, "@SAPA_Code", DbType.String, argEn.ProgramAccountCode);
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
        /// Method to Insert ProgramAccount 
        /// </summary>
        /// <param name="argEn">ProgramAccount Entity is an Input.</param>
        /// <returns>Returns Boolean</returns>
        public bool Insert(ProgramAccountEn argEn)
        {
            bool lbRes = false;
            int iOut = 0;
            string sqlCmd = "Select count(*) as cnt From SAS_ProgramAccount WHERE ";
            try
            {
                if (!FormHelp.IsBlank(sqlCmd))
                {
                    using (IDataReader dr = _DatabaseFactory.ExecuteReader(Helper.GetDataBaseType,
                        DataBaseConnectionString, sqlCmd).CreateDataReader())
                    {
                        if (dr.Read())
                            iOut = clsGeneric.NullToInteger(dr["cnt"]);
                        if (iOut > 0)
                            throw new Exception("Record Already Exist");
                    }
                    if (iOut == 0)
                    {
                        sqlCmd = "INSERT INTO SAS_ProgramAccount(SAPA_Code,SAPA_Desc,SAPA_Status,SAPG_Code) VALUES (@SAPA_Code,@SAPA_Desc,@SAPA_Status,@SAPG_Code) ";

                        if (!FormHelp.IsBlank(sqlCmd))
                        {
                            DbCommand cmd = _DatabaseFactory.GetDbCommand(Helper.GetDataBaseType, sqlCmd, DataBaseConnectionString);
                            _DatabaseFactory.AddInParameter(ref cmd, "@SAPA_Code", DbType.String, argEn.ProgramAccountCode);
                            _DatabaseFactory.AddInParameter(ref cmd, "@SAPA_Desc", DbType.String, argEn.ProgramAccDescription);
                            _DatabaseFactory.AddInParameter(ref cmd, "@SAPA_Status", DbType.Boolean, argEn.ProgramAccStatus);
                            _DatabaseFactory.AddInParameter(ref cmd, "@SAPG_Code", DbType.String, argEn.ProgramCode);
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
        /// Method to Update ProgramAccount 
        /// </summary>
        /// <param name="argEn">ProgramAccount Entity is an Input.</param>
        /// <returns>Returns Boolean</returns>
        public bool Update(ProgramAccountEn argEn)
        {
            bool lbRes = false;
            int iOut = 0;
            string sqlCmd = "Select count(*) as cnt From SAS_ProgramAccount WHERE ";
            try
            {
                if (!FormHelp.IsBlank(sqlCmd))
                {
                    using (IDataReader dr = _DatabaseFactory.ExecuteReader(Helper.GetDataBaseType,
                        DataBaseConnectionString, sqlCmd).CreateDataReader())
                    {
                        if (dr.Read())
                            iOut = clsGeneric.NullToInteger(dr["cnt"]);
                        if (iOut > 0)
                            throw new Exception("Record Already Exist");
                    }
                    if (iOut == 0)
                    {
                        sqlCmd = "UPDATE SAS_ProgramAccount SET SAPA_Code = @SAPA_Code, SAPA_Desc = @SAPA_Desc, SAPA_Status = @SAPA_Status, SAPG_Code = @SAPG_Code WHERE SAPA_Code = @SAPA_Code";

                        if (!FormHelp.IsBlank(sqlCmd))
                        {
                            DbCommand cmd = _DatabaseFactory.GetDbCommand(Helper.GetDataBaseType, sqlCmd, DataBaseConnectionString);
                            _DatabaseFactory.AddInParameter(ref cmd, "@SAPA_Code", DbType.String, argEn.ProgramAccountCode);
                            _DatabaseFactory.AddInParameter(ref cmd, "@SAPA_Desc", DbType.String, argEn.ProgramAccDescription);
                            _DatabaseFactory.AddInParameter(ref cmd, "@SAPA_Status", DbType.Boolean, argEn.ProgramAccStatus);
                            _DatabaseFactory.AddInParameter(ref cmd, "@SAPG_Code", DbType.String, argEn.ProgramCode);
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
        /// Method to Delete ProgramAccount 
        /// </summary>
        /// <param name="argEn">ProgramAccount Entity is an Input.</param>
        /// <returns>Returns Boolean</returns>
        public bool Delete(ProgramAccountEn argEn)
        {
            bool lbRes = false;
            string sqlCmd = "DELETE FROM SAS_ProgramAccount WHERE SAPA_Code = @SAPA_Code";
            
            try
            {
                if (!FormHelp.IsBlank(sqlCmd))
                {
                    DbCommand cmd = _DatabaseFactory.GetDbCommand(Helper.GetDataBaseType, sqlCmd, DataBaseConnectionString);
                    _DatabaseFactory.AddInParameter(ref cmd, "@SAPA_Code", DbType.String, argEn.ProgramAccountCode);
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
        /// Method to Load ProgramAccount Entity 
        /// </summary>
        /// <param name="argReader">IDataReader Object is an Input.</param>
        /// <returns>Returns ProgramAccount Entity</returns>
        private ProgramAccountEn LoadObject(IDataReader argReader)
        {
            ProgramAccountEn loItem = new ProgramAccountEn();
            loItem.ProgramAccountCode = GetValue<string>(argReader, "SAPA_Code");
            loItem.ProgramAccDescription = GetValue<string>(argReader, "SAPA_Desc");
            loItem.ProgramAccStatus = GetValue<bool>(argReader, "SAPA_Status");
            loItem.ProgramCode = GetValue<string>(argReader, "SAPG_Code");

            return loItem;
        }
        /// <summary>
        /// Method to Load ProgramAccount Entity 
        /// </summary>
        /// <param name="argReader">IDataReader Object is an Input.</param>
        /// <returns>Returns ProgramAccount Entity</returns>
        private ProgramAccountEn LoadObject1(IDataReader argReader)
        {
            ProgramAccountEn loItem = new ProgramAccountEn();
            loItem.ProgramCode = GetValue<string>(argReader, "SAPG_Code");
            loItem.descProgram = GetValue<string>(argReader, "SAPG_ProgramBM");
            //loItem.CodeProgram = loItem.ProgramCode + "-" + loItem.descProgram;
            loItem.CodeProgram = loItem.descProgram;
            return loItem;
        }
         /// <summary>
        /// Method to Load ProgramAccount Entity 
        /// </summary>
        /// <param name="argReader">IDataReader Object is an Input.</param>
        /// <returns>Returns ProgramAccount Entity</returns>
        private ProgramAccountEn LoadObject2(IDataReader argReader)
        {
            ProgramAccountEn loItem = new ProgramAccountEn();
            loItem.descProgram = GetValue<string>(argReader, "descProgram");

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


