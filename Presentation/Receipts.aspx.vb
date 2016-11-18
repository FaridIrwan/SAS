#Region "NameSpaces "

Imports MaxModule
Imports MaxGeneric
Imports System.IO
Imports System.Linq
Imports System.Text
Imports System.Data
Imports HTS.SAS.Entities
Imports System.Globalization
Imports System.Data.SqlClient
Imports HTS.SAS.BusinessObjects
Imports System.Collections.Generic
Imports HTS.SAS.DataAccessObjects

#End Region

Partial Class Receipts
    Inherits System.Web.UI.Page

    'MODIFIED by Hafiz @ 29/3/2016
    'MODIFIED by Hafiz @ 01/4/2016

#Region "Global Declarations "

    'Create Instances - Start
    Dim dsReturn As New DataSet
    Private ListObjects As List(Of AccountsEn)
    Private ListObjectsStudent As List(Of StudentEn)
    Private ListTRD As New List(Of AccountsDetailsEn)
    Dim objIntegration As New IntegrationModule.IntegrationNameSpace.Integration
    Dim objIntegrationDL As New SQLPowerQueryManager.PowerQueryManager.IntegrationDL
    'Create Instances - Stop

    Public Auto As Boolean
    Public RrcType As Boolean
    Private StuFlag As Boolean
    Private spnFlag As Boolean

    'Variable Declarations - Start
    Dim totPaidAmt As Double, tamount As Double, totBalAmt As Double
    Dim DeleteFlag As String, flag As String, ErrorDescription As String, AutoNo As Boolean
    'Variable Declarations - Stop

    Dim column As String() = {"matricNo", "StudentName", "ICNo", "Faculty", "ProgramID", "CurrentSemester", "TransactionAmount",
                              "StuIndex", "PaidAmount", "StManual", "SubReferenceTwo", "noAkaun", "Outstanding_Amount"}

#End Region

#Region "Set Message "

    Private Sub SetMessage(ByVal MessageDetails As String)

        lblMsg.Text = String.Empty
        lblMsg.Text = MessageDetails

    End Sub

#End Region

#Region "Get Sub Type "

    Private Function GetSubType() As String

        Dim SubType As String = ddlReceiptFor.SelectedValue

        Select Case SubType

            Case ReceiptsClass.ReceiptStudent
                Return ReceiptsClass.Student

            Case ReceiptsClass.ReceiptSponsor
                Return ReceiptsClass.Sponsor
            Case ReceiptsClass.StudentLoan
                Return ReceiptsClass.Student
            Case Else
                Return String.Empty

        End Select

    End Function

#End Region

#Region "Get Batch Date "

    Private Function GetBatchDate() As String

        If Not FormHelp.IsBlank(txtBatchDate.Text) Then
            Return Trim(txtBatchDate.Text)
        Else
            Return Format(Date.Now, "dd/MM/yyyy")
        End If

    End Function

#End Region

#Region "Get Batch Date "

    Private Function GetBatchCode() As String

        Return clsGeneric.NullToString(txtBatchId.Text)

    End Function

#End Region

#Region "Get Done By "

    Private Function GetDoneBy() As String

        Return Session("User")

    End Function

#End Region

#Region "Page Load "

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Call SetMessage("")
        'Variable Declarations
        Dim MenuId As Integer = 0, UserGroup As Integer = 0

        Try

            'Get Values - Start
            UserGroup = clsGeneric.NullToInteger(Session("UserGroup"))
            MenuId = clsGeneric.NullToInteger(Request.QueryString("Menuid"))
            'Get Values - Stop

            'if pafe not post back - Start
            If Not Page.IsPostBack Then
                Call Menuname(MenuId)
                Call AddEvents()
                Call DisableRecordNavigator()
                Call LoadUserRights(MenuId, UserGroup)

                'Set Session Values - Start
                Session("Menuid") = MenuId
                Session("PageMode") = "Add"
                Session("AddBank") = Nothing
                Session("ListObj") = Nothing
                Session("stualloc") = Nothing
                Session("stuupload") = Nothing
                Session("LaporanHarian") = False
                Session("liststu") = Nothing
                Session("eobjspn") = Nothing
                Session("ExceedLmt") = False
                'Set Session Values - Stop

                Call OnLoadItem()
                Call dates()
                Call nReceipt()
                Call addPayMode()
                Call addBankCode()

            End If
            'if pafe not post back - Stop

            If Not Session("eobjspn") Is Nothing Then
                'Populate Sponsor Invoices
                Call PopulateSponsorInvoice("", "")
            End If

            If Not Session("eobjstu") Is Nothing AndAlso Session("PostSts") = "Posted" Then
                Call AddLoanStudentCode()
            End If

            If Not Session("liststu") Is Nothing Then
                StuFlag = True
                Call LoadStudentDetails()
            End If

            If Not Session("File1") Is Nothing Then
                Call uploadData()
            End If

            If Not Session("CheckApproverList") Is Nothing Then
                SendToApproval()
            End If

            'Display Rcord from Student Ledger screen - Start
            'modified by Hafiz @ 27/5/2016
            If Not Request.QueryString("BatchCode") Is Nothing Then
                Dim str As String = Request.QueryString("BatchCode")
                Dim constr As String() = str.Split(";")

                txtBatchId.Text = constr(0)

                If constr(1) = "St" Then
                    ddlReceiptFor.SelectedValue = "St"
                    lblCtrlAmt.Visible = True
                    txtCtrlAmt.Visible = True
                ElseIf constr(1) = "Sp" Then
                    ddlReceiptFor.SelectedValue = "Sp"
                    lblCtrlAmt.Visible = False
                    txtCtrlAmt.Visible = False
                End If

                Dim accobj As AccountsEn = New AccountsDAL().GetAccList().Where(Function(x) x.BatchCode = constr(0)).FirstOrDefault()

                If Not accobj.SubCategory = "Loan" Then
                    'ddlReceiptFor.SelectedValue = "St"
                    'lblCtrlAmt.Visible = True
                    'txtCtrlAmt.Visible = True
                Else
                    ddlReceiptFor.SelectedValue = "Sl"
                    lblCtrlAmt.Visible = False
                    txtCtrlAmt.Visible = False
                End If

                'added by Hafiz @ 19/7/2016
                'Hide Control Amount TextField - start
                If CInt(Request.QueryString("IsCimbClicks")).Equals(1) Then
                    sCtrlAmt.Visible = False
                    lblCtrlAmt.Visible = False
                    txtCtrlAmt.Visible = False
                End If
                'Hide Control Amount TextField - end

                DirectCast(Master.FindControl("Panel1"), System.Web.UI.WebControls.Panel).Visible = False
                DirectCast(Master.FindControl("td"), System.Web.UI.HtmlControls.HtmlTableCell).Visible = False
                Panel1.Visible = False
                'Added by Hafiz Roslan on 07/01/2016
                'Reason: Disable Student Search for "View" the receipt from STUDENT LEDGER 
                lblStuSpn.Visible = False
                IdtnStud.Visible = False 'Commented by Hafiz Roslan  @ 13/01/2016
                'Modified on 15/2/2016 - disable search button on view from stud ledger - START
                lblIdtnStud.Visible = False
                searchStud.Visible = False
                btnSearchStud.Visible = False
                'disable search button on view from stud ledger - END
                'End code by Hafiz Roslan
                OnSearchOthers()
            End If
            'Display Rcord from Student Ledger screen - Stop

            If Not Session("fileSponsor") Is Nothing And Session("fileType") = "excel" Then
                Dim importobj As New ImportData
                ListObjectsStudent = importobj.GetImportedSponsorData(Session("fileSponsor").ToString())
                Session("liststu") = Nothing
                Session("liststu") = ListObjectsStudent
                LoadStudentDetails()
                Session("fileType") = Nothing
            ElseIf Not Session("fileSponsor") Is Nothing And Session("fileType") = "text" Then
                ListObjectsStudent = readTextFile(Session("fileSponsor").ToString())
                If Session("Err") = Nothing Then
                    LoadStudentDetails()
                End If
                System.IO.File.Delete(Session("fileSponsor"))
                Session("fileSponsor") = Nothing
                Session("fileType") = Nothing
            End If

        Catch ex As Exception

            Session("fileSponsor") = Nothing
            Session("fileType") = Nothing

            'log error
            Call MaxModule.Helper.LogError(ex.Message)

            'Show Error Message
            Call SetMessage(ex.Message)

        End Try

    End Sub

#End Region

#Region "Add Events "

    Private Sub AddEvents()

        Try

            ibtnSave.Attributes.Add("onclick", "return validate()")
            ibtnDelete.Attributes.Add("onclick", "return getconfirm()")
            ibtnView.Attributes.Add("onclick", "return CheckSearch()")
            ibtnOthers.Attributes.Add("onclick", "return CheckSearch()")
            bTnUpdate.Attributes.Add("onClick", "return CheckInvgrid()")
            txtSpnAmount.Attributes.Add("OnKeyUp", "CheckRecpAmount()")
            txtBatchDate.Attributes.Add("onKeyPress", "return CheckBatchDate()")
            txtReceiptDate.Attributes.Add("onKeyPress", "return CheckRecpDate()")
            txtRecNo.Attributes.Add("onKeyPress", "return geterr()")
            txtSpnAmount.Attributes.Add("onKeyPress", "return getcheck()")
            ibtnDelete.Attributes.Add("onClick", "return getconfirm()")
            ibtnRecDate.Attributes.Add("onClick", "return getDate1from()")
            ibtnBatchDate.Attributes.Add("onClick", "return getDate2from()")
            imgbankCode.Attributes.Add("onclick", "new_window=window.open('Addbank.aspx','Hanodale','width=470,height=550,resizable=0');new_window.focus();")
            ibtnPosting.Attributes.Add("onClick", "return getpostconfirm()")
            txtAllocateAmount.Attributes.Add("OnKeyUp", "return amount()")
            txtTotalPenAmt.Attributes.Add("onKeyPress", "checkValue();")
            txtRecNo.Attributes.Add("OnKeyUp", "return geterr()")
            IdtnStud.Attributes.Add("onclick", "new_window=window.open('AddMulStudents.aspx','Hanodale','width=550,height=550,resizable=0');new_window.focus();") 'Commented by Hafiz Roslan  @ 13/01/2016
            btnUpload.Attributes.Add("onclick", "new_window=window.open('File.aspx','Hanodale','width=470,height=380,resizable=0');new_window.focus();")
            btnSearchStud.Attributes.Add("onclick", "return CheckSearch()")
            'ibtnPosting.Attributes.Add("onclick", "new_window=window.open('AddApprover.aspx?MenuId=" & GetMenuId() & "','Hanodale','width=500,height=400,resizable=0');new_window.focus();")
            MenuId.Value = GetMenuId()

        Catch ex As Exception

            'log error
            Call MaxModule.Helper.LogError(ex.Message)

        End Try

    End Sub

#End Region

#Region "Add Loan Student Code "

    ''' <summary>
    ''' Method to Add Students 
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub AddLoanStudentCode()
        Dim eobjf As StudentEn
        eobjf = Session("eobjstu")
        txtStudentId.Text = eobjf.MatricNo
        txtStudentName.Text = eobjf.StudentName

        Dim ListInvObjects As New List(Of AccountsEn)
        Dim obj As New AccountsBAL
        Dim eob As New AccountsEn
        eob.CreditRef = txtStudentId.Text
        eob.PostStatus = "Posted"
        eob.SubType = "Student"
        eob.TransType = ""
        eob.TransStatus = ""

        Try
            ListInvObjects = obj.GetStudentLoanLedgerDetailList(eob)

            If ListInvObjects.Count > 0 Then
                Dim crAmount As Double = 0
                Dim dtAmount As Double = 0
                Dim totalAmount As Double = 0

                For Each loen As AccountsEn In ListInvObjects
                    crAmount = crAmount + loen.Credit
                    dtAmount = dtAmount + loen.Debit
                Next
                totalAmount = dtAmount - crAmount
                lblLoanAmountToPay.Text = String.Format("{0:F}", totalAmount)
            End If

        Catch ex As Exception
            LogError.Log("StudentLedger", "LoadInvoiceGrid", ex.Message)
            lblMsg.Text = ex.Message
        End Try
    End Sub

#End Region

#Region "Navigation Buttons "

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

#End Region

#Region "Save Click "

    Protected Sub ibtnSave_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ibtnSave.Click
        If lblStatus.Value = "Posted" Then
            lblMsg.Text = "Post Record Cannot be Edited"
            lblMsg.Visible = True
            Exit Sub
        End If

        Try
            SpaceValidation()

            If Session("ExceedLmt") = True Then
                lblMsg.Visible = True
                lblMsg.Text = "Total Amount of Receipt Must Not Exceed Control Amount"
                txtCtrlAmt.Focus()
                Return
            End If

            ' Save Student Loan Receipt
            If ddlReceiptFor.SelectedValue = "Sl" Then
                If CDbl(txtLoanAmount.Text.Trim()) > CDbl(lblLoanAmountToPay.Text) Then
                    lblMsg.Visible = True
                    lblMsg.Text = "The amount should not exceed the loan amount"
                    txtLoanAmount.Focus()
                    Return
                End If

                'Added by Hafiz Roslan
                'On 05/01/2015
                If Not Session(ReceiptsClass.SessionPageMode) = "Edit" Then
                    onSaveStudentLoan()
                Else
                    onEditStudentLoan()
                End If

            Else

                'Added by Hafiz Roslan
                'On 05/01/2015
                If Not Session(ReceiptsClass.SessionPageMode) = "Edit" Then
                    'Save Student Receipt and Sponsor Receipt
                    onSave()
                Else
                    onEdit()
                End If

            End If

            setdateFormat()
        Catch ex As Exception
            lblMsg.Text = ex.Message
        End Try

    End Sub

#End Region

#Region "View Click "

    Protected Sub ibtnView_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ibtnView.Click

        'Variable Declarations
        Dim LabelCount As Integer = 0

        Try

            'Get Lable Count
            LabelCount = clsGeneric.NullToInteger(lblCount.Text)

            'Set Session to view Status
            Session(ReceiptsClass.SessionLoadData) = ReceiptsClass.StatusView

            If Not FormHelp.IsBlank(lblCount.Text) Then
                If LabelCount > 0 Then
                    onAdd()
                Else
                    Session(ReceiptsClass.SessionPageMode) = ReceiptsClass.StatusEdit
                    addBankCode()
                    LoadListObjects(False)
                End If
            Else
                Session(ReceiptsClass.SessionPageMode) = ReceiptsClass.StatusEdit
                addBankCode()
                LoadListObjects(False)
            End If

            If lblCount.Text.Length = 0 Then
                Session("PageMode") = "Add"
            End If

        Catch ex As Exception

            'Log & Show Error - Start
            Call MaxModule.Helper.LogError(ex.Message)
            Call SetMessage(ex.Message)
            'Log & Show Error - Stop

        End Try



    End Sub

#End Region

#Region "Posting Click "

    'Protected Sub ibtnPosting_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ibtnPosting.Click
    '    If lblStatus.Value = "Ready" Then
    '        SpaceValidation()
    '        onPost()
    '        ' Post Student Loan Receipt
    '        'If ddlReceiptFor.SelectedValue = "Sl" Then
    '        '    OnPostStudentLoan()
    '        'Else
    '        '    ' Post Student Receipt and Sponsor Receipt

    '        'End If
    '        setdateFormat()
    '    ElseIf lblStatus.Value = "New" Then
    '        lblMsg.Text = "Record not Ready for Posting"
    '        lblMsg.Visible = True
    '    ElseIf lblStatus.Value = "Posted" Then
    '        lblMsg.Text = "Record Already Posted"
    '        lblMsg.Visible = True
    '    End If
    'End Sub

    Protected Sub SendToApproval()

        Try
            If lblStatus.Value = "Ready" Then

                If Not Session("listWF") Is Nothing Then

                    Dim list As List(Of WorkflowSetupEn) = Session("listWF")
                    If list.Count > 0 Then
                        lblMsg.Text = ""

                        SpaceValidation()
                        setdateFormat()

                        If OnPost() = True Then
                            If Session("listWF").count > 0 Then
                                WorkflowApproverList(Trim(txtBatchId.Text), Session("listWF"))
                            End If
                        End If
                    Else
                        Throw New Exception("Posting to workflow failed caused NO approver selected.")
                    End If

                End If

            ElseIf lblStatus.Value = "New" Then
                lblMsg.Text = "Record not Ready for Posting"
                lblMsg.Visible = True
            ElseIf lblStatus.Value = "Posted" Then
                lblMsg.Text = "Record Already Posted"
                lblMsg.Visible = True
            End If

        Catch ex As Exception
            lblMsg.Text = ex.Message.ToString()
        Finally
            Session("listWF") = Nothing
            Session("CheckApproverList") = Nothing
        End Try

    End Sub

#End Region

#Region "WorkflowApproverList"

    Protected Sub WorkflowApproverList(ByVal batchcode As String, ByVal lisWF As List(Of WorkflowSetupEn))

        Dim result As Boolean = False

        Try
            For Each wfEn As WorkflowSetupEn In lisWF

                result = New WorkflowDAL().InsertWFApprovalList(batchcode, wfEn.MenuMasterEn.PageName,
                                                                wfEn.UsersEn.UserName, wfEn.UserGroupsEn.UserGroupName)

            Next
        Catch ex As Exception
            lblMsg.Text = ex.Message.ToString()
            LogError.Log("Receipts", "InsertWFApprovalList", ex.Message)
        End Try

    End Sub

#End Region

#Region "Cancel Click "

    Protected Sub ibtnCancel_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ibtnCancel.Click
        'Get Values - Start
        Dim UserGroup As Integer = clsGeneric.NullToInteger(Session("UserGroup"))
        Dim MenuId As Integer = clsGeneric.NullToInteger(Request.QueryString("Menuid"))
        'Get Values - Stop
        LoadUserRights(MenuId, UserGroup)
        onAdd()
    End Sub

#End Region

#Region "New Click "

    Protected Sub ibtnNew_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ibtnNew.Click
        onAdd()
    End Sub

#End Region

#Region "ddlReceiptFor_SelectedIndexChanged "

    Protected Sub ddlReceiptFor_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlReceiptFor.SelectedIndexChanged

        check_Receiptfor()
        htxtCat.Value = ddlReceiptFor.SelectedValue

        'added by Hafiz Roslan
        'Data Grid Item Bind
        'updated on 2/2/2016
        If ddlReceiptFor.SelectedValue = "St" Then

            'Call ShowEmptyDataGrid(dgStudentView, column)
            Call ShowEmptyDataGrid()
        Else
            dgStudentView.DataSource = Nothing
            dgStudentView.DataBind()
        End If
        'End Data Grid Item Bind

    End Sub

#End Region

#Region "Data Grid Events "

    Protected Sub dgInvoices_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dgInvoices.ItemDataBound
        Dim txtAmount As TextBox
        Select Case e.Item.ItemType
            Case ListItemType.Item, ListItemType.AlternatingItem

                Dim alamount As Double

                txtAmount = CType(e.Item.FindControl("AllovateAmount"), TextBox)
                txtAmount.Attributes.Add("onKeyPress", "checkValue();")
                If txtAmount.Text = "" Then txtAmount.Text = 0
                totPaidAmt = totPaidAmt + CDbl(txtAmount.Text)
                alamount = txtAmount.Text
                txtAmount.Text = String.Format("{0:F}", alamount)

        End Select

    End Sub

    Protected Sub dgStudentView_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dgStudentView.ItemDataBound
        Dim txtAmount As New TextBox
        Dim chk As New CheckBox

        Select Case e.Item.ItemType
            Case ListItemType.Item, ListItemType.AlternatingItem
                txtAmount = CType(e.Item.FindControl("TxtAmt"), TextBox)

                'txtAmount.Attributes.Add("onKeyPress", "checkValue();")

                Dim bamount As Double = 0.0
                txtAmount.Text = String.Format("{0:F}", bamount)
                chk = CType(e.Item.FindControl("chkManual"), CheckBox)
        End Select
    End Sub
    Protected Sub dgInvoices_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs)

    End Sub

#End Region

#Region "cbInvoice_CheckedChanged "

    Protected Sub cbInvoice_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        LoadInvTotals()
    End Sub

#End Region

#Region "rBtnMultiple_CheckedChanged "

    Protected Sub rBtnMultiple_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)
    End Sub

#End Region

#Region "AllovateAmount_TextChanged "

    Protected Sub AllovateAmount_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        LoadInvTotals()
    End Sub

#End Region

#Region "ibtnOthers_Click "

    Protected Sub ibtnOthers_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ibtnOthers.Click
        'Set Session to view Status
        Session("PostSts") = "Posted"
        OnSearchOthers()
    End Sub

#End Region

#Region "ibtnDelete_Click "

    Protected Sub ibtnDelete_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ibtnDelete.Click
        ondelete()
    End Sub

#End Region

#Region "Btnselect_Click "

    Protected Sub Btnselect_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        uploadData()
        setdateFormat()
    End Sub

#End Region

