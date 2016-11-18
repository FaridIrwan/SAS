Imports HTS.SAS.Entities
Imports HTS.SAS.BusinessObjects
Imports HTS.SAS.DataAccessObjects
Imports System.Data
Imports System.Collections.Generic

Partial Class RptStudentLedger
    Inherits System.Web.UI.Page
    ''Private LogErrors As LogError

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        lblMsg.Text = ""
        If Not IsPostBack() Then
            ''Loading User Rights
            LoadUserRights()
            ''load PageName
            Menuname(CInt(Request.QueryString("Menuid")))
            Session("Menuid") = Request.QueryString("Menuid")
            Session("ReceiptFor") = "St"
            lblCredit.Visible = False
            lblDebit.Visible = False
            txtCreditAmount.Visible = False
            txtDebitAmount.Visible = False
            lblOut.Visible = False
            txtOutAmt.Visible = False
            lblMsg.Text = ""

            'Uncommented By Zoya @ 26/02/2016
            ibtnPrint.Attributes.Add("onclick", "return dllValues()")

            'Commented By Zoya@26/02/2016
            'ibtnBMReport.Attributes.Add("onclick", "return dllValues()")
            'ibtnEnReport.Attributes.Add("onclick", "return dllValuesEn()")

            ibtnView.Attributes.Add("onclick", "return dllValues()")
            ibtnSpn1.Attributes.Add("onclick", "return OpenPopup('../AddMulStudents.aspx?cat=St','Student Ledger','550','550')")
            'ibtnSpn1.Attributes.Add("onclick", "new_window=window.open('AddMulStudents.aspx?cat=St','Hanodale','width=520,height=500,resizable=0');new_window.focus();")
            'ibtnSpn1.Attributes.Add("onclick", "window.showModalDialog('AddMulStudents.aspx','Hanodale','width=520,height=500,resizable=0');")
            Session("eobjstu") = Nothing
            Session("LedgerType") = rdbStudentLeddger.Text
        End If
        If Not Session("eobjstu") Is Nothing Then
            addStuCode()
        End If
        'txtDate.Text = Format(CDate(Date.Today), "dd/MM/yyyy")

    End Sub

