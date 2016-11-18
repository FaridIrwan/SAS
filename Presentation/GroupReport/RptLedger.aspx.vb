Imports HTS.SAS.BusinessObjects
Imports HTS.SAS.Entities
Imports System.Data
Imports System.Collections.Generic
Partial Class RptLedger
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
            LoadUserRights()
            ibtnFDate.Attributes.Add("onClick", "return getibtnFDate()")
            ibtnTodate.Attributes.Add("onClick", "return getDateto()")
            'ibtnPrint.Attributes.Add("onclick", "new_window=window.open('RptStuLedgerCRForm.aspx','Hanodale','width=700,height=600,resizable=1');new_window.focus();")
            'ibtnBMReport.Attributes.Add("onclick" "new_window=window.open(../GroupReport/
            'ibtnView.Attributes.Add("onclick", "dllValues1()")
            'ibtnPrint.Attributes.Add("onclick", "dllValues1()")
            'ibtnView.Attributes.Add("onclick", "dllValues()")
            'ibtnPrint.Attributes.Add("onclick", "dllValues()")

            'Commented By Zoya @26/02/2016
            'ibtnBMReport.Attributes.Add("onclick", "return getDate()")
            'ibtnEnReport.Attributes.Add("onclick", "return getDateEn()")

            'Added By Zoya@26/02/2016
            ibtnPrint.Attributes.Add("onclick", "return getDate()")

            ibtnView.Attributes.Add("onclick", "return getDate()")
            txtFrom.Attributes.Add("OnKeyup", "return CheckFromDate()")
            txtTodate.Attributes.Add("OnKeyup", "return CheckToDate()")
            txtFrom.ReadOnly = True
            txtTodate.ReadOnly = True
            ibtnFDate.Visible = False
            ibtnTodate.Visible = False
            dates()
            Session("program") = Nothing
            Session("faculty") = Nothing
            Session("sponsor") = Nothing
            Session("status") = Nothing
            Session("sortby") = Nothing
            'rbtnStudentID.Checked = True
            rbtnStudentID.Visible = True
            rbtnstudname.Visible = True
            rbtnSponsorID.Visible = False
            rbtnSponsorName.Visible = False
        End If
        Session("sortby") = Nothing
    End Sub
    Private Sub dates()
        txtFrom.Text = Format(Date.Now, "dd/MM/yyyy")
        txtTodate.Text = Format(Date.Now, "dd/MM/yyyy")
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

    Protected Sub ChkDateRange_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        'If ChkDateRange.Checked Then
        '    txtFrom.Text = Date.Now
        '    txtTodate.Text = Date.Now
        '    Session("datefrom") = txtFrom.Text
        '    Session("dateto") = txtTodate.Text
        'Else
        '    txtFrom.Text = ""
        '    txtTodate.Text = ""
        'End If
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

    Protected Sub ibtnView_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)

    End Sub

    Protected Sub ibtnCancel_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        ddlLedgerType.SelectedIndex = -1
        ddlStuStatus.SelectedIndex = -1
        ChkDateRange.Checked = False
        rbtnStudentID.Checked = False
        rbtnstudname.Checked = False
        rbtnSponsorID.Checked = False
        rbtnSponsorName.Checked = False
        txtFrom.ReadOnly = False
        txtTodate.ReadOnly = True
        ibtnFDate.Visible = False
        ibtnTodate.Visible = False
        Session("sortby") = Nothing
        dates()
    End Sub

    Protected Sub rbtnStudentID_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        If rbtnStudentID.Checked = True Then
            Session("sortby") = "SAS_Student.SASI_PgId"
        Else
            Session("sortby") = ""
        End If
    End Sub


    Protected Sub rbtnstudname_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        If rbtnstudname.Checked = True Then
            Session("sortby") = "SAS_Student.SASI_Name"
        Else
            Session("sortby") = ""
        End If
    End Sub

    Protected Sub ddlStuStatus_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim status As String
        status = ddlStuStatus.SelectedValue
        If status = "-1" Then
            Session("status") = status
        Else
            Session("status") = status
        End If
    End Sub

    Protected Sub ddlLedgerType_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        If ddlLedgerType.SelectedValue = -1 Or ddlLedgerType.SelectedValue = 0 Then
            rbtnStudentID.Visible = True
            rbtnstudname.Visible = True
            'rbtnStudentID.Checked = True
            'rbtnstudname.Checked = False
            rbtnSponsorID.Visible = False
            rbtnSponsorName.Visible = False
            'Session("sortby") = "dbo.SAS_Student.SASI_PgId"
        Else
            rbtnStudentID.Visible = False
            rbtnstudname.Visible = False
            rbtnSponsorID.Visible = True
            rbtnSponsorName.Visible = True
            'rbtnSponsorID.Checked = True
            'rbtnSponsorName.Checked = False
            'Session("sortby") = "dbo.SAS_Sponsor.SASR_Code"
        End If
    End Sub

    Protected Sub rbtnSponsorID_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        If rbtnSponsorID.Checked = True Then
            Session("sortby") = "SAS_Sponsor.SASR_Code"
        Else
            Session("sortby") = ""
        End If
    End Sub

    Protected Sub rbtnSponsorName_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        If rbtnSponsorName.Checked = True Then
            Session("sortby") = "SAS_Sponsor.SASR_Name"
        Else
            Session("sortby") = ""
        End If
    End Sub

    'Protected Sub ibtnPrint_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ibtnPrint.Click

    'End Sub

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
