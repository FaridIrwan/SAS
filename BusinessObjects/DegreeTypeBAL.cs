using System;
using System.Text;
using System.Transactions;
using System.Collections.Generic;
using HTS.SAS.Entities;
using HTS.SAS.DataAccessObjects;

namespace HTS.SAS.BusinessObjects
{
    /// <summary>
    /// Business Class to handle all the DegreeTypes Methods.
    /// </summary>
    public class DegreeTypeBAL
    {
        /// <summary>
        /// Method to Get List of DegreeTypes
        /// </summary>
        /// <param name="argEn">DegreeType Entity is an Input.</param>
        /// <returns>Returns List of DegreeTypes</returns>
        public List<DegreeTypeEn> GetList(DegreeTypeEn argEn)
        {
            try
            {
                DegreeTypeDAL loDs = new DegreeTypeDAL();
                return loDs.GetList(argEn);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Method to Get List of Active or Inactive DegreeTypes
        /// </summary>
        /// <param name="argEn">DegreeType Entity is an Input.DegreeTypeCode,Description,SName and Status are Input Parameters</param>
        /// <returns>Returns List of DegreeTypes</returns>
        public List<DegreeTypeEn> GetDegreeTypeList(DegreeTypeEn argEn)
        {
            try
            {
                DegreeTypeDAL loDs = new DegreeTypeDAL();
                return loDs.GetDegreeTypeList(argEn);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Method to Get DegreeType Entity
        /// </summary>
        /// <param name="argEn">DegreeType Entity is an Input</param>
        /// <returns>Returns DegreeType Entity</returns>
        public DegreeTypeEn GetItem(DegreeTypeEn argEn)
        {
            try
            {
                DegreeTypeDAL loDs = new DegreeTypeDAL();
                return loDs.GetItem(argEn);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Method to Insert DegreeType 
        /// </summary>
        /// <param name="argEn">DegreeType Entity is an Input.</param>
        /// <returns>Returns Boolean</returns>
        public bool Insert(DegreeTypeEn argEn)
        {
            bool flag;
            using (TransactionScope ts = new TransactionScope())
            {
                try
                {
                    DegreeTypeDAL loDs = new DegreeTypeDAL();
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
        /// Method to Update DegreeType 
        /// </summary>
        /// <param name="argEn">DegreeType Entity is an Input.</param>
        /// <returns>Returns Boolean</returns>
        public bool Update(DegreeTypeEn argEn)
        {
            bool flag;
            using (TransactionScope ts = new TransactionScope())
            {
                try
                {
                    DegreeTypeDAL loDs = new DegreeTypeDAL();
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
        /// Method to Delete DegreeType 
        /// </summary>
        /// <param name="argEn">DegreeType Entity is an Input.</param>
        /// <returns>Returns Boolean</returns>
        public bool Delete(DegreeTypeEn argEn)
        {
            bool flag;
            using (TransactionScope ts = new TransactionScope())
            {
                try
                {
                    DegreeTypeDAL loDs = new DegreeTypeDAL();
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
        /// <param name="argEn">DegreeType Entity as Input.</param>
        /// <returns>Returns a Boolean</returns>
        public bool IsValid(DegreeTypeEn argEn)
        {
            try
            {
                if (argEn.DegreeTypeCode == null || argEn.DegreeTypeCode.ToString().Length <= 0)
                    throw new Exception("DegreeTypeCode Is Required!");
                if (argEn.Description == null || argEn.Description.ToString().Length <= 0)
                    throw new Exception("Description Is Required!");
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }

}