#Region "Methods"
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
            LogError.Log("StudentLedger", "Menuname", ex.Message)
        End Try
        lblMenuName.Text = eobj.MenuName
    End Sub
    ''' <summary>
    ''' Method to Load UserRights
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub LoadUserRights()
        Dim obj As New UsersBAL
        Dim eobj As New UserRightsEn

        'eobj = obj.GetUserRights(5, 1)
        Try
            eobj = obj.GetUserRights(CInt(Request.QueryString("Menuid")), CInt(Session("UserGroup")))

        Catch ex As Exception
            LogError.Log("StudentLedger", "LoadUserRights", ex.Message)
        End Try
        'Rights for Add

        If eobj.IsAdd = True Then
            'ibtnSave.Enabled = True
            'OnAdd()
            ibtnNew.ImageUrl = "../images/add.png"
            ibtnNew.Enabled = True
        Else
            ibtnNew.ImageUrl = "../images/gadd.png"
            ibtnNew.Enabled = False
            ibtnNew.ToolTip = "Access Denied"

        End If
        'Rights for Edit
        If eobj.IsEdit = True Then
            ibtnSave.Enabled = True
            ibtnSave.ImageUrl = "../images/save.png"
            ibtnSave.ToolTip = "Edit"
            If eobj.IsAdd = False Then
                ibtnSave.Enabled = False
                ibtnSave.ImageUrl = "../images/gsave.png"
                ibtnSave.ToolTip = "Access Denied"
            End If

            Session("EditFlag") = True

        Else
            Session("EditFlag") = False
            ibtnSave.Enabled = False
            ibtnSave.ImageUrl = "../images/gsave.png"
        End If
        'Rights for View
        ibtnView.Enabled = eobj.IsView
        If eobj.IsView = True Then
            ibtnView.ImageUrl = "../images/find.png"
            ibtnView.Enabled = True
        Else
            ibtnView.ImageUrl = "../images/gfind.png"
            ibtnView.ToolTip = "Access Denied"
        End If
        'Rights for Delete
        If eobj.IsDelete = True Then
            ibtnDelete.ImageUrl = "../images/delete.png"
            ibtnDelete.Enabled = True
        Else
            ibtnDelete.ImageUrl = "../images/gdelete.png"
            ibtnDelete.ToolTip = "Access Denied"
            ibtnDelete.Enabled = False
        End If

        'Commenetd and uncommenetd by Zoya @ 26/02/2016
        'Rights for Print
        ibtnPrint.Enabled = eobj.IsPrint
        If eobj.IsPrint = True Then
            'EnablePrintUserRights()
            ibtnPrint.Enabled = True
            ibtnPrint.ImageUrl = "../images/print.png"
            ibtnPrint.ToolTip = "Print"
        Else
            'DisablePrintUserRights()
            ibtnPrint.Enabled = False
            ibtnPrint.ImageUrl = "../images/gprint.png"
            ibtnPrint.ToolTip = "Access Denied"
        End If
        'End Commenetd and uncommenetd by Zoya @ 26/02/2016

        If eobj.IsOthers = True Then
            ibtnOthers.Enabled = True
            ibtnOthers.ImageUrl = "../images/others.png"
            ibtnOthers.ToolTip = "Others"
        Else
            ibtnOthers.Enabled = False
            ibtnOthers.ImageUrl = "../images/gothers.png"
            ibtnOthers.ToolTip = "Access Denied"
        End If
        If eobj.IsPost = True Then
            ibtnPosting.Enabled = True
            ibtnPosting.ImageUrl = "../images/posting.png"
            ibtnPosting.ToolTip = "Posting"
        Else
            ibtnPosting.Enabled = False
            ibtnPosting.ImageUrl = "../images/gposting.png"
            ibtnPosting.ToolTip = "Access Denied"
        End If

        ibtnPrevs.Enabled = False
        ibtnPrevs.ImageUrl = "../images/gnew_Prev.png"
        ibtnFirst.Enabled = False
        ibtnFirst.ImageUrl = "../images/gnew_first.png"
        ibtnNext.Enabled = False
        ibtnNext.ImageUrl = "../images/gnew_next.png"
        ibtnLast.Enabled = False
        ibtnLast.ImageUrl = "../images/gnew_last.png"
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
            ibtnFirst.ImageUrl = "../images/gnew_first.png"
            ibtnLast.ImageUrl = "../images/gnew_last.png"
            ibtnPrevs.ImageUrl = "../images/gnew_Prev.png"
            ibtnNext.ImageUrl = "../images/gnew_next.png"
        Else
            ibtnFirst.ImageUrl = "../images/new_last.png"
            ibtnLast.ImageUrl = "../images/new_first.png"
            ibtnPrevs.ImageUrl = "../images/new_Prev.png"
            ibtnNext.ImageUrl = "../images/new_next.png"

        End If
    End Sub
    ''' <summary>
    ''' Method to Add Students 
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub addStuCode()
        Dim eobjf As StudentEn
        Dim ObjBL As New StudentBAL
        eobjf = Session("eobjstu")
        ddlSponser.Items.Clear()
        ddlSponser.Items.Add(New ListItem("All", -1))
        'Commented by Zoya @13/04/2016
        'txtStudentCode.ReadOnly = False
        'Done commented by Zoya @13/04/2016
        txtStudentCode.Text = eobjf.MatricNo
        txtStudentCode.ReadOnly = True
        txtStuName.Text = eobjf.StudentName
        txtICNO.Text = eobjf.ICNo
        txtPg.Text = eobjf.ProgramID
        'eobjf.PostStatus = "Posted"
        Dim objStudent As New StudentEn
        objStudent = ObjBL.FetchLedgerDetails(eobjf.MatricNo)
        lblCurSem.Text = objStudent.CurretSemesterYear
        lblProgram.Text = objStudent.SourceType
        TextBox8.Text = objStudent.StatusBayaran
        If (objStudent.Hostel) Then
            lblblock.Text = objStudent.SABK_Code
            lblfloor.Text = objStudent.SASI_FloorNo
            lblroom.Text = objStudent.SART_Code
            lblkolej.Text = objStudent.SAKO_Code
        Else
            lblblock.Text = ""
            lblfloor.Text = ""
            lblroom.Text = ""
            lblkolej.Text = ""
        End If

        Dim i As Integer = 0
        While i < eobjf.CurrentSemester
            i = i + 1
            ddlSponser.Items.Add(New ListItem(i, i))
        End While
        Session("eobjstu") = Nothing
        LoadInvoiceGrid()
    End Sub
    ''' <summary>
    ''' Method to Load Grid with Student Transactions
    ''' </summary>
    ''' <remarks></remarks>
    ''' modified by Hafiz @ 27/5/2016
    Private Sub LoadInvoiceGrid()
        Dim ListInvObjects As New List(Of AccountsEn)
        Dim obj As New AccountsBAL
        Dim eob As New AccountsEn

        eob.CreditRef = txtStudentCode.Text
        eob.PostStatus = "Posted"
        eob.SubType = "Student"
        eob.TransType = ""
        'eob.TransStatus = "Closed" commented by Hafiz @ 27/5/2015
        Dim studentCode As String
        studentCode = txtStudentCode.Text
        If studentCode = "" Then
        Else
            Session("studentCode") = studentCode
        End If
        Try
            'If rdbStudentLeddger.Checked = True Then
            '    ListInvObjects = obj.GetStudentLedgerDetailList(eob)
            'Else
            '    ListInvObjects = obj.GetStudentLoanLedgerDetailList(eob)
            'End If
            ListInvObjects = obj.GetStudentLedgerCombine(eob)
        Catch ex As Exception
            LogError.Log("StudentLedger", "LoadInvoiceGrid", ex.Message)
        End Try
        If ListInvObjects.Count = 0 Then
            dgInvoices.DataSource = Nothing
            dgInvoices.DataBind()
            lblMsg.Text = "Record did not Exist"
            lblCredit.Visible = False
            lblDebit.Visible = False
            txtCreditAmount.Visible = False
            txtDebitAmount.Visible = False
            lblOut.Visible = False
            txtOutAmt.Visible = False
        Else
            dgInvoices.DataSource = ListInvObjects
            dgInvoices.DataBind()
            Dim dgItem1 As DataGridItem
            Dim link As HyperLink
            Dim str As String, cat As String, desc As String = Nothing

            'Modified by Hafiz @ 16/3/2016 - added &MatricNo param at SPA
            'Modified by Hafiz @ 17/3/2016 - added &MatricNo param at BatchInvoice
            'Modified by Hafiz @ 19/7/2016 - CIMB "View"

            For Each dgItem1 In dgInvoices.Items
                link = dgItem1.Cells(9).Controls(1)
                str = dgItem1.Cells(10).Text
                desc = dgItem1.Cells(2).Text
                cat = dgItem1.Cells(3).Text

                link.Attributes.Add("onClick", "OpenWindow('about:blank')")
                If cat = "Invoice" Then
                    link.NavigateUrl = "../BatchInvoice.aspx?Menuid=14&Formid=Inv&IsStudentLedger=1&BatchCode=" + str + "&MatricNo=" + txtStudentCode.Text + ""
                    link.Target = "MyPopup"
                ElseIf cat = "Receipt" Then
                    'modified by Hafiz @ 27/5/2016
                    'If rdbStudentLeddger.Checked = True Then
                    '    link.NavigateUrl = "../Receipts.aspx?Menuid=17&BatchCode=" + str + ";St"
                    '    link.Target = "MyPopup"
                    'Else
                    '    link.NavigateUrl = "../Receipts.aspx?Menuid=17&BatchCode=" + str + ";Sl"
                    '    link.Target = "MyPopup"
                    'End If

                    If Not desc.Contains("CIMB CLICKS") Then
                        link.NavigateUrl = "../Receipts.aspx?Menuid=17&BatchCode=" + str + ";St"
                    Else
                        link.NavigateUrl = "../Receipts.aspx?Menuid=17&BatchCode=" + str + ";St&IsCimbClicks=1"
                    End If

                    link.Target = "MyPopup"

                ElseIf cat = "Loan" Then
                    link.NavigateUrl = "../StudentLoan.aspx?Menuid=88&BatchCode=" + str + ";St"
                    link.Target = "MyPopup"
                ElseIf cat = "Payment" Then
                    link.NavigateUrl = "../Payments.aspx?Menuid=19&IsStudentLedger=1&BatchCode=" + str + ";A"
                    link.Target = "MyPopup"
                ElseIf cat = "Refund" Then
                    link.NavigateUrl = "../Payments.aspx?Menuid=19&IsStudentLedger=1&BatchCode=" + str + ";St"
                    link.Target = "MyPopup"
                ElseIf cat = "SPA" Then
                    link.NavigateUrl = "../SponsorAllocation.aspx?Menuid=25&BatchCode=" + str + "&MatricNo=" + txtStudentCode.Text + ""
                    link.Target = "MyPopup"
                    'ElseIf cat = "AFC" Then
                    '    link.NavigateUrl = "../WorkFlowStudentAccountView.aspx?Formid=FS&TransID=" + str + ""
                    '    link.Target = "MyPopup"
                ElseIf cat = "AFC" Then
                    link.NavigateUrl = "../AFCDetails.aspx?MatricNo=" + txtStudentCode.Text + "&CurSem=" + lblCurSem.Text + "&BatchCode=" + str + ""
                    link.Target = "MyPopup"
                ElseIf cat = "Credit Note" Then
                    link.NavigateUrl = "../BatchInvoice.aspx?Menuid=16&Formid=CN&IsStudentLedger=1&BatchCode=" + str + "&MatricNo=" + txtStudentCode.Text + ""
                    link.Target = "MyPopup"
                ElseIf cat = "Debit Note" Then
                    link.NavigateUrl = "../BatchInvoice.aspx?Menuid=15&Formid=DN&IsStudentLedger=1&BatchCode=" + str + "&MatricNo=" + txtStudentCode.Text + ""
                    link.Target = "MyPopup"
                ElseIf cat = "STA" Then
                    link.NavigateUrl = "../Payments.aspx?Menuid=19&IsStudentLedger=1&BatchCode=" + str + ";A"
                    link.Target = "MyPopup"
                End If

            Next
            ledgerformat()
            lblCredit.Visible = True
            lblDebit.Visible = True
            txtCreditAmount.Visible = True
            txtDebitAmount.Visible = True
            lblOut.Visible = True
            txtOutAmt.Visible = True
        End If

    End Sub

    Private Sub ledgerformat()
        'Updated by Hafiz Roslan @ 10/2/2016
        'Modified by Hafiz @ 27/5/2016
        'Include the Category = "Receipt" logic

        Dim TotalAmount As Double
        Dim amount As Double
        Dim dr As Double = 0
        Dim cr As Double = 0
        Dim dgItem1 As DataGridItem
        txtDebitAmount.Text = String.Format("{0:N}", 0)
        txtCreditAmount.Text = String.Format("{0:N}", 0)
        txtOutAmt.Text = String.Format("{0:N}", 0)

        For Each dgItem1 In dgInvoices.Items
            If dgItem1.Cells(6).Text = "Credit" Then

                TotalAmount = TotalAmount - CDbl(dgItem1.Cells(7).Text)

                dgItem1.Cells(8).Text = String.Format("{0:N}", TotalAmount)
                amount = dgItem1.Cells(7).Text
                dgItem1.Cells(7).Text = String.Format("{0:N}", amount) & "-"
                cr = cr + amount
                txtCreditAmount.Text = String.Format("{0:N}", cr)

            Else
                If Not dgItem1.Cells(3).Text = "Receipt" Then
                    TotalAmount = TotalAmount + CDbl(dgItem1.Cells(7).Text)

                    dgItem1.Cells(8).Text = String.Format("{0:N}", TotalAmount)
                    amount = dgItem1.Cells(7).Text
                    dgItem1.Cells(7).Text = String.Format("{0:N}", amount) & "+"
                    dr = dr + amount
                    txtDebitAmount.Text = String.Format("{0:N}", dr)

                Else
                    TotalAmount = TotalAmount - CDbl(dgItem1.Cells(7).Text)

                    dgItem1.Cells(8).Text = String.Format("{0:N}", TotalAmount)
                    amount = dgItem1.Cells(7).Text
                    dgItem1.Cells(7).Text = String.Format("{0:N}", amount) & "-"
                    cr = cr + amount
                    txtCreditAmount.Text = String.Format("{0:N}", cr)
                End If

            End If
            If dgItem1.Cells(0).Text = "01/01/0001" Then
                dgItem1.Cells(0).Text = "--"
            End If
        Next

        'Added by Hafiz Roslan
        'Dated: 06/01/2015

        'outstanding amount - Start
        Dim debitAmount As Double = 0.0, creditAmount As Double = 0.0

        debitAmount = CDbl(txtDebitAmount.Text)
        creditAmount = CDbl(txtCreditAmount.Text)

        'If debitAmount > creditAmount Then
        '    txtOutAmt.Text = String.Format("{0:F}", debitAmount - creditAmount)
        'Else
        '    txtOutAmt.Text = String.Format("{0:F}", creditAmount - debitAmount)
        'End If
        txtOutAmt.Text = String.Format("{0:N}", debitAmount - creditAmount)

        'updated by Hafiz Roslan @ 5/2/2016
        'autoupdate sas_studentoutstanding with current outstanding values - START
        'Modified by Hafiz Roslan @ 15/2/2016
        'Modified by Hafiz Roslan @ 27/5/2016
        'Cheeccheeecheck availability of student - add if dont have - START
        Dim _AccountsDAL As New AccountsDAL
        Dim _StudentDAL As New StudentDAL
        Dim argStudent As New StudentEn

        'assign values into the argStudent - START
        argStudent = _StudentDAL.GetItem(txtStudentCode.Text)
        'assign values into the studObj - END

        Try
            _AccountsDAL.AutoUpdateStudOutstanding(argStudent, CDbl(txtOutAmt.Text))

        Catch ex As Exception
            LogError.Log("StudentLedger", "AutoUpdate Function", ex.Message)
        End Try

        'added by Hafiz @ 31/3/2016
        'update flag - start
        If Not _StudentDAL.UpdateFlag(argStudent.MatricNo) = False Then
        End If
        'update flag - end

        'Cheeccheeecheck availability of student - add if dont have - END

        'updated by Hafiz Roslan @ 6/2/2016
        'add function to check currentsemyear and update at sas_studentoutstanding - START
        Try
            _AccountsDAL.CheckCurSemYr(txtStudentCode.Text, lblCurSem.Text)

        Catch ex As Exception
            LogError.Log("Student Ledger Report", "Check Current Semester", ex.Message)
        End Try
        'add function to check currentsemyear and update at sas_studentoutstanding - END

        'autoupdate sas_studentoutstanding with current outstanding values - END

        'outstanding amount - Stop

    End Sub


    ''' <remarks></remarks>
    Private Sub Loanledgerformat()
        Dim TotalAmount As Double
        Dim amount As Double
        Dim dr As Double = 0
        Dim cr As Double = 0
        Dim dgItem1 As DataGridItem
        txtDebitAmount.Text = String.Format("{0:N}", 0)
        txtCreditAmount.Text = String.Format("{0:N}", 0)
        txtOutAmt.Text = String.Format("{0:N}", 0)
        For Each dgItem1 In dgInvoices.Items
            If dgItem1.Cells(6).Text = "Credit" Then
                TotalAmount = TotalAmount + CDbl(dgItem1.Cells(7).Text)
                dgItem1.Cells(8).Text = String.Format("{0:N}", TotalAmount)
                amount = dgItem1.Cells(7).Text
                dgItem1.Cells(7).Text = String.Format("{0:N}", amount) & "-"
                cr = cr + amount
                txtDebitAmount.Text = -(String.Format("{0:N}", cr))
            Else
                TotalAmount = TotalAmount + CDbl(dgItem1.Cells(7).Text)
                dgItem1.Cells(8).Text = String.Format("{0:N}", TotalAmount)
                amount = dgItem1.Cells(7).Text
                dgItem1.Cells(7).Text = String.Format("{0:N}", amount) & "+"
                dr = dr + amount
                txtCreditAmount.Text = String.Format("{0:N}", dr)
            End If
            If dgItem1.Cells(0).Text = "01/01/0001" Then
                dgItem1.Cells(0).Text = "--"
            End If
        Next
        ' txtOutAmt.Text = String.Format("{0:F}", CDbl(txtDebitAmt.Text) - CDbl(txtCreditAmt.Text))

        'Added by Hafiz Roslan
        'Dated: 06/01/2015

        'outstanding amount - Start
        Dim debitAmount As Double = 0.0, creditAmount As Double = 0.0

        debitAmount = CDbl(txtDebitAmount.Text)
        creditAmount = CDbl(txtCreditAmount.Text)

        If debitAmount > creditAmount Then
            txtOutAmt.Text = String.Format("{0:N}", debitAmount - creditAmount)
        Else
            txtOutAmt.Text = String.Format("{0:N}", creditAmount - debitAmount)
        End If
        'outstanding amount - Stop

        'txtOutAmt.Text = String.Format("{0:F}", CDbl(txtDebitAmount.Text) - CDbl(txtCreditAmount.Text))

    End Sub


    ''' <summary>
    ''' Method to Clear the Field Values
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub onclearData()
        txtStudentCode.Text = ""
        txtStuName.Text = ""
        ddlSponser.SelectedIndex = "-1"
        txtICNO.Text = ""
        'txtDate.Text = ""
        lblMsg.Text = ""
        dgInvoices.DataSource = Nothing
        dgInvoices.DataBind()
        'rdbStudentLeddger.Checked = True

        'Added by Zoya @ 26/02/2016       
        txtCreditAmount.Text = ""
        txtDebitAmount.Text = ""
        txtOutAmt.Text = ""
        'End Added by Zoya @ 26/02/2016

        lblCredit.Visible = False
        lblDebit.Visible = False
        txtCreditAmount.Visible = False
        txtDebitAmount.Visible = False
        lblOut.Visible = False
        txtOutAmt.Visible = False

        'Added by Zoya @ 26/02/2016
        TextBox8.Text = ""
        lblCurSem.Text = ""
        lblkolej.Text = ""
        lblProgram.Text = ""
        lblblock.Text = ""
        lblfloor.Text = ""
        lblroom.Text = ""
        'End Added by Zoya @ 26/02/2016
    End Sub
#End Region

    Protected Sub ddlSponser_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlSponser.SelectedIndexChanged
        LoadInvoiceGrid()
    End Sub

    Protected Sub ibtnCancel_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ibtnCancel.Click
        onclearData()
    End Sub

    Protected Sub dgInvoices_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs)

    End Sub

    'Protected Sub rdbStudentLeddger_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles rdbStudentLeddger.CheckedChanged
    '    LoadInvoiceGrid()
    'End Sub

    'Protected Sub rdbStudentLoanLedger_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles rdbStudentLoanLedger.CheckedChanged
    '    LoadInvoiceGrid()
    'End Sub

    Protected Sub btnHidden_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnHidden.Click

    End Sub

    Protected Sub ibtnNew_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ibtnNew.Click
        onclearData()
    End Sub

    Protected Sub ledgerOption_CheckedChanged(sender As Object, e As EventArgs)
        Dim rb As RadioButton = (DirectCast(sender, RadioButton))
        Session("LedgerType") = rb.Text
    End Sub
End Class
