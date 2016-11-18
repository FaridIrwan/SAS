Imports HTS.SAS.Entities
Imports HTS.SAS.BusinessObjects
Imports System.Collections.Generic
Imports System.Data
Partial Class StudentSearch
    Inherits System.Web.UI.Page
    Dim lst As New List(Of StudentEn)
    Dim ds As DataSet
    Dim dsobj As New StudentBAL
    ''Private LogErrors As LogError

    Protected Sub ibtnOK_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ibtnOK.Click

    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack() Then
            Dim loEn As New StudentEn

            Try
                lst = dsobj.GetList(loEn)
            Catch ex As Exception
                LogError.Log("StudentSearch", "Page_Load", ex.Message)
            End Try
            dgView.DataSource = lst
            dgView.DataBind()
        End If
    End Sub

    Protected Sub dgView_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgView.SelectedIndexChanged

        Dim eobjstu As New StudentEn
        'dgView.SelectedIndex = -1
        Session("SelStu") = dgView.DataKeys(dgView.SelectedIndex)
        eobjstu.MatricNo = dgView.DataKeys(dgView.SelectedIndex)
        eobjstu.StudentName = dgView.Items(dgView.SelectedIndex).Cells(1).Text
        eobjstu.ProgramID = dgView.Items(dgView.SelectedIndex).Cells(2).Text
        eobjstu.CurrentSemester = dgView.Items(dgView.SelectedIndex).Cells(3).Text
        Session("SelStuObj") = eObjStu
        UpdateGrid()

    End Sub

    Private Sub UpdateGrid()
        Dim cScript As String = ""
        cScript &= "<script language=javascript>"
        cScript &= "window.opener.location.href = window.opener.location.href;"
        cScript &= "window.close();"
        cScript &= "</script>"
        Response.Write(cScript)
    End Sub

End Class
