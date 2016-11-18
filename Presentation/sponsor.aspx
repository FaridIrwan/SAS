<%@ Page Language="VB" MasterPageFile="~/MasterPage3.master" AutoEventWireup="false" MaintainScrollPositionOnPostback="true"
    CodeFile="Sponsor.aspx.vb" Inherits="Sponsor" Title="Welcome To SAS" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <script language="javascript" type="Scripts/functions"></script>
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
        function validate() {
            var re = /\s*((\S+\s*)*)/;
            if (document.getElementById("<%=txtsponsorCode.ClientID%>").value.replace(re, "$1").length == 0) {
                alert("Sponsor Code Field Cannot Be Blank");
                document.getElementById("<%=txtsponsorCode.ClientID%>").focus();
                return false;
            }
            var digits = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            var temp;
            for (var i = 0; i < document.getElementById("<%=txtsponsorCode.ClientID %>").value.length; i++) {
                temp = document.getElementById("<%=txtsponsorCode.ClientID%>").value.substring(i, i + 1);
                if (digits.indexOf(temp) == -1) {
                    alert("Enter Valid Sponsor Code");
                    document.getElementById("<%=txtsponsorCode.ClientID%>").focus();
                    return false;
                }
            }
            if (document.getElementById("<%=txtsponsorName.ClientID%>").value.replace(re, "$1").length == 0) {
                alert("Sponsor Name Field Cannot Be Blank");
                document.getElementById("<%=txtsponsorName.ClientID%>").focus();
                return false;
            }
            if (document.getElementById("<%=txtsponsorType.ClientID%>").value.replace(re, "$1").length == 0) {
                alert("Sponsor Type Field Cannot Be Blank");
                document.getElementById("<%=txtsponsorType.ClientID%>").focus();
                return false;
            }
            if (document.getElementById("<%=txtGlaccount.ClientID%>").value.replace(re, "$1").length == 0) {
                alert("GL Account Field Cannot Be Blank");
                document.getElementById("<%=txtGlaccount.ClientID%>").focus();
                return false;
            }
            return true;
        }
        function getconfirm() {
            if (document.getElementById("<%=txtSponsorCode.ClientID%>").value == "") {

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
        function isNumberKey(evt) {
            var charCode = (evt.which) ? evt.which : event.keyCode

            if (charCode > 31 && (charCode < 48 || charCode > 57)) {
                alert("Enter Only Digits");
                evt.preventDefault();
                return false;
            }
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
                <%-- Editted By Zoya @25/02/2016--%>

                 <%-- Editted By Zoya @24/02/2016--%>
                <td style="float: left;display:None; visibility:collapse;">
                    <table style="border-collapse: collapse;" class="menuoff" onmouseover="className='menuon';"
                        onmouseout="className='menuoff';">
                        <tr>
                            <td>
                                <asp:ImageButton ID="ibtnPosting" runat="server" ImageUrl="~/images/gposting.png"
                                    ToolTip="Cancel" Visible="false"/>
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
                                <asp:ImageButton ID="ibtnOthers" runat="server" ImageUrl="~/images/gothers.png" ToolTip="Cancel" Visible="false"/>
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
                <%-- Done Editted By Zoya @24/02/2016--%>

                <td style="width: 2%; height: 14px">
                    <asp:ImageButton ID="ibtnFirst" runat="server" ImageUrl="~/images/new_last.png" />
                </td>
                <td style="width: 3%; height: 14px">
                    <asp:ImageButton ID="ibtnPrevs" runat="server" ImageUrl="~/images/new_prev.png" />
                </td>
                <td style="width: 2%; height: 14px">
                    <asp:TextBox ID="txtRecNo" runat="server" Width="52px" disabled="disabled" TabIndex="1"
                        dir="ltr" AutoPostBack="True" Style="text-align: right" MaxLength="7" ReadOnly="true"
                        CssClass="text_box"></asp:TextBox>
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
    <%--  </ContentTemplate>     
</atlas:UpdatePanel>
<atlas:UpdateProgress ID="ProgressIndicator" runat="server">
    <ProgressTemplate>
        Loading the data, please wait... 
        <asp:Image ID="LoadingImage"  runat="server" ImageUrl="~/Images/spinner.gif" />        
    </ProgressTemplate>
 </atlas:UpdateProgress>
<atlas:UpdatePanel ID="up2" runat="server">
<ContentTemplate>--%>
    <asp:Panel ID="PnlAdd" runat="server" Width="100%" Visible="true">
        <table style="width: 100%">
            <tr>
                <td style="width: 100%">
                   <div  style="border: thin solid #A6D9F4; width: 100%">
                        <table class="fields" style="width: 100%; height: 100%;">
                            <tr>
                                <td style="width: 3px; height: 22px">
                                </td>
                                <td class="fields" style="width: 146px; height: 22px">
                                </td>
                                <td style="width: 110px; height: 22px; text-align: center">
                                    <asp:Label ID="lblMsg" runat="server" CssClass="lblError" Style="text-align: center"
                                        Visible="False" Width="312px"></asp:Label>
                                </td>
                                <td style="width: 248px; height: 22px">
                                </td>
                                <td style="width: 110px; height: 22px">
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 3px; height: 16px">
                                </td>
                                <td class="fields" style="width: 146px; height: 16px">
                                    <span
                                        style="font-size: 11pt; color: #ff0066">*</span>
                                    <asp:Label ID="Label1" runat="server" Text="Sponsor Code" Width="101px"></asp:Label>
                                </td>
                                <td style="width: 110px; height: 16px">
                                    <asp:TextBox ID="txtSponsorCode" runat="server" Width="142px" MaxLength="20"></asp:TextBox>
                                </td>
                                <td style="width: 248px; height: 16px">
                                </td>
                                <td style="width: 110px; height: 16px">
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 3px; height: 21px">
                                </td>
                                <td class="fields" style="width: 146px; height: 21px">
                                    <span
                                        style="font-size: 11pt; color: #ff0066">*</span>
                                    <asp:Label ID="Label2" runat="server" Text="Sponsor Name" Width="132px"></asp:Label>
                                </td>
                                <td style="width: 110px; height: 21px">
                                    <asp:TextBox ID="txtSponsorName" runat="server" Width="346px" MaxLength="50"></asp:TextBox>
                                </td>
                                <td style="width: 248px; height: 21px">
                                    <span style="font-size: 11pt; color: #ff0066"></span>
                                </td>
                                <td style="width: 110px; height: 21px">
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 3px; height: 21px">
                                </td>
                                <td class="fields" style="width: 146px; height: 21px">
                                    <asp:Label ID="Label7" runat="server" Text="Short Name" Width="109px"></asp:Label>
                                </td>
                                <td style="width: 110px; height: 21px">
                                    <asp:TextBox ID="txtSponsorSName" runat="server" Width="142px" MaxLength="20"></asp:TextBox>
                                </td>
                                <td style="width: 248px; height: 21px">
                                </td>
                                <td style="width: 110px; height: 21px">
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 3px; height: 21px">
                                </td>
                                <td class="fields" style="width: 146px; height: 21px">
                                    <span
                                        style="font-size: 11pt; color: #ff0066">*</span>
                                    <asp:Label ID="Label4" runat="server" Text="Sponsor Type" Width="93px"></asp:Label>
                                </td>
                                <td style="width: 110px; height: 21px">
                                    <asp:TextBox ID="txtSponsorType" runat="server" MaxLength="50" Width="346px"></asp:TextBox>
                                </td>
                                <td style="width: 248px; height: 21px">
                                    <span style="font-size: 11pt; color: #ff0066"></span>
                                </td>
                                <td style="width: 110px; height: 21px">
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 3px; height: 24px">
                                </td>
                                <td class="fields" style="width: 146px; height: 24px">
                                    <span
                                        style="font-size: 11pt; color: #ff0066">*</span>
                                    <asp:Label ID="Label3" runat="server" Text="GL Account" Width="78px"></asp:Label>
                                </td>
                                <td style="width: 110px; height: 24px">
                                    <asp:TextBox ID="txtGlAccount" runat="server" MaxLength="20" Width="142px" CausesValidation="True"></asp:TextBox>
                                    <asp:Button ID="Check" runat="server" Text="Check" />
                                    &nbsp;
                                    <br />
                                    <asp:Label ID="lblDesc" runat="server"></asp:Label>
                                    <br />
                                    <asp:Image ID="imgGL" runat="server" Height="18px" ImageAlign="Baseline" ImageUrl="~/images/check.png"
                                        Visible="False" />
                                    &nbsp;
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 3px; height: 21px">
                                </td>
                                <td class="fields" style="width: 146px; height: 21px">
                                    <asp:Label ID="Label8" runat="server" Text="Sponsor Remarks" Width="125px"></asp:Label>
                                </td>
                                <td style="width: 110px; height: 21px">
                                    <asp:TextBox ID="txtSponsorDesc" runat="server" Width="346px" MaxLength="50"></asp:TextBox>
                                </td>
                                <td style="width: 248px; height: 21px">
                                </td>
                                <td style="width: 110px; height: 21px">
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 3px; height: 21px">
                                </td>
                                <td class="fields" style="width: 146px; height: 21px">
                                    <asp:Label ID="Label9" runat="server" Text="Sponsor Contact" Width="109px"></asp:Label>
                                </td>
                                <td style="width: 110px; height: 21px">
                                    <asp:TextBox ID="txtSContact" runat="server" Width="346px" MaxLength="50"></asp:TextBox>
                                </td>
                                <td style="width: 248px; height: 21px">
                                </td>
                                <td style="width: 110px; height: 21px">
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 3px; height: 21px">
                                </td>
                                <td class="fields" style="width: 146px; height: 21px">
                                    <asp:Label ID="Label10" runat="server" Text="Sponsor Address " Width="112px"></asp:Label>
                                </td>
                                <td style="width: 110px; height: 21px">
                                    <asp:TextBox ID="txtAddress" runat="server" Width="346px" MaxLength="50"></asp:TextBox>
                                </td>
                                <td style="width: 248px; height: 21px">
                                    <span style="font-size: 11pt; color: #ff0066"></span>
                                </td>
                                <td style="width: 110px; height: 21px">
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 3px; height: 21px">
                                </td>
                                <td class="fields" style="width: 146px; height: 21px">
                                </td>
                                <td style="width: 110px; height: 21px">
                                    <asp:TextBox ID="txtAddress1" runat="server" Width="346px" MaxLength="50"></asp:TextBox>
                                </td>
                                <td style="width: 248px; height: 21px">
                                </td>
                                <td style="width: 110px; height: 21px">
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 3px; height: 21px">
                                </td>
                                <td class="fields" style="width: 146px; height: 21px">
                                </td>
                                <td style="width: 110px; height: 21px">
                                    <asp:TextBox ID="txtAddress2" runat="server" Width="346px" MaxLength="50"></asp:TextBox>
                                </td>
                                <td style="width: 248px; height: 21px">
                                </td>
                                <td style="width: 110px; height: 21px">
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 3px; height: 21px">
                                </td>
                                <td class="fields" style="width: 146px; height: 21px">
                                    <asp:Label ID="Label12" runat="server" Text="Sponsor Phone" Width="112px"></asp:Label>
                                </td>
                                <td style="width: 110px; height: 21px">
                                    <asp:TextBox ID="txtPhone" runat="server" Width="114px" MaxLength="20"></asp:TextBox>
                                </td>
                                <td style="width: 248px; height: 21px">
                                </td>
                                <td style="width: 110px; height: 21px">
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 3px; height: 21px">
                                </td>
                                <td class="fields" style="width: 146px; height: 21px">
                                    <asp:Label ID="Label20" runat="server" Text="Sponsor Fax" Width="112px"></asp:Label>
                                </td>
                                <td style="width: 110px; height: 21px">
                                    <asp:TextBox ID="txtFax" runat="server" Width="114px" MaxLength="20"></asp:TextBox>
                                </td>
                                <td style="width: 248px; height: 21px">
                                </td>
                                <td style="width: 110px; height: 21px">
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 3px; height: 20px">
                                </td>
                                <td class="fields" style="width: 146px; height: 20px">
                                    <asp:Label ID="Label21" runat="server" Text="Sponsor Email" Width="112px"></asp:Label>
                                </td>
                                <td style="width: 110px; height: 20px">
                                    <asp:TextBox ID="txtEmail" runat="server" Width="346px" MaxLength="50"></asp:TextBox>
                                </td>
                                <td style="width: 248px; height: 20px">
                                </td>
                                <td style="width: 110px; height: 20px">
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 3px; height: 21px">
                                </td>
                                <td class="fields" style="width: 146px; height: 21px">
                                    <asp:Label ID="Label22" runat="server" Text="Sponsor Website" Width="112px"></asp:Label>
                                </td>
                                <td style="width: 110px; height: 21px">
                                    <asp:TextBox ID="txtWebsite" runat="server" Width="346px" MaxLength="50"></asp:TextBox>
                                </td>
                                <td style="width: 248px; height: 21px">
                                </td>
                                <td style="width: 110px; height: 21px">
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 3px; height: 21px">
                                </td>
                                <td class="fields" style="vertical-align: top; width: 146px; height: 21px">
                                    <asp:Label ID="Label15" runat="server" Style="vertical-align: middle" Text="Status"
                                        Width="78px"></asp:Label>
                                </td>
                                <td class="fields" style="width: 110px; height: 21px">
                                    <asp:DropDownList ID="ddlStatus" runat="server" Width="146px">
                                        <asp:ListItem Value="1">Active</asp:ListItem>
                                        <asp:ListItem Value="0">Inactive</asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                                <td style="width: 248px; height: 21px">
                                </td>
                                <td style="width: 110px; height: 21px">
                                </td>
                            </tr>
                            <%-- added by Hafiz @ 22/3/2016 --%>
                            <tr>
                                <td style="width: 3px; height: 21px">
                                </td>
                                <td class="fields" style="vertical-align: top; width: 146px; height: 21px">
                                    <asp:Label ID="lblPTPTN" runat="server" Style="vertical-align: middle" Text="PTPTN"
                                        Width="78px"></asp:Label>
                                </td>
                                <td class="fields" style="width: 110px; height: 21px">
                                    <asp:DropDownList ID="ddlPTPTN" runat="server" Width="146px">
                                        <asp:ListItem Value="1">Yes</asp:ListItem>
                                        <asp:ListItem Value="0" Selected="True">No</asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                                <td style="width: 248px; height: 21px">
                                </td>
                                <td style="width: 110px; height: 21px">
                                </td>
                            </tr>
                            <%-- added by Hafiz @ 22/3/2016 end --%>
                            <tr>
                                <td style="width: 3px; height: 21px">
                                </td>
                                <td class="fields" style="vertical-align: top; width: 146px; height: 21px">
                                    <asp:Label ID="Label19" runat="server" Font-Bold="False" Style="vertical-align: top"
                                        Text="Fee Type" Width="113px"></asp:Label>
                                </td>
                                <td class="fields" style="width: 110px; height: 21px">
                                    <asp:ListBox ID="lstStudentFeetype1" runat="server" Height="121px" Width="148px">
                                    </asp:ListBox>
                                    <asp:CheckBox ID="chkSpnFeetypes1" runat="server" AutoPostBack="True" Height="22px"
                                        OnCheckedChanged="chkSpnFeetypes1_CheckedChanged" Width="85px" Visible="False" />
                                    <asp:ImageButton ID="ibtn_spn1_feetype" runat="server" Height="16px" ImageUrl="~/images/find_img.png"
                                        Width="16px"/>
                                </td>
                                <td style="width: 248px; height: 21px">
                                </td>
                                <td style="width: 110px; height: 21px">
                                </td>
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
                <td style="width: 1px; height: 193px;">
                </td>
                <td class="fields" style="width: 100%; height: 193px;" valign="top">
                    &nbsp;<asp:DataGrid ID="dgView" runat="server" AllowSorting="True" AutoGenerateColumns="False"
                        Font-Names="verdana" Font-Size="8pt" Height="17px" Width="100%" DataKeyField="SASR_Code">
                        <FooterStyle CssClass="dgFooterStyle" Height="20px" />
                        <SelectedItemStyle CssClass="dgSelectedItemStyle" BackColor="Gray" Font-Bold="False"
                            Font-Italic="False" Font-Overline="False" Font-Strikeout="False" Font-Underline="False" />
                        <AlternatingItemStyle BackColor="Beige" CssClass="dgAlternatingItemStyle" Font-Bold="False"
                            Font-Italic="False" Font-Overline="False" Font-Strikeout="False" Font-Underline="False" />
                        <ItemStyle CssClass="dgItemStyle" />
                        <HeaderStyle BackColor="#CDD7EE" CssClass="dgHeaderStyle" Font-Bold="True" Font-Italic="False"
                            Font-Overline="False" Font-Size="Medium" Font-Strikeout="False" Font-Underline="False" />
                        <Columns>
                            <asp:ButtonColumn CommandName="Select" HeaderText="Sponsor Code" DataTextField="SASR_Code"
                                Text="SASR_Code"></asp:ButtonColumn>
                            <asp:BoundColumn DataField="SASR_Name" HeaderText="Sponsor Name">
                                <HeaderStyle Font-Bold="True" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                    Font-Underline="False" />
                            </asp:BoundColumn>
                            <asp:BoundColumn DataField="SASR_Type" HeaderText="Sponsor Type"></asp:BoundColumn>
                            <asp:BoundColumn DataField="SASR_Desc" HeaderText="Sponsor Description"></asp:BoundColumn>
                            <asp:BoundColumn DataField="SASR_GLAccount" HeaderText="GL Account Code"></asp:BoundColumn>
                            <asp:BoundColumn DataField="SASSR_SName" HeaderText="SASSR_SName" Visible="False">
                            </asp:BoundColumn>
                            <asp:BoundColumn DataField="SASR_Address" HeaderText="SASR_Address" Visible="False">
                            </asp:BoundColumn>
                            <asp:BoundColumn DataField="SASR_Address1" HeaderText="SASR_Address1" Visible="False">
                            </asp:BoundColumn>
                            <asp:BoundColumn DataField="SASR_Address2" HeaderText="SASR_Address2" Visible="False">
                            </asp:BoundColumn>
                            <asp:BoundColumn DataField="SASR_Contact" HeaderText="SASR_Contact" Visible="False">
                            </asp:BoundColumn>
                            <asp:BoundColumn DataField="SASR_Phone" HeaderText="SASR_Phone" Visible="False">
                            </asp:BoundColumn>
                            <asp:BoundColumn DataField="SASR_Fax" HeaderText="SASR_Fax" Visible="False"></asp:BoundColumn>
                            <asp:BoundColumn DataField="SASR_Email" HeaderText="SASR_Email" Visible="False">
                            </asp:BoundColumn>
                            <asp:BoundColumn DataField="SASR_WebSite" HeaderText="SASR_WebSite" Visible="False">
                            </asp:BoundColumn>
                        </Columns>
                    </asp:DataGrid>
                </td>
            </tr>
        </table>
    </asp:Panel>
     <asp:Button ID="btnHidden" runat="Server" OnClick="btnHidden_Click"  style="display:none" />
    <%-- </ContentTemplate>     
</atlas:UpdatePanel>--%>
</asp:Content>
