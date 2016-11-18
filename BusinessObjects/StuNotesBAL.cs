using System;
using System.Text;
using System.Transactions;
using System.Collections.Generic;
using HTS.SAS.Entities;
using HTS.SAS.DataAccessObjects;

namespace HTS.SAS.BusinessObjects
{
    /// <summary>
    /// Business Class to handle all the StudentNotes Methods.
    /// </summary>
    public class StuNotesBAL
    {
        /// <summary>
        /// Method to Get List of StudentNotes
        /// </summary>
        /// <param name="argEn">StudentNotes Entity as an Input.Student MatricNo as Input Property</param>
        /// <returns>Returns List of StudentNotes</returns>
        public List<StuNotesEn> GetList(StuNotesEn argEn)
        {
            try
            {
                StuNotesDAL loDs = new StuNotesDAL();
                return loDs.GetList(argEn);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Method to Get StudentNotes Entity
        /// </summary>
        /// <param name="argEn">StudentNotes Entity is an Input.MatricNo as Input Property.</param>
        /// <returns>Returns StudentNotes Entity</returns>
        public StuNotesEn GetItem(StuNotesEn argEn)
        {
            try
            {
                StuNotesDAL loDs = new StuNotesDAL();
                return loDs.GetItem(argEn);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Method to Insert StudentNotes
        /// </summary>
        /// <param name="argEn">StudentNotes Entity is an Input.</param>
        /// <returns>Returns Boolean</returns> 
        public bool Insert(StuNotesEn argEn)
        {
            bool flag;
            using (TransactionScope ts = new TransactionScope())
            {
                try
                {
                    StuNotesDAL loDs = new StuNotesDAL();
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
        /// Method to Update StudentNotes
        /// </summary>
        /// <param name="argEn">StudentNotes Entity is an Input.</param>
        /// <returns>Returns Boolean</returns> 
        public bool Update(StuNotesEn argEn)
        {
            bool flag;
            using (TransactionScope ts = new TransactionScope())
            {
                try
                {
                    StuNotesDAL loDs = new StuNotesDAL();
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
        /// Method to Delete StudentNotes
        /// </summary>
        /// <param name="argEn">StudentNotes Entity is an Input.MatricNo as Input Property.</param>
        /// <returns>Returns Boolean</returns> 
        public bool Delete(StuNotesEn argEn)
        {
            bool flag;
            using (TransactionScope ts = new TransactionScope())
            {
                try
                {
                    StuNotesDAL loDs = new StuNotesDAL();
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
        /// <param name="argEn">StudentNotes Entity is as Input.</param>
        /// <returns>Returns a Boolean</returns>
        public bool IsValid(StuNotesEn argEn)
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

