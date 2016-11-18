Imports System.Data
Imports System.Data.SqlClient
Imports System.Web.Configuration
Imports CrystalDecisions.CrystalReports.Engine

Partial Class RptRegistrationPaymentViewer
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

                Dim str As String = Nothing

                str = "SELECT SS.SASI_MatricNo, SS.SASI_Name, "
                str += "SS.SASI_Faculty || ' - ' || SF.SAFC_Desc AS Faculty, "
                str += "SS.SASI_PgId || ' - ' || SP.SAPG_ProgramBM Program, "
                str += "to_char(SA.TransAmount, '99999.99') Amount, "
                str += "SASS_Code = CASE WHEN SS.SASS_Code = 'PA' THEN 'Pelajar Aktif' WHEN SS.SASS_Code <> 'PA' THEN 'Pelajar Tidak Aktif' End, "
                str += "SS.SASI_CurSemYr Semester "
                str += "FROM SAS_Accounts SA "
                str += "LEFT JOIN SAS_Student SS ON SA.CreditRef = SS.SASI_MatricNo "
                str += "LEFT JOIN SAS_Program SP ON SS.SASI_PgId = SP.SAPG_Code "
                str += "LEFT JOIN SAS_Faculty SF ON SS.SASI_Faculty = SF.SAFC_Code "
                str += "WHERE Category = N'Receipt' AND SubType = N'Student' "
                str += "AND BankCode = 'BIMB2' "
                str += "AND SS.SASI_CurSemYr = '" & Session("Semester") & "' "

                'Check for Faculty
                If Not String.IsNullOrEmpty(Session("Faculty")) And Session("Faculty") <> "" Then
                    str += "AND SS.SASI_Faculty = '" & Session("Faculty") & "' "
                End If

                'Check for Program
                If Not String.IsNullOrEmpty(Session("Program")) And Session("Program") <> "" Then
                    str += "AND SS.SASI_PgId = '" & Session("Program") & "' "
                End If

                'Check for Status
                If Session("Status") = "Active" Then
                    str += "AND SS.SASS_Code = 'PA' "
                Else
                    str += "AND SS.SASS_Code <> 'PA' "
                End If

                'Author			: Anil Kumar - T-Melmax Sdn Bhd
                'Created Date	: 20/05/2015

                'DataSet Strating
                Dim _DataSet As DataSet = _ReportHelper.GetDataSet(str)

                'Report XML Loading

                Dim s As String = Server.MapPath("~/xml/RegistrationPayment.xml")
                _DataSet.WriteXml(s)

                'Report XML Ended

                'Records Checking

                If _DataSet.Tables(0).Rows.Count = 0 Then
                    Response.Write("No Record Found")

                Else

                    'Report Loading 
                    'MyReportDocument.Load(Server.MapPath("RptStuAgeingCRFinal.rpt"))
                    MyReportDocument.Load(Server.MapPath("~/GroupReport/RptRegistrationPayment.rpt"))
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


