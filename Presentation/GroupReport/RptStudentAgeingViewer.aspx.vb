Imports System.Data
Imports System.Data.SqlClient
Imports System.Web.Configuration
Imports CrystalDecisions.CrystalReports.Engine

Partial Class RptStudentAgeingViewer
    Inherits System.Web.UI.Page
    Private MyReportDocument As New ReportDocument

#Region "Page Load Starting  "

    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init

        'Modified by Hafiz @ 08/11/2016 for NEW RA

        Try
            If Not IsPostBack Then

                Dim str As String = Nothing
                Dim _ReportHelper As New ReportHelper

                If Request.QueryString("Report") = "1" Then

                    'Ageing Report Based On Student Matric ID - START
                    Dim FeeType As String = Request.QueryString("FeeType")
                    Dim Info As String = Request.QueryString("Info")
                    Dim AgeingBy As String = Request.QueryString("AgeingBy")
                    Dim DateTo As String = Nothing
                    Dim DateFrom As String = Nothing
                    Dim ByDate As String = Request.QueryString("ByDate")
                    Dim d2, m2, y2 As String

                    Dim constr As String() = Info.Split(";")

                    d2 = Mid(ByDate, 1, 2)
                    m2 = Mid(ByDate, 4, 2)
                    y2 = Mid(ByDate, 7, 4)
                    DateTo = y2 + "/" + m2 + "/" + d2
                    DateFrom = "2013/01/01"

                    Dim Query As String = Nothing
                    For i As Integer = 0 To constr.Length - 1

                        If constr(i) <> "" Then
                            If constr(i).Equals("program") Then
                                'this query will include program in the select statement
                                Query = "SELECT DISTINCT TB3.SASI_Matricno MatricNo,TB3.SASI_Name AS Name,TB3.SASI_PgId || ' - ' || C.SAPG_ProgramBM AS Program,"
                                Query += LoadAgeingByQuery(AgeingBy, DateTo)
                                Exit For
                            Else
                                'this query is default query which contains matricno
                                Query = "SELECT DISTINCT TB3.SASI_Matricno MatricNo,TB3.SASI_Name AS Name,"
                                Query += LoadAgeingByQuery(AgeingBy, DateTo)
                            End If
                        End If

                    Next

                    str = Query
                    str += "TO_CHAR(DATE '" + DateTo + "', 'DD/MM/YYYY')AS DateTo "
                    str += "FROM SAS_Accounts SA "
                    str += "INNER JOIN (SELECT B.SASI_MatricNo, B.SASI_Name, "
                    str += "B.SASI_PgId, B.SASI_Faculty, B.SASS_Code, "
                    str += "B.SASI_StatusRec, B.SASI_Intake "
                    str += "FROM SAS_Student B) TB3 ON SA.CreditRef = TB3.SASI_MatricNo "
                    str += "LEFT JOIN SAS_Program C ON C.SAPG_Code = TB3.SASI_PgId "
                    str += "LEFT JOIN SAS_Faculty D ON D.SAFC_Code = TB3.SASI_Faculty "
                    str += "LEFT JOIN (select SS.sasi_matricno,(SS.saso_outstandingamt) amount "
                    str += "from sas_studentoutstanding SS "
                    str += "inner join sas_accounts SA ON SA.creditref=SS.sasi_matricno "
                    str += "WHERE SA.TransDate BETWEEN '" + DateFrom + "' AND '" + DateTo + "' ) TB2 ON TB3.SASI_MatricNo = TB2.sasi_matricno "
                    str += "INNER JOIN SAS_Feestruct SF ON SF.SAST_Code = TB3.SASI_Intake AND SF.SABP_Code = (SELECT SABP_Code FROM SAS_Program WHERE SAPG_Code=C.SAPG_Code) "
                    str += "INNER JOIN SAS_Feestrdetails SFD ON SFD.SAFS_Code = SF.SAFS_Code "
                    str += "WHERE TB3.SASI_StatusRec = '1' "
                    str += "AND TB2.amount > '0' "

                    If Not String.IsNullOrEmpty(FeeType) Then
                        str += "AND SFD.SAFT_Code = '" & FeeType & "'"
                    End If

                    Dim _DataSet As DataSet = _ReportHelper.GetDataSet(str)
                    _DataSet.Tables(0).TableName = "Table"

                    Dim s As String = Server.MapPath("~/xml/StudentAgeingType1.xml")
                    _DataSet.WriteXml(s)

                    Dim CheckboxType As String = AgeingBy

                    If _DataSet.Tables(0).Rows.Count = 0 Then
                        Response.Write("No Record Found")
                    Else
                        MyReportDocument.Load(Server.MapPath("~/GroupReport/RptStudentAgeingType1.rpt"))
                        MyReportDocument.SetDataSource(_DataSet)
                        Session("reportobject") = MyReportDocument
                        CrystalReportViewer1.ReportSource = MyReportDocument
                        MyReportDocument.DataDefinition.FormulaFields("CBType").Text = "'" & CheckboxType & "'"
                        CrystalReportViewer1.DataBind()
                        MyReportDocument.Refresh()
                    End If
                    'Ageing Report Based On Student Matric ID - END

                ElseIf Request.QueryString("Report") = "2" Then

                End If
               

            Else

                MyReportDocument = Session("reportobject")
                CrystalReportViewer1.ReportSource = MyReportDocument
                CrystalReportViewer1.DataBind()
                MyReportDocument.Refresh()

            End If

        Catch ex As Exception
            Response.Write(ex.Message)
        End Try

    End Sub

#End Region

