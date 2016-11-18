using System;
using System.Text;
using System.Transactions;
using System.Collections.Generic;
using HTS.SAS.Entities;
using HTS.SAS.DataAccessObjects;

namespace HTS.SAS.BusinessObjects
{
    /// <summary>
    /// Business Class to handle all the ProgramInfo Methods.
    /// </summary>
    public class ProgramInfoBAL
    {
        /// <summary>
        /// Method to Get List for all record
        /// </summary>
        /// <param name="argEn">SAPG_ProgramType as Input if not null.</param>
        /// <returns>Returns List of ProgramInfo</returns>
        public List<ProgramInfoEn> GetProgramListByDegreeType(DegreeTypeEn argEn)
        {
            try
            {
                ProgramInfoDAL loDs = new ProgramInfoDAL();
                return loDs.GetProgramListByDegreeType(argEn);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<StudentEn> GetProgramInfoListAllMatricNo(AFCEn argEn)
        {
            try
            {
                ProgramInfoDAL loDs = new ProgramInfoDAL();
                return loDs.GetProgramInfoListAllMatricNo(argEn);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<StudentEn> GetAllMatricNoForPosting(AFCEn argEn)
        {
            try
            {
                ProgramInfoDAL loDs = new ProgramInfoDAL();
                return loDs.GetAllMatricNoForPosting(argEn);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Method to Get List of Active ProgramInfo
        /// </summary>
        /// <param name="argEn">SAFC_Code as Input.</param>
        /// <returns>Returns List of ProgramInfo</returns>
        public List<ProgramInfoEn> GetProgramList(string argEn)
        {
            try
            {
                ProgramInfoDAL loDs = new ProgramInfoDAL();
                return loDs.GetProgramList(argEn);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<ProgramInfoEn> GetProgramList(string argEn, bool IsBM)
        {
            try
            {
                ProgramInfoDAL loDs = new ProgramInfoDAL();
                return loDs.GetProgramList(argEn,IsBM);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Method to Get Programs
        /// </summary>
        /// <param name="argEn">ProgramInfo Entity as Input.SAFC_Code as Input Property.</param>
        /// <returns>Returns List Of Programs</returns>
        public List<ProgramInfoEn> GetList(ProgramInfoEn argEn)
        {
            try
            {
                ProgramInfoDAL loDs = new ProgramInfoDAL();
                return loDs.GetList(argEn);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Method to Get List of AFC Programs
        /// </summary>
        /// <param name="argEn">ProgramInfo Entity as an Input.SAFC_Code  as Input Property.</param>
        /// <returns>Returns List of ProgramInfo</returns> 
        public List<ProgramInfoEn> GetAfcPrograms(ProgramInfoEn argEn)
        {
            try
            {
                ProgramInfoDAL loDs = new ProgramInfoDAL();
                return loDs.GetAfcPrograms(argEn);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Method to Get List of AFC Programs By Bidang
        /// </summary>
        /// <param name="argEn">ProgramInfo Entity as an Input.SAFC_Code  as Input Property.</param>
        /// <returns>Returns List of ProgramInfo</returns> 
        public List<ProgramInfoEn> GetAfcProgramsByBidang(ProgramInfoEn argEn)
        {
            try
            {
                ProgramInfoDAL loDs = new ProgramInfoDAL();
                return loDs.GetAfcProgramsByBidang(argEn);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Method to Get List of All ProgramInfo
        /// </summary>
        /// <param name="argEn">ProgramInfo Entity as an Input.ProgramCode,Program and ProgramBM  as Input Properties.</param>
        /// <returns>Returns List of ProgramInfo</returns>
        public List<ProgramInfoEn> GetProgramInfoAll(ProgramInfoEn argEn)
        {
            try
            {
                ProgramInfoDAL loDs = new ProgramInfoDAL();
                return loDs.GetProgramInfoAll(argEn);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Method to Get List of ProgramInfo by SAFC_Code
        /// </summary>
        /// <param name="argEn">ProgramInfo Entity as an Input.SAFC_Code  as Input Property.</param>
        /// <returns>Returns List of ProgramInfo</returns>
        public List<ProgramInfoEn> GetProgramInfoListAll(string argEn)
        {
            try
            {
                ProgramInfoDAL loDs = new ProgramInfoDAL();
                return loDs.GetProgramInfoListAll(argEn);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Method to Get List of Active or Inactive ProgramInfo
        /// </summary>
        /// <param name="argEn">ProgramInfo Entity as an Input.ProgramCode,Program,ProgramBM and Status as Input Properties.</param>
        /// <returns>Returns List of ProgramInfo</returns>
        public List<ProgramInfoEn> GetProgramInfoList(ProgramInfoEn argEn)
        {
            try
            {
                ProgramInfoDAL loDs = new ProgramInfoDAL();
                return loDs.GetProgramInfoList(argEn);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Method to Get ProgramInfo Entity
        /// </summary>
        /// <param name="argEn">ProgramInfo Entity is an Input.SAPG_SName as Input Property.</param>
        /// <returns>Returns ProgramInfo Entity</returns>
        public ProgramInfoEn GetItem(ProgramInfoEn argEn)
        {
            try
            {
                ProgramInfoDAL loDs = new ProgramInfoDAL();
                return loDs.GetItem(argEn);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Method to Insert ProgramInfo 
        /// </summary>
        /// <param name="argEn">ProgramInfo Entity is an Input.</param>
        /// <returns>Returns Boolean</returns>
        public bool Insert(ProgramInfoEn argEn)
        {
            bool flag;
            using (TransactionScope ts = new TransactionScope())
            {
                try
                {
                    ProgramInfoDAL loDs = new ProgramInfoDAL();
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
        /// Method to Update ProgramInfo 
        /// </summary>
        /// <param name="argEn">ProgramInfo Entity is an Input.</param>
        /// <returns>Returns Boolean</returns>
        public bool Update(ProgramInfoEn argEn)
        {
            bool flag;
            using (TransactionScope ts = new TransactionScope())
            {
                try
                {
                    ProgramInfoDAL loDs = new ProgramInfoDAL();
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
        /// Method to Delete ProgramInfo 
        /// </summary>
        /// <param name="argEn">ProgramInfo Entity is an Input.</param>
        /// <returns>Returns Boolean</returns>
        public bool Delete(ProgramInfoEn argEn)
        {
            bool flag;
            using (TransactionScope ts = new TransactionScope())
            {
                try
                {
                    ProgramInfoDAL loDs = new ProgramInfoDAL();
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
        /// <param name="argEn">ProgramInfo Entity is as Input.</param>
        /// <returns>Returns a Boolean</returns>
        public bool IsValid(ProgramInfoEn argEn)
        {
            try
            {
                if (argEn.ProgramCode == null || argEn.ProgramCode.ToString().Length <= 0)
                    throw new Exception("ProgramCode Is Required!");
                if (argEn.Program == null || argEn.Program.ToString().Length <= 0)
                    throw new Exception("Program Is Required!");
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }

}
