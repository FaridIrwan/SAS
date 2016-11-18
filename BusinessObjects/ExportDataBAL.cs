using System;
using System.Text;
using System.Collections.Generic;
using System.Transactions;
using HTS.SAS.Entities;
using HTS.SAS.DataAccessObjects;

namespace HTS.SAS.BusinessObjects
{
    public class ExportDataBAL
    {

        /// <summary>
        /// Getlist  SAS_ExportData Data...
        /// <summary>
        /// <param name=sender></param>
        /// <param name= e></param>
        public List<ExportDataEN> GetList(ExportDataEN argEn)
        {
            try
            {
                ExportDataDAL loDs = new ExportDataDAL();
                return loDs.GetList(argEn);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// GetItem  SAS_ExportData Data...
        /// <summary>
        /// <param name=sender></param>
        /// <param name= e></param>

        public ExportDataEN GetItem(ExportDataEN argEn)
        {
            try
            {
                ExportDataDAL loDs = new ExportDataDAL();
                return loDs.GetItem(argEn);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Insert  SAS_ExportData Data...
        /// <summary>
        /// <param name=sender></param>
        /// <param name= e></param>

        public bool Insert(ExportDataEN argEn)
        {
            bool flag;
            try
                {
                    ExportDataDAL loDs = new ExportDataDAL();
                    return  loDs.Insert(argEn);
                }
                catch (Exception ex)
                {
                    
                    throw ex;
                }
        }
        /// <summary>
        /// Update  SAS_ExportData Data...
        /// <summary>
        /// <param name=sender></param>
        /// <param name= e></param>

        public bool Update(ExportDataEN argEn)
        {
            //bool flag;
            //using (TransactionScope ts = new TransactionScope())
            //{
                try
                {
                    ExportDataDAL loDs = new ExportDataDAL();
                    return loDs.Update(argEn);
                    //ts.Complete();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            //}
            //return flag;
        }
        /// <summary>
        /// Delete  SAS_ExportData Data...
        /// <summary>
        /// <param name=sender></param>
        /// <param name= e></param>

        public bool Delete(ExportDataEN argEn)
        {
            bool flag;
            using (TransactionScope ts = new TransactionScope())
            {
                try
                {
                    ExportDataDAL loDs = new ExportDataDAL();
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
        /// IsValid  SAS_ExportData Data...
        /// <summary>
        /// <param name=sender></param>
        /// <param name= e></param>

        public bool IsValid(ExportDataEN argEn)
        {
            try
            {
                if (argEn.InterfaceID == null || argEn.InterfaceID.ToString().Length <= 0)
                    throw new Exception("InterfaceID Is Required!");
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }

}
