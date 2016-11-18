#Region "NameSpaces "

Imports System.IO
Imports MaxGeneric
Imports HTS.SAS.Entities
Imports HTS.SAS.BusinessObjects
Imports HTS.SAS.DataAccessObjects
Imports System.Collections.Generic

#End Region

Partial Class StudentAcctsUpd
    Inherits System.Web.UI.Page

#Region "File Paths "

    Private ReadOnly Property GetUploadFilePath As String
        Get
            Return clsGeneric.NullToString(
                ConfigurationManager.AppSettings("STUDENT_ACCOUNT_UPLOAD_PATH"))
        End Get
        'Set(value As String)
        '    UploadFile.ForeColor = Drawing.Color.Black
        'End Set
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
        'UploadFile.Attributes("onchange") = "UploadFileName(this)"
        If Not IsPostBack() Then

            onadd()
            DisableRecordNavigator()

        End If

        'If (UploadFile.HasFile) Then
        '    Labeluploadmessage.Text = UploadFile.FileName
        'End If
        lblMsg.Text = ""
        UploadFile.ForeColor = Drawing.Color.Black

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
        ibtnSave.Enabled = True

        ibtnSave.ImageUrl = "images/save.png"

        Session("PageMode") = "Add"
        onClearData()

    End Sub

    Private Sub onClearData()

        Session("ListObject") = Nothing

        pnlDisplay.Visible = False
        lblFileName.Text = ""
        lblTotalStudent.Text = ""
        DisableRecordNavigator()

    End Sub

    Private Sub DisableRecordNavigator()
        Dim flag As Boolean
        If Session("ListObject") Is Nothing Then
            flag = False
            txtRecNo.Text = ""
            lblCount.Text = ""
        Else
            flag = True
        End If
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

#Region "File Upload "

    Protected Sub File_Upload(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnUpload.Click

        'Create Instances
        Dim _FileHelper As New FileHelper

        'Variable Declarations - Start
        Dim TotalRecords As Integer = 0, CountUpdated As Integer = 0
        Dim UploadedStudentFile As String = Nothing
        'Variable Declarations - Stop

        Try

            'Get Uploaded File - Start
            UploadedStudentFile = UploadFile.FileName
            UploadedStudentFile = GetUploadFilePath & Path.GetFileName(UploadedStudentFile)
            'Get Uploaded File - Stop

            'Check file uploaded - Start
            If _FileHelper.IsStudentsFileUploaded(UploadedStudentFile) Then
                Call DisplayMessage("File Uploaded Previously")
                Exit Sub
            End If
            'Check file uploaded - Stop

            'Save File
            UploadFile.SaveAs(UploadedStudentFile)

            'if file uploaded Successfully - Start
            If _FileHelper.UpdateStudentAccounts(UploadedStudentFile, TotalRecords, CountUpdated) Then

                'Show Panel
                pnlDisplay.Visible = True

                'modified by Hafiz @ 26/4/2016
                If Not Session("return") Is Nothing Then

                    If Session("return") = "one_!same" Then

                        If Session("matricno_exist") = "yes" Then
                            Call DisplayMessage("Some of the Student records doesn't exists and will be ignored. So, new records updated and old records remains same.")
                        Else
                            Call DisplayMessage("Some of the Student`s Account No Already Exist. So, new records updated and old records remains same.")
                        End If

                    ElseIf Session("return") = "all_same" Then

                        If Session("matricno_exist") = "yes" Then

                            If TotalRecords = 0 Then

                                lblStudAccUpd.Text = ""
                                Throw New Exception("Student Records Does Not Exist. So, cannot update.")

                            Else
                                Call DisplayMessage("Some of the Student records doesn't exists and will be ignored. So, new records updated and old records remains same.")
                            End If

                        Else
                            Call DisplayMessage("Student`s Account No Already Exist.")
                        End If

                    End If
                Else

                    If Session("matricno_exist") = "yes" Then
                        Call DisplayMessage("Some of the Student records doesn't exists and will be ignored. So, new records updated and old records remains same.")
                    Else
                        Call DisplayMessage("File Uploaded Successfully")
                    End If

                End If
                'Display Error Message

                UploadFile.ForeColor = Drawing.Color.Transparent

                'Display File Details
                Call TextFileToLabel(UploadedStudentFile, TotalRecords, CountUpdated)

                'Track File Details - Start
                Call _FileHelper.TrackStudentFileDetails(
                    UploadedStudentFile, TotalRecords)
                'Track File Details - Stop

            Else
                'Show Panel
                pnlDisplay.Visible = False

                Call DisplayMessage("File Upload Failed")

            End If
                'if file uploaded Successfully - Stop

        Catch ex As Exception

            'Log Error

            'Added by Zoya @1/03/2016
            lblMsg.Text = ex.Message.ToString()
            'End Added by Zoya @1/03/2016

            Call MaxModule.Helper.LogError(ex.Message)

        Finally
            Session("return") = Nothing
            Session("matricno_exist") = Nothing
        End Try

    End Sub

#End Region

#Region "TextFileToLabel "

    Private Sub TextFileToLabel(ByVal UploadedClicksFile As String,
        ByVal TotalRecords As Integer, ByVal CountUpdated As Integer)

        lblFileName.Text = UploadedClicksFile
        lblTotalStudent.Text = TotalRecords

        'label Student Acc Updated
        lblStudAccUpd.ForeColor = Drawing.Color.Red
        lblStudAccUpd.Font.Bold = True
        lblStudAccUpd.Text = CountUpdated

    End Sub

#End Region

End Class
