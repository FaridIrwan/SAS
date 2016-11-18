Imports HTS.SAS.BusinessObjects
Imports HTS.SAS.Entities
Imports System.Data
Imports System.Collections.Generic
Imports System.Data.SqlClient
Imports System.Web.Configuration
Imports CrystalDecisions.CrystalReports.Engine

Partial Class RptStudentOverPaid
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
            ibtnTodate.Attributes.Add("onClick", "return getDateto()")

            'UnCommented By Zoya @26/02/2016
            ibtnPrint.Attributes.Add("onclick", "return getDate()")
            'Added By Zoya @26/02/2016
            ibtnPrint.Attributes.Add("onclick", "new_window=window.open('../GroupReport/RptStudentOverPaidViewer.aspx','Hanodale','width=520,height=400,resize=0,scrollbars=1');new_window.focus();")

            'Commented By Zoya @26/02/2016
            'ibtnBMReport.Attributes.Add("onclick", "return getDate()")
            'ibtnBMReport.Attributes.Add("onclick", "new_window=window.open('../GroupReport/RptStudentOverPaidViewer.aspx','Hanodale','width=520,height=400,resize=0,scrollbars=1');new_window.focus();")
            'ibtnEnReport.Attributes.Add("onclick", "new_window=window.open('../GroupReport/RptStudentOverPaidViewerEn.aspx','Hanodale','width=520,height=400,resize=0,scrollbars=1');new_window.focus();")
            'ibtnEnReport.Attributes.Add("onclick", "return getDate()")

            txtTodate.Attributes.Add("OnKeyup", "return CheckToDate()")
            'LoadUserRights()
            Faculty()
            Program()
            dates()

            Session("program") = Nothing
            Session("faculty") = Nothing
            Session("sponsor") = Nothing
            Session("status") = Nothing
            Session("sortby") = Nothing
        End If
    End Sub
    Private Sub dates()
        txtTodate.Text = Format(Date.Now, "dd/MM/yyyy")
    End Sub
    Private Sub Faculty()
        Dim ObjFacultyEn As New FacultyEn
        Dim ObjFacultyBAL As New FacultyBAL
        Dim LstObjFaculty As New List(Of FacultyEn)
        ObjFacultyEn.SAFC_Code = "%"
        LstObjFaculty = ObjFacultyBAL.GetList(ObjFacultyEn)
        ddlFaculty.Items.Clear()
        'ddlFaculty.Items.Add(New ListItem("-- Select --", "-1"))

        ddlFaculty.DataTextField = "SAFC_Desc"
        ddlFaculty.DataValueField = "SAFC_Code"
        ddlFaculty.DataSource = LstObjFaculty
        ddlFaculty.DataBind()
        ddlFaculty.Items.Insert(0, New ListItem("--Please Select--", "0"))
    End Sub
    Private Sub Program()
        Dim ObjProgramEn As New ProgramInfoEn
        Dim ObjProgramBAL As New ProgramInfoBAL
        Dim LstProgram As New List(Of ProgramInfoEn)

        ObjProgramEn.SAFC_Code = ddlFaculty.SelectedValue
        LstProgram = ObjProgramBAL.GetProgramInfoListAll(ObjProgramEn.SAFC_Code)
        ddlProgram.Items.Clear()
        'ddlProgram.Items.Add(New ListItem("-- Select --", "-1"))
        ddlProgram.DataSource = LstProgram
        ddlProgram.DataTextField = "Program"
        ddlProgram.DataValueField = "ProgramCode"
        'ddlProgram.DataValueField = "ProgramCode"
        ddlProgram.DataBind()
        ddlProgram.Items.Insert(0, New ListItem("--Please Select--", "0"))
    End Sub

    'Private Sub LoadUserRights()

    '    Dim obj As New UsersBAL
    '    Dim eobj As UserRightsEn

    '    'eobj = obj.GetUserRights(5, 1)
    '    eobj = obj.GetUserRights(CInt(Request.QueryString("Menuid")), CInt(Session("UserGroup")))
    '    'Rights for Add
    '    If eobj.IsAdd = True Then
    '        ibtnSave.Enabled = True
    '        'OnAdd()
    '    Else
    '        ibtnNew.Enabled = False
    '        ibtnNew.ImageUrl = "images/gAdd.png"
    '        ibtnNew.ToolTip = "Access Denied"
    '        ibtnDelete.Enabled = False
    '        ibtnDelete.ImageUrl = "images/gdelete.png"
    '        ibtnDelete.ToolTip = "Access Denied"
    '        '-----------------------------------------------
    '        ibtnFirst.Enabled = False
    '        ibtnLast.Enabled = False
    '        ibtnPrevs.Enabled = False
    '        ibtnNext.Enabled = False
    '        ibtnFirst.ToolTip = "Access Denied"
    '        ibtnLast.ToolTip = "Access Denied"
    '        ibtnPrevs.ToolTip = "Access Denied"
    '        ibtnNext.ToolTip = "Access Denied"
    '        ibtnFirst.ImageUrl = "images/gnew_first.png"
    '        ibtnLast.ImageUrl = "images/gnew_last.png"
    '        ibtnPrevs.ImageUrl = "images/gnew_Prev.png"
    '        ibtnNext.ImageUrl = "images/gnew_next.png"
    '        '------------------------------------------------
    '        ibtnSave.Enabled = False
    '        ibtnSave.ImageUrl = "images/gsave.png"
    '        ibtnSave.ToolTip = "Access Denied"
    '    End If
    '    'Rights for Edit
    '    If eobj.IsEdit = True Then
    '        'ibtnSave.ToolTip = "Access Denied"
    '        Session("EditFlag") = True
    '    Else
    '        Session("EditFlag") = False
    '    End If
    '    'Rights for View
    '    ibtnView.Enabled = eobj.IsView
    '    If eobj.IsView = True Then
    '        ibtnView.ImageUrl = "images/find.png"
    '        ibtnView.Enabled = True
    '    Else
    '        ibtnView.ImageUrl = "images/gfind.png"
    '        ibtnView.ToolTip = "Access Denied"
    '    End If
    '    'Rights for Print
    '    ibtnPrint.Enabled = eobj.IsPrint
    '    If eobj.IsPrint = True Then
    '        ibtnPrint.Enabled = True
    '        ibtnPrint.ImageUrl = "images/print.png"
    '        ibtnPrint.ToolTip = "Print"
    '    Else
    '        ibtnPrint.Enabled = False
    '        ibtnPrint.ImageUrl = "images/gprint.png"
    '        ibtnPrint.ToolTip = "Access Denied"
    '    End If
    '    'Checking Default mode
    '    If eobj.IsAddModeDefault = True Then
    '        ' pnlView.Visible = False
    '        'pnlAdd.Visible = True
    '    Else
    '        'pnlAdd.Visible = False
    '        ' pnlView.Visible = True
    '        'LoadGrid()
    '    End If
    '    If eobj.IsOthers = True Then
    '        ibtnOthers.Enabled = True
    '        ibtnOthers.ImageUrl = "images/others.png"
    '        ibtnOthers.ToolTip = "Others"
    '    Else
    '        ibtnOthers.Enabled = False
    '        ibtnOthers.ImageUrl = "images/gothers.png"
    '        ibtnOthers.ToolTip = "Access Denied"
    '    End If
    '    If eobj.IsPost = True Then
    '        ibtnPosting.Enabled = True
    '        ibtnPosting.ImageUrl = "images/posting.png"
    '        ibtnPosting.ToolTip = "Posting"
    '    Else
    '        ibtnPosting.Enabled = False
    '        ibtnPosting.ImageUrl = "images/gposting.png"
    '        ibtnPosting.ToolTip = "Access Denied"
    '    End If
    'End Sub
    Protected Sub ddlType_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs)

    End Sub

    Protected Sub ibtnCancel_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        ddlStuStatus.SelectedIndex = -1
        ddlFaculty.SelectedIndex = -1
        ddlProgram.SelectedIndex = -1
        txtTodate.Text = ""
        rdbtnstudid.Checked = False
        rdbtnstudname.Checked = False
        dates()
        Program()
        ibtnTodate.Visible = False
    End Sub

    Protected Sub ibtnDelete_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)

    End Sub

    Protected Sub rdbtnstudid_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        If rdbtnstudid.Checked = True Then
            Session("sortby") = "MatricNo"
        Else
            Session("sortby") = ""
        End If
    End Sub

    Protected Sub rdbtnstudname_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        If rdbtnstudname.Checked = True Then
            Session("sortby") = "Name"
        Else
            Session("sortby") = ""
        End If
    End Sub

    Protected Sub ddlFaculty_SelectedIndexChanged1(sender As Object, e As EventArgs) Handles ddlFaculty.SelectedIndexChanged
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
    Protected Sub ddlStuStatus_SelectedIndexChanged1(sender As Object, e As EventArgs) Handles ddlStuStatus.SelectedIndexChanged
        If ddlStuStatus.SelectedValue = "Active" Then
            Session("status") = "1"
        Else
            Session("status") = "2"
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
    Protected Sub txtFrom_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs)

    End Sub
End Class
