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
    public class PTPTNSetupDAL
    {
        #region Global Declarations 

        private DbParameterCollection _DbParameterCollection = null;

        private MaxModule.DatabaseProvider _DatabaseFactory = new MaxModule.DatabaseProvider();

        private string DataBaseConnectionString = Helper.GetConnectionString();

        #endregion

        public PTPTNSetupDAL()
        {
        }

        #region Get List 

        public List<PTPTNSetupEn> GetList(PTPTNSetupEn argEn)
        {
            //create instances
            List<PTPTNSetupEn> PTPTNList = new List<PTPTNSetupEn>();

            //variable declarations
            string SqlStatement = null;

            try
            {
                //build sql statement
                SqlStatement = "SELECT * FROM SAS_ptptnsetup WHERE id = 1";

                //Get User Login Details - Start
                IDataReader _IDataReader = _DatabaseFactory.ExecuteReader(Helper.GetDataBaseType,
                        DataBaseConnectionString, SqlStatement).CreateDataReader();
                //Get User Login Details - Stop

                //if details available - Start
                if (_IDataReader != null)
                {
                    while (_IDataReader.Read())
                    {
                        PTPTNSetupEn _PTPTNSetupEn = LoadObject(_IDataReader);
                        PTPTNList.Add(_PTPTNSetupEn);
                    }

                    _IDataReader.Close();
                    _IDataReader.Dispose();

                    return PTPTNList;
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

        public bool Operation(PTPTNSetupEn argEn)
        {
            //variable declarations - Start
            bool Result = false;
            string SqlStatement = null; int RecordsSaved = 0;
            //variable declarations - Stop

            try
            {
                //build sqlstatement - Start
                SqlStatement = "Select min_balance From SAS_ptptnsetup WHERE min_balance = " + argEn.min_balance + " AND id = 1";
                //build sqlstatement - Stop

                //if no duplicate records - Start
                if (_DatabaseFactory.ExecuteReader(Helper.GetDataBaseType, DataBaseConnectionString, SqlStatement).Rows.Count == 0)
                {
                    //Build Sql Columns
                    SqlStatement = "UPDATE SAS_ptptnsetup SET min_balance = " + argEn.min_balance ;
                    SqlStatement += " WHERE id = 1";
                    //Save Details to Database - Start
                    RecordsSaved = _DatabaseFactory.ExecuteSqlStatement(Helper.GetDataBaseType, DataBaseConnectionString, SqlStatement);
                    //Save Details to Database - Stop

                    //if records saved successfully - Start
                    if (RecordsSaved > -1)
                        Result = true;
                    else
                        throw new Exception("Operation Failed!");
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

        #region Reset

        public bool Reset(PTPTNSetupEn argEn)
        {
            //variable declarations - Start
            bool Result = false;
            string SqlStatement = null; int RecordsReset = 0;
            //variable declarations - Stop

            try
            {
                //Build Sql Columns
                SqlStatement = "UPDATE SAS_ptptnsetup SET min_balance = " + argEn.min_balance;
                SqlStatement += " WHERE id = 1";
                //Reset Details to Database - Start
                RecordsReset = _DatabaseFactory.ExecuteSqlStatement(Helper.GetDataBaseType, DataBaseConnectionString, SqlStatement);
                //Reset Details to Database - Stop

                //if records reset successfully - Start
                if (RecordsReset > -1)
                    Result = true;
                else
                    throw new Exception("Operation Failed!");
                //if records reset successfully - Stop

                return Result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region Load Object

        private PTPTNSetupEn LoadObject(IDataReader argReader)
        {
            PTPTNSetupEn loItem = new PTPTNSetupEn();
            loItem.id = GetValue<int>(argReader, "id");
            loItem.min_balance = GetValue<decimal>(argReader, "min_balance");

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
