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
    /// Class to handle all the FeeCharges Methods.
    /// </summary>
    public class FeeChargesDAL
    {
        #region Global Declarations 

        private DbParameterCollection _DbParameterCollection = null;

        private MaxModule.DatabaseProvider _DatabaseFactory =
            new MaxModule.DatabaseProvider();

        private string DataBaseConnectionString = Helper.
           GetConnectionString();

        #endregion

        public FeeChargesDAL()
        {
        }

        #region GetFeeCharges 

        /// <summary>
        /// Method to Get List of FeeCharges
        /// </summary>
        /// <param name="argEn">FeeCharges Entity is an Input.FTCode is an Input Property</param> 
        /// <returns>Returns List of FeeCharges</returns>
        public List<FeeChargesEn> GetFeeCharges(FeeChargesEn argEn)
        {
            List<FeeChargesEn> loEnList = new List<FeeChargesEn>();
            string sqlCmd = "SELECT SAS_FeeCharges.SAFT_Code, SAS_FeeCharges.SASC_Code, SAS_FeeCharges.SAFS_Amount,SAS_FeeCharges.safs_gstamout, SAS_StudentCategory.SASC_Desc " +
                             " FROM SAS_StudentCategory INNER JOIN SAS_FeeCharges ON SAS_StudentCategory.SASC_Code = SAS_FeeCharges.SASC_Code " +
                             " WHERE SAS_FeeCharges.SAFT_Code =@SAFT_Code";
            
            
            try
            {
                if (!FormHelp.IsBlank(sqlCmd))
                {
                    DbCommand cmd = _DatabaseFactory.GetDbCommand(Helper.GetDataBaseType, sqlCmd, DataBaseConnectionString);
                    _DatabaseFactory.AddInParameter(ref cmd, "@SAFT_Code", DbType.String, argEn.FTCode);
                    _DbParameterCollection = cmd.Parameters;

                    using (IDataReader loReader = _DatabaseFactory.GetIDataReader(Helper.GetDataBaseType, cmd,
                        DataBaseConnectionString, sqlCmd, _DbParameterCollection).CreateDataReader()) 
                    {
                        while (loReader.Read())
                        {
                            FeeChargesEn loItem = LoadObject(loReader);
                            loItem.FTCode = GetValue<string>(loReader, "SAFT_Code");
                            loItem.SCDesc = GetValue<string>(loReader, "SASC_Desc");
                            loItem.GSTAmount = GetValue<double>(loReader, "safs_gstamout");
                            loItem.FSAmount = GetValue<double>(loReader, "SAFS_Amount");
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

        #region GetKokoCharges 

        /// <summary>
        /// Method to Get List of KokoCharges
        /// </summary>
        /// <param name="argEn">KokoCharges Entity is an Input.FTCode is an Input Property</param>
        /// <returns>Returns List of KokoCharges</returns>
        public List<FeeChargesEn> GetKokoCharges(FeeChargesEn argEn)
        {
            List<FeeChargesEn> loEnList = new List<FeeChargesEn>();
            string sqlCmd = "SELECT SAKOD_CategoryCode, SAKOD_FeeAmount, SAKO_Code,SAKOD_CategoryName FROM  SAS_KokorikulumDetails WHERE " +
                "SAKO_Code =@SAKO_Code";
                        
            try
            {
                if (!FormHelp.IsBlank(sqlCmd))
                {
                    DbCommand cmd = _DatabaseFactory.GetDbCommand(Helper.GetDataBaseType, sqlCmd, DataBaseConnectionString);
                    _DatabaseFactory.AddInParameter(ref cmd, "@SAKO_Code", DbType.String, argEn.FTCode);
                    _DbParameterCollection = cmd.Parameters;

                    using (IDataReader loReader = _DatabaseFactory.GetIDataReader(Helper.GetDataBaseType, cmd,
                       DataBaseConnectionString, sqlCmd, _DbParameterCollection).CreateDataReader()) 
                    {
                        while (loReader.Read())
                        {
                            FeeChargesEn loItem = LoadObjectKoko(loReader);
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
        /// Method to Get FeeCharges Entity
        /// </summary>
        /// <param name="argEn">FeeCharges Entity is an Input</param>
        /// <returns>Returns FeeCharges Entity</returns>
        public FeeChargesEn GetItem(FeeChargesEn argEn)
        {
            FeeChargesEn loItem = new FeeChargesEn();
            string sqlCmd = "Select * FROM SAS_FeeCharges WHERE SAFT_Code = @SAFT_Code";
            
            try
            {
                if (!FormHelp.IsBlank(sqlCmd))
                {
                    DbCommand cmd = _DatabaseFactory.GetDbCommand(Helper.GetDataBaseType, sqlCmd, DataBaseConnectionString);
                    _DatabaseFactory.AddInParameter(ref cmd, "@SAFT_Code", DbType.String, argEn.FTCode);

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
        /// Method to Insert FeeCharges 
        /// </summary>
        /// <param name="argEn">FeeCharges Entity is an Input.</param>
        /// <returns>Returns Boolean</returns>
        public bool Insert(FeeChargesEn argEn)
        {
            bool lbRes = false;
            int iOut = 0;
            string sqlCmd ;
            try
            {

                if (iOut == 0)
                {
                    sqlCmd = "INSERT INTO SAS_FeeCharges(SAFT_Code,SASC_Code,SAFS_Amount,safs_gstamout) VALUES (@SAFT_Code,@SASC_Code,@SAFS_Amount,@SAFS_GstAmount) ";

                    if (!FormHelp.IsBlank(sqlCmd))
                    {
                        DbCommand cmd = _DatabaseFactory.GetDbCommand(Helper.GetDataBaseType, sqlCmd, DataBaseConnectionString);
                        _DatabaseFactory.AddInParameter(ref cmd, "@SAFT_Code", DbType.String, argEn.FTCode);
                        _DatabaseFactory.AddInParameter(ref cmd, "@SASC_Code", DbType.String, argEn.SCCode);
                        _DatabaseFactory.AddInParameter(ref cmd, "@SAFS_Amount", DbType.Double, argEn.FSAmount);
                        _DatabaseFactory.AddInParameter(ref cmd, "@SAFS_GstAmount", DbType.Double, argEn.GSTAmount);
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

            catch (Exception ex)
            {
                throw ex;
            }
            return lbRes;
        }

        #endregion

        #region InsertKokoCharges -Old

        ///// <summary>
        ///// Method to Insert KokoCharges
        ///// </summary>
        ///// <param name="argEn">KokoCharges Entity is an Input.</param>
        ///// <returns>Returns Boolean</returns>
        public bool InsertKokoCharges(FeeChargesEn argEn)
        {
            bool lbRes = false;
            int iOut = 0;
            string sqlCmd;
            try
            {

                if (iOut == 0)
                {
                    sqlCmd = "INSERT INTO SAS_KokorikulumDetails(SAKO_Code,SAKOD_CategoryCode,SAKOD_FeeAmount,SAKOD_CategoryName) VALUES (@SAKO_Code,@SAKOD_CategoryCode,@SAKOD_FeeAmount,@SAKOD_CategoryName) ";

                    if (!FormHelp.IsBlank(sqlCmd))
                    {
                        DbCommand cmd = _DatabaseFactory.GetDbCommand(Helper.GetDataBaseType, sqlCmd, DataBaseConnectionString);
                        _DatabaseFactory.AddInParameter(ref cmd, "@SAKO_Code", DbType.String, argEn.FTCode);
                        _DatabaseFactory.AddInParameter(ref cmd, "@SAKOD_CategoryCode", DbType.String, argEn.SCCode);
                        _DatabaseFactory.AddInParameter(ref cmd, "@SAKOD_FeeAmount", DbType.Double, argEn.FSAmount);
                        _DatabaseFactory.AddInParameter(ref cmd, "@SAKOD_CategoryName", DbType.String, argEn.SCDesc);
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

            catch (Exception ex)
            {
                throw ex;
            }
            return lbRes;
        }

        #endregion

        #region Update 

        /// <summary>
        /// Method to Update FeeCharges 
        /// </summary>
        /// <param name="argEn">FeeCharges Entity is an Input.</param>
        /// <returns>Returns Boolean</returns>
        public bool Update(FeeChargesEn argEn)
        {
            bool lbRes = false;
            int iOut = 0;
            string sqlCmd;
            try
            {
                    if (iOut == 0)
                    {
                        sqlCmd = "UPDATE SAS_FeeCharges SET SAFS_Amount = @SAFS_Amount,SAFS_gstamout=@SAFS_gstamout WHERE SAFT_Code = @SAFT_Code and SASC_Code = @SASC_Code";

                        if (!FormHelp.IsBlank(sqlCmd))
                        {
                            DbCommand cmd = _DatabaseFactory.GetDbCommand(Helper.GetDataBaseType, sqlCmd, DataBaseConnectionString);
                            _DatabaseFactory.AddInParameter(ref cmd, "@SAFT_Code", DbType.String, argEn.FTCode);
                            _DatabaseFactory.AddInParameter(ref cmd, "@SASC_Code", DbType.String, argEn.SCCode);
                            _DatabaseFactory.AddInParameter(ref cmd, "@SAFS_Amount", DbType.Double, argEn.FSAmount);
                            _DatabaseFactory.AddInParameter(ref cmd, "@SAFS_gstamout", DbType.Double, argEn.GSTAmount);
                            _DbParameterCollection = cmd.Parameters;

                            int liRowAffected = _DatabaseFactory.ExecuteNonQuery(Helper.GetDataBaseType, cmd,
                                DataBaseConnectionString, sqlCmd, _DbParameterCollection);
                            
                            if (liRowAffected > -1)
                                lbRes = true;
                            else
                                throw new Exception("Update Failed! No Row has been updated...");
                        }
                //    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return lbRes;
        }

        #endregion

        #region UpdateKokoDetails

        /// <summary>
        /// Method to Update FeeCharges 
        /// </summary>
        /// <param name="argEn">FeeCharges Entity is an Input.</param>
        /// <returns>Returns Boolean</returns>
        public bool UpdateKokoDetails(KokoEn argEn)
        {
            bool lbRes = false;
            int iOut = 0;
            string sqlCmd;
            string sqlCmd1;
            //DeleteKoko(argEn);
            sqlCmd = "Select count(*) as cnt From SAS_KokorikulumDetails WHERE SAKO_Code = @SAKO_Code and saft_code = @saft_code";
            //sqlCmd1 = "delete From SAS_KokorikulumDetails WHERE SAKO_Code = @SAKO_Code";
            try
            {
                if (!FormHelp.IsBlank(sqlCmd))
                {
                    DbCommand cmdSel = _DatabaseFactory.GetDbCommand(Helper.GetDataBaseType, sqlCmd, DataBaseConnectionString);
                    _DatabaseFactory.AddInParameter(ref cmdSel, "@SAKO_Code", DbType.String, argEn.Code);
                    //_DatabaseFactory.AddInParameter(ref cmdSel, "@SAKOD_CategoryCode", DbType.String, argEn.Category);
                    _DatabaseFactory.AddInParameter(ref cmdSel, "@Saft_code", DbType.String, argEn.Saftcode);
                    _DbParameterCollection = cmdSel.Parameters;

                    using (IDataReader dr = _DatabaseFactory.GetIDataReader(Helper.GetDataBaseType, cmdSel,
                        DataBaseConnectionString, sqlCmd, _DbParameterCollection).CreateDataReader())
                    {
                        if (dr.Read())
                            iOut = clsGeneric.NullToInteger(dr["cnt"]);
                        ///If that category code not exist then insert else update
                        if (iOut == 0)
                        {
                            lbRes = InsertKokoCharges(argEn);
                        }
                        else
                        {
                            //                            sqlCmd = @"UPDATE SAS_KokorikulumDetails 
                            //                                SET SAKO_Code = @SAKO_Code, SAKOD_CategoryCode = @SAKOD_CategoryCode, SAKOD_FeeAmount = @SAKOD_FeeAmount, Saft_code = @Saft_code, sakod_feeamountout = @sakod_feeamountout, sakod_categoryname = @sakod_categoryname, sakod_gstin = @sakod_gstin, sakod_gstout = @sakod_gstout, safd_type = @safd_type
                            //                                WHERE SAKO_Code = @SAKO_Code and sakod_categorycode = @SAKOD_CategoryCode and Saft_code = @Saft_code";
                            sqlCmd = "INSERT INTO SAS_KokorikulumDetails(SAKO_Code,SAKOD_CategoryCode,saft_code,SAKOD_CategoryName,sakod_feeamount,sakod_feeamountout ,sakod_gstin ,sakod_gstout ,safd_type) " +
                    " VALUES (@SAKO_Code,@SAKOD_CategoryCode,@saft_code,@SAKOD_CategoryName,@sakod_feeamountlocalin,@sakod_feeamountlocalout,@sakod_gstlocalin,@sakod_gstlocalout,@safd_type) ";

                            if (!FormHelp.IsBlank(sqlCmd))
                            {
                                DbCommand cmd = _DatabaseFactory.GetDbCommand(Helper.GetDataBaseType, sqlCmd, DataBaseConnectionString);
                                _DatabaseFactory.AddInParameter(ref cmd, "@SAKO_Code", DbType.String, argEn.Code);
                                _DatabaseFactory.AddInParameter(ref cmd, "@SAKOD_CategoryCode", DbType.String, argEn.Category);
                                _DatabaseFactory.AddInParameter(ref cmd, "@sakod_categoryname", DbType.String, argEn.categoryname);
                                _DatabaseFactory.AddInParameter(ref cmd, "@Saft_code", DbType.String, argEn.Saftcode);
                                _DatabaseFactory.AddInParameter(ref cmd, "@SAKOD_FeeAmount", DbType.Double, argEn.FSAmount);
                                _DatabaseFactory.AddInParameter(ref cmd, "@sakod_feeamountout", DbType.Double, argEn.sakodfeeamountlocalout);
                                _DatabaseFactory.AddInParameter(ref cmd, "@sakod_gstin", DbType.Double, argEn.sakodgstamountlocalin);
                                _DatabaseFactory.AddInParameter(ref cmd, "@sakod_gstout", DbType.Double, argEn.sakodfeegstamountlocalout);
                                _DatabaseFactory.AddInParameter(ref cmd, "@safd_type", DbType.String, argEn.FTCode);
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
        /// Method to Delete FeeCharges 
        /// </summary>
        /// <param name="argEn">FeeCharges Entity is an Input.</param>
        /// <returns>Returns Boolean</returns>
        public bool Delete(FeeChargesEn argEn)
        {
            bool lbRes = false;
            string sqlCmd = "DELETE FROM SAS_FeeCharges WHERE SAFT_Code = @SAFT_Code";
            
            try
            {
                if (!FormHelp.IsBlank(sqlCmd))
                {
                    DbCommand cmd = _DatabaseFactory.GetDbCommand(Helper.GetDataBaseType, sqlCmd, DataBaseConnectionString);
                    _DatabaseFactory.AddInParameter(ref cmd, "@SAFT_Code", DbType.String, argEn.FTCode);
                    _DbParameterCollection = cmd.Parameters;

                    int liRowAffected = _DatabaseFactory.ExecuteNonQuery(Helper.GetDataBaseType, cmd,
                                DataBaseConnectionString, sqlCmd, _DbParameterCollection);
                    if (liRowAffected > -1)
                        lbRes = true;
                    else
                        throw new Exception("Delete Failed! No Row has been updated...");
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
        /// Method to Load FeeCharges Entity 
        /// </summary>
        /// <param name="argReader">IDataReader Object is an Input.</param>
        /// <returns>Returns FeeCharges Entity</returns>
        private FeeChargesEn LoadObject(IDataReader argReader)
        {
            FeeChargesEn loItem = new FeeChargesEn();
            loItem.FTCode = GetValue<string>(argReader, "SAFT_Code");
            loItem.SCCode = GetValue<string>(argReader, "SASC_Code");
            loItem.FSAmount = GetValue<double>(argReader, "SAFS_Amount");

            return loItem;
        }
        /// <summary>
        /// Method to Load FeeCharges Entity 
        /// </summary>
        /// <param name="argReader">IDataReader Object is an Input.</param>
        /// <returns>Returns FeeCharges Entity</returns>
        private FeeChargesEn LoadObjectKoko(IDataReader argReader)
        {
            FeeChargesEn loItem = new FeeChargesEn();
            loItem.SCCode = GetValue<string>(argReader, "SAKOD_CategoryCode");
            loItem.FSAmount = GetValue<double>(argReader, "SAKOD_FeeAmount");
            loItem.SCDesc = GetValue<string>(argReader, "SAKOD_CategoryName");

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

        #region Getkokobaru

        /// <summary>
        /// Method to Get List of KokoCharges
        /// </summary>
        /// <param name="argEn">KokoCharges Entity is an Input.FTCode is an Input Property</param>
        /// <returns>Returns List of KokoCharges</returns>
        public List<KokoEn> Getkokobaru(FeeChargesEn argEn)
        {
            List<KokoEn> loEnList = new List<KokoEn>();
            string sqlCmd = "select distinct kk.sako_code , kk.sako_description, kk.sako_status," +
            " local.sakod_feeamount as sakodfeeamountlocalin,local.sakod_gstin as sakodgstamountlocalin, local.sakod_feeamountout as sakodfeeamountlocalout," +
            " local.sakod_gstout as sakodfeegstamountlocalout, nonlocal.sakod_feeamount as sakodfeeamountinterin, nonlocal.sakod_gstin as sakodgstamountinterin, " +
            " nonlocal.sakod_feeamountout as sakodfeeamountinterout,nonlocal.sakod_gstout as sakodgstamountinterout, local.sakod_categorycode as LocalCategory," +
            " nonlocal.sakod_categorycode as NonLocalCategory,local.saft_code as saft_code,local.saft_taxmode as saft_taxmode from  sas_kokorikulum kk inner join sas_feetypes ft on ft.saft_code = saft_code left join (select saft_code,sako_code,sakod_feeamount,  sakod_feeamountout, sakod_gstin, sakod_gstout, " +
            " safd_type,sakod_categorycode,saft_taxmode from sas_kokorikulumdetails where sas_kokorikulumdetails.sakod_categorycode in ('W')  )as local on local.sako_code = kk.sako_code and local.saft_code = ft.saft_code" +
            " left join (select saft_code,sako_code,sakod_feeamount,  sakod_feeamountout, sakod_gstin, sakod_gstout,safd_type, sakod_categorycode,saft_taxmode from sas_kokorikulumdetails " +
            " where sas_kokorikulumdetails.sakod_categorycode in ('BW')  )as nonlocal on nonlocal.sako_code = kk.sako_code and nonlocal.saft_code = ft.saft_code WHERE kk.sako_code = @SAKO_Code";

            try
            {
                if (!FormHelp.IsBlank(sqlCmd))
                {
                    DbCommand cmd = _DatabaseFactory.GetDbCommand(Helper.GetDataBaseType, sqlCmd, DataBaseConnectionString);
                    _DatabaseFactory.AddInParameter(ref cmd, "@SAKO_Code", DbType.String, argEn.FTCode);
                    _DbParameterCollection = cmd.Parameters;

                    using (IDataReader loReader = _DatabaseFactory.GetIDataReader(Helper.GetDataBaseType, cmd,
                       DataBaseConnectionString, sqlCmd, _DbParameterCollection).CreateDataReader())
                    {
                        while (loReader.Read())
                        {
                            KokoEn loItem = new KokoEn();
                            FeeTypesDAL loDs = new FeeTypesDAL();
                            FeeTypesEn loEn = new FeeTypesEn();
                            loItem.sakod_idkoko = GetValue<int>(loReader, "saft_taxmode");
                            loItem.Code = GetValue<string>(loReader, "sako_code");
                            loItem.Saftcode = GetValue<string>(loReader, "saft_code");
                            loItem.LocalCategory = GetValue<string>(loReader, "LocalCategory");
                            loItem.NonLocalCategory = GetValue<string>(loReader, "NonLocalCategory");
                            int Tax = loItem.sakod_idkoko;
                            loItem.taxmode = (GetGstMode(Tax));


                            if (loItem.taxmode == 2)
                            {
                                loItem.sakodfeeamountlocalin = GetValue<double>(loReader, "sakodfeeamountlocalin");
                                loItem.sakodfeeamountlocalout = GetValue<double>(loReader, "sakodfeeamountlocalout");
                                loItem.sakodgstamountlocalin = GetValue<double>(loReader, "sakodgstamountlocalin");
                                loItem.sakodfeegstamountlocalout = GetValue<double>(loReader, "sakodfeegstamountlocalout");
                                loItem.sakodfeeamountinterin = GetValue<double>(loReader, "sakodfeeamountinterin");
                                loItem.sakodfeeamountinterout = GetValue<double>(loReader, "sakodfeeamountinterout");
                                loItem.sakodgstamountinterin = GetValue<double>(loReader, "sakodgstamountinterin");
                                loItem.sakodgstamountinterout = GetValue<double>(loReader, "sakodgstamountinterout");
                                loItem.Local_TempAmount = loItem.sakodfeeamountlocalin - loItem.sakodgstamountlocalin;
                                loItem.LocalTempAmount = loItem.sakodfeeamountlocalout - loItem.sakodfeegstamountlocalout;
                                loItem.Inter_TempAmount = loItem.sakodfeeamountinterin - loItem.sakodgstamountinterin;
                                loItem.NonLocalTempAmount = loItem.sakodfeeamountinterout - loItem.sakodgstamountinterout;
                                loItem.totalfeelocalin = loItem.sakodfeeamountlocalin;
                                loItem.totalfeelocalout = loItem.sakodfeeamountlocalout;
                                loItem.totalfeeinterin = loItem.sakodfeeamountinterin;
                                loItem.totalfeeinterout = loItem.sakodfeeamountinterout;
                            }
                            else if (loItem.taxmode == 1)
                            {


                                loItem.sakodgstamountlocalin = GetValue<double>(loReader, "sakodgstamountlocalin");
                                loItem.sakodfeegstamountlocalout = GetValue<double>(loReader, "sakodfeegstamountlocalout");
                                loItem.sakodgstamountinterin = GetValue<double>(loReader, "sakodgstamountinterin");
                                loItem.sakodgstamountinterout = GetValue<double>(loReader, "sakodgstamountinterout");
                                loItem.totalfeelocalin = GetValue<double>(loReader, "sakodfeeamountlocalin");
                                loItem.totalfeelocalout = GetValue<double>(loReader, "sakodfeeamountlocalout");
                                loItem.totalfeeinterin = GetValue<double>(loReader, "sakodfeeamountinterin");
                                loItem.totalfeeinterout = GetValue<double>(loReader, "sakodfeeamountinterout");
                                loItem.sakodfeeamountlocalin = loItem.totalfeelocalin - loItem.sakodgstamountlocalin;
                                loItem.sakodfeeamountlocalout = loItem.totalfeelocalout - loItem.sakodfeegstamountlocalout;
                                loItem.sakodfeeamountinterin = loItem.totalfeeinterin - loItem.sakodgstamountinterin;
                                loItem.sakodfeeamountinterout = loItem.totalfeeinterout - loItem.sakodgstamountinterout;
                                loItem.Local_TempAmount = loItem.totalfeelocalin - loItem.sakodgstamountlocalin;
                                loItem.LocalTempAmount = loItem.totalfeelocalout - loItem.sakodfeegstamountlocalout;
                                loItem.Inter_TempAmount = loItem.totalfeeinterin - loItem.sakodgstamountinterin;
                                loItem.NonLocalTempAmount = loItem.totalfeeinterout - loItem.sakodgstamountinterout;
                            }



                            if (loItem.Saftcode != null)
                            {
                                loEnList.Add(loItem);
                            }
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

        #region UpdateKo

        public bool UpdateKo(KokoEn argEn)
        {
            bool lbRes = false;

            //if (lbRes)
            InsertKokoCharges(argEn, false);
            return lbRes;

        }

        #endregion

        #region InsertKokoCharges

        /// <summary>
        /// Method to Insert KokoCharges
        /// </summary>
        /// <param name="argEn">KokoCharges Entity is an Input.</param>
        /// <returns>Returns Boolean</returns>
        public bool InsertKokoCharges(KokoEn argEn, bool GenerateAutoNumber = true)
        {
            bool lbRes = false;
            //int iOut = 0;
            string sqlCmd;
            //string sqlCmd = "Select count(*) as cnt From SAS_KokorikulumDetails WHERE SAKO_Code = @SAKO_Code and SAKOD_CategoryCode= @SAKOD_CategoryCode and @saft";
            try
            {                
                sqlCmd = "INSERT INTO SAS_KokorikulumDetails(SAKO_Code,SAKOD_CategoryCode,saft_code,SAKOD_CategoryName,sakod_feeamount,sakod_feeamountout ,sakod_gstin ,sakod_gstout ,safd_type, saft_taxmode) " +
                    " VALUES (@SAKO_Code,@SAKOD_CategoryCode,@saft_code,@SAKOD_CategoryName,@sakod_feeamountlocalin,@sakod_feeamountlocalout,@sakod_gstlocalin,@sakod_gstlocalout,@safd_type, @saft_taxmode) ";

                if (!FormHelp.IsBlank(sqlCmd))
                {
                    DbCommand cmd = _DatabaseFactory.GetDbCommand(Helper.GetDataBaseType, sqlCmd, DataBaseConnectionString);
                    _DatabaseFactory.AddInParameter(ref cmd, "@SAKO_Code", DbType.String, argEn.Code);
                    _DatabaseFactory.AddInParameter(ref cmd, "@SAKOD_CategoryCode", DbType.String, argEn.Category);
                    _DatabaseFactory.AddInParameter(ref cmd, "@saft_code", DbType.String, argEn.Saftcode);
                    _DatabaseFactory.AddInParameter(ref cmd, "@SAKOD_CategoryName", DbType.String, argEn.categoryname);
                    _DatabaseFactory.AddInParameter(ref cmd, "@sakod_feeamountlocalin", DbType.Double, argEn.FSAmount);
                    _DatabaseFactory.AddInParameter(ref cmd, "@sakod_feeamountlocalout", DbType.Double, argEn.sakodfeeamountlocalout);
                    _DatabaseFactory.AddInParameter(ref cmd, "@sakod_gstlocalin", DbType.Double, argEn.sakodgstamountlocalin);
                    _DatabaseFactory.AddInParameter(ref cmd, "@sakod_gstlocalout", DbType.Double, argEn.sakodfeegstamountlocalout);
                    _DatabaseFactory.AddInParameter(ref cmd, "@safd_type", DbType.String, argEn.FTCode);
                    _DatabaseFactory.AddInParameter(ref cmd, "@saft_taxmode", DbType.Int32, argEn.sakod_idkoko);
                    //_DatabaseFactory.AddInParameter(ref cmd, "@sakod_feeamountinterin", DbType.Double, argEn.sakodfeeamountinterin);
                    //_DatabaseFactory.AddInParameter(ref cmd, "@sakod_feeamountinterout", DbType.Double, argEn.sakodfeeamountinterout);
                    //_DatabaseFactory.AddInParameter(ref cmd, "@sakod_gstinterin", DbType.Double, argEn.sakodgstamountinterin);
                    //_DatabaseFactory.AddInParameter(ref cmd, "@sakod_gstinterout", DbType.Double, argEn.sakodgstamountinterout);
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

        #region GetGstMode

        /// <summary>
        /// Method to Get FeeTypes Entity
        /// </summary>
        /// <param name="argEn">FeeTypes Entity is an Input.FeeTypeCode ia an Input Property</param>
        /// <returns>Returns FeeTypes Entity</returns>
        public int GetGstMode(int Tax)
        {
            FeeTypesEn loItem = new FeeTypesEn();
            int Taxmode = 0;

            try
            {
                string sqlCmd = "select sas_taxmod As Taxmode from sas_gst_taxsetup WHERE sas_taxid = '" + Tax + "' ";
                IDataReader _IDataReader = _DatabaseFactory.ExecuteReader(Helper.GetDataBaseType,
                   DataBaseConnectionString, sqlCmd).CreateDataReader();
                //Insert Data - Stop

                //if record exists - Start
                if (_IDataReader.Read())
                {
                    //get receipt id
                    Taxmode = clsGeneric.NullToInteger(_IDataReader[0]);
                }
                //if record exists - Stop

                return Taxmode;
                //if (!FormHelp.IsBlank(sqlCmd))
                //{
                //    DbCommand cmd = _DatabaseFactory.GetDbCommand(Helper.GetDataBaseType, sqlCmd, DataBaseConnectionString);
                //    _DbParameterCollection = cmd.Parameters;

                //    using (IDataReader loReader = _DatabaseFactory.GetIDataReader(Helper.GetDataBaseType, cmd,
                //        DataBaseConnectionString, sqlCmd, _DbParameterCollection).CreateDataReader())
                //    {
                //        if (loReader != null)
                //        {
                //            loReader.Read();
                //            loItem.TaxId = GetValue<int>(loReader, "sas_taxmod");
                //        }
                //        loReader.Close();
                //    }
                //}
            }
            catch (Exception ex)
            {
                MaxModule.Helper.LogError(ex.Message);
                return 0;
            }
        }

        #endregion

    }

}
