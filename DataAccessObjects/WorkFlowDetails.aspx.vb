#Region "NameSpaces "

Imports MaxGeneric
Imports System.Data
Imports System.Collections.Generic
Imports HTS.SAS.BusinessObjects
Imports HTS.SAS.DataAccessObjects
Imports HTS.SAS.Entities
Imports System.Web
Imports System.Linq

#End Region

Partial Class WorkFlowDetails
    Inherits System.Web.UI.Page

#Region "Global Declarations "
    Private _WorkflowDAL As New HTS.SAS.DataAccessObjects.WorkflowDAL
    Private _AccountsDAL As New HTS.SAS.DataAccessObjects.AccountsDAL
    Private _AFCDAL As New HTS.SAS.DataAccessObjects.AFCDAL
    Private _MaxModule As New MaxModule.Workflow
    '    Private _WorkflowSetupDAL As New HTS.SAS.DataAccessObjects.WorkflowSetupDAL
#End Region

#Region "Page_Load "

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Try

            If Not Page.IsPostBack Then

                hidUserRole.Value = Session(Helper.WorkflowRole)
                'Call Bind Grid
                Call BindGrid(hidUserRole.Value)

                'While loading the page make the CFlag as null
                Session("PageMode") = ""
                'Loading User Rights
                'LoadUserRights()

                'while loading list object make it nothing
                Session("ListObj") = Nothing
                DisableRecordNavigator()
                'load PageName
                Menuname(CInt(Request.QueryString("Menuid")))

                lblRemarks.Visible = False
                txtRemarks.Visible = False

                If hidUserRole.Value = "R" Then
                    btnApproved.Text = "Review"
                ElseIf hidUserRole.Value = "A" Then
                    btnApproved.Text = "Approve"
                End If
            End If


        Catch ex As Exception

            Call MaxModule.Helper.LogError(ex.Message)

        End Try

    End Sub
#End Region

#Region "New"
    Protected Sub ibtnNew_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ibtnNew.Click
        OnAdd()
    End Sub
#End Region

#Region "Cancel"
    Protected Sub ibtnCancel_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ibtnCancel.Click
        OnClearData()
    End Sub
#End Region

