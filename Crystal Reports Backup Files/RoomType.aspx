<%@ Page Language="VB" MasterPageFile="~/MasterPage3.master" AutoEventWireup="false" MaintainScrollPositionOnPostback="true"
    CodeFile="RoomType.aspx.vb" Inherits="RoomType" Title="Untitled Page" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <script language="javascript" type="text/javascript">
        function geterr() {
            var digits = "0123456789";
            var temp;
            for (var i = 0; i < document.getElementById("<%=txtRecNo.ClientID %>").value.length; i++) {
                temp = document.getElementById("<%=txtRecNo.ClientID%>").value.substring(i, i + 1);
                if (digits.indexOf(temp) == -1) {
                    alert("Please Enter correct Record No");
                    document.getElementById("<%=txtRecNo.ClientID%>").value = 1;
                    document.getElementById("<%=txtRecNo.ClientID%>").focus();
                    return false;
                }
            }
            return true;
        }
        function validate() {
            var re = /\s*((\S+\s*)*)/;
            if (document.getElementById("<%=ddlKolej.ClientID%>").value == "-1") {
                alert("Select a Kolej");
                document.getElementById("<%=ddlKolej.ClientID%>").focus();
                return false;
            }
            if (document.getElementById("<%=ddlblock.ClientID%>").value == "-1") {
                alert("Select a Block");
                document.getElementById("<%=ddlblock.ClientID%>").focus();
                return false;
            }
            if (document.getElementById("<%=txtRoomTypeCode.ClientID%>").value.replace(re, "$1").length == 0) {
                alert("RoomType Code Field Cannot Be Blank");
                document.getElementById("<%=txtRoomTypeCode.ClientID%>").focus();
                return false;
            }
            var digits = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            var temp;
            for (var i = 0; i < document.getElementById("<%=txtRoomTypeCode.ClientID %>").value.length; i++) {
                temp = document.getElementById("<%=txtRoomTypeCode.ClientID%>").value.substring(i, i + 1);
                if (digits.indexOf(temp) == -1) {
                    alert("Enter Valid RoomType Code");
                    document.getElementById("<%=txtRoomTypeCode.ClientID%>").focus();
                    return false;
                }
            }
            if (document.getElementById("<%=txtRoomTypeDesc.ClientID%>").value.replace(re, "$1").length == 0) {
                alert(" RoomType Description Field Cannot Be Blank");
                document.getElementById("<%=txtRoomTypeDesc.ClientID%>").focus();
                return false;
            }

            return true;
        }
        function getconfirm() {
            var re = /\s*((\S+\s*)*)/;
            if (document.getElementById("<%=txtRoomTypeCode.ClientID%>").value.replace(re, "$1").length == 0) {

                alert("Select a Record");
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
                    <asp:ImageButton ID="ibtnFirst" runat="server" ImageUrl="~/images/new_last.png" OnClick="ibtnFirst_Click" />
                </td>
                <td style="width: 2%; height: 14px">
                    <asp:ImageButton ID="ibtnPrevs" runat="server" ImageUrl="~/images/new_prev.png" OnClick="ibtnPrevs_Click" />
                </td>
                <td style="width: 2%; height: 14px">
                    <asp:TextBox ID="txtRecNo" runat="server" disabled="disabled" TabIndex="1" dir="ltr"
                        ReadOnly="true" CssClass="text_box" AutoPostBack="True" MaxLength="7" Style="text-align: right"
                        Width="52px" OnTextChanged="txtRecNo_TextChanged"></asp:TextBox>
                </td>
                <td style="width: 2%; height: 14px">
                    <asp:Label ID="Label47" runat="server">Of</asp:Label>
                </td>
                <td style="width: 2%; height: 14px">
                    <asp:Label ID="lblCount" runat="server" Width="20px"></asp:Label>
                </td>
                <td style="width: 2%; height: 14px">
                    <asp:ImageButton ID="ibtnNext" runat="server" ImageUrl="~/images/next.png" OnClick="ibtnNext_Click" />
                </td>
                <td style="width: 2%; height: 14px">
                    <asp:ImageButton ID="ibtnLast" runat="server" ImageUrl="~/images/new_first.png" OnClick="ibtnLast_Click" />
                </td>
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
                <td align="right"  class="pagetext" style="height: 39px">
                    <asp:Label ID="lblMenuName" runat="server" Text="Room Type" Width="253px"></asp:Label>
                </td>
            </tr>
        </table>
    </asp:Panel>
    <%-- </ContentTemplate>
    </atlas:UpdatePanel>
    <atlas:UpdateProgress ID="ProgressIndicator" runat="server">
        <ProgressTemplate>
            Loading the data, please wait...
            <asp:Image ID="LoadingImage" runat="server" ImageAlign="AbsMiddle" ImageUrl="~/Images/spinner.gif" />
        </ProgressTemplate>
    </atlas:UpdateProgress>
    <atlas:UpdatePanel ID="up2" runat="server">
        <ContentTemplate>--%>
    <asp:Panel ID="PnlAdd" runat="server" Width="100%">
         <div  style="border: thin solid #A6D9F4; width: 100%">
            <table class="fields" style="width: 100%; height: 100%; text-align: left;">
                <tr>
                    <td class="fields" style="width: 67px; height: 17px">
                    </td>
                    <td style="width: 43px; height: 17px">
                        <asp:Label ID="lblMsg" runat="server" CssClass="lblError" Style="text-align: center"
                            Width="348px"></asp:Label>
                    </td>
                    <td style="width: 100px; height: 17px">
                    </td>
                </tr>
                <tr>
                    <td class="fields" style="width: 67px; height: 17px">
                        &nbsp;
                        <asp:Label ID="Label1" runat="server" Text="Kolej"></asp:Label>
                    </td>
                    <td>
                        <span style="font-size: 11pt; color: #ff0066">
                            <asp:DropDownList ID="ddlKolej" runat="server" AppendDataBoundItems="True" AutoPostBack="True"
                                OnSelectedIndexChanged="ddlKolej_SelectedIndexChanged" TabIndex="17" Width="148px">
                                <asp:ListItem Value="-1">--Select--</asp:ListItem>
                            </asp:DropDownList>
                            *</span>
                    </td>
                    <td style="width: 100px; height: 17px">
                    </td>
                </tr>
                <tr>
                    <td class="fields" style="width: 67px; height: 17px">
                        &nbsp;
                        <asp:Label ID="lblKolej" runat="server" Text="Block"></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlblock" runat="server" AppendDataBoundItems="True" AutoPostBack="True"
                            TabIndex="18" Width="148px">
                            <asp:ListItem Value="-1">--Select--</asp:ListItem>
                        </asp:DropDownList>
                        <span style="font-size: 11pt; color: #ff0066">*</span>
                    </td>
                    <td style="width: 100px; height: 17px">
                    </td>
                </tr>
                <tr>
                    <td class="fields" style="width: 67px; height: 26px">
                        &nbsp;
                        <asp:Label ID="lbRoomTypeCode" runat="server" Text="Room Type Code" Width="119px"></asp:Label>
                    </td>
                    <td style="width: 43px; height: 26px">
                        <asp:TextBox ID="txtRoomTypeCode" runat="server" Width="142px" MaxLength="20"></asp:TextBox><span
                            style="font-size: 11pt; color: #ff0066">*</span>
                    </td>
                    <td style="width: 100px; height: 26px">
                    </td>
                </tr>
                <tr>
                    <td class="fields" style="width: 67px; height: 21px">
                        &nbsp;
                        <asp:Label ID="lbRoomTypedesc" runat="server" Text="Room Type Description" Width="146px"></asp:Label>
                    </td>
                    <td style="width: 43px; height: 21px">
                        <asp:TextBox ID="txtRoomTypeDesc" runat="server" Width="346px" MaxLength="50"></asp:TextBox><span
                            style="font-size: 11pt; color: #ff0066">*</span>
                    </td>
                    <td style="width: 100px; height: 21px">
                    </td>
                </tr>
            </table>
            <br />
        </div>
    </asp:Panel>
    <%--             
        </ContentTemplate>
    </atlas:UpdatePanel>--%>
</asp:Content>
