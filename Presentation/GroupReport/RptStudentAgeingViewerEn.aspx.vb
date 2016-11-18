Imports System.Data
Imports System.Data.SqlClient
Imports System.Web.Configuration
Imports CrystalDecisions.CrystalReports.Engine

Partial Class RptStudentAgeingViewerEn
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

                Dim status As String = Nothing
                Dim datecondition As String = Nothing
                Dim sortbyvalue As String = Nothing
                Dim dateto As String = Nothing
                Dim datefrom As String = Nothing
                Dim strCondition As String = Nothing
                Dim strCondProgram As String = Nothing
                Dim strCondStatus As String = Nothing
                Dim str As String = Nothing

                If Request.QueryString("tdate") = "0" Or Request.QueryString("tdate") Is Nothing Then
                    datecondition = ""
                Else
                    Dim datef As String = Request.QueryString("fdate")
                    Dim datet As String = Request.QueryString("tdate")
                    Dim datelen As Integer = Len(datef)
                    Dim d1, m1, y1, d2, m2, y2 As String
                    d1 = Mid(datef, 1, 2)
                    m1 = Mid(datef, 4, 2)
                    y1 = Mid(datef, 7, 4)
                    'datefrom = y1 + "/" + m1 + "/" + d1
                    datefrom = "2013/01/01"
                    d2 = Mid(datet, 1, 2)
                    m2 = Mid(datet, 4, 2)
                    y2 = Mid(datet, 7, 4)
                    dateto = y2 + "/" + m2 + "/" + d2

                    datecondition = "AND A.PostedDateTime BETWEEN '" + datefrom + "' AND '" + dateto + "'"

                End If

                str = "SELECT DISTINCT TB1.CreditRef MatricNo, TB3.SASI_Name AS  Name, "
                str += "TB3.SASI_Faculty || ' - ' || D.SAFC_Desc AS Faculty, "
                str += "TB3.SASI_PgId || ' - ' || C.SAPG_ProgramBM Program, "
                str += "TB1.DebitAmount, "
                str += "TB2.CreditAmount, "
                str += "CASE WHEN SA.TransDate::date -'" + dateto + "'::date <= 365 THEN (COALESCE(TB2.CreditAmount, 0) - COALESCE(TB1.DebitAmount, 0)) ELSE '0.00' END MoreThanYear, "
                str += "CASE WHEN SA.TransDate::date-'" + dateto + "'::date > 365 AND SA.TransDate::date -'" + dateto + "'::date <= 1095 THEN (COALESCE(TB2.CreditAmount, 0) - COALESCE(TB1.DebitAmount, 0)) ELSE '0.00' END AS OneToThreeYears, "
                str += "CASE WHEN SA.TransDate::date -'" + dateto + "'::date > 1095 THEN (COALESCE(TB2.CreditAmount, 0) - COALESCE(TB1.DebitAmount, 0)) ELSE '0.00' END AS MoreThanThreeYears, "
                str += "CASE WHEN SA.TransDate::date -'" + dateto + "'::date <= 365 THEN (COALESCE(TB2.CreditAmount, 0) - COALESCE(TB1.DebitAmount, 0)) ELSE '0.00' END + "
                str += "CASE WHEN SA.TransDate::date -'" + dateto + "'::date > 365 AND SA.TransDate::date-'" + dateto + "'::date <= 1095 THEN (COALESCE(TB2.CreditAmount, 0) - COALESCE(TB1.DebitAmount, 0)) ELSE '0.00' END + "
                str += "CASE WHEN SA.TransDate::date -'" + dateto + "'::date > 1095 THEN (COALESCE(TB2.CreditAmount, 0) - COALESCE(TB1.DebitAmount, 0)) ELSE '0.00' END AS TotalAmount, "
                str += "to_char(current_date,'" + dateto + "') Dateto, "
                str += "SASS_Code = CASE WHEN TB3.SASS_Code = '1' THEN 'Pelajar Aktif' WHEN TB3.SASS_Code <> '1' THEN 'Pelajar Tidak Aktif' End "
                str += "FROM SAS_Accounts SA "
                str += "INNER JOIN (SELECT B.SASI_MatricNo, B.SASI_Name, B.SASI_PgId, B.SASI_Faculty, B.SASS_Code, B.SASI_StatusRec FROM SAS_Student B) TB3 ON SA.CreditRef = TB3.SASI_MatricNo "
                str += "LEFT JOIN SAS_Program C ON C.SAPG_Code = TB3.SASI_PgId "
                str += "LEFT JOIN SAS_Faculty D ON D.SAFC_Code = TB3.SASI_Faculty "
                str += "LEFT JOIN (SELECT A.CreditRef, SUM(A.TransAmount) DebitAmount FROM SAS_Accounts A "
                str += "WHERE A.TransDate BETWEEN '" + datefrom + "' AND '" + dateto + "' AND A.TransType = 'DEBIT' "
                str += "GROUP BY A.CreditRef) TB1 ON TB3.SASI_MatricNo = TB1.CreditRef "
                str += "LEFT JOIN (SELECT A.CreditRef, SUM(A.TransAmount) CreditAmount FROM SAS_Accounts A "
                str += "WHERE A.TransDate BETWEEN '" + datefrom + "' AND '" + dateto + "' AND A.TransType = 'CREDIT' "
                str += "GROUP BY A.CreditRef) TB2 ON TB3.SASI_MatricNo = TB2.CreditRef "
                str += "WHERE  TB3.SASI_StatusRec = '1' "
                str += "AND (COALESCE(TB2.CreditAmount, 0) - COALESCE(TB1.DebitAmount, 0)) > '0' "

                'Check for Faculty
                If Not String.IsNullOrEmpty(Session("Faculty")) And Session("Faculty") <> "" Then

                    str += "AND TB2.SASI_Faculty = '" & Session("Faculty") & "' "

                End If

                'Check for Program
                If Not String.IsNullOrEmpty(Session("Program")) And Session("Program") <> "" Then

                    str += "AND TB2.SASI_PgId = '" & Session("Program") & "' "

                End If

                'Check for Status
                If Session("Status") = "Active" Then

                    str += "AND TB3.SASS_Code = '1' "

                Else

                    str += "AND TB3.SASS_Code <> '1' "

                End If


                'Author			: Anil Kumar - T-Melmax Sdn Bhd
                'Created Date	: 20/05/2015

                'DataSet Strating
                Dim _DataSet As DataSet = _ReportHelper.GetDataSet(str)

                'Report XML Loading

                Dim s As String = Server.MapPath("~/xml/StudentAgeing.xml")
                _DataSet.WriteXml(s)

                'Report XML Ended

                'Records Checking

                If _DataSet.Tables(0).Rows.Count = 0 Then
                    Response.Write("No Record Found")

                Else

                    'Report Loading 

                    'MyReportDocument.Load(Server.MapPath("RptStuAgeingCRFinal.rpt"))
                    MyReportDocument.Load(Server.MapPath("~/GroupReport/RptStudentAgeingEn.rpt"))
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
