Imports HTS.SAS.BusinessObjects
Imports HTS.SAS.Entities
Imports System.Data
Imports System.Collections.Generic
Partial Class semester
    Inherits System.Web.UI.Page
    Dim ListObjects As List(Of StudentEn)
    Dim CFlag As String
    Dim DFlag As String
    Private ErrorDescription As String
    ''Private LogErrors As LogError
    Protected Sub IbtnView_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ibtnView.Click
        If lblCount.Text <> "" Then
            If CInt(lblCount.Text) > 0 Then
                'Clearing the Fields before Search
                OnClearData()
                If ibtnNew.Enabled = False Then
                    ibtnSave.Enabled = False
                    ibtnSave.ImageUrl = "images/gsave.png"
                    ibtnSave.ToolTip = "Access Denied"
                End If
            Else
                txtCurrSem.Value = ddlCurSem.SelectedValue.ToString
                trNewSem.Visible = True
                'Getting list of DegreeTpes
                LoadListObjects()
            End If
        Else
            trNewSem.Visible = True
            'Getting list of DegreeTpes
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

            'while loading list object make it nothing
            Session("ListObj") = Nothing
            'Disable Navigation in PageLoad
            DisableRecordNavigator()
            'load PageName
            Menuname(CInt(Request.QueryString("Menuid")))

            'Load
            OnLoadCurrSem()
            'Hide Dropdown Menu
            trNewSem.Visible = False
            trProgram.Visible = False
            trProgramType.Visible = False
            txtCurrSem.Value = ""
        End If
        lblMsg.Visible = False
    End Sub
    Protected Sub ibtnCancel_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ibtnCancel.Click
        LoadUserRights()
        OnClearData()
    End Sub
    Protected Sub ibtnNew_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ibtnNew.Click
        OnClearData()
        'OnAdd()
    End Sub
    Protected Sub ibtnDelete_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ibtnDelete.Click
        OnDelete()
    End Sub

    Protected Sub ibtnSave_Click1(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ibtnSave.Click
        SpaceValidation()
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

#Region "Methods"
    ''' <summary>
    ''' Method to Validate Before Save
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub SpaceValidation()
        'If Trim(txtDegreeName.Text).Length = 0 Then

        '    txtDegreeName.Text = Trim(txtDegreeName.Text)
        '    lblMsg.Text = "Enter valid Degree Name "
        '    lblMsg.Visible = True
        '    txtDegreeName.Focus()
        '    Exit Sub
        'End If
        'If Trim(txtShortName.Text).Length = 0 Then

        '    txtShortName.Text = Trim(txtShortName.Text)
        '    lblMsg.Text = "Enter valid Short Name"
        '    lblMsg.Visible = True
        '    txtShortName.Focus()
        '    Exit Sub
        'End If

        If ddlNewSem.SelectedValue = "-1" Then
            lblMsg.Visible = True
            lblMsg.Text = "Please Select New Semester"
            ddlNewSem.Focus()
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
            LogError.Log("DegreeType", "LoadUserRights", ex.Message)
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
    ''' Method to Clear Field Vlues
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub OnClearData()
        Session("ListObj") = Nothing
        DisableRecordNavigator()
        Session("PageMode") = "Add"
        ibtnDelete.Enabled = True
        ibtnView.Enabled = True
        ibtnPrint.Enabled = True
        ibtnOthers.Enabled = True
        ibtnNext.Enabled = True
        ibtnFirst.Enabled = True
        ibtnLast.Enabled = True
        ibtnPrevs.Enabled = True
        ibtnPosting.Enabled = True
        dgView.Dispose()
        dgView.Visible = False
        trCurrSem.Visible = True
    End Sub
    Private Sub OnAdd()
        PnlAdd.Visible = True
        Session("ListObj") = Nothing
        OnClearData()
        PnlView.Visible = False
    End Sub
    ''' <summary>
    ''' Method to Change  the Session to Edit Mode
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub OnEdit()
        Session("PageMode") = "Edit"
    End Sub

    ''' <summary>
    ''' Method to Save and Update DegreeTypes
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub OnSave()
        Dim bsobj As New StudentBAL
        Dim eobj As New StudentEn
        Dim RecAff As Boolean
        lblMsg.Visible = True
        Try
            RecAff = bsobj.UpdateSemester(txtCurrSem.Value, ddlProgram.SelectedValue.ToString(), ddlNewSem.SelectedValue.ToString())
            If RecAff = True Then
                ErrorDescription = "Record Updated Successfully "
                lblMsg.Text = ErrorDescription
                'OnClearData()
            Else
                ErrorDescription = "Update Failed! No Row has been updated..."
                lblMsg.Text = ErrorDescription
            End If
        Catch ex As Exception
            lblMsg.Text = ex.Message.ToString()
            LogError.Log("UpdateSemester", "OnSave", ex.Message)
        End Try
    End Sub
    ''' <summary>
    ''' Method to Delete DegreeTypes
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub OnDelete()
        lblMsg.Visible = True
        'If txtDegreeCode.Text <> "" Then
        '    If lblCount.Text = "" Then lblCount.Text = 0
        '    If lblCount.Text > 0 Then
        '        Dim bsobj As New DegreeTypeBAL
        '        Dim eobj As New DegreeTypeEn
        '        Dim RecAff As Integer
        '        eobj.DegreeTypeCode = txtDegreeCode.Text
        '        Try
        '            RecAff = bsobj.Delete(eobj)
        '            ListObjects = Session("ListObj")
        '            ListObjects.RemoveAt(CInt(txtRecNo.Text) - 1)
        '            lblCount.Text = lblCount.Text - 1
        '            Session("ListObj") = ListObjects
        '            ErrorDescription = "Record Deleted Successfully "
        '            lblMsg.Text = ErrorDescription
        '        Catch ex As Exception
        '            lblMsg.Text = ex.Message.ToString()
        '            LogError.Log("DegreeType", "OnDelete", ex.Message)
        '        End Try
        '        txtDegreeCode.Text = ""
        '        txtDegreeName.Text = ""
        '        txtShortName.Text = ""
        '        ddlStatus.SelectedValue = "1"
        '        DFlag = "Delete"
        '        LoadListObjects()
        '    Else
        '        ErrorDescription = "Select a Record to Delete"
        '        lblMsg.Text = ErrorDescription
        '    End If
        'Else
        '    ErrorDescription = "Select a Record to Delete"
        '    lblMsg.Text = ErrorDescription
        'End If

    End Sub
    ''' <summary>
    ''' Method to Get List of DegreeTypes and Load
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub LoadListObjects()
        Dim ds As New DataSet
        Dim bobj As New StudentBAL
        Dim eobj As New StudentEn

        If ddlCurSem.SelectedIndex = -1 Then
            eobj.CurretSemesterYear = ""
        Else
            eobj.CurretSemesterYear = ddlCurSem.SelectedValue.ToString
        End If

        If ddlProgramType.SelectedIndex = -1 Then
            eobj.ProgramType = ""
        Else
            eobj.ProgramType = ddlProgramType.SelectedValue.ToString
        End If

        If ddlProgram.SelectedIndex = -1 Then
            eobj.ProgramID = ""
        Else
            eobj.ProgramID = ddlProgram.SelectedValue.ToString
        End If

        Try
            ListObjects = bobj.GetListBySemProgTypeProgID(eobj)
        Catch ex As Exception
            LogError.Log("DegreeType", "LoadListObjects", ex.Message)
        End Try

        Session("ListObj") = ListObjects
        lblCount.Text = ListObjects.Count.ToString()

        If ListObjects.Count <> 0 Then
            DisableRecordNavigator()
            txtRecNo.Text = "1"
            PnlView.Visible = True
            PnlAdd.Visible = True
            OnMoveFirst()
            If Session("EditFlag") = True Then
                Session("PageMode") = "Edit"
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
    ''' Method to Load Semester
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub OnLoadSem()
        Dim eobj As New SemesterSetupBAL
        Dim eSemSetup As New SemesterSetupEn
        Try
            eSemSetup.SemisterSetupCode = ""
            eSemSetup.Description = ""
            eSemSetup.Status = True
            trNewSem.Visible = True
            ddlNewSem.Items.Clear()
            ddlNewSem.Items.Add(New ListItem("---Select---", "-1"))
            ddlNewSem.DataTextField = "SemisterCode2"
            ddlNewSem.DataValueField = "SemisterSetupCode"
            Try
                ddlNewSem.DataSource = eobj.GetListSemesterCur(eSemSetup)
            Catch ex As Exception
                LogError.Log("Student", "FillDropDownList", ex.Message)
            End Try
            ddlNewSem.DataBind()
        Catch ex As Exception

        End Try
    End Sub
    ''' <summary>
    ''' Method to Load Current Semester
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub OnLoadCurrSem()
        Dim eobj As New SemesterSetupBAL
        Dim eSemSetup As New SemesterSetupEn
        Try
            eSemSetup.SemisterSetupCode = ""
            eSemSetup.Description = ""
            eSemSetup.Status = True
            ddlCurSem.Items.Clear()
            ddlCurSem.Items.Add(New ListItem("---Select---", "-1"))
            ddlCurSem.DataTextField = "SemisterCode2"
            ddlCurSem.DataValueField = "SemisterSetupCode"
            Try
                ddlCurSem.DataSource = eobj.GetListSemesterCur(eSemSetup)
            Catch ex As Exception
                LogError.Log("Student", "FillDropDownList", ex.Message)
            End Try
            ddlCurSem.DataBind()
        Catch ex As Exception

        End Try
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
        ddlCurSem.SelectedIndex = -1
        ddlNewSem.SelectedIndex = -1
        ddlProgram.SelectedIndex = -1
        ddlProgramType.SelectedIndex = -1
        trNewSem.Visible = False
        trProgram.Visible = False
        trProgramType.Visible = False
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
    ''' Method to Display the list of DegreeTypes
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
        If txtRecNo.Text = 0 Then
            txtRecNo.Text = 1
        Else
            If lblCount.Text = 0 Then
                txtRecNo.Text = 0
            Else
                PnlView.Visible = True
                PnlAdd.Visible = True
                'Dim obj As DegreeTypeEn
                ListObjects = Session("ListObj")
                'obj = ListObjects(RecNo)
                OnLoadSem()
                trNewSem.Visible = True
                trCurrSem.Visible = False
                ibtnDelete.Enabled = False
                ibtnView.Enabled = False
                ibtnPrint.Enabled = False
                ibtnOthers.Enabled = False
                ibtnNext.Enabled = False
                ibtnFirst.Enabled = False
                ibtnLast.Enabled = False
                ibtnPrevs.Enabled = False
                ibtnPosting.Enabled = False
                dgView.Visible = True
                dgView.DataSource = ListObjects
                dgView.DataBind()
            End If
        End If
    End Sub
    ''' <summary>
    ''' Method to get Menu Name
    ''' </summary>
    ''' <param name="MenuId"></param>
    ''' <remarks></remarks>
    Private Sub Menuname(ByVal MenuId As Integer)
        Dim eobj As New MenuEn
        Dim bobj As New MenuBAL
        eobj.MenuId = MenuId

        Try
            eobj = bobj.GetMenus(eobj)
        Catch ex As Exception
            LogError.Log("DegreeType", "Menuname", ex.Message)
        End Try
        lblMenuName.Text = eobj.MenuName
    End Sub
    ''' <summary>
    ''' Method to Load Program Type
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub OnloadProgramType()

        Dim ds As New DataSet
        Dim bobj As New DegreeTypeBAL
        Dim eobj As New DegreeTypeEn
        Dim recStu As Integer

        eobj.DegreeTypeCode = ""
        eobj.Description = ""
        eobj.Status = True
        eobj.SName = ""
        ddlProgramType.Items.Clear()
        ddlProgramType.Items.Add(New ListItem("---Select---", "-1"))
        ddlProgramType.DataTextField = "Description"
        ddlProgramType.DataValueField = "DegreeTypeCode"
        Try
            ddlProgramType.DataSource = bobj.GetDegreeTypeList(eobj)
        Catch ex As Exception
            LogError.Log("Semester", "FillDropDownList", ex.Message)
        End Try
        ddlProgramType.DataBind()

    End Sub
    ''' <summary>
    ''' Method to Load Program
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub OnloadProgram(ByVal DegreeTypeCode As String)

        Dim bobj As New ProgramInfoBAL
        Dim eobj As New DegreeTypeEn

        eobj.DegreeTypeCode = DegreeTypeCode
        ddlProgram.Items.Clear()
        ddlProgram.Items.Add(New ListItem("---Select---", "-1"))
        ddlProgram.DataTextField = "ProgramBM"
        ddlProgram.DataValueField = "ProgramCode"
        Try
            ddlProgram.DataSource = bobj.GetProgramListByDegreeType(eobj)
        Catch ex As Exception
            LogError.Log("Semester", "OnLoadProgram", ex.Message)
        End Try
        ddlProgram.DataBind()

    End Sub
    ''' <summary>
    ''' Method to Display Fields
    ''' </summary>
    ''' <remarks></remarks>
    ''' 
    Private Sub ViewFields()
        trNewSem.Visible = False
        trProgram.Visible = False
        trProgramType.Visible = True
    End Sub

    ''' <summary>
    ''' Method to Display Fields
    ''' </summary>
    ''' <remarks></remarks>
    ''' 
    Private Sub load_fields()
        'ViewFields()
        'OnLoadSem()
        OnloadProgramType()
    End Sub
    Private Overloads Sub OnloadStudent(ByVal sem As String)

    End Sub
#End Region

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

    Protected Sub ddlCurSem_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlCurSem.SelectedIndexChanged
        trProgramType.Visible = True
        txtCurrSem.Value = ddlCurSem.SelectedValue.ToString
        load_fields()
    End Sub

    Protected Sub ddlProgramType_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlProgramType.SelectedIndexChanged
        trProgram.Visible = True
        OnloadProgram(ddlProgramType.SelectedValue.ToString)

        OnLoadSem()
    End Sub
End Class
