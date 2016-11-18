Imports HTS.SAS.Entities
Imports HTS.SAS.BusinessObjects
Imports System.Data
Imports System.Data.SqlClient
Imports System.Collections.Generic

Partial Class UserRights
    Inherits System.Web.UI.Page
    Dim cmd As New SqlCommand
    Dim dr As SqlDataReader

    Dim objBE As New BusinessEntities.UserGroupEn
    Dim objSQLQuery As New SQLPowerQueryManager.PowerQueryManager.UserGroupDL

    Dim objBEDepart As New BusinessEntities.DepartmentEn
    Dim objSQLQueryDepart As New SQLPowerQueryManager.PowerQueryManager.DepartmentDL

    'Dim GlobalSQLConnString As String = ConfigurationManager.ConnectionStrings("SASNEWConnectionString").ToString
    Private GlobalSQLConnString As String = SQLPowerQueryManager.Helper.GetConnectionString()

    Dim DSReturn As New DataSet
    Dim strRetrunErrorMsg As String = String.Empty
    Dim blnReturnValue As Boolean
    Dim strMode As String

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack = True Then
            LoadUserRights()
            LoadDepartment()
            'LoadUserGroups()
            ibtnNext.Enabled = False
            ibtnNext.ImageUrl = "images/gnew_next.png"
            ibtnLast.Enabled = False
            ibtnLast.ImageUrl = "images/gnew_last.png"

            ibtnPrevs.Enabled = False
            ibtnPrevs.ImageUrl = "images/gnew_Prev.png"
            ibtnFirst.Enabled = False
            ibtnFirst.ImageUrl = "images/gnew_first.png"

            PnlView.Visible = False

            ddlRoles.Items.Insert(0, New ListItem("--Please Select--", "0"))

        End If
        lblMsg.Text = ""
    End Sub

    Protected Sub dgRoles_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dgRoles.ItemDataBound

        Dim chkView As CheckBox
        Dim chkEdit As CheckBox
        Dim chkAdd As CheckBox
        Dim chkDelete As CheckBox
        Dim chkPrint As CheckBox
        Dim chkListAll As CheckBox
        Dim chkPost As CheckBox
        Dim chkOthers As CheckBox
        Dim objCmd1 As New SqlCommand

        chkView = CType(e.Item.FindControl("View"), CheckBox)
        chkEdit = CType(e.Item.FindControl("Edit"), CheckBox)
        chkAdd = CType(e.Item.FindControl("Add"), CheckBox)
        chkDelete = CType(e.Item.FindControl("Delete"), CheckBox)
        chkPrint = CType(e.Item.FindControl("Print"), CheckBox)
        chkListAll = CType(e.Item.FindControl("List"), CheckBox)
        chkPost = CType(e.Item.FindControl("Post"), CheckBox)
        chkOthers = CType(e.Item.FindControl("Others"), CheckBox)

        If e.Item.Cells(3).Text = "True" Or e.Item.Cells(3).Text = "False" Then

            If e.Item.Cells(3).Text = "True" Then
                chkAdd.Checked = True
            Else
                chkAdd.Checked = False
            End If

            If e.Item.Cells(4).Text = "True" Then
                chkEdit.Checked = True
            Else
                chkEdit.Checked = False
            End If

            If e.Item.Cells(5).Text = "True" Then
                chkDelete.Checked = True
            Else
                chkDelete.Checked = False
            End If

            If e.Item.Cells(6).Text = "True" Then
                chkView.Checked = True
            Else
                chkView.Checked = False
            End If

            If e.Item.Cells(7).Text = "True" Then
                chkPrint.Checked = True
            Else
                chkPrint.Checked = False
            End If

            If e.Item.Cells(8).Text = "True" Then
                chkListAll.Checked = True
            Else
                chkListAll.Checked = False
            End If

            If e.Item.Cells(9).Text = "True" Then
                chkPost.Checked = True
            Else
                chkPost.Checked = False
            End If

            If e.Item.Cells(10).Text = "True" Then
                chkOthers.Checked = True
            Else
                chkOthers.Checked = False
            End If
        End If

    End Sub

    Private Sub LoadDepartment()
        objBEDepart.SQLCase = 3
        blnReturnValue = objSQLQueryDepart.RetriveData(objBEDepart, strRetrunErrorMsg, GlobalSQLConnString, DSReturn)

        If blnReturnValue Then
            ddlDepartment.DataSource = DSReturn
            ddlDepartment.DataTextField = "Department"
            ddlDepartment.DataValueField = "DepartmentID"
            ddlDepartment.DataBind()
            ddlDepartment.Items.Insert(0, New ListItem("--Please Select--", "0"))
        Else
            LogError.Log("UserRights", "LoadDepartment", strRetrunErrorMsg)
            lblMsg.Text = strRetrunErrorMsg
        End If
    End Sub

    Private Sub LoadUserGroups()
        If ddlDepartment.SelectedIndex = 0 Then
            objBE.DepartmentID = ""
        Else
            objBE.DepartmentID = ddlDepartment.SelectedValue
        End If
        objBE.SQLCase = 2
        blnReturnValue = objSQLQuery.RetriveData(objBE, strRetrunErrorMsg, GlobalSQLConnString, DSReturn)

        If blnReturnValue Then
            ddlRoles.DataSource = DSReturn
            'Edited by Zoya @3/03/2016
            'ddlRoles.DataTextField = "Description"
            ddlRoles.DataTextField = "UserGroupName"
            'End Edited by Zoya @3/03/2016
            ddlRoles.DataValueField = "UserGroupId"
            ddlRoles.DataBind()
            ddlRoles.Items.Insert(0, New ListItem("--Please Select--", "0"))
        Else
            LogError.Log("User Group", "LoadUserGroups", strRetrunErrorMsg)
            lblMsg.Text = strRetrunErrorMsg
        End If
    End Sub

    Protected Sub ibtnSave_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ibtnSave.Click
        'validate dropdownlist selection
        If ddlDepartment.SelectedValue = "0" Or ddlRoles.SelectedValue = "0" Then
            lblMsg.Text = "Please select All Required Fields "
            lblMsg.Visible = True
            Exit Sub
        End If
        If ddlDepartment.SelectedIndex = -1 Then
            lblMsg.Text = "Please Select Department."
            Exit Sub
        End If
        If ddlRoles.SelectedIndex = -1 Then
            lblMsg.Text = "Please Select Group."
            Exit Sub
        End If

        OnSave()
    End Sub

    Protected Sub ibtnCancel_Click1(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ibtnCancel.Click
        ddlDepartment.Items.Clear()
        ddlRoles.Items.Clear()
        dgRoles.DataSource = Nothing
        dgRoles.DataBind()
        LoadUserRights()
        LoadDepartment()
        LoadUserGroups()
        lblMsg.Text = ""
        PnlView.Visible = False
    End Sub

    Protected Sub ibtnNew_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ibtnNew.Click
        ddlDepartment.Items.Clear()
        ddlRoles.Items.Clear()
        dgRoles.DataSource = Nothing
        dgRoles.DataBind()
        LoadDepartment()
        LoadUserGroups()
        lblMsg.Text = ""
        PnlView.Visible = False
    End Sub

    Protected Sub ddlDepartment_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlDepartment.SelectedIndexChanged
        LoadUserGroups()
        ddlRoles_SelectedIndexChanged(sender, e)
    End Sub

    Protected Sub ddlRoles_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlRoles.SelectedIndexChanged
        Try
            Dim bsobj As New UserRightsBAL

            Try
                dgRoles.DataSource = bsobj.GetUserMenu(CInt(ddlRoles.SelectedValue))
            Catch ex As Exception
                LogError.Log("UserRights", "ddlRoles_SelectedIndexChanged", ex.Message)
            End Try
            dgRoles.DataBind()

            For Each dgItem1 As DataGridItem In dgRoles.Items

                Dim chkView As CheckBox = dgItem1.FindControl("View")
                Dim chkEdit As CheckBox = dgItem1.FindControl("Edit")
                Dim chkAdd As CheckBox = dgItem1.FindControl("Add")
                Dim chkDelete As CheckBox = dgItem1.FindControl("Delete")
                Dim chkPrint As CheckBox = dgItem1.FindControl("Print")
                Dim chkListAll As CheckBox = dgItem1.FindControl("List")
                Dim chkPost As CheckBox = dgItem1.FindControl("Post")
                Dim chkOthers As CheckBox = dgItem1.FindControl("Others")

                If ddlDepartment.SelectedValue = "master" AndAlso ddlRoles.SelectedValue = "1" Then
                    cbSelectAll.Checked = True
                    cbSelectAll.Enabled = False
                    chkView.Enabled = False
                    chkEdit.Enabled = False
                    chkAdd.Enabled = False
                    chkDelete.Enabled = False
                    chkPrint.Enabled = False
                    chkListAll.Enabled = False
                    chkPost.Enabled = False
                    chkOthers.Enabled = False
                Else
                    cbSelectAll.Checked = False
                    cbSelectAll.Enabled = True
                    chkView.Enabled = True
                    chkEdit.Enabled = True
                    chkAdd.Enabled = True
                    chkDelete.Enabled = True
                    chkPrint.Enabled = True
                    chkListAll.Enabled = True
                    chkPost.Enabled = True
                    chkOthers.Enabled = True
                End If
            Next

            'BindGrid()

        Catch ex As Exception
            Response.Write(ex.Message)
        End Try

        If ddlRoles.SelectedIndex = 0 Then
            PnlView.Visible = False
        Else
            PnlView.Visible = True
        End If
    End Sub

#Region "Methods"

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
            LogError.Log("UserRights", "LoadUserRights", ex.Message)
        End Try
        'Rights for Add

        If eobj.IsAdd = True Then
            ibtnSave.Enabled = True
            'OnAdd()
            ibtnNew.ImageUrl = "images/add.png"
            ibtnNew.Enabled = True
        Else
            ibtnNew.ImageUrl = "images/gadd.png"
            ibtnNew.Enabled = False
            ibtnNew.ToolTip = "Access Denied"
            ibtnSave.Enabled = False
            ibtnSave.ImageUrl = "images/gsave.png"
            ibtnSave.ToolTip = "Access Denied"
        End If
        'Rights for Edit
        If eobj.IsEdit = True Then

            ibtnSave.ToolTip = "Edit"
            Session("EditFlag") = True
            ibtnSave.Enabled = True

        Else
            Session("EditFlag") = False
            ibtnSave.Enabled = False
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
    ''' Method to Save and Update UserRights
    ''' </summary>
    ''' <remarks></remarks>
    Protected Sub OnSave()
        Dim ObjBS As New UserRightsBAL
        Dim dgItem1 As DataGridItem
        Dim chkView As CheckBox
        Dim chkEdit As CheckBox
        Dim chkAdd As CheckBox
        Dim chkDelete As CheckBox
        Dim chkPrint As CheckBox
        Dim chkListAll As CheckBox
        Dim chkPost As CheckBox
        Dim chkOthers As CheckBox
        Dim ObjUserRights As UserRightsEn
        Dim ListUserRights As New List(Of UserRightsEn)
        If Not ddlRoles.SelectedValue = -1 Then
            For Each dgItem1 In dgRoles.Items
                chkView = dgItem1.FindControl("View")
                chkEdit = dgItem1.FindControl("Edit")
                chkAdd = dgItem1.FindControl("Add")
                chkDelete = dgItem1.FindControl("Delete")
                chkPrint = dgItem1.FindControl("Print")
                chkListAll = dgItem1.FindControl("List")
                chkPost = dgItem1.FindControl("Post")
                chkOthers = dgItem1.FindControl("Others")

                ObjUserRights = New UserRightsEn

                ObjUserRights.UserGroup = CInt(ddlRoles.SelectedValue)
                ObjUserRights.MenuID = dgItem1.Cells(0).Text

                If chkAdd.Checked = True Then
                    ObjUserRights.IsAdd = 1
                Else
                    ObjUserRights.IsAdd = 0
                End If
                If chkView.Checked = True Then
                    ObjUserRights.IsView = 1
                Else
                    ObjUserRights.IsView = 0
                End If

                If chkEdit.Checked = True Then
                    ObjUserRights.IsEdit = 1
                Else
                    ObjUserRights.IsEdit = 0
                End If

                If chkDelete.Checked = True Then
                    ObjUserRights.IsDelete = 1
                Else
                    ObjUserRights.IsDelete = 0
                End If

                If chkPrint.Checked = True Then
                    ObjUserRights.IsPrint = 1
                Else
                    ObjUserRights.IsPrint = 0
                End If

                If chkListAll.Checked = True Then
                    ObjUserRights.IsAddModeDefault = 1
                Else
                    ObjUserRights.IsAddModeDefault = 0
                End If
                If chkPost.Checked = True Then
                    ObjUserRights.IsPost = 1
                Else
                    ObjUserRights.IsPost = 0
                End If
                If chkOthers.Checked = True Then
                    ObjUserRights.IsOthers = 1
                Else
                    ObjUserRights.IsOthers = 0
                End If

                ObjUserRights.LastUser = Session("User")
                ObjUserRights.LastDtTm = Date.Now
                ListUserRights.Add(ObjUserRights)
                ObjUserRights = Nothing
            Next

            Try
                ObjBS.InsertUserRights(ListUserRights)
            Catch ex As Exception
                LogError.Log("UserRights", "OnSave", ex.Message)
            End Try
            lblMsg.Text = "Record Saved Successfully "
            lblMsg.Visible = True
        Else
        End If
    End Sub

    ''' <summary>
    ''' Method to Bind UserRights Grid
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub BindGrid()
        Dim dgItem1 As DataGridItem
        Dim chkView As CheckBox
        Dim chkEdit As CheckBox
        Dim chkAdd As CheckBox
        Dim chkDelete As CheckBox
        Dim chkPrint As CheckBox
        Dim chkListAll As CheckBox
        Dim chkPost As CheckBox
        Dim chkOthers As CheckBox
        Dim objCmd1 As New SqlCommand
        Try

            For Each dgItem1 In dgRoles.Items
                chkView = dgItem1.FindControl("View")
                chkEdit = dgItem1.FindControl("Edit")
                chkAdd = dgItem1.FindControl("Add")
                chkDelete = dgItem1.FindControl("Delete")
                chkPrint = dgItem1.FindControl("Print")
                chkListAll = dgItem1.FindControl("List")
                chkPost = dgItem1.FindControl("Post")
                chkOthers = dgItem1.FindControl("Others")

                If dgItem1.Cells(3).Text = True Then
                    chkAdd.Checked = True
                Else
                    chkAdd.Checked = False
                End If
                If CInt(dgItem1.Cells(4).Text) = True Then
                    chkEdit.Checked = True
                Else
                    chkEdit.Checked = False
                End If
                If CInt(dgItem1.Cells(5).Text) = True Then
                    chkDelete.Checked = True
                Else
                    chkDelete.Checked = False
                End If
                If CInt(dgItem1.Cells(6).Text) = True Then
                    chkView.Checked = True
                Else
                    chkView.Checked = False
                End If
                If CInt(dgItem1.Cells(7).Text) = True Then
                    chkPrint.Checked = True
                Else
                    chkPrint.Checked = False
                End If
                If CInt(dgItem1.Cells(8).Text) = True Then
                    chkListAll.Checked = True
                Else
                    chkListAll.Checked = False
                End If
                If CInt(dgItem1.Cells(8).Text) = True Then
                    chkListAll.Checked = True
                Else
                    chkListAll.Checked = False
                End If
                If CInt(dgItem1.Cells(9).Text) = True Then
                    chkPost.Checked = True
                Else
                    chkPost.Checked = False
                End If
                If CInt(dgItem1.Cells(10).Text) = True Then
                    chkOthers.Checked = True
                Else
                    chkOthers.Checked = False
                End If

            Next
        Catch ex As Exception
            Response.Write(ex.Message)
        End Try
    End Sub
#End Region

End Class