Imports System.Collections.Generic
Imports HTS.SAS.Entities
Imports HTS.SAS.BusinessObjects
Imports HTS.SAS.DataAccessObjects
Imports System.Data
Imports System.Data.SqlClient
Imports System.Globalization
Imports SQLPowerQueryManager.PowerQueryManager
Imports System.Linq


Partial Class BatchInvoice
    Inherits System.Web.UI.Page
#Region "Global Declarations "
    'Global Declaration - Starting
    Private _Helper As New Helper
    Private ListTRD As New List(Of AccountsDetailsEn)
    Private ListObjects As List(Of AccountsEn)
    Private _GstSetupDal As New HTS.SAS.DataAccessObjects.GSTSetupDAL

    Dim listStu As New List(Of StudentEn)
    Private ErrorDescription As String
    Private sumAmt As Double = 0
    Dim sumGST As Double = 0
    Dim DFlag As String
    Dim AutoNo As Boolean
    Dim ListObjectsStudent As List(Of StudentEn)
    Dim MJ_JnlLineEn As New BusinessEntities.MJ_JnlLine
    Dim MJ_JnlHdrEn As New BusinessEntities.MJ_JnlHdr
    Dim AutoNumberEn As New BusinessEntities.AutoNumberEn
    Dim AccountEn As New BusinessEntities.AccountsEn
    Dim objIntegrationDL As New SQLPowerQueryManager.PowerQueryManager.IntegrationDL
    Dim objIntegration As New IntegrationModule.IntegrationNameSpace.Integration

    Dim dsReturn As New DataSet
    Dim dsReturn_II As New DataSet

    'Selection Criteria Declaration - start
    Private scFaculty As String
    Private scProgram As New List(Of String)
    Private scSponsor As New List(Of String)
    Private scHostel As New List(Of String)
    Private scStudentCategory As String
    Private scSemester As String
    Private getStudentDetailsChange As New List(Of StudentEn)
    'Selection Criteria Declaration - start
    Private StuChgMatricNo As New List(Of StudentEn)
    Private StuToSave As New List(Of StudentEn)
    'Global Declaration - Ended
#End Region

#Region "Done By "

    Public Function DoneBy() As Integer

        Return MaxGeneric.clsGeneric.
            NullToInteger(Session(Helper.UserSession))

    End Function

#End Region

    ''Private LogErrors As LogError
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        'load PageName
        If Not IsPostBack() Then
            'Getting MenuId
            Menuname(CInt(Request.QueryString("Menuid")))
            Session("Menuid") = Request.QueryString("Menuid")
            MenuId.Value = GetMenuId()
            lblStatus.Value = "New"

            If CInt(Request.QueryString("IsStudentLedger")).Equals(1) Then
                btnViewStu.Visible = False
                btnSelection.Visible = False
            End If
            'Added 15/3/2016 for view details @ workflow
            If CInt(Request.QueryString("IsView")).Equals(1) Then
                'btnViewStu.Visible = False
                'btnSelection.Visible = False
                Session("loaddata") = "View"
            End If
            
            Session("ReceiptFor") = "St"
            Session("BatchNo") = Nothing
            'Client Validations
            ibtnDelete.Attributes.Add("onClick", "return getconfirm()")
            ibtnInDate.Attributes.Add("onClick", "return getDate1from()")
            ibtnDueDate.Attributes.Add("onClick", "return getDate2from()")
            ibtnPosting.Attributes.Add("onClick", "return getpostconfirm()")
            ibtnBDate.Attributes.Add("onClick", "return getibtnBDate()")
            ibtnSave.Attributes.Add("onCLick", "return Validate()")
            btnSelection.Attributes.Add("onCLick", "return Validate()")
            btnViewStu.Attributes.Add("onCLick", "return Validate()")
            txtRecNo.Attributes.Add("OnKeyup", "return geterr()")
            txtBatchDate.Attributes.Add("OnKeyup", "return CheckBatchDate()")
            txtSemster.Attributes.Add("OnKeyup", "return CheckSem()")
            txtDuedate.Attributes.Add("OnKeyup", "return CheckDueDate()")
            txtInvoiceDate.Attributes.Add("OnKeyup", "return CheckInvDate()")
            txtBatchDate.Attributes.Add("OnKeyup", "return CheckBatchDate()")
            'Load Upload File Sponsor
            btnupload.Attributes.Add("onclick", "new_window=window.open('FileSponsor.aspx?type=addstud','Hanodale','width=470,height=200,resizable=0');new_window.focus();")
            'Student Screen Popup
            ibtnStudent.Attributes.Add("onclick", "new_window=window.open('AddMulStudents.aspx?cat=St','Hanodale','width=520,height=600,resizable=0');new_window.focus();")
            'ibtnPosting.Attributes.Add("onclick", "new_window=window.open('AddApprover.aspx?MenuId=" & GetMenuId() & "','Hanodale','width=500,height=400,resizable=0');new_window.focus();")
            'Student FeeTypes Screen Popup
            'Student FeeTypes Screen Popup 

            'ibtnAddFeeType.Attributes.Add("onclick", "return CheckStudent()")            
            
            'Loading Date Fields
            OnLoadItem()
            lblMsg.Text = ""
            'Clearing Sesssions
            Session("PageMode") = "Add"
            Session("eobj") = Nothing
            Session("AddFee") = Nothing
            Session("LstStueObj") = Nothing
            Session("liststu") = Nothing
            Session("ListObj") = Nothing
            Session("Module") = Nothing
            ClearSession()
            'Loading UserRightsGetlisStudent     
            LoadUserRights()
            'Loading Navigation Controls
            DisableRecordNavigator()
            'Formatting DateFields
            dates()
            LoadStudentCategory()
            LoadFaculty()
            LoadFields()
            'If dgView.Items.Count <= 0 Then
            '    txtTotal.Visible = False
            '    lblTotal.Visible = False
            'End If
            If dgFeeType.Items.Count > 0 Then
                lblTotalFeeAmt.Visible = True
                txtTotalFeeAmt.Visible = True
            Else
                lblTotalFeeAmt.Visible = False
                txtTotalFeeAmt.Visible = False
            End If

            Session("isView") = True

        End If

        ibtnPrint.Attributes.Add("onCLick", "return getPrint()")
        If Request.QueryString("Formid") = "Inv" Then
            btnBatchInvoice.Text = "Invoice"
            Page.Title = String.Format("Student Invoice")
            btnChangeProg.Visible = False
            btnChangeCdtHr.Visible = False
            btnChangeHostel.Visible = False
        ElseIf Request.QueryString("Formid") = "DN" Then
            btnBatchInvoice.Text = "Debit Note"
            Page.Title = String.Format("Student Debit Note")
            btnChangeProg.Visible = False
            btnChangeCdtHr.Visible = True
            btnChangeHostel.Visible = False
            btnChangeHostel2.Visible = True
            'check the session("module") to enable/disable View Student tab - start
            If Session("Module") Is Nothing Then
                btnViewStu.Enabled = True
                btnUpdateCri.Enabled = True
            Else
                If Session("PageMode") <> "Edit" Then
                    btnViewStu.Enabled = False
                Else
                    btnViewStu.Enabled = True
                End If
                btnUpdateCri.Enabled = False
            End If
            'check the session("module") to enable/disable View Student tab - end

        ElseIf Request.QueryString("Formid") = "CN" Then
            btnBatchInvoice.Text = "Credit Note"
            Page.Title = String.Format("Student Credit Note")
            btnChangeProg.Visible = True
            btnChangeCdtHr.Visible = True
            btnChangeHostel.Visible = True
            'check the session("module") to enable/disable View Student tab - start
            If Session("Module") Is Nothing Then
                btnViewStu.Enabled = True
                btnUpdateCri.Enabled = True
            Else
                If Session("PageMode") <> "Edit" Then
                    btnViewStu.Enabled = False
                Else
                    btnViewStu.Enabled = True
                End If
                btnUpdateCri.Enabled = False
            End If
            'check the session("module") to enable/disable View Student tab - end
        End If
        lblMsg.Text = ""
        'Checking for Faculty
        If Not Session("eobj") Is Nothing Then
            addFeeType()
        End If
        'Checking for Added list of Students
        If Not Session("liststu") Is Nothing Then
            addStudent()
        End If

        If Not Session("CheckApproverList") Is Nothing Then
            SendToApproval()
        End If

        'Display Rcord from Student Ledger screen
        Try
            If Not Session("fileSponsor") Is Nothing And Session("fileType") = "excel" Then
                Dim importobj As New ImportData
                ListObjectsStudent = importobj.GetImportedSponsorData(Session("fileSponsor").ToString())
                LoadStudentsTemplates(ListObjectsStudent)
                Session("fileType") = Nothing
            End If
        Catch ex As Exception
            lblMsg.Text = ex.Message
        End Try

        If Session("isView") = True Then
            If Not Request.QueryString("BatchCode") Is Nothing Then
                Dim str As String = Request.QueryString("BatchCode")
                Dim constr As String() = str.Split(";")
                txtBatchNo.Text = constr(0)

                'added by Hafiz @ 22/3/2016
                'get matric no for stud ledger used - start
                If Not Request.QueryString("MatricNo") Is Nothing Then
                    Session("MatricNo") = Request.QueryString("MatricNo")
                End If
                'get matric no for stud ledger used - end

                DirectCast(Master.FindControl("Panel1"), System.Web.UI.WebControls.Panel).Visible = False
                DirectCast(Master.FindControl("td"), System.Web.UI.HtmlControls.HtmlTableCell).Visible = False

                pnlToolbar.Visible = False

                If CInt(Request.QueryString("IsStudentLedger")).Equals(1) Then
                    OnSearchOthers()
                End If
                If CInt(Request.QueryString("IsView")).Equals(1) Then
                    OnSearchView()
                End If

            End If
        End If
       
        If Not Session("Module") Is Nothing Then
            ibtnStudent.Enabled = False
            btnupload.Enabled = False
        Else
            ibtnStudent.Enabled = True
            btnupload.Enabled = True
        End If
    End Sub

    Protected Sub Page_LoadComplete(sender As Object, e As EventArgs) Handles Me.LoadComplete
        Session("isView") = False
    End Sub

    Protected Sub btnBatchInvoice_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnBatchInvoice.Click
        LoadBatchInvoice()
        checkStuList()
    End Sub

    Protected Sub btnSelection_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSelection.Click
        MultiView1.SetActiveView(View2)

        'imgLeft2.ImageUrl = "images/b_white_left.png"
        'imgRight2.ImageUrl = "images/b_white_right.png"
        btnSelection.CssClass = "TabButtonClick"


        'imgLeft1.ImageUrl = "images/b_orange_left.png"
        'imgRight1.ImageUrl = "images/b_orange_right.png"
        btnBatchInvoice.CssClass = "TabButton"

        'imgLeft3.ImageUrl = "images/b_orange_left.png"
        'imgRight3.ImageUrl = "images/b_orange_right.png"
        btnViewStu.CssClass = "TabButton"

        pnlBatch.Visible = False
        pnlSelection.Visible = True
        pnlView.Visible = False

        '
        SponsorGrid()
        FillSponsorCheckbox()
        '
        HostelGrid()
        FillHostelCheckbox()

    End Sub

    Protected Sub btnViewStu_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnViewStu.Click
        OnViewStudentGrid()
    End Sub
    Protected Sub ibtnNext_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ibtnNext.Click
        ClearSession()
        OnMoveNext()
    End Sub
    Protected Sub ibtnLast_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ibtnLast.Click
        ClearSession()
        OnMoveLast()
    End Sub
    Protected Sub ibtnPrevs_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ibtnPrevs.Click
        ClearSession()
        OnMovePrevious()
    End Sub
    Protected Sub ibtnFirst_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ibtnFirst.Click
        ClearSession()
        OnMoveFirst()
    End Sub
    Protected Sub ibtnNew_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ibtnNew.Click
        onAdd()
        onClearData()
        LoadBatchInvoice()
        ClearSession()
    End Sub
    Protected Sub ibtnDelete_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ibtnDelete.Click
        ondelete()
    End Sub
    Protected Sub ibtnCancel_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ibtnCancel.Click
        LoadUserRights()
        onAdd()
        onClearData()
        ClearSession()
    End Sub

    Protected Sub txtFeeAmt_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        hfValidateAmt.Value = False
        Dim eobjTRD As AccountsDetailsEn
        Dim dgitem As DataGridItem
        Dim i As Integer = 0
        Dim txt As TextBox
        Dim listStu As New List(Of StudentEn)
        Dim newListStu As New List(Of StudentEn)
        Dim objStu As New StudentEn
        'varaible declaration 
        Dim FeeAmount As Double, GSTAmt As Double, ActAmount As Double
        ', TaxPercentage As Double
        Dim TaxMode As String
        Dim GetGST As DataSet
        If Not Session(ReceiptsClass.SessionStuChange) Is Nothing Then
            listStu = Session(ReceiptsClass.SessionStuChange)
        End If
        Try
            If listStu.Count > 0 Then
                For Each dgitem In dgView.Items
                    Dim currMatricNo As String
                    Dim currFeeCode As String
                    currMatricNo = dgitem.Cells(dgViewCell.MatricNo).Text
                    currFeeCode = dgView.DataKeys(dgitem.ItemIndex).ToString
                    txt = dgitem.Cells(dgViewCell.Fee_Amount).Controls(1)
                    If txt.Text = "" Then txt.Text = 0
                    dgitem.Cells(dgViewCell.Fee_Amount).Text = String.Format("{0:F}", CDbl(txt.Text))
                    objStu = listStu.Where(Function(y) y.MatricNo = currMatricNo And y.ReferenceCode = currFeeCode).FirstOrDefault()

                    'GST, Actual Fee, Fee Amount Calculation - Start
                    Dim intFee As Double
                    If Not Double.TryParse(txt.Text, intFee) Then
                        lblMsg.Visible = True
                        lblMsg.Text = "Please Enter Valid Fee Amount"
                        hfValidateAmt.Value = True
                        Exit Sub
                    End If
                    FeeAmount = String.Format("{0:F}", CDbl(txt.Text))

                    Dim TaxId As Integer = MaxGeneric.clsGeneric.NullToInteger(dgitem.Cells(dgViewCell.TaxId).Text)
                    If Not TaxId = 0 Then
                        GetGST = _GstSetupDal.GetGstDetails(TaxId)
                    Else
                        Throw New Exception("TaxCode Missing")
                    End If

                    TaxMode = GetGST.Tables(0).Rows(0)(3).ToString()
                    If Not TaxId = 0 Then
                        GSTAmt = _GstSetupDal.GetGstAmount(TaxId, txt.Text)
                    Else
                        Throw New Exception("TaxCode Missing")
                    End If


                    If (TaxMode = Generic._TaxMode.Inclusive) Then
                        ActAmount = MaxGeneric.clsGeneric.NullToDecimal(FeeAmount) - GSTAmt
                    ElseIf (TaxMode = Generic._TaxMode.Exclusive) Then
                        ActAmount = FeeAmount
                        FeeAmount = MaxGeneric.clsGeneric.NullToDecimal(FeeAmount) + GSTAmt
                    End If
                    'GST, Actual Fee, Fee Amount Calculation - End

                    GSTAmt = String.Format("{0:F}", GSTAmt)
                    dgitem.Cells(dgViewCell.GSTAmount).Text = GSTAmt

                    objStu.TransactionAmount = FeeAmount
                    objStu.TaxAmount = GSTAmt
                    objStu.GSTAmount = GSTAmt

                    newListStu.Add(objStu)
                    objStu = Nothing
                    i = i + 1

                Next
                Session(ReceiptsClass.SessionStuChange) = newListStu
                dgView.DataSource = newListStu
                dgView.DataBind()
            Else
                For Each dgitem In dgView.Items
                    Session("AddFee") = Nothing
                    txt = dgitem.Cells(dgViewCell.Fee_Amount).Controls(1)
                    If txt.Text = "" Then txt.Text = 0
                    Dim intFee As Double
                    If Not Double.TryParse(txt.Text, intFee) Then
                        lblMsg.Visible = True
                        lblMsg.Text = "Please Enter Valid Fee Amount"
                        hfValidateAmt.Value = True
                        Exit Sub
                    End If
                    dgitem.Cells(dgViewCell.Fee_Amount).Text = String.Format("{0:F}", CDbl(txt.Text))

                    'GST, Actual Fee, Fee Amount Calculation - Start
                    FeeAmount = String.Format("{0:F}", CDbl(txt.Text))
                    Dim TaxId As Integer = MaxGeneric.clsGeneric.NullToInteger(dgitem.Cells(dgViewCell.TaxId).Text)
                    If Not TaxId = 0 Then
                        GetGST = _GstSetupDal.GetGstDetails(TaxId)
                    Else
                        Throw New Exception("TaxCode Missing")
                    End If
                    TaxMode = GetGST.Tables(0).Rows(0)(3).ToString()
                    ''In Edit mode, GST percentage will get from sas_accountsdetails Tax
                    'If Session("PageMode") = "Edit" Then
                    '    TaxPercentage = CDbl(dgitem.Cells(dgViewCell.Tax).Text)
                    '    GSTAmt = GetGSTWithPercentage(txt.Text, TaxPercentage, TaxMode)
                    'Else
                    '    'In Add mode, GST percentage will get from sas_gst_taxsetup according to taxmode and save the tax percentage
                    '    GSTAmt = _GstSetupDal.GetGstAmount(dgitem.Cells(dgViewCell.TaxId).Text, txt.Text)
                    '    TaxPercentage = GetGST.Tables(0).Rows(0)(4).ToString()
                    'End If

                    If Not TaxId = 0 Then
                        GSTAmt = _GstSetupDal.GetGstAmount(TaxId, txt.Text)
                    Else
                        Throw New Exception("TaxCode Missing")
                    End If
                    If (TaxMode = Generic._TaxMode.Inclusive) Then
                        ActAmount = MaxGeneric.clsGeneric.NullToDecimal(FeeAmount) - GSTAmt
                    ElseIf (TaxMode = Generic._TaxMode.Exclusive) Then
                        ActAmount = FeeAmount
                        FeeAmount = MaxGeneric.clsGeneric.NullToDecimal(FeeAmount) + GSTAmt
                    End If
                    'GST, Actual Fee, Fee Amount Calculation - End

                    GSTAmt = String.Format("{0:F}", GSTAmt)
                    dgitem.Cells(dgViewCell.GSTAmount).Text = GSTAmt
                    eobjTRD = New AccountsDetailsEn
                    eobjTRD.ReferenceCode = dgView.DataKeys(dgitem.ItemIndex).ToString
                    eobjTRD.Description = dgitem.Cells(dgViewCell.Description).Text
                    eobjTRD.TransactionAmount = FeeAmount 'String.Format("{0:F}", CDbl(txt.Text))
                    eobjTRD.Priority = dgitem.Cells(dgViewCell.Priority).Text
                    eobjTRD.TaxId = dgitem.Cells(dgViewCell.TaxId).Text
                    eobjTRD.GSTAmount = GSTAmt
                    eobjTRD.TaxAmount = GSTAmt
                    'eobjTRD.Tax = TaxPercentage
                    ListTRD.Add(eobjTRD)
                    eobjTRD = Nothing
                    i = i + 1
                Next
                Session("AddFee") = ListTRD
                dgView.DataSource = ListTRD
                dgView.DataBind()
                'AddTotal()
            End If
            If dgView.Items.Count <> 0 Then
                Dim chk As CheckBox
                Dim dgitem1 As DataGridItem
                chkSelectedView.Checked = True
                If chkSelectedView.Checked = True Then
                    For Each dgitem1 In dgView.Items
                        chk = dgitem1.Cells(0).Controls(1)
                        chk.Checked = True
                    Next
                End If
            End If

            If dgFeeType.Items.Count <> 0 Then
                Dim chk As CheckBox
                Dim dgitem2 As DataGridItem
                chkFeeType.Checked = True
                For Each dgitem2 In dgFeeType.Items
                    chk = dgitem2.Cells(0).Controls(1)
                    chk.Checked = True
                Next
            End If
        Catch ex As Exception
            If ex.Message = "TaxCode Missing" Then
                lblMsg.Visible = True
                lblMsg.Text = "Required Tax Type"
            End If
            Call MaxModule.Helper.LogError(ex.Message)
        End Try
    End Sub
    Protected Sub txtFeeAmt_dgFeeType_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        hfValidateAmt.Value = False
        Dim eobjTRD As AccountsDetailsEn
        Dim dgitem As DataGridItem
        Dim i As Integer = 0
        Dim txt As TextBox
        Dim listStuToUpdate As New List(Of StudentEn)
        Dim objAccDetails As New AccountsDetailsEn
        'varaible declaration 
        Dim FeeAmount As Double, GSTAmt As Double, ActAmount As Double

        ', TaxPercentage As Double
        Dim TaxMode As String
        Dim GetGST As DataSet
        If Not Session("AddFee") Is Nothing Then
            ListTRD = Session("AddFee")
        End If
        If Not Session(ReceiptsClass.SessionStuChange) Is Nothing Then
            listStu = Session(ReceiptsClass.SessionStuChange)
        End If
        Try
            If ListTRD.Count > 0 Then
                For Each dgitem In dgFeeType.Items
                    Dim currFeeCode As String
                    currFeeCode = dgFeeType.DataKeys(dgitem.ItemIndex).ToString
                    txt = dgitem.Cells(dgFeeTypeCell.Unit_Amount).Controls(1)
                    If txt.Text = "" Then txt.Text = 0
                    Dim intFee As Double
                    If Not Double.TryParse(txt.Text, intFee) Then
                        lblMsg.Visible = True
                        lblMsg.Text = "Please Enter Valid Fee Amount"
                        hfValidateAmt.Value = True
                        Exit Sub
                    End If
                    dgitem.Cells(dgFeeTypeCell.Unit_Amount).Text = String.Format("{0:F}", CDbl(txt.Text))
                    objAccDetails = ListTRD.Where(Function(y) y.ReferenceCode = currFeeCode).FirstOrDefault()
                    listStuToUpdate = listStu.Where(Function(x) x.ReferenceCode = currFeeCode).ToList()

                    'GST, Actual Fee, Fee Amount Calculation - Start
                    FeeAmount = String.Format("{0:F}", CDbl(txt.Text))
                    Dim TaxId As Integer = MaxGeneric.clsGeneric.NullToInteger(dgitem.Cells(dgFeeTypeCell.TaxId).Text)
                    If Not TaxId = 0 Then
                        GetGST = _GstSetupDal.GetGstDetails(TaxId)
                    Else
                        Throw New Exception("TaxCode Missing")
                    End If

                    TaxMode = GetGST.Tables(0).Rows(0)(3).ToString()
                    If Not TaxId = 0 Then
                        GSTAmt = _GstSetupDal.GetGstAmount(TaxId, txt.Text)
                    Else
                        Throw New Exception("TaxCode Missing")
                    End If

                    If (TaxMode = Generic._TaxMode.Inclusive) Then
                        ActAmount = MaxGeneric.clsGeneric.NullToDecimal(FeeAmount) - GSTAmt

                    ElseIf (TaxMode = Generic._TaxMode.Exclusive) Then
                        ActAmount = FeeAmount
                        FeeAmount = MaxGeneric.clsGeneric.NullToDecimal(FeeAmount) + GSTAmt
                    End If
                    'GST, Actual Fee, Fee Amount Calculation - End

                    GSTAmt = String.Format("{0:F}", GSTAmt)
                    dgitem.Cells(dgFeeTypeCell.GSTAmount).Text = GSTAmt


                    objAccDetails.TempPaidAmount = FeeAmount
                    objAccDetails.TaxAmount = GSTAmt
                    For Each obj In listStuToUpdate
                        obj.TransactionAmount = FeeAmount
                        obj.TaxAmount = GSTAmt
                        obj.GSTAmount = GSTAmt
                    Next

                Next
                'Session(ReceiptsClass.SessionStuChange) = newListStu
                'dgView.DataSource = newListStu
                'dgView.DataBind()
                dgFeeType.DataSource = ListTRD
                dgFeeType.DataBind()
                Session("AddFee") = ListTRD

                dgView.DataSource = listStu
                dgView.DataBind()
                Session(ReceiptsClass.SessionStuChange) = listStu
                hfStudentCount.Value = listStu.Count
            Else
                For Each dgitem In dgView.Items
                    Session("AddFee") = Nothing
                    txt = dgitem.Cells(dgViewCell.Fee_Amount).Controls(1)
                    If txt.Text = "" Then txt.Text = 0
                    dgitem.Cells(dgViewCell.Fee_Amount).Text = String.Format("{0:F}", CDbl(txt.Text))

                    'GST, Actual Fee, Fee Amount Calculation - Start
                    Dim intFee As Double
                    If Not Double.TryParse(txt.Text, intFee) Then
                        lblMsg.Visible = True
                        lblMsg.Text = "Please Enter Valid Fee Amount"
                        hfValidateAmt.Value = True
                        Exit Sub
                    End If
                    FeeAmount = String.Format("{0:F}", CDbl(txt.Text))
                    Dim TaxId As Integer = MaxGeneric.clsGeneric.NullToInteger(dgitem.Cells(dgViewCell.TaxId).Text)
                    If Not TaxId = 0 Then
                        GetGST = _GstSetupDal.GetGstDetails(TaxId)
                    Else
                        Throw New Exception("TaxCode Missing")
                    End If
                    TaxMode = GetGST.Tables(0).Rows(0)(3).ToString()
                    ''In Edit mode, GST percentage will get from sas_accountsdetails Tax
                    'If Session("PageMode") = "Edit" Then
                    '    TaxPercentage = CDbl(dgitem.Cells(dgViewCell.Tax).Text)
                    '    GSTAmt = GetGSTWithPercentage(txt.Text, TaxPercentage, TaxMode)
                    'Else
                    '    'In Add mode, GST percentage will get from sas_gst_taxsetup according to taxmode and save the tax percentage
                    '    GSTAmt = _GstSetupDal.GetGstAmount(dgitem.Cells(dgViewCell.TaxId).Text, txt.Text)
                    '    TaxPercentage = GetGST.Tables(0).Rows(0)(4).ToString()
                    'End If

                    If Not TaxId = 0 Then
                        GSTAmt = _GstSetupDal.GetGstAmount(TaxId, txt.Text)
                    Else
                        Throw New Exception("TaxCode Missing")
                    End If

                    If (TaxMode = Generic._TaxMode.Inclusive) Then
                        ActAmount = MaxGeneric.clsGeneric.NullToDecimal(FeeAmount) - GSTAmt
                    ElseIf (TaxMode = Generic._TaxMode.Exclusive) Then
                        ActAmount = FeeAmount
                        FeeAmount = MaxGeneric.clsGeneric.NullToDecimal(FeeAmount) + GSTAmt
                    End If
                    'GST, Actual Fee, Fee Amount Calculation - End

                    GSTAmt = String.Format("{0:F}", GSTAmt)
                    dgitem.Cells(dgViewCell.GSTAmount).Text = GSTAmt
                    eobjTRD = New AccountsDetailsEn
                    eobjTRD.ReferenceCode = dgView.DataKeys(dgitem.ItemIndex).ToString
                    eobjTRD.Description = dgitem.Cells(dgViewCell.Description).Text
                    eobjTRD.TransactionAmount = FeeAmount 'String.Format("{0:F}", CDbl(txt.Text))
                    eobjTRD.Priority = dgitem.Cells(dgViewCell.Priority).Text
                    eobjTRD.TaxId = dgitem.Cells(dgViewCell.TaxId).Text
                    eobjTRD.GSTAmount = GSTAmt
                    'eobjTRD.TaxAmount = GSTAmt
                    'eobjTRD.Tax = TaxPercentage
                    ListTRD.Add(eobjTRD)
                    eobjTRD = Nothing
                    i = i + 1
                Next
                Session("AddFee") = ListTRD
                dgView.DataSource = ListTRD
                dgView.DataBind()
                'AddTotal()
            End If
            If dgView.Items.Count <> 0 Then
                Dim chk As CheckBox
                Dim dgitem1 As DataGridItem
                chkSelectedView.Checked = True
                If chkSelectedView.Checked = True Then
                    For Each dgitem1 In dgView.Items
                        chk = dgitem1.Cells(0).Controls(1)
                        chk.Checked = True
                    Next
                End If
            End If

            If dgFeeType.Items.Count <> 0 Then
                Dim chk As CheckBox
                Dim dgitem2 As DataGridItem
                chkFeeType.Checked = True
                For Each dgitem2 In dgFeeType.Items
                    chk = dgitem2.Cells(0).Controls(1)
                    chk.Checked = True
                Next
            End If
        Catch ex As Exception
            If ex.Message = "TaxCode Missing" Then
                lblMsg.Visible = True
                lblMsg.Text = "Required Tax Type"
            End If
            Call MaxModule.Helper.LogError(ex.Message)
        End Try
    End Sub
    Protected Sub dgProgram_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dgProgram.ItemDataBound
        If Not Session(ReceiptsClass.SessionscProgram) Is Nothing Then
            scProgram = Session(ReceiptsClass.SessionscProgram)
        Else
            scProgram = New List(Of String)
        End If
        Dim Chk As CheckBox
        If chkSelectProgram.Checked = True Then
            Select Case e.Item.ItemType
                Case ListItemType.Item, ListItemType.AlternatingItem
                    Chk = CType(e.Item.FindControl("chkProgram"), CheckBox)
                    Chk.Checked = True
                    If Not scProgram.Contains(e.Item.Cells(1).Text) Then
                        scProgram.Add(e.Item.Cells(1).Text)
                        Session(ReceiptsClass.SessionscProgram) = scProgram
                    End If
            End Select
        End If
    End Sub

    Private Sub clearAllProgram()
        If Not Session(ReceiptsClass.SessionscProgram) Is Nothing Then
            scProgram = Session(ReceiptsClass.SessionscProgram)
        Else
            scProgram = New List(Of String)
        End If
        Dim chk As CheckBox
        Dim dgitem As DataGridItem
        For Each dgitem In dgProgram.Items
            chk = dgitem.Cells(0).Controls(1)
            chk.Checked = False
            scProgram.Remove(dgitem.Cells(1).Text)
        Next
        Session(ReceiptsClass.SessionscProgram) = scProgram
        rbProYes.Checked = False
        rbProNo.Checked = True
    End Sub

    Protected Sub chkSelectProgram_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkSelectProgram.CheckedChanged ', chkSelectProgram.Load
        If chkSelectProgram.Checked = True Then
            rbProYes.Checked = True
            rbProNo.Checked = False
        Else
            clearAllProgram()
            rbProYes.Checked = False
            rbProNo.Checked = True
        End If
        programGrid()
    End Sub
    Protected Sub rbSelectProgram_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        If rbProYes.Checked = True Then
            chkSelectProgram.Checked = True
        Else
            chkSelectProgram.Checked = False
            clearAllProgram()
        End If
        programGrid()
    End Sub
    Protected Sub DgSponsor_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles DgSponsor.ItemDataBound
        If Not Session(ReceiptsClass.SessionscSponsor) Is Nothing Then
            scSponsor = Session(ReceiptsClass.SessionscSponsor)
        Else
            scSponsor = New List(Of String)
        End If
        Dim Chk As CheckBox
        If chkSelectSponsor.Checked = True Then
            Select Case e.Item.ItemType
                Case ListItemType.Item, ListItemType.AlternatingItem
                    Chk = CType(e.Item.FindControl("chkSponsor"), CheckBox)
                    Chk.Checked = True
                    If Not scSponsor.Contains(e.Item.Cells(1).Text) Then
                        scSponsor.Add(e.Item.Cells(1).Text)
                        Session(ReceiptsClass.SessionscSponsor) = scSponsor
                    End If
            End Select
        End If
    End Sub

    Private Sub ClearAllSponsor()
        If Not Session(ReceiptsClass.SessionscSponsor) Is Nothing Then
            scSponsor = Session(ReceiptsClass.SessionscSponsor)
        Else
            scSponsor = New List(Of String)
        End If
        Dim chk As CheckBox
        Dim dgitem As DataGridItem
        For Each dgitem In DgSponsor.Items
            chk = dgitem.Cells(0).Controls(1)
            chk.Checked = False
            scSponsor.Remove(dgitem.Cells(1).Text)
        Next
        Session(ReceiptsClass.SessionscSponsor) = scSponsor
        rbSemNo.Checked = True
        rbSemYes.Checked = False
    End Sub

    Protected Sub chkSelectSponsor_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkSelectSponsor.CheckedChanged ', chkSelectSponsor.Load

        If chkSelectSponsor.Checked = True Then
            rbSemNo.Checked = False
            rbSemYes.Checked = True
        Else
            ClearAllSponsor()
            rbSemNo.Checked = True
            rbSemYes.Checked = False
        End If
        SponsorGrid()
    End Sub
    Protected Sub rbSelectSponsor_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        If rbSemYes.Checked = True Then
            chkSelectSponsor.Checked = True
        Else
            chkSelectSponsor.Checked = False
            ClearAllSponsor()
        End If
        SponsorGrid()
    End Sub

    Protected Sub dgHostel_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dgHostel.ItemDataBound
        If Not Session(ReceiptsClass.SessionscHostel) Is Nothing Then
            scHostel = Session(ReceiptsClass.SessionscHostel)
        Else
            scHostel = New List(Of String)
        End If
        Dim Chk As CheckBox
        If chkSelectHostel.Checked = True Then
            Select Case e.Item.ItemType
                Case ListItemType.Item, ListItemType.AlternatingItem
                    Chk = CType(e.Item.FindControl("chkHostel"), CheckBox)
                    Chk.Checked = True
                    If Not scHostel.Contains(e.Item.Cells(1).Text) Then
                        scHostel.Add(e.Item.Cells(1).Text)
                        Session(ReceiptsClass.SessionscHostel) = scHostel
                    End If
            End Select
        End If
    End Sub
    Protected Sub dgStudent_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dgStudent.ItemDataBound
        Dim Chk As CheckBox
        If chkStudent.Checked = True Then
            Select Case e.Item.ItemType
                Case ListItemType.Item, ListItemType.AlternatingItem
                    Chk = CType(e.Item.FindControl("chk"), CheckBox)
                    Chk.Checked = True
            End Select
        End If
    End Sub
    Private Sub clearAllHostel()
        If Not Session(ReceiptsClass.SessionscProgram) Is Nothing Then
            scProgram = Session(ReceiptsClass.SessionscProgram)
        Else
            scProgram = New List(Of String)
        End If
        Dim chk As CheckBox
        Dim dgitem As DataGridItem
        For Each dgitem In dgHostel.Items
            chk = dgitem.Cells(0).Controls(1)
            chk.Checked = False
            scHostel.Remove(dgitem.Cells(1).Text)
        Next
        Session(ReceiptsClass.SessionscProgram) = scProgram
        rbHosYes.Checked = False
        rbHosNo.Checked = True
    End Sub
    Protected Sub chkSelectHostel_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkSelectHostel.CheckedChanged ', chkSelectHostel.Load

        If chkSelectHostel.Checked = True Then
            rbHosYes.Checked = True
            rbHosNo.Checked = False
        Else
            clearAllHostel()
            rbHosYes.Checked = False
            rbHosNo.Checked = True
        End If
        HostelGrid()
    End Sub
    Protected Sub rbSelectHostel_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        If rbHosYes.Checked = True Then
            chkSelectHostel.Checked = True
        Else
            chkSelectHostel.Checked = False
            clearAllHostel()
        End If
        HostelGrid()
    End Sub
    Protected Sub rbSemIndividual_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles rbSemIndividual.CheckedChanged, rbSemIndividual.Load
        If rbSemIndividual.Checked = True Then
            txtSemster.Visible = True
        End If
    End Sub
    Protected Sub rbSemAll_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles rbSemAll.CheckedChanged, rbSemAll.Load
        If rbSemAll.Checked = True Then
            txtSemster.Visible = False
        End If
    End Sub
    Protected Sub ddlFaculty_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlFaculty.SelectedIndexChanged 'ddlFaculty.Load
        Session("FC_Code") = ddlFaculty.SelectedValue
        scFaculty = ddlFaculty.SelectedValue
        scProgram = New List(Of String)
        Session(ReceiptsClass.SessionscProgram) = scProgram
        programGrid()
    End Sub

    Protected Sub btnUpdateCri_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnUpdateCri.Click
        Dim objup As New StudentBAL
        Dim i As Integer = 0
        Dim lstobjects As New List(Of StudentEn)
        Dim eob As New StudentEn
        Dim sem As Integer = 0
        Dim faculty As String
        Dim cat As String

        If rbProYes.Checked = True Then inprogram()
        If rbSemYes.Checked = True Then inSponsor()
        If rbHosYes.Checked = True Then inHOstel()
        If rbSemAll.Checked = True Then
            eob.CurrentSemester = 0
        Else
            If txtSemster.Text = "" Then
                eob.CurrentSemester = 0
            Else
                eob.CurrentSemester = txtSemster.Text
            End If
        End If
        eob.STsponsercode = New StudentSponEn
        If Not Session("spnstr") Is Nothing Then
            eob.STsponsercode.Sponsor = Session("spnstr")
        Else
            eob.STsponsercode.Sponsor = ""
        End If
        If Not Session("sstr") Is Nothing Then
            eob.SAKO_Code = Session("sstr")
        Else
            eob.SAKO_Code = ""
        End If
        If Not Session("prgstr") Is Nothing Then
            eob.ProgramID = Session("prgstr")
        Else
            eob.ProgramID = ""
        End If
        If ddlFaculty.SelectedValue = "-1" Then
            faculty = ""
        Else
            faculty = ddlFaculty.SelectedValue

        End If
        If ddlStudentType.SelectedValue = "-1" Then
            cat = ""
        Else
            cat = ddlStudentType.SelectedValue

        End If
        hfStdCategory.Value = cat
        eob.Faculty = faculty
        eob.CategoryCode = cat
        eob.StCategoryAcess = New StudentCategoryAccessEn
        eob.StCategoryAcess.MenuID = Session("Menuid")

        Try
            lstobjects = objup.GetlisStudent(eob)
        Catch ex As Exception
            LogError.Log("BatchInvoice", "btnUpdateCri_Click", ex.Message)
        End Try

        pnlView.Visible = True

        'Adding in the exisiting list
        Dim mylst As List(Of StudentEn)
        'If Not Session("LstStueObj") Is Nothing Then
        '    Dim Flag As Boolean
        '    mylst = Session("LstStueObj")
        '    While i < lstobjects.Count
        '        Dim j As Integer = 0
        '        While j < mylst.Count
        '            Flag = False
        '            If mylst(j).MatricNo = lstobjects(i).MatricNo Then
        '                Flag = True
        '                Exit While
        '            End If
        '            j = j + 1
        '        End While
        '        If Flag = False Then
        '            mylst.Add(lstobjects(i))
        '        End If
        '        i = i + 1
        '    End While
        'Else
        '    mylst = lstobjects
        'End If
        mylst = lstobjects
        dgStudent.DataSource = mylst
        dgStudent.DataBind()
        If mylst.Count > 0 Then
            chkStudent.Checked = True
        End If
        Session("LstStueObj") = mylst
        Session("sstr") = ""
        Session("prgstr") = ""
        Session("spnstr") = ""
        hfStudentCount.Value = mylst.Count
        If Not lstobjects Is Nothing Then
            OnViewStudentGrid()
        Else

        End If
    End Sub

    Private Enum dgViewCell As Integer
        CheckBox = 0
        MatricNo = 1
        StudentName = 2
        ReferenceCode = 3
        Description = 4
        Fee_Amount = 5
        TransactionAmount = 6
        Fee_Code = 7
        Actual_Fee_Amount = 8
        GSTAmount = 9
        Total_Fee_Amount = 10
        Priority = 11
        TaxId = 12
    End Enum

    Private Enum dgFeeTypeCell As Integer
        CheckBox = 0
        MatricNo = 1
        StudentName = 2
        ProgramID = 3
        ReferenceCode = 4
        Description = 5
        Unit_Amount = 6
        Student_Qty = 7
        TransactionAmount = 8
        Fee_Code = 9
        Actual_Fee_Amount = 10
        GSTAmount = 11
        Total_Fee_Amount = 12
        Priority = 13
        TaxId = 14
    End Enum


    Protected Sub dgView_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dgView.ItemDataBound
        Dim txtAmount As TextBox
        Dim GSTAmt As Double = 0
        Dim TaxId As Integer = 0
        Dim TaxMode As Integer = 0

        TaxId = Session("TaxId")

        Select Case e.Item.ItemType
            Case ListItemType.Item, ListItemType.AlternatingItem, ListItemType.SelectedItem
                txtAmount = CType(e.Item.FindControl("txtFeeAmt"), TextBox)
                txtAmount.Attributes.Add("onKeyPress", "checkValue();")
                txtAmount.Text = String.Format("{0:F}", CDbl(txtAmount.Text))
                sumAmt = sumAmt + CDbl(e.Item.Cells(dgViewCell.TransactionAmount).Text)

                'GSTAmt = _GstSetupDal.GetGstAmount(TaxId, MaxGeneric.clsGeneric.NullToDecimal(CDbl(e.Item.Cells(4).Text)))
                'sumGST = sumGST + GSTAmt               
                sumGST = sumGST + CDbl(e.Item.Cells(dgViewCell.GSTAmount).Text)
                'GSTAmt = _GstSetupDal.GetGstAmount(TaxId, MaxGeneric.clsGeneric.NullToDecimal(FeeAmount))
            Case ListItemType.Footer
                e.Item.Cells(dgViewCell.TransactionAmount).Text = sumAmt.ToString
                e.Item.Cells(dgViewCell.Total_Fee_Amount).Text = String.Format("{0:F}", sumAmt)
                txtTotal.Text = String.Format("{0:F}", sumAmt)

                e.Item.Cells(dgViewCell.GSTAmount).Text = String.Format("{0:F}", sumGST)
                dgFeeType.Columns(dgFeeTypeCell.Total_Fee_Amount).FooterText = String.Format("{0:F}", sumAmt)
                dgFeeType.Columns(dgFeeTypeCell.GSTAmount).FooterText = String.Format("{0:F}", sumGST)
                txtTotalFeeAmt.Text = String.Format("{0:F}", sumAmt)
                dgFeeType.DataBind()
        End Select

    End Sub

    Protected Sub dgFeeType_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dgFeeType.ItemDataBound
        Dim txtAmount As TextBox
        Dim GSTAmt As Double = 0
        Dim TaxMode As Integer = 0

        Select Case e.Item.ItemType
            Case ListItemType.Item, ListItemType.AlternatingItem, ListItemType.SelectedItem
                txtAmount = CType(e.Item.FindControl("txtFeeAmt_dgFeeType"), TextBox)
                txtAmount.Attributes.Add("onKeyPress", "checkValue();")
                txtAmount.Text = String.Format("{0:F}", CDbl(txtAmount.Text))
        End Select
    End Sub

    Protected Sub ibtnSave_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ibtnSave.Click
        If Session("Module") Is Nothing Then
            If dgStudent.Items.Count = 0 Then
                lblMsg.Text = "Select At least One Student"
                lblMsg.Visible = True
                Exit Sub
            Else
                lblMsg.Visible = False
            End If
        End If

        If dgView.Items.Count = 0 Then
            lblMsg.Text = "Add At least One Fee Item"
            lblMsg.Visible = True
            Exit Sub
        Else
            lblMsg.Visible = False
        End If

        If btnViewStu.Enabled = True Then
            'validate for current process
            'If dgStudent.Items.Count = 0 Then
            '    lblMsg.Text = "Select At least One Student"
            '    lblMsg.Visible = True
            '    Exit Sub
            'Else
            '    lblMsg.Visible = False
            'End If
            'Call to add selected student list
            CreateListObjStudent()

            If Session("lstStu") Is Nothing Then
                lblMsg.Text = "Select At least One Student"
                lblMsg.Visible = True
                Exit Sub
            Else
                lblMsg.Visible = False
            End If
        End If

        If lblStatus.Value = "Posted" Then
            lblMsg.Text = "Post Record Cannot be Edited"
            lblMsg.Visible = True
            Exit Sub
        End If
        'If (Request.QueryString("Formid") = "CN" Or Request.QueryString("Formid") = "DN") And Not Session("Module") Is Nothing Then
        '    If dgView.Items.Count = 0 Then
        '        lblMsg.Text = "Please add at least one fee type"
        '        lblMsg.Visible = True
        '        Exit Sub
        '    Else
        '        CreateListObjStuToSave()
        '        If Not Session(ReceiptsClass.SessionStuToSave) Is Nothing Then
        '            StuToSave = Session(ReceiptsClass.SessionStuToSave)
        '        End If

        '        If StuToSave.Count = 0 Then
        '            lblMsg.Text = "Please select at least one student"
        '            lblMsg.Visible = True
        '            Exit Sub
        '        End If
        '    End If

        'End If

        CreateListObjStuToSave()
        If Not Session(ReceiptsClass.SessionStuToSave) Is Nothing Then
            StuToSave = Session(ReceiptsClass.SessionStuToSave)
        End If

        If StuToSave.Count = 0 Then
            lblMsg.Text = "Please select at least one student"
            lblMsg.Visible = True
            Exit Sub
        End If

        SpaceValidation()
        onSave()
        setDateFormat()
    End Sub

    Protected Sub ibtnView_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ibtnView.Click
        'LoadGrid()
        'Hide MatricNo and StudentName column - start    
        ClearSession()
        'Hide MatricNo and StudentName column - end
        LoadUserRights()
        Session("loaddata") = "View"
        If lblCount.Text <> "" Then
            If CInt(lblCount.Text) > 0 Then
                onClearData()
                If ibtnNew.Enabled = False Then
                    ibtnSave.Enabled = False
                    ibtnSave.ImageUrl = "images/gsave.png"
                    ibtnSave.ToolTip = "Access Denied"
                End If
            Else
                LoadListObjects()
                If lblCount.Text <> "" Then
                    Session("PageMode") = "Edit"
                Else
                    Session("PageMode") = "Add"
                End If
                'Session("BatchNo") = txtBatchNo.Text
            End If
        Else
            LoadListObjects()
            If lblCount.Text <> "" Then
                Session("PageMode") = "Edit"
            Else
                Session("PageMode") = "Add"
            End If
            'Session("BatchNo") = txtBatchNo.Text
        End If
    End Sub

    Protected Sub txtRecNo_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtRecNo.TextChanged

        If Trim(txtRecNo.Text).Length = 0 Then
            txtRecNo.Text = 0
            If lblCount.Text <> Nothing Then
                If CInt(txtRecNo.Text) > CInt(lblCount.Text) Then
                    txtRecNo.Text = lblCount.Text
                    'txtRecNo.ReadOnly = True

                End If
                FillData(CInt(txtRecNo.Text) - 1)
            Else
                txtRecNo.Text = ""
            End If
        Else
            If lblCount.Text <> Nothing Then
                If CInt(txtRecNo.Text) > CInt(lblCount.Text) Then
                    txtRecNo.Text = lblCount.Text
                End If
                FillData(CInt(txtRecNo.Text) - 1)
            Else
                txtRecNo.Text = ""
            End If
        End If
    End Sub

    'Protected Sub ibtnPosting_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ibtnPosting.Click

    '    'Varaible Declaration
    '    Dim BatchCode As String = MaxGeneric.clsGeneric.NullToString(txtBatchNo.Text)
    '    Dim Result As Boolean = False


    '    If lblStatus.Value = "New" Then
    '        lblMsg.Visible = True
    '        lblMsg.Text = "Record not Ready for posting"

    '    ElseIf lblStatus.Value = "Posted" Then
    '        lblMsg.Visible = True
    '        lblMsg.Text = "Record already posted"

    '    ElseIf lblStatus.Value = "Ready" Then
    '        'SpaceValidation()
    '        'OnPost()
    '        'setDateFormat()

    '        'Calling PostToWorFlow
    '        'Result = _Helper.PostToWorkflow(BatchCode, DoneBy(), Request.Url.AbsoluteUri)
    '        Result = _Helper.PostToWorkflow(BatchCode, Session("User"), Request.Url.AbsoluteUri)

    '        If Not Result = False Then
    '            lblMsg.Visible = True
    '            lblMsg.Text = "Record Posted Successfully for Approval"
    '            lblStatus.Value = "Posted"
    '            If lblStatus.Value = "Posted" Then
    '                ibtnStatus.ImageUrl = "images/Posted.gif"
    '                lblStatus.Value = "Posted"
    '            End If
    '            If Not Session("ListObj") Is Nothing Then
    '                ListObjects = Session("ListObj")
    '                Dim obj As AccountsEn
    '                obj = ListObjects(CInt(txtRecNo.Text) - 1)
    '                obj.PostStatus = "Posted"
    '                ListObjects(CInt(txtRecNo.Text) - 1) = obj
    '                Session("ListObj") = ListObjects

    '            End If
    '        Else
    '            lblMsg.Visible = True
    '            lblMsg.Text = "Record Already Posted"
    '        End If
    '    End If

    'End Sub

