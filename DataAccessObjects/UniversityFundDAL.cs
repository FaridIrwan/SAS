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
    /// Class to handle all the UniversityFund Methods.
    /// </summary>
    public class UniversityFundDAL
    {
        #region Global Declarations 

        private DbParameterCollection _DbParameterCollection = null;

        private MaxModule.DatabaseProvider _DatabaseFactory =
            new MaxModule.DatabaseProvider();

        private string DataBaseConnectionString = Helper.
           GetConnectionString();

        #endregion

        #region Get List 

        /// <summary>
        /// Method to Get List of UniversityFund
        /// </summary>
        /// <param name="argEn">UniversityFund Entity as an Input.</param>
        /// <returns>Returns List of UniversityFund</returns>
        public List<UniversityFundEn> GetList(UniversityFundEn argEn)
        {
            //Create Instances
            List<UniversityFundEn> UniversityList = new List<UniversityFundEn>();
            
            try
            {
                //build sql statement
                string SqlStatement = "SELECT * FROM SAS_UniversityFund";

                //Get User Login Details - Start
                IDataReader _IDataReader = _DatabaseFactory.ExecuteReader(Helper.
                    GetDataBaseType, DataBaseConnectionString, SqlStatement).CreateDataReader();
                //Get User Login Details - Stop

                //if details available - Start
                if (_IDataReader != null)
                {
                    while (_IDataReader.Read())
                    {
                        UniversityFundEn _UniversityFundEn = LoadObject(_IDataReader);
                        UniversityList.Add(_UniversityFundEn);
                    }

                    _IDataReader.Close();
                    _IDataReader.Dispose();

                    return UniversityList;
                }
                //if details available - Stop

                return null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region Get Fund List 

        /// <summary>
        /// Method to Get List of Active or Inactive UniversityFunds
        /// </summary>
        /// <param name="argEn">UniversityFund Entity as an Input.UniversityFundCode,Description,GLCode and Status as Input Properties.</param>
        /// <returns>Returns List of UniversityFund</returns>
        public List<UniversityFundEn> GetUniversityFundList(UniversityFundEn argEn)
        {
            //Create Instances
            List<UniversityFundEn> UniversityList = new List<UniversityFundEn>();
                       
            try
            {
                //Set Values - Start
                argEn.UniversityFundCode = argEn.UniversityFundCode.Replace("*", "%");
                argEn.Description = argEn.Description.Replace("*", "%");
                argEn.GLCode = argEn.GLCode.Replace("*", "%");
                //Set Values - Stop

                //build sql statement - Start
                string SqlStatement = "select * from SAS_UniversityFund where SAUF_Code <> '0'";
                if (argEn.UniversityFundCode.Length != 0) SqlStatement = SqlStatement + " and SAUF_Code like '" + argEn.UniversityFundCode + "'";
                if (argEn.Description.Length != 0) SqlStatement = SqlStatement + " and SAUF_Desc like '" + argEn.Description + "'";
                if (argEn.GLCode.Length != 0) SqlStatement = SqlStatement + " and SAUF_GLCode like '" + argEn.GLCode + "'";
                if (argEn.Status == true) SqlStatement = SqlStatement + " and SAUF_Status = true";
                if (argEn.Status == false) SqlStatement = SqlStatement + " and SAUF_Status =false";
                SqlStatement = SqlStatement + " order by SAUF_Code";
                //build sql statement - Stop

                //Get User Login Details - Start
                IDataReader _IDataReader = _DatabaseFactory.ExecuteReader(Helper.
                    GetDataBaseType, DataBaseConnectionString, SqlStatement).CreateDataReader();
                //Get User Login Details - Stop

                //if details available - Start
                if (_IDataReader != null)
                {
                    while (_IDataReader.Read())
                    {
                        UniversityFundEn _UniversityFundEn = LoadObject(_IDataReader);
                        UniversityList.Add(_UniversityFundEn);
                    }

                    _IDataReader.Close();
                    _IDataReader.Dispose();

                    return UniversityList;
                }
                //if details available - Stop

                return null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region Get Item 

        /// <summary>
        /// Method to Get UniversityFund Entity
        /// </summary>
        /// <param name="argEn">UniversityFund Entity is an Input.UniversityFundCode as Input Property.</param>
        /// <returns>Returns UniversityFund Entity</returns>
        public UniversityFundEn GetItem(UniversityFundEn argEn)
        {
            //create instances
            UniversityFundEn _UniversityFundEn = new UniversityFundEn();

            try
            {
                //Build Sqlstatement - Start
                string SqlStatement = "SELECT * FROM SAS_UniversityFund WHERE SAUF_Code = " 
                    + clsGeneric.AddQuotes(argEn.UniversityFundCode);
                //Build Sqlstatement - Stop

                //Get User Login Details - Start
                IDataReader _IDataReader = _DatabaseFactory.ExecuteReader(Helper.
                    GetDataBaseType, DataBaseConnectionString, SqlStatement).CreateDataReader();
                //Get User Login Details - Stop

                //if details available - Start
                if (_IDataReader != null)
                {
                    if (_IDataReader.Read())
                    {
                        _UniversityFundEn = LoadObject(_IDataReader);
                    }

                    _IDataReader.Close();
                    _IDataReader.Dispose();

                    return _UniversityFundEn;
                }
                //if details available - Stop

                return null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region Insert 

        /// <summary>
        /// Method to Insert UniversityFund 
        /// </summary>
        /// <param name="argEn">UniversityFund Entity is an Input.</param>
        /// <returns>Returns Boolean</returns>
        public bool Insert(UniversityFundEn argEn)
        {
            //variable declarations - Start
            string Boolvalue = "false"; bool Result = false;
            string SqlStatement = null; int RecordsSaved = 0;
            //variable declarations - Stop

            try
            {
                //Build Sql Statement - Start
                SqlStatement = "SELECT SAUF_Code FROM SAS_UniversityFund WHERE ";
                SqlStatement += " SAUF_Code = " + clsGeneric.AddQuotes(argEn.UniversityFundCode);
                SqlStatement += " OR SAUF_Desc = " + clsGeneric.AddQuotes(argEn.Description);
                //Build Sql Statement - Stop

                //if no duplicate records - Start
                if (_DatabaseFactory.ExecuteReader(Helper.GetDataBaseType,
                    DataBaseConnectionString, SqlStatement).Rows.Count == 0)
                {
                    if (argEn.Status)
                    { Boolvalue = "true"; }

                    //Build Sql Columns - Start
                    SqlStatement = "INSERT INTO SAS_UniversityFund(SAUF_Code,SAUF_Desc,SAUF_GLCode,SABR_Code,";
                    SqlStatement += "SAUF_Status,SASUF_UpdatedBy,SASUF_UpdatedDtTm) VALUES (";
                    SqlStatement += clsGeneric.AddQuotes(argEn.UniversityFundCode);
                    SqlStatement += clsGeneric.AddComma() + clsGeneric.AddQuotes(argEn.Description);
                    SqlStatement += clsGeneric.AddComma() + clsGeneric.AddQuotes(argEn.GLCode);
                    SqlStatement += clsGeneric.AddComma() + clsGeneric.AddQuotes(argEn.Code.ToString());
                    SqlStatement += clsGeneric.AddComma() + clsGeneric.AddQuotes(Boolvalue);
                    SqlStatement += clsGeneric.AddComma() + clsGeneric.AddQuotes(argEn.UpdatedBy);
                    SqlStatement += clsGeneric.AddComma() + clsGeneric.AddQuotes(argEn.UpdatedDtTm) + ");" ;
                    //Build Sql Columns - Stop
                    
                    //Save Details to Database - Start
                    RecordsSaved = _DatabaseFactory.ExecuteSqlStatement(Helper.
                        GetDataBaseType, DataBaseConnectionString, SqlStatement);
                    //Save Details to Database - Stop

                    //if records saved successfully - Start
                    if (RecordsSaved > -1)
                        Result = true;
                    else
                        throw new Exception("Insertion Failed! No Row has been updated...");
                    //if records saved successfully - Stop
                }
                //if no duplicate records - Stop

                return Result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region Update 

        /// <summary>
        /// Method to Update UniversityFund 
        /// </summary>
        /// <param name="argEn">UniversityFund Entity is an Input.</param>
        /// <returns>Returns Boolean</returns>
        public bool Update(UniversityFundEn argEn)
        {
            //variable declarations - Start
            string Boolvalue = "false"; bool Result = false;
            string SqlStatement = null; int RecordsSaved = 0;
            //variable declarations - Stop

            try
            {
                //Build Sql Statement - Start
                SqlStatement = "SELECT SAUF_Code FROM SAS_UniversityFund WHERE ";
                SqlStatement += " SAUF_Code != " +  clsGeneric.AddQuotes(argEn.UniversityFundCode);
                SqlStatement += " AND SAUF_Desc = " + clsGeneric.AddQuotes(argEn.Description);
                //Build Sql Statement - Stop

                //if no duplicate records - Start
                if (_DatabaseFactory.ExecuteReader(Helper.GetDataBaseType,
                    DataBaseConnectionString, SqlStatement).Rows.Count == 0)
                {
                    if (argEn.Status)
                    { Boolvalue = "true"; }

                    //Build Update Statement - Start
                    SqlStatement = "UPDATE SAS_UniversityFund SET ";
                    SqlStatement += "SAUF_Code = " + clsGeneric.AddQuotes(argEn.UniversityFundCode);
                    SqlStatement += clsGeneric.AddComma() + "SAUF_Desc = " + clsGeneric.AddQuotes(argEn.Description);
                    SqlStatement += clsGeneric.AddComma() + "SAUF_GLCode = " + clsGeneric.AddQuotes(argEn.GLCode);
                    SqlStatement += clsGeneric.AddComma() + "SABR_Code = " + clsGeneric.AddQuotes(argEn.Code.ToString());
                    SqlStatement += clsGeneric.AddComma() + "SAUF_Status = " + clsGeneric.AddQuotes(Boolvalue);
                    SqlStatement += clsGeneric.AddComma() + "SASUF_UpdatedBy = " + clsGeneric.AddQuotes(argEn.UpdatedBy);
                    SqlStatement += clsGeneric.AddComma() + "SASUF_UpdatedDtTm = " + clsGeneric.AddQuotes(argEn.UpdatedDtTm);
                    SqlStatement += " WHERE SAUF_Code = " + clsGeneric.AddQuotes(argEn.UniversityFundCode);
                    //Build Update Statement - Stop

                    //Save Details to Database
                    RecordsSaved = _DatabaseFactory.ExecuteSqlStatement(Helper.
                        GetDataBaseType, DataBaseConnectionString, SqlStatement);

                    //if records saved successfully - Start
                    if (RecordsSaved > -1)
                        Result = true;
                    else
                        throw new Exception("Update Failed! No Row has been updated...");
                    //if records saved successfully - Stop
                }
                //if no duplicate records - Stop

                return Result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region Delete 

        /// <summary>
        /// Method to Delete UniversityFund 
        /// </summary>
        /// <param name="argEn">UniversityFund Entity is an Input.UniversityFundCode as Input Property.</param>
        /// <returns>Returns Boolean</returns>
        public bool Delete(UniversityFundEn argEn)
        {
            //variable declarations
            bool Result = false; string SqlStatement = null; int RecordsSaved = 0;

            try
            {
                //Build Sql Statement - Start
                SqlStatement = "DELETE FROM SAS_UniversityFund WHERE SAUF_Code = ";
                SqlStatement += clsGeneric.AddQuotes(argEn.UniversityFundCode);
                //Build Sql Statement - Stop

                //Save Details to Database
                RecordsSaved = _DatabaseFactory.ExecuteSqlStatement(Helper.
                        GetDataBaseType, DataBaseConnectionString, SqlStatement);

                //if records saved successfully - Start
                if (RecordsSaved > -1)
                    Result = true;
                else
                    throw new Exception("Delete Failed! No Row has been deleted...");
                //if records saved successfully - Stop

                return Result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region Load Object 

        /// <summary>
        /// Method to Load UniversityFund Entity 
        /// </summary>
        /// <param name="argReader">IDataReader Object is an Input.</param>
        /// <returns>Returns UniversityFund Entity</returns>
        private UniversityFundEn LoadObject(IDataReader argReader)
        {
            UniversityFundEn loItem = new UniversityFundEn();
            loItem.UniversityFundCode = GetValue<string>(argReader, "SAUF_Code");
            loItem.Description = GetValue<string>(argReader, "SAUF_Desc");
            loItem.GLCode = GetValue<string>(argReader, "SAUF_GLCode");
            loItem.Code = GetValue<int>(argReader, "SABR_Code");
            loItem.Status = GetValue<bool>(argReader, "SAUF_Status");
            loItem.UpdatedBy = GetValue<string>(argReader, "SASUF_UpdatedBy");
            loItem.UpdatedDtTm = GetValue<string>(argReader, "SASUF_UpdatedDtTm");

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

