<%@ Page Language="VB" AutoEventWireup="false" CodeFile="addSpnRecpts.aspx.vb" Inherits="addSpnRecpts" MaintainScrollPositionOnPostback="true"%>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Add Sponsor Receipts</title>
    <link href="style.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <br />
        <table>
            <tr>
                <td style="width: 100px">
                    <asp:Label ID="Label1" runat="server" Text="Sponsor Code" Width="99px"></asp:Label></td>
                <td style="width: 100px">
                    <asp:TextBox ID="txtSponsorCode" runat="server" Width="183px"></asp:TextBox></td>
                <td style="width: 100px">
                    <table>
                        <tr>
                            <td style="width: 17px">
                                <asp:ImageButton ID="ibtnLoad" runat="server" ImageUrl="~/images/find.gif" TabIndex="5" /></td>
                            <td style="width: 100px">
                                <asp:Label ID="Label12" runat="server" Text="Search" Width="55px"></asp:Label></td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td style="width: 100px">
                    <asp:Label ID="Label2" runat="server" Text="Sponsor Name" Width="100px"></asp:Label></td>
                <td style="width: 100px">
                    <asp:TextBox ID="txtSponsorName" runat="server" Width="182px"></asp:TextBox></td>
                <td style="width: 100px">
                </td>
            </tr>
            <tr>
                <td style="width: 100px">
                </td>
                <td style="width: 100px">
                </td>
                <td style="width: 100px">
                </td>
            </tr>
        </table>
        <asp:Panel ID="pnlSpn" runat="server" Height="450px" Width="611px">
            <hr /><table style="width: 82%">
                <tr>                    
                    <td colspan="2" style="width: 153px">
                        <div style="overflow: auto; width: 569px; height: 150px">
                            <asp:DataGrid ID="dgView" runat="server" AutoGenerateColumns="False" DataKeyField="TransactionCode"
                                Width="100%">
                                <FooterStyle CssClass="dgFooterStyle" Height="20px" />
                                <SelectedItemStyle CssClass="dgSelectedItemStyle" />
                                <AlternatingItemStyle BackColor="Beige" CssClass="dgAlternatingItemStyle" Font-Bold="False"
                                    Font-Italic="False" Font-Overline="False" Font-Strikeout="False" Font-Underline="False" />
                                <ItemStyle CssClass="dgItemStyle" />
                                <HeaderStyle BackColor="#CDD7EE" CssClass="dgHeaderStyle" Font-Bold="True" Font-Italic="False"
                                    Font-Overline="False" Font-Size="Medium" Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Center" />
                                <Columns>
                                    <%-- Modified by Hafiz @ 17/2/2016 --%>
                                    <asp:ButtonColumn CommandName="Select" DataTextField="TransactionCode" HeaderText="Code"
                                        Text="TransactionCode"></asp:ButtonColumn>
                                    <asp:BoundColumn DataField="TempTransCode" HeaderText="Sponsor Invoice"></asp:BoundColumn>
                                    <asp:BoundColumn DataField="SponserCode" HeaderText="Sponsor Code"></asp:BoundColumn>
                                    <asp:BoundColumn DataField="Name" HeaderText="Sponsor Name"></asp:BoundColumn>
                                    <asp:BoundColumn DataField="TransDate" HeaderText="Date " DataFormatString="{0:dd/MM/yyyy}" ItemStyle-HorizontalAlign="Center"></asp:BoundColumn>
                                    <asp:BoundColumn DataField="PaidAmount" HeaderText="Invoice Amount" DataFormatString="{0:F}" ItemStyle-HorizontalAlign="Center"></asp:BoundColumn>
                                    <asp:BoundColumn DataField="TransactionAmount" HeaderText="Receipt Amount" DataFormatString="{0:F}" ItemStyle-HorizontalAlign="Center"></asp:BoundColumn>
                                    <asp:BoundColumn DataField="Category" HeaderText="Category" Visible="False"></asp:BoundColumn>
                                    <asp:BoundColumn DataField="AllocatedAmount" HeaderText="Allocated Amount" DataFormatString="{0:F}" ItemStyle-HorizontalAlign="Center" Visible="True"></asp:BoundColumn>
                                    <asp:BoundColumn DataField="TempAmount" HeaderText="Amount Balance" DataFormatString="{0:F}" ItemStyle-HorizontalAlign="Center" Visible="True"></asp:BoundColumn>
                                </Columns>
                            </asp:DataGrid>
                           
                        </div>
                    </td>                    
                </tr>
            </table>
            
            <table style="width: 100%; height: 2%">
                <caption>
                    <hr />
                    <tr>
                        <td style="width: 8px; height: 30px; text-align: right">
                            <asp:ImageButton ID="ibtnOK" runat="server" Height="28px" ImageUrl="~/images/add_list.gif" Width="28px" />
                        </td>
                        <td style="width: 49px; height: 30px; text-align: left">
                            <asp:Label ID="Label4" runat="server" Text="Select" Width="21px"></asp:Label>
                        </td>
                        <td style="width: 34px; height: 30px; text-align: left">
                            <asp:ImageButton ID="ibtnClose" runat="server" Height="28px" ImageUrl="~/images/ok_cancel.jpg" Width="28px" />
                        </td>
                        <td style="width: 158px; height: 30px; text-align: left">
                            <asp:Label ID="Label6" runat="server" Text="Close" Width="21px"></asp:Label>
                        </td>
                        <td style="width: 142px; height: 30px; text-align: left"></td>
                        <td style="width: 100px; height: 30px; text-align: left"></td>
                    </tr>
                </caption>
                </tr>
            </table>
        </asp:Panel>
        <br />
    
    </div>
    </form>
</body>
</html>
