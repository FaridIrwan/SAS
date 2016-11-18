using System;
using System.Text;
using System.Transactions;
using System.Collections.Generic;
using HTS.SAS.Entities;
using HTS.SAS.DataAccessObjects;

namespace HTS.SAS.BusinessObjects
{
    /// <summary>
    /// Business Class to handle all the Block Methods.
    /// </summary>
    public class DepartmentBAL
    {
        /// <summary>
        /// Method to Get List of Blocks
        /// </summary>
        /// <param name="argEn">Department Entity is an Input.</param>
        /// <returns>Returns List of Dept</returns>
        public List<BlockEn> GetList(BlockEn argEn)
        {
            try
            {
                BlockDAL loDs = new BlockDAL();
                return loDs.GetList(argEn);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Method to Get List of All Block
        /// </summary>
        /// <param name="argEn">Block Entity is an Input.</param>
        /// <returns>Returns List of Block</returns>
        public List<BlockEn> GetBlockListall(BlockEn argEn)
        {
            try
            {
                BlockDAL loDs = new BlockDAL();
                return loDs.GetBlockListall(argEn);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Method to Get List of Blocks by BlockCode
        /// </summary>
        /// <param name="argEn">BlockCode is an Input.</param>
        /// <returns>Returns List of Blocks</returns>
        public List<BlockEn> GetBlockList(string argEn)
        {
            try
            {
                BlockDAL loDs = new BlockDAL();
                return loDs.GetBlockList(argEn);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Method to Get Block Entity
        /// </summary>
        /// <param name="argEn">Block Entity is an Input</param>
        /// <returns>Returns Block Entity</returns>
        public BlockEn GetItem(BlockEn argEn)
        {
            try
            {
                BlockDAL loDs = new BlockDAL();
                return loDs.GetItem(argEn);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Method to Insert Blocks 
        /// </summary>
        /// <param name="argEn">Block Entity is an Input.</param>
        /// <returns>Returns Boolean</returns>
        public bool Insert(BlockEn argEn)
        {
            bool flag;
            using (TransactionScope ts = new TransactionScope())
            {
                try
                {
                    BlockDAL loDs = new BlockDAL();
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
        /// Method to Update Blocks 
        /// </summary>
        /// <param name="argEn">Block Entity is an Input.</param>
        /// <returns>Returns Boolean</returns>
        public bool Update(BlockEn argEn)
        {
            bool flag;
            using (TransactionScope ts = new TransactionScope())
            {
                try
                {
                    BlockDAL loDs = new BlockDAL();
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
        /// Method to Delete Blocks 
        /// </summary>
        /// <param name="argEn">Block Entity is an Input.</param>
        /// <returns>Returns Boolean</returns>
        public bool Delete(BlockEn argEn)
        {
            bool flag;
            using (TransactionScope ts = new TransactionScope())
            {
                try
                {
                    BlockDAL loDs = new BlockDAL();
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
        /// <param name="argEn">Block Entity as Input.</param>
        /// <returns>Returns a Boolean</returns>
        public bool IsValid(BlockEn argEn)
        {
            try
            {
                if (argEn.SABK_Code == null || argEn.SABK_Code.ToString().Length <= 0)
                    throw new Exception("SABK_Code Is Required!");
                if (argEn.SAKO_Code == null || argEn.SAKO_Code.ToString().Length <= 0)
                    throw new Exception("SAKO_Code Is Required!");
                if (argEn.SABK_Description == null || argEn.SABK_Description.ToString().Length <= 0)
                    throw new Exception("SABK_Description Is Required!");
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }

}
