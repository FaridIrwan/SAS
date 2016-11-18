Imports HTS.SAS.Entities
Imports HTS.SAS.BusinessObjects
Imports System.Data
Imports System.Collections.Generic
Partial Class RptSponsorCoverLetter
    Inherits System.Web.UI.Page
    Dim CFlag As String
    Dim DFlag As String
    Dim ListObjects As List(Of SponsorCoverLetterEn)
    Private ErrorDescription As String
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack() Then
            Menuname(CInt(Request.QueryString("Menuid")))
            LoadUserRights()
            ibtnFDate.Attributes.Add("onClick", "return getibtnFDate()")
            ibtnTodate.Attributes.Add("onClick", "return getDateto()")
            txtFrom.Attributes.Add("OnKeyup", "return CheckFromDate()")
            txtTodate.Attributes.Add("OnKeyup", "return CheckToDate()")
            'Adding validation for Save button
            ibtnSave.Attributes.Add("onclick", "return validate()")
            'Adding validation for Delete button
            ibtnDelete.Attributes.Add("onclick", "return getconfirm()")
            txtRecNo.Attributes.Add("OnKeyup", "return geterr()")
            'ibtnPrint.Attributes.Add("onclick", "return dllValues()")
            'ibtnView.Attributes.Add("onclick", "return dllValues()")
            txtFrom.Attributes.Add("OnKeyup", "return CheckFromDate()")
            txtTodate.Attributes.Add("OnKeyup", "return CheckToDate()")
            dates()
        End If
        lblMesg.Visible = False
    End Sub

    Protected Sub ibtnView_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ibtnView.Click
        If lblCount.Text <> "" Then
            If CInt(lblCount.Text) > 0 Then
                'Clear Text Box values
                OnClearData()
                If ibtnNew.Enabled = False Then
                    ibtnSave.Enabled = False
                    ibtnSave.ImageUrl = "images/gsave.png"
                    ibtnSave.ToolTip = "Access Denied"
                End If
            Else
                LoadListObjects()
            End If
        Else
            LoadListObjects()
        End If
    End Sub
