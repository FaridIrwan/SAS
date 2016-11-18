using System;
using System.Text;
using System.Transactions;
using System.Collections.Generic;
using HTS.SAS.Entities;
using HTS.SAS.DataAccessObjects;

namespace HTS.SAS.BusinessObjects
{
    /// <summary>
    /// Business Class to handle all the UniversityProfile Methods.
    /// </summary>
    public class UniversityProfileBAL
    {
        /// <summary>
        /// Method to Get List of UniversityProfile
        /// </summary>
        /// <param name="argEn">UniversityProfile Entity as an Input.</param>
        /// <returns>Returns List of UniversityProfile</returns>
        public List<UniversityProfileEn> GetList(UniversityProfileEn argEn)
        {
            try
            {
                UniversityProfileDAL loDs = new UniversityProfileDAL();
                return loDs.GetList(argEn);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Method to Get List of All UniversityProfiles
        /// </summary>
        /// <param name="argEn">UniversityProfile Entity as an Input.UniversityProfileCode,Name and SName as Input Properties.</param>
        /// <returns>Returns List of UniversityProfile</returns>
        public List<UniversityProfileEn> GetUniversityProfileList(UniversityProfileEn argEn)
        {
            try
            {
                UniversityProfileDAL loDs = new UniversityProfileDAL();
                return loDs.GetUniversityProfileList(argEn);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Method to Get UniversityProfile Entity
        /// </summary>
        /// <param name="argEn">UniversityProfile Entity is an Input.UniversityProfileCode as Input Property.</param>
        /// <returns>Returns UniversityProfile Entity</returns>
        public UniversityProfileEn GetItem(UniversityProfileEn argEn)
        {
            try
            {
                UniversityProfileDAL loDs = new UniversityProfileDAL();
                return loDs.GetItem(argEn);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Method to Insert UniversityProfile 
        /// </summary>
        /// <param name="argEn">UniversityProfile Entity is an Input.</param>
        /// <returns>Returns Boolean</returns>
        public bool Insert(UniversityProfileEn argEn)
        {
            bool flag;
            using (TransactionScope ts = new TransactionScope())
            {
                try
                {
                    UniversityProfileDAL loDs = new UniversityProfileDAL();
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
        /// Method to Update UniversityProfile 
        /// </summary>
        /// <param name="argEn">UniversityProfile Entity is an Input.</param>
        /// <returns>Returns Boolean</returns>
        public bool Update(UniversityProfileEn argEn)
        {
            bool flag;
            using (TransactionScope ts = new TransactionScope())
            {
                try
                {
                    UniversityProfileDAL loDs = new UniversityProfileDAL();
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
        /// Method to Delete UniversityProfile 
        /// </summary>
        /// <param name="argEn">UniversityProfile Entity is an Input.UniversityProfileCode as Input Propoerty.</param>
        /// <returns>Returns Boolean</returns>
        public bool Delete(UniversityProfileEn argEn)
        {
            bool flag;
            using (TransactionScope ts = new TransactionScope())
            {
                try
                {
                    UniversityProfileDAL loDs = new UniversityProfileDAL();
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
        /// <param name="argEn">UniversityProfile Entity as Input.</param>
        /// <returns>Returns a Boolean</returns>
        public bool IsValid(UniversityProfileEn argEn)
        {
            try
            {
                if (argEn.UniversityProfileCode == null || argEn.UniversityProfileCode.ToString().Length <= 0)
                    throw new Exception("UniversityProfileCode Is Required!");
                if (argEn.Name == null || argEn.Name.ToString().Length <= 0)
                    throw new Exception("Name Is Required!");
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }

}