#Region "Bind Grid "

    Private Sub BindGrid(ByVal WorkflowRole As String)

        Dim lstHaveApprover As New List(Of WorkflowEn)
        Dim lstNoApprover As New List(Of WorkflowEn)
        Try

            Dim listWFSetup As List(Of WorkflowSetupEn) = New WorkflowSetupDAL().GetApproverList().Where(Function(x) x.WorkflowApproverEn.UserId = CInt(Session(Helper.UserIDSession))).ToList()
            Dim userid As Integer = listWFSetup.Select(Function(x) x.WorkflowApproverEn.UserId).FirstOrDefault()

            If userid <> 0 Then
                Dim listWF As List(Of WorkflowEn) = _WorkflowDAL.GetWorkflowDetails(userid)
                If listWF.Count > 0 Then

                    For Each obj As WorkflowEn In listWF
                        lstHaveApprover.Add(obj)
                    Next

                End If
            End If

            'find details that have no approver list - start
            Dim menuid As List(Of Long) = listWFSetup.Select(Function(x) x.WorkflowApproverEn.MenuId).ToList()
            Dim listMenu As List(Of MenuMasterEn) = New MenuDAL().GetMenuMasterList().Where(Function(x) menuid.Contains(x.MenuID)).ToList()

            For Each MenuEn As MenuMasterEn In listMenu

                Dim listWF As List(Of WorkflowEn) = _WorkflowDAL.GetWorkflowDetailsNoApprover(MenuEn.PageName)
                If listWF.Count > 0 Then

                    For Each obj As WorkflowEn In listWF
                        lstNoApprover.Add(obj)
                    Next

                End If

            Next
            'find details that have no approver list - end

            lstHaveApprover.AddRange(lstNoApprover)

            If lstHaveApprover.Count > 0 Then
                dgWofkFlowView.DataSource = lstHaveApprover
                dgWofkFlowView.DataBind()
            End If

            ''Reviewer = _WorkflowSetupDAL.GetTotalReviewer()
            'Session("Reviewer") = Reviewer
            ''Approver = _WorkflowSetupDAL.GetTotalApprover()
            'Session("Approver") = Approver

            'If Reviewer = 0 Then

            '    If WorkflowRole = "A" Then
            '        'get WorkFlowAccount Details
            '        WorkFlowAccountDetails = _WorkflowDAL.WorkFlowSASAcountDetails()

            '    ElseIf WorkflowRole = "R" Then
            '        lblMsg.Visible = True
            '        lblMsg.Text = "No Reviewer in Workflow Setup"

            '    End If

            'ElseIf Approver = 0 Then
            '    If WorkflowRole = "A" Then
            '        lblMsg.Visible = True
            '        lblMsg.Text = "No Approver in Workflow Setup"
            '    End If
            'End If

            'If Reviewer > 0 Then

            '    If WorkflowRole = "R" Then
            '        'get WorkFlowAccount Details
            '        WorkFlowAccountDetails = _WorkflowDAL.WorkFlowSASAcountDetails()

            '    ElseIf WorkflowRole = "A" Then
            '        'get WorkFlowAccount Details
            '        WorkFlowAccountDetails = _WorkflowDAL.WorkFlowSASApproveDetails()
            '    End If

            'End If

            ''if data available - Start
            'If WorkFlowAccountDetails.Tables(0).Rows.Count = 0 Then
            '    lblMsg.Visible = True
            '    lblMsg.Text = "No Records"
            '    viewbuttons.Visible = False
            'Else
            '    viewbuttons.Visible = True
            'End If
            ''if data available - Start

            ''Bind Data grid - Start
            'dgWofkFlowView.DataSource = WorkFlowAccountDetails
            'dgWofkFlowView.DataBind()
            ''Bind Data grid - Stop

            ''Dim link As New HyperLink
            ''Dim i As Integer = 0

            ''For Each dgItem1 In dgWofkFlowView.Items
            ''    link = dgItem1.Cells(9).Controls(1)
            ''    link.Attributes.Add("onClick", "OpenWindow('about:blank')")
            ''    link.NavigateUrl = "WorkFlowStudentAccountView.aspx?TransID=" & dgItem1.Cells(2).Text & "&Category=" & dgItem1.Cells(1).Text & "&SubType=" & dgItem1.Cells(8).Text
            ''    link.Target = "MyPopup"
            ''    i += 1
            ''Next

            'Dim dgItem1 As DataGridItem
            'Dim link As HyperLink
            'Dim str As String
            'Dim cat As String
            'Dim type As String
            'For Each dgItem1 In dgWofkFlowView.Items
            '    link = dgItem1.Cells(9).Controls(1)
            '    str = dgItem1.Cells(2).Text
            '    cat = dgItem1.Cells(1).Text
            '    type = dgItem1.Cells(8).Text
            '    link.Attributes.Add("onClick", "OpenWindow('about:blank')")

            '    If cat = "Invoice" And type = "Student" Then
            '        link.NavigateUrl = "BatchInvoice.aspx?Menuid=14&Formid=Inv&IsView=1&BatchCode=" + str + ""
            '        link.Target = "MyPopup"
            '        'ElseIf cat = "Credit Note" And type = "Sponsor" Then
            '        '    link.NavigateUrl = "SponsorCreditNote.aspx?Menuid=21&Formid=CN&BatchCode=" + str + ""
            '        '    link.Target = "MyPopup"
            '        'ElseIf cat = "Debit Note" And type = "Sponsor" Then
            '        '    link.NavigateUrl = "SponsorCreditNote.aspx?Menuid=22&Formid=DN&BatchCode=" + str + ""
            '        '    link.Target = "MyPopup"
            '        'ElseIf cat = "Invoice" And type = "Sponsor" Then
            '        '    link.NavigateUrl = "SponsorInvoice.aspx?Menuid=89&Formid=Inv&IsView=1&BatchCode=" + str + ""
            '        '    link.Target = "MyPopup"
            '        'ElseIf cat = "Loan" Then
            '        '    link.NavigateUrl = "../StudentLoan.aspx?Menuid=88&BatchCode=" + str + ";St"
            '        '    link.Target = "MyPopup"
            '    ElseIf cat = "Payment" And type = "Student" Then
            '        link.NavigateUrl = "Payments.aspx?Menuid=19&IsView=1&BatchCode=" + str + ";A"
            '        link.Target = "MyPopup"
            '        'ElseIf cat = "Refund" Then
            '        '    link.NavigateUrl = "../Payments.aspx?Menuid=19&IsView=1&BatchCode=" + str + ";St"
            '        '    link.Target = "MyPopup"
            '        'ElseIf cat = "SPA" Then
            '        '    link.NavigateUrl = "../SponsorAllocation.aspx?Menuid=25&BatchCode=" + str + ""
            '        '    link.Target = "MyPopup"
            '        'ElseIf cat = "AFC" Then
            '        '    link.NavigateUrl = "WorkFlowStudentAccountView.aspx?TransID=" + str + "&Category=" + cat + "&SubType=" + type
            '        '    link.Target = "MyPopup"
            '    ElseIf cat = "Credit Note" And type = "Student" Then
            '        link.NavigateUrl = "BatchInvoice.aspx?Menuid=16&Formid=CN&IsView=1&BatchCode=" + str + ""
            '        link.Target = "MyPopup"
            '    ElseIf cat = "Debit Note" And type = "Student" Then
            '        link.NavigateUrl = "BatchInvoice.aspx?Menuid=15&Formid=DN&IsView=1&BatchCode=" + str + ""
            '        link.Target = "MyPopup"
            '        'ElseIf cat = "STA" Then
            '        '    link.NavigateUrl = "../Payments.aspx?Menuid=19&&BatchCode=" + str + ";A"
            '        '    link.Target = "MyPopup"
            '    Else
            '        link.NavigateUrl = "WorkFlowStudentAccountView.aspx?TransID=" + str + "&Category=" + cat + "&SubType=" + type
            '        link.Target = "MyPopup"
            '    End If
            'Next

        Catch ex As Exception

            MaxModule.Helper.LogError(ex.Message)

        End Try

    End Sub

