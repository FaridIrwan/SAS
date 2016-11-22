Imports HTS.SAS.BusinessObjects
Imports HTS.SAS.DataAccessObjects
Imports HTS.SAS.Entities
Imports System.Data
Imports System.Collections.Generic
Imports Microsoft.VisualBasic
Imports System.Linq
Imports System.Data.Linq

Partial Class Feetype
    Inherits System.Web.UI.Page

#Region "Global Declarations "
    'declare instant
    Private _GstSetupDal As New HTS.SAS.DataAccessObjects.GSTSetupDAL
    Private _MaxModule As New MaxModule.CfCommon
#End Region

    Private ErrorDescription As String
    Dim DFlag As String
    Dim ListObjects As List(Of FeeTypesEn)

    ''Private LogErrors As LogError
    Protected Sub ibtnNew_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ibtnNew.Click
        OnAdd()
    End Sub

    Protected Sub ibtnSave_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ibtnSave.Click
        SpaceValidation()
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack() Then
            'Adding validation for save button
            ibtnSave.Attributes.Add("onclick", "return validate()")
            ibtnDelete.Attributes.Add("onclick", "return getconfirm()")
            txtRecNo.Attributes.Add("OnKeyUp", "return geterr()")
            txtPriority.Attributes.Add("onKeypress", "return isNumberKey(event)")
            'Load TaxType
            LoadTaxType()
            'Loading User Rights
            Session("PageMode") = "Add"
            LoadUserRights()
            'while loading list object make it nothing
            Session("ListObj") = Nothing
            DisableRecordNavigator()
            'load PageName
            Menuname(CInt(Request.QueryString("Menuid")))
            LoadFacultyWithGlAccount()
            LoadKolejWithGlAccount()
            pnlFaculty.Visible = False
            pnlKolej.Visible = False
        End If
        lblMsg.Text = ""
        lblMsg.Visible = False
    End Sub

    Protected Sub ibtnDelete_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ibtnDelete.Click
        onDelete()
    End Sub

    Protected Sub ibtnView_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ibtnView.Click
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

    Protected Sub dgView_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dgView.ItemDataBound
        Dim txtAmount As TextBox

        'Select Case e.Item.ItemType
        '    Case ListItemType.Item, ListItemType.AlternatingItem
        '        txtAmount = CType(e.Item.FindControl("txtFeeAmount"), TextBox)
        '        txtAmount.Attributes.Add("onKeyPress", "checkValue();")
        '        Dim amount As Double
        '        amount = MaxGeneric.clsGeneric.NullToDecimal(txtAmount.Text)
        '        txtAmount.Text = String.Format("{0:F}", amount)
        'End Select
        'modified by Zoya 16/2/2016
        Select Case e.Item.ItemType
            Case ListItemType.Item, ListItemType.AlternatingItem
                txtAmount = CType(e.Item.FindControl("txtFeeAmount"), TextBox)
                txtAmount.Attributes.Add("onKeyPress", "isNumberKey(event);")
                Dim amount As Double
                amount = MaxGeneric.clsGeneric.NullToDecimal(txtAmount.Text)
                txtAmount.Text = String.Format("{0:F}", amount)
        End Select

    End Sub

    Protected Sub ibtnCancel_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ibtnCancel.Click
        LoadUserRights()
        OnClearData()
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
    ''' Method to Fill the Field Values
    ''' </summary>
    ''' <param name="RecNo"></param>
    ''' <remarks></remarks>
    Private Sub FillData(ByVal RecNo As Integer)
        Dim _ListFeeCharge As New List(Of FeeChargesEn)
        lblMsg.Visible = False
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
        pnlView.Visible = True
        PnlAdd.Visible = True
        If txtRecNo.Text = 0 Then
            txtRecNo.Text = 1
        Else
            If lblCount.Text = 0 Then
                txtRecNo.Text = 0
            Else
                Dim obj As FeeTypesEn
                ListObjects = Session("ListObj")
                obj = ListObjects(RecNo)

                txtFeeCode.Text = obj.FeeTypeCode
                txtFeeDesc.Text = obj.Description
                ddlFeeType.SelectedValue = obj.FeeType
                ddlFeeCategory.SelectedValue = obj.Hostel
                ddlFeeCategory.Enabled = False
                txtPriority.Text = obj.Priority
                txtFeeRemarks.Text = obj.Remarks
                If Not obj.TaxId = 0 Then
                    ddlTaxType.SelectedValue = obj.TaxId
                Else
                    ddlTaxType.SelectedValue = -1
                End If
                ddlTaxType.Enabled = False

                If Not txtFeeCode.Text = "" Then
                    If Not ListObjects.Where(Function(x) x.FeeTypeCode = txtFeeCode.Text).FirstOrDefault() Is Nothing Then
                        _ListFeeCharge = ListObjects.Where(Function(x) x.FeeTypeCode = txtFeeCode.Text).FirstOrDefault().ListFeeCharges

                    End If
                Else

                End If
                dgView.DataSource = _ListFeeCharge
                dgView.DataBind()

                'load faculty/kolej grid - start
                BindFacultyGrid()
                BindKolejGrid()
                'load faculty/kolej grid - end

                If obj.IsTutionFee = 1 Then
                    chkTutionFee.Checked = True
                Else
                    chkTutionFee.Checked = False
                End If

                If obj.IsChangeProgram = 1 Then
                    chkChangeProg.Checked = True
                Else
                    chkChangeProg.Checked = False
                End If

                If obj.Status = True Then
                    ddlStatus.SelectedValue = 1
                Else
                    ddlStatus.SelectedValue = 0
                End If
            End If
        End If
    End Sub
    ''' <summary>
    ''' Method to Validate Before Save
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub SpaceValidation()
        If Trim(txtFeeDesc.Text).Length = 0 Then

            txtFeeDesc.Text = Trim(txtFeeDesc.Text)
            lblMsg.Text = "Enter valid Fee Description "
            lblMsg.Visible = True
            txtFeeDesc.Focus()
            Exit Sub
        End If
        If Trim(txtPriority.Text).Length = 0 Then

            txtPriority.Text = Trim(txtPriority.Text)
            lblMsg.Text = "Enter valid Fee Priority "
            lblMsg.Visible = True
            txtPriority.Focus()
            Exit Sub
        End If

        'added by Hafiz @ 16/11/2016 - Faculty/Kolej Gl Checking START
        If ddlFeeCategory.SelectedValue = "A" Or ddlFeeCategory.SelectedValue = "T" Then
            If FacultyGLAccChecking() = False Then
                Exit Sub
            End If
        ElseIf ddlFeeCategory.SelectedValue = "H" Then
            If KolejGLAccChecking() = False Then
                Exit Sub
            End If
        End If
        'added by Hafiz @ 16/11/2016 - Faculty/Kolej Gl Checking END

        If ddlTaxType.SelectedValue = -1 Then
            lblMsg.Visible = True
            lblMsg.Text = "Enter Valid Tax Type"
            ddlTaxType.Focus()
            Exit Sub
        End If

        onSave()

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
            LogError.Log("FeeType", "Menuname", ex.Message)
        End Try
        lblMenuName.Text = eobj.MenuName
    End Sub
    ''' <summary>
    ''' Method to Load Fields in New Mode
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub OnAdd()
        Session("PageMode") = "Add"
        PnlAdd.Visible = True
        OnClearData()
        LoadGrid(ListObjects)
    End Sub
    ''' <summary>
    ''' Method to Save and Update FeeTypes
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub onSave()
        lblMsg.Visible = True
        Dim txtAmount As TextBox
        Dim _txtGSTAmount As TextBox
        Dim _txtTotFeeAmount As TextBox
        Dim dgItem1 As DataGridItem
        Dim eobj As New FeeTypesEn
        Dim objfee As New FeeChargesEn
        Dim bsobj As New FeeTypesBAL
        Dim LstFee As New List(Of FeeChargesEn)
        Dim LstFacGlAcc As New List(Of FacultyGLAccEn)
        Dim LstKolGlAcc As New List(Of KolejGLAccEn)
        Dim RecAff As Integer

        If Trim(txtPriority.Text) = "" Then txtPriority.Text = 0

        eobj.FeeTypeCode = Trim(txtFeeCode.Text)
        eobj.Description = Trim(txtFeeDesc.Text)
        eobj.Hostel = ddlFeeCategory.SelectedValue
        eobj.FeeType = ddlFeeType.SelectedValue
        eobj.Priority = Trim(txtPriority.Text)
        eobj.TaxId = ddlTaxType.SelectedValue

        If ddlFeeCategory.SelectedValue = "T" Then
            eobj.IsTutionFee = 1
        Else
            eobj.IsTutionFee = 0
        End If

        If (chkChangeProg.Checked) Then
            eobj.IsChangeProgram = 1
        Else
            eobj.IsChangeProgram = 0
        End If

        If ddlStatus.SelectedValue = 0 Then
            eobj.Status = False
        Else
            eobj.Status = True
        End If
        eobj.Remarks = Trim(txtFeeRemarks.Text)
        eobj.UpdatedBy = Session("User")

        'Loop the Data Grid
        Dim i As Integer = 0
        For Each dgItem1 In dgView.Items
            txtAmount = dgItem1.FindControl("txtFeeAmount")
            _txtTotFeeAmount = dgItem1.FindControl("txtTotFeeAmount")
            _txtGSTAmount = dgItem1.FindControl("txtGSTAmount")
            objfee = New FeeChargesEn()
            objfee.FTCode = txtFeeCode.Text
            objfee.SCCode = dgItem1.Cells(0).Text
            objfee.GSTAmount = _txtGSTAmount.Text
            
            If txtAmount.Text = "" Then
                lblMsg.Visible = True
                lblMsg.Text = "Amount Cannot be Empty"
                Exit Sub
            ElseIf txtAmount.Text = 0 Then
                lblMsg.Visible = True
                lblMsg.Text = "Amount Cannot be Zero"
                Exit Sub
            Else
                objfee.FSAmount = _txtTotFeeAmount.Text
            End If

            LstFee.Add(objfee)
            objfee = Nothing
            txtAmount = Nothing
        Next

        eobj.ListFeeCharges = LstFee

        'added by Hafiz @ 16/11/2016 - Faculty/Kolej Gl Acc START
        If ddlFeeCategory.SelectedValue = "A" Or ddlFeeCategory.SelectedValue = "T" Then

            For Each dg As DataGridItem In dgFaculty.Items
                Dim en As New FacultyGLAccEn
                Dim _TextBoxGLAcc As TextBox = dg.FindControl("txtGLAcc")
                Dim constr As String() = dg.Cells(0).Text.Split("-")

                en.SAFT_Code = eobj.FeeTypeCode
                en.SAFC_Code = Trim(constr(0))
                en.SAFC_Desc = Trim(constr(1))
                en.GL_Account = Trim(_TextBoxGLAcc.Text)

                LstFacGlAcc.Add(en)
            Next

            eobj.LstFacultyGL = LstFacGlAcc

        ElseIf ddlFeeCategory.SelectedValue = "H" Then

            For Each dg As DataGridItem In dgKolej.Items
                Dim en As New KolejGLAccEn
                Dim _TextBoxGLAcc As TextBox = dg.FindControl("txtGLAccKolej")
                Dim constr As String() = dg.Cells(0).Text.Split("-")

                en.SAFT_Code = eobj.FeeTypeCode
                en.SAKO_Code = Trim(constr(0))
                en.SAKO_Description = Trim(constr(1))
                en.GL_Account = Trim(_TextBoxGLAcc.Text)

                LstKolGlAcc.Add(en)
            Next

            eobj.LstKolejGL = LstKolGlAcc

        End If
        'added by Hafiz @ 16/11/2016 - Faculty/Kolej Gl Acc END

        If Session("PageMode") = "Add" Then
            Try
                RecAff = bsobj.Insert(eobj)
                ErrorDescription = "Record Saved Successfully "
                lblMsg.Text = ErrorDescription
                LoadListObjects()
            Catch ex As Exception
                lblMsg.Text = ex.Message.ToString()
                LogError.Log("FeeType", "onSave", ex.Message)
            End Try
        ElseIf Session("PageMode") = "Edit" Then
            Try
                RecAff = bsobj.Update(eobj)
                ListObjects = Session("ListObj")
                ListObjects(CInt(txtRecNo.Text) - 1) = eobj
                Session("ListObj") = ListObjects
                ErrorDescription = "Record Updated Successfully "
                lblMsg.Text = ErrorDescription

                For Each dg As DataGridItem In dgFaculty.Items
                    Dim _Image As Image = dg.FindControl("imgGL")
                    _Image.Visible = False
                Next

                For Each dg As DataGridItem In dgKolej.Items
                    Dim _Image As Image = dg.FindControl("imgGLKolej")
                    _Image.Visible = False
                Next
            Catch ex As Exception
                lblMsg.Text = ex.Message.ToString()
                LogError.Log("FeeType", "onSave", ex.Message)
            End Try
        End If
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
    ''' Method to get the List Of FeeTypes
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub LoadListObjects()
        Dim ds As New DataSet
        Dim bobj As New FeeTypesBAL
        Dim eobj As New FeeTypesEn
        Dim recStu As Integer
        Dim isTutionFee As Integer
        Dim isChangeProg As Integer
        Dim feeType As String
        Dim feeCat As String

        If ddlStatus.SelectedValue = -1 Then
            recStu = -1
        Else
            recStu = ddlStatus.SelectedValue
        End If

        If ddlFeeType.SelectedValue = -1 Then
            feeType = ""
        Else
            feeType = ddlFeeType.SelectedValue
        End If

        If ddlFeeCategory.SelectedValue = "-1" Then
            feeCat = ""
        Else
            feeCat = ddlFeeCategory.SelectedValue
        End If

        If chkTutionFee.Checked Then
            isTutionFee = 1
        Else
            isTutionFee = 0
        End If

        'New RA
        If chkChangeProg.Checked Then
            isChangeProg = 1
        Else
            isChangeProg = 0
        End If

        eobj.IsTutionFee = isTutionFee
        eobj.IsChangeProgram = isChangeProg
        eobj.FeeTypeCode = Trim(txtFeeCode.Text)
        eobj.FeeType = feeType
        eobj.Hostel = feeCat
        If txtPriority.Text.Length <> 0 Then
            eobj.Priority = CInt(txtPriority.Text)
        End If
        eobj.Description = Trim(txtFeeDesc.Text)
        eobj.Status = ddlStatus.SelectedValue

        Try
            ListObjects = bobj.GetFeeTypesList(eobj)
        Catch ex As Exception
            LogError.Log("FeeType", "LoadListObjects", ex.Message)
        End Try

        Session("ListObj") = ListObjects
        lblCount.Text = ListObjects.Count.ToString()
        If ListObjects.Count <> 0 Then
            DisableRecordNavigator()
            txtRecNo.Text = "1"
            pnlView.Visible = True
            PnlAdd.Visible = True
            OnMoveFirst()

            LoadGrid(ListObjects)
            If Session("EditFlag") = True Then
                Session("PageMode") = "Edit"
                txtFeeCode.Enabled = False
                ibtnSave.Enabled = True
                ibtnSave.ImageUrl = "images/save.png"
                lblMsg.Visible = True

            Else
                ibtnSave.Enabled = False
                ibtnSave.ImageUrl = "images/gsave.png"
                Session("PageMode") = ""

            End If
        Else
            txtRecNo.Text = ""
            lblCount.Text = ""
            Session("PageMode") = "Add"
            txtFeeCode.Enabled = True
            'Clear Text Box values
            Session("ListObj") = Nothing
            DisableRecordNavigator()
            txtFeeCode.Text = ""
            txtFeeDesc.Text = ""
            txtPriority.Text = ""
            txtFeeRemarks.Text = ""
            ddlFeeType.SelectedValue = "-01"
            ddlFeeCategory.SelectedValue = "-1"
            ddlFeeCategory.Enabled = True
            ddlTaxType.Enabled = True
            LoadGrid(ListObjects)
            ddlStatus.SelectedValue = "1"
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
    ''' Method to Delete the FeeTypes
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub onDelete()
        lblMsg.Visible = True
        If txtFeeCode.Text <> "" Then
            If lblCount.Text = "" Then lblCount.Text = 0
            If lblCount.Text > 0 Then

                Dim bsobj As New FeeTypesBAL
                Dim eobj As New FeeTypesEn
                lblMsg.Visible = True
                eobj.FeeTypeCode = txtFeeCode.Text
                Try
                    bsobj.FeeDelete(eobj)
                    ListObjects = Session("ListObj")
                    ListObjects.RemoveAt(CInt(txtRecNo.Text) - 1)
                    lblCount.Text = lblCount.Text - 1
                    Session("ListObj") = ListObjects
                    lblMsg.Visible = True
                    ErrorDescription = "Record Deleted Successfully "
                    lblMsg.Text = ErrorDescription
                Catch ex As Exception
                    lblMsg.Text = ex.Message.ToString()
                    LogError.Log("FeeType", "onDelete", ex.Message)

                End Try
                txtFeeCode.Text = ""
                txtFeeDesc.Text = ""
                txtPriority.Text = ""
                txtFeeRemarks.Text = ""
                ddlFeeType.SelectedValue = "-01"
                ddlFeeCategory.SelectedValue = "-1"
                ddlFeeCategory.Enabled = True
                ddlStatus.SelectedValue = "1"
                ddlTaxType.Enabled = True
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
    ''' Method to Load the UserRights
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub LoadUserRights()
        Dim obj As New UsersBAL
        Dim eobj As New UserRightsEn

        Try
            eobj = obj.GetUserRights(CInt(Request.QueryString("Menuid")), CInt(Session("UserGroup")))

        Catch ex As Exception
            LogError.Log("FeeType", "LoadUserRights", ex.Message)
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
        ibnPrint.Enabled = eobj.IsPrint
        If eobj.IsPrint = True Then
            ibnPrint.Enabled = True
            ibnPrint.ImageUrl = "images/print.png"
            ibnPrint.ToolTip = "Print"
        Else
            ibnPrint.Enabled = False
            ibnPrint.ImageUrl = "images/gprint.png"
            ibnPrint.ToolTip = "Access Denied"
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
    ''' Method to Load Grid With FeeCharges
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub LoadGrid(_ListObjects As List(Of FeeTypesEn))
        'ListObjects As List(Of FeeTypesEn)
        Dim ds As New DataSet
        Dim bobj As New StudentCategoryBAL
        Dim eobj As New StudentCategoryEn
        Dim eobjStuCat As StudentCategoryEn
        Dim eobjFeeCharge As FeeChargesEn

        Dim ListStudentCategory As New List(Of StudentCategoryEn)
        Dim ListFeeCharge As New List(Of FeeChargesEn)
        Dim i As Integer = 0
        eobj.StudentCategoryCode = ""
        eobj.Description = ""
        eobj.Status = True
        Try
            ListStudentCategory = bobj.GetStudentCategoryList(eobj)

        Catch ex As Exception
            LogError.Log("FeeType", "LoadGrid", ex.Message)
        End Try
        If Not txtFeeCode.Text = "" Then
            '_ListObjects.Find(Function(id) id.FeeTypeCode = txtFeeCode.Text)
            If Not _ListObjects.Where(Function(x) x.FeeTypeCode = txtFeeCode.Text).FirstOrDefault() Is Nothing Then
                ListFeeCharge = _ListObjects.Where(Function(x) x.FeeTypeCode = txtFeeCode.Text).FirstOrDefault().ListFeeCharges
            End If
        Else
            'Loading feeCharges
            While i < ListStudentCategory.Count
                eobjStuCat = ListStudentCategory(i)
                eobjFeeCharge = New FeeChargesEn
                eobjFeeCharge.SCCode = eobjStuCat.StudentCategoryCode
                eobjFeeCharge.SCDesc = eobjStuCat.Description
                ' eobjFeeCharge.GSTAmount = eobjFeeCharge.GSTAmount
                eobjFeeCharge.FSAmount = 0
                ListFeeCharge.Add(eobjFeeCharge)
                eobjFeeCharge = Nothing
                eobjStuCat = Nothing
                i = i + 1
            End While
        End If
        pnlView.Visible = True
        PnlAdd.Visible = True

        dgView.DataSource = ListFeeCharge
        dgView.DataBind()

        'load faculty/kolej grid - start
        BindFacultyGrid()
        BindKolejGrid()
        'load faculty/kolej grid - end

    End Sub
    ''' <summary>
    ''' Method to Clear the Field Values
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub OnClearData()
        Session("PageMode") = "Add"
        txtFeeCode.Enabled = True
        'Clear Text Box values
        Session("ListObj") = Nothing
        DisableRecordNavigator()
        txtFeeCode.Text = ""
        txtFeeDesc.Text = ""
        txtPriority.Text = ""
        txtFeeRemarks.Text = ""
        ddlFeeType.Text = "-01"
        ddlFeeCategory.SelectedValue = "-1"
        ddlFeeCategory.Enabled = True
        LoadGrid(ListObjects)
        lblMsg.Text = ""
        ddlStatus.SelectedValue = "1"
        ddlTaxType.SelectedValue = "-1"
        ddlTaxType.Enabled = True
        chkTutionFee.Checked = False
        chkChangeProg.Checked = False

        For Each dg As DataGridItem In dgFaculty.Items
            Dim tbGL As TextBox = dg.FindControl("txtGLAcc")
            Dim imgGL As Image = dg.FindControl("imgGL")
            tbGL.Text = ""
            imgGL.Visible = False
        Next

        For Each dg As DataGridItem In dgKolej.Items
            Dim tbGL As TextBox = dg.FindControl("txtGLAccKolej")
            Dim imgGL As Image = dg.FindControl("imgGLKolej")
            tbGL.Text = ""
            imgGL.Visible = False
        Next

        pnlFaculty.Visible = False
        pnlKolej.Visible = False
    End Sub
