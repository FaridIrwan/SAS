Imports System.IO
Partial Class _Default
    Inherits System.Web.UI.Page

    Protected Sub Button1_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Button1.Click
        Dim lsDelimeter As String = txtDelimeter.Text
        Dim loReader As New StreamReader("D://file1.txt")
        Dim loWriter As New StreamWriter(Server.MapPath("Uploadfiles") & "\" & txtFName.Text)
        Response.Write(FileUpload1.PostedFile.FileName)

        While loReader.Read() > 0
            Dim lsRow As String = loReader.ReadLine()
            Dim lsArr As String() = lsRow.Split(lsDelimeter.ToCharArray())
            txtColumns.Text = lsArr.Length
            loWriter.WriteLine(lsArr(CInt(txtMatrix.Text)).Trim() + "," + lsArr(CInt(txtICNo.Text)).Trim() + "," + lsArr(CInt(txtName.Text)).Trim() + "," + lsArr(CInt(txtAmount.Text)).Trim())
        End While
        loReader.Close()
        loWriter.Close()
    End Sub


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub
End Class
