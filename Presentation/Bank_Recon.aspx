<%@ Page Title="" Language="VB" MasterPageFile="~/MasterPage3.master" AutoEventWireup="false" CodeFile="Bank_Recon.aspx.vb" Inherits="Bank_Recon" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <asp:Panel ID="Panel1" runat="server" Width="100%">
    <table style="background-image:url(images/Sample.png);">
        <tr>
            <td style="width: 4px; height: 14px">

            </td>
            <td style="width: 14px; height: 14px">
            </td>
                <td >
                <table style="border-collapse:collapse;" class="menuoff" onmouseover="className='menuon';" onmouseout="className='menuoff';">
                <tr>
                <td ><asp:ImageButton ID="ibtnNew" runat="server" ImageUrl="images/add.png" ToolTip="New" /></td>
                <td ><asp:Label ID="Label11" runat="server" Text="New"></asp:Label></td>
                </tr>
                </table>
                </td>
                            
                <td>
                <table style="border-collapse:collapse;" class="menuoff" onmouseover="className='menuon';" onmouseout="className='menuoff';">
                <tr>
                <td><asp:ImageButton ID="ibtnSave" runat="server" ImageUrl="~/images/save.png" ToolTip="Save" /></td>
                <td><asp:Label ID="Label14" runat="server" Text="Save"></asp:Label></td>
                </tr>
                </table>
                </td>
                            
                            
                <td>
                <table style="border-collapse:collapse;" class="menuoff" onmouseover="className='menuon';" onmouseout="className='menuoff';">
                <tr>
                <td><asp:ImageButton ID="ibtnDelete" runat="server" ImageUrl="~/images/delete.png" /></td>
                    <td><asp:Label ID="Label13" runat="server" Text="Delete"></asp:Label></td>
                </tr></table>
                    </td>
                             
                <td>
                <table style="border-collapse:collapse;" class="menuoff" onmouseover="className='menuon';" onmouseout="className='menuoff';">
                <tr>
                <td><asp:ImageButton ID="ibtnView" runat="server" ImageUrl="~/images/find.png" ToolTip="View" /></td>
                <td ><asp:Label ID="Label16" runat="server" Text="Search"></asp:Label></td>
                </tr></table>
                </td>
                            
                <td>
                <table style="border-collapse:collapse;" class="menuoff" onmouseover="className='menuon';" onmouseout="className='menuoff';">
                <tr>
                <td><asp:ImageButton ID="ibtnPrint" runat="server" ImageUrl="~/images/print.png" ToolTip="Print" /></td>
                <td><asp:Label ID="Label17" runat="server" Text="Print"></asp:Label></td>
                </tr>
                </table>
                </td>
                           
                <td>
                <table  style="border-collapse:collapse;" class="menuoff" onmouseover="className='menuon';" onmouseout="className='menuoff';">
                    </table></td>
                           
                    <td>
                <table  style="border-collapse:collapse;" class="menuoff" onmouseover="className='menuon';" onmouseout="className='menuoff';">
                <tr>
                <td ><asp:ImageButton ID="ibtnOthers" runat="server" ImageUrl="~/images/gothers.gif" ToolTip="Cancel" /></td>
                <td ><asp:Label ID="Label5" runat="server" Text="Others"></asp:Label></td>
                </tr></table></td>
                           
                <td>
                <table style="border-collapse:collapse;" class="menuoff" onmouseover="className='menuon';" onmouseout="className='menuoff';">
                <tr>
                <td ><asp:ImageButton ID="ibtnCancel" runat="server" ImageUrl="~/images/cancel.png" ToolTip="Cancel" /></td>
                <td><asp:Label ID="Label18" runat="server" Text="Cancel"></asp:Label></td>
                </tr></table></td>
                    
            <td style="width: 2%; height: 14px">
                <asp:ImageButton ID="ibtnFirst" runat="server" ImageUrl="~/images/new_last.png"/></td>
                      
            <td style="width: 2%; height: 14px">
                <asp:ImageButton ID="ibtnPrevs" runat="server" ImageUrl="~/images/new_prev.png" /></td>
            <td style="width: 2%; height: 14px">
                <asp:TextBox ID="txtRecNo" runat="server" readonly="true" disabled="disabled" tabindex="1" dir="ltr" CssClass="text_box" AutoPostBack="True" MaxLength="7" Style="text-align: right"
                    Width="52px"></asp:TextBox></td>
            <td style="width: 2%; height: 14px">
                <asp:Label ID="Label47" runat="server">Of</asp:Label></td>
                    <td style="width: 2%; height: 14px"><asp:Label ID="lblCount" runat="server" Width="20px"></asp:Label></td>
                       
            <td style="width: 2%; height: 14px">
                <asp:ImageButton ID="ibtnNext" runat="server" ImageUrl="~/images/next.png"/></td>
                      
            <td style="width: 2%; height: 14px">
                <asp:ImageButton ID="ibtnLast" runat="server" ImageUrl="~/images/new_first.png"/></td>
            <td style="width: 2%; height: 14px">
            </td>
            <td style="width: 100%; height: 14px">
            </td>
            <td style="width: 100%; height: 14px">
            </td>
        </tr>
    </table>
    <table class="mainbg" style="width: 100%">
        <tr>
            <td class="vline" style="width: 98%; height: 1px">
            </td>
        </tr>
    </table>
    <table class="mainbg" style="width: 100%">
        <tr>
            <td style="width: 494px; height: 39px">
                <asp:SiteMapPath ID="SiteMapPath1" runat="server">
                </asp:SiteMapPath>
            </td>
            <td align="right" class="pagetext" style="height: 39px">
                <asp:Label ID="lblMenuName" runat="server" Text="Bank Reconciliation" Width="253px"></asp:Label></td>
        </tr>
    </table>
