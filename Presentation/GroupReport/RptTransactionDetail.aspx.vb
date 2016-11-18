Imports HTS.SAS.BusinessObjects
Imports HTS.SAS.Entities
Imports System.Data
Imports System.Collections.Generic
Imports System.Data.SqlClient
Imports System.Web.Configuration

Partial Class RptTransactionDetail
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
            'ibtnPrint.Attributes.Add("onclick", "new_window=window.open('RptTransactionDetailCRForm.aspx','Hanodale','width=700,height=500,resizable=1');new_window.focus();")
            ibtnFDate.Attributes.Add("onClick", "return getibtnFDate()")
            ibtnTodate.Attributes.Add("onClick", "return getDateto()")

            'UnCommented By Zoya @26/02/2016
            ibtnPrint.Attributes.Add("onclick", "return getDate()")

            'Added By Zoya @26/02/2016
            'ibtnPrint.Attributes.Add("onclick", "new_window=window.open('../GroupReport/RptTransactionDetailViewer.aspx','Hanodale','width=520,height=400,resize=1,scrollbars=1');new_window.focus();")

            'Commented By Zoya @26/02/2016
            'ibtnBMReport.Attributes.Add("onclick", "return getDate()")
            'ibtnBMReport.Attributes.Add("onclick", "new_window=window.open('../GroupReport/RptTransactionDetailViewer.aspx','Hanodale','width=520,height=400,resize=1,scrollbars=1');new_window.focus();")
            'ibtnEnReport.Attributes.Add("onclick", "return getDate()")
            'ibtnEnReport.Attributes.Add("onclick", "new_window=window.open('../GroupReport/RptTransactionDetailViewerEn.aspx','Hanodale','width=520,height=400,resize=1,scrollbars=1');new_window.focus();")

            ibtnView.Attributes.Add("onclick", "return getDate()")
            txtFrom.Attributes.Add("OnKeyup", "return CheckFromDate()")
            txtTodate.Attributes.Add("OnKeyup", "return CheckToDate()")
            LoadUserRights()
            txtFrom.ReadOnly = True
            txtTodate.ReadOnly = True
            ibtnFDate.Visible = False
            ibtnTodate.Visible = False
            Faculty()
            Program()
            Sponsor()
            dates()

            RdbtnStudentID.Checked = True
            Session("sortby") = "matricid"

            Session("program") = Nothing
            Session("faculty") = Nothing
            Session("sponsor") = Nothing
            Session("status") = Nothing

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
        'ibtnBMReport.Enabled = eobj.IsPrint
        'ibtnEnReport.Enabled = eobj.IsPrint
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
        Else
            Session("faculty") = faculty
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

    Protected Sub ibtnCancel_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        ddlStuStatus.SelectedIndex = -1
        ddlFaculty.SelectedIndex = -1
        ddlProgram.SelectedIndex = -1
        ddlSponsor.SelectedIndex = -1
        ChkDateRange.Checked = False

        RdbtnStudentID.Checked = True
        Session("sortby") = "matricid"

        RdbtnStudentName.Checked = False
        txtFrom.ReadOnly = True
        txtTodate.ReadOnly = True
        ibtnFDate.Visible = False
        ibtnTodate.Visible = False
        dates()
        Program()
    End Sub

    Protected Sub RdbtnStudentID_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        If RdbtnStudentID.Checked = True Then
            Session("sortby") = "matricid"
        End If
    End Sub

    Protected Sub RdbtnStudentName_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        If RdbtnStudentName.Checked = True Then
            Session("sortby") = "studname"
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

    Protected Sub ddlStuStatus_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim status As String
        status = ddlStuStatus.SelectedValue
        If status = "-1" Then
            'status = "%"
            Session("status") = status
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

#Region "ibtnNew Click"

    'added by Hafiz @ 01/3/2016
    'Button new function

    Protected Sub ibtnNew_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ibtnNew.Click
        onclearData()
    End Sub

    Private Sub onclearData()

        ddlStuStatus.SelectedIndex = "-1"
        ddlFaculty.SelectedIndex = "-1"
        ddlProgram.SelectedIndex = "-1"
        ddlSponsor.SelectedIndex = "-1"

        ChkDateRange.Checked = False
        txtFrom.Text = Format(Date.Now, "dd/MM/yyyy")
        txtTodate.Text = Format(Date.Now, "dd/MM/yyyy")

        RdbtnStudentID.Checked = True
        Session("sortby") = "matricid"

        RdbtnStudentName.Checked = False

        Session("program") = Nothing
        Session("faculty") = Nothing
        Session("sponsor") = Nothing
        Session("status") = Nothing

    End Sub

#End Region

End Class