#End Region

#Region "GST Function "
    Public Function GSTFunc(ByVal Amt As Double, ByVal gst As Double) As String
        Dim TaxMode As Integer = 0
        Try
            TaxMode = _GstSetupDal.GetGstDetails(MaxGeneric.clsGeneric.NullToInteger(ddlTaxType.SelectedValue)).Tables(0).Rows(0)(3).ToString()
        Catch

        End Try
        Dim ActAmout As Double = 0
        If (TaxMode = Generic._TaxMode.Inclusive) Then
            ActAmout = MaxGeneric.clsGeneric.NullToDecimal(Amt) - gst
        ElseIf (TaxMode = Generic._TaxMode.Exclusive) Then
            ActAmout = MaxGeneric.clsGeneric.NullToDecimal(Amt) - gst
        End If
        Return ActAmout
    End Function
#End Region

#Region "GST Function2 "
    Public Function GSTFunc2(ByVal Amt As Double, ByVal gst As Double) As String
        Dim TaxMode As Integer = 0
        Try
            TaxMode = _GstSetupDal.GetGstDetails(MaxGeneric.clsGeneric.NullToInteger(ddlTaxType.SelectedValue)).Tables(0).Rows(0)(3).ToString()
        Catch

        End Try
        Dim ActAmout As Double = 0
        If (TaxMode = Generic._TaxMode.Inclusive) Then
            ActAmout = MaxGeneric.clsGeneric.NullToDecimal(Amt)
        ElseIf (TaxMode = Generic._TaxMode.Exclusive) Then
            ActAmout = MaxGeneric.clsGeneric.NullToDecimal(Amt) - gst
        End If
        Return ActAmout
    End Function