#Region "TxtAmt_TextChanged "
    'modified by Hafiz @ 25/3/2016
    'modified by Hafiz @ 05/4/2016

    Protected Sub TxtAmt_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs)

        Dim totalAmt As Decimal = 0.0
        Dim dgItem1 As DataGridItem
        Dim amt As String = Nothing
        Dim txtAmount As TextBox, txtMatricNo As TextBox
        Dim listRect As New List(Of StudentEn)
        Dim i As Integer
        'Dim StudentGridCheckBox As CheckBox
        listRect = Session("listview")

        Session("ExceedLmt") = False

        'Loop thro the grid items - Start
        For Each dgItem1 In dgStudentView.Items

            txtAmount = dgItem1.Cells(9).Controls(1)
            txtMatricNo = dgItem1.Cells(2).Controls(1)

            i = dgItem1.Cells(11).Text
            amt = dgItem1.Cells(10).Text
            If txtAmount.Text = "" Then txtAmount.Text = 0.0

            If Not txtMatricNo.Text = "" Then

                Try
                    Dim str As String = txtAmount.Text

                    If Not IsNumeric(str) Then
                        dgItem1.Cells(9).Focus()
                        txtAmount.Text = ""

                        Throw New Exception("Enter valid amount")
                    End If

                    If txtAmount.Text = amt Then
                        amt = dgItem1.Cells(10).Text
                        txtAmount.Text = String.Format("{0:F}", CDec(amt))
                        totalAmt = totalAmt + CDec(txtAmount.Text)
                        txtAllocateAmount.Text = String.Format("{0:F}", totalAmt)
                    Else
                        amt = txtAmount.Text
                        dgItem1.Cells(10).Text = amt
                        txtAmount.Text = String.Format("{0:F}", CDec(amt))
                        totalAmt = totalAmt + CDec(txtAmount.Text)
                        txtAllocateAmount.Text = String.Format("{0:F}", totalAmt)
                    End If

                    Dim ctrlamt As Decimal = Session("CtrlAmt")
                    If Not ctrlamt = 0 Then
                        If totalAmt > ctrlamt Then
                            txtCtrlAmt.Focus()
                            Session("ExceedLmt") = True

                            Throw New Exception("Total Amount of Receipt Must Not Exceed Control Amount")
                        End If
                    End If

                Catch ex As Exception
                    Call SetMessage(ex.Message)
                End Try

            End If

        Next
        'Loop thro the grid items - Stop

        Session("listview") = Nothing
        Session("listview") = listRect
        listRect = Nothing
    End Sub

#End Region

#Region "btnReceipt_Click1 "

    Protected Sub btnReceipt_Click1(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnReceipt.Click
        nReceipt()
        check_Receiptfor()
    End Sub

#End Region

#Region "btnSelection_Click1 "

    Protected Sub btnSelection_Click1(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSelection.Click
        'Allocation()
    End Sub

#End Region

#Region "btnInactive_Click1 "

    Protected Sub btnInactive_Click1(ByVal sender As Object, ByVal e As System.EventArgs)

        MultiView1.SetActiveView(View3)
        btnReceipt.CssClass = "TabButton"
        btnSelection.CssClass = "TabButton"
        btnInactive.CssClass = "TabButtonClick"
        bankPanel.Visible = False
        pnlStudentGrid.Visible = False

    End Sub

#End Region

#Region "rBtnSingle_CheckedChanged "

    Protected Sub rBtnSingle_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)
    End Sub

#End Region

#Region "dgStudentView_SelectedIndexChanged "

    Protected Sub dgStudentView_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        dgInvoices.DataSource = Nothing
        Dim chk As CheckBox
        dgInvoices.DataBind()
        Try
            If dgStudentView.SelectedIndex <> -1 Then
                chk = dgStudentView.Items(dgStudentView.SelectedIndex).Cells(8).Controls(1)
                If chk.Checked = True Then
                    bTnUpdate.Enabled = True
                    Auto = chk.Checked
                    Session("Auto") = Auto
                End If

                Dim loen As New AccountsEn
                Dim ListInvObjects As New List(Of AccountsEn)
                Dim lobo As New AccountsBAL
                Dim lostulist As New List(Of StudentEn)

                Dim listtrandetailes As New List(Of AccountsDetailsEn)
                loen.CreditRef = dgStudentView.Items(dgStudentView.SelectedIndex).Cells(2).Text
                lostulist = Session("listview")
                'lostulist.Outstanding_Amount = dgStudentView.Items(dgStudentView.SelectedIndex).Cells(18).Text

                If lblStatus.Value = "Ready" Then
                    Dim manaulchk As CheckBox = dgStudentView.Items(dgStudentView.SelectedIndex).Cells(8).Controls(1)
                    Dim alloctext As TextBox = dgStudentView.Items(dgStudentView.SelectedIndex).Cells(9).Controls(1)
                    If manaulchk.Checked = True Then

                        lostulist = Session("listview")
                        Dim listaccounts As New List(Of AccountsDetailsEn)
                        Dim k As Integer = 0
                        While k < lostulist.Count
                            If loen.CreditRef = lostulist(k).MatricNo Then
                                If lostulist(k).SubReferenceTwo = "Manual" Then
                                    listaccounts = lostulist(k).AccountDetailsList
                                    txtStuIndex.Text = lostulist(k).TransactionAmount
                                    dgInvoices.Visible = True
                                    dgInvoices.DataSource = listaccounts
                                    dgInvoices.DataBind()
                                    Dim dgItem3 As DataGridItem
                                    Dim amt1 As Double
                                    Dim txtAmount1 As TextBox
                                    Dim selchk As CheckBox
                                    Dim totalamt1 As Double
                                    For Each dgItem3 In dgInvoices.Items
                                        txtAmount1 = dgItem3.Cells(5).Controls(1)
                                        selchk = dgItem3.Cells(0).Controls(1)
                                        selchk.Checked = True
                                        amt1 = dgItem3.Cells(4).Text
                                        dgItem3.Cells(1).Text = dgItem3.Cells(11).Text
                                        txtAmount1.Text = String.Format("{0:F}", amt1)
                                        dgItem3.Cells(4).Text = dgItem3.Cells(12).Text
                                        dgItem3.Cells(8).Text = dgItem3.Cells(13).Text
                                        totalamt1 = totalamt1 + txtAmount1.Text
                                        txtALLAmount.Text = String.Format("{0:F}", totalamt1)
                                        dgItem3.Cells(6).Text = Convert.ToString(CDbl(dgItem3.Cells(4).Text) - CDbl(dgItem3.Cells(8).Text))
                                    Next
                                End If
                            End If
                            k = k + 1
                        End While
                        txtStuIndex.Text = alloctext.Text
                        Session("ID") = dgStudentView.Items(dgStudentView.SelectedIndex).Cells(11).Text
                        Allocation()
                    End If
                    dgStudentView.SelectedIndex = -1

                ElseIf lblStatus.Value = "Posted" Then
                    Dim manaulchk As CheckBox = dgStudentView.Items(dgStudentView.SelectedIndex).Cells(8).Controls(1)
                    Dim alloctext As TextBox = dgStudentView.Items(dgStudentView.SelectedIndex).Cells(9).Controls(1)
                    If manaulchk.Checked = True Then

                        lostulist = Session("listview")
                        Dim listaccounts As New List(Of AccountsDetailsEn)
                        Dim k As Integer = 0
                        While k < lostulist.Count
                            If loen.CreditRef = lostulist(k).MatricNo Then
                                If lostulist(k).SubReferenceTwo = "Manual" Then
                                    listaccounts = lostulist(k).AccountDetailsList
                                    txtStuIndex.Text = lostulist(k).TransactionAmount
                                    dgInvoices.Visible = True
                                    dgInvoices.DataSource = listaccounts
                                    dgInvoices.DataBind()
                                    Dim dgItem3 As DataGridItem
                                    Dim amt1 As Double
                                    Dim txtAmount1 As TextBox
                                    Dim selchk As CheckBox
                                    Dim totalamt1 As Double
                                    For Each dgItem3 In dgInvoices.Items
                                        txtAmount1 = dgItem3.Cells(5).Controls(1)
                                        selchk = dgItem3.Cells(0).Controls(1)
                                        selchk.Checked = True
                                        amt1 = dgItem3.Cells(4).Text
                                        dgItem3.Cells(1).Text = dgItem3.Cells(11).Text
                                        txtAmount1.Text = String.Format("{0:F}", amt1)
                                        dgItem3.Cells(4).Text = dgItem3.Cells(12).Text
                                        dgItem3.Cells(8).Text = dgItem3.Cells(13).Text
                                        totalamt1 = totalamt1 + txtAmount1.Text
                                        txtALLAmount.Text = String.Format("{0:F}", totalamt1)
                                        dgItem3.Cells(6).Text = Convert.ToString(CDbl(dgItem3.Cells(4).Text) - CDbl(dgItem3.Cells(8).Text))
                                    Next
                                End If
                            End If
                            k = k + 1
                        End While
                        txtStuIndex.Text = alloctext.Text
                        Session("ID") = dgStudentView.Items(dgStudentView.SelectedIndex).Cells(11).Text
                        bTnUpdate.Enabled = False
                        Allocation()
                    End If
                    dgStudentView.SelectedIndex = -1
                ElseIf lblStatus.Value = "New" Then
                    Dim manaulchk As CheckBox = dgStudentView.Items(dgStudentView.SelectedIndex).Cells(8).Controls(1)
                    Dim alloctext As TextBox = dgStudentView.Items(dgStudentView.SelectedIndex).Cells(9).Controls(1)
                    If manaulchk.Checked = True Then
                        loen.SubType = "Student"
                        loen.PostStatus = "Posted"
                        loen.Category = "'Invoice','Debit Note','AFC'"

                        Try
                            ListInvObjects = lobo.GetStudentAutoAllocation(loen)
                        Catch ex As Exception
                            LogError.Log("Receipts", "dgStudentView_SelectedIndexChanged", ex.Message)
                        End Try

                        dgInvoices.Visible = True
                        Dim Mno As String = dgStudentView.DataKeys(dgStudentView.SelectedIndex)

                        Dim i As Integer
                        i = dgStudentView.Items(dgStudentView.SelectedIndex).Cells(11).Text
                        Dim list As List(Of StudentEn)
                        list = Session("listview")
                        listtrandetailes = list(i).AccountDetailsList
                        If listtrandetailes Is Nothing Then
                            If lblStatus.Value = "Posted" Then
                                dgInvoices.DataSource = ListInvObjects
                                dgInvoices.DataBind()
                            Else
                                dgInvoices.DataSource = ListInvObjects
                                dgInvoices.DataBind()
                            End If

                        Else
                            Dim k As Integer = 0
                            Dim eRecpInv As New AccountsEn
                            Dim listRecpInvoice As New List(Of AccountsEn)
                            If listtrandetailes.Count <> 0 Then
                                While k < listtrandetailes.Count
                                    Dim j As Integer = 0
                                    While j < ListInvObjects.Count
                                        If ListInvObjects(j).TransactionCode = listtrandetailes(k).ReferenceCode Then
                                            eRecpInv = New AccountsEn
                                            eRecpInv = ListInvObjects(j)
                                            eRecpInv.TempAmount = listtrandetailes(k).PaidAmount
                                            listRecpInvoice.Add(eRecpInv)
                                        End If
                                        j = j + 1
                                    End While
                                    k = k + 1
                                End While
                                Dim dgItem1 As DataGridItem
                                Dim amt1 As Double
                                Dim txtAmount1 As TextBox
                                Dim selckeck As CheckBox
                                dgInvoices.DataSource = listRecpInvoice
                                dgInvoices.DataBind()
                                For Each dgItem1 In dgInvoices.Items
                                    txtAmount1 = dgItem1.Cells(5).Controls(1)
                                    selckeck = dgItem1.Cells(0).Controls(1)
                                    selckeck.Checked = True
                                    amt1 = dgItem1.Cells(13).Text
                                    txtAmount1.Text = String.Format("{0:F}", amt1)
                                    dgItem1.Cells(6).Text = Convert.ToString(CDbl(dgItem1.Cells(4).Text) - CDbl(dgItem1.Cells(8).Text))
                                Next
                            Else
                                dgInvoices.DataSource = ListInvObjects
                                dgInvoices.DataBind()
                            End If
                        End If
                        Session("ID") = dgStudentView.Items(dgStudentView.SelectedIndex).Cells(11).Text
                        dgStudentView.SelectedIndex = -1
                        txtStuIndex.Text = alloctext.Text
                        Allocation()
                    Else
                        dgStudentView.SelectedIndex = -1
                    End If
                End If
            End If
        Catch ex As Exception
            lblMsg.Text = ex.Message
        End Try
    End Sub

#End Region

#Region "FLBankFile_DataBinding "

    Protected Sub FLBankFile_DataBinding(ByVal sender As Object, ByVal e As System.EventArgs)
        Response.Write(FLBankFile.PostedFile.FileName)
    End Sub

#End Region

#Region "btnUpload_Click "

    Protected Sub btnUpload_Click(ByVal sender As Object, ByVal e As System.EventArgs)

    End Sub

#End Region

#Region "chkselectall_CheckedChanged "

    Protected Sub chkselectall_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim dgItem1 As DataGridItem
        Dim chkselect As CheckBox
        For Each dgItem1 In dgStudentView.Items
            chkselect = dgItem1.Cells(0).Controls(1)
            If chkselect.Checked = False Then
                chkselect.Checked = False
            Else
                chkselect.Checked = True
            End If
        Next

    End Sub

#End Region

#Region "bTnClose_Click "

    Protected Sub bTnClose_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim i As Integer
        i = Session("ID")
        Dim chk As CheckBox
        chk = dgStudentView.Items(i).Cells(8).Controls(1)
        If lblStatus.Value = "New" Then
            If dgStudentView.Items(i).Cells(12).Text = "0.00" Then
                chk.Checked = False
            End If
        Else
            If dgStudentView.Items(i).Cells(13).Text = "Auto" Then
                chk.Checked = False
            Else
                chk.Checked = True
            End If
        End If
        nReceipt()
        bTnUpdate.Enabled = False

    End Sub

#End Region

#Region "bTnUpdate_Click "

    Protected Sub bTnUpdate_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        bTnUpdate.Attributes.Add("onClick", "return CheckInvgrid()")
        Dim GBFormat As System.Globalization.CultureInfo
        GBFormat = New System.Globalization.CultureInfo("en-GB")
        Dim eobjDetails As New AccountsDetailsEn
        Dim dgItem1 As DataGridItem
        Dim list As New List(Of AccountsDetailsEn)
        Dim amount As TextBox
        Dim selinvchk As CheckBox
        Try
            LoadInvTotals()
            If CDbl(txtALLAmount.Text) > CDbl(txtStuIndex.Text) Then
                lblMsg.Text = "Amount Exceeded the allocated amount"
                Exit Sub
            End If

            Dim allocamount As Double = 0.0
            For Each dgItem1 In dgInvoices.Items
                selinvchk = dgItem1.Cells(0).Controls(1)
                If selinvchk.Checked = True Then
                    amount = dgItem1.Cells(5).Controls(1)
                    '   tempAmount = dgItem1.Cells(9).Controls(1)
                    eobjDetails = New AccountsDetailsEn
                    eobjDetails.ReferenceCode = dgItem1.Cells(1).Text
                    eobjDetails.TransactionAmount = CDbl(amount.Text)
                    eobjDetails.PaidAmount = CDbl(amount.Text)
                    eobjDetails.ReferenceOne = dgItem1.Cells(10).Text
                    eobjDetails.ReferenceTwo = "Manual"
                    eobjDetails.TransactionCode = dgItem1.Cells(1).Text
                    dgItem1.Cells(2).Text = DateTime.Parse(dgItem1.Cells(2).Text, GBFormat)
                    dgItem1.Cells(3).Text = DateTime.Parse(dgItem1.Cells(3).Text, GBFormat)
                    eobjDetails.TransDate = Trim(dgItem1.Cells(2).Text)
                    eobjDetails.DueDate = Trim(dgItem1.Cells(3).Text)
                    eobjDetails.TaxAmount = CDbl(dgItem1.Cells(4).Text)
                    eobjDetails.TempAmount = CDbl(dgItem1.Cells(8).Text)
                    eobjDetails.ReferenceOne = dgItem1.Cells(10).Text
                    eobjDetails.CreditRef = dgItem1.Cells(10).Text
                    'txtALLAmount.Text = CDbl(txtALLAmount.Text) + CDbl(amount.Text)
                    eobjDetails.TransStatus = "Open"
                    list.Add(eobjDetails)
                    allocamount += amount.Text
                    eobjDetails = Nothing
                End If
            Next
            Dim i As Integer
            i = Session("ID")
            Auto = Session("Auto")
            Dim liststudentView As New List(Of StudentEn)
            Dim stu As New StudentEn
            stu.PaidAmount = allocamount
            liststudentView = Session("listview")
            liststudentView(i).AmountPaid = stu.PaidAmount
            liststudentView(i).TransactionAmount = txtStuIndex.Text
            liststudentView(i).AccountDetailsList = list
            Session("listview") = Nothing

            Dim manualchk As CheckBox
            manualchk = dgStudentView.Items(i).Cells(8).Controls(1)
            dgStudentView.Items(i).Cells(12).Text = txtALLAmount.Text
            If txtALLAmount.Text = "0.00" Then
                dgStudentView.Items(i).Cells(13).Text = "Auto"
                liststudentView(i).SManual = "Auto"
                'manualchk.Checked = False
                liststudentView(i).SubReferenceTwo = "Auto"
            Else
                dgStudentView.Items(i).Cells(13).Text = "Manual"
                manualchk.Checked = True
                liststudentView(i).SManual = "Manual"
                liststudentView(i).SubReferenceTwo = "Manual"
            End If
            Session("listview") = liststudentView
            nReceipt()
            liststudentView = Nothing
            bTnUpdate.Enabled = False
            Session("ID") = Nothing
            Session("Auto") = Nothing
        Catch ex As Exception
            lblMsg.Text = ex.Message
        End Try
        'Dim tempAmount As TextBox

    End Sub

#End Region

#Region "BtnSubmit_Click "

    Protected Sub BtnSubmit_Click(ByVal sender As Object, ByVal e As System.EventArgs)

    End Sub

#End Region

#Region "dgUnStudent_SelectedIndexChanged "

    Protected Sub dgUnStudent_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs)

    End Sub

#End Region

#Region "txtRecNo_TextChanged "

    Protected Sub txtRecNo_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs)

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

#End Region

#Region "text Changed "
    'modified by Hafiz @ 04/6/2016
    'added textchanged for Control Amount

    Protected Sub txtSpnAmount_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtSpnAmount.TextChanged
        spnamount()
    End Sub

    Protected Sub txtCtrlAmt_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtCtrlAmt.TextChanged
        CtrlAmt()

        Session("ExceedLmt") = False

        Dim amt As Decimal = Session("CtrlAmt")
        Try
            If Not txtAllocateAmount.Text = 0 Then
                If CDec(txtAllocateAmount.Text) > amt Then
                    txtCtrlAmt.Focus()
                    Session("ExceedLmt") = True

                    Throw New Exception("Total Amount of Receipt Must Not Exceed Control Amount")
                End If
            End If
        Catch ex As Exception
            Call SetMessage(ex.Message)
        End Try

    End Sub

#End Region

#Region "ibtnDelete_Click1 "

    Protected Sub ibtnDelete_Click1(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)

    End Sub

#End Region

#Region "addBankCode "

    ''' <summary>
    ''' Method to Load BankCode Dropdown
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub addBankCode()

        Dim eobjF As New BankProfileEn
        Dim bsobj As New BankProfileBAL
        Dim list As New List(Of BankProfileEn)
        eobjF.BankDetailsCode = ""
        eobjF.Description = ""
        eobjF.ACCode = ""
        eobjF.GLCode = ""
        eobjF.Status = True
        ddlBankCode.Items.Clear()
        ddlBankCode.Items.Add(New ListItem("---Select---", "-1"))
        ddlBankCode.DataTextField = "Description"
        ddlBankCode.DataValueField = "BankDetailsCode"
        If Session("PageMode") = "Add" Then

            Try
                list = bsobj.GetBankProfileList(eobjF)
            Catch ex As Exception
                LogError.Log("Receipts", "addBankCode", ex.Message)
            End Try
        Else

            Try
                list = bsobj.GetBankProfileListAll(eobjF)
            Catch ex As Exception
                LogError.Log("Receipts", "addBankCode", ex.Message)
                lblMsg.Text = ex.Message
            End Try
        End If
        Session("bankcode") = list
        ddlBankCode.DataSource = list
        ddlBankCode.DataBind()

    End Sub

#End Region

#Region "addPayMode "

    ''' <summary>
    ''' Method to Load PaymentMode Dropdown
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub addPayMode()
        Dim eobjF As New PayModeEn
        Dim bsobj As New PayModeBAL
        Dim list As New List(Of PayModeEn)
        eobjF.SAPM_Code = ""
        eobjF.SAPM_Des = ""
        eobjF.SAPM_Status = True

        If Session("PageMode") = "Add" Then
            Try
                list = bsobj.GetPaytype(eobjF)
            Catch ex As Exception
                LogError.Log("Payments", "addPayMode", ex.Message)
            End Try
        Else
            Try
                list = bsobj.GetPaytypeAll(eobjF)
            Catch ex As Exception
                LogError.Log("Payments", "addPayMode", ex.Message)
                lblMsg.Text = ex.Message
            End Try
        End If
        Session("paymode") = list
        ddlPaymentMode.Items.Clear()
        ddlPaymentMode.Items.Add(New ListItem("---Select---", "-1"))
        ddlPaymentMode.DataSource = list
        ddlPaymentMode.DataTextField = "SAPM_Des"
        ddlPaymentMode.DataValueField = "SAPM_Code"
        ddlPaymentMode.DataBind()

    End Sub

