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
#End Region

#Region "Page_Load "

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Try

            If Not Page.IsPostBack Then

                Call BindGrid()

                Session("PageMode") = ""
                Session("ListObj") = Nothing
                DisableRecordNavigator()

                Menuname(CInt(Request.QueryString("Menuid")))

                lblRemarks.Visible = False
                txtRemarks.Visible = False
                btnApproved.Text = "Approve"

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

    Private Sub BindGrid()

        Dim lstHaveApprover As New List(Of WorkflowEn)

        Try

            Dim Approver As Integer = 0
            Session("Approver") = Approver

            Dim listWFSetup As List(Of WorkflowSetupEn) = New WorkflowSetupDAL().GetApproverList().Where(Function(x) x.WorkflowApproverEn.UserId = CInt(Session(Helper.UserIDSession))).ToList()
            Dim userid As Integer = listWFSetup.Select(Function(x) x.WorkflowApproverEn.UserId).FirstOrDefault()

            If userid <> 0 Then
                Dim listWF As List(Of WorkflowEn) = _WorkflowDAL.GetWorkflowDetails(userid)
                If listWF.Count > 0 Then

                    For Each obj As WorkflowEn In listWF

                        Dim _list As List(Of WorkflowSetupEn) = listWFSetup.Where(Function(x) x.WorkflowApproverEn.MenuId = obj.MenuMasterEn.MenuID).ToList()

                        If _list.Any(Function(x) x.WorkflowApproverEn.LowerLimit = 0 AndAlso x.WorkflowApproverEn.UpperLimit = 0) Then
                            lstHaveApprover.Add(obj)
                        Else

                            If _list.Any(Function(x) x.WorkflowApproverEn.LowerLimit = 0 AndAlso x.WorkflowApproverEn.UpperLimit <> 0) Then

                                If _list.Any(Function(x) obj.AccountsEn.TransactionAmount <= x.WorkflowApproverEn.UpperLimit) Then
                                    lstHaveApprover.Add(obj)
                                End If

                            ElseIf _list.Any(Function(x) x.WorkflowApproverEn.LowerLimit <> 0 AndAlso x.WorkflowApproverEn.UpperLimit = 0) Then

                                If _list.Any(Function(x) obj.AccountsEn.TransactionAmount >= x.WorkflowApproverEn.LowerLimit) Then
                                    lstHaveApprover.Add(obj)
                                End If

                            Else

                                If _list.Any(Function(x) obj.AccountsEn.TransactionAmount >= x.WorkflowApproverEn.LowerLimit AndAlso
                                                     obj.AccountsEn.TransactionAmount <= x.WorkflowApproverEn.UpperLimit) Then
                                    lstHaveApprover.Add(obj)
                                End If

                            End If

                        End If

                    Next
                End If
            End If

            If lstHaveApprover.Count > 0 Then
                dgWofkFlowView.DataSource = lstHaveApprover
                dgWofkFlowView.DataBind()

                viewbuttons.Visible = True
            Else
                lblMsg.Visible = True
                lblMsg.Text = "No Records"
                viewbuttons.Visible = False
            End If

            Dim link As HyperLink
            Dim str As String
            Dim cat As String
            Dim type As String
            For Each dgItem1 As GridViewRow In dgWofkFlowView.Rows

                link = dgItem1.Cells(9).Controls(1)
                str = dgItem1.Cells(2).Text
                cat = dgItem1.Cells(1).Text
                type = dgItem1.Cells(8).Text
                link.Attributes.Add("onClick", "OpenWindow('about:blank')")

                If cat = "AFC" Then
                    link.NavigateUrl = "FeePosting.aspx?Menuid=73&Formid=FS&BatchCode=" + str + ""
                    link.Target = "MyPopup"
                    'Receipt - Student
                ElseIf cat = "Receipt" And type = "Student" Then
                    link.NavigateUrl = "Receipts.aspx?Menuid=17&IsView=1&BatchCode=" + str + ";St"
                    link.Target = "MyPopup"
                    'Receipt - Sponsor
                ElseIf cat = "Receipt" And type = "Sponsor" Then
                    link.NavigateUrl = "Receipts.aspx?Menuid=17&IsView=1&BatchCode=" + str + ";Sp"
                    link.Target = "MyPopup"
                    'Invoice - Sponsor
                ElseIf cat = "Invoice" And type = "Sponsor" Then
                    link.NavigateUrl = "SponsorInvoice.aspx?Menuid=89&Formid=DN&IsView=1&BatchCode=" + str + ""
                    link.Target = "MyPopup"
                    'Debit Note - Sponsor
                ElseIf cat = "Debit Note" And type = "Sponsor" Then
                    link.NavigateUrl = "SponsorCreditNote.aspx?Menuid=22&Formid=DN&IsView=1&BatchCode=" + str + ""
                    link.Target = "MyPopup"
                    'Credit Note - Sponsor
                ElseIf cat = "Credit Note" And type = "Sponsor" Then
                    link.NavigateUrl = "SponsorCreditNote.aspx?Menuid=22&Formid=DN&IsView=1&BatchCode=" + str + ""
                    link.Target = "MyPopup"
                    'Invoice - Student
                ElseIf cat = "Invoice" And type = "Student" Then
                    link.NavigateUrl = "BatchInvoice.aspx?Menuid=14&Formid=Inv&IsView=1&BatchCode=" + str + ""
                    link.Target = "MyPopup"
                    'Debit Note - Student
                ElseIf cat = "Debit Note" And type = "Student" Then
                    link.NavigateUrl = "BatchInvoice.aspx?Menuid=15&Formid=DN&IsView=1&BatchCode=" + str + ""
                    link.Target = "MyPopup"
                    'Credit Note - Student
                ElseIf cat = "Credit Note" And type = "Student" Then
                    link.NavigateUrl = "BatchInvoice.aspx?Menuid=16&Formid=CN&IsView=1&BatchCode=" + str + ""
                    link.Target = "MyPopup"
                    'Student Advance
                ElseIf cat = "Loan" And type = "Student" Then
                    link.NavigateUrl = "StudentLoan.aspx?Menuid=88&IsView=1&BatchCode=" + str + ""
                    link.Target = "MyPopup"
                    'Sponsor Allocation
                ElseIf cat = "Allocation" And type = "Sponsor" Then
                    link.NavigateUrl = "SponsorAllocation.aspx?Menuid=25&IsView=1&BatchCode=" + str + ""
                    link.Target = "MyPopup"
                    'Sponsor Payment
                ElseIf cat = "Payment" And type = "Sponsor" Then
                    link.NavigateUrl = "SponsorPayments.aspx?Menuid=20&IsView=1&BatchCode=" + str + ""
                    link.Target = "MyPopup"
                    'Student Payment - Allocation
                ElseIf cat = "Payment" And type = "Student" Then
                    link.NavigateUrl = "Payments.aspx?Menuid=19&IsView=1&BatchCode=" + str + ";A"
                    link.Target = "MyPopup"
                    'Student Payment - Refund
                ElseIf cat = "Refund" And type = "Student" Then
                    link.NavigateUrl = "Payments.aspx?Menuid=19&IsView=1&BatchCode=" + str + ";St"
                    link.Target = "MyPopup"
                Else
                    link.NavigateUrl = "WorkFlowStudentAccountView.aspx?TransID=" + str + "&Category=" + cat + "&SubType=" + type
                    link.Target = "MyPopup"
                End If
            Next

        Catch ex As Exception

            MaxModule.Helper.LogError(ex.Message)

        End Try

    End Sub

