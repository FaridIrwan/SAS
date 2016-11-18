#Region "NameSpaces "

Imports MaxGeneric
Imports System.Data
Imports HTS.SAS.Entities
Imports HTS.SAS.BusinessObjects
Imports System.Collections.Generic

#End Region

Partial Class Login

    Inherits System.Web.UI.Page

#Region "Login Click "

    Protected Sub btnLogin_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnLogin.Click
        Session.Clear()
        FormsAuthentication.SignOut()
        Login()
    End Sub

#End Region

#Region "Login "

    Private Sub Login()

        'Create Instances - Start
        Dim _UsersEn As New UsersEn
        Dim _UserBusinessLayer As New UsersBAL
        'Create Instances - Stop

        'Variable Declarations
        Dim UserName As String = Nothing, UserPassword As String = Nothing

        Try

            'Get Values - Start
            UserPassword = clsGeneric.NullToString(txtPwd.Text)
            UserName = clsGeneric.NullToString(txtUserName.Text)
            'Get Values - Stop

            'if username and passwird not blank - Start
            If Not FormHelp.IsBlank(UserName) And Not FormHelp.IsBlank(UserPassword) Then

                'Set Values - Start
                _UsersEn.UserName = UserName
                _UsersEn.Password = UserPassword
                'Set Values - Stop

                'get User Login Details
                _UsersEn = _UserBusinessLayer.GetLoginUser(_UsersEn)

                'If User Name Available - Start
                If Not _UsersEn.UserName Is Nothing Then

                    'if user status active - Start
                    If _UsersEn.RecStatus = Helper.RecordActive Then

                        'Set Values - Start
                        _UsersEn.UserName = UserName
                        _UsersEn.Password = UserPassword
                        'Set Values - Stop

                        'Set Session Values - Start
                        Session(Helper.UserSession) = _UsersEn.UserName
                        Session(Helper.AliveSession) = _UsersEn.UserName
                        Session(Helper.UserGroupSession) = _UsersEn.UserGroupId
                        Session(Helper.WorkflowSession) = _UsersEn.WorkflowGroup
                        Session(Helper.WorkflowRole) = _UsersEn.WorkflowRole
                        Session(Helper.UserIDSession) = _UsersEn.UserID
                        'Set Session Values - Stop

                        FormsAuthentication.RedirectFromLoginPage(UserName, True)

                    Else

                        'Show Error Message
                        lblError.Text = "Your Account is Inactive / Please Contact Your System Administrator"

                    End If
                    'if user status active - Stop

                Else

                    'Show Error Message
                    lblError.Text = "Invalid UserName/Password"

                End If
                'If User Name Available - Stop

            Else

                'Show Error Message
                lblError.Text = "UserName/Password field cannot be Blank"

            End If
            'if username and passwird not blank - Stop

        Catch ex As Exception

            lblError.Text = "Connection to the Server Failed!"

        End Try

    End Sub

#End Region

#Region "Page Load "

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack() Then
            btnLogin.Attributes.Add("onclick", "return validate()")
            txtUserName.Focus()
        End If
    End Sub

#End Region

End Class