#End Region

#Region "addSpnCode "

    ''' <summary>
    ''' Method to Load Sponsor
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub addSpnCode()
        Dim eobjf As New SponsorEn
        Dim dsobj As New SponsorBAL
        Dim listsp As New List(Of SponsorEn)
        Try
            If spnFlag = True Then
                eobjf = Session("eobjspn")
                spnFlag = False
                txtSponCode.ReadOnly = False
                txtSponName.ReadOnly = False
                txtSponCode.Text = eobjf.SponserCode
                txtSponName.Text = eobjf.Name
                txtSponCode.ReadOnly = True
                txtSponName.ReadOnly = True
                listsp.Add(eobjf)
                Session("listsp") = listsp
                Session("eobjspn") = Nothing
            Else
                If Not eobjf Is Nothing Then
                    txtSponCode.Text = eobjf.SponserCode
                    txtSponName.Text = eobjf.Name
                    Session("liststu") = Nothing
                Else
                    lblMsg.Text = "Sponsor did not exist"
                    lblMsg.Visible = True
                End If
            End If
        Catch ex As Exception
            lblMsg.Text = ex.Message
        End Try

    End Sub

#End Region

#Region "Load Student Details "

    ''' <summary>
    ''' Method to Load Students
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub LoadStudentDetails()

        'Create Instances - Start
        Dim _Receipts As New ReceiptsClass
        Dim ListStudent As New List(Of StudentEn)
        Dim total_data As New List(Of StudentEn)
        Dim ListStudentView As New List(Of StudentEn)
        'Create Instances - Stop

        Dim Index_1 As Integer = 0, indexx As Integer = 0, msg As String = Nothing

        Try
            'Get Student List from Session
            ListStudent = Session("liststu")
            total_data = Session("total_data")

            'Check and Assign Student List View - Start
            If Not Session("listview") Is Nothing Then

                'updated by Hafiz Roslan @ 3/2/2016
                'mapping at the datagrid purposes - START
                ListStudentView = Session("listview")

                If Not total_data Is Nothing Then

                    While indexx < total_data.Count - 1

                        If total_data(indexx).MatricNo = ListStudentView(indexx).MatricNo Then
                            'check what is change and if have, replace into original list
                            If Not total_data(indexx).TransactionAmount = ListStudentView(indexx).TransactionAmount Then
                                ListStudentView(indexx).TransactionAmount = total_data(indexx).TransactionAmount
                            End If

                            If Not total_data(indexx).BankSlipID = ListStudentView(indexx).BankSlipID Then
                                ListStudentView(indexx).BankSlipID = total_data(indexx).BankSlipID
                            End If

                            If Not total_data(indexx).ReceiptDate = ListStudentView(indexx).ReceiptDate Then
                                ListStudentView(indexx).ReceiptDate = total_data(indexx).ReceiptDate
                            End If

                        End If

                        indexx = indexx + 1

                    End While
                    'mapping at the datagrid purposes - START

                End If

            Else

                ListStudentView = New List(Of StudentEn)

            End If
            'Check and Assign Student List View - Stop

            'remove blank in ListStudentView
            While Index_1 < ListStudentView.Count

                If ListStudentView(Index_1).MatricNo Is Nothing Then
                    ListStudentView.Remove(ListStudentView(Index_1))
                End If

                Index_1 = Index_1 + 1

            End While

            'Populate Data Grid - Start
            'updated on 4/2/2016
            If Not ListStudent Is Nothing Then
                If Not _Receipts.LoadStudentListToGrid(ListStudent, ListStudentView, dgStudentView, msg) Then
                    Call SetMessage("Student List Loading Failed...")
                Else
                    'if succes!
                    Dim txtMatricNo As TextBox

                    If Not msg Is Nothing Then
                        Call SetMessage(msg)
                    End If

                    For Each _dgItem In dgStudentView.Items

                        txtMatricNo = _dgItem.Cells(2).Controls(1)

                        If Not txtMatricNo.Text = "" Then
                            txtMatricNo.ReadOnly = True
                        Else
                            txtMatricNo.ReadOnly = False
                        End If
                    Next
                End If
            End If
            'Populate Data Grid - Stop

            'Added by Hafiz Roslan @ 14/01/2016
            'Updated on 4/2/2016
            'Populate ListStudentView at the Student Loan Panel
            'Updated on 11/2/2016

            If Session("ReceiptFor") = "Sl" Then
                Dim stud As New StudentEn
                Dim _AccountsDal As New AccountsDAL
                Dim loan_outstanding As Double = 0.0

                If Not Session("eobjstu") Is Nothing Then
                    stud = Session("eobjstu")
                Else
                    stud = New StudentEn
                End If

                If Not stud.MatricNo Is Nothing Then

                    'get matric no
                    txtStudentId.Text = stud.MatricNo
                    'get stud name
                    txtStudentName.Text = stud.StudentName

                    'get student batchtotal from sass_studentloan - start
                    loan_outstanding = _AccountsDal.GetStudentLoanOutstanding(stud)
                    'get student batchtotal from sass_studentloan - end

                    lblLoanAmountToPay.Text = String.Format("{0:F}", loan_outstanding)

                End If

            End If

            'Set Values - Start
            StuFlag = False
            Call grid_load()
            'txtAllocateAmount.Text = 0.0
            Session("Type") = Nothing
            Session("liststu") = Nothing
            Session("total_data") = Nothing
            Session("eobjstu") = Nothing
            Session("listview") = ListStudentView
            'Set Values - Stop

        Catch ex As Exception

            Call SetMessage(ex.Message)

        End Try
    End Sub

#End Region

#Region "LoadInvoiceGrid "

    ''' <summary>
    ''' Method to Load InvoiceGrid
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub LoadInvoiceGrid()
        Dim eob As New AccountsEn
        Dim bobj As New AccountsBAL
        Dim list As New List(Of AccountsEn)

    End Sub

#End Region

#Region "Load User Rights "

    ''' <summary>
    ''' Method to Load UserRights
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub LoadUserRights(ByVal MenuId As Integer, ByVal UserGroup As Integer)

        Dim obj As New UsersBAL
        Dim eobj As New UserRightsEn

        Try
            eobj = obj.GetUserRights(MenuId, UserGroup)

        Catch ex As Exception
            LogError.Log("Receipts", "LoadUserRights", ex.Message)
        End Try
        'Rights for Add

        If eobj.IsAdd = True Then
            onAdd()
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
            ibtnView.ImageUrl = "images/ready.png"
            ibtnView.Enabled = True
        Else
            ibtnView.ImageUrl = "images/ready.png"
            ibtnView.Enabled = True
            'ibtnView.ToolTip = "Access Denied"
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
            ibtnOthers.ImageUrl = "images/post.png"
            ibtnOthers.ToolTip = "Others"
        Else
            ibtnOthers.Enabled = False
            ibtnOthers.ImageUrl = "images/post.png"
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

#Region "onAdd "

    ''' <summary>
    ''' Method to Load Fields in New Mode
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub onAdd()
        Session("PageMode") = "Add"
        addBankCode()
        Session("ListObj") = Nothing
        Session("ReceiptFor") = Nothing
        today.Value = Now.Date
        today.Value = Format(CDate(today.Value), "dd/MM/yyyy")
        Session("listview") = Nothing
        OnClearData()
        If ibtnNew.Enabled = False Then
            ibtnSave.Enabled = False
            ibtnSave.ImageUrl = "images/gsave.png"
            ibtnSave.ToolTip = "Access Denied"
        End If
        OnLoadItem()
    End Sub

#End Region

#Region "OnClearData "

    ''' <summary>
    ''' Method to Clear the Field Values
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub OnClearData()
        Session("ListObj") = Nothing
        Session("stualloc") = Nothing
        Session("stuupload") = Nothing
        DisableRecordNavigator()
        txtSponCode.Text = ""
        txtSponName.Text = ""
        txtSpnAmount.Text = ""

        txtBatchId.Text = ""
        ddlReceiptFor.SelectedValue = "-1"
        ddlPaymentMode.SelectedValue = "-1"
        ddlBankCode.SelectedValue = "-1"
        txtBatchId.Text = ""
        txtStuIndex.Text = ""
        txtReferenceNo.Text = ""
        txtOtherNo.Text = ""
        txtDescription.Text = ""
        txtReceiptDate.Text = ""
        txtBatchDate.Text = ""
        txtCtrlAmt.Text = ""
        txtAllocateAmount.Text = ""
        txtTotalPenAmt.Text = ""
        txtAddedAmount.Text = ""
        lblStatus.Value = "New"
        ibtnStatus.ImageUrl = "images/NotReady.gif"
        lblMsg.Text = ""
        'list
        ListObjects = Nothing
        dgInvoices.DataSource = Nothing
        dgInvoices.DataBind()
        'add mode
        dgStudentView.DataSource = Nothing
        dgStudentView.DataBind()
        dgUnStudent.DataSource = Nothing
        dgUnStudent.DataBind()
        Session("PageMode") = "Add"
        Session("listsp") = Nothing
        'Onload visible false
        dgInvoices.Visible = False
        txtTotalPenAmt.Visible = False
        txtAddedAmount.Visible = False
        txtAfterBalance.Visible = False
        lblTotal.Visible = False
        check_Receiptfor()
        txtStudentId.Text = ""
        txtStudentName.Text = ""
        txtLoanAmount.Text = "0.00"
        lblLoanAmountToPay.Text = "0.00"
        Session("eobjstu") = Nothing
        Session("ExceedLmt") = False
        'lblPending.Visible = False
        searchStud.Text = ""

    End Sub

#End Region

#Region "onSave "

    ''' <summary>
    ''' Method to Save and Update Receipts 
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub onSave()

        'Create Instances
        Dim _ReceiptClass As New ReceiptsClass

        'Variable Declarations
        Dim BatchCode As String = Nothing

        Try

            Select Case GetSubType()

                Case ReceiptsClass.Student

                    'updated by Hafiz Roslan @ 1/2/2016
                    'modified by Hafiz Roslan @ 01/4/2016
                    'check Matric No and Bank Slip No first - START
                    Dim msg As String = Nothing, tot_grid As Integer = 0, err_msg As String = Nothing
                    Dim txt_matricno As TextBox

                    Dim list As New List(Of StudentEn)

                    For Each _dgitems In dgStudentView.Items

                        txt_matricno = _dgitems.Cells(2).Controls(1)
                        tot_grid = dgStudentView.Items.Count - 1

                        If Not txt_matricno.Text = "" Then

                            If Session("listview") Is Nothing Then
                                msg = "Please Add the Student"
                                Exit For
                            Else
                                Dim objt As List(Of StudentEn) = Session("listview")
                                If objt.Where(Function(x) x.MatricNo = txt_matricno.Text).Count() = 0 Then
                                    msg = "Please Add the Student"
                                    Exit For
                                End If
                            End If

                            If DirectCast(_dgitems.FindControl("TxtAmt"), TextBox).Text = "0.00" Or
                                DirectCast(_dgitems.FindControl("TxtAmt"), TextBox).Text = "" Then

                                msg = "Amount Cannot Be Zero (0)"
                                DirectCast(_dgitems.FindControl("TxtAmt"), TextBox).Focus()
                                Exit For

                            ElseIf DirectCast(_dgitems.FindControl("BankSlipID"), TextBox).Text = "" Then

                                msg = "Bank Slip No is Required"
                                DirectCast(_dgitems.FindControl("BankSlipID"), TextBox).Focus()
                                Exit For

                            End If

                        Else

                            If Not tot_grid = 0 Then
                                Exit For
                            Else
                                msg = "Please Add the Student"
                                Exit For
                            End If

                        End If

                    Next

                    If Not msg Is Nothing Then
                        Throw New Exception(msg)
                        Exit Sub
                    End If

                    'check Matric No and Bank Slip No first - END

                    If _ReceiptClass.OnSaveStudentReceipt(dgStudentView, GetSubType(), GetDoneBy(),
                        GetBatchDate(), txtDescription.Text, txtReceiptDate.Text, txtReferenceNo.Text,
                        ddlPaymentMode.SelectedValue, ddlBankCode.SelectedValue,
                        clsGeneric.NullToDecimal(txtAllocateAmount.Text), CDec(txtCtrlAmt.Text), BatchCode, err_msg) Then

                        'Set Values - Start
                        lblStatus.Value = "Ready"
                        txtBatchId.ReadOnly = True
                        txtBatchId.Text = BatchCode
                        ibtnStatus.ImageUrl = "images/ready.gif"
                        ibtnSave.Enabled = False
                        'Set Values - Stop

                        'Show Message
                        Call SetMessage("Records Saved Successfully")

                        Session(ReceiptsClass.SessionListView) = dgStudentView.DataSource

                    Else

                        'Show Message
                        If Not err_msg Is Nothing Then
                            Call SetMessage(err_msg)
                        Else
                            Call SetMessage("Records Failed to Save")
                        End If

                    End If

                Case ReceiptsClass.Sponsor

                    'modified by Hafiz @ 05/4/2016
                    If txtSponCode.Text = "" Then
                        Throw New Exception("Please Add the Sponsor")
                    End If

                    If txtSpnAmount.Text = "0.00" Or txtSpnAmount.Text = "" Then
                        Throw New Exception("Amount Cannot Be Zero (0)")
                    End If

                    If _ReceiptClass.OnSaveSponsorReceipt(txtSponCode.Text, GetSubType(), GetDoneBy(),
                        GetBatchDate(), txtDescription.Text, txtReceiptDate.Text, txtReferenceNo.Text,
                        ddlPaymentMode.SelectedValue, ddlBankCode.SelectedValue, clsGeneric.NullToDecimal(
                        txtAllocateAmount.Text), BatchCode, ddlSponsorInv.SelectedValue) Then

                        'Set Values - Start
                        lblStatus.Value = "Ready"
                        txtBatchId.ReadOnly = True
                        txtBatchId.Text = BatchCode
                        ibtnStatus.ImageUrl = "images/ready.gif"
                        ibtnSave.Enabled = False
                        'Set Values - Stop

                        'Show Message
                        Call SetMessage("Records Saved Successfully")

                    Else

                        'Show Message
                        Call SetMessage("Records Failed to Save.")

                    End If

            End Select

        Catch ex As Exception

            'Log & Show Error - Start
            Call MaxModule.Helper.LogError(ex.Message)
            Call SetMessage(ex.Message)
            'Log & Show Error - Stop

        End Try

    End Sub

#End Region

#Region "OnEdit"
    'Added by Hafiz Roslan
    'On 05/01/2015

    Private Sub onEdit()

        Dim _ReceiptClass As New ReceiptsClass

        Try

            Select Case GetSubType()

                Case ReceiptsClass.Student

                    'updated by Hafiz Roslan @ 2/2/2016
                    'modified by Hafiz Roslan @ 05/4/2016
                    'check Matric No and Bank Slip No first - START
                    Dim msg As String = Nothing, tot_grid As Integer = 0, err_msg As String = Nothing
                    Dim txt_matricno As TextBox

                    Dim list As New List(Of StudentEn)

                    For Each _dgitems In dgStudentView.Items

                        txt_matricno = _dgitems.Cells(2).Controls(1)
                        tot_grid = dgStudentView.Items.Count - 1

                        If Not txt_matricno.Text = "" Then

                            If Session("listview") Is Nothing Then
                                msg = "Please Add the Student"
                                Exit For
                            Else
                                Dim objt As List(Of StudentEn) = Session("listview")
                                If objt.Where(Function(x) x.MatricNo = txt_matricno.Text).Count() = 0 Then
                                    msg = "Please Add the Student"
                                    Exit For
                                End If
                            End If

                            If DirectCast(_dgitems.FindControl("TxtAmt"), TextBox).Text = "0.00" Or
                                DirectCast(_dgitems.FindControl("TxtAmt"), TextBox).Text = "" Then

                                msg = "Amount Cannot Be Zero (0)"
                                DirectCast(_dgitems.FindControl("TxtAmt"), TextBox).Focus()
                                Exit For

                            ElseIf DirectCast(_dgitems.FindControl("BankSlipID"), TextBox).Text = "" Then

                                msg = "Bank Slip No is Required"
                                DirectCast(_dgitems.FindControl("BankSlipID"), TextBox).Focus()
                                Exit For

                            End If

                        Else

                            If Not tot_grid = 0 Then
                                Exit For
                            Else
                                msg = "Please Add the Student"
                                Exit For
                            End If

                        End If

                    Next

                    If Not msg Is Nothing Then
                        Throw New Exception(msg)
                        Exit Sub
                    End If
                    'check Matric No and Bank Slip No first - END

                    If _ReceiptClass.OnEditStudentReceipt(dgStudentView, GetSubType(), txtBatchId.Text,
                       ddlPaymentMode.SelectedValue, GetBatchDate(), ddlBankCode.SelectedValue, txtReceiptDate.Text,
                       txtReferenceNo.Text, clsGeneric.NullToDecimal(txtAllocateAmount.Text), txtDescription.Text,
                       CDec(txtCtrlAmt.Text), GetDoneBy(), err_msg) Then

                        'Set Values - Start
                        lblStatus.Value = "Ready"
                        txtBatchId.ReadOnly = True
                        ibtnStatus.ImageUrl = "images/ready.gif"
                        ibtnSave.Enabled = False
                        'Set Values - Stop

                        'Show Message
                        Call SetMessage("Records Updated Successfully")

                        Session(ReceiptsClass.SessionListView) = dgStudentView.DataSource

                    Else

                        'Show Message
                        If Not err_msg Is Nothing Then
                            Call SetMessage(err_msg)
                        Else
                            Call SetMessage("Records Failed to Update.")
                        End If

                    End If

                Case ReceiptsClass.Sponsor

                    'modified by Hafiz @ 05/4/2016
                    If txtSpnAmount.Text = "0.00" Or txtSpnAmount.Text = "" Then
                        Throw New Exception("Amount Cannot Be Zero (0)")
                    End If

                    If _ReceiptClass.OnEditSponsorReceipt(GetSubType(), txtBatchId.Text, ddlPaymentMode.SelectedValue,
                       GetBatchDate(), ddlBankCode.SelectedValue, txtReceiptDate.Text,
                       txtReferenceNo.Text, clsGeneric.NullToDecimal(txtAllocateAmount.Text), txtDescription.Text,
                       txtSponCode.Text, ddlSponsorInv.SelectedValue, GetDoneBy()) Then

                        'Set Values - Start
                        lblStatus.Value = "Ready"
                        txtBatchId.ReadOnly = True
                        ibtnStatus.ImageUrl = "images/ready.gif"
                        ibtnSave.Enabled = False
                        'Set Values - Stop

                        'Show Message
                        Call SetMessage("Records Updated Successfully")

                    Else

                        'Show Message
                        Call SetMessage("Records Failed to Update.")

                    End If


            End Select

        Catch ex As Exception

            'Log & Show Error - Start
            Call MaxModule.Helper.LogError(ex.Message)
            Call SetMessage(ex.Message)
            'Log & Show Error - Stop

        End Try

    End Sub

#End Region

#Region "onSaveStudentLoan "

    ''' <summary>
    ''' Method to Save and Update Student Loan Receipts
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub onSaveStudentLoan()

        'Create Instances
        Dim _ReceiptClass As New ReceiptsClass

        'Variable Declarations
        Dim BatchCode As String = Nothing

        Try
            'modified by Hafiz @ 05/4/2016
            If txtStudentId.Text = "" Then
                Throw New Exception("Please Add the Student")
            End If

            If txtLoanAmount.Text = "0.00" Or txtLoanAmount.Text = "" Then
                Throw New Exception("Amount Cannot Be Zero (0)")
            End If

            If _ReceiptClass.OnSaveStudentLoanReceipt(txtStudentId.Text, GetSubType(), GetDoneBy(),
                GetBatchDate(), txtDescription.Text, txtReceiptDate.Text, txtReferenceNo.Text,
                ddlPaymentMode.SelectedValue, ddlBankCode.SelectedValue, lblLoanAmountToPay.Text,
                clsGeneric.NullToDecimal(txtAllocateAmount.Text), BatchCode) Then

                'Set Values - Start
                lblStatus.Value = "Ready"
                txtBatchId.ReadOnly = True
                txtBatchId.Text = BatchCode
                ibtnStatus.ImageUrl = "images/ready.gif"
                ibtnSave.Enabled = False
                'Set Values - Stop

                'Show Message
                Call SetMessage("Records Saved Successfully")
                ibtnSave.Enabled = False
            Else

                'Show Message
                Call SetMessage("Records Failed to Save.")

            End If

        Catch ex As Exception

            'Log & Show Error - Start
            Call MaxModule.Helper.LogError(ex.Message)
            Call SetMessage(ex.Message)
            'Log & Show Error - Stop

        End Try

    End Sub

#End Region

