<%@ Page Language="VB" AutoEventWireup="false" CodeFile="FileSponsor.aspx.vb" Inherits="FileSponsor" MaintainScrollPositionOnPostback="true" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Untitled Page</title>
</head>
<body>
    <form id="form1" runat="server">
        <div  style="border: thin solid #A6D9F4; width: 100%">
        <table>
            <tr>
                <td style="width: 23px">
                </td>
                <td style="width: 100px">
                    <asp:FileUpload ID="File1" runat="server" /></td>
                <td style="width: 100px">
                    <asp:Button ID="btnselect" runat="server" Text="Close" /></td>
            </tr>
            <tr>
                <td style="width: 23px">
                </td>
                <td style="width: 100px">
                </td>
                <td style="width: 100px">
                </td>
            </tr>
        </table>
     </div>
    </form>
</body>
</html>
