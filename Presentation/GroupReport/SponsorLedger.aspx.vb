Imports HTS.SAS.Entities
Imports HTS.SAS.BusinessObjects
Imports System.Data
Imports System.Collections.Generic
Partial Class SponsorLedger
    Inherits System.Web.UI.Page
    Dim SpEn As New SponsorEn
    ''Private LogErrors As LogError

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack() Then

            ''Loading User Rights
            LoadUserRights()
            ''while loading list object make it nothing
            Session("ListObj") = Nothing
            ''load PageName
            Menuname(CInt(Request.QueryString("Menuid")))
            Session("ReceiptFor") = "Sp"
            'Clear the Sessions
            Session("eobjstu") = Nothing
            Session("liststu") = Nothing
            Session("eobjspn") = Nothing
            Session("Menuid") = Request.QueryString("Menuid")
            lblCredit.Visible = False
            lblDebit.Visible = False
            txtCreditAmt.Visible = False
            txtDebitAmt.Visible = False
            lblOut.Visible = False
            txtOutAmt.Visible = False
            lblMsg.Text = ""
            ibtnSpn1.Attributes.Add("onclick", "new_window=window.open('../AddMulStudents.aspx?cat=Sp','Hanodale','width=520,height=600,resizable=1,scrollbars=1');new_window.focus();")

            'added by Hafiz @ 29/2/2016
            ibtnPrint.Attributes.Add("onclick", "return dllValues()")

        End If
        If Not Session("eobjstu") Is Nothing Then
            addStuCode()
        End If
        If Not Session("eobjspn") Is Nothing Then
            addspnCOde()
        End If
        txtDate.Text = Format(CDate(Date.Today), "dd/MM/yyyy")
    End Sub

    Protected Sub ibtnCancel_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ibtnCancel.Click
        dgInvoices.DataSource = Nothing
        dgInvoices.DataBind()
        txtStudentCode.Text = ""
        txtStuName.Text = ""
        lblMsg.Text = ""
        'Added by Zoya @ 26/02/2016
        txtCreditAmt.Text = ""
        txtDebitAmt.Text = ""
        txtOutAmt.Text = ""
        'End Added by Zoya @ 26/02/2016
        lblCredit.Visible = False
        lblDebit.Visible = False
        txtCreditAmt.Visible = False
        txtDebitAmt.Visible = False
        lblOut.Visible = False
        txtOutAmt.Visible = False
    End Sub

    Protected Sub btnHidden_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnHidden.Click
        'If Not Session("eobjstu") Is Nothing Then
        '    addStuCode()
        'End If
        'If Not Session("eobjspn") Is Nothing Then
        '    addspnCOde()
        'End If
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
            LogError.Log("SponsorLedger", "Menuname", ex.Message)
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

        Try
            eobj = obj.GetUserRights(CInt(Request.QueryString("Menuid")), CInt(Session("UserGroup")))

        Catch ex As Exception
            LogError.Log("SponsorLedger", "LoadUserRights", ex.Message)
        End Try
        'Rights for Add

        If eobj.IsAdd = True Then
            'ibtnSave.Enabled = True
            'OnAdd()
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
        ibtnPrevs.Enabled = False
        ibtnPrevs.ImageUrl = "images/gnew_Prev.png"
        ibtnFirst.Enabled = False
        ibtnFirst.ImageUrl = "images/gnew_first.png"
        ibtnNext.Enabled = False
        ibtnNext.ImageUrl = "images/gnew_next.png"
        ibtnLast.Enabled = False
        ibtnLast.ImageUrl = "images/gnew_last.png"
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
    ''' Method to Add Sponsors
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub addspnCOde()
        SpEn = Session("eobjspn")
        txtStudentCode.Text = SpEn.SponserCode
        txtStuName.Text = SpEn.Name
        txtDate.Text = Date.Now
        Session("eobjspn") = Nothing
        lblMsg.Text = ""
        LoadInvoiceGrid()
    End Sub
    ''' <summary>
    ''' Method to Add Students
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub addStuCode()
        Dim eobjf As StudentEn
        eobjf = Session("eobjstu")
        txtStudentCode.Text = eobjf.MatricNo
        txtStuName.Text = eobjf.StudentName
        txtPg.Text = eobjf.ProgramID
        Session("eobjstu") = Nothing
        LoadInvoiceGrid()
        lblCredit.Visible = True
        lblDebit.Visible = True
        txtCreditAmt.Visible = True
        txtDebitAmt.Visible = True
        lblOut.Visible = True
        txtOutAmt.Visible = True

    End Sub
    ''' <summary>
    ''' Method to Load Invoice Grid
    ''' </summary>
    ''' <remarks></remarks>
	''' Modified by Hafiz @ 09/3/2016
	
    Private Sub LoadInvoiceGrid()
        Dim eob As New AccountsEn
        Dim ListInvObjects As New List(Of AccountsEn)
        Dim obj As New AccountsBAL
        eob.CreditRef = txtStudentCode.Text
        eob.PostStatus = "Posted"
        eob.SubType = "Sponsor"
        eob.TransType = ""
        eob.TransStatus = ""

        'added by Hafiz @ 29/2/2016
        Dim sponsorCode As String
        sponsorCode = txtStudentCode.Text
        If sponsorCode = "" Then
        Else
            Session("sponsorCode") = sponsorCode
        End If
        'end

        ListInvObjects = obj.GetStudentLedgerList(eob)
        If ListInvObjects.Count = 0 Then
            dgInvoices.DataSource = Nothing
            dgInvoices.DataBind()
            lblMsg.Text = "Record did not Exist"
            lblCredit.Visible = False
            lblDebit.Visible = False
            txtCreditAmt.Visible = False
            txtDebitAmt.Visible = False
            lblOut.Visible = False
            txtOutAmt.Visible = False
        Else
            dgInvoices.DataSource = ListInvObjects
            dgInvoices.DataBind()
            Dim dgItem1 As DataGridItem
            Dim link As HyperLink
            Dim str As String
            Dim cat As String
            For Each dgItem1 In dgInvoices.Items
                link = dgItem1.Cells(5).Controls(1)
                str = dgItem1.Cells(7).Text
                cat = dgItem1.Cells(6).Text
                If cat = "Receipt" Then
                    link.NavigateUrl = "~/Receipts.aspx?Menuid=17&BatchCode=" + str + ";Sp"
                    link.Target = "MyPopup"
                    link.Attributes.Add("onClick", "OpenWindow('about:blank')")
                ElseIf cat = "Payment" Then
                    link.NavigateUrl = "~/SponsorPayments.aspx?Menuid=20&BatchCode=" + str + ""
                    link.Target = "MyPopup"
                    link.Attributes.Add("onClick", "OpenWindow('about:blank')")
                ElseIf cat = "Allocation" Then
                    link.NavigateUrl = "~/SponsorAllocation.aspx?Menuid=25&BatchCode=" + str + ""
                    link.Target = "MyPopup"
                    link.Attributes.Add("onClick", "OpenWindow('about:blank')")
                ElseIf cat = "Credit Note" Then
                    link.NavigateUrl = "~/SponsorCreditNote.aspx?Menuid=21&Formid=CN&BatchCode=" + str + ""
                    link.Target = "MyPopup"
                    link.Attributes.Add("onClick", "OpenWindow('about:blank')")
                ElseIf cat = "Debit Note" Then
                    link.NavigateUrl = "~/SponsorCreditNote.aspx?Menuid=22&Formid=DN&BatchCode=" + str + ""
                    link.Target = "MyPopup"
                    link.Attributes.Add("onClick", "OpenWindow('about:blank')")
                ElseIf cat = "Invoice" Then
                    link.NavigateUrl = "~/SponsorInvoice.aspx?Menuid=89&IsStudentLedger=1&BatchCode=" + str + ""
                    link.Target = "MyPopup"
                    link.Attributes.Add("onClick", "OpenWindow('about:blank')")
                End If

            Next
            ledgerformat()
            lblCredit.Visible = True
            lblDebit.Visible = True
            txtCreditAmt.Visible = True
            txtDebitAmt.Visible = True
            lblOut.Visible = True
            txtOutAmt.Visible = True
        End If
    End Sub
    ''' <summary>
    ''' Methos to Set the Ledger Format In Grid
    ''' </summary>
    ''' <remarks></remarks>
    ''' Modified by Hafiz @ 28/4/2016
    Private Sub ledgerformat()
        Dim TotalAmount As Double
        Dim amount As Double
        Dim dr As Double = 0
        Dim cr As Double = 0
        Dim dgItem1 As DataGridItem
        txtCreditAmt.Text = String.Format("{0:F}", 0)
        txtDebitAmt.Text = String.Format("{0:F}", 0)
        txtOutAmt.Text = String.Format("{0:F}", 0)
        For Each dgItem1 In dgInvoices.Items
            If dgItem1.Cells(8).Text = "Debit" Then
                TotalAmount = TotalAmount + CDbl(dgItem1.Cells(3).Text)

                dgItem1.Cells(4).Text = String.Format("{0:F}", TotalAmount)
                amount = dgItem1.Cells(3).Text
                dgItem1.Cells(3).Text = String.Format("{0:F}", amount) & " (+)"
                dr = dr + amount
                txtDebitAmt.Text = String.Format("{0:F}", dr)
            Else
                'Modified by Hafiz @ 17/2/2016 - Start
                If Not dgItem1.Cells(6).Text = "Receipt" Then
                    TotalAmount = TotalAmount - CDbl(dgItem1.Cells(3).Text)

                    dgItem1.Cells(4).Text = String.Format("{0:F}", TotalAmount)
                    amount = dgItem1.Cells(3).Text
                    dgItem1.Cells(3).Text = String.Format("{0:F}", amount) & " (-)"
                    cr = cr + amount
                    txtCreditAmt.Text = String.Format("{0:F}", cr)
                Else
                    TotalAmount = TotalAmount - CDbl(dgItem1.Cells(3).Text)

                    dgItem1.Cells(4).Text = String.Format("{0:F}", TotalAmount)
                    amount = dgItem1.Cells(3).Text
                    dgItem1.Cells(3).Text = String.Format("{0:F}", amount) & " (-)"
                    cr = cr + amount

                    txtCreditAmt.Text = String.Format("{0:F}", cr)
                End If
                'Modified by Hafiz @ 17/2/2016 - End
            End If
            If dgItem1.Cells(0).Text = "01/01/0001" Then
                dgItem1.Cells(0).Text = "--"
            End If
        Next
        txtOutAmt.Text = String.Format("{0:F}", CDbl(txtDebitAmt.Text) - CDbl(txtCreditAmt.Text))
    End Sub

#End Region

    'Added by Zoya @ 26/02/2016
    Protected Sub ibtnNew_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ibtnNew.Click
        dgInvoices.DataSource = Nothing
        dgInvoices.DataBind()
        txtStudentCode.Text = ""
        txtStuName.Text = ""
        lblMsg.Text = ""

        txtCreditAmt.Text = ""
        txtDebitAmt.Text = ""
        txtOutAmt.Text = ""

        lblCredit.Visible = False
        lblDebit.Visible = False
        txtCreditAmt.Visible = False
        txtDebitAmt.Visible = False
        lblOut.Visible = False
        txtOutAmt.Visible = False
    End Sub
    'End Added by Zoya @ 26/02/2016

End Class
