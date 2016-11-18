Imports System.Data
Imports System.Data.SqlClient
Imports System.Web.Configuration
Imports CrystalDecisions.CrystalReports.Engine

Partial Class RptInvoiceViewer
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

                Dim cond As String = Nothing
                Dim batchNo As String = Nothing
                Dim cat As String = Nothing
                Dim str As String = Nothing

                str = "SELECT Distinct SAS_Accounts.BatchCode, SAS_Accounts.BatchIntake, SAS_Accounts.Description,"
                str += "CONVERT(VARCHAR(10), SAS_Accounts.BatchDate, 103) BatchDate, CONVERT(VARCHAR(10), SAS_Accounts.TransDate, 103) TransDate, "
                str += "CONVERT(VARCHAR(10), SAS_Accounts.DueDate, 103) DueDate,SAS_FeeTypes.SAFT_Code, "
                str += " SAS_FeeTypes.SAFT_Desc, SAS_FeeTypes.SAFT_Priority,SAS_AccountsDetails.TransAmount, "
                str += "(Select sum(SS.TransAmount) From SAS_Accounts SS Where SS.BatchCode = SAS_Accounts.BatchCode) TotalAmount "
                str += "FROM SAS_Accounts INNER JOIN SAS_AccountsDetails ON SAS_AccountsDetails.TransID = SAS_Accounts.TransID "
                str += "INNER JOIN SAS_FeeTypes ON SAS_AccountsDetails.RefCode = SAS_FeeTypes.SAFT_Code WHERE SAS_AccountsDetails.TransID "
                str += "IN (SELECT SA.TransID FROM SAS_Accounts SA WHERE SA.Category='Invoice' AND SA.PostStatus='Ready' "
                str += "and SA.SubType ='Student')"
                str += cond


                'Author			: Anil Kumar - T-Melmax Sdn Bhd
                'Created Date	: 20/05/2015

                'DataSet Strating
                Dim _DataSet As DataSet = _ReportHelper.GetDataSet(str)
                _DataSet.Tables(0).TableName = "Table"

                'Report AFCReport Loading XML

                Dim s As String = Server.MapPath("xml\StudAgeing.xml")
                _DataSet.WriteXml(s)

                'Report AFCReport Ended XML

                'Records Checking

                If _DataSet.Tables(0).Rows.Count = 0 Then
                    Response.Write("No Record Found")
                Else

                    'Report Loading 


                    MyReportDocument.Load(Server.MapPath("RptInvoice.rpt"))

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

End Class
