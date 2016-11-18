<%@ Page Language="VB" AutoEventWireup="false" CodeFile="Login.aspx.vb" Inherits="Login" MaintainScrollPositionOnPostback="true" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">


<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title>SAS Login</title>
    <meta http-equiv="Content-Type" content="text/html; charset=iso-8859-1" />
    <style type="text/css">
        html, body {
            margin: 0;
            padding: 0;
            height: 100%;
        }

        .style1 {
            height: 260px;
            width: 227px;
        }
    </style>
</head>

<body style="background-image: url(images/back.png); background-repeat: repeat-x; font-family: Arial; font-size: 12px;">
    <script language="javascript" type="text/javascript">
        function validate() {

            var re = /\s*((\S+\s*)*)/;
            if (document.getElementById("<%=txtUserName.ClientID%>").value.replace(re, "$1").length == 0) {
                alert("User Name Field Cannot Be Blank");
                document.getElementById("<%=txtUserName.ClientID%>").focus();
                return false;
            }
            var digits = "ABCDEFGHIJKLMNOPQRSTUVWXYZ abcdefghijklmnopqrstuvwxyz0123456789.";
            var temp;
            for (var i = 0; i < document.getElementById("<%=txtUserName.ClientID %>").value.length; i++) {
            temp = document.getElementById("<%=txtUserName.ClientID%>").value.substring(i, i + 1);
            if (digits.indexOf(temp) == -1) {
                alert("Enter Valid User Name");
                document.getElementById("<%=txtUserName.ClientID%>").focus();
                     return false;
                 }
             }
             if (document.getElementById("<%=txtPwd.ClientID%>").value.replace(re, "$1").length == 0) {
                alert("Password Field Cannot Be Blank");
                document.getElementById("<%=txtPwd.ClientID%>").focus();
                return false;
            }

            return true;
        }

    </script>
    <form id="form1" runat="server">

        <table width="100%" style="height: 100%; padding: 150px;" border="0" cellspacing="0" cellpadding="0">
            <tr>
                <td style="text-align: center;">
                    <table style="height: 283px; margin: auto; width: 544px; background-image: url(images/sas_login.png);" border="0" cellpadding="0" cellspacing="0">
                        <tr>
                            <td class="style1"></td>
                            <td style="width: 258px">
                                <table style="width: 252px; height: 169px">
                                    <tr>
                                        <td style="height: 27px; width: 76px; text-align: left;"><font face="Arial">
                                            <label for="ath_uname">
                                                <strong>
                                                    <span style="font-size: 8pt"></span></strong>
                                            </label>
                                        </font></td>
                                        <td style="height: 27px; text-align: left; width: 111px;">&nbsp;</td>
                                        <td style="width: 12px; height: 27px; text-align: left"></td>
                                    </tr>
                                    <tr>
                                        <td style="height: 13px; width: 76px; text-align: left;"><font face="Arial">
                                            <label for="ath_passwd">
                                                <strong>
                                                    <span style="font-size: 8pt"></span></strong>
                                            </label>
                                        </font></td>
                                        <td style="height: 13px; text-align: left; width: 111px;">&nbsp;<asp:Label ID="Label1" runat="server" ForeColor="White" Text="Login to your account" Width="128px"></asp:Label></td>
                                        <td style="width: 12px; height: 13px; text-align: left"></td>
                                    </tr>
                                    <tr>
                                        <td style="width: 76px; height: 18px; text-align: left">
                                            <asp:Label ID="Label2" runat="server" Text="User Name" ForeColor="White"></asp:Label></td>
                                        <td style="text-align: left; height: 18px; width: 111px;">
                                            <asp:TextBox ID="txtUserName" runat="server" Width="150px"></asp:TextBox></td>
                                        <td style="width: 12px; height: 18px; text-align: left"></td>
                                    </tr>
                                    <tr>
                                        <td style="width: 76px; height: 18px; text-align: left">
                                            <asp:Label ID="Label3" runat="server" Text="Password" ForeColor="White"></asp:Label></td>
                                        <td style="height: 18px; text-align: left; width: 111px;">
                                            <asp:TextBox ID="txtPwd" runat="server" TextMode="Password" Width="150px"></asp:TextBox></td>
                                        <td style="width: 12px; height: 18px; text-align: left"></td>
                                    </tr>
                                    <tr>
                                        <td style="width: 76px; height: 20px"></td>
                                        <td style="height: 20px; text-align: left; width: 111px;">
                                            <asp:ImageButton ID="btnLogin" runat="server" ImageUrl="~/images/logimages/login.png" /></td>
                                        <td style="width: 12px; height: 20px; text-align: left"></td>
                                    </tr>
                                    <tr>
                                        <td colspan="2" style="height: 22px; text-align: center">&nbsp;
                                            <asp:Label ID="lblError" runat="server" Font-Bold="True" Font-Names="Arial" Font-Size="8pt"
                                            ForeColor="Red"></asp:Label></td>
                                        <td colspan="1" style="width: 12px; height: 22px; text-align: center"></td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
    </form>
</body>
</html>
