Imports System.Data
Imports System.Data.SqlClient
Imports System.Web.Configuration
Imports CrystalDecisions.CrystalReports.Engine
Imports System.Collections
Imports System.Web
Imports System.Web.Security
Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports System.Web.UI.WebControls.WebParts
Imports System.Web.UI.HtmlControls
Imports Microsoft.Reporting.WebForms

Partial Class RptRefreshmentFundViewerEn
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
                Dim datecondition As String = Nothing
                Dim sortbyvalue As String = Nothing
                Dim status As String = Nothing
                Dim program As String = Nothing
                Dim faculty As String = Nothing
                Dim condition As String = Nothing
                Dim condProg As String = Nothing
                Dim condStat As String = Nothing
                Dim str As String = Nothing

                If Session("sortby") Is Nothing Then

                    sortbyvalue = ""

                Else

                    sortbyvalue = " Order By " + Session("sortby")

                End If

                If Session("Status") = "All" Then

                    condStat = ""

                ElseIf Session("Status") = "Active" Then

                    condStat = " and SASS_Code = 'PA'"

                Else

                    condStat = " and SASS_Code <> 'PA'"

                End If


                If Session("Faculty") Is Nothing Then

                    faculty = "%"

                Else

                    faculty = Session("Faculty")

                End If

                If Session("Program") Is Nothing Then

                    program = "%"

                Else

                    program = Session("Program")
                    condProg = " and B.SASI_PgId = '" & program & "'"

                End If

                'Modified By Syafiq 10 Feb 2014

                Dim datef As String = Request.QueryString("fdate")
                Dim datet As String = Request.QueryString("tdate")
                Dim d1, m1, y1, d2, m2, y2 As String
                d1 = Mid(datef, 1, 2)
                m1 = Mid(datef, 4, 2)
                y1 = Mid(datef, 7, 4)
                Dim datefrom As String = y1 + "-" + m1 + "-" + d1
                d2 = Mid(datet, 1, 2)
                m2 = Mid(datet, 4, 2)
                y2 = Mid(datet, 7, 4)
                Dim dateto As String = y2 + "-" + m2 + "-" + d2

                If Request.QueryString("fdate") = "0" Or Request.QueryString("tdate") = "0" Or Request.QueryString("fdate") Is Nothing Or Request.QueryString("tdate") Is Nothing Then
                    datecondition = ""
                Else
                    datecondition = "and A.TransDate between '" + datefrom + "' and '" + dateto + "'"
                End If

                condition = condProg + condStat

                Dim strStartDate = Format(CDate("2013-01-01"), "dd/MM/yyyy")
                Dim strDateTo = Format(CDate(dateto), "dd/MM/yyyy")

                str = "SELECT ROW_NUMBER() OVER (ORDER BY CreditRef) AS No, CreditRef AS MatricNo, B.SASI_Name AS Name, "
                str += "B.SASI_Faculty || ' - ' || D.SAFC_Desc AS Faculty, "
                str += "B.SASI_PgId || ' - ' || C.SAPG_ProgramBM Program, "
                str += "BatchCode, TransCode, Description, "
                str += "CAST(TransAmount as DECIMAL(9,2)) AS LoanAmount, CAST(PaidAmount as DECIMAL(9,2)) AS PaidAmount, "
                str += "COALESCE(CAST(TransAmount as DECIMAL(9,2)) - CAST(PaidAmount as DECIMAL(9,2)),0) AS Balance, "
                str += "to_char(TransDate,'DD/MM/YYYY') AS TransDate, "
                str += "SASS_Code = CASE WHEN B.SASS_Code = 'PA' THEN 'Pelajar Aktif' "
                str += "WHEN B.SASS_Code <> 'PA' THEN 'Pelajar Tidak Aktif' End, "
                str += "'" + strStartDate + "' StartDate, "
                str += "'" + strDateTo + "' Enddate "
                str += "FROM SAS_Accounts A "
                str += "LEFT JOIN SAS_Student B ON A.CreditRef = B.SASI_MatricNo "
                str += "LEFT JOIN SAS_Program C ON B.SASI_PgId = C.SAPG_Code "
                str += "LEFT JOIN SAS_Faculty D ON D.SAFC_Code = B.SASI_Faculty "
                str += "WHERE (Category = N'INVOICE') AND (Description LIKE N'%makan%') "
                str += datecondition + " "
                str += condition + " "
                str += sortbyvalue

                'Author			: Anil Kumar - T-Melmax Sdn Bhd
                'Created Date	: 20/05/2015

                'DataSet Strating
                Dim _DataSet As DataSet = _ReportHelper.GetDataSet(str)

                'Report XML Loading
                Dim s As String = Server.MapPath("~/xml/DanaMakanMinum.xml")
                _DataSet.WriteXml(s)

                'Report XML Ended

                'Records Checking

                If _DataSet.Tables(0).Rows.Count = 0 Then
                    Response.Write("No Record Found")

                Else

                    'Report Loading 

                    MyReportDocument.Load(Server.MapPath("~/GroupReport/RptRefreshmentFundEn.rpt"))
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

    Protected Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Unload
        Session("Status") = Nothing
        Session("Program") = Nothing
        Session("fromdate") = Nothing
        Session("todate") = Nothing
        'MyReportDocument.Close()
        'MyReportDocument.Dispose()
    End Sub
End Class
