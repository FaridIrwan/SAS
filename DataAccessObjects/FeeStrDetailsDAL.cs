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
    /// Class to handle all the FeeStructureDetails Methods.
    /// </summary>
    public class FeeStrDetailsDAL
    {
        #region Global Declarations 

        private DbParameterCollection _DbParameterCollection = null;

        private MaxModule.DatabaseProvider _DatabaseFactory =
            new MaxModule.DatabaseProvider();

        private string DataBaseConnectionString = Helper.
           GetConnectionString();

        #endregion

        public FeeStrDetailsDAL()
        {
        }

        #region GetList 

        /// <summary>
        /// Method to Get List of FeeStrDetails
        /// </summary>
        /// <param name="argEn">FeeStrDetails Entity is an Input.</param>
        /// <returns>Returns List of FeeStrDetails</returns>
        public List<FeeStrDetailsEn> GetList(FeeStrDetailsEn argEn)
        {
            List<FeeStrDetailsEn> loEnList = new List<FeeStrDetailsEn>();
            string sqlCmd = "select * from SAS_FeeStrDetails";
            
            try
            {
                if (!FormHelp.IsBlank(sqlCmd))
                {
                    using (IDataReader loReader = _DatabaseFactory.ExecuteReader(Helper.GetDataBaseType,
                        DataBaseConnectionString, sqlCmd).CreateDataReader())
                    {
                        while (loReader.Read())
                        {
                            FeeStrDetailsEn loItem = LoadObject(loReader);
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

        #region GetFeeSDList 

        /// <summary>
        /// Method to Get List of FeeStrDetails
        /// </summary>
        /// <param name="argEn">FeeStrDetails Entity is an Input.FScode is an Input Property.</param>
        /// <returns>Returns List of FeeStrDetails</returns>
        public List<FeeStrDetailsEn> GetFeeSDList(FeeStrDetailsEn argEn)
        {
            List<FeeStrDetailsEn> loEnList = new List<FeeStrDetailsEn>();
            string sqlCmd = "SELECT SAS_FeeStrDetails.*, SAS_FeeTypes.SAFT_Desc FROM SAS_FeeStrDetails "+
                "INNER JOIN SAS_FeeTypes ON SAS_FeeStrDetails.SAFT_Code = SAS_FeeTypes.SAFT_Code Where SAS_FeeStrDetails.SAFS_Code = @SAFS_Code";
             
            
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
                        while (loReader.Read())
                        {
                            FeeStrAmountDAL loFeeSAmt = new FeeStrAmountDAL();
                            FeeStrAmountEn loFeeSAmtEn = new FeeStrAmountEn();
                            FeeStrDetailsEn loItem = LoadObject(loReader);
                            loItem.FeeDesc = GetValue<string>(loReader, "SAFT_Desc");
                            loFeeSAmtEn.FSCode = loItem.FSCode;
                            loFeeSAmtEn.Type = loItem.Type;
                            loFeeSAmtEn.FTCode = loItem.FTCode;
                           // loFeeSAmtEn.GSTAmount = loItem.gs;
                            //Getting list of feeamounts
                            loItem.ListFeeAmount = loFeeSAmt.GetFSAmtList(loFeeSAmtEn);
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

        public List<FeeStructEn> GetFeeSDAmountList(FeeStructEn argEn)
        {
            List<FeeStructEn> loEnList = new List<FeeStructEn>();
//            string sqlCmd = @"
//                select fsd.safs_code, fsd.safd_type, fsd.safd_priority, fsd.safs_taxmode, ft.*,  local.safa_amount as local_amount, local.safa_gstamount as local_gstamount, 
//                local.sasc_code as local_cateogry, nonlocal.safa_amount as nonlocal_amount, nonlocal.safa_gstamount as nonlocal_gstamount, nonlocal.sasc_code as nonlocal_cateogry,
//                fsd.safd_feefor, fsd.safd_sem
//                from sas_feestrdetails fsd
//                inner join sas_feetypes ft on ft.saft_code = fsd.saft_code
//                left join (select * from sas_feestramount fsa where fsa.sasc_code in ('Local', 'W') )as local on local.safs_code = fsd.safs_code and local.saft_code = fsd.saft_code and local.safd_type = fsd.safd_type
//                and local.safd_feefor = fsd.safd_feefor and local.safd_sem = fsd.safd_sem
//                left join (select * from sas_feestramount fsa where fsa.sasc_code in ('International', 'BW') )as nonlocal on nonlocal.safs_code = fsd.safs_code and nonlocal.saft_code = fsd.saft_code and  nonlocal.safd_type = fsd.safd_type
//                and nonlocal.safd_feefor = fsd.safd_feefor and nonlocal.safd_sem = fsd.safd_sem
//                where fsd.safs_code =  @SAFS_Code";

            string sqlCmd = @"
                select fsd.safs_code, fsd.safd_type, fsd.safd_priority, fsd.safs_taxmode, ft.*,  local.safa_amount as local_amount, local.safa_gstamount as local_gstamount, 
                local.sasc_code as local_cateogry, nonlocal.safa_amount as nonlocal_amount, nonlocal.safa_gstamount as nonlocal_gstamount, nonlocal.sasc_code as nonlocal_cateogry,
                fsd.safd_feefor, fsd.safd_sem, case when fsd.safd_type = 'T' then fs.safd_feebaseon else '-1' end as safd_feebaseon
                from SAS_FeeStruct fs inner join sas_feestrdetails fsd on fs.safs_code = fsd.safs_code
                inner join sas_feetypes ft on ft.saft_code = fsd.saft_code
                left join (select * from sas_feestramount fsa where fsa.sasc_code in ('Local', 'W') )as local on local.safs_code = fsd.safs_code and local.saft_code = fsd.saft_code and local.safd_type = fsd.safd_type
                and local.safd_feefor = fsd.safd_feefor and local.safd_sem = fsd.safd_sem
                left join (select * from sas_feestramount fsa where fsa.sasc_code in ('International', 'BW') )as nonlocal on nonlocal.safs_code = fsd.safs_code and nonlocal.saft_code = fsd.saft_code and  nonlocal.safd_type = fsd.safd_type
                and nonlocal.safd_feefor = fsd.safd_feefor and nonlocal.safd_sem = fsd.safd_sem
                where fsd.safs_code = @SAFS_Code order by fsd.safd_type";

            try
            {
                if (!FormHelp.IsBlank(sqlCmd))
                {
                    DbCommand cmd = _DatabaseFactory.GetDbCommand(Helper.GetDataBaseType, sqlCmd, DataBaseConnectionString);
                    _DatabaseFactory.AddInParameter(ref cmd, "@SAFS_Code", DbType.String, argEn.FeeStructureCode);
                    _DbParameterCollection = cmd.Parameters;

                    using (IDataReader loReader = _DatabaseFactory.GetIDataReader(Helper.GetDataBaseType, cmd,
                        DataBaseConnectionString, sqlCmd, _DbParameterCollection).CreateDataReader())
                    {
                        while (loReader.Read())
                        {
                            FeeStructEn loItem = new FeeStructEn();
                            loItem.FeeStructureCode = GetValue<string>(loReader, "safs_code");
                            loItem.FTCode = GetValue<string>(loReader, "saft_code");
                            loItem.Description = GetValue<string>(loReader, "saft_desc");
                            loItem.LocalAmount = GetValue<double>(loReader, "local_amount");
                            loItem.LocalGSTAmount = GetValue<double>(loReader, "local_gstamount");
                            loItem.NonLocalAmount = GetValue<double>(loReader, "nonlocal_amount");
                            loItem.NonLocalGSTAmount = GetValue<double>(loReader, "nonlocal_gstamount");
                            loItem.TaxId = GetValue<int>(loReader, "safs_taxmode");
                            loItem.NonLocalTempAmount = loItem.NonLocalAmount - loItem.NonLocalGSTAmount;
                            loItem.LocalTempAmount = loItem.LocalAmount - loItem.LocalGSTAmount;
                            loItem.LocalCategory = GetValue<string>(loReader, "local_cateogry");
                            loItem.NonLocalCategory = GetValue<string>(loReader, "nonlocal_cateogry");
                            loItem.FeeType = GetValue<string>(loReader, "safd_type");
                            loItem.Priority = GetValue<int>(loReader, "safd_priority");
                            loItem.FeeFor = GetValue<string>(loReader, "safd_feefor");
                            loItem.FeeDetailSem = GetValue<int>(loReader, "safd_sem");
                            loItem.FeeBaseOn = GetValue<string>(loReader, "safd_feebaseon");
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
        /// Method to Get FeeStrDetails Entity
        /// </summary>
        /// <param name="argEn">FeeStrDetails Entity is an Input</param>
        /// <returns>Returns FeeStrDetails Entity</returns>
        public FeeStrDetailsEn GetItem(FeeStrDetailsEn argEn)
        {
            FeeStrDetailsEn loItem = new FeeStrDetailsEn();
            string sqlCmd = "Select * FROM SAS_FeeStrDetails WHERE SAFS_Code = @SAFS_Code";
            
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
        /// Method to Insert FeeStrDetails 
        /// </summary>
        /// <param name="argEn">FeeStrDetails Entity is an Input.</param>
        /// <returns>Returns Boolean</returns>
        public bool Insert(FeeStrDetailsEn argEn)
        {
            bool lbRes = false;

            try
            {
                string sqlCmd = "INSERT INTO SAS_FeeStrDetails(SAFS_Code,SAFD_Type,SAFT_Code,SAFD_Priority," +
                "SAFD_FeeFor,SAFD_Sem,safs_taxmode) VALUES (@SAFS_Code,@SAFD_Type,@SAFT_Code,@SAFD_Priority,@SAFD_FeeFor,@SAFD_Sem,@safs_taxmode) ";

                if (!FormHelp.IsBlank(sqlCmd))
                {
                    DbCommand cmd = _DatabaseFactory.GetDbCommand(Helper.GetDataBaseType, sqlCmd, DataBaseConnectionString);
                    _DatabaseFactory.AddInParameter(ref cmd, "@SAFS_Code", DbType.String, clsGeneric.NullToString(argEn.FSCode));
                    _DatabaseFactory.AddInParameter(ref cmd, "@SAFD_Type", DbType.String, clsGeneric.NullToString(argEn.Type));
                    _DatabaseFactory.AddInParameter(ref cmd, "@SAFT_Code", DbType.String, clsGeneric.NullToString(argEn.FTCode));
                    _DatabaseFactory.AddInParameter(ref cmd, "@SAFD_Priority", DbType.Int32, clsGeneric.NullToInteger(argEn.Priority));
                    _DatabaseFactory.AddInParameter(ref cmd, "@SAFD_FeeFor", DbType.String, clsGeneric.NullToString(argEn.FeeFor));
                    _DatabaseFactory.AddInParameter(ref cmd, "@SAFD_Sem", DbType.Int32, clsGeneric.NullToInteger(argEn.Sem));
                    _DatabaseFactory.AddInParameter(ref cmd, "@safs_taxmode", DbType.Int16, clsGeneric.NullToInteger(argEn._TaxId));
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
        /// Method to Update FeeStrDetails 
        /// </summary>
        /// <param name="argEn">FeeStrDetails Entity is an Input.</param>
        /// <returns>Returns Boolean</returns>
        public bool Update(FeeStrDetailsEn argEn)
        {
            bool lbRes = false;
            int iOut = 0;
            string sqlCmd = "Select count(*) as cnt From SAS_FeeStrDetails WHERE SAFS_Code = @SAFS_Code";
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
                        sqlCmd = "UPDATE SAS_FeeStrDetails SET SAFS_Code = @SAFS_Code,SAFD_Type = @SAFD_Type, SAFT_Code = @SAFT_Code, SAFD_Priority = @SAFD_Priority, SAFD_FeeFor = @SAFD_FeeFor, SAFD_Sem = @SAFD_Sem,safs_taxmode=@safs_taxmode WHERE SAFS_Code = @SAFS_Code";

                        if (!FormHelp.IsBlank(sqlCmd))
                        {
                            DbCommand cmd = _DatabaseFactory.GetDbCommand(Helper.GetDataBaseType, sqlCmd, DataBaseConnectionString);
                            _DatabaseFactory.AddInParameter(ref cmd, "@SAFS_Code", DbType.String, argEn.FSCode);
                            _DatabaseFactory.AddInParameter(ref cmd, "@SAFD_Type", DbType.String, argEn.Type);
                            _DatabaseFactory.AddInParameter(ref cmd, "@SAFT_Code", DbType.String, argEn.FTCode);
                            _DatabaseFactory.AddInParameter(ref cmd, "@SAFD_Priority", DbType.Int32, argEn.Priority);
                            _DatabaseFactory.AddInParameter(ref cmd, "@SAFD_FeeFor", DbType.String, argEn.FeeFor);
                            _DatabaseFactory.AddInParameter(ref cmd, "@SAFD_Sem", DbType.Int32, argEn.Sem);
                            _DatabaseFactory.AddInParameter(ref cmd, "@safs_taxmode", DbType.Int16, argEn._TaxId);
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
        /// Method to Delete FeeStrDetails 
        /// </summary>
        /// <param name="argEn">FeeStrDetails Entity is an Input.</param>
        /// <returns>Returns Boolean</returns>
        public bool Delete(FeeStrDetailsEn argEn)
        {
            bool lbRes = false;
            string sqlCmd = "DELETE FROM SAS_FeeStrDetails WHERE SAFS_Code = @SAFS_Code";
            
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
        /// Method to Load FeeStrDetails Entity 
        /// </summary>
        /// <param name="argReader">IDataReader Object is an Input.</param>
        /// <returns>Returns FeeStrDetails Entity</returns>
        private FeeStrDetailsEn LoadObject(IDataReader argReader)
        {
            FeeStrDetailsEn loItem = new FeeStrDetailsEn();
            loItem.FSCode = GetValue<string>(argReader, "SAFS_Code");
            loItem.Type = GetValue<string>(argReader, "SAFD_Type");
            loItem.FTCode = GetValue<string>(argReader, "SAFT_Code");
            loItem.Priority = GetValue<int>(argReader, "SAFD_Priority");
            loItem.FeeFor = GetValue<string>(argReader, "SAFD_FeeFor");
            loItem.Sem = GetValue<int>(argReader, "SAFD_Sem");
            loItem._TaxId = GetValue<int>(argReader, "safs_taxmode");
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
