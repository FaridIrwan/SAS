Imports System.Collections.Generic
Imports HTS.SAS.BusinessObjects
Imports HTS.SAS.DataAccessObjects
Imports HTS.SAS.Entities
Imports System.Data
Imports System.Linq

Partial Class SponsorCreditNote
    Inherits System.Web.UI.Page

#Region "Global Declaration "

    'declare instant
    Private _MaxModule As New MaxModule.CfCommon
    Private ListTRD As New List(Of AccountsDetailsEn)
    Public ListObjects As List(Of AccountsEn)
    Private ErrorDescription As String
    Dim AutoNo As Boolean
    Dim DFlag As String
    Dim GBFormat As System.Globalization.CultureInfo
    Private _Helper As New Helper
    ''Private LogErrors As LogError

#End Region

    Protected Sub p_load()
        MultiView1.SetActiveView(View1)
        imgLeft1.ImageUrl = "images/b_white_left.gif"
        imgRight1.ImageUrl = "images/b_white_right.gif"
        btnBatchInvoice.CssClass = "TabButtonClick"


        imgLeft2.ImageUrl = "images/b_orange_left.gif"
        imgRight2.ImageUrl = "images/b_orange_right.gif"
        btnSelection.CssClass = "TabButton"

        imgLeft3.ImageUrl = "images/b_orange_left.gif"
        imgRight3.ImageUrl = "images/b_orange_right.gif"
        btnViewStu.CssClass = "TabButton"

        'imgLeft4.ImageUrl = "images/b_orange_left.gif"
        'imgRight4.ImageUrl = "images/b_orange_right.gif"
        btnViewBalanceSponsor.CssClass = "TabButton"

        pnlBatch.Visible = True
        pnlSelection.Visible = False
        pnlViewType.Visible = False
        pnlViewSponsor.Visible = False
        trPrint.Visible = False
    End Sub

    Protected Sub ibtnView_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ibtnView.Click
        LoadUserRights()
        Session("loaddata") = "View"

        If lblCount.Text <> "" Then
            If CInt(lblCount.Text) > 0 Then
                onAdd()

            Else
                LoadListObjects()
                If lblCount.Text <> "" Then
                    Session("PageMode") = "Edit"
                Else
                    Session("PageMode") = "Add"
                End If
            End If
        Else
            LoadListObjects()
            If lblCount.Text <> "" Then
                Session("PageMode") = "Edit"
            Else
                Session("PageMode") = "Add"
            End If
        End If
    End Sub

    Protected Sub btnBatchInvoice_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnBatchInvoice.Click
        MultiView1.SetActiveView(View1)
        imgLeft1.ImageUrl = "images/b_white_left.gif"
        imgRight1.ImageUrl = "images/b_white_right.gif"
        btnBatchInvoice.CssClass = "TabButtonClick"


        imgLeft2.ImageUrl = "images/b_orange_left.gif"
        imgRight2.ImageUrl = "images/b_orange_right.gif"
        btnSelection.CssClass = "TabButton"

        imgLeft3.ImageUrl = "images/b_orange_left.gif"
        imgRight3.ImageUrl = "images/b_orange_right.gif"
        btnViewStu.CssClass = "TabButton"

        btnViewBalanceSponsor.CssClass = "TabButton"

        pnlBatch.Visible = True
        pnlViewType.Visible = False
        pnlSelection.Visible = False

        If Not Session("PageMode") = "Edit" Then

            pnlViewReceipt.Visible = False
            pnlViewSponsor.Visible = False
            pnlSponsorBalance.Visible = False
            ddlViewType.Enabled = True

            ddlViewType.SelectedIndex = -1
        End If

    End Sub

    Protected Sub btnSelection_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        MultiView1.SetActiveView(View2)
        imgLeft2.ImageUrl = "images/b_white_left.gif"
        imgRight2.ImageUrl = "images/b_white_right.gif"
        btnSelection.CssClass = "TabButtonClick"


        imgLeft1.ImageUrl = "images/b_orange_left.gif"
        imgRight1.ImageUrl = "images/b_orange_right.gif"
        btnBatchInvoice.CssClass = "TabButton"

        imgLeft3.ImageUrl = "images/b_orange_left.gif"
        imgRight3.ImageUrl = "images/b_orange_right.gif"
        btnViewStu.CssClass = "TabButton"

        btnViewBalanceSponsor.CssClass = "TabButton"

        pnlBatch.Visible = False
        pnlSelection.Visible = True
        pnlViewType.Visible = False
        pnlViewReceipt.Visible = False
        pnlViewSponsor.Visible = False
        pnlSponsorBalance.Visible = False
    End Sub

    Protected Sub btnViewStu_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnViewStu.Click
        OnViewSponsorGrid()

        If CInt(Request.QueryString("IsView")).Equals(1) Then
            ibtnPnlReceipt.Attributes.Clear()
        End If
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        lblMsg.Text = ""

        If Not IsPostBack() Then

            Menuname(CInt(Request.QueryString("Menuid")))
            p_load()
            'Getting MeniId
            Session("Menuid") = Request.QueryString("Menuid")
            MenuId.Value = GetMenuId()
            'Client side Validations
            txtBatchDate.Text = Format(Date.Today, "dd/MM/yyyy")
            txtInvoiceDate.Text = Format(Date.Today, "dd/MM/yyyy")
            ibtnDelete.Attributes.Add("onClick", "return getconfirm()")
            ibtnInDate.Attributes.Add("onClick", "return getDate1from()")
            ibtnPosting.Attributes.Add("onClick", "return getpostconfirm()")
            ibtnBDate.Attributes.Add("onClick", "return getibtnBDate()")
            ibtnSave.Attributes.Add("onClick", "return Validate()")
            btnViewStu.Attributes.Add("onClick", "return Validate()")
            btnSelection.Attributes.Add("onClick", "return Validate()")
            btnViewBalanceSponsor.Attributes.Add("onClick", "return Validate")
            txtRecNo.Attributes.Add("OnKeyup", "return geterr()")
            'txtTotal.Attributes.Add("onKeypress", "return checkValue()")
            txtInvoiceDate.Attributes.Add("OnKeyup", "return CheckInvDate()")
            txtBatchDate.Attributes.Add("OnKeyup", "return CheckBatchDate()")
            'added by Hafiz @ 06/4/2016
            ibtnPrint.Attributes.Add("onCLick", "return getPrint()")
            txtOutAmt.Visible = False
            lblOut.Visible = False
            lblCredit.Visible = False
            lblDebit.Visible = False
            lblWarn.Visible = False
            txtCreditAmt.Visible = False
            txtDebitAmt.Visible = False
            Session("ReceiptFrom") = Nothing
            Session("ReceiptFrom") = "SP"
            Session("ReceiptFor") = Nothing
            Session("ReceiptFor") = "Sp"
            Session("spnObj") = Nothing
            Session("eobjs") = Nothing
            Session("eobjspn") = Nothing
            Session("ViewType") = Nothing
            ibtnPnlReceipt.Attributes.Add("onclick", "new_window=window.open('addSpnRecpts.aspx?cat=SP','Hanodale','width=700,height=400,resizable=0');new_window.focus();")
            ibtnPnlSponsor.Attributes.Add("onclick", "new_window=window.open('AddMulStudents.aspx?cat=Sp','Hanodale','width=520,height=400,resizable=0');new_window.focus();")
            'ibtnPosting.Attributes.Add("onclick", "new_window=window.open('AddApprover.aspx?MenuId=" & GetMenuId() & "','Hanodale','width=500,height=400,resizable=0');new_window.focus();")
            OnLoadItem()
            'Loading UserRights
            LoadUserRights()
            DisableRecordNavigator()
            'Date Formatting
            dates()
            txtBatchNo.ReadOnly = False
            txtBatchNo.Text = "Auto Number"
            txtBatchNo.ReadOnly = True
            lblMsg.Text = ""
            imgGL.Visible = False
            Session("isView") = True
        End If

        If Not Session("spnObj") Is Nothing Then
            addReceipt()
        End If

        If Not Session("eobjspn") Is Nothing Then
            addSponsor()
        End If

        If Not Session("CheckApproverList") Is Nothing Then
            SendToApproval()
        End If

        'Display Rcord from Student Ledger screen
        If Session("isView") = True Then
            If Not Request.QueryString("BatchCode") Is Nothing Then
                txtBatchNo.Text = Request.QueryString("BatchCode")
                DirectCast(Master.FindControl("Panel1"), System.Web.UI.WebControls.Panel).Visible = False
                DirectCast(Master.FindControl("td"), System.Web.UI.HtmlControls.HtmlTableCell).Visible = False
                pnlToolbar.Visible = False

                OnSearchOthers()

            End If
        End If

    End Sub

    Protected Sub Page_LoadComplete(ByVal sender As Object, ByVal e As EventArgs) Handles Me.LoadComplete
        If ddlNoteType.SelectedIndex = 2 Then
            btnBatchInvoice.Text = "Credit Note"
            'lblNoteType.Text = "Credit Note"
        ElseIf ddlNoteType.SelectedIndex = 1 Then
            btnBatchInvoice.Text = "Debit Note"
            'lblNoteType.Text = "Debit Note"
        ElseIf ddlNoteType.SelectedIndex = 0 Then
            btnBatchInvoice.Text = "Select Type"
            'lblNoteType.Text = ""
        End If
        If txtOutAmt.Visible = True Then
            If txtOutAmt.Text > 0.01 And txtOutAmt.Text <> "" Then
                lblMsg.Visible = True
                lblMsg.Text = "Sponsor Out Of Balance"
                'lblWarn.Visible = True
            Else
                'lblWarn.Visible = False
            End If

        End If
        Session("isView") = False
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

    Protected Sub ibtnSave_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ibtnSave.Click

        GBFormat = New System.Globalization.CultureInfo("en-GB")
        If Trim(txtTotal.Text).Length = 0 Then
            lblMsg.Text = "Enter Valid Amount"
            lblMsg.Visible = True
            p_load()
            txtTotal.Focus()
            Exit Sub
        End If
        'Check BatchDate
        If Trim(txtBatchDate.Text).Length < 10 Then
            lblMsg.Text = "Enter Valid Batch Date"
            lblMsg.Visible = True
            p_load()
            txtBatchDate.Focus()
            Exit Sub
        Else
            Try
                txtBatchDate.Text = DateTime.Parse(txtBatchDate.Text, GBFormat)
            Catch ex As Exception
                lblMsg.Text = "Enter Valid Batch Date"
                lblMsg.Visible = True
                p_load()
                txtBatchDate.Focus()
                Exit Sub
            End Try
        End If
        'Check Invoice date
        If Trim(txtInvoiceDate.Text).Length < 10 Then
            lblMsg.Text = "Enter Valid Transaction Date"
            lblMsg.Visible = True
            p_load()
            txtInvoiceDate.Focus()
            Exit Sub
        Else
            Try
                txtInvoiceDate.Text = DateTime.Parse(txtInvoiceDate.Text, GBFormat)
            Catch ex As Exception
                lblMsg.Text = "Enter Valid Transaction Date"
                lblMsg.Visible = True
                p_load()
                txtInvoiceDate.Focus()
                Exit Sub
            End Try
        End If
        If lblStatus.Value = "Posted" Then
            lblMsg.Text = "Record Posted Successfully"
            lblMsg.Visible = True
            Exit Sub
        End If

        onSave()
        SetDateFormat()
    End Sub

    Protected Sub ibtnNew_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ibtnNew.Click
        Session("ViewType") = Nothing
        onAdd()
    End Sub

    Protected Sub ibtnDelete_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ibtnDelete.Click
        ondelete()
    End Sub

    Protected Sub ibtnCancel_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ibtnCancel.Click
        Session("ViewType") = Nothing
        LoadUserRights()
        onAdd()
    End Sub
    'Protected Sub btnSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSearch0.Click
    '    LoadSponsor()
    'End Sub
    Protected Sub dgView_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dgView.ItemDataBound
        Dim Chk As CheckBox
        If chkSelectSponsor.Checked = True Then
            Select Case e.Item.ItemType
                Case ListItemType.Item, ListItemType.AlternatingItem
                    Chk = CType(e.Item.FindControl("chk"), CheckBox)
                    Chk.Checked = True
            End Select
        End If
    End Sub

    Protected Sub chkSelectSponsor_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)

        Dim chkBox As CheckBox

        For Each dgi As DataGridItem In dgView.Items
            chkBox = dgi.Cells(0).Controls(1)
            If chkSelectSponsor.Checked = True Then
                chkBox.Checked = True
            Else
                chkBox.Checked = False
            End If

        Next
    End Sub

    Protected Sub btnUpdateCri_Click1(ByVal sender As Object, ByVal e As System.EventArgs)
        getsponsorlist()
    End Sub

    Protected Sub chkSponsor_CheckedChanged1(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim chkBox As CheckBox

        For Each dgi As DataGridItem In dgSponsor.Items
            chkBox = dgi.Cells(0).FindControl("chk")
            If chkSponsor.Checked = True Then
                chkBox.Checked = True
            Else
                chkBox.Checked = False
            End If
        Next
    End Sub

    'Protected Sub ibtnPosting_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ibtnPosting.Click
    '    'ValidateMultiView(MultiView1)

    '    If Trim(txtDesc.Text).Length = 0 Then
    '        lblMsg.Text = "Enter Valid Description "
    '        lblMsg.Visible = True
    '        Exit Sub
    '    End If
    '    If lblStatus.Value = "New" Then
    '        lblMsg.Text = "Record not Ready for Posting"
    '        lblMsg.Visible = True
    '    ElseIf lblStatus.Value = "Posted" Then
    '        lblMsg.Text = "Record Posted Successfully"
    '        lblMsg.Visible = True
    '    ElseIf lblStatus.Value = "Ready" Then
    '        Dim BatchCode As String = MaxGeneric.clsGeneric.NullToString(txtBatchNo.Text)
    '        SpaceValidation()
    '        'OnPost()
    '        If _Helper.PostToWorkflow(BatchCode, Session("User"), Request.Url.AbsoluteUri) = True Then
    '            OnPost()
    '            lblMsg.Visible = True
    '            lblMsg.Text = "Record Posted Successfully for Approval"
    '            SetDateFormat()
    '        End If
    '    End If
    'End Sub

#Region "SendToApproval"

    Protected Sub SendToApproval()

        Try
            If Trim(txtDesc.Text).Length = 0 Then
                lblMsg.Text = "Enter Valid Description "
                lblMsg.Visible = True
                Exit Sub
            End If

            If lblStatus.Value = "New" Then
                lblMsg.Text = "Record not Ready for Posting"
                lblMsg.Visible = True
            ElseIf lblStatus.Value = "Posted" Then
                lblMsg.Text = "Record Posted Successfully"
                lblMsg.Visible = True
            ElseIf lblStatus.Value = "Ready" Then

                If Not Session("listWF") Is Nothing Then
                    Dim list As List(Of WorkflowSetupEn) = Session("listWF")
                    If list.Count > 0 Then
                        lblMsg.Text = ""
                        lblMsg.Visible = True

                        If _Helper.PostToWorkflow(MaxGeneric.clsGeneric.NullToString(txtBatchNo.Text), Session("User"), Request.Url.AbsoluteUri) = True Then

                            SpaceValidation()
                            SetDateFormat()

                            If OnPost() = True Then
                                If Session("listWF").count > 0 Then
                                    WorkflowApproverList(Trim(txtBatchNo.Text), Session("listWF"))
                                End If

                                lblMsg.Text = "Record Posted Successfully for Approval"
                            Else
                                lblMsg.Text = "Record Failed to Posted"
                            End If

                        Else
                            Throw New Exception("Record Already Posted")
                        End If
                    Else
                        Throw New Exception("Posting to workflow failed caused NO approver selected.")
                    End If

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
            LogError.Log("SponsorCreditDebitNote", "InsertWFApprovalList", ex.Message)
        End Try

    End Sub

#End Region

    Protected Sub txtTotal_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        'modified by Hafiz @ 06/4/2016

        Dim amount As String = ""

        Try
            Dim str As String = txtTotal.Text

            If Not IsNumeric(str) Then
                txtTotal.Focus()
                txtTotal.Text = ""

                Page.ClientScript.RegisterStartupScript(Me.GetType(), "Registered Script",
                                                        "<script>alert('Enter Valid Amount')</script>", False)

            End If

            If Trim(txtTotal.Text).Length <> 0 Then
                amount = txtTotal.Text
                txtTotal.Text = String.Format("{0:F}", CDec(amount))
            End If

        Catch ex As Exception
            Call SetMessage(ex.Message)
        End Try

    End Sub

    Protected Sub txtRecNo_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtRecNo.TextChanged
        If Trim(txtRecNo.Text).Length = 0 Then
            txtRecNo.Text = 1
            If lblCount.Text <> Nothing Then
                If CInt(txtRecNo.Text) > CInt(lblCount.Text) Then
                    txtRecNo.Text = lblCount.Text
                End If
                FillData(CInt(txtRecNo.Text) - 1)
            Else
                txtRecNo.Text = ""
            End If
        Else
            If txtRecNo.Text = "0" Then
                txtRecNo.Text = "1"
            End If
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

    Protected Sub ibtnOthers_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        LoadUserRights()
        OnSearchOthers()
    End Sub


#Region "Methods"
    ''' <summary>
    ''' Method to Add Sponsors
    ''' </summary>
    ''' <remarks></remarks>
    ''' modified by Hafiz @ 26/5/2016

    Private Sub addReceipt()
        txtAllocationCode.ReadOnly = True
        txtSpnName.ReadOnly = True
        txtspnAmount.ReadOnly = True

        Dim eobj As New SponsorEn
        Dim eobj1 As New List(Of SponsorEn)
        Dim _AccDal As New HTS.SAS.DataAccessObjects.AccountsDAL

        eobj = Session("spnObj")
        eobj1 = Nothing
        eobj.Address = ""
        eobj1 = _AccDal.GetReciptSpPockAll(eobj)

        Dim amount As Double
        Dim allocatedamount As Double
        txtAllocationCode.Text = eobj.TransactionCode
        txtSpnsName.Text = eobj.Name

        txtspcode.Text = eobj.CreditRef
        allocatedamount = eobj.AllocatedAmount
        amount = eobj.TempAmount
        txtspnAmount.Text = String.Format("{0:F}", eobj.PaidAmount)
        txtAllAmount.Text = String.Format("{0:F}", amount)
        Session("spnObj") = Nothing
    End Sub

    ''' <summary>
    ''' Method to Load Fields in New Mode
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub onAdd()
        Session("PageMode") = "Add"
        Session("AddFee") = Nothing
        today.Value = Now.Date
        today.Value = Format(CDate(today.Value), "dd/MM/yyyy")
        ibtnSave.Enabled = True
        ibtnSave.ImageUrl = "images/save.png"
        lblStatus.Value = "New"
        onClearData()
        If ibtnNew.Enabled = False Then
            ibtnSave.Enabled = False
            ibtnSave.ImageUrl = "images/gsave.png"
            ibtnSave.ToolTip = "Access Denied"
        End If
        OnLoadItem()
        p_load()
    End Sub

    ''' <summary>
    ''' Method to Load Invoice Grid
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub LoadSponsor()
        Dim eob As New AccountsEn
        Dim ListInvObjects As New List(Of AccountsEn)
        Dim obj As New AccountsBAL
        eob.CreditRef = txtStudentCode.Text
        eob.PostStatus = "Posted"
        eob.SubType = "Sponsor"
        eob.TransType = ""
        eob.TransStatus = ""
        ListInvObjects = obj.GetStudentLedgerList(eob)
        If ListInvObjects.Count = 0 Then
            dgInvoices.DataSource = Nothing
            dgInvoices.DataBind()
            lblMsg.Text = "Record did not Exist"
            lblCredit.Visible = False
            lblDebit.Visible = False
            txtCreditAmt.Visible = False
            txtDebitAmt.Visible = False
            lblOut.Visible = False
            txtOutAmt.Visible = False
        Else
            dgInvoices.DataSource = ListInvObjects
            dgInvoices.DataBind()
            Dim dgItem1 As DataGridItem
            Dim link As HyperLink
            Dim str As String
            Dim cat As String
            For Each dgItem1 In dgInvoices.Items
                link = dgItem1.Cells(5).Controls(1)
                str = dgItem1.Cells(7).Text
                cat = dgItem1.Cells(6).Text
                If cat = "Receipt" Then
                    link.NavigateUrl = "Receipts.aspx?Menuid=17&BatchCode=" + str + ";Sp"
                    link.Target = "MyPopup"
                    link.Attributes.Add("onClick", "OpenWindow('about:blank')")
                ElseIf cat = "Payment" Then
                    link.NavigateUrl = "SponsorPayments.aspx?Menuid=20&BatchCode=" + str + ""
                    link.Target = "MyPopup"
                    link.Attributes.Add("onClick", "OpenWindow('about:blank')")
                ElseIf cat = "Allocation" Then
                    link.NavigateUrl = "SponsorAllocation.aspx?Menuid=25&BatchCode=" + str + ""
                    link.Target = "MyPopup"
                    link.Attributes.Add("onClick", "OpenWindow('about:blank')")
                ElseIf cat = "Credit Note" Then
                    link.NavigateUrl = "SponsorCreditNote.aspx?Menuid=21&Formid=CN&BatchCode=" + str + ""
                    link.Target = "MyPopup"
                    link.Attributes.Add("onClick", "OpenWindow('about:blank')")
                ElseIf cat = "Debit Note" Then
                    link.NavigateUrl = "SponsorCreditNote.aspx?Menuid=22&Formid=DN&BatchCode=" + str + ""
                    link.Target = "MyPopup"
                    link.Attributes.Add("onClick", "OpenWindow('about:blank')")
                End If

            Next
            ledgerformat()
            lblCredit.Visible = True
            lblDebit.Visible = True
            txtCreditAmt.Visible = True
            txtDebitAmt.Visible = True
            lblOut.Visible = True
            txtOutAmt.Visible = True
        End If

    End Sub

    ''' <summary>
    ''' Methos to Set the Ledger Format In Grid
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub ledgerformat()
        Dim TotalAmount As Double
        Dim amount As Double
        Dim dr As Double = 0
        Dim cr As Double = 0
        Dim dgItem1 As DataGridItem
        txtCreditAmt.Text = String.Format("{0:F}", 0)
        txtDebitAmt.Text = String.Format("{0:F}", 0)
        txtOutAmt.Text = String.Format("{0:F}", 0)
        'For Each dgItem1 In dgInvoices.Items
        '    If dgItem1.Cells(8).Text = "Debit" Then
        '        TotalAmount = TotalAmount - CDbl(dgItem1.Cells(3).Text)
        '        dgItem1.Cells(4).Text = String.Format("{0:F}", TotalAmount)
        '        amount = dgItem1.Cells(3).Text
        '        dgItem1.Cells(3).Text = String.Format("{0:F}", amount) & " (-)"
        '        cr = cr + amount
        '        txtCreditAmt.Text = String.Format("{0:F}", cr)
        '    Else
        '        'Added by Zoya @ 22/2/2016 - Start
        '        If Not dgItem1.Cells(6).Text = "Receipt" Then
        '            TotalAmount = TotalAmount + CDbl(dgItem1.Cells(3).Text)
        '            dgItem1.Cells(4).Text = String.Format("{0:F}", TotalAmount)
        '            amount = dgItem1.Cells(3).Text
        '            dgItem1.Cells(3).Text = String.Format("{0:F}", amount) & " (+)"
        '            dr = dr + amount
        '            txtDebitAmt.Text = String.Format("{0:F}", dr)
        '        Else
        '            TotalAmount = TotalAmount - CDbl(dgItem1.Cells(3).Text)
        '            dgItem1.Cells(4).Text = String.Format("{0:F}", TotalAmount)
        '            amount = dgItem1.Cells(3).Text
        '            dgItem1.Cells(3).Text = String.Format("{0:F}", amount) & " (-)"
        '            cr = cr + amount

        '            txtCreditAmt.Text = String.Format("{0:F}", cr)
        '        End If
        '        'Added by Zoya @ 22/2/2016 - End
        '    End If
        For Each dgItem1 In dgInvoices.Items
            If dgItem1.Cells(8).Text = "Debit" Then
                TotalAmount = TotalAmount - CDbl(dgItem1.Cells(3).Text)
                dgItem1.Cells(4).Text = String.Format("{0:F}", TotalAmount)
                amount = dgItem1.Cells(3).Text
                'dgItem1.Cells(3).Text = String.Format("{0:F}", amount) & " (-)"
                'cr = cr + amount
                'txtCreditAmt.Text = String.Format("{0:F}", cr)
                dgItem1.Cells(3).Text = String.Format("{0:F}", amount) & " (+)"
                dr = dr + amount
                txtDebitAmt.Text = String.Format("{0:F}", dr)
            Else
                'Modified by Hafiz @ 17/2/2016 - Start
                If Not dgItem1.Cells(6).Text = "Receipt" Then
                    TotalAmount = TotalAmount + CDbl(dgItem1.Cells(3).Text)
                    dgItem1.Cells(4).Text = String.Format("{0:F}", TotalAmount)
                    amount = dgItem1.Cells(3).Text
                    dgItem1.Cells(3).Text = String.Format("{0:F}", amount) & " (-)"
                    cr = cr + amount
                    txtCreditAmt.Text = String.Format("{0:F}", cr)
                Else
                    TotalAmount = TotalAmount - CDbl(dgItem1.Cells(3).Text)
                    dgItem1.Cells(4).Text = String.Format("{0:F}", TotalAmount)
                    amount = dgItem1.Cells(3).Text
                    dgItem1.Cells(3).Text = String.Format("{0:F}", amount) & " (-)"
                    cr = cr + amount

                    txtCreditAmt.Text = String.Format("{0:F}", cr)
                End If
                'Modified by Hafiz @ 17/2/2016 - End
            End If
            If dgItem1.Cells(0).Text = "01/01/0001" Then
                dgItem1.Cells(0).Text = "--"
            End If
        Next
        txtOutAmt.Text = String.Format("{0:F}", CDbl(txtDebitAmt.Text) - CDbl(txtCreditAmt.Text))
    End Sub

    ''' <summary>
    ''' Method to Validate all Views
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub ValidateMultiView(ByVal mv As MultiView)
        Dim savedIndex As Integer = mv.ActiveViewIndex
        For i As Integer = 0 To mv.Views.Count - 1
            mv.ActiveViewIndex = i
            If i = 0 Then
                JavaScript_Functions()
            End If
            'ibtnSave.Attributes.Add("onClick", "return Validate()")
            'ibtnPosting.Attributes.Add("onClick", "return getpostconfirm()")
        Next
        mv.ActiveViewIndex = savedIndex
    End Sub


    ''' <summary>
    ''' Method to Load DateFields
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub OnLoadItem()
        If Session("PageMode") = "Add" Then
            today.Value = Now.Date
            today.Value = Format(CDate(today.Value), "dd/MM/yyyy")
            txtBatchDate.Text = Format(Date.Now, "dd/MM/yyyy")
            txtInvoiceDate.Text = Format(Date.Now, "dd/MM/yyyy")
            txtBatchNo.Text = "Auto Number"
            txtBatchNo.ReadOnly = True
            'added by Hafiz @ 25/5/2016 
            txtAllocationCode.ReadOnly = True
            txtAllocationCode.Text = ""
            txtSpnsName.ReadOnly = True
            txtSpnsName.Text = ""
            txtspnAmount.ReadOnly = True
            txtspnAmount.Text = ""

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
            LogError.Log("SponsorCreditNote", "Menuname", ex.Message)
        End Try
        lblMenuName.Text = eobj.MenuName
    End Sub

    ''' <summary>
    ''' Method To Change the Date Format(dd/MM/yyyy)
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub SetDateFormat()
        Dim myInvoiceDate As Date = CDate(CStr(txtInvoiceDate.Text))
        Dim myFormat As String = "dd/MM/yyyy"
        Dim myFormattedDate As String = Format(myInvoiceDate, myFormat)
        txtInvoiceDate.Text = myFormattedDate
        Dim myBatchDate As Date = CDate(CStr(txtBatchDate.Text))
        Dim myFormattedDate2 As String = Format(myBatchDate, myFormat)
        txtBatchDate.Text = myFormattedDate2
    End Sub

    ''' <summary>
    ''' Method to Validate
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub SpaceValidation()
        ibtnSave.Attributes.Add("onCLick", "return Validate()")
        Dim GBFormat As System.Globalization.CultureInfo
        GBFormat = New System.Globalization.CultureInfo("en-GB")
        'Batch date
        If Trim(txtInvoiceDate.Text).Length < 10 Then
            lblMsg.Text = "Enter Valid Invoice Date"
            lblMsg.Visible = True
            txtInvoiceDate.Focus()
            Exit Sub
        Else
            Try
                txtInvoiceDate.Text = DateTime.Parse(txtInvoiceDate.Text, GBFormat)
            Catch ex As Exception
                lblMsg.Text = "Enter Valid Invoice Date"
                lblMsg.Visible = True
                txtInvoiceDate.Focus()
                Exit Sub
            End Try
        End If
        'Invoice date
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

        If lblStatus.Value = "Posted" Then
            lblMsg.Text = "Record Already Posted"
            lblMsg.Visible = True
            Exit Sub
        End If


    End Sub

    ''' <summary>
    ''' Method to load the Fields for Sponsors Tab
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub OnViewSponsorGrid()
        MultiView1.SetActiveView(View3)
        imgLeft3.ImageUrl = "images/b_white_left.gif"
        imgRight3.ImageUrl = "images/b_white_right.gif"
        btnViewStu.CssClass = "TabButtonClick"

        imgLeft1.ImageUrl = "images/b_orange_left.gif"
        imgRight1.ImageUrl = "images/b_orange_right.gif"
        btnBatchInvoice.CssClass = "TabButton"

        imgLeft2.ImageUrl = "images/b_orange_left.gif"
        imgRight2.ImageUrl = "images/b_orange_right.gif"
        btnSelection.CssClass = "TabButton"

        pnlBatch.Visible = False
        pnlSelection.Visible = False
        pnlSponsorBalance.Visible = False
        pnlViewType.Visible = True

        If Not Session("PageMode") = "Edit" Then

            ddlViewType.Enabled = True

            If Session("ViewType") = "receipt" Then
                ddlViewType.SelectedIndex = 1
                pnlViewReceipt.Visible = True
                pnlViewSponsor.Visible = False
            ElseIf Session("ViewType") = "sponsor" Then
                ddlViewType.SelectedIndex = 2
                pnlViewReceipt.Visible = False
                pnlViewSponsor.Visible = True
            Else
                ddlViewType.SelectedIndex = -1
                pnlViewReceipt.Visible = False
                pnlViewSponsor.Visible = False
            End If

        End If

    End Sub

    ''' <summary>
    ''' Method to Change the Date Format
    ''' </summary>
    ''' <remarks>Date in ddd/mm/yyyy Format</remarks>
    Private Sub dates()
        Dim GBFormat As System.Globalization.CultureInfo
        GBFormat = New System.Globalization.CultureInfo("en-GB")

        txtInvoiceDate.Text = Format(Date.Now, "dd/MM/yyyy")
        txtBatchDate.Text = Format(Date.Now, "dd/MM/yyyy")
    End Sub

    ''' <summary>
    ''' Method to Add Sponsors to Grid
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub addSponsor()
        Dim listspnList As List(Of SponsorEn)
        Dim eobj As SponsorEn

        eobj = Session("eobjspn")
        txtStudentCode.Text = eobj.SponserCode
        txtStuName.Text = eobj.Name
        txtDate.Text = Date.Now
        lblMsg.Text = ""

        'Checking for duplicate sponsor
        If Not Session("eobjs") Is Nothing Then
            listspnList = Session("eobjs")
            Dim i As Integer = 0
            Dim Flag As Boolean = False
            While i < listspnList.Count
                If eobj.SponserCode = listspnList(i).SponserCode Then
                    Flag = True
                    Exit While
                End If
                i = i + 1
            End While
            If Flag = False Then
                listspnList = New List(Of SponsorEn)
                listspnList.Add(eobj)
            End If
        Else
            listspnList = New List(Of SponsorEn)
            listspnList.Add(eobj)
        End If

        'LoadSponsor()
        dgSponsor.DataSource = listspnList
        dgSponsor.DataBind()

        Session("eobjspn") = Nothing
        Session("eobjs") = Nothing

    End Sub

    ''' <summary>
    ''' Method to get the List Of Sponsor CreditNotes
    ''' </summary>
    ''' <remarks></remarks>
    ''' 
    Public Sub LoadListObjects()
        Dim obj As New AccountsBAL
        Dim menuId As Integer
        Dim Mylist As New List(Of AccountsEn)
        Dim EnObj As New AccountsEn
        Try
            menuId = CInt(Request.QueryString("Menuid"))

            If Session("loaddata") = "others" Then
                If ddlNoteType.SelectedIndex = 2 Then
                    EnObj.TransType = "Credit"
                    EnObj.Category = "Credit Note"
                ElseIf ddlNoteType.SelectedIndex = 1 Then
                    EnObj.TransType = "Debit"
                    EnObj.Category = "Debit Note"
                ElseIf ddlNoteType.SelectedIndex = 0 Then
                    EnObj.Category = ""
                    EnObj.TransType = ""
                End If

                If txtBatchNo.Text <> "Auto Number" Then
                    EnObj.BatchCode = txtBatchNo.Text
                Else
                    EnObj.BatchCode = ""
                End If

                EnObj.PostStatus = "Posted"
                EnObj.SubType = "Sponsor"

                If CInt(Request.QueryString("IsView")).Equals(1) Then
                    EnObj.PostStatus = "Ready"
                End If

                Try
                    ListObjects = obj.GetTransactions(EnObj)
                Catch ex As Exception
                    LogError.Log("SponsorCreditNote", "LoadListObjects", ex.Message)
                End Try

            ElseIf Session("loaddata") = "View" Then
                If ddlNoteType.SelectedIndex = 2 Then
                    EnObj.TransType = "Credit"
                    EnObj.Category = "Credit Note"
                ElseIf ddlNoteType.SelectedIndex = 1 Then
                    EnObj.TransType = "Debit"
                    EnObj.Category = "Debit Note"
                ElseIf ddlNoteType.SelectedIndex = 0 Then
                    EnObj.Category = ""
                    EnObj.TransType = ""
                End If

                If txtBatchNo.Text <> "Auto Number" Then
                    EnObj.BatchCode = txtBatchNo.Text
                Else
                    EnObj.BatchCode = ""
                End If

                EnObj.PostStatus = "Ready"
                EnObj.SubType = "Sponsor"

                Try
                    ListObjects = obj.GetTransactions(EnObj)
                Catch ex As Exception
                    LogError.Log("SponsorCreditNote", "LoadListObjects", ex.Message)
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
                    lblMsg.Visible = True
                    ibtnSave.Enabled = True
                    ibtnSave.ImageUrl = "images/save.png"
                    trPrint.Visible = True
                Else
                    Session("PageMode") = ""
                    ibtnSave.Enabled = False
                    ibtnSave.ImageUrl = "images/gsave.png"
                End If
            Else
                txtRecNo.Text = ""
                lblCount.Text = ""
                'onClearData()

                If DFlag = "Delete" Then
                Else
                    lblMsg.Visible = True
                    ErrorDescription = "Record did not Exist"
                    lblMsg.Text = ErrorDescription
                    DFlag = ""
                End If
            End If

        Catch ex As Exception
            'Log & Show Error - Start
            Call MaxModule.Helper.LogError(ex.Message)
            Call SetMessage(ex.Message)
            'Log & Show Error - Stop
        End Try
    End Sub

    ''' <summary>
    ''' Method to Save and Update Sponsor Credit 0r DebitNotes
    ''' </summary>
    ''' <remarks></remarks>
    ''' modified by Hafiz @ 26/5/2016

    Private Sub onSave()
        Dim eobj As New AccountsEn
        Dim bsobj As New AccountsBAL
        Dim eobjs As New SponsorEn
        Dim Listsponser As New List(Of SponsorEn)

        GBFormat = New System.Globalization.CultureInfo("en-GB")

        Dim TRT As String
        Dim Status As String
        Dim menuId As Integer
        lblMsg.Text = ""
        lblMsg.Visible = True
        menuId = CInt(Request.QueryString("Menuid"))

        If Session("ViewType") = "receipt" Then

            If Not String.IsNullOrEmpty(txtAllocationCode.Text) Then

                Dim bal As Double = 0.0
                bal = CDbl(txtAllAmount.Text)
                bal = String.Format("{0:F}", bal)
                If bal >= CDbl(txtTotal.Text) Then
                    Dim amount As Double
                    amount = txtTotal.Text
                    txtTotal.Text = String.Format("{0:F}", amount)

                    eobj.CreditRefOne = Trim(txtAllocationCode.Text)
                    eobj.CrRefOne = Trim(txtSpnsName.Text)
                    eobj.SubReferenceOne = Trim(txtspnAmount.Text)
                    eobj.SubReferenceTwo = Trim(txtAllAmount.Text)
                    eobj.CreditRef = Trim(txtspcode.Text)

                    Dim spnObj As New SponsorEn
                    spnObj.SponserCode = eobj.CreditRef
                    spnObj.Name = eobj.CrRefOne

                    Listsponser.Add(spnObj)
                    spnObj = Nothing
                Else
                    txtTotal.Text = 0.0
                    txtTotal.Focus()
                    lblMsg.Text = "Total Amount Exceed the Available Amount"
                    lblMsg.Visible = True
                    Exit Sub
                End If

            Else
                lblMsg.Text = "Receipt No Cannot Be Empty"
                lblMsg.Visible = True
                Exit Sub
            End If

        ElseIf Session("ViewType") = "sponsor" Then

            eobj.CreditRefOne = ""
            eobj.CrRefOne = ""
            eobj.SubReferenceOne = ""
            eobj.SubReferenceTwo = ""
            eobj.CreditRef = ""

            Dim chkBox As CheckBox
            For Each dgi As DataGridItem In dgSponsor.Items
                chkBox = dgi.Cells(0).FindControl("chk")
                Dim spnObj As New SponsorEn
                If chkBox.Checked = True Then
                    spnObj = New SponsorEn
                    spnObj.SponserCode = dgSponsor.DataKeys(dgi.ItemIndex).ToString
                    spnObj.Name = dgi.Cells(2).Text
                    Listsponser.Add(spnObj)
                    spnObj = Nothing
                End If
            Next

            If Listsponser.Count = 0 Then
                lblMsg.Text = "Select At Least One Sponsor"
                lblMsg.Visible = True
                Exit Sub
            End If

        Else
            lblMsg.Text = "Select Sponsor Either By Receipt Or Sponsor"
            lblMsg.Visible = True
            Exit Sub
        End If

        If ddlNoteType.SelectedIndex = 2 Then
            eobj.Category = "Credit Note"
            Status = "O"
            TRT = "TSCN"
            eobj.TransType = "Credit"
        ElseIf ddlNoteType.SelectedIndex = 1 Then
            Status = "O"
            eobj.Category = "Debit Note"
            eobj.TransType = "Debit"
        End If

        eobj.Description = Trim(txtDesc.Text)
        eobj.TransDate = Trim(txtInvoiceDate.Text)
        eobj.BatchDate = Trim(txtBatchDate.Text)
        eobj.SubType = "Sponsor"
        eobj.DueDate = DateTime.Now
        eobj.PostedDateTime = DateTime.Now
        eobj.UpdatedTime = DateTime.Now
        eobj.ChequeDate = DateTime.Now
        eobj.CreatedBy = Session("User")
        eobj.CreatedDateTime = DateTime.Now
        eobj.PostStatus = "Ready"
        eobj.TransStatus = "Open"
        eobj.BatchCode = Trim(txtBatchNo.Text)
        eobj.TempAmount = Trim(txtTotal.Text)
        eobj.UpdatedBy = Session("User")
        eobj.GLCode = Trim(txtGLCode.Text)

        If Session("PageMode") = "Add" Then
            Try
                txtBatchNo.Text = bsobj.SponsorBatchInsert(eobj, Listsponser)
                txtBatchNo.ReadOnly = True
                ErrorDescription = "Record Saved Successfully "
                ibtnStatus.ImageUrl = "images/ready.gif"
                lblStatus.Value = "Ready"
                lblMsg.Text = ErrorDescription

            Catch ex As Exception
                lblMsg.Text = ex.Message.ToString()
                Response.Write(ex.Message.ToString())
                LogError.Log("SponsorCreditNote", "Onsave", ex.Message)
            End Try

        ElseIf Session("PageMode") = "Edit" Then
            Try
                txtBatchNo.Text = bsobj.SponsorBatchUpdate(eobj, Listsponser)
                ErrorDescription = "Record Updated Successfully "
                ibtnStatus.ImageUrl = "images/ready.gif"
                lblStatus.Value = "Ready"
                lblMsg.Text = ErrorDescription
            Catch ex As Exception
                lblMsg.Text = ex.Message.ToString()

                LogError.Log("SponsorCreditNote", "Onsave", ex.Message)
            End Try

        End If

    End Sub

    Private Sub JavaScript_Functions()

        Dim strScript As String = ""
        ClientScript.RegisterStartupScript(Me.GetType(), "Confirm", "getpostconfirm();", True)
        'End If
    End Sub

    ''' <summary>
    ''' Method to Delete Sponsor Credit or Debit Notes 
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub ondelete()
        lblMsg.Visible = True
        If Trim(txtBatchNo.Text).Length <> 0 Then
            If lblCount.Text = "" Then lblCount.Text = 0
            If lblCount.Text > 0 Then

                Dim bsobj As New AccountsBAL
                Dim eobj As New AccountsEn

                Dim RecAff As Integer
                lblMsg.Visible = True

                Try
                    eobj.BatchCode = txtBatchNo.Text
                    RecAff = bsobj.BatchDelete(eobj)
                    lblMsg.Visible = True
                    ErrorDescription = "Record Deleted Successfully "
                    lblMsg.Text = ErrorDescription

                Catch ex As Exception
                    lblMsg.Text = ex.Message.ToString()
                    LogError.Log("SponsorCreditNote", "OnDelete", ex.Message)
                End Try

                Session("ListObj") = Nothing
                DFlag = "Delete"
                Session("loaddata") = "View"
                txtAllocationCode.ReadOnly = True
                txtSpnsName.ReadOnly = True
                txtspnAmount.ReadOnly = True
                txtAllAmount.ReadOnly = True
                txtAllocationCode.Text = ""
                txtSpnsName.Text = ""
                txtspnAmount.Text = ""
                txtAllAmount.Text = ""
                txtBatchNo.Text = ""
                txtBatchDate.Text = ""
                txtInvoiceDate.Text = ""
                txtGLCode.Text = ""
                txtDesc.Text = ""
                txtTotal.Text = ""
                txtSpnCode.Text = ""
                txtSpnName.Text = ""
                txtSpnType.Text = ""
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
    ''' Method to Clear the Field Values
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub onClearData()
        Session("ListObj") = Nothing
        DisableRecordNavigator()
        Session("AddFee") = Nothing
        'added by Hafiz @ 25/5/2016 
        txtAllocationCode.ReadOnly = True
        txtAllocationCode.Text = ""
        txtSpnsName.ReadOnly = True
        txtSpnsName.Text = ""
        txtspnAmount.ReadOnly = True
        txtspnAmount.Text = ""
        txtAllAmount.ReadOnly = True
        txtAllAmount.Text = ""
        txtBatchNo.Text = ""
        txtBatchDate.Text = ""
        txtInvoiceDate.Text = ""
        txtDesc.Text = ""
        txtTotal.Text = ""
        txtSpnCode.Text = ""
        txtSpnName.Text = ""
        txtSpnType.Text = ""
        lblMsg.Text = ""
        txtGLCode.Text = ""
        lblResultGlCode.Text = ""
        ddlNoteType.SelectedValue = "-1"
        ddlViewType.SelectedValue = "-1"
        imgGL.Visible = False
        dgView.DataSource = Nothing
        dgView.DataBind()
        dgSponsor.DataSource = Nothing
        dgSponsor.DataBind()
        ibtnStatus.ImageUrl = "images/notready.gif"
        'OnLoadItem()

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
            LogError.Log("SponsorCreditNote", "LoadUserRights", ex.Message)
        End Try
        'Rights for Add

        If eobj.IsAdd = True Then
            'ibtnSave.Enabled = True
            'onAdd()

            Session("PageMode") = "Add"
            Session("AddFee") = Nothing
            today.Value = Now.Date
            today.Value = Format(CDate(today.Value), "dd/MM/yyyy")
            ibtnSave.Enabled = True
            ibtnSave.ImageUrl = "images/save.png"
            lblStatus.Value = "New"

            'Start ClearData
            Session("ListObj") = Nothing
            DisableRecordNavigator()
            Session("AddFee") = Nothing
            txtBatchNo.Text = ""
            txtBatchDate.Text = ""
            txtInvoiceDate.Text = ""
            txtDesc.Text = ""
            txtTotal.Text = ""
            txtSpnCode.Text = ""
            txtSpnName.Text = ""
            txtSpnType.Text = ""
            lblMsg.Text = ""
            txtGLCode.Text = ""
            lblResultGlCode.Text = ""
            'ddlNoteType.SelectedValue = "-1"
            imgGL.Visible = False
            dgView.DataSource = Nothing
            dgView.DataBind()
            dgSponsor.DataSource = Nothing
            dgSponsor.DataBind()
            ibtnStatus.ImageUrl = "images/notready.gif"
            'end clear Data

            If ibtnNew.Enabled = False Then
                ibtnSave.Enabled = False
                ibtnSave.ImageUrl = "images/gsave.png"
                ibtnSave.ToolTip = "Access Denied"
            End If
            OnLoadItem()
            p_load()

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
        hfIsPrint.Value = eobj.IsPrint
        'ibtnPrint.Enabled = eobj.IsPrint
        'If eobj.IsPrint = True Then
        '    ibtnPrint.Enabled = True
        '    ibtnPrint.ImageUrl = "images/print.png"
        '    ibtnPrint.ToolTip = "Print"
        'Else
        '    ibtnPrint.Enabled = False
        '    ibtnPrint.ImageUrl = "images/gprint.png"
        '    ibtnPrint.ToolTip = "Access Denied"
        'End If
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

#Region "Added by Hafiz @ 06/4/2016 - print"

    Private Sub PrintAble()
        Dim IsPrint As Boolean = hfIsPrint.Value
        ibtnPrint.Enabled = IsPrint
        If IsPrint = True Then
            ibtnPrint.Enabled = True
            ibtnPrint.ImageUrl = "~/images/print.png"
            ibtnPrint.ToolTip = "Print"
        Else
            DisablePrint()
        End If
    End Sub

    Private Sub DisablePrint()
        ibtnPrint.Enabled = False
        ibtnPrint.ImageUrl = "~/images/gprint.png"
        ibtnPrint.ToolTip = "Access Denied"
    End Sub

#End Region


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
                Dim obj As AccountsEn

                ListObjects = Session("ListObj")
                obj = ListObjects(RecNo)

                txtspcode.Text = obj.CreditRef
                ddlNoteType.SelectedValue = obj.Category
                txtGLCode.Text = obj.GLCode
                txtBatchNo.Text = obj.BatchCode
                txtBatchDate.Text = obj.BatchDate
                txtInvoiceDate.Text = obj.TransDate
                txtDesc.Text = obj.Description

                If obj.PostStatus = "Ready" Then
                    txtTotal.Text = String.Format("{0:F}", obj.TempAmount)
                Else
                    txtTotal.Text = String.Format("{0:F}", obj.TransactionAmount)
                End If

                ibtnStatus.ImageUrl = "images/ready.gif"
                lblStatus.Value = "Ready"

                If obj.PostStatus = "Ready" Then
                    lblStatus.Value = "Ready"
                    ibtnStatus.ImageUrl = "images/Ready.gif"
                    DisablePrint()
                End If
                If obj.PostStatus = "Posted" Then
                    lblStatus.Value = "Posted"
                    ibtnStatus.ImageUrl = "images/Posted.gif"
                    PrintAble()
                End If

                If Not String.IsNullOrEmpty(obj.CreditRefOne) Then
                    'Receipt
                    ddlViewType.SelectedIndex = 1
                    ddlViewType.Enabled = False
                    Check_Type()

                    txtAllocationCode.Text = obj.CreditRefOne
                    txtSpnsName.Text = obj.CrRefOne
                    txtspnAmount.Text = String.Format("{0:F}", obj.SubReferenceOne)
                    txtAllAmount.Text = String.Format("{0:F}", obj.SubReferenceTwo)
                Else
                    'Sponsor
                    ddlViewType.SelectedIndex = 2
                    ddlViewType.Enabled = False
                    Check_Type()

                    Dim mylst As New List(Of SponsorEn)
                    Dim bsobj As New AccountsBAL

                    Try
                        mylst = bsobj.GetSponserListByBatchID(obj.BatchCode)
                    Catch ex As Exception
                        LogError.Log("SponsorCreditNote", "FillData", ex.Message)
                    End Try
                    dgSponsor.DataSource = mylst
                    dgSponsor.DataBind()
                    Session("eobjs") = mylst

                    Dim chkBox As CheckBox
                    Dim dgitem As DataGridItem
                    For Each dgitem In dgSponsor.Items
                        chkBox = dgitem.Cells(0).FindControl("chk")
                        chkBox.Checked = True
                    Next
                End If

                CheckWorkflowStatus(obj)

            End If
        End If

        SetDateFormat()

    End Sub

    ''' <summary>
    ''' Method to Load Sponsors in Grid
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub LoadGrid()
        Dim ds As New DataSet
        Dim eobj As New SponsorEn
        Dim bsobjs As New SponsorBAL
        Dim list As New List(Of SponsorEn)
        chkSelectSponsor.Visible = True
        eobj.SponserCode = txtSpnCode.Text
        eobj.Name = txtSpnName.Text
        eobj.Type = txtSpnType.Text
        eobj.GLAccount = ""
        eobj.Status = True

        Try
            list = bsobjs.GetSponserList(eobj)
        Catch ex As Exception
            LogError.Log("SponsorCreditNote", "LoadGrid", ex.Message)
        End Try
        dgView.DataSource = list
        dgView.DataBind()
        If dgView.Items.Count = 0 Then
            lblMsg.Text = "Record did not Exist"
            lblMsg.Visible = True
            chkSelectSponsor.Visible = False
        End If
    End Sub

    ''' <summary>
    ''' Method to Add Sponsors
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub getsponsorlist()
        Dim Listsponser As New List(Of SponsorEn)
        Dim chkBox As CheckBox
        For Each dgi As DataGridItem In dgView.Items
            chkBox = dgi.Cells(0).Controls(1)
            Dim eobj As New SponsorEn
            If chkBox.Checked = True Then
                eobj = New SponsorEn
                eobj.SponserCode = dgView.DataKeys(dgi.ItemIndex).ToString
                eobj.Name = dgi.Cells(2).Text
                Listsponser.Add(eobj)
                eobj = Nothing
            End If
        Next
        If Listsponser.Count = 0 Then
            lblMsg.Text = "Select At least One Sponsor"
            lblMsg.Visible = True
            Exit Sub
        End If
        OnViewSponsorGrid()

        'Adding in the exisiting list
        Dim mylst As List(Of SponsorEn)
        Dim i As Integer = 0
        If Not Session("eobjs") Is Nothing Then
            Dim Flag As Boolean
            mylst = Session("eobjs")
            While i < Listsponser.Count
                Dim j As Integer = 0
                While j < mylst.Count
                    Flag = False
                    If mylst(j).SponserCode = Listsponser(i).SponserCode Then
                        Flag = True
                        Exit While
                    End If
                    j = j + 1
                End While
                If Flag = False Then
                    mylst.Add(Listsponser(i))
                End If
                i = i + 1
            End While
        Else
            mylst = Listsponser
        End If

        chkSponsor.Visible = True
        Session("eobjs") = mylst
        dgSponsor.DataSource = mylst
        dgSponsor.DataBind()


    End Sub

    ''' <summary>
    ''' Method to Post Sponsor Credit or Debit Notes
    ''' </summary>
    ''' <remarks></remarks>
    Private Function OnPost() As Boolean

        Dim result As Boolean = False
        Dim eobj As New AccountsEn
        Dim bsobj As New AccountsBAL
        Dim Listsponser As New List(Of SponsorEn)

        GBFormat = New System.Globalization.CultureInfo("en-GB")

        Dim TRT As String
        Dim Status As String
        Dim menuId As Integer
        Dim bid As String = ""
        lblMsg.Text = ""
        lblMsg.Visible = True

        eobj = New AccountsEn
        If ddlNoteType.SelectedIndex = 1 Then

            Status = "O"
            TRT = "SDN"
            eobj.TransType = "Debit"
            eobj.Category = "Debit Note"
        ElseIf ddlNoteType.SelectedIndex = 2 Then

            Status = "O"
            TRT = "SCN"
            eobj.TransType = "Credit"
            eobj.Category = "Credit Note"
        End If

        lblMsg.Text = ""
        lblMsg.Visible = True
        menuId = CInt(Request.QueryString("Menuid"))

        If Session("ViewType") = "receipt" Then

            eobj.CreditRefOne = Trim(txtAllocationCode.Text)
            eobj.CrRefOne = Trim(txtSpnsName.Text)
            eobj.SubReferenceOne = Trim(txtspnAmount.Text)
            eobj.SubReferenceTwo = Trim(txtAllAmount.Text)
            eobj.CreditRef = Trim(txtspcode.Text)

            Dim spnObj As New SponsorEn
            spnObj.SponserCode = eobj.CreditRef
            spnObj.Name = eobj.CrRefOne

            Listsponser.Add(spnObj)
            spnObj = Nothing

        ElseIf Session("ViewType") = "sponsor" Then

            Dim chkBox As CheckBox
            For Each dgi As DataGridItem In dgSponsor.Items
                chkBox = dgi.Cells(0).FindControl("chk")
                Dim spnObj As New SponsorEn
                If chkBox.Checked = True Then
                    spnObj = New SponsorEn
                    spnObj.SponserCode = dgSponsor.DataKeys(dgi.ItemIndex).ToString
                    spnObj.Name = dgi.Cells(2).Text
                    Listsponser.Add(spnObj)
                    spnObj = Nothing
                End If
            Next

        End If

        eobj.Description = Trim(txtDesc.Text)
        eobj.TransDate = Trim(txtInvoiceDate.Text)
        eobj.BatchDate = Trim(txtBatchDate.Text)
        eobj.SubType = "Sponsor"
        eobj.DueDate = DateTime.Now
        eobj.PostedBy = Session("User")
        eobj.PostedDateTime = DateTime.Now
        eobj.UpdatedTime = DateTime.Now
        eobj.ChequeDate = DateTime.Now
        eobj.CreatedDateTime = DateTime.Now
        eobj.CreditRef = Trim(txtspcode.Text)
        eobj.PostStatus = "Ready"
        eobj.TransStatus = "Closed"
        eobj.UpdatedBy = Session("User")
        eobj.BatchCode = Trim(txtBatchNo.Text)
        eobj.TempAmount = Trim(txtTotal.Text)
        eobj.TransactionAmount = Trim(txtTotal.Text)
        eobj.GLCode = Trim(txtGLCode.Text)

        Try
            txtBatchNo.Text = bsobj.SponsorBatchUpdate(eobj, Listsponser)

            If Not String.IsNullOrEmpty(txtBatchNo.Text) Then
                result = True
            Else
                result = False
            End If

            If Not Session("ListObj") Is Nothing Then
                ListObjects = Session("ListObj")
                If lblStatus.Value = "Posted" Then
                    ibtnStatus.ImageUrl = "images/posted.gif"
                    lblStatus.Value = "Posted"
                End If
            End If
            lblMsg.Text = ErrorDescription

        Catch ex As Exception
            result = False
            lblMsg.Text = ex.Message.ToString()
            LogError.Log("SponsorCreditNote", "Onpost", ex.Message)
        End Try

        Return result

    End Function

    ''' <summary>
    ''' Method to Search for Posted Records
    ''' </summary>
    ''' <remarks></remarks>
    ''' 
    Private Sub OnSearchOthers()
        Session("loaddata") = "others"

        If lblCount.Text <> "" Then
            If CInt(lblCount.Text) > 0 Then
                onClearData()
                If ibtnNew.Enabled = False Then
                    ibtnSave.Enabled = False
                    ibtnSave.ImageUrl = "images/gsave.png"
                    ibtnSave.ToolTip = "Access Denied"
                End If
            Else
                Session("PageMode") = "Edit"
                LoadListObjects()

            End If
        Else
            Session("PageMode") = "Edit"
            LoadListObjects()

            PostEnFalse()
        End If

        If CInt(Request.QueryString("IsView")).Equals(1) Then
            ddlNoteType.Enabled = False
            txtBatchDate.Enabled = False
            txtInvoiceDate.Enabled = False
            txtGLCode.Enabled = False
            txtDesc.Enabled = False
            txtTotal.Enabled = False
        End If

    End Sub

    ''' <summary>
    ''' Method to Disable Options After Posting
    ''' </summary>
    ''' <remarks></remarks>
    ''' 
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
        'ibtnPrint.Enabled = False
        'ibtnPrint.ImageUrl = "images/gprint.png"
        'ibtnPrint.ToolTip = "Access denied"
        ibtnPosting.Enabled = False
        ibtnPosting.ImageUrl = "images/gposting.png"
        ibtnPosting.ToolTip = "Access denied"
        'ibtnOthers.Enabled = False
        'ibtnOthers.ImageUrl = "images/post.png"
        'ibtnOthers.ToolTip = "Access denied"
    End Sub

    ''' <summary>
    ''' Method to Check GL Code
    ''' </summary>
    ''' <param name="strGLColde"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    ''' 
    Private Function CheckCode(ByVal strGLColde As String) As Boolean

        Try
            imgGL.Visible = False
            Dim oleCF As New OleDb.OleDbConnection
            Dim dsCF As New DataSet
            Dim strsql As String
            oleCF.ConnectionString = "Provider=Ifxoledbc.2;Password=acctdb;Persist Security Info=True;User ID=acctdb;Data Source=umklive@ol_saga;Extended Properties="
            strsql = "select glac_account, glac_desc from gl_account where  glac_account = '" & strGLColde & "'"
            Dim daCF As New OleDb.OleDbDataAdapter(strsql, oleCF)
            daCF.Fill(dsCF, "gl_account")

            If dsCF.Tables("gl_account").Rows.Count > 0 Then
                imgGL.ImageUrl = "~/images/check.png"
                CheckCode = True
            Else
                imgGL.ImageUrl = "~/images/cross.png"
                CheckCode = False
            End If

        Catch ex As Exception
            Throw ex
        End Try

        Return CheckCode
    End Function
#End Region

    Protected Sub ibtnafterpost_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        If scriptid.Value = "No" Then
            Exit Sub
        End If

    End Sub

    Protected Sub btnViewBalanceSponsor_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnViewBalanceSponsor.Click
        MultiView1.SetActiveView(View4)
        imgLeft2.ImageUrl = "images/b_white_left.gif"
        imgRight2.ImageUrl = "images/b_white_right.gif"
        btnSelection.CssClass = "TabButton"


        imgLeft1.ImageUrl = "images/b_orange_left.gif"
        imgRight1.ImageUrl = "images/b_orange_right.gif"
        btnBatchInvoice.CssClass = "TabButton"

        imgLeft3.ImageUrl = "images/b_orange_left.gif"
        imgRight3.ImageUrl = "images/b_orange_right.gif"
        btnViewStu.CssClass = "TabButton"

        btnViewBalanceSponsor.CssClass = "TabButtonClick"

        pnlBatch.Visible = False
        pnlSelection.Visible = True
        pnlViewType.Visible = False
        pnlViewSponsor.Visible = False
        pnlSponsorBalance.Visible = True
    End Sub

#Region "GL Checking "

    Protected Sub ibtnCheckGL_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ibtnCheckGL.Click
        Dim result As Boolean = False
        Dim _GLCode As String = txtGLCode.Text
        Dim _GLLedgerType As String = Nothing
        Dim _GLDescription As String = Nothing

        Try
            imgGL.Visible = False
            'Adding validation for Check button
            ibtnCheckGL.Attributes.Add("onclick", "return validate()")
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
                        lblResultGlCode.Text = _GLDescription
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
        Catch ex As Exception
            'Log & Show Error - Start
            Call MaxModule.Helper.LogError(ex.Message)
            Call SetMessage(ex.Message)
            'Log & Show Error - Stop
        End Try
    End Sub

#End Region

#Region "Set Message "

    Private Sub SetMessage(ByVal MessageDetails As String)

        lblMsg.Text = String.Empty
        lblMsg.Text = MessageDetails

    End Sub

#End Region

    Protected Sub btnHidden_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnHidden.Click
        'If Not Session("spnObj") Is Nothing Then
        '    addspnCOde()
        '    addsponser()
        'End If
    End Sub

#Region "View Type "
    'added by Hafiz @ 1/6/2016

    Protected Sub ddlViewType_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlViewType.SelectedIndexChanged

        Check_Type()

    End Sub

    Private Sub Check_Type()

        If ddlViewType.SelectedIndex = 1 Then
            pnlViewReceipt.Visible = True
            pnlViewSponsor.Visible = False

            Session("ViewType") = "receipt"
        ElseIf ddlViewType.SelectedIndex = 2 Then
            pnlViewSponsor.Visible = True
            pnlViewReceipt.Visible = False

            Session("ViewType") = "sponsor"
        Else
            ddlViewType.Visible = True
            pnlViewReceipt.Visible = False
            pnlViewSponsor.Visible = False

            Session("ViewType") = Nothing
        End If

        ClearData_ViewType()

    End Sub

    Private Sub ClearData_ViewType()

        If ddlViewType.SelectedIndex = 1 Then
            chkSponsor.Checked = False
            dgSponsor.DataSource = Nothing
            dgSponsor.DataBind()
        ElseIf ddlViewType.SelectedIndex = 2 Then
            txtAllocationCode.Text = ""
            txtSpnsName.Text = ""
            txtspnAmount.Text = ""
            txtAllAmount.Text = ""
        Else
            txtAllocationCode.Text = ""
            txtSpnsName.Text = ""
            txtspnAmount.Text = ""
            txtAllAmount.Text = ""
            chkSponsor.Checked = False
            dgSponsor.DataSource = Nothing
            dgSponsor.DataBind()
        End If

    End Sub

#End Region

#Region "GetApprovalDetails"

    Protected Function GetMenuId() As Integer

        Dim MenuId As Integer = New MenuDAL().GetMenuMasterList().Where(Function(x) x.PageName = "Sponsor Debit/Credit Note").Select(Function(y) y.MenuID).FirstOrDefault()
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