</asp:Panel>
    <asp:Panel ID="PnlAdd" runat="server"  Width="100%">
<div  style="border: thin solid #A6D9F4; width: 100%">
<table class="fields" style="width: 100%; height: 100%; text-align: left;">
    <tr>
        <td class="fields" style="width: 67px; height: 17px">
        </td>
        <td style="width: 43px; height: 17px">
            <asp:Label ID="lblMsg" runat="server" CssClass="lblError" Style="text-align: center"
                Width="348px"></asp:Label></td>
        <td style="width: 100px; height: 17px">
        </td>
    </tr>
    <tr>
    <td class="fields" style="width: 67px; height: 17px">
        &nbsp;
            <asp:Label ID="Label10" runat="server" Text="Bank Code" Width="97px"></asp:Label>
    <td style="width: 43px; height: 17px">
        <span
                style="font-size: 11pt; color: #ff0066">
          <asp:DropDownList ID="ddlBankCode" runat="server" AppendDataBoundItems="True" TabIndex="11"
                Width="149px">
                <asp:ListItem Value="-1">--Select--</asp:ListItem>
            </asp:DropDownList>*
        </span></td>
    <td style="width: 100px; height: 17px">
    </td>
</tr>
<tr>
    <td class="fields" style="width: 67px; height: 17px">
        &nbsp;
            <asp:Label ID="lblUpload" runat="server" Text="Upload File"></asp:Label></td>
    <td style="width: 43px; height: 17px">
        <span
                style="font-size: 11pt; color: #ff0066">
            <asp:FileUpload ID="UploadFile" runat="server" Width="300px" />*

        </span></td>
    <td style="width: 100px; height: 17px">
    </td>
</tr>
    <tr>
        <td class="fields" style="width: 67px; height: 26px" colspan="2">
             <asp:Button ID="btnUpload" runat="server" Text="Upload Recon File"></asp:Button>
        </td>
    </tr>
    
</table>
<br />
</div>
</asp:Panel>
     <asp:Panel ID="pnlMatch" runat="server" Width="100%" Visible="false">
        <fieldset style="width: 98%; padding: 2px; margin-left:5px; border: thin solid #A6D9F4">
            <legend style="margin-left: 10px">
                <asp:Label ID="lblSummary" runat="server" Text="Matching Details" Font-Bold="true"></asp:Label>
            </legend>
            <table width="100%">
                <tr>
                    <td>
                        <asp:Label ID="Label2" runat="server" Text="Total Amount" Font-Bold="true"></asp:Label>
                    </td>
                    <td>
                        <asp:Label ID="lblTotalAmount" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="Label1" runat="server" Text="Total Records" Font-Bold="true"></asp:Label>
                    </td>
                    <td>
                        <asp:Label ID="lblTotalRecords" runat="server"></asp:Label>
                    </td>
                </tr>
            </table>
        </fieldset>
    </asp:Panel>
