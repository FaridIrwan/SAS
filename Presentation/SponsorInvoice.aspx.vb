Imports System.Collections.Generic
Imports HTS.SAS.Entities
Imports HTS.SAS.BusinessObjects
Imports System.Linq
Imports System.Data
Imports HTS.SAS.DataAccessObjects
Imports System.IO
Imports iTextSharp.text
Imports iTextSharp.text.pdf
Imports System.Net.Mail
Imports System.Net
Imports iTextSharp.text.html
Imports System.Configuration
Imports System.Web.Configuration
Imports System.Net.Configuration

Partial Class SponsorInvoice
    Inherits System.Web.UI.Page
#Region "Global Declarations "
    'Instant Declaration
    Private _GstSetupDal As New HTS.SAS.DataAccessObjects.GSTSetupDAL
    Private ListTRD As New List(Of AccountsDetailsEn)
    Private ListObjects As List(Of AccountsEn)
    Dim listStu As New List(Of StudentEn)
    Private ErrorDescription As String
    Private sumAmt As Double = 0
    Dim sumGST As Double = 0
    Dim DFlag As String
    Dim AutoNo As Boolean
    Dim column As String() = {"", "MatricNo", "StudentName", "ProgramID", "CurrentSemester", "STsponsercode.Sponsor"}
    'new global declarations - start
    Private StuToSave As New List(Of StudentEn)
    'new global declarations - end
    Private _MiddleHelper As New MaxMiddleware.Helper
    Private _Helper As New Helper

#End Region

    ''Private LogErrors As LogError
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        'load PageName
        If Not IsPostBack() Then
            'Getting MenuId
            Menuname(CInt(Request.QueryString("Menuid")))
            Session("Menuid") = Request.QueryString("Menuid")
            MenuId.Value = GetMenuId()

            If CInt(Request.QueryString("IsStudentLedger")).Equals(1) Then
                btnViewStu.Visible = False
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
            txtDuedate.Attributes.Add("OnKeyup", "return CheckDueDate()")
            txtInvoiceDate.Attributes.Add("OnKeyup", "return CheckInvDate()")
            txtBatchDate.Attributes.Add("OnKeyup", "return CheckBatchDate()")
            'Student Screen Popup
            ibtnStudent.Attributes.Add("onclick", "new_window=window.open('AddMulStudents.aspx?cat=St','Hanodale','width=520,height=600,resizable=0');new_window.focus();")
            'ibtnAddStudent.Attributes.Add("onclick", "new_window=window.open('AddMulStudents.aspx?cat=St','Hanodale','width=520,height=600,resizable=0');new_window.focus();")
            'End If
            'Student FeeTypes Screen Popup
            'Student FeeTypes Screen Popup
            ibtnAddFeeType.Attributes.Add("onclick", "return CheckStudent()")
            ibtnPrint.Attributes.Add("onclick", "new_window=window.open('LookupSponsorCoverLetter.aspx','Hanodale','width=700,height=650,resizable=0, top=100');new_window.focus();")
            'ibtnPosting.Attributes.Add("onclick", "new_window=window.open('AddApprover.aspx?MenuId=" & GetMenuId() & "','Hanodale','width=500,height=400,resizable=0');new_window.focus();")
            'btnPrintSCL.Attributes.Add("onclick", "return dllValues()")
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
            Session("ProgSelected") = Nothing
            Session("LstStuWithFeeType") = Nothing
            Session("auto") = Nothing
            Session("batchcode") = Nothing
            Session("SCLCode") = Nothing
            Session("SCLSponsor") = Nothing
            'sumFeetypeAmt = 0
            'sumFeetypeGST = 0
            ClearSession()
            'Loading UserRights
            LoadUserRights()
            'FixEmptyRow(dgStudent, column)
            'Loading Navigation Controls
            DisableRecordNavigator()
            'Formatting DateFields
            dates()
            'LoadStudentCategory()
            'LoadFaculty()
            LoadFields()
            'Load TaxType
            LoadTaxType()
            BindSponsor()
            'LoadSponCoverLetter()
            ''fill 
            'BindSponsor()
            'If dgView.Items.Count <= 0 Then
            '    txtTotal.Visible = False
            '    lblTotal.Visible = False
            'End If
            If dgFeeType.Items.Count <= 0 Then
                txtTotalFeeAmt.Visible = False
                lblTotalFeeAmt.Visible = False
            End If

            Session("isView") = True
        End If
        ' ibtnPrint.Attributes.Add("onCLick", "return getPrint()")
        If Request.QueryString("Formid") = "Inv" Then
            btnBatchInvoice.Text = "Invoice"
        ElseIf Request.QueryString("Formid") = "DN" Then
            btnBatchInvoice.Text = "Debit Note"
        ElseIf Request.QueryString("Formid") = "CN" Then
            btnBatchInvoice.Text = "Credit Note"
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

        'Check selected student from studentDetails
        'If Not Session("lstStu") Is Nothing Then
        '    addStudentList()
        'End If


        'Display Rcord from Student Ledger screen
        If Session("isView") = True Then
            If Not Request.QueryString("BatchCode") Is Nothing Then
                Dim str As String = Request.QueryString("BatchCode")
                Dim constr As String() = str.Split(";")
                txtBatchNo.Text = constr(0)
                DirectCast(Master.FindControl("Panel1"), System.Web.UI.WebControls.Panel).Visible = False
                DirectCast(Master.FindControl("td"), System.Web.UI.HtmlControls.HtmlTableCell).Visible = False

                pnlToolbar.Visible = False
                OnSearchOthers()
            End If
        End If

        If Session("PageMode") = "Edit" Then
            ddlSponsor.Enabled = False
            ibtnLoad.Enabled = False
        Else
            ddlSponsor.Enabled = True
            ibtnLoad.Enabled = True
        End If
        'Session("SCLType") = rblSCLType.SelectedValue.ToString()
        'Session("SCLCode") = ddlSponCoverLetter.SelectedValue.ToString()
        'lblSCLMsg.Text = ""
    End Sub

    Protected Sub Page_LoadComplete(ByVal sender As Object, ByVal e As EventArgs) Handles Me.LoadComplete
        Session("isView") = False
    End Sub

    Protected Sub btnBatchInvoice_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnBatchInvoice.Click
        LoadBatchInvoice()
        checkProgWithStu()
    End Sub

    Protected Sub btnSelection_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSelection.Click
        MultiView1.SetActiveView(View2)
        btnSelection.CssClass = "TabButtonClick"
        btnBatchInvoice.CssClass = "TabButton"
        btnViewStu.CssClass = "TabButton"

        pnlBatch.Visible = False
        pnlSelection.Visible = True
        pnlView.Visible = False
        SponsorGrid()
    End Sub

    Protected Sub btnViewStu_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnViewStu.Click

        'If lblStatus.Value.Equals("New") And Session("spnstr") Is Nothing Then
        '    pnlSelection.Visible = True
        '    lblMsg.Text = "Please select the sponsor(s)"
        '    lblMsg.Visible = True
        'Else
        '    OnViewStudentGrid()
        'End If

        'If Not Session("PageMode") = "Edit" Then
        '    BindSponsor()
        'End If

        'Bind students and sponsors
        OnViewStudentGrid()

        If CInt(Request.QueryString("IsView")).Equals(1) Then
            ddlSponsor.Enabled = False
            ibtnLoad.Attributes.Clear()
            ibtnAddStudent.Attributes.Clear()
            btnLoadFeeType.Enabled = False

            Dim cb As CheckBox
            For Each dgItem As DataGridItem In dgstudent1.Items
                cb = dgItem.Cells(0).Controls(1)
                cb.Enabled = False
            Next
        End If

    End Sub
    Protected Sub ibtnAddStudent_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ibtnAddStudent.Click
        Dim lststud As New List(Of StudentEn)
        Dim _StudentDAL As New HTS.SAS.DataAccessObjects.StudentDAL
        Dim Sponsor As String
        Sponsor = Trim(ddlSponsor.SelectedValue)
        If ddlSponsor.SelectedValue = "-1" Then
            lblMsg.Text = "Please select sponsor"
            lblMsg.Visible = True
        Else
            'Sponsor = ddlSponsor.SelectedValue
            ibtnAddStudent.Attributes.Add("onclick", "new_window=window.open('StudentSponsor.aspx?SponsorCode=" & Sponsor & "','Hanodale','width=450,height=600,resizable=0');new_window.focus();")

        End If

    End Sub

    Protected Sub ibtnNext_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ibtnNext.Click
        OnMoveNext()
    End Sub

    Protected Sub ibtnLast_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ibtnLast.Click
        OnMoveLast()
    End Sub

    Protected Sub ibtnPrevs_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ibtnPrevs.Click
        OnMovePrevious()
    End Sub

    Protected Sub ibtnFirst_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ibtnFirst.Click
        OnMoveFirst()
    End Sub

    Protected Sub ibtnNew_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ibtnNew.Click
        If Session("PageMode") = "Edit" Then
            ddlSponsor.Enabled = False
            ibtnLoad.Enabled = False
        Else

            ddlSponsor.Enabled = True
            ibtnLoad.Enabled = True
        End If
        onAdd()
        LoadBatchInvoice()
        'If chkSelectAll.Visible = True Then
        '    chkSelectAll.Visible = False
        'End If
    End Sub

    Protected Sub ibtnDelete_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ibtnDelete.Click
        ondelete()
    End Sub

    Protected Sub ibtnCancel_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ibtnCancel.Click
        If Session("PageMode") = "Edit" Then
            ddlSponsor.Enabled = False
            ibtnLoad.Enabled = False
        Else
            ddlSponsor.Enabled = True
            ibtnLoad.Enabled = True
        End If
        LoadUserRights()
        onAdd()
    End Sub
    Protected Sub txtFeeAmt_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs)
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
        If Not Session(ReceiptsClass.SessionStuToSave) Is Nothing Then
            listStu = Session(ReceiptsClass.SessionStuToSave)
        End If
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
                FeeAmount = String.Format("{0:F}", CDbl(txt.Text))
                GetGST = _GstSetupDal.GetGstDetails(MaxGeneric.clsGeneric.NullToInteger(dgitem.Cells(dgViewCell.TaxId).Text))
                TaxMode = GetGST.Tables(0).Rows(0)(3).ToString()

                GSTAmt = _GstSetupDal.GetGstAmount(dgitem.Cells(dgViewCell.TaxId).Text, txt.Text)

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
                dgitem.Cells(dgViewCell.Fee_Amount).Text = String.Format("{0:F}", CDbl(txt.Text))

                'GST, Actual Fee, Fee Amount Calculation - Start
                FeeAmount = String.Format("{0:F}", CDbl(txt.Text))
                GetGST = _GstSetupDal.GetGstDetails(MaxGeneric.clsGeneric.NullToInteger(dgitem.Cells(dgViewCell.TaxId).Text))
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

                GSTAmt = _GstSetupDal.GetGstAmount(dgitem.Cells(dgViewCell.TaxId).Text, txt.Text)

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

    End Sub
    'Protected Sub txtFeeAmt_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs)
    '    'Dim eobjTRD As AccountsDetailsEn
    '    'Dim dgitem As DataGridItem
    '    'Dim i As Integer = 0
    '    'Dim txt As TextBox
    '    'For Each dgitem In dgView.Items
    '    '    Session("AddFee") = Nothing
    '    '    txt = dgitem.Cells(2).Controls(1)
    '    '    If txt.Text = "" Then txt.Text = 0
    '    '    dgitem.Cells(2).Text = String.Format("{0:F}", CDbl(txt.Text))
    '    '    eobjTRD = New AccountsDetailsEn
    '    '    eobjTRD.ReferenceCode = dgView.DataKeys(dgitem.ItemIndex).ToString
    '    '    eobjTRD.Description = dgitem.Cells(1).Text
    '    '    eobjTRD.TransactionAmount = String.Format("{0:F}", CDbl(txt.Text))
    '    '    eobjTRD.Priority = dgitem.Cells(8).Text
    '    '    ListTRD.Add(eobjTRD)
    '    '    eobjTRD = Nothing
    '    '    i = i + 1
    '    'Next
    '    'Session("AddFee") = ListTRD
    '    'dgView.DataSource = ListTRD
    '    'dgView.DataBind()
    '    Dim ListTRD As New List(Of AccountsDetailsEn)
    '    ListTRD = Session("AddFee")
    '    Dim dgi As DataGridItem
    '    dgi = DirectCast(DirectCast(sender, TextBox).Parent.Parent, DataGridItem)
    '    Dim strRefCode As String = dgView.DataKeys(dgi.ItemIndex)
    '    Dim strPaidAmount As Double = CDbl(DirectCast(dgi.FindControl("txtFeeAmt"), TextBox).Text)

    '    ListTRD.Where(Function(x) x.ReferenceCode = strRefCode).FirstOrDefault().PaidAmount = strPaidAmount
    '    ListTRD.Where(Function(x) x.ReferenceCode = strRefCode).FirstOrDefault().TransactionAmount = strPaidAmount
    '    Session("AddFee") = ListTRD

    '    AddTotal()
    'End Sub

    Protected Sub DgSponsor_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles DgSponsor.ItemDataBound
        Dim Chk As CheckBox
        If chkSelectSponsor.Checked = True Then
            Select Case e.Item.ItemType
                Case ListItemType.Item, ListItemType.AlternatingItem
                    Chk = CType(e.Item.FindControl("chk"), CheckBox)
                    Chk.Checked = True
            End Select
        End If
    End Sub

    Protected Sub dgStudent_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dgStudent.ItemDataBound
        Dim Chk As CheckBox
        Dim LinkButton As New LinkButton
        If e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem Then


            ' For Each row As Windows.Forms.DataGridView In dgStudent.Items

            LinkButton = CType(e.Item.FindControl("lnkView"), LinkButton)

            'LinkButton.Attributes.Add("onclick", "javascript:window.open('StudentDetails.aspx?Menuid=16&Formid=FS& IsStudentLedger=1 & ProgramId=" & e.Item.Cells(3).Text & "&SponsorCode=" & e.Item.Cells(5).Text & "','Hanodale','width=520,height=300,resizable=0'); return false;")
            'LinkButton.Attributes.Add("onclick", "javascript:window.open('StudentDetails.aspx?ProgramId=" & e.Item.Cells(1).Text & "&SponsorCode=" & e.Item.Cells(3).Text & "','Hanodale','width=520,height=300,resizable=0'); return false;")
            LinkButton.Attributes.Add("onclick", "javascript:window.open('StudentDetails.aspx?ProgramId=" & e.Item.Cells(1).Text & "&SponsorCode=" & e.Item.Cells(3).Text & "&BatchCode=" & txtBatchNo.Text.Trim() & "&MatricNo=" & e.Item.Cells(4).Text & "&Status=" & lblStatus.Value & "','Hanodale','width=550,height=550,resizable=0,scrollbars=1'); return false;")
            'Session("MatricNo") = e.Item.Cells(1).Text
            'Session("ProgramId") = e.Item.Cells(3).Text
            ' Next

        End If

        If chkStudent.Checked = True Then
            Select Case e.Item.ItemType
                Case ListItemType.Item, ListItemType.AlternatingItem
                    Chk = CType(e.Item.FindControl("chkProgram"), CheckBox)
                    Chk.Checked = True

            End Select
        End If
    End Sub
    Protected Sub dgStudent1_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dgstudent1.ItemDataBound
        Dim Chk As CheckBox
        Dim LinkButton As New LinkButton

        'If chkStudent.Checked = True Then
        Select Case e.Item.ItemType
            Case ListItemType.Item, ListItemType.AlternatingItem
                Chk = CType(e.Item.FindControl("chkStudents"), CheckBox)
                Chk.Checked = True

        End Select
        'End If
    End Sub

    Protected Sub btnUpdateCri_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnUpdateCri.Click
        Dim objup As New StudentBAL
        Dim i As Integer = 0
        Dim lstobjects As New List(Of StudentEn)
        Dim eob As New StudentEn
        Dim sem As Integer = 0

        inSponsor()

        eob.CurrentSemester = 0

        eob.STsponsercode = New StudentSponEn

        If Not Session("spnstr") Is Nothing Then
            eob.STsponsercode.Sponsor = Session("spnstr")
        End If
        eob.ProgramID = String.Empty
        eob.SAKO_Code = String.Empty
        eob.Faculty = String.Empty
        eob.CategoryCode = String.Empty
        eob.StCategoryAcess = New StudentCategoryAccessEn
        eob.StCategoryAcess.MenuID = Session("Menuid")

        Try
            lstobjects = objup.GetlisStudent(eob)
        Catch ex As Exception
            LogError.Log("SponsorInvoice", "btnUpdateCri_Click", ex.Message)
        End Try

        pnlView.Visible = True

        'Adding in the exisiting list
        Dim mylst As List(Of StudentEn)
        If Not Session("LstStueObj") Is Nothing Then
            Dim Flag As Boolean
            mylst = Session("LstStueObj")
            While i < lstobjects.Count
                Dim j As Integer = 0
                While j < mylst.Count
                    Flag = False
                    If mylst(j).MatricNo = lstobjects(i).MatricNo Then
                        Flag = True
                        Exit While
                    End If
                    j = j + 1
                End While
                If Flag = False Then
                    mylst.Add(lstobjects(i))
                End If
                i = i + 1
            End While
        Else
            mylst = lstobjects
        End If
        dgStudent.DataSource = mylst
        dgStudent.DataBind()
        Session("LstStueObj") = mylst
        Session("sstr") = ""
        Session("prgstr") = ""
        Session("spnstr") = ""
        If Not lstobjects Is Nothing Then
            OnViewStudentGrid()
        Else

        End If
    End Sub

    'Protected Sub dgView_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dgView.ItemDataBound
    '    Dim txtAmount As TextBox
    '    Select Case e.Item.ItemType
    '        Case ListItemType.Item, ListItemType.AlternatingItem, ListItemType.SelectedItem
    '            txtAmount = CType(e.Item.FindControl("txtFeeAmt"), TextBox)
    '            txtAmount.Text = e.Item.Cells(dgViewCell.Fee_Amount).Text
    '            'txtAmount.Attributes.Add("onKeyPress", "checkValue();")
    '            'txtAmount.Text = String.Format("{0:F}", CDbl(txtAmount.Text))
    '            sumAmt = sumAmt + CDbl(e.Item.Cells(dgViewCell.Fee_Amount).Text)
    '        Case ListItemType.Footer
    '            e.Item.Cells(dgViewCell.Fee_Amount).Text = sumAmt.ToString
    '            'e.Item.Cells(2).Text = String.Format("{0:F}", sumAmt)
    '            Dim footerlbl As Label
    '            footerlbl = DirectCast(e.Item.FindControl("lblFooter"), Label)
    '            footerlbl.Text = String.Format("{0:F}", sumAmt)
    '            txtTotal.Text = String.Format("{0:F}", sumAmt)
    '            Session("FeeSum") = sumAmt
    '    End Select

    'End Sub

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
                dgFeeType.Columns(11).FooterText = String.Format("{0:F}", sumAmt)
                dgFeeType.Columns(10).FooterText = String.Format("{0:F}", sumGST)
                txtTotalFeeAmt.Text = String.Format("{0:F}", sumAmt)
                dgFeeType.DataBind()
        End Select

    End Sub

    'Protected Sub dgFeeType_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dgFeeType.ItemDataBound
    '    Select Case e.Item.ItemType
    '        Case ListItemType.Footer
    '            ' e.Item.Cells(9).Text = sumAmt.ToString
    '            e.Item.Cells(10).Text = String.Format("{0:F}", sumFeetypeAmt)
    '            txtTotalFeeAmt.Text = String.Format("{0:F}", sumFeetypeAmt)
    '            e.Item.Cells(9).Text = String.Format("{0:F}", sumFeetypeGST)
    '    End Select
    'End Sub

    Protected Sub ibtnSave_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ibtnSave.Click
        Dim lstobjects As New List(Of StudentEn)
        Dim sumamountstudent1 As Double = 0
        Dim j As Integer = 0
        'Dim dgItem1 As DataGridItem
        Dim tSponsorLimit As Double = 0
        Dim tAllocatedAmount As Double = 0
        Dim tOutstandingAmount As Double = 0
        Dim tTotalAmount As Double = 0
        Dim sumamountstudent As Double = 0
        Dim eob As New StudentEn
        Dim eobs As New StudentEn
        Dim eobjstu As New StudentEn
        Dim eobj As New StudentBAL
        If dgView.Items.Count = 0 Then
            lblMsg.Text = "Add At least One Fee Item"
            lblMsg.Visible = True
            Exit Sub
        Else
            lblMsg.Visible = False
        End If
        'If dgStudent.Items.Count = 0 Then
        '    lblMsg.Text = "Select At least One Student"
        '    lblMsg.Visible = True
        '    Exit Sub
        'Else
        '    lblMsg.Visible = False
        'End If
        Dim lstStue As List(Of StudentEn)
        Dim lstStud As List(Of AccountsDetailsEn)
        Dim lstsponsor As List(Of StudentEn)
        If hfProgramCount.Value <= 0 Then
            lblMsg.Text = "Select At least One Program"
            lblMsg.Visible = True
            Exit Sub
        End If

        'If Not Session("LstStueObj") Is Nothing Then
        '    lstStue = Session("LstStueObj")
        '    If lstStue.Count > 0 Then
        '        If lstStue.Count = 0 Then
        '            lblMsg.Text = "Select At least One Student"
        '            lblMsg.Visible = True
        '            Exit Sub
        '        Else
        '            Dim lstmatricno As List(Of String)
        '            lstmatricno = lstStue.Select(Function(x) x.MatricNo).Distinct().ToList()
        '            For Each obj In lstmatricno
        '                For Each stu In lstStue.Where(Function(x) x.MatricNo = obj).ToList()
        '                    sumamountstudent = sumamountstudent + (stu.TransactionAmount + stu.GSTAmount)
        '                    eob.TempAmount = sumamountstudent
        '                    If eob.TempAmount > stu.SponsorLimit Then
        '                        If stu.SponsorLimit > 0 Then
        '                            lblMsg.Text = "Sponsor Limit Exceeded"
        '                            'lblMsg.Text = "Sponsor Limit Exceeded"
        '                            lblMsg.Visible = True
        '                            Exit Sub
        '                        ElseIf stu.SponsorLimit = 0 Then
        '                            lblMsg.Visible = False
        '                        End If
        '                    ElseIf eob.TempAmount > stu.OutstandingAmount Then
        '                        If stu.SponsorLimit > 0 Then
        '                            lblMsg.Text = "Sponsor Limit Exceeded"
        '                            lblMsg.Visible = True
        '                            Exit Sub
        '                        ElseIf stu.SponsorLimit = 0 Then
        '                            lblMsg.Visible = False
        '                        End If
        '                    Else
        '                        lblMsg.Visible = False
        '                    End If
        '                Next
        '                sumamountstudent = 0
        '            Next
        '            lblMsg.Visible = False
        '        End If
        '    Else
        '        lstStue = New List(Of StudentEn)
        '    End If
        'Else
        '    lstStue = New List(Of StudentEn)
        'End If

        If Not Session(ReceiptsClass.SessionStuToSave) Is Nothing Then
            StuToSave = Session(ReceiptsClass.SessionStuToSave)
            If StuToSave.Count = 0 Then
                lblMsg.Text = "Select At least One Student"
                lblMsg.Visible = True
                Exit Sub
            Else
                Dim lstmatric As List(Of String)
                lstmatric = StuToSave.Select(Function(x) x.MatricNo).Distinct().ToList()
                For Each obj In lstmatric
                    For Each stu In StuToSave.Where(Function(x) x.MatricNo = obj).ToList()
                        sumamountstudent = sumamountstudent + stu.TransactionAmount
                        eob.TempAmount = sumamountstudent
                        eob.TempAmount = String.Format("{0:F}", eob.TempAmount)
                        If eob.TempAmount > stu.SponsorLimit Then
                            If stu.SponsorLimit > 0 Then
                                lblMsg.Text = "Sponsor Limit Exceeded"
                                'lblMsg.Text = "Sponsor Limit Exceeded"
                                lblMsg.Visible = True
                                Exit Sub
                            ElseIf stu.SponsorLimit = 0 Then
                                lblMsg.Visible = False
                            End If
                        ElseIf eob.TempAmount > stu.OutstandingAmount Then
                            If stu.SponsorLimit > 0 Then
                                lblMsg.Text = "Sponsor Limit Exceeded"
                                lblMsg.Visible = True
                                Exit Sub
                            ElseIf stu.SponsorLimit = 0 Then
                                lblMsg.Visible = False
                            End If
                        Else
                            lblMsg.Visible = False
                        End If
                    Next
                    sumamountstudent = 0
                Next
                lblMsg.Visible = False
            End If
        Else
            StuToSave = New List(Of StudentEn)
        End If
        'If Not Session("SponsorShip") Is Nothing Then
        '    lstsponsor = Session("SponsorShip")
        '    Dim lstmatricno As List(Of String)
        '    lstmatricno = lstsponsor.Select(Function(x) x.MatricNo).Distinct().ToList()
        '    For Each obj In lstmatricno
        '        For Each stu In lstsponsor.Where(Function(x) x.MatricNo = obj).ToList()
        '            sumamountstudent = sumamountstudent + stu.TransactionAmount
        '            eob.TempAmount = sumamountstudent
        '            If eob.TempAmount > stu.SponsorLimit Then
        '                If stu.SponsorLimit > 0 And stu.OutstandingAmount > 0 Then
        '                    lblMsg.Text = "Sponsor Limit Exceeded"
        '                    'lblMsg.Text = "Sponsor Limit Exceeded"
        '                    lblMsg.Visible = True
        '                    Exit Sub
        '                ElseIf stu.SponsorLimit = 0 And stu.OutstandingAmount = 0 Then
        '                    lblMsg.Visible = False
        '                End If
        '            ElseIf eob.TempAmount > stu.OutstandingAmount Then
        '                If stu.SponsorLimit > 0 And stu.OutstandingAmount > 0 Then
        '                    lblMsg.Text = "Sponsor Limit Exceeded"
        '                    lblMsg.Visible = True
        '                    Exit Sub
        '                ElseIf stu.SponsorLimit = 0 And stu.OutstandingAmount = 0 Then
        '                    lblMsg.Visible = False
        '                End If
        '            Else
        '                lblMsg.Visible = False
        '            End If
        '        Next
        '        sumamountstudent = 0
        '    Next
        'Else
        '    lstsponsor = New List(Of StudentEn)
        'End If
        If Not Session("LstStueObj") Is Nothing Then
            lstStue = Session("LstStueObj")
        Else
            lstStue = New List(Of StudentEn)
            If lstStue.Count = 0 Then
                lblMsg.Text = "Select At least One Student"
                lblMsg.Visible = True
                Exit Sub
            Else
                lblMsg.Visible = False
            End If
        End If

        'Dim saveAllStu As List(Of StudentEn) = Session("LstStueObj")
        'dgstudent1.DataSource = saveAllStu
        'dgstudent1.DataBind()
        'For Each dgItem1 In dgstudent1.Items
        '    If dgItem1.Cells(0).Text = dgItem1.Cells(0).Text Then
        '        eobs = New StudentEn
        '        eobs.MatricNo = eob.MatricNo
        '        eobs.TempAmount = dgItem1.Cells(8).Text
        '    End If
        'Next


        CreateListObjFeeType()
        Dim LstStuWithFeeType As List(Of StudentEn) = Session("LstStuWithFeeType")
        If LstStuWithFeeType.Count > 0 Then
            lblMsg.Visible = False
        Else
            lblMsg.Text = "Select At least One Student"
            lblMsg.Visible = True
            Exit Sub
        End If

        If lblStatus.Value = "Posted" Then
            lblMsg.Text = "Post Record Cannot be Edited"
            lblMsg.Visible = True
            Exit Sub
        End If
        SpaceValidation()
        onSave()
        setDateFormat()
    End Sub

    Protected Sub ibtnView_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ibtnView.Click
        'LoadGrid()
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

    '    'If lblStatus.Value = "New" Then
    '    '    lblMsg.Text = "Record not ready for Posting"
    '    '    lblMsg.Visible = True
    '    'ElseIf lblStatus.Value = "Posted" Then
    '    '    Return
    '    'ElseIf lblStatus.Value = "Ready" Then
    '    '    SpaceValidation()
    '    '    OnPost()
    '    '    setDateFormat()
    '    'End If

    '    Dim BatchCode As String = MaxGeneric.clsGeneric.NullToString(txtBatchNo.Text)
    '    Dim eobj As New AccountsEn

    '    'Calling PostToWorkFlow
    '    If _Helper.PostToWorkflow(BatchCode, Session("User"), Request.Url.AbsoluteUri) = True Then
    '        'lblMsg.Text = "Record Posted"
    '        OnPost()
    '        setDateFormat()
    '        lblMsg.Visible = True
    '        lblMsg.Text = "Record Posted Successfully for Approval"
    '    Else
    '        'lblMsg.Text = "Record Already Posted"
    '        'onPost()
    '        'setDateFormat()
    '        'lblMsg.Text = "Record Posted Successfully for Approval"
    '        lblMsg.Visible = True
    '        'lblMsg.Text = "Record Posted Failed"
    '        lblMsg.Text = "Record Already Posted"
    '    End If

    'End Sub

