#Region "NameSpaces "

Imports MaxGeneric
Imports System.Data
Imports Telerik.Web.UI
Imports HTS.SAS.Entities
Imports System.Data.SqlClient
Imports HTS.SAS.BusinessObjects
Imports HTS.SAS.DataAccessObjects
Imports System.Collections.Generic
Imports System.Reflection.MethodBase
Imports System.Linq

#End Region

Partial Class MasterPage3
    Inherits System.Web.UI.MasterPage

#Region "Page Load "

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs)

        Try

            If Not Page.IsPostBack Then

                'Set Session TimeOut
                Session.Timeout = Helper.SessionTimeOut

                'Load Menu
                Call LoadMenu(Session(Helper.MenuSession))

            End If

            'Set Branch Code
            Session(Helper.BranchCodeSession) = "1"

            'Set User Name
            lblUser.Text = Session(Helper.UserSession)

            Response.Cache.SetCacheability(HttpCacheability.NoCache)
            Response.Cache.SetAllowResponseInBrowserHistory(False)

            'If Session Not available - Start
            If Session(Helper.UserSession) Is Nothing Then
                'Redirect User to Login Page
                Response.Redirect(Helper.LoginPage)
            End If
            'If Session Not available - Stop

        Catch ex As Exception

            Call Generic.LogError(GetCurrentMethod().ToString(), ex.Message)

        End Try

    End Sub

#End Region

