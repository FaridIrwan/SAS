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
    /// Class to handle all the Kolej Methods.
    /// </summary>
    public class KolejDAL
    {
        #region Global Declarations 

        private DbParameterCollection _DbParameterCollection = null;

        private MaxModule.DatabaseProvider _DatabaseFactory =
            new MaxModule.DatabaseProvider();

        private string DataBaseConnectionString = Helper.
           GetConnectionString();

        #endregion

        public KolejDAL()
        {
        }

        #region Get List 

        /// <summary>
        /// Method to Get List of Kolej
        /// </summary>
        /// <param name="argEn">Kolej Entity is an Input.</param>
        /// <returns>Returns List of Kolej</returns>
        public List<KolejEn> GetList(KolejEn argEn)
        {
            //create instances
            List<KolejEn> KolejList = new List<KolejEn>();

            //variable declarations
            string SqlStatement = null;

            try
            {
                //build sql statement
                SqlStatement = "SELECT * FROM SAS_Kolej";

                //Get User Login Details - Start
                IDataReader _IDataReader = _DatabaseFactory.ExecuteReader(Helper.GetDataBaseType,
                        DataBaseConnectionString, SqlStatement).CreateDataReader();
                //Get User Login Details - Stop

                //if details available - Start
                if (_IDataReader != null)
                {
                    while (_IDataReader.Read())
                    {
                        KolejEn _KolejEn = LoadObject(_IDataReader);
                        KolejList.Add(_KolejEn);
                    }

                    _IDataReader.Close();
                    _IDataReader.Dispose();

                    return KolejList;
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

        #region Get List For Koko

        /// <summary>
        /// Method to Get List of Kolej
        /// </summary>
        /// <param name="argEn">Kolej Entity is an Input.</param>
        /// <returns>Returns List of Kolej</returns>
        public List<KolejEn> GetListKokorikulum(KolejEn argEn)
        {
            //create instances
            List<KolejEn> KolejList = new List<KolejEn>();

            //variable declarations
            string SqlStatement = null;

            try
            {
                //build sql statement
                SqlStatement = "SELECT * FROM sas_kokorikulum where Sako_code ='" + argEn.SAKO_Code + "'";

                //Get User Login Details - Start
                IDataReader _IDataReader = _DatabaseFactory.ExecuteReader(Helper.GetDataBaseType,
                        DataBaseConnectionString, SqlStatement).CreateDataReader();
                //Get User Login Details - Stop

                //if details available - Start
                if (_IDataReader != null)
                {
                    while (_IDataReader.Read())
                    {
                        KolejEn _KolejEn = LoadObject(_IDataReader);
                        KolejList.Add(_KolejEn);
                    }

                    _IDataReader.Close();
                    _IDataReader.Dispose();

                    return KolejList;
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

        #region Get List For Koko

        /// <summary>
        /// Method to Get List of Kolej
        /// </summary>
        /// <param name="argEn">Kolej Entity is an Input.</param>
        /// <returns>Returns List of Kolej</returns>
        public List<KolejEn> GetListKolej(KolejEn argEn)
        {
            //create instances
            List<KolejEn> KolejList = new List<KolejEn>();

            //variable declarations
            string SqlStatement = null;

            try
            {
                //build sql statement
                SqlStatement = "SELECT * FROM sas_kokorikulum where Sako_code ='" + argEn.SAKO_Code + "'";

                //Get User Login Details - Start
                IDataReader _IDataReader = _DatabaseFactory.ExecuteReader(Helper.GetDataBaseType,
                        DataBaseConnectionString, SqlStatement).CreateDataReader();
                //Get User Login Details - Stop

                //if details available - Start
                if (_IDataReader != null)
                {
                    while (_IDataReader.Read())
                    {
                        KolejEn _KolejEn = LoadObject(_IDataReader);
                        KolejList.Add(_KolejEn);
                    }

                    _IDataReader.Close();
                    _IDataReader.Dispose();

                    return KolejList;
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

        #region Get Kolej List 

        /// <summary>
        /// Method to Get List of all Kolej
        /// </summary>
        /// <param name="argEn">Kolej Entity is an Input.</param>
        /// <returns>Returns List of Kolej</returns>
        public List<KolejEn> GetKolejList(KolejEn argEn)
        {
            //create instances
            List<KolejEn> KolejList = new List<KolejEn>();

            //variable declarations
            string SqlStatement = null;

            try
            {
                //Replace wildchars - Start
                argEn.SAKO_Code = argEn.SAKO_Code.Replace("*", "");
                argEn.SAKO_Description = argEn.SAKO_Description.Replace("*", "");
                //Replace wildchars - Stop

                //Build Sqlstatement - Start
                SqlStatement = "select * from SAS_Kolej  WHERE SAKO_Code  <> '0'";
                SqlStatement += " and SAKO_Code like '%" + argEn.SAKO_Code + "%'";
                SqlStatement += " and SAKO_Description like '%";
                SqlStatement += argEn.SAKO_Description + "%' order by SAKO_Code";
                //Build Sqlstatement - Stop

                //Get User Login Details - Start
                IDataReader _IDataReader = _DatabaseFactory.ExecuteReader(Helper.GetDataBaseType,
                        DataBaseConnectionString, SqlStatement).CreateDataReader();
                //Get User Login Details - Stop

                //if details available - Start
                if (_IDataReader != null)
                {
                    while (_IDataReader.Read())
                    {
                        KolejEn _KolejEn = LoadObject(_IDataReader);
                        KolejList.Add(_KolejEn);
                    }

                    _IDataReader.Close();
                    _IDataReader.Dispose();

                    return KolejList;
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
        /// Method to Get Kolej Entity
        /// </summary>
        /// <param name="argEn">Kolej Entity is an Input.SAKO_Code as Input Property.</param>
        /// <returns>Returns Kolej Entity</returns>
        public KolejEn GetItem(KolejEn argEn)
        {
            //create instances
            KolejEn _KolejEn = new KolejEn();

            //vraiable declarations
            string SqlStatement = null;

            try
            {
                //Build Sql Statement - Start
                SqlStatement = "Select * FROM SAS_Kolej WHERE SAKO_Code = ";
                SqlStatement += clsGeneric.AddQuotes(argEn.SAKO_Code);
                //Build Sql Statement - Stop

                //Get User Login Details - Start
                IDataReader _IDataReader = _DatabaseFactory.ExecuteReader(Helper.GetDataBaseType,
                        DataBaseConnectionString, SqlStatement).CreateDataReader();
                //Get User Login Details - Stop

                //if details available - Start
                if (_IDataReader != null)
                {
                    while (_IDataReader.Read())
                    {
                        _KolejEn = LoadObject(_IDataReader);
                    }

                    _IDataReader.Close();
                    _IDataReader.Dispose();

                    return _KolejEn;
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
        /// Method to Insert Kolej 
        /// </summary>
        /// <param name="argEn">Kolej Entity is an Input.</param>
        /// <returns>Returns Boolean</returns>
        public bool Insert(KolejEn argEn)
        {
            //variable declarations - Start
            bool Result = false;
            string SqlStatement = null; int RecordsSaved = 0;
            //variable declarations - Stop

            try
            {
                //build sqlstatement - Start
                SqlStatement = "Select SAKO_Code, SAKO_Description From SAS_Kolej WHERE ";
                SqlStatement += " SAKO_Code = " + clsGeneric.AddQuotes(argEn.SAKO_Code);
                SqlStatement += " OR SAKO_Description = " + clsGeneric.AddQuotes(argEn.SAKO_Description);
                //build sqlstatement - Stop

                //if no duplicate records - Start
                if (_DatabaseFactory.ExecuteReader(Helper.GetDataBaseType,
                        DataBaseConnectionString, SqlStatement).Rows.Count == 0)
                {
                    //Build Sql Columns
                    
                    SqlStatement = "INSERT INTO sas_kolej VALUES (" + clsGeneric.AddQuotes(argEn.SAKO_Code);
                    SqlStatement += "," + clsGeneric.AddQuotes(argEn.SAKO_Description) +")";

                    //Save Details to Database - Start
                    RecordsSaved = _DatabaseFactory.ExecuteSqlStatement(Helper.GetDataBaseType, 
                        DataBaseConnectionString, SqlStatement);
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
        /// Method to Update Kolej 
        /// </summary>
        /// <param name="argEn">Kolej Entity is an Input.</param>
        /// <returns>Returns Boolean</returns>
        /// Edited by Zoya @ 19/02/2016
        public bool Update(KolejEn argEn)
        {
            //variable declarations - Start
            bool Result = false;
            string SqlStatement = null; int RecordsSaved = 0;
            //variable declarations - Stop

            try
            {
                //build sqlstatement - Start
                SqlStatement = "Select SAKO_Code From SAS_Kolej WHERE ";
                SqlStatement += " SAKO_Code = " + clsGeneric.AddQuotes(argEn.SAKO_Code);
                SqlStatement += " AND SAKO_Description = " + clsGeneric.AddQuotes(argEn.SAKO_Description);
                //build sqlstatement - Stop

                //if no duplicate records - Start
                if (_DatabaseFactory.ExecuteReader(Helper.GetDataBaseType,
                        DataBaseConnectionString, SqlStatement).Rows.Count == 0)
                {
                    //Build Update Statement - Start

                    SqlStatement = "UPDATE SAS_Kolej SET ";
                    SqlStatement += "SAKO_Code = " + clsGeneric.AddQuotes(argEn.SAKO_Code);
                    SqlStatement += clsGeneric.AddComma() + "SAKO_Description = " + clsGeneric.AddQuotes(argEn.SAKO_Description);
                    SqlStatement += " where SAKO_Code = " + clsGeneric.AddQuotes(argEn.SAKO_Code);
                    //Build Update Statement - Stop

                    //Save Details to Database
                    RecordsSaved = _DatabaseFactory.ExecuteSqlStatement(Helper.GetDataBaseType,
                        DataBaseConnectionString, SqlStatement);

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
        /// Method to Update Kolej 
        /// </summary>
        /// <param name="argEn">Kolej Entity is an Input.</param>
        /// <returns>Returns Boolean</returns>
        public bool Delete(KolejEn argEn)
        {
            //variable declarations
            string SqlStatement = null; int HasRows = 0; int RecordsSaved = 0; bool Result = false;

            try
            {
                //Build Sqlstatement - Start
                SqlStatement = "select sum(rows) as total from(SELECT COUNT(*) AS rows FROM  SAS_Student where SAKO_Code = ";
                SqlStatement += clsGeneric.AddQuotes(argEn.SAKO_Code) + " UNION ALL ";
                SqlStatement += "select count(*) as rows from SAS_Block WHERE SAKO_Code = ";
                SqlStatement += clsGeneric.AddQuotes(argEn.SAKO_Code) + ") AS U";
                //Build Sqlstatement - Stop

                //Get User Login Details - Start
                IDataReader _IDataReader = _DatabaseFactory.ExecuteReader(Helper.GetDataBaseType,
                        DataBaseConnectionString, SqlStatement).CreateDataReader();
                //Get User Login Details - Stop

                //if details available - Start
                if (_IDataReader != null)
                {
                    if (_IDataReader.Read())
                    {
                        HasRows = clsGeneric.NullToInteger(_IDataReader["total"]);
                        if (HasRows > 0)
                            throw new Exception("Record Already in Use");
                    }

                    _IDataReader.Close();
                    _IDataReader.Dispose();

                    //if record not used - Start
                    if (HasRows == 0)
                    {
                        //build sql statement - Start
                        SqlStatement = "DELETE FROM SAS_Kolej WHERE SAKO_Code = ";
                        SqlStatement += clsGeneric.AddQuotes(argEn.SAKO_Code);
                        //build sql statement - Stop

                        //Save Details to Database
                        RecordsSaved = _DatabaseFactory.ExecuteSqlStatement(Helper.GetDataBaseType,
                            DataBaseConnectionString, SqlStatement);

                        //if records saved successfully - Start
                        if (RecordsSaved > -1)
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

        #region Load Object 

        /// <summary>
        /// Method to Load Kolej Entity 
        /// </summary>
        /// <param name="argReader">IDataReader Object is an Input.</param>
        /// <returns>Returns Kolej Entity</returns>
        private KolejEn LoadObject(IDataReader argReader)
        {
            KolejEn loItem = new KolejEn();
            loItem.SAKO_Code = GetValue<string>(argReader, "SAKO_Code");
            loItem.SAKO_Description = GetValue<string>(argReader, "SAKO_Description");

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

        #region Get Auto Number 

        /// <summary>
        /// Method to Get AutoNumber
        /// </summary>
        /// <param name="Description">Description as Input</param>
        /// <returns>Returns AutoNumber</returns>
        public string GetAutoNumber(string Description)
        {
            string AutoNo = "";
            int CurNo = 0;
            int NoDigit = 0;
            int AutoCode = 0;
            int i = 0;
            string SqlStr;
            SqlStr = "select * from SAS_AutoNumber where SAAN_Des='" + Description + "'";

            try
            {
                IDataReader loReader = _DatabaseFactory.ExecuteReader(Helper.GetDataBaseType,
                        DataBaseConnectionString, SqlStr).CreateDataReader();

                if (loReader.Read())
                {
                    AutoCode = Convert.ToInt32(loReader["SAAN_Code"]);
                    CurNo = Convert.ToInt32(loReader["SAAN_CurNo"]) + 1;
                    NoDigit = Convert.ToInt32(loReader["SAAN_NoDigit"]);
                    AutoNo = Convert.ToString(loReader["SAAN_Prefix"]);
                    if (CurNo.ToString().Length < NoDigit)
                    {
                        while (i < NoDigit - CurNo.ToString().Length)
                        {
                            AutoNo = AutoNo + "0";
                            i = i + 1;
                        }
                        AutoNo = AutoNo + CurNo;
                    }
                    loReader.Close();

                }

                AutoNumberEn loItem = new AutoNumberEn();
                loItem.SAAN_Code = AutoCode;
                AutoNumberDAL cods = new AutoNumberDAL();
                cods.GetItem(loItem);

                loItem.SAAN_Code = Convert.ToInt32(AutoCode);
                loItem.SAAN_CurNo = CurNo;
                loItem.SAAN_AutoNo = AutoNo;


                cods.Update(loItem);


                return AutoNo;
            }

            catch (Exception ex)
            {
                Console.Write("Error in connection : " + ex.Message);
                return ex.ToString();
            }

        }

        #endregion
    }

}
