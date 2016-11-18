Imports HTS.SAS.Entities
Imports HTS.SAS.BusinessObjects
Imports System.Data
Imports System.Collections.Generic

Partial Class ChequeManagement
    Inherits System.Web.UI.Page
    Dim ListCheques As New List(Of ChequeDetailsEn)
    Dim listPayments As New List(Of AccountsDetailsEn)
    Dim listaccounts As New List(Of AccountsEn)
    Dim listRefund As New List(Of StudentEn)
    Private ListObjects As List(Of ChequeEn)
    Dim ErrorDescription As String
    Dim DFlag As String
    Dim dateflag As String
    Dim datestring As String
    ''Private LogErrors As LogError
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack() Then
            LoadUserRights()
            Session("PayforCheque") = Nothing
            Session("Cheques") = Nothing
            Session("ChePayments") = Nothing
            Session("PageMode") = "Add"
            Session("Menuid") = Request.QueryString("Menuid")
            'load PageName
            Menuname(CInt(Request.QueryString("Menuid")))
            dates()
            'Client Validations
            txtChequeDate.Attributes.Add("OnKeyup", "return CheckChequeDate()")
            txtChequeDate.Attributes.Add("onblur", "return ChangeDate(event,'" & txtChequeDate.ClientID & "')")
            txtCStart.Attributes.Add("onKeypress", "checkValue()")
            txtCEND.Attributes.Add("onKeypress", "checkValue()")
            ibtnDelete.Attributes.Add("onclick", "return getconfirm()")
            ibtnPosting.Attributes.Add("onClick", "return getpostconfirm()")
            ibtnView.Attributes.Add("onclick", "return CheckSearch()")
            ibtnOthers.Attributes.Add("onclick", "return CheckSearch()")
            ibtnSave.Attributes.Add("onclick", "return Validate()")
            ChequeNo.Attributes.Add("OnClick", "return Chequealidate()")
            ibtnChequetDate.Attributes.Add("onClick", "return getChequeDate()")
            ibtnAll.Attributes.Add("onclick", "new_window=window.open('AddPayments.aspx?cat=1','Hanodale','width=520,height=350,resizable=0');new_window.focus();")
            ibtnRef.Attributes.Add("onclick", "new_window=window.open('AddPayments.aspx?cat=St','Hanodale','width=550,height=350,resizable=0');new_window.focus();")
            ibtnSpPay.Attributes.Add("onclick", "new_window=window.open('AddPayments.aspx?cat=2','Hanodale','width=550,height=350,resizable=0');new_window.focus();")
            OnLoadItem()
            DisableRecordNavigator()

        End If
        If Not Session("PayforCheque") Is Nothing Then
            addPayments()
        End If
    End Sub
