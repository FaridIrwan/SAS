
using System;
using System.Text;
using System.Collections.Generic;
using HTS.SAS.Entities;
//using HTS.SAS.DataAccessObjects;
using HTS.SAS.DataAccessObjects;
using System.Transactions;
using System.Data.SqlClient;

namespace HTS.SAS.BusinessObjects
{
    /// <summary>
    /// Business Class to handle all the Students Methods.
    /// </summary>
    public class StudentBAL
    {
        /// <summary>
        /// Method to Get List of Students
        /// </summary>
        /// <param name="argEn">Students Entity as an Input.</param>
        /// <returns>Returns List of Students</returns>
        public List<StudentEn> GetList(StudentEn argEn)
        {
            try
            {
                StudentDAL loDs = new StudentDAL();
                return loDs.GetList(argEn);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Method to Get List of Students
        /// </summary>
        /// <param name="argEn">Students Entity as an Input.</param>
        /// <returns>Returns List of Students</returns>
        public List<StudentEn> GetListBySemProgTypeProgID(StudentEn argEn)
        {
            try
            {
                StudentDAL loDs = new StudentDAL();
                return loDs.GetListBySemProgTypeProgID(argEn);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Method to Get List of Students with Sponsers
        /// </summary>
        /// <param name="argEn">Students Entity as an Input.Sponsor,SAKO_Code,ProgramID,Faculty and CategoryCode as Input Properties.</param>
        /// <returns>Returns List of Students</returns>
        public List<StudentEn> GetlisStudent(StudentEn argEn)
        {
            try
            {
                StudentDAL loDs = new StudentDAL();
                return loDs.GetlisStudent(argEn);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Method to Get List of Students with Sponsers
        /// </summary>
        /// <param name="argEn">Students Entity as an Input.Sponsor,SAKO_Code,ProgramID,Faculty and CategoryCode as Input Properties.</param>
        /// <returns>Returns List of Students</returns>
        public List<KokoEn> GetKokoDetails(KokoEn argEn)
        {
            try
            {
                StudentDAL loDs = new StudentDAL();
                return loDs.GetKokoDetails(argEn);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Method to Get List of Students with Sponsers
        /// </summary>
        /// <param name="argEn">Students Entity as an Input.Sponsor,SAKO_Code,ProgramID,Faculty and CategoryCode as Input Properties.</param>
        /// <returns>Returns List of Students</returns>
        public List<StudentEn> GetlistStudentByStudent(StudentEn argEn)
        {
            try
            {
                StudentDAL loDs = new StudentDAL();
                return loDs.GetlistStudentByStudent(argEn);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Method to Get List of Students with Sponsers (filtering sponsorship validity and student status (active & registered))
        /// </summary>
        /// <param name="argEn">Students Entity as an Input.Sponsor,SAKO_Code,ProgramID,Faculty and CategoryCode as Input Properties.</param>
        /// <returns>Returns List of Students</returns>
        public List<StudentEn> GetlistStudentByStudentWithValidity(StudentEn argEn)
        {
            try
            {
                StudentDAL loDs = new StudentDAL();
                return loDs.GetlistStudentByStudentWithValidity(argEn);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Method to Get List of Students with Sponsers (filtering sponsorship validity and student status (active & registered))
        /// </summary>
        /// <param name="argEn">Students Entity as an Input.Sponsor,SAKO_Code,ProgramID,Faculty and CategoryCode as Input Properties.</param>
        /// <returns>Returns List of Students</returns>
        public StudentEn GetStudentDetailsBySponsorAllocationWithStuValidity(StudentEn argEn)
        {
            try
            {
                StudentDAL loDs = new StudentDAL();
                return loDs.GetStudentDetailsBySponsorAllocationWithStuValidity(argEn);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Method to Get List of Students with SponsorShip (filtering sponsorship validity and student status (active & registered))
        /// </summary>
        /// <param name="argEn">Students Entity as an Input.Sponsor,SAKO_Code,ProgramID,Faculty and CategoryCode as Input Properties.</param>
        /// <returns>Returns List of Students</returns>
        public List<StudentEn> GetStudentSponsorship(string programID, string Sponsor)
        {
            try
            {
                StudentDAL loDs = new StudentDAL();
                return loDs.GetStudentSponsorship(programID, Sponsor);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Method to Check For Students
        /// </summary>
        /// <param name="argEn">Students Entity as an Input.MatricNo,StudentName,ICNo,ProgramId and Faculty as Input Properties.</param>
        /// <returns>Returns List of Students</returns>
        public List<StudentEn> CheckStudentList(StudentEn argEn)
        {
            try
            {
                StudentDAL loDs = new StudentDAL();
                return loDs.CheckStudentList(argEn);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Method to Get List of Active or Inactive Students
        /// </summary>
        /// <param name="argEn">Students Entity as an Input.MatricNo,StudentName,ICNo,ProgramId and StatusRec as Input Properties.</param>
        /// <returns>Returns List of Students</returns>
        public List<StudentEn> GetListByProgram(StudentEn argEn)
        {
            try
            {
                StudentDAL loDs = new StudentDAL();
                return loDs.GetListByProgram(argEn);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<StudentEn> GetListGroupedByProgram(StudentEn argEn)
        {
            try
            {
                StudentDAL loDs = new StudentDAL();
                return loDs.GetListGroupedByProgram(argEn);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Added by Solomon to fetch the details based on the status.
        /// </summary>
        /// <param name="argEn"></param>
        /// <returns></returns>
        public List<StudentEn> GetListByProgramForFee(StudentEn argEn)
        {
            try
            {
                StudentDAL loDs = new StudentDAL();
                return loDs.GetListByProgramForFee(argEn);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Method to Get List of Active or Inactive Students
        /// </summary>
        /// <param name="argEn">Students Entity as an Input.MatricNo,StudentName,ICNo,ProgramId and StatusRec as Input Properties.</param>
        /// <returns>Returns List of Students</returns>
        public List<StudentEn> GetStudentList(StudentEn argEn)
        {
            try
            {
                StudentDAL loDs = new StudentDAL();
                return loDs.GetStudentList(argEn);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Method to Get List of All Students
        /// </summary>
        /// <param name="argEn">Students Entity as an Input.MatricNo,StudentName,ICNo,ProgramId,ID and StatusRec as Input Properties.</param>
        /// <returns>Returns List of Students</returns>
        public List<StudentEn> GetListStudent(StudentEn argEn)
        {
            try
            {
                StudentDAL loDs = new StudentDAL();
                return loDs.GetListStudent(argEn);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Method to Get List of All Students
        /// </summary>
        /// <param name="argEn">Students Entity as an Input.MatricNo,StudentName,ICNo,ProgramId,ID and StatusRec as Input Properties.</param>
        /// <returns>Returns List of Students</returns>
        public List<StudentEn> GetListStudentOutstanding(StudentEn argEn)
        {
            try
            {
                StudentDAL loDs = new StudentDAL();
                return loDs.GetListStudentOutstanding(argEn);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Method to Get List of Students who has Outstanding amount.
        /// </summary>
        /// <param name="argEn">Students Entity as an Input.Students Program Id, Semester Properties.</param>
        /// <param name="HasToIncludeLoan">Include Loan Amount Status</param>
        /// <returns>Returns List of Students</returns>
        public List<StudentEn> GetListOutstanding(StudentEn argEn, bool HasToIncludeLoan, bool IncludeCurSem)
        {
            try
            {
                StudentDAL loDs = new StudentDAL();
                return loDs.GetListOutstanding(argEn, HasToIncludeLoan, IncludeCurSem);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Method to Get Student Entity
        /// </summary>
        /// <param name="argEn">Student Entity is an Input.MatricNo as Input Property.</param>
        /// <returns>Returns Student Entity</returns>
        public StudentEn GetItem(string MatricNo)
        {
            try
            {
                StudentDAL loDs = new StudentDAL();
                return loDs.GetItem(MatricNo);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Method to Get Student Information
        /// </summary>
        /// <param name="argEn">Student Entity is an Input.MatricNo as Input Property.</param>
        /// <returns>Returns Student Entity</returns>
        public StudentEn GetStudInfo(string ICNO)
        {
            try
            {
                StudentDAL loDs = new StudentDAL();
                return loDs.GetStudInfo(ICNO);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Method to Insert Student 
        /// </summary>
        /// <param name="argEn">Student Entity is an Input.</param>
        /// <returns>Returns Boolean</returns>
        public bool Insert(StudentEn argEn)
        {
                try
                {
                    StudentDAL loDs = new StudentDAL();
                    return loDs.Insert(argEn);
                    
                }
                catch (Exception ex)
                {
                    
                    throw ex;
                }
        }
        /// <summary>
        /// Method to Update Student 
        /// </summary>
        /// <param name="argEn">Student Entity is an Input.</param>
        /// <returns>Returns Boolean</returns>
        public bool Update(StudentEn argEn)
        {
            try
                {
                    StudentDAL loDs = new StudentDAL();
                    return loDs.Update(argEn);
                }
                catch (Exception ex)
                {
                    
                    throw ex;
                }
        }
        /// <summary>
        /// Method to Update Student 
        /// </summary>
        /// <param name="argEn">Student Entity is an Input.</param>
        /// <returns>Returns Boolean</returns>
        public bool UpdateSemester(string CurSem, string Program, String newsem)
        {
            try
            {
                StudentDAL loDs = new StudentDAL();
                return loDs.UpdateSemester(CurSem, Program, newsem);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        /// <summary>
        /// Method to Update AFC status For Student 
        /// </summary>
        /// <param name="argEn">Student Entity is an Input.</param>
        /// <returns>Returns Boolean</returns>
        //public bool UpdateAFC(StudentEn argEn)
        //{
        //        try
        //        {
        //            StudentDAL loDs = new StudentDAL();
        //            return loDs.UpdateAfc(argEn);
        //        }
        //        catch (Exception ex)
        //        {
                    
        //            throw ex;
        //        }
        //}
        /// <summary>
        /// Method to Delete Student 
        /// </summary>
        /// <param name="argEn">Student Entity is an Input.MatricNo as Input Property.</param>
        /// <returns>Returns Boolean</returns>
        public bool Delete(StudentEn argEn)
        {
                try
                {
                    StudentDAL loDs = new StudentDAL();
                    return loDs.Delete(argEn);
                }
                catch (Exception ex)
                {
                    
                    throw ex;
                }
        }
        /// <summary>
        /// Method to Check Validation
        /// </summary>
        /// <param name="argEn">Student Entity is as Input.</param>
        /// <returns>Returns a Boolean</returns>
        public bool IsValid(StudentEn argEn)
        {
            try
            {
                if (argEn.MatricNo == null || argEn.MatricNo.ToString().Length <= 0)
                    throw new Exception("MatricNo Is Required!");
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Method to Insert Student Outstanding 
        /// </summary>
        /// <param name="argEn">Student Entity is an Input.</param>
        /// <returns>Returns Boolean</returns>
        public bool InsertStudentOutstanding(List<StudentEn> lstStudents)
        {
            bool isInserted = false;
            using (TransactionScope ts = new TransactionScope())
            {
                try
                {
                    StudentDAL loDs = new StudentDAL();
                    foreach (StudentEn argEn in lstStudents)
                    {
                        loDs.InsertStudentOutstanding(argEn);
                    }
                    isInserted = true;
                    ts.Complete();
                }
                catch (Exception ex)
                {
                    
                    throw ex;
                }
            }
            return isInserted;
        }

        /// <summary>
        /// Method to Insert Student Outstanding 
        /// </summary>
        /// <param name="argEn">Student Entity is an Input.</param>
        /// <returns>Returns Boolean</returns>
        public bool UpdatingStudentOutstandingReleaseStatus(StudentEn argEn,List<StudentEn> lstStudents)
        {
            bool isInserted = false;
            using (TransactionScope ts = new TransactionScope())
            {
                try
                {
                    StudentDAL loDs = new StudentDAL();
                    loDs.UpdateStudentOutstandingStatusEmpty(argEn);
                    foreach (StudentEn argEn1 in lstStudents)
                    {
                        loDs.UpdateStudentOutstandingStatus(argEn1);
                    }
                    isInserted = true;
                    ts.Complete();
                }
                catch (Exception ex)
                {
                    
                    throw ex;
                }
            }
            return isInserted;
        }

        public StudentEn FetchLedgerDetails(string MatricNo)
        {
            StudentDAL objDAL = new StudentDAL();
            return objDAL.FetchLedgerDetails(MatricNo);
        }

        public List<StudentEn> GetListStudentChange(string Category, int TrackModule)
        {
            try
            {
                StudentDAL loDs = new StudentDAL();
                return loDs.GetListStudentChange(Category, TrackModule);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<StudentEn> GetListStudentChangeByProgram(string Category, int TrackModule, string FeeType)
        {
            try
            {
                StudentDAL loDs = new StudentDAL();
                return loDs.GetListStudentChangeByProgram(Category, TrackModule, FeeType);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<StudentEn> GetListStudentChangeDetails(string Category, int TrackModule, string FeeType)
        {
            try
            {
                StudentDAL loDs = new StudentDAL();
                return loDs.GetListStudentChangeDetails(Category, TrackModule, FeeType);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //added by Hafiz @ 04/3/2016
        //methods for load data from sas_studentoutstanding
        public List<StudentEn> GetOutstandingForUploadedFile(List<StudentEn> list_stud)
        {
            List<StudentEn> loEnList = new List<StudentEn>();

            using (TransactionScope ts = new TransactionScope())
            {
                try
                {
                    StudentDAL loDs = new StudentDAL();

                    foreach (StudentEn argEn in list_stud)
                    {
                        StudentEn loItem = new StudentEn();

                        loItem = loDs.GetOutstandingForUploadedFile(argEn);
                        loEnList.Add(loItem);
                    }
                    ts.Complete();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

            return loEnList;
        }

        /// <summary>
        /// Method to Get List of All Students
        /// </summary>
        /// <param name="argEn">Students Entity as an Input.MatricNo,StudentName,ICNo,ProgramId,ID and StatusRec as Input Properties.</param>
        /// <returns>Returns List of Students</returns>
        public List<StudentEn> GetListOutstandingAmtAllStud(StudentEn argEn)
        {
            try
            {
                StudentDAL loDs = new StudentDAL();
                return loDs.GetListOutstandingAmtAllStud(argEn);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<AccountsDetailsEn> GetListStudentForAllocation(string sponsor)
        {
            try
            {
                StudentDAL loDs = new StudentDAL();
                return loDs.GetListStudentForAllocation(sponsor);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Method to Get List of Students with SponsorShip (filtering sponsorship validity and student status (active & registered))
        /// </summary>
        /// <param name="argEn">Students Entity as an Input.Sponsor,SAKO_Code,ProgramID,Faculty and CategoryCode as Input Properties.</param>
        /// <returns>Returns List of Students</returns>
        public List<StudentEn> GetStudentSponsorshipWithoutValidity(string Matricno)
        {
            try
            {
                StudentDAL loDs = new StudentDAL();
                return loDs.GetStudentSponsorshipWithoutValidity(Matricno);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Method to Get List of Students with SponsorShip (filtering sponsorship validity and student status (active & registered))
        /// </summary>
        /// <param name="argEn">Students Entity as an Input.Sponsor,SAKO_Code,ProgramID,Faculty and CategoryCode as Input Properties.</param>
        /// <returns>Returns List of Students</returns>
        public List<StudentEn> GetSponsorFeeList(string Sponsor)
        {
            try
            {
                StudentDAL loDs = new StudentDAL();
                return loDs.GetSponsorFeeList(Sponsor);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Method to Delete Student List - change Prog/Cdt Hour/Hostel Withdrawn
        /// </summary>
        /// <returns>Returns Boolean</returns>
        //public bool DeleteStudentChanges(List<StudentEn> argList, string Category, int TrackModule)
        //{
        //    try
        //    {
        //        StudentDAL loDs = new StudentDAL();
        //        return loDs.DeleteStudentChanges(argList, Category, TrackModule);
        //    }
        //    catch (Exception ex)
        //    {

        //        throw ex;
        //    }
        //}
    }

}
