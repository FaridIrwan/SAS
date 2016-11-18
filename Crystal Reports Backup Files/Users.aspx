<%@ Page Language="VB" MasterPageFile="~/MasterPage3.master" AutoEventWireup="false" MaintainScrollPositionOnPostback="true"
    CodeFile="Users.aspx.vb" Inherits="Users" Title="User Setup" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <script language="javascript" type="Scripts/popcalendar.js"></script>
    <script language="javascript" type="Scripts/functions.js"></script>

    <script language="javascript" type="text/javascript">

        function CheckBoxCheck(rb) {
            debugger;
            var gv = document.getElementById("<%=dgDataGrid.ClientID%>");
            var chk = gv.getElementsByTagName("input");
            var row = rb.parentNode.parentNode;
            for (var i = 0; i < chk.length; i++) {
                if (chk[i].type == "checkbox") {
                    if (chk[i].checked && chk[i] != rb) {
                        chk[i].checked = false;
                        break;
                    }
                }
            }
        }

        function validate() {
            var re = /\s*((\S+\s*)*)/;
            if (document.getElementById("<%=txtUserName.ClientID%>").value.replace(re, "$1").length == 0) {
                alert("User Name Field Cannot Be Blank");
                document.getElementById("<%=txtUserName.ClientID%>").focus();
                return false;
            }
            var digits = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            var temp;
            for (var i = 0; i < document.getElementById("<%=txtUserName.ClientID %>").value.length; i++) {
                temp = document.getElementById("<%=txtUserName.ClientID%>").value.substring(i, i + 1);
                if (digits.indexOf(temp) == -1) {
                    alert("Enter Valid User Name");
                    document.getElementById("<%=txtUserName.ClientID%>").focus();
                    return false;
                }
            }

            if (document.getElementById("<%=txtPassword.ClientID%>").value.replace(re, "$1").length == 0) {
                alert("Password Field Cannot Be Blank");
                document.getElementById("<%=txtPassword.ClientID%>").focus();
                return false;
            }
            var digits = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            var temp;
            for (var i = 0; i < document.getElementById("<%=txtPassword.ClientID %>").value.length; i++) {
                temp = document.getElementById("<%=txtPassword.ClientID%>").value.substring(i, i + 1);
                if (digits.indexOf(temp) == -1) {
                    alert("Enter Valid Password");
                    document.getElementById("<%=txtPassword.ClientID%>").focus();
                    return false;
                }
            }
            var password = document.getElementById("<%=txtPassword.ClientID%>").value;

            if (password.length < 6) {
                alert("Password Must Contain Atleast 6 Characters.");
                document.getElementById("<%=txtPassword.ClientID%>").focus();
                return false;
            }
            if (document.getElementById("<%=ddlUserGroup.ClientID%>").value == "-1") {
                alert("Select a Valid User Group");
                document.getElementById("<%=ddlUserGroup.ClientID%>").focus();
                return false;
            }
            if (document.getElementById("<%=txtEmail.ClientID%>").value.replace(re, "$1").length == 0) {
                alert("Email Field Cannot Be Blank");
                document.getElementById("<%=txtEmail.ClientID%>").focus();
                return false;
            }
            var emailPat = /^(\".*\"|[A-Za-z]\w*)@(\[\d{1,3}(\.\d{1,3}){3}]|[A-Za-z]\w*(\.[A-Za-z]\w*)+)$/;
            var emailid = document.getElementById("<%=txtEmail.ClientID %>").value;
            var matchArray = emailid.match(emailPat);
            if (matchArray == null) {
                alert("Enter Valid Email");
                document.getElementById("<%=txtEmail.ClientID %>").focus();
                return false;
            }
            return true;
        }
    </script>

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
                                <asp:ImageButton ID="ibtnDelete" runat="server" ImageUrl="~/images/delete.png" ToolTip="Delete"/>
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
                    <asp:Label ID="lblMenuName" runat="server" Width="422px" Text="Department"></asp:Label>
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
                    <td style="height: 43px">
                        <table width="100%">
                            <tr>
                                <td width="10%">
                                    <asp:Label ID="Label9" runat="server" Text="Search"></asp:Label>
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
            <asp:HiddenField ID="hdnUserID" runat="server"></asp:HiddenField>
        </div>
    </asp:Panel>

    <asp:Panel ID="pnlEdit" runat="server" Width="100%" Visible="true">
        <table style="width: 100%">
            <tr>
                <td style="width: 100%">
                    <div style="border: thin solid #A6D9F4; width: 100%">
                        <table class="fields" style="width: 100%; height: 100%">
                            <tr>
                                <td style="width: 6px; height: 26px">
                                    <span style="font-size: 11pt; color: #ff0066">*</span></td>
                                <td class="fields" style="width: 67px; height: 26px">
                                    <asp:Label ID="Label1" runat="server" Text="User Name" Width="76px"></asp:Label>
                                </td>
                                <td style="width: 43px; height: 26px">
                                    <asp:TextBox ID="txtUserName" runat="server" Width="142px" MaxLength="20"></asp:TextBox>
                                </td>
                                <td style="width: 100px; height: 26px"></td>
                            </tr>
                            <tr>
                                <td style="width: 6px; height: 26px">
                                    <span style="font-size: 11pt; color: #ff0066">*</span></td>
                                <td class="fields" style="width: 67px; height: 26px">
                                    <asp:Label ID="Label4" runat="server" Text="Password" Width="76px"></asp:Label>
                                </td>
                                <td style="width: 43px; height: 26px">
                                    <asp:TextBox ID="txtPassword" runat="server" MaxLength="20" Width="142px"></asp:TextBox>
                                </td>
                                <td style="width: 100px; height: 26px"></td>
                            </tr>
                            <tr>
                                <td style="width: 6px; height: 26px">
                                    <span style="font-size: 11pt; color: #ff0066">*</span></td>
                                <td class="fields" style="width: 67px; height: 26px">
                                    <asp:Label ID="Label12" runat="server" Text="Department" Width="76px"></asp:Label>
                                </td>
                                <td style="width: 43px; height: 26px">
                                    <asp:DropDownList ID="ddlDepartment" runat="server" Width="147px" AutoPostBack="true">
                                    </asp:DropDownList>
                                </td>
                                <td style="width: 100px; height: 26px"></td>
                            </tr>
                            <tr>
                                <td style="width: 6px; height: 26px">
                                    <span style="font-size: 11pt; color: #ff0066">*</span></td>
                                <td class="fields" style="width: 67px; height: 26px">
                                    <asp:Label ID="Label8" runat="server" Text="User Group" Width="76px"></asp:Label>
                                </td>
                                <td style="width: 43px; height: 26px">
                                    <asp:DropDownList ID="ddlUserGroup" runat="server" Width="147px">
                                    </asp:DropDownList>
                                </td>
                                <td style="width: 100px; height: 26px"></td>
                            </tr>
                            <tr>
                                <td style="width: 6px; height: 21px">
                                    <span style="font-size: 11pt; color: #ff0066">*</span></td>
                                <td class="fields" style="width: 67px; height: 21px">
                                    <asp:Label ID="Label2" runat="server" Text="Email" Width="117px"></asp:Label>
                                </td>
                                <td style="width: 43px; height: 21px">
                                    <asp:TextBox ID="txtEmail" runat="server" Width="346px" MaxLength="50"></asp:TextBox>
                                </td>
                                <td style="width: 100px; height: 21px"></td>
                            </tr>
                            <tr>
                                <td style="width: 6px; height: 21px">
                                    <span style="font-size: 11pt; color: #ff0066">*</span></td>
                                <td class="fields" style="width: 67px; height: 21px">
                                    <asp:Label ID="Label7" runat="server" Style="vertical-align: middle" Text="Status"
                                        Width="78px"></asp:Label>
                                </td>
                                <td style="width: 43px; height: 21px">
                                    <span style="font-size: 11pt; color: #ff0066">
                                        <asp:DropDownList ID="ddlStatus" runat="server" Width="146px">
                                            <asp:ListItem Value="1">Active</asp:ListItem>
                                            <asp:ListItem Value="0">Inactive</asp:ListItem>
                                        </asp:DropDownList>
                                    </span>
                                </td>
                                <td style="width: 100px; height: 21px"></td>
                            </tr>
                        </table>
                    </div>
                </td>
            </tr>
        </table>
    </asp:Panel>

</asp:Content>
