#region NameSpaces 

using System;
using System.Data;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data.Common;
using HTS.SAS.Entities;
using MaxGeneric;

#endregion

namespace HTS.SAS.DataAccessObjects
{
    /// <summary>
    /// Class to handle all the UserGroups Methods.
    /// </summary>
    public class UserGroupsDAL
    {
        #region Global Declarations 

        private DbParameterCollection _DbParameterCollection = null;

        private MaxModule.DatabaseProvider _DatabaseFactory =
            new MaxModule.DatabaseProvider();

        private string DataBaseConnectionString = Helper.
           GetConnectionString();

        #endregion

        public UserGroupsDAL()
        {
        }

        #region GetUserGroups 

        /// <summary>
        /// Method to Get List of Active UserGroups
        /// </summary>
        /// <returns>Returns List of UserGroups</returns>
        public List<UserGroupsEn> GetUserGroups()
        {
            List<UserGroupsEn> loEnList = new List<UserGroupsEn>();
            //string sqlCmd = "select * from UR_UserGroups where Status=1";
            string sqlCmd = "select * from UR_UserGroups where Status='true'";
            try
            {
                if (!FormHelp.IsBlank(sqlCmd))
                {
                    using (IDataReader loReader = _DatabaseFactory.ExecuteReader(Helper.GetDataBaseType,
                        DataBaseConnectionString, sqlCmd).CreateDataReader())
                    {
                        while (loReader.Read())
                        {
                            UserGroupsEn loItem = LoadObject(loReader);
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

        #region GetList 

        /// <summary>
        /// Method to Get List of UserGroups
        /// </summary>
        /// <param name="argEn">UserGroups Entity as an Input.</param>
        /// <returns>Returns List of UserGroups</returns>
        public List<UserGroupsEn> GetList(UserGroupsEn argEn)
        {
            List<UserGroupsEn> loEnList = new List<UserGroupsEn>();
            string sqlCmd = "select * from UR_UserGroups";
            try
            {
                if (!FormHelp.IsBlank(sqlCmd))
                {
                    using (IDataReader loReader = _DatabaseFactory.ExecuteReader(Helper.GetDataBaseType,
                        DataBaseConnectionString, sqlCmd).CreateDataReader())
                    {
                        while (loReader.Read())
                        {
                            UserGroupsEn loItem = LoadObject(loReader);
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

        #region GetUserGroupsTypelist 

        /// <summary>
        /// Method to Get List of Active or Inative UserGroups
        /// </summary>
        /// <param name="argEn">UserGroups Entity as an Input.UserGroupName,Description and Status as Input properties.</param>
        /// <returns>Returns List of UserGroups</returns>
        public List<UserGroupsEn> GetUserGroupsTypelist(UserGroupsEn argEn)
        {
            List<UserGroupsEn> loEnList = new List<UserGroupsEn>();
            argEn.UserGroupName = argEn.UserGroupName.Replace("*", "%");
            argEn.Description = argEn.Description.Replace("*", "%");

            string sqlCmd = "select * from UR_UserGroups where UserGroupId <> '0'";
            if (argEn.DepartmentID != "0") sqlCmd = sqlCmd + " and DepartmentID = '" + argEn.DepartmentID + "'";
            if (argEn.UserGroupName.Length != 0) sqlCmd = sqlCmd + " and UserGroupName like '" + argEn.UserGroupName + "'";
            if (argEn.Description.Length != 0) sqlCmd = sqlCmd + " and Description like '" + argEn.Description + "'";
            //if (argEn.Status == true) sqlCmd = sqlCmd + " and Status =1";
            if (argEn.Status == true) sqlCmd = sqlCmd + " and Status ='true'";
            //if (argEn.Status == false) sqlCmd = sqlCmd + " and Status =0";
            if (argEn.Status == false) sqlCmd = sqlCmd + " and Status = 'false'";
            sqlCmd = sqlCmd + " order by UserGroupId";
            try
            {
                if (!FormHelp.IsBlank(sqlCmd))
                {
                    using (IDataReader loReader = _DatabaseFactory.ExecuteReader(Helper.GetDataBaseType,
                        DataBaseConnectionString, sqlCmd).CreateDataReader())
                    {
                        while (loReader.Read())
                        {
                            UserGroupsEn loItem = LoadObject(loReader);
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

        #region GetUserGroupsTypelistAll 

        /// <summary>
        /// Method to Get List of All UserGroups
        /// </summary>
        /// <param name="argEn">UserGroups Entity as an Input.UserGroupName and Description as Input properties.</param>
        /// <returns>Returns List of UserGroups</returns>
        public List<UserGroupsEn> GetUserGroupsTypelistAll(UserGroupsEn argEn)
        {
            List<UserGroupsEn> loEnList = new List<UserGroupsEn>();
            argEn.UserGroupName = argEn.UserGroupName.Replace("*", "%");
            argEn.Description = argEn.Description.Replace("*", "%");

            string sqlCmd = "select * from UR_UserGroups where UserGroupId <> '0'";
            if (argEn.UserGroupName.Length != 0) sqlCmd = sqlCmd + " and UserGroupName like '" + argEn.UserGroupName + "'";
            if (argEn.Description.Length != 0) sqlCmd = sqlCmd + " and Description like '" + argEn.Description + "'";
            sqlCmd = sqlCmd + " order by UserGroupId";
            try
            {
                if (!FormHelp.IsBlank(sqlCmd))
                {
                    using (IDataReader loReader = _DatabaseFactory.ExecuteReader(Helper.GetDataBaseType,
                        DataBaseConnectionString, sqlCmd).CreateDataReader())
                    {
                        while (loReader.Read())
                        {
                            UserGroupsEn loItem = LoadObject(loReader);
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

        #region GetItem 

        /// <summary>
        /// Method to Get UserGroups Entity
        /// </summary>
        /// <param name="argEn">UserGroups Entity is an Input.UserGroupId and UserGroupName as Input Properties.</param>
        /// <returns>Returns UserGroups Entity</returns>
        public UserGroupsEn GetItem(UserGroupsEn argEn)
        {
            UserGroupsEn loItem = new UserGroupsEn();
            string sqlCmd = "Select * FROM UR_UserGroups WHERE UserGroupId = @UserGroupId and UserGroupName = @UserGroupName";
            try
            {
                if (!FormHelp.IsBlank(sqlCmd))
                {
                    DbCommand cmd = _DatabaseFactory.GetDbCommand(Helper.GetDataBaseType, sqlCmd, DataBaseConnectionString);
                    _DatabaseFactory.AddInParameter(ref cmd, "@UserGroupId", DbType.Int32, argEn.UserGroupId);
                    _DatabaseFactory.AddInParameter(ref cmd, "@UserGroupName", DbType.String, argEn.UserGroupName);
                    _DbParameterCollection = cmd.Parameters;

                    using (IDataReader loReader = _DatabaseFactory.GetIDataReader(Helper.GetDataBaseType, cmd,
                        DataBaseConnectionString, sqlCmd, _DbParameterCollection).CreateDataReader())
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

        #region Insert

        /// <summary>
        /// Method to Insert UserGroups 
        /// </summary>
        /// <param name="argEn">UserGroups Entity is an Input.</param>
        /// <returns>Returns Boolean</returns>
        public bool Insert(UserGroupsEn argEn)
        {
            bool lbRes = false;
            int iOut = 0;
            string sqlCmd = "Select count(*) as cnt From UR_UserGroups WHERE UserGroupName = @UserGroupName or Description = @Description";
            
            //string SqlStatement1 = String.Empty;
            try
            {
                // Checking for Duplicates

                if (!FormHelp.IsBlank(sqlCmd))
                {
                    DbCommand cmdSel = _DatabaseFactory.GetDbCommand(Helper.GetDataBaseType, sqlCmd, DataBaseConnectionString);
                    _DatabaseFactory.AddInParameter(ref cmdSel, "@UserGroupName", DbType.String, argEn.UserGroupName);
                    _DatabaseFactory.AddInParameter(ref cmdSel, "@Description", DbType.String, argEn.Description);
                    _DbParameterCollection = cmdSel.Parameters;

                    using (IDataReader dr = _DatabaseFactory.GetIDataReader(Helper.GetDataBaseType, cmdSel,
                        DataBaseConnectionString, sqlCmd, _DbParameterCollection).CreateDataReader())
                    {
                        if (dr.Read())
                            iOut = clsGeneric.NullToInteger(dr["cnt"]);
                        if (iOut > 0)
                            throw new Exception("Record Already Exist");
                    }
                    if (iOut == 0)
                    {
                        //sqlCmd = "INSERT INTO UR_UserGroups(DepartmentID,UserGroupName,Status,Description,LastUpdatedBy,LastUpdatedDtTm) VALUES (@DepartmentID,@UserGroupName,@Status,@Description,@LastUpdatedBy,@LastUpdatedDtTm)  select @@identity";
                        sqlCmd = "INSERT INTO UR_UserGroups(DepartmentID,UserGroupName,Status,Description,LastUpdatedBy,LastUpdatedDtTm) VALUES " +
                                    "(@DepartmentID,@UserGroupName,@Status,@Description,@LastUpdatedBy,@LastUpdatedDtTm); " +
                                    "select max(usergroupid + 1) from UR_UserGroups;";
                        if (!FormHelp.IsBlank(sqlCmd))
                        {
                            DbCommand cmd = _DatabaseFactory.GetDbCommand(Helper.GetDataBaseType, sqlCmd, DataBaseConnectionString);
                            _DatabaseFactory.AddInParameter(ref cmd, "@DepartmentID", DbType.String, argEn.DepartmentID);
                            _DatabaseFactory.AddInParameter(ref cmd, "@UserGroupName", DbType.String, argEn.UserGroupName);
                            _DatabaseFactory.AddInParameter(ref cmd, "@Status", DbType.Boolean, argEn.Status);
                            _DatabaseFactory.AddInParameter(ref cmd, "@Description", DbType.String, argEn.Description);
                            _DatabaseFactory.AddInParameter(ref cmd, "@LastUpdatedBy", DbType.String, argEn.LastUpdatedBy);
                            _DatabaseFactory.AddInParameter(ref cmd, "@LastUpdatedDtTm", DbType.DateTime, Helper.DateConversion(argEn.LastUpdatedDtTm));                            
                            _DbParameterCollection = cmd.Parameters;
                            
                            int liRowAffected = clsGeneric.NullToInteger(_DatabaseFactory.ExecuteScalarCommand(Helper.GetDataBaseType, cmd,
                            DataBaseConnectionString, sqlCmd, _DbParameterCollection));
                            
                            if (liRowAffected > 0)
                            {                                
                                argEn.UserGroupId = clsGeneric.NullToInteger(GetGroupID(argEn.DepartmentID, argEn.UserGroupName));                                
                                InsertUserGroupMenu(argEn);
                                lbRes = true;
                            }                          
                            
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return lbRes;
        }

        #endregion

        #region Update 

        /// <summary>
        /// Method to Update UserGroups 
        /// </summary>
        /// <param name="argEn">UserGroups Entity is an Input.</param>
        /// <returns>Returns Boolean</returns>
        public bool Update(UserGroupsEn argEn)
        {
            bool lbRes = false;
            int iOut = 0;
            string sqlCmd = "Select count(*) as cnt From UR_UserGroups WHERE UserGroupName != @UserGroupName and Description = @Description";
            try
            {
                if (!FormHelp.IsBlank(sqlCmd))
                {
                    DbCommand cmdSel = _DatabaseFactory.GetDbCommand(Helper.GetDataBaseType, sqlCmd, DataBaseConnectionString);
                    _DatabaseFactory.AddInParameter(ref cmdSel, "@UserGroupName", DbType.String, argEn.UserGroupName);
                    _DatabaseFactory.AddInParameter(ref cmdSel, "@Description", DbType.String, argEn.Description);
                    _DbParameterCollection = cmdSel.Parameters;

                    using (IDataReader dr = _DatabaseFactory.GetIDataReader(Helper.GetDataBaseType, cmdSel,
                        DataBaseConnectionString, sqlCmd, _DbParameterCollection).CreateDataReader())
                    {
                        if (dr.Read())
                            iOut = clsGeneric.NullToInteger(dr["cnt"]);
                        if (iOut > 0)
                            throw new Exception("Record Already Exist");
                    }
                    if (iOut == 0)
                    {
                        sqlCmd = "UPDATE UR_UserGroups SET DepartmentID = @DepartmentID, UserGroupName = @UserGroupName, Status = @Status, Description = @Description, LastUpdatedBy = @LastUpdatedBy, LastUpdatedDtTm = @LastUpdatedDtTm WHERE UserGroupId = @UserGroupId and UserGroupName = @UserGroupName";
                        if (!FormHelp.IsBlank(sqlCmd))
                        {
                            DbCommand cmd = _DatabaseFactory.GetDbCommand(Helper.GetDataBaseType, sqlCmd, DataBaseConnectionString);
                            _DatabaseFactory.AddInParameter(ref cmd, "@DepartmentID", DbType.String, argEn.DepartmentID);
                            _DatabaseFactory.AddInParameter(ref cmd, "@UserGroupId", DbType.String, argEn.UserGroupId);
                            _DatabaseFactory.AddInParameter(ref cmd, "@UserGroupName", DbType.String, argEn.UserGroupName);
                            _DatabaseFactory.AddInParameter(ref cmd, "@Status", DbType.Boolean, argEn.Status);
                            _DatabaseFactory.AddInParameter(ref cmd, "@Description", DbType.String, argEn.Description);
                            _DatabaseFactory.AddInParameter(ref cmd, "@LastUpdatedBy", DbType.String, argEn.LastUpdatedBy);
                            _DatabaseFactory.AddInParameter(ref cmd, "@LastUpdatedDtTm", DbType.DateTime, Helper.DateConversion(argEn.LastUpdatedDtTm));
                            _DbParameterCollection = cmd.Parameters;

                            int liRowAffected = _DatabaseFactory.ExecuteNonQuery(Helper.GetDataBaseType, cmd,
                                DataBaseConnectionString, sqlCmd, _DbParameterCollection);

                            if (liRowAffected > -1)
                            {
                                InsertUserGroupMenu(argEn);
                                lbRes = true;
                            }
                            else
                                throw new Exception("Update Failed! No Row has been updated...");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return lbRes;
        }

        #endregion

        #region Delete 

        /// <summary>
        /// Method to Delete UserGroups 
        /// </summary>
        /// <param name="argEn">UserGroups Entity is an Input.UserGroupId as Input Property.</param>
        /// <returns>Returns Boolean</returns>
        public bool Delete(UserGroupsEn argEn)
        {
            bool lbRes = false;
            int iOut = 0;
            //string sqlCmd = "Select count(*) as cnt From UR_Users INNER JOIN  UR_UserGroups ON UR_Users.UserGroupId = UR_UserGroups.UserGroupId WHERE UR_UserGroups.UserGroupId = @UserGroupId ";
            string sqlCmd = "Select count(*) as cnt From UR_Users where UserGroupId = @UserGroupId ";
            try
            {
                if (!FormHelp.IsBlank(sqlCmd))
                {
                    DbCommand cmdSel = _DatabaseFactory.GetDbCommand(Helper.GetDataBaseType, sqlCmd, DataBaseConnectionString);
                    _DatabaseFactory.AddInParameter(ref cmdSel, "@UserGroupId", DbType.String, argEn.UserGroupId);
                    _DbParameterCollection = cmdSel.Parameters;

                    using (IDataReader dr = _DatabaseFactory.GetIDataReader(Helper.GetDataBaseType, cmdSel,
                        DataBaseConnectionString, sqlCmd, _DbParameterCollection).CreateDataReader())
                    {
                        if (dr.Read())
                            iOut = clsGeneric.NullToInteger(dr["cnt"]);
                        if (iOut > 0)
                            throw new Exception("Record Already In Use");
                    }
                    if (iOut == 0)
                    {
                        sqlCmd = "DELETE FROM UR_UserRights WHERE UserGroupId = @UserGroupId; DELETE FROM UR_UserGroups WHERE UserGroupId = @UserGroupId";
                        if (!FormHelp.IsBlank(sqlCmd))
                        {
                            DbCommand cmd = _DatabaseFactory.GetDbCommand(Helper.GetDataBaseType, sqlCmd, DataBaseConnectionString);
                            _DatabaseFactory.AddInParameter(ref cmd, "@UserGroupId", DbType.Int32, argEn.UserGroupId);
                            _DbParameterCollection = cmd.Parameters;

                            int liRowAffected = _DatabaseFactory.ExecuteNonQuery(Helper.GetDataBaseType, cmd,
                                DataBaseConnectionString, sqlCmd, _DbParameterCollection);

                            if (liRowAffected > -1)
                                lbRes = true;
                            else
                                throw new Exception("Delete Failed! No Row has been updated...");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return lbRes;
        }

        #endregion

        #region GetGroupID

        public int GetGroupID (string DeptID, string GroupName)
        {
            int UserGroupId = 0;
            string sqlGetID = "Select usergroupid as id FROM UR_UserGroups WHERE Departmentid = " + clsGeneric.AddQuotes(DeptID);
            sqlGetID = sqlGetID + " and UserGroupName = " + clsGeneric.AddQuotes(GroupName);

            try
            {
                if (!FormHelp.IsBlank(sqlGetID))
                {
                    using (IDataReader loReader = _DatabaseFactory.ExecuteReader(Helper.GetDataBaseType,
                        DataBaseConnectionString, sqlGetID).CreateDataReader())
                    {
                        while (loReader.Read())
                        {    
                            UserGroupId = clsGeneric.NullToInteger(loReader["id"]);
                        }
                        loReader.Close();
                    }                  
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return UserGroupId;
        }

        #endregion

        #region LoadObject

        /// <summary>
        /// Method to Load UserGroups Entity 
        /// </summary>
        /// <param name="argReader">IDataReader Object is an Input.</param>
        /// <returns>Returns UserGroups Entity</returns>
        private UserGroupsEn LoadObject(IDataReader argReader)
        {
            UserGroupsEn loItem = new UserGroupsEn();
            loItem.UserGroupId = GetValue<int>(argReader, "UserGroupId");
            loItem.DepartmentID = GetValue<string>(argReader, "DepartmentID");
            loItem.UserGroupName = GetValue<string>(argReader, "UserGroupName");
            loItem.Status = GetValue<bool>(argReader, "Status");
            loItem.Description = GetValue<string>(argReader, "Description");
            loItem.LastUpdatedBy = GetValue<string>(argReader, "LastUpdatedBy");
            loItem.LastUpdatedDtTm = GetValue<DateTime>(argReader, "LastUpdatedDtTm");

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

        #region InsertUserGroupMenu 

        /// <summary>
        /// Method to Insert UserGroupsMenu
        /// </summary>
        /// <param name="argEn">UserGroups Entity is an Input.</param>
        /// <returns>Returns Boolean</returns>
        public bool InsertUserGroupMenu(UserGroupsEn argEn)
        {
            List<UserRightsEn> loEnList = new List<UserRightsEn>();
            string sqlCmd = "select * from UR_MenuMaster ";
            try
            {
                if (!FormHelp.IsBlank(sqlCmd))
                {
                    using (IDataReader loReader = _DatabaseFactory.ExecuteReader(Helper.GetDataBaseType,
                        DataBaseConnectionString, sqlCmd).CreateDataReader())
                    {
                        while (loReader.Read())
                        {
                            UserRightsEn loItem = new UserRightsEn();
                            loItem.MenuID = GetValue<int>(loReader, "MenuID");
                            loItem.UserGroup = argEn.UserGroupId;
                            loItem.LastUser = argEn.LastUpdatedBy;
                            loItem.LastDtTm = argEn.LastUpdatedDtTm;
                            loEnList.Add(loItem);
                        }
                        loReader.Close();
                    }
                    UserRightsDAL userRights = new UserRightsDAL();
                    userRights.InsertUserRights(loEnList);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return true;
        }

        #endregion

        #region GetUserGroupList
        /// <summary>
        /// Method to Get User group list
        /// </summary>
        /// <param name="argEn">UserGroups Entity as Input.</param>
        /// <returns>Returns a dataset</returns>
        public List<UserGroupsEn> GetUserGroupList(UserGroupsEn argEn)
        {
            List<UserGroupsEn> loEnList = new List<UserGroupsEn>();
            //variable declarations
            string sqlCmd = null;
            try
            {
                // With...
                sqlCmd = "SELECT * FROM UR_UserGroups WHERE Status = \'true\' ";

                if (argEn.DepartmentID.Length != 0) sqlCmd = sqlCmd + "AND DepartmentID = " + clsGeneric.AddQuotes(argEn.DepartmentID);

                if (!FormHelp.IsBlank(sqlCmd))
                {
                    using (IDataReader loReader = _DatabaseFactory.ExecuteReader(Helper.GetDataBaseType,
                        DataBaseConnectionString, sqlCmd).CreateDataReader())
                    {
                        while (loReader.Read())
                        {
                            UserGroupsEn loItem = LoadObject(loReader);
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
    }

}


