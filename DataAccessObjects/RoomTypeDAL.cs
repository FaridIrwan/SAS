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
    /// Class to handle all the RoomType Methods.
    /// </summary>
    public class RoomTypeDAL
    {
        #region Global Declarations 

        private DbParameterCollection _DbParameterCollection = null;

        private MaxModule.DatabaseProvider _DatabaseFactory =
            new MaxModule.DatabaseProvider();

        private string DataBaseConnectionString = Helper.
           GetConnectionString();

        #endregion

        public RoomTypeDAL()
        {
        }

        #region GetList 

        /// <summary>
        /// Method to Get List of RoomType
        /// </summary>
        /// <param name="argEn">RoomType Entity as an Input.</param>
        /// <returns>Returns List of RoomType</returns>
        public List<RoomTypeEn> GetList(RoomTypeEn argEn)
        {
            List<RoomTypeEn> loEnList = new List<RoomTypeEn>();
            string sqlCmd = "select * from SAS_RoomType";
            
            try
            {
                if (!FormHelp.IsBlank(sqlCmd))
                {
                    using (IDataReader loReader = _DatabaseFactory.ExecuteReader(Helper.GetDataBaseType,
                        DataBaseConnectionString, sqlCmd).CreateDataReader())
                    {
                        while (loReader.Read())
                        {
                            RoomTypeEn loItem = LoadObject(loReader);
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

        #region GetRoomTypeListall 

        /// <summary>
        /// Method to Get List of all RoomType
        /// </summary>
        /// <param name="argEn">RoomType Entity is an Input.</param>
        /// <returns>Returns List of RoomType</returns>
        public List<RoomTypeEn> GetRoomTypeListall(RoomTypeEn argEn)
        {
            List<RoomTypeEn> loEnList = new List<RoomTypeEn>();


            argEn.SART_Code = argEn.SART_Code.Replace("*", "%");
            argEn.SART_Description = argEn.SART_Description.Replace("*", "%");
            argEn.SABK_Code = argEn.SABK_Code.Replace("*", "%");
            argEn.SAKO_Code = argEn.SAKO_Code.Replace("*", "%");

            string sqlCmd = "SELECT SAS_RoomType.*, SAS_Kolej.SAKO_Code AS Expr1 FROM SAS_Block INNER JOIN "+
                            " SAS_Kolej ON SAS_Block.SAKO_Code = SAS_Kolej.SAKO_Code INNER JOIN SAS_RoomType ON SAS_Block.SABK_Code = SAS_RoomType.SABK_Code  WHERE SART_Code  <> '0'";
            if (argEn.SART_Code.Length != 0) sqlCmd = sqlCmd + " and SAS_RoomType.SART_Code like '" + argEn.SART_Code + "'";
            if (argEn.SART_Description.Length != 0) sqlCmd = sqlCmd + " and SAS_RoomType.SART_Description like '" + argEn.SART_Description + "'";
            if (argEn.SABK_Code.Length != 0) sqlCmd = sqlCmd + " and SAS_Block.SABK_Code like '" + argEn.SABK_Code + "'";
            if (argEn.SABK_Code.Length != 0) sqlCmd = sqlCmd + " and SAS_Kolej.SAKO_Code like '" + argEn.SAKO_Code + "'";
            sqlCmd = sqlCmd + " order by SAS_RoomType.SART_Code";
            
            try
            {
                if (!FormHelp.IsBlank(sqlCmd))
                {
                    using (IDataReader loReader = _DatabaseFactory.ExecuteReader(Helper.GetDataBaseType,
                        DataBaseConnectionString, sqlCmd).CreateDataReader())
                    {
                        while (loReader.Read())
                        {
                            RoomTypeEn loItem = LoadObject(loReader);
                            loItem.SAKO_Code = GetValue<string>(loReader, "Expr1");
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

        #region GetRoomTypeList 

        /// <summary>
        /// Method to Get List of RoomType by SART_Code
        /// </summary>
        /// <param name="argEn">SABK_Code as Input.</param>
        /// <returns>Returns List of RoomType</returns>
        public List<RoomTypeEn> GetRoomTypeList(string argEn)
        {
            List<RoomTypeEn> loEnList = new List<RoomTypeEn>();
            string sqlCmd = "select * from SAS_RoomType where SABK_Code='"+ argEn +"' ";
            
            try
            {
                if (!FormHelp.IsBlank(sqlCmd))
                {
                    using (IDataReader loReader = _DatabaseFactory.ExecuteReader(Helper.GetDataBaseType,
                        DataBaseConnectionString, sqlCmd).CreateDataReader())
                    {
                        while (loReader.Read())
                        {
                            RoomTypeEn loItem = LoadObject(loReader);
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
        /// Method to Get RoomType Entity
        /// </summary>
        /// <param name="argEn">RoomType Entity is an Input.SART_Code as Input Property.</param>
        /// <returns>Returns RoomType Entity</returns>
        public RoomTypeEn GetItem(RoomTypeEn argEn)
        {
            RoomTypeEn loItem = new RoomTypeEn();
            string sqlCmd = "Select * FROM SAS_RoomType WHERE SART_Code = @SART_Code";
            
            try
            {
                if (!FormHelp.IsBlank(sqlCmd))
                {
                    DbCommand cmd = _DatabaseFactory.GetDbCommand(Helper.GetDataBaseType, sqlCmd, DataBaseConnectionString);
                    _DatabaseFactory.AddInParameter(ref cmd, "@SART_Code", DbType.String, argEn.SART_Code);
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
        /// Method to Insert RoomType 
        /// </summary>
        /// <param name="argEn">RoomType Entity is an Input.</param>
        /// <returns>Returns Boolean</returns>
        public bool Insert(RoomTypeEn argEn)
        {
            bool lbRes = false;
            int iOut = 0;
            string sqlCmd = "Select count(*) as cnt From SAS_RoomType WHERE SABK_Code=@SABK_Code AND SART_Code = @SART_Code";
            try
            {
                if (!FormHelp.IsBlank(sqlCmd))
                {
                    DbCommand cmdSel = _DatabaseFactory.GetDbCommand(Helper.GetDataBaseType, sqlCmd, DataBaseConnectionString);
                    _DatabaseFactory.AddInParameter(ref cmdSel, "@SART_Code", DbType.String, argEn.SART_Code);
                    _DatabaseFactory.AddInParameter(ref cmdSel, "@SART_Description", DbType.String, argEn.SART_Description);
                    _DatabaseFactory.AddInParameter(ref cmdSel, "@SABK_Code", DbType.String, argEn.SABK_Code);
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
                        sqlCmd = "INSERT INTO SAS_RoomType(SART_Code,SABK_Code,SART_Description) VALUES (@SART_Code,@SABK_Code,@SART_Description) ";

                        if (!FormHelp.IsBlank(sqlCmd))
                        {
                            DbCommand cmd = _DatabaseFactory.GetDbCommand(Helper.GetDataBaseType, sqlCmd, DataBaseConnectionString);
                            _DatabaseFactory.AddInParameter(ref cmd, "@SART_Code", DbType.String, argEn.SART_Code);
                            _DatabaseFactory.AddInParameter(ref cmd, "@SABK_Code", DbType.String, argEn.SABK_Code);
                            _DatabaseFactory.AddInParameter(ref cmd, "@SART_Description", DbType.String, argEn.SART_Description);
                            _DbParameterCollection = cmd.Parameters;

                            int liRowAffected = _DatabaseFactory.ExecuteNonQuery(Helper.GetDataBaseType, cmd,
                                DataBaseConnectionString, sqlCmd, _DbParameterCollection);

                            if (liRowAffected > -1)
                                lbRes = true;
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
        /// Method to Update RoomType 
        /// </summary>
        /// <param name="argEn">RoomType Entity is an Input.</param>
        /// <returns>Returns Boolean</returns>
        public bool Update(RoomTypeEn argEn)
        {
            bool lbRes = false;
            int iOut = 0;
            string sqlCmd = "Select count(*) as cnt From SAS_RoomType WHERE SABK_Code=@SABK_Code and SART_Code = @SART_Code";
            try
            {
                if (!FormHelp.IsBlank(sqlCmd))
                {
                    DbCommand cmdSel = _DatabaseFactory.GetDbCommand(Helper.GetDataBaseType, sqlCmd, DataBaseConnectionString);
                    _DatabaseFactory.AddInParameter(ref cmdSel, "@SART_Code", DbType.String, argEn.SART_Code);
                    _DatabaseFactory.AddInParameter(ref cmdSel, "@SART_Description", DbType.String, argEn.SART_Description);
                    _DatabaseFactory.AddInParameter(ref cmdSel, "@SABK_Code", DbType.String, argEn.SABK_Code);
                    _DbParameterCollection = cmdSel.Parameters;

                    using (IDataReader dr = _DatabaseFactory.GetIDataReader(Helper.GetDataBaseType, cmdSel,
                        DataBaseConnectionString, sqlCmd, _DbParameterCollection).CreateDataReader()) 
                    {
                        if (dr.Read())
                            iOut = clsGeneric.NullToInteger(dr["cnt"]);
                        if (iOut > 1)
                            throw new Exception("Record Already Exist");
                    }
                    if (iOut <= 1)
                    {
                        sqlCmd = "UPDATE SAS_RoomType SET SART_Code = @SART_Code, SABK_Code = @SABK_Code, SART_Description = @SART_Description WHERE SART_Code = @SART_Code";
                        if (!FormHelp.IsBlank(sqlCmd))
                        {
                            DbCommand cmd = _DatabaseFactory.GetDbCommand(Helper.GetDataBaseType, sqlCmd, DataBaseConnectionString);
                            _DatabaseFactory.AddInParameter(ref cmd, "@SART_Code", DbType.String, argEn.SART_Code);
                            _DatabaseFactory.AddInParameter(ref cmd, "@SABK_Code", DbType.String, argEn.SABK_Code);
                            _DatabaseFactory.AddInParameter(ref cmd, "@SART_Description", DbType.String, argEn.SART_Description);
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
        /// Method to Delete RoomType 
        /// </summary>
        /// <param name="argEn">RoomType Entity is an Input.</param>
        /// <returns>Returns Boolean</returns>
        public bool Delete(RoomTypeEn argEn)
        {

            bool lbRes = false;
            int iOut = 0;

            string sqlCmd = "select sum(rows) as total from(SELECT COUNT(*) AS rows FROM  SAS_Student where SART_Code = @SART_Code union all select count(*) as rows from SAS_HostelStruct WHERE SAHB_RoomTYpe = @SART_Code)AS U";
            try
            {
                if (!FormHelp.IsBlank(sqlCmd))
                {
                    DbCommand cmdSel = _DatabaseFactory.GetDbCommand(Helper.GetDataBaseType, sqlCmd, DataBaseConnectionString);
                    _DatabaseFactory.AddInParameter(ref cmdSel, "@SART_Code", DbType.String, argEn.SART_Code);
                    _DbParameterCollection = cmdSel.Parameters;

                    using (IDataReader dr = _DatabaseFactory.GetIDataReader(Helper.GetDataBaseType, cmdSel,
                        DataBaseConnectionString, sqlCmd, _DbParameterCollection).CreateDataReader()) 
                    {
                        if (dr.Read())
                            iOut = clsGeneric.NullToInteger(dr["total"]);
                        if (iOut > 0)
                            throw new Exception("Record Already In Use");
                    }
                    if (iOut == 0)
                    {
                        sqlCmd = "DELETE FROM SAS_RoomType WHERE SART_Code = @SART_Code";
                        if (!FormHelp.IsBlank(sqlCmd))
                        {
                            DbCommand cmd = _DatabaseFactory.GetDbCommand(Helper.GetDataBaseType, sqlCmd, DataBaseConnectionString);
                            _DatabaseFactory.AddInParameter(ref cmd, "@SART_Code", DbType.String, argEn.SART_Code);
                            _DbParameterCollection = cmd.Parameters;

                            int liRowAffected = _DatabaseFactory.ExecuteNonQuery(Helper.GetDataBaseType, cmd,
                                DataBaseConnectionString, sqlCmd, _DbParameterCollection);
                            
                            if (liRowAffected > -1)
                                lbRes = true;
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

        #region LoadObject 

        /// <summary>
        /// Method to Load RoomType Entity 
        /// </summary>
        /// <param name="argReader">IDataReader Object is an Input.</param>
        /// <returns>Returns RoomType Entity</returns>
        private RoomTypeEn LoadObject(IDataReader argReader)
        {
            RoomTypeEn loItem = new RoomTypeEn();
            loItem.SART_Code = GetValue<string>(argReader, "SART_Code");
            loItem.SABK_Code = GetValue<string>(argReader, "SABK_Code");
            loItem.SART_Description = GetValue<string>(argReader, "SART_Description");

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
    }

}
