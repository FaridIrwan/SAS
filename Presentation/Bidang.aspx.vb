Imports HTS.SAS.BusinessObjects
Imports HTS.SAS.Entities
Imports System.Data
Imports System.Collections.Generic

Partial Class Bidang
    Inherits System.Web.UI.Page
    Dim CFlag As String
    Dim DFlag As String
    Dim ListObjects As List(Of BidangEn)
    Private ErrorDescription As String

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Not IsPostBack() Then
            'Adding validation for save button
            ibtnSave.Attributes.Add("onclick", "return validate()")

            'While loading the page make the CFlag as null

            Session("PageMode") = ""
            'Loading User Rights
            LoadUserRights()

            'while loading list object make it nothing
            Session("ListObj") = Nothing
            DisableRecordNavigator()
            'load PageName
            Menuname(CInt(Request.QueryString("Menuid")))

        End If
        lblMsg.Text = ""
        lblMsg.Visible = False

    End Sub

    Private Sub LoadUserRights()
        Dim obj As New UsersBAL
        Dim eobj As New UserRightsEn

        Try
            eobj = obj.GetUserRights(CInt(Request.QueryString("Menuid")), CInt(Session("UserGroup")))
        Catch ex As Exception
            LogError.Log("Bidang", "LoadUserRights", ex.Message)
        End Try
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
        If eobj.IsEdit = True Then
            ibtnSave.Enabled = True
            ibtnSave.ImageUrl = "images/save.png"
            ibtnSave.ToolTip = "Edit"
            If eobj.IsAdd = False Then
                ibtnSave.Enabled = False
                ibtnSave.ImageUrl = "images/gsave.png"
                ibtnSave.ToolTip = "Access Denied"
            End If

            Session("EditFlag") = True

        Else
            Session("EditFlag") = False
            ibtnSave.Enabled = False
            ibtnSave.ImageUrl = "images/gsave.png"
        End If
        'Rights for View
        ibtnView.Enabled = eobj.IsView
        If eobj.IsView = True Then
            ibtnView.ImageUrl = "images/find.png"
            ibtnView.Enabled = True
        Else
            ibtnView.ImageUrl = "images/gfind.png"
            ibtnView.ToolTip = "Access Denied"
        End If
        'Rights for Delete
        If eobj.IsDelete = True Then
            ibtnDelete.ImageUrl = "images/delete.png"
            ibtnDelete.Enabled = True
        Else
            ibtnDelete.ImageUrl = "images/gdelete.png"
            ibtnDelete.ToolTip = "Access Denied"
            ibtnDelete.Enabled = False
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

    Private Sub Menuname(ByVal MenuId As Integer)
        Dim eobj As New MenuEn
        Dim bobj As New MenuBAL
        eobj.MenuId = MenuId
        Try
            eobj = bobj.GetMenus(eobj)
        Catch ex As Exception
            LogError.Log("Bidang", "Menuname", ex.Message)
        End Try
        lblMenuName.Text = eobj.MenuName
    End Sub

    Private Sub OnAdd()
        Session("PageMode") = "Add"
        PnlAdd.Visible = True
        OnClearData()
        dgView.SelectedIndex = -1
        PnlView.Visible = False
    End Sub

    Private Sub OnClearData()
        txtBidangCode.Enabled = True
        'Clear Text Box values
        Session("ListObj") = Nothing
        DisableRecordNavigator()
        txtBidangCode.Text = ""
        txtBidangName.Text = ""
        ddlStatus.SelectedValue = 1
        lblMsg.Text = ""

        Session("PageMode") = "Add"
    End Sub

    Protected Sub ibtnSave_Click(sender As Object, e As ImageClickEventArgs) Handles ibtnSave.Click
        SpaceValidation()
    End Sub

    Private Sub SpaceValidation()
        If Trim(txtBidangName.Text).Length = 0 Then

            txtBidangName.Text = Trim(txtBidangName.Text)
            lblMsg.Text = "Enter valid Bidang Description"
            lblMsg.Visible = True
            txtBidangName.Focus()
            Exit Sub
        End If

        OnSave()
    End Sub

    Private Sub OnSave()
        Dim bsobj As New BidangBAL
        Dim eobj As New BidangEn

        Dim RecAff As Integer
        eobj.BidangCode = Trim(txtBidangCode.Text)
        eobj.Description = Trim(txtBidangName.Text)
        If ddlStatus.SelectedValue = 1 Then
            eobj.Status = True
        Else
            eobj.Status = False
        End If
        eobj.UpdatedBy = Session("User")

        eobj.UpdatedDtTm = Date.Now.ToString()
        lblMsg.Visible = True

        If Session("PageMode") = "Add" Then
            Try
                RecAff = bsobj.Insert(eobj)
                ErrorDescription = eobj.BidangCode + " - Record Saved Successfully "
                lblMsg.Text = ErrorDescription

            Catch ex As Exception

                lblMsg.Text = ex.Message.ToString()
                LogError.Log("Bidang", "OnSave", ex.Message)
            End Try

        ElseIf Session("PageMode") = "Edit" Then
            Try
                RecAff = bsobj.Update(eobj)

                ListObjects = Session("ListObj")
                ListObjects(CInt(txtRecNo.Text) - 1) = eobj
                Session("ListObj") = ListObjects
                ErrorDescription = eobj.BidangCode + " - Record Updated Successfully "
                lblMsg.Text = ErrorDescription
            Catch ex As Exception
                lblMsg.Text = ex.Message.ToString()
                LogError.Log("Bidang", "OnSave", ex.Message)
            End Try
        End If

    End Sub

    Protected Sub ibtnNew_Click(sender As Object, e As ImageClickEventArgs) Handles ibtnNew.Click

        txtBidangCode.Enabled = True
        'Clear Text Box values
        Session("ListObj") = Nothing
        DisableRecordNavigator()
        txtBidangCode.Text = ""
        txtBidangName.Text = ""
        ddlStatus.SelectedValue = 1
        lblMsg.Text = ""

        Session("PageMode") = "Add"

    End Sub

    'Added by Zoya @3/03/2016
    Protected Sub ibtnCancel_Click(sender As Object, e As ImageClickEventArgs) Handles ibtnCancel.Click
        OnClearData()
    End Sub
    'End Added by Zoya @3/03/2016

    Private Sub DisableRecordNavigator()

        Dim flag As Boolean
        If Session("ListObj") Is Nothing Then
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

    Protected Sub ibtnView_Click(sender As Object, e As ImageClickEventArgs) Handles ibtnView.Click

        If lblCount.Text <> "" Then
            If CInt(lblCount.Text) > 0 Then
                OnClearData()
                If ibtnNew.Enabled = False Then
                    ibtnSave.Enabled = False
                    ibtnSave.ImageUrl = "images/gsave.png"
                    ibtnSave.ToolTip = "Access Denied"
                End If
            Else
                LoadListObjects()
            End If
        Else
            LoadListObjects()
        End If

    End Sub

    Public Sub LoadListObjects()

        Dim ds As New DataSet
        Dim bobj As New BidangBAL
        Dim eobj As New BidangEn
        Dim recStu As Integer

        If ddlStatus.SelectedValue = -1 Then
            recStu = -1
        Else
            recStu = ddlStatus.SelectedValue
        End If
        eobj.BidangCode = Trim(txtBidangCode.Text)
        eobj.Description = Trim(txtBidangName.Text)
        eobj.Status = ddlStatus.SelectedValue

        Try
            ListObjects = bobj.GetList(eobj)
        Catch ex As Exception
            LogError.Log("Bidang", "LoadListObjects", ex.Message)
        End Try

        Session("ListObj") = ListObjects
        lblCount.Text = ListObjects.Count.ToString()

        If ListObjects.Count <> 0 Then
            DisableRecordNavigator()

            txtRecNo.Text = 1

            PnlView.Visible = False
            PnlAdd.Visible = True
            OnMoveFirst()
            If Session("EditFlag") = True Then

                Session("PageMode") = "Edit"
                txtBidangCode.Enabled = False
                lblMsg.Visible = True
                ibtnSave.Enabled = True
                ibtnSave.ImageUrl = "images/save.png"
            Else
                Session("PageMode") = ""
                ibtnSave.Enabled = False
                ibtnSave.ImageUrl = "images/gsave.png"
            End If
        Else
            txtRecNo.Text = ""
            lblCount.Text = ""
            txtBidangCode.Enabled = True
            'Clear Text Box values
            Session("ListObj") = Nothing
            DisableRecordNavigator()
            txtBidangCode.Text = ""
            txtBidangName.Text = ""
            ddlStatus.SelectedValue = 1

            Session("PageMode") = "Add"

            If DFlag = "Delete" Then
            Else
                lblMsg.Visible = True
                ErrorDescription = "Record did not Exist"
                lblMsg.Text = ErrorDescription
                DFlag = ""
            End If

        End If

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
        If txtRecNo.Text = 0 Then
            txtRecNo.Text = 1
        Else
            If lblCount.Text = 0 Then
                txtRecNo.Text = 0
            Else
                PnlView.Visible = False
                PnlAdd.Visible = True
                Dim obj As BidangEn
                ListObjects = Session("ListObj")
                obj = ListObjects(RecNo)

                txtBidangCode.Text = obj.BidangCode
                txtBidangName.Text = obj.Description

                If obj.Status = True Then
                    ddlStatus.SelectedValue = 1
                Else
                    ddlStatus.SelectedValue = 0
                End If
            End If
        End If
    End Sub

    Protected Sub ibtnFirst_Click(sender As Object, e As ImageClickEventArgs) Handles ibtnFirst.Click
        OnMoveFirst()
    End Sub

    Protected Sub ibtnPrevs_Click(sender As Object, e As ImageClickEventArgs) Handles ibtnPrevs.Click
        OnMovePrevious()
    End Sub

    Protected Sub ibtnNext_Click(sender As Object, e As ImageClickEventArgs) Handles ibtnNext.Click
        OnMoveNext()
    End Sub

    Protected Sub ibtnLast_Click(sender As Object, e As ImageClickEventArgs) Handles ibtnLast.Click
        OnMoveLast()
    End Sub

    Protected Sub ibtnDelete_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ibtnDelete.Click
        OnDelete()
    End Sub

    Private Sub OnDelete()

        If txtBidangCode.Text.Length <> 0 Then
            If lblCount.Text = "" Then lblCount.Text = 0
            If lblCount.Text > 0 Then
                Dim bsobj As New BidangBAL
                Dim eobj As New BidangEn
                Dim RecAff As Integer
                eobj.BidangCode = txtBidangCode.Text

                Try
                    RecAff = bsobj.Delete(eobj)
                    lblMsg.Visible = True
                    ErrorDescription = "Record Deleted Successfully "
                    lblMsg.Text = ErrorDescription
                    DFlag = "Delete"

                Catch ex As Exception
                    lblMsg.Text = ex.Message.ToString()
                    lblMsg.Visible = True
                    LogError.Log("BidangSetup", "OnDelete", ex.Message)
                End Try
                txtBidangCode.Text = ""
                txtBidangName.Text = ""
                ddlStatus.SelectedValue = "1"
                LoadListObjects()
            Else

                ErrorDescription = "Select a Record to Delete"
                lblMsg.Text = ErrorDescription
            End If
        Else
            ErrorDescription = "Select a Record to Delete"
            lblMsg.Text = ErrorDescription
        End If

    End Sub

End Class