#Region "SendToApproval"

    Protected Sub SendToApproval()

        Try
            If Not Session("listWF") Is Nothing Then
                Dim list As List(Of WorkflowSetupEn) = Session("listWF")
                If list.Count > 0 Then

                    If _Helper.PostToWorkflow(MaxGeneric.clsGeneric.NullToString(txtBatchNo.Text), Session("User"), Request.Url.AbsoluteUri) = True Then

                        setDateFormat()

                        If OnPost() = True Then
                            If Session("listWF").count > 0 Then
                                WorkflowApproverList(Trim(txtBatchNo.Text), Session("listWF"))
                            End If

                            lblMsg.Visible = True
                            lblMsg.Text = "Record Posted Successfully for Approval"

                        End If
                    Else
                        lblMsg.Visible = True
                        lblMsg.Text = "Record Already Posted"
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

        Dim LstStueObj As New List(Of StudentEn)
        If Not Session("LstStueObj") Is Nothing Then
            LstStueObj = Session("LstStueObj")
        End If

        Dim ListObjectsStu As List(Of StudentEn)
        If Not Session("LstStueObjFromDB") Is Nothing Then
            ListObjectsStu = Session("LstStueObjFromDB")
        Else
            ListObjectsStu = New List(Of StudentEn)
        End If

        Dim chk As CheckBox
        Dim dgitem As DataGridItem
        If chkStudent.Checked = True Then
            For Each dgitem In dgStudent.Items
                chk = dgitem.Cells(0).Controls(1)
                chk.Checked = True
                LstStueObj.AddRange(ListObjectsStu.Where(Function(x) Not LstStueObj.Any(Function(y) y.ProgramID = dgitem.Cells(1).Text)).ToList())
            Next
            hfProgramCount.Value = dgStudent.Items.Count
        Else
            For Each dgitem In dgStudent.Items
                chk = dgitem.Cells(0).Controls(1)
                chk.Checked = False
            Next
            LstStueObj = New List(Of StudentEn)
            hfProgramCount.Value = 0
        End If
        Session("LstStueObj") = LstStueObj
    End Sub
    Protected Sub chkStudent1_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)

        Dim LstStueObj As New List(Of StudentEn)
        If Not Session("LstStueObj") Is Nothing Then
            LstStueObj = Session("LstStueObj")
        End If

        Dim ListObjectsStu As List(Of StudentEn)
        If Not Session("LstStueObjFromDB") Is Nothing Then
            ListObjectsStu = Session("LstStueObjFromDB")
        Else
            ListObjectsStu = New List(Of StudentEn)
        End If

        Dim chk As CheckBox
        Dim dgitem As DataGridItem
        If chkStudentall.Checked = True Then
            For Each dgitem In dgstudent1.Items
                chk = dgitem.Cells(0).Controls(1)
                chk.Checked = True
                LstStueObj.AddRange(ListObjectsStu.Where(Function(x) Not LstStueObj.Any(Function(y) y.MatricNo = dgitem.Cells(1).Text)).ToList())
            Next
            hfStudentCount.Value = dgstudent1.Items.Count
        Else
            For Each dgitem In dgstudent1.Items
                chk = dgitem.Cells(0).Controls(1)
                chk.Checked = False
            Next
            LstStueObj = New List(Of StudentEn)
            hfStudentCount.Value = 0
        End If
        Session("LstStueObj") = LstStueObj
    End Sub
    Private Sub LoadFeeType()

    End Sub
    Protected Sub ibtnRemoveFee_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        'If dgView.SelectedIndex <> -1 Then
        '    Dim dgitem As DataGridItem
        '    Dim i As Integer = 0
        '    For Each dgitem In dgView.Items
        '        dgitem.Cells(dgViewCell.Priority).Text = i
        '        i = i + 1
        '    Next
        '    ListTRD = Session("AddFee")
        '    If Not ListTRD Is Nothing Then
        '        If dgView.SelectedIndex <> -1 Then

        '            Try
        '                ListTRD.RemoveAt(CInt(dgView.SelectedItem.Cells(dgViewCell.Priority).Text))
        '            Catch ex As Exception
        '                LogError.Log("SponsorInvoice", "ibtnRemoveFee_Click", ex.Message)
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
        'Else
        '    lblMsg.Text = "Please Select a Feetype to Remove"
        '    lblMsg.Visible = True
        'End If
        If dgFeeType.SelectedIndex <> -1 Then
            Dim dgitem As DataGridItem
            Dim i As Integer = 0

            ListTRD = Session("AddFee")
            Dim currListSt As New List(Of StudentEn)
            currListSt = Session(ReceiptsClass.SessionStuToSave)
            If Not ListTRD Is Nothing Then
                If dgFeeType.SelectedIndex <> -1 Then

                    Try
                        'ListTRD.RemoveAt(CInt(dgFeeType.SelectedIndex))
                        Dim ReferenceCode As String = dgFeeType.DataKeys(dgFeeType.SelectedIndex).ToString()
                        ListTRD.RemoveAll(Function(x) x.ReferenceCode = ReferenceCode)
                        currListSt.RemoveAll(Function(x) x.ReferenceCode = ReferenceCode)
                    Catch ex As Exception
                        LogError.Log("SponsorInvoice", "ibtnRemoveFee_Click", ex.Message)
                    End Try
                    dgFeeType.DataSource = ListTRD
                    dgFeeType.DataBind()
                    dgView.DataSource = currListSt
                    dgView.DataBind()

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
                        chkSelectAll.Checked = True
                        Dim chk As CheckBox
                        For Each dgitem In dgView.Items
                            chk = dgitem.Cells(0).Controls(1)
                            chk.Checked = True
                        Next
                    Else
                        chkSelectAll.Checked = False
                        chkSelectAll.Visible = False
                        dgView.DataSource = Nothing
                        dgView.DataBind()
                        txtTotal.Visible = False
                        lblTotal.Visible = False
                    End If

                    Session("AddFee") = ListTRD
                    Session(ReceiptsClass.SessionStuToSave) = currListSt
                    dgFeeType.SelectedIndex = -1
                End If
            End If
        Else
            lblMsg.Text = "Please Select a Feetype to Remove"
            lblMsg.Visible = True
        End If
    End Sub

    Protected Sub ibtnOthers_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ibtnOthers.Click
        LoadUserRights()
        OnSearchOthers()
    End Sub

    Protected Sub dgView_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs)

    End Sub

    Protected Sub dgHostel_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs)

    End Sub

    Protected Sub txtRecNo_TextChanged1(ByVal sender As Object, ByVal e As System.EventArgs)

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
        ddlIntake.Items.Add(New System.Web.UI.WebControls.ListItem("--Select--", "-1"))
        'ddlIntake.Items.Add(New ListItem("All", "1"))
        ddlIntake.DataTextField = "SemisterSetupCode"
        ddlIntake.DataValueField = "SemisterSetupCode"
        eIntake.SemisterSetupCode = "%"

        Try
            listIntake = bIntake.GetListSemesterCode(eIntake)
        Catch ex As Exception
            LogError.Log("SponsorInvoice", "addIntake", ex.Message)
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

        Dim ListStuCat As New List(Of StudentCategoryEn)

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
            LogError.Log("SponsorInvoice", "Menuname", ex.Message)
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
        dgStudent.Visible = False
        dgstudent1.Visible = True
        'imgLeft3.ImageUrl = "images/b_white_left.png"
        'imgRight3.ImageUrl = "images/b_white_right.png"
        btnViewStu.CssClass = "TabButtonClick"


        'imgLeft1.ImageUrl = "images/b_orange_left.png"
        'imgRight1.ImageUrl = "images/b_orange_right.png"
        btnBatchInvoice.CssClass = "TabButton"

        'imgLeft2.ImageUrl = "images/b_orange_left.png"
        'imgRight2.ImageUrl = "images/b_orange_right.png"
        btnSelection.CssClass = "TabButton"
        'If dgStudent.Items.Count > 0 Then
        '    chkStudent.Visible = True
        'Else
        '    chkStudent.Visible = False
        'End If
        'chkStudentall.Visible = False
        chkStudent.Visible = False
        pnlBatch.Visible = False
        pnlSelection.Visible = False
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
            lblMsg.Text = "Enter Valid Description "
            lblMsg.Visible = True
            txtDesc.Focus()
            Exit Sub
        End If
        'Batch Intake
        If Trim(ddlIntake.SelectedValue).Length < 0 Then
            ddlIntake.SelectedValue = Trim(ddlIntake.SelectedValue)
            lblMsg.Text = "Select Batch Intake"
            lblMsg.Visible = True
            ddlIntake.Focus()
            Exit Sub
        End If
        'Batch date
        If Trim(txtBatchDate.Text).Length < 10 Then
            lblMsg.Text = "Enter Valid Batch Date"
            lblMsg.Visible = True
            txtBatchDate.Focus()
            Exit Sub
        Else
            Try
                txtBatchDate.Text = DateTime.Parse(txtBatchDate.Text, GBFormat)
            Catch ex As Exception
                lblMsg.Text = "Enter Valid Batch Date"
                lblMsg.Visible = True
                txtBatchDate.Focus()
                Exit Sub
            End Try
        End If
        'Invoice date
        If Trim(txtInvoiceDate.Text).Length < 10 Then
            lblMsg.Text = "Enter Valid Invoice Date"
            lblMsg.Visible = True
            txtInvoiceDate.Focus()
            Exit Sub
        Else
            Try
                txtInvoiceDate.Text = DateTime.Parse(txtInvoiceDate.Text, GBFormat)
            Catch ex As Exception
                lblMsg.Text = "Enter Valid Invoice Date"
                lblMsg.Visible = True
                txtInvoiceDate.Focus()
                Exit Sub
            End Try
        End If

        'Due date
        If Trim(txtDuedate.Text).Length < 10 Then
            lblMsg.Text = "Enter Valid Due Date"
            lblMsg.Visible = True
            txtInvoiceDate.Focus()
            Exit Sub
        Else
            Try
                txtDuedate.Text = DateTime.Parse(txtDuedate.Text, GBFormat)
            Catch ex As Exception
                lblMsg.Text = "Enter Valid Due Date"
                lblMsg.Visible = True
                txtInvoiceDate.Focus()
                Exit Sub
            End Try
        End If
        If lblStatus.Value = "Posted" Then
            lblMsg.Text = "Record Already Posted"
            lblMsg.Visible = True
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
        txtBatchDate.Text = Format(Date.Now, "dd/MM/yyyy")
        txtInvoiceDate.Text = Format(Date.Now, "dd/MM/yyyy")
        txtDuedate.Text = Format(DateAdd(DateInterval.Day, 30, Date.Now), "dd/MM/yyyy")
    End Sub
    ''' <summary>
    ''' Method to Add FeeTypes
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub addFeeType()
        Dim stuList As New List(Of StudentEn)
        Dim eobjFt As FeeTypesEn
        Dim listFee As New List(Of FeeTypesEn)
        Dim eobjTRD As New AccountsDetailsEn
        Dim i As Integer = 0
        Dim ListObjectsStu As New List(Of StudentEn)
        Dim objStu As New StudentEn
        Dim objStu2 As New AccountsDetailsEn
        Dim a As New StudentEn
        Dim newListStu As New List(Of StudentEn)
        Dim currListSt As New List(Of StudentEn)

        If Not Session("LstStueObj") Is Nothing Then
            ListObjectsStu = Session("LstStueObj")
        Else
            ListObjectsStu = New List(Of StudentEn)
        End If
        Session("LstStueObj") = stuList
        If ListObjectsStu.Count = 0 Then
            lblMsg.Text = "Please select at least one student"
            lblMsg.Visible = True
            Session("AddFee") = Nothing
            Exit Sub
        End If

        If Not Session("AddFee") Is Nothing Then
            ListTRD = Session("AddFee")
        Else
            ListTRD = New List(Of AccountsDetailsEn)
        End If

        If Not Session(ReceiptsClass.SessionStuToSave) Is Nothing Then
            currListSt = Session(ReceiptsClass.SessionStuToSave)
        Else
            currListSt = New List(Of StudentEn)
        End If
        listFee = Session("eobj")
        newListStu.AddRange(currListSt)
        Dim lstmatricno As List(Of String)
        lstmatricno = ListObjectsStu.Select(Function(x) x.MatricNo).Distinct().ToList()
        'lstmatricno = ListObjectsStu.Select(Function(x) x.MatricNo).Distinct().ToList()
        'hfProgramCount.Value = 0
        'For Each obj In lstmatricno
        '    For Each stu In currListSt.Where(Function(x) x.MatricNo = obj).ToList()
        '        currListSt = New List(Of StudentEn)
        '    Next
        'Next
        Dim totalTransAmount As Double = 0
        Dim totalGSTAmount As Double = 0
        Dim totalActualFeeAmount As Double = 0
        'For Each obj In lstmatricno
        '    For Each a In newListStu.Where(Function(x) x.MatricNo = obj).ToList()
        If Not Session("auto") Is Nothing Then
            'Add fee type according to AFC

            If listFee.Count > 0 Then
                For Each obj In lstmatricno
                    For Each a In ListObjectsStu.Where(Function(x) x.MatricNo = obj).ToList()
                        While i < listFee.Count
                            totalTransAmount = 0
                            totalGSTAmount = 0
                            eobjFt = listFee(i)
                            Dim j As Integer = 0


                            If Not currListSt.Any(Function(x) x.ReferenceCode = eobjFt.FeeTypeCode) Then
                                While j < ListObjectsStu.Count

                                    objStu = New StudentEn
                                    objStu2 = New AccountsDetailsEn
                                    objStu.MatricNo = ListObjectsStu(j).MatricNo
                                    'objStu2.CreditRef = MaxGeneric.clsGeneric.NullToString(objStu.MatricNo)
                                    objStu.StudentName = ListObjectsStu(j).StudentName
                                    objStu.ProgramID = ListObjectsStu(j).ProgramID
                                    objStu.CurrentSemester = ListObjectsStu(j).CurrentSemester
                                    objStu.ReferenceCode = eobjFt.FeeTypeCode
                                    objStu.Description = eobjFt.Description
                                    If ListObjectsStu(j).CategoryCode = ReceiptsClass.Student_BUKAN_WARGANEGARA Or ListObjectsStu(j).CategoryCode = ReceiptsClass.student_International Then
                                        objStu.TransactionAmount = String.Format("{0:F}", eobjFt.NonLocalAmount)
                                        objStu.GSTAmount = String.Format("{0:F}", eobjFt.NonLocalGSTAmount)
                                        objStu.TaxAmount = String.Format("{0:F}", eobjFt.NonLocalGSTAmount)
                                    Else
                                        objStu.TransactionAmount = String.Format("{0:F}", eobjFt.LocalAmount)
                                        objStu.GSTAmount = String.Format("{0:F}", eobjFt.LocalGSTAmount)
                                        objStu.TaxAmount = String.Format("{0:F}", eobjFt.LocalGSTAmount)
                                    End If
                                    totalTransAmount = totalTransAmount + objStu.TransactionAmount
                                    totalGSTAmount = totalGSTAmount + objStu.GSTAmount
                                    objStu.Priority = eobjFt.Priority
                                    objStu.PostStatus = "Ready"
                                    objStu.TransStatus = "Open"
                                    objStu.TaxId = eobjFt.TaxId
                                    objStu.AccountDetailsEn = eobjTRD
                                    objStu.SponsorCode = Session("SponsorValue").ToString() 'ddlSponsor.SelectedValue
                                    objStu.Internal_Use = ListObjectsStu(j).ProgramID
                                    objStu.SponsorLimit = ListObjectsStu(j).SponsorLimit
                                    objStu.OutstandingAmount = ListObjectsStu(j).OutstandingAmount
                                    newListStu.Add(objStu)
                                    'ListTRD.Add(objStu2)
                                    totalActualFeeAmount = totalTransAmount - totalGSTAmount

                                    If Not ListTRD.Any(Function(x) x.ReferenceCode = eobjFt.FeeTypeCode) Then
                                        ListTRD.Add(New AccountsDetailsEn With {.ReferenceCode = eobjFt.FeeTypeCode, .Description = eobjFt.Description, .TransactionAmount = totalTransAmount,
                                                                                      .GSTAmount = totalGSTAmount, .TaxAmount = objStu.GSTAmount, .TempAmount = totalActualFeeAmount,
                                                                                .TempPaidAmount = objStu.TransactionAmount, .TaxId = objStu.TaxId, .StudentQty = 1, .Priority = objStu.Priority,
                                                                                .MatricNumber = objStu.MatricNo})
                                    Else
                                        Dim assignNewTotal As AccountsDetailsEn = ListTRD.Where(Function(x) x.ReferenceCode = eobjFt.FeeTypeCode).FirstOrDefault()
                                        assignNewTotal.TransactionAmount = totalTransAmount
                                        assignNewTotal.GSTAmount = totalGSTAmount
                                        assignNewTotal.TempAmount = totalActualFeeAmount
                                        assignNewTotal.StudentQty = assignNewTotal.StudentQty + 1
                                        'assignNewTotal.CreditRef = objStu.MatricNo
                                    End If
                                    objStu = Nothing
                                    j = j + 1

                                End While
                            Else
                                While j < ListObjectsStu.Count

                                    If Not newListStu.Any(Function(x) x.MatricNo = ListObjectsStu(j).MatricNo And x.ReferenceCode = eobjFt.FeeTypeCode And x.ProgramID = ListObjectsStu(j).ProgramID) Then
                                        objStu = New StudentEn
                                        objStu.MatricNo = ListObjectsStu(j).MatricNo
                                        objStu.StudentName = ListObjectsStu(j).StudentName
                                        objStu.ProgramID = ListObjectsStu(j).ProgramID
                                        objStu.CurrentSemester = ListObjectsStu(j).CurrentSemester
                                        objStu.ReferenceCode = eobjFt.FeeTypeCode
                                        objStu.Description = eobjFt.Description
                                        If ListObjectsStu(j).CategoryCode = ReceiptsClass.Student_BUKAN_WARGANEGARA Or ListObjectsStu(j).CategoryCode = ReceiptsClass.student_International Then
                                            objStu.TransactionAmount = String.Format("{0:F}", eobjFt.NonLocalAmount)
                                            objStu.GSTAmount = String.Format("{0:F}", eobjFt.NonLocalGSTAmount)
                                            objStu.TaxAmount = String.Format("{0:F}", eobjFt.NonLocalGSTAmount)
                                        Else
                                            objStu.TransactionAmount = String.Format("{0:F}", eobjFt.LocalAmount)
                                            objStu.GSTAmount = String.Format("{0:F}", eobjFt.LocalGSTAmount)
                                            objStu.TaxAmount = String.Format("{0:F}", eobjFt.LocalGSTAmount)
                                        End If
                                        totalTransAmount = totalTransAmount + objStu.TransactionAmount
                                        totalGSTAmount = totalGSTAmount + objStu.GSTAmount
                                        objStu.Priority = eobjFt.Priority
                                        objStu.PostStatus = "Ready"
                                        objStu.TransStatus = "Open"
                                        objStu.TaxId = eobjFt.TaxId
                                        objStu.SponsorCode = Session("SponsorValue").ToString() ' ddlSponsor.SelectedValue
                                        objStu.Internal_Use = ListObjectsStu(j).ProgramID
                                        objStu.SponsorLimit = ListObjectsStu(j).SponsorLimit
                                        objStu.OutstandingAmount = ListObjectsStu(j).OutstandingAmount
                                        newListStu.Add(objStu)
                                        totalActualFeeAmount = totalTransAmount - totalGSTAmount

                                        If Not ListTRD.Any(Function(x) x.ReferenceCode = eobjFt.FeeTypeCode) Then
                                            ListTRD.Add(New AccountsDetailsEn With {.ReferenceCode = eobjFt.FeeTypeCode, .Description = eobjFt.Description, .TransactionAmount = totalTransAmount,
                                                                                          .GSTAmount = totalGSTAmount, .TaxAmount = objStu.GSTAmount, .TempAmount = totalActualFeeAmount,
                                                                                     .TempPaidAmount = objStu.TransactionAmount, .TaxId = objStu.TaxId, .StudentQty = 1, .Priority = objStu.Priority,
                                                                                    .MatricNumber = objStu.MatricNo})
                                        Else
                                            Dim assignNewTotal As AccountsDetailsEn = ListTRD.Where(Function(x) x.ReferenceCode = eobjFt.FeeTypeCode).FirstOrDefault()
                                            assignNewTotal.TransactionAmount = totalTransAmount
                                            assignNewTotal.GSTAmount = totalGSTAmount
                                            assignNewTotal.TempAmount = totalActualFeeAmount
                                            assignNewTotal.StudentQty = assignNewTotal.StudentQty + 1
                                            'assignNewTotal.CreditRef = objStu.MatricNo
                                        End If
                                        objStu = Nothing
                                    End If
                                    j = j + 1

                                End While
                            End If
                            i += 1

                        End While
                    Next
                Next
            End If
        Else
            ''Add fee type manually

            listFee = Session("eobj")
            If listFee.Count <> 0 Then
                ' ''IF STUDENT IS NOT SELECTED - START
                'For Each fee In listFee
                '    If Not ListTRD.Any(Function(x) x.ReferenceCode = fee.FeeTypeCode) Then
                '        objStu.ReferenceCode = fee.FeeTypeCode
                '        objStu.Description = fee.Description
                '        'If fee.CategoryCode = "Local" Then
                '        objStu.TransactionAmount = String.Format("{0:F}", fee.LocalAmount)
                '        objStu.GSTAmount = String.Format("{0:F}", fee.LocalGSTAmount)
                '        objStu.TaxAmount = String.Format("{0:F}", fee.LocalGSTAmount)
                '        'Else
                '        '    objStu.TransactionAmount = String.Format("{0:F}", fee.NonLocalAmount)
                '        '    objStu.GSTAmount = String.Format("{0:F}", fee.NonLocalGSTAmount)
                '        '    objStu.TaxAmount = String.Format("{0:F}", fee.NonLocalGSTAmount)
                '        'End If
                '        objStu.Priority = fee.Priority
                '        objStu.PostStatus = "Ready"
                '        objStu.TransStatus = "Open"
                '        objStu.TaxId = fee.TaxId
                '        objStu.SponsorCode = ddlSponsor.SelectedValue
                '        'objStu.Internal_Use = fee.ProgramID
                '        ListTRD.Add(objStu)
                '        objStu = Nothing
                '    End If
                'Next
                ' ''IF STUDENT IS NOT SELECTED - END

                If ListObjectsStu.Count > 0 Then
                    For Each eobjFt In listFee
                        totalTransAmount = 0
                        totalGSTAmount = 0
                        If Not currListSt.Any(Function(x) x.ReferenceCode = eobjFt.FeeTypeCode) Then
                            For Each stu In ListObjectsStu
                                objStu = New StudentEn
                                objStu.MatricNo = stu.MatricNo
                                objStu.StudentName = stu.StudentName
                                objStu.ProgramID = stu.ProgramID
                                objStu.CurrentSemester = stu.CurrentSemester
                                objStu.ReferenceCode = eobjFt.FeeTypeCode
                                objStu.Description = eobjFt.Description
                                If stu.CategoryCode = ReceiptsClass.Student_BUKAN_WARGANEGARA Or stu.CategoryCode = ReceiptsClass.student_International Then
                                    objStu.TransactionAmount = String.Format("{0:F}", eobjFt.NonLocalAmount)
                                    objStu.GSTAmount = String.Format("{0:F}", eobjFt.NonLocalGSTAmount)
                                    objStu.TaxAmount = String.Format("{0:F}", eobjFt.NonLocalGSTAmount)
                                Else
                                    objStu.TransactionAmount = String.Format("{0:F}", eobjFt.LocalAmount)
                                    objStu.GSTAmount = String.Format("{0:F}", eobjFt.LocalGSTAmount)
                                    objStu.TaxAmount = String.Format("{0:F}", eobjFt.LocalGSTAmount)
                                End If
                                totalTransAmount = totalTransAmount + objStu.TransactionAmount
                                totalGSTAmount = totalGSTAmount + objStu.GSTAmount
                                objStu.Priority = eobjFt.Priority
                                objStu.PostStatus = "Ready"
                                objStu.TransStatus = "Open"
                                objStu.TaxId = eobjFt.TaxId
                                objStu.AccountDetailsEn = eobjTRD
                                objStu.SponsorCode = Session("SponsorValue").ToString() 'ddlSponsor.SelectedValue
                                objStu.Internal_Use = stu.ProgramID
                                objStu.SponsorLimit = stu.SponsorLimit
                                objStu.OutstandingAmount = stu.OutstandingAmount
                                totalActualFeeAmount = totalTransAmount - totalGSTAmount

                                If Not ListTRD.Any(Function(x) x.ReferenceCode = eobjFt.FeeTypeCode) Then
                                    ListTRD.Add(New AccountsDetailsEn With {.ReferenceCode = eobjFt.FeeTypeCode, .Description = eobjFt.Description, .TransactionAmount = totalTransAmount,
                                                                                  .GSTAmount = totalGSTAmount, .TaxAmount = objStu.GSTAmount, .TempAmount = totalActualFeeAmount,
                                                                            .TempPaidAmount = objStu.TransactionAmount, .TaxId = objStu.TaxId, .StudentQty = 1, .Priority = objStu.Priority,
                                                                           .MatricNumber = objStu.MatricNo})
                                Else
                                    Dim assignNewTotal As AccountsDetailsEn = ListTRD.Where(Function(x) x.ReferenceCode = eobjFt.FeeTypeCode).FirstOrDefault()
                                    assignNewTotal.TransactionAmount = totalTransAmount
                                    assignNewTotal.GSTAmount = totalGSTAmount
                                    assignNewTotal.TempAmount = totalActualFeeAmount
                                    assignNewTotal.StudentQty = assignNewTotal.StudentQty + 1
                                    'assignNewTotal.CreditRef = objStu.MatricNo
                                End If
                                newListStu.Add(objStu)
                                objStu = Nothing
                            Next
                        Else
                            For Each stu In ListObjectsStu
                                If Not StuToSave.Any(Function(x) x.MatricNo = stu.MatricNo And x.ReferenceCode = eobjFt.FeeTypeCode And x.ProgramID = stu.ProgramID) Then
                                    objStu = New StudentEn
                                    objStu.MatricNo = stu.MatricNo
                                    objStu.StudentName = stu.StudentName
                                    objStu.ProgramID = stu.ProgramID
                                    objStu.CurrentSemester = stu.CurrentSemester
                                    objStu.ReferenceCode = eobjFt.FeeTypeCode
                                    objStu.Description = eobjFt.Description
                                    If stu.CategoryCode = ReceiptsClass.Student_BUKAN_WARGANEGARA Or stu.CategoryCode = ReceiptsClass.student_International Then
                                        objStu.TransactionAmount = String.Format("{0:F}", eobjFt.NonLocalAmount)
                                        objStu.GSTAmount = String.Format("{0:F}", eobjFt.NonLocalGSTAmount)
                                        objStu.TaxAmount = String.Format("{0:F}", eobjFt.NonLocalGSTAmount)
                                    Else
                                        objStu.TransactionAmount = String.Format("{0:F}", eobjFt.LocalAmount)
                                        objStu.GSTAmount = String.Format("{0:F}", eobjFt.LocalGSTAmount)
                                        objStu.TaxAmount = String.Format("{0:F}", eobjFt.LocalGSTAmount)
                                    End If
                                    totalTransAmount = totalTransAmount + objStu.TransactionAmount
                                    totalGSTAmount = totalGSTAmount + objStu.GSTAmount
                                    objStu.Priority = eobjFt.Priority
                                    objStu.PostStatus = "Ready"
                                    objStu.TransStatus = "Open"
                                    objStu.TaxId = eobjFt.TaxId
                                    objStu.SponsorCode = Session("SponsorValue").ToString() ' ddlSponsor.SelectedValue
                                    objStu.Internal_Use = stu.ProgramID
                                    objStu.SponsorLimit = stu.SponsorLimit
                                    objStu.OutstandingAmount = stu.OutstandingAmount
                                    totalActualFeeAmount = totalTransAmount - totalGSTAmount

                                    If Not ListTRD.Any(Function(x) x.ReferenceCode = eobjFt.FeeTypeCode) Then
                                        ListTRD.Add(New AccountsDetailsEn With {.ReferenceCode = eobjFt.FeeTypeCode, .Description = eobjFt.Description, .TransactionAmount = totalTransAmount,
                                                                                      .GSTAmount = totalGSTAmount, .TaxAmount = objStu.GSTAmount, .TempAmount = totalActualFeeAmount,
                                                                                .TempPaidAmount = objStu.TransactionAmount, .TaxId = objStu.TaxId, .StudentQty = 1, .Priority = objStu.Priority,
                                                                               .MatricNumber = objStu.MatricNo})
                                    Else
                                        Dim assignNewTotal As AccountsDetailsEn = ListTRD.Where(Function(x) x.ReferenceCode = eobjFt.FeeTypeCode).FirstOrDefault()
                                        assignNewTotal.TransactionAmount = assignNewTotal.TransactionAmount + objStu.TransactionAmount
                                        assignNewTotal.GSTAmount = assignNewTotal.GSTAmount + objStu.GSTAmount
                                        assignNewTotal.TempAmount = assignNewTotal.TransactionAmount - assignNewTotal.GSTAmount
                                        assignNewTotal.StudentQty = assignNewTotal.StudentQty + 1
                                        'assignNewTotal.CreditRef = objStu.MatricNo
                                    End If
                                    newListStu.Add(objStu)
                                    objStu = Nothing
                                End If
                            Next
                        End If
                    Next
                End If
            End If

        End If
        '    Next
        'Next


        'ListTRD.AddRange(newListStu.Select(Function(x) New AccountsDetailsEn With {.ReferenceCode = x.ReferenceCode, .Description = x.Description, .TransactionAmount = x.TransactionAmount,
        '                                                                      .GSTAmount = x.GSTAmount, .TaxAmount = x.TaxAmount, .Priority = x.Priority, .PostStatus = "Ready", .TransStatus = "Open", .TaxId = x.TaxId}))

        Session("AddFee") = ListTRD
        Session(ReceiptsClass.SessionStuToSave) = newListStu
        'StuToSave = newListStu.OrderBy(Function(x) x.MatricNo).ToList()
        AddStudColumnDgView()
        dgView.DataSource = newListStu
        dgView.DataBind()

        'GROUP FEE TYPE - START
        dgFeeType.DataSource = ListTRD
        dgFeeType.DataBind()
        If ListTRD.Count > 0 Then
            chkFeeType.Checked = True
            chkFeeType.Visible = False
            lblTotalFeeAmt.Visible = True
            txtTotalFeeAmt.Visible = True
        Else
            chkFeeType.Checked = False
            chkFeeType.Visible = False
            lblTotalFeeAmt.Visible = False
            txtTotalFeeAmt.Visible = False
        End If
        If ListTRD.Count > 0 Then
            'enable checkbox
            Dim chk As CheckBox
            Dim dgitem As DataGridItem
            For Each dgitem In dgFeeType.Items
                chk = dgitem.Cells(0).Controls(1)
                chk.Checked = True
            Next
        End If
        'GROUP FEE TYPE - END
        Dim eob As New StudentEn
        Dim sumamountstudent As Double = 0
        Dim lstmatric As List(Of String)
        lstmatric = newListStu.Select(Function(x) x.MatricNo).Distinct().ToList()
        For Each dgItem1 In dgstudent1.Items
            For Each obj In lstmatric
                'If dgItem1.Cells(1).Text = obj Then
                For Each stu In newListStu.Where(Function(x) x.MatricNo = obj And x.MatricNo = dgItem1.Cells(1).Text).ToList()

                    sumamountstudent = sumamountstudent + stu.TransactionAmount
                    eob.TempAmount = sumamountstudent
                    eob.TempAmount = String.Format("{0:F}", eob.TempAmount)
                    dgItem1.Cells(3).Text = String.Format("{0:F}", eob.TempAmount)
                    If stu.SponsorLimit = 0 And stu.AllocatedAmount = 0 Then
                        dgItem1.Cells(8).Text = "-"
                    End If
                Next

                'End If
            Next
            sumamountstudent = 0
        Next
        Session("eobj") = Nothing
        If newListStu.Count > 0 Then
            chkSelectAll.Checked = True

            'Enable checkbox
            Dim chk As CheckBox
            Dim dgitem As DataGridItem
            For Each dgitem In dgView.Items
                chk = dgitem.Cells(0).Controls(1)
                chk.Checked = True
            Next
        Else
            chkSelectAll.Checked = False
        End If
    End Sub
    ''' <summary>
    ''' Method to Add the List of Students
    ''' </summary>
    ''' <remarks></remarks>
    ''' asal punye '24052016'
    'Private Sub addStudent()
    '    Dim ListObjectsStu As List(Of StudentEn)
    '    Dim eobj As StudentEn
    '    Dim mylist As List(Of StudentEn)
    '    Dim i As Integer = 0

    '    If Not Session("LstStueObj") Is Nothing Then
    '        ListObjectsStu = Session("LstStueObj")
    '    Else
    '        ListObjectsStu = New List(Of StudentEn)
    '    End If
    '    mylist = Session("liststu")
    '    'Checking for the Exisiting Students in the Grid
    '    If mylist.Count <> 0 Then
    '        While i < mylist.Count
    '            eobj = mylist(i)
    '            Dim j As Integer = 0
    '            Dim Flag As Boolean = False
    '            While j < ListObjectsStu.Count
    '                If ListObjectsStu(j).MatricNo = eobj.MatricNo Then
    '                    Flag = True
    '                    Exit While
    '                End If
    '                j = j + 1
    '            End While
    '            If Flag = False Then
    '                ListObjectsStu.Add(eobj)
    '            End If
    '            i = i + 1
    '        End While
    '    End If
    '    If ListObjectsStu.Count = 0 Then
    '        chkStudent.Visible = False
    '        btnLoadFeeType.Visible = False
    '    Else
    '        chkStudent.Visible = True
    '        btnLoadFeeType.Visible = True
    '    End If
    '    Session("LstStueObj") = ListObjectsStu
    '    dgStudent.DataSource = ListObjectsStu
    '    dgStudent.DataBind()
    '    Session("liststu") = Nothing
    'End Sub

    'baru 24052016 by farid'
    Private Sub addStudent()
        Dim ListObjectsStu As List(Of StudentEn)
        Dim objup As New StudentBAL
        Dim eobj As StudentEn
        Dim mylist As List(Of StudentEn)
        Dim sponsorship As New List(Of StudentEn)
        Dim lstSponsorShip As New List(Of StudentEn)
        Dim i As Integer = 0

        If Not Session("LstStueObj") Is Nothing Then
            ListObjectsStu = Session("LstStueObj")
        Else
            ListObjectsStu = New List(Of StudentEn)
        End If
        If Not Session("SponsorShip") Is Nothing Then
            sponsorship = Session("SponsorShip")
        Else
            sponsorship = New List(Of StudentEn)
        End If
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
                    lstSponsorShip = objup.GetStudentSponsorshipWithoutValidity(eobj.MatricNo)
                    If lstSponsorShip.Count > 0 Then
                        sponsorship.AddRange(lstSponsorShip)
                    End If
                End If
                i = i + 1
            End While
        End If
        'If ListObjectsStu.Count = 0 Then
        '    chkStudent.Visible = False
        '    btnLoadFeeType.Visible = False
        'Else
        '    chkStudent.Visible = True
        '    btnLoadFeeType.Visible = True
        'End If
        Session("SponsorShip") = sponsorship
        Session("LstStueObj") = ListObjectsStu
        dgstudent1.DataSource = ListObjectsStu
        dgstudent1.DataBind()
        Dim sponsorfee As New List(Of StudentEn)
        sponsorfee = objup.GetSponsorFeeList(ddlSponsor.SelectedValue)

        'Dim chk As CheckBox
        'Dim dgitem As DataGridItem
        If sponsorfee.Count = 0 Then

        Else
            sponsorship = sponsorship.Where(Function(x) sponsorfee.Any(Function(y) y.ReferenceCode = x.ReferenceCode)).ToList()
        End If
        Dim eob As New StudentEn
        Dim sumamountstudent As Double = 0
        Dim lstmatric As List(Of String)
        lstmatric = ListObjectsStu.Select(Function(x) x.MatricNo).Distinct().ToList()
        For Each dgItem1 In dgstudent1.Items
            For Each obj In lstmatric
                'If dgItem1.Cells(1).Text = obj Then
                For Each stu In sponsorship.Where(Function(x) x.MatricNo = obj And x.MatricNo = dgItem1.Cells(1).Text).ToList()

                    sumamountstudent = sumamountstudent + stu.TransactionAmount
                    eob.TempAmount = sumamountstudent
                    eob.TempAmount = String.Format("{0:F}", eob.TempAmount)
                    dgItem1.Cells(3).Text = String.Format("{0:F}", eob.TempAmount)
                    'saveAllStu.AddRange(saveAllStu
                    If stu.SponsorLimit = 0 And stu.AllocatedAmount = 0 Then
                        dgItem1.Cells(8).Text = "-"
                    End If
                Next

                'End If
            Next
            sumamountstudent = 0
        Next
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
        ibtnSave.Enabled = True
        If Session("loaddata") = "others" Then
            eob.TransType = "Credit"
            eob.Category = "Invoice"
            If txtBatchNo.Text <> "" Then
                eob.BatchCode = txtBatchNo.Text
            Else
                eob.BatchCode = ""
            End If
            eob.PostStatus = "Posted"
            eob.SubType = "Sponsor"

            If CInt(Request.QueryString("IsView")).Equals(1) Then
                eob.PostStatus = "Ready"
            End If

            Try
                ListObjects = obj.GetSponsorInvoiceTransactions(eob)
            Catch ex As Exception
                LogError.Log("SponsorInvoice", "LoadListObjects", ex.Message)
            End Try

        ElseIf Session("loaddata") = "View" Then
            eob.TransType = "Credit"
            eob.Category = "Invoice"
            If txtBatchNo.Text <> "" Then
                eob.BatchCode = txtBatchNo.Text
            Else
                eob.BatchCode = ""
            End If
            eob.PostStatus = "Ready"
            eob.SubType = "Sponsor"

            Try
                ListObjects = obj.GetSponsorInvoiceTransactions(eob)
            Catch ex As Exception
                LogError.Log("SponsorInvoice", "LoadListObjects", ex.Message)
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

                ListObjects = Session("ListObj")
                obj = ListObjects(RecNo)
                txtBatchNo.Text = obj.BatchCode
                txtBatchNo.ReadOnly = True

                If obj.BatchIntake <> "" Or obj.BatchIntake IsNot Nothing Then
                    ddlIntake.SelectedValue = obj.BatchIntake
                Else
                    ddlIntake.SelectedIndex = -1
                End If

                ddlIntake.Enabled = False
                txtDesc.Text = obj.Description
                txtBatchDate.Text = obj.BatchDate
                txtInvoiceDate.Text = obj.TransDate
                txtDuedate.Text = obj.DueDate
                txtTotal.Text = obj.TransactionAmount
                eobjTransDatails = New AccountsDetailsEn
                eobjTransDatails.TransactionID = obj.TranssactionID
                Dim mylst As New List(Of StudentEn)
                If obj.Internal_Use = "auto" Then
                    AddStudColumnDgView()
                    Session("auto") = "auto"
                Else
                    ClearSession()
                End If
                Dim lststud As New List(Of StudentEn)
                Dim _StudentDAL As New HTS.SAS.DataAccessObjects.StudentDAL
                Dim LstStueObj As New List(Of StudentEn)
                If obj.PostStatus = "Ready" Then
                    Try
                        'ListTranctionDetails = objProcess.GetStudentSponsorInvoiceAccountsDetails(eobjTransDatails)
                        ListTranctionDetails = objProcess.GetStudentSponsorInvoiceAccountsDetailsByBatchCode(obj)
                        If ListTranctionDetails.Count > 0 Then
                            LstStueObj.AddRange(ListTranctionDetails.Select(Function(x) New StudentEn With {.ProgramID = x.Internal_Use, .MatricNo = x.MatricNo,
                                                                                     .StudentName = x.StudentName, .CurrentSemester = x.CurrentSemester,
                                                                                                           .TransactionAmount = x.TransactionAmount, .TaxAmount = x.TaxAmount,
                                                                                                           .ReferenceCode = x.ReferenceCode, .TaxId = x.TaxId,
                                                                                                           .Description = x.Description, .GSTAmount = x.GSTAmount, .SponsorCode = x.SponsorCode,
                                                                                                           .Internal_Use = x.Internal_Use, .CurretSemesterYear = x.Sudentacc.CurretSemesterYear,
                                                                                                           .ProgramName = x.Sudentacc.ProgramName, .ICNo = x.Sudentacc.ICNo,
                                                                                                            .CategoryCode = x.Sudentacc.CategoryCode
                                                                                    }))
                            Session(ReceiptsClass.SessionStuToSave) = LstStueObj
                            Dim newStuList As New List(Of StudentEn)
                            newStuList.AddRange(LstStueObj.Where(Function(p) Not newStuList.Any(Function(q) q.MatricNo = p.MatricNo)).Select(Function(x) New StudentEn With {.ProgramID = x.ProgramID, .MatricNo = x.MatricNo,
                                                                                     .StudentName = x.StudentName, .CurrentSemester = x.CurrentSemester,
                                                                                                          .Internal_Use = x.Internal_Use, .CurretSemesterYear = x.CurretSemesterYear,
                                                                                                           .ProgramName = x.ProgramName, .ICNo = x.ICNo, .CategoryCode = x.CategoryCode
                                                                                    }))
                            ListTranctionDetails.ForEach(Sub(x) x.ProgramID = x.Internal_Use)
                            Session("LstStueObj") = newStuList
                            hfStudentCount.Value = newStuList.Count

                            ''GROUPING FEE CODE - START
                            If ListTranctionDetails.Count > 0 Then
                                Dim totalTransAmount As Double = 0
                                Dim totalGSTAmount As Double = 0
                                Dim GSTAmt As Double = 0
                                Dim totalActualAmount As Double = 0

                                ListTRD = New List(Of AccountsDetailsEn)
                                For Each stu In ListTranctionDetails
                                    If Not ListTRD.Any(Function(x) x.ReferenceCode = stu.ReferenceCode) Then
                                        totalGSTAmount = totalGSTAmount + stu.GSTAmount
                                        totalTransAmount = totalTransAmount + stu.TransactionAmount
                                        totalActualAmount = totalTransAmount - totalGSTAmount
                                        ListTRD.Add(New AccountsDetailsEn With {.ReferenceCode = stu.ReferenceCode, .Description = stu.Description, .TransactionAmount = totalTransAmount,
                                                                                  .GSTAmount = totalGSTAmount, .TaxAmount = stu.GSTAmount, .TempAmount = totalActualAmount,
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
                        End If
                    Catch ex As Exception
                        LogError.Log("SponsorInvoice", "FillData", ex.Message)
                    End Try

                    dgView.DataSource = ListTranctionDetails
                    dgView.DataBind()

                    'If dgView.Items.Count >= 0 Then
                    '    txtTotal.Visible = True
                    '    lblTotal.Visible = True
                    'End If
                    Session("LstStuWithFeeType") = LstStueObj
                    ' Session("AddFee") = ListTranctionDetails 
                    'Dim mylst As New List(Of StudentEn)
                    'Dim bsobj As New AccountsBAL
                    'Dim loen As New StudentEn
                    'loen.BatchCode = txtBatchNo.Text

                    'Try
                    '    mylst = bsobj.GetListStudentSponsorInvoicebyBatchID(loen)
                    'Catch ex As Exception
                    '    LogError.Log("SponsorInvoice", "FillData", ex.Message)
                    'End Try
                    'BindSponsor()  '(New System.Collections.Generic.Mscorlib_CollectionDebugView(Of HTS.SAS.Entities.StudentEn)(mylst)).Items(0).SponsorCode
                    Dim bsobj As New AccountsBAL
                    Dim loen As New StudentEn
                    Dim lststud3 As New List(Of StudentEn)
                    Dim batchcode As String
                    Dim Sponsor As String
                    Sponsor = obj.CreditRefOne
                    batchcode = txtBatchNo.Text
                    'loen.BatchCode = txtBatchNo.Text
                    lststud3 = _StudentDAL.GetStudentDetailsForSponsorInvoice(batchcode, Sponsor)

                    'Try
                    '    mylst = bsobj.GetListStudentSponsorInvoicebyBatchIDForStudent(loen)
                    'Catch ex As Exception
                    '    LogError.Log("SponsorInvoice", "FillData", ex.Message)
                    'End Try

                    ibtnStatus.ImageUrl = "images/Ready.gif"
                    lblStatus.Value = "Ready"

                    hfProgramCount.Value = lststud3.Count
                    dgstudent1.DataSource = lststud3
                    dgstudent1.DataBind()

                    ddlSponsor.SelectedValue = obj.CreditRefOne

                    Session("LstStueObj") = lststud3

                    Dim lstmatric As List(Of String)
                    lstmatric = lststud3.Select(Function(x) x.MatricNo).Distinct().ToList()
                    For Each dgItem1 In dgstudent1.Items
                        For Each objects In lstmatric
                            'If dgItem1.Cells(1).Text = obj Then
                            For Each stu In lststud3.Where(Function(x) x.MatricNo = objects And x.MatricNo = dgItem1.Cells(1).Text).ToList()
                                If stu.SponsorLimit = 0 And stu.OutstandingAmount = 0 Then
                                    dgItem1.Cells(8).Text = "-"
                                End If
                            Next

                            'End If
                        Next

                    Next

                    'Changing Status
                    If obj.PostStatus = "Ready" Then
                        lblStatus.Value = "Ready"
                        ibtnStatus.ImageUrl = "images/Ready.gif"
                        'pnlSponCoverLetter.Visible = False
                        DisablePrint()
                    End If
                    If obj.PostStatus = "Posted" Then
                        lblStatus.Value = "Posted"
                        ibtnStatus.ImageUrl = "images/Posted.gif"
                        'pnlSponCoverLetter.Visible = True
                        PrintAble()
                        Session("batchcode") = txtBatchNo.Text
                    End If

                    'Enable checkbox

                    Dim chk As CheckBox
                    Dim dgitem As DataGridItem

                    For Each dgitem In dgStudent.Items
                        chk = dgitem.Cells(0).Controls(1)
                        chk.Checked = True
                    Next
                ElseIf obj.PostStatus = "Posted" Then
                    ListTranctionDetails = objProcess.GetStudentSponsorInvoiceAccountsDetailsByBatchCode(obj)
                    If ListTranctionDetails.Count > 0 Then
                        LstStueObj.AddRange(ListTranctionDetails.Select(Function(x) New StudentEn With {.ProgramID = x.Internal_Use, .MatricNo = x.MatricNo,
                                                        .StudentName = x.StudentName, .CurrentSemester = x.CurrentSemester,
                                                                            .TransactionAmount = x.TransactionAmount, .TaxAmount = x.TaxAmount,
                                                                            .ReferenceCode = x.ReferenceCode, .TaxId = x.TaxId,
                                                                            .Description = x.Description, .GSTAmount = x.GSTAmount, .SponsorCode = x.SponsorCode,
                                                                            .Internal_Use = x.Internal_Use, .CurretSemesterYear = x.Sudentacc.CurretSemesterYear,
                                                                            .ProgramName = x.Sudentacc.ProgramName, .ICNo = x.Sudentacc.ICNo,
                                                                                .CategoryCode = x.Sudentacc.CategoryCode
                                                        }))
                        Session(ReceiptsClass.SessionStuToSave) = LstStueObj
                        Dim newStuList As New List(Of StudentEn)
                        newStuList.AddRange(LstStueObj.Where(Function(p) Not newStuList.Any(Function(q) q.MatricNo = p.MatricNo)).Select(Function(x) New StudentEn With {.ProgramID = x.ProgramID, .MatricNo = x.MatricNo,
                                                                                 .StudentName = x.StudentName, .CurrentSemester = x.CurrentSemester,
                                                                                                      .Internal_Use = x.Internal_Use, .CurretSemesterYear = x.CurretSemesterYear,
                                                                                                       .ProgramName = x.ProgramName, .ICNo = x.ICNo, .CategoryCode = x.CategoryCode
                                                                                }))
                        ListTranctionDetails.ForEach(Sub(x) x.ProgramID = x.Internal_Use)
                        Session("LstStueObj") = newStuList
                        hfStudentCount.Value = newStuList.Count

                        ''GROUPING FEE CODE - START
                        If ListTranctionDetails.Count > 0 Then
                            Dim totalTransAmount As Double = 0
                            Dim totalGSTAmount As Double = 0
                            Dim GSTAmt As Double = 0
                            Dim totalActualAmount As Double = 0

                            ListTRD = New List(Of AccountsDetailsEn)
                            For Each stu In ListTranctionDetails
                                If Not ListTRD.Any(Function(x) x.ReferenceCode = stu.ReferenceCode) Then
                                    totalGSTAmount = totalGSTAmount + stu.GSTAmount
                                    totalTransAmount = totalTransAmount + stu.TransactionAmount
                                    totalActualAmount = totalTransAmount - totalGSTAmount
                                    ListTRD.Add(New AccountsDetailsEn With {.ReferenceCode = stu.ReferenceCode, .Description = stu.Description, .TransactionAmount = totalTransAmount,
                                                                              .GSTAmount = totalGSTAmount, .TaxAmount = stu.GSTAmount, .TempAmount = totalActualAmount,
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
                    End If

                    dgView.DataSource = ListTranctionDetails
                    dgView.DataBind()

                    'If dgView.Items.Count >= 0 Then
                    '    txtTotal.Visible = True
                    '    lblTotal.Visible = True
                    'End If
                    'Session("AddFee") = ListTranctionDetails
                    'Dim mylst As New List(Of StudentEn)
                    'Dim bsobj As New AccountsBAL
                    'Dim loen As New StudentEn
                    'loen.BatchCode = txtBatchNo.Text
                    'Try
                    '    mylst = bsobj.GetListStudentSponsorInvoicebyBatchID(loen)
                    'Catch ex As Exception
                    '    LogError.Log("SponsorInvoice", "FillData", ex.Message)
                    'End Try
                    Dim bsobj As New AccountsBAL
                    Dim loen As New StudentEn
                    Dim lststud3 As New List(Of StudentEn)
                    Dim batchcode As String
                    Dim Sponsor As String
                    Sponsor = obj.CreditRefOne
                    batchcode = txtBatchNo.Text
                    'loen.BatchCode = txtBatchNo.Text
                    lststud3 = _StudentDAL.GetStudentDetailsForSponsorInvoice(batchcode, Sponsor)
                    'Try
                    '    mylst = bsobj.GetListStudentSponsorInvoicebyBatchIDForStudent(loen)
                    'Catch ex As Exception
                    '    LogError.Log("SponsorInvoice", "FillData", ex.Message)
                    'End Try

                    ibtnStatus.ImageUrl = "images/Posted.gif"
                    lblStatus.Value = "Posted"
                    ibtnAddStudent.Enabled = False
                    hfProgramCount.Value = lststud3.Count
                    dgstudent1.DataSource = lststud3
                    dgstudent1.DataBind()
                    'Changing Status
                    'lblStatus.Value = "Posted"
                    'ibtnStatus.ImageUrl = "images/Posted.gif"

                    Dim lstmatric As List(Of String)
                    lstmatric = lststud3.Select(Function(x) x.MatricNo).Distinct().ToList()
                    For Each dgItem1 In dgstudent1.Items
                        For Each objects In lstmatric
                            'If dgItem1.Cells(1).Text = obj Then
                            For Each stu In lststud3.Where(Function(x) x.MatricNo = objects And x.MatricNo = dgItem1.Cells(1).Text).ToList()
                                If stu.SponsorLimit = 0 And stu.OutstandingAmount = 0 Then
                                    dgItem1.Cells(8).Text = "-"
                                End If
                            Next

                            'End If
                        Next

                    Next
                    'hfProgramCount.Value = mylst.Count
                    'dgStudent.DataSource = mylst
                    'dgStudent.DataBind()
                    ddlSponsor.SelectedValue = obj.CreditRefOne
                    Session("LstStueObj") = lststud3


                    'pnlSponCoverLetter.Visible = True
                    PrintAble()

                    Session("batchcode") = txtBatchNo.Text
                    'Enable checkbox
                    Dim chk As CheckBox
                    Dim dgitem As DataGridItem

                    For Each dgitem In dgStudent.Items
                        chk = dgitem.Cells(0).Controls(1)
                        chk.Checked = True
                    Next
                End If
                'ddlSponsor.SelectedIndex = -1
                If mylst.Count > 0 Then
                    BindSponsor()
                    ddlSponsor.SelectedValue = mylst(0).SponsorCode
                    Session("SponsorValue") = mylst(0).SponsorCode
                    Dim sponsorDetail As New SponsorEn
                    Dim SponsorList As New List(Of SponsorEn)
                    SponsorList = Session(ReceiptsClass.SessionSponsorList)
                    If Not SponsorList Is Nothing Then
                        If SponsorList.Any(Function(x) x.SponserCode = ddlSponsor.SelectedValue.ToString()) Then
                            sponsorDetail = SponsorList.Where(Function(x) x.SponserCode = ddlSponsor.SelectedValue.ToString()).FirstOrDefault()
                        End If
                    Else
                        sponsorDetail = New SponsorEn
                    End If
                    Session("SCLSponsor") = sponsorDetail
                    If Session("PageMode") = "Edit" Then
                        ddlSponsor.Enabled = False
                        ibtnLoad.Enabled = False
                    Else
                        ddlSponsor.Enabled = True
                        ibtnLoad.Enabled = True
                    End If
                End If

                'Enable checkbox
                If dgView.Items.Count > 0 Then
                    Dim chk As CheckBox
                    For Each dgitem In dgView.Items
                        chk = dgitem.Cells(0).Controls(1)
                        chk.Checked = True
                        If lblStatus.Value = "Posted" Then
                            chk.Enabled = False
                        Else
                            chk.Enabled = True
                        End If
                    Next
                    If Not Session("auto") Is Nothing Then
                        chkSelectAll.Checked = True
                    End If
                    If lblStatus.Value = "Posted" Then
                        chkSelectAll.Enabled = False
                    Else
                        chkSelectAll.Enabled = True
                    End If
                Else
                    chkSelectAll.Checked = False
                End If
                If dgFeeType.Items.Count > 0 Then
                    Dim chk As CheckBox
                    For Each dgitem In dgFeeType.Items
                        chk = dgitem.Cells(0).Controls(1)
                        chk.Checked = True
                        If lblStatus.Value = "Posted" Then
                            chk.Enabled = False
                        Else
                            chk.Enabled = True
                        End If
                    Next
                    chkFeeType.Checked = True
                    chkFeeType.Visible = False
                    txtTotalFeeAmt.Visible = True
                    lblTotalFeeAmt.Visible = True
                    If lblStatus.Value = "Posted" Then
                        chkFeeType.Enabled = False
                    Else
                        chkFeeType.Enabled = True
                    End If

                Else
                    chkFeeType.Checked = False
                    chkFeeType.Visible = False
                    txtTotalFeeAmt.Visible = False
                    lblTotalFeeAmt.Visible = False
                End If

                If mylst.Count > 0 Then
                    Dim chk As CheckBox
                    For Each dgitem In dgStudent.Items
                        chk = dgitem.Cells(0).Controls(1)
                        If mylst.Any(Function(x) x.ProgramID = dgitem.Cells(1).Text) Then
                            chk.Checked = True
                            If lblStatus.Value = "Posted" Then
                                chk.Enabled = False
                            Else
                                chk.Enabled = True
                            End If
                        End If
                    Next
                    chkStudent.Checked = True
                    chkStudent.Visible = True
                    If lblStatus.Value = "Posted" Then
                        chkStudent.Enabled = False
                    Else
                        chkStudent.Enabled = True
                    End If
                Else
                    chkStudent.Checked = False
                    chkStudent.Visible = False
                End If

                CheckWorkflowStatus(obj)

            End If
        End If
        setDateFormat()
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
                    RecAff = bsobj.SponsorInvoiceDelete(eob)
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
                    LogError.Log("SponsorInvoice", "ondelete", ex.Message)
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
        'dgView.Columns(8).FooterText = String.Format("{0:F}", sumValue)
        DirectCast(dgView.Controls(0).Controls(dgView.Items.Count + 1).FindControl("lblFooter"), Label).Text = String.Format("{0:F}", sumValue)
        Session("FeeSum") = sumValue
    End Sub

    Private Enum dgStudentCells As Integer
        ProgramID = 1
        ProgramType = 2
        SponsorCode = 3
        CreditRef = 4
        MatricNo = 5
        StudentName = 6
        CurrentSemester = 7
        SponsorLimit = 8
        OutstandingAmount = 9
    End Enum
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
                eobjstu.MatricNo = dgitem.Cells(dgStudentCells.MatricNo).Text
                eobjstu.StudentName = dgitem.Cells(dgStudentCells.StudentName).Text
                eobjstu.ProgramID = dgitem.Cells(dgStudentCells.ProgramID).Text
                eobjstu.CurrentSemester = dgitem.Cells(dgStudentCells.CurrentSemester).Text
                eobjstu.SponsorCode = dgitem.Cells(dgStudentCells.SponsorCode).Text
                eobjstu.SponsorLimit = dgitem.Cells(dgStudentCells.SponsorLimit).Text
                eobjstu.OutstandingAmount = dgitem.Cells(dgStudentCells.OutstandingAmount).Text
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
    Private Sub PrintAble()
        Dim isPrint As Boolean = Session("IsPrint")
        ibtnPrint.Enabled = isPrint
        If isPrint = True Then
            'Print button will enable on POSTED record only
            ibtnPrint.Enabled = True
            ibtnPrint.ImageUrl = "images/print.png"
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
    ''' Method to Load the UserRights
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub LoadUserRights()
        Dim obj As New UsersBAL
        Dim eobj As New UserRightsEn

        Try
            eobj = obj.GetUserRights(CInt(Request.QueryString("Menuid")), CInt(Session("UserGroup")))
        Catch ex As Exception
            LogError.Log("SponsorInvoice", "LoadUserRights", ex.Message)
        End Try

        'Rights for Add
        If eobj.IsAdd = True Then
            onAdd()
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
            ddlSponsor.Enabled = False
            ibtnLoad.Enabled = False
            ibtnSave.ImageUrl = "images/save.png"
            ibtnSave.ToolTip = "Edit"
            If eobj.IsAdd = False Then
                ibtnSave.Enabled = False
                ibtnSave.ImageUrl = "images/gsave.png"
                ibtnSave.ToolTip = "Access Denied"
            End If

            Session("EditFlag") = True

        Else
            ddlSponsor.Enabled = True
            ibtnLoad.Enabled = True
            Session("EditFlag") = False
            ibtnSave.Enabled = False
            ibtnSave.ImageUrl = "images/gsave.png"
        End If
        'Rights for View
        ibtnView.Enabled = eobj.IsView
        If eobj.IsView = True Then
            ibtnView.ImageUrl = "images/ready.png"
            ibtnView.Enabled = True
        Else
            ibtnView.ImageUrl = "images/ready.png"
            ibtnView.Enabled = True
            'ibtnView.ToolTip = "Access Denied"
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
        Session("IsPrint") = eobj.IsPrint
        'ibtnPrint.Enabled = eobj.IsPrint
        'If eobj.IsPrint = True Then
        '    ''Print button will enable on POSTED record only
        '    'ibtnPrint.Enabled = True
        '    'ibtnPrint.ImageUrl = "images/print.png"
        '    'ibtnPrint.ToolTip = "Print"
        'Else
        '    ibtnPrint.Enabled = False
        '    ibtnPrint.ImageUrl = "images/gprint.png"
        '    ibtnPrint.ToolTip = "Access Denied"
        'End If
        If eobj.IsOthers = True Then
            ibtnOthers.Enabled = True
            ibtnOthers.ImageUrl = "images/post.png"
            ibtnOthers.ToolTip = "Others"
        Else
            ibtnOthers.Enabled = False
            ibtnOthers.ImageUrl = "images/post.png"
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
    Private Sub onSave()
        Dim eobj As New AccountsEn
        Dim bsobj As New AccountsBAL
        Dim LstTRDetails As New List(Of AccountsDetailsEn)
        Dim listStu As New List(Of StudentEn)
        Dim Status As String
        lblMsg.Text = ""
        lblMsg.Visible = True


        eobj.Category = "Invoice"
        Status = "O"
        eobj.TransType = "Debit"

        If dgView.Items.Count <> 0 Then

            ' CreateListObjStudent()

            eobj.BatchDate = Trim(txtBatchDate.Text)
            eobj.Description = Trim(txtDesc.Text)
            eobj.TransDate = Trim(txtInvoiceDate.Text)
            eobj.DueDate = Trim(txtDuedate.Text)
            eobj.SponsorID = Trim(Session("SponsorValue").ToString()) ' Trim(ddlSponsor.SelectedValue)
            eobj.SubType = "Sponsor"
            eobj.TransStatus = "Open"
            eobj.PostedDateTime = DateTime.Now
            eobj.UpdatedTime = DateTime.Now
            eobj.ChequeDate = DateTime.Now
            eobj.CreatedBy = Session("User")
            eobj.CreatedDateTime = DateTime.Now
            eobj.PostStatus = "Ready"
            eobj.BatchCode = Trim(txtBatchNo.Text)
            eobj.TransactionAmount = Trim(txtTotal.Text)
            eobj.BatchIntake = Trim(ddlIntake.SelectedValue)
            eobj.AccountDetailsList = Session("AddFee")
            eobj.UpdatedBy = Session("User")

            If Not Session("lstStu") Is Nothing Then
                listStu = Session("lstStu")
            Else
                listStu = Nothing
            End If
            Dim bid As String = ""
            'Saving
            If Session("PageMode") = "Add" Then
                Try

                    txtBatchNo.ReadOnly = False
                    If Not Session("LstStuWithFeeType") Is Nothing Then
                        listStu = Session("LstStuWithFeeType")
                        txtBatchNo.Text = bsobj.SponsorInvoiceInsert(eobj, listStu, True)
                    Else
                        txtBatchNo.Text = bsobj.SponsorInvoiceInsert(eobj, listStu)
                    End If

                    ErrorDescription = "Record Saved Successfully"
                    ' Update the Student OutStanding Table 

                    ibtnStatus.ImageUrl = "images/ready.gif"
                    lblStatus.Value = "Ready"
                    lblMsg.Text = ErrorDescription
                    ibtnSave.Enabled = False
                Catch ex As Exception
                    lblMsg.Text = ex.Message.ToString()
                    LogError.Log("SponsorInvoice", "onSave", ex.Message)
                End Try
                'Updating
            ElseIf Session("PageMode") = "Edit" Then
                eobj.BatchCode = Trim(txtBatchNo.Text)
                Try
                    If Not Session("LstStuWithFeeType") Is Nothing Then
                        listStu = Session("LstStuWithFeeType")
                        txtBatchNo.Text = bsobj.SponsorInvoiceUpdate(eobj, listStu, True)
                    Else
                        txtBatchNo.Text = bsobj.SponsorInvoiceUpdate(eobj, listStu)
                    End If
                    ErrorDescription = "Record Updated Successfully"
                    ' Update the Student OutStanding Table 

                    ibtnStatus.ImageUrl = "images/ready.gif"
                    lblStatus.Value = "Ready"
                    lblMsg.Text = ErrorDescription

                Catch ex As Exception
                    lblMsg.Text = ex.Message.ToString()
                    LogError.Log("SponsorInvoice", "onSave", ex.Message)
                End Try

            End If
        Else
            lblMsg.Text = "Please Enter At least one Feetype"
        End If
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
            LogError.Log("SponsorInvoice", "SponsorGrid", ex.Message)
        End Try
        DgSponsor.DataSource = list
        DgSponsor.DataBind()
    End Sub

    ''' <summary>
    ''' Method to load the Sponsors in Grid
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub BindSponsor()
        Dim obj As New SponsorBAL
        Dim list As New List(Of SponsorEn)
        Dim eob As New SponsorEn

        ddlSponsor.Items.Clear()
        ddlSponsor.Items.Add(New System.Web.UI.WebControls.ListItem("--Select--", "-1"))
        ddlSponsor.DataTextField = "Name"
        ddlSponsor.DataValueField = "SponserCode"

        Try
            list = obj.GetList(eob)
            Session(ReceiptsClass.SessionSponsorList) = list
        Catch ex As Exception
            LogError.Log("SponsorInvoice", "SponsorGrid", ex.Message)
        End Try
        ddlSponsor.DataSource = list
        ddlSponsor.DataBind()

    End Sub

    ''' <summary>
    ''' Method to Load all the Grids in Selection Panel
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub selection()
        ' programGrid()
        SponsorGrid()
        ' HostelGrid()
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
    ''' Method to Clear the Field Values
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub onClearData()
        Session("ListObj") = Nothing
        DisableRecordNavigator()
        Session("AddFee") = Nothing
        txtBatchNo.Text = ""
        txtBatchNo.ReadOnly = False
        txtBatchNo.Enabled = False
        ddlIntake.Enabled = True
        ddlIntake.SelectedValue = "-1"
        txtBatchDate.Text = ""
        ibtnStatus.ImageUrl = "images/notready.gif"
        lblStatus.Value = "New"
        txtInvoiceDate.Text = ""
        txtDesc.Text = ""
        txtDuedate.Text = ""
        Session("lstStu") = Nothing
        Session("liststu") = Nothing
        Session("LstStueObj") = Nothing
        ''added by farid on 29062016
        dgstudent1.DataSource = Nothing
        dgstudent1.DataBind()
        'end added
        dgStudent.DataSource = Nothing
        dgStudent.DataBind()
        dgView.DataSource = Nothing
        dgView.DataBind()

        chkSelectSponsor.Checked = False
        'chkStudent.Checked = False
        'If dgView.Items.Count <= 0 Then
        '    txtTotal.Visible = False
        '    lblTotal.Visible = False
        'End If
        If dgFeeType.Items.Count <= 0 Then
            txtTotalFeeAmt.Visible = False
            lblTotalFeeAmt.Visible = False
        End If
        LoadBatchInvoice()
        'FixEmptyRow(dgStudent, column)
        ddlSponsor.SelectedValue = "-1"
        Session("LstStueObj") = Nothing
        Session("liststu") = Nothing
        Session("ListObj") = Nothing
        Session("ProgSelected") = Nothing
        Session("LstStuWithFeeType") = Nothing
        Session("auto") = Nothing
        btnLoadFeeType.Visible = False
        Session(ReceiptsClass.SessionSponsorList) = New List(Of SponsorEn)
        dgFeeType.DataSource = Nothing
        dgFeeType.DataBind()
        chkFeeType.Checked = False
        chkFeeType.Visible = False
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
        onClearData()
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
    Private Function OnPost() As Boolean
        Dim result As Boolean = False
        Dim eobj As New AccountsEn
        Dim bsobj As New AccountsBAL
        Dim LstTRDetails As New List(Of AccountsDetailsEn)
        Dim listStu As New List(Of StudentEn)
        Dim Status As String
        Dim bid As String = ""
        lblMsg.Text = ""
        lblMsg.Visible = True

        eobj.Category = "Invoice"
        Status = "O"
        'eobj.TransType = "Credit"
        eobj.TransType = "Debit"

        If dgView.Items.Count <> 0 Then
            'CreateListObjStudent()
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
            eobj.SubType = "Sponsor"
            eobj.TransStatus = "Open"
            eobj.PostedBy = Session("User")
            eobj.PostedDateTime = DateTime.Now
            eobj.UpdatedTime = DateTime.Now
            eobj.ChequeDate = DateTime.Now
            eobj.CreatedDateTime = DateTime.Now
            'eobj.MatricNo = Trim(txtMatrciNo.Text)
            'eobj.StudentName = Trim(txtName.Text)
            'eobj.ProgramID = Trim(txtProgrammeName.Text)
            eobj.PostStatus = "Ready"
            eobj.UpdatedBy = Session("User")
            If Not Session("lstStu") Is Nothing Then
                listStu = Session("lstStu")
            Else
                Dim stu As New StudentEn()
                Dim _AccountsDAL As New HTS.SAS.DataAccessObjects.AccountsDAL
                stu.BatchCode = txtBatchNo.Text
                listStu = _AccountsDAL.GetListStudentSponsorInvoicebyBatchID1(stu)
            End If
            Try
                Dim isAutoDetails As Boolean = False
                If Not Session("auto") Is Nothing Then
                    If Session("auto") = "auto" Then
                        isAutoDetails = True
                        listStu = Session("LstStuWithFeeType")
                    End If
                End If

                txtBatchNo.Text = bsobj.SponsorInvoiceUpdate(eobj, listStu, isAutoDetails)
                'If chkSponCoverLetter.Checked = True Then
                '    Dim ListSCL As New List(Of SponsorCoverLetterEn)
                '    ListSCL = Session("ListObj_LetterDetails")
                '    Dim result As SponsorCoverLetterEn = ListSCL.Find(Function(x) x.Code = ddlSponCoverLetter.SelectedValue)
                '    SendSponCoverLetter(result)
                'End If
                'ErrorDescription = "Record Posted Successfully "
                lblMsg.Text = ErrorDescription
                ibtnStatus.ImageUrl = "images/posted.gif"
                'lblStatus.Value = "Posted"
                'eobj.PostStatus = "Posted"
                lblMsg.Visible = True
                result = True
                If Not Session("ListObj") Is Nothing Then
                    ListObjects = Session("ListObj")
                    ListObjects(CInt(txtRecNo.Text) - 1) = eobj
                    Session("ListObj") = ListObjects
                    'If lblStatus.Value = "Posted" Then
                    '    ibtnStatus.ImageUrl = "images/posted.gif"
                    '    lblStatus.Value = "Posted"
                    '    'pnlSponCoverLetter.Visible = True
                    '    PrintAble()
                    '    Session("batchcode") = txtBatchNo.Text
                    'End If
                    'commented on 09032016 by farid
                End If
            Catch ex As Exception
                lblMsg.Text = ex.Message.ToString()
                LogError.Log("SponsorInvoice", "OnPost", ex.Message)
            End Try

        Else
            lblMsg.Text = "Please Enter At least one Fee Type"
        End If

        Return result

    End Function

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

        If CInt(Request.QueryString("IsView")).Equals(1) Then
            txtBatchDate.Enabled = False
            txtInvoiceDate.Enabled = False
            txtDuedate.Enabled = False
            ibtnAddFeeType.Attributes.Clear()
            ibtnRemoveFee.Attributes.Clear()
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

#End Region

    Protected Overloads Sub LoadFields()
        trPrint.Visible = False
        addIntake()
    End Sub

    Protected Sub chkSelectSponsor_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkSelectSponsor.CheckedChanged
        SponsorGrid()
    End Sub

    Protected Sub btnHidden_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnHidden.Click

    End Sub

    Protected Sub ibtnLoad_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ibtnLoad.Click
        Dim objup As New StudentBAL
        Dim i As Integer = 0
        Dim lstobjects As New List(Of StudentEn)
        Dim lststudentnew As New List(Of StudentEn)
        Dim eob As New StudentEn
        Dim sem As Integer = 0
        Dim lstSponsorShip As New List(Of StudentEn)
        'Dim lstSponsorShip1 As New List(Of StudentEn)
        btnLoadFeeType.Visible = True
        dgStudent.Visible = False

        Dim j As Integer = 0
        Dim dgItem1 As DataGridItem
        Dim tTotalAmount As Double = 0
        Dim tSponsorLimit As Double = 0
        Dim tAllocatedAmount As Double = 0
        Dim tOutstandingAmount As Double = 0
        Dim sumamountstudent1 As Double = 0
        Dim Student As New StudentEn
        ''clear all the session if sponsor changed
        Session(ReceiptsClass.SessionStuToSave) = New List(Of StudentEn)
        Session("LstStueObj") = New List(Of StudentEn)
        Session("SponsorShip") = New List(Of StudentEn)
        Session("SponsorValue") = ddlSponsor.SelectedValue
        'inSponsor()
        eob.CurrentSemester = 0
        eob.STsponsercode = New StudentSponEn
        eob.STsponsercode.Sponsor = ddlSponsor.SelectedValue
        eob.ProgramID = String.Empty
        eob.SAKO_Code = String.Empty
        eob.Faculty = String.Empty
        eob.CategoryCode = String.Empty
        eob.StCategoryAcess = New StudentCategoryAccessEn
        eob.StCategoryAcess.MenuID = Session("Menuid")

        Try
            lstobjects = objup.GetlistStudentByStudentWithValidity(eob)
            lstSponsorShip = objup.GetStudentSponsorship("", ddlSponsor.SelectedValue)
            lstSponsorShip = lstSponsorShip.Where(Function(x) x.OutstandingAmount > 0 Or x.SponsorLimit = 0).Select(Function(x) x).ToList()
        Catch ex As Exception
            LogError.Log("SponsorInvoice", "ibtnLoad_Click", ex.Message)
        End Try

        pnlView.Visible = True

        ''Adding in the exisiting list
        'Dim mylst As List(Of StudentEn)
        'If Not Session("LstStueObj") Is Nothing Then
        '    Dim Flag As Boolean
        '    mylst = Session("LstStueObj")
        '    mylst = lstobjects
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
        'If lstSponsorShip.Count > 0 Then

        '    lstSponsorShip1.AddRange(lstSponsorShip.Where(Function(y) Not lstSponsorShip.Any(Function(z) z.MatricNo = y.MatricNo)).Select(Function(x) x).ToList())
        'End If
        'dgStudent.DataSource = lstobjects
        'dgStudent.DataBind()
        ' Session("LstStueObj") = lstobjects ''remarked - Session("LstStueObj") is to store the program not studentlist
        Session("SponsorShip") = lstSponsorShip
        Dim saveAllStu As New List(Of StudentEn)
        'save all the student to session by default
        If lstSponsorShip.Count > 0 Then
            saveAllStu.AddRange(lstSponsorShip.Where(Function(y) Not saveAllStu.Any(Function(z) z.MatricNo = y.MatricNo)).Select(Function(x) New StudentEn With {.ProgramID = x.ProgramID, .SponsorCode = x.SponsorCode, .MatricNo = x.MatricNo,
                                                                                     .StudentName = x.StudentName, .CurrentSemester = x.CurrentSemester,
                                                                                     .AllocatedAmount = x.PaidAmount, .OutstandingAmount = x.OutstandingAmount,
                                                                                     .Internal_Use = x.ProgramID, .ICNo = x.ICNo, .TaxId = x.TaxId,
                                                                                      .ProgramName = x.ProgramName, .CurretSemesterYear = x.CurretSemesterYear,
                                                                                      .CategoryCode = x.CategoryCode, .SponsorLimit = x.SponsorLimit, .TransactionAmount = x.TransactionAmount, .TaxAmount = x.GSTAmount, .GSTAmount = x.GSTAmount, .ReferenceCode = x.ReferenceCode, .Description = x.Description
                                                                                    }))
        End If
        Session("LstStueObjFromDB") = saveAllStu
        Session("LstStueObj") = saveAllStu

        Dim sponsorfee As New List(Of StudentEn)
        sponsorfee = objup.GetSponsorFeeList(ddlSponsor.SelectedValue)

        'Dim chk As CheckBox
        'Dim dgitem As DataGridItem
        If sponsorfee.Count = 0 Then

        Else
            lstSponsorShip = lstSponsorShip.Where(Function(x) sponsorfee.Any(Function(y) y.ReferenceCode = x.ReferenceCode)).ToList()
        End If

        dgstudent1.DataSource = saveAllStu
        dgstudent1.DataBind()
        MultiView1.SetActiveView(View3)
        chkStudentall.Visible = True
        dgstudent1.Visible = True
        'While j < lstSponsorShip.Count

        Dim sumamountstudent As Double = 0
        Dim lstmatric As List(Of String)
        lstmatric = saveAllStu.Select(Function(x) x.MatricNo).Distinct().ToList()
        For Each dgItem1 In dgstudent1.Items
            For Each obj In lstmatric
                'If dgItem1.Cells(1).Text = obj Then
                For Each stu In lstSponsorShip.Where(Function(x) x.MatricNo = obj And x.MatricNo = dgItem1.Cells(1).Text).ToList()

                    sumamountstudent = sumamountstudent + stu.TransactionAmount
                    eob.TempAmount = sumamountstudent
                    eob.TempAmount = String.Format("{0:F}", eob.TempAmount)
                    dgItem1.Cells(3).Text = String.Format("{0:F}", eob.TempAmount)
                    'saveAllStu.AddRange(saveAllStu
                    If stu.SponsorLimit = 0 And stu.AllocatedAmount = 0 Then
                        dgItem1.Cells(8).Text = "-"
                    End If
                Next

                'End If
            Next
            sumamountstudent = 0
        Next
        'Dim l As Double = 0

        'While j < saveAllStu.Count
        '    For Each dgItem1 In dgstudent1.Items

        '    Next
        '    l = l + 1
        '    'LoadTotals()
        'End While
        'dgstudent1.DataSource = lstSponsorShip
        'dgstudent1.DataBind()
        ''End If
        'If lstSponsorShip.Count > 0 Then
        '    dgstudent1.DataSource = lstSponsorShip
        '    dgstudent1.DataBind()
        '    If dgstudent1.Items.Count > 0 Then
        '        dgstudent1.SelectedIndex = dgstudent1.Items.Count - 1
        '    End If
        'End If
        hfStudentCount.Value = saveAllStu.Count
        Session("sstr") = ""
        Session("prgstr") = ""
        Session("spnstr") = ""
        If Not lstobjects Is Nothing Then
            OnViewStudentGrid()
        Else

        End If
        hfProgramCount.Value = lstobjects.Count
        'Enable checkbox - start
        If lstobjects.Count > 0 Then
            chkStudentall.Visible = True
            btnLoadFeeType.Visible = True
            chkStudentall.Checked = True
            Dim chk As CheckBox
            Dim dgitem As DataGridItem
            If chkStudentall.Checked = True Then
                For Each dgitem In dgstudent1.Items
                    chk = dgitem.Cells(0).Controls(1)
                    chk.Checked = True
                Next
            Else
                For Each dgitem In dgstudent1.Items
                    chk = dgitem.Cells(0).Controls(1)
                    chk.Checked = False
                Next
            End If
            hfProgramCount.Value = lstobjects.Count
        Else
            chkStudentall.Visible = False
            btnLoadFeeType.Visible = False
            chkStudentall.Checked = False
        End If
        'Enable checkbox - end
    End Sub


    Protected Sub dgStudent_ItemCommand(source As Object, e As DataGridCommandEventArgs) Handles dgStudent.ItemCommand

        Dim chek As New CheckBox
        Dim dgItem1 As DataGridItem
        Dim LinkButton As New HyperLink
        Dim i As Integer = 0
        For Each dgItem1 In dgStudent.Items
            chek = dgItem1.Cells(0).Controls(1)
            chek.Checked = True
            'LinkButton = dgItem1.Cells(8).Controls(1)
            'LinkButton.Attributes.Add("onClick", "OpenWindow1('about:blank')")
            'link.NavigateUrl = "FeeStructure.aspx?Menuid=16&Formid=FS&IsStudentLedger=1&ProgramId=" & dgItem1.Cells(1).Text & "&Semester=" & dgItem1.Cells(5).Text

            'LinkButton.Attributes.Add("onclick", "javascript:new_window=window.open('FeeStructure.aspx?Menuid=16&Formid=FS IsStudentLedger=1 & ProgramId=" & dgItem1.Cells(4).Text & "&Semester=" & dgItem1.Cells(5).Text & "','Hanodale','width=520,height=600,resizable=0');new_window.focus();")
            LinkButton.Attributes.Add("onclick", "javascript:new_window=window.open('FeeStructure.aspx?Menuid=16&Formid=FS IsStudentLedger=1 & ProgramId=" & dgItem1.Cells(4).Text & "&Semester=" & dgItem1.Cells(5).Text & "&BatchCode=" & txtBatchNo.Text.Trim() & "','Hanodale','width=520,height=600,resizable=0');new_window.focus();")
            LinkButton.Target = "MyPopup"
        Next


    End Sub
#Region "GST Calculation - Starting "
    Protected Sub txtFeeAmount_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs)

        'control declaration
        Dim dgitem As DataGridItem
        Dim _txtFeeAmount As TextBox
        Dim _txtGSTAmount As TextBox
        Dim _txtTotFeeAmount As TextBox
        Dim _txtActFeeAmount As TextBox

        'varaible declaration
        Dim FeeAmount As Double, GSTAmt As Double, ActAmout As Double
        Dim TaxMode As String
        Try

            TaxMode = _GstSetupDal.GetGstDetails(MaxGeneric.clsGeneric.NullToInteger(0)).Tables(0).Rows(0)(3).ToString()

            'GST Calculation - Stating
            For Each dgitem In dgView.Items
                _txtFeeAmount = dgitem.FindControl("txtFeeAmount")
                _txtGSTAmount = dgitem.FindControl("txtGSTAmount")
                _txtTotFeeAmount = dgitem.FindControl("txtTotFeeAmount")
                _txtActFeeAmount = dgitem.FindControl("txtActFeeAmount")

                FeeAmount = _txtFeeAmount.Text
                GSTAmt = _GstSetupDal.GetGstAmount(0, MaxGeneric.clsGeneric.NullToDecimal(FeeAmount))


                If (TaxMode = Generic._TaxMode.Inclusive) Then
                    ActAmout = MaxGeneric.clsGeneric.NullToDecimal(FeeAmount) - GSTAmt
                ElseIf (TaxMode = Generic._TaxMode.Exclusive) Then
                    ActAmout = FeeAmount
                    FeeAmount = MaxGeneric.clsGeneric.NullToDecimal(FeeAmount) + GSTAmt
                End If

                _txtActFeeAmount.Text = String.Format("{0:F}", ActAmout)
                _txtGSTAmount.Text = String.Format("{0:F}", GSTAmt)
                _txtTotFeeAmount.Text = String.Format("{0:F}", FeeAmount)
                sumAmt = sumAmt + FeeAmount
            Next
            txtTotal.Text = sumAmt

            'GST Calculation - Ended
        Catch ex As Exception
            If ex.Message = "There is no row at position 0." Then
                lblMsg.Visible = True
                lblMsg.Text = "Required Tax Type"
            End If
            Call MaxModule.Helper.LogError(ex.Message)
        End Try
    End Sub

    Public Function GSTActAmt(ByVal Amt As Double, ByVal gst As Double, ByVal TaxId As Integer, ByVal ReferenceCode As String) As String
        Dim ActAmout As Double = 0
        Dim TaxMode As Integer = _GstSetupDal.GetGstDetails(MaxGeneric.clsGeneric.NullToInteger(TaxId)).Tables(0).Rows(0)(3).ToString()
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
#End Region


#Region "Load Taxtype "
    Public Sub LoadTaxType()
        'ddlTaxType.DataSource = _GstSetupDal.GetGstDetails(0)

        'ddlTaxType.DataTextField = "sas_taxtype"
        'ddlTaxType.DataValueField = "sas_taxid"
        'ddlTaxType.DataBind()
        'ddlTaxType.Items.Add(New ListItem("--Select--", "-1"))
        'ddlTaxType.SelectedValue = -1
    End Sub
#End Region

    Protected Sub chkSelectAll_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)

        Dim dgItem1 As DataGridItem
        Dim chkselect As CheckBox

        For Each dgItem1 In dgView.Items
            chkselect = dgItem1.Cells(0).Controls(1)
            If chkSelectAll.Checked = False Then
                chkselect.Checked = False
            Else
                chkselect.Checked = True
            End If
        Next
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

    Private Enum dgViewCell As Integer
        CheckBox = 0
        MatricNo = 1
        StudentName = 2
        ProgramID = 3
        ReferenceCode = 4
        Description = 5
        Fee_Amount = 6
        TransactionAmount = 7
        Fee_Code = 8
        Actual_Fee_Amount = 9
        GSTAmount = 10
        Total_Fee_Amount = 11
        Priority = 12
        TaxId = 13
    End Enum

    'Protected Sub chkProgram_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)
    '    If Not Session("lstStu") Is Nothing Then
    '        listStu = Session("lstStu")
    '    Else
    '        listStu = New List(Of StudentEn)
    '    End If
    '    If Not Session(ReceiptsClass.SessionStuToSave) Is Nothing Then
    '        StuToSave = Session(ReceiptsClass.SessionStuToSave)
    '    Else
    '        StuToSave = New List(Of StudentEn)
    '    End If

    '    Dim chk As CheckBox = (DirectCast(sender, CheckBox))
    '    Dim dgitem As DataGridItem = DirectCast(chk.Parent.Parent, DataGridItem)
    '    'Dim chkSelectAll As CheckBox = DirectCast(dgView.FindControl("chkSelectAll"), CheckBox)  ' dgView.Controls(0).FindControl("chkSelectAll")
    '    If listStu.Count > 0 Then
    '        If chk.Checked Then
    '            If Not StuToSave.Any(Function(x) x.ProgramID = dgitem.Cells(dgStudentCells.ProgramID).Text) Then
    '                StuToSave.AddRange(listStu.Where(Function(x) x.ProgramID = dgitem.Cells(dgStudentCells.ProgramID).Text).ToList())
    '            End If
    '        Else
    '            StuToSave.RemoveAll(Function(x) x.ProgramID = dgitem.Cells(dgStudentCells.ProgramID).Text)
    '            If StuToSave.Count = 0 Then
    '                chkSelectAll.Checked = False
    '            End If
    '        End If
    '    End If
    '    Session(ReceiptsClass.SessionStuToSave) = StuToSave
    'End Sub

    Protected Sub btnLoadFeeType_Click(sender As Object, e As EventArgs) Handles btnLoadFeeType.Click
        Dim stuBAL As New StudentBAL
        Dim stuList As New List(Of StudentEn)
        Dim mylst As New List(Of StudentEn)
        Dim mylst1 As New List(Of StudentEn)
        Dim sponsorship As New List(Of StudentEn)
        Dim sponsorfee As New List(Of StudentEn)
        Dim objup As New StudentBAL
        Dim eob As New StudentEn
        'checkProgWithStu()
        If Not Session("LstStueObj") Is Nothing Then

            mylst1 = Session("LstStueObj")
            'mylst.Distinct()
        Else
            mylst = New List(Of StudentEn)
        End If

        If mylst1.Count > 0 Then
            mylst.AddRange(mylst1.Where(Function(y) Not mylst.Any(Function(z) z.MatricNo = y.MatricNo)).Select(Function(x) New StudentEn With {.ProgramID = x.ProgramID, .SponsorCode = x.SponsorCode, .MatricNo = x.MatricNo,
                                                                                     .StudentName = x.StudentName, .CurrentSemester = x.CurrentSemester,
                                                                                     .AllocatedAmount = x.AllocatedAmount, .OutstandingAmount = x.OutstandingAmount,
                                                                                     .Internal_Use = x.ProgramID, .ICNo = x.ICNo, .TaxId = x.TaxId,
                                                                                      .ProgramName = x.ProgramName, .CurretSemesterYear = x.CurretSemesterYear,
                                                                                      .CategoryCode = x.CategoryCode, .SponsorLimit = x.SponsorLimit, .TransactionAmount = x.TransactionAmount, .TaxAmount = x.GSTAmount, .ReferenceCode = x.ReferenceCode, .Description = x.Description
                                                                                    }))
        End If

        If hfStudentCount.Value = 0 Or mylst.Count = 0 Then
            lblMsg.Text = "Please select at least one student"
            lblMsg.Visible = True
            Exit Sub
        End If
        'If hfProgramCount.Value = 0 Then
        '    lblMsg.Text = "Please select at least one program"
        '    lblMsg.Visible = True
        '    Exit Sub
        'Else
        '    If hfStudentCount.Value = 0 Or mylst.Count = 0 Then
        '        lblMsg.Text = "Please select at least one student"
        '        lblMsg.Visible = True
        '        Exit Sub
        '    End If
        'End If
        If Not Session("SponsorShip") Is Nothing Then
            sponsorship = Session("SponsorShip")
        Else
            sponsorship = New List(Of StudentEn)
        End If

        'If mylst.Count > 0 Then
        '    sponsorship.AddRange(mylst.Where(Function(y) Not mylst.Any(Function(z) z.MatricNo = y.MatricNo)).Select(Function(x) New StudentEn With {.ProgramID = x.ProgramID, .SponsorCode = x.SponsorCode, .MatricNo = x.MatricNo,
        '                                                                             .StudentName = x.StudentName, .CurrentSemester = x.CurrentSemester,
        '                                                                             .AllocatedAmount = x.PaidAmount, .OutstandingAmount = x.OutstandingAmount,
        '                                                                             .Internal_Use = x.ProgramID, .ICNo = x.ICNo, .TaxId = x.TaxId,
        '                                                                              .ProgramName = x.ProgramName, .CurretSemesterYear = x.CurretSemesterYear,
        '                                                                              .CategoryCode = x.CategoryCode, .SponsorLimit = x.SponsorLimit, .TransactionAmount = x.TransactionAmount, .TaxAmount = x.GSTAmount, .ReferenceCode = x.ReferenceCode, .Description = x.Description
        '                                                                            }))
        'End If

        'Session("SponsorShip") = sponsorship
        'If sponsorship.Count > 0 Then
        '    stuList.AddRange(sponsorship.Where(Function(y) Not stuList.Any(Function(z) z.MatricNo = y.MatricNo And z.ReferenceCode = y.ReferenceCode)).Select(Function(x) New StudentEn With {.ProgramID = x.ProgramID, .SponsorCode = x.SponsorCode, .MatricNo = x.MatricNo,
        '                                                                             .StudentName = x.StudentName, .CurrentSemester = x.CurrentSemester,
        '                                                                             .AllocatedAmount = x.PaidAmount, .OutstandingAmount = x.OutstandingAmount,
        '                                                                             .Internal_Use = x.ProgramID, .ICNo = x.ICNo, .TaxId = x.TaxId,
        '                                                                              .ProgramName = x.ProgramName, .CurretSemesterYear = x.CurretSemesterYear,
        '                                                                              .CategoryCode = x.CategoryCode, .SponsorLimit = x.SponsorLimit, .TransactionAmount = x.TransactionAmount, .TaxAmount = x.GSTAmount, .ReferenceCode = x.ReferenceCode, .Description = x.Description
        '                                                                            }))
        'End If
        'Dim chk As CheckBox
        'Dim dgitem As DataGridItem
        'For Each dgitem In dgStudent.Items
        '    chk = dgitem.Cells(0).Controls(1)
        '    If chk.Checked = True Then
        '        stuList.AddRange(sponsorship.Where(Function(x) x.ProgramID = dgitem.Cells(1).Text))
        '    End If
        'Next
        Dim chk As CheckBox
        Dim dgitem As DataGridItem
        For Each dgitem In dgstudent1.Items
            chk = dgitem.Cells(0).Controls(1)
            If chk.Checked = True Then
                stuList.AddRange(sponsorship.Where(Function(x) x.MatricNo = dgitem.Cells(1).Text))
            End If
        Next
        'stuList.Remove(Function(x) Not mylst.Any(Function(y)  y.ProgramID  )))
        sponsorfee = objup.GetSponsorFeeList(ddlSponsor.SelectedValue)

        'Dim chk As CheckBox
        'Dim dgitem As DataGridItem
        If sponsorfee.Count = 0 Then

        Else
            stuList = stuList.Where(Function(x) sponsorfee.Any(Function(y) y.ReferenceCode = x.ReferenceCode)).ToList()
        End If
        'Dim stuList2 As List(Of StudentEn)
        'stuList2 = mylst.Where(Function(x) stuList.Any(Function(y) y.MatricNo = x.MatricNo)).ToList()
        stuList = stuList.Where(Function(x) mylst.Any(Function(y) y.ProgramID = x.ProgramID And y.MatricNo = x.MatricNo) And x.ReferenceCode <> Nothing).ToList()
        stuList = stuList.Where(Function(x) mylst.Any(Function(y) y.ProgramID = x.ProgramID And y.MatricNo = x.MatricNo) And (x.FullySponsor = True Or x.SponFeeCode = x.ReferenceCode)).ToList()

        Session(ReceiptsClass.SessionStuToSave) = stuList
        'Dim test As List(Of StudentEn) = mylst.Where(Function(x) Not stuList.Any(Function(y) y.MatricNo = x.MatricNo)).ToList()
        If mylst.Any(Function(x) Not stuList.Any(Function(y) y.MatricNo = x.MatricNo)) Or stuList.Count = 0 Then
            lblMsg.Text = "Some of the student's program does not map to any fee type, please add fee type manually"
            lblMsg.Visible = True
            'Exit Sub
        End If
        'newStuList.Any(Function(x) Not StuChgMatricNo.Any(Function(y) y.MatricNo = x.MatricNo))
        dgstudent1.DataSource = mylst
        dgstudent1.DataBind()
        Dim sumamountstudent As Double = 0
        Dim lstmatric As List(Of String)
        lstmatric = mylst.Select(Function(x) x.MatricNo).Distinct().ToList()
        For Each dgItem1 In dgstudent1.Items
            For Each obj In lstmatric
                'If dgItem1.Cells(1).Text = obj Then
                For Each stu In stuList.Where(Function(x) x.MatricNo = obj And x.MatricNo = dgItem1.Cells(1).Text).ToList()

                    sumamountstudent = sumamountstudent + stu.TransactionAmount
                    eob.TempAmount = sumamountstudent
                    eob.TempAmount = String.Format("{0:F}", eob.TempAmount)
                    dgItem1.Cells(3).Text = String.Format("{0:F}", eob.TempAmount)
                    If stu.SponsorLimit = 0 And stu.AllocatedAmount = 0 Then
                        dgItem1.Cells(8).Text = "-"
                    End If
                Next

                'End If
            Next
            sumamountstudent = 0
        Next

        AddStudColumnDgView()
        If stuList.Count > 0 Then
            dgView.DataSource = stuList
            dgView.DataBind()
        Else
            dgView.DataSource = Nothing
            dgView.DataBind()
        End If

        LoadBatchInvoice()
        Dim studistinct As New StudentEn
        ''GROUPING FEE CODE - START
        If stuList.Count > 0 Then
            Dim totalTransAmount As Double = 0, totalTransAmount1 As Double = 0, totalTransAmount2 As Double = 0
            Dim totalGSTAmount As Double = 0, totalGSTAmount1 As Double = 0, totalGSTAmount2 As Double = 0
            Dim totalActualFeeAmount As Double = 0, totalActualFeeAmount1 As Double = 0, totalActualFeeAmount2 As Double = 0
            'Dim saveAllStu As New List(Of StudentEn)
            'save all the student to session by default
            ListTRD = New List(Of AccountsDetailsEn)
            'ListTRD.AddRange(stuList.Where(Function(y) Not ListTRD.Any(Function(z) z.ReferenceCode = y.ReferenceCode)).Select(Function(x) New AccountsDetailsEn With {.ICNo = x.MatricNo}))



            Dim lstReferenceCode As List(Of String)
            lstReferenceCode = stuList.Select(Function(x) x.ReferenceCode).Distinct().ToList()
            Dim lstmatricno As List(Of String)
            lstmatricno = stuList.Select(Function(x) x.MatricNo).Distinct().ToList()
            'For Each obj In lstmatricno
            '    For Each stu In mylst.Where(Function(x) x.MatricNo = obj).ToList()
            For Each obj In lstReferenceCode
                For Each stu In stuList.Where(Function(x) x.ReferenceCode = obj).ToList()
                    'Feetype = stu.Feetype
                    totalGSTAmount = 0
                    totalTransAmount = 0
                    totalTransAmount1 = 0
                    totalActualFeeAmount = 0
                    If Not ListTRD.Any(Function(x) x.ReferenceCode = stu.ReferenceCode) Then
                        'If Not stuList.Any(Function(x) x.MatricNo = stu.MatricNo) Then

                        totalGSTAmount = totalGSTAmount + stu.GSTAmount
                        totalTransAmount = totalTransAmount + stu.TransactionAmount
                        totalActualFeeAmount = totalTransAmount - totalGSTAmount
                        ListTRD.Add(New AccountsDetailsEn With {.ReferenceCode = stu.ReferenceCode, .Description = stu.Description, .TransactionAmount = totalTransAmount,
                                                                  .GSTAmount = totalGSTAmount, .TaxAmount = stu.GSTAmount, .TempAmount = totalActualFeeAmount,
                                                                .TempPaidAmount = stu.TransactionAmount, .TaxId = stu.TaxId, .StudentQty = 1})

                        '    'End If
                        '    'Else
                        '    'End If
                        'Else
                        'End If
                    Else
                        Dim assignNewTotal As AccountsDetailsEn = ListTRD.Where(Function(x) x.ReferenceCode = stu.ReferenceCode).FirstOrDefault()
                        assignNewTotal.TransactionAmount = assignNewTotal.TransactionAmount + stu.TransactionAmount
                        assignNewTotal.GSTAmount = assignNewTotal.GSTAmount + stu.GSTAmount
                        assignNewTotal.TempAmount = assignNewTotal.TransactionAmount - assignNewTotal.GSTAmount
                        assignNewTotal.StudentQty = assignNewTotal.StudentQty + 1

                    End If
                    'End If
                    'totalGSTAmount = totalGSTAmount1 + totalGSTAmount2
                    'totalTransAmount = totalTransAmount2 + totalTransAmount2
                    'totalActualFeeAmount = totalActualFeeAmount1 + totalActualFeeAmount2
                Next
                'totalTransAmount1 = totalTransAmount1 + totalTransAmount
                'txtAmountforStudent.Text = totalTransAmount1
            Next
            Session("AddFee") = ListTRD
            dgFeeType.DataSource = ListTRD
            dgFeeType.DataBind()
            If ListTRD.Count > 0 Then
                chkFeeType.Checked = True
                chkFeeType.Visible = False
            Else
                chkFeeType.Checked = False
                chkFeeType.Visible = False
            End If
        Else
            dgFeeType.DataSource = Nothing
            dgFeeType.DataBind()
            chkFeeType.Checked = False
            chkFeeType.Visible = False
        End If
        ''GROUPING FEE CODE - END

        'Dim chk As CheckBox
        'Dim dgitem As DataGridItem
        If stuList.Count > 0 Then
            chkSelectAll.Checked = True
            chkStudentall.Checked = True
            'Enable checkbox
            For Each dgitem In dgView.Items
                chk = dgitem.Cells(0).Controls(1)
                chk.Checked = True
            Next

            For Each dgitem In dgFeeType.Items
                chk = dgitem.Cells(0).Controls(1)
                chk.Checked = True
            Next
        Else
            chkSelectAll.Checked = False
        End If
        'chkSelectAll.Checked = False
        chkFeeType.Visible = False
        Session("auto") = "auto"
    End Sub

    ''' <summary>
    ''' Method to Remove ProgramId in the session which is not selected
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub checkProgWithStu()
        Dim LstStueObj As New List(Of StudentEn)

        If Not Session("LstStueObj") Is Nothing Then
            LstStueObj = Session("LstStueObj")
        Else
            LstStueObj = New List(Of StudentEn)
        End If
        Dim chk As CheckBox
        Dim dgitem As DataGridItem
        For Each dgitem In dgStudent.Items
            chk = dgitem.Cells(0).Controls(1)
            If chk.Checked = False Then
                LstStueObj.RemoveAll(Function(x) x.ProgramID = dgitem.Cells(1).Text)
            End If
        Next

        Session("LstStueObj") = LstStueObj
        hfStudentCount.Value = LstStueObj.Count

    End Sub

    ''' <summary>
    ''' Method to Add the List of Students
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub addStudentList()
        Dim ListObjectsStu As List(Of StudentEn)
        Dim mylist As List(Of StudentEn)

        If Not Session("LstStueObjFromDB") Is Nothing Then
            ListObjectsStu = Session("LstStueObjFromDB")
        Else
            ListObjectsStu = New List(Of StudentEn)
        End If
        mylist = Session("lstStu")
        'Checking for the Exisiting Students in the Grid
        If mylist.Count > 0 Then
            ListObjectsStu.RemoveAll(Function(x) x.ProgramID = mylist.FirstOrDefault().ProgramID)
            ListObjectsStu.AddRange(mylist)
        Else
            If Not Session("ProgSelected") Is Nothing Then
                ListObjectsStu.RemoveAll(Function(x) x.ProgramID = Session("ProgSelected").ToString())
            End If
        End If

        'If ListObjectsStu.Count = 0 Then
        '    chkStudent.Visible = False
        '    btnLoadFeeType.Visible = False
        'Else
        '    chkStudent.Visible = True
        '    btnLoadFeeType.Visible = True
        'End If
        Session("LstStueObj") = ListObjectsStu
        Session("LstStueObjFromDB") = ListObjectsStu
        hfStudentCount.Value = ListObjectsStu.Count
        Session("lstStu") = Nothing
    End Sub

    Private Sub AddStudColumnDgView()
        Dim fieldMatricNo As BoundColumn = DirectCast(dgView.Columns(dgViewCell.MatricNo), BoundColumn)
        Dim fieldName As BoundColumn = DirectCast(Me.dgView.Columns(dgViewCell.StudentName), BoundColumn)
        Dim fieldProgramID As BoundColumn = DirectCast(Me.dgView.Columns(dgViewCell.ProgramID), BoundColumn)
        fieldMatricNo.DataField = "MatricNo"
        fieldName.DataField = "StudentName"
        fieldProgramID.DataField = "ProgramID"
        dgView.Columns(dgViewCell.MatricNo).Visible = True
        dgView.Columns(dgViewCell.StudentName).Visible = True
        dgView.Columns(dgViewCell.CheckBox).Visible = True
        ' chkSelectAll.Visible = True

    End Sub

    Private Sub ClearSession()
        Dim fieldMatricNo As BoundColumn = DirectCast(dgView.Columns(dgViewCell.MatricNo), BoundColumn)
        Dim fieldName As BoundColumn = DirectCast(Me.dgView.Columns(dgViewCell.StudentName), BoundColumn)
        Dim fieldProgramID As BoundColumn = DirectCast(Me.dgView.Columns(dgViewCell.ProgramID), BoundColumn)
        fieldMatricNo.DataField = ""
        fieldName.DataField = ""
        fieldProgramID.DataField = ""
        dgView.Columns(dgViewCell.MatricNo).Visible = False
        dgView.Columns(dgViewCell.StudentName).Visible = False
        dgView.Columns(dgViewCell.CheckBox).Visible = False
        dgView.DataBind()
        ' chkSelectAll.Visible = False
        Session("LstStuWithFeeType") = Nothing
        Session(ReceiptsClass.SessionStuToSave) = Nothing
        StuToSave = New List(Of StudentEn)
        Session("LstStueObj") = Nothing
        Session("SponsorShip") = Nothing
        Session("SponsorValue") = "-1"
    End Sub

    ''' <summary>
    ''' Method to Create List of FeeType 
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub CreateListObjFeeType()
        Dim chk As CheckBox
        Dim dgitem As DataGridItem
        Dim lstStu As New List(Of StudentEn)
        Dim mylst As New List(Of StudentEn)
        Dim tSponsorLimit As Double = 0
        Dim tAllocatedAmount As Double = 0
        Dim tOutstandingAmount As Double = 0
        Dim tTotalAmount As Double = 0
        Dim sumamountstudent As Double = 0
        Dim eob As New StudentEn
        If Not Session(ReceiptsClass.SessionStuToSave) Is Nothing Then
            StuToSave = Session(ReceiptsClass.SessionStuToSave)
        Else
            StuToSave = New List(Of StudentEn)
        End If
        If Not Session("AddFee") Is Nothing Then
            ListTRD = Session("AddFee")
        Else
            ListTRD = New List(Of AccountsDetailsEn)
        End If
        If Not Session("LstStueObj") Is Nothing Then
            mylst = Session("LstStueObj")
        Else
            mylst = New List(Of StudentEn)
        End If

        For Each dgitem In dgFeeType.Items
            chk = dgitem.Cells(0).Controls(1)
            Dim ReferenceCode As String = dgFeeType.DataKeys(dgitem.ItemIndex).ToString()
            If chk.Checked = True Then
                'lstStu.RemoveAll(Function(x) x.ReferenceCode = ReferenceCode)
                lstStu.AddRange(StuToSave.Where(Function(y) y.ReferenceCode = ReferenceCode).ToList())
            Else
                ListTRD.RemoveAll(Function(x) x.ReferenceCode = ReferenceCode)
            End If
        Next
        lstStu.ForEach(Sub(x) x.Internal_Use = x.ProgramID)
        'kk()
        '    Next
        'Next
        'lstStu.RemoveAll(Function(y) Not ListTRD.Any(Function(z) z.ReferenceCode <> y.ReferenceCode))
        Session("LstStuWithFeeType") = lstStu


    End Sub


    'Private Sub LoadSponCoverLetter()
    '    Dim bobj As New SponsorCoverLetterBAL
    '    Dim eobj As New SponsorCoverLetterEn
    '    Dim ListObjects As New List(Of SponsorCoverLetterEn)
    '    eobj.Code = String.Empty
    '    eobj.Title = String.Empty
    '    ddlSponCoverLetter.Items.Clear()
    '    ddlSponCoverLetter.DataTextField = "Code"
    '    ddlSponCoverLetter.DataValueField = "Code"

    '    Try
    '        ListObjects = bobj.GetList(eobj)
    '        Session("ListObj_LetterDetails") = ListObjects
    '    Catch ex As Exception
    '        LogError.Log("SponsorInvoice", "LoadSponCoverLetter", ex.Message)
    '    End Try

    '    ddlSponCoverLetter.DataSource = ListObjects
    '    ddlSponCoverLetter.DataBind()
    '    ddlSponCoverLetter.SelectedIndex = 0
    '    Session("SCLCode") = ListObjects.Find(Function(x) x.Code = ddlSponCoverLetter.SelectedValue)
    'End Sub

    Private Sub SendSponCoverLetter(ByVal value As SponsorCoverLetterEn)
        'Variable Declarations - Start
        Dim mainHtml As String = String.Empty
        Dim pdfHtml As String = String.Empty

        Dim headerBodyHtml As String = String.Empty
        Dim studentName As String = String.Empty
        Dim studentICNo As String = String.Empty
        Dim programName As String = String.Empty
        Dim studentCurrentSemesterYr As String = String.Empty
        Dim address1 As String = String.Empty
        Dim address2 As String = String.Empty
        Dim city As String = String.Empty
        Dim dueAmount As String = String.Empty
        Dim state As String = String.Empty
        Dim pincode As String = String.Empty
        Dim chkselect As CheckBox
        Dim toMail As String
        Dim SMatricNo As Integer = 0
        Dim senderMail As String = String.Empty
        Dim MailTo As Integer = 0, MailFrom As Integer = 0
        Dim FileId As Integer = 0, MailSubject As String = Nothing
        Dim FileName As String = Nothing
        Dim MailBody As String = Nothing
        'Variable Declarations - Stop

        Dim bsobj As New SponsorCoverLetterBAL
        Dim eobj As New SponsorCoverLetterEn
        Dim RecAff As Integer = 0
        eobj = value
        eobj.UpdatedBy = Session("User")
        eobj.UpdatedTime = Date.Now.ToString()


        If Not (ConfigurationManager.AppSettings("MailGroup")) Is Nothing Then
            senderMail = ConfigurationManager.AppSettings("MailGroup").ToString()
        End If


        'Delete all the files 
        Try
            Dim s As String
            For Each s In System.IO.Directory.GetFiles(Server.MapPath(_Helper.GetSponsorCoverLetterFolder))
                System.IO.File.Delete(s)
            Next s
        Catch

        End Try
        Dim sponsorDetail As New SponsorEn
        Dim SponsorList As New List(Of SponsorEn)
        SponsorList = Session(ReceiptsClass.SessionSponsorList)
        If Not SponsorList Is Nothing Then
            If SponsorList.Any(Function(x) x.SponserCode = ddlSponsor.SelectedValue.ToString()) Then
                sponsorDetail = SponsorList.Where(Function(x) x.SponserCode = ddlSponsor.SelectedValue.ToString()).FirstOrDefault()
            End If
        Else
            sponsorDetail = New SponsorEn
        End If
        Try
            Dim BatchMailBody As String = ""
            toMail = sponsorDetail.Email
            'Dim results = persons.GroupBy(Function(p) p.PersonId, Function(p) p.car, Function(key, g) New With { _
            '    Key .PersonId = key, _
            '    Key .Cars = g.ToList() _
            '})
            ' Items.Select(Function(i As MyItem) New Order(i.OrderID)) _
            '.GroupBy(Function(o As Order) New Order(o.OrderID)) _
            '.OrderBy(Function(o As Order) o.DateOrdered).ToList()

            'Dim oResult = PersonList _
            '    .GroupBy(Function(v) New With {Key v.City, Key v.Country}) _
            '    .Where(Function(grp) grp.Count > 1).ToList()

            If Not Session(ReceiptsClass.SessionStuToSave) Is Nothing Then
                StuToSave = Session(ReceiptsClass.SessionStuToSave)
            Else
                StuToSave = New List(Of StudentEn)
            End If
            If StuToSave.Count > 0 Then
                Dim groupMatricNo As List(Of String) = StuToSave.Select(Function(x) x.MatricNo).Distinct().ToList()
                For Each i In groupMatricNo
                    SMatricNo = i
                    Dim newlistStu As List(Of StudentEn) = StuToSave.Where(Function(x) x.MatricNo = i).ToList()
                    studentName = newlistStu.FirstOrDefault().StudentName
                    studentICNo = newlistStu.FirstOrDefault().ICNo
                    programName = newlistStu.FirstOrDefault().ProgramName
                    Dim FeeTable As String = String.Empty
                    Dim TotalAmount As Double = 0
                    If newlistStu.Count > 0 Then
                        FeeTable = "<table><thead style=""background-color:lightgrey; ""><tr><th>KETERANGAN</th><th>JUMLAH (RM)</th></tr></thead>"
                        For Each J In newlistStu
                            FeeTable += " <tr><td>" + J.Description + "</td><td>" + String.Format(J.TransactionAmount, "{0:F}") + "</td></tr>"
                            TotalAmount += J.TransactionAmount
                        Next J
                        FeeTable += "<tfoot style=""background-color:lightgrey""><tr><td>JUMLAH KESELURUHAN</td><td>" + String.Format(TotalAmount, "{0:F}") + "</td></tr></tfoot></table>"
                    End If

                    'Build Mail Subject - Start
                    MailSubject = "TUNTUTAN YURAN PENGAJIAN PELAJAR IJAZAH LANJUTAN DI UNIVERSITI PUTRA MALAYSIA"
                    'Build Mail Subject - Stop

                    'Build Mail Body - Start
                    MailBody = _MiddleHelper.GetFileContents(_Helper.GetSponsorCoverLetterPath)
                    MailBody = String.Format(MailBody,
                                             eobj.OurRef, eobj.YourRef,
                                             Date.Now.ToShortDateString(), eobj.Address.ToString(), studentName, SMatricNo,
                                             studentICNo, programName, studentCurrentSemesterYr, FeeTable, eobj.SignBy, eobj.Name)
                    BatchMailBody += MailBody
                Next i
            End If

            If BatchMailBody <> "" Then
                FileName = "/" + ddlSponsor.SelectedValue.ToString() + "_" + Date.Now.ToString("ddMMyyyy") + "_SponsorCoverLetter.pdf"
                CreatePdf(FileName, BatchMailBody, BatchMailBody, String.Format("{0:F}", dueAmount).ToString(), eobj.SignBy, eobj.Name)
            End If
            If toMail <> String.Empty Then
                'Send Mail Messages
                Dim isMailSent As Boolean = SendMessage(FileName, Trim(MailSubject), BatchMailBody, senderMail, toMail)
                If isMailSent Then
                    'lblSponCoverLetterMsg.Text = "The Mail has been sent Successfully"
                Else
                    'lblSponCoverLetterMsg.Text = "The Mail fail to send"
                End If
            Else
                ' lblSponCoverLetterMsg.Text = "The Sponsor do not have email"
            End If



        Catch smtpex As SmtpException
            MaxModule.Helper.LogError(smtpex.Message)
            ' lblSponCoverLetterMsg.Text = smtpex.Message.ToString
        Catch ex As Exception
            MaxModule.Helper.LogError(ex.Message)
            ' lblSponCoverLetterMsg.Text = ex.Message.ToString
        End Try

    End Sub

    Private Function FindID(ByVal bk As SponsorCoverLetterEn) As Boolean
        If bk.Code = "-1" Then ' ddlSponCoverLetter.SelectedValue Then
            Return True
        Else
            Return False
        End If
    End Function
    Public Function SendMessage(ByVal fileName As String, ByVal subject As String, ByVal messageBody As String, ByVal fromAddress As String, ByVal toAddress As String) As Integer
        Dim MailSettings As SmtpSection = DirectCast(ConfigurationManager.GetSection("system.net/mailSettings/smtp"), SmtpSection)

        Dim theMailMessage As New MailMessage()
        'Dim strUserName As String = "syafiqfathyamer@gmail.com"
        Dim strUserName As String = MailSettings.Network.UserName
        Dim strPassword As String = MailSettings.Network.Password

        Try
            With theMailMessage
                'specify your file location            
                Dim attachFile As Attachment = New Attachment(Server.MapPath(_Helper.GetSponsorCoverLetterFolder) + fileName)

                .From = New MailAddress(MailSettings.From)
                .To.Add(New MailAddress(toAddress.ToString()))
                .Subject = subject
                .Body = messageBody
                .IsBodyHtml = True
                .Attachments.Add(attachFile)
            End With

            'E-Mail Credentials and Sending
            'Dim theClient As SmtpClient = New SmtpClient("smtp.gmail.com", 587)
            Dim theClient As SmtpClient = New SmtpClient()
            theClient.Host = MailSettings.Network.Host
            theClient.Port = MailSettings.Network.Port
            theClient.Timeout = 30000
            theClient.UseDefaultCredentials = False
            theClient.EnableSsl = True

            Dim theCredential As System.Net.NetworkCredential = New System.Net.NetworkCredential(strUserName, strPassword)
            theClient.Credentials = theCredential

            'Send the email
            theClient.Send(theMailMessage)
            Return True

        Catch smtpex As SmtpException
            MaxModule.Helper.LogError(smtpex.Message)
            Return False
        Catch ex As System.Exception
            MaxModule.Helper.LogError(ex.Message)
            Return False
        End Try

    End Function

    Private Sub CreatePdf(ByVal fileName As String, ByVal mainHtml As String, ByVal pdfHtml As String, ByVal dueAmount As String, ByVal SignBy As String, ByVal Name As String)
        Try
            Dim mydoc As New Document(PageSize.A4, 20, 20, 20, 20)
            Dim cb As iTextSharp.text.pdf.PdfContentByte = Nothing
            Dim writer As iTextSharp.text.pdf.PdfWriter = Nothing

            writer = PdfWriter.GetInstance(mydoc, New FileStream(Server.MapPath(_Helper.GetSponsorCoverLetterFolder) + fileName, FileMode.Create))

            mydoc.Open()
            cb = writer.DirectContent

            cb.AddTemplate((PdfFooter(cb, SignBy, Name)), 0, 0)

            cb.MoveTo(20, 537)
            cb.LineTo(mydoc.PageSize.Width - 20, 537)
            cb.Stroke()


            Dim myhtmlworker As New simpleparser.HTMLWorker(mydoc)
            myhtmlworker.Parse(New StringReader(pdfHtml))
            mydoc.Close()

        Catch ex As Exception
            MaxModule.Helper.LogError(ex.Message)
        End Try

    End Sub
    Private Function PdfFooter(ByVal cb As PdfContentByte, ByVal SignBy As String, ByVal Name As String) As PdfTemplate
        ' Create the template and assign height
        Dim tmpFooter As PdfTemplate = cb.CreateTemplate(580, 400)
        ' Move to the bottom left corner of the template
        tmpFooter.MoveTo(1, 1)
        ' Place the footer content
        tmpFooter.Stroke()
        ' Begin writing the footer
        tmpFooter.BeginText()
        ' Set the font and size
        Dim f_cn As BaseFont = BaseFont.CreateFont("c:\\windows\\fonts\\arial.ttf", BaseFont.CP1252, BaseFont.NOT_EMBEDDED)
        tmpFooter.SetFontAndSize(f_cn, 11)
        ' Write out details from the payee table
        ' tmpFooter.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "Regards:", 20, 550, 0)

        'If rbNew.Checked Then
        '    tmpFooter.ShowTextAligned(PdfContentByte.ALIGN_LEFT, txtSignBy.Text.Trim(), 20, 315, 0)
        '    tmpFooter.ShowTextAligned(PdfContentByte.ALIGN_LEFT, txtName.Text.Trim(), 20, 300, 0)
        'Else
        tmpFooter.ShowTextAligned(PdfContentByte.ALIGN_LEFT, SignBy, 20, 315, 0)
        tmpFooter.ShowTextAligned(PdfContentByte.ALIGN_LEFT, Name, 20, 300, 0)
        'End If

        tmpFooter.EndText()
        ' Stamp a line above the page footer
        cb.SetLineWidth(1)
        cb.MoveTo(20, 350)
        cb.LineTo(580, 350)
        cb.Stroke()
        ' Return the footer template
        Return tmpFooter
    End Function

    Protected Sub chkprogram_checkedchanged(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim LstStueObj As New List(Of StudentEn)
        'If Not Session("LstStueObj") Is Nothing Then
        '    LstStueObj = Session("LstStueObj")
        'End If

        Dim ListObjectsStu As List(Of StudentEn)
        If Not Session("LstStueObjFromDB") Is Nothing Then
            ListObjectsStu = Session("LstStueObjFromDB")
        Else
            ListObjectsStu = New List(Of StudentEn)
        End If

        'Enable checkbox
        Dim chk As CheckBox
        Dim dgitem As DataGridItem
        For Each dgitem In dgStudent.Items
            chk = dgitem.Cells(0).Controls(1)
            If chk.Checked = True Then
                LstStueObj.AddRange(ListObjectsStu.Where(Function(x) x.ProgramID = dgitem.Cells(1).Text).ToList())
            Else
                If chkStudent.Checked = True Then
                    chkStudent.Checked = False
                End If
            End If
        Next
        hfProgramCount.Value = LstStueObj.Select(Function(x) x.ProgramID).Distinct().Count()
        hfStudentCount.Value = LstStueObj.Count()
        Session("LstStueObj") = LstStueObj

    End Sub

    Protected Sub chkStudents_checkedchanged(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim LstStueObj As New List(Of StudentEn)
        'If Not Session("LstStueObj") Is Nothing Then
        '    LstStueObj = Session("LstStueObj")
        'End If

        Dim ListObjectsStu As List(Of StudentEn)
        If Not Session("LstStueObjFromDB") Is Nothing Then
            ListObjectsStu = Session("LstStueObjFromDB")
        Else
            ListObjectsStu = New List(Of StudentEn)
        End If

        'Enable checkbox
        Dim chk As CheckBox
        Dim dgitem As DataGridItem
        For Each dgitem In dgstudent1.Items
            chk = dgitem.Cells(0).Controls(1)
            If chk.Checked = True Then
                LstStueObj.AddRange(ListObjectsStu.Where(Function(x) x.MatricNo = dgitem.Cells(1).Text).ToList())
            Else
                If chkStudent.Checked = True Then
                    chkStudent.Checked = False
                End If
            End If
        Next
        'hfProgramCount.Value = LstStueObj.Select(Function(x) x.ProgramID).Distinct().Count()
        If hfStudentCount.Value = LstStueObj.Count() Then
            chkStudentall.Checked = True
        ElseIf hfStudentCount.Value > LstStueObj.Count() Then
            chkStudentall.Checked = False
        End If

        hfStudentCount.Value = LstStueObj.Count()

        Session("LstStueObj") = LstStueObj

    End Sub

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

#Region "GetApprovalDetails"

    Protected Function GetMenuId() As Integer

        Dim MenuId As Integer = New MenuDAL().GetMenuMasterList().Where(Function(x) x.PageName = "Sponsor Invoice").Select(Function(y) y.MenuID).FirstOrDefault()
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
