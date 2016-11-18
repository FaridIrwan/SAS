Imports System.Data
Imports System.Collections.Generic
Imports HTS.SAS.BusinessObjects
Imports HTS.SAS.Entities
Partial Class addSpn
    Inherits System.Web.UI.Page
    ''Private LogErrors As LogError
    ''' <summary>
    ''' Method to Load Sponsors in Grid
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub LoadGrid()
        Dim eob As New SponsorEn
        Dim dsobj As New SponsorBAL
        Dim list As New List(Of SponsorEn)

        If Not eob Is Nothing Then
            eob.SponserCode = ""
            eob.Name = ""
            eob.Type = ""
            eob.GLAccount = ""
            eob.Status = True
            Try
                list = dsobj.GetSponserList(eob)
            Catch ex As Exception
                LogError.Log("addSpn", "LoadGrid", ex.Message)
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
            Dim eobj As New SponsorEn
            eobj.SponserCode = dgView.DataKeys(dgView.SelectedIndex)
            eobj.Name = dgView.Items(dgView.SelectedIndex).Cells(1).Text
            eobj.TempSpnNo = Request.QueryString("Spid")
            dgView.SelectedIndex = -1
            Session("Spneobj") = eobj
        End If
    End Sub
   
  
    Protected Sub ibtnClose_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ibtnClose.Click
        UpdateGrid()
    End Sub

    Protected Sub dgView_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgView.SelectedIndexChanged

    End Sub
End Class
