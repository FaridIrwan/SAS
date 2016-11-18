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
    /// Class to handle all the FeeStructureAmount Methods.
    /// </summary>
    public class FeeStrAmountDAL
    {
        #region Global Declarations 

        private DbParameterCollection _DbParameterCollection = null;

        private MaxModule.DatabaseProvider _DatabaseFactory =
            new MaxModule.DatabaseProvider();

        private string DataBaseConnectionString = Helper.
           GetConnectionString();

        #endregion

        public FeeStrAmountDAL()
        {
        }

        #region GetFSAmtList 

        /// <summary>
        /// Method to Get List of all FeeStrAmounts
        /// </summary>
        /// <param name="argEn">FeeStrAmount Entity is an Input.FScode,FTCode and Type are Input Properties</param>
        /// <returns>Returns List of FeeStrAmount</returns>
        public List<FeeStrAmountEn> GetFSAmtList(FeeStrAmountEn argEn)
        {
            List<FeeStrAmountEn> loEnList = new List<FeeStrAmountEn>();
            string sqlCmd = "SELECT SAS_StudentCategory.SASC_Desc , SAS_FeeStrAmount.* FROM SAS_FeeStrAmount INNER JOIN " +
                      " SAS_StudentCategory ON SAS_FeeStrAmount.SASC_Code = SAS_StudentCategory.SASC_Code where SAS_FeeStrAmount.SAFS_Code = @SAFS_Code" +
                      " and SAS_FeeStrAmount.SAFD_Type = @SAFD_Type";
               //string sqlCmd = "SELECT SAS_StudentCategory.SASC_Desc , SAS_FeeStrAmount.* FROM SAS_FeeStrAmount INNER JOIN " +
               //       " SAS_StudentCategory ON SAS_FeeStrAmount.SASC_Code = SAS_StudentCategory.SASC_Code where SAS_FeeStrAmount.SAFS_Code = @SAFS_Code" +
               //       " and SAS_FeeStrAmount.SAFT_Code = @SAFT_Code and SAS_FeeStrAmount.SAFD_Type = @SAFD_Type";
            
            try
            {
                if (!FormHelp.IsBlank(sqlCmd))
                {
                    DbCommand cmd = _DatabaseFactory.GetDbCommand(Helper.GetDataBaseType, sqlCmd, DataBaseConnectionString);
                    _DatabaseFactory.AddInParameter(ref cmd, "@SAFS_Code", DbType.String, argEn.FSCode);
                   //_DatabaseFactory.AddInParameter(ref cmd, "@SAFT_Code", DbType.String, argEn.FTCode);
                    _DatabaseFactory.AddInParameter(ref cmd, "@SAFD_Type", DbType.String, argEn.Type);
                    _DbParameterCollection = cmd.Parameters;


                    using (IDataReader loReader = _DatabaseFactory.GetIDataReader(Helper.GetDataBaseType, cmd,
                       DataBaseConnectionString, sqlCmd, _DbParameterCollection).CreateDataReader()) 
                    {
                        while (loReader.Read())
                        {
                            FeeStrAmountEn loItem = LoadObject(loReader);
                            loItem.SCCode = GetValue<string>(loReader, "SASC_Code");
                            loItem.FTCode = GetValue<string>(loReader, "SAFT_Code");
                            loItem.SCDesc = GetValue<string>(loReader, "SASC_Desc");
                            loItem.GSTAmount = GetValue<double>(loReader, "safa_gstamount");
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

        #region GetList 

        /// <summary>
        /// Method to Get List of FeeStrAmount
        /// </summary>
        /// <param name="argEn">FeeStrAmount Entity is an Input.</param>
        /// <returns>Returns List of FeeStrAmount</returns>
        public List<FeeStrAmountEn> GetList(FeeStrAmountEn argEn)
        {
            List<FeeStrAmountEn> loEnList = new List<FeeStrAmountEn>();
            string sqlCmd = "select * from SAS_FeeStrAmount";
            
            try
            {
                if (!FormHelp.IsBlank(sqlCmd))
                {
                    using (IDataReader loReader = _DatabaseFactory.ExecuteReader(Helper.GetDataBaseType,
                        DataBaseConnectionString, sqlCmd).CreateDataReader())
                    {
                        while (loReader.Read())
                        {
                            FeeStrAmountEn loItem = LoadObject(loReader);
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
        /// Method to Get FeeStrAmount Entity
        /// </summary>
        /// <param name="argEn">FeeStrAmount Entity is an Input</param>
        /// <returns>Returns FeeStrAmount Entity</returns>
        public FeeStrAmountEn GetItem(FeeStrAmountEn argEn)
        {
            FeeStrAmountEn loItem = new FeeStrAmountEn();
            string sqlCmd = "Select * FROM SAS_FeeStrAmount WHERE SAFS_Code = @SAFS_Code";
            
            try
            {
                if (!FormHelp.IsBlank(sqlCmd))
                {
                    DbCommand cmd = _DatabaseFactory.GetDbCommand(Helper.GetDataBaseType, sqlCmd, DataBaseConnectionString);
                    _DatabaseFactory.AddInParameter(ref cmd, "@SAFS_Code", DbType.String, argEn.FSCode);
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
        /// Method to Insert FeeStrAmount 
        /// </summary>
        /// <param name="argEn">FeeStrAmount Entity is an Input.</param>
        /// <returns>Returns Boolean</returns>
        public bool Insert(FeeStrAmountEn argEn)
        {
            bool lbRes = false;
            try
            {
                string sqlCmd = @"INSERT INTO SAS_FeeStrAmount(SAFS_Code,SAFD_Type,SAFT_Code,SASC_Code,SAFA_Amount,safa_gstamount, SAFD_FeeFor, SAFD_Sem) VALUES 
                                (@SAFS_Code,@SAFD_Type,@SAFT_Code,@SASC_Code,@SAFA_Amount,@safa_gstamount, @SAFD_FeeFor, @SAFD_Sem) ";

                if (!FormHelp.IsBlank(sqlCmd))
                {
                    DbCommand cmd = _DatabaseFactory.GetDbCommand(Helper.GetDataBaseType, sqlCmd, DataBaseConnectionString);
                    _DatabaseFactory.AddInParameter(ref cmd, "@SAFS_Code", DbType.String, clsGeneric.NullToString(argEn.FSCode));
                    _DatabaseFactory.AddInParameter(ref cmd, "@SAFT_Code", DbType.String, clsGeneric.NullToString(argEn.FTCode));
                    _DatabaseFactory.AddInParameter(ref cmd, "@SASC_Code", DbType.String, clsGeneric.NullToString(argEn.SCCode));
                    _DatabaseFactory.AddInParameter(ref cmd, "@SAFD_Type", DbType.String, clsGeneric.NullToString(argEn.Type));
                    _DatabaseFactory.AddInParameter(ref cmd, "@SAFA_Amount", DbType.Double, clsGeneric.NullToDecimal(argEn.FeeAmount));
                    _DatabaseFactory.AddInParameter(ref cmd, "@safa_gstamount", DbType.Double, clsGeneric.NullToDecimal(argEn.GSTAmount));
                    _DatabaseFactory.AddInParameter(ref cmd, "@SAFD_FeeFor", DbType.String, clsGeneric.NullToString(argEn.FeeFor));
                    _DatabaseFactory.AddInParameter(ref cmd, "@SAFD_Sem", DbType.Int32, clsGeneric.NullToInteger(argEn.FeeDetailSem));
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
        /// Method to Update FeeStrAmount 
        /// </summary>
        /// <param name="argEn">FeeStrAmount Entity is an Input.</param>
        /// <returns>Returns Boolean</returns>
        public bool Update(FeeStrAmountEn argEn)
        {
            bool lbRes = false;
            int iOut = 0;
            string sqlCmd = "Select count(*) as cnt From SAS_FeeStrAmount WHERE SAFS_Code = @SAFS_Code";
            try
            {
                if (!FormHelp.IsBlank(sqlCmd))
                {
                    DbCommand cmdSel = _DatabaseFactory.GetDbCommand(Helper.GetDataBaseType, sqlCmd, DataBaseConnectionString);
                    _DatabaseFactory.AddInParameter(ref cmdSel, "@SAFS_Code", DbType.String, argEn.FSCode);
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
                        sqlCmd = "UPDATE SAS_FeeStrAmount SET SAFS_Code = @SAFS_Code, SAFT_Code = @SAFT_Code, SASC_Code = @SASC_Code,SAFD_Type = @SAFD_Type, SAFA_Amount = @SAFA_Amount,safa_gstamount=safa_gstamount WHERE SAFS_Code = @SAFS_Code";

                        if (!FormHelp.IsBlank(sqlCmd))
                        {
                            DbCommand cmd = _DatabaseFactory.GetDbCommand(Helper.GetDataBaseType, sqlCmd, DataBaseConnectionString);
                            _DatabaseFactory.AddInParameter(ref cmd, "@SAFS_Code", DbType.String, argEn.FSCode);
                            _DatabaseFactory.AddInParameter(ref cmd, "@SAFT_Code", DbType.String, argEn.FTCode);
                            _DatabaseFactory.AddInParameter(ref cmd, "@SASC_Code", DbType.String, argEn.SCCode);
                            _DatabaseFactory.AddInParameter(ref cmd, "@SAFD_Type", DbType.String, argEn.Type);
                            _DatabaseFactory.AddInParameter(ref cmd, "@SAFA_Amount", DbType.Double, argEn.FeeAmount);
                            _DatabaseFactory.AddInParameter(ref cmd, "@safa_gstamount", DbType.Double, argEn.GSTAmount);
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
        /// Method to Delete FeeStrAmount 
        /// </summary>
        /// <param name="argEn">FeeStrAmount Entity is an Input.</param>
        /// <returns>Returns Boolean</returns>
        public bool Delete(FeeStrAmountEn argEn)
        {
            bool lbRes = false;
            string sqlCmd = "DELETE FROM SAS_FeeStrAmount WHERE SAFS_Code = @SAFS_Code";
            
            try
            {
                if (!FormHelp.IsBlank(sqlCmd))
                {
                    DbCommand cmd = _DatabaseFactory.GetDbCommand(Helper.GetDataBaseType, sqlCmd, DataBaseConnectionString);
                    _DatabaseFactory.AddInParameter(ref cmd, "@SAFS_Code", DbType.String, argEn.FSCode);
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
        /// Method to Load FeeStrAmount Entity 
        /// </summary>
        /// <param name="argReader">IDataReader Object is an Input.</param>
        /// <returns>Returns FeeStrAmount Entity</returns>
        private FeeStrAmountEn LoadObject(IDataReader argReader)
        {
            FeeStrAmountEn loItem = new FeeStrAmountEn();
            loItem.FSCode = GetValue<string>(argReader, "SAFS_Code");
            loItem.FTCode = GetValue<string>(argReader, "SAFT_Code");
            loItem.SCCode = GetValue<string>(argReader, "SASC_Code");
            loItem.Type = GetValue<string>(argReader, "SAFD_Type");
            loItem.FeeAmount = GetValue<double>(argReader, "SAFA_Amount");
            loItem.GSTAmount = GetValue<double>(argReader, "safa_gstamount");

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

