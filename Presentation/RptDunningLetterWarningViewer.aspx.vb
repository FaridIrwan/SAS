Imports System.Collections.Generic
Imports System.Data
Imports HTS.SAS.BusinessObjects
Imports HTS.SAS.Entities

Partial Class RptDunningLetterWarningViewer
    Inherits System.Web.UI.Page

    Protected Sub Load_Page(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim ObjDunningLettersEn As New DunningLettersEn
        Dim ObjDunningLettersBAL As New DunningLettersBAL
        Dim LstObjDunningLetters As New List(Of DunningLettersEn)
        Dim obj As New DunningLettersEn
        Dim strStudentStatus, status As String

        strStudentStatus = Session("StudentStatus")

        If strStudentStatus = -1 Then
            status = ""
        ElseIf strStudentStatus = 1 Then
            status = "ACTIVE"
        ElseIf strStudentStatus = 0 Then
            status = "INACTIVE"
        Else
            status = ""
        End If

        LstObjDunningLetters = ObjDunningLettersBAL.CheckListDunning(ObjDunningLettersEn)
        If (LstObjDunningLetters.Count <> 0) Then
            obj = LstObjDunningLetters(0)
        Else

        End If

        Dim strCountWarn As String
        If obj.Warning Is Nothing Then
            strCountWarn = "0"
        Else
            strCountWarn = obj.Warning
        End If
        lblCountwarn.Text = "Dunning Letter Issued " & strCountWarn & " Times"
        LoadGrid()
    End Sub

    Protected Sub btnPrint_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnPrint.Click

        Dim ObjDunningLettersEn As New DunningLettersEn
        Dim ObjDunningLettersBAL As New DunningLettersBAL
        Dim LstObjDunningLetters As New List(Of DunningLettersEn)
        'Dim strDunno, strWarning As String
        Dim list As New List(Of DunningLettersEn)
        Dim strCount As Integer
        Dim strWarn, strMatricNo As String
        Dim strMatricNoAccu As String = ""
        Dim obj As DunningLettersEn
        Dim obj1 As DunningLettersEn
        Dim strWarnMsg As String
        Dim strCodeWarn As String = ""
        Dim strStudentStatus, status As String

        strStudentStatus = Session("StudentStatus")
        If strStudentStatus = -1 Then
            status = ""
        ElseIf strStudentStatus = 1 Then
            status = "ACTIVE"
        ElseIf strStudentStatus = 0 Then
            status = "INACTIVE"
        Else
            status = ""
        End If

        LstObjDunningLetters = ObjDunningLettersBAL.CheckListDunning(ObjDunningLettersEn)
        strCount = LstObjDunningLetters.Count
        For i As Integer = 0 To strCount
            obj = LstObjDunningLetters(i)
            strWarn = obj.Warning
            strMatricNo = obj.MatricNo
            If strWarn = "3" Then
                strMatricNoAccu = strMatricNoAccu + "," + strMatricNo + ","
            End If
            i = i + 1
        Next

        If strMatricNoAccu <> "" Then
            strWarnMsg = "Overlimit for send dunning letter"
            Response.Write("<script>javascript:alert('" & strWarnMsg & "')</script>")
        Else
            
            list = ObjDunningLettersBAL.GetListDunning(ObjDunningLettersEn)
            obj = list(0)
            LstObjDunningLetters = ObjDunningLettersBAL.CheckDunningListing(ObjDunningLettersEn, obj.MatricNo)
            obj1 = LstObjDunningLetters(0)
            strCodeWarn = obj1.Code
            Response.Redirect("RptDunninglattersCRFormFinal.aspx?dunno1=" & strCodeWarn & "")
        End If

    End Sub

#Region "Method"
    ''' <summary>
    ''' Method to Load Grid Dunning LetterListing
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub LoadGrid()
        Dim ds As New DataSet
        Dim bobj As New DunningLettersBAL
        Dim eobj As New DunningLettersEn
        Dim strStudentStatus, status As String

        strStudentStatus = Session("StudentStatus")
        If strStudentStatus = -1 Then
            status = ""
        ElseIf strStudentStatus = 1 Then
            status = "ACTIVE"
        ElseIf strStudentStatus = 0 Then
            status = "INACTIVE"
        Else
            status = ""
        End If

        Dim ListDunningLetter As New List(Of DunningLettersEn)

        Try
            ListDunningLetter = bobj.ListDunningWarning(eobj)
        Catch ex As Exception
            LogError.Log("DunningLetter", "LoadGrid", ex.Message)
        End Try

        dgView.DataSource = ListDunningLetter
        dgView.DataBind()
    End Sub
#End Region

End Class
