Imports HTS.SAS.BusinessObjects
Imports HTS.SAS.Entities
Imports System.Data
Imports System.Collections.Generic
Imports System.Data.SqlClient
Imports System.Web.Configuration
Imports CrystalDecisions.CrystalReports.Engine

Partial Class RptRefreshmentFund
    Inherits System.Web.UI.Page

    Dim objBE As New BusinessEntities.ProgramEn
    Dim objSQLQuery As New SQLPowerQueryManager.PowerQueryManager.ProgramDL
    Dim GlobalSQLConnString As String = ConfigurationManager.ConnectionStrings("SASNEWConnectionString").ToString
    Dim DSReturn As New DataSet
    Dim strRetrunErrorMsg As String = String.Empty
    Dim blnReturnValue As Boolean
    Dim strMode As String

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Not IsPostBack() Then
            Menuname(CInt(Request.QueryString("Menuid")))
            ibtnTodate.Attributes.Add("onClick", "return getDateto()")

            'UnCommented By Zoya @26/02/2016
            ibtnPrint.Attributes.Add("onclick", "return getDate()")
            'Added By Zoya @26/02/2016
            ibtnPrint.Attributes.Add("onclick", "new_window=window.open('../GroupReport/RptRefreshmentFundViewer.aspx','Hanodale','width=520,height=400,resize=0,scrollbars=1');new_window.focus();")

            'Commented By Zoya @26/02/2016
            'ibtnBMReport.Attributes.Add("onclick", "return getDate()")
            'ibtnBMReport.Attributes.Add("onclick", "new_window=window.open('../GroupReport/RptRefreshmentFundViewer.aspx','Hanodale','width=520,height=400,resize=0,scrollbars=1');new_window.focus();")
            'ibtnEnReport.Attributes.Add("onclick", "return getDate()")
            'ibtnEnReport.Attributes.Add("onclick", "new_window=window.open('../GroupReport/RptRefreshmentFundViewerEn.aspx','Hanodale','width=520,height=400,resize=0,scrollbars=1');new_window.focus();")

            'ibtnView.Attributes.Add("onclick", "return getDate()")
            txtTodate.Attributes.Add("OnKeyup", "return CheckToDate()")
            Faculty()
            txtTodate.Text = Format(Now(), "dd/MM/yyyy")
            ddlProgram.Enabled = False
            ddlProgram.Items.Insert(0, New ListItem("--Please Select--", "0"))

            Session("Program") = Nothing
            Session("Faculty") = Nothing
            'Session("sponsor") = Nothing
            Session("Status") = Nothing
            Session("Sortby") = Nothing

        End If
    End Sub

    Private Sub Menuname(ByVal MenuId As Integer)
        Dim eobj As New MenuEn
        Dim bobj As New MenuBAL
        eobj.MenuId = MenuId
        eobj = bobj.GetMenus(eobj)
        lblMenuName.Text = eobj.MenuName
    End Sub

    Private Sub Faculty()
        Dim ObjFacultyEn As New FacultyEn
        Dim ObjFacultyBAL As New FacultyBAL
        Dim LstObjFaculty As New List(Of FacultyEn)
        ObjFacultyEn.SAFC_Code = "%"
        LstObjFaculty = ObjFacultyBAL.GetList(ObjFacultyEn)
        ddlFaculty.Items.Clear()

        ddlFaculty.DataTextField = "SAFC_Desc"
        ddlFaculty.DataValueField = "SAFC_Code"
        ddlFaculty.DataSource = LstObjFaculty
        ddlFaculty.DataBind()
        ddlFaculty.Items.Insert(0, New ListItem("--Please Select--", "0"))
    End Sub
    Private Sub Program()
        objBE.SAFC_Code = ddlFaculty.SelectedValue
        blnReturnValue = objSQLQuery.GetProgramByFaculty(objBE, strRetrunErrorMsg, GlobalSQLConnString, DSReturn)

        If blnReturnValue Then
            ddlProgram.DataSource = DSReturn
            ddlProgram.DataTextField = "SAPG_ProgramBM"
            ddlProgram.DataValueField = "SAPG_Code"
            ddlProgram.DataBind()
            ddlProgram.Items.Insert(0, New ListItem("--Please Select--", "0"))
        Else
            LogError.Log("Refreshment Fund Report", "Program", strRetrunErrorMsg)
            lblMsg.Text = strRetrunErrorMsg
        End If

    End Sub

    Protected Sub rdbtnstudid_CheckedChanged(sender As Object, e As EventArgs) Handles rdbtnstudid.CheckedChanged
        rdbtnstudname.Checked = False
        ViewState("MatricNo") = "MatricNo"
    End Sub

    Protected Sub rdbtnstudname_CheckedChanged(sender As Object, e As EventArgs) Handles rdbtnstudname.CheckedChanged
        rdbtnstudid.Checked = False
        ViewState("Name") = "Name"
    End Sub

    Protected Sub ddlFaculty_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlFaculty.SelectedIndexChanged
        If ddlFaculty.SelectedIndex = 0 Then
            Program()
            ddlProgram.Enabled = False
            Session("Faculty") = "%"
        Else
            Session("Faculty") = ddlFaculty.SelectedItem.ToString
            ddlProgram.Enabled = True
            Program()
        End If
    End Sub

    'Protected Sub ibtnPrint_Click(sender As Object, e As ImageClickEventArgs) Handles ibtnPrint.Click

    'End Sub

    Protected Sub ddlStatus_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlStatus.SelectedIndexChanged
        If ddlStatus.SelectedIndex = 0 Then
            Session("Status") = Nothing
        Else
            Session("Status") = ddlStatus.SelectedItem.ToString
        End If
    End Sub

    Protected Sub ddlProgram_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlProgram.SelectedIndexChanged
        If ddlProgram.SelectedIndex = 0 Then
            Session("Program") = Nothing
        Else
            Session("Program") = ddlProgram.SelectedValue.ToString
        End If
    End Sub

    Protected Sub ibtnCancel_Click(sender As Object, e As ImageClickEventArgs) Handles ibtnCancel.Click
        ddlStatus.SelectedIndex = 0
        ddlFaculty.SelectedIndex = 0
        ddlProgram.SelectedIndex = 0
        txtTodate.Text = ""
        rdbtnstudid.Checked = False
        rdbtnstudname.Checked = False
    End Sub


End Class