#End Region

#Region "Pagination Grid "
    Protected Sub dgWofkFlowView_PageIndexChanged(source As Object, e As GridViewPageEventArgs) Handles dgWofkFlowView.PageIndexChanging

        dgWofkFlowView.PageIndex = e.NewPageIndex
        dgWofkFlowView.DataBind()

        Call BindGrid()

    End Sub
#End Region

#Region "Methods"

    Private Sub OnAdd()

        Session("PageMode") = "Add"
        OnClearData()

    End Sub

    Private Sub OnClearData()

        Session("ListObj") = Nothing

        DisableRecordNavigator()
        lblRemarks.Visible = False
        txtRemarks.Visible = False
        lblMsg.Text = ""

        Session("PageMode") = "Add"
    End Sub

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

#Region "Posting SAS Account Details "

    Protected Sub btnApproved_Click(sender As Object, e As EventArgs) Handles btnApproved.Click
        lblMsg.Visible = True
        lblMsg.Text = PostRejectSASAccounts(Generic._WorkflowType.Approved, "A")
    End Sub
#End Region

#Region "Reject SAS Account Details "

    Protected Sub btnReject_Click(sender As Object, e As EventArgs) Handles btnReject.Click

        lblRemarks.Visible = True
        txtRemarks.Visible = True
        btnApproved.Visible = False

        If txtRemarks.Text.Length > 0 Then

            lblMsg.Visible = True
            lblMsg.Text = PostRejectSASAccounts(Generic._WorkflowType.Rejected, "A")
        Else
            lblMsg.Text = "Please put the remark of rejection."
        End If

    End Sub
