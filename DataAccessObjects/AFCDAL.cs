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
    /// Business Class to handle all the AFC Transactions.
    /// </summary>
    public class AFCDAL
    {
        #region Global Declarations 

        private DbParameterCollection _DbParameterCollection = null;

        private MaxModule.DatabaseProvider _DatabaseFactory = new MaxModule.DatabaseProvider();

        private string DataBaseConnectionString = Helper.GetConnectionString();

        #endregion

        public AFCDAL()
        {
        }

        #region GetList 

        /// <summary>
        /// Method to Get List of AFC
        /// </summary>
        /// <param name="argEn">AFC Entity is an Input.</param>
        /// <returns>Returns List of AFC Entities</returns>
        public List<AFCEn> GetList(AFCEn argEn)
        {
            List<AFCEn> loEnList = new List<AFCEn>();
            argEn.SAFC_Code = argEn.SAFC_Code.Replace("*", "%");
            argEn.BatchCode = argEn.BatchCode.Replace("*", "%");
            string sqlCmd = "select * from SAS_AFC where reference = '"+argEn.Reference+"'  and safc_code <> ''";
            if (argEn.SAFC_Code.Length != 0) sqlCmd = sqlCmd + " and safc_code = '" + argEn.SAFC_Code + "'";
            if (argEn.BatchCode.Length != 0) sqlCmd = sqlCmd + " and BatchCode = '" + argEn.BatchCode + "'";
                        
            try
            {
                if (!FormHelp.IsBlank(sqlCmd))
                {
                    using (IDataReader loReader = _DatabaseFactory.ExecuteReader(Helper.GetDataBaseType,
                       DataBaseConnectionString, sqlCmd).CreateDataReader())
                    {
                        while (loReader.Read())
                        {
                            AFCDetailsDS loAFCD = new AFCDetailsDS();
                            AFCDetailsEn loAFCDEn = new AFCDetailsEn();
                            AFCEn loItem = LoadObject(loReader);
                            loAFCDEn.TransCode = loItem.TransCode;
                            loItem.AFCDetailslist = loAFCD.GetList(loAFCDEn);
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
        /// Method to Get an AFC Entity
        /// </summary>
        /// <param name="argEn">AFC Entity is an Input</param>
        /// <returns>Returns AFC Entity</returns>
        public AFCEn GetItem(AFCEn argEn)
        {
            AFCEn loItem = new AFCEn();
            string sqlCmd = "Select * FROM SAS_AFC WHERE TransCode = @TransCode";
            
            try
            {
                if (!FormHelp.IsBlank(sqlCmd))
                {
                    DbCommand cmd = _DatabaseFactory.GetDbCommand(Helper.GetDataBaseType, sqlCmd, DataBaseConnectionString);
                    _DatabaseFactory.AddInParameter(ref cmd, "@TransCode", DbType.Int32, argEn.TransCode);
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

        #region CheckAFC 

        /// <summary>
        /// Method to Get an AFC If Exists
        /// </summary>
        /// <param name="argEn">AFC Entity is an Input</param>
        /// <returns>Returns boolean</returns>
        public AFCEn CheckAFC(AFCEn argEn)
        {
            AFCEn loItem = new AFCEn();
            try
            {
                loItem.BatchCode = "";
                //Check Whether Fee Strucuture Is Exist
                AFCDetailsEn loAfcdetails = new AFCDetailsEn();
                string sem = argEn.Semester;
                for (int i = 0; i < argEn.AFCDetailslist.Count; i++)
                {
                    loAfcdetails = argEn.AFCDetailslist[i];
                    if (CheckFeeSetup(loAfcdetails, sem) == false)
                    {
                        loItem.BatchCode = "NOFEESTRUCTURE";
                        if (loItem.Reference != null)
                        {
                            loItem.Reference += "," + loAfcdetails.ProgramID;
                        }
                        else
                        {
                            loItem.Reference = loAfcdetails.ProgramID;
                        }
                        return loItem;
                    }
                }
                //Check Koko Exist For Each Student

                List<StudentEn> listStud = new List<StudentEn>();
                StudentEn studEn = new StudentEn();
                //listStud = CheckKoko(loAfcdetails, sem);
                //if (listStud.Count > 0)
                //{
                //    for (int i = 0; i < listStud.Count; i++)
                //    {
                //        studEn = listStud[i];
                //        loItem.BatchCode = "NOKOKOFEE";
                //        if (loItem.Reference != null)
                //        {
                //            loItem.Reference += "," + studEn.MatricNo;
                //        }
                //        else
                //        {
                //            loItem.Reference = studEn.MatricNo;
                //        }
                //        return loItem;
                //    }
                //}

                //Check Hostel Fee Exist For Each Student

                string semintake = argEn.Semester;

                listStud = CheckHostelFee(loAfcdetails, sem, semintake);
                if (listStud.Count > 0)
                {
                    for (int i = 0; i < listStud.Count; i++)
                    {
                        studEn = listStud[i];
                        loItem.BatchCode = "NOHOSTELFEE";
                        if (loItem.Reference != null)
                        {
                            loItem.Reference += "," + studEn.MatricNo;
                        }
                        else
                        {
                            //loItem.Reference = studEn.MatricNo;
                            loItem.Reference = studEn.ProgramID;
                        }
                        return loItem;
                    }
                }
            }
            catch (Exception ex)
            {
                //throw ex;
            }
            //if (loItem.BatchCode == "")
            //{
            //    int a = argEn.AFCDetailslist.Count;
            //    int i = 0;
            //    string prog = "";
            //    while (i < a)
            //    {
            //        if (i == 0)
            //        {
            //            prog = "'" + argEn.AFCDetailslist[i].ProgramID + "'";
            //            i++;
            //        }
            //        else
            //        {
            //            prog = ",'" + argEn.AFCDetailslist[i].ProgramID + "'";
            //            i++;
            //        }
            //    }

            //    string sqlCmd = "select * from SAS_AFC sa inner join SAS_AFCDetails sd on sa.TransCode = sd.TransCode " +
            //    " where SAFC_Code = @SAFC_Code and Semester = @Semester and sd.programcode in (" + prog + ")";
                
            //    try
            //    {
            //        if (!FormHelp.IsBlank(sqlCmd))
            //        {
            //            DbCommand cmd = _DatabaseFactory.GetDbCommand(Helper.GetDataBaseType, sqlCmd, DataBaseConnectionString);
            //            _DatabaseFactory.AddInParameter(ref cmd, "@SAFC_Code", DbType.String, argEn.SAFC_Code);
            //            //_DatabaseFactory.AddInParameter(ref cmd, "@Semester", DbType.String, argEn.Semester);
            //            _DatabaseFactory.AddInParameter(ref cmd, "@Semester", DbType.String, argEn.CurrentSemester);
            //            _DbParameterCollection = cmd.Parameters;

            //            using (IDataReader loReader = _DatabaseFactory.GetIDataReader(Helper.GetDataBaseType, cmd,
            //                DataBaseConnectionString, sqlCmd, _DbParameterCollection).CreateDataReader()) 
            //            {
            //                if (loReader != null)
            //                {
            //                    loReader.Read();
            //                    loItem = LoadObjectAFC(loReader);
            //                }
            //                loReader.Close();

            //            }
            //        }
            //    }
            //    catch (Exception ex)
            //    {
            //        //throw ex;
            //        loItem.BatchCode = "";
            //    }

            //}
            return loItem;
        }

        #endregion

        #region CheckFeeSetup 

        /// <summary>
        /// Method to Get an AFC Fee Structure If Exists
        /// </summary>
        /// <param name="argEn">AFC Entity is an Input</param>
        /// <returns>Returns boolean</returns>
        private bool CheckFeeSetup(AFCDetailsEn argEn,string sem)
        {
            FeeStructEn loItem = new FeeStructEn();
            //string sqlCmd = "select * from SAS_FeeStruct where SAPG_Code = @SAPG_Code and SAST_Code = @Semester";
            string sqlCmd = "select * from SAS_FeeStruct where SABP_Code = @SABP_Code and SAST_Code = @Semester";

            try
            {
                if (!FormHelp.IsBlank(sqlCmd))
                {
                    DbCommand cmd = _DatabaseFactory.GetDbCommand(Helper.GetDataBaseType, sqlCmd, DataBaseConnectionString);
                    //_DatabaseFactory.AddInParameter(ref cmd, "@SAPG_Code", DbType.String, argEn.ProgramID);
                    _DatabaseFactory.AddInParameter(ref cmd, "@SABP_Code", DbType.String, argEn.BidangCode);
                    _DatabaseFactory.AddInParameter(ref cmd, "@Semester", DbType.String, sem);
                    _DbParameterCollection = cmd.Parameters;

                    using (IDataReader loReader = _DatabaseFactory.GetIDataReader(Helper.GetDataBaseType, cmd,
                        DataBaseConnectionString, sqlCmd, _DbParameterCollection).CreateDataReader())
                    {
                        if (loReader != null)
                        {
                            loReader.Read();

                            loItem = LoadObjectFeeStruct(loReader);
                        }
                        loReader.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                //throw ex;
                return false;
            }
            return true;
        }

        #endregion

        #region CheckKoko 

        /// <summary>
        /// Method to Get an Check KOKO If Exists
        /// </summary>
        /// <param name="argEn">AFC Entity is an Input</param>
        /// <returns>Returns boolean</returns>
        private List<StudentEn> CheckKoko(AFCDetailsEn argEn, string sem)
        {
            string SQLSTR;
            StudentEn loItem = new StudentEn();
            StudentDAL lostuds = new StudentDAL();
            List<StudentEn> loStudEn = new List<StudentEn>();
            SQLSTR = "SELECT * from SAS_Student SS where SS.SASI_PgId='" + argEn.ProgramID + "' and SS.SASI_Faculty = '" + argEn.Faculty +
                     "' and SS.SASI_STATUSRec='1' and SS.SASI_AfcStatus='0' and SS.SASI_CurSemYr = '" + sem + "' order by SS.SASI_PgId";
            
            try
            {
           
              if(!FormHelp.IsBlank(SQLSTR))
              {
                   using (IDataReader loReader = _DatabaseFactory.ExecuteReader(Helper.GetDataBaseType,
                        DataBaseConnectionString, SQLSTR).CreateDataReader())
                  {
                      //Student Loop
                      while (loReader.Read())
                         {
                            loItem = lostuds.LoadObject(loReader);
                            //if (loItem.KokoCode == "-1" || loItem.KokoCode == null || loItem.KokoCode == "")
                            //{
                            //}
                            //else
                            //{
                            //    if (CheckAmountKoko(loItem.MatricNo) == false)
                            //    {
                                    loStudEn.Add(loItem);
                            //    }
                            //}
                         }
                      loReader.Close();
                   }
               }
             }
             catch (Exception ex)
             {
                    //throw ex;
             }
            return loStudEn;
        }

        #endregion

        #region CheckHostelFee 

        /// <summary>
        /// Method to Get an Check KOKO If Exists
        /// </summary>
        /// <param name="argEn">AFC Entity is an Input</param>
        /// <returns>Returns boolean</returns>
        private List<StudentEn> CheckHostelFee(AFCDetailsEn argEn, string sem, string intake)
        {
            string SQLSTR;
            StudentEn loItem = new StudentEn();
            StudentDAL lostuds = new StudentDAL();
            List<StudentEn> loStudEn = new List<StudentEn>();
            SQLSTR = "SELECT * from SAS_Student SS where SS.SASI_PgId='" + argEn.ProgramID + "' and SS.SASI_Faculty = '" + argEn.Faculty +
                     "' and SS.SASI_STATUSRec='1' and SS.SASI_AfcStatus='0' and SS.SASI_CurSemYr = '" + sem + "' and SS.SASI_Intake = '" + intake + 
                     "' and sasi_hostel = '1' order by SS.SASI_PgId";
            
            try
            {
                if (!FormHelp.IsBlank(SQLSTR))
                {
                    using (IDataReader loReader = _DatabaseFactory.ExecuteReader(Helper.GetDataBaseType,
                        DataBaseConnectionString, SQLSTR).CreateDataReader())
                    {
                        //Student Loop
                        while (loReader.Read())
                        {
                            loItem = lostuds.LoadObject(loReader);
                            if (CheckAmountHostel(loItem.MatricNo) == false)
                            {
                                loStudEn.Add(loItem);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                //throw ex;
            }
            return loStudEn;
        }

        #endregion

        #region CheckAmountHostel 

        /// <summary>
        /// Method to Check Amount Hostel If Exists
        /// </summary>
        /// <param name="argEn">AFC Entity is an Input</param>
        /// <returns>Returns boolean</returns>
        private bool CheckAmountHostel(string matricNo)
        {
            double totalAll;
            string sqlCmdHostel = "Select SAHA_Amount from SAS_Student SS,SAS_HostelStruct SH,SAS_HostelStrAmount SA," +
                                "SAS_FeeTypes SF where SASI_Hostel = '1' and SASI_MatricNo = @MatricNo and (SS.SAKO_Code = SH.SAHB_Code and SS.SABK_Code = SH.SAHB_Block " +
                                "and SS.SART_Code = SH.SAHB_RoomTYpe) and SH.SAHS_Code = SA.SAHS_Code and SS.SASI_FeeCat = SA.SASC_Code and SF.SAFT_Code = SA.SAFT_Code";
            
            try
            {
                if (!FormHelp.IsBlank(sqlCmdHostel))
                {
                    DbCommand cmd = _DatabaseFactory.GetDbCommand(Helper.GetDataBaseType, sqlCmdHostel, DataBaseConnectionString);
                    _DatabaseFactory.AddInParameter(ref cmd, "@MatricNo", DbType.String, matricNo);
                    _DbParameterCollection = cmd.Parameters;

                    using (IDataReader loReaderHostel = _DatabaseFactory.GetIDataReader(Helper.GetDataBaseType, cmd,
                        DataBaseConnectionString, sqlCmdHostel, _DbParameterCollection).CreateDataReader()) 
                    {
                        loReaderHostel.Read();
                        if (loReaderHostel != null)
                        {
                            AFCEn loHostel = new AFCEn();
                            loHostel.TransAmount = GetValue<double>(loReaderHostel, "SAHA_Amount");
                            totalAll = loHostel.TransAmount;
                        }
                        loReaderHostel.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                //throw ex;
                return false;
            }
            return true;
        }

        #endregion

        #region CheckAmountKoko 

        /// <summary>
        /// Method to Check Amount Koko If Exists
        /// </summary>
        /// <param name="argEn">AFC Entity is an Input</param>
        /// <returns>Returns boolean</returns>
        private bool CheckAmountKoko(string matricNo)
        {
            string sqlCmd = "select SK.SAKO_Code As SAKO_Code,SK.SAKO_Description as SAKO_Description,(SKD.SAKOD_FeeAmount * SK.SAKO_CreditHours) as SAHA_AMOUNT from " +
                            "SAS_Student SS,SAS_Kokorikulum SK,SAS_KokorikulumDetails SKD where SASI_MatricNo = @MatricNo and SASI_KokoCode <> 'null' " +
                            "and SS.SASI_FeeCat = SKD.SAKOD_CategoryCode and SS.SASI_KokoCode = SK.SAKO_Code and SKD.SAKO_Code = SK.SAKO_Code";
            
            try
            {
                if (!FormHelp.IsBlank(sqlCmd))
                {
                    DbCommand cmd = _DatabaseFactory.GetDbCommand(Helper.GetDataBaseType, sqlCmd, DataBaseConnectionString);
                    _DatabaseFactory.AddInParameter(ref cmd, "@MatricNo", DbType.String, matricNo);
                    _DbParameterCollection = cmd.Parameters;

                    using (IDataReader loReaderKoko = _DatabaseFactory.GetIDataReader(Helper.GetDataBaseType, cmd,
                        DataBaseConnectionString, sqlCmd, _DbParameterCollection).CreateDataReader()) 
                    {
                        loReaderKoko.Read();
                        if (loReaderKoko != null)
                        {
                            AFCEn loKoko = new AFCEn();
                            loKoko.TransAmount = GetValue<double>(loReaderKoko, "SAHA_Amount");
                        }
                        loReaderKoko.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                //throw ex;
                return false;
            }
            return true;
        }

        #endregion

        #region GetItemJBilling 

        /// <summary>
        /// Method to Get an AFC Entity From SQL
        /// </summary>
        /// <param name="argEn">AFC Entity is an Input</param>
        /// <returns>Returns AFC Entity</returns>
        public AFCEn GetItemJBilling(string batch)
        {
            bool lbRes = false;
            AFCEn loItem = new AFCEn();
            string sqlCmd = "select distinct CreditRef,TransID,SASI_Name,SAS_Accounts.TransCode,SAS_Accounts.Description,Category," +
            "TransType,TransAmount,SAS_AFC.BatchCode, SAS_Accounts.transdate from SAS_Accounts inner join SAS_AFC on SAS_Accounts.BatchCode = " +
            "SAS_AFC.BatchCode inner join SAS_Student on SAS_Accounts.CreditRef = SAS_Student.SASI_MatricNo where " +
            "SAS_AFC.BatchCode = @batchCode and SAS_Accounts.PostStatus = 'Ready'";
            
            try
            {
                if (!FormHelp.IsBlank(sqlCmd))
                {
                    DbCommand cmd = _DatabaseFactory.GetDbCommand(Helper.GetDataBaseType, sqlCmd, DataBaseConnectionString);
                    _DatabaseFactory.AddInParameter(ref cmd, "@batchCode", DbType.String, batch);
                    _DbParameterCollection = cmd.Parameters;

                    using (IDataReader loReader = _DatabaseFactory.GetIDataReader(Helper.GetDataBaseType, cmd,
                        DataBaseConnectionString, sqlCmd, _DbParameterCollection).CreateDataReader()) 
                    {
                        if (loReader != null)
                        {
                            while (loReader.Read())
                            {
                                loItem = LoadObjectAFCPosted(loReader);
                                InsertJBitBilling(loItem, batch);
                            }
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

        #region CheckNewStudentAFC 

        public string CheckNewStudentAFC(AFCEn argEn)
        {
            bool lbRes = false;
            AFCEn loItem = new AFCEn();
            int a = argEn.AFCDetailslist.Count;
            int i = 0;
            string prog = "";
            string matricNo=string.Empty;
            while (i < a)
            {
                if (i == 0)
                {
                    prog = "'" + argEn.AFCDetailslist[i].ProgramID + "'";
                    i++;
                }
                else
                {
                    prog = ",'" + argEn.AFCDetailslist[i].ProgramID + "'";
                    i++;
                }
            }
            string sqlCmd = @"SELECT ST.SASI_MATRICNO FROM SAS_STUDENT ST WHERE ST.SASI_PostStatus= '0' AND ST.SASI_PGID IN (" + prog + ") AND ST.SASI_CURSEMYR='" + argEn.Semester + "'";
            
            try
            {
                if (!FormHelp.IsBlank(sqlCmd))
                {
                    using (IDataReader loReader = _DatabaseFactory.ExecuteReader(Helper.GetDataBaseType,
                        DataBaseConnectionString, sqlCmd).CreateDataReader())
                    {
                        if (loReader != null)
                        {
                            while (loReader.Read())
                            {
                                matricNo = loReader["SASI_MATRICNO"].ToString();
                            }
                        }
                        loReader.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return matricNo;
        }

        #endregion

        #region Insert 

        /// <summary>
        /// Method to Insert 
        /// </summary>
        /// <param name="argEn">AFC Entity is an Input.</param>
        /// <returns>Returns Boolean</returns>
        public bool Insert(AFCEn argEn)
        {
            bool lbRes = false;
            string batch;
                try
                {     
                    string sqlCmd = "INSERT INTO SAS_AFC(AFCode,SAFC_Code,BatchCode,Semester,Description,Bdate,TransDate,";
                     sqlCmd += "DueDate,BatchTotal,PostedFor,Reference,Updatedby,Uptimestamp) VALUES (@AFCode,@SAFC_Code,";
                     sqlCmd += "@BatchCode,@Semester,@Description,@Bdate,@TransDate,@DueDate,@BatchTotal,@PostedFor,@Reference,";
                     sqlCmd += "@Updatedby,@Updatetime); Select max(transcode) from SAS_AFC ";

                    if (!FormHelp.IsBlank(sqlCmd))
                    {
                        DbCommand cmd = _DatabaseFactory.GetDbCommand(Helper.GetDataBaseType, sqlCmd, DataBaseConnectionString);
                        argEn.AFCode = GetAutoNumber("AC");
                        _DatabaseFactory.AddInParameter(ref cmd, "@AFCode", DbType.String, argEn.AFCode);
                        _DatabaseFactory.AddInParameter(ref cmd, "@SAFC_Code", DbType.String, argEn.SAFC_Code);
                        _DatabaseFactory.AddInParameter(ref cmd, "@BatchCode", DbType.String, argEn.BatchCode);
                        _DatabaseFactory.AddInParameter(ref cmd, "@Semester", DbType.String, argEn.CurrentSemester);
                        _DatabaseFactory.AddInParameter(ref cmd, "@Description", DbType.String, argEn.Description);
                        _DatabaseFactory.AddInParameter(ref cmd, "@Bdate", DbType.DateTime, Helper.DateConversion(argEn.Bdate));
                        _DatabaseFactory.AddInParameter(ref cmd, "@TransDate", DbType.DateTime, Helper.DateConversion(argEn.TransDate));
                        _DatabaseFactory.AddInParameter(ref cmd, "@DueDate", DbType.DateTime, Helper.DateConversion(argEn.DueDate));
                        _DatabaseFactory.AddInParameter(ref cmd, "@BatchTotal", DbType.Double, argEn.BatchTotal);
                        _DatabaseFactory.AddInParameter(ref cmd, "@PostedFor", DbType.String, argEn.PostedFor);
                        _DatabaseFactory.AddInParameter(ref cmd, "@Reference", DbType.String, argEn.Reference);
                        _DatabaseFactory.AddInParameter(ref cmd, "@Updatedby", DbType.String, argEn.Updatedby);
                        _DatabaseFactory.AddInParameter(ref cmd, "@Updatetime", DbType.String, Helper.DateConversion(Convert.ToDateTime(argEn.Updatetime)));
                        _DbParameterCollection = cmd.Parameters;

                        argEn.TransCode = clsGeneric .NullToInteger(_DatabaseFactory.ExecuteScalarCommand(Helper.GetDataBaseType, cmd,
                                DataBaseConnectionString, sqlCmd, _DbParameterCollection));

                        if (argEn.TransCode >= 0)
                        {
                            if (argEn.AFCDetailslist != null)
                            {
                                if (argEn.AFCDetailslist.Count != 0)
                                {
                                    AFCDetailsDS loDS = new AFCDetailsDS();
                                    for (int i = 0; i < argEn.AFCDetailslist.Count; i++)
                                    {
                                        argEn.AFCDetailslist[i].TransCode = argEn.TransCode;                                        
                                        loDS.Insert(argEn.AFCDetailslist[i]);
                                    }
                                }
                            }
                            lbRes = true;
                        }
                            
                        else
                        {
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

        #region InsertJBitBilling 

        /// <summary>
        /// Method to Insert Data into Jbit Billing Table
        /// </summary>
        /// <param name="loItem">AFC Entity is an Input.</param>
        /// <returns>Returns Boolean</returns>
        public bool InsertJBitBilling(AFCEn loItem,string batch)
        {
            bool lbRes = false;
            double totalAmount = 0.0;
            try
            {
                //Start Insert Hostel Details
                string sqlCmdHostel = "select SA.SAFT_Code As SAFT_Code,SF.SAFT_Desc as SAFT_Desc,SAHA_Amount from SAS_Student SS,SAS_HostelStruct SH,SAS_HostelStrAmount SA," +
                    "SAS_FeeTypes SF where SASI_Hostel = '1' and SASI_MatricNo = @MatricNo and (SS.SAKO_Code = SH.SAHB_Code and SS.SABK_Code = SH.SAHB_Block " +
                    "and SS.SART_Code = SH.SAHB_RoomTYpe) and SH.SAHS_Code = SA.SAHS_Code and SS.SASI_FeeCat = SA.SASC_Code and SF.SAFT_Code = SA.SAFT_Code";
                
                try
                {
                    if (!FormHelp.IsBlank(sqlCmdHostel))
                    {
                        DbCommand cmdHostel = _DatabaseFactory.GetDbCommand(Helper.GetDataBaseType, sqlCmdHostel, DataBaseConnectionString);
                        _DatabaseFactory.AddInParameter(ref cmdHostel, "@MatricNo", DbType.String, loItem.CreditRef);
                        _DbParameterCollection = cmdHostel.Parameters;

                        using (IDataReader loReaderHostel = _DatabaseFactory.GetIDataReader(Helper.GetDataBaseType, cmdHostel,
                            DataBaseConnectionString, sqlCmdHostel, _DbParameterCollection).CreateDataReader()) 
                        {
                            loReaderHostel.Read();
                            if (loReaderHostel != null)
                            {
                                AFCEn loHostel = new AFCEn();
                                loHostel.BatchCode = batch;
                                loHostel.SAFT_Code = GetValue<string>(loReaderHostel, "SAFT_Code");
                                loHostel.SAFT_Desc = GetValue<string>(loReaderHostel, "SAFT_Desc");
                                loHostel.TransAmount = GetValue<double>(loReaderHostel, "SAHA_Amount");
                                loHostel.AFCode = loItem.SAFC_Code;
                                InsertJBitBillingDetails(loHostel);
                                totalAmount = loHostel.TransAmount;
                            }
                            loReaderHostel.Close();
                        }
                    }
                }
                catch (Exception ex)
                {
                    //throw ex;
                }


                //Start Insert KOKO Details
                string sqlCmdKoko = "select SK.SAKO_Code As SAKO_Code,SK.SAKO_Description as SAKO_Description,(SKD.SAKOD_FeeAmount * SK.SAKO_CreditHours) as SAHA_AMOUNT from " +
                "SAS_Student SS,SAS_Kokorikulum SK,SAS_KokorikulumDetails SKD where SASI_MatricNo = @MatricNo and SASI_KokoCode <> 'null' " +
                "and SS.SASI_FeeCat = SKD.SAKOD_CategoryCode and SS.SASI_KokoCode = SK.SAKO_Code and SKD.SAKO_Code = SK.SAKO_Code";
                                               
                try
                {
                    if (!FormHelp.IsBlank(sqlCmdKoko))
                    {
                        DbCommand cmdKoko = _DatabaseFactory.GetDbCommand(Helper.GetDataBaseType, sqlCmdKoko, DataBaseConnectionString);
                        _DatabaseFactory.AddInParameter(ref cmdKoko, "@MatricNo", DbType.String, loItem.CreditRef);
                        _DbParameterCollection = cmdKoko.Parameters;

                        using (IDataReader loReaderKoko = _DatabaseFactory.GetIDataReader(Helper.GetDataBaseType, cmdKoko,
                            DataBaseConnectionString, sqlCmdKoko, _DbParameterCollection).CreateDataReader()) 
                        {
                            loReaderKoko.Read();
                            if (loReaderKoko != null)
                            {
                                AFCEn loKoko = new AFCEn();
                                loKoko.BatchCode = batch;
                                loKoko.SAFT_Code = GetValue<string>(loReaderKoko, "SAKO_Code");
                                loKoko.SAFT_Desc = GetValue<string>(loReaderKoko, "SAKO_Description");
                                loKoko.TransAmount = GetValue<double>(loReaderKoko, "SAHA_Amount");
                                loKoko.AFCode = loItem.SAFC_Code;
                                InsertJBitBillingDetails(loKoko);

                                _DatabaseFactory.ExecuteNonQuery(Helper.GetDataBaseType, cmdKoko,
                                    DataBaseConnectionString, sqlCmdKoko, _DbParameterCollection);

                                totalAmount += loKoko.TransAmount;
                            }
                            loReaderKoko.Close();
                        }
                    }
                }
                catch (Exception ex)
                {

                    //throw ex;
                }//End Insert KOKO Details

                string sqlCmd = "INSERT INTO jbit_billing(Sb_MatricNo,Sb_Name,Sb_TransID,Sb_TransDate,Sb_TransCode,Sb_Description, " +
                "Sb_Category,Sb_Transtype,Sb_TransAmount,Sb_Flag,Sb_BatchCode) VALUES (@Sb_MatricNo,@Sb_Name,@Sb_TransID,@Sb_TransDate,@Sb_TransCode,@Sb_Description, " +
                "@Sb_Category,@Sb_Transtype,@Sb_TransAmount,'0',@Sb_BatchCode)select @@identity ";

                if (!FormHelp.IsBlank(sqlCmd))
                {
                    DbCommand cmd = _DatabaseFactory.GetDbCommand(Helper.GetDataBaseType, sqlCmd, DataBaseConnectionString);
                    _DatabaseFactory.AddInParameter(ref cmd, "@Sb_MatricNo", DbType.String, loItem.CreditRef);
                    _DatabaseFactory.AddInParameter(ref cmd, "@Sb_Name", DbType.String, loItem.SASI_Name);
                    _DatabaseFactory.AddInParameter(ref cmd, "@Sb_TransID", DbType.Int32, loItem.TransID);
                    _DatabaseFactory.AddInParameter(ref cmd, "@Sb_TransDate", DbType.DateTime, loItem.TransDate);
                    _DatabaseFactory.AddInParameter(ref cmd, "@Sb_TransCode", DbType.String, loItem.TransCodeAFC);
                    _DatabaseFactory.AddInParameter(ref cmd, "@Sb_Description", DbType.String, loItem.DescriptionAFC);
                    _DatabaseFactory.AddInParameter(ref cmd, "@Sb_Category", DbType.String, loItem.Category);
                    _DatabaseFactory.AddInParameter(ref cmd, "@Sb_Transtype", DbType.String, loItem.TransType);
                    _DatabaseFactory.AddInParameter(ref cmd, "@Sb_TransAmount", DbType.Double, loItem.TransAmount);
                    _DatabaseFactory.AddInParameter(ref cmd, "@Sb_BatchCode", DbType.String, batch);
                    _DbParameterCollection = cmd.Parameters;

                    int liRowAffected = _DatabaseFactory.ExecuteNonQuery(Helper.GetDataBaseType, cmd,
                                DataBaseConnectionString, sqlCmd, _DbParameterCollection);

                    if (liRowAffected > -1)
                    {
                        lbRes = true;
                        AFCEn loDetails = new AFCEn();
                        string sqlCmdDetails = "Select distinct SAS_Accounts.BatchCode,SAS_FeeTypes.SAFT_Code,SAS_FeeTypes.SAFT_Desc,SAS_FeeStrAmount.SAFA_Amount " +
                        " from SAS_Accounts inner join SAS_AccountsDetails on SAS_Accounts.TransID = SAS_AccountsDetails.TransID " +
                        " inner join SAS_FeeTypes on SAS_FeeTypes.SAFT_Code = SAS_AccountsDetails.RefCode inner join SAS_Student on SAS_Student.SASI_MatricNo = SAS_Accounts.CreditRef " +
                        " inner join SAS_AFC on SAS_AFC.BatchCode = SAS_Accounts.BatchCode inner join SAS_FeeStrAmount on (SAS_FeeStrAmount.SAFT_Code = SAS_FeeTypes.SAFT_Code and " +
                        " SAS_Student.SASC_Code = SAS_FeeStrAmount.SASC_Code) where SAS_Accounts.BatchCode = @batchCode and SAS_Accounts.PostStatus = 'Ready'";
                        
                        try
                        {
                            if (!FormHelp.IsBlank(sqlCmdDetails))
                            {
                                DbCommand cmdDetails = _DatabaseFactory.GetDbCommand(Helper.GetDataBaseType, sqlCmdDetails, DataBaseConnectionString);
                                _DatabaseFactory.AddInParameter(ref cmdDetails, "@batchCode", DbType.String, batch);
                                _DbParameterCollection = cmd.Parameters;

                                using (IDataReader loReader = _DatabaseFactory.GetIDataReader(Helper.GetDataBaseType, cmdDetails,
                                    DataBaseConnectionString, sqlCmdDetails, _DbParameterCollection).CreateDataReader())
                                {
                                    if (loReader != null)
                                    {
                                        while (loReader.Read())
                                        {
                                            loDetails = LoadObjectAFCDetails(loReader);
                                            InsertJBitBillingDetails(loDetails);
                                        }
                                    }
                                    loReader.Close();
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            
                            throw ex;
                        }
                        
                        return lbRes;
                    }
                    else
                    { 
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

        #region InsertJBitBillingDetails 

        /// <summary>
        /// Method to Insert Data into Jbit Billing Details Table
        /// </summary>
        /// <param name="loItem">AFC Entity is an Input.</param>
        /// <returns>Returns Boolean</returns>
        public bool InsertJBitBillingDetails(AFCEn loDetails)
        {
            bool lbRes = false;
            try
            {
                string sqlCmd = "INSERT INTO SAS_BillingDetails(SBD_ID,SBD_BatchCode,SAFT_Desc,AFC_Code,SAFT_Code,SBD_TransAmount) " +
                    "VALUES (@SBD_ID,@SBD_BatchCode,@SAFT_Desc,@AFC_Code,@SAFT_Code,@SBD_TransAmount)select @@identity ";

                if (!FormHelp.IsBlank(sqlCmd))
                {
                    DbCommand cmd = _DatabaseFactory.GetDbCommand(Helper.GetDataBaseType, sqlCmd, DataBaseConnectionString);
                    _DatabaseFactory.AddInParameter(ref cmd, "@SBD_ID", DbType.String, loDetails.TransID);
                    _DatabaseFactory.AddInParameter(ref cmd, "@SBD_BatchCode", DbType.String, loDetails.BatchCode);
                    _DatabaseFactory.AddInParameter(ref cmd, "@SAFT_Desc", DbType.String, loDetails.SAFT_Desc);
                    _DatabaseFactory.AddInParameter(ref cmd, "@AFC_Code", DbType.String, loDetails.AFCode);
                    _DatabaseFactory.AddInParameter(ref cmd, "@SAFT_Code", DbType.String, loDetails.SAFT_Code);
                    _DatabaseFactory.AddInParameter(ref cmd, "@SBD_TransAmount", DbType.Double, loDetails.TransAmount);
                    _DbParameterCollection = cmd.Parameters;

                    int liRowAffected = _DatabaseFactory.ExecuteNonQuery(Helper.GetDataBaseType, cmd,
                                DataBaseConnectionString, sqlCmd, _DbParameterCollection);

                    if (liRowAffected > -1)
                        lbRes = true;
                    else
                        throw new Exception("Insertion Billing Details Failed! No Row has been updated...");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return lbRes;
        }

        #endregion

        #region BatchDelete 

        /// <summary>
        /// Method to Delete Batch 
        /// </summary>
        /// <param name="argEn">AFC Entity is an Input.</param>
        /// <returns>Returns Boolean</returns>
        public bool BatchDelete(AFCEn argEn,string Delete)
        {
            bool receff = false;
            AccountsEn loendelete = new AccountsEn();
            AccountsDAL lodsdelete = new AccountsDAL();
            StudentEn loItemStud = new StudentEn();
            StudentDAL lodsStud = new StudentDAL();
            loendelete.BatchCode = argEn.BatchCode;
            if (Delete == "Delete")
            {
                lodsdelete.GetListByBatch(loendelete.BatchCode, "Ready");
                lodsdelete.BatchDelete(loendelete,true);
                receff = AFCBatchDelete(argEn);
            }
            else
            {
                lodsdelete.BatchDelete(loendelete);
                receff = AFCBatchDelete(argEn);
            }
           return receff;
        }

        #endregion

        #region AFCNEW

        public string AFCNEW(AFCEn argEn, string RecStatus, string batch)
        {
            string Batchcode = "";
            bool student = false;
            bool item = false;
            bool IsRecordInserted = false;
            //deleting saved record and inserting a new one for update
            if ((batch != "") && (RecStatus == "Posted") && argEn.PostStatus == "1")
            {
                //GetItemJBilling(batch);
                AccountsEn loendelete = new AccountsEn();
                AccountsDAL lodsdelete = new AccountsDAL();
                AFCEn loafc = new AFCEn();
                lodsdelete.GetListByBatch(batch, "Posted");
                loendelete.BatchCode = batch;
                loafc.BatchCode = batch;
                lodsdelete.BatchDelete(loendelete);
                AFCBatchDelete(loafc);
                student = true;
                item = true;
            }

            ProgramInfoEn loprogram = new ProgramInfoEn();
            List<ProgramInfoEn> listprogram = new List<ProgramInfoEn>();
            AFCDetailsEn loAfcdetails = new AFCDetailsEn();
            AccountsEn loen = new AccountsEn();
            AccountsDetailsEn lodetail = new AccountsDetailsEn();
            AccountsDAL lods = new AccountsDAL();
            StudentDAL loStd = new StudentDAL();
            try
            {
                loen = new AccountsEn();
                loen.Category = "AFC";
                loen.TransType = "Debit";
                loen.SubType = "Student";
                if (RecStatus == "Posted")
                {
                    loen.PostStatus = "Posted";
                    loen.BatchCode = batch;
                }
                else
                {
                    loen.PostStatus = "Ready";

                }

                if (batch == "" || batch == "&nbsp;")
                {
                    // Modified by Syafiq 07 April 2014
                    string CheckBatchNumber;
                    CheckBatchNumber = GetAutoNumber("Batch");
                    if (CheckBatchNumber != "Auto Number")
                    {
                        batch = CheckBatchNumber;
                    }
                    else
                    {
                        return "Error in Batch Number";
                    }
                }
                else
                {
                    String bc = batch;
                    batch = bc;
                }

                loen.TransStatus = "Open";
                loen.TransDate = argEn.TransDate;
                loen.DueDate = argEn.DueDate;
                loen.BatchDate = argEn.Bdate;
                loen.ChequeDate = DateTime.Now;
                loen.PostedDateTime = DateTime.Now;
                loen.UpdatedTime = DateTime.Now;
                loen.CreatedDateTime = DateTime.Now;
                loen.Description = argEn.Description;

                // Modified by Syafiq 10 Feb 2014
                loen.CreatedBy = argEn.Updatedby;
                loen.UpdatedBy = argEn.Updatedby;

                //Program Loop
                for (int i = 0; i < argEn.AFCDetailslist.Count; i++)
                {
                    loAfcdetails = argEn.AFCDetailslist[i];

                    using (IDataReader loReader = _DatabaseFactory.GetStudentDetails(loen, loAfcdetails.ProgramID, argEn.SAFC_Code, argEn.PostStatus, argEn.CurrentSemester, 
                        argEn.Semester, argEn.CreditRef, RecStatus, batch).CreateDataReader())
                    {
                        while (loReader.Read())
                        {
                            loen.CreditRef = GetValue<string>(loReader, "output_CreditRef");
                            loen.MatricNo = GetValue<string>(loReader, "output_MatricNo");
                            loen.TransactionAmount = Convert.ToDouble(GetValue<decimal>(loReader, "output_TransactionAmount"));
                            loen.BatchCode = GetValue<string>(loReader, "output_BatchCode");
                            loen.TempTransCode = GetValue<string>(loReader, "output_TempTransCode");

                            student = true;

                            string sqlcmd = "SELECT sa.transid as transsid,* FROM SAS_accountsdetails sad inner join sas_accounts sa on sad.transtempcode = sa.transtempcode WHERE " +
                            "sad.transtempcode = '" + loen.TempTransCode + "' AND sad.transid = 0 And sa.batchcode = '" + loen.BatchCode + "'";

                            if (!FormHelp.IsBlank(sqlcmd))
                            {
                                using (IDataReader io = _DatabaseFactory.ExecuteReader(Helper.GetDataBaseType,
                                    DataBaseConnectionString, sqlcmd).CreateDataReader())
                                {
                                    while (io.Read())
                                    {
                                        StudentEn loItem = new StudentEn();

                                        loen.TransactionID = GetValue<int>(io, "transsid");

                                        if (loen.TransactionID != 0)
                                        {
                                            string sqlcmd1 = "UPDATE SAS_accountsdetails SET transid = '" + loen.TransactionID + "' WHERE transtempcode = '" + loen.TempTransCode + "' AND transid = 0";

                                            if (!FormHelp.IsBlank(sqlcmd1))
                                            {
                                                using (IDataReader dr = _DatabaseFactory.ExecuteReader(Helper.GetDataBaseType,
                                                    DataBaseConnectionString, sqlcmd1).CreateDataReader())
                                                {

                                                }
                                            }
                                        }


                                    }
                                    io.Close();
                                }
                            }

                            IsRecordInserted = true;

                            //insert into SAS_LogAutoNumber - start
                            InsertIntoSASLogAutoNumber(loen);
                            //insert into SAS_LogAutoNumber - end

                            if (loen.PostStatus == "Posted")
                            {
                                AccountsDAL account = new AccountsDAL();
                                account.UpdateStudentOutstanding(loen.CreditRef);
                            }

                            if (RecStatus == "Posted")
                            {
                                UpdateStudentAFCStatus(loen.CreditRef, argEn.CurrentSemester, argEn.AFCDetailslist[0].ProgramID, 2, loen.MatricNo);
                            }
                            else
                            {
                                UpdateStudentAFCStatus(loen.CreditRef, argEn.CurrentSemester, argEn.AFCDetailslist[0].ProgramID, 1, loen.MatricNo);
                            }
                        }
                    }

                    if (student == false)
                    {
                        throw new Exception("No Student in Current Semester in Setup Structure ");
                    }
                    if (!IsRecordInserted)
                    {
                        throw new Exception("Record Insertion failed Kindly check the settings ");
                    }
                    else
                    {
                        //Insert AFC
                        argEn.BatchCode = batch;
                        Batchcode = batch;
                        bool resInsert = false;
                        resInsert = Insert(argEn);
                    }
                }

            }
            catch (Exception ex)
            {

                throw ex;
            }

            return Batchcode;
        }

        #endregion

        #region InsertIntoSASLogAutoNumber
        //added by Hafiz @ 02/11/2016
        //Insert AFC into LOG Table for Future Use - taken from SAS_Accounts

        public void InsertIntoSASLogAutoNumber(AccountsEn argEn)
        {
            try
            {
                string sqlCmdlog = "INSERT INTO SAS_LogAutoNumber(BatchNo,TransactionNo,Status,Category,CreatedBy,Createdon) " +
                "VALUES (@BatchNo,@TransactionNo,@Status,@Category,@CreatedBy,@Createdon)";

                if (!FormHelp.IsBlank(sqlCmdlog))
                {
                    DbCommand cmdlog = _DatabaseFactory.GetDbCommand(Helper.GetDataBaseType, sqlCmdlog, DataBaseConnectionString);
                    _DatabaseFactory.AddInParameter(ref cmdlog, "@BatchNo", DbType.String, argEn.BatchCode);
                    _DatabaseFactory.AddInParameter(ref cmdlog, "@Status", DbType.String, argEn.PostStatus);


                    if (argEn.PostStatus == "Ready")
                    {
                        _DatabaseFactory.AddInParameter(ref cmdlog, "@TransactionNo", DbType.String, argEn.TempTransCode);
                    }
                    else
                    {
                        _DatabaseFactory.AddInParameter(ref cmdlog, "@TransactionNo", DbType.String, argEn.TransactionCode);
                    }
                    _DatabaseFactory.AddInParameter(ref cmdlog, "@Category", DbType.String, argEn.Category);
                    if (string.IsNullOrEmpty(argEn.CreatedBy))
                    {
                        _DatabaseFactory.AddInParameter(ref cmdlog, "@CreatedBy", DbType.String, argEn.UpdatedBy);
                    }
                    else
                    {
                        _DatabaseFactory.AddInParameter(ref cmdlog, "@CreatedBy", DbType.String, argEn.CreatedBy);
                    }
                    _DatabaseFactory.AddInParameter(ref cmdlog, "@Createdon", DbType.String, DateTime.Now.ToString("yyyy/MM/dd"));
                    _DbParameterCollection = cmdlog.Parameters;

                    int liRowAffected = _DatabaseFactory.ExecuteNonQuery(Helper.GetDataBaseType, cmdlog,
                        DataBaseConnectionString, sqlCmdlog, _DbParameterCollection);
                }
            }
            catch (Exception ex)
            {
                MaxModule.Helper.LogError(ex.Message);
                throw ex;
            }
        }

        #endregion

        #region Update

        /// <summary>
        /// Method to Update 
        /// </summary>
        /// <param name="argEn">AFC Entity is an Input.</param>
        /// <returns>Returns Boolean</returns>
        public bool Update(AFCEn argEn)
        {
            bool lbRes = false;
            String batch;
            try
            {
                string sqlCmd = "UPDATE SAS_AFC SET TransCode = @TransCode, AFCode = @AFCode, SAFC_Code = @SAFC_Code, BatchCode = @BatchCode, Description = @Description, Bdate = @Bdate, TransDate = @TransDate, DueDate = @DueDate,BatchTotal = @BatchTotal, PostedFor = @PostedFor, Reference = @Reference, Updatedby = @Updatedby, Updatetime = @Updatetime WHERE TransCode = @TransCode";

                if (!FormHelp.IsBlank(sqlCmd))
                {
                    DbCommand cmd = _DatabaseFactory.GetDbCommand(Helper.GetDataBaseType, sqlCmd, DataBaseConnectionString);
                    _DatabaseFactory.AddInParameter(ref cmd, "@TransCode", DbType.Int32, argEn.TransCode);
                    _DatabaseFactory.AddInParameter(ref cmd, "@AFCode", DbType.String, argEn.AFCode);
                    _DatabaseFactory.AddInParameter(ref cmd, "@SAFC_Code", DbType.String, argEn.SAFC_Code);
                    _DatabaseFactory.AddInParameter(ref cmd, "@BatchCode", DbType.String, argEn.BatchCode);
                    _DatabaseFactory.AddInParameter(ref cmd, "@Description", DbType.String, argEn.Description);
                    _DatabaseFactory.AddInParameter(ref cmd, "@Bdate", DbType.DateTime, argEn.Bdate);
                    _DatabaseFactory.AddInParameter(ref cmd, "@TransDate", DbType.DateTime, argEn.TransDate);
                    _DatabaseFactory.AddInParameter(ref cmd, "@DueDate", DbType.DateTime, argEn.DueDate);
                    _DatabaseFactory.AddInParameter(ref cmd, "@PostedFor", DbType.String, argEn.PostedFor);
                    _DatabaseFactory.AddInParameter(ref cmd, "@BatchTotal", DbType.Double, argEn.BatchTotal);
                    _DatabaseFactory.AddInParameter(ref cmd, "@Reference", DbType.String, argEn.Reference);
                    _DatabaseFactory.AddInParameter(ref cmd, "@Updatedby", DbType.String, argEn.Updatedby);
                    _DatabaseFactory.AddInParameter(ref cmd, "@Updatetime", DbType.String, argEn.Updatetime);
                    _DbParameterCollection = cmd.Parameters;

                    argEn.AFCode = GetAutoNumber("AC");
                    argEn.BatchCode = GetAutoNumber("Batch");
                    batch = argEn.BatchCode;

                    int lb = _DatabaseFactory.ExecuteNonQuery(Helper.GetDataBaseType, cmd,
                                DataBaseConnectionString, sqlCmd, _DbParameterCollection);

                    if (argEn.AFCDetailslist != null)
                    {
                        if (argEn.AFCDetailslist.Count != 0)
                        {
                            AFCDetailsDS loDS = new AFCDetailsDS();
                            //Inserting AFCDetails
                            for (int i = 0; i < argEn.AFCDetailslist.Count; i++)
                            {
                                argEn.AFCDetailslist[i].TransCode = lb;
                                loDS.Insert(argEn.AFCDetailslist[i]);
                            }
                        }
                    }

                    if (lb > -1)
                        lbRes = true;                    
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return lbRes;
        }

        #endregion

        #region AFCBatchDelete 

        /// <summary>
        /// Method to Delete Batch AFC
        /// </summary>
        /// <param name="argEn">AFC Entity is an Input.</param>
        /// <returns>Returns Boolean</returns>
        public bool AFCBatchDelete(AFCEn argEn)
        {
            bool lbRes = false;
            List<AFCDetailsEn> loafcdetails = new List<AFCDetailsEn>();
            string sqlCmd = "SELECT * FROM SAS_AFC WHERE batchcode = @batchcode";
            
            try
            {
                if (!FormHelp.IsBlank(sqlCmd))
                {
                    DbCommand cmd = _DatabaseFactory.GetDbCommand(Helper.GetDataBaseType, sqlCmd, DataBaseConnectionString);
                    _DatabaseFactory.AddInParameter(ref cmd, "@batchcode", DbType.String, argEn.BatchCode);
                    _DbParameterCollection = cmd.Parameters;

                    using (IDataReader loReader = _DatabaseFactory.GetIDataReader(Helper.GetDataBaseType, cmd,
                        DataBaseConnectionString, sqlCmd, _DbParameterCollection).CreateDataReader())
                    {
                        AFCDetailsDS loObjAccountsDAL = new AFCDetailsDS();
                        AFCDetailsEn loItem = null;
                        while (loReader.Read())
                        {
                            loItem = new AFCDetailsEn();
                            loItem.TransCode = GetValue<int>(loReader, "transcode");
                            loafcdetails.Add(loItem);
                            loItem = null;
                        }
                        loReader.Close();
                        int i = 0;
                        //deleting each item in batch
                        for (i = 0; i < loafcdetails.Count; i++)
                        {
                            loObjAccountsDAL.Delete(loafcdetails[i]);
                        }
                    }
                }
            }
        
            catch (Exception ex)
            {
                throw ex;
            }
            //Delete(argEn);
            if (Delete(argEn))
            {
                lbRes = true;
            }

            return lbRes;
        }

        #endregion

        #region Delete 

        /// <summary>
        /// Method to Delete AFC
        /// </summary>
        /// <param name="argEn">AFC Entity is an Input.</param>
        /// <returns>Returns Boolean</returns>
        public bool Delete(AFCEn argEn)
        {
            bool lbRes = false;
            string sqlCmd = "DELETE FROM SAS_AFC WHERE batchcode = @batchcode";
            
            try
            {
                if (!FormHelp.IsBlank(sqlCmd))
                {
                    DbCommand cmd = _DatabaseFactory.GetDbCommand(Helper.GetDataBaseType, sqlCmd, DataBaseConnectionString);
                    _DatabaseFactory.AddInParameter(ref cmd, "@BatchCode", DbType.String, argEn.BatchCode);
                    _DbParameterCollection = cmd.Parameters;

                    int liRowAffected = _DatabaseFactory.ExecuteNonQuery(Helper.GetDataBaseType, cmd,
                                DataBaseConnectionString, sqlCmd, _DbParameterCollection);

                    if (liRowAffected > -1)
                        lbRes = true;
                    else
                        throw new Exception("Deletion Failed! No Row has been deleted...");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return lbRes;
        }

        #endregion

        #region Load Object 

        /// <summary>
        /// Method to Loadobject AFC 
        /// </summary>
        /// <param name="argReader">IDataReader Object is an Input.</param>
        /// <returns>Returns BatchCode Entity</returns>
        private AFCEn LoadObjectAFC(IDataReader argReader)
        {
            AFCEn loItem = new AFCEn();
            loItem.BatchCode = GetValue<string>(argReader, "BatchCode");

            return loItem;
        }
        /// <summary>
        /// Method to Loadobject AFC 
        /// </summary>
        /// <param name="argReader">IDataReader Object is an Input.</param>
        /// <returns>Returns BatchCode Entity</returns>
        private FeeStructEn LoadObjectFeeStruct(IDataReader argReader)
        {
            FeeStructEn loItem = new FeeStructEn();
            loItem.FeeStructureCode = GetValue<string>(argReader, "SAFS_Code");

            return loItem;
        }
        /// <summary>
        /// Method to Load AFC Entity 
        /// </summary>
        /// <param name="argReader">IDataReader Object is an Input.</param>
        /// <returns>Returns AccountDetails Entity</returns>
        private AFCEn LoadObject(IDataReader argReader)
        {
            AFCEn loItem = new AFCEn();
            loItem.TransCode = GetValue<int>(argReader, "TransCode");
            loItem.AFCode = GetValue<string>(argReader, "AFCode");
            loItem.SAFC_Code = GetValue<string>(argReader, "SAFC_Code");
            loItem.BatchCode = GetValue<string>(argReader, "BatchCode");
            loItem.Description = GetValue<string>(argReader, "Description");
            loItem.Bdate = GetValue<DateTime>(argReader, "Bdate");
            loItem.Semester = GetValue<String>(argReader, "Semester");
            loItem.TransDate = GetValue<DateTime>(argReader, "TransDate");
            loItem.DueDate = GetValue<DateTime>(argReader, "DueDate");
            loItem.BatchTotal = GetValue<double>(argReader, "BatchTotal");
            loItem.PostedFor = GetValue<string>(argReader, "PostedFor");
            loItem.Reference = GetValue<string>(argReader, "Reference");
            loItem.Updatedby = GetValue<string>(argReader, "Updatedby");
            loItem.Updatetime = GetValue<string>(argReader, "Uptimestamp");

            return loItem;
        }
        /// <summary>
        /// Method to Load AFC Entity 
        /// </summary>
        /// <param name="argReader">IDataReader Object is an Input.</param>
        /// <returns>Returns AccountDetails Entity</returns>
        private AFCEn LoadObjectAFCPosted(IDataReader argReader)
        {
            AFCEn loItem = new AFCEn();
            loItem.CreditRef = GetValue<string>(argReader, "CreditRef");
            loItem.SASI_Name = GetValue<string>(argReader, "SASI_Name");
            loItem.TransID = GetValue<int>(argReader, "TransID");
            loItem.TransDate = GetValue<DateTime>(argReader, "TransDate");
            loItem.TransCodeAFC = GetValue<string>(argReader, "TransCode");
            loItem.DescriptionAFC = GetValue<string>(argReader, "Description");
            loItem.Category = GetValue<string>(argReader, "Category");
            loItem.TransType = GetValue<string>(argReader, "TransType");
            loItem.TransAmount = GetValue<Double>(argReader, "TransAmount");


            return loItem;
        }
        private static T GetValue<T>(IDataReader argReader, string argColNm)
        {
            if (!argReader.IsDBNull(argReader.GetOrdinal(argColNm)))
                return (T)argReader.GetValue(argReader.GetOrdinal(argColNm));
            else
                return default(T);
        }
        
        /// <summary>
        /// Method to Load AFC Entity 
        /// </summary>
        /// <param name="argReader">IDataReader Object is an Input.</param>
        /// <returns>Returns AccountDetails Entity</returns>
        private AFCEn LoadObjectAFCDetails(IDataReader argReader)
        {
            AFCEn loDetails = new AFCEn();
            loDetails.BatchCode = GetValue<string>(argReader, "BatchCode");
            loDetails.SAFT_Code = GetValue<string>(argReader, "SAFT_Code");
            loDetails.SAFT_Desc = GetValue<string>(argReader, "SAFT_Desc");
            loDetails.TransAmount = GetValue<double>(argReader, "SAFA_Amount");

            return loDetails;
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

        #region FetchBatchNumber 

        public string FetchBatchNumber(string Faculty,string Semester,string Program)
        {
            string BatchCode=string.Empty;
            string SqlStr = @"select * from SAS_AFC AF left join SAS_AFCDetails AFC on AF.TransCode=AFC.TransCode
                                where AF.SAFC_Code='" + Faculty + "'" +
                                "and af.Semester='" + Semester + "'" +
                                "and afc.ProgramCode='" + Program + "'" +
                                "and af.Reference='Ready'";

            try
            {
                IDataReader loReader = _DatabaseFactory.ExecuteReader(Helper.GetDataBaseType,
                         DataBaseConnectionString, SqlStr).CreateDataReader();

                //if (lsError.Length > 0)
                //    throw new Exception(lsError);
                if (loReader.Read())
                {
                    BatchCode = loReader["BatchCode"].ToString();
                }
                loReader.Close();
            }
            catch (Exception err)
            {
                throw err;
            }
            return BatchCode;
        }

        #endregion

        #region FetchBatchNumberReport 

        public string FetchBatchNumberReport(string Faculty, string Semester, string Program)
        {
            string BatchCode = string.Empty;
            string SqlStr = @"select * from SAS_AFC AF left join SAS_AFCDetails AFC on AF.TransCode=AFC.TransCode
                                where AF.SAFC_Code='" + Faculty + "'" +
                                "and af.Semester='" + Semester + "'" +
                                "and afc.ProgramCode='" + Program + "'";

            try
            {
                IDataReader loReader = _DatabaseFactory.ExecuteReader(Helper.GetDataBaseType,
                          DataBaseConnectionString, SqlStr).CreateDataReader();

                //if (lsError.Length > 0)
                //    throw new Exception(lsError);
                if (loReader.Read())
                {
                    BatchCode = loReader["BatchCode"].ToString();
                }
                loReader.Close();
            }
            catch (Exception err)
            {
                throw err;
            }
            return BatchCode;
        }

        #endregion

        #region IsPosted 

        public string IsPosted(string Faculty, string Semester, string Program)
        {
            string Status = string.Empty;
            string SqlStr = @"select case af.Reference when 'Posted' then 'true' else 'false' end as Status from SAS_AFC AF left join SAS_AFCDetails AFC on AF.TransCode=AFC.TransCode
                                where AF.SAFC_Code='" + Faculty + "'" +
                                "and af.Semester='" + Semester + "'" +
                                "and afc.ProgramCode='" + Program + "'";

            try
            {
                IDataReader loReader = _DatabaseFactory.ExecuteReader(Helper.GetDataBaseType,
                          DataBaseConnectionString, SqlStr).CreateDataReader();
                if (loReader.Read())
                {
                    Status = loReader["Status"].ToString();
                }
                loReader.Close();
            }
            catch (Exception err)
            {
                throw err;
            }
            return Status;
        }

        #endregion

        #region UpdateStudentAFCStatus 

        public int UpdateStudentAFCStatus(string Matric, string Semester, string Program,int Status,string sMatricNo)
        {
            string SqlStr = "update SAS_Student set SASI_PostStatus= '" + Status + "'"+ " where SASI_CurSemYr='" + Semester + "' and SASI_PgId='" + Program + "'AND SASI_MatricNo='"+sMatricNo+"'";

            int iAffectedRows = 0;
            try
            {
                iAffectedRows = _DatabaseFactory.ExecuteSqlStatement(Helper.GetDataBaseType, 
                    DataBaseConnectionString, SqlStr);
            }
            catch (Exception err)
            {
                throw err;
            }
            return iAffectedRows;
        }

        #endregion

        #region UpdateAFCReference

        public bool UpdateAFCReference(string batch)
        {
            bool lbRes = false;
            string MatricNo = null, CurrentSem = null, Program = null;
            
            try
            {
                string sqlCmd = "UPDATE SAS_AFC SET Reference = 'Posted' WHERE batchcode = @batchcode";

                if (!FormHelp.IsBlank(sqlCmd))
                {
                    DbCommand cmd = _DatabaseFactory.GetDbCommand(Helper.GetDataBaseType, sqlCmd, DataBaseConnectionString);                
                    _DatabaseFactory.AddInParameter(ref cmd, "@BatchCode", DbType.String, batch);
                    _DbParameterCollection = cmd.Parameters;

                    int liRowAffected = _DatabaseFactory.ExecuteNonQuery(Helper.GetDataBaseType, cmd,
                                DataBaseConnectionString, sqlCmd, _DbParameterCollection);

                    if (liRowAffected > -1)
                    {
                        lbRes = true;

                        //Update Student PostStatus -Start
                        //modified by Hafiz @ 21/11/2016
                        sqlCmd = "select distinct sas_accounts.creditref,sas_afc.semester,sas_student.sasi_pgid " +
                            "from sas_accounts inner join sas_student on sas_accounts.creditref = sas_student.sasi_matricno " +
                            "inner join sas_afc on sas_accounts.batchcode = sas_afc.batchcode " +
                            "where sas_afc.batchcode = " + clsGeneric.AddQuotes(batch);

                        if (!FormHelp.IsBlank(sqlCmd))
                        {
                            IDataReader loReader = _DatabaseFactory.ExecuteReader(Helper.GetDataBaseType,
                                DataBaseConnectionString, sqlCmd).CreateDataReader();

                            while (loReader.Read())
                            {
                                if (loReader.RecordsAffected != -1)
                                {
                                    MatricNo = GetValue<string>(loReader, "creditref");
                                    CurrentSem = GetValue<string>(loReader, "semester");
                                    Program = GetValue<string>(loReader, "sasi_pgid");

                                    UpdateStudentAFCStatus(MatricNo, CurrentSem, Program, 2, MatricNo);
                                }
                                else
                                    throw new Exception("No AFC Details - Student");
                            }
                            loReader.Close();
                        }
                        //Update Student PostStatus -End
                    }
                    else
                        throw new Exception("Update AFC Failed.");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return lbRes;

        }

        #endregion
    }

}
//---------------------------------------------------------------------------------

