Imports System.Data
Imports System.Data.SqlClient
Imports System.Web.Configuration
Imports CrystalDecisions.CrystalReports.Engine

Partial Class RptStuReconciliationCRFormFinal
    Inherits System.Web.UI.Page
    Private MyReportDocument As ReportDocument

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        'Dim MyReportDocument As ReportDocument = New ReportDocument()
        MyReportDocument = New ReportDocument()
        If Not IsPostBack Then
            Dim program As String
            Dim faculty As String
            Dim sponsor As String
            Dim datecondition As String = ""
            Dim sortbyvalue As String = ""
            Dim sortbyvalue1 As String = ""
            sortbyvalue = "Order By dbo.SAS_Student.SASI_Faculty,dbo.SAS_Student.SASI_PgId,dbo.SAS_Accounts.BankCode"
            sortbyvalue1 = "Order By dbo.SAS_Accounts.BankCode"
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

                datecondition = " and dbo.SAS_Accounts.TransDate between '" + datefrom + "' and '" + dateto + "'"
            End If


            Dim constr As String
            constr = WebConfigurationManager.ConnectionStrings("SASNEWConnectionString").ConnectionString
            Dim str As String = Nothing
            If Session("reconcilation") = 0 Then
                str = " SELECT   CONVERT(VARCHAR(10), dbo.SAS_Accounts.TransDate,103) as TransDate,  dbo.SAS_Accounts.TransCode, dbo.SAS_Accounts.Description, dbo.SAS_Accounts.CreditRef, dbo.SAS_Accounts.Category, dbo.SAS_Accounts.TransAmount, dbo.SAS_Program.SAPG_Program,dbo.SAS_Faculty.SAFC_Desc, "
                str += " dbo.SAS_Accounts.TransStatus, dbo.SAS_Accounts.BankCode, dbo.SAS_BankDetails.SABD_Desc, dbo.SAS_Accounts.TransType,  "
                str += " dbo.SAS_Student.SASI_PgId, dbo.SAS_Student.SASI_Faculty, dbo.SAS_Student.SASI_MatricNo, dbo.SAS_Student.SASI_Name "
                str += " FROM dbo.SAS_Faculty, dbo.SAS_Program, dbo.SAS_Accounts INNER JOIN "
                str += " dbo.SAS_BankDetails ON dbo.SAS_Accounts.BankCode = dbo.SAS_BankDetails.SABD_Code INNER JOIN "
                str += " dbo.SAS_Student ON dbo.SAS_Accounts.CreditRef = dbo.SAS_Student.SASI_MatricNo "
                str += " where dbo.SAS_Faculty.SAFC_Code =dbo.SAS_Student.SASI_Faculty and dbo.SAS_Program.SAPG_Code = dbo.SAS_Student.SASI_PgId and dbo.SAS_Student.SASI_PgId like '" + program + "' and dbo.SAS_Student.SASI_Faculty like '" + faculty + "'"
                str += datecondition + " "
                str += sortbyvalue
            Else
                str = " SELECT  CONVERT(VARCHAR(10), dbo.SAS_Accounts.TransDate,103) as TransDate, dbo.SAS_Accounts.Description,  dbo.SAS_Accounts.TransCode, dbo.SAS_Accounts.CreditRef, dbo.SAS_Accounts.Category, dbo.SAS_Accounts.TransAmount,dbo.SAS_Program.SAPG_Program,dbo.SAS_Faculty.SAFC_Desc,dbo.SAS_Sponsor.SASR_Name, "
                str += " dbo.SAS_Accounts.TransStatus, dbo.SAS_Accounts.BankCode, dbo.SAS_BankDetails.SABD_Desc, dbo.SAS_Accounts.TransType, "
                str += " dbo.SAS_Sponsor.SASR_Code FROM dbo.SAS_Faculty, dbo.SAS_Program, dbo.SAS_Accounts INNER JOIN "
                str += " dbo.SAS_BankDetails ON dbo.SAS_Accounts.BankCode = dbo.SAS_BankDetails.SABD_Code INNER JOIN "
                str += " dbo.SAS_Sponsor ON dbo.SAS_Accounts.CreditRef = dbo.SAS_Sponsor.SASR_Code "
                str += " where dbo.SAS_Sponsor.SASR_Code like '" + sponsor + "'"
                str += datecondition + " "
                str += sortbyvalue1
            End If

            Dim con As SqlConnection = New SqlConnection(constr)
            Dim cmd As SqlCommand = New SqlCommand(str, con)
            Dim da As SqlDataAdapter = New SqlDataAdapter(cmd)
            Dim ds As DataSet = New DataSet()
            da.Fill(ds)

            'Dim s As String = Server.MapPath("xml\ReconciliationStud.xml")
            'ds.WriteXml(s)
            'Dim s As String = Server.MapPath("xml\ReconciliationSponsor.xml")
            'ds.WriteXml(s)
           

            If ds.Tables(0).Rows.Count = 0 Then
                Response.Write("No Record Found")
                Return
            End If

            'CrystalReportViewer1.DisplayGroupTree = False
            'CrystalReportViewer1.HasCrystalLogo = False
            'Dim MyReportDocument As ReportDocument = New ReportDocument()
            If Session("reconcilation") = 0 Then
                MyReportDocument.Load(Server.MapPath("RptStuReconciliationCRFinal1.rpt"))
            Else
                MyReportDocument.Load(Server.MapPath("RptStuReconciliationCRFinalSP3.rpt"))
            End If
            MyReportDocument.SetDataSource(ds)

            Session("reportobject") = MyReportDocument
            CrystalReportViewer1.ReportSource = MyReportDocument
            'CrystalReportViewer1.DataBind()
            MyReportDocument.Refresh()
        Else
            MyReportDocument = Session("reportobject")

            CrystalReportViewer1.ReportSource = MyReportDocument
            'CrystalReportViewer1.DataBind()
            MyReportDocument.Refresh()
        End If
    End Sub

    Protected Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Unload
        'MyReportDocument.Close()
        'MyReportDocument.Dispose()
    End Sub
End Class
