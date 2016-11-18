Imports System.Collections.Generic
Imports HTS.SAS.Entities
Imports HTS.SAS.BusinessObjects
Imports System.Data
Partial Class Kolej
    Inherits System.Web.UI.Page
    Dim CFlag As String
    Dim DFlag As String
    Dim ListObjects As List(Of KolejEn)
    Private ErrorDescription As String
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        lblMsg.Visible = False
        If Not IsPostBack() Then
            'Adding validation for save button
            ibtnSave.Attributes.Add("onclick", "return validate()")
            ibtnDelete.Attributes.Add("onclick", "return getconfirm()")
            txtRecNo.Attributes.Add("OnKeyUp", "return geterr()")
            'While loading the page make the CFlag as null
            Session("PageMode") = ""
            'Loading User Rights
            LoadUserRights()

            'while loading list object make it nothing
            Session("ListObj") = Nothing
            DisableRecordNavigator()
            'load PageName
            Menuname(CInt(Request.QueryString("Menuid")))
        End If
    End Sub


#Region "Methods"
    ''' <summary>
    ''' Method to Load UserRights
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub LoadUserRights()
        Dim obj As New UsersBAL
        Dim eobj As New UserRightsEn

        Try
            eobj = obj.GetUserRights(CInt(Request.QueryString("Menuid")), CInt(Session("UserGroup")))
        Catch ex As Exception
            LogError.Log("Kolej", "LoadUserRights", ex.Message)
        End Try
        'Rights for Add

        If eobj.IsAdd = True Then
            'ibtnSave.Enabled = True
            OnAdd()
            ibtnNew.ImageUrl = "images/add.png"
            ibtnNew.Enabled = True
        Else
            ibtnNew.ImageUrl = "images/gadd.png"
            ibtnNew.Enabled = False
            ibtnNew.ToolTip = "Access Denied"

        End If
        'Rights for Edit
        If eobj.IsEdit = True Then
            ibtnSave.Enabled = True
            ibtnSave.ImageUrl = "images/save.png"
            ibtnSave.ToolTip = "Edit"
            If eobj.IsAdd = False Then
                ibtnSave.Enabled = False
                ibtnSave.ImageUrl = "images/gsave.png"
                ibtnSave.ToolTip = "Access Denied"
            End If

            Session("EditFlag") = True

        Else
            Session("EditFlag") = False
            ibtnSave.Enabled = False
            ibtnSave.ImageUrl = "images/gsave.png"
        End If
        'Rights for View
        ibtnView.Enabled = eobj.IsView
        If eobj.IsView = True Then
            ibtnView.ImageUrl = "images/find.png"
            ibtnView.Enabled = True
        Else
            ibtnView.ImageUrl = "images/gfind.png"
            ibtnView.ToolTip = "Access Denied"
        End If
        'Rights for Delete
        If eobj.IsDelete = True Then
            ibtnDelete.ImageUrl = "images/delete.png"
            ibtnDelete.Enabled = True
        Else
            ibtnDelete.ImageUrl = "images/gdelete.png"
            ibtnDelete.ToolTip = "Access Denied"
            ibtnDelete.Enabled = False
        End If
        'Rights for Print
        ibtnPrint.Enabled = eobj.IsPrint
        If eobj.IsPrint = True Then
            ibtnPrint.Enabled = True
            ibtnPrint.ImageUrl = "images/print.png"
            ibtnPrint.ToolTip = "Print"
        Else
            ibtnPrint.Enabled = False
            ibtnPrint.ImageUrl = "images/gprint.png"
            ibtnPrint.ToolTip = "Access Denied"
        End If
        If eobj.IsOthers = True Then
            ibtnOthers.Enabled = True
            ibtnOthers.ImageUrl = "images/others.png"
            ibtnOthers.ToolTip = "Others"
        Else
            ibtnOthers.Enabled = False
            ibtnOthers.ImageUrl = "images/gothers.png"
            ibtnOthers.ToolTip = "Access Denied"
        End If
        If eobj.IsPost = True Then
            ibtnPosting.Enabled = True
            ibtnPosting.ImageUrl = "images/posting.png"
            ibtnPosting.ToolTip = "Posting"
        Else
            ibtnPosting.Enabled = False
            ibtnPosting.ImageUrl = "images/gposting.png"
            ibtnPosting.ToolTip = "Access Denied"
        End If
    End Sub

    Private Sub OnAdd()
        Session("PageMode") = "Add"
        'pnlAdd.Visible = True
        OnClearData()
        'dgView.SelectedIndex = -1
        'PnlView.Visible = False
        'lblMsg.Visible = True
        'lblMsg.Text = Session("PageMode")
    End Sub
    ''' <summary>
    ''' Method to Change  the Session to Edit Mode
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub OnEdit()
        Session("PageMode") = "Edit"
        txtKolejCode.Enabled = False

    End Sub
    ''' <summary>
    ''' Method to Clear Field Vlues
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub OnClearData()
        txtKolejCode.Enabled = True
        'Clear Text Box values
        Session("ListObj") = Nothing
        DisableRecordNavigator()
        txtKolejCode.Text = ""
        txtKolejDesc.Text = ""

        Session("PageMode") = "Add"
    End Sub

    ''' <summary>
    ''' Method to Save and Update Kolej
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub OnSave()
        Dim bsobj As New KolejBAL
        Dim eobj As New KolejEn
        Dim RecAff As Integer

        eobj.SAKO_Code = Trim(txtKolejCode.Text)
        eobj.SAKO_Description = Trim(txtKolejDesc.Text)

        lblMsg.Visible = True
        If Session("PageMode") = "Add" Then
            Try
                RecAff = bsobj.Insert(eobj)

                If RecAff = -1 Then
                    ErrorDescription = "Record Saved Successfully "
                    lblMsg.Text = ErrorDescription
                ElseIf RecAff = 0 Then
                    ErrorDescription = "Record already exist "
                    lblMsg.Text = ErrorDescription
                End If

            Catch ex As Exception
                lblMsg.Text = ex.Message.ToString
                LogError.Log("Kolej", "OnSave", ex.Message)
            End Try
        ElseIf Session("PageMode") = "Edit" Then
            Try

                RecAff = bsobj.Update(eobj)
                ListObjects = Session("ListObj")
                ListObjects(CInt(txtRecNo.Text) - 1) = eobj
                Session("ListObj") = ListObjects
                ErrorDescription = "Record is Updated Successfully "
                lblMsg.Text = ErrorDescription
            Catch ex As Exception
                lblMsg.Text = ex.Message.ToString
                LogError.Log("Kolej", "OnSave", ex.Message)
            End Try
        End If
        Session("PageMode") = ""
        'Changed by Zoya @ 19/02/2016
        txtKolejCode.Enabled = False
        'changed end
    End Sub
    ''' <summary>
    ''' Method to Delete Kolej
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub OnDelete()
        lblMsg.Visible = True
        If txtKolejCode.Text <> "" Then
            If lblCount.Text = "" Then lblCount.Text = 0
            If lblCount.Text > 0 Then
                Dim bsobj As New KolejBAL
                Dim eobj As New KolejEn

                Dim RecAff As Integer
                lblMsg.Visible = True
                eobj.SAKO_Code = txtKolejCode.Text
                Try
                    RecAff = bsobj.Delete(eobj)
                    ListObjects = Session("ListObj")
                    ListObjects.RemoveAt(CInt(txtRecNo.Text) - 1)
                    lblCount.Text = lblCount.Text - 1
                    Session("ListObj") = ListObjects
                    ErrorDescription = "Record Deleted Successfully "
                    lblMsg.Text = ErrorDescription
                Catch ex As Exception
                    lblMsg.Text = ex.Message.ToString()
                    LogError.Log("Kolej", "OnDelete", ex.Message)
                End Try
                txtKolejCode.Text = ""
                txtKolejDesc.Text = ""
                DFlag = "Delete"
                LoadListObjects()
            Else
                ErrorDescription = "Select a Record to Delete"
                lblMsg.Text = ErrorDescription
            End If
        Else
            ErrorDescription = "Select a Record to Delete"
            lblMsg.Text = ErrorDescription
        End If

    End Sub
    ''' <summary>
    ''' Method to Get List of Kolej and Load
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub LoadListObjects()
        Dim eob As New KolejEn
        Dim bobj As New KolejBAL


        If txtKolejCode.Text = "" Then txtKolejCode.Text = "*"
        eob.SAKO_Code = Trim(txtKolejCode.Text)
        eob.SAKO_Description = Trim(txtKolejDesc.Text)
        Try
            ListObjects = bobj.GetKolejList(eob)
        Catch ex As Exception
            LogError.Log("Kolej", "LoadListObjects", ex.Message)
        End Try
        Session("ListObj") = ListObjects
        lblCount.Text = ListObjects.Count.ToString()

        If ListObjects.Count <> 0 Then
            DisableRecordNavigator()
            If lblCount.Text = 0 Then
                txtRecNo.Text = 0
            Else
                txtRecNo.Text = 1
            End If
            'PnlView.Visible = False
            'PnlAdd.Visible = True
            OnMoveFirst()
            If Session("EditFlag") = True Then
                Session("PageMode") = "Edit"
                txtKolejCode.Enabled = False
            Else
                Session("PageMode") = ""
                ibtnSave.Enabled = False
                ibtnSave.ImageUrl = "images/gsave.png"
            End If
        Else
            txtRecNo.Text = ""
            lblCount.Text = ""
            OnClearData()

            If DFlag = "Delete" Then
            Else
                lblMsg.Visible = True
                ErrorDescription = "Record did not Exist"
                lblMsg.Text = ErrorDescription
                DFlag = ""
            End If

        End If
    End Sub
    ''' <summary>
    ''' Method to Move to First Record
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub OnMoveFirst()
        txtRecNo.Text = "1"
        FillData(0)
    End Sub
    ''' <summary>
    ''' Method to Move to Next Record
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub OnMoveNext()
        txtRecNo.Text = CInt(txtRecNo.Text) + 1
        FillData(CInt(txtRecNo.Text) - 1)
    End Sub
    ''' <summary>
    ''' Method to Move to Previous Record
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub OnMovePrevious()
        txtRecNo.Text = CInt(txtRecNo.Text) - 1
        FillData(CInt(txtRecNo.Text) - 1)
    End Sub
    ''' <summary>
    ''' Method to Move to Last Record
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub OnMoveLast()
        txtRecNo.Text = lblCount.Text
        FillData(CInt(txtRecNo.Text) - 1)
    End Sub
    ''' <summary>
    ''' Method to Enable or Disable Navigation Buttons
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub DisableRecordNavigator()
        Dim flag As Boolean
        If Session("ListObj") Is Nothing Then
            flag = False
            txtRecNo.Text = ""
            lblCount.Text = ""
        Else
            flag = True
        End If
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
    ''' <summary>
    ''' Method to Display the list of DegreeTypes
    ''' </summary>
    ''' <param name="RecNo">Parameter is RecNo</param>
    ''' <remarks></remarks>
    Private Sub FillData(ByVal RecNo As Integer)
        'Conditions for Button Enable & Disable
        If txtRecNo.Text = lblCount.Text Then
            ibtnNext.Enabled = False
            ibtnNext.ImageUrl = "images/gnew_next.png"
            ibtnLast.Enabled = False
            ibtnLast.ImageUrl = "images/gnew_last.png"
        Else
            ibtnNext.Enabled = True
            ibtnNext.ImageUrl = "images/new_next.png"
            ibtnLast.Enabled = True
            ibtnLast.ImageUrl = "images/new_last.png"
        End If
        If txtRecNo.Text = "1" Then
            ibtnPrevs.Enabled = False
            ibtnPrevs.ImageUrl = "images/gnew_Prev.png"
            ibtnFirst.Enabled = False
            ibtnFirst.ImageUrl = "images/gnew_first.png"
        Else
            ibtnPrevs.Enabled = True
            ibtnPrevs.ImageUrl = "images/new_prev.png"
            ibtnFirst.Enabled = True
            ibtnFirst.ImageUrl = "images/new_first.png"
        End If
        If lblCount.Text = 0 Then
            txtRecNo.Text = 0
        Else
            'PnlView.Visible = False
            'PnlAdd.Visible = True
            Dim obj As KolejEn
            ListObjects = Session("ListObj")
            obj = ListObjects(RecNo)

            txtKolejCode.Text = obj.SAKO_Code
            txtKolejDesc.Text = obj.SAKO_Description

        End If
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
        Try
            eobj = bobj.GetMenus(eobj)
        Catch ex As Exception
            LogError.Log("Kolej", "Menuname", ex.Message)
        End Try
        lblMenuName.Text = eobj.MenuName
    End Sub
#End Region

    Protected Sub ibtnView_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ibtnView.Click
        If lblCount.Text <> "" Then
            If CInt(lblCount.Text) > 0 Then
                OnClearData()
            Else
                LoadListObjects()
            End If
        Else
            LoadListObjects()
        End If
    End Sub

    Protected Sub ibtnSave_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ibtnSave.Click
        OnSave()
    End Sub

    Protected Sub ibtnDelete_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ibtnDelete.Click
        OnDelete()
    End Sub

    Protected Sub txtRecNo_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        If lblCount.Text <> Nothing Then
            If CInt(txtRecNo.Text) > CInt(lblCount.Text) Then
                txtRecNo.Text = lblCount.Text
            End If
            FillData(CInt(txtRecNo.Text) - 1)
        Else
            txtRecNo.Text = ""
        End If
    End Sub

    Protected Sub ibtnPrevs_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ibtnPrevs.Click
        OnMovePrevious()
    End Sub

    Protected Sub ibtnNext_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ibtnNext.Click
        OnMoveNext()
    End Sub

    Protected Sub ibtnLast_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ibtnLast.Click
        OnMoveLast()
    End Sub

    Protected Sub ibtnFirst_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ibtnFirst.Click
        OnMoveFirst()
    End Sub

    Protected Sub ibtnNew_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ibtnNew.Click
        OnAdd()
    End Sub

    Protected Sub ibtnCancel_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ibtnCancel.Click
        OnClearData()
    End Sub
End Class
