Imports System.Data
Imports System.Data.SqlClient
Imports System.Web.Configuration
Imports CrystalDecisions.CrystalReports.Engine

Partial Class RptStatementAccountViewerEn
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
                Dim sortbyvalue As String = Nothing
                Dim status As String = Nothing
                Dim program As String = Nothing
                Dim faculty As String = Nothing
                Dim sponsor As String = Nothing
                Dim datecondition As String = Nothing
                Session("reportobject") = Nothing
                Dim str As String = Nothing

                If Session("status") Is Nothing Then

                    status = "%"

                Else

                    status = Session("status")

                End If


                If Session("faculty") Is Nothing Then

                    faculty = "%"

                Else

                    faculty = Session("faculty")

                End If

                If Session("program") Is Nothing Then

                    program = "%"

                Else

                    program = Session("program")

                End If

                If Session("sponsor") Is Nothing Then

                    sponsor = "%"

                Else

                    sponsor = Session("sponsor")

                End If

                If Request.QueryString("fdate") = "0" Or Request.QueryString("tdate") = "0" Or Request.QueryString("fdate") Is Nothing Or Request.QueryString("tdate") Is Nothing Then

                    datecondition = ""

                Else

                    Dim datef As String = Request.QueryString("fdate")
                    Dim datet As String = Request.QueryString("tdate")
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

                    datecondition = " and dbo.SAS_Accounts.TransDate between '" + datefrom + "' and '" + dateto + "'"

                End If

                If Not sponsor = "%" Then
                    str = " SELECT     dbo.SAS_Accounts.CreditRef, dbo.SAS_Student.SASI_MatricNo,"
                    str += " dbo.SAS_Student.SASI_Add1,dbo.SAS_Student.SASI_Add2,dbo.SAS_Student.SASI_Add3,SAS_Student.SASI_City,dbo.SAS_Faculty.SAFC_SName, dbo.SAS_Student.SASI_Postcode,dbo.SAS_Faculty.SAFC_Desc, "
                    str += " dbo.SAS_Student.SASI_Name, dbo.SAS_Accounts.TransCode,dbo.SAS_Accounts.TransType, "
                    str += " CONVERT(VARCHAR(10), dbo.SAS_Accounts.TransDate,103) as TransDate, dbo.SAS_Accounts.Description, dbo.SAS_Student.SASI_ID, dbo.SAS_Student.SASI_Intake, "
                    str += " dbo.SAS_Program.SAPG_Program, dbo.SAS_Accounts.TransAmount "
                    str += " FROM dbo.SAS_Accounts INNER JOIN"
                    str += " dbo.SAS_Student ON dbo.SAS_Accounts.CreditRef = dbo.SAS_Student.SASI_MatricNo INNER JOIN "
                    str += " dbo.SAS_Program ON dbo.SAS_Student.SASI_PgId = dbo.SAS_Program.SAPG_Code INNER JOIN "
                    str += " dbo.SAS_Faculty ON dbo.SAS_Student.SASI_Faculty = dbo.SAS_Faculty.SAFC_Code "
                    str += " INNER JOIN SAS_StudentSpon ON SAS_Student.SASI_MatricNo = SAS_StudentSpon.SASI_MatricNo "
                    str += " where dbo.SAS_Accounts.poststatus = 'Posted' and dbo.SAS_Student.SASI_StatusRec like '" + status + "' and dbo.SAS_Student.SASI_PgId like '" + program + "'"
                    str += " and  dbo.SAS_Student.SASI_Faculty like '" + faculty + "' and dbo.SAS_StudentSpon.SASS_Sponsor like '" + sponsor + "'"
                    str += datecondition + " "
                    str += sortbyvalue

                Else

                    str = " SELECT dbo.SAS_Accounts.CreditRef, dbo.SAS_Student.SASI_MatricNo,"
                    str += " dbo.SAS_Student.SASI_Add1,dbo.SAS_Student.SASI_Add2,dbo.SAS_Student.SASI_Add3,SAS_Student.SASI_City,dbo.SAS_Faculty.SAFC_SName, dbo.SAS_Student.SASI_Postcode, "
                    str += " dbo.SAS_Student.SASI_Name, dbo.SAS_Accounts.TransCode,dbo.SAS_Accounts.TransType, "
                    str += " CONVERT(VARCHAR(10), dbo.SAS_Accounts.TransDate,103) as TransDate, dbo.SAS_Accounts.Description, dbo.SAS_Student.SASI_ID, dbo.SAS_Student.SASI_Intake, "
                    str += " dbo.SAS_Program.SAPG_Program, dbo.SAS_Accounts.TransAmount "
                    str += " FROM dbo.SAS_Accounts INNER JOIN"
                    str += " dbo.SAS_Student ON dbo.SAS_Accounts.CreditRef = dbo.SAS_Student.SASI_MatricNo INNER JOIN "
                    str += " dbo.SAS_Program ON dbo.SAS_Student.SASI_PgId = dbo.SAS_Program.SAPG_Code INNER JOIN "
                    str += " dbo.SAS_Faculty ON dbo.SAS_Student.SASI_Faculty = dbo.SAS_Faculty.SAFC_Code  "
                    str += " where dbo.SAS_Accounts.poststatus = 'Posted' and dbo.SAS_Student.SASI_StatusRec like '" + status + "' and dbo.SAS_Student.SASI_PgId like '" + program + "'"
                    str += " and  dbo.SAS_Student.SASI_Faculty like '" + faculty + "' "
                    str += datecondition + " "
                    str += sortbyvalue

                End If

                'Author			: Anil Kumar - T-Melmax Sdn Bhd
                'Created Date	: 20/05/2015

                'DataSet Strating
                Dim _DataSet As DataSet = _ReportHelper.GetDataSet(str)

                'Report XML Loading

                Dim s As String = Server.MapPath("~\xml\statementAC.xml")
                _DataSet.WriteXml(s)

                'Report XML Ended

                'Records Checking

                If _DataSet.Tables(0).Rows.Count = 0 Then
                    Response.Write("No Record Found")
                    Return
                End If

                'Report Loading

                MyReportDocument.Load(Server.MapPath("RptStatementAccountEn.rpt"))
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
