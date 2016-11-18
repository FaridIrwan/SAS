Imports HTS.SAS.Entities
Imports HTS.SAS.BusinessObjects
Imports System.Data
Imports System.Collections.Generic
Imports System.IO
Imports iTextSharp.text
Imports iTextSharp.text.pdf
Imports System.Net.Mail
Imports System.Net
Imports iTextSharp.text.html
Imports System.Configuration
Imports System.Web.Configuration
Imports System.Net.Configuration
Imports HTS.SAS.DataAccessObjects
Imports System.Linq

Partial Class SponsorCoverLetter
    Inherits System.Web.UI.Page

#Region "Declaration"
    Dim column As String() = {"MatricNo", "StudentName", "ICNo", "ProgramID", "CurretSemesterYear", "ProgramName"}
    '{"MatricNo", "StudentName", "SASI_Email", "ProgramID", "CurrentSemester", "OutStandingAmount", "SASI_Add1", "SASI_Add2", "SASI_City", "SASI_State", "SASI_Postcode"}
    Dim CFlag As String
    Dim DFlag As String
    Dim ListObjects As List(Of SponsorCoverLetterEn)
    Private ErrorDescription As String
    Private _MiddleHelper As New MaxMiddleware.Helper
    Private _Helper As New Helper
    Private _SponsorCoverLetterDAL As New SponsorCoverLetterDAL
    Private _SponsorDAL As New SponsorDAL


#End Region

#Region "Properties"

#End Region