#Region "SendToApproval"

    Protected Sub SendToApproval()

        Try
            If lblStatus.Value = "New" Then
                lblMsg.Visible = True
                lblMsg.Text = "Record not Ready for posting"
            ElseIf lblStatus.Value = "Posted" Then
                lblMsg.Visible = True
                lblMsg.Text = "Record already posted"
            ElseIf lblStatus.Value = "Ready" Then

                If Not Session("listWF") Is Nothing Then
                    Dim list As List(Of WorkflowSetupEn) = Session("listWF")
                    If list.Count > 0 Then

                        Dim Result As Boolean = False
                        lblMsg.Text = ""

                        Result = _Helper.PostToWorkflow(MaxGeneric.clsGeneric.NullToString(txtBatchNo.Text), Session("User"), Request.Url.AbsoluteUri)

                        If Not Result = False Then

                            If Session("listWF").count > 0 Then
                                WorkflowApproverList(Trim(txtBatchNo.Text), Session("listWF"))
                            End If

                            lblMsg.Visible = True
                            lblMsg.Text = "Record Posted Successfully for Approval"
                            lblStatus.Value = "Posted"

                            If lblStatus.Value = "Posted" Then
                                ibtnStatus.ImageUrl = "images/Posted.gif"
                                lblStatus.Value = "Posted"
                            End If

                            If Not Session("ListObj") Is Nothing Then
                                ListObjects = Session("ListObj")
                                Dim obj As AccountsEn
                                obj = ListObjects(CInt(txtRecNo.Text) - 1)
                                obj.PostStatus = "Posted"
                                ListObjects(CInt(txtRecNo.Text) - 1) = obj
                                Session("ListObj") = ListObjects

                            End If
                        Else
                            lblMsg.Visible = True
                            lblMsg.Text = "Record Already Posted"
                        End If

                    Else
                        Throw New Exception("Posting to workflow failed caused NO approver selected.")
                    End If
                End If
                
            End If

        Catch ex As Exception
            lblMsg.Text = ex.Message.ToString()
        Finally
            Session("listWF") = Nothing
            Session("CheckApproverList") = Nothing
        End Try

    End Sub

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
            LogError.Log("Receipts", "InsertWFApprovalList", ex.Message)
        End Try

    End Sub

