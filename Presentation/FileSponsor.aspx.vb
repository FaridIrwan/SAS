Partial Class FileSponsor
    Inherits System.Web.UI.Page

    Protected Sub btnselect_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnselect.Click
        If Not File1.PostedFile Is Nothing And File1.PostedFile.ContentLength > 0 Then
            Dim fn As String = System.IO.Path.GetFileName(File1.PostedFile.FileName)
            Dim SaveLocation As String = Server.MapPath("data") & "\" & fn
            Dim fileOK As Boolean = False
            Dim fileType As String = Nothing
            If File1.HasFile Then
                Dim fileExtension As String
                fileExtension = System.IO.Path. _
                    GetExtension(File1.FileName).ToLower()
                Dim allowedExtensions As String() = _
                    {".xls", ".txt"}

                For i As Integer = 0 To allowedExtensions.Length - 1
                    If fileExtension = allowedExtensions(i) Then
                        fileOK = True
                    End If
                Next
                If fileOK Then
                    Try
                        File1.PostedFile.SaveAs(SaveLocation)
                        Response.Write("File has been Uploaded.")
                        If fileExtension = ".xls" Then
                            fileType = "excel"
                        ElseIf fileExtension = ".txt" Then
                            fileType = "text"
                        End If
                        Session("fileSponsor") = SaveLocation
                        Session("fileType") = fileType
                        UpdateGrid()
                    Catch ex As Exception
                        Response.Write("File Upload Failed.")
                    End Try
                Else
                    Response.Write("Select a Valid File to Upload.")
                End If
            Else
                Response.Write("Please Select a File to Upload.")
            End If

        Else
            Response.Write("Please Select a File to Upload.")
        End If

    End Sub
    Private Sub UpdateGrid()
        Dim cScript As String = ""
        cScript = "<SCRIPT LANGUAGE=""JavaScript"">window.opener.document.getElementById('ctl00_ContentPlaceHolder1_btnHidden').click();self.close();</SCRIPT>"
        ClientScript.RegisterStartupScript(Me.GetType(), "ClosePopUp", cScript)
    End Sub
End Class
