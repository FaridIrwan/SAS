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
    /// Class to handle all the Students Methods.
    /// </summary>
    public class StudentDAL
    {
        #region Global Declarations

        private DbParameterCollection _DbParameterCollection = null;

        private MaxModule.DatabaseProvider _DatabaseFactory =
            new MaxModule.DatabaseProvider();

        private string DataBaseConnectionString = Helper.
           GetConnectionString();

        private const string FeeCtgyAdmission = "A";
        private const string FeeCtgyTuition = "T";
        private const string FeeCtgyHostel = "H";
        private const int ChangeProgram = 1;
        private const int ChangeHostel = 3;
        private const int ChangeCreditHrs = 2;
        #endregion

        public StudentDAL()
        {
        }

        #region GetListBySemProgTypeProgID

        /// <summary>
        /// Method to Get List of Students
        /// </summary>
        /// <param name="argEn">Students Entity as an Input.</param>
        /// <returns>Returns List of Students</returns>
        public List<StudentEn> GetListBySemProgTypeProgID(StudentEn argEn)
        {
            List<StudentEn> loEnList = new List<StudentEn>();
            argEn.CurretSemesterYear = argEn.CurretSemesterYear.Replace("*", "%");
            argEn.ProgramType = argEn.ProgramType.Replace("*", "%");
            argEn.ProgramID = argEn.ProgramID.Replace("*", "%");
            //argEn.StudentName = argEn.StudentName.Replace("*", "%");
            string sqlCmd = "select * from sas_student, SAS_Program, SAS_DegreeType " +
            "where sas_student.SASI_PgId = SAS_Program.SAPG_Code and SAS_DegreeType.SADT_Code = SAS_Program.SAPG_ProgramType ";
            if (argEn.CurretSemesterYear.Length != 0) sqlCmd = sqlCmd + " and sas_student.SASI_CurSemYr = '" + argEn.CurretSemesterYear + "'";
            if (argEn.ProgramType.Length != 0) sqlCmd = sqlCmd + " and SAS_Program.SAPG_ProgramType = '" + argEn.ProgramType + "'";
            if (argEn.ProgramID.Length != 0) sqlCmd = sqlCmd + " and sas_student.SASI_PgId = '" + argEn.ProgramID + "'";
            sqlCmd = sqlCmd + " order by SASI_MatricNo, sas_student.SASI_CurSemYr,SAS_Program.SAPG_ProgramType";

            try
            {
                if (!FormHelp.IsBlank(sqlCmd))
                {
                    using (IDataReader loReader = _DatabaseFactory.ExecuteReader(Helper.GetDataBaseType,
                        DataBaseConnectionString, sqlCmd).CreateDataReader())
                    {
                        while (loReader.Read())
                        {
                            StudentEn loItem = LoadObjectForSem(loReader);
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

        #region GetList

        /// <summary>
        /// Method to Get List of Students
        /// </summary>
        /// <param name="argEn">Students Entity as an Input.</param>
        /// <returns>Returns List of Students</returns>
        public List<StudentEn> GetList(StudentEn argEn)
        {
            List<StudentEn> loEnList = new List<StudentEn>();
            argEn.MatricNo = argEn.MatricNo.Replace("*", "%");
            string sqlCmd = "select * from SAS_Student";
            if (argEn.MatricNo.Length != 0) sqlCmd = sqlCmd + " WHERE SASI_MatricNo like '" + argEn.MatricNo + "%'";
            sqlCmd = sqlCmd + " order by SASI_MatricNo";

            try
            {
                if (!FormHelp.IsBlank(sqlCmd))
                {
                    using (IDataReader loReader = _DatabaseFactory.ExecuteReader(Helper.GetDataBaseType,
                        DataBaseConnectionString, sqlCmd).CreateDataReader())
                    {
                        while (loReader.Read())
                        {
                            StudentEn loItem = LoadObject(loReader);
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

        #region GetListByProgramFeeAmount

        /// <summary>
        /// Method to Get List of Students
        /// </summary>
        /// <param name="argEn">Students Entity as an Input.</param>
        /// <returns>Returns List of Students</returns>
        public List<StudentEn> GetListByProgramFeeAmount(StudentEn argEn)
        {
            List<StudentEn> loEnList = new List<StudentEn>();
            argEn.ProgramID = argEn.ProgramID.Replace("*", "%");
            string sqlCmd = "select  s.sasc_code ,count(*) as Tot from SAS_Student s LEFT JOIN sas_studentcategory scat On s.sasc_code=scat.sasc_code";
            if (argEn.ProgramID.Length != 0) sqlCmd += " WHERE s.SASI_PgId like '" + argEn.ProgramID + "' AND ";
            //sqlCmd += " s.SASI_PostStatus ="+argEn.PostStatus+" AND s.SASI_StatusRec = '1' and s.SASS_CODE = 'PA' ";
            sqlCmd += " s.SASI_PostStatus ='" + argEn.PostStatus + "'" + " AND s.SASI_StatusRec = 'true' and s.SASS_CODE = '1' ";
            if (argEn.CurretSemesterYear.Length != 0) sqlCmd += " and s.SASI_CurSemYr = '" + argEn.CurretSemesterYear + "'";
            if (argEn.Faculty.Length != 0) sqlCmd += " and s.SASI_Faculty = '" + argEn.Faculty + "'";
            if (argEn.ProgramID.Length != 0) sqlCmd = sqlCmd + " and s.SASI_PgId =  '" + argEn.ProgramID + "' group by s.sasc_code ";


            try
            {
                if (!FormHelp.IsBlank(sqlCmd))
                {
                    using (IDataReader loReader = _DatabaseFactory.ExecuteReader(Helper.GetDataBaseType,
                        DataBaseConnectionString, sqlCmd).CreateDataReader())
                    {
                        while (loReader.Read())
                        {

                            StudentEn loItem = new StudentEn();
                            loItem.SASI_Add1 = loReader["SASC_Code"].ToString();
                            loItem.SASI_Add2 = loReader["Tot"].ToString();
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

        #region GetListByProgram

        /// <summary>
        /// Method to Get List of Students
        /// </summary>
        /// <param name="argEn">Students Entity as an Input.</param>
        /// <returns>Returns List of Students</returns>
        /// modified by Hafiz @ 09/6/2016 - comment 'sast_iscurrentsem = true' related
        /// modified by Hafiz @ 11/11/2016 - uncomment filter SASI_CurSemYr
        
        public List<StudentEn> GetListByProgram(StudentEn argEn)
        {
            List<StudentEn> loEnList = new List<StudentEn>();
            argEn.ProgramID = argEn.ProgramID.Replace("*", "%");
            string sqlCmd = "select * from SAS_Student";
            //string sqlCmd = "select SAS_Student.SASI_MatricNo,SAS_Student.SASI_Name, 0 AS transamount from SAS_Student";
            if (argEn.ProgramID.Length != 0) sqlCmd += " WHERE SASI_PgId like '" + argEn.ProgramID + "' AND ";
            //sqlCmd += " SAS_Student.SASI_PostStatus ="+argEn.PostStatus+" AND SAS_Student.SASI_StatusRec = '1' and SAS_Student.SASS_CODE = 'PA' ";
            //sqlCmd += " SAS_Student.SASI_PostStatus ='" + argEn.PostStatus +"'"+ " AND SAS_Student.SASI_StatusRec = 'true' and SAS_Student.SASS_CODE = 'PA' ";
            sqlCmd += "SAS_Student.SASI_PostStatus ='" + argEn.PostStatus + "'" + " AND SAS_Student.SASI_StatusRec = 'true' AND SAS_Student.SASI_reg_status = 2 " +
                        "AND sass_code IN (SELECT sass_code FROM sas_studentstatus where sass_blstatus ='true') ";
            if (argEn.CurretSemesterYear.Length != 0) sqlCmd += " and SASI_CurSemYr = '" + argEn.CurretSemesterYear + "'";
            if (argEn.Faculty.Length != 0) sqlCmd += "and SASI_Faculty = '" + argEn.Faculty + "'";
            if (argEn.ProgramID.Length != 0) sqlCmd = sqlCmd + " and SAS_Student.SASI_PgId =  '" + argEn.ProgramID + "'";

            //sqlCmd += " and SASI_CurSemYr in (select Sast_Code from sas_semestersetup where sast_iscurrentsem = true)"; comment by Hafiz @ 09/6/2016

            if (!string.IsNullOrEmpty(argEn.Intake))
            {
                if (argEn.Intake.Length != 0) sqlCmd += " and SAS_Student.SASI_Intake = '" + argEn.Intake + "'";
            }
            sqlCmd += " order by SASI_MatricNo";

            try
            {
                if (!FormHelp.IsBlank(sqlCmd))
                {
                    using (IDataReader loReader = _DatabaseFactory.ExecuteReader(Helper.GetDataBaseType,
                        DataBaseConnectionString, sqlCmd).CreateDataReader())
                    {
                        while (loReader.Read())
                        {
                            StudentEn loItem = LoadObject(loReader);
                            loItem.TransactionAmount = 0;
                            loEnList.Add(loItem);
                            //StudentEn loItem = new StudentEn();
                            //loItem.MatricNo = loReader["SASI_MatricNo"].ToString();
                            //loItem.StudentName = loReader["SASI_Name"].ToString();
                            //loItem.TransactionAmount =  Convert.ToDouble(loReader["transamount"]);
                            //loEnList.Add(loItem);
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

        #region GetListGroupedByProgram

        /// <summary>
        /// Method to Get List of Students
        /// </summary>
        /// <param name="argEn">Students Entity as an Input.</param>
        /// <returns>Returns List of Students</returns>
        public List<StudentEn> GetListGroupedByProgram(StudentEn argEn)
        {
            List<StudentEn> loEnList = new List<StudentEn>();
            argEn.ProgramID = argEn.ProgramID.Replace("*", "%");
            //string sqlCmd = "select SC.SASC_Desc AS SASI_Name,COUNT(ST.SASI_FeeCat) AS SASI_MatricNo from SAS_StudentCategory SC LEFT JOIN SAS_Student ST ON SC.SASC_Code=ST.SASI_FeeCat";
            //Edit Mona 18/2/2016
            string sqlCmd = "select SC.SASC_Code AS SASI_Name,COUNT(ST.SASI_FeeCat) AS SASI_MatricNo from SAS_StudentCategory SC LEFT JOIN SAS_Student ST ON SC.SASC_Code=ST.SASI_FeeCat";
            if (argEn.ProgramID.Length != 0) sqlCmd += " WHERE ST.SASI_PgId like '" + argEn.ProgramID + "' AND ";
            //sqlCmd += " ST.SASI_PostStatus =" + argEn.PostStatus + " AND ST.SASI_StatusRec = '1' and ST.SASS_CODE = 'PA' ";
            sqlCmd += " ST.SASI_PostStatus = '" + argEn.PostStatus + "'" + " AND ST.SASI_StatusRec = 'true' ";
            //if (argEn.CurretSemesterYear.Length != 0) sqlCmd += " and ST.SASI_CurSemYr = '" + argEn.CurretSemesterYear + "'";
            if (!string.IsNullOrEmpty(argEn.CurretSemesterYear))
            {
                if (argEn.CurretSemesterYear.Length != 0) sqlCmd += " and ST.SASI_CurSemYr = '" + argEn.CurretSemesterYear + "'";
            }
            if (!string.IsNullOrEmpty(argEn.Intake))
            {
                if (argEn.Intake.Length != 0) sqlCmd += " and ST.SASI_Intake = '" + argEn.Intake + "'";
            }  
            //if (argEn.ProgramID.Length != 0) sqlCmd = sqlCmd + " and ST.SASI_PgId =  '" + argEn.ProgramID + "'";
            //Edit Mona 18/2/2016
            //sqlCmd += "Group by ST.SASI_FeeCat,SC.SASC_Desc ";
            sqlCmd += "Group by ST.SASI_FeeCat,SC.SASC_Code ";

            try
            {
                if (!FormHelp.IsBlank(sqlCmd))
                {
                    using (IDataReader loReader = _DatabaseFactory.ExecuteReader(Helper.GetDataBaseType,
                        DataBaseConnectionString, sqlCmd).CreateDataReader())
                    {
                        while (loReader.Read())
                        {
                            StudentEn loItem = new StudentEn();
                            loItem.MatricNo = loReader["SASI_MatricNo"].ToString();
                            loItem.FeeCat = loReader["SASI_Name"].ToString();
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

        #region GetListByProgramForFee

        /// <summary>
        /// Added by Solomon to fetch the details based on the status
        /// </summary>
        /// <param name="argEn"></param>
        /// <returns></returns>
        /// modified by Hafiz @ 09/6/2016 - comment 'sast_iscurrentsem = true' related

        public List<StudentEn> GetListByProgramForFee(StudentEn argEn)
        {
            List<StudentEn> loEnList = new List<StudentEn>();
            argEn.ProgramID = argEn.ProgramID.Replace("*", "%");

            string sqlCmd = @"SELECT ST.SASI_MatricNo,ST.SASI_Name,AC.transamount 
                                FROM SAS_AFC AF 
                                LEFT JOIN SAS_AFCDetails AFD ON AF.TransCode=AFD.TransCode
                                LEFT JOIN SAS_Accounts AC ON AF.BatchCode=AC.BatchCode 
                                LEFT JOIN SAS_AccountsDetails ACD ON AC.TransID=ACD.TransID
                                LEFT JOIN SAS_Student ST ON AC.CreditRef=ST.SASI_MatricNo
                                where AF.Reference='";
            if (argEn.PostStatus == "2") sqlCmd += "Posted'";
            else sqlCmd += "Ready'";
            if (argEn.CurretSemesterYear.Length != 0) sqlCmd += "AND AF.Semester='" + argEn.CurretSemesterYear + "' ";
            if (argEn.Intake.Length != 0) sqlCmd += "AND ST.SASI_Intake='" + argEn.Intake + "' ";
            if (argEn.ProgramID.Length != 0) sqlCmd = sqlCmd + " and ST.SASI_PgId =  '" + argEn.ProgramID + "'";
            if (argEn.BatchCode.Length != 0) sqlCmd = sqlCmd + " and AF.BatchCode =  '" + argEn.BatchCode + "'";

            sqlCmd += " AND AF.Semester = ST.SASI_CurSemYr ";
            //sqlCmd += " and ST.SASI_CurSemYr IN (select Sast_Code from sas_semestersetup where sast_iscurrentsem = true) "; comment by Hafiz @ 09/6/2016

            sqlCmd += " GROUP BY ST.SASI_MatricNo,ST.SASI_Name,AC.transamount ORDER BY ST.SASI_MatricNo";

            try
            {
                if (!FormHelp.IsBlank(sqlCmd))
                {
                    using (IDataReader loReader = _DatabaseFactory.ExecuteReader(Helper.GetDataBaseType,
                        DataBaseConnectionString, sqlCmd).CreateDataReader())
                    {
                        while (loReader.Read())
                        {
                            StudentEn loItem = new StudentEn();
                            loItem.MatricNo = loReader["SASI_MatricNo"].ToString();
                            loItem.StudentName = loReader["SASI_Name"].ToString();
                            loItem.TransactionAmount = GetValue<double>(loReader, "transamount");
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

        #region GetStudentList

        /// <summary>
        /// Method to Get List of Active or Inactive Students
        /// </summary>
        /// <param name="argEn">Students Entity as an Input.MatricNo,StudentName,ICNo,ProgramId and StatusRec as Input Properties.</param>
        /// <returns>Returns List of Students</returns>
        public List<StudentEn> GetStudentList(StudentEn argEn)
        {
            List<StudentEn> loEnList = new List<StudentEn>();
            argEn.MatricNo = argEn.MatricNo.Replace("*", "%");
            argEn.StudentName = argEn.StudentName.Replace("*", "%");
            argEn.ICNo = argEn.ICNo.Replace("*", "%");
            argEn.ProgramID = argEn.ProgramID.Replace("*", "%");
            string sqlCmd = "SELECT  SAS_Student.* FROM SAS_Student where SASI_MatricNo <> '0'";
            if (argEn.MatricNo.Length != 0) sqlCmd = sqlCmd + " and SASI_MatricNo like '" + argEn.MatricNo + "'";
            if (argEn.StudentName.Length != 0) sqlCmd = sqlCmd + " and SASI_Name like '" + argEn.StudentName + "'";
            if (argEn.ICNo.Length != 0) sqlCmd = sqlCmd + " and SASI_ICNo like '" + argEn.ICNo + "'";
            if (argEn.ProgramID != "-1") sqlCmd = sqlCmd + " and SASI_PgId like '" + argEn.ProgramID + "'";
            //if (argEn.SASI_StatusRec == true) sqlCmd = sqlCmd + " and SASI_StatusRec = 1";
            if (argEn.SASI_StatusRec == true) sqlCmd = sqlCmd + " and SASI_StatusRec = 'true'";
            //if (argEn.SASI_StatusRec == false) sqlCmd = sqlCmd + " and SASI_StatusRec = 0";
            if (argEn.SASI_StatusRec == false) sqlCmd = sqlCmd + " and SASI_StatusRec = 'false'";
            //if (argEn.Faculty.Length != 0 && argEn.Faculty != "-1") sqlCmd = sqlCmd + " and SASI_StatusRec = 0";
            if (argEn.Faculty.Length != 0 && argEn.Faculty != "-1") sqlCmd = sqlCmd + " and SASI_StatusRec = 'false'";
            sqlCmd = sqlCmd + " order by SASI_MatricNo";
            try
            {
                if (!FormHelp.IsBlank(sqlCmd))
                {
                    using (IDataReader loReader = _DatabaseFactory.ExecuteReader(Helper.GetDataBaseType,
                        DataBaseConnectionString, sqlCmd).CreateDataReader())
                    {
                        while (loReader.Read())
                        {
                            StudentSponEn loStuSpnEn = new StudentSponEn();
                            StudentSponDAL loStuSpnDal = new StudentSponDAL();
                            StudentEn loItem = LoadObject(loReader);
                            loStuSpnEn.MatricNo = loItem.MatricNo;
                            //Getting the list of studentsponsors
                            loItem.ListStuSponser = loStuSpnDal.GetStuSponserList(loStuSpnEn);
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

        #region GetListStudent

        /// <summary>
        /// Method to Get List of All Students
        /// </summary>
        /// <param name="argEn">Students Entity as an Input.MatricNo,StudentName,ICNo,ProgramId,ID and StatusRec as Input Properties.</param>
        /// <returns>Returns List of Students</returns>
        /// updated by Hafiz @ 05/4/2016
        public List<StudentEn> GetListStudent(StudentEn argEn)
        {
            List<StudentEn> loEnList = new List<StudentEn>();
            argEn.MatricNo = argEn.MatricNo.Replace("*", "%");
            argEn.StudentName = argEn.StudentName.Replace("*", "%");
            argEn.Faculty = argEn.Faculty.Replace("*", "%");
            argEn.ProgramID = argEn.ProgramID.Replace("*", "%");
            argEn.ID = argEn.ID.Replace("*", "%");
            string sqlCmd = " SELECT SAS_Student.* FROM SAS_Student where sasi_statusrec = 'true' ";
            //if (argEn.MatricNo.Length != 0) sqlCmd = sqlCmd + " and SASI_MatricNo like '%" + argEn.MatricNo + "%'";
            if (argEn.MatricNo.Length != 0) sqlCmd = sqlCmd + " and SASI_MatricNo = '" + argEn.MatricNo + "'";
            if (argEn.StudentName.Length != 0) sqlCmd = sqlCmd + " and SASI_Name like '%" + argEn.StudentName + "%'";
            if (argEn.Faculty.Length != 0) sqlCmd = sqlCmd + " and SASI_Faculty like '%" + argEn.Faculty + "%'";
            if (argEn.ProgramID.Length != 0) sqlCmd = sqlCmd + " and SASI_PgId like '%" + argEn.ProgramID + "%'";
            if (argEn.ID.Length != 0) sqlCmd = sqlCmd + " and SASI_ICNo like '%" + argEn.ID + "%'";
            if (argEn.SAKO_Code.Length != 0) sqlCmd = sqlCmd + " and SAKO_Code = '" + argEn.SAKO_Code + "'";
            if (argEn.SABK_Code.Length != 0) sqlCmd = sqlCmd + " and SABK_Code = '" + argEn.SABK_Code + "'";
            if (argEn.SART_Code.Length != 0) sqlCmd = sqlCmd + " and SART_Code = '" + argEn.SART_Code + "'";
            sqlCmd = sqlCmd + "order by SASI_MatricNo";
            try
            {
                if (!FormHelp.IsBlank(sqlCmd))
                {
                    using (IDataReader loReader = _DatabaseFactory.ExecuteReader(Helper.GetDataBaseType,
                        DataBaseConnectionString, sqlCmd).CreateDataReader())
                    {
                        while (loReader.Read())
                        {
                            StudentEn loItem = LoadObject(loReader);
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

        #region GetListStudentOutstanding

        /// <summary>
        /// Method to Get List of Students with Sponsers
        /// </summary>
        /// <param name="argEn">Students Entity as an Input.Sponsor,SAKO_Code,ProgramID,Faculty and CategoryCode as Input Properties.</param>
        /// <returns>Returns List of Students</returns>
        public List<StudentEn> GetListStudentOutstanding(StudentEn argEn)
        {
            string sqlCmd;
            List<StudentEn> loEnList = new List<StudentEn>();

            argEn.ProgramID = argEn.ProgramID.Replace("*", "%");
            argEn.CurretSemesterYear = argEn.CurretSemesterYear.Replace("*", "%");

            //            sqlCmd = @"SELECT    * FROM      ( SELECT  DISTINCT
            //                                                        SS.SASI_MatricNo ,
            //                                                        SS.SASI_Name ,
            //                                                        SS.SASI_PgId ,
            //                                                        SS.SASI_CurSem ,
            //                                                        SS.SASI_CurSemYr ,
            //                                                        SS.SASI_Email,
            //                                                        
            //                                                       ( SELECT CASE WHEN SUM(TransAmount) IS NULL THEN 0
            //								ELSE SUM(TransAmount) END 
            //                                                                 FROM   SAS_Accounts
            //                                                                 WHERE   CreditRef = SS.SASI_MatricNo
            //                                                                        AND SubType = 'Student'
            //                                                                        AND PostStatus = 'Posted'
            //                                                                        AND TransType = 'Debit'
            //                                                        )AS  TotalDebit ,
            //                                                        ( SELECT CASE WHEN SUM(TransAmount) IS NULL THEN 0
            //								ELSE SUM(TransAmount) END 
            //                                                                 FROM   SAS_Accounts
            //                                                                 WHERE   CreditRef = SS.SASI_MatricNo
            //                                                                        AND SubType = 'Student'
            //                                                                        AND PostStatus = 'Posted'
            //                                                                        AND TransType = 'Credit'
            //                                                        )AS  TotalCredit,
            //                                                         ( SELECT CASE WHEN SUM(TransAmount) IS NULL THEN 0
            //								                            ELSE SUM(TransAmount) END 
            //                                                                 FROM   SAS_Accounts
            //                                                                 WHERE   CreditRef = SS.SASI_MatricNo
            //                                                                        AND SubType = 'Student'
            //                                                                        AND PostStatus = 'Posted'
            //                                                                        AND TransType = 'Debit'
            //                                                        ) - 
            //                                                        ( SELECT CASE WHEN SUM(TransAmount) IS NULL THEN 0
            //								                            ELSE SUM(TransAmount) END 
            //                                                                 FROM   SAS_Accounts
            //                                                                 WHERE   CreditRef = SS.SASI_MatricNo
            //                                                                        AND SubType = 'Student'
            //                                                                        AND PostStatus = 'Posted'
            //                                                                        AND TransType = 'Credit'
            //                                                        ) AS OutstandingAmount ,
            //                                                        NULL AS SASO_LoanAmount ,
            //                                                        0 AS SASO_IsReleased,
            //                                                        SS.SASI_Add1,
            //                                                        SS.SASI_Add2,
            //                                                        SS.SASI_City,
            //                                                        SS.SASI_State,
            //                                                        SS.SASI_Postcode
            //                                              FROM      SAS_Accounts SA
            //                                                        INNER JOIN SAS_Student SS ON SA.CreditRef = SS.SASI_MatricNo
            //                                              WHERE     SA.SubType = 'Student'
            //                                                        AND SA.PostStatus = 'Posted'
            //                                            ) A
            //                                  WHERE     A.OutstandingAmount > 0";
            //Modified by Jessica (08/03/2016) - TotalDebit & TotalCredit for category (Credit Note, Debit Note and Invoice will take from SAS_AccountsDetails)

            sqlCmd = @"SELECT    * FROM      ( SELECT  DISTINCT
                                                        SS.SASI_MatricNo ,
                                                        SS.SASI_Name ,
                                                        SS.SASI_PgId ,
                                                        SS.SASI_CurSem ,
                                                        SS.SASI_CurSemYr ,
                                                        SS.SASI_Email,
                                                        
                                                      (SELECT CASE WHEN Sum(Debit) IS NULL THEN 0
				                                        ELSE SUM(Debit) END AS  TotalDebit FROM      
				                                        (SELECT case when acc.category = 'Credit Note' or  acc.category = 'Debit Note' or acc.category = 'Invoice' then
							                                            SUM(de.transamount)
							                                        else     
							                                          SUM(acc.TransAmount)
							                                            END Debit
				                                        FROM SAS_Accounts acc
				                                        left join sas_accountsdetails de on acc.transid = de.transid
				                                        WHERE acc.creditRef = SS.SASI_MatricNo and acc.subtype = 'Student' and acc.poststatus = 'Posted' and acc.transtype = 'Debit'
				                                         GROUP BY acc.category, acc.TransType  
				                                        )as Debit),
                                                        (SELECT CASE WHEN Sum(Credit) IS NULL THEN 0
				                                            ELSE SUM(Credit) END AS  TotalCredit FROM
			                                            (SELECT
						                                              case when acc.category = 'Credit Note' or  acc.category = 'Debit Note' or acc.category = 'Invoice' then
									                                                SUM(de.transamount)
									                                            else     
									                                              SUM(acc.TransAmount)
									                                                END Credit
						                                            FROM SAS_Accounts acc
						                                            left join sas_accountsdetails de on acc.transid = de.transid
						                                            WHERE acc.creditRef = SS.SASI_MatricNo and acc.subtype = 'Student' and acc.poststatus = 'Posted' and acc.transtype = 'Credit'
						                                             GROUP BY acc.category, acc.TransType  
						                                            )as Credit),
                                                        (SELECT CASE WHEN Sum(Debit) IS NULL THEN 0
				                                                        ELSE SUM(Debit) END AS  TotalDebit FROM      
				                                                        (SELECT case when acc.category = 'Credit Note' or  acc.category = 'Debit Note' or acc.category = 'Invoice' then
							                                                            SUM(de.transamount)
							                                                        else     
							                                                          SUM(acc.TransAmount)
							                                                            END Debit
				                                                        FROM SAS_Accounts acc
				                                                        left join sas_accountsdetails de on acc.transid = de.transid
				                                                        WHERE acc.creditRef = SS.SASI_MatricNo and acc.subtype = 'Student' and acc.poststatus = 'Posted' and acc.transtype = 'Debit'
				                                                         GROUP BY acc.category, acc.TransType  
				                                                        )as Debit)
                                                        -
                                                        (SELECT CASE WHEN Sum(Credit) IS NULL THEN 0
				                                                        ELSE SUM(Credit) END AS  TotalCredit FROM
			                                                        (SELECT case when acc.category = 'Credit Note' or  acc.category = 'Debit Note' or acc.category = 'Invoice' then
									                                                SUM(de.transamount)
									                                            else     
									                                                SUM(acc.TransAmount)
									                                                END Credit
						                                            FROM SAS_Accounts acc
						                                            left join sas_accountsdetails de on acc.transid = de.transid
						                                            WHERE acc.creditRef = SS.SASI_MatricNo and acc.subtype = 'Student' and acc.poststatus = 'Posted' and acc.transtype = 'Credit'
						                                                GROUP BY acc.category, acc.TransType  
						                                            )as Credit)
                                                        as OutstandingAmount,                                                        
                                                        NULL AS SASO_LoanAmount ,
                                                        0 AS SASO_IsReleased,
                                                        SS.SASI_Add1,
                                                        SS.SASI_Add2,
                                                        SS.SASI_Add3,
                                                        SS.SASI_City,
                                                        SS.SASI_State,
                                                        SS.SASI_Postcode                                                        
                                              FROM      SAS_Accounts SA
                                                        INNER JOIN SAS_Student SS ON SA.CreditRef = SS.SASI_MatricNo
                                              WHERE     SA.SubType = 'Student'
                                                        AND SA.PostStatus = 'Posted'
                                            ) A
                                  WHERE A.OutstandingAmount > 0";


            if (argEn.CurretSemesterYear.Length != 0) sqlCmd = sqlCmd + " AND A.SASI_CurSemYr = '" + argEn.CurretSemesterYear + "'";
            if (argEn.ProgramID.Length != 0) sqlCmd = sqlCmd + " and A.SASI_PgId = '" + argEn.ProgramID + "'";

            sqlCmd = sqlCmd + @" order by A.SASI_MatricNo";
            try
            {
                if (!FormHelp.IsBlank(sqlCmd))
                {
                    using (IDataReader loReader = _DatabaseFactory.ExecuteReader(Helper.GetDataBaseType,
                        DataBaseConnectionString, sqlCmd).CreateDataReader())
                    {
                        while (loReader.Read())
                        {
                            StudentEn loItem = new StudentEn();
                            loItem.MatricNo = GetValue<string>(loReader, "SASI_MatricNo");
                            loItem.StudentName = GetValue<string>(loReader, "SASI_Name");
                            loItem.ProgramID = GetValue<string>(loReader, "SASI_PgId");
                            loItem.CurrentSemester = GetValue<int>(loReader, "SASI_CurSem");
                            loItem.SASI_Email = GetValue<string>(loReader, "SASI_Email");
                            loItem.OutstandingAmount = GetValue<double>(loReader, "OutstandingAmount");
                            loItem.SASI_Add1 = GetValue<string>(loReader, "SASI_Add1");
                            loItem.SASI_Add2 = GetValue<string>(loReader, "SASI_Add2");
                            loItem.SASI_Add3 = GetValue<string>(loReader, "SASI_Add3");
                            loItem.SASI_City = GetValue<string>(loReader, "SASI_City");
                            loItem.SASI_State = GetValue<string>(loReader, "SASI_State");
                            loItem.SASI_Postcode = GetValue<string>(loReader, "SASI_Postcode");                            
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

        #region GetListOutstanding

        /// <summary>
        ///  Method to Get List of Students who has Outstanding amount.
        /// </summary>
        /// <param name="argEn">Students Entity as an Input.Students Program Id, Semester Properties.</param>
        /// <param name="IncludeLoanAmount">Include Loan Amount Status</param>
        /// <returns>Returns List of Students</returns>
        /// /// Updated by Hafiz Roslan @ 5/2/2016
        /// Updated by Hafiz Roslan @ 16/2/2016
        /// Why? Outstanding amount needs to show all to assign flag
        /// Updated by Hafiz @ 24/2/2016
        /// Updated by Hafiz @ 26/2/2016
        /// 
        public List<StudentEn> GetListOutstanding(StudentEn argEn, bool IncludeLoanAmount, bool IncludeCurSem)
        {
            string sqlCmd = "";
            List<StudentEn> loEnList = new List<StudentEn>();

            argEn.ProgramID = argEn.ProgramID.Replace("*", "%");
            argEn.CurretSemesterYear = argEn.CurretSemesterYear.Replace("*", "%");

            if (IncludeLoanAmount)
            {
                if (IncludeCurSem)
                {
                    //added by Hafiz Roslan @ 5/2/2016
                    //updated by Hafiz @ 26/2/2016
                    sqlCmd = @"SELECT  
                                SASO.SASO_Id ,
                                SASO.SASI_MatricNo ,
                                SASO.SASI_Name ,
                                SASO.SASI_PgId ,
                                SASO.SASI_CurSem ,
                                SASO.SASI_CurSemYr ,
                                --exclude the AFC - START
                                --(COALESCE(SASO.SASO_DueAmount, 0) + COALESCE(SASO.SASO_LoanAmount, 0)) - 
                                (COALESCE(SASO.saso_outstandingamt, 0)) - 
		                             (
		                                 SELECT COALESCE(SUM(transamount),0)
		                                 FROM SAS_afc SAFC 
		                                 INNER JOIN SAS_Semestersetup SASSem ON SASSem.sast_code = SAFC.semester
		                                 INNER JOIN SAS_Accounts SASA ON SASA.batchcode = SAFC.batchcode
		                                 WHERE SASSem.SAST_iscurrentsem = TRUE
                                         AND SASA.poststatus = 'Posted'
		                                 AND SASA.creditref = SASO.SASI_MatricNo 
		                             ) AS SASO_DueAmount,
                                --exlude the AFC - END
                                --SASO.SASO_LoanAmount ,
                                SASO.SASO_IsReleased
                        FROM SAS_StudentOutstanding SASO 
                        INNER JOIN SAS_Student SASS ON SASS.SASI_MatricNo = SASO.SASI_MatricNo
                        --WHERE (COALESCE(SASO.SASO_DueAmount, 0) + COALESCE(SASO.SASO_LoanAmount, 0)) IS NOT NULL 
                        WHERE (COALESCE(SASO.saso_outstandingamt, 0)) IS NOT NULL 
                        AND SASS.SASS_Code IN ('1') ";
                }
                else
                {
                    sqlCmd = @"SELECT
                                SASO.SASO_Id ,
                                SASO.SASI_MatricNo ,
                                SASO.SASI_Name ,
                                SASO.SASI_PgId ,
                                SASO.SASI_CurSem ,
                                SASO.SASI_CurSemYr ,
                                --COALESCE(SASO.SASO_DueAmount, 0) + COALESCE(SASO.SASO_LoanAmount, 0) AS SASO_DueAmount ,
                                COALESCE(SASO.saso_outstandingamt, 0) AS SASO_DueAmount ,
                                SASO.SASO_LoanAmount ,
                                SASO.SASO_IsReleased
                    FROM SAS_StudentOutstanding SASO
                    INNER JOIN SAS_Student SASS ON SASS.SASI_MatricNo = SASO.SASI_MatricNo 
                    --WHERE (COALESCE(SASO.SASO_DueAmount, 0) + COALESCE(SASO.SASO_LoanAmount, 0)) IS NOT NULL 
                    WHERE (COALESCE(SASO.saso_outstandingamt, 0)) IS NOT NULL 
                    AND SASS.SASS_Code IN ('1') ";
                }
            }
            else
            {
                if (IncludeCurSem)
                {
                    //added by Hafiz Roslan @ 5/2/2016
                    //updated by Hafiz @ 26/2/2016
                    sqlCmd = @"SELECT DISTINCT 
                                SASO.SASO_Id ,
                                SASO.SASI_MatricNo ,
                                SASO.SASI_Name ,
                                SASO.SASI_PgId ,
                                SASO.SASI_CurSem ,
                                SASO.SASI_CurSemYr ,
                                --exclude the AFC - START
                                --COALESCE(SASO.SASO_DueAmount, 0) - 
                                COALESCE(SASO.saso_outstandingamt, 0) - 
		                            (
		                                SELECT COALESCE(SUM(transamount),0)
		                                FROM SAS_afc SAFC 
		                                INNER JOIN SAS_Semestersetup SASSem ON SASSem.sast_code = SAFC.semester
		                                INNER JOIN SAS_Accounts SASA ON SASA.batchcode = SAFC.batchcode
		                                WHERE SASSem.SAST_iscurrentsem = TRUE
                                        AND SASA.poststatus = 'Posted'		                                
                                        AND SASA.creditref = SASO.SASI_MatricNo 
		                            ) AS SASO_DueAmount,
                                --exlude the AFC - END
                                --SASO.SASO_LoanAmount ,
                                SASO.SASO_IsReleased
                    FROM SAS_StudentOutstanding SASO 
                    INNER JOIN SAS_Student SASS ON SASS.SASI_MatricNo = SASO.SASI_MatricNo
                    WHERE SASO.SASO_DueAmount IS NOT NULL 
                    AND SASS.SASS_Code IN ('1') ";
                }
                else
                {
                    sqlCmd = @"SELECT  
                                SASO.SASO_Id ,
                                SASO.SASI_MatricNo ,
                                SASO.SASI_Name ,
                                SASO.SASI_PgId ,
                                SASO.SASI_CurSem ,
                                SASO.SASI_CurSemYr ,
                                --SASO.SASO_DueAmount ,
                                SASO.saso_outstandingamt ,
                                --SASO.SASO_LoanAmount ,
                                SASO.SASO_IsReleased
                    FROM SAS_StudentOutstanding SASO
                    INNER JOIN SAS_Student SASS ON SASS.SASI_MatricNo = SASO.SASI_MatricNo
                    --WHERE SASO.SASO_DueAmount IS NOT NULL 
                    WHERE SASO.saso_outstandingamt IS NOT NULL 
                    AND SASS.SASS_Code IN ('1') ";
                }
            }

            if (argEn.ProgramID.Length != 0) sqlCmd = sqlCmd + " AND SASO.SASI_PgId = '" + argEn.ProgramID + "'";

            if (argEn.CurretSemesterYear.Length != 0) sqlCmd = sqlCmd + " AND SASO.SASI_CurSemYr = '" + argEn.CurretSemesterYear + "'";

            sqlCmd = sqlCmd + @" order by SASI_MatricNo";
            try
            {
                if (!FormHelp.IsBlank(sqlCmd))
                {
                    using (IDataReader loReader = _DatabaseFactory.ExecuteReader(Helper.GetDataBaseType,
                        DataBaseConnectionString, sqlCmd).CreateDataReader())
                    {
                        while (loReader.Read())
                        {
                            StudentEn loItem = new StudentEn();
                            loItem.MatricNo = GetValue<string>(loReader, "SASI_MatricNo");
                            loItem.StudentName = GetValue<string>(loReader, "SASI_Name");
                            loItem.ProgramID = GetValue<string>(loReader, "SASI_PgId");
                            loItem.CurrentSemester = GetValue<int>(loReader, "SASI_CurSem");
                            loItem.CurretSemesterYear = GetValue<string>(loReader, "SASI_CurSemYr");
                            //loItem.OutstandingAmount = GetValue<double>(loReader, "SASO_DueAmount");
                            loItem.OutstandingAmount = GetValue<double>(loReader, "saso_outstandingamt");
                            //loItem.LoanAmount = GetValue<double>(loReader, "SASO_LoanAmount");
                            loItem.IsReleased = GetValue<int>(loReader, "SASO_IsReleased");

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

        #region GetlisStudent

        /// <summary>
        /// Method to Get List of Students with Sponsers
        /// </summary>
        /// <param name="argEn">Students Entity as an Input.Sponsor,SAKO_Code,ProgramID,Faculty and CategoryCode as Input Properties.</param>
        /// <returns>Returns List of Students</returns>
        public List<StudentEn> GetlisStudent(StudentEn argEn)
        {
            string sqlCmd;
            List<StudentEn> loEnList = new List<StudentEn>();
            argEn.STsponsercode.Sponsor = argEn.STsponsercode.Sponsor.Replace("*", "%");
            argEn.SAKO_Code = argEn.SAKO_Code.Replace("*", "%");
            argEn.ProgramID = argEn.ProgramID.Replace("*", "%");
            argEn.Faculty = argEn.Faculty.Replace("*", "%");
            argEn.CategoryCode = argEn.CategoryCode.Replace("*", "%");
            // checking for student sponsor
            if (argEn.STsponsercode.Sponsor.Length != 0)
            {
                sqlCmd = " select Distinct  A.SASI_MatricNo, SASI_Name, A.SASI_PgId, A.SASI_CurSem from SAS_student A INNER JOIN SAS_StudentSpon B ON A.SASI_MAtricNO = B.SASI_MAtricNO" +
                                " INNER JOIN SAS_StudentCategoryAccess ON A.SASS_Code = SAS_StudentCategoryAccess.SASC_Code where " +
                                " (SAS_StudentCategoryAccess.MenuID = " + argEn.StCategoryAcess.MenuID + ") and (SAS_StudentCategoryAccess.Status = '1')" +
                                " and  B.SASS_Sponsor =" + argEn.STsponsercode.Sponsor + "";
            }
            else
            {
                sqlCmd = " select Distinct  A.SASI_MatricNo, SASI_Name, A.SASI_PgId, A.SASI_CurSem  from SAS_student A " +
                                " INNER JOIN SAS_StudentCategoryAccess ON A.SASS_Code = SAS_StudentCategoryAccess.SASC_Code where " +
                                " (SAS_StudentCategoryAccess.MenuID = " + argEn.StCategoryAcess.MenuID + ") and (SAS_StudentCategoryAccess.Status = '1')";

            }
            if (argEn.CategoryCode.Length != 0) sqlCmd = sqlCmd + " and A.SASC_Code ='" + argEn.CategoryCode + "'";
            if (argEn.CurrentSemester != 0) sqlCmd = sqlCmd + " and SASI_CurSem =" + argEn.CurrentSemester;
            if (argEn.SAKO_Code.Length != 0) sqlCmd = sqlCmd + " and SAKO_Code in(" + argEn.SAKO_Code + ")";
            if (argEn.ProgramID.Length != 0) sqlCmd = sqlCmd + " and SASI_PgId in (" + argEn.ProgramID + ")";
            if (argEn.Faculty.Length != 0) sqlCmd = sqlCmd + " and SASI_Faculty ='" + argEn.Faculty + "'";
            sqlCmd = sqlCmd + " order by A.SASI_MatricNo";
            try
            {
                if (!FormHelp.IsBlank(sqlCmd))
                {
                    using (IDataReader loReader = _DatabaseFactory.ExecuteReader(Helper.GetDataBaseType,
                        DataBaseConnectionString, sqlCmd).CreateDataReader())
                    {
                        while (loReader.Read())
                        {
                            StudentEn loItem = new StudentEn();
                            loItem.MatricNo = GetValue<string>(loReader, "SASI_MatricNo");
                            loItem.StudentName = GetValue<string>(loReader, "SASI_Name");
                            loItem.ProgramID = GetValue<string>(loReader, "SASI_PgId");
                            loItem.CurrentSemester = GetValue<int>(loReader, "SASI_CurSem");
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

        #region GetlistStudentByStudent

        /// <summary>
        /// Method to Get List of Students with Sponsers
        /// </summary>
        /// <param name="argEn">Students Entity as an Input.Sponsor,SAKO_Code,ProgramID,Faculty and CategoryCode as Input Properties.</param>
        /// <returns>Returns List of Students</returns>

        public List<StudentEn> GetlistStudentByStudent(StudentEn argEn)
        {
            string sqlCmd = string.Empty;
            List<StudentEn> loEnList = new List<StudentEn>();

            // checking for student sponsor
            if (argEn.STsponsercode.Sponsor.Length != 0)
            {
                //sqlCmd = " select Distinct  A.SASI_MatricNo, SASI_Name, A.SASI_PgId, A.SASI_CurSem,B.SASS_Sponsor from SAS_student A INNER JOIN SAS_StudentSpon B ON A.SASI_MAtricNO = B.SASI_MAtricNO" +
                //                " INNER JOIN SAS_StudentCategoryAccess ON A.SASS_Code = SAS_StudentCategoryAccess.SASC_Code where " +
                //                " (SAS_StudentCategoryAccess.MenuID = " + argEn.StCategoryAcess.MenuID + ") and (SAS_StudentCategoryAccess.Status = 'true')" +
                //                " and  B.SASS_Sponsor = ('" + argEn.STsponsercode.Sponsor + "')";
                sqlCmd = "select distinct A.sapg_code, A.sapg_program,B.SASI_CurSem,C.SASS_Sponsor from sas_program A INNER JOIN sas_student B on A.sapg_code = B.sasi_pgid " +
                            "Inner Join sas_studentspon C on B.sasi_matricno = C.sasi_matricno " +
                            "INNER JOIN  SAS_StudentCategoryAccess ON B.SASS_Code = SAS_StudentCategoryAccess.SASC_Code " +
                            "where C.sass_sponsor = ('" + argEn.STsponsercode.Sponsor + "') AND (SAS_StudentCategoryAccess.MenuID = " + argEn.StCategoryAcess.MenuID + ")" +
                            " and (SAS_StudentCategoryAccess.Status = 'true') AND sass_code IN (SELECT sass_code FROM sas_studentstatus where sass_blstatus  ='true')";
            }
            //sqlCmd = sqlCmd + " order by A.SASI_MatricNo";
            sqlCmd = sqlCmd + "Group By A.sapg_code, A.sapg_Program, B.SASI_MatricNo,B.SASI_Name,B.SASI_CurSem,C.SASS_Sponsor";
            try
            {
                if (!FormHelp.IsBlank(sqlCmd))
                {
                    using (IDataReader loReader = _DatabaseFactory.ExecuteReader(Helper.GetDataBaseType,
                        DataBaseConnectionString, sqlCmd).CreateDataReader())
                    {
                        while (loReader.Read())
                        {
                            StudentEn loItem = new StudentEn();
                            //loItem.MatricNo = GetValue<string>(loReader, "SASI_MatricNo");
                            //loItem.StudentName = GetValue<string>(loReader, "SASI_Name");
                            loItem.ProgramID = GetValue<string>(loReader, "sapg_code");
                            loItem.ProgramType = GetValue<string>(loReader, "sapg_program");
                            loItem.CurrentSemester = GetValue<int>(loReader, "SASI_CurSem");
                            loItem.SponsorCode = GetValue<string>(loReader, "SASS_Sponsor");
                            loItem.CreditRef = "0";
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

        public List<StudentEn> GetlistStudentByStudentWithValidity(StudentEn argEn)
        {
            string sqlCmd = string.Empty;
            List<StudentEn> loEnList = new List<StudentEn>();

            // checking for student sponsor
            if (argEn.STsponsercode.Sponsor.Length != 0)
            {
                //sqlCmd = " select Distinct  A.SASI_MatricNo, SASI_Name, A.SASI_PgId, A.SASI_CurSem,B.SASS_Sponsor from SAS_student A INNER JOIN SAS_StudentSpon B ON A.SASI_MAtricNO = B.SASI_MAtricNO" +
                //                " INNER JOIN SAS_StudentCategoryAccess ON A.SASS_Code = SAS_StudentCategoryAccess.SASC_Code where " +
                //                " (SAS_StudentCategoryAccess.MenuID = " + argEn.StCategoryAcess.MenuID + ") and (SAS_StudentCategoryAccess.Status = 'true')" +
                //                " and  B.SASS_Sponsor = ('" + argEn.STsponsercode.Sponsor + "')";
                sqlCmd = "select distinct A.sapg_code, A.sapg_program,C.SASS_Sponsor from sas_program A INNER JOIN sas_student B on A.sapg_code = B.sasi_pgid " +
                            "Inner Join sas_studentspon C on B.sasi_matricno = C.sasi_matricno " +
                            "INNER JOIN  SAS_StudentCategoryAccess ON B.SASS_Code = SAS_StudentCategoryAccess.SASC_Code " +
                            "where C.sass_sponsor = ('" + argEn.STsponsercode.Sponsor + "') AND (SAS_StudentCategoryAccess.MenuID = " + argEn.StCategoryAcess.MenuID + ")" +
                            " and (SAS_StudentCategoryAccess.Status = 'true') AND sass_code IN (SELECT sass_code FROM sas_studentstatus where sass_blstatus  ='true')" +
                           " and B.SASI_Reg_Status =" + Helper.StuRegistered
                            + " and B.sass_code =" + clsGeneric.AddQuotes(Helper.StuActive) +
                            " and TO_DATE( c.SASS_sDATE,'DD/MM/YYYY') <= current_date and TO_DATE( c.SASS_EDATE,'DD/MM/YYYY') >= current_date";
                //" and TO_DATE( c.SASS_sDATE,'DD/MM/YYYY') <= current_date and  TO_DATE( c.SASS_EDATE,'YYYY/MM/DD') >= current_date ";

                //sqlCmd = sqlCmd + " order by A.SASI_MatricNo";
                sqlCmd = sqlCmd + " Group By A.sapg_code, A.sapg_Program, B.SASI_MatricNo,B.SASI_Name,C.SASS_Sponsor";
            }
            try
            {
                if (!FormHelp.IsBlank(sqlCmd))
                {
                    using (IDataReader loReader = _DatabaseFactory.ExecuteReader(Helper.GetDataBaseType,
                        DataBaseConnectionString, sqlCmd).CreateDataReader())
                    {
                        while (loReader.Read())
                        {
                            StudentEn loItem = new StudentEn();
                            //loItem.MatricNo = GetValue<string>(loReader, "SASI_MatricNo");
                            //loItem.StudentName = GetValue<string>(loReader, "SASI_Name");
                            loItem.ProgramID = GetValue<string>(loReader, "sapg_code");
                            loItem.ProgramType = GetValue<string>(loReader, "sapg_program");
                            loItem.SponsorCode = GetValue<string>(loReader, "SASS_Sponsor");
                            loItem.CreditRef = "0";
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

        public List<StudentEn> GetStudentSponsorship(string programID, string Sponsor)
        {
            List<StudentEn> loEnList = new List<StudentEn>();
            List<AccountsDetailsEn> loAccounts = new List<AccountsDetailsEn>();
            string sqlCmdSponFeeTypes = "Select count(*) as cnt From sas_sponsorfeetypes where sasr_code = '" + Sponsor + "'";
            int iOut = 0;
            using (IDataReader dr = _DatabaseFactory.ExecuteReader(Helper.GetDataBaseType,
                     DataBaseConnectionString, sqlCmdSponFeeTypes).CreateDataReader())
            {
                if (dr.Read())
                    iOut = clsGeneric.NullToInteger(dr["cnt"]);
                dr.Close();
            }

            string sqlCmd = "";
            sqlCmd = @"select stu.sasi_matricno, stu.sasi_crdithrs, stu.sasi_name, stu.sasi_pgid, prog.sapg_programbm, stu.sasi_icno, stu.sasi_cursemyr, stu.sasi_intake, sspon.sass_sponsor, 
                  ( SELECT CASE WHEN fsd.safd_type = 'T' THEN stu.sasi_crdithrs * fsa.safa_amount
                   ELSE SUM(acc.transamount) END 
                    FROM   SAS_SponsorInvoice D  
	                        inner join sas_sponsor_inv_rec sir on sir.invoice_id = D.batchcode 
	                        inner join sas_accounts accRec on sir.receipt_id = accRec.transid and accRec.creditRef = D.creditRef1 
	                        inner join sas_accounts accAllocation on accAllocation.creditref = D.creditref1 and accRec.BatchCode = accAllocation.creditref1 
	                        inner join sas_accounts acc on acc.creditref1 = d.creditref1 and d.creditref = acc.creditref and accAllocation.BatchCode = acc.BatchCode 
	                        where acc.poststatus = 'Posted' and acc.transstatus = 'Closed' and acc.category ='SPA' and stu.SASI_MatricNo = D.creditref  
                 )AS tuition_amount,fsa.safa_gstamount,fsa.safa_amount,
                   ft.saft_desc, fsd.safs_taxmode, ft.saft_code, fs.safd_feebaseon,fs.safs_tutamt,
                   sspon.sass_limit, sspon.sass_type,
                        ( SELECT CASE WHEN SUM(acc.transamount) IS NULL THEN 0
                    ELSE SUM(acc.transamount) END 
                    FROM   SAS_SponsorInvoice D  
	                        inner join sas_sponsor_inv_rec sir on sir.invoice_id = D.batchcode 
	                        inner join sas_accounts accRec on sir.receipt_id = accRec.transid and accRec.creditRef = D.creditRef1 
	                        inner join sas_accounts accAllocation on accAllocation.creditref = D.creditref1 and accRec.BatchCode = accAllocation.creditref1 
	                        inner join sas_accounts acc on acc.creditref1 = d.creditref1 and d.creditref = acc.creditref and accAllocation.BatchCode = acc.BatchCode 
	                        where acc.poststatus = 'Posted' and acc.transstatus = 'Closed' and acc.category ='SPA' and stu.SASI_MatricNo = D.creditref  
                    )AS  AllocatedAmount, 
                    (select case when sspon.sass_limit = 0 THEN 0
                    ELSE
                        (sspon.sass_limit - (SELECT CASE WHEN SUM(acc.transamount) IS NULL THEN 0
                    ELSE SUM(acc.transamount) END 
                   FROM   SAS_SponsorInvoice D  
	                        inner join sas_sponsor_inv_rec sir on sir.invoice_id = D.batchcode 
	                        inner join sas_accounts accRec on sir.receipt_id = accRec.transid and accRec.creditRef = D.creditRef1 
	                        inner join sas_accounts accAllocation on accAllocation.creditref = D.creditref1 and accRec.BatchCode = accAllocation.creditref1 
	                        inner join sas_accounts acc on acc.creditref1 = d.creditref1 and d.creditref = acc.creditref and accAllocation.BatchCode = acc.BatchCode 
	                        where acc.poststatus = 'Posted' and acc.transstatus = 'Closed' and acc.category ='SPA' and stu.SASI_MatricNo = D.creditref  ))
                    END) AS AvailableAmont  ,
                    stu.sasc_code, sft.saft_code as SponFeeCode, fsd.safd_type,stu.sasi_hostel
                    from sas_student stu
                    INNER JOIN SAS_StudentSpon sspon ON stu.SASI_MAtricNO = sspon.SASI_MAtricNO 
                    left join sas_program prog on prog.sapg_code = stu.sasi_pgid
                    left join sas_feestruct fs on  prog.sabp_code = fs.sabp_code  and stu.sasi_intake = fs.sast_code 
                    left join sas_feestrdetails fsd on fs.safs_code = fsd.safs_code and 
                    (CASE stu.sasi_cursem
	                    WHEN 1 THEN 
	                    (fsd.safd_type = 'A' and fsd.safd_feefor = '0' and fsd.safd_sem = 0 )
	                    or ( fsd.safd_feefor = '0' and  fsd.safd_type = 'S' and 
	                     fsd.saft_code not in (select saft_code from sas_feestrdetails where safs_code =fs.safs_code and safd_type = 'A' and safd_feefor = '0' and safd_sem = 0  ))
or (fsd.safd_type = 'T' and fsd.safd_feefor = '0' and fsd.safd_sem = 0)  
	                     or (fsd.safd_sem = 1 and fsd.safd_type = 'S' and fsd.safd_feefor = '1' and
	                      fsd.saft_code not in (select saft_code from sas_feestrdetails where safs_code =fs.safs_code and safd_type = 'S' and safd_feefor = '0' ) 
	                      and fsd.saft_code not in (select saft_code from sas_feestrdetails where safs_code =fs.safs_code and safd_type = 'A' and safd_feefor = '0' and safd_sem = 0  ))
	                      else
	                      ( fsd.safd_feefor = '0' and  fsd.safd_type = 'S' ) or (fsd.safd_type = 'T' and fsd.safd_feefor = '0' and fsd.safd_sem = 0)
	                     or (fsd.safd_sem = stu.sasi_cursem and fsd.safd_type = 'S' and fsd.safd_feefor = '1' and
	                      fsd.saft_code not in (select saft_code from sas_feestrdetails where safs_code =fs.safs_code and safd_type = 'S' and safd_feefor = '0' ) )
                    END)
                     left join sas_feestramount fsa on fs.safs_code = fsa.safs_code and fsd.saft_code = fsa.saft_code and stu.sasc_code = fsa.sasc_code  
                    and fsd.saft_code = fsa.saft_code and fsa.safd_feefor = fsd.safd_feefor and fsa.safd_sem = fsd.safd_sem and fsa.safd_type = fsd.safd_type
                     left join sas_feetypes ft on ft.saft_code = fsd.saft_code 
                    left join sas_sponsorfeetypes sft on sspon.sass_sponsor = sft.sasr_code and ft.saft_code = sft.saft_code where sspon.sass_sponsor = " + clsGeneric.AddQuotes(Sponsor) +
           " and stu.sasi_reg_status = " + Helper.StuRegistered +
            " and stu.sass_code =" + clsGeneric.AddQuotes(Helper.StuActive) +
   " and TO_DATE( sspon.SASS_sDATE,'DD/MM/YYYY') <= current_date and TO_DATE( sspon.SASS_EDATE,'DD/MM/YYYY') >= current_date  union " +
                //---------------------------------------------------------------------------------------------------------------------------

@"select distinct stu.sasi_matricno, stu.sasi_crdithrs, stu.sasi_name, stu.sasi_pgid, prog.sapg_programbm, stu.sasi_icno, stu.sasi_cursemyr, stu.sasi_intake, sspon.sass_sponsor, kk.sakod_feeamount,kk.sakod_feeamountout,kk.sakod_gstout,ftp.saft_desc,kk.saft_taxmode,
kk.saft_code,koko.sako_glcode,kk.sakod_gstin,
            sspon.sass_limit,sspon.sass_type,
                        ( SELECT CASE WHEN SUM(acc.transamount) IS NULL THEN 0
                    ELSE SUM(acc.transamount) END 
                    FROM   SAS_SponsorInvoice D  
	                        inner join sas_sponsor_inv_rec sir on sir.invoice_id = D.batchcode 
	                        inner join sas_accounts accRec on sir.receipt_id = accRec.transid and accRec.creditRef = D.creditRef1 
	                        inner join sas_accounts accAllocation on accAllocation.creditref = D.creditref1 and accRec.BatchCode = accAllocation.creditref1 
	                        inner join sas_accounts acc on acc.creditref1 = d.creditref1 and d.creditref = acc.creditref and accAllocation.BatchCode = acc.BatchCode 
	                        where acc.poststatus = 'Posted' and acc.transstatus = 'Closed' and acc.category ='SPA' and stu.SASI_MatricNo = D.creditref  
                    )AS  AllocatedAmount, 
                   (select case when sspon.sass_limit = 0 THEN 0
                    ELSE
                        (sspon.sass_limit - (SELECT CASE WHEN SUM(acc.transamount) IS NULL THEN 0
                    ELSE SUM(acc.transamount) END 
                  FROM SAS_SponsorInvoice D  
	                        inner join sas_sponsor_inv_rec sir on sir.invoice_id = D.batchcode 
	                        inner join sas_accounts accRec on sir.receipt_id = accRec.transid and accRec.creditRef = D.creditRef1 
	                        inner join sas_accounts accAllocation on accAllocation.creditref = D.creditref1 and accRec.BatchCode = accAllocation.creditref1 
	                        inner join sas_accounts acc on acc.creditref1 = d.creditref1 and d.creditref = acc.creditref and accAllocation.BatchCode = acc.BatchCode 
	                        where acc.poststatus = 'Posted' and acc.transstatus = 'Closed' and acc.category ='SPA' and stu.SASI_MatricNo = D.creditref  ))
                    END) AS AvailableAmont  ,
                    stu.sasc_code,kk.sakod_categorycode,kk.safd_type,stu.sasi_hostel
                    from sas_student stu
                    
                    INNER JOIN SAS_StudentSpon sspon ON stu.SASI_MAtricNO = sspon.SASI_MAtricNO 
                    left join sas_program prog on prog.sapg_code = stu.sasi_pgid
                   left join sas_feestruct fs on  prog.sabp_code = fs.sabp_code  and stu.sasi_intake = fs.sast_code 
           left join SAS_Kokorikulum koko on koko.sako_code = stu.sasi_kokocode
             left join sas_kokorikulumdetails kk on kk.sako_code = koko.sako_code and kk.sakod_categorycode = stu.sasc_code 
             left join sas_feetypes ftp on ftp.saft_code = kk.saft_code where sspon.sass_sponsor = " + clsGeneric.AddQuotes(Sponsor) +
           " and stu.sasi_reg_status = " + Helper.StuRegistered +
            " and stu.sass_code =" + clsGeneric.AddQuotes(Helper.StuActive) +
   " and TO_DATE( sspon.SASS_sDATE,'DD/MM/YYYY') <= current_date and TO_DATE( sspon.SASS_EDATE,'DD/MM/YYYY') >= current_date  union " +

                      //---------------------------------------------------------------------------------------------------------------------------------------------------------------------

                  @" select distinct stu.sasi_matricno, stu.sasi_crdithrs, stu.sasi_name, stu.sasi_pgid, prog.sapg_programbm, stu.sasi_icno, stu.sasi_cursemyr, stu.sasi_intake, sspon.sass_sponsor, hsta.saha_amount,hsta.safa_gstamount,
fs.safs_tutamt,ft.saft_desc,ft.saft_taxmode,ft.saft_code,fs.sabp_code,fs.safs_cramt
,sspon.sass_limit,sspon.sass_type,
                        ( SELECT CASE WHEN SUM(acc.transamount) IS NULL THEN 0
                    ELSE SUM(acc.transamount) END 
                    FROM   SAS_SponsorInvoice D  
	                        inner join sas_sponsor_inv_rec sir on sir.invoice_id = D.batchcode 
	                        inner join sas_accounts accRec on sir.receipt_id = accRec.transid and accRec.creditRef = D.creditRef1 
	                        inner join sas_accounts accAllocation on accAllocation.creditref = D.creditref1 and accRec.BatchCode = accAllocation.creditref1 
	                        inner join sas_accounts acc on acc.creditref1 = d.creditref1 and d.creditref = acc.creditref and accAllocation.BatchCode = acc.BatchCode 
	                        where acc.poststatus = 'Posted' and acc.transstatus = 'Closed' and acc.category ='SPA' and stu.SASI_MatricNo = D.creditref  
                    )AS  AllocatedAmount, 
                    (select case when sspon.sass_limit = 0 THEN 0
                    ELSE
                        (sspon.sass_limit - (SELECT CASE WHEN SUM(acc.transamount) IS NULL THEN 0
                    ELSE SUM(acc.transamount) END 
                   FROM SAS_SponsorInvoice D  
	                        inner join sas_sponsor_inv_rec sir on sir.invoice_id = D.batchcode 
	                        inner join sas_accounts accRec on sir.receipt_id = accRec.transid and accRec.creditRef = D.creditRef1 
	                        inner join sas_accounts accAllocation on accAllocation.creditref = D.creditref1 and accRec.BatchCode = accAllocation.creditref1 
	                        inner join sas_accounts acc on acc.creditref1 = d.creditref1 and d.creditref = acc.creditref and accAllocation.BatchCode = acc.BatchCode 
	                        where acc.poststatus = 'Posted' and acc.transstatus = 'Closed' and acc.category ='SPA' and stu.SASI_MatricNo = D.creditref  ))
                    END) AS AvailableAmont  ,
                    stu.sasc_code, hs.sahb_code,ft.saft_hostel,stu.sasi_hostel
                    from sas_student stu
                    
                    INNER JOIN SAS_StudentSpon sspon ON stu.SASI_MAtricNO = sspon.SASI_MAtricNO 
                    left join sas_program prog on prog.sapg_code = stu.sasi_pgid
                    left join sas_feestruct fs on  prog.sabp_code = fs.sabp_code  and stu.sasi_intake = fs.sast_code 
                    left join sas_hostelstruct hs on hs.sahb_code = stu.sako_code and hs.sahb_block = stu.sabk_code 
             left join SAS_HostelStramount hsta on hsta.sasc_code = stu.sasc_code and hsta.sahs_code = hs.sahs_code         
             left join sas_feetypes ft on ft.saft_code = hsta.saft_code 
    
            where sspon.sass_sponsor = " + clsGeneric.AddQuotes(Sponsor) +
           " and stu.sasi_reg_status = " + Helper.StuRegistered +
            " and stu.sass_code =" + clsGeneric.AddQuotes(Helper.StuActive) +
   " and TO_DATE( sspon.SASS_sDATE,'DD/MM/YYYY') <= current_date and TO_DATE( sspon.SASS_EDATE,'DD/MM/YYYY') >= current_date" +
    " order by sasi_matricno";
                       
            try
            {
                if (!FormHelp.IsBlank(sqlCmd))
                {
                    using (IDataReader loReader = _DatabaseFactory.ExecuteReader(Helper.GetDataBaseType,
                        DataBaseConnectionString, sqlCmd).CreateDataReader())
                    {
                        while (loReader.Read())
                        {
                            StudentEn loItem = new StudentEn();
                            //AccountsEn accountsitem = new AccountsEn();
                            loItem.MatricNo = GetValue<string>(loReader, "sasi_matricno");
                            loItem.CrditHrDiff = GetValue<double>(loReader, "sasi_crdithrs");
                            loItem.StudentName = GetValue<string>(loReader, "sasi_name");
                            loItem.ProgramID = GetValue<string>(loReader, "sasi_pgid");
                            loItem.ICNo = GetValue<string>(loReader, "sasi_icno");
                            loItem.CurretSemesterYear = GetValue<string>(loReader, "sasi_cursemyr");
                            loItem.SponsorCode = GetValue<string>(loReader, "sass_sponsor");
                            loItem.ProgramName = GetValue<string>(loReader, "sapg_programbm");
                            loItem.CategoryCode = GetValue<string>(loReader, "sasc_code");
                            loItem.FullySponsor = GetValue<bool>(loReader, "sass_type");
                            loItem.SponFeeCode = GetValue<string>(loReader, "SponFeeCode");
                            loItem.SponsorLimit = GetValue<double>(loReader, "sass_limit");
                            loItem.PaidAmount = GetValue<double>(loReader, "AllocatedAmount");
                            loItem.OutstandingAmount = GetValue<double>(loReader, "AvailableAmont");
                            loItem.ReferenceCode = GetValue<string>(loReader, "saft_code");
                            loItem.Description = GetValue<string>(loReader, "saft_desc");
                            loItem.SART_Code = GetValue<string>(loReader, "safd_type");
                            loItem.FeeBaseOn = GetValue<string>(loReader, "safd_feebaseon");
                            loItem.SASI_StatusRec = GetValue<bool>(loReader, "sasi_hostel");
                            loItem.TaxId = GetValue<int>(loReader, "safs_taxmode");
                            if (loItem.SART_Code == "T")
                            {
                                GSTSetupDAL gst = new GSTSetupDAL();
                                decimal transamount;
                                decimal gstamount;
                                decimal total;
                                double calculate;
                                if (loItem.FeeBaseOn == "1")
                                {
                                    loItem.TransactionAmount = GetValue<double>(loReader, "safa_amount");
                                    loItem.GSTAmount = GetValue<double>(loReader, "safa_gstamount");
                                    //loItem.TransactionAmount = GetValue<double>(loReader, "tuition_amount");
                                    //transamount = System.Convert.ToDecimal(loItem.TransactionAmount);
                                    //gstamount = gst.GetGstAmount(loItem.TaxId, transamount);
                                    if (loItem.TaxId == 1)
                                    {
                                        calculate = (loItem.TransactionAmount - loItem.GSTAmount);
                                        total = System.Convert.ToDecimal(calculate * loItem.CrditHrDiff);
                                        gstamount = gst.GetGstAmount(loItem.TaxId, total);
                                        loItem.TransactionAmount = System.Convert.ToDouble(gstamount) + System.Convert.ToDouble(total);
                                        loItem.GSTAmount = System.Convert.ToDouble(gstamount);
                                        loItem.TaxAmount = System.Convert.ToDouble(gstamount);
                                    }
                                    else
                                    {
                                        transamount = System.Convert.ToDecimal(loItem.TransactionAmount);
                                        gstamount = gst.GetGstAmount(loItem.TaxId, transamount);
                                        loItem.TransactionAmount = System.Convert.ToDouble(transamount);
                                        loItem.GSTAmount = System.Convert.ToDouble(gstamount);
                                        loItem.TaxAmount = System.Convert.ToDouble(gstamount);
                                    }
                                }
                                else if (loItem.FeeBaseOn == "0")
                                {
                                    loItem.TransactionAmount = GetValue<double>(loReader, "safa_amount");
                                    transamount = System.Convert.ToDecimal(loItem.TransactionAmount);
                                    gstamount = gst.GetGstAmount(loItem.TaxId, transamount);
                                    loItem.GSTAmount = GetValue<double>(loReader, "safa_gstamount");
                                    loItem.TaxAmount = GetValue<double>(loReader, "safa_gstamount");
                                    //if (loItem.TaxId == 1)
                                    //{
                                    //    loItem.TransactionAmount = System.Convert.ToDouble(gstamount) + System.Convert.ToDouble(transamount);
                                    //    loItem.GSTAmount = System.Convert.ToDouble(gstamount);
                                    //    loItem.TaxAmount = System.Convert.ToDouble(gstamount);
                                    //}
                                    //else
                                    //{
                                    //    loItem.GSTAmount = System.Convert.ToDouble(gstamount);
                                    //    loItem.TaxAmount = System.Convert.ToDouble(gstamount);
                                    //}
                                }
                            }
                            else if (loItem.SART_Code == "K")
                            {
                                if (loItem.SASI_StatusRec == true)
                                {
                                    loItem.TransactionAmount = GetValue<double>(loReader, "tuition_amount");
                                    loItem.GSTAmount = GetValue<double>(loReader, "safs_tutamt");
                                    loItem.TaxAmount = GetValue<double>(loReader, "safs_tutamt");
                                    //loItem.TransactionAmount = loItem.TransactionAmount - loItem.GSTAmount;
                                }
                                else if (loItem.SASI_StatusRec == false)
                                {
                                    loItem.TransactionAmount = GetValue<double>(loReader, "safa_gstamount");
                                    loItem.GSTAmount = GetValue<double>(loReader, "safa_amount");
                                    loItem.TaxAmount = GetValue<double>(loReader, "safa_amount");
                                    //loItem.TransactionAmount = loItem.TransactionAmount - loItem.GSTAmount;
                                }
                            }
                            else if (loItem.SART_Code == "H")
                            {

                                loItem.TransactionAmount = GetValue<double>(loReader, "tuition_amount");
                                loItem.GSTAmount = GetValue<double>(loReader, "safa_gstamount");
                                loItem.TaxAmount = GetValue<double>(loReader, "safa_gstamount");
                            }
                            else
                            {
                                loItem.TransactionAmount = GetValue<double>(loReader, "safa_amount");
                                loItem.GSTAmount = GetValue<double>(loReader, "safa_gstamount");
                                loItem.TaxAmount = GetValue<double>(loReader, "safa_gstamount");
                            }
                            if (loItem.ReferenceCode != null)
                            {
                                loEnList.Add(loItem);
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
            return loEnList;
        }

        #endregion

        #region GetSponsorFeeList

        /// <summary>
        /// Method to Get List of Sponsor FeeTypes by SponsorCode
        /// </summary>
        /// <param name="argEn">SponsorFeeTypes Entity as an Input.SponsorCode as Input Property.</param>
        /// <returns>Returns List of SponsorFeeTypes</returns>
        public List<StudentEn> GetSponsorFeeList(string Sponsor)
        {
            //List<SponsorFeeTypesEn> loEnList = new List<SponsorFeeTypesEn>();
            List<StudentEn> loEnList = new List<StudentEn>();
            List<AccountsDetailsEn> loAccounts = new List<AccountsDetailsEn>();
            string sqlCmd = "select SFT.SASR_Code,SFT.SAFT_Code,FT.SAFT_Desc from SAS_SponsorFeeTypes SFT INNER JOIN " +
            "SAS_FeeTypes FT ON SFT.SAFT_Code=FT.SAFT_Code where SFT.SASR_Code= " + clsGeneric.AddQuotes(Sponsor);

            try
            {
                if (!FormHelp.IsBlank(sqlCmd))
                {

                    using (IDataReader loReader = _DatabaseFactory.ExecuteReader(Helper.GetDataBaseType,
                        DataBaseConnectionString, sqlCmd).CreateDataReader())
                    {
                        while (loReader.Read())
                        {
                            StudentEn loItem = new StudentEn();
                            loItem.ReferenceCode = GetValue<string>(loReader, "SAFT_Code");
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
        
        #region GetStudentSponsorshipWithoutValidity

        public List<StudentEn> GetStudentSponsorshipWithoutValidity(string MatricNo)
        {
            List<StudentEn> loEnList = new List<StudentEn>();
            List<AccountsDetailsEn> loAccounts = new List<AccountsDetailsEn>();
           

            string sqlCmd = "";
            sqlCmd = @"select stu.sasi_matricno, stu.sasi_crdithrs, stu.sasi_name, stu.sasi_pgid, prog.sapg_programbm, stu.sasi_icno, stu.sasi_cursemyr, stu.sasi_intake, sspon.sass_sponsor, 
                  ( SELECT CASE WHEN fsd.safd_type = 'T' THEN stu.sasi_crdithrs * fsa.safa_amount
                   ELSE SUM(acc.transamount) END 
                    FROM   SAS_SponsorInvoice D  
	                        inner join sas_sponsor_inv_rec sir on sir.invoice_id = D.batchcode 
	                        inner join sas_accounts accRec on sir.receipt_id = accRec.transid and accRec.creditRef = D.creditRef1 
	                        inner join sas_accounts accAllocation on accAllocation.creditref = D.creditref1 and accRec.BatchCode = accAllocation.creditref1 
	                        inner join sas_accounts acc on acc.creditref1 = d.creditref1 and d.creditref = acc.creditref and accAllocation.BatchCode = acc.BatchCode 
	                        where acc.poststatus = 'Posted' and acc.transstatus = 'Closed' and acc.category ='SPA' and stu.SASI_MatricNo = D.creditref  
                 )AS tuition_amount,fsa.safa_gstamount,fsa.safa_amount,
                   ft.saft_desc, fsd.safs_taxmode, ft.saft_code, fs.safd_feebaseon,fs.safs_tutamt,
                   sspon.sass_limit, sspon.sass_type,
                        ( SELECT CASE WHEN SUM(acc.transamount) IS NULL THEN 0
                    ELSE SUM(acc.transamount) END 
                    FROM   SAS_SponsorInvoice D  
	                        inner join sas_sponsor_inv_rec sir on sir.invoice_id = D.batchcode 
	                        inner join sas_accounts accRec on sir.receipt_id = accRec.transid and accRec.creditRef = D.creditRef1 
	                        inner join sas_accounts accAllocation on accAllocation.creditref = D.creditref1 and accRec.BatchCode = accAllocation.creditref1 
	                        inner join sas_accounts acc on acc.creditref1 = d.creditref1 and d.creditref = acc.creditref and accAllocation.BatchCode = acc.BatchCode 
	                        where acc.poststatus = 'Posted' and acc.transstatus = 'Closed' and acc.category ='SPA' and stu.SASI_MatricNo = D.creditref  
                    )AS  AllocatedAmount, 
                    (select case when sspon.sass_limit = 0 THEN 0
                    ELSE
                        (sspon.sass_limit - (SELECT CASE WHEN SUM(acc.transamount) IS NULL THEN 0
                    ELSE SUM(acc.transamount) END 
                   FROM   SAS_SponsorInvoice D  
	                        inner join sas_sponsor_inv_rec sir on sir.invoice_id = D.batchcode 
	                        inner join sas_accounts accRec on sir.receipt_id = accRec.transid and accRec.creditRef = D.creditRef1 
	                        inner join sas_accounts accAllocation on accAllocation.creditref = D.creditref1 and accRec.BatchCode = accAllocation.creditref1 
	                        inner join sas_accounts acc on acc.creditref1 = d.creditref1 and d.creditref = acc.creditref and accAllocation.BatchCode = acc.BatchCode 
	                        where acc.poststatus = 'Posted' and acc.transstatus = 'Closed' and acc.category ='SPA' and stu.SASI_MatricNo = D.creditref  ))
                    END) AS AvailableAmont  ,
                    stu.sasc_code, sft.saft_code as SponFeeCode, fsd.safd_type,stu.sasi_hostel
                    from sas_student stu
                    INNER JOIN SAS_StudentSpon sspon ON stu.SASI_MAtricNO = sspon.SASI_MAtricNO 
                    left join sas_program prog on prog.sapg_code = stu.sasi_pgid
                    left join sas_feestruct fs on  prog.sabp_code = fs.sabp_code  and stu.sasi_intake = fs.sast_code 
                    left join sas_feestrdetails fsd on fs.safs_code = fsd.safs_code and 
                    (CASE stu.sasi_cursem
	                    WHEN 1 THEN 
	                    (fsd.safd_type = 'A' and fsd.safd_feefor = '0' and fsd.safd_sem = 0 )
	                    or ( fsd.safd_feefor = '0' and  fsd.safd_type = 'S' and 
	                     fsd.saft_code not in (select saft_code from sas_feestrdetails where safs_code =fs.safs_code and safd_type = 'A' and safd_feefor = '0' and safd_sem = 0  ))
or (fsd.safd_type = 'T' and fsd.safd_feefor = '0' and fsd.safd_sem = 0)  
	                     or (fsd.safd_sem = 1 and fsd.safd_type = 'S' and fsd.safd_feefor = '1' and
	                      fsd.saft_code not in (select saft_code from sas_feestrdetails where safs_code =fs.safs_code and safd_type = 'S' and safd_feefor = '0' ) 
	                      and fsd.saft_code not in (select saft_code from sas_feestrdetails where safs_code =fs.safs_code and safd_type = 'A' and safd_feefor = '0' and safd_sem = 0  ))
	                      else
	                      ( fsd.safd_feefor = '0' and  fsd.safd_type = 'S' ) or (fsd.safd_type = 'T' and fsd.safd_feefor = '0' and fsd.safd_sem = 0)
	                     or (fsd.safd_sem = stu.sasi_cursem and fsd.safd_type = 'S' and fsd.safd_feefor = '1' and
	                      fsd.saft_code not in (select saft_code from sas_feestrdetails where safs_code =fs.safs_code and safd_type = 'S' and safd_feefor = '0' ) )
                    END)
                     left join sas_feestramount fsa on fs.safs_code = fsa.safs_code and fsd.saft_code = fsa.saft_code and stu.sasc_code = fsa.sasc_code  
                    and fsd.saft_code = fsa.saft_code and fsa.safd_feefor = fsd.safd_feefor and fsa.safd_sem = fsd.safd_sem and fsa.safd_type = fsd.safd_type
                     left join sas_feetypes ft on ft.saft_code = fsd.saft_code 
                    left join sas_sponsorfeetypes sft on sspon.sass_sponsor = sft.sasr_code and ft.saft_code = sft.saft_code where stu.sasi_matricno = " + clsGeneric.AddQuotes(MatricNo) +
           "union " +
                //---------------------------------------------------------------------------------------------------------------------------

@"select distinct stu.sasi_matricno, stu.sasi_crdithrs, stu.sasi_name, stu.sasi_pgid, prog.sapg_programbm, stu.sasi_icno, stu.sasi_cursemyr, stu.sasi_intake, sspon.sass_sponsor, kk.sakod_feeamount,kk.sakod_feeamountout,kk.sakod_gstout,ftp.saft_desc,kk.saft_taxmode,
kk.saft_code,koko.sako_glcode,kk.sakod_gstin,
            sspon.sass_limit,sspon.sass_type,
                        ( SELECT CASE WHEN SUM(acc.transamount) IS NULL THEN 0
                    ELSE SUM(acc.transamount) END 
                    FROM   SAS_SponsorInvoice D  
	                        inner join sas_sponsor_inv_rec sir on sir.invoice_id = D.batchcode 
	                        inner join sas_accounts accRec on sir.receipt_id = accRec.transid and accRec.creditRef = D.creditRef1 
	                        inner join sas_accounts accAllocation on accAllocation.creditref = D.creditref1 and accRec.BatchCode = accAllocation.creditref1 
	                        inner join sas_accounts acc on acc.creditref1 = d.creditref1 and d.creditref = acc.creditref and accAllocation.BatchCode = acc.BatchCode 
	                        where acc.poststatus = 'Posted' and acc.transstatus = 'Closed' and acc.category ='SPA' and stu.SASI_MatricNo = D.creditref  
                    )AS  AllocatedAmount, 
                   (select case when sspon.sass_limit = 0 THEN 0
                    ELSE
                        (sspon.sass_limit - (SELECT CASE WHEN SUM(acc.transamount) IS NULL THEN 0
                    ELSE SUM(acc.transamount) END 
                  FROM SAS_SponsorInvoice D  
	                        inner join sas_sponsor_inv_rec sir on sir.invoice_id = D.batchcode 
	                        inner join sas_accounts accRec on sir.receipt_id = accRec.transid and accRec.creditRef = D.creditRef1 
	                        inner join sas_accounts accAllocation on accAllocation.creditref = D.creditref1 and accRec.BatchCode = accAllocation.creditref1 
	                        inner join sas_accounts acc on acc.creditref1 = d.creditref1 and d.creditref = acc.creditref and accAllocation.BatchCode = acc.BatchCode 
	                        where acc.poststatus = 'Posted' and acc.transstatus = 'Closed' and acc.category ='SPA' and stu.SASI_MatricNo = D.creditref  ))
                    END) AS AvailableAmont  ,
                    stu.sasc_code,kk.sakod_categorycode,kk.safd_type,stu.sasi_hostel
                    from sas_student stu
                    
                    INNER JOIN SAS_StudentSpon sspon ON stu.SASI_MAtricNO = sspon.SASI_MAtricNO 
                    left join sas_program prog on prog.sapg_code = stu.sasi_pgid
                   left join sas_feestruct fs on  prog.sabp_code = fs.sabp_code  and stu.sasi_intake = fs.sast_code 
           left join SAS_Kokorikulum koko on koko.sako_code = stu.sasi_kokocode
             left join sas_kokorikulumdetails kk on kk.sako_code = koko.sako_code and kk.sakod_categorycode = stu.sasc_code 
             left join sas_feetypes ftp on ftp.saft_code = kk.saft_code where stu.sasi_matricno = " + clsGeneric.AddQuotes(MatricNo) +
   " union " +

                      //---------------------------------------------------------------------------------------------------------------------------------------------------------------------

                  @" select distinct stu.sasi_matricno, stu.sasi_crdithrs, stu.sasi_name, stu.sasi_pgid, prog.sapg_programbm, stu.sasi_icno, stu.sasi_cursemyr, stu.sasi_intake, sspon.sass_sponsor, hsta.saha_amount,hsta.safa_gstamount,
fs.safs_tutamt,ft.saft_desc,ft.saft_taxmode,ft.saft_code,fs.sabp_code,fs.safs_cramt
,sspon.sass_limit,sspon.sass_type,
                        ( SELECT CASE WHEN SUM(acc.transamount) IS NULL THEN 0
                    ELSE SUM(acc.transamount) END 
                    FROM   SAS_SponsorInvoice D  
	                        inner join sas_sponsor_inv_rec sir on sir.invoice_id = D.batchcode 
	                        inner join sas_accounts accRec on sir.receipt_id = accRec.transid and accRec.creditRef = D.creditRef1 
	                        inner join sas_accounts accAllocation on accAllocation.creditref = D.creditref1 and accRec.BatchCode = accAllocation.creditref1 
	                        inner join sas_accounts acc on acc.creditref1 = d.creditref1 and d.creditref = acc.creditref and accAllocation.BatchCode = acc.BatchCode 
	                        where acc.poststatus = 'Posted' and acc.transstatus = 'Closed' and acc.category ='SPA' and stu.SASI_MatricNo = D.creditref  
                    )AS  AllocatedAmount, 
                    (select case when sspon.sass_limit = 0 THEN 0
                    ELSE
                        (sspon.sass_limit - (SELECT CASE WHEN SUM(acc.transamount) IS NULL THEN 0
                    ELSE SUM(acc.transamount) END 
                   FROM SAS_SponsorInvoice D  
	                        inner join sas_sponsor_inv_rec sir on sir.invoice_id = D.batchcode 
	                        inner join sas_accounts accRec on sir.receipt_id = accRec.transid and accRec.creditRef = D.creditRef1 
	                        inner join sas_accounts accAllocation on accAllocation.creditref = D.creditref1 and accRec.BatchCode = accAllocation.creditref1 
	                        inner join sas_accounts acc on acc.creditref1 = d.creditref1 and d.creditref = acc.creditref and accAllocation.BatchCode = acc.BatchCode 
	                        where acc.poststatus = 'Posted' and acc.transstatus = 'Closed' and acc.category ='SPA' and stu.SASI_MatricNo = D.creditref  ))
                    END) AS AvailableAmont  ,
                    stu.sasc_code, hs.sahb_code,ft.saft_hostel,stu.sasi_hostel
                    from sas_student stu
                    
                    INNER JOIN SAS_StudentSpon sspon ON stu.SASI_MAtricNO = sspon.SASI_MAtricNO 
                    left join sas_program prog on prog.sapg_code = stu.sasi_pgid
                    left join sas_feestruct fs on  prog.sabp_code = fs.sabp_code  and stu.sasi_intake = fs.sast_code 
                    left join sas_hostelstruct hs on hs.sahb_code = stu.sako_code and hs.sahb_block = stu.sabk_code 
             left join SAS_HostelStramount hsta on hsta.sasc_code = stu.sasc_code and hsta.sahs_code = hs.sahs_code         
             left join sas_feetypes ft on ft.saft_code = hsta.saft_code 
    
            where stu.sasi_matricno = " + clsGeneric.AddQuotes(MatricNo) +
    " order by sasi_matricno";
                       
            try
            {
                if (!FormHelp.IsBlank(sqlCmd))
                {
                    using (IDataReader loReader = _DatabaseFactory.ExecuteReader(Helper.GetDataBaseType,
                        DataBaseConnectionString, sqlCmd).CreateDataReader())
                    {
                        while (loReader.Read())
                        {
                            StudentEn loItem = new StudentEn();
                            //AccountsEn accountsitem = new AccountsEn();
                            loItem.MatricNo = GetValue<string>(loReader, "sasi_matricno");
                            loItem.CrditHrDiff = GetValue<double>(loReader, "sasi_crdithrs");
                            loItem.StudentName = GetValue<string>(loReader, "sasi_name");
                            loItem.ProgramID = GetValue<string>(loReader, "sasi_pgid");
                            loItem.ICNo = GetValue<string>(loReader, "sasi_icno");
                            loItem.CurretSemesterYear = GetValue<string>(loReader, "sasi_cursemyr");
                            loItem.SponsorCode = GetValue<string>(loReader, "sass_sponsor");
                            loItem.ProgramName = GetValue<string>(loReader, "sapg_programbm");
                            loItem.CategoryCode = GetValue<string>(loReader, "sasc_code");
                            loItem.FullySponsor = GetValue<bool>(loReader, "sass_type");
                            loItem.SponFeeCode = GetValue<string>(loReader, "SponFeeCode");
                            loItem.SponsorLimit = GetValue<double>(loReader, "sass_limit");
                            loItem.PaidAmount = GetValue<double>(loReader, "AllocatedAmount");
                            loItem.OutstandingAmount = GetValue<double>(loReader, "AvailableAmont");
                            loItem.ReferenceCode = GetValue<string>(loReader, "saft_code");
                            loItem.Description = GetValue<string>(loReader, "saft_desc");
                            loItem.SART_Code = GetValue<string>(loReader, "safd_type");
                            loItem.FeeBaseOn = GetValue<string>(loReader, "safd_feebaseon");
                            loItem.SASI_StatusRec = GetValue<bool>(loReader, "sasi_hostel");
                            loItem.TaxId = GetValue<int>(loReader, "safs_taxmode");
                            if (loItem.SART_Code == "T")
                            {
                                GSTSetupDAL gst = new GSTSetupDAL();
                                decimal transamount;
                                decimal gstamount;
                                decimal total;
                                double calculate;
                                if (loItem.FeeBaseOn == "1")
                                {
                                    loItem.TransactionAmount = GetValue<double>(loReader, "safa_amount");
                                    loItem.GSTAmount = GetValue<double>(loReader, "safa_gstamount");
                                    //loItem.TransactionAmount = GetValue<double>(loReader, "tuition_amount");
                                    //transamount = System.Convert.ToDecimal(loItem.TransactionAmount);
                                    //gstamount = gst.GetGstAmount(loItem.TaxId, transamount);
                                    if (loItem.TaxId == 1)
                                    {
                                        calculate = (loItem.TransactionAmount - loItem.GSTAmount);
                                        total = System.Convert.ToDecimal(calculate * loItem.CrditHrDiff);
                                        gstamount = gst.GetGstAmount(loItem.TaxId, total);
                                        loItem.TransactionAmount = System.Convert.ToDouble(gstamount) + System.Convert.ToDouble(total);
                                        loItem.GSTAmount = System.Convert.ToDouble(gstamount);
                                        loItem.TaxAmount = System.Convert.ToDouble(gstamount);
                                    }
                                    else
                                    {
                                        transamount = System.Convert.ToDecimal(loItem.TransactionAmount);
                                        gstamount = gst.GetGstAmount(loItem.TaxId, transamount);
                                        loItem.TransactionAmount = System.Convert.ToDouble(transamount);
                                        loItem.GSTAmount = System.Convert.ToDouble(gstamount);
                                        loItem.TaxAmount = System.Convert.ToDouble(gstamount);
                                    }
                                }
                                else if (loItem.FeeBaseOn == "0")
                                {
                                    loItem.TransactionAmount = GetValue<double>(loReader, "safa_amount");
                                    transamount = System.Convert.ToDecimal(loItem.TransactionAmount);
                                    gstamount = gst.GetGstAmount(loItem.TaxId, transamount);
                                    loItem.GSTAmount = GetValue<double>(loReader, "safa_gstamount");
                                    loItem.TaxAmount = GetValue<double>(loReader, "safa_gstamount");
                                    //if (loItem.TaxId == 1)
                                    //{
                                    //    loItem.TransactionAmount = System.Convert.ToDouble(gstamount) + System.Convert.ToDouble(transamount);
                                    //    loItem.GSTAmount = System.Convert.ToDouble(gstamount);
                                    //    loItem.TaxAmount = System.Convert.ToDouble(gstamount);
                                    //}
                                    //else
                                    //{
                                    //    loItem.GSTAmount = System.Convert.ToDouble(gstamount);
                                    //    loItem.TaxAmount = System.Convert.ToDouble(gstamount);
                                    //}
                                }
                            }
                            else if (loItem.SART_Code == "K")
                            {
                                if (loItem.SASI_StatusRec == true)
                                {
                                    loItem.TransactionAmount = GetValue<double>(loReader, "tuition_amount");
                                    loItem.GSTAmount = GetValue<double>(loReader, "safs_tutamt");
                                    loItem.TaxAmount = GetValue<double>(loReader, "safs_tutamt");
                                    //loItem.TransactionAmount = loItem.TransactionAmount - loItem.GSTAmount;
                                }
                                else if (loItem.SASI_StatusRec == false)
                                {
                                    loItem.TransactionAmount = GetValue<double>(loReader, "safa_gstamount");
                                    loItem.GSTAmount = GetValue<double>(loReader, "safa_amount");
                                    loItem.TaxAmount = GetValue<double>(loReader, "safa_amount");
                                    //loItem.TransactionAmount = loItem.TransactionAmount - loItem.GSTAmount;
                                }
                            }
                            else if (loItem.SART_Code == "H")
                            {

                                loItem.TransactionAmount = GetValue<double>(loReader, "tuition_amount");
                                loItem.GSTAmount = GetValue<double>(loReader, "safa_gstamount");
                                loItem.TaxAmount = GetValue<double>(loReader, "safa_gstamount");
                            }
                            else
                            {
                                loItem.TransactionAmount = GetValue<double>(loReader, "safa_amount");
                                loItem.GSTAmount = GetValue<double>(loReader, "safa_gstamount");
                                loItem.TaxAmount = GetValue<double>(loReader, "safa_gstamount");
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

        #region CheckStudentList

        /// <summary>
        /// Method to Check For Students
        /// </summary>
        /// <param name="argEn">Students Entity as an Input.MatricNo,StudentName,ICNo,ProgramId and Faculty as Input Properties.</param>
        /// <returns>Returns List of Students</returns>
        public List<StudentEn> CheckStudentList(StudentEn argEn)
        {
            string sqlCmd;
            List<StudentEn> loEnList = new List<StudentEn>();
            argEn.MatricNo = argEn.MatricNo.Replace("*", "%");
            if (argEn.StudentName.Length != 0) argEn.StudentName = argEn.StudentName.Replace("*", "%");
            if (argEn.ICNo.Length != 0) argEn.ICNo = argEn.ICNo.Replace("*", "%");
            if (argEn.ProgramID.Length != 0) argEn.ProgramID = argEn.ProgramID.Replace("*", "%");
            if (argEn.Faculty.Length != 0) argEn.Faculty = argEn.Faculty.Replace("*", "%");
            //Checking for student sponsor
            if (argEn.STsponsercode.Sponsor.Length != 0)
            {
                sqlCmd = " SELECT SAS_Student.SASI_MatricNo, SAS_Student.SASI_Name, SAS_Student.SASI_PgId, SAS_Student.SASI_Faculty," +
                         " SAS_Student.SASI_ICNo, SAS_Student.SASI_CurSem, SAS_StudentSpon.SASS_Sponsor, SAS_StudentSpon.SASS_SDate," +
                         " SAS_StudentSpon.SASS_EDate FROM SAS_Student INNER JOIN SAS_StudentSpon ON SAS_Student.SASI_MatricNo = SAS_StudentSpon.SASI_MatricNo " +
                         " where SAS_StudentSpon.SASS_Sponsor = '" + argEn.STsponsercode.Sponsor + " ' ";
                if (argEn.MatricNo.Length != 0) sqlCmd = sqlCmd + " and SAS_Student.SASI_MatricNo ='" + argEn.MatricNo + "'";
            }
            else
            {
                sqlCmd = " SELECT SAS_Student.SASI_MatricNo, SAS_Student.SASI_Name, SAS_Student.SASI_PgId, SAS_Student.SASI_Faculty," +
                          " SAS_Student.SASI_ICNo, SAS_Student.SASI_CurSem, SAS_StudentSpon.SASS_Sponsor, SAS_StudentSpon.SASS_EDate, " +
                          " SAS_StudentSpon.SASS_SDate FROM SAS_Student LEFT OUTER JOIN SAS_StudentSpon ON SAS_Student.SASI_MatricNo = SAS_StudentSpon.SASI_MatricNo" +
                          " where  SAS_Student.SASI_MatricNo = '" + argEn.MatricNo + "'";
            }

            if (argEn.CurrentSemester != 0) sqlCmd = sqlCmd + " and SAS_Student.SASI_CurSem =" + argEn.CurrentSemester;
            if (argEn.ICNo.Length != 0) sqlCmd = sqlCmd + " and SAS_Student.SASI_ICNo ='" + argEn.ICNo + "'";
            if (argEn.ProgramID.Length != 0) sqlCmd = sqlCmd + " and SAS_Student.SASI_PgId = '" + argEn.ProgramID + "'";
            if (argEn.Faculty.Length != 0) sqlCmd = sqlCmd + " and SAS_Student.SASI_Faculty = '" + argEn.Faculty + "'";
            //if (argEn.SASI_StatusRec == true) sqlCmd = sqlCmd + " and SAS_Student.SASI_StatusRec = 1";
            if (argEn.SASI_StatusRec == true) sqlCmd = sqlCmd + " and SAS_Student.SASI_StatusRec = 'true'";
            //if (argEn.SASI_StatusRec == false) sqlCmd = sqlCmd + " and SAS_Student.SASI_StatusRec = 0";
            if (argEn.SASI_StatusRec == false) sqlCmd = sqlCmd + " and SAS_Student.SASI_StatusRec = 'false'";
            sqlCmd = sqlCmd + " order by SAS_Student.SASI_MatricNo";
            try
            {
                if (!FormHelp.IsBlank(sqlCmd))
                {
                    using (IDataReader loReader = _DatabaseFactory.ExecuteReader(Helper.GetDataBaseType,
                        DataBaseConnectionString, sqlCmd).CreateDataReader())
                    {
                        while (loReader.Read())
                        {
                            StudentEn loItem = new StudentEn();

                            loItem.STsponsercode = new StudentSponEn();
                            loItem.MatricNo = GetValue<string>(loReader, "SASI_MatricNo");
                            loItem.StudentName = GetValue<string>(loReader, "SASI_Name");
                            loItem.ICNo = GetValue<string>(loReader, "SASI_ICNo");
                            loItem.ProgramID = GetValue<string>(loReader, "SASI_PgId");
                            loItem.Faculty = GetValue<string>(loReader, "SASI_Faculty");
                            loItem.CurrentSemester = GetValue<int>(loReader, "SASI_CurSem");
                            loItem.STsponsercode.Sponsor = GetValue<string>(loReader, "SASS_Sponsor");
                            loItem.STsponsercode.SDate = GetValue<string>(loReader, "SASS_SDate");
                            loItem.STsponsercode.EDate = GetValue<string>(loReader, "SASS_EDate");
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
        /// Method to Get Student Entity
        /// </summary>
        /// <param name="argEn">Student Entity is an Input.MatricNo as Input Property.</param>
        /// <returns>Returns Student Entity</returns>
        public StudentEn GetItem(string MatricNo)
        {
            StudentEn loItem = new StudentEn();
            string sqlCmd = "Select * FROM SAS_Student WHERE SASI_MatricNo = " + clsGeneric.AddQuotes(MatricNo);
            try
            {
                if (!FormHelp.IsBlank(sqlCmd))
                {
                    //DbCommand cmd = _DatabaseFactory.GetDbCommand(Helper.GetDataBaseType, sqlCmd, DataBaseConnectionString);
                    //_DatabaseFactory.AddInParameter(ref cmd, "@SASI_MatricNo", DbType.String, MatricNo);
                    //_DbParameterCollection = cmd.Parameters;

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

        #region GetStudInfo

        /// <summary>
        /// Method to Get Student Entity
        /// </summary>
        /// <param name="argEn">Student Entity is an Input.MatricNo as Input Property.</param>
        /// <returns>Returns Student Entity</returns>
        public StudentEn GetStudInfo(string ICNO)
        {
            StudentEn loItem = new StudentEn();
            string sqlCmd = "Select * FROM SAS_Student WHERE SASI_ICNo = @SASI_ICNo";
            try
            {
                if (!FormHelp.IsBlank(sqlCmd))
                {
                    DbCommand cmd = _DatabaseFactory.GetDbCommand(Helper.GetDataBaseType, sqlCmd, DataBaseConnectionString);
                    _DatabaseFactory.AddInParameter(ref cmd, "@SASI_ICNo", DbType.String, ICNO);
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
                loItem.MatricNo = "null";
                return loItem;
            }
            return loItem;
        }

        #endregion

        #region Insert

        /// <summary>
        /// Method to Insert Student 
        /// </summary>
        /// <param name="argEn">Student Entity is an Input.</param>
        /// <returns>Returns Boolean</returns>
        public bool Insert(StudentEn argEn)
        {
            bool lbRes = false;
            int iOut = 0;
            string sqlCmd = "Select count(*) as cnt From SAS_Student where SASI_MatricNo = '" + argEn.MatricNo + "'";
            try
            {
                if (!FormHelp.IsBlank(sqlCmd))
                {
                    using (IDataReader dr = _DatabaseFactory.ExecuteReader(Helper.GetDataBaseType,
                        DataBaseConnectionString, sqlCmd).CreateDataReader())
                    {
                        if (dr.Read())
                            iOut = clsGeneric.NullToInteger(dr["cnt"]);
                        if (iOut > 0)
                            throw new Exception("Record Already Exist");
                        dr.Close();
                    }
                    if (iOut == 0)
                    {
                        sqlCmd = "INSERT INTO SAS_Student(SASI_MatricNo,SASI_Name,SASI_PgId,SASI_Faculty,SASI_ICNo,SASI_Passport,SASI_ID,SASI_Intake," +
                        "SASI_CurSem,SASI_CurSemYr,SASI_Studytype,SASS_Code,SASC_Code,SASI_Hostel,SAKO_Code,SASI_OtherID,SABK_Code,SASI_FloorNo," +
                        //"SART_Code,SASI_CrditHrs,SASI_GPA,SASI_CGPA,SASI_Add1,SASI_Add2,SASI_Add3,SASI_City,SASI_State,SASI_Country,SASI_Postcode," +
                        "SART_Code,SASI_CrditHrs,SASI_GPA,SASI_Add1,SASI_Add2,SASI_Add3,SASI_City,SASI_State,SASI_Country,SASI_Postcode," +
                        "SASI_Email,SASI_Tel,SASI_HP,SASI_Bank,SASI_AccNo,SASI_GLCode,SABR_Code,SASI_StatusRec,SASI_AFCStatus,SASI_UpdatedBy," +
                        "SASI_UpdatedDtTm,SASI_MAdd1,SASI_MAdd2,SASI_MAdd3,SASI_MCity,SASI_MState,SASI_MCountry,SASI_MPostcode,SASI_FeeCat,SASI_KokoCode,SASI_Reg_Status) " +
                        "VALUES (@SASI_MatricNo,@SASI_Name,@SASI_PgId,@SASI_Faculty,@SASI_ICNo,@SASI_Passport,@SASI_ID,@SASI_Intake,@SASI_CurSem," +
                        "@SASI_CurSemYr,@SASI_Studytype,@SASS_Code,@SASC_Code,@SASI_Hostel,@SAKO_Code,@SASI_OtherID,@SABK_Code,@SASI_FloorNo," +
                        //"@SART_Code,@SASI_CrditHrs,@SASI_GPA,@SASI_CGPA,@SASI_Add1,@SASI_Add2,@SASI_Add3,@SASI_City,@SASI_State,@SASI_Country," +
                        "@SART_Code,@SASI_CrditHrs,@SASI_GPA,@SASI_Add1,@SASI_Add2,@SASI_Add3,@SASI_City,@SASI_State,@SASI_Country," +
                        "@SASI_Postcode,@SASI_Email,@SASI_Tel,@SASI_HP,@SASI_Bank,@SASI_AccNo,@SASI_GLCode,@SABR_Code,@SASI_StatusRec,@SASI_AFCStatus," +
                        "@SASI_UpdatedBy,@SASI_UpdatedDtTm,@SASI_MAdd1,@SASI_MAdd2,@SASI_MAdd3,@SASI_MCity,@SASI_MState,@SASI_MCountry,@SASI_MPostcode," +
                        "@SASI_FeeCat,@SASI_KokoCode, @SASI_RegStatus,@sasi_intakehostel) ";
                        if (!FormHelp.IsBlank(sqlCmd))
                        {
                            DbCommand cmd = _DatabaseFactory.GetDbCommand(Helper.GetDataBaseType, sqlCmd, DataBaseConnectionString);
                            _DatabaseFactory.AddInParameter(ref cmd, "@SASI_MatricNo", DbType.String, argEn.MatricNo);
                            _DatabaseFactory.AddInParameter(ref cmd, "@SASI_Name", DbType.String, argEn.StudentName);
                            _DatabaseFactory.AddInParameter(ref cmd, "@SASI_PgId", DbType.String, argEn.ProgramID);
                            _DatabaseFactory.AddInParameter(ref cmd, "@SASI_Faculty", DbType.String, argEn.Faculty);
                            _DatabaseFactory.AddInParameter(ref cmd, "@SASI_ICNo", DbType.String, argEn.ICNo);
                            _DatabaseFactory.AddInParameter(ref cmd, "@SASI_Passport", DbType.String, argEn.Passport);
                            _DatabaseFactory.AddInParameter(ref cmd, "@SASI_ID", DbType.String, argEn.ID);
                            _DatabaseFactory.AddInParameter(ref cmd, "@SASI_Intake", DbType.String, argEn.Intake);
                            _DatabaseFactory.AddInParameter(ref cmd, "@SASI_CurSem", DbType.Int32, argEn.CurrentSemester);
                            _DatabaseFactory.AddInParameter(ref cmd, "@SASI_CurSemYr", DbType.String, argEn.CurretSemesterYear);
                            _DatabaseFactory.AddInParameter(ref cmd, "@SASI_Studytype", DbType.String, argEn.Studytype);
                            _DatabaseFactory.AddInParameter(ref cmd, "@SASS_Code", DbType.String, argEn.StudentCode);
                            _DatabaseFactory.AddInParameter(ref cmd, "@SASC_Code", DbType.String, argEn.CategoryCode);
                            _DatabaseFactory.AddInParameter(ref cmd, "@SASI_Hostel", DbType.Boolean, argEn.Hostel);
                            _DatabaseFactory.AddInParameter(ref cmd, "@SAKO_Code", DbType.String, argEn.SAKO_Code);
                            _DatabaseFactory.AddInParameter(ref cmd, "@SASI_OtherID", DbType.String, argEn.SASI_OtherID);
                            _DatabaseFactory.AddInParameter(ref cmd, "@SABK_Code", DbType.String, argEn.SABK_Code);
                            _DatabaseFactory.AddInParameter(ref cmd, "@SASI_FloorNo", DbType.String, argEn.SASI_FloorNo);
                            _DatabaseFactory.AddInParameter(ref cmd, "@SART_Code", DbType.String, argEn.SART_Code);
                            _DatabaseFactory.AddInParameter(ref cmd, "@SASI_CrditHrs", DbType.Double, argEn.SASI_CrditHrs);
                            _DatabaseFactory.AddInParameter(ref cmd, "@SASI_GPA", DbType.Double, argEn.SASI_GPA);
                            //_DatabaseFactory.AddInParameter(ref cmd, "@SASI_CGPA", DbType.Double, argEn.SASI_CGPA);
                            _DatabaseFactory.AddInParameter(ref cmd, "@SASI_Add1", DbType.String, argEn.SASI_Add1);
                            _DatabaseFactory.AddInParameter(ref cmd, "@SASI_Add2", DbType.String, argEn.SASI_Add2);
                            _DatabaseFactory.AddInParameter(ref cmd, "@SASI_Add3", DbType.String, argEn.SASI_Add3);
                            _DatabaseFactory.AddInParameter(ref cmd, "@SASI_City", DbType.String, argEn.SASI_City);
                            _DatabaseFactory.AddInParameter(ref cmd, "@SASI_State", DbType.String, argEn.SASI_State);
                            _DatabaseFactory.AddInParameter(ref cmd, "@SASI_Country", DbType.String, argEn.SASI_Country);
                            _DatabaseFactory.AddInParameter(ref cmd, "@SASI_Postcode", DbType.String, argEn.SASI_Postcode);
                            _DatabaseFactory.AddInParameter(ref cmd, "@SASI_Email", DbType.String, argEn.SASI_Email);
                            _DatabaseFactory.AddInParameter(ref cmd, "@SASI_Tel", DbType.String, argEn.SASI_Tel);
                            _DatabaseFactory.AddInParameter(ref cmd, "@SASI_HP", DbType.String, argEn.SASI_HP);
                            _DatabaseFactory.AddInParameter(ref cmd, "@SASI_Bank", DbType.String, argEn.SASI_Bank);
                            _DatabaseFactory.AddInParameter(ref cmd, "@SASI_AccNo", DbType.String, argEn.SASI_AccNo);
                            _DatabaseFactory.AddInParameter(ref cmd, "@SASI_GLCode", DbType.String, argEn.SASI_GLCode);
                            _DatabaseFactory.AddInParameter(ref cmd, "@SABR_Code", DbType.Int32, argEn.SABR_Code);
                            _DatabaseFactory.AddInParameter(ref cmd, "@SASI_StatusRec", DbType.Boolean, argEn.SASI_StatusRec);
                            _DatabaseFactory.AddInParameter(ref cmd, "@SASI_AFCStatus", DbType.Boolean, argEn.SASI_AFCStatus);
                            _DatabaseFactory.AddInParameter(ref cmd, "@SASI_UpdatedBy", DbType.String, argEn.SASI_UpdatedBy);
                            _DatabaseFactory.AddInParameter(ref cmd, "@SASI_UpdatedDtTm", DbType.String, argEn.SASI_UpdatedDtTm);
                            _DatabaseFactory.AddInParameter(ref cmd, "@SASI_MAdd1", DbType.String, argEn.SASI_MAdd1);
                            _DatabaseFactory.AddInParameter(ref cmd, "@SASI_MAdd2", DbType.String, argEn.SASI_MAdd2);
                            _DatabaseFactory.AddInParameter(ref cmd, "@SASI_MAdd3", DbType.String, argEn.SASI_MAdd3);
                            _DatabaseFactory.AddInParameter(ref cmd, "@SASI_MCity", DbType.String, argEn.SASI_MCity);
                            _DatabaseFactory.AddInParameter(ref cmd, "@SASI_MState", DbType.String, argEn.SASI_MState);
                            _DatabaseFactory.AddInParameter(ref cmd, "@SASI_MCountry", DbType.String, argEn.SASI_MCountry);
                            _DatabaseFactory.AddInParameter(ref cmd, "@SASI_MPostcode", DbType.String, argEn.SASI_MPostcode);
                            _DatabaseFactory.AddInParameter(ref cmd, "@SASI_FeeCat", DbType.String, argEn.FeeCat);
                            _DatabaseFactory.AddInParameter(ref cmd, "@SASI_KokoCode", DbType.String, argEn.KokoCode);
                            _DatabaseFactory.AddInParameter(ref cmd, "@SASI_RegStatus", DbType.String, argEn.RegistrationStatus);
                            _DatabaseFactory.AddInParameter(ref cmd, "@sasi_intakehostel", DbType.String, argEn.HostelIntake);

                            _DbParameterCollection = cmd.Parameters;

                            int liRowAffected = _DatabaseFactory.ExecuteNonQuery(Helper.GetDataBaseType, cmd,
                                DataBaseConnectionString, sqlCmd, _DbParameterCollection);

                            if (liRowAffected > -1)
                                lbRes = true;
                            else
                                throw new Exception("Insertion Failed! No Row has been updated...");

                            StudentSponDAL loStuSpon = new StudentSponDAL();
                            StudentSponEn loStuSponEn = new StudentSponEn();
                            StuSponFeeTypesDAL loStuSponFee = new StuSponFeeTypesDAL();
                            StuSponFeeTypesEn loStuSponFeeEn = new StuSponFeeTypesEn();

                            //Deleting Existing StudentSponsor and StuFeeTypes
                            loStuSponEn.MatricNo = argEn.MatricNo;
                            loStuSpon.Delete(loStuSponEn);
                            loStuSponFeeEn.MatricNo = argEn.MatricNo;
                            loStuSponFee.Delete(loStuSponFeeEn);
                            if (argEn.ListStuSponser != null)
                            {
                                for (int i = 0; i < argEn.ListStuSponser.Count; i++)
                                {
                                    //Insert Student Sponser
                                    loStuSpon.Insert(argEn.ListStuSponser[i]);
                                    //Insert Student Sponsor Fee Types                             
                                    for (int j = 0; j < argEn.ListStuSponser[i].ListStuSponFeeTypes.Count; j++)
                                    {
                                        loStuSponFee.Insert(argEn.ListStuSponser[i].ListStuSponFeeTypes[j]);
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

        #region UpdateSemester

        /// <summary>
        /// Method to Update Student Semester
        /// </summary>
        /// <param name="argEn">Student Entity is an Input.</param>
        /// <returns>Returns Boolean</returns>
        public bool UpdateSemester(string CurSem, string Program, String newSem)
        {
            bool lbRes = false;
            int rows = 0;

            string sqlCmd = "SELECT count(*) as rows FROM SAS_Student WHERE SASI_CurSemYr = '" + CurSem + "' ";

            try
            {
                if (!FormHelp.IsBlank(sqlCmd))
                {
                    using (IDataReader dr = _DatabaseFactory.ExecuteReader(Helper.GetDataBaseType,
                        DataBaseConnectionString, sqlCmd).CreateDataReader())
                    {
                        if (dr.Read())
                            rows = clsGeneric.NullToInteger(dr["rows"]);
                        dr.Close();
                        if (rows == 0)
                            throw new Exception("Record Does Not Exist");
                    }

                    if (rows > 0)
                    {
                        sqlCmd = "UPDATE SAS_Student SET SASI_poststatus='0', SASI_CurSemYr = '" + newSem + "' WHERE SASI_CurSemYr = '" + CurSem + "'";

                        if (Program.Length != 0 && Program != "-1") sqlCmd = sqlCmd + " AND SASI_pgid = '" + Program + "' ";

                        if (!FormHelp.IsBlank(sqlCmd))
                        {
                            int liRowAffected = _DatabaseFactory.ExecuteSqlStatement(Helper.
                                GetDataBaseType, DataBaseConnectionString, sqlCmd);

                            if (liRowAffected > 0)
                                lbRes = true;
                            else if (liRowAffected == 0)
                                throw new Exception("Record Does Not Exist");
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

        #region Update

        /// <summary>
        /// Method to Update Student 
        /// </summary>
        /// <param name="argEn">Student Entity is an Input.</param>
        /// <returns>Returns Boolean</returns>
        public bool Update(StudentEn argEn)
        {
            bool lbRes = false;
            int iOut = 0; int iOutTrack = 0;
            string OutProg = null;
            double OutCdtHr = 0.00;
            Boolean OutHostel;
            string strCategory = null;
            int intModule = 0;
            string sqlProg; string sqlCdtHr; string sqlHostel;
            string OutFaculty = null;
            string OutKolej = null;
            string OutBlock = null;
            string OutRoom = null;
            //added by farid on 08062016
            string OldSesi = null;
            string sqlHostel1;
            string strCategory1 = null;
            int intModule1 = 0;
            string newFaculty = null;
            string newKolej = null;
            string newBlock = null;
            string newRoom = null;

            string sqlCmd = "Select count(*) as cnt From SAS_Student where SASI_MatricNo = '" + argEn.MatricNo + "'";

            //if program/credit hours/hostel changes
            //changes by farid on 08062016
            string sqlChanges = "Select sasi_cursemyr as Sesi,SASI_PgId as Program, SASI_CrditHrs as CdtHour, SASI_Hostel as Hostel, sasi_faculty as Faculty, sako_code as Kolej, sabk_code as Block, sart_code as Room From SAS_Student where SASI_MatricNo = '" + argEn.MatricNo + "'";
            //if matric no exist
            string sqlCount = "Select count(*) as track From sas_trackingnotes where SASI_MatricNo = '" + argEn.MatricNo + "'";

            using (IDataReader drTrack = _DatabaseFactory.ExecuteReader(Helper.GetDataBaseType,
                        DataBaseConnectionString, sqlChanges).CreateDataReader())
            {
                if (drTrack.Read())
                    OutProg = clsGeneric.NullToString(drTrack["Program"]);

                OutFaculty = clsGeneric.NullToString(drTrack["Faculty"]);

                OutCdtHr = Convert.ToDouble(drTrack["CdtHour"]);
                if (FormHelp.IsBlank(OutCdtHr))
                {
                    OutCdtHr = 0.00;
                }
                OutHostel = clsGeneric.NullToBoolean(drTrack["Hostel"]);

                OutKolej = clsGeneric.NullToString(drTrack["Kolej"]);
                OutBlock = clsGeneric.NullToString(drTrack["Block"]);
                OutRoom = clsGeneric.NullToString(drTrack["Room"]);
                //added by farid on 08062016
                OldSesi = clsGeneric.NullToString(drTrack["Sesi"]);

                drTrack.Close();
                if (FormHelp.IsBlank(OutProg))
                    throw new Exception("Record Doesn't Exist!");
            }

            try
            {
                if (!FormHelp.IsBlank(sqlCmd))
                {
                    using (IDataReader dr = _DatabaseFactory.ExecuteReader(Helper.GetDataBaseType,
                        DataBaseConnectionString, sqlCmd).CreateDataReader())
                    {
                        if (dr.Read())
                            iOut = clsGeneric.NullToInteger(dr["cnt"]);
                        dr.Close();
                        if (iOut > 1)
                            throw new Exception("Record Already Exist!");
                    }

                    if (iOut == 1)
                    {
                        sqlCmd = "UPDATE SAS_Student SET SASI_MatricNo = @SASI_MatricNo, SASI_Name = @SASI_Name, SASI_PgId = @SASI_PgId, SASI_Faculty = @SASI_Faculty," +
                                 " SASI_ICNo = @SASI_ICNo, SASI_Passport = @SASI_Passport, SASI_ID = @SASI_ID, SASI_Intake = @SASI_Intake, SASI_CurSem = @SASI_CurSem," +
                                 " SASI_CurSemYr = @SASI_CurSemYr, SASI_Studytype = @SASI_Studytype, SASS_Code = @SASS_Code, SASC_Code = @SASC_Code, SASI_Hostel = @SASI_Hostel," +
                                 " SAKO_Code = @SAKO_Code, SASI_OtherID = @SASI_OtherID, SABK_Code = @SABK_Code, SASI_FloorNo = @SASI_FloorNo, SART_Code = @SART_Code," +
                                 " SASI_CrditHrs = @SASI_CrditHrs, SASI_GPA = @SASI_GPA, SASI_CGPA = @SASI_CGPA, SASI_Add1 = @SASI_Add1, SASI_Add2 = @SASI_Add2, SASI_Add3 = @SASI_Add3," +
                                 " SASI_City = @SASI_City, SASI_State = @SASI_State, SASI_Country = @SASI_Country, SASI_Postcode = @SASI_Postcode, SASI_Email = @SASI_Email," +
                                 " SASI_Tel = @SASI_Tel, SASI_HP = @SASI_HP, SASI_Bank = @SASI_Bank, SASI_AccNo = @SASI_AccNo, SASI_GLCode = @SASI_GLCode, SABR_Code = @SABR_Code," +
                                 " SASI_StatusRec = @SASI_StatusRec,SASI_AFCStatus = @SASI_AFCStatus, SASI_UpdatedBy = @SASI_UpdatedBy, SASI_UpdatedDtTm = @SASI_UpdatedDtTm, SASI_MAdd1 = @SASI_MAdd1," +
                                 " SASI_MAdd2 = @SASI_MAdd2, SASI_MAdd3 = @SASI_MAdd3, SASI_MCity = @SASI_MCity, SASI_MState = @SASI_MState, SASI_MCountry = @SASI_MCountry," +
                                 " SASI_MPostcode = @SASI_MPostcode, SASI_FeeCat=@SASI_FeeCat, SASI_KokoCode = @SASI_KokoCode, SASI_Reg_Status = @SASI_RegStatus  WHERE SASI_MatricNo = @SASI_MatricNo";

                        if (!FormHelp.IsBlank(sqlCmd))
                        {
                            DbCommand cmd = _DatabaseFactory.GetDbCommand(Helper.GetDataBaseType, sqlCmd, DataBaseConnectionString);
                            _DatabaseFactory.AddInParameter(ref cmd, "@SASI_MatricNo", DbType.String, argEn.MatricNo);
                            _DatabaseFactory.AddInParameter(ref cmd, "@SASI_Name", DbType.String, argEn.StudentName);
                            _DatabaseFactory.AddInParameter(ref cmd, "@SASI_PgId", DbType.String, argEn.ProgramID);
                            _DatabaseFactory.AddInParameter(ref cmd, "@SASI_Faculty", DbType.String, argEn.Faculty);
                            _DatabaseFactory.AddInParameter(ref cmd, "@SASI_ICNo", DbType.String, argEn.ICNo);
                            _DatabaseFactory.AddInParameter(ref cmd, "@SASI_Passport", DbType.String, argEn.Passport);
                            _DatabaseFactory.AddInParameter(ref cmd, "@SASI_ID", DbType.String, argEn.ID);
                            _DatabaseFactory.AddInParameter(ref cmd, "@SASI_Intake", DbType.String, argEn.Intake);
                            _DatabaseFactory.AddInParameter(ref cmd, "@SASI_CurSem", DbType.Int32, argEn.CurrentSemester);
                            _DatabaseFactory.AddInParameter(ref cmd, "@SASI_CurSemYr", DbType.String, argEn.CurretSemesterYear);
                            _DatabaseFactory.AddInParameter(ref cmd, "@SASI_Studytype", DbType.String, argEn.Studytype);
                            _DatabaseFactory.AddInParameter(ref cmd, "@SASS_Code", DbType.String, argEn.StudentCode);
                            _DatabaseFactory.AddInParameter(ref cmd, "@SASC_Code", DbType.String, argEn.CategoryCode);
                            _DatabaseFactory.AddInParameter(ref cmd, "@SASI_Hostel", DbType.Boolean, argEn.Hostel);
                            _DatabaseFactory.AddInParameter(ref cmd, "@SAKO_Code", DbType.String, argEn.SAKO_Code);
                            _DatabaseFactory.AddInParameter(ref cmd, "@SASI_OtherID", DbType.String, argEn.SASI_OtherID);
                            _DatabaseFactory.AddInParameter(ref cmd, "@SABK_Code", DbType.String, argEn.SABK_Code);
                            _DatabaseFactory.AddInParameter(ref cmd, "@SASI_FloorNo", DbType.String, argEn.SASI_FloorNo);
                            _DatabaseFactory.AddInParameter(ref cmd, "@SART_Code", DbType.String, argEn.SART_Code);
                            _DatabaseFactory.AddInParameter(ref cmd, "@SASI_CrditHrs", DbType.Double, argEn.SASI_CrditHrs);
                            _DatabaseFactory.AddInParameter(ref cmd, "@SASI_GPA", DbType.Double, argEn.SASI_GPA);
                            _DatabaseFactory.AddInParameter(ref cmd, "@SASI_CGPA", DbType.String, argEn.SASI_CGPA);
                            _DatabaseFactory.AddInParameter(ref cmd, "@SASI_Add1", DbType.String, argEn.SASI_Add1);
                            _DatabaseFactory.AddInParameter(ref cmd, "@SASI_Add2", DbType.String, argEn.SASI_Add2);
                            _DatabaseFactory.AddInParameter(ref cmd, "@SASI_Add3", DbType.String, argEn.SASI_Add3);
                            _DatabaseFactory.AddInParameter(ref cmd, "@SASI_City", DbType.String, argEn.SASI_City);
                            _DatabaseFactory.AddInParameter(ref cmd, "@SASI_State", DbType.String, argEn.SASI_State);
                            _DatabaseFactory.AddInParameter(ref cmd, "@SASI_Country", DbType.String, argEn.SASI_Country);
                            _DatabaseFactory.AddInParameter(ref cmd, "@SASI_Postcode", DbType.String, argEn.SASI_Postcode);
                            _DatabaseFactory.AddInParameter(ref cmd, "@SASI_Email", DbType.String, argEn.SASI_Email);
                            _DatabaseFactory.AddInParameter(ref cmd, "@SASI_Tel", DbType.String, argEn.SASI_Tel);
                            _DatabaseFactory.AddInParameter(ref cmd, "@SASI_HP", DbType.String, argEn.SASI_HP);
                            _DatabaseFactory.AddInParameter(ref cmd, "@SASI_Bank", DbType.String, argEn.SASI_Bank);
                            _DatabaseFactory.AddInParameter(ref cmd, "@SASI_AccNo", DbType.String, argEn.SASI_AccNo);
                            _DatabaseFactory.AddInParameter(ref cmd, "@SASI_GLCode", DbType.String, argEn.SASI_GLCode);
                            _DatabaseFactory.AddInParameter(ref cmd, "@SABR_Code", DbType.Int32, argEn.SABR_Code);
                            _DatabaseFactory.AddInParameter(ref cmd, "@SASI_StatusRec", DbType.Boolean, argEn.SASI_StatusRec);
                            _DatabaseFactory.AddInParameter(ref cmd, "@SASI_AFCStatus", DbType.Boolean, argEn.SASI_AFCStatus);
                            _DatabaseFactory.AddInParameter(ref cmd, "@SASI_UpdatedBy", DbType.String, argEn.SASI_UpdatedBy);
                            _DatabaseFactory.AddInParameter(ref cmd, "@SASI_UpdatedDtTm", DbType.String, argEn.SASI_UpdatedDtTm);
                            _DatabaseFactory.AddInParameter(ref cmd, "@SASI_MAdd1", DbType.String, argEn.SASI_MAdd1);
                            _DatabaseFactory.AddInParameter(ref cmd, "@SASI_MAdd2", DbType.String, argEn.SASI_MAdd2);
                            _DatabaseFactory.AddInParameter(ref cmd, "@SASI_MAdd3", DbType.String, argEn.SASI_MAdd3);
                            _DatabaseFactory.AddInParameter(ref cmd, "@SASI_MCity", DbType.String, argEn.SASI_MCity);
                            _DatabaseFactory.AddInParameter(ref cmd, "@SASI_MState", DbType.String, argEn.SASI_MState);
                            _DatabaseFactory.AddInParameter(ref cmd, "@SASI_MCountry", DbType.String, argEn.SASI_MCountry);
                            _DatabaseFactory.AddInParameter(ref cmd, "@SASI_MPostcode", DbType.String, argEn.SASI_MPostcode);
                            _DatabaseFactory.AddInParameter(ref cmd, "@SASI_FeeCat", DbType.String, argEn.FeeCat);
                            _DatabaseFactory.AddInParameter(ref cmd, "@SASI_KokoCode", DbType.String, argEn.KokoCode);
                            _DatabaseFactory.AddInParameter(ref cmd, "@SASI_RegStatus", DbType.String, argEn.RegistrationStatus);
                            _DatabaseFactory.AddInParameter(ref cmd, "@sasi_intakehostel", DbType.String, argEn.HostelIntake);
                            _DbParameterCollection = cmd.Parameters;

                            int liRowAffected = _DatabaseFactory.ExecuteNonQuery(Helper.GetDataBaseType, cmd,
                                DataBaseConnectionString, sqlCmd, _DbParameterCollection);

                            if (liRowAffected > -1)
                            {
                                lbRes = true;

                                using (IDataReader drTrack = _DatabaseFactory.ExecuteReader(Helper.GetDataBaseType,
                                    DataBaseConnectionString, sqlCount).CreateDataReader())
                                {
                                    if (drTrack.Read())
                                        iOutTrack = clsGeneric.NullToInteger(drTrack["track"]);
                                    drTrack.Close();
                                }

                                //if program changes
                                if (OutProg != argEn.ProgramID)
                                {
                                    strCategory = "Credit Note";
                                    intModule = 1;
                                    sqlProg = "DELETE FROM sas_trackingnotes where sasi_matricno=" + clsGeneric.AddQuotes(argEn.MatricNo) + " and track_module=" + intModule + ";";
                                    sqlProg += "INSERT INTO sas_trackingnotes(sasi_matricno,cur_sasi_pgid,old_sasi_pgid,flag,category,track_module,updateby,updatedttm) ";
                                    sqlProg += "VALUES ('" + argEn.MatricNo + "', '" + argEn.ProgramID + "', '" + OutProg + "', true," + clsGeneric.AddQuotes(strCategory);
                                    sqlProg += clsGeneric.AddComma() + intModule;
                                    sqlProg += clsGeneric.AddComma() + clsGeneric.AddQuotes(argEn.SASI_UpdatedBy) + ", '" + argEn.SASI_UpdatedDtTm + "');";

                                    if (!FormHelp.IsBlank(sqlProg))
                                    {
                                        _DatabaseFactory.ExecuteSqlStatement(Helper.GetDataBaseType, DataBaseConnectionString, sqlProg);
                                    }

                                }

                                ////if faculty changes
                                //if (OutFaculty != argEn.Faculty)
                                //{
                                //    strCategory = "Credit Note";
                                //    intModule = 1;

                                //    sqlProg = "INSERT INTO sas_trackingnotes(sasi_matricno,cur_sasi_pgid,old_sasi_pgid,flag,category,track_module,updateby,updatedttm) ";
                                //    sqlProg += "VALUES ('" + argEn.MatricNo + "', '" + argEn.ProgramID + "', '" + OutProg + "', true," + clsGeneric.AddQuotes(strCategory);
                                //    sqlProg += clsGeneric.AddComma() + intModule;
                                //    sqlProg += clsGeneric.AddComma() + clsGeneric.AddQuotes(argEn.SASI_UpdatedBy) + ", '" + argEn.SASI_UpdatedDtTm + "');";

                                //    if (!FormHelp.IsBlank(sqlProg))
                                //    {
                                //        _DatabaseFactory.ExecuteSqlStatement(Helper.GetDataBaseType, DataBaseConnectionString, sqlProg);
                                //    }

                                //}

                                //if credit hours changes
                                if (!FormHelp.IsBlank(OutCdtHr) && OutCdtHr != argEn.SASI_CrditHrs)
                                {
                                    double diff = OutCdtHr - argEn.SASI_CrditHrs;

                                    if (argEn.SASI_CrditHrs > OutCdtHr)
                                    {
                                        strCategory = "Debit Note";
                                    }

                                    if (argEn.SASI_CrditHrs < OutCdtHr)
                                    {
                                        strCategory = "Credit Note";
                                    }

                                    intModule = 2;
                                    sqlCdtHr = "DELETE FROM sas_trackingnotes where sasi_matricno=" + clsGeneric.AddQuotes(argEn.MatricNo) + " and track_module=" + intModule + ";";
                                    sqlCdtHr += "INSERT INTO sas_trackingnotes(sasi_matricno,cur_sasi_crdithrs,old_sasi_crdithrs,difference,category,flag,track_module,updateby,updatedttm) ";
                                    sqlCdtHr += "VALUES ('" + argEn.MatricNo + "', " + argEn.SASI_CrditHrs + ", " + OutCdtHr + clsGeneric.AddComma() + diff;
                                    sqlCdtHr += clsGeneric.AddComma() + clsGeneric.AddQuotes(strCategory) + clsGeneric.AddComma();
                                    sqlCdtHr += " true, " + intModule + clsGeneric.AddComma() + clsGeneric.AddQuotes(argEn.SASI_UpdatedBy) + ", '" + argEn.SASI_UpdatedDtTm + "');";

                                    if (!FormHelp.IsBlank(sqlCdtHr) && diff != 0)
                                    {
                                        _DatabaseFactory.ExecuteSqlStatement(Helper.GetDataBaseType, DataBaseConnectionString, sqlCdtHr);
                                    }

                                }
                                //added by farid on 08062016
                                if (OldSesi == argEn.CurretSemesterYear)
                                {


                                    //if hostel changes
                                    if (OutHostel == true && argEn.Hostel == false)
                                    {
                                        strCategory = "Credit Note";
                                        intModule = 3;

                                        sqlHostel = "DELETE FROM sas_trackingnotes where sasi_matricno=" + clsGeneric.AddQuotes(argEn.MatricNo) + " and track_module=" + intModule + ";";
                                        sqlHostel += "INSERT INTO sas_trackingnotes(sasi_matricno,cur_sasi_hostel,old_sasi_hostel,flag,category,track_module,updateby,updatedttm, sako_code, sabk_code, sart_code) ";
                                        sqlHostel += "VALUES ('" + argEn.MatricNo + "', " + argEn.Hostel + ", " + OutHostel + ", true, " + clsGeneric.AddQuotes(strCategory);
                                        sqlHostel += clsGeneric.AddComma() + intModule + clsGeneric.AddComma() + clsGeneric.AddQuotes(argEn.SASI_UpdatedBy) + clsGeneric.AddComma() + clsGeneric.AddQuotes(argEn.SASI_UpdatedDtTm) +
                                                    clsGeneric.AddComma() + clsGeneric.AddQuotes(OutKolej) + clsGeneric.AddComma() + clsGeneric.AddQuotes(OutBlock) + clsGeneric.AddComma() + clsGeneric.AddQuotes(OutRoom) + ");";

                                        if (!FormHelp.IsBlank(sqlHostel))
                                        {
                                            _DatabaseFactory.ExecuteSqlStatement(Helper.GetDataBaseType, DataBaseConnectionString, sqlHostel);
                                        }
                                    }
                                    //added by farid 07062016
                                    if (OutHostel == false && argEn.Hostel == true)
                                    {
                                        strCategory = "Debit Note";
                                        intModule = 3;
                                        OutKolej = argEn.SAKO_Code;
                                        OutBlock = argEn.SABK_Code;
                                        OutRoom = argEn.SART_Code;
                                        sqlHostel = "DELETE FROM sas_trackingnotes where sasi_matricno=" + clsGeneric.AddQuotes(argEn.MatricNo) + " and track_module=" + intModule + ";";
                                        sqlHostel += "INSERT INTO sas_trackingnotes(sasi_matricno,cur_sasi_hostel,old_sasi_hostel,flag,category,track_module,updateby,updatedttm, sako_code, sabk_code, sart_code) ";
                                        sqlHostel += "VALUES ('" + argEn.MatricNo + "', " + argEn.Hostel + ", " + OutHostel + ", true, " + clsGeneric.AddQuotes(strCategory);
                                        sqlHostel += clsGeneric.AddComma() + intModule + clsGeneric.AddComma() + clsGeneric.AddQuotes(argEn.SASI_UpdatedBy) + clsGeneric.AddComma() + clsGeneric.AddQuotes(argEn.SASI_UpdatedDtTm) +
                                                    clsGeneric.AddComma() + clsGeneric.AddQuotes(OutKolej) + clsGeneric.AddComma() + clsGeneric.AddQuotes(OutBlock) + clsGeneric.AddComma() + clsGeneric.AddQuotes(OutRoom) + ");";

                                        if (!FormHelp.IsBlank(sqlHostel))
                                        {
                                            _DatabaseFactory.ExecuteSqlStatement(Helper.GetDataBaseType, DataBaseConnectionString, sqlHostel);
                                        }
                                    }
                                    if (OutHostel == true && argEn.Hostel == true)
                                    {

                                        //if (OutBlock == argEn.SABK_Code)
                                        //{
                                        strCategory = "Credit Note";
                                        intModule = 3;
                                        //OutKolej = argEn.SAKO_Code;
                                        //OutBlock = argEn.SABK_Code;
                                        //OutRoom = argEn.SART_Code;
                                        sqlHostel = "DELETE FROM sas_trackingnotes where sasi_matricno=" + clsGeneric.AddQuotes(argEn.MatricNo) + " and category=" + clsGeneric.AddQuotes(strCategory) + " and track_module=" + intModule + ";";
                                        sqlHostel += "INSERT INTO sas_trackingnotes(sasi_matricno,cur_sasi_hostel,old_sasi_hostel,flag,category,track_module,updateby,updatedttm, sako_code, sabk_code, sart_code) ";
                                        sqlHostel += "VALUES ('" + argEn.MatricNo + "', " + argEn.Hostel + ", " + OutHostel + ", true, " + clsGeneric.AddQuotes(strCategory);
                                        sqlHostel += clsGeneric.AddComma() + intModule + clsGeneric.AddComma() + clsGeneric.AddQuotes(argEn.SASI_UpdatedBy) + clsGeneric.AddComma() + clsGeneric.AddQuotes(argEn.SASI_UpdatedDtTm) +
                                                    clsGeneric.AddComma() + clsGeneric.AddQuotes(OutKolej) + clsGeneric.AddComma() + clsGeneric.AddQuotes(OutBlock) + clsGeneric.AddComma() + clsGeneric.AddQuotes(OutRoom) + ");";
                                        //}
                                        //}
                                        strCategory1 = "Debit Note";
                                        intModule1 = 3;
                                        newKolej = argEn.SAKO_Code;
                                        newBlock = argEn.SABK_Code;
                                        newRoom = argEn.SART_Code;
                                        sqlHostel1 = "DELETE FROM sas_trackingnotes where sasi_matricno=" + clsGeneric.AddQuotes(argEn.MatricNo) + " and category=" + clsGeneric.AddQuotes(strCategory1) + " and track_module=" + intModule1 + ";";
                                        sqlHostel1 += "INSERT INTO sas_trackingnotes(sasi_matricno,cur_sasi_hostel,old_sasi_hostel,flag,category,track_module,updateby,updatedttm, sako_code, sabk_code, sart_code) ";
                                        sqlHostel1 += "VALUES ('" + argEn.MatricNo + "', " + argEn.Hostel + ", " + OutHostel + ", true, " + clsGeneric.AddQuotes(strCategory1);
                                        sqlHostel1 += clsGeneric.AddComma() + intModule1 + clsGeneric.AddComma() + clsGeneric.AddQuotes(argEn.SASI_UpdatedBy) + clsGeneric.AddComma() + clsGeneric.AddQuotes(argEn.SASI_UpdatedDtTm) +
                                                    clsGeneric.AddComma() + clsGeneric.AddQuotes(newKolej) + clsGeneric.AddComma() + clsGeneric.AddQuotes(newBlock) + clsGeneric.AddComma() + clsGeneric.AddQuotes(newRoom) + ");";

                                        if (!FormHelp.IsBlank(sqlHostel))
                                        {
                                            _DatabaseFactory.ExecuteSqlStatement(Helper.GetDataBaseType, DataBaseConnectionString, sqlHostel);
                                        }
                                        if (!FormHelp.IsBlank(sqlHostel1))
                                        {
                                            _DatabaseFactory.ExecuteSqlStatement(Helper.GetDataBaseType, DataBaseConnectionString, sqlHostel1);
                                        }

                                        //if (OutKolej != argEn.SAKO_Code)
                                        //{
                                        //    strCategory = "Debit Note";
                                        //    intModule = 3;
                                        //    OutKolej = argEn.SAKO_Code;
                                        //    OutBlock = argEn.SABK_Code;
                                        //    OutRoom = argEn.SART_Code;
                                        //    sqlHostel = "DELETE FROM sas_trackingnotes where sasi_matricno=" + clsGeneric.AddQuotes(argEn.MatricNo) + " and category=" + clsGeneric.AddQuotes(strCategory) + " and track_module=" + intModule + ";";
                                        //    sqlHostel += "INSERT INTO sas_trackingnotes(sasi_matricno,cur_sasi_hostel,old_sasi_hostel,flag,category,track_module,updateby,updatedttm, sako_code, sabk_code, sart_code) ";
                                        //    sqlHostel += "VALUES ('" + argEn.MatricNo + "', " + argEn.Hostel + ", " + OutHostel + ", true, " + clsGeneric.AddQuotes(strCategory);
                                        //    sqlHostel += clsGeneric.AddComma() + intModule + clsGeneric.AddComma() + clsGeneric.AddQuotes(argEn.SASI_UpdatedBy) + clsGeneric.AddComma() + clsGeneric.AddQuotes(argEn.SASI_UpdatedDtTm) +
                                        //                clsGeneric.AddComma() + clsGeneric.AddQuotes(OutKolej) + clsGeneric.AddComma() + clsGeneric.AddQuotes(OutBlock) + clsGeneric.AddComma() + clsGeneric.AddQuotes(OutRoom) + ");";

                                        //    if (!FormHelp.IsBlank(sqlHostel))
                                        //    {
                                        //        _DatabaseFactory.ExecuteSqlStatement(Helper.GetDataBaseType, DataBaseConnectionString, sqlHostel);
                                        //    }
                                        //}
                                    }
                                }
                                // end added

                            }
                            else
                                throw new Exception("Update Failed! No Row has been updated...");

                            StudentSponDAL loStuSpon = new StudentSponDAL();
                            StudentSponEn loStuSponEn = new StudentSponEn();
                            StuSponFeeTypesDAL loStuSponFee = new StuSponFeeTypesDAL();
                            StuSponFeeTypesEn loStuSponFeeEn = new StuSponFeeTypesEn();

                            //Insert Student Sponser
                            loStuSponEn.MatricNo = argEn.MatricNo;
                            loStuSpon.Delete(loStuSponEn);
                            loStuSponFeeEn.MatricNo = argEn.MatricNo;
                            loStuSponFee.Delete(loStuSponFeeEn);
                            if (argEn.ListStuSponser != null)
                            {
                                for (int i = 0; i < argEn.ListStuSponser.Count; i++)
                                {
                                    //Insert Student Sponser
                                    loStuSpon.Insert(argEn.ListStuSponser[i]);
                                    //Insert Student Sponsor Fee Types                             
                                    for (int j = 0; j < argEn.ListStuSponser[i].ListStuSponFeeTypes.Count; j++)
                                    {
                                        loStuSponFee.Insert(argEn.ListStuSponser[i].ListStuSponFeeTypes[j]);
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

        #region Delete

        /// <summary>
        /// Method to Delete Student 
        /// </summary>
        /// <param name="argEn">Student Entity is an Input.MatricNo as Input Property.</param>
        /// <returns>Returns Boolean</returns>
        public bool Delete(StudentEn argEn)
        {
            bool lbRes = false;
            int rows = 0;

            string sqlCmd = "select count(*) as rows  from SAS_Accounts WHERE CreditRef = @CreditRef";
            try
            {      // Checking for usage in Transactions
                if (!FormHelp.IsBlank(sqlCmd))
                {
                    DbCommand cmdSel = _DatabaseFactory.GetDbCommand(Helper.GetDataBaseType, sqlCmd, DataBaseConnectionString);
                    _DatabaseFactory.AddInParameter(ref cmdSel, "@CreditRef", DbType.String, argEn.MatricNo);
                    _DbParameterCollection = cmdSel.Parameters;

                    using (IDataReader dr = _DatabaseFactory.GetIDataReader(Helper.GetDataBaseType, cmdSel,
                       DataBaseConnectionString, sqlCmd, _DbParameterCollection).CreateDataReader())
                    {
                        if (dr.Read())
                            rows = clsGeneric.NullToInteger(dr["rows"]);
                        if (rows > 0)
                            throw new Exception("Record Already in Use");
                    }
                    if (rows == 0)
                    {

                        sqlCmd = "DELETE FROM SAS_Student WHERE SASI_MatricNo = @SASI_MatricNo";
                        if (!FormHelp.IsBlank(sqlCmd))
                        {
                            DbCommand cmd = _DatabaseFactory.GetDbCommand(Helper.GetDataBaseType, sqlCmd, DataBaseConnectionString);
                            _DatabaseFactory.AddInParameter(ref cmd, "@SASI_MatricNo", DbType.String, argEn.MatricNo);
                            _DbParameterCollection = cmd.Parameters;

                            int liRowAffected = _DatabaseFactory.ExecuteNonQuery(Helper.GetDataBaseType, cmd,
                                DataBaseConnectionString, sqlCmd, _DbParameterCollection);

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

        #region LoadObject

        /// <summary>
        /// Method to Load Student Entity 
        /// </summary>
        /// <param name="argReader">IDataReader Object is an Input.</param>
        /// <returns>Returns Student Entity</returns>
        public StudentEn LoadObject(IDataReader argReader)
        {
            StudentEn loItem = new StudentEn();
            loItem.MatricNo = GetValue<string>(argReader, "SASI_MatricNo");
            loItem.StudentName = GetValue<string>(argReader, "SASI_Name");
            loItem.ProgramID = GetValue<string>(argReader, "SASI_PgId");
            loItem.Faculty = GetValue<string>(argReader, "SASI_Faculty");
            loItem.ICNo = GetValue<string>(argReader, "SASI_ICNo");
            loItem.Passport = GetValue<string>(argReader, "SASI_Passport");
            loItem.ID = GetValue<string>(argReader, "SASI_ID");
            loItem.Intake = GetValue<string>(argReader, "SASI_Intake");
            loItem.CurrentSemester = GetValue<int>(argReader, "SASI_CurSem");
            loItem.CurretSemesterYear = GetValue<string>(argReader, "SASI_CurSemYr");
            loItem.Studytype = GetValue<string>(argReader, "SASI_Studytype");
            loItem.StudentCode = GetValue<string>(argReader, "SASS_Code");
            loItem.CategoryCode = GetValue<string>(argReader, "SASC_Code");
            loItem.Hostel = GetValue<bool>(argReader, "SASI_Hostel");
            loItem.SAKO_Code = GetValue<string>(argReader, "SAKO_Code");
            loItem.SASI_OtherID = GetValue<string>(argReader, "SASI_OtherID");
            loItem.SABK_Code = GetValue<string>(argReader, "SABK_Code");
            loItem.SASI_FloorNo = GetValue<string>(argReader, "SASI_FloorNo");
            loItem.SART_Code = GetValue<string>(argReader, "SART_Code");
            loItem.SASI_CrditHrs = GetValue<double>(argReader, "SASI_CrditHrs");
            loItem.SASI_GPA = GetValue<double>(argReader, "SASI_GPA");
            //loItem.SASI_CGPA = GetValue<double>(argReader, "SASI_CGPA");
            loItem.SASI_Add1 = GetValue<string>(argReader, "SASI_Add1");
            loItem.SASI_Add2 = GetValue<string>(argReader, "SASI_Add2");
            loItem.SASI_Add3 = GetValue<string>(argReader, "SASI_Add3");
            loItem.SASI_City = GetValue<string>(argReader, "SASI_City");
            loItem.SASI_State = GetValue<string>(argReader, "SASI_State");
            loItem.SASI_Country = GetValue<string>(argReader, "SASI_Country");
            loItem.SASI_Postcode = GetValue<string>(argReader, "SASI_Postcode");
            loItem.SASI_Email = GetValue<string>(argReader, "SASI_Email");
            loItem.SASI_Tel = GetValue<string>(argReader, "SASI_Tel");
            loItem.SASI_HP = GetValue<string>(argReader, "SASI_HP");
            loItem.SASI_Bank = GetValue<string>(argReader, "SASI_Bank");
            loItem.SASI_AccNo = GetValue<string>(argReader, "SASI_AccNo");
            loItem.SASI_GLCode = GetValue<string>(argReader, "SASI_GLCode");
            loItem.SABR_Code = GetValue<int>(argReader, "SABR_Code");
            loItem.SASI_StatusRec = GetValue<bool>(argReader, "SASI_StatusRec");
            loItem.SASI_AFCStatus = GetValue<bool>(argReader, "SASI_AFCStatus");
            loItem.SASI_UpdatedBy = GetValue<string>(argReader, "SASI_UpdatedBy");
            loItem.SASI_UpdatedDtTm = GetValue<string>(argReader, "SASI_UpdatedDtTm");
            loItem.SASI_MAdd1 = GetValue<string>(argReader, "SASI_MAdd1");
            loItem.SASI_MAdd2 = GetValue<string>(argReader, "SASI_MAdd2");
            loItem.SASI_MAdd3 = GetValue<string>(argReader, "SASI_MAdd3");
            loItem.SASI_MCity = GetValue<string>(argReader, "SASI_MCity");
            loItem.SASI_MState = GetValue<string>(argReader, "SASI_MState");
            loItem.SASI_MCountry = GetValue<string>(argReader, "SASI_MCountry");
            loItem.SASI_MPostcode = GetValue<string>(argReader, "SASI_MPostcode");
            loItem.FeeCat = GetValue<string>(argReader, "SASI_FeeCat");
            loItem.KokoCode = GetValue<string>(argReader, "SASI_KokoCode");
            loItem.RegistrationStatus = GetValue<int>(argReader, "SASI_Reg_Status");
            loItem.HostelIntake = GetValue<string>(argReader, "sasi_intakehostel");
            return loItem;
        }
        /// <summary>
        /// Method to Load Student Entity 
        /// </summary>
        /// <param name="argReader">IDataReader Object is an Input.</param>
        /// <returns>Returns Student Entity</returns>
        public StudentEn LoadObjectForSem(IDataReader argReader)
        {
            StudentEn loItem = new StudentEn();
            loItem.MatricNo = GetValue<string>(argReader, "SASI_MatricNo");
            loItem.StudentName = GetValue<string>(argReader, "SASI_Name");
            //loItem.Intake = GetValue<string>(argReader, "SASI_Intake");
            loItem.ProgramID = GetValue<string>(argReader, "SASI_PgId");
            //loItem.CurretSemesterYear = GetValue<string>(argReader, "SASI_CurSemYr");
            loItem.CurrentSemester = GetValue<int>(argReader, "SASI_CurSem");
            loItem.SponsorLimit = GetValue<double>(argReader, "sass_limit");

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

        #region InsertStudentOutstanding

        /// <summary>
        /// Method to Insert Student Outstanding 
        /// </summary>
        /// <param name="argEn">Student Entity is an Input.</param>
        /// <returns>Returns Boolean</returns>
        /// modified by Hafiz @ 27/5/2016
        public bool InsertStudentOutstanding(StudentEn argEn)
        {
            bool lbRes = false;
            int iOut = 0;
            string sqlCmd = "Select count(*) as cnt From SAS_StudentOutstanding where SASI_MatricNo = '" + argEn.MatricNo + "'";
            try
            {
                if (!FormHelp.IsBlank(sqlCmd))
                {
                    using (IDataReader dr = _DatabaseFactory.ExecuteReader(Helper.GetDataBaseType,
                        DataBaseConnectionString, sqlCmd).CreateDataReader())
                    {
                        if (dr.Read())
                            iOut = clsGeneric.NullToInteger(dr["cnt"]);
                        dr.Close();
                        if (iOut > 0)
                            throw new Exception("Record Already Exist");
                    }
                    if (iOut == 0)
                    {
                        sqlCmd = "INSERT INTO SAS_StudentOutstanding(SASI_MatricNo,SASI_Name,SASI_PgId,SASI_CurSem,SASO_Outstandingamt) values(@SASI_MatricNo,@SASI_Name,@SASI_PgId,@SASI_CurSem,@SASO_Outstandingamt) ";
                        if (!FormHelp.IsBlank(sqlCmd))
                        {
                            DbCommand cmd = _DatabaseFactory.GetDbCommand(Helper.GetDataBaseType, sqlCmd, DataBaseConnectionString);
                            _DatabaseFactory.AddInParameter(ref cmd, "@SASI_MatricNo", DbType.String, argEn.MatricNo);
                            _DatabaseFactory.AddInParameter(ref cmd, "@SASI_Name", DbType.String, argEn.StudentName);
                            _DatabaseFactory.AddInParameter(ref cmd, "@SASI_PgId", DbType.String, argEn.ProgramID);
                            _DatabaseFactory.AddInParameter(ref cmd, "@SASI_CurSem", DbType.Int32, argEn.CurrentSemester);
                            _DatabaseFactory.AddInParameter(ref cmd, "@SASO_Outstandingamt", DbType.Double, argEn.OutstandingAmount);
                            _DbParameterCollection = cmd.Parameters;

                            int liRowAffected = _DatabaseFactory.ExecuteNonQuery(Helper.GetDataBaseType, cmd,
                                DataBaseConnectionString, sqlCmd, _DbParameterCollection);

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

        #region UpdateStudentOutstandingStatusEmpty

        /// <summary>
        /// 
        /// </summary>
        /// <param name="argEn"></param>
        /// <returns></returns>
        /// Modified by Hafiz Roslan @ 08/01/2016
        /// Modified by Hafiz Roslan @ 16/2/2106
        /// modified by Hafiz @ 27/5/2016

        public bool UpdateStudentOutstandingStatusEmpty(StudentEn argEn)
        {
            bool lbRes = false;
            string sqlCmd = string.Empty, sqlCmd_extend;

            List<StudentEn> loEnList1 = new List<StudentEn>();

            sqlCmd = " SELECT * FROM SAS_StudentOutstanding WHERE SASO_Outstandingamt IS NOT NULL";

            try
            {
                if (!FormHelp.IsBlank(sqlCmd))
                {
                    using (IDataReader loReader = _DatabaseFactory.ExecuteReader(Helper.GetDataBaseType,
                        DataBaseConnectionString, sqlCmd).CreateDataReader())
                    {
                        while (loReader.Read())
                        {
                            argEn.MatricNo = GetValue<string>(loReader, "SASI_MatricNo");
                            argEn.OutstandingAmount = GetValue<double>(loReader, "SASO_Outstandingamt");
                            argEn.IsReleased = GetValue<int>(loReader, "SASO_IsReleased");
                            argEn.ProgramID = GetValue<string>(loReader, "SASI_PgId");
                            argEn.CurretSemesterYear = GetValue<string>(loReader, "SASI_CurSemYr");

                            if (argEn.ProgramID.Length == 0 && argEn.CurretSemesterYear.Length == 0)
                            {
                                sqlCmd_extend = " ";

                                UpdateIsReleased(argEn.MatricNo, argEn.OutstandingAmount, argEn.IsReleased, sqlCmd_extend);
                            }
                            else if (argEn.ProgramID.Length != 0 && argEn.CurretSemesterYear.Length != 0)
                            {
                                sqlCmd_extend = " AND SASI_PgId = '" + argEn.ProgramID + "'" + " AND SASI_CurSemYr ";
                                sqlCmd_extend += " = '" + argEn.CurretSemesterYear + "'";

                                UpdateIsReleased(argEn.MatricNo, argEn.OutstandingAmount, argEn.IsReleased, sqlCmd_extend);
                            }
                            else
                            {
                                sqlCmd_extend = " AND SASI_PgId = '" + argEn.ProgramID + "'";

                                UpdateIsReleased(argEn.MatricNo, argEn.OutstandingAmount, argEn.IsReleased, sqlCmd_extend);
                            }

                            //add to list
                            loEnList1.Add(argEn);
                            lbRes = true;
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

        #endregion

        #region UpdateStudentOutstandingStatus

        /// <summary>
        /// 
        /// </summary>
        /// <param name="argEn"></param>
        /// <returns></returns>
        /// //modified by Hafiz Roslan @ 8/1/2016
        //change flag/SASO_IsReleased to 2 for exempted
        //Modified by Hafiz Roslan @ 16/2/2106
        public bool UpdateStudentOutstandingStatus(StudentEn argEn)
        {
            bool lbRes = false;

            string sqlCmd = string.Empty;
            //sqlCmd = "UPDATE SAS_StudentOutstanding SET SASO_IsReleased=1 WHERE SASI_MatricNo=@SASI_MatricNo ; ";
            sqlCmd = "UPDATE SAS_StudentOutstanding SET SASO_IsReleased=2 WHERE SASI_MatricNo=@SASI_MatricNo ; ";

            if (!FormHelp.IsBlank(sqlCmd))
            {
                DbCommand cmd = _DatabaseFactory.GetDbCommand(Helper.GetDataBaseType, sqlCmd, DataBaseConnectionString);
                _DatabaseFactory.AddInParameter(ref cmd, "@SASI_MatricNo", DbType.String, argEn.MatricNo);
                _DbParameterCollection = cmd.Parameters;

                int liRowAffected = _DatabaseFactory.ExecuteNonQuery(Helper.GetDataBaseType, cmd,
                                DataBaseConnectionString, sqlCmd, _DbParameterCollection);

                if (liRowAffected > -1)
                    lbRes = true;
                else
                    throw new Exception("Update Failed! No Row has been updated...");
            }

            return lbRes;

        }

        #endregion

        #region FetchLedgerDetails

        public StudentEn FetchLedgerDetails(string MatricNo)
        {
            StudentEn objStudent = new StudentEn();
            string sqlCmd = @"SELECT ST.SASI_Hostel, ST.SASI_CurSem, ST.SASI_CurSemYr,SS.SASS_Description,SP.SAPG_ProgramBM,SK.SAKO_Description,SB.SABK_Description,ST.SASI_FloorNo,RT.SART_Description
                            FROM SAS_Student ST LEFT JOIN SAS_StudentStatus SS ON ST.SASS_Code=SS.SASS_Code
                            LEFT JOIN SAS_Program SP ON ST.SASI_PgId=SP.SAPG_Code 
                            LEFT JOIN SAS_Kolej SK ON ST.SAKO_Code=SK.SAKO_Code 
                            LEFT JOIN SAS_Block SB ON ST.SABK_Code=SB.SABK_Code
                            LEFT JOIN SAS_RoomType RT ON SB.SABK_Code=RT.SABK_Code AND SK.SAKO_Code=RT.SART_Code
                            INNER JOIN SAS_ACCOUNTS SA ON st.SASI_MatricNo = SA.Creditref
                            WHERE ST.SASI_MatricNo='" + MatricNo + "' and SA.Poststatus= 'Posted'";
            try
            {
                IDataReader loReader = _DatabaseFactory.ExecuteReader(Helper.GetDataBaseType,
                        DataBaseConnectionString, sqlCmd).CreateDataReader();

                //if (lsError.Length > 0)
                //    throw new Exception(lsError);
                if (loReader.Read())
                {
                    //added by Mona 30/3/2016
                    if (FormHelp.IsBlank(GetValue<int>(loReader, "SASI_CurSem")))
                    {
                        objStudent.CurrentSemester = 0;
                    }
                    else
                    {                        
                        objStudent.CurrentSemester = GetValue<int>(loReader, "SASI_CurSem");
                    }

                    //Checking NULL value for CurretSemesterYear
                    if (String.IsNullOrEmpty(loReader["SASI_CurSemYr"].ToString()))
                    {
                        objStudent.CurretSemesterYear = "";
                    }
                    else
                    {
                        objStudent.CurretSemesterYear = loReader["SASI_CurSemYr"].ToString();
                    }

                    //Checking NULL value for StatusBayaran
                    if (String.IsNullOrEmpty(loReader["SASS_Description"].ToString()))
                    {
                        objStudent.StatusBayaran = "";
                    }
                    else
                    {
                        objStudent.StatusBayaran = loReader["SASS_Description"].ToString();
                    }

                    //Checking NULL value for SourceType
                    if (String.IsNullOrEmpty(loReader["SAPG_ProgramBM"].ToString()))
                    {
                        objStudent.SourceType = "";
                    }
                    else
                    {
                        objStudent.SourceType = loReader["SAPG_ProgramBM"].ToString();
                    }

                    //Checking NULL value for SABK_Code
                    if (String.IsNullOrEmpty(loReader["SABK_Description"].ToString()))
                    {
                        objStudent.SABK_Code = "";
                    }
                    else
                    {
                        objStudent.SABK_Code = loReader["SABK_Description"].ToString();
                    }

                    //Checking NULL value for SAKO_Code
                    if (String.IsNullOrEmpty(loReader["SAKO_Description"].ToString()))
                    {
                        objStudent.SAKO_Code = "";
                    }
                    else
                    {
                        objStudent.SAKO_Code = loReader["SAKO_Description"].ToString();
                    }

                    //Checking NULL value for SASI_FloorNo
                    if (String.IsNullOrEmpty(loReader["SASI_FloorNo"].ToString()))
                    {
                        objStudent.SASI_FloorNo = "";
                    }
                    else
                    {
                        objStudent.SASI_FloorNo = loReader["SASI_FloorNo"].ToString();
                    }

                    //Checking NULL value for SART_Code
                    if (String.IsNullOrEmpty(loReader["SART_Description"].ToString()))
                    {
                        objStudent.SART_Code = "";
                    }
                    else
                    {
                        objStudent.SART_Code = loReader["SART_Description"].ToString();
                    }

                    //Checking NULL value for Hostel
                    if (String.IsNullOrEmpty(loReader["SASI_Hostel"].ToString()))
                    {
                        //objStudent.Hostel = Convert.ToBoolean(loReader["SASI_Hostel"]);
                        objStudent.Hostel = false;
                    }
                    else
                    {
                        //objStudent.Hostel = Convert.ToBoolean(loReader["SASI_Hostel"]);
                        objStudent.Hostel = true;
                    }
                }
                loReader.Close();
            }
            catch (Exception err)
            {
                throw err;
            }
            return objStudent;
        }

        #endregion

        #region GetListStudent Change Prog/Credit Hour/Hostel

        public List<StudentEn> GetListStudentChange(string Category, int TrackModule)
        {
            string sqlCmd;
            List<StudentEn> loEnList = new List<StudentEn>();

            sqlCmd = "select ST.*,SS.SASI_Name, SS.sasi_pgid, SS.sasi_cursem from sas_trackingnotes ST INNER JOIN SAS_student SS ON ST.sasi_matricno = SS.sasi_matricno " +
                        "where flag = true and track_module = " + clsGeneric.NullToInteger(TrackModule) + " and category = " + clsGeneric.AddQuotes(Category);
            sqlCmd = sqlCmd + " order by ST.sasi_matricno;";

            try
            {
                if (!FormHelp.IsBlank(sqlCmd))
                {
                    using (IDataReader loReader = _DatabaseFactory.ExecuteReader(Helper.GetDataBaseType,
                        DataBaseConnectionString, sqlCmd).CreateDataReader())
                    {
                        while (loReader.Read())
                        {
                            StudentEn loItem = new StudentEn();
                            loItem.MatricNo = GetValue<string>(loReader, "sasi_matricno");
                            loItem.StudentName = GetValue<string>(loReader, "sasi_name");
                            loItem.OldProgramID = GetValue<string>(loReader, "old_sasi_pgid");
                            loItem.CurProgramID = GetValue<string>(loReader, "cur_sasi_pgid");
                            loItem.OldCrditHrs = GetValue<double>(loReader, "old_sasi_crdithrs");
                            loItem.SASI_CrditHrs = GetValue<double>(loReader, "cur_sasi_crdithrs");
                            loItem.CrditHrDiff = GetValue<double>(loReader, "difference");
                            loItem.ProgramID = GetValue<string>(loReader, "sasi_pgid");
                            loItem.CurrentSemester = GetValue<int>(loReader, "sasi_cursem");
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

        public List<StudentEn> GetListStudentChangeByProgram(string Category, int TrackModule, string FeeType)
        {
            string sqlCmd;
            List<StudentEn> loEnList = new List<StudentEn>();

            if (TrackModule == ChangeHostel)
            {
                sqlCmd = @"
                            select distinct stu.sasi_matricno, stu.sasi_name, stu.sasc_code, stu.sasi_cursem, stu.sasi_intake, stu.sasi_pgid, tn.*, prog.sapg_code
                            from sas_student stu
                            inner join sas_program prog on stu.sasi_pgid = prog.sapg_code
                            inner join sas_trackingnotes tn on stu.sasi_matricno = tn.sasi_matricno                            
                            where tn.track_module = " + clsGeneric.NullToInteger(TrackModule) + " and tn.category =  " + clsGeneric.AddQuotes(Category) + "and tn.flag = true  order by stu.sasi_matricno";
            }
            else
            {
                sqlCmd = "select distinct stu.sasi_matricno, stu.sasi_name, stu.sasi_cursem, stu.sasi_intake, stu.sasi_pgid, tn.*"
                  + " from sas_student stu inner join sas_trackingnotes tn  on stu.sasi_matricno = tn.sasi_matricno";
                if (TrackModule == ChangeProgram)
                    sqlCmd = sqlCmd + " left join sas_program prog on prog.sapg_code = tn.old_sasi_pgid ";
                if (TrackModule == ChangeCreditHrs)
                    sqlCmd = sqlCmd + " left join sas_program prog on prog.sapg_code = stu.sasi_pgid";
                sqlCmd = sqlCmd + " where tn.track_module = " + clsGeneric.NullToInteger(TrackModule) + "  and tn.category = " + clsGeneric.AddQuotes(Category) +
               " and tn.flag = true ";
                sqlCmd = sqlCmd + " order by stu.sasi_matricno";
            }
            try
            {
                if (!FormHelp.IsBlank(sqlCmd))
                {
                    using (IDataReader loReader = _DatabaseFactory.ExecuteReader(Helper.GetDataBaseType,
                        DataBaseConnectionString, sqlCmd).CreateDataReader())
                    {
                        while (loReader.Read())
                        {
                            StudentEn loItem = new StudentEn();
                            loItem.MatricNo = GetValue<string>(loReader, "sasi_matricno");
                            loItem.StudentName = GetValue<string>(loReader, "sasi_name");
                            loItem.OldProgramID = GetValue<string>(loReader, "old_sasi_pgid");
                            loItem.CurProgramID = GetValue<string>(loReader, "cur_sasi_pgid");
                            loItem.OldCrditHrs = GetValue<double>(loReader, "old_sasi_crdithrs");
                            loItem.SASI_CrditHrs = GetValue<double>(loReader, "cur_sasi_crdithrs");
                            loItem.CrditHrDiff = GetValue<double>(loReader, "difference");
                            loItem.ProgramID = GetValue<string>(loReader, "sasi_pgid");
                            loItem.CurrentSemester = GetValue<int>(loReader, "sasi_cursem");
                            loItem.SAKO_Code = GetValue<string>(loReader, "sako_code");
                            loItem.SART_Code = GetValue<string>(loReader, "sart_code");
                            loItem.SABK_Code = GetValue<string>(loReader, "sabk_code");

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

        public List<StudentEn> GetListStudentChangeDetails(string Category, int TrackModule, string FeeType)
        {
            string sqlCmd = "";
            List<StudentEn> loEnList = new List<StudentEn>();

            if (TrackModule == ChangeHostel)
            {
                sqlCmd = @"
                            select stu.sasi_matricno, stu.sasi_name, stu.sasc_code, stu.sasi_cursem, stu.sasi_intake, stu.sasi_pgid, tn.*, prog.sapg_code, ft.saft_programchange,
                            hsd.saft_code, ft.saft_taxmode, ft.saft_desc, ft.saft_priority, ft.saft_istutionfee, ft.saft_hostel, hsa.saha_amount,hsa.safa_gstamount, hsa.sasc_code
                            from sas_student stu
                            inner join sas_program prog on stu.sasi_pgid = prog.sapg_code
                            inner join sas_trackingnotes tn on stu.sasi_matricno = tn.sasi_matricno 
                            left join SAS_HostelStruct hs on hs.sahb_block = tn.sabk_code and hs.sahb_roomtype = tn.sart_code and hs.sahb_code = tn.sako_code
                            left join SAS_HostelStrDetails hsd on hs.sahs_code = hsd.sahs_code
                            left join sas_feetypes ft on ft.saft_code = hsd.saft_code
                            left join sas_hostelstramount hsa on hsa.sahs_code = hsd.sahs_code and hsd.saft_code = hsa.saft_code and stu.sasc_code = hsa.sasc_code
                            where tn.track_module = " + clsGeneric.NullToInteger(TrackModule) + " and tn.category =  " + clsGeneric.AddQuotes(Category) + "and tn.flag = true  order by stu.sasi_matricno";
            }
            else
            {

                //cheking the sast_code (effetive from ) = sasi_intake 
                //first sem student will take fee safd_type = A and safd_type = S individual & annual sem 
                //2nd sem onwards will take fee safd_type = S and individual & annual sem
                //priority of the fee admission > all semester > individual semester
                sqlCmd = @"
                        select stu.sasi_matricno, stu.sasi_name, stu.sasc_code, stu.sasi_cursem, stu.sasi_intake, stu.sasi_pgid, tn.*, fs.sapg_code, fs.safs_status, fs.safs_semester,
                        fsd.safd_type, fsd.saft_code, ft.saft_taxmode, ft.saft_desc, ft.saft_priority, ft.saft_istutionfee, ft.saft_hostel, stu.sasi_crdithrs,
                       ( SELECT CASE WHEN fsd.safd_type = 'T' THEN stu.sasi_crdithrs * fsa.safa_amount
                   ELSE SUM(acc.transamount) END 
                    FROM   SAS_SponsorInvoice D  
	                        inner join sas_sponsor_inv_rec sir on sir.invoice_id = D.batchcode 
	                        inner join sas_accounts accRec on sir.receipt_id = accRec.transid and accRec.creditRef = D.creditRef1 
	                        inner join sas_accounts accAllocation on accAllocation.creditref = D.creditref1 and accRec.BatchCode = accAllocation.creditref1 
	                        inner join sas_accounts acc on acc.creditref1 = d.creditref1 and d.creditref = acc.creditref and accAllocation.BatchCode = acc.BatchCode 
	                        where acc.poststatus = 'Posted' and acc.transstatus = 'Closed' and acc.category ='SPA' and stu.SASI_MatricNo = D.creditref  
                 )AS tuition_amount,fs.safs_tutamt, fsa.safa_amount,
                        fsa.safa_gstamount, fsa.sasc_code  ";
                if (TrackModule == ChangeCreditHrs)
                {
                    sqlCmd = sqlCmd + @" ,(CASE fs.safd_feebaseon 
                         When '1' then 	
	                        fs.safs_tutamt * ABS(tn.difference)
                        When '0' then
	                        fs.safs_tutamt
                        else
	                        0.0
                        END )as newsafa_amount         ";
                }
                if (TrackModule == ChangeProgram)
                {
                    sqlCmd = sqlCmd + ", fsa.safa_amount   as newsafa_amount";
                }
                sqlCmd = sqlCmd + " , ft.saft_programchange, fs.safd_feebaseon ";
                sqlCmd = sqlCmd + @" from sas_student stu
                        inner join sas_trackingnotes tn on stu.sasi_matricno = tn.sasi_matricno ";
                if (TrackModule == ChangeProgram)
                {
                    sqlCmd = sqlCmd + @" left join sas_program prog on prog.sapg_code = tn.old_sasi_pgid";
                }
                if (TrackModule == ChangeCreditHrs)
                {
                    sqlCmd = sqlCmd + @" left join sas_program prog on prog.sapg_code = stu.sasi_pgid";
                }
                sqlCmd = sqlCmd + @" left join sas_feestruct fs on  stu.sasi_intake = fs.sast_code  and  prog.sabp_code = fs.sabp_code ";
                if (TrackModule == ChangeCreditHrs)
                {
                    sqlCmd = sqlCmd + @" and fs.safd_feebaseon in ('0', '1')";
                }
                sqlCmd = sqlCmd + @" LEFT JOIN SAS_SemesterSetup sem ON fs.SAST_Code = sem.SAST_Code and sem.sast_code = stu.sasi_intake
                        left join  sas_feestrdetails fsd on fs.safs_code = fsd.safs_code ";
                if (TrackModule == ChangeCreditHrs)
                {
                    sqlCmd = sqlCmd + "  and fsd.safd_type = 'T' ";
                }
                if (TrackModule == ChangeProgram)
                {
                    sqlCmd = sqlCmd + @" and 
                        (CASE stu.sasi_cursem
	                        WHEN 1 THEN 
	                        (fsd.safd_type = 'A' and fsd.safd_feefor = '0' and fsd.safd_sem = 0 )
	                        or ( fsd.safd_feefor = '0' and  fsd.safd_type = 'S' and 
	                         fsd.saft_code not in (select saft_code from sas_feestrdetails where safs_code =fs.safs_code and safd_type = 'A' and safd_feefor = '0' and safd_sem = 0  ))

or (fsd.safd_type = 'T' and fsd.safd_feefor = '0' and fsd.safd_sem = 0)
	                         or (fsd.safd_sem = 1 and fsd.safd_type = 'S' and fsd.safd_feefor = '1' and
	                          fsd.saft_code not in (select saft_code from sas_feestrdetails where safs_code =fs.safs_code and safd_type = 'S' and safd_feefor = '0' ) 
	                          and fsd.saft_code not in (select saft_code from sas_feestrdetails where safs_code =fs.safs_code and safd_type = 'A' and safd_feefor = '0' and safd_sem = 0  ))
	                          else
	                          ( fsd.safd_feefor = '0' and  fsd.safd_type = 'S' ) or (fsd.safd_type = 'T' and fsd.safd_feefor = '0' and fsd.safd_sem = 0)
	                         or (fsd.safd_sem = stu.sasi_cursem and fsd.safd_type = 'S' and fsd.safd_feefor = '1' and
	                          fsd.saft_code not in (select saft_code from sas_feestrdetails where safs_code =fs.safs_code and safd_type = 'S' and safd_feefor = '0' ) )
                        END) ";
                }

                sqlCmd = sqlCmd + @" left join sas_feestramount fsa on fsd.safs_code = fsa.safs_code and stu.sasc_code = fsa.sasc_code 
                         and fsd.saft_code = fsa.saft_code and fsa.safd_feefor = fsd.safd_feefor and fsa.safd_sem = fsd.safd_sem and fsa.safd_type = fsd.safd_type
                        left join sas_feetypes ft on ft.saft_code = fsa.saft_code ";
                if (TrackModule == ChangeProgram)
                {
                    sqlCmd = sqlCmd + " and ft.saft_programchange = 1 ";
                }
                sqlCmd = sqlCmd + "  where tn.track_module = " + clsGeneric.NullToInteger(TrackModule) + "  and tn.category = " + clsGeneric.AddQuotes(Category) + " and tn.flag = true ";
                sqlCmd = sqlCmd + " order by stu.sasi_matricno";
            }


            try
            {
                if (!FormHelp.IsBlank(sqlCmd))
                {
                    using (IDataReader loReader = _DatabaseFactory.ExecuteReader(Helper.GetDataBaseType,
                        DataBaseConnectionString, sqlCmd).CreateDataReader())
                    {
                        while (loReader.Read())
                        {
                            StudentEn loItem = new StudentEn();
                            loItem.MatricNo = GetValue<string>(loReader, "sasi_matricno");
                            loItem.StudentName = GetValue<string>(loReader, "sasi_name");
                            loItem.OldProgramID = GetValue<string>(loReader, "old_sasi_pgid");
                            loItem.CurProgramID = GetValue<string>(loReader, "cur_sasi_pgid");
                            loItem.OldCrditHrs = GetValue<double>(loReader, "old_sasi_crdithrs");
                            loItem.SASI_CrditHrs = GetValue<double>(loReader, "cur_sasi_crdithrs");
                            loItem.CrditHrDiff = GetValue<double>(loReader, "difference");
                            loItem.ProgramID = GetValue<string>(loReader, "sasi_pgid");
                            loItem.CurrentSemester = GetValue<int>(loReader, "sasi_cursem");
                            loItem.TaxId = GetValue<int>(loReader, "saft_taxmode");

                            if (TrackModule == ChangeHostel)
                            {
                                loItem.TransactionAmount = GetValue<double>(loReader, "saha_amount");
                                //loItem.TaxAmount = 0.0;
                                //loItem.GSTAmount = 0.0;
                                loItem.TaxAmount = GetValue<double>(loReader, "safa_gstamount");
                                loItem.GSTAmount = GetValue<double>(loReader, "safa_gstamount");
                                loItem.SAKO_Code = GetValue<string>(loReader, "sako_code");
                                loItem.SABK_Code = GetValue<string>(loReader, "sabk_code");
                            }
                            else
                            {

                                //modified by farid 07062016
                                if (TrackModule == ChangeProgram)
                                {
                                    loItem.FeeCat = GetValue<string>(loReader, "safd_feebaseon");
                                    loItem.Internal_Use = GetValue<string>(loReader, "safd_type");
                                    loItem.StudentCredithour = GetValue<double>(loReader, "sasi_crdithrs");
                                    if (loItem.Internal_Use == "T")
                                    {
                                        GSTSetupDAL gst = new GSTSetupDAL();
                                        decimal transamount;
                                        decimal gstamount;
                                        decimal total;
                                        double calculate;
                                        if (loItem.FeeCat == "0")
                                        {
                                            loItem.TransactionAmount = GetValue<double>(loReader, "safa_amount");
                                            transamount = System.Convert.ToDecimal(loItem.TransactionAmount);
                                            gstamount = gst.GetGstAmount(loItem.TaxId, transamount);
                                            loItem.GSTAmount = GetValue<double>(loReader, "safa_gstamount");
                                            //if (loItem.TaxId == 1)
                                            //{
                                            //    loItem.TransactionAmount = System.Convert.ToDouble(gstamount) + System.Convert.ToDouble(transamount);
                                            //    loItem.GSTAmount = System.Convert.ToDouble(gstamount);
                                            //    loItem.TaxAmount = System.Convert.ToDouble(gstamount);
                                            //}
                                            //else
                                            //{
                                            //    loItem.GSTAmount = System.Convert.ToDouble(gstamount);
                                            //    loItem.TaxAmount = System.Convert.ToDouble(gstamount);
                                            //}
                                        }
                                        else if (loItem.FeeCat == "1")
                                        {
                                            loItem.TransactionAmount = GetValue<double>(loReader, "safa_amount");
                                            loItem.GSTAmount = GetValue<double>(loReader, "safa_gstamount");

                                            if (loItem.TaxId == 1)
                                            {
                                                calculate = (loItem.TransactionAmount - loItem.GSTAmount);
                                                total = System.Convert.ToDecimal(calculate * loItem.StudentCredithour);
                                                gstamount = gst.GetGstAmount(loItem.TaxId, total);
                                                loItem.TransactionAmount = System.Convert.ToDouble(gstamount) + System.Convert.ToDouble(total);
                                                loItem.GSTAmount = System.Convert.ToDouble(gstamount);
                                                loItem.TaxAmount = System.Convert.ToDouble(gstamount);
                                            }
                                            else
                                            {
                                                transamount = System.Convert.ToDecimal(loItem.TransactionAmount);
                                                gstamount = gst.GetGstAmount(loItem.TaxId, transamount);
                                                loItem.TransactionAmount = System.Convert.ToDouble(transamount);
                                                loItem.GSTAmount = System.Convert.ToDouble(gstamount);
                                                loItem.TaxAmount = System.Convert.ToDouble(gstamount);
                                            }
                                        }
                                    }
                                    else
                                    {
                                        loItem.TransactionAmount = GetValue<double>(loReader, "newsafa_amount");
                                        loItem.TaxAmount = GetValue<double>(loReader, "safa_gstamount");
                                        loItem.GSTAmount = GetValue<double>(loReader, "safa_gstamount");
                                    }
                                }
                                else
                                {
                                    loItem.TransactionAmount = GetValue<double>(loReader, "newsafa_amount");
                                    loItem.TaxAmount = GetValue<double>(loReader, "safa_gstamount");
                                    loItem.GSTAmount = GetValue<double>(loReader, "safa_gstamount");
                                }
                            }
                            if (TrackModule == ChangeCreditHrs)
                            {
                                loItem.TaxAmount = 0.0;
                                loItem.GSTAmount = 0.0;
                            }
                            loItem.ReferenceCode = GetValue<string>(loReader, "saft_code");
                            loItem.Description = GetValue<string>(loReader, "saft_desc");
                            loItem.Priority = GetValue<int>(loReader, "saft_priority");
                            loItem.IsTutionFee = GetValue<int>(loReader, "saft_istutionfee");
                            loItem.ProgramChange = GetValue<int>(loReader, "saft_programchange");
                            loItem.CategoryCode = GetValue<string>(loReader, "sasc_code");
                            if (TrackModule != ChangeHostel)
                            {
                                loItem.FeeBaseOn = GetValue<string>(loReader, "safd_feebaseon");
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

        #region GetStudentDetails
        public List<StudentEn> GetStudentDetails(string MatricNo, string programID)
        {
            List<StudentEn> loEnList = new List<StudentEn>();

            string sqlCmd = "select * from sas_student where " +
                "sas_student.sasi_matricno ='" + MatricNo + "'";
            //AND sasi_pgid ='" + programID +"'";

            sqlCmd = sqlCmd + " order by SASI_MatricNo";

            try
            {
                if (!FormHelp.IsBlank(sqlCmd))
                {
                    using (IDataReader loReader = _DatabaseFactory.ExecuteReader(Helper.GetDataBaseType,
                        DataBaseConnectionString, sqlCmd).CreateDataReader())
                    {
                        while (loReader.Read())
                        {
                            StudentEn loItem = LoadObjectForSem(loReader);
                            loItem.MatricNo = GetValue<string>(loReader, "SASI_MatricNo");
                            loItem.StudentName = GetValue<string>(loReader, "SASI_Name");
                            //  loItem.ProgramID = GetValue<string>(loReader, "SASI_PgId");
                            loItem.CurrentSemester = GetValue<int>(loReader, "SASI_CurSem");
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

        #region GetStudentDetailsBySponsor

        public List<StudentEn> GetStudentDetailsBySponsor(string programID, string Sponsor)
        {
            List<StudentEn> loEnList = new List<StudentEn>();

            //string sqlCmd = "select Distinct A.SASI_MatricNo, SASI_Name, A.SASI_PgId, A.SASI_CurSem,B.SASS_Sponsor, B. sass_limit " +
            //                "from SAS_student A INNER JOIN SAS_StudentSpon B ON A.SASI_MAtricNO = B.SASI_MAtricNO " +
            //                "INNER JOIN SAS_StudentCategoryAccess ON A.SASS_Code = SAS_StudentCategoryAccess.SASC_Code where " +
            //                "B.SASS_Sponsor = " + clsGeneric.AddQuotes(Sponsor) + " and A.SASI_PgId = " + clsGeneric.AddQuotes(programID);
            string sqlCmd = "Select R.SASI_MatricNo, R.SASI_Name, R.SASI_PgId, R.SASI_CurSem,R.SASS_Sponsor, R.sass_limit,sum(TransAmount) AllocatedAmount, " +
                            "(R.sass_limit - Sum(Transamount)) AvailableAmont From ( " +
                            "select Distinct A.SASI_MatricNo, SASI_Name, A.SASI_PgId, A.SASI_CurSem,B.SASS_Sponsor, B. sass_limit " +
                            "from SAS_student A INNER JOIN SAS_StudentSpon B ON A.SASI_MAtricNO = B.SASI_MAtricNO " +
                            "INNER JOIN SAS_StudentCategoryAccess ON A.SASS_Code = SAS_StudentCategoryAccess.SASC_Code where " +
                            "B.SASS_Sponsor = " + clsGeneric.AddQuotes(Sponsor) + " and A.SASI_PgId = " + clsGeneric.AddQuotes(programID) +
                            " ) R LEFT JOIN SAS_SponsorInvoice D ON R.SASI_MatricNo = D.creditref " +
                            "Group By R.SASI_MatricNo, R.SASI_Name, R.SASI_PgId, R.SASI_CurSem,R.SASS_Sponsor, R.sass_limit order by R.SASI_MatricNo ";

            //sqlCmd = sqlCmd + " order by A.SASI_MatricNo";

            try
            {
                if (!FormHelp.IsBlank(sqlCmd))
                {
                    using (IDataReader loReader = _DatabaseFactory.ExecuteReader(Helper.GetDataBaseType,
                        DataBaseConnectionString, sqlCmd).CreateDataReader())
                    {
                        while (loReader.Read())
                        {
                            StudentEn loItem = LoadObjectForSem(loReader);
                            loItem.MatricNo = GetValue<string>(loReader, "SASI_MatricNo");
                            loItem.StudentName = GetValue<string>(loReader, "SASI_Name");
                            loItem.ProgramID = GetValue<string>(loReader, "SASI_PgId");
                            loItem.CurrentSemester = GetValue<int>(loReader, "SASI_CurSem");
                            loItem.SponsorLimit = GetValue<double>(loReader, "sass_limit");
                            loItem.PaidAmount = GetValue<double>(loReader, "AllocatedAmount");
                            loItem.OutstandingAmount = GetValue<double>(loReader, "AvailableAmont");
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

        public List<StudentEn> GetStudentDetailsBySponsorWithStuValidity(string programID, string Sponsor)
        {
            List<StudentEn> loEnList = new List<StudentEn>();

            //string sqlCmd = "select Distinct A.SASI_MatricNo, SASI_Name, A.SASI_PgId, A.SASI_CurSem,B.SASS_Sponsor, B. sass_limit " +
            //                "from SAS_student A INNER JOIN SAS_StudentSpon B ON A.SASI_MAtricNO = B.SASI_MAtricNO " +
            //                "INNER JOIN SAS_StudentCategoryAccess ON A.SASS_Code = SAS_StudentCategoryAccess.SASC_Code where " +
            //                "B.SASS_Sponsor = " + clsGeneric.AddQuotes(Sponsor) + " and A.SASI_PgId = " + clsGeneric.AddQuotes(programID);

            //string sqlCmd = "Select R.SASI_MatricNo, R.SASI_Name, R.SASI_PgId, R.SASI_CurSem,R.SASS_Sponsor, R.sass_limit,sum(TransAmount) AllocatedAmount, " +
            //                "(R.sass_limit - Sum(Transamount)) AvailableAmont From ( " +
            //                "select Distinct A.SASI_MatricNo, SASI_Name, A.SASI_PgId, A.SASI_CurSem,B.SASS_Sponsor, B. sass_limit " +
            //                "from SAS_student A INNER JOIN SAS_StudentSpon B ON A.SASI_MAtricNO = B.SASI_MAtricNO " +
            //                "INNER JOIN SAS_StudentCategoryAccess ON A.SASS_Code = SAS_StudentCategoryAccess.SASC_Code where " +
            //                "B.SASS_Sponsor = " + clsGeneric.AddQuotes(Sponsor) + " and A.SASI_PgId = " + clsGeneric.AddQuotes(programID) +
            //                " and A.SASI_Reg_Status =" + Helper.StuRegistered
            //                + " and A.sass_code =" + clsGeneric.AddQuotes(Helper.StuActive) +
            //                " and CAST(B.SASS_SDATE  as DATE) <= " + clsGeneric.AddQuotes(Helper.DateConversion(DateTime.Today))
            //                + " and CAST(B.SASS_EDATE  as DATE) >= " + clsGeneric.AddQuotes(Helper.DateConversion(DateTime.Today)) +
            //                ") R LEFT JOIN SAS_SponsorInvoice D ON R.SASI_MatricNo = D.creditref " +
            //                "Group By R.SASI_MatricNo, R.SASI_Name, R.SASI_PgId, R.SASI_CurSem,R.SASS_Sponsor, R.sass_limit order by R.SASI_MatricNo ";

            string sqlCmd = @"
                      
                        select Distinct  stu.SASI_MatricNo, SASI_Name,  stu.SASI_PgId,  stu.sasi_intake,  stu.SASI_CurSem, sspon.SASS_Sponsor, sspon.sass_limit, stu.sasc_code, sspon.sass_type,
                         ( SELECT CASE WHEN SUM(acc.transamount) IS NULL THEN 0
                        ELSE SUM(acc.transamount) END 
                        FROM   SAS_SponsorInvoice D  
	                            inner join sas_sponsor_inv_rec sir on sir.invoice_id = D.batchcode 
	                            inner join sas_accounts accRec on sir.receipt_id = accRec.transid and accRec.creditRef = D.creditRef1 
	                            inner join sas_accounts accAllocation on accAllocation.creditref = D.creditref1 and accRec.BatchCode = accAllocation.creditref1 
	                            inner join sas_accounts acc on acc.creditref1 = d.creditref1 and d.creditref = acc.creditref and accAllocation.BatchCode = acc.BatchCode 
	                            where acc.poststatus = 'Posted' and acc.transstatus = 'Closed' and acc.category ='SPA' and stu.SASI_MatricNo = D.creditref  
                        )AS  AllocatedAmount, 
                        (select case when sspon.sass_limit = 0 THEN 0
                        ELSE
                            (sspon.sass_limit - (SELECT CASE WHEN SUM(acc.transamount) IS NULL THEN 0
                        ELSE SUM(acc.transamount) END 
                        FROM   SAS_SponsorInvoice D  
	                            inner join sas_sponsor_inv_rec sir on sir.invoice_id = D.batchcode 
	                            inner join sas_accounts accRec on sir.receipt_id = accRec.transid and accRec.creditRef = D.creditRef1 
	                            inner join sas_accounts accAllocation on accAllocation.creditref = D.creditref1 and accRec.BatchCode = accAllocation.creditref1 
	                            inner join sas_accounts acc on acc.creditref1 = d.creditref1 and d.creditref = acc.creditref and accAllocation.BatchCode = acc.BatchCode 
	                            where acc.poststatus = 'Posted' and acc.transstatus = 'Closed' and acc.category ='SPA' and stu.SASI_MatricNo = D.creditref  ))
                        END) AS AvailableAmont
                        from SAS_student stu
                        INNER JOIN SAS_StudentSpon sspon ON  stu.SASI_MAtricNO = sspon.SASI_MAtricNO 
                        INNER JOIN SAS_StudentCategoryAccess ON  stu.SASS_Code = SAS_StudentCategoryAccess.SASC_Code                         
                        left join sas_program prog on prog.sapg_code = stu.sasi_pgid
                        left join sas_feestruct fs on  stu.sasi_intake = fs.sast_code and prog.sabp_code = fs.sabp_code 
                        left join sas_semestersetup sem on fs.sast_code = sem.sast_code and sem.sast_code = stu.sasi_intake
                        left join sas_feestrdetails fsd on fs.safs_code = fsd.safs_code and
                        (CASE stu.sasi_cursem
                        WHEN 1 THEN 
                        (fsd.safd_type = 'A' and fsd.safd_feefor = '0' and fsd.safd_sem = 0 )
                        or ( fsd.safd_feefor = '0' and  fsd.safd_type = 'S' and 
                        fsd.saft_code not in (select saft_code from sas_feestrdetails where safs_code =fs.safs_code and safd_type = 'A' and safd_feefor = '0' and safd_sem = 0  ))
                        or (fsd.safd_sem = 1 and fsd.safd_type = 'S' and fsd.safd_feefor = '1' and
                        fsd.saft_code not in (select saft_code from sas_feestrdetails where safs_code =fs.safs_code and safd_type = 'S' and safd_feefor = '0' ) 
                        and fsd.saft_code not in (select saft_code from sas_feestrdetails where safs_code =fs.safs_code and safd_type = 'A' and safd_feefor = '0' and safd_sem = 0  ))

                        else
                        ( fsd.safd_feefor = '0' and  fsd.safd_type = 'S' )
                        or (fsd.safd_sem = stu.sasi_cursem and fsd.safd_type = 'S' and fsd.safd_feefor = '1' and
                        fsd.saft_code not in (select saft_code from sas_feestrdetails where safs_code =fs.safs_code and safd_type = 'S' and safd_feefor = '0' ) )
                        END)
                        left join sas_feestramount fsa on fs.safs_code = fsa.safs_code and stu.sasc_code = fsa.sasc_code  and fsd.saft_code = fsa.saft_code
                        and fsd.saft_code = fsa.saft_code and fsa.safd_feefor = fsd.safd_feefor and fsa.safd_sem = fsd.safd_sem and fsa.safd_type = fsd.safd_type
                        left join sas_feetypes ft on ft.saft_code = fsa.saft_code and fsd.saft_code = fsa.saft_code    
                        
                       where sspon.SASS_Sponsor = " + clsGeneric.AddQuotes(Sponsor) + " and stu.SASI_PgId = " + clsGeneric.AddQuotes(programID) +
                        " and stu.SASI_Reg_Status =" + Helper.StuRegistered +
                        " and stu.sass_code =" + clsGeneric.AddQuotes(Helper.StuActive) +
                        " and TO_DATE( sspon.SASS_sDATE,'DD/MM/YYYY') <= current_date and TO_DATE( sspon.SASS_EDATE,'DD/MM/YYYY') >= current_date " +
                //    " and TO_DATE( B.SASS_sDATE,'DD/MM/YYYY') <= current_date and  TO_DATE( B.SASS_EDATE,'YYYY/MM/DD') >= current_date " +
                        " Group By stu.SASI_MatricNo, stu.SASI_Name, stu.SASI_PgId, stu.sasi_intake, stu.SASI_CurSem, sspon.SASS_Sponsor, sspon.sass_limit, stu.sasc_code, sspon.sass_type order by stu.SASI_MatricNo ";
            //inner join sas_sponsorfeetypes sft on B.sass_sponsor = sft.sasr_code and ft.saft_code = sft.saft_code
            //sqlCmd = sqlCmd + " order by A.SASI_MatricNo";

            try
            {
                if (!FormHelp.IsBlank(sqlCmd))
                {
                    using (IDataReader loReader = _DatabaseFactory.ExecuteReader(Helper.GetDataBaseType,
                        DataBaseConnectionString, sqlCmd).CreateDataReader())
                    {
                        while (loReader.Read())
                        {
                            StudentEn loItem = LoadObjectForSem(loReader);
                            loItem.MatricNo = GetValue<string>(loReader, "SASI_MatricNo");
                            loItem.StudentName = GetValue<string>(loReader, "SASI_Name");
                            loItem.ProgramID = GetValue<string>(loReader, "SASI_PgId");
                            loItem.CurrentSemester = GetValue<int>(loReader, "SASI_CurSem");
                            loItem.SponsorLimit = GetValue<double>(loReader, "sass_limit");
                            loItem.PaidAmount = GetValue<double>(loReader, "AllocatedAmount");
                            loItem.OutstandingAmount = GetValue<double>(loReader, "AvailableAmont");
                            loItem.CategoryCode = GetValue<string>(loReader, "sasc_code");
                            loItem.FullySponsor = GetValue<Boolean>(loReader, "sass_type");
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

        #region UpdateTrackingNote

        public bool UpdateTrackingNote(List<StudentEn> trackingnote, int trackid)
        {
            bool lbRes = false;
            try
            {
                if (trackingnote.Count > 0)
                {
                    foreach (StudentEn i in trackingnote.ToArray())
                    {
                        string sqlCmd = "";
                        if (trackid == 1)
                        {
                            sqlCmd = "UPDATE sas_trackingnotes SET flag=false WHERE sasi_matricno= " + clsGeneric.AddQuotes(i.MatricNo) +
                                " and cur_sasi_pgid= " + clsGeneric.AddQuotes(i.CurProgramID) + " and old_sasi_pgid= " + clsGeneric.AddQuotes(i.OldProgramID) +
                                " and category=" + clsGeneric.AddQuotes(i.Category) +
                                " and track_module= " + trackid;
                        }
                        else if (trackid == 2)
                        {
                            sqlCmd = "UPDATE sas_trackingnotes SET flag=false WHERE sasi_matricno= " + clsGeneric.AddQuotes(i.MatricNo) +
                                " and cur_sasi_crdithrs= " + i.SASI_CrditHrs + " and old_sasi_crdithrs= " + i.OldCrditHrs + " and difference= " + i.CrditHrDiff +
                                " and category= " + clsGeneric.AddQuotes(i.Category) +
                                " and track_module= " + trackid;
                        }
                        else if (trackid == 3)
                        {
                            sqlCmd = "UPDATE sas_trackingnotes SET flag=false WHERE sasi_matricno= " + clsGeneric.AddQuotes(i.MatricNo) +
                                " and category= " + clsGeneric.AddQuotes(i.Category) +
                                " and track_module= " + trackid;
                        }
                        if (!FormHelp.IsBlank(sqlCmd))
                        {
                            int liRowAffected = _DatabaseFactory.ExecuteSqlStatement(Helper.
                                GetDataBaseType, DataBaseConnectionString, sqlCmd);

                            if (liRowAffected > 0)
                                lbRes = true;

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

        #region UpdateIsReleased
        //Added by Hafiz Roslan
        //On 08/01/2016
        //Modified by Hafiz Roslan @ 16/2/2106

        public bool UpdateIsReleased(String MatricNo, Double OutstandingAmount, int IsReleased, String sqlCmd_extend)
        {
            bool flag = false;
            string sqlCmd = String.Empty;

            if (IsReleased != 2)
            {
                if (OutstandingAmount > 0)
                {
                    //saso_dueamount>0 = saso_isreleased=1 = 'N'/'Blocked' in SMP
                    sqlCmd = " UPDATE SAS_StudentOutstanding SET SASO_IsReleased = 1 WHERE ";
                    sqlCmd += " SASI_MatricNo = " + clsGeneric.AddQuotes(MatricNo) + " ";
                }
                else if (OutstandingAmount <= 0)
                {
                    //saso_dueamount=0 = saso_isreleased=0 = 'Y'/'UnBlocked' in SMP
                    sqlCmd = " UPDATE SAS_StudentOutstanding SET SASO_IsReleased = 0 WHERE ";
                    sqlCmd += " SASI_MatricNo = " + clsGeneric.AddQuotes(MatricNo) + " ";
                }
            }
            else
            {
                if (OutstandingAmount > 0)
                {
                    //saso_dueamount>0 = saso_isreleased=1 = 'N'/'Blocked' in SMP
                    sqlCmd = " UPDATE SAS_StudentOutstanding SET SASO_IsReleased = 1 WHERE ";
                    sqlCmd += " SASI_MatricNo = " + clsGeneric.AddQuotes(MatricNo) + " ";
                }
                else if (OutstandingAmount <= 0)
                {
                    //saso_dueamount=0 = saso_isreleased=0 = 'Y'/'UnBlocked' in SMP
                    sqlCmd = " UPDATE SAS_StudentOutstanding SET SASO_IsReleased = 0 WHERE ";
                    sqlCmd += " SASI_MatricNo = " + clsGeneric.AddQuotes(MatricNo) + " ";
                }
            }

            sqlCmd = sqlCmd + sqlCmd_extend;

            try
            {
                if (!FormHelp.IsBlank(sqlCmd))
                {
                    int liRowAffected = _DatabaseFactory.ExecuteSqlStatement(Helper.
                        GetDataBaseType, DataBaseConnectionString, sqlCmd);

                    if (liRowAffected > -1)
                    {
                        flag = true;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return flag;
        }

        #endregion

        #region InsStudOutstanding

        //added by Hafiz @ 23/2/2016
        //another version of insertion into sas_studentoutstanding
        //modified by Hafiz @ 11/3/2016
        //modified by Hafiz @ 25/4/2016
        //modified by Hafiz @ 27/5/2016

        public bool InsStudOutstanding(StudentEn argEn)
        {
            bool lbRes = false;
            int iOut = 0;
            string sqlCmd = "Select count(*) as cnt From SAS_StudentOutstanding where SASI_MatricNo = '" + argEn.MatricNo + "'";
            try
            {
                if (!FormHelp.IsBlank(sqlCmd))
                {
                    using (IDataReader dr = _DatabaseFactory.ExecuteReader(Helper.GetDataBaseType,
                        DataBaseConnectionString, sqlCmd).CreateDataReader())
                    {
                        if (dr.Read())
                            iOut = clsGeneric.NullToInteger(dr["cnt"]);

                        dr.Close();

                        if (iOut > 1)
                            throw new Exception("Record Already Exist!");
                    }

                    //no student found
                    if (iOut == 0)
                    {
                        sqlCmd = @"INSERT INTO SAS_StudentOutstanding(SASI_MatricNo,SASI_Name,SASI_PgId,SASI_CurSem,SASI_CurSemYr,SASO_Outstandingamt,SASO_isReleased)
                                   VALUES(@SASI_MatricNo,@SASI_Name,@SASI_PgId,@SASI_CurSem,@SASI_CurSemYr,@SASO_Outstandingamt,@SASO_isReleased) ";

                        if (!FormHelp.IsBlank(sqlCmd))
                        {
                            DbCommand cmd = _DatabaseFactory.GetDbCommand(Helper.GetDataBaseType, sqlCmd, DataBaseConnectionString);
                            _DatabaseFactory.AddInParameter(ref cmd, "@SASI_MatricNo", DbType.String, argEn.MatricNo);
                            _DatabaseFactory.AddInParameter(ref cmd, "@SASI_Name", DbType.String, argEn.StudentName);
                            _DatabaseFactory.AddInParameter(ref cmd, "@SASI_PgId", DbType.String, argEn.ProgramID);
                            _DatabaseFactory.AddInParameter(ref cmd, "@SASI_CurSem", DbType.Int32, argEn.CurrentSemester);
                            _DatabaseFactory.AddInParameter(ref cmd, "@SASI_CurSemYr", DbType.String, argEn.CurretSemesterYear);
                            _DatabaseFactory.AddInParameter(ref cmd, "@SASO_Outstandingamt", DbType.Double, argEn.OutstandingAmount);

                            if (argEn.OutstandingAmount < 0)
                            {
                                _DatabaseFactory.AddInParameter(ref cmd, "@SASO_isReleased", DbType.Int32, 0);
                            }
                            else if (argEn.OutstandingAmount > 0)
                            {
                                _DatabaseFactory.AddInParameter(ref cmd, "@SASO_isReleased", DbType.Int32, 1);
                            }
                            else if (argEn.OutstandingAmount == 0)
                            {
                                _DatabaseFactory.AddInParameter(ref cmd, "@SASO_Outstandingamt", DbType.Double, 0.0);
                                _DatabaseFactory.AddInParameter(ref cmd, "@SASO_isReleased", DbType.Int32, 0);
                            }

                            _DbParameterCollection = cmd.Parameters;

                            int liRowAffected = _DatabaseFactory.ExecuteNonQuery(Helper.GetDataBaseType, cmd,
                                DataBaseConnectionString, sqlCmd, _DbParameterCollection);

                            if (liRowAffected > -1)
                                lbRes = true;
                            else
                                throw new Exception("Insert Failed!");
                        }
                    }

                    //found student
                    if (iOut == 1)
                    {
                        double Outstandingamt = 0;
                        int flag = 0;

                        sqlCmd = "SELECT * From SAS_StudentOutstanding WHERE SASI_MatricNo = '" + argEn.MatricNo + "'";

                        if (!FormHelp.IsBlank(sqlCmd))
                        {
                            using (IDataReader loReader = _DatabaseFactory.ExecuteReader(Helper.GetDataBaseType,
                                DataBaseConnectionString, sqlCmd).CreateDataReader())
                            {
                                while (loReader.Read())
                                {
                                    Outstandingamt = GetValue<double>(loReader, "SASO_Outstandingamt");
                                    flag = GetValue<int>(loReader, "SASO_isreleased");

                                    //updated the data - start
                                    String sql_cmd = "";

                                    if (Outstandingamt != argEn.OutstandingAmount)
                                    {
                                        sql_cmd = "UPDATE SAS_StudentOutstanding SET SASO_Outstandingamt = @SASO_Outstandingamt ";
                                    }
                                    if (argEn.OutstandingAmount == 0)
                                    {
                                        if (flag != 2)
                                        {
                                            sql_cmd = "UPDATE SAS_StudentOutstanding SET SASO_Outstandingamt = @SASO_Outstandingamt, SASO_isreleased = '0' ";
                                        }
                                        else
                                        {
                                            sql_cmd = "UPDATE SAS_StudentOutstanding SET SASO_Outstandingamt = @SASO_Outstandingamt ";
                                        }
                                    }

                                    if (sql_cmd != "")
                                    {
                                        sql_cmd = sql_cmd + "WHERE SASI_MatricNo = '" + argEn.MatricNo + "'";
                                    }

                                    //execute the query - start
                                    if (!FormHelp.IsBlank(sql_cmd))
                                    {
                                        DbCommand cmd = _DatabaseFactory.GetDbCommand(Helper.GetDataBaseType, sql_cmd, DataBaseConnectionString);
                                        _DatabaseFactory.AddInParameter(ref cmd, "@SASO_Outstandingamt", DbType.String, argEn.OutstandingAmount);
                                        _DbParameterCollection = cmd.Parameters;

                                        int liRowAffected = _DatabaseFactory.ExecuteNonQuery(Helper.GetDataBaseType, cmd,
                                            DataBaseConnectionString, sql_cmd, _DbParameterCollection);

                                        if (liRowAffected > -1)
                                        {
                                            lbRes = true;
                                        }
                                        else
                                            throw new Exception("Update Failed!");
                                    }
                                    //updated the data - end

                                    //execute the query - end
                                }

                                loReader.Close();
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

        #region UpdateFlag
        //added by Hafiz @ 31/3/2016
        //modified by Hafiz @ 27/5/2016
        public bool UpdateFlag(String matricNo)
        {
            bool res = false;

            String sql_cmd = "";
            String sqlCmd = "";
            double outstandingamt = 0.0;
            int flag = 0;

            sql_cmd = "SELECT * From SAS_StudentOutstanding WHERE SASI_MatricNo = '" + matricNo + "'";

            if (!FormHelp.IsBlank(sql_cmd))
            {
                using (IDataReader loReader = _DatabaseFactory.ExecuteReader(Helper.GetDataBaseType,
                    DataBaseConnectionString, sql_cmd).CreateDataReader())
                {
                    while (loReader.Read())
                    {
                        outstandingamt = GetValue<double>(loReader, "SASO_Outstandingamt");
                        flag = GetValue<int>(loReader, "SASO_isreleased");

                        if (flag != 2)
                        {
                            if (outstandingamt <= 0)
                            {
                                if (flag != 0)
                                {
                                    sqlCmd = "UPDATE SAS_StudentOutstanding SET SASO_isreleased = '0'";
                                }
                            }
                            else if (outstandingamt > 0)
                            {
                                if (flag != 1)
                                {
                                    sqlCmd = "UPDATE SAS_StudentOutstanding SET SASO_isreleased = '1'";
                                }
                            }

                            if (sqlCmd != "")
                            {
                                sqlCmd = sqlCmd + "WHERE SASI_MatricNo = '" + matricNo + "'";
                            }
                        }

                        if (!FormHelp.IsBlank(sqlCmd))
                        {
                            int liRowAffected = _DatabaseFactory.ExecuteSqlStatement(Helper.GetDataBaseType,
                                 DataBaseConnectionString, sqlCmd);

                            if (liRowAffected > -1)
                                res = true;
                            else
                                throw new Exception("Update Failed! No Row has been updated...");
                        }
                    }

                    loReader.Close();
                }
            }

            return res;
        }

        #endregion
        
        #region UpdateOutStatInclCurSem
        //Added by Hafiz @ 26/2/2016
        //Update new outstanding status while Exclude Cur. Sem. checkbox checked

        public bool UpdateOutStatInclCurSem(StudentEn argEn)
        {
            bool lbRes = false;
            int iOut = 0;

            string sqlCmd = "Select count(*) as cnt From SAS_StudentOutstanding where SASI_MatricNo = '" + argEn.MatricNo + "'";

            try
            {
                if (!FormHelp.IsBlank(sqlCmd))
                {
                    using (IDataReader dr = _DatabaseFactory.ExecuteReader(Helper.GetDataBaseType,
                        DataBaseConnectionString, sqlCmd).CreateDataReader())
                    {
                        if (dr.Read())
                            iOut = clsGeneric.NullToInteger(dr["cnt"]);
                        dr.Close();
                        if (iOut > 1)
                            throw new Exception("Record Already Exist");
                    }

                    if (iOut == 1)
                    {
                        string sql_upd = "";
                        int is_released = 0;

                        sqlCmd = "SELECT * From SAS_StudentOutstanding WHERE SASI_MatricNo = '" + argEn.MatricNo + "'";

                        if (!FormHelp.IsBlank(sqlCmd))
                        {
                            using (IDataReader loReader = _DatabaseFactory.ExecuteReader(Helper.GetDataBaseType,
                                DataBaseConnectionString, sqlCmd).CreateDataReader())
                            {
                                while (loReader.Read())
                                {
                                    is_released = GetValue<int>(loReader, "SASO_IsReleased");

                                    //updated the data - start
                                    if (is_released == argEn.IsReleased)
                                    {
                                        return true;
                                    }
                                    else
                                    {
                                        if (argEn.IsReleased != 2)
                                        {
                                            if (argEn.OutstandingAmount > 0)
                                            {
                                                //saso_dueamount>0 = saso_isreleased=1 = 'N'/'Blocked' in SMP
                                                sql_upd = " UPDATE SAS_StudentOutstanding SET SASO_IsReleased = 1 WHERE ";
                                                sql_upd += " SASI_MatricNo = '" + argEn.MatricNo + "' ";
                                            }
                                            else if (argEn.OutstandingAmount <= 0)
                                            {
                                                //saso_dueamount=0 = saso_isreleased=0 = 'Y'/'UnBlocked' in SMP
                                                sql_upd = " UPDATE SAS_StudentOutstanding SET SASO_IsReleased = 0 WHERE ";
                                                sql_upd += " SASI_MatricNo = '" + argEn.MatricNo + "' ";
                                            }
                                        }
                                        else
                                        {
                                            //updated with 2 stats
                                            sql_upd = " UPDATE SAS_StudentOutstanding SET SASO_IsReleased = 2 WHERE ";
                                            sql_upd += " SASI_MatricNo = '" + argEn.MatricNo + "' ";
                                        }
                                    }

                                    //execute the query - start
                                    if (!FormHelp.IsBlank(sql_upd))
                                    {
                                        int liRowAffected = _DatabaseFactory.ExecuteSqlStatement(Helper.
                                            GetDataBaseType, DataBaseConnectionString, sql_upd);

                                        if (liRowAffected > -1)
                                        {
                                            lbRes = true;
                                        }
                                    }
                                    //execute the query - end
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

        //added by Hafiz @ 04/3/2016
        //modified by Hafiz @ 27/5/2016
        //methods for load data from sas_studentoutstanding
        public StudentEn GetOutstandingForUploadedFile(StudentEn argEn)
        {
            StudentEn loItem = new StudentEn();

            string sqlCmd = @"SELECT  
                                SASO.SASO_Id ,
                                SASO.SASI_MatricNo ,
                                SASO.SASI_Name ,
                                SASO.SASI_PgId ,
                                SASO.SASI_CurSem ,
                                SASO.SASI_CurSemYr ,
                                SASO.SASO_Outstandingamt ,
                                SASO.SASO_IsReleased
                    FROM SAS_StudentOutstanding SASO
                    INNER JOIN SAS_Student SASS ON SASS.SASI_MatricNo = SASO.SASI_MatricNo
                    WHERE SASO.SASO_Outstandingamt IS NOT NULL 
                    AND SASS.SASS_Code IN ('1') 
                    AND SASO.SASI_MatricNo = '" + argEn.MatricNo + "' ";

            sqlCmd = sqlCmd + @" order by SASI_MatricNo";
            try
            {
                if (!FormHelp.IsBlank(sqlCmd))
                {
                    using (IDataReader loReader = _DatabaseFactory.ExecuteReader(Helper.GetDataBaseType,
                        DataBaseConnectionString, sqlCmd).CreateDataReader())
                    {
                        while (loReader.Read())
                        {
                            loItem.MatricNo = GetValue<string>(loReader, "SASI_MatricNo");
                            loItem.StudentName = GetValue<string>(loReader, "SASI_Name");
                            loItem.ProgramID = GetValue<string>(loReader, "SASI_PgId");
                            loItem.CurrentSemester = GetValue<int>(loReader, "SASI_CurSem");
                            loItem.CurretSemesterYear = GetValue<string>(loReader, "SASI_CurSemYr");
                            loItem.OutstandingAmount = GetValue<double>(loReader, "SASO_Outstandingamt");
                            loItem.IsReleased = GetValue<int>(loReader, "SASO_IsReleased");
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

        #region GetStudentHostelFee

        public decimal GetStudentHostelFee(string MatricNo)
        {
            //variable declarations
            decimal HostelFee = 0;

            try
            {
                //build sql statement Debit Amount - Start
                string sqlCmd = "select SUM(SAHA_Amount) from SAS_Student SS,SAS_HostelStruct SH,SAS_HostelStrAmount SA, " +
                             "SAS_FeeTypes SF where SASI_Hostel = true and SASI_MatricNo = " + MaxGeneric.clsGeneric.AddQuotes(MatricNo) + " and (SS.SAKO_Code = SH.SAHB_Code and SS.SABK_Code = SH.SAHB_Block) " +
                             "and SH.SAHS_Code = SA.SAHS_Code and SS.SASI_FeeCat = SA.SASC_Code and SF.SAFT_Code = SA.SAFT_Code and  sasi_hostel = true";
                //build sql statement Debit Amount - Stop                

                if (!FormHelp.IsBlank(sqlCmd))
                {
                    HostelFee = clsGeneric.NullToDecimal(_DatabaseFactory.ExecuteScalar(
                        Helper.GetDataBaseType, DataBaseConnectionString, sqlCmd));
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return HostelFee;
        }

        #endregion

        #region GetStudentKokoFee

        public decimal GetStudentKokoFee(string MatricNo)
        {
            //variable declarations
            decimal KokoFee = 0;

            try
            {
                //build sql statement Debit Amount - Start
                //string sqlCmd = "select (SKD.SAKOD_FeeAmount * SK.SAKO_CreditHours) as SAHA_AMOUNT from SAS_Student SS,SAS_Kokorikulum SK,SAS_KokorikulumDetails SKD " +
                //                "where SASI_MatricNo = " + MaxGeneric.clsGeneric.AddQuotes(MatricNo) + " and SASI_KokoCode <> '-1' and SS.SASI_FeeCat = SKD.SAKOD_CategoryCode " +
                //                "and SS.SASI_KokoCode = SK.SAKO_Code and SKD.SAKO_Code = SK.SAKO_Code";
                string sqlCmd = "select case when (select sasi_hostel from sas_student where sasi_matricno = " + MaxGeneric.clsGeneric.AddQuotes(MatricNo) + ") = true then sum (sakod_feeamount) " +
                                "else sum (sakod_feeamountout) end as SAHA_AMOUNT from sas_kokorikulumdetails " +
                                "where sako_code = (select sasi_kokocode from sas_student where sasi_matricno = " + MaxGeneric.clsGeneric.AddQuotes(MatricNo) + ") and " +
                                "sakod_categorycode = (select sasc_code from sas_student where sasi_matricno = " + MaxGeneric.clsGeneric.AddQuotes(MatricNo) + ")";
                //build sql statement Debit Amount - Stop                

                if (!FormHelp.IsBlank(sqlCmd))
                {
                    KokoFee = clsGeneric.NullToDecimal(_DatabaseFactory.ExecuteScalar(
                        Helper.GetDataBaseType, DataBaseConnectionString, sqlCmd));
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return KokoFee;
        }

        #endregion      
  
        #region GetTuitionFee

        //public decimal GetTuitionFee(string Intake, string Program, double CrdtHr)
        //{
        //    //variable declarations
        //    decimal TuitionFee = 0;
        //    double Amount = 0;
        //    string FeeBase = "";

        //    try
        //    {
        //        //build sql statement Debit Amount - Start
        //        string sqlCmd = "select safs_tutamt,safd_feebaseon from sas_feestruct where sast_code = " + MaxGeneric.clsGeneric.AddQuotes(Intake) +
        //                        " and sabp_code in (SELECT sabp_code FROM sas_program WHERE sapg_code = " + MaxGeneric.clsGeneric.AddQuotes(Program) + ")";
        //        //build sql statement Debit Amount - Stop                

        //        if (!FormHelp.IsBlank(sqlCmd))
        //        {
        //            IDataReader loReader = _DatabaseFactory.ExecuteReader(Helper.GetDataBaseType,
        //                DataBaseConnectionString, sqlCmd).CreateDataReader();

        //            if (loReader.Read())
        //            {
        //                Amount = GetValue<double>(loReader, "safs_tutamt");
        //                FeeBase = GetValue<string>(loReader, "safd_feebaseon");

        //                if (FeeBase == "1")
        //                {
        //                    TuitionFee = Convert.ToDecimal(Amount) * Convert.ToDecimal(CrdtHr);
        //                }
        //                if (FeeBase == "0")
        //                {
        //                    TuitionFee = Convert.ToDecimal(Amount);
        //                }
        //            }
        //            loReader.Close();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //    return TuitionFee;
        //}
        public decimal GetTuitionFee(string Intake, string Program, double CrdtHr, string Matricno)
        {
            //variable declarations
            decimal TuitionFee = 0;
            double Amount = 0;
            double NonLocalAmount = 0;
            string category = "";
            string FeeBase = "";

            try
            {
                //build sql statement Debit Amount - Start
                //string sqlCmd = "select safs_tutamt,safd_feebaseon,safs_nonlocaltutamt from sas_feestruct where sast_code = " + MaxGeneric.clsGeneric.AddQuotes(Intake) +
                //                " and sabp_code in (SELECT sabp_code FROM sas_program WHERE sapg_code = " + MaxGeneric.clsGeneric.AddQuotes(Program) + ")";
                //build sql statement Debit Amount - Stop          
                //added by farid 19072016
                string sqlCmd1 = "select sasc_code from sas_student where sasi_matricno = " + MaxGeneric.clsGeneric.AddQuotes(Matricno);

                if (!FormHelp.IsBlank(sqlCmd1))
                {
                    IDataReader loReader = _DatabaseFactory.ExecuteReader(Helper.GetDataBaseType,
                            DataBaseConnectionString, sqlCmd1).CreateDataReader();

                    if (loReader.Read())
                    {
                        category = GetValue<string>(loReader, "sasc_code");
                    }
                    loReader.Close();
                }
                //end added farid

                //modified Mona 19/7/2016
                string sqlCmd = "select sas_feestramount.safa_amount, sas_feestruct.safd_feebaseon from sas_feestruct inner join sas_feestramount" +
                                " on sas_feestramount.safs_code = sas_feestruct.safs_code where sas_feestruct.sast_code = " + MaxGeneric.clsGeneric.AddQuotes(Intake) +
                                " and sas_feestruct.sabp_code in (SELECT sabp_code FROM sas_program WHERE sapg_code = " + MaxGeneric.clsGeneric.AddQuotes(Program) +
                                ") and sas_feestramount.safs_code in (select safs_code from sas_feestruct where sast_code = " + MaxGeneric.clsGeneric.AddQuotes(Intake) +
                                " and sabp_code in (SELECT sabp_code FROM sas_program WHERE sapg_code = " + MaxGeneric.clsGeneric.AddQuotes(Program) +
                                ")) and sas_feestramount.safd_type = 'T' and sas_feestramount.sasc_code = " + MaxGeneric.clsGeneric.AddQuotes(category);

                if (!FormHelp.IsBlank(sqlCmd))
                {
                    IDataReader loReader = _DatabaseFactory.ExecuteReader(Helper.GetDataBaseType,
                        DataBaseConnectionString, sqlCmd).CreateDataReader();

                    if (loReader.Read())
                    {
                        //Amount = GetValue<double>(loReader, "safs_tutamt");
                        Amount = GetValue<double>(loReader, "safa_amount");
                        FeeBase = GetValue<string>(loReader, "safd_feebaseon");
                        //NonLocalAmount = GetValue<double>(loReader, "safs_nonlocaltutamt");
                        if (FeeBase == "1")
                        {
                            //if (category == "W")
                            //{
                            //    TuitionFee = Convert.ToDecimal(Amount) * Convert.ToDecimal(CrdtHr);
                            //}
                            //else
                            //{
                            //    TuitionFee = Convert.ToDecimal(NonLocalAmount) * Convert.ToDecimal(CrdtHr);
                            //}

                            TuitionFee = Convert.ToDecimal(Amount) * Convert.ToDecimal(CrdtHr);
                        }
                        if (FeeBase == "0")
                        {
                            //if (category == "W")
                            //{
                            //    TuitionFee = Convert.ToDecimal(Amount);
                            //}
                            //else
                            //{
                            //    TuitionFee = Convert.ToDecimal(NonLocalAmount);
                            //}
                            TuitionFee = Convert.ToDecimal(Amount);
                        }
                    }
                    loReader.Close();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return TuitionFee;
        }

        #endregion

        #region GetStudentDetailsBySponsorAllocationWithStuValidity

        public StudentEn GetStudentDetailsBySponsorAllocationWithStuValidity(StudentEn argEn)
        {
            StudentEn loItem = new StudentEn();

            //string sqlCmd = "select Distinct A.SASI_MatricNo, SASI_Name, A.SASI_PgId, A.SASI_CurSem,B.SASS_Sponsor, B. sass_limit " +
            //                "from SAS_student A INNER JOIN SAS_StudentSpon B ON A.SASI_MAtricNO = B.SASI_MAtricNO " +
            //                "INNER JOIN SAS_StudentCategoryAccess ON A.SASS_Code = SAS_StudentCategoryAccess.SASC_Code where " +
            //                "B.SASS_Sponsor = " + clsGeneric.AddQuotes(Sponsor) + " and A.SASI_PgId = " + clsGeneric.AddQuotes(programID);

            //string sqlCmd = "Select R.SASI_MatricNo, R.SASI_Name, R.SASI_PgId, R.SASI_CurSem,R.SASS_Sponsor, R.sass_limit,sum(TransAmount) AllocatedAmount, " +
            //                "(R.sass_limit - Sum(Transamount)) AvailableAmont From ( " +
            //                "select Distinct A.SASI_MatricNo, SASI_Name, A.SASI_PgId, A.SASI_CurSem,B.SASS_Sponsor, B. sass_limit " +
            //                "from SAS_student A INNER JOIN SAS_StudentSpon B ON A.SASI_MAtricNO = B.SASI_MAtricNO " +
            //                "INNER JOIN SAS_StudentCategoryAccess ON A.SASS_Code = SAS_StudentCategoryAccess.SASC_Code where " +
            //                "B.SASS_Sponsor = " + clsGeneric.AddQuotes(Sponsor) + " and A.SASI_PgId = " + clsGeneric.AddQuotes(programID) +
            //                " and A.SASI_Reg_Status =" + Helper.StuRegistered
            //                + " and A.sass_code =" + clsGeneric.AddQuotes(Helper.StuActive) +
            //                " and CAST(B.SASS_SDATE  as DATE) <= " + clsGeneric.AddQuotes(Helper.DateConversion(DateTime.Today))
            //                + " and CAST(B.SASS_EDATE  as DATE) >= " + clsGeneric.AddQuotes(Helper.DateConversion(DateTime.Today)) +
            //                ") R LEFT JOIN SAS_SponsorInvoice D ON R.SASI_MatricNo = D.creditref " +
            //                "Group By R.SASI_MatricNo, R.SASI_Name, R.SASI_PgId, R.SASI_CurSem,R.SASS_Sponsor, R.sass_limit order by R.SASI_MatricNo ";

            string sqlCmd = @"
                      
                        select Distinct  stu.SASI_MatricNo, SASI_Name,  stu.SASI_PgId,  stu.sasi_intake,  stu.SASI_CurSem, sspon.SASS_Sponsor, sspon.sass_limit, stu.sasc_code, sspon.sass_type,
                         ( SELECT CASE WHEN SUM(acc.transamount) IS NULL THEN 0
                        ELSE SUM(acc.transamount) END 
                        FROM   SAS_SponsorInvoice D  
	                            inner join sas_sponsor_inv_rec sir on sir.invoice_id = D.batchcode 
	                            inner join sas_accounts accRec on sir.receipt_id = accRec.transid and accRec.creditRef = D.creditRef1 
	                            inner join sas_accounts accAllocation on accAllocation.creditref = D.creditref1 and accRec.BatchCode = accAllocation.creditref1 
	                            inner join sas_accounts acc on acc.creditref1 = d.creditref1 and d.creditref = acc.creditref and accAllocation.BatchCode = acc.BatchCode 
	                            where acc.poststatus = 'Posted' and acc.transstatus = 'Closed' and acc.category ='SPA' and stu.SASI_MatricNo = D.creditref  
                        )AS  AllocatedAmount, 
                        (select case when sspon.sass_limit = 0 THEN 0
                        ELSE
                            (sspon.sass_limit - (SELECT CASE WHEN SUM(acc.transamount) IS NULL THEN 0
                        ELSE SUM(acc.transamount) END 
                        FROM   SAS_SponsorInvoice D  
	                            inner join sas_sponsor_inv_rec sir on sir.invoice_id = D.batchcode 
	                            inner join sas_accounts accRec on sir.receipt_id = accRec.transid and accRec.creditRef = D.creditRef1 
	                            inner join sas_accounts accAllocation on accAllocation.creditref = D.creditref1 and accRec.BatchCode = accAllocation.creditref1 
	                            inner join sas_accounts acc on acc.creditref1 = d.creditref1 and d.creditref = acc.creditref and accAllocation.BatchCode = acc.BatchCode 
	                            where acc.poststatus = 'Posted' and acc.transstatus = 'Closed' and acc.category ='SPA' and stu.SASI_MatricNo = D.creditref  ))
                        END) AS AvailableAmont
                        from SAS_student stu
                        INNER JOIN SAS_StudentSpon sspon ON  stu.SASI_MAtricNO = sspon.SASI_MAtricNO 
                        INNER JOIN SAS_StudentCategoryAccess ON  stu.SASS_Code = SAS_StudentCategoryAccess.SASC_Code                         
                        left join sas_program prog on prog.sapg_code = stu.sasi_pgid
                        left join sas_feestruct fs on  stu.sasi_intake = fs.sast_code and prog.sabp_code = fs.sabp_code 
                        left join sas_semestersetup sem on fs.sast_code = sem.sast_code and sem.sast_code = stu.sasi_intake
                        left join sas_feestrdetails fsd on fs.safs_code = fsd.safs_code and
                        (CASE stu.sasi_cursem
                        WHEN 1 THEN 
                        (fsd.safd_type = 'A' and fsd.safd_feefor = '0' and fsd.safd_sem = 0 )
                        or ( fsd.safd_feefor = '0' and  fsd.safd_type = 'S' and 
                        fsd.saft_code not in (select saft_code from sas_feestrdetails where safs_code =fs.safs_code and safd_type = 'A' and safd_feefor = '0' and safd_sem = 0  ))
                        or (fsd.safd_sem = 1 and fsd.safd_type = 'S' and fsd.safd_feefor = '1' and
                        fsd.saft_code not in (select saft_code from sas_feestrdetails where safs_code =fs.safs_code and safd_type = 'S' and safd_feefor = '0' ) 
                        and fsd.saft_code not in (select saft_code from sas_feestrdetails where safs_code =fs.safs_code and safd_type = 'A' and safd_feefor = '0' and safd_sem = 0  ))

                        else
                        ( fsd.safd_feefor = '0' and  fsd.safd_type = 'S' )
                        or (fsd.safd_sem = stu.sasi_cursem and fsd.safd_type = 'S' and fsd.safd_feefor = '1' and
                        fsd.saft_code not in (select saft_code from sas_feestrdetails where safs_code =fs.safs_code and safd_type = 'S' and safd_feefor = '0' ) )
                        END)
                        left join sas_feestramount fsa on fs.safs_code = fsa.safs_code and stu.sasc_code = fsa.sasc_code  and fsd.saft_code = fsa.saft_code
                        and fsd.saft_code = fsa.saft_code and fsa.safd_feefor = fsd.safd_feefor and fsa.safd_sem = fsd.safd_sem and fsa.safd_type = fsd.safd_type
                        left join sas_feetypes ft on ft.saft_code = fsa.saft_code and fsd.saft_code = fsa.saft_code    
                        
                       where stu.SASI_MatricNo = " + clsGeneric.AddQuotes(argEn.MatricNo) +
                        " and stu.SASI_Reg_Status =" + Helper.StuRegistered +
                        " and stu.sass_code =" + clsGeneric.AddQuotes(Helper.StuActive) +
                        " and TO_DATE( sspon.SASS_sDATE,'DD/MM/YYYY') <= current_date and TO_DATE( sspon.SASS_EDATE,'DD/MM/YYYY') >= current_date " +
                //    " and TO_DATE( B.SASS_sDATE,'DD/MM/YYYY') <= current_date and  TO_DATE( B.SASS_EDATE,'YYYY/MM/DD') >= current_date " +
                        " Group By stu.SASI_MatricNo, stu.SASI_Name, stu.SASI_PgId, stu.sasi_intake, stu.SASI_CurSem, sspon.SASS_Sponsor, sspon.sass_limit, stu.sasc_code, sspon.sass_type order by stu.SASI_MatricNo ";
            //inner join sas_sponsorfeetypes sft on B.sass_sponsor = sft.sasr_code and ft.saft_code = sft.saft_code
            //sqlCmd = sqlCmd + " order by A.SASI_MatricNo";

            try
            {
                if (!FormHelp.IsBlank(sqlCmd))
                {
                    using (IDataReader loReader = _DatabaseFactory.ExecuteReader(Helper.GetDataBaseType,
                        DataBaseConnectionString, sqlCmd).CreateDataReader())
                    {
                        while (loReader.Read())
                        {
                            //StudentEn loItem = LoadObjectForSem(loReader);
                            loItem.MatricNo = GetValue<string>(loReader, "SASI_MatricNo");
                            loItem.StudentName = GetValue<string>(loReader, "SASI_Name");
                            loItem.ProgramID = GetValue<string>(loReader, "SASI_PgId");
                            loItem.CurrentSemester = GetValue<int>(loReader, "SASI_CurSem");
                            loItem.SponsorLimit = GetValue<double>(loReader, "sass_limit");
                            loItem.PaidAmount = GetValue<double>(loReader, "AllocatedAmount");
                            loItem.OutstandingAmount = GetValue<double>(loReader, "AvailableAmont");
                            loItem.CategoryCode = GetValue<string>(loReader, "sasc_code");
                            loItem.FullySponsor = GetValue<Boolean>(loReader, "sass_type");
                            //loEnList.Add(loItem);
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

        #region GetAFCDetailByStudent

        public DataSet GetAFCDetailByStudent(string MatricNo, string ProgId, string Intake, string StudentCategory, int CurrentSem, double CdtHrs)
        {
            //variable declarations
            string SqlStatement = null;

            try
            {
                SqlStatement = "SELECT SAS_FeeStrAmount.SAFS_Code, SAS_FeeStrAmount.SAFA_Amount, CASE WHEN SAS_FeeStrAmount.SAFD_Type = 'A' THEN 'Admission Fee' END As SAFD_Type," +
                                " SAS_FeeStrAmount.SAFT_Code,SAS_FeeStruct.sast_code,SAS_FeeStrAmount.safd_sem FROM SAS_FeeStrAmount" +
                                " INNER JOIN SAS_FeeStrDetails ON SAS_FeeStrAmount.SAFS_Code = SAS_FeeStrDetails.SAFS_Code INNER JOIN SAS_FeeStruct ON" +
                                " SAS_FeeStrDetails.SAFS_Code = SAS_FeeStruct.SAFS_Code INNER JOIN SAS_FeeTypes ON SAS_FeeStrDetails.SAFT_Code = SAS_FeeTypes.SAFT_Code" +
                                " WHERE (SAS_FeeStrAmount.SASC_Code = " + clsGeneric.AddQuotes(StudentCategory) + ") " +
                                " AND SAS_FeeStruct.sabp_code IN (SELECT sabp_code FROM sas_program WHERE sapg_code = " + clsGeneric.AddQuotes(ProgId) + ") and SAS_FeeStruct.sast_code = " + clsGeneric.AddQuotes(Intake) +
                                " AND SAS_FeeStrAmount.SAFD_Type = 'A'" +
                                " GROUP BY SAS_FeeStrAmount.SAFS_Code, SAS_FeeStrAmount.SAFA_Amount, SAS_FeeStrAmount.SAFD_Type,SAS_FeeStrAmount.SAFT_Code,SAS_FeeStruct.sast_code,SAS_FeeStrAmount.safd_sem" + 
                                " UNION " +
                                "SELECT SAS_FeeStrAmount.SAFS_Code, SAS_FeeStrAmount.SAFA_Amount, CASE WHEN SAS_FeeStrAmount.SAFD_Type = 'S' THEN 'Semester Fee' END As SAFD_Type," +
                                " SAS_FeeStrAmount.SAFT_Code,SAS_FeeStruct.sast_code,SAS_FeeStrAmount.safd_sem FROM SAS_FeeStrAmount" +
                                " INNER JOIN SAS_FeeStrDetails ON SAS_FeeStrAmount.SAFS_Code = SAS_FeeStrDetails.SAFS_Code INNER JOIN SAS_FeeStruct ON" +
                                " SAS_FeeStrDetails.SAFS_Code = SAS_FeeStruct.SAFS_Code INNER JOIN SAS_FeeTypes ON SAS_FeeStrDetails.SAFT_Code = SAS_FeeTypes.SAFT_Code" +
                                " WHERE (SAS_FeeStrAmount.SASC_Code = " + clsGeneric.AddQuotes(StudentCategory) + ") and SAS_FeeStruct.sabp_code IN (SELECT sabp_code FROM sas_program WHERE sapg_code = " + clsGeneric.AddQuotes(ProgId) + ")" +
                                " AND SAS_FeeStruct.sast_code = " + clsGeneric.AddQuotes(Intake) + " AND SAS_FeeStrAmount.SAFD_Type = 'S' AND SAS_FeeStrAmount.safd_sem IN (0," + clsGeneric.NullToInteger(CurrentSem) + ")" +
                                " GROUP BY SAS_FeeStrAmount.SAFS_Code, SAS_FeeStrAmount.SAFA_Amount, SAS_FeeStrAmount.SAFD_Type,SAS_FeeStrAmount.SAFT_Code,SAS_FeeStruct.sast_code,SAS_FeeStrAmount.safd_sem" +
                                " UNION " +
                                "SELECT SAS_FeeStrAmount.SAFS_Code, CASE WHEN SAS_FeeStruct.safd_feebaseon = '1' THEN (sas_feestruct.safs_tutamt * " + clsGeneric.NullToDecimal(Convert.ToDecimal(CdtHrs)) + ")" +
                                " WHEN SAS_FeeStruct.safd_feebaseon = '0' THEN sas_feestruct.safs_tutamt END AS SAFA_Amount," +
                                " CASE WHEN SAS_FeeStrAmount.SAFD_Type = 'T' THEN 'Tuition Fee' END AS SAFD_Type,SAS_FeeStrAmount.SAFT_Code,SAS_FeeStruct.sast_code,CAST(SAS_FeeStruct.safd_feebaseon AS int) as safd_sem FROM SAS_FeeStrAmount" +
                                " INNER JOIN SAS_FeeStrDetails ON SAS_FeeStrAmount.SAFS_Code = SAS_FeeStrDetails.SAFS_Code INNER JOIN SAS_FeeStruct ON" +
                                " SAS_FeeStrDetails.SAFS_Code = SAS_FeeStruct.SAFS_Code INNER JOIN SAS_FeeTypes ON SAS_FeeStrDetails.SAFT_Code = SAS_FeeTypes.SAFT_Code" +
                                " WHERE (SAS_FeeStrAmount.SASC_Code = " + clsGeneric.AddQuotes(StudentCategory) + ") and SAS_FeeStruct.sabp_code IN (SELECT sabp_code FROM sas_program WHERE sapg_code = " + clsGeneric.AddQuotes(ProgId) + ")" +
                                " and SAS_FeeStruct.sast_code = " + clsGeneric.AddQuotes(Intake) + " AND SAS_FeeStrAmount.SAFD_Type = 'T'" +
                                " GROUP BY SAS_FeeStrAmount.SAFS_Code, sas_feestruct.safs_tutamt, SAS_FeeStrAmount.SAFD_Type,SAS_FeeStrAmount.SAFT_Code,SAS_FeeStruct.sast_code,SAS_FeeStruct.safd_feebaseon" + 
                                " UNION " +
                                "SELECT '' as SAFS_Code, (SKD.SAKOD_FeeAmount * SK.SAKO_CreditHours) as SAFA_Amount,'Kokurikulum Fee' as SAFD_Type,SK.SAKO_Code As SAFT_Code," +
                                " '' as sast_code,null as safd_sem from SAS_Student SS,SAS_Kokorikulum SK,SAS_KokorikulumDetails SKD where SASI_MatricNo = " + clsGeneric.AddQuotes(MatricNo) + " and SASI_KokoCode <> '-1'" + 
                                " and SS.SASI_FeeCat = SKD.SAKOD_CategoryCode and SS.SASI_KokoCode = SK.SAKO_Code and SKD.SAKO_Code = SK.SAKO_Code" +
                                " UNION " +
                                "SELECT SA.sahs_code as SAFS_Code, SUM(SAHA_Amount) AS SAFA_Amount,'Hostel Fee' as SAFD_Type,SA.SAFT_Code, '' as sast_code,null as safd_sem" + 
                                " from SAS_Student SS,SAS_HostelStruct SH,SAS_HostelStrAmount SA, SAS_FeeTypes SF where SASI_Hostel = true and SASI_MatricNo = " + clsGeneric.AddQuotes(MatricNo) +
                                " and (SS.SAKO_Code = SH.SAHB_Code and SS.SABK_Code = SH.SAHB_Block) and SH.SAHS_Code = SA.SAHS_Code and SS.SASI_FeeCat = SA.SASC_Code" +
                                " and SF.SAFT_Code = SA.SAFT_Code and  sasi_hostel = true" +
                                " GROUP BY SA.sahs_code,SA.SAFT_Code ORDER BY SAFD_Type";

            if (!FormHelp.IsBlank(SqlStatement))
                {
                    //Binding Fee Details - start
                    DataSet _DataSet = _DatabaseFactory.ExecuteDataSet(Helper.GetDataBaseType,
                        DataBaseConnectionString, SqlStatement);
                    //Sas Fee details - Ended
                    return _DataSet;
                }

            }
            catch (Exception ex)
            {
                MaxModule.Helper.LogError(ex.Message);

            }
            return null;
        }

        #endregion

        #region GetKokoDetails

        //<summary>
        //Method to Get List of  Active or Inactive StudentCategory
        //</summary>
        //<param name="argEn">StudentCategory Entity as an Input.StudentCategoryCode,Description and Status as Input Properties.</param>
        //<returns>Returns List of StudentCategory</returns>
        public List<KokoEn> GetKokoDetails(KokoEn argEn)
        {
            List<KokoEn> loEnList = new List<KokoEn>();
            
            string sqlCmd = "select distinct kk.sako_code , kk.sako_description, kk.sako_status,kk.saft_taxmode," +
            " local.sakod_feeamount as sakodfeeamountlocalin,local.sakod_gstin as sakodgstamountlocalin, local.sakod_feeamountout as sakodfeeamountlocalout," +
            " local.sakod_gstout as sakodfeegstamountlocalout, nonlocal.sakod_feeamount as sakodfeeamountinterin, nonlocal.sakod_gstin as sakodgstamountinterin, " +
            " nonlocal.sakod_feeamountout as sakodfeeamountinterout,nonlocal.sakod_gstout as sakodgstamountinterout, local.sakod_categorycode as LocalCategory," +
            " nonlocal.sakod_categorycode as NonLocalCategory,local.saft_code as saft_code from  sas_kokorikulum kk inner join sas_feetypes ft on ft.saft_code = saft_code left join (select saft_code,sako_code,sakod_feeamount,  sakod_feeamountout, sakod_gstin, sakod_gstout, " +
            " safd_type,sakod_categorycode from sas_kokorikulumdetails where sas_kokorikulumdetails.sakod_categorycode in ('W')  )as local on local.sako_code = kk.sako_code and local.saft_code = ft.saft_code" +
            " left join (select saft_code,sako_code,sakod_feeamount,  sakod_feeamountout, sakod_gstin, sakod_gstout,safd_type, sakod_categorycode from sas_kokorikulumdetails " +
            " where sas_kokorikulumdetails.sakod_categorycode in ('BW')  )as nonlocal on nonlocal.sako_code = kk.sako_code and nonlocal.saft_code = ft.saft_code WHERE kk.sako_code = " + clsGeneric.AddQuotes(argEn.Code);
            sqlCmd = sqlCmd + " order by saft_code";

            try
            {
                if (!FormHelp.IsBlank(sqlCmd))
                {
                    using (IDataReader loReader = _DatabaseFactory.ExecuteReader(Helper.GetDataBaseType,
                        DataBaseConnectionString, sqlCmd).CreateDataReader())
                    {
                        while (loReader.Read())
                        {
                            KokoEn loItem = new KokoEn();
                            loItem.Code = GetValue<string>(loReader, "sako_code");
                            loItem.Saftcode = GetValue<string>(loReader, "saft_code");
                            loItem.LocalCategory = GetValue<string>(loReader, "LocalCategory");
                            loItem.NonLocalCategory = GetValue<string>(loReader, "NonLocalCategory");
                            loItem.sakodfeeamountlocalin = GetValue<double>(loReader, "sakodfeeamountlocalin");
                            loItem.sakodfeeamountlocalout = GetValue<double>(loReader, "sakodfeeamountlocalout");
                            loItem.sakodgstamountlocalin = GetValue<double>(loReader, "sakodgstamountlocalin");
                            loItem.sakodfeegstamountlocalout = GetValue<double>(loReader, "sakodfeegstamountlocalout");
                            loItem.sakodfeeamountinterin = GetValue<double>(loReader, "sakodfeeamountinterin");
                            loItem.sakodfeeamountinterout = GetValue<double>(loReader, "sakodfeeamountinterout");
                            loItem.sakodgstamountinterin = GetValue<double>(loReader, "sakodgstamountinterin");
                            loItem.sakodgstamountinterout = GetValue<double>(loReader, "sakodgstamountinterout");
                            //loItem.Category = GetValue<string>(loReader, "sakod_categorycode");
                            //loItem.Saftcode = GetValue<string>(loReader, "saft_code");
                            //loItem.Code = GetValue<string>(loReader, "sako_code");
                            //if (loItem.Category == "W")
                            //{
                            //    loItem.LocalCategory = GetValue<string>(loReader, "sakod_categorycode");

                            //    loItem.sakod_idkoko = GetValue<int>(loReader, "sakod_id");
                            //    loItem.sakodfeeamountlocalin = GetValue<double>(loReader, "SAKOD_FeeAmount");
                            //    loItem.sakodfeeamountlocalout = GetValue<double>(loReader, "sakod_feeamountout");
                            //    loItem.sakodgstamountlocalin = GetValue<double>(loReader, "sakod_gstin");
                            //    loItem.sakodfeegstamountlocalout = GetValue<double>(loReader, "sakod_gstout");
                            //    loItem.categoryname = GetValue<string>(loReader, "sakod_categoryname");

                            //}
                            //else if (loItem.Category == "BW")
                            //{
                            //    loItem.NonLocalCategory = GetValue<string>(loReader, "sakod_categorycode");

                            //    loItem.sakod_idkoko = GetValue<int>(loReader, "sakod_id");
                            //    loItem.sakodfeeamountinterin = GetValue<double>(loReader, "SAKOD_FeeAmount");
                            //    loItem.sakodfeeamountinterout = GetValue<double>(loReader, "sakod_feeamountout");
                            //    loItem.sakodgstamountinterin = GetValue<double>(loReader, "sakod_gstin");
                            //    loItem.sakodgstamountinterout = GetValue<double>(loReader, "sakod_gstout");
                            //    loItem.categoryname = GetValue<string>(loReader, "sakod_categoryname");

                            //}
                            if (loItem.Saftcode != null)
                            {
                                loEnList.Add(loItem);
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
            return loEnList;
        }

        #endregion

        #region GetListOutstandingAmtAllStud
        //added by Hafiz @ 26/4/2016
        //modified by Hafiz @ 27/5/2016
        //will be use to retrived outstanding amount by Student Dunning Letter

        public List<StudentEn> GetListOutstandingAmtAllStud(StudentEn argEn)
        {
            string sqlCmd;
            List<StudentEn> loEnList = new List<StudentEn>();

            argEn.ProgramID = argEn.ProgramID.Replace("*", "%");
            argEn.CurretSemesterYear = argEn.CurretSemesterYear.Replace("*", "%");

            sqlCmd = @"select 
	                    SS.SASI_MatricNo ,
	                    SS.SASI_Name ,
	                    SS.SASI_PgId ,
	                    SS.SASI_CurSem ,
	                    SS.SASI_CurSemYr ,
	                    SS.SASI_Email,
	                    SO.SASO_Outstandingamt,
	                    SS.SASI_Add1,
	                    SS.SASI_Add2,
	                    SS.SASI_Add3,
	                    SS.SASI_City,
	                    SS.SASI_State,
	                    SS.SASI_Postcode
                    from sas_student ss
                    inner join sas_studentoutstanding so ON SS.SASI_MatricNo = SO.SASI_MatricNo
                    where SO.SASO_Outstandingamt > 0 ";

            if (argEn.CurretSemesterYear.Length != 0) sqlCmd = sqlCmd + " AND ss.SASI_CurSemYr = '" + argEn.CurretSemesterYear + "'";
            if (argEn.ProgramID.Length != 0) sqlCmd = sqlCmd + " and ss.SASI_PgId = '" + argEn.ProgramID + "'";

            sqlCmd = sqlCmd + @" order by ss.SASI_MatricNo";

            try
            {
                if (!FormHelp.IsBlank(sqlCmd))
                {
                    using (IDataReader loReader = _DatabaseFactory.ExecuteReader(Helper.GetDataBaseType,
                        DataBaseConnectionString, sqlCmd).CreateDataReader())
                    {
                        while (loReader.Read())
                        {
                            StudentEn loItem = new StudentEn();
                            loItem.MatricNo = GetValue<string>(loReader, "SASI_MatricNo");
                            loItem.StudentName = GetValue<string>(loReader, "SASI_Name");
                            loItem.ProgramID = GetValue<string>(loReader, "SASI_PgId");
                            loItem.CurrentSemester = GetValue<int>(loReader, "SASI_CurSem");
                            loItem.SASI_Email = GetValue<string>(loReader, "SASI_Email");
                            loItem.OutstandingAmount = GetValue<double>(loReader, "SASO_Outstandingamt");
                            loItem.SASI_Add1 = GetValue<string>(loReader, "SASI_Add1");
                            loItem.SASI_Add2 = GetValue<string>(loReader, "SASI_Add2");
                            loItem.SASI_Add3 = GetValue<string>(loReader, "SASI_Add3");
                            loItem.SASI_City = GetValue<string>(loReader, "SASI_City");
                            loItem.SASI_State = GetValue<string>(loReader, "SASI_State");
                            loItem.SASI_Postcode = GetValue<string>(loReader, "SASI_Postcode");
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

        #region GetStudentDetailsBySponsorWithoutStuValidity

        public List<StudentEn> GetStudentDetailsBySponsorWithoutStuValidity(string Sponsor, string MatricNo)
        {
            List<StudentEn> loEnList = new List<StudentEn>();
            MatricNo = MatricNo.Replace("*", "%");
            //string sqlCmd = "select Distinct A.SASI_MatricNo, SASI_Name, A.SASI_PgId, A.SASI_CurSem,B.SASS_Sponsor, B. sass_limit " +
            //                "from SAS_student A INNER JOIN SAS_StudentSpon B ON A.SASI_MAtricNO = B.SASI_MAtricNO " +
            //                "INNER JOIN SAS_StudentCategoryAccess ON A.SASS_Code = SAS_StudentCategoryAccess.SASC_Code where " +
            //                "B.SASS_Sponsor = " + clsGeneric.AddQuotes(Sponsor) + " and A.SASI_PgId = " + clsGeneric.AddQuotes(programID);

            //string sqlCmd = "Select R.SASI_MatricNo, R.SASI_Name, R.SASI_PgId, R.SASI_CurSem,R.SASS_Sponsor, R.sass_limit,sum(TransAmount) AllocatedAmount, " +
            //                "(R.sass_limit - Sum(Transamount)) AvailableAmont From ( " +
            //                "select Distinct A.SASI_MatricNo, SASI_Name, A.SASI_PgId, A.SASI_CurSem,B.SASS_Sponsor, B. sass_limit " +
            //                "from SAS_student A INNER JOIN SAS_StudentSpon B ON A.SASI_MAtricNO = B.SASI_MAtricNO " +
            //                "INNER JOIN SAS_StudentCategoryAccess ON A.SASS_Code = SAS_StudentCategoryAccess.SASC_Code where " +
            //                "B.SASS_Sponsor = " + clsGeneric.AddQuotes(Sponsor) + " and A.SASI_PgId = " + clsGeneric.AddQuotes(programID) +
            //                " and A.SASI_Reg_Status =" + Helper.StuRegistered
            //                + " and A.sass_code =" + clsGeneric.AddQuotes(Helper.StuActive) +
            //                " and CAST(B.SASS_SDATE  as DATE) <= " + clsGeneric.AddQuotes(Helper.DateConversion(DateTime.Today))
            //                + " and CAST(B.SASS_EDATE  as DATE) >= " + clsGeneric.AddQuotes(Helper.DateConversion(DateTime.Today)) +
            //                ") R LEFT JOIN SAS_SponsorInvoice D ON R.SASI_MatricNo = D.creditref " +
            //                "Group By R.SASI_MatricNo, R.SASI_Name, R.SASI_PgId, R.SASI_CurSem,R.SASS_Sponsor, R.sass_limit order by R.SASI_MatricNo ";

            string sqlCmd = @"
                      
                        select Distinct  stu.SASI_MatricNo, SASI_Name,  stu.SASI_PgId,  stu.sasi_intake,  stu.SASI_CurSem, sspon.SASS_Sponsor, sspon.sass_limit, stu.sasc_code, sspon.sass_type,
                         ( SELECT CASE WHEN SUM(acc.transamount) IS NULL THEN 0
                        ELSE SUM(acc.transamount) END 
                        FROM   SAS_SponsorInvoice D  
	                            inner join sas_sponsor_inv_rec sir on sir.invoice_id = D.batchcode 
	                            inner join sas_accounts accRec on sir.receipt_id = accRec.transid and accRec.creditRef = D.creditRef1 
	                            inner join sas_accounts accAllocation on accAllocation.creditref = D.creditref1 and accRec.BatchCode = accAllocation.creditref1 
	                            inner join sas_accounts acc on acc.creditref1 = d.creditref1 and d.creditref = acc.creditref and accAllocation.BatchCode = acc.BatchCode 
	                            where acc.poststatus = 'Posted' and acc.transstatus = 'Closed' and acc.category ='SPA' and stu.SASI_MatricNo = D.creditref  
                        )AS  AllocatedAmount, 
                        (select case when sspon.sass_limit = 0 THEN 0
                        ELSE
                            (sspon.sass_limit - (SELECT CASE WHEN SUM(acc.transamount) IS NULL THEN 0
                        ELSE SUM(acc.transamount) END 
                        FROM   SAS_SponsorInvoice D  
	                            inner join sas_sponsor_inv_rec sir on sir.invoice_id = D.batchcode 
	                            inner join sas_accounts accRec on sir.receipt_id = accRec.transid and accRec.creditRef = D.creditRef1 
	                            inner join sas_accounts accAllocation on accAllocation.creditref = D.creditref1 and accRec.BatchCode = accAllocation.creditref1 
	                            inner join sas_accounts acc on acc.creditref1 = d.creditref1 and d.creditref = acc.creditref and accAllocation.BatchCode = acc.BatchCode 
	                            where acc.poststatus = 'Posted' and acc.transstatus = 'Closed' and acc.category ='SPA' and stu.SASI_MatricNo = D.creditref  ))
                        END) AS AvailableAmont
                        from SAS_student stu
                        INNER JOIN SAS_StudentSpon sspon ON  stu.SASI_MAtricNO = sspon.SASI_MAtricNO 
                                                
                        left join sas_program prog on prog.sapg_code = stu.sasi_pgid
                        left join sas_feestruct fs on  stu.sasi_intake = fs.sast_code and prog.sabp_code = fs.sabp_code 
                        left join sas_semestersetup sem on fs.sast_code = sem.sast_code and sem.sast_code = stu.sasi_intake
                        left join sas_feestrdetails fsd on fs.safs_code = fsd.safs_code and
                        (CASE stu.sasi_cursem
                        WHEN 1 THEN 
                        (fsd.safd_type = 'A' and fsd.safd_feefor = '0' and fsd.safd_sem = 0 )
                        or ( fsd.safd_feefor = '0' and  fsd.safd_type = 'S' and 
                        fsd.saft_code not in (select saft_code from sas_feestrdetails where safs_code =fs.safs_code and safd_type = 'A' and safd_feefor = '0' and safd_sem = 0  ))
                        or (fsd.safd_sem = 1 and fsd.safd_type = 'S' and fsd.safd_feefor = '1' and
                        fsd.saft_code not in (select saft_code from sas_feestrdetails where safs_code =fs.safs_code and safd_type = 'S' and safd_feefor = '0' ) 
                        and fsd.saft_code not in (select saft_code from sas_feestrdetails where safs_code =fs.safs_code and safd_type = 'A' and safd_feefor = '0' and safd_sem = 0  ))

                        else
                        ( fsd.safd_feefor = '0' and  fsd.safd_type = 'S' )
                        or (fsd.safd_sem = stu.sasi_cursem and fsd.safd_type = 'S' and fsd.safd_feefor = '1' and
                        fsd.saft_code not in (select saft_code from sas_feestrdetails where safs_code =fs.safs_code and safd_type = 'S' and safd_feefor = '0' ) )
                        END)
                        left join sas_feestramount fsa on fs.safs_code = fsa.safs_code and stu.sasc_code = fsa.sasc_code  and fsd.saft_code = fsa.saft_code
                        and fsd.saft_code = fsa.saft_code and fsa.safd_feefor = fsd.safd_feefor and fsa.safd_sem = fsd.safd_sem and fsa.safd_type = fsd.safd_type
                        left join sas_feetypes ft on ft.saft_code = fsa.saft_code and fsd.saft_code = fsa.saft_code    
                        
                       where sspon.SASS_Sponsor = " + clsGeneric.AddQuotes(Sponsor);
            if (MatricNo.Length != 0) sqlCmd = sqlCmd + " and stu.SASI_MatricNo like '%" + MatricNo + "%'";
            //" and TO_DATE( sspon.SASS_sDATE,'DD/MM/YYYY') <= current_date and TO_DATE( sspon.SASS_EDATE,'DD/MM/YYYY') >= current_date " +
            //    " and TO_DATE( B.SASS_sDATE,'DD/MM/YYYY') <= current_date and  TO_DATE( B.SASS_EDATE,'YYYY/MM/DD') >= current_date " +
            sqlCmd = sqlCmd + " Group By stu.SASI_MatricNo, stu.SASI_Name, stu.SASI_PgId, stu.sasi_intake, stu.SASI_CurSem, sspon.SASS_Sponsor, sspon.sass_limit, stu.sasc_code, sspon.sass_type order by stu.SASI_MatricNo ";
            //inner join sas_sponsorfeetypes sft on B.sass_sponsor = sft.sasr_code and ft.saft_code = sft.saft_code
            //sqlCmd = sqlCmd + " order by A.SASI_MatricNo";
            //commented INNER JOIN SAS_StudentCategoryAccess ON  stu.SASS_Code = SAS_StudentCategoryAccess.SASC_Code 30062016
            try
            {
                if (!FormHelp.IsBlank(sqlCmd))
                {
                    using (IDataReader loReader = _DatabaseFactory.ExecuteReader(Helper.GetDataBaseType,
                        DataBaseConnectionString, sqlCmd).CreateDataReader())
                    {
                        while (loReader.Read())
                        {
                            StudentEn loItem = LoadObjectForSem(loReader);
                            loItem.MatricNo = GetValue<string>(loReader, "SASI_MatricNo");
                            loItem.StudentName = GetValue<string>(loReader, "SASI_Name");
                            loItem.ProgramID = GetValue<string>(loReader, "SASI_PgId");
                            loItem.CurrentSemester = GetValue<int>(loReader, "SASI_CurSem");
                            loItem.SponsorLimit = GetValue<double>(loReader, "sass_limit");
                            loItem.PaidAmount = GetValue<double>(loReader, "AllocatedAmount");
                            loItem.OutstandingAmount = GetValue<double>(loReader, "AvailableAmont");
                            loItem.CategoryCode = GetValue<string>(loReader, "sasc_code");
                            loItem.FullySponsor = GetValue<Boolean>(loReader, "sass_type");
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

        #region GetStudentDetailsForSponsorInvoice

        public List<StudentEn> GetStudentDetailsForSponsorInvoice(string batchcode, string Sponsor)
        {
            List<StudentEn> loEnList = new List<StudentEn>();

            string sqlCmd = @"
                      
                        
                      
                        select Distinct  SAS_SponsorInvoice.transamount,SAS_SponsorInvoice.batchcode,stu.SASI_MatricNo, SASI_Name,  stu.SASI_PgId, prog.sapg_program,  stu.sasi_intake,  stu.SASI_CurSem, sspon.SASS_Sponsor, sspon.sass_limit, stu.sasc_code, sspon.sass_type,
                        ( SELECT CASE WHEN SUM(SAS_SponsorInvoice.transamount) IS NULL THEN 0
                        ELSE Sum(sas_sponsorinvoicedetails.transamount) end from sas_sponsorinvoicedetails where transid in (select transid from sas_sponsorinvoice where batchcode = " + clsGeneric.AddQuotes(batchcode) + 
                        @" and creditref = stu.SASI_MatricNo) )AS  TransactionAMT,                          
                        ( SELECT CASE WHEN SUM(acc.transamount) IS NULL THEN 0
                        ELSE SUM(acc.transamount) END 
                        FROM   SAS_SponsorInvoice D  
	                            inner join sas_sponsor_inv_rec sir on sir.invoice_id = D.batchcode 
	                            inner join sas_accounts accRec on sir.receipt_id = accRec.transid and accRec.creditRef = D.creditRef1 
	                            inner join sas_accounts accAllocation on accAllocation.creditref = D.creditref1 and accRec.BatchCode = accAllocation.creditref1 
	                            inner join sas_accounts acc on acc.creditref1 = d.creditref1 and d.creditref = acc.creditref and accAllocation.BatchCode = acc.BatchCode 
	                            where acc.poststatus = 'Posted' and acc.transstatus = 'Closed' and acc.category ='SPA' and stu.SASI_MatricNo = D.creditref  
                        )AS  AllocatedAmount, 
                        (select case when sspon.sass_limit = 0 THEN 0
                        ELSE
                            (sspon.sass_limit - (SELECT CASE WHEN SUM(acc.transamount) IS NULL THEN 0
                        ELSE SUM(acc.transamount) END 
                        FROM   SAS_SponsorInvoice D  
	                            inner join sas_sponsor_inv_rec sir on sir.invoice_id = D.batchcode 
	                            inner join sas_accounts accRec on sir.receipt_id = accRec.transid and accRec.creditRef = D.creditRef1 
	                            inner join sas_accounts accAllocation on accAllocation.creditref = D.creditref1 and accRec.BatchCode = accAllocation.creditref1 
	                            inner join sas_accounts acc on acc.creditref1 = d.creditref1 and d.creditref = acc.creditref and accAllocation.BatchCode = acc.BatchCode 
	                            where acc.poststatus = 'Posted' and acc.transstatus = 'Closed' and acc.category ='SPA' and stu.SASI_MatricNo = D.creditref  ))
                        END) AS AvailableAmont
                        from SAS_student stu
                        INNER JOIN SAS_StudentSpon sspon ON  stu.SASI_MAtricNO = sspon.SASI_MAtricNO 
                        INNER JOIN SAS_StudentCategoryAccess ON  stu.SASS_Code = SAS_StudentCategoryAccess.SASC_Code
                        Inner join SAS_SponsorInvoice ON SAS_SponsorInvoice.creditref = stu.SASI_MAtricNO                    
                        left join sas_program prog on prog.sapg_code = stu.sasi_pgid
                        left join sas_feestruct fs on  stu.sasi_intake = fs.sast_code and prog.sabp_code = fs.sabp_code 
                        left join sas_semestersetup sem on fs.sast_code = sem.sast_code and sem.sast_code = stu.sasi_intake
                        left join sas_feestrdetails fsd on fs.safs_code = fsd.safs_code and
                        (CASE stu.sasi_cursem
                        WHEN 1 THEN 
                        (fsd.safd_type = 'A' and fsd.safd_feefor = '0' and fsd.safd_sem = 0 )
                        or ( fsd.safd_feefor = '0' and  fsd.safd_type = 'S' and 
                        fsd.saft_code not in (select saft_code from sas_feestrdetails where safs_code =fs.safs_code and safd_type = 'A' and safd_feefor = '0' and safd_sem = 0  ))
                        or (fsd.safd_sem = 1 and fsd.safd_type = 'S' and fsd.safd_feefor = '1' and
                        fsd.saft_code not in (select saft_code from sas_feestrdetails where safs_code =fs.safs_code and safd_type = 'S' and safd_feefor = '0' ) 
                        and fsd.saft_code not in (select saft_code from sas_feestrdetails where safs_code =fs.safs_code and safd_type = 'A' and safd_feefor = '0' and safd_sem = 0  ))

                        else
                        ( fsd.safd_feefor = '0' and  fsd.safd_type = 'S' )
                        or (fsd.safd_sem = stu.sasi_cursem and fsd.safd_type = 'S' and fsd.safd_feefor = '1' and
                        fsd.saft_code not in (select saft_code from sas_feestrdetails where safs_code =fs.safs_code and safd_type = 'S' and safd_feefor = '0' ) )
                        END)
                        left join sas_feestramount fsa on fs.safs_code = fsa.safs_code and stu.sasc_code = fsa.sasc_code  and fsd.saft_code = fsa.saft_code
                        and fsd.saft_code = fsa.saft_code and fsa.safd_feefor = fsd.safd_feefor and fsa.safd_sem = fsd.safd_sem and fsa.safd_type = fsd.safd_type
                        left join sas_feetypes ft on ft.saft_code = fsa.saft_code and fsd.saft_code = fsa.saft_code    
                        
                       where sspon.SASS_Sponsor = " + clsGeneric.AddQuotes(Sponsor) + " and SAS_SponsorInvoice.BatchCode = " + clsGeneric.AddQuotes(batchcode) +
                       " Group By SAS_SponsorInvoice.transamount,SAS_SponsorInvoice.batchcode,stu.SASI_MatricNo, stu.SASI_Name, stu.SASI_PgId,prog.sapg_program, stu.sasi_intake, stu.SASI_CurSem, sspon.SASS_Sponsor, sspon.sass_limit, stu.sasc_code, sspon.sass_type order by stu.SASI_MatricNo ";

            try
            {
                if (!FormHelp.IsBlank(sqlCmd))
                {
                    using (IDataReader loReader = _DatabaseFactory.ExecuteReader(Helper.GetDataBaseType,
                        DataBaseConnectionString, sqlCmd).CreateDataReader())
                    {
                        while (loReader.Read())
                        {
                            StudentEn loItem = LoadObjectForSem(loReader);
                            loItem.TempAmount = GetValue<double>(loReader, "TransactionAMT");
                            loItem.MatricNo = GetValue<string>(loReader, "SASI_MatricNo");
                            loItem.StudentName = GetValue<string>(loReader, "SASI_Name");
                            loItem.ProgramID = GetValue<string>(loReader, "SASI_PgId");
                            loItem.CurrentSemester = GetValue<int>(loReader, "SASI_CurSem");
                            loItem.SponsorLimit = GetValue<double>(loReader, "sass_limit");
                            loItem.AllocatedAmount = GetValue<double>(loReader, "AllocatedAmount");
                            loItem.OutstandingAmount = GetValue<double>(loReader, "AvailableAmont");
                            loItem.CategoryCode = GetValue<string>(loReader, "sasc_code");
                            //loItem.FullySponsor = GetValue<Boolean>(loReader, "sass_type");
                            loItem.BatchCode = GetValue<string>(loReader, "BatchCode");
                            loItem.ProgramID = GetValue<string>(loReader, "SASI_PgId");
                            loItem.SponsorCode = GetValue<string>(loReader, "SASS_Sponsor");
                            loItem.ProgramType = GetValue<string>(loReader, "sapg_program");
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

        #region GetListStudentForAllocation

        /// <summary>
        /// Method to Get List of All Students
        /// </summary>
        /// <param name="argEn">Students Entity as an Input.MatricNo,StudentName,ICNo,ProgramId,ID and StatusRec as Input Properties.</param>
        /// <returns>Returns List of Students</returns>
        public List<AccountsDetailsEn> GetListStudentForAllocation(string sponsor)
        {
            List<AccountsDetailsEn> loEnList = new List<AccountsDetailsEn>();

            string sqlCmd = @"select sa.sasi_matricno,ss.sasi_name,ss.sasi_icno,ss.sasi_pgid,ss.sasi_cursem from SAS_StudentSpon sa inner join sas_student ss on ss.sasi_matricno = sa.sasi_matricno where sa.sass_sponsor = " + clsGeneric.AddQuotes(sponsor);
            try
            {
                if (!FormHelp.IsBlank(sqlCmd))
                {
                    using (IDataReader loReader = _DatabaseFactory.ExecuteReader(Helper.GetDataBaseType,
                        DataBaseConnectionString, sqlCmd).CreateDataReader())
                    {
                        while (loReader.Read())
                        {
                            AccountsDetailsEn loItem = new AccountsDetailsEn();
                            loItem.Sudentacc = new StudentEn();
                            loItem.Sudentacc.MatricNo = GetValue<string>(loReader, "SASI_MatricNo");
                            loItem.Sudentacc.StudentName = GetValue<string>(loReader, "SASI_Name");
                            loItem.Sudentacc.ProgramID = GetValue<string>(loReader, "SASI_PgId");
                            loItem.Sudentacc.ICNo = GetValue<string>(loReader, "SASI_ICNo");
                            loItem.CurrentSemester = GetValue<int>(loReader, "SASI_CurSem");
                            
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


