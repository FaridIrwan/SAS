#region NameSpaces 

using System;
using System.Data;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data.Common;
using HTS.SAS.Entities;
using MaxGeneric;
using System.Linq;

#endregion

namespace HTS.SAS.DataAccessObjects
{
    /// <summary>
    /// Class to handle all the FeeStructure Methods.
    /// </summary>
    public class FeeStructDAL
    {
        #region Global Declarations 

        private DbParameterCollection _DbParameterCollection = null;

        private MaxModule.DatabaseProvider _DatabaseFactory =
            new MaxModule.DatabaseProvider();

        private string DataBaseConnectionString = Helper.
           GetConnectionString();

        #endregion

        public FeeStructDAL()
        {
        }

        #region GetList 

        /// <summary>
        /// Method to Get List of FeeStructure
        /// </summary>
        /// <param name="argEn">FeeStructure Entity is an Input.</param>
        /// <returns>Returns List of FeeStructure</returns>
        public List<FeeStructEn> GetList(FeeStructEn argEn)
        {
            List<FeeStructEn> loEnList = new List<FeeStructEn>();
            string sqlCmd = "select * from SAS_FeeStruct";
            
            try
            {
                if (!FormHelp.IsBlank(sqlCmd))
                {
                    using (IDataReader loReader = _DatabaseFactory.ExecuteReader(Helper.GetDataBaseType,
                        DataBaseConnectionString, sqlCmd).CreateDataReader())
                    {
                        while (loReader.Read())
                        {
                            FeeStructEn loItem = LoadObject(loReader);
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

        //#region GetFeeStructure 

        ///// <summary>
        ///// Method to Get List of Active or Inactive FeeStuctures
        ///// </summary>
        ///// <param name="argEn">FeeStucture Entity is an Input.FeeStryctureCode,PGCode,Semester,Description and status are Input Properties</param>
        ///// <returns>Returns List of FeeStructures.</returns>
        //public List<FeeStructEn> GetFeeStructure(FeeStructEn argEn)
        //{
        //    List<FeeStructEn> loEnList = new List<FeeStructEn>();
        //    //argEn.PGCode = "";
        //    string sqlCmd = "SELECT SAS_FeeStruct.*, SAS_SemesterSetup.SAST_Semester, SAS_SemesterSetup.SAST_Description FROM SAS_FeeStruct INNER JOIN" +
        //                    " SAS_SemesterSetup ON SAS_FeeStruct.SAST_Code = SAS_SemesterSetup.SAST_Code";
        //    if (argEn.FeeStructureCode.Length != 0) sqlCmd = sqlCmd + " and SAFS_Code = '" + argEn.FeeStructureCode + "'";
        //    if (argEn.PGCode.Length != 0) sqlCmd = sqlCmd + " and SAPG_Code = '" + argEn.PGCode + "'";
        //    if (!string.IsNullOrEmpty(argEn.semestersetup.Semester))
        //    {
        //        if (argEn.semestersetup.Semester.Length != 0) sqlCmd = sqlCmd + " and SAS_SemesterSetup.SAST_Semester = '" + argEn.semestersetup.Semester + "'";
        //    }
        //    if (!string.IsNullOrEmpty(argEn.semestersetup.Description))
        //    {
        //        if (argEn.semestersetup.Description.Length != 0) sqlCmd = sqlCmd + " and SAS_SemesterSetup.SAST_Description = '" + argEn.semestersetup.Description + "'";
        //    }

        //    if (argEn.Status == false) sqlCmd = sqlCmd + " and SAFS_Status =  false";
        //    if (argEn.Status == true) sqlCmd = sqlCmd + " and SAFS_Status = true";
        //    if (argEn.BidangCode.Length != 0) sqlCmd = sqlCmd + " WHERE sabp_code = '" + argEn.BidangCode + "'";
        //    sqlCmd = sqlCmd + " ORDER BY SAFS_Code";

        //    //string sqlCmd = "select * from sas_fee_bidang ORDER BY sabp_code";

        //    //string sqlCmd = "select distinct SAS_FeeStruct.sabp_code, max(SAS_FeeStruct.safs_code) AS safs_code,max(SAS_FeeStruct.safs_taxmode) AS safs_taxmode, " +
        //    //                "min(SAS_FeeStruct.sast_code) as sast_code FROM SAS_FeeStruct "; 
        //    //if (argEn.BidangCode.Length != 0) sqlCmd = sqlCmd + " WHERE sabp_code = '" + argEn.BidangCode + "'";
        //    //sqlCmd = sqlCmd + " group by sabp_code ORDER BY SAS_FeeStruct.sabp_code";

        //    try
        //    {
        //        if (!FormHelp.IsBlank(sqlCmd))
        //        {
        //            using (IDataReader loReader = _DatabaseFactory.ExecuteReader(Helper.GetDataBaseType,
        //                 DataBaseConnectionString, sqlCmd).CreateDataReader())
        //            {
        //                while (loReader.Read())
        //                {
        //                    FeeStrDetailsDAL loFeeSD = new FeeStrDetailsDAL();
        //                    FeeStrDetailsEn loFeeSDEn = new FeeStrDetailsEn();
        //                    FeeStructEn loItem = LoadObject(loReader);
        //                    loFeeSDEn.FSCode = loItem.FeeStructureCode;
        //                    loFeeSDEn._TaxId = loItem.TaxId;
        //                    loItem.ListFeeStrDetails = loFeeSD.GetFeeSDList(loFeeSDEn);
        //                    loEnList.Add(loItem);
        //                }
        //                loReader.Close();
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //    return loEnList;
        //}

        //#endregion

        #region GetFeeStructure (Bidang)

        /// <summary>
        /// Method to Get List of Active or Inactive FeeStuctures
        /// </summary>
        /// <param name="argEn">FeeStucture Entity is an Input.FeeStryctureCode,PGCode,Semester,Description and status are Input Properties</param>
        /// <returns>Returns List of FeeStructures.</returns>
        public List<FeeStructEn> GetFeeStructure(FeeStructEn argEn)
        {
            List<FeeStructEn> loEnList = new List<FeeStructEn>();
            //argEn.PGCode = "";
            string sqlCmd = "SELECT SAS_FeeStruct.*, SAS_SemesterSetup.SAST_Semester, SAS_SemesterSetup.SAST_Description FROM SAS_FeeStruct INNER JOIN" +
                            " SAS_SemesterSetup ON SAS_FeeStruct.SAST_Code = SAS_SemesterSetup.SAST_Code";
            if (argEn.FeeStructureCode.Length != 0) sqlCmd = sqlCmd + " and SAFS_Code = '" + argEn.FeeStructureCode + "'";
            //if (argEn.PGCode.Length != 0) sqlCmd = sqlCmd + " and SAPG_Code = '" + argEn.PGCode + "'";
            if (!string.IsNullOrEmpty(argEn.semestersetup.Semester))
            {
                if (argEn.semestersetup.Semester.Length != 0) sqlCmd = sqlCmd + " and SAS_SemesterSetup.SAST_Semester = '" + argEn.semestersetup.Semester + "'";
            }
            if (!string.IsNullOrEmpty(argEn.semestersetup.Description))
            {
                if (argEn.semestersetup.Description.Length != 0) sqlCmd = sqlCmd + " and SAS_SemesterSetup.SAST_Description = '" + argEn.semestersetup.Description + "'";
            }

            if (argEn.Status == false) sqlCmd = sqlCmd + " and SAFS_Status =  false";
            if (argEn.Status == true) sqlCmd = sqlCmd + " and SAFS_Status = true";
            if (argEn.BidangCode.Length != 0) sqlCmd = sqlCmd + " WHERE sabp_code = '" + argEn.BidangCode + "'";
            if (!string.IsNullOrEmpty(argEn.STCode))
            {
                if (argEn.STCode.Length != 0) sqlCmd = sqlCmd + " AND SAS_FeeStruct.SAST_Code = '" + argEn.STCode + "'";
            }            
            sqlCmd = sqlCmd + " ORDER BY SAFS_Code";            

            try
            {
                if (!FormHelp.IsBlank(sqlCmd))
                {
                    using (IDataReader loReader = _DatabaseFactory.ExecuteReader(Helper.GetDataBaseType,
                         DataBaseConnectionString, sqlCmd).CreateDataReader())
                    {
                        while (loReader.Read())
                        {
                            FeeStrDetailsDAL loFeeSD = new FeeStrDetailsDAL();
                            FeeStrDetailsEn loFeeSDEn = new FeeStrDetailsEn();
                            FeeStructEn loItem = LoadObject(loReader);
                            loFeeSDEn.FSCode = loItem.FeeStructureCode;
                            loFeeSDEn._TaxId = loItem.TaxId;
                            loItem.ListFeeStrDetails = loFeeSD.GetFeeSDList(loFeeSDEn);
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

        /// <summary>
        /// Method to Get List of Active or Inactive FeeStuctures
        /// </summary>
        /// <param name="argEn">FeeStucture Entity is an Input.FeeStryctureCode,PGCode,Semester,Description and status are Input Properties</param>
        /// <returns>Returns List of FeeStructures.</returns>
        /// Created by : Jessica
        /// Created Date : 24/02/2016
        public List<FeeStructEn> GetFeeStructureDetailList(FeeStructEn argEn)
        {
            List<FeeStructEn> loEnList = new List<FeeStructEn>();
            //argEn.PGCode = "";
            string sqlCmd = "SELECT SAS_FeeStruct.*, SAS_SemesterSetup.SAST_Semester, SAS_SemesterSetup.SAST_Description FROM SAS_FeeStruct INNER JOIN" +
                            " SAS_SemesterSetup ON SAS_FeeStruct.SAST_Code = SAS_SemesterSetup.SAST_Code";
            if (argEn.FeeStructureCode.Length != 0) sqlCmd = sqlCmd + " and SAFS_Code = '" + argEn.FeeStructureCode + "'";
            //if (argEn.PGCode.Length != 0) sqlCmd = sqlCmd + " and SAPG_Code = '" + argEn.PGCode + "'";
            if (!string.IsNullOrEmpty(argEn.semestersetup.Semester))
            {
                if (argEn.semestersetup.Semester.Length != 0) sqlCmd = sqlCmd + " and SAS_SemesterSetup.SAST_Semester = '" + argEn.semestersetup.Semester + "'";
            }
            if (!string.IsNullOrEmpty(argEn.semestersetup.SemisterSetupCode))
            {
                if (argEn.semestersetup.SemisterSetupCode.Length != 0)
                    sqlCmd = sqlCmd + " and SAS_SemesterSetup.sast_code = '" + argEn.semestersetup.SemisterSetupCode + "'";
            }
            if (!string.IsNullOrEmpty(argEn.semestersetup.Description))
            {
                if (argEn.semestersetup.Description.Length != 0) sqlCmd = sqlCmd + " and SAS_SemesterSetup.SAST_Description = '" + argEn.semestersetup.Description + "'";
            }

            if (argEn.Status == false) sqlCmd = sqlCmd + " and SAFS_Status =  false";
            if (argEn.Status == true) sqlCmd = sqlCmd + " and SAFS_Status = true";
            if (argEn.BidangCode.Length != 0) sqlCmd = sqlCmd + " WHERE sabp_code = '" + argEn.BidangCode + "'";
            if (!string.IsNullOrEmpty(argEn.STCode))
            {
                if (argEn.STCode.Length != 0) sqlCmd = sqlCmd + " AND SAS_FeeStruct.SAST_Code = '" + argEn.STCode + "'";
            }
            sqlCmd = sqlCmd + " ORDER BY SAFS_Code";

            try
            {
                if (!FormHelp.IsBlank(sqlCmd))
                {
                    using (IDataReader loReader = _DatabaseFactory.ExecuteReader(Helper.GetDataBaseType,
                         DataBaseConnectionString, sqlCmd).CreateDataReader())
                    {
                        while (loReader.Read())
                        {
                            FeeStrDetailsDAL loFeeSD = new FeeStrDetailsDAL();
                            FeeStructEn loItem = LoadObject(loReader);
                            loItem.lstFeeStrWithAmt = loFeeSD.GetFeeSDAmountList(loItem);
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

        #region GetFeeStructureList 

        /// <summary>
        /// Method to Get List of Active or Inactive FeeStucture BY StudentCategory
        /// </summary>
        /// <param name="argEn">FeeStucture Entity is an Input.FeeStryctureCode,PGCode,EffectFm and Status are Input Properties</param>
        /// <returns>Returns List of FeeStructure.</returns>
        public List<FeeStructEn> GetFeeStructureList(FeeStructEn argEn)
        {
            List<FeeStructEn> loEnList = new List<FeeStructEn>();
            argEn.FeeStructureCode = argEn.FeeStructureCode.Replace("*", "%");
            argEn.PGCode = argEn.PGCode.Replace("*", "%");
            argEn.EffectFm = argEn.EffectFm.Replace("*", "%");
            string sqlCmd = " SELECT SAS_FeeStruct.SAFS_Code, SAS_FeeStruct.SAPG_Code, SAS_FeeStruct.SAFS_EffectFm, SAS_FeeStruct.SAFS_Status, " +
                " SAS_FeeStruct.SAFS_UpdatedUser, SAS_FeeStruct.SAFS_UpdatedDtTm, SAS_FeeStrDetails.SAFD_Type, " +
                " SAS_FeeStrDetails.SAFT_Code, SAS_FeeTypes.SAFT_Desc, SAS_FeeStrDetails.SAFD_Priority, SAS_FeeStrDetails.SAFD_FeeFor, " +
                " SAS_FeeStrDetails.SAFD_Sem, SAS_FeeStrAmount.SASC_Code, SAS_StudentCategory.SASC_Desc, SAS_FeeStrAmount.SAFA_Amount, " +
                " SAS_FeeStruct.SAFS_CrPoint, SAS_FeeStruct.SAFS_TutAmt,SAS_FeeStruct.SAFS_CrAmt " +
                " FROM  SAS_FeeStruct INNER JOIN SAS_FeeStrDetails ON SAS_FeeStruct.SAFS_Code = SAS_FeeStrDetails.SAFS_Code INNER JOIN " +
                " SAS_FeeStrAmount ON SAS_FeeStruct.SAFS_Code = SAS_FeeStrAmount.SAFS_Code AND SAS_FeeStrDetails.SAFT_Code = SAS_FeeStrAmount.SAFT_Code " +
                " INNER JOIN SAS_StudentCategory ON SAS_FeeStrAmount.SASC_Code = SAS_StudentCategory.SASC_Code INNER JOIN " +
                " SAS_FeeTypes ON SAS_FeeStrDetails.SAFT_Code = SAS_FeeTypes.SAFT_Code";
            if (argEn.PGCode.Length != 0) sqlCmd = sqlCmd + " and SAS_FeeStruct.SAPG_Code = '" + argEn.PGCode + "'";
            if (argEn.EffectFm.Length != 0) sqlCmd = sqlCmd + " and SAS_FeeStruct.SAFS_EffectFm = '" + argEn.EffectFm + "%'";
            //if (argEn.Status == false) sqlCmd = sqlCmd + " and SAS_FeeStruct.SAFS_Status = 0";
            //if (argEn.Status == true) sqlCmd = sqlCmd + " and SAS_FeeStruct.SAFS_Status = 1";
            if (argEn.Status == false) sqlCmd = sqlCmd + " and SAS_FeeStruct.SAFS_Status = false";
            if (argEn.Status == true) sqlCmd = sqlCmd + " and SAS_FeeStruct.SAFS_Status = true";
            sqlCmd = sqlCmd + " ORDER BY SAS_FeeStruct.SAFS_Code";
            
            try
            {
                if (!FormHelp.IsBlank(sqlCmd))
                {
                    using (IDataReader loReader = _DatabaseFactory.ExecuteReader(Helper.GetDataBaseType,
                        DataBaseConnectionString, sqlCmd).CreateDataReader())
                    {
                        while (loReader.Read())
                        {
                            FeeStructEn loItem = LoadObject(loReader);
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
        /// Method to Get FeeStructure Entity
        /// </summary>
        /// <param name="argEn">FeeStructure Entity is an Input</param>
        /// <returns>Returns FeeStructure Entity</returns>
        public FeeStructEn GetItem(FeeStructEn argEn)
        {
            FeeStructEn loItem = new FeeStructEn();
            string sqlCmd = "Select * FROM SAS_FeeStruct WHERE SAFS_Code = @SAFS_Code";
            
            try
            {
                if (!FormHelp.IsBlank(sqlCmd))
                {
                    DbCommand cmd = _DatabaseFactory.GetDbCommand(Helper.GetDataBaseType, sqlCmd, DataBaseConnectionString);
                    _DatabaseFactory.AddInParameter(ref cmd, "@SAFS_Code", DbType.String, argEn.FeeStructureCode);
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

        //#region Insert 

        ///// <summary>
        ///// Method to Insert FeeStructure 
        ///// </summary>
        ///// <param name="argEn">FeeStructure Entity is an Input.</param>
        ///// <returns>Returns Boolean</returns>
        //public bool Insert(FeeStructEn argEn)
        //{
        //    bool lbRes = false;
        //    int iOut = 0;
        //    string sqlCmd = "Select count(*) as cnt From SAS_FeeStruct WHERE SAFS_Code != @SAFS_Code and SAPG_Code=@SAPG_Code and SAST_Code=@SAST_Code";
        //    try
        //    {
        //        if (!FormHelp.IsBlank(sqlCmd))
        //        {
        //            DbCommand cmdSel = _DatabaseFactory.GetDbCommand(Helper.GetDataBaseType, sqlCmd, DataBaseConnectionString);
        //            _DatabaseFactory.AddInParameter(ref cmdSel, "@SAFS_Code", DbType.String, argEn.FeeStructureCode);
        //            _DatabaseFactory.AddInParameter(ref cmdSel, "@SAPG_Code", DbType.String, argEn.PGCode);
        //            _DatabaseFactory.AddInParameter(ref cmdSel, "@SAST_Code", DbType.String, argEn.STCode);
        //            _DbParameterCollection = cmdSel.Parameters;

        //            using (IDataReader dr = _DatabaseFactory.GetIDataReader(Helper.GetDataBaseType, cmdSel,
        //                DataBaseConnectionString, sqlCmd, _DbParameterCollection).CreateDataReader())
        //            {
        //                if (dr.Read())
        //                {
        //                   // iOut = GetValue<int>(dr, "cnt");
        //                    iOut = clsGeneric.NullToInteger(dr["cnt"]);
        //                }
        //                if (iOut > 0)
        //                {
        //                    throw new Exception("Record Already Exist");
        //                }
        //                dr.Close();
        //                dr.Dispose();
        //            }
        //            if (iOut == 0)
        //            {
        //                string sqlCmd_fee = "INSERT INTO SAS_FeeStruct(SAFS_Code,SAPG_Code,SAST_Code,SAFS_Status,SAFS_UpdatedUser," +
        //                "SAFS_UpdatedDtTm,SAFS_CrPoint,SAFS_TutAmt,SAFS_CrAmt,SAFS_Semester,safs_taxmode) VALUES (@SAFS_Code,@SAPG_Code,@SAST_Code," +
        //                "@SAFS_Status,@SAFS_UpdatedUser,@SAFS_UpdatedDtTm,@SAFS_CrPoint,@SAFS_TutAmt,@SAFS_CrAmt,@SAFS_Semester,@safs_taxmode) ";

        //                if (!FormHelp.IsBlank(sqlCmd_fee))
        //                {
        //                    argEn.FeeStructureCode = GetAutoNumber("FS");
        //                    DbCommand cmd = _DatabaseFactory.GetDbCommand(Helper.GetDataBaseType, sqlCmd_fee, DataBaseConnectionString);
        //                    _DatabaseFactory.AddInParameter(ref cmd, "@SAFS_Code", DbType.String, argEn.FeeStructureCode);
        //                  //  _DatabaseFactory.AddInParameter(ref cmd, "@SAFS_Code", DbType.String, argEn.csSAST_Code);
        //                    _DatabaseFactory.AddInParameter(ref cmd, "@SAPG_Code", DbType.String, argEn.PGCode);
        //                    _DatabaseFactory.AddInParameter(ref cmd, "@SAST_Code", DbType.String, argEn.STCode);
        //                    _DatabaseFactory.AddInParameter(ref cmd, "@SAFS_Status", DbType.Boolean, argEn.Status);
        //                    _DatabaseFactory.AddInParameter(ref cmd, "@SAFS_UpdatedUser", DbType.String, argEn.UpdatedUser);
        //                    _DatabaseFactory.AddInParameter(ref cmd, "@SAFS_UpdatedDtTm", DbType.String, argEn.UpdatedDtTm);
        //                    _DatabaseFactory.AddInParameter(ref cmd, "@SAFS_CrPoint", DbType.Double, argEn.CrPoint);
        //                    _DatabaseFactory.AddInParameter(ref cmd, "@SAFS_TutAmt", DbType.Double, argEn.TutAmt);
        //                    _DatabaseFactory.AddInParameter(ref cmd, "@SAFS_CrAmt", DbType.Double, 0);
        //                    _DatabaseFactory.AddInParameter(ref cmd, "@SAFS_Semester", DbType.String, argEn.Semester);
        //                    _DatabaseFactory.AddInParameter(ref cmd, "@SAFS_Semester", DbType.Int16, argEn.Semester);
        //                    _DatabaseFactory.AddInParameter(ref cmd, "@safs_taxmode", DbType.Int16, argEn.TaxId);
        //                    _DbParameterCollection = cmd.Parameters;

        //                    int liRowAffected = _DatabaseFactory.ExecuteNonQuery(Helper.GetDataBaseType, cmd,
        //                        DataBaseConnectionString, sqlCmd_fee, _DbParameterCollection);
                            
        //                    int i = 0;

        //                    FeeStrDetailsDAL loFstrDal = new FeeStrDetailsDAL();
        //                    FeeStrAmountDAL loFstrAmDal = new FeeStrAmountDAL();
        //                    string type;
        //                    //Inserting FeestructureDetails and FeeAmounts
        //                    if (argEn.ListFeeStrDetails != null && argEn.ListFeeStrDetails.Count > 0)
        //                    {
        //                        while (i < argEn.ListFeeStrDetails.Count)
        //                        {
        //                            int j = 0;

        //                            type = argEn.ListFeeStrDetails[i].Type;
        //                            argEn.ListFeeStrDetails[i].FSCode = argEn.FeeStructureCode;
        //                            loFstrDal.Insert(argEn.ListFeeStrDetails[i]);
        //                            while (j < argEn.ListFeeStrDetails[i].ListFeeAmount.Count)
        //                            {
        //                                argEn.ListFeeStrDetails[i].ListFeeAmount[j].Type = type;
        //                                argEn.ListFeeStrDetails[i].ListFeeAmount[j].FSCode = argEn.FeeStructureCode;
        //                                loFstrAmDal.Insert(argEn.ListFeeStrDetails[i].ListFeeAmount[j]);
        //                                j = j + 1;
        //                            }
        //                            i = i + 1;
        //                        }
        //                    }

        //                    if (liRowAffected > -1)
        //                        lbRes = true;
        //                    else
        //                        throw new Exception("Insertion Failed! No Row has been updated...");
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

        //#endregion

        //#region Insert 

        ///// <summary>
        ///// Method to Insert FeeStructure 
        ///// </summary>
        ///// <param name="argEn">FeeStructure Entity is an Input.</param>
        ///// <returns>Returns Boolean</returns>
        //public bool Insert(FeeStructEn argEn)
        //{
        //    bool lbRes = false;
        //    int iOut = 0;
        //    string sqlCmd = "Select count(*) as cnt From SAS_FeeStruct WHERE SAFS_Code != @SAFS_Code and SAPG_Code=@SAPG_Code and SAST_Code=@SAST_Code";
        //    try
        //    {
        //        if (!FormHelp.IsBlank(sqlCmd))
        //        {
        //            DbCommand cmdSel = _DatabaseFactory.GetDbCommand(Helper.GetDataBaseType, sqlCmd, DataBaseConnectionString);
        //            _DatabaseFactory.AddInParameter(ref cmdSel, "@SAFS_Code", DbType.String, argEn.FeeStructureCode);
        //            _DatabaseFactory.AddInParameter(ref cmdSel, "@SAPG_Code", DbType.String, argEn.PGCode);
        //            _DatabaseFactory.AddInParameter(ref cmdSel, "@SAST_Code", DbType.String, argEn.STCode);
        //            _DbParameterCollection = cmdSel.Parameters;

        //            using (IDataReader dr = _DatabaseFactory.GetIDataReader(Helper.GetDataBaseType, cmdSel,
        //                DataBaseConnectionString, sqlCmd, _DbParameterCollection).CreateDataReader())
        //            {
        //                if (dr.Read())
        //                {
        //                   // iOut = GetValue<int>(dr, "cnt");
        //                    iOut = clsGeneric.NullToInteger(dr["cnt"]);
        //                }
        //                if (iOut > 0)
        //                {
        //                    throw new Exception("Record Already Exist");
        //                }
        //                dr.Close();
        //                dr.Dispose();
        //            }
        //            if (iOut == 0)
        //            {
        //                string sqlCmd_fee = "INSERT INTO SAS_FeeStruct(SAFS_Code,SAPG_Code,SAST_Code,SAFS_Status,SAFS_UpdatedUser," +
        //                "SAFS_UpdatedDtTm,SAFS_CrPoint,SAFS_TutAmt,SAFS_CrAmt,SAFS_Semester,safs_taxmode) VALUES (@SAFS_Code,@SAPG_Code,@SAST_Code," +
        //                "@SAFS_Status,@SAFS_UpdatedUser,@SAFS_UpdatedDtTm,@SAFS_CrPoint,@SAFS_TutAmt,@SAFS_CrAmt,@SAFS_Semester,@safs_taxmode) ";

        //                if (!FormHelp.IsBlank(sqlCmd_fee))
        //                {
        //                    argEn.FeeStructureCode = GetAutoNumber("FS");
        //                    DbCommand cmd = _DatabaseFactory.GetDbCommand(Helper.GetDataBaseType, sqlCmd_fee, DataBaseConnectionString);
        //                    _DatabaseFactory.AddInParameter(ref cmd, "@SAFS_Code", DbType.String, argEn.FeeStructureCode);
        //                  //  _DatabaseFactory.AddInParameter(ref cmd, "@SAFS_Code", DbType.String, argEn.csSAST_Code);
        //                    _DatabaseFactory.AddInParameter(ref cmd, "@SAPG_Code", DbType.String, argEn.PGCode);
        //                    _DatabaseFactory.AddInParameter(ref cmd, "@SAST_Code", DbType.String, argEn.STCode);
        //                    _DatabaseFactory.AddInParameter(ref cmd, "@SAFS_Status", DbType.Boolean, argEn.Status);
        //                    _DatabaseFactory.AddInParameter(ref cmd, "@SAFS_UpdatedUser", DbType.String, argEn.UpdatedUser);
        //                    _DatabaseFactory.AddInParameter(ref cmd, "@SAFS_UpdatedDtTm", DbType.String, argEn.UpdatedDtTm);
        //                    _DatabaseFactory.AddInParameter(ref cmd, "@SAFS_CrPoint", DbType.Double, argEn.CrPoint);
        //                    _DatabaseFactory.AddInParameter(ref cmd, "@SAFS_TutAmt", DbType.Double, argEn.TutAmt);
        //                    _DatabaseFactory.AddInParameter(ref cmd, "@SAFS_CrAmt", DbType.Double, 0);
        //                    _DatabaseFactory.AddInParameter(ref cmd, "@SAFS_Semester", DbType.String, argEn.Semester);
        //                    _DatabaseFactory.AddInParameter(ref cmd, "@SAFS_Semester", DbType.Int16, argEn.Semester);
        //                    _DatabaseFactory.AddInParameter(ref cmd, "@safs_taxmode", DbType.Int16, argEn.TaxId);
        //                    _DbParameterCollection = cmd.Parameters;

        //                    int liRowAffected = _DatabaseFactory.ExecuteNonQuery(Helper.GetDataBaseType, cmd,
        //                        DataBaseConnectionString, sqlCmd_fee, _DbParameterCollection);

        //                    int i = 0;

        //                    FeeStrDetailsDAL loFstrDal = new FeeStrDetailsDAL();
        //                    FeeStrAmountDAL loFstrAmDal = new FeeStrAmountDAL();
        //                    string type;
        //                    //Inserting FeestructureDetails and FeeAmounts
        //                    if (argEn.ListFeeStrDetails != null && argEn.ListFeeStrDetails.Count > 0)
        //                    {
        //                        while (i < argEn.ListFeeStrDetails.Count)
        //                        {
        //                            int j = 0;

        //                            type = argEn.ListFeeStrDetails[i].Type;
        //                            argEn.ListFeeStrDetails[i].FSCode = argEn.FeeStructureCode;
        //                            loFstrDal.Insert(argEn.ListFeeStrDetails[i]);
        //                            while (j < argEn.ListFeeStrDetails[i].ListFeeAmount.Count)
        //                            {
        //                                argEn.ListFeeStrDetails[i].ListFeeAmount[j].Type = type;
        //                                argEn.ListFeeStrDetails[i].ListFeeAmount[j].FSCode = argEn.FeeStructureCode;
        //                                loFstrAmDal.Insert(argEn.ListFeeStrDetails[i].ListFeeAmount[j]);
        //                                j = j + 1;
        //                            }
        //                            i = i + 1;
        //                        }
        //                    }

        //                    if (liRowAffected > -1)
        //                        lbRes = true;
        //                    else
        //                        throw new Exception("Insertion Failed! No Row has been updated...");
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

        //#endregion

        #region Insert

        /// <summary>
        /// Method to Insert FeeStructure 
        /// </summary>
        /// <param name="argEn">FeeStructure Entity is an Input.</param>
        /// <returns>Returns Boolean</returns>
        public bool Insert(FeeStructEn argEn, bool GenerateAutoNumber = true)
        {
            bool lbRes = false;
            int iOut = 0;

            //string sqlCmd = "Select count(*) as cnt From SAS_FeeStruct WHERE SAFS_Code = @SAFS_Code and SABP_Code=@SABP_Code and SAST_Code=@SAST_Code";
            string sqlCmd = "Select count(*) as cnt From SAS_FeeStruct WHERE SABP_Code=@SABP_Code and SAST_Code=@SAST_Code";

            try
            {
                if (!FormHelp.IsBlank(sqlCmd))
                {
                    DbCommand cmdSel = _DatabaseFactory.GetDbCommand(Helper.GetDataBaseType, sqlCmd, DataBaseConnectionString);
                    //_DatabaseFactory.AddInParameter(ref cmdSel, "@SAFS_Code", DbType.String, argEn.FeeStructureCode);
                    _DatabaseFactory.AddInParameter(ref cmdSel, "@SABP_Code", DbType.String, clsGeneric.NullToString(argEn.BidangCode));
                    _DatabaseFactory.AddInParameter(ref cmdSel, "@SAST_Code", DbType.String, clsGeneric.NullToString(argEn.STCode));
                    _DbParameterCollection = cmdSel.Parameters;

                    using (IDataReader dr = _DatabaseFactory.GetIDataReader(Helper.GetDataBaseType, cmdSel,
                        DataBaseConnectionString, sqlCmd, _DbParameterCollection).CreateDataReader())
                    {
                        if (dr.Read())
                        {
                            // iOut = GetValue<int>(dr, "cnt");
                            iOut = clsGeneric.NullToInteger(dr["cnt"]);
                        }
                        if (iOut > 0)
                        {
                            throw new Exception("Record Already Exist");
                        }
                        dr.Close();
                        dr.Dispose();
                    }
                    if (iOut == 0)
                    {
                        //string sqlDelete = "DELETE from SAS_FeeStrDetails where SAFS_Code in (select SAFS_Code from SAS_FeeStruct WHERE SABP_Code=@SABP_Code and SAST_Code=@SAST_Code);";
                        //sqlDelete += " DELETE from SAS_FeeStrAmount where SAFS_Code in (select SAFS_Code from SAS_FeeStruct WHERE SABP_Code=@SABP_Code and SAST_Code=@SAST_Code);";
                        //sqlDelete += " DELETE From SAS_FeeStruct WHERE SABP_Code=@SABP_Code and SAST_Code=@SAST_Code";

                        //DbCommand cmdDEl = _DatabaseFactory.GetDbCommand(Helper.GetDataBaseType, sqlDelete, DataBaseConnectionString);                        

                        //_DatabaseFactory.AddInParameter(ref cmdDEl, "@SABP_Code", DbType.String, argEn.BidangCode);
                        //_DatabaseFactory.AddInParameter(ref cmdDEl, "@SAST_Code", DbType.String, argEn.STCode);
                        //_DbParameterCollection = cmdDEl.Parameters;

                        //int liRowDeleted = _DatabaseFactory.ExecuteNonQuery(Helper.GetDataBaseType, cmdDEl,
                        //    DataBaseConnectionString, sqlDelete, _DbParameterCollection);

                        //if (liRowDeleted > -1)
                        //{
                        string sqlCmd_fee = "INSERT INTO SAS_FeeStruct(SAFS_Code,SABP_Code,SAST_Code,SAFS_Status,SAFS_UpdatedUser," +
                        "SAFS_UpdatedDtTm,SAFS_CrPoint,SAFS_TutAmt,SAFS_CrAmt,SAFS_Semester,safs_taxmode, safd_FeeBaseOn, safs_nonlocaltutamt) VALUES (@SAFS_Code,@SABP_Code,@SAST_Code," +
                        "@SAFS_Status,@SAFS_UpdatedUser,@SAFS_UpdatedDtTm,@SAFS_CrPoint,@SAFS_TutAmt,@SAFS_CrAmt,@SAFS_Semester,@safs_taxmode,@safd_FeeBaseOn, @safs_nonlocaltutamt) ";

                        if (!FormHelp.IsBlank(sqlCmd_fee))
                        {
                            if (GenerateAutoNumber)
                            {
                                argEn.FeeStructureCode = GetAutoNumber("FS");
                            }
                            DbCommand cmd = _DatabaseFactory.GetDbCommand(Helper.GetDataBaseType, sqlCmd_fee, DataBaseConnectionString);
                            _DatabaseFactory.AddInParameter(ref cmd, "@SAFS_Code", DbType.String, clsGeneric.NullToString(argEn.FeeStructureCode));
                            //_DatabaseFactory.AddInParameter(ref cmd, "@SAFS_Code", DbType.String, argEn.csSAST_Code);
                            //_DatabaseFactory.AddInParameter(ref cmd, "@SAPG_Code", DbType.String, argEn.PGCode);
                            _DatabaseFactory.AddInParameter(ref cmd, "@SABP_Code", DbType.String, clsGeneric.NullToString(argEn.BidangCode));
                            _DatabaseFactory.AddInParameter(ref cmd, "@SAST_Code", DbType.String, clsGeneric.NullToString(argEn.STCode));
                            _DatabaseFactory.AddInParameter(ref cmd, "@SAFS_Status", DbType.Boolean, clsGeneric.NullToBoolean(argEn.Status));
                            _DatabaseFactory.AddInParameter(ref cmd, "@SAFS_UpdatedUser", DbType.String, clsGeneric.NullToString(argEn.UpdatedUser));
                            _DatabaseFactory.AddInParameter(ref cmd, "@SAFS_UpdatedDtTm", DbType.String, clsGeneric.NullToString(argEn.UpdatedDtTm));
                            _DatabaseFactory.AddInParameter(ref cmd, "@SAFS_CrPoint", DbType.Double, clsGeneric.NullToDecimal(argEn.CrPoint));
                            _DatabaseFactory.AddInParameter(ref cmd, "@SAFS_TutAmt", DbType.Double, clsGeneric.NullToDecimal(argEn.TutAmt));
                            _DatabaseFactory.AddInParameter(ref cmd, "@SAFS_CrAmt", DbType.Double, 0);
                            _DatabaseFactory.AddInParameter(ref cmd, "@SAFS_Semester", DbType.String, clsGeneric.NullToString(argEn.Semester));
                            //_DatabaseFactory.AddInParameter(ref cmd, "@SAFS_Semester", DbType.Int16, argEn.Semester);
                            _DatabaseFactory.AddInParameter(ref cmd, "@safs_taxmode", DbType.Int16, clsGeneric.NullToInteger(argEn.TaxId));
                            _DatabaseFactory.AddInParameter(ref cmd, "@safd_FeeBaseOn", DbType.String, clsGeneric.NullToString(argEn.FeeBaseOn));
                            _DatabaseFactory.AddInParameter(ref cmd, "@safs_nonlocaltutamt", DbType.Double, clsGeneric.NullToDecimal(argEn.NonTutAmt));
                            _DbParameterCollection = cmd.Parameters;

                            int liRowAffected = _DatabaseFactory.ExecuteNonQuery(Helper.GetDataBaseType, cmd,
                                DataBaseConnectionString, sqlCmd_fee, _DbParameterCollection);

                            int i = 0;

                            FeeStrDetailsDAL loFstrDal = new FeeStrDetailsDAL();
                            FeeStrAmountDAL loFstrAmDal = new FeeStrAmountDAL();
                            string type;
                            //Inserting FeestructureDetails and FeeAmounts
                            if (argEn.ListFeeStrDetails != null && argEn.ListFeeStrDetails.Count > 0)
                            {
                                while (i < argEn.ListFeeStrDetails.Count)
                                {
                                    int j = 0;

                                    type = argEn.ListFeeStrDetails[i].Type;
                                    argEn.ListFeeStrDetails[i].FSCode = argEn.FeeStructureCode;
                                    loFstrDal.Insert(argEn.ListFeeStrDetails[i]);
                                    while (j < argEn.ListFeeStrDetails[i].ListFeeAmount.Count)
                                    {
                                        argEn.ListFeeStrDetails[i].ListFeeAmount[j].Type = type;
                                        argEn.ListFeeStrDetails[i].ListFeeAmount[j].FSCode = argEn.FeeStructureCode;
                                        loFstrAmDal.Insert(argEn.ListFeeStrDetails[i].ListFeeAmount[j]);
                                        j = j + 1;
                                    }
                                    i = i + 1;
                                }
                            }

                            if (liRowAffected > -1)
                                lbRes = true;
                            else
                                throw new Exception("Insertion Failed! No Row has been updated...");
                        }

                        //}
                        //else
                        //    throw new Exception("Validation Before Insertion Failed!..");

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

        #region Update_Original -Comment out by jessica

        /// <summary>
        /// Method to Update FeeStructure 
        /// </summary>
        /// <param name="argEn">FeeStructure Entity is an Input.</param>
        /// <returns>Returns Boolean</returns>
        //public bool Update(FeeStructEn argEn)
        //{
        //    bool lbRes = false;
        //    int iOut = 0;
        //    string sqlCmd = "Select count(*) as cnt From SAS_FeeStruct WHERE SAFS_Code != @SAFS_Code and SAPG_Code=@SAPG_Code and " +
        //        //" SAST_Code=@SAST_Code and SAFS_Semester = @SAFS_Semester";
        //        " SAST_Code=@SAST_Code ";
        //    try
        //    {
        //        if (!FormHelp.IsBlank(sqlCmd))
        //        {
        //            DbCommand cmdSel = _DatabaseFactory.GetDbCommand(Helper.GetDataBaseType, sqlCmd, DataBaseConnectionString);
        //            _DatabaseFactory.AddInParameter(ref cmdSel, "@SAFS_Code", DbType.String, argEn.FeeStructureCode);
        //            _DatabaseFactory.AddInParameter(ref cmdSel, "@SAPG_Code", DbType.String, argEn.PGCode);
        //            _DatabaseFactory.AddInParameter(ref cmdSel, "@SAST_Code", DbType.String, argEn.STCode);
        //            //_DatabaseFactory.AddInParameter(ref cmdSel, "@SAFS_Semester", DbType.String, argEn.Semester);                    
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
        //                if (Delete(argEn))
        //                {

        //                    sqlCmd = "UPDATE SAS_FeeStruct SET SAFS_Code = @SAFS_Code, SAPG_Code = @SAPG_Code, SAST_Code = @SAST_Code," +
        //                        "SAFS_Status = @SAFS_Status, SAFS_UpdatedUser = @SAFS_UpdatedUser, SAFS_UpdatedDtTm = @SAFS_UpdatedDtTm, " +
        //                        "SAFS_CrPoint = @SAFS_CrPoint, SAFS_TutAmt = @SAFS_TutAmt,SAFS_CrAmt=@SAFS_CrAmt, SAFS_Semester=@SAFS_Semester,safs_taxmode=@safs_taxmode WHERE SAFS_Code = @SAFS_Code";

        //                    if (!FormHelp.IsBlank(sqlCmd))
        //                    {
        //                        DbCommand cmd = _DatabaseFactory.GetDbCommand(Helper.GetDataBaseType, sqlCmd, DataBaseConnectionString);
        //                        _DatabaseFactory.AddInParameter(ref cmd, "@SAFS_Code", DbType.String, argEn.FeeStructureCode);
        //                        _DatabaseFactory.AddInParameter(ref cmd, "@SAPG_Code", DbType.String, argEn.PGCode);
        //                        _DatabaseFactory.AddInParameter(ref cmd, "@SAST_Code", DbType.String, argEn.STCode);
        //                        _DatabaseFactory.AddInParameter(ref cmd, "@SAFS_Status", DbType.Boolean, argEn.Status);
        //                        _DatabaseFactory.AddInParameter(ref cmd, "@SAFS_UpdatedUser", DbType.String, argEn.UpdatedUser);
        //                        _DatabaseFactory.AddInParameter(ref cmd, "@SAFS_UpdatedDtTm", DbType.String, argEn.UpdatedDtTm);
        //                        _DatabaseFactory.AddInParameter(ref cmd, "@SAFS_CrPoint", DbType.Double, argEn.CrPoint);
        //                        _DatabaseFactory.AddInParameter(ref cmd, "@SAFS_TutAmt", DbType.Double, argEn.TutAmt);
        //                        _DatabaseFactory.AddInParameter(ref cmd, "@SAFS_CrAmt", DbType.Double, 0);// argEn.CrAmount
        //                        _DatabaseFactory.AddInParameter(ref cmd, "@SAFS_Semester", DbType.String, argEn.Semester);
        //                        _DatabaseFactory.AddInParameter(ref cmd, "@safs_taxmode", DbType.Int16, argEn.TaxId);
        //                        _DbParameterCollection = cmd.Parameters;

        //                        int liRowAffected = _DatabaseFactory.ExecuteNonQuery(Helper.GetDataBaseType, cmd,
        //                            DataBaseConnectionString, sqlCmd, _DbParameterCollection);

        //                        int i = 0;

        //                        FeeStrDetailsDAL loFstrDal = new FeeStrDetailsDAL();
        //                        FeeStrAmountDAL loFstrAmDal = new FeeStrAmountDAL();
        //                        FeeStrDetailsEn loFstrEn = new FeeStrDetailsEn();
        //                        FeeStrAmountEn loFstrAmEn = new FeeStrAmountEn();
        //                        loFstrEn.FSCode = argEn.FeeStructureCode;
        //                        loFstrAmEn.FSCode = argEn.FeeStructureCode;
        //                        loFstrDal.Delete(loFstrEn);
        //                        loFstrAmDal.Delete(loFstrAmEn);
        //                        string type;
        //                        //Inserting FeestructureDetails and FeeAmounts
        //                        if (argEn.ListFeeStrDetails != null && argEn.ListFeeStrDetails.Count > 0)
        //                        {
        //                            while (i < argEn.ListFeeStrDetails.Count)
        //                            {
        //                                type = argEn.ListFeeStrDetails[i].Type;
        //                                argEn.ListFeeStrDetails[i].FSCode = argEn.FeeStructureCode;
        //                                loFstrDal.Insert(argEn.ListFeeStrDetails[i]);
        //                                int j = 0;
        //                                while (j < argEn.ListFeeStrDetails[i].ListFeeAmount.Count)
        //                                {
        //                                    argEn.ListFeeStrDetails[i].ListFeeAmount[j].Type = type;
        //                                    argEn.ListFeeStrDetails[i].ListFeeAmount[j].FSCode = argEn.FeeStructureCode;
        //                                    loFstrAmDal.Insert(argEn.ListFeeStrDetails[i].ListFeeAmount[j]);
        //                                    j = j + 1;
        //                                }
        //                                i = i + 1;
        //                            }
        //                        }

        //                        if (liRowAffected > -1)
        //                            lbRes = true;
        //                        else
        //                            throw new Exception("Update Failed! No Row has been updated...");
        //                    }
        //                }

        //                else
        //                    throw new Exception("Update Failed! No Row has been updated...");                        
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //    return lbRes;
        //}

        #endregion

        #region Update
        /// <summary>
        /// Method to Update FeeStructure 
        /// </summary>
        /// <param name="argEn">FeeStructure Entity is an Input.</param>
        /// <returns>Returns Boolean</returns>
        /// Modified by Hafiz @ 24/2/2016
        /// Remove Delete(argEn) since this function already have inside Update

        //public bool Update(FeeStructEn argEn)
        //{
        //    bool lbRes = false;
        //    int iOut = 0;
        //    string sqlCmd = "Select count(*) as cnt From SAS_FeeStruct WHERE SAST_CODE != @SAST_CODE and SABP_Code=@SABP_Code";
        //    try
        //    {
        //        if (!FormHelp.IsBlank(sqlCmd))
        //        {
        //            DbCommand cmdSel = _DatabaseFactory.GetDbCommand(Helper.GetDataBaseType, sqlCmd, DataBaseConnectionString);
        //            _DatabaseFactory.AddInParameter(ref cmdSel, "@SAST_CODE", DbType.String, argEn.STCode);
        //            _DatabaseFactory.AddInParameter(ref cmdSel, "@SABP_Code", DbType.String, argEn.BidangCode);
        //            //_DatabaseFactory.AddInParameter(ref cmdSel, "@SAFS_Semester", DbType.String, argEn.Semester);                    
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
        //                //if (Delete(argEn))
        //                //{

        //                sqlCmd = "UPDATE SAS_FeeStruct SET SAFS_Code = @SAFS_Code, SAPG_Code = @SAPG_Code, SAST_Code = @SAST_Code," +
        //                    "SAFS_Status = @SAFS_Status, SAFS_UpdatedUser = @SAFS_UpdatedUser, SAFS_UpdatedDtTm = @SAFS_UpdatedDtTm, " +
        //                    "SAFS_CrPoint = @SAFS_CrPoint, SAFS_TutAmt = @SAFS_TutAmt,SAFS_CrAmt=@SAFS_CrAmt, SAFS_Semester=@SAFS_Semester,safs_taxmode=@safs_taxmode WHERE SAFS_Code = @SAFS_Code";

        //                if (!FormHelp.IsBlank(sqlCmd))
        //                {
        //                    DbCommand cmd = _DatabaseFactory.GetDbCommand(Helper.GetDataBaseType, sqlCmd, DataBaseConnectionString);
        //                    _DatabaseFactory.AddInParameter(ref cmd, "@SAFS_Code", DbType.String, clsGeneric.NullToString( argEn.FeeStructureCode));
        //                    _DatabaseFactory.AddInParameter(ref cmd, "@SAPG_Code", DbType.String, clsGeneric.NullToString(argEn.PGCode));
        //                    _DatabaseFactory.AddInParameter(ref cmd, "@SAST_Code", DbType.String, clsGeneric.NullToString(argEn.STCode));
        //                    _DatabaseFactory.AddInParameter(ref cmd, "@SAFS_Status", DbType.Boolean, clsGeneric.NullToBoolean(argEn.Status));
        //                    _DatabaseFactory.AddInParameter(ref cmd, "@SAFS_UpdatedUser", DbType.String, clsGeneric.NullToString(argEn.UpdatedUser));
        //                    _DatabaseFactory.AddInParameter(ref cmd, "@SAFS_UpdatedDtTm", DbType.String, clsGeneric.NullToString(argEn.UpdatedDtTm));
        //                    _DatabaseFactory.AddInParameter(ref cmd, "@SAFS_CrPoint", DbType.Double, clsGeneric.NullToDecimal(argEn.CrPoint));
        //                    _DatabaseFactory.AddInParameter(ref cmd, "@SAFS_TutAmt", DbType.Double, clsGeneric.NullToDecimal(argEn.TutAmt));
        //                    _DatabaseFactory.AddInParameter(ref cmd, "@SAFS_CrAmt", DbType.Double, 0);// argEn.CrAmount
        //                    _DatabaseFactory.AddInParameter(ref cmd, "@SAFS_Semester", DbType.String, clsGeneric.NullToString( argEn.Semester));
        //                    _DatabaseFactory.AddInParameter(ref cmd, "@safs_taxmode", DbType.Int16, clsGeneric.NullToInteger(argEn.TaxId));
        //                    _DbParameterCollection = cmd.Parameters;

        //                    int liRowAffected = _DatabaseFactory.ExecuteNonQuery(Helper.GetDataBaseType, cmd,
        //                        DataBaseConnectionString, sqlCmd, _DbParameterCollection);

        //                    int i = 0;

        //                    FeeStrDetailsDAL loFstrDal = new FeeStrDetailsDAL();
        //                    FeeStrAmountDAL loFstrAmDal = new FeeStrAmountDAL();
        //                    FeeStrDetailsEn loFstrEn = new FeeStrDetailsEn();
        //                    FeeStrAmountEn loFstrAmEn = new FeeStrAmountEn();
        //                    loFstrEn.FSCode = argEn.FeeStructureCode;
        //                    loFstrAmEn.FSCode = argEn.FeeStructureCode;
        //                    loFstrDal.Delete(loFstrEn);
        //                    loFstrAmDal.Delete(loFstrAmEn);
        //                    string type;
        //                    //Inserting FeestructureDetails and FeeAmounts
        //                    if (argEn.ListFeeStrDetails != null && argEn.ListFeeStrDetails.Count > 0)
        //                    {
        //                        while (i < argEn.ListFeeStrDetails.Count)
        //                        {
        //                            type = argEn.ListFeeStrDetails[i].Type;
        //                            argEn.ListFeeStrDetails[i].FSCode = argEn.FeeStructureCode;
        //                            loFstrDal.Insert(argEn.ListFeeStrDetails[i]);
        //                            int j = 0;
        //                            while (j < argEn.ListFeeStrDetails[i].ListFeeAmount.Count)
        //                            {
        //                                argEn.ListFeeStrDetails[i].ListFeeAmount[j].Type = type;
        //                                argEn.ListFeeStrDetails[i].ListFeeAmount[j].FSCode = argEn.FeeStructureCode;
        //                                loFstrAmDal.Insert(argEn.ListFeeStrDetails[i].ListFeeAmount[j]);
        //                                j = j + 1;
        //                            }
        //                            i = i + 1;
        //                        }
        //                    }

        //                    if (liRowAffected > -1)
        //                        lbRes = true;
        //                    else
        //                        throw new Exception("Update Failed! No Row has been updated...");
        //                }
        //                //}
        //                //
        //                //else
        //                //    throw new Exception("Update Failed! No Row has been updated...");                        
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //    return lbRes;
        //}
        #endregion

        #region Update
        // created by Jessica
        //modified by Hafiz @ 25/10/2016

        public bool Update(FeeStructEn argEn)
        {
            bool lbRes = false;

            lbRes = Delete(argEn,"update");

            if (lbRes)
                Insert(argEn, false);
            
            return lbRes;
        }
       
        #endregion

        #region Delete 

        /// <summary>
        /// Method to Delete FeeStructure 
        /// </summary>
        /// <param name="argEn">FeeStructure Entity is an Input.</param>
        /// <returns>Returns Boolean</returns>
        public bool Delete(FeeStructEn argEn, String optional_params = "")
        {
            bool lbRes = false;
            string sqlCmd = "DELETE FROM SAS_FeeStruct WHERE SAFS_Code = @SAFS_Code";

            try
            {
                if (!FormHelp.IsBlank(sqlCmd))
                {
                    if (optional_params != "update")
                    {
                        CheckPostedAFC(argEn.FeeStructureCode);
                    }

                    DbCommand cmd = _DatabaseFactory.GetDbCommand(Helper.GetDataBaseType, sqlCmd, DataBaseConnectionString);
                    _DatabaseFactory.AddInParameter(ref cmd, "@SAFS_Code", DbType.String, argEn.FeeStructureCode);
                    _DbParameterCollection = cmd.Parameters;

                    FeeStrDetailsDAL loFstrDal = new FeeStrDetailsDAL();
                    FeeStrAmountDAL loFstrAmDal = new FeeStrAmountDAL();
                    FeeStrDetailsEn loFstrEn = new FeeStrDetailsEn();
                    FeeStrAmountEn loFstrAmEn = new FeeStrAmountEn();
                    loFstrEn.FSCode = argEn.FeeStructureCode;
                    loFstrAmEn.FSCode = argEn.FeeStructureCode;
                    //Deleting FeestructureDetails and FeeAmounts
                    loFstrDal.Delete(loFstrEn);
                    loFstrAmDal.Delete(loFstrAmEn);

                    int liRowAffected = _DatabaseFactory.ExecuteNonQuery(Helper.GetDataBaseType, cmd,
                                DataBaseConnectionString, sqlCmd, _DbParameterCollection);

                    if (liRowAffected > -1)
                    {
                        lbRes = true;
                    }
                    else
                        throw new Exception("Delete Failed!..");
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
        /// Method to Load FeeStructure Entity 
        /// </summary>
        /// <param name="argReader">IDataReader Object is an Input.</param>
        /// <returns>Returns FeeStructure Entity</returns>
        public FeeStructEn LoadObject(IDataReader argReader)
        {
            FeeStructEn loItem = new FeeStructEn();
            loItem.FeeStructureCode = GetValue<string>(argReader, "SAFS_Code");
            loItem.PGCode = GetValue<string>(argReader, "SAPG_Code");
            loItem.STCode = GetValue<string>(argReader, "SAST_Code");
            loItem.EffectFm = GetValue<string>(argReader, "SAFS_EffectFm");
            loItem.Status = GetValue<bool>(argReader, "SAFS_Status");
            loItem.UpdatedUser = GetValue<string>(argReader, "SAFS_UpdatedUser");
            loItem.UpdatedDtTm = GetValue<string>(argReader, "SAFS_UpdatedDtTm");
            loItem.CrPoint = GetValue<double>(argReader, "SAFS_CrPoint");
            loItem.TutAmt = GetValue<double>(argReader, "SAFS_TutAmt");
            loItem.TaxId = GetValue<int>(argReader, "safs_taxmode");
            loItem.CrAmount = 0;// GetValue<double>(argReader, "SAFS_CrAmt");@safs_taxmode
            loItem.BidangCode = GetValue<string>(argReader, "sabp_code");
            loItem.FeeBaseOn = GetValue<string>(argReader, "safd_FeeBaseOn");

            return loItem;
        }
        /// <summary>
        /// Method to Load FeeStructure Entity 
        /// </summary>
        /// <param name="argReader">IDataReader Object is an Input.</param>
        /// <returns>Returns FeeStructure Entity</returns>
        public FeeStructEn LoadObjectSTCODE(IDataReader argReader)
        {
            FeeStructEn loItemFee = new FeeStructEn();
            loItemFee.STCode = GetValue<string>(argReader, "SAST_Code");
            return loItemFee;
        }
        private static T GetValue<T>(IDataReader argReader, string argColNm)
        {
            if (!argReader.IsDBNull(argReader.GetOrdinal(argColNm)))
                return (T)argReader.GetValue(argReader.GetOrdinal(argColNm));
            else
                return default(T);
        }

        #endregion

        #region GetAutoNumber 

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
                if (!FormHelp.IsBlank(SqlStr))
                {

                    IDataReader loReader = _DatabaseFactory.ExecuteReader(Helper.GetDataBaseType,
                        DataBaseConnectionString, SqlStr).CreateDataReader();
                                       
                          
          //IDataReader loReader = _DatabaseFactory.GetIDataReader(Helper.GetDataBaseType,
          //              DataBaseConnectionString, SqlStr,).CreateDataReader();

                    if (loReader.Read())
                    {
                        string strCode = loReader.GetInt32(0).ToString();
                        AutoCode = clsGeneric.NullToInteger(loReader["saan_code"]);
                        CurNo = Convert.ToInt32(loReader["saan_curno"]) + 1;
                        NoDigit = Convert.ToInt32(loReader["saan_nodigit"]);
                        AutoNo = Convert.ToString(loReader["saan_prefix"]);
                        if (CurNo.ToString().Length < NoDigit)
                        {
                            while (i < NoDigit - CurNo.ToString().Length)
                            {
                                AutoNo = AutoNo + "0";
                                i = i + 1;
                            }
                            AutoNo = AutoNo + CurNo;
                        }
                       
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

        #region CheckPostedAFC

        public bool CheckPostedAFC(string fscode)
        {
            string sqlCmd = "";
            bool res = false;
            int output = 0;

            try
            {
                sqlCmd = "Select count(*) as cnt FROM SAS_AFC AF INNER JOIN SAS_AFCDetails AD ON AF.TransCode=AD.TransCode " +
                    "INNER JOIN SAS_Program SP ON AD.ProgramCode=SP.SAPG_Code " +
                    "WHERE AF.Reference='Posted' " +
                    "AND SP.sabp_code IN (SELECT sabp_code FROM SAS_FeeStruct WHERE safs_code = '" + fscode + "') " +
                    "AND AF.semester IN (SELECT sast_code FROM SAS_FeeStruct WHERE safs_code = '" + fscode + "') ";

                if (!FormHelp.IsBlank(sqlCmd))
                {
                    using (IDataReader dr = _DatabaseFactory.ExecuteReader(Helper.GetDataBaseType,
                        DataBaseConnectionString, sqlCmd).CreateDataReader())
                    {
                        if (dr.Read())
                            output = clsGeneric.NullToInteger(dr["cnt"]);
                        if (output > 0)
                            throw new Exception("Delete Failed - Fee Structure Already Posted in AFC.");
                        dr.Close();
                    }
                    if (output == 0) res = true;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return res;
        }
        
        #endregion

    }

}
