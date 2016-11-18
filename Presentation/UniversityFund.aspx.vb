Imports HTS.SAS.BusinessObjects
Imports HTS.SAS.Entities
Imports System.Data
Imports System.Collections.Generic

Partial Class universityFund
    Inherits System.Web.UI.Page
#Region "Global Declaration "

    'declare instant
    Private _MaxModule As New MaxModule.CfCommon
    Dim ListObjects As List(Of UniversityFundEn)
    Dim CFlag As String
    Dim DFlag As String
    Private ErrorDescription As String

#End Region

    ''Private LogErrors As LogError
    Protected Sub IbtnView_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ibtnView.Click
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
        lblMsg.Visible = False
        If Not IsPostBack() Then
            'Adding validation for save button
            ibtnSave.Attributes.Add("onclick", "return validate()")
            ibtnDelete.Attributes.Add("onclick", "return getconfirm()")
            txtRecNo.Attributes.Add("OnKeyUp", "return geterr()")
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
    ''' Method to Load UserRights
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub LoadUserRights()
        Dim obj As New UsersBAL
        Dim eobj As New UserRightsEn

        Try
            eobj = obj.GetUserRights(CInt(Request.QueryString("Menuid")), CInt(Session("UserGroup")))
        Catch ex As Exception
            LogError.Log("UniversityFund", "LoadUserRights", ex.Message)
        End Try
        'Rights for Add

        If eobj.IsAdd = True Then
            'ibtnSave.Enabled = True
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
    ''' <summary>
    ''' Method to Load Fields in New Mode
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub OnAdd()
        Session("PageMode") = "Add"
        PnlAdd.Visible = True
        ibtnSave.Enabled = True
        ibtnSave.ImageUrl = "images/save.png"
        OnClearData()
        dgView.SelectedIndex = -1
        PnlView.Visible = False
    End Sub
    ''' <summary>
    ''' Method to Validate Before Save
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub SpaceValidation()
        If Trim(txtFundName.Text).Length = 0 Then

            txtFundName.Text = Trim(txtFundName.Text)
            lblMsg.Text = "Enter Valid Description"
            lblMsg.Visible = True
            txtFundName.Focus()
            Exit Sub
        End If
        If Trim(txtGlAccount.Text).Length = 0 Then
            lblMsg.Text = "Enter Valid GL Account"
            lblMsg.Visible = True
            txtGlAccount.Text = Trim(txtGlAccount.Text)

            txtGlAccount.Focus()
            Exit Sub
        End If
        OnSave()
    End Sub
    ''' <summary>
    ''' Method to Change to Edit Mode
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub OnEdit()
        Session("PageMode") = "Edit"

    End Sub
    ''' <summary>
    ''' Method to Save and Update University Funds
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub OnSave()
        Dim bsobj As New UniversityFundBAL
        Dim eobj As New UniversityFundEn
        Dim RecAff As Integer
        eobj.UniversityFundCode = Trim(txtFundCode.Text)
        eobj.Description = Trim(txtFundName.Text)
        eobj.GLCode = Trim(txtGlAccount.Text)
        If ddlStatus.SelectedValue = 0 Then
            eobj.Status = False
        Else
            eobj.Status = True
        End If
        eobj.UpdatedBy = Session("User")
        eobj.UpdatedDtTm = Date.Now.ToString()
        lblMsg.Visible = True

        If Session("PageMode") = "Add" Then
            Try
                RecAff = bsobj.Insert(eobj)
                If RecAff = 0 Then
                    ErrorDescription = "Record already Exist"
                Else
                    ErrorDescription = "Record Saved Successfully "
                End If

                lblMsg.Text = ErrorDescription
            Catch ex As Exception
                lblMsg.Text = ex.Message.ToString()
                LogError.Log("UniversityFund", "OnSave", ex.Message)
            End Try
        ElseIf Session("PageMode") = "Edit" Then
            Try
                RecAff = bsobj.Update(eobj)
                ListObjects = Session("ListObj")
                ListObjects(CInt(txtRecNo.Text) - 1) = eobj
                Session("ListObj") = ListObjects

                ErrorDescription = "Record Updated Successfully "
                lblMsg.Text = ErrorDescription
            Catch ex As Exception
                lblMsg.Text = ex.Message.ToString()
                LogError.Log("UniversityFund", "OnSave", ex.Message)
            End Try

        End If

    End Sub
    ''' <summary>
    ''' Method to Delete University Funds
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub OnDelete()
        lblMsg.Visible = True
        If txtFundCode.Text <> "" Then
            If lblCount.Text = "" Then lblCount.Text = 0
            If lblCount.Text > 0 Then
                Dim bsobj As New UniversityFundBAL
                Dim eobj As New UniversityFundEn

                Dim RecAff As Integer
                lblMsg.Visible = True
                eobj.UniversityFundCode = txtFundCode.Text
                Try
                    RecAff = bsobj.Delete(eobj)
                    ListObjects = Session("ListObj")
                    ListObjects.RemoveAt(CInt(txtRecNo.Text) - 1)
                    lblCount.Text = lblCount.Text - 1
                    Session("ListObj") = ListObjects
                    ErrorDescription = "Record Deleted Successfully "
                    lblMsg.Text = ErrorDescription
                Catch ex As Exception
                    lblMsg.Text = ex.Message.ToString()
                    LogError.Log("UniversityFund", "OnDelete", ex.Message)
                End Try
                txtFundCode.Text = ""
                txtFundName.Text = ""
                txtGlAccount.Text = ""
                ddlStatus.SelectedValue = "1"
                DFlag = "Delete"
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
    ''' <summary>
    ''' Method to get the List Of University Funds
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub LoadListObjects()
        Dim ds As New DataSet
        Dim bobj As New UniversityFundBAL
        Dim eobj As New UniversityFundEn
        Dim recStu As Integer

        If ddlStatus.SelectedValue = -1 Then
            recStu = -1
        Else
            recStu = ddlStatus.SelectedValue
        End If
        eobj.UniversityFundCode = Trim(txtFundCode.Text)
        eobj.Description = Trim(txtFundName.Text)
        eobj.GLCode = Trim(txtGlAccount.Text)
        eobj.Status = ddlStatus.SelectedValue

        Try
            ListObjects = bobj.GetUniversityFundList(eobj)
        Catch ex As Exception
            LogError.Log("UniversityFund", "LoadListObjects", ex.Message)
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
            PnlView.Visible = False
            PnlAdd.Visible = True
            OnMoveFirst()

            If Session("EditFlag") = True Then
                Session("PageMode") = "Edit"
                txtFundCode.Enabled = False
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
        If txtRecNo.Text = 0 Then
            txtRecNo.Text = 1
        Else
            If lblCount.Text = 0 Then
                txtRecNo.Text = 0
            Else
                PnlView.Visible = False
                PnlAdd.Visible = True
                Dim obj As UniversityFundEn
                ListObjects = Session("ListObj")
                obj = ListObjects(RecNo)

                txtFundCode.Text = obj.UniversityFundCode
                txtFundName.Text = obj.Description
                txtGlAccount.Text = obj.GLCode

                If obj.Status = True Then
                    ddlStatus.SelectedValue = 1
                Else
                    ddlStatus.SelectedValue = 0
                End If
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
            LogError.Log("UniversityFund", "Menuname", ex.Message)
        End Try
        lblMenuName.Text = eobj.MenuName
    End Sub
    ''' <summary>
    ''' Method to Clear the Field Values
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub OnClearData()
        txtFundCode.Enabled = True
        'Clear Text Box values
        Session("ListObj") = Nothing
        DisableRecordNavigator()
        txtFundCode.Text = ""
        txtFundName.Text = ""
        txtGlAccount.Text = ""
        ddlStatus.SelectedValue = "1"
        Session("PageMode") = "Add"

        lblDesc.Text = ""
        lblDesc.Visible = False
        imgGL.Visible = False
    End Sub
#End Region

    Protected Sub ibtnCancel_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ibtnCancel.Click

        LoadUserRights()
        OnClearData()
    End Sub
    Protected Sub Check_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Check.Click

        'varaible declaration
        Dim result As Boolean = False
        Dim _GLCode As String = txtGlAccount.Text
        Dim _GLLedgerType As String = Nothing
        Dim _GLDescription As String = Nothing

        Try
            imgGL.Visible = False
            'Adding validation for Check button
            Check.Attributes.Add("onclick", "return validate()")
            'Check Empty GLCode - Starting
            If Not _GLCode = "" Then
                'Check GLCODE in CF - Starting
                result = _MaxModule.GLCodeValid(_GLCode)

                If result Then
                    'Check GLLedgerType in CF
                    _GLLedgerType = _MaxModule.GetLedgerType(_GLCode)

                    If Not _GLLedgerType = "" Then
                        'Retrive GLDescription in CF 
                        _GLDescription = _MaxModule.GetGLDescription(_GLCode, _GLLedgerType)
                        lblDesc.Text = _GLDescription
                        imgGL.Visible = True
                        imgGL.ImageUrl = "~/images/check.png"
                    End If
                Else
                    lblMsg.Text = "Invalid GLCode"
                    imgGL.Visible = True
                    imgGL.ImageUrl = "~/images/cross.png"
                End If
                'Check GLCODE in CF - Ended
            End If
            'Check Empty GLCode - Starting
            'imgGL.Visible = False
            'Dim oleCF As New OleDb.OleDbConnection
            'Dim dsCF As New DataSet
            'Dim strsql As String
            'Dim olCFEn As New ConnectionEn
            'Dim olCFBal As New ConnectionBAL

            'olCFEn.Code = "I"
            'olCFEn = olCFBal.GetConnectionString(olCFEn)
            'oleCF.ConnectionString = olCFEn.ConnectionStrings
            'strsql = "select glac_account, glac_desc from gl_account where  glac_account = '" & txtGlAccount.Text & "'"
            'Dim daCF As New OleDb.OleDbDataAdapter(strsql, oleCF)
            'daCF.Fill(dsCF, "gl_account")

            'If dsCF.Tables("gl_account").Rows.Count > 0 Then
            '    imgGL.ImageUrl = "~/images/check.png"

            'Else
            '    imgGL.ImageUrl = "~/images/cross.png"

            'End If
            'imgGL.Visible = True

        Catch ex As Exception
            Throw ex
        End Try

    End Sub
    'Protected Sub Check_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Check.Click
    '    Try
    '        imgGL.Visible = False
    '        Dim oleCF As New OleDb.OleDbConnection
    '        Dim dsCF As New DataSet

    '        Dim strGL As String = "A-00-00000-00-A18102"

    '        If txtGlAccount.Text = strGL Then
    '            imgGL.ImageUrl = "~/images/check.png"
    '        Else
    '            imgGL.ImageUrl = "~/images/cross.png"
    '        End If
    '        imgGL.Visible = True

    '    Catch ex As Exception
    '        Throw ex
    '    End Try
    'End Sub


    Protected Sub txtGlAccount_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtGlAccount.TextChanged
        If imgGL.Visible Then
            imgGL.Visible = False
        End If
    End Sub
End Class
