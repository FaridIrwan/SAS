Imports HTS.SAS.Entities
Imports HTS.SAS.BusinessObjects
Imports System.Data
Imports System.Data.SqlClient
Imports System.Web.Configuration
Imports System.Collections.Generic
Imports CrystalDecisions.CrystalReports.Engine
Partial Class RptDunninglattersCRFormFinal1
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim MyReportDocument As ReportDocument = New ReportDocument()
        If Not IsPostBack Then
            'Dim code As String
            Dim code, Title, Ref, FromDate, Msg, SignBy, OfficerName, Todate As String

            code = Request.QueryString("codeid")
            Title = Request.QueryString("Title")
            Ref = Request.QueryString("Ref")
            FromDate = Request.QueryString("From")
            Msg = Request.QueryString("Msg")
            SignBy = Request.QueryString("SignBy")
            OfficerName = Request.QueryString("Name")
            Todate = Request.QueryString("Todate")

            'Dim eobjStuEn As New StudentEn
            'Dim eobjStubs As New StudentBAL
            'Dim listStu As New List(Of StudentEn)
            'eobjStuEn.MatricNo = code
            'eobjStuEn = eobjStubs.GetItem(eobjStuEn)
            'If Not eobjStuEn.MatricNo Is Nothing Then
            '    Session("MatricNo") = eobjStuEn
            '    listStu.Add(eobjStuEn)
            '    'Else
            '    '    lblMesg.Visible = True
            '    '    lblMesg.Text = "Record does not exist"
            'End If


            Dim constr As String
            constr = WebConfigurationManager.ConnectionStrings("SASConnectionString").ConnectionString

            Dim sname, sid, faulty, pgid, intake As String
            Dim str As String = "SELECT SASI_Name, SASI_ID, SASI_Faculty, SASI_PgId, SASI_Intake"
            str += " FROM dbo.SAS_Student where SASI_MatricNo='" + code + "'"

            Dim creditamount As String = " SELECT SUM(SAS_Accounts.TransAmount) AS DebitAmount FROM SAS_Accounts INNER JOIN SAS_Student ON"
            creditamount += " SAS_Accounts.CreditRef = dbo.SAS_Student.SASI_MatricNo WHERE SAS_Accounts.TransType = 'Credit' "
            creditamount += " AND SAS_Accounts.poststatus = 'Posted' AND SASI_MatricNo = '" + code + "'"

            Dim debitamount As String = " SELECT SUM(SAS_Accounts.TransAmount) AS DebitAmount FROM SAS_Accounts INNER JOIN SAS_Student ON"
            debitamount += " SAS_Accounts.CreditRef = dbo.SAS_Student.SASI_MatricNo WHERE SAS_Accounts.TransType = 'Debit' "
            debitamount += " AND SAS_Accounts.poststatus = 'Posted' AND SASI_MatricNo = '" + code + "'"

            Dim con As SqlConnection = New SqlConnection(constr)
            Dim cmd As SqlCommand
            cmd = New SqlCommand(debitamount, con)
            con.Open()

            Session("debitamount") = cmd.ExecuteScalar()
            cmd.Dispose()

            cmd = New SqlCommand(creditamount, con)
            Session("creditamount") = cmd.ExecuteScalar()
            If Session("creditamount") Is DBNull.Value Then
                Session("creditamount") = 0
            End If
            cmd.Dispose()

            Dim totdr As Integer = Session("debitamount")
            Dim totcr As Integer = Session("creditamount")
            Dim outstan As Integer = Val(totdr) - Val(totcr)
            Session("outstanding") = outstan

            cmd = New SqlCommand(str, con)
            'Dim cmd As SqlCommand = New SqlCommand(str, con)

            Dim reader As SqlDataReader = cmd.ExecuteReader()
            If reader.HasRows Then
                Do While reader.Read()
                    sname = reader(0)
                    sid = reader(1)
                    faulty = reader(2)
                    pgid = reader(3)
                    intake = reader(4)
                Loop
                'End If
                'con.Close()

                'Dim da As SqlDataAdapter = New SqlDataAdapter(cmd)
                'Dim ds As DataSet = New DataSet()

                'da.Fill(ds)
                'If ds.Tables(0).Rows.Count = 0 Then
                '    Response.Write("No Record Found")
                '    Return
                'End If
                'ds.Tables(0).Columns.Add(Title)

                'ds.WriteXml("d:\Dunningletter.xml")

                CrystalReportViewer1.HasCrystalLogo = False

                MyReportDocument.Load(Server.MapPath("RptDunninglattersCRFinal.rpt"))
                'MyReportDocument.SetParameterValue("dateperiord", Session("dateperiord"))

                MyReportDocument.SetParameterValue("sname", sname)
                MyReportDocument.SetParameterValue("sid", sid)
                MyReportDocument.SetParameterValue("faulty", faulty)
                MyReportDocument.SetParameterValue("pgid", pgid)
                MyReportDocument.SetParameterValue("intake", intake)
                MyReportDocument.SetParameterValue("FromDate", FromDate)
                MyReportDocument.SetParameterValue("Title", Title)
                MyReportDocument.SetParameterValue("Todate", Todate)
                MyReportDocument.SetParameterValue("OfficerName", OfficerName)
                MyReportDocument.SetParameterValue("outstanding", outstan)

                'MyReportDocument.SetDataSource(ds)

                Session("reportobject") = MyReportDocument
                CrystalReportViewer1.ReportSource = MyReportDocument
                CrystalReportViewer1.DataBind()
            Else
                Response.Write("Record Not Found")
            End If
            con.Close()
        Else
            MyReportDocument = Session("reportobject")
            CrystalReportViewer1.ReportSource = MyReportDocument
            CrystalReportViewer1.DataBind()
        End If
    End Sub

End Class
