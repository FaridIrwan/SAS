Imports System.Collections.Generic
Imports HTS.SAS.BusinessObjects
Imports HTS.SAS.Entities
Imports System.Data

Partial Class AutoNumber
    Inherits System.Web.UI.Page
    Dim CFlag As String
    Dim DFlag As String
    Dim ListObjects As List(Of AutoNumberEn)
    Private ErrorDescription As String
    ''Private LogErrors As LogError


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack() Then
            'Adding validation for save button
            ibtnSave.Attributes.Add("onclick", "return validate()")
            ibtnDelete.Attributes.Add("onclick", "return getconfirm()")
            txtRecNo.Attributes.Add("OnKeyup", "return geterr()")
            'Loading User Rights
            'Session("PageMode") = ""
            Session("PageMode") = "Add"
            LoadUserRights()
            'While loading the page make the CFlag as null
            '

            'while loading list object make it nothing
            Session("ListObj_autoNumber") = Nothing
            DisableRecordNavigator()
            Menuname(CInt(Request.QueryString("Menuid")))
        End If
        lblMsg.Visible = False

    End Sub
    Private Sub LoadUserRights()
        Dim obj As New UsersBAL
        Dim eobj As UserRightsEn

        'eobj = obj.GetUserRights(5, 1)
        Try
            eobj = obj.GetUserRights(CInt(Request.QueryString("Menuid")), 1)
        Catch ex As Exception
            LogError.Log("AutoNumber", "LoadUserRights", ex.Message)
        End Try

        'Rights for Add

        If eobj.IsAdd = True Then
            ibtnSave.Enabled = True
            OnAdd()
        Else
            ibtnSave.Enabled = False
            ibtnSave.ImageUrl = "images/gsave.gif"
            ibtnSave.ToolTip = "Access Denied"
        End If
        'Rights for Edit
        If eobj.IsEdit = True Then
            'ibtnSave.ToolTip = "Access Denied"
            Session("EditFlag") = True
        Else
            Session("EditFlag") = False
        End If
        'Rights for View
        ibtnView.Enabled = eobj.IsView
        If eobj.IsView = True Then
            ibtnView.ImageUrl = "images/find.gif"
            ibtnView.Enabled = True
        Else
            ibtnView.ImageUrl = "images/gfind.gif"
            ibtnView.ToolTip = "Access Denied"
        End If
        'Rights for Print
        ibtnPrint.Enabled = eobj.IsPrint
        If eobj.IsPrint = True Then
            ibtnPrint.Enabled = True
            ibtnPrint.ImageUrl = "images/print.gif"
            ibtnPrint.ToolTip = "Print"
        Else
            ibtnPrint.Enabled = False
            ibtnPrint.ImageUrl = "images/gprint.gif"
            ibtnPrint.ToolTip = "Access Denied"
        End If
        'Checking Default mode
        If eobj.IsAddModeDefault = True Then
            pnlView.Visible = True
            PnlAdd.Visible = True

        Else
            PnlAdd.Visible = False
            pnlView.Visible = True
        End If
        If eobj.IsOthers = True Then
            ibtnOthers.Enabled = True
            ibtnOthers.ImageUrl = "images/others.gif"
            ibtnOthers.ToolTip = "Others"
        Else
            ibtnOthers.Enabled = False
            ibtnOthers.ImageUrl = "images/gothers.gif"
            ibtnOthers.ToolTip = "Access Denied"
        End If
        If eobj.IsPost = True Then
            ibtnPosting.Enabled = True
            ibtnPosting.ImageUrl = "images/posting.gif"
            ibtnPosting.ToolTip = "Posting"
        Else
            ibtnPosting.Enabled = False
            ibtnPosting.ImageUrl = "images/gposting.gif"
            ibtnPosting.ToolTip = "Access Denied"
        End If
    End Sub
    Private Sub OnAdd()
        Session("PageMode") = "Add"
        ibtnSave.Enabled = True
        ibtnSave.ImageUrl = "images/save.gif"
        pnlAdd.Visible = True
        'Clear Text Box values
        OnClearData()

    End Sub
    Private Sub OnClearData()
        txtANDesc.Enabled = True
        Session("ListObj_AutoNumber") = Nothing
        DisableRecordNavigator()
        'Clear Text Box values
        txtANDesc.Text = ""
        txtPrefix.Text = ""
        txtNoDigits.Text = ""
        txtStartNo.Text = ""
        pnlView.Visible = False
        Session("PageMode") = "Add"
    End Sub
    Private Sub DisableRecordNavigator()
        Dim flag As Boolean
        If Session("ListObj_AutoNumber") Is Nothing Then
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
            ibtnFirst.ImageUrl = "images/gnew_first.gif"
            ibtnLast.ImageUrl = "images/gnew_last.gif"
            ibtnPrevs.ImageUrl = "images/gnew_Prev.gif"
            ibtnNext.ImageUrl = "images/gnew_next.gif"
        Else
            ibtnFirst.ImageUrl = "images/new_last.gif"
            ibtnLast.ImageUrl = "images/new_first.gif"
            ibtnPrevs.ImageUrl = "images/new_Prev.gif"
            ibtnNext.ImageUrl = "images/new_next.gif"
        End If
    End Sub
    Private Sub Menuname(ByVal MenuId As Integer)
        Dim eobj As New MenuEn
        Dim bobj As New MenuBAL
        eobj.MenuId = MenuId
        Try
            eobj = bobj.GetMenus(eobj)
        Catch ex As Exception
            LogError.Log("AutoNumber", "Menuname", ex.Message)
        End Try

        lblMenuName.Text = eobj.MenuName
    End Sub
    Private Sub SpaceValidation()
        If Trim(txtANDesc.Text).Length = 0 Then

            txtANDesc.Text = Trim(txtANDesc.Text)
            lblMsg.Text = "Enter valid Description "
            lblMsg.Visible = True
            txtANDesc.Focus()
            Exit Sub
        End If
        If Trim(txtNoDigits.Text).Length = 0 Then

            txtNoDigits.Text = Trim(txtNoDigits.Text)
            lblMsg.Text = "Enter valid No of digits "
            lblMsg.Visible = True
            txtNoDigits.Focus()
            Exit Sub
        End If
        If Trim(txtPrefix.Text).Length = 0 Then
            lblMsg.Text = "Enter valid prefix"
            lblMsg.Visible = True
            txtPrefix.Text = Trim(txtPrefix.Text)

            txtPrefix.Focus()
            Exit Sub
        End If
        If Trim(txtStartNo.Text).Length = 0 Then
            lblMsg.Text = "Enter valid prefix"
            lblMsg.Visible = True
            txtStartNo.Text = Trim(txtStartNo.Text)

            txtStartNo.Focus()
            Exit Sub
        End If
        OnSave()
    End Sub
    Private Sub OnSave()
        lblMsg.Visible = False
        Dim bsobj As New AutoNumberBAL
        Dim eobj As New AutoNumberEn
        Dim RecAff As Integer
        eobj.SAAN_Des = Trim(txtANDesc.Text)
        eobj.SAAN_NoDigit = Trim(txtNoDigits.Text)
        eobj.SAAN_Prefix = Trim(txtPrefix.Text)
        eobj.SAAN_StartNo = Trim(txtStartNo.Text)
        lblMsg.Visible = True
        If Session("PageMode") = "Add" Then
            Try

                eobj.SAAN_CurNo = Trim(txtStartNo.Text)
                RecAff = bsobj.Insert(eobj)
                'User Defined Message
                ErrorDescription = "Record Saved Successfully "
                lblMsg.Text = ErrorDescription
            Catch ex As Exception
                lblMsg.Text = ex.Message.ToString()
                LogError.Log("AutoNumber", "OnSave", ex.Message)
            End Try
        ElseIf Session("PageMode") = "Edit" Then
            Try
                eobj.SAAN_Code = Trim(txtANCode.Text)
                eobj.SAAN_CurNo = Trim(txtStartNo.Text)
                RecAff = bsobj.UpdateAutoNumber(eobj)
                ListObjects = Session("ListObj_AutoNumber")
                ListObjects(CInt(txtRecNo.Text) - 1) = eobj
                Session("ListObj_AutoNumber") = ListObjects
                'User Defined Message

                ErrorDescription = "Record is updated Successfully "
                lblMsg.Text = ErrorDescription
            Catch ex As Exception
                lblMsg.Text = ex.Message.ToString()
                LogError.Log("AutoNumber", "OnSave", ex.Message)
            End Try
        End If

    End Sub
    Private Sub OnDelete()
        lblMsg.Visible = True
        If txtANDesc.Text <> "" Then
            If lblCount.Text = "" Then lblCount.Text = 0
            If lblCount.Text > 0 Then
                Dim bsobj As New AutoNumberBAL
                Dim eobj As New AutoNumberEn
                Dim RecAff As Integer
                lblMsg.Visible = True
                eobj.SAAN_Des = txtANDesc.Text
                Try

                    RecAff = bsobj.Delete(eobj)
                    ListObjects = Session("ListObj_AutoNumber")
                    ListObjects.RemoveAt(CInt(txtRecNo.Text) - 1)
                    lblCount.Text = lblCount.Text - 1
                    Session("ListObj_AutoNumber") = ListObjects
                    lblMsg.Visible = True
                    ErrorDescription = "Record Deleted Successfully "
                    lblMsg.Text = ErrorDescription
                Catch ex As Exception

                    lblMsg.Text = ex.Message.ToString()
                    LogError.Log("AutoNumber", "OnDelete", ex.Message)
                End Try
                txtANDesc.Text = ""
                txtNoDigits.Text = ""
                txtPrefix.Text = ""
                txtStartNo.Text = ""
                DFlag = "Delete"
                LoadListObjects()
            Else
                ErrorDescription = "Record not Seleted"
                lblMsg.Text = ErrorDescription
            End If
        Else
            ErrorDescription = "Record not Seleted"
            lblMsg.Text = ErrorDescription
        End If
    End Sub
    Public Sub LoadListObjects()
        Dim ds As New DataSet
        Dim bobj As New AutoNumberBAL
        Dim eobj As New AutoNumberEn

        If txtANDesc.Text = "" Then txtANDesc.Text = "*"
        eobj.SAAN_Des = Trim(txtANDesc.Text)
        eobj.SAAN_NoDigit = 12
        eobj.SAAN_Prefix = Trim(txtPrefix.Text)
        eobj.SAAN_StartNo = 1
        ListObjects = bobj.GetAutoNumberList(eobj)
        'ListObjects = bobj.GetBankProfileList(Trim(txtBankCode.Text), Trim(txtBankName.Text), Trim(txtAccountCode.Text), recStu)
        Session("ListObj_AutoNumber") = ListObjects
        lblCount.Text = ListObjects.Count.ToString()
        If ListObjects.Count <> 0 Then
            DisableRecordNavigator()
            txtRecNo.Text = "1"
            pnlView.Visible = False
            pnlAdd.Visible = True
            OnMoveFirst()
            If Session("EditFlag") = True Then
                Session("PageMode") = "Edit"
                txtANDesc.Enabled = False
            Else
                Session("PageMode") = ""
                ibtnSave.Enabled = False
                ibtnSave.ImageUrl = "images/gsave.gif"
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
        FillData(CInt(lblCount.Text) - 1)
    End Sub
    Private Sub FillData(ByVal RecNo As Integer)
        lblMsg.Visible = False
        'Conditions for Button Enable & Disable
        If txtRecNo.Text = lblCount.Text Then
            ibtnNext.Enabled = False
            ibtnNext.ImageUrl = "images/gnew_next.gif"
            ibtnLast.Enabled = False
            ibtnLast.ImageUrl = "images/gnew_last.gif"
        Else
            ibtnNext.Enabled = True
            ibtnNext.ImageUrl = "images/new_next.gif"
            ibtnLast.Enabled = True
            ibtnLast.ImageUrl = "images/new_last.gif"
        End If
        If txtRecNo.Text = "1" Then
            ibtnPrevs.Enabled = False
            ibtnPrevs.ImageUrl = "images/gnew_Prev.gif"
            ibtnFirst.Enabled = False
            ibtnFirst.ImageUrl = "images/gnew_first.gif"
        Else
            ibtnPrevs.Enabled = True
            ibtnPrevs.ImageUrl = "images/new_prev.gif"
            ibtnFirst.Enabled = True
            ibtnFirst.ImageUrl = "images/new_first.gif"
        End If
        If txtRecNo.Text = 0 Then
            txtRecNo.Text = 1
        Else
            If lblCount.Text = 0 Then
                txtRecNo.Text = 0
            Else
                pnlView.Visible = False
                pnlAdd.Visible = True
                Dim obj As AutoNumberEn
                ListObjects = Session("ListObj_AutoNumber")
                obj = ListObjects(RecNo)
                txtANCode.Text = obj.SAAN_Code
                txtANDesc.Text = obj.SAAN_Des
                txtNoDigits.Text = obj.SAAN_NoDigit
                txtPrefix.Text = obj.SAAN_Prefix
                txtStartNo.Text = obj.SAAN_StartNo
                txtCurNo.Text = obj.SAAN_CurNo
            End If
        End If
    End Sub
    Protected Sub ibtnNew_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ibtnNew.Click
        OnAdd()
    End Sub

    Protected Sub ibtnSave_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ibtnSave.Click
        SpaceValidation()
    End Sub

    Protected Sub ibtnDelete_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ibtnDelete.Click
        OnDelete()
    End Sub

    Protected Sub ibtnView_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)
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

    Protected Sub ibtnFirst_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ibtnFirst.Click
        OnMoveFirst()
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

    Protected Sub ibtnCancel_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ibtnCancel.Click
        OnClearData()
    End Sub

    End Class
