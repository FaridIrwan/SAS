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
    /// Class to handle all the PayMode Methods.
    /// </summary>
    public class PayModeDAL
    {
        #region Global Declarations 

        private DbParameterCollection _DbParameterCollection = null;

        private MaxModule.DatabaseProvider _DatabaseFactory =
            new MaxModule.DatabaseProvider();

        private string DataBaseConnectionString = Helper.
           GetConnectionString();

        #endregion

        public PayModeDAL()
        {
        }

        #region GetPaytype 

        /// <summary>
        /// Method to Get List of PayMode
        /// </summary>
        /// <param name="argEn">PayMode Entity is an Input.SAPM_Code,SAPM_Des and SAPM_Status are Input Properties.</param>
        /// <returns>Returns List of PayMode</returns>
        public List<PayModeEn> GetPaytype(PayModeEn argEn)
        {
            List<PayModeEn> loEnList = new List<PayModeEn>();
            argEn.SAPM_Code = argEn.SAPM_Code.Replace("*", "%");
            argEn.SAPM_Des = argEn.SAPM_Des.Replace("*", "%");
            string sqlCmd = "select * from SAS_PayMode where SAPM_Code <> '0'";
            if (argEn.SAPM_Code.Length != 0) sqlCmd = sqlCmd + " and SAPM_Code like '" + argEn.SAPM_Code + "'";
            if (argEn.SAPM_Des.Length != 0) sqlCmd = sqlCmd + " and SAPM_Des like '" + argEn.SAPM_Des + "'";
            //if (argEn.SAPM_Status == true) sqlCmd = sqlCmd + " and SAPM_Status =1";
            if (argEn.SAPM_Status == true) sqlCmd = sqlCmd + " and SAPM_Status ='true'";
            //if (argEn.SAPM_Status == false) sqlCmd = sqlCmd + " and SAPM_Status =0";
            if (argEn.SAPM_Status == false) sqlCmd = sqlCmd + " and SAPM_Status = 'false'";
            sqlCmd = sqlCmd + " order by SAPM_Code";
            
            try
            {
                if (!FormHelp.IsBlank(sqlCmd))
                {
                    using (IDataReader loReader = _DatabaseFactory.ExecuteReader(Helper.GetDataBaseType,
                        DataBaseConnectionString, sqlCmd).CreateDataReader())
                    {
                        while (loReader.Read())
                        {
                            PayModeEn loItem = LoadObject(loReader);
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

        #region GetPaytypeAll 

        /// <summary>
        /// Method to Get List of PayMode
        /// </summary>
        /// <param name="argEn">PayMode Entity is an Input.SAPM_Code,SAPM_Des and SAPM_Status are Input Properties.</param>
        /// <returns>Returns List of PayMode</returns>
        public List<PayModeEn> GetPaytypeAll(PayModeEn argEn)
        {
            List<PayModeEn> loEnList = new List<PayModeEn>();
            argEn.SAPM_Code = argEn.SAPM_Code.Replace("*", "%");
            argEn.SAPM_Des = argEn.SAPM_Des.Replace("*", "%");
            string sqlCmd = "select * from SAS_PayMode where SAPM_Code <> '0' and sapm_status ='" + argEn.SAPM_Status + "'";
            if (argEn.SAPM_Code.Length != 0) sqlCmd = sqlCmd + " and SAPM_Code like '" + argEn.SAPM_Code + "'";
            if (argEn.SAPM_Des.Length != 0) sqlCmd = sqlCmd + " and SAPM_Des like '" + argEn.SAPM_Des + "'";
            sqlCmd = sqlCmd + " order by SAPM_Code";
            
            try
            {
                if (!FormHelp.IsBlank(sqlCmd))
                {
                    using (IDataReader loReader = _DatabaseFactory.ExecuteReader(Helper.GetDataBaseType,
                        DataBaseConnectionString, sqlCmd).CreateDataReader())
                    {
                        while (loReader.Read())
                        {
                            PayModeEn loItem = LoadObject(loReader);
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
        /// Method to Get PayMode Entity
        /// </summary>
        /// <param name="argEn">PayMode Entity is an Input.SAPM_Code as Input Property.</param>
        /// <returns>Returns PayMode Entity</returns>
        public PayModeEn GetItem(PayModeEn argEn)
        {
            PayModeEn loItem = new PayModeEn();
            string sqlCmd = "Select * FROM SAS_PayMode WHERE SAPM_Code = @SAPM_Code";
            
            try
            {
                if (!FormHelp.IsBlank(sqlCmd))
                {
                    DbCommand cmd = _DatabaseFactory.GetDbCommand(Helper.GetDataBaseType, sqlCmd, DataBaseConnectionString);
                    _DatabaseFactory.AddInParameter(ref cmd, "@SAPM_Code", DbType.String, argEn.SAPM_Code);
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
        /// Method to Insert PayMode 
        /// </summary>
        /// <param name="argEn">PayMode Entity is an Input.</param>
        /// <returns>Returns Boolean</returns>
        public bool Insert(PayModeEn argEn)
        {
            bool lbRes = false;
            int iOut = 0;
            string sqlCmd = "Select count(*) as cnt From SAS_PayMode WHERE SAPM_Code = @SAPM_Code or SAPM_Des = @SAPM_Des";
            try
            {
                if (!FormHelp.IsBlank(sqlCmd))
                {
                    DbCommand cmdSel = _DatabaseFactory.GetDbCommand(Helper.GetDataBaseType, sqlCmd, DataBaseConnectionString);
                    _DatabaseFactory.AddInParameter(ref cmdSel, "@SAPM_Code", DbType.String, argEn.SAPM_Code);
                    _DatabaseFactory.AddInParameter(ref cmdSel, "@SAPM_Des", DbType.String, argEn.SAPM_Des);
                    _DbParameterCollection = cmdSel.Parameters;

                    using (IDataReader dr = _DatabaseFactory.GetIDataReader(Helper.GetDataBaseType, cmdSel,
                        DataBaseConnectionString, sqlCmd, _DbParameterCollection).CreateDataReader())
                    {
                        if (dr.Read())
                          //  iOut = GetValue<int>(dr, "cnt");
                            iOut = clsGeneric.NullToInteger(dr["cnt"]);
                        if (iOut > 0)
                            throw new Exception("Record Already Exist");
                    }
                    if (iOut == 0)
                    {
                        sqlCmd = "INSERT INTO SAS_PayMode(SAPM_Code,SAPM_Des,SAPM_Status) VALUES (@SAPM_Code,@SAPM_Des,@SAPM_Status) ";

                        if (!FormHelp.IsBlank(sqlCmd))
                        {
                            DbCommand cmd = _DatabaseFactory.GetDbCommand(Helper.GetDataBaseType, sqlCmd, DataBaseConnectionString);
                            _DatabaseFactory.AddInParameter(ref cmd, "@SAPM_Code", DbType.String, argEn.SAPM_Code);
                            _DatabaseFactory.AddInParameter(ref cmd, "@SAPM_Des", DbType.String, argEn.SAPM_Des);
                            _DatabaseFactory.AddInParameter(ref cmd, "@SAPM_Status", DbType.Boolean, argEn.SAPM_Status);
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
        /// Method to Update PayMode 
        /// </summary>
        /// <param name="argEn">PayMode Entity is an Input.</param>
        /// <returns>Returns Boolean</returns>
        public bool Update(PayModeEn argEn)
        {
            bool lbRes = false;
            int iOut = 0;
            string sqlCmd = "Select count(*) as cnt From SAS_PayMode WHERE SAPM_Code != @SAPM_Code and  SAPM_Des = @SAPM_Des";
            try
            {
                if (!FormHelp.IsBlank(sqlCmd))
                {
                    DbCommand cmdSel = _DatabaseFactory.GetDbCommand(Helper.GetDataBaseType, sqlCmd, DataBaseConnectionString);
                    _DatabaseFactory.AddInParameter(ref cmdSel, "@SAPM_Code", DbType.String, argEn.SAPM_Code);
                    _DatabaseFactory.AddInParameter(ref cmdSel, "@SAPM_Des", DbType.String, argEn.SAPM_Des);
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
                        sqlCmd = "UPDATE SAS_PayMode SET SAPM_Code = @SAPM_Code, SAPM_Des = @SAPM_Des, SAPM_Status = @SAPM_Status WHERE SAPM_Code = @SAPM_Code";

                        if (!FormHelp.IsBlank(sqlCmd))
                        {
                            DbCommand cmd = _DatabaseFactory.GetDbCommand(Helper.GetDataBaseType, sqlCmd, DataBaseConnectionString);
                            _DatabaseFactory.AddInParameter(ref cmd, "@SAPM_Code", DbType.String, argEn.SAPM_Code);
                            _DatabaseFactory.AddInParameter(ref cmd, "@SAPM_Des", DbType.String, argEn.SAPM_Des);
                            _DatabaseFactory.AddInParameter(ref cmd, "@SAPM_Status", DbType.Boolean, argEn.SAPM_Status);
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
        /// Method to Delete PayMode 
        /// </summary>
        /// <param name="argEn">PayMode Entity is an Input.</param>
        /// <returns>Returns Boolean</returns>
        public bool Delete(PayModeEn argEn)
        {
            bool lbRes = false;

            int total = 0;
            string sqlCmd = "select count(*) as cnt from sas_accounts where paymentmode = @paymode";
            try
            {
                if (!FormHelp.IsBlank(sqlCmd))
                {
                    DbCommand cmdSel = _DatabaseFactory.GetDbCommand(Helper.GetDataBaseType, sqlCmd, DataBaseConnectionString);
                    _DatabaseFactory.AddInParameter(ref cmdSel, "@paymode", DbType.String, argEn.SAPM_Code);
                    _DbParameterCollection = cmdSel.Parameters;

                    using (IDataReader dr = _DatabaseFactory.GetIDataReader(Helper.GetDataBaseType, cmdSel,
                        DataBaseConnectionString, sqlCmd, _DbParameterCollection).CreateDataReader())
                    {
                        if (dr.Read())
                            total = clsGeneric.NullToInteger(dr["cnt"]);
                        if (total > 0)
                            throw new Exception("Record Already in Use");
                    }
                    if (total == 0)
                    {
                        sqlCmd = "DELETE FROM SAS_PayMode WHERE SAPM_Code = @SAPM_Code";

                        if (!FormHelp.IsBlank(sqlCmd))
                        {
                            DbCommand cmd = _DatabaseFactory.GetDbCommand(Helper.GetDataBaseType, sqlCmd, DataBaseConnectionString);
                            _DatabaseFactory.AddInParameter(ref cmd, "@SAPM_Code", DbType.String, argEn.SAPM_Code);
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
        /// Method to Load PayMode Entity 
        /// </summary>
        /// <param name="argReader">IDataReader Object is an Input.</param>
        /// <returns>Returns PayMode Entity</returns>
        private PayModeEn LoadObject(IDataReader argReader)
        {
            PayModeEn loItem = new PayModeEn();
            loItem.SAPM_Code = GetValue<string>(argReader, "SAPM_Code");
            loItem.SAPM_Des = GetValue<string>(argReader, "SAPM_Des");
            loItem.SAPM_Status = GetValue<bool>(argReader, "SAPM_Status");

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
//---------------------------------------------------------------------------------

