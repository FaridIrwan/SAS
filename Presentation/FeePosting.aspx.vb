#Region "NameSpaces "

Imports MaxGeneric
Imports HTS.SAS.Entities
Imports HTS.SAS.BusinessObjects
Imports HTS.SAS.DataAccessObjects
Imports System.Data
Imports System.Data.SqlClient
Imports System.Collections.Generic
Imports System.Globalization
Imports System.Linq

#End Region

Partial Class FeePosting
    Inherits System.Web.UI.Page

#Region "Global Declarations "

    Dim AutoNo As Boolean
    Private ErrorDescription As String
    Dim DFlag As String
    Dim eflag As String
    Dim listafc As New List(Of AFCEn)
    Dim ListObjects As List(Of ProgramInfoEn)

    Dim objIntegrationDL As New SQLPowerQueryManager.PowerQueryManager.IntegrationDL
    Dim objIntegration As New IntegrationModule.IntegrationNameSpace.Integration

    Dim MJ_JnlLineEn As New BusinessEntities.MJ_JnlLine
    Dim dsReturn As New DataSet

#End Region

#Region "Page Load "

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Not IsPostBack() Then
            addBidangCode()
            addSemesterCode()
            addfaculty()
            addsemester()
            Menuname(CInt(Request.QueryString("Menuid")))
            MenuId.Value = GetMenuId()
            lblStatus.Value = "New"
            LoadUserRights()
            Session("ListObj") = Nothing
            Session("listWF") = Nothing
            'Checking Validations
            ibtnDelete.Attributes.Add("onClick", "return getconfirm()")
            ibtnInDate.Attributes.Add("onClick", "return  getDate1from()")
            ibtnDueDate.Attributes.Add("onClick", "return getDate2from()")
            ibtnPosting.Attributes.Add("onClick", "return getpostconfirm()")
            ibtnBDate.Attributes.Add("onClick", "return getibtnBDate()")
            'ibtnSave.Attributes.Add("onClick", "return validate()")
            txtDueDate.Attributes.Add("OnKeyup", "return CheckDueDate()")
            txtInvoiceDate.Attributes.Add("OnKeyup", "return CheckInvDate()")
            txtBatchDate.Attributes.Add("OnKeyup", "return CheckBatchDate()")
            'rbSemester.Attributes.Add("onClick", "return getControlFields()")
            'Clearing Sessions
            Session("listprog") = Nothing
            Session("AddFee") = Nothing
            Session("LstStueObj") = Nothing
            'Disable Navigation
            DisableRecordNavigator()
            OnLoadItem()
            'Formatting Date
            dates()
            getControlObject()
            LoadProgram()
            'addProgramInfo()
            addDesc()
            'ibtnSave.Attributes.Add("onclick", "return do_totals1();")
            'ibtnPrint.Attributes.Add("onclick", "return do_totals1();") 
            'ibtnPosting.Attributes.Add("onclick", "return do_totals1();") commented by Hafiz @ 7/10/2016 - unused function maybe?
            'ibtnPosting.Attributes.Add("onclick", "new_window=window.open('AddApprover.aspx?MenuId=" & GetMenuId() & "','Hanodale','width=500,height=400,resizable=0');new_window.focus();")
        End If

        If Not Request.QueryString("BatchCode") Is Nothing Then
            Dim str As String = Request.QueryString("BatchCode")
            Dim constr As String() = str.Split(";")
            txtBatchNo.Text = constr(0)

            DirectCast(Master.FindControl("Panel1"), System.Web.UI.WebControls.Panel).Visible = False
            DirectCast(Master.FindControl("td"), System.Web.UI.HtmlControls.HtmlTableCell).Visible = False
            Panel1.Visible = False

            GetData(ddlFaculty.SelectedValue, ddlSemester.SelectedValue, "", txtBatchNo.Text, ddlCurrSem.SelectedValue)

            OnSearchOthers(txtBatchNo.Text)
        End If

        If Not Session("CheckApproverList") Is Nothing Then
            SendToApproval()
        End If

        If Session("User") Is Nothing Then
            Response.Redirect("~/login.aspx")
        End If
    End Sub

#End Region

    Protected Sub ddlFaculty_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlFaculty.SelectedIndexChanged
        GetData(ddlFaculty.SelectedValue, ddlProgramInfo.SelectedValue, ddlSemester.SelectedValue, "", ddlCurrSem.SelectedValue)
        LoadProgram()
    End Sub

    Protected Sub ibtnSave_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ibtnSave.Click
        Session("Batch") = ""
        If SpaceValidation() Then
            onSave()
        End If
        setDateFormat()
    End Sub

    'Protected Sub ibtnPosting_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ibtnPosting.Click
    '    If SpaceValidation() Then
    '        OnPost()
    '    End If
    '    setDateFormat()
    'End Sub

    Protected Sub SendToApproval()

        Try
            If Not Session("listWF") Is Nothing Then

                Dim list As List(Of WorkflowSetupEn) = Session("listWF")
                If list.Count > 0 Then
                    lblMsg.Text = ""

                    If SpaceValidation() Then

                        setDateFormat()

                        If OnPost() = True Then
                            If Session("listWF").count > 0 Then
                                WorkflowApproverList(Trim(txtBatchNo.Text), Session("listWF"))
                            End If
                        End If
                    End If
                Else
                    Throw New Exception("Posting to workflow failed caused NO approver selected.")
                End If

            End If

        Catch ex As Exception
            lblMsg.Text = ex.Message.ToString()
        Finally
            Session("listWF") = Nothing
            Session("CheckApproverList") = Nothing
        End Try

    End Sub

    Private Sub LoadReport(ByVal batch As String)

        'Temporary Comment
        hvbatch.Value = ConstructBatchNumber()
        If hvbatch.Value = "|" Then
            lblMsg.Text = "Select at least one record to print the report "
            Exit Sub
        End If
        If String.IsNullOrEmpty(hvbatch.Value) Then
            lblMsg.Text = "Select at least one record to print the report "

        Else
            Dim scriptstringOpen As String = "OpenWindow();"
            ClientScript.RegisterStartupScript(Me.GetType(), "OpenWindow", scriptstringOpen, True)
        End If

    End Sub

    Protected Sub ibtnNew_Click1(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ibtnNew.Click
        onAdd()
    End Sub

    Protected Sub ibtnDelete_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)

    End Sub

