<%@ Page Language="VB" MasterPageFile="~/MasterPage3.master" AutoEventWireup="false" MaintainScrollPositionOnPostback="true"
    CodeFile="SponsorLedger.aspx.vb" Inherits="SponsorLedger" Title="Welcome To SAS" %>

<%@ Register Assembly="AtlasControlToolkit" Namespace="AtlasControlToolkit" TagPrefix="atlasToolkit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <script language="javascript" type="text/javascript">
        function OpenWindow(URL) {
            var WindowName = "MyPopup";
            var Features = "location=no,toolbar=no,menubar=no,height=600,scrollbars=yes";
            window.open(URL, WindowName, Features);
        }

        //added by Hafiz @ 29/2/2016
        function dllValues() {
            if (document.getElementById("<%=txtStudentCode.ClientID %>").value == "") {
                 alert("Please Choose Sponsor Code");
                 return false
             }
             else {
                 window.open('../GroupReport/ReportSponsorLedgerViewer.aspx', 'SAS', 'width=700,height=500,resizable=1');
                 return true
             }
        }
        //end

    </script>

    <asp:Panel ID="Panel1" runat="server" Width="100%">
        <table style="background-image: url(images/Sample.png);">
            <tr>
                <td style="width: 4px; height: 14px">
                </td>
                <td style="width: 14px; height: 14px">
                </td>
                <td>
                    <table style="border-collapse: collapse;" class="menuoff" onmouseover="className='menuon';"
                        onmouseout="className='menuoff';">
                        <tr>
                            <td style="height: 14px">
                                <asp:ImageButton ID="ibtnNew" runat="server" ImageUrl="~/images/add.png" ToolTip="New" />
                            </td>
                            <td style="width: 3%; height: 14px">
                                <asp:Label ID="Label11" runat="server" Text="New"></asp:Label>
                            </td>
                        </tr>
                    </table>
                </td>
                <%--Eddited by Zoya @26/02/2016--%>
                <td style="float: left;display:None; visibility:collapse;">
                    <table style="border-collapse: collapse;" class="menuoff" onmouseover="className='menuon';"
                        onmouseout="className='menuoff';">
                        <tr>
                            <td style="width: 3%; height: 14px">
                                <asp:ImageButton ID="ibtnSave" runat="server" ImageUrl="~/images/save.png" ToolTip="Save" Visible="False"/>
                            </td>
                            <td style="width: 35px; height: 14px">
                                <asp:Label ID="Label14" runat="server" Text="Save" Visible="False"></asp:Label>
                            </td>
                        </tr>
                    </table>
                </td>
                <td style="float: left;display:None; visibility:collapse;">
                    <table style="border-collapse: collapse;" class="menuoff" onmouseover="className='menuon';"
                        onmouseout="className='menuoff';">
                        <tr>
                            <td style="width: 3%; height: 14px">
                                <asp:ImageButton ID="ibtnDelete" runat="server" ImageUrl="~/images/delete.png" Visible="False"/>
                            </td>
                            <td style="width: 3%; height: 14px">
                                <asp:Label ID="Label13" runat="server" Text="Delete" Visible="False"></asp:Label>
                            </td>
                        </tr>
                    </table>
                </td>
                <td style="float: left;display:None; visibility:collapse;">
                    <table style="border-collapse: collapse;" class="menuoff" onmouseover="className='menuon';"
                        onmouseout="className='menuoff';">
                        <tr>
                            <td style="width: 3%; height: 14px">
                                <asp:ImageButton ID="ibtnView" runat="server" ImageUrl="~/images/find.png" ToolTip="View" Visible="False"/>
                            </td>
                            <td style="width: 3%; height: 14px">
                                <asp:Label ID="Label16" runat="server" Text="Search" Visible="False"></asp:Label>
                            </td>
                        </tr>
                    </table>
                </td>
                <%--Done Eddited by Zoya @26/02/2016--%>
                <td>
                    <table style="border-collapse: collapse;" class="menuoff" onmouseover="className='menuon';"
                        onmouseout="className='menuoff';">
                        <tr>
                            <td style="width: 3%; height: 14px">
                                <asp:ImageButton ID="ibtnPrint" runat="server" ImageUrl="~/images/print.png" ToolTip="Print" />
                            </td>
                            <td style="width: 3%; height: 14px">
                                <asp:Label ID="Label17" runat="server" Text="Print"></asp:Label>
                            </td>
                        </tr>
                    </table>
                </td>
                 <%-- Editted By Zoya @25/02/2016--%>
                <td style="float: left;display:None; visibility:collapse;">
                    <table style="border-collapse: collapse;" class="menuoff" onmouseover="className='menuon';"
                        onmouseout="className='menuoff';">
                        <tr>
                            <td style="width: 3%; height: 14px">
                                <asp:ImageButton ID="ibtnPosting" runat="server" ImageUrl="~/images/gposting.png"
                                    ToolTip="Cancel" Visible="false"/>
                            </td>
                            <td style="width: 3%; height: 14px">
                                &nbsp;<asp:Label ID="Label6" runat="server" Text="Posting" Visible="false"></asp:Label>
                            </td>
                        </tr>
                    </table>
                </td>
               
                <td style="float: left;display:None; visibility:collapse;">
                    <table style="border-collapse: collapse;" class="menuoff" onmouseover="className='menuon';"
                        onmouseout="className='menuoff';">
                        <tr>
                            <td style="width: 3%; height: 14px">
                                <asp:ImageButton ID="ibtnOthers" runat="server" ImageUrl="~/images/gothers.png" ToolTip="Cancel" Visible="false"/>
                            </td>
                            <td style="width: 3%; height: 14px">
                                <asp:Label ID="Label5" runat="server" Text="Others" Visible="false"></asp:Label>
                            </td>
                        </tr>
                    </table>
                </td>
                <td style="float: left">
                    <table style="border-collapse: collapse;" class="menuoff" onmouseover="className='menuon';"
                        onmouseout="className='menuoff';">
                        <tr>
                            <td style="width: 3%; height: 14px">
                                <asp:ImageButton ID="ibtnCancel" runat="server" ImageUrl="~/images/cancel.png" ToolTip="Cancel" />
                            </td>
                            <td style="width: 3%; height: 14px">
                                <asp:Label ID="Label18" runat="server" Text="Cancel"></asp:Label>
                            </td>
                        </tr>
                    </table>
                </td>
                <%-- Done Editted By Zoya @25/02/2016--%>
                <%--Editted By Zoya @2/03/2016--%>
                <td style="float: left;display:None; visibility:collapse;width: 2%; height: 14px">
                    <asp:ImageButton ID="ibtnFirst" runat="server" ImageUrl="~/images/new_last.png" Visible="false"/>
                </td>
                <td style="float: left;display:None; visibility:collapse;width: 2%; height: 14px">
                    <asp:ImageButton ID="ibtnPrevs" runat="server" ImageUrl="~/images/new_prev.png" Visible="false"/>
                </td>
                <td style="float: left;display:None; visibility:collapse;width: 2%; height: 14px">
                    <asp:TextBox ID="txtRecNo" runat="server" AutoPostBack="True" MaxLength="5" Style="text-align: right"
                        Width="52px" ReadOnly="true" CssClass="text_box" disabled="disabled" TabIndex="1"
                        dir="ltr" Visible="false"></asp:TextBox>
                </td>
                <td style="float: left;display:None; visibility:collapse;width: 2%; height: 14px">
                    <asp:Label ID="Label47" runat="server" Visible="false">Of</asp:Label>
                </td>
                <td style="float: left;display:None; visibility:collapse;width: 2%; height: 14px">
                    <asp:Label ID="lblCount" runat="server" Width="20px" Visible="false"></asp:Label>
                </td>
                <td style="float: left;display:None; visibility:collapse;width: 2%; height: 14px">
                    <asp:ImageButton ID="ibtnNext" runat="server" ImageUrl="~/images/next.png" Visible="false"/>
                </td>
                <td style="float: left;display:None; visibility:collapse;width: 2%; height: 14px">
                    <asp:ImageButton ID="ibtnLast" runat="server" ImageUrl="~/images/new_first.png" Visible="false"/>
                </td>
                <%--Done Editted By Zoya @2/03/2016--%>
                <td style="width: 2%; height: 14px">
                </td>
                <td style="width: 100%; height: 14px">
                </td>
                <td style="width: 100%; height: 14px">
                </td>
            </tr>
        </table>
        <table style="width: 100%">
            <tr>
                <td class="vline" style="width: 100%; height: 1px">
                </td>
            </tr>
        </table>
        <table style="width: 100%">
            <tr>
                <td style="width: 400px; height:39px;">
                    <asp:SiteMapPath ID="SiteMapPath1" runat="server">
                    </asp:SiteMapPath>
                </td>
                <td class="pagetext" style="text-align: right">
                    <asp:Label ID="lblMenuName" runat="server"></asp:Label>
                </td>
            </tr>
        </table>
        <asp:Label ID="lblMsg" runat="server" CssClass="lblError" Style="text-align: center"
            Width="359px"></asp:Label></asp:Panel>

    <table style="width: 100%">
        <tr>
            <td style="width: 100%">
              <div  style="border: thin solid #A6D9F4; width: 100%">
                    <table width="100%">
                        <tr>
                            <td style="width: 13%">
                                <asp:Label ID="Label1" runat="server" Text="Sponsor Code" Width="97px"></asp:Label>
                            </td>
                            <td style="width: 14%">
                                <asp:TextBox ID="txtStudentCode" runat="server" MaxLength="20" Width="142px" ReadOnly="True"></asp:TextBox>
                            </td>
                            <td style="width: 101px; text-align: left">
                                <asp:Image ID="ibtnSpn1" runat="server" Height="16px" ImageUrl="~/images/find_img.png"
                                    Width="16px" />
                            </td>
                            <td style="width: 136px; text-align: left">
                            </td>
                            <td style="width: 41px">
                            </td>
                            <td style="width: 378px">
                                &nbsp;
                            </td>
                            <td style="width: 378px">
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 13%; height: 25px;">
                                <asp:Label ID="Label3" runat="server" Text="Sponsor Name" Width="97px"></asp:Label>
                            </td>
                            <td style="width: 14%; height: 25px;">
                                <asp:TextBox ID="txtStuName" runat="server" MaxLength="20" Width="400px" ReadOnly="True"></asp:TextBox>
                            </td>
                            <td style="width: 101px; text-align: right; height: 25px;">
                            </td>
                            <td style="width: 136px; text-align: right; height: 25px;">
                            </td>
                            <td style="width: 41px; height: 25px;">
                                &nbsp;
                            </td>
                            <td style="width: 378px; height: 25px;">
                            </td>
                            <td style="width: 378px; height: 25px;">
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 13%; height: 25px;">
                                <asp:Label ID="Label9" runat="server" Text="Date" Width="37px" Visible="False"></asp:Label>
                            </td>
                            <td colspan="2" style="height: 25px">
                                <asp:TextBox ID="txtDate" runat="server" MaxLength="20" Width="142px" ReadOnly="True"
                                    Visible="False"></asp:TextBox>
                            </td>
                            <td colspan="1" style="height: 25px; text-align: right; width: 136px;">
                            </td>
                            <td style="width: 41px; height: 25px;">
                            </td>
                            <td style="width: 378px; height: 25px;">
                            </td>
                            <td style="width: 378px; height: 25px;">
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 13%; height: 25px;">
                                <asp:Label ID="Label12" runat="server" Text="Status" Width="97px" Visible="False"></asp:Label>
                            </td>
                            <td style="width: 14%; height: 25px;">
                                <asp:TextBox ID="TextBox8" runat="server" MaxLength="20" Width="142px" Visible="False"></asp:TextBox>
                            </td>
                            <td style="width: 101px; text-align: right; height: 25px;">
                            </td>
                            <td style="width: 136px; height: 25px; text-align: right">
                            </td>
                            <td style="width: 41px; height: 25px;">
                            </td>
                            <td style="width: 378px; height: 25px;">
                            </td>
                            <td style="width: 378px; height: 25px;">
                            </td>
                        </tr>
                    </table>
               </div>
            </td>
        </tr>
    </table>
    <table width="100%">
        <tr>
            <td style="height: 16px" width="1%">
            </td>
            <td style="width: 72%; height: 16px">
                <asp:Label ID="Label2" runat="server" Text="Sponsor Ledger" Width="97px" Visible="False"></asp:Label>
            </td>
            <td style="height: 16px" width="30%">
            </td>
            <td style="height: 16px" width="30%">
            </td>
            <td style="height: 16px" width="1%">
            </td>
        </tr>
        <tr>
            <td style="height: 16px" width="1%">
            </td>
            <td colspan="3" style="height: 16px">
                <asp:DataGrid ID="dgInvoices" runat="server" AutoGenerateColumns="False" DataKeyField="TransactionCode"
                    Width="100%">
                    <FooterStyle CssClass="dgFooterStyle" Height="20px" />
                    <SelectedItemStyle CssClass="dgSelectedItemStyle" />
                    <AlternatingItemStyle BackColor="Beige" CssClass="dgAlternatingItemStyle" Font-Bold="False"
                        Font-Italic="False" Font-Overline="False" Font-Strikeout="False" Font-Underline="False" />
                    <ItemStyle CssClass="dgItemStyle" />
                    <HeaderStyle BackColor="#CDD7EE" CssClass="dgHeaderStyle" Font-Bold="True" Font-Italic="False"
                        Font-Overline="False" Font-Size="Medium" Font-Strikeout="False" Font-Underline="False" />
                    <Columns>
                        <asp:BoundColumn DataField="TransDate" DataFormatString="{0:dd/MM/yyyy}" HeaderText="Date">
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField="TransactionCode" HeaderText="Document No"></asp:BoundColumn>
                        <asp:BoundColumn DataField="Description" HeaderText="Description">
                            <HeaderStyle Width="40%" />
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField="TransactionAmount" HeaderText="Transaction Amount">
                            <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                Font-Underline="False" HorizontalAlign="Right" />
                        </asp:BoundColumn>
                        <asp:BoundColumn HeaderText="Statement Balance">
                            <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                Font-Underline="False" HorizontalAlign="Right" />
                        </asp:BoundColumn>
                        <asp:TemplateColumn HeaderText="View">
                            <ItemTemplate>
                                <asp:HyperLink ID="btnview" runat="server">View</asp:HyperLink>
                            </ItemTemplate>
                        </asp:TemplateColumn>
                        <asp:BoundColumn DataField="Category" HeaderText="Category" Visible="False"></asp:BoundColumn>
                        <asp:BoundColumn DataField="BatchCode" HeaderText="BatchCode" Visible="False"></asp:BoundColumn>
                        <asp:BoundColumn DataField="TransType" HeaderText="TransType" Visible="False"></asp:BoundColumn>
                    </Columns>
                </asp:DataGrid>
            </td>
            <td style="height: 16px" width="1%">
            </td>
        </tr>
        <tr>
            <td style="height: 16px" width="1%">
            </td>
            <td style="width: 72%; height: 16px; text-align: right">
                &nbsp; &nbsp;&nbsp;
            </td>
            <td style="height: 16px; text-align: right" width="30%">
                <asp:Label ID="lblDebit" runat="server" Text="Total Debit Amount" Visible="False"></asp:Label>
            </td>
            <td style="height: 16px; text-align: right" width="30%">
                <asp:TextBox ID="txtDebitAmt" runat="server" Style="text-align: right" Width="101px"
                    Visible="False" Font-Bold="True" ReadOnly="True"></asp:TextBox>
            </td>
            <td style="height: 16px" width="1%">
            </td>
        </tr>
        <tr>
            <td style="height: 16px" width="1%">
            </td>
            <td style="width: 72%; height: 16px; text-align: right">
                <asp:TextBox ID="txtPg" runat="server" MaxLength="20" Width="240px" Visible="False"></asp:TextBox>
            </td>
            <td style="height: 16px; text-align: right" width="30%">
                <asp:Label ID="lblCredit" runat="server" Text="Total Credit Amount" Visible="False"></asp:Label>
            </td>
            <td style="height: 16px; text-align: right" width="30%">
                <asp:TextBox ID="txtCreditAmt" runat="server" Style="text-align: right" Width="102px"
                    Visible="False" Font-Bold="True" ReadOnly="True"></asp:TextBox>
            </td>
            <td style="height: 16px" width="1%">
            </td>
        </tr>
        <tr>
            <td style="height: 16px" width="1%">
            </td>
            <td style="width: 72%; height: 16px; text-align: right">
            </td>
            <td style="height: 16px; text-align: right" width="30%">
                <asp:Label ID="lblOut" runat="server" Text="Outstanding Amount"></asp:Label>
            </td>
            <td style="height: 16px; text-align: right" width="30%">
                <asp:TextBox ID="txtOutAmt" runat="server" Font-Bold="True" Style="text-align: right"
                    Width="102px" ReadOnly="True"></asp:TextBox>
            </td>
            <td style="height: 16px" width="1%">
            </td>
        </tr>
    </table>
    <br />
     <asp:Button ID="btnHidden" runat="Server" style="display:none" />

</asp:Content>
