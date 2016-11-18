Imports HTS.SAS.BusinessObjects
Imports HTS.SAS.Entities
Imports System.Data
Imports System.Collections.Generic
Partial Class AddSpnFee
    Inherits System.Web.UI.Page
    Dim listfee As New List(Of FeeTypesEn)
    ''Private LogErrors As LogError
    ''' <summary>
    ''' Method to Load FeeTypes in Grid
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub LoadGrid()
        Dim eobj As New FeeTypesEn
        Dim bobj As New FeeTypesBAL

        Try
            listfee = bobj.GetList(eobj)
        Catch ex As Exception
            LogError.Log("AddSpnFee", "LoadGrid", ex.Message)
        End Try
        If Not listfee Is Nothing Then
            dgView.DataSource = listfee
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
        'Dim eobj As eFeetype
        laodvalues()
        UpdateGrid()
    End Sub
    ''' <summary>
    ''' Method to Load Grid values to Session
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub laodvalues()
        Dim chkBox As CheckBox
        For Each dgi As DataGridItem In dgView.Items
            chkBox = dgi.Cells(0).Controls(1)
            Dim eobj As New FeeTypesEn
            If chkBox.Checked = True Then
                eobj = New FeeTypesEn
                'eobj.FeeTypeCode = dgView.DataKeys(dgi.ItemIndex).ToString
                eobj.FeeTypeCode = dgView.DataKeys(dgi.ItemIndex)
                eobj.Description = dgi.Cells(2).Text
                eobj.FeeType = dgi.Cells(3).Text
                eobj.Priority = Request.QueryString("Spid")
                'eobj.tempAmount = dgi.Cells(3).Text
                listfee.Add(eobj)
                'eobj = Nothing
            End If
        Next
        Session("eobj") = listfee

        'If dgView.SelectedIndex <> -1 Then
        '    Dim eobj As New SponsorEn
        '    eobj.SponserCode = dgView.DataKeys(dgView.SelectedIndex)
        '    eobj.Name = dgView.Items(dgView.SelectedIndex).Cells(1).Text
        '    eobj.TempSpnNo = Request.QueryString("Spid")
        '    dgView.SelectedIndex = -1
        '    Session("Spneobj") = eobj
        'End If
    End Sub

    Protected Sub ibtnClose_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ibtnClose.Click
        UpdateGrid()
    End Sub
End Class
