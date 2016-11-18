#Region "NameSpaces "

Imports System.IO
Imports MaxGeneric
Imports HTS.SAS.Entities
Imports HTS.SAS.BusinessObjects
Imports HTS.SAS.DataAccessObjects
Imports System.Collections.Generic

#End Region

Partial Class CimbClicksTrans
    Inherits System.Web.UI.Page

#Region "File Paths "

    Private ReadOnly Property GetUploadFilePath As String
        Get
            Return clsGeneric.NullToString(
                ConfigurationManager.AppSettings("CIMB_CLICKS_UPLOAD_PATH"))
        End Get
    End Property

#End Region

#Region "Display Message "

    Private Sub DisplayMessage(ByVal MessageToDisplay As String)

        lblMsg.Text = String.Empty
        lblMsg.Text = MessageToDisplay

    End Sub

#End Region

#Region "Page Load "

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        'Create Instances - Start
        Dim _BankProfileEn As New BankProfileEn
        Dim _BankProfileBAL As New BankProfileBAL
        Dim ListBankProfileEn As New List(Of BankProfileEn)
        'Create Instances - Stop

        Try
            'if page is not post back - Start
            If Not Page.IsPostBack Then

                'Adding validation for save button
                ibtnSave.Attributes.Add("onclick", "return validate()")

                ''Set Values - Start
                '_BankProfileEn.Status = True
                '_BankProfileEn.ACCode = String.Empty
                '_BankProfileEn.GLCode = String.Empty
                '_BankProfileEn.Description = String.Empty
                '_BankProfileEn.BankDetailsCode = String.Empty
                ''Set Values - Stop

                ''Get Bank Codes
                'ListBankProfileEn = _BankProfileBAL.GetBankProfileList(_BankProfileEn)

                ''Populate Drop Down List - Start
                'ddlBankCode.Items.Clear()
                'ddlBankCode.Items.Add(New ListItem("---Select---", "-1"))
                'ddlBankCode.DataTextField = "Description"
                'ddlBankCode.DataValueField = "BankDetailsCode"
                'ddlBankCode.DataSource = ListBankProfileEn
                'ddlBankCode.DataBind()
                ''Populate Drop Down List - Stop

                'Populate Drop Down List - Start
                Call BindBankCode()
                'Populate Drop Down List - Stop

            End If
            'if page is not post back - Stop

        Catch ex As Exception

            'Log Error
            Call DisplayMessage(ex.Message)
            Call MaxModule.Helper.LogError(ex.Message)

        End Try

    End Sub

#End Region

