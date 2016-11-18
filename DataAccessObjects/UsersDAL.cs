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
    /// Class to handle all the Users Methods.
    /// </summary>
    public class UsersDAL
    {
        #region Global Declarations 

        private DbParameterCollection _DbParameterCollection = null;

        private MaxModule.DatabaseProvider _DatabaseFactory =
            new MaxModule.DatabaseProvider();

        private string DataBaseConnectionString = Helper.
           GetConnectionString();               

        #endregion

        public UsersDAL()
        {
        }

        #region GetList 

        /// <summary>
        /// Method to Get List of Users
        /// </summary>
        /// <param name="argEn">Users Entity  as an Inputs.</param>
        /// <returns>Returns List of Users</returns>
        public List<UsersEn> GetList(UsersEn argEn)
        {
            List<UsersEn> loEnList = new List<UsersEn>();
            string sqlCmd = "select * from UR_Users";
            try
            {
                if (!FormHelp.IsBlank(sqlCmd))
                {
                    using (IDataReader loReader = _DatabaseFactory.ExecuteReader(Helper.GetDataBaseType,
                        DataBaseConnectionString, sqlCmd).CreateDataReader())
                    {
                        while (loReader.Read())
                        {
                            UsersEn loItem = LoadObject(loReader);
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

        #region GetUsersList 

        /// <summary>
        /// Method to Get List of Active Or Inactive Users
        /// </summary>
        /// <param name="argEn">Users Entity  as an Input.UserName,Email,UserStatus,UserGroupId and RecStatus as Input Properties.</param>
        /// <returns>Returns List of Users</returns>
        public List<UsersEn> GetUsersList(UsersEn argEn)
        {
            List<UsersEn> loEnList = new List<UsersEn>();
            argEn.UserName = argEn.UserName.Replace("*", "%");
            argEn.Email = argEn.Email.Replace("*", "%");
            argEn.UserStatus = argEn.UserStatus.Replace("*", "%");
            String SqlStr = "select * from Ur_Users where UserName <> '0'";
            if (argEn.UserName.Length != 0) SqlStr = SqlStr + " and Ur_Users.UserName like '" + argEn.UserName + "'";
            if (argEn.Email.Length != 0) SqlStr = SqlStr + " and Ur_Users.Email like '" + argEn.Email + "'";
            if (argEn.UserStatus.Length != 0) SqlStr = SqlStr + " and Ur_Users.UserStatus like '" + argEn.UserStatus + "'";
            //if (argEn.RecStatus == true) SqlStr = SqlStr + " and Ur_Users.RecStatus =1";
            if (argEn.RecStatus == true) SqlStr = SqlStr + " and Ur_Users.RecStatus ='true'";
            //if (argEn.RecStatus == false) SqlStr = SqlStr + " and Ur_Users.RecStatus =0";
            if (argEn.RecStatus == false) SqlStr = SqlStr + " and Ur_Users.RecStatus = 'false'";
            if (argEn.UserGroupId != 0) SqlStr = SqlStr + " and Ur_Users.UserGroupId =" + argEn.UserGroupId;
            SqlStr = SqlStr + " order by Ur_Users.UserID";
            try
            {
                if (!FormHelp.IsBlank(SqlStr))
                {
                    using (IDataReader loReader = _DatabaseFactory.ExecuteReader(Helper.GetDataBaseType,
                        DataBaseConnectionString, SqlStr).CreateDataReader())
                    {
                        while (loReader.Read())
                        {
                            UsersEn loItem = LoadObject(loReader);
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
        /// Method to Get Users Entity
        /// </summary>
        /// <param name="argEn">Users Entity is an Input.UserName as Input Property.</param>
        /// <returns>Returns Users Entity</returns>
        public UsersEn GetItem(UsersEn argEn)
        {
            UsersEn loItem = new UsersEn();

            string sqlCmd = "Select * FROM UR_Users WHERE UserName = @UserName";

            try
            {
                if (!FormHelp.IsBlank(sqlCmd))
                {
                    DbCommand cmd = _DatabaseFactory.GetDbCommand(Helper.GetDataBaseType, sqlCmd, DataBaseConnectionString);
                    _DatabaseFactory.AddInParameter(ref cmd, "@UserName", DbType.String, argEn.UserName);
                    _DbParameterCollection = cmd.Parameters;

                    using (IDataReader loReader = _DatabaseFactory.GetIDataReader(Helper.GetDataBaseType, cmd,
                        DataBaseConnectionString, sqlCmd, _DbParameterCollection).CreateDataReader())
                    {
                        while (loReader.Read())
                        {
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

        #region Get Login User 

        /// <summary>
        /// Method to Get Login User
        /// </summary>
        /// <param name="argEn">Users Entity  as an Input.UserName and Password as Input Properties.</param>
        /// <returns>Returns Users Entity</returns>
        public UsersEn GetLoginUser(UsersEn argEn)
        {
            #region Declarations 

            //Create Instances
            UsersEn UserDetails = new UsersEn();

            //vairiable declarations
            string SqlStatement = null;

            #endregion

            try
            {
                //Build Sql Statement - Start
                SqlStatement = "SELECT * FROM UR_Users WHERE UserName = " + clsGeneric.AddQuotes(argEn.UserName);
                SqlStatement += " AND Password = " + clsGeneric.AddQuotes(argEn.Password);
                SqlStatement += " AND UserStatus = 'True'";
                //Build Sql Statement - Stop
                
                //Get User Login Details - Start
                IDataReader _IDataReader = _DatabaseFactory.ExecuteReader(Helper.GetDataBaseType,DataBaseConnectionString,
                    SqlStatement).CreateDataReader();
                //Get User Login Details - Stop
                            
                //if user login details available - Start
                if (_IDataReader != null)
                {
                    if (_IDataReader.Read())
                    {
                        //load data to entities
                        UserDetails = LoadObject(_IDataReader);
                    }
                    _IDataReader.Close();
                    _IDataReader.Dispose();
                    return UserDetails;
                }
                //if user login details available - Stop

                return null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region Insert 

        /// <summary>
        /// Method to Insert Users 
        /// </summary>
        /// <param name="argEn">Users Entity is an Input.</param>
        /// <returns>Returns Boolean</returns>
        //public bool Insert(UsersEn argEn)
        //{
                       
        //    bool lbRes = false;
        //    int iOut = 0;
        //    string sqlCmd = "Select count(*) as cnt From UR_Users WHERE UserID = @UserID or UserName = @UserName";
        //    try
        //    {
        //        //Checking for Duplicates
        //        if (!FormHelp.IsBlank(sqlCmd))
        //        {
        //            DbCommand cmdSel = _DatabaseFactory.GetDbCommand(Helper.GetDataBaseType, sqlCmd, DataBaseConnectionString);
        //            _DatabaseFactory.AddInParameter(ref cmdSel, "@UserID", DbType.Int32, argEn.UserID);
        //            _DatabaseFactory.AddInParameter(ref cmdSel, "@UserName", DbType.String, argEn.UserName);
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
        //                //get UserId -Start
        //                string sqlCmd1 = "Select max(userid) from ur_users;";
        //                int userID = clsGeneric.NullToInteger(_DatabaseFactory.ExecuteScalar(Helper.GetDataBaseType, DataBaseConnectionString, sqlCmd1));
        //                userID = clsGeneric.ConvertToInt(userID) + 1;
        //                //get UserID -Stop

        //                sqlCmd = "INSERT INTO UR_Users(userid,UserName,Password,UserGroupId,UserStatus,RecStatus,department,Email,LastUpdatedBy,LastUpdatedDtTm,workflow_group,workflow_role) VALUES (@userid,@UserName,@Password,@UserGroupId,@UserStatus,@RecStatus,@Department,@Email,@LastUpdatedBy,@LastUpdatedDtTm,@workflow_group,@workflow_role); ";
        //                 if(!FormHelp.IsBlank(sqlCmd))
        //                {

        //                    DbCommand cmd = _DatabaseFactory.GetDbCommand(Helper.GetDataBaseType, sqlCmd, DataBaseConnectionString);
        //                    _DatabaseFactory.AddInParameter(ref cmd, "@userid", DbType.Int32,userID);
        //                    _DatabaseFactory.AddInParameter(ref cmd, "@UserName", DbType.String, argEn.UserName);
        //                    _DatabaseFactory.AddInParameter(ref cmd, "@Password", DbType.String, argEn.Password);
        //                    _DatabaseFactory.AddInParameter(ref cmd, "@UserGroupId", DbType.Int32, argEn.UserGroupId);
        //                    _DatabaseFactory.AddInParameter(ref cmd, "@UserStatus", DbType.String, argEn.UserStatus);
        //                    _DatabaseFactory.AddInParameter(ref cmd, "@RecStatus", DbType.Boolean, argEn.RecStatus);
        //                    _DatabaseFactory.AddInParameter(ref cmd, "@Department", DbType.String, argEn.Department);
        //                    _DatabaseFactory.AddInParameter(ref cmd, "@Email", DbType.String, argEn.Email);
        //                    _DatabaseFactory.AddInParameter(ref cmd, "@LastUpdatedBy", DbType.String, argEn.LastUpdatedBy);
        //                    _DatabaseFactory.AddInParameter(ref cmd, "@LastUpdatedDtTm", DbType.DateTime, Helper.DateConversion(argEn.LastUpdatedDtTm));
        //                    _DatabaseFactory.AddInParameter(ref cmd, "@workflow_group", DbType.Int32, argEn.WorkflowGroup);
        //                    _DatabaseFactory.AddInParameter(ref cmd, "@workflow_role", DbType.String, argEn.WorkflowRole);
        //                    _DbParameterCollection = cmd.Parameters;

        //                    int liRowAffected = _DatabaseFactory.ExecuteNonQuery(Helper.GetDataBaseType, cmd,
        //                        DataBaseConnectionString, sqlCmd, _DbParameterCollection);
                            
        //                    if (liRowAffected > -1)
        //                        lbRes = true;
        //                    else
        //                        throw new Exception("Insertion Failed! No Row has been updated...");
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
        //Modified Zoya 22/2/2016

        public bool Insert(UsersEn argEn)
        {

            bool lbRes = false;
            int iOut = 0;
            string sqlCmd = "Select count(*) as cnt From UR_Users WHERE UserID = @UserID or UserName = @UserName";
            string sqlUser = null;
            try
            {
                //Checking for Duplicates
                if (!FormHelp.IsBlank(sqlCmd))
                {
                    DbCommand cmdSel = _DatabaseFactory.GetDbCommand(Helper.GetDataBaseType, sqlCmd, DataBaseConnectionString);
                    _DatabaseFactory.AddInParameter(ref cmdSel, "@UserID", DbType.Int32, argEn.UserID);
                    _DatabaseFactory.AddInParameter(ref cmdSel, "@UserName", DbType.String, argEn.UserName);
                    _DbParameterCollection = cmdSel.Parameters;

                    using (IDataReader dr = _DatabaseFactory.GetIDataReader(Helper.GetDataBaseType, cmdSel,
                        DataBaseConnectionString, sqlCmd, _DbParameterCollection).CreateDataReader())
                    {
                        if (dr.Read())
                            iOut = clsGeneric.NullToInteger(dr["cnt"]);
                        if (iOut > 0)
                            throw new Exception("Record Already Exist");
                    }

                    int iOut1 = 0;
                    string sqlCmd2 = "Select count(*) as cnt From UR_Users WHERE email = @email";

                    if (!FormHelp.IsBlank(sqlCmd2))
                    {
                        DbCommand cmdS = _DatabaseFactory.GetDbCommand(Helper.GetDataBaseType, sqlCmd2, DataBaseConnectionString);
                        _DatabaseFactory.AddInParameter(ref cmdS, "@email", DbType.String, argEn.Email);
                        _DbParameterCollection = cmdS.Parameters;

                        using (IDataReader dr = _DatabaseFactory.GetIDataReader(Helper.GetDataBaseType, cmdS,
                            DataBaseConnectionString, sqlCmd2, _DbParameterCollection).CreateDataReader())
                        {
                            if (dr.Read())
                                iOut1 = clsGeneric.NullToInteger(dr["cnt"]);
                            if (iOut1 > 0)
                                throw new Exception("Email Already Exist");
                        }
                    }

                    if (iOut == 0 && iOut1 == 0)
                    {
                        //get UserId -Start
                        string sqlCmd1 = "Select max(userid) from ur_users;";
                        int userID = clsGeneric.NullToInteger(_DatabaseFactory.ExecuteScalar(Helper.GetDataBaseType, DataBaseConnectionString, sqlCmd1));
                        userID = clsGeneric.ConvertToInt(userID) + 1;
                        //get UserID -Stop

                        sqlCmd = "INSERT INTO UR_Users(userid,UserName,Password,UserGroupId,UserStatus,RecStatus,department,Email,LastUpdatedBy,LastUpdatedDtTm,workflow_group,workflow_role,staff_no,staff_name,designation,user_expiry) " +
                            "VALUES (@userid,@UserName,@Password,@UserGroupId,@UserStatus,@RecStatus,@Department,@Email,@LastUpdatedBy,@LastUpdatedDtTm,@workflow_group,@workflow_role,@staff_no,@staff_name,@designation,@user_expiry); ";
                        if (!FormHelp.IsBlank(sqlCmd))
                        {

                            DbCommand cmd = _DatabaseFactory.GetDbCommand(Helper.GetDataBaseType, sqlCmd, DataBaseConnectionString);
                            _DatabaseFactory.AddInParameter(ref cmd, "@userid", DbType.Int32, userID);
                            _DatabaseFactory.AddInParameter(ref cmd, "@UserName", DbType.String, argEn.UserName);
                            _DatabaseFactory.AddInParameter(ref cmd, "@Password", DbType.String, argEn.Password);
                            _DatabaseFactory.AddInParameter(ref cmd, "@UserGroupId", DbType.Int32, argEn.UserGroupId);
                            _DatabaseFactory.AddInParameter(ref cmd, "@UserStatus", DbType.String, argEn.UserStatus);
                            _DatabaseFactory.AddInParameter(ref cmd, "@RecStatus", DbType.Boolean, argEn.RecStatus);
                            _DatabaseFactory.AddInParameter(ref cmd, "@Department", DbType.String, argEn.Department);
                            _DatabaseFactory.AddInParameter(ref cmd, "@Email", DbType.String, argEn.Email);
                            _DatabaseFactory.AddInParameter(ref cmd, "@LastUpdatedBy", DbType.String, argEn.LastUpdatedBy);
                            _DatabaseFactory.AddInParameter(ref cmd, "@LastUpdatedDtTm", DbType.DateTime, Helper.DateConversion(argEn.LastUpdatedDtTm));
                            _DatabaseFactory.AddInParameter(ref cmd, "@workflow_group", DbType.Int32, argEn.WorkflowGroup);
                            _DatabaseFactory.AddInParameter(ref cmd, "@workflow_role", DbType.String, argEn.WorkflowRole);
                            //Added 3/8/16
                            _DatabaseFactory.AddInParameter(ref cmd, "@staff_no", DbType.String, argEn.StaffID);
                            _DatabaseFactory.AddInParameter(ref cmd, "@staff_name", DbType.String, argEn.StaffName);
                            _DatabaseFactory.AddInParameter(ref cmd, "@designation", DbType.String, argEn.JobTitle);
                            _DatabaseFactory.AddInParameter(ref cmd, "@user_expiry", DbType.DateTime, Helper.DateConversion(argEn.StaffExpiryDtTm));
                            _DbParameterCollection = cmd.Parameters;

                            int liRowAffected = _DatabaseFactory.ExecuteNonQuery(Helper.GetDataBaseType, cmd,
                                DataBaseConnectionString, sqlCmd, _DbParameterCollection);

                            if (liRowAffected > -1)
                            {
                                lbRes = true;
                                //if (argEn.WorkflowGroup == 1)
                                //{
                                //    sqlUser = "INSERT INTO UR_UserRights(UserGroupId,MenuID,IsAdd,IsEdit,IsDelete,IsView,IsPrint,IsPost,IsOthers,DefaultMode,LastUpdatedBy,LastUpdatedDtTm) VALUES (" +
                                //                argEn.UserGroupId + ",114,true,true,true,true,true,true,true,false," + MaxGeneric.clsGeneric.AddQuotes(argEn.LastUpdatedBy) + ",current_date) ";                                                
                                    
                                //    if (!FormHelp.IsBlank(sqlUser))
                                //    {
                                //        int liUserAffected = _DatabaseFactory.ExecuteSqlStatement(Helper.GetDataBaseType,
                                //             DataBaseConnectionString, sqlUser);

                                //        if (liUserAffected > -1)
                                //            lbRes = true;                                        
                                //    }
                                //}
                            }
                            else
                                throw new Exception("Insertion Failed! No Row has been updated...");
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
        /// Method to Update Users 
        /// </summary>
        /// <param name="argEn">Users Entity is an Input.</param>
        /// <returns>Returns Boolean</returns>
        //public bool Update(UsersEn argEn)
        //{
        //    bool lbRes = false;
        //    int iOut = 0;
        //    string sqlCmd = "Select count(*) as cnt From UR_Users WHERE UserID != @UserID and UserName = @UserName";
        //    try
        //    {
        //        if (!FormHelp.IsBlank(sqlCmd))
        //        {
        //            DbCommand cmdSel = _DatabaseFactory.GetDbCommand(Helper.GetDataBaseType, sqlCmd, DataBaseConnectionString);
        //            _DatabaseFactory.AddInParameter(ref cmdSel, "@UserID", DbType.Int32, argEn.UserID);
        //            _DatabaseFactory.AddInParameter(ref cmdSel, "@UserName", DbType.String, argEn.UserName);
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
        //                sqlCmd = "UPDATE UR_Users SET UserName = @UserName, Password = @Password, UserGroupId = @UserGroupId, UserStatus = @UserStatus, RecStatus = @RecStatus, Email = @Email, LastUpdatedBy = @LastUpdatedBy, LastUpdatedDtTm = @LastUpdatedDtTm WHERE UserID = @UserID and UserName = @UserName";
        //                if (!FormHelp.IsBlank(sqlCmd))
        //                {
        //                    DbCommand cmd = _DatabaseFactory.GetDbCommand(Helper.GetDataBaseType, sqlCmd, DataBaseConnectionString);
        //                    _DatabaseFactory.AddInParameter(ref cmd, "@UserID", DbType.Int32, argEn.UserID);
        //                    _DatabaseFactory.AddInParameter(ref cmd, "@UserName", DbType.String, argEn.UserName);
        //                    _DatabaseFactory.AddInParameter(ref cmd, "@Password", DbType.String, argEn.Password);
        //                    _DatabaseFactory.AddInParameter(ref cmd, "@UserGroupId", DbType.Int32, argEn.UserGroupId);
        //                    _DatabaseFactory.AddInParameter(ref cmd, "@UserStatus", DbType.String, argEn.UserStatus);
        //                    _DatabaseFactory.AddInParameter(ref cmd, "@RecStatus", DbType.Boolean, argEn.RecStatus);
        //                    _DatabaseFactory.AddInParameter(ref cmd, "@Email", DbType.String, argEn.Email);
        //                    _DatabaseFactory.AddInParameter(ref cmd, "@LastUpdatedBy", DbType.String, argEn.LastUpdatedBy);
        //                    _DatabaseFactory.AddInParameter(ref cmd, "@LastUpdatedDtTm", DbType.DateTime, argEn.LastUpdatedDtTm);
        //                    _DbParameterCollection = cmd.Parameters;

        //                    int liRowAffected = _DatabaseFactory.ExecuteNonQuery(Helper.GetDataBaseType, cmd,
        //                        DataBaseConnectionString, sqlCmd, _DbParameterCollection);

        //                    if (liRowAffected > -1)
        //                        lbRes = true;
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
        //Modified Zoya 22/2/2016
        public bool Update(UsersEn argEn)
        {
            bool lbRes = false;
            int iOut = 0;
            string sqlCmd = "Select count(*) as cnt From UR_Users WHERE UserID != @UserID and UserName = @UserName";


            try
            {
                if (!FormHelp.IsBlank(sqlCmd))
                {
                    DbCommand cmdSel = _DatabaseFactory.GetDbCommand(Helper.GetDataBaseType, sqlCmd, DataBaseConnectionString);
                    _DatabaseFactory.AddInParameter(ref cmdSel, "@UserID", DbType.Int32, argEn.UserID);
                    _DatabaseFactory.AddInParameter(ref cmdSel, "@UserName", DbType.String, argEn.UserName);
                    _DbParameterCollection = cmdSel.Parameters;

                    using (IDataReader dr = _DatabaseFactory.GetIDataReader(Helper.GetDataBaseType, cmdSel,
                        DataBaseConnectionString, sqlCmd, _DbParameterCollection).CreateDataReader())
                    {
                        if (dr.Read())
                            iOut = clsGeneric.NullToInteger(dr["cnt"]);
                        if (iOut > 0)
                            throw new Exception("Record Already Exist");
                    }

                    int iOut1 = 0;
                    string sqlCmd2 = "Select count(*) as cnt From UR_Users WHERE  UserID != @UserID and email = @email";

                    if (!FormHelp.IsBlank(sqlCmd2))
                    {
                        DbCommand cmdS = _DatabaseFactory.GetDbCommand(Helper.GetDataBaseType, sqlCmd2, DataBaseConnectionString);
                        _DatabaseFactory.AddInParameter(ref cmdS, "@UserID", DbType.Int32, argEn.UserID);
                        _DatabaseFactory.AddInParameter(ref cmdS, "@email", DbType.String, argEn.Email);
                        _DbParameterCollection = cmdS.Parameters;

                        using (IDataReader dr = _DatabaseFactory.GetIDataReader(Helper.GetDataBaseType, cmdS,
                            DataBaseConnectionString, sqlCmd2, _DbParameterCollection).CreateDataReader())
                        {
                            if (dr.Read())
                                iOut1 = clsGeneric.NullToInteger(dr["cnt"]);
                            if (iOut1 > 0)
                                throw new Exception("Email Already Exist");
                        }
                    }

                    if (iOut == 0 && iOut1 == 0)
                    {
                        sqlCmd = "UPDATE UR_Users SET UserName = @UserName, Password = @Password, UserGroupId = @UserGroupId, Department = @Department, UserStatus = @UserStatus, RecStatus = @RecStatus, " +
                            "Email = @Email, LastUpdatedBy = @LastUpdatedBy, LastUpdatedDtTm = @LastUpdatedDtTm, staff_no = @staff_no, staff_name = @staff_name, " +
                            "designation = @designation, user_expiry = @user_expiry WHERE UserID = @UserID and UserName = @UserName";

                        if (!FormHelp.IsBlank(sqlCmd))
                        {
                            DbCommand cmd = _DatabaseFactory.GetDbCommand(Helper.GetDataBaseType, sqlCmd, DataBaseConnectionString);
                            _DatabaseFactory.AddInParameter(ref cmd, "@UserID", DbType.Int32, argEn.UserID);
                            _DatabaseFactory.AddInParameter(ref cmd, "@UserName", DbType.String, argEn.UserName);
                            _DatabaseFactory.AddInParameter(ref cmd, "@Password", DbType.String, argEn.Password);
                            _DatabaseFactory.AddInParameter(ref cmd, "@UserGroupId", DbType.Int32, argEn.UserGroupId);
                            _DatabaseFactory.AddInParameter(ref cmd, "@Department", DbType.String, argEn.Department);
                            _DatabaseFactory.AddInParameter(ref cmd, "@UserStatus", DbType.String, argEn.UserStatus);
                            _DatabaseFactory.AddInParameter(ref cmd, "@RecStatus", DbType.Boolean, argEn.RecStatus);
                            _DatabaseFactory.AddInParameter(ref cmd, "@Email", DbType.String, argEn.Email);
                            _DatabaseFactory.AddInParameter(ref cmd, "@LastUpdatedBy", DbType.String, argEn.LastUpdatedBy);                            
                            _DatabaseFactory.AddInParameter(ref cmd, "@LastUpdatedDtTm", DbType.DateTime, Helper.DateConversion(argEn.LastUpdatedDtTm));
                            //Added 3/8/16
                            _DatabaseFactory.AddInParameter(ref cmd, "@staff_no", DbType.String, argEn.StaffID);
                            _DatabaseFactory.AddInParameter(ref cmd, "@staff_name", DbType.String, argEn.StaffName);
                            _DatabaseFactory.AddInParameter(ref cmd, "@designation", DbType.String, argEn.JobTitle);
                            _DatabaseFactory.AddInParameter(ref cmd, "@user_expiry", DbType.DateTime, Helper.DateConversion(argEn.StaffExpiryDtTm));
                            _DbParameterCollection = cmd.Parameters;

                            int liRowAffected = _DatabaseFactory.ExecuteNonQuery(Helper.GetDataBaseType, cmd,
                                DataBaseConnectionString, sqlCmd, _DbParameterCollection);

                            if (liRowAffected > -1)
                                lbRes = true;
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
        /// Method to Delete Users 
        /// </summary>
        /// <param name="argEn">Users Entity is an Input.UserName as Input Property.</param>
        /// <returns>Returns Boolean</returns>
        //public bool Delete(UsersEn argEn)
        //{
        //    bool lbRes = false;
        //    string sqlCmd = "DELETE FROM UR_Users WHERE UserID = @UserID";
        //    try
        //    {
        //        if (!FormHelp.IsBlank(sqlCmd))
        //        {
        //            DbCommand cmd = _DatabaseFactory.GetDbCommand(Helper.GetDataBaseType, sqlCmd, DataBaseConnectionString);
        //            _DatabaseFactory.AddInParameter(ref cmd, "@UserID", DbType.Int32, argEn.UserID);
        //            _DbParameterCollection = cmd.Parameters;

        //            int liRowAffected = _DatabaseFactory.ExecuteNonQuery(Helper.GetDataBaseType, cmd,
        //                DataBaseConnectionString, sqlCmd, _DbParameterCollection);

        //            if (liRowAffected > -1)
        //                lbRes = true;
        //            else
        //                throw new Exception("Delete Failed! No Row has been updated...");
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //    return lbRes;
        //}
        public bool Delete(List<UsersEn> argEn)
        {
            //Editted by Zoya @23/02/2016
            bool lbRes = false;
            string sqlCmd;

            try
            {
                for (int i = 0; i < argEn.Count; i++)
                {
                    sqlCmd = "DELETE FROM UR_Users WHERE UserID = @UserID ";

                    if (!FormHelp.IsBlank(sqlCmd))
                    {
                        DbCommand cmd = _DatabaseFactory.GetDbCommand(Helper.GetDataBaseType, sqlCmd, DataBaseConnectionString);
                        _DatabaseFactory.AddInParameter(ref cmd, "@UserID", DbType.Int32, argEn[i].UserID);
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
            //Done Editted by Zoya @23/02/2016
            catch (Exception ex)
            {
                throw ex;
            }
            return lbRes;
        }

        #endregion

        #region LoadObject 

        /// <summary>
        /// Method to Load Users Entity 
        /// </summary>
        /// <param name="argReader">IDataReader Object is an Input.</param>
        /// <returns>Returns Users Entity</returns>
        private UsersEn LoadObject(IDataReader argReader)
        {
            UsersEn _UsersEn = new UsersEn();
            _UsersEn.UserID = GetValue<int>(argReader, "UserID");
            _UsersEn.UserName = GetValue<string>(argReader, "UserName");
            _UsersEn.Password = GetValue<string>(argReader, "Password");
            _UsersEn.UserGroupId = GetValue<int>(argReader, "UserGroupId");
            _UsersEn.Department = GetValue<string>(argReader, "Department");
            _UsersEn.UserStatus = GetValue<string>(argReader, "UserStatus");
            _UsersEn.RecStatus = GetValue<bool>(argReader, "RecStatus");
            _UsersEn.WorkflowRole = GetValue<string>(argReader, "Workflow_Role");
            _UsersEn.WorkflowGroup = GetValue<int>(argReader, "Workflow_Group");
            _UsersEn.Email = GetValue<string>(argReader, "Email");
            _UsersEn.LastUpdatedBy = GetValue<string>(argReader, "LastUpdatedBy");
            _UsersEn.LastUpdatedDtTm = GetValue<DateTime>(argReader, "LastUpdatedDtTm");
            //Added 3/8/2016
            _UsersEn.StaffID = GetValue<string>(argReader, "staff_no");
            _UsersEn.StaffName = GetValue<string>(argReader, "staff_name");
            _UsersEn.JobTitle = GetValue<string>(argReader, "designation");
            _UsersEn.StaffExpiryDtTm = GetValue<DateTime>(argReader, "user_expiry");
            return _UsersEn;
        }


        /// <summary>
        /// Method to Load Users Entity  withyout password
        /// </summary>
        /// <param name="argReader">IDataReader Object is an Input.</param>
        /// <returns>Returns Users Entity</returns>
        private UsersEn LoadObjectNoPassword(IDataReader argReader)
        {
            UsersEn _UsersEn = new UsersEn();
            _UsersEn.UserID = GetValue<int>(argReader, "UserID");
            _UsersEn.UserName = GetValue<string>(argReader, "UserName");
            _UsersEn.Department = GetValue<string>(argReader, "Department");

            //Editted by Zoya @3/03/2016
            //_UsersEn.Description = GetValue<string>(argReader, "Description");           
            _UsersEn.UserGroupName = GetValue<string>(argReader, "UserGroupName");
            //End Added by Zoya @3/03/2016

            //_UsersEn.Password = GetValue<string>(argReader, "Password");
            //_UsersEn.UserGroupId = GetValue<int>(argReader, "UserGroupId");
            _UsersEn.UserStatus = GetValue<string>(argReader, "UserStatus");
            //_UsersEn.RecStatus = GetValue<bool>(argReader, "RecStatus");
            //_UsersEn.Email = GetValue<string>(argReader, "Email");
            //_UsersEn.LastUpdatedBy = GetValue<string>(argReader, "LastUpdatedBy");
            //_UsersEn.LastUpdatedDtTm = GetValue<DateTime>(argReader, "LastUpdatedDtTm");

            return _UsersEn;

            return _UsersEn;
        }


        /// <summary>
        /// Method to Load UserRights Entity 
        /// </summary>
        /// <param name="argReader">IDataReader Object is an Input.</param>
        /// <returns>Returns UserRights Entity</returns>
        private UserRightsEn LoadUserRightsObject(IDataReader argReader)
        {
            UserRightsEn loItem = new UserRightsEn();
            loItem.UserGroup = GetValue<int>(argReader, "UserGroupId");
            loItem.MenuID = GetValue<int>(argReader, "MenuID");
            loItem.IsAdd = GetValue<bool>(argReader, "IsAdd");
            loItem.IsEdit = GetValue<bool>(argReader, "IsEdit");
            loItem.IsDelete = GetValue<bool>(argReader, "IsDelete");
            loItem.IsView = GetValue<bool>(argReader, "IsView");
            loItem.IsPrint = GetValue<bool>(argReader, "IsPrint");
            loItem.IsPost = GetValue<bool>(argReader, "IsPost");
            loItem.IsOthers = GetValue<bool>(argReader, "IsOthers");
            loItem.IsAddModeDefault = GetValue<bool>(argReader, "DefaultMode");
            loItem.LastUser = GetValue<string>(argReader, "LastUpdatedBy");
            loItem.LastDtTm = GetValue<DateTime>(argReader, "LastUpdatedDtTm");

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

        #region GetUserRights 

        /// <summary>
        /// Method to Get UserRights
        /// </summary>
        /// <param name="MenuId">GroupId  as Input Property.</param>
        /// <param name="GroupId">MenuId as Input Property.</param>
        /// <returns>Returns UserRights Entity</returns>
        public UserRightsEn GetUserRights(int MenuId, int GroupId)
        {
            #region Declarations 

            //create instances
            UserRightsEn UserRights = new UserRightsEn();

            //vairiable declarations
            string SqlStatement = null;

            #endregion

            try
            {
                //Build Sql Statement - Start
                SqlStatement =  "SELECT * FROM UR_UserRights WHERE ";
                SqlStatement += " UserGroupId = " + GroupId;
                SqlStatement += " AND MenuID = " + MenuId;
                //Build Sql Statement - Stop

                //Get User Login Details - Start
                IDataReader _IDataReader = _DatabaseFactory.ExecuteReader(Helper.GetDataBaseType,DataBaseConnectionString,
                    SqlStatement).CreateDataReader();
                //Get User Login Details - Stop

                //if user login details available - Start
                if (_IDataReader != null)
                {
                    if (_IDataReader.Read())
                    {
                        //load data to entities
                        UserRights = LoadUserRightsObject(_IDataReader);
                    }
                    _IDataReader.Close();
                    _IDataReader.Dispose();
                    return UserRights;
                }
                //if user login details available - Stop

                return null;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        
        }
        #endregion

        #region DataGrid

        /// <summary>
        /// Data Grid
        /// </summary>
        /// <param name="argEn">Users Entity  as an Input.UserName and Password as Input Properties.</param>
        /// <returns>Returns Users Entity</returns>
        public List<UsersEn> DataGrid(UsersEn argEn)
        {

            #region Declarations

            //Create Instances
            List<UsersEn> UserList = new List<UsersEn>();

            //vairiable declarations
            string SqlStatement = null;

            #endregion

            try
            {
                //****Commented by Zoya @3/03/2016
                //// With...
                //// Build Sql Statement - Start
                //SqlStatement = "SELECT US.UserID, US.UserName, SD.Department, UUG.Description, ";
                //SqlStatement += "CASE WHEN US.UserStatus = \'True\' THEN \'Active\' ELSE \'Inactive\' END UserStatus ";
                //SqlStatement += "FROM UR_Users US ";
                //SqlStatement += "LEFT JOIN SAS_Department SD ON US.Department = SD.DepartmentID ";
                //SqlStatement += "LEFT JOIN UR_UserGroups UUG ON US.UserGroupId = UUG.UserGroupId ";
                //if (!FormHelp.IsBlank(argEn.SearchCriteria))
                //{
                //    SqlStatement = (SqlStatement + (" WHERE US.UserName = \'"
                //                + (argEn.SearchCriteria + ("\' OR SD.Department LIKE \'%"
                //                + (argEn.SearchCriteria + "%\' ")))));
                //    SqlStatement = (SqlStatement + ("OR UUG.Description LIKE \'%"
                //                + (argEn.SearchCriteria + "%\' ")));
                //}

                //SqlStatement += " ORDER BY US.UserName";
                //// Return Data Set

                //****Edited by Zoya @3/03/2016

                // With...
                // Build Sql Statement - Start
                SqlStatement = "SELECT US.UserID, US.UserName, SD.Department, UUG.UserGroupName, ";
                SqlStatement += "CASE WHEN US.UserStatus = \'True\' THEN \'Active\' ELSE \'Inactive\' END UserStatus ";
                SqlStatement += "FROM UR_Users US ";
                SqlStatement += "LEFT JOIN SAS_Department SD ON US.Department = SD.DepartmentID ";
                SqlStatement += "LEFT JOIN UR_UserGroups UUG ON US.UserGroupId = UUG.UserGroupId ";
                if (!FormHelp.IsBlank(argEn.SearchCriteria))
                {
                    SqlStatement = (SqlStatement + (" WHERE US.UserName = \'"
                                + (argEn.SearchCriteria + ("\' OR SD.Department LIKE \'%"
                                + (argEn.SearchCriteria + "%\' ")))));
                    SqlStatement = (SqlStatement + ("OR UUG.UserGroupName LIKE \'%"
                                + (argEn.SearchCriteria + "%\' ")));
                }

                SqlStatement += " ORDER BY US.UserName";
                // Return Data Set

                //****End Edited by Zoya @3/03/2016

                IDataReader _IDataReader = _DatabaseFactory.ExecuteReader(Helper.GetDataBaseType, DataBaseConnectionString, SqlStatement).CreateDataReader();

                //if user login details available - Start
                if (_IDataReader != null)
                {
                    while (_IDataReader.Read())
                    {
                        //load data to entities
                        // UsersEn _UsersEn = new UsersEn();
                        UsersEn _UsersEn = LoadObjectNoPassword(_IDataReader);
                        UserList.Add(_UsersEn);

                        //_UsersEn.UserID = GetValue<int>(_IDataReader, "UserID");
                        //_UsersEn.UserName = GetValue<string>(_IDataReader, "UserName");
                        //_UsersEn.Password = GetValue<string>(_IDataReader, "Password");
                        //_UsersEn.UserGroupId = GetValue<int>(_IDataReader, "UserGroupId");
                        //_UsersEn.UserStatus = GetValue<string>(_IDataReader, "UserStatus");
                        //_UsersEn.RecStatus = GetValue<bool>(_IDataReader, "RecStatus");
                        //_UsersEn.Email = GetValue<string>(_IDataReader, "Email");
                        //_UsersEn.LastUpdatedBy = GetValue<string>(_IDataReader, "LastUpdatedBy");
                        //_UsersEn.LastUpdatedDtTm = GetValue<DateTime>(_IDataReader, "LastUpdatedDtTm");

                    }
                    _IDataReader.Close();
                    _IDataReader.Dispose();

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return UserList;
        }

        #endregion

        #region GetUser
        /// <summary>
        /// Get User
        /// </summary>
        /// <param name="argEn">Users Entity  as an Input.</param>
        /// <returns>Returns Users Entity</returns>
        public List<UsersEn> GetUser(UsersEn UserEn)
        {
          // Declarations

            //Create Instances
            List<UsersEn> loEnList = new List<UsersEn>();

            //vairiable declarations
            string SqlStatement = null;

            
            try
            {
                // With...
                // Build Sql Statement
                SqlStatement = ("SELECT * FROM UR_Users WHERE UserID = " + UserEn.UserID);

                if (!FormHelp.IsBlank(SqlStatement))
                {
                    using (IDataReader _IDataReader = _DatabaseFactory.ExecuteDataSet(Helper.GetDataBaseType, DataBaseConnectionString, SqlStatement).CreateDataReader())
                    {
                        while (_IDataReader.Read())
                        {
                            UsersEn _UsersEn = LoadObject(_IDataReader);
                            loEnList.Add(_UsersEn);
                        }
                        _IDataReader.Close();
                    }
                }
                             
            }
            catch (Exception ex)
            {
                throw ex;
            }
            // Return Data Set
            return loEnList;
        }

        #endregion

        #region CheckDuplicateStaff

        public int CheckDuplicateStaff(string StaffId)
        {
            int IntProcess = 0;
            string sqlGetStaff = "SELECT COUNT(*) AS Staff FROM UR_Users WHERE staff_no = " + clsGeneric.AddQuotes(StaffId);

            try
            {
                if (!FormHelp.IsBlank(sqlGetStaff))
                {
                    using (IDataReader loReader = _DatabaseFactory.ExecuteReader(Helper.GetDataBaseType,
                        DataBaseConnectionString, sqlGetStaff).CreateDataReader())
                    {
                        while (loReader.Read())
                        {
                            IntProcess = clsGeneric.NullToInteger(loReader["Staff"]);
                        }
                        loReader.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return IntProcess;
        }

        #endregion

        #region InsertMenuByUser

        public bool InsertMenuByUser(List<UserRightsEn> argEn)
        {
            try
            {
                if (argEn.Count != 0)
                {
                    DeleteMenu(argEn[0]);
                }
                for (int i = 0; i < argEn.Count; i++)
                {
                    InsertMenu(argEn[i]);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return true;
        }

        #endregion

        #region DeleteMenu

        /// <returns>Returns Boolean</returns>
        public bool DeleteMenu(UserRightsEn argEn)
        {
            bool lbRes = false;
            string sqlCmd = "DELETE FROM ur_usermenu WHERE userid =@UserId";
            try
            {
                if (!FormHelp.IsBlank(sqlCmd))
                {
                    DbCommand cmd = _DatabaseFactory.GetDbCommand(Helper.GetDataBaseType, sqlCmd, DataBaseConnectionString);
                    _DatabaseFactory.AddInParameter(ref cmd, "@UserId", DbType.Int32, argEn.UserID);
                    _DbParameterCollection = cmd.Parameters;

                    int liRowAffected = _DatabaseFactory.ExecuteNonQuery(Helper.GetDataBaseType, cmd,
                        DataBaseConnectionString, sqlCmd, _DbParameterCollection);

                    if (liRowAffected > -1)
                        lbRes = true;
                    else
                        throw new Exception("Insertion Failed! No Row has been updated...");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return lbRes;
        }

        #endregion

        #region InsertMenu

        /// <summary>
        /// Method to Insert UserRights 
        /// </summary>
        /// <param name="argEn">UserRights Entity is an Input.</param>
        /// <returns>Returns Boolean</returns>
        public bool InsertMenu(UserRightsEn argEn)
        {
            bool lbRes = false;

            string sqlCmd;
            try
            {
                //Checking for Duplicates
                sqlCmd = "INSERT INTO ur_usermenu(userid,menuid,updatedby,updateddttm) VALUES (@UserId, @MenuID, @UpdatedBy, @UpdatedDtTm) ";
                if (!FormHelp.IsBlank(sqlCmd))
                {
                    DbCommand cmd = _DatabaseFactory.GetDbCommand(Helper.GetDataBaseType, sqlCmd, DataBaseConnectionString);
                    _DatabaseFactory.AddInParameter(ref cmd, "@UserId", DbType.Int32, argEn.UserID);
                    _DatabaseFactory.AddInParameter(ref cmd, "@MenuID", DbType.Int32, argEn.MenuID);
                    _DatabaseFactory.AddInParameter(ref cmd, "@UpdatedBy", DbType.String, argEn.LastUser);
                    _DatabaseFactory.AddInParameter(ref cmd, "@UpdatedDtTm", DbType.DateTime, Helper.DateConversion(argEn.LastDtTm));                    
                    _DbParameterCollection = cmd.Parameters;

                    int liRowAffected = _DatabaseFactory.ExecuteNonQuery(Helper.GetDataBaseType, cmd,
                        DataBaseConnectionString, sqlCmd, _DbParameterCollection);

                    if (liRowAffected > -1)
                        lbRes = true;
                    else
                        throw new Exception("Insertion Failed! No Row has been updated...");
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return lbRes;
        }

        #endregion

        #region GetWorkflowList()

        public DataSet GetWorkflowList()
        {
            string SqlStatement;

            try
            {
                SqlStatement = "SELECT * FROM ur_users users " +
                               "INNER JOIN ur_usergroups usergroups ON usergroups.usergroupid = users.usergroupid " +
                               "INNER JOIN sas_department dept ON dept.departmentid = users.department";

                if (!FormHelp.IsBlank(SqlStatement))
                {
                    //Binding Sas Account Details - start
                    DataSet _DataSet = _DatabaseFactory.ExecuteDataSet(Helper.GetDataBaseType,
                        DataBaseConnectionString, SqlStatement);
                    //Sas Account status - Ended
                    return _DataSet;
                }
            }
            catch (Exception ex)
            {
                MaxModule.Helper.LogError(ex.Message);
            }

            return null;
        }

        #endregion

    }
        
}



