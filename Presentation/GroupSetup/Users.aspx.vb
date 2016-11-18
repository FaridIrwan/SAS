Imports System.Collections.Generic
Imports HTS.SAS.BusinessObjects
Imports HTS.SAS.Entities
Imports System.Data
Imports System.Data.SqlClient
Imports System.Linq

Partial Class Users
    Inherits System.Web.UI.Page

#Region "Declare variables"

    Dim CFlag As String
    Dim DFlag As String
    Dim ListObjects As List(Of UsersEn)
    Private ErrorDescription As String

    Dim objUserGroupEn As New HTS.SAS.Entities.UserGroupsEn
    ' Dim objUserGroupEn As New BusinessEntities.UserGroupEn
    'Dim objUserGroupDL As New SQLPowerQueryManager.PowerQueryManager.UserGroupDL
    Dim objUserGroupDL As New HTS.SAS.BusinessObjects.UserGroupsBAL

    Dim objDepartmentEn As New HTS.SAS.Entities.DepartmentEn
    'Dim objDepartmentEn As New BusinessEntities.DepartmentEn
    'Dim objDepartmentDL As New SQLPowerQueryManager.PowerQueryManager.DepartmentDL
    Dim objDepartmentDL As New HTS.SAS.DataAccessObjects.DepartmentDAL

    Dim objUserEn As New HTS.SAS.Entities.UsersEn
    'Dim objUserEn As New BusinessEntities.UserEn
    'Dim objUserDL As New SQLPowerQueryManager.PowerQueryManager.UserDL 
    Dim objUserDL As New HTS.SAS.BusinessObjects.UsersBAL
    Dim objUsrDAL As New HTS.SAS.DataAccessObjects.UsersDAL

    'Dim GlobalSQLConnString As String = ConfigurationManager.ConnectionStrings("SASNEWConnectionString").ToString
    Private GlobalSQLConnString As String = SQLPowerQueryManager.Helper.GetConnectionString()

    'Dim DSReturn As New DataSet
    Dim strRetrunErrorMsg As String = String.Empty
    Dim blnReturnValue As Boolean
    Dim strMode As String

