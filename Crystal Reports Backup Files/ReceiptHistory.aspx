<%@ Page Title="" Language="VB" MasterPageFile="~/MasterPage3.master" AutoEventWireup="false" MaintainScrollPositionOnPostback="true"
    CodeFile="ReceiptHistory.aspx.vb" Inherits="ReceiptHistory" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <script language="javascript" type="text/javascript">

        function getibtnFDate() {
            popUpCalendar(document.getElementById("<%=ibtnFdate.ClientID%>"), document.getElementById("<%=txtFrom.ClientID%>"), 'dd/mm/yyyy')
        }
        function getDateto() {
            popUpCalendar(document.getElementById("<%=ibtnTdate.ClientID%>"), document.getElementById("<%=txtTo.ClientID%>"), 'dd/mm/yyyy')
        }

        function geterr() {
            var digits = "0123456789";
            var temp;
            for (var i = 0; i < document.getElementById("<%=txtRecNo.ClientID %>").value.length; i++) {
                temp = document.getElementById("<%=txtRecNo.ClientID%>").value.substring(i, i + 1);
                if (digits.indexOf(temp) == -1) {
                    alert("Enter Valid Record No");
                    document.getElementById("<%=txtRecNo.ClientID%>").value = 1;
                    document.getElementById("<%=txtRecNo.ClientID%>").focus();
                    return false;
                }
            }
            return true;
        }
     
    </script>
    <asp:Panel ID="pnlToolbar" runat="server" Width="100%">
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
                            <td>
                                <asp:ImageButton ID="ibtnNew" runat="server" Enabled="false" ImageUrl="~/images/gadd.png" ToolTip="New" />
                            </td>
                            <td>
                                <asp:Label ID="Label11" runat="server" Text="New"></asp:Label>
                            </td>
                        </tr>
                    </table>
                </td>
                <td>
                    <table style="border-collapse: collapse;" class="menuoff" onmouseover="className='menuon';"
                        onmouseout="className='menuoff';">
                        <tr>
                            <td>
                                <asp:ImageButton ID="ibtnSave" runat="server" ImageUrl="~/images/gsave.png" 
                                    ToolTip="Save" Enabled="False" />
                            </td>
                            <td>
                                <asp:Label ID="Label14" runat="server" Text="Save"></asp:Label>
                            </td>
                        </tr>
                    </table>
                </td>
                <td>
                    <table style="border-collapse: collapse;" class="menuoff" onmouseover="className='menuon';"
                        onmouseout="className='menuoff';">
                        <tr>
                            <td>
                                <asp:ImageButton ID="ibtnDelete" runat="server" ImageUrl="~/images/gdelete.png" 
                                    ToolTip="Delete" Enabled="False" />
                            </td>
                            <td>
                                <asp:Label ID="Label13" runat="server" Text="Delete"></asp:Label>
                            </td>
                        </tr>
                    </table>
                </td>
                <td>
                    <table style="border-collapse: collapse;" class="menuoff" onmouseover="className='menuon';"
                        onmouseout="className='menuoff';">
                        <tr>
                            <td>
                                <asp:ImageButton ID="ibtnView" runat="server" ImageUrl="~/images/gfind.png" 
                                    ToolTip="View" Enabled="False" />
                            </td>
                            <td>
                                <asp:Label ID="Label16" runat="server" Text="Search"></asp:Label>
                            </td>
                        </tr>
                    </table>
                </td>
                <td>
                    <table style="border-collapse: collapse;" class="menuoff" onmouseover="className='menuon';"
                        onmouseout="className='menuoff';">
                        <tr id="trPrint" runat="server">
                            <td>
                                <asp:ImageButton ID="ibtnPrint" runat="server" ImageUrl="~/images/gprint.png" 
                                    ToolTip="Print" Enabled="False" />
                            </td>
                            <td>
                                <asp:Label ID="lblPrint" runat="server" Text="Print"></asp:Label>
                            </td>
                        </tr>
                    </table>
                </td>
                <td>
                    <table style="border-collapse: collapse;" class="menuoff" onmouseover="className='menuon';"
                        onmouseout="className='menuoff';">
                        <tr>
                            <td>
                                <%--OnClick="ibtnPosting_Click"--%>
                                <asp:ImageButton ID="ibtnPosting" runat="server" ImageUrl="~/images/gposting.png"
                                    ToolTip="Cancel" />
                            </td>
                            <td>
                                <asp:Label ID="Label6" runat="server" Text="Posting"></asp:Label>
                            </td>
                        </tr>
                    </table>
                </td>
                <td>
                    <table style="border-collapse: collapse;" class="menuoff" onmouseover="className='menuon';"
                        onmouseout="className='menuoff';">
                        <tr>
                            <td>
                                <%--  OnClick="ibtnOthers_Click"--%>
                                <asp:ImageButton ID="ImageButton1" runat="server" ImageUrl="~/images/gothers.png"
                                    ToolTip="Cancel" />
                            </td>
                            <td>
                                <asp:Label ID="Label5" runat="server" Text="Others"></asp:Label>
                            </td>
                        </tr>
                    </table>
                </td>
                <td>
                    <table style="border-collapse: collapse;" class="menuoff" onmouseover="className='menuon';"
                        onmouseout="className='menuoff';">
                        <tr>
                            <td>
                                <asp:ImageButton ID="ibtnCancel" runat="server" ImageUrl="~/images/cancel.png" ToolTip="Cancel"
                                    Style="width: 19px" />
                            </td>
                            <td>
                                <asp:Label ID="Label18" runat="server" Text="Cancel"></asp:Label>
                            </td>
                        </tr>
                    </table>
                </td>
                <td style="width: 2%; height: 14px">
                    <asp:ImageButton ID="ibtnFirst" runat="server" 
                        ImageUrl="~/images/gnew_first.png" ToolTip="First" />
                </td>
                <td style="width: 2%; height: 14px">
                    <asp:ImageButton ID="ibtnPrevs" runat="server" 
                        ImageUrl="~/images/gnew_prev.png" ToolTip="Previous" />
                </td>
                <td style="width: 2%; height: 14px">
                    <%-- OnTextChanged="txtRecNo_TextChanged"--%>
                    <asp:TextBox ID="txtRecNo" runat="server" AutoPostBack="True" Width="50px" MaxLength="7"
                        ReadOnly="true" CssClass="text_box" disabled="disabled" TabIndex="1" dir="ltr"></asp:TextBox>
                </td>
                <td style="width: 2%; height: 14px">
                    <asp:Label ID="Label47" runat="server">Of</asp:Label>
                </td>
                <td style="width: 2%; height: 14px">
                    <asp:Label ID="lblCount" runat="server" Width="20px"></asp:Label>
                </td>
                <td style="width: 2%; height: 14px">
                    <asp:ImageButton ID="ibtnNext" runat="server" ImageUrl="~/images/gnew_next.png" 
                        ToolTip="Next" />
                </td>
                <td style="width: 2%; height: 14px">
                    <asp:ImageButton ID="ibtnLast" runat="server" ImageUrl="~/images/gnew_last.png" 
                        ToolTip="Last" />
                </td>
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
                <td class="vline" style="width: 746px; height: 1px">
                </td>
            </tr>
        </table>
        <table style="width: 100%">
            <tr>
                <td width="50%">
                    <asp:SiteMapPath ID="SiteMapPath1" runat="server">
                    </asp:SiteMapPath>
                </td>
                <td width="50%" align="right" class="pagetext" >
                    <asp:Label ID="lblMenuName" runat="server" Width="450px" Text="Receipt History"></asp:Label>
                </td>
            </tr>
        </table>
    </asp:Panel>
    <br />
    <asp:Panel ID="pnlBody" runat="server">
        <fieldset style="width: 100%; border: thin solid #A6D9F4;">
            <legend><strong><span style="color: #000000;">Selection Criteria</span></strong></legend>
            <br />
            <table width="100%">
                <tr>
                    <td colspan="5">
                        <asp:Label ID="lblMesg" runat="server" CssClass="lblError1" ForeColor="Red" Visible="False"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td style="width: 85px">
                        <asp:Label ID="lblProgram" runat="server" Text="Date From"></asp:Label>
                    </td>
                    <td style="width: 209px" valign="top">
                        <asp:TextBox ID="txtFrom" runat="server"></asp:TextBox>
                        <asp:Image ID="ibtnFDate" runat="server" ImageUrl="~/images/cal.gif" />
                    </td>
                    <td style="width: 77px" >
                        <asp:Label ID="Label1" runat="server" Text="Date To"></asp:Label>
                    </td>
                    <td style="width: 170px" valign="top">
                        <asp:TextBox ID="txtTo" runat="server"></asp:TextBox>
                        <asp:Image ID="ibtnTDate" runat="server" ImageUrl="~/images/cal.gif" />
                    </td>
                    <td valign="top">
                        <asp:Button ID="btnFind" runat="server" Text="Find" Width="57px" />
                    </td>
                </tr>
            </table>
            <br />
        </fieldset>
        <br />
        <br />
        <br />
        <asp:DataGrid ID="dgStudent" runat="server" AutoGenerateColumns="False" Width="100%">
            <FooterStyle CssClass="dgFooterStyle" Height="20px" />
            <SelectedItemStyle CssClass="dgSelectedItemStyle" />
            <AlternatingItemStyle BackColor="Beige" CssClass="dgAlternatingItemStyle" Font-Bold="False"
                Font-Italic="False" Font-Overline="False" Font-Strikeout="False" Font-Underline="False" />
            <ItemStyle CssClass="dgItemStyle" />
            <HeaderStyle BackColor="#CDD7EE" CssClass="dgHeaderStyle" Font-Bold="True" Font-Italic="False"
                Font-Overline="False" Font-Size="Medium" Font-Strikeout="False" Font-Underline="False" />
            <Columns>
                <asp:BoundColumn DataField="UpdatedTime" HeaderText="Date" DataFormatString="{0:dd/MM/yyyy hh:mm tt}" ></asp:BoundColumn>
                <asp:BoundColumn DataField="BatchCode" HeaderText="Receipt No"></asp:BoundColumn>
                <asp:BoundColumn DataField="UpdatedBy"  HeaderText="Performed By"></asp:BoundColumn>
                <asp:BoundColumn DataField="TransactionCode"  HeaderText="Action"></asp:BoundColumn>
            </Columns>
        </asp:DataGrid>
        <br />
        <br />
    </asp:Panel>
</asp:Content>
