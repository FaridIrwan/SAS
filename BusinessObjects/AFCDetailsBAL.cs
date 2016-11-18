using System;
using System.Text;
using System.Collections.Generic;
using HTS.SAS.Entities;
using HTS.SAS.DataAccessObjects;
using System.Transactions;
namespace HTS.SAS.BusinessObjects
{
    /// <summary>
    /// Business Class to handle all the AFCDetails Transactions.
    /// </summary>
    public class AFCDetailsBAL
    {
        /// <summary>
        /// Method to Get List of AFCDetails
        /// </summary>
        /// <param name="argEn">AFCDetails Entity is an Input.</param>
        /// <returns>Returns List of AFCDetails Entities</returns>
        public List<AFCDetailsEn> GetList(AFCDetailsEn argEn)
        {
            try
            {
                AFCDetailsDS loDs = new AFCDetailsDS();
                return loDs.GetList(argEn);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Method to Get an AFCDetails Entity
        /// </summary>
        /// <param name="argEn">AFCDetails Entity is an Input</param>
        /// <returns>Returns AFCDetails Entity</returns>
        public AFCDetailsEn GetItem(AFCDetailsEn argEn)
        {
            try
            {
                AFCDetailsDS loDs = new AFCDetailsDS();
                return loDs.GetItem(argEn);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Method to Insert 
        /// </summary>
        /// <param name="argEn">AFCetails Entity is an Input.</param>
        /// <returns>Returns Boolean</returns>
        public bool Insert(AFCDetailsEn argEn)
        {
                try
                {
                    AFCDetailsDS loDs = new AFCDetailsDS();
                    return loDs.Insert(argEn);
                }
                catch (Exception ex)
                {
                    
                    throw ex;
                }
        }
        /// <summary>
        /// Method to Update 
        /// </summary>
        /// <param name="argEn">AFCDetails Entity is an Input.</param>
        /// <returns>Returns Boolean</returns>
        public bool Update(AFCDetailsEn argEn)
        {
                try
                {
                    AFCDetailsDS loDs = new AFCDetailsDS();
                    return loDs.Update(argEn);
                }
                catch (Exception ex)
                {
                    
                    throw ex;
                }
        }
        /// <summary>
        /// Method to Delete 
        /// </summary>
        /// <param name="argEn">AFCDetails Entity is an Input.</param>
        /// <returns>Returns Boolean</returns>
        public bool Delete(AFCDetailsEn argEn)
        {
            try
            {
                AFCDetailsDS loDs = new AFCDetailsDS();
                return loDs.Delete(argEn);
            }
            
            catch (Exception ex)
            {
                
                throw ex;
            }
        }
        /// <summary>
        /// Method to Check Validation
        /// </summary>
        /// <param name="argEn">AFCDetails Entity as Input.</param>
        /// <returns>Returns a Boolean</returns>
        public bool IsValid(AFCDetailsEn argEn)
        {
            try
            {
                if (argEn.TransCode == null || argEn.TransCode.ToString().Length <= 0)
                    throw new Exception("TransCode Is Required!");
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }

}