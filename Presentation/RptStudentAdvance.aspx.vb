Imports System.Data
Imports System.Data.SqlClient
Imports System.Web.Configuration
Imports CrystalDecisions.CrystalReports.Engine

Partial Class RptStudentAdvance
    Inherits System.Web.UI.Page
    Private MyReportDocument As New ReportDocument

#Region "Global Declarations "
    'Author			: Anil Kumar - T-Melmax Sdn Bhd
    'Created Date	: 20/05/2015

    Private _ReportHelper As New ReportHelper

#End Region

#Region "Page Load Starting  "

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        'Author			: Anil Kumar - T-Melmax Sdn Bhd
        'Created Date	: 20/05/2015

        Try

            If Not IsPostBack Then

                Dim cond As String = Nothing
                Dim batchNo As String = Nothing
                Dim cat As String = Nothing
                Dim str As String = Nothing

                batchNo = Request.QueryString("BatchNo")

                'str = "SELECT DISTINCT "
                'str += " SAS_StudentLoan.BatchCode , "
                'str += " SAS_StudentLoan.BatchIntake ,SAS_StudentLoan.Description,'Advance' as SAFT_Desc, "
                'str += " CONVERT(VARCHAR(10), SAS_StudentLoan.BatchDate, 103) BatchDate , "
                'str += " CONVERT(VARCHAR(10), SAS_StudentLoan.TransDate, 103) TransDate , "
                'str += " CONVERT(VARCHAR(10), SAS_StudentLoan.DueDate, 103) DueDate , "
                'str += " SAS_StudentLoan.TransAmount "
                'str += "FROM SAS_StudentLoan  "
                'str += "WHERE   SAS_StudentLoan.BatchCode='" + batchNo + "';"


                str = "SELECT DISTINCT  SAS_StudentLoan.BatchCode ,  SAS_StudentLoan.BatchIntake ,SAS_StudentLoan.Description,'Advance' as SAFT_Desc, "
                str += "to_char(SAS_StudentLoan.BatchDate, 'DD/MM/YYYY') BatchDate , to_char(SAS_StudentLoan.TransDate, 'DD/MM/YYYY') TransDate , "
                str += "to_char(SAS_StudentLoan.DueDate, 'DD/MM/YYYY') DueDate ,  SAS_StudentLoan.TransAmount FROM SAS_StudentLoan  WHERE   SAS_StudentLoan.BatchCode='" + batchNo + "';"
                'Author			: Anil Kumar - T-Melmax Sdn Bhd
                'Created Date	: 20/05/2015

                'DataSet Strating
                Dim _DataSet As DataSet = _ReportHelper.GetDataSet(str)
                _DataSet.Tables(0).TableName = "Table"
                'Report XML Loading

                Dim s As String = Server.MapPath("xml\StudAgeing.xml")
                _DataSet.WriteXml(s)

                'Report XML Ended

                'Records Checking

                If _DataSet.Tables(0).Rows.Count = 0 Then
                    Response.Write("No Record Found")

                Else

                    'Report Loading

                    MyReportDocument.Load(Server.MapPath("~/ProcessReport/RptStudentAdvance.rpt"))
                    MyReportDocument.SetDataSource(_DataSet)
                    Session("reportobject") = MyReportDocument
                    CrystalReportViewer1.ReportSource = MyReportDocument
                    CrystalReportViewer1.DataBind()
                    MyReportDocument.Refresh()

                    'Report Ended

                End If

            Else

                'Report Loading

                MyReportDocument = Session("reportobject")
                CrystalReportViewer1.ReportSource = MyReportDocument
                CrystalReportViewer1.DataBind()
                MyReportDocument.Refresh()

                'Report Ended

            End If

        Catch ex As Exception

            Response.Write(ex.Message)

        End Try
    End Sub
#End Region
End Class
