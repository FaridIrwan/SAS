using System;
using System.Text;
using System.Transactions;
using System.Collections.Generic;
using HTS.SAS.Entities;
using HTS.SAS.DataAccessObjects;

namespace HTS.SAS.BusinessObjects
{
    /// <summary>
    /// Business Class to handle all the DunningLetters.
    /// </summary>
    public class SponsorCoverLetterBAL
    {

        /// <summary>
        /// Method to Get SponsorCoverLetter
        /// </summary>
        /// <param name="argEn">SponsorCoverLetter Entity is an Input.</param>
        /// <returns>Returns List of SponsorCoverLetterEn.</returns>
        public List<SponsorCoverLetterEn> GetList(SponsorCoverLetterEn argEn)
        {
            try
            {
                SponsorCoverLetterDAL loDs = new SponsorCoverLetterDAL();
                return loDs.GetList(argEn);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Method to Insert SponsorCoverLetter
        /// </summary>
        /// <param name="argEn">SponsorCoverLetter Entity is the Input.</param>
        /// <returns>Returns a Boolean</returns>
        public bool Insert(SponsorCoverLetterEn argEn)
        {
            bool flag;
            using (TransactionScope ts = new TransactionScope())
            {
                try
                {
                    SponsorCoverLetterDAL loDs = new SponsorCoverLetterDAL();
                    flag = loDs.Insert(argEn);
                    ts.Complete();
                }
                catch (Exception ex)
                {

                    throw ex;
                }
            }
            return flag;
        }
        /// <summary>
        /// Method to Update SponsorCoverLetter
        /// </summary>
        /// <param name="argEn">SponsorCoverLetter Entity is the Input.</param>
        /// <returns>Returns a Boolean</returns>
        public bool Update(SponsorCoverLetterEn argEn)
        {
            bool flag;
            using (TransactionScope ts = new TransactionScope())
            {
                try
                {
                    SponsorCoverLetterDAL loDs = new SponsorCoverLetterDAL();
                    flag = loDs.Update(argEn);
                    ts.Complete();
                }
                catch (Exception ex)
                {

                    throw ex;
                }
            }
            return flag;
        }
        /// <summary>
        ///  Method to Delete  SponsorCoverLetter 
        /// </summary>
        /// <param name="argEn">SponsorCoverLetter Entity is an Input.SASCL_Code is Property.</param>
        /// <returns>Returns a Boolean</returns>
        public bool Delete(SponsorCoverLetterEn argEn)
        {
            bool flag;
            using (TransactionScope ts = new TransactionScope())
            {
                try
                {
                    SponsorCoverLetterDAL loDs = new SponsorCoverLetterDAL();
                    flag = loDs.Delete(argEn);
                    ts.Complete();
                }
                catch (Exception ex)
                {

                    throw ex;
                }
            }
            return flag;
        }

        /// <summary>
        /// Method to Get Program By Sponsor
        /// </summary>
        /// <param name="SponsorId">SponsorId is an Input.</param>
        /// <returns>Returns List of ProgramInfoEn.</returns>
        public List<ProgramInfoEn> GetProgramBySponsor(string SponsorId)
        {
            try
            {
                SponsorCoverLetterDAL loDs = new SponsorCoverLetterDAL();
                return loDs.GetProgramBySponsor(SponsorId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Method to get student details by sponsor
        /// </summary>
        /// <param name="argEn">Sponsor cover letter Entity is an Input</param>
        /// <returns>returns list of SponsorCoverLetterEn</returns>
        public List<SponsorCoverLetterEn> GetSponsorStudentDetails(SponsorCoverLetterEn argEn)
        {
            try
            {
                SponsorCoverLetterDAL loDs = new SponsorCoverLetterDAL();
                return loDs.GetSponsorStudentDetails(argEn);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Method to get sponsor list (filter by student registration status and activation status)
        /// </summary>
        /// <param name="argEn">Sponsor Entity is an Input</param>
        /// <returns>returns list of Sponsor</returns>
        public List<SponsorEn> GetSponsorWithStudent(SponsorEn argEn)
        {
            try
            {
                SponsorCoverLetterDAL loDs = new SponsorCoverLetterDAL();
                return loDs.GetSponsorWithStudent(argEn);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}