#End Region

#Region "GST Calculation - Starting "
    Protected Sub txtFeeAmount_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs)

        'control declaration
        Dim dgitem As DataGridItem
        Dim _txtFeeAmount As TextBox
        Dim _txtGSTAmount As TextBox
        Dim _txtTotFeeAmount As TextBox
        Dim _txtActFeeAmount As TextBox

        'varaible declaration
        Dim FeeAmount As Double, GSTAmt As Double, ActAmout As Double
        Dim TaxMode As String
        Try

            TaxMode = _GstSetupDal.GetGstDetails(MaxGeneric.clsGeneric.NullToInteger(ddlTaxType.SelectedValue)).Tables(0).Rows(0)(3).ToString()

            'GST Calculation - Stating
            For Each dgitem In dgView.Items
                _txtFeeAmount = dgitem.FindControl("txtFeeAmount")
                _txtGSTAmount = dgitem.FindControl("txtGSTAmount")
                _txtTotFeeAmount = dgitem.FindControl("txtTotFeeAmount")
                _txtActFeeAmount = dgitem.FindControl("txtActFeeAmount")

                FeeAmount = _txtFeeAmount.Text
                GSTAmt = _GstSetupDal.GetGstAmount(ddlTaxType.SelectedValue, MaxGeneric.clsGeneric.NullToDecimal(FeeAmount))


                If (TaxMode = Generic._TaxMode.Inclusive) Then
                    ActAmout = MaxGeneric.clsGeneric.NullToDecimal(FeeAmount) - GSTAmt
                ElseIf (TaxMode = Generic._TaxMode.Exclusive) Then
                    ActAmout = FeeAmount
                    FeeAmount = MaxGeneric.clsGeneric.NullToDecimal(FeeAmount) + GSTAmt
                End If

                _txtActFeeAmount.Text = String.Format("{0:F}", ActAmout)
                _txtGSTAmount.Text = String.Format("{0:F}", GSTAmt)
                _txtTotFeeAmount.Text = String.Format("{0:F}", FeeAmount)

            Next
            'GST Calculation - Ended
        Catch ex As Exception
            If ex.Message = "There is no row at position 0." Then
                lblMsg.Visible = True
                lblMsg.Text = "Required Tax Type"
            End If
            Call MaxModule.Helper.LogError(ex.Message)
        End Try
    End Sub