#End Region

#Region "Post/Reject Records "

    Public Function PostRejectSASAccounts(ByVal WorkflowType As Short, ByVal ProcessRole As String) As String

        Dim _CheckBox As CheckBox
        Dim _Posting As New MaxModule.Workflow
        Dim _HtmlInputHidden As HtmlInputHidden
        Dim accObj As New AccountsEn, afcObj As New AFCEn

        Dim SuccessStatus As Short = 0, WorkflowId As Integer = 0, Result As Boolean = False, RejectStatus As Boolean = False
        Dim txtRemark As String, WorkflowStatus As Integer = 0
        Dim ReviewToApprove As Boolean = False, PendingReview As Boolean = False, PendingApprove As Boolean = False
        Dim PostingResult As String = Nothing, pageURL As String
        Dim splist As New List(Of SponsorEn)

        Try
            For Each _DataGridItem As GridViewRow In dgWofkFlowView.Rows

                _CheckBox = _DataGridItem.Cells(0).Controls(1)
                _HtmlInputHidden = _DataGridItem.Cells(9).Controls(2).FindControl("Hidden_WorkFlowId")

                If _CheckBox.Checked = True Then

                    WorkflowId = clsGeneric.NullToInteger(_HtmlInputHidden.Value)

                    accObj.BatchCode = _DataGridItem.Cells(2).Text
                    afcObj.BatchCode = _DataGridItem.Cells(2).Text
                    accObj.TransType = _DataGridItem.Cells(1).Text
                    pageURL = _DataGridItem.Cells(7).Text
                    accObj.SubType = _DataGridItem.Cells(8).Text

                    Select Case WorkflowType

                        Case Generic._WorkflowType.Approved

                            txtRemark = ""

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

                                        If accObj.TransType = "Loan" And pageURL = "ASP.studentloan_aspx" Then
                                            Call _AccountsDAL.UpdatePostingStatusStudentLoan(accObj.BatchCode, Session("User"))
                                        ElseIf accObj.TransType = "AFC" Then
                                            Call _AFCDAL.UpdateAFCReference(accObj.BatchCode)
                                            Call _AccountsDAL.UpdatePostingStatus(accObj.BatchCode, Session("User"))
                                        ElseIf accObj.TransType = "Allocation" Then
                                            Call _AccountsDAL.UpdatePostingStatusClosed(accObj.BatchCode, Session("User"))
                                            Call _AccountsDAL.SponsorBatchPost(accObj.BatchCode, Session("User"))
                                        ElseIf accObj.TransType = "Payment" And accObj.SubType = "Sponsor" Then
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

                                        Call _WorkflowDAL.UpdateWorkflow(WorkflowId, Generic._WorkflowType.Posted, txtRemark, ProcessRole, Session("User"))

                                        _CheckBox.Checked = False
                                    Else
                                        Call _WorkflowDAL.DeleteReviewApprove(WorkflowId, Generic._WorkflowType.Approved, ProcessRole, Session("User"))
                                    End If

                                End If

                            ElseIf _WorkflowDAL.GetWorkflowProcess(WorkflowId, ProcessRole) < Session("Approver") Then

                                If WorkflowStatus > -1 Then
                                    _CheckBox.Checked = False
                                    PendingApprove = True
                                Else
                                End If

                            End If

                        Case Generic._WorkflowType.Rejected

                            txtRemark = Trim(txtRemarks.Text)

                            Call _WorkflowDAL.UpdateWorkflow(WorkflowId, Generic._WorkflowType.Rejected, txtRemark, ProcessRole, Session("User"))

                            If accObj.TransType = "AFC" Then
                                Call _AccountsDAL.BatchDelete(accObj, True)
                                RejectStatus = _AFCDAL.AFCBatchDelete(afcObj)

                            ElseIf accObj.TransType = "Loan" And pageURL = "ASP.studentloan_aspx" Then
                                Call _MaxModule.ResetPostingStatusToReadyStudentLoan(accObj.BatchCode)
                                RejectStatus = True
                            Else
                                Call _MaxModule.ResetPostingStatusToReady(accObj.BatchCode)
                                RejectStatus = True
                            End If

                            If RejectStatus Then
                                _CheckBox.Checked = False
                            End If

                    End Select

                End If

            Next

            If WorkflowType = Generic._WorkflowType.Approved Then

                If ProcessRole = "A" Then

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

        Call BindGrid()

        Return PostingResult

    End Function

#End Region

End Class
