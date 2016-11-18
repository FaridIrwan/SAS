<%@ Page Language="VB" AutoEventWireup="false" CodeFile="StudentSearch.aspx.vb" Inherits="StudentSearch" MaintainScrollPositionOnPostback="true" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Untitled Page</title>
    <link href="style.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <div style="text-align: left">
            <table style="width: 400px">
                <tr>
                    <td style="width: 100px; height: 16px">
                    </td>
                    <td style="width: 100px; height: 16px">
                        <asp:DataGrid ID="dgView" runat="server" AutoGenerateColumns="False" DataKeyField="MatricNo"
                            Width="420px">
                            <FooterStyle CssClass="dgFooterStyle" Height="20px" />
                            <SelectedItemStyle CssClass="dgSelectedItemStyle" />
                            <AlternatingItemStyle BackColor="Beige" CssClass="dgAlternatingItemStyle" Font-Bold="False"
                                Font-Italic="False" Font-Overline="False" Font-Strikeout="False" Font-Underline="False" />
                            <ItemStyle CssClass="dgItemStyle" />
                            <HeaderStyle BackColor="#CDD7EE" CssClass="dgHeaderStyle" Font-Bold="True" Font-Italic="False"
                                Font-Overline="False" Font-Size="Medium" Font-Strikeout="False" Font-Underline="False" />
                            <Columns>
                                <asp:ButtonColumn CommandName="Select" DataTextField="MatricNo" HeaderText="Matrix No"></asp:ButtonColumn>
                                <asp:BoundColumn DataField="StudentName" HeaderText="Student Name"></asp:BoundColumn>
                                <asp:BoundColumn DataField="ProgramID" HeaderText="Program Id"></asp:BoundColumn>
                                <asp:BoundColumn DataField="CurrentSemester" HeaderText="Semester"></asp:BoundColumn>
                            </Columns>
                        </asp:DataGrid></td>
                </tr>
                <tr>
                    <td style="width: 100px; height: 16px; text-align: center">
                    </td>
                    <td style="width: 100px; height: 16px; text-align: center">
                        <table style="width: 100%; height: 100%">
                            <tr>
                                <td style="width: 18px; text-align: right">
                                    <asp:ImageButton ID="ibtnOK" runat="server" Height="28px" ImageUrl="~/images/add_list.gif" 
                                        Width="28px" /></td>
                                <td style="width: 100px; text-align: left">
                                    <asp:Label ID="Label1" runat="server" Text="Select" Width="21px"></asp:Label></td>
                                <td style="width: 100px; text-align: left">
                                    <asp:ImageButton ID="ibtnClose" runat="server" Height="28px" ImageUrl="~/images/ok_cancel.jpg"
                                        Width="28px" /></td>
                                <td style="width: 930px; text-align: left">
                                    <asp:Label ID="Label2" runat="server" Text="Close" Width="21px"></asp:Label></td>
                                <td style="width: 100px; text-align: left">
                                    </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
            <br />
        </div>
    
    </div>
    </form>
</body>
</html>
