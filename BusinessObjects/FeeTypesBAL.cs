using System;
using System.Text;
using System.Transactions;
using System.Collections.Generic;
using HTS.SAS.Entities;
using HTS.SAS.DataAccessObjects;

namespace HTS.SAS.BusinessObjects
{
    /// <summary>
    /// Business Class to handle all the FeeTypes Methods.
    /// </summary>
    public class FeeTypesBAL
    {
        /// <summary>
        /// Method to Get List of FeeTypes
        /// </summary>
        /// <param name="argEn">FeeTypes Entity is an Input.</param>
        /// <returns>Returns List of FeeTypes</returns>
        public List<FeeTypesEn> GetList(FeeTypesEn argEn)
        {
            try
            {
                FeeTypesDAL loDs = new FeeTypesDAL();
                return loDs.GetList(argEn);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Method to Get List of Active or Inactive FeeTypes
        /// </summary>
        /// <param name="argEn">FeeTypes Entity is an Input.FeeTypeCode,FeeType,Description and Status are Input Properties.</param>
        /// <returns>Returns List of FeeTypes</returns>
        public List<FeeTypesEn> GetFeeTypesList(FeeTypesEn argEn)
        {
            try
            {
                FeeTypesDAL loDs = new FeeTypesDAL();
                return loDs.GetFeeTypesList(argEn);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Method to Get List of Koko Charges
        /// </summary>
        /// <param name="argEn">Koko Charges Entity is an Input.</param>
        /// <returns>Returns List of FeeTypes</returns>
        public List<FeeTypesEn> GetKokoList(FeeTypesEn argEn)
        {
            try
            {
                FeeTypesDAL loDs = new FeeTypesDAL();
                return loDs.GetKokoList(argEn);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Method to Get List of Koko Charges
        /// </summary>
        /// <param name="argEn">Koko Charges Entity is an Input.</param>
        /// <returns>Returns List of FeeTypes</returns>
        public List<FeeTypesEn> GetKokokurikulumList(FeeTypesEn argEn)
        {
            try
            {
                FeeTypesDAL loDs = new FeeTypesDAL();
                return loDs.GetKokokurikulumList(argEn);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Method to Get List of Koko Charges
        /// </summary>
        /// <param name="argEn">Koko Charges Entity is an Input.</param>
        /// <returns>Returns List of FeeTypes</returns>
        public List<FeeTypesEn> GetKokoListddl(FeeTypesEn argEn)
        {
            try
            {
                FeeTypesDAL loDs = new FeeTypesDAL();
                return loDs.GetKokoListddl(argEn);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Method to Get List of FeeTypes
        /// </summary>
        /// <param name="argEn">FeeTypes Entity is an Input.FeeTypeCode,FeeType,Hostel,Description,GLCode,Priority and Status are Input Properties.</param>
        /// <returns>Returns List of FeeTypes</returns>
        public List<FeeTypesEn> GetFeeList(FeeTypesEn argEn)
        {
            try
            {
                FeeTypesDAL loDs = new FeeTypesDAL();
                return loDs.GetFeeList(argEn);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Method to Get List of Student FeeTypes
        /// </summary>
        /// <param name="argEn">FeeTypes Entity is an Input.FeeTypeCode,FeeType,Hostel,Description and Status are Input Properties.</param>
        /// <returns>Returns List of FeeTypes</returns>
        public List<FeeTypesEn> GetStudentFee(FeeTypesEn argEn)
        {
            try
            {
                FeeTypesDAL loDs = new FeeTypesDAL();
                return loDs.GetStudentFee(argEn);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Method to Get FeeTypes Entity
        /// </summary>
        /// <param name="argEn">FeeTypes Entity is an Input.FeeTypeCode ia an Input Property</param>
        /// <returns>Returns FeeTypes Entity</returns>
        public FeeTypesEn GetItem(FeeTypesEn argEn)
        {
            try
            {
                FeeTypesDAL loDs = new FeeTypesDAL();
                return loDs.GetItem(argEn);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Method to Insert FeeTypes 
        /// </summary>
        /// <param name="argEn">FeeTypes Entity is an Input.</param>
        /// <returns>Returns Boolean</returns>
        public bool Insert(FeeTypesEn argEn)
        {
            bool flag;
            using (TransactionScope ts = new TransactionScope())
            {
                try
                {
                    FeeTypesDAL loDs = new FeeTypesDAL();
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
        /// Method to Insert KokoTypes 
        /// </summary>
        /// <param name="argEn">FeeTypes Entity is an Input.</param>
        /// <returns>Returns Boolean</returns>
        public bool InsertKoko(FeeTypesEn argEn)
        {
            bool flag;
            using (TransactionScope ts = new TransactionScope())
            {
                try
                {
                    FeeTypesDAL loDs = new FeeTypesDAL();
                    flag = loDs.InsertKoko(argEn);
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
        /// Method to Update FeeTypes 
        /// </summary>
        /// <param name="argEn">FeeTypes Entity is an Input.</param>
        /// <returns>Returns Boolean</returns>
        
        public bool Update(FeeTypesEn argEn)
        {
            bool flag;
            using (TransactionScope ts = new TransactionScope())
            {
                try
                {
                    FeeTypesDAL loDs = new FeeTypesDAL();
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
        /// Method to Update Koko List 
        /// </summary>
        /// <param name="argEn">Koko List Entity is an Input.</param>
        /// <returns>Returns Boolean</returns>

        public bool UpdateKokoList(FeeTypesEn argEn)
        {
            bool flag;
            using (TransactionScope ts = new TransactionScope())
            {
                try
                {
                    FeeTypesDAL loDs = new FeeTypesDAL();
                    flag = loDs.UpdateKokoList(argEn);
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
        /// Method to Delete FeeTypes 
        /// </summary>
        /// <param name="argEn">FeeTypes Entity is an Input.</param>
        /// <returns>Returns Boolean</returns>
        public bool Delete(FeeTypesEn argEn)
        {
            bool flag;
            using (TransactionScope ts = new TransactionScope())
            {
                try
                {
                    FeeTypesDAL loDs = new FeeTypesDAL();
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
        /// Method to Delete FeeTypes 
        /// </summary>
        /// <param name="argEn">FeeTypes Entity is an Input.</param>
        /// <returns>Returns Boolean</returns>
        public bool DeleteKoko(FeeTypesEn argEn)
        {
            bool flag;
            using (TransactionScope ts = new TransactionScope())
            {
                try
                {
                    FeeTypesDAL loDs = new FeeTypesDAL();
                    flag = loDs.DeleteKoko(argEn);
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
        /// Method to Delete All FeeTypes 
        /// </summary>
        /// <param name="argEn">FeeTypes Entity is an Input.FeeTypeCode ia an Input Property</param>
        /// <returns>Returns Boolean</returns>
        public bool FeeDelete(FeeTypesEn argEn)
        {
            bool flag;
            using (TransactionScope ts = new TransactionScope())
            {
                try
                {
                    FeeTypesDAL loDs = new FeeTypesDAL();
                    flag = loDs.FeeDelete(argEn);
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
        /// <param name="argEn">FeeTypes Entity is as Input.</param>
        /// <returns>Returns a Boolean</returns>
        public bool IsValid(FeeTypesEn argEn)
        {
            try
            {
                if (argEn.FeeTypeCode == null || argEn.FeeTypeCode.ToString().Length <= 0)
                    throw new Exception("FeeTypeCode Is Required!");
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<FeeTypesEn> GetFeeDetails(FeeTypesEn argEn)
        {
            try
            {
                FeeTypesDAL loDs = new FeeTypesDAL();
                return loDs.GetFeeDetails(argEn);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<KokoEn> GetKokoStudent(FeeTypesEn argEn)
        {
            try
            {
                FeeTypesDAL loDs = new FeeTypesDAL();
                return loDs.GetKokoStudent(argEn);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }

}
