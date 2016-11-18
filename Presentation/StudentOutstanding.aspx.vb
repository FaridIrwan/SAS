Imports HTS.SAS.Entities
Imports HTS.SAS.BusinessObjects
Imports System.Data
Imports System.Collections.Generic
Imports HTS.SAS.DataAccessObjects
'new import by Hafiz @ 04/3/2016
Imports MaxGeneric
Imports System.IO

Partial Class StudentOutstanding
    Inherits System.Web.UI.Page

#Region "Declaration"
    Dim column As String() = {"MatricNo", "StudentName", "ProgramID", "CurrentSemester", "OutStandingAmount", "IsReleased"}
    Dim CFlag As String
    Dim DFlag As String
    Dim ListObjects As List(Of DunningLettersEn)
    Private ErrorDescription As String
#End Region

#Region "Page and Control Events"

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack() Then
            LoadProgram()
            LoadSemester()
            FixEmptyRow(dgStudent, column)
            'added by Hafiz @ 04/3/2016
            Session("fileSponsor") = Nothing
            btnUpload.Attributes.Add("onclick", "new_window=window.open('File.aspx','Hanodale','width=470,height=380,resizable=0');new_window.focus();")

            'Added by Hafiz @ 23/2/2016
            SearchStudOutstandingAmt()

        End If
        If (dgStudent.Items(0).Cells.Count > 1) Then
            If String.IsNullOrEmpty(dgStudent.Items(0).Cells(1).Text) Then
                FixEmptyRow(dgStudent, column)
            End If
        End If

        'Added by Hafiz @ 04/3/2016
        CheckUploadedFile()

    End Sub


    Protected Sub btnFind_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnFind.Click
        Dim objup As New StudentBAL
        Dim lstobjects As New List(Of StudentEn)
        Dim eob As New StudentEn
        Dim chkReleaseStatus As CheckBox
        Dim releaseStatus As Integer = 0

        'Added by Hafiz @ 04/3/2016
        CheckUploadedFile()

        If ddlProgram.SelectedValue.ToString().Equals("-1") Then
            eob.ProgramID = ""
        Else
            eob.ProgramID = ddlProgram.SelectedValue.ToString()
        End If


        If ddlSemester.SelectedValue.ToString().Equals("-1") Then
            eob.CurretSemesterYear = ""

        Else
            eob.CurretSemesterYear = ddlSemester.SelectedValue.ToString()

        End If


        Try
            'updated by Hafiz Roslan @ 5/2/2016
            'added checkbox for the Include Current Semester - START
            If chkExcludeLoan.Checked Then
                If chkExcludeCurSem.Checked Then
                    lstobjects = objup.GetListOutstanding(eob, True, True)
                Else
                    lstobjects = objup.GetListOutstanding(eob, True, False)
                End If
            Else
                If chkExcludeCurSem.Checked Then
                    lstobjects = objup.GetListOutstanding(eob, False, True)
                Else
                    lstobjects = objup.GetListOutstanding(eob, False, False)
                End If
            End If
            'added checkbox for the Include Current Semester - END

        Catch ex As Exception
            LogError.Log("Student Dunning Letter", "btnFind_Click", ex.Message)
        End Try

        If Not lstobjects Is Nothing And lstobjects.Count > 0 Then
            Session("lstOutstandingStudents") = lstobjects
            dgStudent.DataSource = lstobjects
            dgStudent.DataBind()

            ' Check the Check Box if it already checked
            For Each dgItem1 In dgStudent.Items
                chkReleaseStatus = dgItem1.Cells(5).Controls(1)
                releaseStatus = CInt(dgItem1.Cells(6).Text)
                'Change by Hafiz Roslan
                'On 08/01/2016
                'is_released = 2 for the EXEMPTED status
                If releaseStatus.Equals(2) Then
                    chkReleaseStatus.Checked = True
                End If
            Next
        Else
            FixEmptyRow(dgStudent, column)
        End If


    End Sub


    Protected Sub ddlProgram_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlProgram.SelectedIndexChanged
        If ddlProgram.SelectedIndex > 0 Then
            ddlSemester.Enabled = True
        Else
            ddlSemester.SelectedIndex = 0
            ddlSemester.Enabled = False
        End If
        FixEmptyRow(dgStudent, column)
    End Sub

    Protected Sub ibtnNew_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ibtnNew.Click
        OnAdd()
        FixEmptyRow(dgStudent, column)

        'added by Hafiz @ 04/3/2016
        Session("fileSponsor") = Nothing
        CheckUploadedFile()

    End Sub

    Protected Sub ibtnCancel_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ibtnCancel.Click
        LoadUserRights()
        OnAdd()
        FixEmptyRow(dgStudent, column)

        'added by Hafiz @ 04/3/2016
        Session("fileSponsor") = Nothing
        CheckUploadedFile()

    End Sub