#Region "Load Menu "

    Private Sub LoadMenu(ByVal MenuName As String)

        'Create Instances - Start
        Dim _MenuEn As New MenuEn
        Dim _UsersEn As New UsersEn
        Dim MenuTable As DataTable = Nothing
        Dim _UserRightsBAL As New UserRightsBAL
        Dim MenuList As New List(Of MenuMasterEn), MenuListP As New List(Of MenuMasterEn), MenuListA As New List(Of MenuMasterEn)
        Dim ListPrep As New List(Of WorkflowSetupEn), ListApp As New List(Of WorkflowSetupEn)
        'Create Instances - Stop

        'Variable Declarations
        Dim MenuIndex As Short = 0, SubMenuIndex As Short = 0, MenuGroup As String = Nothing, Index As Integer

        Try

            'Clear Menu Items
            Call RadPanelBar1.Items.Clear()

            'Get Menu Table
            If Session(Helper.WorkflowSession) = "1" Then
                MenuTable = GetMenuTableWorkFlow()
            ElseIf Session(Helper.WorkflowSession) = "0" Then
                MenuTable = GetMenuTable()
            End If

            'Loop Thro the Menu Table - Start
            For MenuIndex = 0 To MenuTable.Rows.Count - 1

                Dim MenuRow As DataRow = MenuTable.Rows(MenuIndex)
                Dim ParentMenu As RadPanelItem = New RadPanelItem()

                'Set Menu Propoerties - Start
                ParentMenu.Text = MenuRow(Helper.MenuNameCol)
                ParentMenu.Value = MenuRow(Helper.PageUrlCol)
                ParentMenu.ImageUrl = MenuRow(Helper.ImageUrlCol)
                ParentMenu.NavigateUrl = MenuRow(Helper.PageUrlCol)
                'Set Menu Propoerties - Stop

                'Add to Menu Bar
                Call RadPanelBar1.Items.Add(ParentMenu)

                'If Menu Session Not Available - Start
                If Not Session(Helper.MenuSession) Is Nothing Then

                    'if Menu Not Home - Start 
                    If Session(Helper.MenuSession).Equals(Helper.HomeMenu) = False Then

                        If clsGeneric.NullToShort(Session(Helper.UserGroupSession)) = 1 Then
                            'Load menu Master Admin - start
                            MenuList = New UserRightsDAL().GetMenuMasterAdmin(MenuRow(Helper.MenuNameCol), clsGeneric.NullToShort(Session(Helper.UserGroupSession)))
                            'Load menu Master Admin - end
                        Else

                            ListPrep = New WorkflowSetupDAL().GetPreparerList().Where(Function(x) x.WorkflowPreparerEn.UserId.Equals(
                                                                                     clsGeneric.NullToInteger(Session(Helper.UserIDSession)))).ToList()
                            ListApp = New WorkflowSetupDAL().GetApproverList().Where(Function(x) x.WorkflowApproverEn.UserId.Equals(
                                                                                         clsGeneric.NullToInteger(Session(Helper.UserIDSession)))).ToList()
                            'setup - start
                            If MenuRow(Helper.MenuNameCol) = "Setup" Then

                                MenuList = New List(Of MenuMasterEn)

                                If ListApp.Count > 0 Then
                                    MenuListA = _UserRightsBAL.GetMenuByWorkflowGroup(MenuRow(Helper.MenuNameCol),
                                                                                     clsGeneric.NullToShort(Session(Helper.UserGroupSession)),
                                                                                     clsGeneric.NullToInteger(Session(Helper.UserIDSession)))
                                Else
                                    MenuListA = New List(Of MenuMasterEn)
                                End If

                                If ListPrep.Count > 0 Then
                                    MenuListP = _UserRightsBAL.GetMenuByUserGroup(MenuRow(Helper.MenuNameCol), clsGeneric.NullToShort(Session(Helper.UserGroupSession)))
                                Else
                                    MenuListP = New List(Of MenuMasterEn)
                                End If

                                MenuListA.AddRange(MenuListP)
                                MenuList.AddRange(MenuListA)

                            End If
                            'setup - end

                            'process - start
                            If MenuRow(Helper.MenuNameCol) = "Process" Then

                                MenuList = New List(Of MenuMasterEn)

                                If ListApp.Count > 0 Then
                                    MenuListA = _UserRightsBAL.GetMenuByWorkflowGroup(MenuRow(Helper.MenuNameCol),
                                                                                     clsGeneric.NullToShort(Session(Helper.UserGroupSession)),
                                                                                     clsGeneric.NullToInteger(Session(Helper.UserIDSession)))
                                Else
                                    MenuListA = New List(Of MenuMasterEn)
                                End If

                                If ListPrep.Count > 0 Then
                                    MenuListP = New UserRightsDAL().GetPreparerWorkflowList(MenuRow(Helper.MenuNameCol),
                                                                                           clsGeneric.NullToInteger(Session(Helper.UserIDSession)),
                                                                                           clsGeneric.NullToShort(Session(Helper.UserGroupSession)))
                                Else
                                    MenuListP = New List(Of MenuMasterEn)
                                End If

                                MenuListA.AddRange(MenuListP)
                                MenuList.AddRange(MenuListA)

                            End If
                            'process - end

                            'reports - start
                            If MenuRow(Helper.MenuNameCol) = "Reports" Then

                                MenuList = New List(Of MenuMasterEn)

                                If ListApp.Count > 0 AndAlso ListPrep.Count > 0 Then
                                    MenuList = _UserRightsBAL.GetMenuByUserGroup(MenuRow(Helper.MenuNameCol), clsGeneric.NullToShort(Session(Helper.UserGroupSession)), "Both")
                                ElseIf ListApp.Count > 0 Then
                                    MenuList = _UserRightsBAL.GetMenuByUserGroup(MenuRow(Helper.MenuNameCol), clsGeneric.NullToShort(Session(Helper.UserGroupSession)), "IsApprover")
                                ElseIf ListPrep.Count > 0 Then
                                    MenuList = _UserRightsBAL.GetMenuByUserGroup(MenuRow(Helper.MenuNameCol), clsGeneric.NullToShort(Session(Helper.UserGroupSession)), "IsPreparer")
                                End If

                            End If
                            'reports - end

                            MenuList = MenuList.OrderBy(Function(x) x.PageOrder).ToList()

                        End If

                        'loop thro the Sub Menu Items - Start
                        For SubMenuIndex = 0 To MenuList.Count - 1

                            Dim SubMenuItem As RadPanelItem = New RadPanelItem()

                            'Set Sub Menu Propoerties - Start
                            SubMenuItem.Text = MenuList(SubMenuIndex).PageName
                            SubMenuItem.Value = MenuList(SubMenuIndex).PageUrl.ToString()
                            SubMenuItem.NavigateUrl = MenuList(SubMenuIndex).PageUrl.ToString()
                            'Set Sub Menu Propoerties - Stop

                            'Add Sub Menu
                            RadPanelBar1.Items(MenuIndex).Items.Add(SubMenuItem)

                        Next
                        'loop thro the Sub Menu Items - Stop

                    End If
                    'if Menu Not Home - Stop

                End If
                'If Menu Session Not Available - Stop

            Next
            'Loop Thro the Menu Table - Stop

            'Set Menu Group
            MenuGroup = Session(Helper.MenuSession)

            'Set Menu Index - Start
            Select Case MenuGroup
                Case Helper.HomeMenu
                    Index = Generic._Menu.Home
                Case Helper.SetUpMenu
                    Index = Generic._Menu.Setup
                Case Helper.ProcessMenu
                    Index = Generic._Menu.Process
                Case Helper.ReportsMenu
                    Index = Generic._Menu.Reports
                    'Case Helper.DashboardMenu
                    'Index = Generic._Menu.Dashboard                
            End Select
            'Set Menu Index - Stop

            'Menu Display - Start
            If RadPanelBar1.Items.Count > 0 Then
                RadPanelBar1.Items(Index).Selected = True
                RadPanelBar1.Items(Index).Expanded = True
            End If
            'Menu Display - Stop

        Catch ex As Exception

            Call Generic.LogError(GetCurrentMethod().ToString(), ex.Message)

        End Try

    End Sub

    Function GetMenuTable() As DataTable

        'Create new DataTable instance.
        Dim MenuTable As New DataTable

        'Add Menu Columns - Start
        MenuTable.Columns.Add(Helper.MenuIdCol, GetType(Short))
        MenuTable.Columns.Add(Helper.MenuNameCol, GetType(String))
        MenuTable.Columns.Add(Helper.ImageUrlCol, GetType(String))
        MenuTable.Columns.Add(Helper.PageUrlCol, GetType(String))
        'Add Menu Columns - Stop

        'Add Menu Rows - Start
        MenuTable.Rows.Add(Generic._Menu.Home, Helper.HomeMenu, Helper.HomeImg, Helper.HomePage)
        'MenuTable.Rows.Add(Generic._Menu.Dashboard, Helper.DashboardMenu, Helper.DashboardImg, Helper.DashboardPage)
        MenuTable.Rows.Add(Generic._Menu.Setup, Helper.SetUpMenu, Helper.SettingsImg, Helper.SetupPage)
        MenuTable.Rows.Add(Generic._Menu.Process, Helper.ProcessMenu, Helper.ProcessImg, Helper.ProcessPage)
        MenuTable.Rows.Add(Generic._Menu.Reports, Helper.ReportsMenu, Helper.ReportsImg, Helper.ReportsPage)
        'Add Menu Rows - Stop

        Return MenuTable

    End Function

    Protected Sub RadPanelBar1_ItemClick(ByVal sender As Object, ByVal e As RadPanelBarEventArgs) 'Handles RadPanelBar1.ItemClick

    End Sub