#Region "onEditStudentLoan "

    'Added by Hafiz Roslan
    'On 06/01/2015

    Private Sub onEditStudentLoan()

        'Create Instances
        Dim _ReceiptClass As New ReceiptsClass

        Try
            If txtLoanAmount.Text = "0.00" Or txtLoanAmount.Text = "" Then
                Throw New Exception("Amount Cannot Be Zero (0)")
            End If

            If _ReceiptClass.OnEditStudentLoanReceipt(GetSubType(), txtBatchId.Text, ddlPaymentMode.SelectedValue,
                GetBatchDate(), ddlBankCode.SelectedValue, lblLoanAmountToPay.Text, txtReceiptDate.Text, txtReferenceNo.Text,
                clsGeneric.NullToDecimal(txtAllocateAmount.Text), txtDescription.Text, txtStudentId.Text, GetDoneBy()) Then

                'Set Values - Start
                lblStatus.Value = "Ready"
                txtBatchId.ReadOnly = True
                ibtnStatus.ImageUrl = "images/ready.gif"
                ibtnSave.Enabled = False
                'Set Values - Stop

                'Show Message
                Call SetMessage("Records Updated Successfully")

            Else

                'Show Message
                Call SetMessage("Records Failed to Update.")

            End If

        Catch ex As Exception

            'Log & Show Error - Start
            Call MaxModule.Helper.LogError(ex.Message)
            Call SetMessage(ex.Message)
            'Log & Show Error - Stop

        End Try

    End Sub

#End Region

#Region "Disable Record Navigator "

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

#End Region

#Region "Menu Name "

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
            LogError.Log("Receipts", "Menuname", ex.Message)
        End Try
        lblMenuName.Text = eobj.MenuName
    End Sub

#End Region

#Region "check_Receiptfor "

    ''' <summary>
    ''' Method to Check for Receipt to Student or Sponsor
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub check_Receiptfor()
        If ddlReceiptFor.SelectedValue = "St" Then
            bankPanel.Visible = True
            'modified by Hafiz Roslan @ 11/2/2016 - start
            btnInactive.Enabled = False
            btnInactive.Visible = False
            'modified by Hafiz Roslan @ 11/2/2016 - end
            pnlReceiptsp.Visible = False
            'modified by Hafiz Roslan @ 5/2/2016 - start
            btnUpload.Visible = False
            Label15.Visible = False
            'modified by Hafiz Roslan @ 5/2/2016 - end
            Panel4.Visible = True
            pnlStudentLoan.Visible = False
            lblStuSpn.Visible = False
            searchStud.Visible = False
            btnSearchStud.Visible = False
            lblCtrlAmt.Visible = True
            txtCtrlAmt.Visible = True
            lblIdtnStud.Text = "Search Student"
            Session("ReceiptFor") = ddlReceiptFor.SelectedValue
        ElseIf ddlReceiptFor.SelectedValue = "Sp" Then
            bankPanel.Visible = False
            btnInactive.Enabled = False
            btnUpload.Enabled = False
            pnlReceiptsp.Visible = False
            Panel4.Visible = False
            pnlStudentLoan.Visible = False
            lblStuSpn.Visible = True
            searchStud.Visible = True
            btnSearchStud.Visible = True
            lblCtrlAmt.Visible = False
            txtCtrlAmt.Visible = False
            lblIdtnStud.Text = "Search Sponsor"
            lblStuSpn.Text = "Search Sponsor by Sponsor Code"
            Session("ReceiptFor") = ddlReceiptFor.SelectedValue
        ElseIf ddlReceiptFor.SelectedValue = "Sl" Then
            bankPanel.Visible = False
            btnInactive.Enabled = False
            btnUpload.Enabled = False
            pnlReceiptsp.Visible = False
            Panel4.Visible = False
            pnlStudentLoan.Visible = True
            lblStuSpn.Visible = True
            searchStud.Visible = True
            btnSearchStud.Visible = True
            lblCtrlAmt.Visible = False
            txtCtrlAmt.Visible = False
            lblIdtnStud.Text = "Search Student"
            lblStuSpn.Text = "Search Student by Matric No"
            Session("ReceiptFor") = ddlReceiptFor.SelectedValue
        Else
            bankPanel.Visible = False
            btnInactive.Enabled = False
            btnUpload.Enabled = False
            pnlReceiptsp.Visible = False
            Panel4.Visible = False
        End If
    End Sub

#End Region

#Region "Datagrid Item Bind "
    'Added by Hafiz Roslan
    'Date added 19/1/2016
    'Added one empty grid table when first started

    Private Sub ShowEmptyDataGrid()

        'Create Instances - Start
        Dim stud As New StudentEn
        Dim list As New List(Of StudentEn)
        'Create Instances - Stop

        Dim index As Integer = 0

        If Not Session("listview") Is Nothing Then
            list = Session("listview")
        End If

        'remove blank in list - START
        While index < list.Count

            If list(index).MatricNo Is Nothing Then
                list.Remove(list(index))
            End If

            index = index + 1

        End While
        'remove blank in list - END

        list.Add(stud)

        dgStudentView.DataSource = list
        dgStudentView.DataBind()

        For Each _dgItems In dgStudentView.Items
            DirectCast(_dgItems.FindControl("txtTransDate"), TextBox).Enabled = False
            DirectCast(_dgItems.FindControl("BankSlipID"), TextBox).Enabled = False
            DirectCast(_dgItems.FindControl("TxtAmt"), TextBox).Enabled = False

            DirectCast(_dgItems.FindControl("txtTransDate"), TextBox).Visible = False
            DirectCast(_dgItems.FindControl("BankSlipID"), TextBox).Visible = False
            DirectCast(_dgItems.FindControl("TxtAmt"), TextBox).Visible = False

            DirectCast(_dgItems.FindControl("txtTransDate"), TextBox).Text = DateTime.Now.ToString("yyyy-MM-dd")
            _dgItems.Cells(dgStudentViewCell.CurrentSemester).Text = Nothing
            _dgItems.Cells(dgStudentViewCell.OutstandingAmount).Text = Nothing

            'For i As Integer = 0 To dgStudentView.Columns.Count - 1
            '_dgItems.Cells(i).BorderColor = Drawing.Color.White
            'Next
        Next

    End Sub

#End Region

#Region "On Add Button Click"
    'Added by Hafiz Roslan @ 22/1/2016
    'Modified by Hafiz @ 15/7/2016
    'On click button add

    Protected Sub Add_OnClick(ByVal sender As Object, ByVal e As System.EventArgs)

        Dim dgItem1 As DataGridItem
        Dim eob As New StudentEn
        Dim eob1 As New StudentEn
        Dim bobj As New StudentBAL
        Dim list As New List(Of StudentEn)
        Dim total_data As New List(Of StudentEn)
        Dim listStudent As New List(Of StudentEn)
        Dim obj As New List(Of StudentEn)

        Dim matricNo As TextBox = Nothing, amount As TextBox = Nothing, bankslipno As TextBox = Nothing
        Dim trans_date As TextBox = Nothing

        Try
            If CDec(txtCtrlAmt.Text) > 0 Then

                For Each dgItem1 In dgStudentView.Items

                    matricNo = dgItem1.Cells(2).Controls(1)
                    amount = dgItem1.Cells(9).Controls(1)
                    bankslipno = DirectCast(dgItem1.FindControl("BankSlipID"), TextBox)
                    trans_date = DirectCast(dgItem1.FindControl("txtTransDate"), TextBox)

                    If matricNo.Text = "" Then
                        dgItem1.Cells(2).Controls(1).Focus()
                        Throw New Exception("Matric No is Required")
                    Else
                        eob1 = New StudentEn

                        eob.MatricNo = Trim(matricNo.Text)
                        eob.TransactionAmount = Trim(amount.Text)
                        eob.BankSlipID = Trim(bankslipno.Text)
                        eob.ReceiptDate = Trim(trans_date.Text)
                        eob.StudentName = ""
                        eob.Faculty = ""
                        eob.ProgramID = ""
                        eob.ID = ""
                        eob.SAKO_Code = ""
                        eob.SABK_Code = ""
                        eob.SART_Code = ""
                        eob.StCategoryAcess = New StudentCategoryAccessEn
                        eob.StCategoryAcess.MenuID = 0

                        'updated by Hafiz Roslan on 3/2/2016
                        'why? mapping data - START
                        eob1.MatricNo = eob.MatricNo
                        eob1.TransactionAmount = eob.TransactionAmount
                        eob1.BankSlipID = eob.BankSlipID
                        eob1.ReceiptDate = eob.ReceiptDate
                    End If

                    total_data.Add(eob1)
                    eob1 = Nothing

                Next

                Session("total_data") = total_data

                'why? mapping data - END

                'find student via matric no - start
                Try
                    If Not eob.MatricNo = "" Then
                        list = bobj.GetListStudent(eob)
                    End If
                Catch ex As Exception
                    LogError.Log("Receipts", "matricNo_TextChanged", ex.Message)
                End Try
                'find student via matric no - end

                'insert records into list - start
                If Not IsNothing(list) AndAlso list.Count > 0 Then
                    'have record

                    'assign selected student into new obj
                    For Each stud As StudentEn In list

                        Dim newstuobj As New StudentEn

                        newstuobj.MatricNo = stud.MatricNo
                        newstuobj.TransactionAmount = eob.TransactionAmount
                        newstuobj.BankSlipID = eob.BankSlipID
                        newstuobj.ReceiptDate = eob.ReceiptDate
                        newstuobj.StudentName = stud.StudentName
                        newstuobj.ICNo = stud.ICNo
                        newstuobj.Faculty = stud.Faculty
                        newstuobj.ProgramID = stud.ProgramID
                        newstuobj.CurrentSemester = stud.CurrentSemester
                        newstuobj.CurretSemesterYear = stud.CurretSemesterYear
                        newstuobj.TempAmount = 0

                        listStudent.Add(newstuobj)

                        Session("eobjstu") = newstuobj   'single obj
                        Session("liststu") = listStudent 'list obj

                        'populate data at the grid
                        newstuobj = Nothing
                        StuFlag = True

                        Call LoadStudentDetails()
                    Next

                Else
                    'dont have record
                    lblMsg.Text = "No student records found"
                End If
                'insert records into list - end

            Else
                Throw New Exception("Please Add Control Amount")
            End If

        Catch ex As Exception
            Call SetMessage(ex.Message)

        Finally

            'force garbage collection
            Call GC.Collect(0)

        End Try


    End Sub

#End Region

#Region "Button Delete Click"
    'Added by Hafiz Roslan @ 21/1/2016
    'On click button delete

    Protected Sub Delete_OnClick(ByVal sender As Object, ByVal e As System.EventArgs)

        Dim _Receipts As New ReceiptsClass
        Dim list As New List(Of StudentEn)

        Dim Index As Integer = 0
        Dim txtMatricNo As String = Nothing, compare_text As String = Nothing, batchid As String = Nothing

        list = Session("listview")

        If Not data4delbutton.Value = "" Then
            txtMatricNo = Trim(data4delbutton.Value)
        End If

        While Index < list.Count - 1

            Try
                compare_text = Trim(list(Index).MatricNo)

                If Not Session(ReceiptsClass.SessionPageMode) = "Edit" Then
                    If compare_text = txtMatricNo Then
                        list.Remove(list(Index))
                        lblMsg.Text = "[Delete] Operation Success"
                    End If
                Else
                    'needs to check from db too for deletion
                    batchid = txtBatchId.Text

                    If Not _Receipts.OnDeleteGridList(txtMatricNo, batchid) Then
                        'false
                        If compare_text = txtMatricNo Then
                            list.Remove(list(Index))
                            lblMsg.Text = "[Delete] Operation Success"
                        End If
                    Else
                        'true
                        If compare_text = txtMatricNo Then
                            list.Remove(list(Index))
                            lblMsg.Text = "[Delete] Operation Success"
                        End If
                    End If
                End If

            Catch ex As Exception
                lblMsg.Text = ex.Message
            End Try

            Index = Index + 1

        End While

        dgStudentView.DataSource = list
        dgStudentView.DataBind()

        'map retrived list
        MappingDataGrid(list, dgStudentView)

    End Sub

#End Region

#Region "Mapping the Data into The DataGrid"
    'added by Hafiz Roslan on 21/01/2016
    'modified by Hafiz @ 07/4/2016
    '?? Mapped data to datagrid

    Protected Sub MappingDataGrid(ByVal list As List(Of StudentEn), ByVal dgStudList As DataGrid)

        Dim txt_matricno As TextBox, txt_Amount As TextBox, str_Amount As String = Nothing, index As Integer = 0
        Dim txt_transdate As Date = Nothing, total_amt As Decimal = 0.0

        For Each dgItem1 In dgStudList.Items

            txt_matricno = dgItem1.Cells(2).Controls(1)     'matricno textbox
            txt_Amount = dgItem1.Cells(9).Controls(1)       'amount text box
            str_Amount = clsGeneric.NullToDecimal(dgItem1.Cells(10).Text)   'amount boundcolumn

            If Not dgItem1.Cells(1).Text = "&nbsp;" Then
                'get matricno
                txt_matricno.Text = dgItem1.Cells(1).Text
                'get amount
                txt_Amount.Text = String.Format("{0:F}", CDec(str_Amount))
                'get outstanding amount
                DirectCast(dgItem1.FindControl("Outstanding_Amount"), Label).Text = String.Format("{0:F}", CDec(list(index).Outstanding_Amount))
                'get bank slip no
                DirectCast(dgItem1.FindControl("BankSlipID"), TextBox).Text = list(index).BankSlipID
                'updated by Hafiz Roslan @ 27/01/2016
                'get transaction date
                txt_transdate = list(index).ReceiptDate.ToString("yyyy-MM-dd")

                If Not txt_transdate = "0001-01-01" Then
                    DirectCast(dgItem1.FindControl("txtTransDate"), TextBox).Text = list(index).ReceiptDate.ToString("yyyy-MM-dd")
                Else
                    DirectCast(dgItem1.FindControl("txtTransDate"), TextBox).Text = DateTime.Now.ToString("yyyy-MM-dd")
                End If

                total_amt = total_amt + CDec(str_Amount)

            End If

            'Enable/disable delete button - Start
            EnableDisableDeleteButton(dgItem1)
            'Enable/disable delete button - End

            index = index + 1
        Next

        'updated @ 07/4/2016
        txtAllocateAmount.Text = total_amt

    End Sub

#End Region

#Region "Enable/Disable Delete Button"
    'by Hafiz Roslan
    'on 22/01/2016
    'Control for Delete Button

    Protected Sub EnableDisableDeleteButton(ByVal DataGrid As DataGridItem)

        Dim DeleteButton As Button = Nothing, AddButton As Button = Nothing, txtMatricNo As TextBox

        Call FormHelp.GetDataGridControl(DataGrid, EnumHelp.CommandButton.Add.ToString(), AddButton)
        Call FormHelp.GetDataGridControl(DataGrid, EnumHelp.CommandButton.Delete.ToString(), DeleteButton)

        'txtMatricNo = Trim(DataGrid.Cells(1).Text())

        DeleteButton.Attributes.Add("onclick", "Data4DeleteButton(' " + DataGrid.Cells(1).Text + " ') ")

        txtMatricNo = DataGrid.Cells(2).Controls(1)

        If Not txtMatricNo.Text Is Nothing AndAlso txtMatricNo.Text = "" Then
            Call FormHelp.SetAttribute(True, EnumHelp.Attribute.Visible, AddButton)
            Call FormHelp.SetAttribute(False, EnumHelp.Attribute.Visible, DeleteButton)
            txtMatricNo.ReadOnly = False

            DirectCast(DataGrid.FindControl("txtTransDate"), TextBox).Enabled = False
            DirectCast(DataGrid.FindControl("BankSlipID"), TextBox).Enabled = False
            DirectCast(DataGrid.FindControl("TxtAmt"), TextBox).Enabled = False

            DirectCast(DataGrid.FindControl("txtTransDate"), TextBox).Visible = False
            DirectCast(DataGrid.FindControl("BankSlipID"), TextBox).Visible = False
            DirectCast(DataGrid.FindControl("TxtAmt"), TextBox).Visible = False

            DirectCast(DataGrid.FindControl("txtTransDate"), TextBox).Text = DateTime.Now.ToString("yyyy-MM-dd")
            DataGrid.Cells(dgStudentViewCell.CurrentSemester).Text = Nothing
            DataGrid.Cells(dgStudentViewCell.OutstandingAmount).Text = Nothing
        Else
            Call FormHelp.SetAttribute(False, EnumHelp.Attribute.Visible, AddButton)
            Call FormHelp.SetAttribute(True, EnumHelp.Attribute.Visible, DeleteButton)
            txtMatricNo.ReadOnly = True
        End If

    End Sub

#End Region

#Region "PostedDisableButton"
    'Added by Hafiz Roslan
    'On 28/01/2016
    'Disable datagrid button when status=posted

    Protected Sub PostedDisableButton(ByVal _dgItems As DataGridItem)

        Dim DeleteButton As Button = Nothing, AddButton As Button = Nothing

        Call FormHelp.GetDataGridControl(_dgItems, EnumHelp.CommandButton.Add.ToString(), AddButton)
        Call FormHelp.GetDataGridControl(_dgItems, EnumHelp.CommandButton.Delete.ToString(), DeleteButton)

        Call FormHelp.SetAttribute(False, EnumHelp.Attribute.Enabled, AddButton)
        Call FormHelp.SetAttribute(False, EnumHelp.Attribute.Enabled, DeleteButton)

    End Sub

#End Region

#Region "Dates "

    ''' <summary>
    ''' Method to Change the Date Format
    ''' </summary>
    ''' <remarks>Date in ddd/mm/yyyy Format</remarks>
    Private Sub dates()

        'txtBatchDate.Text = Format(Date.Now, "dd/MM/yyyy")
        txtReceiptDate.Text = Format(Date.Now, "dd/MM/yyyy")

    End Sub

#End Region

#Region "On Load Item "

    ''' <summary>
    ''' Method to Load DateFields
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub OnLoadItem()
        If Session("PageMode") = "Add" Then
            If AutoNo = True And lblStatus.Value <> "Ready" Then
                'txtBatchId.Text = "Auto Number"
                txtBatchId.ReadOnly = True
            End If
            txtBatchDate.Text = Format(Date.Now, "dd/MM/yyyy")
            txtReceiptDate.Text = Format(Date.Now, "dd/MM/yyyy")
            txtBatchId.ReadOnly = False
            txtBatchDate.ReadOnly = True
            txtReceiptDate.ReadOnly = True
            spnamount()
            CtrlAmt()
            Session("ReceiptFor") = Nothing
        End If
    End Sub

#End Region

#Region "total pending Amount "

    ''' <summary>
    ''' Method to Caliculate the Pending Amount
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub totalpendingAmount()
        Dim dgItem1 As DataGridItem
        Dim totalpendingAmount As Double
        For Each dgItem1 In dgInvoices.Items
            totalpendingAmount = totalpendingAmount + dgItem1.Cells(5).Text
        Next
        txtTotalPenAmt.Text = String.Format("{0:F}", totalpendingAmount)
        txtAfterBalance.Text = String.Format("{0:F}", totalpendingAmount)
    End Sub

#End Region

#Region "RemovegirdItem "

    ''' <summary>
    ''' Method to Remove GridItem
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub RemovegirdItem()
        Dim dgItem1 As DataGridItem
        For Each dgItem1 In dgInvoices.Items

        Next
    End Sub

#End Region

#Region "LoadPaidInvoices "

    ''' <summary>
    ''' Method to Load Student Invoice into Grid
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub LoadPaidInvoices()
        Dim dgItem1 As DataGridItem
        Dim listInvoices As New List(Of AccountsDetailsEn)
        Dim eTTRDetails As New AccountsDetailsEn
        Dim tempamount As Double = 0.0
        Dim InvoiceAmount As Double = 0.0
        Dim txtAmount As TextBox
        Dim txtpdAmount As TextBox
        Dim chk As CheckBox
        Dim alamount As Double
        Dim pamount As Double
        Dim Tamount As Double
        Dim Status As Double
        txtAddedAmount.Text = 0
        Try
            For Each dgItem1 In dgInvoices.Items
                chk = dgItem1.Cells(0).Controls(1)
                txtAmount = dgItem1.Cells(6).Controls(1)
                txtAmount.Text = "0.00"

                If chk.Checked = True Then
                    chk.Checked = False
                End If

                InvoiceAmount = InvoiceAmount + dgItem1.Cells(5).Text
                If txtAllocateAmount.Text >= InvoiceAmount Then
                    eTTRDetails = New AccountsDetailsEn
                    eTTRDetails.ReferenceCode = dgItem1.Cells(1).Text
                    eTTRDetails.PaidAmount = dgItem1.Cells(5).Text
                    txtAmount.Text = eTTRDetails.PaidAmount
                    alamount = txtAmount.Text
                    If chk.Checked = False Then
                        chk.Checked = True
                    End If

                    txtAmount.Text = String.Format("{0:F}", alamount)
                    listInvoices.Add(eTTRDetails)
                    eTTRDetails = Nothing

                Else
                    If txtAllocateAmount.Text = InvoiceAmount Then
                    Else
                        tempamount = InvoiceAmount - txtAllocateAmount.Text
                        eTTRDetails = New AccountsDetailsEn
                        eTTRDetails.ReferenceCode = dgItem1.Cells(1).Text
                        eTTRDetails.PaidAmount = dgItem1.Cells(5).Text - tempamount

                        If eTTRDetails.PaidAmount > 0 Then
                            If chk.Checked = False Then
                                chk.Checked = True
                            End If
                            txtAmount.Text = eTTRDetails.PaidAmount

                            alamount = txtAmount.Text
                            txtAmount.Text = String.Format("{0:F}", alamount)
                            Status = dgItem1.Cells(5).Text - txtAmount.Text
                            If Status = 0 Then
                                eTTRDetails.Status = "C"
                            End If
                            listInvoices.Add(eTTRDetails)
                        End If
                        eTTRDetails = Nothing
                    End If
                End If

                txtpdAmount = dgItem1.Cells(7).Controls(1)
                txtpdAmount.Text = dgItem1.Cells(5).Text - txtAmount.Text
                pamount = txtpdAmount.Text
                txtpdAmount.Text = String.Format("{0:F}", pamount)
                Tamount = CDbl(txtAddedAmount.Text) + CDbl(txtAmount.Text)
                txtAddedAmount.Text = String.Format("{0:F}", Tamount)
                Dim totalpending As Double
                totalpending = totalpending + dgItem1.Cells(5).Text
                txtTotalPenAmt.Text = String.Format("{0:F}", totalpending)
                txtAfterBalance.Text = String.Format("{0:F}", CDbl(txtTotalPenAmt.Text) - CDbl(txtAddedAmount.Text))

            Next
        Catch ex As Exception
            lblMsg.Text = ex.Message
        End Try

        dgInvoices.Columns(7).FooterText = txtTotalPenAmt.Text
        Session("paidInvoices") = listInvoices

    End Sub

