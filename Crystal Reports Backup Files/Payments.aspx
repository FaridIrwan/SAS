<%@ Page Language="VB" MasterPageFile="~/MasterPage3.master" AutoEventWireup="false" MaintainScrollPositionOnPostback="true"
    CodeFile="Payments.aspx.vb" Inherits="Payments" Title="Payments" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <script language="javascript" type="Scripts/popcalendar.js"></script>
    <script language="javascript" type="Scripts/functions.js"></script>
    <script language="javascript" type="text/javascript">

        function geterr() {
            var digits = "0123456789";
            var temp;
            for (var i = 0; i < document.getElementById("<%=txtRecNo.ClientID %>").value.length; i++) {
                temp = document.getElementById("<%=txtRecNo.ClientID%>").value.substring(i, i + 1);
                if (digits.indexOf(temp) == -1) {
                    alert("Please Enter Correct Record No");
                    document.getElementById("<%=txtRecNo.ClientID%>").value = 1;
                    document.getElementById("<%=txtRecNo.ClientID%>").focus();
                    return false;
                }
            }
            return true;
        }
        function CheckSearch() {
            if (document.getElementById("<%=ddlpaymentfor.ClientID%>").value == "-1") {
                alert("Select a Payment For to Search");
                document.getElementById("<%=ddlpaymentfor.ClientID%>").focus();
                return false;
            }
            return true;
        }
        function CheckBDate() {
            var digits = "0123456789/";
            var temp;
            for (var i = 0; i < document.getElementById("<%=txtBDate.ClientID %>").value.length; i++) {
                temp = document.getElementById("<%=txtBDate.ClientID%>").value.substring(i, i + 1);
                if (digits.indexOf(temp) == -1) {
                    alert("Enter Valid Date (dd/mm/yyyy)");
                    document.getElementById("<%=txtBDate.ClientID%>").value = "";
                    document.getElementById("<%=txtBDate.ClientID%>").focus();
                    return false;
                }
            }
            return true;
        }
        function CheckTdate() {
            var digits = "0123456789/";
            var temp;
            for (var i = 0; i < document.getElementById("<%=txtPaymentDate.ClientID %>").value.length; i++) {
                temp = document.getElementById("<%=txtPaymentDate.ClientID%>").value.substring(i, i + 1);
                if (digits.indexOf(temp) == -1) {
                    alert("Enter Valid Date (dd/mm/yyyy)");
                    document.getElementById("<%=txtPaymentDate.ClientID%>").value = "";
                    document.getElementById("<%=txtPaymentDate.ClientID%>").focus();
                    return false;
                }
            }
            return true;
        }
        function BDate() {
            popUpCalendar(document.getElementById("<%=ibtnBDate.ClientID%>"), document.getElementById("<%=txtBDate.ClientID%>"), 'dd/mm/yyyy')

        }
        function getpaymentDate() {
            popUpCalendar(document.getElementById("<%=ibtnPaymentDate.ClientID%>"), document.getElementById("<%=txtPaymentDate.ClientID%>"), 'dd/mm/yyyy')

        }
        function getpostconfirm() {
            if (document.getElementById("<%=lblStatus.ClientID%>").value == "New") {
                alert("Select a Record to Post");
                return false;
            }
            if (document.getElementById("<%=lblStatus.ClientID%>").value == "Posted") {
                alert("Record Already Posted");
                return false;
            }
            if (confirm("Posted Record Cannot Be Altered, Do You Want To Proceed?")) {
                return true;
            }
            else {
                return false;
            }

        }

        function getconfirm() {
            if (document.getElementById("<%=lblStatus.ClientID%>").value == "Posted") {
                alert("Posted Record Cannot be Deleted");
                return false;
            }
            if (document.getElementById("<%=lblStatus.ClientID%>").value == "New") {
                alert("Select a Record to Delete");
                return false;
            }

            if (document.getElementById("<%=txtBatchid.ClientID%>").value == "") {
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
        function checknValue() {

            if (((event.keyCode < 48) || (event.keyCode > 57)) && (event.keyCode != 13) && (event.keyCode != 46)) {
                alert("Enter Valid Amount");
                event.keyCode = 0;
            }
        }

        function Validate() {
            var re = /\s*((\S+\s*)*)/;
            if (document.getElementById("<%=lblStatus.ClientID%>").value == "Posted") {
                alert("Posted record cannot be edited.");
                return false;
            }
            if (document.getElementById("<%=ddlpaymentfor.ClientID%>").value == "-1") {
                alert("Select a Payment For");
                document.getElementById("<%=ddlpaymentfor.ClientID%>").focus();
                return false;
            }
            <%--if (document.getElementById("<%= txtBatchid.ClientID%>").value.replace(re, "$1").length == 0) {
                alert("Batch Id Field Cannot Be Blank");
                document.getElementById("<%= txtBatchid.ClientID%>").focus();
                return false;
            }--%>

            if (document.getElementById("<%=ddlPaymentMode.ClientID%>").value == "-1") {
                alert("Select a Payment Mode");
                document.getElementById("<%=ddlPaymentMode.ClientID%>").focus();
                return false;
            }
            if (document.getElementById("<%=ddlBankCode.ClientID%>").value == "-1") {
                alert("Select a Bank Code");
                document.getElementById("<%=ddlBankCode.ClientID%>").focus();
                return false;
            }
            if (document.getElementById("<%=txtDescri.ClientID%>").value.replace(re, "$1").length == 0) {
                alert("Description Field Cannot Be Blank");
                document.getElementById("<%=txtDescri.ClientID%>").focus();
                return false;
            }
            if (document.getElementById("<%=ddlpaymentfor.ClientID%>").value == "1") {
                if (document.getElementById("<%=txtAllCode.ClientID%>").value.replace(re, "$1").length == 0) {
                    alert("Allocation Sl No Field Cannot Be Blank");
                    document.getElementById("<%=txtAllCode.ClientID%>").focus();
                    return false;
                }
                if (document.getElementById("<%=txtRef1.ClientID%>").value.replace(re, "$1").length == 0) {
                    alert("Receipt No Field Cannot Be Blank");
                    document.getElementById("<%=txtRef1.ClientID%>").focus();
                    return false;
                }
            }

            if (document.getElementById("<%=txtBDate.ClientID%>").value.replace(re, "$1").length == 0) {
                alert("Batch Date Field Cannot Be Blank");
                document.getElementById("<%=txtBDate.ClientID%>").focus();
                return false;
            }

            var len = document.getElementById("<%=txtBdate.ClientID%>").value
            var RegExPattern = /^((((0?[1-9]|[12]\d|3[01])[\.\-\/](0?[13578]|1[02])[\.\-\/]((1[6-9]|[2-9]\d)?\d{2}))|((0?[1-9]|[12]\d|30)[\.\-\/](0?[13456789]|1[012])[\.\-\/]((1[6-9]|[2-9]\d)?\d{2}))|((0?[1-9]|1\d|2[0-8])[\.\-\/]0?2[\.\-\/]((1[6-9]|[2-9]\d)?\d{2}))|(29[\.\-\/]0?2[\.\-\/]((1[6-9]|[2-9]\d)?(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00)|00)))|(((0[1-9]|[12]\d|3[01])(0[13578]|1[02])((1[6-9]|[2-9]\d)?\d{2}))|((0[1-9]|[12]\d|30)(0[13456789]|1[012])((1[6-9]|[2-9]\d)?\d{2}))|((0[1-9]|1\d|2[0-8])02((1[6-9]|[2-9]\d)?\d{2}))|(2902((1[6-9]|[2-9]\d)?(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00)|00))))$/;
            var errorMessage = 'Enter Valid Batch date in dd/mm/yyyy format.';
            if (document.getElementById("<%=txtBdate.ClientID%>").value == "") {

                // return false;
            }
            else {
                if (document.getElementById("<%=txtBdate.ClientID%>").value.match(RegExPattern)) {
                    if (len.length == 8) {
                        alert(errorMessage);
                        document.getElementById("<%=txtBdate.ClientID%>").value = "";
                        document.getElementById("<%=txtBdate.ClientID%>").focus();
                        return false;
                    }
                }
                else {
                    alert(errorMessage);
                    document.getElementById("<%=txtBdate.ClientID%>").value = "";
                    document.getElementById("<%=txtBdate.ClientID%>").focus();
                    return false;
                }
            }
            var str1 = document.getElementById("<%=txtBdate.ClientID %>").value;
            var str2 = document.getElementById("<%=today.ClientID %>").value;
            var dt1 = parseInt(str1.substring(0, 2), 10);
            var mon1 = parseInt(str1.substring(3, 5), 10);
            var yr1 = parseInt(str1.substring(6, 10), 10);
            var dt2 = parseInt(str2.substring(0, 2), 10);
            var mon2 = parseInt(str2.substring(3, 5), 10);
            var yr2 = parseInt(str2.substring(6, 10), 10);
            var date1 = new Date(yr1, mon1, dt1);
            var date2 = new Date(yr2, mon2, dt2);




            if (date2 < date1) {
                alert("Batch Date Cannot be Greater than Current Date");
                document.getElementById("<%=txtBdate.ClientID%>").value = "";
                document.getElementById("<%=txtBdate.ClientID%>").focus();
                //"");
                return false;
            }

            if (document.getElementById("<%=txtPaymentDate.ClientID%>").value == "") {
                alert("Transaction Date Field Cannot Be Blank");
                document.getElementById("<%=txtPaymentDate.ClientID%>").focus();
                return false;
            }


            var len = document.getElementById("<%=txtPaymentDate.ClientID%>").value
            var RegExPattern = /^((((0?[1-9]|[12]\d|3[01])[\.\-\/](0?[13578]|1[02])[\.\-\/]((1[6-9]|[2-9]\d)?\d{2}))|((0?[1-9]|[12]\d|30)[\.\-\/](0?[13456789]|1[012])[\.\-\/]((1[6-9]|[2-9]\d)?\d{2}))|((0?[1-9]|1\d|2[0-8])[\.\-\/]0?2[\.\-\/]((1[6-9]|[2-9]\d)?\d{2}))|(29[\.\-\/]0?2[\.\-\/]((1[6-9]|[2-9]\d)?(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00)|00)))|(((0[1-9]|[12]\d|3[01])(0[13578]|1[02])((1[6-9]|[2-9]\d)?\d{2}))|((0[1-9]|[12]\d|30)(0[13456789]|1[012])((1[6-9]|[2-9]\d)?\d{2}))|((0[1-9]|1\d|2[0-8])02((1[6-9]|[2-9]\d)?\d{2}))|(2902((1[6-9]|[2-9]\d)?(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00)|00))))$/;
            var errorMessage = 'Enter Valid Transaction Date in dd/mm/yyyy format.';
            if (document.getElementById("<%=txtPaymentDate.ClientID%>").value == "") {

                // return false;
            }
            else {
                if (document.getElementById("<%=txtPaymentDate.ClientID%>").value.match(RegExPattern)) {
                    if (len.length == 8) {
                        alert(errorMessage);
                        document.getElementById("<%=txtPaymentDate.ClientID%>").value = "";
                        document.getElementById("<%=txtPaymentDate.ClientID%>").focus();
                        return false;
                    }
                }
                else {
                    alert(errorMessage);
                    document.getElementById("<%=txtPaymentDate.ClientID%>").value = "";
                    document.getElementById("<%=txtPaymentDate.ClientID%>").focus();
                    return false;
                }
            }
            var str1 = document.getElementById("<%=txtPaymentDate.ClientID %>").value;
            var str2 = document.getElementById("<%=today.ClientID %>").value;
            var dt1 = parseInt(str1.substring(0, 2), 10);
            var mon1 = parseInt(str1.substring(3, 5), 10);
            var yr1 = parseInt(str1.substring(6, 10), 10);
            var dt2 = parseInt(str2.substring(0, 2), 10);
            var mon2 = parseInt(str2.substring(3, 5), 10);
            var yr2 = parseInt(str2.substring(6, 10), 10);
            var date1 = new Date(yr1, mon1, dt1);
            var date2 = new Date(yr2, mon2, dt2);




            if (date2 < date1) {
                alert("Transaction Date Cannot be Greater than Current Date");
                document.getElementById("<%=txtPaymentDate.ClientID%>").value = "";
                document.getElementById("<%=txtPaymentDate.ClientID%>").focus();
                //"");
                return false;
            }

            if (document.getElementById("<%=txtSpnCode.ClientID%>").value == "") {
                alert("Sponsor Code Field Cannot Be Blank");
                document.getElementById("<%=txtSpnCode.ClientID%>").focus();
                return false;
            }

            var str1 = document.getElementById("<%=txtPaymentDate.ClientID %>").value;
            var str2 = document.getElementById("<%=txtBdate.ClientID %>").value;
            var dt1 = parseInt(str1.substring(0, 2), 10);
            var mon1 = parseInt(str1.substring(3, 5), 10);
            var yr1 = parseInt(str1.substring(6, 10), 10);
            var dt2 = parseInt(str2.substring(0, 2), 10);
            var mon2 = parseInt(str2.substring(3, 5), 10);
            var yr2 = parseInt(str2.substring(6, 10), 10);
            var date1 = new Date(yr1, mon1, dt1);
            var date2 = new Date(yr2, mon2, dt2);




            if (date2 > date1) {
                alert("Transaction Date Cannot be Lesser than Batch Date");
                document.getElementById("<%=txtPaymentDate.ClientID%>").value = "";
                document.getElementById("<%=txtPaymentDate.ClientID%>").focus();
                //"");
                return false;
            }


            return true;
        }

        function getPrint() {

            var str = document.getElementById("<%=ddlpaymentfor.ClientID %>").value;
            window.open('GroupReport/RptPaymentViewer.aspx?mode=' + str, 'SAS', 'width=700,height=500,resizable=1'); new_window.focus();
        }
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
                            <td>
                                <asp:ImageButton ID="ibtnNew" runat="server" ImageUrl="~/images/add.png" ToolTip="New" />
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
                                <asp:ImageButton ID="ibtnDelete" runat="server" ImageUrl="~/images/delete.png" OnClick="ibtnDelete_Click" />
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
                            <td style="width: 3%; height: 14px">
                                <div id="wrap">
                                    <ul id="navbar">
                                        <li><a href="#">
                                            <img src="images/find.png" width="24" height="24" border="0" align="middle" />&nbsp;Search
                                            <img src="images/down.png" width="16" height="16" border="0" align="middle" />
                                        </a>
                                            <ul>
                                                <li><a href="#">
                                                    <asp:ImageButton ID="ibtnView" runat="server" ImageUrl="~/images/ready.png" /></a></li>
                                                <li><a href="#">
                                                    <asp:ImageButton ID="ibtnOthers" runat="server" ImageUrl="~/images/post.png" ToolTip="Cancel"
                                                        OnClick="ibtnOthers_Click" /></li>
                                            </ul>
                                        </li>
                                    </ul>
                                </div>
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
                                    OnClick="ibtnCancel_Click" />
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
                    <asp:TextBox ID="txtRecNo" runat="server" AutoPostBack="True" MaxLength="7" Style="text-align: right"
                        Width="52px" OnTextChanged="txtRecNo_TextChanged" ReadOnly="true" CssClass="text_box"
                        disabled="disabled" TabIndex="1" dir="ltr"></asp:TextBox>
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
                    <asp:Label ID="lblMenuName" runat="server" Width="422px"></asp:Label>
                </td>
            </tr>
        </table>
        <table>
            <tr>
                <td style="width: 100px">
                    <asp:Label ID="lblMsg" runat="server" CssClass="lblError" Style="text-align: center"
                        Width="301px"></asp:Label>
                </td>
            </tr>
        </table>
    </asp:Panel>

    <asp:Panel ID="Panel2" runat="server" Width="100%">
        <table style="width: 100%">
            <tr>
                <td style="width: 100%">
                    <div style="border: thin solid #A6D9F4; width: 100%">
                        <table width="500">
                            <tr>
                                <td style="width: 2%; height: 16px;">
                                </td>
                                <td style="width: 13%; height: 16px;">
                                </td>
                                <td colspan="5" style="text-align: left; height: 16px;">
                                </td>
                                <td colspan="2" style="height: 16px">
                                </td>
                                <td style="width: 378px; height: 16px;">
                                </td>
                                <td style="width: 125px; height: 16px;">
                                </td>
                                <td style="height: 16px">
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 2%; height: 17px;">
                                </td>
                                <td style="width: 13%; height: 17px;">
                                </td>
                                <td style="width: 156px; height: 17px;">
                                </td>
                                <td style="width: 15%; height: 17px;">
                                </td>
                                <td style="width: 11%; height: 17px;">
                                </td>
                                <td style="width: 3px; height: 17px;">
                                </td>
                                <td style="text-align: right; width: 138px; height: 17px;">
                                </td>
                                <td colspan="2" style="height: 17px">
                                </td>
                                <td style="width: 378px; height: 17px;">
                                </td>
                                <td style="width: 125px; height: 17px;">
                                </td>
                                <td style="height: 17px">
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 2%; height: 25px;">
                                </td>
                                <td style="width: 13%; height: 25px;">
                                    <asp:Label ID="Label3" runat="server" Text="Payment For" Width="97px"></asp:Label>
                                </td>
                                <td style="width: 156px; height: 25px;">
                                    <span style="color: #ff0000">
                                        <asp:DropDownList ID="ddlpaymentfor" runat="server" Width="149px" AutoPostBack="True"
                                            OnSelectedIndexChanged="ddlpaymentfor_SelectedIndexChanged">
                                            <asp:ListItem Value="-1">--Select--</asp:ListItem>
                                            <asp:ListItem Value="1">Allocation</asp:ListItem>
                                            <asp:ListItem Value="St">Refund</asp:ListItem>
                                        </asp:DropDownList>
                                        *</span>
                                </td>
                                <td style="width: 15%; height: 25px;">
                                </td>
                                <td style="width: 11%; height: 25px;">
                                </td>
                                <td style="height: 25px; width: 3px;">
                                </td>
                                <td style="text-align: left; height: 25px; width: 138px;">
                                    <asp:Label ID="Label4" runat="server" Style="text-align: left" Text="Batch Id" Width="97px"></asp:Label>
                                </td>
                                <td colspan="2" style="height: 25px">
                                    <span style="color: #ff0000">
                                        <asp:TextBox ID="txtBatchid" runat="server" MaxLength="20" Style="text-align: left"
                                            Width="103px" ReadOnly="True"></asp:TextBox>*</span>
                                </td>
                                <td style="width: 378px; height: 25px;">
                                </td>
                                <td style="width: 125px; height: 25px;">
                                    <asp:Label ID="Label12" runat="server" ForeColor="Black" Style="position: static"
                                        Text="Amount" Visible="False" Width="82px"></asp:Label>
                                </td>
                                <td style="height: 25px;">
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 2%; height: 27px;">
                                </td>
                                <td style="width: 13%; height: 27px;">
                                    <asp:Label ID="Label8" runat="server" Text="Payment Mode" Width="97px"></asp:Label>
                                </td>
                                <td style="width: 156px; height: 27px;">
                                    <span style="color: #ff0000">
                                        <asp:DropDownList ID="ddlPaymentMode" runat="server" AppendDataBoundItems="True"
                                            Width="149px">
                                            <asp:ListItem Value="-1">--Select--</asp:ListItem>
                                        </asp:DropDownList>
                                        *</span>
                                </td>
                                <td style="width: 15%; height: 27px">
                                </td>
                                <td style="width: 11%; height: 27px">
                                </td>
                                <td style="height: 27px; width: 3px;">
                                </td>
                                <td style="text-align: left; height: 27px; width: 138px;">
                                    <asp:Label ID="Label7" runat="server" Text="Batch Date  " Width="97px" Style="text-align: left"></asp:Label>
                                </td>
                                <td style="height: 27px;">
                                    <asp:TextBox ID="txtBDate" runat="server" MaxLength="10" Style="text-align: left" ReadOnly="true"
                                        Width="103px"></asp:TextBox><span style="color: #ff0000">*</span>
                                </td>
                                <td style="width: 67px; height: 27px;">
                                    <asp:Image ID="ibtnBDate" runat="server" ImageUrl="~/images/cal.gif" Visible="false" />
                                </td>
                                <td rowspan="3" style="width: 378px">
                                </td>
                                <td style="width: 125px" rowspan="3">
                                    <asp:ImageButton ID="ibtnStatus" runat="server" Enabled="False" ImageUrl="~/images/NotReady.gif"
                                        CssClass="cursor" />
                                </td>
                                <td rowspan="3">
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 2%; height: 16px">
                                </td>
                                <td style="width: 13%; height: 16px;">
                                    <asp:Label ID="Label10" runat="server" Text="Bank Code" Width="97px"></asp:Label>
                                </td>
                                <td style="width: 156px; height: 16px;">
                                    <span style="color: #ff0000">
                                        <asp:DropDownList ID="ddlBankCode" runat="server" AppendDataBoundItems="True" TabIndex="11"
                                            Width="149px">
                                            <asp:ListItem Value="-1">--Select--</asp:ListItem>
                                        </asp:DropDownList>
                                        *</span>
                                </td>
                                <td style="width: 15%; height: 16px">
                                </td>
                                <td style="width: 11%; height: 16px">
                                </td>
                                <td style="height: 16px; width: 3px;">
                                </td>
                                <td style="text-align: left; height: 16px; width: 138px;">
                                    <asp:Label ID="Label9" runat="server" Text="Transaction Date  " Width="97px" Style="text-align: left"></asp:Label>
                                </td>
                                <td style="height: 16px;">
                                    <asp:TextBox ID="txtPaymentDate" runat="server" MaxLength="10" Width="102px" ReadOnly="true"></asp:TextBox><span
                                        style="color: #ff0000">*</span>
                                </td>
                                <td style="height: 16px; width: 67px;">
                                    <asp:Image ID="ibtnPaymentDate" runat="server" ImageUrl="~/images/cal.gif" Visible="false" />
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 2%; height: 23px">
                                </td>
                                <td style="width: 13%; height: 23px;">
                                    <asp:Label ID="Label20" runat="server" Text="Description" Width="97px"></asp:Label>
                                </td>
                                <td colspan="3" style="height: 23px">
                                    <asp:TextBox ID="txtDescri" runat="server" MaxLength="50" Width="265px"></asp:TextBox><span
                                        style="color: #ff0000">*</span>
                                </td>
                                <td style="height: 23px; width: 3px;">
                                </td>
                                <td style="text-align: left; height: 23px; width: 138px;">
                                    <asp:Label ID="Label" runat="server" Text="Transaction Amount" Width="97px" Style="text-align: left"></asp:Label>
                                </td>
                                <td style="height: 23px;">
                                    <span style="color: #ff0000">
                                        <asp:TextBox ID="Total" runat="server" Style="text-align: right" MaxLength="20" ReadOnly="True"
                                            Width="102px">0.0</asp:TextBox>*</span>
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 2%; height: 25px">
                                </td>
                                <td style="width: 13%; height: 25px;">
                                    <asp:Label ID="Label15" runat="server" Text="Allocation Sl no." Visible="False" Width="97px"></asp:Label>
                                </td>
                                <td colspan="3" style="height: 25px">
                                    <span style="color: #ff0000">
                                        <asp:TextBox ID="txtAllCode" runat="server" MaxLength="20" Width="143px" ReadOnly="True"
                                            Visible="False"></asp:TextBox>
                                    <asp:Label ID="Label48" runat="server" Text="*" Visible="False" 
                                        Width="97px"></asp:Label>
                                    </span>
                                </td>
                                <td style="height: 25px; width: 3px;">
                                </td>
                                <td style="text-align: left; height: 25px; width: 138px;">
                                    <asp:Label ID="Type" runat="server" Text="Select Student" Visible="False"></asp:Label>
                                </td>
                                <td style="height: 25px;">
                                    <asp:Image ID="ibtnSpn1" runat="server" Height="16px" ImageUrl="~/images/find_img.png"
                                        Width="16px" Visible="False" />
                                    <asp:Image ID="ibtnstu" runat="server" Height="16px" ImageUrl="~/images/find_img.png"
                                        Width="16px" Visible="False" />
                                </td>
                                <td style="height: 25px; width: 67px;">
                                </td>
                                <td style="width: 378px; height: 25px">
                                </td>
                                <td style="width: 125px; height: 25px;">
                                </td>
                                <td style="height: 25px">
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 2%; height: 25px">
                                </td>
                                <td style="width: 13%; height: 25px">
                                    <asp:Label ID="Label1" runat="server" Text="Receipt No.  " Width="97px" Visible="False"></asp:Label>
                                </td>
                                <td style="width: 156px; height: 25px">
                                    <asp:TextBox ID="txtRef1" runat="server" MaxLength="20" Width="143px" Visible="False"
                                        ReadOnly="True"></asp:TextBox>
                                </td>
                                <td style="width: 15%; height: 25px">
                                </td>
                                <td style="width: 11%; height: 25px">
                                </td>
                                <td style="width: 3px; height: 25px">
                                </td>
                                <td style="width: 138px; height: 25px">
                                    <asp:Label ID="Label2" runat="server" Text="Sponsor  Code" Width="97px" Visible="False"></asp:Label>
                                </td>
                                <td style="height: 25px">
                                    <asp:TextBox ID="txtSpnCode" runat="server" MaxLength="20" Width="106px" Visible="False"
                                        ReadOnly="True"></asp:TextBox>
                                </td>
                                <td style="width: 67px; height: 25px">
                                </td>
                                <td style="width: 378px; height: 25px">
                                </td>
                                <td style="width: 125px; height: 25px">
                                    <asp:TextBox ID="txtBatch" runat="server" Visible="False" Width="99px"></asp:TextBox>
                                </td>
                                <td style="height: 25px">
                                </td>
                            </tr>
                            <tr runat="server" id="upload" visible="false">
                                <td style="width: 2%; height: 25px">
                                    &nbsp;</td>
                                <td style="width: 13%; height: 25px">
                                    <asp:Label ID="lblUpload" runat="server" Text="File Upload" Width="97px"></asp:Label>
                                </td>
                                <td style="width: 156px; height: 25px">
                                    <asp:Button ID="btnUpload" runat="server" Text="Upload" />
                                </td>
                                <td style="width: 15%; height: 25px">
                                    &nbsp;</td>
                                <td style="width: 11%; height: 25px">
                                    &nbsp;</td>
                                <td style="width: 3px; height: 25px">
                                    &nbsp;</td>
                                <td style="width: 138px; height: 25px">
                                    &nbsp;</td>
                                <td style="height: 25px">
                                    &nbsp;</td>
                                <td style="width: 67px; height: 25px">
                                    &nbsp;</td>
                                <td style="width: 378px; height: 25px">
                                    &nbsp;</td>
                                <td style="width: 125px; height: 25px">
                                    &nbsp;</td>
                                <td style="height: 25px">
                                    &nbsp;</td>
                            </tr>
                        </table>
                        <asp:Panel ID="pnlStudentGrid" runat="server" Width="100%">
                            <table width="100%">
                                <tr>
                                    <td style="height: 16px" width="1%">
                                    </td>
                                    <td style="width: 100%;">
                                        &nbsp;
                                        <asp:DataGrid ID="dgView" runat="server" AutoGenerateColumns="False" Width="100%"
                                            OnSelectedIndexChanged="dgView_SelectedIndexChanged">
                                            <FooterStyle CssClass="dgFooterStyle" Height="20px" />
                                            <SelectedItemStyle CssClass="dgSelectedItemStyle" />
                                            <AlternatingItemStyle BackColor="Beige" CssClass="dgAlternatingItemStyle" Font-Bold="False"
                                                Font-Italic="False" Font-Overline="False" Font-Strikeout="False" Font-Underline="False" />
                                            <ItemStyle CssClass="dgItemStyle" />
                                            <HeaderStyle BackColor="#CDD7EE" CssClass="dgHeaderStyle" Font-Bold="True" Font-Italic="False"
                                                Font-Overline="False" Font-Size="Medium" Font-Strikeout="False" Font-Underline="False" />
                                            <Columns>
                                                <asp:TemplateColumn HeaderText="Select" Visible="False">
                                                    <ItemTemplate>
                                                        <asp:CheckBox ID="chk" runat="server" OnCheckedChanged="chk_CheckedChanged" AutoPostBack="True" />
                                                    </ItemTemplate>
                                                </asp:TemplateColumn>
                                                <asp:BoundColumn DataField="MatricNO" HeaderText="Matric Id"></asp:BoundColumn>
                                                <asp:BoundColumn DataField="StudentName" HeaderText="Name"></asp:BoundColumn>
                                                <asp:BoundColumn DataField="ProgramId" HeaderText="Program"></asp:BoundColumn>
                                                <asp:BoundColumn DataField="CurrentSemester" HeaderText="CurSem"></asp:BoundColumn>
                                                <asp:BoundColumn HeaderText="OutStanding Amount" Visible="False">
                                                    <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                                        Font-Underline="False" HorizontalAlign="Right" />
                                                    <HeaderStyle Font-Bold="True" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                                        Font-Underline="False" HorizontalAlign="Center" />
                                                </asp:BoundColumn>
                                                <asp:TemplateColumn HeaderText="Amount">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="TxtAmt" runat="server" Style="text-align: right" OnTextChanged="TxtAmt_TextChanged1"
                                                            AutoPostBack="True"></asp:TextBox>
                                                    </ItemTemplate>
                                                    <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                                        Font-Underline="False" HorizontalAlign="Center" />
                                                    <HeaderStyle Font-Bold="True" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                                        Font-Underline="False" HorizontalAlign="Center" />
                                                </asp:TemplateColumn>
                                                <asp:BoundColumn DataField="TransactionAmount" HeaderText="Amount" DataFormatString="{0:F}">
                                                    <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                                        Font-Underline="False" HorizontalAlign="Right" />
                                                    <HeaderStyle Font-Bold="True" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                                        Font-Underline="False" HorizontalAlign="Center" />
                                                </asp:BoundColumn>
                                                <asp:TemplateColumn HeaderText="Voucher">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="Voucher" runat="server"></asp:TextBox>
                                                    </ItemTemplate>
                                                    <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                                        Font-Underline="False" HorizontalAlign="Center" />
                                                    <HeaderStyle Font-Bold="True" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                                        Font-Underline="False" HorizontalAlign="Center" />
                                                </asp:TemplateColumn>
                                                <asp:TemplateColumn HeaderText="Pocket Amount">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="PocketAmount" runat="server"></asp:TextBox>
                                                    </ItemTemplate>
                                                    <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                                        Font-Underline="False" HorizontalAlign="Center" />
                                                    <HeaderStyle Font-Bold="True" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                                        Font-Underline="False" HorizontalAlign="Center" />
                                                </asp:TemplateColumn>
                                                <asp:BoundColumn HeaderText="VoucherNo" Visible="False"></asp:BoundColumn>
                                            </Columns>
                                        </asp:DataGrid>&nbsp;
                                        <asp:HiddenField ID="lblStatus" runat="server" />
                                        <asp:HiddenField ID="today" runat="server" />
                                        &nbsp;
                                    </td>
                                    <td style="height: 16px" width="1%">
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                    </div>
                </td>
            </tr>
        </table>
    </asp:Panel> <asp:Button ID="btnHidden" runat="Server" OnClick="btnHidden_Click"  style="display:none" />

</asp:Content>
