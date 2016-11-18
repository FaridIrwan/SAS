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

    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init

        'Author			: Anil Kumar - T-Melmax Sdn Bhd
        'Created Date	: 20/05/2015
        'Modified by Hafiz @ 09/3/2016 - MONTHLY

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
                Str += " to_char(sas_accounts.transdate,'DD/MM/YYYY') AS TRANSDATE,"
                Str += " to_char(sas_accounts.duedate, 'DD/MM/YYYY') AS DUEDATE,"

                Str += " case when DATE_PART('year',CURRENT_DATE) - DATE_PART('year',duedate) >=1 Then"
                Str += " ((DATE_PART('year',CURRENT_DATE) - DATE_PART('year',duedate)) * 12) + (DATE_PART('month',Current_date) - DATE_PART('month',duedate)) "
                Str += " Else"
                Str += " DATE_PART('month',Current_date) - DATE_PART('month',duedate)"
                Str += " END AS Different_month, "

                Str += " case when DATE_PART('month',Current_date) - DATE_PART('month',duedate) < -1 then (SAS_Accounts.TransAmount - SAS_Accounts.PaidAmount) else '0.00' end as ""PreviousMonth"","
                Str += " case when DATE_PART('month',Current_date) - DATE_PART('month',duedate) >= 1 and DATE_PART('month',Current_date) - DATE_PART('month',duedate) < 2 then (SAS_Accounts.TransAmount - SAS_Accounts.PaidAmount) else '0.00' end as ""Month1"","
                Str += " case when DATE_PART('month',Current_date) - DATE_PART('month',duedate) >= 2 and DATE_PART('month',Current_date) - DATE_PART('month',duedate) < 3 then (SAS_Accounts.TransAmount - SAS_Accounts.PaidAmount) else '0.00' end as ""Month2"","
                Str += " case when DATE_PART('month',Current_date) - DATE_PART('month',duedate) >= 3 and DATE_PART('month',Current_date) - DATE_PART('month',duedate) < 4 then (SAS_Accounts.TransAmount - SAS_Accounts.PaidAmount) else '0.00' end as ""Month3"","
                Str += " case when DATE_PART('month',Current_date) - DATE_PART('month',duedate) >= 4 and DATE_PART('month',Current_date) - DATE_PART('month',duedate) < 5 then (SAS_Accounts.TransAmount - SAS_Accounts.PaidAmount) else '0.00' end as ""Month4"","
                Str += " case when DATE_PART('month',Current_date) - DATE_PART('month',duedate) >= 5 and DATE_PART('month',Current_date) - DATE_PART('month',duedate) < 6 then (SAS_Accounts.TransAmount - SAS_Accounts.PaidAmount) else '0.00' end as ""Month5"","
                Str += " case when DATE_PART('month',Current_date) - DATE_PART('month',duedate) >= 6 and DATE_PART('month',Current_date) - DATE_PART('month',duedate) < 7 then (SAS_Accounts.TransAmount - SAS_Accounts.PaidAmount) else '0.00' end as ""Month6"","
                Str += " case when DATE_PART('month',Current_date) - DATE_PART('month',duedate) >= 7 and DATE_PART('month',Current_date) - DATE_PART('month',duedate) < 8 then (SAS_Accounts.TransAmount - SAS_Accounts.PaidAmount) else '0.00' end as ""Month7"","
                Str += " case when DATE_PART('month',Current_date) - DATE_PART('month',duedate) >= 8 and DATE_PART('month',Current_date) - DATE_PART('month',duedate) < 9 then (SAS_Accounts.TransAmount - SAS_Accounts.PaidAmount) else '0.00' end as ""Month8"","
                Str += " case when DATE_PART('month',Current_date) - DATE_PART('month',duedate) >= 9 and DATE_PART('month',Current_date) - DATE_PART('month',duedate) < 10 then (SAS_Accounts.TransAmount - SAS_Accounts.PaidAmount) else '0.00' end as ""Month9"","
                Str += " case when DATE_PART('month',Current_date) - DATE_PART('month',duedate) >= 10 and DATE_PART('month',Current_date) - DATE_PART('month',duedate) < 11 then (SAS_Accounts.TransAmount - SAS_Accounts.PaidAmount) else '0.00' end as ""Month10"","
                Str += " case when DATE_PART('month',Current_date) - DATE_PART('month',duedate) >= 11 and DATE_PART('month',Current_date) - DATE_PART('month',duedate) < 12 then (SAS_Accounts.TransAmount - SAS_Accounts.PaidAmount) else '0.00' end as ""Month11"","

                Str += " case when"
                Str += " DATE_PART('year',CURRENT_DATE) - DATE_PART('year',duedate) >=1 Then"
                Str += " (SAS_Accounts.TransAmount - SAS_Accounts.PaidAmount) "
                Str += " Else"
                Str += " case when DATE_PART('month',Current_date) - DATE_PART('month',duedate) >= 12 then "
                Str += " (SAS_Accounts.TransAmount - SAS_Accounts.PaidAmount) "
                Str += " Else '0.00' end"
                Str += " end as ""Others"" "

                Str += " FROM  SAS_Accounts INNER JOIN "
                Str += " SAS_Sponsor ON SAS_Accounts.CreditRef = SAS_Sponsor.SASR_Code "
                Str += " where poststatus='Posted' and  Transtype='Credit' and subtype='Sponsor' "
                Str += " and  SAS_Sponsor.SASR_Code like '" + sponsor + "' "

                'Author			: Anil Kumar - T-Melmax Sdn Bhd
                'Created Date	: 20/05/2015

                'DataSet Strating
                Dim _DataSet As DataSet = _ReportHelper.GetDataSet(Str)
                _DataSet.Tables(0).TableName = "Table"

                'Report XML Loading
                Dim s As String = Server.MapPath("~/xml/SponsorAgeing.xml")
                _DataSet.WriteXml(s)
                'Report XML Ended

                'Records Checking
                If _DataSet.Tables(0).Rows.Count = 0 Then
                    Response.Write("No Record Found")

                Else
                    'Report Loading
                    MyReportDocument.Load(Server.MapPath("~/ProcessReport/RptSpAgeingCRMonthFinal.rpt"))
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
