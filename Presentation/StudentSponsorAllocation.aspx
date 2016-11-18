<%@ Page Language="VB" AutoEventWireup="false" CodeFile="StudentSponsorAllocation.aspx.vb" Inherits="StudentSponsorAllocation" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Student Sponsor Allocation</title>
    <link href="style.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
        <div style="text-align: left">
            <table style="width: 400px">
                <tr>
                    <td style="width: 3px; height: 16px; text-align: center">
                    
                                    <asp:Label ID="lblmatricno" runat="server" Font-Bold="False" Text=" Matric No :" Width="113px"></asp:Label>
                                
                     </td><td style="width: 3px; height: 16px; text-align: center">
                                    <asp:TextBox ID="txtMatricNo" runat="server" Width="100px"></asp:TextBox>
                        </td>
                                    <!--<input name="txtMatricNo" onkeyup="Filterchanged()" runat="server" />-->
                                <td style="width: 3px; height: 16px; text-align: center">
                                <asp:ImageButton ID="ibtnSearch" runat="server" ImageUrl="~/images/find.gif" />
                            </td>
                            <td style="width: 270px; height: 16px">
                                <asp:Label ID="Label14" runat="server" Text="Search" Width="104px"></asp:Label>
                            </td>
                            <td style="width: 100px; height: 16px">
                            </td>
                         </tr>
                
                        </table>
                
                
            <table style="width: 400px">
                 <tr>
                    <td align="center">
                        <asp:Label ID="lblMsg" runat="server" CssClass="lblError" Style="text-align: left"
                            Width="348px"></asp:Label>
                    </td>
                </tr>
                 <tr>
                    <td style="width: 100px; height: 16px">
                        <table style="width: 416px; height: 25px">
                            <tr>
                                <td style="width: 100px; height: 149px; vertical-align: top;">
                                    <asp:CheckBox ID="chkStudent" runat="server"
                                        AutoPostBack="True" Text="Select All" />
                                    <div style="overflow: auto; width: 550px; height:200px ">
                                        <asp:DataGrid ID="dgStudentInfo" runat="server" AutoGenerateColumns="False" Width="500px" AllowPaging="true" PageSize="25"
                                            Height="123px" Style="vertical-align: text-top" ShowFooter="true" Visible ="false" >
                                            <FooterStyle CssClass="dgFooterStyle" Height="20px" />
                                            <PagerStyle Mode="NumericPages" HorizontalAlign="Center" />
                                            <SelectedItemStyle CssClass="dgSelectedItemStyle" />
                                            <AlternatingItemStyle BackColor="Beige" CssClass="dgAlternatingItemStyle" Font-Bold="False"
                                                Font-Italic="False" Font-Overline="False" Font-Strikeout="False" Font-Underline="False" />
                                            <ItemStyle CssClass="dgItemStyle" />
                                            <HeaderStyle BackColor="#CDD7EE" CssClass="dgHeaderStyle" Font-Bold="True" Font-Italic="False"
                                                Font-Overline="False" Font-Size="Medium" Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Center" />
                                            <%-- <HeaderTemplate>
                     
                         <input type="checkbox" id="mainCB" onclick="javascript: CheckAll(this);" />
                    </HeaderTemplate>--%>

                                            <Columns>
                                                <asp:TemplateColumn HeaderText="Select">
                                                    <ItemTemplate>
                                                        &nbsp;<asp:CheckBox ID="chk" runat="server" AutoPostBack="true" OnCheckedChanged="chk_CheckedChanged" />
                                                    </ItemTemplate>
                                                </asp:TemplateColumn>
                                                <asp:BoundColumn DataField="MatricNo" HeaderText="Matric No"></asp:BoundColumn>
                                                <asp:BoundColumn DataField="StudentName" HeaderText="Name"></asp:BoundColumn>
                                                <asp:BoundColumn DataField="ICNo" HeaderText="IC NO"></asp:BoundColumn>
                                                <asp:BoundColumn DataField="ProgramID" HeaderText="Program"></asp:BoundColumn>
                                                <asp:BoundColumn DataField="CurrentSemester" HeaderText="Semester"></asp:BoundColumn> 
                                                
                                                <%--<asp:BoundColumn DataField="OutstandingAmount" HeaderText="Available Amount"></asp:BoundColumn>--%>
                                                <%--<asp:BoundColumn DataField="CategoryCode" HeaderText="Category Code" Visible="false"></asp:BoundColumn>
                                                <asp:BoundColumn DataField="ProgramID" HeaderText="Program ID" Visible="false"></asp:BoundColumn>--%>
                                            </Columns>
                                        </asp:DataGrid>
                                    </div>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td style="width: 100px; text-align: right">
                        <table style=" height: 25px">
                            <tr>
                                <td >
                                    <table>
                                        <tr>
                                            <td style="width: 350px; height: 30px; text-align: left"></td>
                                            <td style="width: 10px; height: 30px; text-align: left">
                                                <asp:ImageButton ID="ibtnOK" runat="server" ImageUrl="~/images/add_list.gif" Width="24px"
                                                    Height="24px" />
                                            </td>
                                            <td style="width: 50px; height: 30px; text-align: left">
                                                <asp:Label ID="Label2" runat="server" Text="Select" Width="21px"></asp:Label>
                                            </td>
                                            <td style="width: 10px; height: 30px; text-align: left">
                                                <asp:ImageButton ID="ibtnClose" runat="server" Height="24px" ImageUrl="~/images/ok_cancel.jpg"
                                                    Width="24px" />
                                            </td>
                                            <td style="width: 50px; height: 30px; text-align: left">
                                                <asp:Label ID="Label3" runat="server" Text="Close" Width="21px"></asp:Label>
                                            </td>

                                        </tr>
                                    </table>
                                </td>
                            </tr>

                        </table>
                    </td>
                </tr>
               
                <tr>
                    <td>
                        <asp:Label ID="lblCount" runat="server" Text="Student Count : " Font-Bold="True" Visible="False"></asp:Label>
                        <asp:Label ID="lblstucount" Font-Bold="True" runat="server"></asp:Label>
                        <br />
                        <asp:Label ID="lblAmount" runat="server" Style="font-weight: 700" Text="Total Batch Amount : RM " Visible="False"></asp:Label>
                        <asp:Label ID="lblbatch" runat="server" Style="font-weight: 700"></asp:Label>
                        <br />
                    </td>
                </tr>
                
               
                <tr>
                    <td style="width: 100px; height: 149px; vertical-align: top;">
                        <table style="width: 416px; height: 25px">
                            <tr>
                                <td style="width: 100px; height: 149px; vertical-align: top;">
                                    <div style="overflow: auto; width: 550px; height:200px">
                                        <asp:Panel ID="pnlStuNotAvailable" runat="server" Width="100%" Visible="false">
                                            <span>
                                                List Below Show Student N/A for Sponsorship.
                                            </span>
                                        <asp:DataGrid ID="dgStudentNotAvailable" runat="server" AutoGenerateColumns="False" Width="100%" AllowPaging="true" PageSize="25"
                                            Height="123px" Style="vertical-align: text-top" ShowFooter="false">
                                            <FooterStyle CssClass="dgFooterStyle" Height="20px" />
                                            <PagerStyle Mode="NumericPages" HorizontalAlign="Center" />
                                            <SelectedItemStyle CssClass="dgSelectedItemStyle" />
                                            <AlternatingItemStyle BackColor="Beige" CssClass="dgAlternatingItemStyle" Font-Bold="False"
                                                Font-Italic="False" Font-Overline="False" Font-Strikeout="False" Font-Underline="False" />
                                            <ItemStyle CssClass="dgItemStyle" />
                                            <HeaderStyle BackColor="#CDD7EE" CssClass="dgHeaderStyle" Font-Bold="True" Font-Italic="False"
                                                Font-Overline="False" Font-Size="Medium" Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Center" />
                                            <Columns>
                                                <asp:TemplateColumn HeaderText="Select" Visible="false">
                                                    <ItemTemplate>
                                                        &nbsp;<asp:CheckBox ID="chk" runat="server" AutoPostBack="true" Checked="true" />
                                                    </ItemTemplate>
                                                </asp:TemplateColumn>
                                                <asp:BoundColumn DataField="MatricNo" HeaderText="Matric No"></asp:BoundColumn>
                                                <asp:BoundColumn DataField="StudentName" HeaderText="Student Name"></asp:BoundColumn>
                                                <asp:BoundColumn DataField="CurrentSemester" HeaderText="Semester"></asp:BoundColumn>
                                                <asp:BoundColumn DataField="SponsorLimit" HeaderText="Sponsor Limit"></asp:BoundColumn>
                                                <asp:BoundColumn DataField="PaidAmount" HeaderText="Allocated Amount"></asp:BoundColumn>
                                                <asp:BoundColumn DataField="OutstandingAmount" HeaderText="Available Amount"></asp:BoundColumn>
                                            </Columns>
                                        </asp:DataGrid>
                                            </asp:Panel>
                                    </div>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
        </div>
    </form>
</body>
</html>