#End Region

    Protected Sub chkStudent_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)

        Dim chk As CheckBox
        Dim dgitem As DataGridItem
        Dim stuList As New List(Of StudentEn)

        If chkStudent.Checked = True Then
            For Each dgitem In dgStudent.Items
                chk = dgitem.Cells(0).Controls(1)
                chk.Checked = True
            Next
            checkStuList()
            If Not Session(ReceiptsClass.SessionStuChgMatricNo) Is Nothing Then
                stuList = Session(ReceiptsClass.SessionStuChgMatricNo)
            End If
            If dgFeeType.Items.Count = 0 Then
                lblMsg.Text = "Please Add Fee Code"
            Else
                addStuToExistingFee(stuList)
            End If
        Else
            For Each dgitem In dgStudent.Items
                chk = dgitem.Cells(0).Controls(1)
                chk.Checked = False
            Next

            Session("AddFee") = Nothing
            Session(ReceiptsClass.SessionStuChgMatricNo) = Nothing
            Session(ReceiptsClass.SessionStuChange) = Nothing
            dgFeeType.DataSource = Nothing
            dgFeeType.DataBind()
            dgView.DataSource = Nothing
            dgView.DataBind()
            txtTotalFeeAmt.Visible = False
            lblTotalFeeAmt.Visible = False
            chkFeeType.Checked = False
            chkFeeType.Visible = False
            lblMsg.Text = "No Student Selected, Fee Code Will Be Removed"
        End If
    End Sub
    Protected Sub ibtnRemoveFee_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)

        Dim dgitem As DataGridItem
        Dim i As Integer = 0

        Dim newListStu As New List(Of StudentEn)
        Dim currListSt As New List(Of StudentEn)
        If Not Session(ReceiptsClass.SessionStuChange) Is Nothing Then
            currListSt = Session(ReceiptsClass.SessionStuChange)
        End If
        ListTRD = New List(Of AccountsDetailsEn)
        If Not Session("AddFee") Is Nothing Then
            ListTRD = Session("AddFee")
        End If
        If dgFeeType.Visible = True Then
            If dgFeeType.SelectedIndex <> -1 Then
                Try
                    Dim ReferenceCode As String = dgFeeType.DataKeys(dgFeeType.SelectedIndex).ToString()
                    ListTRD.RemoveAll(Function(x) x.ReferenceCode = ReferenceCode)
                    currListSt.RemoveAll(Function(x) x.ReferenceCode = ReferenceCode)

                    If ListTRD.Count > 0 Then
                        dgFeeType.DataSource = ListTRD
                        dgFeeType.DataBind()
                    End If

                    If currListSt.Count > 0 Then
                        dgView.DataSource = currListSt
                        dgView.DataBind()
                    End If


                    If ListTRD.Count > 0 Then
                        chkFeeType.Checked = True
                        'enable checkbox
                        Dim chk As CheckBox
                        For Each dgitem In dgFeeType.Items
                            chk = dgitem.Cells(0).Controls(1)
                            chk.Checked = True
                        Next
                    Else
                        chkFeeType.Checked = False
                        chkFeeType.Visible = False
                        dgFeeType.DataSource = Nothing
                        dgFeeType.DataBind()
                        txtTotalFeeAmt.Visible = False
                        lblTotalFeeAmt.Visible = False
                    End If

                    If currListSt.Count > 0 Then
                        chkSelectedView.Checked = True
                        Dim chk As CheckBox
                        For Each dgitem In dgView.Items
                            chk = dgitem.Cells(0).Controls(1)
                            chk.Checked = True
                        Next
                    Else
                        chkSelectedView.Checked = False
                        chkSelectedView.Visible = False
                        dgView.DataSource = Nothing
                        dgView.DataBind()
                        txtTotal.Visible = False
                        lblTotal.Visible = False
                    End If

                    Session("AddFee") = ListTRD
                    Session(ReceiptsClass.SessionStuToSave) = currListSt
                    dgFeeType.SelectedIndex = -1
                Catch ex As Exception
                    LogError.Log("BatchInvoice", "ibtnRemoveFee_Click", ex.Message)
                End Try
            Else
                lblMsg.Visible = True
                lblMsg.Text = "Please Select a Feetype to Remove"

            End If
        End If

        If dgView.Visible = True Then
            If dgView.SelectedIndex <> -1 Then
                Try
                    Dim ReferenceCode As String = dgView.DataKeys(dgView.SelectedIndex).ToString()
                    Dim MatricNo As String = dgView.Items(dgView.SelectedIndex).Cells(dgViewCell.MatricNo).Text
                    Dim getselectedReferenCode As New List(Of StudentEn)
                    currListSt.RemoveAll(Function(x) x.ReferenceCode = ReferenceCode And x.MatricNo = MatricNo)
                    Dim updateCurrList As New AccountsDetailsEn
                    updateCurrList = ListTRD.Where(Function(x) x.ReferenceCode = ReferenceCode).FirstOrDefault()
                    getselectedReferenCode = currListSt.Where(Function(x) x.ReferenceCode = ReferenceCode).ToList()
                    If getselectedReferenCode.Count > 0 Then
                        updateCurrList.TransactionAmount = 0
                        updateCurrList.GSTAmount = 0
                        updateCurrList.TempAmount = 0
                        For Each obj In getselectedReferenCode
                            updateCurrList.TransactionAmount = updateCurrList.TransactionAmount + obj.TransactionAmount
                            updateCurrList.GSTAmount = updateCurrList.GSTAmount + obj.GSTAmount
                            updateCurrList.TempAmount = updateCurrList.TransactionAmount - updateCurrList.GSTAmount
                        Next
                        updateCurrList.StudentQty = getselectedReferenCode.Count
                    Else
                        ListTRD.RemoveAll(Function(x) x.ReferenceCode = ReferenceCode)
                    End If

                    If ListTRD.Count > 0 Then
                        dgFeeType.DataSource = ListTRD
                        dgFeeType.DataBind()
                    End If

                    If currListSt.Count > 0 Then
                        dgView.DataSource = currListSt
                        dgView.DataBind()
                    End If

                    If ListTRD.Count > 0 Then
                        chkFeeType.Checked = True
                        'enable checkbox
                        Dim chk As CheckBox
                        For Each dgitem In dgFeeType.Items
                            chk = dgitem.Cells(0).Controls(1)
                            chk.Checked = True
                        Next
                    Else
                        chkFeeType.Checked = False
                        chkFeeType.Visible = False
                        dgFeeType.DataSource = Nothing
                        dgFeeType.DataBind()
                        txtTotalFeeAmt.Visible = False
                        lblTotalFeeAmt.Visible = False
                    End If

                    If currListSt.Count > 0 Then
                        chkSelectedView.Checked = True
                        Dim chk As CheckBox
                        For Each dgitem In dgView.Items
                            chk = dgitem.Cells(0).Controls(1)
                            chk.Checked = True
                        Next
                    Else
                        chkSelectedView.Checked = False
                        chkSelectedView.Visible = False
                        dgView.DataSource = Nothing
                        dgView.DataBind()
                        txtTotal.Visible = False
                        lblTotal.Visible = False
                    End If

                    Session("AddFee") = ListTRD
                    Session(ReceiptsClass.SessionStuToSave) = currListSt
                    dgView.SelectedIndex = -1
                Catch ex As Exception
                    LogError.Log("BatchInvoice", "ibtnRemoveFee_Click", ex.Message)
                End Try
            Else
                lblMsg.Visible = True
                lblMsg.Text = "Please Select a Feetype to Remove"

            End If
        End If

        'If Request.QueryString("Formid") = "CN" Or Request.QueryString("Formid") = "DN" Then
        '    If Not Session(ReceiptsClass.SessionStuChange) Is Nothing Then
        '        currListSt = Session(ReceiptsClass.SessionStuChange)
        '    End If
        '    If Not currListSt Is Nothing Then
        '        Try
        '            currListSt.RemoveAt(dgView.SelectedIndex)
        '        Catch ex As Exception
        '            LogError.Log("BatchInvoice", "ibtnRemoveFee_Click", ex.Message)
        '        End Try
        '        dgView.DataSource = currListSt
        '        dgView.DataBind()
        '        Session(ReceiptsClass.SessionStuChange) = currListSt
        '        dgView.SelectedIndex = -1
        '    End If
        'Else
        '    For Each dgitem In dgView.Items
        '        dgitem.Cells(dgViewCell.Priority).Text = i
        '        i = i + 1
        '    Next

        '    If Not ListTRD Is Nothing Then
        '        If dgView.SelectedIndex <> -1 Then

        '            Try
        '                ListTRD.RemoveAt(CInt(dgView.SelectedItem.Cells(dgViewCell.Priority).Text))
        '            Catch ex As Exception
        '                LogError.Log("BatchInvoice", "ibtnRemoveFee_Click", ex.Message)
        '            End Try
        '            dgView.DataSource = ListTRD
        '            dgView.DataBind()
        '            If ListTRD.Count <> 0 Then
        '                Session("AddFee") = ListTRD
        '            Else
        '                Session("AddFee") = Nothing
        '            End If
        '            dgView.SelectedIndex = -1
        '        End If
        '    End If
        'End If

    End Sub

    Protected Sub ibtnOthers_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        ClearSession()
        LoadUserRights()
        OnSearchOthers()
    End Sub

