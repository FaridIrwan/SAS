Imports System.IO
Imports HTS.SAS.BusinessObjects
Imports HTS.SAS.Entities
Imports System.Data
Imports System.Collections.Generic

Partial Class FileUpload
    Inherits System.Web.UI.Page
    Dim CFlag As String
    Dim DFlag As String


    Protected Sub Button1_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Button1.Click
        'Dim lsDelimeter As String = txtDelimeter.Text
        Dim lsDelimeter As Char = txtDelimeter.Text
        If FileUpload1.HasFile Then

            Dim myFileStream As FileStream
            Dim fn As String = System.IO.Path.GetFileName(FileUpload1.PostedFile.FileName)
            Dim SaveLocation As String = Server.MapPath("Data") & "\" & fn
            Dim fileOK As Boolean = False

            Dim fileExtension As String
            fileExtension = System.IO.Path. _
                GetExtension(FileUpload1.FileName).ToLower()
            Dim allowedExtensions As String() = _
                {".txt"}
            For i As Integer = 0 To allowedExtensions.Length - 1
                If fileExtension = allowedExtensions(i) Then
                    fileOK = True
                End If
            Next


            If fileOK Then
                Try
                    FileUpload1.PostedFile.SaveAs(SaveLocation)
                Catch ex As Exception
                    Response.Write("File Upload Failed.")
                End Try
            Else
                Response.Write("Select a Valid File to Upload.")
            End If

            Dim loReader As New StreamReader(SaveLocation)

            Dim filename As String = "\\WINDEV\Uploadfiles" & "\" & txtStuName.Text

            myFileStream = New FileStream(filename, FileMode.OpenOrCreate, FileAccess.ReadWrite)

            'loReader = File.OpenText(FileUpload1.PostedFile.FileName)
            Dim loWriter As New StreamWriter(myFileStream)
            'Response.Write(FileUpload1.PostedFile.FileName)

            'While loReader.Read > 0
            Do While True
                Dim lsRow As String = loReader.ReadLine()
                If lsRow Is Nothing Then
                    Exit Do
                Else
                    Dim lsArr = Split(lsRow, txtDelimeter.Text)
                    Try
                        txtColumns.Text = lsArr.Length
                        loWriter.WriteLine(" " + lsArr(CInt(txtMatrixNo.Text) - 1).Trim() + "," + lsArr(CInt(txtICNo.Text) - 1).Trim() + "," + lsArr(CInt(txtName.Text) - 1).Trim() + "," + lsArr(CInt(txtAmount.Text) - 1).Trim())
                    Catch ex As Exception
                        lblMsg.Text = "Check your inputs"
                    End Try

                End If
            Loop
            'End While
            loReader.Close()
            loWriter.Close()
            If System.IO.File.Exists(SaveLocation) Then
                System.IO.File.Delete(SaveLocation)
            End If

            lblMsg.Text = "File Saved Successfully "
        Else

            lblMsg.Text = "Select a File"
            Exit Sub
        End If


    End Sub

    Private Sub Menuname(ByVal MenuId As Integer)
        Dim eobj As New MenuEn
        Dim bobj As New MenuBAL
        eobj.MenuId = MenuId
        Try
            eobj = bobj.GetMenus(eobj)
        Catch ex As Exception
            LogError.Log("FileUpload", "Menuname", ex.Message)
        End Try
        lblMenuName.Text = eobj.MenuName

    End Sub
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Not IsPostBack() Then
            'GetUpload()
            LoadUserRights()
            ImageButton1.Attributes.Add("onClick", "return Validation()")
            onadd()
            DisableRecordNavigator()
            Button1.Attributes.Add("onClick", "return Validation()")

        End If

        lblMsg.Text = ""
    End Sub


    Private Sub onSave()
    End Sub

    Private Sub GetUpload()
    End Sub
    Private Sub onadd()
        ImageButton1.Enabled = True

        ImageButton1.ImageUrl = "images/save.png"
        ImageButton1.ToolTip = "Save"
        Session("PageMode") = "Add"
        onClearData()
        txtColumns.Text = "Auto Number"
    End Sub
    Private Sub LoadUserRights()
        Dim obj As New UsersBAL
        Dim eobj As New UserRightsEn


        Try
            eobj = obj.GetUserRights(CInt(Request.QueryString("Menuid")), CInt(Session("UserGroup")))
        Catch ex As Exception
            LogError.Log("FileUpload", "LoadUserRights", ex.Message)
        End Try
        'Rights for Add
        If eobj.IsAdd = True Then
            ImageButton1.Enabled = True
            onadd()
            ImageButton1.ToolTip = "Save"
            ImageButton1.ImageUrl = "images/save.png"
        Else
            ImageButton1.Enabled = False
            ImageButton1.ImageUrl = "images/gsave.png"
            ImageButton1.ToolTip = "Access Denied"
        End If
        'Rights for Edit
        If eobj.IsEdit = True Then
            'ibtnSave.ToolTip = "Access Denied"
            Session("EditFlag") = True
        Else
            Session("EditFlag") = False
        End If
        'Rights for View
        ibtnV.Enabled = eobj.IsView
        If eobj.IsView = True Then
            ibtnV.ImageUrl = "images/find.png"
            ibtnV.Enabled = True
        Else
            ibtnV.ImageUrl = "images/gfind.png"
            ibtnV.ToolTip = "Access Denied"
        End If
        'Rights for Print
        ibtnPrint.Enabled = eobj.IsPrint
        If eobj.IsPrint = True Then
            ibtnPrint.Enabled = True
            ibtnPrint.ImageUrl = "images/print.png"
            ibtnPrint.ToolTip = "Print"
        Else
            ibtnPrint.Enabled = False
            ibtnPrint.ImageUrl = "images/gprint.png"
            ibtnPrint.ToolTip = "Access Denied"
        End If

        If eobj.IsOthers = True Then
            ibtnOthers.Enabled = True
            ibtnOthers.ImageUrl = "images/others.png"
            ibtnOthers.ToolTip = "Others"
        Else
            ibtnOthers.Enabled = False
            ibtnOthers.ImageUrl = "images/gothers.png"
            ibtnOthers.ToolTip = "Access Denied"
        End If
        If eobj.IsPost = True Then
            ibtnPosting.Enabled = True
            ibtnPosting.ImageUrl = "images/posting.png"
            ibtnPosting.ToolTip = "Posting"
        Else
            ibtnPosting.Enabled = False
            ibtnPosting.ImageUrl = "images/gposting.png"
            ibtnPosting.ToolTip = "Access Denied"
        End If
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
    Private Sub loadlistobjects()

    End Sub

    Protected Sub ibtnNext_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ibtnNext.Click
        OnMoveNext()
    End Sub

    Protected Sub ibtnLast_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ibtnLast.Click
        OnMoveLast()
    End Sub

    Protected Sub ibtnPrevs_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ibtnPrevs.Click
        OnMovePrevious()
    End Sub

    Protected Sub ibtnFirst_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ibtnFirst.Click
        OnMoveFirst()
    End Sub
    Private Sub OnMoveFirst()
        txtRecNo.Text = "1"
        FillData(0)
    End Sub
    Private Sub OnMoveNext()
        txtRecNo.Text = CInt(txtRecNo.Text) + 1
        FillData(CInt(txtRecNo.Text) - 1)
    End Sub
    Private Sub OnMovePrevious()
        txtRecNo.Text = CInt(txtRecNo.Text) - 1
        FillData(CInt(txtRecNo.Text) - 1)
    End Sub
    Private Sub OnMoveLast()
        txtRecNo.Text = lblCount.Text
        FillData(CInt(lblCount.Text) - 1)
    End Sub
    Private Sub FillData(ByVal RecNo As Integer)
        'Conditions for Button Enable & Disable
        If txtRecNo.Text = lblCount.Text Then
            ibtnNext.Enabled = False
            ibtnNext.ImageUrl = "images/gnew_next.png"
            ibtnLast.Enabled = False
            ibtnLast.ImageUrl = "images/gnew_last.png"
        Else
            ibtnNext.Enabled = True
            ibtnNext.ImageUrl = "images/new_next.png"
            ibtnLast.Enabled = True
            ibtnLast.ImageUrl = "images/new_last.png"
        End If
        If txtRecNo.Text = "1" Then
            ibtnPrevs.Enabled = False
            ibtnPrevs.ImageUrl = "images/gnew_Prev.png"
            ibtnFirst.Enabled = False
            ibtnFirst.ImageUrl = "images/gnew_first.png"
        Else
            ibtnPrevs.Enabled = True
            ibtnPrevs.ImageUrl = "images/new_prev.png"
            ibtnFirst.Enabled = True
            ibtnFirst.ImageUrl = "images/new_first.png"
        End If

    End Sub

    Protected Sub txtRecNo_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs)

    End Sub
    Private Sub onClearData()
        txtMatrixNo.Text = ""
        txtICNo.Text = ""
        txtName.Text = ""
        txtDelimeter.Text = ""
        txtAmount.Text = ""
        txtStuName.Text = ""
        txtRef.Text = ""
        txtColumns.Text = ""
        txtReference.Text = ""
        Session("ListObject") = Nothing
        DisableRecordNavigator()
    End Sub

    Protected Sub ibtnNew_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ibtnNew.Click
        onadd()

    End Sub

    Protected Sub ibtnCancel_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ibtnCancel.Click
        onadd()
    End Sub
    Private Sub OnDelete()

    End Sub


    Protected Sub ibtnDelete_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ibtnDelete.Click
        OnDelete()
    End Sub


    Protected Sub ibtnV_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ibtnV.Click
        Session("loaddata") = "View"
        If lblCount.Text <> "" Then
            If CInt(lblCount.Text) > 0 Then
                onClearData()
            Else
                loadlistobjects()
                Session("PageMode") = "Edit"
            End If
        Else
            loadlistobjects()
            Session("PageMode") = "Edit"
        End If
    End Sub

    Protected Sub ImageButton1_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ImageButton1.Click
        onSave()
    End Sub
End Class
