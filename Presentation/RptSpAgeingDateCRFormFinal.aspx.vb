Imports System.Data
Imports System.Data.SqlClient
Imports System.Web.Configuration
Imports CrystalDecisions.CrystalReports.Engine

Partial Class RptSpAgeingDateCRFormFinal
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
                Dim datecondition As String = Nothing
                Dim sortbyvalue As String = Nothing
                Dim Str As String = Nothing
                Dim l31, l61, l91, others As String
                Dim period As String = Nothing
                Dim s1 As String = Nothing
                Dim l As Integer = 0
                Dim i As Integer = 0
                Dim x1 As Integer = 1

                If Session("sponsor") Is Nothing Then

                    sponsor = "%"

                Else

                    sponsor = Session("sponsor")

                End If






                If Request.QueryString("periodvalue1") = "" Or Request.QueryString("periodvalue1") Is Nothing Then

                    l31 = 30
                    l61 = 60
                    l91 = 90
                    others = 100

                Else

                    period = Request.QueryString("periodvalue1")
                    Dim str1(3) As String
                    str1 = period.Split(",")
                    l31 = str1(0)
                    l61 = str1(1)
                    l91 = str1(2)

                End If



                If Request.QueryString("fdate") = "0" Or Request.QueryString("tdate") = "0" Or Request.QueryString("fdate") Is Nothing Or Request.QueryString("tdate") Is Nothing Then

                    datecondition = ""

                Else

                    Dim datef As String = Request.QueryString("fdate")
                    Dim datet As String = Request.QueryString("tdate")
                    Dim datelen As Integer = Len(datef)
                    Dim d1, m1, y1, d2, m2, y2 As String
                    d1 = Mid(datef, 1, 2)
                    m1 = Mid(datef, 4, 2)
                    y1 = Mid(datef, 7, 4)
                    Dim datefrom As String = y1 + "/" + m1 + "/" + d1
                    d2 = Mid(datet, 1, 2)
                    m2 = Mid(datet, 4, 2)
                    y2 = Mid(datet, 7, 4)
                    Dim dateto As String = y2 + "/" + m2 + "/" + d2

                    datecondition = " and dbo.SAS_Accounts.DueDate between '" + datefrom + "' and '" + dateto + "'"

                End If

                Dim fdate As String
                Dim tdate As String

                fdate = Request.QueryString("fdate")
                tdate = Request.QueryString("tdate")

                Str = "SELECT SAS_Sponsor.SASR_Code,SAS_Accounts.TransAmount,SAS_Accounts.Transtype,"
                Str += "SAS_Accounts.TransCode, SAS_Sponsor.SASR_Name, SAS_Sponsor.SASR_Address, SAS_Sponsor.SASR_Address1, SAS_Sponsor.SASR_Address2, "
                Str += "(SAS_Accounts.TransAmount - SAS_Accounts.PaidAmount)as Dueamount, "
                Str += "CONVERT(VARCHAR(10),sas_accounts.transdate,105) AS TRANSDATE, "
                Str += "CONVERT(VARCHAR(10),sas_accounts.duedate,105) AS DUEDATE, "
                Str += "DateDiff(dd,duedate,getdate()) AS Different_day, "
                Str += " '" + fdate + "' as datefrom, "
                Str += " '" + tdate + "' as dateto, "
                Str += "case when DateDiff(dd,duedate,getdate()) < -1 then (SAS_Accounts.TransAmount - SAS_Accounts.PaidAmount) else '0.00' end as 'PreDay',"
                Str += "case when DateDiff(dd,duedate,getdate()) > 1 and DateDiff(d,duedate,getdate()) < 2 then (SAS_Accounts.TransAmount - SAS_Accounts.PaidAmount) else '0.00' end as 'Day1',"
                Str += "case when DateDiff(dd,duedate,getdate()) > 2 and DateDiff(d,duedate,getdate()) < 3 then (SAS_Accounts.TransAmount - SAS_Accounts.PaidAmount) else '0.00' end as 'Day2',"
                Str += "case when DateDiff(dd,duedate,getdate()) > 3 and DateDiff(d,duedate,getdate()) < 4 then (SAS_Accounts.TransAmount - SAS_Accounts.PaidAmount) else '0.00' end as 'Day3',"
                Str += "case when DateDiff(dd,duedate,getdate()) > 4 and DateDiff(d,duedate,getdate()) < 5 then (SAS_Accounts.TransAmount - SAS_Accounts.PaidAmount) else '0.00' end as 'Day4',"
                Str += "case when DateDiff(dd,duedate,getdate()) > 5 and DateDiff(d,duedate,getdate()) < 6 then (SAS_Accounts.TransAmount - SAS_Accounts.PaidAmount) else '0.00' end as 'Day5',"
                Str += "case when DateDiff(dd,duedate,getdate()) > 6 and DateDiff(d,duedate,getdate()) < 7 then (SAS_Accounts.TransAmount - SAS_Accounts.PaidAmount) else '0.00' end as 'Day6',"
                Str += "case when DateDiff(dd,duedate,getdate()) > 7 and DateDiff(d,duedate,getdate()) < 8 then (SAS_Accounts.TransAmount - SAS_Accounts.PaidAmount) else '0.00' end as 'Day7',"
                Str += "case when DateDiff(dd,duedate,getdate()) > 8 and DateDiff(d,duedate,getdate()) < 9 then (SAS_Accounts.TransAmount - SAS_Accounts.PaidAmount) else '0.00' end as 'Day8',"
                Str += "case when DateDiff(dd,duedate,getdate()) > 9 and DateDiff(d,duedate,getdate()) < 10 then (SAS_Accounts.TransAmount - SAS_Accounts.PaidAmount) else '0.00' end as 'Day9',"
                Str += "case when DateDiff(dd,duedate,getdate()) > 10 and DateDiff(d,duedate,getdate()) < 11 then (SAS_Accounts.TransAmount - SAS_Accounts.PaidAmount) else '0.00' end as 'Day10',"
                Str += "case when DateDiff(dd,duedate,getdate()) > 11 and DateDiff(d,duedate,getdate()) < 12 then (SAS_Accounts.TransAmount - SAS_Accounts.PaidAmount) else '0.00' end as 'Day11',"
                Str += "case when DateDiff(dd,duedate,getdate()) > 12 and DateDiff(d,duedate,getdate()) < 13 then (SAS_Accounts.TransAmount - SAS_Accounts.PaidAmount) else '0.00' end as 'Day12',"
                Str += "case when DateDiff(dd,duedate,getdate()) > 13 and DateDiff(d,duedate,getdate()) < 14 then (SAS_Accounts.TransAmount - SAS_Accounts.PaidAmount) else '0.00' end as 'Day13',"
                Str += "case when DateDiff(dd,duedate,getdate()) > 14 and DateDiff(d,duedate,getdate()) < 15 then (SAS_Accounts.TransAmount - SAS_Accounts.PaidAmount) else '0.00' end as 'Day14',"
                Str += "case when DateDiff(dd,duedate,getdate()) > 15 and DateDiff(d,duedate,getdate()) < 16 then (SAS_Accounts.TransAmount - SAS_Accounts.PaidAmount) else '0.00' end as 'Day15',"
                Str += "case when DateDiff(dd,duedate,getdate()) > 16 and DateDiff(d,duedate,getdate()) < 17 then (SAS_Accounts.TransAmount - SAS_Accounts.PaidAmount) else '0.00' end as 'Day16',"
                Str += "case when DateDiff(dd,duedate,getdate()) > 17 and DateDiff(d,duedate,getdate()) < 18 then (SAS_Accounts.TransAmount - SAS_Accounts.PaidAmount) else '0.00' end as 'Day17',"
                Str += "case when DateDiff(dd,duedate,getdate()) > 18 and DateDiff(d,duedate,getdate()) < 19 then (SAS_Accounts.TransAmount - SAS_Accounts.PaidAmount) else '0.00' end as 'Day18',"
                Str += "case when DateDiff(dd,duedate,getdate()) > 19 and DateDiff(d,duedate,getdate()) < 20 then (SAS_Accounts.TransAmount - SAS_Accounts.PaidAmount) else '0.00' end as 'Day19',"
                Str += "case when DateDiff(dd,duedate,getdate()) > 20 and DateDiff(d,duedate,getdate()) < 21 then (SAS_Accounts.TransAmount - SAS_Accounts.PaidAmount) else '0.00' end as 'Day20',"
                Str += "case when DateDiff(dd,duedate,getdate()) > 21 and DateDiff(d,duedate,getdate()) < 22 then (SAS_Accounts.TransAmount - SAS_Accounts.PaidAmount) else '0.00' end as 'Day21',"
                Str += "case when DateDiff(dd,duedate,getdate()) > 22 and DateDiff(d,duedate,getdate()) < 23 then (SAS_Accounts.TransAmount - SAS_Accounts.PaidAmount) else '0.00' end as 'Day22',"
                Str += "case when DateDiff(dd,duedate,getdate()) > 23 and DateDiff(d,duedate,getdate()) < 24 then (SAS_Accounts.TransAmount - SAS_Accounts.PaidAmount) else '0.00' end as 'Day23',"
                Str += "case when DateDiff(dd,duedate,getdate()) > 24 and DateDiff(d,duedate,getdate()) < 25 then (SAS_Accounts.TransAmount - SAS_Accounts.PaidAmount) else '0.00' end as 'Day24',"
                Str += "case when DateDiff(dd,duedate,getdate()) > 25 and DateDiff(d,duedate,getdate()) < 26 then (SAS_Accounts.TransAmount - SAS_Accounts.PaidAmount) else '0.00' end as 'Day25',"
                Str += "case when DateDiff(dd,duedate,getdate()) > 26 and DateDiff(d,duedate,getdate()) < 27 then (SAS_Accounts.TransAmount - SAS_Accounts.PaidAmount) else '0.00' end as 'Day26',"
                Str += "case when DateDiff(dd,duedate,getdate()) > 27 and DateDiff(d,duedate,getdate()) < 28 then (SAS_Accounts.TransAmount - SAS_Accounts.PaidAmount) else '0.00' end as 'Day27',"
                Str += "case when DateDiff(dd,duedate,getdate()) > 28 and DateDiff(d,duedate,getdate()) < 29 then (SAS_Accounts.TransAmount - SAS_Accounts.PaidAmount) else '0.00' end as 'Day28',"
                Str += "case when DateDiff(dd,duedate,getdate()) > 29 and DateDiff(d,duedate,getdate()) < 30 then (SAS_Accounts.TransAmount - SAS_Accounts.PaidAmount) else '0.00' end as 'Day29',"
                Str += "case when DateDiff(dd,duedate,getdate()) > 30 and DateDiff(d,duedate,getdate()) < 31 then (SAS_Accounts.TransAmount - SAS_Accounts.PaidAmount) else '0.00' end as 'Day30',"
                Str += "case when DateDiff(dd,duedate,getdate()) > 31 then (SAS_Accounts.TransAmount - SAS_Accounts.PaidAmount) else '0.00' end as Day31"
                Str += " FROM  SAS_Accounts INNER JOIN "
                Str += " SAS_Sponsor ON SAS_Accounts.CreditRef = SAS_Sponsor.SASR_Code "
                Str += " where Transstatus='Open' and poststatus='Posted' and  Transtype='Credit' and subtype='Sponsor' "
                Str += " and  SAS_Sponsor.SASR_Code like '" + sponsor + "'"
                Str += datecondition

                'Str = "SELECT dbo.SAS_Sponsor.SASR_Code,dbo.SAS_Accounts.TransAmount,dbo.SAS_Accounts.Transtype,"
                'Str += "dbo.SAS_Accounts.TransCode, dbo.SAS_Sponsor.SASR_Name, dbo.SAS_Sponsor.SASR_Address, dbo.SAS_Sponsor.SASR_Address1, dbo.SAS_Sponsor.SASR_Address2, "
                'Str += "(dbo.SAS_Accounts.TransAmount - dbo.SAS_Accounts.PaidAmount)as Dueamount, "
                'Str += "CONVERT(VARCHAR(10),sas_accounts.transdate,105) AS TRANSDATE, "
                'Str += "CONVERT(VARCHAR(10),sas_accounts.duedate,105) AS DUEDATE, "
                'Str += "DateDiff(dd,duedate,getdate()) AS Different_day, "
                'Str += " '" + fdate + "' as datefrom, "
                'Str += " '" + tdate + "' as dateto, "
                'Str += "case when DateDiff(dd,duedate,getdate()) < -1 then (dbo.SAS_Accounts.TransAmount - dbo.SAS_Accounts.PaidAmount) else '0.00' end as 'PreDay',"
                'Str += "case when DateDiff(dd,duedate,getdate()) > 1 and DateDiff(d,duedate,getdate()) < 2 then (dbo.SAS_Accounts.TransAmount - dbo.SAS_Accounts.PaidAmount) else '0.00' end as 'Day1',"
                'Str += "case when DateDiff(dd,duedate,getdate()) > 2 and DateDiff(d,duedate,getdate()) < 3 then (dbo.SAS_Accounts.TransAmount - dbo.SAS_Accounts.PaidAmount) else '0.00' end as 'Day2',"
                'Str += "case when DateDiff(dd,duedate,getdate()) > 3 and DateDiff(d,duedate,getdate()) < 4 then (dbo.SAS_Accounts.TransAmount - dbo.SAS_Accounts.PaidAmount) else '0.00' end as 'Day3',"
                'Str += "case when DateDiff(dd,duedate,getdate()) > 4 and DateDiff(d,duedate,getdate()) < 5 then (dbo.SAS_Accounts.TransAmount - dbo.SAS_Accounts.PaidAmount) else '0.00' end as 'Day4',"
                'Str += "case when DateDiff(dd,duedate,getdate()) > 5 and DateDiff(d,duedate,getdate()) < 6 then (dbo.SAS_Accounts.TransAmount - dbo.SAS_Accounts.PaidAmount) else '0.00' end as 'Day5',"
                'Str += "case when DateDiff(dd,duedate,getdate()) > 6 and DateDiff(d,duedate,getdate()) < 7 then (dbo.SAS_Accounts.TransAmount - dbo.SAS_Accounts.PaidAmount) else '0.00' end as 'Day6',"
                'Str += "case when DateDiff(dd,duedate,getdate()) > 7 and DateDiff(d,duedate,getdate()) < 8 then (dbo.SAS_Accounts.TransAmount - dbo.SAS_Accounts.PaidAmount) else '0.00' end as 'Day7',"
                'Str += "case when DateDiff(dd,duedate,getdate()) > 8 and DateDiff(d,duedate,getdate()) < 9 then (dbo.SAS_Accounts.TransAmount - dbo.SAS_Accounts.PaidAmount) else '0.00' end as 'Day8',"
                'Str += "case when DateDiff(dd,duedate,getdate()) > 9 and DateDiff(d,duedate,getdate()) < 10 then (dbo.SAS_Accounts.TransAmount - dbo.SAS_Accounts.PaidAmount) else '0.00' end as 'Day9',"
                'Str += "case when DateDiff(dd,duedate,getdate()) > 10 and DateDiff(d,duedate,getdate()) < 11 then (dbo.SAS_Accounts.TransAmount - dbo.SAS_Accounts.PaidAmount) else '0.00' end as 'Day10',"
                'Str += "case when DateDiff(dd,duedate,getdate()) > 11 and DateDiff(d,duedate,getdate()) < 12 then (dbo.SAS_Accounts.TransAmount - dbo.SAS_Accounts.PaidAmount) else '0.00' end as 'Day11',"
                'Str += "case when DateDiff(dd,duedate,getdate()) > 12 and DateDiff(d,duedate,getdate()) < 13 then (dbo.SAS_Accounts.TransAmount - dbo.SAS_Accounts.PaidAmount) else '0.00' end as 'Day12',"
                'Str += "case when DateDiff(dd,duedate,getdate()) > 13 and DateDiff(d,duedate,getdate()) < 14 then (dbo.SAS_Accounts.TransAmount - dbo.SAS_Accounts.PaidAmount) else '0.00' end as 'Day13',"
                'Str += "case when DateDiff(dd,duedate,getdate()) > 14 and DateDiff(d,duedate,getdate()) < 15 then (dbo.SAS_Accounts.TransAmount - dbo.SAS_Accounts.PaidAmount) else '0.00' end as 'Day14',"
                'Str += "case when DateDiff(dd,duedate,getdate()) > 15 and DateDiff(d,duedate,getdate()) < 16 then (dbo.SAS_Accounts.TransAmount - dbo.SAS_Accounts.PaidAmount) else '0.00' end as 'Day15',"
                'Str += "case when DateDiff(dd,duedate,getdate()) > 16 and DateDiff(d,duedate,getdate()) < 17 then (dbo.SAS_Accounts.TransAmount - dbo.SAS_Accounts.PaidAmount) else '0.00' end as 'Day16',"
                'Str += "case when DateDiff(dd,duedate,getdate()) > 17 and DateDiff(d,duedate,getdate()) < 18 then (dbo.SAS_Accounts.TransAmount - dbo.SAS_Accounts.PaidAmount) else '0.00' end as 'Day17',"
                'Str += "case when DateDiff(dd,duedate,getdate()) > 18 and DateDiff(d,duedate,getdate()) < 19 then (dbo.SAS_Accounts.TransAmount - dbo.SAS_Accounts.PaidAmount) else '0.00' end as 'Day18',"
                'Str += "case when DateDiff(dd,duedate,getdate()) > 19 and DateDiff(d,duedate,getdate()) < 20 then (dbo.SAS_Accounts.TransAmount - dbo.SAS_Accounts.PaidAmount) else '0.00' end as 'Day19',"
                'Str += "case when DateDiff(dd,duedate,getdate()) > 20 and DateDiff(d,duedate,getdate()) < 21 then (dbo.SAS_Accounts.TransAmount - dbo.SAS_Accounts.PaidAmount) else '0.00' end as 'Day20',"
                'Str += "case when DateDiff(dd,duedate,getdate()) > 21 and DateDiff(d,duedate,getdate()) < 22 then (dbo.SAS_Accounts.TransAmount - dbo.SAS_Accounts.PaidAmount) else '0.00' end as 'Day21',"
                'Str += "case when DateDiff(dd,duedate,getdate()) > 22 and DateDiff(d,duedate,getdate()) < 23 then (dbo.SAS_Accounts.TransAmount - dbo.SAS_Accounts.PaidAmount) else '0.00' end as 'Day22',"
                'Str += "case when DateDiff(dd,duedate,getdate()) > 23 and DateDiff(d,duedate,getdate()) < 24 then (dbo.SAS_Accounts.TransAmount - dbo.SAS_Accounts.PaidAmount) else '0.00' end as 'Day23',"
                'Str += "case when DateDiff(dd,duedate,getdate()) > 24 and DateDiff(d,duedate,getdate()) < 25 then (dbo.SAS_Accounts.TransAmount - dbo.SAS_Accounts.PaidAmount) else '0.00' end as 'Day24',"
                'Str += "case when DateDiff(dd,duedate,getdate()) > 25 and DateDiff(d,duedate,getdate()) < 26 then (dbo.SAS_Accounts.TransAmount - dbo.SAS_Accounts.PaidAmount) else '0.00' end as 'Day25',"
                'Str += "case when DateDiff(dd,duedate,getdate()) > 26 and DateDiff(d,duedate,getdate()) < 27 then (dbo.SAS_Accounts.TransAmount - dbo.SAS_Accounts.PaidAmount) else '0.00' end as 'Day26',"
                'Str += "case when DateDiff(dd,duedate,getdate()) > 27 and DateDiff(d,duedate,getdate()) < 28 then (dbo.SAS_Accounts.TransAmount - dbo.SAS_Accounts.PaidAmount) else '0.00' end as 'Day27',"
                'Str += "case when DateDiff(dd,duedate,getdate()) > 28 and DateDiff(d,duedate,getdate()) < 29 then (dbo.SAS_Accounts.TransAmount - dbo.SAS_Accounts.PaidAmount) else '0.00' end as 'Day28',"
                'Str += "case when DateDiff(dd,duedate,getdate()) > 29 and DateDiff(d,duedate,getdate()) < 30 then (dbo.SAS_Accounts.TransAmount - dbo.SAS_Accounts.PaidAmount) else '0.00' end as 'Day29',"
                'Str += "case when DateDiff(dd,duedate,getdate()) > 30 and DateDiff(d,duedate,getdate()) < 31 then (dbo.SAS_Accounts.TransAmount - dbo.SAS_Accounts.PaidAmount) else '0.00' end as 'Day30',"
                'Str += "case when DateDiff(dd,duedate,getdate()) > 31 then (dbo.SAS_Accounts.TransAmount - dbo.SAS_Accounts.PaidAmount) else '0.00' end as Day31"
                'Str += " FROM  dbo.SAS_Accounts INNER JOIN "
                'Str += " dbo.SAS_Sponsor ON dbo.SAS_Accounts.CreditRef = dbo.SAS_Sponsor.SASR_Code "
                'Str += " where Transstatus='Open' and poststatus='Posted' and  Transtype='Credit' and subtype='Sponsor' "
                'Str += " and  dbo.SAS_Sponsor.SASR_Code like '" + sponsor + "'"
                'Str += datecondition

                'Author			: Anil Kumar - T-Melmax Sdn Bhd
                'Created Date	: 20/05/2015

                'DataSet Strating
                Dim _DataSet As DataSet = _ReportHelper.GetDataSet(Str)

                'Report XML Loading

                Dim s As String = Server.MapPath("xml\SponsorAgeingDate.xml")
                _DataSet.WriteXml(s)

                'Report XML Ended

                'Records Checking

                If _DataSet.Tables(0).Rows.Count = 0 Then
                    Response.Write("No Record Found")

                Else

                    'Report Loading 

                    MyReportDocument.Load(Server.MapPath("RptSpAgeingDateCRFormFinal.rpt"))
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
