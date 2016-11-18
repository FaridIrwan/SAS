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
    /// Class to handle all the StudentSponsor FeeTypes Methods.
    /// </summary>
    public class StuSponFeeTypesDAL
    {
        #region Global Declarations 

        private DbParameterCollection _DbParameterCollection = null;

        private MaxModule.DatabaseProvider _DatabaseFactory =
            new MaxModule.DatabaseProvider();

        private string DataBaseConnectionString = Helper.
           GetConnectionString();

        #endregion

        public StuSponFeeTypesDAL()
        {
        }

        #region GetList 

        /// <summary>
        /// Method to Get StudentSponsor FeeTypes 
        /// </summary>
        /// <param name="argEn">StuSponFeeTypes Entity as an Input.</param>
        /// <returns>Returns StuSponFeeTypes Entity</returns>
        public List<StuSponFeeTypesEn> GetList(StuSponFeeTypesEn argEn)
        {
            List<StuSponFeeTypesEn> loEnList = new List<StuSponFeeTypesEn>();
            string sqlCmd = "select * from SAS_StuSponFeeTypes";
            try
            {
                if (!FormHelp.IsBlank(sqlCmd))
                {
                    using (IDataReader loReader = _DatabaseFactory.ExecuteReader(Helper.GetDataBaseType,
                        DataBaseConnectionString, sqlCmd).CreateDataReader())
                    {
                        while (loReader.Read())
                        {
                            StuSponFeeTypesEn loItem = LoadObject(loReader);
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

        #region GetStuSponFTList 

        /// <summary>
        /// Method to Get StudentSponsor FeeTypes 
        /// </summary>
        /// <param name="argEn">StuSponFeeTypes Entity as an Input.MatricNo as Input Property.</param>
        /// <returns>Returns StuSponFeeTypes Entity</returns>
        public List<StuSponFeeTypesEn> GetStuSponFTList(StuSponFeeTypesEn argEn)
        {
            List<StuSponFeeTypesEn> loEnList = new List<StuSponFeeTypesEn>();
            string sqlCmd = "SELECT SAS_StuSponFeeTypes.*, SAS_FeeTypes.SAFT_Desc , SAS_Student.SASI_MatricNo "+
                            "FROM SAS_FeeTypes INNER JOIN SAS_StuSponFeeTypes ON SAS_FeeTypes.SAFT_Code = SAS_StuSponFeeTypes.SAFT_Code INNER JOIN "+
                      "SAS_Student ON SAS_StuSponFeeTypes.SASI_MatricNo = SAS_Student.SASI_MatricNo where SAS_Student.SASI_MatricNo='" + argEn.MatricNo + "' and SAS_StuSponFeeTypes.SASR_Code = '"+ argEn.SASR_Code +"'";
            try
            {
                if (!FormHelp.IsBlank(sqlCmd))
                {
                    using (IDataReader loReader = _DatabaseFactory.ExecuteReader(Helper.GetDataBaseType,
                        DataBaseConnectionString, sqlCmd).CreateDataReader())
                    {
                        while (loReader.Read())
                        {
                            StuSponFeeTypesEn loItem = LoadObject(loReader);
                            loItem.FeeDesc = GetValue<string>(loReader, "SAFT_Desc");
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
        /// Method to Get StudentSponsor FeeTypes Entity
        /// </summary>
        /// <param name="argEn">StudentSponFeeTypes Entity is an Input.MatricNo as Input Property.</param>
        /// <returns>Returns StudentSponFeeTypes Entity</returns>
        public StuSponFeeTypesEn GetItem(StuSponFeeTypesEn argEn)
        {
            StuSponFeeTypesEn loItem = new StuSponFeeTypesEn();
            string sqlCmd = "Select * FROM SAS_StuSponFeeTypes WHERE SASI_MatricNo = @SASI_MatricNo";
            
            try
            {
                if (!FormHelp.IsBlank(sqlCmd))
                {
                    DbCommand cmd = _DatabaseFactory.GetDbCommand(Helper.GetDataBaseType, sqlCmd, DataBaseConnectionString);
                    _DatabaseFactory.AddInParameter(ref cmd, "@SASI_MatricNo", DbType.String, argEn.MatricNo);
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
        /// Method to Insert StudentSponsor FeeTypes
        /// </summary>
        /// <param name="argEn">StudentSponsor Entity is an Input.</param>
        /// <returns>Returns Boolean</returns> 
        public bool Insert(StuSponFeeTypesEn argEn)
        {
            bool lbRes = false;
            string sqlCmd;
            try
            {

                sqlCmd = "INSERT INTO SAS_StuSponFeeTypes(SASI_MatricNo,SASR_Code,SAFT_Code) VALUES (@SASI_MatricNo,@SASR_Code,@SAFT_Code) ";
                if (!FormHelp.IsBlank(sqlCmd))
                {
                    DbCommand cmd = _DatabaseFactory.GetDbCommand(Helper.GetDataBaseType, sqlCmd, DataBaseConnectionString);
                    _DatabaseFactory.AddInParameter(ref cmd, "@SASI_MatricNo", DbType.String, argEn.MatricNo);
                    _DatabaseFactory.AddInParameter(ref cmd, "@SASR_Code", DbType.String, argEn.SASR_Code);
                    _DatabaseFactory.AddInParameter(ref cmd, "@SAFT_Code", DbType.String, argEn.SAFT_Code);
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
        /// Method to Insert StudentSponsor FeeTypes
        /// </summary>
        /// <param name="argEn">StudentSponsor Entity is an Input.</param>
        /// <returns>Returns Boolean</returns> 
        public bool Update(StuSponFeeTypesEn argEn)
        {
            bool lbRes = false;
            int iOut = 0;
            string sqlCmd = "Select count(*) as cnt From SAS_StuSponFeeTypes WHERE SASI_MatricNo = @SASI_MatricNo";
            try
            {
                if (!FormHelp.IsBlank(sqlCmd))
                {
                    DbCommand cmdSel = _DatabaseFactory.GetDbCommand(Helper.GetDataBaseType, sqlCmd, DataBaseConnectionString);
                    _DatabaseFactory.AddInParameter(ref cmdSel, "@SASI_MatricNo", DbType.String, argEn.MatricNo);
                    _DbParameterCollection = cmdSel.Parameters;

                    using (IDataReader dr = _DatabaseFactory.GetIDataReader(Helper.GetDataBaseType, cmdSel,
                        DataBaseConnectionString, sqlCmd, _DbParameterCollection).CreateDataReader()) 
                    {
                        if (dr.Read())
                            iOut = clsGeneric.NullToInteger(dr["cnt"]);
                        if (iOut > 0)
                            throw new Exception("Update Failed! No Record Exist!");
                    }
                    if (iOut == 0)
                    {
                        sqlCmd = "UPDATE SAS_StuSponFeeTypes SET SASI_MatricNo = @SASI_MatricNo, SASR_Code = @SASR_Code, SAFT_Code = @SAFT_Code WHERE SASI_MatricNo = @SASI_MatricNo";
                        if (!FormHelp.IsBlank(sqlCmd))
                        {
                            DbCommand cmd = _DatabaseFactory.GetDbCommand(Helper.GetDataBaseType, sqlCmd, DataBaseConnectionString);
                            _DatabaseFactory.AddInParameter(ref cmd, "@SASI_MatricNo", DbType.String, argEn.MatricNo);
                            _DatabaseFactory.AddInParameter(ref cmd, "@SASR_Code", DbType.String, argEn.SASR_Code);
                            _DatabaseFactory.AddInParameter(ref cmd, "@SAFT_Code", DbType.String, argEn.SAFT_Code);
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
        /// Method to Delete StudentSponsor FeeTypes 
        /// </summary>
        /// <param name="argEn">StuSponFeeTypes Entity is an Input.MatricNo as Input Property.</param>
        /// <returns>Returns Boolean</returns> 
        public bool Delete(StuSponFeeTypesEn argEn)
        {
            bool lbRes = false;
            string sqlCmd = "DELETE FROM SAS_StuSponFeeTypes WHERE SASI_MatricNo = @SASI_MatricNo";
            
            try
            {
                if (!FormHelp.IsBlank(sqlCmd))
                {
                    DbCommand cmd = _DatabaseFactory.GetDbCommand(Helper.GetDataBaseType, sqlCmd, DataBaseConnectionString);
                    _DatabaseFactory.AddInParameter(ref cmd, "@SASI_MatricNo", DbType.String, argEn.MatricNo);
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
        /// Method to Load StudentSponsor FeeTypes Entity 
        /// </summary>
        /// <param name="argReader">IDataReader Object is an Input.</param>
        /// <returns>Returns StuSponFeeTypes Entity</returns>
        private StuSponFeeTypesEn LoadObject(IDataReader argReader)
        {
            StuSponFeeTypesEn loItem = new StuSponFeeTypesEn();
            loItem.MatricNo = GetValue<string>(argReader, "SASI_MatricNo");
            loItem.SASR_Code = GetValue<string>(argReader, "SASR_Code");
            loItem.SAFT_Code = GetValue<string>(argReader, "SAFT_Code");

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