#End Region

#Region "Load Taxtype "
    Public Sub LoadTaxType()

        ddlTaxType.DataSource = _GstSetupDal.GetGstDetails(0)
        ddlTaxType.DataTextField = "sas_taxtype"
        ddlTaxType.DataValueField = "sas_taxid"
        ddlTaxType.DataBind()
        ddlTaxType.Items.Add(New ListItem("--Select--", "-1"))
        ddlTaxType.SelectedValue = -1

    End Sub
#End Region

    Protected Sub txtGLAcc_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        For Each dg As DataGridItem In dgFaculty.Items
            Dim imgGL As Image = dg.FindControl("imgGL")
            If imgGL.Visible Then
                imgGL.Visible = False
            End If
        Next
    End Sub

    Protected Sub txtGLAccKolej_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        For Each dg As DataGridItem In dgKolej.Items
            Dim imgGL As Image = dg.FindControl("imgGLKolej")
            If imgGL.Visible Then
                imgGL.Visible = False
            End If
        Next
    End Sub

    Protected Sub ddlFeeCategory_SelectedIndexChanged(sender As Object, e As EventArgs)
        If ddlFeeCategory.SelectedValue = "T" Then
            chkTutionFee.Checked = True
            pnlFaculty.Visible = True
            pnlKolej.Visible = False
        Else
            chkTutionFee.Checked = False

            If ddlFeeCategory.SelectedValue = "H" Then
                pnlFaculty.Visible = False
                pnlKolej.Visible = True
            Else
                pnlFaculty.Visible = True
                pnlKolej.Visible = False
            End If

        End If
    End Sub

