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
Imports MaxGeneric


Partial Class StudentDunningLetter
    Inherits System.Web.UI.Page

#Region "Declaration"
    Dim column As String() = {"MatricNo", "StudentName", "SASI_Email", "ProgramID", "CurrentSemester", "OutStandingAmount", "SASI_Add1", "SASI_Add2", "SASI_Add3", "SASI_City", "SASI_State", "SASI_Postcode"}
    Dim CFlag As String
    Dim DFlag As String
    Dim ListObjects As List(Of DunningLettersEn)
    Private ErrorDescription As String
    Private _MiddleHelper As New MaxMiddleware.Helper
    Private _Helper As New Helper
    Private _DunningLettersDAL As New HTS.SAS.DataAccessObjects.DunningLettersDAL

#End Region

#Region "Properties"

#End Region

#Region "Page and Control Events"

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Not IsPostBack() Then
            LoadProgram()
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

    Protected Sub rbNew_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        'pnlNew.Visible = True
        pnlExisting.Visible = False
    End Sub

    Protected Sub rbExisting_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        pnlNew.Visible = False
        pnlExisting.Visible = True
        LoadListObjects()
        ddlCode.SelectedIndex = 0
        DisplayEmptyResult()

    End Sub

    Protected Sub btnSend_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSend.Click

        'If (ddlCode.SelectedIndex = 0) Then
        '    lblMesg.Text = "Please select the dunning letter code."
        '    lblMesg.Visible = True
        '    ddlCode.Focus()
        '    Return
        'End If

        'Clear message text
        lblMesg.Visible = False
        lblMesg.Text = ""
        lblMesgUnSent.Visible = False
        lblMesgUnSent.Text = ""

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

        If ddl_bankac.SelectedValue = "-1" Then
            lblMesg.Visible = True
            lblMesg.Text = "Please select Nombor Akaun."
            Return
        End If

        'Calling DunningletterWaring
        OnDunningLetterWaring()

    End Sub

    Private Sub CreatePdf(ByVal fileName As String, ByVal mainHtml As String, ByVal pdfHtml As String, ByVal dueAmount As String)

        'Try

        '    Dim doc As Document = New Document
        '    ' Dim fileName As String = "\" + matricNo + "_DunningLetter.pdf"
        '    PdfWriter.GetInstance(doc, New FileStream(Server.MapPath("DunningLetter") + fileName, FileMode.Create))
        '    doc.Open()

        '    ' we add some conten
        '    Dim phrase1 As New Phrase(Environment.NewLine)
        '    'Dim myPhrase As New Phrase("Dear Sir/Madam:", FontFactory.GetFont(FontFactory.TIMES_ROMAN, 10, Font.BOLD))
        '    Dim myPhrase As New Phrase("Tuan, ", FontFactory.GetFont(FontFactory.TIMES_ROMAN, 10, Font.BOLD))
        '    myPhrase.Add(phrase1)
        '    myPhrase.Add(phrase1)
        '    Dim firstLine As String
        '    'firstLine = "Re: Second Reminder - Overdue Invoice Amount: RM " + dueAmount
        '    firstLine = "PERINGATAN KEDUA - TUNTUTAN HUTANG YURAN PENGAJIAN: RM " + dueAmount
        '    myPhrase.Add(New Phrase(firstLine, FontFactory.GetFont(FontFactory.TIMES_ROMAN, 10)))
        '    myPhrase.Add(phrase1)
        '    myPhrase.Add(phrase1)
        '    Dim secondLine As String = "Recently your attention was called upon regarding the above referenced account via our letter.The amount of RM " + dueAmount + "  is now considerably past due and what concerns us that to date we have not heard back from you."
        '    myPhrase.Add(New Phrase(secondLine, FontFactory.GetFont(FontFactory.TIMES_ROMAN, 10)))
        '    myPhrase.Add(phrase1)
        '    doc.Add(myPhrase)
        '    myPhrase = New Phrase("We must receive payment immediately to keep your credit in good standing with us.", FontFactory.GetFont(FontFactory.TIMES_ROMAN, 10))
        '    myPhrase.Add(phrase1)

        '    doc.Add(myPhrase)
        '    doc.Close()

        'Catch ex As Exception
        '    MaxModule.Helper.LogError(ex.Message)
        'End Try

        Try
            Dim mydoc As New Document(PageSize.A4, 20, 20, 20, 20)
            Dim cb As iTextSharp.text.pdf.PdfContentByte = Nothing
            Dim writer As iTextSharp.text.pdf.PdfWriter = Nothing

            writer = PdfWriter.GetInstance(mydoc, New FileStream(Server.MapPath("DunningLetter") + fileName, FileMode.Create))

            mydoc.Open()
            cb = writer.DirectContent

            cb.AddTemplate((PdfFooter(cb)), 0, 0)

            cb.MoveTo(20, 537)
            'cb.LineTo(mydoc.PageSize.Width - 20, 537)
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
        'Dim f_cn As BaseFont = BaseFont.CreateFont("c:\\windows\\fonts\\arial.ttf", BaseFont.CP1252, BaseFont.NOT_EMBEDDED)
        'tmpFooter.SetFontAndSize(f_cn, 11)
        Dim f_cn As BaseFont = BaseFont.CreateFont("c:\\windows\\fonts\\calibri.ttf", BaseFont.CP1252, BaseFont.NOT_EMBEDDED)
        tmpFooter.SetFontAndSize(f_cn, 10)

        ' Write out details from the payee table
        ' tmpFooter.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "Regards:", 20, 550, 0)

        'If rbNew.Checked Then
        '    tmpFooter.ShowTextAligned(PdfContentByte.ALIGN_LEFT, txtSignBy.Text.Trim(), 20, 315, 0)
        '    tmpFooter.ShowTextAligned(PdfContentByte.ALIGN_LEFT, txtName.Text.Trim(), 20, 300, 0)
        'Else
        tmpFooter.ShowTextAligned(PdfContentByte.ALIGN_LEFT, txt2Signby.Text.Trim(), 20, 315, 0)
        tmpFooter.ShowTextAligned(PdfContentByte.ALIGN_LEFT, txt2Name.Text.Trim(), 20, 300, 0)
        'End If

        tmpFooter.EndText()
        ' Stamp a line above the page footer
        cb.SetLineWidth(1)
        cb.MoveTo(20, 350)
        'cb.LineTo(580, 350)
        cb.Stroke()
        ' Return the footer template
        Return tmpFooter
    End Function

    'Public Function BuildPDFHtml(ByVal MatricNo As String, ByVal LetterDate As String, ByVal Subject As String, ByVal BankAccount As String, ByVal studentName As String, ByVal address1 As String, ByVal address2 As String, ByVal city As String, ByVal state As String, ByVal pincode As String, ByVal dueAmount As String) As String
    Public Function BuildPDFHtml(ByVal MatricNo As String, ByVal LetterDate As String, ByVal Subject As String, ByVal BankAccount As String, ByVal studentName As String, ByVal address1 As String, ByVal address2 As String, ByVal city As String, ByVal pincode As String, ByVal state As String, ByVal dueAmount As String) As String

        Dim pdfhtml As String = String.Empty
        Try
            'pdfhtml = "<br/><br/><br/>" +
            '    "<table width='100%' cellpadding='0' cellspacing=0><tr><td align='left'><b>" + studentName + "</b></td><td align='right'>" + txt2Date.Text.Trim() + "</td></tr></table> " +
            '    "<b>" + address1 + "</b><br />" +
            '    "<b>" + address2 + "</b><br />" +
            '    "<b>" + city + "</b><br />" +
            '     "<b>" + state + "</b><br />" +
            '     "<b>" + pincode + "</b><br />" +
            '    "<br />" +
            '    "<br />" +
            '    "<b>Ref : " + txt2Ref.Text.Trim() + "</b><br />" +
            '    "<br />" +
            '    "<b>Dear " + studentName + "</b><br />" +
            '    "<b>Reg : " + txt2Tittle.Text.Trim() + "</b>" +
            '    "<hr />" +
            '    "<br /><br />" +
            '    "<div style='height:700px;width:100%'>" + txt2Msg.Text.Trim().Replace("$", dueAmount) + "</div>"

            pdfhtml = "<table><tr><td>"
            pdfhtml += "<p style='font-family:Calibri;font-size:10px'>"
            pdfhtml += "Rujukan Kami : UPM/BEN/SKP/01/Q5 ( " + MatricNo + " )<br />"
            pdfhtml += "Rujukan Tuan : <br />"
            pdfhtml += "Tarikh &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;: " + LetterDate + "</p><br />"

            pdfhtml += "<p style='font-family:Calibri;font-size:10px'>"
            pdfhtml += studentName + "<br />"
            If Trim(address1).Length > 0 Then
                pdfhtml += address1 + "<br />"
            End If
            If Trim(address2).Length > 0 Then
                pdfhtml += address2 + "<br />"
            End If
            If Trim(pincode).Length > 0 Then
                pdfhtml += pincode + "&nbsp;"
            End If
            If Trim(city).Length > 0 Then
                pdfhtml += city + "<br />"
            End If
            If Trim(state).Length > 0 Then
                pdfhtml += state + "</p><br />"
            End If

            pdfhtml += "<p style='font-family:Calibri;font-size:10px'>Tuan,</p><br />"
            pdfhtml += "<p style='font-family:Calibri;font-size:10px'><strong>" + Subject + "</strong></p><br />"
            pdfhtml += "<p style='font-family:Calibri;font-size:10px'>Dengan segala hormatnya perkara di atas adalah dirujuk. </p> "
            pdfhtml += "<p style='font-family:Calibri;font-size:10px;margin-left:0px'>"
            pdfhtml += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Untuk makluman tuan, pejabat ini sedang dalam proses pengemaskinian lejar pelajar dan<br />"
            pdfhtml += "daripada semakan didapati pihak tuan masih belum menjelaskan hutang yang tertunggak berjumlah<br />"
            pdfhtml += "<strong>RM " + dueAmount + "</strong></p><br />"
            pdfhtml += "<p style='font-family:Calibri;font-size:10px;margin-left:0px'>"
            pdfhtml += "2.&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Sehubungan dengan itu, kerjasama pihak tuan amatlah dihargai supaya menjelaskan hutang<br />"
            pdfhtml += "berkenaan dengan membuat bayaran melalui CIMBClicks melalui <strong><i>modul Pay Bill – UNIVERSITI PUTRA</i></strong><br />"
            pdfhtml += "<strong><i>MALAYSIA KAMPUS SERDANG</i></strong> atau menggunakan Slip Bayaran di mana-mana cawangan CIMB Bank<br />"
            pdfhtml += "Berhad atas nama <strong>UPM COLLECTION</strong>, nombor akaun <strong>" + BankAccount + ".</strong> Sila tuliskan Nama, No K/P, No Matrik<br /> "
            pdfhtml += "dan No Telefon Bimbit di bahagian belakang slip tersebut dan serahkan satu salinan slip ke kaunter <br />"
            pdfhtml += "Seksyen Kewangan Pelajar II atau fax ke no 03-89472048.</p><br />"
            pdfhtml += "<p style='font-family:Calibri;font-size:10px;margin-left:0px'>"
            pdfhtml += "3.&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Bayaran melalui kad kredit Visa atau Mastercard atau Bankcard (Kad ATM) boleh dibuat di<br />"
            pdfhtml += "Kaunter Seksyen Kewangan Pelajar II pada waktu berurusan kaunter. Sekiranya tuan memerlukan<br />"
            pdfhtml += "penjelasan lanjut berkenaan perkara ini, sila hubungi pejabat ini di Seksyen Kewangan Pelajar II<br />"
            pdfhtml += "melalui nombor talian 03 - 8946 4161 / 03 - 8946 4156 atau emel kepada <strong>bursar.student_pg@upm.edu.my</strong></p><br />"
            pdfhtml += "<p style='font-family:Calibri;font-size:10px;margin-left:0px'>"
            pdfhtml += "4.&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Pihak tuan diminta menjelaskan hutang yuran tertunggak pengajian ini dalam tempoh <strong>14 hari</strong><br />"
            pdfhtml += "dari tarikh surat ini dan kegagalan tuan menjelaskan hutang ini membolehkan tindakan diambil ke atas<br />"
            pdfhtml += "tuan. Sila abaikan surat ini jika pihak tuan telah menjelaskan yuran berkenaan.</p><br />"
            pdfhtml += "<p style='font-family:Calibri;font-size:10px'>Sekian, terima kasih.<br /><br />"
            pdfhtml += "Saya yang menjalankan tugas,<br /><br /><br />"
            pdfhtml += "<strong>MAZITAH BINTI AHMAD</strong><br />"
            pdfhtml += "Pegawai Kewangan<br />"
            pdfhtml += "Pejabat Bursar<br />"
            pdfhtml += "Universiti Putra Malaysia<br /></p><br /><br />"
            pdfhtml += "<p style='font-family:Calibri;font-size:10px;text-align:center'>TIADA TANDATANGAN DIPERLUKAN KERANA INI ADALAH CETAKAN KOMPUTER</p>"
            pdfhtml += "</td></tr></table></body></html>"

        Catch ex As Exception
            Throw
        End Try
        Return pdfhtml
    End Function

    'Public Function BuildHtml(ByVal studentName As String, ByVal address1 As String, ByVal address2 As String, ByVal city As String, ByVal state As String, ByVal pincode As String, ByVal dueAmount As String) As String
    Public Function BuildHtml(ByVal MatricNo As String, ByVal LetterDate As String, ByVal Subject As String, ByVal BankAccount As String, ByVal studentName As String, ByVal address1 As String, ByVal address2 As String, ByVal city As String, ByVal pincode As String, ByVal state As String, ByVal dueAmount As String) As String
        Dim html As String = String.Empty
        'Dim pdfhtml As String = String.Empty
        Try
            'html = "<html><body><br/><br/><br/>" +
            '     "<table width='100%' cellpadding='0' cellspacing=0><tr><td align='left'><b>" + studentName + "</b></td><td align='right'>" + txt2Date.Text.Trim() + "</td></tr></table> " +
            '     "<b>" + address1 + "</b><br />" +
            '     "<b>" + address2 + "</b><br />" +
            '     "<b>" + city + "</b><br />" +
            '     "<b>" + state + "</b><br />" +
            '     "<b>" + pincode + "</b><br />" +
            '     "<br />" +
            '     "<br />" +
            '     "<br />" +
            '     "<b>Ref : " + txt2Ref.Text.Trim() + "</b><br />" +
            '     "<br />" +
            '     "<b>Dear " + studentName + "</b><br />" +
            '     "<br />" +
            '     "<b>Reg : " + txt2Tittle.Text.Trim() + "</b>" +
            '     "<hr />" +
            '     "<br />" +
            '     "<div style='width:100%'>" + txt2Msg.Text.Trim().Replace("$", dueAmount) + "</div><br /><hr />" +
            '     "<b>Regards:</b><br />" +
            '     "<b>" + txt2Signby.Text.Trim() + "</b><br />" +
            '     "<b>" + txt2Name.Text.Trim() + "</b>" +
            '     "<br />" +
            '     "</body></html>"

            html = "<table><tr><td>"
            html += "<p style='font-family:Calibri;font-size:13px'>"
            html += "Rujukan Kami : UPM/BEN/SKP/01/Q5 ( " + MatricNo + " )<br />"
            html += "Rujukan Tuan : <br />"
            html += "Tarikh &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;: " + LetterDate + "</p>"

            html += "<p style='font-family:Calibri;font-size:13px'>"
            html += studentName + "<br />"
            If Trim(address1).Length > 0 Then
                html += address1 + "<br />"
            End If
            If Trim(address2).Length > 0 Then
                html += address2 + "<br />"
            End If
            If Trim(pincode).Length > 0 Then
                html += pincode + "&nbsp;"
            End If
            If Trim(city).Length > 0 Then
                html += city + "<br />"
            End If
            If Trim(state).Length > 0 Then
                html += state + "</p>"
            End If

            html += "<p style='font-family:Calibri;font-size:13px'>Tuan,</p>"
            html += "<p style='font-family:Calibri;font-size:13px'><strong>" + Subject + "</strong></p>"
            html += "<p style='font-family:Calibri;font-size:13px'>Dengan segala hormatnya perkara di atas adalah dirujuk.</p>"
            html += "<p style='font-family:Calibri;font-size:13px;margin-left:0px'>"
            html += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Untuk makluman tuan, pejabat ini sedang dalam proses pengemaskinian lejar pelajar dan<br />"
            html += "daripada semakan didapati pihak tuan masih belum menjelaskan hutang yang tertunggak berjumlah<br />"
            html += "<strong>RM " + dueAmount + "</strong></p>"
            html += "<p style='font-family:Calibri;font-size:13px;margin-left:0px'>"
            html += "2.&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Sehubungan dengan itu, kerjasama pihak tuan amatlah dihargai supaya menjelaskan hutang<br />"
            html += "berkenaan dengan membuat bayaran melalui CIMBClicks melalui <strong><i>modul Pay Bill – UNIVERSITI PUTRA</i></strong><br />"
            html += "<strong><i>MALAYSIA KAMPUS SERDANG</i></strong> atau menggunakan Slip Bayaran di mana-mana cawangan CIMB Bank<br />"
            html += "Berhad atas nama <strong>UPM COLLECTION</strong>, nombor akaun <strong>" + BankAccount + ".</strong> Sila tuliskan Nama, No K/P, No Matrik<br /> "
            html += "dan No Telefon Bimbit di bahagian belakang slip tersebut dan serahkan satu salinan slip ke kaunter <br />"
            html += "Seksyen Kewangan Pelajar II atau fax ke no 03-89472048.</p>"
            html += "<p style='font-family:Calibri;font-size:13px;margin-left:0px'>"
            html += "3.&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Bayaran melalui kad kredit Visa atau Mastercard atau Bankcard (Kad ATM) boleh dibuat di<br />"
            html += "Kaunter Seksyen Kewangan Pelajar II pada waktu berurusan kaunter. Sekiranya tuan memerlukan<br />"
            html += "penjelasan lanjut berkenaan perkara ini, sila hubungi pejabat ini di Seksyen Kewangan Pelajar II<br />"
            html += "melalui nombor talian 03 - 8946 4161 / 03 - 8946 4156 atau emel kepada <strong>bursar.student_pg@upm.edu.my</strong></p>"
            html += "<p style='font-family:Calibri;font-size:13px;margin-left:0px'>"
            html += "4.&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Pihak tuan diminta menjelaskan hutang yuran tertunggak pengajian ini dalam tempoh <strong>14 hari</strong><br />"
            html += "dari tarikh surat ini dan kegagalan tuan menjelaskan hutang ini membolehkan tindakan diambil ke atas<br />"
            html += "tuan. Sila abaikan surat ini jika pihak tuan telah menjelaskan yuran berkenaan.</p>"
            html += "<p style='font-family:Calibri;font-size:13px'>Sekian, terima kasih.<br /><br />"
            html += "Saya yang menjalankan tugas,<br /><br /><br />"
            html += "<strong>MAZITAH BINTI AHMAD</strong><br />"
            html += "Pegawai Kewangan<br />"
            html += "Pejabat Bursar<br />"
            html += "Universiti Putra Malaysia<br /></p><br />"
            html += "<p style='font-family:Calibri;font-size:13px;text-align:center'>TIADA TANDATANGAN DIPERLUKAN KERANA INI ADALAH CETAKAN KOMPUTER</p>"
            html += "</td></tr></table></body></html>"

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
                Dim attachFile As Attachment = New Attachment(Server.MapPath("DunningLetter") + fileName)

                .From = New MailAddress(MailSettings.From)
                .To.Add(New MailAddress(toAddress.ToString()))
                .Subject = subject
                .IsBodyHtml = True
                .Body = messageBody
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
        Dim objup As New StudentBAL
        Dim lstobjects As New List(Of StudentEn)
        Dim eob As New StudentEn

        If ddlProgram.SelectedValue.ToString().Equals("-1") Then
            eob.ProgramID = ""
        Else
            eob.ProgramID = ddlProgram.SelectedValue.ToString()
        End If


        If ddlSemester.SelectedValue.ToString().Equals("-1") Then
            eob.CurretSemesterYear = ""

        Else
            eob.CurretSemesterYear = ddlSemester.SelectedValue.ToString()

        End If

        Try
            'modified by Hafiz @ 26/4/2016 - start
            'lstobjects = objup.GetListStudentOutstanding(eob)
            lstobjects = objup.GetListOutstandingAmtAllStud(eob)
            'end modfied - end

        Catch ex As Exception
            LogError.Log("Student Dunning Letter", "btnFind_Click", ex.Message)
        End Try

        If Not lstobjects Is Nothing And lstobjects.Count > 0 Then
            dgStudent.DataSource = lstobjects
            dgStudent.DataBind()
        Else
            FixEmptyRow(dgStudent, column)
        End If

        'Clear message text
        lblMesg.Text = ""
        lblMesgUnSent.Text = ""

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
        Dim objBAL As New ProgramInfoBAL
        Dim listObj As New List(Of ProgramInfoEn)
        Dim loen As New ProgramInfoEn
        loen.ProgramCode = ""
        loen.Program = ""
        loen.ProgramBM = ""
        loen.Status = True
        loen.ProgramType = ""
        loen.Faculty = ""
        Try
            listObj = objBAL.GetProgramInfoList(loen)
        Catch ex As Exception
            LogError.Log("StudentDunningLetter", "LoadProgram", ex.Message)
        End Try
        ddlProgram.Items.Clear()
        ddlProgram.Items.Add(New System.Web.UI.WebControls.ListItem("--Select--", "-1"))
        ddlProgram.DataTextField = "Program"
        ddlProgram.DataValueField = "ProgramCode"
        ddlProgram.DataSource = listObj
        ddlProgram.DataBind()


        ddl_bankac.Items.Clear()
        ddl_bankac.Items.Add(New System.Web.UI.WebControls.ListItem("--Select--", "-1"))
        ddl_bankac.DataSource = _DunningLettersDAL.BankDetails("")
        ddl_bankac.DataTextField = "sabd_code"
        ddl_bankac.DataValueField = "sabd_accode"
        ddl_bankac.DataBind()

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
        ibtnFDate.Attributes.Add("onClick", "return getibtnFDate()")
        ibtnTodate.Attributes.Add("onClick", "return getDateto()")
        ibtnDunningFDate.Attributes.Add("onClick", "return getDunningFDate()")
        ibtnDunningTDate.Attributes.Add("onClick", "return getDunningTDate()")
        txtFrom.Attributes.Add("OnKeyup", "return CheckFromDate()")
        txtTodate.Attributes.Add("OnKeyup", "return CheckToDate()")
        ibtnSave.Attributes.Add("onclick", "return validate()")
        ' btnSend.Attributes.Add("onclick", "return validate()")
        ibtnDelete.Attributes.Add("onclick", "return getconfirm()")
        txtRecNo.Attributes.Add("OnKeyup", "return geterr()")
        txtFrom.Attributes.Add("OnKeyup", "return CheckFromDate()")
        txtTodate.Attributes.Add("OnKeyup", "return CheckToDate()")
        dates()
    End Sub

    Private Sub dates()
        txtFrom.Text = Format(Date.Now, "dd/MM/yyyy")
        txtTodate.Text = Format(Date.Now, "dd/MM/yyyy")
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

        'ibtnPosting.Enabled = False
        'ibtnPosting.ImageUrl = "images/gposting.png"
        'ibtnPosting.ToolTip = "Access Denied"

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
        ibtnSave.Attributes.Add("onCLick", "return Validate()")
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
        If Trim(txtTodate.Text).Length < 10 Then
            lblMesg.Text = "Enter Valid To Date"
            lblMesg.Visible = True
            txtTodate.Focus()
            Exit Sub
        Else
            Try
                txtTodate.Text = DateTime.Parse(txtTodate.Text, GBFormat)
            Catch ex As Exception
                lblMesg.Text = "Enter Valid To Date"
                lblMesg.Visible = True
                txtTodate.Focus()
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
        Dim bsobj As New DunningLettersBAL
        Dim eobj As New DunningLettersEn
        Dim RecAff As Integer
        eobj.Code = Trim(txtCode.Text)
        eobj.Title = Trim(txtTitle.Text)
        eobj.Reference = Trim(txtRef.Text)
        eobj.ToDate = Trim(txtTodate.Text)
        eobj.FromDate = Trim(txtFrom.Text)
        eobj.Message = Trim(txtMsg.Text)
        eobj.Name = Trim(txtName.Text)
        eobj.SignBy = Trim(txtSignBy.Text)
        eobj.FromDate = Trim(txtFrom.Text)
        eobj.ToDate = Trim(txtTodate.Text)
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
        Dim AccNo As String = Nothing
        Dim address3 As String = String.Empty
        Dim strWarning As String = Nothing
        Dim intTotalEmail As Integer = 0
        Dim intSent As Integer = 0
        Dim intUnSent As Integer = 0
        Dim statecode As String = String.Empty
        'Variable Declarations - Stop

        lblMesg.Visible = False
        lblMesgSent.Visible = False
        lblMesgUnSent.Visible = False
        lblMesg.Text = ""
        lblMesgSent.Text = ""
        lblMesgUnSent.Text = ""
        'Dim StudentMatricNos As String = Nothing
        Dim stdSent As String = Nothing
        Dim stdUnSent As String = Nothing

        Dim bsobj As New DunningLettersBAL
        Dim eobj As New DunningLettersEn
        Dim RecAff As Integer = 0
        'eobj.Code = Trim(ddlCode.SelectedValue)
        'eobj.Title = Trim(txt2Tittle.Text)
        'eobj.Reference = Trim(txt2Ref.Text)
        'eobj.ToDate = Trim(txt2Date.Text)
        'eobj.FromDate = Trim(txtFrom.Text)
        'eobj.Message = Trim(txt2Msg.Text)
        'eobj.Name = Trim(txt2Name.Text)
        'eobj.SignBy = Trim(txt2Signby.Text)
        'eobj.FromDate = Trim(txtFrom.Text)
        'eobj.ToDate = Trim(txt2Date.Text)
        eobj.UpdatedBy = Session("User")
        eobj.UpdatedTime = Date.Now.ToString()

        eobj.Semester = ddlSemester.SelectedValue
        eobj.pgID = ddlProgram.SelectedValue
        eobj.InsertBy = Session("User")
        eobj.InsertDate = Now.Date.ToShortDateString()
        AccNo = Trim(ddl_bankac.SelectedValue)
        'eobj.Warning = "1"
        'eobj.Code = "W1"
        If Not (ConfigurationManager.AppSettings("MailGroup")) Is Nothing Then
            senderMail = ConfigurationManager.AppSettings("MailGroup").ToString()
        End If

        'Delete all the files 
        Try
            Dim s As String
            For Each s In System.IO.Directory.GetFiles(Server.MapPath("DunningLetter"))
                System.IO.File.Delete(s)
            Next s
        Catch

        End Try

        Try
            For Each dgItem1 In dgStudent.Items
                chkselect = dgItem1.Cells(0).Controls(1)
                If chkselect.Checked = True Then
                    toMail = dgItem1.Cells(3).Text
                    If String.IsNullOrEmpty(toMail) = False Then
                        SMatricNo = dgItem1.Cells(1).Text
                        'FileName = "\" + SMatricNo.ToString() + "_DunningLetter.pdf"
                        studentName = dgItem1.Cells(2).Text.ToString()
                        dueAmount = dgItem1.Cells(6).Text.ToString()
                        address1 = dgItem1.Cells(7).Text.ToString()
                        address2 = dgItem1.Cells(8).Text.ToString()
                        address3 = dgItem1.Cells(9).Text.ToString()
                        city = dgItem1.Cells(10).Text.ToString()
                        statecode = dgItem1.Cells(11).Text.ToString()
                        If String.IsNullOrEmpty(statecode) = False Then
                            state = _DunningLettersDAL.GetState(statecode)
                        End If
                        pincode = dgItem1.Cells(12).Text.ToString()

                        eobj.MatricNo = SMatricNo
                        eobj.Semester = dgItem1.Cells(5).Text

                        'If ddl_bankac.SelectedValue = "-1" Then
                        '    AccNo = ""
                        'Else
                        'AccNo = Trim(ddl_bankac.SelectedValue)

                        'End If

                        strWarning = _DunningLettersDAL.GetCountDunning(eobj.MatricNo, eobj.Semester)

                        If Not strWarning = "overlimit" Then

                            If strWarning = "1" Then
                                eobj.Warning = "1"
                                eobj.Code = "W1"
                                MailSubject = "PERINGATAN PERTAMA - "
                            End If

                            If strWarning = "2" Then
                                eobj.Warning = "2"
                                eobj.Code = "W2"
                                MailSubject = "PERINGATAN KEDUA - "
                            End If

                            If strWarning = "3" Then
                                eobj.Warning = "3"
                                eobj.Code = "W3"
                                MailSubject = "PERINGATAN KETIGA - "
                            End If

                            'Build Mail Subject - Start
                            MailSubject += "TUNTUTAN HUTANG YURAN PENGAJIAN"
                            'Build Mail Subject - Stop

                            'Filename - Start
                            FileName = "\" + SMatricNo.ToString() + "_DunningLetter_" + strWarning + ".pdf"
                            'Filename - End

                            'Build Mail Body - Start
                            'MailBody = _MiddleHelper.GetFileContents(_Helper.GetDunningLetterPath)
                            'MailBody = String.Format(MailBody, SMatricNo, Now.Date.ToShortDateString(), MailSubject, dueAmount, AccNo, studentName, address1, address2, address3, pincode, city, state)
                            'Build Mail Body - Stop

                            mainHtml = BuildHtml(SMatricNo, Now.Date.ToShortDateString(), MailSubject, AccNo, studentName, address1, address2, city, pincode, state, String.Format("{0:F}", dueAmount).ToString())
                            pdfHtml = BuildPDFHtml(SMatricNo, Now.Date.ToShortDateString(), MailSubject, AccNo, studentName, address1, address2, city, pincode, state, String.Format("{0:F}", dueAmount).ToString())

                            'CreatePdf(FileName, MailBody, MailBody, String.Format("{0:F}", dueAmount).ToString())
                            CreatePdf(FileName, mainHtml, pdfHtml, String.Format("{0:F}", dueAmount).ToString())

                            'Send Mail Messages
                            SendMessage(FileName, Trim(MailSubject), mainHtml, senderMail, toMail)

                            'Insert Dunning Letter Student Details
                            _DunningLettersDAL.InsertDunning(eobj)

                            If intSent = 0 Then
                                stdSent = SMatricNo
                            Else
                                stdSent &= clsGeneric.AddComma() & SMatricNo
                            End If
                            intSent += 1

                            strWarning = Nothing
                            eobj.Warning = Nothing
                            eobj.Code = Nothing
                            MailSubject = Nothing

                        Else
                            If intUnSent = 0 Then
                                stdUnSent = SMatricNo
                            Else
                                stdUnSent &= clsGeneric.AddComma() & SMatricNo
                            End If
                            intUnSent += 1

                        End If

                        intTotalEmail += 1
                    End If
                End If
            Next

            lblMesg.Visible = True
            lblMesg.Text = "Total email(s) " + Convert.ToString(intSent) + " out of " + Convert.ToString(intTotalEmail) + " sent successfully."

            'lblMesgSent.Visible = True
            'lblMesgSent.Text = "The Mail has been sent Successfully for Student Matric Number " + stdSent

            If intUnSent > 0 Then
                lblMesgUnSent.Visible = True
                lblMesgUnSent.Text = "Email was not sent as reach maximum reminders for Student Matric Number: " + stdUnSent
            End If

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
        Dim myDuedate As Date = CDate(CStr(txtTodate.Text))
        Dim myFormattedDate1 As String = Format(myDuedate, myFormat)
        txtTodate.Text = myFormattedDate1

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
        txtTodate.Text = Format(Date.Now, "dd/MM/yyyy")
        'Clear Text Box values
        OnClearData()
    End Sub

    ''' <summary>
    ''' Method to Clear the Field Values
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub OnClearData()
        ddlProgram.SelectedIndex = 0
        ddlSemester.SelectedIndex = 0
        DisableRecordNavigator()
        lblMesg.Text = ""
        txtCode.Enabled = True
        'Clear Text Box values
        txtCode.Text = ""
        txtTitle.Text = ""
        txtRef.Text = ""
        txtMsg.Text = ""
        txtSignBy.Text = ""
        txtName.Text = ""
        Session("PageMode") = "Add"
        lblMesgUnSent.Text = ""
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
        Dim ds As New DataSet
        Dim bobj As New DunningLettersBAL
        Dim eobj As New DunningLettersEn

        eobj.Code = String.Empty
        eobj.Title = String.Empty

        Session("ListObj_LetterDetails") = ListObjects
        ' lblCount.Text = ListObjects.Count.ToString()

        'Load the Dunning Letter created already
        Try
            ListObjects = bobj.GetList(eobj)
        Catch ex As Exception
            LogError.Log("StudentDunningLetter", "LoadDunningLetterCode", ex.Message)
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
        Dim result As DunningLettersEn = ListObjects.Find(AddressOf FindID)

        If ddlCode.SelectedIndex > 0 And result IsNot Nothing Then
            DisplayResult(result)
        Else
            DisplayEmptyResult()
        End If

    End Sub

    Private Sub DisplayResult(ByVal result As DunningLettersEn)
        txt2Tittle.Text = result.Title
        txt2Date.Text = Format(result.FromDate, "dd/MM/yyyy")
        txt2Ref.Text = result.Reference
        txt2Msg.Text = result.Message
        txt2Signby.Text = result.SignBy
        txt2SignDate.Text = Format(result.ToDate, "dd/MM/yyyy")
        txt2Name.Text = result.Name
    End Sub

    Private Sub DisplayEmptyResult()
        txt2Tittle.Text = String.Empty
        txt2Date.Text = String.Empty
        txt2Ref.Text = String.Empty
        txt2Msg.Text = String.Empty
        txt2Signby.Text = String.Empty
        txt2SignDate.Text = String.Empty
        txt2Name.Text = String.Empty
    End Sub

    Private Function FindID(ByVal bk As DunningLettersEn) As Boolean
        If bk.Code = ddlCode.SelectedValue Then
            Return True
        Else
            Return False
        End If
    End Function

End Class
