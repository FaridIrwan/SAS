Imports System.Data
Imports System.Data.SqlClient
Imports System.Web.Configuration
Imports CrystalDecisions.CrystalReports.Engine

Partial Class RptDepositLedgerViewer
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

        If Not IsPostBack Then

            Dim sortbyvalue As String = Nothing
            Dim status As String = Nothing
            Dim program As String = Nothing
            Dim faculty As String = Nothing
            Dim sponsor As String = Nothing
            Dim datecondition As String = Nothing
            Dim str As String = Nothing

            If Session("sortby") Is Nothing Then

                sortbyvalue = " Order By SAS_Student.SASI_Faculty,SAS_Student.SASI_PgId,SAS_Student.SASI_Name"

            ElseIf Session("sortby") = "SAS_Accounts.CreditRef" Then

                sortbyvalue = " Order By SAS_Student.SASI_Faculty,SAS_Student.SASI_PgId," + Session("sortby")

            ElseIf Session("sortby") = "SAS_Student.SASI_Name" Then

                sortbyvalue = " Order By SAS_Student.SASI_Faculty,SAS_Student.SASI_PgId," + Session("sortby")

            End If

            If Session("status") Is Nothing Then

                status = "t'"
                status += " or SASI_StatusRec = 'f"
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
                str = " SELECT     SAS_Accounts.CreditRef,  SAS_Accounts.TransDate, SAS_Student.SASI_Name, SAS_Student.SASI_Faculty, SAS_Student.SASI_PgId, SAS_Program.SAPG_Program,SAS_Faculty.SAFC_Desc, "
                str += " SAS_AccountsDetails.RefCode,SAS_Accounts.Description, SAS_FeeTypes.SAFT_Desc, SAS_FeeTypes.SAFT_FeeType, SUM(SAS_AccountsDetails.TransAmount) AS Amount"
                str += " FROM SAS_Faculty, SAS_Program, SAS_Accounts INNER JOIN SAS_AccountsDetails ON SAS_Accounts.TransID = SAS_AccountsDetails.TransID INNER JOIN "
                str += " SAS_Student ON SAS_Accounts.CreditRef = SAS_Student.SASI_MatricNo INNER JOIN "
                str += " SAS_FeeTypes ON SAS_AccountsDetails.RefCode = SAS_FeeTypes.SAFT_Code "
                str += " INNER JOIN SAS_StudentSpon ON SAS_Student.SASI_MatricNo = SAS_StudentSpon.SASI_MatricNo "
                str += " WHERE  SAS_Accounts.TransID = SAS_AccountsDetails.TransID and SAS_Faculty.SAFC_Code =SAS_Student.SASI_Faculty and SAS_Program.SAPG_Code = SAS_Student.SASI_PgId and (SAS_FeeTypes.SAFT_FeeType = '01') "
                str += " and SAS_Student.SASI_StatusRec = '" + status + "' and  SAS_Student.SASI_PgId like '" + program + "'"
                str += " and SAS_Student.SASI_Faculty like '" + faculty + "' "
                str += " and SAS_StudentSpon.SASS_Sponsor like '" + sponsor + "'"
                str += datecondition + " "
                str += " GROUP BY SAS_Accounts.CreditRef, SAS_Student.SASI_Faculty, SAS_Student.SASI_PgId, SAS_AccountsDetails.RefCode,SAS_Accounts.TransDate,  "
                str += " SAS_FeeTypes.SAFT_Desc, SAS_FeeTypes.SAFT_FeeType, SAS_Student.SASI_Name ,  SAS_Student.SASI_Name, SAS_Program.SAPG_Program, SAS_Faculty.SAFC_Desc, SAS_Accounts.Description"
                str += sortbyvalue

            Else

                str = " SELECT     SAS_Accounts.CreditRef, SAS_Student.SASI_Name,SAS_Accounts.TransDate, SAS_Student.SASI_Faculty, SAS_Student.SASI_PgId,SAS_Program.SAPG_Program, SAS_Faculty.SAFC_Desc,  "
                str += " SAS_AccountsDetails.RefCode, SAS_FeeTypes.SAFT_Desc, SAS_FeeTypes.SAFT_FeeType, SUM(SAS_AccountsDetails.TransAmount) AS Amount"
                str += " FROM SAS_Faculty,SAS_program, SAS_Accounts INNER JOIN SAS_AccountsDetails ON SAS_Accounts.TransID = SAS_AccountsDetails.TransID INNER JOIN "
                str += " SAS_Student ON SAS_Accounts.CreditRef = SAS_Student.SASI_MatricNo INNER JOIN "
                str += " SAS_FeeTypes ON SAS_AccountsDetails.RefCode = SAS_FeeTypes.SAFT_Code "
                str += " WHERE   SAS_Faculty.SAFC_Code =SAS_Student.SASI_Faculty and SAS_Program.SAPG_Code = SAS_Student.SASI_PgId and (SAS_FeeTypes.SAFT_FeeType = '01' )  "
                str += " and SAS_Student.SASI_StatusRec = '" + status + "' and  SAS_Student.SASI_PgId like '" + program + "'"
                str += " and  SAS_Student.SASI_Faculty like '" + faculty + "'"
                str += datecondition + " "
                str += " GROUP BY SAS_Accounts.CreditRef, SAS_Student.SASI_Faculty, SAS_Student.SASI_PgId, SAS_AccountsDetails.RefCode,SAS_Accounts.TransDate,  "
                str += " SAS_FeeTypes.SAFT_Desc, SAS_FeeTypes.SAFT_FeeType, SAS_Student.SASI_Name, SAS_Program.SAPG_Program, SAS_Faculty.SAFC_Desc"
                str += sortbyvalue

                '    str = " SELECT     dbo.SAS_Accounts.CreditRef,  dbo.SAS_Accounts.TransDate, dbo.SAS_Student.SASI_Name, dbo.SAS_Student.SASI_Faculty, dbo.SAS_Student.SASI_PgId, dbo.SAS_Program.SAPG_Program,dbo.SAS_Faculty.SAFC_Desc, "
                '    str += " dbo.SAS_AccountsDetails.RefCode,dbo.SAS_Accounts.Description, dbo.SAS_FeeTypes.SAFT_Desc, dbo.SAS_FeeTypes.SAFT_FeeType, SUM(dbo.SAS_AccountsDetails.TransAmount) AS Amount"
                '    str += " FROM dbo.SAS_Faculty, dbo.SAS_Program, dbo.SAS_Accounts INNER JOIN dbo.SAS_AccountsDetails ON dbo.SAS_Accounts.TransID = dbo.SAS_AccountsDetails.TransID INNER JOIN "
                '    str += " dbo.SAS_Student ON dbo.SAS_Accounts.CreditRef = dbo.SAS_Student.SASI_MatricNo INNER JOIN "
                '    str += " dbo.SAS_FeeTypes ON dbo.SAS_AccountsDetails.RefCode = dbo.SAS_FeeTypes.SAFT_Code "
                '    str += " INNER JOIN SAS_StudentSpon ON SAS_Student.SASI_MatricNo = SAS_StudentSpon.SASI_MatricNo "
                '    str += " WHERE  dbo.SAS_Accounts.TransID = dbo.SAS_AccountsDetails.TransID and dbo.SAS_Faculty.SAFC_Code =dbo.SAS_Student.SASI_Faculty and dbo.SAS_Program.SAPG_Code = dbo.SAS_Student.SASI_PgId and (dbo.SAS_FeeTypes.SAFT_FeeType = '01') "
                '    str += " and dbo.SAS_Student.SASI_StatusRec like '" + status + "' and  dbo.SAS_Student.SASI_PgId like '" + program + "'"
                '    str += " and dbo.SAS_Student.SASI_Faculty like '" + faculty + "' "
                '    str += " and dbo.SAS_StudentSpon.SASS_Sponsor like '" + sponsor + "'"
                '    str += datecondition + " "
                '    str += " GROUP BY dbo.SAS_Accounts.CreditRef, dbo.SAS_Student.SASI_Faculty, dbo.SAS_Student.SASI_PgId, dbo.SAS_AccountsDetails.RefCode,dbo.SAS_Accounts.TransDate,  "
                '    str += " dbo.SAS_FeeTypes.SAFT_Desc, dbo.SAS_FeeTypes.SAFT_FeeType, dbo.SAS_Student.SASI_Name ,  dbo.SAS_Student.SASI_Name, SAS_Program.SAPG_Program, SAS_Faculty.SAFC_Desc, SAS_Accounts.Description"
                '    str += sortbyvalue

                'Else

                '    str = " SELECT     dbo.SAS_Accounts.CreditRef, dbo.SAS_Student.SASI_Name,dbo.SAS_Accounts.TransDate, dbo.SAS_Student.SASI_Faculty, dbo.SAS_Student.SASI_PgId,dbo.SAS_Program.SAPG_Program, dbo.SAS_Faculty.SAFC_Desc,  "
                '    str += " dbo.SAS_AccountsDetails.RefCode, dbo.SAS_FeeTypes.SAFT_Desc, dbo.SAS_FeeTypes.SAFT_FeeType, SUM(dbo.SAS_AccountsDetails.TransAmount) AS Amount"
                '    str += " FROM dbo.SAS_Faculty,dbo.SAS_program, dbo.SAS_Accounts INNER JOIN dbo.SAS_AccountsDetails ON dbo.SAS_Accounts.TransID = dbo.SAS_AccountsDetails.TransID INNER JOIN "
                '    str += " dbo.SAS_Student ON dbo.SAS_Accounts.CreditRef = dbo.SAS_Student.SASI_MatricNo INNER JOIN "
                '    str += " dbo.SAS_FeeTypes ON dbo.SAS_AccountsDetails.RefCode = dbo.SAS_FeeTypes.SAFT_Code "
                '    str += " WHERE   dbo.SAS_Faculty.SAFC_Code =dbo.SAS_Student.SASI_Faculty and dbo.SAS_Program.SAPG_Code = dbo.SAS_Student.SASI_PgId and (dbo.SAS_FeeTypes.SAFT_FeeType = '01' )  "
                '    str += " and dbo.SAS_Student.SASI_StatusRec like '" + status + "' and  dbo.SAS_Student.SASI_PgId like '" + program + "'"
                '    str += " and  dbo.SAS_Student.SASI_Faculty like '" + faculty + "'"
                '    str += datecondition + " "
                '    str += " GROUP BY dbo.SAS_Accounts.CreditRef, dbo.SAS_Student.SASI_Faculty, dbo.SAS_Student.SASI_PgId, dbo.SAS_AccountsDetails.RefCode,dbo.SAS_Accounts.TransDate,  "
                '    str += " dbo.SAS_FeeTypes.SAFT_Desc, dbo.SAS_FeeTypes.SAFT_FeeType, dbo.SAS_Student.SASI_Name, SAS_Program.SAPG_Program, SAS_Faculty.SAFC_Desc"
                '    str += sortbyvalue

            End If

            'Author			: Anil Kumar - T-Melmax Sdn Bhd
            'Created Date	: 20/05/2015

            'DataSet Strating
            Dim _DataSet As DataSet = _ReportHelper.GetDataSet(str)

            _DataSet.Tables(0).TableName = "Table"

            'Report AFCReport Loading XML

            Dim s As String = Server.MapPath("~/xml/DepositLedger.xml")
            _DataSet.WriteXml(s)

            'Report AFCReport Ended XML

            'Records Checking

            If _DataSet.Tables(0).Rows.Count = 0 Then
                Response.Write("No Record Found")
                Return
            End If

            'Report Loading 

            MyReportDocument.Load(Server.MapPath("~/GroupReport/RptDepositLedger.rpt"))
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
    End Sub

#End Region

    Protected Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Unload
        'MyReportDocument.Close()
        'MyReportDocument.Dispose()
    End Sub
End Class
