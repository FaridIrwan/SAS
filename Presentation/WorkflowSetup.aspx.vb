Imports HTS.SAS.BusinessObjects
Imports HTS.SAS.DataAccessObjects
Imports HTS.SAS.Entities
Imports System.Data
Imports System.Collections.Generic
Imports System.Linq

Partial Class WorkflowSetup
    Inherits System.Web.UI.Page

    Dim CFlag As String
    Dim DFlag As String
    Private ErrorDescription As String
    Dim ListObjects As List(Of WorkflowSetupEn)

#Region "Page_Load"

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        'retrieved postback from JS - start
        Me.ClientScript.GetPostBackEventReference(Me, String.Empty)
        Dim eventTarget As String, eventArgument As String

        If Me.Request("__EVENTTARGET") Is Nothing Then
            eventTarget = String.Empty
        Else
            eventTarget = Me.Request("__EVENTTARGET")
        End If

        If Me.Request("__EVENTARGUMENT") Is Nothing Then
            eventArgument = String.Empty
        Else
            eventArgument = Me.Request("__EVENTARGUMENT")
        End If

        If eventTarget = "CustomPostBack" Then
            Dim valuePassed As String = eventArgument
            If valuePassed = "Yes" Then
                OnSave()
            ElseIf valuePassed = "No" Then
                OnAdd()
            Else
                Throw New Exception("Failed to save records.")
            End If

        End If
        'retrieved postback from JS - end

        If Not IsPostBack() Then

            ibtnSave.Attributes.Add("onclick", "return validate()")
            ibtnDelete.Attributes.Add("onclick", "return getconfirm()")
            lblMsg.Text = ""
            lblMsg.Visible = False
            Session("PageMode") = ""

            'Loading User Rights
            LoadUserRights()
            FillProcess()

            Session("ListObj") = Nothing
            DisableRecordNavigator()

            'load PageName
            Menuname(CInt(Request.QueryString("Menuid")))

            'hide - start
            PnlAdd.Visible = False
            PnlUser.Visible = False
            PnlView.Visible = True

            ibtnSave.Enabled = False
            ibtnDelete.Enabled = False
            'hide - end

            'load gvView - start
            GetViewData()
            BindViewGrid()
            'load gvView - end

            'load gvUser - start
            GetUserData()
            BindUserGrid()
            'load gvUser - end

        End If

    End Sub

#End Region

#Region "FillProcess"

    Private Sub FillProcess()

        Dim urobj As New UserRightsBAL
        Dim listurEn As New List(Of UserRightsEn)

        Try
            listurEn = urobj.GetMenuByUser(0)

            If listurEn IsNot Nothing Then
                ddlProcess.DataSource = listurEn
                ddlProcess.DataTextField = "PageName"
                ddlProcess.DataValueField = "MenuID"
                ddlProcess.DataBind()
                ddlProcess.Items.Insert(0, New ListItem("--Please Select--", "-1"))
            Else
                ddlProcess.DataTextField = "No Record In Process List"
            End If
        Catch ex As Exception
            LogError.Log("WorkflowSetup", "FillProcess", ex.Message)
            lblMsg.Text = ex.Message
        End Try

    End Sub

#End Region

#Region "ddlProcess_SelectedIndexChanged"

    Protected Sub ddlProcess_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlProcess.SelectedIndexChanged

        If ddlProcess.SelectedValue <> "-1" Then
            PnlAdd.Visible = True
            PnlUser.Visible = True
        End If

    End Sub

#End Region

#Region "GetViewData"

    Protected Sub GetViewData()

        Dim ViewData As List(Of WorkflowSetupEn) = New WorkflowSetupDAL().GetViewList()
        If ViewData.Count <> 0 Then
            Session("ViewData") = ViewData
        Else
            Session("ViewData") = Nothing
        End If
    End Sub

#End Region