#Region "Methods"
    Private Sub dates()
        txtFrom.Text = Format(Date.Now, "dd/MM/yyyy")
        txtTodate.Text = Format(Date.Now, "dd/MM/yyyy")
    End Sub
    ''' <summary>
    ''' Method to Load the UserRights
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub LoadUserRights()
        Dim obj As New UsersBAL
        Dim eobj As UserRightsEn

        eobj = obj.GetUserRights(CInt(Request.QueryString("Menuid")), CInt(Session("UserGroup")))
        'Rights for Add

        If eobj.IsAdd = True Then
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
    ''' <summary>
    ''' Method to Clear the Fields in NewMode
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub OnAdd()
        Session("PageMode") = "Add"
        ibtnSave.Enabled = True
        ibtnSave.ImageUrl = "images/save.png"
        txtFrom.Text = Format(Date.Now, "dd/MM/yyyy")
        txtTodate.Text = Format(Date.Now, "dd/MM/yyyy")
        'Clear Text Box values
        OnClearData()
    End Sub
    ''' <summary>
    ''' Method to Clear the Field Values
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub OnClearData()
        Session("ListObj_Details") = Nothing
        DisableRecordNavigator()
        lblMesg.Text = ""
        txtCode.Enabled = True
        'Clear Text Box values
        txtCode.Text = Nothing
        txtTitle.Text = Nothing
        txtOurRef.Text = Nothing
        txtYourRef.Text = Nothing
        txtMsg.Text = Nothing
        txtAddress.Text = Nothing
        txtSignBy.Text = Nothing
        txtName.Text = Nothing
        Session("PageMode") = "Add"
    End Sub

    Private Sub OnSave()
        lblMesg.Visible = False
        Dim bsobj As New SponsorCoverLetterBAL
        Dim eobj As New SponsorCoverLetterEn
        'Dim RecAff As Integer
        Dim RecAff As Boolean
        eobj.Code = Trim(txtCode.Text)
        eobj.Title = Trim(txtTitle.Text)
        eobj.OurRef = Trim(txtOurRef.Text)
        eobj.YourRef = Trim(txtYourRef.Text)
        eobj.ToDate = Trim(txtTodate.Text)
        eobj.FromDate = Trim(txtFrom.Text)
        eobj.Message = Trim(txtMsg.Text)
        eobj.Name = Trim(txtName.Text)
        eobj.Address = Trim(txtAddress.Text)
        eobj.SignBy = Trim(txtSignBy.Text)
        eobj.UpdatedBy = Session("User")
        eobj.UpdatedTime = Date.Now.ToString()
        lblMesg.Visible = True
        If Session("PageMode") = "Add" Then
            Try

                RecAff = bsobj.Insert(eobj)
                'User Defined Message
                ErrorDescription = "Record Saved Successfully "
                lblMesg.Text = ErrorDescription
                txtCode.Enabled = False
            Catch ex As Exception
                lblMesg.Text = ex.Message.ToString()
            End Try
        ElseIf Session("PageMode") = "Edit" Then
            Try
                RecAff = bsobj.Update(eobj)
                ListObjects = Session("ListObj_Details")
                ListObjects(CInt(txtRecNo.Text) - 1) = eobj
                Session("ListObj_Details") = ListObjects
                'User Defined Message
                ErrorDescription = "Record Updated Successfully "
                lblMesg.Text = ErrorDescription
            Catch ex As Exception
                lblMesg.Text = ex.Message.ToString()
            End Try
        End If

    End Sub

    Private Sub OnDelete()
        lblMesg.Visible = True
        If txtCode.Text <> "" Then
            If lblCount.Text = "" Then lblCount.Text = 0
            If lblCount.Text > 0 Then
                Dim bsobj As New SponsorCoverLetterBAL
                Dim eobj As New SponsorCoverLetterEn
                Dim RecAff As Integer
                lblMesg.Visible = True
                eobj.Code = txtCode.Text
                Try
                    RecAff = bsobj.Delete(eobj)
                    ListObjects = Session("ListObj_Details")
                    ListObjects.RemoveAt(CInt(txtRecNo.Text) - 1)
                    lblCount.Text = lblCount.Text - 1
                    Session("ListObj_Details") = ListObjects
                    lblMesg.Visible = True
                    ErrorDescription = "Record Deleted Successfully "
                    lblMesg.Text = ErrorDescription
                Catch ex As Exception
                    lblMesg.Text = ex.Message.ToString()
                End Try
                txtCode.Text = Nothing
                txtTitle.Text = Nothing
                txtMsg.Text = Nothing
                txtAddress.Text = Nothing
                txtSignBy.Text = Nothing
                txtName.Text = Nothing
                txtOurRef.Text = Nothing
                DFlag = "Delete"
                LoadListObjects()
            Else
                ErrorDescription = "Record not Seleted"
                lblMesg.Text = ErrorDescription
            End If
        Else
            ErrorDescription = "Record not Seleted"
            lblMesg.Text = ErrorDescription
        End If

    End Sub

    Public Sub LoadListObjects()
        Dim ds As New DataSet
        Dim bobj As New SponsorCoverLetterBAL
        Dim eobj As New SponsorCoverLetterEn

        eobj.Code = Trim(txtCode.Text)
        eobj.Title = Trim(txtTitle.Text)
        ListObjects = bobj.GetList(eobj)
        Session("ListObj_Details") = ListObjects
        lblCount.Text = ListObjects.Count.ToString()
        If ListObjects.Count <> 0 Then
            DisableRecordNavigator()
            txtRecNo.Text = "1"
            OnMoveFirst()
            If Session("EditFlag") = True Then
                Session("PageMode") = "Edit"
                txtCode.Enabled = False
                ibtnSave.Enabled = True
                ibtnSave.ImageUrl = "images/save.png"
                lblMesg.Visible = True
            Else
                Session("PageMode") = ""
                ibtnSave.Enabled = False
                ibtnSave.ImageUrl = "images/gsave.png"
                ErrorDescription = "Record did not Exist"
                lblMesg.Text = ErrorDescription
                lblMesg.Visible = True
            End If
        Else
            txtRecNo.Text = ""
            lblCount.Text = ""
            'Clear Text Box values
            OnClearData()
            If DFlag = "Delete" Then
            Else

                ErrorDescription = "Record did not Exist"
                lblMesg.Text = ErrorDescription
                lblMesg.Visible = True
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
        FillData(CInt(lblCount.Text) - 1)
    End Sub
    ''' <summary>
    ''' Method to Enable or Disable Navigation Buttons
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub DisableRecordNavigator()
        Dim flag As Boolean
        If Session("ListObj_Details") Is Nothing Then
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

    Private Sub FillData(ByVal RecNo As Integer)
        lblMesg.Visible = False
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
        If txtRecNo.Text = 0 Then
            txtRecNo.Text = 1
        Else
            If lblCount.Text = 0 Then
                txtRecNo.Text = 0
            Else

                Dim obj As SponsorCoverLetterEn
                ListObjects = Session("ListObj_Details")
                obj = ListObjects(RecNo)
                txtCode.Text = obj.Code
                txtTitle.Text = obj.Title
                txtMsg.Text = obj.Message
                txtAddress.Text = obj.Address
                txtSignBy.Text = obj.SignBy
                txtName.Text = obj.Name
                txtFrom.Text = obj.FromDate
                txtOurRef.Text = obj.OurRef
                txtYourRef.Text = obj.YourRef
                txtTodate.Text = obj.ToDate

            End If
        End If
        setDateFormat()
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
    ''' Method to Validate
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub SpaceValidation()
        ibtnSave.Attributes.Add("onClick", "return validate()")
        Dim GBFormat As System.Globalization.CultureInfo
        GBFormat = New System.Globalization.CultureInfo("en-GB")

        'Batch date
        If Trim(txtFrom.Text).Length < 10 Then
            lblMesg.Text = "Enter Valid From Date"
            lblMesg.Visible = True
            txtFrom.Focus()
            Exit Sub
        Else
            Try
                txtFrom.Text = DateTime.Parse(txtFrom.Text, GBFormat)
            Catch ex As Exception
                lblMesg.Text = "Enter Valid From Date"
                lblMesg.Visible = True
                txtFrom.Focus()
                Exit Sub
            End Try
        End If
        'Invoice date
        If Trim(txtTodate.Text).Length < 10 Then
            lblMesg.Text = "Enter Valid To Date"
            lblMesg.Visible = True
            txtTodate.Focus()
            Exit Sub
        Else
            Try
                txtTodate.Text = DateTime.Parse(txtTodate.Text, GBFormat)
            Catch ex As Exception
                lblMesg.Text = "Enter Valid To Date"
                lblMesg.Visible = True
                txtTodate.Focus()
                Exit Sub
            End Try
        End If


    End Sub

    ''' <summary>
    ''' Method To Change the Date Format(dd/MM/yyyy)
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub setDateFormat()
        Dim myInvoiceDate As Date = CDate(CStr(txtFrom.Text))
        Dim myFormat As String = "dd/MM/yyyy"
        Dim myFormattedDate As String = Format(myInvoiceDate, myFormat)
        txtFrom.Text = myFormattedDate
        Dim myDuedate As Date = CDate(CStr(txtTodate.Text))
        Dim myFormattedDate1 As String = Format(myDuedate, myFormat)
        txtTodate.Text = myFormattedDate1

    End Sub
#End Region

    Protected Sub ibtnSave_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ibtnSave.Click
        SpaceValidation()
        OnSave()
        setDateFormat()
    End Sub

    Protected Sub ibtnCancel_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)

    End Sub

    Protected Sub ibtnNext_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        OnMoveNext()
    End Sub

    Protected Sub ibtnPrevs_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        OnMovePrevious()
    End Sub

    Protected Sub ibtnFirst_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        OnMoveFirst()
    End Sub

    Protected Sub ibtnLast_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        OnMoveLast()
    End Sub

    Protected Sub ibtnCancel_Click1(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ibtnCancel.Click
        LoadUserRights()
        OnAdd()
    End Sub

    Protected Sub ibtnNew_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ibtnNew.Click
        OnAdd()
    End Sub

    Protected Sub ibtnDelete_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ibtnDelete.Click
        OnDelete()
    End Sub
End Class
