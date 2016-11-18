#Region "NameSpaces "

Imports System.IO
Imports MaxGeneric
Imports HTS.SAS.Entities
Imports HTS.SAS.BusinessObjects
Imports HTS.SAS.DataAccessObjects
Imports System.Collections.Generic

#End Region

Partial Class Bank_Recon
    Inherits System.Web.UI.Page

#Region "File Paths "

    Private ReadOnly Property GetUploadFilePath As String
        Get
            Return clsGeneric.NullToString(
                ConfigurationManager.AppSettings("BANK_RECON_UPLOAD_PATH"))
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

    Protected Sub File_Upload(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnUpload.Click

        'Create Instances
        Dim _FileHelper As New FileHelper

        'Variable Declarations - Start
        Dim BankCode As String = Nothing, TotalRecords As Integer = 0
        Dim UploadedReconFile As String = Nothing, TotalAmount As Decimal = 0
        Dim MatchingTotalAmount As Decimal = 0, MatchingTotalRecords As Integer = 0
        Dim UnMatchingTotalAmount As Decimal = 0, UnMatchingTotalRecords As Integer = 0
        'Variable Declarations - Stop

        Try

            'Get Bank Code
            BankCode = ddlBankCode.SelectedValue

            'Get Uploaded File - Start
            UploadedReconFile = UploadFile.FileName
            UploadedReconFile = GetUploadFilePath & Path.GetFileName(UploadedReconFile)
            'Get Uploaded File - Stop

            'Check file uploaded - Start
            If _FileHelper.IsReconFileUploaded(UploadedReconFile) Then
                Call DisplayMessage("File Uploaded Previously")
                Exit Sub
            End If
            'Check file uploaded - Stop

            'Save File
            UploadFile.SaveAs(UploadedReconFile)

            'if file uploaded Successfully - Start
            If _FileHelper.UploadBankStatement(UploadedReconFile, dgMatchingRecords,
                dgUnMatchingRecords, MatchingTotalAmount, MatchingTotalRecords,
                UnMatchingTotalAmount, UnMatchingTotalRecords) Then

                'Show Panels - Start
                pnlMatch.Visible = True
                pnlUnMatch.Visible = True
                GridMatch.Visible = True
                GridUnMatch.Visible = True
                'Show Panels - Stop

                'Display Error Message

                'dgTest.DataSource = Session("FileData")
                'dgTest.DataBind()

                Call DisplayMessage("File Uploaded Successfully")

                'Display File Details - Start
                Call TextFileToLabel(UploadedReconFile, MatchingTotalAmount,
                    MatchingTotalRecords, UnMatchingTotalAmount, UnMatchingTotalRecords)
                'Display File Details - Stop

                'Track File Details - Start
                Call _FileHelper.TrackReconFileDetails(UploadedReconFile,
                    MatchingTotalAmount, MatchingTotalRecords, UnMatchingTotalAmount,
                    UnMatchingTotalRecords, BankCode)
                'Track File Details - Stop

            Else

                'Show Panels - Start
                pnlMatch.Visible = False
                pnlUnMatch.Visible = False
                GridMatch.Visible = False
                GridUnMatch.Visible = False
                'Show Panels - Stop

                'Display Error Message
                Call DisplayMessage("File Upload Failed")

            End If
            'if file uploaded Successfully - Stop

        Catch ex As Exception

            'Log Error
            Call MaxModule.Helper.LogError(ex.Message)

        End Try

    End Sub

#End Region

#Region "TextFileToLabel "

    Private Sub TextFileToLabel(ByVal UploadedReconFile As String,
        ByVal MatchingTotalAmount As Decimal, ByVal MatchingTotalRecords As Integer,
        ByVal UnMatchingTotalAmount As Decimal, ByVal UnMatchingTotalRecords As Integer)

        lblTotalRecords.Text = MatchingTotalRecords
        lblTotalAmount.Text = clsGeneric.SetCurrencyFormat(MatchingTotalAmount)
        lblUnTotalRecords.Text = UnMatchingTotalRecords
        lblUnTotalAmount.Text = clsGeneric.SetCurrencyFormat(UnMatchingTotalAmount)

    End Sub

#End Region

End Class