#Region "GetUserData"

    Protected Sub GetUserData()

        Dim UserData As List(Of WorkflowSetupEn) = New WorkflowSetupDAL().GetUserList()
        If UserData.Count <> 0 Then
            Session("UserData") = UserData
        Else
            Session("UserData") = Nothing
        End If

    End Sub

#End Region

#Region "BindViewGrid"

    Protected Sub BindViewGrid()

        Try
            If Not Session("ViewData") Is Nothing Then
                gvView.DataSource = Session("ViewData")
                gvView.DataBind()

                For Each gv As GridViewRow In gvView.Rows
                    gv.Cells(1).Text = IIf(gv.Cells(1).Text = True, "Active", "Inactive")
                Next

            Else
                gvView.DataSource = Nothing
                gvView.DataBind()
            End If

        Catch ex As Exception
            lblMsg.Text = ex.Message.ToString()
            LogError.Log("WorkflowSetup", "BindViewGrid", ex.Message)
        End Try
            
    End Sub

#End Region

#Region "BindUserGrid"

    Protected Sub BindUserGrid()

        Try
            If Not Session("UserData") Is Nothing Then
                gvUser.DataSource = Session("UserData")
                gvUser.DataBind()

                gvUser.Columns(1).Visible = False
            Else
                gvUser.DataSource = Nothing
                gvUser.DataBind()
            End If

        Catch ex As Exception
            lblMsg.Text = ex.Message.ToString()
            LogError.Log("WorkflowSetup", "BindUserGrid", ex.Message)
        End Try

    End Sub

#End Region

