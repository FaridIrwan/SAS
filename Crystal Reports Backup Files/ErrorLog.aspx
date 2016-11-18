<%@ Page Language="VB" MasterPageFile="~/MasterPage3.master" AutoEventWireup="false" MaintainScrollPositionOnPostback="true"
    CodeFile="ErrorLog.aspx.vb" Inherits="ErrorLog" Title="Untitled Page" %> 

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <script language="javascript" type="Scripts/popcalendar.js"></script>
    <script language="javascript" type="Scripts/functions.js"></script>
    <script language="javascript" type="text/javascript">
        function getDate() {
            popUpCalendar(document.getElementById("<%=ibtnDate.ClientID%>"), document.getElementById("<%=txtDate.ClientID%>"), 'dd/mm/yyyy')

        }
        function CheckDate() {

            var digits = "0123456789/";
            var temp;
            for (var i = 0; i < document.getElementById("<%=txtDate.ClientID %>").value.length; i++) {
                temp = document.getElementById("<%=txtDate.ClientID%>").value.substring(i, i + 1);
                if (digits.indexOf(temp) == -1) {
                    alert("Enter Valid Date (dd/mm/yyyy)");
                    document.getElementById("<%=txtDate.ClientID%>").value = "";
                    document.getElementById("<%=txtDate.ClientID%>").focus();
                    return false;
                }
            }
            return true;
        }



    </script>
    <%-- <atlas:ScriptManager ID="scriptmanager1" runat="Server" EnablePartialRendering="true">
    </atlas:ScriptManager>
    <atlas:UpdatePanel ID="up1" runat="server">
        <ContentTemplate>--%>
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
                            <td>
                                <asp:ImageButton ID="ibtnNew" runat="server" ImageUrl="images/add.png" ToolTip="New" />
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
                                <asp:ImageButton ID="ibtnSave" runat="server" ImageUrl="~/images/save.png" ToolTip="Save" />
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
                                <asp:ImageButton ID="ibtnDelete" runat="server" ImageUrl="~/images/delete.png" />
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
                                <asp:ImageButton ID="ibtnView" runat="server" ImageUrl="~/images/find.png" ToolTip="View" />
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
                        <tr>
                            <td>
                                <asp:ImageButton ID="ibtnPrint" runat="server" ImageUrl="~/images/print.png" ToolTip="Print" />
                            </td>
                            <td>
                                <asp:Label ID="Label17" runat="server" Text="Print"></asp:Label>
                            </td>
                        </tr>
                    </table>
                </td>
                <td>
                    <table style="border-collapse: collapse;" class="menuoff" onmouseover="className='menuon';"
                        onmouseout="className='menuoff';">
                        <tr>
                            <td>
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
                                <asp:ImageButton ID="ibtnOthers" runat="server" ImageUrl="~/images/gothers.gif" ToolTip="Cancel" />
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
                                <asp:ImageButton ID="ibtnCancel" runat="server" ImageUrl="~/images/cancel.png" ToolTip="Cancel" />
                            </td>
                            <td>
                                <asp:Label ID="Label18" runat="server" Text="Cancel"></asp:Label>
                            </td>
                        </tr>
                    </table>
                </td>
                <td style="width: 2%; height: 14px">
                    <asp:ImageButton ID="ibtnFirst" runat="server" ImageUrl="~/images/new_last.png" />
                </td>
                <td style="width: 2%; height: 14px">
                    <asp:ImageButton ID="ibtnPrevs" runat="server" ImageUrl="~/images/new_prev.png" />
                </td>
                <td style="width: 2%; height: 14px">
                    <asp:TextBox ID="txtRecNo" runat="server" ReadOnly="true" disabled="disabled" TabIndex="1"
                        dir="ltr" CssClass="text_box" AutoPostBack="True" MaxLength="5" Style="text-align: right"
                        Width="52px"></asp:TextBox>
                </td>
                <td style="width: 2%; height: 14px">
                    <asp:Label ID="Label47" runat="server">Of</asp:Label>
                </td>
                <td style="width: 2%; height: 14px">
                    <asp:Label ID="lblCount" runat="server" Width="20px"></asp:Label>
                </td>
                <td style="width: 2%; height: 14px">
                    <asp:ImageButton ID="ibtnNext" runat="server" ImageUrl="~/images/next.png" />
                </td>
                <td style="width: 2%; height: 14px">
                    <asp:ImageButton ID="ibtnLast" runat="server" ImageUrl="~/images/new_first.png" />
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
                <td class="vline" style="width: 100%; height: 1px">
                </td>
            </tr>
        </table>
        <table style="width: 100%">
            <tr>
                <td style="width: 400px">
                    <asp:SiteMapPath ID="SiteMapPath1" runat="server">
                    </asp:SiteMapPath>
                </td>
                <td class="pagetext" style="text-align: right">
                    <asp:Label ID="lblMenuName" runat="server" Width="273px"></asp:Label>
                </td>
            </tr>
        </table>
        <asp:Label ID="lblMsg" runat="server" CssClass="lblError" Style="text-align: center"
            Width="359px"></asp:Label></asp:Panel>
    <%--  </ContentTemplate>
    </atlas:UpdatePanel>
    <atlas:UpdateProgress ID="ProgressIndicator" runat="server">
        <ProgressTemplate>
            Loading the data, please wait...
            <asp:Image ID="LoadingImage" runat="server" ImageAlign="AbsMiddle" ImageUrl="~/Images/spinner.gif" />
        </ProgressTemplate>
    </atlas:UpdateProgress>
    <atlas:UpdatePanel ID="up2" runat="server">
        <ContentTemplate>--%>
    <div style="border: thin solid #A6D9F4; width: 100%">
        <table>
            <tr>
                <td style="width: 100px; height: 25px">
                    <asp:Label ID="Label9" runat="server" Style="text-align: left" Text="ErrorLog Date"
                        Width="79px"></asp:Label>
                </td>
                <td style="width: 100px; height: 25px">
                    <asp:TextBox ID="txtDate" runat="server" MaxLength="10" Width="116px"></asp:TextBox>
                </td>
                <td style="width: 100px; height: 25px">
                    <asp:Image ID="ibtnDate" runat="server" ImageUrl="~/images/cal.gif" />
                </td>
                <td style="width: 1443px; height: 25px; text-align: left">
                    <asp:Button ID="Search" runat="server" Text="Search" OnClick="Search_Click" />
                </td>
            </tr>
            <tr>
                <td style="width: 100px; height: 25px">
                </td>
                <td style="width: 100px; height: 25px">
                </td>
                <td style="width: 100px; height: 25px">
                </td>
                <td style="width: 1443px; height: 25px; text-align: left">
                </td>
            </tr>
        </table>
        <table>
            <tr>
                <td colspan="3">
                    <asp:DataGrid ID="dgInvoices" runat="server" AutoGenerateColumns="False" Width="100%">
                        <FooterStyle CssClass="dgFooterStyle" Height="20px" />
                        <SelectedItemStyle CssClass="dgSelectedItemStyle" />
                        <AlternatingItemStyle BackColor="Beige" CssClass="dgAlternatingItemStyle" Font-Bold="False"
                            Font-Italic="False" Font-Overline="False" Font-Strikeout="False" Font-Underline="False" />
                        <ItemStyle CssClass="dgItemStyle" />
                        <HeaderStyle BackColor="#CDD7EE" CssClass="dgHeaderStyle" Font-Bold="True" Font-Italic="False"
                            Font-Overline="False" Font-Size="Medium" Font-Strikeout="False" Font-Underline="False" />
                        <Columns>
                            <asp:BoundColumn DataField="DateTime" DataFormatString="{0:dd/MM/yyyy}" HeaderText="Date">
                            </asp:BoundColumn>
                            <asp:BoundColumn DataField="ScreenName" HeaderText="Screen Name"></asp:BoundColumn>
                            <asp:BoundColumn DataField="MethodName" HeaderText="Method Name">
                                <HeaderStyle Width="40%" />
                            </asp:BoundColumn>
                            <asp:BoundColumn DataField="ErrorMessage" HeaderText="Error Message">
                                <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                    Font-Underline="False" HorizontalAlign="Center" />
                            </asp:BoundColumn>
                        </Columns>
                    </asp:DataGrid>
                </td>
            </tr>
            <tr>
                <td style="width: 119px">
                </td>
                <td style="width: 100px">
                </td>
                <td style="width: 100px">
                </td>
            </tr>
        </table>
    </div>
    <%--   </ContentTemplate>
    </atlas:UpdatePanel>--%>
</asp:Content>