#Region "New GL Account RA - Faculty"
    'added by Hafiz @ 15/11/2016

    Protected Sub LoadFacultyWithGlAccount()

        'Dim Faculty As String = Nothing
        Dim listEn As List(Of FacultyEn) = New FacultyDAL().GetList(New FacultyEn With {.SAFC_Code = ""})
        Dim Faculty = listEn.Select(Function(x) New With {.Faculty = x.SAFC_Code & " - " & x.SAFC_Desc}).ToList()

        dgFaculty.DataSource = Faculty
        dgFaculty.DataBind()

    End Sub


    Protected Sub dgFaculty_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs)

        If e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem Then
            'Dim CheckGLAcc As Button = CType(e.Item.FindControl("CheckGLAcc"), Button)
            Dim lblDesc As Label = CType(e.Item.FindControl("lblDesc"), Label)
            Dim imgGL As Image = CType(e.Item.FindControl("imgGL"), Image)

            'CheckGLAcc.Attributes.Add("onclick", "return validate()")
            lblDesc.Text = ""
            lblDesc.Visible = False
            imgGL.Visible = False
        End If

    End Sub

    Protected Sub dgFaculty_ItemCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs)

        If e.CommandName = "Check" Then
            Dim result As Boolean = False
            Dim _GLLedgerType As String = Nothing
            Dim _GLDescription As String = Nothing

            Dim _TextBox As TextBox = dgFaculty.Items(e.Item.ItemIndex).FindControl("txtGLAcc")
            Dim _ImgGL As Image = dgFaculty.Items(e.Item.ItemIndex).FindControl("imgGL")

            If Not _TextBox.Text = "" Then
                result = _MaxModule.GLCodeValidR(_TextBox.Text)

                If result Then
                    'Check GLLedgerType in CF
                    _GLLedgerType = _MaxModule.GetLedgerType(_TextBox.Text)

                    If Not _GLLedgerType = "" Then
                        'Retrive GLDescription in CF 
                        _GLDescription = _MaxModule.GetGLDescription(_TextBox.Text, _GLLedgerType)
                        'lblDesc.Visible = True
                        'lblDesc.Text = _GLDescription
                        _ImgGL.Visible = True
                        _ImgGL.ImageUrl = "~/images/check.png"
                    End If
                Else
                    lblMsg.Text = "Invalid GLCode"
                    _ImgGL.Visible = True
                    _ImgGL.ImageUrl = "~/images/cross.png"
                    Exit Sub
                End If
            End If

        End If

    End Sub

    Protected Function FacultyGLAccChecking() As Boolean

        Dim res As Boolean = False
        Dim counter As Integer = 0

        For i = 0 To dgFaculty.Items.Count - 1
            Dim _TextBox As TextBox = dgFaculty.Items(i).Cells(1).Controls(1)
            If _TextBox.Text = "" Then
                counter = counter + 1
            End If
        Next

        If counter = dgFaculty.Items.Count Then
            lblMsg.Text = "At Least One of The GL Accounts Need To Be Fill."
            lblMsg.Visible = True
            res = False
        Else
            For Each dg As DataGridItem In dgFaculty.Items
                Dim tbGL As TextBox = dg.FindControl("txtGLAcc")
                Dim imgGL As Image = dg.FindControl("imgGL")
                Dim result As Boolean = False
                Dim _GLLedgerType As String = Nothing
                Dim _GLDescription As String = Nothing

                If Not tbGL.Text = "" Then
                    result = _MaxModule.GLCodeValidR(tbGL.Text)

                    If result Then
                        'Check GLLedgerType in CF
                        _GLLedgerType = _MaxModule.GetLedgerType(tbGL.Text)

                        If Not _GLLedgerType = "" Then
                            'Retrive GLDescription in CF 
                            _GLDescription = _MaxModule.GetGLDescription(tbGL.Text, _GLLedgerType)
                            'lblDesc.Visible = True
                            'lblDesc.Text = _GLDescription
                            imgGL.Visible = True
                            imgGL.ImageUrl = "~/images/check.png"
                        End If
                        res = True
                    Else
                        imgGL.Visible = True
                        imgGL.ImageUrl = "~/images/cross.png"
                        lblMsg.Visible = True
                        lblMsg.Text = "Enter Valid GL Account"
                        Return False
                    End If
                End If

            Next

        End If

        Return res

    End Function

    Protected Sub BindFacultyGrid()

        If ddlFeeCategory.SelectedValue = "A" Or ddlFeeCategory.SelectedValue = "T" Then

            pnlFaculty.Visible = True

            If Not txtFeeCode.Text = "" Then

                Dim lstEn As List(Of FacultyGLAccEn) = ListObjects.Where(Function(x) x.FeeTypeCode = txtFeeCode.Text).Select(Function(x) x.LstFacultyGL).FirstOrDefault()
                For Each dg As DataGridItem In dgFaculty.Items
                    Dim _Image As Image = dg.FindControl("imgGL")
                    Dim _TextBox As TextBox = dg.FindControl("txtGLAcc")

                    If lstEn.Count > 0 Then
                        Dim constr As String() = dg.Cells(0).Text.Split("-")

                        If lstEn.Where(Function(x) x.SAFC_Code = Trim(constr(0)) AndAlso x.SAFC_Desc = Trim(constr(1))).Count > 0 Then
                            _TextBox.Text = lstEn.Where(Function(x) x.SAFC_Code = Trim(constr(0)) AndAlso x.SAFC_Desc = Trim(constr(1))).Select(Function(x) x.GL_Account).FirstOrDefault()
                        End If
                    Else
                        _TextBox.Text = ""
                    End If

                    _Image.Visible = False

                Next

            End If

        Else
            pnlFaculty.Visible = False
        End If

    End Sub

