<%@ Page Language="VB" MasterPageFile="~/MasterPage3.master" AutoEventWireup="false"
    CodeFile="RptSponsorCoverLetter.aspx.vb" Inherits="RptSponsorCoverLetter" Title="Welcome To SAS" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <script language="javascript" type="text/javascript">
        function getibtnFDate() {
            popUpCalendar(document.getElementById("<%=ibtnFdate.ClientID%>"), document.getElementById("<%=txtFrom.ClientID%>"), 'dd/mm/yyyy')
        }
        function getDateto() {
            popUpCalendar(document.getElementById("<%=ibtnTodate.ClientID%>"), document.getElementById("<%=txtToDate.ClientID%>"), 'dd/mm/yyyy')
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
        function getconfirm() {
            var re = /\s*((\S+\s*)*)/;
            if (document.getElementById("<%=txtCode.ClientID%>").value.replace(re, "$1").length == 0) {

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
        function CheckFromDate() {
            var digits = "0123456789/";
            var temp;
            for (var i = 0; i < document.getElementById("<%=txtFrom.ClientID %>").value.length; i++) {
                temp = document.getElementById("<%=txtFrom.ClientID%>").value.substring(i, i + 1);
                if (digits.indexOf(temp) == -1) {
                    alert("Enter Valid Date (dd/mm/yyyy)");
                    document.getElementById("<%=txtFrom.ClientID%>").value = "";
                    document.getElementById("<%=txtFrom.ClientID%>").focus();
                    return false;
                }
            }
            return true;
        }
        function CheckToDate() {
            var digits = "0123456789/";
            var temp;
            for (var i = 0; i < document.getElementById("<%=txtToDate.ClientID %>").value.length; i++) {
                temp = document.getElementById("<%=txtToDate.ClientID%>").value.substring(i, i + 1);
                if (digits.indexOf(temp) == -1) {
                    alert("Enter Valid Date (dd/mm/yyyy)");
                    document.getElementById("<%=txtToDate.ClientID%>").value = "";
                    document.getElementById("<%=txtToDate.ClientID%>").focus();
                    return false;
                }
            }
            return true;
        }
        function validate() {
            var re = /\s*((\S+\s*)*)/;
            if (document.getElementById("<%=txtCode.ClientID%>").value.replace(re, "$1").length == 0) {
                alert("Code Field Cannot Be Blank");
                document.getElementById("<%=txtCode.ClientID%>").focus();
                return false;
            }
            var digits = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            var temp;
            for (var i = 0; i < document.getElementById("<%=txtCode.ClientID %>").value.length; i++) {
                temp = document.getElementById("<%=txtCode.ClientID%>").value.substring(i, i + 1);
                if (digits.indexOf(temp) == -1) {
                    alert("Enter Valid Code");
                    document.getElementById("<%=txtCode.ClientID%>").focus();
                    return false;
                }
            }
            if (document.getElementById("<%=txtTitle.ClientID%>").value.replace(re, "$1").length == 0) {
                alert("Title Field Cannot Be Blank");
                document.getElementById("<%=txtTitle.ClientID%>").focus();
                return false;
            }
            if (document.getElementById("<%=txtOurRef.ClientID%>").value.replace(re, "$1").length == 0) {
                alert("Our Ref Field Cannot Be Blank");
                document.getElementById("<%=txtOurRef.ClientID%>").focus();
                return false;
            }
            if (document.getElementById("<%=txtYourRef.ClientID%>").value.replace(re, "$1").length == 0) {
                alert("Your Ref Field Cannot Be Blank");
                document.getElementById("<%=txtYourRef.ClientID%>").focus();
                return false;
            }
            if (document.getElementById("<%=txtMsg.ClientID%>").value.replace(re, "$1").length == 0) {
                alert("Message Field Cannot Be Blank");
                document.getElementById("<%=txtMsg.ClientID%>").focus();
                return false;
            }
           <%-- if (document.getElementById("<%=txtAddress.ClientID%>").value.replace(re, "$1").length == 0) {
                alert("Address Field Cannot Be Blank");
                document.getElementById("<%=txtAddress.ClientID%>").focus();
                return false;
            } --%>
            if (document.getElementById("<%=txtSignBy.ClientID%>").value.replace(re, "$1").length == 0) {
                alert("Sign By Field Cannot Be Blank");
                document.getElementById("<%=txtSignBy.ClientID%>").focus();
                return false;
            }
            if (document.getElementById("<%=txtName.ClientID%>").value.replace(re, "$1").length == 0) {
                alert("Name Field Cannot Be Blank");
                document.getElementById("<%=txtName.ClientID%>").focus();
                return false;
            }
            //txtFrom---------------------------------------------------------------------------

            var len = document.getElementById("<%=txtFrom.ClientID%>").value
            var RegExPattern = /^((((0?[1-9]|[12]\d|3[01])[\.\-\/](0?[13578]|1[02])[\.\-\/]((1[6-9]|[2-9]\d)?\d{2}))|((0?[1-9]|[12]\d|30)[\.\-\/](0?[13456789]|1[012])[\.\-\/]((1[6-9]|[2-9]\d)?\d{2}))|((0?[1-9]|1\d|2[0-8])[\.\-\/]0?2[\.\-\/]((1[6-9]|[2-9]\d)?\d{2}))|(29[\.\-\/]0?2[\.\-\/]((1[6-9]|[2-9]\d)?(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00)|00)))|(((0[1-9]|[12]\d|3[01])(0[13578]|1[02])((1[6-9]|[2-9]\d)?\d{2}))|((0[1-9]|[12]\d|30)(0[13456789]|1[012])((1[6-9]|[2-9]\d)?\d{2}))|((0[1-9]|1\d|2[0-8])02((1[6-9]|[2-9]\d)?\d{2}))|(2902((1[6-9]|[2-9]\d)?(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00)|00))))$/;
            var errorMessage = 'Enter Valid Date (dd/mm/yyyy)';

            if (document.getElementById("<%=txtFrom.ClientID%>").value.match(RegExPattern)) {
                if (len.length == 8) {

                    alert(errorMessage);
                    document.getElementById("<%=txtFrom.ClientID%>").value = "";
                    document.getElementById("<%=txtFrom.ClientID%>").focus();

                    return false;
                }
            }
            else {

                alert(errorMessage);
                document.getElementById("<%=txtFrom.ClientID%>").value = "";
                document.getElementById("<%=txtFrom.ClientID%>").focus();
                return false;
            }


            //txtTodate---------------------------------------------------------------------------
            var len = document.getElementById("<%=txtTodate.ClientID%>").value
            var RegExPattern = /^((((0?[1-9]|[12]\d|3[01])[\.\-\/](0?[13578]|1[02])[\.\-\/]((1[6-9]|[2-9]\d)?\d{2}))|((0?[1-9]|[12]\d|30)[\.\-\/](0?[13456789]|1[012])[\.\-\/]((1[6-9]|[2-9]\d)?\d{2}))|((0?[1-9]|1\d|2[0-8])[\.\-\/]0?2[\.\-\/]((1[6-9]|[2-9]\d)?\d{2}))|(29[\.\-\/]0?2[\.\-\/]((1[6-9]|[2-9]\d)?(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00)|00)))|(((0[1-9]|[12]\d|3[01])(0[13578]|1[02])((1[6-9]|[2-9]\d)?\d{2}))|((0[1-9]|[12]\d|30)(0[13456789]|1[012])((1[6-9]|[2-9]\d)?\d{2}))|((0[1-9]|1\d|2[0-8])02((1[6-9]|[2-9]\d)?\d{2}))|(2902((1[6-9]|[2-9]\d)?(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00)|00))))$/;
            var errorMessage = 'Enter Valid Date (dd/mm/yyyy)';

            if (document.getElementById("<%=txtTodate.ClientID%>").value.match(RegExPattern)) {
                if (len.length == 8) {
                    alert(errorMessage);
                    document.getElementById("<%=txtTodate.ClientID%>").value = "";
                    document.getElementById("<%=txtTodate.ClientID%>").focus();
                    return false;
                }
            }
            else {
                alert(errorMessage);
                document.getElementById("<%=txtTodate.ClientID%>").value = "";
                document.getElementById("<%=txtTodate.ClientID%>").focus();
                return false;
            }

            return true;
        }
    </script>

    <asp:Panel ID="Panel1" runat="server" Width="100%">
        <table style="background-image: url(images/Sample.png);">
            <tr>
                <td style="width: 4px; height: 14px"></td>
                <td style="width: 14px; height: 14px"></td>
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
                <td>
                    <table style="border-collapse: collapse;" class="menuoff" onmouseover="className='menuon';"
                        onmouseout="className='menuoff';">
                        <tr>
                            <td style="width: 3%; height: 14px">
                                <asp:ImageButton ID="ibtnSave" runat="server" ImageUrl="~/images/save.png" ToolTip="Save" />
                            </td>
                            <td style="width: 3%; height: 14px">
                                <asp:Label ID="Label14" runat="server" Text="Save"></asp:Label>
                            </td>
                        </tr>
                    </table>
                </td>
                <td>
                    <table style="border-collapse: collapse;" class="menuoff" onmouseover="className='menuon';"
                        onmouseout="className='menuoff';">
                        <tr>
                            <td style="height: 14px; width: 3%;">
                                <asp:ImageButton ID="ibtnDelete" runat="server" ImageUrl="~/images/delete.png" />
                            </td>
                            <td style="width: 3%; height: 14px">
                                <asp:Label ID="Label13" runat="server" Text="Delete"></asp:Label>
                            </td>
                        </tr>
                    </table>
                </td>
                <td>
                    <table style="border-collapse: collapse;" class="menuoff" onmouseover="className='menuon';"
                        onmouseout="className='menuoff';">
                        <tr>
                            <td style="width: 3%; height: 14px">
                                <asp:ImageButton ID="ibtnView" runat="server" ImageUrl="~/images/find.png" ToolTip="View" />
                            </td>
                            <td style="width: 3%; height: 14px">
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
                            <td style="width: 3%; height: 14px">
                                <asp:ImageButton ID="ibtnPrint" runat="server" ImageUrl="~/images/print.png" ToolTip="Print" Visible="false"/>
                            </td>
                            <td style="width: 3%; height: 14px">
                                <asp:Label ID="Label17" runat="server" Text="Print" Visible="false"></asp:Label>
                            </td>
                        </tr>
                    </table>
                </td>
                <td style="float: left;display:None; visibility:collapse;">
                    <table style="border-collapse: collapse;" class="menuoff" onmouseover="className='menuon';"
                        onmouseout="className='menuoff';">
                        <tr>
                            <td style="width: 3%; height: 14px">
                                <asp:ImageButton ID="ibtnPosting" runat="server" ImageUrl="~/images/gposting.png"
                                    ToolTip="Cancel" Visible="false"/>
                            </td>
                            <td style="width: 3%; height: 14px">&nbsp;<asp:Label ID="Label6" runat="server" Text="Posting" Visible="false"></asp:Label>
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
                                <asp:ImageButton ID="ibtnCancel" runat="server" ImageUrl="~/images/cancel.png" ToolTip="Cancel"
                                    OnClick="ibtnCancel_Click1" />
                            </td>
                            <td style="width: 2%; height: 14px">
                                <asp:Label ID="Label18" runat="server" Text="Cancel"></asp:Label>
                            </td>
                        </tr>
                    </table>
                </td>
                <%-- Done Editted By Zoya @24/02/2016--%>
                <td style="width: 2%; height: 14px">
                    <asp:ImageButton ID="ibtnFirst" runat="server" ImageUrl="~/images/new_last.png" OnClick="ibtnFirst_Click" />
                </td>
                <td style="width: 2%; height: 14px">
                    <asp:ImageButton ID="ibtnPrevs" runat="server" ImageUrl="~/images/new_prev.png" OnClick="ibtnPrevs_Click" />
                </td>
                <td style="width: 2%; height: 14px">
                    <asp:TextBox ID="txtRecNo" runat="server" ReadOnly="true" CssClass="text_box" Width="52px"
                        AutoPostBack="True" Style="text-align: right" disabled="disabled" TabIndex="1"
                        dir="ltr"></asp:TextBox>
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
        <table class="mainbg" width="100%">
            <tr>
                <td width="50%">
                    <asp:SiteMapPath ID="SiteMapPath1" runat="server" Width="240px">
                    </asp:SiteMapPath>
                </td>
                <td align="right" width="50%" class="pagetext">
                    <asp:Label ID="lblMenuName" runat="server" Text="University Fund"></asp:Label>
                </td>
            </tr>
        </table>
    </asp:Panel>

    <fieldset style="width: 99%; border: thin solid #A6D9F4;">
        <legend><strong><span style="color: #000000;">Selection Criteria</span></strong></legend>
        <table style="width: 100%">
            <tr>
                <td colspan="2" class="auto-style2"></td>
                <td colspan="2" style="height: 16px">
                    <asp:Label ID="lblMesg" runat="server" ForeColor="Red" Visible="False" CssClass=" lblError"></asp:Label>
                </td>
                <td style="width: 1650px; height: 16px"></td>
            </tr>
            <tr>
                <td colspan="2" class="auto-style2">
                    <span style="color: #ff0000">*</span>
                    <asp:Label ID="Label1" runat="server" Text="Code"></asp:Label>
                </td>
                <td colspan="2" style="height: 16px">
                    <asp:TextBox ID="txtCode" runat="server"></asp:TextBox>
                </td>
                <td style="width: 1650px; height: 16px"></td>
            </tr>
            <tr>
                <td colspan="2" class="auto-style2">
                    <span
                        style="color: #ff0000">*</span>
                    <asp:Label ID="Label2" runat="server" Text="Title"></asp:Label>
                </td>
                <td colspan="2" style="height: 16px">
                    <asp:TextBox ID="txtTitle" runat="server" Width="519px" MaxLength="50"></asp:TextBox>
                </td>
                <td style="width: 1650px; height: 16px">
                    <span style="color: #ff0000"></span>
                </td>
            </tr>
            <tr>
                <td colspan="2" class="auto-style2">
                    <span style="color: #ff0000">*</span>
                    <asp:Label ID="Label3" runat="server" Text="Our Ref"></asp:Label>
                </td>
                <td colspan="2" style="height: 16px">
                    <asp:TextBox ID="txtOurRef" runat="server"></asp:TextBox>
                </td>
                <td style="width: 1650px; height: 16px"></td>
            </tr>
            <tr>
                <td colspan="2" class="auto-style2">
                    <span style="color: #ff0000">*</span>
                    <asp:Label ID="Label12" runat="server" Text="Your Ref"></asp:Label>
                </td>
                <td colspan="2" style="height: 16px">
                    <asp:TextBox ID="txtYourRef" runat="server"></asp:TextBox>
                </td>
                <td style="width: 1650px; height: 16px"></td>
            </tr>
            <tr style="display: none">
                <td colspan="2" class="auto-style2">
                    <span style="color: #ff0000">*</span>
                    <asp:Label ID="Label4" runat="server" Text="Date"></asp:Label>
                </td>
                <td colspan="1" style="width: 71px; height: 16px">
                    <asp:TextBox ID="txtFrom" runat="server"></asp:TextBox>
                </td>
                <td colspan="1" style="width: 444px; height: 16px">
                    <asp:Image ID="ibtnFDate" runat="server" ImageUrl="~/images/cal.gif" />
                </td>
                <td colspan="2" style="height: 16px"></td>
                <td colspan="1" style="height: 16px"></td>
                <td style="width: 10px; height: 16px"></td>
            </tr>
            <tr style="display: none">
                <td colspan="2" style="vertical-align: top;" class="auto-style4">
                    <span style="color: #ff0000">*</span>
                    <asp:Label ID="Label15" runat="server" Text="Address"></asp:Label>
                </td>
                <td colspan="2" style="height: 88px">
                    <asp:TextBox ID="txtAddress" runat="server" EnableTheming="False" Height="108px"
                        TextMode="MultiLine" Width="519px"></asp:TextBox>
                </td>
                <td style="width: 1650px; height: 88px">
                    <span style="color: #ff0000"></span>
                </td>
            </tr>
            <tr>
                <td colspan="2" style="vertical-align: top;" class="auto-style4">
                    <span style="color: #ff0000">*</span>
                    <asp:Label ID="Label7" runat="server" Text="Message"></asp:Label>
                </td>
                <td colspan="2" style="height: 88px">
                    <asp:TextBox ID="txtMsg" runat="server" EnableTheming="False" Height="108px"
                        TextMode="MultiLine" Width="519px"></asp:TextBox>
                </td>
                <td style="width: 1650px; height: 88px">
                    <span style="color: #ff0000"></span>
                </td>
            </tr>
            <tr>
                <td colspan="2" class="auto-style2">
                    <span
                        style="color: #ff0000">*</span>
                    <asp:Label ID="Label8" runat="server" Text="Sign By"></asp:Label>
                </td>
                <td colspan="2" style="height: 16px">
                    <asp:TextBox ID="txtSignBy" runat="server" Width="519px" MaxLength="50"></asp:TextBox>
                </td>
                <td style="width: 1650px; height: 16px">
                    <span style="color: #ff0000"></span>
                </td>
            </tr>
            <tr>
                <td colspan="2" class="auto-style4" style="vertical-align: top;">
                    <span style="color: #ff0000">*</span>
                    <asp:Label ID="Label9" runat="server" Text="Name, Designation & Address"></asp:Label>
                </td>
                <td colspan="2" style="height: 88px">
                    <asp:TextBox ID="txtName" runat="server" EnableTheming="false" Height="108px" TextMode="MultiLine" Width="519px" MaxLength="1000"></asp:TextBox>
                </td>
                <td style="width: 1650px; height: 88px">
                    <span style="color: #ff0000"></span>
                </td>
            </tr>
            <tr style="display: none">
                <td colspan="2" class="auto-style2">
                    <span style="color: #ff0000">*</span>
                    <asp:Label ID="Label10" runat="server" Text="Date"></asp:Label>
                </td>
                <td colspan="1" style="height: 16px">
                    <asp:TextBox ID="txtTodate" runat="server"></asp:TextBox>
                </td>
                <td colspan="2" style="height: 16px">
                    <asp:Image ID="ibtnTodate" runat="server" ImageUrl="~/images/cal.gif" />
                </td>
                <td style="width: 1833px; height: 16px"></td>
            </tr>
        </table>
    </fieldset>

    <%-- </ContentTemplate>
    </atlas:UpdatePanel>--%>
</asp:Content>
<asp:Content ID="Content2" runat="server" ContentPlaceHolderID="head">
    <style type="text/css">
        .auto-style1
        {
            height: 24px;
        }

        .auto-style2
        {
            height: 16px;
            width: 162px;
        }

        .auto-style3
        {
            height: 24px;
            width: 162px;
        }

        .auto-style4
        {
            height: 88px;
            width: 162px;
        }
    </style>
</asp:Content>

