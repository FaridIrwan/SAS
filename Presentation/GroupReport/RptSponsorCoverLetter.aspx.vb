#Region "Namespaces "
Imports System.Data
Imports System.Data.SqlClient
Imports System.Web.Configuration
Imports CrystalDecisions.CrystalReports.Engine
#End Region
Partial Class GroupReport_RptSponsorCoverLetter
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

        If Not IsPostBack Then

            Dim sortbyvalue As String = Nothing
         
            Dim str As String = Nothing

            str = "SELECT SAS_Student.SASI_MatricNo, SAS_Student.SASI_Name, SAS_Student.SASI_ICNo, SAS_Student.SASI_Passport, SAS_Student.SASI_CurSemYr, SAS_Sponsor.SASR_Name, "
            str += "SAS_Sponsor.SASR_Address, SAS_Sponsor.SASR_Address1, SAS_Sponsor.SASR_Address2, SAS_Program.SAPG_Program, SAS_FeeTypes.SAFT_Desc, "
            str += "SAS_FeeStrAmount.SAFA_Amount"
            str += " FROM SAS_FeeStrAmount INNER JOIN"
            str += " SAS_FeeStruct ON SAS_FeeStrAmount.SAFS_Code = SAS_FeeStruct.SAFS_Code INNER JOIN"
            str += " SAS_FeeStrDetails ON SAS_FeeStruct.SAFS_Code = SAS_FeeStrDetails.SAFS_Code INNER JOIN"
            str += " SAS_FeeTypes ON SAS_FeeStrDetails.SAFT_Code = SAS_FeeTypes.SAFT_Code INNER JOIN"
            str += " SAS_SponsorInvoiceDetails INNER JOIN"
            str += " SAS_SponsorInvoice ON SAS_SponsorInvoiceDetails.TransID = SAS_SponsorInvoice.TransID INNER JOIN"
            str += " SAS_Sponsor ON SAS_SponsorInvoice.CreditRef1 = SAS_Sponsor.SASR_Code INNER JOIN"
            str += " SAS_Student ON SAS_SponsorInvoice.CreditRef = SAS_Student.SASI_MatricNo INNER JOIN"
            str += " SAS_Program ON SAS_Student.SASI_PgId = SAS_Program.SAPG_Code ON SAS_FeeStruct.SAPG_Code = SAS_Program.SAPG_Code"

            'Author			: Anil Kumar - T-Melmax Sdn Bhd
            'Created Date	: 20/05/2015
            '-----------------------------------
            'DataSet Strating
            Dim _DataSet As DataSet = _ReportHelper.GetDataSet(str)


            'Report AFCReport Loading XML

            Dim s As String = Server.MapPath("~/xml/studentsponsercoverletter.xml")
            _DataSet.WriteXml(s)

            'Report AFCReport Ended XML

            'Records Checking

            If _DataSet.Tables(0).Rows.Count = 0 Then
                Response.Write("No Record Found")

            End If

            'Report Loading 

            MyReportDocument.Load(Server.MapPath("~/GroupReport/RptSponserCoverLetter.rpt"))
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
    End Sub

#End Region

    Protected Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Unload
        'MyReportDocument.Close()
        'MyReportDocument.Dispose()
    End Sub
End Class
