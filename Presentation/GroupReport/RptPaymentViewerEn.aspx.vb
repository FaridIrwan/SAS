Imports System.Data
Imports System.Data.SqlClient
Imports System.Web.Configuration
Imports CrystalDecisions.CrystalReports.Engine

Partial Class RptPaymentViewerEn
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
                Dim mode As String = Nothing
                Dim category As String = Nothing
                Dim str As String = Nothing

                mode = Request.QueryString("mode")

                If mode = "1" Then
                    category = "Payment"
                Else
                    category = "Refund"
                End If
                If Not Session("statusPrint") Is Nothing Then

                Else

                End If

                str = "SELECT 'Status' = CASE WHEN Category = 'Payment' THEN 'Allocation' Else 'Refund' END,"
                str += "'StatusPaymentMode' = CASE WHEN PaymentMode = 'CHQ' THEN 'CHEQUE' Else 'TELEGRAPHIC TRANSFER' END,"
                str += "BankCode,Description, CreditRef, CreditRef1, BatchCode,CONVERT(VARCHAR(10), BatchDate, 103) BatchDate, CONVERT(VARCHAR(10), TransDate, 103) TransDate,"
                str += "SubRef1, SAS_AccountsDetails.TransAmount,SAS_Accounts.VoucherNo "
                str += ",SAS_Student.SASI_MatricNo,SAS_Student.SASI_Name,SAS_Student.SASI_PgId, SAS_Student.SASI_CurSem, SAS_AccountsDetails.TransAmount,SAS_Accounts.VoucherNo, SAS_Accounts.PocketAmount "
                str += "FROM SAS_Accounts Inner Join SAS_AccountsDetails on SAS_AccountsDetails.transid = SAS_Accounts.TransID "
                str += "INNER JOIN SAS_Student ON SAS_AccountsDetails.RefCode = SAS_Student.SASI_MatricNo"
                str += " WHERE SAS_Accounts.Category='" & category & "' AND SAS_Accounts.PostStatus='" & Session("statusPrint") & _
                       "' and SAS_Accounts.SubType ='Student'"
                str += cond


                'Author			: Anil Kumar - T-Melmax Sdn Bhd
                'Created Date	: 20/05/2015

                'DataSet Strating
                Dim _DataSet As DataSet = _ReportHelper.GetDataSet(str)

                'Report AFCReport Loading XML

                Dim s As String = Server.MapPath("xml\Payments.xml")
                _DataSet.WriteXml(s)

                'Report AFCReport Ended XML

                'Records Checking

                If _DataSet.Tables(0).Rows.Count = 0 Then
                    Response.Write("No Record Found")
                Else

                    'Report Loading 

                    MyReportDocument.Load(Server.MapPath("~/GroupReport/RptPaymentEn.rpt"))

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
