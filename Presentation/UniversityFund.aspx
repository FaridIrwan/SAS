<%@ Page Language="VB" MasterPageFile="~/MasterPage3.master" AutoEventWireup="false" MaintainScrollPositionOnPostback="true"
    CodeFile="UniversityFund.aspx.vb" Inherits="universityfund" Title="Welcome to SAS" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <script language="javascript" type="text/javascript">
        function geterr() {
            var digits = "0123456789";
            var temp;
            for (var i = 0; i < document.getElementById("<%=txtRecNo.ClientID %>").value.length; i++) {
                temp = document.getElementById("<%=txtRecNo.ClientID%>").value.substring(i, i + 1);
                if (digits.indexOf(temp) == -1) {
                    alert("Enter valid Record No");
                    document.getElementById("<%=txtRecNo.ClientID%>").value = 1;
                    document.getElementById("<%=txtRecNo.ClientID%>").focus();
                    return false;
                }
            }
            return true;
        }
        function validate() {
            var re = /\s*((\S+\s*)*)/;
            if (document.getElementById("<%=txtFundCode.ClientID%>").value.replace(re, "$1").length == 0) {
                alert("University Fund Code Field Cannot Be Blank");
                document.getElementById("<%=txtFundCode.ClientID%>").focus();
                return false;
            }
            var digits = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            var temp;
            for (var i = 0; i < document.getElementById("<%=txtFundCode.ClientID %>").value.length; i++) {
                temp = document.getElementById("<%=txtFundCode.ClientID%>").value.substring(i, i + 1);
                if (digits.indexOf(temp) == -1) {
                    alert("Enter Valid University Fund Code");
                    document.getElementById("<%=txtFundCode.ClientID%>").focus();
                    return false;
                }
            }
            if (document.getElementById("<%=txtFundName.ClientID%>").value.replace(re, "$1").length == 0) {
                alert("University Fund Description Field Cannot Be Blank");
                document.getElementById("<%=txtFundName.ClientID%>").focus();
                return false;
            }
            if (document.getElementById("<%=txtGlaccount.ClientID%>").value.replace(re, "$1").length == 0) {
                alert("University Fund GL Account Field Cannot Be Blank");
                document.getElementById("<%=txtGlaccount.ClientID%>").focus();
                return false;
            }
            return true;
        }
        function getconfirm() {
            if (document.getElementById("<%=txtFundCode.ClientID%>").value == "") {

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
    <%-- <atlas:ScriptManager ID="scriptmanager1" EnablePartialRendering="true" runat="Server" />
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
                <%-- Editted By Zoya @25/02/2016--%>
                <td style="float: left;display:None; visibility:collapse;">
                    <table style="border-collapse: collapse;" class="menuoff" onmouseover="className='menuon';"
                        onmouseout="className='menuoff';">
                        <tr>
                            <td>
                                <asp:ImageButton ID="ibtnPrint" runat="server" ImageUrl="~/images/print.png" ToolTip="Print" Visible="false"/>
                            </td>
                            <td>
                                <asp:Label ID="Label17" runat="server" Text="Print" Visible="false"></asp:Label>
                            </td>
                        </tr>
                    </table>
                </td>
                <%--Done Editted By Zoya @25/02/2016--%>

                <%-- Editted By Zoya @24/02/2016--%>
                <td style="float: left;display:None; visibility:collapse;">
                    <table style="border-collapse: collapse;" class="menuoff" onmouseover="className='menuon';"
                        onmouseout="className='menuoff';">
                        <tr>
                            <td>
                                <asp:ImageButton ID="ibtnPosting" runat="server" ImageUrl="~/images/gposting.png"
                                    ToolTip="Cancel" Visible="false" />
                            </td>
                            <td>
                                <asp:Label ID="Label6" runat="server" Text="Posting" Visible="false"></asp:Label>
                            </td>
                        </tr>
                    </table>
                </td>
                <td style="float: left;display:None; visibility:collapse;">
                    <table style="border-collapse: collapse;" class="menuoff" onmouseover="className='menuon';"
                        onmouseout="className='menuoff';">
                        <tr>
                            <td>
                                <asp:ImageButton ID="ibtnOthers" runat="server" ImageUrl="~/images/gothers.gif" ToolTip="Cancel" Visible="false"/>
                            </td>
                            <td>
                                <asp:Label ID="Label5" runat="server" Text="Others" Visible="false"></asp:Label>
                            </td>
                        </tr>
                    </table>
                </td>
                <td style="float: left">
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
               <%-- <td style="width: 3%; height: 14px">
                </td>--%>

                <%-- Done Editted By Zoya @24/02/2016--%>

                <td style="width: 2%; height: 14px">
                    <asp:ImageButton ID="ibtnFirst" runat="server" ImageUrl="~/images/new_last.png" />
                </td>
                <td style="height: 14px; width: 4px;">
                </td>
                <td style="width: 2%; height: 14px">
                    <asp:ImageButton ID="ibtnPrevs" runat="server" ImageUrl="~/images/new_prev.png" />
                </td>
                <td style="width: 2%; height: 14px">
                    <asp:TextBox ID="txtRecNo" runat="server" Width="52px" AutoPostBack="True" Style="text-align: right"
                        MaxLength="7" ReadOnly="true" CssClass="text_box" disabled="disabled" TabIndex="1"
                        dir="ltr"></asp:TextBox>
                </td>
                <td style="width: 2%; height: 14px">
                    <asp:Label ID="Label47" runat="server">Of</asp:Label>
                </td>
                <td style="width: 2%; height: 14px">
                    <asp:Label ID="lblCount" runat="server" Width="20px"></asp:Label>
                </td>
                <td style="width: 2%; height: 14px">
                    <asp:ImageButton ID="ibtnNext" runat="server" ImageUrl="~/images/new_next.png" />
                </td>
                <td style="height: 14px">
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
                <td class="pagetext" style="height: 39px" align="right">
                    <asp:Label ID="lblMenuName" runat="server" Text="University Fund" Width="253px"></asp:Label>
                </td>
            </tr>
        </table>
    </asp:Panel>
    <%-- </ContentTemplate>     
</atlas:UpdatePanel>
<atlas:UpdateProgress ID="ProgressIndicator" runat="server">
    <ProgressTemplate>
        Loading the data, please wait... 
        <asp:Image ID="LoadingImage" ImageAlign=AbsMiddle runat="server" ImageUrl="~/Images/spinner.gif" />        
    </ProgressTemplate>
 </atlas:UpdateProgress>
<atlas:UpdatePanel ID="up2" runat="server">
<ContentTemplate>--%>
    <asp:Panel ID="PnlAdd" runat="server" Width="100%">
        <table style="width: 100%">
            <tr>
                <td style="width: 100%">
                        <div  style="border: thin solid #A6D9F4; width: 100%">
                        <table class="fields" style="width: 100%;">
                            <tr>
                                <td style="width: 3px; height: 26px">
                                </td>
                                <td class="fields" style="width: 66px; height: 26px">
                                </td>
                                <td style="width: 134px; height: 26px; text-align: center">
                                    <asp:Label ID="lblMsg" runat="server" CssClass="lblError" Style="text-align: center"
                                        Width="348px"></asp:Label>
                                </td>
                                <td style="width: 178px; height: 26px">
                                </td>
                                <td style="width: 100px; height: 26px">
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 3px; height: 26px">
                                </td>
                                <td class="fields" style="width: 66px; height: 26px">
                                    <span
                                        style="font-size: 11pt; color: #ff0066">*</span>
                                    <asp:Label ID="Label1" runat="server" Text="Code"></asp:Label>
                                </td>
                                <td style="height: 26px" colspan="2">
                                    <asp:TextBox ID="txtFundCode" runat="server" Width="142px" MaxLength="20"></asp:TextBox>
                                </td>
                                <td style="width: 100px; height: 26px">
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 3px; height: 21px">
                                </td>
                                <td class="fields" style="width: 66px; height: 21px">
                                    <span
                                        style="font-size: 11pt; color: #ff0066">*</span>
                                    <asp:Label ID="Label2" runat="server" Text="Description"></asp:Label>
                                </td>
                                <td style="height: 21px" colspan="2">
                                    <asp:TextBox ID="txtFundName" runat="server" Width="346px" MaxLength="50"></asp:TextBox>
                                </td>
                                <td style="width: 100px; height: 21px">
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 3px; height: 25px;">
                                </td>
                                <td class="fields" style="width: 66px; height: 25px;">
                                    <span style="font-size: 11pt; color: #ff0066">*</span>
                                    <asp:Label ID="Label3" runat="server" Text="GL Account" Width="78px"></asp:Label>
                                </td>
                                <td style="height: 25px;" colspan="2">
                                    <asp:TextBox ID="txtGlAccount" runat="server" Width="142px" MaxLength="20"></asp:TextBox><asp:Button ID="Check" runat="server"
                                            Text="Check" />
                                    &nbsp;
                                     <br />
                                    <asp:Label ID="lblDesc" runat="server"></asp:Label>
                                    <br />
                                    <asp:Image ID="imgGL" runat="server" Height="18px" ImageAlign="Baseline" ImageUrl="~/images/check.png"
                                        Visible="False" />
                                    &nbsp;
                                </td>
                                <td style="width: 100px; height: 25px;">
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 3px">
                                </td>
                                <td class="fields" style="vertical-align: text-top; width: 66px">
                                    <asp:Label ID="Label7" runat="server" Style="vertical-align: middle" Text="Status"
                                        Width="78px"></asp:Label>
                                </td>
                                <td class="fields" style="width: 134px">
                                    <asp:DropDownList ID="ddlStatus" runat="server" Width="146px">
                                        <asp:ListItem Value="1">Active</asp:ListItem>
                                        <asp:ListItem Value="0">Inactive</asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                                <td style="width: 178px">
                                </td>
                                <td style="width: 100px">
                                </td>
                            </tr>
                        </table>
                        <br />
                        <br />
                        </div>
                </td>
            </tr>
        </table>
    </asp:Panel>
    <asp:Panel ID="PnlView" runat="server" Visible="False" Width="100%">
        <table style="width: 100%;">
            <tr>
                <td style="width: 1px; height: 21px">
                </td>
                <td class="fields" style="width: 100%; height: 21px">
                    <asp:DataGrid ID="dgView" runat="server" AutoGenerateColumns="False" DataKeyField="SAUF_Code"
                        Width="100%">
                        <FooterStyle CssClass="dgFooterStyle" Height="20px" />
                        <SelectedItemStyle CssClass="dgSelectedItemStyle" />
                        <AlternatingItemStyle BackColor="Beige" CssClass="dgAlternatingItemStyle" Font-Bold="False"
                            Font-Italic="False" Font-Overline="False" Font-Strikeout="False" Font-Underline="False" />
                        <ItemStyle CssClass="dgItemStyle" />
                        <HeaderStyle BackColor="#CDD7EE" CssClass="dgHeaderStyle" Font-Bold="True" Font-Italic="False"
                            Font-Overline="False" Font-Size="Medium" Font-Strikeout="False" Font-Underline="False" />
                        <Columns>
                            <asp:ButtonColumn CommandName="Select" DataTextField="SAUF_Code" HeaderText="Fund Code"
                                Text="SAUF_Code"></asp:ButtonColumn>
                            <asp:BoundColumn DataField="SAUF_Desc" HeaderText="Fund Name"></asp:BoundColumn>
                            <asp:BoundColumn DataField="SAUF_GLCode" HeaderText="GLAccount"></asp:BoundColumn>
                        </Columns>
                    </asp:DataGrid>
                </td>
                <td style="width: 100px; height: 21px">
                </td>
            </tr>
        </table>
    </asp:Panel>
    <%-- </ContentTemplate>     
</atlas:UpdatePanel>--%>
</asp:Content>
