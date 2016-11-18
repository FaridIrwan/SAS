#region NameSpaces

using System;
using System.Data;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data.Common;
using HTS.SAS.Entities;
using MaxGeneric;
using System.Linq;

#endregion

namespace HTS.SAS.DataAccessObjects
{
    public class WorkflowSetupDAL
    {
        #region Global Declarations

        private DbParameterCollection _DbParameterCollection = null;

        private MaxModule.DatabaseProvider _DatabaseFactory = new MaxModule.DatabaseProvider();

        private string DataBaseConnectionString = Helper.GetConnectionString();

        #endregion

        #region GetList

        public List<WorkflowSetupEn> GetList(WorkflowSetupEn argEn)
        {
            List<WorkflowSetupEn> loEnList = new List<WorkflowSetupEn>();
            string sqlCmd = "SELECT * FROM ur_workflowsetup";
            try
            {
                if (!FormHelp.IsBlank(sqlCmd))
                {
                    using (IDataReader loReader = _DatabaseFactory.ExecuteReader(Helper.GetDataBaseType,
                        DataBaseConnectionString, sqlCmd).CreateDataReader())
                    {
                        while (loReader.Read())
                        {
                            WorkflowSetupEn loItem = LoadObject(loReader);
                            loEnList.Add(loItem);
                        }
                        loReader.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return loEnList;
        }

        #endregion

        #region GetPreparerList

        public List<WorkflowSetupEn> GetPreparerList()
        {
            List<WorkflowSetupEn> loEnList = new List<WorkflowSetupEn>();

            string sqlCmd = @"SELECT * FROM ur_usermenu_preparer";
            try
            {
                if (!FormHelp.IsBlank(sqlCmd))
                {
                    using (IDataReader loReader = _DatabaseFactory.ExecuteReader(Helper.GetDataBaseType,
                        DataBaseConnectionString, sqlCmd).CreateDataReader())
                    {
                        while (loReader.Read())
                        {
                            WorkflowSetupEn obj = new WorkflowSetupEn();
                            obj.WorkflowPreparerEn = new WorkflowPreparerEn();
                            obj.WorkflowPreparerEn.WorkflowSetupId = GetValue<int>(loReader, "workflowsetup_id");
                            obj.WorkflowPreparerEn.UserId = GetValue<int>(loReader, "userid");
                            obj.WorkflowPreparerEn.MenuId = GetValue<int>(loReader, "menuid");
                            loEnList.Add(obj);
                        }
                        loReader.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return loEnList;
        }

        #endregion

        #region GetApproverList

        public List<WorkflowSetupEn> GetApproverList()
        {
            List<WorkflowSetupEn> loEnList = new List<WorkflowSetupEn>();

            string sqlCmd = @"SELECT * FROM ur_usermenu_approver";
            try
            {
                if (!FormHelp.IsBlank(sqlCmd))
                {
                    using (IDataReader loReader = _DatabaseFactory.ExecuteReader(Helper.GetDataBaseType,
                        DataBaseConnectionString, sqlCmd).CreateDataReader())
                    {
                        while (loReader.Read())
                        {
                            WorkflowSetupEn obj = new WorkflowSetupEn();
                            obj.WorkflowApproverEn = new WorkflowApproverEn();
                            obj.WorkflowApproverEn.WorkflowSetupId = GetValue<int>(loReader, "workflowsetup_id");
                            obj.WorkflowApproverEn.UserId = GetValue<int>(loReader, "userid");
                            obj.WorkflowApproverEn.MenuId = GetValue<int>(loReader, "menuid");
                            obj.WorkflowApproverEn.LowerLimit = GetValue<double>(loReader, "lower_limit");
                            obj.WorkflowApproverEn.UpperLimit = GetValue<double>(loReader, "upper_limit");
                            loEnList.Add(obj);
                        }
                        loReader.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return loEnList;
        }

        #endregion

        #region GetUserList

        public List<WorkflowSetupEn> GetUserList()
        {
            List<WorkflowSetupEn> loEnList = new List<WorkflowSetupEn>();

            string sqlCmd = @"SELECT * FROM ur_users users
                            INNER JOIN ur_usergroups usergroups ON usergroups.usergroupid = users.usergroupid ";
            try
            {
                if (!FormHelp.IsBlank(sqlCmd))
                {
                    using (IDataReader loReader = _DatabaseFactory.ExecuteReader(Helper.GetDataBaseType,
                        DataBaseConnectionString, sqlCmd).CreateDataReader())
                    {
                        while (loReader.Read())
                        {
                            WorkflowSetupEn obj = new WorkflowSetupEn();
                            obj.UsersEn = new UsersEn();
                            obj.UsersEn.UserID = GetValue<int>(loReader, "userid");
                            obj.UsersEn.UserName = GetValue<String>(loReader, "username");
                            obj.UsersEn.StaffName = GetValue<String>(loReader, "staff_name");
                            obj.UsersEn.StaffID = GetValue<String>(loReader, "staff_no");
                            obj.UsersEn.UserStatus = GetValue<String>(loReader, "userstatus");
                            obj.UserGroupsEn = new UserGroupsEn();
                            obj.UserGroupsEn.UserGroupName = GetValue<String>(loReader, "usergroupname");
                            loEnList.Add(obj);
                        }
                        loReader.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return loEnList;
        }

        #endregion

        #region GetViewList

        public List<WorkflowSetupEn> GetViewList()
        {
            List<WorkflowSetupEn> loEnList = new List<WorkflowSetupEn>();

            string sqlCmd = @"SELECT pagename, ur_workflowsetup.status, total_preparer TotalPreparer, total_approver TotalApprover, 
                            ur_workflowsetup.lastupdatedby, ur_workflowsetup.lastupdateddttm
                            FROM ur_workflowsetup 
                            INNER JOIN ur_menumaster ON ur_menumaster.menuid = ur_workflowsetup.menuid ";
            try
            {
                if (!FormHelp.IsBlank(sqlCmd))
                {
                    using (IDataReader loReader = _DatabaseFactory.ExecuteReader(Helper.GetDataBaseType,
                        DataBaseConnectionString, sqlCmd).CreateDataReader())
                    {
                        while (loReader.Read())
                        {
                            WorkflowSetupEn obj = new WorkflowSetupEn();
                            obj.MenuMasterEn = new MenuMasterEn();
                            obj.MenuMasterEn.PageName = GetValue<String>(loReader, "pagename");
                            obj.Status = GetValue<bool>(loReader, "status");
                            obj.TotalPreparer = GetValue<int>(loReader, "TotalPreparer");
                            obj.TotalApprover = GetValue<int>(loReader, "TotalApprover");
                            obj.LastUpdatedBy = GetValue<string>(loReader, "lastupdatedby");
                            obj.LastUpdatedDtTm = GetValue<DateTime>(loReader, "lastupdateddttm");
                            loEnList.Add(obj);
                        }
                        loReader.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return loEnList;
        }

        #endregion

        #region GetWorkflowSetupDetails

        public WorkflowSetupEn GetWorkflowSetupDetails(WorkflowSetupEn argEn)
        {
            WorkflowSetupEn loItem = new WorkflowSetupEn();

            string sqlCmd = "SELECT * FROM ur_workflowsetup WHERE menuid = " +
                            "(SELECT menuid FROM ur_menumaster WHERE pagename = " + clsGeneric.AddQuotes(argEn.MenuMasterEn.PageName) + ") " +
                            "AND status = " + argEn.Status;
            try
            {
                if (!FormHelp.IsBlank(sqlCmd))
                {

                    using (IDataReader loReader = _DatabaseFactory.ExecuteReader(Helper.GetDataBaseType,
                        DataBaseConnectionString, sqlCmd).CreateDataReader())
                    {
                        if (loReader != null)
                        {
                            loReader.Read();
                            loItem = LoadObject(loReader);
                        }
                        loReader.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return loItem;
        }

        #endregion

        #region GetWorkflowPreparerDetails

        public List<WorkflowSetupEn> GetWorkflowPreparerDetails(WorkflowSetupEn argEn)
        {
            List<WorkflowSetupEn> _lst = new List<WorkflowSetupEn>();

            string sqlCmd = @"SELECT * FROM ur_usermenu_preparer a 
                            INNER JOIN ur_users b ON b.userid = a.userid 
                            INNER JOIN ur_usergroups c ON c.usergroupid = b.usergroupid
                            WHERE workflowsetup_id = " + argEn.Id + " AND menuid = " + argEn.MenuId ;
            try
            {
                if (!FormHelp.IsBlank(sqlCmd))
                {
                    using (IDataReader loReader = _DatabaseFactory.ExecuteReader(Helper.GetDataBaseType,
                        DataBaseConnectionString, sqlCmd).CreateDataReader())
                    {
                        while (loReader.Read())
                        {
                            WorkflowSetupEn obj = new WorkflowSetupEn();
                            obj.UsersEn = new UsersEn();
                            obj.UsersEn.UserID = GetValue<int>(loReader, "userid");
                            obj.UsersEn.UserName = GetValue<String>(loReader, "username");
                            obj.UsersEn.StaffName = GetValue<String>(loReader, "staff_name");
                            obj.UsersEn.StaffID = GetValue<String>(loReader, "staff_no");
                            obj.UserGroupsEn = new UserGroupsEn();
                            obj.UserGroupsEn.UserGroupName = GetValue<String>(loReader, "usergroupname");
                            _lst.Add(obj);
                        }
                        loReader.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return _lst;
        }

        #endregion

        #region GetWorkflowApproverDetails

        public List<WorkflowSetupEn> GetWorkflowApproverDetails(WorkflowSetupEn argEn)
        {
            List<WorkflowSetupEn> _lst = new List<WorkflowSetupEn>();

            string sqlCmd = @"SELECT * FROM ur_usermenu_approver a 
                            INNER JOIN ur_users b ON b.userid = a.userid 
                            INNER JOIN ur_usergroups c ON c.usergroupid = b.usergroupid
                            WHERE workflowsetup_id = " + argEn.Id + " AND menuid = " + argEn.MenuId;
            try
            {
                if (!FormHelp.IsBlank(sqlCmd))
                {
                    using (IDataReader loReader = _DatabaseFactory.ExecuteReader(Helper.GetDataBaseType,
                        DataBaseConnectionString, sqlCmd).CreateDataReader())
                    {
                        while (loReader.Read())
                        {
                            WorkflowSetupEn obj = new WorkflowSetupEn();
                            obj.UsersEn = new UsersEn();
                            obj.UsersEn.UserID = GetValue<int>(loReader, "userid");
                            obj.UsersEn.UserName = GetValue<String>(loReader, "username");
                            obj.UsersEn.StaffName = GetValue<String>(loReader, "staff_name");
                            obj.UsersEn.StaffID = GetValue<String>(loReader, "staff_no");
                            obj.UserGroupsEn = new UserGroupsEn();
                            obj.UserGroupsEn.UserGroupName = GetValue<String>(loReader, "usergroupname");
                            obj.WorkflowApproverEn = new WorkflowApproverEn();
                            obj.WorkflowApproverEn.LowerLimit = GetValue<Double>(loReader, "lower_limit");
                            obj.WorkflowApproverEn.UpperLimit = GetValue<Double>(loReader, "upper_limit");
                            _lst.Add(obj);
                        }
                        loReader.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return _lst;
        }

        #endregion

        #region Insert

        public int Insert(WorkflowSetupEn argEn)
        {
            int cnt = 0, id = 0;
            bool result = false;

            string sqlCmd = "SELECT COUNT(*) AS cnt FROM ur_workflowsetup WHERE menuid = @menuid";

            try
            {
                if (!FormHelp.IsBlank(sqlCmd))
                {
                    DbCommand cmdSel = _DatabaseFactory.GetDbCommand(Helper.GetDataBaseType, sqlCmd, DataBaseConnectionString);
                    _DatabaseFactory.AddInParameter(ref cmdSel, "@menuid", DbType.Int16, argEn.MenuId);
                    _DbParameterCollection = cmdSel.Parameters;

                    using (IDataReader dr = _DatabaseFactory.GetIDataReader(Helper.GetDataBaseType, cmdSel,
                        DataBaseConnectionString, sqlCmd, _DbParameterCollection).CreateDataReader())
                    {
                        if (dr.Read())
                            cnt = clsGeneric.NullToInteger(dr["cnt"]);
                    }

                    if (cnt > 0)
                    {
                        sqlCmd = "UPDATE ur_workflowsetup SET total_preparer = @total_preparer, total_approver = @total_approver, status = @status, " +
                                 "LastUpdatedBy = @LastUpdatedBy, LastUpdatedDtTm = @LastUpdatedDtTm WHERE menuid = @menuid ";

                        if (!FormHelp.IsBlank(sqlCmd))
                        {
                            DbCommand cmd = _DatabaseFactory.GetDbCommand(Helper.GetDataBaseType, sqlCmd, DataBaseConnectionString);
                            _DatabaseFactory.AddInParameter(ref cmd, "@menuid", DbType.Int16, argEn.MenuId);
                            _DatabaseFactory.AddInParameter(ref cmd, "@total_preparer", DbType.Int32, argEn.TotalPreparer);
                            _DatabaseFactory.AddInParameter(ref cmd, "@total_approver", DbType.Int32, argEn.TotalApprover);
                            _DatabaseFactory.AddInParameter(ref cmd, "@Status", DbType.Boolean, argEn.Status);
                            _DatabaseFactory.AddInParameter(ref cmd, "@LastUpdatedBy", DbType.String, argEn.LastUpdatedBy);
                            _DatabaseFactory.AddInParameter(ref cmd, "@LastUpdatedDtTm", DbType.DateTime, Helper.DateConversion(argEn.LastUpdatedDtTm));
                            _DbParameterCollection = cmd.Parameters;

                            int liRowAffected = _DatabaseFactory.ExecuteNonQuery(Helper.GetDataBaseType, cmd,
                                DataBaseConnectionString, sqlCmd, _DbParameterCollection);

                            if (liRowAffected > -1)
                            {
                                result = true;
                            }
                            else
                            {
                                throw new Exception("Update Failed! No Row has been updated.");
                            }
                        }
                    }

                    if (cnt == 0)
                    {
                        sqlCmd = "INSERT INTO ur_workflowsetup(menuid, status, total_preparer, total_approver, LastUpdatedBy, LastUpdatedDtTm) " +
                                 "VALUES (@menuid, @status, @total_preparer, @total_approver, @LastUpdatedBy, @LastUpdatedDtTm)";

                        if (!FormHelp.IsBlank(sqlCmd))
                        {
                            DbCommand cmd = _DatabaseFactory.GetDbCommand(Helper.GetDataBaseType, sqlCmd, DataBaseConnectionString);
                            _DatabaseFactory.AddInParameter(ref cmd, "@menuid", DbType.Int16, argEn.MenuId);
                            _DatabaseFactory.AddInParameter(ref cmd, "@total_preparer", DbType.Int32, argEn.TotalPreparer);
                            _DatabaseFactory.AddInParameter(ref cmd, "@total_approver", DbType.Int32, argEn.TotalApprover);
                            _DatabaseFactory.AddInParameter(ref cmd, "@Status", DbType.Boolean, argEn.Status);
                            _DatabaseFactory.AddInParameter(ref cmd, "@LastUpdatedBy", DbType.String, argEn.LastUpdatedBy);
                            _DatabaseFactory.AddInParameter(ref cmd, "@LastUpdatedDtTm", DbType.DateTime, Helper.DateConversion(argEn.LastUpdatedDtTm));
                            _DbParameterCollection = cmd.Parameters;

                            int liRowAffected = clsGeneric.NullToInteger(_DatabaseFactory.ExecuteScalarCommand(Helper.GetDataBaseType, cmd,
                            DataBaseConnectionString, sqlCmd, _DbParameterCollection));

                            if (liRowAffected > -1)
                            {
                                result = true;
                            }
                            else
                            {
                                throw new Exception("Insert Failed! No Row has been inserted.");
                            }
                        }

                    }
                }

                if (result == true)
                {
                    string getId = "SELECT id as id FROM ur_workflowsetup WHERE menuid = " + argEn.MenuId;

                    try
                    {
                        if (!FormHelp.IsBlank(getId))
                        {
                            using (IDataReader loReader = _DatabaseFactory.ExecuteReader(Helper.GetDataBaseType,
                                DataBaseConnectionString, getId).CreateDataReader())
                            {
                                while (loReader.Read())
                                {
                                    id = clsGeneric.NullToInteger(loReader["id"]);
                                }
                                loReader.Close();
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return id;
        }

        #endregion

        #region Insert Into Preparer Workflow Table

        public bool InsertPreparer(WorkflowPreparerEn argEn)
        {
            int cnt = 0;
            bool result = false;

            string sqlCmd = "SELECT COUNT(*) AS cnt FROM ur_usermenu_preparer WHERE workflowsetup_id = @workflowsetup_id AND userid = @userid AND menuid = @menuid";

            try
            {
                if (!FormHelp.IsBlank(sqlCmd))
                {
                    DbCommand cmdSel = _DatabaseFactory.GetDbCommand(Helper.GetDataBaseType, sqlCmd, DataBaseConnectionString);
                    _DatabaseFactory.AddInParameter(ref cmdSel, "@workflowsetup_id", DbType.Int16, argEn.WorkflowSetupId);
                    _DatabaseFactory.AddInParameter(ref cmdSel, "@userid", DbType.Int16, argEn.UserId);
                    _DatabaseFactory.AddInParameter(ref cmdSel, "@menuid", DbType.Int16, argEn.MenuId);
                    _DbParameterCollection = cmdSel.Parameters;

                    using (IDataReader dr = _DatabaseFactory.GetIDataReader(Helper.GetDataBaseType, cmdSel,
                        DataBaseConnectionString, sqlCmd, _DbParameterCollection).CreateDataReader())
                    {
                        if (dr.Read())
                            cnt = clsGeneric.NullToInteger(dr["cnt"]);
                    }

                    if (cnt > 0)
                    {
                        return result = true;
                    }

                    if (cnt == 0)
                    {
                        sqlCmd = "INSERT INTO ur_usermenu_preparer(workflowsetup_id, userid, menuid, updatedby, updateddttm) " +
                                 "VALUES (@workflowsetup_id, @userid, @menuid, @updatedby, @updateddttm)";

                        if (!FormHelp.IsBlank(sqlCmd))
                        {
                            DbCommand cmd = _DatabaseFactory.GetDbCommand(Helper.GetDataBaseType, sqlCmd, DataBaseConnectionString);
                            _DatabaseFactory.AddInParameter(ref cmd, "@workflowsetup_id", DbType.Int16, argEn.WorkflowSetupId);
                            _DatabaseFactory.AddInParameter(ref cmd, "@userid", DbType.Int16, argEn.UserId);
                            _DatabaseFactory.AddInParameter(ref cmd, "@menuid", DbType.Int16, argEn.MenuId);
                            _DatabaseFactory.AddInParameter(ref cmd, "@updatedby", DbType.String, argEn.LastUpdatedBy);
                            _DatabaseFactory.AddInParameter(ref cmd, "@updateddttm", DbType.DateTime, Helper.DateConversion(argEn.LastUpdatedDtTm));
                            _DbParameterCollection = cmd.Parameters;

                            int liRowAffected = clsGeneric.NullToInteger(_DatabaseFactory.ExecuteScalarCommand(Helper.GetDataBaseType, cmd,
                            DataBaseConnectionString, sqlCmd, _DbParameterCollection));

                            if (liRowAffected > -1)
                            {
                                result = true;
                            }
                            else
                            {
                                throw new Exception("Insert Failed! No Row has been inserted.");
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return result;
        }

        #endregion

        #region Delete Uncheck Preparer From Workflow Table

        public bool DeletePreparer(WorkflowPreparerEn argEn)
        {
            int cnt = 0;
            bool result = false;

            string sqlCmd = "SELECT COUNT(*) AS cnt FROM ur_usermenu_preparer WHERE workflowsetup_id = @workflowsetup_id AND userid = @userid AND menuid = @menuid";

            try
            {
                if (!FormHelp.IsBlank(sqlCmd))
                {
                    DbCommand cmdSel = _DatabaseFactory.GetDbCommand(Helper.GetDataBaseType, sqlCmd, DataBaseConnectionString);
                    _DatabaseFactory.AddInParameter(ref cmdSel, "@workflowsetup_id", DbType.Int16, argEn.WorkflowSetupId);
                    _DatabaseFactory.AddInParameter(ref cmdSel, "@userid", DbType.Int16, argEn.UserId);
                    _DatabaseFactory.AddInParameter(ref cmdSel, "@menuid", DbType.Int16, argEn.MenuId);
                    _DbParameterCollection = cmdSel.Parameters;

                    using (IDataReader dr = _DatabaseFactory.GetIDataReader(Helper.GetDataBaseType, cmdSel,
                        DataBaseConnectionString, sqlCmd, _DbParameterCollection).CreateDataReader())
                    {
                        if (dr.Read())
                            cnt = clsGeneric.NullToInteger(dr["cnt"]);
                    }

                    if (cnt == 0)
                    {
                        return result = true;
                    }

                    if (cnt > 0)
                    {
                        sqlCmd = "DELETE FROM ur_usermenu_preparer WHERE workflowsetup_id = @workflowsetup_id AND userid = @userid AND menuid = @menuid";

                        if (!FormHelp.IsBlank(sqlCmd))
                        {
                            DbCommand cmd = _DatabaseFactory.GetDbCommand(Helper.GetDataBaseType, sqlCmd, DataBaseConnectionString);
                            _DatabaseFactory.AddInParameter(ref cmd, "@workflowsetup_id", DbType.Int16, argEn.WorkflowSetupId);
                            _DatabaseFactory.AddInParameter(ref cmd, "@userid", DbType.Int16, argEn.UserId);
                            _DatabaseFactory.AddInParameter(ref cmd, "@menuid", DbType.Int16, argEn.MenuId);
                            _DbParameterCollection = cmd.Parameters;

                            int liRowAffected = clsGeneric.NullToInteger(_DatabaseFactory.ExecuteScalarCommand(Helper.GetDataBaseType, cmd,
                            DataBaseConnectionString, sqlCmd, _DbParameterCollection));

                            if (liRowAffected > -1)
                            {
                                result = true;
                            }
                            else
                            {
                                throw new Exception("Delete Failed! No Row has been deleted.");
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return result;
        }

        #endregion

        #region Insert Into Approver Workflow Table

        public bool InsertApprover(WorkflowApproverEn argEn)
        {
            int cnt = 0;
            bool result = false;

            string sqlCmd = "SELECT COUNT(*) AS cnt FROM ur_usermenu_approver WHERE workflowsetup_id = @workflowsetup_id AND userid = @userid AND menuid = @menuid";

            try
            {
                if (!FormHelp.IsBlank(sqlCmd))
                {
                    DbCommand cmdSel = _DatabaseFactory.GetDbCommand(Helper.GetDataBaseType, sqlCmd, DataBaseConnectionString);
                    _DatabaseFactory.AddInParameter(ref cmdSel, "@workflowsetup_id", DbType.Int16, argEn.WorkflowSetupId);
                    _DatabaseFactory.AddInParameter(ref cmdSel, "@userid", DbType.Int16, argEn.UserId);
                    _DatabaseFactory.AddInParameter(ref cmdSel, "@menuid", DbType.Int16, argEn.MenuId);
                    _DbParameterCollection = cmdSel.Parameters;

                    using (IDataReader dr = _DatabaseFactory.GetIDataReader(Helper.GetDataBaseType, cmdSel,
                        DataBaseConnectionString, sqlCmd, _DbParameterCollection).CreateDataReader())
                    {
                        if (dr.Read())
                            cnt = clsGeneric.NullToInteger(dr["cnt"]);
                    }

                    if (cnt > 0)
                    {
                        if (argEn.LowerLimit >= 0  || argEn.UpperLimit >= 0)
                        {
                            sqlCmd = "UPDATE ur_usermenu_approver SET lower_limit = " + argEn.LowerLimit + ",upper_limit = " + argEn.UpperLimit + " " +
                                     "WHERE workflowsetup_id = " + argEn.WorkflowSetupId + " AND userid = " + argEn.UserId + " AND menuid = " + argEn.MenuId;

                            int liRowAffected = _DatabaseFactory.ExecuteSqlStatement(Helper.GetDataBaseType,
                                DataBaseConnectionString, sqlCmd);

                            if (liRowAffected > -1)
                                result = true;
                            else
                                throw new Exception("Updation Failed!");
                        }
                        else
                        {
                            return result = true;
                        }
                    }

                    if (cnt == 0)
                    {
                        sqlCmd = "INSERT INTO ur_usermenu_approver(workflowsetup_id, userid, menuid, updatedby, updateddttm, lower_limit, upper_limit) " +
                                 "VALUES (@workflowsetup_id, @userid, @menuid, @updatedby, @updateddttm, @lower_limit, @upper_limit)";

                        if (!FormHelp.IsBlank(sqlCmd))
                        {
                            DbCommand cmd = _DatabaseFactory.GetDbCommand(Helper.GetDataBaseType, sqlCmd, DataBaseConnectionString);
                            _DatabaseFactory.AddInParameter(ref cmd, "@workflowsetup_id", DbType.Int16, argEn.WorkflowSetupId);
                            _DatabaseFactory.AddInParameter(ref cmd, "@userid", DbType.Int16, argEn.UserId);
                            _DatabaseFactory.AddInParameter(ref cmd, "@menuid", DbType.Int16, argEn.MenuId);
                            _DatabaseFactory.AddInParameter(ref cmd, "@updatedby", DbType.String, argEn.LastUpdatedBy);
                            _DatabaseFactory.AddInParameter(ref cmd, "@updateddttm", DbType.DateTime, Helper.DateConversion(argEn.LastUpdatedDtTm));
                            _DatabaseFactory.AddInParameter(ref cmd, "@lower_limit", DbType.Double, argEn.LowerLimit);
                            _DatabaseFactory.AddInParameter(ref cmd, "@upper_limit", DbType.Double, argEn.UpperLimit);
                            _DbParameterCollection = cmd.Parameters;

                            int liRowAffected = clsGeneric.NullToInteger(_DatabaseFactory.ExecuteScalarCommand(Helper.GetDataBaseType, cmd,
                            DataBaseConnectionString, sqlCmd, _DbParameterCollection));

                            if (liRowAffected > -1)
                            {
                                result = true;
                            }
                            else
                            {
                                throw new Exception("Insert Failed! No Row has been inserted.");
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return result;
        }

        #endregion

        #region Delete Uncheck Approver From Workflow Table

        public bool DeleteApprover(WorkflowApproverEn argEn)
        {
            int cnt = 0;
            bool result = false;

            string sqlCmd = "SELECT COUNT(*) AS cnt FROM ur_usermenu_approver WHERE workflowsetup_id = @workflowsetup_id AND userid = @userid AND menuid = @menuid";

            try
            {
                if (!FormHelp.IsBlank(sqlCmd))
                {
                    DbCommand cmdSel = _DatabaseFactory.GetDbCommand(Helper.GetDataBaseType, sqlCmd, DataBaseConnectionString);
                    _DatabaseFactory.AddInParameter(ref cmdSel, "@workflowsetup_id", DbType.Int16, argEn.WorkflowSetupId);
                    _DatabaseFactory.AddInParameter(ref cmdSel, "@userid", DbType.Int16, argEn.UserId);
                    _DatabaseFactory.AddInParameter(ref cmdSel, "@menuid", DbType.Int16, argEn.MenuId);
                    _DbParameterCollection = cmdSel.Parameters;

                    using (IDataReader dr = _DatabaseFactory.GetIDataReader(Helper.GetDataBaseType, cmdSel,
                        DataBaseConnectionString, sqlCmd, _DbParameterCollection).CreateDataReader())
                    {
                        if (dr.Read())
                            cnt = clsGeneric.NullToInteger(dr["cnt"]);
                    }

                    if (cnt == 0)
                    {
                        return result = true;
                    }

                    if (cnt > 0)
                    {
                        sqlCmd = "DELETE FROM ur_usermenu_approver WHERE workflowsetup_id = @workflowsetup_id AND userid = @userid AND menuid = @menuid";

                        if (!FormHelp.IsBlank(sqlCmd))
                        {
                            DbCommand cmd = _DatabaseFactory.GetDbCommand(Helper.GetDataBaseType, sqlCmd, DataBaseConnectionString);
                            _DatabaseFactory.AddInParameter(ref cmd, "@workflowsetup_id", DbType.Int16, argEn.WorkflowSetupId);
                            _DatabaseFactory.AddInParameter(ref cmd, "@userid", DbType.Int16, argEn.UserId);
                            _DatabaseFactory.AddInParameter(ref cmd, "@menuid", DbType.Int16, argEn.MenuId);
                            _DbParameterCollection = cmd.Parameters;

                            int liRowAffected = clsGeneric.NullToInteger(_DatabaseFactory.ExecuteScalarCommand(Helper.GetDataBaseType, cmd,
                            DataBaseConnectionString, sqlCmd, _DbParameterCollection));

                            if (liRowAffected > -1)
                            {
                                result = true;
                            }
                            else
                            {
                                throw new Exception("Delete Failed! No Row has been deleted.");
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return result;
        }

        #endregion

        #region Delete Unchecked User

        public bool DeleteUnchecked(int id, int userid, int menuid)
        {
            int cnt = 0;
            bool result = false;

            string sqlCmd = "SELECT COUNT(*) AS cnt FROM ur_usermenu_approver a FULL JOIN ur_usermenu_preparer b " +
                            "ON a.userid = b.userid WHERE (a.workflowsetup_id = " + id + " OR b.workflowsetup_id = " + id + ") " +
                            "AND (a.userid = " + userid + " OR b.userid = " + userid + ") AND (a.menuid = " + menuid + " OR b.menuid = " + menuid + ")";
            try
            {
                if (!FormHelp.IsBlank(sqlCmd))
                {
                    using (IDataReader dr = _DatabaseFactory.ExecuteReader(Helper.GetDataBaseType,
                        DataBaseConnectionString, sqlCmd).CreateDataReader())
                    {
                        if (dr.Read())
                            cnt = clsGeneric.NullToInteger(dr["cnt"]);
                    }

                    if (cnt == 0)
                    {
                        return result = true;
                    }

                    if (cnt > 0)
                    {
                        sqlCmd = "DELETE FROM ur_usermenu_preparer WHERE EXISTS (SELECT 1 FROM ur_usermenu_preparer " +
                                 "WHERE workflowsetup_id = " + id + " AND userid = " + userid + " AND menuid = " + menuid + " ) " +
                                 "AND userid = " + userid + " AND workflowsetup_id = " + id + "; ";
                        sqlCmd += "DELETE FROM ur_usermenu_approver WHERE EXISTS (SELECT 1 FROM ur_usermenu_approver " +
                                  "WHERE workflowsetup_id = " + id + " AND userid = " + userid + " AND menuid = " + menuid + " ) " +
                                  "AND userid = " + userid + " AND workflowsetup_id = " + id + "; ";

                        if (!FormHelp.IsBlank(sqlCmd))
                        {
                            int liRowAffected = clsGeneric.NullToInteger(_DatabaseFactory.ExecuteSqlStatement(Helper.GetDataBaseType, 
                                DataBaseConnectionString, sqlCmd));

                            if (liRowAffected > -1)
                            {
                                result = true;
                            }
                            else
                            {
                                throw new Exception("Delete Failed! No Row has been deleted.");
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return result;
        }

        #endregion

        #region Delete

        public bool Delete(int ProcessId)
        {
            bool result = false;

            List<WorkflowSetupEn> list = GetList(new WorkflowSetupEn()).FindAll(x => x.MenuId.Equals(ProcessId));

            try
            {
                if (list.Count > 0)
                {
                    string sqlCmd = "";

                    sqlCmd = "DELETE FROM ur_usermenu_preparer WHERE workflowsetup_id = " + list.Select(y => y.Id).FirstOrDefault() + ";";
                    sqlCmd += "DELETE FROM ur_usermenu_approver WHERE workflowsetup_id = " + list.Select(y => y.Id).FirstOrDefault() + ";";
                    sqlCmd += "DELETE FROM ur_workflowsetup WHERE id = " + list.Select(y => y.Id).FirstOrDefault();

                    if (!FormHelp.IsBlank(sqlCmd))
                    {
                        int liRowAffected = clsGeneric.NullToInteger(_DatabaseFactory.ExecuteSqlStatement(Helper.GetDataBaseType,
                            DataBaseConnectionString, sqlCmd));

                        if (liRowAffected > -1)
                        {
                            result = true;
                        }
                        else
                        {
                            throw new Exception("Delete Failed! No Row has been deleted.");
                        }
                    }
                }
                else
                {
                    throw new Exception("Process Id Not Existed, Thus Delete Process Failed.");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return result;
        }

        #endregion

        //#region Update

        ////<summary>
        ////Method to Update WorkflowSetupEn 
        ////</summary>
        ////<param name="argEn">WorkflowSetupEn Entity is an Input.</param>
        ////<returns>Returns Boolean</returns>
        //public bool Update(WorkflowSetupEn argEn)
        //{
        //    bool lbRes = false;
        //    int iOut = 0;
        //    string sqlCmd = "Select count(*) as cnt From UR_WorkFlowSetup WHERE TotalReview=@TotalReview AND TotalApprove = @TotalApprove AND Status=@Status";
        //    try
        //    {
        //        if (!FormHelp.IsBlank(sqlCmd))
        //        {
        //            DbCommand cmdSel = _DatabaseFactory.GetDbCommand(Helper.GetDataBaseType, sqlCmd, DataBaseConnectionString);
        //            _DatabaseFactory.AddInParameter(ref cmdSel, "@TotalReview", DbType.Int32, argEn.TotalReview);
        //            _DatabaseFactory.AddInParameter(ref cmdSel, "@TotalApprove", DbType.Int32, argEn.TotalApprove);
        //            _DatabaseFactory.AddInParameter(ref cmdSel, "@Status", DbType.Boolean, argEn.Status);
        //            _DbParameterCollection = cmdSel.Parameters;

        //            using (IDataReader dr = _DatabaseFactory.GetIDataReader(Helper.GetDataBaseType, cmdSel,
        //                DataBaseConnectionString, sqlCmd, _DbParameterCollection).CreateDataReader())
        //            {
        //                if (dr.Read())
        //                    iOut = clsGeneric.NullToInteger(dr["cnt"]);
        //                if (iOut > 0)
        //                    throw new Exception("Record Already Exist");
        //            }
        //            if (iOut == 0)
        //            {
        //                sqlCmd = "UPDATE UR_WorkFlowSetup SET TotalReview = @TotalReview, TotalApprove = @TotalApprove, Status = @Status, LastUpdatedBy = @LastUpdatedBy, LastUpdatedDtTm = @LastUpdatedDtTm WHERE GroupId = @GroupId";
        //                if (!FormHelp.IsBlank(sqlCmd))
        //                {
        //                    DbCommand cmd = _DatabaseFactory.GetDbCommand(Helper.GetDataBaseType, sqlCmd, DataBaseConnectionString);
        //                    _DatabaseFactory.AddInParameter(ref cmd, "@GroupId", DbType.Int32, argEn.GroupId);
        //                    _DatabaseFactory.AddInParameter(ref cmd, "@TotalReview", DbType.Int32, argEn.TotalReview);
        //                    _DatabaseFactory.AddInParameter(ref cmd, "@TotalApprove", DbType.Int32, argEn.TotalApprove);
        //                    _DatabaseFactory.AddInParameter(ref cmd, "@Status", DbType.Boolean, argEn.Status);
        //                    _DatabaseFactory.AddInParameter(ref cmd, "@LastUpdatedBy", DbType.String, argEn.LastUpdatedBy);
        //                    _DatabaseFactory.AddInParameter(ref cmd, "@LastUpdatedDtTm", DbType.DateTime, Helper.DateConversion(argEn.LastUpdatedDtTm));
        //                    _DbParameterCollection = cmd.Parameters;

        //                    int liRowAffected = _DatabaseFactory.ExecuteNonQuery(Helper.GetDataBaseType, cmd,
        //                        DataBaseConnectionString, sqlCmd, _DbParameterCollection);

        //                    if (liRowAffected > -1)
        //                    {
        //                        lbRes = true;
        //                    }
        //                    else
        //                        throw new Exception("Update Failed! No Row has been updated...");
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //    return lbRes;
        //}

        //#endregion



        //#region GetUserGroupsTypelist

        ///// <summary>
        ///// Method to Get List of all WorkflowSetup
        ///// </summary>
        ///// <param name="argEn">WorkflowSetup Entity as an Input.</param>
        ///// <returns>Returns List of WorkflowSetup</returns>
        //public List<WorkflowSetupEn> GetWorkflowSetupTypelistall(WorkflowSetupEn argEn)
        //{
        //    List<WorkflowSetupEn> loEnList = new List<WorkflowSetupEn>();

        //    string sqlCmd = "select * from ur_workflowsetup where groupid <> '0'";

        //    if ((argEn.TotalReview == -1) && (argEn.TotalReview == -1))
        //    {
        //        sqlCmd = sqlCmd + " order by groupid";
        //    }

        //    else if ((argEn.TotalReview != -1) || (argEn.TotalReview != -1))
        //    {
        //        if (argEn.TotalReview != -1) sqlCmd = sqlCmd + " and TotalReview = '" + argEn.TotalReview + "'";
        //        if (argEn.TotalApprove != -1) sqlCmd = sqlCmd + " and TotalApprove = '" + argEn.TotalApprove + "'"; 

        //        //if (argEn.Status == true) sqlCmd = sqlCmd + " and Status =1";
        //        if (argEn.Status == true) sqlCmd = sqlCmd + " and Status ='true'";
        //        //if (argEn.Status == false) sqlCmd = sqlCmd + " and Status =0";
        //        if (argEn.Status == false) sqlCmd = sqlCmd + " and Status = 'false'";
        //        sqlCmd = sqlCmd + " order by groupid";
        //    }

        //    try
        //    {
        //        if (!FormHelp.IsBlank(sqlCmd))
        //        {
        //            using (IDataReader loReader = _DatabaseFactory.ExecuteReader(Helper.GetDataBaseType,
        //                DataBaseConnectionString, sqlCmd).CreateDataReader())
        //            {
        //                while (loReader.Read())
        //                {
        //                    WorkflowSetupEn loItem = LoadObject(loReader);
        //                    loEnList.Add(loItem);
        //                }
        //                loReader.Close();
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //    return loEnList;
        //}

        //#endregion

        #region LoadObject

        private WorkflowSetupEn LoadObject(IDataReader argReader)
        {
            WorkflowSetupEn loItem = new WorkflowSetupEn();

            loItem.Id = GetValue<int>(argReader, "id");
            loItem.MenuId = GetValue<int>(argReader, "menuid");
            loItem.Status = GetValue<bool>(argReader, "Status");
            loItem.TotalPreparer = GetValue<int>(argReader, "total_preparer");
            loItem.TotalApprover = GetValue<int>(argReader, "total_approver");
            loItem.LastUpdatedBy = GetValue<string>(argReader, "lastupdatedby");
            loItem.LastUpdatedDtTm = GetValue<DateTime>(argReader, "lastupdateddttm");

            return loItem;
        }

        private static T GetValue<T>(IDataReader argReader, string argColNm)
        {
            if (!argReader.IsDBNull(argReader.GetOrdinal(argColNm)))
                return (T)argReader.GetValue(argReader.GetOrdinal(argColNm));
            else
                return default(T);
        }

        #endregion

        //#region GetList

        //// public List<WorkflowSetupEn> GetList(WorkflowSetupEn argEn)
        //// {
        ////     //create instances
        ////     List<WorkflowSetupEn> workflowsetupList = new List<WorkflowSetupEn>();

        ////     //variable declarations
        ////     string SqlStatement = null;

        ////     try
        ////     {
        ////         //build sql statement
        ////         SqlStatement = "SELECT * FROM ur_workflowsetup ";

        ////         //Get User Login Details - Start
        ////         IDataReader _IDataReader = _DatabaseFactory.ExecuteReader(Helper.GetDataBaseType,
        ////                 DataBaseConnectionString, SqlStatement).CreateDataReader();
        ////         //Get User Login Details - Stop

        ////         //if details available - Start
        ////         if (_IDataReader != null)
        ////         {
        ////             while (_IDataReader.Read())
        ////             {
        ////                 WorkflowSetupEn _WorkflowSetupEn = LoadObject(_IDataReader);
        ////                 workflowsetupList.Add(_WorkflowSetupEn);
        ////             }

        ////             _IDataReader.Close();
        ////             _IDataReader.Dispose();

        ////             return workflowsetupList;
        ////         }
        ////         //if details available - Stop

        ////         return null;
        ////     }
        ////     catch (Exception ex)
        ////     {
        ////         throw ex;
        ////     }
        //// }

        // #endregion

        //#region GetWorkflowProcess

        ///// <summary>
        ///// Method to Get Total Reviewer/Approver
        ///// </summary>        
        //public WorkflowSetupEn GetWorkflowProcess(WorkflowSetupEn argEn)
        //{
        //    WorkflowSetupEn loItem = new WorkflowSetupEn();           
        //    int iOut = 0;

        //    string sqlCmd = "Select count(*) as cnt From UR_WorkFlowSetup WHERE Status=@Status";

        //    try
        //    {
        //        if (!FormHelp.IsBlank(sqlCmd))
        //        {
        //            DbCommand cmdSel = _DatabaseFactory.GetDbCommand(Helper.GetDataBaseType, sqlCmd, DataBaseConnectionString);
        //            _DatabaseFactory.AddInParameter(ref cmdSel, "@Status", DbType.Boolean, argEn.Status);
        //            _DbParameterCollection = cmdSel.Parameters;

        //            using (IDataReader dr = _DatabaseFactory.GetIDataReader(Helper.GetDataBaseType, cmdSel,
        //                DataBaseConnectionString, sqlCmd, _DbParameterCollection).CreateDataReader())
        //            {
        //                if (dr.Read())
        //                    iOut = clsGeneric.NullToInteger(dr["cnt"]);
        //                if (iOut == 0)
        //                    throw new Exception("No Workflow Setup");
        //            }
        //            if (iOut > 0)
        //            {
        //                sqlCmd = "select * from ur_workflowsetup where status = @Status";
        //                if (!FormHelp.IsBlank(sqlCmd))
        //                {
        //                    DbCommand cmd = _DatabaseFactory.GetDbCommand(Helper.GetDataBaseType, sqlCmd, DataBaseConnectionString);
        //                    _DatabaseFactory.AddInParameter(ref cmd, "@Status", DbType.Boolean, argEn.Status);
        //                    _DbParameterCollection = cmd.Parameters;

        //                    using (IDataReader loReader = _DatabaseFactory.GetIDataReader(Helper.GetDataBaseType, cmd,
        //                        DataBaseConnectionString, sqlCmd, _DbParameterCollection).CreateDataReader())
        //                    {
        //                        if (loReader != null)
        //                        {
        //                            loReader.Read();
        //                            loItem = LoadObject(loReader);
        //                        }
        //                        loReader.Close();
        //                    }
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }

        //    return loItem;
        //}

        //#endregion        

        //#region GetTotalReviewer

        //public int GetTotalReviewer()
        //{
        //    int IntReviewer = 0;
        //    string sqlGetReviewer = "SELECT totalreview as Review from ur_workflowsetup WHERE status = true ";
            
        //    try
        //    {
        //        if (!FormHelp.IsBlank(sqlGetReviewer))
        //        {
        //            using (IDataReader loReader = _DatabaseFactory.ExecuteReader(Helper.GetDataBaseType,
        //                DataBaseConnectionString, sqlGetReviewer).CreateDataReader())
        //            {
        //                while (loReader.Read())
        //                {
        //                    IntReviewer = clsGeneric.NullToInteger(loReader["Review"]);
        //                }
        //                loReader.Close();
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //    return IntReviewer;
        //}

        //#endregion

        //#region GetTotalApprover

        //public int GetTotalApprover()
        //{
        //    int IntApprover = 0;
        //    string sqlGetApprover = "SELECT totalapprove as Approve from ur_workflowsetup WHERE status = true ";

        //    try
        //    {
        //        if (!FormHelp.IsBlank(sqlGetApprover))
        //        {
        //            using (IDataReader loReader = _DatabaseFactory.ExecuteReader(Helper.GetDataBaseType,
        //                DataBaseConnectionString, sqlGetApprover).CreateDataReader())
        //            {
        //                while (loReader.Read())
        //                {
        //                    IntApprover = clsGeneric.NullToInteger(loReader["Approve"]);
        //                }
        //                loReader.Close();
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //    return IntApprover;
        //}

        //#endregion

    }
}
