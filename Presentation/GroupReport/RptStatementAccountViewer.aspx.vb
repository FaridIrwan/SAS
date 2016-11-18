Imports System.Data
Imports System.Data.SqlClient
Imports System.Web.Configuration
Imports CrystalDecisions.CrystalReports.Engine

Partial Class RptStatementAccountViewer
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
        'Modified by Hafiz @ 07/3/2016
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
                Session("reportobject") = Nothing
                Dim str As String = Nothing

                If Session("sortby") Is Nothing Then
                    sortbyvalue = " Order By t.SASI_Name"
                ElseIf Session("sortby") = "t.SASI_MatricNo" Then
                    sortbyvalue = " Order By " + Session("sortby")
                ElseIf Session("sortby") = "t.SASI_Name" Then
                    sortbyvalue = " Order By " + Session("sortby")
                    'ElseIf Session("sortby") = "t.Sponsor" Then
                    '    sortbyvalue = " Order By t.Sponsor "
                    'ElseIf Session("sortby") = "t.SASI_Postcode" Then
                    '    sortbyvalue = " Order By " + Session("sortby")
                End If

                If Session("status") Is Nothing Then
                    status = " "
                Else
                    status = " and ss.SASS_Code IN " + _ReportHelper.Status(Session("status")) + " "
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
                    datecondition = " "
                    dateconditionLO = " "
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

                'message text area - start
                Dim textMessages As String = Nothing

                If Session("textarea") Is Nothing Then
                    textMessages = ""
                Else
                    textMessages = Session("textarea")
                End If
                'message text area - end

                'mofified query by Hafiz @ 14/4/2016
                If Not sponsor = "%" Then

                    str = "SELECT * FROM ( "
                    str += " (select ss.SASI_Name,ss.SASI_ICNo,ss.sasi_MatricNo,ss.SASI_PgId,"
                    str += " case when ss.SASI_PgId != '' then"
                    str += " (select sapg_program from SAS_Program"
                    str += " WHERE SAPG_Code = ss.SASI_PgId)"
                    str += " else '' end as Program,"
                    str += " ss.SASI_CurSemYr,"
                    str += " ss.SASI_Intake,ss.SASI_CurSem AS No_Of_Sem,sa.Category, "
                    str += " (select sk.sako_description from sas_kolej sk where sk.sako_code=ss.SAKO_Code) as Hostel, "
                    str += " case when ss.SASS_Code != '' Then"
                    str += " (SELECT SASS_code ||'-'|| SASS_description FROM SAS_StudentStatus"
                    str += " WHERE SASS_code = ss.SASS_Code) end Curr_Status,"
                    str += " ss.SASc_Code, to_char(sa.transdate, 'DD/MM/YYYY') Sb_TransDate,sa.description, "
                    str += " CASE WHEN sa.Category = 'Receipt' THEN "
                    str += " CASE WHEN sa.Description != 'CIMB CLICKS' THEN "
                    str += " sa.BankRecNo Else sa.BatchCode End"
                    str += " Else "
                    str += " CASE WHEN Category = 'Loan' THEN "
                    str += " BatchCode Else sa.TransCode End"
                    str += " END TransCode, "
                    str += " CASE WHEN sa.category = 'Credit Note' or sa.category = 'Debit Note' or sa.category = 'Invoice' then "
                    str += " CASE  WHEN sa.transtype='Credit' THEN "
                    str += " sum(de.transamount)"
                    str += " ELSE 0 "
                    str += " End"
                    str += " Else "
                    str += " CASE  WHEN sa.transtype='Credit' THEN "
                    str += " sum(sa.transamount)"
                    str += " ELSE 0 "
                    str += " End"
                    str += " End as Credit, "
                    str += " CASE WHEN sa.category = 'Credit Note' or sa.category = 'Debit Note' or sa.category = 'Invoice' then "
                    str += " CASE WHEN sa.transtype='Debit' THEN "
                    str += " sum(de.transamount)"
                    str += " ELSE 0 "
                    str += " End"
                    str += " Else	"
                    str += " CASE WHEN sa.transtype='Debit' THEN "
                    str += " sa.transamount"
                    str += " ELSE 0      "
                    str += " End"
                    str += " END as Debit,"
                    str += " st.SASS_Sponsor ||'-'|| sp.SASR_name As Sponsor,st.SASS_SDate as Valid_From,st.SASS_EDate as Valid_To  "
                    str += " from SAS_Student ss   "
                    str += " inner join sas_accounts sa on sa.creditref=ss.sasi_MatricNo  and sa.PostStatus = 'Posted' and sa.SubType = 'Student' "
                    str += " left join sas_accountsdetails de on sa.transid = de.transid"
                    str += " LEFT JOIN SAS_StudentSpon st ON sa.CreditRef = st.SASI_MatricNo"
                    str += " LEFT JOIN SAS_Sponsor sp ON st.SASS_Sponsor = sp.SASR_Code "
                    str += " where sa.transtype in  ('Debit','Credit') "
                    str += " AND sa.subcategory NOT IN ('Loan') "
                    str += " AND ss.SASI_PgId like '" + program + "'"
                    str += " AND ss.SASI_Faculty like '" + faculty + "'"
                    str += " AND st.SASS_Sponsor like '" + sponsor + "'"
                    str += status
                    str += datecondition
                    str += " group by ss.sasi_name,ss.SASI_ICNo,ss.sasi_MatricNo,ss.SASI_PgId,ss.SASI_CurSemYr,sa.Category, ss.SASI_Intake,ss.SASI_CurSem,"
                    str += " ss.SART_Code, ss.SASI_FloorNo, ss.SABK_Code, ss.SAKO_Code, ss.SASS_Code, ss.SASc_Code, sa.transcode, sa.transdate, sa.description, "
                    str += " transtype, sa.bankrecno, sa.batchcode, st.SASS_Sponsor, sp.SASR_name, st.SASS_SDate, st.SASS_EDate, sa.transamount)"

                    str += " UNION"

                    str += " (select *  "
                    str += " FROM"
                    str += " (select ss.SASI_Name, ss.SASI_ICNo,ss.sasi_MatricNo,ss.SASI_PgId,"
                    str += " case when ss.SASI_PgId != '' then"
                    str += " (select sapg_program from SAS_Program"
                    str += " WHERE SAPG_Code = ss.SASI_PgId)"
                    str += " else '' end as Program,"
                    str += " ss.SASI_CurSemYr,ss.SASI_Intake,ss.SASI_CurSem AS No_Of_Sem,lo.Category,"
                    str += " (select sk.sako_description "
                    str += " from sas_kolej sk "
                    str += " where sk.sako_code=ss.SAKO_Code) as Hostel,"
                    str += " case when ss.SASS_Code != '' Then"
                    str += " (SELECT SASS_code ||'-'|| SASS_description FROM SAS_StudentStatus"
                    str += " WHERE SASS_code = ss.SASS_Code) end Curr_Status,"
                    str += " ss.SASc_Code, to_char(lo.transdate, 'DD/MM/YYYY') Sb_TransDate, lo.description, lo.transcode,"
                    str += " CASE WHEN lo.Category = 'Receipt' THEN  "
                    str += " CASE WHEN lo.TransType = 'Debit' THEN "
                    str += " lo.TransAmount"
                    str += " ELSE 0 "
                    str += " End"
                    str += " Else "
                    str += " CASE WHEN lo.TransType = 'Credit' THEN "
                    str += " lo.TransAmount"
                    str += " ELSE 0 "
                    str += " End"
                    str += " END Debit , "
                    str += " CASE WHEN lo.Category = 'Receipt' THEN  "
                    str += " CASE WHEN lo.TransType = 'Credit' THEN "
                    str += " lo.TransAmount"
                    str += " ELSE 0 "
                    str += " End"
                    str += " Else "
                    str += " CASE WHEN lo.TransType = 'Debit' THEN "
                    str += " lo.TransAmount"
                    str += " ELSE 0 "
                    str += " End"
                    str += " END Credit,"
                    str += " st.SASS_Sponsor ||'-'|| sp.SASR_name As Sponsor,st.SASS_SDate as Valid_From,st.SASS_EDate as Valid_To"
                    str += " FROM  SAS_StudentLoan lo "
                    str += " left join sas_student ss on lo.creditref = ss.sasi_matricno "
                    str += " LEFT JOIN SAS_StudentSpon st ON ss.sasi_matricno = st.SASI_MatricNo"
                    str += " LEFT JOIN SAS_Sponsor sp ON st.SASS_Sponsor = sp.SASR_Code "
                    str += " where lo.transtype in  ('Debit','Credit') "
                    str += " AND lo.SubType = 'Student' and lo.PostStatus ='Posted'"
                    str += " AND ss.SASI_PgId like '" + program + "'"
                    str += " AND ss.SASI_Faculty like '" + faculty + "'"
                    str += " AND st.SASS_Sponsor like '" + sponsor + "'"
                    str += status
                    str += dateconditionLO
                    str += " ) A ) ) t "
                    str += sortbyvalue

                Else

                    str = "SELECT * FROM ( "
                    str += " (select ss.SASI_Name,ss.SASI_ICNo,ss.sasi_MatricNo,ss.SASI_PgId,"
                    str += " case when ss.SASI_PgId != '' then"
                    str += " (select sapg_program from SAS_Program"
                    str += " WHERE SAPG_Code = ss.SASI_PgId)"
                    str += " else '' end as Program,"
                    str += " ss.SASI_CurSemYr,"
                    str += " ss.SASI_Intake,ss.SASI_CurSem AS No_Of_Sem,sa.Category, "
                    str += " (select sk.sako_description from sas_kolej sk where sk.sako_code=ss.SAKO_Code) as Hostel, "
                    str += " case when ss.SASS_Code != '' Then"
                    str += " (SELECT SASS_code ||'-'|| SASS_description FROM SAS_StudentStatus"
                    str += " WHERE SASS_code = ss.SASS_Code) end Curr_Status,"
                    str += " ss.SASc_Code, to_char(sa.transdate, 'DD/MM/YYYY') Sb_TransDate,sa.description, "
                    str += " CASE WHEN sa.Category = 'Receipt' THEN "
                    str += " CASE WHEN sa.Description != 'CIMB CLICKS' THEN "
                    str += " sa.BankRecNo Else sa.BatchCode End"
                    str += " Else "
                    str += " CASE WHEN Category = 'Loan' THEN "
                    str += " BatchCode Else sa.TransCode End"
                    str += " END TransCode, "
                    str += " CASE WHEN sa.category = 'Credit Note' or sa.category = 'Debit Note' or sa.category = 'Invoice' then "
                    str += " CASE  WHEN sa.transtype='Credit' THEN "
                    str += " sum(de.transamount)"
                    str += " ELSE 0 "
                    str += " End"
                    str += " Else "
                    str += " CASE  WHEN sa.transtype='Credit' THEN "
                    str += " sum(sa.transamount)"
                    str += " ELSE 0 "
                    str += " End"
                    str += " End as Credit, "
                    str += " CASE WHEN sa.category = 'Credit Note' or sa.category = 'Debit Note' or sa.category = 'Invoice' then "
                    str += " CASE WHEN sa.transtype='Debit' THEN "
                    str += " sum(de.transamount)"
                    str += " ELSE 0 "
                    str += " End"
                    str += " Else	"
                    str += " CASE WHEN sa.transtype='Debit' THEN "
                    str += " sa.transamount"
                    str += " ELSE 0      "
                    str += " End"
                    str += " END as Debit,"
                    str += " st.SASS_Sponsor ||'-'|| sp.SASR_name As Sponsor,st.SASS_SDate as Valid_From,st.SASS_EDate as Valid_To  "
                    str += " from SAS_Student ss   "
                    str += " inner join sas_accounts sa on sa.creditref=ss.sasi_MatricNo  and sa.PostStatus = 'Posted' and sa.SubType = 'Student' "
                    str += " left join sas_accountsdetails de on sa.transid = de.transid"
                    str += " LEFT JOIN SAS_StudentSpon st ON sa.CreditRef = st.SASI_MatricNo"
                    str += " LEFT JOIN SAS_Sponsor sp ON st.SASS_Sponsor = sp.SASR_Code "
                    str += " where sa.transtype in  ('Debit','Credit') "
                    str += " AND sa.subcategory NOT IN ('Loan') "
                    str += " AND ss.SASI_PgId like '" + program + "'"
                    str += " AND ss.SASI_Faculty like '" + faculty + "'"
                    str += status
                    str += datecondition + " "
                    str += " group by ss.sasi_name,ss.SASI_ICNo,ss.sasi_MatricNo,ss.SASI_PgId,ss.SASI_CurSemYr,sa.Category, ss.SASI_Intake,ss.SASI_CurSem,"
                    str += " ss.SART_Code, ss.SASI_FloorNo, ss.SABK_Code, ss.SAKO_Code, ss.SASS_Code, ss.SASc_Code, sa.transcode, sa.transdate, sa.description, "
                    str += " transtype, sa.bankrecno, sa.batchcode, st.SASS_Sponsor, sp.SASR_name, st.SASS_SDate, st.SASS_EDate,sa.transamount)"

                    str += " UNION"

                    str += " (select *  "
                    str += " FROM"
                    str += " (select ss.SASI_Name, ss.SASI_ICNo,ss.sasi_MatricNo,ss.SASI_PgId,"
                    str += " case when ss.SASI_PgId != '' then"
                    str += " (select sapg_program from SAS_Program"
                    str += " WHERE SAPG_Code = ss.SASI_PgId)"
                    str += " else '' end as Program,"
                    str += " ss.SASI_CurSemYr,ss.SASI_Intake,ss.SASI_CurSem AS No_Of_Sem,lo.Category,"
                    str += " (select sk.sako_description "
                    str += " from sas_kolej sk "
                    str += " where sk.sako_code=ss.SAKO_Code) as Hostel,"
                    str += " case when ss.SASS_Code != '' Then"
                    str += " (SELECT SASS_code ||'-'|| SASS_description FROM SAS_StudentStatus"
                    str += " WHERE SASS_code = ss.SASS_Code) end Curr_Status,"
                    str += " ss.SASc_Code, to_char(lo.transdate, 'DD/MM/YYYY') Sb_TransDate, lo.description, lo.transcode,"
                    str += " CASE WHEN lo.Category = 'Receipt' THEN  "
                    str += " CASE WHEN lo.TransType = 'Debit' THEN "
                    str += " lo.TransAmount"
                    str += " ELSE 0 "
                    str += " End"
                    str += " Else "
                    str += " CASE WHEN lo.TransType = 'Credit' THEN "
                    str += " lo.TransAmount"
                    str += " ELSE 0 "
                    str += " End"
                    str += " END Debit , "
                    str += " CASE WHEN lo.Category = 'Receipt' THEN  "
                    str += " CASE WHEN lo.TransType = 'Credit' THEN "
                    str += " lo.TransAmount"
                    str += " ELSE 0 "
                    str += " End"
                    str += " Else "
                    str += " CASE WHEN lo.TransType = 'Debit' THEN "
                    str += " lo.TransAmount"
                    str += " ELSE 0 "
                    str += " End"
                    str += " END Credit,"
                    str += " st.SASS_Sponsor ||'-'|| sp.SASR_name As Sponsor,st.SASS_SDate as Valid_From,st.SASS_EDate as Valid_To"
                    str += " FROM  SAS_StudentLoan lo "
                    str += " left join sas_student ss on lo.creditref = ss.sasi_matricno "
                    str += " LEFT JOIN SAS_StudentSpon st ON ss.sasi_matricno = st.SASI_MatricNo"
                    str += " LEFT JOIN SAS_Sponsor sp ON st.SASS_Sponsor = sp.SASR_Code "
                    str += " where lo.transtype in  ('Debit','Credit') "
                    str += " AND lo.SubType = 'Student' and lo.PostStatus ='Posted'"
                    str += " AND ss.SASI_PgId like '" + program + "'"
                    str += " AND ss.SASI_Faculty like '" + faculty + "'"
                    str += status
                    str += dateconditionLO
                    str += " ) A ) ) t "
                    str += sortbyvalue

                End If

                Dim _DataSet As DataSet = _ReportHelper.GetDataSet(str)
                _DataSet.Tables(0).TableName = "Table"

                'modified by Hafiz @ 12/4/2016
                'get messages textarea - start
                Dim _newDataSet As New DataSet()

                _newDataSet.Tables.Add("Messages")
                _newDataSet.Tables("Messages").Columns.Add("text")

                Dim _newRow As DataRow = _newDataSet.Tables("Messages").NewRow()

                _newRow("text") = textMessages
                _newDataSet.Tables("Messages").Rows.Add(_newRow)

                _DataSet.Tables.Add(_newDataSet.Tables(0).Copy())
                'get messages textarea - end

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
                'MyReportDocument.Load(Server.MapPath("RptStatementAccount.rpt"))
                MyReportDocument.Load(Server.MapPath("RptStatementAccount_NEW.rpt"))
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
