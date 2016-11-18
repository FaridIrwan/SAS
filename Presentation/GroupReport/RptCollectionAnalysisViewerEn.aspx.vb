Imports System.Data
Imports System.Data.SqlClient
Imports System.Web.Configuration
Imports CrystalDecisions.CrystalReports.Engine

Partial Class RptCollectionAnalysisViewerEn
    Inherits System.Web.UI.Page
    Private MyReportDocument As ReportDocument
#Region "Global Declarations "

    Private _ReportHelper As New ReportHelper

#End Region

#Region "Page Load Starting  "

    'Author			: Anil Kumar - T-Melmax Sdn Bhd
    'Purpose		: Get the AFCReport Report
    'Created Date	: 20/05/2015

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Try
            If Not IsPostBack Then

                Dim sortbyvalue As String = Nothing
                Dim status As String = Nothing
                Dim program As String = Nothing
                Dim faculty As String = Nothing
                Dim sponsor As String = Nothing
                Dim datecondition As String = Nothing
                Dim str As String = Nothing

                If Session("sortby") Is Nothing Then

                    sortbyvalue = "Order By SAS_Student.SASI_PgId,SAS_Student.SASI_Name"

                ElseIf Session("sortby") = "SAS_Accounts.CreditRef" Then

                    sortbyvalue = " Order By SAS_Student.SASI_PgId, " + Session("sortby")

                ElseIf Session("sortby") = "SAS_Student.SASI_Name" Then

                    sortbyvalue = " Order By SAS_Student.SASI_PgId," + Session("sortby")

                ElseIf Session("sortby") = "SAS_Accounts.BankCode" Then

                    sortbyvalue = " Order By SAS_Student.SASI_PgId," + Session("sortby")

                End If

                If Session("status") Is Nothing Then

                    status = "t'"
                    status += " or SAS_Student.SASI_StatusRec = 'f"

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

                If Request.QueryString("fdate") = "0" Or Request.QueryString("tdate") = "0" Then

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

                If Not sponsor = "%" Then
                    str = " SELECT SAS_Accounts.TransCode, to_char(SAS_Accounts.TransDate,'DD/MM/YYYY') as date, "
                    str += "SAS_Accounts.BankCode, SAS_BankDetails.SABD_Desc, SAS_Program.SAPG_Program, "
                    str += "SAS_Student.SASI_Add1, SAS_Student.SASI_Add2,SAS_Student.SASI_Add3, SAS_Student.SASI_City,"
                    str += "SAS_BankDetails.SABD_ACCode,  SAS_Accounts.CreditRef, "
                    str += "SAS_Student.SASI_Name, SAS_Student.SASI_Add1, SAS_Student.SASI_Add2, SAS_Student.SASI_Add3, SAS_Student.SASI_City, SAS_Accounts.Description, "
                    str += "SAS_Accounts.TransAmount, SAS_Student.SASI_Faculty,  "
                    str += " SAS_Student.SASI_PgId, SAS_StudentSpon.SASS_Sponsor "
                    str += "FROM  SAS_Program, SAS_Accounts INNER JOIN SAS_Student ON SAS_Accounts.CreditRef = SAS_Student.SASI_MatricNo "
                    str += " INNER JOIN  SAS_BankDetails ON SAS_Accounts.BankCode = SAS_BankDetails.SABD_Code "
                    str += " INNER JOIN SAS_StudentSpon ON SAS_Student.SASI_MatricNo = SAS_StudentSpon.SASI_MatricNo "
                    str += " where SAS_Program.SAPG_Code = SAS_Student.SASI_PgId and SAS_Accounts.TransType ='Debit' and SAS_Student.SASI_StatusRec = '" + status + "' and  SAS_Student.SASI_PgId like '" + program + "'"
                    str += " and  SAS_Student.SASI_Faculty like '" + faculty + "' "
                    str += " and poststatus='Posted' and SAS_StudentSpon.SASS_Sponsor like '" + sponsor + "'"
                    str += datecondition + " "
                    str += sortbyvalue

                Else

                    str = " SELECT SAS_Accounts.TransCode,  to_char(SAS_Accounts.TransDate,'DD/MM/YYYY') as date, "
                    str += "SAS_Accounts.BankCode, SAS_BankDetails.SABD_Desc, SAS_Program.SAPG_Program, "
                    str += "SAS_Student.SASI_Add1, SAS_Student.SASI_Add2, SAS_Student.SASI_Add3, SAS_Student.SASI_City,"
                    str += "SAS_BankDetails.SABD_ACCode,  SAS_Accounts.CreditRef, "
                    str += "SAS_Student.SASI_Name, SAS_Accounts.Description, "
                    str += "SAS_Accounts.TransAmount, SAS_Student.SASI_Faculty,  "
                    str += " SAS_Student.SASI_PgId "
                    str += " FROM SAS_Program, SAS_Accounts INNER JOIN SAS_Student ON SAS_Accounts.CreditRef = SAS_Student.SASI_MatricNo "
                    str += " INNER JOIN  SAS_BankDetails ON SAS_Accounts.BankCode = SAS_BankDetails.SABD_Code "
                    str += " where SAS_Program.SAPG_Code = SAS_Student.SASI_PgId and SAS_Accounts.TransType ='Debit' and SAS_Student.SASI_StatusRec ='" + status + "' and  SAS_Student.SASI_PgId like '" + program + "'"
                    str += " and  SAS_Student.SASI_Faculty like '" + faculty + "' "
                    str += " and poststatus='Posted'"
                    str += datecondition + " "
                    str += sortbyvalue

                    '    str = " SELECT SAS_Accounts.TransCode, CONVERT(VARCHAR(10),sas_accounts.transdate,105) as date, "
                    '    str += "SAS_Accounts.BankCode, SAS_BankDetails.SABD_Desc, dbo.SAS_Program.SAPG_Program, "
                    '    str += "SAS_Student.SASI_Add1, SAS_Student.SASI_Add2,SAS_Student.SASI_Add3, SAS_Student.SASI_City,"
                    '    str += "SAS_BankDetails.SABD_ACCode,  SAS_Accounts.CreditRef, "
                    '    str += "SAS_Student.SASI_Name, SAS_Student.SASI_Add1, SAS_Student.SASI_Add2, SAS_Student.SASI_Add3, SAS_Student.SASI_City, SAS_Accounts.Description, "
                    '    str += "SAS_Accounts.TransAmount, SAS_Student.SASI_Faculty,  "
                    '    str += " SAS_Student.SASI_PgId, SAS_StudentSpon.SASS_Sponsor "
                    '    str += "FROM  dbo.SAS_Program, SAS_Accounts INNER JOIN SAS_Student ON SAS_Accounts.CreditRef = SAS_Student.SASI_MatricNo "
                    '    str += " INNER JOIN  SAS_BankDetails ON SAS_Accounts.BankCode = SAS_BankDetails.SABD_Code "
                    '    str += " INNER JOIN SAS_StudentSpon ON SAS_Student.SASI_MatricNo = SAS_StudentSpon.SASI_MatricNo "
                    '    str += " where dbo.SAS_Program.SAPG_Code = dbo.SAS_Student.SASI_PgId and SAS_Accounts.TransType ='Debit' and dbo.SAS_Student.SASI_StatusRec like '" + status + "' and  dbo.SAS_Student.SASI_PgId like '" + program + "'"
                    '    str += " and  dbo.SAS_Student.SASI_Faculty like '" + faculty + "' "
                    '    str += " and poststatus='Posted' and dbo.SAS_StudentSpon.SASS_Sponsor like '" + sponsor + "'"
                    '    str += datecondition + " "
                    '    str += sortbyvalue

                    'Else

                    '    str = " SELECT SAS_Accounts.TransCode, CONVERT(VARCHAR(10),sas_accounts.transdate,105) as date, "
                    '    str += "SAS_Accounts.BankCode, SAS_BankDetails.SABD_Desc, dbo.SAS_Program.SAPG_Program, "
                    '    str += "SAS_Student.SASI_Add1, SAS_Student.SASI_Add2, SAS_Student.SASI_Add3, SAS_Student.SASI_City,"
                    '    str += "SAS_BankDetails.SABD_ACCode,  SAS_Accounts.CreditRef, "
                    '    str += "SAS_Student.SASI_Name, SAS_Accounts.Description, "
                    '    str += "SAS_Accounts.TransAmount, SAS_Student.SASI_Faculty,  "
                    '    str += " SAS_Student.SASI_PgId "
                    '    str += " FROM dbo.SAS_Program, SAS_Accounts INNER JOIN SAS_Student ON SAS_Accounts.CreditRef = SAS_Student.SASI_MatricNo "
                    '    str += " INNER JOIN  SAS_BankDetails ON SAS_Accounts.BankCode = SAS_BankDetails.SABD_Code "
                    '    str += " where dbo.SAS_Program.SAPG_Code = dbo.SAS_Student.SASI_PgId and SAS_Accounts.TransType ='Debit' and dbo.SAS_Student.SASI_StatusRec like '" + status + "' and  dbo.SAS_Student.SASI_PgId like '" + program + "'"
                    '    str += " and  dbo.SAS_Student.SASI_Faculty like '" + faculty + "' "
                    '    str += " and poststatus='Posted'"
                    '    str += datecondition + " "
                    '    str += sortbyvalue

                End If

                'Author			: Anil Kumar - T-Melmax Sdn Bhd
                'Created Date	: 20/05/2015

                'DataSet Strating

                Dim _DataSet As DataSet = _ReportHelper.GetDataSet(str)
                _DataSet.Tables(0).TableName = "Table"
                'Report CollectionAnalysis Loading XML

                Dim s As String = Server.MapPath("~/xml/CollectionAnalysys.xml")
                _DataSet.WriteXml(s)

                'Records Checking

                If _DataSet.Tables(0).Rows.Count = 0 Then
                    Response.Write("No Record Found")

                Else

                    'Report Loading

                    MyReportDocument.Load(Server.MapPath("~/GroupReport/RptCollectionAnalysisEn.rpt"))
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
