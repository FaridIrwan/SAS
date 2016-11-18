Imports HTS.SAS.Entities
Imports HTS.SAS.BusinessObjects
Imports HTS.SAS.DataAccessObjects
Imports System.Data
Imports System.Collections.Generic
Imports System.Linq

Partial Class StudentLoan
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
        If Not IsPostBack() Then
            'Adding validation for save,Delete,search & others buttons
            ibtnSave.Attributes.Add("onclick", "return Validate()")
            ibtnDelete.Attributes.Add("onclick", "return getconfirm()")
            'Added by Zoya @6/04/2016
            Total.Attributes.Add("onKeypress", "isNumberKey(event)")
            'Done Added by Zoya @6/04/2016
            ibtnOthers.Attributes.Add("onclick", "return CheckSearch()")
            ibtnPrint.Attributes.Add("onclick", "return getPrint()")
            'Loading User Rights
            LoadUserRights()
            Session("PageMode") = "Add"
            Session("Menuid") = Request.QueryString("Menuid")
            MenuId.Value = GetMenuId()
            txtBDate.Attributes.Add("OnKeyup", "return CheckBDate()")
            txtPaymentDate.Attributes.Add("OnKeyup", "return CheckTdate()")
            txtDueDate.Attributes.Add("OnKeyup", "return CheckDueDate()")
            'While loading the page make the CFlag as null
            txtRecNo.Attributes.Add("OnKeyup", "return geterr()")
            txtBatchid.Text = "Auto Number"
            'while loading list object make it nothing
            Session("ListObj") = Nothing
            Session("liststu") = Nothing
            Session("stu") = Nothing
            DisableRecordNavigator()
            'load PageName
            Menuname(CInt(Request.QueryString("Menuid")))
            ibtnDelete.Attributes.Add("onClick", "return getconfirm()")
            ibtnPosting.Attributes.Add("onClick", "return getpostconfirm()")
            ibtnBDate.Attributes.Add("onClick", "return BDate()")
            dates()
            ibtnPaymentDate.Attributes.Add("onClick", "return getpaymentDate()")
            ibtnDueDate.Attributes.Add("onClick", "return getDate2from()")
            lblMsg.Text = ""
            ibtnstu.Attributes.Add("onclick", "new_window=window.open('AddMulStudents.aspx?cat=St','Hanodale','width=550,height=550,resizable=0');new_window.focus();")
            'ibtnPosting.Attributes.Add("onclick", "new_window=window.open('AddApprover.aspx?MenuId=" & GetMenuId() & "','Hanodale','width=500,height=400,resizable=0');new_window.focus();")
            'ibtnSpn1.Attributes.Add("onclick", "window.showModalDialog('AddMulStudents.aspx','Hanodale','width=520,height=500,resizable=0');")
            Session("eobjstu") = Nothing
            BindUniversityFund()
            Session("statusPrint") = Nothing
        End If

        If Not Session("eobjstu") Is Nothing Then
            AddStuCode()
        End If

        lblMsg.Visible = False

        If Not Session("CheckApproverList") Is Nothing Then
            SendToApproval()
        End If

        If Not Request.QueryString("BatchCode") Is Nothing Then
            Dim str As String = Request.QueryString("BatchCode")
            Dim constr As String() = str.Split(";")
            txtBatchid.Text = constr(0)

            DirectCast(Master.FindControl("Panel1"), System.Web.UI.WebControls.Panel).Visible = False
            DirectCast(Master.FindControl("td"), System.Web.UI.HtmlControls.HtmlTableCell).Visible = False
            Panel1.Visible = False
            OnSearchOthers()
        End If
    End Sub
    Protected Overloads Sub Page_LoadComplete(ByVal sender As Object, ByVal e As EventArgs) Handles Me.LoadComplete

    End Sub