#End Region

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack() Then

            ibtnSave.Attributes.Add("onclick", "return validate()")

            Session("PageMode") = ""
            PageFunctional("Default")

            LoadUserRights()
            lblMsg.Text = ""

            Session("ListObj") = Nothing
            FillDataGrid()
            FillDepartment()
            FillUserGroup()

            Menuname(CInt(Request.QueryString("Menuid")))

            ibtnExpiryDate.Attributes.Add("onClick", "return getDate2from()")
            txtExpiryDate.Attributes.Add("OnKeyup", "return CheckBatchDate()")
            dates()

        End If
        lblMsg.Text = ""
    End Sub

    ''' <summary>
    ''' Method to Change the Date Format
    ''' </summary>
    ''' <remarks>Date in ddd/mm/yyyy Format</remarks>
    Private Sub dates()
        txtExpiryDate.Text = Format(DateAdd(DateInterval.Year, 1, Date.Now), "dd/MM/yyyy")
    End Sub

    Private Sub FillDataGrid()
        Dim DSReturn As New List(Of UsersEn)
        Try
            If DSReturn IsNot Nothing Then DSReturn.Clear()
            DSReturn = objUserDL.DataGrid(objUserEn)
            'If Not IsNothing(DSReturn) Then DSReturn.Clear()
            'DSReturn = objUserDL.DataGrid(objUserEn)

            DataGridDataBinding(DSReturn)

            If DSReturn.Count > 0 Then
                'If DSReturn.Tables(0).Rows.Count > 0 Then
                lblDataGridMsg.Text = ""
                lblDataGridMsg.Visible = False
            Else
                lblDataGridMsg.Text = "No Record Found..."
                lblDataGridMsg.Visible = True
            End If

        Catch ex As Exception
            LogError.Log("User", "FillDataGrid", ex.Message)
            lblMsg.Text = ex.Message
        End Try

        dgDataGrid.DataSource = DSReturn
        dgDataGrid.DataBind()

    End Sub

    Private Sub DataGridDataBinding(ByVal DSReturn As List(Of UsersEn))

        Try
            If DSReturn IsNot Nothing Then
                'If DSReturn.Tables.Count <> 0 Then
                'If DSReturn.Tables(0).Rows.Count > 0 Then
                dgDataGrid.DataSource = DSReturn
                dgDataGrid.DataBind()
                dgDataGrid.Visible = True
            Else
                dgDataGrid.Controls.Clear()
                dgDataGrid.Visible = False
                'End If
                'End If
            End If
        Catch ex As Exception
            LogError.Log("User", "DataGridDataBinding", ex.Message)
            lblMsg.Text = ex.Message
        End Try

    End Sub

    Protected Sub dgDataGrid_PageIndexChanged(source As Object, e As DataGridPageChangedEventArgs) Handles dgDataGrid.PageIndexChanged
        dgDataGrid.CurrentPageIndex = e.NewPageIndex
        FillDataGrid()
    End Sub

    Private Sub FillDepartment()
        Dim DSReturn As New List(Of DepartmentEn)

        Try
            DSReturn = objDepartmentDL.GetDepartmentList(objDepartmentEn)

            If DSReturn IsNot Nothing Then

                ddlDepartment.DataSource = DSReturn
                ddlDepartment.DataTextField = "Department"
                ddlDepartment.DataValueField = "DepartmentID"
                ddlDepartment.DataBind()
                ddlDepartment.Items.Insert(0, New ListItem("--Please Select--", "-1"))
            Else
                LogError.Log("User", "FillDepartment", "No Record In Department")
                lblMsg.Text = "No Record In Department"
            End If
        Catch ex As Exception
            LogError.Log("User", "FillDepartment", ex.Message)
            lblMsg.Text = ex.Message
        End Try

    End Sub

    Private Sub FillUserGroup()
        Dim DSReturn As New List(Of UserGroupsEn)

        If ddlDepartment.SelectedIndex = 0 Then
            objUserGroupEn.DepartmentID = ""
        Else
            objUserGroupEn.DepartmentID = ddlDepartment.SelectedValue
        End If

        Try
            DSReturn = objUserGroupDL.GetUserGroupList(objUserGroupEn)

            If DSReturn IsNot Nothing Then
                ddlUserGroup.DataSource = DSReturn
                'Editted by Zoya @3/03/2016
                'ddlUserGroup.DataTextField = "Description"
                ddlUserGroup.DataTextField = "UserGroupName"
                'End Editted by Zoya @3/03/2016
                ddlUserGroup.DataValueField = "UserGroupId"
                ddlUserGroup.DataBind()
                ddlUserGroup.Items.Insert(0, New ListItem("--Please Select--", "-1"))

                ddlUserGroup.AutoPostBack = True
                AddHandler ddlUserGroup.SelectedIndexChanged, AddressOf ddlUserGroup_SelectedIndexChanged
            Else
                LogError.Log("User Group", "FillUserGroup", "No Record In User Group")
                lblMsg.Text = "No Record In User Group"
            End If
        Catch ex As Exception
            LogError.Log("User", "FillUserGroup", ex.Message)
            lblMsg.Text = ex.Message
        End Try

    End Sub

    Protected Sub ddlDepartment_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlDepartment.SelectedIndexChanged
        FillUserGroup()
    End Sub

    Protected Sub ddlUserGroup_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlUserGroup.SelectedIndexChanged
        Try
            If ddlUserGroup.SelectedValue = "1" Then
                ddlUserGroup.SelectedValue = "-1"
                Throw New Exception("This Usergroup Is Disable By Default. Choose others.")
            End If
        Catch ex As Exception
            lblMsg.Visible = True
            lblMsg.Text = ex.Message
        End Try

    End Sub

    Private Sub DataGridDataBinding(ByVal DSReturn As List(Of String), ByVal blnValue As Boolean)

        Try
            If DSReturn IsNot Nothing Then
                'If DSReturn.Tables.Count <> 0 Then
                'If DSReturn.Tables(0).Rows.Count > 0 Then
                dgDataGrid.DataSource = DSReturn
                dgDataGrid.DataBind()
                dgDataGrid.Visible = True
            Else
                dgDataGrid.Controls.Clear()
                dgDataGrid.Visible = False
                'End If
                'End If
            End If
        Catch ex As Exception
            LogError.Log("User", "DataGridDataBinding", ex.Message)
            lblMsg.Text = ex.Message
        End Try

    End Sub
    Private Sub LoadData()

        Dim DSReturn As New List(Of UsersEn)
        objUserEn.UserID = hdnUserID.Value
        DSReturn = objUserDL.GetUser(objUserEn)
        Dim UserSelected As New UsersEn
        UserSelected = DSReturn.Find(Function(x) x.UserID = objUserEn.UserID)

        Try
            If UserSelected IsNot Nothing Then
                'DSReturn = objUserDL.GetUser(objUserEn)
                txtUserName.Text = UserSelected.UserName.ToString()
                txtPassword.Text = UserSelected.Password.ToString()
                ddlDepartment.SelectedValue = UserSelected.Department.ToString()
                ddlUserGroup.SelectedValue = UserSelected.UserGroupId.ToString()
                txtEmail.Text = UserSelected.Email.ToString()

                'ddlApprovalGroup.SelectedValue = UserSelected.WorkflowGroup.ToString()
                'If ddlApprovalGroup.SelectedValue = 1 Then
                '    rbApproval.SelectedValue = UserSelected.WorkflowRole.ToString()
                '    rbApproval.Visible = True
                'End If

                If UserSelected.UserStatus.ToString() = True Then
                    ddlStatus.SelectedValue = 1
                Else
                    ddlStatus.SelectedValue = 0
                End If
                'ddlStatus.SelectedValue = UserSelected.Status.ToString()

                'Added 3/8/2016
                txtStaffNo.Text = UserSelected.StaffID.ToString()
                txtStaffName.Text = UserSelected.StaffName.ToString()
                txtDesignation.Text = UserSelected.JobTitle.ToString()
                txtExpiryDate.Text = UserSelected.StaffExpiryDtTm.ToShortDateString()

                'Master User - Disable editing
                If Trim(txtUserName.Text) = "demo" Then
                    ddlDepartment.Enabled = False
                    ddlUserGroup.Enabled = False
                Else
                    ddlDepartment.Enabled = True
                    ddlUserGroup.Enabled = True
                End If

                'GetMenu(CInt(0))
            End If
        Catch ex As Exception
            LogError.Log("User", "LoadData", ex.Message)
            lblMsg.Text = ex.Message
        End Try

    End Sub

    'Private Sub LoadData()
    '    'Dim DSReturn As New List(Of UsersEn)
    '    Try
    '        objUserEn.UserID = hdnUserID.Value

    '        If Not IsNothing(DSReturn) Then DSReturn.Clear()

    '        DSReturn = objUserDL.GetUser(objUserEn)

    '        If DSReturn.Tables(0).Rows.Count > 0 Then
    '            With DSReturn.Tables(0).Rows(0)
    '                If IsDBNull(.Item("UserName")) Then
    '                    txtUserName.Text = ""
    '                Else
    '                    txtUserName.Text = .Item("UserName")
    '                End If

    '                If IsDBNull(.Item("Password")) Then
    '                    txtPassword.Text = ""
    '                Else
    '                    txtPassword.Text = .Item("Password")
    '                End If

    '                If IsDBNull(.Item("Department")) Then
    '                    ddlDepartment.SelectedIndex = -1
    '                Else
    '                    ddlDepartment.SelectedValue = .Item("Department")
    '                End If

    '                If IsDBNull(.Item("UserGroupId")) Then
    '                    ddlUserGroup.SelectedIndex = -1
    '                Else
    '                    ddlUserGroup.SelectedValue = .Item("UserGroupId")
    '                End If

    '                If IsDBNull(.Item("Email")) Then
    '                    txtEmail.Text = ""
    '                Else
    '                    txtEmail.Text = .Item("Email")
    '                End If

    '                If IsDBNull(.Item("UserStatus")) Then
    '                    ddlStatus.SelectedIndex = -1
    '                Else
    '                    If .Item("UserStatus") Then
    '                        ddlStatus.SelectedValue = 1
    '                    Else
    '                        ddlStatus.SelectedValue = 0
    '                    End If
    '                End If

    '            End With
    '        Else
    '            ClearData()
    '            lblMsg.Text = "Record doesn't exits!"
    '        End If

    '    Catch ex As Exception
    '        LogError.Log("User", "LoadData", ex.Message)
    '        lblMsg.Text = ex.Message
    '    End Try
    'End Sub

    Private Sub InsertUpdateData(ByVal strMode As String)

        Try
            With objUserEn
                .UserName = clsEmbeddedQuote(txtUserName.Text)
                .Password = clsEmbeddedQuote(txtPassword.Text)
                .RecStatus = True
                .UserGroupId = ddlUserGroup.SelectedValue
                .Department = ddlDepartment.SelectedValue
                .Email = clsEmbeddedQuote(txtEmail.Text)
                'Added by Mona 3/8/16 -Start
                .StaffID = clsEmbeddedQuote(txtStaffNo.Text)
                .StaffName = clsEmbeddedQuote(txtStaffName.Text)
                .JobTitle = clsEmbeddedQuote(txtDesignation.Text)
                .StaffExpiryDtTm = Trim(txtExpiryDate.Text)
                'Added by Mona 3/8/16 -End

                'Insert rbApproval value to database if ddlApprovalGroup is selected
                'If ddlApprovalGroup.SelectedValue = 1 Then
                '    .WorkflowRole = rbApproval.SelectedValue
                '    .WorkflowGroup = ddlApprovalGroup.SelectedValue
                'Else
                '    .WorkflowRole = "P"
                '    .WorkflowGroup = 0
                'End If

                If ddlStatus.SelectedValue = 1 Then
                    .UserStatus = True
                Else
                    .UserStatus = False
                End If

                If strMode = "New" Then
                    .LastUpdatedBy = Session("User")
                    .LastUpdatedDtTm = Format(Now(), "yyyy-MM-dd")
                Else
                    .UserID = hdnUserID.Value
                    .LastUpdatedBy = Session("User")
                    .LastUpdatedDtTm = Format(Now(), "yyyy-MM-dd")
                End If

            End With

            If strMode = "New" Then

                Dim DSReturn As New List(Of UsersEn)

                If Not IsNothing(DSReturn) Then DSReturn.Clear()

                DSReturn = objUserDL.GetUser(objUserEn)

                'If DSReturn.Tables(0).Rows.Count = 0 Then
                If DSReturn IsNot Nothing Then
                    blnReturnValue = objUserDL.Insert(objUserEn)

                    If blnReturnValue Then

                        'Get UserId for MenuProcess
                        Dim DSUser As UsersEn = Nothing
                        DSUser = objUserDL.GetItem(objUserEn)
                        hdnUserID.Value = DSUser.UserID.ToString()

                        'SaveMenu(hdnUserID.Value)

                        PageFunctional("Default")
                        ClearData()
                        FillDataGrid()
                        lblMsg.Text = "Record successfully saved"
                    Else
                        LogError.Log("User", "InsertUpdateData", "Insert Failed! No Row has been inserted.")
                        lblMsg.Text = "Insert Failed! No record has been inserted."
                    End If

                Else
                    lblMsg.Text = "Record already exist."
                    Exit Sub
                End If
            Else
                blnReturnValue = objUserDL.Update(objUserEn)

                If blnReturnValue Then

                    'SaveMenu(hdnUserID.Value)

                    PageFunctional("Default")
                    ClearData()
                    FillDataGrid()
                    lblMsg.Text = "Record successfully updated"
                Else
                    LogError.Log("User", "InsertUpdateData", "Update Failed! No Row has been updated.")
                    lblMsg.Text = "Update Failed! No record has been updated."
                End If

            End If

        Catch ex As Exception
            LogError.Log("User", "InsertUpdateData", ex.Message)
            lblMsg.Text = ex.Message
        End Try

    End Sub

    'Private Sub Delete(strUserID As String)
    '    '
    '    Try
    '        objUserEn.UserID = strUserID

    '        blnReturnValue = objUserDL.Delete(objUserEn)

    '        If blnReturnValue Then
    '            PageFunctional("Default")
    '            FillDataGrid()
    '            lblMsg.Text = "Record deleted Successfully"
    '        Else
    '            LogError.Log("User", "Delete", "Delete Failed! No Row has been deleted.")
    '            lblMsg.Text = "Delete Failed! No record has been deleted."
    '        End If
    '    Catch ex As Exception
    '        LogError.Log("User", "Delete", ex.Message)
    '        lblMsg.Text = ex.Message
    '    End Try
    'End Sub

    Private Sub Delete()

        'Editted by Zoya @23/02/2016
        Dim listUsers As New List(Of UsersEn)
        Dim cb As CheckBox
        objUserEn = New UsersEn

        For Each dgitem In dgDataGrid.Items
            cb = dgitem.Cells(0).Controls(1)
            If cb.Checked = True Then

                objUserEn.UserID = dgitem.Cells(1).Text.Trim
                listUsers.Add(objUserEn)
                dgDataGrid.SelectedIndex = -1
                objUserEn = New UsersEn
            End If
        Next

        Try '

            blnReturnValue = objUserDL.Delete(listUsers)

            If blnReturnValue Then
                PageFunctional("Default")
                ClearData()
                FillDataGrid()
                'Done Editted by Zoya @23/02/2016
                lblMsg.Text = "Record deleted Successfully"
            Else
                LogError.Log("User", "Delete", "Delete Failed! No Row has been deleted.")
                lblMsg.Text = "Delete Failed! No record has been deleted."
            End If
        Catch ex As Exception
            LogError.Log("User", "Delete", ex.Message)
            lblMsg.Text = ex.Message
        End Try
    End Sub

    Protected Sub ibtnNew_Click(sender As Object, e As ImageClickEventArgs) Handles ibtnNew.Click
        PageFunctional("Edit")
        ClearData()
        ViewState("strMode") = "New"
        'GetMenu(CInt(0))
    End Sub

    Protected Sub ibtnCancel_Click(sender As Object, e As ImageClickEventArgs) Handles ibtnCancel.Click
        PageFunctional("Default")
        ClearData()
        FillDataGrid()
    End Sub

    Protected Sub ibtnSearch_Click(sender As Object, e As ImageClickEventArgs) Handles ibtnSearch.Click

        If Not String.IsNullOrEmpty(txtSearch.Text) = True Then
            objUserEn.SearchCriteria = clsEmbeddedSpace(txtSearch.Text)
        Else
            objUserEn.SearchCriteria = ""
        End If

        FillDataGrid()
    End Sub

    Protected Sub ibtnOpen_Click(sender As Object, e As ImageClickEventArgs) Handles ibtnOpen.Click
        Dim atLeastOneSelected As Boolean = True
        Dim cb As CheckBox

        For Each dgitem In dgDataGrid.Items
            cb = dgitem.Cells(0).Controls(1)
            If cb IsNot Nothing AndAlso cb.Checked Then
                atLeastOneSelected = False
                hdnUserID.Value = dgitem.Cells(1).Text.Trim
            End If
        Next

        If atLeastOneSelected = False Then
            PageFunctional("Edit")
            LoadData()
            ViewState("strMode") = "Edit"
        Else
            lblMsg.Text = "Please select atleast 1 User."
        End If
    End Sub

    Protected Sub ibtnSave_Click(sender As Object, e As ImageClickEventArgs) Handles ibtnSave.Click
        strMode = ViewState("strMode")

        'Added by Zoya @23/02/2016
        If Trim(txtPassword.Text).Length < 6 Then
            lblMsg.Visible = True
            lblMsg.Text = "Password Must Contain Atleast 6 Characters."
            Exit Sub
        End If
        'End Added By Zoya

        InsertUpdateData(strMode)
    End Sub

    Protected Sub ibtnDelete_Click(sender As Object, e As ImageClickEventArgs) Handles ibtnDelete.Click
        'Editted By Zoya @ 23/02/2016

        Dim atLeastOneSelected As Boolean = True
        'Dim strUserID As String = ""
        Dim cb As CheckBox
        '
        For Each dgitem In dgDataGrid.Items
            cb = dgitem.Cells(0).Controls(1)
            If cb IsNot Nothing AndAlso cb.Checked Then
                atLeastOneSelected = False
                'strUserID = dgitem.Cells(1).Text.Trim
            End If
        Next
        '
        If atLeastOneSelected = False Then
            'Delete(strUserID)
            Delete()
        Else
            lblMsg.Text = "Please select atleast 1 User."
        End If

        'Done Editted By Zoya @ 23/02/2016
    End Sub

    'Protected Sub ibtnDelete_Click(sender As Object, e As ImageClickEventArgs) Handles ibtnDelete.Click
    '    Dim atLeastOneSelected As Boolean = True
    '    Dim strUserID As String = ""
    '    Dim cb As CheckBox
    '    Dim listUsers As New List(Of UsersEn)
    '    '
    '    For Each dgitem In dgDataGrid.Items
    '        cb = dgitem.Cells(0).Controls(1)
    '        If cb IsNot Nothing AndAlso cb.Checked Then
    '            atLeastOneSelected = False
    '            objUserEn.UserID = dgitem.Cells(1).Text.Trim

    '            listUsers.Add(objUserEn)
    '            dgDataGrid.SelectedIndex = -1
    '            objUserEn = Nothing
    '        End If
    '    Next
    '    '
    '    If atLeastOneSelected = False Then
    '        Delete(strUserID)
    '    Else
    '        lblMsg.Text = "Please select atleast 1 User."
    '    End If

    'End Sub

    Protected Sub ibtnRefresh_Click(sender As Object, e As ImageClickEventArgs) Handles ibtnRefresh.Click
        ClearData()
        FillDataGrid()
    End Sub

