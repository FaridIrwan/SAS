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
    /// Class to handle all the HostelStructureDetails Methods.
    /// </summary>
    public class HostelStrDetailsDAL
    {
        #region Global Declarations 

        private DbParameterCollection _DbParameterCollection = null;

        private MaxModule.DatabaseProvider _DatabaseFactory =
            new MaxModule.DatabaseProvider();

        private string DataBaseConnectionString = Helper.
           GetConnectionString();

        #endregion

        public HostelStrDetailsDAL()
        {
        }

        #region GetList 

        /// <summary>
        /// Method to Get List of HostelStrDetails
        /// </summary>
        /// <param name="argEn">HostelStrDetails Entity is an Input.</param>
        /// <returns>Returns List of HostelStrDetails</returns>
        public List<HostelStrDetailsEn> GetList(HostelStrDetailsEn argEn)
        {
            List<HostelStrDetailsEn> loEnList = new List<HostelStrDetailsEn>();
            string sqlCmd = "select * from SAS_HostelStrDetails";
            
            try
            {
                if (!FormHelp.IsBlank(sqlCmd))
                {
                    using (IDataReader loReader = _DatabaseFactory.ExecuteReader(Helper.GetDataBaseType,
                        DataBaseConnectionString, sqlCmd).CreateDataReader())
                    {
                        while (loReader.Read())
                        {
                            HostelStrDetailsEn loItem = LoadObject(loReader);
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

        #region GetHostelAmtList 

        /// <summary>
        /// Method to Get List of Hostel Amounts
        /// </summary>
        /// <param name="argEn">HostelStrDetails Entity is an Input.HSCode as input Property</param>
        /// <returns>Returns List of HostelStrDetails</returns>
        public List<HostelStrDetailsEn> GetHostelAmtList(HostelStrDetailsEn argEn)
        {
            List<HostelStrDetailsEn> loEnList = new List<HostelStrDetailsEn>();
            string sqlCmd = "SELECT SAS_HostelStrDetails.*, SAS_FeeTypes.SAFT_Desc " +
                            "FROM SAS_HostelStrDetails INNER JOIN SAS_FeeTypes ON SAS_HostelStrDetails.SAFT_Code = SAS_FeeTypes.SAFT_Code " +
                            "WHERE SAS_HostelStrDetails.SAHS_Code = @SAHS_Code";

            try
            {
                if (!FormHelp.IsBlank(sqlCmd))
                {
                    DbCommand cmd = _DatabaseFactory.GetDbCommand(Helper.GetDataBaseType, sqlCmd, DataBaseConnectionString);
                    _DatabaseFactory.AddInParameter(ref cmd, "@SAHS_Code", DbType.String, argEn.HSCode);
                    _DbParameterCollection = cmd.Parameters;

                    using (IDataReader loReader = _DatabaseFactory.GetIDataReader(Helper.GetDataBaseType, cmd,
                        DataBaseConnectionString, sqlCmd, _DbParameterCollection).CreateDataReader())
                    {
                        HostelStrAmountDAL loHostelStrAmt = new HostelStrAmountDAL();
                        HostelStrAmountEn loHostelStrAmtEn = new HostelStrAmountEn();
                        while (loReader.Read())
                        {
                            HostelStrDetailsEn loItem = LoadObject(loReader);
                            loItem.Description = GetValue<string>(loReader, "SAFT_Desc");
                            loHostelStrAmtEn.HSCode = loItem.HSCode;
                            loHostelStrAmtEn.FTCode = loItem.FTCode;
                            //getting the list of Hostel FeeAmounts
                            loItem.ListFeeAmount = loHostelStrAmt.GetDescList(loHostelStrAmtEn);
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
        /// Method to Get HostelStrDetails Entity
        /// </summary>
        /// <param name="argEn">HostelStrDetails Entity is an Input.HSCode as Input Property.</param>
        /// <returns>Returns HostelStrDetails Entity</returns>
        public HostelStrDetailsEn GetItem(HostelStrDetailsEn argEn)
        {
            HostelStrDetailsEn loItem = new HostelStrDetailsEn();
            string sqlCmd = "Select * FROM SAS_HostelStrDetails WHERE SAHS_Code = @SAHS_Code";
            
            try
            {
                if (!FormHelp.IsBlank(sqlCmd))
                {
                    DbCommand cmd = _DatabaseFactory.GetDbCommand(Helper.GetDataBaseType, sqlCmd, DataBaseConnectionString);
                    _DatabaseFactory.AddInParameter(ref cmd, "@SAHS_Code", DbType.String, argEn.HSCode);
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
        /// Method to Insert HostelStrDetails 
        /// </summary>
        /// <param name="argEn">HostelStrDetails Entity is an Input.</param>
        /// <returns>Returns Boolean</returns>
        public bool Insert(HostelStrDetailsEn argEn)
        {
            bool lbRes = false;
            //int iOut = 0;

            try
            {
                string sqlCmd = "INSERT INTO SAS_HostelStrDetails(SAHS_Code,SAHD_Code,SAFT_Code,SAHD_Type,SAHD_Priority, safs_taxmode) VALUES (@SAHS_Code,@SAHD_Code,@SAFT_Code,@SAHD_Type,@SAHD_Priority, @safs_taxmode) ";

                if (!FormHelp.IsBlank(sqlCmd))
                {
                    DbCommand cmd = _DatabaseFactory.GetDbCommand(Helper.GetDataBaseType, sqlCmd, DataBaseConnectionString);
                    _DatabaseFactory.AddInParameter(ref cmd, "@SAHS_Code", DbType.String, clsGeneric.NullToString(argEn.HSCode));
                    _DatabaseFactory.AddInParameter(ref cmd, "@SAHD_Code", DbType.String, clsGeneric.NullToString(argEn.HDCode));
                    _DatabaseFactory.AddInParameter(ref cmd, "@SAFT_Code", DbType.String, clsGeneric.NullToString(argEn.FTCode));
                    _DatabaseFactory.AddInParameter(ref cmd, "@SAHD_Type", DbType.String, clsGeneric.NullToString(argEn.HDType));
                    _DatabaseFactory.AddInParameter(ref cmd, "@SAHD_Priority", DbType.Int32, clsGeneric.NullToInteger(argEn.HDPriority));
                    _DatabaseFactory.AddInParameter(ref cmd, "@safs_taxmode", DbType.Int32, clsGeneric.NullToInteger(argEn.TaxId));
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
        /// Method to Update HostelStrDetails 
        /// </summary>
        /// <param name="argEn">HostelStrDetails Entity is an Input.</param>
        /// <returns>Returns Boolean</returns>
        public bool Update(HostelStrDetailsEn argEn)
        {
            bool lbRes = false;
            int iOut = 0;
            string sqlCmd = "Select count(*) as cnt From SAS_HostelStrDetails WHERE SAHS_Code = @SAHS_Code";
            try
            {
                if (!FormHelp.IsBlank(sqlCmd))
                {
                    DbCommand cmdSel = _DatabaseFactory.GetDbCommand(Helper.GetDataBaseType, sqlCmd, DataBaseConnectionString);
                    _DatabaseFactory.AddInParameter(ref cmdSel, "@SAHS_Code", DbType.String, argEn.HSCode);
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
                        sqlCmd = "UPDATE SAS_HostelStrDetails SET SAHS_Code = @SAHS_Code, SAHD_Code = @SAHD_Code, SAFT_Code = @SAFT_Code, SAHD_Type = @SAHD_Type, SAHD_Priority = @SAHD_Priority WHERE SAHS_Code = @SAHS_Code";
                        
                        if (!FormHelp.IsBlank(sqlCmd))
                        {
                            DbCommand cmd = _DatabaseFactory.GetDbCommand(Helper.GetDataBaseType, sqlCmd, DataBaseConnectionString);
                            _DatabaseFactory.AddInParameter(ref cmd, "@SAHS_Code", DbType.String, argEn.HSCode);
                            _DatabaseFactory.AddInParameter(ref cmd, "@SAHD_Code", DbType.String, argEn.HDCode);
                            _DatabaseFactory.AddInParameter(ref cmd, "@SAFT_Code", DbType.String, argEn.FTCode);
                            _DatabaseFactory.AddInParameter(ref cmd, "@SAHD_Type", DbType.String, argEn.HDType);
                            _DatabaseFactory.AddInParameter(ref cmd, "@SAHD_Priority", DbType.Int32, argEn.HDPriority);
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
        /// Method to Delete HostelStrDetails 
        /// </summary>
        /// <param name="argEn">HostelStrDetails Entity is an Input.</param>
        /// <returns>Returns Boolean</returns>
        public bool Delete(HostelStrDetailsEn argEn)
        {
            bool lbRes = false;
            string sqlCmd = "DELETE FROM SAS_HostelStrDetails WHERE SAHS_Code = @SAHS_Code";

            try
            {
                if (!FormHelp.IsBlank(sqlCmd))
                {
                    DbCommand cmd = _DatabaseFactory.GetDbCommand(Helper.GetDataBaseType, sqlCmd, DataBaseConnectionString);
                    _DatabaseFactory.AddInParameter(ref cmd, "@SAHS_Code", DbType.String, argEn.HSCode);
                    _DbParameterCollection = cmd.Parameters;

                    int liRowAffected = _DatabaseFactory.ExecuteNonQuery(Helper.GetDataBaseType, cmd,
                                DataBaseConnectionString, sqlCmd, _DbParameterCollection);

                    if (liRowAffected > -1)
                        lbRes = true;
                    else
                        throw new Exception("Deletion Failed! No Row has been updated...");
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
        /// Method to Load HostelStrDetails Entity 
        /// </summary>
        /// <param name="argReader">IDataReader Object is an Input.</param>
        /// <returns>Returns HostelStrDetails Entity</returns>
        private HostelStrDetailsEn LoadObject(IDataReader argReader)
        {
            HostelStrDetailsEn loItem = new HostelStrDetailsEn();
            loItem.HSCode = GetValue<string>(argReader, "SAHS_Code");
            loItem.HDCode = GetValue<string>(argReader, "SAHD_Code");
            loItem.FTCode = GetValue<string>(argReader, "SAFT_Code");
            loItem.HDType = GetValue<string>(argReader, "SAHD_Type");
            loItem.HDPriority = GetValue<int>(argReader, "SAHD_Priority");

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

        #region GetHostelDetailsAmtList

        //Created By : Jessica
        //Created Date :  19/02/16
        /// <summary>
        /// Method to Get List of Hostel Amounts
        /// </summary>
        /// <param name="argEn">HostelStruct Entity is an Input.HSCode as input Property</param>
        /// <returns>Returns List of HostelStruct</returns>
        public List<HostelStructEn> GetHostelDetailsAmtList(HostelStructEn argEn)
        {
            List<HostelStructEn> loEnList = new List<HostelStructEn>();
            string sqlCmd = @"
                            select  hsd.SAHS_Code , hsd.saft_code, hsd.sahd_priority,
                            hsd.safs_taxmode, ft.*,
                            local.saha_amount as Local_Amount, local.safa_gstamount as Local_GSTAmount, nonlocal.saha_amount as NonLocal_Amount, nonlocal.safa_gstamount as NonLocal_GSTAmount
                            , local.sasc_code as Local_Category, nonlocal.sasc_code as NonLocal_Category, hsd.sahd_type
                            from  sas_hostelstrDetails hsd 
                            inner join SAS_FeeTypes ft on ft.saft_code = hsd.saft_code
                            left join (select saha_amount,  safa_gstamount, sahs_code, saft_code, sasc_code from sas_hostelstrAmount where sas_hostelstrAmount.sasc_code in ('Local', 'W')  )as local on local.sahs_code = hsd.sahs_code and local.saft_code = hsd.saft_code
                            left join (select saha_amount,  safa_gstamount, sahs_code, saft_code, sasc_code from sas_hostelstrAmount where sas_hostelstrAmount.sasc_code in ('International', 'BW')  )as nonlocal on nonlocal.sahs_code = hsd.sahs_code and nonlocal.saft_code = hsd.saft_code
                            WHERE hsd.SAHS_Code = @SAHS_Code";
            sqlCmd = sqlCmd + " order by hsd.saft_code";

            try
            {
                if (!FormHelp.IsBlank(sqlCmd))
                {
                    DbCommand cmd = _DatabaseFactory.GetDbCommand(Helper.GetDataBaseType, sqlCmd, DataBaseConnectionString);
                    _DatabaseFactory.AddInParameter(ref cmd, "@SAHS_Code", DbType.String, argEn.HostelStructureCode);
                    _DbParameterCollection = cmd.Parameters;

                    using (IDataReader loReader = _DatabaseFactory.GetIDataReader(Helper.GetDataBaseType, cmd,
                        DataBaseConnectionString, sqlCmd, _DbParameterCollection).CreateDataReader())
                    {
                        HostelStrAmountDAL loHostelStrAmt = new HostelStrAmountDAL();
                        HostelStrAmountEn loHostelStrAmtEn = new HostelStrAmountEn();
                        while (loReader.Read())
                        {

                            HostelStructEn loItemDetails = new HostelStructEn();
                            loItemDetails.HostelStructureCode = GetValue<string>(loReader, "SAHS_Code");
                            loItemDetails.FTCode = GetValue<string>(loReader, "saft_code");
                            loItemDetails.Description = GetValue<string>(loReader, "saft_desc");
                            loItemDetails.LocalAmount = GetValue<double>(loReader, "Local_Amount");
                            loItemDetails.LocalGSTAmount = GetValue<double>(loReader, "Local_GSTAmount");
                            loItemDetails.NonLocalAmount = GetValue<double>(loReader, "NonLocal_Amount");
                            loItemDetails.NonLocalGSTAmount = GetValue<double>(loReader, "NonLocal_GSTAmount");
                            loItemDetails.TaxId = GetValue<int>(loReader, "safs_taxmode");
                            loItemDetails.NonLocalTempAmount = loItemDetails.NonLocalAmount - loItemDetails.NonLocalGSTAmount;
                            loItemDetails.LocalTempAmount = loItemDetails.LocalAmount - loItemDetails.LocalGSTAmount;
                            loItemDetails.LocalCategory = GetValue<string>(loReader, "Local_Category");
                            loItemDetails.NonLocalCategory = GetValue<string>(loReader, "NonLocal_Category");
                            loItemDetails.FeeCategory = GetValue<string>(loReader, "sahd_type");
                            loItemDetails.Priority = GetValue<int>(loReader, "sahd_priority");
                            loEnList.Add(loItemDetails);
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
        
    }

}