#Region "Methods"
    ''' <summary>
    ''' Method to Fill the Intake DropDown
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub addIntake()
        Dim eIntake As New SemesterSetupEn
        Dim bIntake As New SemesterSetupBAL
        Dim listIntake As New List(Of SemesterSetupEn)
        ddlIntake.Items.Clear()
        ddlIntake.Items.Add(New ListItem("---Select---", "-1"))
        'ddlIntake.Items.Add(New ListItem("All", "1"))
        ddlIntake.DataTextField = "SemisterSetupCode"
        ddlIntake.DataValueField = "SemisterSetupCode"
        eIntake.SemisterSetupCode = "%"

        Try
            listIntake = bIntake.GetListSemesterCode(eIntake)
        Catch ex As Exception
            LogError.Log("BatchInvoice", "addIntake", ex.Message)
        End Try
        ddlIntake.DataSource = listIntake
        ddlIntake.DataBind()
        'Session("faculty") = listfac
    End Sub
    ''' <summary>
    ''' Method to Load Student Category Dropdown
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub LoadStudentCategory()
        Dim bStuCategory As New StudentCategoryBAL
        Dim eStuCategory As New StudentCategoryEn
        eStuCategory.StudentCategoryCode = ""
        eStuCategory.Description = ""
        eStuCategory.Status = True
        ddlStudentType.Items.Clear()
        Dim ListStuCat As New List(Of StudentCategoryEn)
        ddlStudentType.Items.Add(New ListItem("---Select---", "-1"))
        ddlStudentType.DataTextField = "Description"
        ddlStudentType.DataValueField = "StudentCategoryCode"
        Try
            ddlStudentType.DataSource = bStuCategory.GetStudentCategoryListAll(eStuCategory)
        Catch ex As Exception
            LogError.Log("BatchInvoice", "LoadStudentCategory", ex.Message)
        End Try

        ddlStudentType.DataBind()
    End Sub
    ''' <summary>
    ''' Method to Load Faculty Dropdown
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub LoadFaculty()
        Dim eFaculty As New FacultyEn
        Dim bFaculty As New FacultyBAL
        ddlFaculty.Items.Clear()
        ddlFaculty.Items.Add(New ListItem("---Select---", "-1"))
        ddlFaculty.DataTextField = "SAFC_Desc"
        ddlFaculty.DataValueField = "SAFC_Code"
        eFaculty.SAFC_Code = "%"
        Try
            ddlFaculty.DataSource = bFaculty.GetList(eFaculty)
        Catch ex As Exception
            LogError.Log("BatchInvoice", "LoadFaculty", ex.Message)
        End Try

        ddlFaculty.DataBind()
    End Sub
    ''' <summary>
    ''' Method to get the MenuName
    ''' </summary>
    ''' <param name="MenuId">Parameter is MenuId</param>
    ''' <remarks></remarks>
    Private Sub Menuname(ByVal MenuId As Integer)
        Dim eobj As New MenuEn
        Dim bobj As New MenuBAL
        eobj.MenuId = MenuId
        Try
            eobj = bobj.GetMenus(eobj)
        Catch ex As Exception
            LogError.Log("BatchInvoice", "Menuname", ex.Message)
        End Try
        lblMenuName.Text = eobj.MenuName
    End Sub
    ''' <summary>
    ''' Method to Load the Fields for Invoice Tab
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub LoadBatchInvoice()
        MultiView1.SetActiveView(View1)

        'imgLeft1.ImageUrl = "images/b_white_left.png"
        'imgRight1.ImageUrl = "images/b_white_right.png"
        btnBatchInvoice.CssClass = "TabButtonClick"


        'imgLeft2.ImageUrl = "images/b_orange_left.png"
        'imgRight2.ImageUrl = "images/b_orange_right.png"
        btnSelection.CssClass = "TabButton"

        'imgLeft3.ImageUrl = "images/b_orange_left.png"
        'imgRight3.ImageUrl = "images/b_orange_right.png"
        btnViewStu.CssClass = "TabButton"

        pnlBatch.Visible = True
        pnlSelection.Visible = False
        pnlView.Visible = False
    End Sub
    ''' <summary>
    ''' Method to load the Fields for View Students Tab
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub OnViewStudentGrid()
        MultiView1.SetActiveView(View3)

        'imgLeft3.ImageUrl = "images/b_white_left.png"
        'imgRight3.ImageUrl = "images/b_white_right.png"
        btnViewStu.CssClass = "TabButtonClick"


        'imgLeft1.ImageUrl = "images/b_orange_left.png"
        'imgRight1.ImageUrl = "images/b_orange_right.png"
        btnBatchInvoice.CssClass = "TabButton"

        'imgLeft2.ImageUrl = "images/b_orange_left.png"
        'imgRight2.ImageUrl = "images/b_orange_right.png"
        btnSelection.CssClass = "TabButton"
        If dgStudent.Items.Count > 0 Then
            chkStudent.Visible = True
        Else
            chkStudent.Visible = False
        End If

        pnlBatch.Visible = False
        pnlSelection.Visible = False
        pnlView.Visible = True
        pnlView.Visible = True
    End Sub


    ''' <summary>
    ''' Method to Change the Date Format
    ''' </summary>
    ''' <remarks>Date in ddd/mm/yyyy Format</remarks>
    Private Sub dates()
        'Dim GBFormat As System.Globalization.CultureInfo
        'GBFormat = New System.Globalization.CultureInfo("en-GB")
        txtBatchDate.Text = Format(Date.Now, "dd/MM/yyyy")
        txtInvoiceDate.Text = Format(Date.Now, "dd/MM/yyyy")
        txtDuedate.Text = Format(DateAdd(DateInterval.Day, 30, Date.Now), "dd/MM/yyyy")
    End Sub
    ''' <summary>
    ''' Method to Validate
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub SpaceValidation()
        ibtnSave.Attributes.Add("onCLick", "return Validate()")
        Dim GBFormat As System.Globalization.CultureInfo
        GBFormat = New System.Globalization.CultureInfo("en-GB")
        If Trim(txtDesc.Text).Length = 0 Then
            txtDesc.Text = Trim(txtDesc.Text)
            lblMsg.Visible = True
            lblMsg.Text = "Enter Valid Description "
            txtDesc.Focus()
            Exit Sub
        End If
        'Batch Intake
        If Trim(ddlIntake.SelectedValue).Length < 0 Then
            ddlIntake.SelectedValue = Trim(ddlIntake.SelectedValue)
            lblMsg.Visible = True
            lblMsg.Text = "Select Batch Intake"
            ddlIntake.Focus()
            Exit Sub
        End If
        'Batch date
        If Trim(txtBatchDate.Text).Length < 10 Then
            lblMsg.Visible = True
            lblMsg.Text = "Enter Valid Batch Date"
            txtBatchDate.Focus()
            Exit Sub
        Else
            Try
                txtBatchDate.Text = DateTime.Parse(txtBatchDate.Text, GBFormat)
            Catch ex As Exception
                lblMsg.Visible = True
                lblMsg.Text = "Enter Valid Batch Date"
                txtBatchDate.Focus()
                Exit Sub
            End Try
        End If
        'Invoice date
        If Trim(txtInvoiceDate.Text).Length < 10 Then
            lblMsg.Visible = True
            lblMsg.Text = "Enter Valid Invoice Date"
            txtInvoiceDate.Focus()
            Exit Sub
        Else
            Try
                txtInvoiceDate.Text = DateTime.Parse(txtInvoiceDate.Text, GBFormat)
            Catch ex As Exception
                lblMsg.Visible = True
                lblMsg.Text = "Enter Valid Invoice Date"
                txtInvoiceDate.Focus()
                Exit Sub
            End Try
        End If

        'Due date
        If Trim(txtDuedate.Text).Length < 10 Then
            lblMsg.Visible = True
            lblMsg.Text = "Enter Valid Due Date"
            txtDuedate.Focus()
            Exit Sub
        Else
            Try
                txtDuedate.Text = DateTime.Parse(txtDuedate.Text, GBFormat)
            Catch ex As Exception
                lblMsg.Visible = True
                lblMsg.Text = "Enter Valid Due Date"
                txtDuedate.Focus()
                Exit Sub
            End Try
        End If
        If lblStatus.Value = "Posted" Then
            lblMsg.Visible = True
            lblMsg.Text = "Record Already Posted"
            Exit Sub
        End If
    End Sub
    ''' <summary>
    ''' Method to Load DateFields
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub OnLoadItem()
        If Session("PageMode") = "Add" Then
            LoadBatchInvoice()
            'txtBatchNo.Text = "Auto Number"
            'txtBatchNo.ReadOnly = True
        End If
        'txtBatchDate.Text = Format(Date.Now, "dd/MM/yyyy")
        txtInvoiceDate.Text = Format(Date.Now, "dd/MM/yyyy")
        txtDuedate.Text = Format(DateAdd(DateInterval.Day, 30, Date.Now), "dd/MM/yyyy")
        txtBatchDate.ReadOnly = True
        txtInvoiceDate.ReadOnly = True
        txtDuedate.ReadOnly = True
    End Sub
    ''' <summary>
    ''' Method to Add FeeTypes
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub addFeeType()
        Dim eobjFt As FeeTypesEn
        Dim listFee As New List(Of FeeTypesEn)
        Dim eobjTRD As New AccountsDetailsEn
        Dim objStu As New StudentEn
        Dim newListStu As New List(Of StudentEn)
        Dim currListSt As New List(Of StudentEn)
        Dim ListObjectsStu As New List(Of StudentEn)
        Dim totalTransAmount As Double = 0
        Dim totalGSTAmount As Double = 0
        Dim totalActualFeeAmount As Double = 0
        Dim LocalStuCount As Integer = 0
        Dim InternationalStuCount As Integer = 0
        Dim TakeLocalFeeAmount As Boolean = False
        Dim existingFeeType As New AccountsDetailsEn

        Dim i As Integer = 0
        If Not Session(ReceiptsClass.SessionStuChange) Is Nothing Then
            currListSt = Session(ReceiptsClass.SessionStuChange)
        End If
        If Not Session(ReceiptsClass.SessionStuChgMatricNo) Is Nothing Then
            StuChgMatricNo = Session(ReceiptsClass.SessionStuChgMatricNo)
        End If
        If Not Session("AddFee") Is Nothing Then
            ListTRD = Session("AddFee")
        Else
            ListTRD = New List(Of AccountsDetailsEn)
        End If
        listFee = Session("eobj")
        newListStu.AddRange(currListSt)
        InternationalStuCount = StuChgMatricNo.Where(Function(x) (x.CategoryCode = ReceiptsClass.Student_BUKAN_WARGANEGARA Or x.CategoryCode = ReceiptsClass.student_International)).ToList().Count()
        LocalStuCount = StuChgMatricNo.Where(Function(x) (x.CategoryCode = ReceiptsClass.Student_WARGANEGARA Or x.CategoryCode = ReceiptsClass.student_Local)).ToList().Count()

        If InternationalStuCount > 0 And LocalStuCount > 0 Then
            TakeLocalFeeAmount = True
        End If

        If (Request.QueryString("Formid") = "CN" Or Request.QueryString("Formid") = "DN") And Not Session("Module") Is Nothing Then
            If listFee.Count <> 0 Then
                While i < listFee.Count
                    totalTransAmount = 0
                    totalGSTAmount = 0
                    eobjFt = listFee(i)
                    Dim j As Integer = 0
                    existingFeeType = ListTRD.Where(Function(x) x.ReferenceCode = eobjFt.FeeTypeCode).FirstOrDefault()
                    If Not currListSt.Any(Function(x) x.ReferenceCode = eobjFt.FeeTypeCode) Then
                        While j < StuChgMatricNo.Count
                            objStu = New StudentEn
                            objStu.MatricNo = StuChgMatricNo(j).MatricNo
                            objStu.StudentName = StuChgMatricNo(j).StudentName
                            objStu.ProgramID = StuChgMatricNo(j).ProgramID
                            objStu.CurrentSemester = StuChgMatricNo(j).CurrentSemester
                            objStu.ReferenceCode = eobjFt.FeeTypeCode
                            objStu.Description = eobjFt.Description
                            If TakeLocalFeeAmount = True Then
                                objStu.TempAmount = eobjFt.LocalAmount
                                objStu.GSTAmount = eobjFt.LocalGSTAmount
                            Else
                                If StuChgMatricNo(j).CategoryCode = ReceiptsClass.Student_BUKAN_WARGANEGARA Or StuChgMatricNo(j).CategoryCode = ReceiptsClass.student_International Then
                                    objStu.TempAmount = eobjFt.NonLocalAmount
                                    objStu.GSTAmount = eobjFt.NonLocalGSTAmount
                                Else
                                    objStu.TempAmount = eobjFt.LocalAmount
                                    objStu.GSTAmount = eobjFt.LocalGSTAmount
                                End If
                            End If

                            If Math.Abs(StuChgMatricNo(j).CrditHrDiff) > 0 Then
                                objStu.TransactionAmount = String.Format("{0:F}", objStu.TempAmount * Math.Abs(StuChgMatricNo(j).CrditHrDiff))
                                objStu.GSTAmount = String.Format("{0:F}", _GstSetupDal.GetGstAmount(eobjFt.TaxId, objStu.TransactionAmount))
                                objStu.TaxAmount = String.Format("{0:F}", objStu.GSTAmount)
                            Else
                                objStu.TransactionAmount = String.Format("{0:F}", objStu.TempAmount)
                                objStu.GSTAmount = String.Format("{0:F}", objStu.GSTAmount)
                                objStu.TaxAmount = String.Format("{0:F}", objStu.GSTAmount)
                            End If
                            totalTransAmount = totalTransAmount + objStu.TransactionAmount
                            totalGSTAmount = totalGSTAmount + objStu.GSTAmount
                            objStu.Priority = eobjFt.Priority
                            objStu.PostStatus = "Ready"
                            objStu.TransStatus = "Open"
                            objStu.TaxId = eobjFt.TaxId
                            objStu.Internal_Use = Session("Module").ToString() + ";" + StuChgMatricNo(j).CrditHrDiff.ToString()
                            objStu.AccountDetailsEn = eobjTRD
                            objStu.CrditHrDiff = StuChgMatricNo(j).CrditHrDiff
                            totalActualFeeAmount = totalTransAmount - totalGSTAmount
                            If Not ListTRD.Any(Function(x) x.ReferenceCode = eobjFt.FeeTypeCode) Then
                                ListTRD.Add(New AccountsDetailsEn With {.ReferenceCode = eobjFt.FeeTypeCode, .Description = eobjFt.Description, .TransactionAmount = totalTransAmount,
                                                                              .GSTAmount = totalGSTAmount, .TaxAmount = objStu.GSTAmount, .TempAmount = totalActualFeeAmount,
                                                                        .TempPaidAmount = objStu.TransactionAmount, .TaxId = objStu.TaxId, .StudentQty = 1, .Priority = objStu.Priority})
                            Else
                                Dim assignNewTotal As AccountsDetailsEn = ListTRD.Where(Function(x) x.ReferenceCode = eobjFt.FeeTypeCode).FirstOrDefault()
                                assignNewTotal.TransactionAmount = totalTransAmount
                                assignNewTotal.GSTAmount = totalGSTAmount
                                assignNewTotal.TempAmount = totalActualFeeAmount
                                assignNewTotal.StudentQty = assignNewTotal.StudentQty + 1
                            End If
                            newListStu.Add(objStu)
                            objStu = Nothing
                            j += 1
                        End While
                    Else
                        While j < StuChgMatricNo.Count
                            If Not currListSt.Any(Function(x) x.MatricNo = StuChgMatricNo(j).MatricNo AndAlso x.ReferenceCode = eobjFt.FeeTypeCode) Then
                                objStu = New StudentEn
                                objStu.MatricNo = StuChgMatricNo(j).MatricNo
                                objStu.StudentName = StuChgMatricNo(j).StudentName
                                objStu.ProgramID = StuChgMatricNo(j).ProgramID
                                objStu.CurrentSemester = StuChgMatricNo(j).CurrentSemester
                                objStu.ReferenceCode = eobjFt.FeeTypeCode
                                objStu.Description = eobjFt.Description
                                If existingFeeType.ReferenceCode <> Nothing Then
                                    objStu.TransactionAmount = existingFeeType.TempPaidAmount
                                    objStu.GSTAmount = existingFeeType.TaxAmount
                                    objStu.TaxAmount = existingFeeType.TaxAmount
                                Else
                                    If TakeLocalFeeAmount = True Then
                                        objStu.TempAmount = eobjFt.LocalAmount
                                        objStu.GSTAmount = eobjFt.LocalGSTAmount
                                    Else
                                        If StuChgMatricNo(j).CategoryCode = ReceiptsClass.Student_BUKAN_WARGANEGARA Or StuChgMatricNo(j).CategoryCode = ReceiptsClass.student_International Then
                                            objStu.TempAmount = eobjFt.NonLocalAmount
                                            objStu.GSTAmount = eobjFt.NonLocalGSTAmount
                                        Else
                                            objStu.TempAmount = eobjFt.LocalAmount
                                            objStu.GSTAmount = eobjFt.LocalGSTAmount
                                        End If
                                    End If
                                    If StuChgMatricNo(j).CrditHrDiff > 0 Then
                                        objStu.TransactionAmount = String.Format("{0:F}", objStu.TempAmount * Math.Abs(StuChgMatricNo(j).CrditHrDiff))
                                        objStu.GSTAmount = String.Format("{0:F}", _GstSetupDal.GetGstAmount(eobjFt.TaxId, objStu.TransactionAmount))
                                        objStu.TaxAmount = String.Format("{0:F}", objStu.GSTAmount)
                                    Else
                                        objStu.TransactionAmount = String.Format("{0:F}", objStu.TempAmount)
                                        objStu.GSTAmount = String.Format("{0:F}", objStu.GSTAmount)
                                        objStu.TaxAmount = String.Format("{0:F}", objStu.GSTAmount)
                                    End If
                                End If

                                totalTransAmount = totalTransAmount + objStu.TransactionAmount
                                totalGSTAmount = totalGSTAmount + objStu.GSTAmount
                                objStu.Priority = eobjFt.Priority
                                objStu.PostStatus = "Ready"
                                objStu.TransStatus = "Open"
                                objStu.TaxId = eobjFt.TaxId
                                totalActualFeeAmount = totalTransAmount - totalGSTAmount
                                If Not ListTRD.Any(Function(x) x.ReferenceCode = eobjFt.FeeTypeCode) Then
                                    ListTRD.Add(New AccountsDetailsEn With {.ReferenceCode = eobjFt.FeeTypeCode, .Description = eobjFt.Description, .TransactionAmount = totalTransAmount,
                                                                                  .GSTAmount = totalGSTAmount, .TaxAmount = objStu.GSTAmount, .TempAmount = totalActualFeeAmount,
                                                                             .TempPaidAmount = objStu.TransactionAmount, .TaxId = objStu.TaxId, .StudentQty = 1, .Priority = objStu.Priority})
                                Else
                                    Dim assignNewTotal As AccountsDetailsEn = ListTRD.Where(Function(x) x.ReferenceCode = eobjFt.FeeTypeCode).FirstOrDefault()
                                    assignNewTotal.TransactionAmount = totalTransAmount
                                    assignNewTotal.GSTAmount = totalGSTAmount
                                    assignNewTotal.TempAmount = totalActualFeeAmount
                                    assignNewTotal.StudentQty = assignNewTotal.StudentQty + 1
                                End If
                                newListStu.Add(objStu)
                                objStu = Nothing

                            End If
                            j += 1
                        End While
                    End If
                    i = i + 1
                End While
            End If
            'ListTRD.AddRange(newListStu.Select(Function(x) New AccountsDetailsEn With {.ReferenceCode = x.ReferenceCode, .Description = x.Description, .TransactionAmount = x.TransactionAmount,
            '                                                                        .GSTAmount = x.GSTAmount, .TaxAmount = x.TaxAmount, .Priority = x.Priority, .PostStatus = "Ready", .TransStatus = "Open", .TaxId = x.TaxId}))


            Session("AddFee") = ListTRD
            dgFeeType.DataSource = ListTRD
            dgFeeType.DataBind()
            Session(ReceiptsClass.SessionStuChange) = newListStu
            newListStu = newListStu.OrderBy(Function(x) x.MatricNo).ToList()
            dgView.DataSource = newListStu
            dgView.DataBind()
            Session("eobj") = Nothing
            StuChgMatricNo = New List(Of StudentEn)

        Else
            If listFee.Count <> 0 Then
                For Each eobjFt In listFee
                    existingFeeType = ListTRD.Where(Function(x) x.ReferenceCode = eobjFt.FeeTypeCode).FirstOrDefault()
                    totalTransAmount = 0
                    totalGSTAmount = 0
                    If Not currListSt.Any(Function(x) x.ReferenceCode = eobjFt.FeeTypeCode) Then
                        For Each stu In StuChgMatricNo
                            objStu = New StudentEn
                            objStu.MatricNo = stu.MatricNo
                            objStu.StudentName = stu.StudentName
                            'objStu.ProgramID = stu.ProgramID
                            'objStu.CurrentSemester = stu.CurrentSemester
                            objStu.ReferenceCode = eobjFt.FeeTypeCode
                            objStu.Description = eobjFt.Description
                            If TakeLocalFeeAmount = True Then
                                objStu.TransactionAmount = String.Format("{0:F}", eobjFt.LocalAmount)
                                objStu.GSTAmount = String.Format("{0:F}", eobjFt.LocalGSTAmount)
                                objStu.TaxAmount = String.Format("{0:F}", eobjFt.LocalGSTAmount)
                            Else
                                If stu.CategoryCode = ReceiptsClass.Student_BUKAN_WARGANEGARA Or stu.CategoryCode = ReceiptsClass.student_International _
                                    Or hfStdCategory.Value = ReceiptsClass.Student_BUKAN_WARGANEGARA Or hfStdCategory.Value = ReceiptsClass.student_International Then
                                    objStu.TransactionAmount = String.Format("{0:F}", eobjFt.NonLocalAmount)
                                    objStu.GSTAmount = String.Format("{0:F}", eobjFt.NonLocalGSTAmount)
                                    objStu.TaxAmount = String.Format("{0:F}", eobjFt.NonLocalGSTAmount)
                                Else
                                    objStu.TransactionAmount = String.Format("{0:F}", eobjFt.LocalAmount)
                                    objStu.GSTAmount = String.Format("{0:F}", eobjFt.LocalGSTAmount)
                                    objStu.TaxAmount = String.Format("{0:F}", eobjFt.LocalGSTAmount)
                                End If
                            End If

                            totalTransAmount = totalTransAmount + objStu.TransactionAmount
                            totalGSTAmount = totalGSTAmount + objStu.GSTAmount
                            objStu.Priority = eobjFt.Priority
                            objStu.PostStatus = "Ready"
                            objStu.TransStatus = "Open"
                            objStu.TaxId = eobjFt.TaxId
                            totalActualFeeAmount = totalTransAmount - totalGSTAmount
                            If Not ListTRD.Any(Function(x) x.ReferenceCode = eobjFt.FeeTypeCode) Then
                                ListTRD.Add(New AccountsDetailsEn With {.ReferenceCode = eobjFt.FeeTypeCode, .Description = eobjFt.Description, .TransactionAmount = totalTransAmount,
                                                                              .GSTAmount = totalGSTAmount, .TaxAmount = objStu.GSTAmount, .TempAmount = totalActualFeeAmount,
                                                                        .TempPaidAmount = objStu.TransactionAmount, .TaxId = objStu.TaxId, .StudentQty = 1, .Priority = objStu.Priority})
                            Else
                                Dim assignNewTotal As AccountsDetailsEn = ListTRD.Where(Function(x) x.ReferenceCode = eobjFt.FeeTypeCode).FirstOrDefault()
                                assignNewTotal.TransactionAmount = totalTransAmount
                                assignNewTotal.GSTAmount = totalGSTAmount
                                assignNewTotal.TempAmount = totalActualFeeAmount
                                assignNewTotal.StudentQty = assignNewTotal.StudentQty + 1
                            End If
                            newListStu.Add(objStu)
                            objStu = Nothing
                        Next
                    Else
                        For Each stu In StuChgMatricNo
                            If Not currListSt.Any(Function(x) x.ReferenceCode = eobjFt.FeeTypeCode And x.MatricNo = stu.MatricNo) Then
                                objStu = New StudentEn
                                objStu.MatricNo = stu.MatricNo
                                objStu.StudentName = stu.StudentName
                                'objStu.ProgramID = stu.ProgramID
                                'objStu.CurrentSemester = stu.CurrentSemester
                                objStu.ReferenceCode = eobjFt.FeeTypeCode
                                objStu.Description = eobjFt.Description
                                If existingFeeType.ReferenceCode <> Nothing Then
                                    objStu.TransactionAmount = existingFeeType.TempPaidAmount
                                    objStu.GSTAmount = existingFeeType.TaxAmount
                                    objStu.TaxAmount = existingFeeType.TaxAmount
                                Else
                                    If TakeLocalFeeAmount = True Then
                                        objStu.TransactionAmount = String.Format("{0:F}", eobjFt.LocalAmount)
                                        objStu.GSTAmount = String.Format("{0:F}", eobjFt.LocalGSTAmount)
                                        objStu.TaxAmount = String.Format("{0:F}", eobjFt.LocalGSTAmount)
                                    Else
                                        If stu.CategoryCode = ReceiptsClass.Student_BUKAN_WARGANEGARA Or stu.CategoryCode = ReceiptsClass.student_International Then
                                            objStu.TransactionAmount = String.Format("{0:F}", eobjFt.NonLocalAmount)
                                            objStu.GSTAmount = String.Format("{0:F}", eobjFt.NonLocalGSTAmount)
                                            objStu.TaxAmount = String.Format("{0:F}", eobjFt.NonLocalGSTAmount)
                                        Else
                                            objStu.TransactionAmount = String.Format("{0:F}", eobjFt.LocalAmount)
                                            objStu.GSTAmount = String.Format("{0:F}", eobjFt.LocalGSTAmount)
                                            objStu.TaxAmount = String.Format("{0:F}", eobjFt.LocalGSTAmount)
                                        End If
                                    End If

                                End If

                                totalTransAmount = totalTransAmount + objStu.TransactionAmount
                                totalGSTAmount = totalGSTAmount + objStu.GSTAmount
                                objStu.Priority = eobjFt.Priority
                                objStu.PostStatus = "Ready"
                                objStu.TransStatus = "Open"
                                objStu.TaxId = eobjFt.TaxId
                                totalActualFeeAmount = totalTransAmount - totalGSTAmount
                                If Not ListTRD.Any(Function(x) x.ReferenceCode = eobjFt.FeeTypeCode) Then
                                    ListTRD.Add(New AccountsDetailsEn With {.ReferenceCode = eobjFt.FeeTypeCode, .Description = eobjFt.Description, .TransactionAmount = totalTransAmount,
                                                                                  .GSTAmount = totalGSTAmount, .TaxAmount = objStu.GSTAmount, .TempAmount = totalActualFeeAmount,
                                                                            .TempPaidAmount = objStu.TransactionAmount, .TaxId = objStu.TaxId, .StudentQty = 1, .Priority = objStu.Priority})
                                Else
                                    Dim assignNewTotal As AccountsDetailsEn = ListTRD.Where(Function(x) x.ReferenceCode = eobjFt.FeeTypeCode).FirstOrDefault()
                                    assignNewTotal.TransactionAmount = assignNewTotal.TransactionAmount + objStu.TransactionAmount
                                    assignNewTotal.GSTAmount = assignNewTotal.GSTAmount + objStu.GSTAmount
                                    assignNewTotal.TempAmount = assignNewTotal.TransactionAmount - assignNewTotal.GSTAmount
                                    assignNewTotal.StudentQty = assignNewTotal.StudentQty + 1
                                End If
                                newListStu.Add(objStu)
                                objStu = Nothing
                            End If
                        Next
                    End If
                Next
            End If
            Session("AddFee") = ListTRD
            dgFeeType.DataSource = ListTRD
            dgFeeType.DataBind()
            Session(ReceiptsClass.SessionStuChange) = newListStu
            newListStu = newListStu.OrderBy(Function(x) x.MatricNo).ToList()
            AddStudColumnDgView()
            dgView.DataSource = newListStu
            dgView.DataBind()
            Session("eobj") = Nothing

            StuChgMatricNo = New List(Of StudentEn)
        End If
        If dgView.Items.Count > 0 Then
            Dim chk As CheckBox
            Dim dgitem As DataGridItem
            chkSelectedView.Checked = True
            If chkSelectedView.Checked = True Then
                For Each dgitem In dgView.Items
                    chk = dgitem.Cells(0).Controls(1)
                    chk.Checked = True
                Next
            End If
        End If
        If dgFeeType.Items.Count > 0 Then
            Dim chk As CheckBox
            Dim dgitem As DataGridItem
            chkFeeType.Checked = True
            chkFeeType.Visible = True
            For Each dgitem In dgFeeType.Items
                chk = dgitem.Cells(0).Controls(1)
                chk.Checked = True
            Next
            txtTotalFeeAmt.Visible = True
            lblTotalFeeAmt.Visible = True
        Else
            txtTotalFeeAmt.Visible = False
            lblTotalFeeAmt.Visible = False
            chkFeeType.Visible = False
        End If
        If Not Session("Module") Is Nothing Then
            pnlDgFeeType.Visible = False
            pnlDgView.Visible = True
        Else
            pnlDgFeeType.Visible = True
            pnlDgView.Visible = False
        End If
    End Sub

    Private Sub addStuToExistingFee(ByVal stuList As List(Of StudentEn))
        Dim objStu As New StudentEn
        Dim newListStu As New List(Of StudentEn)
        Dim currListSt As New List(Of StudentEn)

        If Not Session(ReceiptsClass.SessionStuChange) Is Nothing Then
            currListSt = Session(ReceiptsClass.SessionStuChange)
        End If

        If Not Session("AddFee") Is Nothing Then
            ListTRD = Session("AddFee")
        Else
            ListTRD = New List(Of AccountsDetailsEn)
        End If

        newListStu.AddRange(currListSt)
        If ListTRD.Count <> 0 Then
            For Each eobjFt In ListTRD
                For Each stu In stuList
                    If Not currListSt.Any(Function(x) x.ReferenceCode = eobjFt.ReferenceCode And x.MatricNo = stu.MatricNo) Then
                        objStu = New StudentEn
                        objStu.MatricNo = stu.MatricNo
                        objStu.StudentName = stu.StudentName
                        objStu.ReferenceCode = eobjFt.ReferenceCode
                        objStu.Description = eobjFt.Description
                        objStu.TransactionAmount = eobjFt.TempPaidAmount
                        objStu.GSTAmount = eobjFt.TaxAmount
                        objStu.TaxAmount = eobjFt.TaxAmount
                        objStu.Priority = eobjFt.Priority
                        objStu.PostStatus = "Ready"
                        objStu.TransStatus = "Open"
                        objStu.TaxId = eobjFt.TaxId

                        Dim assignNewTotal As AccountsDetailsEn = ListTRD.Where(Function(x) x.ReferenceCode = eobjFt.ReferenceCode).FirstOrDefault()
                        assignNewTotal.TransactionAmount = assignNewTotal.TransactionAmount + objStu.TransactionAmount
                        assignNewTotal.GSTAmount = assignNewTotal.GSTAmount + objStu.GSTAmount
                        assignNewTotal.TempAmount = assignNewTotal.TransactionAmount - assignNewTotal.GSTAmount
                        assignNewTotal.StudentQty = assignNewTotal.StudentQty + 1

                        newListStu.Add(objStu)
                        objStu = Nothing
                    End If
                Next
            Next
        End If
        Session("AddFee") = ListTRD
        dgFeeType.DataSource = ListTRD
        dgFeeType.DataBind()
        Session(ReceiptsClass.SessionStuChange) = newListStu
        newListStu = newListStu.OrderBy(Function(x) x.MatricNo).ToList()
        AddStudColumnDgView()
        dgView.DataSource = newListStu
        dgView.DataBind()
        If dgView.Items.Count > 0 Then
            Dim chk As CheckBox
            Dim dgitem As DataGridItem
            chkSelectedView.Checked = True
            If chkSelectedView.Checked = True Then
                For Each dgitem In dgView.Items
                    chk = dgitem.Cells(0).Controls(1)
                    chk.Checked = True
                Next
            End If
        End If
        If dgFeeType.Items.Count > 0 Then
            Dim chk As CheckBox
            Dim dgitem As DataGridItem
            chkFeeType.Checked = True
            chkFeeType.Visible = True
            For Each dgitem In dgFeeType.Items
                chk = dgitem.Cells(0).Controls(1)
                chk.Checked = True
            Next
            txtTotalFeeAmt.Visible = True
            lblTotalFeeAmt.Visible = True
        Else
            txtTotalFeeAmt.Visible = False
            lblTotalFeeAmt.Visible = False
            chkFeeType.Visible = False
        End If
    End Sub


    ''' <summary>
    ''' Method to Add the List of Students
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub addStudent()
        Dim ListObjectsStu As List(Of StudentEn)
        Dim eobj As StudentEn
        Dim mylist As List(Of StudentEn)
        Dim i As Integer = 0

        If Not Session("LstStueObj") Is Nothing Then
            ListObjectsStu = Session("LstStueObj")
        Else
            ListObjectsStu = New List(Of StudentEn)
        End If
        Dim recentAddCount As Integer = ListObjectsStu.Count

        mylist = Session("liststu")
        'Checking for the Exisiting Students in the Grid
        If mylist.Count <> 0 Then
            While i < mylist.Count
                eobj = mylist(i)
                Dim j As Integer = 0
                Dim Flag As Boolean = False
                While j < ListObjectsStu.Count
                    If ListObjectsStu(j).MatricNo = eobj.MatricNo Then
                        Flag = True
                        Exit While
                    End If
                    j = j + 1
                End While
                If Flag = False Then
                    ListObjectsStu.Add(eobj)
                End If
                i = i + 1
            End While
        End If
        If ListObjectsStu.Count = 0 Then
            chkStudent.Visible = False
            chkStudent.Checked = False
        Else
            chkStudent.Visible = True
            chkStudent.Checked = True
        End If
        Session("LstStueObj") = ListObjectsStu
        'Session(ReceiptsClass.SessionStuChgMatricNo) = ListObjectsStu
        dgStudent.DataSource = ListObjectsStu
        dgStudent.DataBind()

        ' hfStudentCount.Value = ListObjectsStu.Count
        If recentAddCount <> ListObjectsStu.Count And recentAddCount <> 0 Then
            ''reflect the existing fee code to recent added student
            If Session("Module") Is Nothing Then
                addStuToExistingFee(mylist)
            End If
        End If
        Session("liststu") = Nothing
    End Sub
    ''' <summary>
    ''' Method to get the List Of Transactions
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub LoadListObjects()
        Dim eob As New AccountsEn
        Dim obj As New AccountsBAL
        Dim Mylist As New List(Of AccountsEn)

        If Session("loaddata") = "others" Then
            If Request.QueryString("Formid") = "Inv" Then
                eob.TransType = "Credit"
                eob.Category = "Invoice"
            ElseIf Request.QueryString("Formid") = "CN" Then
                eob.TransType = "Debit"
                eob.Category = "Credit Note"
            ElseIf Request.QueryString("Formid") = "DN" Then
                eob.TransType = "Credit"
                eob.Category = "Debit Note"
            End If
            If txtBatchNo.Text <> "Auto Number" Then
                eob.BatchCode = txtBatchNo.Text
            Else
                eob.BatchCode = ""
            End If

            If ddlIntake.SelectedIndex = 0 Then
                eob.BatchIntake = ""
            Else
                eob.BatchIntake = ddlIntake.SelectedValue
            End If

            If txtBatchDate.Text <> "" Then
                eob.BatchDate = Date.ParseExact(txtBatchDate.Text, "dd/MM/yyyy", CultureInfo.InvariantCulture)
            End If

            eob.PostStatus = "Posted"
            eob.SubType = "Student"
            Try
                ListObjects = obj.GetTransactions(eob)
            Catch ex As Exception
                LogError.Log("BatchInvoice", "LoadListObjects", ex.Message)
            End Try

        ElseIf Session("loaddata") = "View" Then
            If Request.QueryString("Formid") = "Inv" Then
                eob.TransType = "Credit"
                eob.Category = "Invoice"
            ElseIf Request.QueryString("Formid") = "CN" Then
                eob.TransType = "Debit"
                eob.Category = "Credit Note"
            ElseIf Request.QueryString("Formid") = "DN" Then
                eob.TransType = "Credit"
                eob.Category = "Debit Note"
            End If
            If txtBatchNo.Text <> "Auto Number" Then
                eob.BatchCode = txtBatchNo.Text
            Else
                eob.BatchCode = ""
            End If

            If ddlIntake.SelectedIndex = 0 Then
                eob.BatchIntake = ""
            Else
                eob.BatchIntake = ddlIntake.SelectedValue
            End If

            If txtBatchDate.Text <> "" Then
                eob.BatchDate = Date.ParseExact(txtBatchDate.Text, "dd/MM/yyyy", CultureInfo.InvariantCulture)
            End If
            eob.PostStatus = "Ready"
            eob.SubType = "Student"

            Try
                ListObjects = obj.GetTransactions(eob)
            Catch ex As Exception
                LogError.Log("BatchInvoice", "LoadListObjects", ex.Message)
            End Try

        End If
        Session("loaddata") = Nothing

        'Removing duplicate batchid object from the list
        Dim i As Integer = 0
        If Not ListObjects Is Nothing Then
            While i < ListObjects.Count
                Dim j As Integer = 0
                Dim objcount As Boolean = False
                While j < Mylist.Count
                    If Mylist(j).BatchCode = ListObjects(i).BatchCode Then
                        objcount = True
                        Exit While
                    End If
                    j = j + 1
                End While
                If objcount = False Then
                    Mylist.Add(ListObjects(i))
                End If
                i = i + 1
            End While
        End If
        ListObjects = Nothing
        ListObjects = Mylist
        Session("ListObj") = ListObjects

        lblCount.Text = ListObjects.Count.ToString()
        If ListObjects.Count <> 0 Then
            Session("BatchNo") = Nothing
            txtBatchNo.Enabled = True
            Session("BatchNo") = txtBatchNo.Text
            txtBatchNo.Enabled = False
            'Enable Navigation
            DisableRecordNavigator()
            txtRecNo.Text = "1"
            If lblStatus.Value <> "Posted" Then
                OnMoveFirst()
            End If
            If Session("EditFlag") = True Then
                lblMsg.Visible = True
                txtBatchNo.Enabled = False
                ibtnSave.Enabled = True
                ibtnSave.ImageUrl = "images/save.png"
                trPrint.Visible = True
            Else
                Session("PageMode") = ""
                ibtnSave.Enabled = False
                ibtnSave.ImageUrl = "images/gsave.png"
            End If
        Else
            txtRecNo.Text = ""
            lblCount.Text = ""
            'Clear the fields
            onClearData()
            'Date Formatting
            dates()

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
    ''' Method to Fill the Field Values
    ''' </summary>
    ''' <param name="RecNo"></param>
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
        If txtRecNo.Text = 0 Then
            txtRecNo.Text = 1
        Else
            If lblCount.Text = 0 Then
                txtRecNo.Text = 0
            Else
                Dim obj As AccountsEn
                Dim ListTranctionDetails As New List(Of AccountsDetailsEn)
                Dim eobjTransDatails As AccountsDetailsEn
                Dim objProcess As New AccountsDetailsBAL

                Dim m_no As String = Nothing

                ListObjects = Session("ListObj")
                obj = ListObjects(RecNo)
                txtBatchNo.Text = obj.BatchCode
                txtBatchNo.ReadOnly = True
                If obj.BatchIntake <> "" Or obj.BatchIntake IsNot Nothing Then
                    ddlIntake.SelectedValue = obj.BatchIntake
                Else
                    ddlIntake.SelectedIndex = -1
                End If

                'added by Hafiz @ 22/3/2016
                'assign matric no to object for stud ledger used - start
                If Not Session("MatricNo") Is Nothing Then
                    m_no = Session("MatricNo")
                Else
                    m_no = ""
                End If
                'assign matric no to object for stud ledger used - end

                'ddlIntake.Enabled = False
                txtDesc.Text = obj.Description
                txtBatchDate.Text = obj.BatchDate
                txtInvoiceDate.Text = obj.TransDate
                txtDuedate.Text = obj.DueDate
                txtTotal.Text = obj.TransactionAmount
                eobjTransDatails = New AccountsDetailsEn
                eobjTransDatails.TransactionID = obj.TranssactionID

                'Get selection criteria - start
                Dim objSelectionCriteria As New SelectionCriteriaBAL
                Dim enSelectionCriteria As New SelectionCriteriaEn
                enSelectionCriteria.BatchCode = txtBatchNo.Text
                Dim getSelection = objSelectionCriteria.GetSC(enSelectionCriteria)
                'clear session - start
                Session(ReceiptsClass.SessionscHostel) = Nothing
                Session(ReceiptsClass.SessionscProgram) = Nothing
                Session(ReceiptsClass.SessionscSponsor) = Nothing
                chkSelectProgram.Checked = False
                chkSelectSponsor.Checked = False
                chkSelectHostel.Checked = False
                'clear session - end
                If getSelection.SAFC_Code <> "" Then
                    scFaculty = getSelection.SAFC_Code
                    ddlFaculty.SelectedValue = scFaculty
                    Session("FC_Code") = ddlFaculty.SelectedValue
                    programGrid()
                Else
                    ddlFaculty.SelectedValue = "-1"
                End If
                If getSelection.SAPG_Code <> "" Then
                    scProgram.AddRange(Split(getSelection.SAPG_Code, ","))
                    Session(ReceiptsClass.SessionscProgram) = scProgram
                    FillProgramCheckbox()
                Else
                    programGrid()
                    clearAllProgram()
                End If
                If getSelection.SASR_Code <> "" Then
                    scSponsor.AddRange(Split(getSelection.SASR_Code, ","))
                    Session(ReceiptsClass.SessionscSponsor) = scSponsor
                    SponsorGrid()
                    FillSponsorCheckbox()
                Else
                    SponsorGrid()
                    ClearAllSponsor()
                End If
                If getSelection.SAKO_Code <> "" Then
                    scHostel.AddRange(Split(getSelection.SAKO_Code, ","))
                    Session(ReceiptsClass.SessionscHostel) = scHostel
                    HostelGrid()
                    FillHostelCheckbox()
                Else
                    HostelGrid()
                    clearAllHostel()
                End If
                If getSelection.SASC_Code <> "" Then
                    ddlStudentType.SelectedValue = getSelection.SASC_Code
                Else
                    ddlStudentType.SelectedValue = "-1"
                End If
                'Add hiddenCategory
                hfStdCategory.Value = ddlStudentType.SelectedValue

                If getSelection.Sem <> "" Then
                    Dim getnewSem As New List(Of String)
                    getnewSem.AddRange(Split(getSelection.Sem, ","))
                    If getnewSem.Count > 0 Then
                        If getnewSem.Contains("individual") Then
                            rbSemIndividual.Checked = True
                            rbSemAll.Checked = False
                            txtSemster.Text = getnewSem.Item(1).ToString
                        Else
                            rbSemIndividual.Checked = False
                            rbSemAll.Checked = True
                            txtSemster.Text = String.Empty
                        End If
                    End If
                Else
                    rbSemIndividual.Checked = False
                    rbSemAll.Checked = False
                    txtSemster.Text = String.Empty
                End If
                'Get selection criteria - end
                Dim ListStuChange As New List(Of StudentEn)
                If obj.PostStatus = "Ready" Then
                    Try
                        'If Request.QueryString("Formid") = "CN" Or Request.QueryString("Formid") = "DN" Then
                        ListTranctionDetails = objProcess.GetStudentAccountsDetailsByBatchCode(obj, m_no)
                        AddStudColumnDgView()
                        ListStuChange.AddRange(ListTranctionDetails.Select(Function(x) New StudentEn With {.MatricNo = x.MatricNo,
                                                                                                           .StudentName = x.StudentName,
                                                                                                           .CurrentSemester = x.CurrentSemester,
                                                                                                           .TransactionAmount = x.TransactionAmount,
                                                                                                           .TaxAmount = x.TaxAmount, .GSTAmount = x.GSTAmount,
                                                                                                          .TaxId = x.TaxId, .ReferenceCode = x.ReferenceCode,
                                                                                                          .Description = x.Description, .Priority = x.Priority, .Internal_Use = x.Internal_Use}).Distinct().ToList())
                        If ListStuChange.Count <> 0 Then
                            Dim i As Integer = 0
                            While i < ListStuChange.Count
                                If ListStuChange(i).Internal_Use <> Nothing Then
                                    Dim getinternal_use As String() = ListStuChange(i).Internal_Use.Split(";")
                                    Session("Module") = getinternal_use(0)
                                    'btnViewStu.Enabled = False
                                    If getinternal_use(1) <> Nothing Then
                                        ListStuChange(i).CrditHrDiff = getinternal_use(1)
                                    End If
                                End If
                                i = i + 1
                            End While
                            'If Not Session("Module") Is Nothing Then
                            '    AddStudColumnDgView()
                            'End If
                        End If
                        Dim listNewStu As New List(Of StudentEn)
                        If Not Session("Module") Is Nothing Then
                            For Each i In ListTranctionDetails
                                If Not listNewStu.Any(Function(x) x.MatricNo = i.MatricNo) Then
                                    Dim getinternal_use As String() = i.Internal_Use.Split(";")
                                    listNewStu.Add(New StudentEn With {.MatricNo = i.MatricNo, .StudentName = i.StudentName, .ProgramID = i.ProgramID,
                                                                                                     .TaxId = i.TaxId, .CrditHrDiff = getinternal_use(1)})
                                End If
                            Next
                        Else
                            listNewStu = ListStuChange.GroupBy(Function(x) x.MatricNo).Select(Function(y) y.First()).ToList()

                        End If
                        Session(ReceiptsClass.SessionStuChange) = ListStuChange
                        Session(ReceiptsClass.SessionStuChgMatricNo) = listNewStu
                        hfStudentCount.Value = listNewStu.Count


                        ''GROUPING FEE CODE - START
                        If ListTranctionDetails.Count > 0 Then
                            Dim totalTransAmount As Double = 0
                            Dim totalGSTAmount As Double = 0
                            Dim GSTAmt As Double = 0
                            Dim totalActualFeeAmount As Double = 0

                            ListTRD = New List(Of AccountsDetailsEn)
                            For Each stu In ListTranctionDetails
                                If Not ListTRD.Any(Function(x) x.ReferenceCode = stu.ReferenceCode) Then
                                    totalGSTAmount = totalGSTAmount + stu.GSTAmount
                                    totalTransAmount = totalTransAmount + stu.TransactionAmount
                                    totalActualFeeAmount = totalTransAmount - totalGSTAmount
                                    ListTRD.Add(New AccountsDetailsEn With {.ReferenceCode = stu.ReferenceCode, .Description = stu.Description, .TransactionAmount = totalTransAmount,
                                                                              .GSTAmount = totalGSTAmount, .TaxAmount = stu.GSTAmount, .TempAmount = totalActualFeeAmount,
                                                                            .TempPaidAmount = stu.TransactionAmount, .TaxId = stu.TaxId, .StudentQty = 1, .Priority = stu.Priority})
                                Else
                                    Dim assignNewTotal As AccountsDetailsEn = ListTRD.Where(Function(x) x.ReferenceCode = stu.ReferenceCode).FirstOrDefault()
                                    assignNewTotal.TransactionAmount = assignNewTotal.TransactionAmount + stu.TransactionAmount
                                    assignNewTotal.GSTAmount = assignNewTotal.GSTAmount + stu.GSTAmount
                                    assignNewTotal.TempAmount = assignNewTotal.TransactionAmount - assignNewTotal.GSTAmount
                                    assignNewTotal.StudentQty = assignNewTotal.StudentQty + 1
                                End If
                                totalGSTAmount = 0
                                totalTransAmount = 0
                            Next
                            Session("AddFee") = ListTRD
                            dgFeeType.DataSource = ListTRD
                            dgFeeType.DataBind()
                        End If
                        ''GROUPING FEE CODE - END

                        'Else
                        '    ListTranctionDetails = objProcess.GetStudentAccountsDetailsWithTaxAmount(eobjTransDatails)
                        'End If

                    Catch ex As Exception
                        LogError.Log("BatchInvoice", "FillData", ex.Message)
                    End Try

                    dgView.DataSource = ListTranctionDetails
                    dgView.DataBind()
                    'Dim chk As CheckBox
                    'Dim dgitem As DataGridItem
                    'If dgView.Items.Count > 0 Then
                    '    txtTotal.Visible = True
                    '    lblTotal.Visible = True
                    '    chkSelectedView.Checked = True
                    '    If chkSelectedView.Checked = True Then
                    '        For Each dgitem In dgView.Items
                    '            chk = dgitem.Cells(0).Controls(1)
                    '            chk.Checked = True
                    '        Next
                    '    End If
                    'End If
                    'Session("AddFee") = ListTranctionDetails
                    Dim mylst As New List(Of StudentEn)
                    Dim bsobj As New AccountsBAL
                    Dim loen As New StudentEn
                    loen.BatchCode = txtBatchNo.Text

                    Try
                        mylst = bsobj.GetListStudentbyBatchID(loen)
                    Catch ex As Exception
                        LogError.Log("BatchInvoice", "FillData", ex.Message)
                    End Try

                    '                  List<Person> distinctPeople = allPeople
                    '                  .GroupBy(p >= p.PersonId)
                    '                  .Select(g >= g.First())
                    '.ToList();

                    Dim distinctMyLst As New List(Of StudentEn)
                    distinctMyLst = mylst.GroupBy(Function(x) x.MatricNo).Select(Function(y) y.First()).ToList()
                    dgStudent.DataSource = distinctMyLst
                    dgStudent.DataBind()


                    ibtnStatus.ImageUrl = "images/Ready.gif"
                    lblStatus.Value = "Ready"
                    Session("LstStueObj") = distinctMyLst

                    'Changing Status
                    If obj.PostStatus = "Ready" Then
                        lblStatus.Value = "Ready"
                        ibtnStatus.ImageUrl = "images/Ready.gif"
                        DisablePrint()
                    End If
                    If obj.PostStatus = "Posted" Then
                        lblStatus.Value = "Posted"
                        ibtnStatus.ImageUrl = "images/Posted.gif"
                    End If

                    'Enable checkbox

                    Dim chk As CheckBox
                    Dim dgitem As DataGridItem

                    For Each dgitem In dgStudent.Items
                        chk = dgitem.Cells(0).Controls(1)
                        chk.Checked = True
                    Next

                ElseIf obj.PostStatus = "Posted" Then
                    Try
                        ' If Request.QueryString("Formid") = "CN" Or Request.QueryString("Formid") = "DN" Then
                        ListTranctionDetails = objProcess.GetStudentAccountsDetailsByBatchCode(obj, m_no)
                        AddStudColumnDgView()
                        ListStuChange.AddRange(ListTranctionDetails.Select(Function(x) New StudentEn With {.MatricNo = x.MatricNo,
                                                                                                        .StudentName = x.StudentName,
                                                                                                        .CurrentSemester = x.CurrentSemester,
                                                                                                        .TransactionAmount = x.TransactionAmount,
                                                                                                        .TaxAmount = x.TaxAmount, .GSTAmount = x.GSTAmount,
                                                                                                       .TaxId = x.TaxId, .ReferenceCode = x.ReferenceCode,
                                                                                                       .Description = x.Description, .Priority = x.Priority, .Internal_Use = x.Internal_Use}).Distinct.ToList())
                        If ListStuChange.Count <> 0 Then
                            Dim i As Integer = 0
                            While i < ListStuChange.Count
                                If ListStuChange(i).Internal_Use <> Nothing Then
                                    Dim getinternal_use As String() = ListStuChange(i).Internal_Use.Split(";")
                                    Session("Module") = getinternal_use(0)
                                    'btnViewStu.Enabled = False
                                    If getinternal_use(1) <> Nothing Then
                                        ListStuChange(i).CrditHrDiff = getinternal_use(1)
                                    End If
                                End If
                                i = i + 1
                            End While
                        End If

                        Dim listNewStu As New List(Of StudentEn)
                        If Not Session("Module") Is Nothing Then
                            For Each i In ListTranctionDetails
                                If Not listNewStu.Any(Function(x) x.MatricNo = i.MatricNo) Then
                                    Dim getinternal_use As String() = i.Internal_Use.Split(";")
                                    listNewStu.Add(New StudentEn With {.MatricNo = i.MatricNo, .StudentName = i.StudentName, .ProgramID = i.ProgramID,
                                                                                                     .TaxId = i.TaxId, .CrditHrDiff = getinternal_use(1)})
                                End If
                            Next
                        Else
                            listNewStu = ListStuChange.GroupBy(Function(x) x.MatricNo).Select(Function(y) y.First()).ToList()

                        End If
                        Session(ReceiptsClass.SessionStuChange) = ListStuChange
                        Session(ReceiptsClass.SessionStuChgMatricNo) = listNewStu
                        hfStudentCount.Value = listNewStu.Count

                        ''GROUPING FEE CODE - START
                        If ListTranctionDetails.Count > 0 Then
                            Dim totalTransAmount As Double = 0
                            Dim totalGSTAmount As Double = 0
                            Dim GSTAmt As Double = 0
                            Dim totalActualFeeAmount As Double = 0

                            ListTRD = New List(Of AccountsDetailsEn)
                            For Each stu In ListTranctionDetails
                                If Not ListTRD.Any(Function(x) x.ReferenceCode = stu.ReferenceCode) Then
                                    totalGSTAmount = totalGSTAmount + stu.GSTAmount
                                    totalTransAmount = totalTransAmount + stu.TransactionAmount
                                    totalActualFeeAmount = totalTransAmount - totalGSTAmount
                                    ListTRD.Add(New AccountsDetailsEn With {.ReferenceCode = stu.ReferenceCode, .Description = stu.Description, .TransactionAmount = totalTransAmount,
                                                                              .GSTAmount = totalGSTAmount, .TaxAmount = stu.GSTAmount, .TempAmount = totalActualFeeAmount,
                                                                            .StudentQty = 1, .TempPaidAmount = stu.TransactionAmount, .TaxId = stu.TaxId, .Priority = stu.Priority})
                                Else
                                    Dim assignNewTotal As AccountsDetailsEn = ListTRD.Where(Function(x) x.ReferenceCode = stu.ReferenceCode).FirstOrDefault()
                                    assignNewTotal.TransactionAmount = assignNewTotal.TransactionAmount + stu.TransactionAmount
                                    assignNewTotal.GSTAmount = assignNewTotal.GSTAmount + stu.GSTAmount
                                    assignNewTotal.TempAmount = assignNewTotal.TransactionAmount - assignNewTotal.GSTAmount
                                    assignNewTotal.StudentQty = assignNewTotal.StudentQty + 1
                                End If
                                totalGSTAmount = 0
                                totalTransAmount = 0
                            Next
                            Session("AddFee") = ListTRD
                            dgFeeType.DataSource = ListTRD
                            dgFeeType.DataBind()
                        End If
                        ''GROUPING FEE CODE - END

                        'Else
                        'ListTranctionDetails = objProcess.GetStudentAccountsDetailsWithTaxAmount(eobjTransDatails)
                        'End If
                    Catch ex As Exception
                        LogError.Log("BatchInvoice", "FillData", ex.Message)
                    End Try

                    dgView.DataSource = ListTranctionDetails
                    dgView.DataBind()

                    'If dgView.Items.Count >= 0 Then
                    '    txtTotal.Visible = True
                    '    lblTotal.Visible = True
                    'End If

                    'Session("AddFee") = ListTranctionDetails
                    Dim mylst As New List(Of StudentEn)
                    Dim bsobj As New AccountsBAL
                    Dim loen As New StudentEn
                    loen.BatchCode = txtBatchNo.Text
                    Try
                        mylst = bsobj.GetListStudentbyBatchID(loen)
                    Catch ex As Exception
                        LogError.Log("BatchInvoice", "FillData", ex.Message)
                    End Try

                    Dim distinctMyLst As New List(Of StudentEn)
                    distinctMyLst = mylst.GroupBy(Function(x) x.MatricNo).Select(Function(y) y.First()).ToList()
                    dgStudent.DataSource = distinctMyLst
                    dgStudent.DataBind()
                    'dgStudent.DataSource = mylst
                    'dgStudent.DataBind()

                    Session("LstStueObj") = distinctMyLst

                    'Changing Status
                    lblStatus.Value = "Posted"
                    ibtnStatus.ImageUrl = "images/Posted.gif"
                    If lblStatus.Value = "Posted" Then
                        PrintAble()
                    End If
                    ''Enable checkbox
                    'Dim chk As CheckBox
                    'Dim dgitem As DataGridItem
                    'If dgStudent.Items.Count > 0 Then
                    '    chkStudent.Checked = True
                    '    For Each dgitem In dgStudent.Items
                    '        chk = dgitem.Cells(0).Controls(1)
                    '        chk.Checked = True
                    '    Next
                    'End If

                End If

                CheckWorkflowStatus(obj)

            End If
        End If
        'If Not Session("Module") Is Nothing Then
        If dgView.Items.Count > 0 Then
            Dim chk As CheckBox
            Dim dgitem As DataGridItem
            chkSelectedView.Checked = True
            If chkSelectedView.Checked = True Then
                For Each dgitem In dgView.Items
                    chk = dgitem.Cells(0).Controls(1)
                    chk.Checked = True
                Next
            End If
        End If
        'End If
        If dgFeeType.Items.Count > 0 Then
            Dim chk As CheckBox
            Dim dgitem As DataGridItem
            chkFeeType.Visible = True
            chkFeeType.Checked = True
            For Each dgitem In dgFeeType.Items
                chk = dgitem.Cells(0).Controls(1)
                chk.Checked = True
            Next
            txtTotalFeeAmt.Visible = True
            lblTotalFeeAmt.Visible = True
        Else
            txtTotalFeeAmt.Visible = False
            lblTotalFeeAmt.Visible = False
            chkFeeType.Visible = False
            chkFeeType.Checked = False
        End If

        If Not Session("Module") Is Nothing Then
            If dgStudent.Items.Count > 0 Then
                chkStudent.Enabled = False
                Dim chk As CheckBox
                Dim dgitem As DataGridItem
                chkStudent.Checked = True
                For Each dgitem In dgStudent.Items
                    chk = dgitem.Cells(0).Controls(1)
                    chk.Enabled = False
                Next
            End If
            pnlDgView.Visible = True
            pnlDgFeeType.Visible = False
        Else
            If dgStudent.Items.Count > 0 Then
                chkStudent.Enabled = True
                Dim chk As CheckBox
                Dim dgitem As DataGridItem
                chkStudent.Checked = True
                For Each dgitem In dgStudent.Items
                    chk = dgitem.Cells(0).Controls(1)
                    chk.Enabled = True
                Next
            End If
            pnlDgView.Visible = False
            pnlDgFeeType.Visible = True
        End If

        setDateFormat()
        Session("MatricNo") = Nothing
        hfValidateAmt.Value = False
    End Sub
    ''' <summary>
    ''' Method To Change the Date Format(dd/MM/yyyy)
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub setDateFormat()
        Dim myInvoiceDate As Date = CDate(CStr(txtInvoiceDate.Text))
        Dim myFormat As String = "dd/MM/yyyy"
        Dim myFormattedDate As String = Format(myInvoiceDate, myFormat)
        txtInvoiceDate.Text = myFormattedDate
        Dim myDuedate As Date = CDate(CStr(txtDuedate.Text))
        Dim myFormattedDate1 As String = Format(myDuedate, myFormat)
        txtDuedate.Text = myFormattedDate1
        Dim myBatchDate As Date = CDate(CStr(txtBatchDate.Text))
        Dim myFormattedDate2 As String = Format(myBatchDate, myFormat)
        txtBatchDate.Text = myFormattedDate2
    End Sub
    ''' <summary>
    ''' Method to Delete the Transactions
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub ondelete()
        lblMsg.Visible = True
        Dim RecAff As Boolean
        Dim bsobj As New AccountsBAL
        Dim eob As New AccountsEn
        If Trim(txtBatchNo.Text).Length <> 0 Then
            If lblCount.Text = "" Then lblCount.Text = 0
            If lblCount.Text > 0 Then
                Try
                    eob.BatchCode = Trim(txtBatchNo.Text)
                    RecAff = bsobj.BatchDelete(eob)
                    lblMsg.Visible = True
                    ErrorDescription = "Record Deleted Successfully "
                    lblMsg.Text = ErrorDescription
                    DFlag = "Delete"
                    Session("loaddata") = "View"
                    txtBatchNo.Text = ""
                    txtBatchDate.Text = ""
                    txtInvoiceDate.Text = ""
                    txtDesc.Text = ""
                    txtDuedate.Text = ""
                    dgView.DataSource = Nothing
                    dgView.DataBind()
                    dgStudent.DataSource = Nothing
                    dgStudent.DataBind()
                    LoadListObjects()
                Catch ex As Exception
                    lblMsg.Text = ex.Message.ToString()
                    LogError.Log("BatchInvoice", "ondelete", ex.Message)
                End Try
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
    ''' Method to Add all the FeeAmounts in Grid
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub AddTotal()
        Dim sumValue As Double
        Dim dgi As DataGridItem
        Dim txt As TextBox
        For Each dgi In dgView.Items
            txt = dgi.Cells(dgViewCell.Fee_Amount).Controls(1)
            sumValue = sumValue + CDbl(txt.Text)
            txt.Text = String.Format("{0:F}", CDbl(txt.Text))
        Next
        txtTotal.Text = String.Format("{0:F}", sumValue)
        dgView.Columns(dgViewCell.Fee_Amount).FooterText = String.Format("{0:F}", sumValue)
    End Sub
    ''' <summary>
    ''' Method to Create List of Students
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub CreateListObjStudent()
        Dim eobjstu As StudentEn

        Dim chk As CheckBox
        Dim dgitem As DataGridItem
        listStu = Nothing
        listStu = New List(Of StudentEn)
        For Each dgitem In dgStudent.Items
            chk = dgitem.Cells(0).Controls(1)

            If chk.Checked = True Then
                eobjstu = New StudentEn
                eobjstu.MatricNo = dgitem.Cells(1).Text
                eobjstu.StudentName = dgitem.Cells(2).Text
                eobjstu.ProgramID = dgitem.Cells(3).Text
                eobjstu.CurrentSemester = dgitem.Cells(4).Text
                listStu.Add(eobjstu)
                eobjstu = Nothing
            End If
        Next
        If listStu.Count <> 0 Then
            Session("lstStu") = listStu
        Else
            Session("lstStu") = Nothing
        End If
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
            LogError.Log("BatchInvoice", "LoadUserRights", ex.Message)
        End Try

        'Rights for Add
        If eobj.IsAdd = True Then
            'onAdd()
            ibtnNew.ImageUrl = "~/images/add.png"
            ibtnNew.Enabled = True
        Else
            ibtnNew.ImageUrl = "~/images/gadd.png"
            ibtnNew.Enabled = False
            ibtnNew.ToolTip = "Access Denied"

        End If
        'Rights for Edit
        If eobj.IsEdit = True Then
            ibtnSave.Enabled = True
            ibtnSave.ImageUrl = "~/images/save.png"
            ibtnSave.ToolTip = "Edit"
            If eobj.IsAdd = False Then
                ibtnSave.Enabled = False
                ibtnSave.ImageUrl = "~/images/gsave.png"
                ibtnSave.ToolTip = "Access Denied"
            End If

            Session("EditFlag") = True

        Else
            Session("EditFlag") = False
            ibtnSave.Enabled = False
            ibtnSave.ImageUrl = "~/images/gsave.png"
        End If
        'Rights for View
        ibtnView.Enabled = eobj.IsView
        If eobj.IsView = True Then
            ibtnView.ImageUrl = "~/images/ready.png"
            ibtnView.Enabled = True
        Else
            ibtnView.ImageUrl = "~/images/ready.png"
            ibtnView.Enabled = True
            'ibtnView.ToolTip = "Access Denied"
        End If
        'Rights for Delete
        If eobj.IsDelete = True Then
            ibtnDelete.ImageUrl = "~/images/delete.png"
            ibtnDelete.Enabled = True
        Else
            ibtnDelete.ImageUrl = "~/images/gdelete.png"
            ibtnDelete.ToolTip = "Access Denied"
            ibtnDelete.Enabled = False
        End If
        'Rights for Print      
        hfIsPrint.Value = eobj.IsPrint
        'ibtnPrint.Enabled = eobj.IsPrint
        'If eobj.IsPrint = True Then
        '    ibtnPrint.Enabled = True
        '    ibtnPrint.ImageUrl = "~/images/print.png"
        '    ibtnPrint.ToolTip = "Print"
        'Else
        '    ibtnPrint.Enabled = False
        '    ibtnPrint.ImageUrl = "~/images/gprint.png"
        '    ibtnPrint.ToolTip = "Access Denied"
        'End If

        If eobj.IsOthers = True Then
            ibtnOthers.Enabled = True
            ibtnOthers.ImageUrl = "~/images/post.png"
            ibtnOthers.ToolTip = "Others"
        Else
            ibtnOthers.Enabled = False
            ibtnOthers.ImageUrl = "~/images/post.png"
            ibtnOthers.ToolTip = "Access Denied"
        End If
        If eobj.IsPost = True Then
            ibtnPosting.Enabled = True
            ibtnPosting.ImageUrl = "~/images/posting.png"
            ibtnPosting.ToolTip = "Posting"
        Else
            ibtnPosting.Enabled = False
            ibtnPosting.ImageUrl = "~/images/gposting.png"
            ibtnPosting.ToolTip = "Access Denied"
        End If
    End Sub

    Private Sub PrintAble()
        Dim IsPrint As Boolean = hfIsPrint.Value
        ibtnPrint.Enabled = IsPrint
        If IsPrint = True Then
            ibtnPrint.Enabled = True
            ibtnPrint.ImageUrl = "~/images/print.png"
            ibtnPrint.ToolTip = "Print"
        Else
            DisablePrint()
        End If
    End Sub

    Private Sub DisablePrint()
        ibtnPrint.Enabled = False
        ibtnPrint.ImageUrl = "~/images/gprint.png"
        ibtnPrint.ToolTip = "Access Denied"
    End Sub
    ''' <summary>
    ''' Method to Save and Update Invoices
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub onSave()
        Dim eobj As New AccountsEn
        Dim bsobj As New AccountsBAL
        Dim LstTRDetails As New List(Of AccountsDetailsEn)
        Dim listStu As New List(Of StudentEn)
        Dim bsobjStudent As New StudentBAL
        Dim Status As String

        lblMsg.Text = ""
        lblMsg.Visible = True

        If Request.QueryString("Formid") = "Inv" Then
            eobj.Category = "Invoice"
            Status = "O"
            eobj.TransType = "Debit"
        ElseIf Request.QueryString("Formid") = "DN" Then
            eobj.Category = "Debit Note"
            Status = "O"
            eobj.TransType = "Debit"
        ElseIf Request.QueryString("Formid") = "CN" Then
            eobj.Category = "Credit Note"
            Status = "O"
            eobj.TransType = "Credit"
        End If

        If dgView.Items.Count <> 0 Then

            ''update student list - start 
            'If (Request.QueryString("Formid") = "CN" Or Request.QueryString("Formid") = "DN") And Not Session("Module") Is Nothing Then
            '    'CreateListObjStudentChanges()
            '    'CreateListObjStuToSave()
            '    If Not Session(ReceiptsClass.SessionStuToSave) Is Nothing Then
            '        listStu = Session(ReceiptsClass.SessionStuToSave)
            '    Else
            '        listStu = New List(Of StudentEn)
            '    End If

            'Else
            '    If Not Session("lstStu") Is Nothing Then
            '        listStu = Session("lstStu")
            '    Else
            '        listStu = New List(Of StudentEn)
            '    End If
            'End If
            ''update student list - end 
            If Not Session(ReceiptsClass.SessionStuToSave) Is Nothing Then
                listStu = Session(ReceiptsClass.SessionStuToSave)
            Else
                listStu = New List(Of StudentEn)
            End If
            If txtBatchDate.Text = "" Then
                eobj.BatchDate = Format(Date.Now, "dd/MM/yyyy")
            Else
                eobj.BatchDate = Trim(txtBatchDate.Text)
            End If
            eobj.Description = Trim(txtDesc.Text)
            eobj.TransDate = Trim(txtInvoiceDate.Text)
            eobj.DueDate = Trim(txtDuedate.Text)
            eobj.SubType = "Student"
            eobj.TransStatus = "Open"
            eobj.PostedDateTime = DateTime.Now
            eobj.UpdatedTime = DateTime.Now
            eobj.ChequeDate = DateTime.Now
            eobj.CreatedBy = Session("User")
            eobj.CreatedDateTime = DateTime.Now
            eobj.PostStatus = "Ready"
            'eobj.BatchCode = Trim(txtBatchNo.Text)
            eobj.TransactionAmount = Trim(txtTotal.Text)
            eobj.BatchIntake = Trim(ddlIntake.SelectedValue)
            eobj.AccountDetailsList = Session("AddFee")

            'Added by Zoya @15/04/2016
            Dim dgitem As DataGridItem
            Dim chkBox As CheckBox
            Dim txt As TextBox

            For Each dgitem In dgFeeType.Items
                chkBox = dgitem.Cells(0).Controls(1)
                If chkBox.Checked = True Then
                    txt = dgitem.Cells(dgFeeTypeCell.Unit_Amount).Controls(1)
                    If txt.Text = 0 Then
                        lblMsg.Visible = True
                        lblMsg.Text = "Amount Cannot be Zero"
                        Exit Sub
                    End If
                End If
            Next

            For Each dgitem In dgView.Items
                chkBox = dgitem.Cells(0).Controls(1)
                If chkBox.Checked = True Then
                    txt = dgitem.Cells(dgViewCell.Fee_Amount).Controls(1)
                    If txt.Text = 0 Then
                        lblMsg.Visible = True
                        lblMsg.Text = "Amount Cannot be Zero"
                        Exit Sub
                    End If
                End If
            Next
            'Done Added by Zoya @15/04/2016

            eobj.UpdatedBy = Session("User")

            Dim bid As String = ""

            'Selection criteria - start
            Dim newSCFaculty = ddlFaculty.SelectedValue
            scProgram = If((Not Session(ReceiptsClass.SessionscProgram) Is Nothing), Session(ReceiptsClass.SessionscProgram), scProgram)
            scSponsor = If((Not Session(ReceiptsClass.SessionscSponsor) Is Nothing), Session(ReceiptsClass.SessionscSponsor), scSponsor)
            scHostel = If((Not Session(ReceiptsClass.SessionscHostel) Is Nothing), Session(ReceiptsClass.SessionscHostel), scHostel)
            Dim newSCProgram = If((scProgram.Count > 0), String.Join(",", scProgram), "")
            Dim newSCSponsor = If((scSponsor.Count > 0), String.Join(",", scSponsor), "")
            Dim newSCHostel = If((scHostel.Count > 0), String.Join(",", scHostel), "")
            scSemester = If((rbSemAll.Checked = True), "all", If((rbSemIndividual.Checked = True), "individual, " + txtSemster.Text, ""))
            scStudentCategory = ddlStudentType.SelectedValue
            'Selection criteria - end
            Dim newsc As New SelectionCriteriaEn
            newsc.SAFC_Code = newSCFaculty
            If rbProYes.Checked Then newsc.SAPG_Code = newSCProgram
            If rbSemYes.Checked Then newsc.SASR_Code = newSCSponsor
            If rbHosYes.Checked Then newsc.SAKO_Code = newSCHostel
            newsc.SASC_Code = scStudentCategory
            newsc.Sem = scSemester
            'Saving
            If Session("PageMode") = "Add" Then
                Try
                    txtBatchNo.ReadOnly = False
                    ''check student batch insert - start
                    'If (eobj.Category = "Credit Note" Or eobj.Category = "Debit Note") Then
                    '    CreateListObjTrackingNotes(eobj.Category)

                    '    Dim trackid As Integer = 0
                    '    Dim internal_use As String = ""
                    '    If Session("Module") = "ChangeProgram" Then
                    '        trackid = 1
                    '    ElseIf Session("Module") = "ChangeCdtHour" Then
                    '        trackid = 2
                    '    ElseIf Session("Module") = "ChangeHostel" Then
                    '        trackid = 3
                    '    End If

                    '    Dim trackingnote As New List(Of StudentEn)
                    '    trackingnote = Session("lstStu")
                    '    'no record for AccountDetailsList 
                    '    txtBatchNo.Text = bsobj.StudentBatchInsert(eobj, listStu, newsc, True, trackingnote, trackid)

                    'Else
                    '    txtBatchNo.Text = bsobj.StudentBatchInsert(eobj, listStu, newsc)
                    'End If
                    ''check student batch insert - end
                    CreateListObjTrackingNotes(eobj.Category)
                    Dim trackid As Integer = 0
                    Dim internal_use As String = ""
                    If Session("Module") = "ChangeProgram" Then
                        trackid = 1
                    ElseIf Session("Module") = "ChangeCdtHour" Then
                        trackid = 2
                    ElseIf Session("Module") = "ChangeHostel" Then
                        trackid = 3
                    End If

                    Dim trackingnote As New List(Of StudentEn)
                    trackingnote = Session("lstStu")
                    'no record for AccountDetailsList 
                    txtBatchNo.Text = bsobj.StudentBatchInsert(eobj, listStu, newsc, True, trackingnote, trackid)

                    ErrorDescription = "Record Saved Successfully "

                    'Remove StudentList for Prog Changes/Cdt Hour Changes/Withdrawn from Hostel
                    'If btnViewStu.Enabled = False Then
                    '    bsobjStudent.DeleteStudentChanges(listStu, eobj.Category, Convert.ToInt32(lblModule.Value))
                    'End If

                    ' Update the Student OutStanding Table 

                    ibtnStatus.ImageUrl = "images/ready.gif"
                    lblStatus.Value = "Ready"
                    lblMsg.Text = ErrorDescription
                    ibtnSave.Enabled = False

                Catch ex As Exception
                    lblMsg.Text = ex.Message.ToString()
                    LogError.Log("BatchInvoice", "onSave", ex.Message)
                End Try
                ' Updating
            ElseIf Session("PageMode") = "Edit" Then
                Try

                    eobj.BatchCode = txtBatchNo.Text
                    'If (eobj.Category = "Credit Note" Or eobj.Category = "Debit Note") Then
                    CreateListObjTrackingNotes(eobj.Category)
                    Dim trackid As Integer = 0
                    If Session("Module") = "ChangeProgram" Then
                        trackid = 1
                    ElseIf Session("Module") = "ChangeCdtHour" Then
                        trackid = 2
                    ElseIf Session("Module") = "ChangeHostel" Then
                        trackid = 3
                    End If
                    'no record for AccountDetailsList 
                    txtBatchNo.Text = bsobj.StudentBatchUpdateEditMode(eobj, listStu, newsc, True)

                    'Else
                    '    txtBatchNo.Text = bsobj.StudentBatchUpdateEditMode(eobj, listStu, newsc)
                    'End If


                    'If eobj.Category = "Invoice" Then
                    '    eobj.BatchCode = txtBatchNo.Text
                    '    txtBatchNo.Text = bsobj.StudentBatchUpdateEditMode(eobj, listStu)
                    'Else
                    '    txtBatchNo.Text = bsobj.StudentBatchUpdate(eobj, listStu)
                    'End If

                    ErrorDescription = "Record Updated Successfully "
                    ' Update the Student OutStanding Table 

                    ibtnStatus.ImageUrl = "images/ready.gif"
                    lblStatus.Value = "Ready"
                    lblMsg.Text = ErrorDescription

                Catch ex As Exception
                    lblMsg.Text = ex.Message.ToString()
                    LogError.Log("BatchInvoice", "onSave", ex.Message)
                End Try

            End If
        Else
            lblMsg.Text = "Please Enter At least one Feetype"
        End If
    End Sub
    ''' <summary>
    ''' Method to Load the Programs in Grid
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub programGrid()
        Dim obj As New ProgramInfoBAL
        Dim ds As New ProgramInfoEn
        Dim FC_code As String
        Dim list As New List(Of ProgramInfoEn)
        If Session("FC_Code") = "" Then
            FC_code = ""
        Else
            FC_code = Session("FC_Code")
        End If
        ds.SAFC_Code = FC_code

        Try
            list = obj.GetList(ds)
        Catch ex As Exception
            LogError.Log("BatchInvoice", "programGrid", ex.Message)
        End Try
        dgProgram.DataSource = list
        dgProgram.DataBind()

    End Sub
    ''' <summary>
    ''' Method to load the Sponsors in Grid
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub SponsorGrid()
        Dim obj As New SponsorBAL
        Dim list As New List(Of SponsorEn)
        Dim eob As New SponsorEn

        Try
            list = obj.GetList(eob)
        Catch ex As Exception
            LogError.Log("BatchInvoice", "SponsorGrid", ex.Message)
        End Try
        DgSponsor.DataSource = list
        DgSponsor.DataBind()
    End Sub
    ''' <summary>
    ''' Method to load the HostelGrid
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub HostelGrid()
        Dim obj As New KolejBAL
        Dim list As New List(Of KolejEn)
        Dim eob As New KolejEn

        Try
            list = obj.GetList(eob)
        Catch ex As Exception
            LogError.Log("BatchInvoice", "HostelGrid", ex.Message)
        End Try
        dgHostel.DataSource = list
        dgHostel.DataBind()
    End Sub
    ''' <summary>
    ''' Method to Load all the Grids in Selection Panel
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub selection()
        programGrid()
        SponsorGrid()
        HostelGrid()
    End Sub
    ''' <summary>
    ''' Method to Get Selected Programs from ProgramGrid
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub inprogram()
        Dim chkBox As CheckBox
        Dim prgStr As String = ""
        Dim pgCode As String
        For Each dgi As DataGridItem In dgProgram.Items
            chkBox = dgi.Cells(0).Controls(1)
            If chkBox.Checked = True Then

                If prgStr = "" Then
                    pgCode = dgi.Cells(1).Text & ""
                    prgStr = prgStr & "'" & pgCode & "'"
                Else
                    pgCode = dgi.Cells(1).Text
                    prgStr = prgStr & ",'" & pgCode & "'"
                End If

            End If
        Next
        Session("prgstr") = prgStr
    End Sub
    ''' <summary>
    ''' Method to Get Selected Sponsor from SponsorGrid
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub inSponsor()
        Dim chkBox As CheckBox
        Dim sponStr As String = ""
        Dim SPnCode As String
        For Each dgi As DataGridItem In DgSponsor.Items
            chkBox = dgi.Cells(0).Controls(1)
            If chkBox.Checked = True Then
                If sponStr = "" Then
                    SPnCode = dgi.Cells(1).Text & ""
                    sponStr = sponStr & "'" & SPnCode & "'"
                Else
                    SPnCode = dgi.Cells(1).Text
                    sponStr = sponStr & ",'" & SPnCode & "'"
                End If
            End If
        Next
        Session("spnstr") = sponStr
    End Sub
    ''' <summary>
    ''' Method to Get Selected Hostel from HostelGrid
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub inHOstel()
        Dim chkBox As CheckBox
        Dim HsStr As String = ""
        Dim HsCode As String
        For Each dgi As DataGridItem In dgHostel.Items
            chkBox = dgi.Cells(0).Controls(1)
            If chkBox.Checked = True Then
                If HsStr = "" Then
                    HsCode = dgi.Cells(1).Text & ""
                    HsStr = HsStr & "'" & HsCode & "'"
                Else
                    HsCode = dgi.Cells(1).Text
                    HsStr = HsStr & ",'" & HsCode & "'"
                End If
            End If
        Next
        Session("sstr") = HsStr
    End Sub
    ''' <summary>
    ''' Method to Clear the Field Values
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub onClearData()
        Session("ListObj") = Nothing
        DisableRecordNavigator()
        Session("AddFee") = Nothing
        txtBatchNo.Text = ""
        txtBatchNo.Enabled = True
        txtBatchNo.ReadOnly = False
        txtBatchDate.Text = Format(Date.Now, "dd/MM/yyyy")
        txtInvoiceDate.Text = Format(Date.Now, "dd/MM/yyyy")
        txtDuedate.Text = Format(DateAdd(DateInterval.Day, 30, Date.Now), "dd/MM/yyyy")
        ibtnStatus.ImageUrl = "images/notready.gif"
        lblStatus.Value = "New"
        txtDesc.Text = ""
        Session("lstStu") = Nothing
        Session("liststu") = Nothing
        Session("LstStueObj") = Nothing
        dgStudent.DataSource = Nothing
        dgStudent.DataBind()
        dgView.DataSource = Nothing
        dgView.DataBind()
        dgFeeType.DataSource = Nothing
        dgFeeType.DataBind()
        ddlIntake.SelectedValue = "-1"
        ddlFaculty.SelectedValue = "-1"
        ddlStudentType.SelectedValue = "-1"
        hfStdCategory.Value = "-1"
        dgProgram.DataSource = Nothing
        dgProgram.DataBind()
        rbProYes.Checked = False
        rbSemYes.Checked = False
        rbHosYes.Checked = False
        rbProNo.Checked = True
        rbSemNo.Checked = True
        rbHosNo.Checked = True
        chkSelectProgram.Checked = False
        chkSelectSponsor.Checked = False
        chkSelectHostel.Checked = False
        chkFeeType.Visible = False
        chkFeeType.Checked = False
        'If dgView.Items.Count <= 0 Then
        '    txtTotal.Visible = False
        '    lblTotal.Visible = False
        'End If
        pnlDgView.Visible = False
        pnlDgFeeType.Visible = False
        If dgFeeType.Items.Count > 0 Then
            txtTotalFeeAmt.Visible = True
            lblTotalFeeAmt.Visible = True
        Else
            txtTotalFeeAmt.Visible = False
            lblTotalFeeAmt.Visible = False
        End If
        LoadBatchInvoice()
        'clear session - start
        Session(ReceiptsClass.SessionscHostel) = New List(Of String)
        Session(ReceiptsClass.SessionscProgram) = New List(Of String)
        Session(ReceiptsClass.SessionscSponsor) = New List(Of String)
        clearAllHostel()
        clearAllProgram()
        ClearAllSponsor()
        'clear session - end
    End Sub
    ''' <summary>
    ''' Method to Load Fields in New Mode
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub onAdd()
        Session("PageMode") = "Add"
        Session("AddFee") = Nothing
        ibtnSave.Enabled = True
        lblMsg.Text = ""
        today.Value = Now.Date
        today.Value = Format(CDate(today.Value), "dd/MM/yyyy")
        ibtnStatus.ImageUrl = "images/notready.gif"
        lblStatus.Value = "New"
        ibtnSave.ImageUrl = "images/save.png"
        'onClearData()
        If ibtnNew.Enabled = False Then
            ibtnSave.Enabled = False
            ibtnSave.ImageUrl = "images/gsave.png"
            ibtnSave.ToolTip = "Access Denied"
        End If
        OnLoadItem()
        LoadBatchInvoice()
    End Sub

    ''' <summary>
    ''' Method to Post Invoices
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub OnPost()
        Dim eobj As New AccountsEn
        Dim bsobj As New AccountsBAL
        Dim LstTRDetails As New List(Of AccountsDetailsEn)
        Dim listStu As New List(Of StudentEn)
        Dim Status As String
        Dim bid As String = ""
        lblMsg.Text = ""
        lblMsg.Visible = True

        If Request.QueryString("Formid") = "Inv" Then
            eobj.Category = "Invoice"
            Status = "O"
            eobj.TransType = "Credit"
        ElseIf Request.QueryString("Formid") = "DN" Then
            eobj.Category = "Debit Note"
            Status = "O"
            eobj.TransType = "Credit"
        ElseIf Request.QueryString("Formid") = "CN" Then
            eobj.Category = "Credit Note"
            Status = "O"
            eobj.TransType = "Debit"
        End If

        If Request.QueryString("Formid") = "Inv" Then
            eobj.Category = "Invoice"
            Status = "O"
            eobj.TransType = "Debit"
        ElseIf Request.QueryString("Formid") = "DN" Then
            eobj.Category = "Debit Note"
            Status = "O"
            eobj.TransType = "Debit"
        ElseIf Request.QueryString("Formid") = "CN" Then
            eobj.Category = "Credit Note"
            Status = "O"
            eobj.TransType = "Credit"
        End If

        If dgView.Items.Count <> 0 Then
            CreateListObjStudent()
            eobj.BatchCode = Trim(txtBatchNo.Text)
            eobj.Description = Trim(txtDesc.Text)
            eobj.BatchDate = Trim(txtBatchDate.Text)
            eobj.TransDate = Trim(txtInvoiceDate.Text)
            eobj.DueDate = Trim(txtDuedate.Text)
            eobj.BatchCode = Trim(txtBatchNo.Text)
            eobj.BatchIntake = ddlIntake.SelectedValue
            eobj.TransactionAmount = Trim(txtTotal.Text)
            eobj.AccountDetailsList = Session("AddFee")
            eobj.DueDate = Trim(txtDuedate.Text)
            eobj.SubType = "Student"
            eobj.TransStatus = "Open"
            eobj.PostedBy = Session("User")
            eobj.PostedDateTime = DateTime.Now
            eobj.UpdatedTime = DateTime.Now
            eobj.ChequeDate = DateTime.Now
            eobj.CreatedDateTime = DateTime.Now
            'eobj.MatricNo = Trim(txtMatrciNo.Text)
            'eobj.StudentName = Trim(txtName.Text)
            'eobj.ProgramID = Trim(txtProgrammeName.Text)
            eobj.PostStatus = "Posted"
            eobj.UpdatedBy = Session("User")
            If Not Session("lstStu") Is Nothing Then
                listStu = Session("lstStu")
            Else
                listStu = Nothing
            End If
            Try
                txtBatchNo.Text = bsobj.StudentBatchUpdate(eobj, listStu)
                ErrorDescription = "Record Posted Successfully "
                lblMsg.Text = ErrorDescription
                ibtnStatus.ImageUrl = "images/posted.gif"
                lblStatus.Value = "Posted"
                eobj.PostStatus = "Posted"
                lblMsg.Visible = True
                Dim scriptstringCallUrl As String = "getCallUrl();"
                ClientScript.RegisterStartupScript(Me.GetType(), "OpenUrl", scriptstringCallUrl, True)
                If Not Session("ListObj") Is Nothing Then
                    ListObjects = Session("ListObj")
                    ListObjects(CInt(txtRecNo.Text) - 1) = eobj
                    Session("ListObj") = ListObjects
                    If lblStatus.Value = "Posted" Then
                        ibtnStatus.ImageUrl = "images/posted.gif"
                        lblStatus.Value = "Posted"
                    End If

                End If
            Catch ex As Exception
                lblMsg.Text = ex.Message.ToString()
                LogError.Log("BatchInvoice", "OnPost", ex.Message)
            End Try

        Else
            lblMsg.Text = "Please Enter At least one Fee Type"
        End If

        If txtBatchNo.Text <> " " Then

            'Get Status Integration To SAGA
            dsReturn = objIntegrationDL.GetIntegrationStatus()

            'Check Status Integration To SAGA
            If dsReturn.Tables(0).Rows(0).Item("CON_Value1") = "1" Then
                Dim strBatchNo As String = Trim(txtBatchNo.Text)
                Invoice(strBatchNo)
            Else
                ErrorDescription = "Record Posted Successfully But No Integration To CF. Please Call Administrator "
                lblMsg.Text = ErrorDescription
            End If

        End If
    End Sub

    ''' <summary>
    ''' Method to Search for Posted Records
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub OnSearchOthers()
        Session("loaddata") = "others"

        If lblCount.Text <> "" Then
            If CInt(lblCount.Text) > 0 Then
                onClearData()
                If ibtnNew.Enabled = False Then
                    ibtnSave.Enabled = False
                    ibtnSave.ImageUrl = "images/gsave.png"
                    ibtnSave.ToolTip = "Access Denied"
                End If

            Else
                Session("PageMode") = "Edit"
                LoadListObjects()
            End If
        Else
            Session("PageMode") = "Edit"
            LoadListObjects()

            PostEnFalse()
        End If
    End Sub
    ''' <summary>
    ''' Method to Disable Options After Posting
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

        ''Print will enable only when is POSTED
        'ibtnPrint.Enabled = False
        'ibtnPrint.ImageUrl = "images/gprint.png"
        'ibtnPrint.ToolTip = "Access denied"
        ibtnPosting.Enabled = False
        ibtnPosting.ImageUrl = "images/gposting.png"
        ibtnPosting.ToolTip = "Access denied"
        'ibtnOthers.Enabled = False
        'ibtnOthers.ImageUrl = "images/post.png"
        'ibtnOthers.ToolTip = "Access denied"
    End Sub
    ''' <summary>
    ''' Method to Load Students Template
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub LoadStudentsTemplates(ByVal studentList As List(Of StudentEn))
        dgView.DataSource = Nothing
        dgView.DataBind()

        Dim list As StudentEn
        Dim liststud As New List(Of StudentEn)
        Dim eobj As New StudentEn
        Dim i As Integer = 0

        Dim amt As Double = 0.0
        Dim pocAmt As Double = 0.0
        Dim j As Integer = 0
        Dim stuen As New StudentEn
        Dim bsstu As New AccountsBAL
        Dim objStu As New StudentBAL
        Dim outamt As Double = 0.0
        'Dim NoKelompok As HiddenField = Nothing
        'Dim NoWarran As HiddenField = Nothing
        'Dim amaunWarran As HiddenField = Nothing
        'Dim noAkaunPelajar As HiddenField = Nothing
        'Dim statusBayaran As HiddenField = Nothing
        'dgView.PageSize = mylst.Count

        For Each stuItem As StudentEn In studentList
            eobj = New StudentEn
            eobj.MatricNo = stuItem.MatricNo
            eobj.ICNo = String.Empty
            eobj.Faculty = String.Empty
            eobj.StudentName = String.Empty
            eobj.ProgramID = String.Empty
            eobj.Faculty = String.Empty
            eobj.CurrentSemester = 0
            eobj.SASI_StatusRec = True
            eobj.STsponsercode = New StudentSponEn()
            eobj.STsponsercode.Sponsor = ""
            Try
                'list = objStu.GetItem(eobj)
                list = objStu.GetItem(eobj.MatricNo)
                liststud.Add(list)
            Catch ex As Exception
                LogError.Log("SponsorAllocation", "UploadData", ex.Message)
                Exit Sub
            End Try
            If list.MatricNo = "" Then
                lblMsg.Text = "Invalid Matric No exists in uploaded file."
                lblMsg.Visible = True
                Session("fileSponsor") = Nothing
                Exit Sub
            End If
        Next
        chkStudent.Visible = True
        dgStudent.DataSource = liststud
        dgStudent.DataBind()

    End Sub

#End Region

#Region "Post To SAGA"
    Private Sub Invoice(strBatchCode As String)

        Try
            Dim strCategory As String = String.Empty

            If Request.QueryString("Formid") = "Inv" Then
                strCategory = "Invoice"
            ElseIf Request.QueryString("Formid") = "DN" Then
                strCategory = "Debit Note"
            ElseIf Request.QueryString("Formid") = "CN" Then
                strCategory = "Credit Note"
            End If

            objIntegration.InvoiceDebitCredit(strBatchCode, strCategory)

        Catch ex As Exception
            lblMsg.Text = ex.Message.ToString()
        End Try

    End Sub
#End Region

    Protected Overloads Sub LoadFields()
        trPrint.Visible = False
        addIntake()
    End Sub

    Protected Sub btnHidden_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        'If Not Session("liststu") Is Nothing Then
        '    addStudent()
        'End If
        'If Not Session("eobj") Is Nothing Then
        '    addFeeType()
        'End If
    End Sub

    Protected Sub btnChangeProg_Click(sender As Object, e As EventArgs) Handles btnChangeProg.Click

        Dim cat As String = "Credit Note"
        Dim trackid As Integer = 1
        lblModule.Value = 1
        ViewStudentChanges(cat, trackid, ReceiptsClass.FeeCtgyTuition)
        dgStudentProg.Columns(3).Visible = True
        dgStudentProg.Columns(4).Visible = True
        dgStudentProg.Columns(5).Visible = False
        dgStudentProg.Columns(6).Visible = False
        dgStudentProg.Columns(7).Visible = False
        dgStudentProg.Columns(8).Visible = False
        dgStudentProg.Columns(9).Visible = False

        btnViewStu.Enabled = False
        Session("Module") = "ChangeProgram"

    End Sub

    Private Sub ViewStudentChanges(Category As String, ModuleId As Integer, FeeType As String)

        Dim objup As New StudentBAL
        Dim i As Integer = 0
        Dim lstobjects As New List(Of StudentEn)
        Dim lstStudent As New List(Of StudentEn)
        Dim eob As New StudentEn
        Dim sem As Integer = 0
        pnlDgFeeType.Visible = False
        pnlDgView.Visible = False
        txtTotal.Text = String.Empty
        txtTotalFeeAmt.Text = String.Empty
        chkStudentProg.Checked = False
        dgView.DataSource = Nothing
        dgView.DataBind()
        dgStudentProg.DataSource = Nothing
        dgStudentProg.DataBind()
        chkFeeType.Checked = False
        chkFeeType.Visible = False
        dgFeeType.DataSource = Nothing
        dgFeeType.DataBind()
        Session("AddFee") = New List(Of AccountsDetailsEn)
        Session(ReceiptsClass.SessionStuChange) = New List(Of StudentEn)
        Session(ReceiptsClass.SessionStuChgMatricNo) = New List(Of StudentEn)
        Session(ReceiptsClass.SessionStuToSave) = New List(Of StudentEn)
        Session("LstStueObj") = New List(Of StudentEn)
        Dim TaxType As Integer = 1

        Try
            'If FeeType = ReceiptsClass.FeeCtgyTuition Then

            lstobjects = objup.GetListStudentChangeDetails(Category, ModuleId, FeeType)
            lstStudent = objup.GetListStudentChangeByProgram(Category, ModuleId, FeeType)
            If ModuleId = 2 Then
                ' GSTAmt = _GstSetupDal.GetGstAmount(dgitem.Cells(dgViewCell.TaxId).Text, txt.Text)
                lstobjects.ForEach(Sub(obj) obj.GSTAmount = String.Format("{0:F}", _GstSetupDal.GetGstAmount(obj.TaxId, obj.TransactionAmount)))
                lstobjects.ForEach(Sub(obj) obj.TaxAmount = obj.GSTAmount)
            End If

            btnLoadFeeType.Visible = True
            'Else
            '    lstobjects = objup.GetListStudentChange(Category, ModuleId)
            '    btnLoadFeeType.Visible = False
            'End If
        Catch ex As Exception
            LogError.Log("BatchInvoice", "btnChangeProg_Click", ex.Message)
        End Try

        'pnlViewChange.Visible = True

        dgStudentProg.DataSource = lstStudent
        dgStudentProg.DataBind()
        Session("LstStueObj") = lstobjects
        If lstStudent.Count <> 0 Then
            Dim chk As CheckBox
            Dim dgitem As DataGridItem
            chkStudentProg.Checked = True
            If chkStudentProg.Checked = True Then
                For Each dgitem In dgStudentProg.Items
                    chk = dgitem.Cells(0).Controls(1)
                    chk.Checked = True
                    If CInt(Request.QueryString("IsView")).Equals(1) Then
                        chk.Enabled = False
                    End If

                Next
                ' getStudentDetailsChange = lstobjects
            End If
        End If

        'REMOVE AUTO ADD TO LIST -START
        ' Session(ReceiptsClass.SessionStuChange) = lstobjects
        hfStudentCount.Value = lstobjects.Count
        StuChgMatricNo = New List(Of StudentEn)
        StuChgMatricNo.AddRange(lstobjects.Where(Function(x) Not StuChgMatricNo.Any(Function(y) y.MatricNo = x.MatricNo)).Select(Function(x) New StudentEn With {.MatricNo = x.MatricNo, .StudentName = x.StudentName, .ProgramID = x.ProgramID, .CurrentSemester = x.CurrentSemester, .CrditHrDiff = x.CrditHrDiff, .CategoryCode = x.CategoryCode}))

        Session(ReceiptsClass.SessionStuChgMatricNo) = StuChgMatricNo
        'REMOVE AUTO ADD TO LIST -END

        'Session("sstr") = ""
        'Session("prgstr") = ""
        'Session("spnstr") = ""

        If Not lstobjects Is Nothing Then
            OnViewStudentGridChange(ModuleId)
        Else

        End If
    End Sub

    Private Sub OnViewStudentGridChange(ModuleId As Integer)

        MultiView1.SetActiveView(View4)

        'btnViewStu.CssClass = "TabButtonClick"
        btnViewStu.Enabled = False
        btnBatchInvoice.CssClass = "TabButton"

        btnSelection.CssClass = "TabButton"
        'chkStudentProg.Visible = False
        pnlBatch.Visible = False
        pnlSelection.Visible = False
        pnlView.Visible = False

        pnlViewChange.Visible = True

    End Sub

    Protected Sub btnChangeCdtHr_Click(sender As Object, e As EventArgs) Handles btnChangeCdtHr.Click

        'Dim category As String
        Dim trackid As Integer = 2
        lblModule.Value = 2

        If Request.QueryString("Formid") = "DN" Then
            ViewStudentChanges("Debit Note", trackid, ReceiptsClass.FeeCtgyTuition)
        ElseIf Request.QueryString("Formid") = "CN" Then
            ViewStudentChanges("Credit Note", trackid, ReceiptsClass.FeeCtgyTuition)
        End If

        dgStudentProg.Columns(5).Visible = True
        dgStudentProg.Columns(6).Visible = True
        dgStudentProg.Columns(7).Visible = True
        dgStudentProg.Columns(3).Visible = False
        dgStudentProg.Columns(4).Visible = False
        dgStudentProg.Columns(8).Visible = False
        dgStudentProg.Columns(9).Visible = False

        btnViewStu.Enabled = False
        Session("Module") = "ChangeCdtHour"

    End Sub

    Protected Sub btnChangeHostel_Click(sender As Object, e As EventArgs) Handles btnChangeHostel.Click

        Dim cat As String = "Credit Note"
        Dim trackid As Integer = 3
        lblModule.Value = 3

        ViewStudentChanges(cat, trackid, ReceiptsClass.FeeCtgyHostel)
        dgStudentProg.Columns(8).Visible = True
        dgStudentProg.Columns(9).Visible = True
        dgStudentProg.Columns(3).Visible = False
        dgStudentProg.Columns(4).Visible = False
        dgStudentProg.Columns(5).Visible = False
        dgStudentProg.Columns(6).Visible = False
        dgStudentProg.Columns(7).Visible = False

        btnViewStu.Enabled = False
        Session("Module") = "ChangeHostel"

    End Sub

    Private Sub CreateListObjStudentChanges()
        Dim eobjstu As StudentEn

        Dim chk As CheckBox
        Dim dgitem As DataGridItem
        listStu = Nothing
        listStu = New List(Of StudentEn)
        For Each dgitem In dgStudentProg.Items
            chk = dgitem.Cells(0).Controls(1)

            If chk.Checked = True Then
                eobjstu = New StudentEn
                eobjstu.MatricNo = dgitem.Cells(1).Text
                eobjstu.StudentName = dgitem.Cells(2).Text
                eobjstu.ProgramID = dgitem.Cells(8).Text
                eobjstu.CurrentSemester = dgitem.Cells(9).Text
                eobjstu.CrditHrDiff = dgitem.Cells(7).Text
                eobjstu.Internal_Use = Session("Module").ToString() + ";" + eobjstu.CrditHrDiff.ToString()
                listStu.Add(eobjstu)
                eobjstu = Nothing
            End If
        Next
        If listStu.Count <> 0 Then
            Session("lstStu") = listStu
        Else
            Session("lstStu") = Nothing
        End If
    End Sub

    Protected Sub dgView_SelectedIndexChanged(sender As Object, e As EventArgs) Handles dgView.SelectedIndexChanged

    End Sub

#Region "GST Function "
    Public Function GSTActAmt(ByVal Amt As Double, ByVal gst As Double, ByVal TaxId As Integer) As String
        Dim ActAmout As Double = 0
        Dim TaxMode As Integer = 0
        If TaxId <> 0 Then
            TaxMode = _GstSetupDal.GetGstDetails(MaxGeneric.clsGeneric.NullToInteger(TaxId)).Tables(0).Rows(0)(3).ToString()
        End If
        If (TaxMode = Generic._TaxMode.Inclusive) Then
            ActAmout = MaxGeneric.clsGeneric.NullToDecimal(Amt) - gst
        ElseIf (TaxMode = Generic._TaxMode.Exclusive) Then
            ActAmout = MaxGeneric.clsGeneric.NullToDecimal(Amt) - gst
        End If
        Return String.Format("{0:F}", ActAmout)
    End Function

    Public Function GSTFeeAmt(ByVal FeeAmt As Double, ByVal gst As Double, ByVal TaxId As Integer) As String
        Dim TaxMode As Integer = 0
        Try
            TaxMode = _GstSetupDal.GetGstDetails(MaxGeneric.clsGeneric.NullToInteger(TaxId)).Tables(0).Rows(0)(3).ToString()
        Catch

        End Try
        Dim FeeAmout As Double = 0
        If (TaxMode = Generic._TaxMode.Inclusive) Then
            FeeAmout = MaxGeneric.clsGeneric.NullToDecimal(FeeAmt)
        ElseIf (TaxMode = Generic._TaxMode.Exclusive) Then
            FeeAmout = MaxGeneric.clsGeneric.NullToDecimal(FeeAmt) - gst
        End If
        Return FeeAmout
    End Function

    Public Function GSTFeeAmtdgFeeType(ByVal FeeAmt As Double, ByVal gst As Double, ByVal TaxId As Integer, ByVal currFeeCode As String) As String
        Dim StuCount As Integer, TotalFeeAmount As Double, TotalGSTAmt As Double, TotalActAmount As Double
        Dim TaxMode As Integer = 0

        If Not Session("AddFee") Is Nothing Then
            ListTRD = Session("AddFee")
        End If
        Dim objAccDetails As New AccountsDetailsEn
        objAccDetails = ListTRD.Where(Function(y) y.ReferenceCode = currFeeCode).FirstOrDefault()
        If Not Session(ReceiptsClass.SessionStuChange) Is Nothing Then
            listStu = Session(ReceiptsClass.SessionStuChange)
        End If
        StuCount = listStu.Where(Function(x) x.ReferenceCode = currFeeCode).ToList().Count()
        Try
            TaxMode = _GstSetupDal.GetGstDetails(MaxGeneric.clsGeneric.NullToInteger(TaxId)).Tables(0).Rows(0)(3).ToString()
        Catch

        End Try
        TotalGSTAmt = gst * StuCount
        TotalFeeAmount = FeeAmt * StuCount
        Dim FeeAmout As Double = 0
        If (TaxMode = Generic._TaxMode.Inclusive) Then
            FeeAmout = MaxGeneric.clsGeneric.NullToDecimal(FeeAmt)

            TotalActAmount = TotalFeeAmount - TotalGSTAmt
            TotalFeeAmount = MaxGeneric.clsGeneric.NullToDecimal(TotalFeeAmount)
        ElseIf (TaxMode = Generic._TaxMode.Exclusive) Then
            FeeAmout = MaxGeneric.clsGeneric.NullToDecimal(FeeAmt) - gst
            TotalFeeAmount = MaxGeneric.clsGeneric.NullToDecimal(TotalFeeAmount)
            TotalActAmount = FeeAmout * StuCount

        End If

        objAccDetails.TransactionAmount = TotalFeeAmount
        objAccDetails.TempAmount = TotalActAmount
        objAccDetails.GSTAmount = TotalGSTAmt
        objAccDetails.StudentQty = StuCount
        'dgFeeType.DataSource = ListTRD
        'dgFeeType.DataBind()
        Return FeeAmout
    End Function
#End Region

    'Private Function GetGSTWithPercentage(Amount As Decimal, TaxPercentage As Decimal, TaxMode As Integer) As Decimal
    '    Dim GstAmount As Decimal = 0
    '    If TaxMode = CShort(Generic._TaxMode.Inclusive) Then
    '        GstAmount = (Amount * TaxPercentage) / (100 + TaxPercentage)
    '        Return GstAmount
    '    ElseIf TaxMode = CShort(Generic._TaxMode.Exclusive) Then
    '        GstAmount = (Amount * TaxPercentage) / 100
    '        Return GstAmount
    '    End If

    'End Function

    Protected Sub btnUpdateCri_Load(sender As Object, e As EventArgs) Handles btnUpdateCri.Load

    End Sub

    Protected Sub chkProgram_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        If Not Session(ReceiptsClass.SessionscProgram) Is Nothing Then
            scProgram = Session(ReceiptsClass.SessionscProgram)
        Else
            scProgram = New List(Of String)
        End If
        Dim chk As CheckBox = (DirectCast(sender, CheckBox))
        Dim dgitem As DataGridItem = DirectCast(chk.Parent.Parent, DataGridItem)

        If chk.Checked Then
            If rbProYes.Checked = False Then
                rbProYes.Checked = True
                rbProNo.Checked = False
            End If
            If Not scProgram.Contains(dgitem.Cells(1).Text) Then
                scProgram.Add(dgitem.Cells(1).Text)
            End If
        Else
            scProgram.Remove(dgitem.Cells(1).Text)
        End If
        Session(ReceiptsClass.SessionscProgram) = scProgram
    End Sub

    Protected Sub chkHostel_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        If Not Session(ReceiptsClass.SessionscHostel) Is Nothing Then
            scHostel = Session(ReceiptsClass.SessionscHostel)
        Else
            scHostel = New List(Of String)
        End If
        Dim chk As CheckBox = (DirectCast(sender, CheckBox))
        Dim dgitem As DataGridItem = DirectCast(chk.Parent.Parent, DataGridItem)

        If chk.Checked Then
            If rbHosYes.Checked = False Then
                rbHosYes.Checked = True
                rbHosNo.Checked = False
            End If
            If Not scHostel.Contains(dgitem.Cells(1).Text) Then
                scHostel.Add(dgitem.Cells(1).Text)
            End If
        Else
            scHostel.Remove(dgitem.Cells(1).Text)
        End If
        Session(ReceiptsClass.SessionscHostel) = scHostel
    End Sub

    Protected Sub chkSponsor_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        If Not Session(ReceiptsClass.SessionscSponsor) Is Nothing Then
            scSponsor = Session(ReceiptsClass.SessionscSponsor)
        Else
            scSponsor = New List(Of String)
        End If
        Dim chk As CheckBox = (DirectCast(sender, CheckBox))
        Dim dgitem As DataGridItem = DirectCast(chk.Parent.Parent, DataGridItem)
        If chk.Checked Then
            If rbSemYes.Checked = False Then
                rbSemYes.Checked = True
                rbSemNo.Checked = False
            End If
            If Not scSponsor.Contains(dgitem.Cells(1).Text) Then
                scSponsor.Add(dgitem.Cells(1).Text)
            End If
        Else
            scSponsor.Remove(dgitem.Cells(1).Text)
        End If
        Session(ReceiptsClass.SessionscSponsor) = scSponsor
    End Sub

    Private Sub FillProgramCheckbox()
        If Not Session(ReceiptsClass.SessionscProgram) Is Nothing Then
            scProgram = Session(ReceiptsClass.SessionscProgram)
        Else
            scProgram = New List(Of String)
        End If
        Dim chk As CheckBox
        Dim dgitem As DataGridItem
        For Each dgitem In dgProgram.Items
            If scProgram.Count > 0 Then
                rbProYes.Checked = True
                rbProNo.Checked = False
                If scProgram.Contains(dgitem.Cells(1).Text) Then
                    chk = dgitem.Cells(0).Controls(1)
                    chk.Checked = True
                End If
            End If
        Next
    End Sub

    Private Sub FillSponsorCheckbox()
        If Not Session(ReceiptsClass.SessionscSponsor) Is Nothing Then
            scSponsor = Session(ReceiptsClass.SessionscSponsor)
        Else
            scSponsor = New List(Of String)
        End If
        Dim chk As CheckBox
        Dim dgitem As DataGridItem
        For Each dgitem In DgSponsor.Items
            If scSponsor.Count > 0 Then
                rbSemYes.Checked = True
                rbSemNo.Checked = False
                If scSponsor.Contains(dgitem.Cells(1).Text) Then
                    chk = dgitem.Cells(0).Controls(1)
                    chk.Checked = True
                End If
            End If
        Next
    End Sub

    Private Sub FillHostelCheckbox()
        If Not Session(ReceiptsClass.SessionscHostel) Is Nothing Then
            scHostel = Session(ReceiptsClass.SessionscHostel)
        Else
            scHostel = New List(Of String)
        End If
        Dim chk As CheckBox
        Dim dgitem As DataGridItem
        For Each dgitem In dgHostel.Items
            If scHostel.Count > 0 Then
                rbHosYes.Checked = True
                rbHosNo.Checked = False
                If scHostel.Contains(dgitem.Cells(1).Text) Then
                    chk = dgitem.Cells(0).Controls(1)
                    chk.Checked = True
                End If
            End If
        Next
    End Sub

    Protected Sub btnLoadFeeType_Click(sender As Object, e As EventArgs) Handles btnLoadFeeType.Click
        btnViewStu.Enabled = True
        'If Not Session(ReceiptsClass.SessionStuChange) Is Nothing Then
        '    getStudentDetailsChange = Session(ReceiptsClass.SessionStuChange)
        'Else
        '    getStudentDetailsChange = New List(Of StudentEn)
        'End If
        Dim lstobjects As New List(Of StudentEn)

        Session("AddFee") = New List(Of AccountsDetailsEn)
        If Not Session("LstStueObj") Is Nothing Then
            lstobjects = Session("LstStueObj")
        Else
            lstobjects = New List(Of StudentEn)
        End If

        Dim chk1 As CheckBox
        Dim dgitem1 As DataGridItem
        For Each dgitem1 In dgStudentProg.Items
            chk1 = dgitem1.Cells(0).Controls(1)
            If chk1.Checked = True Then
                getStudentDetailsChange.AddRange(lstobjects.Where(Function(x) x.MatricNo = dgitem1.Cells(1).Text))
            End If
        Next

        If getStudentDetailsChange.Count = 0 Then
            lblMsg.Text = "Please select at least one student"
        Else
            LoadBatchInvoice()
            If getStudentDetailsChange.Count > 0 Then
                Dim ListTRD As New List(Of AccountsDetailsEn)
                AddStudColumnDgView()
                Dim newStuList As New List(Of StudentEn)
                Dim newStuList2 As New List(Of StudentEn)
                If Session("Module") = "ChangeProgram" Then
                    newStuList = getStudentDetailsChange.Where(Function(x) x.ReferenceCode <> Nothing And x.ProgramChange = 1).ToList()
                ElseIf Session("Module") = "ChangeCdtHour" Then
                    newStuList = getStudentDetailsChange.Where(Function(x) x.ReferenceCode <> Nothing And x.IsTutionFee = 1).ToList()
                ElseIf Session("Module") = "ChangeHostel" Then
                    newStuList = getStudentDetailsChange.Where(Function(x) x.ReferenceCode <> Nothing).ToList()
                End If

                'ListTRD.AddRange(newStuList.Select(Function(x) New AccountsDetailsEn With {.ReferenceCode = x.ReferenceCode, .Description = x.Description, .TransactionAmount = x.TransactionAmount,
                '                                                                      .GSTAmount = x.GSTAmount, .TaxAmount = x.TaxAmount, .Priority = x.Priority, .PostStatus = "Ready", .TransStatus = "Open", .TaxId = x.TaxId}))

                ''GROUPING FEE CODE - START
                If newStuList.Count > 0 Then
                    Dim totalTransAmount As Double = 0
                    Dim totalGSTAmount As Double = 0
                    Dim totalActualFeeAmount As Double = 0
                    If Not Session("AddFee") Is Nothing Then
                        ListTRD = Session("AddFee")
                    Else
                        ListTRD = New List(Of AccountsDetailsEn)
                    End If
                    For Each stu In newStuList
                        If Not ListTRD.Any(Function(x) x.ReferenceCode = stu.ReferenceCode) Then
                            totalGSTAmount = totalGSTAmount + stu.GSTAmount
                            'Change 27/4/2016 -Start
                            If Session("Module") = "ChangeCdtHour" Then
                                If stu.TaxId = 1 Then
                                    stu.TransactionAmount = stu.TransactionAmount + stu.GSTAmount
                                    totalTransAmount = totalTransAmount + stu.TransactionAmount + stu.GSTAmount
                                Else
                                    totalTransAmount = totalTransAmount + stu.TransactionAmount
                                End If
                            Else
                                totalTransAmount = totalTransAmount + stu.TransactionAmount
                            End If
                            'totalTransAmount = totalTransAmount + stu.TransactionAmount
                            'Change 27/4/2016 -End

                            totalActualFeeAmount = totalTransAmount - totalGSTAmount
                            ListTRD.Add(New AccountsDetailsEn With {.ReferenceCode = stu.ReferenceCode, .Description = stu.Description, .TransactionAmount = totalTransAmount,
                                                                      .GSTAmount = totalGSTAmount, .TaxAmount = stu.GSTAmount, .TempAmount = totalActualFeeAmount,
                                                                    .TempPaidAmount = stu.TransactionAmount, .TaxId = stu.TaxId, .StudentQty = 1})
                        Else
                            Dim assignNewTotal As AccountsDetailsEn = ListTRD.Where(Function(x) x.ReferenceCode = stu.ReferenceCode).FirstOrDefault()
                            'Change 27/4/2016 -Start                            
                            'assignNewTotal.TransactionAmount = totalTransAmount + stu.TransactionAmount
                            assignNewTotal.GSTAmount = totalGSTAmount + stu.GSTAmount
                            If Session("Module") = "ChangeCdtHour" Then
                                If stu.TaxId = 1 Then
                                    assignNewTotal.TransactionAmount = totalTransAmount + stu.TransactionAmount + stu.GSTAmount
                                Else
                                    assignNewTotal.TransactionAmount = totalTransAmount + stu.TransactionAmount
                                End If
                            Else
                                assignNewTotal.TransactionAmount = totalTransAmount + stu.TransactionAmount
                            End If
                            'Change 27/4/2016 -End
                            assignNewTotal.TempAmount = assignNewTotal.TransactionAmount - assignNewTotal.GSTAmount
                            assignNewTotal.StudentQty = assignNewTotal.StudentQty + 1
                        End If
                    Next
                End If
                ''GROUPING FEE CODE - END


                Session("AddFee") = ListTRD
                Session(ReceiptsClass.SessionStuChange) = newStuList
                StuChgMatricNo = New List(Of StudentEn)
                StuChgMatricNo.AddRange(getStudentDetailsChange.Where(Function(x) Not StuChgMatricNo.Any(Function(y) y.MatricNo = x.MatricNo)).Select(Function(x) New StudentEn With {.MatricNo = x.MatricNo, .StudentName = x.StudentName, .ProgramID = x.ProgramID, .CurrentSemester = x.CurrentSemester, .CrditHrDiff = x.CrditHrDiff}))
                Session(ReceiptsClass.SessionStuChgMatricNo) = StuChgMatricNo
                hfStudentCount.Value = StuChgMatricNo.Count
                If newStuList.Count > 0 Then
                    dgView.DataSource = newStuList
                    dgView.DataBind()
                    Dim chk As CheckBox
                    Dim dgitem As DataGridItem
                    chkSelectedView.Checked = True
                    If chkSelectedView.Checked = True Then
                        For Each dgitem In dgView.Items
                            chk = dgitem.Cells(0).Controls(1)
                            chk.Checked = True

                            If CInt(Request.QueryString("IsView")).Equals(1) Then
                                chk.Enabled = False
                            End If
                        Next
                    End If
                    pnlDgView.Visible = True
                Else
                    dgView.DataSource = Nothing
                    dgView.DataBind()
                    pnlDgView.Visible = False
                End If
                pnlDgFeeType.Visible = False
                If ListTRD.Count > 0 Then
                    dgFeeType.DataSource = ListTRD
                    dgFeeType.DataBind()
                    chkFeeType.Checked = True
                    'enable checkbox
                    Dim chk As CheckBox
                    Dim dgitem As DataGridItem
                    For Each dgitem In dgFeeType.Items
                        chk = dgitem.Cells(0).Controls(1)
                        chk.Checked = True
                    Next
                Else
                    dgFeeType.DataSource = Nothing
                    dgFeeType.DataBind()
                End If
                ' Session(ReceiptsClass.SessionStuChgMatricNo) = getStudentDetailsChange.Select(Function(x) New StudentEn With {.MatricNo = x.MatricNo, .StudentName = x.StudentName, .ProgramID = x.ProgramID, .CurrentSemester = x.CurrentSemester, .CrditHrDiff = x.CrditHrDiff}).ToList()
                Dim MsgToDisplay = "Some of the student's program does not map to any fee type, please add fee type manually"
                If Session("Module") = "ChangeHostel" Then
                    Dim chk2 As CheckBox
                    Dim dgitem2 As DataGridItem
                    For Each dgitem2 In dgStudentProg.Items
                        chk2 = dgitem2.Cells(0).Controls(1)
                        If chk2.Checked = True Then
                            MsgToDisplay = "Hostel Fee Structure For Code " & dgitem2.Cells(10).Text & " And Block " & dgitem2.Cells(11).Text & " Does Not Exist"
                        End If
                    Next

                End If
                If StuChgMatricNo.Any(Function(x) Not newStuList.Any(Function(y) y.MatricNo = x.MatricNo)) Or newStuList.Count = 0 Then
                    lblMsg.Text = MsgToDisplay
                    lblMsg.Visible = True
                End If

            End If

            'getStudentDetailsCh
            'Hide AddFeeType/RemoveFeeType
            Label26.Visible = False
            ibtnAddFeeType.Visible = False
            Label27.Visible = False
            ibtnRemoveFee.Visible = False
        End If
    End Sub

    Private Sub ClearAllStudentChange()
        If Not Session(ReceiptsClass.SessionStuChange) Is Nothing Then
            getStudentDetailsChange = Session(ReceiptsClass.SessionStuChange)
        Else
            getStudentDetailsChange = New List(Of StudentEn)
        End If
        Dim chk As CheckBox
        Dim dgitem As DataGridItem
        For Each dgitem In dgStudentProg.Items
            chk = dgitem.Cells(0).Controls(1)
            chk.Checked = False
            getStudentDetailsChange.Remove(getStudentDetailsChange.Where(Function(x) x.MatricNo = dgitem.Cells(1).Text).Select(Function(x) x).FirstOrDefault())
        Next
        Session(ReceiptsClass.SessionStuChange) = getStudentDetailsChange
    End Sub
    Protected Sub chkStudentProg_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        'Dim getliststu As New List(Of StudentEn)

        Dim chk As CheckBox
        Dim dgitem As DataGridItem
        'If Not Session("LstStueObj") Is Nothing Then
        '    getliststu = Session("LstStueObj")
        'Else
        '    getliststu = New List(Of StudentEn)
        'End If

        'If Not Session(ReceiptsClass.SessionStuChange) Is Nothing Then
        '    getStudentDetailsChange = Session(ReceiptsClass.SessionStuChange)
        'Else
        '    getStudentDetailsChange = New List(Of StudentEn)
        'End If

        If chkStudentProg.Checked = True Then
            Dim eobjstu As New StudentEn
            For Each dgitem In dgStudentProg.Items
                chk = dgitem.Cells(0).Controls(1)
                chk.Checked = True
            Next
            'getStudentDetailsChange = getliststu
        Else
            For Each dgitem In dgStudentProg.Items
                chk = dgitem.Cells(0).Controls(1)
                chk.Checked = False

                'If Session("Module") = "ChangeProgram" Then
                '    getStudentDetailsChange.RemoveAll(Function(x) x.MatricNo = dgitem.Cells(1).Text And x.OldProgramID = dgitem.Cells(3).Text And x.CurProgramID = dgitem.Cells(4).Text)

                'ElseIf Session("Module") = "ChangeCdtHour" Then
                '    getStudentDetailsChange.RemoveAll(Function(x) x.MatricNo = dgitem.Cells(1).Text And x.OldCrditHrs = dgitem.Cells(5).Text And x.SASI_CrditHrs = dgitem.Cells(6).Text)
                'ElseIf Session("Module") = "ChangeHostel" Then
                '    getStudentDetailsChange.RemoveAll(Function(x) x.MatricNo = dgitem.Cells(1).Text)
                'Else
                '    getStudentDetailsChange.RemoveAll(Function(x) x.MatricNo = dgitem.Cells(1).Text)
                'End If
            Next
            'getStudentDetailsChange = New List(Of StudentEn)
        End If
        'getStudentDetailsChange.ForEach(Sub(x) x.Internal_Use = Session("Module").ToString() + ";" + x.CrditHrDiff.ToString())
        'Session(ReceiptsClass.SessionStuChange) = getStudentDetailsChange
        'Session(ReceiptsClass.SessionTrackingNotes) = getStudentDetailsChange
        'hfStudentCount.Value = getStudentDetailsChange.Count
    End Sub
    'Protected Sub chkStuProg_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)
    '    If Not Session(ReceiptsClass.SessionStuChange) Is Nothing Then
    '        getStudentDetailsChange = Session(ReceiptsClass.SessionStuChange)
    '    Else
    '        getStudentDetailsChange = New List(Of StudentEn)
    '    End If
    '    Dim getliststu As New List(Of StudentEn)
    '    If Not Session("LstStueObj") Is Nothing Then
    '        getliststu = Session("LstStueObj")
    '    Else
    '        getliststu = New List(Of StudentEn)
    '    End If

    '    Dim chk As CheckBox = (DirectCast(sender, CheckBox))
    '    Dim dgitem As DataGridItem = DirectCast(chk.Parent.Parent, DataGridItem)


    '    If Session("Module") = "ChangeProgram" Then
    '        If chk.Checked Then
    '            If Not getStudentDetailsChange.Any(Function(x) x.MatricNo = dgitem.Cells(1).Text And x.OldProgramID = dgitem.Cells(3).Text And x.CurProgramID = dgitem.Cells(4).Text) Then
    '                getStudentDetailsChange.AddRange(getliststu.Where(Function(x) x.MatricNo = dgitem.Cells(1).Text And x.OldProgramID = dgitem.Cells(3).Text And x.CurProgramID = dgitem.Cells(4).Text).ToList())
    '            End If
    '        Else
    '            getStudentDetailsChange.RemoveAll(Function(x) x.MatricNo = dgitem.Cells(1).Text And x.OldProgramID = dgitem.Cells(3).Text And x.CurProgramID = dgitem.Cells(4).Text)
    '            If getStudentDetailsChange.Count = 0 Then
    '                chkStudentProg.Checked = False
    '            End If
    '        End If
    '    ElseIf Session("Module") = "ChangeCdtHour" Then
    '        If chk.Checked Then
    '            If Not getStudentDetailsChange.Any(Function(x) x.MatricNo = dgitem.Cells(1).Text And x.OldCrditHrs = dgitem.Cells(5).Text And x.SASI_CrditHrs = dgitem.Cells(6).Text) Then
    '                getStudentDetailsChange.AddRange(getliststu.Where(Function(x) x.MatricNo = dgitem.Cells(1).Text And x.OldCrditHrs = dgitem.Cells(5).Text And x.SASI_CrditHrs = dgitem.Cells(6).Text).ToList())
    '            End If
    '        Else
    '            getStudentDetailsChange.RemoveAll(Function(x) x.MatricNo = dgitem.Cells(1).Text And x.OldCrditHrs = dgitem.Cells(5).Text And x.SASI_CrditHrs = dgitem.Cells(6).Text)
    '            If getStudentDetailsChange.Count = 0 Then
    '                chkStudentProg.Checked = False
    '            End If
    '        End If
    '    ElseIf Session("Module") = "ChangeHostel" Then
    '        If chk.Checked Then
    '            If Not getStudentDetailsChange.Any(Function(x) x.MatricNo = dgitem.Cells(1).Text) Then
    '                getStudentDetailsChange.AddRange(getliststu.Where(Function(x) x.MatricNo = dgitem.Cells(1).Text).ToList())
    '            End If
    '        Else
    '            getStudentDetailsChange.RemoveAll(Function(x) x.MatricNo = dgitem.Cells(1).Text)
    '            If getStudentDetailsChange.Count = 0 Then
    '                chkStudentProg.Checked = False
    '            End If
    '        End If
    '    Else
    '        If chk.Checked Then
    '            If Not getStudentDetailsChange.Any(Function(x) x.MatricNo = dgitem.Cells(1).Text) Then
    '                getStudentDetailsChange.AddRange(getliststu.Where(Function(x) x.MatricNo = dgitem.Cells(1).Text).ToList())
    '            End If
    '        Else
    '            getStudentDetailsChange.RemoveAll(Function(x) x.MatricNo = dgitem.Cells(1).Text)
    '            If getStudentDetailsChange.Count = 0 Then
    '                chkStudentProg.Checked = False
    '            End If
    '        End If
    '    End If
    '    getStudentDetailsChange.ForEach(Sub(x) x.Internal_Use = Session("Module").ToString() + ";" + x.CrditHrDiff.ToString())
    '    Session(ReceiptsClass.SessionStuChange) = getStudentDetailsChange
    '    Session(ReceiptsClass.SessionTrackingNotes) = getStudentDetailsChange
    '    hfStudentCount.Value = getStudentDetailsChange.Count
    'End Sub
    Private Sub ClearSession()
        Dim fieldMatricNo As BoundColumn = DirectCast(dgView.Columns(dgViewCell.MatricNo), BoundColumn)
        Dim fieldName As BoundColumn = DirectCast(Me.dgView.Columns(dgViewCell.StudentName), BoundColumn)
        fieldMatricNo.DataField = ""
        fieldName.DataField = ""

        dgView.Columns(dgViewCell.MatricNo).Visible = False
        dgView.Columns(dgViewCell.StudentName).Visible = False
        dgView.Columns(dgViewCell.CheckBox).Visible = False
        dgView.DataBind()
        'chkSelectedView.Visible = False
        chkFeeType.Visible = False
        Session(ReceiptsClass.SessionStuChgMatricNo) = Nothing
        Session("Module") = Nothing
        Session(ReceiptsClass.SessionStuToSave) = Nothing
        StuToSave = New List(Of StudentEn)
        getStudentDetailsChange = New List(Of StudentEn)
        Session(ReceiptsClass.SessionStuChange) = New List(Of StudentEn)
        hfStudentCount.Value = 0
    End Sub
    Protected Sub chkView_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        If Not Session(ReceiptsClass.SessionStuChange) Is Nothing Then
            getStudentDetailsChange = Session(ReceiptsClass.SessionStuChange)
        Else
            getStudentDetailsChange = New List(Of StudentEn)
        End If

        If Not Session(ReceiptsClass.SessionStuToSave) Is Nothing Then
            StuToSave = Session(ReceiptsClass.SessionStuToSave)
        Else
            StuToSave = New List(Of StudentEn)
        End If

        Dim chk As CheckBox = (DirectCast(sender, CheckBox))
        Dim dgitem As DataGridItem = DirectCast(chk.Parent.Parent, DataGridItem)
        Dim ReferenceCode As String = dgView.DataKeys(dgitem.DataSetIndex).ToString()
        If chk.Checked Then
            If Not StuToSave.Any(Function(x) x.MatricNo = dgitem.Cells(dgViewCell.MatricNo).Text And x.ReferenceCode = ReferenceCode) Then
                StuToSave.Add(getStudentDetailsChange.Where(Function(x) x.MatricNo = dgitem.Cells(dgViewCell.MatricNo).Text And x.ReferenceCode = ReferenceCode).FirstOrDefault())
            End If
        Else
            StuToSave.Remove(StuToSave.Where(Function(x) x.MatricNo = dgitem.Cells(dgViewCell.MatricNo).Text And x.ReferenceCode = ReferenceCode).Select(Function(x) x).FirstOrDefault())
            If StuToSave.Count = 0 Then
                chkSelectedView.Checked = False
            End If
        End If
        Session(ReceiptsClass.SessionStuToSave) = StuToSave
    End Sub
    Protected Sub chkSelectedView_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        'If Not Session(ReceiptsClass.SessionStuChange) Is Nothing Then
        '    getStudentDetailsChange = Session(ReceiptsClass.SessionStuChange)
        'Else
        '    getStudentDetailsChange = New List(Of StudentEn)
        'End If

        'If Not Session(ReceiptsClass.SessionStuToSave) Is Nothing Then
        '    StuToSave = Session(ReceiptsClass.SessionStuToSave)
        'Else
        '    StuToSave = New List(Of StudentEn)
        'End If

        Dim chk As CheckBox
        Dim dgitem As DataGridItem

        If chkSelectedView.Checked = True Then
            Dim eobjstu As New StudentEn
            For Each dgitem In dgView.Items
                chk = dgitem.Cells(dgViewCell.CheckBox).Controls(1)
                chk.Checked = True
            Next
            ' StuToSave = getStudentDetailsChange
        Else
            For Each dgitem In dgView.Items
                chk = dgitem.Cells(dgViewCell.CheckBox).Controls(1)
                chk.Checked = False
                StuToSave = New List(Of StudentEn)
            Next
        End If
        'Session("lstStu") = listStu
        ' Session(ReceiptsClass.SessionStuToSave) = StuToSave
    End Sub

    Private Sub AddStudColumnDgView()
        Dim fieldMatricNo As BoundColumn = DirectCast(dgView.Columns(dgViewCell.MatricNo), BoundColumn)
        Dim fieldName As BoundColumn = DirectCast(Me.dgView.Columns(dgViewCell.StudentName), BoundColumn)
        fieldMatricNo.DataField = "MatricNo"
        fieldName.DataField = "StudentName"
        dgView.Columns(dgViewCell.MatricNo).Visible = True
        dgView.Columns(dgViewCell.StudentName).Visible = True
        dgView.Columns(dgViewCell.CheckBox).Visible = True
        'chkSelectedView.Visible = True
        ' chkFeeType.Visible = True
    End Sub

    Private Sub CreateListObjTrackingNotes(category As String)
        Dim eobjstu As StudentEn

        Dim chk As CheckBox
        Dim dgitem As DataGridItem
        listStu = Nothing
        listStu = New List(Of StudentEn)
        For Each dgitem In dgStudentProg.Items
            chk = dgitem.Cells(0).Controls(1)

            If chk.Checked = True Then
                eobjstu = New StudentEn
                eobjstu.MatricNo = dgitem.Cells(1).Text
                eobjstu.StudentName = dgitem.Cells(2).Text
                eobjstu.ProgramID = dgitem.Cells(8).Text
                eobjstu.CurrentSemester = dgitem.Cells(9).Text
                eobjstu.OldCrditHrs = dgitem.Cells(5).Text
                eobjstu.OldProgramID = dgitem.Cells(3).Text
                eobjstu.CurProgramID = dgitem.Cells(4).Text
                eobjstu.SASI_CrditHrs = dgitem.Cells(6).Text
                eobjstu.CrditHrDiff = dgitem.Cells(7).Text
                eobjstu.Category = category
                listStu.Add(eobjstu)
                eobjstu = Nothing
            End If
        Next
        If listStu.Count <> 0 Then
            Session("lstStu") = listStu
        Else
            Session("lstStu") = Nothing
        End If
    End Sub

    Private Sub CreateListObjStuToSave()
        Dim chk As CheckBox
        Dim dgitem As DataGridItem
        If Not Session(ReceiptsClass.SessionStuChange) Is Nothing Then
            getStudentDetailsChange = Session(ReceiptsClass.SessionStuChange)
        Else
            getStudentDetailsChange = New List(Of StudentEn)
        End If
        StuToSave = New List(Of StudentEn)
        'getStudentDetailsChange.ForEach(Sub(x) x.Internal_Use = Session("Module").ToString() + ";" + x.CrditHrDiff.ToString())

        'For Each dgitem In dgView.Items
        '    chk = dgitem.Cells(0).Controls(1)
        '    If chk.Checked = True Then

        '        Dim ReferenceCode As String = dgView.DataKeys(dgitem.DataSetIndex).ToString()
        '        If chk.Checked Then
        '            StuToSave.Add(getStudentDetailsChange.Where(Function(x) x.MatricNo = dgitem.Cells(dgViewCell.MatricNo).Text And x.ReferenceCode = ReferenceCode And x.TransactionAmount = dgitem.Cells(dgViewCell.TransactionAmount).Text).FirstOrDefault())
        '        End If
        '    End If
        'Next
        If Session("Module") Is Nothing Then
            For Each dgitem In dgFeeType.Items
                chk = dgitem.Cells(0).Controls(1)
                Dim ReferenceCode As String = dgFeeType.DataKeys(dgitem.ItemIndex).ToString()
                If chk.Checked = True Then
                    'lstStu.RemoveAll(Function(x) x.ReferenceCode = ReferenceCode)
                    StuToSave.AddRange(getStudentDetailsChange.Where(Function(y) y.ReferenceCode = ReferenceCode).ToList())
                End If
            Next
        Else
            For Each dgitem In dgView.Items
                chk = dgitem.Cells(0).Controls(1)
                Dim ReferenceCode As String = dgView.DataKeys(dgitem.ItemIndex).ToString()
                If chk.Checked = True Then
                    StuToSave.AddRange(getStudentDetailsChange.Where(Function(x) x.ReferenceCode = ReferenceCode And x.MatricNo = dgitem.Cells(1).Text).ToList())
                End If
            Next
            StuToSave.ForEach(Sub(x) x.Internal_Use = Session("Module").ToString() + ";" + x.CrditHrDiff.ToString())
        End If

        Session(ReceiptsClass.SessionStuToSave) = StuToSave
    End Sub

    Protected Sub chkFeeType_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)

        Dim dgItem1 As DataGridItem
        Dim chkselect As CheckBox

        For Each dgItem1 In dgFeeType.Items
            chkselect = dgItem1.Cells(0).Controls(1)
            If chkFeeType.Checked = False Then
                chkselect.Checked = False
            Else
                chkselect.Checked = True
            End If
        Next
    End Sub

    'checkStuList
    ''' <summary>
    ''' Method to Remove Student in the session which is not selected
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub checkStuList()
        Dim LstStueObj As New List(Of StudentEn)

        If Not Session("LstStueObj") Is Nothing Then
            LstStueObj = Session("LstStueObj")
        Else
            LstStueObj = New List(Of StudentEn)
        End If
        StuChgMatricNo = New List(Of StudentEn)

        Dim chk As CheckBox
        Dim dgitem As DataGridItem
        If Not Session("Module") Is Nothing Then
            For Each dgitem In dgStudentProg.Items
                chk = dgitem.Cells(0).Controls(1)
                If chk.Checked = True Then
                    If Session("Module") = "ChangeProgram" Then
                        StuChgMatricNo.Add(LstStueObj.Where(Function(x) x.MatricNo = dgitem.Cells(1).Text And x.OldProgramID = dgitem.Cells(3).Text And x.CurProgramID = dgitem.Cells(4).Text).FirstOrDefault())
                    ElseIf Session("Module") = "ChangeCdtHour" Then
                        StuChgMatricNo.Add(LstStueObj.Where(Function(x) x.MatricNo = dgitem.Cells(1).Text And x.OldCrditHrs = dgitem.Cells(5).Text And x.SASI_CrditHrs = dgitem.Cells(6).Text).FirstOrDefault())
                    ElseIf Session("Module") = "ChangeHostel" Then
                        StuChgMatricNo.Add(LstStueObj.Where(Function(x) x.MatricNo = dgitem.Cells(1).Text).FirstOrDefault())
                    End If
                End If
            Next
        Else
            For Each dgitem In dgStudent.Items
                chk = dgitem.Cells(0).Controls(1)
                If chk.Checked = True Then
                    StuChgMatricNo.AddRange(LstStueObj.Where(Function(x) x.MatricNo = dgitem.Cells(1).Text))
                End If
            Next
        End If

        Session(ReceiptsClass.SessionStuChgMatricNo) = StuChgMatricNo
        hfStudentCount.Value = StuChgMatricNo.Count
        If StuChgMatricNo.Count > 0 Then
            AddStudColumnDgView()
        End If

    End Sub

    Protected Sub chk_CheckedChanged(sender As Object, e As EventArgs)
        If Not Session(ReceiptsClass.SessionStuChgMatricNo) Is Nothing Then
            StuChgMatricNo = Session(ReceiptsClass.SessionStuChgMatricNo)
        Else
            StuChgMatricNo = New List(Of StudentEn)
        End If
        Dim listStu As New List(Of StudentEn)
        If Not Session(ReceiptsClass.SessionStuChange) Is Nothing Then
            listStu = Session(ReceiptsClass.SessionStuChange)
        Else
            listStu = New List(Of StudentEn)
        End If
        If Not Session("AddFee") Is Nothing Then
            ListTRD = Session("AddFee")
        Else
            ListTRD = New List(Of AccountsDetailsEn)
        End If

        Dim newListTRD As New List(Of AccountsDetailsEn)
        Dim recalculateListCount As Integer = 0
        Dim chk As CheckBox = (DirectCast(sender, CheckBox))
        Dim dgitem As DataGridItem = DirectCast(chk.Parent.Parent, DataGridItem)
        Dim MatricNo As String = dgStudent.DataKeys(dgitem.ItemIndex).ToString()
        If chkStudent.Checked = True Then
            chkStudent.Checked = False
        End If
        If chk.Checked = True Then
            If dgFeeType.Items.Count = 0 Then
                lblMsg.Text = "Please Add Fee Code For Selected Student"
            Else
                checkStuList()
                StuChgMatricNo = Session(ReceiptsClass.SessionStuChgMatricNo)
                addStuToExistingFee(StuChgMatricNo)
            End If
        Else
            StuChgMatricNo.RemoveAll(Function(x) x.MatricNo = MatricNo)
            Session(ReceiptsClass.SessionStuChgMatricNo) = StuChgMatricNo
            listStu.RemoveAll(Function(x) x.MatricNo = MatricNo)
            Session(ReceiptsClass.SessionStuChange) = listStu
            dgView.DataSource = listStu
            dgView.DataBind()
            For Each eobjFt In ListTRD
                If listStu.Any(Function(x) x.ReferenceCode = eobjFt.ReferenceCode) Then
                    ''re-calculate the fee amount
                    recalculateListCount = listStu.Where(Function(x) x.ReferenceCode = eobjFt.ReferenceCode).Count()
                    eobjFt.GSTAmount = eobjFt.TaxAmount * recalculateListCount
                    eobjFt.TransactionAmount = eobjFt.TempPaidAmount * recalculateListCount
                    eobjFt.StudentQty = recalculateListCount
                    eobjFt.TempAmount = eobjFt.TransactionAmount - eobjFt.GSTAmount
                    newListTRD.Add(eobjFt)
                Else
                    newListTRD = Nothing
                End If
            Next
            If ListTRD.Count = 0 Then
                newListTRD = Nothing
            End If
            Session("AddFee") = newListTRD
            dgFeeType.DataSource = newListTRD
            dgFeeType.DataBind()

            If dgFeeType.Items.Count > 0 Then
                chkFeeType.Checked = True
                chkFeeType.Visible = True
            Else
                chkFeeType.Checked = False
                chkFeeType.Visible = False
            End If

            lblMsg.Text = "Fee Code Will Be Removed For Unchecked Student"
        End If
        Dim chkGrid As CheckBox
        Dim dgitem1 As DataGridItem
        If dgView.Items.Count > 0 Then
            chkSelectedView.Checked = True
            If chkSelectedView.Checked = True Then
                For Each dgitem1 In dgView.Items
                    chkGrid = dgitem1.Cells(0).Controls(1)
                    chkGrid.Checked = True
                Next
            End If
        Else
            chkSelectedView.Visible = False
            chkSelectedView.Checked = False
        End If
        If dgFeeType.Items.Count > 0 Then
            chkFeeType.Checked = True
            chkFeeType.Visible = True
            For Each dgitem1 In dgFeeType.Items
                chkGrid = dgitem1.Cells(0).Controls(1)
                chkGrid.Checked = True
            Next
            txtTotalFeeAmt.Visible = True
            lblTotalFeeAmt.Visible = True
        Else
            txtTotalFeeAmt.Visible = False
            lblTotalFeeAmt.Visible = False
            chkFeeType.Visible = False
            chkFeeType.Checked = False
        End If
    End Sub

    Private Sub OnSearchView()
        Session("loaddata") = "View"

        'If lblCount.Text <> "" Then
        '    If CInt(lblCount.Text) > 0 Then
        '        onClearData()
        '        If ibtnNew.Enabled = False Then
        '            ibtnSave.Enabled = False
        '            ibtnSave.ImageUrl = "images/gsave.png"
        '            ibtnSave.ToolTip = "Access Denied"
        '        End If

        '    Else
        '        Session("PageMode") = "Edit"
        '        LoadListObjects()
        '    End If
        'Else
        Session("PageMode") = "Edit"
        LoadListObjects()

        PostEnFalse()

        txtBatchNo.Enabled = False
        ddlIntake.Enabled = False
        txtBatchDate.Enabled = False
        txtInvoiceDate.Enabled = False
        txtDuedate.Enabled = False
        ibtnAddFeeType.Attributes.Clear()
        ibtnRemoveFee.Attributes.Clear()

        Dim cb As CheckBox, tb As TextBox
        For Each dgItem As DataGridItem In dgFeeType.Items
            cb = dgItem.Cells(0).Controls(1)
            tb = dgItem.Cells(dgFeeTypeCell.Unit_Amount).Controls(1)

            cb.Enabled = False
            tb.Enabled = False
        Next
        'End If
    End Sub

    Protected Sub ibtnAddFeeType_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)

        Dim strStudentCount As String = hfStudentCount.Value
        Dim strStudent As String = hfStdCategory.Value
        If CInt(strStudentCount) > 0 Then
            Dim url As String = "StudentFeetype.aspx?Student=" & strStudent & ""
            Dim sb As New StringBuilder()
            sb.Append("<script type = 'text/javascript'>")
            sb.Append("window.open('")
            sb.Append(url)
            sb.Append("','_blank','location=no,status=0,scrollbars=1,resizable=no,top=8%,left=10%,width=520px,height=500px');")
            sb.Append("</script>")
            ClientScript.RegisterStartupScript(Me.GetType(), _
                      "script", sb.ToString())
        Else
            lblMsg.Visible = True
            lblMsg.Text = "Please select at least one student"
            Exit Sub
        End If

    End Sub

    Protected Sub btnChangeHostel2_Click(sender As Object, e As EventArgs) Handles btnChangeHostel2.Click

        Dim cat As String = "Debit Note"
        Dim trackid As Integer = 3
        lblModule.Value = 3

        ViewStudentChanges(cat, trackid, ReceiptsClass.FeeCtgyHostel)
        dgStudentProg.Columns(8).Visible = True
        dgStudentProg.Columns(9).Visible = True
        dgStudentProg.Columns(3).Visible = False
        dgStudentProg.Columns(4).Visible = False
        dgStudentProg.Columns(5).Visible = False
        dgStudentProg.Columns(6).Visible = False
        dgStudentProg.Columns(7).Visible = False

        btnViewStu.Enabled = False
        Session("Module") = "ChangeHostel"

    End Sub

#Region "GetApprovalDetails"

    Protected Function GetMenuId() As Integer

        Dim MenuId As Integer

        If Request.QueryString("Formid") = "Inv" Then
            MenuId = New MenuDAL().GetMenuMasterList().Where(Function(x) x.PageName = "Student Invoice").Select(Function(y) y.MenuID).FirstOrDefault()
        ElseIf Request.QueryString("Formid") = "DN" Then
            MenuId = New MenuDAL().GetMenuMasterList().Where(Function(x) x.PageName = "Student Debit Note").Select(Function(y) y.MenuID).FirstOrDefault()
        ElseIf Request.QueryString("Formid") = "CN" Then
            MenuId = New MenuDAL().GetMenuMasterList().Where(Function(x) x.PageName = "Student Credit Note").Select(Function(y) y.MenuID).FirstOrDefault()
        End If

        Return MenuId

    End Function

#End Region

#Region "For popup AddApprover() close"

    Protected Sub btnHiddenApp_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnHiddenApp.Click

    End Sub

#End Region

#Region "CheckWorkflowStatus"

    Protected Sub CheckWorkflowStatus(ByVal obj As AccountsEn)

        If obj.PostStatus = "Ready" Then

            Dim _list As List(Of WorkflowEn) = New WorkflowDAL().GetList().Where(Function(x) x.BatchCode = obj.BatchCode).ToList()
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

        End If

    End Sub

#End Region

End Class
