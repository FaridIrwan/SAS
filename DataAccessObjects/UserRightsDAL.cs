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
    /// Class to handle all the UserRights Methods.
    /// </summary>
    public class UserRightsDAL
    {
        #region Global Declarations 

        private DbParameterCollection _DbParameterCollection = null;

        private MaxModule.DatabaseProvider _DatabaseFactory =
            new MaxModule.DatabaseProvider();

        private string DataBaseConnectionString = Helper.
           GetConnectionString();

        #endregion

        public UserRightsDAL()
        {
        }

        #region GetMenuByUserGroup 

        /// <summary>
        /// Method to Get List of Menus By UserGroups
        /// </summary>
        /// <param name="MenuName">MenuName is an Input.</param>
        /// <param name="UserGroupID">UserGroupID  is an Input.</param>
        /// <returns>Returns List of Menus</returns>
        public List<MenuMasterEn> GetMenuByUserGroup(string MenuName, int UserGroupID, string optional_params = "")
        {
            #region Declarations 

            //Create Instances - Start
            MenuMasterEn MenuDetails = new MenuMasterEn();
            List<MenuMasterEn> MenuList = new List<MenuMasterEn>();
            //Create Instances - Stop

            //variable declarations
            string SqlStatement = null;

            #endregion

            try
            {
                //Build Sql Statement - Start
                SqlStatement = "SELECT UR_MenuMaster.MenuID, UR_MenuMaster.MenuName, UR_MenuMaster.PageName, UR_MenuMaster.PageDescription, " +
                         " UR_MenuMaster.PageUrl, UR_MenuMaster.ImageUrl, UR_MenuMaster.Status, UR_MenuMaster.PageOrder, UR_MenuMaster.LastUpdatedBy, " +
                         " UR_MenuMaster.LastUpdatedDtTm, UR_UserRights.IsAdd, UR_UserRights.IsEdit, UR_UserRights.IsDelete, UR_UserRights.IsView, " +
                         " UR_UserRights.IsPrint, UR_UserRights.IsPost, UR_UserRights.IsOthers FROM UR_MenuMaster INNER JOIN UR_UserRights " +
                         " ON UR_MenuMaster.MenuID = UR_UserRights.MenuID WHERE UR_UserRights.UserGroupId = " + UserGroupID +
                         " AND UR_MenuMaster.MenuName = " + clsGeneric.AddQuotes(MenuName) + " AND UR_MenuMaster.Status = 'true' ";

                if (MenuName == "Setup")
                {
                    SqlStatement += " AND UR_MenuMaster.MenuID <> 13";
                }

                if (MenuName == "Process")
                {
                    SqlStatement += " AND UR_MenuMaster.MenuID <> 114";
                }

                if (MenuName == "Reports" && optional_params != "")
                {
                    if (optional_params == "IsApprover")
                    {
                        SqlStatement += " AND UR_MenuMaster.MenuID = 18";
                    }
                    else
                    {
                        SqlStatement += "";
                    }
                }

                SqlStatement += " ORDER BY UR_MenuMaster.PageOrder";
                //Build Sql Statement - Stop

                //Get User Login Details - Start
                IDataReader _IDataReader = _DatabaseFactory.ExecuteReader(Helper.GetDataBaseType,DataBaseConnectionString,
                    SqlStatement).CreateDataReader();
                //Get User Login Details - Stop

                //if user login details available - Start
                if (_IDataReader != null)
                {
                    while (_IDataReader.Read())
                    {
                        //load data to entities
                        MenuDetails = LoadMenuObject(_IDataReader);
                        MenuList.Add(MenuDetails);
                    }
                    _IDataReader.Close();
                    _IDataReader.Dispose();
                    return MenuList;
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

        #region InsertUserRights 

        /// <summary>
        /// Method to Insert List of UserRights 
        /// </summary>
        /// <param name="argEn">List of UserRights Entity is an Input.</param>
        /// <returns>Returns Boolean</returns>
        public bool InsertUserRights (List<UserRightsEn> argEn)
        {
            try
            {
                if (argEn.Count != 0)
                {
                    Delete(argEn[0]);
                }
                for (int i = 0; i < argEn.Count; i++)
                {
                    Insert(argEn[i]);
                }
            }
            catch(Exception ex)
            {
                throw ex;
            }
            return true;
        }

        #endregion

        #region GetUserMenu 

        /// <summary>
        /// Method to Get List of All UserRights  by UserGroupID
        /// </summary>
        /// <param name="UserGroupID">UserGroupID  as an Input.</param>
        /// <returns>Returns List of UserRights</returns>
        /// edited by Hafiz @ 30/9/2016
        /// remove "UR_MenuMaster.MenuID <> 114 AND" 
        public List<UserRightsEn> GetUserMenu(int UserGroupID)
        {
            List<UserRightsEn> loEnList = new List<UserRightsEn>();
            string sqlCmd = "SELECT UR_MenuMaster.MenuID, UR_MenuMaster.MenuName, UR_MenuMaster.PageName, UR_UserRights.IsAdd, " +
                "UR_UserRights.IsEdit, UR_UserRights.IsDelete, UR_UserRights.IsView, UR_UserRights.IsPost, UR_UserRights.IsOthers, UR_UserRights.IsPrint," +
                "UR_UserRights.DefaultMode,UR_UserRights.UserGroupId FROM UR_MenuMaster INNER JOIN " +
                "UR_UserRights ON UR_MenuMaster.MenuID = UR_UserRights.MenuID WHERE " +
                "UR_MenuMaster.Status = true AND " +
                "UR_UserRights.UserGroupId =" + UserGroupID + " order by UR_MenuMaster.MenuName desc" ;
             
            try
            {
                if (!FormHelp.IsBlank(sqlCmd))
                {
                    using (IDataReader loReader = _DatabaseFactory.ExecuteReader(Helper.GetDataBaseType,
                        DataBaseConnectionString, sqlCmd).CreateDataReader())
                    {
                        while (loReader.Read())
                        {
                            UserRightsEn loItem = LoadObject(loReader);
                            loItem.MenuName = GetValue<string>(loReader, "MenuName");
                            loItem.PageName = GetValue<string>(loReader, "PageName");
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

        #region GetMenuList 

        /// <summary>
        /// Method to Get List of All MenuID
        /// </summary>
        /// <param name="argEn">UserRights Entity  as an Input.</param>
        /// <returns>Returns List of UserRights</returns>
        public List<UserRightsEn> GetMenuList(UserRightsEn argEn)
        {
            List<UserRightsEn> loEnList = new List<UserRightsEn>();
            string sqlCmd = "select * from UR_MenuMaster";
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
        /// Method to Get List of Active UserRights
        /// </summary>
        /// <param name="argEn">UserRights Entity as an Input.</param>
        /// <returns>Returns List of UserRights</returns>
        public List<UserRightsEn> GetList(UserRightsEn argEn)
        {
            List<UserRightsEn> loEnList = new List<UserRightsEn>();
            string sqlCmd = "select * from UR_UserRights";
            try
            {
                if (!FormHelp.IsBlank(sqlCmd))
                {
                    using (IDataReader loReader = _DatabaseFactory.ExecuteReader(Helper.GetDataBaseType,
                        DataBaseConnectionString, sqlCmd).CreateDataReader())
                    {
                        while (loReader.Read())
                        {
                            UserRightsEn loItem = LoadObject(loReader);
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
        /// Method to Get UserRights Entity
        /// </summary>
        /// <param name="argEn">UserRights Entity is an Input.</param>
        /// <returns>Returns UserRights Entity</returns>
        public UserRightsEn GetItem(UserRightsEn argEn)
        {
            UserRightsEn loItem = new UserRightsEn();
            string sqlCmd = "Select * FROM UR_UserRights ";
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

        #region Insert 

        /// <summary>
        /// Method to Insert UserRights 
        /// </summary>
        /// <param name="argEn">UserRights Entity is an Input.</param>
        /// <returns>Returns Boolean</returns>
        public bool Insert(UserRightsEn argEn)
        {
            bool lbRes = false;

            string sqlCmd;
            try
            {
                //Checking for Duplicates
                sqlCmd = "INSERT INTO UR_UserRights(UserGroupId,MenuID,IsAdd,IsEdit,IsDelete,IsView,IsPrint,IsPost,IsOthers,DefaultMode,LastUpdatedBy,LastUpdatedDtTm) VALUES (@UserGroupId,@MenuID,@IsAdd,@IsEdit,@IsDelete,@IsView,@IsPrint,@IsPost,@IsOthers,@DefaultMode,@LastUpdatedBy,@LastUpdatedDtTm) ";
                if (!FormHelp.IsBlank(sqlCmd))
                {
                    DbCommand cmd = _DatabaseFactory.GetDbCommand(Helper.GetDataBaseType, sqlCmd, DataBaseConnectionString);
                    _DatabaseFactory.AddInParameter(ref cmd, "@UserGroupId", DbType.Int32, argEn.UserGroup);
                    _DatabaseFactory.AddInParameter(ref cmd, "@MenuID", DbType.Int32, argEn.MenuID);
                    _DatabaseFactory.AddInParameter(ref cmd, "@IsAdd", DbType.Boolean, argEn.IsAdd);
                    _DatabaseFactory.AddInParameter(ref cmd, "@IsEdit", DbType.Boolean, argEn.IsEdit);
                    _DatabaseFactory.AddInParameter(ref cmd, "@IsDelete", DbType.Boolean, argEn.IsDelete);
                    _DatabaseFactory.AddInParameter(ref cmd, "@IsView", DbType.Boolean, argEn.IsView);
                    _DatabaseFactory.AddInParameter(ref cmd, "@IsPrint", DbType.Boolean, argEn.IsPrint);
                    _DatabaseFactory.AddInParameter(ref cmd, "@IsPost", DbType.Boolean, argEn.IsPost);
                    _DatabaseFactory.AddInParameter(ref cmd, "@IsOthers", DbType.Boolean, argEn.IsOthers);
                    _DatabaseFactory.AddInParameter(ref cmd, "@DefaultMode", DbType.Boolean, argEn.IsAddModeDefault);
                    _DatabaseFactory.AddInParameter(ref cmd, "@LastUpdatedBy", DbType.String, argEn.LastUser);
                    _DatabaseFactory.AddInParameter(ref cmd, "@LastUpdatedDtTm", DbType.DateTime, Helper.DateConversion(argEn.LastDtTm));                                                
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

        #region Update 

        /// <summary>
        /// Method to Update UserRights 
        /// </summary>
        /// <param name="argEn">UserRights Entity is an Input.</param>
        /// <returns>Returns Boolean</returns>
        public bool Update(UserRightsEn argEn)
        {
            bool lbRes = false;
            int iOut = 0;
            string sqlCmd = "Select count(*) as cnt From UR_UserRights WHERE UserGroupId = @UserGroupId";
            try
            {
                if (!FormHelp.IsBlank(sqlCmd))
                {
                    using (IDataReader dr = _DatabaseFactory.ExecuteReader(Helper.GetDataBaseType,
                        DataBaseConnectionString, sqlCmd).CreateDataReader())
                    {
                        if (dr.Read())
                            iOut = clsGeneric.NullToInteger(dr["cnt"]);
                        if (iOut > 0)
                            throw new Exception("Record Already Exist");
                    }
                    if (iOut == 0)
                    {
                        sqlCmd = "UPDATE UR_UserRights SET UserGroupId = @UserGroupId, MenuID = @MenuID, IsAdd = @IsAdd, IsEdit = @IsEdit, IsDelete = @IsDelete, IsView = @IsView, IsPrint = @IsPrint, IsPost = @IsPost, IsOthers = @IsOthers, DefaultMode = @DefaultMode, LastUpdatedBy = @LastUpdatedBy, LastUpdatedDtTm = @LastUpdatedDtTm WHERE ";
                        if (!FormHelp.IsBlank(sqlCmd))
                        {
                            DbCommand cmd = _DatabaseFactory.GetDbCommand(Helper.GetDataBaseType, sqlCmd, DataBaseConnectionString);
                            _DatabaseFactory.AddInParameter(ref cmd, "@UserGroupId", DbType.Int32, argEn.UserGroup);
                            _DatabaseFactory.AddInParameter(ref cmd, "@MenuID", DbType.Int32, argEn.MenuID);
                            _DatabaseFactory.AddInParameter(ref cmd, "@IsAdd", DbType.Boolean, argEn.IsAdd);
                            _DatabaseFactory.AddInParameter(ref cmd, "@IsEdit", DbType.Boolean, argEn.IsEdit);
                            _DatabaseFactory.AddInParameter(ref cmd, "@IsDelete", DbType.Boolean, argEn.IsDelete);
                            _DatabaseFactory.AddInParameter(ref cmd, "@IsView", DbType.Boolean, argEn.IsView);
                            _DatabaseFactory.AddInParameter(ref cmd, "@IsPrint", DbType.Boolean, argEn.IsPrint);
                            _DatabaseFactory.AddInParameter(ref cmd, "@IsPost", DbType.Boolean, argEn.IsPost);
                            _DatabaseFactory.AddInParameter(ref cmd, "@IsOthers", DbType.Boolean, argEn.IsOthers);
                            _DatabaseFactory.AddInParameter(ref cmd, "@DefaultMode", DbType.Boolean, argEn.IsAddModeDefault);
                            _DatabaseFactory.AddInParameter(ref cmd, "@LastUpdatedBy", DbType.String, argEn.LastUser);
                            _DatabaseFactory.AddInParameter(ref cmd, "@LastUpdatedDtTm", DbType.DateTime, argEn.LastDtTm);
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
        /// Method to Delete UserRights 
        /// </summary>
        /// <param name="argEn">UserRights Entity is an Input.UserGroup as Input Property.</param>
        /// <returns>Returns Boolean</returns>
        public bool Delete(UserRightsEn argEn)
        {
            bool lbRes = false;
            string sqlCmd = "DELETE FROM UR_UserRights WHERE UserGroupId =@UserGroupID";
            try
            {
                if (!FormHelp.IsBlank(sqlCmd))
                {
                    DbCommand cmd = _DatabaseFactory.GetDbCommand(Helper.GetDataBaseType, sqlCmd, DataBaseConnectionString);
                    _DatabaseFactory.AddInParameter(ref cmd, "@UserGroupId", DbType.Int32, argEn.UserGroup);
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

        #region LoadMenuObject 

        /// <summary>
        /// Method to Load MenuMaster Entity 
        /// </summary>
        /// <param name="argReader">IDataReader Object is an Input.</param>
        /// <returns>Returns MenuMaster Entity</returns>
        private MenuMasterEn LoadMenuObject(IDataReader argReader)
        {
            MenuMasterEn loItem = new MenuMasterEn();
            loItem.MenuID = GetValue<int>(argReader, "MenuID");
            loItem.MenuName = GetValue<string>(argReader, "MenuName");
            loItem.PageName = GetValue<string>(argReader, "PageName");
            loItem.PageDescription = GetValue<string>(argReader, "PageDescription");
            loItem.PageUrl = GetValue<string>(argReader, "PageUrl");
            loItem.ImageUrl = GetValue<string>(argReader, "ImageUrl");
            loItem.Status = GetValue<bool>(argReader, "Status");
            loItem.PageOrder = GetValue<int>(argReader, "PageOrder");
            loItem.LastUpdatedBy = GetValue<string>(argReader, "LastUpdatedBy");
            loItem.LastUpdatedDtTm = GetValue<DateTime>(argReader, "LastUpdatedDtTm");
            loItem.IsAdd = GetValue<bool>(argReader, "IsAdd");
            loItem.IsEdit = GetValue<bool>(argReader, "IsEdit");
            loItem.IsDelete = GetValue<bool>(argReader, "IsDelete");
            loItem.IsView = GetValue<bool>(argReader, "IsView");
            loItem.IsPrint = GetValue<bool>(argReader, "IsPrint");
            loItem.IsPost = GetValue<bool>(argReader, "IsPost");
            loItem.IsOthers = GetValue<bool>(argReader, "IsOthers");

            return loItem;
        }
        /// <summary>
        /// Method to Load UserRights Entity 
        /// </summary>
        /// <param name="argReader">IDataReader Object is an Input.</param>
        /// <returns>Returns UserRights Entity</returns>
        private UserRightsEn LoadObject(IDataReader argReader)
        {
            UserRightsEn loItem = new UserRightsEn();
            loItem.UserGroup = GetValue<int>(argReader, "UserGroupId");
            loItem.MenuID = GetValue<int>(argReader, "MenuID");
            loItem.MenuName = GetValue<string>(argReader, "MenuName");
            loItem.IsAdd = GetValue<bool>(argReader, "IsAdd");
            loItem.IsEdit = GetValue<bool>(argReader, "IsEdit");
            loItem.IsDelete = GetValue<bool>(argReader, "IsDelete");
            loItem.IsView = GetValue<bool>(argReader, "IsView");
            loItem.IsPrint = GetValue<bool>(argReader, "IsPrint");
            loItem.IsPost = GetValue<bool>(argReader, "IsPost");
            loItem.IsOthers = GetValue<bool>(argReader, "IsOthers");
            loItem.IsAddModeDefault = GetValue<bool>(argReader, "DefaultMode");          

            return loItem;
        }

        private UserRightsEn LoadURMenu(IDataReader argReader)
        {
            UserRightsEn loItem = new UserRightsEn();            
            loItem.MenuID = GetValue<int>(argReader, "MenuID");
            loItem.MenuName = GetValue<string>(argReader, "MenuName");
            loItem.PageName = GetValue<string>(argReader, "PageName");
           
            return loItem;
        }

        private UserRightsEn LoadObjectUser(IDataReader argReader)
        {
            UserRightsEn loItem = new UserRightsEn();
            loItem.MenuID = GetValue<int>(argReader, "MenuID");
            loItem.MenuName = GetValue<string>(argReader, "MenuName");
            loItem.PageName = GetValue<string>(argReader, "PageName");
            loItem.IsAdd = GetValue<bool>(argReader, "IsAdd");
            loItem.IsEdit = GetValue<bool>(argReader, "IsEdit");
            loItem.IsDelete = GetValue<bool>(argReader, "IsDelete");
            loItem.IsView = GetValue<bool>(argReader, "IsView");
            loItem.IsPrint = GetValue<bool>(argReader, "IsPrint");
            loItem.IsPost = GetValue<bool>(argReader, "IsPost");
            loItem.IsOthers = GetValue<bool>(argReader, "IsOthers");
            loItem.IsAddModeDefault = GetValue<bool>(argReader, "DefaultMode");

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

        #region GetMenuByWorkflowGroup

        /// <summary>
        /// Method to Get List of Menus By UserGroups
        /// </summary>
        /// <param name="MenuName">MenuName is an Input.</param>
        /// <param name="UserGroupID">UserGroupID  is an Input.</param>
        /// <returns>Returns List of Menus</returns>
        public List<MenuMasterEn> GetMenuByWorkflowGroup(string MenuName, int UserGroupID, int UserID)
        {
            #region Declarations

            //Create Instances - Start
            MenuMasterEn MenuDetails = new MenuMasterEn();
            List<MenuMasterEn> MenuList = new List<MenuMasterEn>();
            //Create Instances - Stop

            //variable declarations
            string SqlStatement = null;

            #endregion

            try
            {
                //Build Sql Statement - Start
                SqlStatement = "SELECT UR_MenuMaster.MenuID, UR_MenuMaster.MenuName, UR_MenuMaster.PageName, UR_MenuMaster.PageDescription, " +
                         " UR_MenuMaster.PageUrl, UR_MenuMaster.ImageUrl, UR_MenuMaster.Status, UR_MenuMaster.PageOrder, UR_MenuMaster.LastUpdatedBy, " +
                         " UR_MenuMaster.LastUpdatedDtTm, UR_UserRights.IsAdd, UR_UserRights.IsEdit, UR_UserRights.IsDelete, UR_UserRights.IsView, " +
                         " UR_UserRights.IsPrint, UR_UserRights.IsPost, UR_UserRights.IsOthers FROM UR_MenuMaster INNER JOIN UR_UserRights " +
                         " ON UR_MenuMaster.MenuID = UR_UserRights.MenuID WHERE UR_UserRights.UserGroupId = " + UserGroupID +
                         " AND UR_MenuMaster.MenuName = " + clsGeneric.AddQuotes(MenuName) + " AND UR_MenuMaster.Status = 'true' ";

                if (MenuName == "Setup")
                {
                    SqlStatement += " AND UR_MenuMaster.MenuID = 13";
                }
                
                if (MenuName == "Process")
                {
                    SqlStatement += " AND UR_MenuMaster.MenuID = 114";         
                    //SqlStatement += " AND UR_MenuMaster.MenuID IN (SELECT MenuID from ur_usermenu WHERE userid = " + UserID + ") ";      
                }

                if (MenuName == "Reports")
                {
                    SqlStatement += " AND UR_MenuMaster.MenuID = 18";
                }
                
                SqlStatement += " ORDER BY UR_MenuMaster.PageOrder";
                //Build Sql Statement - Stop

                //Get User Login Details - Start
                IDataReader _IDataReader = _DatabaseFactory.ExecuteReader(Helper.GetDataBaseType, DataBaseConnectionString,
                    SqlStatement).CreateDataReader();
                //Get User Login Details - Stop

                //if user login details available - Start
                if (_IDataReader != null)
                {
                    while (_IDataReader.Read())
                    {
                        //load data to entities
                        MenuDetails = LoadMenuObject(_IDataReader);
                        MenuList.Add(MenuDetails);
                    }
                    _IDataReader.Close();
                    _IDataReader.Dispose();
                    return MenuList;
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

        #region GetUserMenu

        /// <summary>
        /// Method to Get List of All UserRights  by UserID
        /// </summary>
        /// <param name="UserGroupID">UserID  as an Input.</param>
        /// <returns>Returns List of UserRights</returns>
        public List<UserRightsEn> GetMenuByUser(int UserID)
        {
            List<UserRightsEn> loEnList = new List<UserRightsEn>();
            string sqlCmd = null;
            //Create Instances - Start
            MenuMasterEn MenuDetails = new MenuMasterEn();
            List<MenuMasterEn> MenuList = new List<MenuMasterEn>();
            //Create Instances - Stop

            if (UserID > 0)
            {
                //sqlCmd = "SELECT UR_MenuMaster.MenuID, UR_MenuMaster.MenuName, UR_MenuMaster.PageName, UR_UserRights.IsAdd, " +
                //"UR_UserRights.IsEdit, UR_UserRights.IsDelete, UR_UserRights.IsView, UR_UserRights.IsPost, UR_UserRights.IsPrint," +
                //"UR_UserRights.DefaultMode FROM UR_MenuMaster INNER JOIN " +
                //"UR_UserRights ON UR_MenuMaster.MenuID = UR_UserRights.MenuID WHERE " +
                //"UR_MenuMaster.Status = true AND UR_MenuMaster.MenuID <> 114 AND " +                 
                //"UR_UserRights.UserId =" + UserID + " order by UR_MenuMaster.MenuName desc";

                //sqlCmd = "SELECT UR_MenuMaster.MenuID, UR_MenuMaster.MenuName, UR_MenuMaster.PageName " +
                sqlCmd = "SELECT UR_MenuMaster.MenuID " +
               "FROM UR_MenuMaster WHERE UR_MenuMaster.Status = true AND UR_MenuMaster.MenuName = 'Process' " +
               "AND UR_MenuMaster.MenuID IN (SELECT MenuID from ur_usermenu WHERE UserId = " + UserID + ") " +
               "ORDER BY UR_MenuMaster.MenuName desc, UR_MenuMaster.pageorder";
            }
            else
            {               
                sqlCmd = "SELECT UR_MenuMaster.MenuID, UR_MenuMaster.MenuName, UR_MenuMaster.PageName " +
               "FROM UR_MenuMaster WHERE UR_MenuMaster.Status = true AND UR_MenuMaster.MenuName = 'Process' " +
               "ORDER BY UR_MenuMaster.MenuName desc, UR_MenuMaster.pageorder";  
            }
                        
            try
            {
                if (!FormHelp.IsBlank(sqlCmd))
                {
                    using (IDataReader loReader = _DatabaseFactory.ExecuteReader(Helper.GetDataBaseType,
                        DataBaseConnectionString, sqlCmd).CreateDataReader())
                    {
                        while (loReader.Read())
                        {        
                            if (UserID > 0)
                            {
                                UserRightsEn loItem = new UserRightsEn();
                                loItem.MenuID = GetValue<int>(loReader, "MenuID");
                                //loItem.MenuName = GetValue<string>(loReader, "MenuName");
                                //loItem.PageName = GetValue<string>(loReader, "PageName");                                
                                loEnList.Add(loItem);
                            }
                            else
                            {
                                UserRightsEn loMenuItem = LoadURMenu(loReader);
                                loMenuItem.MenuID = GetValue<int>(loReader, "MenuID");
                                loMenuItem.MenuName = GetValue<string>(loReader, "MenuName");
                                loMenuItem.PageName = GetValue<string>(loReader, "PageName");
                                //loMenuItem.IsAdd = false;
                                //loMenuItem.IsEdit = false;
                                //loMenuItem.IsDelete = false;
                                //loMenuItem.IsView = false;
                                //loMenuItem.IsPrint = false;
                                //loMenuItem.IsPost = false;                                
                                //loMenuItem.IsAddModeDefault = false;
                                loEnList.Add(loMenuItem);
                            }                         
                            
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

        #region GetPreparerWorkflowList

        public List<MenuMasterEn> GetPreparerWorkflowList(string MenuName, int UserID, int UserGroupId)
        {
            MenuMasterEn _MenuMasterEn = new MenuMasterEn();
            List<MenuMasterEn> list = new List<MenuMasterEn>();

            try
            {
                string SqlStatement = @"SELECT mm.MenuID, mm.MenuName, mm.PageName, mm.PageDescription, mm.PageUrl, mm.ImageUrl, mm.Status, mm.PageOrder, mm.LastUpdatedBy,
                                mm.LastUpdatedDtTm, ur.IsAdd, ur.IsEdit, ur.IsDelete, ur.IsView, ur.IsPrint, ur.IsPost, ur.IsOthers 
                                FROM ur_usermenu_preparer prep
                                INNER JOIN ur_menumaster mm ON mm.menuid = prep.menuid
                                INNER JOIN ur_userrights ur ON ur.usergroupid = (SELECT usergroupid FROM ur_users WHERE userid = prep.userid) 
                                                            AND prep.menuid = ur.menuid
                                WHERE prep.userid = " + UserID + @" AND ur.usergroupid   = " + UserGroupId + @"
                                AND mm.menuname = " + clsGeneric.AddQuotes(MenuName) + @" AND mm.status = 'true'
                                AND mm.menuid <> 114 
                                ORDER BY mm.pageorder ";

                IDataReader _IDataReader = _DatabaseFactory.ExecuteReader(Helper.GetDataBaseType, DataBaseConnectionString, SqlStatement).CreateDataReader();

                if (_IDataReader != null)
                {
                    while (_IDataReader.Read())
                    {
                        _MenuMasterEn = LoadMenuObject(_IDataReader);
                        list.Add(_MenuMasterEn);
                    }
                    _IDataReader.Close();
                    _IDataReader.Dispose();

                    return list;
                }

                return null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region GetMenuMasterAdmin

        public List<MenuMasterEn> GetMenuMasterAdmin(string MenuName, int UserGroupID)
        {
            #region Declarations

            //Create Instances - Start
            MenuMasterEn MenuDetails = new MenuMasterEn();
            List<MenuMasterEn> MenuList = new List<MenuMasterEn>();
            //Create Instances - Stop

            //variable declarations
            string SqlStatement = null;

            #endregion

            try
            {
                SqlStatement = "SELECT UR_MenuMaster.MenuID, UR_MenuMaster.MenuName, UR_MenuMaster.PageName, UR_MenuMaster.PageDescription, " +
                         " UR_MenuMaster.PageUrl, UR_MenuMaster.ImageUrl, UR_MenuMaster.Status, UR_MenuMaster.PageOrder, UR_MenuMaster.LastUpdatedBy, " +
                         " UR_MenuMaster.LastUpdatedDtTm, UR_UserRights.IsAdd, UR_UserRights.IsEdit, UR_UserRights.IsDelete, UR_UserRights.IsView, " +
                         " UR_UserRights.IsPrint, UR_UserRights.IsPost, UR_UserRights.IsOthers FROM UR_MenuMaster INNER JOIN UR_UserRights " +
                         " ON UR_MenuMaster.MenuID = UR_UserRights.MenuID WHERE UR_UserRights.UserGroupId = " + UserGroupID +
                         " AND UR_MenuMaster.MenuName = " + clsGeneric.AddQuotes(MenuName) + " AND UR_MenuMaster.Status = 'true' " +
                         " ORDER BY UR_MenuMaster.PageOrder";

                IDataReader _IDataReader = _DatabaseFactory.ExecuteReader(Helper.GetDataBaseType, DataBaseConnectionString,
                    SqlStatement).CreateDataReader();

                if (_IDataReader != null)
                {
                    while (_IDataReader.Read())
                    {
                        MenuDetails = LoadMenuObject(_IDataReader);
                        MenuList.Add(MenuDetails);
                    }
                    _IDataReader.Close();
                    _IDataReader.Dispose();
                    return MenuList;
                }

                return null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion
    }

}


