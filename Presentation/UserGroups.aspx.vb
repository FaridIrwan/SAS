Imports HTS.SAS.BusinessObjects
Imports HTS.SAS.Entities
Imports System.Data
Imports System.Collections.Generic
Imports System.Linq

Partial Class UserGroups
    Inherits System.Web.UI.Page
    Dim CFlag As String
    Dim DFlag As String
    Dim ListObjects As List(Of UserGroupsEn)
    Private ErrorDescription As String

    Dim objBE As New BusinessEntities.DepartmentEn
    Dim objSQLQuery As New SQLPowerQueryManager.PowerQueryManager.DepartmentDL
    'Dim GlobalSQLConnString As String = ConfigurationManager.ConnectionStrings("SASNEWConnectionString").ToString
    Private GlobalSQLConnString As String = SQLPowerQueryManager.Helper.GetConnectionString()

    Dim DSReturn As New DataSet
    Dim strRetrunErrorMsg As String = String.Empty
    Dim blnReturnValue As Boolean
    Dim strMode As String

    'Private LogErrors As LogError
    Protected Sub IbtnView_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ibtnView.Click
        'LoadGrid()
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
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack() Then
            'Adding validation for save button
            ibtnSave.Attributes.Add("onclick", "return validate()")
            ibtnDelete.Attributes.Add("onclick", "return getconfirm()")
            txtRecNo.Attributes.Add("OnKeyUp", "return geterr()")
            'While loading the page make the CFlag as null
            Session("PageMode") = ""
            'Loading User Rights
            LoadUserRights()
            FillDllDepartment()
            'while loading list object make it nothing
            Session("ListObj") = Nothing
            DisableRecordNavigator()
            'load PageName
            Menuname(CInt(Request.QueryString("Menuid")))
        End If
        lblMsg.Text = ""
        lblMsg.Visible = False
    End Sub

    Private Sub FillDllDepartment()
        objBE.SQLCase = 4
        blnReturnValue = objSQLQuery.RetriveData(objBE, strRetrunErrorMsg, GlobalSQLConnString, DSReturn)

        If blnReturnValue Then
            ddlDepartment.DataSource = DSReturn
            ddlDepartment.DataTextField = "Department"
            ddlDepartment.DataValueField = "DepartmentID"
            ddlDepartment.DataBind()
            ddlDepartment.Items.Insert(0, New ListItem("--Please Select--", "0"))
        Else
            LogError.Log("User Group", "FillDllDepartment", strRetrunErrorMsg)
            lblMsg.Text = strRetrunErrorMsg
        End If
    End Sub

    Protected Sub ibtnSave_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ibtnSave.Click
        SpaceValidation()
    End Sub
    Protected Sub ibtnNew_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ibtnNew.Click
        OnAdd()
    End Sub
    Protected Sub ibtnDelete_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ibtnDelete.Click
        OnDelete()
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

    Protected Sub txtRecNo_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtRecNo.TextChanged
        If Trim(txtRecNo.Text).Length = 0 Then
            txtRecNo.Text = 0
            If lblCount.Text <> Nothing Then
                If CInt(txtRecNo.Text) > CInt(lblCount.Text) Then
                    txtRecNo.Text = lblCount.Text
                End If
                FillData(CInt(txtRecNo.Text) - 1)
            Else
                txtRecNo.Text = ""
            End If
        Else
            If lblCount.Text <> Nothing Then
                If CInt(txtRecNo.Text) > CInt(lblCount.Text) Then
                    txtRecNo.Text = lblCount.Text
                End If
                FillData(CInt(txtRecNo.Text) - 1)
            Else
                txtRecNo.Text = ""
            End If
        End If
    End Sub

