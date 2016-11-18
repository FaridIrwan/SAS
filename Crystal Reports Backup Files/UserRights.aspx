<%@ Page Language="VB" MasterPageFile="~/MasterPage3.master" MaintainScrollPositionOnPostback="true" AutoEventWireup="false" CodeFile="UserRights.aspx.vb" Inherits="UserRights" Title="User Rights" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <script type="text/javascript">

        function checkAll(bx) {
            var cbs = document.getElementsByTagName('input');
            for (var i = 0; i < cbs.length; i++) {
                if (cbs[i].type == 'checkbox') {
                    //if (cbs[i].name == 'chkAdd') {
                                       cbs[i].checked = bx.checked;
                                    //}
                    //cbs[i].checked = bx.checked;
                }
            }
        }

        //function checkAll_99() {
        //    var array = document.getElementsByTagName("input");

        //    for (var ii = 0; ii < array.length; ii++) {

        //        if (array[ii].type == "checkbox") {
        //            if (array[ii].className == 'classAdd') {
        //                array[ii].checked = true;
        //            }
        //        }
        //    }
        //}

    </script>

    <asp:Panel ID="Panel1" runat="server" Width="100%">
        <table style="background-image: url(images/Sample.png);">
            <tr>
                <td style="width: 4px; height: 14px"></td>
                <td style="width: 14px; height: 14px"></td>

                <td>
                    <table style="border-collapse: collapse;" class="menuoff" onmouseover="className='menuon';" onmouseout="className='menuoff';">
                        <tr>
                            <td>
                                <asp:ImageButton ID="ibtnNew" runat="server" ImageUrl="images/add.png" ToolTip="New" /></td>
                            <td>
                                <asp:Label ID="Label11" runat="server" Text="New"></asp:Label></td>
                        </tr>
                    </table>
                </td>

                <td>
                    <table style="border-collapse: collapse;" class="menuoff" onmouseover="className='menuon';" onmouseout="className='menuoff';">
                        <tr>
                            <td>
                                <asp:ImageButton ID="ibtnSave" runat="server" ImageUrl="~/images/save.png" ToolTip="Save" /></td>
                            <td>
                                <asp:Label ID="Label14" runat="server" Text="Save"></asp:Label></td>
                        </tr>
                    </table>
                </td>


                <td>
                    <table style="border-collapse: collapse;" class="menuoff" onmouseover="className='menuon';" onmouseout="className='menuoff';">
                        <tr>
                            <td>
                                <asp:ImageButton ID="ibtnDelete" runat="server" ImageUrl="~/images/delete.png" /></td>
                            <td>
                                <asp:Label ID="Label13" runat="server" Text="Delete"></asp:Label></td>
                        </tr>
                    </table>
                </td>

                <td>
                    <table style="border-collapse: collapse;" class="menuoff" onmouseover="className='menuon';" onmouseout="className='menuoff';">
                        <tr>
                            <td>
                                <asp:ImageButton ID="ibtnView" runat="server" ImageUrl="~/images/find.png" ToolTip="View" /></td>
                            <td>
                                <asp:Label ID="Label16" runat="server" Text="Search"></asp:Label></td>
                        </tr>
                    </table>
                </td>

                <td>
                    <table style="border-collapse: collapse;" class="menuoff" onmouseover="className='menuon';" onmouseout="className='menuoff';">
                        <tr>
                            <td>
                                <asp:ImageButton ID="ibtnPrint" runat="server" ImageUrl="~/images/print.png" ToolTip="Print" /></td>
                            <td>
                                <asp:Label ID="Label17" runat="server" Text="Print"></asp:Label></td>
                        </tr>
                    </table>
                </td>

                <td>
                    <table style="border-collapse: collapse;" class="menuoff" onmouseover="className='menuon';" onmouseout="className='menuoff';">
                        <tr>
                            <td>
                                <asp:ImageButton ID="ibtnPosting" runat="server" ImageUrl="~/images/gposting.png" ToolTip="Cancel" /></td>
                            <td>
                                <asp:Label ID="Label6" runat="server" Text="Posting"></asp:Label></td>
                        </tr>
                    </table>
                </td>

                <td>
                    <table style="border-collapse: collapse;" class="menuoff" onmouseover="className='menuon';" onmouseout="className='menuoff';">
                        <tr>
                            <td>
                                <asp:ImageButton ID="ibtnOthers" runat="server" ImageUrl="~/images/gothers.gif" ToolTip="Cancel" /></td>
                            <td>
                                <asp:Label ID="Label5" runat="server" Text="Others"></asp:Label></td>
                        </tr>
                    </table>
                </td>

                <td>
                    <table style="border-collapse: collapse;" class="menuoff" onmouseover="className='menuon';" onmouseout="className='menuoff';">
                        <tr>
                            <td>
                                <asp:ImageButton ID="ibtnCancel" runat="server" ImageUrl="~/images/cancel.png" ToolTip="Cancel" /></td>
                            <td>
                                <asp:Label ID="Label18" runat="server" Text="Cancel"></asp:Label></td>
                        </tr>
                    </table>
                </td>

                <td style="width: 2%; height: 14px">
                    <asp:ImageButton ID="ibtnFirst" runat="server" ImageUrl="~/images/new_last.png" /></td>
                <td style="width: 2%; height: 14px">
                    <asp:ImageButton ID="ibtnPrevs" runat="server" ImageUrl="~/images/new_prev.png" /></td>
                <td style="width: 2%; height: 14px">
                    <asp:TextBox ID="txtRecNo" ReadOnly="true" disabled="disabled" TabIndex="1" dir="ltr" CssClass="text_box" runat="server" AutoPostBack="True" Style="text-align: right"
                        Width="52px"></asp:TextBox></td>
                <td style="width: 2%; height: 14px">
                    <asp:Label ID="Label47" runat="server">Of</asp:Label></td>
                <td style="width: 2%; height: 14px">
                    <asp:Label ID="lblCount" runat="server" Width="20px"></asp:Label></td>

                <td style="width: 2%; height: 14px">
                    <asp:ImageButton ID="ibtnNext" runat="server" ImageUrl="~/images/new_next.png"
                        Width="24px" /></td>

                <td style="width: 2%; height: 14px">
                    <asp:ImageButton ID="ibtnLast" runat="server" ImageUrl="~/images/new_first.png" /></td>
                <td style="width: 2%; height: 14px"></td>
                <td style="width: 100%; height: 14px"></td>
                <td style="width: 100%; height: 14px"></td>
            </tr>
        </table>
        <table class="mainbg" style="width: 100%">
            <tr>
                <td class="vline" style="width: 98%; height: 1px"></td>
            </tr>
        </table>
        <table class="mainbg" style="width: 100%">
            <tr>
                <td style="width: 589px; height: 39px">
                    <asp:SiteMapPath ID="SiteMapPath1" runat="server">
                    </asp:SiteMapPath>
                </td>
                <td align="right" class="pagetext" style="height: 39px">
                    <asp:Label ID="lblMenuName" runat="server" Text="User Rights" Width="350px"></asp:Label></td>
            </tr>
        </table>
    </asp:Panel>

    <asp:Panel ID="PnlAdd" runat="server" Width="100%">
        <table style="width: 100%">
            <tr>
                <td style="width: 100%">
                    <div style="border: thin solid #A6D9F4; width: 100%">
                        <table class="fields" style="width: 100%; height: 100%">
                            <tr>
                                <td style="width: 3px; height: 26px"></td>
                                <td class="fields" style="width: 67px; height: 26px"></td>
                                <td style="width: 43px; height: 26px">
                                    <asp:Label ID="lblMsg" runat="server" CssClass="lblError" Style="text-align: center"
                                        Width="348px"></asp:Label></td>
                                <td style="width: 100px; height: 26px"></td>
                            </tr>
                            <tr>
                                <td style="width: 3px; height: 26px"></td>
                                <td class="fields" style="width: 67px; height: 26px">
                                    <asp:Label ID="Label2" runat="server" Text="Department " Width="97px"></asp:Label></td>
                                <td style="width: 43px; height: 26px">
                                    <span style="font-size: 11pt; color: #ff0066">
                                        <asp:DropDownList ID="ddlDepartment" runat="server" Width="188px" AutoPostBack="True">
                                        </asp:DropDownList></span></td>
                                <td style="width: 100px; height: 26px"></td>
                            </tr>
                            <tr>
                                <td style="width: 3px; height: 26px"></td>
                                <td class="fields" style="width: 67px; height: 26px">
                                    <asp:Label ID="Label1" runat="server" Text="Group " Width="97px"></asp:Label></td>
                                <td style="width: 43px; height: 26px">
                                    <span style="font-size: 11pt; color: #ff0066">
                                        <asp:DropDownList ID="ddlRoles" runat="server" Width="188px" AutoPostBack="True">
                                        </asp:DropDownList></span></td>
                                <td style="width: 100px; height: 26px"></td>
                            </tr>
                        </table>
                    </div>
                </td>
            </tr>
        </table>
    </asp:Panel>
    <asp:Panel ID="PnlView" runat="server" Width="100%">
        <table style="width: 100%;">
            <tr>
                <td style="width: 3px;"></td>
                <td>
                    <asp:CheckBox ID="cbSelectAll" runat="server" Text="Select All" onclick="checkAll(this);" />
                </td>
            </tr>
            <tr>
                <td style="width: 3px;"></td>
                <td style="vertical-align: text-top;"><%--class="fields"--%>
                    <asp:DataGrid ID="dgRoles" runat="server" Font-Size="8pt" Font-Names="verdana" Width="98%"
                        AllowSorting="True" AutoGenerateColumns="False" Height="17px">
                        <FooterStyle CssClass="dgFooterStyle" Height="20px" />
                        <ItemStyle CssClass="dgItemStyle" />
                        <SelectedItemStyle CssClass="dgSelectedItemStyle" />
                        <Columns>
                            <asp:BoundColumn DataField="MenuID" HeaderText="Menu ID" Visible="False">
                                <HeaderStyle Font-Bold="True" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                    Font-Underline="False" />
                            </asp:BoundColumn>
                            <asp:BoundColumn DataField="MenuName" HeaderText="Menu Name">
                                <HeaderStyle Font-Bold="True" Font-Italic="False" Font-Overline="False" Font-Strikeout="False" Font-Underline="False" />
                            </asp:BoundColumn>
                            <asp:BoundColumn DataField="PageName" HeaderText="Page Name"></asp:BoundColumn>
                            <asp:BoundColumn DataField="IsAdd" HeaderText="IsAdd" Visible="False"></asp:BoundColumn>
                            <asp:BoundColumn DataField="IsEdit" HeaderText="IsEdit" Visible="False"></asp:BoundColumn>
                            <asp:BoundColumn DataField="IsDelete" HeaderText="IsDelete" Visible="False"></asp:BoundColumn>
                            <asp:BoundColumn DataField="IsView" HeaderText="IsView" Visible="False"></asp:BoundColumn>
                            <asp:BoundColumn DataField="IsPrint" HeaderText="IsPrint" Visible="False"></asp:BoundColumn>
                            <asp:BoundColumn DataField="IsAddModeDefault" HeaderText="DefaultMode" Visible="False"></asp:BoundColumn>
                            <asp:BoundColumn DataField="IsPost" HeaderText="Post" Visible="False"></asp:BoundColumn>
                            <asp:BoundColumn DataField="IsOthers" HeaderText="Others" Visible="False"></asp:BoundColumn>
                            <asp:TemplateColumn>
                                <HeaderTemplate>
                                    <asp:Label ID="lblAdd" runat="server" Font-Bold="True" Text="Add" Width="42px"></asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:CheckBox name="chkAdd" ID="Add" runat="server" classname="classAdd" />
                                </ItemTemplate>
                            </asp:TemplateColumn>
                            <asp:TemplateColumn>
                                <HeaderTemplate>
                                    <asp:Label ID="lblView" runat="server" Font-Bold="True" Text="View" Width="42px"></asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    &nbsp;<asp:CheckBox ID="View" runat="server"></asp:CheckBox>
                                </ItemTemplate>
                                <HeaderStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False" Font-Underline="False" />
                            </asp:TemplateColumn>
                            <asp:TemplateColumn>
                                <HeaderTemplate>
                                    <asp:Label ID="lblEdit" runat="server" Font-Bold="True" Text="Edit" Width="33px"></asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:CheckBox ID="Edit" runat="server" />
                                </ItemTemplate>
                            </asp:TemplateColumn>
                            <asp:TemplateColumn>
                                <HeaderTemplate>
                                    <asp:Label ID="lblDelete" runat="server" Font-Bold="True" Text="Delete" Width="56px"></asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:CheckBox ID="Delete" runat="server" />
                                </ItemTemplate>
                            </asp:TemplateColumn>
                            <asp:TemplateColumn>
                                <HeaderTemplate>
                                    <asp:Label ID="lblPrint" runat="server" Font-Bold="True" Text="Print" Width="56px"></asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:CheckBox ID="Print" runat="server" />
                                </ItemTemplate>
                            </asp:TemplateColumn>
                            <asp:TemplateColumn Visible="False">
                                <HeaderTemplate>
                                    <asp:Label ID="lblList" runat="server" Font-Bold="True" Text="List All" Width="56px"></asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:CheckBox ID="List" runat="server" />
                                </ItemTemplate>
                            </asp:TemplateColumn>
                            <asp:TemplateColumn>
                                <HeaderTemplate>
                                    <asp:Label ID="lblPost" runat="server" Font-Bold="True" Text="Post" Width="56px"></asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:CheckBox ID="Post" runat="server" />
                                </ItemTemplate>
                            </asp:TemplateColumn>
                            <asp:TemplateColumn>
                                <HeaderTemplate>
                                    <asp:Label ID="lblOthers" runat="server" Font-Bold="True" Text="Others" Width="56px"></asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:CheckBox ID="Others" runat="server" />
                                </ItemTemplate>
                            </asp:TemplateColumn>
                        </Columns>
                        <AlternatingItemStyle BackColor="Beige" Font-Bold="False" Font-Italic="False" Font-Overline="False"
                            Font-Strikeout="False" Font-Underline="False" CssClass="dgAlternatingItemStyle" />
                        <HeaderStyle BackColor="#CDD7EE" Font-Bold="True" Font-Italic="False" Font-Overline="False"
                            Font-Size="Medium" Font-Strikeout="False" Font-Underline="False" CssClass="dgHeaderStyle" />
                    </asp:DataGrid></td>
            </tr>
        </table>
        <asp:Label ID="userGroupID" runat="server"></asp:Label>
    </asp:Panel>

    <br />
</asp:Content>

