Imports System.Data
Imports System.Data.SqlClient
Imports System.Web.Configuration
Imports CrystalDecisions.CrystalReports.Engine
Partial Class AFCReportForm
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
        'Modified by Hafiz @ 9/11/2016

        Try

            Dim bat As String = Nothing
            Dim batrpt As String = Nothing
            Dim AFc As String = Nothing
            Dim str As String = Nothing
            Dim dt As DataTable

            If Not IsPostBack Then

                bat = Request.QueryString("bat")  '"B000000000000752" 
                Dim check As String() = bat.Split("|")
                bat = check(0)
                batrpt = check(1) '"0" 

                str = "select DISTINCT sa.TransID TransID,sa.BatchCode BatchCode,sa.CreditRef CreditRef,st.SASI_Name SASI_Name,"
                str += " sa.Description Description,sa.TransAmount TotalTrans,sp.SAPG_Program SAPG_Program,sf.SAFC_Desc "
                str += " SAFC_Desc from SAS_AFC af inner join SAS_AFCDetails ad on af.TransCode = ad.TransCode inner join "
                str += " SAS_Accounts sa on sa.BatchCode = af.BatchCode inner join SAS_Student st on st.SASI_MatricNo = "
                str += " sa.CreditRef inner join SAS_Program sp on sp.SAPG_Code = st.SASI_PgId inner join SAS_Faculty sf "
                str += " on sf.SAFC_Code = st.SASI_Faculty"
                str += " where sa.BatchCode in( " + bat + ")"

                'Author			: Anil Kumar - T-Melmax Sdn Bhd
                'Created Date	: 20/05/2015

                'DataSet Strating
                Dim _DataSet As DataSet = _ReportHelper.GetDataSet(str)
                _DataSet.Tables(0).TableName = "Table"

                'Report XML Loading

                dt = _DataSet.Tables(0)

                dt = dt.DefaultView.ToTable(True, "Description")

                For i As Integer = 0 To dt.Rows.Count - 1

                    AFc += dt.Rows(i)("Description") & ","

                Next
                If (AFc.Contains(",")) Then

                    AFc = AFc.TrimEnd(",")

                End If


                'Report XML Loading
                Dim s As String = Server.MapPath("xml\AFCReport.xml")
                _DataSet.WriteXml(s)

                'Report XML Ended

                'Records Checking

                If _DataSet.Tables(0).Rows.Count = 0 Then
                    Response.Write("No Record Found")
                    Return
                End If

                'Report Loading 

                '_DataSet.ReadXml(s)
                MyReportDocument.Load(Server.MapPath("ProcessReport\RptAFC.rpt"))
                MyReportDocument.SetDataSource(_DataSet)
                Session("reportobject") = MyReportDocument
                CrystalReportViewer1.ReportSource = MyReportDocument
                MyReportDocument.DataDefinition.FormulaFields("UnboundString1").Text = "'" & batrpt & "'"
                MyReportDocument.DataDefinition.FormulaFields("UnboundString2").Text = "'" & AFc & "'"
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
