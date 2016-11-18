using System;
using System.Text;
using System.Transactions;
using System.Collections.Generic;
using HTS.SAS.Entities;
using HTS.SAS.DataAccessObjects;

namespace HTS.SAS.BusinessObjects
{
    /// <summary>
    /// Business Class to handle all the Kolej Methods.
    /// </summary>
    public class KolejBAL
    {
        /// <summary>
        /// Method to Get List of Kolej
        /// </summary>
        /// <param name="argEn">Kolej Entity is an Input.</param>
        /// <returns>Returns List of Kolej</returns>
        public List<KolejEn> GetList(KolejEn argEn)
        {
            try
            {
                KolejDAL loDs = new KolejDAL();
                return loDs.GetList(argEn);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Method to Get List of Kolej
        /// </summary>
        /// <param name="argEn">Kolej Entity is an Input.</param>
        /// <returns>Returns List of Kolej</returns>
        public List<KolejEn> GetListKolej(KolejEn argEn)
        {
            try
            {
                KolejDAL loDs = new KolejDAL();
                return loDs.GetListKolej(argEn);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Method to Get List of Kolej
        /// </summary>
        /// <param name="argEn">Kolej Entity is an Input.</param>
        /// <returns>Returns List of Kolej</returns>
        public List<KolejEn> GetListKokorikulum(KolejEn argEn)
        {
            try
            {
                KolejDAL loDs = new KolejDAL();
                return loDs.GetListKokorikulum(argEn);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Method to Get List of All Kolej
        /// </summary>
        /// <param name="argEn">Kolej Entity is an Input.</param>
        /// <returns>Returns List of Kolej</returns>
        public List<KolejEn> GetKolejList(KolejEn argEn)
        {
            try
            {
                KolejDAL loDs = new KolejDAL();
                return loDs.GetKolejList(argEn);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Method to Get Kolej Entity
        /// </summary>
        /// <param name="argEn">Kolej Entity is an Input.SAKO_Code as Input Property.</param>
        /// <returns>Returns Kolej Entity</returns>
        public KolejEn GetItem(KolejEn argEn)
        {
            try
            {
                KolejDAL loDs = new KolejDAL();
                return loDs.GetItem(argEn);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Method to Insert Kolej 
        /// </summary>
        /// <param name="argEn">Kolej Entity is an Input.</param>
        /// <returns>Returns Boolean</returns>
        public bool Insert(KolejEn argEn)
        {
            bool flag;
            using (TransactionScope ts = new TransactionScope())
            {
                try
                {
                    KolejDAL loDs = new KolejDAL();
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
        /// Method to Update Kolej 
        /// </summary>
        /// <param name="argEn">Kolej Entity is an Input.</param>
        /// <returns>Returns Boolean</returns>
        public bool Update(KolejEn argEn)
        {
            bool flag;
            using (TransactionScope ts = new TransactionScope())
            {
                try
                {
                    KolejDAL loDs = new KolejDAL();
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
        /// Method to Delete Kolej 
        /// </summary>
        /// <param name="argEn">Kolej Entity is an Input.</param>
        /// <returns>Returns Boolean</returns>
        public bool Delete(KolejEn argEn)
        {
            bool flag;
            using (TransactionScope ts = new TransactionScope())
            {
                try
                {
                    KolejDAL loDs = new KolejDAL();
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
        /// <param name="argEn">Kolej Entity is as Input.</param>
        /// <returns>Returns a Boolean</returns>
        public bool IsValid(KolejEn argEn)
        {
            try
            {
                if (argEn.SAKO_Code == null || argEn.SAKO_Code.ToString().Length <= 0)
                    throw new Exception("SAKO_Code Is Required!");
                if (argEn.SAKO_Description == null || argEn.SAKO_Description.ToString().Length <= 0)
                    throw new Exception("SAKO_Description Is Required!");
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }

}
