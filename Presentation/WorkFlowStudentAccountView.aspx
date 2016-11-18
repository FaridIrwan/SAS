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
                <fieldset id="fsDetails" runat="server">
                    <%--<legend><strong><span style="color: #000000"></span></strong></legend>--%>
                    <asp:DataGrid ID="dgProgram" runat="server" AutoGenerateColumns="False" Width="100%" AllowPaging="false" PageSize="25"
                        Height="123px" Style="vertical-align: text-top" Visible="false">
                        <FooterStyle CssClass="dgFooterStyle" Height="20px" />
                        <PagerStyle Mode="NumericPages" HorizontalAlign="Center" />
                        <SelectedItemStyle CssClass="dgSelectedItemStyle" />
                        <AlternatingItemStyle BackColor="Beige" CssClass="dgAlternatingItemStyle" Font-Bold="False"
                            Font-Italic="False" Font-Overline="False" Font-Strikeout="False" Font-Underline="False" />
                        <ItemStyle CssClass="dgItemStyle" />
                        <HeaderStyle BackColor="#CDD7EE" CssClass="dgHeaderStyle" Font-Bold="True" Font-Italic="False"
                            Font-Overline="False" Font-Size="Medium" Font-Strikeout="False" Font-Underline="False" />
                        <Columns>
                            <asp:BoundColumn DataField="bidang" HeaderText="Bidang"></asp:BoundColumn>
                            <asp:BoundColumn DataField="program" HeaderText="Program"></asp:BoundColumn>   
                            <asp:BoundColumn DataField="semintake" HeaderText="Semester Intake"></asp:BoundColumn>  
                            <asp:BoundColumn DataField="amount" HeaderText="Amount" DataFormatString="{0:F}"></asp:BoundColumn>                            
                        </Columns>
                    </asp:DataGrid>
                    <br />                    
                    <asp:Label ID="lblCount" runat="server" Text="Student Count : " Font-Bold="True" Visible="False"></asp:Label>
                    <asp:Label ID="lblstucount" Font-Bold="True" runat="server"></asp:Label>
                    <br />
                    <br />
                    <asp:Label ID="lblAmount" runat="server" Style="font-weight: 700" Text="Total Batch Amount : RM " Visible="False"></asp:Label>
                    <asp:Label ID="lblbatch" runat="server" Style="font-weight: 700"></asp:Label>
                    <br />
                    <br />                    
                    <asp:Label ID="lblSponsorCode" runat="server" Text="Sponsor Code : " Font-Bold="True" Visible="False"></asp:Label>
                    <asp:Label ID="lblCode" Font-Bold="True" runat="server"></asp:Label>
                    <br />
                    <br />
                    <asp:Label ID="lblSponsorName" runat="server" Style="font-weight: 700" Text="Sponsor Name : " Visible="False"></asp:Label>
                    <asp:Label ID="lblName" runat="server" Style="font-weight: 700"></asp:Label>
                    <br />
                    <br />
                    <asp:Label ID="lblSpnAmt" runat="server" Style="font-weight: 700" Text="Total Sponsor Amount : RM " Visible="False"></asp:Label>
                    <asp:Label ID="lblAmt" runat="server" Style="font-weight: 700"></asp:Label>
                    <br />
                    &nbsp;
                </fieldset>
            </td>
            <td>&nbsp;</td>
        </tr>
        <tr>
            <td id="tblStudAcc" runat="server" style="width: 71%; height: 149px; vertical-align: top;">
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
                            <asp:BoundColumn DataField="FeeCode" HeaderText="Fee Code" Visible="false"></asp:BoundColumn>                               
                            <asp:BoundColumn DataField="FeeDesc" HeaderText="Fee Description" Visible="false"></asp:BoundColumn>
                            <asp:BoundColumn DataField="SASI_MatricNo" HeaderText="Matric No"></asp:BoundColumn>
                            <asp:BoundColumn DataField="SASI_Name" HeaderText="Student Name"></asp:BoundColumn>   
                            <asp:BoundColumn DataField="SASR_Code" HeaderText="Sponsor Code" Visible="false"></asp:BoundColumn>
                            <asp:BoundColumn DataField="SASR_Name" HeaderText="Sponsor Name" Visible="false"></asp:BoundColumn>
                            <asp:BoundColumn DataField="sasi_cursem" HeaderText="Semester" Visible="false"></asp:BoundColumn>                           
                            <asp:BoundColumn DataField="transamount" HeaderText="Fee Amount" DataFormatString="{0:F}"></asp:BoundColumn>
                        </Columns>
                    </asp:DataGrid>
                </fieldset>
            </td>
            <td style="width: 104%; height: 149px; vertical-align: top;">&nbsp;</td>
        </tr>

    </table>
</asp:Content>