#End Region

#Region "Pagination Grid "
    Protected Sub dgWofkFlowView_PageIndexChanged(source As Object, e As DataGridPageChangedEventArgs) Handles dgWofkFlowView.PageIndexChanged

        'Grid Index - Start
        dgWofkFlowView.PageIndex = e.NewPageIndex
        dgWofkFlowView.DataBind()

        'Bind Data grid - Start
        Call BindGrid(hidUserRole.Value)
        'Bind Data grid - Stop

    End Sub
#End Region

#Region "Methods"

    ''' <summary>
    ''' Method to Add new page
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub OnAdd()
        Session("PageMode") = "Add"
        OnClearData()
    End Sub

    ''' <summary>
    ''' Method to Clear data
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub OnClearData()

        Session("ListObj") = Nothing

        DisableRecordNavigator()
        lblRemarks.Visible = False
        txtRemarks.Visible = False
        lblMsg.Text = ""

        Session("PageMode") = "Add"
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
            LogError.Log("WorkFlowDetails", "LoadUserRights", ex.Message)
        End Try
        'Rights for Add

        If eobj.IsAdd = True Then
            'ibtnSave.Enabled = True

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
            LogError.Log("UniversityFund", "Menuname", ex.Message)
        End Try
        lblMenuName.Text = eobj.MenuName
    End Sub

#End Region

#Region "Grid Item "
    'Protected Sub dgWofkFlowView_ItemCommand(source As Object, e As DataGridCommandEventArgs)
    '    'GridItem declaration
    '    Dim gditem As DataGridItem
    '    'LinkButton delclaration
    '    Dim _LinkButton As New LinkButton
    '    Dim TransId As Integer = 0

    '    Try

    '        gditem = DirectCast(DirectCast(e.CommandSource, LinkButton).NamingContainer, DataGridItem)

    '        If e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem Then

    '            _LinkButton = DirectCast(e.Item.FindControl("studentlink"), LinkButton)
    '            _LinkButton.PostBackUrl = "WorkFlowStudentAccountView.aspx?TransID=" + _LinkButton.CommandArgument.ToString()
    '            _LinkButton.OnClientClick = "window.document.forms[0].target='_blank';"

    '        End If

    '    Catch ex As Exception
    '        Call MaxModule.Helper.LogError(ex.Message)
    '    End Try
    'End Sub