#Region "gvView_ItemCommand"

    Protected Sub gvView_ItemCommand(source As Object, e As GridViewCommandEventArgs) Handles gvView.RowCommand

        Dim obj As New WorkflowSetupEn, argEn As New WorkflowSetupEn
        Dim _lst As New List(Of WorkflowSetupEn)
        Dim _dal As New WorkflowSetupDAL
        Dim gv As GridViewRow = DirectCast(DirectCast(e.CommandSource, LinkButton).NamingContainer, GridViewRow)
        Dim lb As LinkButton = TryCast(gv.Cells(0).FindControl("imgBtn1"), LinkButton)

        obj.MenuMasterEn = New MenuMasterEn()
        obj.MenuMasterEn.PageName = lb.Text
        obj.Status = IIf(gv.Cells(1).Text = "Active", True, False)
        obj.TotalPreparer = CInt(gv.Cells(2).Text)
        obj.TotalApprover = CInt(gv.Cells(3).Text)

        Try
            argEn = _dal.GetWorkflowSetupDetails(obj)
        Catch ex As Exception
            lblMsg.Text = ex.Message.ToString()
            LogError.Log("WorkflowSetup", "GetWorkflowSetupDetails", ex.Message)
        End Try

        If Not argEn Is Nothing Then

            'hide - start
            PnlView.Visible = False
            PnlAdd.Visible = True
            PnlUser.Visible = True

            ibtnSave.Enabled = True
            ibtnDelete.Enabled = True
            'hide - end

            'ddlprocess map - start
            ddlProcess.SelectedValue = argEn.MenuId
            ddlProcess.Enabled = False
            'ddlprocess map - end

            'ddlstatus map - start
            ddlStatus.SelectedValue = IIf(argEn.Status = True, "1", "0")
            'ddlstatus map - end

            'get preparer - start
            If obj.TotalPreparer = argEn.TotalPreparer AndAlso obj.TotalPreparer > 0 Then
                Try
                    _lst = _dal.GetWorkflowPreparerDetails(argEn)
                Catch ex As Exception
                    lblMsg.Text = ex.Message.ToString()
                    LogError.Log("WorkflowSetup", "GetWorkflowPreparerDetails", ex.Message)
                End Try

                Dim cb As CheckBox, cb_preparer As CheckBox
                For Each _gv As GridViewRow In gvUser.Rows

                    cb = _gv.Cells(0).Controls(1)
                    cb_preparer = _gv.Cells(6).Controls(1)

                    If _lst.Any(Function(x) x.UsersEn.UserName = _gv.Cells(2).Text) AndAlso
                        _lst.Any(Function(x) x.UsersEn.StaffID = _gv.Cells(4).Text) AndAlso
                        _lst.Any(Function(x) x.UsersEn.StaffName = _gv.Cells(3).Text) AndAlso
                        _lst.Any(Function(x) x.UserGroupsEn.UserGroupName = _gv.Cells(5).Text) Then
                        cb.Checked = True
                        cb_preparer.Checked = True
                    End If
                Next

            End If
            'get preparer - end

            'get approver - start
            If obj.TotalApprover = argEn.TotalApprover AndAlso obj.TotalApprover > 0 Then
                Try
                    _lst = _dal.GetWorkflowApproverDetails(argEn)
                Catch ex As Exception
                    lblMsg.Text = ex.Message.ToString()
                    LogError.Log("WorkflowSetup", "GetWorkflowApproverDetails", ex.Message)
                End Try

                Dim cb As CheckBox, cb_approver As CheckBox
                Dim txtLowerLim As TextBox, txtUpperLim As TextBox
                For Each _gv As GridViewRow In gvUser.Rows

                    cb = _gv.Cells(0).Controls(1)
                    cb_approver = _gv.Cells(7).Controls(1)
                    txtLowerLim = CType(_gv.Cells(8).FindControl("txtLowerLim"), TextBox)
                    txtUpperLim = CType(_gv.Cells(8).FindControl("txtUpperLim"), TextBox)

                    If _lst.Any(Function(x) x.UsersEn.UserName = _gv.Cells(2).Text) AndAlso
                        _lst.Any(Function(x) x.UsersEn.StaffID = _gv.Cells(4).Text) AndAlso
                        _lst.Any(Function(x) x.UsersEn.StaffName = _gv.Cells(3).Text) AndAlso
                        _lst.Any(Function(x) x.UserGroupsEn.UserGroupName = _gv.Cells(5).Text) Then

                        cb.Checked = True
                        cb_approver.Checked = True

                        txtLowerLim.Enabled = True
                        txtLowerLim.Text = _lst.Where(Function(x) x.UsersEn.UserName = _gv.Cells(2).Text).Select(Function(y) y.WorkflowApproverEn.LowerLimit).FirstOrDefault()
                        txtUpperLim.Enabled = True
                        txtUpperLim.Text = _lst.Where(Function(x) x.UsersEn.UserName = _gv.Cells(2).Text).Select(Function(y) y.WorkflowApproverEn.UpperLimit).FirstOrDefault()
                    End If

                Next
            End If
            'get approver - end

        End If

    End Sub

#End Region

#Region "gvUser_RowDataBound"

    Protected Sub gvUser_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles gvUser.RowDataBound

        'If e.Row.RowType = DataControlRowType.Header Then
        '    TryCast(e.Row.Cells(0).FindControl("cbSelect"), CheckBox).Attributes.Add("onclick", "javascript:SelectAll('" +
        '                                                                             TryCast(e.Row.FindControl("cbSelect"), CheckBox).ClientID + "')")
        'End If

        'If e.Row.RowType = DataControlRowType.DataRow Then
        '    Dim cbSelect As CheckBox = TryCast(e.Row.Cells(0).FindControl("cbSelect"), CheckBox)
        '    Dim chkPreparer As CheckBox = TryCast(e.Row.Cells(6).FindControl("chkPreparer"), CheckBox)
        '    Dim chkApprover As CheckBox = TryCast(e.Row.Cells(7).FindControl("chkApprover"), CheckBox)
        '    Dim txtLowerLim As TextBox = TryCast(e.Row.Cells(8).FindControl("txtLowerLim"), TextBox)
        '    Dim txtUpperLim As TextBox = TryCast(e.Row.Cells(8).FindControl("txtUpperLim"), TextBox)

        '    If cbSelect.Checked = False Then
        '        chkPreparer.Checked = False
        '        chkApprover.Checked = False
        '        txtLowerLim.Enabled = False
        '        txtLowerLim.Text = ""
        '        txtUpperLim.Enabled = False
        '        txtUpperLim.Text = ""
        '    End If

        'End If

    End Sub

