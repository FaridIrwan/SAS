Imports HTS.SAS.Entities
Imports HTS.SAS.BusinessObjects
Imports System.Data
Imports System.Collections.Generic
Partial Class AddFaculty
    Inherits System.Web.UI.Page


    ''' <summary>
    ''' Mtghod to Load the Grid
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub LoadGrid()
        Dim eob As New FacultyEn
        Dim bobj As New FacultyBAL
        Dim list As New List(Of FacultyEn)
        eob.SAFC_Code = "%"

        Try
            list = bobj.GetList(eob)
        Catch ex As Exception
            LogError.Log("AddFaculty", "LoadGrid", ex.Message)
        End Try
        If Not list Is Nothing Then
            dgView.DataSource = list
            dgView.DataBind()
        Else
            Response.Write("No Faculty are Available")
        End If
    End Sub
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
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
    Protected Sub ibtnOK_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ibtnOK.Click
        laodvalues()
        Try
            UpdateGrid()
        Catch ex As Exception
            LogError.Log("AddFaculty", "ibtnOK_Click", ex.Message)
        End Try
    End Sub
    ''' <summary>
    ''' Method to Load Grid values to Session
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub laodvalues()
        If dgView.SelectedIndex <> -1 Then
            Dim eobj As New FacultyEn
            eobj.SAFC_Code = dgView.DataKeys(dgView.SelectedIndex)
            eobj.SAFC_Desc = dgView.Items(dgView.SelectedIndex).Cells(1).Text

            If dgView.Items(dgView.SelectedIndex).Cells(2).Text.Contains("&nbsp") Then
                eobj.SAFC_SName = ""
            Else
                eobj.SAFC_SName = dgView.Items(dgView.SelectedIndex).Cells(2).Text
            End If

            dgView.SelectedIndex = -1
            Session("eobjFaculty") = eobj
        End If
    End Sub

    Protected Sub ibtnClose_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ibtnClose.Click
        Try
            UpdateGrid()
        Catch ex As Exception
            LogError.Log("AddFaculty", "ibtnClose_Click", ex.Message)
        End Try

    End Sub
End Class
