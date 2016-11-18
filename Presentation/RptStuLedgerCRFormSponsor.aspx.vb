Imports System.Data
Imports System.Data.SqlClient
Imports System.Web.Configuration
Imports CrystalDecisions.CrystalReports.Engine

Partial Class RptStuLedgerCRFormSponsor
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

                str = "SELECT     SAS_Sponsor.SASR_Code, SAS_Sponsor.SASR_Name, SAS_Sponsor.SASR_Address, "
                str += " SAS_Sponsor.SASR_Address1, "
                str += " SAS_Sponsor.SASR_Address2, SAS_Sponsor.SASR_Status, SAS_Accounts.TransDate, SAS_Accounts.TransCode, "
                str += " SAS_Accounts.Description, SAS_Accounts.Category, SAS_Accounts.TransAmount "
                str += " FROM SAS_Accounts INNER JOIN "
                str += " SAS_Sponsor ON SAS_Accounts.CreditRef = SAS_Sponsor.SASR_Code "
                str += sortbyvalue

                'str = "SELECT     dbo.SAS_Sponsor.SASR_Code, dbo.SAS_Sponsor.SASR_Name, dbo.SAS_Sponsor.SASR_Address, "
                'str += " dbo.SAS_Sponsor.SASR_Address1, "
                'str += " dbo.SAS_Sponsor.SASR_Address2, dbo.SAS_Sponsor.SASR_Status, dbo.SAS_Accounts.TransDate, dbo.SAS_Accounts.TransCode, "
                'str += " dbo.SAS_Accounts.Description, dbo.SAS_Accounts.Category, dbo.SAS_Accounts.TransAmount "
                'str += " FROM dbo.SAS_Accounts INNER JOIN "
                'str += " dbo.SAS_Sponsor ON dbo.SAS_Accounts.CreditRef = dbo.SAS_Sponsor.SASR_Code "
                'str += sortbyvalue

                Dim totaldebit As String = "SELECT sum(transamount) FROM SAS_ACCOUNTS WHERE TRANSTYPE='Debit'"
                Dim totalcredit As String = "SELECT sum(transamount) FROM SAS_ACCOUNTS WHERE TRANSTYPE='Credit'"
                Dim totalamount As String = "SELECT sum(transamount) FROM SAS_ACCOUNTS"


                'Execute Scalar Starting 

                Session("totaldebit") = _ReportHelper.GetExecuteScalar(totaldebit)
                Session("totalcredit") = _ReportHelper.GetExecuteScalar(totalcredit)
                Session("totalamount") = _ReportHelper.GetExecuteScalar(totalamount)

                Dim totdr As Integer = Session("totaldebit")
                Dim totcr As Integer = Session("totalcredit")
                Dim outstan As Integer = totdr - totcr
                Session("outstanding") = outstan



                'DataSet Strating
                Dim _DataSet As DataSet = _ReportHelper.GetDataSet(str)

                'Report XML Loading

                Dim s As String = Server.MapPath("~/xml/RptStuLegerCRSpons.xml")
                _DataSet.WriteXml(s)

                'Report XML Ended

                'Records Checking

                If _DataSet.Tables(0).Rows.Count = 0 Then
                    Response.Write("No Record Found")

                Else

                    'Report Loading 

                    MyReportDocument.Load(Server.MapPath("RptStuLegerCRSpons.rpt"))
                    'MyReportDocument.Load(Server.MapPath("RptTransactionDetailCR.rpt"))

                    MyReportDocument.SetParameterValue("totaldebit", Session("totaldebit"))
                    MyReportDocument.SetParameterValue("totalcredit", Session("totalcredit"))
                    MyReportDocument.SetParameterValue("totalamount", Session("totalamount"))
                    MyReportDocument.SetParameterValue("outstanding", Session("outstanding"))

                    Dim config As String
                    config = ConfigurationManager.ConnectionStrings("SASConnectionString").ToString()
                    Dim comstring As String() = config.Split(";")
                    Dim username As String = comstring(3)
                    Dim userid As String() = username.Split("=")
                    Dim user As String = userid(1)
                    Dim password As String = comstring(4)
                    Dim passwor As String() = password.Split("=")
                    Dim pass As String = passwor(1)
                    Dim servername As String = comstring(0)
                    Dim servern As String() = servername.Split("=")
                    Dim server1 As String = servern(1)
                    Dim database As String = comstring(1)
                    Dim databas As String() = database.Split("=")
                    Dim data As String = databas(1)
                    MyReportDocument.SetDatabaseLogon(user, pass, server1, data)
                    'MyReportDocument.SetDatabaseLogon("sa", "sa", ".", "SAS")
                    MyReportDocument.VerifyDatabase()

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
