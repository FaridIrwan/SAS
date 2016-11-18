<%@ Page Title="" Language="VB" MasterPageFile="~/MasterPage3.master" AutoEventWireup="false" CodeFile="HostelInvoice.aspx.vb" Inherits="GroupProcess_HostelInvoice" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:Panel ID="pnlHeader" runat="server" Width="100%">
        <table style="background-image: url(../images/Sample.png);">
            <tr>
                <td style="width: 4px; height: 14px"></td>
                <td style="width: 14px; height: 14px"></td>
                <td>
                    <table style="border-collapse: collapse;" class="menuoff" onmouseover="className='menuon';"
                        onmouseout="className='menuoff';">
                        <tr>
                            <td style="width: 3%; height: 14px">
                                <asp:ImageButton ID="ibtnNew" runat="server" ImageUrl="~/images/add.png" ToolTip="New" />
                            </td>
                            <td style="width: 3%; height: 14px">
                                <asp:Label ID="lblNew" runat="server" Text="New" meta:resourcekey="Label14Resource1"></asp:Label>
                            </td>
                        </tr>
                    </table>
                </td>
                <td>
                    <table style="border-collapse: collapse;" class="menuoff" onmouseover="className='menuon';"
                        onmouseout="className='menuoff';">
                        <tr>
                            <td style="width: 3%; height: 14px">
                                <asp:ImageButton ID="ibtnSave" runat="server" ImageUrl="~/images/save.png" ToolTip="Save" ValidationGroup="1" />
                            </td>
                            <td style="width: 3%; height: 14px">
                                <asp:Label ID="lblSave" runat="server" Text="Save" meta:resourcekey="Label14Resource1"></asp:Label>
                            </td>
                        </tr>
                    </table>
                </td>
                <td>
                    <table style="border-collapse: collapse;" class="menuoff" onmouseover="className='menuon';"
                        onmouseout="className='menuoff';">
                        <tr>
                            <td style="width: 3%; height: 14px">
                                <asp:ImageButton ID="ibtnOpen" runat="server" ImageUrl="~/images/edit.gif" ToolTip="Open" />
                            </td>
                            <td style="width: 3%; height: 14px">
                                <asp:Label ID="lblOpen" runat="server" Text="Open" meta:resourcekey="Label14Resource1"></asp:Label>
                            </td>
                        </tr>
                    </table>
                </td>
                <td>
                    <table style="border-collapse: collapse;" class="menuoff" onmouseover="className='menuon';"
                        onmouseout="className='menuoff';">
                        <tr>
                            <td style="width: 3%; height: 14px">
                                <asp:ImageButton ID="ibtnDelete" runat="server" ImageUrl="~/images/delete.png" ToolTip="Delete" />
                            </td>
                            <td style="width: 3%; height: 14px">
                                <asp:Label ID="lblDelete" runat="server" Text="Delete" meta:resourcekey="Label13Resource1"></asp:Label>
                            </td>
                        </tr>
                    </table>
                </td>
                <td>
                    <table style="border-collapse: collapse;" class="menuoff" onmouseover="className='menuon';"
                        onmouseout="className='menuoff';">
                        <tr>
                            <td style="width: 3%; height: 14px">
                                <asp:ImageButton ID="ibtnSearch" runat="server" ImageUrl="~/images/find.png" ToolTip="Search" />
                            </td>
                            <td style="width: 3%; height: 14px">
                                <asp:Label ID="lblSearch" runat="server" Text="Search" meta:resourcekey="Label16Resource1"></asp:Label>
                            </td>
                        </tr>
                    </table>
                </td>
                <td>
                    <table style="border-collapse: collapse;" class="menuoff" onmouseover="className='menuon';"
                        onmouseout="className='menuoff';">
                        <tr>
                            <td style="width: 3%; height: 14px">
                                <asp:ImageButton ID="ibtnRefresh" runat="server" ImageUrl="~/images/refresh.png" ToolTip="Refresh" />
                            </td>
                            <td style="width: 3%; height: 14px">
                                <asp:Label ID="lblRefresh" runat="server" Text="Refresh" meta:resourcekey="Label16Resource1"></asp:Label>
                            </td>
                        </tr>
                    </table>
                </td>
                <td>
                    <table style="border-collapse: collapse;" class="menuoff" onmouseover="className='menuon';"
                        onmouseout="className='menuoff';">
                        <tr>
                            <td style="width: 3%; height: 14px">
                                <asp:ImageButton ID="ibtnCancel" runat="server" ImageUrl="~/images/cancel.png" ToolTip="Cancel" Width="24px" />
                            </td>
                            <td style="width: 3%; height: 14px">
                                <asp:Label ID="lblCancel" runat="server" Text="Cancel" meta:resourcekey="Label18Resource1"></asp:Label>
                            </td>
                        </tr>
                    </table>
                </td>
                <td style="width: 100%; height: 14px"></td>
                <td style="width: 100%; height: 14px"></td>
            </tr>
        </table>
        <table style="width: 100%">
            <tr>
                <td class="vline" style="width: 100%; height: 1px"></td>
            </tr>
        </table>
        <table style="width: 100%">
            <tr>
                <td style="width: 400px">
                    <asp:SiteMapPath ID="SiteMapPath1" runat="server">
                    </asp:SiteMapPath>
                </td>
                <td class="pagetext" style="text-align: right">
                    <asp:Label ID="lblMenuName" runat="server" Width="422px" Text="Hostel Invoice"></asp:Label>
                </td>
            </tr>
        </table>
        <table>
            <tr>
                <td style="width: 100px">
                    <asp:Label ID="lblMsg" runat="server" CssClass="lblError" Style="text-align: left" Width="301px"></asp:Label>
                </td>
            </tr>
        </table>
    </asp:Panel>

    <asp:Panel ID="pnlSearch" runat="server" Width="100%">

        <div style="border: thin solid #A6D9F4; margin: 5px; width: 99%">
            <table style="width: 100%; margin-left: 5px;">
                <tr>
                    <td style="width: 13%"></td>
                    <td style="width: 13%"></td>
                    <td style="width: 183px; text-align: left"></td>
                    <td style="width: 100px; text-align: right"></td>
                    <td style="width: 271px"></td>
                    <td style="width: 378px"></td>
                    <td style="width: 378px"></td>
                </tr>
                <tr>
                    <td style="width: 13%">
                        <strong>
                            <asp:Label ID="Label1" runat="server" Text="Matric No." Width="97px"></asp:Label></strong>
                    </td>
                    <td style="width: 13%">
                        <asp:TextBox ID="txtStudentCode" runat="server" MaxLength="20" Width="216px"></asp:TextBox>
                    </td>
                    <td style="width: 183px; text-align: left">
                        <asp:Image ID="ibtnSpn1" runat="server" Height="16px" ImageAlign="Left" ImageUrl="~/images/find_img.png"
                            Style="margin-left: 0px" />
                    </td>
                    <td style="width: 42px;">
                        &nbsp;</td>
                    <td style="width: 271px">
                        &nbsp;</td>
                    <td style="width: 378px">&nbsp;
                    </td>
                    <td style="width: 378px"></td>
                </tr>
                <tr>
                    <td style="height: 25px; width: 13%">
                        <strong>
                            <asp:Label ID="Label3" runat="server" Text="Student Name " Width="97px"></asp:Label></strong>
                    </td>
                    <td style="width: 13%">
                        <asp:Label ID="txtStuName" runat="server" Width="218px" Text=""></asp:Label>
                    </td>
                    <td style="width: 183px; text-align: right"></td>
                    <td style="width: 42px;">
                        <strong>
                        <asp:Label ID="Label4" runat="server" Style="text-align: left" Text="IC No" Width="42px"></asp:Label>
                        </strong></td>
                    <td style="width: 271px; padding-left: 5px">
                        <asp:Label ID="txtICNO" runat="server" Width="142px"></asp:Label>
                    </td>
                    <td style="width: 378px">&nbsp;</td>
                    <td style="width: 378px"></td>
                </tr>
                <tr>
                    <td style="width: 13%; height: 25px;">
                        <strong>Kolej</strong>
                    </td>
                    <td style="width: 13%; height: 25px;">
                        <asp:Label ID="lblkolej" runat="server" Text=""></asp:Label>
                    </td>
                    <td style="width: 183px; text-align: right; height: 25px;">&nbsp;</td>
                    <td align="left">
                        <strong>Block</strong>
                    </td>
                    <td style="width: 271px; height: 25px;">
                        <asp:Label ID="lblblock" runat="server" Text=""></asp:Label>
                    </td>
                    <td style="width: 378px; height: 25px;">&nbsp;</td>
                    <td style="width: 378px; height: 25px;">&nbsp;</td>
                </tr>
                <tr>
                    <td style="width: 13%; height: 25px;">
                        <strong>Floor No</strong>
                    </td>
                    <td style="width: 13%; height: 25px;">
                        <asp:Label ID="lblfloor" runat="server" Text=""></asp:Label>
                    </td>
                    <td style="width: 183px; text-align: right; height: 25px;">&nbsp;</td>
                    <td align="left"><strong>Room Type</strong></td>
                    <td>
                        <asp:Label ID="lblroom" runat="server" Text=""></asp:Label>
                    </td>
                    <td style="width: 378px; height: 25px;">&nbsp;</td>
                    <td style="width: 378px; height: 25px;">&nbsp;</td>
                </tr>
            </table>
        </div>

        <div align="center" style="margin: 5px; width: 99%">
            <table width="100%" align="center">
                <tr>
                    <td align="center">
                        <asp:Label ID="lblDataGridMsg" runat="server" CssClass="userMsg"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:DataGrid ID="dgDataGrid" runat="server" AutoGenerateColumns="False"
                            Width="99%" HorizontalAlign="Center" AllowPaging="True">
                            <FooterStyle CssClass="dgFooterStyle" Height="20px" />
                            <PagerStyle HorizontalAlign="Center" Mode="NumericPages" />
                            <SelectedItemStyle CssClass="dgSelectedItemStyle" />
                            <AlternatingItemStyle BackColor="Beige" CssClass="dgAlternatingItemStyle" Font-Bold="False"
                                Font-Italic="False" Font-Overline="False" Font-Strikeout="False" Font-Underline="False" />
                            <ItemStyle CssClass="dgItemStyle" />
                            <HeaderStyle BackColor="#CDD7EE" CssClass="dgHeaderStyle" Font-Bold="True" Font-Italic="False"
                                Font-Overline="False" Font-Size="Medium" Font-Strikeout="False" Font-Underline="False" />
                            <Columns>
                                <asp:TemplateColumn HeaderText="Select">
                                    <ItemTemplate>
                                        <asp:CheckBox ID="cbSelect" runat="server" AutoPostBack="true" onclick="CheckBoxCheck(this);" />
                                    </ItemTemplate>
                                    <HeaderStyle Width="5%" HorizontalAlign="Center" />
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:TemplateColumn>
                                <asp:BoundColumn DataField="UserID" HeaderText="UserID" Visible="false">
                                    <HeaderStyle Width="5%" HorizontalAlign="Center" />
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:BoundColumn>
                                <asp:BoundColumn DataField="UserName" HeaderText="User Name">
                                    <HeaderStyle Width="15%" HorizontalAlign="Center" />
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:BoundColumn>
                                <asp:BoundColumn DataField="Department" HeaderText="Department">
                                    <HeaderStyle HorizontalAlign="Left" />
                                    <ItemStyle HorizontalAlign="Left" />
                                </asp:BoundColumn>
                                <asp:BoundColumn DataField="Description" HeaderText="User Group">
                                    <HeaderStyle HorizontalAlign="Left" />
                                    <ItemStyle HorizontalAlign="Left" />
                                </asp:BoundColumn>
                                <asp:BoundColumn DataField="UserStatus" HeaderText="Status">
                                    <HeaderStyle HorizontalAlign="Center" />
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:BoundColumn>
                            </Columns>
                        </asp:DataGrid>
                    </td>
                </tr>
            </table>
        </div>
    </asp:Panel>

</asp:Content>

