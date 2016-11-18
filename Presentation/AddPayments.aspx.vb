Imports HTS.SAS.Entities
Imports HTS.SAS.BusinessObjects
Imports System.Data
Imports System.Collections.Generic
Partial Class AddPayments
    Inherits System.Web.UI.Page
    Dim cat As String
    ''Private LogErrors As LogError
    ''' <summary>
    ''' Mtghod to Load the Grid
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub LoadGrid()
        Dim eob As New AccountsEn
        Dim dsobj As New AccountsBAL
        Dim list As New List(Of AccountsEn)

        If Not list Is Nothing Then
            eob.PostStatus = "Posted"
            eob.BatchCode = ""
            eob.TransStatus = "Open"
            If cat = "1" Then
                eob.Category = "Payment"
                eob.SubType = "Student"
                Try
                    list = dsobj.GetSPAllocationPayments(eob)
                Catch ex As Exception
                    LogError.Log("AddPayments", "LoadGrid", ex.Message)
                End Try
                dgView.DataSource = list
                dgView.DataBind()
            ElseIf cat = "St" Then
                eob.Category = "Refund"
                eob.SubType = "Student"
                Try
                    list = dsobj.GetPayments(eob)
                Catch ex As Exception
                    LogError.Log("AddPayments", "LoadGrid", ex.Message)
                End Try
                dgView.Columns(0).Visible = True
                dgView.Columns(1).Visible = False
                dgView.Columns(5).Visible = False
                dgView.Columns(3).Visible = False
                dgView.DataSource = list
                dgView.DataBind()
            ElseIf cat = "2" Then
                eob.Category = "Payment"
                eob.SubType = "Sponsor"
                Try
                    list = dsobj.GetSPAllocationPayments(eob)
                Catch ex As Exception
                    LogError.Log("AddPayments", "LoadGrid", ex.Message)
                End Try
                dgView.Columns(3).HeaderText = "Sponsor"
                dgView.DataSource = list
                dgView.DataBind()

            End If
        Else
            Response.Write("No Payments are Available")
        End If
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Session("Chequefor") Is Nothing Then
            cat = Request.QueryString("Cat")
        Else

            cat = Session("Chequefor")
        End If

        If Not IsPostBack() Then

            LoadGrid()
        End If

    End Sub
    Private Sub UpdateGrid()
        Dim cScript As String = ""
        'cScript &= "<script language=javascript>"
        'cScript &= "window.opener.location.href = window.opener.location.href;"
        'cScript &= "window.close();"
        'cScript &= "</script>"
        'Response.Write(cScript)
        cScript = "<SCRIPT LANGUAGE=""JavaScript"">window.opener.document.getElementById('ctl00_ContentPlaceHolder1_btnHidden').click();self.close();</SCRIPT>"
        ClientScript.RegisterStartupScript(Me.GetType(), "ClosePopUp", cScript)
    End Sub
    ''' <summary>
    ''' Method to Load Grid values to Session
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub laodvalues()
        Dim GBFormat As System.Globalization.CultureInfo
        GBFormat = New System.Globalization.CultureInfo("en-GB")
        If dgView.SelectedIndex <> -1 Then
            Dim eobj As New AccountsEn
            eobj.TransactionCode = dgView.DataKeys(dgView.SelectedIndex)
            eobj.TransType = dgView.Items(dgView.SelectedIndex).Cells(2).Text
            eobj.CreditRef = dgView.Items(dgView.SelectedIndex).Cells(3).Text
            eobj.TransDate = DateTime.Parse(dgView.Items(dgView.SelectedIndex).Cells(4).Text, GBFormat)
            eobj.BatchCode = dgView.Items(dgView.SelectedIndex).Cells(5).Text
            eobj.TransStatus = dgView.Items(dgView.SelectedIndex).Cells(6).Text
            eobj.CreditRefOne = dgView.Items(dgView.SelectedIndex).Cells(7).Text
            eobj.BatchDate = CDate(dgView.Items(dgView.SelectedIndex).Cells(8).Text)
            eobj.BankCode = dgView.Items(dgView.SelectedIndex).Cells(9).Text
            eobj.TranssactionID = dgView.Items(dgView.SelectedIndex).Cells(10).Text
            eobj.Category = dgView.Items(dgView.SelectedIndex).Cells(11).Text
            eobj.VoucherNo = dgView.Items(dgView.SelectedIndex).Cells(12).Text
            eobj.SubType = dgView.Items(dgView.SelectedIndex).Cells(13).Text
            dgView.SelectedIndex = -1
            Session("PayforCheque") = eobj
        End If
    End Sub

    Protected Sub ibtnOK_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ibtnOK.Click
        laodvalues()
        UpdateGrid()
    End Sub

    Protected Sub ibtnClose_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ibtnClose.Click
        UpdateGrid()
    End Sub
End Class