#End Region

#Region "User Defined Mothods"

    ''' <summary>
    ''' Method to Fill the Semester DropDown
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub LoadSemester()
        Dim eSemester As New SemesterSetupEn
        Dim bSemester As New SemesterSetupBAL
        Dim listSem As New List(Of SemesterSetupEn)

        ddlSemester.Items.Add(New ListItem("---Select---", "-1"))
        ddlSemester.DataTextField = "SemisterSetupCode"
        ddlSemester.DataValueField = "SemisterSetupCode"
        eSemester.SemisterSetupCode = "%"

        Try
            listSem = bSemester.GetListSemesterCode(eSemester)
        Catch ex As Exception
            LogError.Log("FeePosting", "addsemester", ex.Message)
        End Try
        ddlSemester.DataSource = listSem
        ddlSemester.DataBind()
        'Session("faculty") = listfac
    End Sub

    ''' <summary>
    ''' Method to Load Programs Dropdown
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub LoadProgram()
        Dim objBAL As New ProgramInfoBAL
        Dim listObj As New List(Of ProgramInfoEn)
        Dim loen As New ProgramInfoEn
        loen.ProgramCode = ""
        loen.Program = ""
        loen.ProgramBM = ""
        loen.Status = True
        loen.Faculty = ""
        loen.ProgramType = ""
        Try
            listObj = objBAL.GetProgramInfoList(loen)
        Catch ex As Exception
            LogError.Log("StudentDunningLetter", "LoadProgram", ex.Message)
        End Try
        ddlProgram.Items.Clear()
        ddlProgram.Items.Add(New ListItem("--Select--", "-1"))
        ddlProgram.DataTextField = "CodeProgram"
        ddlProgram.DataValueField = "ProgramCode"
        ddlProgram.DataSource = listObj
        ddlProgram.DataBind()
    End Sub


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

        'If eobj.IsAdd = True Then
        '    OnAdd()
        '    ibtnNew.ImageUrl = "images/add.png"
        '    ibtnNew.Enabled = True
        'Else
        'ibtnNew.ImageUrl = "images/gadd.png"
        'ibtnNew.Enabled = False
        'ibtnNew.ToolTip = "Access Denied"

        'End If
        'Rights for Edit
        'If eobj.IsEdit = True Then
        '    ibtnSave.Enabled = True
        '    ibtnSave.ImageUrl = "images/save.png"
        '    ibtnSave.ToolTip = "Edit"
        '    If eobj.IsAdd = False Then
        '        ibtnSave.Enabled = False
        '        ibtnSave.ImageUrl = "images/gsave.png"
        '        ibtnSave.ToolTip = "Access Denied"
        '    End If

        '    Session("EditFlag") = True

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

        'ibtnOthers.Enabled = False
        'ibtnOthers.ImageUrl = "images/gothers.png"
        'ibtnOthers.ToolTip = "Access Denied"

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
        ibtnSave.Enabled = True
        ibtnSave.ImageUrl = "images/save.png"
        Session.Remove("lstOutstandingStudents")
        lblMesg.Text = String.Empty
        ddlProgram.SelectedIndex = 0
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

