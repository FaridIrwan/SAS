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
    public class BidangDAL
    {
        #region Global Declarations

        private DbParameterCollection _DbParameterCollection = null;

        private MaxModule.DatabaseProvider _DatabaseFactory =
            new MaxModule.DatabaseProvider();

        private string DataBaseConnectionString = Helper.
           GetConnectionString();

        #endregion

        public BidangDAL()
        {
        }

        #region Insert
        
        public bool Insert(BidangEn argEn)
        {
            bool lbRes = false;
            int iOut = 0;
            BidangEn eobjSCAccess = new BidangEn();
            BidangDAL dobjSCAccess = new BidangDAL();
            string sqlCmd = "Select count(*) as cnt From sas_bidang WHERE sabp_code = @sabp_code or sabp_desc = @sabp_desc";
            try
            {
                if (!FormHelp.IsBlank(sqlCmd))
                {
                    DbCommand cmdSel = _DatabaseFactory.GetDbCommand(Helper.GetDataBaseType, sqlCmd, DataBaseConnectionString);
                    _DatabaseFactory.AddInParameter(ref cmdSel, "@sabp_code", DbType.String, argEn.BidangCode);
                    _DatabaseFactory.AddInParameter(ref cmdSel, "@sabp_desc", DbType.String, argEn.Description);
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
                        sqlCmd = "INSERT INTO sas_bidang(sabp_code,sabp_desc,sabp_status,sabp_updatedby,sabp_updateddttm) " +
                                    "VALUES (@sabp_code,@sabp_desc,@sabp_status,@sabp_updatedby,@sabp_updateddttm) ";

                        if (!FormHelp.IsBlank(sqlCmd))
                        {
                            DbCommand cmd = _DatabaseFactory.GetDbCommand(Helper.GetDataBaseType, sqlCmd, DataBaseConnectionString);
                            _DatabaseFactory.AddInParameter(ref cmd, "@sabp_code", DbType.String, argEn.BidangCode);
                            _DatabaseFactory.AddInParameter(ref cmd, "@sabp_desc", DbType.String, argEn.Description);
                            _DatabaseFactory.AddInParameter(ref cmd, "@sabp_status", DbType.Boolean, argEn.Status);                            
                            _DatabaseFactory.AddInParameter(ref cmd, "@sabp_updatedby", DbType.String, argEn.UpdatedBy);
                            _DatabaseFactory.AddInParameter(ref cmd, "@sabp_updateddttm", DbType.String, argEn.UpdatedDtTm);
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
        
        public bool Update(BidangEn argEn)
        {
            bool lbRes = false;
            int iOut = 0;
            string sqlCmd = "Select count(*) as cnt From sas_bidang WHERE sabp_code != @sabp_code and sabp_desc = @sabp_desc";
            try
            {
                if (!FormHelp.IsBlank(sqlCmd))
                {
                    DbCommand cmdSel = _DatabaseFactory.GetDbCommand(Helper.GetDataBaseType, sqlCmd, DataBaseConnectionString);
                    _DatabaseFactory.AddInParameter(ref cmdSel, "@sabp_code", DbType.String, argEn.BidangCode);
                    _DatabaseFactory.AddInParameter(ref cmdSel, "@sabp_desc", DbType.String, argEn.Description);
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
                        sqlCmd = "UPDATE sas_bidang SET sabp_desc = @sabp_desc, sabp_status = @sabp_status, " +
                                    "sabp_updatedby = @sabp_updatedby, sabp_updateddttm = @sabp_updateddttm WHERE sabp_code = @sabp_code";

                        if (!FormHelp.IsBlank(sqlCmd))
                        {
                            DbCommand cmd = _DatabaseFactory.GetDbCommand(Helper.GetDataBaseType, sqlCmd, DataBaseConnectionString);
                            _DatabaseFactory.AddInParameter(ref cmd, "@sabp_code", DbType.String, argEn.BidangCode);
                            _DatabaseFactory.AddInParameter(ref cmd, "@sabp_desc", DbType.String, argEn.Description);
                            _DatabaseFactory.AddInParameter(ref cmd, "@sabp_status", DbType.Boolean, argEn.Status);
                            _DatabaseFactory.AddInParameter(ref cmd, "@sabp_updatedby", DbType.String, argEn.UpdatedBy);
                            _DatabaseFactory.AddInParameter(ref cmd, "@sabp_updateddttm", DbType.String, argEn.UpdatedDtTm);
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

        #region GetList

        public List<BidangEn> GetList(BidangEn argEn)
        {
            List<BidangEn> loEnList = new List<BidangEn>();
            string sqlCmd = "select * from sas_bidang ORDER BY sabp_code; ";

            try
            {
                if (!FormHelp.IsBlank(sqlCmd))
                {
                    using (IDataReader loReader = _DatabaseFactory.ExecuteReader(Helper.GetDataBaseType,
                        DataBaseConnectionString, sqlCmd).CreateDataReader())
                    {
                        while (loReader.Read())
                        {
                            BidangEn loItem = LoadObject(loReader);
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

        #region GetBidangList

        public List<BidangEn> GetBidangList(BidangEn argEn)
        {
            List<BidangEn> loEnList = new List<BidangEn>();
            string sqlCmd = "select * from sas_bidang WHERE sabp_status = true; ";

            try
            {
                if (!FormHelp.IsBlank(sqlCmd))
                {
                    using (IDataReader loReader = _DatabaseFactory.ExecuteReader(Helper.GetDataBaseType,
                        DataBaseConnectionString, sqlCmd).CreateDataReader())
                    {
                        while (loReader.Read())
                        {
                            BidangEn loItem = LoadObject(loReader);
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

        #region GetSelectedBidang

        public BidangEn GetSelectedBidang(string argEn)
        {
            BidangEn loItem = new BidangEn();
            string sqlCmd = "select * from sas_bidang WHERE sabp_code ='" + argEn + "'";

            try
            {
                if (!FormHelp.IsBlank(sqlCmd))
                {
                    using (IDataReader loReader = _DatabaseFactory.ExecuteReader(Helper.GetDataBaseType,
                        DataBaseConnectionString, sqlCmd).CreateDataReader())
                    {
                        while (loReader.Read())
                        {
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

        #region LoadObject
        
        private BidangEn LoadObject(IDataReader argReader)
        {
            BidangEn loItem = new BidangEn();
            loItem.BidangCode = GetValue<string>(argReader, "sabp_code");
            loItem.Description = GetValue<string>(argReader, "sabp_desc");
            loItem.Status = GetValue<bool>(argReader, "sabp_status");

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

        #region Delete
       
        public bool Delete(BidangEn argEn)
        {
            //variable declarations
            string SqlStatement = null; int HasRows = 0; int RecordsDeleted = 0; bool Result = false;

            try
            {
                //Build Sql Statement - Start
                SqlStatement = "SELECT count(*) as rows FROM SAS_Program where sabp_code = ";
                SqlStatement += clsGeneric.AddQuotes(argEn.BidangCode);
                //Build Sql Statement - Stop

                //Get Program having BidangCode selected - Start
                IDataReader _IDataReader = _DatabaseFactory.ExecuteReader(Helper.GetDataBaseType,
                        DataBaseConnectionString, SqlStatement).CreateDataReader();
                //Get Program having BidangCode selected - Stop

                //if details available - Start
                if (_IDataReader != null)
                {
                    if (_IDataReader.Read())
                    {
                        HasRows = clsGeneric.NullToInteger(_IDataReader["rows"]);
                        if (HasRows > 0)
                            throw new Exception("Bidang Code Already in Use");
                    }

                    _IDataReader.Close();
                    _IDataReader.Dispose();

                    //if record not used - Start
                    if (HasRows == 0)
                    {
                        //build sql statement - Start
                        SqlStatement = "DELETE FROM sas_bidang WHERE sabp_code = ";
                        SqlStatement += clsGeneric.AddQuotes(argEn.BidangCode);
                        //build sql statement - Stop

                        //Save Details to Database
                        RecordsDeleted = _DatabaseFactory.ExecuteSqlStatement(Helper.GetDataBaseType,
                            DataBaseConnectionString, SqlStatement);

                        //if records saved successfully - Start
                        if (RecordsDeleted > -1)
                            Result = true;
                        else
                            throw new Exception("Delete Failed! No Row has been deleted...");
                        //if records saved successfully - Stop
                    }
                    //if record not used - Sto

                    return Result;
                }
                //if details available - Stop

                return false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion
    }
}
