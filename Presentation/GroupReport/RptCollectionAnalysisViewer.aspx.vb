Imports System.Data
Imports System.Data.SqlClient
Imports System.Web.Configuration
Imports CrystalDecisions.CrystalReports.Engine
Imports CrystalDecisions.Shared

Partial Class RptCollectionAnalysisViewer
    Inherits System.Web.UI.Page
    Private MyReportDocument As New ReportDocument

#Region "Global Declarations "

    Private _ReportHelper As New ReportHelper

#End Region

#Region "Page Load Starting  "

    'Author			: Anil Kumar - T-Melmax Sdn Bhd
    'Purpose		: Get the AFCReport Report
    'Created Date	: 20/05/2015
    'Modified by Hafiz @ 10/3/2016
    'Modified by Hafiz @ 26/4/2016

    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init

        Try
            If Not IsPostBack Then
                Dim _ReportHelper As New ReportHelper

                Dim sortbyvalue As String = Nothing
                Dim status As String = Nothing
                Dim program As String = Nothing
                Dim faculty As String = Nothing
                Dim sponsor As String = Nothing
                Dim datecondition As String = Nothing
                Dim str As String = Nothing

                If Session("sortby") Is Nothing Then

                    sortbyvalue = "Order By a.SASI_PgId,a.SASI_Name"

                ElseIf Session("sortby") = "a.CreditRef" Then

                    sortbyvalue = "Order By a.SASI_PgId," + Session("sortby")

                ElseIf Session("sortby") = "a.SASI_Name" Then

                    sortbyvalue = "Order By a.SASI_PgId," + Session("sortby")

                ElseIf Session("sortby") = "a.BankCode" Then

                    sortbyvalue = "Order By a.SASI_PgId," + Session("sortby")

                End If

                If Session("status") Is Nothing Then
                    status = ""
                Else
                    status = " and SAS_Student.SASS_Code IN " + _ReportHelper.Status(Session("status")) + " "
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

                    str = "SELECT * from ( "
                    str += " (SELECT distinct " +
                            " CASE " +
                                " WHEN SAS_Accounts.Category = 'Receipt'  THEN " +
                                " CASE WHEN SAS_Accounts.SubCategory = 'Loan' THEN " +
                                     " (select transcode from SAS_StudentLoan where BatchCode = SAS_Accounts.BatchCode) " +
                                        " Else " +
                                     " CASE WHEN SAS_Accounts.Description != 'CIMB CLICKS' THEN SAS_Accounts.BankRecNo " +
                                      " ELSE SAS_Accounts.TransCode END" +
                                " End" +
                            " Else" +
                                " SAS_Accounts.TransCode" +
                                        " END TransCode," +
                                " SAS_Accounts.Category," +
                                " SAS_Accounts.BatchCode," +
                                " SAS_Accounts.TransType," +
                                " SAS_Accounts.Description," +
                                " to_char(SAS_Accounts.TransDate,'DD/MM/YYYY') as date," +
                                " SAS_Accounts.BankCode, " +
                                " CASE WHEN SAS_Student.SASI_PgId = (select SAPG_Code from SAS_Program where SAPG_Code = SAS_Student.SASI_PgId) then " +
                                    " (select sapg_program from SAS_Program where SAPG_Code = SAS_Student.SASI_PgId)" +
                                " END AS Program," +
                        " SAS_Student.SASI_Add1," +
                        " SAS_Student.SASI_Add2," +
                        " SAS_Student.SASI_Add3," +
                        " SAS_Student.SASI_City," +
                        " SAS_Accounts.CreditRef," +
                        " SAS_Student.SASI_Name," +
                        " SAS_Accounts.TransAmount," +
                        " SAS_Student.SASI_Faculty," +
                        " SAS_Student.SASI_PgId" +
                        " FROM SAS_Student" +
                        " INNER JOIN SAS_Accounts ON SAS_Accounts.CreditRef=SAS_Student.SASI_MatricNo and SAS_Accounts.PostStatus = 'Posted' and SAS_Accounts.SubType = 'Student' " +
                        " INNER JOIN SAS_StudentSpon ON SAS_Student.SASI_MatricNo = SAS_StudentSpon.SASI_MatricNo" +
                        " LEFT JOIN SAS_AccountsDetails ON SAS_Accounts.TransId = SAS_AccountsDetails.TransId " +
                        " WHERE SAS_Accounts.TransType IN ('Debit','Credit') " +
                        " AND SAS_Accounts.Category NOT IN ('Refund','AFC','Invoice','Debit Note','Credit Note','Loan','SPA')" +
                        " AND SAS_Student.SASI_PgId like '" + program + "' " +
                        " AND SAS_Student.SASI_Faculty like '" + faculty + "' " +
                        " AND SAS_StudentSpon.SASS_Sponsor like '" + sponsor + "' " +
                        status + " " + datecondition + " " +
                        " Order By SAS_Student.SASI_PgId,SAS_Student.SASI_Name)"

                    str += " UNION"

                    str += "(SELECT distinct" +
                            " SAS_Accounts.TransCode," +
                            " SAS_Accounts.Category," +
                            " SAS_Accounts.BatchCode," +
                            " SAS_Accounts.TransType," +
                            " SAS_Accounts.Description, " +
                            " to_char(SAS_Accounts.TransDate,'DD/MM/YYYY') as date, " +
                            " SAS_Accounts.BankCode," +
                            " CASE WHEN SAS_Student.SASI_PgId = (select SAPG_Code from SAS_Program where SAPG_Code = SAS_Student.SASI_PgId) then" +
                                " (select sapg_program from SAS_Program where SAPG_Code = SAS_Student.SASI_PgId)" +
                            " END AS Program," +
                            " SAS_Student.SASI_Add1," +
                            " SAS_Student.SASI_Add2," +
                            " SAS_Student.SASI_Add3," +
                            " SAS_Student.SASI_City," +
                            " SAS_Accounts.CreditRef," +
                            " SAS_Student.SASI_Name," +
                            " SAS_Accounts.TransAmount," +
                            " SAS_Student.SASI_Faculty," +
                            " SAS_Student.SASI_PgId" +
                            " FROM SAS_Student " +
                        " INNER JOIN SAS_Accounts ON SAS_Accounts.CreditRef=SAS_Student.SASI_MatricNo and SAS_Accounts.PostStatus = 'Posted' and SAS_Accounts.SubType = 'Student'" +
                        " INNER JOIN SAS_StudentSpon ON SAS_Student.SASI_MatricNo = SAS_StudentSpon.SASI_MatricNo" +
                        " LEFT JOIN SAS_AccountsDetails ON SAS_Accounts.TransId = SAS_AccountsDetails.TransId " +
                        " WHERE SAS_Accounts.Category IN ('SPA')" +
                        " AND SAS_Accounts.TransType = 'Credit' AND SAS_Accounts.Description LIKE '%Sponsor Allocation%'" +
                        " AND SAS_Student.SASI_PgId like '" + program + "' " +
                        " AND SAS_Student.SASI_Faculty like '" + faculty + "' " +
                        " AND SAS_StudentSpon.SASS_Sponsor like '" + sponsor + "' " +
                        status + " " + datecondition + " " +
                        " Order By SAS_Student.SASI_PgId,SAS_Student.SASI_Name)"

                    str += ") a " + sortbyvalue

                Else

                    str = "SELECT * from ( "
                    str += " (SELECT distinct " +
                            " CASE " +
                                " WHEN SAS_Accounts.Category = 'Receipt'  THEN " +
                                " CASE WHEN SAS_Accounts.SubCategory = 'Loan' THEN " +
                                     " (select transcode from SAS_StudentLoan where BatchCode = SAS_Accounts.BatchCode) " +
                                        " Else " +
                                     " CASE WHEN SAS_Accounts.Description != 'CIMB CLICKS' THEN SAS_Accounts.BankRecNo " +
                                      " ELSE SAS_Accounts.TransCode END" +
                                " End" +
                            " Else" +
                                " SAS_Accounts.TransCode" +
                                        " END TransCode," +
                                " SAS_Accounts.Category," +
                                " SAS_Accounts.BatchCode," +
                                " SAS_Accounts.TransType," +
                                " SAS_Accounts.Description," +
                                " to_char(SAS_Accounts.TransDate,'DD/MM/YYYY') as date," +
                                " SAS_Accounts.BankCode, " +
                                " CASE WHEN SAS_Student.SASI_PgId = (select SAPG_Code from SAS_Program where SAPG_Code = SAS_Student.SASI_PgId) then " +
                                    " (select sapg_program from SAS_Program where SAPG_Code = SAS_Student.SASI_PgId)" +
                                " END Program," +
                        " SAS_Student.SASI_Add1," +
                        " SAS_Student.SASI_Add2," +
                        " SAS_Student.SASI_Add3," +
                        " SAS_Student.SASI_City," +
                        " SAS_Accounts.CreditRef," +
                        " SAS_Student.SASI_Name," +
                        " SAS_Accounts.TransAmount," +
                        " SAS_Student.SASI_Faculty," +
                        " SAS_Student.SASI_PgId" +
                        " FROM SAS_Student" +
                        " INNER JOIN SAS_Accounts ON SAS_Accounts.CreditRef=SAS_Student.SASI_MatricNo and SAS_Accounts.PostStatus = 'Posted' and SAS_Accounts.SubType = 'Student' " +
                        " LEFT JOIN SAS_AccountsDetails ON SAS_Accounts.TransId = SAS_AccountsDetails.TransId " +
                        " WHERE SAS_Accounts.TransType IN ('Debit','Credit') " +
                        " AND SAS_Accounts.Category NOT IN ('Refund','AFC','Invoice','Debit Note','Credit Note','Loan','SPA','STA')" +
                        " AND SAS_Student.SASI_PgId like '" + program + "' " +
                        " AND SAS_Student.SASI_Faculty like '" + faculty + "' " +
                        status + " " + datecondition + " " +
                        " Order By SAS_Student.SASI_PgId,SAS_Student.SASI_Name)"

                    str += " UNION"

                    str += "(SELECT distinct" +
                            " SAS_Accounts.TransCode," +
                            " SAS_Accounts.Category," +
                            " SAS_Accounts.BatchCode," +
                            " SAS_Accounts.TransType," +
                            " SAS_Accounts.Description, " +
                            " to_char(SAS_Accounts.TransDate,'DD/MM/YYYY') as date, " +
                            " SAS_Accounts.BankCode," +
                            " CASE WHEN SAS_Student.SASI_PgId = (select SAPG_Code from SAS_Program where SAPG_Code = SAS_Student.SASI_PgId) then" +
                                " (select sapg_program from SAS_Program where SAPG_Code = SAS_Student.SASI_PgId)" +
                            " END Program," +
                            " SAS_Student.SASI_Add1," +
                            " SAS_Student.SASI_Add2," +
                            " SAS_Student.SASI_Add3," +
                            " SAS_Student.SASI_City," +
                            " SAS_Accounts.CreditRef," +
                            " SAS_Student.SASI_Name," +
                            " SAS_Accounts.TransAmount," +
                            " SAS_Student.SASI_Faculty," +
                            " SAS_Student.SASI_PgId" +
                            " FROM SAS_Student " +
                        " INNER JOIN SAS_Accounts ON SAS_Accounts.CreditRef=SAS_Student.SASI_MatricNo and SAS_Accounts.PostStatus = 'Posted' and SAS_Accounts.SubType = 'Student'" +
                        " LEFT JOIN SAS_AccountsDetails ON SAS_Accounts.TransId = SAS_AccountsDetails.TransId " +
                        " WHERE SAS_Accounts.Category IN ('SPA')" +
                        " AND SAS_Accounts.TransType = 'Credit' AND SAS_Accounts.Description LIKE '%Sponsor Allocation%'" +
                        " AND SAS_Student.SASI_PgId like '" + program + "' " +
                        " AND SAS_Student.SASI_Faculty like '" + faculty + "' " +
                        status + " " + datecondition + " " +
                        " Order By SAS_Student.SASI_PgId,SAS_Student.SASI_Name)"

                    str += ") a " + sortbyvalue

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
                    MyReportDocument.Load(Server.MapPath("~/GroupReport/RptCollectionAnalysis.rpt"))
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