#Region "LoadAgeingByQuery"
    'created by Hafiz @ 8/11/2016 for NEW RA

    Protected Function LoadAgeingByQuery(ByVal AgeingBy As String, ByVal DateTo As String) As String

        Dim AgeingByQuery As String = Nothing

        If AgeingBy = "rbYearly" Then
            'yearly - START
            For yr As Integer = 0 To -4 Step -1

                'Dim res As Date = dt.AddYears(yr)
                'Dim YearColumn As String = "<" & CStr(res.Year)

                Select Case yr
                    Case 0
                        AgeingByQuery += "CASE WHEN '" & DateTo & "'::date - SA.TransDate::date <= 365 THEN COALESCE(TB2.amount, 0) ELSE '0.00' END AS ""First"","
                    Case -1
                        AgeingByQuery += "CASE WHEN '" & DateTo & "'::date - SA.TransDate::date > 365 AND '" + DateTo + "'::date - SA.TransDate::date <= 730 THEN COALESCE(TB2.amount, 0) ELSE '0.00' END AS ""Second"","
                    Case -2
                        AgeingByQuery += "CASE WHEN '" & DateTo & "'::date - SA.TransDate::date > 730 AND '" + DateTo + "'::date - SA.TransDate::date <= 1095 THEN COALESCE(TB2.amount, 0) ELSE '0.00' END AS ""Third"","
                    Case -3
                        AgeingByQuery += "CASE WHEN '" & DateTo & "'::date - SA.TransDate::date > 1095 AND '" + DateTo + "'::date - SA.TransDate::date <= 1460 THEN COALESCE(TB2.amount, 0) ELSE '0.00' END AS ""Fourth"","
                    Case -4
                        AgeingByQuery += "CASE WHEN '" & DateTo & "'::date - SA.TransDate::date > 1460 THEN COALESCE(TB2.amount, 0) ELSE '0.00' END AS ""Fifth"","
                End Select

            Next

            AgeingByQuery += "CASE WHEN '" + DateTo + "'::date - SA.TransDate::date <= 365 THEN COALESCE(TB2.amount, 0) ELSE '0.00' END + "
            AgeingByQuery += "CASE WHEN '" + DateTo + "'::date - SA.TransDate::date > 365 AND '" + DateTo + "'::date - SA.TransDate::date <= 730 THEN COALESCE(TB2.amount, 0) ELSE '0.00' END + "
            AgeingByQuery += "CASE WHEN '" + DateTo + "'::date - SA.TransDate::date > 730 AND '" + DateTo + "'::date - SA.TransDate::date <= 1095 THEN COALESCE(TB2.amount, 0) ELSE '0.00' END + "
            AgeingByQuery += "CASE WHEN '" + DateTo + "'::date - SA.TransDate::date > 1095 AND '" + DateTo + "'::date - SA.TransDate::date <= 1460 THEN COALESCE(TB2.amount, 0) ELSE '0.00' END + "
            AgeingByQuery += "CASE WHEN '" + DateTo + "'::date - SA.TransDate::date > 1460 THEN COALESCE(TB2.amount, 0) ELSE '0.00' END AS TotalAmount,"
            'yearly - END

        ElseIf AgeingBy = "rbVariousMonths" Then
            '6/12/36 months - START
            AgeingByQuery += "CASE WHEN '" & DateTo & "'::date - SA.TransDate::date <= 182.5 THEN COALESCE(TB2.amount, 0) ELSE '0.00' END AS ""First"","
            AgeingByQuery += "CASE WHEN '" & DateTo & "'::date - SA.TransDate::date > 182.5 AND '" + DateTo + "'::date - SA.TransDate::date <= 365 THEN COALESCE(TB2.amount, 0) ELSE '0.00' END AS ""Second"","
            AgeingByQuery += "CASE WHEN '" & DateTo & "'::date - SA.TransDate::date > 365 AND '" + DateTo + "'::date - SA.TransDate::date <= 547.5 THEN COALESCE(TB2.amount, 0) ELSE '0.00' END AS ""Third"","
            AgeingByQuery += "CASE WHEN '" & DateTo & "'::date - SA.TransDate::date > 547.5 AND '" + DateTo + "'::date - SA.TransDate::date <= 730 THEN COALESCE(TB2.amount, 0) ELSE '0.00' END AS ""Fourth"","
            AgeingByQuery += "CASE WHEN '" & DateTo & "'::date - SA.TransDate::date > 730 THEN COALESCE(TB2.amount, 0) ELSE '0.00' END AS ""Fifth"","

            AgeingByQuery += "CASE WHEN '" + DateTo + "'::date - SA.TransDate::date <= 182.5 THEN COALESCE(TB2.amount, 0) ELSE '0.00' END + "
            AgeingByQuery += "CASE WHEN '" + DateTo + "'::date - SA.TransDate::date > 182.5 AND '" + DateTo + "'::date - SA.TransDate::date <= 365 THEN COALESCE(TB2.amount, 0) ELSE '0.00' END + "
            AgeingByQuery += "CASE WHEN '" + DateTo + "'::date - SA.TransDate::date > 365 AND '" + DateTo + "'::date - SA.TransDate::date <= 547.5 THEN COALESCE(TB2.amount, 0) ELSE '0.00' END + "
            AgeingByQuery += "CASE WHEN '" + DateTo + "'::date - SA.TransDate::date > 547.5 AND '" + DateTo + "'::date - SA.TransDate::date <= 730 THEN COALESCE(TB2.amount, 0) ELSE '0.00' END + "
            AgeingByQuery += "CASE WHEN '" + DateTo + "'::date - SA.TransDate::date > 730 THEN COALESCE(TB2.amount, 0) ELSE '0.00' END AS TotalAmount,"
            '6/12/36 months - END
        End If

        Return AgeingByQuery

    End Function

#End Region

#Region "Page_Unload"

    Protected Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Unload
        'MyReportDocument.Close()
        'MyReportDocument.Dispose()
    End Sub

#End Region

End Class