#Region "SearchStudOutstandingAmt"

    'Added by Hafiz @ 23/2/2016
    'modified by Hafiz @ 27/5/2016
    'Find students with outstanding, insert into sas_outstanding

    Private Sub SearchStudOutstandingAmt()

        Dim res As Boolean = False
        Dim _StudentDAL As New StudentDAL
        Dim _AccountsDAL As New AccountsDAL
        Dim stuobj As New StudentEn
        Dim list_acc As New List(Of StudentEn)

        Try
            stuobj.MatricNo = ""

            list_acc = _StudentDAL.GetList(stuobj)

            If Not list_acc Is Nothing And list_acc.Count > 0 Then

                For Each acc_obj As StudentEn In list_acc
                    Dim accobj As New AccountsEn

                    accobj.CreditRef = acc_obj.MatricNo
                    accobj.PostStatus = "Posted"
                    accobj.SubType = "Student"
                    accobj.TransType = ""
                    accobj.TransStatus = ""

                    Dim cr As Double = 0.0, dr As Double = 0.0

                    Dim ledger As List(Of AccountsEn) = _AccountsDAL.GetStudentLedgerCombine(accobj)
                    For Each ldgr In ledger

                        If ldgr.TransType = "Credit" Then
                            cr = cr + ldgr.TransactionAmount
                        ElseIf ldgr.TransType = "Debit" Then
                            If Not ldgr.Category = "Receipt" Then
                                dr = dr + ldgr.TransactionAmount
                            Else
                                cr = cr + ldgr.TransactionAmount
                            End If
                        End If

                    Next

                    acc_obj.OutstandingAmount = dr - cr
                    If Not _StudentDAL.InsStudOutstanding(acc_obj) = False Then
                        res = True
                    End If

                    'updated by Hafiz @ 24/2/2016
                    'if out_amt = 0, insert/update into table - start
                    If ledger.Count = 0 Then
                        acc_obj.OutstandingAmount = String.Format("{0:F}", 0)
                        If Not _StudentDAL.InsStudOutstanding(acc_obj) = False Then
                            res = True
                        End If

                    End If
                    'if out_amt = 0, insert/update into table - end

                    'added by Hafiz @ 31/3/2016
                    'update flag - start
                    If Not _StudentDAL.UpdateFlag(accobj.CreditRef) = False Then
                    End If
                    'update flag - end
                Next

            End If

        Catch ex As Exception

            LogError.Log("Error", "btnFind_Click", ex.Message)

        End Try

    End Sub

#End Region

#End Region

    Protected Sub ibtnSave_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ibtnSave.Click
        Dim objup As New StudentBAL
        Dim _StudentDAL As New StudentDAL
        Dim lstobjects As New List(Of StudentEn)
        Dim eob As New StudentEn
        Dim eobEN As New StudentEn
        Dim chkReleaseStatus As New CheckBox

        If Not Session("lstOutstandingStudents") Is Nothing Then
            '    lstobjects = Session("lstOutstandingStudents")

            ' Check the Check Box if it already checked

            If ddlProgram.SelectedValue.ToString().Equals("-1") Then
                eobEN.ProgramID = ""
            Else
                eobEN.ProgramID = ddlProgram.SelectedValue.ToString()
            End If


            If ddlSemester.SelectedValue.ToString().Equals("-1") Then
                eobEN.CurretSemesterYear = ""
            Else
                eobEN.CurretSemesterYear = ddlSemester.SelectedValue.ToString()
            End If

            For Each dgItem1 In dgStudent.Items
                chkReleaseStatus = dgItem1.Cells(5).Controls(1)

                If chkReleaseStatus.Checked Then

                    eob = New StudentEn
                    eob.MatricNo = dgItem1.Cells(0).Text.ToString()
                    lstobjects.Add(eob)

                End If
            Next

            Try
                'Modified by Hafiz @ 26/2/2016
                'Update the out_amt when exclude cur sem is checked - START
                If chkExcludeCurSem.Checked Then

                    'assign current outstanding amount
                    For Each dgItem1 In dgStudent.Items

                        chkReleaseStatus = dgItem1.Cells(5).Controls(1)

                        If chkReleaseStatus.Checked Then
                            eobEN.IsReleased = 2
                        Else
                            eobEN.IsReleased = 0
                        End If

                        eobEN.MatricNo = dgItem1.Cells(0).Text.ToString()
                        eobEN.OutstandingAmount = dgItem1.Cells(4).Text.ToString()

                        _StudentDAL.UpdateOutStatInclCurSem(eobEN)

                    Next

                Else

                    objup.UpdatingStudentOutstandingReleaseStatus(eobEN, lstobjects)

                End If
                'Update the out_amt when exclude cur sem is checked - END

                'User Defined Message
                ErrorDescription = "Release Status Updated Successfully "
                lblMesg.Text = ErrorDescription
                lblMesg.Visible = True
            Catch ex As Exception
                lblMesg.Text = ex.Message.ToString()
                lblMesg.Visible = True
            End Try

        Else
            lblMesg.Text = "Record is not available "

            FixEmptyRow(dgStudent, column)
        End If

    End Sub