#Region "Methods"
    'modified by Hafiz @ 07/6/2016 - include logic for ddlCurrSem.SelectedValue

    'Private Sub GetData(ByVal strValue As String, Optional ByVal strProgram As String = "", Optional ByVal strSemester As String = "")
    Private Sub GetData(ByVal strValue As String, Optional ByVal strSemester As String = "", Optional ByVal strBidang As String = "",
                        Optional ByVal strBatchCode As String = "", Optional ByVal strCurrSem As String = "")
        Dim Obj As New ProgramInfoBAL
        Dim list As New List(Of ProgramInfoEn)
        Dim eob As New ProgramInfoEn
        Dim AFCBAL As New AFCBAL
        Dim _CheckBox As CheckBox = Nothing

        Session("listprog") = Nothing
        If strValue = "-1" Or strValue = "1" Then
            eob.Faculty = ""
        Else
            eob.Faculty = strValue
        End If

        'If strProgram = "-1" Then
        '    eob.ProgramCode = ""
        'Else
        '    eob.ProgramCode = strProgram
        'End If

        If strSemester = "-1" Then
            eob.Semester = ""
        Else
            eob.Semester = strSemester
        End If

        If strBidang = "-1" Then
            eob.BidangCode = ""
        Else
            eob.BidangCode = ddlBidang.SelectedValue
        End If

        If strCurrSem = "-1" Then
            eob.CurrentSemester = ""
        Else
            eob.CurrentSemester = ddlCurrSem.SelectedValue
        End If

        If strBatchCode = "-1" Then
            eob.BatchNo = ""
        Else
            eob.BatchNo = strBatchCode
            txtBatchNo.Text = strBatchCode
        End If

        'eob.TransStatus = ddlstatus.SelectedValue
        If Request.QueryString("Formid") = "FS" Then
            eob.Faculty = ""
            eob.Semester = ""
            eob.BidangCode = ""
            eob.CurrentSemester = ""
            eob.TransStatus = "1"
        Else
            eob.TransStatus = ddlstatus.SelectedValue
        End If

        list = Obj.GetAfcPrograms(eob)
        dgView.DataSource = Nothing
        dgView.DataBind()

        Try
            dgView.DataSource = list
            dgView.DataBind()
        Catch ex As HttpException
            dgView.PageIndex = 0
            dgView.DataBind()
            dgView.PageIndex = 1
        End Try

        'dgView.Columns(6).Visible = True
        'Added by Solomon to bind the programs based on the faculty.
        Dim dgItem1 As GridViewRow
        Dim link As New HyperLink
        Dim i As Integer = 0
        Dim Status As String = String.Empty

        For Each dgItem1 In dgView.Rows

            'Added Mona 28/3/16
            'modified by Hafiz 20/10/16
            'Batchcode-Visible
            Dim ChkBoxHeader As CheckBox = DirectCast(dgView.HeaderRow.FindControl("chkboxSelectAll"), CheckBox)
            _CheckBox = dgItem1.Cells(0).Controls(1)

            dgView.Columns(4).Visible = False
            dgView.Columns(6).Visible = False
            dgView.Columns(7).Visible = False
            dgView.Columns(9).Visible = False
            dgView.Columns(10).Visible = False

            If ddlstatus.SelectedValue = "1" Or ddlstatus.SelectedValue = "2" Then
                dgView.Columns(6).Visible = True
                ChkBoxHeader.Enabled = False
            End If

            If ddlstatus.SelectedValue <> "-1" Then
                'bind data - start
                dgItem1.Cells(4).Text = list.Where(Function(x) x.ProgramID = dgItem1.Cells(1).Text AndAlso x.Semester = dgItem1.Cells(5).Text).Select(Function(y) y.Faculty).FirstOrDefault()
                dgItem1.Cells(6).Text = list.Where(Function(x) x.ProgramID = dgItem1.Cells(1).Text AndAlso x.Semester = dgItem1.Cells(5).Text).Select(Function(y) y.BatchNo).FirstOrDefault()
                dgItem1.Cells(7).Text = list.Where(Function(x) x.ProgramID = dgItem1.Cells(1).Text AndAlso x.Semester = dgItem1.Cells(5).Text AndAlso x.BatchNo =
                                                       dgItem1.Cells(6).Text).Select(Function(y) y.TransStatus).FirstOrDefault()
                dgItem1.Cells(9).Text = list.Where(Function(x) x.ProgramID = dgItem1.Cells(1).Text AndAlso x.Semester = dgItem1.Cells(5).Text AndAlso x.BatchNo =
                                                       dgItem1.Cells(6).Text).Select(Function(y) y.BidangCode).FirstOrDefault()
                dgItem1.Cells(10).Text = list.Where(Function(x) x.ProgramID = dgItem1.Cells(1).Text AndAlso x.Semester = dgItem1.Cells(5).Text AndAlso x.BatchNo =
                                                       dgItem1.Cells(6).Text).Select(Function(y) y.CurrentSemester).FirstOrDefault()
                'bind data - end
            End If

            'Status = AFCBAL.IsPosted(dgItem1.Cells(4).Text, dgItem1.Cells(5).Text, dgItem1.Cells(1).Text)
            link = dgItem1.Cells(8).Controls(1)
            link.Attributes.Add("onClick", "OpenWindow1('about:blank')")
            link.NavigateUrl = "FeeStructure.aspx?Formid=FS&IsFeePosting=1&ProgramId=" & dgItem1.Cells(1).Text & "&Semester=" & dgItem1.Cells(5).Text & "&PostStatus=" _
                            & ddlstatus.SelectedValue & "&BatchCode=" & dgItem1.Cells(6).Text & "&Faculty=" & dgItem1.Cells(4).Text & "&BidangCode=" & dgItem1.Cells(9).Text _
                            & "&CurrSem=" & dgItem1.Cells(10).Text
            link.Target = "MyPopup"
            i += 1
            'If Status = "true" Then
            '    dgItem1.Visible = False
            'End If

            If Not String.IsNullOrEmpty(strBatchCode) Then
                If (dgItem1.Cells(6).Text) = strBatchCode Then
                    _CheckBox.Checked = True
                End If
            End If

        Next

        Session("listprog") = list

        txtBatchNo.Visible = True
        Label1.Visible = True
        txtBatchNo.ReadOnly = False

        If ddlstatus.SelectedValue = "-1" Or Not String.IsNullOrEmpty(txtBatchNo.Text) Then
            If list.Count > 0 Then
                If (Not String.IsNullOrEmpty(list(0).TransStatus)) Then
                    If list(0).TransStatus = "Posted" Then
                        ddlstatus.SelectedValue = 2
                    Else
                        ddlstatus.SelectedValue = 1
                        txtBatchNo.ReadOnly = True
                    End If
                End If

            End If
        End If

        If ddlstatus.SelectedValue = "2" Then
            ibtnSave.Enabled = False
            ibtnSave.ImageUrl = "images/gsave.png"
            ibtnSave.ToolTip = "Access Denied"

            ibtnNew.Enabled = False
            ibtnNew.ImageUrl = "images/gadd.png"
            ibtnNew.ToolTip = "Access Denied"

            ibtnDelete.Enabled = False
            ibtnDelete.ImageUrl = "images/gdelete.png"
            ibtnDelete.ToolTip = "Access Denied"

            ibtnPosting.Enabled = False
            ibtnPosting.ImageUrl = "images/gposting.png"
            ibtnPosting.ToolTip = "Access Denied"

            ibtnStatus.ImageUrl = "images/posted.gif"

            'Added Mona 30/3/2016
            ibtnPrint.Enabled = True
            ibtnPrint.ImageUrl = "images/print.png"
            ibtnPrint.ToolTip = "Print"

        Else
            ibtnSave.Enabled = True
            ibtnSave.ImageUrl = "images/save.png"
            ibtnSave.ToolTip = "Save"

            ibtnNew.Enabled = True
            ibtnNew.ImageUrl = "images/add.png"
            ibtnNew.ToolTip = "New"

            ibtnDelete.Enabled = True
            ibtnDelete.ImageUrl = "images/delete.png"
            ibtnDelete.ToolTip = "Delete"

            'ibtnPosting.Enabled = True
            'ibtnPosting.ImageUrl = "images/posting.png"
            'ibtnPosting.ToolTip = "Access Denied"

            If ddlstatus.SelectedValue = 0 Or ddlstatus.SelectedValue = -1 Then
                ibtnStatus.ImageUrl = "images/NotReady.gif"
                lblStatus.Value = "Held"

                ibtnPosting.Enabled = False
                ibtnPosting.ImageUrl = "images/gposting.png"
                ibtnPosting.ToolTip = "Access Denied"

                'txtBatchNo.Visible = False
                'Label1.Visible = False

                'Added Mona 28/2/2016
                ibtnPrint.Enabled = False
                ibtnPrint.ImageUrl = "images/gprint.png"
                ibtnPrint.ToolTip = "Access Denied"
            Else
                'ibtnStatus.ImageUrl = "images/ready.gif"
                'ibtnSave.Enabled = True
                'ibtnSave.ImageUrl = "images/gsave.png"
                'ibtnSave.ToolTip = "Access Denied"

                If ddlstatus.SelectedValue = 1 Then
                    lblStatus.Value = "Ready"
                    ibtnStatus.ImageUrl = "images/Ready.gif"
                End If

                ibtnPosting.Enabled = True
                ibtnPosting.ImageUrl = "images/posting.png"

                'Added Mona 28/2/2016
                ibtnPrint.Enabled = True
                ibtnPrint.ImageUrl = "images/print.png"
                ibtnPrint.ToolTip = "Print"

                ibtnSave.Enabled = False
                ibtnSave.ImageUrl = "images/gsave.png"
                ibtnSave.ToolTip = "Access Denied"
            End If
        End If

    End Sub

