Imports HTS.SAS.Entities
Imports HTS.SAS.BusinessObjects
Imports System.Data
Imports System.Collections.Generic
Imports System.Data.SqlClient
'Imports System.Windows.Forms
'Imports System.Drawing

Partial Class AddMulsudent
    Inherits System.Web.UI.Page
    Dim listStudent As New List(Of StudentEn)
    'Public Property AutoCompleteMode() As AutoCompleteMode
    ''' <summary>
    ''' Method to load Students in Grid
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub LoadstuGrid()
        Dim eob As New StudentEn
        Dim bobj As New StudentBAL
        Dim list As New List(Of StudentEn)
        Dim faculty As String
        Dim program As String
        Dim hostel As String
        Dim block As String
        Dim room As String
        Dim i As Integer = 0
        faculty = ddlFaculty.SelectedValue
        program = ddlProgram.SelectedValue
        hostel = ddlHostelCOde.SelectedValue
        block = ddlBloackCode.SelectedValue
        room = ddlRoomType.SelectedValue
        If faculty = "-1" Then faculty = ""
        If program = "-1" Then program = ""
        If hostel = "-1" Then hostel = ""
        If block = "-1" Then block = ""
        If room = "-1" Then room = ""
        If Not list Is Nothing Then
            eob.MatricNo = Trim(txtMatricNo.Text)
            eob.StudentName = Trim(txtname.Text)
            eob.Faculty = faculty
            eob.ProgramID = program
            eob.ID = Trim(txtPassport.Text)
            eob.SAKO_Code = hostel
            eob.SABK_Code = block
            eob.SART_Code = room
            eob.StCategoryAcess = New StudentCategoryAccessEn
            i = Session("Menuid")
            eob.StCategoryAcess.MenuID = i
            Try
                list = bobj.GetListStudent(eob)
            Catch ex As Exception
                LogError.Log("AddMulStudents", "LoadstuGrid", ex.Message)
            End Try
            dgStudentView.DataSource = list
            dgStudentView.DataBind()
            txtMatricNo.Focus()
        Else
            Response.Write("No Students are Available")
        End If
    End Sub

    ''' <summary>
    ''' Method to Load Programs Dropdown
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub LoadProgram(ByVal fac As String)
        Dim objBAL As New ProgramInfoBAL
        Dim listObj As New List(Of ProgramInfoEn)
        Dim loen As New ProgramInfoEn
        If ddlFaculty.SelectedIndex <> 0 Then
            loen.Faculty = fac
        End If
        loen.ProgramType = ""
        loen.ProgramCode = ""
        loen.Program = ""
        loen.ProgramBM = ""
        loen.Status = True
        Try
            listObj = objBAL.GetProgramInfoList(loen)
        Catch ex As Exception
            LogError.Log("AddMulStudents", "LoadProgram", ex.Message)
        End Try
        ddlProgram.Items.Clear()
        ddlProgram.Items.Add(New ListItem("--Select--", "-1"))
        ddlProgram.DataTextField = "CodeProgram"
        ddlProgram.DataValueField = "ProgramCode"
        ddlProgram.DataSource = listObj
        ddlProgram.DataBind()
    End Sub
    ''' <summary>
    ''' Method to Load Faculty Dropdown
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub LoadFaculty()
        Dim eFaculty As New FacultyEn
        Dim bFaculty As New FacultyBAL
        ddlFaculty.Items.Clear()
        eFaculty.SAFC_Code = "%"
        ddlFaculty.Items.Add(New ListItem("---Select---", "-1"))
        ddlFaculty.DataTextField = "SAFC_Desc"
        ddlFaculty.DataValueField = "SAFC_Code"
        Try
            ddlFaculty.DataSource = bFaculty.GetList(eFaculty)
        Catch ex As Exception
            LogError.Log("AddMulStudents", "LoadFaculty", ex.Message)
        End Try
        ddlFaculty.DataBind()
    End Sub
    Private Sub UpdateGrid()
        Dim cScript As String = ""
        cScript = "<SCRIPT LANGUAGE=""JavaScript"">window.opener.document.getElementById('ctl00_ContentPlaceHolder1_btnHidden').click();self.close();</SCRIPT>"
        ClientScript.RegisterStartupScript(Me.GetType(), "ClosePopUp", cScript)
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim cat As String

        If Session("ReceiptFor") Is Nothing Then
            cat = Request.QueryString("Cat")
        Else

            cat = Session("ReceiptFor")
        End If

        If Not IsPostBack() Then
            If cat = "Sp" Then
                spPnl.Visible = True

                stPnl.Visible = False


            ElseIf cat = "St" Or cat = "Sl" Then
                spPnl.Visible = False
                stPnl.Visible = True
            ElseIf cat = "Sb" Then
                spPnl.Visible = True
                stPnl.Visible = False
            Else
                UpdateGrid()
            End If
            LoadFaculty()
            FillDropDownList()
            chkStudent.Visible = False
        End If

    End Sub

    Protected Sub IbtnLoad_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ibtnLoad.Click
        LoadstuGrid()
        chkStudent.Visible = True
    End Sub


    Protected Sub ibtnStudetSelect_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ibtnStudetSelect.Click
        Dim eobj As New StudentEn

        Dim chkBox As CheckBox
        For Each dgi As DataGridItem In dgStudentView.Items
            chkBox = dgi.Cells(0).Controls(1)
            If chkBox.Checked = True Then
                eobj = New StudentEn
                eobj.MatricNo = dgi.Cells(1).Text
                eobj.StudentName = dgi.Cells(2).Text
                eobj.ICNo = dgi.Cells(3).Text
                eobj.Faculty = dgi.Cells(4).Text
                eobj.ProgramID = dgi.Cells(5).Text
                eobj.CurrentSemester = dgi.Cells(6).Text
                eobj.CurretSemesterYear = dgi.Cells(7).Text
                eobj.CategoryCode = dgi.Cells(8).Text
                eobj.TempAmount = 0
                listStudent.Add(eobj)
                dgStudentView.SelectedIndex = -1
                Session("eobjstu") = eobj
                Session("liststu") = listStudent
                eobj = Nothing
            End If
        Next

        UpdateGrid()
    End Sub

    Protected Sub ibtnClose_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ibtnClose.Click
        UpdateGrid()
    End Sub
    ''' <summary>
    ''' Method to load Sponsor in Grid
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub LoadGrid()
        Dim eob As New SponsorEn
        Dim bobj As New SponsorBAL
        Dim list As New List(Of SponsorEn)
        If Not list Is Nothing Then
            eob.SponserCode = Trim(txtSpnCode.Text)
            eob.Name = Trim(txtname.Text)
            eob.Type = Trim(txtSpnType.Text)
            eob.GLAccount = ""
            eob.Status = True
            Try
                list = bobj.GetSponserList(eob)
            Catch ex As Exception
                LogError.Log("AddMulStudents", "LoadGrid", ex.Message)
            End Try
            dgView.DataSource = list
            dgView.DataBind()
        Else
            Response.Write("No Fee types are Available")
        End If
    End Sub
    ''' <summary>
    ''' Method to Load All the Dropdowns
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub FillDropDownList()
        Dim eKolej As New KolejEn
        Dim bKolej As New KolejBAL
        ddlHostelCOde.Items.Clear()
        ddlHostelCOde.Items.Add(New ListItem("---Select---", "-1"))
        ddlHostelCOde.DataTextField = "SAKO_Description"
        ddlHostelCOde.DataValueField = "SAKO_Code"

        Try
            ddlHostelCOde.DataSource = bKolej.GetList(eKolej)
        Catch ex As Exception
            LogError.Log("HostelFee", "FillDropDownList", ex.Message)
        End Try

        ddlHostelCOde.DataBind()
    End Sub
    ''' <summary>
    ''' Method to Load Block Dropdown
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub LoadBlock()
        Dim eBlock As New BlockEn
        Dim bBlock As New BlockBAL
        ddlBloackCode.Items.Clear()
        ddlBloackCode.Items.Add(New ListItem("---Select---", "-1"))
        ddlBloackCode.DataTextField = "SABK_Description"
        ddlBloackCode.DataValueField = "SABK_Code"
        Try
            ddlBloackCode.DataSource = bBlock.GetBlockList(ddlHostelCOde.SelectedValue)

        Catch ex As Exception
            LogError.Log("HostelFee", "LoadBlock", ex.Message)
        End Try
        ddlBloackCode.DataBind()
        ddlRoomType.Items.Clear()
        ddlRoomType.Items.Add(New ListItem("---Select---", "-1"))
    End Sub
    ''' <summary>
    ''' Method to Load Room Dropdown
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub LoadRoom(Optional ByVal defaultVal As String = "")
        Dim bRoomType As New RoomTypeBAL
        ddlRoomType.Items.Clear()
        ddlRoomType.Items.Add(New ListItem("---Select---", "-1"))
        ddlRoomType.DataTextField = "SART_Description"
        ddlRoomType.DataValueField = "SART_Code"
        'ddlRoomType.SelectedValue = defaultVal
        Try
            ddlRoomType.DataSource = bRoomType.GetRoomTypeList(ddlBloackCode.SelectedValue)
        Catch ex As Exception
            LogError.Log("HostelFee", "LoadRoom", ex.Message)
        End Try
        ddlRoomType.DataBind()
    End Sub
    ''' <summary>
    ''' Method to load Sponsor in Balance
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub LoadGridBal()
        Dim eob As New SponsorEn
        Dim bobj As New SponsorBAL
        Dim list As New List(Of SponsorEn)
        If Not list Is Nothing Then
            eob.SponserCode = Trim(txtSpnCode.Text)
            eob.Name = Trim(txtname.Text)
            eob.Type = Trim(txtSpnType.Text)
            eob.GLAccount = ""
            eob.Status = True
            Try
                list = bobj.GetSponserList(eob)
            Catch ex As Exception
                LogError.Log("AddMulStudents", "LoadGridBal", ex.Message)
            End Try
            dgView.DataSource = list
            dgView.DataBind()
        Else
            Response.Write("No Fee types are Available")
        End If
    End Sub
    Protected Sub ibtnOK_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ibtnOK.Click
        If dgView.SelectedIndex <> -1 Then
            Dim eobj As New SponsorEn
            eobj.SponserCode = dgView.DataKeys(dgView.SelectedIndex)
            eobj.Name = dgView.Items(dgView.SelectedIndex).Cells(1).Text
            eobj.Type = dgView.Items(dgView.SelectedIndex).Cells(2).Text
            dgView.SelectedIndex = -1
            Session("eobjspn") = eobj
        End If
        UpdateGrid()
    End Sub

    Protected Sub ibtnSearch_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ibtnSearch.Click

        Dim cat As String
        cat = Request.QueryString("Cat")
        If cat = "Sb" Then
            LoadGridBal()
        Else
            LoadGrid()
        End If

    End Sub

    Protected Sub ImageButton1_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ImageButton1.Click
        UpdateGrid()
    End Sub

    Protected Sub txtMatricNo_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtMatricNo.TextChanged

    End Sub

    Protected Sub ddlFaculty_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlFaculty.SelectedIndexChanged
        LoadProgram(ddlFaculty.SelectedValue)
    End Sub

    Protected Sub chkStudent_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkStudent.CheckedChanged

        Dim chk As CheckBox
        Dim dgitem As DataGridItem
        If chkStudent.Checked = True Then
            For Each dgitem In dgStudentView.Items
                chk = dgitem.Cells(0).Controls(1)
                chk.Checked = True
            Next
        Else
            For Each dgitem In dgStudentView.Items
                chk = dgitem.Cells(0).Controls(1)
                chk.Checked = False
            Next
        End If
    End Sub

    Protected Sub ddlHostelCOde_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlHostelCOde.SelectedIndexChanged
        LoadBlock()
    End Sub


    Protected Sub ddlBloackCode_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlBloackCode.SelectedIndexChanged
        LoadRoom()
    End Sub
End Class
