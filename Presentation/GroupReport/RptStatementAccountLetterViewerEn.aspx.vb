Imports HTS.SAS.Entities
Imports HTS.SAS.BusinessObjects
Imports System.Data
Imports System.Data.SqlClient
Imports System.Web.Configuration
Imports System.Collections.Generic
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

                Dim code As String
                Dim datecondition As String = Nothing
                Dim sortbyvalue As String = Nothing
                Dim status As String = Nothing
                Dim program As String = Nothing
                Dim faculty As String = Nothing
                Dim sponsor As String = Nothing
                Dim str As String = Nothing
                Dim str1 As String = Nothing
                Dim _DataSet As DataSet

                If Session("sortby") Is Nothing Then

                    sortbyvalue = " Order By SAS_Student.SASI_Name"

                ElseIf Session("sortby") = "SAS_Student.SASI_MatricNo" Then

                    sortbyvalue = " Order By SAS_Student.SASI_Name," + Session("sortby")

                ElseIf Session("sortby") = "SAS_Student.SASI_Name" Then

                    sortbyvalue = " Order By " + Session("sortby")

                ElseIf Session("sortby") = "SAS_Faculty.SAFC_SName" Then

                    sortbyvalue = " Order By SAS_Student.SASI_Name," + Session("sortby")

                ElseIf Session("sortby") = "SAS_Student.SASI_Postcode" Then

                    sortbyvalue = " Order By SAS_Student.SASI_Name," + Session("sortby")

                End If


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


                Dim dunno As String
                dunno = Request.QueryString("dunno1")

                If Session("DunningNo") Is Nothing Then

                    code = dunno

                Else

                    code = Session("DunningNo")

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

                If Not sponsor = "%" Then

                    str = " SELECT     SAS_Accounts.CreditRef, SAS_Student.SASI_MatricNo,"
                    str += " SAS_Student.SASI_Add1,SAS_Student.SASI_Add2,SAS_Student.SASI_Add3,SAS_Student.SASI_City,SAS_Faculty.SAFC_SName, SAS_Student.SASI_Postcode, "
                    str += " SAS_Student.SASI_Name, SAS_Accounts.TransCode,SAS_Accounts.TransType, "
                    str += "to_char(SAS_Accounts.TransDate,'DD/MM/YYYY') as TransDate, SAS_Accounts.Description, SAS_Student.SASI_ID, SAS_Student.SASI_Intake, "
                    str += " SAS_Program.SAPG_Program, SAS_Accounts.TransAmount "
                    str += " FROM SAS_Accounts INNER JOIN"
                    str += " SAS_Student ON SAS_Accounts.CreditRef = SAS_Student.SASI_MatricNo INNER JOIN "
                    str += " SAS_Program ON SAS_Student.SASI_PgId = SAS_Program.SAPG_Code INNER JOIN "
                    str += " SAS_Faculty ON SAS_Student.SASI_Faculty = SAS_Faculty.SAFC_Code "
                    str += " INNER JOIN SAS_StudentSpon ON SAS_Student.SASI_MatricNo = SAS_StudentSpon.SASI_MatricNo "
                    str += " where SAS_Accounts.poststatus = 'Posted' and SAS_Student.SASI_StatusRec = '" + status + "' and SAS_Student.SASI_PgId like '" + program + "'"
                    str += " and  SAS_Student.SASI_Faculty like '" + faculty + "' and SAS_StudentSpon.SASS_Sponsor like '" + sponsor + "'"
                    str += datecondition + " "
                    str += sortbyvalue
                Else
                    str = " SELECT     SAS_Accounts.CreditRef, SAS_Student.SASI_MatricNo,"
                    str += " SAS_Student.SASI_Add1,SAS_Student.SASI_Add2,SAS_Student.SASI_Add3,SAS_Student.SASI_City,SAS_Faculty.SAFC_SName, SAS_Student.SASI_Postcode, "
                    str += " SAS_Student.SASI_Name, SAS_Accounts.TransCode,SAS_Accounts.TransType, "
                    str += " to_char(SAS_Accounts.TransDate,'DD/MM/YYYY') as TransDate, SAS_Accounts.Description, SAS_Student.SASI_ID, SAS_Student.SASI_Intake, "
                    str += " SAS_Program.SAPG_Program, SAS_Accounts.TransAmount "
                    str += " FROM SAS_Accounts INNER JOIN"
                    str += " SAS_Student ON SAS_Accounts.CreditRef = SAS_Student.SASI_MatricNo INNER JOIN "
                    str += " SAS_Program ON SAS_Student.SASI_PgId = SAS_Program.SAPG_Code INNER JOIN "
                    str += " SAS_Faculty ON SAS_Student.SASI_Faculty = SAS_Faculty.SAFC_Code  "
                    str += " where SAS_Accounts.poststatus = 'Posted' and SAS_Student.SASI_StatusRec = '" + status + "' and SAS_Student.SASI_PgId like '" + program + "'"
                    str += " and  SAS_Student.SASI_Faculty like '" + faculty + "' "
                    str += datecondition + " "
                    str += sortbyvalue

                    '    str = " SELECT     dbo.SAS_Accounts.CreditRef, dbo.SAS_Student.SASI_MatricNo,"
                    '    str += " dbo.SAS_Student.SASI_Add1,dbo.SAS_Student.SASI_Add2,dbo.SAS_Student.SASI_Add3,SAS_Student.SASI_City,dbo.SAS_Faculty.SAFC_SName, dbo.SAS_Student.SASI_Postcode, "
                    '    str += " dbo.SAS_Student.SASI_Name, dbo.SAS_Accounts.TransCode,dbo.SAS_Accounts.TransType, "
                    '    str += " CONVERT(VARCHAR(10), dbo.SAS_Accounts.TransDate,103) as TransDate, dbo.SAS_Accounts.Description, dbo.SAS_Student.SASI_ID, dbo.SAS_Student.SASI_Intake, "
                    '    str += " dbo.SAS_Program.SAPG_Program, dbo.SAS_Accounts.TransAmount "
                    '    str += " FROM dbo.SAS_Accounts INNER JOIN"
                    '    str += " dbo.SAS_Student ON dbo.SAS_Accounts.CreditRef = dbo.SAS_Student.SASI_MatricNo INNER JOIN "
                    '    str += " dbo.SAS_Program ON dbo.SAS_Student.SASI_PgId = dbo.SAS_Program.SAPG_Code INNER JOIN "
                    '    str += " dbo.SAS_Faculty ON dbo.SAS_Student.SASI_Faculty = dbo.SAS_Faculty.SAFC_Code "
                    '    str += " INNER JOIN SAS_StudentSpon ON SAS_Student.SASI_MatricNo = SAS_StudentSpon.SASI_MatricNo "
                    '    str += " where dbo.SAS_Accounts.poststatus = 'Posted' and dbo.SAS_Student.SASI_StatusRec like '" + status + "' and dbo.SAS_Student.SASI_PgId like '" + program + "'"
                    '    str += " and  dbo.SAS_Student.SASI_Faculty like '" + faculty + "' and dbo.SAS_StudentSpon.SASS_Sponsor like '" + sponsor + "'"
                    '    str += datecondition + " "
                    '    str += sortbyvalue
                    'Else
                    '    str = " SELECT     dbo.SAS_Accounts.CreditRef, dbo.SAS_Student.SASI_MatricNo,"
                    '    str += " dbo.SAS_Student.SASI_Add1,dbo.SAS_Student.SASI_Add2,dbo.SAS_Student.SASI_Add3,SAS_Student.SASI_City,dbo.SAS_Faculty.SAFC_SName, dbo.SAS_Student.SASI_Postcode, "
                    '    str += " dbo.SAS_Student.SASI_Name, dbo.SAS_Accounts.TransCode,dbo.SAS_Accounts.TransType, "
                    '    str += " CONVERT(VARCHAR(10), dbo.SAS_Accounts.TransDate,103) as TransDate, dbo.SAS_Accounts.Description, dbo.SAS_Student.SASI_ID, dbo.SAS_Student.SASI_Intake, "
                    '    str += " dbo.SAS_Program.SAPG_Program, dbo.SAS_Accounts.TransAmount "
                    '    str += " FROM dbo.SAS_Accounts INNER JOIN"
                    '    str += " dbo.SAS_Student ON dbo.SAS_Accounts.CreditRef = dbo.SAS_Student.SASI_MatricNo INNER JOIN "
                    '    str += " dbo.SAS_Program ON dbo.SAS_Student.SASI_PgId = dbo.SAS_Program.SAPG_Code INNER JOIN "
                    '    str += " dbo.SAS_Faculty ON dbo.SAS_Student.SASI_Faculty = dbo.SAS_Faculty.SAFC_Code  "
                    '    str += " where dbo.SAS_Accounts.poststatus = 'Posted' and dbo.SAS_Student.SASI_StatusRec like '" + status + "' and dbo.SAS_Student.SASI_PgId like '" + program + "'"
                    '    str += " and  dbo.SAS_Student.SASI_Faculty like '" + faculty + "' "
                    '    str += datecondition + " "
                    '    str += sortbyvalue
                End If


                str1 = "SELECT SADL_Code,SADL_Title,SADL_Ref,SADL_Message,SADL_SignBy,SADL_Name,to_char(SADL_FrDate,'DD/MM/YYYY') as FromDate,"
                str1 += "to_char(SADL_ToDate,'DD/MM/YYYY') as ToDateSADL_ToDate,SADL_UpdatedBy,To_char(SADL_UpdatedTime,'DD/MM/YYYY') as UpdateDate from SAS_DunningLetters where SADL_Code='" + code + "'"

                'Author			: Anil Kumar - T-Melmax Sdn Bhd
                'Created Date	: 20/05/2015

                'DataSet Strating
                _DataSet = _ReportHelper.GetDataSet(str, str1)

                _DataSet.Tables(0).TableName = "DunningLatter"
                '_DataSet = _ReportHelper.GetDataSet(str1)

                '_DataSet = _ReportHelper.

                'Report XML Loading


                'Dim con As SqlConnection = New SqlConnection(constr)
                'Dim cmd As SqlCommand
                'cmd = New SqlCommand(str, con)

                'Dim da As SqlDataAdapter = New SqlDataAdapter(cmd)
                'Dim ds As DataSet = New DataSet()

                'da.Fill(ds, "StatmentAC")
                'cmd.Dispose()

                'cmd = New SqlCommand(str1, con)
                'Dim da1 As SqlDataAdapter = New SqlDataAdapter(cmd)
                'da1.Fill(ds, "DunningLatter")

                Dim s As String = Server.MapPath("~/xml/StateDunningletter.xml")
                _DataSet.WriteXml(s)

                'Report XML Ended

                'Records Checking

                If _DataSet.Tables(0).Rows.Count = 0 Then
                    Response.Write("No Record Found")
                    Return
                End If

                'Report Loading 

                MyReportDocument.Load(Server.MapPath("~/GroupReport/RptStatementAccountLetterEn.rpt"))
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
