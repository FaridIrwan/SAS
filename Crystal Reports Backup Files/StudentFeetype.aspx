<%@ Page Language="VB" AutoEventWireup="false" CodeFile="StudentFeetype.aspx.vb" MaintainScrollPositionOnPostback="true"
    Inherits="StudentFeetype" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <link href="style.css" rel="stylesheet" type="text/css" />
</head>
<body topmargin="0" leftmargin="0" alink="#00ff66">
    <form id="form1" runat="server">
    <script type="text/javascript" language="javascript">
        function closeChildWindow() {
            //alert('called');
            if (window.opener == "undefined" || window.opener == null) {
                //alert('Session is expired');
                //window.parent.history.go(0);
                window.dialogArguments.location.reload(true);
                //window.parent.location.reload(true);
                //window.parent.submit();
            }
            else {
                window.opener.location.reload(true);
            }

            window.close();

            return true;
        }
    </script>
    <div style="text-align: left">
        <table style="width: 400px">
            <tr>
                <td style="width: 100px; height: 14px">
                </td>
                <td style="width: 100px; height: 14px">
                    <table style="width: 418px; height: 57px">
                        <tr>
                            <td style="width: 78px">
                            </td>
                            <td style="width: 100px">
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 78px">
                                <asp:Label ID="Label1" runat="server" Text="Fee Code"></asp:Label>
                            </td>
                            <td style="width: 100px">
                                <asp:TextBox ID="txtFeeCode" runat="server" MaxLength="20" Width="142px"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 78px; height: 30px;">
                                <asp:Label ID="Label4" runat="server" Text="Fee Description"></asp:Label>
                            </td>
                            <td style="width: 100px; height: 30px;">
                                <asp:TextBox ID="txtFeeDesc" runat="server" MaxLength="50" Width="234px"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 78px">
                                <asp:Label ID="Label5" runat="server" Text="Fee Type"></asp:Label>
                            </td>
                            <td style="width: 100px">
                                <asp:DropDownList ID="ddlFeeType" runat="server" Width="111px">
                                    <asp:ListItem Value="-1">--Select--</asp:ListItem>
                                    <asp:ListItem Value="01">Deposit </asp:ListItem>
                                    <asp:ListItem Value="02">Non Deposit </asp:ListItem>
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 78px">
                                <asp:Label ID="Label9" runat="server" Text="Fee Category"></asp:Label>
                            </td>
                            <td style="width: 100px">
                                <asp:DropDownList ID="ddlFeeCategory" runat="server">
                                    <asp:ListItem Value="-1">--Select--</asp:ListItem>
                                    <asp:ListItem Value="A">Admission Fee</asp:ListItem>
                                    <asp:ListItem Value="T">Tuition Fee</asp:ListItem>
                                    <asp:ListItem Value="H">Hostel</asp:ListItem>
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 78px; height: 22px;">
                                <asp:Label ID="Label6" runat="server" Text="Student Category" Width="87px"></asp:Label>
                            </td>
                            <td style="width: 100px; height: 22px;">
                                <asp:DropDownList ID="ddlStudentCategory" runat="server" Width="110px">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 78px; text-align: right">
                                <asp:ImageButton ID="ibtnView" runat="server" ImageUrl="~/images/find.png" ToolTip="View" />
                            </td>
                            <td style="width: 100px">
                                <asp:Label ID="Label16" runat="server" Text="Search"></asp:Label>
                            </td>
                        </tr>
                    </table>
                    <hr />
                </td>
            </tr>
            <tr>
                <td style="width: 100px; height: 16px">
                </td>
                <td style="width: 100px; height: 16px">
                    &nbsp;<table style="width: 416px; height: 25px">
                        <tr>
                            <td style="width: 100px; height: 123px">
                                <div style="overflow: auto; width: 500px; height: 200px;">
                                    <asp:DataGrid ID="dgView" runat="server" AutoGenerateColumns="False" DataKeyField="FeeTypeCode"
                                        Width="420px" Height="46px">
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
                                            <asp:ButtonColumn CommandName="Select" DataTextField="FeeTypeCode" HeaderText="Fee Code"
                                                Text="FeeTypeCode"></asp:ButtonColumn>
                                            <asp:BoundColumn DataField="Description" HeaderText="Fee Desc"></asp:BoundColumn>
                                            <asp:BoundColumn DataField="FSAmount" HeaderText="Fee Amount"></asp:BoundColumn>
                                            <asp:BoundColumn DataField="Priority" HeaderText="Priority"></asp:BoundColumn>
                                        </Columns>
                                    </asp:DataGrid></div>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td style="width: 100px; height: 16px; text-align: center">
                </td>
                <td style="width: 100px; height: 16px; text-align: center">
                    <table style="width: 100%; height: 100%">
                        <tr>
                            <td style="width: 100px; text-align: right">
                            </td>
                            <td style="width: 100px; text-align: left">
                                &nbsp;<table style="width: 100%; height: 100%">
                                    <tr>
                                        <td style="width: 8px; height: 30px; text-align: right">
                                            <asp:ImageButton ID="ibtnOK" runat="server" ImageUrl="~/images/add_list.gif" Width="24px"
                                                Height="24px" />
                                        </td>
                                        <td style="width: 49px; height: 30px; text-align: left">
                                            <asp:Label ID="Label2" runat="server" Text="Select" Width="21px"></asp:Label>
                                        </td>
                                        <td style="width: 34px; height: 30px; text-align: left">
                                            <asp:ImageButton ID="ibtnClose" runat="server" Height="24px" ImageUrl="~/images/ok_cancel.jpg"
                                                Width="24px" />
                                        </td>
                                        <td style="width: 158px; height: 30px; text-align: left">
                                            <asp:Label ID="Label3" runat="server" Text="Close" Width="21px"></asp:Label>
                                        </td>
                                        <td style="width: 142px; height: 30px; text-align: left">
                                        </td>
                                        <td style="width: 100px; height: 30px; text-align: left">
                                        </td>
                                    </tr>
                                </table>
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
