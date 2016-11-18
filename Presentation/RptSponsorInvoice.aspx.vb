Imports System.Data
Imports System.Data.SqlClient
Imports System.Web.Configuration
Imports CrystalDecisions.CrystalReports.Engine


Partial Class RptSponsorInvoice
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
                Dim sponsor As String = Nothing
                Dim str As String = Nothing

                batchNo = Request.QueryString("BatchNo")

                str = "           SELECT DISTINCT"
                str += "                       SAS_Student.SASI_Name  AS BatchCode ,"
                str += "			            SAS_SponsorInvoice.BatchCode AS Batch ,"
                str += "			            SAS_SponsorInvoice.BatchIntake ,"
                str += "			            SAS_SponsorInvoice.Description ,"
                str += "			            SAS_Sponsor.SASR_Name AS Sponsor ,"
                str += "			            CONVERT(VARCHAR(10), SAS_SponsorInvoice.BatchDate, 103) BatchDate ,"
                str += "			            CONVERT(VARCHAR(10), SAS_SponsorInvoice.TransDate, 103) TransDate ,"
                str += "			            CONVERT(VARCHAR(10), SAS_SponsorInvoice.DueDate, 103) DueDate ,"
                str += "			            SAS_FeeTypes.SAFT_Code ,"
                str += "			            SAS_FeeTypes.SAFT_Desc ,"
                str += "			            SAS_FeeTypes.SAFT_Priority ,"
                str += "			            SAS_SponsorInvoiceDetails.TransAmount ,"
                str += "			            SAS_SponsorInvoice.TransAmount AS GroupTotal,"
                str += "			            ( SELECT    SUM(SS.TransAmount)"
                str += "			                FROM      SAS_SponsorInvoice SS"
                str += "                        WHERE (SS.BatchCode = SAS_SponsorInvoice.BatchCode)"
                str += "			            ) TotalAmount"
                str += "            FROM SAS_SponsorInvoice "
                str += "			            INNER JOIN SAS_SponsorInvoiceDetails ON SAS_SponsorInvoiceDetails.TransID = SAS_SponsorInvoice.TransID"
                str += "			            INNER JOIN SAS_Student ON SAS_Student.SASI_MatricNo =SAS_SponsorInvoice.CreditRef"
                str += "			            INNER JOIN SAS_Sponsor ON SAS_SponsorInvoice.CreditRef1 = SAS_Sponsor.SASR_Code"
                str += "			            INNER JOIN SAS_FeeTypes ON SAS_SponsorInvoiceDetails.RefCode = SAS_FeeTypes.SAFT_Code"
                str += "	            WHERE   SAS_SponsorInvoiceDetails.TransID IN ("
                str += "                                                 Select SA.TransID"
                str += "			                                             FROM    SAS_SponsorInvoice SA"
                str += "			                                             WHERE   SA.Category = 'Invoice' ) AND SAS_SponsorInvoice.BatchCode ='" + batchNo + "' ORDER BY SAS_Student.SASI_Name  "


                'Author			: Anil Kumar - T-Melmax Sdn Bhd
                'Created Date	: 20/05/2015

                'DataSet Strating
                Dim _DataSet As DataSet = _ReportHelper.GetDataSet(str)

                'Report XML Loading
                Dim s As String = Server.MapPath("~/xml/SponserInvoice.xml")
                _DataSet.WriteXml(s)

                'Report XML Ended

                'Records Checking

                If _DataSet.Tables(0).Rows.Count = 0 Then
                    Response.Write("No Record Found")

                Else

                    'Report Loading 

                    MyReportDocument.Load(Server.MapPath("RptSponsorInvoice.rpt"))
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
