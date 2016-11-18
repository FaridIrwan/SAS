Imports HTS.SAS.Entities
Imports HTS.SAS.BusinessObjects
Imports HTS.SAS.DataAccessObjects
Imports System.Data
Imports System.Collections.Generic
Imports System.Linq

Partial Class SponsorPayments
    Inherits System.Web.UI.Page
#Region "Global Declarations "
    'Global Declaration - Starting
    Private _Helper As New Helper
    Dim ListObjects As List(Of AccountsEn)
    Dim CFlag As String
    Dim DFlag As String
    Dim AutoNo As Boolean
    Dim PAidAmount As Double
    Private ErrorDescription As String
    Dim _AccDal As New HTS.SAS.DataAccessObjects.AccountsDAL
    'Global Declaration - Ended
#End Region

#Region "Done By "

    Public Function DoneBy() As Integer

        Return MaxGeneric.clsGeneric.
            NullToInteger(Session(Helper.UserSession))

    End Function

#End Region

    ''Private LogErrors As LogError

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        lblMsg.Text = ""
        If Not IsPostBack() Then
            'Adding Validation for all buttons
            ibtnSave.Attributes.Add("onclick", "return Validate()")
            ibtnDelete.Attributes.Add("onclick", "return getconfirm()")
            ibtnPosting.Attributes.Add("onclick", "return getpostconfirm()")
            txtRecNo.Attributes.Add("OnKeyup", "return geterr()")
            txtBDate.Attributes.Add("OnKeyup", "return CheckBatchDate()")
            txtPaymentDate.Attributes.Add("OnKeyup", "return CheckTransDate()")
            txtchequeDate.Attributes.Add("OnKeyup", "return CheckChequeDate()")
            txtAmountPaid.Attributes.Add("onKeypress", "return checknValue()")
            ibtnPaymentDate.Attributes.Add("onClick", "return getpaymentDate()")
            ibtnChequeDate.Attributes.Add("onClick", "return getChequeDate()")
            Session("Menuid") = Request.QueryString("Menuid")
            MenuId.Value = GetMenuId()
            ibtnBDate.Attributes.Add("onClick", "return BDate()")
            'Loading User Rights
            Session("PageMode") = ""
            Session("AddBank") = Nothing
            Session("ReceiptFrom") = "SP"
            addBankCode()
            addPayMode()
            Session("PageMode") = "Add"
            LoadUserRights()
            DisableRecordNavigator()
            Session("spnobj") = Nothing
            'load PageName
            Menuname(CInt(Request.QueryString("Menuid")))
            dates()
            ibtnSpn1.Attributes.Add("onclick", "new_window=window.open('addspnRecpts.aspx?cat=SP','Hanodale','width=800,height=600,resizable=0');new_window.focus();")
            'ibtnPosting.Attributes.Add("onclick", "new_window=window.open('AddApprover.aspx?MenuId=" & GetMenuId() & "','Hanodale','width=500,height=400,resizable=0');new_window.focus();")
            lblMsg.Text = ""
        End If

        If Not Session("spnObj") Is Nothing Then
            addSpnCode()
        End If

        If Not Session("CheckApproverList") Is Nothing Then
            SendToApproval()
        End If

        If Session("PageMode") = "Add" Then
            If AutoNo = True Then
                txtReceipNo.ReadOnly = False
                txtReceipNo.Text = "Auto Number"
                txtReceipNo.ReadOnly = True

            End If
        End If
        If Not Request.QueryString("BatchCode") Is Nothing Then
            txtReceipNo.Text = Request.QueryString("BatchCode")
            DirectCast(Master.FindControl("Panel1"), System.Web.UI.WebControls.Panel).Visible = False
            DirectCast(Master.FindControl("td"), System.Web.UI.HtmlControls.HtmlTableCell).Visible = False
            Panel1.Visible = False
            OnSearchOthers()
        End If

    End Sub
    Protected Sub ibtnSave_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ibtnSave.Click
        If Trim(txtPayeeName.Text).Length = 0 Then
            lblMsg.Text = "Enter Valid Payee Name"
            lblMsg.Visible = True
            Exit Sub
        End If
        If Trim(txtDesc.Text).Length = 0 Then
            lblMsg.Text = "Enter Valid Description"
            lblMsg.Visible = True
            Exit Sub
        End If
        If lblStatus.Value = "Posted" Then
            lblMsg.Text = "Record Already Posted"
            lblMsg.Visible = True
            Exit Sub
        End If
        'Dim bal As Double = 0.0
        'bal = CDbl(txtspnAmount.Text) - CDbl(txtAllAmount.Text)
        'bal = String.Format("{0:F}", bal)
        'If bal >= CDbl(txtAmountPaid.Text) Then
        '    Dim amount As Double
        '    amount = txtAmountPaid.Text
        '    txtAmountPaid.Text = String.Format("{0:F}", amount)
        'Else
        '    txtAmountPaid.Text = ""
        '    'lblMsg.Text = "Amount to be Paid Exceed the Outstanding Amount"
        '    lblMsg.Text = "Amount to be Paid Exceed the Allocated Amount"
        '    lblMsg.Visible = True
        '    Exit Sub
        'End If
        Dim bal As Double = 0.0
        bal = CDbl(txtAllAmount.Text)
        bal = String.Format("{0:F}", bal)
        If bal >= CDbl(txtAmountPaid.Text) Then
            Dim amount As Double
            amount = txtAmountPaid.Text
            txtAmountPaid.Text = String.Format("{0:F}", amount)
        Else
            txtAmountPaid.Text = 0.0
            'lblMsg.Text = "Amount to be Paid Exceed the Outstanding Amount"
            lblMsg.Text = "Amount to be Paid Exceed the Available Amount"
            lblMsg.Visible = True
            Exit Sub
        End If
        SpaceValidation()
        onSave()
        SetDateFormat()

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
    Protected Sub ibtnView_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ibtnView.Click
        LoadUserRights()
        Session("loaddata") = "View"
        If lblCount.Text <> "" Then
            If CInt(lblCount.Text) > 0 Then
                onAdd()
            Else
                Session("PageMode") = "Edit"
                addBankCode()
                LoadListObjects()

            End If
        Else
            Session("PageMode") = "Edit"
            addBankCode()
            LoadListObjects()

        End If
        If lblCount.Text.Length = 0 Then
            Session("PageMode") = "Add"
        End If
    End Sub

    'Protected Sub ibtnPosting_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ibtnPosting.Click

    '    'Varaible Declaration
    '    Dim BatchCode As String = MaxGeneric.clsGeneric.NullToString(txtReceipNo.Text)

    '    'Calling PostToWorFlow
    '    ' _Helper.PostToWorkflow(BatchCode, DoneBy(), Request.Url.AbsoluteUri)
    '    'Calling PostToWorkFlow
    '    'If Not _Helper.PostToWorkflow(BatchCode, DoneBy(), Request.Url.AbsoluteUri) = True Then
    '    '    lblMsg.Visible = True
    '    '    lblMsg.Text = "Record Already Posted"
    '    'Else
    '    '    onPost()
    '    '    lblMsg.Visible = True
    '    '    lblMsg.Text = "Record Posted Successfully"
    '    'End If

    '    If _Helper.PostToWorkflow(BatchCode, Session("User"), Request.Url.AbsoluteUri) = True Then
    '        'lblMsg.Text = "Record Posted"
    '        onPost()
    '        lblMsg.Visible = True
    '        lblMsg.Text = "Record Posted Successfully for Approval"
    '    Else
    '        'lblMsg.Text = "Record Already Posted"
    '        'onPost()
    '        'setDateFormat()
    '        'lblMsg.Text = "Record Posted Successfully for Approval"
    '        lblMsg.Visible = True
    '        lblMsg.Text = "Record Already Posted"
    '    End If
    '    'If lblStatus.Value = "Ready" Then
    '    '    SpaceValidation()
    '    '    onPost()
    '    '    SetDateFormat()
    '    'ElseIf lblStatus.Value = "New" Then
    '    '    lblMsg.Text = "Record not Ready for Posting"
    '    '    lblMsg.Visible = True
    '    'ElseIf lblStatus.Value = "Posted" Then
    '    '    lblMsg.Text = "Record Already Posted"
    '    '    lblMsg.Visible = True
    '    'End If
    'End Sub

