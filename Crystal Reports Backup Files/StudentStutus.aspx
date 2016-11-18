<%@ Page Language="VB" MasterPageFile="~/MasterPage3.master" AutoEventWireup="false" MaintainScrollPositionOnPostback="true"
    CodeFile="StudentStutus.aspx.vb" Inherits="studentStutus" Title="Welcome to SAS" %>

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
            if (document.getElementById("<%=txtStatusCode.ClientID%>").value.replace(re, "$1").length == 0) {
                alert(" Student Status Code Field Cannot Be Blank");
                document.getElementById("<%=txtStatusCode.ClientID%>").focus();
                return false;
            }
            var digits = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            var temp;
            for (var i = 0; i < document.getElementById("<%=txtStatusCode.ClientID %>").value.length; i++) {
                temp = document.getElementById("<%=txtStatusCode.ClientID%>").value.substring(i, i + 1);
                if (digits.indexOf(temp) == -1) {
                    alert("Enter Valid Student Status Code");
                    document.getElementById("<%=txtStatusCode.ClientID%>").focus();
                    return false;
                }
            }
            if (document.getElementById("<%=txtStatusDesc.ClientID%>").value.replace(re, "$1").length == 0) {
                alert("Student Status Description Field Cannot Be Blank");
                document.getElementById("<%=txtStatusDesc.ClientID%>").focus();
                return false;
            }
            return true;
        }
        function disable() {
            // Check if we are disabled or enabling the test drop down list
            //var div = document.getElementById('dg');
            var filterDDL = document.getElementById('<%= ddlblStatus.ClientID %>');

            if (filterDDL.options[filterDDL.selectedIndex].value == "Inactive") {
                // Enable the test drop down list
                document.getElementById('<%= ddlStatus.ClientID %>').disabled = false;
                document.getElementById('dg').style.display = "block"
                return false;

            }
            else if (filterDDL.options[filterDDL.selectedIndex].value == "1") {
                document.getElementById('<%= ddlStatus.ClientID %>').disabled = false;
                document.getElementById('dg').style.display = "block"

                return true;
            }
            else {
                document.getElementById('<%= ddlStatus.ClientID %>').disabled = true;
                document.getElementById('dg').style.display = "none";

                return true;
            }
        }


        function getconfirm() {
            if (document.getElementById("<%=txtStatusCode.ClientID%>").value == "") {

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
    <%--<atlas:ScriptManager ID="scriptmanager1" EnablePartialRendering="true" runat="Server" />
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
                    <asp:TextBox ID="txtRecNo" runat="server" disabled="disabled" TabIndex="1" dir="ltr"
                        Width="52px" AutoPostBack="True" Style="text-align: right" MaxLength="7" ReadOnly="true"
                        CssClass="text_box"></asp:TextBox>
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
                <td  align="right"  class="pagetext" style="height: 39px"  >
                    <asp:Label ID="lblMenuName" runat="server" Text="Student Status" Width="253px"></asp:Label>
                </td>
            </tr>
        </table>
    </asp:Panel>
    <%--  </ContentTemplate>     
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
                    <div style="border: thin solid #A6D9F4; width: 100%">
                        <table class="fields" style="width: 100%; height: 100%">
                            <tr>
                                <td style="width: 6px; height: 26px">
                                </td>
                                <td class="fields" style="width: 67px; height: 26px">
                                </td>
                                <td style="width: 43px; height: 26px">
                                    <asp:Label ID="lblMsg" runat="server" CssClass="lblError" Style="text-align: center"
                                        Width="348px"></asp:Label>
                                </td>
                                <td style="width: 100px; height: 26px">
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 6px; height: 26px">
                                </td>
                                <td class="fields" style="width: 67px; height: 26px">
                                    <asp:Label ID="Label1" runat="server" Text="Status Code" Width="76px"></asp:Label>
                                </td>
                                <td style="width: 43px; height: 26px">
                                    <asp:TextBox ID="txtStatusCode" runat="server" Width="142px" MaxLength="20"></asp:TextBox><span
                                        style="font-size: 11pt; color: #ff0066">*</span>
                                </td>
                                <td style="width: 100px; height: 26px">
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 6px; height: 21px">
                                </td>
                                <td class="fields" style="width: 67px; height: 21px">
                                    <asp:Label ID="Label2" runat="server" Text="Status Description" Width="117px"></asp:Label>
                                </td>
                                <td style="width: 43px; height: 21px">
                                    <asp:TextBox ID="txtStatusDesc" runat="server" Width="346px" MaxLength="50"></asp:TextBox><span
                                        style="font-size: 11pt; color: #ff0066">*</span>
                                </td>
                                <td style="width: 100px; height: 21px">
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 6px; height: 21px">
                                </td>
                                <td class="fields" style="width: 67px; height: 21px">
                                    <asp:Label ID="Label3" runat="server" Style="vertical-align: middle" Text="Business Logic Status"
                                        Width="144px"></asp:Label>
                                </td>
                                <td style="width: 43px; height: 21px">
                                    <asp:DropDownList ID="ddlblStatus" runat="server" Width="146px">
                                        <asp:ListItem Value="1">Active</asp:ListItem>
                                        <asp:ListItem Value="0">Inactive</asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                                <td style="width: 100px; height: 21px">
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 6px; height: 21px">
                                </td>
                                <td class="fields" style="width: 67px; height: 21px">
                                    <asp:Label ID="Label7" runat="server" Style="vertical-align: middle" Text="Status"
                                        Width="78px"></asp:Label>
                                </td>
                                <td style="width: 43px; height: 21px">
                                    <asp:DropDownList ID="ddlStatus" runat="server" Width="146px">
                                        <asp:ListItem Value="1">Active</asp:ListItem>
                                        <asp:ListItem Value="0">Inactive</asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                                <td style="width: 80px; height: 21px">
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 6px; height: 21px">
                                </td>
                                <td class="fields" style="width: 67px; height: 21px">
                                </td>
                                <td style="width: 43px; height: 21px">
                                    <div id="dg">
                                        <asp:DataGrid ID="dgMenuList" runat="server" AllowSorting="True" AutoGenerateColumns="False"
                                            Font-Names="verdana" Font-Size="8pt" Height="17px" OnItemDataBound="dgMenuList_ItemDataBound"
                                            Width="100%">
                                            <FooterStyle CssClass="dgFooterStyle" Height="20px" />
                                            <SelectedItemStyle CssClass="dgSelectedItemStyle" />
                                            <AlternatingItemStyle BackColor="Beige" CssClass="dgAlternatingItemStyle" Font-Bold="False"
                                                Font-Italic="False" Font-Overline="False" Font-Strikeout="False" Font-Underline="False" />
                                            <ItemStyle CssClass="dgItemStyle" />
                                            <Columns>
                                                <asp:BoundColumn DataField="menuid" HeaderText="Menu ID" Visible="False">
                                                    <HeaderStyle Font-Bold="True" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                                        Font-Underline="False" />
                                                </asp:BoundColumn>
                                                <asp:BoundColumn DataField="MenuName" HeaderText="Menu Name" Visible="False">
                                                    <HeaderStyle Font-Bold="True" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                                        Font-Underline="False" />
                                                </asp:BoundColumn>
                                                <asp:BoundColumn DataField="PageName" HeaderText="Page Name"></asp:BoundColumn>
                                                <asp:TemplateColumn>
                                                    <HeaderTemplate>
                                                        <asp:Label ID="lblList" runat="server" Font-Bold="True" Text="Add" Width="56px"></asp:Label>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:CheckBox ID="ChkStatus" runat="server" AutoPostBack="true" />
                                                    </ItemTemplate>
                                                </asp:TemplateColumn>
                                                <asp:BoundColumn DataField="Status" HeaderText="status"></asp:BoundColumn>
                                            </Columns>
                                            <HeaderStyle BackColor="#CDD7EE" CssClass="dgHeaderStyle" Font-Bold="True" Font-Italic="False"
                                                Font-Overline="False" Font-Size="Medium" Font-Strikeout="False" Font-Underline="False" />
                                        </asp:DataGrid></div>
                                </td>
                                <td style="width: 100px; height: 21px">
                                </td>
                            </tr>
                        </table>
                    </div>
                </td>
            </tr>
        </table>
    </asp:Panel>
    <asp:Panel ID="PnlView" runat="server" Height="100%" Width="100%">
        <table style="width: 100%; height: 100%;">
            <tr>
                <td style="width: 3px">
                </td>
                <td class="fields" style="width: 100%">
                    &nbsp;<asp:DataGrid ID="dgView" runat="server" Width="100%" AutoGenerateColumns="False"
                        DataKeyField="SASS_Code">
                        <FooterStyle CssClass="dgFooterStyle" Height="20px" />
                        <SelectedItemStyle CssClass="dgSelectedItemStyle" />
                        <AlternatingItemStyle BackColor="Beige" CssClass="dgAlternatingItemStyle" Font-Bold="False"
                            Font-Italic="False" Font-Overline="False" Font-Strikeout="False" Font-Underline="False" />
                        <ItemStyle CssClass="dgItemStyle" />
                        <HeaderStyle BackColor="#CDD7EE" CssClass="dgHeaderStyle" Font-Bold="True" Font-Italic="False"
                            Font-Overline="False" Font-Size="Medium" Font-Strikeout="False" Font-Underline="False" />
                        <Columns>
                            <asp:ButtonColumn CommandName="Select" DataTextField="StudentStatusCode" HeaderText="Status Code"
                                Text="StudentStatusCode"></asp:ButtonColumn>
                            <asp:BoundColumn DataField="Description" HeaderText="Status Description"></asp:BoundColumn>
                        </Columns>
                    </asp:DataGrid>
                </td>
                <td style="width: 100px">
                </td>
            </tr>
        </table>
    </asp:Panel>
    <%--</ContentTemplate>     
</atlas:UpdatePanel>--%>
</asp:Content>
