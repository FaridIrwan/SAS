Imports HTS.SAS.Entities
Imports HTS.SAS.BusinessObjects
Imports System.Data
Imports System.Data.SqlClient
Imports System.Web.Configuration
Imports System.Collections.Generic
Imports CrystalDecisions.CrystalReports.Engine

Partial Class RptLaporanPengkreditan
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

                Dim num As String = Session("autonum")
                Dim constr As String = Nothing
                Dim str As String = Nothing
               
                str = " select sd.STD_TarikhTransaksi STD_TarikhTransaksi, ss.SASI_MatricNo STD_NoPelajar, "
                str += " sd.STD_NamaPelajar STD_NamaPelajar, sd.STD_NoIC STD_NoIC, sd.STD_NoAkaunPelajar STD_NoAkaunPelajar, "
                str += " sd.STD_StatusBayaran STD_StatusBayaran,sd.STD_AmaunWarran STD_AmaunWarran, "
                str += " sd.STD_AmaunPotongan STD_AmaunPotongan,sd.STD_NilaiBersih STD_NilaiBersih "
                str += " from SAS_SucceedTransactionHeader st, SAS_SucceedTransactionDetails sd, SAS_Student ss "
                str += " where st.STH_AutoNumber = sd.STH_AutoNum_H and ss.SASI_ICNo = sd.STD_NoIC  and st.STH_AutoNumber = '" & num & "'"


                'Author			: Anil Kumar - T-Melmax Sdn Bhd
                'Created Date	: 20/05/2015

                'DataSet Strating
                Dim _DataSet As DataSet = _ReportHelper.GetDataSet(str)

                'Report XML Loading


                Dim s As String = Server.MapPath("xml\LaporanPengkreditan.xml")

                _DataSet.WriteXml(s)

                'Report XML Ended

                'Records Checking

                If _DataSet.Tables(0).Rows.Count = 0 Then

                    Response.Write("No Record Found")
                 

                Else

                    'Report Loading

                    MyReportDocument.Load(Server.MapPath("~\ProcessReport\LaporanPengkreditan.rpt"))
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