#Region "SendToApproval"

    Protected Sub SendToApproval()

        Try
            If Not Session("listWF") Is Nothing Then
                Dim list As List(Of WorkflowSetupEn) = Session("listWF")
                If list.Count > 0 Then

                    If _Helper.PostToWorkflow(MaxGeneric.clsGeneric.NullToString(txtReceipNo.Text), Session("User"), Request.Url.AbsoluteUri) = True Then

                        SetDateFormat()

                        If onPost() = True Then
                            If Session("listWF").count > 0 Then
                                WorkflowApproverList(Trim(txtReceipNo.Text), Session("listWF"))
                            End If

                            lblMsg.Visible = True
                            lblMsg.Text = "Record Posted Successfully for Approval"

                        End If
                    Else
                        lblMsg.Visible = True
                        lblMsg.Text = "Record Already Posted"
                    End If

                Else
                    Throw New Exception("Posting to workflow failed caused NO approver selected.")
                End If

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

    Protected Sub ibtnCancel_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ibtnCancel.Click
        LoadUserRights()
        onAdd()
    End Sub
    Protected Sub ibtnNew_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ibtnNew.Click
        onAdd()
    End Sub

    Protected Sub ibtnDelete_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ibtnDelete.Click
        lblMsg.Visible = True
        If Trim(txtReceipNo.Text).Length <> 0 Then
            If lblCount.Text = "" Then lblCount.Text = 0
            If lblCount.Text > 0 Then
                Dim eobj As New AccountsEn
                Dim bsobj As New AccountsBAL

                Dim RecAff As Integer
                lblMsg.Visible = True

                Try
                    eobj.BatchCode = txtReceipNo.Text
                    RecAff = bsobj.BatchDelete(eobj)
                    Session("loaddata") = "View"
                    lblMsg.Visible = True
                    ErrorDescription = "Record Deleted Successfully"
                    lblMsg.Text = ErrorDescription

                Catch ex As Exception
                    lblMsg.Text = ex.Message.ToString()
                    LogError.Log("SponsorPayments", "ibtnDelete_Click", ex.Message)
                End Try

                DFlag = "Delete"
                'OnClearData()
                txtAllocationCode.ReadOnly = True
                txtSpnName.ReadOnly = True
                txtspnAmount.ReadOnly = True
                txtAllAmount.ReadOnly = True
                'ddlBankCode.ReadOnly = True
                Session("ListObj") = Nothing
                DisableRecordNavigator()
                txtReceipNo.Text = ""

                txtAmountPaid.Text = ""

                ddlPayment.SelectedValue = "-1"
                txtPaymentDate.Text = ""
                txtDesc.Text = ""
                txtBDate.Text = ""
                txtPayeeName.Text = ""
                txtCheque.Text = ""
                txtAllocationCode.Text = ""
                txtchequeDate.Text = ""
                ddlBankCode.Text = "-1"
                txtSpnName.Text = ""
                txtspnAmount.Text = ""
                txtAllAmount.Text = ""
                txtCreditref.Text = ""
                LoadListObjects()
            Else
                ErrorDescription = "Record Not Selected"
                lblMsg.Text = ErrorDescription
            End If

        Else
            ErrorDescription = "Record Not Selected"
            lblMsg.Text = ErrorDescription
        End If
    End Sub
    Protected Sub ibtnOthers_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        LoadUserRights()
        OnSearchOthers()
    End Sub
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
    Protected Sub ddlPayment_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        If (ddlPayment.SelectedValue = "1") Then
            txtCheque.Text = ""
        Else
            txtCheque.Text = 0
        End If
    End Sub
