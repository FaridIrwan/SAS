Imports System.Data
Imports System.Data.SqlClient
Imports System.Web.Configuration
Imports CrystalDecisions.CrystalReports.Engine
Partial Class RptSpAgeingMonthCRFormFinal
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
                Dim sponsor As String = Nothing
                Dim Str As String = Nothing

                If Session("sponsor") Is Nothing Then

                    sponsor = "%"

                Else

                    sponsor = Session("sponsor")

                End If

                Str = "SELECT SAS_Sponsor.SASR_Code,SAS_Accounts.TransAmount,SAS_Accounts.Transtype,"
                Str += " SAS_Accounts.TransCode, SAS_Sponsor.SASR_Name, SAS_Sponsor.SASR_Address, SAS_Sponsor.SASR_Address1, SAS_Sponsor.SASR_Address2, "
                Str += " (SAS_Accounts.TransAmount - SAS_Accounts.PaidAmount)as Dueamount,"
                Str += " CONVERT(VARCHAR(10),sas_accounts.transdate,105) AS TRANSDATE,"
                Str += " CONVERT(VARCHAR(10),sas_accounts.duedate,105) AS DUEDATE,"
                Str += " DateDiff(mm,duedate,getdate()) AS Different_month, "
                Str += " case when DateDiff(mm,duedate,getdate()) < -1 then (SAS_Accounts.TransAmount - SAS_Accounts.PaidAmount) else '0.00' end as 'PreviousMonth',"
                Str += " case when DateDiff(mm,duedate,getdate()) > 1 and DateDiff(d,duedate,getdate()) < 2 then (SAS_Accounts.TransAmount - SAS_Accounts.PaidAmount) else '0.00' end as 'Month1',"
                Str += " case when DateDiff(mm,duedate,getdate()) > 2 and DateDiff(d,duedate,getdate()) < 3 then (SAS_Accounts.TransAmount - SAS_Accounts.PaidAmount) else '0.00' end as 'Month2',"
                Str += " case when DateDiff(mm,duedate,getdate()) > 3 and DateDiff(d,duedate,getdate()) < 4 then (SAS_Accounts.TransAmount - SAS_Accounts.PaidAmount) else '0.00' end as 'Month3',"
                Str += " case when DateDiff(mm,duedate,getdate()) > 4 and DateDiff(d,duedate,getdate()) < 5 then (SAS_Accounts.TransAmount - SAS_Accounts.PaidAmount) else '0.00' end as 'Month4',"
                Str += " case when DateDiff(mm,duedate,getdate()) > 5 and DateDiff(d,duedate,getdate()) < 6 then (SAS_Accounts.TransAmount - SAS_Accounts.PaidAmount) else '0.00' end as 'Month5',"
                Str += " case when DateDiff(mm,duedate,getdate()) > 6 and DateDiff(d,duedate,getdate()) < 7 then (SAS_Accounts.TransAmount - SAS_Accounts.PaidAmount) else '0.00' end as 'Month6',"
                Str += " case when DateDiff(mm,duedate,getdate()) > 7 and DateDiff(d,duedate,getdate()) < 8 then (SAS_Accounts.TransAmount - SAS_Accounts.PaidAmount) else '0.00' end as 'Month7',"
                Str += " case when DateDiff(mm,duedate,getdate()) > 8 and DateDiff(d,duedate,getdate()) < 9 then (SAS_Accounts.TransAmount - SAS_Accounts.PaidAmount) else '0.00' end as 'Month8',"
                Str += " case when DateDiff(mm,duedate,getdate()) > 9 and DateDiff(d,duedate,getdate()) < 10 then (SAS_Accounts.TransAmount - SAS_Accounts.PaidAmount) else '0.00' end as 'Month9',"
                Str += " case when DateDiff(mm,duedate,getdate()) > 10 and DateDiff(d,duedate,getdate()) < 11 then (SAS_Accounts.TransAmount - SAS_Accounts.PaidAmount) else '0.00' end as 'Month10',"
                Str += " case when DateDiff(mm,duedate,getdate()) > 11 and DateDiff(d,duedate,getdate()) < 12 then (SAS_Accounts.TransAmount - SAS_Accounts.PaidAmount) else '0.00' end as 'Month11',"
                Str += " case when DateDiff(mm,duedate,getdate()) > 12 then (SAS_Accounts.TransAmount - SAS_Accounts.PaidAmount) else '0.00' end as Others"
                Str += " FROM  SAS_Accounts INNER JOIN "
                Str += " SAS_Sponsor ON SAS_Accounts.CreditRef = SAS_Sponsor.SASR_Code "
                Str += " where Transstatus='Open' and poststatus='Posted' and  Transtype='Credit' and subtype='Sponsor' "
                Str += " and  SAS_Sponsor.SASR_Code like '" + sponsor + "' "

                'Str = "SELECT dbo.SAS_Sponsor.SASR_Code,dbo.SAS_Accounts.TransAmount,dbo.SAS_Accounts.Transtype,"
                'Str += " dbo.SAS_Accounts.TransCode, dbo.SAS_Sponsor.SASR_Name, dbo.SAS_Sponsor.SASR_Address, dbo.SAS_Sponsor.SASR_Address1, dbo.SAS_Sponsor.SASR_Address2, "
                'Str += " (dbo.SAS_Accounts.TransAmount - dbo.SAS_Accounts.PaidAmount)as Dueamount,"
                'Str += " CONVERT(VARCHAR(10),sas_accounts.transdate,105) AS TRANSDATE,"
                'Str += " CONVERT(VARCHAR(10),sas_accounts.duedate,105) AS DUEDATE,"
                'Str += " DateDiff(mm,duedate,getdate()) AS Different_month, "
                'Str += " case when DateDiff(mm,duedate,getdate()) < -1 then (dbo.SAS_Accounts.TransAmount - dbo.SAS_Accounts.PaidAmount) else '0.00' end as 'PreviousMonth',"
                'Str += " case when DateDiff(mm,duedate,getdate()) > 1 and DateDiff(d,duedate,getdate()) < 2 then (dbo.SAS_Accounts.TransAmount - dbo.SAS_Accounts.PaidAmount) else '0.00' end as 'Month1',"
                'Str += " case when DateDiff(mm,duedate,getdate()) > 2 and DateDiff(d,duedate,getdate()) < 3 then (dbo.SAS_Accounts.TransAmount - dbo.SAS_Accounts.PaidAmount) else '0.00' end as 'Month2',"
                'Str += " case when DateDiff(mm,duedate,getdate()) > 3 and DateDiff(d,duedate,getdate()) < 4 then (dbo.SAS_Accounts.TransAmount - dbo.SAS_Accounts.PaidAmount) else '0.00' end as 'Month3',"
                'Str += " case when DateDiff(mm,duedate,getdate()) > 4 and DateDiff(d,duedate,getdate()) < 5 then (dbo.SAS_Accounts.TransAmount - dbo.SAS_Accounts.PaidAmount) else '0.00' end as 'Month4',"
                'Str += " case when DateDiff(mm,duedate,getdate()) > 5 and DateDiff(d,duedate,getdate()) < 6 then (dbo.SAS_Accounts.TransAmount - dbo.SAS_Accounts.PaidAmount) else '0.00' end as 'Month5',"
                'Str += " case when DateDiff(mm,duedate,getdate()) > 6 and DateDiff(d,duedate,getdate()) < 7 then (dbo.SAS_Accounts.TransAmount - dbo.SAS_Accounts.PaidAmount) else '0.00' end as 'Month6',"
                'Str += " case when DateDiff(mm,duedate,getdate()) > 7 and DateDiff(d,duedate,getdate()) < 8 then (dbo.SAS_Accounts.TransAmount - dbo.SAS_Accounts.PaidAmount) else '0.00' end as 'Month7',"
                'Str += " case when DateDiff(mm,duedate,getdate()) > 8 and DateDiff(d,duedate,getdate()) < 9 then (dbo.SAS_Accounts.TransAmount - dbo.SAS_Accounts.PaidAmount) else '0.00' end as 'Month8',"
                'Str += " case when DateDiff(mm,duedate,getdate()) > 9 and DateDiff(d,duedate,getdate()) < 10 then (dbo.SAS_Accounts.TransAmount - dbo.SAS_Accounts.PaidAmount) else '0.00' end as 'Month9',"
                'Str += " case when DateDiff(mm,duedate,getdate()) > 10 and DateDiff(d,duedate,getdate()) < 11 then (dbo.SAS_Accounts.TransAmount - dbo.SAS_Accounts.PaidAmount) else '0.00' end as 'Month10',"
                'Str += " case when DateDiff(mm,duedate,getdate()) > 11 and DateDiff(d,duedate,getdate()) < 12 then (dbo.SAS_Accounts.TransAmount - dbo.SAS_Accounts.PaidAmount) else '0.00' end as 'Month11',"
                'Str += " case when DateDiff(mm,duedate,getdate()) > 12 then (dbo.SAS_Accounts.TransAmount - dbo.SAS_Accounts.PaidAmount) else '0.00' end as Others"
                'Str += " FROM  dbo.SAS_Accounts INNER JOIN "
                'Str += " dbo.SAS_Sponsor ON dbo.SAS_Accounts.CreditRef = dbo.SAS_Sponsor.SASR_Code "
                'Str += " where Transstatus='Open' and poststatus='Posted' and  Transtype='Credit' and subtype='Sponsor' "
                'Str += " and  dbo.SAS_Sponsor.SASR_Code like '" + sponsor + "' "

                'Author			: Anil Kumar - T-Melmax Sdn Bhd
                'Created Date	: 20/05/2015

                'DataSet Strating
                Dim _DataSet As DataSet = _ReportHelper.GetDataSet(Str)

                'Report XML Loading

                Dim s As String = Server.MapPath("xml\SponsorAgeing.xml")
                _DataSet.WriteXml(s)

                'Report XML Ended

                'Records Checking

                If _DataSet.Tables(0).Rows.Count = 0 Then
                    Response.Write("No Record Found")

                Else

                    'Report Loading

                    MyReportDocument.Load(Server.MapPath("RptSpAgeingCRMonthFinal.rpt"))
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
