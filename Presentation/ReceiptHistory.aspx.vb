Imports HTS.SAS.Entities
Imports HTS.SAS.BusinessObjects
Imports System.Data
Imports System.Collections.Generic
Partial Class ReceiptHistory
    Inherits System.Web.UI.Page

#Region "Declaration"
    Dim column As String() = {"UpdatedTime", "BatchCode", "UpdatedBy", "TransactionCode"}
    Dim CFlag As String
    Dim DFlag As String
    Dim ListObjects As List(Of DunningLettersEn)
    Private ErrorDescription As String
#End Region

#Region "Properties"

#End Region

#Region "Page and Control Events"

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack() Then
            ibtnFDate.Attributes.Add("onClick", "return getibtnFDate()")
            ibtnTDate.Attributes.Add("onClick", "return getDateto()")
            txtFrom.Text = Format(Date.Now, "dd/MM/yyyy")
            txtTo.Text = Format(Date.Now, "dd/MM/yyyy")
            FixEmptyRow(dgStudent, column)
        End If
        If (dgStudent.Items(0).Cells.Count > 1) Then
            If String.IsNullOrEmpty(dgStudent.Items(0).Cells(1).Text) Then
                FixEmptyRow(dgStudent, column)
            End If
        End If

    End Sub


    Protected Sub btnFind_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnFind.Click
        Dim objup As New AccountsBAL
        Dim lstobjects As New List(Of AccountsEn)
        Dim eob As New AccountsEn

        Dim datef As String = Trim(txtFrom.Text)
        Dim datet As String = Trim(txtTo.Text)
        Dim datelen As Integer = Len(datef)
        Dim d1, m1, y1, d2, m2, y2 As String
        d1 = Mid(datef, 1, 2)
        m1 = Mid(datef, 4, 2)
        y1 = Mid(datef, 7, 4)
        Dim datefrom As String = y1 + "/" + m1 + "/" + d1
        d2 = Mid(datet, 1, 2)
        m2 = Mid(datet, 4, 2)
        y2 = Mid(datet, 7, 4)
        Dim dateto As String = y2 + "/" + m2 + "/" + d2

        Try
            lstobjects = objup.GetReceiptHistory(datefrom, dateto)

        Catch ex As Exception
            LogError.Log("Receipt History", "btnFind_Click", ex.Message)
        End Try


        If Not lstobjects Is Nothing And lstobjects.Count > 0 Then
            Session("lstOutstandingStudents") = lstobjects
            dgStudent.DataSource = lstobjects
            dgStudent.DataBind()
        Else
            FixEmptyRow(dgStudent, column)
        End If


    End Sub

    Protected Sub ibtnNew_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ibtnNew.Click
        OnAdd()
        FixEmptyRow(dgStudent, column)
    End Sub

    Protected Sub ibtnCancel_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ibtnCancel.Click
        LoadUserRights()
        OnAdd()
        FixEmptyRow(dgStudent, column)
    End Sub


#End Region

#Region "User Defined Mothods"

    ''' <summary>
    ''' This method is to bind empty row with gridview
    ''' </summary>
    ''' <param name="gridView">gridview to bind</param>
    ''' <param name="Columns">collection of columns</param>
    Private Sub FixEmptyRow(ByVal gridView As DataGrid, ByVal Columns As String())
        Dim dt As New DataTable()
        For i As Integer = 0 To Columns.Length - 1
            Dim dcNew As New DataColumn(Columns(i))
            dcNew.AllowDBNull = True
            dt.Columns.Add(dcNew)
        Next
        dt.Rows.Add(dt.NewRow())
        gridView.DataSource = dt
        gridView.DataBind()

        Dim columnCount As Integer = gridView.Items(0).Cells.Count
        gridView.Items(0).Cells.Clear()
        gridView.Items(0).Cells.Add(New TableCell())
        gridView.Items(0).Cells(0).ColumnSpan = columnCount
        gridView.Items(0).Cells(0).Text = "There is no record"
        gridView.ItemStyle.HorizontalAlign = HorizontalAlign.Left

    End Sub

    ''' <summary>
    ''' Method to Load the UserRights
    ''' </summary>
    ''' <remarks></remarks>
    ''' 
    Private Sub LoadUserRights()
        Dim obj As New UsersBAL
        Dim eobj As UserRightsEn

        eobj = obj.GetUserRights(CInt(Request.QueryString("Menuid")), CInt(Session("UserGroup")))
        'Rights for Add

        ibtnNew.ImageUrl = "images/gadd.png"
        ibtnNew.Enabled = False
        ibtnNew.ToolTip = "Access Denied"
        'Else
        Session("EditFlag") = False
        ibtnSave.Enabled = False
        ibtnSave.ImageUrl = "images/gsave.png"
        'End If
        'Rights for View
        ibtnView.Enabled = False ' eobj.IsView
        ibtnView.ImageUrl = "images/gfind.png"
        ibtnView.ToolTip = "Access Denied"
        'Rights for Delete
        ibtnDelete.ImageUrl = "images/gdelete.png"
        ibtnDelete.ToolTip = "Access Denied"
        ibtnDelete.Enabled = False

        'Rights for Print
        ibtnPrint.Enabled = eobj.IsPrint

        ibtnPrint.Enabled = False
        ibtnPrint.ImageUrl = "images/gprint.png"
        ibtnPrint.ToolTip = "Access Denied"

        'ibtnPosting.Enabled = False
        'ibtnPosting.ImageUrl = "images/gposting.png"
        'ibtnPosting.ToolTip = "Access Denied"

    End Sub

    ''' <summary>
    ''' Method to get Menu Name
    ''' </summary>
    ''' <param name="MenuId"></param>
    ''' <remarks></remarks>
    Private Sub Menuname(ByVal MenuId As Integer)
        Dim eobj As New MenuEn
        Dim bobj As New MenuBAL
        eobj.MenuId = MenuId
        eobj = bobj.GetMenus(eobj)
        lblMenuName.Text = eobj.MenuName
    End Sub

    ''' <summary>
    ''' Method to Clear the Fields in NewMode
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub OnAdd()
        Session("PageMode") = "Add"
        ibtnSave.Enabled = False
        ibtnSave.ImageUrl = "images/gsave.png"
        Session.Remove("lstOutstandingStudents")
        lblMesg.Text = String.Empty
        txtFrom.Text = Format(Date.Now, "dd/MM/yyyy")
        txtTo.Text = Format(Date.Now, "dd/MM/yyyy")

    End Sub

    ''' <summary>
    ''' Method to Enable or Disable Navigation Buttons
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub DisableRecordNavigator()
        Dim flag As Boolean
        flag = False
        ibtnFirst.Enabled = flag
        ibtnLast.Enabled = flag
        ibtnPrevs.Enabled = flag
        ibtnNext.Enabled = flag
        If flag = False Then
            ibtnFirst.ImageUrl = "images/gnew_first.png"
            ibtnLast.ImageUrl = "images/gnew_last.png"
            ibtnPrevs.ImageUrl = "images/gnew_Prev.png"
            ibtnNext.ImageUrl = "images/gnew_next.png"
        Else
            ibtnFirst.ImageUrl = "images/new_last.png"
            ibtnLast.ImageUrl = "images/new_first.png"
            ibtnPrevs.ImageUrl = "images/new_Prev.png"
            ibtnNext.ImageUrl = "images/new_next.png"
        End If
    End Sub



#End Region


    Protected Sub ibtnSave_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)

    End Sub
End Class