#End Region

#Region "Navigation Moves "

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

#End Region

#Region "FillData "

    ''' <summary>
    ''' Method to Fill the Field Values
    ''' </summary>
    ''' <param name="RecordNumber"></param>
    ''' <remarks></remarks>
    Private Sub FillData(ByVal RecordNumber As Integer)

        'Create Instance - Start
        Dim _StudentEn As New StudentEn
        Dim _AccountsEn As New AccountsEn
        Dim _AccountsBAL As New AccountsBAL
        Dim ListStudentEn As New List(Of StudentEn)
        Dim ListSponsorEn As New List(Of SponsorEn)
        Dim _AccountsDetailsEn As AccountsDetailsEn
        Dim ListAccountsDetailsEn As List(Of AccountsDetailsEn)
        'Create Instance - Stop

        'Variable Declarations
        Dim Index As Integer = 0, Index_1 As Integer = 0

        Try

            'Conditions for Button Enable & Disable - Start
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
            'Conditions for Button Enable & Disable - Stop

            If txtRecNo.Text = 0 Then
                txtRecNo.Text = 1
            Else

                If lblCount.Text = 0 Then
                    txtRecNo.Text = 0
                Else
                    Session(ReceiptsClass.SessionRecordNo) = RecordNumber
                    ListObjects = Session(ReceiptsClass.SessionListObject)
                    _AccountsEn = ListObjects(RecordNumber)

                    'populate and set payment method - Start
                    ddlPaymentMode.Items.Clear()
                    ddlPaymentMode.Items.Add(New ListItem("---Select---", "-1"))
                    ddlPaymentMode.DataSource = Session(ReceiptsClass.SessionPayMode)
                    ddlPaymentMode.DataBind()
                    If _AccountsEn.PaymentMode = "" Then
                        ddlPaymentMode.SelectedIndex = "-1"
                    Else
                        ddlPaymentMode.SelectedValue = _AccountsEn.PaymentMode

                    End If

                    'populate and set payment method - Stop

                    'populate and set Bank Code - Start
                    ddlBankCode.Items.Clear()
                    ddlBankCode.Items.Add(New ListItem("---Select---", "-1"))
                    ddlBankCode.DataSource = Session(ReceiptsClass.SessionBankCode)
                    ddlBankCode.DataBind()
                    If _AccountsEn.BankCode = "" Then
                        ddlBankCode.SelectedIndex = "-1"
                    Else
                        ddlBankCode.SelectedValue = _AccountsEn.BankCode

                    End If

                    'populate and set Bank Code - Stop

                    'Set Page Values - Start
                    txtBatchId.Text = _AccountsEn.BatchCode
                    txtBatchDate.Text = _AccountsEn.BatchDate
                    txtReceiptDate.Text = _AccountsEn.TransDate
                    txtDescription.Text = _AccountsEn.Description
                    txtReferenceNo.Text = _AccountsEn.SubReferenceOne
                    txtCtrlAmt.Text = _AccountsEn.ControlAmt

                    Session("CtrlAmt") = txtCtrlAmt.Text
                    'Set Page Values - Stop

                End If

                Session("posted_oustanding_amt") = Nothing

                'Check Module Type - Start
                Select Case GetSubType()

                    Case ReceiptsClass.Student

                        'UPDATED by Hafiz Roslan @ 4/2/2016
                        'STUDENT LOAN - START
                        'updated on 10/2/2016

                        Dim _AccountsDal As New AccountsDAL
                        Dim GetStudOutstndAmt As String = Nothing

                        If ddlReceiptFor.SelectedValue = ReceiptsClass.StudentLoan Then

                            Try
                                Dim loan_outstanding As Double = 0.0

                                _StudentEn.MatricNo = _AccountsEn.CreditRef
                                txtStudentId.Text = _StudentEn.MatricNo
                                _StudentEn.BatchCode = txtBatchId.Text

                                ListStudentEn = _AccountsBAL.GetStudentReceiptsbyBatchID(_StudentEn)

                                'get student loan_outstanding from sass_studentloan - start
                                loan_outstanding = _AccountsDal.GetStudentLoanOutstanding(_StudentEn)
                                'get student loan_outstanding from sass_studentloan - end

                                'loop thro the Account details - Start
                                If Index_1 < ListStudentEn.Count Then

                                    'Create Instance

                                    'Set Entity Values - Start
                                    txtStudentName.Text = ListStudentEn(Index_1).StudentName
                                    txtLoanAmount.Text = String.Format("{0:F}", ListStudentEn(Index_1).TransactionAmount)
                                    txtAllocateAmount.Text = String.Format("{0:F}", txtLoanAmount.Text)

                                    Index_1 = Index_1 + 1

                                End If
                                'loop thro the Account details - Stop

                                lblLoanAmountToPay.Text = String.Format("{0:F}", loan_outstanding)

                                'added by Hafiz Roslan
                                'on 27/01/2016
                                'why To set image followed the status - Start
                                If _AccountsEn.PostStatus = "Posted" Then
                                    lblStatus.Value = "Posted"
                                    ibtnStatus.ImageUrl = "images/Posted.gif"
                                    txtLoanAmount.ReadOnly = True
                                Else
                                    ibtnStatus.ImageUrl = "images/ready.gif"
                                    lblStatus.Value = "Ready"
                                End If
                                'To set image followed the status - End

                            Catch
                                lblMsg.Text = ""
                            End Try
                            'STUDENT LOAN - END

                        Else

                            'Set Entity Values - Start
                            _StudentEn.MatricNo = _AccountsEn.CreditRef
                            _StudentEn.BatchCode = txtBatchId.Text
                            '_StudentEn.Outstanding_Amount = _AccountsEn.Outstanding_Amount
                            '_StudentEn.BankSlipID = _AccountsEn.BankSlipID
                            _StudentEn.StuIndex = 1
                            'Set Entity Values - Stop

                            'Get Student List for the given Matric No
                            ListStudentEn = _AccountsBAL.GetStudentReceiptsbyBatchID(_StudentEn)

                            'Loop thro the Student List - Start
                            While Index < ListStudentEn.Count - 1

                                'Create instance
                                ListAccountsDetailsEn = New List(Of AccountsDetailsEn)

                                Index_1 = 0

                                'loop thro the Account details - Start
                                While Index_1 < ListStudentEn(Index).AccountDetailsList.Count

                                    'Create Instance
                                    _AccountsDetailsEn = New AccountsDetailsEn

                                    'Set Entity Values - Start
                                    _AccountsDetailsEn.TransactionAmount = ListStudentEn(Index).AccountDetailsList(Index_1).TempPaidAmount
                                    _AccountsDetailsEn.PaidAmount = ListStudentEn(Index).AccountDetailsList(Index_1).TempPaidAmount
                                    _AccountsDetailsEn.TaxAmount = ListStudentEn(Index).AccountDetailsList(Index_1).TransactionAmount
                                    _AccountsDetailsEn.TempAmount = ListStudentEn(Index).AccountDetailsList(Index_1).PaidAmount
                                    _AccountsDetailsEn.ReferenceTwo = ListStudentEn(Index).AccountDetailsList(Index_1).ReferenceTwo
                                    _AccountsDetailsEn.ReferenceOne = ListStudentEn(Index).AccountDetailsList(Index_1).ReferenceOne
                                    _AccountsDetailsEn.TransDate = ListStudentEn(Index).AccountDetailsList(Index_1).TransDate
                                    _AccountsDetailsEn.DueDate = ListStudentEn(Index).AccountDetailsList(Index_1).DueDate
                                    _AccountsDetailsEn.TransactionCode = ListStudentEn(Index).AccountDetailsList(Index_1).TransactionCode
                                    _AccountsDetailsEn.CreditRef = ListStudentEn(Index).AccountDetailsList(Index_1).CreditRef
                                    _AccountsDetailsEn.ReferenceCode = ListStudentEn(Index).AccountDetailsList(Index_1).ReferenceCode
                                    'Set Entity Values - Stop

                                    'Add to Accounts Details List
                                    ListAccountsDetailsEn.Add(_AccountsDetailsEn)

                                    'Increment Index
                                    Index_1 = Index_1 + 1

                                End While
                                'loop thro the Account details - Stop

                                'Add Account Details for the Student Record
                                ListStudentEn(Index).AccountDetailsList = ListAccountsDetailsEn

                                Index = Index + 1

                            End While
                            'Loop thro the Student List - Stop

                            'If Student List Available - Start
                            If Not ListStudentEn Is Nothing Then

                                'Added by Hafiz Roslan @ 22/1/2016
                                Dim stud As New StudentEn

                                ListStudentEn.Add(stud)

                                'Bind Student Data Grid - Start
                                dgStudentView.DataSource = ListStudentEn
                                dgStudentView.DataBind()
                                'Bind Student Data Grid - Stop

                                'Set Session
                                Session(ReceiptsClass.SessionListView) = ListStudentEn

                                'Variable Declarations - Start
                                Dim AmountTextBox As TextBox = Nothing, txt_matricno As TextBox = Nothing
                                Dim TotalAmount As Double = 0, _DataGridItem As DataGridItem = Nothing, Index_2 As Integer = 0
                                Dim TransAmount As Double = 0, _CheckBox As CheckBox = Nothing, SelectCheckBox As CheckBox = Nothing
                                'Variable Declarations - Stop

                                'Loop thro the Data grid Items - Start
                                For Each _DataGridItem In dgStudentView.Items

                                    'Get Grid Controls - Start
                                    'Commented by Hafiz Roslan @ 12/01/2016
                                    'Disable grid item Auto/Manual
                                    '_CheckBox = _DataGridItem.Cells(8).Controls(1)
                                    'End Disable
                                    AmountTextBox = _DataGridItem.Cells(9).Controls(1)
                                    SelectCheckBox = _DataGridItem.Cells(0).Controls(1)
                                    txt_matricno = _DataGridItem.Cells(2).Controls(1)
                                    'Get Grid Controls - Stop

                                    'Set Grid Values - Start
                                    SelectCheckBox.Checked = True
                                    TransAmount = _DataGridItem.Cells(10).Text
                                    AmountTextBox.Text = String.Format("{0:F}", TransAmount)
                                    TotalAmount = TotalAmount + TransAmount
                                    txtAllocateAmount.Text = String.Format("{0:F}", TotalAmount)
                                    txt_matricno.Text = ListStudentEn(Index_2).CreditRef
                                    'Set Grid Values - Stop

                                    'get & Set Grid Controls - Start
                                    DirectCast(dgStudentView.Items(Index_2).Cells(16).FindControl("TxtAmt"), TextBox).Text =
                                      String.Format("{0:F}", clsGeneric.NullToDecimal(ListStudentEn(_DataGridItem.ItemIndex).TransactionAmount))
                                    'DirectCast(dgStudentView.Items(Index_2).Cells(16).FindControl("BankSlipID"), TextBox).Text =
                                    '    ListStudentEn(_DataGridItem.ItemIndex).TransactionCode
                                    'DirectCast(dgStudentView.Items(Index_2).Cells(17).FindControl("txtTransDate"), TextBox).Text =
                                    '    ListStudentEn(_DataGridItem.ItemIndex).ReceiptDate.ToString("yyyy-MM-dd")
                                    DirectCast(_DataGridItem.FindControl("txtTransDate"), TextBox).Text = ListStudentEn(_DataGridItem.ItemIndex).ReceiptDate.ToString("yyyy-MM-dd")
                                    'get & Set Grid Controls - Stop
                                    DirectCast(_DataGridItem.FindControl("BankSlipID"), TextBox).Text = ListStudentEn(Index_2).BankSlipID
                                    '_DataGridItem.Cells(18).Text = ListStudentEn(Index_2).Outstanding_Amount
                                    'DirectCast(_DataGridItem.FindControl("Outstanding_Amount"), Label).Text = ListStudentEn(Index_2).Outstanding_Amount

                                    'Modified by Hafiz @ 23/3/2016
                                    'modified by Hafiz @ 25/4/2016
                                    'Outstanding amount fixes - start
                                    Dim _stud As New StudentEn
                                    _stud = _AccountsDal.GetStudentOutstanding(ListStudentEn(Index_2).CreditRef)

                                    GetStudOutstndAmt = (_stud.OutstandingAmount) + (_stud.LoanAmount)

                                    DirectCast(_DataGridItem.FindControl("Outstanding_Amount"), Label).Text = String.Format("{0:F}", CDbl(GetStudOutstndAmt))
                                    'Outstanding amount fixes - end

                                    _DataGridItem.Cells(11).Text = Index_2

                                    'Increment Index
                                    Index_2 = Index_2 + 1

                                    EnableDisableDeleteButton(_DataGridItem)

                                    'updated by Hafiz Roslan @ 28/01/2016
                                    If _AccountsEn.PostStatus = "Posted" Then

                                        PostedDisableButton(_DataGridItem)
                                        Session("posted_oustanding_amt") = Nothing

                                    End If
                                Next
                                'Loop thro the Data grid Items - Stop

                                'Set Post Status - Start
                                If _AccountsEn.PostStatus = "Posted" Then
                                    lblStatus.Value = "Posted"
                                    ibtnStatus.ImageUrl = "images/Posted.gif"
                                Else
                                    ibtnStatus.ImageUrl = "images/ready.gif"
                                    lblStatus.Value = "Ready"
                                End If
                                'Set Post Status - Start

                            End If
                            'If Student List Available - Stop
                        End If

                    Case ReceiptsClass.Sponsor
                        Dim SponsorDts As New SponsorDAL
                        Dim SponsorEnt As New SponsorEn
                        SponsorEnt.SponsorID = _AccountsEn.CreditRef
                        Try
                            Session("eobjspn") = SponsorDts.GetItem(SponsorEnt)
                        Catch
                            lblMsg.Text = ""
                        End Try

                        txtAllocateAmount.Text = String.Format("{0:F}", _AccountsEn.TransactionAmount)
                        txtSpnAmount.Text = String.Format("{0:F}", _AccountsEn.TransactionAmount)
                        Call PopulateSponsorInvoice(_AccountsEn.CreditRef, _AccountsEn.TranssactionID)

                        'added by Hafiz Roslan
                        'on 27/01/2016
                        'why To set image followed the status - Start
                        If _AccountsEn.PostStatus = "Posted" Then
                            lblStatus.Value = "Posted"
                            ibtnStatus.ImageUrl = "images/Posted.gif"
                        Else
                            ibtnStatus.ImageUrl = "images/ready.gif"
                            lblStatus.Value = "Ready"
                        End If
                        'To set image followed the status - End

                End Select
                'Check Module Type - Stop

                CheckWorkflowStatus(_AccountsEn)

            End If

            'Set Date Format
            Call setdateFormat()

        Catch ex As Exception

            'Log & Show Error - Start
            Call MaxModule.Helper.LogError(ex.Message)
            Call SetMessage(ex.Message)
            'Log & Show Error - Stop

        End Try


        'If txtRecNo.Text = 0 Then
        '    txtRecNo.Text = 1
        'Else

        '    If lblCount.Text = 0 Then
        '        txtRecNo.Text = 0
        '    Else
        '        'Session("recno") = RecNo
        '        'Dim obj As AccountsEn
        '        'ListObjects = Session("ListObj")
        '        'obj = ListObjects(RecNo)
        '        ''If obj.SubType = "Student" Then
        '        ''    ddlReceiptFor.SelectedValue = "St"
        '        ''ElseIf obj.SubType = "Sponsor" Then
        '        ''    ddlReceiptFor.SelectedValue = "Sp"
        '        ''End If


        '        Dim Trcptobj As New AccountsBAL
        '        Dim listTrcpt As New List(Of StudentEn)
        '        Dim spnlist As New List(Of SponsorEn)
        '        'Dim eobj As New SponsorEn
        '        Dim bsobjspn As New AccountsBAL
        '        check_Receiptfor()




        '        If ddlReceiptFor.SelectedValue = "Sp" Then
        '            txtSponCode.ReadOnly = False
        '            txtSponName.ReadOnly = False
        '            'eobj.SponserCode = obj.CreditRef
        '            'eobj.BatchCode = txtBatchId.Text

        '            Try
        '                spnlist = bsobjspn.GetSponserListByBatchID(txtBatchId.Text)

        '            Catch ex As Exception
        '                LogError.Log("Receipts", "FillData", ex.Message)
        '                lblMsg.Text = ex.Message
        '            End Try
        '            If Not spnlist Is Nothing Then
        '                txtSponCode.Text = spnlist(0).SponserCode
        '                txtSponName.Text = spnlist(0).Name
        '                Session("listsp") = Nothing
        '                Session("listsp") = spnlist
        '            End If

        '            txtSponCode.Text = obj.CreditRef
        '            txtSpnAmount.Text = obj.TransactionAmount
        '            spnamount()
        '            txtSponCode.ReadOnly = True
        '            txtSponName.ReadOnly = True
        '            If obj.PostStatus = "Posted" Then
        '                lblStatus.Value = "Posted"
        '                ibtnStatus.ImageUrl = "images/Posted.gif"
        '            Else
        '                ibtnStatus.ImageUrl = "images/ready.gif"
        '                lblStatus.Value = "Ready"
        '            End If

        '        ElseIf ddlReceiptFor.SelectedValue = "Sl" Then
        '            'Load Student Loan Details
        '            txtStudentId.ReadOnly = False
        '            txtStudentName.ReadOnly = False
        '            Dim sobj As New StudentEn
        '            Dim studentBAL As New StudentBAL
        '            sobj.MatricNo = obj.CreditRef

        '            Try
        '                sobj = studentBAL.GetItem(sobj.MatricNo)
        '            Catch ex As Exception
        '                LogError.Log("Receipts", "FillData", ex.Message)
        '                lblMsg.Text = ex.Message
        '            End Try

        '            txtStudentId.Text = sobj.MatricNo
        '            txtStudentName.Text = sobj.StudentName
        '            txtAllocateAmount.Text = String.Format("{0:F}", obj.TransactionAmount)
        '            txtLoanAmount.Text = String.Format("{0:F}", obj.TransactionAmount)


        '            txtStudentId.ReadOnly = True
        '            txtStudentName.ReadOnly = True
        '            If obj.PostStatus = "Posted" Then
        '                lblStatus.Value = "Posted"
        '                ibtnStatus.ImageUrl = "images/Posted.gif"
        '            Else
        '                ibtnStatus.ImageUrl = "images/ready.gif"
        '                lblStatus.Value = "Ready"
        '            End If

        '        Else


        '        End If
        '    End If
        'End If

    End Sub

#End Region

#Region "setdateFormat "

    ''' <summary>
    ''' Method To Change the Date Format(dd/MM/yyyy)
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub setdateFormat()
        'Dim GBFormat As System.Globalization.CultureInfo
        'GBFormat = New System.Globalization.CultureInfo("en-GB")
        Dim myReceiptDate As Date = CDate(CStr(txtReceiptDate.Text))
        Dim myFormat As String = "dd/MM/yyyy"
        Dim myFormattedDate As String = Format(myReceiptDate, myFormat)
        txtReceiptDate.Text = myFormattedDate
        Dim myBatchDate As Date = CDate(CStr(txtBatchDate.Text))
        Dim myFormattedDate1 As String = Format(myBatchDate, myFormat)
        txtBatchDate.Text = myFormattedDate1
    End Sub

#End Region

#Region "LoadListObjects "

    ''' <summary>
    ''' Method to get the List Of Receipts
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub LoadListObjects(ByVal IsPostedRecords As Boolean)

        'Create Instances - Start
        Dim _AccountsEn As New AccountsEn
        Dim _AccountsBAL As New AccountsBAL
        Dim ListAccountsEn As New List(Of AccountsEn)
        'Create Instances - Stop

        'Variable Declarations
        Dim TotalRecords As Integer = 0

        Try
            If Session(ReceiptsClass.SessionLoadData) =
                ReceiptsClass.StatusView Then

                'Set Values - Start
                _AccountsEn.BatchIntake = String.Empty
                If Session("PostSts") = "Posted" Then
                    _AccountsEn.PostStatus = ReceiptsClass.StatusPosted
                    Session("PostSts") = Nothing
                Else

                    'commented by Hafiz Roslan @ 4/2/2016
                    'If ddlReceiptFor.SelectedValue = ReceiptsClass.StudentLoan Then
                    '    _AccountsEn.Category = ReceiptsClass.Loan
                    '    Call SetMessage("Records does not Exist")
                    '    Exit Sub
                    'End If

                    If Request.QueryString("BatchCode") <> "" Then
                        _AccountsEn.PostStatus = ReceiptsClass.StatusPosted

                    Else
                        _AccountsEn.PostStatus = ReceiptsClass.StatusReady
                    End If

                    lblStatus.Value = ReceiptsClass.StatusReady
                End If

                'updated by Hafiz Roslan @ 10/2/2016
                _AccountsEn.Category = ReceiptsClass.CategoryReceipt
                'Set Values - Start

                'Get Batch Code - Start
                If txtBatchId.Text <> "Auto Number" Then
                    _AccountsEn.BatchCode = GetBatchCode()
                Else
                    _AccountsEn.BatchCode = String.Empty
                End If
                'Get Batch Code - Stop

                'Check Module Type to Set Sub Type - Start
                'Modified by Hafiz @ 16/2/2016 - add subcategory for Receipt for Student Loan
                Dim SubType As String = ddlReceiptFor.SelectedValue

                Select Case SubType

                    Case ReceiptsClass.ReceiptStudent
                        _AccountsEn.SubType = ReceiptsClass.Student

                    Case ReceiptsClass.ReceiptSponsor
                        _AccountsEn.SubType = ReceiptsClass.Sponsor

                    Case ReceiptsClass.StudentLoan
                        _AccountsEn.SubType = ReceiptsClass.Student
                        _AccountsEn.SubCategory = ReceiptsClass.Loan

                End Select
                'Check Module Type to Set Sub Type - Stop

                'If Option Posted Records - Start
                If IsPostedRecords Then
                    'Set Status to Posted
                    _AccountsEn.PostStatus = ReceiptsClass.StatusPosted
                End If
                'If Option Posted Records - Stop

                If CInt(Request.QueryString("IsView")).Equals(1) Then
                    _AccountsEn.PostStatus = ReceiptsClass.StatusReady
                End If

                'Load List Object
                ListObjects = _AccountsBAL.GetTransactions(_AccountsEn)

            End If

            'Set Session Values - Start
            Session(ReceiptsClass.SessionLoadData) = Nothing
            Session(ReceiptsClass.SessionListObject) = ListObjects
            Session(ReceiptsClass.SessionReceiptList) = ListObjects
            'Set Session Values - Stop

            'Get Total Records
            TotalRecords = ListObjects.Count

            'Display Total Records
            lblCount.Text = TotalRecords

            'if Records Exist - Start
            If TotalRecords > 0 Then

                'Disable Record Navigator
                Call DisableRecordNavigator()

                'Set Record No
                txtRecNo.Text = "1"

                'On Move First
                Call OnMoveFirst()

                'Check if User has rights to Edit - Start
                If Session(ReceiptsClass.SessionEditFlag) = True Then
                    ibtnSave.Enabled = True
                    txtSpnCode.Enabled = False
                    ibtnSave.ImageUrl = "images/save.png"
                    Session(ReceiptsClass.SessionPageMode) = ReceiptsClass.StatusEdit
                Else
                    ibtnSave.Enabled = False
                    ibtnSave.ImageUrl = "images/gsave.png"
                    Session(ReceiptsClass.SessionPageMode) = String.Empty
                End If
                'Check if User has rights to Edit - Stop

            Else

                txtRecNo.Text = String.Empty
                lblCount.Text = String.Empty
                If Not DeleteFlag = ReceiptsClass.StatusDelete Then
                    DeleteFlag = String.Empty
                    Call SetMessage("Records does not Exist")
                End If

            End If
            'if Records Exist - Stop

        Catch ex As Exception

            'Log & Show Error - Start
            Call MaxModule.Helper.LogError(ex.Message)
            Call SetMessage(ex.Message)
            'Log & Show Error - Stop

        End Try

        'Dim Mylist As New List(Of AccountsEn)
        'Dim obj As New AccountsBAL
        'Dim eob As New AccountsEn
        'If Session("loaddata") = "others" Then

        '    eob.Category = "Receipt"
        '    eob.PostStatus = "Posted"
        '    eob.BatchIntake = ""

        '    If txtBatchDate.Text <> "" Then
        '        eob.BatchDate = txtBatchDate.Text
        '    End If

        '    If ddlReceiptFor.SelectedValue = "Sp" Then
        '        eob.SubType = "Sponsor"
        '    ElseIf ddlReceiptFor.SelectedValue = "St" Then
        '        eob.SubType = "Student"
        '    ElseIf ddlReceiptFor.SelectedValue = "Sl" Then
        '        eob.SubType = "Student"
        '    End If
        '    If txtBatchId.Text <> "" Then
        '        eob.BatchCode = txtBatchId.Text
        '    Else
        '        eob.BatchCode = ""
        '    End If

        '    Try
        '        If ddlReceiptFor.SelectedValue = "Sl" Then
        '            ListObjects = obj.GetLoanTransactions(eob)
        '        Else
        '            ListObjects = obj.GetTransactions(eob)
        '        End If

        '    Catch ex As Exception
        '        LogError.Log("Receipts", "LoadListObjects", ex.Message)
        '    End Try
        '    'Removing duplicate batchid object from the list
        '    Dim i As Integer = 0
        '    While i < ListObjects.Count
        '        Dim j As Integer = 0
        '        Dim objcount As Boolean = False
        '        While j < Mylist.Count
        '            If Mylist(j).BatchCode = ListObjects(i).BatchCode Then
        '                objcount = True
        '                Exit While
        '            End If
        '            j = j + 1
        '        End While
        '        If objcount = False Then
        '            Mylist.Add(ListObjects(i))
        '        End If
        '        i = i + 1
        '    End While
        '    ListObjects = Nothing
        '    ListObjects = Mylist
        'ElseIf Session("loaddata") = "View" Then

        '    eob.Category = "Receipt"
        '    eob.PostStatus = "Ready"
        '    eob.BatchIntake = ""

        '    If txtBatchDate.Text <> "" Then
        '        eob.BatchDate = txtBatchDate.Text
        '    End If

        '    If ddlReceiptFor.SelectedValue = "Sp" Then
        '        eob.SubType = "Sponsor"
        '    ElseIf ddlReceiptFor.SelectedValue = "St" Then
        '        eob.SubType = "Student"
        '    ElseIf ddlReceiptFor.SelectedValue = "Sl" Then
        '        eob.SubType = "Student"
        '    End If
        '    If txtBatchId.Text <> "Auto Number" Then
        '        eob.BatchCode = txtBatchId.Text
        '    Else
        '        eob.BatchCode = ""
        '    End If

        '    If ddlReceiptFor.SelectedValue = "Sl" Then
        '        ListObjects = obj.GetLoanTransactions(eob)
        '    Else
        '        ListObjects = obj.GetTransactions(eob)
        '    End If

        '    Dim i As Integer = 0
        '    'Removing Duplicates
        '    While i < ListObjects.Count
        '        Dim j As Integer = 0
        '        Dim objcount As Boolean = False
        '        While j < Mylist.Count
        '            If Mylist(j).BatchCode = ListObjects(i).BatchCode Then
        '                objcount = True
        '                Exit While
        '            End If
        '            j = j + 1
        '        End While
        '        If objcount = False Then
        '            Mylist.Add(ListObjects(i))
        '        End If
        '        i = i + 1
        '    End While
        '    ListObjects = Nothing
        '    ListObjects = Mylist
        'End If
        'Session("loaddata") = Nothing
        'Session("ListObj") = ListObjects
        'Session("RecptList") = ListObjects
        'lblCount.Text = ListObjects.Count.ToString()
        'If ListObjects.Count <> 0 Then
        '    DisableRecordNavigator()
        '    txtRecNo.Text = "1"
        '    OnMoveFirst()
        '    If Session("EditFlag") = True Then
        '        Session("PageMode") = "Edit"
        '        txtSpnCode.Enabled = False
        '        ibtnSave.Enabled = True
        '        ibtnSave.ImageUrl = "images/save.png"
        '    Else
        '        Session("PageMode") = ""
        '        ibtnSave.Enabled = False
        '        ibtnSave.ImageUrl = "images/gsave.png"
        '    End If
        'Else
        '    txtRecNo.Text = ""
        '    lblCount.Text = ""
        '    If DeleteFlag = "Delete" Then
        '    Else
        '        lblMsg.Visible = True
        '        ErrorDescription = "Records did not Exist"
        '        lblMsg.Text = ErrorDescription
        '        DeleteFlag = ""
        '    End If
        'End If
    End Sub

#End Region

#Region "onPost "

    ''' <summary>
    ''' Method to Post Payments
    ''' </summary>
    ''' <remarks></remarks>
    Protected Function OnPost() As Boolean

        'Create Instances
        Dim result As Boolean = False
        Dim _ReceiptClass As New ReceiptsClass
        Dim _AccountsDAL As New AccountsDAL

        'Variable Declarations
        Dim BatchCode As String = Nothing
        Dim msg As String = Nothing

        Try

            'Get Batch Code
            BatchCode = clsGeneric.NullToString(txtBatchId.Text)

            'Post to Work flow - Start
            'Modified by Hafiz @ 16/2/2016
            If _ReceiptClass.PostToWorkflow(BatchCode, GetDoneBy(), ddlReceiptFor.SelectedValue, "Receipts.aspx.vb", msg) Then

                result = True
                'Show Message
                Call SetMessage(msg)

            Else

                result = False
                'Show Message
                Call SetMessage("Record Already Posted")

            End If
            'Post to Work flow - Stop

        Catch ex As Exception

            'Log & Show Error - Start
            Call MaxModule.Helper.LogError(ex.Message)
            Call SetMessage(ex.Message)
            'Log & Show Error - Stop

        End Try

        Return result

        'Dim eobjDetails As New AccountsDetailsEn
        'Dim list As New List(Of AccountsDetailsEn)
        'Dim eobj As New AccountsEn
        'Dim bsobj As New AccountsBAL
        'Dim LstTRDetails As New List(Of AccountsEn)

        'eobj.TransDate = Trim(txtReceiptDate.Text)
        'eobj.BatchCode = txtBatchId.Text
        'If txtBatchDate.Text <> "" Then
        '    eobj.BatchDate = Trim(txtBatchDate.Text)
        'Else
        '    eobj.BatchDate = Format(Date.Now, "dd/MM/yyyy")
        'End If
        'If (ddlReceiptFor.SelectedValue = "St") Then
        '    eobj.SubType = "Student"
        'Else
        '    eobj.SubType = "Sponsor"
        'End If
        'eobj.BatchTotal = CDbl(txtAllocateAmount.Text)
        'If Trim(txtAddedAmount.Text).Length = 0 Then
        '    eobj.PaidAmount = 0
        'Else
        '    eobj.PaidAmount = CDbl(txtAddedAmount.Text)
        'End If
        'eobj.Description = txtDescription.Text
        'eobj.Category = "Receipt"
        'eobj.PostStatus = "Posted"
        'eobj.SubReferenceOne = txtReferenceNo.Text
        'eobj.TransStatus = "Open"
        'eobj.TransType = "Debit"
        'eobj.PaymentMode = ddlPaymentMode.SelectedValue
        'eobj.PostedDateTime = DateTime.Now
        'eobj.DueDate = DateTime.Now
        'eobj.UpdatedTime = DateTime.Now
        'eobj.ChequeDate = DateTime.Now
        'eobj.CreatedDateTime = DateTime.Now
        'eobj.BankCode = ddlBankCode.SelectedValue
        'eobj.PostedBy = Session("User")
        'eobj.UpdatedBy = Session("User")
        'Dim liststudentListView As New List(Of StudentEn)
        'Dim listUnMacth As New List(Of StudentEn)
        'Dim listsponser As New List(Of SponsorEn)
        'Dim eospn As New SponsorEn
        'Dim dgItem1 As DataGridItem
        'Dim eTUnmatch As New StudentEn
        'If ddlReceiptFor.SelectedValue = "Sp" Then
        '    If txtSponCode.Text = "" Or txtSpnAmount.Text = "" Then
        '        lblMsg.Text = " Select a Sponsor. "
        '        Exit Sub
        '    Else
        '        eobj.CreditRef = txtSponCode.Text
        '        eobj.TransactionAmount = txtSpnAmount.Text
        '        eospn.SponserCode = txtSponCode.Text
        '        eospn.Name = txtSponName.Text
        '        listsponser.Add(eospn)

        '    End If
        '    If txtSpnAmount.Text = "" Or txtSpnAmount.Text = 0 Then
        '        lblMsg.Text = " Enter Sponsor Amount "
        '        Exit Sub
        '    End If
        'Else
        '    liststudentListView = Session("listview")
        '    If liststudentListView Is Nothing Then
        '        lblMsg.Text = "Select At least One Student. "
        '        Exit Sub
        '    Else
        '        liststudentListView = Session("listview")
        '    End If
        '    LoadTotals()
        '    Dim txtAmount As TextBox
        '    Dim selchk As New CheckBox
        '    For Each dgItem1 In dgStudentView.Items
        '        Dim p As Integer = 0
        '        selchk = dgItem1.Cells(0).Controls(1)
        '        If selchk.Checked = True Then
        '            While p < liststudentListView.Count
        '                Dim match As Boolean = False
        '                If liststudentListView(p).noAkaun Is Nothing Then
        '                    If dgItem1.Cells(2).Text = liststudentListView(p).MatricNo Then
        '                        match = True
        '                    End If
        '                Else
        '                    If dgItem1.Cells(2).Text = liststudentListView(p).MatricNo And dgItem1.Cells(15).Text = liststudentListView(p).noAkaun Then
        '                        match = True
        '                    End If
        '                End If

        '                If match = True Then
        '                    eTUnmatch = New StudentEn
        '                    txtAmount = dgItem1.Cells(9).Controls(1)
        '                    eTUnmatch.MatricNo = dgItem1.Cells(2).Text
        '                    eTUnmatch.StudentName = dgItem1.Cells(3).Text
        '                    eTUnmatch.ICNo = dgItem1.Cells(4).Text
        '                    eTUnmatch.TransactionAmount = txtAmount.Text
        '                    eTUnmatch.PaidAmount = liststudentListView(p).AmountPaid
        '                    eTUnmatch.SubReferenceTwo = liststudentListView(p).SubReferenceTwo
        '                    If (liststudentListView(p).AccountDetailsList Is Nothing) Then
        '                        'If (liststudentListView(p).AccountDetailsList.Count <= 0) Then
        '                        eTUnmatch.SubReferenceTwo = "Auto"
        '                    Else
        '                        If (liststudentListView(p).AccountDetailsList.Count = 0) Then
        '                            eTUnmatch.SubReferenceTwo = "Auto"
        '                        End If
        '                    End If
        '                    eTUnmatch.AccountDetailsList = liststudentListView(p).AccountDetailsList
        '                    listUnMacth.Add(eTUnmatch)
        '                    eTUnmatch = Nothing

        '                End If
        '                p = p + 1
        '            End While
        '        End If
        '    Next
        'End If
        'If (ddlReceiptFor.SelectedValue = "St") Then
        '    If listUnMacth.Count = 0 Then
        '        ErrorDescription = "Select At least One Student"
        '        lblMsg.Text = ErrorDescription
        '        Exit Sub
        '    End If
        '    If txtAllocateAmount.Text = 0 Then
        '        lblMsg.Text = " Enter Student Amount "
        '        Exit Sub
        '    End If
        'End If
        'Try
        '    If ddlReceiptFor.SelectedValue = "Sp" Then
        '        txtBatchId.Text = bsobj.SponsorBatchUpdate(eobj, listsponser)
        '    Else
        '        txtBatchId.Text = bsobj.StudentBatchUpdate(eobj, listUnMacth)
        '        LoadReport(txtBatchId.Text)
        '    End If

        '    txtBatchId.ReadOnly = False
        '    txtBatchId.Text = eobj.BatchCode
        '    txtBatchId.ReadOnly = True
        '    ErrorDescription = "Record Posted Successfully "
        '    ibtnStatus.ImageUrl = "images/posted.gif"
        '    lblStatus.Value = "Posted"
        '    lblMsg.Text = ErrorDescription

        '    If txtBatchId.Text <> " " Then
        '        Dim strBatchNo As String = Trim(txtBatchId.Text)

        '        'Get Status Integration To SAGA
        '        dsReturn = objIntegrationDL.GetIntegrationStatus()

        '        'Check Status Integration To SAGA
        '        If dsReturn.Tables(0).Rows(0).Item("CON_Value2") = "1" Then
        '            objIntegration.Receipt(strBatchNo)
        '        Else
        '            ErrorDescription = "Record Posted Successfully But No Integration To CF. Please Call Administrator "
        '            lblMsg.Text = ErrorDescription
        '        End If
        '    End If

        'Catch ex As Exception
        '    lblMsg.Text = ex.Message.ToString()
        '    LogError.Log("Receipts", "OnPost", ex.Message)
        'End Try


    End Function

#End Region

#Region "OnPostStudentLoan "

    ''' <summary>
    ''' Method to Post Payments
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub OnPostStudentLoan()
        Dim eobj As New AccountsEn
        Dim bsobj As New AccountsBAL
        Dim eobstu As New StudentEn
        lblMsg.Text = ""
        lblMsg.Visible = True

        eobj.TransDate = Trim(txtReceiptDate.Text)
        eobj.BatchCode = txtBatchId.Text
        eobj.BatchDate = Trim(txtBatchDate.Text)
        eobj.BatchTotal = CDbl(txtAllocateAmount.Text)
        eobj.PaidAmount = 0
        eobj.Description = txtDescription.Text
        eobj.Category = "Receipt"
        eobj.PostStatus = "Posted"
        eobj.SubReferenceOne = txtReferenceNo.Text
        eobj.TransStatus = "Open"
        eobj.PaymentMode = ddlPaymentMode.SelectedValue
        eobj.PostedBy = GetDoneBy()
        eobj.PostedDateTime = DateTime.Now
        eobj.DueDate = DateTime.Now
        eobj.UpdatedTime = DateTime.Now
        eobj.ChequeDate = DateTime.Now
        eobj.CreatedDateTime = DateTime.Now
        eobj.BankCode = ddlBankCode.SelectedValue
        eobj.UpdatedBy = Session("User")
        eobj.CreditRef = Trim(txtStudentId.Text)
        eobj.SubType = "Student"
        eobj.TransType = "Credit"
        eobj.TransactionAmount = CDbl(txtLoanAmount.Text)

        If Not Session("eobjstu") Is Nothing Then
            eobstu = Session("eobjstu")
        End If

        Dim bid As String = ""
        lblMsg.Visible = True
        'Status=New
        Try
            txtBatchId.Text = bsobj.StudentLoanUpdate(eobj, eobstu)
            ErrorDescription = "Record Posted Successfully "
            txtBatchId.ReadOnly = True
            ibtnStatus.ImageUrl = "images/posted.gif"
            lblMsg.Visible = True
            lblMsg.Text = ErrorDescription
            lblStatus.Value = "Posted"
            eobj.PostStatus = "Posted"

            lblMsg.Text = ErrorDescription
        Catch ex As Exception
            lblMsg.Text = ex.Message.ToString()
            LogError.Log("Payments", "OnPost", ex.Message)
        End Try

    End Sub

#End Region

#Region "LoadReport "

    ''' <summary>
    ''' Method to Load Reports
    ''' </summary>
    ''' <param name="batch"></param>
    ''' <remarks></remarks>
    Private Sub LoadReport(ByVal batch As String)
        If ddlPaymentMode.SelectedValue = "EFT" Then
            Session("LaporanHarian") = True
        End If
        Dim scriptstringOpen As String = "OpenWindow();"
        ClientScript.RegisterStartupScript(Me.GetType(), "OpenWindow", scriptstringOpen, True)

    End Sub

