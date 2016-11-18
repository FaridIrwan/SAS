<%@ Page Language="VB" MasterPageFile="~/MasterPage3.master" AutoEventWireup="false" MaintainScrollPositionOnPostback="true" CodeFile="Kokorikulum.aspx.vb" Inherits="Kokorikulum" Title="Kokorikulum" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <script language="javascript" type="text/javascript">
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
        function checkValue() {

            if (((event.keyCode < 48) || (event.keyCode > 57)) && (event.keyCode != 13) && (event.keyCode != 46)) {
                alert("Enter Valid Amount");
                event.keyCode = 0;
            }
        }
        function checknValue() {

            if (((event.keyCode < 48) || (event.keyCode > 57)) && (event.keyCode != 13) && (event.keyCode != 46)) {
                alert("Enter Valid Values");
                event.keyCode = 0;
            }
        }
        function validate() {
            var re = /\s*((\S+\s*)*)/;
            if (document.getElementById("<%=txtKokoCode.ClientID%>").value.replace(re, "$1").length == 0) {
                alert("Fee Code Field Cannot Be Blank");
                document.getElementById("<%=txtKokoCode.ClientID%>").focus();
                return false;
            }
            var digits = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            var temp;
            for (var i = 0; i < document.getElementById("<%=txtKokoCode.ClientID %>").value.length; i++) {
                temp = document.getElementById("<%=txtKokoCode.ClientID%>").value.substring(i, i + 1);
                if (digits.indexOf(temp) == -1) {
                    alert("Enter Valid Fee Code");
                    document.getElementById("<%=txtKokoCode.ClientID%>").focus();
                    return false;
                }
            }
            if (document.getElementById("<%=txtKokoDesc.ClientID%>").value.replace(re, "$1").length == 0) {
                alert("Fee Description Field Cannot Be Blank");
                document.getElementById("<%=txtKokoDesc.ClientID%>").focus();
                return false;
            }

            if (document.getElementById("<%=txtCreditHours.ClientID%>").value.replace(re, "$1").length == 0) {
                alert("Fee Priority Field Cannot Be Blank");
                document.getElementById("<%=txtCreditHours.ClientID%>").focus();
                return false;
            }
            var digits = "0123456789";
            var temp;
            for (var i = 0; i < document.getElementById("<%=txtCreditHours.ClientID %>").value.length; i++) {
                temp = document.getElementById("<%=txtCreditHours.ClientID%>").value.substring(i, i + 1);
                if (digits.indexOf(temp) == -1) {
                    alert("Enter valid Credit Hours");
                    document.getElementById("<%=txtCreditHours.ClientID%>").focus();
                    return false;
                }
            }
            if (document.getElementById("<%=txtGlAmount.ClientID%>").value.replace(re, "$1").length == 0) {
                alert("GL Account Field Cannot Be Blank");
                document.getElementById("<%=txtGlAmount.ClientID%>").focus();
                return false;
            }

            return true;
        }
        function getconfirm() {
            if (document.getElementById("<%=txtKokoCode.ClientID%>").value == "") {
                alert("Select a Record to Delete");
                return false;
            }
            else {
                if (confirm("Do you want to Delete Record?")) {
                    return true;
                }
                else {
                    return false;
                }
            }
            return true;
        }
    </script>
    <%-- </ContentTemplate>
    </atlas:UpdatePanel>--%>
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
                                <asp:ImageButton ID="ibnPrint" runat="server" ImageUrl="~/images/print.png" ToolTip="Print" />
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
                    <asp:TextBox ID="txtRecNo" runat="server" disabled="disabled" TabIndex="1" dir="ltr"
                        Width="52px" ReadOnly="true" CssClass="text_box" AutoPostBack="True" Style="text-align: right"
                        MaxLength="7"></asp:TextBox>
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
        <table class="mainbg" style="width: 100%;">
            <tr>
                <td class="vline" style="width: 98%; height: 1px">
                </td>
            </tr>
        </table>
        <table class="mainbg" style="width: 100%;">
            <tr>
                <td style="width: 494px; height: 39px;">
                    <asp:SiteMapPath ID="SiteMapPath1" runat="server">
                    </asp:SiteMapPath>
                </td>
                <td align="right"  class="pagetext" style="height: 39px">
                    <asp:Label ID="lblMenuName" runat="server" Text="University Fund" Width="253px"></asp:Label>
                </td>
            </tr>
        </table>
    </asp:Panel>
    <%-- </ContentTemplate>
    </atlas:UpdatePanel>--%>
    <asp:Panel ID="PnlAdd" runat="server" Width="100%">
        <table style="width: 100%">
            <tr>
                <td style="width: 100%">
                    <div  style="border: thin solid #A6D9F4; width: 100%">
                        <table class="fields" style="width: 100%; height: 100%;">
                            <tr>
                                <td style="width: 3px; height: 26px">
                                </td>
                                <td class="fields" style="width: 147px; height: 26px">
                                </td>
                                <td style="width: 12px; height: 26px; text-align: center">
                                    <asp:Label ID="lblMsg" runat="server" CssClass="lblError" Width="348px"></asp:Label>
                                </td>
                                <td style="width: 218px; height: 26px">
                                </td>
                                <td style="width: 100px; height: 26px">
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 3px; height: 26px">
                                    <span style="font-size: 11pt; color: #ff0066">*</span></td>
                                <td class="fields" style="width: 147px; height: 26px">
                                    <asp:Label ID="lblkokoCode" runat="server" Text="Kokurikulum Code"></asp:Label>
                                </td>
                                <td style="width: 12px; height: 26px">
                                    <asp:TextBox ID="txtKokoCode" runat="server" Width="142px" MaxLength="20"></asp:TextBox>
                                </td>
                                <td style="width: 218px; height: 26px">
                                </td>
                                <td style="width: 100px; height: 26px">
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 3px; height: 21px">
                                    <span style="font-size: 11pt; color: #ff0066">*</span></td>
                                <td class="fields" style="width: 147px; height: 21px">
                                    <asp:Label ID="lblKokoDesc" runat="server" Text="Kokurikulum Description"></asp:Label>
                                </td>
                                <td style="width: 12px; height: 21px">
                                    <asp:TextBox ID="txtKokoDesc" runat="server" Width="346px" MaxLength="50"></asp:TextBox>
                                </td>
                                <td style="width: 218px; height: 21px">
                                </td>
                                <td style="width: 100px; height: 21px">
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 3px; height: 16px;">
                                    <span style="font-size: 11pt; color: #ff0066">*</span></td>
                                <td class="fields" style="width: 147px; height: 16px;">
                                    <asp:Label ID="lblCreditHours" runat="server" Text="Credit Hours"></asp:Label>
                                </td>
                                <td style="width: 12px; height: 16px;">
                                    <asp:TextBox ID="txtCreditHours" runat="server" MaxLength="2" Width="42px"></asp:TextBox>
                                </td>
                                <td style="width: 218px; height: 16px;">
                                </td>
                                <td style="width: 100px; height: 16px;">
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 3px; height: 25px;">
                                    <span style="font-size: 11pt; color: #ff0066">*</span></td>
                                <td class="fields" style="width: 147px; height: 25px;">
                                    <asp:Label ID="lblGLCode" runat="server" Text="GL Code"></asp:Label>
                                </td>
                                <td class="fields" style="height: 25px;" colspan="2">
                                    <asp:TextBox ID="txtGlAmount" runat="server" MaxLength="25" Width="346px"></asp:TextBox>
                                    <asp:Button ID="Check" runat="server" Text="Check" />
                                    <asp:Image ID="imgGL" runat="server" Height="18px" ImageAlign="Baseline" 
                                        ImageUrl="~/images/check.png" Visible="False" />
                                </td>
                                <td style="width: 100px; height: 25px;">
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 3px; height: 25px">
                                </td>
                                <td class="fields" style="width: 147px; height: 25px">
                                    <asp:Label ID="Label8" runat="server" Style="vertical-align: middle" Text="Status"
                                        Width="78px"></asp:Label>
                                </td>
                                <td class="fields" style="width: 12px; height: 25px">
                                    <asp:DropDownList ID="ddlStatus" runat="server" Width="146px">
                                        <asp:ListItem Value="1">Active</asp:ListItem>
                                        <asp:ListItem Value="0">Inactive</asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                                <td style="width: 218px; height: 25px">
                                </td>
                                <td style="width: 100px; height: 25px">
                                </td>
                            </tr>
                        </table>
                    </div>
                </td>
            </tr>
        </table>
    </asp:Panel>
    <asp:Panel ID="pnlView" runat="server" Width="100%">
        <table style="width: 100%;">
            <tr>
                <td style="width: 3px; height: 16px;">
                </td>
                <td class="fields" style="width: 100%; height: 16px;">
                    &nbsp;<asp:DataGrid ID="dgView" runat="server" AutoGenerateColumns="False" Width="100%"
                        Style="vertical-align: top">
                        <FooterStyle CssClass="dgFooterStyle" Height="20px" />
                        <SelectedItemStyle CssClass="dgSelectedItemStyle" />
                        <AlternatingItemStyle BackColor="Beige" CssClass="dgAlternatingItemStyle" Font-Bold="False"
                            Font-Italic="False" Font-Overline="False" Font-Strikeout="False" Font-Underline="False" />
                        <ItemStyle CssClass="dgItemStyle" />
                        <HeaderStyle BackColor="#CDD7EE" CssClass="dgHeaderStyle" Font-Bold="True" Font-Italic="False"
                            Font-Overline="False" Font-Size="Medium" Font-Strikeout="False" Font-Underline="False" />
                        <Columns>
                            <asp:BoundColumn DataField="SCCode" HeaderText="Category Code"></asp:BoundColumn>
                            <asp:BoundColumn DataField="SCDesc" HeaderText="Category Name"></asp:BoundColumn>
                            <asp:TemplateColumn HeaderText="Fee Amount">
                                <ItemTemplate>
                                    <asp:TextBox ID="txtFeeAmount" runat="server" Width="67px"></asp:TextBox>
                                </ItemTemplate>
                            </asp:TemplateColumn>
                            <asp:BoundColumn DataField="FSAmount" HeaderText="FeeAmount" Visible="False"></asp:BoundColumn>
                        </Columns>
                    </asp:DataGrid>
                </td>
                <td style="width: 100px; height: 16px;">
                </td>
            </tr>
        </table>
    </asp:Panel>
    <%-- </ContentTemplate>
    </atlas:UpdatePanel>--%>
</asp:Content>
