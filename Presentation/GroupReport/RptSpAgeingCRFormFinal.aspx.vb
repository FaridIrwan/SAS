Imports System.Data
Imports System.Data.SqlClient
Imports System.Web.Configuration
Imports CrystalDecisions.CrystalReports.Engine


Partial Class RptSpAgeingCRFormFinal
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
        'Modified by Hafiz @ 09/3/2016 - QUATERLY

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


                Dim l31, l61, l91, others As String
                Dim period As String
                Dim x1 As Integer = 1

                If Request.QueryString("periodvalue1") = "" Or Request.QueryString("periodvalue1") Is Nothing Then

                    l31 = 30
                    l61 = 60
                    l91 = 90
                    others = 120

                Else

                    period = Request.QueryString("periodvalue1")
                    Dim str1(3) As String
                    str1 = period.Split(",")
                    l31 = str1(0)
                    l61 = str1(1)
                    l91 = str1(2)

                End If

                Str = "SELECT SAS_Sponsor.SASR_Code,SAS_Accounts.TransAmount,(SAS_Accounts.TransAmount - SAS_Accounts.PaidAmount)as Dueamount,SAS_Accounts.TransCode, SAS_Sponsor.SASR_Name, SAS_Sponsor.SASR_Address,SAS_Sponsor.SASR_Address1, SAS_Sponsor.SASR_Address2,"
                Str += " to_char(Current_date,'DD/MM/YYYY') AS TRANSDATE,to_char(sas_accounts.duedate, 'DD/MM/YYYY') AS DUEDATE,"
                Str += " DATE_PART('day',Current_date - duedate) AS Different_Days, case when DATE_PART('day',Current_date - duedate) <" + l31 + " then (SAS_Accounts.TransAmount - SAS_Accounts.PaidAmount) else '0.00' end as "" < 30 "","
                Str += " case when DATE_PART('day',Current_date - duedate) >" + l31 + " and DATE_PART('day',Current_date - duedate) < " + CStr((CInt(l61) + 1)) + " then (SAS_Accounts.TransAmount - SAS_Accounts.PaidAmount) else '0.00' end as "" < 60 "","
                Str += " case when DATE_PART('day',Current_date - duedate) > " + l61 + "  and DATE_PART('day',Current_date - duedate) < " + CStr((CInt(l91) + 1)) + " then (SAS_Accounts.TransAmount - SAS_Accounts.PaidAmount) else '0.00' end as "" < 90 "","
                Str += " case when DATE_PART('day',Current_date - duedate) > " + l91 + "  then (SAS_Accounts.TransAmount - SAS_Accounts.PaidAmount) else '0.00' end as Others, "
                Str += l31 + " as Header1, " + l61 + "  as Header2, " + l91 + "  as Header3 "
                Str += " FROM  SAS_Accounts INNER JOIN "
                'Str += " SAS_Sponsor ON (SAS_Accounts.CreditRef = SAS_Sponsor.SASR_Code) "
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
                    MyReportDocument.Load(Server.MapPath("~/ProcessReport/RptSpAgeingCRFinal.rpt"))
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