#Region "On Save "

    ''' <summary>
    ''' Method to Save and Update Invoices
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub onSave()

        'Create Instances - Start
        Dim _AFCEn As AFCEn
        Dim _AFCEn_1 As AFCEn
        Dim _AFCDetailsEn As AFCDetailsEn
        Dim _ProgramInfoBAL As New ProgramInfoBAL
        Dim ListStudentEn As New List(Of StudentEn)
        Dim ListProgramInfoEn As New List(Of ProgramInfoEn)
        'Create Instances - Stop

        'Variable Declarations - Start
        Dim MatricNo As String = Nothing, CheckBatchNumber As String = Nothing
        Dim _CheckBox As CheckBox = Nothing, FeeFor As String = Nothing, Index As Integer = 0
        Dim Status As String = Nothing, Batch As String = Nothing, StudentMatricNos As String = Nothing
        'Variable Declarations - Stop

        Try

            If ddlstatus.SelectedValue = 1 Then
                lblMsg.Text = "Record Already Saved."

                If Not Session("listprog") Is Nothing Then
                    'Set Status
                    ddlstatus.SelectedValue = 1

                    'Get Data - Start                    
                    GetData(ddlFaculty.SelectedValue, ddlSemester.SelectedValue, ddlBidang.SelectedValue, "", ddlCurrSem.SelectedValue)
                    'Get Data - Stop
                End If

                Exit Sub

            End If

            'check program list session available - Start
            If Not Session("listprog") Is Nothing Then

                'get program list from session
                ListProgramInfoEn = Session("listprog")

                'Check if no records selected - Start
                If ListProgramInfoEn.Count = 0 Then
                    lblMsg.Text = "No Programs to Select"
                    lblMsg.Visible = True
                    Exit Sub
                End If
                'Check if no records selected - Stop

            Else
                lblMsg.Text = "No AFC Record Is Available"
            End If
            'check program list session available - Stop

            Dim _listAFC As New List(Of AFCEn)
            Dim list As List(Of Integer) = DirectCast(ViewState("SelectedRecords"), List(Of Integer))

            If list.Count > 0 Then

                For Each idx As Integer In list

                    If rbSemester.Checked = True Then
                        FeeFor = "Semester"
                    Else
                        FeeFor = "Annual"
                    End If

                    _AFCEn = New AFCEn()

                    _AFCEn.SAFC_Code = ListProgramInfoEn(idx).Faculty

                    If Not String.IsNullOrEmpty(ListProgramInfoEn(idx).Semester) Then
                        _AFCEn.Semester = ListProgramInfoEn(idx).Semester
                    Else
                        Throw New Exception("Save Failed - Semester Is Empty.")
                    End If

                    _AFCEn.CurrentSemester = ListProgramInfoEn(idx).CurrentSemester
                    _AFCEn.DueDate = Trim(txtDueDate.Text)
                    _AFCEn.TransDate = Trim(txtInvoiceDate.Text)
                    _AFCEn.Bdate = Trim(txtBatchDate.Text)
                    _AFCEn.PostedFor = FeeFor
                    _AFCEn.Reference = "Ready"
                    _AFCEn.Description = "AFC-" & _AFCEn.CurrentSemester
                    _AFCEn.Updatetime = DateTime.Now
                    _AFCEn.Updatedby = Session("User")

                    'afcdetails - start
                    _AFCDetailsEn = New AFCDetailsEn
                    Dim ListAFCDetailsEn As New List(Of AFCDetailsEn)
                    'ListAFCDetailsEn.Clear()
                    _AFCDetailsEn.ProgramID = ListProgramInfoEn(idx).ProgramID
                    _AFCDetailsEn.Faculty = ListProgramInfoEn(idx).Faculty
                    _AFCDetailsEn.BidangCode = ListProgramInfoEn(idx).BidangCode
                    _AFCEn.PostStatus = ddlstatus.SelectedValue
                    ListAFCDetailsEn.Add(_AFCDetailsEn)
                    _AFCDetailsEn = Nothing
                    'afcdetails - end

                    _AFCEn.AFCDetailslist = ListAFCDetailsEn
                    If (ListAFCDetailsEn.Count = 0) Then
                        lblMsg.Text = "Select At Least One Program "
                        Exit Sub
                        _AFCEn.AFCDetailslist = New List(Of AFCDetailsEn)
                    End If

                    _AFCEn.SASI_Name = ListProgramInfoEn(idx).ProgramID

                    'get the student matric no for the given program details
                    ListStudentEn = _ProgramInfoBAL.GetProgramInfoListAllMatricNo(_AFCEn)

                    'Loop thro the Student List - Start
                    For Index = 0 To ListStudentEn.Count - 1

                        'Get Matric No
                        MatricNo = ListStudentEn(Index).MatricNo

                        If Index = 0 Then
                            StudentMatricNos = clsGeneric.AddQuotes(MatricNo)
                        Else
                            StudentMatricNos &= clsGeneric.AddComma() & clsGeneric.AddQuotes(MatricNo)
                        End If

                    Next
                    'Loop thro the Student List - Stop

                    _AFCEn.CreditRef = StudentMatricNos
                    StudentMatricNos = String.Empty
                    lblMsg.Visible = True
                    Status = "Ready"
                    Batch = ""

                    _AFCEn_1 = New AFCBAL().CheckAFC(_AFCEn)
                    MatricNo = New AFCBAL().CheckNewStudentAFC(_AFCEn)

                    'if batch code available - Start
                    If Not FormHelp.IsBlank(_AFCEn_1.BatchCode) Then

                        If (_AFCEn_1.BatchCode = "NOFEESTRUCTURE") Then
                            If String.IsNullOrEmpty(ListProgramInfoEn(idx).BidangCode) Then
                                ErrorDescription = "No Bidang Code for program " & _AFCEn_1.Reference
                            Else
                                ErrorDescription = "No Fee structure for program " & _AFCEn_1.Reference
                            End If

                            lblMsg.Text = ErrorDescription
                            lblMsg.Visible = True
                            Exit Sub

                        ElseIf (_AFCEn_1.BatchCode = "NOHOSTELFEE") Then
                            ErrorDescription = "No Hostel Fee For Student in program " & _AFCEn_1.Reference
                            lblMsg.Text = ErrorDescription
                            lblMsg.Visible = True
                            Exit Sub

                        Else
                            'modified by Hafiz @ 21/10/2016
                            If Not FormHelp.IsBlank(MatricNo) Then
                                _listAFC.Add(_AFCEn)
                            Else
                                lblMsg.Text = ErrorDescription
                                lblMsg.Visible = True
                                Exit Sub
                            End If

                        End If

                    Else
                        'modified by Hafiz @ 21/10/2016
                        _listAFC.Add(_AFCEn)

                    End If
                    'if batch code available - Stop

                Next

            Else
                Throw New Exception("Select at least one data to save.")
            End If

            If _listAFC.Count > 0 Then

                If InsertAFC(_listAFC, CheckBatchNumber) Then

                    ddlstatus.SelectedValue = 1
                    GetData(ddlFaculty.SelectedValue, ddlProgramInfo.SelectedValue, ddlSemester.SelectedValue, CheckBatchNumber, ddlCurrSem.SelectedValue)

                End If

            End If

        Catch ex As Exception

            'Log Error
            lblMsg.Text = ex.Message
            Call MaxModule.Helper.LogError(ex.Message)

        End Try

    End Sub

#End Region

#Region "InsertAFC"

    Protected Function InsertAFC(ByVal _listAFC As List(Of AFCEn), ByRef CheckBatchNumber As String) As Boolean

        Dim Batch As String = ""

        For Each _AFCEn As AFCEn In _listAFC
            Batch = Session("Batch")
            CheckBatchNumber = New AFCBAL().AFCNEW(_AFCEn, "Ready", Batch)
            Session("Batch") = CheckBatchNumber
        Next

        If CheckBatchNumber <> "Error in Batch Number" Then
            txtBatchNo.Text = CheckBatchNumber
            txtBatchNo.ReadOnly = True
            ErrorDescription = "Record Saved Successfully "
            ibtnStatus.ImageUrl = "images/ready.gif"
            lblStatus.Value = "Ready"
            lblMsg.Text = ErrorDescription
            ibtnSave.Enabled = False

            Return True
        Else
            lblMsg.Text = CheckBatchNumber
            lblMsg.Visible = True

            Return False
        End If

    End Function

#End Region

#Region "Onpost "

    ''' <summary>
    ''' Method to Post AFC'S
    ''' </summary>
    ''' modified by Hafiz @ 03/8/2016

    Protected Function OnPost() As Boolean

        Dim result As Boolean = False
        Dim _WorkflowDAL As New HTS.SAS.DataAccessObjects.WorkflowDAL
        Dim StrBatchId As String = String.Empty, _lstBatchId As New List(Of String)

        Try
            For Each _dgItem As GridViewRow In dgView.Rows

                Dim _BatchId As String = _dgItem.Cells(6).Text
                Dim _CheckBox As CheckBox = _dgItem.Cells(0).Controls(1)

                If _CheckBox.Checked Then
                    _lstBatchId.Add(_BatchId)
                Else
                    Throw New Exception("Please Select Program")
                    Return False
                End If

            Next

            If _lstBatchId.Where(Function(x) x.ToString() = txtBatchNo.Text).Count > 0 Then

                StrBatchId = txtBatchNo.Text

                If _WorkflowDAL.Workflow(StrBatchId, Session(Helper.UserSession), Me.ToString()) Then
                    lblMsg.Visible = True
                    result = True
                    lblMsg.Text = "Record Posted Successfully for Approval."
                    lblStatus.Value = "Ready"
                    ibtnStatus.ImageUrl = "images/ready.gif"
                    ddlstatus.SelectedValue = 1
                Else
                    lblMsg.Visible = True
                    result = False
                    lblMsg.Text = "Record Already Posted."
                    ddlstatus.SelectedValue = 1
                    StrBatchId = ""
                End If

                GetData(ddlFaculty.SelectedValue, ddlProgramInfo.SelectedValue, ddlSemester.SelectedValue, StrBatchId, ddlCurrSem.SelectedValue)

            Else
                Throw New Exception("Please Select Program")
                Return False
            End If

        Catch ex As Exception

            lblMsg.Text = ex.Message.ToString()
            Call MaxModule.Helper.LogError(ex.Message)

        End Try

        Return result

    End Function

