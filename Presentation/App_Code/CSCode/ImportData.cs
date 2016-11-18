using System;
using System.Collections.Generic;
using System.Web;
using NPOI;
using NPOI.HSSF.UserModel;
using NPOI.SS.Util;
using System.Data;
using System.IO;
using HTS.SAS.Entities;
using HTS.SAS.BusinessObjects;
/// <summary>
/// Summary description for ImportData
/// </summary>
public class ImportData
{
	public ImportData()
	{
		//
		// TODO: Add constructor logic here
		//
	}

    public List<StudentEn> GetImportedSponsorData(string filePath)
    {
        DataTable dtSource = null;

        dtSource = GetSourceFromExcel(filePath);
        List<StudentEn> _studentlist = new List<StudentEn>();
        _studentlist = ConvertToList(dtSource);
        return _studentlist;
    }

    /// <summary>
    /// This method is to convert dataTable to List
    /// </summary>
    /// <param name="dtCurrency">DataTable - currency details</param>
    private List<StudentEn> ConvertToList(DataTable dtStudent)
    {
        string currencyString = string.Empty;
        try
        {
            List<StudentEn> lstStudents = new List<StudentEn>();
            StudentEn _studentEN = new StudentEn();
          

            for (int index = 0; index < dtStudent.Rows.Count; index++)
            {
                _studentEN = new StudentEn();
                if (dtStudent.Rows[index]["MatricNo"].ToString() != "")
                {
                _studentEN.MatricNo = dtStudent.Rows[index]["MatricNo"].ToString();
                _studentEN.StudentName = dtStudent.Rows[index]["StudentName"].ToString();
                _studentEN.ProgramID = dtStudent.Rows[index]["Program"].ToString();
                _studentEN.CurrentSemester =Convert.ToInt32(dtStudent.Rows[index]["Semester"].ToString());
                _studentEN.ICNo = dtStudent.Rows[index]["ICNo"].ToString();
                _studentEN.TransactionAmount = Convert.ToDouble(dtStudent.Rows[index]["AllocatedAmount"]);
                _studentEN.TempAmount = Convert.ToDouble(dtStudent.Rows[index]["PocketAmount"]);
                lstStudents.Add(_studentEN);
                }
            }
            return lstStudents;
        }
        catch (Exception err)
        {
            throw err;
        }
    }

    private DataTable GetSourceFromExcel(string file)
    {
        DataTable dtPatterns = new DataTable();
        int column = 0;
        int row = 0;

        HSSFWorkbook hssfworkbook;
        FileStream fileName = new FileStream(file, FileMode.Open, FileAccess.Read);
        hssfworkbook = new HSSFWorkbook(fileName);
        HSSFSheet sheet1 = (HSSFSheet)hssfworkbook.GetSheet("Sheet1");
        if (sheet1.LastRowNum > 0)
        {
            for (row = 0; row <= sheet1.LastRowNum; row++)
            {
                HSSFRow dataRow = (HSSFRow)sheet1.GetRow(row);
                if (row.Equals(0))
                {
                    dtPatterns.Columns.Add("MatricNo", typeof(string));        
                    dtPatterns.Columns.Add("StudentName", typeof(string));
                    dtPatterns.Columns.Add("Program", typeof(string));
                    dtPatterns.Columns.Add("Semester", typeof(string));
                    dtPatterns.Columns.Add("ICNo", typeof(string));
                    dtPatterns.Columns.Add("AllocatedAmount", typeof(double));
                    dtPatterns.Columns.Add("PocketAmount", typeof(double));
                }
                else
                {
                    DataRow dr = dtPatterns.NewRow();
                    //Add the value to the row
                    for (column = 0; column < dataRow.Cells.Count; column++)
                    {
                        dr[column] = dataRow.Cells[column].ToString();
                    }
                    dtPatterns.Rows.Add(dr);
                }
            }
        }

        return dtPatterns;
    }

}