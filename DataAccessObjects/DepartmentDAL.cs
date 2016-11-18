using System;
using System.Data;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data.Common;
using HTS.SAS.Entities;
using MaxGeneric;

namespace HTS.SAS.DataAccessObjects
{
    /// <summary>
    /// Class to handle all the Department Methods.
    /// </summary>
    public class DepartmentDAL
    {
         #region Global Declarations 

        private DbParameterCollection _DbParameterCollection = null;

        private MaxModule.DatabaseProvider _DatabaseFactory =
            new MaxModule.DatabaseProvider();

        private string DataBaseConnectionString = Helper.
           GetConnectionString();

        #endregion

        public DepartmentDAL()
        {
        }

        #region LoadObject

        private DepartmentEn LoadObject(IDataReader argReader)
        {
            DepartmentEn _DepartmentEn = new DepartmentEn();
            _DepartmentEn.AutoID = GetValue<int>(argReader,"AutoId");
            _DepartmentEn.DepartmentID = GetValue<string>(argReader, "DepartmentID");
            _DepartmentEn.Department = GetValue<string>(argReader, "Department");
            _DepartmentEn.Status = GetValue<bool>(argReader, "Status");
            _DepartmentEn.CreatedBy = GetValue<string>(argReader, "CreatedBy");
            _DepartmentEn.CreateDate = GetValue<DateTime>(argReader, "CreateDate");
            _DepartmentEn.ModifiedBy = GetValue<string>(argReader, "ModifiedBy");
            _DepartmentEn.ModifiedDate = GetValue<DateTime>(argReader, "ModifiedDate");

            return _DepartmentEn;
         }

        private static T GetValue<T>(IDataReader argReader, string argColNm)
        {
            if (!argReader.IsDBNull(argReader.GetOrdinal(argColNm)))
                return (T)argReader.GetValue(argReader.GetOrdinal(argColNm));
            else
                return default(T);
        }
        

        #endregion

        #region GetDepartmentList

        /// <summary>
        /// Method to Get List of Departments
        /// </summary>
        /// <param name="argEn">Department Entity  as an Inputs.</param>
        /// <returns>Returns List of Department</returns>
        public List<DepartmentEn> GetDepartmentList(DepartmentEn argEn)
        {
                //declaration
                List<DepartmentEn> depEnList = new List<DepartmentEn>();
                 string SqlStatement = null;
            try
            {
                // Build Sql Statement
                SqlStatement = "SELECT * FROM SAS_Department";
                if (!FormHelp.IsBlank(SqlStatement))
                {
                    using (IDataReader _IDataReader = _DatabaseFactory.ExecuteReader(Helper.GetDataBaseType, DataBaseConnectionString, SqlStatement).CreateDataReader())
                    {
                        while (_IDataReader.Read())
                        {
                            DepartmentEn _DepartmentEn = LoadObject(_IDataReader);
                            depEnList.Add(_DepartmentEn);
                        }
                        _IDataReader.Close();
                    }
                    
                }
               
            }
            catch (Exception ex)
            {
                throw ex;
            }
            //return data set
            return depEnList;

        }
        #endregion
        
    }

    
}
