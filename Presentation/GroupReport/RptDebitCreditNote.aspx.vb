Imports HTS.SAS.BusinessObjects
Imports HTS.SAS.Entities
Imports System.Data
Imports System.Collections.Generic
Imports System.Data.SqlClient
Imports System.Web.Configuration
Imports CrystalDecisions.CrystalReports.Engine

Partial Class RptDebitCreditNote
    Inherits System.Web.UI.Page

    Private Sub Menuname(ByVal MenuId As Integer)
        Dim eobj As New MenuEn
        Dim bobj As New MenuBAL
        eobj.MenuId = MenuId
        eobj = bobj.GetMenus(eobj)
        lblMenuName.Text = eobj.MenuName
    End Sub
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack() Then
            Menuname(CInt(Request.QueryString("Menuid")))
            ibtnFDate.Attributes.Add("onClick", "return getibtnFDate()")
            ibtnTodate.Attributes.Add("onClick", "return getDateto()")

            'UnCommented By Zoya @26/02/2016
            ibtnPrint.Attributes.Add("onclick", "return getDate()")

            'Added By Zoya @26/02/2016
            'Commented by Zoya @9/03/2016
            'ibtnPrint.Attributes.Add("onclick", "new_window=window.open('../GroupReport/RptDebitCreditNoteViewer.aspx','Hanodale','width=520,height=400,resize=0,scrollbars=1');new_window.focus();")

            'Commented By Zoya @26/02/2016
            'ibtnBMReport.Attributes.Add("onclick", "return getDate()")
            'ibtnBMReport.Attributes.Add("onclick", "new_window=window.open('../GroupReport/RptDebitCreditNoteViewer.aspx','Hanodale','width=520,height=400,resize=0,scrollbars=1');new_window.focus();")
            'ibtnEnReport.Attributes.Add("onclick", "return getDate()")
            'ibtnEnReport.Attributes.Add("onclick", "new_window=window.open('../GroupReport/RptDebitCreditNoteViewerEn.aspx','Hanodale','width=520,height=400,resize=0,scrollbars=1');new_window.focus();")

            ibtnView.Attributes.Add("onclick", "return getDate()")
            txtFrom.Attributes.Add("OnKeyup", "return CheckFromDate()")
            txtTodate.Attributes.Add("OnKeyup", "return CheckToDate()")
            LoadUserRights()
            txtFrom.ReadOnly = True
            txtTodate.ReadOnly = True
            ibtnFDate.Visible = False
            ibtnTodate.Visible = False
            Semester()
            dates()

            'Editted by Zoya @9/03/2016
            'Session("program") = Nothing
            'Session("faculty") = Nothing
            'Session("sponsor") = Nothing
            Session("status") = Nothing
            Session("sesi") = Nothing
            Session("type") = Nothing

            'Added by Zoya @9/03/2016
            rdbtnstudid.Checked = True
            Session("sortby") = "SASI_MatricNo"

        End If
    End Sub
    Private Sub dates()
        txtFrom.Text = Format(Date.Now, "dd/MM/yyyy")
        txtTodate.Text = Format(Date.Now, "dd/MM/yyyy")
    End Sub
    Private Sub Semester()
        Dim eobj As New SemesterSetupBAL
        Dim eSemSetup As New SemesterSetupEn
        Try
            eSemSetup.SemisterSetupCode = ""
            eSemSetup.Description = ""
            eSemSetup.Status = True
            ddlSem.Items.Clear()
            ddlSem.Items.Add(New ListItem("---Select---", "-1"))

            'Editted by Zoya @8/03/2016
            'ddlSem.DataTextField = "Semester"
            'ddlSem.DataValueField = "Semester"
            ddlSem.DataTextField = "SemisterCode2"
            ddlSem.DataValueField = "SemisterSetupCode"
            'End Editted by Zoya @8/03/2016

            Try
                ddlSem.DataSource = eobj.GetListSemesterCur(eSemSetup)
            Catch ex As Exception
                LogError.Log("Student", "FillDropDownList", ex.Message)
            End Try
            ddlSem.DataBind()
        Catch ex As Exception

        End Try
    End Sub
    Private Sub LoadUserRights()

        Dim obj As New UsersBAL
        Dim eobj As UserRightsEn

        'eobj = obj.GetUserRights(5, 1)
        eobj = obj.GetUserRights(CInt(Request.QueryString("Menuid")), CInt(Session("UserGroup")))
        'Rights for Add
        If eobj.IsAdd = True Then
            ibtnSave.Enabled = True
            'OnAdd()
        Else
            ibtnNew.Enabled = False
            ibtnNew.ImageUrl = "../images/gAdd.png"
            ibtnNew.ToolTip = "Access Denied"
            ibtnDelete.Enabled = False
            ibtnDelete.ImageUrl = "../images/gdelete.png"
            ibtnDelete.ToolTip = "Access Denied"
            '-----------------------------------------------
            ibtnFirst.Enabled = False
            ibtnLast.Enabled = False
            ibtnPrevs.Enabled = False
            ibtnNext.Enabled = False
            ibtnFirst.ToolTip = "Access Denied"
            ibtnLast.ToolTip = "Access Denied"
            ibtnPrevs.ToolTip = "Access Denied"
            ibtnNext.ToolTip = "Access Denied"
            ibtnFirst.ImageUrl = "../images/gnew_first.png"
            ibtnLast.ImageUrl = "../images/gnew_last.png"
            ibtnPrevs.ImageUrl = "../images/gnew_Prev.png"
            ibtnNext.ImageUrl = "../images/gnew_next.png"
            '------------------------------------------------
            ibtnSave.Enabled = False
            ibtnSave.ImageUrl = "../images/gsave.png"
            ibtnSave.ToolTip = "Access Denied"
        End If
        'Rights for Edit
        If eobj.IsEdit = True Then
            'ibtnSave.ToolTip = "Access Denied"
            Session("EditFlag") = True
        Else
            Session("EditFlag") = False
        End If
        'Rights for View
        ibtnView.Enabled = eobj.IsView
        If eobj.IsView = True Then
            ibtnView.ImageUrl = "../images/find.png"
            ibtnView.Enabled = True
        Else
            ibtnView.ImageUrl = "../images/gfind.png"
            ibtnView.ToolTip = "Access Denied"
        End If

        'Commenetd and uncommenetd by Zoya @ 26/02/2016
        'Rights for Print
        ibtnPrint.Enabled = eobj.IsPrint
        If eobj.IsPrint = True Then
            'EnablePrintUserRights()
            ibtnPrint.Enabled = True
            ibtnPrint.ImageUrl = "../images/print.png"
            ibtnPrint.ToolTip = "Print"
        Else
            'DisablePrintUserRights()
            ibtnPrint.Enabled = False
            ibtnPrint.ImageUrl = "../images/gprint.png"
            ibtnPrint.ToolTip = "Access Denied"
        End If
        'End Commenetd and uncommenetd by Zoya @ 26/02/2016

        'Checking Default mode
        If eobj.IsAddModeDefault = True Then
            ' pnlView.Visible = False
            'pnlAdd.Visible = True
        Else
            'pnlAdd.Visible = False
            ' pnlView.Visible = True
            'LoadGrid()
        End If
        If eobj.IsOthers = True Then
            ibtnOthers.Enabled = True
            ibtnOthers.ImageUrl = "../images/others.png"
            ibtnOthers.ToolTip = "Others"
        Else
            ibtnOthers.Enabled = False
            ibtnOthers.ImageUrl = "../images/gothers.png"
            ibtnOthers.ToolTip = "Access Denied"
        End If
        If eobj.IsPost = True Then
            ibtnPosting.Enabled = True
            ibtnPosting.ImageUrl = "../images/posting.png"
            ibtnPosting.ToolTip = "Posting"
        Else
            ibtnPosting.Enabled = False
            ibtnPosting.ImageUrl = "../images/gposting.png"
            ibtnPosting.ToolTip = "Access Denied"
        End If
    End Sub

    Protected Sub ChkDateRange_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ChkDateRange.CheckedChanged
        If ChkDateRange.Checked = True Then
            txtFrom.ReadOnly = False
            txtTodate.ReadOnly = False
            ibtnFDate.Visible = True
            ibtnTodate.Visible = True
        Else
            txtFrom.ReadOnly = True
            txtTodate.ReadOnly = True
            ibtnFDate.Visible = False
            ibtnTodate.Visible = False
            dates()
        End If
    End Sub

    Protected Sub ibtnPrint_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)

    End Sub

    Protected Sub ibtnView_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)

    End Sub

    Protected Sub ibtnCancel_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ibtnCancel.Click
        ddlStuStatus.SelectedIndex = -1
        ChkDateRange.Checked = False
        'txtFrom.Text = ""
        'txtTodate.Text = ""

        'Commented by Zoya @9/03/2016
        'rdbtnstudid.Checked = False

        rdbtnstudname.Checked = False
        dates()
        Semester()
        txtFrom.ReadOnly = True
        txtTodate.ReadOnly = True
        ibtnFDate.Visible = False
        ibtnTodate.Visible = False

        'Added By Zoya @8/03/2016
        ddlType.SelectedValue = -1
        'End Added By Zoya @8/03/2016

        'Added by Zoya @9/03/2016
        rdbtnstudid.Checked = True
        Session("sortby") = "SASI_MatricNo"

    End Sub

    Protected Sub ibtnNew_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ibtnNew.Click
        ddlStuStatus.SelectedIndex = -1
        ChkDateRange.Checked = False
        'txtFrom.Text = ""
        'txtTodate.Text = ""
        rdbtnstudid.Checked = True
        rdbtnstudname.Checked = False

        dates()
        Semester()
        txtFrom.ReadOnly = True
        txtTodate.ReadOnly = True
        ibtnFDate.Visible = False
        ibtnTodate.Visible = False
        ddlType.SelectedValue = -1

        Session("status") = Nothing
        Session("sesi") = Nothing
        Session("type") = Nothing
        Session("sortby") = "SASI_MatricNo"

    End Sub


    Protected Sub ibtnDelete_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)

    End Sub

    Protected Sub rdbtnstudid_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)

        'Editted by Zoya @9/03/2016

        'If rdbtnstudid.Checked = True Then
        '    Session("sortby") = "SASI_MatricNo"
        'Else
        '    Session("sortby") = ""
        'End If

        If rdbtnstudid.Checked = True Then
            Session("sortby") = "SASI_MatricNo"
        End If
        'Done Editted by Zoya @9/03/2016

    End Sub

    Protected Sub rdbtnstudname_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)

        'Editted by Zoya @9/03/2016

        'If rdbtnstudname.Checked = True Then
        '    Session("sortby") = "SASI_Name"
        'Else
        '    Session("sortby") = ""
        'End If

        If rdbtnstudname.Checked = True Then
            Session("sortby") = "SASI_Name"
        End If
        'Done Editted by Zoya @9/03/2016

    End Sub

    Protected Sub ddlStuStatus_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim status As String
        status = ddlStuStatus.SelectedValue
        'Editted by Zoya @9/03/2016

        'If status = "-1" Then
        '    status = "%"
        '    Session("status") = Nothing
        'Else
        '    Session("status") = status
        'End If

        If status = "-1" Then
            Session("status") = status
        Else
            Session("status") = status
        End If
        'Done Editted by Zoya @9/03/2016

    End Sub
    Protected Sub ddlType_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim type As String
        type = ddlType.SelectedValue
        If type = "-1" Then
        Else
            Session("type") = type
        End If
    End Sub
    Protected Sub ddlSem_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim sesi As String
        sesi = ddlSem.SelectedValue
        If sesi = "-1" Then
        Else
            Session("sesi") = sesi
        End If
    End Sub
    Protected Sub txtFrom_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs)

    End Sub

    'Commented by Zoya @ 26/02/2016

    'Protected Sub EnablePrintUserRights()
    '    ibtnBMReport.Enabled = True
    '    ibtnEnReport.Enabled = True
    '    'ibtnPrint.ImageUrl = "../images/print.png"
    '    ibtnBMReport.ImageUrl = ""
    '    ibtnEnReport.ImageUrl = ""
    '    ibtnBMReport.ToolTip = "Print"
    '    ibtnEnReport.ToolTip = "Print"
    '    'ibtnPrint.ToolTip = "Print"
    'End Sub

    'Protected Sub DisablePrintUserRights()
    '    ibtnBMReport.Enabled = False
    '    ibtnEnReport.Enabled = False
    '    'ibtnPrint.ImageUrl = "../images/print.png"
    '    ibtnBMReport.ImageUrl = ""
    '    ibtnEnReport.ImageUrl = ""
    '    ibtnBMReport.ToolTip = "Access Denied"
    '    ibtnEnReport.ToolTip = "Access Denied"
    '    'ibtnPrint.ToolTip = "Access Denied"
    'End Sub
End Class
