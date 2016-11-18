
Partial Class Default4
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Session("Menu") = "Dashboard"
        'Response.Redirect("Reports.aspx")
    End Sub
End Class
