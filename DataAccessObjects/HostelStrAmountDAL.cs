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
    /// Class to handle all the HostelStructureAmount Methods.
    /// </summary>
    public class HostelStrAmountDAL
    {
        #region Global Declarations 

        private DbParameterCollection _DbParameterCollection = null;

        private MaxModule.DatabaseProvider _DatabaseFactory =
            new MaxModule.DatabaseProvider();

        private string DataBaseConnectionString = Helper.
           GetConnectionString();

        #endregion

        public HostelStrAmountDAL()
        {
        }

        #region GetList 

        /// <summary>
        /// Method to Get List of HostelStructureAmount
        /// </summary>
        /// <param name="argEn">HostelStrAmount Entity is an Input.</param>
        /// <returns>Returns List of HostelStrAmounts</returns>
        public List<HostelStrAmountEn> GetList(HostelStrAmountEn argEn)
        {
            List<HostelStrAmountEn> loEnList = new List<HostelStrAmountEn>();
            string sqlCmd = "select * from SAS_HostelStrAmount";
            
            try
            {
                if (!FormHelp.IsBlank(sqlCmd))
                {
                    using (IDataReader loReader = _DatabaseFactory.ExecuteReader(Helper.GetDataBaseType,
                        DataBaseConnectionString, sqlCmd).CreateDataReader())
                    {
                        while (loReader.Read())
                        {
                            HostelStrAmountEn loItem = LoadObject(loReader);
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

        #region GetDescList 

        /// <summary>
        /// Method to Get List of HostelStructureAmount
        /// </summary>
        /// <param name="argEn">HostelStrAmount Entity is an Input.HSCode as Input Property</param>
        /// <returns>Returns List of HostelStrAmounts</returns>
        public List<HostelStrAmountEn> GetDescList(HostelStrAmountEn argEn)
        {
            List<HostelStrAmountEn> loEnList = new List<HostelStrAmountEn>();
            string sqlCmd = "SELECT SAS_HostelStrAmount.SASC_Code,SAS_HostelStrAmount.SAFT_Code, SAS_HostelStrAmount.SAHS_Code,SAS_FeeTypes.SAFT_Desc, SAS_StudentCategory.SASC_Desc," +
                            " SAS_StudentCategory.SASC_Code AS Expr1,SAS_HostelStrAmount.SAHA_Amount FROM SAS_StudentCategory INNER JOIN " +
                            "SAS_HostelStrAmount ON SAS_StudentCategory.SASC_Code = SAS_HostelStrAmount.SASC_Code INNER JOIN " +
                             "SAS_FeeTypes ON SAS_HostelStrAmount.SAFT_Code = SAS_FeeTypes.SAFT_Code WHERE SAS_HostelStrAmount.SAHS_Code = @SAHS_Code and SAS_HostelStrAmount.SAFT_Code = @SAFT_Code";

            try
            {
                if (!FormHelp.IsBlank(sqlCmd))
                {
                    DbCommand cmd = _DatabaseFactory.GetDbCommand(Helper.GetDataBaseType, sqlCmd, DataBaseConnectionString);
                    _DatabaseFactory.AddInParameter(ref cmd, "@SAHS_Code", DbType.String, argEn.HSCode);
                    _DatabaseFactory.AddInParameter(ref cmd, "@SAFT_Code", DbType.String, argEn.FTCode);
                    _DbParameterCollection = cmd.Parameters;

                    using (IDataReader loReader = _DatabaseFactory.GetIDataReader(Helper.GetDataBaseType, cmd,
                        DataBaseConnectionString, sqlCmd, _DbParameterCollection).CreateDataReader())
                    {
                        while (loReader.Read())
                        {
                            HostelStrAmountEn loItem = LoadObject(loReader);
                            loItem.Description = GetValue<string>(loReader, "SAFT_Desc");
                            loItem.SCDesc = GetValue<string>(loReader, "SASC_Desc");
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
        /// Method to Get HostelStrAmount Entity
        /// </summary>
        /// <param name="argEn">HostelStrAmount Entity is an Input.HSCode as Input Property.</param>
        /// <returns>Returns HostelStrAmount Entity</returns>
        public HostelStrAmountEn GetItem(HostelStrAmountEn argEn)
        {
            HostelStrAmountEn loItem = new HostelStrAmountEn();
            string sqlCmd = "Select * FROM SAS_HostelStrAmount WHERE SAHS_Code = @SAHS_Code";
            
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
        /// Method to Insert HostelStrAmount 
        /// </summary>
        /// <param name="argEn">HostelStrAmount Entity is an Input.</param>
        /// <returns>Returns Boolean</returns>
        public bool Insert(HostelStrAmountEn argEn)
        {
            bool lbRes = false;
            string sqlCmd;
            try
            {
                sqlCmd = "INSERT INTO SAS_HostelStrAmount(SAHS_Code,SAFT_Code,SASC_Code,SAHA_Amount,safa_gstamount) VALUES (@SAHS_Code,@SAFT_Code,@SASC_Code,@SAHA_Amount, @safa_gstamount) ";

                if (!FormHelp.IsBlank(sqlCmd))
                {
                    DbCommand cmd = _DatabaseFactory.GetDbCommand(Helper.GetDataBaseType, sqlCmd, DataBaseConnectionString);
                    _DatabaseFactory.AddInParameter(ref cmd, "@SAHS_Code", DbType.String, clsGeneric.NullToString(argEn.HSCode));
                    _DatabaseFactory.AddInParameter(ref cmd, "@SAFT_Code", DbType.String, clsGeneric.NullToString(argEn.FTCode));
                    _DatabaseFactory.AddInParameter(ref cmd, "@SASC_Code", DbType.String, clsGeneric.NullToString(argEn.SCCode));
                    _DatabaseFactory.AddInParameter(ref cmd, "@SAHA_Amount", DbType.Double, clsGeneric.NullToDecimal(argEn.HAAmount));
                    _DatabaseFactory.AddInParameter(ref cmd, "@safa_gstamount", DbType.Double, clsGeneric.NullToDecimal(argEn.GstAmount));
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
        /// Method to Update HostelStrAmount 
        /// </summary>
        /// <param name="argEn">HostelStrAmount Entity is an Input.</param>
        /// <returns>Returns Boolean</returns>
        public bool Update(HostelStrAmountEn argEn)
        {
            bool lbRes = false;
            string sqlCmd;
            try
            {
                sqlCmd = "UPDATE SAS_HostelStrAmount SET SAHS_Code = @SAHS_Code, SAFT_Code = @SAFT_Code, SASC_Code = @SASC_Code, SAHA_Amount = @SAHA_Amount WHERE SAHS_Code = @SAHS_Code";

                if (!FormHelp.IsBlank(sqlCmd))
                {
                    DbCommand cmd = _DatabaseFactory.GetDbCommand(Helper.GetDataBaseType, sqlCmd, DataBaseConnectionString);
                    _DatabaseFactory.AddInParameter(ref cmd, "@SAHS_Code", DbType.String, argEn.HSCode);
                    _DatabaseFactory.AddInParameter(ref cmd, "@SAFT_Code", DbType.String, argEn.FTCode);
                    _DatabaseFactory.AddInParameter(ref cmd, "@SASC_Code", DbType.String, argEn.SCCode);
                    _DatabaseFactory.AddInParameter(ref cmd, "@SAHA_Amount", DbType.Double, argEn.HAAmount);
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

        #region  Delete 

        /// <s
        /// ummary>
        /// Method to Delete HostelStrAmount 
        /// </summary>
        /// <param name="argEn">HostelStrAmount Entity is an Input.</param>
        /// <returns>Returns Boolean</returns>
        public bool Delete(HostelStrAmountEn argEn)
        {
            bool lbRes = false;
            string sqlCmd = "DELETE FROM SAS_HostelStrAmount WHERE SAHS_Code = @SAHS_Code";

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
        /// Method to Load HostelStrAmount Entity 
        /// </summary>
        /// <param name="argReader">IDataReader Object is an Input.</param>
        /// <returns>Returns HostelStrAmount Entity</returns>
        private HostelStrAmountEn LoadObject(IDataReader argReader)
        {
            HostelStrAmountEn loItem = new HostelStrAmountEn();
            loItem.HSCode = GetValue<string>(argReader, "SAHS_Code");
            loItem.FTCode = GetValue<string>(argReader, "SAFT_Code");
            loItem.SCCode = GetValue<string>(argReader, "SASC_Code");
            loItem.HAAmount = GetValue<double>(argReader, "SAHA_Amount");

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
