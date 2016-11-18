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
    /// Class to handle all the ChequeDetails.
    /// </summary>
    public class ChequeDetailsDAL
    {
        #region Global Declarations 

        private DbParameterCollection _DbParameterCollection = null;

        private MaxModule.DatabaseProvider _DatabaseFactory =
            new MaxModule.DatabaseProvider();

        private string DataBaseConnectionString = Helper.
           GetConnectionString();

        #endregion

        public ChequeDetailsDAL()
        {
        }

        #region GetList 

        /// <summary>
        /// Method to Get ChequeDetails 
        /// </summary>
        /// <param name="argEn">ChequeDetails Entity is an Input.ProcessID is Input Property.</param>
        /// <returns>Returns List of ChequeDetails</returns>
        public List<ChequeDetailsEn> GetList(ChequeDetailsEn argEn)
        {
            List<ChequeDetailsEn> loEnList = new List<ChequeDetailsEn>();
            string sqlCmd = "select * from SAS_ChequeDetails where processid = '"+argEn.ProcessId+"'";
            
            try
            {
                if (!FormHelp.IsBlank(sqlCmd))
                {
                    using (IDataReader loReader = _DatabaseFactory.ExecuteReader(Helper.GetDataBaseType,
                        DataBaseConnectionString, sqlCmd).CreateDataReader())
                    {
                        while (loReader.Read())
                        {
                            ChequeDetailsEn loItem = LoadObject(loReader);
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
        /// Method to Get ChequeDetails Item
        /// </summary>
        /// <param name="argEn">ChequeDetails Entity is an Input.</param>
        /// <returns>Returns a ChequeDetails Item</returns>
        public ChequeDetailsEn GetItem(ChequeDetailsEn argEn)
        {
            ChequeDetailsEn loItem = new ChequeDetailsEn();
            string sqlCmd = "Select * FROM SAS_ChequeDetails ";
            
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
        /// Method to Insert ChequeDetails
/// </summary>
        /// <param name="argEn">ChequeDetails Entity is the Input.</param>
        /// <returns>Returns a Boolean</returns>
        public bool Insert(ChequeDetailsEn argEn)
        {
            bool lbRes = false;

            try
            {

                string sqlCmd = "INSERT INTO SAS_ChequeDetails(ProcessId,ChequeStartNo,ChequeEndNo) VALUES (@ProcessId,@ChequeStartNo,@ChequeEndNo) ";

                if (!FormHelp.IsBlank(sqlCmd))
                {
                    DbCommand cmd = _DatabaseFactory.GetDbCommand(Helper.GetDataBaseType, sqlCmd, DataBaseConnectionString);
                    _DatabaseFactory.AddInParameter(ref cmd, "@ProcessId", DbType.String, argEn.ProcessId);
                    _DatabaseFactory.AddInParameter(ref cmd, "@ChequeStartNo", DbType.String, argEn.ChequeStartNo);
                    _DatabaseFactory.AddInParameter(ref cmd, "@ChequeEndNo", DbType.String, argEn.ChequeEndNo);
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
        /// Method to Update ChequeDetails
/// </summary>
        /// <param name="argEn">ChequeDetails Entity is the Input.</param>
        /// <returns>Returns a Boolean</returns>
        public bool Update(ChequeDetailsEn argEn)
        {
            bool lbRes = false;
            int iOut = 0;
            string sqlCmd = "Select count(*) as cnt From SAS_ChequeDetails WHERE ";
            try
            {
                if (!FormHelp.IsBlank(sqlCmd))
                {
                    using (IDataReader dr = _DatabaseFactory.ExecuteReader(Helper.GetDataBaseType,
                        DataBaseConnectionString, sqlCmd).CreateDataReader())
                    {
                        if (dr.Read())
                            iOut = clsGeneric.NullToInteger(dr["cnt"]);
                        if (iOut < 0)
                            throw new Exception("Update Failed! No Record Exist!");
                    }
                    if (iOut != 0)
                    {
                        sqlCmd = "UPDATE SAS_ChequeDetails SET ProcessId = @ProcessId, ChequeStartNo = @ChequeStartNo, ChequeEndNo = @ChequeEndNo WHERE ";

                        if (!FormHelp.IsBlank(sqlCmd))
                        {
                            DbCommand cmd = _DatabaseFactory.GetDbCommand(Helper.GetDataBaseType, sqlCmd, DataBaseConnectionString);
                            _DatabaseFactory.AddInParameter(ref cmd, "@ProcessId", DbType.String, argEn.ProcessId);
                            _DatabaseFactory.AddInParameter(ref cmd, "@ChequeStartNo", DbType.String, argEn.ChequeStartNo);
                            _DatabaseFactory.AddInParameter(ref cmd, "@ChequeEndNo", DbType.String, argEn.ChequeEndNo);
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
        /// Method to Delete  ChequeDetails
/// </summary>
        /// <param name="argEn">ChequeDetails Entity is an Input.ProceddID is Property.</param>
        /// <returns>Returns a Boolean</returns>
        public bool Delete(ChequeDetailsEn argEn)
        {
            bool lbRes = false;
            string sqlCmd = "DELETE FROM SAS_ChequeDetails WHERE processID = @ProcessID ";
            
            try
            {
                if(!FormHelp.IsBlank(sqlCmd))
                {
                    DbCommand cmd = _DatabaseFactory.GetDbCommand(Helper.GetDataBaseType, sqlCmd, DataBaseConnectionString);
                    _DatabaseFactory.AddInParameter(ref cmd, "@ProcessID", DbType.String, argEn.ProcessId);
                    _DbParameterCollection = cmd.Parameters;

                    int liRowAffected = _DatabaseFactory.ExecuteNonQuery(Helper.GetDataBaseType, cmd,
                                DataBaseConnectionString, sqlCmd, _DbParameterCollection);

                    if (liRowAffected > -1)
                        lbRes = true;
                    else
                        throw new Exception("Deletion Failed! No Row has been deleted...");
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
        /// Method to Load ChequeDetails Entity
/// </summary>
        /// <param name="argReader">Reader Object is an Input.</param>
        /// <returns>Returns Cheques Entity Object.</returns>
        private ChequeDetailsEn LoadObject(IDataReader argReader)
        {
            ChequeDetailsEn loItem = new ChequeDetailsEn();
            loItem.ProcessId = GetValue<string>(argReader, "ProcessId");
            loItem.ChequeStartNo = GetValue<string>(argReader, "ChequeStartNo");
            loItem.ChequeEndNo = GetValue<string>(argReader, "ChequeEndNo");

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


