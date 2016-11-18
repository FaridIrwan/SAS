Imports HTS.SAS.Entities
Imports HTS.SAS.BusinessObjects
Imports System.data
Imports System.Collections.Generic
Partial Class StudentNotes
    Inherits System.Web.UI.Page
    Dim matno As String
    ''Private LogErrors As LogError
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Session("mno") = Nothing Then
        Else
            matno = Session("mno")
            LoadRepeater(matno)
        End If
    End Sub

    Private Sub LoadRepeater(ByVal SMatNo As String)
        Dim obj As New StuNotesBAL
        Dim eobj As New StuNotesEn
        Dim list As New List(Of StuNotesEn)
        eobj.MatricNo = matno

        Try
            list = obj.GetList(eobj)
        Catch ex As Exception
            LogError.Log("StudentNotes", "LoadRepeater", ex.Message)
        End Try
        StuNotesList.DataSource = list
        StuNotesList.DataBind()
    End Sub

    Protected Sub btnAdd_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAdd.Click
        Dim eobj As New StuNotesEn
        Dim obj As New StuNotesBAL
        eobj.MatricNo = matno
        eobj.SASN_Code = "1"
        eobj.Remarks = Trim(txtRemarks.Text)
        eobj.SASN_UpdatedBy = "Prabhu"
        eobj.SASN_UpdatedDtTm = Date.Now

        Try
            Dim i As Integer = obj.Insert(eobj)
        Catch ex As Exception
            LogError.Log("StudentNotes", "btnAdd_Click", ex.Message)
        End Try
        Response.Write("Record Saved Successfully ")

    End Sub
End Class
