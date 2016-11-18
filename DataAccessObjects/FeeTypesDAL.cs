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
    /// Class to handle all the FeeTypes Methods.
    /// </summary>
    public class FeeTypesDAL
    {
        #region Global Declarations

        private DbParameterCollection _DbParameterCollection = null;

        private MaxModule.DatabaseProvider _DatabaseFactory =
            new MaxModule.DatabaseProvider();

        private string DataBaseConnectionString = Helper.
           GetConnectionString();

        #endregion

        public FeeTypesDAL()
        {
        }

        #region GetList

        /// <summary>
        /// Method to Get List of FeeTypes
        /// </summary>
        /// <param name="argEn">FeeTypes Entity is an Input.</param>
        /// <returns>Returns List of FeeTypes</returns>
        public List<FeeTypesEn> GetList(FeeTypesEn argEn)
        {
            List<FeeTypesEn> loEnList = new List<FeeTypesEn>();
            string sqlCmd = "select * from SAS_FeeTypes";

            try
            {
                if (!FormHelp.IsBlank(sqlCmd))
                {
                    using (IDataReader loReader = _DatabaseFactory.ExecuteReader(Helper.GetDataBaseType,
                        DataBaseConnectionString, sqlCmd).CreateDataReader())
                    {
                        while (loReader.Read())
                        {
                            FeeTypesEn loItem = LoadObject(loReader);
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

        #region GetFeeList

        /// <summary>
        /// Method to Get List of FeeTypes
        /// </summary>
        /// <param name="argEn">FeeTypes Entity is an Input.FeeTypeCode,FeeType,Description and Status are Input Properties.</param>
        /// <returns>Returns List of FeeTypes</returns>
        public List<FeeTypesEn> GetFeeList(FeeTypesEn argEn)
        {
            List<FeeTypesEn> loEnList = new List<FeeTypesEn>();
            argEn.FeeTypeCode = argEn.FeeTypeCode.Replace("*", "%");
            argEn.FeeType = argEn.FeeType.Replace("*", "%");
            argEn.Description = argEn.Description.Replace("*", "%");
            string sqlCmd = "SELECT * from SAS_FeeTypes WHERE SAS_FeeTypes.SAFT_Code <> '0' ";
            if (argEn.FeeTypeCode.Length != 0) sqlCmd = sqlCmd + " and SAFT_Code like '" + argEn.FeeTypeCode + "'";
            if (argEn.FeeType.Length != 0) sqlCmd = sqlCmd + " and SAS_SAFT_FeeType like '" + argEn.FeeType + "'";
            if (argEn.Description.Length != 0) sqlCmd = sqlCmd + " and SAFT_Desc like '" + argEn.Description + "'";
            //if (argEn.Status == true) sqlCmd = sqlCmd + " and SAFT_Status =1";
            if (argEn.Status == true) sqlCmd = sqlCmd + " and SAFT_Status = true";
            //if (argEn.Status == false) sqlCmd = sqlCmd + " and SAFT_Status =0";
            if (argEn.Status == false) sqlCmd = sqlCmd + " and SAFT_Status = false";
            sqlCmd = sqlCmd + " order by SAFT_Code";


            try
            {
                if (!FormHelp.IsBlank(sqlCmd))
                {
                    using (IDataReader loReader = _DatabaseFactory.ExecuteReader(Helper.GetDataBaseType,
                        DataBaseConnectionString, sqlCmd).CreateDataReader())
                    {
                        FeeChargesDAL loFeecharges = new FeeChargesDAL();
                        FeeChargesEn loFeeChrageEn = new FeeChargesEn();
                        while (loReader.Read())
                        {
                            FeeTypesEn loItem = LoadObject(loReader);
                            loFeeChrageEn.FTCode = argEn.FeeTypeCode;
                            loItem.ListFeeCharges = loFeecharges.GetFeeCharges(loFeeChrageEn);
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

        #region GetKokoList

        /// <summary>
        /// Method to Get List of Active or Inactive FeeTypes
        /// </summary>
        /// <param name="argEn">FeeTypes Entity is an Input.FeeTypeCode,FeeType,Hostel,Description,GLCode,Priority and Status are Input Properties.</param>
        /// <returns>Returns List of FeeTypes</returns>
        public List<FeeTypesEn> GetKokoList(FeeTypesEn argEn)
        {
            List<FeeTypesEn> loEnList = new List<FeeTypesEn>();
            argEn.FeeTypeCode = argEn.FeeTypeCode.Replace("*", "%");
            argEn.Description = argEn.Description.Replace("*", "%");
            argEn.GLCode = argEn.GLCode.Replace("*", "%");

            string sqlCmd = "SELECT * FROM SAS_Kokorikulum WHERE SAKO_Code <> '0'  and sako_status = '" + argEn.Status+ "'";
            if (argEn.FeeTypeCode.Length != 0) sqlCmd = sqlCmd + " and SAKO_Code like '" + argEn.FeeTypeCode + "'";
            if (argEn.Description.Length != 0) sqlCmd = sqlCmd + " and SAKO_Description like '" + argEn.Description + "'";
            if (argEn.GLCode.Length != 0) sqlCmd = sqlCmd + " and SAKO_GLCode like '" + argEn.GLCode + "'";
            if (argEn.CreditHours != 0) sqlCmd = sqlCmd + " and SAKO_CreditHours like '" + argEn.CreditHours + "'";
            sqlCmd = sqlCmd +  " order by SAKO_Code";


            try
            {
                if (!FormHelp.IsBlank(sqlCmd))
                {
                    FeeChargesDAL loDs = new FeeChargesDAL();
                    FeeChargesEn loEn = new FeeChargesEn();
                    using (IDataReader loReader = _DatabaseFactory.ExecuteReader(Helper.GetDataBaseType,
                        DataBaseConnectionString, sqlCmd).CreateDataReader())
                    {
                        while (loReader.Read())
                        {
                            FeeTypesEn loItem = LoadObjectKoko(loReader);
                            loEn.FTCode = loItem.FeeTypeCode;
                            loItem.ListFeeCharges = loDs.GetKokoCharges(loEn);
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

        #region GetKokoListddl

        /// <summary>
        /// Method to Get List of Active or Inactive FeeTypes
        /// </summary>
        /// <param name="argEn">FeeTypes Entity is an Input.FeeTypeCode,FeeType,Hostel,Description,GLCode,Priority and Status are Input Properties.</param>
        /// <returns>Returns List of FeeTypes</returns>
        public List<FeeTypesEn> GetKokoListddl(FeeTypesEn argEn)
        {
            List<FeeTypesEn> loEnList = new List<FeeTypesEn>();

            string sqlCmd = "SELECT * FROM SAS_Kokorikulum WHERE SAKO_Code <> '0'";
            sqlCmd = sqlCmd + " order by SAKO_Code";

            try
            {
                if (!FormHelp.IsBlank(sqlCmd))
                {
                    using (IDataReader loReader = _DatabaseFactory.ExecuteReader(Helper.GetDataBaseType,
                        DataBaseConnectionString, sqlCmd).CreateDataReader())
                    {
                        while (loReader.Read())
                        {
                            FeeTypesEn loItem = LoadObjectKoko(loReader);
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

        #region GetFeeTypesList

        /// <summary>
        /// Method to Get List of Active or Inactive FeeTypes
        /// </summary>
        /// <param name="argEn">FeeTypes Entity is an Input.FeeTypeCode,FeeType,Hostel,Description,GLCode,Priority and Status are Input Properties.</param>
        /// <returns>Returns List of FeeTypes</returns>
        public List<FeeTypesEn> GetFeeTypesList(FeeTypesEn argEn)
        {
            //varaible declare
            string sqlCmd = null;

            List<FeeTypesEn> loEnList = new List<FeeTypesEn>();
            argEn.FeeTypeCode = argEn.FeeTypeCode.Replace("*", "%");
            argEn.FeeType = argEn.FeeType.Replace("*", "%");
            argEn.Hostel = argEn.Hostel.Replace("*", "%");
            argEn.Description = argEn.Description.Replace("*", "%");
            //argEn.GLCode = argEn.GLCode.Replace("*", "%");

            sqlCmd = "SELECT * FROM SAS_FeeTypes WHERE SAS_FeeTypes.SAFT_Code <> '0' ";

            if (argEn.FeeTypeCode.Length != 0) sqlCmd = sqlCmd + " and SAS_FeeTypes.SAFT_Code like '" + argEn.FeeTypeCode + "'";
            if (argEn.FeeType.Length != 0) sqlCmd = sqlCmd + " and SAS_FeeTypes.SAFT_FeeType like '" + argEn.FeeType + "'";
            if (argEn.Hostel.Length != 0) sqlCmd = sqlCmd + " and SAS_FeeTypes.SAFT_Hostel like '" + argEn.Hostel + "'";
            if (argEn.Description.Length != 0) sqlCmd = sqlCmd + " and SAS_FeeTypes.SAFT_Desc like '" + argEn.Description + "'";
            //if (argEn.GLCode.Length != 0) sqlCmd = sqlCmd + " and SAS_FeeTypes.SAFT_GLCode like '" + argEn.GLCode + "'";
            //if (argEn.Status == true) sqlCmd = sqlCmd + " and SAS_FeeTypes.SAFT_Status =1";
            if (argEn.Status == true) sqlCmd = sqlCmd + " and SAS_FeeTypes.SAFT_Status = true";
            //if (argEn.Status == false) sqlCmd = sqlCmd + " and SAS_FeeTypes.SAFT_Status =0";
            if (argEn.Status == false) sqlCmd = sqlCmd + " and SAS_FeeTypes.SAFT_Status = false";
            if (argEn.Priority != 0) sqlCmd = sqlCmd + " and SAS_FeeTypes.SAFT_Priority=" + argEn.Priority;
            if (argEn.IsTutionFee != 0) sqlCmd = sqlCmd + " and SAS_FeeTypes.SAFT_IsTutionFee=" + argEn.IsTutionFee;
            if (argEn.IsChangeProgram != 0) sqlCmd = sqlCmd + " and SAS_FeeTypes.saft_programchange=" + argEn.IsChangeProgram;
            
            sqlCmd = sqlCmd + " order by SAS_FeeTypes.SAFT_Code";
          
            try
            {
                if (!FormHelp.IsBlank(sqlCmd))
                {
                    FeeChargesDAL loDs = new FeeChargesDAL();
                    FeeChargesEn loEn = new FeeChargesEn();
                    FacultyGLAccEn faEn = new FacultyGLAccEn();
                    KolejGLAccEn koEn = new KolejGLAccEn();

                    using (IDataReader loReader = _DatabaseFactory.ExecuteReader(Helper.GetDataBaseType,
                        DataBaseConnectionString, sqlCmd).CreateDataReader())
                    {
                        while (loReader.Read())
                        {
                            FeeTypesEn loItem = LoadObject(loReader);
                            loEn.FTCode = loItem.FeeTypeCode;
                            faEn.SAFT_Code = loItem.FeeTypeCode;
                            koEn.SAFT_Code = loItem.FeeTypeCode;
                            loItem.ListFeeCharges = loDs.GetFeeCharges(loEn);
                            loItem.LstFacultyGL = GetFacultyGLAccount(faEn);
                            loItem.LstKolejGL = GetKolejGLAccount(koEn);
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

        #region GetStudentFee

        /// <summary>
        /// Method to Get List of Student FeeTypes
        /// </summary>
        /// <param name="argEn">FeeTypes Entity is an Input.FeeTypeCode,FeeType,Hostel,Description and Status are Input Properties.</param>
        /// <returns>Returns List of FeeTypes</returns>
        public List<FeeTypesEn> GetStudentFee(FeeTypesEn argEn)
        {
            List<FeeTypesEn> loEnList = new List<FeeTypesEn>();
            argEn.FeeTypeCode = argEn.FeeTypeCode.Replace("*", "%");
            argEn.FeeType = argEn.FeeType.Replace("*", "%");
            argEn.Hostel = argEn.Hostel.Replace("*", "%");
            argEn.Description = argEn.Description.Replace("*", "%");
            string sqlCmd = "SELECT SAS_FeeTypes.SAFT_Code,SAS_FeeTypes.saft_taxmode, SAS_FeeTypes.SAFT_Desc,SAS_FeeTypes.SAFT_Priority, SAS_FeeCharges.SASC_Code, SAS_FeeCharges.SAFS_Amount,SAS_FeeCharges.safs_gstamout " +
                 "FROM SAS_FeeCharges INNER JOIN SAS_FeeTypes ON SAS_FeeCharges.SAFT_Code = SAS_FeeTypes.SAFT_Code " +
                 "WHERE";
            if (argEn.SCCode != "-1")
            {
                sqlCmd = sqlCmd + " SAS_FeeCharges.SASC_Code = '" + argEn.SCCode + "'";
            }
            else
            {
                sqlCmd = sqlCmd + " SAS_FeeCharges.SASC_Code != '" + argEn.SCCode + "'";
            }

            if (argEn.FeeTypeCode.Length != 0) sqlCmd = sqlCmd + " and SAS_FeeTypes.SAFT_Code like '" + argEn.FeeTypeCode + "'";
            if (argEn.FeeType.Length != 0) sqlCmd = sqlCmd + " and SAS_FeeTypes.SAFT_FeeType like '" + argEn.FeeType + "'";
            if (argEn.Hostel.Length != 0) sqlCmd = sqlCmd + " and SAS_FeeTypes.SAFT_Hostel like '" + argEn.Hostel + "'";
            if (argEn.Description.Length != 0) sqlCmd = sqlCmd + " and SAS_FeeTypes.SAFT_Desc like '" + argEn.Description + "'";
            //if (argEn.Status == true) sqlCmd = sqlCmd + " and SAS_FeeTypes.SAFT_Status =1";
            if (argEn.Status == true) sqlCmd = sqlCmd + " and SAS_FeeTypes.SAFT_Status = true";
            //if (argEn.Status == false) sqlCmd = sqlCmd + " and SAS_FeeTypes.SAFT_Status =0";
            if (argEn.Status == false) sqlCmd = sqlCmd + " and SAS_FeeTypes.SAFT_Status = false";


            try
            {
                if (!FormHelp.IsBlank(sqlCmd))
                {
                    //_DatabaseFactory.AddInParameter(ref cmd, "@SASC_Code", DbType.String, SCCode);
                    using (IDataReader loReader = _DatabaseFactory.ExecuteReader(Helper.GetDataBaseType,
                        DataBaseConnectionString, sqlCmd).CreateDataReader())
                    {
                        while (loReader.Read())
                        {
                            FeeTypesEn loItem = new FeeTypesEn();
                            loItem.FeeTypeCode = GetValue<string>(loReader, "SAFT_Code");
                            loItem.Description = GetValue<string>(loReader, "SAFT_Desc");
                            loItem.Priority = GetValue<int>(loReader, "SAFT_Priority");
                            loItem.SCCode = GetValue<string>(loReader, "SASC_Code");
                            loItem.FSAmount = GetValue<double>(loReader, "SAFS_Amount");
                            loItem.GSTAmount = GetValue<double>(loReader, "safs_gstamout");
                            loItem.TaxId = GetValue<int>(loReader, "saft_taxmode");                            
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
        /// Method to Get FeeTypes Entity
        /// </summary>
        /// <param name="argEn">FeeTypes Entity is an Input.FeeTypeCode ia an Input Property</param>
        /// <returns>Returns FeeTypes Entity</returns>
        public FeeTypesEn GetItem(FeeTypesEn argEn)
        {
            FeeTypesEn loItem = new FeeTypesEn();
            string sqlCmd = "Select * FROM SAS_FeeTypes WHERE SAFT_Code = @SAFT_Code";

            try
            {
                if (!FormHelp.IsBlank(sqlCmd))
                {
                    DbCommand cmd = _DatabaseFactory.GetDbCommand(Helper.GetDataBaseType, sqlCmd, DataBaseConnectionString);
                    _DatabaseFactory.AddInParameter(ref cmd, "@SAFT_Code", DbType.String, argEn.FeeTypeCode);
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
        /// Method to Insert FeeTypes 
        /// </summary>
        /// <param name="argEn">FeeTypes Entity is an Input.</param>
        /// <returns>Returns Boolean</returns>
        public bool Insert(FeeTypesEn argEn)
        {
            bool lbRes = false;
            int iOut = 0;
            string sqlCmd = "Select count(*) as cnt From SAS_FeeTypes WHERE SAFT_Code = @SAFT_Code or SAFT_Desc= @SAFT_Desc";
            try
            {
                if (!FormHelp.IsBlank(sqlCmd))
                {
                    DbCommand cmdSel = _DatabaseFactory.GetDbCommand(Helper.GetDataBaseType, sqlCmd, DataBaseConnectionString);
                    _DatabaseFactory.AddInParameter(ref cmdSel, "@SAFT_Code", DbType.String, argEn.FeeTypeCode);
                    _DatabaseFactory.AddInParameter(ref cmdSel, "@SAFT_Desc", DbType.String, argEn.Description);
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
                        sqlCmd = "INSERT INTO SAS_FeeTypes(SAFT_Code,SAFT_Desc,SAFT_FeeType,SAFT_Hostel,SAFT_Priority,SAFT_Remarks,SAFT_GLCode,SAFT_Status,SAFT_UpdatedBy,SAFT_UpdatedDtTm,SAFT_IsTutionFee,saft_taxmode,saft_programchange)";
                        sqlCmd += "VALUES (@SAFT_Code,@SAFT_Desc,@SAFT_FeeType,@SAFT_Hostel,@SAFT_Priority,@SAFT_Remarks,@SAFT_GLCode,@SAFT_Status,@SAFT_UpdatedBy,@SAFT_UpdatedDtTm,@SAFT_IsTutionFee,@SAFT_TaxMode,@saft_programchange) ";

                        if (!FormHelp.IsBlank(sqlCmd))
                        {
                            DbCommand cmd = _DatabaseFactory.GetDbCommand(Helper.GetDataBaseType, sqlCmd, DataBaseConnectionString);
                            _DatabaseFactory.AddInParameter(ref cmd, "@SAFT_Code", DbType.String, clsGeneric.NullToString(argEn.FeeTypeCode));
                            _DatabaseFactory.AddInParameter(ref cmd, "@SAFT_Desc", DbType.String, clsGeneric.NullToString(argEn.Description));
                            _DatabaseFactory.AddInParameter(ref cmd, "@SAFT_FeeType", DbType.String, clsGeneric.NullToString(argEn.FeeType));
                            _DatabaseFactory.AddInParameter(ref cmd, "@SAFT_Hostel", DbType.String, clsGeneric.NullToString(argEn.Hostel));
                            _DatabaseFactory.AddInParameter(ref cmd, "@SAFT_Priority", DbType.Int32, clsGeneric.NullToInteger(argEn.Priority));
                            _DatabaseFactory.AddInParameter(ref cmd, "@SAFT_Remarks", DbType.String, clsGeneric.NullToString(argEn.Remarks));
                            _DatabaseFactory.AddInParameter(ref cmd, "@SAFT_GLCode", DbType.String, clsGeneric.NullToString(argEn.GLCode));
                            _DatabaseFactory.AddInParameter(ref cmd, "@SAFT_Status", DbType.Boolean, clsGeneric.NullToBoolean(argEn.Status));
                            _DatabaseFactory.AddInParameter(ref cmd, "@SAFT_UpdatedBy", DbType.String, clsGeneric.NullToString(argEn.UpdatedBy));
                            _DatabaseFactory.AddInParameter(ref cmd, "@SAFT_UpdatedDtTm", DbType.String, clsGeneric.NullToString(DateTime.Now.ToString("dd/MM/yyyy")));
                            // _DatabaseFactory.AddInParameter(ref cmd, "@SAFT_UpdatedDtTm", DbType.String, argEn.UpdatedDtTm);
                            _DatabaseFactory.AddInParameter(ref cmd, "@SAFT_IsTutionFee", DbType.String, clsGeneric.NullToString(argEn.IsTutionFee));
                            _DatabaseFactory.AddInParameter(ref cmd, "@SAFT_TaxMode", DbType.Int32, clsGeneric.NullToInteger(argEn.TaxId));
                            _DatabaseFactory.AddInParameter(ref cmd, "@saft_programchange", DbType.String, clsGeneric.NullToString(argEn.IsChangeProgram));
                            _DbParameterCollection = cmd.Parameters;

                            int liRowAffected = _DatabaseFactory.ExecuteNonQuery(Helper.GetDataBaseType, cmd,
                                DataBaseConnectionString, sqlCmd, _DbParameterCollection);

                            if (liRowAffected > -1)
                                lbRes = true;
                            else
                                throw new Exception("Insertion Failed! No Row has been updated...");
                            //Inserting FeeCharges
                            if (argEn.ListFeeCharges != null)
                            {
                                if (argEn.ListFeeCharges.Count != 0)
                                {
                                    FeeChargesDAL loDS = new FeeChargesDAL();
                                    for (int i = 0; i < argEn.ListFeeCharges.Count; i++)
                                    {
                                        loDS.Insert(argEn.ListFeeCharges[i]);
                                    }
                                }
                            }
                            //Inserting Faculty With GL Account
                            if (argEn.LstFacultyGL != null)
                            {
                                if (argEn.LstFacultyGL.Count != 0)
                                {
                                    for (int i = 0; i < argEn.LstFacultyGL.Count; i++)
                                    {
                                        InsertIntoFacultyGLAccount(argEn.LstFacultyGL[i]);
                                    }
                                }
                            }

                            //Inserting Kolej With GL Account
                            if (argEn.LstKolejGL != null)
                            {
                                if (argEn.LstKolejGL.Count != 0)
                                {
                                    for (int i = 0; i < argEn.LstKolejGL.Count; i++)
                                    {
                                        InsertIntoKolejGLAccount(argEn.LstKolejGL[i]);
                                    }
                                }
                            }
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

        #region InsertKoko

        /// <summary>
        /// Method to Insert FeeTypes 
        /// </summary>
        /// <param name="argEn">FeeTypes Entity is an Input.</param>
        /// <returns>Returns Boolean</returns>
        
        //Updated by Farid 6/4/2016
        public bool InsertKoko(FeeTypesEn argEn)
        {
            bool lbRes = false;
            int iOut = 0;
            //string sqlCmd = "Select count(*) as cnt From SAS_Kokorikulum WHERE SAKO_Code = @SAKO_Code";
            string sqlCmd1 = "Select count(*) as cnt From SAS_Kokorikulum WHERE SAKO_Code = @SAKO_Code";
            try
            {
                if (!FormHelp.IsBlank(sqlCmd1))
                {
                    DbCommand cmdSel = _DatabaseFactory.GetDbCommand(Helper.GetDataBaseType, sqlCmd1, DataBaseConnectionString);
                    _DatabaseFactory.AddInParameter(ref cmdSel, "@SAKO_Code", DbType.String, argEn.FeeTypeCode);
                    _DbParameterCollection = cmdSel.Parameters;

                    using (IDataReader dr = _DatabaseFactory.GetIDataReader(Helper.GetDataBaseType, cmdSel,
                        DataBaseConnectionString, sqlCmd1, _DbParameterCollection).CreateDataReader())
                    {
                        if (dr.Read())
                            iOut = clsGeneric.NullToInteger(dr["cnt"]);
                        if (iOut > 0)
                            throw new Exception("Record Already Exist");
                    }
                    if (iOut == 0)
                    {
                        //string sqlCmd;
                        sqlCmd1 = "INSERT INTO SAS_Kokorikulum(SAKO_Code,SAKO_Description,SAKO_GLCode,SAKO_Status,SAKO_CreditHours,safd_type)" +
                         " VALUES (@SAKO_Code,@SAKO_Description,@SAKO_GLCode,@SAKO_Status,@SAKO_CreditHours,@saft_hostel) ";


                        if (!FormHelp.IsBlank(sqlCmd1))
                        {
                            DbCommand cmd = _DatabaseFactory.GetDbCommand(Helper.GetDataBaseType, sqlCmd1, DataBaseConnectionString);
                            _DatabaseFactory.AddInParameter(ref cmd, "@SAKO_Code", DbType.String, argEn.FeeTypeCode);
                            _DatabaseFactory.AddInParameter(ref cmd, "@SAKO_Description", DbType.String, argEn.Description);
                            _DatabaseFactory.AddInParameter(ref cmd, "@SAKO_GLCode", DbType.String, argEn.GLCode);
                            _DatabaseFactory.AddInParameter(ref cmd, "@SAKO_Status", DbType.Boolean, argEn.Status);
                            _DatabaseFactory.AddInParameter(ref cmd, "@SAKO_CreditHours", DbType.Int32, argEn.CreditHours);
                            //_DatabaseFactory.AddInParameter(ref cmd, "@TaxID", DbType.Int32, argEn.TaxId);
                            _DatabaseFactory.AddInParameter(ref cmd, "@saft_hostel", DbType.String, argEn.SCCode);
                            _DbParameterCollection = cmd.Parameters;

                            int liRowAffected = _DatabaseFactory.ExecuteNonQuery(Helper.GetDataBaseType, cmd,
                                DataBaseConnectionString, sqlCmd1, _DbParameterCollection);

                            if (liRowAffected > -1)
                                lbRes = true;
                            else
                                throw new Exception("Insertion Failed! No Row has been updated...");
                            //Inserting FeeCharges
                            if (argEn.ListKokoCharges != null)
                            {
                                if (argEn.ListKokoCharges.Count != 0)
                                {
                                    FeeChargesDAL loDS = new FeeChargesDAL();
                                    for (int i = 0; i < argEn.ListKokoCharges.Count; i++)
                                    {
                                        loDS.UpdateKo(argEn.ListKokoCharges[i]);
                                    }
                                }
                            }
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
        /// Method to Updates FeeTypes 
        /// </summary>
        /// <param name="argEn">FeeTypes Entity is an Input.</param>
        /// <returns>Returns Boolean</returns>
        public bool Update(FeeTypesEn argEn)
        {
            bool lbRes = false;
            int iOut = 0;
            string sqlCmd = "Select count(*) as cnt From SAS_FeeTypes WHERE SAFT_Code != @SAFT_Code and SAFT_Desc = @SAFT_Desc";
            try
            {
                if (!FormHelp.IsBlank(sqlCmd))
                {
                    DbCommand cmdSel = _DatabaseFactory.GetDbCommand(Helper.GetDataBaseType, sqlCmd, DataBaseConnectionString);
                    _DatabaseFactory.AddInParameter(ref cmdSel, "@SAFT_Code", DbType.String, argEn.FeeTypeCode);
                    _DatabaseFactory.AddInParameter(ref cmdSel, "@SAFT_Desc", DbType.String, argEn.Description);
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
                        sqlCmd = "UPDATE SAS_FeeTypes SET SAFT_Code = @SAFT_Code, SAFT_Desc = @SAFT_Desc, SAFT_FeeType = @SAFT_FeeType, SAFT_Hostel = @SAFT_Hostel,";
                        sqlCmd+="SAFT_Priority = @SAFT_Priority, SAFT_Remarks = @SAFT_Remarks, SAFT_GLCode = @SAFT_GLCode, SAFT_Status = @SAFT_Status, SAFT_UpdatedBy = @SAFT_UpdatedBy,";
                        sqlCmd += "SAFT_UpdatedDtTm = @SAFT_UpdatedDtTm,SAFT_IsTutionFee=@SAFT_IsTutionFee,SAFT_TaxMode=@SAFT_TaxMode,saft_programchange=@saft_programchange WHERE SAFT_Code = @SAFT_Code";

                        if (!FormHelp.IsBlank(sqlCmd))
                        {
                            DbCommand cmd = _DatabaseFactory.GetDbCommand(Helper.GetDataBaseType, sqlCmd, DataBaseConnectionString);
                            _DatabaseFactory.AddInParameter(ref cmd, "@SAFT_Code", DbType.String, argEn.FeeTypeCode);
                            _DatabaseFactory.AddInParameter(ref cmd, "@SAFT_Desc", DbType.String, argEn.Description);
                            _DatabaseFactory.AddInParameter(ref cmd, "@SAFT_FeeType", DbType.String, argEn.FeeType);
                            _DatabaseFactory.AddInParameter(ref cmd, "@SAFT_Hostel", DbType.String, argEn.Hostel);
                            _DatabaseFactory.AddInParameter(ref cmd, "@SAFT_Priority", DbType.Int32, argEn.Priority);
                            _DatabaseFactory.AddInParameter(ref cmd, "@SAFT_Remarks", DbType.String, argEn.Remarks);
                            _DatabaseFactory.AddInParameter(ref cmd, "@SAFT_GLCode", DbType.String, argEn.GLCode);
                            _DatabaseFactory.AddInParameter(ref cmd, "@SAFT_Status", DbType.Boolean, argEn.Status);
                            _DatabaseFactory.AddInParameter(ref cmd, "@SAFT_UpdatedBy", DbType.String, argEn.UpdatedBy);
                            _DatabaseFactory.AddInParameter(ref cmd, "@SAFT_UpdatedDtTm", DbType.String, argEn.UpdatedDtTm);
                            _DatabaseFactory.AddInParameter(ref cmd, "@SAFT_IsTutionFee", DbType.String, argEn.IsTutionFee);
                            _DatabaseFactory.AddInParameter(ref cmd, "@SAFT_TaxMode", DbType.Int32, clsGeneric.NullToInteger(argEn.TaxId));
                            _DatabaseFactory.AddInParameter(ref cmd, "@saft_programchange", DbType.String, argEn.IsChangeProgram);
                            _DbParameterCollection = cmd.Parameters;

                            int liRowAffected = _DatabaseFactory.ExecuteNonQuery(Helper.GetDataBaseType, cmd,
                                DataBaseConnectionString, sqlCmd, _DbParameterCollection);

                            if (liRowAffected > -1)
                                lbRes = true;
                            else
                                throw new Exception("Update Failed! No Row has been updated...");
                            
                            //updating feecharges
                            if (argEn.ListFeeCharges != null)
                            {
                                if (argEn.ListFeeCharges.Count != 0)
                                {
                                    FeeChargesDAL loDS = new FeeChargesDAL();
                                    for (int i = 0; i < argEn.ListFeeCharges.Count; i++)
                                    {
                                        loDS.Update(argEn.ListFeeCharges[i]);
                                    }
                                }
                            }

                            //Inserting Faculty With GL Account
                            if (argEn.LstFacultyGL != null)
                            {
                                if (argEn.LstFacultyGL.Count != 0)
                                {
                                    for (int i = 0; i < argEn.LstFacultyGL.Count; i++)
                                    {
                                        InsertIntoFacultyGLAccount(argEn.LstFacultyGL[i]);
                                    }
                                }
                            }

                            //Inserting Kolej With GL Account
                            if (argEn.LstKolejGL != null)
                            {
                                if (argEn.LstKolejGL.Count != 0)
                                {
                                    for (int i = 0; i < argEn.LstKolejGL.Count; i++)
                                    {
                                        InsertIntoKolejGLAccount(argEn.LstKolejGL[i]);
                                    }
                                }
                            }
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

        #region UpdateKokoList

        /// <summary>
        /// Method to Updates FeeTypes 
        /// </summary>
        /// <param name="argEn">FeeTypes Entity is an Input.</param>
        /// <returns>Returns Boolean</returns>
        //public bool UpdateKokoList(FeeTypesEn argEn)
        //{
        //    bool lbRes = false;
        //    int iOut = 0;
        //    string sqlCmd = "Select count(*) as cnt From SAS_Kokorikulum WHERE SAKO_Code = @SAKO_Code and SAKO_Description= @SAKO_Description and SAKO_GLCode = @SAKO_GLCode and SAKO_Status = @SAKO_Status and SAKO_CreditHours = @SAKO_CreditHours";
        //    try
        //    {
        //        if (!FormHelp.IsBlank(sqlCmd))
        //        {
        //            DbCommand cmdSel = _DatabaseFactory.GetDbCommand(Helper.GetDataBaseType, sqlCmd, DataBaseConnectionString);
        //            _DatabaseFactory.AddInParameter(ref cmdSel, "@SAKO_Code", DbType.String, argEn.FeeTypeCode);
        //            _DatabaseFactory.AddInParameter(ref cmdSel, "@SAKO_Description", DbType.String, argEn.Description);
        //            _DatabaseFactory.AddInParameter(ref cmdSel, "@SAKO_GLCode", DbType.String, argEn.GLCode);
        //            _DatabaseFactory.AddInParameter(ref cmdSel, "@SAKO_Status", DbType.String, argEn.Status);
        //            _DatabaseFactory.AddInParameter(ref cmdSel, "@SAKO_CreditHours", DbType.Int32, argEn.CreditHours);
        //            _DbParameterCollection = cmdSel.Parameters;

        //            using (IDataReader dr = _DatabaseFactory.GetIDataReader(Helper.GetDataBaseType, cmdSel,
        //                DataBaseConnectionString, sqlCmd, _DbParameterCollection).CreateDataReader())
        //            {
        //                if (dr.Read())
        //                    iOut = clsGeneric.NullToInteger(dr["cnt"]);
        //                if (iOut > 0)
        //                    throw new Exception("Record Already Exist");
        //            }
        //            if (iOut == 0)
        //            {
        //                sqlCmd = "UPDATE SAS_Kokorikulum SET SAKO_Description=@SAKO_Description,SAKO_GLCode=@SAKO_GLCode,SAKO_Status=@SAKO_Status,SAKO_CreditHours=@SAKO_CreditHours where SAKO_Code=@SAKO_Code";

        //                if (!FormHelp.IsBlank(sqlCmd))
        //                {
        //                    DbCommand cmd = _DatabaseFactory.GetDbCommand(Helper.GetDataBaseType, sqlCmd, DataBaseConnectionString);
        //                    _DatabaseFactory.AddInParameter(ref cmd, "@SAKO_Code", DbType.String, argEn.FeeTypeCode);
        //                    _DatabaseFactory.AddInParameter(ref cmd, "@SAKO_Description", DbType.String, argEn.Description);
        //                    _DatabaseFactory.AddInParameter(ref cmd, "@SAKO_GLCode", DbType.String, argEn.GLCode);
        //                    _DatabaseFactory.AddInParameter(ref cmd, "@SAKO_Status", DbType.String, argEn.Status);
        //                    _DatabaseFactory.AddInParameter(ref cmd, "@SAKO_CreditHours", DbType.Int32, argEn.CreditHours);
        //                    _DbParameterCollection = cmd.Parameters;

        //                    int liRowAffected = _DatabaseFactory.ExecuteNonQuery(Helper.GetDataBaseType, cmd,
        //                        DataBaseConnectionString, sqlCmd, _DbParameterCollection);

        //                    if (liRowAffected > -1)
        //                        lbRes = true;
        //                    else
        //                        throw new Exception("Update Failed! No Row has been updated...");
        //                    //updating feecharges
        //                    if (argEn.ListFeeCharges != null)
        //                    {
        //                        if (argEn.ListFeeCharges.Count != 0)
        //                        {
        //                            FeeChargesDAL loDS = new FeeChargesDAL();
        //                            for (int i = 0; i < argEn.ListFeeCharges.Count; i++)
        //                            {
        //                                loDS.UpdateKokoDetails(argEn.ListFeeCharges[i]);
        //                            }
        //                        }
        //                    }
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //    return lbRes;
        //}

        public bool UpdateKokoList(FeeTypesEn argEn)
        {
            bool lbRes = false;
            string sqlCmd = "UPDATE SAS_Kokorikulum SET SAKO_Description=@SAKO_Description,SAKO_GLCode=@SAKO_GLCode,SAKO_Status=@SAKO_Status,SAKO_CreditHours=@SAKO_CreditHours,safd_type=@saft_hostel where SAKO_Code=@SAKO_Code";
            try
            {

                if (!FormHelp.IsBlank(sqlCmd))
                {
                    DbCommand cmd = _DatabaseFactory.GetDbCommand(Helper.GetDataBaseType, sqlCmd, DataBaseConnectionString);
                    _DatabaseFactory.AddInParameter(ref cmd, "@SAKO_Code", DbType.String, clsGeneric.NullToString(argEn.FeeTypeCode));
                    _DatabaseFactory.AddInParameter(ref cmd, "@SAKO_Description", DbType.String, clsGeneric.NullToString(argEn.Description));
                    _DatabaseFactory.AddInParameter(ref cmd, "@SAKO_GLCode", DbType.String, clsGeneric.NullToString(argEn.GLCode));
                    _DatabaseFactory.AddInParameter(ref cmd, "@SAKO_Status", DbType.Boolean, clsGeneric.NullToBoolean(argEn.Status));
                    _DatabaseFactory.AddInParameter(ref cmd, "@SAKO_CreditHours", DbType.Int32, clsGeneric.NullToInteger(argEn.CreditHours));
                    _DatabaseFactory.AddInParameter(ref cmd, "@saft_hostel", DbType.String, argEn.SCCode);
                    _DbParameterCollection = cmd.Parameters;

                    int liRowAffected = _DatabaseFactory.ExecuteNonQuery(Helper.GetDataBaseType, cmd,
                        DataBaseConnectionString, sqlCmd, _DbParameterCollection);

                    if (liRowAffected > -1)
                        lbRes = true;
                    else
                        throw new Exception("Update Failed! No Row has been updated...");
                    //updating feecharges
                    if (argEn.ListKokoCharges != null)
                    {

                        if (argEn.ListKokoCharges.Count != 0)
                        {
                            DeleteKokoCharges(argEn);
                            FeeChargesDAL loDS = new FeeChargesDAL();
                            for (int i = 0; i < argEn.ListKokoCharges.Count; i++)
                            {
                                lbRes = loDS.UpdateKo(argEn.ListKokoCharges[i]);
                            }
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

        #region FeeDelete

        /// <summary>
        /// Method to Delete All FeeTypes 
        /// </summary>
        /// <param name="argEn">FeeTypes Entity is an Input.FeeTypeCode ia an Input Property</param>
        /// <returns>Returns Boolean</returns>
        public bool FeeDelete(FeeTypesEn argEn)
        {
            bool lbRes = false;
            int iOut = 0;
            List<FeeChargesEn> lofeechargeslist = new List<FeeChargesEn>();
            string sqlCmd = " SELECT COUNT(*)as cnt FROM SAS_FeeTypes RIGHT OUTER JOIN SAS_HostelStrDetails ON SAS_FeeTypes.SAFT_Code = SAS_HostelStrDetails.SAFT_Code " +
                            " RIGHT OUTER JOIN SAS_SponsorFeeTypes ON SAS_FeeTypes.SAFT_Code = SAS_SponsorFeeTypes.SAFT_Code RIGHT OUTER JOIN SAS_FeeStrDetails " +
                            " ON SAS_FeeTypes.SAFT_Code = SAS_FeeStrDetails.SAFT_Code WHERE  (SAS_FeeTypes.SAFT_Code = @FeeTypeCode) OR " +
                            " (SAS_FeeStrDetails.SAFT_Code =  @FeeTypeCode) OR (SAS_HostelStrDetails.SAFT_Code =  @FeeTypeCode) ";
            try
            {
                if (!FormHelp.IsBlank(sqlCmd))
                {
                    DbCommand cmdSel = _DatabaseFactory.GetDbCommand(Helper.GetDataBaseType, sqlCmd, DataBaseConnectionString);
                    _DatabaseFactory.AddInParameter(ref cmdSel, "@FeeTypeCode", DbType.String, argEn.FeeTypeCode);
                    _DbParameterCollection = cmdSel.Parameters;

                    using (IDataReader dr = _DatabaseFactory.GetIDataReader(Helper.GetDataBaseType, cmdSel,
                        DataBaseConnectionString, sqlCmd, _DbParameterCollection).CreateDataReader())
                    {
                        if (dr.Read())
                            iOut = clsGeneric.NullToInteger(dr["cnt"]);
                        if (iOut > 0)
                            throw new Exception("Record Already In Use");
                    }
                    if (iOut == 0)
                    {
                        string sqlCmd1 = "Select * from SAS_FeeCharges WHERE SAFT_Code = @SAFT_Code";

                        {
                            if (!FormHelp.IsBlank(sqlCmd1))
                            {
                                DbCommand cmd = _DatabaseFactory.GetDbCommand(Helper.GetDataBaseType, sqlCmd1, DataBaseConnectionString);
                                _DatabaseFactory.AddInParameter(ref cmd, "@SAFT_Code", DbType.String, argEn.FeeTypeCode);
                                _DbParameterCollection = cmd.Parameters;

                                using (IDataReader loReader = _DatabaseFactory.GetIDataReader(Helper.GetDataBaseType, cmd,
                                DataBaseConnectionString, sqlCmd1, _DbParameterCollection).CreateDataReader())
                                {
                                    FeeChargesDAL lods = new FeeChargesDAL();
                                    FeeChargesEn loen = new FeeChargesEn();
                                    while (loReader.Read())
                                    {
                                        loen = new FeeChargesEn();
                                        loen.FTCode = GetValue<string>(loReader, "SAFT_Code");
                                        lofeechargeslist.Add(loen);
                                        loen = null;
                                    }
                                    loReader.Close();
                                    int i = 0;
                                    //deleting each item in batch
                                    for (i = 0; i < lofeechargeslist.Count; i++)
                                    {
                                        lods.Delete(lofeechargeslist[i]);
                                    }

                                }

                            }

                        }
                        Delete(argEn);
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
        /// Method to Delete FeeTypes 
        /// </summary>
        /// <param name="argEn">FeeTypes Entity is an Input.</param>
        /// <returns>Returns Boolean</returns>
        public bool Delete(FeeTypesEn argEn)
        {
            bool lbRes = false;

            string sqlCmd = "DELETE FROM SAS_FeeTypes WHERE SAFT_Code = @SAFT_Code";
            try
            {
                if (!FormHelp.IsBlank(sqlCmd))
                {
                    DbCommand cmd = _DatabaseFactory.GetDbCommand(Helper.GetDataBaseType, sqlCmd, DataBaseConnectionString);
                    _DatabaseFactory.AddInParameter(ref cmd, "@SAFT_Code", DbType.String, argEn.FeeTypeCode);
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
        /// Method to Load FeeTypes Entity 
        /// </summary>
        /// <param name="argReader">IDataReader Object is an Input.</param>
        /// <returns>Returns FeeTypes Entity</returns>
        private FeeTypesEn LoadObject(IDataReader argReader)
        {
            FeeTypesEn loItem = new FeeTypesEn();
            loItem.FeeTypeCode = GetValue<string>(argReader, "SAFT_Code");
            loItem.Description = GetValue<string>(argReader, "SAFT_Desc");
            loItem.FeeType = GetValue<string>(argReader, "SAFT_FeeType");
            loItem.Hostel = GetValue<string>(argReader, "SAFT_Hostel");
            loItem.Priority = GetValue<int>(argReader, "SAFT_Priority");
            loItem.Remarks = GetValue<string>(argReader, "SAFT_Remarks");
            loItem.GLCode = GetValue<string>(argReader, "SAFT_GLCode");
            loItem.Status = GetValue<bool>(argReader, "SAFT_Status");
            loItem.UpdatedBy = GetValue<string>(argReader, "SAFT_UpdatedBy");
            loItem.UpdatedDtTm = GetValue<string>(argReader, "SAFT_UpdatedDtTm");
            loItem.IsTutionFee = GetValue<int>(argReader, "SAFT_IsTutionFee");
            loItem.TaxId = GetValue<int>(argReader, "SAFT_TaxMode");
            loItem.IsChangeProgram = GetValue<int>(argReader, "saft_programchange");            
            return loItem;
        }
        /// <summary>
        /// Method to Load FeeTypes Entity 
        /// </summary>
        /// <param name="argReader">IDataReader Object is an Input.</param>
        /// <returns>Returns FeeTypes Entity</returns>
        private FeeTypesEn LoadObjectKoko(IDataReader argReader)
        {
            FeeTypesEn loItem = new FeeTypesEn();
            loItem.FeeTypeCode = GetValue<string>(argReader, "SAKO_Code");
            loItem.Description = GetValue<string>(argReader, "SAKO_Description");
            loItem.GLCode = GetValue<string>(argReader, "SAKO_GLCode");
            loItem.Status = GetValue<bool>(argReader, "SAKO_Status");
            loItem.CreditHours = GetValue<int>(argReader, "SAKO_CreditHours");
            return loItem;
        }
        private static T GetValue<T>(IDataReader argReader, string argColNm)
        {
            if (!argReader.IsDBNull(argReader.GetOrdinal(argColNm)))
                return (T)argReader.GetValue(argReader.GetOrdinal(argColNm));
            else
                return default(T);
        }
        private FeeTypesEn LoadObjectKokoBaru(IDataReader argReader)
        {
            FeeTypesEn loItem = new FeeTypesEn();
            loItem.FeeTypeCode = GetValue<string>(argReader, "SAKO_Code");
            loItem.Description = GetValue<string>(argReader, "SAKO_Description");
            //loItem.GLCode = GetValue<string>(argReader, "SAKO_GLCode");
            loItem.Status = GetValue<bool>(argReader, "SAKO_Status");
            //loItem.CreditHours = GetValue<int>(argReader, "SAKO_CreditHours");
            return loItem;
        }

        #endregion

        #region GetFeeDetails

        public List<FeeTypesEn> GetFeeDetails(FeeTypesEn argEn)
        {
            List<FeeTypesEn> loEnList = new List<FeeTypesEn>();

            string sqlCmd = "SELECT distinct SAS_FeeTypes.saft_code, SAS_FeeTypes.saft_taxmode, " +
                            "SAS_FeeTypes.SAFT_Desc, SAS_FeeTypes.SAFT_Priority, SAS_FeeTypes.SAFT_FeeType";

            if (argEn.SCCode.Length != 0)
            {
                sqlCmd = sqlCmd + ", fee.SASC_Code, fee.SAFS_Amount, fee.safs_gstamout  ";
            }
            else
            {
                sqlCmd = sqlCmd + ", (SELECT max(safs_amount) FROM sas_feecharges WHERE (fee.saft_code = saft_code) " +
                                    "AND (sasc_code IN ('Local','W'))) As Local_Amount " +
                                    ", (SELECT max(safs_amount) FROM sas_feecharges WHERE (fee.saft_code = saft_code) " +
                                    "AND (sasc_code IN ('International','BW'))) As NonLocal_Amount " +
                                    ", (SELECT max(safs_gstamout) FROM sas_feecharges WHERE (fee.saft_code = saft_code) " +
                                    "AND sasc_code IN ('Local','W')) As Local_GSTAmount " +
                                    ", (SELECT max(safs_gstamout) FROM sas_feecharges WHERE (fee.saft_code = saft_code) " +
                                    "AND sasc_code IN ('International','BW')) As NonLocal_GSTAmount ";
                //Added By Jessica - 26/02/2016
                sqlCmd = sqlCmd + @", (SELECT sasc_code FROM sas_feecharges WHERE (fee.saft_code = saft_code) AND sasc_code IN ('Local','W')) As local_category ,
                                    (SELECT sasc_code FROM sas_feecharges WHERE (fee.saft_code = saft_code) AND sasc_code IN ('International','BW')) As nonLocal_category ";

            }

            sqlCmd = sqlCmd + "FROM SAS_FeeCharges fee INNER JOIN SAS_FeeTypes ON fee.SAFT_Code = SAS_FeeTypes.SAFT_Code WHERE ";
            if (argEn.FeeTypeCode.Length != 0)
            {
                sqlCmd = sqlCmd + "SAS_FeeTypes.SAFT_Code like '%" + argEn.FeeTypeCode + "%'";
            }
            else
            {
                sqlCmd = sqlCmd + "SAS_FeeTypes.SAFT_Code like '%%' ";
            }
            if (argEn.SCCode.Length != 0) sqlCmd = sqlCmd + " and fee.SASC_Code like '%" + argEn.SCCode + "%'";
            if (argEn.FeeType.Length != 0) sqlCmd = sqlCmd + " and SAS_FeeTypes.SAFT_FeeType like '%" + argEn.FeeType + "%'";
            if (argEn.Hostel.Length != 0) sqlCmd = sqlCmd + " and SAS_FeeTypes.SAFT_Hostel like '%" + argEn.Hostel + "%'";
            if (argEn.Description.Length != 0) sqlCmd = sqlCmd + " and SAS_FeeTypes.SAFT_Desc like '%" + argEn.Description + "%'";
            if (argEn.Status == true) sqlCmd = sqlCmd + " and SAS_FeeTypes.SAFT_Status = true";
            if (argEn.Status == false) sqlCmd = sqlCmd + " and SAS_FeeTypes.SAFT_Status = false";

            sqlCmd = sqlCmd + " order by SAS_FeeTypes.saft_code";

            try
            {
                if (!FormHelp.IsBlank(sqlCmd))
                {
                    using (IDataReader loReader = _DatabaseFactory.ExecuteReader(Helper.GetDataBaseType,
                        DataBaseConnectionString, sqlCmd).CreateDataReader())
                    {
                        while (loReader.Read())
                        {
                            FeeTypesEn loItem = new FeeTypesEn();
                            loItem.FeeTypeCode = GetValue<string>(loReader, "SAFT_Code");
                            loItem.TaxId = GetValue<int>(loReader, "saft_taxmode");
                            loItem.Description = GetValue<string>(loReader, "SAFT_Desc");
                            loItem.Priority = GetValue<int>(loReader, "SAFT_Priority");
                            loItem.FeeType = GetValue<string>(loReader, "SAFT_FeeType");
                            if (argEn.SCCode.Length != 0)
                            {
                                loItem.SCCode = GetValue<string>(loReader, "SASC_Code");
                                loItem.FSAmount = GetValue<double>(loReader, "SAFS_Amount");
                                loItem.GSTAmount = GetValue<double>(loReader, "safs_gstamout");
                                loItem.LocalAmount = 0.00;
                                loItem.NonLocalAmount = 0.00;
                                loItem.LocalGSTAmount = 0.00;
                                loItem.NonLocalGSTAmount = 0.00;
                                loItem.LocalCategory = string.Empty;
                                loItem.NonLocalCategory = string.Empty;
                            }
                            else
                            {
                                loItem.SCCode = "";
                                loItem.FSAmount = 0.00;
                                loItem.GSTAmount = 0.00;
                                loItem.LocalAmount = GetValue<double>(loReader, "Local_Amount");
                                loItem.NonLocalAmount = GetValue<double>(loReader, "NonLocal_Amount");
                                loItem.LocalGSTAmount = GetValue<double>(loReader, "Local_GSTAmount");
                                loItem.NonLocalGSTAmount = GetValue<double>(loReader, "NonLocal_GSTAmount");
                                loItem.LocalCategory = GetValue<string>(loReader, "local_category");
                                loItem.NonLocalCategory = GetValue<string>(loReader, "nonlocal_category");
                            }
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

        #region DeleteKokorikulum
        /// <summary>
        /// Method to Delete Kokorikulum 
        /// </summary>
        /// <param name="argEn">FeeTypes Entity is an Input.</param>
        /// <returns>Returns Boolean</returns>
        public bool DeleteKoko(FeeTypesEn argEn)
        {
            bool lbRes = false;

            string sqlCmd = "DELETE FROM SAS_Kokorikulum WHERE SAKO_Code = @SAKO_Code";
            try
            {
                if (!FormHelp.IsBlank(sqlCmd))
                {
                    DbCommand cmd = _DatabaseFactory.GetDbCommand(Helper.GetDataBaseType, sqlCmd, DataBaseConnectionString);
                    _DatabaseFactory.AddInParameter(ref cmd, "@SAKO_Code", DbType.String, argEn.FeeTypeCode);
                    _DbParameterCollection = cmd.Parameters;

                    int liRowAffected = _DatabaseFactory.ExecuteNonQuery(Helper.GetDataBaseType, cmd,
                                DataBaseConnectionString, sqlCmd, _DbParameterCollection);

                    if (liRowAffected > -1)
                        lbRes = true;
                    else
                        throw new Exception("Delete Failed! No Row has been deleted...");


                    lbRes = DeleteKokoCharges(argEn);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return lbRes;
        }

        #endregion

        #region DeleteKokoCharges

        /// <summary>
        /// Method to Delete KokoCharges
        /// </summary>
        /// <param name="argEn">KokoCharges Entity is an Input.</param>
        /// <returns>Returns Boolean</returns>
        //public bool DeleteKokoCharges(FeeTypesEn argEn)
        //{
        //    bool lbRes = false;

        //    string sqlCmd = "DELETE FROM SAS_KokorikulumDetails WHERE SAKO_Code = @SAKO_Code";
        //    try
        //    {
        //        if (!FormHelp.IsBlank(sqlCmd))
        //        {
        //            DbCommand cmd = _DatabaseFactory.GetDbCommand(Helper.GetDataBaseType, sqlCmd, DataBaseConnectionString);
        //            _DatabaseFactory.AddInParameter(ref cmd, "@SAKO_Code", DbType.String, argEn.FeeTypeCode);
        //            _DbParameterCollection = cmd.Parameters;

        //            int liRowAffected = _DatabaseFactory.ExecuteNonQuery(Helper.GetDataBaseType, cmd,
        //                        DataBaseConnectionString, sqlCmd, _DbParameterCollection);

        //            if (liRowAffected > -1)
        //                lbRes = true;
        //            else
        //                throw new Exception("Delete Failed! No Row has been deleted...");
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //    return lbRes;
        //}

        //Updated by Farid 6/4/2016
        public bool DeleteKokoCharges(FeeTypesEn argEn)
        {
            bool lbRes = false;

            string sqlCmd = "DELETE FROM SAS_KokorikulumDetails WHERE SAKO_Code = @SAKO_Code";
            try
            {
                if (!FormHelp.IsBlank(sqlCmd))
                {
                    DbCommand cmd = _DatabaseFactory.GetDbCommand(Helper.GetDataBaseType, sqlCmd, DataBaseConnectionString);
                    _DatabaseFactory.AddInParameter(ref cmd, "@SAKO_Code", DbType.String, argEn.FeeTypeCode);
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

        #region GetKokokurikulumList

        /// <summary>
        /// Method to Get List of Active or Inactive FeeTypes
        /// </summary>
        /// <param name="argEn">FeeTypes Entity is an Input.FeeTypeCode,FeeType,Hostel,Description,GLCode,Priority and Status are Input Properties.</param>
        /// <returns>Returns List of FeeTypes</returns>
        public List<FeeTypesEn> GetKokokurikulumList(FeeTypesEn argEn)
        {
            List<FeeTypesEn> loEnList = new List<FeeTypesEn>();
            argEn.FeeTypeCode = argEn.FeeTypeCode.Replace("*", "%");
            //argEn.Description = argEn.Description.Replace("*", "%");
            //argEn.GLCode = argEn.GLCode.Replace("*", "%");

            //string sqlCmd = "SELECT * FROM SAS_Kokorikulum WHERE SAKO_Code <> '0'";
            string sqlCmd = "SELECT kk.sako_status,ko.sako_code,ko.sako_description FROM SAS_Kokorikulum kk inner join SAS_Kolej ko on ko.sako_code = kk.sako_code where kk.sako_code <> '0'";
            if (argEn.FeeTypeCode.Length != 0) sqlCmd = sqlCmd + " and kk.SAKO_Code like '" + argEn.FeeTypeCode + "'";
            //if (argEn.Description.Length != 0) sqlCmd = sqlCmd + " and SAKO_Description like '" + argEn.Description + "'";
            //if (argEn.GLCode.Length != 0) sqlCmd = sqlCmd + " and SAKO_GLCode like '" + argEn.GLCode + "'";
            //if (argEn.CreditHours != 0) sqlCmd = sqlCmd + " and SAKO_CreditHours like '" + argEn.CreditHours + "'";
            //if (argEn.Status == true) sqlCmd = sqlCmd + " and SAFS_Status = 1";
            if (argEn.Status == true) sqlCmd = sqlCmd + " and kk.sako_status = true";
            //if (argEn.Status == false) sqlCmd = sqlCmd + " and SAFS_Status = 0 ";
            if (argEn.Status == false) sqlCmd = sqlCmd + " and kk.sako_status = false ";

            sqlCmd = sqlCmd + " order by kk.SAKO_Code";


            try
            {
                if (!FormHelp.IsBlank(sqlCmd))
                {
                    FeeChargesDAL loDs = new FeeChargesDAL();
                    FeeChargesEn loEn = new FeeChargesEn();
                    using (IDataReader loReader = _DatabaseFactory.ExecuteReader(Helper.GetDataBaseType,
                        DataBaseConnectionString, sqlCmd).CreateDataReader())
                    {
                        while (loReader.Read())
                        {
                            FeeTypesEn loItem = LoadObjectKokoBaru(loReader);
                            loEn.FTCode = loItem.FeeTypeCode;
                            loItem.ListKokoCharges = loDs.Getkokobaru(loEn);
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

        #region GetKokoStudent

        /// <summary>
        /// Method to Get List of Active or Inactive FeeTypes
        /// </summary>
        /// <param name="argEn">FeeTypes Entity is an Input.FeeTypeCode,FeeType,Hostel,Description,GLCode,Priority and Status are Input Properties.</param>
        /// <returns>Returns List of FeeTypes</returns>
        public List<KokoEn> GetKokoStudent(FeeTypesEn argEn)
        {
            List<KokoEn> loEnList = new List<KokoEn>();


            string sqlCmd = "select * from sas_kokorikulum kk inner join sas_student ss on ss.sasi_kokocode = kk.sako_code where kk.sako_code = '" + argEn.FeeTypeCode + "'";

            try
            {
                if (!FormHelp.IsBlank(sqlCmd))
                {
                    //FeeChargesDAL loDs = new FeeChargesDAL();
                    //FeeChargesEn loEn = new FeeChargesEn();
                    using (IDataReader loReader = _DatabaseFactory.ExecuteReader(Helper.GetDataBaseType,
                        DataBaseConnectionString, sqlCmd).CreateDataReader())
                    {
                        while (loReader.Read())
                        {
                            KokoEn loItem = new KokoEn();
                            loItem.Code = GetValue<string>(loReader, "sako_code");
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

        //Faculty GL - START
        #region InsertIntoFacultyGLAccount

        public bool InsertIntoFacultyGLAccount(FacultyGLAccEn argEn)
        {
            bool result = false;
            int cnt = 0;

            string sqlCmd = "SELECT COUNT(*) AS cnt FROM sas_faculty_glaccount WHERE SAFT_Code = '" + argEn.SAFT_Code + "' " +
                            "AND SAFC_Code = '" + argEn.SAFC_Code + "' ";
            try
            {
                if (!FormHelp.IsBlank(sqlCmd))
                {
                    using (IDataReader dr = _DatabaseFactory.ExecuteReader(Helper.GetDataBaseType,
                        DataBaseConnectionString, sqlCmd).CreateDataReader())
                    {
                        if (dr.Read())
                            cnt = clsGeneric.NullToInteger(dr["cnt"]);
                    }

                    if (cnt > 0)
                    {
                        sqlCmd = "SELECT GL_Account FROM sas_faculty_glaccount WHERE SAFT_Code = '" + argEn.SAFT_Code + "' " +
                                 "AND SAFC_Code = '" + argEn.SAFC_Code + "' ";

                        if (!FormHelp.IsBlank(sqlCmd))
                        {
                            using (IDataReader loReader = _DatabaseFactory.ExecuteReader(Helper.GetDataBaseType,
                                DataBaseConnectionString, sqlCmd).CreateDataReader())
                            {
                                while (loReader.Read())
                                {
                                    String GLAccount = GetValue<string>(loReader, "GL_Account");
                                    if (GLAccount != argEn.GL_Account)
                                    {
                                        UpdateIntoFacultyGLAccount(argEn);
                                    }
                                }
                                loReader.Close();
                            }
                        }
                    }

                    if (cnt == 0)
                    {
                        sqlCmd = "INSERT INTO sas_faculty_glaccount(SAFT_Code,SAFC_Code,SAFC_Desc,GL_Account)";
                        sqlCmd += "VALUES (@SAFT_Code,@SAFC_Code,@SAFC_Desc,@GL_Account) ";

                        if (!FormHelp.IsBlank(sqlCmd))
                        {
                            DbCommand cmd = _DatabaseFactory.GetDbCommand(Helper.GetDataBaseType, sqlCmd, DataBaseConnectionString);
                            _DatabaseFactory.AddInParameter(ref cmd, "@SAFT_Code", DbType.String, clsGeneric.NullToString(argEn.SAFT_Code));
                            _DatabaseFactory.AddInParameter(ref cmd, "@SAFC_Code", DbType.String, clsGeneric.NullToString(argEn.SAFC_Code));
                            _DatabaseFactory.AddInParameter(ref cmd, "@SAFC_Desc", DbType.String, clsGeneric.NullToString(argEn.SAFC_Desc));
                            _DatabaseFactory.AddInParameter(ref cmd, "@GL_Account", DbType.String, clsGeneric.NullToString(argEn.GL_Account));
                            _DbParameterCollection = cmd.Parameters;

                            int liRowAffected = _DatabaseFactory.ExecuteNonQuery(Helper.GetDataBaseType, cmd,
                                DataBaseConnectionString, sqlCmd, _DbParameterCollection);

                            if (liRowAffected > -1)
                                result = true;
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

            return result;
        }

        #endregion

        #region UpdateIntoFacultyGLAccount

        public bool UpdateIntoFacultyGLAccount(FacultyGLAccEn argEn)
        {
            bool lbRes = false;

            try
            {
                string sqlCmd = "UPDATE SAS_Faculty_Glaccount SET GL_Account = @GL_Account WHERE SAFT_Code = @SAFT_Code and SAFC_Code = @SAFC_Code";

                if (!FormHelp.IsBlank(sqlCmd))
                {
                    DbCommand cmd = _DatabaseFactory.GetDbCommand(Helper.GetDataBaseType, sqlCmd, DataBaseConnectionString);
                    _DatabaseFactory.AddInParameter(ref cmd, "@SAFT_Code", DbType.String, argEn.SAFT_Code);
                    _DatabaseFactory.AddInParameter(ref cmd, "@SAFC_Code", DbType.String, argEn.SAFC_Code);
                    _DatabaseFactory.AddInParameter(ref cmd, "@GL_Account", DbType.String, argEn.GL_Account);
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

        #region GetFacultyGLAccount

        public List<FacultyGLAccEn> GetFacultyGLAccount(FacultyGLAccEn argEn)
        {
            List<FacultyGLAccEn> _List = new List<FacultyGLAccEn>();

            string sqlCmd = "SELECT SAFT_Code,SAFC_Code,SAFC_Desc,GL_Account " +
                             "FROM SAS_Faculty_Glaccount " +
                             "WHERE SAFT_Code = @SAFT_Code";
            try
            {
                if (!FormHelp.IsBlank(sqlCmd))
                {
                    DbCommand cmd = _DatabaseFactory.GetDbCommand(Helper.GetDataBaseType, sqlCmd, DataBaseConnectionString);
                    _DatabaseFactory.AddInParameter(ref cmd, "@SAFT_Code", DbType.String, argEn.SAFT_Code);
                    _DbParameterCollection = cmd.Parameters;

                    using (IDataReader loReader = _DatabaseFactory.GetIDataReader(Helper.GetDataBaseType, cmd,
                        DataBaseConnectionString, sqlCmd, _DbParameterCollection).CreateDataReader())
                    {
                        while (loReader.Read())
                        {
                            FacultyGLAccEn loItem = new FacultyGLAccEn();
                            loItem.SAFT_Code = GetValue<string>(loReader, "SAFT_Code");
                            loItem.SAFC_Code = GetValue<string>(loReader, "SAFC_Code");
                            loItem.SAFC_Desc = GetValue<string>(loReader, "SAFC_Desc");
                            loItem.GL_Account = GetValue<string>(loReader, "GL_Account");
                            _List.Add(loItem);
                        }
                        loReader.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return _List;
        }

        #endregion
        //Faculty GL - END

        //Kolej GL - START
        #region InsertIntoKolejGLAccount

        public bool InsertIntoKolejGLAccount(KolejGLAccEn argEn)
        {
            bool result = false;
            int cnt = 0;

            string sqlCmd = "SELECT COUNT(*) AS cnt FROM sas_kolej_glaccount WHERE SAFT_Code = '" + argEn.SAFT_Code + "' " +
                            "AND SAKO_Code = '" + argEn.SAKO_Code + "' ";
            try
            {
                if (!FormHelp.IsBlank(sqlCmd))
                {
                    using (IDataReader dr = _DatabaseFactory.ExecuteReader(Helper.GetDataBaseType,
                        DataBaseConnectionString, sqlCmd).CreateDataReader())
                    {
                        if (dr.Read())
                            cnt = clsGeneric.NullToInteger(dr["cnt"]);
                    }

                    if (cnt > 0)
                    {
                        sqlCmd = "SELECT GL_Account FROM sas_kolej_glaccount WHERE SAFT_Code = '" + argEn.SAFT_Code + "' " +
                                 "AND SAKO_Code = '" + argEn.SAKO_Code + "' ";

                        if (!FormHelp.IsBlank(sqlCmd))
                        {
                            using (IDataReader loReader = _DatabaseFactory.ExecuteReader(Helper.GetDataBaseType,
                                DataBaseConnectionString, sqlCmd).CreateDataReader())
                            {
                                while (loReader.Read())
                                {
                                    String GLAccount = GetValue<string>(loReader, "GL_Account");
                                    if (GLAccount != argEn.GL_Account)
                                    {
                                        UpdateIntoKolejGLAccount(argEn);
                                    }
                                }
                                loReader.Close();
                            }
                        }
                    }

                    if (cnt == 0)
                    {
                        sqlCmd = "INSERT INTO sas_kolej_glaccount(SAFT_Code,SAKO_Code,SAKO_Description,GL_Account)";
                        sqlCmd += "VALUES (@SAFT_Code,@SAKO_Code,@SAKO_Description,@GL_Account) ";

                        if (!FormHelp.IsBlank(sqlCmd))
                        {
                            DbCommand cmd = _DatabaseFactory.GetDbCommand(Helper.GetDataBaseType, sqlCmd, DataBaseConnectionString);
                            _DatabaseFactory.AddInParameter(ref cmd, "@SAFT_Code", DbType.String, clsGeneric.NullToString(argEn.SAFT_Code));
                            _DatabaseFactory.AddInParameter(ref cmd, "@SAKO_Code", DbType.String, clsGeneric.NullToString(argEn.SAKO_Code));
                            _DatabaseFactory.AddInParameter(ref cmd, "@SAKO_Description", DbType.String, clsGeneric.NullToString(argEn.SAKO_Description));
                            _DatabaseFactory.AddInParameter(ref cmd, "@GL_Account", DbType.String, clsGeneric.NullToString(argEn.GL_Account));
                            _DbParameterCollection = cmd.Parameters;

                            int liRowAffected = _DatabaseFactory.ExecuteNonQuery(Helper.GetDataBaseType, cmd,
                                DataBaseConnectionString, sqlCmd, _DbParameterCollection);

                            if (liRowAffected > -1)
                                result = true;
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

            return result;
        }

        #endregion

        #region UpdateIntoKolejGLAccount

        public bool UpdateIntoKolejGLAccount(KolejGLAccEn argEn)
        {
            bool lbRes = false;

            try
            {
                string sqlCmd = "UPDATE sas_kolej_glaccount SET GL_Account = @GL_Account WHERE SAFT_Code = @SAFT_Code and SAKO_Code = @SAKO_Code";

                if (!FormHelp.IsBlank(sqlCmd))
                {
                    DbCommand cmd = _DatabaseFactory.GetDbCommand(Helper.GetDataBaseType, sqlCmd, DataBaseConnectionString);
                    _DatabaseFactory.AddInParameter(ref cmd, "@SAFT_Code", DbType.String, argEn.SAFT_Code);
                    _DatabaseFactory.AddInParameter(ref cmd, "@SAKO_Code", DbType.String, argEn.SAKO_Code);
                    _DatabaseFactory.AddInParameter(ref cmd, "@GL_Account", DbType.String, argEn.GL_Account);
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

        #region GetKolejGLAccount

        public List<KolejGLAccEn> GetKolejGLAccount(KolejGLAccEn argEn)
        {
            List<KolejGLAccEn> _List = new List<KolejGLAccEn>();

            string sqlCmd = "SELECT SAFT_Code,SAKO_Code,SAKO_Description,GL_Account " +
                             "FROM sas_kolej_glaccount " +
                             "WHERE SAFT_Code = @SAFT_Code";
            try
            {
                if (!FormHelp.IsBlank(sqlCmd))
                {
                    DbCommand cmd = _DatabaseFactory.GetDbCommand(Helper.GetDataBaseType, sqlCmd, DataBaseConnectionString);
                    _DatabaseFactory.AddInParameter(ref cmd, "@SAFT_Code", DbType.String, argEn.SAFT_Code);
                    _DbParameterCollection = cmd.Parameters;

                    using (IDataReader loReader = _DatabaseFactory.GetIDataReader(Helper.GetDataBaseType, cmd,
                        DataBaseConnectionString, sqlCmd, _DbParameterCollection).CreateDataReader())
                    {
                        while (loReader.Read())
                        {
                            KolejGLAccEn loItem = new KolejGLAccEn();
                            loItem.SAFT_Code = GetValue<string>(loReader, "SAFT_Code");
                            loItem.SAKO_Code = GetValue<string>(loReader, "SAKO_Code");
                            loItem.SAKO_Description = GetValue<string>(loReader, "SAKO_Description");
                            loItem.GL_Account = GetValue<string>(loReader, "GL_Account");
                            _List.Add(loItem);
                        }
                        loReader.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return _List;
        }

        #endregion
        //Kolej GL - END
    }

}
