Imports HTS.SAS.Entities
Imports HTS.SAS.BusinessObjects
Imports HTS.SAS.DataAccessObjects
Imports System.Data
Imports System.Collections.Generic
Imports System.Data.SqlClient
Imports System.Linq

Partial Class AddApprover
    Inherits System.Web.UI.Page

    Dim MenuId As Integer, Batchcode As String

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        MenuId = Request.QueryString("MenuId")

        If Not IsPostBack() Then
            Session("CheckApproverList") = Nothing
            LoadTextBox()
            LoadApprovalList()
        End If

    End Sub

    Protected Sub LoadTextBox()
        Dim _MenuMasterEn As MenuMasterEn = New MenuDAL().GetMenuMasterList().FindAll(Function(x) x.MenuID = MenuId).FirstOrDefault()

        txtProcessName.Text = _MenuMasterEn.PageName
        txtTimeSendApproval.Text = DateTime.Now
        txtPreparedBy.Text = GetPreparedBy()
        txtPostBy.Text = Session(Helper.UserSession)

        hdnPageName.Value = _MenuMasterEn.PageName
    End Sub

    Protected Sub LoadApprovalList()

        Dim list As List(Of WorkflowSetupEn) = New WorkflowSetupDAL().GetApproverList().FindAll(Function(x) x.WorkflowApproverEn.MenuId = MenuId).ToList()
        Dim listUid As List(Of Long) = list.Select(Function(x) x.WorkflowApproverEn.UserId).ToList()

        If list.Count > 0 Then

            Dim listU As List(Of WorkflowSetupEn) = New WorkflowSetupDAL().GetUserList().Where(Function(x) listUid.Contains(x.UsersEn.UserID)).ToList()

            If listU.Count > 0 Then

                listU.Remove(listU.Where(Function(x) x.UsersEn.UserName = Session(Helper.UserSession)).FirstOrDefault())

                If listU.Count > 0 Then

                    If CheckForPreparer(listU) > 0 Then

                        gvUser.DataSource = listU
                        gvUser.DataBind()

                        For Each gv As GridViewRow In gvUser.Rows
                            gv.Cells(3).Text = IIf(True, "Active", "Inactive")
                        Next

                    Else
                        LoadNoApprovalList()
                    End If

                Else
                    LoadNoApprovalList()
                End If

            End If
        Else
            LoadNoApprovalList()
        End If

    End Sub

    Protected Sub LoadNoApprovalList()
        lblMsg.Visible = True
        lblMsg.Text = "No Approver Found For " & hdnPageName.Value & "`s Process"
    End Sub

    Protected Function CheckForPreparer(ByRef listU As List(Of WorkflowSetupEn)) As Integer

        Dim en As UsersEn = New UsersDAL().GetItem(New UsersEn With {.UserName = GetPreparedBy()})

        Dim PreparerId As Integer = New WorkflowSetupDAL().GetPreparerList().Where(Function(x) x.WorkflowPreparerEn.MenuId = MenuId _
                                                                                       And x.WorkflowPreparerEn.UserId = en.UserID).Select(Function(y) y.WorkflowPreparerEn.UserId).FirstOrDefault()
        If PreparerId <> 0 Then
            listU.Remove(listU.Where(Function(x) x.UsersEn.UserID = PreparerId).FirstOrDefault())
        End If

        Return listU.Count

    End Function

    Private Sub UpdateGrid()
        Dim cScript As String = ""

        Session("CheckApproverList") = True

        cScript = "<SCRIPT LANGUAGE=""JavaScript"">window.opener.document.getElementById('ctl00_ContentPlaceHolder1_btnHiddenApp').click();self.close();</SCRIPT>"
        ClientScript.RegisterStartupScript(Me.GetType(), "ClosePopUp", cScript)
    End Sub

    Protected Sub ibtnClose_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ibtnClose.Click

        Session("listWF") = Nothing
        UpdateGrid()

    End Sub

    Protected Sub ibtnOK_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ibtnOK.Click

        Dim listWF As New List(Of WorkflowSetupEn)
        Dim cb As CheckBox, lblUG As Label

        Try
            For Each gv As GridViewRow In gvUser.Rows

                cb = gv.Cells(0).Controls(1)
                lblUG = gv.Cells(2).Controls(1)

                If cb.Checked = True Then

                    Dim en As New WorkflowSetupEn

                    en.MenuMasterEn = New MenuMasterEn
                    en.MenuMasterEn.PageName = hdnPageName.Value
                    en.UsersEn = New UsersEn
                    en.UsersEn.UserName = gv.Cells(1).Text
                    en.UsersEn.UserStatus = gv.Cells(3).Text
                    en.UserGroupsEn = New UserGroupsEn
                    en.UserGroupsEn.UserGroupName = lblUG.Text
                    listWF.Add(en)

                End If

            Next

            If listWF.Count = 0 Then
                If gvUser.Rows.Count > 0 Then
                    Throw New Exception("Select at least one user to proceed.")
                Else
                    Session("listWF") = listWF
                End If
            Else
                Session("listWF") = listWF
            End If

            UpdateGrid()

        Catch ex As Exception
            lblMsg.Visible = True
            lblMsg.Text = ex.Message.ToString()
        End Try

    End Sub

    Public Function GetPreparedBy() As String

        Try
            Batchcode = Request.QueryString("Batchcode")
            If String.IsNullOrEmpty(Batchcode) Then
                Throw New Exception("Batch number not found or empty.")
            End If

            Dim en As New AccountsEn, createdby As String = ""
            en.BatchCode = Batchcode

            If Trim(txtProcessName.Text) = "Sponsor Invoice" Then
                createdby = New AccountsDAL().GetSponsorInvoiceItem(en).CreatedBy.ToString()
            Else
                createdby = New AccountsDAL().GetItem(en).CreatedBy.ToString()
            End If

            If String.IsNullOrEmpty(createdby) Then
                createdby = "unknown"
            End If

            Return createdby

        Catch ex As Exception
            lblMsg.Visible = True
            lblMsg.Text = ex.Message.ToString()

            Return "unknown"
        End Try

    End Function

End Class
