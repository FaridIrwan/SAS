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

Partial Class RptDailyCashCollectionViewer
    Inherits System.Web.UI.Page

    Private MyReportDocument As New ReportDocument

#Region "Global Declarations "

    'Author			: Anil Kumar - T-Melmax Sdn Bhd
    'Created Date	: 20/05/2015

    Private _ReportHelper As New ReportHelper

#End Region

#Region "Page Load Starting  "

    'Author			: Anil Kumar - T-Melmax Sdn Bhd
    'Created Date	: 20/05/2015

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Dim datecondition As String = Nothing
        Dim sortbyvalue As String = Nothing
        Dim status As String = Nothing
        Dim condition As String = Nothing
        Dim condUser As String = Nothing
        Dim condStat As String = Nothing
        Dim str As String = Nothing
        Dim datef As String = Request.QueryString("fdate")
        Dim datet As String = Request.QueryString("tdate")
        Dim d1, m1, y1, d2, m2, y2 As String

        Try

            If Not IsPostBack Then

                If Session("sortby") Is Nothing Then

                    sortbyvalue = ""

                Else

                    sortbyvalue = " Order By " + Session("sortby")

                End If




                If Session("status") Is Nothing Then

                    condStat = ""

                ElseIf Session("status") = "1" Then

                    condStat = " and SASS_Code = 'PA'"

                Else

                    condStat = " and SASS_Code <> 'PA'"

                End If

                If Session("userCounter") Is Nothing Then

                    condUser = ""

                Else

                    condUser = "  and (postedby = '" + Session("userCounter") + "' or UpdatedBy = '" + Session("userCounter") + "') "

                End If

                If Request.QueryString("fdate") = "0" Or Request.QueryString("tdate") = "0" Or Request.QueryString("fdate") Is Nothing Or Request.QueryString("tdate") Is Nothing Then

                    datecondition = ""

                Else

                    d1 = Mid(datef, 1, 2)
                    m1 = Mid(datef, 4, 2)
                    y1 = Mid(datef, 7, 4)
                    Dim datefrom As String = y1 + "/" + m1 + "/" + d1
                    d2 = Mid(datet, 1, 2)
                    m2 = Mid(datet, 4, 2)
                    y2 = Mid(datet, 7, 4)
                    Dim dateto As String = y2 + "/" + m2 + "/" + d2

                    datecondition = " and sa.TransDate between '" + datefrom + "' and '" + dateto + "'"

                End If

                condition = condStat + condUser

                str = " select sa.UpdatedBy UserCounter,ss.SASI_MatricNo MatricNo,ss.SASI_Name StudentName,sa.Description "
                str += " descReceipt,sa.TransAmount TransAmount,sa.TransCode ReceiptNo,ss.SASS_Code StudentStatus,sa.TransDate "
                str += " TransDate from sas_accounts sa inner join SAS_Student ss on ss.SASI_MatricNo = sa.CreditRef where "
                str += " Category = 'Receipt' and SubType = 'Student' and PaymentMode = 'CSH' and poststatus = 'Posted' "
                str += datecondition + " "
                str += condition + " "
                str += sortbyvalue

                'Author			: Anil Kumar - T-Melmax Sdn Bhd
                'Created Date	: 20/05/2015

                'DataSet Strating
                Dim _DataSet As DataSet = _ReportHelper.GetDataSet(str)
                ' _DataSet.Tables(0).TableName = "Table"

                'Report CollectionAnalysis Loading XML

                Dim s As String = Server.MapPath("~/xml/DailyCash.xml")
                _DataSet.WriteXml(s)

                'Records Checking

                If _DataSet.Tables(0).Rows.Count = 0 Then
                    Response.Write("No Record Found")
                Else

                    'Report Loading

                    MyReportDocument.Load(Server.MapPath("~/GroupReport/RptDailyCashCollection.rpt"))
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

        Session("fromdate") = Nothing
        Session("todate") = Nothing

    End Sub
End Class