<asp:Panel ID="GridMatch" runat="server"  Width="100%">
    <table class="fields" style="width: 100%; height: 100%; text-align: left;">
    <tr>
        <td>   <asp:DataGrid ID="dgMatchingRecords" runat="server" AutoGenerateColumns="False" Width="100%">
                    <FooterStyle CssClass="dgFooterStyle" Height="20px" />
                <SelectedItemStyle CssClass="dgSelectedItemStyle" />
                <AlternatingItemStyle BackColor="Beige" CssClass="dgAlternatingItemStyle" Font-Bold="False"
                    Font-Italic="False" Font-Overline="False" Font-Strikeout="False" Font-Underline="False" />
                <ItemStyle CssClass="dgItemStyle" />
                <HeaderStyle BackColor="#CDD7EE" CssClass="dgHeaderStyle" Font-Bold="True" Font-Italic="False"
                    Font-Overline="False" Font-Size="Medium" Font-Strikeout="False" Font-Underline="False" />
                <Columns>
                    <asp:BoundColumn DataField="RECEIPT_NO" HeaderText="Receipt No" ></asp:BoundColumn>
                    <asp:BoundColumn DataField="PAID_AMOUNT" HeaderText="Paid Amount" ></asp:BoundColumn>
                    <asp:BoundColumn DataField="RECEIPT_DATE" HeaderText="Receipt Date" ></asp:BoundColumn>
               </Columns>
            </asp:DataGrid>
        </td>
    </tr>
    </table>
</asp:Panel>
     <asp:Panel ID="pnlUnMatch" runat="server" Width="100%" Visible="false">
        <fieldset style="width: 98%; padding: 2px; margin-left:5px; border: thin solid #A6D9F4">
            <legend style="margin-left: 10px">
                <asp:Label ID="Label3" runat="server" Text="UnMatching Details" Font-Bold="true"></asp:Label>
            </legend>
            <table width="100%">
                <tr>
                    <td>
                        <asp:Label ID="Label7" runat="server" Text="Total Amount" Font-Bold="true"></asp:Label>
                    </td>
                    <td>
                        <asp:Label ID="lblUnTotalAmount" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="Label8" runat="server" Text="Total Records" Font-Bold="true"></asp:Label>
                    </td>
                    <td>
                        <asp:Label ID="lblUnTotalRecords" runat="server"></asp:Label>
                    </td>
                </tr>
            </table>
        </fieldset>
    </asp:Panel>
<asp:Panel ID="GridUnMatch" runat="server"  Width="100%">
    <table class="fields" style="width: 100%; height: 100%; text-align: left;">
    <tr>
        <td>   <asp:DataGrid ID="dgUnMatchingRecords" runat="server" AutoGenerateColumns="False" Width="100%">
                    <FooterStyle CssClass="dgFooterStyle" Height="20px" />
                <SelectedItemStyle CssClass="dgSelectedItemStyle" />
                <AlternatingItemStyle BackColor="Beige" CssClass="dgAlternatingItemStyle" Font-Bold="False"
                    Font-Italic="False" Font-Overline="False" Font-Strikeout="False" Font-Underline="False" />
                <ItemStyle CssClass="dgItemStyle" />
                <HeaderStyle BackColor="#CDD7EE" CssClass="dgHeaderStyle" Font-Bold="True" Font-Italic="False"
                    Font-Overline="False" Font-Size="Medium" Font-Strikeout="False" Font-Underline="False" />
                <Columns>
                    <asp:BoundColumn DataField="RECEIPT_NO" HeaderText="Receipt No" ></asp:BoundColumn>
                    <asp:BoundColumn DataField="PAID_AMOUNT" HeaderText="Paid Amount" ></asp:BoundColumn>
                    <asp:BoundColumn DataField="RECEIPT_DATE" HeaderText="Receipt Date" ></asp:BoundColumn>
               </Columns>
            </asp:DataGrid>
        </td>
    </tr>
    </table>
</asp:Panel>

<asp:Panel ID="Panel2" runat="server"  Width="100%">
    <table class="fields" style="width: 100%; height: 100%; text-align: left;">
    <tr>
        <td>   <asp:DataGrid ID="dgTest" runat="server" AutoGenerateColumns="False" Width="100%">
                    <FooterStyle CssClass="dgFooterStyle" Height="20px" />
                <SelectedItemStyle CssClass="dgSelectedItemStyle" />
                <AlternatingItemStyle BackColor="Beige" CssClass="dgAlternatingItemStyle" Font-Bold="False"
                    Font-Italic="False" Font-Overline="False" Font-Strikeout="False" Font-Underline="False" />
                <ItemStyle CssClass="dgItemStyle" />
                <HeaderStyle BackColor="#CDD7EE" CssClass="dgHeaderStyle" Font-Bold="True" Font-Italic="False"
                    Font-Overline="False" Font-Size="Medium" Font-Strikeout="False" Font-Underline="False" />
                
            </asp:DataGrid>
        </td>
    </tr>
    </table>
</asp:Panel>

</asp:Content>

