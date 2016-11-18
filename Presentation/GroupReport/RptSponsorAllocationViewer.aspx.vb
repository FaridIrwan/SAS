Imports System.Data
Imports System.Data.SqlClient
Imports System.Web.Configuration
Imports CrystalDecisions.CrystalReports.Engine
Imports CrystalDecisions.Shared
Imports System.Web

Partial Class RptSponsorAllocationViewer
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
        'Modified by Hafiz @ 10/3/2016
        'Modified by Hafiz @ 26/4/2016

        Try

            If Not IsPostBack Then
                Dim _ReportHelper As New ReportHelper

                Dim sortbyvalue As String = Nothing
                Dim status As String = Nothing
                Dim program As String = Nothing
                Dim faculty As String = Nothing
                Dim sponsor As String = Nothing
                Dim datecondition As String = Nothing
                Dim str As String = Nothing

                If Session("sortby") Is Nothing Then
                    sortbyvalue = "Order By SAS_Sponsor.SASR_Name"
                ElseIf Session("sortby") = "SAS_Sponsor.SASR_Name" Then
                    sortbyvalue = "Order By " + Session("sortby")
                ElseIf Session("sortby") = "SAS_Accounts.TransDate" Then
                    sortbyvalue = "Order By SAS_Sponsor.SASR_Name,SAS_Accounts.CreditRef1," + Session("sortby")
                ElseIf Session("sortby") = "SAS_Accounts.CreditRef" Then
                    sortbyvalue = "Order By " + Session("sortby")
                End If

                If Session("status") Is Nothing Then
                    status = ""
                Else
                    status = " and SAS_Student.SASS_Code IN " + _ReportHelper.Status(Session("status")) + " "
                End If

                If Session("faculty") Is Nothing Then

                    faculty = "%"

                Else

                    faculty = Session("faculty")

                End If

                If Session("program") Is Nothing Then

                    program = "%"

                Else

                    program = Session("program")

                End If

                If Session("sponsor") Is Nothing Then

                    sponsor = "%"

                Else

                    sponsor = Session("sponsor")

                End If


                If Request.QueryString("fdate") = "0" Or Request.QueryString("tdate") = "0" Then

                    datecondition = ""

                Else

                    Dim datef As String = Request.QueryString("fdate")
                    Dim datet As String = Request.QueryString("tdate")
                    Dim d1, m1, y1, d2, m2, y2 As String
                    d1 = Mid(datef, 1, 2)
                    m1 = Mid(datef, 4, 2)
                    y1 = Mid(datef, 7, 4)
                    Dim datefrom As String = y1 + "/" + m1 + "/" + d1
                    d2 = Mid(datet, 1, 2)
                    m2 = Mid(datet, 4, 2)
                    y2 = Mid(datet, 7, 4)
                    Dim dateto As String = y2 + "/" + m2 + "/" + d2

                    datecondition = " and SAS_Accounts.TransDate between '" + datefrom + "' and '" + dateto + "'"

                End If

                str = "SELECT COUNT(SAS_Accounts.CreditRef) AS NoOfStudents, SUM(SAS_AccountsDetails.TransAmount) AS Allocated, "
                str += " SUM(SAS_AccountsDetails.TempAmount) AS Pocket, SAS_Accounts.TransCode AS DocumentNo,"
                str += " SUM(SAS_AccountsDetails.TempAmount + SAS_AccountsDetails.TransAmount) AS TotalAmount,"
                str += " SAS_Accounts.CreditRef1 AS ReceiptNumber,to_char(SAS_Accounts.TransDate,'DD/MM/YYYY') as TransDate, SAS_Accounts.CreditRef, SAS_Sponsor.SASR_Name,"
                'str += " SAS_Accounts_1.TransAmount AS ReceivedAmount "
                str += " SAS_Accounts.TransAmount AS ReceivedAmount "
                str += " FROM SAS_Student INNER JOIN"
                str += " SAS_AccountsDetails ON SAS_Student.SASI_MatricNo = SAS_AccountsDetails.RefCode INNER JOIN"
                str += " SAS_Accounts ON SAS_AccountsDetails.TransCode = SAS_Accounts.TransCode INNER JOIN"
                'str += " SAS_Accounts SAS_Accounts_1 ON SAS_Accounts.CreditRef1 = SAS_Accounts_1.TransCode INNER JOIN"
                str += " SAS_Sponsor ON SAS_Accounts.CreditRef = SAS_Sponsor.SASR_Code "
                str += " WHERE SAS_Accounts.Category = 'Allocation'  "
                str += " and SAS_Student.SASI_PgId like '" + program + "' "
                str += " and  SAS_Student.SASI_Faculty like '" + faculty + "' and  SAS_Sponsor.SASR_Code like '" + sponsor + "' "
                str += status
                str += datecondition + " "
                str += " GROUP BY SAS_Accounts.TransCode, SAS_Accounts.CreditRef1, SAS_Accounts.TransDate, SAS_Accounts.CreditRef,"
                str += " SAS_Sponsor.SASR_Name, SAS_Accounts.TransAmount "
                str += sortbyvalue

                'Author			: Anil Kumar - T-Melmax Sdn Bhd
                'Created Date	: 20/05/2015

                'DataSet Strating
                Dim _DataSet As DataSet = _ReportHelper.GetDataSet(str)
                _DataSet.Tables(0).TableName = "Table"

                'Report XML Loading
                Dim s As String = Server.MapPath("~/xml/Allocation.xml")
                _DataSet.WriteXml(s)
                'Report XML Ended

                'Records Checking
                If _DataSet.Tables(0).Rows.Count = 0 Then
                    Response.Write("No Record Found")

                Else

                    'Report Loading 
                    MyReportDocument.Load(Server.MapPath("~/GroupReport/RptSponsorAllocation.rpt"))
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
