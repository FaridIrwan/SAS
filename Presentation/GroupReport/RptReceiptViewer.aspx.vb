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
Partial Class RptReceiptViewer
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

                Dim str As String = Nothing
                Dim batchid As String = Nothing

                batchid = Request.QueryString("bat")
                Dim constr As String = Nothing


                If Session("LaporanHarian") = True Then

                    Dim strDate As String = Format(Date.Today, "yyyy/MM/dd").ToString

                    str = "SELECT '" + Session("User") + "' AS curuser, "
                    str += "CONVERT(VARCHAR(11),CONVERT(DATETIME,'" + strDate + "'),106) AS dateToday, "
                    str += "SAS_Student.SASI_MatricNo SASI_MatricNo, "
                    str += "SAS_Student.SASI_Name SASI_Name, "
                    str += "SAS_Accounts.Description AS Description, "
                    str += "CONVERT(DECIMAL(16,2),SAS_Accounts.TransAmount) TransAmount, "
                    str += "CONVERT(VARCHAR(11),sas_accounts.transdate,106) AS Transdate, "
                    str += "StudentStatus = CASE WHEN SAS_Student.SASS_Code = 'PA' THEN 'Pelajar Aktif' WHEN SAS_Student.SASS_Code <> 'PA' THEN 'Pelajar Tidak Aktif' End "
                    str += "FROM SAS_Accounts "
                    str += "INNER JOIN SAS_Student ON SAS_Accounts.CreditRef = SAS_Student.SASI_MatricNo "
                    str += "INNER JOIN  SAS_BankDetails ON SAS_Accounts.BankCode = SAS_BankDetails.SABD_Code  "
                    str += "WHERE SAS_Accounts.BatchCode = '" + batchid + "'"
                Else
                    str = " SELECT SAS_Student.SASI_MatricNo, 'NumberWord' as NumberToWord, SAS_Student.SASI_Name, SAS_Student.SASI_Add1 || SAS_Student.SASI_Add2 || SAS_Student.SASI_Add3 AS Address,"
                    str += "SAS_Accounts.TransCode,  SAS_Accounts.Description, SAS_Accounts.PaymentMode,SAS_Accounts.transdate as date, SAS_Accounts.TransAmount, SAS_Accounts.BatchCode, "
                    str += "SAS_Accounts.BankCode, 'demo' UserID  FROM SAS_Accounts INNER JOIN  SAS_Student ON SAS_Accounts.CreditRef = SAS_Student.SASI_MatricNo "
                    str += "INNER JOIN  SAS_BankDetails ON SAS_Accounts.BankCode = SAS_BankDetails.SABD_Code  where SAS_Accounts.BatchCode='" + batchid + "'"

                End If

                'Author			: Anil Kumar - T-Melmax Sdn Bhd
                'Created Date	: 20/05/2015
                'CONVERT(VARCHAR(11),SAS_Accounts.transdate,106) as date
                'DataSet Strating
                Dim _DataSet As DataSet = _ReportHelper.GetDataSet(str)

                'Report AFCReport Loading XML

                Dim s As String = Server.MapPath("~\xml\Receipts.xml")
                _DataSet.WriteXml(s)

                'Report AFCReport Ended XML

                'Records Checking

                If _DataSet.Tables(0).Rows.Count = 0 Then
                    Response.Write("No Record Found")
                Else

                    'Report Loading 


                    If Session("LaporanHarian") = True Then

                        MyReportDocument.Load(Server.MapPath("~/GroupReport/RptDailyReport.rpt"))

                    Else

                        MyReportDocument.Load(Server.MapPath("~/GroupReport/RptReceipt.rpt"))

                    End If

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
        Session("LaporanHarian") = False
        'MyReportDocument.Close()
        'MyReportDocument.Dispose()
    End Sub

    'Private Function CrystalReportViewer1() As Object
    '    Throw New NotImplementedException
    'End Function

End Class