#Region "Methods"
    ''' <summary>
    ''' Method to Validate Before Save
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub SpaceValidation()
        If Trim(txtCategoryName.Text).Length = 0 Then

            txtCategoryName.Text = Trim(txtCategoryName.Text)
            lblMsg.Text = "Enter valid Group Description"
            lblMsg.Visible = True
            txtCategoryName.Focus()
            Exit Sub
        End If

        OnSave()
    End Sub
    ''' <summary>
    ''' Method to Load UserRights
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub LoadUserRights()
        Dim obj As New UsersBAL
        Dim eobj As New UserRightsEn
        Try
            eobj = obj.GetUserRights(CInt(Request.QueryString("Menuid")), CInt(Session("UserGroup")))
        Catch ex As Exception
            LogError.Log("UserGroups", "LoadUserRights", ex.Message)
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
        'If eobj.IsOthers = True Then
        '    ibtnOthers.Enabled = True
        '    ibtnOthers.ImageUrl = "images/others.png"
        '    ibtnOthers.ToolTip = "Others"
        'Else
        '    ibtnOthers.Enabled = False
        '    ibtnOthers.ImageUrl = "images/gothers.png"
        '    ibtnOthers.ToolTip = "Access Denied"
        'End If
        'If eobj.IsPost = True Then
        '    ibtnPosting.Enabled = True
        '    ibtnPosting.ImageUrl = "images/posting.png"
        '    ibtnPosting.ToolTip = "Posting"
        'Else
        '    ibtnPosting.Enabled = False
        '    ibtnPosting.ImageUrl = "images/gposting.png"
        '    ibtnPosting.ToolTip = "Access Denied"
        'End If
    End Sub
    ''' <summary>
    ''' Method to Clear the Field Values
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub OnClearData()
        txtCategoryCode.Enabled = True
        txtCategoryName.ReadOnly = False
        txtCategoryName.Enabled = True
        Session("ListObj") = Nothing
        DisableRecordNavigator()
        txtCategoryCode.Text = ""
        txtCategoryName.Text = ""
        ddlStatus.Enabled = True
        ddlStatus.SelectedValue = 1
        ddlDepartment.SelectedIndex = -1
        lblMsg.Text = ""
        Session("PageMode") = "Add"
    End Sub
    ''' <summary>
    ''' Method to Load Fields in New Mode
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub OnAdd()
        Session("PageMode") = "Add"
        PnlAdd.Visible = True
        OnClearData()
        dgView.SelectedIndex = -1
        PnlView.Visible = False
    End Sub
    ''' <summary>
    ''' Method to Change to Edit Mode
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub OnEdit()

        Session("PageMode") = "Edit"
        txtCategoryCode.Enabled = False

    End Sub
    ''' <summary>
    ''' Method to Save and Update UserGroups
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub OnSave()
        Dim bsobj As New UserGroupsBAL
        Dim eobj As New UserGroupsEn
        Dim RecAff As Boolean
        eobj.UserGroupName = Trim(txtCategoryCode.Text)
        eobj.Description = Trim(txtCategoryName.Text)
        eobj.DepartmentID = ddlDepartment.SelectedValue

        If ddlStatus.SelectedValue = 0 Then
            eobj.Status = False
        Else
            eobj.Status = True
        End If
        eobj.LastUpdatedBy = Session("User")
        eobj.LastUpdatedDtTm = Date.Now.ToString()
        lblMsg.Visible = True
        If Session("PageMode") = "Add" Then
            Try
                RecAff = bsobj.Insert(eobj)
                If (RecAff = True) Then
                    ErrorDescription = "Record Saved Successfully "
                Else
                    ErrorDescription = "Record Failed To Be Inserted. "
                End If
                lblMsg.Text = ErrorDescription
            Catch ex As Exception
                lblMsg.Text = ex.Message.ToString()
                LogError.Log("UserGroups", "OnSave", ex.Message)
            End Try
        ElseIf Session("PageMode") = "Edit" Then
            Try
                eobj.UserGroupId = userGroupID.Text
                RecAff = bsobj.Update(eobj)
                ListObjects = Session("ListObj")
                Session("ListObj") = ListObjects
                ErrorDescription = "Record Updated Successfully "
                lblMsg.Text = ErrorDescription
            Catch ex As Exception

                lblMsg.Text = ex.Message.ToString()
                LogError.Log("UserGroups", "OnSave", ex.Message)
            End Try
        End If

    End Sub
    ''' <summary>
    ''' Method to Delete UserGroups
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub OnDelete()
        If txtCategoryCode.Text <> "" Then
            If lblCount.Text = "" Then lblCount.Text = 0
            If lblCount.Text > 0 Then
                Dim bsobj As New UserGroupsBAL
                Dim eobj As New UserGroupsEn
                Dim RecAff As Integer
                eobj.UserGroupId = userGroupID.Text

                Try
                    RecAff = bsobj.Delete(eobj)
                    'ListObjects = Session("ListObj")
                    'ListObjects.RemoveAt(CInt(txtRecNo.Text) - 1)
                    'lblCount.Text = lblCount.Text - 1
                    'Session("ListObj") = ListObjects
                    OnClearData()
                    ErrorDescription = "Record Deleted Successfully "
                    lblMsg.Text = ErrorDescription
                    lblMsg.Visible = True

                Catch ex As Exception
                    lblMsg.Text = ex.Message.ToString()
                    LogError.Log("UserGroups", "OnDelete", ex.Message)
                End Try

                txtCategoryCode.Text = ""
                txtCategoryName.Text = ""
                ddlStatus.SelectedValue = -1
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
    ''' <summary>
    ''' Method to get the List Of UserGroups
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub LoadListObjects()
        Dim ds As New DataSet
        Dim bobj As New UserGroupsBAL
        Dim eobj As New UserGroupsEn
        Dim recStu As Integer
        If ddlStatus.SelectedValue = 1 Then
            recStu = 1
        Else
            recStu = ddlStatus.SelectedValue
        End If

        eobj.DepartmentID = ddlDepartment.SelectedValue
        eobj.UserGroupName = txtCategoryCode.Text
        eobj.Description = txtCategoryName.Text
        eobj.Status = recStu

        Try
            ListObjects = bobj.GetUserGroupsTypelist(eobj)
        Catch ex As Exception
            LogError.Log("UserGroups", "LoadListObjects", ex.Message)
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
                txtCategoryCode.Enabled = False
                lblMsg.Visible = True
                'ibtnSave.Enabled = True
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
        FillData(CInt(lblCount.Text) - 1)
    End Sub
    ''' <summary>
    ''' Method to Enable or Disable Navigation Buttons
    ''' </summary>
    ''' <remarks></remarks>
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
    ''' <summary>
    ''' Method to Fill the Field Values
    ''' </summary>
    ''' <param name="RecNo"></param>
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
            PnlView.Visible = False
            PnlAdd.Visible = True
            Dim obj As UserGroupsEn
            ListObjects = Session("ListObj")
            obj = ListObjects(RecNo)

            'Editted by Zoya @3/03/2016

            'If (ddlDepartment.SelectedValue = "0") Then
            '    lblMsg.Text = "Please select department"
            'Else
            '    ddlDepartment.SelectedValue = obj.DepartmentID
            '    ddlDepartment.SelectedValue = obj.DepartmentID
            '    userGroupID.Text = obj.UserGroupId
            '    txtCategoryCode.Text = obj.UserGroupName
            '    txtCategoryName.Text = obj.Description
            'End If


            If (DFlag <> "Delete" And ddlDepartment.SelectedValue = "0") Then
                lblMsg.Text = "Please select department"

            ElseIf DFlag <> "Delete" Then
                ddlDepartment.SelectedValue = obj.DepartmentID
                ddlDepartment.SelectedValue = obj.DepartmentID
                userGroupID.Text = obj.UserGroupId
                txtCategoryCode.Text = obj.UserGroupName
                txtCategoryName.Text = obj.Description
            End If
            'End Editted by Zoya @3/03/2016

            If obj.Status = True Then
                ddlStatus.SelectedValue = 1
            Else
                ddlStatus.SelectedValue = 0
            End If

            'Master User = UGid:1 UGname=Admin
            If obj.UserGroupId.Equals(1) AndAlso obj.UserGroupName.Equals("Admin") Then
                txtCategoryName.ReadOnly = True
                txtCategoryName.Enabled = False
                ddlStatus.Enabled = False
                ibtnSave.Enabled = False
                ibtnDelete.Enabled = False
            Else
                txtCategoryName.ReadOnly = False
                txtCategoryName.Enabled = True
                ddlStatus.Enabled = True
                ibtnSave.Enabled = True
                ibtnDelete.Enabled = True
            End If

        End If
    End Sub
    ''' <summary>
    ''' Method to get the MenuName
    ''' </summary>
    ''' <param name="MenuId">Parameter is MenuId</param>
    ''' <remarks></remarks>
    Private Sub Menuname(ByVal MenuId As Integer)
        Dim eobj As New MenuEn
        Dim bobj As New MenuBAL
        eobj.MenuId = MenuId
        Try
            eobj = bobj.GetMenus(eobj)
        Catch ex As Exception
            LogError.Log("UserGroups", "Menuname", ex.Message)
        End Try
        lblMenuName.Text = eobj.MenuName
    End Sub
#End Region

    Protected Sub ibtnCancel_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ibtnCancel.Click
        LoadUserRights()
        OnClearData()
    End Sub

End Class
