
Partial Class Home
    Inherits System.Web.UI.Page

    'Protected Sub ImageButton1_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ibtnEnquiry.Click
    'Session("Menu") = "Enquiry"
    'Response.Redirect("Enquiry.aspx")
    ' End Sub

    Protected Sub ibtnHome_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ibtnHome.Click
        Session(Helper.MenuSession) = "Home"
        Response.Redirect("Home.aspx")
    End Sub

    Protected Sub ibtnSetup_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ibtnSetup.Click
        Session(Helper.MenuSession) = "Setup"
        Response.Redirect("Setup.aspx")
    End Sub

    Protected Sub ibtnProcess_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ibtnProcess.Click
        Session(Helper.MenuSession) = "Process"
        Response.Redirect("Process.aspx")
    End Sub

    Protected Sub ibtnStudent_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ibtnStudent.Click
        Session(Helper.MenuSession) = "Process"
        Response.Redirect("Process.aspx")
    End Sub

    Protected Sub ibtnSpn_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ibtnSpn.Click
        Session(Helper.MenuSession) = "Process"
        Response.Redirect("Process.aspx")
    End Sub

    Protected Sub IbtnReports_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles IbtnReports.Click
        Session(Helper.MenuSession) = "Reports"
        Response.Redirect("Reports.aspx")
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        'Session(Helper.MenuSession) = "Home"
    End Sub
End Class
