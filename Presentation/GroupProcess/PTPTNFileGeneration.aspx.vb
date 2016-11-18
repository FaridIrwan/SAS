#Region "NameSpaces "

Imports HTS.SAS.Entities
Imports HTS.SAS.BusinessObjects
Imports System
Imports System.Data
Imports System.IO
Imports System.IO.FileSystemEventArgs
Imports System.Collections.Generic
Imports MaxGeneric
Imports System.Configuration
Imports System.Globalization

#End Region

Partial Class PTPTNFileGeneration
    Inherits System.Web.UI.Page

#Region "Page Load "

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Not IsPostBack() Then
            onadd()
            Menuname(CInt(Request.QueryString("Menuid")))
            ibtnDbtDate.Attributes.Add("onClick", "return getDbtDate()")
            txtDebitingDate.Attributes.Add("OnKeyup", "return CheckDbtDate()")
            hdnFileNoCount.Value = 0
            Response.Clear()
        End If
        lblMsg.Text = ""
        FileUpload1.ForeColor = Drawing.Color.Black
        'Added by Hafiz @ 14/1/2016
        'Enable pnlSearch
        pnlSearch.Visible = True
    End Sub

#End Region


#Region "Methods "
    ''' <summary>
    ''' Method to get the MenuName
    ''' </summary>
    ''' <param name="MenuId">Parameter is MenuId</param>
    ''' <remarks></remarks>
    Private Sub Menuname(ByVal MenuId As Integer)
        Dim eobj As New MenuEn
        Dim bobj As New MenuBAL
        eobj.MenuId = MenuId
        eobj = bobj.GetMenus(eobj)
        lblMenuName.Text = eobj.MenuName
    End Sub

#End Region

#Region "On New"
    Protected Sub ibtnNew_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ibtnNew.Click
        onadd()
    End Sub
#End Region

#Region "On Clear"
    Protected Sub ibtnCancel_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ibtnCancel.Click
        onadd()
    End Sub
#End Region

    Private Sub onadd()
        Session("PageMode") = "Add"
        onClearData()
    End Sub

    Private Sub onClearData()

        Session("ListObject") = Nothing

        pnlDisplay.Visible = False
        lblFileName.Text = ""
        lblTotalStudent.Text = ""
        lblTotalAmount.Text = ""
        txtDebitingDate.Text = ""

    End Sub

#Region "Display Message "

    Private Sub DisplayMessage(ByVal MessageToDisplay As String)

        lblMsg.Text = String.Empty
        lblMsg.Text = MessageToDisplay

    End Sub

#End Region

#Region "File Paths "

    Private ReadOnly Property GetUploadFilePath As String
        Get
            Return clsGeneric.NullToString(
                ConfigurationManager.AppSettings("PTPTN_UPLOAD_PATH"))
        End Get
    End Property

    Private ReadOnly Property GetDownloadFilePath As String
        Get
            'Return clsGeneric.NullToString(ConfigurationManager.AppSettings("PTPTN_DOWNLOAD_PATH"))
            Return ConfigurationManager.AppSettings("DIRECT_DEBIT_FILE_DOWNLOAD_PATH")
        End Get
    End Property

#End Region

#Region "btnGenerate_Click "
    'modified by Hafiz @ 24/5/2016

    Protected Sub btnGenerate_Click(sender As Object, e As EventArgs) Handles btnGenerate.Click

        'Create Instances
        Dim _FileHelper As New FileHelper()

        'Variable Declarations - Start
        Dim TotalAmount As Decimal = 0, TotalRecords As Integer = 0, MandateTotalRecords As Integer = 0, Contents As String = Nothing
        Dim UploadedPtptnFile As String = Nothing, DirectDebitFile As String = Nothing, MandateFile As String = Nothing, DDFail As String = Nothing
        Dim ActiveTotalRecords As Integer = 0, InactiveTotalRecords As Integer = 0, dt_result As DateTime = Nothing
        'Variable Declarations - Stop

        Try
            'Get Uploaded File - Start
            UploadedPtptnFile = FileUpload1.FileName
            UploadedPtptnFile = GetUploadFilePath & Path.GetFileName(UploadedPtptnFile)
            'Get Uploaded File - Stop

            'Check file uploaded - Start
            If _FileHelper.IsPtptnFileUploaded(UploadedPtptnFile) Then
                Call DisplayMessage("File Uploaded Previously")
                Exit Sub
            End If
            'Check file uploaded - Stop

            If txtDebitingDate.Text <> "" Then
                Dim myformat As String = "dd/MM/yyyy"
                Dim DbtDate As String = Trim(txtDebitingDate.Text)

                If Not DateTime.TryParseExact(DbtDate, myformat, New CultureInfo("en-US"), Globalization.DateTimeStyles.None, dt_result) Then
                    Throw New Exception("Debiting Date Format Error")
                End If

            End If

            'Save File
            FileUpload1.SaveAs(UploadedPtptnFile)

            'Generate Direct Debit File
            If _FileHelper.GenerateDirectDebitFile(UploadedPtptnFile, ActiveTotalRecords, TotalAmount, DirectDebitFile, dt_result) Then

                hdnDirectDebitFile.Value = DirectDebitFile

                pnlDisplay.Visible = True
                pnlSearch.Visible = False
                FileUpload1.ForeColor = Drawing.Color.Transparent

                Dim faildd As String = Session("DirectDebitFileFail")

                If Not faildd Is Nothing Then

                    Dim _StringBuilder As New StringBuilder

                    InactiveTotalRecords = Session("FailTotalRecords")

                    'Build Text File Name - Start
                    DDFail = Path.GetFileNameWithoutExtension(UploadedPtptnFile)
                    DDFail &= "_" & Format(CDate(Now), "yyyyMMdd") & "_FAILED.txt"
                    DDFail = GetDownloadFilePath() & DDFail
                    'Build Text File Name - Start

                    'Concate Header/Details/Footer Contents - Start
                    _StringBuilder.AppendLine(faildd)
                    'Concate Header/Details/Footer Contents - Stop

                    'Create File - Start
                    If InactiveTotalRecords > 0 Then
                        Call MaxGeneric.clsGeneric.CreateFile(DDFail, False, _StringBuilder.ToString())
                    End If
                    'Create File - Stop

                    If System.IO.File.Exists(DDFail) Then
                        tblDDFail.Visible = True
                        hdnDDFail.Value = DDFail
                    End If
                End If

                Dim obj As Object = Session("lststudent")
                Contents = _FileHelper.MandateGenerateToTextFile(obj, MandateTotalRecords)

                If Not Contents Is Nothing Then

                    Dim _StringBuilder As New StringBuilder

                    'Build Text File Name - Start
                    MandateFile = Path.GetFileNameWithoutExtension(UploadedPtptnFile)
                    MandateFile &= "_" & Format(CDate(Now), "yyyyMMdd") & "_MANDATE.txt"
                    MandateFile = GetDownloadFilePath() & MandateFile
                    'Build Text File Name - Start

                    'Concate Header/Details/Footer Contents - Start
                    _StringBuilder.AppendLine(Contents)
                    'Concate Header/Details/Footer Contents - Stop

                    'Create File - Start
                    If MandateTotalRecords > 0 Then
                        Call MaxGeneric.clsGeneric.CreateFile(MandateFile, False, _StringBuilder.ToString())
                    End If
                    'Create File - Stop

                    If System.IO.File.Exists(MandateFile) Then
                        tblMandateFile.Visible = True
                        hdnMandateFile.Value = MandateFile
                    End If

                End If

                Call DisplayMessage("PTPTN File Generated Successfully")

                'Display File Details
                TotalRecords = ActiveTotalRecords + InactiveTotalRecords
                Call TextFileToLabel(DirectDebitFile, TotalAmount, TotalRecords, ActiveTotalRecords, InactiveTotalRecords, dt_result)

                'Track File Details - Start
                Call _FileHelper.TrackPtptnFileDetails(UploadedPtptnFile, DirectDebitFile, TotalAmount, TotalRecords, dt_result)
                'Track File Details - Stop

            Else

                'Show Panel
                pnlDisplay.Visible = False

                'Display Error Message
                Call DisplayMessage("File Generation Failed")

            End If

        Catch ex As Exception

            'Log & Display Error
            Call MaxModule.Helper.LogError(ex.Message)
            Call DisplayMessage(ex.Message)

        End Try

    End Sub

#End Region

#Region "TextFileToLabel "
    'modified by Hafiz @ 25/05/2016

    Private Sub TextFileToLabel(ByVal DirectDebitFile As String, ByVal TotalAmount As Decimal, ByVal TotalRecords As Integer,
                                ByVal ActiveTotalRecords As Integer, ByVal InactiveTotalRecords As Integer,
                                Optional ByVal dt_result As DateTime = Nothing)

        lblFileName.Text = DirectDebitFile

        trDbtDate.Visible = False
        If dt_result <> Nothing Then
            trDbtDate.Visible = True
            lblDbtDate.Text = Format(dt_result, "dd/MM/yyyy")
        End If

        lblTotalAmount.Text = clsGeneric.SetCurrencyFormat(TotalAmount)
        lblTotalStudent.Text = TotalRecords
        lblTotalStudentActive.Text = ActiveTotalRecords
        lblTotalStudentInactive.Text = InactiveTotalRecords
    End Sub

#End Region

#Region "Button Download"
    'modified by Hafiz @ 25/05/2016

    Protected Sub btnDDFileDownload_Click(sender As Object, e As EventArgs) Handles btnDDFileDownload.Click
        Dim value = hdnDirectDebitFile.Value
        Call DownloadFile(value)
    End Sub

    Protected Sub btnDDFileDownloadFail_Click(sender As Object, e As EventArgs) Handles btnDDFileDownloadFail.Click
        Dim value = hdnDDFail.Value
        Call DownloadFile(value)
    End Sub

    Protected Sub btnDDMandateFile_Click(sender As Object, e As EventArgs) Handles btnDDMandateFile.Click
        Dim value = hdnMandateFile.Value
        Call DownloadFile(value)
    End Sub

    Protected Sub DownloadFile(ByVal val As String)
        Dim file As System.IO.FileInfo
        file = New System.IO.FileInfo(val)

        Dim response As System.Web.HttpResponse
        response = System.Web.HttpContext.Current.Response

        response.ClearContent()
        response.Clear()
        response.ContentType = "text/plain"
        response.AddHeader("Content-Disposition", "attachment; filename=" + file.Name + ";")
        response.TransmitFile(val)
        response.Flush()
        response.End()
    End Sub

#End Region

End Class
