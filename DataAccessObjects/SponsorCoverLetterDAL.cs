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
    public class SponsorCoverLetterDAL
    {

        #region Global Declarations

        private DbParameterCollection _DbParameterCollection = null;

        private MaxModule.DatabaseProvider _DatabaseFactory =
            new MaxModule.DatabaseProvider();

        private string DataBaseConnectionString = Helper.
           GetConnectionString();
        
        #endregion

        public SponsorCoverLetterDAL()
        {

        }

        #region GetList()
        public List<SponsorCoverLetterEn> GetList(SponsorCoverLetterEn argEn)
        {
            List<SponsorCoverLetterEn> loEnList = new List<SponsorCoverLetterEn>();
            argEn.Code = argEn.Code.Replace("*", "%");
            argEn.Title = argEn.Title.Replace("*", "%");
            string sqlCmd = "select * from SAS_SponsorCoverLetter where SASCL_Code <> '0'";
            if (argEn.Code.Length != 0) sqlCmd = sqlCmd + "and SASCL_Code like " + clsGeneric.AddQuotes(argEn.Code);
            if (argEn.Title.Length != 0) sqlCmd = sqlCmd + "and SASCL_Title like " + clsGeneric.AddQuotes(argEn.Title);

            try
            {
                if (!FormHelp.IsBlank(sqlCmd))
                {
                    using (IDataReader loReader = _DatabaseFactory.ExecuteReader(Helper.GetDataBaseType, DataBaseConnectionString, sqlCmd).CreateDataReader())
                    {
                        while (loReader.Read())
                        {
                            SponsorCoverLetterEn loItem = LoadSponsorCoverLetterObject(loReader);
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

        #region Insert
        /// <summary>
        /// Method to Insert SponsorCoverLetter
        /// </summary>
        /// <param name="argEn">SponsorCoverLetterEn Entity is the Input.</param>
        /// <returns>Returns a Boolean</returns>
        public bool Insert(SponsorCoverLetterEn argEn)
        {
            bool lbRes = false;
            int iOut = 0;
            string sqlCmd = @"Select count(*) as cnt from sas_SponsorCoverLetter where sascl_code = @code or 
            sascl_title = @title";
            try
            {
                if (!FormHelp.IsBlank(sqlCmd))
                {
                    DbCommand cmdSel = _DatabaseFactory.GetDbCommand(Helper.GetDataBaseType, sqlCmd, DataBaseConnectionString);
                    _DatabaseFactory.AddInParameter(ref cmdSel, "@code", DbType.String, argEn.Code);
                    _DatabaseFactory.AddInParameter(ref cmdSel, "@title", DbType.String, argEn.Title);
                    _DbParameterCollection = cmdSel.Parameters;

                    using (IDataReader dr = _DatabaseFactory.GetIDataReader(Helper.GetDataBaseType, cmdSel,
                        DataBaseConnectionString, sqlCmd, _DbParameterCollection).CreateDataReader())
                    {
                        if (dr.Read())
                            iOut = clsGeneric.NullToInteger(dr["cnt"]);
                        if (iOut > 0)
                            throw new Exception("Record Already Exist");
                        dr.Close();
                    }
                    if (iOut == 0)
                    {
                        sqlCmd = @"insert into sas_sponsorcoverletter(sascl_code, sascl_title, sascl_ourref, sascl_yourref, 
                                    sascl_address, sascl_message, sascl_signby, sascl_name, sascl_frdate, sascl_todate, sascl_updatedby,
                                    sascl_updatedtime)
                                    VALUES
                                    (@code, @title, @ourref, @yourref, @address, @message, @signby, @name, @frdate, @todate, @updatedby, @updatedtime)";

                        if (!FormHelp.IsBlank(sqlCmd))
                        {
                            DbCommand cmd = _DatabaseFactory.GetDbCommand(Helper.GetDataBaseType, sqlCmd, DataBaseConnectionString);
                            _DatabaseFactory.AddInParameter(ref cmd, "@code", DbType.String, clsGeneric.NullToString(argEn.Code));
                            _DatabaseFactory.AddInParameter(ref cmd, "@title", DbType.String, clsGeneric.NullToString(argEn.Title));
                            _DatabaseFactory.AddInParameter(ref cmd, "@ourref", DbType.String, clsGeneric.NullToString(argEn.OurRef));
                            _DatabaseFactory.AddInParameter(ref cmd, "@yourref", DbType.String, clsGeneric.NullToString(argEn.YourRef));
                            _DatabaseFactory.AddInParameter(ref cmd, "@address", DbType.String, clsGeneric.NullToString(argEn.Address));
                            _DatabaseFactory.AddInParameter(ref cmd, "@message", DbType.String, clsGeneric.NullToString(argEn.Message));
                            _DatabaseFactory.AddInParameter(ref cmd, "@signby", DbType.String, clsGeneric.NullToString(argEn.SignBy));
                            _DatabaseFactory.AddInParameter(ref cmd, "@name", DbType.String, clsGeneric.NullToString(argEn.Name));
                            _DatabaseFactory.AddInParameter(ref cmd, "@frdate", DbType.DateTime, Helper.DateConversion(argEn.FromDate));
                            _DatabaseFactory.AddInParameter(ref cmd, "@todate", DbType.DateTime, Helper.DateConversion(argEn.ToDate));
                            _DatabaseFactory.AddInParameter(ref cmd, "@updatedby", DbType.String, clsGeneric.NullToString(argEn.UpdatedBy));
                            _DatabaseFactory.AddInParameter(ref cmd, "@updatedtime", DbType.DateTime, Helper.DateConversion(argEn.UpdatedTime));
                            _DbParameterCollection = cmd.Parameters;

                            int liRowAffected = _DatabaseFactory.ExecuteNonQuery(Helper.GetDataBaseType, cmd, DataBaseConnectionString,
                                sqlCmd, _DbParameterCollection);

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
        /// Method to Update SponsorCoverLetter
        /// </summary>
        /// <param name="argEn">SponsorCoverLetterEn Entity is the Input.</param>
        /// <returns>Returns a Boolean</returns>
        public bool Update(SponsorCoverLetterEn argEn)
        {
            bool lbRes = false;
            int iOut = 0;
            string sqlCmd = "Select count(*) as cnt From sas_sponsorcoverletter WHERE sascl_code != @code and sascl_title = @title";
            try
            {
                if (!FormHelp.IsBlank(sqlCmd))
                {
                    DbCommand cmdSel = _DatabaseFactory.GetDbCommand(Helper.GetDataBaseType, sqlCmd, DataBaseConnectionString);
                    _DatabaseFactory.AddInParameter(ref cmdSel, "@code", DbType.String, argEn.Code);
                    _DatabaseFactory.AddInParameter(ref cmdSel, "@title", DbType.String, argEn.Title);
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
                        sqlCmd = @"UPDATE sas_sponsorcoverletter SET sascl_code = @code, sascl_title = @title, sascl_ourref = @ourref, sascl_yourref = @yourref,
                                    sascl_message = @message, sascl_address = @address,
                                    sascl_signby = @signby, sascl_name = @name, sascl_frdate = @frdate, sascl_todate = @todate, sascl_updatedby = @updatedby,
                                    sascl_updatedtime = @updatedtime WHERE sascl_code = @code";

                        if (!FormHelp.IsBlank(sqlCmd))
                        {
                            DbCommand cmd = _DatabaseFactory.GetDbCommand(Helper.GetDataBaseType, sqlCmd, DataBaseConnectionString);
                            _DatabaseFactory.AddInParameter(ref cmd, "@code", DbType.String, clsGeneric.NullToString(argEn.Code));
                            _DatabaseFactory.AddInParameter(ref cmd, "@title", DbType.String, clsGeneric.NullToString(argEn.Title));
                            _DatabaseFactory.AddInParameter(ref cmd, "@ourref", DbType.String, clsGeneric.NullToString(argEn.OurRef));
                            _DatabaseFactory.AddInParameter(ref cmd, "@yourref", DbType.String, clsGeneric.NullToString(argEn.YourRef));
                            _DatabaseFactory.AddInParameter(ref cmd, "@address", DbType.String, clsGeneric.NullToString(argEn.Address));
                            _DatabaseFactory.AddInParameter(ref cmd, "@message", DbType.String, clsGeneric.NullToString(argEn.Message));
                            _DatabaseFactory.AddInParameter(ref cmd, "@signby", DbType.String, clsGeneric.NullToString(argEn.SignBy));
                            _DatabaseFactory.AddInParameter(ref cmd, "@name", DbType.String, clsGeneric.NullToString(argEn.Name));
                            _DatabaseFactory.AddInParameter(ref cmd, "@frdate", DbType.DateTime, Helper.DateConversion(argEn.FromDate));
                            _DatabaseFactory.AddInParameter(ref cmd, "@todate", DbType.DateTime, Helper.DateConversion(argEn.ToDate));
                            _DatabaseFactory.AddInParameter(ref cmd, "@updatedby", DbType.String, clsGeneric.NullToString(argEn.UpdatedBy));
                            _DatabaseFactory.AddInParameter(ref cmd, "@updatedtime", DbType.DateTime, Helper.DateConversion(argEn.UpdatedTime));
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
        ///  Method to Delete  SponsorCoverLetterEn 
        /// </summary>
        /// <param name="argEn">SponsorCoverLetterEn Entity is an Input. SASCL_Code is Property.</param>
        /// <returns>Returns a Boolean</returns>
        public bool Delete(SponsorCoverLetterEn argEn)
        {
            bool lbRes = false;
            string sqlCmd = "DELETE FROM sas_sponsorcoverletter WHERE SASCL_Code = @code";

            try
            {
                if (!FormHelp.IsBlank(sqlCmd))
                {
                    DbCommand cmd = _DatabaseFactory.GetDbCommand(Helper.GetDataBaseType, sqlCmd, DataBaseConnectionString);
                    _DatabaseFactory.AddInParameter(ref cmd, "@code", DbType.String, argEn.Code);
                    _DbParameterCollection = cmd.Parameters;

                    int liRowAffected = _DatabaseFactory.ExecuteNonQuery(Helper.GetDataBaseType, cmd,
                                DataBaseConnectionString, sqlCmd, _DbParameterCollection);

                    if (liRowAffected > -1)
                        lbRes = true;
                    else
                        throw new Exception("Deletion Failed! No Row has been Updated...");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return lbRes;
        }

        #endregion


        #region GetProgramBySponsor

        public List<ProgramInfoEn> GetProgramBySponsor(string Sponsor)
        {
            List<ProgramInfoEn> loEnList = new List<ProgramInfoEn>();

            string sqlCmd = @"select distinct prog.* from sas_student stu 
                                inner join sas_studentSpon sspon on stu.sasi_matricno = sspon.sasi_matricno
                                inner join sas_program prog on stu.sasi_pgid = prog.sapg_code
                                inner join sas_feestruct fs on fs.sapg_code = stu.sasi_pgid and stu.sasi_cursemyr = fs.sast_code
                                inner join sas_feestramount fsa on fsa.safs_code = fs.safs_code and stu.sasc_code = fsa.sasc_code
                                inner join sas_sponsorfeetypes sft on sft.saft_code = fsa.saft_code and sft.sasr_code = sspon.sass_sponsor 
                                where sspon.sass_sponsor = " + clsGeneric.AddQuotes(Sponsor)
                                                             + " and stu.SASI_Reg_Status =" + Helper.StuRegistered
                                                             +" and stu.sass_code =" + clsGeneric.AddQuotes(Helper.StuActive);

            try
            {
                if (!FormHelp.IsBlank(sqlCmd))
                {
                    using (IDataReader argReader = _DatabaseFactory.ExecuteReader(Helper.GetDataBaseType,
                        DataBaseConnectionString, sqlCmd).CreateDataReader())
                    {
                        while (argReader.Read())
                        {

                            ProgramInfoEn loItem = new ProgramInfoEn();
                            loItem.ProgramCode = GetValue<string>(argReader, "SAPG_Code");
                            loItem.Program = GetValue<string>(argReader, "SAPG_Program");
                            loEnList.Add(loItem);
                        }
                        argReader.Close();
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

        public List<SponsorCoverLetterEn> GetSponsorStudentDetails(SponsorCoverLetterEn argEn)
        {
            List<SponsorCoverLetterEn> loEnList = new List<SponsorCoverLetterEn>();

            string sqlCmd = "";
            if (argEn.MatricNo != string.Empty)
                sqlCmd = "select * from sas_student stu";
            else
                sqlCmd = "select distinct *, stu.sasi_matricno from sas_student stu";
            sqlCmd += @"        inner join sas_studentSpon sSpon on sSpon.sasi_matricno = stu.sasi_matricno
                                inner join sas_program prog on prog.sapg_code = stu.sasi_pgid
                                inner join sas_feestruct fs on fs.sapg_code = stu.sasi_pgid and stu.sasi_cursemyr = fs.sast_code
                                inner join sas_feestramount fsa on fsa.safs_code = fs.safs_code and stu.sasc_code = fsa.sasc_code
                                inner join sas_sponsorfeetypes sft on sft.saft_code = fsa.saft_code and sft.sasr_code = sspon.sass_sponsor
                                inner join sas_feetypes ft on fsa.saft_code = ft.saft_code" +
                                " where  stu.SASI_Reg_Status =" + Helper.StuRegistered
                                + " and stu.sass_code =" + clsGeneric.AddQuotes(Helper.StuActive) +
                                " and CAST(SSPON.SASS_SDATE  as DATE) <= " + clsGeneric.AddQuotes(Helper.DateConversion(DateTime.Today))
                                + " and CAST(SSPON.SASS_EDATE  as DATE) >= " + clsGeneric.AddQuotes(Helper.DateConversion(DateTime.Today));
            if (argEn.SponsorCode != string.Empty && argEn.SponsorCode != null)
                sqlCmd += " and sspon.sass_sponsor = " + clsGeneric.AddQuotes(argEn.SponsorCode);
            if (argEn.ProgramID != string.Empty && argEn.ProgramID != null)
                sqlCmd += " and prog.sapg_code =" + clsGeneric.AddQuotes(argEn.ProgramID);
            if (argEn.MatricNo != string.Empty && argEn.MatricNo != null)
                sqlCmd += " and stu.sasi_matricno =" + clsGeneric.AddQuotes(argEn.MatricNo);
            sqlCmd += " order by stu.sasi_matricno";

            try
            {
                if (!FormHelp.IsBlank(sqlCmd))
                {
                    using (IDataReader argReader = _DatabaseFactory.ExecuteReader(Helper.GetDataBaseType,
                        DataBaseConnectionString, sqlCmd).CreateDataReader())
                    {
                        while (argReader.Read())
                        {

                            SponsorCoverLetterEn loItem = LoadObjectDetails(argReader);
                            loItem.FeeTypeCode = GetValue<string>(argReader, "SAFT_Code");
                            loItem.Description = GetValue<string>(argReader, "SAFT_Desc");
                            loItem.FeeAmount = GetValue<double>(argReader, "safa_amount");
                            loItem.GSTAmount = GetValue<double>(argReader, "safa_gstamount");
                            
                            loEnList.Add(loItem);
                        }
                        argReader.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return loEnList;
        }

        public List<SponsorEn> GetSponsorWithStudent(SponsorEn argEn)
        {
            List<SponsorEn> loEnList = new List<SponsorEn>();
            string sqlCmd = @"select distinct spon.* from sas_student stu
                                inner join sas_studentSpon sSpon on sSpon.sasi_matricno = stu.sasi_matricno
                                inner join sas_program prog on prog.sapg_code = stu.sasi_pgid
                                inner join sas_feestruct fs on fs.sapg_code = stu.sasi_pgid and stu.sasi_cursemyr = fs.sast_code
                                inner join sas_feestramount fsa on fsa.safs_code = fs.safs_code and stu.sasc_code = fsa.sasc_code
                                inner join sas_sponsorfeetypes sft on sft.saft_code = fsa.saft_code and sft.sasr_code = sspon.sass_sponsor 
                                inner join sas_sponsor spon on spon.sasr_code = sspon.sass_sponsor" +
                                " where  stu.SASI_Reg_Status =" + Helper.StuRegistered
                                + " and stu.sass_code =" + clsGeneric.AddQuotes(Helper.StuActive)
                                +" order by spon.sasr_name";

            try
            {
                if (!FormHelp.IsBlank(sqlCmd))
                {
                    using (IDataReader argReader = _DatabaseFactory.ExecuteReader(Helper.GetDataBaseType,
                        DataBaseConnectionString, sqlCmd).CreateDataReader())
                    {
                        while (argReader.Read())
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

                            loEnList.Add(loItem);
                        }
                        argReader.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return loEnList;
        }

        private SponsorCoverLetterEn LoadSponsorCoverLetterObject(IDataReader argReader)
        {
            SponsorCoverLetterEn loItem = new SponsorCoverLetterEn();
            loItem.Code = GetValue<string>(argReader, "SASCL_Code");
            loItem.Title = GetValue<string>(argReader, "SASCL_Title");
            loItem.OurRef = GetValue<string>(argReader, "SASCL_OurRef");
            loItem.YourRef = GetValue<string>(argReader, "SASCL_YourRef");
            loItem.Message = GetValue<string>(argReader, "SASCL_Message");
            loItem.Address = GetValue<string>(argReader, "SASCL_Address");
            loItem.SignBy = GetValue<string>(argReader, "SASCL_SignBy");
            loItem.Name = GetValue<string>(argReader, "SASCL_Name");
            loItem.FromDate = GetValue<DateTime>(argReader, "SASCL_FrDate");
            loItem.ToDate = GetValue<DateTime>(argReader, "SASCL_ToDate");
            loItem.UpdatedBy = GetValue<string>(argReader, "SASCL_UpdatedBy");
            loItem.UpdatedTime = GetValue<DateTime>(argReader, "SASCL_UpdatedTime");

            return loItem;
        }
        private SponsorCoverLetterEn LoadObjectDetails(IDataReader argReader)
        {
            SponsorCoverLetterEn loItem = new SponsorCoverLetterEn();
            //loItem.Code = GetValue<string>(argReader, "SASCL_Code");
            //loItem.Title = GetValue<string>(argReader, "SASCL_Title");
            //loItem.OurRef = GetValue<string>(argReader, "SASCL_OurRef");
            //loItem.YourRef = GetValue<string>(argReader, "SASCL_YourRef");
            //loItem.Message = GetValue<string>(argReader, "SASCL_Message");
            //loItem.Address = GetValue<string>(argReader, "SASCL_Address");
            //loItem.SignBy = GetValue<string>(argReader, "SASCL_SignBy");
            //loItem.Name = GetValue<string>(argReader, "SASCL_Name");
            //loItem.FromDate = GetValue<DateTime>(argReader, "SASCL_FrDate");
            //loItem.ToDate = GetValue<DateTime>(argReader, "SASCL_ToDate");
            //loItem.UpdatedBy = GetValue<string>(argReader, "SASCL_UpdatedBy");
            //loItem.UpdatedTime = GetValue<DateTime>(argReader, "SASCL_UpdatedTime");

            StudentEn stuEn = new StudentEn();
            stuEn.MatricNo = GetValue<string>(argReader, "sasi_matricno");
            stuEn.StudentName = GetValue<string>(argReader, "sasi_name");
            stuEn.ICNo = GetValue<string>(argReader, "sasi_icno");
            stuEn.CurretSemesterYear = GetValue<string>(argReader, "sasi_cursemyr");
            stuEn.ProgramID = GetValue<string>(argReader, "sasi_pgid");
            loItem.Studentacc = stuEn;

            SponsorEn sponEn = new SponsorEn();
            sponEn.SponserCode = GetValue<string>(argReader, "sasr_code");
            //sponEn.Name = GetValue<string>(argReader, "sasr_name");
            //sponEn.Email = GetValue<string>(argReader, "sasr_email");
            //sponEn.Status = GetValue<bool>(argReader, "sasr_status");
            loItem.SponsorDetails = sponEn;

            loItem.ProgramName = GetValue<string>(argReader, "sapg_program");

            return loItem;
        }
        private static T GetValue<T>(IDataReader argReader, string argColNm)
        {
            if (!argReader.IsDBNull(argReader.GetOrdinal(argColNm)))
                return (T)argReader.GetValue(argReader.GetOrdinal(argColNm));
            else
                return default(T);
        }

    }
}
