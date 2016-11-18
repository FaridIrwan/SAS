using System;
using System.Text;
using System.Transactions;
using System.Collections.Generic;
using HTS.SAS.Entities;
using HTS.SAS.DataAccessObjects;

namespace HTS.SAS.BusinessObjects
{
    /// <summary>
    /// Business Class to handle all the FeeStructure Methods.
    /// </summary>
    public class FeeStructBAL
    {
        /// <summary>
        /// Method to Get List of FeeStructure
        /// </summary>
        /// <param name="argEn">FeeStructure Entity is an Input.</param>
        /// <returns>Returns List of FeeStructure</returns>
        public List<FeeStructEn> GetList(FeeStructEn argEn)
        {
            try
            {
                FeeStructDAL loDs = new FeeStructDAL();
                return loDs.GetList(argEn);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Method to Get List of Active or Inactive FeeStucture
        /// </summary>
        /// <param name="argEn">FeeStucture Entity is an Input.FeeStryctureCode,PGCode,Semester,Description and status are Input Properties</param>
        /// <returns></returns>
        public List<FeeStructEn> GetFeeStructure(FeeStructEn argEn)
        {
            try
            {
                FeeStructDAL loDs = new FeeStructDAL();
                return loDs.GetFeeStructure(argEn);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Method to Get List of Active or Inactive FeeStucture BY StudentCategory
        /// </summary>
        /// <param name="argEn">FeeStucture Entity is an Input.FeeStryctureCode,PGCode,EffectFm and Status are Input Properties</param>
        /// <returns></returns>
        public List<FeeStructEn> GetFeeStructureList(FeeStructEn argEn)
        {
            try
            {
                FeeStructDAL loDs = new FeeStructDAL();
                return loDs.GetFeeStructureList(argEn);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Method to Get FeeStructure Entity
        /// </summary>
        /// <param name="argEn">FeeStructure Entity is an Input</param>
        /// <returns>Returns FeeStructure Entity</returns>
        public FeeStructEn GetItem(FeeStructEn argEn)
        {
            try
            {
                FeeStructDAL loDs = new FeeStructDAL();
                return loDs.GetItem(argEn);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Method to Insert FeeStructure 
        /// </summary>
        /// <param name="argEn">FeeStructure Entity is an Input.</param>
        /// <returns>Returns Boolean</returns>
        public bool Insert(FeeStructEn argEn)
        {
            bool flag;
            using (TransactionScope ts = new TransactionScope())
            {
                try
                {
                    FeeStructDAL loDs = new FeeStructDAL();
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
        /// Method to Update FeeStructure 
        /// </summary>
        /// <param name="argEn">FeeStructure Entity is an Input.</param>
        /// <returns>Returns Boolean</returns>
        public bool Update(FeeStructEn argEn)
        {
            bool flag;
            using (TransactionScope ts = new TransactionScope())
            {
                try
                {
                    FeeStructDAL loDs = new FeeStructDAL();
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
        /// Method to Delete FeeStructure 
        /// </summary>
        /// <param name="argEn">FeeStructure Entity is an Input.</param>
        /// <returns>Returns Boolean</returns>
        public bool Delete(FeeStructEn argEn)
        {
            bool flag;
            using (TransactionScope ts = new TransactionScope())
            {
                try
                {
                    FeeStructDAL loDs = new FeeStructDAL();
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
        /// <param name="argEn">FeeStructure Entity is as Input.</param>
        /// <returns>Returns a Boolean</returns>
        public bool IsValid(FeeStructEn argEn)
        {
            try
            {
                if (argEn.FeeStructureCode == null || argEn.FeeStructureCode.ToString().Length <= 0)
                    throw new Exception("FeeStructureCode Is Required!");
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Method to Get List of Active or Inactive FeeStucture
        /// </summary>
        /// <param name="argEn">FeeStucture Entity is an Input.PGCode,Semester,Description and status are Input Properties</param>
        /// <returns></returns>
        public List<FeeStructEn> GetFeeStructureDetailList(FeeStructEn argEn)
        {
            try
            {
                FeeStructDAL loDs = new FeeStructDAL();
                return loDs.GetFeeStructureDetailList(argEn);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }

}