#End Region

#Region "New GL Account RA - Kolej"
    'added by Hafiz @ 18/11/2016

    Protected Sub LoadKolejWithGlAccount()

        'Dim Faculty As String = Nothing
        Dim listEn As List(Of KolejEn) = New KolejDAL().GetList(New KolejEn)
        Dim Kolej = listEn.Select(Function(x) New With {.Kolej = x.SAKO_Code & " - " & x.SAKO_Description}).ToList()

        dgKolej.DataSource = Kolej
        dgKolej.DataBind()

    End Sub

    Protected Sub dgKolej_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs)

        If e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem Then
            Dim imgGL As Image = CType(e.Item.FindControl("imgGLKolej"), Image)
            imgGL.Visible = False
        End If

    End Sub

    Protected Sub dgKolej_ItemCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs)

        If e.CommandName = "Check" Then
            Dim result As Boolean = False
            Dim _GLLedgerType As String = Nothing
            Dim _GLDescription As String = Nothing

            Dim _TextBox As TextBox = dgKolej.Items(e.Item.ItemIndex).FindControl("txtGLAccKolej")
            Dim _ImgGL As Image = dgKolej.Items(e.Item.ItemIndex).FindControl("imgGLKolej")

            If Not _TextBox.Text = "" Then
                result = _MaxModule.GLCodeValidR(_TextBox.Text)

                If result Then
                    'Check GLLedgerType in CF
                    _GLLedgerType = _MaxModule.GetLedgerType(_TextBox.Text)

                    If Not _GLLedgerType = "" Then
                        'Retrive GLDescription in CF 
                        _GLDescription = _MaxModule.GetGLDescription(_TextBox.Text, _GLLedgerType)
                        'lblDesc.Visible = True
                        'lblDesc.Text = _GLDescription
                        _ImgGL.Visible = True
                        _ImgGL.ImageUrl = "~/images/check.png"
                    End If
                Else
                    lblMsg.Text = "Invalid GLCode"
                    _ImgGL.Visible = True
                    _ImgGL.ImageUrl = "~/images/cross.png"
                    Exit Sub
                End If
            End If

        End If

    End Sub

    Protected Function KolejGLAccChecking() As Boolean

        Dim res As Boolean = False
        Dim counter As Integer = 0

        For i = 0 To dgKolej.Items.Count - 1
            Dim _TextBox As TextBox = dgKolej.Items(i).Cells(1).Controls(1)
            If _TextBox.Text = "" Then
                counter = counter + 1
            End If
        Next

        If counter = dgKolej.Items.Count Then
            lblMsg.Text = "At Least One of The GL Accounts Need To Be Fill."
            lblMsg.Visible = True
            res = False
        Else
            For Each dg As DataGridItem In dgKolej.Items
                Dim tbGL As TextBox = dg.FindControl("txtGLAccKolej")
                Dim imgGL As Image = dg.FindControl("imgGLKolej")
                Dim result As Boolean = False
                Dim _GLLedgerType As String = Nothing
                Dim _GLDescription As String = Nothing

                If Not tbGL.Text = "" Then
                    result = _MaxModule.GLCodeValidR(tbGL.Text)

                    If result Then
                        'Check GLLedgerType in CF
                        _GLLedgerType = _MaxModule.GetLedgerType(tbGL.Text)

                        If Not _GLLedgerType = "" Then
                            'Retrive GLDescription in CF 
                            _GLDescription = _MaxModule.GetGLDescription(tbGL.Text, _GLLedgerType)
                            'lblDesc.Visible = True
                            'lblDesc.Text = _GLDescription
                            imgGL.Visible = True
                            imgGL.ImageUrl = "~/images/check.png"
                        End If
                        res = True
                    Else
                        imgGL.Visible = True
                        imgGL.ImageUrl = "~/images/cross.png"
                        lblMsg.Visible = True
                        lblMsg.Text = "Enter Valid GL Account"
                        Return False
                    End If
                End If

            Next

        End If

        Return res

    End Function

    Protected Sub BindKolejGrid()

        If ddlFeeCategory.SelectedValue = "H" Then

            pnlKolej.Visible = True

            If Not txtFeeCode.Text = "" Then

                Dim lstEn As List(Of KolejGLAccEn) = ListObjects.Where(Function(x) x.FeeTypeCode = txtFeeCode.Text).Select(Function(x) x.LstKolejGL).FirstOrDefault()
                For Each dg As DataGridItem In dgKolej.Items
                    Dim _Image As Image = dg.FindControl("imgGLKolej")
                    Dim _TextBox As TextBox = dg.FindControl("txtGLAccKolej")

                    If lstEn.Count > 0 Then
                        Dim constr As String() = dg.Cells(0).Text.Split("-")

                        If lstEn.Where(Function(x) x.SAKO_Code = Trim(constr(0)) AndAlso x.SAKO_Description = Trim(constr(1))).Count > 0 Then
                            _TextBox.Text = lstEn.Where(Function(x) x.SAKO_Code = Trim(constr(0)) AndAlso x.SAKO_Description = Trim(constr(1))).Select(Function(x) x.GL_Account).FirstOrDefault()
                        End If
                    Else
                        _TextBox.Text = ""
                    End If

                    _Image.Visible = False

                Next

            End If

        Else
            pnlKolej.Visible = False
        End If

    End Sub

#End Region


End Class