#End Region

#Region "chkApprover_CheckedChanged"

    Protected Sub chkApprover_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)

        Dim cbSelect As CheckBox, chkApprover As CheckBox
        Dim txtLowerLim As TextBox, txtUpperLim As TextBox

        lblMsg.Text = ""

        For Each gv As GridViewRow In gvUser.Rows
            cbSelect = gv.Cells(0).Controls(1)
            chkApprover = gv.Cells(7).Controls(1)
            txtLowerLim = CType(gv.Cells(8).FindControl("txtLowerLim"), TextBox)
            txtUpperLim = CType(gv.Cells(8).FindControl("txtUpperLim"), TextBox)

            If chkApprover.Checked = True Then
                If cbSelect.Checked = False Then
                    chkApprover.Checked = False
                    cbSelect.Focus()
                    lblMsg.Text = "Select Username First To Proceed"
                End If
            End If

            If cbSelect.Checked = True Then

                If chkApprover.Checked = True Then
                    txtLowerLim.Enabled = True
                    txtUpperLim.Enabled = True
                Else
                    txtLowerLim.Enabled = False
                    txtLowerLim.Text = ""
                    txtUpperLim.Enabled = False
                    txtUpperLim.Text = ""
                End If

            End If

        Next

    End Sub

#End Region

#Region "ddlSearchStats_SelectedIndexChanged"

    Protected Sub ddlSearchStats_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlSearchStats.SelectedIndexChanged

        BindViewGrid()

        If ddlSearchStats.SelectedValue = "1" Then
            For Each gv As GridViewRow In gvView.Rows

                If Not gv.Cells(1).Text.Contains("Active") Then
                    gv.Visible = False
                End If

            Next

        ElseIf ddlSearchStats.SelectedValue = "0" Then
            For Each gv As GridViewRow In gvView.Rows

                If Not gv.Cells(1).Text.Contains("Inactive") Then
                    gv.Visible = False
                End If

            Next
        End If

    End Sub

#End Region

#Region "cbSelect_CheckedChanged"

    Protected Sub cbSelect_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)

        Dim cb As CheckBox, cb_preparer As CheckBox, cb_approver As CheckBox
        Dim txtLowerLim As TextBox, txtUpperLim As TextBox

        For Each gv As GridViewRow In gvUser.Rows
            cb = gv.Cells(0).Controls(1)
            cb_preparer = gv.Cells(6).Controls(1)
            cb_approver = gv.Cells(7).Controls(1)
            txtLowerLim = CType(gv.Cells(8).FindControl("txtLowerLim"), TextBox)
            txtUpperLim = CType(gv.Cells(8).FindControl("txtUpperLim"), TextBox)

            If cb.Checked = False Then
                cb_preparer.Checked = False
                cb_approver.Checked = False

                txtLowerLim.Text = ""
                txtLowerLim.Enabled = False
                txtUpperLim.Text = ""
                txtUpperLim.Enabled = False
            End If

        Next

    End Sub

#End Region

