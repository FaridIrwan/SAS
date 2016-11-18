Imports System.Data
Imports System.Data.SqlClient
Imports System.Web.Configuration
Imports CrystalDecisions.CrystalReports.Engine

Partial Class RptTransactionDetailViewer
    Inherits System.Web.UI.Page
    Private MyReportDocument As New ReportDocument

#Region "Global Declarations "
    'Author			: Anil Kumar - T-Melmax Sdn Bhd
    'Created Date	: 20/05/2015

    Private _ReportHelper As New ReportHelper

#End Region

#Region "Page Load Starting  "

    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init

        'Author			: Anil Kumar - T-Melmax Sdn Bhd
        'Created Date	: 20/05/2015
        'Modified by Hafiz @ 01/3/2016
        'Modified by Hafiz @ 26/4/2016

        Try

            If Not IsPostBack Then
                Dim _ReportHelper As New ReportHelper

                Dim sortbyvalue As String = Nothing
                Dim status As String = Nothing
                Dim program As String = Nothing
                Dim faculty As String = Nothing
                Dim sponsor As String = Nothing
                Dim datecondition As String = Nothing, dateconditionLO As String = Nothing
                Dim str As String = Nothing

                If Session("sortby") Is Nothing Then
                    sortbyvalue = " "
                Else
                    If Session("sortby") = "matricid" Then
                        sortbyvalue = " Order By t.SASI_MatricNo "
                    ElseIf Session("sortby") = "studname" Then
                        sortbyvalue = " Order By t.SASI_Name "
                    End If
                End If

                'active/inactive status dropdown - start
                If Session("status") = Nothing Or Session("status") = "-1" Then
                    status = " "
                Else
                    status = " and ss.SASS_Code IN " + _ReportHelper.Status(Session("status")) + " "
                End If
                'active/inactive status dropdown - end

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
                    dateconditionLO = ""
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

                    datecondition = " and sa.TransDate between '" + datefrom + "' and '" + dateto + "' "
                    dateconditionLO = " and lo.TransDate between '" + datefrom + "' and '" + dateto + "' "
                End If

                'query modified by Hafiz @ 15/04/2016
                If Not sponsor = "%" Then
                    str = "SELECT * FROM ( " +
                          "(select ss.SASI_Name,ss.SASI_ICNo,ss.sasi_MatricNo, " +
                            "case when ss.SASI_Faculty != '' then " +
                                 "(select SAFC_Desc from SAS_Faculty " +
                                  "WHERE SAFC_Code = ss.SASI_Faculty) " +
                                 "else '' " +
                            "end as Faculty, " +
                            "case when ss.SASI_PgId != '' then " +
                                 "(select sapg_program from SAS_Program " +
                                  "WHERE SAPG_Code = ss.SASI_PgId) " +
                                 "else '' " +
                            "end as Program, " +
                            "ss.SASI_CurSemYr, " +
                            "ss.SASI_Intake,ss.SASI_CurSem AS No_Of_Sem,sa.Category, " +
                            "(select sk.sako_description from sas_kolej sk where sk.sako_code=ss.SAKO_Code) as Hostel, " +
                            "case when ss.SASS_Code != '' Then " +
                                 "(SELECT SASS_code ||'-'|| SASS_description FROM SAS_StudentStatus " +
                                 "WHERE SASS_code = ss.SASS_Code) " +
                                 "end as Curr_Status, " +
                            "ss.SASc_Code, to_char(sa.transdate, 'DD/MM/YYYY') Sb_TransDate,sa.description, " +
                            "CASE WHEN sa.Category = 'Receipt' THEN " +
                                    "CASE WHEN sa.Description != 'CIMB CLICKS' THEN sa.BankRecNo " +
                                    "Else sa.BatchCode " +
                                    "End " +
                                 "Else " +
                                    "CASE WHEN Category = 'Loan' THEN BatchCode " +
                                    "Else sa.TransCode " +
                                    "End " +
                                 "END TransCode, " +
                            "CASE WHEN sa.category = 'Credit Note' or sa.category = 'Debit Note' or sa.category = 'Invoice' then " +
                                    "CASE  WHEN sa.transtype='Credit' THEN sum(de.transamount) " +
                                    "Else 0 " +
                                    "End " +
                                 "Else " +
                                    "CASE  WHEN sa.transtype='Credit' THEN sum(sa.transamount) " +
                                    "Else 0 " +
                                    "End " +
                                 "End as Credit, " +
                            "CASE WHEN sa.category = 'Credit Note' or sa.category = 'Debit Note' or sa.category = 'Invoice' then " +
                                    "CASE WHEN sa.transtype='Debit' THEN sum(de.transamount) " +
                                    "ELSE 0 " +
                                    "End " +
                                 "Else " +
                                    "CASE WHEN sa.transtype='Debit' THEN sum(sa.transamount) " +
                                    "ELSE 0 " +
                                    "End " +
                                 "END as Debit, " +
                            "st.SASS_Sponsor ||'-'|| sp.SASR_name As Sponsor " +
                            "from SAS_Student ss " +
                            "inner join sas_accounts sa on sa.creditref=ss.sasi_MatricNo  and sa.PostStatus = 'Posted' and sa.SubType = 'Student' " +
                            "left join sas_accountsdetails de on sa.transid = de.transid " +
                            "LEFT JOIN SAS_StudentSpon st ON sa.CreditRef = st.SASI_MatricNo " +
                            "LEFT JOIN SAS_Sponsor sp ON st.SASS_Sponsor = sp.SASR_Code " +
                            "where sa.transtype in  ('Debit','Credit') " +
                            "AND sa.subcategory NOT IN ('Loan') " +
                            "AND ss.SASI_PgId like '" + program + "' " +
                            "AND ss.SASI_Faculty like '" + faculty + "' " +
                            "AND st.SASS_Sponsor like '" + sponsor + "' " +
                    status + datecondition +
                    "group by ss.sasi_name,ss.SASI_ICNo,ss.sasi_MatricNo,ss.SASI_PgId,ss.SASI_Faculty,ss.SASI_CurSemYr,sa.Category, ss.SASI_Intake, " +
                    "ss.SASI_CurSem, ss.SART_Code, ss.SASI_FloorNo, ss.SABK_Code, ss.SAKO_Code, ss.SASS_Code, ss.SASc_Code, sa.TransCode, sa.transdate, " +
                    "sa.description, transtype, sa.bankrecno, sa.batchcode, st.SASS_Sponsor, sp.SASR_name) "

                    str += "UNION "

                    str += "(select * FROM " +
                            "(select ss.SASI_Name, ss.SASI_ICNo,ss.sasi_MatricNo, " +
                                "case when ss.SASI_Faculty != '' then " +
                                        "(select SAFC_Desc from SAS_Faculty " +
                                        "WHERE SAFC_Code = ss.SASI_Faculty) " +
                                        "else '' " +
                                     "end as Faculty, " +
                                "case when ss.SASI_PgId != '' then " +
                                        "(select sapg_program from SAS_Program " +
                                        "WHERE SAPG_Code = ss.SASI_PgId) " +
                                        "else '' " +
                                     "end as Program, " +
                            "ss.SASI_CurSemYr,ss.SASI_Intake,ss.SASI_CurSem AS No_Of_Sem,lo.Category, " +
                            "(select sk.sako_description " +
                            "from sas_kolej sk " +
                            "where sk.sako_code=ss.SAKO_Code) as Hostel, " +
                                "case when ss.SASS_Code != '' Then " +
                                        "(SELECT SASS_code ||'-'|| SASS_description FROM SAS_StudentStatus " +
                                        "WHERE SASS_code = ss.SASS_Code) " +
                                     "end as Curr_Status, " +
                            "ss.SASc_Code, to_char(lo.transdate, 'DD/MM/YYYY') Sb_TransDate, lo.description, lo.transcode, " +
                                "CASE WHEN lo.Category = 'Receipt' THEN " +
                                        "CASE WHEN lo.TransType = 'Debit' THEN lo.TransAmount " +
                                        "ELSE 0 " +
                                        "End " +
                                     "Else " +
                                        "CASE WHEN lo.TransType = 'Credit' THEN lo.TransAmount " +
                                        "ELSE 0 " +
                                        "End " +
                                     "END Debit, " +
                                "CASE WHEN lo.Category = 'Receipt' THEN " +
                                        "CASE WHEN lo.TransType = 'Credit' THEN lo.TransAmount " +
                                        "ELSE 0 " +
                                        "End " +
                                     "Else " +
                                        "CASE WHEN lo.TransType = 'Debit' THEN lo.TransAmount " +
                                        "ELSE 0 " +
                                        "End " +
                                     "END Credit, " +
                            "st.SASS_Sponsor ||'-'|| sp.SASR_name As Sponsor " +
                            "FROM  SAS_StudentLoan lo " +
                            "left join sas_student ss on lo.creditref = ss.sasi_matricno " +
                            "LEFT JOIN SAS_StudentSpon st ON ss.sasi_matricno = st.SASI_MatricNo " +
                            "LEFT JOIN SAS_Sponsor sp ON st.SASS_Sponsor = sp.SASR_Code " +
                            "where lo.transtype in  ('Debit','Credit') " +
                            "AND lo.SubType = 'Student' " +
                            "and lo.PostStatus ='Posted' " +
                            "AND ss.SASI_PgId like '" + program + "' " +
                            "AND ss.SASI_Faculty like '" + faculty + "' " +
                            "AND st.SASS_Sponsor like '" + sponsor + "' " +
                            status +
                            dateconditionLO +
                            " ) A ) ) t " + sortbyvalue
                Else
                    str = "SELECT * FROM ( " +
                          "(select ss.SASI_Name,ss.SASI_ICNo,ss.sasi_MatricNo, " +
                            "case when ss.SASI_Faculty != '' then " +
                                 "(select SAFC_Desc from SAS_Faculty " +
                                  "WHERE SAFC_Code = ss.SASI_Faculty) " +
                                 "else '' " +
                            "end as Faculty, " +
                            "case when ss.SASI_PgId != '' then " +
                                 "(select sapg_program from SAS_Program " +
                                  "WHERE SAPG_Code = ss.SASI_PgId) " +
                                 "else '' " +
                            "end as Program, " +
                            "ss.SASI_CurSemYr, " +
                            "ss.SASI_Intake,ss.SASI_CurSem AS No_Of_Sem,sa.Category, " +
                            "(select sk.sako_description from sas_kolej sk where sk.sako_code=ss.SAKO_Code) as Hostel, " +
                            "case when ss.SASS_Code != '' Then " +
                                 "(SELECT SASS_code ||'-'|| SASS_description FROM SAS_StudentStatus " +
                                 "WHERE SASS_code = ss.SASS_Code) " +
                                 "end as Curr_Status, " +
                            "ss.SASc_Code, to_char(sa.transdate, 'DD/MM/YYYY') Sb_TransDate,sa.description, " +
                            "CASE WHEN sa.Category = 'Receipt' THEN " +
                                    "CASE WHEN sa.Description != 'CIMB CLICKS' THEN sa.BankRecNo " +
                                    "Else sa.BatchCode " +
                                    "End " +
                                 "Else " +
                                    "CASE WHEN Category = 'Loan' THEN BatchCode " +
                                    "Else sa.TransCode " +
                                    "End " +
                                 "END TransCode, " +
                            "CASE WHEN sa.category = 'Credit Note' or sa.category = 'Debit Note' or sa.category = 'Invoice' then " +
                                    "CASE  WHEN sa.transtype='Credit' THEN sum(de.transamount) " +
                                    "Else 0 " +
                                    "End " +
                                 "Else " +
                                    "CASE  WHEN sa.transtype='Credit' THEN sum(sa.transamount) " +
                                    "Else 0 " +
                                    "End " +
                                 "End as Credit, " +
                            "CASE WHEN sa.category = 'Credit Note' or sa.category = 'Debit Note' or sa.category = 'Invoice' then " +
                                    "CASE WHEN sa.transtype='Debit' THEN sum(de.transamount) " +
                                    "ELSE 0 " +
                                    "End " +
                                 "Else " +
                                    "CASE WHEN sa.transtype='Debit' THEN sa.transamount " +
                                    "ELSE 0 " +
                                    "End " +
                                 "END as Debit, " +
                            "st.SASS_Sponsor ||'-'|| sp.SASR_name As Sponsor " +
                            "from SAS_Student ss " +
                            "inner join sas_accounts sa on sa.creditref=ss.sasi_MatricNo  and sa.PostStatus = 'Posted' and sa.SubType = 'Student' " +
                            "left join sas_accountsdetails de on sa.transid = de.transid " +
                            "LEFT JOIN SAS_StudentSpon st ON sa.CreditRef = st.SASI_MatricNo " +
                            "LEFT JOIN SAS_Sponsor sp ON st.SASS_Sponsor = sp.SASR_Code " +
                            "where sa.transtype in  ('Debit','Credit') " +
                            "AND sa.subcategory NOT IN ('Loan') " +
                            "AND ss.SASI_PgId like '" + program + "' " +
                            "AND ss.SASI_Faculty like '" + faculty + "' " +
                            status + datecondition +
                            "group by ss.sasi_name,ss.SASI_ICNo,ss.sasi_MatricNo,ss.SASI_PgId,ss.SASI_Faculty,ss.SASI_CurSemYr,sa.Category, ss.SASI_Intake, " +
                            "ss.SASI_CurSem, ss.SART_Code, ss.SASI_FloorNo, ss.SABK_Code, ss.SAKO_Code, ss.SASS_Code, ss.SASc_Code, sa.TransCode, sa.transdate, " +
                            "sa.description, transtype, sa.bankrecno, sa.batchcode, st.SASS_Sponsor, sp.SASR_name,sa.transamount) "

                    str += "UNION "

                    str += "(select * FROM " +
                            "(select ss.SASI_Name, ss.SASI_ICNo,ss.sasi_MatricNo, " +
                                "case when ss.SASI_Faculty != '' then " +
                                        "(select SAFC_Desc from SAS_Faculty " +
                                        "WHERE SAFC_Code = ss.SASI_Faculty) " +
                                        "else '' " +
                                     "end as Faculty, " +
                                "case when ss.SASI_PgId != '' then " +
                                        "(select sapg_program from SAS_Program " +
                                        "WHERE SAPG_Code = ss.SASI_PgId) " +
                                        "else '' " +
                                     "end as Program, " +
                            "ss.SASI_CurSemYr,ss.SASI_Intake,ss.SASI_CurSem AS No_Of_Sem,lo.Category, " +
                            "(select sk.sako_description " +
                            "from sas_kolej sk " +
                            "where sk.sako_code=ss.SAKO_Code) as Hostel, " +
                                "case when ss.SASS_Code != '' Then " +
                                        "(SELECT SASS_code ||'-'|| SASS_description FROM SAS_StudentStatus " +
                                        "WHERE SASS_code = ss.SASS_Code) " +
                                     "end as Curr_Status, " +
                            "ss.SASc_Code, to_char(lo.transdate, 'DD/MM/YYYY') Sb_TransDate, lo.description, lo.transcode, " +
                                "CASE WHEN lo.Category = 'Receipt' THEN " +
                                        "CASE WHEN lo.TransType = 'Debit' THEN lo.TransAmount " +
                                        "ELSE 0 " +
                                        "End " +
                                     "Else " +
                                        "CASE WHEN lo.TransType = 'Credit' THEN lo.TransAmount " +
                                        "ELSE 0 " +
                                        "End " +
                                     "END Debit, " +
                                "CASE WHEN lo.Category = 'Receipt' THEN " +
                                        "CASE WHEN lo.TransType = 'Credit' THEN lo.TransAmount " +
                                        "ELSE 0 " +
                                        "End " +
                                     "Else " +
                                        "CASE WHEN lo.TransType = 'Debit' THEN lo.TransAmount " +
                                        "ELSE 0 " +
                                        "End " +
                                     "END Credit, " +
                            "st.SASS_Sponsor ||'-'|| sp.SASR_name As Sponsor " +
                            "FROM  SAS_StudentLoan lo " +
                            "left join sas_student ss on lo.creditref = ss.sasi_matricno " +
                            "LEFT JOIN SAS_StudentSpon st ON ss.sasi_matricno = st.SASI_MatricNo " +
                            "LEFT JOIN SAS_Sponsor sp ON st.SASS_Sponsor = sp.SASR_Code " +
                            "where lo.transtype in  ('Debit','Credit') " +
                            "AND lo.SubType = 'Student' " +
                            "and lo.PostStatus ='Posted' " +
                            "AND ss.SASI_PgId like '" + program + "' " +
                            "AND ss.SASI_Faculty like '" + faculty + "' " +
                            status +
                            dateconditionLO +
                            " ) A ) ) t " + sortbyvalue
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

                    MyReportDocument.Load(Server.MapPath("~/GroupReport/RptTransactionDetail.rpt"))
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
