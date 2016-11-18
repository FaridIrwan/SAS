Partial Class Default4
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        'Session("Menu") = "Setup"
        Session(Helper.MenuSession) = "Setup"

        'Response.Redirect("Setup.aspx")
    End Sub
End Class
