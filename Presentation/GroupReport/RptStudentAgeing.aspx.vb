Imports HTS.SAS.BusinessObjects
Imports HTS.SAS.DataAccessObjects
Imports HTS.SAS.Entities
Imports System.Data
Imports System.Collections.Generic
Imports System.Data.SqlClient
Imports System.Web.Configuration
Imports System.Linq

'modified by Hafiz @ 03/3/2016

Partial Class RptStudentAgeing
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
            ibtnDatePanelType1.Attributes.Add("onClick", "return PopupCalendar('ibtnDatePanelType1')")
            ibtnDatePanelType2.Attributes.Add("onClick", "return PopupCalendar('ibtnDatePanelType2')")
            ibtnPrint.Attributes.Add("onclick", "return getDate()")
            txtDatePanelType1.Attributes.Add("OnKeyup", "return CheckToDate('txtDatePanelType1')")
            txtDatePanelType2.Attributes.Add("OnKeyup", "return CheckToDate('txtDatePanelType2')")

            OnClearData()
            FormatDate()

            Faculty()
            'ddlProgram.Items.Insert(0, New ListItem("--Please Select--", "-1"))

        End If

    End Sub

    Private Sub FormatDate()
        txtDatePanelType1.Text = Format(Date.Now, "dd/MM/yyyy")
        txtDatePanelType2.Text = Format(Date.Now, "dd/MM/yyyy")
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
    End Sub

    Protected Sub ibtnCancel_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        OnClearData()
        FormatDate()
    End Sub

    Protected Sub ddlFaculty_SelectedIndexChanged1(sender As Object, e As EventArgs) Handles ddlFaculty.SelectedIndexChanged

        Dim faculty As String

        faculty = ddlFaculty.SelectedValue

        If faculty = "-1" Then
            Session("faculty") = Nothing
        Else
            Session("faculty") = faculty
        End If

    End Sub

#Region "added by Hafiz @ 08/11/2016 for new RA"

    Protected Sub OnClearData()
        PanelType1.Visible = False
        PanelType2.Visible = False
        PanelType3.Visible = False

        ddlReportType.SelectedValue = "-1"
        'Panel Type 1 clear
        ddlFeeType.SelectedValue = "-1"
        cbMatricId.Checked = False
        cbProgram.Checked = False
        'Panel Type 2 clear

        FormatDate()
        rbYearly.Checked = False
        rbVariousMonths.Checked = False
        rbQuaterly.Checked = False
        rbYearly.Checked = False

        Session("program") = Nothing
        Session("faculty") = Nothing
        Session("status") = Nothing
        Session("sortby") = Nothing
    End Sub

    Protected Sub ddlReportType_SelectedIndexChanged(sender As Object, e As EventArgs)

        If ddlReportType.SelectedValue <> "-1" Then
            If ddlReportType.SelectedValue = "1" Then
                PanelType1.Visible = True
                PanelType2.Visible = False
                PanelType3.Visible = False

                LoadFeeType()
            ElseIf ddlReportType.SelectedValue = "2" Then
                PanelType1.Visible = False
                PanelType2.Visible = True
                PanelType3.Visible = False
            ElseIf ddlReportType.SelectedValue = "3" Then
                PanelType1.Visible = False
                PanelType2.Visible = False
                PanelType3.Visible = True
            End If
        Else
            PanelType1.Visible = False
            PanelType2.Visible = False
            PanelType3.Visible = False
        End If

    End Sub

    Protected Sub LoadFeeType()

        Dim list As List(Of FeeTypesEn) = New FeeTypesDAL().GetList(New FeeTypesEn).Where(Function(x) x.Status = True).ToList()
        If list.Count > 0 Then
            ddlFeeType.Items.Clear()
            ddlFeeType.DataTextField = "Description"
            ddlFeeType.DataValueField = "FeeTypeCode"
            ddlFeeType.DataSource = list
            ddlFeeType.DataBind()
            ddlFeeType.Items.Insert(0, New ListItem("-- Please Select --", "-1"))
        Else
            ddlFeeType.Items.Insert(0, New ListItem("-- No Fee Type Found --", "-1"))
        End If

    End Sub

#End Region
End Class
