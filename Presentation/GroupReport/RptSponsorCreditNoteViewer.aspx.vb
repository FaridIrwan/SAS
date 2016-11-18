Imports System.Data
Imports System.Data.SqlClient
Imports System.Web.Configuration
Imports CrystalDecisions.CrystalReports.Engine
Imports CrystalDecisions.Shared

Partial Class RptSponsorCreditNoteViewer
    Inherits System.Web.UI.Page

#Region "Global Declarations "

    'added by Hafiz @ 06/4/2016

    Private MyReportDocument As New ReportDocument
    Private _ReportHelper As New ReportHelper

#End Region
 
#Region "Page Load Start"

    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init

        Try
            Dim str As String = Nothing
            Dim batchcode As String = ""
            Dim category As String = ""

            If Not IsPostBack Then

                If Request.QueryString("batchNo") Is Nothing Then
                    Response.Write("Load Report Failed. No Batchcode Selected")
                    Return
                Else
                    BatchCode = Request.QueryString("batchNo")
                    category = Request.QueryString("Formid")

                    str = " SELECT SAS_Sponsor.SASR_Code,SAS_Sponsor.SASR_Name,SAS_Sponsor.SASSR_SName,SAS_Sponsor.SASR_Address," +
                          " SAS_Sponsor.SASR_Address1,SAS_Sponsor.SASR_Address2,SAS_Sponsor.SASR_Contact,SAS_Sponsor.SASR_Phone," +
                          " SAS_Sponsor.SASR_Fax,SAS_Sponsor.SASR_Email,SAS_Sponsor.SASR_WebSite,SAS_Sponsor.SASR_Type,SAS_Sponsor.SASR_Desc," +
                          " SAS_Sponsor.SASR_GLAccount,SAS_Sponsor.SABR_Code,SAS_Sponsor.SASR_UpdatedBy,SAS_Sponsor.SASR_UpdatedDtTm," +
                          " SAS_Sponsor.SASR_Status,SAS_Accounts.GLcode,SAS_Accounts.BatchCode,TO_CHAR(SAS_Accounts.BatchDate, 'DD/MM/YYYY') As BatchDate," +
                          " SAS_Accounts.Description,SAS_Accounts.Transamount FROM SAS_Sponsor INNER JOIN SAS_Accounts ON SAS_Sponsor.SASR_Code = SAS_Accounts.CreditRef" +
                          " WHERE SAS_Accounts.BatchCode='" + batchcode + "' "

                    Dim _DataSet As DataSet = _ReportHelper.GetDataSet(str)
                    _DataSet.Tables(0).TableName = "SponsorNotes"

                    Dim _NewCol As New DataColumn("type", GetType(String))
                    _NewCol.DefaultValue = "Sponsor " & category
                    _DataSet.Tables(0).Columns.Add(_NewCol)

                    Dim s As String = Server.MapPath("~/xml/SponsorNotes.xml")
                    _DataSet.WriteXml(s)

                    If _DataSet.Tables(0).Rows.Count = 0 Then
                        Response.Write("No Record Found")
                        Return
                    End If

                    MyReportDocument.Load(Server.MapPath("~/GroupReport/RptSponsorCreditNote.rpt"))
                    MyReportDocument.SetDataSource(_DataSet)
                    Session("reportobject") = MyReportDocument
                    CrystalReportViewer1.ReportSource = MyReportDocument
                    CrystalReportViewer1.DataBind()
                    MyReportDocument.Refresh()
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
    End Sub

End Class
