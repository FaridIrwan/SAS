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
    /// Class to handle all the BankProfiles Methods.
    /// </summary>
    public class BankProfileDAL
    {
        #region Global Declarations 

        private DbParameterCollection _DbParameterCollection = null;

        private MaxModule.DatabaseProvider _DatabaseFactory =
            new MaxModule.DatabaseProvider();

        private string DataBaseConnectionString = Helper.
           GetConnectionString();

        #endregion

        public BankProfileDAL()
        {
        }

        #region GetList 

        /// <summary>
        /// Method to Get List of BankProfiles
        /// </summary>
        /// <param name="argEn">BankProfile Entity is an Input.</param>
        /// <returns>Returns List of BankProfiles</returns>
        public List<BankProfileEn> GetList(BankProfileEn argEn)
        {
            List<BankProfileEn> loEnList = new List<BankProfileEn>();
            string sqlCmd = "select * from SAS_BankDetails";
            
            try
            {
                if (!FormHelp.IsBlank(sqlCmd))
                {
                    using (IDataReader loReader = _DatabaseFactory.ExecuteReader(Helper.GetDataBaseType,
                        DataBaseConnectionString, sqlCmd).CreateDataReader())
                    {
                        while (loReader.Read())
                        {
                            BankProfileEn loItem = LoadObject(loReader);
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

        #region GetBankProfileList 

        /// <summary>
        /// Method to Get List of Active or Inactive BankProfiles
        /// </summary>
        /// <param name="argEn">BankProfile Entity is an Input.BankDetailsCode,Description,ACCode,GLCode,Status are Input Properties</param>
        /// <returns>Returns List of BankProfiles</returns>
        public List<BankProfileEn> GetBankProfileList(BankProfileEn argEn)
        {
            List<BankProfileEn> loEnList = new List<BankProfileEn>();
            
                argEn.BankDetailsCode = argEn.BankDetailsCode.Replace("*", "%");              
                argEn.Description = argEn.Description.Replace("*", "%");             
                argEn.ACCode = argEn.ACCode.Replace("*", "%");
                argEn.GLCode = argEn.GLCode.Replace("*", "%"); 
            string sqlCmd = "select SABD_Code,SABD_Desc,SABD_ACCode,SABD_GLCode,SABD_Status from SAS_BankDetails where SABD_Code <> '0'";

            if (argEn.BankDetailsCode.Length != 0) sqlCmd = sqlCmd + " and SABD_Code like '" + argEn.BankDetailsCode + "'";
            if (argEn.Description.Length !=0) sqlCmd = sqlCmd + " and SABD_Desc like '" + argEn.Description + "'";
            if (argEn.ACCode.Length != 0) sqlCmd = sqlCmd + " and SABD_ACCode like '" + argEn.ACCode  + "'";
            if (argEn.GLCode.Length != 0) sqlCmd = sqlCmd + " and SABD_GLCode like '" + argEn.GLCode + "'";
            //if ( argEn.Status == true) sqlCmd = sqlCmd + " and SABD_Status =1";
            //if (argEn.Status == false) sqlCmd = sqlCmd + " and SABD_Status =0";
            if (argEn.Status == true) sqlCmd = sqlCmd + " and SABD_Status = 'true'";
            if (argEn.Status == false) sqlCmd = sqlCmd + " and SABD_Status = 'false'";
            sqlCmd = sqlCmd + " order by SABD_Code";

            
            try
            {
                if (!FormHelp.IsBlank(sqlCmd))
                {
                    using (IDataReader loReader = _DatabaseFactory.ExecuteReader(Helper.GetDataBaseType,
                        DataBaseConnectionString, sqlCmd).CreateDataReader())
                    {
                        while (loReader.Read())
                        {
                            BankProfileEn loItem = LoadObject(loReader);
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

        #region GetBankProfileListAll 

        /// <summary>
        /// Method to Get List of All BankProfiles
        /// </summary>
        /// <param name="argEn">BankProfile Entity is an Input.BankDetailsCode,Description,ACCode,GLCode,Status are Input Properties</param>
        /// <returns>Returns List of BankProfiles</returns>
        public List<BankProfileEn> GetBankProfileListAll(BankProfileEn argEn)
        {
            List<BankProfileEn> loEnList = new List<BankProfileEn>();

            argEn.BankDetailsCode = argEn.BankDetailsCode.Replace("*", "%");
            argEn.Description = argEn.Description.Replace("*", "%");
            argEn.ACCode = argEn.ACCode.Replace("*", "%");
            argEn.GLCode = argEn.GLCode.Replace("*", "%");
            string sqlCmd = "select SABD_Code,SABD_Desc,SABD_ACCode,SABD_GLCode,SABD_Status from SAS_BankDetails where SABD_Code <> '0'";

            if (argEn.BankDetailsCode.Length != 0) sqlCmd = sqlCmd + " and SABD_Code like '" + argEn.BankDetailsCode + "'";
            if (argEn.Description.Length != 0) sqlCmd = sqlCmd + " and SABD_Desc like '" + argEn.Description + "'";
            if (argEn.ACCode.Length != 0) sqlCmd = sqlCmd + " and SABD_ACCode like '" + argEn.ACCode + "'";
            if (argEn.GLCode.Length != 0) sqlCmd = sqlCmd + " and SABD_GLCode like '" + argEn.GLCode + "'";
            sqlCmd = sqlCmd + " order by SABD_Code";
                        
            try
            {
                if (!FormHelp.IsBlank(sqlCmd))
                {
                    using (IDataReader loReader = _DatabaseFactory.ExecuteReader(Helper.GetDataBaseType,
                        DataBaseConnectionString, sqlCmd).CreateDataReader())
                    {
                        while (loReader.Read())
                        {
                            BankProfileEn loItem = LoadObject(loReader);
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
        /// Method to Get BankProfile Entity
        /// </summary>
        /// <param name="argEn">BankProfile Entity is an Input</param>
        /// <returns>Returns BankProfile Entity</returns>
        public BankProfileEn GetItem(BankProfileEn argEn)
        {
            BankProfileEn loItem = new BankProfileEn();
            string sqlCmd = "Select * FROM SAS_BankDetails WHERE SABD_Code = @SABD_Code ";
            
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
        /// Method to Insert Bankprofiles
        /// </summary>
        /// <param name="argEn">Bankprofile Entity is an Input.</param>
        /// <returns>Returns Boolean</returns>
        public bool Insert(BankProfileEn argEn)
        {
            bool lbRes = false;
            int iOut = 0;
            string sqlCmd = "Select count(*) as cnt From SAS_BankDetails WHERE SABD_Code = @SABD_Code or SABD_Desc = @SABD_Desc";
            try
            {

                if (!FormHelp.IsBlank(sqlCmd))
                {
                    DbCommand cmdSel = _DatabaseFactory.GetDbCommand(Helper.GetDataBaseType, sqlCmd, DataBaseConnectionString);
                    _DatabaseFactory.AddInParameter(ref cmdSel, "@SABD_Code", DbType.String, argEn.BankDetailsCode);
                    _DatabaseFactory.AddInParameter(ref cmdSel, "@SABD_Desc", DbType.String, argEn.Description);
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
                        sqlCmd = "INSERT INTO SAS_BankDetails(SABD_Code,SABD_Desc,SABD_ACCode,SABD_GLCode,SABD_Status,SABR_Code,SABD_UpdatedBy,SABD_UpdatedDtTm) VALUES (@SABD_Code,@SABD_Desc,@SABD_ACCode,@SABD_GLCode,@SABD_Status,@SABR_Code,@SABD_UpdatedBy,@SABD_UpdatedDtTm) ";

                        if (!FormHelp.IsBlank(sqlCmd))
                        {
                            DbCommand cmd = _DatabaseFactory.GetDbCommand(Helper.GetDataBaseType, sqlCmd, DataBaseConnectionString);
                            _DatabaseFactory.AddInParameter(ref cmd, "@SABD_Code", DbType.String, argEn.BankDetailsCode);
                            _DatabaseFactory.AddInParameter(ref cmd, "@SABD_Desc", DbType.String, argEn.Description);
                            _DatabaseFactory.AddInParameter(ref cmd, "@SABD_ACCode", DbType.String, argEn.ACCode);
                            _DatabaseFactory.AddInParameter(ref cmd, "@SABD_GLCode", DbType.String, argEn.GLCode);
                            _DatabaseFactory.AddInParameter(ref cmd, "@SABD_Status", DbType.Boolean, argEn.Status);
                            _DatabaseFactory.AddInParameter(ref cmd, "@SABR_Code", DbType.Int32, argEn.Code);
                            _DatabaseFactory.AddInParameter(ref cmd, "@SABD_UpdatedBy", DbType.String, argEn.UpdatedBy);
                            _DatabaseFactory.AddInParameter(ref cmd, "@SABD_UpdatedDtTm", DbType.String, argEn.UpdatedDtTm);
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
        /// Method to Update Bankprofiles
        /// </summary>
        /// <param name="argEn">Bankprofile Entity is an Input.</param>
        /// <returns>Returns Boolean</returns>
        public bool Update(BankProfileEn argEn)
        {
            bool lbRes = false;
            int iOut = 0;
            string sqlCmd = "Select count(*) as cnt From SAS_BankDetails WHERE SABD_Code != @SABD_Code and  SABD_Desc = @SABD_Desc";
            try
            {
                if (!FormHelp.IsBlank(sqlCmd))
                {
                    DbCommand cmdSel = _DatabaseFactory.GetDbCommand(Helper.GetDataBaseType, sqlCmd, DataBaseConnectionString);
                    _DatabaseFactory.AddInParameter(ref cmdSel, "@SABD_Code", DbType.String, argEn.BankDetailsCode);
                    _DatabaseFactory.AddInParameter(ref cmdSel, "@SABD_Desc", DbType.String, argEn.Description);
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
                        sqlCmd = "UPDATE SAS_BankDetails SET SABD_Code = @SABD_Code, SABD_Desc = @SABD_Desc, SABD_ACCode = @SABD_ACCode, SABD_GLCode = @SABD_GLCode, SABD_Status = @SABD_Status, SABR_Code = @SABR_Code, SABD_UpdatedBy = @SABD_UpdatedBy, SABD_UpdatedDtTm = @SABD_UpdatedDtTm WHERE SABD_Code = @SABD_Code";

                        if (!FormHelp.IsBlank(sqlCmd))
                        {
                            DbCommand cmd = _DatabaseFactory.GetDbCommand(Helper.GetDataBaseType, sqlCmd, DataBaseConnectionString);
                            _DatabaseFactory.AddInParameter(ref cmd, "@SABD_Code", DbType.String, argEn.BankDetailsCode);
                            _DatabaseFactory.AddInParameter(ref cmd, "@SABD_Desc", DbType.String, argEn.Description);
                            _DatabaseFactory.AddInParameter(ref cmd, "@SABD_ACCode", DbType.String, argEn.ACCode);
                            _DatabaseFactory.AddInParameter(ref cmd, "@SABD_GLCode", DbType.String, argEn.GLCode);
                            _DatabaseFactory.AddInParameter(ref cmd, "@SABD_Status", DbType.Boolean, argEn.Status);
                            _DatabaseFactory.AddInParameter(ref cmd, "@SABR_Code", DbType.Int32, argEn.Code);
                            _DatabaseFactory.AddInParameter(ref cmd, "@SABD_UpdatedBy", DbType.String, argEn.UpdatedBy);
                            _DatabaseFactory.AddInParameter(ref cmd, "@SABD_UpdatedDtTm", DbType.String, argEn.UpdatedDtTm);
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
        /// Method to Delete Bankprofiles
        /// </summary>
        /// <param name="argEn">Bankprofile Entity is an Input.</param>
        /// <returns>Returns Boolean</returns>
        public bool Delete(BankProfileEn argEn)
        {
            bool lbRes = false;

            int total = 0;
            string sqlCmd = "select sum(rows) as total from (select count(*) as rows  from SAS_Accounts WHERE BankCode = @BankCode  union all select count(*) as rows from SAS_Student WHERE SASI_Bank = @BankCode)AS U";
            try
            {
                if (!FormHelp.IsBlank(sqlCmd))
                {
                    DbCommand cmdSel = _DatabaseFactory.GetDbCommand(Helper.GetDataBaseType, sqlCmd, DataBaseConnectionString);
                    _DatabaseFactory.AddInParameter(ref cmdSel, "@BankCode", DbType.String, argEn.BankDetailsCode);
                    _DbParameterCollection = cmdSel.Parameters;

                    using (IDataReader dr = _DatabaseFactory.GetIDataReader(Helper.GetDataBaseType, cmdSel,
                        DataBaseConnectionString, sqlCmd, _DbParameterCollection).CreateDataReader())
                    {
                        if (dr.Read())
                            total = clsGeneric.NullToInteger(dr["total"]);
                        if (total > 0)
                            throw new Exception("Record Already in Use");
                    }
                        if (total == 0)
                        {
                            string sqlCmd1 = "DELETE FROM SAS_BankDetails WHERE SABD_Code = @SABD_Code";

                            if (!FormHelp.IsBlank(sqlCmd1))
                            {
                                DbCommand cmd = _DatabaseFactory.GetDbCommand(Helper.GetDataBaseType, sqlCmd1, DataBaseConnectionString);
                                _DatabaseFactory.AddInParameter(ref cmd, "@SABD_Code", DbType.String, argEn.BankDetailsCode);
                                _DbParameterCollection = cmd.Parameters;

                                int liRowAffected = _DatabaseFactory.ExecuteNonQuery(Helper.GetDataBaseType, cmd,
                                DataBaseConnectionString, sqlCmd1, _DbParameterCollection);

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
        /// Method to Load BankProfile Entity 
        /// </summary>
        /// <param name="argReader">IDataReader Object is an Input.</param>
        /// <returns>Returns BankProfile Entity</returns>
        private BankProfileEn LoadObject(IDataReader argReader)
        {
            BankProfileEn loItem = new BankProfileEn();
            loItem.BankDetailsCode = GetValue<string>(argReader, "SABD_Code");
            loItem.Description = GetValue<string>(argReader, "SABD_Desc");
            loItem.ACCode = GetValue<string>(argReader, "SABD_ACCode");
            loItem.GLCode = GetValue<string>(argReader, "SABD_GLCode");
            loItem.Status = GetValue<bool>(argReader, "SABD_Status");
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
