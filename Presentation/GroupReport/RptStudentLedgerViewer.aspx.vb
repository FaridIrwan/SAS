Imports System.Data
Imports System.Data.SqlClient
Imports System.Web.Configuration
Imports CrystalDecisions.CrystalReports.Engine
Imports CrystalDecisions.Shared

Partial Class RptStudentLedgerViewer
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

        Try

            If Not IsPostBack Then

                Dim sortbyvalue As String = Nothing
                Dim status As String = Nothing
                Dim datecondition As String = Nothing
                Dim str As String = Nothing

                If Session("sortby") = Nothing Then

                    sortbyvalue = " Order By SASI_PgId, SASI_Name, Sb_TransDate "

                Else

                    sortbyvalue = " Order By " + Session("sortby") + ", Sb_TransDate "

                End If

                If Session("status") = Nothing Or Session("status") = -1 Then

                    status = "%"

                Else

                    status = Session("status")

                End If
                str = Session("studentCode")
                Dim GetSponsorSQL As String = Nothing
                Dim Category As String = Session("LedgerType").ToString()
                If Session("studentCode") <> Nothing Then

                    'str = "select SASI_Name,SASI_ICNo,Sb_MatricNo,SASI_PgId,SASI_CurSemYr,SASI_Intake,SASI_CurSem,SART_Code "
                    'str += " + ' ' + SASI_FloorNo + ' ' +  SABK_Code + ' ' +  SAKO_Code  Hostel, SASS_Code , so.SASR_Code, "
                    'str += " SASR_Name,CONVERT(VARCHAR(10), Sb_TransDate, 103) Sb_TransDate,Sb_FeeCode,Sb_Description,Debit,Credit from SAS_Student ss "
                    'str += " inner join ( SELECT Sb_MatricNo,Debit,Credit,Sb_Description,Sb_FeeCode,Sb_TransDate"
                    'str += " FROM (SELECT * from vjbit_billing) p PIVOT ( SUM(sb_transAmount) FOR sb_transType IN"
                    'str += " ( [Debit], [Credit] )) AS pvt) pv on ss.SASI_MatricNo = pv.Sb_MatricNo inner join SAS_Program sp on sp.SAPG_Code = "
                    'str += " ss.SASI_PgId left join SAS_StudentSpon spo on spo.SASI_MatricNo = ss.SASI_MatricNo left join"
                    'str += " SAS_Sponsor so on so.SASR_Code = spo.SASS_Sponsor where Sb_MatricNo = '" + Session("studentCode") + "'"
                    'str += sortbyvalue

                    'str = " select ss.SASI_Name,ss.SASI_ICNo,ss.sasi_MatricNo,ss.SASI_PgId,ss.SASI_CurSemYr,ss.SASI_Intake,ss.SASI_CurSem,sa.Category,"
                    'str += " ss.SART_Code || ss.SASI_FloorNo || ss.SABK_Code || ss.SAKO_Code Hostel, ss.SASS_Code , ss.SASc_Code,so.SASR_Name,sa.transcode,"
                    'str += " to_char(sa.transdate, 'DD/MM/YYYY') Sb_TransDate,sa.description"
                    'str += " ,CASE WHEN sa.transtype='Credit' THEN sum (sa.transamount)"
                    'str += " WHEN sa.transtype='Debit' THEN sum (sa.transamount)"
                    'str += " ELSE 0"
                    'str += " END  as Credit"
                    'str += " ,CASE WHEN sa.transtype='Debit' THEN sum (sa.transamount)"
                    'str += " ELSE 0"
                    'str += " END  as Debit"

                    'str += " from SAS_Student ss  "
                    'str += " inner join sas_accounts sa on sa.creditref=ss.sasi_MatricNo"
                    'str += " left join SAS_StudentSpon spo on spo.SASI_MatricNo = ss.SASI_MatricNo"
                    'str += " left join SAS_Sponsor so on so.SASR_Code = spo.SASS_Sponsor where sa.transtype in  ('Debit','Credit') and ss.sasi_MatricNo = '" + Session("studentCode") + "' "
                    'str += " group by ss.sasi_name,ss.SASI_ICNo,ss.sasi_MatricNo,ss.SASI_PgId,ss.SASI_CurSemYr,sa.Category,ss.SASI_Intake,ss.SASI_CurSem,ss.SART_Code,"
                    'str += " ss.SASI_FloorNo,ss.SABK_Code,ss.SAKO_Code, ss.SASS_Code , ss.SASc_Code,so.SASR_Name,sa.transcode,sa.transdate, sa.description, transtype "
                    'str += sortbyvalue
                    'query modified by Hafiz @ 29/2/2016

                    If Category = "Student Ledger" Then

                        str = "select ss.SASI_Name,ss.SASI_ICNo,ss.sasi_MatricNo,ss.SASI_PgId,ss.SASI_CurSemYr,ss.SASI_Intake,ss.SASI_CurSem,sa.Category,"
                        'str += " ss.SART_Code || ss.SASI_FloorNo || ss.SABK_Code || ss.SAKO_Code Hostel,"
                        str += " (select sk.sako_description from sas_kolej sk where sk.sako_code=ss.SAKO_Code) as Hostel,"
                        str += " ss.SASS_Code , ss.SASc_Code,"
                        str += " to_char(sa.transdate, 'DD/MM/YYYY') Sb_TransDate,sa.description,"
                        str += " CASE WHEN sa.Category = 'Receipt' THEN"
                        str += " CASE WHEN sa.Description != 'CIMB CLICKS' THEN sa.BankRecNo"
                        str += " Else sa.TransCode"
                        str += " End"
                        str += " Else"
                        str += " CASE WHEN Category = 'Loan' THEN BatchCode"
                        str += " Else sa.TransCode"
                        str += " End"
                        str += " END TransCode,"
                        str += " CASE WHEN sa.category = 'Credit Note' or sa.category = 'Debit Note' or sa.category = 'Invoice' then"
                        str += " CASE "
                        str += " WHEN sa.transtype='Credit' THEN sum (de.transamount)"
                        str += " ELSE 0"
                        str += " End"
                        str += " Else"
                        str += " CASE "
                        str += " WHEN sa.transtype='Credit' THEN sum (sa.transamount)"
                        str += " ELSE 0"
                        str += " End"
                        str += " End as Credit,"
                        'str += " CASE WHEN sa.category = 'Credit Note' or sa.category = 'Debit Note' or sa.category = 'Invoice' then"
                        'str += " CASE WHEN sa.transtype='Debit' THEN sum (de.transamount)"
                        'str += " ELSE 0"
                        'str += " End"
                        'str += " Else	CASE WHEN sa.transtype='Debit' THEN sum (sa.transamount)	ELSE 0      End"
                        'str += " END as Debit"
                        str += " CASE when sa.category = 'Credit Note' or sa.category = 'Debit Note' or sa.category = 'Invoice' THEN"
                        str += " CASE when sa.transtype = 'Debit' then SUM(de.TransAmount)"
                        str += " Else 0 End"
                        str += " Else"
                        str += " CASE WHEN sa.TransType = 'Debit' THEN sa.TransAmount"
                        str += " ELSE 0 End"
                        str += " END as Debit"
                        str += " FROM SAS_Student ss  "
                        str += " inner join sas_accounts sa on sa.creditref=ss.sasi_MatricNo  and sa.PostStatus = 'Posted' and sa.SubType = 'Student'"
                        str += " left join sas_accountsdetails de on sa.transid = de.transid"
                        'str += " left join SAS_StudentSpon spo on spo.SASI_MatricNo = ss.SASI_MatricNo"
                        'str += " left join SAS_Sponsor so on so.SASR_Code = spo.SASS_Sponsor"
                        str += " where sa.transtype in  ('Debit','Credit')"
                        str += " and sa.creditref = '" + Session("studentCode") + "' AND sa.subcategory NOT IN ('Loan') AND sa.TransAmount > 0 " ' and sa.Transstatus = 'Closed'
                        str += " group by ss.sasi_name,ss.SASI_ICNo,ss.sasi_MatricNo,ss.SASI_PgId,ss.SASI_CurSemYr,sa.Category, ss.SASI_Intake,ss.SASI_CurSem,ss.SART_Code,"
                        str += " ss.SASI_FloorNo, ss.SABK_Code, ss.SAKO_Code, ss.SASS_Code, ss.SASc_Code, sa.transcode, sa.transdate, sa.description, transtype, sa.bankrecno, sa.batchcode, sa.TransAmount,sa.postedtimestamp"
                        'str += " order by sa.transdate "
                        str += " order by sa.postedtimestamp "
                    ElseIf Category = "Loan Ledger" Then
                        str = "select * "
                        str += " FROM   (select ss.SASI_Name, ss.SASI_ICNo,ss.sasi_MatricNo,ss.SASI_PgId,ss.SASI_CurSemYr,ss.SASI_Intake,ss.SASI_CurSem,lo.Category, lo.subtype,"
                        'str += " ss.SART_Code || ss.SASI_FloorNo || ss.SABK_Code || ss.SAKO_Code Hostel,"
                        str += " (select sk.sako_description from sas_kolej sk where sk.sako_code=ss.SAKO_Code) as Hostel,"
                        str += " ss.SASS_Code , ss.SASc_Code,   lo.transcode, to_char(lo.transdate, 'DD/MM/YYYY') Sb_TransDate, lo.description,"

                        'str += " CASE WHEN lo.TransType = 'Credit' THEN lo.TransAmount"
                        'str += " ELSE 0 END Debit ,"
                        'str += " CASE WHEN lo.TransType = 'Debit' THEN lo.TransAmount "
                        'str += " ELSE 0 END Credit ,"

                        str += " CASE WHEN lo.Category = 'Receipt' THEN "
                        str += " CASE WHEN lo.TransType = 'Debit' THEN lo.TransAmount"
                        str += " ELSE 0"
                        str += " End"
                        str += " Else"
                        str += " CASE WHEN lo.TransType = 'Credit' THEN lo.TransAmount"
                        str += " ELSE 0"
                        str += " End"
                        str += " END Debit ,"

                        str += " CASE WHEN lo.Category = 'Receipt' THEN "
                        str += " CASE WHEN lo.TransType = 'Credit' THEN lo.TransAmount"
                        str += " ELSE 0"
                        str += " End"
                        str += " Else"
                        str += " CASE WHEN lo.TransType = 'Debit' THEN lo.TransAmount"
                        str += " ELSE 0"
                        str += " End"
                        str += " END Credit,"

                        str += " lo.TransAmount, lo.TransType, lo.BatchCode"
                        str += " FROM  SAS_StudentLoan lo"
                        str += " left join sas_student ss on lo.creditref = ss.sasi_matricno"
                        'str += " LEFT JOIN SAS_STUDENTSPON SPO ON SPO.SASI_MATRICNO = SS.SASI_MATRICNO"
                        'str += " LEFT JOIN SAS_SPONSOR SO ON SO.SASR_CODE = SPO.SASS_SPONSOR "
                        str += " WHERE lo.transtype in  ('Debit','Credit') and   lo.CreditRef = '" + Session("studentCode") + "'  AND lo.SubType = 'Student' and lo.PostStatus ='Posted'" ' and lo.Transstatus = 'Closed'
                        str += " ORDER BY lo.postedtimestamp) A"
                        'str += " order by A.Sb_postedtimestamp"
                    Else
                        Response.Write("Please Select Ledger Type")
                        Return
                    End If
                    GetSponsorSQL = "select spon.sasr_code, spon.sasr_name, sspon.sasi_matricno from SAS_STUDENTSPON sspon inner join sas_sponsor spon on sspon.sass_sponsor = spon.sasr_code where sasi_matricno='" + Session("studentCode") + "'"
                End If

                'Author			: Anil Kumar - T-Melmax Sdn Bhd
                'Created Date	: 20/05/2015

                'DataSet Strating
                Dim _DataSet As DataSet = _ReportHelper.GetDataSet(str)
                _DataSet.Tables(0).TableName = "Table"

                Dim _result As New DataSet()
                _result.Tables.Add("Category")
                _result.Tables("Category").Columns.Add("Type")
                Dim newRow As DataRow = _result.Tables("Category").NewRow()
                newRow("Type") = Category
                _result.Tables("Category").Rows.Add(newRow)

                Dim _SponDataSet As DataSet = _ReportHelper.GetDataSet(GetSponsorSQL)
                _SponDataSet.Tables(0).TableName = "StudentSpon"

                _DataSet.Tables.Add(_result.Tables(0).Copy())
                _DataSet.Tables.Add(_SponDataSet.Tables(0).Copy())

                'Report XML Loading

                Dim s As String = Server.MapPath("~/xml/StudentLedger.xml")

                _DataSet.WriteXml(s)

                'Report XML Ended

                'Records Checking

                If _DataSet.Tables(0).Rows.Count = 0 Then
                    Response.Write("No Record Found")

                Else

                    'Report Loading 
                    MyReportDocument.Load(Server.MapPath("~/GroupReport/RptStudentLedger.rpt"))


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
        ' Session("studentCode") = Nothing
        ' Session("status") = Nothing
        'MyReportDocument.Close()
        'MyReportDocument.Dispose()
    End Sub
End Class