#End Region

#Region "Logout "

    Protected Sub btnLogout_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnLogout.Click
        Session.Clear()
        Request.Cookies.Clear()
        FormsAuthentication.SignOut()
        RadPanelBar1.Controls.Clear()
        Response.Redirect(Helper.LoginPage)
    End Sub

    Protected Sub btnImgLogout_Click(sender As Object, e As ImageClickEventArgs) Handles btnImgLogout.Click
        btnLogout_Click(sender, e)
    End Sub

#End Region

    Function GetMenuTableWorkFlow() As DataTable

        'Create new DataTable instance.
        Dim MenuTable As New DataTable

        'Add Menu Columns - Start
        MenuTable.Columns.Add(Helper.MenuIdCol, GetType(Short))
        MenuTable.Columns.Add(Helper.MenuNameCol, GetType(String))
        MenuTable.Columns.Add(Helper.ImageUrlCol, GetType(String))
        MenuTable.Columns.Add(Helper.PageUrlCol, GetType(String))
        'Add Menu Columns - Stop

        'Add Menu Rows - Start
        MenuTable.Rows.Add(Generic._Menu.Home, Helper.HomeMenu, Helper.HomeImg, Helper.HomePage)
        MenuTable.Rows.Add(Generic._Menu.Setup, Helper.SetUpMenu, Helper.SettingsImg, Helper.SetupPage)
        MenuTable.Rows.Add(Generic._Menu.Process, Helper.ProcessMenu, Helper.ProcessImg, Helper.ProcessPage)
        MenuTable.Rows.Add(Generic._Menu.Reports, Helper.ReportsMenu, Helper.ReportsImg, Helper.ReportsPage)
        'Add Menu Rows - Stop

        Return MenuTable

    End Function

End Class