#Region "Page and Control Events"

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Not IsPostBack() Then
            LoadSponsor()
            LoadSemester()
            FixEmptyRow(dgStudent, column)
            DunningLetterAttributes()
            LoadListObjects()
            ddlCode.SelectedIndex = 0
            DisplayEmptyResult()
        End If
        If (dgStudent.Items(0).Cells.Count > 1) Then
            If String.IsNullOrEmpty(dgStudent.Items(0).Cells(1).Text) Then
                FixEmptyRow(dgStudent, column)
            End If
        End If

    End Sub


    Protected Sub chkSelectAll_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)

        Dim dgItem1 As DataGridItem
        Dim chkselect As CheckBox
        Dim chkselectAll As CheckBox = CType(sender, CheckBox)

        For Each dgItem1 In dgStudent.Items
            chkselect = dgItem1.Cells(0).Controls(1)
            If chkselectAll.Checked = False Then
                chkselect.Checked = False
            Else
                chkselect.Checked = True
            End If
        Next
    End Sub

    Protected Sub btnSend_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSend.Click


        If (ddlCode.SelectedIndex = 0) Then
            lblMesg.Text = "Please select the sponsor cover letter code."
            lblMesg.Visible = True
            ddlCode.Focus()
            Return
        End If

        ' Validates when the grid is not loaded
        Dim isChecked As Boolean
        If (dgStudent.Items(0).Cells.Count = 1) Then
            lblMesg.Text = "Please load the student Information."
            lblMesg.Visible = True
            Return
        End If
        'Validates when the student is not selected
        Dim chkBox As CheckBox
        For Each dgi As DataGridItem In dgStudent.Items
            chkBox = dgi.Cells(0).Controls(1)
            If chkBox.Checked = True Then
                isChecked = True
            End If
        Next

        If isChecked = False Then
            lblMesg.Text = "Please select the student(s)."
            lblMesg.Visible = True
            Return
        End If

        'Calling DunningletterWaring
        OnDunningLetterWaring()
    End Sub

    Private Sub CreatePdf(ByVal fileName As String, ByVal mainHtml As String, ByVal pdfHtml As String, ByVal dueAmount As String)

        'Dim doc As Document = New Document
        '' Dim fileName As String = "\" + matricNo + "_DunningLetter.pdf"
        'PdfWriter.GetInstance(doc, New FileStream(Server.MapPath("DunningLetter") + fileName, FileMode.Create))
        'doc.Open()

        '' we add some conten
        'Dim phrase1 As New Phrase(Environment.NewLine)
        'Dim myPhrase As New Phrase("Dear Sir/Madam:", FontFactory.GetFont(FontFactory.TIMES_ROMAN, 10, Font.BOLD))
        'myPhrase.Add(phrase1)
        'myPhrase.Add(phrase1)
        'Dim firstLine As String
        'firstLine = "Re: Second Reminder - Overdue Invoice Amount: RM " + dueAmount
        'myPhrase.Add(New Phrase(firstLine, FontFactory.GetFont(FontFactory.TIMES_ROMAN, 10)))
        'myPhrase.Add(phrase1)
        'myPhrase.Add(phrase1)
        'Dim secondLine As String = "Recently your attention was called upon regarding the above referenced account via our letter.The amount of RM " + dueAmount + "  is now considerably past due and what concerns us that to date we have not heard back from you."
        'myPhrase.Add(New Phrase(secondLine, FontFactory.GetFont(FontFactory.TIMES_ROMAN, 10)))
        'myPhrase.Add(phrase1)
        'doc.Add(myPhrase)
        'myPhrase = New Phrase("We must receive payment immediately to keep your credit in good standing with us.", FontFactory.GetFont(FontFactory.TIMES_ROMAN, 10))
        'myPhrase.Add(phrase1)

        'doc.Add(myPhrase)
        'doc.Close()

        Try
            Dim mydoc As New Document(PageSize.A4, 20, 20, 20, 20)
            Dim cb As iTextSharp.text.pdf.PdfContentByte = Nothing
            Dim writer As iTextSharp.text.pdf.PdfWriter = Nothing

            writer = PdfWriter.GetInstance(mydoc, New FileStream(Server.MapPath("SponsorCoverLetter") + fileName, FileMode.Create))

            mydoc.Open()
            cb = writer.DirectContent

            cb.AddTemplate((PdfFooter(cb)), 0, 0)

            cb.MoveTo(20, 537)
            cb.LineTo(mydoc.PageSize.Width - 20, 537)
            cb.Stroke()


            Dim myhtmlworker As New simpleparser.HTMLWorker(mydoc)
            myhtmlworker.Parse(New StringReader(pdfHtml))
            mydoc.Close()

        Catch ex As Exception
            MaxModule.Helper.LogError(ex.Message)
        End Try

    End Sub


    Private Function PdfFooter(ByVal cb As PdfContentByte) As PdfTemplate
        ' Create the template and assign height
        Dim tmpFooter As PdfTemplate = cb.CreateTemplate(580, 400)
        ' Move to the bottom left corner of the template
        tmpFooter.MoveTo(1, 1)
        ' Place the footer content
        tmpFooter.Stroke()
        ' Begin writing the footer
        tmpFooter.BeginText()
        ' Set the font and size
        Dim f_cn As BaseFont = BaseFont.CreateFont("c:\\windows\\fonts\\arial.ttf", BaseFont.CP1252, BaseFont.NOT_EMBEDDED)
        tmpFooter.SetFontAndSize(f_cn, 11)
        ' Write out details from the payee table
        ' tmpFooter.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "Regards:", 20, 550, 0)

        'If rbNew.Checked Then
        '    tmpFooter.ShowTextAligned(PdfContentByte.ALIGN_LEFT, txtSignBy.Text.Trim(), 20, 315, 0)
        '    tmpFooter.ShowTextAligned(PdfContentByte.ALIGN_LEFT, txtName.Text.Trim(), 20, 300, 0)
        'Else
        tmpFooter.ShowTextAligned(PdfContentByte.ALIGN_LEFT, txtSignBy.Text.Trim(), 20, 315, 0)
        tmpFooter.ShowTextAligned(PdfContentByte.ALIGN_LEFT, txtName.Text.Trim(), 20, 300, 0)
        'End If

        tmpFooter.EndText()
        ' Stamp a line above the page footer
        cb.SetLineWidth(1)
        cb.MoveTo(20, 350)
        cb.LineTo(580, 350)
        cb.Stroke()
        ' Return the footer template
        Return tmpFooter
    End Function

    Public Function BuildPDFHtml(ByVal studentName As String, ByVal address1 As String, ByVal address2 As String, ByVal city As String, ByVal state As String, ByVal pincode As String, ByVal dueAmount As String) As String
        Dim pdfhtml As String = String.Empty
        Try
            'If rbNew.Checked Then
            '    pdfhtml = "<br/><br/><br/>" +
            '                 "<table width='100%' cellpadding='0' cellspacing=0><tr><td align='left'><b>" + studentName + "</b></td><td align='right'>" + txtFrom.Text.Trim() + "</td></tr></table> " +
            '                 "<b>" + address1 + "</b><br />" +
            '                 "<b>" + address2 + "</b><br />" +
            '                 "<b>" + city + "</b><br />" +
            '                 "<b>" + state + "</b><br />" +
            '                 "<b>" + pincode + "</b><br />" +
            '                 "<br />" +
            '                 "<br />" +
            '                 "<b>Ref : " + txtRef.Text.Trim() + "</b><br />" +
            '                 "<br />" +
            '                 "<b>Dear " + studentName + "</b><br />" +
            '                 "<b>Reg : " + txtTitle.Text.Trim() + "</b>" +
            '                 "<hr />" +
            '                 "<br /><br />" +
            '                 "<div style='height:700px;width:100%'>" + txtMsg.Text.Trim().Replace("$", dueAmount) + "</div>"
            'Else
            pdfhtml = "<br/><br/><br/>" +
                "<table width='100%' cellpadding='0' cellspacing=0><tr><td align='left'><b>" + studentName + "</b></td><td align='right'>" + txtToDate.Text.Trim() + "</td></tr></table> " +
                "<b>" + address1 + "</b><br />" +
                "<b>" + address2 + "</b><br />" +
                "<b>" + city + "</b><br />" +
                 "<b>" + state + "</b><br />" +
                 "<b>" + pincode + "</b><br />" +
                "<br />" +
                "<br />" +
                "<b>Ref : " + txtYourRef.Text.Trim() + "</b><br />" +
                "<br />" +
                "<b>Dear " + studentName + "</b><br />" +
                "<b>Reg : " + txtTitle.Text.Trim() + "</b>" +
                "<hr />" +
                "<br /><br />" +
                "<div style='height:700px;width:100%'>" + txtMsg.Text.Trim().Replace("$", dueAmount) + "</div>"



            'End If
        Catch ex As Exception
            Throw
        End Try
        Return pdfhtml
    End Function
    Public Function BuildHtml(ByVal studentName As String, ByVal address1 As String, ByVal address2 As String, ByVal city As String, ByVal state As String, ByVal pincode As String, ByVal dueAmount As String) As String
        Dim html As String = String.Empty
        Dim pdfhtml As String = String.Empty
        Try
            'If rbNew.Checked Then
            '    html = "<br/><br/><br/>" +
            '         "<table width='100%' cellpadding='0' cellspacing=0><tr><td align='left'><b>" + studentName + "</b></td><td align='right'>" + txtFrom.Text.Trim() + "</td></tr></table> " +
            '         "<b>" + address1 + "</b><br />" +
            '         "<b>" + address2 + "</b><br />" +
            '         "<b>" + city + "</b><br />" +
            '         "<b>" + state + "</b><br />" +
            '         "<b>" + pincode + "</b><br />" +
            '         "<br />" +
            '         "<br />" +
            '         "<br />" +
            '         "<b>Ref : " + txtRef.Text.Trim() + "</b><br />" +
            '         "<br />" +
            '         "<b>Dear " + studentName + "</b><br />" +
            '         "<br />" +
            '         "<b>Reg : " + txtTitle.Text.Trim() + "</b>" +
            '         "<hr />" +
            '         "<br />" +
            '         "<div style='width:100%'>" + txtMsg.Text.Trim().Replace("$", dueAmount) + "</div> <br /> <hr />" +
            '         "<b>Regards:</b><br />" +
            '         "<b>" + txtSignBy.Text.Trim() + "</b><br />" +
            '         "<b>" + txtName.Text.Trim() + "</b>" +
            '         "<br />" +
            '         "<br />"
            'Else
            html = "<html><body><br/><br/><br/>" +
                 "<table width='100%' cellpadding='0' cellspacing=0><tr><td align='left'><b>" + studentName + "</b></td><td align='right'>" + txtToDate.Text.Trim() + "</td></tr></table> " +
                 "<b>" + address1 + "</b><br />" +
                 "<b>" + address2 + "</b><br />" +
                 "<b>" + city + "</b><br />" +
                 "<b>" + state + "</b><br />" +
                 "<b>" + pincode + "</b><br />" +
                 "<br />" +
                 "<br />" +
                 "<br />" +
                 "<b>Ref : " + txtOurRef.Text.Trim() + "</b><br />" +
                 "<br />" +
                 "<b>Dear " + studentName + "</b><br />" +
                 "<br />" +
                 "<b>Reg : " + txtTitle.Text.Trim() + "</b>" +
                 "<hr />" +
                 "<br />" +
                 "<div style='width:100%'>" + txtMsg.Text.Trim().Replace("$", dueAmount) + "</div><br /><hr />" +
                 "<b>Regards:</b><br />" +
                 "<b>" + txtSignBy.Text.Trim() + "</b><br />" +
                 "<b>" + txtName.Text.Trim() + "</b>" +
                 "<br />" +
                 "</body></html>"
            'End If

        Catch ex As Exception
            Throw
        End Try
        Return html
    End Function

    Public Function SendMessage(ByVal fileName As String, ByVal subject As String, ByVal messageBody As String, ByVal fromAddress As String, ByVal toAddress As String) As Integer

        'Dim configurationFile As Configuration = WebConfigurationManager.OpenWebConfiguration("PathToConfigFile")
        'Dim MailSettings As MailSettingsSectionGroup = configurationFile.GetSectionGroup("system.net/mailSettings")

        Dim MailSettings As SmtpSection = DirectCast(ConfigurationManager.GetSection("system.net/mailSettings/smtp"), SmtpSection)

        Dim theMailMessage As New MailMessage()
        'Dim strUserName As String = "syafiqfathyamer@gmail.com"
        Dim strUserName As String = MailSettings.Network.UserName
        Dim strPassword As String = MailSettings.Network.Password

        Try
            With theMailMessage
                '.From = New MailAddress("syafiqfathyamer@gmail.com")
                '.To.Add("syafiqfathyamer@hotmail.com")
                '.To.Add("s.fathyamer@censof.com")

                'specify your file location 
                Dim attachFile As Attachment = New Attachment(Server.MapPath("SponsorCoverLetter") + fileName)

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
            Return True

        Catch smtpex As SmtpException
            MaxModule.Helper.LogError(smtpex.Message)
            Return False
        Catch ex As System.Exception
            MaxModule.Helper.LogError(ex.Message)
            Return False
        End Try



        'Try
        '    Dim message As MailMessage = New MailMessage()
        '    message.IsBodyHtml = True
        '    Dim client As SmtpClient = New SmtpClient()
        '    client.Timeout = 30000
        '    client.EnableSsl = False

        '    message.From = New MailAddress(fromAddress.ToString())
        '    message.To.Add(New MailAddress(toAddress.ToString()))

        '    Dim attachFile As Attachment = New Attachment(Server.MapPath("DunningLetter") + fileName) 'specify your file location 

        '    message.Attachments.Add(attachFile)
        '    message.Subject = subject
        '    message.Body = messageBody
        '    message.IsBodyHtml = True

        '    Dim theCredential As System.Net.NetworkCredential = New System.Net.NetworkCredential("syafiqfathyamer@gmail.com", "savage123")
        '    client.Credentials = theCredential

        '    client.Send(message)

        'Catch ex As Exception
        '    Throw ex
        'End Try
        'Return 1
    End Function

    Protected Sub btnFind_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnFind.Click
        Dim objup As New SponsorCoverLetterDAL
        Dim lstobjects As New List(Of SponsorCoverLetterEn)
        Dim eob As New SponsorCoverLetterEn
        Dim stuEn As New StudentEn
        Dim sponEn As New SponsorEn

        If ddl_Sponsor.SelectedValue.ToString().Equals("-1") Then
            lblMesg.Text = "Please select sponsor"
            lblMesg.Visible = True
            ddl_Sponsor.Focus()
            Return
        End If

        If ddlProgram.SelectedValue.ToString().Equals("-1") Then
            stuEn.ProgramID = String.Empty
        Else
            stuEn.ProgramID = ddlProgram.SelectedValue.ToString()
        End If


        If ddlSemester.SelectedValue.ToString().Equals("-1") Then
            stuEn.CurretSemesterYear = String.Empty
        Else
            stuEn.CurretSemesterYear = ddlSemester.SelectedValue.ToString()
        End If

        If ddl_Sponsor.SelectedValue.ToString().Equals("-1") Then
            sponEn.SponserCode = String.Empty
        Else
            sponEn.SponserCode = ddl_Sponsor.SelectedValue.ToString()
        End If

        eob.Studentacc = stuEn
        eob.SponsorDetails = sponEn

        Try
            lstobjects = objup.GetSponsorStudentDetails(eob)
        Catch ex As Exception
            LogError.Log("Sponsor Cover Letter", "btnFind_Click", ex.Message)
        End Try

        If Not lstobjects Is Nothing And lstobjects.Count > 0 Then
            dgStudent.DataSource = lstobjects
            dgStudent.DataBind()
        Else
            FixEmptyRow(dgStudent, column)
        End If


    End Sub


    Protected Sub ddlProgram_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlProgram.SelectedIndexChanged
        If ddlProgram.SelectedIndex > 0 Then
            ddlSemester.Enabled = True
        Else
            ddlSemester.SelectedIndex = 0
            ddlSemester.Enabled = False
        End If
    End Sub

    Protected Sub ibtnNew_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ibtnNew.Click
        OnAdd()
        FixEmptyRow(dgStudent, column)
        LoadListObjects()
        ddlCode.SelectedIndex = 0
        DisplayEmptyResult()
    End Sub

    Protected Sub ibtnCancel_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ibtnCancel.Click
        LoadUserRights()
        OnAdd()
        FixEmptyRow(dgStudent, column)
        OnAdd()
        FixEmptyRow(dgStudent, column)
        LoadListObjects()
        ddlCode.SelectedIndex = 0
        DisplayEmptyResult()
    End Sub


