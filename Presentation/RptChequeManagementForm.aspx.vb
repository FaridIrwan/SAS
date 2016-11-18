Imports System.Data
Imports System.Data.SqlClient
Imports System.Web.Configuration
Imports CrystalDecisions.CrystalReports.Engine
Imports CrystalDecisions.Shared

Partial Class RptChequeManagementForm
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

            Dim bat As String = Nothing
            Dim type As String = Nothing
            Dim conv As New NumericCurrencyToWord()
            Dim str As String = Nothing

            If Not IsPostBack Then

                bat = Request.QueryString("bat")
                type = Request.QueryString("type")

                If type = "1" Then

                    str = "SELECT SAS_Accounts.TransAmount as TransAmount, CONVERT(VARCHAR(10),SAS_Cheque.ChequeDate,105) as ChequeDate,  SAS_Student.SASI_Name as Name ,SAS_Cheque.description as words "
                    str += " FROM  SAS_Accounts INNER JOIN SAS_Cheque ON SAS_Accounts.CreditRef1 = SAS_Cheque.PaymentNo INNER JOIN "
                    str += " SAS_Student ON SAS_Accounts.CreditRef = SAS_Student.SASI_MatricNo WHERE SAS_Cheque.ProcessID = '" + bat + "'"

                ElseIf type = "St" Then

                    str = " SELECT SAS_Accounts.TransAmount as TransAmount, CONVERT(VARCHAR(10),SAS_Cheque.ChequeDate,105) as ChequeDate, SAS_Student.SASI_Name as Name, SAS_Cheque.description as words "
                    str += " FROM SAS_Accounts INNER JOIN SAS_Student ON SAS_Accounts.CreditRef = SAS_Student.SASI_MatricNo INNER JOIN "
                    str += " SAS_Cheque ON SAS_Accounts.BatchCode = SAS_Cheque.PaymentNo WHERE SAS_Cheque.ProcessID = '" + bat + "'"

                ElseIf type = "2" Then

                    str = " SELECT SAS_Accounts.TransAmount as TransAmount, CONVERT(VARCHAR(10),SAS_Cheque.ChequeDate,105) as ChequeDate, SAS_Sponsor.SASR_Name as Name,SAS_Cheque.description as words "
                    str += " FROM SAS_Accounts INNER JOIN SAS_Cheque ON SAS_Accounts.TransCode = SAS_Cheque.PaymentNo INNER JOIN "
                    str += " SAS_Sponsor ON SAS_Accounts.CreditRef = SAS_Sponsor.SASR_Code WHERE SAS_Cheque.ProcessID = '" + bat + "'"

                End If

                'Author			: Anil Kumar - T-Melmax Sdn Bhd
                'Created Date	: 20/05/2015

                'DataSet Strating
                Dim _DataSet As DataSet = _ReportHelper.GetDataSet(str)

                Dim row As DataRow

                For Each row In _DataSet.Tables(0).Rows

                    Dim db As Double = row.Item(0)
                    row.Item(3) = conv.Convert(db, "", "Cents")

                Next

                'Report XML Loading

                Dim s As String = Server.MapPath("xml\ChequeReport.xml")
                _DataSet.WriteXml(s)

                'Report XML Ended

                'Records Checking

                If _DataSet.Tables(0).Rows.Count = 0 Then
                    Response.Write("No Record Found")

                Else

                    'Report Loading
                    MyReportDocument.Load(Server.MapPath("RptCheques.rpt"))
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
