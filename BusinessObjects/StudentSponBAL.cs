using System;
using System.Text;
using System.Transactions;
using System.Collections.Generic;
using HTS.SAS.Entities;
using HTS.SAS.DataAccessObjects;

namespace HTS.SAS.BusinessObjects
{
    /// <summary>
    /// Business Class to handle all the StudentSponsor Methods.
    /// </summary>
    public class StudentSponBAL
    {
        /// <summary>
        /// Method to Get List of StudentSponsor
        /// </summary>
        /// <param name="argEn">StudentSponsor Entity as an Input.</param>
        /// <returns>Returns List of StudentSponsor</returns>
        public List<StudentSponEn> GetList(StudentSponEn argEn)
        {
            try
            {
                StudentSponDAL loDs = new StudentSponDAL();
                return loDs.GetList(argEn);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Method to Get List of StudentSponsor
        /// </summary>
        /// <param name="argEn">StudentSponsor Entity as an Input.MatricNo as Input Property.</param>
        /// <returns>Returns List of StudentSponsor</returns>
        public List<StudentSponEn> GetStuSponsorList(StudentSponEn argEn)
        {
            try
            {
                StudentSponDAL loDs = new StudentSponDAL();
                return loDs.GetStuSponserList(argEn);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Method to Get StudentSponsor Entity
        /// </summary>
        /// <param name="argEn">StudentSponsor Entity is an Input.MatricNo as Input Property.</param>
        /// <returns>Returns StudentSponsor Entity</returns>
        public StudentSponEn GetItem(StudentSponEn argEn)
        {
            try
            {
                StudentSponDAL loDs = new StudentSponDAL();
                return loDs.GetItem(argEn);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Method to Insert StudentSponsor 
        /// </summary>
        /// <param name="argEn">StudentSponsor Entity is an Input.</param>
        /// <returns>Returns Boolean</returns> 
        public bool Insert(StudentSponEn argEn)
        {
            bool flag;
            using (TransactionScope ts = new TransactionScope())
            {
                try
                {
                    StudentSponDAL loDs = new StudentSponDAL();
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
        /// Method to Update StudentSponsor 
        /// </summary>
        /// <param name="argEn">StudentSponsor Entity is an Input.</param>
        /// <returns>Returns Boolean</returns> 
        public bool Update(StudentSponEn argEn)
        {
            bool flag;
            using (TransactionScope ts = new TransactionScope())
            {
                try
                {
                    StudentSponDAL loDs = new StudentSponDAL();
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
        /// Method to Delete StudentSponsor 
        /// </summary>
        /// <param name="argEn">StudentSponsor Entity is an Input.MatricNo as Input Property.</param>
        /// <returns>Returns Boolean</returns> 
        public bool Delete(StudentSponEn argEn)
        {
            bool flag;
            using (TransactionScope ts = new TransactionScope())
            {
                try
                {
                    StudentSponDAL loDs = new StudentSponDAL();
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
        /// Method to Check Validation
        /// </summary>
        /// <param name="argEn">StudentSponsor Entity is as Input.</param>
        /// <returns>Returns a Boolean</returns>
        public bool IsValid(StudentSponEn argEn)
        {
            try
            {
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }

}
