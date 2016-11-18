Imports System.Collections.Generic
Imports HTS.SAS.BusinessObjects
Imports HTS.SAS.Entities
Imports System.Data
Partial Class programinfo
    Inherits System.Web.UI.Page
    Private _MaxModule As New MaxModule.CfCommon
    Dim CFlag As String
    Dim DFlag As String
    Dim flag As String
    Dim ListObjects As List(Of ProgramInfoEn)
    Private ErrorDescription As String
    Private FacultyCode As String
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
        If Not IsPostBack() Then
            'Adding validation for save button
            ibtnSave.Attributes.Add("onclick", "return validate()")
            ibtnDelete.Attributes.Add("onclick", "return getconfirm()")
            txtRecNo.Attributes.Add("OnKeyUp", "return geterr()")
            txtTotalSemester.Attributes.Add("onKeypress", "return checknValue()")
            txtSemesterYear.Attributes.Add("onKeypress", "return checknValue()")

            'Added by Zoya 16/2/2016
            txtTotalSemester.Attributes.Add("onKeypress", "return isNumberKey(event)")

            'While loading the page make the CFlag as null
            Session("PageMode") = ""
            'Loading User Rights
            LoadUserRights()
            AddProgram()
            AddBidang()
            ibtnAddFaculty.Attributes.Add("onclick", "new_window=window.open('AddFaculty.aspx','Hanodale','width=480,height=400,resizable=0');new_window.focus();")
            'while loading list object make it nothing
            Session("ListObj") = Nothing
            DisableRecordNavigator()
            'load PageName
            Menuname(CInt(Request.QueryString("Menuid")))
        End If
        lblMsg.Visible = False
        If Not Session("eobjFaculty") Is Nothing Then
            addFaculty()
        End If
    End Sub

    Protected Sub ibtnSave_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ibtnSave.Click
        SpaceValidation()
    End Sub

    Protected Sub ibtnNew_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ibtnNew.Click
        OnAdd()
        lblDesc.Text = ""
        lblDesc.Visible = False
        imgGL.Visible = False
        lblDesc2.Text = ""
        lblDesc2.Visible = False
        Image1.Visible = False
    End Sub

    Protected Sub ibtnDelete_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ibtnDelete.Click
        OnDelete()
        lblMsg.Visible = True
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
    Protected Sub ibtnCancel_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ibtnCancel.Click
        LoadUserRights()
        OnClearData()
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

    Protected Sub btnHidden_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnHidden.Click

    End Sub
