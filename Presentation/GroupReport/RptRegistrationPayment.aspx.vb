Imports HTS.SAS.BusinessObjects
Imports HTS.SAS.Entities
Imports System.Data
Imports System.Collections.Generic
Imports System.Data.SqlClient
Imports System.Web.Configuration

Partial Class RptRegistrationPayment
    Inherits System.Web.UI.Page

    Dim objBE As New BusinessEntities.SemesterSetup
    Dim objSQLQuery As New SQLPowerQueryManager.PowerQueryManager.SemesterSetup
    Dim GlobalSQLConnString As String = ConfigurationManager.ConnectionStrings("SASNEWConnectionString").ToString
    Dim DSReturn As New DataSet
    Dim strRetrunErrorMsg As String = String.Empty
    Dim blnReturnValue As Boolean

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack() Then

            'UnCommented By Zoya @26/02/2016
            ibtnPrint.Attributes.Add("onclick", "return getDate()")
            'Added By Zoya @26/02/2016
            ibtnPrint.Attributes.Add("onclick", "new_window=window.open('../GroupReport/RptRegistrationPaymentViewer.aspx','Hanodale','width=520,height=400,resize=0,scrollbars=1');new_window.focus();")

            'Commented By Zoya @26/02/2016
            'ibtnBMReport.Attributes.Add("onclick", "return getDate()")
            'ibtnEnReport.Attributes.Add("onclick", "return getDate()")
            'ibtnBMReport.Attributes.Add("onclick", "new_window=window.open('../GroupReport/RptRegistrationPaymentViewer.aspx','Hanodale','width=520,height=400,resize=0,scrollbars=1');new_window.focus();")
            'ibtnEnReport.Attributes.Add("onclick", "new_window=window.open('../GroupReport/RptRegistrationPaymentViewerEn.aspx','Hanodale','width=520,height=400,resize=0,scrollbars=1');new_window.focus();")

            Faculty()
            Program()
            Semester()
            'ddlProgram.Enabled = False
            'ddlProgram.Items.Insert(0, New ListItem("--Please Select--", "0"))
            'txtFrom.ReadOnly = True
            lblMsg.Text = ""
            Session("Program") = Nothing
            Session("Faculty") = Nothing
            Session("Status") = Nothing
            Session("Sortby") = Nothing
        End If
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
        ddlProgram.DataBind()
        ddlProgram.Items.Insert(0, New ListItem("--Please Select--", "0"))
    End Sub
    Private Sub Semester()
        Try
            objBE.SQLCase = 1
            blnReturnValue = objSQLQuery.SemesterSetup_Retreive_Details(objBE, GlobalSQLConnString, strRetrunErrorMsg, DSReturn)

            If blnReturnValue Then
                ddlSemester.DataSource = DSReturn
                ddlSemester.DataTextField = "SAST_Code"
                ddlSemester.DataValueField = "SAST_Code"
                ddlSemester.DataBind()
                ddlSemester.Items.Insert(0, New ListItem("--Please Select--", "0"))
            Else
                LogError.Log("Registration Payment Report", "Semester Dropdown List", strRetrunErrorMsg)
                lblMsg.Text = strRetrunErrorMsg
            End If
        Catch ex As Exception
            LogError.Log("Registration Payment Report", "Semester Dropdown List", ex.Message)
            lblMsg.Text = ex.Message
        End Try

    End Sub

    Protected Sub ddlFaculty_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlFaculty.SelectedIndexChanged
        If ddlFaculty.SelectedIndex = 0 Then
            Program()
            ddlProgram.Enabled = False
            Session("Faculty") = "%"
        Else
            Session("Faculty") = ddlFaculty.SelectedValue.ToString
            ddlProgram.Enabled = True
            Program()
        End If
    End Sub
    Protected Sub ddlStuStatus_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlStuStatus.SelectedIndexChanged
        If ddlStuStatus.SelectedIndex = 0 Then
            Session("Status") = Nothing
        Else
            Session("Status") = ddlStuStatus.SelectedItem.ToString
        End If
    End Sub
    Protected Sub ddlProgram_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim strProgram As String
        strProgram = ddlProgram.SelectedValue
        If strProgram = "-1" Then
            strProgram = "%"
            Session("Program") = strProgram
        Else
            Session("Program") = strProgram
        End If
    End Sub
    Protected Sub ddlSemester_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlSemester.SelectedIndexChanged
        Dim strSemester As String
        strSemester = ddlProgram.SelectedValue
        If strSemester = "-1" Then
            strSemester = "%"
            Session("Semester") = strSemester
        Else
            Session("Semester") = strSemester
        End If
    End Sub

    Protected Sub ibtnCancel_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        ddlStuStatus.SelectedIndex = 0
        ddlFaculty.SelectedIndex = 0
        ddlSemester.SelectedIndex = 0
        lblMsg.Text = ""

        Session("Program") = Nothing
        Session("Faculty") = Nothing
        Session("Status") = Nothing
        Session("Sortby") = Nothing
    End Sub
End Class
