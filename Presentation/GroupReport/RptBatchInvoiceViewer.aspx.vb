Imports System.Data
Imports System.Data.SqlClient
Imports System.Web.Configuration
Imports CrystalDecisions.CrystalReports.Engine

Partial Class GroupReport_RptBatchInvoiceViewer
    Inherits System.Web.UI.Page


    Private MyReportDocument As New ReportDocument
    Private _ReportHelper As New ReportHelper

    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        Try
            Dim BatchCode As String = ""
            Dim Category As String = ""
            If Not IsPostBack Then
                If Request.QueryString("batchNo") Is Nothing Then
                    Response.Write("Load Report Failed. No Batchcode Selected")
                    Return
                Else
                    BatchCode = Request.QueryString("batchNo")
                    Category = Request.QueryString("Formid")
                    Dim str As String = Nothing
                    str = "SELECT sas_student.sasi_matricno, sas_student.sasi_name, sas_student.sasi_cursemyr, SAS_FeeTypes.SAFT_Code, SAS_FeeTypes.SAFT_Desc, SAS_FeeTypes.SAFT_FeeType, SAS_FeeTypes.SAFT_Hostel," +
                            " SAS_FeeTypes.SAFT_Priority, SAS_FeeTypes.SAFT_Remarks, SAS_FeeTypes.SAFT_GLCode, SAS_FeeTypes.SAFT_Status,SAS_AccountsDetails.TransAmount," +
                            " SAS_AccountsDetails.RefCode, SAS_AccountsDetails.TransID, SAS_AccountsDetails.TransTempCode,SAS_AccountsDetails.TransCode," +
                            " SAS_FeeTypes.saft_taxmode, SAS_AccountsDetails.TaxAmount, SAS_AccountsDetails.Tax, SAS_AccountsDetails.internal_use " +
                            " FROM sas_accounts inner join SAS_AccountsDetails  on " +
                            " sas_accounts.transcode = SAS_AccountsDetails.transcode and sas_accounts.transtempcode = SAS_AccountsDetails.transtempcode " +
                            " INNER JOIN SAS_FeeTypes ON SAS_AccountsDetails.RefCode = SAS_FeeTypes.SAFT_Code " +
                             " inner join sas_student on sas_student.sasi_matricno = sas_accounts.creditref" +
                             " WHERE batchcode = '" + BatchCode + "'"
                    str += " order by sas_student.sasi_matricno"

                    Dim _DataSet As DataSet = _ReportHelper.GetDataSet(str)
                    _DataSet.Tables(0).TableName = "BatchInvoice"

                    Dim _result As New DataSet()
                    _result.Tables.Add("Category")
                    _result.Tables("Category").Columns.Add("Type")
                    Dim newRow As DataRow = _result.Tables("Category").NewRow()
                    newRow("Type") = Category
                    _result.Tables("Category").Rows.Add(newRow)

                    _DataSet.Tables.Add(_result.Tables(0).Copy())

                    Dim s As String = Server.MapPath("~/xml/BatchInvoice.xml")
                    _DataSet.WriteXml(s)

                    If _DataSet.Tables(0).Rows.Count = 0 Then
                        Response.Write("No Record Found")
                        Return
                    End If
                    MyReportDocument.Load(Server.MapPath("~/GroupReport/RptBatchInvoice.rpt"))
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
End Class
