<%@ Page Language="VB" Title="" MasterPageFile="~/MasterPage3.master" AutoEventWireup="false" CodeFile="AFCDetails.aspx.vb" Inherits="AFCDetails" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <table style="width: 100%">
        <tr>
            <td align="center">
                <asp:Label ID="lblMsg" runat="server" CssClass="lblError" Style="text-align: center"
                    Width="348px"></asp:Label>
            </td>
            <td>
                <br />
            </td>
        </tr>
        <tr>
            <td class="pagetext" style="text-align: left">
                <asp:Label ID="lbl1" runat="server" Text="AFC Fee Details"></asp:Label>
                </td>
            <td>
                <br />
            </td>
        </tr>        
        <tr>
            <td style="width: 71%; height: 149px; vertical-align: top;">
                <fieldset>
                    <asp:DataGrid ID="dgFeeType" runat="server" AutoGenerateColumns="False" Width="100%" AllowPaging="true" PageSize="25"
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
                            <asp:BoundColumn DataField="SAFT_Desc" HeaderText="Fee Name"></asp:BoundColumn>
                            <asp:BoundColumn DataField="SAFT_Code" HeaderText="Fee Code"></asp:BoundColumn>
                            <asp:BoundColumn DataField="SAFD_Type" HeaderText="Fee Type"></asp:BoundColumn>   
                            <asp:BoundColumn DataField="SAFA_Amount" HeaderText="Fee Amount" DataFormatString="{0:F}"></asp:BoundColumn>
                            <asp:BoundColumn DataField="SAST_Code" HeaderText="Semester Code" Visible="false"></asp:BoundColumn>
                            <asp:BoundColumn DataField="SAFD_Sem" HeaderText="Semester" Visible="false"></asp:BoundColumn>                            
                        </Columns>
                    </asp:DataGrid>
                </fieldset>
            </td>
            <td style="width: 104%; height: 149px; vertical-align: top;">&nbsp;</td>
        </tr>
    </table>
</asp:Content>
