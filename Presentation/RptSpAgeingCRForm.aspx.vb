Imports System.Data
Imports System.Data.SqlClient
Imports System.Web.Configuration
Imports CrystalDecisions.CrystalReports.Engine

Partial Class RptStuAgeingCRForm
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
                'Response.Write("Session Name is " + Session("sortby"))

                If Session("status") = Nothing Then
                    'Response.Write("Status Nothing")
                Else
                    'Response.Write(Session("status"))
                End If

                If Session("faculty") = Nothing Then
                    'Response.Write("Faculty Nothing")
                Else
                    'Response.Write(Session("faculty"))
                End If

                If Session("program") = Nothing Then
                    'Response.Write("Program Nothing")
                Else
                    'Response.Write(Session("program"))
                End If

                If Session("sponsor") = Nothing Then
                    'Response.Write("Sponsor Nothing")
                Else
                    'Response.Write(Session("sponsor"))
                End If



                str = " SELECT TransCode, CreditRef, Category, TransDate, DueDate, SubType "
                str += " FROM dbo.SAS_Accounts WHERE SubType = 'Sponsor'"


                Dim dateperiord As String = "select case when DateDiff(d,getdate(),duedate) < 31 then '30' "
                dateperiord += " when DateDiff(d,getdate(),duedate) > 30 and DateDiff(d,getdate(),duedate) < 61 then '60' "
                dateperiord += " else '90' end FROM dbo.SAS_Accounts"

                'Author			: Anil Kumar - T-Melmax Sdn Bhd
                'Created Date	: 20/05/2015

                'Execute Scalar Starting
                Session("dateperiord") = GetExecuteScalar(dateperiord)

                'DataSet Strating
                Dim _DataSet As DataSet = _ReportHelper.GetDataSet(str)

                'Report XML Loading

                Dim s As String = Server.MapPath("~/xml/SpAgeingCRForm.xml")
                _DataSet.WriteXml(s)

                'Report XML Ended

                'Records Checking

                If _DataSet.Tables(0).Rows.Count = 0 Then
                    Response.Write("No Record Found")

                Else

                    'Report Loading

                    Dim MyReportDocument As ReportDocument = New ReportDocument()
                    MyReportDocument.Load(Server.MapPath("RptSpAgeingCR.rpt"))
                    MyReportDocument.SetParameterValue("dateperiord", Session("dateperiord"))
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


    Private Function GetExecuteScalar(dateperiord As String) As Object
        Throw New NotImplementedException
    End Function

End Class
