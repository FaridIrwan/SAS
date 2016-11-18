Imports System.Data
Imports System.Data.SqlClient
Imports System.Web.Configuration
Imports CrystalDecisions.CrystalReports.Engine
Imports CrystalDecisions.Shared

Partial Class ReportSponsorLedgerViewer
    Inherits System.Web.UI.Page
    Private MyReportDocument As New ReportDocument

#Region "Global Declarations "

    Private _ReportHelper As New ReportHelper

#End Region

#Region "Page Load Starting"

    'Added by Hafiz @ 29/2/2016
    'Sponsor Ledger Report Viewer

    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init

        Try

            If Not IsPostBack Then

                Dim sortbyvalue As String = Nothing
                Dim status As String = Nothing
                Dim datecondition As String = Nothing
                Dim str As String = Nothing

                If Session("sponsorCode") <> Nothing Then

                    'modified by Hafiz @ 21/4/2016D:\Workspace\SAS\SAS4.0\Presentation\GroupReport\ReportSponsorLedgerViewer.aspx.vb
                    str = " SELECT batchcode,transcode,creditref,description,transamount,to_char(transdate, 'DD/MM/YYYY') AS Date,subtype,"
                    str += " poststatus,transtype,category,SAS_Sponsor.SASR_Code,SAS_Sponsor.SASR_Name,SAS_Sponsor.SASR_Address,SAS_Sponsor.SASR_Address1,"
                    str += " SAS_Sponsor.SASR_Address2,SAS_Sponsor.SASR_Status"
                    str += " FROM SAS_Accounts"
                    str += " INNER JOIN SAS_Sponsor ON SAS_Accounts.CreditRef = SAS_Sponsor.SASR_Code"
                    str += " WHERE CreditRef = '" + Session("sponsorCode") + "'"
                    str += " and SubType ='Sponsor' and PostStatus ='Posted' "

                    str += " UNION"

                    str += " SELECT batchcode,transcode,creditref1,description,transamount,to_char(transdate, 'DD/MM/YYYY') AS Date,subtype,"
                    str += " poststatus,transtype,category,SAS_Sponsor.SASR_Code, SAS_Sponsor.SASR_Name, SAS_Sponsor.SASR_Address, SAS_Sponsor.SASR_Address1,"
                    str += " SAS_Sponsor.SASR_Address2, SAS_Sponsor.SASR_Status"
                    str += " FROM (SELECT batchcode,transcode,creditref1,description,transamount,transdate,subtype,poststatus,transtype,category, ROW_NUMBER() "
                    str += " OVER (PARTITION BY batchcode ORDER BY batchcode) rn FROM sas_sponsorinvoice) sas_sponsorinvoice"
                    str += " INNER JOIN SAS_Sponsor ON sas_sponsorinvoice.creditref1 = SAS_Sponsor.SASR_Code       "
                    str += " WHERE creditref1 = '" + Session("sponsorCode") + "' "
                    str += " and SubType ='Sponsor'and PostStatus ='Posted' and rn = 1 "
                    str += " order by Date"

                End If

                Dim _DataSet As DataSet = _ReportHelper.GetDataSet(str)
                _DataSet.Tables(0).TableName = "Table"

                Dim _DataTable As DataTable = _DataSet.Tables(0)

                'report XML - start
                Dim s As String = Server.MapPath("~/xml/ReportSponsorLedgerViewer.xml")
                _DataSet.WriteXml(s)
                'report XML - end

                'Records Checking
                If _DataSet.Tables(0).Rows.Count = 0 Then
                    Response.Write("No Record Found")

                Else

                    'Report Loading 
                    MyReportDocument.Load(Server.MapPath("~/GroupReport/ReportSponsorLedgerViewer.rpt"))

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

    End Sub
End Class
