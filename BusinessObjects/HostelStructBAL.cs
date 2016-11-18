using System;
using System.Text;
using System.Transactions;
using System.Collections.Generic;
using HTS.SAS.Entities;
using HTS.SAS.DataAccessObjects;

namespace HTS.SAS.BusinessObjects
{
    /// <summary>
    /// Business Class to handle all the HostelStructure Methods.
    /// </summary>
    public class HostelStructBAL
    {
        /// <summary>
        /// Method to Get List of HostelStructure
        /// </summary>
        /// <param name="argEn">HostelStructure Entity is an Input.</param>
        /// <returns>Returns List of HostelStructure</returns>
        public List<HostelStructEn> GetList(HostelStructEn argEn)
        {
            try
            {
                HostelStructDAL loDs = new HostelStructDAL();
                return loDs.GetList(argEn);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Method to Get List of HostelSructure Feelist
        /// </summary>
        /// <param name="argEn">HostelStructure Entity is an Input.HostelStructureCode,Code,Block and Status are Input Properties.</param>
        /// <returns>Returns List of HostelStructure</returns>
        public List<HostelStructEn> GetHostelFeeList(HostelStructEn argEn)
        {
            try
            {
                HostelStructDAL loDs = new HostelStructDAL();
                return loDs.GetHostelFeeList(argEn);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Method to Get List of Active or Inactive Hostels
        /// </summary>
        /// <param name="argEn">HostelStructure Entity is an Input.RoomType,Code,Block and Status are Input Properties.</param>
        /// <returns>Returns List of HostelStructure</returns>
        public List<HostelStructEn> GetHostelList(HostelStructEn argEn)
        {
            try
            {
                HostelStructDAL loDs = new HostelStructDAL();
                return loDs.GetHostelList(argEn);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Method to Get HostelStructucture Entity
        /// </summary>
        /// <param name="argEn">HostelStructucture Entity is an Input.HostelStuctureCode as Input Property.</param>
        /// <returns>Returns HostelStructucture Entity</returns>
        public HostelStructEn GetItem(HostelStructEn argEn)
        {
            try
            {
                HostelStructDAL loDs = new HostelStructDAL();
                return loDs.GetItem(argEn);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Method to Insert HostelStructure 
        /// </summary>
        /// <param name="argEn">HostelStructure Entity is an Input.</param>
        /// <returns>Returns Boolean</returns>
        public bool Insert(HostelStructEn argEn)
        {
            bool flag;
            using (TransactionScope ts = new TransactionScope())
            {
                try
                {
                    HostelStructDAL loDs = new HostelStructDAL();
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
        /// Method to Update HostelStructure 
        /// </summary>
        /// <param name="argEn">HostelStructure Entity is an Input.</param>
        /// <returns>Returns Boolean</returns>
        public bool Update(HostelStructEn argEn)
        {
            bool flag;
            using (TransactionScope ts = new TransactionScope())
            {
                try
                {
                    HostelStructDAL loDs = new HostelStructDAL();
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
        /// Method to Delete HostelStructure 
        /// </summary>
        /// <param name="argEn">HostelStructure Entity is an Input.</param>
        /// <returns>Returns Boolean</returns>
        public bool Delete(HostelStructEn argEn)
        {
            bool flag;
            using (TransactionScope ts = new TransactionScope())
            {
                try
                {
                    HostelStructDAL loDs = new HostelStructDAL();
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
        /// <param name="argEn">HostelStructure Entity is as Input.</param>
        /// <returns>Returns a Boolean</returns>
        public bool IsValid(HostelStructEn argEn)
        {
            try
            {
                if (argEn.HostelStructureCode == null || argEn.HostelStructureCode.ToString().Length <= 0)
                    throw new Exception("HostelStructureCode Is Required!");
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Method to Get List of Active or Inactive Hostels
        /// </summary>
        /// <param name="argEn">HostelStructure Entity is an Input. Code,Block and Status are Input Properties.</param>
        /// <returns>Returns List of HostelStructure</returns>
        public List<HostelStructEn> GetHostelStructList(HostelStructEn argEn)
        {
            try
            {
                HostelStructDAL loDs = new HostelStructDAL();
                return loDs.GetHostelStructList(argEn);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Method to Get List of Hostel Fee On Student
        /// </summary>
        /// <param name="argEn">Koko Charges Entity is an Input.</param>
        /// <returns>Returns List of FeeTypes</returns>
        public List<HostelStructEn> GetHostelStudent(HostelStructEn argEn)
        {
            try
            {
                HostelStructDAL loDs = new HostelStructDAL();
                return loDs.GetHostelStudent(argEn);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }

}