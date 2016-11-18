Imports MaxGeneric
Imports System.Data
Imports HTS.SAS.BusinessObjects
Imports HTS.SAS.Entities
Imports System.Collections.Generic
Imports System.Linq

Partial Class StudentSponsor
    Inherits System.Web.UI.Page

    Private _StudentDAL As New HTS.SAS.DataAccessObjects.StudentDAL
    Private _AccountsDAL As New HTS.SAS.DataAccessObjects.AccountsDAL
    Dim MatricNo As String
    Dim ProgramId As String
    Dim BatchCode As String = String.Empty
    Dim Status As String = String.Empty
    Dim Sponsor As String
    Private listStu As New List(Of StudentEn)
    Private lststud As New List(Of StudentEn)
    Private lststudA As New List(Of StudentEn)
    Dim listStudent As New List(Of StudentEn)
    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        Sponsor = Request.QueryString("SponsorCode")
        If Not Page.IsPostBack Then
            'MatricNo = Request.QueryString("MatricNo")


        End If
    End Sub
    Private Sub BindGrid()
        Dim i As Integer = 0
        'Dim list As New List(Of StudentEn)
        If Not lststud Is Nothing Then
            MatricNo = Trim(txtMatricNo.Text)
            lststud = _StudentDAL.GetStudentDetailsBySponsorWithoutStuValidity(Sponsor, MatricNo)
            dgStudentInfo.Visible = True
            dgStudentInfo.DataSource = lststud
            dgStudentInfo.DataBind()
            txtMatricNo.Focus()
            If lststud.Count = 0 Then
                lblMsg.Text = "No Students are Available"
                lblMsg.Visible = True
                dgStudentInfo.Visible = False
            End If
        Else
            lblMsg.Text = "No Students are Available"
            lblMsg.Visible = True
            Response.Write("No Students are Available")
        End If

    End Sub
    Protected Sub ibtnSearch_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ibtnSearch.Click

        BindGrid()

    End Sub
    Protected Sub chk_CheckedChanged(sender As Object, e As EventArgs)
        Dim chk As CheckBox = (DirectCast(sender, CheckBox))
        Dim dgitem As DataGridItem = DirectCast(chk.Parent.Parent, DataGridItem)
        If Not Session("lstStu") Is Nothing Then
            listStu = Session("lstStu")
        Else
            listStu = New List(Of StudentEn)
        End If

        If chk.Checked = True Then
            chk.Checked = True
            If Not listStu.Any(Function(x) x.MatricNo = dgitem.Cells(1).Text) Then
                Dim eobjstu As New StudentEn
                eobjstu.ProgramID = Request.QueryString("ProgramId")
                eobjstu.SponsorCode = Request.QueryString("SponsorCode")
                eobjstu.MatricNo = dgitem.Cells(1).Text
                eobjstu.StudentName = dgitem.Cells(2).Text
                eobjstu.CurrentSemester = dgitem.Cells(3).Text
                eobjstu.CategoryCode = dgitem.Cells(7).Text
                listStu.Add(eobjstu)
            End If
        Else
            chk.Checked = False
            listStu.Remove(listStu.Where(Function(x) x.MatricNo = dgitem.Cells(1).Text).Select(Function(x) x).FirstOrDefault())
        End If
        Session("lstStu") = listStu
        Session("check") = chk.Checked
    End Sub

    

    Protected Sub chkStudent_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkStudent.CheckedChanged

        Dim chk As CheckBox
        Dim dgitem As DataGridItem
        If Not Session("lstStu") Is Nothing Then
            listStu = Session("lstStu")
        Else
            listStu = New List(Of StudentEn)
        End If
        If chkStudent.Checked = True Then
            Dim eobjstu As New StudentEn
            For Each dgitem In dgStudentInfo.Items
                chk = dgitem.Cells(0).Controls(1)
                chk.Checked = True
                If Not listStu.Any(Function(x) x.MatricNo = dgitem.Cells(1).Text) Then
                    eobjstu.ProgramID = Request.QueryString("ProgramId")
                    eobjstu.SponsorCode = Request.QueryString("SponsorCode")
                    eobjstu.MatricNo = dgitem.Cells(1).Text
                    eobjstu.StudentName = dgitem.Cells(2).Text
                    eobjstu.CurrentSemester = dgitem.Cells(3).Text
                    eobjstu.CategoryCode = dgitem.Cells(7).Text
                    listStu.Add(eobjstu)
                End If
            Next
        Else
            For Each dgitem In dgStudentInfo.Items
                chk = dgitem.Cells(0).Controls(1)
                chk.Checked = False
                listStu.Remove(listStu.Where(Function(x) x.MatricNo = dgitem.Cells(1).Text).Select(Function(x) x).FirstOrDefault())
            Next
        End If
        Session("lstStu") = listStu

    End Sub

    Protected Sub ibtnOK_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ibtnOK.Click
        Dim eobj As New StudentEn

        Dim chkBox As CheckBox
        For Each dgi As DataGridItem In dgStudentInfo.Items
            chkBox = dgi.Cells(0).Controls(1)
            If chkBox.Checked = True Then
                eobj = New StudentEn
                eobj.MatricNo = dgi.Cells(1).Text
                eobj.StudentName = dgi.Cells(2).Text
                eobj.CurrentSemester = dgi.Cells(3).Text
                eobj.SponsorLimit = dgi.Cells(4).Text
                eobj.AllocatedAmount = dgi.Cells(5).Text
                eobj.OutstandingAmount = dgi.Cells(6).Text
                eobj.CategoryCode = dgi.Cells(7).Text
                eobj.ProgramID = dgi.Cells(8).Text
                eobj.TempAmount = 0
                listStudent.Add(eobj)
                dgStudentInfo.SelectedIndex = -1
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
    ''' Method to Update Grid
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub UpdateGrid()
        Dim chk As CheckBox
        For Each dgitem In dgStudentInfo.Items
            chk = dgitem.Cells(0).Controls(1)
            'chk.Checked = True
            If chkStudent.Checked = True Then
                chkStudent.Checked = True
                chkStudent.Enabled = True
                chk.Checked = True
                chk.Enabled = False
            Else
                chk.Checked = False
                chk.Enabled = True
                chkStudent.Checked = False
                chkStudent.Enabled = True
            End If
            'If chk.Checked = True Then
            '    chk.Checked = True
            '    chk.Enabled = False
            'Else
            '    chk.Checked = False
            '    chk.Enabled = True
            'End If
        Next
        Dim cScript As String = ""
        cScript = "<SCRIPT LANGUAGE=""JavaScript"">window.opener.document.getElementById('ctl00_ContentPlaceHolder1_btnHidden').click();self.close();</SCRIPT>"
        ClientScript.RegisterStartupScript(Me.GetType(), "ClosePopUp", cScript)
    End Sub

End Class