#End Region

#Region "User Defined Mothods"

    ''' <summary>
    ''' Method to Fill the Semester DropDown
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub LoadSemester()
        Dim eSemester As New SemesterSetupEn
        Dim bSemester As New SemesterSetupBAL
        Dim listSem As New List(Of SemesterSetupEn)

        ddlSemester.Items.Add(New System.Web.UI.WebControls.ListItem("---Select---", "-1"))
        ddlSemester.DataTextField = "SemisterSetupCode"
        ddlSemester.DataValueField = "SemisterSetupCode"
        eSemester.SemisterSetupCode = "%"

        Try
            listSem = bSemester.GetListSemesterCode(eSemester)
        Catch ex As Exception
            LogError.Log("FeePosting", "addsemester", ex.Message)
        End Try
        ddlSemester.DataSource = listSem
        ddlSemester.DataBind()
        'Session("faculty") = listfac
    End Sub

    ''' <summary>
    ''' Method to Load Programs Dropdown
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub LoadProgram()
        Dim objBAL As New SponsorCoverLetterBAL
        Dim listObj As New List(Of ProgramInfoEn)

        Try
            listObj = objBAL.GetProgramBySponsor(ddl_Sponsor.SelectedValue)
        Catch ex As Exception
            LogError.Log("SponsorCoverLetter", "LoadProgram", ex.Message)
        End Try
        ddlProgram.Items.Clear()
        ddlProgram.Items.Add(New System.Web.UI.WebControls.ListItem("--Select--", "-1"))
        ddlProgram.DataTextField = "Program"
        ddlProgram.DataValueField = "ProgramCode"
        ddlProgram.DataSource = listObj
        ddlProgram.DataBind()

    End Sub

    ''' <summary>
    ''' This method is to bind empty row with gridview
    ''' </summary>
    ''' <param name="gridView">gridview to bind</param>
    ''' <param name="Columns">collection of columns</param>
    Private Sub FixEmptyRow(ByVal gridView As DataGrid, ByVal Columns As String())
        Dim dt As New DataTable()
        For i As Integer = 0 To Columns.Length - 1
            Dim dcNew As New DataColumn(Columns(i))
            dcNew.AllowDBNull = True
            dt.Columns.Add(dcNew)
        Next
        dt.Rows.Add(dt.NewRow())
        gridView.DataSource = dt
        gridView.DataBind()

        Dim columnCount As Integer = gridView.Items(0).Cells.Count
        gridView.Items(0).Cells.Clear()
        gridView.Items(0).Cells.Add(New TableCell())
        gridView.Items(0).Cells(0).ColumnSpan = columnCount
        gridView.Items(0).Cells(0).Text = "There is no record"
        gridView.ItemStyle.HorizontalAlign = HorizontalAlign.Left

    End Sub

    Private Sub DunningLetterAttributes()
        Menuname(CInt(Request.QueryString("Menuid")))
        LoadUserRights()
        ibtnTodate.Attributes.Add("onClick", "return getDateto()")
        ibtnFDate.Attributes.Add("onClick", "return getibtnFDate()")
        txtFrom.Attributes.Add("OnKeyup", "return CheckFromDate()")
        txtToDate.Attributes.Add("OnKeyup", "return CheckToDate()")
        ibtnSave.Attributes.Add("onclick", "return validate()")
        ' btnSend.Attributes.Add("onclick", "return validate()")
        ibtnDelete.Attributes.Add("onclick", "return getconfirm()")
        txtRecNo.Attributes.Add("OnKeyup", "return geterr()")
        txtFrom.Attributes.Add("OnKeyup", "return CheckFromDate()")
        txtToDate.Attributes.Add("OnKeyup", "return CheckToDate()")
        dates()
    End Sub

    Private Sub dates()
        txtFrom.Text = Format(Date.Now, "dd/MM/yyyy")
        txtToDate.Text = Format(Date.Now, "dd/MM/yyyy")
    End Sub

    ''' <summary>
    ''' Method to Load the UserRights
    ''' </summary>
    ''' <remarks></remarks>
    ''' 
    Private Sub LoadUserRights()
        Dim obj As New UsersBAL
        Dim eobj As UserRightsEn

        eobj = obj.GetUserRights(CInt(Request.QueryString("Menuid")), CInt(Session("UserGroup")))
        'Rights for Add

        If eobj.IsAdd = True Then
            OnAdd()
            ibtnNew.ImageUrl = "images/add.png"
            ibtnNew.Enabled = True
        Else
            ibtnNew.ImageUrl = "images/gadd.png"
            ibtnNew.Enabled = False
            ibtnNew.ToolTip = "Access Denied"

        End If
        'Rights for Edit

        ibtnSave.Enabled = False
        ibtnSave.ImageUrl = "images/gsave.png"
        ibtnSave.ToolTip = "Access Denied"

        'Rights for View
        ibtnView.Enabled = eobj.IsView
        ibtnView.ImageUrl = "images/gfind.png"
        ibtnView.ToolTip = "Access Denied"
        'Rights for Delete
        ibtnDelete.ImageUrl = "images/gdelete.png"
        ibtnDelete.ToolTip = "Access Denied"
        ibtnDelete.Enabled = False

        'Rights for Print
        ibtnPrint.Enabled = eobj.IsPrint

        ibtnPrint.Enabled = False
        ibtnPrint.ImageUrl = "images/gprint.png"
        ibtnPrint.ToolTip = "Access Denied"

        'ibtnOthers.Enabled = False
        'ibtnOthers.ImageUrl = "images/gothers.png"
        'ibtnOthers.ToolTip = "Access Denied"

        ibtnPosting.Enabled = False
        ibtnPosting.ImageUrl = "images/gposting.png"
        ibtnPosting.ToolTip = "Access Denied"

    End Sub

    ''' <summary>
    ''' Method to get Menu Name
    ''' </summary>
    ''' <param name="MenuId"></param>
    ''' <remarks></remarks>
    Private Sub Menuname(ByVal MenuId As Integer)
        Dim eobj As New MenuEn
        Dim bobj As New MenuBAL
        eobj.MenuId = MenuId
        eobj = bobj.GetMenus(eobj)
        lblMenuName.Text = eobj.MenuName
    End Sub

    ''' <summary>
    ''' Method to Validate
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub SpaceValidation()
        ibtnSave.Attributes.Add("onClick", "return validate()")
        Dim GBFormat As System.Globalization.CultureInfo
        GBFormat = New System.Globalization.CultureInfo("en-GB")

        'Batch date
        If Trim(txtFrom.Text).Length < 10 Then
            lblMesg.Text = "Enter Valid From Date"
            lblMesg.Visible = True
            txtFrom.Focus()
            Exit Sub
        Else
            Try
                txtFrom.Text = DateTime.Parse(txtFrom.Text, GBFormat)
            Catch ex As Exception
                lblMesg.Text = "Enter Valid Fron Date"
                lblMesg.Visible = True
                txtFrom.Focus()
                Exit Sub
            End Try
        End If
        'Invoice date
        If Trim(txtToDate.Text).Length < 10 Then
            lblMesg.Text = "Enter Valid To Date"
            lblMesg.Visible = True
            txtToDate.Focus()
            Exit Sub
        Else
            Try
                txtToDate.Text = DateTime.Parse(txtToDate.Text, GBFormat)
            Catch ex As Exception
                lblMesg.Text = "Enter Valid To Date"
                lblMesg.Visible = True
                txtToDate.Focus()
                Exit Sub
            End Try
        End If
    End Sub

    ''' <summary>
    ''' Method to Save and Update Bank Profile
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub OnSave()
        lblMesg.Visible = False
        Dim bsobj As New SponsorCoverLetterBAL
        Dim eobj As New SponsorCoverLetterEn
        Dim RecAff As Integer
        eobj.Code = ddlCode.SelectedValue
        eobj.Title = Trim(txtTitle.Text)
        eobj.YourRef = Trim(txtYourRef.Text)
        eobj.OurRef = Trim(txtOurRef.Text)
        eobj.ToDate = Trim(txtToDate.Text)
        eobj.FromDate = Trim(txtFrom.Text)
        eobj.Message = Trim(txtMsg.Text)
        eobj.Name = Trim(txtName.Text)
        eobj.SignBy = Trim(txtSignBy.Text)
        eobj.FromDate = Trim(txtFrom.Text)
        eobj.ToDate = Trim(txtToDate.Text)
        eobj.UpdatedBy = Session("User")
        eobj.UpdatedTime = Date.Now.ToString()
        lblMesg.Visible = True
        If Session("PageMode") = "Add" Then
            Try
                RecAff = bsobj.Insert(eobj)
                'User Defined Message
                ErrorDescription = "Record Saved Successfully "
                Session("PageMode") = "Edit"
                lblMesg.Text = ErrorDescription

            Catch ex As Exception
                lblMesg.Text = ex.Message.ToString()
            End Try
        ElseIf Session("PageMode") = "Edit" Then
            Try
                RecAff = bsobj.Update(eobj)
                ListObjects = Session("ListObj_BankDetails")
                ListObjects(CInt(txtRecNo.Text) - 1) = eobj
                Session("ListObj_BankDetails") = ListObjects
                'User Defined Message
                ErrorDescription = "Record Updated Successfully "
                lblMesg.Text = ErrorDescription
            Catch ex As Exception
                lblMesg.Text = ex.Message.ToString()
            End Try
        End If

    End Sub
    ''' <summary>
    ''' Method to Save and Student Dunning Letterwaring
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub OnDunningLetterWaring()
        'Variable Declarations - Start
        Dim mainHtml As String = String.Empty
        Dim pdfHtml As String = String.Empty

        Dim headerBodyHtml As String = String.Empty
        Dim studentName As String = String.Empty
        Dim studentICNo As String = String.Empty
        Dim programName As String = String.Empty
        Dim studentCurrentSemesterYr As String = String.Empty
        Dim address1 As String = String.Empty
        Dim address2 As String = String.Empty
        Dim city As String = String.Empty
        Dim dueAmount As String = String.Empty
        Dim state As String = String.Empty
        Dim pincode As String = String.Empty
        Dim chkselect As CheckBox
        Dim toMail As String
        Dim SMatricNo As Integer = 0
        Dim senderMail As String = String.Empty
        Dim MailTo As Integer = 0, MailFrom As Integer = 0
        Dim FileId As Integer = 0, MailSubject As String = Nothing
        Dim FileName As String = Nothing
        Dim MailBody As String = Nothing
        'Variable Declarations - Stop

        lblMesg.Visible = False
        Dim bsobj As New SponsorCoverLetterBAL
        Dim eobj As New SponsorCoverLetterEn
        Dim RecAff As Integer = 0
        eobj.Code = Trim(ddlCode.SelectedValue)
        eobj.Title = Trim(txtTitle.Text)
        eobj.YourRef = Trim(txtYourRef.Text)
        eobj.OurRef = Trim(txtOurRef.Text)
        eobj.FromDate = Trim(txtFrom.Text)
        eobj.Message = Trim(txtMsg.Text)
        eobj.Address = Trim(txtAddress.Text)
        eobj.Name = Trim(txtName.Text)
        eobj.SignBy = Trim(txtSignBy.Text)
        eobj.ToDate = Trim(txtToDate.Text)
        eobj.UpdatedBy = Session("User")
        eobj.UpdatedTime = Date.Now.ToString()


        If Not (ConfigurationManager.AppSettings("MailGroup")) Is Nothing Then
            senderMail = ConfigurationManager.AppSettings("MailGroup").ToString()
        End If

        'Delete all the files 
        Try
            Dim s As String
            For Each s In System.IO.Directory.GetFiles(Server.MapPath("SponsorCoverLetter"))
                System.IO.File.Delete(s)
            Next s
        Catch

        End Try
        Dim sponsorDetail As New SponsorEn
        Dim SponsorList As New List(Of SponsorEn)
        SponsorList = Session(ReceiptsClass.SessionSponsorList)
        If Not SponsorList Is Nothing Then
            If SponsorList.Any(Function(x) x.SponserCode = ddl_Sponsor.SelectedValue.ToString()) Then
                sponsorDetail = SponsorList.Where(Function(x) x.SponserCode = ddl_Sponsor.SelectedValue.ToString()).FirstOrDefault()
            End If
        Else
            sponsorDetail = New SponsorEn
        End If
        Try
            Dim BatchMailBody As String = ""
            toMail = sponsorDetail.Email
            For Each dgItem1 In dgStudent.Items
                chkselect = dgItem1.Cells(0).Controls(1)
                If chkselect.Checked = True Then
                    SMatricNo = dgItem1.Cells(1).Text
                    'FileName = "/" + ddl_Sponsor.SelectedValue.ToString() + "_" + SMatricNo.ToString() + "_SponsorCoverLetter.pdf"
                    studentName = dgItem1.Cells(2).Text.ToString()
                    studentICNo = dgItem1.Cells(3).Text.ToString()
                    programName = dgItem1.Cells(6).Text.ToString()
                    studentCurrentSemesterYr = dgItem1.Cells(5).Text.ToString()
                    Dim objup As New SponsorCoverLetterDAL
                    Dim lstobjects As New List(Of SponsorCoverLetterEn)
                    Dim newStudent As New StudentEn
                    newStudent.MatricNo = SMatricNo
                    newStudent.ProgramID = dgItem1.Cells(4).Text.ToString()
                    eobj.Studentacc = newStudent
                    eobj.SponsorDetails = sponsorDetail
                    lstobjects = objup.GetSponsorStudentDetails(eobj)

                    Dim FeeTable As String = String.Empty
                    Dim TotalAmount As Double = 0
                    If lstobjects.Count > 0 Then
                        FeeTable = "<table><thead style=""background-color:lightgrey; ""><tr><th>KETERANGAN</th><th>JUMLAH (RM)</th></tr></thead>"
                        For Each i In lstobjects
                            FeeTable += " <tr><td>" + i.FeeTypeCode + "</td><td>" + String.Format(i.FeeAmount, "{0:F}") + "</td></tr>"
                            TotalAmount += i.FeeAmount
                        Next i
                        FeeTable += "<tfoot style=""background-color:lightgrey""><tr><td>JUMLAH KESELURUHAN</td><td>" + String.Format(TotalAmount, "{0:F}") + "</td></tr></tfoot></table>"
                    End If


                    'Build Mail Subject - Start
                    MailSubject = "TUNTUTAN YURAN PENGAJIAN PELAJAR IJAZAH LANJUTAN DI UNIVERSITI PUTRA MALAYSIA"
                    'Build Mail Subject - Stop

                    'Build Mail Body - Start
                    MailBody = _MiddleHelper.GetFileContents(_Helper.GetSponsorCoverLetterPath)
                    MailBody = String.Format(MailBody,
                                             eobj.OurRef, eobj.YourRef,
                                             Date.Now.ToShortDateString(), eobj.Address.ToString(), studentName, SMatricNo,
                                             studentICNo, programName, studentCurrentSemesterYr, FeeTable)
                    BatchMailBody += MailBody
                    'Build Mail Body - Stop
                End If
            Next
            If BatchMailBody <> "" Then
                FileName = "/" + ddl_Sponsor.SelectedValue.ToString() + "_" + Date.Now.ToString("ddMMyyyy") + "_SponsorCoverLetter.pdf"
                CreatePdf(FileName, BatchMailBody, BatchMailBody, String.Format("{0:F}", dueAmount).ToString())
            End If
            If toMail <> String.Empty Then
                'Send Mail Messages
                SendMessage(FileName, Trim(MailSubject), BatchMailBody, senderMail, toMail)
            End If

            lblMesg.Visible = True
            lblMesg.Text = "The Mail has been sent Successfully"

        Catch smtpex As SmtpException
            MaxModule.Helper.LogError(smtpex.Message)
            lblMesg.Text = smtpex.Message.ToString
        Catch ex As Exception
            MaxModule.Helper.LogError(ex.Message)
            lblMesg.Text = ex.Message.ToString
        End Try

        'lblMesg.Visible = True
        'If Session("PageMode") = "Add" Then
        '    Try
        '        RecAff = bsobj.Insert(eobj)
        '        'User Defined Message
        '        ErrorDescription = "Record Saved Successfully "
        '        Session("PageMode") = "Edit"
        '        lblMesg.Text = ErrorDescription

        '    Catch ex As Exception
        '        lblMesg.Text = ex.Message.ToString()
        '    End Try
        'ElseIf Session("PageMode") = "Edit" Then
        '    Try
        '        RecAff = bsobj.Update(eobj)
        '        ListObjects = Session("ListObj_BankDetails")
        '        ListObjects(CInt(txtRecNo.Text) - 1) = eobj
        '        Session("ListObj_BankDetails") = ListObjects
        '        'User Defined Message
        '        ErrorDescription = "Record Updated Successfully "
        '        lblMesg.Text = ErrorDescription
        '    Catch ex As Exception
        '        lblMesg.Text = ex.Message.ToString()
        '    End Try
        'End If

    End Sub
    ''' <summary>
    ''' Method To Change the Date Format(dd/MM/yyyy)
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub setDateFormat()
        Dim myInvoiceDate As Date = CDate(CStr(txtFrom.Text))
        Dim myFormat As String = "dd/MM/yyyy"
        Dim myFormattedDate As String = Format(myInvoiceDate, myFormat)
        txtFrom.Text = myFormattedDate
        Dim myDuedate As Date = CDate(CStr(txtToDate.Text))
        Dim myFormattedDate1 As String = Format(myDuedate, myFormat)
        txtToDate.Text = myFormattedDate1

    End Sub

    ''' <summary>
    ''' Method to Clear the Fields in NewMode
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub OnAdd()
        Session("PageMode") = "Add"
        ibtnSave.Enabled = True
        ibtnSave.ImageUrl = "images/save.png"
        txtFrom.Text = Format(Date.Now, "dd/MM/yyyy")
        txtToDate.Text = Format(Date.Now, "dd/MM/yyyy")
        'Clear Text Box values
        OnClearData()
    End Sub

    ''' <summary>
    ''' Method to Clear the Field Values
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub OnClearData()
        ddlProgram.SelectedIndex = 0
        ddlProgram.DataBind()
        ddlSemester.SelectedIndex = 0
        DisableRecordNavigator()
        lblMesg.Text = ""
        ddlCode.Enabled = True
        'Clear Text Box values
        ddlCode.SelectedIndex = 0
        txtTitle.Text = Nothing
        txtOurRef.Text = Nothing
        txtYourRef.Text = Nothing
        txtAddress.Text = Nothing
        txtMsg.Text = Nothing
        txtSignBy.Text = Nothing
        txtName.Text = Nothing

        Session("PageMode") = "Add"
    End Sub

    ''' <summary>
    ''' Method to Enable or Disable Navigation Buttons
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub DisableRecordNavigator()
        Dim flag As Boolean
        flag = False
        ibtnFirst.Enabled = flag
        ibtnLast.Enabled = flag
        ibtnPrevs.Enabled = flag
        ibtnNext.Enabled = flag
        If flag = False Then
            ibtnFirst.ImageUrl = "images/gnew_first.png"
            ibtnLast.ImageUrl = "images/gnew_last.png"
            ibtnPrevs.ImageUrl = "images/gnew_Prev.png"
            ibtnNext.ImageUrl = "images/gnew_next.png"
        Else
            ibtnFirst.ImageUrl = "images/new_last.png"
            ibtnLast.ImageUrl = "images/new_first.png"
            ibtnPrevs.ImageUrl = "images/new_Prev.png"
            ibtnNext.ImageUrl = "images/new_next.png"
        End If
    End Sub

    ''' <summary>
    ''' Method to get list of Bank Profiles
    ''' </summary>
    ''' <remarks></remarks>
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

        If ListObjects.Count <> 0 Then
            DisableRecordNavigator()
            'txtRecNo.Text = "1"
            '' OnMoveFirst()
            'If Session("EditFlag") = True Then
            '    Session("PageMode") = "Edit"
            '    txtCode.Enabled = False
            '    ibtnSave.Enabled = True
            '    ibtnSave.ImageUrl = "images/save.png"
            '    lblMesg.Visible = True
            'Else
            '    Session("PageMode") = ""
            '    ibtnSave.Enabled = False
            '    ibtnSave.ImageUrl = "images/gsave.png"
            '    ErrorDescription = "Record did not Exist"
            '    lblMesg.Text = ErrorDescription
            '    lblMesg.Visible = True
            'End If
        Else
            txtRecNo.Text = ""
            lblCount.Text = ""
            'Clear Text Box values
            OnClearData()
            'If DFlag = "Delete" Then
            'Else

            '    ErrorDescription = "Record did not Exist"
            '    lblMesg.Text = ErrorDescription
            '    lblMesg.Visible = True
            '    DFlag = ""
            'End If
        End If
    End Sub

#End Region

    Protected Sub ibtnSave_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ibtnSave.Click
        SpaceValidation()
        OnSave()
        setDateFormat()
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

    Private Function FindID(ByVal bk As SponsorCoverLetterEn) As Boolean
        If bk.Code = ddlCode.SelectedValue Then
            Return True
        Else
            Return False
        End If
    End Function

    ''' <summary>
    ''' Method to Load Sponsor Dropdown
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub LoadSponsor()
        Dim objBAL As New SponsorCoverLetterBAL
        Dim listObj As New List(Of SponsorEn)
        Dim loen As New SponsorEn
        loen.SponserCode = ""
        loen.Name = ""


        Try
            listObj = objBAL.GetSponsorWithStudent(loen)
        Catch ex As Exception
            LogError.Log("SponsorCoverLetter", "LoadSponsor", ex.Message)
        End Try

        ddl_Sponsor.Items.Clear()
        ddl_Sponsor.Items.Add(New System.Web.UI.WebControls.ListItem("--Select--", "-1"))

        ddl_Sponsor.DataTextField = "Name"
        ddl_Sponsor.DataValueField = "SponserCode"
        ddl_Sponsor.DataSource = listObj
        ddl_Sponsor.DataBind()
        Session(ReceiptsClass.SessionSponsorList) = listObj
    End Sub

    Protected Sub ddl_Sponsor_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddl_Sponsor.SelectedIndexChanged
        If ddl_Sponsor.SelectedIndex > 0 Then
            ddlProgram.Enabled = True
            LoadProgram()
        Else
            ddlProgram.SelectedIndex = 0
            ddlProgram.Enabled = False
        End If
    End Sub

End Class
