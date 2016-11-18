Imports System.Data
Imports System.Data.SqlClient
Imports System.Web.Configuration
Imports CrystalDecisions.CrystalReports.Engine

Partial Class RptSponsorAgeingViewer
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

                Dim sponsor As String = Nothing
                Dim sortbyvalue As String = Nothing
                Dim Str As String = Nothing

                If Session("sponsor") Is Nothing Then

                    sponsor = "%"

                Else
                    sponsor = Session("sponsor")

                End If

                Str = "SELECT SAS_Sponsor.SASR_Code, SAS_Accounts.TransAmount,SAS_Accounts.TransCode,to_char(Current_date,'DD/MM/YYYY') AS TRANSDATE,"
                Str += " SAS_Sponsor.SASR_Name, SAS_Sponsor.SASR_Address,SAS_Sponsor.SASR_Address1, SAS_Sponsor.SASR_Address2,SAS_Accounts.TransCode,"
                Str += "to_char(sas_accounts.duedate, 'DD/MM/YYYY') AS DUEDATE,"
                Str += " case when DueDate > (CURRENT_DATE) then (SAS_Accounts.TransAmount - SAS_Accounts.PaidAmount)"
                Str += " Else '0.00' end as  Year1,"
                Str += " case when DueDate > (CURRENT_DATE - INTERVAL '1 year') then (SAS_Accounts.TransAmount - SAS_Accounts.PaidAmount)"
                Str += " Else '0.00' end as Year2,"
                Str += " case when DueDate > (CURRENT_DATE - INTERVAL '2 year') then (SAS_Accounts.TransAmount - SAS_Accounts.PaidAmount)"
                Str += " Else '0.00' end as Year3,"
                Str += " case when DueDate > (CURRENT_DATE - INTERVAL '3 year') then (SAS_Accounts.TransAmount - SAS_Accounts.PaidAmount)"
                Str += " Else '0.00' end as Year4,"
                Str += " case when DueDate > (CURRENT_DATE - INTERVAL '4 year') then (SAS_Accounts.TransAmount - SAS_Accounts.PaidAmount)"
                Str += "  Else '0.00' end as Year5,"
                Str += " case when DueDate > (CURRENT_DATE - INTERVAL '5 year') then (SAS_Accounts.TransAmount - SAS_Accounts.PaidAmount)"
                Str += "  Else '0.00' end as Year6"
                Str += " From SAS_Accounts INNER JOIN"
                Str += " SAS_Sponsor ON SAS_Accounts.CreditRef = SAS_Sponsor.SASR_Code"
                Str += " WHERE Transstatus='Open' and poststatus='Posted' and  Transtype='Credit' and subtype='Sponsor'"
                Str += " and  SAS_Sponsor.SASR_Code like '" + sponsor + "' "
                'Str = "SELECT dbo.SAS_Sponsor.SASR_Code, dbo.SAS_Accounts.TransAmount,dbo.SAS_Accounts.TransCode,CONVERT(VARCHAR(10),getdate(),105) AS TRANSDATE,"
                'Str += " dbo.SAS_Sponsor.SASR_Name, dbo.SAS_Sponsor.SASR_Address,dbo.SAS_Sponsor.SASR_Address1, dbo.SAS_Sponsor.SASR_Address2,dbo.SAS_Accounts.TransCode,"
                'Str += " CONVERT(VARCHAR(10),sas_accounts.duedate,105) AS DUEDATE,"
                'Str += " case when DueDate > DATENAME(year,GETDATE()) then (dbo.SAS_Accounts.TransAmount - dbo.SAS_Accounts.PaidAmount)"
                'Str += " Else '0.00' end as  'Year1',"
                'Str += " case when DueDate > DATEADD(year,-1,GETDATE()) then (dbo.SAS_Accounts.TransAmount - dbo.SAS_Accounts.PaidAmount)"
                'Str += " Else '0.00' end as 'Year2',"
                'Str += " case when DueDate > DATEADD(year,-2,GETDATE()) then (dbo.SAS_Accounts.TransAmount - dbo.SAS_Accounts.PaidAmount)"
                'Str += " Else '0.00' end as 'Year3',"
                'Str += " case when DueDate > DATEADD(year,-3,GETDATE()) then (dbo.SAS_Accounts.TransAmount - dbo.SAS_Accounts.PaidAmount)"
                'Str += " Else '0.00' end as 'Year4',"
                'Str += " case when DueDate > DATEADD(year,-4,GETDATE()) then (dbo.SAS_Accounts.TransAmount - dbo.SAS_Accounts.PaidAmount)"
                'Str += "  Else '0.00' end as 'Year5',"
                'Str += " case when DueDate > DATEADD(year,-5,GETDATE()) then (dbo.SAS_Accounts.TransAmount - dbo.SAS_Accounts.PaidAmount)"
                'Str += "  Else '0.00' end as 'Year6'"
                'Str += " From dbo.SAS_Accounts INNER JOIN"
                'Str += " dbo.SAS_Sponsor ON dbo.SAS_Accounts.CreditRef = dbo.SAS_Sponsor.SASR_Code"
                'Str += " WHERE Transstatus='Open' and poststatus='Posted' and  Transtype='Credit' and subtype='Sponsor'"
                'Str += " and  dbo.SAS_Sponsor.SASR_Code like '" + sponsor + "' "

                'Author			: Anil Kumar - T-Melmax Sdn Bhd
                'Created Date	: 20/05/2015

                'DataSet Strating
                Dim _DataSet As DataSet = _ReportHelper.GetDataSet(Str)

                'Report XML Loading
                Dim s As String = Server.MapPath("~/xml/SponsorAgeingDate.xml")
                _DataSet.WriteXml(s)

                'Report XML Ended

                'Records Checking

                If _DataSet.Tables(0).Rows.Count = 0 Then
                    Response.Write("No Record Found")

                Else

                    'Report Loading 

                    MyReportDocument.Load(Server.MapPath("~/GroupReport/RptSponsorAgeing.rpt"))
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