#Region "UploadData"

    'added by Hafiz @ 04/3/2016
    'methods for Upload File.

    'btn select onclick
    Protected Sub Btnselect_Click(ByVal sender As Object, ByVal e As System.EventArgs)

        If Not Session("fileSponsor") Is Nothing Then
            uploadData()
        Else
            lblMesg.Text = "File Upload Failed."
        End If

    End Sub

    'check uploaded file
    Private Sub CheckUploadedFile()

        Dim temp_path As String = Session("fileSponsor")

        If Not temp_path Is Nothing Then
            lblFileUpload.Visible = True
            Btnselect.Visible = True
            btnUpload.Visible = False

            lblFileUpload.Text = temp_path
        Else
            lblFileUpload.Text = ""

            lblFileUpload.Visible = False
            Btnselect.Visible = False
            btnUpload.Visible = True

        End If

    End Sub

    'upload file methods
    Public Sub uploadData()

        Dim path As String = Session("fileSponsor")
        Dim loReader As New StreamReader(path)

        Dim _AccountDAL As New AccountsDAL
        Dim _StudentBAL As New StudentBAL
        Dim _StudentDAL As New StudentDAL
        Dim list_stud_success As New List(Of StudentEn)
        Dim list_stud_fail As New List(Of StudentEn)
        Dim grid_list As New List(Of StudentEn)
        Dim i As Integer
        Dim LineContent As String = Nothing
        Dim matric_no As String = Nothing
        Dim chkReleaseStatus As CheckBox = Nothing
        Dim releaseStatus As Integer = 0

        lblMesg.Text = ""

        While loReader.Peek <> -1

            LineContent = loReader.ReadLine()

            matric_no = clsGeneric.NullToString(Trim(LineContent))

            'get outstanding details
            Dim eTstudent As New StudentEn

            eTstudent = _AccountDAL.GetStudentOutstanding(matric_no)

            'start the updating
            Try
                If Not eTstudent.MatricNo Is Nothing Then

                    If Not eTstudent.IsReleased = "2" Then
                        If _StudentDAL.UpdateStudentOutstandingStatus(eTstudent) Then
                            'success updated
                            list_stud_success.Add(eTstudent)
                        End If
                    Else
                        'fail updated
                        list_stud_fail.Add(eTstudent)
                    End If

                End If

            Catch ex As Exception

                lblMesg.Text = ex.Message.ToString()

            End Try

            i = i + 1

        End While

        loReader.Close()

        'populate successfull data at the grid
        Try
            'concat success+fail. want to see alll result
            list_stud_success.AddRange(list_stud_fail)

            'updated result
            grid_list = _StudentBAL.GetOutstandingForUploadedFile(list_stud_success)

        Catch ex As Exception

            lblMesg.Text = ex.Message.ToString()

        End Try

        If Not grid_list Is Nothing And grid_list.Count > 0 Then

            dgStudent.DataSource = grid_list
            dgStudent.DataBind()

            For Each dgItem1 In dgStudent.Items

                dgItem1.Cells(0).ForeColor = Drawing.Color.Red

                chkReleaseStatus = dgItem1.Cells(5).Controls(1)
                releaseStatus = CInt(dgItem1.Cells(6).Text)

                'is_released = 2 for the EXEMPTED status
                If releaseStatus.Equals(2) Then
                    chkReleaseStatus.Checked = True
                End If

            Next
        Else
            FixEmptyRow(dgStudent, column)
        End If

        Session("fileSponsor") = Nothing

    End Sub

#End Region


End Class
