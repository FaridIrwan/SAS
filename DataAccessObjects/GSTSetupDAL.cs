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
    public class GSTSetupDAL
    {
        #region Global Declarations

        private DbParameterCollection _DbParameterCollection = null;

        private MaxModule.DatabaseProvider _DatabaseFactory =
            new MaxModule.DatabaseProvider();

        private string DataBaseConnectionString = Helper.
           GetConnectionString();

        #endregion

        #region Insert / Update

        /// <summary>
        /// Method to Insert / Update
        /// </summary>
        /// <param name="argEn">GST SetUp is an Input.   </param>
        /// <returns>Returns Boolean</returns>
        /// Edited by Zoya @ 18/2/2016
        public int InsertUpdate(int GstTaxId, int TaxCode, string TaxType, decimal TaxPercentage, short TaxMode, string GLAccount)
        {
            //create instances
            DbCommand _DbCommand;

            //variable declarations
            string SqlStatement = null;

            try
            {
                if (GstTaxId == 0)
                {
                    //string sqlCmd = "Select count(*) as cnt From sas_gst_taxsetup WHERE sas_taxcode = @TaxCode and sas_taxtype = @TaxType and sas_taxmod = @TaxMode and sas_taxpercentage = @TaxPer";

                    string sqlCmd = "Select count(*) as cnt From sas_gst_taxsetup WHERE sas_taxtype = @TaxType";

                    if (!FormHelp.IsBlank(sqlCmd))
                    {
                        int iOut = 0;
                        DbCommand cmdSel = _DatabaseFactory.GetDbCommand(Helper.GetDataBaseType, sqlCmd, DataBaseConnectionString);
                        _DatabaseFactory.AddInParameter(ref cmdSel, "@TaxCode", DbType.Int32, TaxCode);
                        _DatabaseFactory.AddInParameter(ref cmdSel, "@TaxType", DbType.String, TaxType);
                        _DatabaseFactory.AddInParameter(ref cmdSel, "@TaxPer", DbType.Decimal, TaxPercentage);
                        _DatabaseFactory.AddInParameter(ref cmdSel, "@TaxMode", DbType.Int16, TaxMode);
                        _DatabaseFactory.AddInParameter(ref cmdSel, "@GLAccount", DbType.String, GLAccount);
                        _DbParameterCollection = cmdSel.Parameters;

                        using (IDataReader dr = _DatabaseFactory.GetIDataReader(Helper.GetDataBaseType, cmdSel,
                            DataBaseConnectionString, sqlCmd, _DbParameterCollection).CreateDataReader())
                        {
                            if (dr.Read())
                                iOut = clsGeneric.NullToInteger(dr["cnt"]);
                            if (iOut > 0)
                                return -2;
                            //throw new Exception("Record Already Exist");
                        }
                        if (iOut == 0)
                        {
                            SqlStatement = "INSERT INTO sas_gst_taxsetup(sas_taxcode,sas_taxtype,sas_taxmod,sas_taxpercentage,sas_glaccount)";
                            SqlStatement += " VALUES (@TaxCode, @TaxType, @TaxMode, @TaxPer, @GLAccount)";
                        }
                        else
                            SqlStatement = string.Empty;
                    }
                }
                else
                {
                    SqlStatement = "Update sas_gst_taxsetup set sas_taxcode=@TaxCode,sas_taxtype=@TaxType,sas_taxmod=@TaxMode,sas_taxpercentage=@TaxPer,sas_glaccount=@GLAccount";
                    SqlStatement += " where sas_taxid=@GstTaxId";
                }

                    if (!FormHelp.IsBlank(SqlStatement))
                    {
                        _DbCommand = _DatabaseFactory.GetDbCommand(Helper.GetDataBaseType, SqlStatement, DataBaseConnectionString);
                        _DatabaseFactory.AddInParameter(ref _DbCommand, "@TaxCode", DbType.Int32, TaxCode);
                        _DatabaseFactory.AddInParameter(ref _DbCommand, "@TaxType", DbType.String, TaxType);
                        _DatabaseFactory.AddInParameter(ref _DbCommand, "@TaxPer", DbType.Decimal, TaxPercentage);
                        _DatabaseFactory.AddInParameter(ref _DbCommand, "@TaxMode", DbType.Int16, TaxMode);
                        _DatabaseFactory.AddInParameter(ref _DbCommand, "@GLAccount", DbType.String, GLAccount);

                        if (GstTaxId > 0)
                        {
                            _DatabaseFactory.AddInParameter(ref _DbCommand, "@GstTaxId", DbType.Int32, GstTaxId);
                        }

                        _DbParameterCollection = _DbCommand.Parameters;

                        return _DatabaseFactory.ExecuteNonQuery(Helper.GetDataBaseType, _DbCommand,
                            DataBaseConnectionString, SqlStatement, _DbParameterCollection);
                    }
               
                    return -1;
               
            }
            catch (Exception ex)
            {
                MaxModule.Helper.LogError(ex.Message);
                return -1;
            }
        }

        #endregion

        #region Delete Record

        /// <summary>
        /// Method to Delete
        /// </summary>
        /// <param name="argEn">GST SetUp is an Input.   </param>
        /// <returns>Returns Boolean</returns>
        /// Edited by Zoya @ 19/2/2016
        public int Delete(int GstTaxId)
        {
            //create instances
            DbCommand _DbCommand;

            //variable declarations
            string SqlStatement = null;

            try
            {
         
                if (GstTaxId != 0)
                {
                    string sqlCmd = "SELECT COUNT(*)as cnt FROM SAS_FeeTypes right outer join sas_gst_taxsetup ON SAS_FeeTypes.saft_taxmode = sas_gst_taxsetup.sas_taxid" +
                      " WHERE (SAS_FeeTypes.saft_taxmode= @GstTaxId)";

                    if (!FormHelp.IsBlank(sqlCmd))
                    {
                        int iOut = 0;
                        DbCommand cmdSel = _DatabaseFactory.GetDbCommand(Helper.GetDataBaseType, sqlCmd, DataBaseConnectionString);
                        _DatabaseFactory.AddInParameter(ref cmdSel, "@GstTaxId", DbType.Int32, GstTaxId);
                        _DbParameterCollection = cmdSel.Parameters;

                        using (IDataReader dr = _DatabaseFactory.GetIDataReader(Helper.GetDataBaseType, cmdSel,
                            DataBaseConnectionString, sqlCmd, _DbParameterCollection).CreateDataReader())
                        {
                            if (dr.Read())
                                iOut = clsGeneric.NullToInteger(dr["cnt"]);
                            if (iOut > 0)
                                return -2;
                            //throw new Exception("Record Already In Use");
                        }

                        if (iOut == 0)
                        {

                            SqlStatement = "DELETE FROM sas_gst_taxsetup WHERE sas_taxid=@GstTaxId";

                        }

                    }
                }

                if (!FormHelp.IsBlank(SqlStatement))
                {
                    _DbCommand = _DatabaseFactory.GetDbCommand(Helper.GetDataBaseType, SqlStatement, DataBaseConnectionString);
                    _DatabaseFactory.AddInParameter(ref _DbCommand, "@GstTaxId", DbType.Int32, GstTaxId);
                    _DbParameterCollection = _DbCommand.Parameters;

                    return _DatabaseFactory.ExecuteNonQuery(Helper.GetDataBaseType, _DbCommand,
                        DataBaseConnectionString, SqlStatement, _DbParameterCollection);
                }

                return -1;
            }

            catch (Exception ex)
            {
                MaxModule.Helper.LogError(ex.Message);
                return -1;
            }
        }

        #endregion

        #region Get Gst Details

        /// <summary>
        /// Method to Get ChequeDetails 
        /// </summary>
        /// <param name="argEn">ChequeDetails Entity is an Input.ProcessID is Input Property.</param>
        /// <returns>Returns List of ChequeDetails</returns>
        public DataSet GetGstDetails(int GstTaxId)
        {
            //variable declarations
            string SqlStatement = null;

            try
            {
                if (GstTaxId == 0)
                {

                    SqlStatement = "select sas_taxid, CASE sas_taxcode";
                    SqlStatement += " WHEN 1 THEN 'SR'";
                    SqlStatement += " WHEN 2 THEN 'ES'";
                    SqlStatement += " WHEN 3 THEN 'OS'";
                    SqlStatement += " WHEN 4 THEN 'DS'";
                    SqlStatement += " WHEN 5 THEN 'GS'";
                    SqlStatement += " WHEN 6 THEN 'RS'";
                    SqlStatement += " WHEN 7 THEN 'AJS'";
                    SqlStatement += " WHEN 8 THEN 'ES43'";
                    SqlStatement += " WHEN 9 THEN 'ZRL'";
                    SqlStatement += " WHEN 10 THEN 'ZRE'";
                    SqlStatement += " ELSE 'Invalid Code'";
                    SqlStatement += " END as taxcode,sas_taxtype,CASE sas_taxmod";
                    SqlStatement += " WHEN 1 THEN 'Exclusive'";
                    SqlStatement += " WHEN 2 THEN 'Inclusive'";
                    SqlStatement += " ELSE 'Invalid Code'";
                    SqlStatement += " END as taxmode";
                    SqlStatement += ", sas_taxpercentage, sas_glaccount as glaccount";
                    SqlStatement += " FROM sas_gst_taxsetup";
                }
                else
                {
                    SqlStatement = "SELECT * FROM sas_gst_taxsetup where sas_taxid= " + GstTaxId;
                }

                if (!FormHelp.IsBlank(SqlStatement))
                {
                    DataSet _DataSet = _DatabaseFactory.ExecuteDataSet(Helper.GetDataBaseType,
                        DataBaseConnectionString, SqlStatement);

                    return _DataSet;
                }

            }
            catch (Exception ex)
            {
                return null;
            }
            return null;
        }

        #endregion

        #region Get Gst Amount

        public decimal GetGstAmount(int Taxid, decimal Amount)
        {
            //varaible declaration
            string SqlStatement = null; decimal GstAmount = 0; short TaxMode = 0; decimal TaxPercentage = 0;

            try
            {
                //build sql statement
                SqlStatement = "select sas_taxpercentage,sas_taxmod from sas_gst_taxsetup where sas_taxid =" + Taxid;

                //if sql statement not blank - Start
                if (!FormHelp.IsBlank(SqlStatement))
                {
                    //get Gst Details- Start
                    DataSet _DataSet = _DatabaseFactory.ExecuteDataSet(Helper.GetDataBaseType,
                        DataBaseConnectionString, SqlStatement);
                    //get Gst Details- Stop

                    //if data available - Start
                    if (_DataSet.Tables[0].Rows.Count > 0)
                    {
                        //get Values - Start
                        TaxMode = clsGeneric.NullToShort(_DataSet.Tables[0].Rows[0]["sas_taxmod"]);
                        TaxPercentage = clsGeneric.NullToShort(_DataSet.Tables[0].Rows[0]["sas_taxpercentage"]);
                        //get Values - Stop
                    }
                    //if data available - Stop

                    //calculate tax amount - start
                    if (TaxMode == (short)(Helper.TaxMode.Inclusive))
                    {
                        GstAmount = (Amount * TaxPercentage) / (100 + TaxPercentage);
                        return GstAmount;
                    }
                    else if (TaxMode == (short)(Helper.TaxMode.Exclusive))
                    {
                        GstAmount = (Amount * TaxPercentage) / 100;
                        return GstAmount;
                    }
                    //calculate tax amount - stop
                }
                //if sql statement not blank - Stop

                return 0;
            }
            catch
            {
                return 0;
            }

        }

        #endregion
    }

}
