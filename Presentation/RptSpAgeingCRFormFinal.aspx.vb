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

                Dim firstcol As String = "30"
                Dim secondcol As String = "60"
                Dim thirdcol As String = "90"



                Str = "SELECT SAS_Sponsor.SASR_Code,SAS_Accounts.TransAmount,(SAS_Accounts.TransAmount - SAS_Accounts.PaidAmount)as Dueamount,SAS_Accounts.TransCode, SAS_Sponsor.SASR_Name, SAS_Sponsor.SASR_Address,SAS_Sponsor.SASR_Address1, SAS_Sponsor.SASR_Address2,"
                Str += " CONVERT(VARCHAR(10),getdate(),105) AS TRANSDATE,CONVERT(VARCHAR(10),sas_accounts.duedate,105) AS DUEDATE,"
                Str += "DateDiff(d,duedate,getdate()) AS Different_Days, case when DateDiff(d,duedate,getdate()) <" + l31 + " then (SAS_Accounts.TransAmount - SAS_Accounts.PaidAmount) else '0.00' end as '< " + firstcol + "',"
                Str += " case when DateDiff(d,duedate,getdate()) >" + l31 + " and DateDiff(d,duedate,getdate()) < " + CStr((CInt(l61) + 1)) + " then (SAS_Accounts.TransAmount - SAS_Accounts.PaidAmount) else '0.00' end as '< " + secondcol + "',"
                Str += " case when DateDiff(d,duedate,getdate()) > " + l61 + "  and DateDiff(d,duedate,getdate()) < " + CStr((CInt(l91) + 1)) + " then (SAS_Accounts.TransAmount - SAS_Accounts.PaidAmount) else '0.00' end as '< " + thirdcol + "',"
                Str += " case when DateDiff(d,duedate,getdate()) > " + l91 + "  then (SAS_Accounts.TransAmount - SAS_Accounts.PaidAmount) else '0.00' end as Others, "
                Str += l31 + " as Header1, " + l61 + "  as Header2, " + l91 + "  as Header3 "
                Str += " FROM  SAS_Accounts INNER JOIN "
                'Str += " SAS_Sponsor ON (SAS_Accounts.CreditRef = SAS_Sponsor.SASR_Code) "
                Str += " SAS_Sponsor ON SAS_Accounts.CreditRef = SAS_Sponsor.SASR_Code "
                Str += " where Transstatus='Open' and poststatus='Posted' and  Transtype='Credit' and subtype='Sponsor' "
                Str += " and  SAS_Sponsor.SASR_Code like '" + sponsor + "' "

                'Str = "SELECT dbo.SAS_Sponsor.SASR_Code,dbo.SAS_Accounts.TransAmount,(dbo.SAS_Accounts.TransAmount - dbo.SAS_Accounts.PaidAmount)as Dueamount,dbo.SAS_Accounts.TransCode, dbo.SAS_Sponsor.SASR_Name, dbo.SAS_Sponsor.SASR_Address,dbo.SAS_Sponsor.SASR_Address1, dbo.SAS_Sponsor.SASR_Address2,"
                'Str += " CONVERT(VARCHAR(10),getdate(),105) AS TRANSDATE,CONVERT(VARCHAR(10),sas_accounts.duedate,105) AS DUEDATE,"
                'Str += "DateDiff(d,duedate,getdate()) AS Different_Days, case when DateDiff(d,duedate,getdate()) <" + l31 + " then (dbo.SAS_Accounts.TransAmount - dbo.SAS_Accounts.PaidAmount) else '0.00' end as '< " + firstcol + "',"
                'Str += " case when DateDiff(d,duedate,getdate()) >" + l31 + " and DateDiff(d,duedate,getdate()) < " + CStr((CInt(l61) + 1)) + " then (dbo.SAS_Accounts.TransAmount - dbo.SAS_Accounts.PaidAmount) else '0.00' end as '< " + secondcol + "',"
                'Str += " case when DateDiff(d,duedate,getdate()) > " + l61 + "  and DateDiff(d,duedate,getdate()) < " + CStr((CInt(l91) + 1)) + " then (dbo.SAS_Accounts.TransAmount - dbo.SAS_Accounts.PaidAmount) else '0.00' end as '< " + thirdcol + "',"
                'Str += " case when DateDiff(d,duedate,getdate()) > " + l91 + "  then (dbo.SAS_Accounts.TransAmount - dbo.SAS_Accounts.PaidAmount) else '0.00' end as Others, "
                'Str += l31 + " as Header1, " + l61 + "  as Header2, " + l91 + "  as Header3 "
                'Str += " FROM  dbo.SAS_Accounts INNER JOIN "
                ''Str += " dbo.SAS_Sponsor ON (dbo.SAS_Accounts.CreditRef = dbo.SAS_Sponsor.SASR_Code) "
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

                    MyReportDocument.Load(Server.MapPath("RptSpAgeingCRFinal.rpt"))
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