#End Region

#Region "LoadInvTotals "

    ''' <summary>
    ''' Method to Add all Invoice Amounts in the Invoice Grid
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub LoadInvTotals()
        Dim chk As CheckBox
        Dim txtAmount As TextBox
        Dim dgItem1 As DataGridItem
        Dim totalAmt1 As Double = 0
        Dim BalAmt As Double = 0.0
        For Each dgItem1 In dgInvoices.Items
            Dim totalAmt As Double = 0
            chk = dgItem1.Cells(0).Controls(1)
            If chk.Checked = True Then
                Dim AllAmt As Double = 0
                Dim Allpck As Double = 0
                txtAmount = dgItem1.Cells(5).Controls(1)
                If txtAmount.Text <> "" Then
                    BalAmt = CDbl(txtAmount.Text)
                End If
                totalAmt1 += BalAmt
            End If

        Next
        txtALLAmount.Text = String.Format("{0:F}", totalAmt1)

    End Sub

#End Region

#Region "LoadTotals "

    ''' <summary>
    ''' Method to Add all Amounts in the Student Grid
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub LoadTotals()
        Dim chk As CheckBox
        Dim txtAmount As TextBox
        Dim dgItem1 As DataGridItem
        Dim totalAmt1 As Double = 0
        Dim BalAmt As Double = 0.0
        ' DirectCast(dgStudentView.Items(0).Cells(17).FindControl("txtBankInSlip"),TextBox)

        For Each dgItem1 In dgStudentView.Items
            Dim totalAmt As Double = 0
            chk = dgItem1.Cells(0).Controls(1)
            If chk.Checked = True Then
                Dim AllAmt As Double = 0
                Dim Allpck As Double = 0
                txtAmount = dgItem1.Cells(9).Controls(1)

                If txtAmount.Text <> "" Then
                    BalAmt = CDbl(txtAmount.Text)
                End If
                totalAmt1 += BalAmt
            End If

        Next
        txtAllocateAmount.Text = String.Format("{0:F}", totalAmt1)

    End Sub