#End Region

#Region "Posting SAS Account Details "
    Protected Sub btnApproved_Click(sender As Object, e As EventArgs) Handles btnApproved.Click
        'Call Post SAS Accounts - Starting
        lblMsg.Visible = True
        lblMsg.Text = PostRejectSASAccounts(Generic._WorkflowType.Approved, hidUserRole.Value)
        'Call Post SAS Accounts - Ended

        ''Call Bind Grid
        'Call BindGrid(hidUserRole.Value)

    End Sub
#End Region

#Region "Reject SAS Account Details "
    Protected Sub btnReject_Click(sender As Object, e As EventArgs) Handles btnReject.Click

        'Remark rejection to be key-in
        lblRemarks.Visible = True
        txtRemarks.Visible = True
        btnApproved.Visible = False

        If txtRemarks.Text.Length > 0 Then

            'Call Reject SAS Accounts - Starting
            lblMsg.Visible = True

            lblMsg.Text = PostRejectSASAccounts(Generic._WorkflowType.Rejected, hidUserRole.Value)
            'Call Reject SAS Accounts - Ended
        Else
            lblMsg.Text = "Please put the remark of rejection."
        End If

        'Call Bind Grid
        'Call BindGrid(hidUserRole.Value)

    End Sub
#End Region

#Region "Post/Reject Records "

    Public Function PostRejectSASAccounts(ByVal WorkflowType As Short, ByVal ProcessRole As String) As String

        'Create Instaces - Start
        Dim _CheckBox As CheckBox
        Dim _DataGridItem As DataGridItem
        Dim _Posting As New MaxModule.Workflow
        Dim _HtmlInputHidden As HtmlInputHidden
        'Create Instaces - Stop

        Dim accObj As New AccountsEn, afcObj As New AFCEn

        'varaible C declaration
        Dim SuccessStatus As Short = 0, WorkflowId As Integer = 0, Result As Boolean = False, RejectStatus As Boolean = False
        Dim txtRemark As String, WorkflowStatus As Integer = 0
        Dim pageURL As String, subtype As String
        Dim ReviewToApprove As Boolean = False, PendingReview As Boolean = False, PendingApprove As Boolean = False
        Dim PostingResult As String = Nothing
        Dim eobj As AccountsEn
        Dim splist As New List(Of SponsorEn)
        Try
            'Looping Grid Items - Strating
            For Each _DataGridItem In dgWofkFlowView.Rows

                'Get Controls - Start
                _CheckBox = _DataGridItem.Cells(0).Controls(1)
                _HtmlInputHidden = _DataGridItem.Cells(9).Controls(2).FindControl("Hidden_WorkFlowId")
                'Get Controls - Stop

                'Check Box Check -Starting
                If _CheckBox.Checked = True Then

                    'get Work Flow id
                    WorkflowId = clsGeneric.NullToInteger(_HtmlInputHidden.Value)

                    'get BatchCode
                    accObj.BatchCode = _DataGridItem.Cells(2).Text
                    afcObj.BatchCode = _DataGridItem.Cells(2).Text
                    accObj.TransType = _DataGridItem.Cells(1).Text
                    pageURL = _DataGridItem.Cells(7).Text
                    'subtype = _DataGridItem.Cells(8).Text
                    'edited by 10032016 by farid
                    accObj.SubType = _DataGridItem.Cells(8).Text

                    Select Case WorkflowType

                        Case Generic._WorkflowType.Approved

                            txtRemark = ""

                            If ProcessRole = "R" Then

                                'Update workflow process
                                'Call _WorkflowDAL.UpdateWorkflow(WorkflowId, Generic._WorkflowType.Reviewed, txtRemark, Session(Helper.WorkflowRole))

                                'Update Workflow Status - Reviewer/Approver
                                Call _WorkflowDAL.UpdateReviewApprove(WorkflowId, Generic._WorkflowType.Reviewed, ProcessRole, Session("User"))

                                If _WorkflowDAL.GetWorkflowProcess(WorkflowId, ProcessRole) >= Session("Reviewer") Then

                                    WorkflowStatus = _WorkflowDAL.UpdateWorkflow(WorkflowId, Generic._WorkflowType.Reviewed, txtRemark, ProcessRole, Session("User"))

                                    If WorkflowStatus > -1 Then
                                        _CheckBox.Checked = False
                                        'Return "Record Sent for Approval Successfully"
                                        ReviewToApprove = True
                                    Else
                                        'Return "Record Sent for Approval Failed"
                                        'Exit Select
                                    End If

                                ElseIf _WorkflowDAL.GetWorkflowProcess(WorkflowId, ProcessRole) < Session("Reviewer") Then

                                    If WorkflowStatus > -1 Then
                                        _CheckBox.Checked = False
                                        'Return "Record Reviewed Successfully but Pending Reviewer Group"
                                        PendingReview = True
                                    Else
                                        'Return "Record Reviewed Failed"
                                        'Exit Select
                                    End If

                                End If

                            ElseIf ProcessRole = "A" Then

                                'Update Workflow Status - Reviewer/Approver
                                Call _WorkflowDAL.UpdateReviewApprove(WorkflowId, Generic._WorkflowType.Approved, ProcessRole, Session("User"))

                                If _WorkflowDAL.GetWorkflowProcess(WorkflowId, ProcessRole) >= Session("Approver") Then

                                    If accObj.TransType = "Invoice" And accObj.SubType = "Sponsor" Then
                                        Call _AccountsDAL.UpdatePostingStatusSponsorInvoice(accObj.BatchCode, Session("User"))
                                        Result = True
                                        Call _WorkflowDAL.UpdateWorkflow(WorkflowId, Generic._WorkflowType.Posted, txtRemark, ProcessRole, Session("User"))
                                        _CheckBox.Checked = False
                                    Else

                                        'Post to CF
                                        Result = _Posting.PostToCenturyFinancials(WorkflowId)

                                        If Result Then

                                            'Update Posted -Account Details
                                            If accObj.TransType = "Loan" And pageURL = "ASP.studentloan_aspx" Then
                                                Call _AccountsDAL.UpdatePostingStatusStudentLoan(accObj.BatchCode, Session("User"))
                                            ElseIf accObj.TransType = "AFC" Then
                                                Call _AFCDAL.UpdateAFCReference(accObj.BatchCode)
                                                Call _AccountsDAL.UpdatePostingStatus(accObj.BatchCode, Session("User"))
                                            ElseIf accObj.TransType = "Allocation" Then
                                                'added by farid 01032016
                                                Call _AccountsDAL.UpdatePostingStatusClosed(accObj.BatchCode, Session("User"))
                                                Call _AccountsDAL.SponsorBatchPost(accObj.BatchCode, Session("User"))
                                            ElseIf accObj.TransType = "Payment" And accObj.SubType = "Sponsor" Then
                                                'added by farid 01032016
                                                Call _AccountsDAL.UpdatePostingStatusClosed(accObj.BatchCode, Session("User"))
                                                Call _AccountsDAL.SponsorBatchPost(accObj.BatchCode, Session("User"))
                                            ElseIf accObj.TransType = "Payment" And accObj.SubType = "Student" Then
                                                Call _AccountsDAL.UpdatePostingStatusClosed(accObj.BatchCode, Session("User"))
                                                Call _AccountsDAL.Subcategoryupdate(accObj.BatchCode, Session("User"))
                                            ElseIf accObj.TransType = "Credit Note" And accObj.SubType = "Student" Then
                                                Call _AccountsDAL.UpdatePostingStatus(accObj.BatchCode, Session("User"))
                                                Call _AccountsDAL.UpdatePostingStatusForAFCFLAGreverse(accObj.BatchCode)
                                            Else
                                                Call _AccountsDAL.UpdatePostingStatus(accObj.BatchCode, Session("User"))
                                            End If

                                            'Update Status
                                            Call _WorkflowDAL.UpdateWorkflow(WorkflowId, Generic._WorkflowType.Posted, txtRemark, ProcessRole, Session("User"))
                                            _CheckBox.Checked = False
                                        Else
                                            Call _WorkflowDAL.DeleteReviewApprove(WorkflowId, Generic._WorkflowType.Approved, ProcessRole, Session("User"))
                                        End If

                                    End If

                                ElseIf _WorkflowDAL.GetWorkflowProcess(WorkflowId, ProcessRole) < Session("Approver") Then

                                    If WorkflowStatus > -1 Then
                                        _CheckBox.Checked = False
                                        'Return "Record Approved Successfully but Pending Approval Group
                                        PendingApprove = True
                                    Else
                                        'Return "Record Approved Failed"
                                        'Exit Select
                                    End If

                                End If

                            End If

                        Case Generic._WorkflowType.Rejected

                            txtRemark = Trim(txtRemarks.Text)

                            'Update Status
                            Call _WorkflowDAL.UpdateWorkflow(WorkflowId, Generic._WorkflowType.Rejected, txtRemark, ProcessRole, Session("User"))

                            ''Update AFC Status-Held
                            'Call _AccountsDAL.BatchDelete(accObj, True)

                            'RejectStatus = _AFCDAL.AFCBatchDelete(afcObj)
                            'If RejectStatus Then
                            '    _CheckBox.Checked = False
                            'End If

                            If accObj.TransType = "AFC" Then
                                'Update AFC Status-Held
                                Call _AccountsDAL.BatchDelete(accObj, True)
                                RejectStatus = _AFCDAL.AFCBatchDelete(afcObj)

                            ElseIf accObj.TransType = "Loan" And pageURL = "ASP.studentloan_aspx" Then
                                'RejectStatus = _MaxModule.ResetPostingStatusToReadyStudentLoan(accObj.BatchCode)
                                Call _MaxModule.ResetPostingStatusToReadyStudentLoan(accObj.BatchCode)
                                RejectStatus = True

                                'ElseIf accObj.TransType = "Invoice" And accObj.SubType = "Sponsor" Then

                                'ElseIf accObj.TransType = "Allocation" Then

                                'ElseIf accObj.TransType = "Payment" And accObj.SubType = "Sponsor" Then

                                'ElseIf accObj.TransType = "Payment" And accObj.SubType = "Student" Then

                            Else
                                'RejectStatus = _MaxModule.ResetPostingStatusToReady(accObj.BatchCode)
                                Call _MaxModule.ResetPostingStatusToReady(accObj.BatchCode)
                                RejectStatus = True
                            End If

                            If RejectStatus Then
                                _CheckBox.Checked = False
                            End If

                    End Select

                End If
                'Check Box Check -Ended

            Next
            'Looping Grid Items - Ended    

            If WorkflowType = Generic._WorkflowType.Approved Then

                If ProcessRole = "R" Then

                    If PendingReview Then
                        PostingResult = "Record Reviewed Successfully but Pending Reviewer Group"
                    Else
                        PostingResult = "Record Reviewed Failed"
                    End If

                    If ReviewToApprove Then
                        PostingResult = "Record Sent for Approval Successfully"
                    Else
                        PostingResult = "Record Sent for Approval Failed"
                    End If

                ElseIf ProcessRole = "A" Then

                    If PendingApprove Then
                        PostingResult = "Record Approved Successfully but Pending Approval Group"
                    Else
                        PostingResult = "Record Approved Failed"
                    End If

                    If Result Then
                        If accObj.TransType = "Invoice" And accObj.SubType = "Sponsor" Then
                            PostingResult = "Record Approved"
                        Else
                            PostingResult = "Record Approved and Posting to CF Successful"
                        End If
                    Else
                        PostingResult = "Posting to CF Failed"
                    End If

                End If

            End If

            If WorkflowType = Generic._WorkflowType.Rejected Then

                If RejectStatus Then
                    txtRemarks.Text = ""
                    lblRemarks.Visible = False
                    txtRemarks.Visible = False
                    PostingResult = "Rejection Is Successful"
                Else
                    txtRemarks.Text = ""
                    lblRemarks.Visible = False
                    txtRemarks.Visible = False
                    PostingResult = "Rejection Is Failed"
                End If

            End If

        Catch ex As Exception

            Call MaxModule.Helper.LogError(ex.Message)
            Return Nothing

        End Try

        'Call Bind Grid
        Call BindGrid(hidUserRole.Value)

        Return PostingResult

    End Function

#End Region

End Class
