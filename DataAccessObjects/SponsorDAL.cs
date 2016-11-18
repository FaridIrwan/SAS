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
    /// Class to handle all the Sponsors Methods.
    /// </summary>
    public class SponsorDAL
    {
        #region Global Declarations

        private DbParameterCollection _DbParameterCollection = null;

        private MaxModule.DatabaseProvider _DatabaseFactory =
            new MaxModule.DatabaseProvider();

        private string DataBaseConnectionString = Helper.
           GetConnectionString();

        #endregion

        public SponsorDAL()
        {
        }

        #region GetList

        /// <summary>
        /// Method to Get List of Sponsors
        /// </summary>
        /// <param name="argEn">Sponsors Entity as an Input.</param>
        /// <returns>Returns List of Sponsors</returns>
        public List<SponsorEn> GetList(SponsorEn argEn)
        {
            List<SponsorEn> loEnList = new List<SponsorEn>();
            string sqlCmd = "select * from SAS_Sponsor order by sasr_name";

            try
            {
                if (!FormHelp.IsBlank(sqlCmd))
                {
                    using (IDataReader loReader = _DatabaseFactory.ExecuteReader(Helper.GetDataBaseType,
                        DataBaseConnectionString, sqlCmd).CreateDataReader())
                    {
                        while (loReader.Read())
                        {
                            SponsorEn loItem = LoadObject(loReader);
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

        #region GetSponserList

        /// <summary>
        /// Method to Get List Active or Inactive of Sponsors
        /// </summary>
        /// <param name="argEn">Sponsors Entity as an Input.SponsorCode,Name,Type,GLAccount and Status as Input Properties.</param>
        /// <returns>Returns List of Sponsors</returns>
        public List<SponsorEn> GetSponserList(SponsorEn argEn)
        {
            List<SponsorEn> loEnList = new List<SponsorEn>();
            SponsorFeeTypesDAL dobjSPFeeType = new SponsorFeeTypesDAL();
            SponsorFeeTypesEn eobjSPFeeType;
            argEn.SponserCode = argEn.SponserCode.Replace("*", "%");
            argEn.Name = argEn.Name.Replace("*", "%");
            argEn.Type = argEn.Type.Replace("*", "%");
            argEn.GLAccount = argEn.GLAccount.Replace("*", "%");
            string sqlCmd = "select SASR_Code,SASR_Name,SASSR_SName,SASR_Address,SASR_Address1,SASR_Address2,SASR_Contact,SASR_Phone,SASR_Fax,SASR_Email,SASR_WebSite,SASR_Type,SASR_Desc,SASR_GLAccount,SASR_Status,SASR_ptptn from SAS_Sponsor where SASR_Code <> '0'";
            if (argEn.SponserCode.Length != 0) sqlCmd = sqlCmd + " and SASR_Code like '" + argEn.SponserCode + "'";
            if (argEn.Name.Length != 0) sqlCmd = sqlCmd + " and SASR_Name like '" + argEn.Name + "'";
            if (argEn.Type.Length != 0) sqlCmd = sqlCmd + " and SASR_Type like '" + argEn.Type + "'";
            if (argEn.GLAccount.Length != 0) sqlCmd = sqlCmd + " and SASR_GLAccount like '" + argEn.GLAccount + "'";

            if (argEn.Status == true) sqlCmd = sqlCmd + " and SASR_Status ='true'";
            if (argEn.Status == false) sqlCmd = sqlCmd + " and SASR_Status ='false'";

            //if (argEn.ptptn == true) sqlCmd = sqlCmd + " and SASR_ptptn ='true'";
            //if (argEn.ptptn == false) sqlCmd = sqlCmd + " and SASR_ptptn ='false'";

            sqlCmd = sqlCmd + " order by SASR_Code";

            try
            {
                if (!FormHelp.IsBlank(sqlCmd))
                {
                    using (IDataReader loReader = _DatabaseFactory.ExecuteReader(Helper.GetDataBaseType,
                        DataBaseConnectionString, sqlCmd).CreateDataReader())
                    {
                        while (loReader.Read())
                        {
                            SponsorEn loItem = LoadObject(loReader);
                            eobjSPFeeType = new SponsorFeeTypesEn();
                            eobjSPFeeType.SponserCode = loItem.SponserCode;
                            loItem.LstSponserFeeTypes = dobjSPFeeType.GetSPFeeTypeList(eobjSPFeeType);
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

        #region GetSponsorBal

        /// <summary>
        /// Method to Get List Active or Inactive of Sponsors
        /// </summary>
        /// <param name="argEn">Sponsors Entity as an Input.SponsorCode,Name,Type,GLAccount and Status as Input Properties.</param>
        /// <returns>Returns List of Sponsors</returns>
        public List<SponsorEn> GetSponsorBal(SponsorEn argEn)
        {
            List<SponsorEn> loEnList = new List<SponsorEn>();
            SponsorFeeTypesDAL dobjSPFeeType = new SponsorFeeTypesDAL();
            SponsorFeeTypesEn eobjSPFeeType;
            argEn.SponserCode = argEn.SponserCode.Replace("*", "%");
            argEn.Name = argEn.Name.Replace("*", "%");
            argEn.Type = argEn.Type.Replace("*", "%");
            argEn.GLAccount = argEn.GLAccount.Replace("*", "%");
            string sqlCmd = "select SASR_Code,SASR_Name,SASSR_SName,SASR_Address,SASR_Address1,SASR_Address2,SASR_Contact,SASR_Phone,SASR_Fax,SASR_Email,SASR_WebSite,SASR_Type,SASR_Desc,SASR_GLAccount,SASR_Status from SAS_Sponsor where SASR_Code <> '0'";
            if (argEn.SponserCode.Length != 0) sqlCmd = sqlCmd + " and SASR_Code like '" + argEn.SponserCode + "'";
            if (argEn.Name.Length != 0) sqlCmd = sqlCmd + " and SASR_Name like '" + argEn.Name + "'";
            if (argEn.Type.Length != 0) sqlCmd = sqlCmd + " and SASR_Type like '" + argEn.Type + "'";
            if (argEn.GLAccount.Length != 0) sqlCmd = sqlCmd + " and SASR_GLAccount like '" + argEn.GLAccount + "'";
            //if (argEn.Status == true) sqlCmd = sqlCmd + " and SASR_Status =1";
            if (argEn.Status == true) sqlCmd = sqlCmd + " and SASR_Status = 'true'";
            //if (argEn.Status == false) sqlCmd = sqlCmd + " and SASR_Status =0";
            if (argEn.Status == false) sqlCmd = sqlCmd + " and SASR_Status = 'false'";
            sqlCmd = sqlCmd + " order by SASR_Code";

            try
            {
                if (!FormHelp.IsBlank(sqlCmd))
                {
                    using (IDataReader loReader = _DatabaseFactory.ExecuteReader(Helper.GetDataBaseType,
                        DataBaseConnectionString, sqlCmd).CreateDataReader())
                    {
                        while (loReader.Read())
                        {
                            SponsorEn loItem = LoadObject(loReader);
                            eobjSPFeeType = new SponsorFeeTypesEn();
                            eobjSPFeeType.SponserCode = loItem.SponserCode;
                            loItem.LstSponserFeeTypes = dobjSPFeeType.GetSPFeeTypeList(eobjSPFeeType);
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
        /// Method to Get Sponsor Entity
        /// </summary>
        /// <param name="argEn">Sponsor Entity is an Input.SASR_Code as Input Property.</param>
        /// <returns>Returns Sponsor Entity</returns>
        public SponsorEn GetItem(SponsorEn argEn)
        {
            SponsorEn loItem = new SponsorEn();
            string sqlCmd = "Select * FROM SAS_Sponsor WHERE SASR_Code='" + argEn.SponsorID + "'";

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
        /// Method to Insert Sponsor 
        /// </summary>
        /// <param name="argEn">Sponsor Entity is an Input.</param>
        /// <returns>Returns Boolean</returns>
        public bool Insert(SponsorEn argEn)
        {
            bool lbRes = false;
            int iOut = 0, resetPTPTN = 0;

            SponsorFeeTypesEn eobjSPFeeType = new SponsorFeeTypesEn();
            SponsorFeeTypesDAL dobjSPFeeType = new SponsorFeeTypesDAL();
            string sqlCmd = "Select count(*) as cnt From SAS_Sponsor WHERE SASR_Code = @SASR_Code or SASR_Name = @SASR_Name";
            try
            {
                if (!FormHelp.IsBlank(sqlCmd))
                {
                    DbCommand cmdSel = _DatabaseFactory.GetDbCommand(Helper.GetDataBaseType, sqlCmd, DataBaseConnectionString);
                    _DatabaseFactory.AddInParameter(ref cmdSel, "@SASR_Code", DbType.String, argEn.SponserCode);
                    _DatabaseFactory.AddInParameter(ref cmdSel, "@SASR_Name", DbType.String, argEn.Name);
                    _DbParameterCollection = cmdSel.Parameters;

                    using (IDataReader dr = _DatabaseFactory.GetIDataReader(Helper.GetDataBaseType, cmdSel,
                        DataBaseConnectionString, sqlCmd, _DbParameterCollection).CreateDataReader())
                    {
                        //if (dr.Read())
                        if (dr.Read() && dr.FieldCount != 0)
                            //iOut = GetValue<int>(dr, "cnt");
                            iOut = clsGeneric.NullToInteger(dr["cnt"]);
                        if (iOut > 0)
                            throw new Exception("Record Already Exist");
                    }
                    if (iOut == 0)
                    {
                        sqlCmd = "INSERT INTO SAS_Sponsor(SASR_Code,SASR_Name,SASSR_SName,SASR_Address,SASR_Address1,SASR_Address2,SASR_Contact,SASR_Phone,SASR_Fax,SASR_Email,SASR_WebSite,SASR_Type,SASR_Desc,SASR_GLAccount,SABR_Code,SASR_UpdatedBy,SASR_UpdatedDtTm,SASR_Status,SASR_ptptn) VALUES (@SASR_Code,@SASR_Name,@SASSR_SName,@SASR_Address,@SASR_Address1,@SASR_Address2,@SASR_Contact,@SASR_Phone,@SASR_Fax,@SASR_Email,@SASR_WebSite,@SASR_Type,@SASR_Desc,@SASR_GLAccount,@SABR_Code,@SASR_UpdatedBy,@SASR_UpdatedDtTm,@SASR_Status,@SASR_ptptn) ";
                        if (!FormHelp.IsBlank(sqlCmd))
                        {
                            DbCommand cmd = _DatabaseFactory.GetDbCommand(Helper.GetDataBaseType, sqlCmd, DataBaseConnectionString);
                            _DatabaseFactory.AddInParameter(ref cmd, "@SASR_Code", DbType.String, argEn.SponserCode);
                            _DatabaseFactory.AddInParameter(ref cmd, "@SASR_Name", DbType.String, argEn.Name);
                            _DatabaseFactory.AddInParameter(ref cmd, "@SASSR_SName", DbType.String, argEn.SName);
                            _DatabaseFactory.AddInParameter(ref cmd, "@SASR_Address", DbType.String, argEn.Address);
                            _DatabaseFactory.AddInParameter(ref cmd, "@SASR_Address1", DbType.String, argEn.Address1);
                            _DatabaseFactory.AddInParameter(ref cmd, "@SASR_Address2", DbType.String, argEn.Address2);
                            _DatabaseFactory.AddInParameter(ref cmd, "@SASR_Contact", DbType.String, argEn.Contact);
                            _DatabaseFactory.AddInParameter(ref cmd, "@SASR_Phone", DbType.String, argEn.Phone);
                            _DatabaseFactory.AddInParameter(ref cmd, "@SASR_Fax", DbType.String, argEn.Fax);
                            _DatabaseFactory.AddInParameter(ref cmd, "@SASR_Email", DbType.String, argEn.Email);
                            _DatabaseFactory.AddInParameter(ref cmd, "@SASR_WebSite", DbType.String, argEn.WebSite);
                            _DatabaseFactory.AddInParameter(ref cmd, "@SASR_Type", DbType.String, argEn.Type);
                            _DatabaseFactory.AddInParameter(ref cmd, "@SASR_Desc", DbType.String, argEn.Description);
                            _DatabaseFactory.AddInParameter(ref cmd, "@SASR_GLAccount", DbType.String, argEn.GLAccount);
                            _DatabaseFactory.AddInParameter(ref cmd, "@SABR_Code", DbType.Int32, argEn.Code);
                            _DatabaseFactory.AddInParameter(ref cmd, "@SASR_UpdatedBy", DbType.String, argEn.UpdatedBy);
                            _DatabaseFactory.AddInParameter(ref cmd, "@SASR_UpdatedDtTm", DbType.String, argEn.UpdatedDtTm);
                            _DatabaseFactory.AddInParameter(ref cmd, "@SASR_Status", DbType.Boolean, argEn.Status);
                            _DatabaseFactory.AddInParameter(ref cmd, "@SASR_ptptn", DbType.Boolean, argEn.ptptn);
                            _DbParameterCollection = cmd.Parameters;

                            int liRowAffected = _DatabaseFactory.ExecuteNonQuery(Helper.GetDataBaseType, cmd,
                                DataBaseConnectionString, sqlCmd, _DbParameterCollection);

                            int i = 0;
                            eobjSPFeeType.SponserCode = argEn.SponserCode;
                            //Deleting the existing sponsor feetypes
                            dobjSPFeeType.Delete(eobjSPFeeType);
                            //Inserting new Sponsor feetypes
                            while (i < argEn.LstSponserFeeTypes.Count)
                            {
                                eobjSPFeeType = argEn.LstSponserFeeTypes[i];
                                dobjSPFeeType.Insert(eobjSPFeeType);
                                eobjSPFeeType = null;
                                i = i + 1;
                            }

                            if (liRowAffected > -1)
                            {
                                lbRes = true;

                                if (argEn.ptptn == true)
                                {
                                    sqlCmd = "UPDATE SAS_Sponsor SET SASR_ptptn = false " +
                                             "WHERE SASR_Code != " + clsGeneric.AddQuotes(argEn.SponserCode);

                                    //Update Details to Database
                                    resetPTPTN = _DatabaseFactory.ExecuteSqlStatement(Helper.GetDataBaseType,
                                        DataBaseConnectionString, sqlCmd);

                                    if (resetPTPTN > -1)
                                        lbRes = true;
                                    else
                                        throw new Exception("Reset Current Semester Failed! No Row has been updated...");
                                }
                            }
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
        /// Method to Update Sponsor 
        /// </summary>
        /// <param name="argEn">Sponsor Entity is an Input.</param>
        /// <returns>Returns Boolean</returns>
        public bool Update(SponsorEn argEn)
        {
            bool lbRes = false;
            int iOut = 0, resetPTPTN = 0; ;
            string sqlCmd = "Select count(*) as cnt From SAS_Sponsor WHERE SASR_Code != @SASR_Code and SASR_Name = @SASR_Name";
            try
            {
                if (!FormHelp.IsBlank(sqlCmd))
                {
                    DbCommand cmdSel = _DatabaseFactory.GetDbCommand(Helper.GetDataBaseType, sqlCmd, DataBaseConnectionString);
                    _DatabaseFactory.AddInParameter(ref cmdSel, "@SASR_Code", DbType.String, argEn.SponserCode);
                    _DatabaseFactory.AddInParameter(ref cmdSel, "@SASR_Name", DbType.String, argEn.Name);
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
                        sqlCmd = "UPDATE SAS_Sponsor SET SASR_Code = @SASR_Code, SASR_Name = @SASR_Name, SASSR_SName = @SASSR_SName, SASR_Address = @SASR_Address, SASR_Address1 = @SASR_Address1, SASR_Address2 = @SASR_Address2, SASR_Contact = @SASR_Contact, SASR_Phone = @SASR_Phone, SASR_Fax = @SASR_Fax, SASR_Email = @SASR_Email, SASR_WebSite = @SASR_WebSite, SASR_Type = @SASR_Type, SASR_Desc = @SASR_Desc, SASR_GLAccount = @SASR_GLAccount, SABR_Code = @SABR_Code, SASR_UpdatedBy = @SASR_UpdatedBy, SASR_UpdatedDtTm = @SASR_UpdatedDtTm, SASR_Status = @SASR_Status, SASR_ptptn = @SASR_ptptn WHERE SASR_Code = @SASR_Code";
                        if (!FormHelp.IsBlank(sqlCmd))
                        {
                            DbCommand cmd = _DatabaseFactory.GetDbCommand(Helper.GetDataBaseType, sqlCmd, DataBaseConnectionString);
                            _DatabaseFactory.AddInParameter(ref cmd, "@SASR_Code", DbType.String, argEn.SponserCode);
                            _DatabaseFactory.AddInParameter(ref cmd, "@SASR_Name", DbType.String, argEn.Name);
                            _DatabaseFactory.AddInParameter(ref cmd, "@SASSR_SName", DbType.String, argEn.SName);
                            _DatabaseFactory.AddInParameter(ref cmd, "@SASR_Address", DbType.String, argEn.Address);
                            _DatabaseFactory.AddInParameter(ref cmd, "@SASR_Address1", DbType.String, argEn.Address1);
                            _DatabaseFactory.AddInParameter(ref cmd, "@SASR_Address2", DbType.String, argEn.Address2);
                            _DatabaseFactory.AddInParameter(ref cmd, "@SASR_Contact", DbType.String, argEn.Contact);
                            _DatabaseFactory.AddInParameter(ref cmd, "@SASR_Phone", DbType.String, argEn.Phone);
                            _DatabaseFactory.AddInParameter(ref cmd, "@SASR_Fax", DbType.String, argEn.Fax);
                            _DatabaseFactory.AddInParameter(ref cmd, "@SASR_Email", DbType.String, argEn.Email);
                            _DatabaseFactory.AddInParameter(ref cmd, "@SASR_WebSite", DbType.String, argEn.WebSite);
                            _DatabaseFactory.AddInParameter(ref cmd, "@SASR_Type", DbType.String, argEn.Type);
                            _DatabaseFactory.AddInParameter(ref cmd, "@SASR_Desc", DbType.String, argEn.Description);
                            _DatabaseFactory.AddInParameter(ref cmd, "@SASR_GLAccount", DbType.String, argEn.GLAccount);
                            _DatabaseFactory.AddInParameter(ref cmd, "@SABR_Code", DbType.Int32, argEn.Code);
                            _DatabaseFactory.AddInParameter(ref cmd, "@SASR_UpdatedBy", DbType.String, argEn.UpdatedBy);
                            _DatabaseFactory.AddInParameter(ref cmd, "@SASR_UpdatedDtTm", DbType.String, argEn.UpdatedDtTm);
                            _DatabaseFactory.AddInParameter(ref cmd, "@SASR_Status", DbType.Boolean, argEn.Status);
                            _DatabaseFactory.AddInParameter(ref cmd, "@SASR_ptptn", DbType.Boolean, argEn.ptptn);
                            _DbParameterCollection = cmd.Parameters;

                            int liRowAffected = _DatabaseFactory.ExecuteNonQuery(Helper.GetDataBaseType, cmd,
                                DataBaseConnectionString, sqlCmd, _DbParameterCollection);


                            SponsorFeeTypesEn eobjSPFeeType = new SponsorFeeTypesEn();
                            SponsorFeeTypesDAL dobjSPFeeType = new SponsorFeeTypesDAL();
                            int i = 0;
                            eobjSPFeeType.SponserCode = argEn.SponserCode;
                            //Deleting the existing sponsor feetypes
                            dobjSPFeeType.Delete(eobjSPFeeType);
                            //Inserting new Sponsor feetypes
                            while (i < argEn.LstSponserFeeTypes.Count)
                            {
                                eobjSPFeeType = argEn.LstSponserFeeTypes[i];
                                dobjSPFeeType.Insert(eobjSPFeeType);
                                eobjSPFeeType = null;
                                i = i + 1;
                            }

                            if (liRowAffected > -1)
                            {
                                lbRes = true;

                                if (argEn.ptptn == true)
                                {
                                    sqlCmd = "UPDATE SAS_Sponsor SET SASR_ptptn = false " +
                                             "WHERE SASR_Code != " + clsGeneric.AddQuotes(argEn.SponserCode);

                                    //Update Details to Database
                                    resetPTPTN = _DatabaseFactory.ExecuteSqlStatement(Helper.GetDataBaseType,
                                        DataBaseConnectionString, sqlCmd);

                                    if (resetPTPTN > -1)
                                        lbRes = true;
                                    else
                                        throw new Exception("Reset Current Semester Failed! No Row has been updated...");
                                }
                            }
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
        /// Method to Delete Sponsor 
        /// </summary>
        /// <param name="argEn">Sponsor Entity is an Input.SASR_Code as Input Property.</param>
        /// <returns>Returns Boolean</returns>
        public bool Delete(SponsorEn argEn)
        {
            bool lbRes = false;
            int total = 0;
            string sqlCmd = "select sum(rows) as total from (select count(*) as rows from SAS_Accounts WHERE CreditRef = @CreditRef  union all select count(*) as rows from SAS_StudentSpon WHERE SASS_Sponsor = @CreditRef)AS U";

            try
            {
                if (!FormHelp.IsBlank(sqlCmd))
                {
                    DbCommand cmdSel = _DatabaseFactory.GetDbCommand(Helper.GetDataBaseType, sqlCmd, DataBaseConnectionString);
                    _DatabaseFactory.AddInParameter(ref cmdSel, "@CreditRef", DbType.String, argEn.SponserCode);
                    _DbParameterCollection = cmdSel.Parameters;

                    using (IDataReader dr = _DatabaseFactory.GetIDataReader(Helper.GetDataBaseType, cmdSel,
                        DataBaseConnectionString, sqlCmd, _DbParameterCollection).CreateDataReader())
                    {
                        if (dr.Read())
                            total = clsGeneric.NullToInteger(dr["total"]);
                        if (total > 0)
                            throw new Exception("Record Already in Use");
                    }
                    if (total == 0)
                    {
                        sqlCmd = "DELETE FROM SAS_Sponsor WHERE SASR_Code = @SASR_Code";
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
                                throw new Exception("Delete Failed! No Row has been deleted...");
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

        #region LoadObject

        /// <summary>
        /// Method to Load Sponsor Entity 
        /// </summary>
        /// <param name="argReader">IDataReader Object is an Input.</param>
        /// <returns>Returns Sponsor Entity</returns>
        private SponsorEn LoadObject(IDataReader argReader)
        {
            SponsorEn loItem = new SponsorEn();
            loItem.SponserCode = GetValue<string>(argReader, "SASR_Code");
            loItem.Name = GetValue<string>(argReader, "SASR_Name");
            loItem.SName = GetValue<string>(argReader, "SASSR_SName");
            loItem.Address = GetValue<string>(argReader, "SASR_Address");
            loItem.Address1 = GetValue<string>(argReader, "SASR_Address1");
            loItem.Address2 = GetValue<string>(argReader, "SASR_Address2");
            loItem.Contact = GetValue<string>(argReader, "SASR_Contact");
            loItem.Phone = GetValue<string>(argReader, "SASR_Phone");
            loItem.Fax = GetValue<string>(argReader, "SASR_Fax");
            loItem.Email = GetValue<string>(argReader, "SASR_Email");
            loItem.WebSite = GetValue<string>(argReader, "SASR_WebSite");
            loItem.Type = GetValue<string>(argReader, "SASR_Type");
            loItem.Description = GetValue<string>(argReader, "SASR_Desc");
            loItem.GLAccount = GetValue<string>(argReader, "SASR_GLAccount");
            loItem.Status = GetValue<bool>(argReader, "SASR_Status");
            loItem.ptptn = GetValue<bool>(argReader, "SASR_ptptn");

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

        #region Get Sponsor Posted Invoices

        public DataTable GetSponsorPostedInvoice(string SponsorID, string AccTransId)
        {
            //variable declaractions
            string SqlStatement = null;

            try
            {
                //Build Sql Statement - Start
                //SqlStatement = "SELECT transid AS invoice_id, transcode::text || ' - ' || transamount::text AS invoice_details FROM sas_sponsorinvoice ";
                //SqlStatement += " WHERE transid NOT IN (SELECT invoice_id from sas_sponsor_inv_rec);";

                if (AccTransId == "")
                {
                    //SqlStatement = "SELECT transid AS invoice_id, batchcode::text || ' - ' || transamount::text AS invoice_details FROM sas_sponsorinvoice ";
                    //SqlStatement += " WHERE transid NOT IN (SELECT invoice_id from sas_sponsor_inv_rec) and PostStatus ='Posted' and creditref1='" + SponsorID + "'";
                    SqlStatement = "select distinct batchcode as  invoice_id, batchcode::text || ' - ' || transamount::text AS invoice_details FROM sas_sponsorinvoice  ";
                    SqlStatement += " where PostStatus ='Posted' and creditref1='" + SponsorID + "' group by batchcode,transamount";

                }
                else
                {
                    SqlStatement = "select spi.batchcode as invoice_id, batchcode::text || ' - ' || spi.transamount::text AS invoice_details FROM sas_sponsorinvoice spi";
                    SqlStatement += " inner join sas_sponsor_inv_rec spr on spi.batchcode=spr.invoice_id where spr.receipt_id='" + AccTransId + "' and spi.creditref1='" + SponsorID + "' group by spi.batchcode , spi.transamount";


                }
                //Build Sql Statement - Stop

                return _DatabaseFactory.ExecuteDataTable(Helper.GetDataBaseType,
                    DataBaseConnectionString, SqlStatement);
            }
            catch (Exception ex)
            {
                MaxModule.Helper.LogError(ex.Message);
                return null;
            }
        }

        #endregion
    }

}
