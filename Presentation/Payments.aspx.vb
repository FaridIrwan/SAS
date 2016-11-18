Imports HTS.SAS.Entities
Imports HTS.SAS.BusinessObjects
Imports HTS.SAS.DataAccessObjects
Imports System.Data
Imports System.Collections.Generic
Imports System.Data.SqlClient
Imports System.Linq

Partial Class Payments
    Inherits System.Web.UI.Page
#Region "Global Declarations "
    'Global Declaration - Starting
    Private _Helper As New Helper
    Private ListTRD As New List(Of AccountsDetailsEn)
    Private ListObjects As List(Of AccountsEn)
    Dim listStu As New List(Of StudentEn)
    Private ErrorDescription As String
    Dim AutoNo As Boolean
    Dim DFlag As String
    Dim eflag As String
    Private ListObjectsStudent As List(Of StudentEn)

    Dim objIntegrationDL As New SQLPowerQueryManager.PowerQueryManager.IntegrationDL
    Dim objIntegration As New IntegrationModule.IntegrationNameSpace.Integration
    Dim dsReturn As New DataSet
    'Global Declaration - Ended
#End Region

#Region "Done By "

    Public Function DoneBy() As Integer

        Return MaxGeneric.clsGeneric.
            NullToInteger(Session(Helper.UserSession))

    End Function

#End Region

    Protected Sub Page_AbortTransaction(sender As Object, e As EventArgs) Handles Me.AbortTransaction

    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack() Then
            'Adding validation for save,Delete,search & others buttons
            upload.Visible = False
            lblUpload.Visible = False
            btnUpload.Visible = False
            ibtnSave.Attributes.Add("onclick", "return Validate()")
            ibtnDelete.Attributes.Add("onclick", "return getconfirm()")
            ibtnView.Attributes.Add("onclick", "return CheckSearch()")
            ibtnOthers.Attributes.Add("onclick", "return CheckSearch()")
            ibtnPrint.Attributes.Add("onclick", "return getPrint()")
            'Loading User Rights
            LoadUserRights()
            Session("PageMode") = "Add"
            Session("Menuid") = Request.QueryString("Menuid")
            Session("AddBank") = Nothing
            txtBDate.Attributes.Add("OnKeyup", "return CheckBDate()")
            txtPaymentDate.Attributes.Add("OnKeyup", "return CheckTdate()")
            'While loading the page make the CFlag as null
            txtRecNo.Attributes.Add("OnKeyup", "return geterr()")

            'while loading list object make it nothing
            Session("ListObj") = Nothing
            Session("liststu") = Nothing
            Session("stu") = Nothing
            DisableRecordNavigator()
            'load PageName
            Menuname(CInt(Request.QueryString("Menuid")))
            MenuId.Value = GetMenuId()
            lblStatus.Value = "New"
            ibtnDelete.Attributes.Add("onClick", "return getconfirm()")
            ibtnPosting.Attributes.Add("onClick", "return getpostconfirm()")
            ibtnBDate.Attributes.Add("onClick", "return BDate()")
            dates()
            ibtnPaymentDate.Attributes.Add("onClick", "return getpaymentDate()")
            lblMsg.Text = ""
            btnUpload.Attributes.Add("onclick", "new_window=window.open('File.aspx','Hanodale','width=470,height=380,resizable=0');new_window.focus();")
            ibtnSpn1.Attributes.Add("onclick", "new_window=window.open('AddSpnAll.aspx','Hanodale','width=520,height=450,resizable=0');new_window.focus();")
            ibtnstu.Attributes.Add("onclick", "new_window=window.open('AddMulStudents.aspx?cat=St','Hanodale','width=550,height=550,resizable=0');new_window.focus();")
            'ibtnPosting.Attributes.Add("onclick", "new_window=window.open('AddApprover.aspx?MenuId=" & GetMenuId() & "','Hanodale','width=500,height=400,resizable=0');new_window.focus();")
            addPayMode()
            addBankCode()
            Session("statusPrint") = Nothing
            Label48.Visible = False
        End If

        If Not Session("SpnAlleobj") Is Nothing Then
            addallcode()
        End If
        If Not Session("liststu") Is Nothing Then
            addStuCode()
        End If

        lblMsg.Visible = False

        If Not Session("CheckApproverList") Is Nothing Then
            SendToApproval()
        End If

        Try
            If Not Session("fileSponsor") Is Nothing And Session("fileType") = "excel" Then
                Dim importobj As New ImportData
                ListObjectsStudent = importobj.GetImportedSponsorData(Session("fileSponsor").ToString())
                Session("liststu") = Nothing
                Session("liststu") = ListObjectsStudent
                addStuCode()
                Session("fileType") = Nothing
            End If
        Catch ex As Exception
            lblMsg.Text = ex.Message
        End Try

        If Not Request.QueryString("BatchCode") Is Nothing Then
            Dim str As String = Request.QueryString("BatchCode")
            Dim constr As String() = str.Split(";")
            txtBatchid.Text = constr(0)
            If constr(1) = "St" Then
                ddlpaymentfor.SelectedValue = "St"
            ElseIf constr(1) = "A" Then
                ddlpaymentfor.SelectedValue = "1"
            End If
            DirectCast(Master.FindControl("Panel1"), System.Web.UI.WebControls.Panel).Visible = False
            DirectCast(Master.FindControl("td"), System.Web.UI.HtmlControls.HtmlTableCell).Visible = False
            Panel1.Visible = False
            'OnSearchOthers()
            If CInt(Request.QueryString("IsStudentLedger")).Equals(1) Then
                OnSearchOthers()
            End If
            If CInt(Request.QueryString("IsView")).Equals(1) Then
                OnSearchView()
            End If

            txtBDate.ReadOnly = True
            txtPaymentDate.ReadOnly = True
        End If
    End Sub
    'Protected Overloads Sub Page_LoadComplete(ByVal sender As Object, ByVal e As EventArgs) Handles Me.LoadComplete
    '    ddlPaymentMode.SelectedIndex = 1
    'End Sub

