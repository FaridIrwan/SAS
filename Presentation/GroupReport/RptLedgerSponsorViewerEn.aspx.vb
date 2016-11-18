Imports System.Data
Imports System.Data.SqlClient
Imports System.Web.Configuration
Imports CrystalDecisions.CrystalReports.Engine

Partial Class RptLedgerSponsorViewerEn
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
        Dim sortbyvalue As String = Nothing
        Dim status As String = Nothing
        Dim datecondition As String = Nothing
        Dim datef As String = Request.QueryString("fdate")
        Dim datet As String = Request.QueryString("tdate")
        Dim str As String = Nothing

        Try

            If Not IsPostBack Then


                If Session("sortby") = Nothing Then

                    sortbyvalue = "Order By SAS_Sponsor.SASR_Name"

                Else

                    sortbyvalue = " Order By " + Session("sortby")

                End If

                If Session("status") = Nothing Or Session("status") = -1 Then

                    status = "t'"
                    status += " or SAS_Sponsor.SASR_Status = 'f"

                Else

                    status = Session("status")

                End If


                If Request.QueryString("fdate") = "0" Or Request.QueryString("tdate") = "0" Or Request.QueryString("fdate") Is Nothing Or Request.QueryString("tdate") Is Nothing Then

                    datecondition = ""

                Else

                    datef = Request.QueryString("fdate")
                    datet = Request.QueryString("tdate")
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
                Str = "SELECT     SAS_Accounts.CreditRef, SAS_Sponsor.SASR_Code, SAS_Sponsor.SASR_Name, SAS_Sponsor.SASR_Address, "
                Str += " SAS_Sponsor.SASR_Address1, SAS_Sponsor.SASR_Address2, SAS_Sponsor.SASR_Status,to_char(SAS_Accounts.transdate, 'DD/MM/YYYY') as Date, "
                Str += " SAS_Accounts.TransCode, SAS_Accounts.Description,SAS_Accounts.transtype, SAS_Accounts.Category, SAS_Accounts.TransAmount "
                Str += " FROM SAS_Accounts INNER JOIN "
                Str += " SAS_Sponsor ON SAS_Accounts.CreditRef = SAS_Sponsor.SASR_Code "
                Str += " WHERE SAS_Accounts.SubType = 'Sponsor'and SAS_Sponsor.SASR_Status = '" + status + "' "
                Str += " and poststatus='Posted' "
                Str += datecondition
                Str += sortbyvalue

                'str = "SELECT     dbo.SAS_Accounts.CreditRef, dbo.SAS_Sponsor.SASR_Code, dbo.SAS_Sponsor.SASR_Name, dbo.SAS_Sponsor.SASR_Address, "
                'str += " dbo.SAS_Sponsor.SASR_Address1, dbo.SAS_Sponsor.SASR_Address2, dbo.SAS_Sponsor.SASR_Status,CONVERT(VARCHAR(10),dbo.SAS_Accounts.TransDate,105) as Date, "
                'str += " dbo.SAS_Accounts.TransCode, dbo.SAS_Accounts.Description,dbo.SAS_Accounts.transtype, dbo.SAS_Accounts.Category, dbo.SAS_Accounts.TransAmount "
                'str += " FROM dbo.SAS_Accounts INNER JOIN "
                'str += " dbo.SAS_Sponsor ON dbo.SAS_Accounts.CreditRef = dbo.SAS_Sponsor.SASR_Code "
                'str += " WHERE dbo.SAS_Accounts.SubType = 'Sponsor'and dbo.SAS_Sponsor.SASR_Status like '" + status + "' "
                'str += " and poststatus='Posted' "
                'str += datecondition
                'str += sortbyvalue


                'Author			: Anil Kumar - T-Melmax Sdn Bhd
                'Created Date	: 20/05/2015

                'DataSet Strating
                Dim _DataSet As DataSet = _ReportHelper.GetDataSet(Str)
                _DataSet.Tables(0).TableName = "Table"
                'DataTable Strating
                Dim _DataTable As DataTable = _DataSet.Tables(0)

                'Report AFCReport Loading XML

                Dim s As String = Server.MapPath("~/xml/SponsorLedger.xml")
                _DataSet.WriteXml(s)

                'Report AFCReport Ended XML

                'Records Checking

                If _DataSet.Tables(0).Rows.Count = 0 Then
                    Response.Write("No Record Found")
                Else

                    'Report Loading            

                    MyReportDocument.Load(Server.MapPath("~/GroupReport/RptLedgerSponsorEn.rpt"))
                    MyReportDocument.SetDataSource(_DataSet)
                    Session("reportobject") = MyReportDocument
                    CrystalReportViewer1.ReportSource = MyReportDocument

                    'If datef <> "" Then

                    '    MyReportDocument.SetParameterValue("p_fdate", datef)
                    '    MyReportDocument.SetParameterValue("p_tdate", datet)

                    'Else

                    '    MyReportDocument.SetParameterValue("p_fdate", "")
                    '    MyReportDocument.SetParameterValue("p_tdate", "")

                    'End If

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
