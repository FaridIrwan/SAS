Imports System.Data
Imports System.Data.SqlClient
Imports System.Web.Configuration
Imports CrystalDecisions.CrystalReports.Engine

Partial Class RptSpAllocationCRForm
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
                Dim l As Integer = 0
                Dim str As String = Nothing

                If Session("sortby") = Nothing Then

                    Session("sortby") = "NoSort"
                    sortbyvalue = ""

                Else

                    sortbyvalue = " Order By " + Session("sortby")

                End If

                If Session("status") = Nothing Then

                    status = ""

                Else

                    status = " where SAS_Student.SASI_statusrec =" + Session("status")

                End If

                If Session("faculty") = Nothing Then

                    faculty = ""

                Else

                    faculty = " and SAS_Student.SASI_faculty =" + Session("faculty")

                End If

                If Session("program") = Nothing Then

                    program = " "

                Else

                    program = " and SAS_Student.SASI_pgid =" + Session("program")

                End If

                If Session("sponsor") = Nothing Then

                    sponsor = ""

                Else

                    sponsor = " and SAS_sponsor.sasr_code=" + Session("sponsor")

                End If

                str = "SELECT SAS_Accounts.TransCode, COUNT(SAS_Accounts.CreditRef) AS NoOfStudents, SAS_Sponsor.SASR_Name, SAS_Accounts.TransDate,"
                str += "SUM(SAS_AccountsDetails.TempAmount) AS PocketAmount, SUM(SAS_AccountsDetails.TransAmount) AS AllocationAmount,"
                str += "SAS_Accounts_1.TransCode AS ReceiptNo, SAS_Accounts_1.TransAmount AS ReceivedAmount FROM SAS_Accounts INNER JOIN "
                str += " SAS_Sponsor ON SAS_Accounts.CreditRef = SAS_Sponsor.SASR_Code INNER JOIN SAS_Accounts SAS_Accounts_1 ON "
                str += "SAS_Accounts.CreditRef1 = SAS_Accounts_1.TransCode INNER JOIN SAS_AccountsDetails ON "
                str += "SAS_Accounts.TransCode = SAS_AccountsDetails.TransCode INNER JOIN  SAS_Student ON "
                str += " SAS_AccountsDetails.RefCode = SAS_Student.SASI_MatricNo GROUP BY SAS_Accounts.TransCode, SAS_Accounts.TransDate,"
                str += "SAS_Sponsor.SASR_Name, SAS_Accounts_1.TransCode,  SAS_Accounts_1.TransAmount"


                'Author			: Anil Kumar - T-Melmax Sdn Bhd
                'Created Date	: 20/05/2015

                'DataSet Strating
                Dim _DataSet As DataSet = _ReportHelper.GetDataSet(str)

                'Report XML Loading

                Dim s As String = Server.MapPath("~/xml/SpAllocationCR.xml")
                _DataSet.WriteXml(s)

                'Report XML Ended

                'Records Checking

                If _DataSet.Tables(0).Rows.Count = 0 Then

                    While l < _DataSet.Tables(0).Rows.Count
                        Dim noofstudents As Integer = _DataSet.Tables(0).Rows(l).Item("NoOfStudents")
                        Dim PocketAmount As Integer = _DataSet.Tables(0).Rows(l).Item("PocketAmount")
                        Dim AllocationAmount As Integer = _DataSet.Tables(0).Rows(l).Item("AllocationAmount")
                        Session("NoOfStudents") = noofstudents
                        Session("PocketAmount") = PocketAmount
                        Session("AllocationAmount") = AllocationAmount
                        Session("totalamount") = AllocationAmount
                        Session("balanceamount") = PocketAmount


                        l = l + 1
                    End While
                End If

                'Report Loading 
                MyReportDocument.Load(Server.MapPath("RptSpAllocationCR.rpt"))
                MyReportDocument.SetParameterValue("noofstudents", Session("noofstudents"))
                MyReportDocument.SetParameterValue("amountallocation", Session("AllocationAmount"))
                MyReportDocument.SetParameterValue("pocketamount", Session("PocketAmount"))
                MyReportDocument.SetParameterValue("totalamount", Session("totalamount"))
                MyReportDocument.SetParameterValue("balanceamount", Session("balanceamount"))
                ''MyReportDocument.SetParameterValue("PaidAmount", Session("PaidAmount"))
                MyReportDocument.SetDataSource(_DataSet)
                Session("reportobject") = MyReportDocument
                CrystalReportViewer1.ReportSource = MyReportDocument
                CrystalReportViewer1.DataBind()
                MyReportDocument.Refresh()

                'Report Ended

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
