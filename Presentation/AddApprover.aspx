<%@ Page Language="VB" AutoEventWireup="false" CodeFile="AddApprover.aspx.vb" MaintainScrollPositionOnPostback="true" Inherits="AddApprover" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <base target="_self" />
    <title></title>
    <link href="style.css" rel="stylesheet" type="text/css" />
    <script language="javascript" type="text/javascript">
        var textFieldInFocus;
        function handleOnFocus(form_element) {
            textFieldInFocus = form_element;
        }
        function handleOnBlur() {
            textFieldInFocus = null;
        }

        function CheckAll(oCheckbox) {
            var gv = document.getElementById("<%=gvUser.ClientID %>");
            for (i = 1; i < gv.rows.length; i++) {
                gv.rows[i].cells[0].getElementsByTagName("INPUT")[0].checked = oCheckbox.checked;
            }
        }
    </script>
    <style type="text/css">
        .auto-style6 {
            height: 21px;
            width: 65%;
        }
        .auto-style7 {
            height: 30px;
            width: 20px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <script language="javascript" type="text/javascript">
            function RefreshParent() {
                window.opener.document.getElementById('ctl00_ContentPlaceHolder1_btnHiddenApp').click();
                window.close();
            }
        </script>

    <h4 id="h4" runat="server">Approval List</h4>
    <asp:Panel ID="PnlApprover" runat="server" Height="50px" Width="400px">
        <table>
            <tr>
                <td style="width: 6px; height: 21px"></td>
                <td style="width: 3px; height: 21px">
                    <asp:Label ID="lblProcessName" runat="server" Font-Bold="False" Text="Process Name" Width="113px"></asp:Label>
                </td>
                <td style="width: 270px; height: 21px">
                    <asp:TextBox ID="txtProcessName" runat="server" Enabled="false"></asp:TextBox>
                </td>
                <td style="width: 100px; height: 21px"></td>
            </tr>

            <tr>
                <td style="width: 6px; height: 21px"></td>
                <td style="width: 3px; height: 21px">
                    <asp:Label ID="lblTimeSendApproval" runat="server" Font-Bold="False" Text="Time Send Approval" Width="113px"></asp:Label>
                </td>
                <td style="width: 270px; height: 21px">
                    <asp:TextBox ID="txtTimeSendApproval" runat="server" Enabled="false"></asp:TextBox>
                </td>
                <td style="width: 100px; height: 21px"></td>
            </tr>

            <tr>
                <td style="width: 6px; height: 21px"></td>
                <td style="width: 3px; height: 21px">
                    <asp:Label ID="lblPreparedBy" runat="server" Font-Bold="False" Text="Prepared By" Width="113px"></asp:Label>
                </td>
                <td style="width: 270px; height: 21px">
                    <asp:TextBox ID="txtPreparedBy" runat="server" Enabled="false"></asp:TextBox>
                </td>
                <td style="width: 100px; height: 21px"></td>
            </tr>

            <tr>
                <td style="width: 6px; height: 21px"></td>
                <td style="width: 3px; height: 21px">
                    <asp:Label ID="lblPostBy" runat="server" Font-Bold="False" Text="Posting By" Width="113px"></asp:Label>
                </td>
                <td style="width: 270px; height: 21px">
                    <asp:TextBox ID="txtPostBy" runat="server" Enabled="false"></asp:TextBox>
                </td>
                <td style="width: 100px; height: 21px"></td>
            </tr>
        </table>
        <table style="width: 100%;">
            <tr>
                <td style="width: 6px; height: 21px">
                    <asp:Label ID="lblMsg" runat="server" CssClass="lblError" Visible="false"></asp:Label>
                </td>
            </tr>
            <tr>
                <td class="auto-style6">

                    <asp:GridView ID="gvUser" runat="server" AutoGenerateColumns="false" Width="80%" Style="vertical-align: text-top">
                        
                        <FooterStyle CssClass="dgFooterStyle" Height="20px" />
                        <SelectedRowStyle CssClass="dgSelectedItemStyle" />
                        <AlternatingRowStyle BackColor="Beige" CssClass="dgAlternatingItemStyle" Font-Bold="False"
                            Font-Italic="False" Font-Overline="False" Font-Strikeout="False" Font-Underline="False" />
                        <RowStyle CssClass="dgItemStyle" HorizontalAlign="Center" />
                        <HeaderStyle BackColor="#00699b" CssClass="dgHeaderStyle" ForeColor="#ffffff" Font-Bold="False" Font-Italic="False"
                            Font-Overline="False" Font-Size="8pt" Font-Strikeout="False" Font-Underline="False" />
                        <Columns>
                            <asp:TemplateField HeaderText="Select">
                                <HeaderTemplate>
                                    <input id="cbSelectAll" type="checkbox" onclick="CheckAll(this);" />
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:CheckBox ID="cbSelect" runat="server" AutoPostBack="true" ></asp:CheckBox>
                                </ItemTemplate>
                                <HeaderStyle Width="5%" />
                            </asp:TemplateField>
                            <asp:BoundField DataField="UsersEn.UserName" HeaderText="Username" />
                            <asp:TemplateField HeaderText="Usergroup">
                                <ItemTemplate>
                                    <asp:Label ID="lblUg" runat="server" Text='<%#(Eval("UserGroupsEn.UserGroupName"))%>' ></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="UsersEn.UserStatus" HeaderText="User Status" />
                        </Columns>
                    </asp:GridView>
                </td>
            </tr>
        </table>

        <table style="width: 100%; height: 100%">
            <tr>
                <td style="width: 8px; height: 30px; text-align: right">
                    <asp:ImageButton ID="ibtnOK" runat="server" Height="24px" ImageUrl="~/images/add_list.gif"
                        Width="24px" />
                </td>
                <td style="width: 49px; height: 30px; text-align: left">
                    <asp:Label ID="Label3" runat="server" Text="Select" Width="21px"></asp:Label>
                </td>
                <td style="text-align: left" class="auto-style7">
                    <asp:ImageButton ID="ibtnClose" runat="server" Height="24px" ImageUrl="~/images/ok_cancel.jpg"
                        Width="24px" />
                </td>
                <td style="width: 158px; height: 30px; text-align: left">
                    <asp:Label ID="Label4" runat="server" Text="Cancel" Width="21px"></asp:Label>
                </td>
                <td style="width: 142px; height: 30px; text-align: left">
                </td>
                <td style="width: 100px; height: 30px; text-align: left">
                </td>
            </tr>
        </table>
        <asp:HiddenField ID="hdnPageName" runat="server" />
    </asp:Panel>
    </form>
</body>
</html>
