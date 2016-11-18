Imports HTS.SAS.Entities
Imports HTS.SAS.BusinessObjects
Imports System.Data
Imports System.Data.SqlClient
Imports System.Web.Configuration
Imports System.Collections.Generic
Imports CrystalDecisions.CrystalReports.Engine
Partial Class RptStudentOutStandingViewer
    Inherits System.Web.UI.Page
    Private MyReportDocument As New ReportDocument

#Region "Global Declarations "
    'Author			: Anil Kumar - T-Melmax Sdn Bhd
    'Created Date	: 20/05/2015

    Private _ReportHelper As New ReportHelper

#End Region

#Region "Page Load Starting  "

    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init

        'Author			: Anil Kumar - T-Melmax Sdn Bhd
        'Created Date	: 20/05/2015
        'Modified by Hafiz @ 02/3/2016
        'Modified by Hafiz @ 26/4/2016

        Try

            If Not IsPostBack Then
                Dim _ReportHelper As New ReportHelper

                Dim datecondition As String = Nothing
                Dim sortbyvalue As String = Nothing
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

                If Session("status") Is Nothing Then

                    condStat = ""

                ElseIf Session("status") = "1" Then

                    condStat = " and ss.SASS_Code IN " + _ReportHelper.Status("1") + " "

                Else

                    condStat = " and ss.SASS_Code <> " + _ReportHelper.Status("1") + " "

                End If


                If Session("faculty") Is Nothing Then

                    faculty = ""

                Else

                    faculty = Session("faculty")

                End If

                If Session("program") Is Nothing Then

                    program = ""

                Else

                    program = Session("program")
                    condProg = " and SS.SASI_PgId = '" & program & "'"

                End If

                'Modified By Syafiq 10 Feb 2014

                Dim datef As String = Request.QueryString("fdate")
                Dim datet As String = Request.QueryString("tdate")
                Dim d1, m1, y1, d2, m2, y2 As String
                d1 = Mid(datef, 1, 2)
                m1 = Mid(datef, 4, 2)
                y1 = Mid(datef, 7, 4)
                'Dim datefrom As String = y1 + "-" + m1 + "-" + d1
                Dim datefrom As String = "2013-01-01"
                d2 = Mid(datet, 1, 2)
                m2 = Mid(datet, 4, 2)
                y2 = Mid(datet, 7, 4)
                Dim dateto As String = y2 + "-" + m2 + "-" + d2

                If Request.QueryString("tdate") = "0" Or Request.QueryString("tdate") Is Nothing Then
                    datecondition = ""
                Else
                    datecondition = " and SA.TransDate between '" + datefrom + "' and '" + dateto + "'"
                End If

                condition = condProg + condStat

                'str = "SELECT SS.SASI_MatricNo MatricNo, "
                'str += "SS.SASI_Name AS Name, "
                'str += "SS.SASI_Faculty || ' - ' || SF.SAFC_Desc AS Faculty, "
                'str += "SS.SASI_PgId || ' - ' || SP.SAPG_ProgramBM Program, "
                'str += "SS.SASI_CurSem Semester, "
                'str += "COALESCE(X1.DebitAmount, 0) DebitAmount, "
                'str += "COALESCE(X2.CreditAmount, 0) CreditAmount, "
                'str += "(COALESCE(X2.CreditAmount, 0)- COALESCE(X1.DebitAmount, 0)) OutStandingAmt, "
                'str += "to_char(current_date ,'" + dateto + "') Dateto, "
                'str += "SASS_Code = CASE WHEN SS.SASS_Code = '1' THEN 'Pelajar Aktif' WHEN SS.SASS_Code <> '1' THEN 'Pelajar Tidak Aktif' End "
                'str += "FROM ((SAS_Student SS LEFT JOIN SAS_Program SP ON SP.SAPG_Code = SS.SASI_PgId LEFT JOIN SAS_Faculty SF ON SF.SAFC_Code = SS.SASI_Faculty) "
                'str += "LEFT JOIN (SELECT CreditRef AS MatricNo , COALESCE(SUM(TRANSAMOUNT), 0) AS DebitAmount "
                'str += "FROM SAS_Accounts "
                'str += "WHERE TRANSTYPE = 'DEBIT' AND TransDate BETWEEN '" + datefrom + "' AND '" + dateto + "' "
                'str += "GROUP BY CreditRef) X1 ON SS.SASI_MatricNo = X1.MatricNo) "
                'str += "LEFT JOIN (SELECT CreditRef AS MatricNo, COALESCE(SUM(TRANSAMOUNT), 0) AS CreditAmount "
                'str += "FROM SAS_Accounts WHERE TRANSTYPE = 'CREDIT' AND TransDate BETWEEN '" + datefrom + "' AND '" + dateto + "' "
                'str += "GROUP BY CreditRef) X2 ON SS.SASI_MatricNo = X2.MatricNo "
                'str += "WHERE (COALESCE(X2.CreditAmount, 0)- COALESCE(X1.DebitAmount, 0)) > '0' "

                str = " SELECT DISTINCT SS.SASI_MatricNo MatricNo, SS.SASI_Name AS Name, SS.SASI_Faculty || ' - ' || SF.SAFC_Desc AS Faculty, "
                'str += "SS.SASI_PgId || ' - ' || SP.SAPG_ProgramBM Program, SS.SASI_CurSem Semester, (COALESCE(SO.SASO_DueAmount, 0) + COALESCE(SO.SASO_LoanAmount, 0)) OutStandingAmt, "
                str += "SS.SASI_PgId || ' - ' || SP.SAPG_ProgramBM Program, SS.SASI_CurSem Semester, (COALESCE(SO.saso_outstandingamt, 0)) OutStandingAmt, "
                str += "to_char(current_date ,'" + dateto + "') Dateto, CASE WHEN SS.SASS_Code = '1' THEN 'Pelajar Aktif' WHEN SS.SASS_Code <> '1' THEN 'Pelajar Tidak Aktif' End "
                str += "FROM SAS_Student SS "
                str += "INNER JOIN SAS_Faculty SF ON SF.SAFC_Code = SS.sasi_faculty "
                str += "INNER JOIN SAS_Program SP ON SP.SAPG_Code = SS.SASI_PgId "
                str += "INNER JOIN SAS_StudentOutstanding SO ON SS.SASI_MatricNo = SO.SASI_MatricNo "
                str += "LEFT JOIN SAS_Accounts SA ON SA.creditref = SS.SASI_MatricNo "
                str += "WHERE SA.TransDate BETWEEN '" + datefrom + "' AND '" + dateto + "' "
                'str += "AND (COALESCE(SO.SASO_DueAmount, 0) + COALESCE(SO.SASO_LoanAmount, 0)) > 0 "
                str += "AND (COALESCE(SO.saso_outstandingamt, 0)) > 0 "

                'Check for Faculty
                If Not String.IsNullOrEmpty(Session("Faculty")) And Session("Faculty") <> "" Then

                    str += "AND SASI_Faculty = '" & Session("Faculty") & "' "

                End If

                str += condition + " "
                str += sortbyvalue

                'Author			: Anil Kumar - T-Melmax Sdn Bhd
                'Created Date	: 20/05/2015

                'DataSet Strating
                Dim _DataSet As DataSet = _ReportHelper.GetDataSet(str)
                _DataSet.Tables(0).TableName = "Table"

                'Report XML Loading

                Dim s As String = Server.MapPath("~/xml/OutstandingAmt.xml")
                _DataSet.WriteXml(s)

                'Report XML Ended

                'Records Checking

                If _DataSet.Tables(0).Rows.Count = 0 Then
                    Response.Write("No Record Found")

                Else

                    'Report Loadin
                    MyReportDocument.Load(Server.MapPath("~/GroupReport/RptStudentOutStanding.rpt"))
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
        'MyReportDocument.Close()
        'MyReportDocument.Dispose()
    End Sub

End Class
