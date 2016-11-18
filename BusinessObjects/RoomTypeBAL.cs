using System;
using System.Text;
using System.Transactions;
using System.Collections.Generic;
using HTS.SAS.Entities;
using HTS.SAS.DataAccessObjects;

namespace HTS.SAS.BusinessObjects
{
    /// <summary>
    /// Business Class to handle all the RoomType Methods.
    /// </summary>
    public class RoomTypeBAL
    {
        /// <summary>
        /// Method to Get List of RoomType
        /// </summary>
        /// <param name="argEn">RoomType Entity as an Input.</param>
        /// <returns>Returns List of RoomType</returns>
        public List<RoomTypeEn> GetList(RoomTypeEn argEn)
        {
            try
            {
                RoomTypeDAL loDs = new RoomTypeDAL();
                return loDs.GetList(argEn);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Method to Get List of All RoomType
        /// </summary>
        /// <param name="argEn">RoomType Entity is an Input.</param>
        /// <returns>Returns List of RoomType</returns>
        public List<RoomTypeEn> GetRoomTypeListall(RoomTypeEn argEn)
        {
            try
            {
                RoomTypeDAL loDs = new RoomTypeDAL();
                return loDs.GetRoomTypeListall(argEn);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Method to Get List of RoomType by SART_Code
        /// </summary>
        /// <param name="argEn">SABK_Code as Input.</param>
        /// <returns>Returns List of RoomType</returns>
        public List<RoomTypeEn> GetRoomTypeList(string argEn)
        {
            try
            {
                RoomTypeDAL loDs = new RoomTypeDAL();
                return loDs.GetRoomTypeList(argEn);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Method to Get RoomType Entity
        /// </summary>
        /// <param name="argEn">RoomType Entity is an Input.SART_Code as Input Property.</param>
        /// <returns>Returns RoomType Entity</returns>
        public RoomTypeEn GetItem(RoomTypeEn argEn)
        {
            try
            {
                RoomTypeDAL loDs = new RoomTypeDAL();
                return loDs.GetItem(argEn);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Method to Insert RoomType 
        /// </summary>
        /// <param name="argEn">RoomType Entity is an Input.</param>
        /// <returns>Returns Boolean</returns>
        public bool Insert(RoomTypeEn argEn)
        {
            bool flag;
            using (TransactionScope ts = new TransactionScope())
            {
                try
                {
                    RoomTypeDAL loDs = new RoomTypeDAL();
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
        /// Method to Update RoomType 
        /// </summary>
        /// <param name="argEn">RoomType Entity is an Input.</param>
        /// <returns>Returns Boolean</returns>
        public bool Update(RoomTypeEn argEn)
        {
            bool flag;
            using (TransactionScope ts = new TransactionScope())
            {
                try
                {
                    RoomTypeDAL loDs = new RoomTypeDAL();
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
        /// Method to Delete RoomType 
        /// </summary>
        /// <param name="argEn">RoomType Entity is an Input.</param>
        /// <returns>Returns Boolean</returns>
        public bool Delete(RoomTypeEn argEn)
        {
            bool flag;
            using (TransactionScope ts = new TransactionScope())
            {
                try
                {
                    RoomTypeDAL loDs = new RoomTypeDAL();
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
        /// <param name="argEn">RoomType Entity is as Input.</param>
        /// <returns>Returns a Boolean</returns>
        public bool IsValid(RoomTypeEn argEn)
        {
            try
            {
                if (argEn.SART_Code == null || argEn.SART_Code.ToString().Length <= 0)
                    throw new Exception("SART_Code Is Required!");
                if (argEn.SABK_Code == null || argEn.SABK_Code.ToString().Length <= 0)
                    throw new Exception("SABK_Code Is Required!");
                if (argEn.SART_Description == null || argEn.SART_Description.ToString().Length <= 0)
                    throw new Exception("SART_Description Is Required!");
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }

}
