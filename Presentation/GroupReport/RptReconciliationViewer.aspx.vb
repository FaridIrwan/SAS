Imports System.Data
Imports System.Data.SqlClient
Imports System.Web.Configuration
Imports CrystalDecisions.CrystalReports.Engine

Partial Class RptReconciliationViewer
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

                Dim program As String = Nothing
                Dim faculty As String = Nothing
                Dim sponsor As String = Nothing
                Dim datecondition As String = Nothing
                Dim sortbyvalue As String = Nothing
                Dim sortbyvalue1 As String = Nothing
                Dim str As String = Nothing
                Dim s As String = Nothing
                sortbyvalue = "Order By SAS_Student.SASI_Faculty,SAS_Student.SASI_PgId,SAS_Accounts.BankCode"
                sortbyvalue1 = "Order By SAS_Sponsor.SASR_Code, SAS_Accounts.TransDate"

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


                If Session("reconcilation") = 0 Then
                    str = " SELECT  to_char(SAS_Accounts.TransDate,'DD/MM/YYYY') as TransDate,  SAS_Accounts.TransCode, SAS_Accounts.CreditRef, SAS_Accounts.Category, SAS_Accounts.TransAmount,SAS_Program.SAPG_Program,SAS_Faculty.SAFC_Desc,  "
                    str += " SAS_Accounts.TransStatus, SAS_Accounts.BankCode, SAS_BankDetails.SABD_Desc, SAS_Accounts.TransType,  "
                    str += " SAS_Student.SASI_PgId, SAS_Student.SASI_Faculty, SAS_Student.SASI_MatricNo, SAS_Student.SASI_Name "
                    str += " FROM   SAS_Faculty, SAS_Program, SAS_Accounts INNER JOIN "
                    str += " SAS_BankDetails ON SAS_Accounts.BankCode = SAS_BankDetails.SABD_Code INNER JOIN "
                    str += " SAS_Student ON SAS_Accounts.CreditRef = SAS_Student.SASI_MatricNo "
                    str += " where SAS_Faculty.SAFC_Code =SAS_Student.SASI_Faculty and SAS_Program.SAPG_Code = SAS_Student.SASI_PgId and SAS_Student.SASI_PgId like '" + program + "' and SAS_Student.SASI_Faculty like '" + faculty + "'"
                    str += datecondition + " "
                    str += sortbyvalue
                Else
                    str = " SELECT  to_char(SAS_Accounts.TransDate,'DD/MM/YYYY') as TransDate,   SAS_Accounts.TransCode, SAS_Accounts.CreditRef, SAS_Accounts.Category, SAS_Accounts.TransAmount, "
                    str += " SAS_Accounts.TransStatus, SAS_Accounts.Description, SAS_Accounts.BankCode, SAS_BankDetails.SABD_Desc, SAS_Accounts.TransType, "
                    str += " SAS_Sponsor.SASR_Code, SAS_Sponsor.SASR_Name FROM SAS_Accounts INNER JOIN "
                    str += " SAS_BankDetails ON SAS_Accounts.BankCode = SAS_BankDetails.SABD_Code INNER JOIN "
                    str += " SAS_Sponsor ON SAS_Accounts.CreditRef = SAS_Sponsor.SASR_Code "
                    str += " where SAS_Sponsor.SASR_Code like '" + sponsor + "'"
                    str += datecondition + " "
                    str += sortbyvalue1
                    '    str = " SELECT   CONVERT(VARCHAR(10), dbo.SAS_Accounts.TransDate,103) as TransDate,  dbo.SAS_Accounts.TransCode, dbo.SAS_Accounts.CreditRef, dbo.SAS_Accounts.Category, dbo.SAS_Accounts.TransAmount,dbo.SAS_Program.SAPG_Program,dbo.SAS_Faculty.SAFC_Desc,  "
                    '    str += " dbo.SAS_Accounts.TransStatus, dbo.SAS_Accounts.BankCode, dbo.SAS_BankDetails.SABD_Desc, dbo.SAS_Accounts.TransType,  "
                    '    str += " dbo.SAS_Student.SASI_PgId, dbo.SAS_Student.SASI_Faculty, dbo.SAS_Student.SASI_MatricNo, dbo.SAS_Student.SASI_Name "
                    '    str += " FROM   dbo.SAS_Faculty, dbo.SAS_Program, dbo.SAS_Accounts INNER JOIN "
                    '    str += " dbo.SAS_BankDetails ON dbo.SAS_Accounts.BankCode = dbo.SAS_BankDetails.SABD_Code INNER JOIN "
                    '    str += " dbo.SAS_Student ON dbo.SAS_Accounts.CreditRef = dbo.SAS_Student.SASI_MatricNo "
                    '    str += " where dbo.SAS_Faculty.SAFC_Code =dbo.SAS_Student.SASI_Faculty and dbo.SAS_Program.SAPG_Code = dbo.SAS_Student.SASI_PgId and dbo.SAS_Student.SASI_PgId like '" + program + "' and dbo.SAS_Student.SASI_Faculty like '" + faculty + "'"
                    '    str += datecondition + " "
                    '    str += sortbyvalue
                    'Else
                    '    str = " SELECT  CONVERT(VARCHAR(10), dbo.SAS_Accounts.TransDate,103) as TransDate,   dbo.SAS_Accounts.TransCode, dbo.SAS_Accounts.CreditRef, dbo.SAS_Accounts.Category, dbo.SAS_Accounts.TransAmount, "
                    '    str += " dbo.SAS_Accounts.TransStatus, dbo.SAS_Accounts.Description, dbo.SAS_Accounts.BankCode, dbo.SAS_BankDetails.SABD_Desc, dbo.SAS_Accounts.TransType, "
                    '    str += " dbo.SAS_Sponsor.SASR_Code, dbo.SAS_Sponsor.SASR_Name FROM dbo.SAS_Accounts INNER JOIN "
                    '    str += " dbo.SAS_BankDetails ON dbo.SAS_Accounts.BankCode = dbo.SAS_BankDetails.SABD_Code INNER JOIN "
                    '    str += " dbo.SAS_Sponsor ON dbo.SAS_Accounts.CreditRef = dbo.SAS_Sponsor.SASR_Code "
                    '    str += " where dbo.SAS_Sponsor.SASR_Code like '" + sponsor + "'"
                    '    str += datecondition + " "
                    '    str += sortbyvalue1
                End If

                'Author			: Anil Kumar - T-Melmax Sdn Bhd
                'Created Date	: 20/05/2015

                'DataSet Strating
                Dim _DataSet As DataSet = _ReportHelper.GetDataSet(str)
                _DataSet.Tables(0).TableName = "Table"

                'Report XML Loading 

                If Session("reconcilation") = 0 Then

                    s = Server.MapPath("~/xml/ReconciliationStud.xml")
                    _DataSet.WriteXml(s)

                Else
                    s = Server.MapPath("~/xml/ReconciliationSponsor.xml")
                    _DataSet.WriteXml(s)

                End If

                'Report XML Ended 

                'Records Checking

                If _DataSet.Tables(0).Rows.Count = 0 Then
                    Response.Write("No Record Found")
                Else

                    'Report Loading

                    If Session("reconcilation") = 0 Then
                        MyReportDocument.Load(Server.MapPath("~/GroupReport/RptReconciliationEn.rpt"))
                    Else
                        MyReportDocument.Load(Server.MapPath("~/GroupReport/RptReconciliationSponsorEn.rpt"))
                    End If

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
        'MyReportDocument.Close()
        'MyReportDocument.Dispose()
    End Sub
End Class