#Region "Methods"
    ''' <summary>
    ''' Method to Change the Date Format
    ''' </summary>
    ''' <remarks>Date in ddd/mm/yyyy Format</remarks>
    Private Sub dates()

        txtPaymentDate.Text = Format(Date.Now, "dd/MM/yyyy")
        txtBDate.Text = Format(Date.Now, "dd/MM/yyyy")
    End Sub
    ''' <summary>
    ''' Method to Add Bankcode to Dropdown
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
                LogError.Log("Payments", "addBankCode", ex.Message)
            End Try
        Else
            Try
                list = bsobj.GetBankProfileListAll(eobjF)
            Catch ex As Exception
                LogError.Log("Payments", "addBankCode", ex.Message)
            End Try
        End If
        Session("bankcode") = list
        ddlBankCode.DataSource = list
        ddlBankCode.DataBind()

    End Sub
    ''' <summary>
    ''' Method to Validate
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub SpaceValidation()
        ibtnSave.Attributes.Add("onclick", "return Validate()")
        Dim GBFormat As System.Globalization.CultureInfo
        GBFormat = New System.Globalization.CultureInfo("en-GB")



        'Receipt For
        If ddlpaymentfor.SelectedValue = "-1" Then
            lblMsg.Text = "Select a Receipt For"
            lblMsg.Visible = True
            ddlpaymentfor.Focus()
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

        'Description
        If Trim(txtDescri.Text).Length = 0 Then
            txtDescri.Text = Trim(txtDescri.Text)
            lblMsg.Text = "Enter Valid Description "
            lblMsg.Visible = True
            txtDescri.Focus()
            Exit Sub
        End If

        If ddlpaymentfor.SelectedValue = "1" Then

            'AllocationCode
            If Trim(txtAllCode.Text).Length = 0 Then
                txtAllCode.Text = Trim(txtAllCode.Text)
                lblMsg.Text = "Allocation Sl No Field Cannot Be Blank "
                lblMsg.Visible = True
                txtAllCode.Focus()
                Exit Sub
            End If

            'Reference Code
            If Trim(txtRef1.Text).Length = 0 Then
                txtRef1.Text = Trim(txtRef1.Text)
                lblMsg.Text = "Receipt No Field Cannot Be Blank "
                lblMsg.Visible = True
                txtRef1.Focus()
                Exit Sub
            End If

        End If
        'Batch date
        If Trim(txtPaymentDate.Text).Length < 10 Then
            lblMsg.Text = "Enter Valid Payment Date"
            lblMsg.Visible = True
            txtPaymentDate.Focus()
            Exit Sub
        Else
            Try
                txtPaymentDate.Text = DateTime.Parse(txtPaymentDate.Text, GBFormat)
            Catch ex As Exception
                lblMsg.Text = "Enter Valid Payment Date"
                lblMsg.Visible = True
                txtPaymentDate.Focus()
                Exit Sub
            End Try
        End If
        'Invoice date
        If Trim(txtBDate.Text).Length < 10 Then
            lblMsg.Text = "Enter Valid Invoice Date"
            lblMsg.Visible = True
            txtBDate.Focus()
            Exit Sub
        Else
            Try
                txtBDate.Text = DateTime.Parse(txtBDate.Text, GBFormat)
            Catch ex As Exception
                lblMsg.Text = "Enter Valid Batch Date"
                lblMsg.Visible = True
                txtBDate.Focus()
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
            End Try
        End If
        Session("paymode") = list
        ddlPaymentMode.Items.Clear()
        ddlPaymentMode.Items.Add(New ListItem("--Select--", "-1"))
        ddlPaymentMode.DataTextField = "SAPM_Des"
        ddlPaymentMode.DataValueField = "SAPM_Code"
        ddlPaymentMode.DataSource = list
        ddlPaymentMode.DataBind()

    End Sub
    ''' <summary>
    ''' Method to Load Students to Grid
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub addStuCode()
        Dim dsobj As New StudentBAL
        Dim eobjf As New StudentEn
        Dim stu As New StudentEn
        Dim stuNew As New StudentEn
        Dim listestudent As New List(Of StudentEn)
        Dim liststudentView As New List(Of StudentEn)
        '  RrcType = Session("Type")
        'If RrcType = True Then
        Dim i As Integer = 0

        listestudent = Session("liststu")
        If Not Session("stu") Is Nothing Then
            liststudentView = Session("stu")
        Else
            liststudentView = New List(Of StudentEn)
        End If
        If listestudent.Count <> 0 Then
            While i < listestudent.Count
                stu = listestudent(i)
                Dim k As Integer = 0
                Dim Flag As Boolean = False
                If k = 0 Then
                    stuNew = dsobj.GetItem(stu.MatricNo)
                    stu.StudentName = stuNew.StudentName
                    stu.ProgramID = stuNew.ProgramID
                    stu.CurrentSemester = stuNew.CurrentSemester
                End If
                While k < liststudentView.Count
                    stuNew = dsobj.GetItem(stu.MatricNo)
                    stu.StudentName = stuNew.StudentName
                    stu.ProgramID = stuNew.ProgramID
                    stu.CurrentSemester = stuNew.CurrentSemester
                    If liststudentView(k).MatricNo = stu.MatricNo Then
                        Flag = True
                        Exit While
                    End If
                    k = k + 1
                End While
                If Flag = False Then
                    liststudentView.Add(stu)
                End If
                i = i + 1
            End While
        End If

        If liststudentView Is Nothing Then
            dgView.DataSource = Nothing
            dgView.DataBind()
        Else
            Dim dgItem1 As DataGridItem
            Dim totalamt As Double = 0.0
            Dim allamt As TextBox
            Dim j As Integer = 0
            Dim bsstu As New AccountsBAL
            Dim outamt As Double = 0.0
            Dim alloamt As Double = 0.0
            Dim stuen As New StudentEn
            Dim chk As CheckBox
            Dim pocketAmount As TextBox
            'dgView.PageSize = mylst.Count
            dgView.DataSource = liststudentView
            Session("stu") = liststudentView
            dgView.DataBind()
            dgView.Columns(5).Visible = True
            dgView.Columns(7).Visible = False
            dgView.Columns(6).Visible = True
            dgView.Columns(0).Visible = True
            While j < liststudentView.Count
                For Each dgItem1 In dgView.Items
                    If dgItem1.Cells(1).Text = liststudentView(j).MatricNo Then
                        chk = dgItem1.Cells(0).Controls(1)
                        chk.Checked = True
                        allamt = dgItem1.Cells(6).Controls(1)
                        allamt.Text = String.Format("{0:F}", alloamt)
                        pocketAmount = dgItem1.Cells(9).Controls(1)
                        pocketAmount.Text = String.Format("{0:F}", liststudentView(j).TempAmount)
                        chk.Checked = True
                        stuen.MatricNo = dgItem1.Cells(1).Text
                        Try
                            'outamt = bsstu.GetStudentOutstandingAmt(stuen)
                            'outamt = bsstu.StudentOutstandingAmount(stuen)
                            outamt = bsstu.GetStudentOutstandingAmtInSponsorAllocation(stuen)
                        Catch ex As Exception
                            LogError.Log("Payments", "addStuCode", ex.Message)
                        End Try
                        If Session("fileType") = Nothing Then
                            If outamt >= 0 Then outamt = 0.0
                            dgItem1.Cells(5).Text = String.Format("{0:F}", outamt)
                            allamt.Text = String.Format("{0:F}", -outamt)
                            totalamt += outamt
                            Total.Text = String.Format("{0:F}", -totalamt)
                        Else
                            dgItem1.Cells(5).Text = String.Format("{0:F}", outamt)
                            allamt.Text = String.Format("{0:F}", liststudentView(j).TransactionAmount)
                            totalamt += allamt.Text
                            Total.Text = String.Format("{0:F}", totalamt)
                        End If
                        Exit For
                    End If
                Next
                j = j + 1
            End While
        End If
        Session("liststu") = Nothing
        Session("Type") = Nothing
    End Sub
    ''' <summary>
    ''' Method to add Allocations to Grid
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub addallcode()
        Dim eobjSpnall As AccountsEn
        Dim obj As New AccountsBAL
        'Dim alloc As New AccountsEn
        'Dim alloc2 As New AccountsEn
        eobjSpnall = Session("SpnAlleobj")
        txtAllCode.ReadOnly = False
        txtAllCode.Text = eobjSpnall.TransactionCode
        txtAllCode.ReadOnly = True
        txtRef1.Text = eobjSpnall.CreditRefOne
        txtSpnCode.Text = eobjSpnall.CreditRef
        txtBatch.Text = eobjSpnall.BatchCode
        'alloc2.BatchCode = eobjSpnall.BatchCode
        'alloc2.SubCategory = eobjSpnall.SubCategory
        'alloc2.SubCategory = ""
        'Try
        '    txtBatch.Text = obj.Subcategoryupdate(alloc2)
        'Catch ex As Exception
        '    LogError.Log("Payments", "addallcode", ex.Message)
        'End Try
        Dim listSt As New List(Of StudentEn)
        Dim objstu As New AccountsBAL
        Dim eobstu As New StudentEn

        eobstu.BatchCode = eobjSpnall.BatchCode
        eobstu.Category = "SPA"
        eobstu.Description = "Sponsor Pocket Amount"
        eobstu.TransStatus = "Ready"

        Try
            listSt = objstu.GetStudentAllocationTrans(eobstu)
        Catch ex As Exception
            LogError.Log("Payments", "addallcode", ex.Message)
        End Try
        If Not listSt Is Nothing Then

        End If
        Dim alltotal As Double = 0.0
        Dim amount As Double = 0.0
        Dim dgitem As DataGridItem
        Dim i As Integer = 0
        For Each dgitem In dgView.Items
            amount = dgitem.Cells(7).Text
            dgitem.Cells(7).Text = String.Format("{0:F}", amount)
            alltotal += amount
        Next
        Total.Text = String.Format("{0:F}", alltotal)
        Dim dgItem1 As DataGridItem
        Dim j As Integer = 0
        Dim Vno As TextBox
        Dim pAmount As TextBox
        Dim amt As Double = 0.0
        Dim paamt As Double = 0.0
        Dim liststuAll As New List(Of AccountsDetailsEn)
        Dim objstu1 As New AccountsDetailsBAL
        Dim eobastu As New AccountsDetailsEn
        Dim eobastu1 As New AccountsEn
        Dim stlist As New List(Of StudentEn)
        Dim stuen As New StudentEn
        Dim chk As CheckBox
        eobastu1.TranssactionID = eobjSpnall.TranssactionID
        Try
            liststuAll = objstu1.GetStuDentAllocationPocketAmount(eobastu1)
        Catch ex As Exception
            LogError.Log("Payments", "addallcode", ex.Message)
        End Try
        dgView.DataSource = liststuAll
        dgView.DataBind()
        While j < liststuAll.Count
            For Each dgItem1 In dgView.Items
                If dgItem1.Cells(1).Text = liststuAll(j).Sudentacc.MatricNo Then
                    chk = dgItem1.Cells(0).Controls(1)
                    chk.Checked = True
                    Vno = dgItem1.Cells(8).Controls(1)
                    Vno.Text = " "
                    pAmount = dgItem1.Cells(9).Controls(1)
                    paamt = liststuAll(j).TempAmount
                    pAmount.Text = String.Format("{0:F}", paamt)
                    pAmount.ReadOnly = True
                    Exit For
                End If
            Next
            j = j + 1
        End While
        LoadTotal()
        Session("SpnAlleobj") = Nothing
        Session("spncode") = eobjSpnall.TransactionCode
    End Sub
    ''' <summary>
    ''' Method to Save and Update Payments 
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub onSave()
        Dim eobj As New AccountsEn
        Dim bsobj As New AccountsBAL
        Dim list As New List(Of AccountsDetailsEn)
        Dim listst As New List(Of StudentEn)
        Dim eobstu As New StudentEn
        Dim obj As New AccountsBAL
        Dim alloc As New AccountsEn
        Dim alloc2 As New AccountsEn
        lblMsg.Text = ""
        lblMsg.Visible = True
        'tambah by farid 27022016
        'If (ddlpaymentfor.SelectedValue = "1") Then
        '    alloc2.BatchCode = Trim(txtBatch.Text)
        '    alloc2.SubCategory = ""
        '    Try
        '        txtBatch.Text = obj.Subcategoryupdate(alloc2)
        '    Catch ex As Exception
        '        LogError.Log("Payments", "addallcode", ex.Message)
        '    End Try
        'Else
        'End If
        eobj.SubReferenceOne = Trim(txtSpnCode.Text)
        eobj.CreditRefOne = Trim(txtRef1.Text)
        eobj.BankCode = ddlBankCode.SelectedValue
        eobj.PaymentMode = ddlPaymentMode.SelectedValue

        eobj.Description = Trim(txtDescri.Text)
        'eobj.BatchCode = Trim(txtBatchid.Text)
        eobj.BatchDate = Trim(txtBDate.Text)
        eobj.TransDate = Trim(txtPaymentDate.Text)
        eobj.SubType = "Student"
        If (ddlpaymentfor.SelectedValue = "1") Then
            eobj.Category = "Payment"
            If txtAllCode.Text = "" Or txtRef1.Text = "" Then
                lblMsg.Text = " Select An Allocation"
                Exit Sub
            Else
                'eobj.CreditRef = Trim(txtAllCode.Text)
                eobj.TransactionAmount = Trim(Total.Text)
                eobj.PaidAmount = Trim(Total.Text)
                eobj.SubReferenceTwo = Trim(txtBatch.Text)
            End If
            'edit By farid 23/02/2016
            eobj.TransType = "Debit"
            'End
        Else
            eobj.Category = "Refund"
            eobj.BatchTotal = Trim(Total.Text)
            eobj.PaidAmount = 0
            'added By farid 24/02/2016
            eobj.VoucherNo = Trim(txtAllCode.Text)
            'Added By Zoya 17/02/2016
            eobj.TransType = "Debit"
            'End
        End If

        'eobj.TransType = "Credit"
        eobj.PostStatus = "Ready"
        eobj.TransStatus = "Open"
        eobj.PostedDateTime = DateTime.Now
        eobj.UpdatedTime = DateTime.Now
        eobj.DueDate = DateTime.Now
        eobj.CreatedBy = Session("User")
        eobj.CreatedDateTime = DateTime.Now
        eobj.ChequeDate = DateTime.Now
        eobj.UpdatedBy = Session("User")

        Dim eobjstu As AccountsDetailsEn
        Dim vochure As TextBox
        Dim pocketAmount As TextBox
        Dim chk As CheckBox
        Dim dgitem As DataGridItem
        list = Nothing
        list = New List(Of AccountsDetailsEn)
        If (ddlpaymentfor.SelectedValue = "1") Then
            If txtAllCode.Text = "" Then
                lblMsg.Text = " Select A Sponsor. "
                Exit Sub
            End If
            For Each dgitem In dgView.Items

                vochure = dgitem.Cells(8).Controls(1)
                pocketAmount = dgitem.Cells(9).Controls(1)
                'Added by Farid 17/2/2016
                chk = dgitem.Cells(0).Controls(1)
                chk.Checked = True

                If Not dgitem.Cells(7).Text = "0.00" Then

                    If vochure.Text = "" Or Trim(vochure.Text).Length = 0 Then
                        ErrorDescription = "Enter Voucher No"
                        lblMsg.Text = ErrorDescription
                        Exit Sub
                    ElseIf Trim(vochure.Text).Length > 12 Then
                        ErrorDescription = "Exceed Max Length Voucher No (12)"
                        lblMsg.Text = ErrorDescription
                    Else
                        eobj.VoucherNo = vochure.Text
                    End If

                    If pocketAmount.Text = "" Or Trim(pocketAmount.Text).Length = 0 Then
                        ErrorDescription = "Enter Pocket Amount"
                        lblMsg.Text = ErrorDescription
                        Exit Sub
                    Else
                        eobj.PocketAmount = pocketAmount.Text
                    End If
                Else
                    eobj.PocketAmount = ""
                    eobj.VoucherNo = ""
                End If
                'Added by Farid 17/2/2016
                LoadTotal()
                eobj.TransactionAmount = Trim(Total.Text)
                eobj.PaidAmount = Trim(Total.Text)
                'commented 250216 by farid
                'eobj.CreditRef = Trim(dgitem.Cells(1).Text)

                eobjstu = New AccountsDetailsEn
                eobjstu.ReferenceCode = dgitem.Cells(1).Text
                eobjstu.PaidAmount = dgitem.Cells(7).Text
                eobjstu.TransactionAmount = dgitem.Cells(7).Text
                eobjstu.ReferenceOne = vochure.Text
                eobjstu.ReferenceTwo = Trim(txtBatch.Text)
                'commented 250216 by farid
                'eobjstu.ReferenceThree = Trim(pocketAmount.Text)
                eobjstu.TempAmount = Trim(pocketAmount.Text)
                eobjstu.PostStatus = "Ready"
                eobjstu.TransStatus = "Open"
                list.Add(eobjstu)
                eobjstu = Nothing


            Next
            If Not Session("spncode") Is Nothing Then
                eobstu.TransactionCode = Session("spncode")
            Else
                eobstu.TransactionCode = txtAllCode.Text

            End If
            listst.Add(eobstu)

        Else
            Dim dgItem1 As DataGridItem
            Dim amount As TextBox
            Dim chkselect As CheckBox
            Dim estu As StudentEn
            Dim AllAmt As Double = 0.0
            Dim outamt As Double = 0.0

            For Each dgItem1 In dgView.Items

                chkselect = dgItem1.Cells(0).Controls(1)
                If chkselect.Checked = True Then
                    vochure = dgItem1.Cells(8).Controls(1)
                    'pocketAmount = dgItem1.Cells(9).Controls(1)
                    'If vochure.Text = "" Or Trim(vochure.Text).Length = 0 Then
                    '    ErrorDescription = "Enter Vochure No"
                    '    lblMsg.Text = ErrorDescription
                    '    Exit Sub
                    'Else
                    '    eobj.VoucherNo = vochure.Text
                    'End If
                    'If pocketAmount.Text = "" Or Trim(pocketAmount.Text).Length = 0 Then
                    '    ErrorDescription = "Enter Pocket Amount"
                    '    lblMsg.Text = ErrorDescription
                    '    Exit Sub
                    'Else
                    '    eobj.PocketAmount = pocketAmount.Text
                    'End If
                    amount = dgItem1.Cells(6).Controls(1)
                    outamt = dgItem1.Cells(5).Text
                    outamt = outamt * -1
                    AllAmt = amount.Text
                    'If AllAmt > -outamt Then
                    If AllAmt > outamt Then
                        lblMsg.Text = "Amount Cannot be Greater than Credit Amount"
                        eflag = "no"
                        Exit Sub
                    End If
                    eobj.CreditRef = Trim(dgItem1.Cells(1).Text)
                    estu = New StudentEn
                    estu.MatricNo = dgItem1.Cells(1).Text
                    'estu.PaidAmount = dgItem1.Cells(4).Text
                    estu.PaidAmount = dgItem1.Cells(5).Text
                    estu.TransactionAmount = amount.Text
                    estu.VoucherNo = dgItem1.Cells(8).Text
                    estu.PocketAmount = dgItem1.Cells(9).Text
                    estu.PostStatus = "Ready"
                    estu.TransStatus = "Open"
                    listst.Add(estu)
                    estu = Nothing
                End If
            Next
            If listst.Count = 0 Then
                lblMsg.Visible = True
                lblMsg.Text = "Select At Least One Student"
                Exit Sub
            End If
        End If
        If list Is Nothing Then
            eobj.AccountDetailsList = Nothing
        Else
            eobj.AccountDetailsList = list
        End If

        Dim bid As String = ""

        lblMsg.Visible = True
        If Session("PageMode") = "Add" Then
            If (ddlpaymentfor.SelectedValue = "1") Then
                Try

                    txtBatchid.Text = bsobj.StudentBatchInsert(eobj, listst)
                    txtBatchid.ReadOnly = True
                    ErrorDescription = "Record Saved Successfully "
                    ibtnStatus.ImageUrl = "images/ready.gif"
                    lblStatus.Value = "Ready"
                    lblMsg.Text = ErrorDescription
                    ibtnSave.Enabled = False

                Catch ex As Exception
                    lblMsg.Text = ex.Message.ToString()
                    LogError.Log("Payments", "onSave", ex.Message)
                End Try
            Else
                Try

                    txtBatchid.Text = bsobj.StudentBatchInsert(eobj, listst)
                    txtBatchid.ReadOnly = True
                    ErrorDescription = "Record Saved Successfully "
                    ibtnStatus.ImageUrl = "images/ready.gif"
                    lblStatus.Value = "Ready"
                    lblMsg.Text = ErrorDescription
                    ibtnSave.Enabled = False

                Catch ex As Exception
                    lblMsg.Text = ex.Message.ToString()
                    LogError.Log("Payments", "onSave", ex.Message)
                End Try
            End If

        ElseIf Session("PageMode") = "Edit" Then
            Try
                txtBatchid.Text = bsobj.StudentBatchUpdate(eobj, listst)
                txtBatchid.ReadOnly = False
                ErrorDescription = "Record Updated Successfully "
                ibtnStatus.ImageUrl = "images/ready.gif"
                lblStatus.Value = "Ready"
                lblMsg.Text = ErrorDescription

            Catch ex As Exception
                lblMsg.Text = ex.Message.ToString()
                LogError.Log("Payments", "onSave", ex.Message)
            End Try

        End If
        'setDateFormat()
    End Sub
    ''' <summary>
    ''' Method to Post Payments
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub OnPost()
        Dim eobj As New AccountsEn
        Dim bsobj As New AccountsBAL
        Dim list As New List(Of AccountsDetailsEn)
        Dim listst As New List(Of StudentEn)
        Dim eobstu As New StudentEn
        lblMsg.Text = ""
        lblMsg.Visible = True

        'eobj.CreditRef = Trim(txtAllCode.Text)
        eobj.SubReferenceOne = Trim(txtSpnCode.Text)
        eobj.CreditRefOne = Trim(txtRef1.Text)
        eobj.BankCode = ddlBankCode.SelectedValue
        eobj.PaymentMode = ddlPaymentMode.SelectedValue
        'eobj.TransactionAmount = Trim(Total.Text)

        eobj.Description = Trim(txtDescri.Text)
        eobj.BatchCode = Trim(txtBatchid.Text)
        eobj.BatchDate = Trim(txtBDate.Text)
        eobj.TransDate = Trim(txtPaymentDate.Text)
        eobj.SubType = "Student"
        If (ddlpaymentfor.SelectedValue = "1") Then
            eobj.Category = "Payment"
            'added by farid 250216
            eobj.TransType = "Debit"
            eobj.PostStatus = "Ready"
            eobj.TransStatus = "Closed"
            If txtAllCode.Text = "" Or txtRef1.Text = "" Then
                lblMsg.Text = " Select A Allocation"
                Exit Sub
            Else
                'eobj.CreditRef = Trim(txtAllCode.Text)
                eobj.TransactionAmount = Trim(Total.Text)
                eobj.PaidAmount = Trim(Total.Text)
                eobj.SubReferenceTwo = Trim(txtBatch.Text)
            End If
        Else
            eobj.Category = "Refund"
            eobj.TransType = "Debit"
            eobj.BatchTotal = Trim(Total.Text)
            eobj.PaidAmount = 0
            'eobj.CreditRef
        End If
        'eobj.TransType = "Credit"
        'eobj.PostStatus = "Posted"
        'eobj.TransStatus = "Open"
        eobj.PostedBy = Session("User")
        eobj.PostedDateTime = DateTime.Now
        eobj.UpdatedTime = DateTime.Now
        eobj.DueDate = DateTime.Now
        eobj.CreatedDateTime = DateTime.Now
        eobj.ChequeDate = DateTime.Now

        Dim eobjstu As AccountsDetailsEn
        Dim vochure As TextBox
        Dim pocketAmount As TextBox

        Dim dgitem As DataGridItem
        list = Nothing
        list = New List(Of AccountsDetailsEn)
        If (ddlpaymentfor.SelectedValue = "1") Then
            If txtAllCode.Text = "" Then
                lblMsg.Text = " Select A Sponsor. "
                Exit Sub
            End If
            For Each dgitem In dgView.Items

                vochure = dgitem.Cells(8).Controls(1)
                If vochure.Text = "" Or Trim(vochure.Text).Length = 0 Then
                    ErrorDescription = "Enter Voucher No"
                    lblMsg.Text = ErrorDescription
                    Exit Sub
                Else
                    eobj.VoucherNo = vochure.Text
                End If

                pocketAmount = dgitem.Cells(9).Controls(1)
                If pocketAmount.Text = "" Or Trim(pocketAmount.Text).Length = 0 Then
                    ErrorDescription = "Enter Pocket Amount"
                    lblMsg.Text = ErrorDescription
                    Exit Sub
                Else
                    eobj.PocketAmount = pocketAmount.Text
                End If
                'eobj.CreditRef = dgitem.Cells(1).Text
                eobjstu = New AccountsDetailsEn
                eobjstu.ReferenceCode = dgitem.Cells(1).Text
                'eobjstu.PaidAmount = dgitem.Cells(7).Text
                eobjstu.TransactionAmount = dgitem.Cells(7).Text
                eobjstu.ReferenceOne = vochure.Text
                eobjstu.ReferenceTwo = Trim(txtBatch.Text)
                eobjstu.PaidAmount = Trim(pocketAmount.Text)
                eobjstu.PostStatus = "Posted"
                eobjstu.TransStatus = "Open"
                list.Add(eobjstu)
                eobjstu = Nothing
            Next
            If Not Session("spncode") Is Nothing Then
                eobstu.TransactionCode = Session("spncode")
            Else
                eobstu.TransactionCode = txtAllCode.Text

            End If
            listst.Add(eobstu)

        Else
            Dim dgItem1 As DataGridItem
            Dim amount As TextBox
            Dim chkselect As CheckBox
            Dim estu As StudentEn
            Dim AllAmt As Double = 0.0
            Dim outamt As Double = 0.0

            For Each dgItem1 In dgView.Items
                chkselect = dgItem1.Cells(0).Controls(1)
                If chkselect.Checked = True Then
                    vochure = dgItem1.Cells(8).Controls(1)
                    pocketAmount = dgItem1.Cells(9).Controls(1)
                    If vochure.Text = "" Or Trim(vochure.Text).Length = 0 Then
                        ErrorDescription = "Enter Vochure No"
                        lblMsg.Text = ErrorDescription
                        Exit Sub
                    Else
                        eobj.VoucherNo = vochure.Text
                    End If
                    If pocketAmount.Text = "" Or Trim(pocketAmount.Text).Length = 0 Then
                        ErrorDescription = "Enter Pocket Amount"
                        lblMsg.Text = ErrorDescription
                        Exit Sub
                    Else
                        eobj.PocketAmount = pocketAmount.Text
                    End If
                    amount = dgItem1.Cells(6).Controls(1)
                    outamt = dgItem1.Cells(5).Text
                    AllAmt = amount.Text
                    If AllAmt > -outamt Then
                        lblMsg.Text = "Amount Cannot be Greater than Outstanding Amount"
                        eflag = "no"
                        Exit Sub
                    End If
                    estu = New StudentEn
                    estu.MatricNo = dgItem1.Cells(1).Text
                    estu.PaidAmount = dgItem1.Cells(4).Text
                    estu.TransactionAmount = amount.Text
                    estu.VoucherNo = dgItem1.Cells(8).Text
                    estu.PostStatus = "Posted"
                    estu.TransStatus = "Open"
                    listst.Add(estu)
                    estu = Nothing
                End If
            Next
            If listst.Count = 0 Then
                lblMsg.Visible = True
                lblMsg.Text = "Select At Least One Student"
                Exit Sub
            End If
        End If
        If list Is Nothing Then
            eobj.AccountDetailsList = Nothing
        Else
            eobj.AccountDetailsList = list
        End If

        Dim bid As String = ""
        lblMsg.Visible = True
        'Status=New
        Try
            txtBatchid.Text = bsobj.StudentBatchUpdate(eobj, listst)
            'ErrorDescription = "Record Posted Successfully "
            txtBatchid.ReadOnly = True
            ibtnStatus.ImageUrl = "images/posted.gif"
            lblMsg.Visible = True
            lblMsg.Text = ErrorDescription
            'lblStatus.Value = "Posted"
            'eobj.PostStatus = "Posted"

            'Remove item from List 
            If Not Session("ListObj") Is Nothing Then
                ListObjects = Session("ListObj")
                ListObjects(CInt(txtRecNo.Text) - 1) = eobj
                Session("ListObj") = ListObjects
                'If lblStatus.Value = "Posted" Then
                '    ibtnStatus.ImageUrl = "images/posted.gif"
                '    lblStatus.Value = "Posted"
                'End If

            End If



            'If Trim(txtBatchid.Text) <> "" Or Trim(txtBatchid.Text) <> Nothing Then
            '    'Get Status Integration To SAGA
            '    dsReturn = objIntegrationDL.GetIntegrationStatus()

            '    'Check Status Integration To SAGA
            '    If dsReturn.Tables(0).Rows(0).Item("CON_Value3") = "1" Then

            '        Dim strBatchNo As String = Trim(txtBatchid.Text)

            '        'Post To SAGA
            '        objIntegration.Payment(strBatchNo)

            '    Else
            '        ErrorDescription = "Record Posted Successfully But No Integration To CF. Please Call Administrator "
            '    End If
            'End If

            lblMsg.Text = ErrorDescription

        Catch ex As Exception
            lblMsg.Text = ex.Message.ToString()
            LogError.Log("Payments", "OnPost", ex.Message)
        End Try

    End Sub

    ''' <summary>
    ''' Method to Add all Amounts in the Grid
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub LoadTotals()
        Dim chk As CheckBox
        Dim txtAmount As TextBox
        Dim dgItem1 As DataGridItem
        Dim totalAmt1 As Double = 0
        Dim outAmt As Double = 0
        For Each dgItem1 In dgView.Items
            Dim totalAmt As Double = 0
            chk = dgItem1.Cells(0).Controls(1)
            If chk.Checked = True Then
                Dim AllAmt As Double = 0
                Dim Allpck As Double = 0
                txtAmount = dgItem1.Cells(6).Controls(1)
                outAmt = dgItem1.Cells(5).Text
                If txtAmount.Text = "" Then txtAmount.Text = 0.0
                AllAmt = txtAmount.Text
                If AllAmt > -outAmt Then
                    lblMsg.Text = "Amount Cannot be Greater than Outstanding Amount"
                    eflag = "no"
                    Exit Sub
                End If
                totalAmt1 += AllAmt
            End If
            Total.Text = String.Format("{0:F}", totalAmt1)
        Next
    End Sub

    ''' <summary>
    ''' Method to Load Total in the Grid
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub LoadTotal()

        Dim chk As CheckBox
        Dim txtpaamount As TextBox
        Dim dgItem1 As DataGridItem
        Dim totalAmt1 As Double = 0
        For Each dgItem1 In dgView.Items
            Dim totalAmt As Double = 0
            chk = dgItem1.Cells(0).Controls(1)
            If chk.Checked = True Then
                Dim AllAmt As Double = 0
                Dim Allpck As Double = 0
                txtpaamount = dgItem1.Cells(9).Controls(1)
                If txtpaamount.Text = "" Then txtpaamount.Text = 0.0
                Allpck = txtpaamount.Text
                totalAmt1 += Allpck
            End If
            Total.Text = String.Format("{0:F}", totalAmt1)
        Next


    End Sub

    ''' <summary>
    ''' Method to Check for Allocation or Refund and load Grid
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub check_Paymenttfor()

        If ddlpaymentfor.SelectedValue = "1" Then
            Label15.Visible = True
            Label1.Visible = True
            Label2.Visible = True
            txtAllCode.Visible = True
            txtRef1.Visible = True
            txtSpnCode.Visible = True
            ibtnstu.Visible = False
            ibtnSpn1.Visible = True
            dgView.Columns(5).Visible = False
            dgView.Columns(6).Visible = False
            'edited by farid 11032016
            dgView.Columns(7).Visible = False
            dgView.Columns(0).Visible = False
            dgView.Columns(8).Visible = True
            Type.Text = "Select Allocation"
            Type.Visible = True
            dgView.DataSource = Nothing
            dgView.DataBind()
            Session("ReceiptFor") = ddlpaymentfor.SelectedValue
        ElseIf ddlpaymentfor.SelectedValue = "St" Then
            Label15.Visible = False
            Label1.Visible = False
            Label2.Visible = False
            txtAllCode.Visible = False
            txtRef1.Visible = False
            txtSpnCode.Visible = False
            ibtnSpn1.Visible = False
            ibtnstu.Visible = True
            Type.Text = "Select Student"
            Type.Visible = True
            dgView.Columns(0).Visible = True
            dgView.Columns(5).Visible = True
            dgView.Columns(7).Visible = False
            dgView.Columns(6).Visible = True
            dgView.Columns(8).Visible = False
            dgView.Columns(9).Visible = False
            dgView.DataSource = Nothing
            dgView.DataBind()
            Session("ReceiptFor") = ddlpaymentfor.SelectedValue
            upload.Visible = True
            lblUpload.Visible = False
            btnUpload.Visible = False
            Label15.Visible = True
            Label15.Text = "Voucher No."
            txtAllCode.Visible = True
            txtAllCode.Enabled = True
            txtAllCode.ReadOnly = False
            Label48.Visible = True
        Else
            Label15.Visible = False
            Label1.Visible = False
            Label2.Visible = False
            txtAllCode.Visible = False
            txtRef1.Visible = False
            txtSpnCode.Visible = False
            ibtnSpn1.Visible = False
            ibtnstu.Visible = False
            Type.Visible = False
            dgView.DataSource = Nothing
            dgView.DataBind()
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
            LogError.Log("Payments", "LoadUserRights", ex.Message)
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
    ''' Method to Load Fields in New Mode
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub onAdd()
        'Session("ListObj") = Nothing
        Session("stu") = Nothing
        check_Paymenttfor()
        'OnClearData()
        If ibtnNew.Enabled = False Then
            ibtnSave.Enabled = False
            ibtnSave.ImageUrl = "images/gsave.png"
            ibtnSave.ToolTip = "Access Denied"
        End If
        If Session("PageMode") = "Add" Then
            addBankCode()
            txtBatchid.ReadOnly = False
            'txtBatchid.Text = "Auto Number"
            txtBatchid.ReadOnly = True
            txtBDate.Text = Format(Date.Now, "dd/MM/yyyy")
            txtPaymentDate.Text = Format(Date.Now, "dd/MM/yyyy")
        End If
        today.Value = Now.Date
        today.Value = Format(CDate(today.Value), "dd/MM/yyyy")
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
        txtBatchid.Text = ""
        DisableRecordNavigator()
        txtAllCode.Text = ""
        txtRef1.Text = ""
        lblMsg.Text = ""
        txtSpnCode.Text = ""
        ddlPaymentMode.SelectedValue = "-1"
        ddlBankCode.SelectedValue = "-1"
        ddlpaymentfor.SelectedValue = "-1"
        txtDescri.Text = ""
        dates()
        'txtBatchid.Text = ""
        'txtBDate.Text = ""
        Total.Text = 0.0
        'txtPaymentDate.Text = ""
        dgView.DataSource = Nothing
        dgView.DataBind()
        Session("PageMode") = "Add"
    End Sub

    Private Sub ondate()
        txtPaymentDate.Text = Date.Now
    End Sub
    ''' <summary>
    ''' Method to get the List Of Transactions
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub LoadListObjects()
        Dim eob As New AccountsEn
        Dim bobj As New AccountsBAL
        If txtBatchid.Text <> "" Then
            eob.BatchCode = txtBatchid.Text
        Else
            eob.BatchCode = ""
        End If
        eob.BatchIntake = ""
        eob.SubType = "Student"
        eob.PostStatus = "Ready"
        If Session("loaddata") = "View" Then
            If ddlpaymentfor.SelectedValue = "1" Then
                eob.Category = "Payment"
            ElseIf ddlpaymentfor.SelectedValue = "St" Then
                eob.Category = "Refund"
            End If

            Try
                ListObjects = bobj.GetTransactions(eob)
            Catch ex As Exception
                lblMsg.Text = ex.Message
                LogError.Log("Payments", "LoadListObjects", ex.Message)
            End Try
        ElseIf Session("loaddata") = "others" Then

            eob.SubType = "Student"
            eob.PostStatus = "Posted"
            If ddlpaymentfor.SelectedValue = "1" Then
                eob.Category = "Payment"
                eob.TransStatus = ""
                Try
                    ListObjects = bobj.GetSPAllocationTransactions(eob)
                Catch ex As Exception
                    LogError.Log("Payments", "LoadListObjects", ex.Message)
                    lblMsg.Text = ex.Message
                End Try

            ElseIf ddlpaymentfor.SelectedValue = "St" Then
                eob.Category = "Refund"

                Try
                    ListObjects = bobj.GetTransactions(eob)
                Catch ex As Exception
                    LogError.Log("Payments", "LoadListObjects", ex.Message)
                    lblMsg.Text = ex.Message
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
            txtBatchid.Text = ""
            DisableRecordNavigator()
            txtAllCode.Text = ""
            txtRef1.Text = ""
            txtSpnCode.Text = ""
            ddlPaymentMode.SelectedValue = "-1"
            ddlBankCode.SelectedValue = "-1"
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

                Dim eobj As AccountsEn
                ListObjects = Session("ListObj")
                eobj = ListObjects(RecNo)
                txtAllCode.ReadOnly = False
                txtRef1.ReadOnly = False
                txtSpnCode.ReadOnly = False
                'commented by farid on 25022016
                'txtAllCode.Text = eobj.CreditRef
                txtRef1.Text = eobj.CreditRefOne
                txtSpnCode.Text = eobj.SubReferenceOne
                txtAllCode.ReadOnly = True
                txtRef1.ReadOnly = True
                txtSpnCode.ReadOnly = True
                ddlPaymentMode.Items.Clear()
                ddlPaymentMode.Items.Add(New ListItem("---Select---", "-1"))
                ddlPaymentMode.DataSource = Session("paymode")
                ddlPaymentMode.DataBind()
                ddlPaymentMode.SelectedValue = eobj.PaymentMode
                ddlBankCode.Items.Clear()
                ddlBankCode.Items.Add(New ListItem("---Select---", "-1"))
                ddlBankCode.DataSource = Session("bankcode")
                ddlBankCode.DataBind()
                ddlBankCode.SelectedValue = eobj.BankCode
                If eobj.Category = "Payment" Then
                    'add by farid 23022016'
                    txtAllCode.Text = eobj.TransactionCode
                    LoadUserRights()
                    ddlpaymentfor.SelectedValue = "1"
                Else
                    'commented by farid on 25022016
                    'txtAllCode.Text = eobj.TransactionCode
                    LoadUserRights()
                    ddlpaymentfor.SelectedValue = "St"
                End If
                check_Paymenttfor()
                txtDescri.Text = eobj.Description
                txtBatchid.Text = eobj.BatchCode
                txtBDate.Text = eobj.BatchDate
                txtPaymentDate.Text = eobj.TransDate
                txtBDate.Text = eobj.BatchDate

                If eobj.PostStatus = "Ready" Then
                    lblStatus.Value = "Ready"
                    ibtnStatus.ImageUrl = "images/Ready.gif"
                End If
                If eobj.PostStatus = "Posted" Then
                    lblStatus.Value = "Posted"
                    ibtnStatus.ImageUrl = "images/Posted.gif"
                End If
                If ddlpaymentfor.SelectedValue = "1" Then
                    'add by farid 25022016'
                    If eobj.PostStatus = "Ready" Then
                        txtAllCode.Text = eobj.TempTransCode
                        lblStatus.Value = "Ready"
                        ibtnStatus.ImageUrl = "images/Ready.gif"
                        'eobj.TransactionCode.Trim(txtAllCode.Text)
                    End If
                    'add by farid 23022016'
                    dgView.Columns(9).Visible = True
                    Dim liststuAll As New List(Of AccountsDetailsEn)
                    Dim objstu As New AccountsDetailsBAL
                    Dim eobstu As New AccountsDetailsEn
                    Dim stlist As New List(Of StudentEn)
                    Dim stuen As New StudentEn
                    Total.Text = String.Format("{0:F}", eobj.TransactionAmount)
                    txtBatch.Text = eobj.SubReferenceOne
                    eobstu.TransactionID = eobj.TranssactionID

                    Try
                        liststuAll = objstu.GetStudentPaymentAllocation(eobstu)
                    Catch ex As Exception
                        LogError.Log("Payments", "FillData", ex.Message)
                    End Try
                    dgView.DataSource = liststuAll
                    dgView.DataBind()
                    Dim dgItem1 As DataGridItem
                    Dim j As Integer = 0
                    Dim Vno As TextBox
                    Dim pAmount As TextBox
                    Dim amt As Double = 0.0
                    Dim chk As CheckBox
                    While j < liststuAll.Count
                        For Each dgItem1 In dgView.Items
                            If dgItem1.Cells(1).Text = liststuAll(j).Sudentacc.MatricNo Then
                                chk = dgItem1.Cells(0).Controls(1)
                                chk.Checked = True
                                Vno = dgItem1.Cells(8).Controls(1)
                                Vno.Text = liststuAll(j).ReferenceOne
                                pAmount = dgItem1.Cells(9).Controls(1)
                                'edit by farid 25022016'
                                pAmount.Text = liststuAll(j).TempAmount
                                Exit For
                            End If
                        Next
                        j = j + 1
                    End While
                    LoadTotal()
                ElseIf ddlpaymentfor.SelectedValue = "St" Then
                    'add by farid 25022016'
                    txtAllCode.Text = eobj.VoucherNo
                    Dim Vouno As TextBox
                    Dim pocketAm As TextBox
                    Total.Text = String.Format("{0:F}", eobj.BatchTotal)
                    Dim estu As New StudentEn
                    Dim listTrcpt As New List(Of StudentEn)
                    Dim Trcptobj As New AccountsBAL
                    estu.MatricNo = eobj.CreditRef
                    estu.BatchCode = txtBatchid.Text

                    Try
                        listTrcpt = Trcptobj.GetStudentReceiptsbyBatchID(estu)
                    Catch ex As Exception
                        LogError.Log("Payments", "FillData", ex.Message)
                    End Try
                    If listTrcpt Is Nothing Then
                    Else
                        dgView.DataSource = listTrcpt
                        dgView.DataBind()
                        Session("listview") = listTrcpt
                        Dim dgItem1 As DataGridItem
                        Dim amt As Double = 0.0
                        Dim k As Integer = 0
                        Dim chkselect As CheckBox
                        Dim txtAmount As TextBox
                        While k < listTrcpt.Count
                            For Each dgItem1 In dgView.Items
                                If dgItem1.Cells(1).Text = listTrcpt(k).MatricNo Then
                                    Vouno = dgItem1.Cells(8).Controls(1)
                                    Vouno.Text = listTrcpt(k).VoucherNo
                                    pocketAm = dgItem1.Cells(9).Controls(1)
                                    pocketAm.Text = listTrcpt(k).PocketAmount
                                    txtAmount = dgItem1.Cells(6).Controls(1)
                                    chkselect = dgItem1.Cells(0).Controls(1)
                                    chkselect.Checked = True
                                    amt = txtAmount.Text
                                    txtAmount.Text = String.Format("{0:F}", listTrcpt(k).TransactionAmount)
                                    dgItem1.Cells(5).Text = String.Format("{0:F}", -listTrcpt(k).TransactionAmount)
                                End If
                            Next
                            k = k + 1
                        End While
                    End If
                End If

                CheckWorkflowStatus(eobj)

            End If
        End If
        setDateFormat()
    End Sub

    ''' <summary>
    ''' Method To Change the Date Format(dd/MM/yyyy)
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub setDateFormat()

        'Dim GBFormat As System.Globalization.CultureInfo
        'GBFormat = New System.Globalization.CultureInfo("en-GB")
        Dim myPaymentDate As Date = CDate(CStr(txtPaymentDate.Text))
        Dim myFormat As String = "dd/MM/yyyy"
        Dim myFormattedDate As String = Format(myPaymentDate, myFormat)
        txtPaymentDate.Text = myFormattedDate
        Dim myBatchDate As Date = CDate(CStr(txtBDate.Text))
        Dim myFormattedDate2 As String = Format(myBatchDate, myFormat)
        txtBDate.Text = myFormattedDate2

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
            lblMsg.Text = ex.Message
            LogError.Log("Payments", "Menuname", ex.Message)
        End Try
        lblMenuName.Text = eobj.MenuName
    End Sub
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
                eob.BatchCode = Trim(txtBatchid.Text)
                RecAff = bsobj.BatchDelete(eob)
                onAdd()
                DFlag = "Delete"
                Session("loaddata") = "View"
                lblMsg.Text = "Record Deleted Successfully "
                lblMsg.Visible = True
                LoadListObjects()
                'Session("ListObj") = ListObjects
            Catch ex As Exception
                lblMsg.Text = ex.Message.ToString()
                LogError.Log("Payments", "ondelete", ex.Message)
            End Try

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
                addBankCode()
                LoadListObjects()

            End If
        Else
            Session("PageMode") = "Edit"
            addBankCode()
            LoadListObjects()

            PostEnFalse()
        End If

    End Sub
#End Region

    Protected Sub ibtnPosting_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)

        'If lblStatus.Value = "New" Then
        '    lblMsg.Text = "Record not ready for Posting"
        '    lblMsg.Visible = True
        'ElseIf lblStatus.Value = "Posted" Then
        '    lblMsg.Text = "Record already Posted"
        '    lblMsg.Visible = True
        'ElseIf lblStatus.Value = "Ready" Then
        '    SpaceValidation()
        '    OnPost()
        '    setDateFormat()
        'End If

    End Sub

    Protected Sub ibtnSave_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ibtnSave.Click

        If dgView.Items.Count = 0 Then
            lblMsg.Text = "Select At least One Student"
            lblMsg.Visible = True
            Exit Sub
        Else
            lblMsg.Visible = False
        End If
        SpaceValidation()
        onSave()
        setDateFormat()
    End Sub
    Protected Sub ibtnView_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ibtnView.Click
        Session("loaddata") = "View"
        If lblCount.Text <> "" Then
            If CInt(lblCount.Text) > 0 Then
                onAdd()
            Else

                Session("PageMode") = "Edit"
                addBankCode()
                LoadListObjects()
                Session("statusPrint") = "Ready"
            End If
        Else
            Session("PageMode") = "Edit"
            addBankCode()
            LoadListObjects()
            Session("statusPrint") = "Ready"
        End If
        If lblCount.Text.Length = 0 Then
            Session("PageMode") = "Add"
        End If
    End Sub

    Protected Sub txtvochure_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs)

    End Sub

    'Protected Sub ibtnPosting_Click1(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ibtnPosting.Click
    '    'Varaible Declaration
    '    Dim BatchCode As String = MaxGeneric.clsGeneric.NullToString(txtBatchid.Text)

    '    'Calling PostToWorFlow
    '    '_Helper.PostToWorkflow(BatchCode, DoneBy(), Request.Url.AbsoluteUri)

    '    If _Helper.PostToWorkflow(BatchCode, Session("User"), Request.Url.AbsoluteUri) Then
    '        'If ddlpaymentfor.SelectedValue = "1" Then
    '        '    OnPost()
    '        'Else

    '        'End If
    '        lblMsg.Visible = True
    '        lblMsg.Text = "Record Posted Successfully for Approval"
    '    Else
    '        lblMsg.Visible = True
    '        lblMsg.Text = "Record Posted Failed"
    '    End If

    '        'If lblStatus.Value = "New" Then
    '        '    lblMsg.Text = "Record not Ready for Posting"
    '        '    lblMsg.Visible = True
    '        'ElseIf lblStatus.Value = "Posted" Then
    '        '    lblMsg.Text = "Record Already Posted"
    '        '    lblMsg.Visible = True
    '        'ElseIf lblStatus.Value = "Ready" Then
    '        '    SpaceValidation()
    '        '    OnPost()
    '        '    setDateFormat()
    '        'End If
    'End Sub

#Region "SendToApproval"

    Protected Sub SendToApproval()

        Try
            If Not Session("listWF") Is Nothing Then
                Dim list As List(Of WorkflowSetupEn) = Session("listWF")
                If list.Count > 0 Then

                    If _Helper.PostToWorkflow(MaxGeneric.clsGeneric.NullToString(txtBatchid.Text), Session("User"), Request.Url.AbsoluteUri) = True Then

                        setDateFormat()

                        If Session("listWF").count > 0 Then
                            WorkflowApproverList(Trim(txtBatchid.Text), Session("listWF"))
                        End If

                        lblMsg.Visible = True
                        lblMsg.Text = "Record Posted Successfully for Approval"

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

    Protected Sub ibtnNew_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ibtnNew.Click
        'onAdd()
        OnClearData()
    End Sub
    Protected Sub ibtnFirst_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ibtnFirst.Click
        OnMoveFirst()
    End Sub

    Protected Sub ibtnNext_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ibtnNext.Click
        OnMoveNext()
    End Sub

    Protected Sub ibtnPrevs_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ibtnPrevs.Click
        OnMovePrevious()
    End Sub

    Protected Sub ibtnLast_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ibtnLast.Click
        OnMoveLast()
    End Sub
    Protected Sub dgView_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dgView.ItemDataBound
        Dim txtVoucher As TextBox
        Dim txtamt As TextBox
        Dim txt As Double = 0.0
        Select Case e.Item.ItemType
            Case ListItemType.Item, ListItemType.AlternatingItem
                txtVoucher = CType(e.Item.FindControl("Voucher"), TextBox)
                txtVoucher.Attributes.Add("onKeyPress", "return geterr()")
                txtVoucher.Text = txtAllCode.Text
                txtamt = CType(e.Item.FindControl("TxtAmt"), TextBox)
                'txtamt.Attributes.Add("onkeypress", "checknValue(event,this);")
                txtamt.Text = String.Format("{0:F}", txt)
        End Select
    End Sub
    Protected Sub ibtnOthers_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        OnSearchOthers()
    End Sub
    Protected Sub ibtnDelete_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        ondelete()
    End Sub

    Protected Sub ibtnCancel_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ibtnCancel.Click
        LoadUserRights()
        ibtnNew.Enabled = True
        ibtnNew.ImageUrl = "images/add.png"
        ibtnDelete.ImageUrl = "images/delete.png"
        ibtnDelete.Enabled = True
        ibtnNew.ToolTip = "New"
        ibtnSave.ToolTip = "Save"
        ibtnDelete.ToolTip = "Delete"
        ibtnView.ToolTip = "Search"
        onAdd()
        OnClearData()
        txtBatchid.ReadOnly = False
    End Sub

    Protected Sub txtRecNo_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        If txtRecNo.Text = "" Then
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

    Protected Sub dgView_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs)

    End Sub


    Protected Sub ddlpaymentfor_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlpaymentfor.SelectedIndexChanged
        check_Paymenttfor()
    End Sub


    Protected Sub TxtAmt_TextChanged1(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim chk As CheckBox
        Dim textamt As TextBox
        Dim totalAmt As Double = 0.0
        Dim amt As Double = 0.0
        'Added by zoya 16/02/2016
        Dim creditAmt As Double = 0.0
        'end

        Dim dgItem1 As DataGridItem
        For Each dgItem1 In dgView.Items
            chk = dgItem1.Cells(0).Controls(1)
            textamt = dgItem1.Cells(6).Controls(1)

            'Added by zoya 16/02/2016
            creditAmt = dgItem1.Cells(5).Text
            'end

            If chk.Checked = True Then
                If textamt.Text = "" Then textamt.Text = 0.0
                If textamt.Text = amt Then
                    textamt.Text = String.Format("{0:F}", amt)
                    totalAmt += amt
                    Total.Text = String.Format("{0:F}", totalAmt)
                Else

                    amt = textamt.Text
                    textamt.Text = String.Format("{0:F}", amt)
                    totalAmt += amt
                    Total.Text = String.Format("{0:F}", totalAmt)

                    'Added by zoya 16/02/2016
                    creditAmt = creditAmt * -1

                    If amt > creditAmt Then
                        lblMsg.Visible = True
                        lblMsg.Text = "Amount Cannot be Greater than Credit Amount"
                        eflag = "no"
                        Exit Sub
                    End If
                    'end

                End If

            End If
        Next
    End Sub

    Protected Sub chk_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim chk As CheckBox
        Dim dgItem1 As DataGridItem
        For Each dgItem1 In dgView.Items
            chk = dgItem1.Cells(0).Controls(1)
            If chk.Checked = True Then
                LoadTotals()
            Else
                LoadTotals()
            End If
        Next
    End Sub

    Protected Sub ibtnPrint_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ibtnPrint.Click

    End Sub

    Protected Sub btnHidden_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnHidden.Click

    End Sub

    Private Sub OnSearchView()

        Session("loaddata") = "View"
        Session("PageMode") = "Edit"
        addBankCode()
        LoadListObjects()
        PostEnFalse()

        If CInt(Request.QueryString("IsView")).Equals(1) Then
            ddlpaymentfor.Enabled = False
            txtBDate.Enabled = False
            txtPaymentDate.Enabled = False
            ibtnSpn1.Attributes.Clear()
            ibtnstu.Attributes.Clear()

            Dim cb As CheckBox, txtAmt As TextBox, txtVoucher As TextBox, txtPctAmt As TextBox
            For Each dgItem As DataGridItem In dgView.Items
                cb = dgItem.Cells(0).Controls(1)
                txtAmt = dgItem.Cells(6).Controls(1)
                txtVoucher = dgItem.Cells(8).Controls(1)
                txtPctAmt = dgItem.Cells(9).Controls(1)

                cb.Enabled = False
                txtAmt.Enabled = False
                dgItem.Cells(7).Enabled = False
                txtVoucher.Enabled = False
                txtPctAmt.Enabled = False
            Next

        End If

    End Sub

#Region "GetApprovalDetails"

    Protected Function GetMenuId() As Integer

        Dim MenuId As Integer = New MenuDAL().GetMenuMasterList().Where(Function(x) x.PageName = "Student Payments").Select(Function(y) y.MenuID).FirstOrDefault()
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