#Region "Methods"
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
            LogError.Log("ChequeManagement", "Menuname", ex.Message)
        End Try

        lblMenuName.Text = eobj.MenuName
    End Sub
    ''' <summary>
    ''' Method to Load DateFields
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub OnLoadItem()
        If Session("PageMode") = "Add" Then
            txtBatchid.Text = "Auto Number"
            txtBatchid.ReadOnly = True
        End If
        lblMsg.Text = ""
        txtPaymentDate.ReadOnly = True
        txtBDate.ReadOnly = True
    End Sub
    ''' <summary>
    ''' Method to Change the Date Format
    ''' </summary>
    ''' <remarks>Date in ddd/mm/yyyy Format</remarks>
    Private Sub dates()
        txtChequeDate.Text = Format(Date.Now, "dd/MM/yyyy")
        txtPaymentDate.Text = ""
        txtBDate.Text = ""

    End Sub
    ''' <summary>
    ''' Method To Change the Date Format(dd/MM/yyyy)
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub setDateFormat()
        Dim myFormat As String = "dd/MM/yyyy"
        'Dim GBFormat As System.Globalization.CultureInfo
        If Not datestring = "yes" Then
            'GBFormat = New System.Globalization.CultureInfo("en-GB")
            Dim myPaymentDate As Date = CDate(CStr(txtPaymentDate.Text))

            Dim myFormattedDate As String = Format(myPaymentDate, myFormat)
            txtPaymentDate.Text = myFormattedDate
            Dim myBatchDate As Date = CDate(CStr(txtBDate.Text))
            Dim myFormattedDate2 As String = Format(myBatchDate, myFormat)
            txtBDate.Text = myFormattedDate2
        End If
        If Not dateflag = "no" Then
            Dim myChequeDate As Date = CDate(CStr(txtChequeDate.Text))
            Dim myFormattedDate3 As String = Format(myChequeDate, myFormat)
            txtChequeDate.Text = myFormattedDate3
        End If

    End Sub
    ''' <summary>
    ''' Method to Add Payments to Dropdown
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub addPayments()

        Dim eobjpay As AccountsEn
        Dim loen As New AccountsEn
        Dim bobj As New AccountsBAL
        Dim listTrcpt As New List(Of StudentEn)
        Dim listAccounts As New List(Of AccountsEn)
        Dim estu As New StudentEn
        eobjpay = Session("PayforCheque")
        txtAllCode.ReadOnly = False
        txtAllCode.Text = eobjpay.TransactionCode
        txtAllCode.ReadOnly = True
        txtPaymentDate.Text = eobjpay.TransDate
        txtAllCode.ReadOnly = True
        txtBDate.Text = eobjpay.BatchDate
        txtAllCode.ReadOnly = True
        txtBankCode.Text = eobjpay.BankCode
        txtBankCode.ReadOnly = True
        dateflag = "no"
        setDateFormat()
        dateflag = ""
        'adding Student Payments
        If eobjpay.Category = "Payment" And eobjpay.SubType = "Student" Then
            estu.BatchCode = eobjpay.BatchCode
            estu.Category = "STA"
            estu.Description = "Sponsor Pocket Amount"
            estu.TransStatus = "Close"

            Try
                listTrcpt = bobj.GetStudentAllocationTrans(estu)
            Catch ex As Exception
                LogError.Log("ChequeManagement", "addPayments", ex.Message)
            End Try
            Dim listTrcptFINAL As New List(Of StudentEn)
            Dim loit As New StudentEn
            For Each loit In listTrcpt
                If Not loit.TransactionAmount = "0.00" Then
                    listTrcptFINAL.Add(loit)
                End If
            Next
            dgView.DataSource = listTrcptFINAL
            dgView.DataBind()
            Session("ChePayments") = listTrcptFINAL
            'adding Sponsor Payments
        ElseIf eobjpay.Category = "Payment" And eobjpay.SubType = "Sponsor" Then
            loen.BatchCode = eobjpay.BatchCode
            loen.Category = "Payment"
            loen.SubType = "Sponsor"
            loen.PostStatus = "Posted"

            Try
                listAccounts = bobj.GetTransactions(loen)
            Catch ex As Exception
                LogError.Log("ChequeManagement", "addPayments", ex.Message)
            End Try
            dgsponsor.DataSource = listAccounts
            dgsponsor.DataBind()
            Session("ChePayments") = listAccounts
            'adding Student Refunds
        ElseIf eobjpay.Category = "Refund" Then
            txtAllCode.Text = eobjpay.BatchCode
            Dim Trcptobj As New AccountsBAL
            estu.MatricNo = eobjpay.CreditRef
            estu.BatchCode = eobjpay.BatchCode
            Try
                listTrcpt = Trcptobj.GetStudentReceiptsbyBatchID(estu)
            Catch ex As Exception
                LogError.Log("ChequeManagement", "addPayments", ex.Message)
            End Try

            dgView.DataSource = listTrcpt
            dgView.DataBind()
            Session("ChePayments") = listTrcpt
        End If

    End Sub
    ''' <summary>
    ''' Method to Save Cheques
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub onSave()
        Dim sg As String = ""
        Dim GBFormat As System.Globalization.CultureInfo
        GBFormat = New System.Globalization.CultureInfo("en-GB")
        Dim eobj As New ChequeEn
        Dim bsobj As New ChequeBAL
        Dim list As New List(Of ChequeEn)
        Dim eobstu As New StudentEn
        lblMsg.Text = ""
        lblMsg.Visible = True

        eobj.ProcessID = Trim(txtBatchid.Text)
        eobj.PaymentNo = Trim(txtAllCode.Text)
        eobj.Description = Trim(txtDescri.Text)

        eobj.TransactionDate = DateTime.ParseExact(CStr(txtPaymentDate.Text), "dd/MM/yyyy", GBFormat)
        eobj.ChequeDate = Trim(txtChequeDate.Text)
        'eobj.TransactionDate = Trim(txtPaymentDate.Text)
        eobj.PrintStatus = "Ready"
        eobj.UpdatedBy = Session("user")
        eobj.UpdatedTime = DateTime.Now

        Dim dgItem1 As DataGridItem
        Dim i As Integer = 0
        Dim j As Integer = 0
        Dim total As Integer = 0
        Dim eoblist As New ChequeDetailsEn
        Dim Number As Integer = 0
        If txtBatchid.Text = "Auto Number" Then eobj.ProcessID = ""

        If Not Session("Cheques") Is Nothing Then
            ListCheques = Session("Cheques")
        Else
            ListCheques = New List(Of ChequeDetailsEn)
            lblMsg.Text = "Add Cheques"
            Exit Sub
        End If
        If Not Session("ChePayments") Is Nothing Then
            If ddlChequefor.SelectedValue = "1" Then
                listRefund = Session("ChePayments")
                eobj.PaymentType = "Payment"
            ElseIf ddlChequefor.SelectedValue = "St" Then
                listRefund = Session("ChePayments")
                eobj.PaymentType = "Refund"
            ElseIf ddlChequefor.SelectedValue = "2" Then
                listaccounts = Session("ChePayments")
                eobj.PaymentType = "Sponsor Payments"
            End If
        Else
            If ddlChequefor.SelectedValue = "1" Then
                listRefund = New List(Of StudentEn)
                eobj.PaymentType = "Payment"
            ElseIf ddlChequefor.SelectedValue = "St" Then
                listRefund = New List(Of StudentEn)
                eobj.PaymentType = "Refund"
            ElseIf ddlChequefor.SelectedValue = "2" Then
                listaccounts = New List(Of AccountsEn)
                eobj.PaymentType = "Sponsor Payments"
            End If
        End If
        Dim loen As New ChequeDetailsEn
        Dim locheacc As AccountsEn
        Dim listcheacc As New List(Of AccountsEn)
        If ddlChequefor.SelectedValue = "1" Or ddlChequefor.SelectedValue = "St" Then
            If ListCheques.Count <> 0 Then
                While i < ListCheques.Count
                    loen = ListCheques(i)
                    loen.Number = -(CInt(loen.ChequeStartNo) - CInt(loen.ChequeEndNo) - 1).ToString()
                    total += loen.Number
                    i = i + 1
                End While
                If total >= listRefund.Count Then
                    While j < ListCheques.Count
                        eoblist = ListCheques(j)
                        Number = CInt(eoblist.ChequeStartNo)
                        For Each dgItem1 In dgView.Items
                            If Number <= CInt(eoblist.ChequeEndNo) Then
                                dgItem1.Cells(10).Text = Number
                                locheacc = New AccountsEn
                                locheacc.ChequeNo = Number
                                locheacc.TransactionCode = dgItem1.Cells(11).Text
                                locheacc.ChequeDate = Trim(txtChequeDate.Text)
                                locheacc.UpdatedTime = DateTime.Now
                                locheacc.UpdatedBy = Session("user")
                                listcheacc.Add(locheacc)
                                Number = Number + 1
                            End If
                        Next
                        j = j + 1
                    End While

                Else
                    lblMsg.Text = "Add More Cheques"
                    Exit Sub
                End If
            End If
        ElseIf ddlChequefor.SelectedValue = "2" Then
            If ListCheques.Count <> 0 Then
                While i < ListCheques.Count
                    loen = ListCheques(i)
                    loen.Number = -(CInt(loen.ChequeStartNo) - CInt(loen.ChequeEndNo) - 1).ToString()
                    total += loen.Number
                    i = i + 1
                End While
                If total >= listaccounts.Count Then
                    While j < ListCheques.Count
                        eoblist = ListCheques(j)
                        Number = CInt(eoblist.ChequeStartNo)
                        For Each dgItem1 In dgsponsor.Items
                            If dgItem1.Cells(7).Text = "" Then
                                If Number <= CInt(eoblist.ChequeEndNo) Then
                                    dgItem1.Cells(7).Text = Number
                                    locheacc = New AccountsEn
                                    locheacc.ChequeNo = Number
                                    locheacc.TransactionCode = dgItem1.Cells(8).Text
                                    locheacc.ChequeDate = Trim(txtChequeDate.Text)
                                    locheacc.UpdatedTime = DateTime.Now
                                    locheacc.UpdatedBy = Session("user")
                                    listcheacc.Add(locheacc)
                                    Number = Number + 1
                                End If
                            End If
                        Next
                        j = j + 1
                    End While
                Else
                    lblMsg.Text = "Add More Cheques"
                    Exit Sub
                End If
            End If
        End If


        eobj.ChequeDetailslist = ListCheques
        eobj.AcccountChques = listcheacc
        lblMsg.Visible = True
        If Session("PageMode") = "Add" Then
            Try

                txtBatchid.Text = bsobj.Insert(eobj)
                txtBatchid.ReadOnly = True
                ErrorDescription = "Record Saved Successfully "
                ibtnStatus.ImageUrl = "images/ready.gif"
                lblStatus.Value = "Ready"
                lblMsg.Text = ErrorDescription

            Catch ex As Exception
                lblMsg.Text = ex.Message.ToString()
                LogError.Log("ChequeManagement", "onSave", ex.Message)
                sg = "no"
            End Try
        ElseIf Session("PageMode") = "Edit" Then
            Try

                eobj.ProcessID = txtBatchid.Text
                txtBatchid.Text = bsobj.Update(eobj)
                txtBatchid.ReadOnly = False
                ErrorDescription = "Record Updated Successfully "
                ibtnStatus.ImageUrl = "images/ready.gif"
                lblStatus.Value = "Ready"
                lblMsg.Text = ErrorDescription

            Catch ex As Exception
                lblMsg.Text = ex.Message.ToString()
                LogError.Log("ChequeManagement", "onSave", ex.Message)
            End Try

        End If
        'setDateFormat()
        If Not sg = "no" Then
            LoadReport(txtBatchid.Text)
        End If
    End Sub
    ''' <summary>
    ''' Method to Post Cheques
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub onPost()
        Dim GBFormat As System.Globalization.CultureInfo
        GBFormat = New System.Globalization.CultureInfo("en-GB")
        Dim eobj As New ChequeEn
        Dim bsobj As New ChequeBAL
        Dim list As New List(Of ChequeEn)
        Dim eobstu As New StudentEn
        lblMsg.Text = ""
        lblMsg.Visible = True

        eobj.ProcessID = Trim(txtBatchid.Text)
        eobj.PaymentNo = Trim(txtAllCode.Text)
        eobj.Description = Trim(txtDescri.Text)

        eobj.TransactionDate = DateTime.ParseExact(CStr(txtPaymentDate.Text), "dd/MM/yyyy", GBFormat)
        eobj.ChequeDate = Trim(txtChequeDate.Text)
        eobj.PrintStatus = "Posted"
        eobj.UpdatedBy = Session("user")
        eobj.UpdatedTime = DateTime.Now

        Dim dgItem1 As DataGridItem
        Dim i As Integer = 0
        Dim j As Integer = 0
        Dim total As Integer = 0
        Dim eoblist As New ChequeDetailsEn
        Dim Number As Integer = 0


        If Not Session("Cheques") Is Nothing Then
            ListCheques = Session("Cheques")
        Else
            ListCheques = New List(Of ChequeDetailsEn)
        End If
        If Not Session("ChePayments") Is Nothing Then
            If ddlChequefor.SelectedValue = "1" Then
                listRefund = Session("ChePayments")
                eobj.PaymentType = "Payment"
            ElseIf ddlChequefor.SelectedValue = "St" Then
                listRefund = Session("ChePayments")
                eobj.PaymentType = "Refund"
            ElseIf ddlChequefor.SelectedValue = "2" Then
                listaccounts = Session("ChePayments")
                eobj.PaymentType = "Sponsor Payments"
            End If
        Else
            If ddlChequefor.SelectedValue = "1" Then
                listRefund = New List(Of StudentEn)
                eobj.PaymentType = "Payment"
            ElseIf ddlChequefor.SelectedValue = "St" Then
                listRefund = New List(Of StudentEn)
                eobj.PaymentType = "Refund"
            ElseIf ddlChequefor.SelectedValue = "2" Then
                listaccounts = New List(Of AccountsEn)
                eobj.PaymentType = "Sponsor Payments"
            End If
        End If
        Dim loen As New ChequeDetailsEn
        Dim locheacc As AccountsEn
        Dim listcheacc As New List(Of AccountsEn)
        If ddlChequefor.SelectedValue = "1" Or ddlChequefor.SelectedValue = "St" Then
            If ListCheques.Count <> 0 Then
                While i < ListCheques.Count
                    loen = ListCheques(i)
                    loen.Number = -(CInt(loen.ChequeStartNo) - CInt(loen.ChequeEndNo) - 1).ToString()
                    total += loen.Number
                    i = i + 1
                End While
                If total >= listRefund.Count Then
                    While j < ListCheques.Count
                        eoblist = ListCheques(j)
                        Number = CInt(eoblist.ChequeStartNo)
                        For Each dgItem1 In dgView.Items
                            If Number <= CInt(eoblist.ChequeEndNo) Then
                                dgItem1.Cells(10).Text = Number
                                locheacc = New AccountsEn
                                locheacc.ChequeNo = Number
                                locheacc.TransactionCode = dgItem1.Cells(11).Text
                                locheacc.ChequeDate = Trim(txtChequeDate.Text)
                                locheacc.UpdatedTime = DateTime.Now
                                locheacc.UpdatedBy = Session("user")
                                listcheacc.Add(locheacc)
                                Number = Number + 1
                            End If
                        Next
                        j = j + 1
                    End While

                Else
                    lblMsg.Text = "Add More Cheques"
                    Exit Sub
                End If
            End If
        ElseIf ddlChequefor.SelectedValue = "2" Then
            If ListCheques.Count <> 0 Then
                While i < ListCheques.Count
                    total += ListCheques(i).Number
                    i = i + 1
                End While
                If total >= listaccounts.Count Then
                    While j < ListCheques.Count
                        eoblist = ListCheques(j)
                        Number = CInt(eoblist.ChequeStartNo)
                        For Each dgItem1 In dgsponsor.Items
                            If dgItem1.Cells(7).Text = "" Then
                                If Number <= CInt(eoblist.ChequeEndNo) Then
                                    dgItem1.Cells(7).Text = Number
                                    locheacc = New AccountsEn
                                    locheacc.ChequeNo = Number
                                    locheacc.TransactionCode = dgItem1.Cells(8).Text
                                    locheacc.ChequeDate = Trim(txtChequeDate.Text)
                                    locheacc.UpdatedTime = DateTime.Now
                                    locheacc.UpdatedBy = Session("user")
                                    listcheacc.Add(locheacc)
                                    Number = Number + 1
                                End If
                            End If
                        Next
                        j = j + 1
                    End While
                Else
                    lblMsg.Text = "Add More Cheques"
                    Exit Sub
                End If
            End If
        End If
        eobj.ChequeDetailslist = ListCheques
        eobj.AcccountChques = listcheacc
        lblMsg.Visible = True

        Try

            eobj.ProcessID = txtBatchid.Text
            txtBatchid.Text = bsobj.Update(eobj)
            txtBatchid.ReadOnly = True
            ibtnStatus.ImageUrl = "images/posted.gif"
            lblMsg.Visible = True
            ErrorDescription = "Record Posted Successfully"
            lblMsg.Text = ErrorDescription
            lblStatus.Value = "Posted"
            eobj.PostStatus = "Posted"

        Catch ex As Exception
            lblMsg.Text = ex.Message.ToString()
            LogError.Log("ChequeManagement", "onPost", ex.Message)
        End Try

        LoadReport(txtBatchid.Text)
        'setDateFormat()
    End Sub
    Private Sub LoadReport(ByVal batch As String)
        Dim scriptstringOpen As String = "OpenWindow();"
        ClientScript.RegisterStartupScript(Me.GetType(), "OpenWindow", scriptstringOpen, True)
    End Sub
    ''' <summary>
    ''' Method to Load the UserRights
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub LoadUserRights()
        Dim obj As New UsersBAL
        Dim eobj As UserRightsEn

        Try
            eobj = obj.GetUserRights(CInt(Request.QueryString("Menuid")), CInt(Session("UserGroup")))
        Catch ex As Exception
            LogError.Log("ChequeManagement", "LoadUserRights", ex.Message)
        End Try

        'Rights for Add

        If eobj.IsAdd = True Then
            'ibtnSave.Enabled = True
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
            ibtnPrint.ImageUrl = "images/gprint.gif"
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
    ''' Method to Validate
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub SpaceValidation()
        ibtnSave.Attributes.Add("onCLick", "return Validate()")
        Dim GBFormat As System.Globalization.CultureInfo
        GBFormat = New System.Globalization.CultureInfo("en-GB")

        'Cheque date
        If Trim(txtChequeDate.Text).Length < 10 Then
            lblMsg.Text = "Enter Valid Cheque Date"
            lblMsg.Visible = True
            txtBDate.Focus()
            Exit Sub
        Else
            Try
                txtChequeDate.Text = DateTime.Parse(txtChequeDate.Text, GBFormat)
            Catch ex As Exception
                lblMsg.Text = "Enter Valid Cheque Date"
                lblMsg.Visible = True
                txtChequeDate.Focus()
                Exit Sub
            End Try
        End If

        If lblStatus.Value = "Posted" Then
            lblMsg.Text = "Record Already Posted"
            lblMsg.Visible = True
            Exit Sub
        End If


    End Sub
    Private Sub check_Paymenttfor()
        If ddlChequefor.SelectedValue = "1" Then
            ibtnRef.Visible = False
            ibtnAll.Visible = True
            ibtnSpPay.Visible = False
            Session("Chequefor") = ddlChequefor.SelectedValue
        ElseIf ddlChequefor.SelectedValue = "St" Then

            ibtnAll.Visible = False
            ibtnRef.Visible = True
            ibtnSpPay.Visible = False
            Session("Chequefor") = ddlChequefor.SelectedValue
        ElseIf ddlChequefor.SelectedValue = "2" Then
            ibtnAll.Visible = False
            ibtnRef.Visible = False
            ibtnSpPay.Visible = True
            Session("Chequefor") = ddlChequefor.SelectedValue
        Else
            ibtnAll.Visible = False
            ibtnRef.Visible = False
            ibtnSpPay.Visible = False
        End If
    End Sub
    ''' <summary>
    ''' Method to Load Fields in New Mode
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub onAdd()
        Session("ListObj") = Nothing
        Session("stu") = Nothing
        check_Paymenttfor()
        OnClearData()
        If ibtnNew.Enabled = False Then
            ibtnSave.Enabled = False
            ibtnSave.ImageUrl = "images/gsave.png"
            ibtnSave.ToolTip = "Access Denied"
        End If
        If Session("PageMode") = "Add" Then
            txtBatchid.ReadOnly = False
            txtBatchid.Text = "Auto Number"
            txtBatchid.ReadOnly = True
        End If
        Today.Value = Now.Date
        Today.Value = Format(CDate(Today.Value), "dd/MM/yyyy")
    End Sub
    ''' <summary>
    ''' Method to Clear the Field Values
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub OnClearData()
        ibtnStatus.ImageUrl = "images/notready.gif"
        lblStatus.Value = "New"
        Session("ListObj") = Nothing
        Session("ReceiptFor") = Nothing
        txtBatchid.Text = "Auto Number"
        DisableRecordNavigator()
        txtAllCode.Text = ""
        txtCEND.Text = ""
        txtCStart.Text = ""
        lblMsg.Text = ""
        txtBankCode.Text = ""
        ddlChequefor.SelectedValue = "-1"
        txtDescri.Text = ""
        txtBDate.Text = ""
        txtPaymentDate.Text = ""
        dgView.DataSource = Nothing
        dgView.DataBind()
        dgCheque.DataSource = Nothing
        dgCheque.DataBind()
        dgsponsor.DataSource = Nothing
        dgsponsor.DataBind()
        Session("PageMode") = "Add"
        Session("PayforCheque") = Nothing
        Session("Cheques") = Nothing
        Session("ChePayments") = Nothing
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
    ''' Method to add Cheque Numbers
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub addCheques()
        ChequeNo.Attributes.Add("OnClick", "return Chequealidate()")
        lblMsg.Text = ""
        Dim loen As ChequeDetailsEn
        Dim list As New List(Of ChequeDetailsEn)
        Dim eobj As New ChequeDetailsEn
        Dim i As Integer = 0
        'If Not txtCStart.Text = "" & txtCEND.Text = "" Then
        If Not Session("Cheques") Is Nothing Then
            ListCheques = Session("Cheques")
        Else
            ListCheques = New List(Of ChequeDetailsEn)
        End If
        loen = New ChequeDetailsEn
        loen.ChequeStartNo = Trim(txtCStart.Text)
        loen.ChequeEndNo = Trim(txtCEND.Text)
        If (Convert.ToInt64(txtCEND.Text) < Convert.ToInt64(txtCStart.Text)) Then
            lblMsg.Text = "End Number Cannot be Lesser Than Start Number"
            Exit Sub
        End If
        loen.Number = (Convert.ToInt64(txtCEND.Text) - Convert.ToInt64(txtCStart.Text) - 1).ToString()
        list.Add(loen)
        'Checking for the Exisiting Students in the Grid
        If list.Count <> 0 Then
            While i < list.Count
                eobj = list(i)
                Dim j As Integer = 0
                Dim Flag As Boolean = False
                While j < ListCheques.Count
                    If ListCheques(j).ChequeStartNo = eobj.ChequeStartNo Then
                        Flag = True
                        Exit While
                    End If
                    j = j + 1
                End While
                If Flag = False Then
                    ListCheques.Add(eobj)
                End If
                i = i + 1
            End While
        End If
        dgCheque.DataSource = ListCheques
        dgCheque.DataBind()
        Dim dgcheitem As DataGridItem
        For Each dgcheitem In dgCheque.Items
            Dim link As LinkButton
            link = dgcheitem.Cells(0).Controls(0)
            dgcheitem.Cells(2).Text = (dgcheitem.Cells(1).Text - link.Text + 1)
        Next
        Session("Cheques") = ListCheques
    End Sub

    ''' <summary>
    ''' Method to get the List Of Transactions
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub LoadListObjects()
        Dim eob As New ChequeEn
        Dim bobj As New ChequeBAL
        If txtBatchid.Text <> "Auto Number" Then
            eob.ProcessID = txtBatchid.Text
        Else
            eob.ProcessID = ""
        End If

        If Session("loaddata") = "View" Then
            If ddlChequefor.SelectedValue = "1" Then
                eob.PaymentType = "Payment"
                eob.PrintStatus = "Ready"
                Try
                    ListObjects = bobj.GetList(eob)
                Catch ex As Exception
                    LogError.Log("ChequeManagement", "LoadListObjects", ex.Message)
                End Try
            ElseIf ddlChequefor.SelectedValue = "St" Then
                eob.PaymentType = "Refund"
                eob.PrintStatus = "Ready"
                Try
                    ListObjects = bobj.GetRefundList(eob)
                Catch ex As Exception
                    LogError.Log("ChequeManagement", "LoadListObjects", ex.Message)
                End Try
            ElseIf ddlChequefor.SelectedValue = "2" Then
                eob.PaymentType = "Sponsor Payments"
                eob.PrintStatus = "Ready"
                Try
                    ListObjects = bobj.GetList(eob)
                Catch ex As Exception
                    LogError.Log("ChequeManagement", "LoadListObjects", ex.Message)
                End Try
            End If

        ElseIf Session("loaddata") = "others" Then

            If ddlChequefor.SelectedValue = "1" Then
                eob.PaymentType = "Payment"
                eob.PrintStatus = "Posted"
                Try
                    ListObjects = bobj.GetList(eob)
                Catch ex As Exception
                    LogError.Log("ChequeManagement", "LoadListObjects", ex.Message)
                End Try
            ElseIf ddlChequefor.SelectedValue = "St" Then
                eob.PaymentType = "Refund"
                eob.PrintStatus = "Posted"
                Try
                    ListObjects = bobj.GetRefundList(eob)
                Catch ex As Exception
                    LogError.Log("ChequeManagement", "LoadListObjects", ex.Message)
                End Try
            ElseIf ddlChequefor.SelectedValue = "2" Then
                eob.PaymentType = "Sponsor Payments"
                eob.PrintStatus = "Posted"
                Try
                    ListObjects = bobj.GetList(eob)
                Catch ex As Exception
                    LogError.Log("ChequeManagement", "LoadListObjects", ex.Message)
                End Try
            End If

        End If

        Session("ListObj") = ListObjects
        lblCount.Text = ListObjects.Count.ToString()
        If ListObjects.Count <> 0 Then
            DisableRecordNavigator()
            txtRecNo.Text = "1"

            OnMoveFirst()
            If Session("EditFlag") = True Then
                Session("PageMode") = "Edit"
                txtBatchid.Enabled = False
            Else
                Session("PageMode") = ""
                ibtnSave.Enabled = False
                ibtnSave.ImageUrl = "images/gsave.png"
            End If
        Else
            txtRecNo.Text = ""
            lblCount.Text = ""
            ibtnStatus.ImageUrl = "images/notready.gif"
            lblStatus.Value = "New"
            Session("ListObj") = Nothing
            txtBatchid.Text = "Auto Number"
            DisableRecordNavigator()
            txtAllCode.Text = ""
            txtDescri.Text = ""
            dates()
            dgView.DataSource = Nothing
            dgView.DataBind()
            Session("PageMode") = "Add"
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

                Dim eobj As ChequeEn
                ListObjects = Session("ListObj")
                eobj = ListObjects(RecNo)

                txtAllCode.Text = eobj.PaymentNo
                txtBankCode.Text = eobj.BankCode


                If eobj.PaymentType = "Payment" Then
                    ddlChequefor.SelectedValue = "1"
                ElseIf eobj.PaymentType = "Refund" Then
                    ddlChequefor.SelectedValue = "St"
                ElseIf eobj.PaymentType = "Sponsor Payment" Then
                    ddlChequefor.SelectedValue = "2"
                End If
                check_Paymenttfor()
                txtDescri.Text = eobj.Description
                txtBatchid.Text = eobj.ProcessID
                txtBDate.Text = eobj.TransactionDate
                txtPaymentDate.Text = eobj.TransactionDate
                txtChequeDate.Text = eobj.ChequeDate
                Dim lochedetails As New List(Of ChequeDetailsEn)
                lochedetails = eobj.ChequeDetailslist
                dgCheque.DataSource = lochedetails
                dgCheque.DataBind()
                Dim dgcheitem As DataGridItem
                For Each dgcheitem In dgCheque.Items
                    Dim link As LinkButton
                    link = dgcheitem.Cells(0).Controls(0)
                    dgcheitem.Cells(2).Text = (dgcheitem.Cells(1).Text - link.Text + 1)
                Next
                Session("Cheques") = lochedetails
                If eobj.PrintStatus = "Ready" Then
                    lblStatus.Value = "Ready"
                    ibtnStatus.ImageUrl = "images/Ready.gif"
                End If
                If eobj.PrintStatus = "Posted" Then
                    lblStatus.Value = "Posted"
                    ibtnStatus.ImageUrl = "images/Posted.gif"
                End If

                Dim estu As New StudentEn
                Dim loen As New AccountsEn
                Dim listaccounts As New List(Of AccountsEn)
                Dim listTrcpt As New List(Of StudentEn)
                Dim Trcptobj As New AccountsBAL

                If ddlChequefor.SelectedValue = "1" Then
                    estu.BatchCode = eobj.BatchCode
                    estu.Category = "STA"
                    estu.Description = "Sponsor Pocket Amount"
                    estu.TransStatus = "Posted"
                    Try
                        listTrcpt = Trcptobj.GetStudentAllocationTrans(estu)
                    Catch ex As Exception
                        LogError.Log("ChequeManagement", "FillData", ex.Message)
                    End Try
                    dgView.DataSource = listTrcpt
                    dgView.DataBind()

                    Dim dgItem1 As DataGridItem
                    Dim j As Integer = 0
                    Dim Vno As TextBox
                    Dim amt As Double = 0.0
                    While j < listTrcpt.Count
                        For Each dgItem1 In dgView.Items
                            If dgItem1.Cells(1).Text = listTrcpt(j).MatricNo Then
                                Vno = dgItem1.Cells(8).Controls(1)
                                Vno.Text = listTrcpt(j).VoucherNo
                                Exit For
                            End If
                        Next
                        j = j + 1
                    End While
                ElseIf ddlChequefor.SelectedValue = "2" Then
                    loen.BatchCode = eobj.BatchCode
                    loen.Category = "Payment"
                    loen.SubType = "Sponsor"
                    loen.PostStatus = "Posted"
                    Try
                        listaccounts = Trcptobj.GetTransactions(loen)
                    Catch ex As Exception
                        LogError.Log("ChequeManagement", "FillData", ex.Message)
                    End Try
                    dgsponsor.DataSource = listaccounts
                    dgsponsor.DataBind()
                ElseIf ddlChequefor.SelectedValue = "St" Then
                    Dim Vouno As TextBox

                    estu.BatchCode = eobj.BatchCode

                    Try
                        listTrcpt = Trcptobj.GetStudentReceiptsbyBatchID(estu)
                    Catch ex As Exception
                        LogError.Log("ChequeManagement", "FillData", ex.Message)
                    End Try
                    If listTrcpt Is Nothing Then
                    Else
                        dgView.DataSource = listTrcpt
                        dgView.DataBind()
                        Session("ChePayments") = listTrcpt
                        Dim dgItem1 As DataGridItem
                        Dim amt As Double = 0.0
                        Dim k As Integer = 0
                        While k < listTrcpt.Count
                            For Each dgItem1 In dgView.Items
                                If dgItem1.Cells(1).Text = listTrcpt(k).MatricNo Then
                                    Vouno = dgItem1.Cells(8).Controls(1)
                                    Vouno.Text = listTrcpt(k).VoucherNo
                                End If
                            Next
                            k = k + 1
                        End While
                    End If
                End If
            End If
        End If
        setDateFormat()
    End Sub
    ''' <summary>
    ''' Method to get cheques
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub GetCheques()

    End Sub
    ''' <summary>
    ''' Method to Search for Posted Records
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub OnSearchOthers()
        Session("loaddata") = "others"
        If lblCount.Text <> "" Then
            If CInt(lblCount.Text) > 0 Then
                onAdd()
            Else
                Session("PageMode") = "Edit"
                LoadListObjects()

            End If
        Else
            Session("PageMode") = "Edit"
            LoadListObjects()

            PostEnFalse()
        End If

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
        ibtnPrint.Enabled = True
        ibtnPrint.ImageUrl = "images/print.png"
        ibtnPrint.ToolTip = "Print"
        ibtnPosting.Enabled = False
        ibtnPosting.ImageUrl = "images/gposting.png"
        ibtnPosting.ToolTip = "Access denied"
        'ibtnOthers.Enabled = False
        'ibtnOthers.ImageUrl = "images/post.png"
        'ibtnOthers.ToolTip = "Access denied"
    End Sub
    ''' <summary>
    ''' Method to Delete the Transactions
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub ondelete()
        Dim RecAff As Boolean
        Dim eob As New ChequeEn
        Dim bsobj As New ChequeBAL
        Dim str As String
        If lblStatus.Value = "Ready" Then
            Try
                eob.ProcessID = Trim(txtBatchid.Text)
                RecAff = bsobj.BatchDelete(eob)
                str = ddlChequefor.SelectedValue
                onAdd()
                DFlag = "Delete"
                Session("loaddata") = "View"
                lblMsg.Text = "Record Deleted Successfully "
                lblMsg.Visible = True
                ddlChequefor.SelectedValue = str
                LoadListObjects()
                'Session("ListObj") = ListObjects
            Catch ex As Exception
                lblMsg.Text = ex.Message.ToString()
                LogError.Log("ChequeManagement", "ondelete", ex.Message)
            End Try
        End If
        lblMsg.Visible = True
    End Sub
