Imports HTS.SAS.BusinessObjects
Imports HTS.SAS.Entities
Imports System.Data
Imports System.Collections.Generic
Imports System.Data.SqlClient
Imports System.Web.Configuration
Imports CrystalDecisions.CrystalReports.Engine

Partial Class RptStudentOutStanding
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
            'ibtnPrint.Attributes.Add("onclick", "new_window=window.open('../GroupReport/RptStudentOutStandingViewer.aspx','Hanodale','width=520,height=400,resize=1,scrollbars=1');new_window.focus();")

            'Commented By Zoya @26/02/2016
            'ibtnBMReport.Attributes.Add("onclick", "return getDate()")
            'ibtnBMReport.Attributes.Add("onclick", "new_window=window.open('../GroupReport/RptStudentOutStandingViewer.aspx','Hanodale','width=520,height=400,resize=1,scrollbars=1');new_window.focus();")
            'ibtnEnReport.Attributes.Add("onclick", "return getDate()")
            'ibtnEnReport.Attributes.Add("onclick", "new_window=window.open('../GroupReport/RptStudentOutStandingViewerEn.aspx','Hanodale','width=520,height=400,resize=1,scrollbars=1');new_window.focus();")

            txtTodate.Attributes.Add("OnKeyup", "return CheckToDate()")
            Faculty()
            ddlProgram.Items.Insert(0, New ListItem("--Please Select--", "-1"))
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
        ddlFaculty.Items.Insert(0, New ListItem("--Please Select--", "-1"))
        'ddlFaculty.Items.Add(New ListItem("-- Select --", "-1"))
    End Sub
    Private Sub Program()
        Dim ObjProgramEn As New ProgramInfoEn
        Dim ObjProgramBAL As New ProgramInfoBAL
        Dim LstProgram As New List(Of ProgramInfoEn)

        ObjProgramEn.SAFC_Code = ddlFaculty.SelectedValue
        LstProgram = ObjProgramBAL.GetProgramInfoListAll(ObjProgramEn.SAFC_Code)
        ddlProgram.Items.Clear()
        ddlProgram.DataSource = LstProgram
        ddlProgram.DataTextField = "Program"
        ddlProgram.DataValueField = "ProgramCode"
        'ddlProgram.DataValueField = "ProgramCode"
        ddlProgram.DataBind()
        ddlProgram.Items.Insert(0, New ListItem("--Please Select--", "-1"))
    End Sub

    Protected Sub ddlType_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs)

    End Sub

    Protected Sub ibtnPrint_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)

    End Sub

    Protected Sub ibtnView_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)

    End Sub

    Protected Sub ibtnCancel_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        ddlStuStatus.SelectedIndex = -1
        ddlFaculty.SelectedIndex = -1
        ddlProgram.SelectedIndex = -1
        'txtFrom.Text = ""
        txtTodate.Text = ""
        rdbtnstudid.Checked = False
        rdbtnstudname.Checked = False
        dates()
        Program()
        ibtnTodate.Visible = False

        'added by Hafiz @ 02/3/2016
        Session("program") = Nothing
        Session("faculty") = Nothing
        Session("sponsor") = Nothing
        Session("status") = Nothing
        Session("sortby") = Nothing
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

    'Modified by Hafiz @ 02/3/2016
    Protected Sub ddlStuStatus_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs)

        Dim status As String

        status = ddlStuStatus.SelectedValue

        If status = "-1" Then
            Session("status") = Nothing
        Else

            If ddlStuStatus.SelectedItem.ToString = "Active" Then
                Session("status") = "1"
            Else
                Session("status") = "2"
            End If

        End If

    End Sub

    Protected Sub ddlFaculty_SelectedIndexChanged1(sender As Object, e As EventArgs) Handles ddlFaculty.SelectedIndexChanged
        Program()

        Dim faculty As String

        faculty = ddlFaculty.SelectedValue

        If faculty = "-1" Then
            Session("faculty") = Nothing
        Else
            Session("faculty") = faculty
        End If
    End Sub
    Protected Sub ddlProgram_SelectedIndexChanged1(sender As Object, e As EventArgs) Handles ddlProgram.SelectedIndexChanged

        Dim program As String

        program = ddlProgram.SelectedValue

        If program = "-1" Then
            Session("program") = Nothing
        Else
            Session("program") = program
        End If
    End Sub

#Region "ibtnNew Click"

    'added by Hafiz @ 15/4/2016
    'Button new function

    Protected Sub ibtnNew_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ibtnNew.Click
        ddlStuStatus.SelectedIndex = "-1"
        ddlFaculty.SelectedIndex = "-1"
        ddlProgram.SelectedIndex = "-1"

        txtTodate.Text = Format(Date.Now, "dd/MM/yyyy")

        rdbtnstudid.Checked = False
        rdbtnstudname.Checked = False

        dates()
        Program()
        ibtnTodate.Visible = True

        Session("program") = Nothing
        Session("faculty") = Nothing
        Session("sponsor") = Nothing
        Session("status") = Nothing
        Session("sortby") = Nothing
    End Sub

#End Region

End Class



