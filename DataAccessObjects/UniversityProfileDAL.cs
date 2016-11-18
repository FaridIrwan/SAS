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
    /// Class to handle all the UniversityProfile Methods.
    /// </summary>
    public class UniversityProfileDAL
    {
        #region Global Declarations 

        private DbParameterCollection _DbParameterCollection = null;

        private MaxModule.DatabaseProvider _DatabaseFactory =
            new MaxModule.DatabaseProvider();

        private string DataBaseConnectionString = Helper.
           GetConnectionString();

        #endregion

        public UniversityProfileDAL()
        {
        }

        #region GetList 

        /// <summary>
        /// Method to Get List of UniversityProfile
        /// </summary>
        /// <param name="argEn">UniversityProfile Entity as an Input.</param>
        /// <returns>Returns List of UniversityProfile</returns>
        public List<UniversityProfileEn> GetList(UniversityProfileEn argEn)
        {
            List<UniversityProfileEn> loEnList = new List<UniversityProfileEn>();
            string sqlCmd = "select * from SAS_UniversityProfile";
            try
            {
                if (!FormHelp.IsBlank(sqlCmd))
                {
                    using (IDataReader loReader = _DatabaseFactory.ExecuteReader(Helper.GetDataBaseType,
                        DataBaseConnectionString, sqlCmd).CreateDataReader())
                    {
                        while (loReader.Read())
                        {
                            UniversityProfileEn loItem = LoadObject(loReader);
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

        #region GetUniversityProfileList 

        /// <summary>
        /// Method to Get List of All UniversityProfiles
        /// </summary>
        /// <param name="argEn">UniversityProfile Entity as an Input.UniversityProfileCode,Name and SName as Input Properties.</param>
        /// <returns>Returns List of UniversityProfile</returns>
        public List<UniversityProfileEn> GetUniversityProfileList(UniversityProfileEn argEn)
        {
            List<UniversityProfileEn> loEnList = new List<UniversityProfileEn>();
            argEn.UniversityProfileCode = argEn.UniversityProfileCode.Replace("*", "%");
            argEn.Name = argEn.Name.Replace("*", "%");
            argEn.SName = argEn.SName.Replace("*", "%");

            string sqlCmd = "select SAUP_Code, SAUP_Name, SAUP_SName, SAUP_Adress, SAUP_Adress1, SAUP_Adress2, SAUP_City," +
            "SAUP_State, SAUP_Country, SAUP_PostCode, SAUP_Phone, SAUP_Fax, SAUP_Email, SAUP_Website, SAUP_Logo, " +
                  "SABR_Code from SAS_UniversityProfile where SAUP_Code <> '' ";
            if (argEn.UniversityProfileCode.Length != 0) sqlCmd = sqlCmd + " and SAUP_Code like '" + argEn.UniversityProfileCode + "'";
            if (argEn.Name.Length != 0) sqlCmd = sqlCmd + " and SAUP_Name like '" + argEn.Name + "'";
            if (argEn.SName.Length != 0) sqlCmd = sqlCmd + " and SAUP_SName like '" + argEn.SName + "'";
            sqlCmd = sqlCmd + " order by SAUP_Code";
            try
            {
                if (!FormHelp.IsBlank(sqlCmd))
                {
                    using (IDataReader loReader = _DatabaseFactory.ExecuteReader(Helper.GetDataBaseType,
                        DataBaseConnectionString, sqlCmd).CreateDataReader())
                    {
                        while (loReader.Read())
                        {
                            UniversityProfileEn loItem = LoadObject(loReader);
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
        /// Method to Get UniversityProfile Entity
        /// </summary>
        /// <param name="argEn">UniversityProfile Entity is an Input.UniversityProfileCode as Input Property.</param>
        /// <returns>Returns UniversityProfile Entity</returns>
        public UniversityProfileEn GetItem(UniversityProfileEn argEn)
        {
            UniversityProfileEn loItem = new UniversityProfileEn();
            string sqlCmd = "Select * FROM SAS_UniversityProfile WHERE SAUP_Code = @SAUP_Code";
            try
            {
                if (!FormHelp.IsBlank(sqlCmd))
                {
                    DbCommand cmd = _DatabaseFactory.GetDbCommand(Helper.GetDataBaseType, sqlCmd, DataBaseConnectionString);
                    _DatabaseFactory.AddInParameter(ref cmd, "@SAUP_Code", DbType.String, argEn.UniversityProfileCode);
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
        /// Method to Insert UniversityProfile 
        /// </summary>
        /// <param name="argEn">UniversityProfile Entity is an Input.</param>
        /// <returns>Returns Boolean</returns>
        public bool Insert(UniversityProfileEn argEn)
        {
            bool lbRes = false;
            int iOut = 0;
            string sqlCmd = "Select count(*) as cnt From SAS_UniversityProfile WHERE SAUP_Code = @SAUP_Code or SAUP_Name = @SAUP_Name";
            try
            {
                if (!FormHelp.IsBlank(sqlCmd))
                {
                    DbCommand cmdSel = _DatabaseFactory.GetDbCommand(Helper.GetDataBaseType, sqlCmd, DataBaseConnectionString);
                    _DatabaseFactory.AddInParameter(ref cmdSel, "@SAUP_Code", DbType.String, argEn.UniversityProfileCode);
                    _DatabaseFactory.AddInParameter(ref cmdSel, "@SAUP_Name", DbType.String, argEn.Name);
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
                        sqlCmd = "INSERT INTO SAS_UniversityProfile(SAUP_Code,SAUP_Name,SAUP_SName,SAUP_Adress,SAUP_Adress1,SAUP_Adress2,SAUP_City,SAUP_State,SAUP_Country,SAUP_PostCode,SAUP_Phone,SAUP_Fax,SAUP_Email,SAUP_Website,SAUP_Logo,SABR_Code,SAUP_UpdatedUser,SAUP_UpdatedDtTm) VALUES (@SAUP_Code,@SAUP_Name,@SAUP_SName,@SAUP_Adress,@SAUP_Adress1,@SAUP_Adress2,@SAUP_City,@SAUP_State,@SAUP_Country,@SAUP_PostCode,@SAUP_Phone,@SAUP_Fax,@SAUP_Email,@SAUP_Website,@SAUP_Logo,@SABR_Code,@SAUP_UpdatedUser,@SAUP_UpdatedDtTm) ";
                        if (!FormHelp.IsBlank(sqlCmd))
                        {
                            DbCommand cmd = _DatabaseFactory.GetDbCommand(Helper.GetDataBaseType, sqlCmd, DataBaseConnectionString);
                            _DatabaseFactory.AddInParameter(ref cmd, "@SAUP_Code", DbType.String, argEn.UniversityProfileCode);
                            _DatabaseFactory.AddInParameter(ref cmd, "@SAUP_Name", DbType.String, argEn.Name);
                            _DatabaseFactory.AddInParameter(ref cmd, "@SAUP_SName", DbType.String, argEn.SName);
                            _DatabaseFactory.AddInParameter(ref cmd, "@SAUP_Adress", DbType.String, argEn.Adress);
                            _DatabaseFactory.AddInParameter(ref cmd, "@SAUP_Adress1", DbType.String, argEn.Adress1);
                            _DatabaseFactory.AddInParameter(ref cmd, "@SAUP_Adress2", DbType.String, argEn.Adress2);
                            _DatabaseFactory.AddInParameter(ref cmd, "@SAUP_City", DbType.String, argEn.City);
                            _DatabaseFactory.AddInParameter(ref cmd, "@SAUP_State", DbType.String, argEn.State);
                            _DatabaseFactory.AddInParameter(ref cmd, "@SAUP_Country", DbType.String, argEn.Country);
                            _DatabaseFactory.AddInParameter(ref cmd, "@SAUP_PostCode", DbType.String, argEn.PostCode);
                            _DatabaseFactory.AddInParameter(ref cmd, "@SAUP_Phone", DbType.String, argEn.Phone);
                            _DatabaseFactory.AddInParameter(ref cmd, "@SAUP_Fax", DbType.String, argEn.Fax);
                            _DatabaseFactory.AddInParameter(ref cmd, "@SAUP_Email", DbType.String, argEn.Email);
                            _DatabaseFactory.AddInParameter(ref cmd, "@SAUP_Website", DbType.String, argEn.Website);
                            //_DatabaseFactory.AddInParameter(ref cmd, "@SAUP_Logo", DbType.String, argEn.Logo);
                            _DatabaseFactory.AddInParameter(ref cmd, "@SAUP_Logo", DbType.String, string.Empty);
                            _DatabaseFactory.AddInParameter(ref cmd, "@SABR_Code", DbType.Int32, argEn.Code);
                            _DatabaseFactory.AddInParameter(ref cmd, "@SAUP_UpdatedUser", DbType.String, argEn.UpdatedUser);
                            _DatabaseFactory.AddInParameter(ref cmd, "@SAUP_UpdatedDtTm", DbType.String, argEn.UpdatedDtTm);
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

        /// <summary>
        /// Method to Update UniversityProfile 
        /// </summary>
        /// <param name="argEn">UniversityProfile Entity is an Input.</param>
        /// <returns>Returns Boolean</returns>
        public bool Update(UniversityProfileEn argEn)
        {
            bool lbRes = false;
            int iOut = 0;
            string sqlCmd = "Select count(*) as cnt From SAS_UniversityProfile WHERE SAUP_Code != @SAUP_Code and SAUP_Name = @SAUP_Name";
            try
            {
                if (!FormHelp.IsBlank(sqlCmd))
                {
                    DbCommand cmdSel = _DatabaseFactory.GetDbCommand(Helper.GetDataBaseType, sqlCmd, DataBaseConnectionString);
                    _DatabaseFactory.AddInParameter(ref cmdSel, "@SAUP_Code", DbType.String, argEn.UniversityProfileCode);
                    _DatabaseFactory.AddInParameter(ref cmdSel, "@SAUP_Name", DbType.String, argEn.Name);
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
                        sqlCmd = "UPDATE SAS_UniversityProfile SET SAUP_Code = @SAUP_Code, SAUP_Name = @SAUP_Name, SAUP_SName = @SAUP_SName, SAUP_Adress = @SAUP_Adress, SAUP_Adress1 = @SAUP_Adress1, SAUP_Adress2 = @SAUP_Adress2, SAUP_City = @SAUP_City, SAUP_State = @SAUP_State, SAUP_Country = @SAUP_Country, SAUP_PostCode = @SAUP_PostCode, SAUP_Phone = @SAUP_Phone, SAUP_Fax = @SAUP_Fax, SAUP_Email = @SAUP_Email, SAUP_Website = @SAUP_Website, SAUP_Logo = @SAUP_Logo, SABR_Code = @SABR_Code, SAUP_UpdatedUser = @SAUP_UpdatedUser, SAUP_UpdatedDtTm = @SAUP_UpdatedDtTm WHERE SAUP_Code = @SAUP_Code";
                        if (!FormHelp.IsBlank(sqlCmd))
                        {
                            DbCommand cmd = _DatabaseFactory.GetDbCommand(Helper.GetDataBaseType, sqlCmd, DataBaseConnectionString);
                            _DatabaseFactory.AddInParameter(ref cmd, "@SAUP_Code", DbType.String, argEn.UniversityProfileCode);
                            _DatabaseFactory.AddInParameter(ref cmd, "@SAUP_Name", DbType.String, argEn.Name);
                            _DatabaseFactory.AddInParameter(ref cmd, "@SAUP_SName", DbType.String, argEn.SName);
                            _DatabaseFactory.AddInParameter(ref cmd, "@SAUP_Adress", DbType.String, argEn.Adress);
                            _DatabaseFactory.AddInParameter(ref cmd, "@SAUP_Adress1", DbType.String, argEn.Adress1);
                            _DatabaseFactory.AddInParameter(ref cmd, "@SAUP_Adress2", DbType.String, argEn.Adress2);
                            _DatabaseFactory.AddInParameter(ref cmd, "@SAUP_City", DbType.String, argEn.City);
                            _DatabaseFactory.AddInParameter(ref cmd, "@SAUP_State", DbType.String, argEn.State);
                            _DatabaseFactory.AddInParameter(ref cmd, "@SAUP_Country", DbType.String, argEn.Country);
                            _DatabaseFactory.AddInParameter(ref cmd, "@SAUP_PostCode", DbType.String, argEn.PostCode);
                            _DatabaseFactory.AddInParameter(ref cmd, "@SAUP_Phone", DbType.String, argEn.Phone);
                            _DatabaseFactory.AddInParameter(ref cmd, "@SAUP_Fax", DbType.String, argEn.Fax);
                            _DatabaseFactory.AddInParameter(ref cmd, "@SAUP_Email", DbType.String, argEn.Email);
                            _DatabaseFactory.AddInParameter(ref cmd, "@SAUP_Website", DbType.String, argEn.Website);
                            //_DatabaseFactory.AddInParameter(ref cmd, "@SAUP_Logo", DbType.String, argEn.Logo);
                            _DatabaseFactory.AddInParameter(ref cmd, "@SAUP_Logo", DbType.String, string.Empty);
                            _DatabaseFactory.AddInParameter(ref cmd, "@SABR_Code", DbType.Int32, argEn.Code);
                            _DatabaseFactory.AddInParameter(ref cmd, "@SAUP_UpdatedUser", DbType.String, argEn.UpdatedUser);
                            _DatabaseFactory.AddInParameter(ref cmd, "@SAUP_UpdatedDtTm", DbType.String, argEn.UpdatedDtTm);
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
        /// Method to Delete UniversityProfile 
        /// </summary>
        /// <param name="argEn">UniversityProfile Entity is an Input.UniversityProfileCode as Input Propoerty.</param>
        /// <returns>Returns Boolean</returns>
        public bool Delete(UniversityProfileEn argEn)
        {
            bool lbRes = false;
            string sqlCmd = "DELETE FROM SAS_UniversityProfile WHERE SAUP_Code = @SAUP_Code";
           try
            {
                if (!FormHelp.IsBlank(sqlCmd))
                {
                    DbCommand cmd = _DatabaseFactory.GetDbCommand(Helper.GetDataBaseType, sqlCmd, DataBaseConnectionString);
                    _DatabaseFactory.AddInParameter(ref cmd, "@SAUP_Code", DbType.String, argEn.UniversityProfileCode);
                    _DbParameterCollection = cmd.Parameters;

                    int liRowAffected = _DatabaseFactory.ExecuteNonQuery(Helper.GetDataBaseType, cmd,
                        DataBaseConnectionString, sqlCmd, _DbParameterCollection);
                    
                    if (liRowAffected > -1)
                        lbRes = true;
                    else
                        throw new Exception("Delete Failed! No Row has been deleted...");
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
        /// Method to Load UniversityProfile Entity 
        /// </summary>
        /// <param name="argReader">IDataReader Object is an Input.</param>
        /// <returns>Returns UniversityProfile Entity</returns>
        private UniversityProfileEn LoadObject(IDataReader argReader)
        {
            UniversityProfileEn loItem = new UniversityProfileEn();
            loItem.UniversityProfileCode = GetValue<string>(argReader, "SAUP_Code");
            loItem.Name = GetValue<string>(argReader, "SAUP_Name");
            loItem.SName = GetValue<string>(argReader, "SAUP_SName");
            loItem.Adress = GetValue<string>(argReader, "SAUP_Adress");
            loItem.Adress1 = GetValue<string>(argReader, "SAUP_Adress1");
            loItem.Adress2 = GetValue<string>(argReader, "SAUP_Adress2");
            loItem.City = GetValue<string>(argReader, "SAUP_City");
            loItem.State = GetValue<string>(argReader, "SAUP_State");
            loItem.Country = GetValue<string>(argReader, "SAUP_Country");
            loItem.PostCode = GetValue<string>(argReader, "SAUP_PostCode");
            loItem.Phone = GetValue<string>(argReader, "SAUP_Phone");
            loItem.Fax = GetValue<string>(argReader, "SAUP_Fax");
            loItem.Email = GetValue<string>(argReader, "SAUP_Email");
            loItem.Website = GetValue<string>(argReader, "SAUP_Website");
            loItem.Logo = GetValue<string>(argReader, "SAUP_Logo");
            loItem.Code = GetValue<int>(argReader, "SABR_Code");

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