#Region "Methods"

    ''' <summary>
    ''' Method Button Configurations
    ''' </summary>
    ''' <param name="strMode"></param>
    ''' <remarks></remarks>
    Private Sub PageFunctional(ByVal strMode As String)

        If strMode = "Default" Then

            ibtnNew.Visible = True
            lblNew.Visible = True
            ibtnOpen.Visible = True
            lblOpen.Visible = True
            ibtnDelete.Visible = True
            lblDelete.Visible = True
            ibtnSearch.Visible = True
            lblSearch.Visible = True
            ibtnRefresh.Visible = True
            lblRefresh.Visible = True
            ibtnSave.Visible = False
            lblSave.Visible = False
            ibtnCancel.Visible = False
            lblCancel.Visible = False

            'Panel
            pnlSearch.Visible = True
            pnlEdit.Visible = False

        ElseIf strMode = "Edit" Then

            txtUserName.ReadOnly = True
            txtUserName.Enabled = False

            'Buttons
            ibtnNew.Visible = False
            lblNew.Visible = False
            ibtnOpen.Visible = False
            lblOpen.Visible = False
            ibtnDelete.Visible = False
            lblDelete.Visible = False
            ibtnSearch.Visible = False
            lblSearch.Visible = False
            ibtnRefresh.Visible = False
            lblRefresh.Visible = False
            ibtnSave.Visible = True
            lblSave.Visible = True
            ibtnCancel.Visible = True
            lblCancel.Visible = True

            'Panel
            pnlSearch.Visible = False
            pnlEdit.Visible = True
        End If
    End Sub

    Private Function clsEmbeddedQuote(ByVal strText As String) As String
        clsEmbeddedQuote = Replace(strText, "'", "''")
        If String.IsNullOrEmpty(clsEmbeddedQuote) = True Then
            clsEmbeddedQuote = ""
        End If
        Return clsEmbeddedQuote
    End Function

    Private Function clsEmbeddedSpace(ByVal strText As String) As String
        clsEmbeddedSpace = Replace(strText, "'", "")
        If String.IsNullOrEmpty(clsEmbeddedSpace) = True Then
            clsEmbeddedSpace = ""
        End If
        Return clsEmbeddedSpace
    End Function

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
            LogError.Log("Users", "LoadUserRights", ex.Message)
        End Try
        'Rights for Add

        If eobj.IsAdd = True Then
            ibtnNew.ImageUrl = "~/images/add.png"
            ibtnNew.Enabled = True
        Else
            ibtnNew.ImageUrl = "~/images/gadd.png"
            ibtnNew.Enabled = False
            ibtnNew.ToolTip = "Access Denied"
        End If
        'Rights for Edit
        If eobj.IsEdit = True Then
            ibtnSave.Enabled = True
            ibtnSave.ImageUrl = "~/images/save.png"
            ibtnSave.ToolTip = "Edit"
            If eobj.IsAdd = False Then
                ibtnSave.Enabled = False
                ibtnSave.ImageUrl = "~/images/gsave.png"
                ibtnSave.ToolTip = "Access Denied"
            End If

            Session("EditFlag") = True

        Else
            Session("EditFlag") = False
            ibtnSave.Enabled = False
            ibtnSave.ImageUrl = "images/gsave.png"
        End If

        'Rights for Delete
        If eobj.IsDelete = True Then
            ibtnDelete.ImageUrl = "~/images/delete.png"
            ibtnDelete.Enabled = True
        Else
            ibtnDelete.ImageUrl = "~/images/gdelete.png"
            ibtnDelete.ToolTip = "Access Denied"
            ibtnDelete.Enabled = False
        End If

    End Sub


    ''' <summary>
    ''' Method to Clear the Field Values
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub ClearData()
        txtUserName.Enabled = True
        txtUserName.ReadOnly = False
        txtUserName.Text = ""
        txtEmail.Text = ""
        txtPassword.Text = ""
        txtStaffNo.Text = ""
        txtStaffName.Text = ""
        txtDesignation.Text = ""
        ddlDepartment.Enabled = True
        ddlUserGroup.Enabled = True
        ddlDepartment.SelectedValue = "-1"
        ddlUserGroup.SelectedValue = "-1"
        ddlStatus.SelectedValue = "1"
        lblMsg.Text = ""
        txtSearch.Text = ""
        dgDataGrid.CurrentPageIndex = 0
        dgDataGrid.Controls.Clear()
        dgDataGrid.Visible = False
        'rbApproval.Visible = False
        'ddlApprovalGroup.SelectedValue = 0
        'dgRoles.Visible = False
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
            LogError.Log("Users", "Menuname", ex.Message)
        End Try
        lblMenuName.Text = eobj.MenuName
    End Sub

#End Region

    'Protected Sub ddlApprovalGroup_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlApprovalGroup.SelectedIndexChanged
    '    If ddlApprovalGroup.SelectedIndex = 1 Then
    '        rbApproval.Visible = True
    '        dgRoles.Visible = True
    '    Else
    '        rbApproval.Visible = False
    '        dgRoles.Visible = False
    '    End If
    'End Sub

    Protected Sub txtPassword_TextChanged(sender As Object, e As EventArgs) Handles txtPassword.TextChanged

        'Added by Zoya @23/02/2016
        If Trim(txtPassword.Text).Length < 6 Then
            lblMsg.Visible = True
            lblMsg.Text = "Password Must Contain Atleast 6 Characters."
            Exit Sub
        End If
        'End Added By Zoya

    End Sub

    'Protected Sub dgRoles_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dgRoles.ItemDataBound

    '    Dim chkAdd As CheckBox
    '    Dim objCmd1 As New SqlCommand

    '    'chkAdd = CType(e.Item.FindControl("Add"), CheckBox)

    '    'If e.Item.Cells(3).Text = "True" Or e.Item.Cells(3).Text = "False" Then

    '    '    If e.Item.Cells(3).Text = "True" Then
    '    '        chkAdd.Checked = True
    '    '    Else
    '    '        chkAdd.Checked = False
    '    '    End If

    '    'End If        

    '    Dim ObjUserRights As UserRightsEn
    '    Dim listUserMenu As New List(Of UserRightsEn)

    '    For Each dgItem1 In dgRoles.Items
    '        chkAdd = dgItem1.FindControl("Add")

    '        ObjUserRights = New UserRightsEn

    '        ObjUserRights.UserID = CInt(hdnUserID.Value)
    '        ObjUserRights.LastUser = Session("User")
    '        ObjUserRights.LastDtTm = Format(Now(), "yyyy-MM-dd")

    '        If chkAdd.Checked = True Then
    '            ObjUserRights.MenuID = dgItem1.Cells(0).Text
    '            listUserMenu.Add(ObjUserRights)
    '        End If
    '        ObjUserRights = Nothing
    '    Next

    'End Sub

    'Private Sub GetMenu(ByVal UserId As Integer)
    '    Try
    '        Dim bsobj As New UserRightsBAL
    '        Dim chkAdd As CheckBox
    '        Dim DSUserMenu As New DataSet
    '        Dim MenuIndex As Short = 0

    '        Try
    '            dgRoles.DataSource = bsobj.GetMenuByUser(UserId)
    '        Catch ex As Exception
    '            LogError.Log("Users", "GetMenu_dgRoles", ex.Message)
    '        End Try

    '        dgRoles.DataBind()
    '        dgRoles.Visible = True


    '        If CInt(hdnUserID.Value) > 0 Then

    '            'Dim ObjUserRights As UserRightsEn
    '            Dim listUserMenu As New List(Of UserRightsEn)

    '            listUserMenu = bsobj.GetMenuByUser(hdnUserID.Value)

    '            If listUserMenu.Count > 0 Then

    '                While MenuIndex < listUserMenu.Count

    '                    For Each dgItem1 In dgRoles.Items

    '                        chkAdd = dgItem1.FindControl("Add")

    '                        If dgItem1.Cells(0).Text = listUserMenu(MenuIndex).MenuID Then
    '                            chkAdd.Checked = True
    '                            Exit For
    '                        End If

    '                    Next

    '                    MenuIndex = MenuIndex + 1

    '                End While

    '            End If

    '        End If

    '    Catch ex As Exception
    '        Response.Write(ex.Message)
    '    End Try

    '    'If ddlRoles.SelectedIndex = 0 Then
    '    '    PnlView.Visible = False
    '    'Else
    '    '    PnlView.Visible = True
    '    'End If
    'End Sub

    'Private Sub SaveMenu(ByVal UserId As String)

    '    Dim chkAdd As CheckBox
    '    Dim ObjUserRights As UserRightsEn
    '    Dim listUserMenu As New List(Of UserRightsEn)

    '    If ddlApprovalGroup.SelectedIndex = 1 Then

    '        For Each dgItem1 In dgRoles.Items
    '            chkAdd = dgItem1.FindControl("Add")

    '            ObjUserRights = New UserRightsEn

    '            ObjUserRights.UserID = CInt(UserId)
    '            ObjUserRights.LastUser = Session("User")
    '            ObjUserRights.LastDtTm = Format(Now(), "yyyy-MM-dd")

    '            If chkAdd.Checked = True Then
    '                ObjUserRights.MenuID = dgItem1.Cells(0).Text
    '                listUserMenu.Add(ObjUserRights)
    '            End If
    '            ObjUserRights = Nothing
    '        Next

    '        Try
    '            objUsrDAL.InsertMenuByUser(listUserMenu)
    '        Catch ex As Exception
    '            LogError.Log("UserRights", "OnSave", ex.Message)
    '        End Try

    '        If strMode = "New" Then
    '            lblMsg.Text = "Record successfully saved"
    '        Else
    '            lblMsg.Text = "Record successfully updated"
    '        End If

    '    Else
    '        ObjUserRights = New UserRightsEn
    '        ObjUserRights.UserID = CInt(UserId)

    '        Try
    '            objUsrDAL.DeleteMenu(ObjUserRights)
    '        Catch ex As Exception
    '            LogError.Log("UserRights", "OnSave", ex.Message)
    '        End Try

    '    End If

    'End Sub

End Class
