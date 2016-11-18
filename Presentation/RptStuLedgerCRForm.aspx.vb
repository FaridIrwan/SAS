Imports System.Data
Imports System.Data.SqlClient
Imports System.Web.Configuration
Imports CrystalDecisions.CrystalReports.Engine

Partial Class RptStuLedgerCRForm
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

                Dim sortbyvalue As String = Nothing
                Dim str As String = Nothing

                If Session("sortby") = Nothing Then

                    Session("sortby") = "NoSort"
                    sortbyvalue = ""

                Else

                    sortbyvalue = " Order By " + Session("sortby")

                End If

                str = " SELECT SAS_Student.SASI_Name, SAS_Student.SASI_PgId, "
                str += " SAS_Student.SASI_Passport, SAS_Student.SASS_Code, "
                str += " SAS_Student.SASI_Add1, SAS_Student.SASI_Add2, SAS_Student.SASI_Add3, SAS_Student.SASI_StatusRec, "
                str += " SAS_Accounts.TransDate, SAS_Accounts.TransCode, SAS_Accounts.Description, SAS_Accounts.Category, "
                str += " SAS_Accounts.TransAmount"
                str += " FROM SAS_Accounts INNER JOIN "
                str += " SAS_Student ON SAS_Accounts.CreditRef = SAS_Student.SASI_MatricNo "
                str += sortbyvalue

                'str = " SELECT dbo.SAS_Student.SASI_Name, dbo.SAS_Student.SASI_PgId, "
                'str += " dbo.SAS_Student.SASI_Passport, dbo.SAS_Student.SASS_Code, "
                'str += " dbo.SAS_Student.SASI_Add1, dbo.SAS_Student.SASI_Add2, dbo.SAS_Student.SASI_Add3, dbo.SAS_Student.SASI_StatusRec, "
                'str += " dbo.SAS_Accounts.TransDate, dbo.SAS_Accounts.TransCode, dbo.SAS_Accounts.Description, dbo.SAS_Accounts.Category, "
                'str += " dbo.SAS_Accounts.TransAmount"
                'str += " FROM dbo.SAS_Accounts INNER JOIN "
                'str += " dbo.SAS_Student ON dbo.SAS_Accounts.CreditRef = dbo.SAS_Student.SASI_MatricNo "
                'str += sortbyvalue


                Dim totaldebit As String = "SELECT sum(transamount) FROM SAS_ACCOUNTS WHERE TRANSTYPE='Debit'"
                Dim totalcredit As String = "SELECT sum(transamount) FROM SAS_ACCOUNTS WHERE TRANSTYPE='Credit'"
                Dim totalamount As String = "SELECT sum(transamount) FROM SAS_ACCOUNTS"

                Session("totaldebit") = _ReportHelper.GetExecuteScalar(totaldebit)
                Session("totalcredit") = _ReportHelper.GetExecuteScalar(totalcredit)
                Session("totalamount") = _ReportHelper.GetExecuteScalar(totalamount)


                Dim totdr As Integer = Session("totaldebit")
                Dim totcr As Integer = Session("totalcredit")
                Dim outstan As Integer = totdr - totcr
                Session("outstanding") = outstan


                'Author			: Anil Kumar - T-Melmax Sdn Bhd
                'Created Date	: 20/05/2015

                'DataSet Strating
                Dim _DataSet As DataSet = _ReportHelper.GetDataSet(str)

                'Report XML Loading
                Dim s As String = Server.MapPath("~/xml/RptStuLedgerCR.xml")
                _DataSet.WriteXml(s)

                'Report XML Ended

                'Records Checking

                If _DataSet.Tables(0).Rows.Count = 0 Then
                    Response.Write("No Record Found")
                
                Else

                    'Report Loading 
                    MyReportDocument.Load(Server.MapPath("~\ProcessReport\RptStuLedgerCR.rpt"))
                    'MyReportDocument.Load(Server.MapPath("RptTransactionDetailCR.rpt"))
                    MyReportDocument.SetParameterValue("totaldebit", Session("totaldebit"))
                    MyReportDocument.SetParameterValue("totalcredit", Session("totalcredit"))
                    MyReportDocument.SetParameterValue("totalamount", Session("totalamount"))
                    MyReportDocument.SetParameterValue("outstanding", Session("outstanding"))
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