#Region "Methods"

    ''' <summary>
    ''' Method to Add Students 
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub AddStuCode()
        Dim eobjf As StudentEn
        eobjf = Session("eobjstu")
        txtSudentId.Text = eobjf.MatricNo
        'Added by Zoya @6/04/2016
        txtSudentId.ReadOnly = True
        'Done added by Zoya @6/04/2016
        txtStudentName.ReadOnly = True
        txtStudentName.Text = eobjf.StudentName
        txtICNo.ReadOnly = True
        txtICNo.Text = eobjf.ICNo
    End Sub

    ''' <summary>
    ''' Method to Change the Date Format
    ''' </summary>
    ''' <remarks>Date in ddd/mm/yyyy Format</remarks>
    Private Sub dates()

        txtPaymentDate.Text = Format(Date.Now, "dd/MM/yyyy")
        txtBDate.Text = Format(Date.Now, "dd/MM/yyyy")
        txtDueDate.Text = Format(DateAdd(DateInterval.Day, 30, Date.Now), "dd/MM/yyyy")
    End Sub
    ''' <summary>
    ''' Method to Add Bankcode to Dropdown
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub BindUniversityFund()
        Dim eobjF As New UniversityFundEn
        Dim bsobj As New UniversityFundBAL
        Dim list As New List(Of UniversityFundEn)
        eobjF.UniversityFundCode = ""
        eobjF.Description = ""
        eobjF.GLCode = ""
        eobjF.Status = True
        ddlFund.Items.Clear()
        ddlFund.Items.Add(New ListItem("---Select---", "-1"))
        ddlFund.DataTextField = "Description"
        ddlFund.DataValueField = "UniversityFundCode"

        Try
            list = bsobj.GetUniversityFundList(eobjF)
        Catch ex As Exception
            LogError.Log("Student Loan", "addBankCode", ex.Message)
        End Try
        Session("funde") = list
        ddlFund.DataSource = list
        ddlFund.DataBind()
    End Sub
    ''' <summary>
    ''' Method to Validate
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub SpaceValidation()
        ibtnSave.Attributes.Add("onclick", "return Validate()")
        Dim GBFormat As System.Globalization.CultureInfo
        GBFormat = New System.Globalization.CultureInfo("en-GB")

        'Description
        If Trim(txtDescri.Text).Length = 0 Then
            txtDescri.Text = Trim(txtDescri.Text)
            lblMsg.Text = "Enter Valid Description "
            lblMsg.Visible = True
            txtDescri.Focus()
            Exit Sub
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

        'Due date
        If Trim(txtDueDate.Text).Length < 10 Then
            lblMsg.Text = "Enter Valid Due Date"
            lblMsg.Visible = True
            txtDueDate.Focus()
            Exit Sub
        Else
            Try
                txtDueDate.Text = DateTime.Parse(txtDueDate.Text, GBFormat)
            Catch ex As Exception
                lblMsg.Text = "Enter Valid Due Date"
                lblMsg.Visible = True
                txtDueDate.Focus()
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
    ''' Method to Save and Update Payments 
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub onSave()
        Dim eobj As New AccountsEn
        Dim bsobj As New AccountsBAL
        Dim list As New List(Of AccountsDetailsEn)
        Dim eobstu As New StudentEn
        lblMsg.Text = ""
        lblMsg.Visible = True

        eobj.SubReferenceOne = ddlFund.SelectedValue ' University Fund Id
        eobj.PayeeName = Trim(txtPayeeName.Text)
        eobj.Description = Trim(txtDescri.Text)
        eobj.CreditRef = Trim(txtSudentId.Text)
        eobj.Description = Trim(txtDescri.Text)
        eobj.BatchCode = Trim(txtBatchid.Text)
        eobj.BatchDate = Trim(txtBDate.Text)
        eobj.TransDate = Trim(txtPaymentDate.Text)
        eobj.SubType = "Student"
        eobj.Category = "Loan"
        eobj.TransType = "Credit"
        eobj.PostStatus = "Ready"
        eobj.TransStatus = "Open"
        eobj.TransactionAmount = CDbl(Total.Text)
        eobj.BatchTotal = CDbl(Total.Text)

        'Modified by Mona 19/2/2016
        'eobj.PostedDateTime = DateTime.Now
        'eobj.UpdatedTime = DateTime.Now
        'eobj.CreatedDateTime = DateTime.Now
        'eobj.ChequeDate = DateTime.Now
        eobj.DueDate = Trim(txtDueDate.Text)
        eobj.PostedDateTime = Format(Now(), "yyyy-MM-dd")
        eobj.UpdatedTime = Format(Now(), "yyyy-MM-dd")
        eobj.CreatedBy = DoneBy()
        eobj.CreatedDateTime = Format(Now(), "yyyy-MM-dd")
        eobj.ChequeDate = Format(Now(), "yyyy-MM-dd")

        eobj.UpdatedBy = Session("User")

        lblMsg.Visible = True
        If Session("PageMode") = "Add" Then
            Try
                txtBatchid.Text = bsobj.StudentLoanInsert(eobj)
                txtBatchid.ReadOnly = True
                ErrorDescription = "Record Saved Successfully "
                ibtnStatus.ImageUrl = "images/ready.gif"
                lblStatus.Value = "Ready"
                lblMsg.Text = ErrorDescription
                ibtnSave.Enabled = False

            Catch ex As Exception
                lblMsg.Text = ex.Message.ToString()
                LogError.Log("Student Loan", "onSave", ex.Message)
            End Try
        ElseIf Session("PageMode") = "Edit" Then
            Try
                txtBatchid.Text = bsobj.StudentLoanUpdate(eobj, eobstu)
                txtBatchid.ReadOnly = False
                ErrorDescription = "Record Updated Successfully "
                ibtnStatus.ImageUrl = "images/ready.gif"
                lblStatus.Value = "Ready"
                lblMsg.Text = ErrorDescription
                ibtnSave.Enabled = False

            Catch ex As Exception
                lblMsg.Text = ex.Message.ToString()
                LogError.Log("Student Loan", "onSave", ex.Message)
            End Try

        End If
        'setDateFormat()
    End Sub
    ''' <summary>
    ''' Method to Post Payments
    ''' </summary>
    ''' <remarks></remarks>
    Private Function OnPost() As Boolean

        'Create Instances
        Dim result As Boolean = False
        Dim _Helper As New Helper

        Try

            If _Helper.PostToWorkflow(txtBatchid.Text, Session(Helper.UserSession), Me.ToString(), "StudentLoan") Then
                result = True
                lblMsg.Visible = True
                'lblStatus.Value = "Posted"
                'ibtnStatus.ImageUrl = "images/posted.gif"
                lblMsg.Text = "Record Posted Successfully for Approval"
            Else
                result = False
                lblMsg.Visible = True
                'lblStatus.Value = "Posted"
                lblMsg.Text = "Record Already Posted"
            End If

        Catch ex As Exception

            lblMsg.Text = ex.Message.ToString()
            Call MaxModule.Helper.LogError(ex.Message)

        End Try

        Return result

        'Dim eobj As New AccountsEn
        'Dim bsobj As New AccountsBAL
        'Dim eobstu As New StudentEn
        'lblMsg.Text = ""
        'lblMsg.Visible = True
        '' University Fund Id
        'eobj.SubReferenceOne = ddlFund.SelectedValue
        'eobj.PayeeName = Trim(txtPayeeName.Text)
        'eobj.Description = Trim(txtDescri.Text)
        'eobj.CreditRef = Trim(txtSudentId.Text)
        'eobj.Description = Trim(txtDescri.Text)
        'eobj.BatchCode = Trim(txtBatchid.Text)
        'eobj.BatchDate = Trim(txtBDate.Text)
        'eobj.TransDate = Trim(txtPaymentDate.Text)
        'eobj.SubType = "Student"
        'eobj.Category = "Loan"
        'eobj.TransType = "Credit"
        'eobj.PostStatus = "Posted"
        'eobj.TransStatus = "Open"
        'eobj.TransactionAmount = CDbl(Total.Text)
        'eobj.BatchTotal = CDbl(Total.Text)
        'eobj.PostedDateTime = DateTime.Now
        'eobj.UpdatedTime = DateTime.Now
        'eobj.DueDate = Trim(txtDueDate.Text)
        'eobj.CreatedDateTime = DateTime.Now
        'eobj.ChequeDate = DateTime.Now
        'eobj.UpdatedBy = Session("User")

        'If Not Session("eobjstu") Is Nothing Then
        '    eobstu = Session("eobjstu")
        'End If

        'Dim bid As String = ""
        'lblMsg.Visible = True
        ''Status=New
        'Try
        '    txtBatchid.Text = bsobj.StudentLoanUpdate(eobj, eobstu)
        '    ErrorDescription = "Record Posted to Workflow Successfully "
        '    txtBatchid.ReadOnly = True
        '    ibtnStatus.ImageUrl = "images/posted.gif"
        '    lblMsg.Visible = True
        '    lblMsg.Text = ErrorDescription
        '    lblStatus.Value = "Posted"
        '    eobj.PostStatus = "Posted"

        '    lblMsg.Text = ErrorDescription
        'Catch ex As Exception
        '    lblMsg.Text = ex.Message.ToString()
        '' LogError.Log("Payments", "OnPost", ex.Message)
        ' End Try

    End Function


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
            ibtnOthers.Enabled = True
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
        OnClearData()
        If ibtnNew.Enabled = False Then
            ibtnSave.Enabled = False
            ibtnSave.ImageUrl = "images/gsave.png"
            ibtnSave.ToolTip = "Access Denied"
        End If
        If Session("PageMode") = "Add" Then
            BindUniversityFund()
            txtBatchid.ReadOnly = False
            txtBatchid.Text = "Auto Number"
            txtBatchid.ReadOnly = True
            txtBDate.Text = Format(Date.Now, "dd/MM/yyyy")
            txtPaymentDate.Text = Format(Date.Now, "dd/MM/yyyy")
        End If
        today.Value = Now.Date
        today.Value = Format(CDate(today.Value), "dd/MM/yyyy")
        txtDueDate.Text = Format(DateAdd(DateInterval.Day, 30, Date.Now), "dd/MM/yyyy")
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
        txtPayeeName.Text = ""
        txtSudentId.Text = ""
        lblMsg.Text = ""
        ddlFund.SelectedValue = "-1"
        txtDescri.Text = ""
        dates()
        Total.Text = 0.0
        txtStudentName.Text = ""
        txtICNo.Text = ""
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
        Dim obj As New AccountsBAL
        Dim Mylist As New List(Of AccountsEn)

        If Session("loaddata") = "others" Then

            eob.TransType = "Credit"
            eob.Category = "Loan"

            'added by Hafiz Roslan @ 6/2/2016
            'fixed issue regarding student ledger viewed incorrect data - START
            If Not Request.QueryString("BatchCode") Is Nothing Then
                Dim str As String = Request.QueryString("BatchCode")
                Dim constr As String() = str.Split(";")
                txtBatchid.Text = constr(0)
            End If
            'fixed issue regarding student ledger viewed incorrect data - END

            If txtBatchid.Text <> "Auto Number" Then
                eob.BatchCode = txtBatchid.Text
            Else
                eob.BatchCode = ""
            End If
            eob.PostStatus = "Posted"
            eob.SubType = "Student"

            If CInt(Request.QueryString("IsView")).Equals(1) Then
                eob.PostStatus = "Ready"
            End If

            Try
                ListObjects = obj.GetLoanTransactions(eob)
            Catch ex As Exception
                LogError.Log("StudentLoan", "LoadListObjects", ex.Message)
            End Try

        ElseIf Session("loaddata") = "View" Then
            eob.TransType = "Credit"
            eob.Category = "Loan"

            If txtBatchid.Text <> "Auto Number" Then
                eob.BatchCode = txtBatchid.Text
            Else
                eob.BatchCode = ""
            End If
            eob.PostStatus = "Ready"
            eob.SubType = "Student"

            Try
                ListObjects = obj.GetLoanTransactions(eob)
            Catch ex As Exception
                LogError.Log("StudentLoan", "LoadListObjects", ex.Message)
            End Try

        End If
        Session("loaddata") = Nothing

        'Removing duplicate batchid object from the list
        Dim i As Integer = 0
        If Not ListObjects Is Nothing Then
            While i < ListObjects.Count
                Dim j As Integer = 0
                Dim objcount As Boolean = False
                While j < Mylist.Count
                    If Mylist(j).BatchCode = ListObjects(i).BatchCode Then
                        objcount = True
                        Exit While
                    End If
                    j = j + 1
                End While
                If objcount = False Then
                    Mylist.Add(ListObjects(i))
                End If
                i = i + 1
            End While
        End If
        ListObjects = Nothing
        ListObjects = Mylist
        Session("ListObj") = ListObjects

        lblCount.Text = ListObjects.Count.ToString()
        If ListObjects.Count <> 0 Then
            Session("BatchNo") = Nothing
            txtBatchid.Enabled = True
            Session("BatchNo") = txtBatchid.Text
            txtBatchid.Enabled = False
            'Enable Navigation
            DisableRecordNavigator()
            txtRecNo.Text = "1"
            If lblStatus.Value <> "Posted" Then
                OnMoveFirst()
            End If
            If Session("EditFlag") = True Then
                lblMsg.Visible = True
                txtBatchid.Enabled = False
                ibtnSave.Enabled = True
                ibtnSave.ImageUrl = "images/save.png"
            Else
                Session("PageMode") = ""
                ibtnSave.Enabled = False
                ibtnSave.ImageUrl = "images/gsave.png"
            End If
            'DisableRecordNavigator()
            'txtRecNo.Text = "1"

            'OnMoveFirst()
            'If Session("EditFlag") = True Then
            '    Session("PageMode") = "Edit"
            '    txtBatchid.Enabled = False
            'Else
            '    Session("PageMode") = ""
            '    ibtnSave.Enabled = False
            '    ibtnSave.ImageUrl = "images/gsave.png"
            'End If
        Else
            txtRecNo.Text = ""
            lblCount.Text = ""
            ibtnStatus.ImageUrl = "images/notready.gif"
            lblStatus.Value = "New"
            Session("ListObj") = Nothing
            txtBatchid.Text = "Auto Number"
            DisableRecordNavigator()
            txtPayeeName.Text = ""
            txtSudentId.Text = ""
            ddlFund.SelectedValue = "-1"
            txtDescri.Text = ""
            dates()
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
        End If


        Dim eobj As AccountsEn
        Dim sobj As New StudentEn
        Dim studentBAL As New StudentBAL

        ListObjects = Session("ListObj")
        eobj = ListObjects(RecNo)
        txtPayeeName.ReadOnly = False
        txtSudentId.ReadOnly = False
        txtPayeeName.Text = eobj.PayeeName
        txtSudentId.Text = eobj.CreditRef
        sobj.MatricNo = Trim(txtSudentId.Text)

        ' Fill the Student Name and IC Number
        sobj = studentBAL.GetItem(sobj.MatricNo)
        txtStudentName.Text = sobj.StudentName
        txtICNo.Text = sobj.ICNo

        ' txtPayeeName.ReadOnly = True
        ' txtSudentId.ReadOnly = True
        ddlFund.Items.Clear()
        BindUniversityFund()

        'updated by Hafiz Roslan @ 6/2/2016
        Try
            If Not ddlFund.Items.FindByValue(eobj.SubReferenceOne.Trim) Is Nothing Then
                ddlFund.SelectedValue = eobj.SubReferenceOne.Trim
            End If

        Catch ex As Exception
            lblMsg.Text = ex.Message.ToString()
        End Try

        txtDescri.Text = eobj.Description
        txtBatchid.Text = eobj.BatchCode
        txtPaymentDate.Text = eobj.TransDate
        txtBDate.Text = eobj.BatchDate
        txtDueDate.Text = eobj.DueDate
        Total.Text = String.Format("{0:F}", eobj.TransactionAmount)

        If eobj.PostStatus = "Ready" Then
            lblStatus.Value = "Ready"
            ibtnStatus.ImageUrl = "images/Ready.gif"
        End If
        If eobj.PostStatus = "Posted" Then
            lblStatus.Value = "Posted"
            ibtnStatus.ImageUrl = "images/Posted.gif"
        End If

        CheckWorkflowStatus(eobj)
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
        Dim myDuedate As Date = CDate(CStr(txtDueDate.Text))
        Dim myFormattedDate1 As String = Format(myDuedate, myFormat)
        txtDueDate.Text = myFormattedDate1

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
                RecAff = bsobj.BatchLoanDelete(eob)
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
        ibtnPrint.Enabled = False
        ibtnPrint.ImageUrl = "images/gprint.png"
        ibtnPrint.ToolTip = "Access denied"
        ibtnPosting.Enabled = False
        ibtnPosting.ImageUrl = "images/gposting.png"
        ibtnPosting.ToolTip = "Access denied"

    End Sub
    ''' <summary>
    ''' Method to Search for Posted Records
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub OnSearchOthers()
        LoadUserRights()
        Session("loaddata") = "others"
        If lblCount.Text <> "" Then
            If CInt(lblCount.Text) > 0 Then
                onAdd()
            Else
                Session("PageMode") = "Edit"
                BindUniversityFund()
                LoadListObjects()

            End If
        Else
            Session("PageMode") = "Edit"
            BindUniversityFund()
            LoadListObjects()

            PostEnFalse()
        End If

        If CInt(Request.QueryString("IsView")).Equals(1) Then
            ddlFund.Enabled = False
            txtBDate.Enabled = False
            txtPaymentDate.Enabled = False
            txtDueDate.Enabled = False
            ibtnstu.Attributes.Clear()
        End If

    End Sub
#End Region

    'Protected Sub ibtnPosting_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ibtnPosting.Click

    '    If lblStatus.Value = "New" Then
    '        lblMsg.Text = "Record not ready for Posting"
    '        lblMsg.Visible = True
    '    ElseIf lblStatus.Value = "Posted" Then
    '        lblMsg.Text = "Record already Posted"
    '        lblMsg.Visible = True
    '    ElseIf lblStatus.Value = "Ready" Then
    '        SpaceValidation()
    '        OnPost()
    '        setDateFormat()

    '        ''Create Instances
    '        'Dim _Helper As New Helper

    '        'Try

    '        '    If _Helper.PostToWorkflow(txtBatchid.Text,
    '        '        Session(Helper.UserSession), Me.ToString()) Then
    '        '        lblMsg.Visible = True
    '        '        lblStatus.Value = "Posted"
    '        '        ibtnStatus.ImageUrl = "images/posted.gif"
    '        '        lblMsg.Text = "Posted to workflow successfully"
    '        '    Else
    '        '        lblMsg.Visible = True
    '        '        lblStatus.Value = "Posted"
    '        '        '  lblMsg.Text = "Posting to workflow Failed"
    '        '    End If

    '        'Catch ex As Exception

    '        '    lblMsg.Text = ex.Message.ToString()
    '        '    Call MaxModule.Helper.LogError(ex.Message)

    '        'End Try

    '    End If

    'End Sub

#Region "SendToApproval"

    Protected Sub SendToApproval()

        Try
            If lblStatus.Value = "New" Then
                lblMsg.Text = "Record not ready for Posting"
                lblMsg.Visible = True
            ElseIf lblStatus.Value = "Posted" Then
                lblMsg.Text = "Record already Posted"
                lblMsg.Visible = True
            ElseIf lblStatus.Value = "Ready" Then

                If Not Session("listWF") Is Nothing Then
                    Dim list As List(Of WorkflowSetupEn) = Session("listWF")
                    If list.Count > 0 Then
                        lblMsg.Text = ""

                        SpaceValidation()
                        setDateFormat()

                        If OnPost() = True Then
                            If Session("listWF").count > 0 Then
                                WorkflowApproverList(Trim(txtBatchid.Text), Session("listWF"))
                            End If
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
            LogError.Log("Receipts", "InsertWFApprovalList", ex.Message)
        End Try

    End Sub

#End Region

    Protected Sub ibtnSave_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ibtnSave.Click
        SpaceValidation()
        onSave()
        setDateFormat()
    End Sub
    Protected Sub ibtnView_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ibtnView.Click
        LoadUserRights()
        Session("loaddata") = "View"
        If lblCount.Text <> "" Then
            If CInt(lblCount.Text) > 0 Then
                onAdd()
            Else

                Session("PageMode") = "Edit"
                BindUniversityFund()
                LoadListObjects()
                Session("statusPrint") = "Ready"
            End If
        Else
            Session("PageMode") = "Edit"
            BindUniversityFund()
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

    '    If lblStatus.Value = "New" Then
    '        lblMsg.Text = "Record not Ready for Posting"
    '        lblMsg.Visible = True
    '    ElseIf lblStatus.Value = "Posted" Then
    '        lblMsg.Text = "Record Already Posted"
    '        lblMsg.Visible = True
    '    ElseIf lblStatus.Value = "Ready" Then
    '        SpaceValidation()
    '        OnPost()
    '        setDateFormat()
    '    End If

    'End Sub

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

    Protected Sub ibtnOthers_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ibtnOthers.Click
        OnSearchOthers()
    End Sub

    Protected Sub ibtnDelete_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ibtnDelete.Click
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

    Protected Sub ibtnPrint_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ibtnPrint.Click

    End Sub

    Protected Sub btnHidden_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnHidden.Click

    End Sub

#Region "GetApprovalDetails"

    Protected Function GetMenuId() As Integer

        Dim MenuId As Integer = New MenuDAL().GetMenuMasterList().Where(Function(x) x.PageName = "Student Advance").Select(Function(y) y.MenuID).FirstOrDefault()
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
