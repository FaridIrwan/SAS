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
    public class DashboardDAL
    {
        #region Global Declarations 

        private DbParameterCollection _DbParameterCollection = null;

        private MaxModule.DatabaseProvider _DatabaseFactory =
            new MaxModule.DatabaseProvider();

        private string DataBaseConnectionString = Helper.
           GetConnectionString();

        #endregion

        #region Get Dashboard 

        public enum DashboardType
        {
            Student_Outstanding_Invoice = 1,
            Collection_By_Semester = 2,
            Net_Invoices_Against_Payment = 3,
            Sponsor_Ageing_Monthly = 4,
            Student_Ageing_By_Faculty = 5
        }

        public DataSet GetData(DashboardType dashboardType)
        {
            DataSet dt = new DataSet();

            string strStoredProc = GetStoredProcedureName(dashboardType);

            if (!string.IsNullOrEmpty(strStoredProc))
            {
                dt = _DatabaseFactory.ExecuteDataSet(Helper.GetDataBaseType,
                    DataBaseConnectionString, strStoredProc, true);
            }
            return dt;
        }

        public string GetStoredProcedureName(DashboardType dashboardType)
        {
            switch (dashboardType)
            {
                case DashboardType.Collection_By_Semester:
                    return "DB_GetCollectionBySemester";
                
                case DashboardType.Net_Invoices_Against_Payment:
                    return "DB_GetNetInvoicesAgainstPayment";

                case DashboardType.Sponsor_Ageing_Monthly:
                    return "DB_GetSponsorAgeingMonthly";

                case DashboardType.Student_Ageing_By_Faculty:
                    return "DB_GetStudentAgeingByFaculty";

                case DashboardType.Student_Outstanding_Invoice:
                    return "DB_GetStudentOutstandingInvoice";

                default:
                    return string.Empty;
            }
        }

        #endregion
    }
}