#Region "File Upload "
    'modified by Hafiz @ 04/6/2016

    Protected Sub File_Upload(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnUpload.Click

        'Create Instances
        Dim _FileHelper As New FileHelper

        'Variable Declarations - Start
        Dim BankCode As String = Nothing, TotalRecords As Integer = 0, HeaderNo As String = Nothing
        Dim UploadedClicksFile As String = Nothing, TotalAmount As Decimal = 0
        'Variable Declarations - Stop

        Try
            'Get Bank Code
            BankCode = ddlBankCode.SelectedValue

            'Get Uploaded File - Start
            UploadedClicksFile = UploadFile.FileName
            UploadedClicksFile = GetUploadFilePath & Path.GetFileName(UploadedClicksFile)
            'Get Uploaded File - Stop

            'Save File
            UploadFile.SaveAs(UploadedClicksFile)

            'Check file uploaded - Start
            'Modified by Hafiz Roslan @ 11-1-2016
            If _FileHelper.IsClicksFileUploaded(UploadedClicksFile, HeaderNo) Then
                Call DisplayMessage(UploadFile.FileName & " - File Uploaded Previously")
                lblFileUpload.Text = ""
                Exit Sub
            End If
            'Check file uploaded - Stop   

            'if file uploaded Successfully - Start
            If _FileHelper.UploadCimbClicksFile(UploadedClicksFile,
                dgClicksTransactions, TotalAmount, TotalRecords, BankCode) Then

                'Show Panel
                pnlDisplay.Visible = True
                'Added By Zoya @8/03/2016
                _summry.Visible = True
                'End Added By Zoya @8/03/2016
                batch_code.Visible = False

                'Display Error Message
                Call DisplayMessage("File Uploaded Successfully")

                'Added By Zoya @7/03/2016
                'Dispaly a Message if the student Name Does not Exist
                Dim StudentName As String

                For Each dgitem In dgClicksTransactions.Items
                    StudentName = dgitem.Cells(0).Text
                    If StudentName = "&nbsp;" Then
                        StudentNameMsg.Text = "Student Record not Exist"
                    End If
                Next
                'End Added By Zoya @7/03/2016

                'Added by Hafiz Roslan @ 12/01/2016
                'For the File Upload related matters
                UploadFile.Visible = False
                lblFileUpload.Text = UploadFile.FileName

                'Display File Details
                Call TextFileToLabel(UploadedClicksFile, TotalAmount, TotalRecords, HeaderNo)

                'Reset Controls
                btnUpload.Visible = False

            Else
                'Show Panel
                pnlDisplay.Visible = False

                'Display Error Message
                Call DisplayMessage("File Upload Failed")

                lblFileUpload.Text = UploadFile.FileName

                'Reset Controls
                btnUpload.Visible = False

            End If
            'if file uploaded Successfully - Stop

        Catch ex As Exception

            'Log Error
            Call MaxModule.Helper.LogError(ex.Message)
        Finally

        End Try

    End Sub

#End Region

#Region "TextFileToLabel "

    Private Sub TextFileToLabel(ByVal UploadedClicksFile As String,
        ByVal TotalAmount As Decimal, ByVal TotalRecords As Integer, ByVal HeaderNo As String)

        lblFileName.Text = UploadedClicksFile
        lblTotalStudent.Text = TotalRecords
        lblTotalAmount.Text = clsGeneric.SetCurrencyFormat(TotalAmount)
        'Added by Hafiz Roslan @ 12/01/2016
        hidHeaderNo.Value = HeaderNo
        'Assign header no
    End Sub

#End Region

#Region "On Save "
    'updated by Hafiz @ 19/7/2016

    Protected Sub File_Save(ByVal sender As Object, ByVal e As System.EventArgs) Handles ibtnSave.Click

        'Create Instances
        Dim _FileHelper As New FileHelper

        'Variable Declarations
        Dim TotalRecords As Integer = 0
        Dim UploadedClicksFile As String = Nothing, TotalAmount As Decimal = 0, HeaderNo As String = Nothing
        Dim BatchCode As String = Nothing

        Try

            'Get Values - Start
            TotalAmount = lblTotalAmount.Text
            TotalRecords = lblTotalStudent.Text
            UploadedClicksFile = Path.GetFileName(lblFileName.Text)
            'Get Values - Stop

            'Added by Hafiz Roslan @ 12/01/2016
            'Assign value to headerno
            HeaderNo = hidHeaderNo.Value

            'Save Data - Start
            If _FileHelper.InsertClicksTransToAccounts(dgClicksTransactions,
                Session(Helper.UserSession), lblTotalAmount.Text, lblFileName.Text,
                ddlBankCode.SelectedValue, HeaderNo, BatchCode) Then

                Call DisplayMessage("Records Saved Successfully")

                batch_code.Visible = True

                lblBatchCode.ForeColor = Drawing.Color.Red
                lblBatchCode.Text = BatchCode
                lblFileUpload.Text = UploadedClicksFile

                'Track File Details - Start
                Call _FileHelper.TrackClicksFileDetails(UploadedClicksFile,
                    TotalAmount, TotalRecords, ddlBankCode.SelectedValue, HeaderNo)
                'Track File Details - Stop

                ibtnPosting.Enabled = True
                ibtnPosting.ImageUrl = "~/images/posting.png"
                ibtnPosting.ToolTip = "Posting"

            Else
                Call DisplayMessage("Records Failed to Save")

                lblFileUpload.Text = UploadedClicksFile

            End If
            'Save Data - Stop


        Catch ex As Exception

            'Log Error
            Call MaxModule.Helper.LogError(ex.Message)

        End Try

    End Sub


#End Region

#Region "On Post "
    'modified by Hafiz @ 19/7/2016

    Protected Sub File_Post(ByVal sender As Object, ByVal e As System.EventArgs) Handles ibtnPosting.Click

        'Create Instances - Start
        Dim Workflow As New WorkflowDAL()
        Dim _AccountsDAL As New AccountsDAL()
        Dim _DataGridItem As DataGridItem = Nothing
        'Create Instances - Stop

        'Variable declarations
        Dim BatchCode As String = Nothing

        Try

            BatchCode = lblBatchCode.Text

            If Not Workflow.Workflow(BatchCode, Session("User"), Me.ToString()) Then
                Call DisplayMessage("Posting to workflow failed.")
                Exit Sub
            Else
                Call DisplayMessage("Record Posted Successfully for Approval")
            End If

            'commented by Hafiz @ 19/7/2016
            'Loop thro the Data Grid Items - Start
            'For Each _DataGridItem In dgClicksTransactions.Items

            '    'get batch Code
            '    BatchCode = _DataGridItem.Cells(6).Text

            '    'Post To workflow for apporval - Start
            '    'If Not Workflow.Workflow(BatchCode, Session(Helper.UserSession), Me.ToString()) Then
            '    If Not Workflow.Workflow(BatchCode, Session("User"), Me.ToString()) Then

            '        'Update Accounts Details - Start
            '        'If Not _AccountsDAL.UpdatePostingStatus(BatchCode, Session(Helper.UserSession)) Then
            '        Call DisplayMessage("Posting to workflow failed.")
            '        Exit Sub
            '        'End If
            '        'Update Accounts Details - Stop

            '    End If
            '    'Post To workflow for apporval - Stop

            'Next
            ''Loop thro the Data Grid Items - Stop

            'Call DisplayMessage("Record Posted Successfully for Approval")

        Catch ex As Exception

            'Log Error
            Call MaxModule.Helper.LogError(ex.Message)

        End Try

    End Sub

#End Region

#Region "On Clear"
    Protected Sub File_Cancel(ByVal sender As Object, ByVal e As System.EventArgs) Handles ibtnCancel.Click
        Call ClearControls()
    End Sub
#End Region

#Region "On New"
    Protected Sub File_New(ByVal sender As Object, ByVal e As System.EventArgs) Handles ibtnNew.Click
        Call ClearControls()
    End Sub
#End Region

#Region "Clear Controls"
    Public Sub ClearControls()

        'Dropdown bind- Start
        Call BindBankCode()
        'DropDown bind - End
    End Sub
#End Region

#Region "BindBankCod"
    Public Sub BindBankCode()
        'Create Instances - Start
        Dim _BankProfileEn As New BankProfileEn
        Dim _BankProfileBAL As New BankProfileBAL
        Dim ListBankProfileEn As New List(Of BankProfileEn)
        'Create Instances - Stop

        'Set Values - Start
        _BankProfileEn.Status = True
        _BankProfileEn.ACCode = String.Empty
        _BankProfileEn.GLCode = String.Empty
        _BankProfileEn.Description = String.Empty
        _BankProfileEn.BankDetailsCode = String.Empty
        'Set Values - Stop

        'Get Bank Codes
        ListBankProfileEn = _BankProfileBAL.GetBankProfileList(_BankProfileEn)

        'Populate Drop Down List - Start
        ddlBankCode.Items.Clear()
        ddlBankCode.Items.Add(New ListItem("---Select---", "-1"))
        ddlBankCode.DataTextField = "Description"
        ddlBankCode.DataValueField = "BankDetailsCode"
        ddlBankCode.DataSource = ListBankProfileEn
        ddlBankCode.DataBind()
        'Populate Drop Down List - Stop

        'clear Controls - Start
        dgClicksTransactions.DataSource = Nothing
        dgClicksTransactions.DataBind()
        _summry.Visible = False
        pnlDisplay.Visible = False
        batch_code.Visible = False
        lblMsg.Text = ""
        lblFileName.Text = ""
        lblTotalAmount.Text = ""
        lblTotalStudent.Text = ""
        lblFileUpload.Text = ""
        UploadFile.Visible = True

        'Added by Zoya @7/03/2016
        StudentNameMsg.Text = ""
        btnUpload.Visible = True
        'End Added by Zoya @7/03/2016

        'clear controls - End
    End Sub
#End Region

End Class
