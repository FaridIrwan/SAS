using System;
using System.Text;
using System.Transactions;
using System.Collections.Generic;
using HTS.SAS.Entities;
using HTS.SAS.DataAccessObjects;

namespace HTS.SAS.BusinessObjects
{
    public class BidangBAL
    {
        #region GetList

        public List<BidangEn> GetList(BidangEn argEn)
        {
            try
            {
                BidangDAL loDs = new BidangDAL();
                return loDs.GetList(argEn);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region GetBidangList

        public List<BidangEn> GetBidangList(BidangEn argEn)
        {
            try
            {
                BidangDAL loDs = new BidangDAL();
                return loDs.GetBidangList(argEn);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        public BidangEn GetSelectedBidang(string argEn)
        {
            try
            {
                BidangDAL loDs = new BidangDAL();
                return loDs.GetSelectedBidang(argEn);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #region Insert Bidang

        public bool Insert(BidangEn argEn)
        {
            bool flag;
            using (TransactionScope ts = new TransactionScope())
            {
                try
                {
                    BidangDAL loDs = new BidangDAL();
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
        
        #endregion

        #region Update

        public bool Update(BidangEn argEn)
        {
            bool flag;
            using (TransactionScope ts = new TransactionScope())
            {
                try
                {
                    BidangDAL loDs = new BidangDAL();
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

        #endregion

        #region Delete

        public bool Delete(BidangEn argEn)
        {
            bool flag;
            using (TransactionScope ts = new TransactionScope())
            {
                try
                {
                    BidangDAL loDs = new BidangDAL();
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

        #endregion

    }
}