#End Region

#Region "SpaceValidation "

    ''' <summary>
    ''' Method to Validate
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub SpaceValidation()
        Dim GBFormat As System.Globalization.CultureInfo
        GBFormat = New System.Globalization.CultureInfo("en-GB")

        'Receipt For
        If ddlReceiptFor.SelectedValue = "-1" Then
            lblMsg.Text = "Select a Receipt For"
            lblMsg.Visible = True
            ddlReceiptFor.Focus()
            Exit Sub
        End If
        'Payment Mode
        If ddlPaymentMode.SelectedValue = "-1" Then
            lblMsg.Text = "Select a Payment Mode"
            lblMsg.Visible = True
            ddlPaymentMode.Focus()
            Exit Sub
        End If

        'Bank Code
        If ddlBankCode.SelectedValue = "-1" Then
            lblMsg.Text = "Select a Bank Code"
            lblMsg.Visible = True
            ddlBankCode.Focus()
            Exit Sub
        End If
        If Trim(txtDescription.Text).Length = 0 Then
            txtDescription.Text = Trim(txtDescription.Text)
            lblMsg.Text = "Enter Valid Description "
            lblMsg.Visible = True
            txtDescription.Focus()
            Exit Sub
        End If

        'Batch date
        If Trim(txtBatchDate.Text).Length < 10 Then
            lblMsg.Text = "Enter Valid Batch Date"
            lblMsg.Visible = True
            txtBatchDate.Focus()
            Exit Sub
        Else
            Try
                txtBatchDate.Text = DateTime.Parse(txtBatchDate.Text, GBFormat)
            Catch ex As Exception
                lblMsg.Text = "Enter Valid Batch Date"
                lblMsg.Visible = True
                txtBatchDate.Focus()
                Exit Sub
            End Try
        End If

        'Due date
        If Trim(txtReceiptDate.Text).Length < 10 Then
            lblMsg.Text = "Enter Valid Due Date"
            lblMsg.Visible = True
            txtReceiptDate.Focus()
            Exit Sub
        Else
            Try
                txtReceiptDate.Text = DateTime.Parse(txtReceiptDate.Text, GBFormat)
            Catch ex As Exception
                lblMsg.Text = "Enter Valid Due Date"
                lblMsg.Visible = True
                txtReceiptDate.Focus()
                Exit Sub
            End Try
        End If
        If lblStatus.Value = "Posted" Then
            lblMsg.Text = "Record Already Posted"
            lblMsg.Visible = True
            Exit Sub
        End If

    End Sub

#End Region

#Region "onallocateAmount "

    ''' <summary>
    ''' Method to Caliculate Allocation Amounts in Grid
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub onallocateAmount()
        Dim dgi As DataGridItem
        Dim txtAmount As TextBox
        Dim txtPdAmount As TextBox
        Dim chkbox As CheckBox
        Dim alamount As Double
        Dim pamount As Double
        Dim AddedAmount As Double = 0
        txtAddedAmount.Text = "0.0"
        'Loading Aotu Allocated amounts
        For Each dgi In dgInvoices.Items

            txtAmount = dgi.Cells(6).Controls(1)
            txtPdAmount = dgi.Cells(7).Controls(1)

            AddedAmount = CDbl(txtAddedAmount.Text) + CDbl(txtAmount.Text)
            txtAddedAmount.Text = String.Format("{0:F}", AddedAmount)
            If CDbl(txtAllocateAmount.Text) >= AddedAmount Then
                chkbox = dgi.Cells(0).Controls(1)
                If chkbox.Checked = True Then

                    alamount = txtAmount.Text
                    If dgi.Cells(5).Text > alamount Then
                        txtPdAmount.Text = dgi.Cells(5).Text - alamount
                    Else
                        txtAmount = dgi.Cells(6).Controls(1)
                        txtAmount.Text = "0.00"
                        txtPdAmount = dgi.Cells(7).Controls(1)
                        txtPdAmount.Text = "0.00"
                    End If
                    pamount = txtPdAmount.Text
                    txtAmount.Text = String.Format("{0:F}", alamount)
                    txtPdAmount.Text = String.Format("{0:F}", pamount)

                Else
                    txtAmount = dgi.Cells(6).Controls(1)
                    txtAmount.Text = "0.00"
                    txtPdAmount = dgi.Cells(7).Controls(1)
                    txtPdAmount.Text = "0.00"

                End If
            Else

                AddedAmount = CDbl(txtAddedAmount.Text) - CDbl(txtAmount.Text)
                txtAmount.Text = "0.00"
                txtAddedAmount.Text = String.Format("{0:F}", AddedAmount)
            End If
        Next


    End Sub

#End Region

#Region "ondelete "

    ''' <summary>
    ''' Method to Delete the Transactions
    ''' </summary>
    ''' <remarks></remarks> 
    Private Sub ondelete()
        Dim RecAff As Boolean
        Dim eob As New AccountsEn
        Dim bsobj As New AccountsBAL
        If lblStatus.Value = "Ready" Then
            Try
                eob.BatchCode = Trim(txtBatchId.Text)
                RecAff = bsobj.BatchDelete(eob)
                onAdd()
                DeleteFlag = "Delete"
                Session("loaddata") = "View"
                lblMsg.Text = "Record Deleted Successfully "

                '// Logfile created 

                Dim message As String
                Dim userName As String


                If Not Session("User") Is Nothing Then
                    userName = Session("User")
                Else
                    userName = String.Empty
                End If

                eob.UpdatedTime = DateTime.Now
                message = "** Date :" + DateTime.Now.ToString() + " ||" + " Receipt Number:" + eob.BatchCode.ToString() + " ||" + " Deleted by:" + userName
                Dim filePath As String
                filePath = HttpContext.Current.Server.MapPath("~") + "LogErrors" + "\ReceiptLog.txt"
                ' Writest to the file 
                LogEntry(message, filePath)

                eob.DeletedBy = userName
                eob.UpdatedBy = String.Empty

                'Inserts to the database
                bsobj.InsertReceiptUserAction(eob)

                '//

                lblMsg.Visible = True
                LoadListObjects(False)
            Catch ex As Exception
                lblMsg.Text = ex.Message.ToString()
                LogError.Log("Receipts", "ondelete", ex.Message)
            End Try
        End If
    End Sub

#End Region

#Region "OnSearchOthers "

    ''' <summary>
    ''' Method to Search for Posted Records
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub OnSearchOthers()
        Session("loaddata") = "View"
        If lblCount.Text <> "" Then
            If CInt(lblCount.Text) > 0 Then
                OnClearData()
                If ibtnNew.Enabled = False Then
                    ibtnSave.Enabled = False
                    ibtnSave.ImageUrl = "images/gsave.png"
                    ibtnSave.ToolTip = "Access Denied"
                End If
            Else
                Session("PageMode") = "Edit"
                addBankCode()
                LoadListObjects(False)

            End If
        Else
            Session("PageMode") = "Edit"
            addBankCode()
            LoadListObjects(False)

            PostEnFalse()
        End If
        If lblCount.Text.Length = 0 Then
            Session("PageMode") = "Add"
        End If

        If CInt(Request.QueryString("IsView")).Equals(1) Then

            ddlReceiptFor.Enabled = False
            ddlPaymentMode.Enabled = False
            ddlBankCode.Enabled = False
            txtDescription.Enabled = False
            txtBatchId.Enabled = False
            txtCtrlAmt.Enabled = False

            If ddlReceiptFor.SelectedValue = "Sp" Then
                pnlReceiptsp.Visible = True

                ddlSponsorInv.Enabled = False
                txtSpnAmount.Enabled = False
            ElseIf ddlReceiptFor.SelectedValue = "Sl" Then
                pnlStudentLoan.Visible = True

                txtLoanAmount.Enabled = False
            Else
                bankPanel.Visible = True

                Dim txtAmount As TextBox, txtBsn As TextBox, txtMatricNo As TextBox, txtTransDt As TextBox
                For Each dgItem As DataGridItem In dgStudentView.Items

                    txtMatricNo = dgItem.Cells(dgStudentViewCell.MatricNo).Controls(1)
                    txtMatricNo.Enabled = False

                    txtAmount = dgItem.Cells(9).Controls(1)
                    txtAmount.Enabled = False

                    txtBsn = dgItem.Cells(dgStudentViewCell.BankSlipNo).Controls(1)
                    txtBsn.Enabled = False

                    txtTransDt = dgItem.Cells(dgStudentViewCell.TransactionDate).Controls(1)
                    txtTransDt.Enabled = False
                Next
            End If

        End If



    End Sub

#End Region

#Region "PostEnFalse "

    ''' <summary>
    ''' Method to Disable Options After Posting
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub PostEnFalse()
        ibtnNew.Enabled = False
        ibtnNew.ImageUrl = "images/gadd.png"
        ibtnNew.ToolTip = "Access Denied"
        ibtnSave.Enabled = False
        ibtnSave.ImageUrl = "images/gsave.png"
        ibtnSave.ToolTip = "Access Denied"
        ibtnDelete.Enabled = False
        ibtnDelete.ImageUrl = "images/gdelete.png"
        ibtnDelete.ToolTip = "Access Denied"
        'ibtnView.Enabled = True
        'ibtnView.ImageUrl = "images/ready.png"
        'ibtnView.ToolTip = "Access Denied"
        ibtnPrint.Enabled = True
        ibtnPrint.ImageUrl = "images/print.png"
        ibtnPrint.ToolTip = "Print"
        ibtnPosting.Enabled = False
        ibtnPosting.ImageUrl = "images/gposting.png"
        ibtnPosting.ToolTip = "Access denied"
        ' ibtnOthers.Enabled = False
        'ibtnOthers.ImageUrl = "images/post.png"
        'ibtnOthers.ToolTip = "Access denied"

        'updated by Hafiz Roslan @ 27/01/2016
        'disable Add & Delete button at the DataGrid - Start
        For Each _dgItems In dgStudentView.Items
            PostedDisableButton(_dgItems)
        Next
        'disable Add & Delete button at the DataGrid - End

    End Sub

#End Region

#Region "uploadData "

    ''' <summary>
    ''' Method to Upload Files
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub uploadData()
        Dim lsDelimeter As String = ","
        lblMsg.Text = ""
        Dim path As String = Session("file1")
        Dim loReader As New StreamReader(path)

        Dim eTstudent As StudentEn
        Dim listStudent As New List(Of StudentEn)
        Dim listUnStudent As New List(Of StudentEn)
        Dim list As New List(Of StudentEn)
        Dim alllist As New List(Of StudentEn)
        Dim i As Integer
        Dim objStu As New StudentBAL

        While loReader.Read() > 0

            Dim lsRow As String = loReader.ReadLine()
            Dim lsArr As String() = lsRow.Split(lsDelimeter.ToCharArray())

            eTstudent = New StudentEn
            Try
                eTstudent.MatricNo = lsArr(0)
                eTstudent.ICNo = lsArr(1).Trim()
                eTstudent.StudentName = lsArr(2).Trim()
                eTstudent.TransactionAmount = CDbl(lsArr(3).Trim())
            Catch ex As Exception
                lblMsg.Text = "File Cannot be Read"
                Exit Sub
            End Try

            eTstudent.ProgramID = ""
            eTstudent.Faculty = ""
            eTstudent.CurrentSemester = 0
            eTstudent.SASI_StatusRec = True
            eTstudent.STsponsercode = New StudentSponEn()
            eTstudent.STsponsercode.Sponsor = ""
            'Check Student
            Try
                list = objStu.CheckStudentList(eTstudent)
            Catch ex As Exception
                LogError.Log("Receipts", "UploadData", ex.Message)
                Exit Sub
            End Try

            If list.Count = 0 Then
                eTstudent = New StudentEn
                Try
                    eTstudent.StudentName = lsArr(2).Trim()
                    eTstudent.MatricNo = lsArr(0)
                    eTstudent.ICNo = lsArr(1).Trim()
                    eTstudent.StuIndex = i
                    eTstudent.TransactionAmount = CDbl(lsArr(3).Trim())
                Catch ex As Exception
                    lblMsg.Text = "File Cannot be Read"
                    Exit Sub
                End Try

                listUnStudent.Add(eTstudent)
                'eTstudent = Nothing
            Else

                eTstudent.StudentName = list(0).StudentName
                eTstudent.MatricNo = list(0).MatricNo
                eTstudent.ICNo = list(0).ICNo
                eTstudent.ProgramID = list(0).ProgramID
                eTstudent.Faculty = list(0).Faculty
                eTstudent.CurrentSemester = list(0).CurrentSemester
                eTstudent.StuIndex = i
                eTstudent.TransactionAmount = CDbl(lsArr(3).Trim())
                listStudent.Add(eTstudent)
                eTstudent = Nothing
            End If
            i = i + 1

        End While
        loReader.Close()
        'loWriter.Close()

        dgUnStudent.DataSource = listUnStudent
        dgUnStudent.DataBind()
        Dim dgItem2 As DataGridItem
        Dim amt As Double
        Dim txtAmount As TextBox
        For Each dgItem2 In dgUnStudent.Items
            txtAmount = dgItem2.Cells(9).Controls(1)
            amt = dgItem2.Cells(10).Text
            txtAmount.Text = String.Format("{0:F}", amt)

        Next
        Dim totalAmt As Double = 0
        Dim totalPCAmt As Double = 0
        Dim stuen As New StudentEn
        Dim bsstu As New AccountsBAL
        Dim outamt As Double = 0.0
        Dim eobj As New StudentEn
        Dim k As Integer
        If Not Session("listview") Is Nothing Then
            alllist = Session("listview")
        Else
            alllist = New List(Of StudentEn)
        End If
        If listStudent.Count <> 0 Then
            While k < listStudent.Count
                eobj = listStudent(k)
                Dim j As Integer = 0
                Dim Flag As Boolean = False
                While j < alllist.Count
                    If alllist(j).MatricNo = eobj.MatricNo Then
                        Flag = True
                        Exit While
                    End If
                    j = j + 1
                End While
                If Flag = False Then
                    alllist.Add(eobj)
                End If
                k = k + 1
            End While
        End If
        If alllist Is Nothing Then
            dgStudentView.DataSource = Nothing
            dgStudentView.DataBind()
        Else
            Dim dgItem1 As DataGridItem
            Dim OutAmount As TextBox
            Dim j As Integer = 0
            Dim stramt As String
            Dim selchk As CheckBox
            Dim manualchk As CheckBox
            Dim link As New LinkButton
            dgStudentView.DataSource = alllist
            Session("listview") = alllist
            dgStudentView.DataBind()
            'Loading grid with Outstanding Amounts
            While j < alllist.Count
                For Each dgItem1 In dgStudentView.Items
                    If dgItem1.Cells(2).Text = alllist(j).MatricNo Then
                        selchk = dgItem1.Cells(0).Controls(1)
                        manualchk = dgItem1.Cells(8).Controls(1)
                        selchk.Checked = True
                        link = dgItem1.Cells(1).Controls(0)
                        OutAmount = dgItem1.Cells(9).Controls(1)
                        stramt = dgItem1.Cells(10).Text
                        If stramt = "" Or stramt = "0" Then stramt = 0.0
                        If Not alllist(j).AccountDetailsList Is Nothing Then
                            If alllist(j).AccountDetailsList.Count > 0 Then
                                dgItem1.Cells(12).Text = alllist(j).AmountPaid
                                manualchk.Checked = True
                                dgItem1.Cells(13).Text = False
                            End If
                        Else
                            manualchk.Checked = False
                            dgItem1.Cells(13).Text = True
                        End If
                        OutAmount.Text = String.Format("{0:F}", stramt)
                        dgItem1.Cells(11).Text = j
                        Exit For
                    End If
                Next
                j = j + 1
            End While
        End If
        Session("file1") = Nothing
    End Sub

#End Region

