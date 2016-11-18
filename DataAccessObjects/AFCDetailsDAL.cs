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
    /// Class to handle all the AFCDetails Transactions.
    /// </summary>
    public class AFCDetailsDS
    {
        #region Global Declarations 

        private DbParameterCollection _DbParameterCollection = null;

        private MaxModule.DatabaseProvider _DatabaseFactory =
            new MaxModule.DatabaseProvider();

        private string DataBaseConnectionString = Helper.
           GetConnectionString();

        #endregion

        public AFCDetailsDS()
        {
        }

        #region GetList 

        /// <summary>
        /// Method to Get List of AFCDetails
        /// </summary>
        /// <param name="argEn">AFCDetails Entity is an Input.</param>
        /// <returns>Returns List of AFCDetails Entities</returns>
        public List<AFCDetailsEn> GetList(AFCDetailsEn argEn)
        {
            List<AFCDetailsEn> loEnList = new List<AFCDetailsEn>();
            string sqlCmd = @"select  SAS_AFCDetails.ProgramCode, SAS_Program.SAFC_Code, SAS_AFCDetails.StudentsNo,SAS_Program.SAPG_Program,SAS_AFC.Semester from SAS_AFCDetails
                            INNER JOIN SAS_Program ON SAS_AFCDetails.ProgramCode = SAS_Program.SAPG_Code left join SAS_AFC on SAS_AFC.TransCode=SAS_AFCDetails.TransCode
                            WHERE SAS_AFCDetails.transcode = '" + argEn.TransCode + "'";
            
            try
            {
                if (!FormHelp.IsBlank(sqlCmd))
                {
                    using (IDataReader loReader = _DatabaseFactory.ExecuteReader(Helper.GetDataBaseType,
                        DataBaseConnectionString, sqlCmd).CreateDataReader())
                    {
                        while (loReader.Read())
                        {
                            AFCDetailsEn loItem = new AFCDetailsEn();
                            loItem.ProgramID = GetValue<string>(loReader, "ProgramCode");
                            loItem.Program = GetValue<string>(loReader, "SAPG_Program");
                            loItem.Faculty = GetValue<string>(loReader, "SAFC_Code");
                            loItem.Semester = GetValue<string>(loReader, "Semester");
                            loItem.TransStatus = "";
                            loItem.NoOfStudents = GetValue<string>(loReader, "StudentsNo");

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
        /// Method to Get an AFCDetails Entity
        /// </summary>
        /// <param name="argEn">AFCDetails Entity is an Input</param>
        /// <returns>Returns AFCDetails Entity</returns>
        public AFCDetailsEn GetItem(AFCDetailsEn argEn)
        {
            AFCDetailsEn loItem = new AFCDetailsEn();
            string sqlCmd = "Select * FROM SAS_AFCDetails WHERE TransCode = @TransCode";
            
            try
            {
                if (!FormHelp.IsBlank(sqlCmd))
                {
                    DbCommand cmd = _DatabaseFactory.GetDbCommand(Helper.GetDataBaseType, sqlCmd, DataBaseConnectionString);
                    _DatabaseFactory.AddInParameter(ref cmd, "@TransCode", DbType.Int32, argEn.TransCode);
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
        /// Method to Insert 
        /// </summary>
        /// <param name="argEn">AFCetails Entity is an Input.</param>
        /// <returns>Returns Boolean</returns>
        public bool Insert(AFCDetailsEn argEn)
        {
            bool lbRes = false;
            //int iOut = 0;
            try
            {

                string sqlCmd = "INSERT INTO SAS_AFCDetails(TransCode,TransAmount,StudentsNo,ProgramCode) VALUES (@TransCode,@TransAmount,@StudentsNo,@ProgramCode) ";

                if (!FormHelp.IsBlank(sqlCmd))
                {
                    DbCommand cmd = _DatabaseFactory.GetDbCommand(Helper.GetDataBaseType, sqlCmd, DataBaseConnectionString);
                    _DatabaseFactory.AddInParameter(ref cmd, "@TransCode", DbType.Int32, argEn.TransCode);
                    _DatabaseFactory.AddInParameter(ref cmd, "@StudentsNo", DbType.String, argEn.NoOfStudents);
                    _DatabaseFactory.AddInParameter(ref cmd, "@ProgramCode", DbType.String, argEn.ProgramID);
                    _DatabaseFactory.AddInParameter(ref cmd, "@TransAmount", DbType.Double, argEn.TransactionAmount);
                    _DbParameterCollection = cmd.Parameters;

                    int liRowAffected = _DatabaseFactory.ExecuteNonQuery(Helper.GetDataBaseType, cmd,
                                DataBaseConnectionString, sqlCmd, _DbParameterCollection);

                    if (liRowAffected > -1)
                    {
                        lbRes = true;
                        //Update BatchAmount by Program
                        UpdateBatchAmount(argEn.TransCode, argEn.ProgramID);
                    }
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
        /// Method to Update 
        /// </summary>
        /// <param name="argEn">AFCDetails Entity is an Input.</param>
        /// <returns>Returns Boolean</returns>
        public bool Update(AFCDetailsEn argEn)
        {
            bool lbRes = false;
            int iOut = 0;
            string sqlCmd = "Select count(*) as cnt From SAS_AFCDetails WHERE ";
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
                            throw new Exception("Update Failed! No Record Exist!");
                    }
                    if (iOut == 0)
                    {
                        sqlCmd = "UPDATE SAS_AFCDetails SET TransCode = @TransCode, SAFC_Code = @SAFC_Code, StudentsNo = @StudentsNo, ProgramCode = @ProgramCode, Updatedby = @Updatedby, UpdatedTime = @UpdatedTime WHERE TransCode = @TransCode";


                        if (!FormHelp.IsBlank(sqlCmd))
                        {
                            DbCommand cmd = _DatabaseFactory.GetDbCommand(Helper.GetDataBaseType, sqlCmd, DataBaseConnectionString);
                            _DatabaseFactory.AddInParameter(ref cmd, "@TransCode", DbType.Int32, argEn.TransCode);
                            _DatabaseFactory.AddInParameter(ref cmd, "@StudentsNo", DbType.String, argEn.NoOfStudents);
                            _DatabaseFactory.AddInParameter(ref cmd, "@ProgramCode", DbType.String, argEn.ProgramID);
                            _DatabaseFactory.AddInParameter(ref cmd, "@TransAmount", DbType.Double, argEn.TransactionAmount);
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
        /// Method to Delete 
        /// </summary>
        /// <param name="argEn">AFCDetails Entity is an Input.</param>
        /// <returns>Returns Boolean</returns>
        public bool Delete(AFCDetailsEn argEn)
        {
            bool lbRes = false;
            string sqlCmd = "DELETE FROM SAS_AFCDetails WHERE TransCode = @TransCode";
            
            try
            {
                if (!FormHelp.IsBlank(sqlCmd))
                {
                    DbCommand cmd = _DatabaseFactory.GetDbCommand(Helper.GetDataBaseType, sqlCmd, DataBaseConnectionString);
                    _DatabaseFactory.AddInParameter(ref cmd, "@TransCode", DbType.Int32, argEn.TransCode);
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

        #region Load Object 

        /// <summary>
        /// Method to Load AFCDetails Entity 
        /// </summary>
        /// <param name="argReader">IDataReader Object is an Input.</param>
        /// <returns>Returns AFCDetails Entity</returns>
        private AFCDetailsEn LoadObject(IDataReader argReader)
        {
            AFCDetailsEn loItem = new AFCDetailsEn();
            loItem.TransCode = GetValue<int>(argReader, "TransCode");
            loItem.NoOfStudents = GetValue<string>(argReader, "StudentsNo");
            loItem.ProgramID = GetValue<string>(argReader, "ProgramCode");
            loItem.TransactionAmount = GetValue<double>(argReader, "TransAmount");

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

        #region UpdateBatchAmount
                
        /// <returns>Returns Boolean</returns>
        public bool UpdateBatchAmount(int TransCode, string ProgId)
        {
            bool lbRes = false;
            Decimal iOut = 0;
            string sqlCmd = "select sum(sas_accounts.transamount) as totalamount from sas_accounts, sas_student " +
                            "where category='AFC' and batchcode in (select batchcode from sas_afc where transcode = " + TransCode + ") and sas_student.sasi_pgid = '" + ProgId + 
                            "' and sas_accounts.creditref = sas_student.sasi_matricno ";
            try
            {
                if (!FormHelp.IsBlank(sqlCmd)) 
                {
                    using (IDataReader dr = _DatabaseFactory.ExecuteReader(Helper.GetDataBaseType,
                        DataBaseConnectionString, sqlCmd).CreateDataReader())
                    {
                        if (dr.Read())
                            iOut = clsGeneric.NullToDecimal(dr["totalamount"]);
                        if (iOut == 0)
                            throw new Exception("Update Failed! No Record Exist!");
                    }
                    if (iOut > 0)
                    {
                        string UpdateCmd = "UPDATE SAS_AFCDetails SET TransAmount = @TransAmount WHERE TransCode = @TransCode";


                        if (!FormHelp.IsBlank(UpdateCmd))
                        {
                            DbCommand cmd = _DatabaseFactory.GetDbCommand(Helper.GetDataBaseType, sqlCmd, DataBaseConnectionString);
                            _DatabaseFactory.AddInParameter(ref cmd, "@TransAmount", DbType.Decimal, iOut);
                            _DatabaseFactory.AddInParameter(ref cmd, "@TransCode", DbType.Int32, TransCode);  
                            _DbParameterCollection = cmd.Parameters;

                            int liRowAffected = _DatabaseFactory.ExecuteNonQuery(Helper.GetDataBaseType, cmd,
                                DataBaseConnectionString, UpdateCmd, _DbParameterCollection);

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

    }

}
