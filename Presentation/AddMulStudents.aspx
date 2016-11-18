<%@ Page Language="VB" AutoEventWireup="false" CodeFile="AddMulStudents.aspx.vb" MaintainScrollPositionOnPostback="true"
    Inherits="AddMulsudent" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <base target="_self" />
    <title>Search Student</title>
    <link href="style.css" rel="stylesheet" type="text/css" />
    <script language="javascript" type="text/javascript">
        var textFieldInFocus;
        function handleOnFocus(form_element) {
            textFieldInFocus = form_element;
        }
        function handleOnBlur() {
            textFieldInFocus = null;
        }
        function SelectAllCheckboxes1(chk) {
            $('#<%=dgStudentView.ClientID%>').find("input:checkbox").each(function () {
                if (this != chk) { this.checked = chk.checked; }
            });
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <script language="javascript" type="text/javascript">
        function RefreshParent() {
            window.opener.document.getElementById('ctl00_ContentPlaceHolder1_btnHidden').click();
            window.close();
        }
    </script>
    <div>
        &nbsp;</div>
    <asp:Panel ID="stPnl" runat="server" Height="50px" Width="125px">
        <table>
            <tr>
                <td style="width: 674px;">
                    <asp:Panel ID="pnlStu" runat="server" Width="100%">
                        <table style="width: 100%; height: 150px;">
                            <tr>
                                <td style="width: 6px; height: 21px">
                                </td>
                                <td style="width: 3px; height: 21px">
                                    <asp:Label ID="Label6" runat="server" Font-Bold="False" Text=" Matric No" Width="113px"></asp:Label>
                                </td>
                                <td style="width: 270px; height: 21px">
                                    <asp:TextBox ID="txtMatricNo" runat="server"></asp:TextBox>
                                    <!--<input name="txtMatricNo" onkeyup="Filterchanged()" runat="server" />-->
                                </td>
                                <td style="width: 100px; height: 21px">
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 6px; height: 18px">
                                </td>
                                <td style="width: 3px; height: 18px;">
                                    <asp:Label ID="Label7" runat="server" Font-Bold="False" Text="Name" Width="113px"></asp:Label>
                                </td>
                                <td style="width: 270px; height: 18px;">
                                    <asp:TextBox ID="txtname" runat="server"></asp:TextBox>
                                </td>
                                <td style="width: 100px; height: 18px">
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 6px; height: 18px">
                                </td>
                                <td style="width: 3px; height: 18px">
                                    <asp:Label ID="Label9" runat="server" Font-Bold="False" Text="IC/ Passport" Width="113px"></asp:Label>
                                </td>
                                <td style="width: 270px; height: 18px">
                                    <asp:TextBox ID="txtPassport" runat="server"></asp:TextBox>
                                </td>
                                <td style="width: 100px; height: 18px">
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 6px; height: 18px">
                                </td>
                                <td style="width: 3px; height: 18px;">
                                    <asp:Label ID="Label8" runat="server" Font-Bold="False" Text="Faculty" Width="113px"></asp:Label>
                                </td>
                                <td style="width: 270px; height: 18px;">
                                    <asp:DropDownList ID="ddlFaculty" AutoPostBack="true" runat="server" AppendDataBoundItems="True" Width="140px">
                                        <asp:ListItem Value="-1">--Select--</asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                                <td style="width: 100px; height: 18px">
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 6px; height: 16px; text-align: left">
                                </td>
                                <td style="width: 3px; height: 16px; text-align: left">
                                    <asp:Label ID="Label10" runat="server" Font-Bold="False" Text="Program" Width="113px"></asp:Label>
                                </td>
                                <td style="width: 270px; height: 16px">
                                    <asp:DropDownList ID="ddlProgram" runat="server" AppendDataBoundItems="True" Width="140px">
                                        <asp:ListItem Value="-1">--Select--</asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                                <td style="width: 100px; height: 16px">
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 6px; height: 16px; text-align: left">
                                </td>
                                <td style="width: 3px; height: 16px; text-align: left">
                                    <asp:Label ID="Label16" runat="server" Font-Bold="False" Text="Kolej" Width="113px"></asp:Label>
                                </td>
                                <td style="width: 270px; height: 16px">
                                    <asp:DropDownList ID="ddlHostelCOde" runat="server" AutoPostBack="true" AppendDataBoundItems="True" Width="140px">
                                        <asp:ListItem Value="-1">--Select--</asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                                <td style="width: 100px; height: 16px">
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 6px; height: 16px; text-align: left">
                                </td>
                                <td style="width: 3px; height: 16px; text-align: left">
                                    <asp:Label ID="Label17" runat="server" Font-Bold="False" Text="Blok" Width="113px"></asp:Label>
                                </td>
                                <td style="width: 270px; height: 16px">
                                    <asp:DropDownList ID="ddlBloackCode" runat="server" AutoPostBack="true" AppendDataBoundItems="True" Width="140px">
                                        <asp:ListItem Value="-1">--Select--</asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                                <td style="width: 100px; height: 16px">
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 6px; height: 16px; text-align: left">
                                </td>
                                <td style="width: 3px; height: 16px; text-align: left">
                                    <asp:Label ID="Label18" runat="server" Font-Bold="False" Text="Room Type" Width="113px"></asp:Label>
                                </td>
                                <td style="width: 270px; height: 16px">
                                    <asp:DropDownList ID="ddlRoomType" runat="server" AppendDataBoundItems="True" Width="140px">
                                        <asp:ListItem Value="-1">--Select--</asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                                <td style="width: 100px; height: 16px">
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 6px; height: 16px; text-align: left">
                                </td>
                                <td style="width: 3px; height: 16px; text-align: left">
                                    <asp:Label ID="Label11" runat="server" Font-Bold="False" Text="Semester" Width="113px"
                                        Visible="False"></asp:Label>
                                </td>
                                <td style="width: 270px; height: 16px">
                                    <asp:TextBox ID="txtsem" runat="server" Visible="False"></asp:TextBox>
                                </td>
                                <td style="width: 100px; height: 16px">
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 6px; height: 16px; text-align: right">
                                </td>
                                <td style="width: 3px; height: 16px; text-align: left; vertical-align: top;">
                                    &nbsp;<table>
                                        <tr>
                                            <td style="width: 17px">
                                                <asp:ImageButton ID="ibtnLoad" runat="server" ImageUrl="~/images/find.gif" TabIndex="5" />
                                            </td>
                                            <td style="width: 100px">
                                                <asp:Label ID="Label12" runat="server" Text="Search" Width="55px"></asp:Label>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                                <td style="width: 270px; height: 16px">
                                </td>
                                <td style="width: 100px; height: 16px">
                                </td>
                            </tr>
                        </table>
                        <table style="width: 100%">
                            <tr>
                                <td style="height: 137px">
                                    <div style="overflow: auto; width: 500px; height: 200px;">
                                    <asp:CheckBox ID="chkStudent" runat="server" OnCheckedChanged="chkStudent_CheckedChanged"
                                                AutoPostBack="True" Text="Select All" />
                                        <asp:DataGrid ID="dgStudentView" runat="server" AutoGenerateColumns="False" Width="100%">
                                            <FooterStyle CssClass="dgFooterStyle" Height="20px" />
                                            <SelectedItemStyle CssClass="dgSelectedItemStyle" />
                                            <AlternatingItemStyle BackColor="Beige" CssClass="dgAlternatingItemStyle" Font-Bold="False"
                                                Font-Italic="False" Font-Overline="False" Font-Strikeout="False" Font-Underline="False" />
                                            <ItemStyle CssClass="dgItemStyle" />
                                            <HeaderStyle BackColor="#CDD7EE" CssClass="dgHeaderStyle" Font-Bold="True" Font-Italic="False"
                                                Font-Overline="False" Font-Size="Medium" Font-Strikeout="False" Font-Underline="False" />
                                            <Columns>
                                                <asp:TemplateColumn HeaderText="Select">
                                                    <HeaderTemplate>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:CheckBox ID="chk" runat="server" />
                                                    </ItemTemplate>
                                                </asp:TemplateColumn>
                                                <asp:BoundColumn DataField="MatricNo" HeaderText="Matric No"></asp:BoundColumn>
                                                <asp:BoundColumn DataField="StudentName" HeaderText="Name"></asp:BoundColumn>
                                                <asp:BoundColumn DataField="ICNo" HeaderText="IC No"></asp:BoundColumn>
                                                <asp:BoundColumn DataField="Faculty" HeaderText="Faculty"></asp:BoundColumn>
                                                <asp:BoundColumn DataField="ProgramID" HeaderText="Program"></asp:BoundColumn>
                                                <asp:BoundColumn DataField="CurrentSemester" HeaderText="Semester"></asp:BoundColumn>
                                                <asp:BoundColumn DataField="CurretSemesterYear" HeaderText="Semester" Visible="false">
                                                </asp:BoundColumn>
                                                <asp:BoundColumn DataField="CategoryCode" HeaderText="Category"></asp:BoundColumn>
                                            </Columns>
                                        </asp:DataGrid></div>
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 100px; height: 1px">
                                    <table style="width: 100%; height: 2%">
                                        <tr>
                                            <td style="width: 6px; height: 30px; text-align: left">
                                                <asp:ImageButton ID="ibtnStudetSelect" runat="server" Height="24px" ImageUrl="~/images/add_list.gif"
                                                    Width="24px" />
                                            </td>
                                            <td style="width: 35px; height: 30px; text-align: left">
                                                <asp:Label ID="Label13" runat="server" Text="Select" Width="21px"></asp:Label>
                                            </td>
                                            <td style="width: 11px; height: 30px; text-align: left">
                                                &nbsp;<asp:ImageButton ID="ibtnClose" runat="server" Height="24px" ImageUrl="~/images/ok_cancel.jpg"
                                                    Width="24px" />
                                            </td>
                                            <td style="width: 293px; height: 30px; text-align: left">
                                                <asp:Label ID="Label15" runat="server" Text="Close"></asp:Label>
                                            </td>
                                            <td style="width: 100px; height: 30px; text-align: left">
                                                &nbsp;
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                </td>
            </tr>
            <tr>
                <td style="width: 674px; height: 16px;">
                </td>
            </tr>
        </table>
    </asp:Panel>
    <asp:Panel ID="spPnl" runat="server" Height="50px" Width="514px">
        <table style="width: 400px">
            <tr>
                <td style="width: 100px; height: 16px">
                </td>
                <td style="width: 101px; height: 16px">
                    <table style="width: 100%; height: 150px">
                        <tr>
                            <td style="width: 3px; height: 21px">
                            </td>
                            <td style="width: 270px; height: 21px">
                            </td>
                            <td style="width: 100px; height: 21px">
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 3px; height: 21px">
                                <asp:Label ID="Label3" runat="server" Text="Sponsor Code" Width="93px"></asp:Label>
                            </td>
                            <td style="width: 270px; height: 21px">
                                <asp:TextBox ID="txtSpnCode" runat="server"></asp:TextBox>
                            </td>
                            <td style="width: 100px; height: 21px">
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 3px; height: 18px">
                                <asp:Label ID="Label4" runat="server" Text="Sponsor Name" Width="104px"></asp:Label>
                            </td>
                            <td style="width: 270px; height: 18px">
                                <asp:TextBox ID="txtSpnName" runat="server"></asp:TextBox>
                            </td>
                            <td style="width: 100px; height: 18px">
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 3px; height: 16px">
                                <asp:Label ID="Label5" runat="server" Text="Sponsor Type" Width="104px"></asp:Label>
                            </td>
                            <td style="width: 270px; height: 16px">
                                <asp:TextBox ID="txtSpnType" runat="server"></asp:TextBox>
                            </td>
                            <td style="width: 100px; height: 16px">
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 3px; height: 16px; text-align: right">
                                <asp:ImageButton ID="ibtnSearch" runat="server" ImageUrl="~/images/find.gif" />
                            </td>
                            <td style="width: 270px; height: 16px">
                                <asp:Label ID="Label14" runat="server" Text="Search" Width="104px"></asp:Label>
                            </td>
                            <td style="width: 100px; height: 16px">
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 3px; height: 16px; text-align: right">
                            </td>
                            <td style="width: 270px; height: 16px">
                            </td>
                            <td style="width: 100px; height: 16px">
                            </td>
                        </tr>
                    </table>
                    <hr />
                </td>
            </tr>
            <tr>
                <td style="width: 100px; height: 16px">
                </td>
                <td style="width: 101px; height: 16px">
                    <div id="dvView" style="overflow: auto; width: 500px; height: 150px">
                        <asp:DataGrid ID="dgView" runat="server" AutoGenerateColumns="False" DataKeyField="SponserCode"
                            Width="420px">
                            <FooterStyle CssClass="dgFooterStyle" Height="20px" />
                            <SelectedItemStyle CssClass="dgSelectedItemStyle" />
                            <AlternatingItemStyle BackColor="Beige" CssClass="dgAlternatingItemStyle" Font-Bold="False"
                                Font-Italic="False" Font-Overline="False" Font-Strikeout="False" Font-Underline="False" />
                            <ItemStyle CssClass="dgItemStyle" />
                            <HeaderStyle BackColor="#CDD7EE" CssClass="dgHeaderStyle" Font-Bold="True" Font-Italic="False"
                                Font-Overline="False" Font-Size="Medium" Font-Strikeout="False" Font-Underline="False" />
                            <Columns>
                                <asp:ButtonColumn CommandName="Select" DataTextField="SponserCode" HeaderText="Sponsor Code"
                                    Text="SASR_Code"></asp:ButtonColumn>
                                <asp:BoundColumn DataField="Name" HeaderText="Sponsor Name"></asp:BoundColumn>
                                <asp:BoundColumn DataField="Type" HeaderText="Sponsor Type"></asp:BoundColumn>
                            </Columns>
                        </asp:DataGrid>
                    </div>
                </td>
            </tr>
            <tr>
                <td style="width: 100px; height: 16px; text-align: center">
                </td>
                <td style="width: 101px; height: 16px; text-align: center">
                    <hr />
                    <table style="width: 100%; height: 100%">
                        <tr>
                            <td style="width: 8px; height: 30px; text-align: right">
                                <asp:ImageButton ID="ibtnOK" runat="server" Height="24px" ImageUrl="~/images/add_list.gif"
                                    Width="24px" />
                            </td>
                            <td style="width: 49px; height: 30px; text-align: left">
                                <asp:Label ID="Label1" runat="server" Text="Select" Width="21px"></asp:Label>
                            </td>
                            <td style="width: 34px; height: 30px; text-align: left">
                                <asp:ImageButton ID="ImageButton1" runat="server" Height="24px" ImageUrl="~/images/ok_cancel.jpg"
                                    Width="24px" />
                            </td>
                            <td style="width: 158px; height: 30px; text-align: left">
                                <asp:Label ID="Label2" runat="server" Text="Close" Width="21px"></asp:Label>
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
    </asp:Panel>
    </form>
</body>
</html>