#Region "grid_load "

    ''' <summary>
    ''' Method to Load Student Grid
    ''' </summary>
    ''' <remarks></remarks>
    ''' Updated by Hafiz Roslan @ 2/2/2016
    Private Sub grid_load()
        Dim dgItem1 As DataGridItem

        Dim totalAmt1 As Double = 0
        Dim amt1 As Double
        Dim txtAmount1 As TextBox

        For Each dgItem1 In dgStudentView.Items

            txtAmount1 = dgItem1.Cells(9).Controls(1)
            amt1 = dgItem1.Cells(10).Text

            txtAmount1.Text = String.Format("{0:F}", amt1)
            totalAmt1 = totalAmt1 + txtAmount1.Text

            txtAllocateAmount.Text = String.Format("{0:F}", totalAmt1)

            'added by Hafiz Roslan @ 20/01/2016
            'start - enable/disable add&del button
            EnableDisableDeleteButton(dgItem1)
            'end - enable/disable add&del button
        Next

    End Sub

#End Region

#Region "Allocation "

    ''' <summary>
    ''' Method to load the Fields for Allocation Tab
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub Allocation()
        MultiView1.SetActiveView(View2)

        btnReceipt.CssClass = "TabButton"
        btnSelection.CssClass = "TabButtonClick"
        btnInactive.CssClass = "TabButton"
        bankPanel.Visible = False
        pnlStudentGrid.Visible = True
    End Sub

#End Region

#Region "nReceipt "

    ''' <summary>
    ''' Method to load the Fields for Receipt Tab
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub nReceipt()

        MultiView1.SetActiveView(View1)
        btnReceipt.CssClass = "TabButtonClick"
        btnSelection.CssClass = "TabButton"
        btnInactive.CssClass = "TabButton"
        bankPanel.Visible = True
        pnlStudentGrid.Visible = False
        check_Receiptfor()
    End Sub

#End Region

#Region "chkManual_CheckedChanged "

    Protected Sub chkManual_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)

        'Create Instances - Start
        Dim _Receipts As New ReceiptsClass
        Dim _CheckBox As CheckBox = Nothing
        Dim GridRowSelect As CheckBox = Nothing
        Dim _DataGridItem As DataGridItem = Nothing
        Dim AllocateAmountText As TextBox = Nothing
        'Create Instances - Stop

        'Variable Declarations - Start
        Dim GridItemIndex As Integer = 0
        Dim StudentStatus As String = Nothing, TotalAllocatedAmount As String = Nothing
        'Variable Declarations - Stop

        Try

            'Get Grid Check Box
            _CheckBox = CType(sender, CheckBox)

            'Get grid Item
            _DataGridItem = CType(_CheckBox.NamingContainer, DataGridItem)

            'Get grid Item Index
            GridItemIndex = _DataGridItem.ItemIndex

            'Get Grid Row Select Check Box
            GridRowSelect = dgStudentView.Items(GridItemIndex).Cells(0).Controls(1)

            'Get Allocate Amount Text from Grid
            AllocateAmountText = dgStudentView.Items(GridItemIndex).Cells(9).Controls(1)

            'Set Allocate Amount
            txtStuIndex.Text = AllocateAmountText.Text

            'if Manual check Box Checked - Start
            If _CheckBox.Checked = True Then

                If (GridRowSelect.Checked = False) Then
                    Call SetMessage("Please Select Student To Allocate")
                    Exit Sub
                End If

                If (txtStuIndex.Text = "0.00") Then
                    _CheckBox.Checked = False
                    Call SetMessage("Enter Some Amount To Allocate")
                    Exit Sub
                End If

                'Get Student Status
                StudentStatus = lblStatus.Value

                'Populate Data Grid - Start
                If Not _Receipts.LoadStudentInvoicesToGrid(_DataGridItem, StudentStatus,
                    dgStudentView, dgInvoices, TotalAllocatedAmount) Then

                    dgInvoices.Visible = False
                    Call SetMessage("Student Invoices Loading Failed...")

                Else

                    Call Allocation()
                    dgInvoices.Visible = True
                    Session("ID") = GridItemIndex

                    'if not posted - Start
                    If Not StudentStatus = ReceiptsClass.StatusPosted Then
                        bTnUpdate.Enabled = True
                    End If
                    'if not posted - Stop

                End If
                'Populate Data Grid - Stop

            End If
            'if Manual check Box Checked - Stop

        Catch ex As Exception

            'log error
            Call MaxModule.Helper.LogError(ex.Message)

            'Show Error Message
            Call SetMessage(ex.Message)

        End Try

    End Sub

#End Region

#Region "spnamount "

    ''' <summary>
    ''' Method to Format Sponsor Amount Field
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub spnamount()
        If txtSpnAmount.Text = "" Then

            txtSpnAmount.Text = 0
            txtSpnAmount.Text = String.Format("{0:F}", CDbl(txtSpnAmount.Text))
        Else
            txtSpnAmount.Text = String.Format("{0:F}", CDbl(txtSpnAmount.Text))
        End If
        txtAllocateAmount.Text = String.Format("{0:F}", CDbl(txtSpnAmount.Text))
    End Sub

#End Region

#Region "Format Control Amount Field "
    'added by Hafiz @ 04/6/2016
    'format Control Amount Field

    Private Sub CtrlAmt()

        If txtCtrlAmt.Text = "" Then
            txtCtrlAmt.Text = 0
            txtCtrlAmt.Text = String.Format("{0:F}", CDec(txtCtrlAmt.Text))

            Session("CtrlAmt") = Nothing
        Else
            If IsNumeric(txtCtrlAmt.Text) Then
                txtCtrlAmt.Text = String.Format("{0:F}", CDec(txtCtrlAmt.Text))
                txtCtrlAmt.BorderColor = System.Drawing.ColorTranslator.FromHtml("#23B2D0")

                Session("CtrlAmt") = CDec(txtCtrlAmt.Text)
            Else
                ClientScript.RegisterStartupScript(Me.GetType(), "Alert", "alert('Control Amount`s Invalid Format');", True)
                txtCtrlAmt.Focus()
                txtCtrlAmt.BorderColor = Drawing.Color.Red
            End If
        End If

    End Sub

#End Region

#Region "StudentLoanAmount "

    ''' <summary>
    ''' Method to Format Student Amount Field
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub StudentLoanAmount()
        If txtLoanAmount.Text = "" Then

            txtLoanAmount.Text = 0
            txtLoanAmount.Text = String.Format("{0:F}", CDbl(txtLoanAmount.Text))
        Else
            txtLoanAmount.Text = String.Format("{0:F}", CDbl(txtLoanAmount.Text))
        End If
        txtAllocateAmount.Text = String.Format("{0:F}", CDbl(txtLoanAmount.Text))
    End Sub

#End Region

#Region "LogEntry "

    Public Sub LogEntry(ByVal msg As String, ByVal path As String)
        Try

            If My.Computer.FileSystem.FileExists(path) Then
                'It will open the file, append the your message and close the file
                My.Computer.FileSystem.WriteAllText(path, Environment.NewLine, True)
                My.Computer.FileSystem.WriteAllText(path, msg, True)
            End If

        Catch ex As Exception
            ex.GetBaseException()
        End Try
    End Sub

#End Region

#Region "LoadStudentsTemplates "

    ''' <summary>
    ''' Method to Load Students Template
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub LoadStudentsTemplates(ByVal studentList As List(Of StudentEn))
        dgInvoices.DataSource = Nothing
        dgInvoices.DataBind()

        Dim list As New StudentEn
        Dim listStud As New List(Of StudentEn)
        Dim eobj As New StudentEn
        Dim i As Integer = 0

        Dim dgItem1 As DataGridItem
        Dim txtAmount As TextBox
        Dim txtPocket As TextBox
        Dim amt As Double = 0.0
        Dim pocAmt As Double = 0.0
        Dim j As Integer = 0
        Dim stuen As New StudentEn
        Dim bsstu As New AccountsBAL
        Dim objStu As New StudentBAL
        Dim outamt As Double = 0.0

        For Each stuItem As StudentEn In studentList
            eobj = New StudentEn

            eobj.MatricNo = stuItem.MatricNo
            eobj.ICNo = stuItem.NoIC
            eobj.SASI_StatusRec = True
            Try
                list = objStu.GetItem(eobj.MatricNo)
                listStud.Add(list)
            Catch ex As Exception
                LogError.Log("SponsorAllocation", "UploadData", ex.Message)
                lblMsg.Text = ex.Message
                Exit Sub
            End Try
            If list.MatricNo = "" Then
                lblMsg.Text = "Invalid Matric No exists in uploaded file."
                lblMsg.Visible = True
                Session("fileSponsor") = Nothing
                Exit Sub
            End If
        Next

        dgInvoices.DataSource = listStud
        dgInvoices.DataBind()

        For Each dgItem1 In dgInvoices.Items
            Dim cell11 As Double
            Dim cell12 As Double
            If dgItem1.Cells(11).Text = "" Or dgItem1.Cells(11).Text = "&nbsp;" Then
                cell11 = 0.0
            Else
                cell11 = dgItem1.Cells(11).Text
            End If
            If dgItem1.Cells(12).Text = "" Or dgItem1.Cells(12).Text = "&nbsp;" Then
                cell12 = 0.0
            Else
                cell12 = dgItem1.Cells(12).Text
            End If
            txtAmount = dgItem1.Cells(7).Controls(1)
            amt = CDbl(cell11)
            pocAmt = CDbl(cell12)
            txtAmount.Text = String.Format("{0:F}", amt)
            stuen.MatricNo = dgItem1.Cells(1).Text
            outamt = bsstu.GetStudentOutstandingAmt(stuen)
            dgItem1.Cells(6).Text = String.Format("{0:F}", outamt)
        Next
        Session("spnObj") = Nothing
        Session("liststu") = Nothing
        Session("SPncode") = Nothing
        Session("paidInvoices") = Nothing
        btnSelection.CssClass = "TabButton"
        MultiView1.SetActiveView(View1)

    End Sub

#End Region

#Region "readTextFile "

    ''' <summary>
    ''' Method to read text file
    ''' </summary>
    ''' <remarks></remarks>
    Private Function readTextFile(ByVal filepath As String) As List(Of StudentEn)
        Dim lstStudents As New List(Of StudentEn)
        Dim fileEntries As New List(Of String)

        Try
            ' Read the file into a list...
            Dim reader As StreamReader = New StreamReader(filepath)
            fileEntries.Clear()

            Do Until reader.Peek = -1 'Until eof
                fileEntries.Add(reader.ReadLine)
            Loop

            reader.Close()

        Catch ex As Exception
            ' The file's empty.
            lblMsg.Visible = True
            lblMsg.Text = "The File`s is empty. Error message: " & ex.Message & ""
        End Try
        Dim listStudent As New List(Of StudentEn)
        Try
            For Each line As String In fileEntries
                Dim checkCol As String = line.Substring(0, 10)
                Dim _studentEN As New StudentEn
                Dim _studEnFromDB As New StudentEn
                Dim stud As New StudentBAL
                Dim _studAccFromDB As New AccountsEn
                Dim studAcc As New AccountsBAL
                _studentEN.MatricNo = line.Substring(0, 6)
                _studEnFromDB = stud.GetItem(_studentEN.MatricNo)
                _studentEN.StudentName = _studEnFromDB.StudentName
                _studentEN.Faculty = _studEnFromDB.Faculty
                _studentEN.ProgramID = _studEnFromDB.ProgramID
                _studentEN.MatricNo = _studEnFromDB.MatricNo
                _studentEN.ICNo = _studEnFromDB.ICNo
                _studentEN.CurretSemesterYear = _studEnFromDB.CurretSemesterYear
                _studAccFromDB = studAcc.GetItemTrans(_studentEN)
                _studentEN.CurrentSemester = _studEnFromDB.CurrentSemester
                _studentEN.TransDate = DateTime.ParseExact(line.Substring(14, 8), "ddMMyyyy", CultureInfo.InvariantCulture)
                _studentEN.TransactionAmount = Trim(line.Substring(41, 14))
                _studentEN.noAkaun = line.Substring(30, 9)
                lstStudents.Add(_studentEN)
                dgStudentView.SelectedIndex = -1
                Session("eobjstu") = _studentEN
                Session("liststu") = lstStudents
                _studentEN = Nothing
            Next
            readTextFile = lstStudents
            Session("LaporanHarian") = True
        Catch ex As Exception
            lblMsg.Visible = True
            lblMsg.Text = ex.Message
        End Try
        Return readTextFile
    End Function

#End Region

#Region "ibtnPrint_Click1 "

    Protected Sub ibtnPrint_Click1(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ibtnPrint.Click
        If (ddlReceiptFor.SelectedValue = "St") Then
            LoadReport(txtBatchId.Text)
        End If
    End Sub

#End Region

#Region "txtLoanAmount_TextChanged "

    Protected Sub txtLoanAmount_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtLoanAmount.TextChanged
        'Updated by Hafiz Roslan @ 4/2/2016
        'modified by Hafiz Roslan @ 06/4/2016

        If txtLoanAmount.Text = "" Then
            txtLoanAmount.Text = "0.00"
        End If

        Dim _fontJIAdieuxRegEx As String = "^[a-zA-Z]*$"

        Dim r As New Regex(_fontJIAdieuxRegEx)

        If r.IsMatch(txtLoanAmount.Text) Then
            lblMsg.Text = "Enter Valid Amount"
            txtLoanAmount.Text = "0.00"
            txtLoanAmount.Focus()
        End If

        Try

            If CDbl(txtLoanAmount.Text.Trim()) > CDbl(lblLoanAmountToPay.Text) Then
                lblMsg.Visible = True
                lblMsg.Text = "The amount should not exceed the loan amount"
                txtLoanAmount.Focus()
            End If

            StudentLoanAmount()

        Catch ex As Exception
            lblMsg.Text = "Enter Valid Amount"
            txtLoanAmount.Text = "0.00"
            txtLoanAmount.Focus()
        End Try

    End Sub

#End Region

#Region "btnHidden_Click "

    Protected Sub btnHidden_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnHidden.Click

    End Sub

#End Region

#Region "ddlPaymentMode_SelectedIndexChanged "

    Protected Sub ddlPaymentMode_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlPaymentMode.SelectedIndexChanged

        If ddlPaymentMode.SelectedValue = "EFT" Then
            Session("LaporanHarian") = True
        Else
            Session("LaporanHarian") = False
        End If

    End Sub

#End Region

#Region "Populate Sponsor Invoice "

    Private Sub PopulateSponsorInvoice(ByVal SpID As String, ByVal TransID As String)

        'Create Instances - Start
        Dim _SponsorDAL As New SponsorDAL
        Dim eobjf As New SponsorEn
        Dim SponsorInvoices As DataTable = Nothing
        'Create Instances - Stop

        Try
            If Not Session("eobjspn") Is Nothing Then
                eobjf = Session("eobjspn")
                SpID = eobjf.SponserCode

                'Get Sponor Inv Details
                SponsorInvoices = _SponsorDAL.GetSponsorPostedInvoice(SpID, TransID)
                ' If SponsorInvoices.DataSet.Tables(0).Rows.Count > 0 Then
                'Populate Drop Down List - Start
                Call FormHelp.PopulateDropDownList(SponsorInvoices,
                    ddlSponsorInv, "invoice_details", "invoice_id")
                'Populate Drop Down List - Stop sponserDts 
                pnlReceiptsp.Visible = True
                'Sponsor Details Load - Start

                spnFlag = True
                Call addSpnCode()

                'Sponsor Details Load - Ended
                'End If
            ElseIf SpID > 0 Then
                'Get Sponor Inv Details
                SponsorInvoices = _SponsorDAL.GetSponsorPostedInvoice(SpID, TransID)
                ' If SponsorInvoices.DataSet.Tables(0).Rows.Count > 0 Then
                'Populate Drop Down List - Start
                Call FormHelp.PopulateDropDownList(SponsorInvoices,
                    ddlSponsorInv, "invoice_details", "invoice_id")
                'Populate Drop Down List - Stop sponserDts 
                pnlReceiptsp.Visible = True
                'Sponsor Details Load - Start

                spnFlag = True

                ' Session("eobjspn")
                Call addSpnCode()

                'Sponsor Details Load - Ended
            End If

            'Added by Hafiz Roslan @ 13/01/2016
            'Clear student`s search textbox
            searchStud.Text = ""

        Catch ex As Exception

            MaxModule.Helper.LogError(ex.Message)

        End Try

    End Sub

#End Region

#Region "Search Student by Matric Number/Sponsor by Sponsor Code"
    'Author     : Hafiz Roslan
    'Date       : 13/01/2016
    'Purpose    : Add function for the button to search for the student using matric no
    'WILL NOT BE USE. USED THE LATEST ONE WHICH ADD STUD ON THE GRID. COMMENT ON 22/1/2016 BY HAFIZ
    Protected Sub btnSearchStud_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnSearchStud.Click

        check_Receiptfor()

        If Session("ReceiptFor") = "St" Or Session("ReceiptFor") = "Sl" Then
            'STUDENT && STUDENT LOAN

            'check matric no textbox
            If searchStud.Text = "" Then
                lblMsg.Text = "Please Enter Matric No to Proceed"
                searchStud.Focus()
            Else
                'Load student by matric no
                Dim eob As New StudentEn
                Dim bobj As New StudentBAL
                Dim list As New List(Of StudentEn)
                'Dim listStudent As New List(Of StudentEn)

                If Not list Is Nothing Then
                    eob.MatricNo = Trim(searchStud.Text)
                    eob.StudentName = ""
                    eob.Faculty = ""
                    eob.ProgramID = ""
                    eob.ID = ""
                    eob.SAKO_Code = ""
                    eob.SABK_Code = ""
                    eob.SART_Code = ""
                    eob.StCategoryAcess = New StudentCategoryAccessEn
                    eob.StCategoryAcess.MenuID = 0
                    Try
                        list = bobj.GetListStudent(eob)
                    Catch ex As Exception
                        LogError.Log("Receipts", "btnSearchStud_Click", ex.Message)
                    End Try

                    If Not IsNothing(list) AndAlso list.Count > 0 Then
                        'have record

                        'assign selected student into new obj
                        For Each stud As StudentEn In list
                            Dim newstuobj As New StudentEn

                            newstuobj.MatricNo = stud.MatricNo
                            newstuobj.StudentName = stud.StudentName
                            newstuobj.ICNo = stud.ICNo
                            newstuobj.Faculty = stud.Faculty
                            newstuobj.ProgramID = stud.ProgramID
                            newstuobj.CurrentSemester = stud.CurrentSemester
                            newstuobj.CurretSemesterYear = stud.CurretSemesterYear
                            newstuobj.TempAmount = 0

                            'listStudent.Add(newstuobj)
                            'Session("liststu") = listStudent
                            Session("eobjstu") = newstuobj


                            'populate data at the grid
                            newstuobj = Nothing
                            StuFlag = True
                            Call LoadStudentDetails()
                        Next

                    Else
                        'dont have record
                        lblMsg.Text = "No student found"
                        searchStud.Focus()
                    End If
                Else
                    Response.Write("No Students are Available")
                End If
            End If
            'END STUDENT

        ElseIf Session("ReceiptFor") = "Sp" Then
            'SPONSOR

            If searchStud.Text = "" Then
                lblMsg.Text = "Please enter something to proceed"
                searchStud.Focus()
            Else
                'Load Sponsor
                Dim eob As New SponsorEn
                Dim bobj As New SponsorBAL
                Dim list As New List(Of SponsorEn)
                Dim listSponsor As New List(Of SponsorEn)

                If Not list Is Nothing Then
                    eob.SponserCode = Trim(searchStud.Text)
                    eob.Name = ""
                    eob.Type = ""
                    eob.GLAccount = ""
                    eob.Status = True
                    Try
                        list = bobj.GetSponserList(eob)
                    Catch ex As Exception
                        LogError.Log("Receipts", "btnSearchStud_Click", ex.Message)
                    End Try

                    If Not IsNothing(list) AndAlso list.Count > 0 Then
                        'Assign data to webform
                        For Each spon As SponsorEn In list
                            Dim newsponobj As New SponsorEn

                            newsponobj.SponserCode = spon.SponserCode
                            newsponobj.Name = spon.Name
                            newsponobj.Type = spon.Type

                            listSponsor.Add(newsponobj) 'not used list, 1 data only
                            Session("eobjspn") = newsponobj

                            'Populate Sponsor Invoices
                            Call PopulateSponsorInvoice("", "")
                        Next
                    Else
                        'dont have record
                        lblMsg.Text = "No sponsor found"
                        searchStud.Focus()
                    End If
                Else
                    Response.Write("No Sponsor are Available")
                End If
            End If
            'END SPONSOR

        End If

    End Sub

#End Region


    Private Enum dgStudentViewCell As Integer
        CheckBox = 0
        MatricNo = 2
        StudentName = 3
        ICno = 4
        Faculty = 5
        ProgramID = 6
        CurrentSemester = 7
        Amount = 9
        TransactionAmount = 10
        StuIndex = 11
        AllocatedAmount = 12
        SubReferenceTwo = 14
        NoAccount = 15
        BankSlipNo = 16
        TransactionDate = 17
        OutstandingAmount = 18
    End Enum


#Region "BankSlipID Checking"
    'added by Hafiz @ 01/4/2016

    Protected Sub BankSlipID_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs)

        Dim listnew As New List(Of StudentEn)

        Dim bsn As TextBox = Nothing
        Dim tot As Integer = 0

        Try
            listnew.Clear()

            tot = dgStudentView.Items.Count - 1

            For i As Integer = 0 To dgStudentView.Items.Count - 1

                Dim obj As New StudentEn


                bsn = dgStudentView.Items(i).Cells(dgStudentViewCell.BankSlipNo).Controls(1)


                If Not bsn.Text = "" Then

                    obj.BankSlipID = bsn.Text

                    listnew.Add(obj)

                End If

            Next

            'find similiar bank slip no - start
            If tot > 1 Then

                For l As Integer = 0 To listnew.Count - 1
                    For j As Integer = 0 To listnew.Count - 1
                        If l <> j Then

                            If listnew(l).BankSlipID = listnew(j).BankSlipID Then

                                bsn = dgStudentView.Items(j).Cells(dgStudentViewCell.BankSlipNo).Controls(1)

                                dgStudentView.Items(j).Cells(dgStudentViewCell.BankSlipNo).Focus()
                                bsn.Text = ""

                                Throw New Exception("Bank Slip No Already Exist")

                            End If

                        End If
                    Next
                Next

            End If
            'find similiar bank slip no - end

        Catch ex As Exception
            Call SetMessage(ex.Message)
        End Try

    End Sub

#End Region

#Region "MatricNo_TextChanged"

    'added by Hafiz @ 05/4/2016

    Protected Sub MatricNo_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs)

        Dim listnew As New List(Of StudentEn)

        Dim matricNo As TextBox = Nothing
        Dim tot As Integer = 0

        Try
            'listnew.Clear()

            tot = dgStudentView.Items.Count - 1

            For i As Integer = 0 To dgStudentView.Items.Count - 1

                Dim obj As New StudentEn

                matricNo = dgStudentView.Items(i).Cells(dgStudentViewCell.MatricNo).Controls(1)

                If Not matricNo.Text = "" Then

                    obj.MatricNo = matricNo.Text

                    listnew.Add(obj)

                End If

            Next

            'find similiar matric no - start
            If tot > 0 Then

                For l As Integer = 0 To listnew.Count - 1
                    For j As Integer = 0 To listnew.Count - 1
                        If l <> j Then

                            If listnew(l).MatricNo = listnew(j).MatricNo Then

                                matricNo = dgStudentView.Items(j).Cells(dgStudentViewCell.MatricNo).Controls(1)

                                dgStudentView.Items(j).Cells(dgStudentViewCell.MatricNo).Focus()
                                matricNo.Text = ""

                                Throw New Exception("Matric No Already Exist")
                            End If

                        End If
                    Next
                Next

            End If
            'find similiar matric no - end

        Catch ex As Exception
            Call SetMessage(ex.Message)
        End Try

    End Sub

#End Region

#Region "GetApprovalDetails"

    Protected Function GetMenuId() As Integer

        Dim MenuId As Integer = New MenuDAL().GetMenuMasterList().Where(Function(x) x.PageName = "Receipts").Select(Function(y) y.MenuID).FirstOrDefault()
        Return MenuId

    End Function

#End Region

#Region "For popup AddApprover() close"

    Protected Sub btnHiddenApp_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnHiddenApp.Click

    End Sub

#End Region

#Region "CheckWorkflowStatus"

    Protected Sub CheckWorkflowStatus(ByVal obj As AccountsEn)

        If obj.PostStatus = "Ready" Then

            Dim _list As List(Of WorkflowEn) = New WorkflowDAL().GetList().Where(Function(x) x.BatchCode = obj.BatchCode).ToList()
            If _list.Count > 0 Then

                lblMsg.Visible = True

                If _list.Where(Function(x) x.WorkflowStatus = 1).Count > 0 Then
                    lblMsg.Text = "Record Pending For Approval."
                ElseIf _list.Where(Function(x) x.WorkflowStatus = 3).Count > 0 Then
                    lblMsg.Text = "Record Rejected By Approval. [Reason:" & _list.Where(Function(x) x.WorkflowStatus = 3).Select(Function(y) y.WorkflowRemarks).FirstOrDefault() & "] "
                Else
                    lblMsg.Text = ""
                End If

            End If

        End If

    End Sub

#End Region

End Class
