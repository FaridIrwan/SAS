Imports MaxGeneric
Imports System.Data
Imports HTS.SAS.BusinessObjects
Imports HTS.SAS.Entities
Imports System.Collections.Generic
Imports System.Linq

Partial Class StudentDetails
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

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            'MatricNo = Request.QueryString("MatricNo")
            ProgramId = Request.QueryString("ProgramId")
            Sponsor = Request.QueryString("SponsorCode")
            BatchCode = Request.QueryString("BatchCode")
            Status = Request.QueryString("Status")
            BindGrid()
        End If
    End Sub
    Private Sub BindGrid()
        Dim i As Integer = 0
        'dgStudentInfo.DataSource = _StudentDAL.GetStudentDetails(MatricNo, ProgramId)
        'dgStudentInfo.DataSource = _StudentDAL.GetStudentDetailsBySponsor(ProgramId, Sponsor)
        lststud = _StudentDAL.GetStudentDetailsBySponsorWithStuValidity(ProgramId, Sponsor)

        If BatchCode.Trim() <> String.Empty Then
            Dim stu As New StudentEn()
            stu.BatchCode = BatchCode
            lststudA = _AccountsDAL.GetListStudentSponsorInvoicebyBatchID1(stu)
            lststud = lststud.Where(Function(x) lststudA.Any(Function(y) y.MatricNo = x.MatricNo)).ToList()
            'listStu = New List(Of StudentEn)
            'For Each s As StudentEn In lststud
            '    If lststudA.Where(Function(x) x.MatricNo = s.MatricNo).Count() > 0 Then
            '        Dim eobjstu As New StudentEn
            '        eobjstu.ProgramID = Request.QueryString("ProgramId")
            '        eobjstu.SponsorCode = Request.QueryString("SponsorCode")
            '        eobjstu.MatricNo = s.MatricNo
            '        eobjstu.StudentName = s.StudentName
            '        eobjstu.CurrentSemester = s.CurrentSemester
            '        eobjstu.AllocatedAmount = s.AllocatedAmount
            '        eobjstu.OutstandingAmount = s.OutstandingAmount
            '        listStu.Add(eobjstu)
            '    End If

            '    lststud.Item(i).OutstandingAmount = 0.0
            '    lststud.Item(i).PaidAmount = 0.0
            '    i = i + 1

            'Next

            ' Session("lstStu") = listStu
        End If
        Dim lstFromDB As New List(Of StudentEn)
        lstFromDB = lststud.Where(Function(x) x.OutstandingAmount > 0 Or x.SponsorLimit = 0).ToList()
        listStu = lststud.Where(Function(x) x.OutstandingAmount > 0 Or x.SponsorLimit = 0).ToList()
        Dim lstStudNotAvail As New List(Of StudentEn)
        lstStudNotAvail = lststud.Where(Function(x) x.OutstandingAmount <= 0 And x.SponsorLimit <> 0).ToList()
        Session("lstFromDB") = lstFromDB
        Session("lstStu") = listStu
        If lstFromDB.Count > 0 Then
            chkStudent.Visible = True
            Try
                dgStudentInfo.DataSource = lstFromDB
                dgStudentInfo.DataBind()
                chkStudent.Checked = True
                If Status = "Posted" Then
                    chkStudent.Enabled = False
                Else
                    chkStudent.Enabled = True
                End If
                Dim chk As CheckBox
                Dim dgitem As DataGridItem

                For Each dgitem In dgStudentInfo.Items
                    chk = dgitem.Cells(0).Controls(1)
                    chk.Checked = True
                    If Status = "Posted" Then
                        chk.Enabled = False
                    Else
                        chk.Enabled = True
                    End If
                Next
              
            Catch ex As HttpException
                dgStudentInfo.CurrentPageIndex = 0
                dgStudentInfo.DataBind()
                dgStudentInfo.CurrentPageIndex = 1
            End Try
        Else
            chkStudent.Visible = False
            dgStudentInfo.DataSource = Nothing
            dgStudentInfo.DataBind()
        End If
        If lstStudNotAvail.Count > 0 Then
            pnlStuNotAvailable.Visible = True
            dgStudentNotAvailable.DataSource = lstStudNotAvail
        Else
            pnlStuNotAvailable.Visible = False
            dgStudentNotAvailable.DataSource = Nothing
        End If
        dgStudentNotAvailable.DataBind()

        'If lststud.Count > 0 Then
        '    chkStudent.Checked = True
        '    Dim chk As CheckBox
        '    Dim dgitem As DataGridItem

        '    For Each dgitem In dgStudentInfo.Items
        '        chk = dgitem.Cells(0).Controls(1)
        '        chk.Checked = True
        '    Next
        '    Session("lstStu") = lststud
        'End If

        ''For SponsorInvoice
        Session("ProgSelected") = Request.QueryString("ProgramId")
    End Sub


    Protected Sub dgStudentInfo_PageIndexChanged(source As Object, e As DataGridPageChangedEventArgs) Handles dgStudentInfo.PageIndexChanged
        dgStudentInfo.CurrentPageIndex = e.NewPageIndex

        Dim lstFromDB As New List(Of StudentEn)
        lstFromDB = Session("lstFromDB")

        dgStudentInfo.DataSource = lstFromDB
        dgStudentInfo.DataBind()
    End Sub

    Protected Sub chk_CheckedChanged(sender As Object, e As EventArgs)
        Dim chk As CheckBox = (DirectCast(sender, CheckBox))
        Dim dgitem As DataGridItem = DirectCast(chk.Parent.Parent, DataGridItem)
        If Not Session("lstStu") Is Nothing Then
            listStu = Session("lstStu")
        Else
            listStu = New List(Of StudentEn)
        End If

        If chk.Checked Then
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
            listStu.Remove(listStu.Where(Function(x) x.MatricNo = dgitem.Cells(1).Text).Select(Function(x) x).FirstOrDefault())
        End If

        Session("lstStu") = listStu
    End Sub

    Protected Sub dgStudentInfo_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dgStudentInfo.ItemDataBound
        If e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem Then
            If Not Session("lstStu") Is Nothing Then
                listStu = Session("lstStu")
            Else
                listStu = New List(Of StudentEn)
            End If

            Dim listStuFromDB As New List(Of StudentEn)
            If Not Session("lstFromDB") Is Nothing Then
                listStuFromDB = Session("lstFromDB")
            Else
                listStuFromDB = New List(Of StudentEn)
            End If

            If Session("FeeSum") Is Nothing Then
                Session("FeeSum") = 0
            End If
            Dim chk As CheckBox = CType(e.Item.FindControl("chk"), CheckBox)

            'If Convert.ToDouble(Session("FeeSum")) > Convert.ToDouble(e.Item.Cells(4).Text) Then
            '    chk.Checked = False
            '    chk.Enabled = False
            '    If (listStu.Where(Function(x) x.MatricNo = e.Item.Cells(1).Text).Count() > 0) Then
            '        listStu.Remove(listStu.Where(Function(x) x.MatricNo = e.Item.Cells(1).Text).Select(Function(x) x).FirstOrDefault())
            '    End If
            'Else
            '    chk.Checked = (listStu.Where(Function(x) x.MatricNo = e.Item.Cells(1).Text).Count() > 0)
            'End If

            If Convert.ToDouble(Session("FeeSum")) > Convert.ToDouble(e.Item.Cells(4).Text) Then
                chk.Checked = False
                chk.Enabled = False
                If (listStu.Where(Function(x) x.MatricNo = e.Item.Cells(1).Text).Count() > 0) Then
                    listStu.Remove(listStu.Where(Function(x) x.MatricNo = e.Item.Cells(1).Text).Select(Function(x) x).FirstOrDefault())
                End If
            Else
                chk.Checked = (listStu.Where(Function(x) x.MatricNo = e.Item.Cells(1).Text).Count() > 0)
            End If

        End If
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
        Dim cScript As String = ""
        cScript = "<SCRIPT LANGUAGE=""JavaScript"">window.opener.document.getElementById('ctl00_ContentPlaceHolder1_btnHidden').click();self.close();</SCRIPT>"
        ClientScript.RegisterStartupScript(Me.GetType(), "ClosePopUp", cScript)
    End Sub

End Class
