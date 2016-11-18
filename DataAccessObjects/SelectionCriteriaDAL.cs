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
    /// Class to handle all the SelectionCriteria Methods.
    /// </summary>
    public class SelectionCriteriaDAL
    {
        #region Global Declarations 

        private DbParameterCollection _DbParameterCollection = null;

        private MaxModule.DatabaseProvider _DatabaseFactory =
            new MaxModule.DatabaseProvider();

        private string DataBaseConnectionString = Helper.
           GetConnectionString();

       #endregion

        public SelectionCriteriaDAL()
        {
        }

        #region GetList 

        /// <summary>
        /// Method to Get List of SelectionCriteria
        /// </summary>
        /// <param name="argEn">SelectionCriteria Entity is an Input.</param>
        /// <returns>Returns List of SelectionCriteria</returns>
        public SelectionCriteriaEn GetSCByBatchCode(SelectionCriteriaEn argEn)
        {

            SelectionCriteriaEn loItem = new SelectionCriteriaEn();

            string sqlCmd = "select * from SAS_Selection_Criteria where BatchCode = '" + argEn.BatchCode + "'";

            try
            {
                if (!FormHelp.IsBlank(sqlCmd))
                {
                    using (IDataReader loReader = _DatabaseFactory.ExecuteReader(Helper.GetDataBaseType,
                        DataBaseConnectionString, sqlCmd).CreateDataReader())
                    {
                        //if (loReader != null)
                        //{
                        //    loReader.Read();
                        //    loItem = LoadObject(loReader);

                        //}

                        if (loReader.Read())
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
   

        #region Insert 

        /// <summary>
        /// Method to Insert SelectionCriteria 
        /// </summary>
        /// <param name="argEn">SelectionCriteria Entity is an Input.</param>
        /// <returns>Returns Boolean</returns>
        public bool Insert(SelectionCriteriaEn argEn)
        {
            bool lbRes = false;
            string sqlCmd;
            try
            {
                sqlCmd = "INSERT INTO SAS_Selection_Criteria(BatchCode,safc_code ,sapg_code ,sasr_code ,sako_code, sasc_code, sem )" +
                "VALUES (@BatchCode,@safc_code,@sapg_code,@sasr_code,@sako_code, @sasc_code, @sem) ";

                if (!FormHelp.IsBlank(sqlCmd))
                {
                    DbCommand cmd = _DatabaseFactory.GetDbCommand(Helper.GetDataBaseType, sqlCmd, DataBaseConnectionString);
                    _DatabaseFactory.AddInParameter(ref cmd, "@BatchCode", DbType.String, argEn.BatchCode);
                    _DatabaseFactory.AddInParameter(ref cmd, "@safc_code", DbType.String, argEn.SAFC_Code);
                    _DatabaseFactory.AddInParameter(ref cmd, "@sako_code", DbType.String, argEn.SAKO_Code);
                    _DatabaseFactory.AddInParameter(ref cmd, "@sapg_code", DbType.String, argEn.SAPG_Code);
                    _DatabaseFactory.AddInParameter(ref cmd, "@sasr_code", DbType.String, argEn.SASR_Code);
                    _DatabaseFactory.AddInParameter(ref cmd, "@sasc_code", DbType.String, argEn.SASC_Code);
                    _DatabaseFactory.AddInParameter(ref cmd, "@sem", DbType.String, argEn.Sem);

                    _DbParameterCollection = cmd.Parameters;

                    int liRowAffected = _DatabaseFactory.ExecuteNonQuery(Helper.GetDataBaseType, cmd,
                        DataBaseConnectionString, sqlCmd, _DbParameterCollection);
                                    
                    if (liRowAffected > -1)
                        lbRes = true;                    
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
        /// Method to Update SelectionCriteria 
        /// </summary>
        /// <param name="argEn">SelectionCriteria Entity is an Input.</param>
        /// <returns>Returns Boolean</returns>
        public bool Update(SelectionCriteriaEn argEn)
        {
            bool lbRes = false;
            try
            {
                Delete(argEn);
                Insert(argEn);
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
        /// Method to Delete Faculty 
        /// </summary>
        /// <param name="argEn">SelectionCriteria Entity is an Input.</param>
        /// <returns>Returns Boolean</returns>
        public bool Delete(SelectionCriteriaEn argEn)
        {
            bool lbRes = false;
            string sqlCmd = "DELETE FROM SAS_Selection_Criteria WHERE BatchCode = @BatchCode";

            try
            {
                if (!FormHelp.IsBlank(sqlCmd))
                {
                    DbCommand cmd = _DatabaseFactory.GetDbCommand(Helper.GetDataBaseType, sqlCmd, DataBaseConnectionString);
                    _DatabaseFactory.AddInParameter(ref cmd, "@BatchCode", DbType.String, argEn.BatchCode);

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

        #region Load Object 

        /// <summary>
        /// Method to Load SelectionCriteria Entity 
        /// </summary>
        /// <param name="argReader">IDataReader Object is an Input.</param>
        /// <returns>Returns SelectionCriteria Entity</returns>
        private SelectionCriteriaEn LoadObject(IDataReader argReader)
        {
            SelectionCriteriaEn loItem = new SelectionCriteriaEn();
            loItem.SAFC_Code = GetValue<string>(argReader, "SAFC_Code");
            loItem.BatchCode = GetValue<string>(argReader, "BatchCode");
            loItem.SAKO_Code = GetValue<string>(argReader, "SAKO_Code");
            loItem.SAPG_Code = GetValue<string>(argReader, "SAPG_Code");
            loItem.SASR_Code = GetValue<string>(argReader, "SASR_Code");
            loItem.Sem = GetValue<string>(argReader, "Sem");
            loItem.SASC_Code = GetValue<string>(argReader, "SASC_Code");
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


