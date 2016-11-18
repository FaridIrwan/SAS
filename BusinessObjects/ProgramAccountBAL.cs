using System;
using System.Text;
using System.Transactions;
using System.Collections.Generic;
using HTS.SAS.Entities;
using HTS.SAS.DataAccessObjects;

namespace HTS.SAS.BusinessObjects
{
    /// <summary>
    /// Business Class to handle all the ProgramAccount Methods.
    /// </summary>
    public class ProgramAccountBAL
    {
        /// <summary>
        /// Method to Get List of ProgramAccount
        /// </summary>
        /// <param name="argEn">ProgramAccount Entity is an Input.</param>
        /// <returns>Returns List of ProgramAccount</returns>
        public List<ProgramAccountEn> GetList(ProgramAccountEn argEn)
        {
            try
            {
                ProgramAccountDAL loDs = new ProgramAccountDAL();
                return loDs.GetList(argEn);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Method to Get List of ProgramAccount
        /// </summary>
        /// <param name="argEn">ProgramAccount Entity is an Input.</param>
        /// <returns>Returns List of ProgramAccount</returns>
        public List<ProgramAccountEn> GetListProgram(ProgramAccountEn argEn)
        {
            try
            {
                ProgramAccountDAL loDs = new ProgramAccountDAL();
                return loDs.GetListProgram(argEn);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Method to Get List of Program,Faculty And Semester
        /// </summary>
        /// <param name="argEn">Program,Faculty And Semester Entity is an Output.</param>
        /// <returns>Returns List of ProgramAccount</returns>
        public List<ProgramAccountEn> GetListProgramCombine(ProgramAccountEn argEn)
        {
            try
            {
                ProgramAccountDAL loDs = new ProgramAccountDAL();
                return loDs.GetListProgramCombine(argEn);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Method to Get ProgramAccount Entity
        /// </summary>
        /// <param name="argEn">ProgramAccount Entity is an Input.SAPA_Code as Input Property.</param>
        /// <returns>Returns ProgramAccount Entity</returns>
        public ProgramAccountEn GetItem(ProgramAccountEn argEn)
        {
            try
            {
                ProgramAccountDAL loDs = new ProgramAccountDAL();
                return loDs.GetItem(argEn);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Method to Insert ProgramAccount 
        /// </summary>
        /// <param name="argEn">ProgramAccount Entity is an Input.</param>
        /// <returns>Returns Boolean</returns>
        public bool Insert(ProgramAccountEn argEn)
        {
            bool flag;
            using (TransactionScope ts = new TransactionScope())
            {
                try
                {
                    ProgramAccountDAL loDs = new ProgramAccountDAL();
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
        /// Method to Update ProgramAccount 
        /// </summary>
        /// <param name="argEn">ProgramAccount Entity is an Input.</param>
        /// <returns>Returns Boolean</returns>
        public bool Update(ProgramAccountEn argEn)
        {
            bool flag;
            using (TransactionScope ts = new TransactionScope())
            {
                try
                {
                    ProgramAccountDAL loDs = new ProgramAccountDAL();
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
        /// Method to Delete ProgramAccount 
        /// </summary>
        /// <param name="argEn">ProgramAccount Entity is an Input.</param>
        /// <returns>Returns Boolean</returns>
        public bool Delete(ProgramAccountEn argEn)
        {
            bool flag;
            using (TransactionScope ts = new TransactionScope())
            {
                try
                {
                    ProgramAccountDAL loDs = new ProgramAccountDAL();
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
        /// <param name="argEn">ProgramAccount Entity is as Input.</param>
        /// <returns>Returns a Boolean</returns>
        public bool IsValid(ProgramAccountEn argEn)
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
//---------------------------------------------------------------------------------

//---------------------------------------------------------------------------------