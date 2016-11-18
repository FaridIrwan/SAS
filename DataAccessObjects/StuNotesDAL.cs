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
    /// Class to handle all the StudentNotes Methods.
    /// </summary>
    public class StuNotesDAL
    {
        #region Global Declarations 

        private DbParameterCollection _DbParameterCollection = null;

        private MaxModule.DatabaseProvider _DatabaseFactory =
            new MaxModule.DatabaseProvider();

        private string DataBaseConnectionString = Helper.
           GetConnectionString();

        #endregion

        public StuNotesDAL()
        {
        }

        #region GetList 

        /// <summary>
        /// Method to Get List of StudentNotes
        /// </summary>
        /// <param name="argEn">StudentNotes Entity as an Input.Student MatricNo as Input Property</param>
        /// <returns>Returns List of StudentNotes</returns>
        public List<StuNotesEn> GetList(StuNotesEn argEn)
        {
            List<StuNotesEn> loEnList = new List<StuNotesEn>();
            string sqlCmd = "select * from SAS_StuNotes where SASI_MatricNo = '"+argEn.MatricNo+"'";
            try
            {
                if (!FormHelp.IsBlank(sqlCmd))
                {
                    using (IDataReader loReader = _DatabaseFactory.ExecuteReader(Helper.GetDataBaseType,
                        DataBaseConnectionString, sqlCmd).CreateDataReader())
                    {
                        while (loReader.Read())
                        {
                            StuNotesEn loItem = LoadObject(loReader);
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
        /// Method to Get StudentNotes Entity
        /// </summary>
        /// <param name="argEn">StudentNotes Entity is an Input.MatricNo as Input Property.</param>
        /// <returns>Returns StudentNotes Entity</returns>
        public StuNotesEn GetItem(StuNotesEn argEn)
        {
            StuNotesEn loItem = new StuNotesEn();
            string sqlCmd = "Select * FROM SAS_StuNotes WHERE SASI_MatricNo = @SASI_MatricNo";
            try
            {
                if (!FormHelp.IsBlank(sqlCmd))
                {
                    DbCommand cmd = _DatabaseFactory.GetDbCommand(Helper.GetDataBaseType, sqlCmd, DataBaseConnectionString);
                    _DatabaseFactory.AddInParameter(ref cmd, "@SASI_MatricNo", DbType.String, argEn.MatricNo);
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
        /// Method to Insert StudentNotes
        /// </summary>
        /// <param name="argEn">StudentNotes Entity is an Input.</param>
        /// <returns>Returns Boolean</returns> 
        public bool Insert(StuNotesEn argEn)
        {
            bool lbRes = false;
            int iOut = 0;
            string sqlCmd = "Select count(*) as cnt From SAS_StuNotes WHERE SASI_MatricNo = @SASI_MatricNo";
            try
            {
                sqlCmd = "INSERT INTO SAS_StuNotes(SASN_Code,SASI_MatricNo,SASN_Remarks,SASN_UpdatedBy,SASN_UpdatedDtTm) VALUES (@SASN_Code,@SASI_MatricNo,@SASN_Remarks,@SASN_UpdatedBy,@SASN_UpdatedDtTm) ";
                if (!FormHelp.IsBlank(sqlCmd))
                {
                    DbCommand cmd = _DatabaseFactory.GetDbCommand(Helper.GetDataBaseType, sqlCmd, DataBaseConnectionString);
                    _DatabaseFactory.AddInParameter(ref cmd, "@SASN_Code", DbType.Int32, argEn.SASN_Code);
                    _DatabaseFactory.AddInParameter(ref cmd, "@SASI_MatricNo", DbType.String, argEn.MatricNo);
                    _DatabaseFactory.AddInParameter(ref cmd, "@SASN_Remarks", DbType.String, argEn.Remarks);
                    _DatabaseFactory.AddInParameter(ref cmd, "@SASN_UpdatedBy", DbType.String, argEn.SASN_UpdatedBy);
                    _DatabaseFactory.AddInParameter(ref cmd, "@SASN_UpdatedDtTm", DbType.String, argEn.SASN_UpdatedDtTm);
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
        /// Method to Update StudentNotes
        /// </summary>
        /// <param name="argEn">StudentNotes Entity is an Input.</param>
        /// <returns>Returns Boolean</returns> 
        public bool Update(StuNotesEn argEn)
        {
            bool lbRes = false;
            int iOut = 0;
            string sqlCmd = "Select count(*) as cnt From SAS_StuNotes WHERE ";
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
                            throw new Exception("Update Failed! No Record Exist!");
                    }
                    if (iOut == 0)
                    {
                        sqlCmd = "UPDATE SAS_StuNotes SET SASN_Code = @SASN_Code, SASI_MatricNo = @SASI_MatricNo, SASN_Remarks = @SASN_Remarks, SASN_UpdatedBy = @SASN_UpdatedBy, SASN_UpdatedDtTm = @SASN_UpdatedDtTm WHERE SASI_MatricNo = @SASI_MatricNo";
                        if (!FormHelp.IsBlank(sqlCmd))
                        {
                            DbCommand cmd = _DatabaseFactory.GetDbCommand(Helper.GetDataBaseType, sqlCmd, DataBaseConnectionString);
                            _DatabaseFactory.AddInParameter(ref cmd, "@SASN_Code", DbType.Int32, argEn.SASN_Code);
                            _DatabaseFactory.AddInParameter(ref cmd, "@SASI_MatricNo", DbType.String, argEn.MatricNo);
                            _DatabaseFactory.AddInParameter(ref cmd, "@SASN_Remarks", DbType.String, argEn.Remarks);
                            _DatabaseFactory.AddInParameter(ref cmd, "@SASN_UpdatedBy", DbType.String, argEn.SASN_UpdatedBy);
                            _DatabaseFactory.AddInParameter(ref cmd, "@SASN_UpdatedDtTm", DbType.String, argEn.SASN_UpdatedDtTm);
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
        /// Method to Delete StudentNotes
        /// </summary>
        /// <param name="argEn">StudentNotes Entity is an Input.MatricNo as Input Property.</param>
        /// <returns>Returns Boolean</returns> 
        public bool Delete(StuNotesEn argEn)
        {
            bool lbRes = false;
            string sqlCmd = "DELETE FROM SAS_StuNotes WHERE SASI_MatricNo = @SASI_MatricNo";
            try
            {
                if (!FormHelp.IsBlank(sqlCmd))
                {
                    DbCommand cmd = _DatabaseFactory.GetDbCommand(Helper.GetDataBaseType, sqlCmd, DataBaseConnectionString);
                    _DatabaseFactory.AddInParameter(ref cmd, "@SASI_MatricNo", DbType.String, argEn.MatricNo);
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
        /// Method to Load StudentNotes Entity 
        /// </summary>
        /// <param name="argReader">IDataReader Object is an Input.</param>
        /// <returns>Returns StudentNotes Entity</returns>
        private StuNotesEn LoadObject(IDataReader argReader)
        {
            StuNotesEn loItem = new StuNotesEn();
            loItem.SASN_Code = GetValue<int>(argReader, "SASN_Code");
            loItem.MatricNo = GetValue<string>(argReader, "SASI_MatricNo");
            loItem.Remarks = GetValue<string>(argReader, "SASN_Remarks");
            loItem.SASN_UpdatedBy = GetValue<string>(argReader, "SASN_UpdatedBy");
            loItem.SASN_UpdatedDtTm = GetValue<string>(argReader, "SASN_UpdatedDtTm");

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

