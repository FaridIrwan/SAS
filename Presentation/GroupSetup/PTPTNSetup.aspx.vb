Imports System.Collections.Generic
Imports HTS.SAS.Entities
Imports HTS.SAS.BusinessObjects
Imports HTS.SAS.DataAccessObjects
Imports System.Data

Partial Class PTPTNSetup
    Inherits System.Web.UI.Page
    Dim CFlag As String
    Dim DFlag As String
    Dim ListObjects As List(Of PTPTNSetupEn)
    Private ErrorDescription As String

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        lblMsg.Visible = False
        If Not IsPostBack() Then
            ibtnSave.Attributes.Add("onclick", "return validate()")
            ibtnDelete.Attributes.Add("onclick", "return getconfirm()")
            txtRecNo.Attributes.Add("OnKeyUp", "return geterr()")
            Session("PageMode") = ""
            LoadUserRights()

            Session("ListObj") = Nothing
            DisableRecordNavigator()
            Menuname(CInt(Request.QueryString("Menuid")))
        End If
    End Sub

    Private Sub LoadUserRights()
        Dim obj As New UsersBAL
        Dim eobj As New UserRightsEn

        Try
            eobj = obj.GetUserRights(CInt(Request.QueryString("Menuid")), CInt(Session("UserGroup")))
        Catch ex As Exception
            LogError.Log("PTPTN Setup", "LoadUserRights", ex.Message)
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

    Private Sub OnAdd()
        Session("PageMode") = "Add"
        OnClearData()
    End Sub

    Private Sub OnEdit()
        Session("PageMode") = "Edit"
    End Sub

    Private Sub OnClearData()
        txtMinBal.Enabled = True
        Session("ListObj") = Nothing
        DisableRecordNavigator()
        txtMinBal.Text = ""

        Session("PageMode") = "Add"
    End Sub

    Private Sub OnSave()
        Dim bsobj As New PTPTNSetupDAL
        Dim eobj As New PTPTNSetupEn
        Dim RecAff As Integer

        eobj.id = 1
        eobj.min_balance = CDec(Trim(txtMinBal.Text))

        lblMsg.Visible = True
        If Session("PageMode") = "Add" Then
            Try
                RecAff = bsobj.Operation(eobj)

                If RecAff = -1 Then
                    ErrorDescription = "Record Saved Successfully "
                    lblMsg.Text = ErrorDescription
                ElseIf RecAff = 0 Then
                    ErrorDescription = "Record already exist "
                    lblMsg.Text = ErrorDescription
                End If

            Catch ex As Exception
                lblMsg.Text = ex.Message.ToString
                LogError.Log("PTPTN Setup", "OnSave", ex.Message)
            End Try
        ElseIf Session("PageMode") = "Edit" Then
            Try
                RecAff = bsobj.Operation(eobj)

                ListObjects = Session("ListObj")
                ListObjects(CInt(txtRecNo.Text) - 1) = eobj
                Session("ListObj") = ListObjects
                ErrorDescription = "Record is Updated Successfully "
                lblMsg.Text = ErrorDescription

            Catch ex As Exception
                lblMsg.Text = ex.Message.ToString
                LogError.Log("PTPTN Setup", "OnSave", ex.Message)
            End Try
        End If

        Session("PageMode") = ""

    End Sub

    Private Sub OnReset()
        lblMsg.Visible = True
        If txtMinBal.Text <> "" Then
            If lblCount.Text = "" Then lblCount.Text = 0
            If lblCount.Text > 0 Then
                Dim bsobj As New PTPTNSetupDAL
                Dim eobj As New PTPTNSetupEn

                Dim RecAff As Integer
                lblMsg.Visible = True
                eobj.id = 1
                eobj.min_balance = 0

                Try
                    RecAff = bsobj.Reset(eobj)
                    ErrorDescription = "Record Reset Successfully "
                    lblMsg.Text = ErrorDescription
                Catch ex As Exception
                    lblMsg.Text = ex.Message.ToString()
                    LogError.Log("PTPTN Setup", "OnReset", ex.Message)
                End Try

                DFlag = "Delete"
                LoadListObjects()
            Else
                ErrorDescription = "Operation failed"
                lblMsg.Text = ErrorDescription
            End If
        Else
            ErrorDescription = "Operation failed"
            lblMsg.Text = ErrorDescription
        End If

    End Sub

    Public Sub LoadListObjects()
        Dim bobj As New PTPTNSetupDAL
        Dim eob As New PTPTNSetupEn

        eob.id = 1
        If txtMinBal.Text = "" Then
            eob.min_balance = 0.0
        Else
            eob.min_balance = CDec(Trim(txtMinBal.Text))
        End If

        Try
            ListObjects = bobj.GetList(eob)
        Catch ex As Exception
            LogError.Log("PTPTN Setup", "LoadListObjects", ex.Message)
        End Try

        Session("ListObj") = ListObjects
        lblCount.Text = ListObjects.Count.ToString()

        If ListObjects.Count <> 0 Then
            DisableRecordNavigator()
            If lblCount.Text = 0 Then
                txtRecNo.Text = 0
            Else
                txtRecNo.Text = 1
            End If

            OnMoveFirst()

            If Session("EditFlag") = True Then
                Session("PageMode") = "Edit"
            Else
                Session("PageMode") = ""
                ibtnSave.Enabled = False
                ibtnSave.ImageUrl = "images/gsave.png"
            End If
        Else
            txtRecNo.Text = ""
            lblCount.Text = ""
            OnClearData()

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
        FillData(CInt(txtRecNo.Text) - 1)
    End Sub

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

    Private Sub FillData(ByVal RecNo As Integer)
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
        If lblCount.Text = 0 Then
            txtRecNo.Text = 0
        Else
            Dim obj As PTPTNSetupEn
            ListObjects = Session("ListObj")
            obj = ListObjects(RecNo)

            txtMinBal.Text = obj.min_balance

        End If
    End Sub

    Private Sub Menuname(ByVal MenuId As Integer)
        Dim eobj As New MenuEn
        Dim bobj As New MenuBAL
        eobj.MenuId = MenuId
        Try
            eobj = bobj.GetMenus(eobj)
        Catch ex As Exception
            LogError.Log("PTPTN Setup", "Menuname", ex.Message)
        End Try
        lblMenuName.Text = eobj.MenuName
    End Sub

    Protected Sub ibtnView_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ibtnView.Click
        If lblCount.Text <> "" Then
            If CInt(lblCount.Text) > 0 Then
                OnClearData()
            Else
                LoadListObjects()
            End If
        Else
            LoadListObjects()
        End If
    End Sub

    Protected Sub ibtnSave_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ibtnSave.Click
        OnSave()
    End Sub

    Protected Sub ibtnDelete_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ibtnDelete.Click
        OnReset()
    End Sub

    Protected Sub txtRecNo_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        If lblCount.Text <> Nothing Then
            If CInt(txtRecNo.Text) > CInt(lblCount.Text) Then
                txtRecNo.Text = lblCount.Text
            End If
            FillData(CInt(txtRecNo.Text) - 1)
        Else
            txtRecNo.Text = ""
        End If
    End Sub

    Protected Sub ibtnPrevs_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ibtnPrevs.Click
        OnMovePrevious()
    End Sub

    Protected Sub ibtnNext_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ibtnNext.Click
        OnMoveNext()
    End Sub

    Protected Sub ibtnLast_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ibtnLast.Click
        OnMoveLast()
    End Sub

    Protected Sub ibtnFirst_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ibtnFirst.Click
        OnMoveFirst()
    End Sub

    Protected Sub ibtnNew_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ibtnNew.Click
        OnAdd()
    End Sub

    Protected Sub ibtnCancel_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ibtnCancel.Click
        OnClearData()
    End Sub
End Class
