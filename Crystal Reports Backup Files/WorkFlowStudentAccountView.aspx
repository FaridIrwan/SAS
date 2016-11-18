<%@ Page Title="" Language="VB" MasterPageFile="~/MasterPage3.master" AutoEventWireup="false" CodeFile="WorkFlowStudentAccountView.aspx.vb" Inherits="WorkFlowStudentAccountView" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <table style="width: 100%">
        <tr>
            <td align="center">
                <asp:Label ID="lblMsg" runat="server" CssClass="lblError" Style="text-align: center"
                    Width="348px"></asp:Label>
            </td>
            <td></td>
        </tr>
        <tr>
            <td style="color:#00699b;">
                <br />
                <fieldset>
                    <legend><strong><span style="color: #000000"></span></strong></legend>
                    <asp:Label ID="lblCount" runat="server" Text="Student Count : " Font-Bold="True" Visible="False"></asp:Label>
                    <asp:Label ID="lblstucount" Font-Bold="True" runat="server"></asp:Label>
                    <br />
                    <br />
                    <asp:Label ID="lblAmount" runat="server" Style="font-weight: 700" Text="Total Batch Amount : RM " Visible="False"></asp:Label>
                    <asp:Label ID="lblbatch" runat="server" Style="font-weight: 700"></asp:Label>
                    <br />
                    &nbsp;
                </fieldset>
            </td>
            <td>&nbsp;</td>
        </tr>
        <tr>
            <td style="width: 71%; height: 149px; vertical-align: top;">
                <fieldset>
                    <legend><strong><span style="color: #000000"></span></strong></legend>
                    <asp:DataGrid ID="dgWorkFlowStudentAccount" runat="server" AutoGenerateColumns="False" Width="100%" AllowPaging="true" PageSize="25"
                        Height="123px" Style="vertical-align: text-top">
                        <FooterStyle CssClass="dgFooterStyle" Height="20px" />
                        <PagerStyle Mode="NumericPages" HorizontalAlign="Center" />
                        <SelectedItemStyle CssClass="dgSelectedItemStyle" />
                        <AlternatingItemStyle BackColor="Beige" CssClass="dgAlternatingItemStyle" Font-Bold="False"
                            Font-Italic="False" Font-Overline="False" Font-Strikeout="False" Font-Underline="False" />
                        <ItemStyle CssClass="dgItemStyle" />
                        <HeaderStyle BackColor="#CDD7EE" CssClass="dgHeaderStyle" Font-Bold="True" Font-Italic="False"
                            Font-Overline="False" Font-Size="Medium" Font-Strikeout="False" Font-Underline="False" />
                        <Columns>
                            <asp:BoundColumn DataField="SASI_MatricNo" HeaderText="Matric No"></asp:BoundColumn>
                            <asp:BoundColumn DataField="SASI_Name" HeaderText="Student Name"></asp:BoundColumn>
                            <asp:BoundColumn DataField="transamount" HeaderText="Fee Amount" DataFormatString="{0:F}"></asp:BoundColumn>
                        </Columns>
                    </asp:DataGrid>
                </fieldset>
            </td>
            <td style="width: 104%; height: 149px; vertical-align: top;">&nbsp;</td>
        </tr>

    </table>
</asp:Content>

