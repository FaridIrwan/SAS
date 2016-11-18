Imports System.Data
Imports System.Data.SqlClient
Imports System.Web.Configuration
Imports CrystalDecisions.CrystalReports.Engine
Imports CrystalDecisions.Shared

Partial Class RptStuLedgerCRForm
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

                    sortbyvalue = " Order By SAS_Student.SASI_PgId, SAS_Student.SASI_Name, SAS_Accounts.TransDate "

                Else

                    sortbyvalue = " Order By " + Session("sortby") + ", SAS_Accounts.TransDate "

                End If

                If Session("status") = Nothing Or Session("status") = -1 Then

                    status = "t'"
                    status += " or SAS_Student.SASI_StatusRec = 'f"

                Else

                    status = Session("status")

                End If


                If Request.QueryString("fdate") = "0" Or Request.QueryString("tdate") = "0" Or Request.QueryString("fdate") Is Nothing Or Request.QueryString("tdate") Is Nothing Then

                    datecondition = ""

                Else

                    datef = Request.QueryString("fdate")
                    datet = Request.QueryString("tdate")
                    Dim datelen As Integer = Len(datef)
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
                str = "SELECT SAS_Accounts.CreditRef,SAS_Student.SASI_Name, SAS_Student.SASI_PgId, SAS_Program.SAPG_Program, SAS_Student.SASI_Passport, "
                str += " SAS_Student.SASI_Add1, SAS_Student.SASI_Add2, SAS_Student.SASI_Add3, SAS_Student.SASI_City, SAS_Student.SASS_Code,SAS_Accounts.transtype, "
                str += " to_char(SAS_Accounts.TransDate, 'DD/MM/YYYY') as date1, SAS_Accounts.TransCode, SAS_Accounts.Description, SAS_Accounts.Category, "
                str += " SAS_Accounts.TransAmount, SAS_Accounts.SubType "
                str += " FROM SAS_Program, SAS_Accounts INNER JOIN "
                str += " SAS_Student ON SAS_Accounts.CreditRef = SAS_Student.SASI_MatricNo "
                str += "WHERE SAS_Program.SAPG_Code = SAS_Student.SASI_PgId and SAS_Accounts.SubType = 'Student' and SAS_Accounts.PostStatus = 'Posted' and SAS_Student.SASI_StatusRec = '" + status + "'"
                str += datecondition + " "
                str += ""
                str += sortbyvalue

                'str = "SELECT     SAS_Accounts.CreditRef,SAS_Student.SASI_Name, SAS_Student.SASI_PgId, SAS_Program.SAPG_Program, SAS_Student.SASI_Passport, "
                'str += " SAS_Student.SASI_Add1, SAS_Student.SASI_Add2, SAS_Student.SASI_Add3, SAS_Student.SASI_City, SAS_Student.SASS_Code,SAS_Accounts.transtype, "
                'str += " CONVERT(VARCHAR(10),SAS_Accounts.TransDate,105) as Date1, SAS_Accounts.TransCode, SAS_Accounts.Description, SAS_Accounts.Category, "
                'str += " SAS_Accounts.TransAmount, SAS_Accounts.SubType "
                'str += " FROM SAS_Program, SAS_Accounts INNER JOIN "
                'str += " SAS_Student ON SAS_Accounts.CreditRef = SAS_Student.SASI_MatricNo "
                'str += "WHERE SAS_Program.SAPG_Code = SAS_Student.SASI_PgId and SAS_Accounts.SubType = 'Student' and SAS_Accounts.PostStatus = 'Posted' and SAS_Student.SASI_StatusRec like '" + status + "'"
                'str += datecondition + " "
                'str += ""
                'str += sortbyvalue


                'Author			: Anil Kumar - T-Melmax Sdn Bhd
                'Created Date	: 20/05/2015

                'DataSet Strating
                Dim _DataSet As DataSet = _ReportHelper.GetDataSet(str)

                'DataTable Strating
                'Dim _DataTable As DataTable = _DataSet.Tables(0)
                _DataSet.Tables(0).TableName = "studledger"

                'Report AFCReport Loading XML

                Dim s As String = Server.MapPath("~/xml/StudLedger.xml")

                _DataSet.WriteXml(s)

                'Report AFCReport Ended XML

                'Records Checking

                If _DataSet.Tables(0).Rows.Count = 0 Then
                    Response.Write("No Record Found")
                Else

                    'Report Loading 

                    MyReportDocument.Load(Server.MapPath("/GroupReport/RptLedgerStudent.rpt"))
                    MyReportDocument.SetDataSource(_DataSet)
                    Session("reportobject") = MyReportDocument

                    CrystalReportViewer1.ReportSource = MyReportDocument
                    ' CrystalDecisions.ReportAppServer.
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
