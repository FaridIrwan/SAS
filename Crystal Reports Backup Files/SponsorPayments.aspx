<%@ Page Language="VB" MasterPageFile="~/MasterPage3.master" AutoEventWireup="false" MaintainScrollPositionOnPostback="true"
    CodeFile="SponsorPayments.aspx.vb" Inherits="SponsorPayments" Title="SponsorPayments" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <script language="javascript" src="Scripts/popcalendar.js" type="text/javascript"></script>
    <script language="javascript" src="Scripts/functions.js" type="text/javascript"></script>
    <script language="javascript" type="text/javascript">
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
            if (confirm("Do You Want to Delete Record?")) {
                return true;
            }
            else {
                return false;
            }

            return true;
        }
        function getpostconfirm() {

            if (document.getElementById("<%=lblStatus.ClientID%>").value == "New") {
                alert("Please Select Record to Post");
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
                alert("Record Already Posted");
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


            if (document.getElementById("<%=txtAllAmount.ClientID%>").value.replace(re, "$1").length == 0) {
                alert("Allocated Amount Field Cannot Be Blank");
                document.getElementById("<%=txtAllAmount.ClientID%>").focus();
                return false;
            }
            if (document.getElementById("<%=txtBDate.ClientID%>").value.replace(re, "$1").length == 0) {
                alert("Batch Date Field Cannot Be Blank");
                document.getElementById("<%=txtBdate.ClientID%>").focus();
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





            if (document.getElementById("<%=ddlPayment.ClientID%>").value == "-1") {
                alert("Select a Payment Mode");
                document.getElementById("<%=ddlPayment.ClientID%>").focus();
                return false;
            }
            if (document.getElementById("<%=ddlBankCode.ClientID%>").value == "-1") {
                alert("Select a Bank Code");
                document.getElementById("<%=ddlBankCode.ClientID%>").focus();
                return false;
            }
            if (document.getElementById("<%=txtPaymentDate.ClientID%>").value.replace(re, "$1").length == 0) {
                alert("Transaction Date Field Cannot Be Blank");
                document.getElementById("<%=txtPaymentDate.ClientID%>").focus();
                return false;
            }
            var len = document.getElementById("<%=txtPaymentDate.ClientID%>").value
            var RegExPattern = /^((((0?[1-9]|[12]\d|3[01])[\.\-\/](0?[13578]|1[02])[\.\-\/]((1[6-9]|[2-9]\d)?\d{2}))|((0?[1-9]|[12]\d|30)[\.\-\/](0?[13456789]|1[012])[\.\-\/]((1[6-9]|[2-9]\d)?\d{2}))|((0?[1-9]|1\d|2[0-8])[\.\-\/]0?2[\.\-\/]((1[6-9]|[2-9]\d)?\d{2}))|(29[\.\-\/]0?2[\.\-\/]((1[6-9]|[2-9]\d)?(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00)|00)))|(((0[1-9]|[12]\d|3[01])(0[13578]|1[02])((1[6-9]|[2-9]\d)?\d{2}))|((0[1-9]|[12]\d|30)(0[13456789]|1[012])((1[6-9]|[2-9]\d)?\d{2}))|((0[1-9]|1\d|2[0-8])02((1[6-9]|[2-9]\d)?\d{2}))|(2902((1[6-9]|[2-9]\d)?(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00)|00))))$/;
            var errorMessage = 'Enter Valid Transaction date in dd/mm/yyyy format.';
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
                alert("Payment Date Cannot be Greater than Current Date");
                document.getElementById("<%=txtPaymentDate.ClientID%>").value = "";
                document.getElementById("<%=txtPaymentDate.ClientID%>").focus();
                //"");
                return false;
            }




            if (document.getElementById("<%=txtCheque.ClientID%>").value.replace(re, "$1").length == 0) {
                alert("Cheque No Field Cannot Be Blank");
                document.getElementById("<%=txtCheque.ClientID%>").focus();
                return false;
            }
            //      if (document.getElementById("<%=txtchequeDate.ClientID%>").value=="")
            //      {
            //                 alert("Cheque Field Cannot Be Blank");
            //                 document.getElementById("<%=txtchequeDate.ClientID%>").focus();
            //                 return false;
            //      }
            var len = document.getElementById("<%=txtPaymentDate.ClientID%>").value
            var RegExPattern = /^((((0?[1-9]|[12]\d|3[01])[\.\-\/](0?[13578]|1[02])[\.\-\/]((1[6-9]|[2-9]\d)?\d{2}))|((0?[1-9]|[12]\d|30)[\.\-\/](0?[13456789]|1[012])[\.\-\/]((1[6-9]|[2-9]\d)?\d{2}))|((0?[1-9]|1\d|2[0-8])[\.\-\/]0?2[\.\-\/]((1[6-9]|[2-9]\d)?\d{2}))|(29[\.\-\/]0?2[\.\-\/]((1[6-9]|[2-9]\d)?(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00)|00)))|(((0[1-9]|[12]\d|3[01])(0[13578]|1[02])((1[6-9]|[2-9]\d)?\d{2}))|((0[1-9]|[12]\d|30)(0[13456789]|1[012])((1[6-9]|[2-9]\d)?\d{2}))|((0[1-9]|1\d|2[0-8])02((1[6-9]|[2-9]\d)?\d{2}))|(2902((1[6-9]|[2-9]\d)?(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00)|00))))$/;
            var errorMessage = 'Enter Valid Cheque date in dd/mm/yyyy format.';
            if (document.getElementById("<%=txtchequeDate.ClientID%>").value == "") {

                // return false;
            }
            else {
                if (document.getElementById("<%=txtchequeDate.ClientID%>").value.match(RegExPattern)) {
                    if (len.length == 8) {
                        alert(errorMessage);
                        document.getElementById("<%=txtchequeDate.ClientID%>").value = "";
                        document.getElementById("<%=txtchequeDate.ClientID%>").focus();
                        return false;
                    }
                }
                else {
                    alert(errorMessage);
                    document.getElementById("<%=txtchequeDate.ClientID%>").value = "";
                    document.getElementById("<%=txtchequeDate.ClientID%>").focus();
                    return false;
                }
            }

            var str1 = document.getElementById("<%=txtchequeDate.ClientID %>").value;
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
                alert("Payment Date Cannot be Greater than Current Date");
                document.getElementById("<%=txtchequeDate.ClientID%>").value = "";
                document.getElementById("<%=txtchequeDate.ClientID%>").focus();
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





            if (document.getElementById("<%=txtAmountPaid.ClientID%>").value.replace(re, "$1").length == 0) {
                alert("Amount to be Paid Field Cannot Be Blank");
                document.getElementById("<%=txtAmountPaid.ClientID%>").focus();
                return false;
            }
            if (document.getElementById("<%=txtPayeeName.ClientID%>").value.replace(re, "$1").length == 0) {
                alert("Payee Name Field Cannot Be Blank");
                document.getElementById("<%=txtPayeeName.ClientID%>").focus();
                return false;
            }
            if (document.getElementById("<%=txtDesc.ClientID%>").value.replace(re, "$1").length == 0) {
                alert("Description Field Cannot Be Blank");
                document.getElementById("<%=txtDesc.ClientID%>").focus();
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
                    alert("Please Enter Correct Record No");
                    document.getElementById("<%=txtRecNo.ClientID%>").value = 1;
                    document.getElementById("<%=txtRecNo.ClientID%>").focus();
                    return false;
                }
            }
            return true;
        }
	
    </script>
    <%--   <atlas:ScriptManager ID="scriptmanager1" EnablePartialRendering="true" runat="Server" />
   <atlas:UpdatePanel ID="up1" runat="server">
	<ContentTemplate>
    --%>
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
                                &nbsp;<asp:Label ID="Label6" runat="server" Text="Posting"></asp:Label>
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
                                    ToolTip="Cancel" OnClick="ibtnOthers_Click" />
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
                    <asp:TextBox ID="txtRecNo" runat="server" Width="52px" AutoPostBack="True" Style="text-align: right"
                        MaxLength="7" OnTextChanged="txtRecNo_TextChanged" ReadOnly="true" CssClass="text_box"
                        disabled="disabled" TabIndex="1" dir="ltr"></asp:TextBox>
                </td>
                <td style="width: 2%; height: 14px">
                    <asp:Label ID="Label47" runat="server">Of</asp:Label>&nbsp;
                    <asp:Label ID="lblCount" runat="server" Width="41px"></asp:Label>
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
                    <asp:Label ID="lblMenuName" runat="server" Text="University Fund" Width="354px"></asp:Label>
                </td>
            </tr>
        </table>
        <asp:Label ID="lblMsg" runat="server" CssClass="lblError" Style="text-align: center"
            Width="348px"></asp:Label></asp:Panel>
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
    <asp:Panel ID="Panel2" runat="server" Height="100%" Width="100%">
        <table style="width: 100%">
            <tr>
                <td style="width: 100%">
                   <div  style="border: thin solid #A6D9F4; width: 100%">
                        <table width="100%">
                            <tr>
                                <td style="height: 27px">
                                </td>
                                <td style="width: 16%; height: 27px">
                                </td>
                                <td style="width: 499px; height: 27px">
                                </td>
                                <td colspan="3" style="width: 389px; height: 27px">
                                    <span style="color: #ff0000"></span>
                                </td>
                                <td style="width: 107px; height: 27px; text-align: right">
                                </td>
                                <td style="width: 23px; height: 27px; text-align: right">
                                </td>
                                <td style="width: 23px; height: 27px; text-align: right">
                                </td>
                                <td style="width: 74px; height: 27px; text-align: right">
                                </td>
                                <td colspan="1" style="width: 4638px; height: 27px">
                                </td>
                                <td colspan="1" style="width: 4638px; height: 27px">
                                </td>
                                <td colspan="1" style="width: 4638px; height: 27px">
                                </td>
                                <td colspan="2" style="height: 27px">
                                </td>
                                <td colspan="1" style="height: 27px; width: 645px;">
                                </td>
                                <td style="width: 3px; height: 27px">
                                </td>
                                <td style="width: 378px; height: 27px">
                                </td>
                            </tr>
                            <tr>
                                <td style="height: 27px">
                                </td>
                                <td style="width: 16%; height: 27px;">
                                    <asp:Label ID="Label3" runat="server" Text="Receipt No" Width="97px"></asp:Label>
                                </td>
                                <td style="width: 499px; height: 27px;">
                                </td>
                                <td style="width: 389px; height: 27px;" colspan="3">
                                    <asp:TextBox ID="txtAllocationCode" runat="server" MaxLength="20" Width="143px"></asp:TextBox><span
                                        style="color: #ff0000">*<asp:Image ID="ibtnSpn1" runat="server" Height="16px" ImageUrl="~/images/find_img.png"
                                            Width="16px" /></span>
                                </td>
                                <td style="width: 107px; text-align: right; height: 27px;">
                                    <asp:Label ID="Label23" runat="server" Text="Payment No.  " Width="107px" Style="text-align: left"></asp:Label>
                                </td>
                                <td colspan="1" style="height: 27px; text-align: left">
                                </td>
                                <td colspan="1" style="height: 27px; text-align: left">
                                    <asp:TextBox ID="txtReceipNo" runat="server" MaxLength="20" Width="103px"></asp:TextBox><span
                                        style="color: #ff0000">*&nbsp;</span>
                                </td>
                                <td colspan="1" style="height: 27px; text-align: left">
                                </td>
                                <td rowspan="6" style="width: 4638px">
                                    <asp:ImageButton ID="ibtnStatus" runat="server" Enabled="False" ImageUrl="~/images/NotReady.gif"
                                        CssClass="cursor" />
                                </td>
                                <td colspan="1" style="height: 27px; text-align: left">
                                </td>
                                <td colspan="1" style="width: 5565px;" rowspan="6">
                                </td>
                                <td colspan="1" style="height: 27px; width: 645px;">
                                </td>
                                <td style="width: 3px; height: 27px;">
                                </td>
                                <td style="width: 378px; height: 27px;">
                                </td>
                            </tr>
                            <tr>
                                <td style="height: 25px">
                                </td>
                                <td style="width: 16%; height: 25px;">
                                    <asp:Label ID="Label2" runat="server" Text="Sponsor" Width="97px"></asp:Label>
                                </td>
                                <td colspan="1" style="height: 25px; width: 499px;">
                                </td>
                                <td colspan="3" style="height: 25px; width: 389px;">
                                    <asp:TextBox ID="txtSpnName" runat="server" MaxLength="20" Width="142px"></asp:TextBox><span
                                        style="color: #ff0000">*</span>
                                </td>
                                <td colspan="1" style="width: 107px; height: 25px">
                                    <asp:Label ID="Label7" runat="server" Text="Allocated Amount  " Width="97px" Style="text-align: left"></asp:Label>
                                </td>
                                <td colspan="1" style="width: 23px; height: 25px">
                                </td>
                                <td colspan="1" style="width: 23px; height: 25px">
                                    <span style="color: #ff0000">
                                        <asp:TextBox ID="txtAllAmount" runat="server" MaxLength="20" Width="105px" Style="text-align: right"></asp:TextBox>*</span><span
                                            style="color: #ff0000"></span>
                                </td>
                                <td colspan="1" style="width: 74px; height: 25px">
                                    <span style="color: #ff0000"></span>
                                </td>
                                <td colspan="1" style="width: 645px; height: 25px">
                                </td>
                                <td colspan="1" style="width: 74px; height: 25px">
                                </td>
                                <td colspan="1" style="height: 25px; width: 3px;">
                                </td>
                                <td colspan="1" style="width: 378px; height: 25px;">
                                </td>
                            </tr>
                            <tr>
                                <td style="height: 25px">
                                </td>
                                <td style="width: 16%; height: 25px;">
                                    <asp:Label ID="Label4" runat="server" Text="Amount Received" Width="97px"></asp:Label>
                                </td>
                                <td style="width: 499px; height: 25px;">
                                </td>
                                <td style="width: 389px; height: 25px;" colspan="3">
                                    <asp:TextBox ID="txtspnAmount" runat="server" MaxLength="20" Width="143px" Style="text-align: right"></asp:TextBox><span
                                        style="color: #ff0000">*</span>&nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;
                                    &nbsp; &nbsp; &nbsp; &nbsp;&nbsp;
                                </td>
                                <td style="width: 107px; height: 25px">
                                    <asp:Label ID="Label22" runat="server" Text="Batch Date" Width="97px" Style="text-align: left"></asp:Label>
                                </td>
                                <td style="width: 23px; height: 25px">
                                </td>
                                <td style="width: 23px; height: 25px">
                                    <span style="color: #ff0000">
                                        <asp:TextBox ID="txtBDate" runat="server" MaxLength="10" Style="text-align: left"
                                            Width="105px"></asp:TextBox>*</span>
                                </td>
                                <td style="width: 74px; height: 25px">
                                    <asp:Image ID="ibtnBDate" runat="server" ImageUrl="~/images/cal.gif" />
                                </td>
                                <td style="width: 645px; height: 25px;">
                                </td>
                                <td style="width: 645px; height: 25px">
                                </td>
                                <td colspan="1" rowspan="4" style="width: 3px">
                                </td>
                                <td style="width: 378px; height: 25px;">
                                </td>
                            </tr>
                            <tr>
                                <td style="height: 25px">
                                </td>
                                <td style="width: 16%; height: 25px;">
                                    <asp:Label ID="Label8" runat="server" Text="Payment Mode" Width="97px"></asp:Label>
                                </td>
                                <td style="width: 499px; height: 25px;">
                                </td>
                                <td style="width: 389px; height: 25px;" colspan="3">
                                    <asp:DropDownList ID="ddlPayment" runat="server" Width="149px" AppendDataBoundItems="True"
                                        Height="20px" AutoPostBack="True" OnSelectedIndexChanged="ddlPayment_SelectedIndexChanged">
                                        <asp:ListItem Value="-1">--Select--</asp:ListItem>
                                    </asp:DropDownList>
                                    <span style="color: #ff0000">*</span>&nbsp; &nbsp;&nbsp; &nbsp; &nbsp; &nbsp; &nbsp;
                                    &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;&nbsp;
                                </td>
                                <td style="width: 107px; height: 25px;">
                                    <asp:Label ID="Label9" runat="server" Text="Transaction Date" Width="97px" Style="text-align: left"></asp:Label>
                                </td>
                                <td style="width: 23px; height: 25px">
                                </td>
                                <td style="width: 23px; height: 25px;">
                                    <span style="color: #ff0000">
                                        <asp:TextBox ID="txtPaymentDate" runat="server" MaxLength="10" Width="105px"></asp:TextBox>*</span>
                                </td>
                                <td style="width: 74px; height: 25px;">
                                    <asp:Image ID="ibtnPaymentDate" runat="server" ImageUrl="~/images/cal.gif" />
                                </td>
                                <td style="width: 645px; height: 25px;">
                                </td>
                                <td style="width: 645px; height: 25px">
                                </td>
                            </tr>
                            <tr>
                                <td style="height: 26px">
                                </td>
                                <td style="width: 16%; height: 26px;">
                                    <asp:Label ID="Label21" runat="server" Text="Amount to be Paid" Width="97px"></asp:Label>
                                </td>
                                <td style="width: 499px; height: 26px;">
                                </td>
                                <td style="width: 389px; height: 26px;" colspan="3">
                                    <asp:TextBox ID="txtAmountPaid" runat="server" MaxLength="20" Width="142px" Style="text-align: right"></asp:TextBox><span
                                        style="color: #ff0000">*</span> &nbsp; &nbsp; &nbsp;
                                </td>
                                <td style="width: 107px; height: 26px">
                                    <asp:Label ID="Label15" runat="server" Text="Cheque Date  " Width="97px" Style="text-align: left"></asp:Label>
                                </td>
                                <td style="width: 23px; height: 26px">
                                </td>
                                <td style="width: 23px; height: 26px">
                                    <span style="color: #ff0000">
                                        <asp:TextBox ID="txtchequeDate" runat="server" MaxLength="10" Width="105px"></asp:TextBox>*</span>
                                </td>
                                <td style="width: 74px; height: 26px">
                                    <asp:Image ID="ibtnChequeDate" runat="server" ImageUrl="~/images/cal.gif" />
                                </td>
                                <td style="width: 645px; height: 26px;">
                                </td>
                                <td style="width: 645px; height: 26px">
                                </td>
                            </tr>
                            <tr>
                                <td style="height: 17px">
                                </td>
                                <td style="width: 16%; height: 17px;">
                                    <asp:Label ID="Label10" runat="server" Text="Bank Code" Width="97px"></asp:Label>
                                </td>
                                <td colspan="1" style="height: 17px; width: 499px;">
                                </td>
                                <td colspan="3" style="height: 17px; width: 389px;">
                                    <span style="color: #ff0000">
                                        <asp:DropDownList ID="ddlBankCode" runat="server" Width="149px" AppendDataBoundItems="True"
                                            Height="20px">
                                            <asp:ListItem Value="-1">--Select--</asp:ListItem>
                                        </asp:DropDownList>
                                        *</span>
                                </td>
                                <td colspan="1" style="width: 107px; height: 17px">
                                    <asp:Label ID="Label12" runat="server" Text="Cheque No." Width="97px" ForeColor="Black"></asp:Label>
                                </td>
                                <td colspan="1" style="width: 23px; height: 17px">
                                </td>
                                <td colspan="1" style="width: 23px; height: 17px">
                                    <span style="color: #ff0000">
                                        <asp:TextBox ID="txtCheque" runat="server" MaxLength="20" Width="105px"></asp:TextBox>*</span>
                                </td>
                                <td colspan="1" style="width: 74px; height: 17px">
                                </td>
                                <td colspan="1" style="width: 645px; height: 17px;">
                                </td>
                                <td colspan="1" style="width: 645px; height: 17px">
                                </td>
                            </tr>
                            <tr>
                                <td style="height: 18px">
                                </td>
                                <td style="width: 16%; height: 18px">
                                    <asp:Label ID="Label19" runat="server" Text="Payee Name" Width="97px"></asp:Label>
                                </td>
                                <td style="width: 499px; height: 18px">
                                </td>
                                <td style="height: 18px; width: 389px;" colspan="3">
                                    <asp:TextBox ID="txtPayeeName" runat="server" MaxLength="20" Width="257px"></asp:TextBox><span
                                        style="color: #ff0000">*</span>
                                </td>
                                <td style="width: 107px; height: 18px">
                                    <span style="color: #ff0000"></span>
                                </td>
                                <td style="width: 23px; height: 18px">
                                </td>
                                <td style="width: 23px; height: 18px">
                                    <span style="color: #ff0000"></span>
                                </td>
                                <td style="width: 74px; height: 18px">
                                </td>
                                <td style="width: 4638px; height: 18px">
                                </td>
                                <td style="width: 4638px; height: 18px">
                                </td>
                                <td style="width: 4638px; height: 18px">
                                </td>
                                <td style="width: 5565px; height: 18px">
                                </td>
                                <td style="width: 645px; height: 18px">
                                </td>
                                <td style="width: 645px; height: 18px">
                                </td>
                                <td style="width: 3px; height: 18px">
                                </td>
                            </tr>
                            <tr>
                                <td style="height: 25px">
                                </td>
                                <td style="width: 16%; height: 25px;">
                                    <asp:Label ID="Label20" runat="server" Text="Description" Width="97px"></asp:Label>
                                </td>
                                <td colspan="1" style="height: 25px; width: 499px;">
                                </td>
                                <td colspan="3" style="height: 25px; width: 389px;">
                                    <asp:TextBox ID="txtDesc" runat="server" MaxLength="50" Width="257px"></asp:TextBox><span
                                        style="color: #ff0000">*</span><span style="color: #ff0000"></span>
                                </td>
                                <td colspan="1" style="width: 107px; height: 25px">
                                    <span style="color: #ff0000"></span>
                                </td>
                                <td colspan="1" style="width: 23px; height: 25px">
                                </td>
                                <td colspan="1" style="width: 23px; height: 25px">
                                </td>
                                <td colspan="1" style="width: 74px; height: 25px">
                                </td>
                                <td colspan="1" style="width: 4638px; height: 25px">
                                </td>
                                <td colspan="1" style="width: 4638px; height: 25px">
                                </td>
                                <td colspan="1" style="width: 4638px; height: 25px;">
                                </td>
                                <td colspan="1" style="width: 5565px; height: 25px">
                                </td>
                                <td colspan="1" style="width: 645px; height: 25px;">
                                </td>
                                <td colspan="1" style="width: 645px; height: 25px">
                                </td>
                                <td colspan="1" style="width: 3px; height: 25px;">
                                </td>
                            </tr>
                            <tr>
                                <td style="height: 27px">
                                </td>
                                <td style="width: 16%; height: 27px;">
                                </td>
                                <td colspan="1" style="width: 499px; height: 27px;">
                                </td>
                                <td colspan="3" style="height: 27px; width: 389px;">
                                    &nbsp;<asp:HiddenField ID="lblStatus" runat="server" />
                                </td>
                                <td colspan="1" style="width: 107px; height: 27px">
                                    <asp:HiddenField ID="today" runat="server" />
                                </td>
                                <td colspan="1" style="width: 23px; height: 27px">
                                </td>
                                <td colspan="1" style="width: 23px; height: 27px">
                                    <asp:TextBox ID="txtCreditref" runat="server" Visible="False" Width="90px"></asp:TextBox>
                                </td>
                                <td colspan="1" style="width: 74px; height: 27px">
                                </td>
                                <td colspan="1" style="width: 4638px; height: 27px">
                                </td>
                                <td colspan="1" style="width: 4638px; height: 27px">
                                </td>
                                <td colspan="1" style="width: 4638px; height: 27px;">
                                    &nbsp;
                                </td>
                                <td colspan="1" style="width: 5565px; height: 27px">
                                </td>
                                <td colspan="1" style="width: 645px; height: 27px;">
                                </td>
                                <td colspan="1" style="width: 645px; height: 27px">
                                </td>
                                <td colspan="1" style="width: 3px; height: 27px;">
                                </td>
                            </tr>
                        </table>
                  </div>
                </td>
            </tr>
        </table>
    </asp:Panel>
    <asp:Button ID="btnHidden" runat="Server" OnClick="btnHidden_Click"  style="display:none" />
    <%--</ContentTemplate>     
</atlas:UpdatePanel>--%>

</asp:Content>