#Region "Methods"
    ''' <summary>
    ''' Method to Load Faculty Details
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub addFaculty()
        FacultyCode = ""
        Dim eobjF As FacultyEn
        eobjF = Session("eobjFaculty")
        FacultyCode = eobjF.SAFC_Code
        txtFacultyDesc.Text = eobjF.SAFC_Desc
        txtShortName.Text = eobjF.SAFC_SName
    End Sub

    Private Sub AddProgram()
        Dim eProgram As New DegreeTypeEn
        Dim bProgram As New DegreeTypeBAL

        ddlProgramType.Items.Clear()
        ddlProgramType.Items.Add(New ListItem("---Select---", "-1"))
        ddlProgramType.DataTextField = "Description"
        ddlProgramType.DataValueField = "DegreeTypeCode"
        Try
            ddlProgramType.DataSource = bProgram.GetList(eProgram)
        Catch ex As Exception
            LogError.Log("Student", "FillDropDownList", ex.Message)
        End Try
        ddlProgramType.DataBind()
    End Sub

    Private Sub AddBidang()

        Dim eBidang As New BidangEn
        Dim bBidang As New BidangBAL

        ddlBidang.Items.Clear()
        ddlBidang.Items.Add(New ListItem("---Select---", "-1"))
        ddlBidang.DataTextField = "Description"
        ddlBidang.DataValueField = "BidangCode"
        Try
            ddlBidang.DataSource = bBidang.GetBidangList(eBidang)
        Catch ex As Exception
            LogError.Log("Student", "FillDropDownList", ex.Message)
        End Try

        ddlBidang.DataBind()

    End Sub

    ''' <summary>
    ''' Method to Validate Before Saving
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub SpaceValidation()

        If Trim(txtProgramName.Text).Length = 0 Then

            txtProgramName.Text = Trim(txtProgramName.Text)
            lblMsg.Text = "Enter Valid Program Name "
            lblMsg.Visible = True
            txtProgramName.Focus()
            Exit Sub
        End If
        If Trim(txtPNameBM.Text).Length = 0 Then

            txtPNameBM.Text = Trim(txtPNameBM.Text)
            lblMsg.Text = "Enter Valid Program Description"
            lblMsg.Visible = True
            txtPNameBM.Focus()
            Exit Sub
        End If
        'If Trim(txtFieldStudy.Text).Length = 0 Then

        '    txtFieldStudy.Text = Trim(txtFieldStudy.Text)
        '    lblMsg.Text = "Enter Valid Field Of Study"
        '    lblMsg.Visible = True
        '    txtFieldStudy.Focus()
        '    Exit Sub
        'End If
        'comment by haswati on 01032012
        'If Trim(txtTotalSemester.Text).Length = 0 Then

        'txtTotalSemester.Text = Trim(txtTotalSemester.Text)
        'lblMsg.Text = "Enter Valid Total Semester No"
        'lblMsg.Visible = True
        'txtTotalSemester.Focus()
        '  Exit Sub
        ' End If
        'If Trim(txtSemesterYear.Text).Length = 0 Then

        'txtSemesterYear.Text = Trim(txtSemesterYear.Text)
        ' lblMsg.Text = "Enter Valid  No of Semesters Per Year"
        'lblMsg.Visible = True
        ' txtSemesterYear.Focus()
        ' Exit Sub
        ' End If

        If Trim(txtFacultyDesc.Text).Length = 0 Then

            txtFacultyDesc.Text = Trim(txtFacultyDesc.Text)
            lblMsg.Text = "Enter valid Faculty"
            lblMsg.Visible = False
            txtFacultyDesc.Focus()
            Exit Sub
        End If
        OnSave()
    End Sub
    ''' <summary>
    ''' Method to Load the UserRights
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub LoadUserRights()
        Dim obj As New UsersBAL
        Dim eobj As New UserRightsEn

        'eobj = obj.GetUserRights(5, 1)
        Try
            eobj = obj.GetUserRights(CInt(Request.QueryString("Menuid")), CInt(Session("UserGroup")))
        Catch ex As Exception
            LogError.Log("ProgramInfo", "LoadUserRights", ex.Message)
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
    ''' Method to Clear the Field Values
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub OnClearData()
        txtProgramCode.Enabled = True
        'Clear Text Box values
        Session("ListObj") = Nothing
        DisableRecordNavigator()
        txtProgramCode.Text = ""
        txtProgramName.Text = ""
        txtPNameBM.Text = ""
        txtprogramSName.Text = ""
        txtTotalSemester.Text = ""
        txtSemesterYear.Text = ""
        txtProgramDesc.Text = ""
        txtTuitionFeeAcc.Text = ""
        txtDebtorAcc.Text = ""
        txtFacultyDesc.Text = ""
        txtShortName.Text = ""
        txtAccDesc.Text = ""
        txtTuitionFeeDesc.Text = ""
        ddlStatus.SelectedValue = "1"
        ddlProgramType.SelectedIndex = 0
        'txtFieldStudy.Text = ""
        Session("PageMode") = "Add"
    End Sub
    ''' <summary>
    ''' Method to Load Fields in Add Mode
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
    ''' 
    Private Sub OnEdit()
        Session("PageMode") = "Edit"
        txtProgramCode.Enabled = False

    End Sub
    ''' <summary>
    ''' Method to Save and Update ProgramInfo 
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub OnSave()

        Dim bsobj As New ProgramInfoBAL
        Dim eobj As New ProgramInfoEn
        Dim eobjf As New FacultyEn
        Dim RecAff As Integer
        Dim bsfaculty As New FacultyBAL

        Dim LstACC As New List(Of ProgramAccountEn)
        If Trim(txtTotalSemester.Text) = "" Then txtTotalSemester.Text = 0
        If Trim(txtSemesterYear.Text) = "" Then txtSemesterYear.Text = 0
        eobj.ProgramCode = Trim(txtProgramCode.Text)
        eobj.ProgramType = ddlProgramType.SelectedValue
        eobj.Program = Trim(txtProgramName.Text)
        eobj.ProgramBM = Trim(txtPNameBM.Text)
        'eobj.FieldStudy = Trim(txtFieldStudy.Text)
        eobj.SName = Trim(txtprogramSName.Text)
        eobj.TotalSem = Trim(txtTotalSemester.Text)
        eobj.SemByYear = Trim(txtSemesterYear.Text)
        eobj.Description = Trim(txtProgramDesc.Text)
        eobj.UpdatedBy = Session("User")
        eobj.UpdatedDtTm = System.DateTime.Now.ToShortDateString()
        If ddlStatus.SelectedValue = 0 Then
            eobj.Status = False
        Else
            eobj.Status = True
        End If
        eobj.Tutionacc = Trim(txtTuitionFeeAcc.Text)
        eobj.TutionDes = Trim(txtTuitionFeeDesc.Text)
        eobj.Accountinfo = Trim(txtDebtorAcc.Text)
        eobj.AccountDes = Trim(txtAccDesc.Text)
        eobj.Code = FacultyCode
        eobj.BidangCode = ddlBidang.SelectedValue

        lblMsg.Visible = True
        If Session("PageMode") = "Add" Then
            Try
                RecAff = bsobj.Insert(eobj)
                ErrorDescription = "Record Saved Successfully "
                lblMsg.Text = ErrorDescription

            Catch ex As Exception
                lblMsg.Text = ex.Message.ToString()
                LogError.Log("ProgramInfo", "OnSave", ex.Message)
            End Try
        ElseIf Session("PageMode") = "Edit" Then
            Try
                'eobjf.SAFC_SName = txtShortName.Text
                eobjf.SAFC_Desc = txtFacultyDesc.Text
                eobjf = bsfaculty.GetItem(eobjf)
                eobj.Code = eobjf.SAFC_Code
                RecAff = bsobj.Update(eobj)
                ListObjects = Session("ListObj")
                ListObjects(CInt(txtRecNo.Text) - 1) = eobj
                Session("ListObj") = ListObjects
                ErrorDescription = "Record Updated Successfully "
                lblMsg.Text = ErrorDescription

            Catch ex As Exception
                lblMsg.Text = ex.Message.ToString()
                LogError.Log("ProgramInfo", "OnSave", ex.Message)
            End Try
        End If

    End Sub
    ''' <summary>
    ''' Method to Delete the ProgramInfo
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub OnDelete()
        lblMsg.Visible = True
        If txtProgramCode.Text <> "" Then
            If lblCount.Text = "" Then lblCount.Text = 0
            If lblCount.Text > 0 Then

                Dim bsobj As New ProgramInfoBAL
                Dim eobj As New ProgramInfoEn
                Dim RecAff As Integer
                eobj.ProgramCode = txtProgramCode.Text
                lblMsg.Visible = True
                Try
                    RecAff = bsobj.Delete(eobj)
                    ListObjects = Session("ListObj")
                    ListObjects.RemoveAt(CInt(txtRecNo.Text) - 1)
                    lblCount.Text = lblCount.Text - 1
                    Session("ListObj") = ListObjects
                    lblMsg.Visible = True
                    ErrorDescription = "Record Deleted Successfully "
                    lblMsg.Text = ErrorDescription
                    DFlag = "Delete"
                Catch ex As Exception
                    lblMsg.Visible = True
                    lblMsg.Text = ex.Message.ToString()
                    LogError.Log("ProgramInfo", "OnDelete", ex.Message)
                    flag = "in use"

                End Try
                txtProgramCode.Text = ""
                txtProgramName.Text = ""
                txtPNameBM.Text = ""
                'txtFieldStudy.Text = ""
                txtprogramSName.Text = ""
                txtTotalSemester.Text = ""
                txtSemesterYear.Text = ""
                txtProgramDesc.Text = ""
                txtFacultyDesc.Text = ""
                txtDebtorAcc.Text = ""
                txtTuitionFeeAcc.Text = ""
                txtShortName.Text = ""
                txtAccDesc.Text = ""
                txtTuitionFeeDesc.Text = ""
                ddlStatus.SelectedValue = "1"

                LoadListObjects()
                ' End If
            Else
                ErrorDescription = "Select a Record to Delete"
                lblMsg.Text = ErrorDescription
            End If
        Else
            ErrorDescription = "Select a Record to Delete"
            lblMsg.Text = ErrorDescription
        End If
        If flag = "in use" Then
            lblMsg.Visible = True
            lblMsg.Text = "Record Already in Use"
        End If

    End Sub
    ''' <summary>
    ''' Method to Load Faculty Dropdown
    ''' </summary>
    ''' <param name="Fcode">Fcode as Input.</param>
    ''' <remarks></remarks>
    Private Sub loadddlFaculty(ByVal Fcode As String)

        Dim bobj As New FacultyBAL
        Dim eobj As New FacultyEn
        eobj.SAFC_Code = Fcode
        Dim listFaculty As New List(Of FacultyEn)

        Try
            listFaculty = bobj.GetList(eobj)
        Catch ex As Exception
            LogError.Log("ProgramInfo", "loadddlFaculty", ex.Message)
        End Try
        If listFaculty.Count = 0 Then
            txtFacultyDesc.Text = ""
            txtShortName.Text = ""
        Else
            eobj = listFaculty(0)
            txtFacultyDesc.Text = eobj.SAFC_Desc
            txtShortName.Text = eobj.SAFC_SName
        End If

    End Sub
    ''' <summary>
    ''' Method to get the List Of ProgramInfo
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub LoadListObjects()
        Dim ds As New DataSet
        Dim bobj As New ProgramInfoBAL
        Dim recStu As Integer
        Dim eobj As New ProgramInfoEn
        If ddlStatus.SelectedValue = -1 Then
            recStu = -1
        Else
            recStu = ddlStatus.SelectedValue
        End If
        eobj.ProgramCode = Trim(txtProgramCode.Text)
        eobj.Program = Trim(txtProgramName.Text)
        eobj.ProgramBM = Trim(txtPNameBM.Text)
        'eobj.FieldStudy = Trim(txtFieldStudy.Text)
        eobj.Status = ddlStatus.SelectedValue
        If ddlProgramType.SelectedValue = "-1" Then
            eobj.ProgramType = ""
        Else
            eobj.ProgramType = ddlProgramType.SelectedValue
        End If
        eobj.Faculty = ""
        Try
            ListObjects = bobj.GetProgramInfoList(eobj)
        Catch ex As Exception
            LogError.Log("ProgramInfo", "LoadListObjects", ex.Message)
        End Try
        Session("ListObj") = ListObjects
        lblCount.Text = ListObjects.Count.ToString()

        If ListObjects.Count <> 0 Then
            DisableRecordNavigator()
            txtRecNo.Text = "1"
            PnlView.Visible = False
            PnlAdd.Visible = True
            OnMoveFirst()
            If Session("EditFlag") = True Then
                Session("PageMode") = "Edit"
                txtProgramCode.Enabled = False
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
            ElseIf flag = "in use" Then

                lblMsg.Text = "Record Already in Use"
                lblMsg.Visible = True
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
        lblDesc.Text = ""
        imgGL.Visible = False
        lblDesc2.Text = ""
        Image1.Visible = False
        FillData(0)
    End Sub
    ''' <summary>
    ''' Method to Move to Next Record
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub OnMoveNext()
        txtRecNo.Text = CInt(txtRecNo.Text) + 1
        lblDesc.Text = ""
        imgGL.Visible = False
        lblDesc2.Text = ""
        Image1.Visible = False
        FillData(CInt(txtRecNo.Text) - 1)
    End Sub
    ''' <summary>
    ''' Method to Move to Previous Record
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub OnMovePrevious()
        txtRecNo.Text = CInt(txtRecNo.Text) - 1
        lblDesc.Text = ""
        imgGL.Visible = False
        lblDesc2.Text = ""
        Image1.Visible = False
        FillData(CInt(txtRecNo.Text) - 1)
    End Sub
    ''' <summary>
    ''' Method to Move to Last Record
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub OnMoveLast()
        txtRecNo.Text = lblCount.Text
        lblDesc.Text = ""
        imgGL.Visible = False
        lblDesc2.Text = ""
        Image1.Visible = False
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
                Dim obj As ProgramInfoEn
                ListObjects = Session("ListObj")
                obj = ListObjects(RecNo)
                Dim i As Integer = 0
                txtProgramCode.Text = obj.ProgramCode
                ddlProgramType.SelectedValue = obj.ProgramType
                txtProgramName.Text = obj.Program
                txtPNameBM.Text = obj.ProgramBM
                'txtFieldStudy.Text = obj.FieldStudy
                txtprogramSName.Text = obj.SName
                txtTotalSemester.Text = obj.TotalSem
                txtSemesterYear.Text = obj.SemByYear
                txtProgramDesc.Text = obj.Description
                txtFacultyDesc.Text = obj.SAFC_Desc
                txtShortName.Text = obj.SAFC_Code
                txtDebtorAcc.Text = obj.Accountinfo
                txtTuitionFeeAcc.Text = obj.Tutionacc
                txtAccDesc.Text = obj.AccountDes
                txtTuitionFeeDesc.Text = obj.TutionDes
                If obj.Status = True Then
                    ddlStatus.SelectedValue = 1
                Else
                    ddlStatus.SelectedValue = 0
                End If
                ddlBidang.SelectedValue = obj.BidangCode
                loadddlFaculty(obj.Code)

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
            LogError.Log("ProgramInfo", "Menuname", ex.Message)
        End Try
        lblMenuName.Text = eobj.MenuName
    End Sub

#End Region

    Protected Sub Check_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Check.Click
        'varaible declaration
        Dim result As Boolean = False
        Dim _GLCode As String = txtDebtorAcc.Text
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
                    lblDesc.Text = "Invalid GLCode"
                    imgGL.Visible = True
                    imgGL.ImageUrl = "~/images/cross.png"
                End If
                'Check GLCODE in CF - Ended
            End If
            'Check Empty GLCode - Starting            

        Catch ex As Exception
            Throw ex
        End Try

    End Sub

    Protected Sub Check2_Click(sender As Object, e As EventArgs) Handles Check2.Click
        'varaible declaration
        Dim result As Boolean = False
        Dim _GLCode As String = txtTuitionFeeAcc.Text
        Dim _GLLedgerType As String = Nothing
        Dim _GLDescription As String = Nothing

        Try
            'imgGL.Visible = False
            Image1.Visible = False
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
                        lblDesc2.Text = _GLDescription
                        Image1.Visible = True
                        Image1.ImageUrl = "~/images/check.png"
                    End If
                Else
                    lblDesc2.Text = "Invalid GLCode"
                    Image1.Visible = True
                    Image1.ImageUrl = "~/images/cross.png"
                End If
                'Check GLCODE in CF - Ended
            End If
            'Check Empty GLCode - Starting            

        Catch ex As Exception
            Throw ex
        End Try
    End Sub
End Class
