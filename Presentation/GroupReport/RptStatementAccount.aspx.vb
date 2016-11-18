Imports HTS.SAS.BusinessObjects
Imports HTS.SAS.Entities
Imports System.Data
Imports System.Collections.Generic
Imports System.Data.SqlClient
Imports System.Web.Configuration

Partial Class RptStatementAccount
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

            'Modified by Hafiz @ 07/3/2016

            'UnCommented By Zoya @26/02/2016
            ibtnPrint.Attributes.Add("onclick", "return getDate()")

            'Added By Zoya @26/02/2016
            'ibtnPrint.Attributes.Add("onclick", "new_window=window.open('../GroupReport/RptStatementAccountViewer.aspx','Hanodale','width=520,height=400,resize=0,scrollbars=1');new_window.focus();")

            'Commented By Zoya @26/02/2016
            'ibtnBMReport.Attributes.Add("onclick", "return getDate()")
            'ibtnBMReport.Attributes.Add("onclick", "new_window=window.open('../GroupReport/RptStatementAccountViewer.aspx','Hanodale','width=520,height=400,resize=0,scrollbars=1');new_window.focus();")
            'ibtnEnReport.Attributes.Add("onclick", "return getDate()")
            'ibtnEnReport.Attributes.Add("onclick", "new_window=window.open('../GroupReport/RptStatementAccountViewerEn.aspx','Hanodale','width=520,height=400,resize=0,scrollbars=1');new_window.focus();")

            ibtnView.Attributes.Add("onclick", "return getDate()")
            txtFrom.Attributes.Add("OnKeyup", "return CheckFromDate()")
            txtTodate.Attributes.Add("OnKeyup", "return CheckToDate()")
            LoadUserRights()
            txtFrom.ReadOnly = True
            txtTodate.ReadOnly = True
            ibtnFDate.Visible = False
            ibtnTodate.Visible = False
            Faculty()
            Sponsor()
            Program()
            dates()
            'Session("program") = Nothing
            'Session("faculty") = Nothing
            'Session("sponsor") = Nothing
            'Session("status") = Nothing
            'Session("sortby") = Nothing
            'Session("DunningNo") = Nothing
            LoadUserRights()
            Faculty()
            Program()
            Sponsor()
            'DunningLetter()

            'Added by Zoya @6/04/2016
            'ChkBoxStatementAc.Checked = True
            'Done Added by Zoya @6/04/2016
        End If

    End Sub
    Private Sub dates()
        txtFrom.Text = Format(Date.Now, "dd/MM/yyyy")
        txtTodate.Text = Format(Date.Now, "dd/MM/yyyy")
    End Sub
    Private Sub Faculty()
        Dim ObjFacultyEn As New FacultyEn
        Dim ObjFacultyBAL As New FacultyBAL
        Dim LstObjFaculty As New List(Of FacultyEn)
        ObjFacultyEn.SAFC_Code = "%"
        LstObjFaculty = ObjFacultyBAL.GetList(ObjFacultyEn)
        ddlFaculty.Items.Clear()
        ddlFaculty.Items.Add(New ListItem("-- Select --", "-1"))

        ddlFaculty.DataTextField = "SAFC_Desc"
        ddlFaculty.DataValueField = "SAFC_Code"
        ddlFaculty.DataSource = LstObjFaculty
        ddlFaculty.DataBind()
    End Sub
    Private Sub Program()
        Dim ObjProgramEn As New ProgramInfoEn
        Dim ObjProgramBAL As New ProgramInfoBAL
        Dim LstProgram As New List(Of ProgramInfoEn)
        ObjProgramEn.SAFC_Code = ddlFaculty.SelectedValue
        LstProgram = ObjProgramBAL.GetProgramInfoListAll(ObjProgramEn.SAFC_Code)
        ddlProgram.Items.Clear()
        ddlProgram.Items.Add(New ListItem("-- Select --", "-1"))
        ddlProgram.DataSource = LstProgram
        ddlProgram.DataTextField = "Program"
        ddlProgram.DataValueField = "ProgramCode"
        ddlProgram.DataBind()
    End Sub
    Private Sub Sponsor()
        Dim eobjSponsorEn As New SponsorEn
        Dim objSponsorBs As New SponsorBAL
        Dim listSponsor As New List(Of SponsorEn)
        listSponsor = objSponsorBs.GetList(eobjSponsorEn)
        ddlSponsor.Items.Clear()
        ddlSponsor.Items.Add(New ListItem("-- Select --", "-1"))
        ddlSponsor.DataSource = listSponsor
        ddlSponsor.DataTextField = "Name"
        ddlSponsor.DataValueField = "SponserCode"
        ddlSponsor.DataBind()
    End Sub
    'Private Sub DunningLetter()
    '    Dim eobjDunningEn As New DunningLettersEn
    '    Dim objDunningBs As New DunningLettersBAL
    '    Dim listDunning As New List(Of DunningLettersEn)
    '    eobjDunningEn.Code = ""
    '    eobjDunningEn.Title = ""
    '    listDunning = objDunningBs.GetList(eobjDunningEn)
    '    ddlDunningNo.Items.Clear()
    '    ddlDunningNo.Items.Add(New ListItem("-- Select --", "-1"))
    '    ddlDunningNo.DataSource = listDunning
    '    ddlDunningNo.DataTextField = "Title"
    '    ddlDunningNo.DataValueField = "Code"
    '    ddlDunningNo.DataBind()
    'End Sub

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


    Protected Sub ddlFaculty_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        Program()
        Dim faculty As String
        faculty = ddlFaculty.SelectedValue
        If faculty = "-1" Then
            faculty = "%"
            Session("faculty") = faculty
            Session("program") = faculty
        Else
            Session("faculty") = faculty
        End If
    End Sub

    Protected Sub ChkDateRange_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)
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

    Protected Sub ddlStuStatus_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim status As String
        status = ddlStuStatus.SelectedValue
        If status = "-1" Then
            Session("status") = Nothing
        Else
            Session("status") = status
        End If
    End Sub

    Protected Sub ddlProgram_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim program As String
        program = ddlProgram.SelectedValue
        If program = "-1" Then
            program = "%"
            Session("program") = program
        Else
            Session("program") = program
        End If
    End Sub

    Protected Sub ddlSponsor_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim sponsor As String
        sponsor = ddlSponsor.SelectedValue
        If sponsor = "-1" Then
            sponsor = "%"
            Session("sponsor") = sponsor
        Else
            Session("sponsor") = sponsor
        End If
    End Sub


    Protected Sub rdbtnstudentid_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        If rdbtnstudentid.Checked = True Then
            Session("sortby") = "t.SASI_MatricNo"
        Else
            Session("sortby") = ""
        End If
    End Sub

    Protected Sub rdbtnstudentname_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        If rdbtnstudentname.Checked = True Then
            Session("sortby") = "t.SASI_Name"
        Else
            Session("sortby") = ""
        End If
    End Sub

    'Protected Sub rdbtnsponsor_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)
    '    If rdbtnsponsor.Checked = True Then
    '        Session("sortby") = "t.Sponsor"
    '    Else
    '        Session("sortby") = ""
    '    End If
    'End Sub

    'Protected Sub rdbtnpostcod_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)
    '    If rdbtnpostcod.Checked = True Then
    '        Session("sortby") = "t.SASI_Postcode"
    '    Else
    '        Session("sortby") = ""
    '    End If
    'End Sub

    'Protected Sub ddlDunningNo_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs)
    '    Session("DunningNo") = ddlDunningNo.SelectedValue
    'End Sub

    Protected Sub ibtnCancel_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        ddlStuStatus.SelectedValue = -1
        ddlFaculty.SelectedValue = -1
        ddlProgram.SelectedValue = -1
        ddlSponsor.SelectedValue = -1
        rdbtnstudentid.Checked = False
        rdbtnstudentname.Checked = False
        'rdbtnsponsor.Checked = False
        'rdbtnpostcod.Checked = False
        ChkDateRange.Checked = False
        'Editted by Zoya @6/04/2016
        'ChkBoxStatementAc.Checked = True
        'Done Editted by Zoya @6/04/2016
        'ChkBoxAcInfo.Checked = False
        'ChkBoxDunningLetter.Checked = False
        txtFrom.ReadOnly = True
        txtTodate.ReadOnly = True
        ibtnFDate.Visible = False
        ibtnTodate.Visible = False
        Session("program") = Nothing
        Session("faculty") = Nothing
        Session("sponsor") = Nothing
        Session("status") = Nothing
        Session("sortby") = Nothing
        Session("DunningNo") = Nothing
        txtMessages.Text = Nothing
        Session("textarea") = Nothing
        dates()
        Program()
    End Sub

    'Protected Sub ChkBoxDunningLetter_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)
    '    If ChkBoxDunningLetter.Checked = True Then
    '        ChkBoxAcInfo.Checked = False
    '        ChkBoxStatementAc.Checked = False
    '    End If
    'End Sub

    'Protected Sub ChkBoxStatementAc_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)
    '    If ChkBoxStatementAc.Checked = True Then
    '        ChkBoxDunningLetter.Checked = False
    '    End If
    'End Sub

    'Protected Sub ChkBoxAcInfo_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)
    '    If ChkBoxAcInfo.Checked = True Then
    '        ChkBoxDunningLetter.Checked = False
    '        ChkBoxStatementAc.Checked = True
    '    End If
    'End Sub

    Protected Sub Page_LoadComplete(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.LoadComplete
        Session("StudentStatus") = ddlStuStatus.SelectedValue
    End Sub

    Protected Sub ibtnNew_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles ibtnNew.Click
        ibtnCancel_Click(sender, e)
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

#Region "txtMessages_TextChanged "
    'added by Hafiz @ 12/4/2016

    Protected Sub txtMessages_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs)

        If Not txtMessages.Text Is Nothing Then

            Session("textarea") = txtMessages.Text

        End If

    End Sub

#End Region

End Class
