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

Partial Class RptLetterToBIMBViewerEn
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
                Dim condition As String = Nothing
                Dim condVoc As String = Nothing
                Dim condStat As String = Nothing
                Dim str As String = Nothing

                Dim dateFromTo As String = Format(Today.Date, "dd/MM/yyyy")
                Dim VoucherNo As String = Nothing

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

                If Session("VoucherNo") = Nothing Then
                    condVoc = ""
                Else
                    VoucherNo = Session("VoucherNo")
                    condVoc = " and sa.VoucherNo = '" + VoucherNo + "'"
                End If

                If Request.QueryString("fdate") = "0" Or Request.QueryString("tdate") = "0" Or Request.QueryString("fdate") Is Nothing Or Request.QueryString("tdate") Is Nothing Then
                    datecondition = ""
                Else
                    Dim datef As String = Request.QueryString("fdate")
                    Dim datet As String = Request.QueryString("tdate")
                    Dim d1, m1, y1, d2, m2, y2 As String
                    d1 = Mid(datef, 1, 2)
                    m1 = Mid(datef, 4, 2)
                    y1 = Mid(datef, 7, 4)
                    Dim datefrom As String = y1 + "/" + m1 + "/" + d1
                    d2 = Mid(datet, 1, 2)
                    m2 = Mid(datet, 4, 2)
                    y2 = Mid(datet, 7, 4)
                    Dim dateto As String = y2 + "/" + m2 + "/" + d2

                    datecondition = " and sa.TransDate between '" + datefrom + "' and '" + dateto + "'"

                    dateFromTo = datefrom + "-" + dateto
                End If

                condition = condStat + condVoc

                Dim constr As String
                constr = WebConfigurationManager.ConnectionStrings("SASNewConnectionString").ConnectionString

                If Session("type") = "Sponsor" Then
                    str = " select '" + Session("UP") + "' UP,'" + Session("Cek") + "' Cek,'" + Session("DateCredit") + "' tarikhCredit,"
                    str += " ss.SASI_MatricNo MatricNo,ss.SASI_ICNo ICNO,ss.SASI_Name StudentName,sa.TransAmount "
                    str += " TransAmount,sa.TransCode ReceiptNo,ss.SASS_Code StudentStatus,sa.TransDate TransDate,"
                    str += " ss.SASI_PgId ProgramCode,sp.SAPG_ProgramBM ProgramName "
                    str += " from SAS_Accounts sa inner join SAS_Student ss on ss.SASI_MatricNo = sa.CreditRef inner "
                    str += " join SAS_Program sp on sp.SAPG_Code = ss.SASI_PgId where "
                    str += " sa.Category = 'SPA' and Description <> 'Sponsor Pocket Amount' and SubType = 'Student' "
                    str += " and PostStatus = 'Posted' and CreditRef1 <> '20' "
                    str += datecondition + " "
                    str += condition + " "
                    str += sortbyvalue
                ElseIf Session("type") = "Refund" Then
                    str = " select '" + Session("UP") + "' UP,'" + Session("Cek") + "' Cek,'" + Session("DateCredit") + "' tarikhCredit,"
                    str += " ss.SASI_MatricNo MatricNo,ss.SASI_AccNo AccountNo,ss.SASI_ICNo ICNO,ss.SASI_Name StudentName,sa.TransAmount "
                    str += " TransAmount,ss.SASS_Code StudentStatus, ss.SASI_PgId ProgramCode,sp.SAPG_ProgramBM ProgramName "
                    str += " from SAS_Accounts sa inner join SAS_Student ss on ss.SASI_MatricNo = sa.CreditRef inner "
                    str += " join SAS_Program sp on sp.SAPG_Code = ss.SASI_PgId where "
                    str += " sa.Category = 'Refund' and SubType = 'Student' and PostStatus = 'Posted' "
                    str += datecondition + " "
                    str += condition + " "
                    str += sortbyvalue
                End If


                'Author			: Anil Kumar - T-Melmax Sdn Bhd
                'Created Date	: 20/05/2015

                'DataSet Strating
                Dim _DataSet As DataSet = _ReportHelper.GetDataSet(str)

                'Report AFCReport Loading XML
                Dim s As String = Server.MapPath("~/xml/BIMBLetter.xml")
                _DataSet.WriteXml(s)

                'Report AFCReport Ended XML

                'Records Checking

                If _DataSet.Tables(0).Rows.Count = 0 Then
                    Response.Write("No Record Found")
                Else

                    'Report Loading 

                    MyReportDocument.Load(Server.MapPath("~/GroupReport/RptLetterToBIMBEn.rpt"))
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
        Session("DateCredit") = Nothing
        Session("Cek") = Nothing
        Session("UP") = Nothing
        'MyReportDocument.Close()
        'MyReportDocument.Dispose()
    End Sub
End Class
