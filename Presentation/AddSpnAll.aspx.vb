Imports HTS.SAS.Entities
Imports HTS.SAS.BusinessObjects
Imports System.Data
Imports System.Collections.Generic
Partial Class AddSpnAll
    Inherits System.Web.UI.Page
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
            eob.Category = "Allocation"
            eob.SubType = "Sponsor"
            eob.BatchCode = ""
            'added by farid 27022016
            eob.SubCategory = "Student Payment"
            'eob.TransStatus = "Closed"
            Try
                list = dsobj.GetSPAllocationTransactionsStudentPayment(eob)
            Catch ex As Exception
                LogError.Log("AddSpnAll", "LoadGrid", ex.Message)
            End Try
            dgView.DataSource = list
            dgView.DataBind()
        Else
            Response.Write("No Fee types are Available")
        End If
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack() Then
            LoadGrid()
        End If
    End Sub
    Private Sub UpdateGrid()
        Dim cScript As String = ""
        cScript = "<SCRIPT LANGUAGE=""JavaScript"">window.opener.document.getElementById('ctl00_ContentPlaceHolder1_btnHidden').click();self.close();</SCRIPT>"
        ClientScript.RegisterStartupScript(Me.GetType(), "ClosePopUp", cScript)
    End Sub
    Protected Sub ibtnOK_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ibtnOK.Click
        laodvalues()
        UpdateGrid()
    End Sub
    ''' <summary>
    ''' Method to Load Grid values to Session
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub laodvalues()
        If dgView.SelectedIndex <> -1 Then
            Dim eobj As New AccountsEn
            eobj.TransactionCode = dgView.DataKeys(dgView.SelectedIndex)
            eobj.TransType = dgView.Items(dgView.SelectedIndex).Cells(1).Text
            eobj.CreditRef = dgView.Items(dgView.SelectedIndex).Cells(2).Text
            eobj.BatchCode = dgView.Items(dgView.SelectedIndex).Cells(4).Text
            eobj.TransStatus = dgView.Items(dgView.SelectedIndex).Cells(5).Text
            eobj.CreditRefOne = dgView.Items(dgView.SelectedIndex).Cells(6).Text
            eobj.TranssactionID = dgView.Items(dgView.SelectedIndex).Cells(7).Text
            dgView.SelectedIndex = -1
            Session("SpnAlleobj") = eobj
        End If
    End Sub


    Protected Sub ibtnClose_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ibtnClose.Click
        UpdateGrid()
    End Sub
End Class