#End Region

#Region "WorkflowApproverList"

    Protected Sub WorkflowApproverList(ByVal batchcode As String, ByVal lisWF As List(Of WorkflowSetupEn))

        Dim result As Boolean = False

        Try
            For Each wfEn As WorkflowSetupEn In lisWF

                result = New WorkflowDAL().InsertWFApprovalList(batchcode, wfEn.MenuMasterEn.PageName,
                                                                wfEn.UsersEn.UserName, wfEn.UserGroupsEn.UserGroupName)

            Next
        Catch ex As Exception
            lblMsg.Text = ex.Message.ToString()
            LogError.Log("FeePosting", "InsertWFApprovalList", ex.Message)
        End Try

    End Sub

#End Region

    ''' <summary>
    ''' Method to Control All Fields
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub getControlObject()
        If Not IsPostBack Then
            rbSemester.Checked = False
            rbAnnual.Checked = False
        End If
    End Sub
    ''' <summary>
    ''' Method to Format the Date Feilds
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub dates()
        txtBatchDate.Text = Format(Date.Now, "dd/MM/yyyy")
        txtInvoiceDate.Text = Format(Date.Now, "dd/MM/yyyy")
        txtDueDate.Text = Format(DateAdd(DateInterval.Day, 30, Date.Now), "dd/MM/yyyy")
    End Sub
    ''' <summary>
    ''' Method to Validate Data Fields before Save
    ''' </summary>
    ''' <remarks></remarks>
    Private Function SpaceValidation() As Boolean

        ibtnSave.Attributes.Add("onCLick", "return validate()")
        Dim GBFormat As System.Globalization.CultureInfo
        GBFormat = New System.Globalization.CultureInfo("en-GB")

        'Batch date
        If Trim(txtBatchDate.Text).Length < 10 Then
            lblMsg.Text = "Enter Valid Batch Date"
            lblMsg.Visible = True
            txtBatchDate.Focus()
            Return False
        Else
            Try
                txtBatchDate.Text = DateTime.Parse(txtBatchDate.Text, GBFormat)
            Catch ex As Exception
                lblMsg.Text = "Enter Valid Batch Date"
                lblMsg.Visible = True
                txtBatchDate.Focus()
                Return False
            End Try
        End If
        'If (rbSemester.Checked) Then
        '    If ddlSemester.SelectedIndex = 0 Then
        '        lblMsg.Text = "Please Select Semester"
        '        lblMsg.Visible = True
        '        ddlSemester.Focus()
        '        Return False
        '    End If
        'End If

        'Invoice date
        If Trim(txtInvoiceDate.Text).Length < 10 Then
            lblMsg.Text = "Enter Valid Invoice Date"
            lblMsg.Visible = True
            txtInvoiceDate.Focus()
            Return False
        Else
            Try
                txtInvoiceDate.Text = DateTime.Parse(txtInvoiceDate.Text, GBFormat)
            Catch ex As Exception
                lblMsg.Text = "Enter Valid Invoice Date"
                lblMsg.Visible = True
                txtInvoiceDate.Focus()
                Return False
            End Try
        End If

        'Due date
        If Trim(txtDueDate.Text).Length < 10 Then
            lblMsg.Text = "Enter Valid Due Date"
            lblMsg.Visible = True
            txtInvoiceDate.Focus()
            Return False
        Else
            Try
                txtDueDate.Text = DateTime.Parse(txtDueDate.Text, GBFormat)
            Catch ex As Exception
                lblMsg.Text = "Enter Valid Due Date"
                lblMsg.Visible = True
                txtInvoiceDate.Focus()
                Return False
            End Try
        End If

        If lblStatus.Value = "Posted" Then
            lblMsg.Text = "Record Already Posted"
            lblMsg.Visible = True
            Return False
        End If

        Return True

    End Function
    ''' <summary>
    ''' Method to Format Date After Search
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub setDateFormat()
        'Dim GBFormat As System.Globalization.CultureInfo
        'GBFormat = New System.Globalization.CultureInfo("en-GB")
        'txtBatchDate.Text = Format(DateTime.Parse(txtBatchDate.Text, GBFormat), "dd/MM/yyyy")
        'txtInvoiceDate.Text = Format(DateTime.Parse(txtInvoiceDate.Text, GBFormat), "dd/MM/yyyy")
        'txtDuedate.Text = Format(DateTime.Parse(txtDuedate.Text, GBFormat), "dd/MM/yyyy")
        'Dim myInvoiceDate As Date = CDate(CStr(txtInvoiceDate.Text))
        'Dim myFormat As String = "dd/MM/yyyy"
        'Dim myFormattedDate As String = Format(myInvoiceDate, myFormat)
        'txtInvoiceDate.Text = myFormattedDate
        'Dim myDuedate As Date = CDate(CStr(txtDueDate.Text))
        'Dim myFormattedDate1 As String = Format(myDuedate, myFormat)
        'txtDueDate.Text = myFormattedDate1
        'Dim myBatchDate As Date = CDate(CStr(txtBatchDate.Text))
        'Dim myFormattedDate2 As String = Format(myBatchDate, myFormat)
        'txtBatchDate.Text = myFormattedDate2

        Dim myInvoiceDate As Date = CDate(CStr(txtInvoiceDate.Text))
        Dim myFormat As String = "dd/MM/yyyy"
        Dim myFormattedDate As String = Format(myInvoiceDate, myFormat)
        txtInvoiceDate.Text = myFormattedDate
        Dim myDuedate As Date = CDate(CStr(txtDueDate.Text))
        Dim myFormattedDate1 As String = Format(myDuedate, myFormat)
        txtDueDate.Text = myFormattedDate1
        Dim myBatchDate As Date = CDate(CStr(txtBatchDate.Text))
        Dim myFormattedDate2 As String = Format(myBatchDate, myFormat)
        txtBatchDate.Text = myFormattedDate2

    End Sub
    ''' <summary>
    ''' Method to Fill the Faculty DropDown
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub addfaculty()
        Dim eFaculty As New FacultyEn
        Dim bFaculty As New FacultyBAL
        Dim listfac As New List(Of FacultyEn)
        ddlFaculty.Items.Clear()
        ddlFaculty.Items.Add(New ListItem("---Select---", "-1"))
        ddlFaculty.Items.Add(New ListItem("All", "1"))
        ddlFaculty.DataTextField = "SAFC_Desc"
        ddlFaculty.DataValueField = "SAFC_Code"
        eFaculty.SAFC_Code = "%"

        Try
            listfac = bFaculty.GetList(eFaculty)
        Catch ex As Exception
            LogError.Log("FeePosting", "addfaculty", ex.Message)
        End Try
        ddlFaculty.DataSource = listfac
        ddlFaculty.DataBind()
        Session("faculty") = listfac
    End Sub
    ''' <summary>
    ''' Method to Fill the Description DropDown
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub addProgramInfo()
        'Dim eProgram As New ProgramAccountEn
        'Dim bProgram As New ProgramAccountBAL
        'Dim listProgram As New List(Of ProgramAccountEn)
        'ddlProgramInfo.Items.Clear()
        'ddlProgramInfo.Items.Add(New ListItem("---Select---", "-1"))
        ''ddlProgramInfo.Items.Add(New ListItem("All", "1"))
        'ddlProgramInfo.DataTextField = "CodeProgram"
        'ddlProgramInfo.DataValueField = "ProgramCode"
        'eProgram.ProgramCode = "%"

        'Try
        '    listProgram = bProgram.GetListProgram(eProgram)
        'Catch ex As Exception
        '    LogError.Log("FeePosting", "addProgramInfo", ex.Message)
        'End Try
        'ddlProgramInfo.DataSource = listProgram
        'ddlProgramInfo.DataBind()
        'Session("faculty") = listfac

        Dim eobjF As New ProgramInfoEn
        Dim bsobj As New ProgramInfoBAL
        Dim list As New List(Of ProgramInfoEn)
        eobjF.ProgramType = ""
        eobjF.ProgramCode = ""
        eobjF.Program = ""
        eobjF.ProgramBM = ""
        eobjF.Status = True
        eobjF.Faculty = ""
        ddlProgramInfo.Items.Clear()
        ddlProgramInfo.Items.Add(New ListItem("---Select---", "-1"))
        ddlProgramInfo.DataTextField = "CodeProgram"
        ddlProgramInfo.DataValueField = "ProgramCode"
        Try
            list = bsobj.GetProgramInfoList(eobjF)
        Catch ex As Exception
            LogError.Log("FeePosting", "addProgramInfo", ex.Message)
        End Try

        ddlProgramInfo.DataSource = list
        ddlProgramInfo.DataBind()
    End Sub
    Private Sub LoadProgram()
        Dim objBAL As New ProgramInfoBAL
        Dim listObj As New List(Of ProgramInfoEn)
        Try
            If ddlFaculty.SelectedValue <> "-1" Then
                listObj = objBAL.GetProgramList(ddlFaculty.SelectedValue, True)
                ddlProgramInfo.Items.Clear()
                ddlProgramInfo.Items.Add(New ListItem("--Select--", "-1"))
                ddlProgramInfo.DataTextField = "Program"
                ddlProgramInfo.DataValueField = "ProgramCode"
                ddlProgramInfo.DataSource = listObj
                ddlProgramInfo.DataBind()
            Else
                addProgramInfo()
            End If
        Catch ex As Exception
            LogError.Log("Student", "LoadProgram", ex.Message)
        End Try
    End Sub
    ''' <summary>
    ''' Method to Fill the Program DropDown
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub addDesc()
        Dim eDesc As New ProgramAccountEn
        Dim bDesc As New ProgramAccountBAL
        Dim listDesc As New List(Of ProgramAccountEn)
        'ddlDesc.Items.Clear()
        'ddlDesc.Items.Add(New ListItem("---Select---", "-1"))
        ''ddlDescInfo.Items.Add(New ListItem("All", "1"))
        'ddlDesc.DataTextField = "descProgram"
        'ddlDesc.DataValueField = "descProgram"
        eDesc.descProgram = "%"

        Try
            listDesc = bDesc.GetListProgramCombine(eDesc)
        Catch ex As Exception
            LogError.Log("FeePosting", "addDesc", ex.Message)
        End Try
        'ddlDesc.DataSource = listDesc
        'ddlDesc.DataBind()
        'Session("faculty") = listfac
    End Sub
    ''' <summary>
    ''' Method to Fill the Semester DropDown
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub addsemester()
        Dim eSemester As New SemesterSetupEn
        Dim bSemester As New SemesterSetupBAL
        Dim listSem As New List(Of SemesterSetupEn)
        'ddlSemester.Items.Clear()
        ddlSemester.Items.Add(New ListItem("---Select---", "-1"))
        'ddlFaculty.Items.Add(New ListItem("All", "1"))
        ddlSemester.DataTextField = "SemisterSetupCode"
        ddlSemester.DataValueField = "SemisterSetupCode"
        eSemester.SemisterSetupCode = "%"

        Try
            'listSem = bSemester.GetList(eSemester)
            listSem = bSemester.GetListSemesterCode(eSemester)
        Catch ex As Exception
            LogError.Log("FeePosting", "addsemester", ex.Message)
        End Try
        ddlSemester.DataSource = listSem
        ddlSemester.DataBind()
        'Session("faculty") = listfac
    End Sub
    Private Sub OnLoadItem()
        'txtBatchNo.Text = "Auto Number"
        'txtBatchNo.ReadOnly = True
        Session("ReceiptFor") = Nothing
        lblMsg.Text = ""
        txtBatchDate.Text = Format(Date.Now, "dd/MM/yyyy")
        txtInvoiceDate.Text = Format(Date.Now, "dd/MM/yyyy")
        txtDueDate.Text = Format(DateAdd(DateInterval.Day, 30, Date.Now), "dd/MM/yyyy")
        txtBatchDate.ReadOnly = True
        txtInvoiceDate.ReadOnly = True
        txtDueDate.ReadOnly = True
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
            LogError.Log("FeePosting", "Menuname", ex.Message)
        End Try
        lblMenuName.Text = eobj.MenuName
    End Sub
    ''' <summary>
    ''' Method to Load the UserRights
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub LoadUserRights()
        Dim obj As New UsersBAL
        Dim eobj As New UserRightsEn

        Try
            eobj = obj.GetUserRights(CInt(Request.QueryString("Menuid")), CInt(Session("UserGroup")))
        Catch ex As Exception
            LogError.Log("FeePosting", "LoadUserRights", ex.Message)
        End Try
        'Rights for Add

        If eobj.IsAdd = True Then
            ibtnSave.Enabled = True
            'onAdd()
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
        Else
            ibtnSave.Enabled = True
            ibtnSave.ImageUrl = "images/gsave.png"
            ibtnSave.ToolTip = "Access Denied"
        End If
        'Rights for View
        'ibtnView.Enabled = eobj.IsView
        If eobj.IsView = True Then
            'ibtnView.ImageUrl = "images/ready.png"
            'ibtnView.Enabled = True
        Else
            'ibtnView.ImageUrl = "images/ready.png"
            'ibtnView.ToolTip = "Access Denied"
            'ibtnView.Enabled = True
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
        'ibtnPrint.Enabled = eobj.IsPrint
        'If eobj.IsPrint = True Then
        '    ibtnPrint.Enabled = True
        '    ibtnPrint.ImageUrl = "images/print.png"
        '    ibtnPrint.ToolTip = "Print"
        'Else
        '    ibtnPrint.Enabled = False
        '    ibtnPrint.ImageUrl = "images/gprint.png"
        '    ibtnPrint.ToolTip = "Access Denied"
        'End If
        If eobj.IsOthers = True Then
            'ibtnOthers.Enabled = True
            'ibtnOthers.ImageUrl = "images/post.png"
            ' ibtnOthers.ToolTip = "Others"
        Else
            'ibtnOthers.Enabled = False
            ''ibtnOthers.ImageUrl = "images/post.png"
            'ibtnOthers.ToolTip = "Access Denied"
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
    ''' Method to Load the Fields Before Adding New Record
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub onAdd()
        Session("AddFee") = Nothing
        ibtnStatus.ImageUrl = "images/notready.gif"
        lblStatus.Value = "New"
        today.Value = Now.Date
        today.Value = Format(CDate(today.Value), "dd/MM/yyyy")
        Session("listview") = Nothing
        addBidangCode()
        onClearData()
        If ibtnNew.Enabled = False Then
            ibtnSave.Enabled = True
            ibtnSave.ImageUrl = "images/gsave.png"
            ibtnSave.ToolTip = "Access Denied"
        End If
        OnLoadItem()
    End Sub
    ''' <summary>
    ''' Method to get list of Bank Profiles
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub LoadListObjects()
        Dim eob As New AFCEn
        Dim bobj As New AFCBAL

        Dim faculty As String
        Dim feefor As String
        If txtBatchNo.Text <> "Auto Number" Then
            eob.BatchCode = txtBatchNo.Text
        Else
            eob.BatchCode = ""
        End If
        If ddlFaculty.SelectedValue = "-1" Then
            faculty = ""
        Else
            faculty = ddlFaculty.SelectedValue
        End If
        If rbSemester.Checked = True Then
            feefor = "Semester"
        Else
            feefor = "Annual"
        End If
        If Session("loaddata") = "Others" Then
            eob.Reference = "Posted"
        Else
            eob.Reference = "Ready"
        End If
        eob.SAFC_Code = faculty
        eob.PostedFor = feefor

        Try
            listafc = bobj.GetList(eob)
        Catch ex As Exception
            LogError.Log("FeePosting", "LoadListObjects", ex.Message)
        End Try
        Session("ListObj") = listafc
        ' lblCount.Text = listafc.Count.ToString()
        If listafc.Count <> 0 Then
            DisableRecordNavigator()
            'txtRecNo.Text = "1"
            OnMoveFirst()
            ibtnSave.Enabled = True
            ibtnSave.ImageUrl = "images/gsave.png"
        Else
            'txtRecNo.Text = ""
            ' lblCount.Text = ""
            ibtnStatus.ImageUrl = "images/notready.gif"
            lblStatus.Value = "New"
            'txtBatchNo.Text = "Auto Number"
            Session("ListObj") = Nothing
            DisableRecordNavigator()
            ddlFaculty.SelectedValue = "-1"
            'txtDesc.Text = ""
            ' ddlDesc.SelectedIndex = 0
            dates()
            dgView.DataSource = Nothing
            dgView.DataBind()
            If DFlag = "Delete" Then
            Else
                lblMsg.Visible = True
                onAdd()
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
        'txtRecNo.Text = "1"
        FillData(0)
    End Sub
    ''' <summary>
    ''' Method to Move to Next Record
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub OnMoveNext()
        '' txtRecNo.Text = CInt(txtRecNo.Text) + 1
        ''FillData(CInt(txtRecNo.Text) - 1)
    End Sub
    ''' <summary>
    ''' Method to Move to Previous Record
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub OnMovePrevious()
        'txtRecNo.Text = CInt(txtRecNo.Text) - 1
        'FillData(CInt(txtRecNo.Text) - 1)
    End Sub
    ''' <summary>
    ''' Method to Move to Last Record
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub OnMoveLast()
        'txtRecNo.Text = lblCount.Text
        'FillData(CInt(lblCount.Text) - 1)
    End Sub
    ''' <summary>
    ''' Method to Enable or Disable Navigation Buttons
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub DisableRecordNavigator()
        Dim flag As Boolean
        If Session("ListObj") Is Nothing Then
            flag = False
            'txtRecNo.Text = ""
            'lblCount.Text = ""
        Else
            flag = True
        End If
        'ibtnFirst.Enabled = flag
        'ibtnLast.Enabled = flag
        'ibtnPrevs.Enabled = flag
        'ibtnNext.Enabled = flag
        If flag = False Then
            'ibtnFirst.ImageUrl = "images/gnew_first.png"
            'ibtnLast.ImageUrl = "images/gnew_last.png"
            'ibtnPrevs.ImageUrl = "images/gnew_Prev.png"
            'ibtnNext.ImageUrl = "images/gnew_next.png"
        Else
            'ibtnFirst.ImageUrl = "images/new_last.png"
            'ibtnLast.ImageUrl = "images/new_first.png"
            'ibtnPrevs.ImageUrl = "images/new_Prev.png"
            'ibtnNext.ImageUrl = "images/new_next.png"
        End If
    End Sub
    ''' <summary>
    ''' Method to Display the list of Bank Profiles
    ''' </summary>
    ''' <param name="RecNo">Parameter is RecNo</param>
    ''' <remarks></remarks>
    Private Sub FillData(ByVal RecNo As Integer)
        'lblMsg.Visible = False
        'Conditions for Button Enable & Disable
        'If txtRecNo.Text = lblCount.Text Then
        '    ibtnNext.Enabled = False
        '    ibtnNext.ImageUrl = "images/gnew_next.png"
        '    ibtnLast.Enabled = False
        '    ibtnLast.ImageUrl = "images/gnew_last.png"
        'Else
        '    ibtnNext.Enabled = True
        '    ibtnNext.ImageUrl = "images/new_next.png"
        '    ibtnLast.Enabled = True
        '    ibtnLast.ImageUrl = "images/new_last.png"
        'End If
        'If txtRecNo.Text = "1" Then
        '    ibtnPrevs.Enabled = False
        '    ibtnPrevs.ImageUrl = "images/gnew_Prev.png"
        '    ibtnFirst.Enabled = False
        '    ibtnFirst.ImageUrl = "images/gnew_first.png"
        'Else
        '    ibtnPrevs.Enabled = True
        '    ibtnPrevs.ImageUrl = "images/new_prev.png"
        '    ibtnFirst.Enabled = True
        '    ibtnFirst.ImageUrl = "images/new_first.png"
        'End If

        Dim eobj As AFCEn
        listafc = Session("ListObj")
        eobj = listafc(RecNo)
        ddlFaculty.SelectedValue = eobj.SAFC_Code
        If eobj.PostedFor = "Semester" Then
            rbSemester.Checked = True
            rbAnnual.Checked = False
        Else
            rbSemester.Checked = False
            rbAnnual.Checked = True
        End If
        'txtDesc.Text = eobj.Description
        'addDesc()
        'ddlDesc.SelectedValue = eobj.Description
        'ddlDesc.SelectedValue = eobj.Description
        'txtBatchNo.Text = eobj.BatchCode
        txtBatchDate.Text = eobj.Bdate
        txtInvoiceDate.Text = eobj.TransDate
        txtDueDate.Text = eobj.DueDate

        'ddlSemester.SelectedItem.Text = eobj.Semester.Trim
        ddlSemester.SelectedValue = eobj.Semester
        If eobj.Reference = "Ready" Then
            lblStatus.Value = "Ready"
            ibtnStatus.ImageUrl = "images/Ready.gif"
        End If
        If eobj.Reference = "Posted" Then
            lblStatus.Value = "Posted"
            ibtnStatus.ImageUrl = "images/Posted.gif"
        End If
        dgView.DataSource = listafc
        dgView.DataBind()
        dgView.Columns(6).Visible = False
        dgView.Columns(7).Visible = False
        Dim chek As New CheckBox
        Dim dgItem1 As GridViewRow
        Dim link As New HyperLink
        Dim i As Integer = 0
        For Each dgItem1 In dgView.Rows
            chek = dgItem1.Cells(0).Controls(1)
            chek.Checked = True
            link = dgItem1.Cells(8).Controls(1)
            link.Attributes.Add("onClick", "OpenWindow1('about:blank')")
            link.NavigateUrl = "FeeStructure.aspx?Menuid=16&Formid=FS&IsStudentLedger=1&ProgramId=" & dgItem1.Cells(1).Text & "&Semester=" & dgItem1.Cells(5).Text
            link.Target = "MyPopup"
        Next

        setDateFormat()
    End Sub
    ''' <summary>
    ''' Method to Search for Posted AFC's
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub OnSearchOthers(ByVal strBatchCode As String)

        Dim listAFC As New List(Of ProgramInfoEn)
        Dim AFClink As New HyperLink
        Dim loEnStud As New StudentEn
        Dim Program As String = Nothing, Intake As String = Nothing, Faculty As String = Nothing, Bidang As String = Nothing, CurrSem As String = Nothing

        listAFC = Session("listprog")
        If listAFC.Count > 0 Then
            Program = listAFC(0).ProgramID
            Intake = listAFC(0).Semester
            Faculty = listAFC(0).Faculty
            Bidang = listAFC(0).BidangCode
            CurrSem = listAFC(0).CurrentSemester
        Else
            lblMsg.Text = "AFC Details Is Not Exist"
        End If

        AFClink.Attributes.Add("onClick", "OpenWindow1('about:blank')")
        AFClink.NavigateUrl = "FeeStructure.aspx?Formid=FS&IsFeePosting=1&ProgramId=" & Program & "&Semester=" & Intake & "&PostStatus=1&BatchCode=" & strBatchCode & "&Faculty=" & Faculty & "&BidangCode=" & Bidang & "&CurrSem=" & CurrSem
        AFClink.Target = "MyPopup"

        ddlBidang.Enabled = False
        ddlCurrSem.Enabled = False
        ddlstatus.Enabled = False

    End Sub
    ''' <summary>
    ''' Method to Lock the Record AFTER pOSTING
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub PostEnFalse()
        ibtnNew.Enabled = False
        ibtnNew.ImageUrl = "images/gadd.png"
        ibtnNew.ToolTip = "Access Denied"
        ibtnSave.Enabled = False
        ibtnSave.ImageUrl = "images/gsave.png"
        ibtnSave.ToolTip = "Access Denied"
        ibtnDelete.Enabled = False
        ibtnDelete.ImageUrl = "images/gdelete.png"
        ibtnDelete.ToolTip = "Access Denied"
        'ibtnView.Enabled = False
        'ibtnView.ImageUrl = "images/ready.png"
        'ibtnView.ToolTip = "Access Denied"
        ibtnPrint.Enabled = True
        ibtnPrint.ImageUrl = "images/print.png"
        ibtnPrint.ToolTip = "Print"
        ibtnPosting.Enabled = False
        ibtnPosting.ImageUrl = "images/gposting.png"
        ibtnPosting.ToolTip = "Access denied"
        'ibtnOthers.Enabled = False
        'ibtnOthers.ImageUrl = "images/post.png"
        'ibtnOthers.ToolTip = "Access denied"
    End Sub
    ''' <summary>
    ''' Method to Clear the Field Values
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub onClearData()
        ibtnStatus.ImageUrl = "images/notready.gif"
        lblStatus.Value = "New"
        If lblStatus.Value = "New" Then
            ibtnSave.Enabled = True
            ibtnSave.ImageUrl = "images/save.png"
            ibtnSave.ToolTip = "Edit"
        End If
        Session("ListObj") = Nothing
        DisableRecordNavigator()
        addfaculty()
        ddlFaculty.SelectedValue = -1
        txtBatchNo.ReadOnly = False
        txtBatchNo.Text = ""
        'txtRecNo.Text = ""
        'lblCount.Text = ""
        'ddlDesc.SelectedIndex = -1
        ddlCurrSem.SelectedIndex = -1
        ddlProgramInfo.SelectedIndex = -1
        ddlSemester.SelectedIndex = -1
        ddlstatus.SelectedIndex = -1
        rbSemester.Checked = False
        rbAnnual.Checked = False
        'txtDesc.Text = ""
        'ddlDesc.SelectedIndex = 0
        txtBatchDate.Text = ""
        txtInvoiceDate.Text = ""
        txtDueDate.Text = ""
        'dates()
        dgView.DataSource = Nothing
        dgView.DataBind()
        ViewState("SelectedRecords") = Nothing
        Session("listprog") = Nothing
        ddlSemester.Enabled = False
    End Sub
