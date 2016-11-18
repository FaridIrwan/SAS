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

Partial Class RptDailyReportViewer

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

                Dim strFromDate As Date
                Dim strToDate As Date
                Dim sortby As String = Nothing
                Dim sort As String = Nothing
                Dim str As String = Nothing

                If Session("fromdate") <> Nothing Or Session("todate") <> Nothing Then
                    strFromDate = Session("fromdate")
                    strToDate = Session("todate")
                End If

                If Session("sortby") <> Nothing Then
                    sortby = Session("sortby")
                    If sortby = "MatricNo" Then
                        sortby = "SASI_MatricNo"
                    ElseIf sortby = "Status" Then
                        sortby = "SASS_Code"
                    End If
                    sort = " order by " + sortby
                End If

                Dim strDate As String = Format(Date.Today, "dd/MM/yyyy").ToString
                str = "SELECT '" + Session("User") + "' as curuser,'" + strDate + "' as dateToday,CreditRef as SASI_MatricNo,SASI_Name,"
                str += " 'Terimaan " + strFromDate + "-" + strToDate + "' as Description ,TransAmount,'" + strFromDate + "-" + strToDate + "' as Transdate, "
                str += " SASS_Code as StudentStatus FROM SAS_Accounts inner join SAS_Student on "
                str += " SAS_Student.SASI_MatricNo = SAS_Accounts.CreditRef where Category = 'Receipt' and SubType = 'Student' "
                str += " and TransType = 'Debit' and PaymentMode = 'EFT' and PostStatus = 'Posted' and TransDate between '" + Format(strFromDate, "yyyy-MM-dd") + "' "
                str += " and '" + Format(strToDate, "yyyy-MM-dd") + "' group by CreditRef,TransAmount,SASI_Name,SASS_Code " + sort

                'DataSet Strating
                Dim _DataSet As DataSet = _ReportHelper.GetDataSet(str)
                _DataSet.Tables(0).TableName = "Table"
                Dim conv As New NumericCurrencyToWord()

                Dim row As DataRow
                For Each row In _DataSet.Tables(0).Rows
                    Dim db As Double = row.Item(5)
                    row.Item(1) = conv.Convert(db, "", "Cents")
                Next


                'Report AFCReport Loading XML not available

                Dim s As String = Server.MapPath("~/xml/DailyReport.xml")
                _DataSet.WriteXml(s)

                'Report AFCReport Ended XML



                If _DataSet.Tables(0).Rows.Count = 0 Then
                    Response.Write("No Record Found")
                Else

                    'Report Loading 

                    MyReportDocument.Load(Server.MapPath("~/GroupReport/RptDailyReport.rpt"))
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

        'Session("sortby") = Nothing
        'Session("fromdate") = Nothing
        'Session("todate") = Nothing

    End Sub
End Class
