<%@ Page Language="VB" AutoEventWireup="false" CodeFile="upload.aspx.vb" Inherits="upload" MaintainScrollPositionOnPostback="true" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Untitled Page</title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:Label ID="Label1" runat="server" Text="Select File"></asp:Label>&nbsp;<asp:FileUpload
            ID="FileUpload1" runat="server" /><br />
        <br />
        <asp:Label ID="Label10" runat="server" Text="Delimeter"></asp:Label>
        <asp:TextBox ID="txtDelimeter" runat="server" MaxLength="2" Width="44px"></asp:TextBox><br />
        <br />
        <asp:Label ID="Label2" runat="server" Text="No of Columns"></asp:Label>
        <asp:TextBox ID="txtColumns" runat="server"></asp:TextBox><br />
        <br />
        <asp:Label ID="Label11" runat="server" Text="File Name"></asp:Label><asp:TextBox
            ID="txtFName" runat="server"></asp:TextBox><br />
        <br />
        <table>
            <tr>
                <td style="width: 100px">
                    <asp:Label ID="Label4" runat="server" Text="Field Name"></asp:Label></td>
                <td style="width: 212px">
                    <asp:Label ID="Label8" runat="server" Text="Column Number"></asp:Label></td>
                <td style="width: 100px">
                </td>
            </tr>
            <tr>
                <td style="width: 100px">
                    <asp:Label ID="Label3" runat="server" Text="Matrix No"></asp:Label></td>
                <td style="width: 212px">
                    <asp:TextBox ID="txtMatrix" runat="server" MaxLength="2" Width="47px"></asp:TextBox></td>
                <td style="width: 100px">
                </td>
            </tr>
            <tr>
                <td style="width: 100px; height: 24px">
                    <asp:Label ID="Label5" runat="server" Text="IC No"></asp:Label></td>
                <td style="width: 212px; height: 24px">
                    <asp:TextBox ID="txtICNo" runat="server" MaxLength="2" Width="47px"></asp:TextBox></td>
                <td style="width: 100px; height: 24px">
                </td>
            </tr>
            <tr>
                <td style="width: 100px">
                    <asp:Label ID="Label6" runat="server" Text="Name"></asp:Label></td>
                <td style="width: 212px">
                    <asp:TextBox ID="txtName" runat="server" MaxLength="2" Width="47px"></asp:TextBox></td>
                <td style="width: 100px">
                </td>
            </tr>
            <tr>
                <td style="width: 100px">
                    <asp:Label ID="Label7" runat="server" Text="Amount"></asp:Label></td>
                <td style="width: 212px">
                    <asp:TextBox ID="txtAmount" runat="server" MaxLength="2" Width="47px"></asp:TextBox></td>
                <td style="width: 100px">
                </td>
            </tr>
            <tr>
                <td style="width: 100px">
                    <asp:Label ID="Label9" runat="server" Text="Reference"></asp:Label></td>
                <td style="width: 212px">
                    <asp:TextBox ID="txtReference" runat="server" MaxLength="2" Width="47px"></asp:TextBox></td>
                <td style="width: 100px">
                </td>
            </tr>
        </table>
        <br />
        <asp:Button ID="Button1" runat="server" Text="Create File" /></div>
    </form>
</body>
</html>
