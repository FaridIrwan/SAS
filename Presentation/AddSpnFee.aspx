<%@ Page Language="VB" AutoEventWireup="false" CodeFile="AddSpnFee.aspx.vb" Inherits="AddSpnFee" MaintainScrollPositionOnPostback="true"%>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title></title>
    <link href="style.css" rel="stylesheet" type="text/css" />
</head>
<body topmargin="0" leftmargin="0" alink="#00ff66">
    <form id="form1" runat="server">
    <div style="text-align: left">
        <table style="width: 400px">
            <tr>
                <td style="width: 100px; height: 16px">
                </td>
                <td style="width: 100px; height: 16px">
                <div style="overflow:auto; width:450px; height:200px;" > 
            <asp:DataGrid ID="dgView" runat="server" AutoGenerateColumns="False" DataKeyField="FeeTypeCode"
                Width="420px">
                <FooterStyle CssClass="dgFooterStyle" Height="20px" />
                <SelectedItemStyle CssClass="dgSelectedItemStyle" />
                <AlternatingItemStyle BackColor="Beige" CssClass="dgAlternatingItemStyle" Font-Bold="False"
                    Font-Italic="False" Font-Overline="False" Font-Strikeout="False" Font-Underline="False" />
                <ItemStyle CssClass="dgItemStyle" />
                <HeaderStyle BackColor="#CDD7EE" CssClass="dgHeaderStyle" Font-Bold="True" Font-Italic="False"
                    Font-Overline="False" Font-Size="Medium" Font-Strikeout="False" Font-Underline="False" />
                <Columns>
                    <asp:TemplateColumn HeaderText="Select">
                        <ItemTemplate>
                            <asp:CheckBox ID="chk" runat="server" />
                        </ItemTemplate>
                    </asp:TemplateColumn>
                    <asp:ButtonColumn CommandName="Select" DataTextField="FeeTypeCode" HeaderText="Fee Code"></asp:ButtonColumn>
                    <asp:BoundColumn DataField="Description" HeaderText="Fee Desc"></asp:BoundColumn>
                    <asp:BoundColumn DataField="FeeType" HeaderText="Fee Type"></asp:BoundColumn>
                    <asp:BoundColumn DataField="Priority" HeaderText="Priority"></asp:BoundColumn>
                </Columns>
            </asp:DataGrid></div> </td>
            </tr>
            <tr>
                <td style="width: 100px; height: 16px; text-align: center">
                </td>
                <td style="width: 100px; height: 16px; text-align: center">
                   <hr /> <table style="width: 100%; height: 100%">
                        <tr>
                            <td style="width: 8px; height: 30px; text-align: right">
                                <asp:ImageButton ID="ibtnOK" runat="server" Height="28px" ImageUrl="~/images/add_list.gif"
                                    Width="28px" /></td>
                            <td style="width: 49px; height: 30px; text-align: left">
                                <asp:Label ID="Label1" runat="server" Text="Select" Width="21px"></asp:Label></td>
                            <td style="width: 34px; height: 30px; text-align: left">
                                <asp:ImageButton ID="ibtnClose" runat="server" Height="28px" ImageUrl="~/images/ok_cancel.jpg"
                                    Width="28px" /></td>
                            <td style="width: 158px; height: 30px; text-align: left">
                                <asp:Label ID="Label2" runat="server" Text="Close" Width="21px"></asp:Label></td>
                            <td style="width: 142px; height: 30px; text-align: left">
                            </td>
                            <td style="width: 100px; height: 30px; text-align: left">
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
        <br />
    </div>
    </form>
</body>
</html>
