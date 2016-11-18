Imports HTS.SAS.Entities
Imports HTS.SAS.BusinessObjects
Imports HTS.SAS.DataAccessObjects
Imports System.Linq
Imports System.Collections.Generic
Imports CrystalDecisions.CrystalReports.Engine
Imports CrystalDecisions.Shared
Imports CrystalDecisions.Web
Imports CrystalDecisions.ReportSource
Imports CrystalDecisions.CrystalReports
Imports System.Data
Imports System.Data.SqlClient
Imports System.Configuration
Imports System.Web.Configuration
Imports System.Net.Configuration
Imports System.Net.Mail
Imports System.Net

Partial Class LookupSponsorCoverLetter
    Inherits System.Web.UI.Page

    Dim ListObjects As List(Of SponsorCoverLetterEn)
    Private MyReportDocument As New ReportDocument
    Private _ReportHelper As New ReportHelper
    Private _Helper As New Helper

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Not IsPostBack() Then
            SponsorCoverLetterAttributes()
            LoadListObjects()
            ddlCode.SelectedIndex = 0
            DisplayEmptyResult()
            lblMsg.Text = ""
            Session("SCLCode") = Nothing
        End If
        Session("SCLType") = rblSCLType.SelectedValue.ToString()
        Session("SCLLang") = rblSCLLang.SelectedValue.ToString()
    End Sub


    Private Sub SponsorCoverLetterAttributes()
        ibtnTodate.Attributes.Add("onClick", "return getDateto()")
        ibtnFDate.Attributes.Add("onClick", "return getibtnFDate()")
        txtFrom.Attributes.Add("OnKeyup", "return CheckFromDate()")
        txtToDate.Attributes.Add("OnKeyup", "return CheckToDate()")
        btnPrintSCL.Attributes.Add("onClick", "return validate()")
        btnEmailSCL.Attributes.Add("onClick", "return validate()")
        dates()
    End Sub
    Private Sub dates()
        txtFrom.Text = Format(Date.Now, "dd/MM/yyyy")
        txtToDate.Text = Format(Date.Now, "dd/MM/yyyy")
    End Sub

    Public Sub LoadListObjects()
        Dim bobj As New SponsorCoverLetterBAL
        Dim eobj As New SponsorCoverLetterEn
        Dim ListObjects As New List(Of SponsorCoverLetterEn)
        eobj.Code = String.Empty
        eobj.Title = String.Empty

        Try
            ListObjects = bobj.GetList(eobj)
        Catch ex As Exception
            LogError.Log("SponsorCoverLetter", "GetList", ex.Message)
        End Try
        Session("ListObj_LetterDetails") = ListObjects
        ddlCode.Items.Clear()
        ddlCode.Items.Add(New System.Web.UI.WebControls.ListItem("--Select--", "-1"))
        ddlCode.DataTextField = "Code"
        ddlCode.DataValueField = "Code"
        ddlCode.DataSource = ListObjects
        ddlCode.DataBind()
    End Sub

    Private Sub DisplayEmptyResult()
        txtTitle.Text = String.Empty
        txtOurRef.Text = String.Empty
        txtYourRef.Text = String.Empty
        txtFrom.Text = String.Empty
        txtAddress.Text = String.Empty
        txtMsg.Text = String.Empty
        txtSignBy.Text = String.Empty
        txtName.Text = String.Empty
        txtToDate.Text = String.Empty
    End Sub

    Private Sub DisplayResult(ByVal result As SponsorCoverLetterEn)
        txtTitle.Text = result.Title
        txtOurRef.Text = result.OurRef
        txtYourRef.Text = result.YourRef
        txtFrom.Text = Format(result.FromDate, "dd/MM/yyyy")
        txtAddress.Text = result.Address
        txtMsg.Text = result.Message
        txtSignBy.Text = result.SignBy
        txtName.Text = result.Name
        txtToDate.Text = Format(result.ToDate, "dd/MM/yyyy")
    End Sub

    Protected Sub ddlCode_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlCode.SelectedIndexChanged
        ListObjects = Session("ListObj_LetterDetails")
        Dim result As SponsorCoverLetterEn = ListObjects.Find(AddressOf FindID)

        If ddlCode.SelectedIndex > 0 And result IsNot Nothing Then
            DisplayResult(result)
        Else
            DisplayEmptyResult()
        End If
    End Sub

    Private Function FindID(ByVal bk As SponsorCoverLetterEn) As Boolean
        If bk.Code = ddlCode.SelectedValue Then
            Return True
        Else
            Return False
        End If
    End Function

    Protected Sub btnPrintSCL_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnPrintSCL.Click
        If Session("batchcode") Is Nothing Then
            lblMsg.Text = "No Batchcode selected"
            Exit Sub
        End If
        laodvalues()
        ' ClosePopupWin()
        Dim cScript As String = ""
        cScript = "<SCRIPT LANGUAGE=""JavaScript"">PreviewReport()</SCRIPT>"
        ClientScript.RegisterStartupScript(Me.GetType(), "PreviewReport", cScript)
    End Sub
    Protected Sub ibtnClose_Click(ByVal sender As Object, ByVal e As EventArgs) Handles ibtnClose.Click
        ClosePopupWin()
    End Sub
    Private Sub ClosePopupWin()
        Dim cScript As String = ""
        cScript = "<SCRIPT LANGUAGE=""JavaScript"">window.opener.document.getElementById('ctl00_ContentPlaceHolder1_btnHidden').click();self.close();</SCRIPT>"
        ClientScript.RegisterStartupScript(Me.GetType(), "ClosePopUp", cScript)
    End Sub

    Public Sub laodvalues()

        Dim eobj As New SponsorCoverLetterEn
        eobj.Code = ddlCode.SelectedValue.ToString()
        eobj.Title = txtTitle.Text
        eobj.OurRef = txtOurRef.Text
        eobj.YourRef = txtYourRef.Text
        eobj.Address = txtAddress.Text
        eobj.Message = txtMsg.Text
        eobj.SignBy = txtSignBy.Text
        eobj.Name = txtName.Text
        eobj.FromDate = txtFrom.Text
        eobj.ToDate = txtToDate.Text

        Session("SCLCode") = eobj
    End Sub

    Protected Sub rblSCLType_SelectedIndexChanged(sender As Object, e As EventArgs) Handles rblSCLType.SelectedIndexChanged
        Session("SCLType") = rblSCLType.SelectedValue.ToString()
    End Sub
    Protected Sub rblSCLLang_SelectedIndexChanged(sender As Object, e As EventArgs) Handles rblSCLLang.SelectedIndexChanged
        Session("SCLLang") = rblSCLLang.SelectedValue.ToString()
    End Sub

    Private Sub SponsorCoverLetterEmail()
        Dim str As String = Nothing

        str = "select si.batchcode, stu.sasi_matricno, stu.sasi_name, stu.sasi_cursemyr, stu.sasi_icno, prog.sapg_programbm, ft.saft_code, ft.saft_desc, ft.saft_feetype," +
        " ft.saft_taxmode, sid.transamount, sid.taxamount, sid.tax, ft.SAFT_Hostel, ft.SAFT_Priority, ft.SAFT_Remarks, ft.SAFT_Status, sid.Transid," +
        " sid.RefCode, sid.TransTempCode, sid.TransCode, sid.internal_use, si.creditref1, stu.sasc_code" +
            " from sas_sponsorInvoice si" +
            " inner join sas_sponsorInvoiceDetails sid on si.transcode = sid.transcode and si.transtempcode = sid.transtempcode" +
            " inner join sas_feetypes ft on ft.saft_code = sid.refcode" +
            " inner join sas_student stu on stu.sasi_matricno = si.creditref" +
            " left join sas_program prog on prog.sapg_code = stu.sasi_pgid"
        If Not Session("batchcode") Is Nothing Then
            str += " where si.batchcode = '" + Session("batchcode").ToString() + "'"
        End If

        'DataSet Strating
        Dim _DataSet As DataSet = _ReportHelper.GetDataSet(str)
        _DataSet.Tables(0).TableName = "StudentEn"
        'Report XML Loading
        Dim bobj As New SponsorCoverLetterBAL
        Dim eobj As New SponsorCoverLetterEn
        Dim ListObjects As New List(Of SponsorCoverLetterEn)
        eobj.Code = String.Empty
        eobj.Title = String.Empty
        If Not Session("SCLCode") Is Nothing Then
            eobj = Session("SCLCode")
            ListObjects.Add(eobj)
        Else
            ListObjects = bobj.GetList(eobj)
        End If
        Dim objSpon As New SponsorEn
        If Not Session("SCLSponsor") Is Nothing Then
            objSpon = Session("SCLSponsor")
        End If
        Dim _SponCoverLetterDataSet As DataSet = GetDataSetSponsorCoverLetter(ListObjects)
        Dim _Sponsor As DataSet = GetDataSetSponsor(objSpon)
        _DataSet.Tables.Add(_SponCoverLetterDataSet.Tables(0).Copy())
        _DataSet.Tables.Add(_Sponsor.Tables(0).Copy())
        Dim s As String = Server.MapPath("~/xml/SponsorCoverLetter.xml")
        _DataSet.WriteXml(s)
        'Report XML Ended

        'Records Checking
        If _DataSet.Tables(0).Rows.Count = 0 Then
            Response.Write("No Record Found")
        Else
            ''Default will take BM version -  0 = BM, 1 = EN
            Dim sclReportBatch As String = "~/GroupReport/RptSponsorCoverLetterBatch.rpt"
            If Not Session("SCLLang") Is Nothing Then
                If Session("SCLLang").ToString() = "1" Then
                    sclReportBatch = "~/GroupReport/RptSponsorCoverLetterBatch_EN.rpt"
                Else
                    sclReportBatch = "~/GroupReport/RptSponsorCoverLetterBatch.rpt"
                End If
            End If
            ''Default will take BM version
            Dim sclReportIndividual As String = "~/GroupReport/RptSponsorCoverLetter.rpt"
            If Not Session("SCLLang") Is Nothing Then
                If Session("SCLLang").ToString() = "1" Then
                    sclReportIndividual = "~/GroupReport/RptSponsorCoverLetter_EN.rpt"
                Else
                    sclReportIndividual = "~/GroupReport/RptSponsorCoverLetter.rpt"
                End If
            End If
            'Report Loading 
            ''Default will take Batch Type - 0 = Batch, 1 =Individual
            If Not Session("SCLType") Is Nothing Then
                If Session("SCLType").ToString() = "1" Then
                    MyReportDocument.Load(Server.MapPath(sclReportIndividual))
                Else
                    MyReportDocument.Load(Server.MapPath(sclReportBatch))
                End If
            Else
                MyReportDocument.Load(Server.MapPath(sclReportBatch))
            End If
            MyReportDocument.SetDataSource(_DataSet)
            Session("reportobject") = MyReportDocument
            MyReportDocument.Refresh()
        End If
    End Sub

    Protected Sub btnEmailSCL_Click(sender As Object, e As EventArgs) Handles btnEmailSCL.Click
        Dim sponsorDetail As New SponsorEn
        Dim FileName As String = ""
        If Not Session("SCLSponsor") Is Nothing Then
            sponsorDetail = Session("SCLSponsor")
        End If
        If Session("batchcode") Is Nothing Then
            lblMsg.Text = "No Batchcode selected"
            Exit Sub
        End If
        If String.IsNullOrEmpty(sponsorDetail.Email) Then
            lblMsg.Text = "There is no email for the selected sponsor"
        Else
            lblMsg.Text = ""
            laodvalues()
            Try
                SponsorCoverLetterEmail()
                'Delete all the files 
                Try
                    Dim s As String
                    For Each s In System.IO.Directory.GetFiles(_Helper.GetSponsorCoverLetterFolder)
                        System.IO.File.Delete(s)
                    Next s
                Catch ex As Exception
                    LogError.Log("LookupSponsorCoverLetter", "btnEmailSCL_Click_DeleteFile", ex.Message)
                End Try
                Dim CrExportOptions As ExportOptions
                Dim CrDiskFileDestinationOptions As New  _
                DiskFileDestinationOptions()
                Dim CrFormatTypeOptions As New PdfRtfWordFormatOptions()
                FileName = _Helper.GetSponsorCoverLetterFolder + sponsorDetail.SponserCode + "_" + Date.Now.ToString("ddMMyyyy") + "_SponsorCoverLetter.pdf"
                CrDiskFileDestinationOptions.DiskFileName = FileName
                CrExportOptions = MyReportDocument.ExportOptions
                With CrExportOptions
                    .ExportDestinationType = ExportDestinationType.DiskFile
                    .ExportFormatType = ExportFormatType.PortableDocFormat
                    .DestinationOptions = CrDiskFileDestinationOptions
                    .FormatOptions = CrFormatTypeOptions
                End With
                MyReportDocument.Export()
            Catch ex As Exception
                LogError.Log("LookupSponsorCoverLetter", "btnEmailSCL_Click", ex.Message)
            End Try
            'Send Mail Messages
            Dim isMailSent As Boolean = SendMessage(FileName, sponsorDetail.Email)
            If isMailSent Then
                lblMsg.Text = "The Mail has been sent Successfully"
            Else
                lblMsg.Text = "The Mail fail to send"
            End If
            Dim objReader As New System.IO.StreamReader(FileName)
            objReader.Close()

        End If

    End Sub
    Private Function GetDataSetSponsorCoverLetter(ByVal list As  _
         List(Of SponsorCoverLetterEn)) As DataSet
        Dim _result As New DataSet()
        _result.Tables.Add("SponsorCoverLetter")
        _result.Tables("SponsorCoverLetter").Columns.Add("sascl_code")
        _result.Tables("SponsorCoverLetter").Columns.Add("sascl_title")
        _result.Tables("SponsorCoverLetter").Columns.Add("sascl_ourref")
        _result.Tables("SponsorCoverLetter").Columns.Add("sascl_yourref")
        _result.Tables("SponsorCoverLetter").Columns.Add("sascl_address")
        _result.Tables("SponsorCoverLetter").Columns.Add("sascl_message")
        _result.Tables("SponsorCoverLetter").Columns.Add("sascl_signby")
        _result.Tables("SponsorCoverLetter").Columns.Add("sascl_name")
        _result.Tables("SponsorCoverLetter").Columns.Add("sascl_frdate")
        _result.Tables("SponsorCoverLetter").Columns.Add("sascl_todate")
        _result.Tables("SponsorCoverLetter").Columns.Add("sascl_updatedby")
        _result.Tables("SponsorCoverLetter").Columns.Add("sascl_updatedtime")

        For Each item As SponsorCoverLetterEn In list
            Dim newRow As DataRow = _
                _result.Tables("SponsorCoverLetter").NewRow()
            newRow("sascl_code") = item.Code
            newRow("sascl_address") = item.Address
            newRow("sascl_frdate") = item.FromDate
            newRow("sascl_todate") = item.ToDate
            newRow("sascl_title") = item.Title
            newRow("sascl_ourref") = item.OurRef
            newRow("sascl_yourref") = item.YourRef
            newRow("sascl_message") = item.Message
            newRow("sascl_signby") = item.SignBy
            newRow("sascl_name") = item.Name
            newRow("sascl_updatedby") = item.UpdatedBy
            newRow("sascl_updatedtime") = item.UpdatedTime
            _result.Tables("SponsorCoverLetter").Rows.Add(newRow)
        Next
        Return _result
    End Function

    Public Function SendMessage(ByVal fileName As String, ByVal toAddress As String) As Boolean
        Dim senderMail As String = ""
        Dim subject As String = ""
        Dim messageBody As String = ""

        Dim eobj As New SponsorCoverLetterEn
        If Not Session("SCLCode") Is Nothing Then
            eobj = Session("SCLCode")
        Else
            eobj = New SponsorCoverLetterEn
        End If
        subject = eobj.Title
        messageBody = eobj.Message + "<br/><br/><br/><br/><br/><br/>" + eobj.SignBy + "<br/>" + eobj.Name

        If Not (ConfigurationManager.AppSettings("MailGroup")) Is Nothing Then
            senderMail = ConfigurationManager.AppSettings("MailGroup").ToString()
        End If

        Dim MailSettings As SmtpSection = DirectCast(ConfigurationManager.GetSection("system.net/mailSettings/smtp"), SmtpSection)

        Dim theMailMessage As New MailMessage()
        Dim strUserName As String = MailSettings.Network.UserName
        Dim strPassword As String = MailSettings.Network.Password

        Try
            With theMailMessage
                'specify your file location            
                Dim attachFile As Attachment = New Attachment(fileName)

                .From = New MailAddress(MailSettings.From)
                .To.Add(New MailAddress(toAddress.ToString()))
                .Subject = subject
                .Body = messageBody
                .IsBodyHtml = True
                .Attachments.Add(attachFile)
            End With

            'E-Mail Credentials and Sending
            'Dim theClient As SmtpClient = New SmtpClient("smtp.gmail.com", 587)
            Dim theClient As SmtpClient = New SmtpClient()
            theClient.Host = MailSettings.Network.Host
            theClient.Port = MailSettings.Network.Port
            theClient.Timeout = 30000
            theClient.UseDefaultCredentials = False
            theClient.EnableSsl = True

            Dim theCredential As System.Net.NetworkCredential = New System.Net.NetworkCredential(strUserName, strPassword)
            theClient.Credentials = theCredential

            'Send the email
            theClient.Send(theMailMessage)
            theMailMessage.Dispose()
            Return True

        Catch smtpex As SmtpException
            MaxModule.Helper.LogError(smtpex.Message)
            Return False
        Catch ex As System.Exception
            MaxModule.Helper.LogError(ex.Message)
            Return False
        End Try

    End Function

    Private Function GetDataSetSponsor(ByVal obj As SponsorEn) As DataSet
        Dim _result As New DataSet()
        _result.Tables.Add("Sponsor")
        _result.Tables("Sponsor").Columns.Add("sasr_code")
        _result.Tables("Sponsor").Columns.Add("sasr_name")
        _result.Tables("Sponsor").Columns.Add("sassr_sname")
        _result.Tables("Sponsor").Columns.Add("sasr_address")
        _result.Tables("Sponsor").Columns.Add("sasr_address1")
        _result.Tables("Sponsor").Columns.Add("sasr_address2")
        _result.Tables("Sponsor").Columns.Add("sasr_contact")
        _result.Tables("Sponsor").Columns.Add("sasr_phone")

        Dim newRow As DataRow = _result.Tables("Sponsor").NewRow()
        newRow("sasr_code") = obj.SponserCode
        newRow("sasr_name") = obj.SponsorName
        newRow("sassr_sname") = obj.SName
        newRow("sasr_address") = obj.Address
        newRow("sasr_address1") = obj.Address1
        newRow("sasr_address2") = obj.Address2
        newRow("sasr_contact") = obj.Contact
        newRow("sasr_phone") = obj.Phone
        _result.Tables("Sponsor").Rows.Add(newRow)
        Return _result
    End Function

End Class