#Region "Button Click"

    Protected Sub ibtnSave_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ibtnSave.Click

        Try
            Dim cnt As Integer = 0

            For Each gv As GridViewRow In gvUser.Rows
                Dim cb As CheckBox = gv.Cells(0).Controls(1)

                If cb.Checked = True Then
                    cnt = cnt + 1
                End If
            Next

            If cnt = 0 Then
                Throw New Exception("Please Select At Least One Username")
            End If

            Dim list As List(Of WorkflowSetupEn) = New WorkflowSetupDAL().GetList(New WorkflowSetupEn)
            If list.Any(Function(x) x.MenuId = ddlProcess.SelectedValue) Then

                Page.ClientScript.RegisterStartupScript(Me.GetType, "Warning - Workflow Setup Existed", "<script> WorkflowExisted(); </script>", False)

            Else
                OnSave()
            End If

        Catch ex As Exception
            LogError.Log("WorkflowSetup", "ibtnSave_Click", ex.Message)
            lblMsg.Text = ex.Message
            lblMsg.Visible = True
        End Try

    End Sub

    Protected Sub ibtnNew_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ibtnNew.Click
        OnAdd()
    End Sub

    Protected Sub ibtnCancel_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ibtnCancel.Click
        PnlAdd.Visible = False
        PnlUser.Visible = False
        PnlView.Visible = True

        ibtnSave.Enabled = False
        ibtnDelete.Enabled = False

        OnClearData()
        GetViewData()
        BindViewGrid()
    End Sub

    Protected Sub IbtnView_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ibtnView.Click
        PnlAdd.Visible = False
        PnlUser.Visible = False
        PnlView.Visible = True

        ibtnSave.Enabled = False
        ibtnDelete.Enabled = False

        OnClearData()
        GetViewData()
        BindViewGrid()
    End Sub

    Protected Sub ibtnDelete_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ibtnDelete.Click
        OnDelete()
        GetViewData()
        BindViewGrid()

        PnlAdd.Visible = False
        PnlUser.Visible = False
        PnlView.Visible = True
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

    Protected Sub txtRecNo_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtRecNo.TextChanged
        If lblCount.Text <> Nothing Then
            If CInt(txtRecNo.Text) > CInt(lblCount.Text) Then
                txtRecNo.Text = lblCount.Text
            End If
            FillData(CInt(txtRecNo.Text) - 1)
        Else
            txtRecNo.Text = ""
        End If
    End Sub

    Protected Sub LowerLimit_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        lblMsg.Visible = True
        lblMsg.Text = ""
        Dim cb As CheckBox, cb_approver As CheckBox, txtLowerLim As TextBox

        Try
            For Each gv As GridViewRow In gvUser.Rows
                cb = gv.Cells(0).Controls(1)
                cb_approver = gv.Cells(7).Controls(1)
                txtLowerLim = CType(gv.Cells(8).FindControl("txtLowerLim"), TextBox)

                If cb.Checked = True Then
                    If cb_approver.Checked = True Then
                        If Not String.IsNullOrEmpty(txtLowerLim.Text) Then
                            If Not IsNumeric(txtLowerLim.Text) Then
                                txtLowerLim.Text = ""
                                txtLowerLim.Focus()
                                Throw New Exception("Wrong Format for Lower Limit")
                            End If
                        End If
                    End If
                End If
            Next

        Catch ex As Exception
            lblMsg.Text = ex.Message.ToString()
        End Try

    End Sub

    Protected Sub UpperLimit_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        lblMsg.Visible = True
        lblMsg.Text = ""
        Dim cb As CheckBox, cb_approver As CheckBox, txtUpperLim As TextBox

        Try
            For Each gv As GridViewRow In gvUser.Rows
                cb = gv.Cells(0).Controls(1)
                cb_approver = gv.Cells(7).Controls(1)
                txtUpperLim = CType(gv.Cells(8).FindControl("txtUpperLim"), TextBox)

                If cb.Checked = True Then
                    If cb_approver.Checked = True Then
                        If Not String.IsNullOrEmpty(txtUpperLim.Text) Then
                            If Not IsNumeric(txtUpperLim.Text) Then
                                txtUpperLim.Text = ""
                                txtUpperLim.Focus()
                                Throw New Exception("Wrong Format for Upper Limit")
                            End If
                        End If
                    End If
                End If
            Next

        Catch ex As Exception
            lblMsg.Text = ex.Message.ToString()
        End Try

    End Sub

#End Region

