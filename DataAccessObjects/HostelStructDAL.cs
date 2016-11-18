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
    /// Class to handle all the HostelStructure Methods.
    /// </summary>
    public class HostelStructDAL
    {
        #region Global Declarations 

        private DbParameterCollection _DbParameterCollection = null;

        private MaxModule.DatabaseProvider _DatabaseFactory =
            new MaxModule.DatabaseProvider();

        private string DataBaseConnectionString = Helper.
           GetConnectionString();

        #endregion

        public HostelStructDAL()
        {
        }

        #region GetList 

        /// <summary>
        /// Method to Get List of HostelStructure
        /// </summary>
        /// <param name="argEn">HostelStructure Entity is an Input.</param>
        /// <returns>Returns List of HostelStructure</returns>
        public List<HostelStructEn> GetList(HostelStructEn argEn)
        {
            List<HostelStructEn> loEnList = new List<HostelStructEn>();
            string sqlCmd = "select * from SAS_HostelStruct";
            
            try
            {
                if (!FormHelp.IsBlank(sqlCmd))
                {
                    using (IDataReader loReader = _DatabaseFactory.ExecuteReader(Helper.GetDataBaseType,
                        DataBaseConnectionString, sqlCmd).CreateDataReader())
                    {
                        while (loReader.Read())
                        {
                            HostelStructEn loItem = LoadObject(loReader);
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

        #region GetHostelFeeList 

        /// <summary>
        /// Method to Get List of HostelSructure Feelist
        /// </summary>
        /// <param name="argEn">HostelStructure Entity is an Input.HostelStructureCode,Code,Block and Status are Input Properties.</param>
        /// <returns>Returns List of HostelStructure</returns>
        public List<HostelStructEn> GetHostelFeeList(HostelStructEn argEn)
        {
            List<HostelStructEn> loEnList = new List<HostelStructEn>();
            argEn.HostelStructureCode = argEn.HostelStructureCode.Replace("*", "%");
            argEn.Code = argEn.Code.Replace("*", "%");
            argEn.Block = argEn.Block.Replace("*", "%");
            
            string sqlCmd = "SELECT SAS_HostelStruct.SAHS_Code, SAS_HostelStruct.SAHB_Code, SAS_HostelStruct.SAHB_Block, " +
                      " SAS_HostelStruct.SAHB_RoomTYpe, SAS_HostelStruct.SAHS_EffectFm, SAS_HostelStruct.SAFS_Status, " +
                      " SAS_HostelStruct.SAHS_UpdatedUser, SAS_HostelStruct.SAHS_UpdatedDtTm, SAS_HostelStrDetails.SAHD_Code, " +
                      " SAS_HostelStrDetails.SAFT_Code, SAS_FeeTypes.SAFT_Desc, SAS_HostelStrDetails.SAHD_Type, " +
                      " SAS_HostelStrDetails.SAHD_Priority, SAS_HostelStrAmount.SASC_Code, SAS_StudentCategory.SASC_Code AS Expr1, " +
                      " SAS_StudentCategory.SASC_Desc, SAS_HostelStrAmount.SAHA_Amount " +
                      " FROM  SAS_HostelStruct INNER JOIN " +
                      " SAS_HostelStrDetails ON SAS_HostelStruct.SAHS_Code = SAS_HostelStrDetails.SAHS_Code INNER JOIN " +
                      " SAS_HostelStrAmount ON SAS_HostelStruct.SAHS_Code = SAS_HostelStrAmount.SAHS_Code AND " +
                      " SAS_HostelStrDetails.SAFT_Code = SAS_HostelStrAmount.SAFT_Code INNER JOIN " +
                      " SAS_FeeTypes ON SAS_HostelStrDetails.SAFT_Code = SAS_FeeTypes.SAFT_Code INNER JOIN " +
                      " SAS_StudentCategory ON SAS_HostelStrAmount.SASC_Code = SAS_StudentCategory.SASC_Code where SAS_HostelStruct.SAHS_Code <> '0'";
            if (argEn.HostelStructureCode.Length !=0) sqlCmd =sqlCmd + " SAS_HostelStruct.SAHS_Code like '" + argEn.HostelStructureCode + "'";
            if (argEn.Code.Length != 0) sqlCmd = sqlCmd + " SAS_HostelStruct.SAHB_Code like '" + argEn.Code + "'";
            if (argEn.Block.Length != 0) sqlCmd = sqlCmd + " SAS_HostelStruct.SAHB_Block '" + argEn.Block + "'";
            //if (argEn.Status == true) sqlCmd = sqlCmd + " SAS_HostelStruct.SAFS_Status = 1";
            if (argEn.Status == true) sqlCmd = sqlCmd + " SAS_HostelStruct.SAFS_Status = 'true'";
            //if (argEn.Status == false) sqlCmd = sqlCmd + " SAS_HostelStruct.SAFS_Status = 0 ";
            if (argEn.Status == false) sqlCmd = sqlCmd + " SAS_HostelStruct.SAFS_Status = 'false' ";
            
            try
            {
                if (!FormHelp.IsBlank(sqlCmd))
                {
                    using (IDataReader loReader = _DatabaseFactory.ExecuteReader(Helper.GetDataBaseType,
                        DataBaseConnectionString, sqlCmd).CreateDataReader())
                    {
                        while (loReader.Read())
                        {
                            HostelStructEn loItem = LoadObject(loReader);
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

        #region GetHostelList 

        /// <summary>
        /// Method to Get List of Active or Inactive Hostels
        /// </summary>
        /// <param name="argEn">HostelStructure Entity is an Input.RoomType,Code,Block and Status are Input Properties.</param>
        /// <returns>Returns List of HostelStructure</returns>
        public List<HostelStructEn> GetHostelList(HostelStructEn argEn)
        {
            List<HostelStructEn> loEnList = new List<HostelStructEn>();

            argEn.RoomTYpe = argEn.RoomTYpe.Replace("*", "%");
            argEn.Code = argEn.Code.Replace("*", "%");
            argEn.Block = argEn.Block.Replace("*", "%");

            string sqlCmd = "select * from SAS_HostelStruct WHERE SAHS_Code <> '0'";
            if (argEn.RoomTYpe.Length != 0) sqlCmd = sqlCmd + " and SAHB_RoomTYpe like '" + argEn.RoomTYpe + "'";
            if (argEn.Code.Length != 0) sqlCmd = sqlCmd + " and SAHB_Code like '" + argEn.Code + "'";
            if (argEn.Block.Length != 0) sqlCmd = sqlCmd + " and SAHB_Block like '" + argEn.Block + "'";
            //if (argEn.Status == true) sqlCmd = sqlCmd + " and SAFS_Status = 1";
            if (argEn.Status == true) sqlCmd = sqlCmd + " and SAFS_Status = true";
            //if (argEn.Status == false) sqlCmd = sqlCmd + " and SAFS_Status = 0 ";
            if (argEn.Status == false) sqlCmd = sqlCmd + " and SAFS_Status = false ";

            
            try
            {
                if (!FormHelp.IsBlank(sqlCmd))
                {
                    HostelStrDetailsEn loHostelStrDetEn = new HostelStrDetailsEn();
                    HostelStrDetailsDAL loHostelStrDet = new HostelStrDetailsDAL();

                    using (IDataReader loReader = _DatabaseFactory.ExecuteReader(Helper.GetDataBaseType,
                        DataBaseConnectionString, sqlCmd).CreateDataReader())
                    {
                        while (loReader.Read())
                        {
                            HostelStructEn loItem = LoadObject(loReader);
                            loHostelStrDetEn.HSCode = loItem.HostelStructureCode;
                            loItem.lstHFeeSD = loHostelStrDet.GetHostelAmtList(loHostelStrDetEn);
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
        /// Method to Get HostelStructucture Entity
        /// </summary>
        /// <param name="argEn">HostelStructucture Entity is an Input.HostelStuctureCode as Input Property.</param>
        /// <returns>Returns HostelStructucture Entity</returns>
        public HostelStructEn GetItem(HostelStructEn argEn)
        {
            HostelStructEn loItem = new HostelStructEn();
            string sqlCmd = "Select * FROM SAS_HostelStruct WHERE SAHS_Code = @SAHS_Code";
            
            try
            {
                if (!FormHelp.IsBlank(sqlCmd))
                {
                    DbCommand cmd = _DatabaseFactory.GetDbCommand(Helper.GetDataBaseType, sqlCmd, DataBaseConnectionString);
                    _DatabaseFactory.AddInParameter(ref cmd, "@SAHS_Code", DbType.String, argEn.HostelStructureCode);
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
        /// Method to Insert HostelStructure 
        /// </summary>
        /// <param name="argEn">HostelStructure Entity is an Input.</param>
        /// <returns>Returns Boolean</returns>
        //public bool Insert(HostelStructEn argEn)
        //{
        //    bool lbRes = false;
        //    int iOut = 0;
        //    string sqlCmd = "Select count(*) as cnt From SAS_HostelStruct WHERE SAHS_Code != @SAHS_Code and SAHB_Code=@SAHB_Code and SAHB_Block=@SAHB_Block and SAHB_RoomTYpe=@SAHB_RoomTYpe";
        //    try
        //    {
        //        if (!FormHelp.IsBlank(sqlCmd))
        //        {
        //            DbCommand cmdSel = _DatabaseFactory.GetDbCommand(Helper.GetDataBaseType, sqlCmd, DataBaseConnectionString);
        //            _DatabaseFactory.AddInParameter(ref cmdSel, "@SAHS_Code", DbType.String, argEn.HostelStructureCode);
        //            _DatabaseFactory.AddInParameter(ref cmdSel, "@SAHB_Block", DbType.String, argEn.Block);
        //            _DatabaseFactory.AddInParameter(ref cmdSel, "@SAHB_Code", DbType.String, argEn.Code);
        //            _DatabaseFactory.AddInParameter(ref cmdSel, "@SAHB_RoomTYpe", DbType.String, argEn.RoomTYpe);
        //            _DbParameterCollection = cmdSel.Parameters;

        //            using (IDataReader dr = _DatabaseFactory.GetIDataReader(Helper.GetDataBaseType, cmdSel,
        //                DataBaseConnectionString, sqlCmd, _DbParameterCollection).CreateDataReader()) 
        //            {
        //                if (dr.Read())
        //                    iOut = clsGeneric.NullToInteger(dr["cnt"]);
        //                if (iOut > 0)
        //                    throw new Exception("Record Already Exist");
        //            }
        //        }
        //        if (iOut == 0)
        //        {
        //            sqlCmd = "INSERT INTO SAS_HostelStruct(SAHS_Code,SAHB_Code,SAHB_Block,SAHB_RoomTYpe,SAHS_EffectFm,SAFS_Status,SAHS_UpdatedUser,SAHS_UpdatedDtTm) VALUES (@SAHS_Code,@SAHB_Code,@SAHB_Block,@SAHB_RoomTYpe,@SAHS_EffectFm,@SAFS_Status,@SAHS_UpdatedUser,@SAHS_UpdatedDtTm) ";

        //            if (!FormHelp.IsBlank(sqlCmd))
        //            {

        //                argEn.HostelStructureCode = GetAutoNumber("HostelCode");
        //                DbCommand cmd = _DatabaseFactory.GetDbCommand(Helper.GetDataBaseType, sqlCmd, DataBaseConnectionString);
        //                _DatabaseFactory.AddInParameter(ref cmd, "@SAHS_Code", DbType.String, argEn.HostelStructureCode);
        //                _DatabaseFactory.AddInParameter(ref cmd, "@SAHB_Code", DbType.String, argEn.Code);
        //                _DatabaseFactory.AddInParameter(ref cmd, "@SAHB_Block", DbType.String, argEn.Block);
        //                _DatabaseFactory.AddInParameter(ref cmd, "@SAHB_RoomTYpe", DbType.String, argEn.RoomTYpe);
        //                _DatabaseFactory.AddInParameter(ref cmd, "@SAHS_EffectFm", DbType.String, argEn.EffectFm);
        //                _DatabaseFactory.AddInParameter(ref cmd, "@SAFS_Status", DbType.Boolean, argEn.Status);
        //                _DatabaseFactory.AddInParameter(ref cmd, "@SAHS_UpdatedUser", DbType.String, argEn.UpdatedUser);
        //                _DatabaseFactory.AddInParameter(ref cmd, "@SAHS_UpdatedDtTm", DbType.String, argEn.UpdatedDtTm);
        //                _DbParameterCollection = cmd.Parameters;

        //                int liRowAffected = _DatabaseFactory.ExecuteNonQuery(Helper.GetDataBaseType, cmd,
        //                        DataBaseConnectionString, sqlCmd, _DbParameterCollection);
        //                int i = 0;
        //                int j = 0;
                       
        //                HostelStrDetailsDAL loHstrDal = new HostelStrDetailsDAL();
        //                HostelStrAmountDAL loHstrAmDal = new HostelStrAmountDAL();
        //                while (i < argEn.lstHFeeSD.Count)
        //                {
        //                    //inserting new hostel feeDetails 
        //                    argEn.lstHFeeSD[i].HSCode = argEn.HostelStructureCode;
        //                    loHstrDal.Insert(argEn.lstHFeeSD[i]);
        //                    while (j < argEn.lstHFeeSD[i].ListFeeAmount.Count)
        //                    {
        //                        //inserting new hostel feeAmounts
        //                        argEn.lstHFeeSD[i].ListFeeAmount[j].HSCode = argEn.HostelStructureCode;
        //                        loHstrAmDal.Insert(argEn.lstHFeeSD[i].ListFeeAmount[j]);
        //                        j = j + 1;
        //                    }
        //                    i = i + 1;
        //                }
        //                if (liRowAffected > -1)
        //                    lbRes = true;
        //                else
        //                    throw new Exception("Insertion Failed! No Row has been updated...");
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //    return lbRes;
        //}

        /// <summary>
        /// Method to Insert HostelStructure 
        /// </summary>
        /// <param name="argEn">HostelStructure Entity is an Input.</param>
        /// <returns>Returns Boolean</returns>
        public bool Insert(HostelStructEn argEn, bool GenerateAutoNumber = true)
        {
            bool lbRes = false;
            int iOut = 0;
            //DO NOT USE SAHS_CODE TO CHECKING, CHECK COMPOSITE KEY ( SAHB_Code, SAHB_Block, SAHB_RoomTYpe)
            //string sqlCmd = "Select count(*) as cnt From SAS_HostelStruct WHERE SAHS_Code = @SAHS_Code and SAHB_Code=@SAHB_Code and SAHB_Block=@SAHB_Block and SAHB_RoomTYpe=@SAHB_RoomTYpe";
            string sqlCmd = "Select count(*) as cnt From SAS_HostelStruct WHERE SAHB_Code=@SAHB_Code and SAHB_Block=@SAHB_Block and SAHB_RoomTYpe=@SAHB_RoomTYpe";
            try
            {
                if (!FormHelp.IsBlank(sqlCmd))
                {
                    DbCommand cmdSel = _DatabaseFactory.GetDbCommand(Helper.GetDataBaseType, sqlCmd, DataBaseConnectionString);
                    // _DatabaseFactory.AddInParameter(ref cmdSel, "@SAHS_Code", DbType.String, argEn.HostelStructureCode);
                    _DatabaseFactory.AddInParameter(ref cmdSel, "@SAHB_Block", DbType.String, argEn.Block);
                    _DatabaseFactory.AddInParameter(ref cmdSel, "@SAHB_Code", DbType.String, argEn.Code);
                    _DatabaseFactory.AddInParameter(ref cmdSel, "@SAHB_RoomTYpe", DbType.String, argEn.RoomTYpe);
                    _DbParameterCollection = cmdSel.Parameters;

                    using (IDataReader dr = _DatabaseFactory.GetIDataReader(Helper.GetDataBaseType, cmdSel,
                        DataBaseConnectionString, sqlCmd, _DbParameterCollection).CreateDataReader())
                    {
                        if (dr.Read())
                            iOut = clsGeneric.NullToInteger(dr["cnt"]);
                        if (iOut > 0)
                            throw new Exception("Record Already Exist");
                    }
                }
                if (iOut == 0)
                {
                    sqlCmd = "INSERT INTO SAS_HostelStruct(SAHS_Code,SAHB_Code,SAHB_Block,SAHB_RoomTYpe,SAHS_EffectFm,SAFS_Status,SAHS_UpdatedUser,SAHS_UpdatedDtTm) VALUES (@SAHS_Code,@SAHB_Code,@SAHB_Block,@SAHB_RoomTYpe,@SAHS_EffectFm,@SAFS_Status,@SAHS_UpdatedUser,@SAHS_UpdatedDtTm) ";

                    if (!FormHelp.IsBlank(sqlCmd))
                    {
                        if (GenerateAutoNumber)
                        {
                            argEn.HostelStructureCode = GetAutoNumber("HostelCode");
                        }
                        DbCommand cmd = _DatabaseFactory.GetDbCommand(Helper.GetDataBaseType, sqlCmd, DataBaseConnectionString);
                        _DatabaseFactory.AddInParameter(ref cmd, "@SAHS_Code", DbType.String, clsGeneric.NullToString(argEn.HostelStructureCode));
                        _DatabaseFactory.AddInParameter(ref cmd, "@SAHB_Code", DbType.String, clsGeneric.NullToString(argEn.Code));
                        _DatabaseFactory.AddInParameter(ref cmd, "@SAHB_Block", DbType.String, clsGeneric.NullToString(argEn.Block));
                        _DatabaseFactory.AddInParameter(ref cmd, "@SAHB_RoomTYpe", DbType.String, clsGeneric.NullToString(argEn.RoomTYpe));
                        _DatabaseFactory.AddInParameter(ref cmd, "@SAHS_EffectFm", DbType.String, clsGeneric.NullToString(argEn.EffectFm));
                        _DatabaseFactory.AddInParameter(ref cmd, "@SAFS_Status", DbType.Boolean, clsGeneric.NullToBoolean(argEn.Status));
                        _DatabaseFactory.AddInParameter(ref cmd, "@SAHS_UpdatedUser", DbType.String, clsGeneric.NullToString(argEn.UpdatedUser));
                        _DatabaseFactory.AddInParameter(ref cmd, "@SAHS_UpdatedDtTm", DbType.String, clsGeneric.NullToString(DateTime.Now.ToString("dd/MM/yyyy")));
                        _DbParameterCollection = cmd.Parameters;

                        int liRowAffected = _DatabaseFactory.ExecuteNonQuery(Helper.GetDataBaseType, cmd,
                                DataBaseConnectionString, sqlCmd, _DbParameterCollection);
                        int i = 0;
                        int j = 0;

                        HostelStrDetailsDAL loHstrDal = new HostelStrDetailsDAL();
                        HostelStrAmountDAL loHstrAmDal = new HostelStrAmountDAL();
                        while (i < argEn.lstHFeeSD.Count)
                        {
                            //inserting new hostel feeDetails 
                            argEn.lstHFeeSD[i].HSCode = argEn.HostelStructureCode;
                            loHstrDal.Insert(argEn.lstHFeeSD[i]);
                            j = 0;
                            while (j < argEn.lstHFeeSD[i].ListFeeAmount.Count)
                            {
                                //inserting new hostel feeAmounts
                                argEn.lstHFeeSD[i].ListFeeAmount[j].HSCode = argEn.HostelStructureCode;
                                loHstrAmDal.Insert(argEn.lstHFeeSD[i].ListFeeAmount[j]);
                                j = j + 1;
                            }
                            i = i + 1;
                        }
                        if (liRowAffected > -1)
                            lbRes = true;
                        else
                            throw new Exception("Insertion Failed! No Row has been updated...");
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
        /// Method to Update HostelStructure 
        /// </summary>
        /// <param name="argEn">HostelStructure Entity is an Input.</param>
        /// <returns>Returns Boolean</returns>
        //public bool Update(HostelStructEn argEn)
        //{
        //    bool lbRes = false;
        //    int iOut = 0;
        //    string sqlCmd = "Select count(*) as cnt From SAS_HostelStruct WHERE SAHS_Code != @SAHS_Code and SAHB_Code=@SAHB_Code and SAHB_Block=@SAHB_Block and SAHB_RoomTYpe=@SAHB_RoomTYpe";
        //    try
        //    {
        //        if (!FormHelp.IsBlank(sqlCmd))
        //        {
        //            DbCommand cmdSel = _DatabaseFactory.GetDbCommand(Helper.GetDataBaseType, sqlCmd, DataBaseConnectionString);
        //            _DatabaseFactory.AddInParameter(ref cmdSel, "@SAHS_Code", DbType.String, argEn.HostelStructureCode);
        //            _DatabaseFactory.AddInParameter(ref cmdSel, "@SAHB_Block", DbType.String, argEn.Block);
        //            _DatabaseFactory.AddInParameter(ref cmdSel, "@SAHB_Code", DbType.String, argEn.Code);
        //            _DatabaseFactory.AddInParameter(ref cmdSel, "@SAHB_RoomTYpe", DbType.String, argEn.RoomTYpe);
        //            _DbParameterCollection = cmdSel.Parameters;

        //            using (IDataReader dr = _DatabaseFactory.GetIDataReader(Helper.GetDataBaseType, cmdSel,
        //              DataBaseConnectionString, sqlCmd, _DbParameterCollection).CreateDataReader()) 
        //            {

        //                if (dr.Read())
        //                    iOut = clsGeneric.NullToInteger(dr["cnt"]);
        //                if (iOut > 0)
        //                    throw new Exception("Record Already Exist");
        //            }
        //         }
        //            if (iOut == 0)
        //            {
        //                sqlCmd = "UPDATE SAS_HostelStruct SET SAHS_Code = @SAHS_Code, SAHB_Code = @SAHB_Code, SAHB_Block = @SAHB_Block, SAHB_RoomTYpe = @SAHB_RoomTYpe, SAHS_EffectFm = @SAHS_EffectFm, SAFS_Status = @SAFS_Status, SAHS_UpdatedUser = @SAHS_UpdatedUser, SAHS_UpdatedDtTm = @SAHS_UpdatedDtTm WHERE SAHS_Code = @SAHS_Code";
        //                if (!FormHelp.IsBlank(sqlCmd))
        //                {
        //                    DbCommand cmd = _DatabaseFactory.GetDbCommand(Helper.GetDataBaseType, sqlCmd, DataBaseConnectionString);
        //                    _DatabaseFactory.AddInParameter(ref cmd, "@SAHS_Code", DbType.String, argEn.HostelStructureCode);
        //                    _DatabaseFactory.AddInParameter(ref cmd, "@SAHB_Code", DbType.String, argEn.Code);
        //                    _DatabaseFactory.AddInParameter(ref cmd, "@SAHB_Block", DbType.String, argEn.Block);
        //                    _DatabaseFactory.AddInParameter(ref cmd, "@SAHB_RoomTYpe", DbType.String, argEn.RoomTYpe);
        //                    _DatabaseFactory.AddInParameter(ref cmd, "@SAHS_EffectFm", DbType.String, argEn.EffectFm);
        //                    _DatabaseFactory.AddInParameter(ref cmd, "@SAFS_Status", DbType.Boolean, argEn.Status);
        //                    _DatabaseFactory.AddInParameter(ref cmd, "@SAHS_UpdatedUser", DbType.String, argEn.UpdatedUser);
        //                    _DatabaseFactory.AddInParameter(ref cmd, "@SAHS_UpdatedDtTm", DbType.String, argEn.UpdatedDtTm);
        //                    _DbParameterCollection = cmd.Parameters;

        //                    int liRowAffected = _DatabaseFactory.ExecuteNonQuery(Helper.GetDataBaseType, cmd,
        //                        DataBaseConnectionString, sqlCmd, _DbParameterCollection);
                            
        //                    int i = 0;
        //                    int j = 0;
        //                    HostelStrDetailsEn loHstDet = new HostelStrDetailsEn();
        //                    HostelStrAmountEn loHstAmt = new HostelStrAmountEn();
        //                    HostelStrDetailsDAL loHstrDal = new HostelStrDetailsDAL();
        //                    HostelStrAmountDAL loHstrAmDal = new HostelStrAmountDAL();
        //                    loHstAmt.HSCode = argEn.HostelStructureCode;
        //                    loHstDet.HSCode = argEn.HostelStructureCode;
        //                    //Deleting the existing hostel feeDetails & amounts
        //                    loHstrDal.Delete(loHstDet);
        //                    loHstrAmDal.Delete(loHstAmt);
        //                    while (i < argEn.lstHFeeSD.Count)
        //                    {
        //                        //inserting new hostel feeDetails 
        //                        argEn.lstHFeeSD[i].HSCode = argEn.HostelStructureCode;
        //                        loHstrDal.Insert(argEn.lstHFeeSD[i]);
        //                        while (j < argEn.lstHFeeSD[i].ListFeeAmount.Count)
        //                        {
        //                            //inserting new hostel feeAmounts
        //                            argEn.lstHFeeSD[i].ListFeeAmount[j].HSCode = argEn.HostelStructureCode;
        //                            loHstrAmDal.Insert(argEn.lstHFeeSD[i].ListFeeAmount[j]);
        //                            j = j + 1;
        //                        }
        //                        i = i + 1;
        //                    }
        //                    if (liRowAffected > -1)
        //                        lbRes = true;
        //                    else
        //                        throw new Exception("Update Failed! No Row has been updated...");
        //                }
        //            }
        //        }
            
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //    return lbRes;
        //}

        /// <summary>
        /// Method to Update HostelStructure 
        /// </summary>
        /// <param name="argEn">HostelStructure Entity is an Input.</param>
        /// <returns>Returns Boolean</returns>
        public bool Update(HostelStructEn argEn)
        {
            bool lbRes = false;
            lbRes = Delete(argEn);
            if (lbRes)
                Insert(argEn, false);
            return lbRes;

        }

        #endregion

        #region Delete 

        /// <summary>
        /// Method to Delete HostelStructure 
        /// </summary>
        /// <param name="argEn">HostelStructure Entity is an Input.</param>
        /// <returns>Returns Boolean</returns>
        public bool Delete(HostelStructEn argEn)
        {
            bool lbRes = false;
            string sqlCmd = "DELETE FROM SAS_HostelStruct WHERE SAHS_Code = @SAHS_Code";

            try
            {
                if (!FormHelp.IsBlank(sqlCmd))
                {
                    DbCommand cmd = _DatabaseFactory.GetDbCommand(Helper.GetDataBaseType, sqlCmd, DataBaseConnectionString);
                    _DatabaseFactory.AddInParameter(ref cmd, "@SAHS_Code", DbType.String, argEn.HostelStructureCode);
                    _DbParameterCollection = cmd.Parameters;

                    HostelStrDetailsEn loHstrDet = new HostelStrDetailsEn();
                    HostelStrDetailsDAL loHstrDetDal = new HostelStrDetailsDAL();
                    HostelStrAmountEn loHstrAmt = new HostelStrAmountEn();
                    HostelStrAmountDAL loHstAmtDal = new HostelStrAmountDAL();
                    loHstrAmt.HSCode = argEn.HostelStructureCode;
                    loHstrDet.HSCode = argEn.HostelStructureCode;
                    loHstAmtDal.Delete(loHstrAmt);
                    loHstrDetDal.Delete(loHstrDet);

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
        /// Method to Load HostelStructure Entity 
        /// </summary>
        /// <param name="argReader">IDataReader Object is an Input.</param>
        /// <returns>Returns HostelStructure Entity</returns>
        private HostelStructEn LoadObject(IDataReader argReader)
        {
            HostelStructEn loItem = new HostelStructEn();
            loItem.HostelStructureCode = GetValue<string>(argReader, "SAHS_Code");
            loItem.Code = GetValue<string>(argReader, "SAHB_Code");
            loItem.Block = GetValue<string>(argReader, "SAHB_Block");
            loItem.RoomTYpe = GetValue<string>(argReader, "SAHB_RoomTYpe");
            loItem.EffectFm = GetValue<string>(argReader, "SAHS_EffectFm");
            loItem.Status = GetValue<bool>(argReader, "SAFS_Status");
            loItem.UpdatedUser = GetValue<string>(argReader, "SAHS_UpdatedUser");
            loItem.UpdatedDtTm = GetValue<string>(argReader, "SAHS_UpdatedDtTm");

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

        #region GetHostelStructList

        //Created By : Jessica
        //Created Date :  19/02/16
        ///<summary>
        ///Method to Get List of Active or Inactive Hostels
        /// </summary>
        /// <param name="argEn">HostelStructure Entity is an Input. Code,Block and Status are Input Properties.</param>
        /// <returns>Returns List of HostelStructre</returns>
        public List<HostelStructEn> GetHostelStructList(HostelStructEn argEn)
        {
            List<HostelStructEn> loEnList = new List<HostelStructEn>();

            argEn.RoomTYpe = argEn.RoomTYpe.Replace("*", "%");
            argEn.Code = argEn.Code.Replace("*", "%");
            argEn.Block = argEn.Block.Replace("*", "%");

            string sqlCmd = "select * from SAS_HostelStruct WHERE SAHS_Code <> '0'";
            if (argEn.RoomTYpe.Length != 0) sqlCmd = sqlCmd + " and SAHB_RoomTYpe like '" + argEn.RoomTYpe + "'";
            if (argEn.Code.Length != 0) sqlCmd = sqlCmd + " and SAHB_Code like '" + argEn.Code + "'";
            if (argEn.Block.Length != 0) sqlCmd = sqlCmd + " and SAHB_Block like '" + argEn.Block + "'";
            //if (argEn.Status == true) sqlCmd = sqlCmd + " and SAFS_Status = 1";
            if (argEn.Status == true) sqlCmd = sqlCmd + " and SAFS_Status = true";
            //if (argEn.Status == false) sqlCmd = sqlCmd + " and SAFS_Status = 0 ";
            if (argEn.Status == false) sqlCmd = sqlCmd + " and SAFS_Status = false ";


            try
            {
                if (!FormHelp.IsBlank(sqlCmd))
                {
                    HostelStrDetailsDAL loHostelStrDet = new HostelStrDetailsDAL();

                    using (IDataReader loReader = _DatabaseFactory.ExecuteReader(Helper.GetDataBaseType,
                        DataBaseConnectionString, sqlCmd).CreateDataReader())
                    {
                        while (loReader.Read())
                        {
                            HostelStructEn loItem = LoadObject(loReader);
                            loItem.lstHFeeWithAmt = loHostelStrDet.GetHostelDetailsAmtList(loItem);
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

        #region GetHostelStudent

        /// <summary>
        /// Method to Get List of Active or Inactive FeeTypes
        /// </summary>
        /// <param name="argEn">FeeTypes Entity is an Input.FeeTypeCode,FeeType,Hostel,Description,GLCode,Priority and Status are Input Properties.</param>
        /// <returns>Returns List of FeeTypes</returns>
        public List<HostelStructEn> GetHostelStudent(HostelStructEn argEn)
        {
            List<HostelStructEn> loEnList = new List<HostelStructEn>();


            string sqlCmd = "select * from SAS_HostelStruct hs inner join sas_student std on std.sako_code = hs.sahb_code and std.sabk_code = sahb_block where hs.sahs_code = '" + argEn.HostelStructureCode + "'";

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
                            HostelStructEn loItem = new HostelStructEn();
                            loItem.Code = GetValue<string>(loReader, "sahb_code");
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

    }

}
