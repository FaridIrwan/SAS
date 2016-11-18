<%@ Page Language="VB" MasterPageFile="~/MasterPage3.master" MaintainScrollPositionOnPostback="true" AutoEventWireup="false" CodeFile="SponsorAllocation.aspx.vb" Inherits="SponsorAllocation" Title="SponsorAllocation" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <script type="text/javascript" language="javascript" src="Scripts/popcalendar.js"></script>
    <script type="text/javascript" language="javascript" src="Scripts/functions.js"></script>
    <script language="javascript" type="text/javascript">
        function CheckAllocate() {
            if (document.getElementById("<%=txtAllocateAmount.ClientID%>").value == "") {
                alert("Amount Field Cannot Be Blank");
                document.getElementById("<%=txtAllocateAmount.ClientID%>").focus();
                return false;
            }
            return true;
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
            if (document.getElementById("<%=txtReceipNo.ClientID%>").value == "") {
                alert("Select a Record to Delete");
                return false;
            }
            if (confirm("Do You want to Delete Record?")) {
                return true;
            }
            else {
                return false;
            }

            return true;
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


        function getpostconfirm() {

            if (document.getElementById("<%=lblStatus.ClientID%>").value == "New") {
                alert("Select a Record to Post");
                return false;
            }
            if (document.getElementById("<%=lblStatus.ClientID%>").value == "Posted") {
                alert("Record Already Posted");
                return false;
            }
            var ans = Validate()
            if (ans == false) {
                return false;
            }
            else {
                if (confirm("Posted Record Cannot Be Altered, Do You Want To Proceed?")) {
                    return true;
                }
                else {
                    return false;
                }
            }
        }

        function CheckBatchDate() {
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
        function CheckTransDate() {
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
        function CheckChequeDate() {
            var digits = "0123456789/";
            var temp;
            for (var i = 0; i < document.getElementById("<%=txtchequeDate.ClientID %>").value.length; i++) {
                temp = document.getElementById("<%=txtchequeDate.ClientID%>").value.substring(i, i + 1);
                if (digits.indexOf(temp) == -1) {
                    alert("Enter Valid Date (dd/mm/yyyy)");
                    document.getElementById("<%=txtchequeDate.ClientID%>").value = "";
                    document.getElementById("<%=txtchequeDate.ClientID%>").focus();
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
        function getChequeDate() {
            popUpCalendar(document.getElementById("<%=ibtnChequeDate.ClientID%>"), document.getElementById("<%=txtchequeDate.ClientID%>"), 'dd/mm/yyyy')

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
                alert("Posted Record Cannot be Edited.");
                return false;
            }
            if (document.getElementById("<%=txtAllocationCode.ClientID%>").value.replace(re, "$1").length == 0) {
                alert("Receipt No Field Cannot Be Blank");
                document.getElementById("<%=txtAllocationCode.ClientID%>").focus();
                return false;
            }
            if (document.getElementById("<%=txtspnAmount.ClientID%>").value.replace(re, "$1").length == 0) {
                alert("Payment Amount Field Cannot Be Blank");
                document.getElementById("<%=txtspnAmount.ClientID%>").focus();
                return false;
            }

            if (document.getElementById("<%=ddlBankCode.ClientID%>").value == "-1") {
                alert("Select a Bank Code");
                document.getElementById("<%=ddlBankCode.ClientID%>").focus();
                return false;
            }

            if (document.getElementById("<%=txtBDate.ClientID%>").value.replace(re, "$1").length == 0) {
                alert("Batch Date Field Cannot Be Blank");
                document.getElementById("<%=txtBdate.ClientID%>").focus();
                return false;
            }
            var len = document.getElementById("<%=txtBdate.ClientID%>").value
            var RegExPattern = /^((((0?[1-9]|[12]\d|3[01])[\.\-\/](0?[13578]|1[02])[\.\-\/]((1[6-9]|[2-9]\d)?\d{2}))|((0?[1-9]|[12]\d|30)[\.\-\/](0?[13456789]|1[012])[\.\-\/]((1[6-9]|[2-9]\d)?\d{2}))|((0?[1-9]|1\d|2[0-8])[\.\-\/]0?2[\.\-\/]((1[6-9]|[2-9]\d)?\d{2}))|(29[\.\-\/]0?2[\.\-\/]((1[6-9]|[2-9]\d)?(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00)|00)))|(((0[1-9]|[12]\d|3[01])(0[13578]|1[02])((1[6-9]|[2-9]\d)?\d{2}))|((0[1-9]|[12]\d|30)(0[13456789]|1[012])((1[6-9]|[2-9]\d)?\d{2}))|((0[1-9]|1\d|2[0-8])02((1[6-9]|[2-9]\d)?\d{2}))|(2902((1[6-9]|[2-9]\d)?(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00)|00))))$/;
            var errorMessage = 'Enter Valid Batch Date in dd/mm/yyyy format.';
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
            if (document.getElementById("<%=txtPaymentDate.ClientID%>").value.replace(re, "$1").length == 0) {
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

            if (document.getElementById("<%=ddlBankCode.ClientID%>").value.replace(re, "$1").length == 0) {
                alert("Bank Code Field Cannot Be Blank");
                document.getElementById("<%=ddlBankCode.ClientID%>").focus();
                return false;
            }
            if (document.getElementById("<%=txtDesc.ClientID%>").value.replace(re, "$1").length == 0) {
                alert("Description Field Cannot Be Blank");
                document.getElementById("<%=txtDesc.ClientID%>").focus();
                return false;
            }
            if (document.getElementById("<%=txtAllocateAmount.ClientID%>").value.replace(re, "$1").length == 0) {
                alert("Select At Least One Student");
                document.getElementById("<%=txtAllocateAmount.ClientID%>").focus();
                return false;
            }
            if (document.getElementById("<%=txtAllAmount.ClientID%>").value.replace(re, "$1").length == 0) {
                alert("Allocated Amount Field Cannot Be Blank");
                document.getElementById("<%=txtAllAmount.ClientID%>").focus();
                return false;
            }
            //       if (document.getElementById("<%=txtCheque.ClientID%>").value=="")
            //      {
            //                 alert("Cheque Field Cannot Be Blank");
            //                 document.getElementById("<%=txtCheque.ClientID%>").focus();
            //                 return false;
            //      }
            //      if (document.getElementById("<%=txtchequeDate.ClientID%>").value=="")
            //      {
            //                 alert("Cheque Field Cannot Be Blank");
            //                 document.getElementById("<%=txtchequeDate.ClientID%>").focus();
            //                 return false;
            //      }
            var len = document.getElementById("<%=txtPaymentDate.ClientID%>").value
            var RegExPattern = /^((((0?[1-9]|[12]\d|3[01])[\.\-\/](0?[13578]|1[02])[\.\-\/]((1[6-9]|[2-9]\d)?\d{2}))|((0?[1-9]|[12]\d|30)[\.\-\/](0?[13456789]|1[012])[\.\-\/]((1[6-9]|[2-9]\d)?\d{2}))|((0?[1-9]|1\d|2[0-8])[\.\-\/]0?2[\.\-\/]((1[6-9]|[2-9]\d)?\d{2}))|(29[\.\-\/]0?2[\.\-\/]((1[6-9]|[2-9]\d)?(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00)|00)))|(((0[1-9]|[12]\d|3[01])(0[13578]|1[02])((1[6-9]|[2-9]\d)?\d{2}))|((0[1-9]|[12]\d|30)(0[13456789]|1[012])((1[6-9]|[2-9]\d)?\d{2}))|((0[1-9]|1\d|2[0-8])02((1[6-9]|[2-9]\d)?\d{2}))|(2902((1[6-9]|[2-9]\d)?(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00)|00))))$/;
            var errorMessage = 'Enter Valid Cheque Date in dd/mm/yyyy format.';
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
                alert("Receipt Date Cannot be Greater than Current Date");
                document.getElementById("<%=txtPaymentDate.ClientID%>").value = "";
                document.getElementById("<%=txtPaymentDate.ClientID%>").focus();
                //"");
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

            //Checking Amount to be allocated 
            var EntAmt1 = document.getElementById("<%=txtspnAmount.ClientID%>").value;
            var AllAmt = document.getElementById("<%=txtAllAmount.ClientID%>").value;
            var EntAmt = document.getElementById("<%=txtAllocateAmount.ClientID%>").value;

            var bal = EntAmt1 - AllAmt

            if (bal < EntAmt) {
                alert("Amount to be Allocated Exceeds the Sponsor Balance Payment");
                return false;
            }



            //     if (EntAmt < AllToAmt)
            //     {  
            //        alert("Allocated Amount excees the amount");
            //        return false;
            //     }


            return true;
        }	
	
    </script>
    <%--</ContentTemplate>     
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
                            <!--<td style="width: 1%; height: 14px">
                                <img src="images/find.png" width="24" height="24"  border="0" align="bottom" /></td>-->
                            <!--<td style="width: 3%; height: 14px">
                                <asp:Label ID="Label16" runat="server" Text="Search"></asp:Label></td>-->
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
                            <td style="width: 3%; height: 14px">
                                <asp:ImageButton ID="ibtnPrint" runat="server" ImageUrl="~/images/print.png" ToolTip="Print" />
                            </td>
                            <td style="width: 3%; height: 14px">
                                <asp:Label ID="Label17" runat="server" Text="Print"></asp:Label>
                            </td>
                        </tr>
                    </table>
                </td>
                <td>
                    <table style="border-collapse: collapse;" class="menuoff" onmouseover="className='menuon';"
                        onmouseout="className='menuoff';">
                        <tr>
                            <td style="width: 3%; height: 14px">
                                <asp:ImageButton ID="ibtnPosting" runat="server" ImageUrl="~/images/gposting.png"
                                    ToolTip="Cancel" OnClick="ibtnPosting_Click" />
                            </td>
                            <td style="width: 3%; height: 14px">
                                <asp:Label ID="Label6" runat="server" Text="Posting"></asp:Label>
                            </td>
                        </tr>
                    </table>
                </td>
                <td>
                    <table style="border-collapse: collapse;" class="menuoff" onmouseover="className='menuon';"
                        onmouseout="className='menuoff';">
                        <tr>
                            <td style="width: 3%; height: 14px">
                                <asp:ImageButton ID="ImageButton2" runat="server" ImageUrl="~/images/gothers.png"
                                    ToolTip="Cancel" OnClick="ibtnOthers_Click" />
                            </td>
                            <td style="width: 3%; height: 14px">
                                <asp:Label ID="Label5" runat="server" Text="Others"></asp:Label>
                            </td>
                        </tr>
                    </table>
                </td>
                <td>
                    <table style="border-collapse: collapse;" class="menuoff" onmouseover="className='menuon';"
                        onmouseout="className='menuoff';">
                        <tr>
                            <td style="width: 3%; height: 14px">
                                <asp:ImageButton ID="ibtnCancel" runat="server" ImageUrl="~/images/cancel.png" ToolTip="Cancel" />
                            </td>
                            <td style="width: 3%; height: 14px">
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
                    <asp:TextBox ID="txtRecNo" runat="server" Width="52px" AutoPostBack="True" Style="text-align: right"
                        OnTextChanged="txtRecNo_TextChanged" MaxLength="7" ReadOnly="true" CssClass="text_box"
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
                <td class="pagetext" style="height: 39px; text-align: right;">
                    <asp:Label ID="lblMenuName" runat="server" Width="309px"></asp:Label>
                </td>
            </tr>
        </table>
        <asp:Label ID="lblMsg" runat="server" CssClass="lblError" Style="text-align: center"
            Width="276px"></asp:Label></asp:Panel>
    <%--</ContentTemplate>     
</atlas:UpdatePanel>--%>
    <asp:Panel ID="Panel2" runat="server" Height="100%" Width="100%">
        <table style="width: 100%">
            <tr>
                <td style="width: 100%; height: 302px;">
                    <div style="border: thin solid #A6D9F4; width: 100%">
                        <table style="width: 464px">
                            <tr>
                                <td>
                                </td>
                                <td style="width: 13%">
                                </td>
                                <td style="width: 34px">
                                </td>
                                <td style="width: 1209px; text-align: center;">
                                </td>
                                <td colspan="1" style="width: 6448px; text-align: center">
                                </td>
                                <td style="width: 1266px">
                                    &nbsp;
                                </td>
                                <td style="width: 1266px">
                                </td>
                                <td colspan="2">
                                </td>
                                <td style="width: 151px">
                                </td>
                                <td style="width: 6642px">
                                </td>
                            </tr>
                            <tr>
                                <td style="height: 27px;">
                                </td>
                                <td style="width: 13%; height: 27px;">
                                    <asp:Label ID="Label3" runat="server" Text="Receipt No" Width="97px"></asp:Label>
                                </td>
                                <td style="width: 34px; height: 27px;">
                                </td>
                                <td style="width: 1209px; height: 27px; text-align: left">
                                    <asp:TextBox ID="txtAllocationCode" runat="server" MaxLength="20" Width="142px"></asp:TextBox>
                                    <asp:Image ID="ibtnSpn1" runat="server" Height="20px" ImageUrl="~/images/find_img.png"
                                        Width="21px" />
                                    &nbsp;&nbsp; &nbsp; &nbsp; &nbsp; &nbsp;
                                </td>
                                <td colspan="1" style="width: 6448px; height: 27px; text-align: left">
                                </td>
                                <td style="width: 1266px; height: 27px">
                                    <span style="color: #ff0000">*</span>
                                </td>
                                <td colspan="1" style="width: 1266px; height: 27px">
                                    <asp:Label ID="Label1" runat="server" Style="text-align: left" Text="Batch Id" Width="87px"></asp:Label>
                                </td>
                                <td colspan="2" style="height: 27px">
                                    <asp:TextBox ID="txtReceipNo" runat="server" MaxLength="20" Width="154px"></asp:TextBox>&nbsp;
                                </td>
                                <td style="width: 151px;" rowspan="5">
                                    <asp:ImageButton ID="ibtnStatus" runat="server" Enabled="False" ImageUrl="~/images/NotReady.gif"
                                        CssClass="cursor" />
                                </td>
                                <td style="width: 6642px; height: 27px;">
                                </td>
                            </tr>
                            <tr>
                                <td style="height: 25px;">
                                    &nbsp;</td>
                                <td style="width: 13%; height: 25px;">
                                    <asp:Label ID="Label48" runat="server" Text="Sponsor Invoice" Width="97px" Visible="false"></asp:Label>
                                </td>
                                <td colspan="1" style="height: 25px; width: 34px;">
                                </td>
                                <td style="height: 25px; width: 1209px;">                                   
                                    <asp:TextBox ID="txtSponsorInvoice" runat="server" MaxLength="20" Width="154px" Visible="false"></asp:TextBox>
                                    &nbsp;
                                    <asp:Image ID="ibtnSpnInv" runat="server" Height="20px" ImageUrl="~/images/find_img.png" Width="21px" Visible="false"/>
                                </td>
                                <td colspan="1" style="width: 6448px; height: 25px">
                                </td>
                                <td style="width: 1266px; height: 25px">
                                    <span style="color: #ff0000">*</span>
                                </td>
                                <td colspan="1" style="width: 1266px; height: 25px">
                                    <asp:Label ID="Label22" runat="server" Style="text-align: left" Text="Batch Date"
                                        Width="97px"></asp:Label>
                                </td>
                                <td colspan="1" style="width: 27px; height: 25px;">
                                    <asp:TextBox ID="txtBDate" runat="server" MaxLength="10" Style="text-align: left"
                                        Width="104px"></asp:TextBox>&nbsp;
                                </td>
                                <td colspan="1" style="width: 49px; height: 25px">
                                    <asp:Image ID="ibtnBDate" runat="server" ImageUrl="~/images/cal.gif" />
                                </td>
                                <td colspan="1" style="width: 6642px; height: 25px;">
                                </td>
                            </tr>
                            <tr>
                                <td style="height: 25px;">
                                    <span style="color: #ff0000">*</span>
                                </td>
                                <td style="width: 13%; height: 25px;">
                                    <asp:Label ID="Label2" runat="server" Text="Sponsor" Width="97px"></asp:Label>
                                </td>
                                <td style="width: 34px; height: 25px;">
                                </td>
                                <td style="width: 1209px; height: 25%; text-align: left">
                                    <asp:TextBox ID="txtSpnName" runat="server" MaxLength="20" Width="295px"></asp:TextBox>
                                </td>
                                <td colspan="1" style="width: 6448px; height: 25%; text-align: left">
                                </td>
                                <td style="width: 1266px; height: 25px">
                                    <span style="color: #ff0000">*</span>
                                </td>
                                <td style="width: 1266px; height: 25px">
                                    <asp:Label ID="Label7" runat="server" Style="text-align: left" Text="Allocated Amount  "
                                        Width="97px"></asp:Label>
                                </td>
                                <td style="width: 27px; height: 25px;">
                                    <asp:TextBox ID="txtAllAmount" runat="server" MaxLength="20" Width="103px" Style="text-align: right"></asp:TextBox>&nbsp;
                                </td>
                                <td style="width: 49px; height: 25px;">
                                </td>
                                <td style="width: 6642px; height: 25px;">
                                </td>
                            </tr>
                            <tr>
                                <td style="height: 25px;">
                                    &nbsp;<span style="color: #ff0000">*</span>
                                </td>
                                <td style="width: 13%; height: 25px;">
                                    <asp:Label ID="Label4" runat="server" Text="Amount Received" Width="97px"></asp:Label>
                                </td>
                                <td style="width: 34px; height: 25px;">
                                </td>
                                <td style="width: 1209px; text-align: left; height: 25px;">
                                    &nbsp;&nbsp;&nbsp;<asp:TextBox ID="txtspnAmount" runat="server" MaxLength="20" Style="text-align: right" Width="142px"></asp:TextBox>
                                </td>
                                <td colspan="1" style="width: 6448px; height: 25px; text-align: left">
                                </td>
                                <td style="width: 1266px; height: 25px;">
                                    <span style="color: #ff0000">*</span>
                                </td>
                                <td style="width: 1266px; height: 25px;">
                                    <asp:Label ID="Label9" runat="server" Style="text-align: left" Text="Transaction Date"
                                        Width="97px"></asp:Label>
                                </td>
                                <td style="width: 27px; height: 25px;">
                                    <asp:TextBox ID="txtPaymentDate" runat="server" MaxLength="10" Width="103px"></asp:TextBox>&nbsp;
                                </td>
                                <td style="width: 49px; height: 25px;">
                                    <asp:Image ID="ibtnPaymentDate" runat="server" ImageUrl="~/images/cal.gif" /><span
                                        style="color: #ff0000"></span>
                                </td>
                                <td style="width: 6642px; height: 25px;">
                                </td>
                            </tr>
                            <tr>
                                <td style="height: 25px;">
                                    &nbsp;</td>
                                <td style="width: 13%; height: 25px;">
                                    <asp:Label ID="Label10" runat="server" Text="Bank Code" Width="97px"></asp:Label>
                                </td>
                                <td style="width: 34px; height: 25px;">
                                </td>
                                <td style="width: 1209px; height: 25px;">
                                    &nbsp;
                                <span style="color: #ff0000">
                                    <asp:DropDownList ID="ddlBankCode" runat="server" AppendDataBoundItems="True" TabIndex="11" Width="149px">
                                        <asp:ListItem Value="-1">--Select--</asp:ListItem>
                                    </asp:DropDownList>
                                    </span>
                                </td>
                                <td colspan="1" style="width: 6448px; height: 25px">
                                    &nbsp;</td>
                                <td style="width: 1266px; height: 25px">
                                    <span style="color: #ff0000">*</span>
                                </td>
                                <td style="width: 1266px; height: 25px">
                                    <asp:Label ID="Label19" runat="server" Text="Amount" Width="97px"></asp:Label>
                                </td>
                                <td style="width: 27px; height: 25px;">
                                    <span style="color: #ff0000">
                                        <asp:TextBox ID="txtAllocateAmount" runat="server" Height="15px" MaxLength="20" Style="text-align: right"
                                            Width="103px" AutoPostBack="True" OnTextChanged="txtAllocateAmount_TextChanged"
                                            ReadOnly="True"></asp:TextBox></span>
                                </td>
                                <td style="width: 49px; height: 25px">
                                    <span style="color: #ff0000"></span>
                                </td>
                                <td style="width: 6642px; height: 25px;">
                                </td>
                            </tr>
                            <tr>
                                <td style="height: 25px;">
                                    <span style="color: #ff0000">*</span>
                                </td>
                                <td style="width: 13%; height: 26px;">
                                    <asp:Label ID="Label20" runat="server" Text="Description" Width="97px"></asp:Label>
                                </td>
                                <td colspan="1" style="width: 34px; height: 26px;">
                                </td>
                                <td style="height: 26px; width: 1209px;">
                                    <asp:TextBox ID="txtDesc" runat="server" MaxLength="50" Width="295px"></asp:TextBox>
                                </td>
                                <td colspan="1" style="width: 6448px; height: 26px">
                                    <asp:Image ID="imgbankCode" runat="server" Height="20px" ImageUrl="~/images/find_img.png" Visible="False" Width="21px" />
                                </td>
                                <td style="width: 1266px; height: 26px">
                                    &nbsp;
                                </td>
                                <td colspan="1" style="width: 1266px; height: 26px">
                                    <asp:Label ID="Label15" runat="server" Text="Select Student manually" Width="70px"></asp:Label>
                                </td>
                                <td colspan="1" style="width: 27px; height: 26px;">
                                    &nbsp;
                                    <asp:Image ID="IdtnStud" runat="server" Height="20px" 
                                        ImageUrl="~/images/find_img.png" Width="21px" />
                                </td>
                                <td colspan="1" style="width: 49px; height: 26px">
                                </td>
                                <td colspan="1" style="width: 151px; height: 26px;">
                                    <asp:Button ID="btnAllocate" runat="server" Text="Allocate" Visible="False" />
                                </td>
                                <td colspan="1" style="width: 6642px; height: 26px;">
                                </td>
                            </tr>
                            <tr>
                                <td style="height: 27px;">
                                </td>
                                <td style="width: 13%; height: 27px">
                                    &nbsp;
                                    <asp:Label ID="Label12" runat="server" Text="File Upload" Width="97px"></asp:Label>
                                </td>
                                <td colspan="1" style="width: 34px; height: 27px">
                                </td>
                                <td style="width: 1209px; height: 27px">
                                    &nbsp;
                                    <asp:Button ID="btnupload" runat="server" Text="Select student details template" />
                                </td>
                                <td colspan="1" style="width: 6448px; height: 27px">
                                </td>
                                <td style="width: 1266px; height: 27px">
                                    &nbsp;
                                </td>
                                <td colspan="1" style="width: 1266px; height: 27px">
                                    &nbsp;<asp:Label ID="Label24" runat="server" style="margin-top: 0px" 
                                        Text="Auto Allocate Amount" Visible="False" Width="122px"></asp:Label>
                                </td>
                                <td colspan="1" style="width: 27px; height: 27px">
                                    <asp:TextBox ID="txtauto" runat="server" AutoPostBack="True" Height="15px" 
                                        MaxLength="20" OnTextChanged="txtauto_TextChanged" Style="text-align: right" 
                                        Visible="False" Width="103px"></asp:TextBox>
                                </td>
                                <td colspan="1" style="width: 49px; height: 27px">
                                </td>
                                <td colspan="1" style="width: 151px; height: 27px">
                                </td>
                                <td colspan="1" style="width: 6642px; height: 27px">
                                </td>
                            </tr>
                            <tr>
                                <td style="height: 27px;">
                                </td>
                                <td style="width: 13%; height: 27px">
                                </td>
                                <td colspan="1" style="width: 34px; height: 27px">
                                </td>
                                <td style="width: 1209px; height: 27px">
                                    <asp:HiddenField ID="lblStatus" runat="server" />
                                    <asp:Button ID="Btnselect" runat="server" OnClick="Btnselect_Click" Text="Select"
                                        Width="44px" Visible="False" />
                                    <asp:HiddenField ID="today" runat="server" />
                                    <asp:FileUpload ID="FileUpload1" runat="server" Width="297px" Visible="False" />
                                </td>
                                <td colspan="1" style="width: 6448px; height: 27px">
                                </td>
                                <td style="width: 1266px; height: 27px">
                                    &nbsp;
                                </td>
                                <td colspan="1" style="width: 1266px; height: 27px">
                                    <asp:TextBox ID="txtspcode" runat="server" Visible="False"></asp:TextBox>
                                </td>
                                <td colspan="1" style="width: 53px; height: 27px">
                                    <asp:TextBox ID="txtchequeDate" runat="server" MaxLength="20" Width="103px" Visible="False"></asp:TextBox>
                                    <asp:TextBox ID="txtCheque" runat="server" MaxLength="20" Width="103px" Visible="False"></asp:TextBox>
                                    <asp:HiddenField ID="txtKodUniversiti" runat="server" />
                                    <asp:HiddenField ID="txtKumpulanPelajar" runat="server" />
                                    <asp:HiddenField ID="txtTarikhProses" runat="server" />
                                    <asp:HiddenField ID="txtKodBank" runat="server" />
                                </td>
                                <td colspan="1" style="width: 27px; height: 27px">
                                    <asp:Image ID="ibtnChequeDate" runat="server" ImageUrl="~/images/cal.gif" Visible="False" />
                                </td>
                                <td colspan="1" style="width: 151px; height: 27px">
                                </td>
                                <td colspan="1" style="width: 6642px; height: 27px">
                                </td>
                                <td colspan="1" style="width: 151px; height: 27px">
                                </td>
                            </tr>
                            <tr runat="server" id="trFileGen">
                                <td style="height: 27px;">
                                    &nbsp;</td>
                                <td style="width: 13%; height: 27px">
                                    Action</td>
                                <td colspan="1" style="width: 34px; height: 27px">
                                    &nbsp;</td>
                                <td style="width: 1209px; height: 27px">
                                    <asp:Button ID="btnGenerate" runat="server" 
                                        Text="Generate text file" />
                                </td>
                                <td colspan="1" style="width: 6448px; height: 27px">
                                    &nbsp;</td>
                                <td style="width: 1266px; height: 27px">
                                    &nbsp;</td>
                                <td colspan="1" style="width: 1266px; height: 27px">
                                    &nbsp;</td>
                                <td colspan="1" style="width: 53px; height: 27px">
                                    &nbsp;</td>
                                <td colspan="1" style="width: 27px; height: 27px">
                                    &nbsp;</td>
                                <td colspan="1" style="width: 151px; height: 27px">
                                    &nbsp;</td>
                                <td colspan="1" style="width: 6642px; height: 27px">
                                    &nbsp;</td>
                                <td colspan="1" style="width: 151px; height: 27px">
                                    &nbsp;</td>
                            </tr>
                        </table>
                    </div>
                </td>
            </tr>
        </table>
        <table style="width: 91%">
            <tr>
                <td colspan="4" style="vertical-align: top; height: 206px">
                    <table>
                        <tr>
                            <td style="width: 113px">
                                <!--<asp:Image ID="imgLeft1" runat="server" ImageUrl="images/b_orange_left.gif" ImageAlign="AbsBottom" />-->
                                <asp:Button ID="btnBatchInvoice" runat="server" CssClass="TabButton" Height="24px"
                                    OnClick="btnBatchInvoice_Click" Text="Active" Width="108px" /><!--<asp:Image ID="imgRight1" runat="server" ImageUrl="images/b_orange_right.gif" ImageAlign="AbsBottom" />-->
                            </td>
                            <td style="width: 139px">
                                <!--<asp:Image ID="imgLeft2" runat="server" ImageUrl="images/b_orange_left.gif" ImageAlign="AbsBottom" />-->
                                <asp:Button ID="btnSelection" runat="server" CssClass="TabButton" Height="25px" Text="InActive"
                                    Width="108px" OnClick="btnSelection_Click" /><!--<asp:Image ID="imgRight2" runat="server" ImageUrl="images/b_orange_right.gif" ImageAlign="AbsBottom" />-->
                            </td>
                        </tr>
                    </table>
                    <asp:MultiView ID="MultiView1" runat="server">
                        <asp:View ID="View1" runat="server">
                            <asp:CheckBox ID="chkSelectAll" runat="server" AutoPostBack="True" OnCheckedChanged="chkSelectAll_CheckedChanged"
                                Text="Confirm Students" />
                            <asp:DataGrid ID="dgView" runat="server" AutoGenerateColumns="False" DataKeyField="MatricNo"
                                Width="100%" PageSize="1" OnSelectedIndexChanged="dgView_SelectedIndexChanged">
                                <FooterStyle CssClass="dgFooterStyle" Height="20px" />
                                <SelectedItemStyle CssClass="dgSelectedItemStyle" />
                                <AlternatingItemStyle BackColor="Beige" CssClass="dgAlternatingItemStyle" Font-Bold="False"
                                    Font-Italic="False" Font-Overline="False" Font-Strikeout="False" Font-Underline="False" />
                                <ItemStyle CssClass="dgItemStyle" />
                                <HeaderStyle BackColor="#CDD7EE" CssClass="dgHeaderStyle" Font-Bold="True" Font-Italic="False"
                                    Font-Overline="False" Font-Size="Medium" Font-Strikeout="False" Font-Underline="False" />
                                <Columns>
                                    <asp:TemplateColumn HeaderText="Select">
                                        <ItemTemplate>
                                            &nbsp;<asp:CheckBox ID="Chk" runat="server" AutoPostBack="True" OnCheckedChanged="Chk_CheckedChanged" />
                                        </ItemTemplate>
                                        <HeaderStyle Width="5%" />
                                    </asp:TemplateColumn>
                                    <asp:BoundColumn DataField="MatricNO" HeaderText="Student MatricNo">
                                        <HeaderStyle Width="12%" />
                                    </asp:BoundColumn>
                                    <asp:BoundColumn DataField="StudentName" HeaderText="Name"></asp:BoundColumn>
                                    <asp:BoundColumn DataField="ProgramID" HeaderText="Program">
                                        <HeaderStyle Width="10%" />
                                    </asp:BoundColumn>
                                    <asp:BoundColumn HeaderText="IC No" DataField="ICNo">
                                        <HeaderStyle Width="10%" />
                                    </asp:BoundColumn>
                                    <asp:BoundColumn HeaderText="Semester" DataField="CurrentSemester">
                                        <HeaderStyle Width="10%" />
                                    </asp:BoundColumn>
                                    <asp:BoundColumn HeaderText="OutStanding Amount">
                                        <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                            Font-Underline="False" HorizontalAlign="Right" />
                                        <HeaderStyle Font-Bold="True" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                            Font-Underline="False" VerticalAlign="Middle" Width="10%" />
                                    </asp:BoundColumn>
                                    <asp:TemplateColumn HeaderText="Allocated Amount">
                                        <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                            Font-Underline="False" HorizontalAlign="Right" />
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtAllAmount1" runat="server" AutoPostBack="True" OnTextChanged="txtAllAmount1_TextChanged"
                                                Style="text-align: right" Width="97px"></asp:TextBox>
                                        </ItemTemplate>
                                        <HeaderStyle Font-Bold="True" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                            Font-Underline="False" VerticalAlign="Middle" Width="10%" />
                                    </asp:TemplateColumn>
                                    <asp:BoundColumn HeaderText="Balance Amount" Visible="False">
                                        <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                            Font-Underline="False" HorizontalAlign="Right" />
                                        <HeaderStyle Font-Bold="True" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                            Font-Underline="False" VerticalAlign="Middle" Width="10%" />
                                    </asp:BoundColumn>
                                    <asp:TemplateColumn HeaderText="Pocket Amount">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtpamont" runat="server" OnTextChanged="txtpamont_TextChanged"
                                                Style="text-align: right" Width="86px" AutoPostBack="True"></asp:TextBox>
                                        </ItemTemplate>
                                    </asp:TemplateColumn>
                                    
                                    <asp:BoundColumn DataField="TransactionAmount" HeaderText="Amt" Visible="False" />
                                    <asp:BoundColumn DataField="TransactionAmount" HeaderText="AllocatedAmount" Visible="False" />
                                    <asp:BoundColumn DataField="TempAmount" HeaderText="SPocketAmount" Visible="False" />

                                    <asp:BoundColumn DataField="NoKelompok" HeaderText="NoKelompok" Visible="False" />
                                    <asp:BoundColumn DataField="NoWarran" HeaderText="NoWarran" Visible="False" />
                                    <asp:BoundColumn DataField="amaunWarran" HeaderText="amaunWarran" Visible="False" />
                                    <asp:BoundColumn DataField="noAkaun" HeaderText="noAkaun" Visible="False" />
                                </Columns>
                            </asp:DataGrid></asp:View>
                        <asp:View ID="View2" runat="server">
                            <asp:DataGrid ID="dgUnView" runat="server" AutoGenerateColumns="False" DataKeyField="MatricNo"
                                Width="100%" PageSize="1">
                                <FooterStyle CssClass="dgFooterStyle" Height="20px" />
                                <SelectedItemStyle CssClass="dgSelectedItemStyle" />
                                <AlternatingItemStyle BackColor="Beige" CssClass="dgAlternatingItemStyle" Font-Bold="False"
                                    Font-Italic="False" Font-Overline="False" Font-Strikeout="False" Font-Underline="False" />
                                <ItemStyle CssClass="dgItemStyle" />
                                <HeaderStyle BackColor="#CDD7EE" CssClass="dgHeaderStyle" Font-Bold="True" Font-Italic="False"
                                    Font-Overline="False" Font-Size="Medium" Font-Strikeout="False" Font-Underline="False" />
                                <Columns>
                                    <asp:TemplateColumn HeaderText="Select">
                                        <ItemTemplate>
                                            <asp:CheckBox ID="chk" runat="server" />
                                        </ItemTemplate>
                                        <HeaderStyle Width="5%" />
                                    </asp:TemplateColumn>
                                    <asp:BoundColumn DataField="MatricNO" HeaderText="Student MatricNo">
                                        <HeaderStyle Width="12%" />
                                    </asp:BoundColumn>
                                    <asp:BoundColumn DataField="StudentName" HeaderText="StudentName"></asp:BoundColumn>
                                    <asp:BoundColumn DataField="ProgramID" HeaderText="Program">
                                        <HeaderStyle Width="10%" />
                                    </asp:BoundColumn>
                                    <asp:BoundColumn HeaderText="IC No" DataField="ICNo">
                                        <HeaderStyle Width="10%" />
                                    </asp:BoundColumn>
                                    <asp:BoundColumn HeaderText="Semester" DataField="CurrentSemester">
                                        <HeaderStyle Width="10%" />
                                    </asp:BoundColumn>
                                    <asp:BoundColumn HeaderText="OutStanding Amount" DataField="TransactionAmount">
                                        <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                            Font-Underline="False" HorizontalAlign="Right" />
                                        <HeaderStyle Font-Bold="True" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                            Font-Underline="False" VerticalAlign="Middle" Width="10%" />
                                    </asp:BoundColumn>
                                    <asp:TemplateColumn HeaderText="Allocated Amount">
                                        <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                            Font-Underline="False" HorizontalAlign="Right" />
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtAllAmount1" runat="server" AutoPostBack="True" OnTextChanged="txtAllAmount1_TextChanged"
                                                Style="text-align: right" Width="97px"></asp:TextBox>
                                        </ItemTemplate>
                                        <HeaderStyle Font-Bold="True" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                            Font-Underline="False" VerticalAlign="Middle" Width="10%" />
                                    </asp:TemplateColumn>
                                    <asp:BoundColumn HeaderText="Balance Amount" Visible="False">
                                        <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                            Font-Underline="False" HorizontalAlign="Right" />
                                        <HeaderStyle Font-Bold="True" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                            Font-Underline="False" VerticalAlign="Middle" Width="10%" />
                                    </asp:BoundColumn>
                                    <asp:TemplateColumn HeaderText="Pocket Amount">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtpamont" runat="server" OnTextChanged="txtpamont_TextChanged"
                                                Style="text-align: right" Width="86px"></asp:TextBox>
                                        </ItemTemplate>
                                    </asp:TemplateColumn>
                                    <asp:BoundColumn DataField="TransactionAmount" HeaderText="Amt" Visible="False">
                                    </asp:BoundColumn>
                                </Columns>
                            </asp:DataGrid></asp:View>
                        <asp:View ID="View3" runat="server">
                            <asp:DataGrid ID="dgViewSponsorInv" runat="server" AutoGenerateColumns="False" DataKeyField="MatricNo"
                                Width="100%" PageSize="1" Visible="false">
                                <FooterStyle CssClass="dgFooterStyle" Height="20px" />
                                <SelectedItemStyle CssClass="dgSelectedItemStyle" />
                                <AlternatingItemStyle BackColor="Beige" CssClass="dgAlternatingItemStyle" Font-Bold="False"
                                    Font-Italic="False" Font-Overline="False" Font-Strikeout="False" Font-Underline="False" />
                                <ItemStyle CssClass="dgItemStyle" />
                                <HeaderStyle BackColor="#CDD7EE" CssClass="dgHeaderStyle" Font-Bold="True" Font-Italic="False"
                                    Font-Overline="False" Font-Size="Medium" Font-Strikeout="False" Font-Underline="False" />
                                <Columns>
                                    <asp:TemplateColumn HeaderText="Select">
                                        <ItemTemplate>
                                            <asp:CheckBox ID="chk" runat="server" />
                                        </ItemTemplate>
                                        <HeaderStyle Width="5%" />
                                    </asp:TemplateColumn>
                                    <asp:BoundColumn DataField="MatricNO" HeaderText="Student MatricNo">
                                        <HeaderStyle Width="12%" />
                                    </asp:BoundColumn>
                                    <asp:BoundColumn DataField="StudentName" HeaderText="StudentName"></asp:BoundColumn>
                                    <asp:BoundColumn DataField="ProgramID" HeaderText="Program">
                                        <HeaderStyle Width="10%" />
                                    </asp:BoundColumn>
                                    <asp:BoundColumn HeaderText="IC No" DataField="ICNo">
                                        <HeaderStyle Width="10%" />
                                    </asp:BoundColumn>
                                    <asp:BoundColumn HeaderText="Semester" DataField="CurrentSemester">
                                        <HeaderStyle Width="10%" />
                                    </asp:BoundColumn>
                                    <asp:BoundColumn HeaderText="OutStanding Amount" DataField="TransactionAmount">
                                        <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                            Font-Underline="False" HorizontalAlign="Right" />
                                        <HeaderStyle Font-Bold="True" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                            Font-Underline="False" VerticalAlign="Middle" Width="10%" />
                                    </asp:BoundColumn>
                                    <asp:TemplateColumn HeaderText="Allocated Amount">
                                        <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                            Font-Underline="False" HorizontalAlign="Right" />
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtAllAmount1" runat="server" AutoPostBack="True" OnTextChanged="txtAllAmount1_TextChanged"
                                                Style="text-align: right" Width="97px"></asp:TextBox>
                                        </ItemTemplate>
                                        <HeaderStyle Font-Bold="True" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                            Font-Underline="False" VerticalAlign="Middle" Width="10%" />
                                    </asp:TemplateColumn>
                                    <asp:BoundColumn HeaderText="Balance Amount" Visible="False">
                                        <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                            Font-Underline="False" HorizontalAlign="Right" />
                                        <HeaderStyle Font-Bold="True" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                            Font-Underline="False" VerticalAlign="Middle" Width="10%" />
                                    </asp:BoundColumn>
                                    <asp:TemplateColumn HeaderText="Pocket Amount">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtpamont" runat="server" OnTextChanged="txtpamont_TextChanged"
                                                Style="text-align: right" Width="86px"></asp:TextBox>
                                        </ItemTemplate>
                                    </asp:TemplateColumn>
                                    <asp:BoundColumn DataField="TransactionAmount" HeaderText="Amt" Visible="False">
                                    </asp:BoundColumn>
                                </Columns>
                            </asp:DataGrid></asp:View>
                    </asp:MultiView>
                </td>
            </tr>
            <tr>
                <td style="vertical-align: top; width: 100%; height: 30px; text-align: right">
                    <asp:Label ID="lblTotal" runat="server" Text="Total Amount" Width="142px" Visible="False"></asp:Label>&nbsp;
                </td>
                <td style="vertical-align: top; height: 30px" width="8%">
                    <asp:TextBox ID="txtTotalPenAmt" runat="server" Height="15px" MaxLength="20" ReadOnly="True"
                        Style="text-align: right" Width="98px" Visible="False">0.00</asp:TextBox>
                </td>
                <td style="vertical-align: top; height: 30px">
                </td>
                <td style="vertical-align: top; height: 30px" width="10%">
                </td>
            </tr>
            <tr>
                <td style="vertical-align: top; width: 100%; height: 30px; text-align: right">
                    <asp:Label ID="Label21" runat="server" Text="Total Allocated Amount" Width="142px"
                        Visible="False"></asp:Label>
                </td>
                <td style="vertical-align: top; height: 30px" width="8%">
                    <asp:TextBox ID="txtAddedAmount" runat="server" Height="15px" MaxLength="20" ReadOnly="True"
                        Style="text-align: right" Width="98px" Visible="False">0.00</asp:TextBox>
                </td>
                <td style="vertical-align: top; height: 30px">
                </td>
                <td style="vertical-align: top; height: 30px" width="10%">
                </td>
            </tr>
            <tr>
                <td style="vertical-align: top; width: 100%; height: 30px; text-align: right">
                    <asp:Label ID="Label23" runat="server" Text="Total   " Width="142px" Visible="False"></asp:Label>
                </td>
                <td style="vertical-align: top; height: 30px" width="8%">
                    <asp:TextBox ID="txtAfterBalance" runat="server" Height="15px" MaxLength="20" ReadOnly="True"
                        Style="text-align: right" Width="98px" Visible="False">0.00</asp:TextBox>
                </td>
                <td style="vertical-align: top; height: 30px">
                </td>
                <td style="vertical-align: top; height: 30px" width="10%">
                </td>
            </tr>
        </table>
        <br />
        <asp:ObjectDataSource ID="odsPaymode" runat="server" SelectMethod="GetPaymode" TypeName="HTS.SAS.BS.bsTransaction">
        </asp:ObjectDataSource>
        &nbsp;
        <br />
        <br />
        <asp:DataGrid ID="dgInvoices" runat="server" AutoGenerateColumns="False" DataKeyField="TransactionCode"
            Visible="False" Width="91%">
            <FooterStyle CssClass="dgFooterStyle" Height="20px" />
            <SelectedItemStyle CssClass="dgSelectedItemStyle" />
            <AlternatingItemStyle BackColor="Beige" CssClass="dgAlternatingItemStyle" Font-Bold="False"
                Font-Italic="False" Font-Overline="False" Font-Strikeout="False" Font-Underline="False" />
            <ItemStyle CssClass="dgItemStyle" />
            <HeaderStyle BackColor="#CDD7EE" CssClass="dgHeaderStyle" Font-Bold="True" Font-Italic="False"
                Font-Overline="False" Font-Size="Medium" Font-Strikeout="False" Font-Underline="False" />
            <Columns>
                <asp:BoundColumn DataField="TransDate" HeaderText="Date"></asp:BoundColumn>
                <asp:BoundColumn DataField="TransactionCode" HeaderText="Document No"></asp:BoundColumn>
                <asp:BoundColumn DataField="Description" HeaderText="Description">
                    <HeaderStyle Width="40%" />
                </asp:BoundColumn>
                <asp:BoundColumn DataField="TransactionAmount" HeaderText="Transaction Amount">
                    <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                        Font-Underline="False" HorizontalAlign="Right" />
                </asp:BoundColumn>
                <asp:BoundColumn HeaderText="Statement Balance">
                    <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                        Font-Underline="False" HorizontalAlign="Right" />
                </asp:BoundColumn>
                <asp:BoundColumn DataField="Category" HeaderText="Category" Visible="False"></asp:BoundColumn>
            </Columns>
        </asp:DataGrid><asp:TextBox ID="txtOutAmt" runat="server" Font-Bold="True" Style="text-align: right"
            Visible="False" Width="102px"></asp:TextBox><br />
        <br />
        <asp:Button ID="ibtnYesNo" runat="server" Text="Button" OnClientClick="return confirm('Are you sure, to delete existing text file?');
        return false;" OnClick="ibtnYesNo_Click"/>
        <br />
        <br />
        <br />
    </asp:Panel>
    <br />
    <br />
    <br />
    <br />
    <br />
    <br />
     <asp:Button ID="btnHidden" runat="Server" OnClick="btnHidden_Click"  style="display:none" />
    <%--</ContentTemplate>     
</atlas:UpdatePanel>--%>
</asp:Content>
