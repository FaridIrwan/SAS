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
    /// Class to handle all the SponsorFeeTypes Methods.
    /// </summary>
    public class SponsorFeeTypesDAL
    {
        #region Global Declarations 

        private DbParameterCollection _DbParameterCollection = null;

        private MaxModule.DatabaseProvider _DatabaseFactory =
            new MaxModule.DatabaseProvider();

        private string DataBaseConnectionString = Helper.
           GetConnectionString();

        #endregion

        public SponsorFeeTypesDAL()
        {
        }

        #region GetList 

        /// <summary>
        /// Method to Get List of SponsorFeeTypes
        /// </summary>
        /// <param name="argEn">SponsorFeeTypes Entity as an Input.</param>
        /// <returns>Returns List of SponsorFeeTypes</returns>
        public List<SponsorFeeTypesEn> GetList(SponsorFeeTypesEn argEn)
        {
            List<SponsorFeeTypesEn> loEnList = new List<SponsorFeeTypesEn>();
            string sqlCmd = "select * from SAS_SponsorFeeTypes";
            
            try
            {
                if (!FormHelp.IsBlank(sqlCmd))
                {
                    using (IDataReader loReader = _DatabaseFactory.ExecuteReader(Helper.GetDataBaseType,
                        DataBaseConnectionString, sqlCmd).CreateDataReader())
                    {
                        while (loReader.Read())
                        {
                            SponsorFeeTypesEn loItem = LoadObject(loReader);
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

        #region GetSPFeeTypeList 

        /// <summary>
        /// Method to Get List of Sponsor FeeTypes by SponsorCode
        /// </summary>
        /// <param name="argEn">SponsorFeeTypes Entity as an Input.SponsorCode as Input Property.</param>
        /// <returns>Returns List of SponsorFeeTypes</returns>
        public List<SponsorFeeTypesEn> GetSPFeeTypeList(SponsorFeeTypesEn argEn)
        {
            List<SponsorFeeTypesEn> loEnList = new List<SponsorFeeTypesEn>();
            string sqlCmd = "select SFT.SASR_Code,SFT.SAFT_Code,FT.SAFT_Desc from SAS_SponsorFeeTypes SFT INNER JOIN "+
            "SAS_FeeTypes FT ON SFT.SAFT_Code=FT.SAFT_Code where SFT.SASR_Code=@SASR_Code";
            
            try
            {
                if (!FormHelp.IsBlank(sqlCmd))
                {
                    DbCommand cmd = _DatabaseFactory.GetDbCommand(Helper.GetDataBaseType, sqlCmd, DataBaseConnectionString);
                    _DatabaseFactory.AddInParameter(ref cmd, "@SASR_Code", DbType.String, argEn.SponserCode);
                    _DbParameterCollection = cmd.Parameters;

                    using (IDataReader loReader = _DatabaseFactory.GetIDataReader(Helper.GetDataBaseType, cmd,
                        DataBaseConnectionString, sqlCmd, _DbParameterCollection).CreateDataReader()) 
                    {
                        while (loReader.Read())
                        {
                            SponsorFeeTypesEn loItem = LoadObject(loReader);
                            loItem.FeeTypeDesc = GetValue<string>(loReader, "SAFT_Desc");
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
        /// Method to Get SponsorFeeTypes Entity
        /// </summary>
        /// <param name="argEn">SponsorFeeTypes Entity is an Input.SASR_Code as Input Property.</param>
        /// <returns>Returns SponsorFeeTypes Entity</returns>
        public SponsorFeeTypesEn GetItem(SponsorFeeTypesEn argEn)
        {
            SponsorFeeTypesEn loItem = new SponsorFeeTypesEn();
            string sqlCmd = "Select * FROM SAS_SponsorFeeTypes WHERE SASR_Code = @SASR_Code";
            
            try
            {
                if (!FormHelp.IsBlank(sqlCmd))
                {
                    DbCommand cmdSel = _DatabaseFactory.GetDbCommand(Helper.GetDataBaseType, sqlCmd, DataBaseConnectionString);
                    _DatabaseFactory.AddInParameter(ref cmdSel, "@SASR_Code", DbType.String, argEn.SponserCode);
                    _DbParameterCollection = cmdSel.Parameters;

                    using (IDataReader loReader = _DatabaseFactory.GetIDataReader(Helper.GetDataBaseType, cmdSel,
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
        /// Method to Insert SponsorFeeTypes 
        /// </summary>
        /// <param name="argEn">SponsorFeeTypes Entity is an Input.</param>
        /// <returns>Returns Boolean</returns>
        public bool Insert(SponsorFeeTypesEn argEn)
        {
            bool lbRes = false;
            
            string sqlCmd = "Select count(*) as cnt From SAS_SponsorFeeTypes WHERE SASR_Code = @SASR_Code";
            try
            {
                if (!FormHelp.IsBlank(sqlCmd))
                {

                        sqlCmd = "INSERT INTO SAS_SponsorFeeTypes(SASR_Code,SAFT_Code) VALUES (@SASR_Code,@SAFT_Code) ";

                        if (!FormHelp.IsBlank(sqlCmd))
                        {
                            DbCommand cmd = _DatabaseFactory.GetDbCommand(Helper.GetDataBaseType, sqlCmd, DataBaseConnectionString);
                            _DatabaseFactory.AddInParameter(ref cmd, "@SASR_Code", DbType.String, argEn.SponserCode);
                            _DatabaseFactory.AddInParameter(ref cmd, "@SAFT_Code", DbType.String, argEn.FeeTypeCode);
                            _DbParameterCollection = cmd.Parameters;

                            int liRowAffected = _DatabaseFactory.ExecuteNonQuery(Helper.GetDataBaseType, cmd,
                                DataBaseConnectionString, sqlCmd, _DbParameterCollection); 
                            
                            if (liRowAffected > -1)
                                lbRes = true;
                            else
                                throw new Exception("Insertion Failed! No Row has been updated...");
                        }
                   // }
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
        /// Method to Insert SponsorFeeTypes 
        /// </summary>
        /// <param name="argEn">SponsorFeeTypes Entity is an Input.Sponsor Code and Feetype Code as Input Properties.</param>
        /// <returns>Returns Boolean</returns>
        public bool Update(SponsorFeeTypesEn argEn)
        {
            bool lbRes = false;
            try
            {
                string sqlCmd = "UPDATE SAS_SponsorFeeTypes SET SASR_Code = @SASR_Code, SAFT_Code = @SAFT_Code WHERE SASR_Code = @SASR_Code";
                if (!FormHelp.IsBlank(sqlCmd))
                {
                    DbCommand cmd = _DatabaseFactory.GetDbCommand(Helper.GetDataBaseType, sqlCmd, DataBaseConnectionString);
                    _DatabaseFactory.AddInParameter(ref cmd, "@SASR_Code", DbType.String, argEn.SponserCode);
                    _DatabaseFactory.AddInParameter(ref cmd, "@SAFT_Code", DbType.String, argEn.FeeTypeCode);
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
        /// Method to Delete SponsorFeeTypes 
        /// </summary>
        /// <param name="argEn">SponsorFeeTypes Entity is an Input.SASR_Code as Input Property.</param>
        /// <returns>Returns Boolean</returns>
        public bool Delete(SponsorFeeTypesEn argEn)
        {
            bool lbRes = false;
            string sqlCmd = "DELETE FROM SAS_SponsorFeeTypes WHERE SASR_Code = @SASR_Code";

            try
            {
                if (!FormHelp.IsBlank(sqlCmd))
                {
                    DbCommand cmd = _DatabaseFactory.GetDbCommand(Helper.GetDataBaseType, sqlCmd, DataBaseConnectionString);
                    _DatabaseFactory.AddInParameter(ref cmd, "@SASR_Code", DbType.String, argEn.SponserCode);
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
        /// Method to Load SponsorFeeTypes Entity 
        /// </summary>
        /// <param name="argReader">IDataReader Object is an Input.</param>
        /// <returns>Returns SponsorFeeTypes Entity</returns>
        private SponsorFeeTypesEn LoadObject(IDataReader argReader)
        {
            SponsorFeeTypesEn loItem = new SponsorFeeTypesEn();
            loItem.SponserCode = GetValue<string>(argReader, "SASR_Code");
            loItem.FeeTypeCode = GetValue<string>(argReader, "SAFT_Code");

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