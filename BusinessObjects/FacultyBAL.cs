using System;
using System.Text;
using System.Transactions;
using System.Collections.Generic;
using HTS.SAS.Entities;
using HTS.SAS.DataAccessObjects;


namespace HTS.SAS.BusinessObjects
{
    /// <summary>
    /// Business Class to handle all the Faculty Methods.
    /// </summary>
    public class FacultyBAL
    {
        /// <summary>
        /// Method to Get List of Faculty
        /// </summary>
        /// <param name="argEn">Faculty Entity is an Input.</param>
        /// <returns>Returns List of Faculty</returns>
        public List<FacultyEn> GetList(FacultyEn argEn)
        {
            try
            {
                FacultyDAL loDs = new FacultyDAL();
                return loDs.GetList(argEn);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Method to Get List of All Faculty
        /// </summary>
        /// <param name="argEn">Faculty Entity is an Input.</param>
        /// <returns>Returns List of Faculty</returns>
        public List<FacultyEn> GetFacultyList(FacultyEn argEn)
        {
            try
            {
                FacultyDAL loDs = new FacultyDAL();
                return loDs.GetFacultyList(argEn);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Method to Get Faculty Entity
        /// </summary>
        /// <param name="argEn">Faculty Entity is an Input</param>
        /// <returns>Returns Faculty Entity</returns>
        public FacultyEn GetItem(FacultyEn argEn)
        {
            try
            {
                FacultyDAL loDs = new FacultyDAL();
                return loDs.GetItem(argEn);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Method to Insert Faculty 
        /// </summary>
        /// <param name="argEn">Faculty Entity is an Input.</param>
        /// <returns>Returns Boolean</returns>
        public bool Insert(FacultyEn argEn)
        {
            bool flag;
            using (TransactionScope ts = new TransactionScope())
            {
                try
                {
                    FacultyDAL loDs = new FacultyDAL();
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
        /// Method to Update Faculty 
        /// </summary>
        /// <param name="argEn">Faculty Entity is an Input.</param>
        /// <returns>Returns Boolean</returns>
        public bool Update(FacultyEn argEn)
        {
            bool flag;
            using (TransactionScope ts = new TransactionScope())
            {
                try
                {
                    FacultyDAL loDs = new FacultyDAL();
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
        /// Method to Delete Faculty 
        /// </summary>
        /// <param name="argEn">Faculty Entity is an Input.</param>
        /// <returns>Returns Boolean</returns>
        public bool Delete(FacultyEn argEn)
        {
            bool flag;
            using (TransactionScope ts = new TransactionScope())
            {
                try
                {
                    FacultyDAL loDs = new FacultyDAL();
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
        /// <param name="argEn">Faculty Entity as Input.</param>
        /// <returns>Returns a Boolean</returns>
        public bool IsValid(FacultyEn argEn)
        {
            try
            {
                if (argEn.SAFC_Code == null || argEn.SAFC_Code.ToString().Length <= 0)
                    throw new Exception("SAFC_Code Is Required!");
                if (argEn.SAFC_Desc == null || argEn.SAFC_Desc.ToString().Length <= 0)
                    throw new Exception("SAFC_Desc Is Required!");
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }

}
//---------------------------------------------------------------------------------

//---------------------------------------------------------------------------------