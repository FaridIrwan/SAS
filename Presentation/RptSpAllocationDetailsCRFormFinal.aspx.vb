Imports System.Data
Imports System.Data.SqlClient
Imports System.Web.Configuration
Imports CrystalDecisions.CrystalReports.Engine

Partial Class RptSpAllocationDetailsCRFormFinal
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
                Dim status As String = Nothing
                Dim program As String = Nothing
                Dim faculty As String = Nothing
                Dim sponsor As String = Nothing
                Dim datecondition As String = Nothing
                Dim str As String = Nothing

                If Session("sortby") Is Nothing Then

                    sortbyvalue = "Order By SAS_Sponsor.SASR_Name"

                ElseIf Session("sortby") = "SAS_Accounts.CreditRef" Then

                    sortbyvalue = " Order By " + Session("sortby")

                ElseIf Session("sortby") = "SAS_Sponsor.SASR_Name" Then

                    sortbyvalue = " Order By " + Session("sortby")

                ElseIf Session("sortby") = "SAS_Accounts.TransDate" Then

                    sortbyvalue = " Order By SAS_Sponsor.SASR_Name,SAS_Accounts.CreditRef1," + Session("sortby")

                ElseIf Session("sortby") = "SAS_Student.SASI_MatricNo" Then

                    sortbyvalue = " Order By SAS_Sponsor.SASR_Name,SAS_Accounts.CreditRef1," + Session("sortby")

                ElseIf Session("sortby") = "SAS_Student.SASI_Name" Then

                    sortbyvalue = " Order By SAS_Sponsor.SASR_Name,SAS_Accounts.CreditRef1," + Session("sortby")

                End If

                If Session("status") Is Nothing Then

                    status = "%"

                Else

                    status = Session("status")

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

                    datecondition = " and dbo.SAS_Accounts.TransDate between '" + datefrom + "' and '" + dateto + "'"

                End If

                str = " SELECT COUNT(SAS_Accounts.CreditRef) AS NoOfStudents, SUM(SAS_AccountsDetails.TransAmount) AS Allocated, "
                str += " SUM(SAS_AccountsDetails.TempAmount) AS Pocket, SAS_Accounts.TransCode AS DocumentNo, "
                str += " SUM(SAS_AccountsDetails.TempAmount + SAS_AccountsDetails.TransAmount) AS TotalAmount, "
                str += " SAS_Accounts.CreditRef1 AS ReceiptNumber, CONVERT(VARCHAR(10), SAS_Accounts.TransDate, 103) AS TransDate, "
                str += " SAS_Accounts.CreditRef, SAS_Sponsor.SASR_Name, SAS_Accounts_1.TransAmount AS ReceivedAmount, SAS_Student.SASI_Name, "
                str += " SAS_Student.SASI_MatricNo, SAS_Student.SASI_PgId, SAS_Student.SASI_Faculty "
                str += " FROM SAS_Student INNER JOIN "
                str += " SAS_AccountsDetails ON SAS_Student.SASI_MatricNo = SAS_AccountsDetails.RefCode INNER JOIN "
                str += " SAS_Accounts ON SAS_AccountsDetails.TransCode = SAS_Accounts.TransCode INNER JOIN "
                str += " SAS_Accounts SAS_Accounts_1 ON SAS_Accounts.CreditRef1 = SAS_Accounts_1.TransCode INNER JOIN "
                str += " SAS_Sponsor ON SAS_Accounts.CreditRef = SAS_Sponsor.SASR_Code "
                str += " WHERE     (SAS_Accounts.Category = 'Allocation') AND (SAS_Student.SASI_StatusRec LIKE '" + status + "') AND (SAS_Student.SASI_PgId LIKE '" + program + "') AND "
                str += " (SAS_Student.SASI_Faculty LIKE '" + faculty + "') AND (SAS_Sponsor.SASR_Code LIKE '" + sponsor + "') "
                str += datecondition
                str += " GROUP BY SAS_Accounts.TransCode, SAS_Accounts.CreditRef1, SAS_Accounts.TransDate, SAS_Accounts.CreditRef, "
                str += " SAS_Sponsor.SASR_Name, SAS_Accounts_1.TransAmount, SAS_Student.SASI_Name, SAS_Student.SASI_MatricNo, "
                str += " SAS_Student.SASI_PgId, SAS_Student.SASI_Faculty "
                str += sortbyvalue

                'str = " SELECT COUNT(dbo.SAS_Accounts.CreditRef) AS NoOfStudents, SUM(dbo.SAS_AccountsDetails.TransAmount) AS Allocated, "
                'str += " SUM(dbo.SAS_AccountsDetails.TempAmount) AS Pocket, dbo.SAS_Accounts.TransCode AS DocumentNo, "
                'str += " SUM(dbo.SAS_AccountsDetails.TempAmount + dbo.SAS_AccountsDetails.TransAmount) AS TotalAmount, "
                'str += " dbo.SAS_Accounts.CreditRef1 AS ReceiptNumber, CONVERT(VARCHAR(10), dbo.SAS_Accounts.TransDate, 103) AS TransDate, "
                'str += " dbo.SAS_Accounts.CreditRef, dbo.SAS_Sponsor.SASR_Name, SAS_Accounts_1.TransAmount AS ReceivedAmount, dbo.SAS_Student.SASI_Name, "
                'str += " dbo.SAS_Student.SASI_MatricNo, dbo.SAS_Student.SASI_PgId, dbo.SAS_Student.SASI_Faculty "
                'str += " FROM dbo.SAS_Student INNER JOIN "
                'str += " dbo.SAS_AccountsDetails ON dbo.SAS_Student.SASI_MatricNo = dbo.SAS_AccountsDetails.RefCode INNER JOIN "
                'str += " dbo.SAS_Accounts ON dbo.SAS_AccountsDetails.TransCode = dbo.SAS_Accounts.TransCode INNER JOIN "
                'str += " dbo.SAS_Accounts SAS_Accounts_1 ON dbo.SAS_Accounts.CreditRef1 = SAS_Accounts_1.TransCode INNER JOIN "
                'str += " dbo.SAS_Sponsor ON dbo.SAS_Accounts.CreditRef = dbo.SAS_Sponsor.SASR_Code "
                'str += " WHERE     (dbo.SAS_Accounts.Category = 'Allocation') AND (dbo.SAS_Student.SASI_StatusRec LIKE '" + status + "') AND (dbo.SAS_Student.SASI_PgId LIKE '" + program + "') AND "
                'str += " (dbo.SAS_Student.SASI_Faculty LIKE '" + faculty + "') AND (dbo.SAS_Sponsor.SASR_Code LIKE '" + sponsor + "') "
                'str += datecondition
                'str += " GROUP BY dbo.SAS_Accounts.TransCode, dbo.SAS_Accounts.CreditRef1, dbo.SAS_Accounts.TransDate, dbo.SAS_Accounts.CreditRef, "
                'str += " dbo.SAS_Sponsor.SASR_Name, SAS_Accounts_1.TransAmount, dbo.SAS_Student.SASI_Name, dbo.SAS_Student.SASI_MatricNo, "
                'str += " dbo.SAS_Student.SASI_PgId, dbo.SAS_Student.SASI_Faculty "
                'str += sortbyvalue



                'Author			: Anil Kumar - T-Melmax Sdn Bhd
                'Created Date	: 20/05/2015

                'DataSet Strating
                Dim _DataSet As DataSet = _ReportHelper.GetDataSet(str)

                'Report XML Loading

                Dim s As String = Server.MapPath("xml\AllocationDetails.xml")
                _DataSet.WriteXml(s)

                'Report XML Ended

                'Records Checking

                If _DataSet.Tables(0).Rows.Count = 0 Then
                    Response.Write("No Record Found")

                Else

                    'Report Loading 

                    MyReportDocument.Load(Server.MapPath("RptSpAllocationDetailsCRFinal.rpt"))
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
