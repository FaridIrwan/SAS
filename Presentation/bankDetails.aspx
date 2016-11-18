<%@ Page Language="VB" MasterPageFile="~/MasterPage3.master" AutoEventWireup="false" CodeFile="bankDetails.aspx.vb" 
    MaintainScrollPositionOnPostback="true" Inherits="bankDetails" Title="Welcome To SAS" %>

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
       function getconfirm() {
           var re = /\s*((\S+\s*)*)/;
           if (document.getElementById("<%=txtbankCode.ClientID%>").value.replace(re, "$1").length == 0) {

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
function validate() {
    var re = /\s*((\S+\s*)*)/;
    if (document.getElementById("<%=txtBankCode.ClientID%>").value.replace(re, "$1").length == 0) {
          alert("Bank Code Field Cannot Be Blank");
          document.getElementById("<%=txtbankCode.ClientID%>").focus();
                 return false;
             }
             var digits = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
             var temp;
             for (var i = 0; i < document.getElementById("<%=txtBankCode.ClientID %>").value.length; i++) {
         temp = document.getElementById("<%=txtBankCode.ClientID%>").value.substring(i, i + 1);
         if (digits.indexOf(temp) == -1) {
             alert("Enter Valid Bank Code");
             document.getElementById("<%=txtBankCode.ClientID%>").focus();
                        return false;
                    }
                }
                if (document.getElementById("<%=txtBankName.ClientID%>").value.replace(re, "$1").length == 0) {
        alert("Bank Name Field Cannot Be Blank");
        document.getElementById("<%=txtBankName.ClientID%>").focus();
                 return false;
             }
             if (document.getElementById("<%=txtAccountCode.ClientID%>").value.replace(re, "$1").length == 0) {
        alert("Account Code Field Cannot Be Blank");
        document.getElementById("<%=txtAccountCode.ClientID%>").focus();
                 return false;
             }
             if (document.getElementById("<%=txtGlaccount.ClientID%>").value.replace(re, "$1").length == 0) {
        alert("GL Account Field Cannot Be Blank");
        document.getElementById("<%=txtGlaccount.ClientID%>").focus();
                 return false;
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
                <td style="width: 4px; height: 14px"></td>
                <td style="width: 14px; height: 14px"></td>
                <td>
                    <table style="border-collapse: collapse;" class="menuoff" onmouseover="className='menuon';" onmouseout="className='menuoff';">
                        <tr>
                            <td>
                                <asp:ImageButton ID="ibtnNew" runat="server" ImageUrl="images/add.png" ToolTip="New" /></td>
                            <td>
                                <asp:Label ID="Label11" runat="server" Text="New"></asp:Label></td>
                        </tr>
                    </table>
                </td>

                <td>
                    <table style="border-collapse: collapse;" class="menuoff" onmouseover="className='menuon';" onmouseout="className='menuoff';">
                        <tr>
                            <td>
                                <asp:ImageButton ID="ibtnSave" runat="server" ImageUrl="~/images/save.png" ToolTip="Save" /></td>
                            <td>
                                <asp:Label ID="Label14" runat="server" Text="Save"></asp:Label></td>
                        </tr>
                    </table>
                </td>


                <td>
                    <table style="border-collapse: collapse;" class="menuoff" onmouseover="className='menuon';" onmouseout="className='menuoff';">
                        <tr>
                            <td>
                                <asp:ImageButton ID="ibtnDelete" runat="server" ImageUrl="~/images/delete.png" /></td>
                            <td>
                                <asp:Label ID="Label13" runat="server" Text="Delete"></asp:Label></td>
                        </tr>
                    </table>
                </td>

                <td>
                    <table style="border-collapse: collapse;" class="menuoff" onmouseover="className='menuon';" onmouseout="className='menuoff';">
                        <tr>
                            <td>
                                <asp:ImageButton ID="ibtnView" runat="server" ImageUrl="~/images/find.png" ToolTip="View" /></td>
                            <td>
                                <asp:Label ID="Label16" runat="server" Text="Search"></asp:Label></td>
                        </tr>
                    </table>
                </td>
                 <%-- Editted By Zoya @25/02/2016--%>
                <td style="float: left;display:None; visibility:collapse;">
                    <table style="border-collapse: collapse;" class="menuoff" onmouseover="className='menuon';" onmouseout="className='menuoff';">
                        <tr>
                            <td>
                                <asp:ImageButton ID="ibtnPrint" runat="server" ImageUrl="~/images/print.png" ToolTip="Print" Visible="false"/></td>
                            <td>
                                <asp:Label ID="Label17" runat="server" Text="Print" Visible="false"></asp:Label></td>
                        </tr>
                    </table>
                </td>

                <%--<td>
                    <table style="border-collapse: collapse;" class="menuoff" onmouseover="className='menuon';" onmouseout="className='menuoff';">
                    </table>
                </td>--%>
                <%-- Done Editted By Zoya @25/02/2016--%>

                <%-- Editted By Zoya @24/02/2016--%>
                <td style="float: left;display:None; visibility:collapse;">
                    <table style="border-collapse: collapse;" class="menuoff" onmouseover="className='menuon';" onmouseout="className='menuoff';">
                        <tr>
                            <td>
                                <asp:ImageButton ID="ibtnOthers" runat="server" ImageUrl="~/images/gothers.gif" ToolTip="Cancel" Visible="false" /></td>
                            <td>
                                <asp:Label ID="Label5" runat="server" Text="Others" Visible="false"></asp:Label></td>
                        </tr>
                    </table>
                </td>

                <td style="float: left">
                    <table style="border-collapse: collapse;" class="menuoff" onmouseover="className='menuon';" onmouseout="className='menuoff';">
                        <tr>
                            <td>
                                <asp:ImageButton ID="ibtnCancel" runat="server" ImageUrl="~/images/cancel.png" ToolTip="Cancel" /></td>
                            <td>
                                <asp:Label ID="Label18" runat="server" Text="Cancel"></asp:Label></td>
                        </tr>
                    </table>
                </td>

                <%--<td style="width: 3%; height: 14px"></td>--%>

                <%-- Done Editted By Zoya @24/02/2016--%>

                <td style="width: 2%; height: 14px">
                    <asp:ImageButton ID="ibtnFirst" runat="server" ImageUrl="~/images/new_last.png" /></td>
                <td style="width: 2%; height: 14px">
                    <asp:ImageButton ID="ibtnPrevs" runat="server" ImageUrl="~/images/new_prev.png" /></td>
                <td style="width: 2%; height: 14px">
                    <asp:TextBox ID="txtRecNo" runat="server" disabled="disabled" TabIndex="1" dir="ltr" Width="52px" AutoPostBack="True" Style="text-align: right" MaxLength="7" ReadOnly="true" CssClass="text_box"></asp:TextBox></td>
                <td style="width: 2%; height: 14px">
                    <asp:Label ID="Label47" runat="server">Of</asp:Label></td>
                <td style="width: 2%; height: 14px">
                    <asp:Label ID="lblCount" runat="server" Width="20px"></asp:Label></td>

                <td style="width: 2%; height: 14px">
                    <asp:ImageButton ID="ibtnNext" runat="server" ImageUrl="~/images/new_next.png" /></td>

                <td style="width: 2%; height: 14px">
                    <asp:ImageButton ID="ibtnLast" runat="server" ImageUrl="~/images/new_first.png" /></td>
                <td style="width: 2%; height: 14px"></td>
                <td style="width: 100%; height: 14px"></td>
                <td style="width: 100%; height: 14px"></td>
            </tr>

        </table>
        <table class="mainbg" style="width: 100%;">
            <tr>
                <td class="vline" style="width: 98%; height: 1px"></td>
            </tr>
        </table>
        <table class="mainbg" style="width: 100%;">
            <tr>
                <td style="width: 494px; height: 39px;">
                    <asp:SiteMapPath ID="SiteMapPath1" runat="server">
                    </asp:SiteMapPath>
                </td>
                <td class="pagetext" style="height: 39px" align="right">
                    <asp:Label ID="lblMenuName" runat="server" Text="University Fund" Width="253px"></asp:Label></td>
            </tr>
        </table>
    </asp:Panel>
    <%-- </ContentTemplate>     
</atlas:UpdatePanel>--%>
    <%--<atlas:UpdateProgress ID="ProgressIndicator" runat="server">
    <ProgressTemplate>
        Loading the data, please wait... 
        <asp:Image ID="LoadingImage" ImageAlign=AbsMiddle runat="server" ImageUrl="~/Images/spinner.gif" />        
    </ProgressTemplate>
 </atlas:UpdateProgress>--%>
    <%--<atlas:UpdatePanel ID="up2" runat="server">
<ContentTemplate>--%>

    <asp:Panel ID="PnlAdd" runat="server" Width="100%" Visible="true">
        <table style="width: 100%">
            <tr>
                <td style="width: 100%">
                    <div style="border: thin solid #A6D9F4; width: 100%">
                        <table class="fields" style="width: 100%; height: 100%;">
                            <tr>
                                <td style="width: 3px; height: 26px"></td>
                                <td class="fields" style="width: 67px; height: 26px"></td>
                                <td style="width: 12px; height: 26px; text-align: center">
                                    <asp:Label ID="lblMsg" runat="server" CssClass="lblError" Text="Bank Code" Width="348px"></asp:Label></td>
                                <td style="width: 218px; height: 26px"></td>
                                <td style="width: 100px; height: 26px"></td>
                            </tr>
                            <tr>
                                <td style="width: 3px; height: 26px"></td>
                                <td class="fields" style="width: 67px; height: 26px">
                                    <asp:Label ID="Label1" runat="server" Text="Bank Code"></asp:Label></td>
                                <td style="width: 12px; height: 26px">
                                    <asp:TextBox ID="txtBankCode" runat="server" Width="142px" Style="text-align: left" MaxLength="20"></asp:TextBox><span
                                        style="font-size: 11pt; color: #ff0066">*</span></td>
                                <td style="width: 218px; height: 26px"></td>
                                <td style="width: 100px; height: 26px"></td>
                            </tr>
                            <tr>
                                <td style="width: 3px; height: 21px"></td>
                                <td class="fields" style="width: 67px; height: 21px">
                                    <asp:Label ID="Label2" runat="server" Text="Bank Name" Width="99px"></asp:Label></td>
                                <td style="width: 12px; height: 21px">
                                    <asp:TextBox ID="txtBankName" runat="server" Width="346px" MaxLength="50"></asp:TextBox><span style="font-size: 11pt; color: #ff0066">*</span></td>
                                <td style="width: 218px; height: 21px"></td>
                                <td style="width: 100px; height: 21px"></td>
                            </tr>
                            <tr>
                                <td style="width: 3px"></td>
                                <td class="fields" style="width: 67px">
                                    <asp:Label ID="Label4" runat="server" Text="Account Code" Width="91px"></asp:Label></td>
                                <td style="width: 12px">
                                    <asp:TextBox ID="txtAccountCode" runat="server" Width="142px" MaxLength="20"></asp:TextBox><span
                                        style="font-size: 11pt; color: #ff0066">*</span></td>
                                <td style="width: 218px"></td>
                                <td style="width: 100px"></td>
                            </tr>
                            <tr>
                                <td style="width: 3px"></td>
                                <td class="fields" style="width: 67px">
                                    <asp:Label ID="Label3" runat="server" Text="GL Account" Width="78px"></asp:Label></td>
                                <td class="fields" style="width: 12px">
                                    <asp:TextBox ID="txtGlAccount" runat="server" Width="142px" MaxLength="20"></asp:TextBox><span style="font-size: 11pt"><span
                                        style="color: #ff0066; font-size: 8pt;">*</span><span style="color: #ff0066"></span></span>
                                    <asp:Button ID="Check" runat="server" Text="Check" />
                                    &nbsp;
                                    <br />                                    
                                    <asp:Label ID="lblDescGL" runat="server"></asp:Label>
                                    <br />
                                    <asp:Image ID="imgGL" runat="server" Height="18px" ImageAlign="Baseline"
                                        ImageUrl="~/images/check.png" Visible="False" />
                                    &nbsp;</td>
                                <td style="width: 218px"></td>
                                <td style="width: 100px"></td>
                            </tr>
                            <tr>
                                <td style="width: 3px; height: 21px"></td>
                                <td class="fields" style="width: 67px; height: 21px">
                                    <asp:Label ID="Label7" runat="server" Style="vertical-align: middle" Text="Status"
                                        Width="78px"></asp:Label></td>
                                <td class="fields" style="width: 12px; height: 21px">
                                    <asp:DropDownList ID="ddlStatus" runat="server" Width="146px">
                                        <asp:ListItem Value="1">Active</asp:ListItem>
                                        <asp:ListItem Value="0">Inactive</asp:ListItem>
                                    </asp:DropDownList></td>
                                <td style="width: 218px; height: 21px"></td>
                                <td style="width: 100px; height: 21px"></td>
                            </tr>
                        </table>
                    </div>
                </td>
            </tr>
        </table>
    </asp:Panel>

    <asp:Panel ID="pnlView" runat="server" Height="100%" Width="100%">
        <table style="width: 100%; height: 100%;">
            <tr>
                <td style="width: 3px"></td>
                <td class="fields" style="width: 100%">&nbsp;<asp:DataGrid ID="dgView" runat="server" Width="100%" AutoGenerateColumns="False" DataKeyField="SABD_Code">
                    <FooterStyle CssClass="dgFooterStyle" Height="20px" />
                    <SelectedItemStyle CssClass="dgSelectedItemStyle" />
                    <AlternatingItemStyle BackColor="Beige" CssClass="dgAlternatingItemStyle" Font-Bold="False"
                        Font-Italic="False" Font-Overline="False" Font-Strikeout="False" Font-Underline="False" />
                    <ItemStyle CssClass="dgItemStyle" />
                    <HeaderStyle BackColor="#CDD7EE" CssClass="dgHeaderStyle" Font-Bold="True" Font-Italic="False"
                        Font-Overline="False" Font-Size="Medium" Font-Strikeout="False" Font-Underline="False" />
                    <Columns>
                        <asp:ButtonColumn CommandName="Select" DataTextField="SABD_Code" HeaderText="Bank Code"
                            Text="SABD_Code"></asp:ButtonColumn>
                        <asp:BoundColumn DataField="SABD_Desc" HeaderText="Bank Name"></asp:BoundColumn>
                        <asp:BoundColumn DataField="SABD_ACCode" HeaderText="Account Code"></asp:BoundColumn>
                        <asp:BoundColumn DataField="SABD_GLCode" HeaderText="GLAccount"></asp:BoundColumn>
                    </Columns>
                </asp:DataGrid></td>
                <td style="width: 100px"></td>
            </tr>
        </table>
    </asp:Panel>
</asp:Content>