#End Region

#Region "Post To SAGA"
    Private Sub PostAFC(ByVal strBatchCode As String)
        Try
            objIntegration.InvoiceDebitCredit(strBatchCode, "AFC")
        Catch ex As Exception
            lblMsg.Text = ex.Message.ToString()
            LogError.Log("FeePosting", "PostAFC", ex.Message)
        End Try
    End Sub
#End Region

    Protected Sub ibtnCancel_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ibtnCancel.Click
        LoadUserRights()
        onAdd()
    End Sub

    Protected Sub ibtnDelete_Click1(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ibtnDelete.Click
        Dim RecAff As Boolean
        Dim eob As New AFCEn
        Dim bsobj As New AFCBAL
        If ddlstatus.SelectedValue = 1 Then
            Dim AFCBAL As New AFCBAL
            Dim dgitem As GridViewRow
            Dim CHK As CheckBox
            Dim status As Boolean = False
            Try
                If dgView.Rows.Count > 0 Then


                    For Each dgitem In dgView.Rows
                        CHK = dgitem.Cells(0).Controls(1)
                        If CHK.Checked = True Then
                            eob.BatchCode = dgitem.Cells(6).Text.Trim()
                            RecAff = bsobj.BatchDelete(eob, "Delete")
                            'onAdd()
                            DFlag = "Delete"
                            Session("loaddata") = "View"
                            lblMsg.Text = "Record Deleted Successfully "
                            lblMsg.Visible = True
                            'LoadListObjects()
                            status = True
                        End If
                    Next
                    If (Not status) Then
                        lblMsg.Text = "Kindly select a record to delete"
                    End If
                Else
                    lblMsg.Text = "There is no record to delete"
                End If
                'ddlstatus.SelectedIndex = -1
                'ddlFaculty.SelectedIndex = -1
                'ddlProgramInfo.SelectedIndex = -1
                'ddlSemester.SelectedIndex = -1
                GetData(ddlFaculty.SelectedValue, ddlProgramInfo.SelectedValue, ddlSemester.SelectedValue, "", ddlCurrSem.SelectedValue)
            Catch ex As Exception
                lblMsg.Text = ex.Message.ToString()
                LogError.Log("FeePosting", "ibtnDelete_Click1", ex.Message)
            End Try
        Else
            If dgView.Rows.Count > 0 Then
                lblMsg.Text = "Records are not in Ready Status"
            Else
                lblMsg.Text = "Records are Deleted successfully"
            End If

        End If
    End Sub

    Protected Sub ibtnPrint_Click1(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ibtnPrint.Click
        Dim batchNo As String = ConstructBatchNumber()
        LoadReport(batchNo)
    End Sub

    Protected Sub rbSemester_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles rbSemester.CheckedChanged
        'If rbSemester.Checked = True Then
        '    ddlSemester.Enabled = True
        'Else
        '    ddlSemester.Enabled = False
        'End If
    End Sub

    Protected Sub Page_LoadComplete(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.LoadComplete
        If rbSemester.Checked = True Then
            ddlSemester.Enabled = True
        ElseIf rbAnnual.Checked = True Then
            ddlSemester.Enabled = False
        Else
            ddlSemester.Enabled = False
        End If

    End Sub
    Protected Sub rbAnnual_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles rbAnnual.CheckedChanged

    End Sub

    Protected Sub ddlProgramInfo_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlProgramInfo.SelectedIndexChanged
        GetData(ddlFaculty.SelectedValue, ddlProgramInfo.SelectedValue, ddlSemester.SelectedValue, "", ddlCurrSem.SelectedValue)
    End Sub

    Protected Sub ddlSemester_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlSemester.SelectedIndexChanged
        GetData(ddlFaculty.SelectedValue, ddlProgramInfo.SelectedValue, ddlSemester.SelectedValue, "", ddlCurrSem.SelectedValue)
    End Sub

    Protected Sub ddlstatus_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlstatus.SelectedIndexChanged
        'txtBatchNo.Text = ""
        GetData(ddlFaculty.SelectedValue, ddlSemester.SelectedValue, ddlBidang.SelectedValue, "", ddlCurrSem.SelectedValue)
        lblMsg.Text = ""
    End Sub

    Protected Sub ddlCurrSem_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlCurrSem.SelectedIndexChanged
        GetData(ddlFaculty.SelectedValue, ddlSemester.SelectedValue, ddlBidang.SelectedValue, "", ddlCurrSem.SelectedValue)
        lblMsg.Text = ""
    End Sub

#Region "Added Select All Checkbox"
    'added by Hafiz @ 19/10/2016

    Protected Sub chkboxSelectAll_CheckedChanged(sender As Object, e As EventArgs)

        Dim i As Integer = 0
        Dim list As New List(Of Integer)
        Dim _lst As List(Of ProgramInfoEn) = Session("listprog")
        Dim ChkBoxHeader As CheckBox = DirectCast(dgView.HeaderRow.FindControl("chkboxSelectAll"), CheckBox)

        While i < dgView.PageCount

            dgView.PageIndex = i

            If dgView.PageIndex = 0 Then
                For Each row As GridViewRow In dgView.Rows

                    Dim chckrw As CheckBox = row.FindControl("chk")
                    Dim container As IDataItemContainer = chckrw.NamingContainer

                    If ChkBoxHeader.Checked = True Then
                        chckrw.Checked = True
                    Else
                        chckrw.Checked = False
                    End If
                Next
                Exit While
            End If
            i += 1
        End While

        i = 0
        While i < _lst.Count
            If ChkBoxHeader.Checked = True Then
                If Not list.Contains(i) Then
                    list.Add(i)
                End If
            Else
                If list.Contains(i) Then
                    list.Remove(i)
                End If
            End If
            i += 1
        End While

        ViewState("SelectedRecords") = list

        Dim cb_after As CheckBox = DirectCast(dgView.HeaderRow.FindControl("chkboxSelectAll"), CheckBox)
        If ChkBoxHeader.Checked = False Then
            cb_after.Checked = False
        Else
            cb_after.Checked = True
        End If

    End Sub

    Protected Sub dgView_PageIndexChanging(source As Object, e As GridViewPageEventArgs) Handles dgView.PageIndexChanging

        Dim list As New List(Of Integer)

        If ViewState("SelectedRecords") IsNot Nothing Then
            list = DirectCast(ViewState("SelectedRecords"), List(Of Integer))
        End If

        For Each gv As GridViewRow In dgView.Rows

            Dim _CheckBox As CheckBox = gv.Cells(0).Controls(1)
            Dim container As IDataItemContainer = _CheckBox.NamingContainer

            If _CheckBox.Checked = True Then
                If Not list.Contains(container.DataItemIndex) Then
                    list.Add(container.DataItemIndex)
                End If
            Else
                If list.Contains(container.DataItemIndex) Then
                    list.Remove(container.DataItemIndex)
                End If
            End If

        Next

        ViewState("SelectedRecords") = list

        dgView.PageIndex = e.NewPageIndex

        If Trim(txtBatchNo.Text) <> "" Then
            GetData(ddlFaculty.SelectedValue, ddlProgramInfo.SelectedValue, ddlSemester.SelectedValue, txtBatchNo.Text, ddlCurrSem.SelectedValue)
        Else
            GetData(ddlFaculty.SelectedValue, ddlSemester.SelectedValue, ddlBidang.SelectedValue, "", ddlCurrSem.SelectedValue)
        End If

    End Sub

    Protected Sub dgView_RowDataBound(source As Object, e As GridViewRowEventArgs) Handles dgView.RowDataBound

        Dim _lst As List(Of Integer) = DirectCast(ViewState("SelectedRecords"), List(Of Integer))

        If e.Row.RowType = DataControlRowType.DataRow AndAlso _lst IsNot Nothing Then

            Dim container As IDataItemContainer = e.Row.Cells(0).NamingContainer
            Dim ChkBoxHeader As CheckBox = DirectCast(dgView.HeaderRow.FindControl("chkboxSelectAll"), CheckBox)
            Dim _CheckBox As CheckBox = DirectCast(e.Row.FindControl("chk"), CheckBox)

            If _lst.Contains(container.DataItemIndex) Then
                ChkBoxHeader.Checked = True
                _CheckBox.Checked = True
            Else
                ChkBoxHeader.Checked = False
                _CheckBox.Checked = False
            End If

        End If

    End Sub

#End Region

    Private Function ConstructBatchNumber() As String
        Dim AFCBAL As New AFCBAL
        Dim dgitem As GridViewRow
        Dim CHK As CheckBox
        Dim BatchNumber As String = String.Empty
        Dim BatchNumberRpt As String = String.Empty
        For Each dgitem In dgView.Rows
            CHK = dgitem.Cells(0).Controls(1)
            If CHK.Checked = True Then
                BatchNumber += "'" & dgitem.Cells(6).Text & "',"
                BatchNumberRpt += dgitem.Cells(6).Text & ","
            End If
        Next
        If Not String.IsNullOrEmpty(BatchNumber) Then
            BatchNumber = BatchNumber.TrimEnd(",")
            BatchNumberRpt = BatchNumberRpt.TrimEnd(",")
        End If
        Return BatchNumber & "|" & BatchNumberRpt
    End Function

    Protected Sub txtBatchNo_TextChanged(sender As Object, e As EventArgs) Handles txtBatchNo.TextChanged
        GetData(ddlFaculty.SelectedValue, ddlProgramInfo.SelectedValue, ddlSemester.SelectedValue, txtBatchNo.Text, ddlCurrSem.SelectedValue)
    End Sub

    Protected Sub ibtnView_Click(sender As Object, e As ImageClickEventArgs) Handles ibtnView.Click
        If (Not String.IsNullOrEmpty(txtBatchNo.Text)) Then
            dgView.PageIndex = 1
        End If
        GetData(ddlFaculty.SelectedValue, ddlProgramInfo.SelectedValue, ddlSemester.SelectedValue, "", ddlCurrSem.SelectedValue)
    End Sub

    Private Sub addBidangCode()

        Dim eobjS As New BidangEn
        Dim bsobj As New BidangBAL
        Dim list As New List(Of BidangEn)
        eobjS.BidangCode = ""
        eobjS.Description = ""
        eobjS.Status = True
        ddlBidang.Items.Clear()
        ddlBidang.Items.Add(New ListItem("---Select---", "-1"))
        ddlBidang.DataTextField = "Description"
        ddlBidang.DataValueField = "BidangCode"
        If Session("PageMode") = "Add" Then
            Try
                list = bsobj.GetBidangList(eobjS)
            Catch ex As Exception
                LogError.Log("FeePosting", "addBidangCode", ex.Message)
            End Try

        Else

            Try
                list = bsobj.GetBidangList(eobjS)
            Catch ex As Exception
                LogError.Log("FeePosting", "addBidangCode", ex.Message)
            End Try
        End If

        Session("Bidang") = list
        ddlBidang.DataSource = list
        ddlBidang.DataBind()

    End Sub

    Private Sub addSemesterCode()

        Dim eobjS As New SemesterSetupEn
        Dim bsobj As New SemesterSetupBAL
        Dim listSem As New List(Of SemesterSetupEn)
        eobjS.SemisterSetupCode = ""
        eobjS.Description = ""
        eobjS.Status = True
        ddlCurrSem.Items.Clear()
        ddlCurrSem.Items.Add(New ListItem("---Select---", "-1"))
        ddlCurrSem.DataTextField = "SemisterCode2"
        ddlCurrSem.DataValueField = "SemisterSetupCode"
        If Session("PageMode") = "Add" Then
            Try
                listSem = bsobj.GetCurrentSessionList(eobjS)
            Catch ex As Exception
                LogError.Log("FeePosting", "addSemesterCode", ex.Message)
            End Try

        Else

            Try
                listSem = bsobj.GetCurrentSessionList(eobjS)
            Catch ex As Exception
                LogError.Log("FeePosting", "addSemesterCode", ex.Message)
            End Try
        End If

        Session("Semester") = listSem
        ddlCurrSem.DataSource = listSem
        ddlCurrSem.DataBind()

    End Sub

    Protected Sub ddlBidang_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlBidang.SelectedIndexChanged
        GetData(ddlFaculty.SelectedValue, ddlSemester.SelectedValue, ddlBidang.SelectedValue, "", ddlCurrSem.SelectedValue)
        lblMsg.Text = ""
    End Sub

    Private Sub CheckAllValues()

        Dim categoryIDList As New ArrayList()
        Dim index As Integer = -1
        'For Each row As GridViewRow In dgView.Items
        For Each dgItem As GridViewRow In dgView.Rows

            'index = CInt(dgView.DataKeys(row.RowIndex).Value)
            'Dim result As Boolean = DirectCast(row.FindControl("chk"), CheckBox).Checked

            'Dim result As Boolean = DirectCast(dgItem.Cells(0).Controls(0), CheckBox).Checked
            Dim result As Boolean = DirectCast(dgItem.Cells(0).Controls(0), CheckBox).Checked

            ' Check in the Session
            If Session("CHECKED_ITEMS") IsNot Nothing Then
                categoryIDList = DirectCast(Session("CHECKED_ITEMS"), ArrayList)
            End If
            If result Then
                If Not categoryIDList.Contains(index) Then
                    categoryIDList.Add(index)
                End If
            Else
                categoryIDList.Remove(index)
            End If
        Next
        If categoryIDList IsNot Nothing AndAlso categoryIDList.Count > 0 Then
            Session("CHECKED_ITEMS") = categoryIDList
        End If
    End Sub

#Region "dgView Check Control"
    'added by Hafiz @ 03/8/2016
    'added checkbox control for Status=Ready

    Protected Sub Chk_OnCheckedChanged()

        Dim _BatchId As String = ""

        If ddlstatus.SelectedValue = "1" Then

            If String.IsNullOrEmpty(txtBatchNo.Text) Then
                For Each dgItem As GridViewRow In dgView.Rows
                    Dim chk As CheckBox = dgItem.Cells(0).Controls(1)
                    If chk.Checked = True Then
                        _BatchId = dgItem.Cells(6).Text
                        LoadGetData(_BatchId)
                        CheckWorkflowStatus(_BatchId)
                        Exit For
                    End If
                Next
            Else
                ClearCheckedData()
            End If

        ElseIf ddlstatus.SelectedValue = "0" Then

            Dim list As New List(Of Integer)

            If ViewState("SelectedRecords") IsNot Nothing Then
                list = DirectCast(ViewState("SelectedRecords"), List(Of Integer))
            End If

            For Each gv As GridViewRow In dgView.Rows

                Dim _CheckBox As CheckBox = gv.Cells(0).Controls(1)
                Dim container As IDataItemContainer = _CheckBox.NamingContainer

                If _CheckBox.Checked = True Then

                    If Not list.Contains(container.DataItemIndex) Then
                        list.Add(container.DataItemIndex)
                    End If
                Else
                    If list.Contains(container.DataItemIndex) Then
                        list.Remove(container.DataItemIndex)
                    End If

                End If

            Next

            ViewState("SelectedRecords") = list

        End If

    End Sub

    Sub LoadGetData(ByVal _BatchId As String)
        GetData(ddlFaculty.SelectedValue, ddlSemester.SelectedValue, ddlBidang.SelectedValue, _BatchId, ddlCurrSem.SelectedValue)
    End Sub

    Sub ClearCheckedData()
        For Each dgItem As GridViewRow In dgView.Rows
            Dim _CheckBox As CheckBox = dgItem.Cells(0).Controls(1)
            _CheckBox.Checked = False
        Next
        lblMsg.Text = ""
        LoadGetData("")
    End Sub

#End Region

#Region "GetApprovalDetails"

    Protected Function GetMenuId() As Integer

        Dim MenuId As Integer = New MenuDAL().GetMenuMasterList().Where(Function(x) x.PageName = "AFC").Select(Function(y) y.MenuID).FirstOrDefault()
        Return MenuId

    End Function

#End Region

#Region "For popup AddApprover() close"

    Protected Sub btnHiddenApp_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnHiddenApp.Click

    End Sub

#End Region

#Region "CheckWorkflowStatus"

    Protected Sub CheckWorkflowStatus(ByVal BatchId As String)

        If BatchId <> "" Then
            Dim _list As List(Of WorkflowEn) = New WorkflowDAL().GetList().Where(Function(x) x.BatchCode = BatchId).ToList()
            If _list.Count > 0 Then

                lblMsg.Visible = True

                If _list.Where(Function(x) x.WorkflowStatus = 1).Count > 0 Then
                    lblMsg.Text = "Record Pending For Approval."
                ElseIf _list.Where(Function(x) x.WorkflowStatus = 3).Count > 0 Then
                    lblMsg.Text = "Record Rejected By Approval. [Reason:" & _list.Where(Function(x) x.WorkflowStatus = 3).Select(Function(y) y.WorkflowRemarks).FirstOrDefault() & "]"
                Else
                    lblMsg.Text = ""
                End If

            End If
        Else
            lblMsg.Text = ""
        End If

    End Sub

#End Region

End Class