#Region "LoadUserRights"

    Private Sub LoadUserRights()
        Dim obj As New UsersBAL
        Dim eobj As New UserRightsEn

        Try
            eobj = obj.GetUserRights(CInt(Request.QueryString("Menuid")), CInt(Session("UserGroup")))
        Catch ex As Exception
            LogError.Log("WorkflowSetup", "LoadUserRights", ex.Message)
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

#End Region

#Region "On Move Record"

    ''' <summary>
    ''' Method to Move to First Record
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub OnMoveFirst()
        txtRecNo.Text = "1"
        FillData(0)
    End Sub

    ''' <summary>
    ''' Method to Move to Next Record
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub OnMoveNext()
        txtRecNo.Text = CInt(txtRecNo.Text) + 1
        FillData(CInt(txtRecNo.Text) - 1)
    End Sub

    ''' <summary>
    ''' Method to Move to Previous Record
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub OnMovePrevious()
        txtRecNo.Text = CInt(txtRecNo.Text) - 1
        FillData(CInt(txtRecNo.Text) - 1)
    End Sub

    ''' <summary>
    ''' Method to Move to Last Record
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub OnMoveLast()
        txtRecNo.Text = lblCount.Text
        FillData(CInt(txtRecNo.Text) - 1)
    End Sub

    ''' <summary>
    ''' Method to Display the list of WorkflowSetup
    ''' </summary>
    ''' <param name="RecNo">Parameter is RecNo</param>
    ''' <remarks></remarks>
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

        If lblCount.Text = 0 Then
            txtRecNo.Text = 0
        Else
            Dim obj As WorkflowSetupEn
            ListObjects = Session("ListObj")
            obj = ListObjects(RecNo)

            'ddlReviewer.SelectedValue = obj.TotalReview
            'ddlApprover.SelectedValue = obj.TotalApprove

            If obj.Status = True Then
                ddlStatus.SelectedValue = 1
            Else
                ddlStatus.SelectedValue = 0
            End If

        End If

    End Sub

#End Region

#Region "DisableRecordNavigator"

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

#End Region

#Region "LoadListObjects"

    Public Sub LoadListObjects()

        'Dim bobj As New WorkflowSetupBAL
        Dim eobj As New WorkflowSetupEn
        Dim recStu As Integer

        If ddlStatus.SelectedValue = 1 Then
            recStu = 1
        Else
            recStu = ddlStatus.SelectedValue
        End If
        eobj.Status = recStu

        Try
            'ListObjects = bobj.GetWorkflowSetupTypelistall(eobj)
        Catch ex As Exception
            LogError.Log("WorkflowSetup", "LoadListObjects", ex.Message)
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

#End Region

#Region "OnSave"

    Protected Sub OnSave()

        Dim id As Integer = 0
        Dim result As Boolean
        Dim objdal As New WorkflowSetupDAL
        Dim objen As New WorkflowSetupEn
        Dim total_preparer As Integer, total_approver As Integer
        Dim chk As CheckBox, preparer_cb As CheckBox, approver_cb As CheckBox

        For i = 0 To gvUser.Rows.Count - 1
            chk = gvUser.Rows(i).Cells(0).Controls(1)
            preparer_cb = gvUser.Rows(i).Cells(6).Controls(1)
            approver_cb = gvUser.Rows(i).Cells(7).Controls(1)

            If chk.Checked = True Then

                If preparer_cb.Checked = True Then
                    total_preparer = total_preparer + 1
                End If

                If approver_cb.Checked = True Then
                    total_approver = total_approver + 1
                End If

            End If
        Next

        If total_preparer = 0 AndAlso total_approver = 0 Then
            Throw New Exception("Please Select At Least One Preparer/Approver")
        Else
            objen.TotalPreparer = total_preparer
            objen.TotalApprover = total_approver
        End If

        objen.MenuId = CInt(ddlProcess.SelectedValue)
        objen.LastUpdatedBy = Session("User")
        objen.LastUpdatedDtTm = Date.Now.ToString()

        If ddlStatus.SelectedValue = 0 Then
            objen.Status = False
        Else
            objen.Status = True
        End If

        lblMsg.Visible = True

        If Session("PageMode") = "Add" Then

            Try
                id = objdal.Insert(objen)

                If id <> 0 Then
                    Try
                        result = SaveUserMenu(id, objen)
                    Catch ex As Exception
                        Throw ex
                    End Try
                End If

                If (result = True) Then
                    ErrorDescription = "Record Saved Successfully "
                Else
                    ErrorDescription = "Record Failed To Be Inserted. "
                End If

                lblMsg.Text = ErrorDescription

            Catch ex As Exception
                lblMsg.Text = ex.Message.ToString()
                LogError.Log("WorkflowSetup", "OnSave", ex.Message)
            Finally
                result = False
            End Try

        ElseIf Session("PageMode") = "Edit" Then
        End If

    End Sub

#End Region

#Region "SaveUserMenu"

    Protected Function SaveUserMenu(ByVal id As Integer, ByVal obj As WorkflowSetupEn) As Boolean

        Dim result As Boolean = False
        Dim objdal As New WorkflowSetupDAL
        Dim chk As CheckBox, preparer_cb As CheckBox, approver_cb As CheckBox
        Dim txtLowerLim As TextBox, txtUpperLim As TextBox

        For Each gv As GridViewRow In gvUser.Rows
            chk = gv.Cells(0).Controls(1)
            preparer_cb = gv.Cells(6).Controls(1)
            approver_cb = gv.Cells(7).Controls(1)

            If chk.Checked = True Then

                'Preparer Operation - start
                Dim preparerEn As New WorkflowPreparerEn
                preparerEn.WorkflowSetupId = id
                preparerEn.UserId = CInt(gv.Cells(1).Text)
                preparerEn.MenuId = CInt(obj.MenuId)
                preparerEn.LastUpdatedBy = obj.LastUpdatedBy
                preparerEn.LastUpdatedDtTm = obj.LastUpdatedDtTm

                If preparer_cb.Checked = True Then
                    Try
                        result = objdal.InsertPreparer(preparerEn)
                    Catch ex As Exception
                        lblMsg.Text = ex.Message.ToString()
                        LogError.Log("WorkflowSetup", "InsertPreparer", ex.Message)
                    End Try
                Else
                    Try
                        result = objdal.DeletePreparer(preparerEn)
                    Catch ex As Exception
                        lblMsg.Text = ex.Message.ToString()
                        LogError.Log("WorkflowSetup", "DeletePreparer", ex.Message)
                    End Try
                End If
                'Preparer Operation - end

                'Approver Operation - start
                Dim approverEn As New WorkflowApproverEn
                approverEn.WorkflowSetupId = id
                approverEn.UserId = CInt(gv.Cells(1).Text)
                approverEn.MenuId = CInt(obj.MenuId)
                approverEn.LastUpdatedBy = obj.LastUpdatedBy
                approverEn.LastUpdatedDtTm = obj.LastUpdatedDtTm

                'trans limit - start
                txtLowerLim = CType(gv.Cells(8).FindControl("txtLowerLim"), TextBox)
                txtUpperLim = CType(gv.Cells(8).FindControl("txtUpperLim"), TextBox)

                If String.IsNullOrEmpty(txtLowerLim.Text) Then
                    approverEn.LowerLimit = 0
                Else
                    approverEn.LowerLimit = IIf(Trim(CDbl(txtLowerLim.Text)) <> 0, Trim(CDbl(txtLowerLim.Text)), 0)
                End If

                If String.IsNullOrEmpty(txtUpperLim.Text) Then
                    approverEn.UpperLimit = 0
                Else
                    approverEn.UpperLimit = IIf(Trim(CDbl(txtUpperLim.Text)) <> 0, Trim(CDbl(txtUpperLim.Text)), 0)
                End If
                'trans limit - end

                If approver_cb.Checked = True Then
                    Try
                        result = objdal.InsertApprover(approverEn)
                    Catch ex As Exception
                        lblMsg.Text = ex.Message.ToString()
                        LogError.Log("WorkflowSetup", "InsertApprover", ex.Message)
                    End Try
                Else
                    Try
                        result = objdal.DeleteApprover(approverEn)
                    Catch ex As Exception
                        lblMsg.Text = ex.Message.ToString()
                        LogError.Log("WorkflowSetup", "DeleteApprover", ex.Message)
                    End Try
                End If
                'Approver Operation - end

            Else
                'Delete unchecked - start
                Dim WorkflowSetupId As Integer = 0
                Dim UserId As Integer = 0
                Dim MenuId As Integer = 0

                WorkflowSetupId = id
                UserId = CInt(gv.Cells(1).Text)
                MenuId = CInt(obj.MenuId)

                Try
                    result = objdal.DeleteUnchecked(WorkflowSetupId, UserId, MenuId)
                Catch ex As Exception
                    lblMsg.Text = ex.Message.ToString()
                    LogError.Log("WorkflowSetup", "DeleteUnchecked", ex.Message)
                End Try
                'Delete unchecked - end

            End If
        Next

        Return result

    End Function

#End Region

#Region "OnDelete()"

    Protected Sub OnDelete()

        Dim result As Boolean
        Dim bsobj As New WorkflowSetupDAL
        Dim eobj As New WorkflowSetupEn

        lblMsg.Visible = True

        If ddlProcess.SelectedValue <> "-1" Then

            Try
                result = bsobj.Delete(CInt(ddlProcess.SelectedValue))

                If result = True Then
                    lblMsg.Text = "Record Deleted Successfully "
                Else
                    lblMsg.Text = "Record Failed to be Deleted"
                End If

            Catch ex As Exception
                lblMsg.Text = ex.Message.ToString()
                LogError.Log("WorkflowSetup", "OnDelete", ex.Message)
            End Try

        End If

    End Sub

#End Region

#Region "Menuname"

    Private Sub Menuname(ByVal MenuId As Integer)
        Dim eobj As New MenuEn
        Dim bobj As New MenuBAL
        eobj.MenuId = MenuId
        Try
            eobj = bobj.GetMenus(eobj)
        Catch ex As Exception
            LogError.Log("WorkflowSetup", "Menuname", ex.Message)
        End Try
        lblMenuName.Text = eobj.MenuName
    End Sub

#End Region

#Region "OnAdd"

    Private Sub OnAdd()
        Session("PageMode") = "Add"
        PnlView.Visible = False
        PnlAdd.Visible = True
        PnlUser.Visible = False

        ibtnSave.Enabled = True
        ibtnDelete.Enabled = True

        OnClearData()
    End Sub

#End Region

#Region "OnClearData"

    Private Sub OnClearData()

        Session("ListObj") = Nothing
        lblMsg.Text = ""
        DisableRecordNavigator()
        ddlProcess.SelectedIndex = -1
        ddlProcess.Enabled = True
        ddlStatus.SelectedValue = "1"
        ddlSearchStats.SelectedValue = "1"

        For Each gv As GridViewRow In gvUser.Rows
            Dim cb As CheckBox = gv.Cells(0).Controls(1)
            Dim cb_preparer As CheckBox = gv.Cells(6).Controls(1)
            Dim cb_approver As CheckBox = gv.Cells(7).Controls(1)
            Dim txtLowerLim As TextBox = CType(gv.Cells(8).FindControl("txtLowerLim"), TextBox)
            Dim txtUpperLim As TextBox = CType(gv.Cells(8).FindControl("txtUpperLim"), TextBox)

            cb.Checked = False
            cb_preparer.Checked = False
            cb_approver.Checked = False
            txtLowerLim.Text = ""
            txtLowerLim.Enabled = False
            txtUpperLim.Text = ""
            txtUpperLim.Enabled = False
        Next

        Session("PageMode") = "Add"

    End Sub

#End Region

End Class