#End Region

    Protected Sub ChequeNo_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ChequeNo.Click
        'ChequeNo.Attributes.Add("OnClick", "return Chequealidate()")
        addCheques()
    End Sub

    Protected Sub dgCheque_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgCheque.SelectedIndexChanged
        If dgCheque.SelectedIndex <> -1 Then
            Dim eobj As New List(Of ChequeDetailsEn)
            Dim eobj1 As New ChequeDetailsEn
            Dim dgitem As DataGridItem
            Dim link As LinkButton
            dgitem = dgCheque.SelectedItem
            link = dgitem.Cells(0).Controls(0)
            Dim i As Integer = 0
            txtCStart.Text = link.Text
            txtCEND.Text = dgitem.Cells(1).Text
        End If
    End Sub

    Protected Sub ibtnSave_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ibtnSave.Click
        SpaceValidation()
        onSave()
        datestring = "yes"
        setDateFormat()
    End Sub


    Protected Sub Remove_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Remove.Click
        Dim lolist As List(Of ChequeDetailsEn)
        If dgCheque.SelectedIndex <> -1 Then
            Dim dgitem As DataGridItem
            Dim i As Integer = 0
            Dim k As Integer = 0
            For Each dgitem In dgCheque.Items
                dgitem.Cells(0).Text = i
                i = i + 1
            Next
            lolist = Session("Cheques")
            If Not lolist Is Nothing Then
                If dgCheque.SelectedIndex <> -1 Then
                    lolist.RemoveAt(CInt(dgCheque.SelectedItem.Cells(0).Text))
                    dgCheque.DataSource = lolist
                    dgCheque.DataBind()
                    Dim dgcheitem As DataGridItem
                    For Each dgcheitem In dgCheque.Items
                        Dim link As LinkButton
                        link = dgcheitem.Cells(0).Controls(0)
                        dgcheitem.Cells(2).Text = (dgcheitem.Cells(1).Text - link.Text + 1)
                    Next
                    If lolist.Count <> 0 Then
                        Session("Cheques") = lolist
                        txtCStart.Text = ""
                        txtCEND.Text = ""
                        lblMsg.Text = ""
                    Else
                        txtCStart.Text = ""
                        txtCEND.Text = ""
                        Session("Cheques") = Nothing
                        lblMsg.Text = ""
                    End If
                    dgCheque.SelectedIndex = -1
                End If
            End If
        Else
            lblMsg.Text = "Select A Cheque to Remove"
            lblMsg.Visible = True
        End If
    End Sub

    Protected Sub ibtnView_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ibtnView.Click

        Session("loaddata") = "View"
        If lblCount.Text <> "" Then
            If CInt(lblCount.Text) > 0 Then
                onAdd()
            Else

                Session("PageMode") = "Edit"
                LoadListObjects()
            End If
        Else
            Session("PageMode") = "Edit"
            LoadListObjects()

        End If
        If lblCount.Text.Length = 0 Then
            Session("PageMode") = "Add"
        End If
    End Sub

    Protected Sub ibtnNext_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ibtnNext.Click
        OnMoveNext()
    End Sub

    Protected Sub ibtnLast_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ibtnLast.Click
        OnMoveLast()
    End Sub

    Protected Sub ibtnFirst_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ibtnFirst.Click
        OnMoveFirst()
    End Sub

    Protected Sub ibtnPrevs_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ibtnPrevs.Click
        OnMovePrevious()
    End Sub

    Protected Sub ibtnCancel_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ibtnCancel.Click
        LoadUserRights()
        onAdd()
    End Sub

    Protected Sub ibtnPosting_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ibtnPosting.Click
        If lblStatus.Value = "New" Then
            lblMsg.Text = "Record not Ready for Posting"
            lblMsg.Visible = True
        ElseIf lblStatus.Value = "Posted" Then
            lblMsg.Text = "Record Already Posted"
            lblMsg.Visible = True
        ElseIf lblStatus.Value = "Ready" Then
            SpaceValidation()
            onPost()
            datestring = "yes"
            setDateFormat()
        End If
    End Sub

    Protected Sub ibtnOthers_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ibtnOthers.Click
        OnSearchOthers()
    End Sub


    Protected Sub ibtnDelete_Click1(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ibtnDelete.Click
        ondelete()
    End Sub

    Protected Sub ibtnNew_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ibtnNew.Click
        onAdd()
    End Sub


    Protected Sub ibtnPrint_Click1(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ibtnPrint.Click
        LoadReport(txtBatchid.Text)
    End Sub

    Protected Sub ddlChequefor_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlChequefor.SelectedIndexChanged
        check_Paymenttfor()
        Dim str As String = ddlChequefor.SelectedValue
        OnClearData()
        ddlChequefor.SelectedValue = str
    End Sub
    Protected Sub btnHidden_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnHidden.Click
     
    End Sub
End Class