#Region "Methods"
    ''' <summary>
    ''' Method To Change the Date Format(dd/MM/yyyy)
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub SetDateFormat()
        Dim myPaymentDate As Date = CDate(CStr(txtPaymentDate.Text))
        Dim myFormat As String = "dd/MM/yyyy"
        Dim myFormattedDate As String = Format(myPaymentDate, myFormat)
        txtPaymentDate.Text = myFormattedDate
        Dim mychequeDate As Date = CDate(CStr(txtchequeDate.Text))
        Dim myFormattedDate1 As String = Format(mychequeDate, myFormat)
        txtchequeDate.Text = myFormattedDate1
        Dim myBatchDate As Date = CDate(CStr(txtBDate.Text))
        Dim myFormattedDate2 As String = Format(myBatchDate, myFormat)
        txtBDate.Text = myFormattedDate2
    End Sub
    ''' <summary>
    ''' Method to Change the Date Format
    ''' </summary>
    ''' <remarks>Date in ddd/mm/yyyy Format</remarks>
    Private Sub dates()
        txtPaymentDate.Text = Format(Date.Now, "dd/MM/yyyy")
        txtchequeDate.Text = Format(Date.Now, "dd/MM/yyyy")
        txtBDate.Text = Format(Date.Now, "dd/MM/yyyy")
    End Sub
    ''' <summary>
    ''' Method to Get Sponsors from Popup
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub addSpnCode()
        txtAllocationCode.ReadOnly = True
        txtSpnName.ReadOnly = True
        txtspnAmount.ReadOnly = True
        txtAllAmount.ReadOnly = True

        Dim eobj As New SponsorEn
        Dim eobj1 As New List(Of SponsorEn)

        eobj = Session("spnObj")
        eobj1 = Nothing
        eobj.Address = ""
        eobj1 = _AccDal.GetReciptSpPockAll(eobj)

        Dim amount As Double
        Dim allocatedamount As Double
        txtAllocationCode.Text = eobj.TransactionCode
        txtSpnName.Text = eobj.Name

        txtCreditref.Text = eobj.CreditRef
        'amount = eobj.PaidAmount - (eobj1(0).TransactionAmount + eobj1(0).TempAmount)
        allocatedamount = eobj.AllocatedAmount
        amount = eobj.TempAmount
        txtspnAmount.Text = String.Format("{0:F}", eobj.PaidAmount)
        txtAllAmount.Text = String.Format("{0:F}", amount)
        'txtAllAmount.Text = String.Format("{0:F}", (eobj1(0).TransactionAmount + eobj1(0).TempAmount))
        txtPayeeName.Text = txtSpnName.Text
        txtAmountPaid.Text = 0.0
        txtAvailable.Text = String.Format("{0:F}", amount)
        Session("spt") = eobj.CreditRef
        Session("spnObj") = Nothing

    End Sub
    ''' <summary>
    ''' Method to Load BankCodes Dropdown
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
                LogError.Log("SponsorPayments", "addBankCode", ex.Message)
            End Try
        Else

            Try
                list = bsobj.GetBankProfileListAll(eobjF)
            Catch ex As Exception
                LogError.Log("SponsorPayments", "addBankCode", ex.Message)
            End Try
        End If
        Session("bankcode") = list
        ddlBankCode.DataSource = list

        ddlBankCode.DataBind()
    End Sub
    ''' <summary>
    ''' Method to Load PaymentModes Dropdown
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
            End Try
        End If
        Session("paymode") = list
        ddlPayment.Items.Clear()
        ddlPayment.Items.Add(New ListItem("---Select---", "-1"))
        ddlPayment.DataSource = list
        ddlPayment.DataTextField = "SAPM_Des"
        ddlPayment.DataValueField = "SAPM_Code"
        ddlPayment.DataBind()

    End Sub
    ''' <summary>
    ''' Method to Load UserRights
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub LoadUserRights()
        Dim obj As New UsersBAL
        Dim eobj As New UserRightsEn

        'eobj = obj.GetUserRights(5, 1)

        Try
            eobj = obj.GetUserRights(CInt(Request.QueryString("Menuid")), CInt(Session("UserGroup")))

        Catch ex As Exception
            LogError.Log("SponsorPayments", "LoadUserRights", ex.Message)
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
            'ibtnView.ToolTip = "Access Denied"
            ibtnView.Enabled = True
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
    ''' <summary>
    ''' Method to get the List Of Sponsor Payments
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub LoadListObjects()
        Dim obj As New AccountsBAL
        Dim eobj As New AccountsEn
        eobj.Category = "Payment"
        eobj.TransStatus = "Open"
        If Session("loaddata") = "View" Then
            eobj.BatchCode = txtReceipNo.Text
            eobj.PostStatus = "Ready"
            eobj.SubType = "Sponsor"
            If txtReceipNo.Text <> "Auto Number" Then
                eobj.BatchCode = txtReceipNo.Text
            Else
                eobj.BatchCode = ""
            End If

            Try
                ListObjects = obj.GetTransactions(eobj)
            Catch ex As Exception
                LogError.Log("SponsorPayments", "LoadListObjects", ex.Message)
            End Try
        ElseIf Session("loaddata") = "others" Then
            eobj.BatchCode = txtReceipNo.Text
            eobj.PostStatus = "Posted"
            eobj.SubType = "Sponsor"

            If CInt(Request.QueryString("IsView")).Equals(1) Then
                eobj.PostStatus = "Ready"
            End If

            If txtReceipNo.Text <> "Auto Number" Then
                eobj.BatchCode = txtReceipNo.Text
            Else
                eobj.BatchCode = ""
            End If

            Try
                ListObjects = obj.GetTransactions(eobj)
            Catch ex As Exception
                LogError.Log("SponsorPayments", "LoadListObjects", ex.Message)
            End Try
        End If
        Session("loaddata") = Nothing
        Session("ListObj") = ListObjects
        lblCount.Text = ListObjects.Count.ToString()
        If ListObjects.Count <> 0 Then
            DisableRecordNavigator()
            txtRecNo.Text = "1"

            OnMoveFirst()
            If Session("EditFlag") = True Then
                ibtnSave.Enabled = True
                ibtnSave.ImageUrl = "images/save.png"
            Else
                Session("PageMode") = ""
                ibtnSave.Enabled = False
                ibtnSave.ImageUrl = "images/gsave.png"
            End If
            If txtReceipNo.Text <> "Auto Number" Then
                eobj.BatchCode = txtReceipNo.Text
            Else
                eobj.BatchCode = ""
            End If

        Else
            txtRecNo.Text = ""
            lblCount.Text = ""
            'OnClearData()

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
                Dim obj As AccountsEn
                Dim eobj As New AccountsDetailsEn
                ListObjects = Session("ListObj")
                obj = ListObjects(RecNo)
                txtReceipNo.Text = obj.BatchCode
                txtCreditref.Text = obj.CreditRef
                txtAllocationCode.Text = obj.CreditRefOne
                txtSpnName.Text = obj.CrRefOne
                txtspnAmount.Text = obj.SubReferenceOne
                If obj.TransStatus = "Ready" Then
                    Dim objAccbs As New AccountsBAL
                    Dim tempobj As New AccountsEn
                    tempobj = obj
                    tempobj.TransactionCode = obj.CreditRefOne

                    Try
                        tempobj = objAccbs.GetItemByTransCode(tempobj)
                    Catch ex As Exception
                        LogError.Log("SponsorPayments", "FillData", ex.Message)
                    End Try
                    txtAllAmount.Text = tempobj.PaidAmount
                Else
                    txtAllAmount.Text = obj.SubReferenceTwo
                End If
                ddlBankCode.Items.Clear()
                ddlBankCode.Items.Add(New ListItem("---Select---", "-1"))
                ddlBankCode.DataSource = Session("bankcode")
                ddlBankCode.DataBind()
                ddlBankCode.SelectedValue = obj.BankCode
                ddlPayment.Items.Clear()
                ddlPayment.Items.Add(New ListItem("---Select---", "-1"))
                ddlPayment.DataSource = Session("paymode")
                ddlPayment.DataBind()
                ddlPayment.SelectedValue = obj.PaymentMode
                txtPaymentDate.Text = obj.TransDate
                txtchequeDate.Text = obj.ChequeDate
                txtDesc.Text = obj.Description
                txtAmountPaid.Text = obj.TransactionAmount
                txtBDate.Text = obj.BatchDate
                txtPayeeName.Text = obj.PayeeName
                txtCheque.Text = obj.ChequeNo

                txtspnAmount.Text = String.Format("{0:F}", obj.SubReferenceOne)
                txtAllAmount.Text = String.Format("{0:F}", obj.SubReferenceTwo)
                txtAmountPaid.Text = String.Format("{0:F}", obj.TransactionAmount)


                'Changing Status
                If obj.PostStatus = "Ready" Then
                    lblStatus.Value = "Ready"
                    ibtnStatus.ImageUrl = "images/Ready.gif"
                End If
                If obj.PostStatus = "Posted" Then
                    lblStatus.Value = "Posted"
                    ibtnStatus.ImageUrl = "images/Posted.gif"
                End If

                CheckWorkflowStatus(obj)

            End If

        End If
        SetDateFormat()
    End Sub
    ''' <summary>
    ''' Method to Load Fields in New Mode
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub onAdd()
        Session("ListObj") = Nothing
        today.Value = Now.Date
        today.Value = Format(CDate(today.Value), "dd/MM/yyyy")
        Session("PageMode") = "Add"
        addBankCode()
        OnClearData()
        If ibtnNew.Enabled = False Then
            ibtnSave.Enabled = False
            ibtnSave.ImageUrl = "images/gsave.png"
            ibtnSave.ToolTip = "Access Denied"
        End If
        lblStatus.Value = "New"
        ibtnStatus.ImageUrl = "images/notready.gif"
        OnLoadItem()
    End Sub
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
        'ibtnView.Enabled = False
        'ibtnView.ImageUrl = "images/ready.png"
        'ibtnView.ToolTip = "Access Denied"
        ibtnPrint.Enabled = False
        ibtnPrint.ImageUrl = "images/gprint.png"
        ibtnPrint.ToolTip = "Access denied"
        ibtnPosting.Enabled = False
        ibtnPosting.ImageUrl = "images/gposting.png"
        ibtnPosting.ToolTip = "Access denied"
        'ibtnOthers.Enabled = False
        'ibtnOthers.ImageUrl = "images/post.png"
        'ibtnOthers.ToolTip = "Access denied"
    End Sub
    ''' <summary>
    ''' Method to Load DateFields
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub OnLoadItem()
        If Session("PageMode") = "Add" Then
            txtReceipNo.Text = "Auto Number"
            txtReceipNo.ReadOnly = True
            AutoNo = False
            today.Value = Now.Date
            today.Value = Format(CDate(today.Value), "dd/MM/yyyy")
            txtBDate.Text = Format(Date.Now, "dd/MM/yyyy")
            txtPaymentDate.Text = Format(Date.Now, "dd/MM/yyyy")
            txtchequeDate.Text = Format(Date.Now, "dd/MM/yyyy")
        End If
    End Sub
    ''' <summary>
    ''' Method to Save and Update Sponsor Payments
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub onSave()
        Dim eobj As New AccountsEn
        Dim eobjDetails As New AccountsDetailsEn
        Dim list As New List(Of AccountsDetailsEn)
        Dim bsobj As New AccountsBAL
        Dim eobjs As New SponsorEn
        Dim eobjlist As New List(Of SponsorEn)
        Dim ActSpAmount As Double = 0.0, SpStuAllAmount As Double = 0.0, RspStuAllAmount As Double = 0.0, StAllAmount = 0.0, AvalAllAmount As Double = 0.0
        eobj.CreditRefOne = Trim(txtAllocationCode.Text)
        eobj.CrRefOne = Trim(txtSpnName.Text)
        eobj.BatchCode = Trim(txtReceipNo.Text)
        eobj.SubReferenceOne = Trim(txtspnAmount.Text)
        eobj.SubReferenceTwo = Trim(txtAllAmount.Text)
        eobj.BankCode = ddlBankCode.SelectedValue
        eobj.PaymentMode = ddlPayment.SelectedValue
        eobj.TransDate = Trim(txtPaymentDate.Text)
        eobj.ChequeDate = Trim(txtchequeDate.Text)
        eobj.Description = Trim(txtDesc.Text)
        'eobj.TempAmount = (txtAmountPaid.Text)
        eobj.TempAmount = CDbl(txtAmountPaid.Text)
        'eobj.TransactionAmount = (txtAmountPaid.Text)
        eobj.TransactionAmount = CDbl(txtAmountPaid.Text)
        eobj.BatchDate = Trim(txtBDate.Text)
        eobj.PayeeName = Trim(txtPayeeName.Text)
        eobj.ChequeNo = Trim(txtCheque.Text)
        eobj.CreditRef = Trim(txtCreditref.Text)
        'eobj.PaidAmount = (txtAmountPaid.Text)
        eobj.PaidAmount = CDbl(txtAvailable.Text)
        eobj.TransType = "Debit"
        eobj.Category = "Payment"
        eobj.TransStatus = "Open"
        eobj.PostStatus = "Ready"
        eobj.SubType = "Sponsor"
        eobj.CreatedBy = Session("User")
        eobj.CreatedDateTime = Date.Now
        eobj.PostedDateTime = Date.Now
        eobj.UpdatedTime = Date.Now
        eobj.UpdatedBy = Session("User")
        eobj.AllocatedAmount = Trim(txtAmountPaid.Text)
        eobj.DueDate = Date.Now
        lblMsg.Visible = True
        'Sponser Amount check start

        'ActSpAmount = MaxGeneric.clsGeneric.NullToDecimal(txtspnAmount.Text)
        'RspStuAllAmount = MaxGeneric.clsGeneric.NullToDecimal(txtAllAmount.Text)
        'StAllAmount = MaxGeneric.clsGeneric.NullToDecimal(txtAmountPaid.Text)

        ''If (splist.Count > 0 And splist.Count < 2) Then
        'Try
        '    If ActSpAmount > RspStuAllAmount Then

        '        eobj.AllocatedAmount = RspStuAllAmount + StAllAmount

        '        If eobj.AllocatedAmount = eobj.SubReferenceOne Then
        '            eobj.PaidAmount = 0
        '        End If
        '    Else
        '        Throw New Exception("Allocated Amount Exceeds the Amount Received")
        '        ibtnSave.Enabled = False
        '        'End If
        '    End If

        'Catch ex As Exception
        '    lblMsg.Text = ex.Message.ToString()
        '    LogError.Log("SponsorPayments", "OnPost", ex.Message)
        '    ibtnSave.Enabled = False
        '    ibtnSave.ImageUrl = "images/gsave.png"
        '    ibtnSave.ToolTip = "Access Denied"
        'End Try
        If Session("PageMode") = "Add" Then
            Try
                'If (eobj.SubReferenceTwo > eobj.TransactionAmount) Then
                eobjs.SponserCode = Session("spt")
                eobjlist.Add(eobjs)
                eobj.SponsorList = eobjlist
                eobj.Sponsor = New SponsorEn
                'If (eobj.SubReferenceTwo > eobj.TransactionAmount) And (eobj.SubReferenceTwo > eobj.TempAmount) And (eobj.SubReferenceTwo > eobj.PaidAmount) Then
                txtReceipNo.Text = bsobj.SponsorBatchInsert(eobj, eobjlist)
                ErrorDescription = "Record Saved Successfully"
                lblMsg.Text = ErrorDescription
                ibtnStatus.ImageUrl = "images/ready.gif"
                lblStatus.Value = "Ready"
                'txtReceipNo.ReadOnly = False
                txtReceipNo.ReadOnly = True
                'Display error message saying that Duplicate Record
                'Else
                'ErrorDescription = "Amount to be Paid Exceed the Allocated Amount"
                'lblMsg.Text = ErrorDescription
                'End If
            Catch ex As Exception
                lblMsg.Text = ex.Message.ToString()
                LogError.Log("SponsorPayments", "OnSave", ex.Message)
            End Try
        ElseIf Session("PageMode") = "Edit" Then
            eobj.BatchCode = Trim(txtReceipNo.Text)
            Try
                'If (eobj.SubReferenceTwo > eobj.TransactionAmount) Then
                Dim obj As AccountsEn
                Dim RecNo As Integer
                Dim Seobj As New SponsorEn
                ListObjects = Session("ListObj")
                obj = ListObjects(RecNo)
                Seobj.SponserCode = txtCreditref.Text
                eobjlist.Add(Seobj)
                Seobj.SponsorList = eobjlist
                eobj.BatchCode = txtReceipNo.Text
                'RecAff = bsobj.Delete(eobj)
                txtReceipNo.Text = bsobj.SponsorBatchUpdate(eobj, eobjlist)
                ListObjects = Session("ListObj")
                ListObjects(CInt(txtRecNo.Text) - 1) = eobj
                Session("ListObj") = ListObjects
                ErrorDescription = "Record Updated Successfully"
                lblMsg.Text = ErrorDescription
                'Else
                'ErrorDescription = "Not success"
                'lblMsg.Text = ErrorDescription
                'End If
            Catch ex As Exception
                lblMsg.Text = ex.Message.ToString()
                LogError.Log("SponsorPayments", "OnSave", ex.Message)
            End Try
        End If
        'SetDateFormat()
        'Session("spnobj") = Nothing
    End Sub
    ''' <summary>
    ''' Method to Clear the Field Values
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub OnClearData()
        txtAllocationCode.ReadOnly = True
        txtSpnName.ReadOnly = True
        txtspnAmount.ReadOnly = True
        txtAllAmount.ReadOnly = True
        Session("ListObj") = Nothing
        DisableRecordNavigator()
        txtReceipNo.Text = ""
        txtAmountPaid.Text = ""
        ddlPayment.SelectedValue = "-1"
        txtPaymentDate.Text = ""
        txtDesc.Text = ""
        txtBDate.Text = ""
        txtPayeeName.Text = ""
        txtCheque.Text = ""
        txtAllocationCode.Text = ""
        txtchequeDate.Text = ""
        ddlBankCode.Text = "-1"
        txtSpnName.Text = ""
        txtspnAmount.Text = ""
        txtAllAmount.Text = ""
        lblMsg.Text = ""
        txtCreditref.Text = ""
        Session("PageMode") = "Add"
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
            LogError.Log("SponsorPayments", "Menuname", ex.Message)
        End Try
        lblMenuName.Text = eobj.MenuName
    End Sub
    ''' <summary>
    ''' Method to Search for Posted Records
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub OnSearchOthers()
        Session("loaddata") = "others"
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
                LoadListObjects()
            End If
        Else
            Session("PageMode") = "Edit"
            addBankCode()
            LoadListObjects()
            PostEnFalse()
        End If
        If lblCount.Text.Length = 0 Then
            Session("PageMode") = "Add"
        End If

        If CInt(Request.QueryString("IsView")).Equals(1) Then
            txtAllocationCode.Enabled = False
            ibtnSpn1.Attributes.Clear()
            txtReceipNo.Enabled = False
            txtAllAmount.Enabled = False
            txtspnAmount.Enabled = False
            txtBDate.Enabled = False
            ddlPayment.Enabled = False
            txtPaymentDate.Enabled = False
            txtAmountPaid.Enabled = False
            txtchequeDate.Enabled = False
        End If

    End Sub
    ''' <summary>
    ''' Method to Validate
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub SpaceValidation()
        Dim GBFormat As System.Globalization.CultureInfo
        GBFormat = New System.Globalization.CultureInfo("en-GB")
        'Description
        If Trim(txtDesc.Text).Length = 0 Then
            txtDesc.Text = Trim(txtDesc.Text)
            lblMsg.Text = "Enter Valid Description "
            lblMsg.Visible = True
            txtDesc.Focus()
            Exit Sub
        End If

        'Receipt No
        If Trim(txtAllocationCode.Text).Length = 0 Then
            txtAllocationCode.Text = Trim(txtAllocationCode.Text)
            lblMsg.Text = "Receipt No Field Cannot Be Blank "
            lblMsg.Visible = True
            txtAllocationCode.Focus()
            Exit Sub
        End If


        'Payment Amount
        If Trim(txtspnAmount.Text).Length = 0 Then
            txtspnAmount.Text = Trim(txtspnAmount.Text)
            lblMsg.Text = "Payment Amount Field Cannot Be Blank"
            lblMsg.Visible = True
            txtspnAmount.Focus()
            Exit Sub
        End If

        'Allocated Amount
        'If Trim(txtAllAmount.Text).Length = 0 Then
        '    txtAllAmount.Text = Trim(txtAllAmount.Text)
        '    lblMsg.Text = "Allocated Amount Field Cannot Be Blank"
        '    lblMsg.Visible = True
        '    txtAllAmount.Focus()
        '    Exit Sub
        'End If

        'cheque date
        If Trim(txtchequeDate.Text).Length < 10 Then
            lblMsg.Text = "Enter Valid Cheque Date"
            lblMsg.Visible = True
            txtchequeDate.Focus()
            Exit Sub
        Else
            Try
                txtchequeDate.Text = DateTime.Parse(txtchequeDate.Text, GBFormat).ToString
            Catch ex As Exception
                lblMsg.Text = "Enter Valid Cheque Date"
                lblMsg.Visible = True
                txtchequeDate.Focus()
                Exit Sub
            End Try
        End If
        'Batch date
        If Trim(txtBDate.Text).Length < 10 Then
            lblMsg.Text = "Enter Valid Batch Date"
            lblMsg.Visible = True
            txtBDate.Focus()
            Exit Sub
        Else
            Try
                txtBDate.Text = DateTime.Parse(txtBDate.Text, GBFormat).ToString
            Catch ex As Exception
                lblMsg.Text = "Enter Valid Batch Date"
                lblMsg.Visible = True
                txtBDate.Focus()
                Exit Sub
            End Try
        End If


        'Payment Mode
        If ddlPayment.SelectedValue = "-1" Then
            lblMsg.Text = "Select a Payment Mode"
            lblMsg.Visible = True
            ddlPayment.Focus()
            Exit Sub
        End If

        'Bank Code
        If ddlBankCode.SelectedValue = "-1" Then
            lblMsg.Text = "Select a Bank Code"
            lblMsg.Visible = True
            ddlBankCode.Focus()
            Exit Sub
        End If

        'Due date
        If Trim(txtPaymentDate.Text).Length < 10 Then
            lblMsg.Text = "Enter Valid Payment Date"
            lblMsg.Visible = True
            txtPaymentDate.Focus()
            Exit Sub
        Else
            Try
                txtPaymentDate.Text = DateTime.Parse(txtPaymentDate.Text, GBFormat).ToString
            Catch ex As Exception
                lblMsg.Text = "Enter Valid Payment Date"
                lblMsg.Visible = True
                txtPaymentDate.Focus()
                Exit Sub
            End Try
        End If

        'AmountPaid
        If Trim(txtAmountPaid.Text).Length = 0 Then
            txtAmountPaid.Text = Trim(txtAmountPaid.Text)
            lblMsg.Text = "Amount to be Paid Field Cannot Be Blank"
            lblMsg.Visible = True
            txtAmountPaid.Focus()
            Exit Sub
        End If

        'Payee Name
        If Trim(txtPayeeName.Text).Length = 0 Then
            txtPayeeName.Text = Trim(txtPayeeName.Text)
            lblMsg.Text = "Payee Name Field Cannot Be Blank"
            lblMsg.Visible = True
            txtPayeeName.Focus()
            Exit Sub
        End If

        'Description
        If Trim(txtDesc.Text).Length = 0 Then
            txtDesc.Text = Trim(txtDesc.Text)
            lblMsg.Text = "Enter Valid Description "
            lblMsg.Visible = True
            txtDesc.Focus()
            Exit Sub
        End If

        If lblStatus.Value = "Posted" Then
            lblMsg.Text = "Record Already Posted"
            lblMsg.Visible = True
            Exit Sub
        End If
        'onSave()

    End Sub
    ''' <summary>
    ''' Method to Post Sponsor Payments
    ''' </summary>
    ''' <remarks></remarks>
    Private Function onPost() As Boolean
        Dim result As Boolean = False
        Dim eobj As New AccountsEn
        Dim eobjDetails As New AccountsDetailsEn
        Dim list As New List(Of AccountsEn)
        Dim bsobj As New AccountsBAL
        Dim eobjs As New SponsorEn
        Dim eobjlist As New List(Of SponsorEn)
        Dim bid As String = ""
        lblMsg.Text = ""
        lblMsg.Visible = True
        ListObjects = Session("ListObj")
        eobj.CreditRefOne = Trim(txtAllocationCode.Text)
        eobj.CrRefOne = Trim(txtSpnName.Text)
        eobj.BatchCode = Trim(txtReceipNo.Text)
        eobj.SubReferenceOne = Trim(txtspnAmount.Text)
        eobj.SubReferenceTwo = txtAllAmount.Text - txtAmountPaid.Text
        eobj.BankCode = Trim(ddlBankCode.SelectedValue)
        eobj.PaymentMode = ddlPayment.SelectedValue
        eobj.TransDate = Trim(txtPaymentDate.Text)
        eobj.ChequeDate = Trim(txtchequeDate.Text)
        eobj.Description = Trim(txtDesc.Text)
        'eobj.TempAmount = (txtAmountPaid.Text)
        eobj.TempAmount = CDbl(txtAmountPaid.Text)
        'eobj.TransactionAmount = (txtAmountPaid.Text)
        eobj.TransactionAmount = CDbl(txtAmountPaid.Text)
        eobj.BatchDate = Trim(txtBDate.Text)
        eobj.PayeeName = Trim(txtPayeeName.Text)
        eobj.ChequeNo = Trim(txtCheque.Text)
        eobj.CreditRef = Trim(txtCreditref.Text)
        'eobj.PaidAmount = (txtAmountPaid.Text)
        eobj.PaidAmount = CDbl(txtAvailable.Text)
        eobj.TransType = "Debit"
        eobj.Category = "Payment"
        eobj.AllocatedAmount = Trim(txtAmountPaid.Text)
        'Modified Mona 19/2/2016
        'eobj.TransStatus = "Open"
        'eobj.PostStatus = "Posted"
        eobj.TransStatus = "Closed"
        eobj.PostStatus = "Ready"

        eobj.SubType = "Sponsor"
        eobj.CreatedDateTime = Date.Now
        eobj.PostedBy = Session("User")
        eobj.PostedDateTime = Date.Now
        eobj.UpdatedTime = Date.Now
        eobj.DueDate = Date.Now
        eobj.UpdatedBy = Session("User")
        lblMsg.Visible = True

        Try
            Dim Seobj As New SponsorEn
            Seobj.SponserCode = Trim(txtCreditref.Text)
            eobjlist.Add(Seobj)
            Seobj.SponsorList = eobjlist
            txtReceipNo.Text = bsobj.SponsorBatchUpdate(eobj, eobjlist)
            'ErrorDescription = "Record Posted Successfully"
            lblMsg.Text = ErrorDescription
            lblMsg.Visible = True
            result = True
            'Mona 19/2/2016
            'ibtnStatus.ImageUrl = "images/posted.gif"
            'lblStatus.Value = "Posted"
            txtReceipNo.ReadOnly = False
            txtReceipNo.Text = eobj.BatchCode
            txtReceipNo.ReadOnly = True
            'eobj.PostStatus = "Posted"

            'Remove item from List 
            If Not Session("ListObj") Is Nothing Then
                ListObjects = Session("ListObj")
                Session("ListObj") = ListObjects
                'If lblStatus.Value = "Posted" Then
                '    ibtnStatus.ImageUrl = "images/posted.gif"
                '    'lblStatus.Value = "Posted"
                'End If
            End If
            lblMsg.Text = ErrorDescription

        Catch ex As Exception
            lblMsg.Text = ex.Message.ToString()
            LogError.Log("SponsorPayments", "OnPost", ex.Message)
        End Try

        Session("recno") = Nothing
        Return result

    End Function

#End Region

    Protected Sub btnHidden_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnHidden.Click

    End Sub

#Region "GetApprovalDetails"

    Protected Function GetMenuId() As Integer

        Dim MenuId As Integer = New MenuDAL().GetMenuMasterList().Where(Function(x) x.PageName = "Sponsor Payments").Select(Function(y) y.MenuID).FirstOrDefault()
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
                    lblMsg.Text = "Record Rejected By Approval. [Reason:" & _list.Where(Function(x) x.WorkflowStatus = 3).Select(Function(y) y.WorkflowRemarks).FirstOrDefault() & "]"
                Else
                    lblMsg.Text = ""
                End If

            End If

        End If

    End Sub

#End Region

End Class
