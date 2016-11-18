using System;
using System.Text;
using System.Transactions;
using System.Collections.Generic;
using HTS.SAS.Entities;
using HTS.SAS.DataAccessObjects;


namespace HTS.SAS.BusinessObjects
{
    public class WorkflowSetupBAL
    {

        #region Insert
        /// <summary>
        /// Method to Insert WorkflowSetup 
        /// </summary>
        /// <param name="argEn">WorkflowSetup Entity is an Input.</param>
        /// <returns>Returns Boolean</returns>
        public bool Insert(WorkflowSetupEn argEn)
        {
            bool flag;
            using (TransactionScope ts = new TransactionScope())
            {
                try
                {
                    WorkflowSetupDAL loDs = new WorkflowSetupDAL();
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
        //<summary>
        //Method to Update WorkflowSetupEn 
        //</summary>
        //<param name="argEn">WorkflowSetupEn Entity is an Input.</param>
        //<returns>Returns Boolean</returns>
        public bool Update(WorkflowSetupEn argEn)
        {
            bool flag;
            using (TransactionScope ts = new TransactionScope())
            {
                try
                {
                    WorkflowSetupDAL loDs = new WorkflowSetupDAL();
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
        /// <summary>
        /// Method to Delete WorkflowSetup 
        /// </summary>
        /// <param name="argEn">WorkflowSetup Entity is an Input.</param>
        /// <returns>Returns Boolean</returns>
        public bool Delete(WorkflowSetupEn argEn)
        {
            bool flag;
            using (TransactionScope ts = new TransactionScope())
            {
                try
                {
                    WorkflowSetupDAL loDs = new WorkflowSetupDAL();
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

        #region GetWorkflowSetupTypelistall
        /// <summary>
        /// Method to Get List of All WorkflowSetup
        /// </summary>
        /// <param name="argEn">WorkflowSetup Entity is an Input.</param>
        /// <returns>Returns List of RoomType</returns>
        public List<WorkflowSetupEn> GetWorkflowSetupTypelistall(WorkflowSetupEn argEn)
        {
            try
            {
                WorkflowSetupDAL loDs = new WorkflowSetupDAL();
                return loDs.GetWorkflowSetupTypelistall(argEn);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region GetList
        //public List<WorkflowSetupEn> GetList(WorkflowSetupEn argEn)
        //{
        //    try
        //    {
        //        WorkflowSetupDAL loDs = new WorkflowSetupDAL();
        //        return loDs.GetList(argEn);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}
        #endregion
    }
}
