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
    /// Class to handle all the AutoNumber Methods.
    /// </summary>
    public class AutoNumberDAL
    {
        #region Global Declarations 

        private DbParameterCollection _DbParameterCollection = null;

        private MaxModule.DatabaseProvider _DatabaseFactory =
            new MaxModule.DatabaseProvider();

        private string DataBaseConnectionString = Helper.
           GetConnectionString();

        #endregion

        public AutoNumberDAL()
        {
        }

        #region GetList 

        /// <summary>
        /// Method to Get List of AutoNumber
        /// </summary>
        /// <param name="argEn">AutoNumber Entity is an Input.</param>
        /// <returns>Returns List of AutoNumber Entities</returns>
        public List<AutoNumberEn> GetList(AutoNumberEn argEn)
        {
            List<AutoNumberEn> loEnList = new List<AutoNumberEn>();
            string sqlCmd = "select * from SAS_AutoNumber";
            
            try
            {
                if (!FormHelp.IsBlank(sqlCmd))
                {
                    using (IDataReader loReader = _DatabaseFactory.ExecuteReader(Helper.GetDataBaseType,
                       DataBaseConnectionString, sqlCmd).CreateDataReader())
                    {
                        while (loReader.Read())
                        {
                            AutoNumberEn loItem = LoadObject(loReader);
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

        #region GetAutoNumberList 

        /// <summary>
        /// Method to Get List Of all AutoNumbers
        /// </summary>
        /// <param name="argEn">AutoNumber Entity is an Input.SAAN_Des,SAAN_NoDigit,SAAN_Prefix and SAAN_StartNo are Input Properties.</param>
        /// <returns>Returns List of AutoNumber Entities</returns>
        public List<AutoNumberEn> GetAutoNumberList(AutoNumberEn argEn)
        {
            List<AutoNumberEn> loEnList = new List<AutoNumberEn>();
            argEn.SAAN_Des = argEn.SAAN_Des.Replace("*", "%");
            argEn.SAAN_NoDigit = Convert.ToInt32(Convert.ToString(argEn.SAAN_NoDigit).Replace("*", "%"));
            argEn.SAAN_Prefix = argEn.SAAN_Prefix.Replace("*", "%");
            argEn.SAAN_StartNo =Convert.ToInt32(Convert.ToString(argEn.SAAN_StartNo).Replace("*", "%"));
            string sqlCmd = "select * from SAS_AutoNumber where SAAN_Code <> '0'";
            if (argEn.SAAN_Des.Length != 0) sqlCmd = sqlCmd + " and SAAN_Des like '" + argEn.SAAN_Des + "'";
            if (argEn.SAAN_NoDigit.ToString().Length != 0) sqlCmd = sqlCmd + " and SAAN_NoDigit like '" + argEn.SAAN_NoDigit + "'";
            if (argEn.SAAN_Prefix.Length != 0) sqlCmd = sqlCmd + " and SAAN_Prefix like '" + argEn.SAAN_Prefix + "'";
            if (argEn.SAAN_StartNo.ToString().Length != 0) sqlCmd = sqlCmd + " and SAAN_StartNo like '" + argEn.SAAN_StartNo + "'";
            
            try
            {
                if (!FormHelp.IsBlank(sqlCmd))
                {
                    using (IDataReader loReader = _DatabaseFactory.ExecuteReader(Helper.GetDataBaseType,
                        DataBaseConnectionString, sqlCmd).CreateDataReader())
                    {
                        while (loReader.Read())
                        {
                            AutoNumberEn loItem = LoadObject(loReader);
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
        /// Method to Get an AutoNumber Entity
        /// </summary>
        /// <param name="argEn">AutoNumber Entity is an Input</param>
        /// <returns>Returns AutoNumber Entity</returns>
        public AutoNumberEn GetItem(AutoNumberEn argEn)
        {
            AutoNumberEn loItem = new AutoNumberEn();
            string sqlCmd = "Select * FROM SAS_AutoNumber WHERE SAAN_Code = @SAAN_Code";
            
            try
            {
                if (!FormHelp.IsBlank(sqlCmd))
                {
                    DbCommand cmd = _DatabaseFactory.GetDbCommand(Helper.GetDataBaseType, sqlCmd, DataBaseConnectionString);
                    _DatabaseFactory.AddInParameter(ref cmd, "@SAAN_Code", DbType.Int32, argEn.SAAN_Code);
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
        /// Method to Insert 
        /// </summary>
        /// <param name="argEn">AutoNumber Entity is an Input.</param>
        /// <returns>Returns Boolean</returns>
        public bool Insert(AutoNumberEn argEn)
        {
            bool lbRes = false;
            string sqlCmd = "Select count(*) as cnt From SAS_AutoNumber WHERE SAAN_Code = @SAAN_Code";
            try
            {
                argEn.SAAN_AutoNo = GetAutoNumber(argEn);
                sqlCmd = "INSERT INTO SAS_AutoNumber(SAAN_Des,SAAN_Prefix,SAAN_NoDigit,SAAN_StartNo,SAAN_CurNo,SAAN_AutoNo) VALUES (@SAAN_Des,@SAAN_Prefix,@SAAN_NoDigit,@SAAN_StartNo,@SAAN_CurNo,@SAAN_AutoNo) ";

                if (!FormHelp.IsBlank(sqlCmd))
                {
                    DbCommand cmd = _DatabaseFactory.GetDbCommand(Helper.GetDataBaseType, sqlCmd, DataBaseConnectionString);
                    _DatabaseFactory.AddInParameter(ref cmd, "@SAAN_Des", DbType.String, argEn.SAAN_Des);
                    _DatabaseFactory.AddInParameter(ref cmd, "@SAAN_Prefix", DbType.String, argEn.SAAN_Prefix);
                    _DatabaseFactory.AddInParameter(ref cmd, "@SAAN_NoDigit", DbType.Int32, argEn.SAAN_NoDigit);
                    _DatabaseFactory.AddInParameter(ref cmd, "@SAAN_StartNo", DbType.Int32, argEn.SAAN_StartNo);
                    _DatabaseFactory.AddInParameter(ref cmd, "@SAAN_CurNo", DbType.Int32, argEn.SAAN_CurNo);
                    _DatabaseFactory.AddInParameter(ref cmd, "@SAAN_AutoNo", DbType.String, argEn.SAAN_AutoNo);
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
        /// Method to Update 
        /// </summary>
        /// <param name="argEn">AutoNumber Entity is an Input.</param>
        /// <returns>Returns Boolean</returns>
        public bool Update(AutoNumberEn argEn)
        {
            bool lbRes = false;
            string sqlCmd = "Select count(*) as cnt From SAS_AutoNumber WHERE SAAN_Code = @SAAN_Code";
            try
            {

                sqlCmd = "UPDATE SAS_AutoNumber SET SAAN_CurNo = @SAAN_CurNo, SAAN_AutoNo = @SAAN_AutoNo WHERE SAAN_Code = @SAAN_Code";

                if (!FormHelp.IsBlank(sqlCmd))
                {
                    DbCommand cmd = _DatabaseFactory.GetDbCommand(Helper.GetDataBaseType, sqlCmd, DataBaseConnectionString);
                    _DatabaseFactory.AddInParameter(ref cmd, "@SAAN_Code", DbType.Int32, argEn.SAAN_Code);
                    _DatabaseFactory.AddInParameter(ref cmd, "@SAAN_CurNo", DbType.Int32, argEn.SAAN_CurNo);
                    _DatabaseFactory.AddInParameter(ref cmd, "@SAAN_AutoNo", DbType.String, argEn.SAAN_AutoNo);
                    _DbParameterCollection = cmd.Parameters;

                    int liRowAffected = _DatabaseFactory.ExecuteNonQuery(Helper.GetDataBaseType, cmd,
                                DataBaseConnectionString, sqlCmd, _DbParameterCollection);
                    
                    if (liRowAffected > -1)
                        lbRes = true;
                    else
                        throw new Exception("Update Failed! No Row has been updated...");
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return lbRes;
        }

        #endregion

        #region UpdateAutoNumber 

        /// <summary>
        /// Method to Update AutoNumber 
        /// </summary>
        /// <param name="argEn">AutoNumber Entity is an Input.</param>
        /// <returns>Returns Boolean</returns>
        public bool UpdateAutoNumber(AutoNumberEn argEn)
        {
            bool lbRes = false;
            string sqlCmd = "Select count(*) as cnt From SAS_AutoNumber WHERE SAAN_Code = @SAAN_Code";
            try
            {
                argEn.SAAN_AutoNo = GetAutoNumber(argEn);
                sqlCmd = "UPDATE SAS_AutoNumber SET SAAN_Prefix=@SAAN_Prefix, SAAN_NoDigit=@SAAN_NoDigit, SAAN_StartNo=@SAAN_StartNo, SAAN_CurNo = @SAAN_CurNo, SAAN_AutoNo = @SAAN_AutoNo WHERE SAAN_Code = @SAAN_Code";

                if (!FormHelp.IsBlank(sqlCmd))
                {
                    DbCommand cmd = _DatabaseFactory.GetDbCommand(Helper.GetDataBaseType, sqlCmd, DataBaseConnectionString);
                    _DatabaseFactory.AddInParameter(ref cmd, "@SAAN_Code", DbType.Int32, argEn.SAAN_Code);
                    _DatabaseFactory.AddInParameter(ref cmd, "@SAAN_Des", DbType.String, argEn.SAAN_Des);
                    _DatabaseFactory.AddInParameter(ref cmd, "@SAAN_Prefix", DbType.String, argEn.SAAN_Prefix);
                    _DatabaseFactory.AddInParameter(ref cmd, "@SAAN_NoDigit", DbType.Int32, argEn.SAAN_NoDigit);
                    _DatabaseFactory.AddInParameter(ref cmd, "@SAAN_StartNo", DbType.Int32, argEn.SAAN_StartNo);
                    _DatabaseFactory.AddInParameter(ref cmd, "@SAAN_CurNo", DbType.Int32, argEn.SAAN_CurNo);
                    _DatabaseFactory.AddInParameter(ref cmd, "@SAAN_AutoNo", DbType.String, argEn.SAAN_AutoNo);
                    _DbParameterCollection = cmd.Parameters;

                    int liRowAffected = _DatabaseFactory.ExecuteNonQuery(Helper.GetDataBaseType, cmd,
                                DataBaseConnectionString, sqlCmd, _DbParameterCollection);
                    
                    if (liRowAffected > -1)
                        lbRes = true;
                    else
                        throw new Exception("Update Failed! No Row has been updated...");
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
        /// Method to Delete 
        /// </summary>
        /// <param name="argEn">AutoNumber Entity is an Input.</param>
        /// <returns>Returns Boolean</returns>
        public bool Delete(AutoNumberEn argEn)
        {
            bool lbRes = false;
            string sqlCmd = "DELETE FROM SAS_AutoNumber WHERE SAAN_Des = @SAAN_Des";
            
            try
            {
                if (!FormHelp.IsBlank(sqlCmd))
                {
                    DbCommand cmd = _DatabaseFactory.GetDbCommand(Helper.GetDataBaseType, sqlCmd, DataBaseConnectionString);
                    _DatabaseFactory.AddInParameter(ref cmd, "@SAAN_Des", DbType.String, argEn.SAAN_Des);
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
        /// Method to Load AutoNumber Entity 
        /// </summary>
        /// <param name="argReader">IDataReader Object is an Input.</param>
        /// <returns>Returns AutoNumber Entity</returns>
        private AutoNumberEn LoadObject(IDataReader argReader)
        {
            AutoNumberEn loItem = new AutoNumberEn();
            loItem.SAAN_Code = GetValue<int>(argReader, "SAAN_Code");
            loItem.SAAN_Des = GetValue<string>(argReader, "SAAN_Des");
            loItem.SAAN_Prefix = GetValue<string>(argReader, "SAAN_Prefix");
            loItem.SAAN_NoDigit = GetValue<int>(argReader, "SAAN_NoDigit");
            loItem.SAAN_StartNo = GetValue<int>(argReader, "SAAN_StartNo");
            loItem.SAAN_CurNo = GetValue<int>(argReader, "SAAN_CurNo");
            loItem.SAAN_AutoNo = GetValue<string>(argReader, "SAAN_AutoNo");

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

        #region GetAutoNumber 

        /// <summary>
        /// Method to Get AutoNumber
        /// </summary>
        /// <param name="argEn">Accounts Entity as Input</param>
        /// <returns>Returns AutoNumber</returns>
        public string GetAutoNumber(AutoNumberEn argEn)
        {
            string AutoNo = "";
            int CurNo = 0;
            int NoDigit = 0;
            int AutoCode = 0;
            int i = 0;

            AutoNo = argEn.SAAN_Prefix;
            NoDigit = argEn.SAAN_NoDigit;
            CurNo = argEn.SAAN_StartNo;

            
            try
            {
                if (CurNo.ToString().Length < NoDigit)
                {
                    while (i < NoDigit - CurNo.ToString().Length)
                    {
                        AutoNo = AutoNo + "0";
                        i = i + 1;
                    }
                    AutoNo = AutoNo + CurNo;
                }
                return AutoNo;
            }

            catch (Exception ex)
            {
                Console.Write("Error in connection : " + ex.Message);
                return ex.ToString();
            }

        }

        #endregion
    }
}


