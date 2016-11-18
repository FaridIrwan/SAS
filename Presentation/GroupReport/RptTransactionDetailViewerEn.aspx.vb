Imports System.Data
Imports System.Data.SqlClient
Imports System.Web.Configuration
Imports CrystalDecisions.CrystalReports.Engine

Partial Class RptTransactionDetailViewerEn
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
                Dim str As String = Nothing

                If Session("sortby") Is Nothing Then

                    sortbyvalue = " Order By SAS_Student.SASI_Faculty,SAS_Student.SASI_PgId,SAS_Student.SASI_Name,SAS_Accounts.TransDate"

                ElseIf Session("sortby") = "SAS_Student.SASI_MatricNo" Then

                    sortbyvalue = " Order By SAS_Student.SASI_Faculty,SAS_Student.SASI_PgId, " + Session("sortby") + ",SAS_Accounts.TransDate"

                ElseIf Session("sortby") = "SAS_Student.SASI_Name" Then

                    sortbyvalue = " Order By SAS_Student.SASI_Faculty,SAS_Student.SASI_PgId," + Session("sortby") + ",SAS_Accounts.TransDate"

                End If

                status = "PA"

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
                    datecondition = " and SAS_Accounts.TransDate between '" + datefrom + "' and '" + dateto + "'"

                End If

                If Not sponsor = "%" Then

                    str = " SELECT SAS_Student.SASI_Name, SAS_Student.SASI_ID,to_char(SAS_Accounts.TransDate,'DD/MM/YYYY') as TransDate  , SAS_Accounts.TransCode, SAS_Accounts.Description,SAS_Program.SAPG_Program,SAS_Faculty.SAFC_Desc, "
                    str += " SAS_Accounts.TransAmount, SAS_Student.SASI_MatricNo, SAS_StudentSpon.SASS_Sponsor,SAS_Student.SASI_PgId, SAS_Faculty.SAFC_SName, SAS_Accounts.TransType "
                    str += " FROM SAS_Program, SAS_Accounts INNER JOIN "
                    str += " SAS_Student ON SAS_Accounts.CreditRef = SAS_Student.SASI_MatricNo INNER JOIN "
                    str += " SAS_StudentSpon ON SAS_Student.SASI_MatricNo = SAS_StudentSpon.SASI_MatricNo INNER JOIN"
                    str += " SAS_Faculty ON SAS_Student.SASI_Faculty = SAS_Faculty.SAFC_Code "
                    str += " where poststatus='Posted'  "
                    str += " and SAS_Program.SAPG_Code = SAS_Student.SASI_PgId "

                    'str = " SELECT dbo.SAS_Student.SASI_Name, dbo.SAS_Student.SASI_ID,CONVERT(VARCHAR(10), dbo.SAS_Accounts.TransDate,103) as TransDate , dbo.SAS_Accounts.TransCode, dbo.SAS_Accounts.Description,dbo.SAS_Program.SAPG_Program,dbo.SAS_Faculty.SAFC_Desc, "
                    'str += " dbo.SAS_Accounts.TransAmount, dbo.SAS_Student.SASI_MatricNo, dbo.SAS_StudentSpon.SASS_Sponsor,dbo.SAS_Student.SASI_PgId, dbo.SAS_Faculty.SAFC_SName, dbo.SAS_Accounts.TransType "
                    'str += " FROM dbo.SAS_Program, dbo.SAS_Accounts INNER JOIN "
                    'str += " dbo.SAS_Student ON dbo.SAS_Accounts.CreditRef = dbo.SAS_Student.SASI_MatricNo INNER JOIN "
                    'str += " dbo.SAS_StudentSpon ON dbo.SAS_Student.SASI_MatricNo = dbo.SAS_StudentSpon.SASI_MatricNo INNER JOIN"
                    'str += " dbo.SAS_Faculty ON dbo.SAS_Student.SASI_Faculty = dbo.SAS_Faculty.SAFC_Code "
                    'str += " where poststatus='Posted'  "
                    'str += " and dbo.SAS_Program.SAPG_Code = dbo.SAS_Student.SASI_PgId "

                    If Session("status") = 1 Then

                        str += "and SAS_Student.SASS_Code = '" + status + "' "

                    Else

                        str += "and SAS_Student.SASS_Code <> '" + status + "' "

                    End If

                    str += " and  SAS_Student.SASI_PgId like '" + program + "' "
                    str += " and  SAS_Student.SASI_Faculty like '" + faculty + "' and  SAS_StudentSpon.SASS_Sponsor like '" + sponsor + "' "
                    str += datecondition + " "
                    str += sortbyvalue

                Else
                    str = " SELECT SAS_Student.SASI_Name, SAS_Student.SASI_ID,to_char(SAS_Accounts.TransDate,'DD/MM/YYYY') as TransDate, SAS_Accounts.TransCode, SAS_Accounts.Description,SAS_Program.SAPG_Program,SAS_Faculty.SAFC_Desc, "
                    str += " SAS_Accounts.TransAmount, SAS_Student.SASI_MatricNo,SAS_Faculty.SAFC_SName,SAS_Student.SASI_PgId, SAS_Accounts.TransType "
                    str += " FROM SAS_Program, SAS_Accounts INNER JOIN "
                    str += " SAS_Student ON SAS_Accounts.CreditRef = SAS_Student.SASI_MatricNo INNER JOIN"
                    str += " SAS_Faculty ON SAS_Student.SASI_Faculty = SAS_Faculty.SAFC_Code "
                    str += " where poststatus='Posted'  "
                    str += " and SAS_Program.SAPG_Code = SAS_Student.SASI_PgId "
                    'str = " SELECT dbo.SAS_Student.SASI_Name, dbo.SAS_Student.SASI_ID,CONVERT(VARCHAR(10), dbo.SAS_Accounts.TransDate,103) as TransDate , dbo.SAS_Accounts.TransCode, dbo.SAS_Accounts.Description,dbo.SAS_Program.SAPG_Program,dbo.SAS_Faculty.SAFC_Desc, "
                    'str += " dbo.SAS_Accounts.TransAmount, dbo.SAS_Student.SASI_MatricNo,dbo.SAS_Faculty.SAFC_SName,dbo.SAS_Student.SASI_PgId, dbo.SAS_Accounts.TransType "
                    'str += " FROM dbo.SAS_Program, dbo.SAS_Accounts INNER JOIN "
                    'str += " dbo.SAS_Student ON dbo.SAS_Accounts.CreditRef = dbo.SAS_Student.SASI_MatricNo INNER JOIN"
                    'str += " dbo.SAS_Faculty ON dbo.SAS_Student.SASI_Faculty = dbo.SAS_Faculty.SAFC_Code "
                    'str += " where poststatus='Posted'  "
                    'str += " and dbo.SAS_Program.SAPG_Code = dbo.SAS_Student.SASI_PgId "

                    If Session("status") = 1 Then

                        str += "and SAS_Student.SASS_Code = '" + status + "' "

                    Else

                        str += "and SAS_Student.SASS_Code <> '" + status + "' "

                    End If
                    str += "and  SAS_Student.SASI_PgId like '" + program + "' "
                    str += " and  SAS_Student.SASI_Faculty like '" + faculty + "'"
                    str += datecondition + " "
                    str += sortbyvalue

                End If

                'Author			: Anil Kumar - T-Melmax Sdn Bhd
                'Created Date	: 20/05/2015

                'DataSet Strating
                Dim _DataSet As DataSet = _ReportHelper.GetDataSet(str)
                _DataSet.Tables(0).TableName = "Table"
                'Report XML Loading

                Dim s As String = Server.MapPath("../xml/Transaction.xml")
                _DataSet.WriteXml(s)

                'Report XML Ended

                'Records Checking
                If _DataSet.Tables(0).Rows.Count = 0 Then
                    Response.Write("No Record Found")

                Else
                    'Report Loading 

                    MyReportDocument.Load(Server.MapPath("~/GroupReport/RptTransactionDetailEn.rpt"))
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
