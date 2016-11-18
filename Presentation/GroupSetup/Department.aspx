<%@ Page Language="VB" MasterPageFile="~/MasterPage3.master" MaintainScrollPositionOnPostback="true" 
    AutoEventWireup="false" CodeFile="Department.aspx.vb" Inherits="Department" Title="Welcome To SAS" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <script language="javascript" type="Scripts/popcalendar.js"></script>
    <script language="javascript" type="Scripts/functions.js"></script>

    <asp:Panel ID="pnlHeader" runat="server" Width="100%">
        <table style="background-image: url(../images/Sample.png);">
            <tr>
                <td style="width: 4px; height: 14px"></td>
                <td style="width: 14px; height: 14px"></td>
                <td>
                    <table style="border-collapse: collapse;" class="menuoff" onmouseover="className='menuon';"
                        onmouseout="className='menuoff';">
                        <tr>
                            <td style="height: 14px">
                                <asp:ImageButton ID="ibtnNew" runat="server" ImageUrl="~/images/add.png" ToolTip="New" />
                            </td>
                            <td style="width: 3%; height: 14px">
                                <asp:Label ID="lblNew" runat="server" Text="New" meta:resourcekey="Label11Resource1"></asp:Label>
                            </td>
                        </tr>
                    </table>
                </td>
                <td>
                    <table style="border-collapse: collapse;" class="menuoff" onmouseover="className='menuon';"
                        onmouseout="className='menuoff';">
                        <tr>
                            <td style="width: 24px; height: 14px">
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
                            <td style="width: 24px; height: 14px">
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
                                <asp:ImageButton ID="ibtnDelete" runat="server" ImageUrl="~/images/delete.png" />
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
                                <asp:ImageButton ID="ibtnSearch" runat="server" ImageUrl="~/images/find.png" ToolTip="View" />
                            </td>
                            <td style="width: 3%; height: 14px">
                                <asp:Label ID="lblSearch" runat="server" Text="Search" meta:resourcekey="Label16Resource1"></asp:Label>
                            </td>
                        </tr>
                    </table>
                </td>
                <td style="float: left;display:None; visibility:collapse;">
                    <table style="border-collapse: collapse;" class="menuoff" onmouseover="className='menuon';"
                        onmouseout="className='menuoff';">
                        <tr>
                            <td style="width: 3%; height: 14px">
                                <asp:ImageButton ID="ibtnRefresh" runat="server" ImageUrl="~/images/refresh.png" ToolTip="View" Visible="false" />
                            </td>
                            <td style="width: 3%; height: 14px">
                                <asp:Label ID="lblRefresh" runat="server" Text="Refresh" meta:resourcekey="Label16Resource1" Visible="false"></asp:Label>
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
                <td style="width: 2%; height: 14px"></td>
                <td style="width: 2%; height: 14px"></td>
                <td style="width: 2%; height: 14px"></td>
                <td style="width: 2%; height: 14px"></td>
                <td style="width: 2%; height: 14px"></td>
                <td style="width: 2%; height: 14px"></td>
                <td style="width: 2%; height: 14px"></td>
                <td style="width: 2%; height: 14px"></td>
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
                    <asp:Label ID="lblMenuName" runat="server" Width="422px" text="Department"></asp:Label>
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
        <div align="center" style="border: thin solid #A6D9F4; margin: 5px; width: 99%">
            <br />
            <table width="100%" align="center">
                <tr>
                    <td>
                        <table width="100%">
                            <tr>
                                <td width="10%">
                                    <asp:Label ID="Label2" runat="server" Text="Search"></asp:Label>
                                </td>
                                <td align="left">
                                    <asp:TextBox ID="txtSearch" runat="server" Width="400px"></asp:TextBox>
                                </td>
                            </tr>
                        </table>
                        <br />
                    </td>
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
                                        <asp:CheckBox ID="cbSelect" runat="server"/>
                                    </ItemTemplate>
                                    <HeaderStyle Width="5%" HorizontalAlign="Center" />
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:TemplateColumn>
                                <asp:BoundColumn DataField="AutoID" HeaderText="AutoID" Visible ="false">
                                    <HeaderStyle Width="5%" HorizontalAlign="Center" />
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:BoundColumn>
                                <asp:BoundColumn DataField="DepartmentID" HeaderText="Department Code">
                                    <HeaderStyle Width="10%" HorizontalAlign="Center" />
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:BoundColumn>
                                <asp:BoundColumn DataField="Department" HeaderText="Department">
                                    <HeaderStyle HorizontalAlign="Left" />
                                    <ItemStyle HorizontalAlign="Left" />
                                </asp:BoundColumn>
                            </Columns>
                        </asp:DataGrid>
                    </td>
                </tr>
            </table>
            <asp:HiddenField ID="hdnAutoID" runat="server"></asp:HiddenField>
        </div>
    </asp:Panel>

    <asp:Panel ID="pnlEdit" runat="server" Width="100%">
        <div align="center" style="margin: 5px; width: 99%">
            <table width="100%" align="center">
                <tr>
                    <td>
                        <fieldset style="width: 98%; padding: 5px; margin-left: 2px; border: thin solid #A6D9F4;">
                            <legend>
                                <asp:Label ID="Label1" runat="server" style="padding: 5px;"> Department Detail </asp:Label>
                            </legend>
                            <table border="0" cellpadding="2" cellspacing="0" align="left" style="width: 100%; margin-left: 3px;">
                                <tr>
                                    <td align="left" width="10%">
                                        <asp:Label ID="lblDepartmentID" runat="server" Text="Department ID"></asp:Label>
                                        &nbsp;<font color="red">*</font>
                                    </td>
                                    <td align="left" width="85%">
                                        <asp:TextBox ID="txtDepartmentID" runat="server"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left" width="15%">
                                        <asp:Label ID="lblDepartmentName" runat="server" Text="Department Name"></asp:Label>
                                        &nbsp;<font color="red">*</font>
                                    </td>
                                    <td align="left">
                                        <asp:TextBox ID="txtDepartmentName" runat="server" Width="400px" ></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left">
                                        <asp:Label ID="Label3" runat="server" Text="Status"></asp:Label>
                                        &nbsp;<font color="red">*</font>
                                    </td>
                                    <td align="left">
                                        <asp:RadioButtonList ID="rblStatus" runat="server" RepeatDirection="Horizontal">
                                            <asp:ListItem Value = "1">Active</asp:ListItem>
                                            <asp:ListItem Value = "0">Inactive</asp:ListItem>
                                        </asp:RadioButtonList>
                                    </td>
                                </tr>
                            </table>
                            &nbsp;
                        </fieldset>
                    </td>
                </tr>
            </table>
        </div>
    </asp:Panel>

</asp:Content>
