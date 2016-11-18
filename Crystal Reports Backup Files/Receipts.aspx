<%@ Page Language="VB" MasterPageFile="~/MasterPage3.master" AutoEventWireup="false" MaintainScrollPositionOnPostback="true"
    CodeFile="Receipts.aspx.vb" Inherits="Receipts" Title="Receipts" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <script language="javascript" src="Scripts/popcalendar.js" type="text/javascript"></script>
    <script language="javascript" src="Scripts/functions.js" type="text/javascript"></script>
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
        function OpenWindow() {
            var batch = document.getElementById("<%=txtBatchId.ClientID%>").value;
            new_window = window.open('GroupReport/RptReceiptViewer.aspx?bat=' + batch, 'Hanodale', 'width=520,height=500,resizable=1,srcollable =yes'); new_window.focus();
        }
        function CheckSearch() {
            if (document.getElementById("<%=ddlReceiptFor.ClientID%>").value == "-1") {
                alert("Select a Receipt For to Search");
                document.getElementById("<%=ddlReceiptFor.ClientID%>").focus();
                return false;
            }
            return true;
        }
        function Checkallocamt() {
            if (document.getElementById("<%=txtStuIndex.ClientID%>").value == "0.00") {
                alert("enter amount");
                document.getElementById("<%=txtStuIndex.ClientID%>").focus();
                return false;
            }
            return false;
        }


        function CheckInvgrid() {


            if (document.getElementById("<%=txtAllAmount.ClientID%>").value == "0.00") {
                alert("Allocate the Amount");
                return false;
            }

            var st1 = parseFloat(document.getElementById("<%=txtAllAmount.ClientID%>").value);
            var st2 = parseFloat(document.getElementById("<%=txtStuIndex.ClientID%>").value);
            if (st1 > st2) {
                alert(" Total Amount Cannot be Greater than Amount to be Allocated");
                return false
            }

            return true;
        }

        function CheckBatchDate() {
            var digits = "0123456789/";
            var temp;
            for (var i = 0; i < document.getElementById("<%=txtBatchDate.ClientID %>").value.length; i++) {
                temp = document.getElementById("<%=txtBatchDate.ClientID%>").value.substring(i, i + 1);
                if (digits.indexOf(temp) == -1) {
                    alert("Enter Valid Date (dd/mm/yyyy)");
                    document.getElementById("<%=txtBatchDate.ClientID%>").value = "";
                    document.getElementById("<%=txtBatchDate.ClientID%>").focus();
                    return false;
                }
            }
            return true;
        }
        function CheckRecpDate() {
            var digits = "0123456789/";
            var temp;
            for (var i = 0; i < document.getElementById("<%=txtReceiptDate.ClientID %>").value.length; i++) {
                temp = document.getElementById("<%=txtReceiptDate.ClientID%>").value.substring(i, i + 1);
                if (digits.indexOf(temp) == -1) {
                    alert("Enter Valid Date (dd/mm/yyyy)");
                    document.getElementById("<%=txtReceiptDate.ClientID%>").value = "";
                    document.getElementById("<%=txtReceiptDate.ClientID%>").focus();
                    return false;
                }
            }
            return true;
        }

        function CheckRecpAmount() {
            var digits = "0123456789.";
            var temp;
            for (var i = 0; i < document.getElementById("<%=txtSpnAmount.ClientID %>").value.length; i++) {
                temp = document.getElementById("<%=txtSpnAmount.ClientID%>").value.substring(i, i + 1);
                if (digits.indexOf(temp) == -1) {
                    alert("Enter Valid Amount");
                    document.getElementById("<%=txtSpnAmount.ClientID%>").value = "";
                    document.getElementById("<%=txtSpnAmount.ClientID%>").focus();
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
        //function checknValue()	{
        //								
        //if (((event.keyCode < 48) || (event.keyCode > 57)) && (event.keyCode != 13)  && (event.keyCode != 46) )
        //	{
        //	alert("Please Enter correct Values");
        //	event.keyCode=0;
        //	}
        //	}
        function getDate1from() {
            popUpCalendar(document.getElementById("<%=ibtnRecDate.ClientID%>"), document.getElementById("<%=txtReceiptDate.ClientID%>"), 'dd/mm/yyyy')
        }
        function getDate2from() {
            popUpCalendar(document.getElementById("<%=ibtnBatchDate.ClientID%>"), document.getElementById("<%=txtBatchDate.ClientID%>"), 'dd/mm/yyyy')
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
        function CheckAllocate() {
            if (document.getElementById("<%=txtAllocateAmount.ClientID%>").value == "") {
                alert("Allocated Amount Field Cannot Be Blank");
                document.getElementById("<%=txtAllocateAmount.ClientID%>").focus();
                return false;
            }
            return true;
        }

        function amount() {
            var digits = "0123456789.";
            var temp;
            for (var i = 0; i < document.getElementById("<%=txtAllocateAmount.ClientID %>").value.length; i++) {
                temp = document.getElementById("<%=txtAllocateAmount.ClientID%>").value.substring(i, i + 1);
                if (digits.indexOf(temp) == -1) {
                    alert("Please Enter Correct Amount");
                    document.getElementById("<%=txtAllocateAmount.ClientID%>").value = "";

                    document.getElementById("<%=txtAllocateAmount.ClientID%>").focus();
                    return false;
                }
            }
        }

        function getcheck() {
            var digits = "0123456789.";
            var temp;
            return true;
        }

        function validate() {
            var re = /\s*((\S+\s*)*)/;
            <%--if (document.getElementById("<%=txtBatchId.ClientID%>").value.replace(re, "$1").length == 0) {
                alert("Batch Id Field Cannot Be Blank");
                document.getElementById("<%=txtBatchId.ClientID%>").focus();
                return false;
            }--%>
            var digits = "ABCDEFGHIJKLMNOPQRSTUVWXYZ abcdefghijklmnopqrstuvwxyz0123456789";
            var temp;
            for (var i = 0; i < document.getElementById("<%=txtBatchId.ClientID %>").value.length; i++) {
                temp = document.getElementById("<%=txtBatchId.ClientID%>").value.substring(i, i + 1);
                if (digits.indexOf(temp) == -1) {
                    alert("Please Enter Correct Batch Id");
                    document.getElementById("<%=txtBatchId.ClientID%>").focus();
                    return false;
                }
            }

            if (document.getElementById("<%=ddlReceiptFor.ClientID%>").value == "-1") {
                alert("Select a Receipt For");
                document.getElementById("<%=ddlReceiptFor.ClientID%>").focus();
                return false;
            }
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
            if (document.getElementById("<%=txtDescription.ClientID%>").value.replace(re, "$1").length == 0) {
                alert("Description Field Cannot Be Blank");
                document.getElementById("<%=txtDescription.ClientID%>").focus();
                return false;
            }
            if (document.getElementById("<%=txtAllocateAmount.ClientID%>").value.replace(re, "$1").length == 0) {
                alert("Allocated Amount Field Cannot Be Blank");
                document.getElementById("<%=txtAllocateAmount.ClientID%>").focus();
                return false;
            }
            if (document.getElementById("<%=txtBatchDate.ClientID%>").value.replace(re, "$1").length == 0) {
                alert("Batch Date Field Cannot Be Blank");
                document.getElementById("<%=txtBatchDate.ClientID%>").focus();
                return false;
            }
            var len = document.getElementById("<%=txtBatchDate.ClientID%>").value
            var RegExPattern = /^((((0?[1-9]|[12]\d|3[01])[\.\-\/](0?[13578]|1[02])[\.\-\/]((1[6-9]|[2-9]\d)?\d{2}))|((0?[1-9]|[12]\d|30)[\.\-\/](0?[13456789]|1[012])[\.\-\/]((1[6-9]|[2-9]\d)?\d{2}))|((0?[1-9]|1\d|2[0-8])[\.\-\/]0?2[\.\-\/]((1[6-9]|[2-9]\d)?\d{2}))|(29[\.\-\/]0?2[\.\-\/]((1[6-9]|[2-9]\d)?(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00)|00)))|(((0[1-9]|[12]\d|3[01])(0[13578]|1[02])((1[6-9]|[2-9]\d)?\d{2}))|((0[1-9]|[12]\d|30)(0[13456789]|1[012])((1[6-9]|[2-9]\d)?\d{2}))|((0[1-9]|1\d|2[0-8])02((1[6-9]|[2-9]\d)?\d{2}))|(2902((1[6-9]|[2-9]\d)?(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00)|00))))$/;
            var errorMessage = 'Enter Valid Date dd/mm/yyyy';

            if (document.getElementById("<%=txtBatchDate.ClientID%>").value.match(RegExPattern)) {
                if (len.length == 8) {
                    alert(errorMessage);
                    document.getElementById("<%=txtBatchDate.ClientID%>").value = "";
                    document.getElementById("<%=txtBatchDate.ClientID%>").focus();
                    return false;
                }
            }
            else {
                alert(errorMessage);
                document.getElementById("<%=txtBatchDate.ClientID%>").value = "";
                document.getElementById("<%=txtBatchDate.ClientID%>").focus();
                return false;
            }

            var str1 = document.getElementById("<%=txtBatchDate.ClientID %>").value;
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
                document.getElementById("<%=txtBatchDate.ClientID%>").value = "";
                document.getElementById("<%=txtBatchDate.ClientID%>").focus();
                //"");
                return false;
            }

            if (document.getElementById("<%=txtReceiptDate.ClientID%>").value == "") {
                alert("Receipt Date Field Cannot Be Blank");
                document.getElementById("<%=txtReceiptDate.ClientID%>").focus();
                return false;
            }


            var len = document.getElementById("<%=txtReceiptDate.ClientID%>").value
            var RegExPattern = /^((((0?[1-9]|[12]\d|3[01])[\.\-\/](0?[13578]|1[02])[\.\-\/]((1[6-9]|[2-9]\d)?\d{2}))|((0?[1-9]|[12]\d|30)[\.\-\/](0?[13456789]|1[012])[\.\-\/]((1[6-9]|[2-9]\d)?\d{2}))|((0?[1-9]|1\d|2[0-8])[\.\-\/]0?2[\.\-\/]((1[6-9]|[2-9]\d)?\d{2}))|(29[\.\-\/]0?2[\.\-\/]((1[6-9]|[2-9]\d)?(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00)|00)))|(((0[1-9]|[12]\d|3[01])(0[13578]|1[02])((1[6-9]|[2-9]\d)?\d{2}))|((0[1-9]|[12]\d|30)(0[13456789]|1[012])((1[6-9]|[2-9]\d)?\d{2}))|((0[1-9]|1\d|2[0-8])02((1[6-9]|[2-9]\d)?\d{2}))|(2902((1[6-9]|[2-9]\d)?(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00)|00))))$/;
            var errorMessage = 'Enter Valid Date dd/mm/yyyy';

            if (document.getElementById("<%=txtReceiptDate.ClientID%>").value.match(RegExPattern)) {
                if (len.length == 8) {
                    alert(errorMessage);
                    document.getElementById("<%=txtReceiptDate.ClientID%>").value = "";
                    document.getElementById("<%=txtReceiptDate.ClientID%>").focus();
                    return false;
                }
            }
            else {
                alert(errorMessage);
                document.getElementById("<%=txtReceiptDate.ClientID%>").value = "";
                document.getElementById("<%=txtReceiptDate.ClientID%>").focus();
                return false;
            }
            var str1 = document.getElementById("<%=txtReceiptDate.ClientID %>").value;
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
                document.getElementById("<%=txtReceiptDate.ClientID%>").value = "";
                document.getElementById("<%=txtReceiptDate.ClientID%>").focus();
                //"");
                return false;
            }
            var str1 = document.getElementById("<%=txtReceiptDate.ClientID %>").value;
            var str2 = document.getElementById("<%=txtBatchDate.ClientID %>").value;
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
                document.getElementById("<%=txtReceiptDate.ClientID%>").value = "";
                document.getElementById("<%=txtReceiptDate.ClientID%>").focus();
                //"");
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

            if (document.getElementById("<%=txtBatchId.ClientID%>").value == "") {
                alert("Please Select Record");
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
</atlas:UpdatePanel>
<atlas:UpdateProgress ID="ProgressIndicator" runat="server">
    <ProgressTemplate>
        Loading the data, please wait... 
        <asp:Image ID="LoadingImage" ImageAlign="AbsMiddle" runat="server" ImageUrl="~/Images/spinner.gif" />        
    </ProgressTemplate>
 </atlas:UpdateProgress>
<atlas:UpdatePanel ID="up2" runat="server">
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
                                <asp:ImageButton ID="ibtnSave" runat="server" ImageUrl="~/images/save.png" ToolTip="Save" Width="24px" />
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
                            <!--<asp:ImageButton ID="down" runat="server" ImageUrl="~/images/down.png" ToolTip="Select"/>-->
                </td>
            </tr>
        </table>
        </td>
        <td>
            <table style="border-collapse: collapse;" class="menuoff" onmouseover="className='menuon';"
                onmouseout="className='menuoff';">
                <tr>
                    <td>
                        <asp:ImageButton ID="ibtnPrint" runat="server" ImageUrl="~/images/print.png" ToolTip="Print"/>
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
        </tr> </table>
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
                    <asp:Label ID="lblMenuName" runat="server" Text="University Fund" Width="258px"></asp:Label>
                </td>
            </tr>
        </table>
    </asp:Panel>
    <%-- <asp:Button ID="btnAllocate" runat="server" Text="Allocate" Visible="False" />--%>
    <asp:Panel ID="Panel2" runat="server" Width="100%" Height="100%">
        <table style="width: 100%">
            <tr>
                <td style="width: 99%">
                    <div  style="border: thin solid #A6D9F4; width: 100%">
                        <asp:Panel ID="Panel3" runat="server" Width="125px">
                            <table width="100%">
                                <tr>
                                    <td style="height: 27px; width: 39%;">
                                    </td>
                                    <td style="width: 13%; height: 27px">
                                    </td>
                                    <td colspan="5" style="height: 27px; text-align: center">
                                        <asp:HiddenField ID="today" runat="server" />
                                        <asp:HiddenField ID="lblStatus" runat="server" />
                                        <asp:Label ID="lblMsg" runat="server" CssClass="lblError" Style="text-align: center"
                                            Width="348px"></asp:Label>
                                    </td>
                                    <td colspan="1" style="width: 378px; height: 27px">
                                    </td>
                                    <td colspan="1" style="width: 229px; height: 27px">
                                    </td>
                                </tr>
                              
                                <tr>
                                    <td style="height: 25px; width: 39%;">
                                        <span style="color: #ff0000">*</span></td>
                                    <td style="width: 13%; height: 25px;">
                                        <asp:Label ID="Label1" runat="server" Text="Receipt For" Width="97px"></asp:Label>
                                    </td>
                                    <td style="width: 16%; height: 25px;">
                                        <span style="color: #ff0000">
                                            <asp:DropDownList ID="ddlReceiptFor" runat="server" Width="149px" AutoPostBack="True">
                                                <asp:ListItem Value="-1">--Select--</asp:ListItem>
                                                <asp:ListItem Value="St">Student</asp:ListItem>
                                                <asp:ListItem Value="Sp">Sponsor</asp:ListItem>
                                                <asp:ListItem Value="Sl">Student Loan</asp:ListItem>
                                            </asp:DropDownList>
                                            </span>
                                    &nbsp;</td>
                                    <td style="width: 534px; height: 25px; text-align: right">
                                        <span style="color: #ff0000">*</span></td>
                                    <td style="width: 193px; text-align: left; height: 25px;">
                                        <asp:Label ID="Label22" runat="server" Text="Batch ID" Width="97px"></asp:Label>
                                    </td>
                                    <td colspan="2" style="height: 25px">
                                        <span style="color: #ff0000">
                                            <asp:TextBox ID="txtBatchId" runat="server" MaxLength="20" Width="143px" ReadOnly="True"></asp:TextBox></span>&nbsp;</td>
                                    <td style="width: 378px;" rowspan="5">
                                        <asp:Label ID="Status" runat="server" Font-Bold="False" Font-Size="Larger" Text="Status"
                                            Width="64px" Visible="False"></asp:Label>
                                        <asp:ImageButton ID="ibtnStatus" runat="server" Enabled="False" ImageUrl="~/images/NotReady.gif"
                                            CssClass="cursor" />
                                    </td>
                                    <td style="width: 229px; height: 25px;">
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 39%; height: 25px">
                                        <span style="color: #ff0000">*</span></td>
                                    <td style="width: 13%; height: 25px">
                                        <asp:Label ID="Label8" runat="server" Text="Payment Mode" Width="97px"></asp:Label>
                                    </td>
                                    <td style="width: 16%; height: 25px">
                                        <asp:DropDownList ID="ddlPaymentMode" runat="server" Width="149px" AppendDataBoundItems="True" AutoPostBack="true">
                                            <asp:ListItem Value="-1">--Select--</asp:ListItem>
                                        </asp:DropDownList>
                                        &nbsp;<asp:TextBox ID="txtSpnCode" runat="server" MaxLength="20" Visible="False" Width="147px"></asp:TextBox>
                                    </td>
                                    <td style="width: 534px; height: 25px; text-align: right">
                                        <span style="color: #ff0000">*</span></td>
                                    <td style="width: 193px; height: 25px; text-align: left">
                                        <asp:Label ID="Label23" runat="server" Text="Batch Date" Width="55px" Style="text-align: left"></asp:Label>
                                    </td>
                                    <td style="width: 33px; height: 25px">
                                        <span style="color: #ff0000">
                                            <asp:TextBox ID="txtBatchDate" runat="server" MaxLength="10" Width="94px"></asp:TextBox></span>
                                    &nbsp;</td>
                                    <td style="width: 417px; height: 25px">
                                        <asp:Image ID="ibtnBatchDate" runat="server" ImageUrl="~/images/cal.gif" Visible="false" />
                                    </td>
                                    <td style="width: 229px; height: 25px">
                                    </td>
                                </tr>
                                <tr>
                                    <td style="height: 25px; width: 39%;">
                                        <span style="color: #ff0000">*</span></td>
                                    <td style="width: 13%; height: 25px">
                                        <asp:Label ID="Label10" runat="server" Text="Bank Code" Width="97px"></asp:Label>
                                    </td>
                                    <td style="width: 16%; height: 25px">
                                        <span style="color: #ff0000">
                                            <asp:DropDownList ID="ddlBankCode" runat="server" AppendDataBoundItems="True" TabIndex="11"
                                                Width="149px">
                                                <asp:ListItem Value="-1">--Select--</asp:ListItem>
                                            </asp:DropDownList>
                                            &nbsp;<asp:Image ID="imgbankCode" runat="server" Height="16px" ImageUrl="~/images/find_img.png"
                                                Width="16px" Visible="False" /></span>
                                    </td>
                                    <td style="width: 534px; height: 25px; text-align: right">
                                        <span style="color: #ff0000">*</span></td>
                                    <td style="width: 193px; height: 25px; text-align: left">
                                        <asp:Label ID="Label7" runat="server" Text="Transaction Date" Width="84px" Style="text-align: left"></asp:Label>
                                    </td>
                                    <td style="width: 33px; height: 25px">
                                        <asp:TextBox ID="txtReceiptDate" runat="server" MaxLength="10" Width="95px"></asp:TextBox>&nbsp;</td>
                                    <td style="width: 417px; height: 25px">
                                        <asp:Image ID="ibtnRecDate" runat="server" ImageUrl="~/images/cal.gif" Visible="false" />
                                    </td>
                                    <td style="width: 229px; height: 25px">
                                    </td>
                                </tr>
                                <tr>
                                    <td style="height: 25px; width: 39%;">
                                    </td>
                                    <td style="width: 13%; height: 25px">
                                        <asp:Label ID="Label21" runat="server" Text="Reference No" Width="97px"></asp:Label>
                                    </td>
                                    <td style="width: 16%; height: 25px">
                                        <span style="color: #ff0000">
                                            <asp:TextBox ID="txtReferenceNo" runat="server" MaxLength="20" Width="144px"></asp:TextBox></span>
                                    </td>
                                    <td style="width: 534px; height: 25px; text-align: left">
                                        <span style="color: #ff0000">&nbsp; *</span></td>
                                    <td style="width: 193px; height: 25px; text-align: left">
                                        <span style="width: 193px; height: 25px; text-align: left;">
                                            <asp:Label ID="Label9" runat="server" Text="Total Amount" Width="85px"></asp:Label></span>
                                    </td>
                                    <td style="width: 33px; height: 25px">
                                        <asp:TextBox ID="txtAllocateAmount" runat="server" MaxLength="20" Width="96px" Style="text-align: right"
                                            Height="15px" ReadOnly="True"></asp:TextBox>&nbsp;</td>
                                    <td style="width: 417px; height: 25px">
                                        <%-- <asp:Button ID="btnAllocate" runat="server" Text="Allocate" Visible="False" />--%>
                                    </td>
                                    <td style="width: 229px; height: 25px">
                                    </td>
                                </tr>
                                <tr>
                                    <td style="height: 25px; width: 39%;">
                                        <span style="color: #ff0000">*</span></td>
                                    <td style="width: 13%; height: 25px">
                                        <asp:Label ID="Label20" runat="server" Text="Description" Width="97px"></asp:Label>
                                    </td>
                                    <td style="width: 16%; height: 25px">
                                        <span style="color: #ff0000">
                                            <asp:TextBox ID="txtDescription" runat="server" MaxLength="50" Width="300px"></asp:TextBox></span>
                                    &nbsp;</td>
                                    <td style="width: 534px; height: 25px; text-align: right">
                                    </td>
                                    <td style="width: 193px; height: 25px; text-align: left">
                                        <asp:Label ID="lblStuSpn" runat="server" Text="Select Student" Width="95px"></asp:Label>
                                    </td>
                                    <td style="width: 33px; height: 25px">
                                        <span style="color: #ff0000">
                                            <asp:ImageButton ID="IdtnStud" runat="server" Height="16px" ImageUrl="~/images/find_img.png"
                                                Width="16px" /></span>
                                    </td>
                                    <td style="width: 417px; height: 25px">
                                        <span style="color: #ff0000"></span>
                                    </td>
                                    <td style="width: 229px; height: 25px">
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 39%; height: 25px;">
                                    </td>
                                    <td style="width: 13%; height: 25px;">
                                        <asp:Label ID="Label19" runat="server" Text="Other No" Width="97px" Visible="False"></asp:Label>
                                    </td>
                                    <td style="width: 16%; height: 25px;">
                                        <span style="color: #ff0000">
                                            <asp:TextBox ID="txtOtherNo" runat="server" MaxLength="20" Width="140px" Visible="False"></asp:TextBox></span>
                                    </td>
                                    <td style="width: 534px; text-align: right; height: 25px;">
                                    </td>
                                    <td style="width: 193px; text-align: left; height: 25px;">
                                        &nbsp;</td>
                                    <td style="width: 33px; height: 25px;">
                                        &nbsp;</td>
                                    <td style="width: 417px; height: 25px;">
                                    </td>
                                    <td style="width: 378px; height: 25px;">
                                        <asp:Label ID="lblStatus1" runat="server" Font-Bold="False" Font-Size="Larger" Text="New"
                                            Width="120px" Visible="False"></asp:Label>
                                    </td>
                                    <td style="width: 229px; height: 25px;">
                                    </td>
                                </tr>
                            </table>
                            <br />
                            <asp:Panel ID="Panel4" runat="server" Height="100%" Width="759px" Visible="False">
                                &nbsp;
                                <br />
                                <table style="width: 100%; height: 9px">
                                    <tr>
                                        <td style="width: 5px; height: 12px">
                                        </td>
                                        <td >
                                            <asp:Label ID="Label15" runat="server" Text="File Upload" Width="97px"></asp:Label>
                                             <asp:Button ID="btnUpload" runat="server" Text="Upload" /><asp:Button ID="Btnselect"
                                                runat="server" OnClick="Btnselect_Click" Text="Select" Visible="False" />
                                        </td>
                                        <td colspan="2" align="left">
                                        </td>
                                        <td style="width: 867px; height: 12px">
                                        </td>
                                        <td style="width: 67px; height: 12px">
                                        </td>
                                        <td style="width: 100px; height: 12px">
                                            <br />
                                            <br />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 5px; height: 4px">
                                        </td>
                                        <td colspan="3">
                                            <table>
                                                <tr>
                                                    <td>
                                                        <asp:Button ID="btnReceipt" runat="server" CssClass="TabButton" Height="24px" Text="Receipt"
                                                            Width="108px" OnClick="btnReceipt_Click1" />
                                                    </td>
                                                    <td>
                                                        <asp:Button ID="btnSelection" runat="server" CssClass="TabButton" Height="25px" Text="Allocation"
                                                            Width="108px" OnClick="btnSelection_Click1" />
                                                    </td>
                                                    <td>
                                                        <asp:Button ID="btnInactive" runat="server" CssClass="TabButton" Height="25px" Text="Inactive"
                                                            Width="108px" OnClick="btnInactive_Click1" />
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                        <td style="width: 867px; height: 4px">
                                            <asp:FileUpload ID="FLBankFile" runat="server" Width="306px" Visible="False" />
                                        </td>
                                        <td style="width: 67px; height: 4px">
                                        </td>
                                        <td style="width: 100px; height: 4px">
                                        </td>
                                    </tr>
                                </table>
                            </asp:Panel>
                        </asp:Panel>
                        <asp:MultiView ID="MultiView1" runat="server">
                            <asp:View ID="View2" runat="server">
                                <asp:Panel ID="pnlStudentGrid" runat="server" Height="100%" Width="100%">
                                    <table width="100%">
                                        <tr>
                                            <td style="height: 13px" width="1%">
                                            </td>
                                            <td style="width: 100%; height: 13px">
                                                <asp:DataGrid ID="dgInvoices" runat="server" AutoGenerateColumns="False" DataKeyField="TransactionCode"
                                                    Width="100%" OnSelectedIndexChanged="dgInvoices_SelectedIndexChanged">
                                                    <FooterStyle CssClass="dgFooterStyle" Height="20px" Font-Bold="False" Font-Italic="False"
                                                        Font-Overline="False" Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Right" />
                                                    <SelectedItemStyle CssClass="dgSelectedItemStyle" />
                                                    <AlternatingItemStyle BackColor="Beige" CssClass="dgAlternatingItemStyle" Font-Bold="False"
                                                        Font-Italic="False" Font-Overline="False" Font-Strikeout="False" Font-Underline="False" />
                                                    <ItemStyle CssClass="dgItemStyle" />
                                                    <HeaderStyle BackColor="#CDD7EE" CssClass="dgHeaderStyle" Font-Bold="True" Font-Italic="False"
                                                        Font-Overline="False" Font-Size="Medium" Font-Strikeout="False" Font-Underline="False" />
                                                    <Columns>
                                                        <asp:TemplateColumn HeaderText="Select">
                                                            <ItemTemplate>
                                                                <asp:CheckBox ID="cbInvoice" runat="server" AutoPostBack="True" OnCheckedChanged="cbInvoice_CheckedChanged" />
                                                            </ItemTemplate>
                                                        </asp:TemplateColumn>
                                                        <asp:BoundColumn DataField="TransactionCode" HeaderText="Document No"></asp:BoundColumn>
                                                        <asp:BoundColumn DataField="TransDate" HeaderText="Date" DataFormatString="{0:dd/MM/yyyy}">
                                                        </asp:BoundColumn>
                                                        <asp:BoundColumn HeaderText="Due Date" DataField="DueDate" DataFormatString="{0:dd/MM/yyyy}">
                                                        </asp:BoundColumn>
                                                        <asp:BoundColumn DataField="TransactionAmount" HeaderText="Total Amount" FooterText="Total">
                                                            <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                                                Font-Underline="False" HorizontalAlign="Right" />
                                                            <FooterStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                                                Font-Underline="False" HorizontalAlign="Right" />
                                                        </asp:BoundColumn>
                                                        <asp:TemplateColumn HeaderText="Allocated Amount ">
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="AllovateAmount" runat="server" Style="text-align: right" Width="120px"
                                                                    OnTextChanged="AllovateAmount_TextChanged" AutoPostBack="True"></asp:TextBox>
                                                            </ItemTemplate>
                                                            <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                                                Font-Underline="False" HorizontalAlign="Right" />
                                                            <HeaderStyle Width="15%" Font-Bold="True" Font-Italic="False" Font-Overline="False"
                                                                Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Center" />
                                                            <FooterStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                                                Font-Underline="False" HorizontalAlign="Right" />
                                                        </asp:TemplateColumn>
                                                        <asp:BoundColumn HeaderText="Outstanding Amount" Visible="False">
                                                            <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                                                Font-Underline="False" HorizontalAlign="Right" />
                                                            <FooterStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                                                Font-Underline="False" HorizontalAlign="Right" />
                                                            <HeaderStyle Font-Bold="True" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                                                Font-Underline="False" HorizontalAlign="Center" />
                                                        </asp:BoundColumn>
                                                        <asp:TemplateColumn HeaderText="BAmt" Visible="False">
                                                            <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                                                Font-Underline="False" HorizontalAlign="Right" />
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="txtPAmount" runat="server" Width="120px"></asp:TextBox>
                                                            </ItemTemplate>
                                                            <HeaderStyle Font-Bold="True" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                                                Font-Underline="False" HorizontalAlign="Center" />
                                                        </asp:TemplateColumn>
                                                        <asp:BoundColumn DataField="PaidAmount" HeaderText="PaidAmount"></asp:BoundColumn>
                                                        <asp:BoundColumn DataField="TempPaidAmount" HeaderText="TempAmount" Visible="False">
                                                        </asp:BoundColumn>
                                                        <asp:BoundColumn DataField="CreditRef" HeaderText="matricno" Visible="False"></asp:BoundColumn>
                                                        <asp:BoundColumn DataField="ReferenceCode" HeaderText="refcode" Visible="False">
                                                        </asp:BoundColumn>
                                                        <asp:BoundColumn DataField="TaxAmount" HeaderText="TaxAmount" Visible="False"></asp:BoundColumn>
                                                        <asp:BoundColumn DataField="TempAmount" HeaderText="TempAmount" Visible="False">
                                                        </asp:BoundColumn>
                                                    </Columns>
                                                </asp:DataGrid>
                                            </td>
                                            <td style="width: 101%; height: 13px">
                                            </td>
                                            <td style="height: 13px; width: 30%;">
                                            </td>
                                            <td style="height: 13px">
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="height: 16px" width="1%">
                                            </td>
                                            <td style="width: 101%; height: 16px; text-align: right">
                                            </td>
                                            <td style="width: 101%; height: 16px">
                                            </td>
                                            <td style="width: 30%; height: 16px">
                                            </td>
                                            <td style="height: 16px">
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="height: 16px" width="1%">
                                            </td>
                                            <td style="width: 101%; height: 16px; text-align: right">
                                            </td>
                                            <td style="width: 101%; height: 16px">
                                            </td>
                                            <td style="width: 30%; height: 16px">
                                            </td>
                                            <td style="height: 16px">
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="height: 16px" width="1%">
                                            </td>
                                            <td style="width: 101%; height: 16px; text-align: right;">
                                                <asp:TextBox ID="txtALLAmount" runat="server" Height="14px" BackColor="Transparent"
                                                    BorderColor="Transparent" ForeColor="White" BorderStyle="None"></asp:TextBox>
                                                <asp:Label ID="Label2" runat="server" Text="Amount to be Allocated" Width="114px"
                                                    Height="15px"></asp:Label>
                                                <asp:TextBox ID="txtStuIndex" runat="server" Width="93px" Style="text-align: right"
                                                    Height="14px" ReadOnly="True"></asp:TextBox>&nbsp;
                                            </td>
                                            <td style="width: 101%; height: 16px">
                                            </td>
                                            <td style="width: 30%; height: 16px">
                                            </td>
                                            <td style="height: 16px">
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="height: 6px" width="1%">
                                            </td>
                                            <td style="width: 101%; height: 2px; text-align: right">
                                            </td>
                                            <td style="width: 101%; height: 6px; text-align: right">
                                            </td>
                                            <td style="width: 30%; height: 6px; text-align: right">
                                            </td>
                                            <td style="height: 6px">
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="height: 6px" width="1%">
                                            </td>
                                            <td style="width: 101%; height: 6px; text-align: right">
                                                <asp:Label ID="lblTotal" runat="server" Text="Total   " Width="142px"></asp:Label>
                                                &nbsp; &nbsp; &nbsp;
                                                <asp:TextBox ID="txtTotalPenAmt" runat="server" Height="15px" MaxLength="20" ReadOnly="True"
                                                    Style="text-align: right" Width="100px">0.00</asp:TextBox>
                                                &nbsp; &nbsp; &nbsp;&nbsp;<asp:TextBox ID="txtAddedAmount" runat="server" Height="15px"
                                                    MaxLength="20" Width="121px" Style="text-align: right" ReadOnly="True">0.00</asp:TextBox>
                                                &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;&nbsp; &nbsp;<asp:TextBox ID="txtAfterBalance"
                                                    runat="server" Height="15px" MaxLength="20" ReadOnly="True" Style="text-align: right"
                                                    Width="121px">0.00</asp:TextBox>
                                                <asp:Button ID="bTnUpdate" runat="server" Enabled="False" OnClick="bTnUpdate_Click"
                                                    Text="Update" />
                                                &nbsp;
                                                <asp:Button ID="bTnClose" runat="server" OnClick="bTnClose_Click" Text="Close" />
                                            </td>
                                            <td style="width: 101%; height: 6px; text-align: right">
                                            </td>
                                            <td style="height: 6px; text-align: right; width: 30%;">
                                            </td>
                                            <td style="height: 6px">
                                            </td>
                                        </tr>
                                    </table>
                                </asp:Panel>
                            </asp:View>
                            <asp:View ID="View1" runat="server">
                                <asp:Panel ID="bankPanel" runat="server" Height="100%" Width="100%">
                                    <table width="100%">
                                        <tr>
                                            <td style="height: 4px" width="1%">
                                            </td>
                                            <td style="width: 101%; height: 4px">
                                                &nbsp;<asp:CheckBox ID="chkselectall" runat="server" AutoPostBack="True" OnCheckedChanged="chkselectall_CheckedChanged"
                                                    Visible="false" />
                                            </td>
                                            <td style="width: 101%; height: 4px">
                                            </td>
                                            <td style="height: 4px; width: 30%;">
                                            </td>
                                            <td style="height: 4px; width: 3px;">
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="height: 16px" width="1%">
                                            </td>
                                            <td style="width: 101%; height: 16px">
                                                <asp:DataGrid ID="dgStudentView" runat="server" AutoGenerateColumns="False" Width="100%"
                                                    OnSelectedIndexChanged="dgStudentView_SelectedIndexChanged" DataKeyField="ICNo">
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
                                                                <asp:CheckBox ID="chk" Checked="true" runat="server" AutoPostBack="True" OnCheckedChanged="chk_CheckedChanged" />
                                                            </ItemTemplate>
                                                        </asp:TemplateColumn>
                                                        <asp:ButtonColumn DataTextField="MatricNO" HeaderText="Matric No" Text="MatricNO"
                                                            CommandName="Select"></asp:ButtonColumn>
                                                        
                                                        <asp:BoundColumn DataField="MatricNO" HeaderText="Matric No" Visible="False"></asp:BoundColumn>
                                                        <asp:BoundColumn DataField="StudentName" HeaderText="Name"></asp:BoundColumn>
                                                        <asp:BoundColumn DataField="ICNo" HeaderText="IC No"></asp:BoundColumn>
                                                        <asp:BoundColumn DataField="Faculty" HeaderText="Faculty" Visible="False"></asp:BoundColumn>
                                                        <asp:BoundColumn DataField="ProgramID" HeaderText="Program" Visible="False"></asp:BoundColumn>
                                                        <asp:BoundColumn DataField="CurrentSemester" HeaderText="Semester"></asp:BoundColumn>
                                                        <asp:TemplateColumn HeaderText="Auto/Manual  ">
                                                            <ItemTemplate>
                                                                <asp:CheckBox ID="chkManual" runat="server" Text="Manual" OnCheckedChanged="chkManual_CheckedChanged"
                                                                    AutoPostBack="True" />
                                                            </ItemTemplate>
                                                        </asp:TemplateColumn>
                                                        <asp:TemplateColumn HeaderText="Amount">
                                                            <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                                                Font-Underline="False" HorizontalAlign="Left" />
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="TxtAmt" runat="server" AutoPostBack="True" OnTextChanged="TxtAmt_TextChanged"
                                                                    Style="text-align: right"></asp:TextBox>
                                                            </ItemTemplate>
                                                            <HeaderStyle Font-Bold="True" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                                                Font-Underline="False" HorizontalAlign="Left" />
                                                        </asp:TemplateColumn>
                                                        <asp:BoundColumn DataField="TransactionAmount" HeaderText="Amount" Visible="False">
                                                        </asp:BoundColumn>
                                                        <asp:BoundColumn DataField="StuIndex" HeaderText="id" Visible="False"></asp:BoundColumn>
                                                        <asp:BoundColumn DataFormatString="{0:F}" HeaderText="Allocated Amount" DataField="PaidAmount">
                                                        </asp:BoundColumn>
                                                        <asp:BoundColumn DataField="StManual" HeaderText="AM" Visible="False"></asp:BoundColumn>
                                                        <asp:BoundColumn DataField="SubReferenceTwo" HeaderText="subreference2" Visible="False">
                                                        </asp:BoundColumn>
                                                        <asp:BoundColumn DataField="noAkaun" HeaderText="noAkaun" Visible="False">
                                                        </asp:BoundColumn>
                                                         
                                                        <asp:TemplateColumn HeaderText="Bank Slip No">
                                                            <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                                                Font-Underline="False" HorizontalAlign="Left" />
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="txtBankInSlip" runat="server" OnTextChanged="txtBankInSlip_TextChanged"
                                                                      Style="text-align: right"></asp:TextBox>
                                                            </ItemTemplate>
                                                            <HeaderStyle Font-Bold="True" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                                                Font-Underline="False" HorizontalAlign="Left" />
                                                        </asp:TemplateColumn>

                                                        <asp:TemplateColumn HeaderText="Transaction Date">
                                                            <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                                                Font-Underline="False" HorizontalAlign="Left" />
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="txtTransDate" runat="server"  OnTextChanged="txtTransDate_TextChanged"
                                                                     Style="text-align: right" type = "date" ></asp:TextBox>
                                                            </ItemTemplate>
                                                            <HeaderStyle Font-Bold="True" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                                                Font-Underline="False" HorizontalAlign="Left" />
                                                        </asp:TemplateColumn>

                                                        </Columns>
                                                </asp:DataGrid>
                                            </td>
                                            <td style="width: 101%; height: 16px">
                                            </td>
                                            <td style="height: 16px; width: 30%;">
                                            </td>
                                            <td style="height: 16px; width: 3px;">
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="height: 6px" width="1%">
                                            </td>
                                            <td style="width: 101%; height: 6px; text-align: right">
                                                &nbsp;<asp:Label ID="Label12" runat="server" Text="Total   " Visible="False" Width="142px"></asp:Label>
                                                &nbsp; &nbsp;<asp:TextBox ID="TextBox1" runat="server" Height="15px" MaxLength="20"
                                                    ReadOnly="True" Style="text-align: right" Visible="False" Width="100px">0.00</asp:TextBox>
                                                &nbsp; &nbsp; &nbsp;
                                                <asp:TextBox ID="TextBox2" runat="server" Height="15px" MaxLength="20" ReadOnly="True"
                                                    Style="text-align: right" Visible="False" Width="121px">0.00</asp:TextBox>
                                                &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;<asp:TextBox ID="TextBox3" runat="server" Height="15px"
                                                    MaxLength="20" ReadOnly="True" Style="text-align: right" Visible="False" Width="121px">0.00</asp:TextBox>&nbsp;
                                            </td>
                                            <td style="width: 101%; height: 6px; text-align: right">
                                            </td>
                                            <td style="height: 6px; text-align: right; width: 30%;">
                                            </td>
                                            <td style="height: 6px; width: 3px;">
                                            </td>
                                        </tr>
                                    </table>
                                </asp:Panel>
                                <asp:Panel ID="pnlStudentLoan" runat="server" Visible="false" Height="100%" Width="759px"
                                    CssClass="line">
                                    &nbsp; &nbsp;&nbsp;
                                    <br />
                                    <table>
                                        <tr>
                                            <td style="width: 48px; height: 25px">
                                                <asp:Label ID="lblStudentId" runat="server" Text="Matric ID" Width="73px"></asp:Label>
                                            </td>
                                            <td style="width: 145px; height: 25px">
                                                <asp:TextBox ID="txtStudentId" runat="server" ReadOnly="True"></asp:TextBox>
                                            </td>
                                            <td style="width: 81px; height: 25px">
                                                &nbsp;<asp:Label ID="lblStudentName" runat="server" Text="Student Name"></asp:Label>
                                            </td>
                                            <td style="width: 146px; height: 25px">
                                                <asp:TextBox ID="txtStudentName" runat="server" ReadOnly="True"></asp:TextBox>
                                            </td>
                                            <td style="height: 25px">
                                                <asp:Label ID="lblLoanAmount" runat="server" Text="Amount"></asp:Label>
                                            </td>
                                            <td style="width: 3px; height: 25px">
                                                <asp:TextBox ID="txtLoanAmount" runat="server" AutoPostBack="True" Style="text-align: right"
                                                    Width="96px"></asp:TextBox>
                                            </td>
                                            <td style="width: 92px; height: 25px" align="right">
                                              <asp:Label ID="lblLoanAmountToPay" runat="server" Text="0.00" 
                                                    style="font-size: small; font-weight: 700; color: #FF0000"></asp:Label>
                                            </td>
                                        </tr>
                                    </table>
                                </asp:Panel>
                                <asp:Panel ID="pnlReceiptsp" runat="server" Height="100%" Width="759px">
                                    &nbsp; &nbsp;&nbsp;
                                    <br />
                                    <table>
                                        <tr>
                                            <td style="width: 48px; height: 25px">
                                                <asp:Label ID="Label4" runat="server" Text="Sponsor Code" Width="73px"></asp:Label>
                                            </td>
                                            <td style="width: 145px; height: 25px">
                                                <asp:TextBox ID="txtSponCode" runat="server" ReadOnly="True"></asp:TextBox>
                                            </td>
                                            <td style="width: 81px; height: 25px">
                                                &nbsp;<asp:Label ID="Label24" runat="server" Text="Sponsor Name"></asp:Label>
                                            </td>
                                            <td style="width: 146px; height: 25px">
                                                <asp:TextBox ID="txtSponName" runat="server" ReadOnly="True"></asp:TextBox>
                                            </td>
                                            <td style="height: 25px">
                                                <asp:Label ID="Label27" runat="server" Text="Amount"></asp:Label>
                                            </td>
                                            <td style="width: 3px; height: 25px">
                                                <asp:TextBox ID="txtSpnAmount" runat="server" AutoPostBack="True" OnTextChanged="txtSpnAmount_TextChanged"
                                                    Style="text-align: right" Width="96px"></asp:TextBox>
                                            </td>
                                            <td style="width: 92px; height: 25px">
                                            </td>
                                        </tr>
                                    </table>
                                </asp:Panel>
                            </asp:View>
                            <asp:View ID="View3" runat="server">
                                <table width="100%">
                                    <tr>
                                        <td style="height: 16px" width="1%">
                                        </td>
                                        <td style="width: 101%; height: 16px">
                                        </td>
                                        <td style="width: 101%; height: 16px">
                                        </td>
                                        <td style="height: 16px; width: 30%;">
                                        </td>
                                        <td style="height: 16px">
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="height: 16px" width="1%">
                                        </td>
                                        <td style="width: 101%; height: 16px; text-align: left;">
                                            <asp:DataGrid ID="dgUnStudent" runat="server" AutoGenerateColumns="False" Width="100%"
                                                OnSelectedIndexChanged="dgUnStudent_SelectedIndexChanged" DataKeyField="MatricNO">
                                                <FooterStyle CssClass="dgFooterStyle" Height="20px" />
                                                <SelectedItemStyle CssClass="dgSelectedItemStyle" />
                                                <AlternatingItemStyle BackColor="Beige" CssClass="dgAlternatingItemStyle" Font-Bold="False"
                                                    Font-Italic="False" Font-Overline="False" Font-Strikeout="False" Font-Underline="False" />
                                                <ItemStyle CssClass="dgItemStyle" Font-Bold="False" Font-Italic="False" Font-Overline="False"
                                                    Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Left" />
                                                <HeaderStyle BackColor="#CDD7EE" CssClass="dgHeaderStyle" Font-Bold="True" Font-Italic="False"
                                                    Font-Overline="False" Font-Size="Medium" Font-Strikeout="False" Font-Underline="False"
                                                    HorizontalAlign="Left" />
                                                <Columns>
                                                    <asp:TemplateColumn HeaderText="Select" Visible="False">
                                                        <ItemTemplate>
                                                            <asp:CheckBox ID="chk" runat="server" />
                                                        </ItemTemplate>
                                                    </asp:TemplateColumn>
                                                    <asp:ButtonColumn DataTextField="MatricNO" HeaderText="Matric No" Text="MatricNO"
                                                        CommandName="Select"></asp:ButtonColumn>
                                                    <asp:BoundColumn DataField="MatricNO" HeaderText="Matric No" Visible="False"></asp:BoundColumn>
                                                    <asp:BoundColumn DataField="StudentName" HeaderText="Name"></asp:BoundColumn>
                                                    <asp:BoundColumn DataField="ICNo" HeaderText="IC No"></asp:BoundColumn>
                                                    <asp:BoundColumn HeaderText="Faculty" Visible="False"></asp:BoundColumn>
                                                    <asp:BoundColumn HeaderText="Program" Visible="False"></asp:BoundColumn>
                                                    <asp:BoundColumn DataField="CurrentSemester" HeaderText="Semester" Visible="False">
                                                    </asp:BoundColumn>
                                                    <asp:TemplateColumn HeaderText="Auto/Manual  " Visible="False">
                                                        <ItemTemplate>
                                                            <asp:CheckBox ID="chkManual" runat="server" Text="Manual  " OnCheckedChanged="chkManual_CheckedChanged" />
                                                        </ItemTemplate>
                                                    </asp:TemplateColumn>
                                                    <asp:TemplateColumn HeaderText="Amount">
                                                        <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                                            Font-Underline="False" HorizontalAlign="Left" />
                                                        <ItemTemplate>
                                                            <asp:TextBox ID="TxtAmt" runat="server" AutoPostBack="True" OnTextChanged="TxtAmt_TextChanged"
                                                                Style="text-align: right"></asp:TextBox>
                                                        </ItemTemplate>
                                                        <HeaderStyle Font-Bold="True" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                                            Font-Underline="False" HorizontalAlign="Left" />
                                                    </asp:TemplateColumn>
                                                    <asp:BoundColumn DataField="TransactionAmount" HeaderText="Amount" Visible="False">
                                                    </asp:BoundColumn>
                                                    <asp:BoundColumn HeaderText="id" Visible="False"></asp:BoundColumn>
                                                    <asp:BoundColumn DataFormatString="{0:F}" HeaderText="Allocated Amount"></asp:BoundColumn>
                                                    <asp:BoundColumn HeaderText="AM" Visible="False"></asp:BoundColumn>
                                                </Columns>
                                            </asp:DataGrid><br />
                                            <asp:Panel ID="pnlunmach" runat="server" Height="50px" Width="125px" Visible="False">
                                                <br />
                                                <asp:Label ID="lblStuerror" runat="server" CssClass="lblError" Style="text-align: center"
                                                    Width="348px"></asp:Label>&nbsp;<table>
                                                        <tr>
                                                            <td style="width: 106px">
                                                            </td>
                                                            <td style="width: 133px">
                                                                <asp:Label ID="Label3" runat="server" Text="Matric No" Width="48px"></asp:Label>
                                                            </td>
                                                            <td style="width: 51px">
                                                                <asp:Label ID="Name" runat="server" Text="Name" Width="31px"></asp:Label>
                                                            </td>
                                                            <td style="width: 100px">
                                                                <asp:Label ID="Label26" runat="server" Text="IC No" Width="27px"></asp:Label>
                                                            </td>
                                                            <td style="width: 67px">
                                                                <asp:Label ID="Amount" runat="server" Text="IC No" Width="27px" Visible="False"></asp:Label>
                                                            </td>
                                                            <td style="width: 100px">
                                                            </td>
                                                            <td style="width: 100px">
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td style="width: 106px">
                                                            </td>
                                                            <td style="width: 133px">
                                                                <asp:TextBox ID="txtMno" runat="server" Width="126px"></asp:TextBox>
                                                            </td>
                                                            <td style="width: 51px">
                                                                <asp:TextBox ID="txtStuName" runat="server" Width="126px"></asp:TextBox>
                                                            </td>
                                                            <td style="width: 100px">
                                                                <asp:TextBox ID="txtICno" runat="server" Width="126px"></asp:TextBox>
                                                            </td>
                                                            <td style="width: 67px">
                                                                <asp:TextBox ID="txtStuAmount" runat="server" Width="126px" Visible="False"></asp:TextBox>
                                                            </td>
                                                            <td style="width: 100px">
                                                            </td>
                                                            <td style="width: 100px">
                                                                <asp:Button ID="BtnSubmit" runat="server" OnClick="BtnSubmit_Click" Text="Submit" />
                                                            </td>
                                                        </tr>
                                                    </table>
                                            </asp:Panel>
                                        </td>
                                        <td style="width: 101%; height: 16px">
                                        </td>
                                        <td style="height: 16px; width: 30%;">
                                        </td>
                                        <td style="height: 16px">
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="height: 6px" width="1%">
                                        </td>
                                        <td style="width: 101%; height: 6px; text-align: right">
                                            <asp:DataGrid ID="DataGrid1" runat="server" AutoGenerateColumns="False" Width="100%"
                                                OnSelectedIndexChanged="dgUnStudent_SelectedIndexChanged" DataKeyField="MatricNO">
                                                <FooterStyle CssClass="dgFooterStyle" Height="20px" />
                                                <SelectedItemStyle CssClass="dgSelectedItemStyle" />
                                                <AlternatingItemStyle BackColor="Beige" CssClass="dgAlternatingItemStyle" Font-Bold="False"
                                                    Font-Italic="False" Font-Overline="False" Font-Strikeout="False" Font-Underline="False" />
                                                <ItemStyle CssClass="dgItemStyle" Font-Bold="False" Font-Italic="False" Font-Overline="False"
                                                    Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Left" />
                                                <HeaderStyle BackColor="#CDD7EE" CssClass="dgHeaderStyle" Font-Bold="True" Font-Italic="False"
                                                    Font-Overline="False" Font-Size="Medium" Font-Strikeout="False" Font-Underline="False"
                                                    HorizontalAlign="Left" />
                                                <Columns>
                                                    <asp:TemplateColumn HeaderText="Select" Visible="False">
                                                        <ItemTemplate>
                                                            <asp:CheckBox ID="chk" runat="server" />
                                                        </ItemTemplate>
                                                    </asp:TemplateColumn>
                                                    <asp:ButtonColumn DataTextField="MatricNO" HeaderText="Matric No" Text="MatricNO"
                                                        CommandName="Select"></asp:ButtonColumn>
                                                    <asp:BoundColumn DataField="MatricNO" HeaderText="Matric No" Visible="False"></asp:BoundColumn>
                                                    <asp:BoundColumn DataField="StudentName" HeaderText="Name"></asp:BoundColumn>
                                                    <asp:BoundColumn DataField="ICNo" HeaderText="IC No"></asp:BoundColumn>
                                                    <asp:BoundColumn HeaderText="Faculty" Visible="False"></asp:BoundColumn>
                                                    <asp:BoundColumn HeaderText="Program" Visible="False"></asp:BoundColumn>
                                                    <asp:BoundColumn DataField="CurrentSemester" HeaderText="Semester" Visible="False">
                                                    </asp:BoundColumn>
                                                    <asp:TemplateColumn HeaderText="Auto/Manual  " Visible="False">
                                                        <ItemTemplate>
                                                            <asp:CheckBox ID="chkManual" runat="server" Text="Manual  " OnCheckedChanged="chkManual_CheckedChanged" />
                                                        </ItemTemplate>
                                                    </asp:TemplateColumn>
                                                    <asp:TemplateColumn HeaderText="Amount">
                                                        <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                                            Font-Underline="False" HorizontalAlign="Left" />
                                                        <ItemTemplate>
                                                            <asp:TextBox ID="TxtAmt" runat="server" AutoPostBack="True" OnTextChanged="TxtAmt_TextChanged"
                                                                Style="text-align: right"></asp:TextBox>
                                                        </ItemTemplate>
                                                        <HeaderStyle Font-Bold="True" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                                            Font-Underline="False" HorizontalAlign="Left" />
                                                    </asp:TemplateColumn>
                                                    <asp:BoundColumn DataField="TransactionAmount" HeaderText="Amount" Visible="False">
                                                    </asp:BoundColumn>
                                                    <asp:BoundColumn HeaderText="id" Visible="False"></asp:BoundColumn>
                                                    <asp:BoundColumn DataFormatString="{0:F}" HeaderText="Allocated Amount"></asp:BoundColumn>
                                                    <asp:BoundColumn HeaderText="AM" Visible="False"></asp:BoundColumn>
                                                </Columns>
                                            </asp:DataGrid>
                                        </td>
                                        <td style="width: 101%; height: 6px; text-align: right">
                                        </td>
                                        <td style="width: 30%; height: 6px; text-align: right">
                                        </td>
                                        <td style="height: 6px">
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="height: 6px" width="1%">
                                        </td>
                                        <td style="width: 101%; height: 6px; text-align: right">
                                            &nbsp;<asp:Label ID="Label25" runat="server" Text="Total   " Visible="False" Width="142px"></asp:Label>
                                            &nbsp; &nbsp;<asp:TextBox ID="TextBox4" runat="server" Height="15px" MaxLength="20"
                                                ReadOnly="True" Style="text-align: right" Visible="False" Width="100px">0.00</asp:TextBox>
                                            &nbsp; &nbsp; &nbsp;
                                            <asp:TextBox ID="TextBox5" runat="server" Height="15px" MaxLength="20" ReadOnly="True"
                                                Style="text-align: right" Visible="False" Width="121px">0.00</asp:TextBox>
                                            &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;<asp:TextBox ID="TextBox6" runat="server" Height="15px"
                                                MaxLength="20" ReadOnly="True" Style="text-align: right" Visible="False" Width="121px">0.00</asp:TextBox>&nbsp;
                                        </td>
                                        <td style="width: 101%; height: 6px; text-align: right">
                                        </td>
                                        <td style="height: 6px; text-align: right; width: 30%;">
                                        </td>
                                        <td style="height: 6px">
                                        </td>
                                    </tr>
                                </table>
                            </asp:View>
                        </asp:MultiView><br />
                        <br />
                        <br />
                        &nbsp;<asp:HiddenField ID="htxtCat" runat="server" Value="All" />
                    </div>
                </td>
            </tr>
        </table>
    </asp:Panel>
     <asp:Button ID="btnHidden" runat="Server" OnClick="btnHidden_Click"  style="display:none" />
    <br />
    <br />
    <br />
    <br />
    <br />
    <%--</ContentTemplate>     
</atlas:UpdatePanel>--%>
</asp:Content>
