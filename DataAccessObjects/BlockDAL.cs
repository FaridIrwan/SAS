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
    /// Class to handle all the Block Methods.
    /// </summary>
    public class BlockDAL
    {
        #region Global Declarations 

        private DbParameterCollection _DbParameterCollection = null;

        private MaxModule.DatabaseProvider _DatabaseFactory =
            new MaxModule.DatabaseProvider();

        private string DataBaseConnectionString = Helper.
           GetConnectionString();

        #endregion

        public BlockDAL()
        {
        }

        #region GetList 

        /// <summary>
        /// Method to Get List of Blocks
        /// </summary>
        /// <param name="argEn">Blocks Entity is an Input.</param>
        /// <returns>Returns List of Blocks</returns>
        public List<BlockEn> GetList(BlockEn argEn)
        {
            List<BlockEn> loEnList = new List<BlockEn>();
            string sqlCmd = "select * from SAS_Block";
            
            try
            {
                if (!FormHelp.IsBlank(sqlCmd))
                {
                    using (IDataReader loReader = _DatabaseFactory.ExecuteReader(Helper.GetDataBaseType,
                        DataBaseConnectionString, sqlCmd).CreateDataReader())
                    {
                        while (loReader.Read())
                        {
                            BlockEn loItem = LoadObject(loReader);
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

        #region GetBlockListall 

        /// <summary>
        /// Method to Get List of all Block
        /// </summary>
        /// <param name="argEn">Block Entity is an Input.</param>
        /// <returns>Returns List of Block</returns>
        public List<BlockEn> GetBlockListall(BlockEn argEn)
        {
            List<BlockEn> loEnList = new List<BlockEn>();
            argEn.SABK_Code = argEn.SABK_Code.Replace("*", "%");
            argEn.SABK_Description = argEn.SABK_Description.Replace("*", "%");
            argEn.SAKO_Code = argEn.SAKO_Code.Replace("*", "%");
            string sqlCmd = "select * from SAS_Block  WHERE SABK_Code  <> '0'";
            if (argEn.SABK_Code.Length != 0) sqlCmd = sqlCmd + " and SABK_Code like '" + argEn.SABK_Code + "'";
            if (argEn.SABK_Description.Length != 0) sqlCmd = sqlCmd + " and SABK_Description like '" + argEn.SABK_Description + "'";
            if (argEn.SAKO_Code.Length != 0) sqlCmd = sqlCmd + " and SAKO_Code like '" + argEn.SAKO_Code + "'";
            sqlCmd = sqlCmd + " order by SABK_Code";
            
            try
            {
                if (!FormHelp.IsBlank(sqlCmd))
                {
                    using (IDataReader loReader = _DatabaseFactory.ExecuteReader(Helper.GetDataBaseType,
                        DataBaseConnectionString, sqlCmd).CreateDataReader())
                    {
                        while (loReader.Read())
                        {
                            BlockEn loItem = LoadObject(loReader);
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

        #region GetBlockList 

        /// <summary>
        /// Method to Get List of Blocks by KolejCode
        /// </summary>
        /// <param name="argEn">KolejCode is an Input.</param>
        /// <returns>Returns List of Blocks</returns>
        public List<BlockEn> GetBlockList(string argEn)
        {
            List<BlockEn> loEnList = new List<BlockEn>();
            string sqlCmd = "select * from SAS_Block where SAKO_Code='"+ argEn +"'";
            
            try
            {
                if (!FormHelp.IsBlank(sqlCmd))
                {
                    using (IDataReader loReader = _DatabaseFactory.ExecuteReader(Helper.GetDataBaseType,
                        DataBaseConnectionString, sqlCmd).CreateDataReader())
                    {
                        while (loReader.Read())
                        {
                            BlockEn loItem = LoadObject(loReader);
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
        /// Method to Get Block Entity
        /// </summary>
        /// <param name="argEn">Block Entity is an Input</param>
        /// <returns>Returns Block Entity</returns>
        public BlockEn GetItem(BlockEn argEn)
        {
            BlockEn loItem = new BlockEn();
            string sqlCmd = "Select * FROM SAS_Block WHERE SABK_Code = @SABK_Code";
            
            try
            {
                if (!FormHelp.IsBlank(sqlCmd))
                {
                    DbCommand cmd = _DatabaseFactory.GetDbCommand(Helper.GetDataBaseType, sqlCmd, DataBaseConnectionString);
                    _DatabaseFactory.AddInParameter(ref cmd, "@SABK_Code", DbType.String, argEn.SABK_Code);
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
        /// Method to Insert Blocks 
        /// </summary>
        /// <param name="argEn">Block Entity is an Input.</param>
        /// <returns>Returns Boolean</returns>
        public bool Insert(BlockEn argEn)
        {
            bool lbRes = false;
            int iOut = 0;
            string sqlCmd = "Select count(*) as cnt From SAS_Block WHERE SAKO_Code=@SAKO_Code and SABK_Code = @SABK_Code";
            try
            {
                if (!FormHelp.IsBlank(sqlCmd))
                {
                    DbCommand cmdSel = _DatabaseFactory.GetDbCommand(Helper.GetDataBaseType, sqlCmd, DataBaseConnectionString);
                    _DatabaseFactory.AddInParameter(ref cmdSel, "@SABK_Code", DbType.String, argEn.SABK_Code);
                    _DatabaseFactory.AddInParameter(ref cmdSel, "@SAKO_Code", DbType.String, argEn.SAKO_Code);
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
                        sqlCmd = "INSERT INTO SAS_Block(SABK_Code,SAKO_Code,SABK_Description) VALUES (@SABK_Code,@SAKO_Code,@SABK_Description) ";

                        if (!FormHelp.IsBlank(sqlCmd))
                        {
                            DbCommand cmd = _DatabaseFactory.GetDbCommand(Helper.GetDataBaseType, sqlCmd, DataBaseConnectionString);
                            _DatabaseFactory.AddInParameter(ref cmd, "@SABK_Code", DbType.String, argEn.SABK_Code);
                            _DatabaseFactory.AddInParameter(ref cmd, "@SAKO_Code", DbType.String, argEn.SAKO_Code);
                            _DatabaseFactory.AddInParameter(ref cmd, "@SABK_Description", DbType.String, argEn.SABK_Description);
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
        /// Method to Update Blocks 
        /// </summary>
        /// <param name="argEn">Block Entity is an Input.</param>
        /// <returns>Returns Boolean</returns>
        public bool Update(BlockEn argEn)
        {
            bool lbRes = false;
            int iOut = 0;
            string sqlCmd = "Select count(*) as cnt From SAS_Block WHERE SAKO_Code =@SAKO_Code and SABK_Code = @SABK_Code ";
            try
            {
                if (!FormHelp.IsBlank(sqlCmd))
                {
                    DbCommand cmdSel = _DatabaseFactory.GetDbCommand(Helper.GetDataBaseType, sqlCmd, DataBaseConnectionString);
                    _DatabaseFactory.AddInParameter(ref cmdSel, "@SABK_Code", DbType.String, argEn.SABK_Code);
                    _DatabaseFactory.AddInParameter(ref cmdSel, "@SABK_Description", DbType.String, argEn.SABK_Description);
                    _DatabaseFactory.AddInParameter(ref cmdSel, "@SAKO_Code", DbType.String, argEn.SAKO_Code);
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
                        sqlCmd = "UPDATE SAS_Block SET SABK_Code = @SABK_Code, SAKO_Code = @SAKO_Code, SABK_Description = @SABK_Description WHERE SABK_Code = @SABK_Code";

                        if (!FormHelp.IsBlank(sqlCmd))
                        {
                            DbCommand cmd = _DatabaseFactory.GetDbCommand(Helper.GetDataBaseType, sqlCmd, DataBaseConnectionString);
                            _DatabaseFactory.AddInParameter(ref cmd, "@SABK_Code", DbType.String, argEn.SABK_Code);
                            _DatabaseFactory.AddInParameter(ref cmd, "@SAKO_Code", DbType.String, argEn.SAKO_Code);
                            _DatabaseFactory.AddInParameter(ref cmd, "@SABK_Description", DbType.String, argEn.SABK_Description);
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
        /// Method to Delete Blocks 
        /// </summary>
        /// <param name="argEn">Block Entity is an Input.</param>
        /// <returns>Returns Boolean</returns>
        public bool Delete(BlockEn argEn)
        {

            bool lbRes = false;
            int iOut = 0;

            string sqlCmd = "select sum(rows) as total from(SELECT COUNT(*) AS rows FROM  SAS_Student where SABK_Code = @SABK_Code union all select count(*) as rows from SAS_HostelStruct WHERE SAHB_Block = @SABK_Code union all select count(*) as rows from SAS_RoomType WHERE SABK_Code = @SABK_Code)AS U";
            try
            {
                if (!FormHelp.IsBlank(sqlCmd))
                {
                    DbCommand cmdSel = _DatabaseFactory.GetDbCommand(Helper.GetDataBaseType, sqlCmd, DataBaseConnectionString);
                    _DatabaseFactory.AddInParameter(ref cmdSel, "@SABK_Code", DbType.String, argEn.SABK_Code);
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
                        sqlCmd = "DELETE FROM SAS_Block WHERE SABK_Code = @SABK_Code";

                        if (!FormHelp.IsBlank(sqlCmd))
                        {
                            DbCommand cmd = _DatabaseFactory.GetDbCommand(Helper.GetDataBaseType, sqlCmd, DataBaseConnectionString);
                            _DatabaseFactory.AddInParameter(ref cmd, "@SABK_Code", DbType.String, argEn.SABK_Code);
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
        /// Method to Load Block Entity 
        /// </summary>
        /// <param name="argReader">IDataReader Object is an Input.</param>
        /// <returns>Returns Block Entity</returns>
        private BlockEn LoadObject(IDataReader argReader)
        {
            BlockEn loItem = new BlockEn();
            loItem.SABK_Code = GetValue<string>(argReader, "SABK_Code");
            loItem.SAKO_Code = GetValue<string>(argReader, "SAKO_Code");
            loItem.SABK_Description = GetValue<string>(argReader, "SABK_Description");

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
