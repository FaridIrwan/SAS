Imports System.Collections.Generic
Imports HTS.SAS.BusinessObjects
Imports HTS.SAS.Entities
Imports System.Data

Partial Class bankDetails
    Inherits System.Web.UI.Page
#Region "Global Declaration "
    'declare instant
    Private _MaxModule As New MaxModule.CfCommon
    Dim ListObjects As List(Of BankProfileEn)

    Dim CFlag As String
    Dim DFlag As String
    Private ErrorDescription As String

#End Region

    Protected Sub IbtnView_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ibtnView.Click
        If lblCount.Text <> "" Then
            If CInt(lblCount.Text) > 0 Then
                'Clear Text Box values
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
            'Adding validation for Save button
            ibtnSave.Attributes.Add("onclick", "return validate()")
            'Adding validation for Delete button
            ibtnDelete.Attributes.Add("onclick", "return getconfirm()")

            txtRecNo.Attributes.Add("OnKeyup", "return geterr()")
            'Loading User Rights
            Session("PageMode") = "Add"
            LoadUserRights()
            'while loading list object make it nothing
            Session("ListObj_BankDetails") = Nothing
            DisableRecordNavigator()
            Menuname(CInt(Request.QueryString("Menuid")))
        End If
        lblMsg.Text = ""
        lblMsg.Visible = False
    End Sub

    Protected Sub ibtnSave_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ibtnSave.Click
        SpaceValidation()
    End Sub
    Protected Sub ibtnNew_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ibtnNew.Click
        OnAdd()
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
    ''' Method to Validate Data Fields before Save
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub SpaceValidation()
        'BankName
        If Trim(txtBankName.Text).Length = 0 Then

            txtBankName.Text = Trim(txtBankName.Text)
            lblMsg.Text = "Enter valid Bank Name "
            lblMsg.Visible = True
            txtBankName.Focus()
            Exit Sub
        End If
        'AccountCode
        If Trim(txtAccountCode.Text).Length = 0 Then

            txtAccountCode.Text = Trim(txtAccountCode.Text)
            lblMsg.Text = "Enter valid Account Code "
            lblMsg.Visible = True
            txtBankName.Focus()
            Exit Sub
        End If
        'GlAccount
        If Trim(txtGlAccount.Text).Length = 0 Then
            lblMsg.Text = "Enter valid GL Account"
            lblMsg.Visible = True
            txtGlAccount.Text = Trim(txtGlAccount.Text)

            txtGlAccount.Focus()
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

        Try
            eobj = obj.GetUserRights(CInt(Request.QueryString("Menuid")), CInt(Session("UserGroup")))
        Catch ex As Exception
            LogError.Log("BankDetails", "LoadUserRights", ex.Message)
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
    ''' Method to Clear the Fields in NewMode
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub OnAdd()
        Session("PageMode") = "Add"
        ibtnSave.Enabled = True
        ibtnSave.ImageUrl = "images/save.png"
        PnlAdd.Visible = True
        'Clear Text Box values
        OnClearData()
    End Sub
    ''' <summary>
    ''' Method to Clear the Field Values
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub OnClearData()
        txtBankCode.Enabled = True
        Session("ListObj_BankDetails") = Nothing
        DisableRecordNavigator()
        lblMsg.Text = ""
        'Clear Text Box values
        txtBankCode.Text = ""
        txtBankName.Text = ""
        txtAccountCode.Text = ""
        txtGlAccount.Text = ""
        ddlStatus.SelectedValue = "1"
        dgView.SelectedIndex = -1
        pnlView.Visible = False
        Session("PageMode") = "Add"

        lblDescGL.Text = ""
        lblDescGL.Visible = False
        imgGL.Visible = False
    End Sub
    ''' <summary>
    ''' Method to Save and Update Bank Profile
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub OnSave()
        lblMsg.Visible = False
        Dim bsobj As New BankProfileBAL
        Dim eobj As New BankProfileEn
        Dim RecAff As Integer
        eobj.BankDetailsCode = Trim(txtBankCode.Text)
        eobj.Description = Trim(txtBankName.Text)
        eobj.ACCode = Trim(txtAccountCode.Text)
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
                eobj.Code = CInt(Session("BranchCode"))
                RecAff = bsobj.Insert(eobj)
                'User Defined Message
                ErrorDescription = "Record Saved Successfully "
                lblMsg.Text = ErrorDescription

            Catch ex As Exception
                lblMsg.Text = ex.Message.ToString()
                LogError.Log("BankDetails", "OnSave", lblMsg.Text)
            End Try
        ElseIf Session("PageMode") = "Edit" Then
            Try
                RecAff = bsobj.Update(eobj)
                ListObjects = Session("ListObj_BankDetails")
                ListObjects(CInt(txtRecNo.Text) - 1) = eobj
                Session("ListObj_BankDetails") = ListObjects
                'User Defined Message
                ErrorDescription = "Record Updated Successfully "
                lblMsg.Text = ErrorDescription
            Catch ex As Exception
                lblMsg.Text = ex.Message.ToString()
                LogError.Log("BankDetails", "OnSave", lblMsg.Text)
            End Try
        End If

    End Sub
    ''' <summary>
    ''' Method to Delete BankProfiles
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub OnDelete()
        lblMsg.Visible = True
        If txtBankCode.Text <> "" Then
            If lblCount.Text = "" Then lblCount.Text = 0
            If lblCount.Text > 0 Then
                Dim bsobj As New BankProfileBAL
                Dim eobj As New BankProfileEn
                Dim RecAff As Integer
                lblMsg.Visible = True
                eobj.BankDetailsCode = txtBankCode.Text
                Try
                    RecAff = bsobj.Delete(eobj)
                    ListObjects = Session("ListObj_BankDetails")
                    ListObjects.RemoveAt(CInt(txtRecNo.Text) - 1)
                    lblCount.Text = lblCount.Text - 1
                    Session("ListObj_BankDetails") = ListObjects
                    lblMsg.Visible = True
                    ErrorDescription = "Record Deleted Successfully "
                    lblMsg.Text = ErrorDescription
                Catch ex As Exception
                    lblMsg.Text = ex.Message.ToString()
                    LogError.Log("BankDetails", "OnDelete", lblMsg.Text)
                End Try
                txtBankCode.Text = ""
                txtBankName.Text = ""
                txtAccountCode.Text = ""
                txtGlAccount.Text = ""
                ddlStatus.SelectedValue = "1"
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
    ''' Method to get list of Bank Profiles
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub LoadListObjects()
        Dim ds As New DataSet
        Dim bobj As New BankProfileBAL
        Dim eobj As New BankProfileEn
        Dim recStu As Integer
        If ddlStatus.SelectedValue = -1 Then
            recStu = -1
        Else
            recStu = ddlStatus.SelectedValue
        End If
        If txtBankCode.Text = "" Then txtBankCode.Text = "*"
        eobj.BankDetailsCode = Trim(txtBankCode.Text)
        eobj.Description = Trim(txtBankName.Text)
        eobj.ACCode = Trim(txtAccountCode.Text)
        eobj.GLCode = Trim(txtGlAccount.Text)
        eobj.Status = ddlStatus.SelectedValue

        Try
            ListObjects = bobj.GetBankProfileList(eobj)
        Catch ex As Exception
            LogError.Log("BankDetails", "LoadListObjects", ex.Message)
        End Try
        Session("ListObj_BankDetails") = ListObjects
        lblCount.Text = ListObjects.Count.ToString()
        If ListObjects.Count <> 0 Then
            DisableRecordNavigator()
            txtRecNo.Text = "1"
            pnlView.Visible = False
            PnlAdd.Visible = True
            OnMoveFirst()
            If Session("EditFlag") = True Then
                Session("PageMode") = "Edit"
                txtBankCode.Enabled = False
                ibtnSave.Enabled = True
                ibtnSave.ImageUrl = "images/save.png"
                lblMsg.Visible = True
            Else
                Session("PageMode") = ""
                ibtnSave.Enabled = False
                ibtnSave.ImageUrl = "images/gsave.png"
                ErrorDescription = "Record did not Exist"
                lblMsg.Text = ErrorDescription
                lblMsg.Visible = True
            End If
        Else
            txtRecNo.Text = ""
            lblCount.Text = ""
            txtBankCode.Enabled = True
            Session("ListObj_BankDetails") = Nothing
            DisableRecordNavigator()
            'Clear Text Box values
            txtBankCode.Text = ""
            txtBankName.Text = ""
            txtAccountCode.Text = ""
            txtGlAccount.Text = ""
            ddlStatus.SelectedValue = "1"
            dgView.SelectedIndex = -1
            pnlView.Visible = False
            Session("PageMode") = "Add"
            If DFlag = "Delete" Then
            Else

                ErrorDescription = "Record did not Exist"
                lblMsg.Text = ErrorDescription
                lblMsg.Visible = True
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
        lblDescGL.Text = ""
        imgGL.Visible = False
        FillData(0)
    End Sub
    ''' <summary>
    ''' Method to Move to Next Record
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub OnMoveNext()
        txtRecNo.Text = CInt(txtRecNo.Text) + 1
        lblDescGL.Text = ""
        imgGL.Visible = False
        FillData(CInt(txtRecNo.Text) - 1)
    End Sub
    ''' <summary>
    ''' Method to Move to Previous Record
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub OnMovePrevious()
        txtRecNo.Text = CInt(txtRecNo.Text) - 1
        lblDescGL.Text = ""
        imgGL.Visible = False
        FillData(CInt(txtRecNo.Text) - 1)
    End Sub
    ''' <summary>
    ''' Method to Move to Last Record
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub OnMoveLast()
        txtRecNo.Text = lblCount.Text
        lblDescGL.Text = ""
        imgGL.Visible = False
        FillData(CInt(lblCount.Text) - 1)
    End Sub
    ''' <summary>
    ''' Method to Enable or Disable Navigation Buttons
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub DisableRecordNavigator()
        Dim flag As Boolean
        If Session("ListObj_BankDetails") Is Nothing Then
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
    ''' Method to Display the list of Bank Profiles
    ''' </summary>
    ''' <param name="RecNo">Parameter is RecNo</param>
    ''' <remarks></remarks>
    Private Sub FillData(ByVal RecNo As Integer)
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
        If txtRecNo.Text = 0 Then
            txtRecNo.Text = 1
        Else
            If lblCount.Text = 0 Then
                txtRecNo.Text = 0
            Else
                pnlView.Visible = False
                PnlAdd.Visible = True
                Dim obj As BankProfileEn
                ListObjects = Session("ListObj_BankDetails")
                obj = ListObjects(RecNo)
                txtBankCode.Text = obj.BankDetailsCode
                txtBankName.Text = obj.Description
                txtAccountCode.Text = obj.ACCode
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
            LogError.Log("BankDetails", "Menuname", ex.Message)
        End Try
        lblMenuName.Text = eobj.MenuName
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

    Protected Sub ibtnCancel_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ibtnCancel.Click
        LoadUserRights()
        OnClearData()
    End Sub

    Protected Sub ibtnDelete_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ibtnDelete.Click
        OnDelete()
    End Sub
    Protected Sub Check_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Check.Click
        'varaible declaration
        Dim result As Boolean = False
        Dim _GLCode As String = txtGlAccount.Text
        Dim _GLLedgerType As String = Nothing
        Dim _GLDescription As String = Nothing

        Try
            imgGL.Visible = False
            lblDescGL.Text = ""
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
                        lblDescGL.Visible = True
                        lblDescGL.Text = _GLDescription
                        imgGL.Visible = True
                        imgGL.ImageUrl = "~/images/check.png"
                    End If
                Else
                    lblDescGL.Visible = True
                    lblDescGL.Text = "Invalid GLCode"
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
    Protected Sub txtGlAccount_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtGlAccount.TextChanged
        If imgGL.Visible Then
            imgGL.Visible = False
        End If
    End Sub
End Class
