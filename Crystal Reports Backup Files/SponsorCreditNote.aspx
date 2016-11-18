<%@ Page Language="VB" MasterPageFile="~/MasterPage3.master" AutoEventWireup="false" MaintainScrollPositionOnPostback="true"
    CodeFile="SponsorCreditNote.aspx.vb" Inherits="SponsorCreditNote" Title="Untitled Page" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <script language="javascript" type="Scripts/popcalendar.js"></script>
    <script language="javascript" type="Scripts/functions"></script>
    <script language="javascript" type="text/javascript">
        function getconfirm() {
            if (document.getElementById("<%=lblStatus.ClientID%>").value == "Posted") {
                alert("Posted Record Cannot be Deleted");
                return false;
            }
            if (document.getElementById("<%=lblStatus.ClientID%>").value == "New") {
                alert("Select Record to Delete");
                return false;
            }
            if (document.getElementById("<%=txtBatchNo.ClientID%>").value == "") {
                alert("Select Record to Delete");
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
        function Validate() {
            var re = /\s*((\S+\s*)*)/;
            if (document.getElementById("<%=txtBatchNo.ClientID%>").value.replace(re, "$1").length == 0) {
                alert("Batch No Field Cannot Be Blank");
                document.getElementById("<%=txtBatchNo.ClientID%>").focus();
                return false;
            }

            if (document.getElementById("<%=txtBatchDate.ClientID%>").value.replace(re, "$1").length == 0) {
                alert("Batch Date Field Cannot Be Blank");
                document.getElementById("<%=txtBatchDate.ClientID%>").focus();
                return false;
            }
            if (document.getElementById("<%=txtInvoiceDate.ClientID%>").value.replace(re, "$1").length == 0) {
                alert("Transaction Date Field Cannot Be Blank");
                document.getElementById("<%=txtInvoiceDate.ClientID%>").focus();
                return false;
            }
            if (document.getElementById("<%=txtDesc.ClientID%>").value.replace(re, "$1").length == 0) {
                alert("Description Field Cannot Be Blank");
                document.getElementById("<%=txtDesc.ClientID%>").focus();
                return false;
            }
            if (document.getElementById("<%=txtTotal.ClientID%>").value.replace(re, "$1").length == 0) {
                alert("Total Amount Field Cannot Be Blank");
                document.getElementById("<%=txtTotal.ClientID%>").focus();
                return false;
            }

            //txtBatchDate---------------------------------------------------------------------------
            var len = document.getElementById("<%=txtBatchDate.ClientID%>").value
            var RegExPattern = /^((((0?[1-9]|[12]\d|3[01])[\.\-\/](0?[13578]|1[02])[\.\-\/]((1[6-9]|[2-9]\d)?\d{2}))|((0?[1-9]|[12]\d|30)[\.\-\/](0?[13456789]|1[012])[\.\-\/]((1[6-9]|[2-9]\d)?\d{2}))|((0?[1-9]|1\d|2[0-8])[\.\-\/]0?2[\.\-\/]((1[6-9]|[2-9]\d)?\d{2}))|(29[\.\-\/]0?2[\.\-\/]((1[6-9]|[2-9]\d)?(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00)|00)))|(((0[1-9]|[12]\d|3[01])(0[13578]|1[02])((1[6-9]|[2-9]\d)?\d{2}))|((0[1-9]|[12]\d|30)(0[13456789]|1[012])((1[6-9]|[2-9]\d)?\d{2}))|((0[1-9]|1\d|2[0-8])02((1[6-9]|[2-9]\d)?\d{2}))|(2902((1[6-9]|[2-9]\d)?(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00)|00))))$/;
            var errorMessage = 'Please enter Valid Batch Date in dd/mm/yyyy format.';

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
                alert("Batch Date Cannot Be Greater Than Current/Transaction Date");
                document.getElementById("<%=txtBatchDate.ClientID%>").value = "";
                document.getElementById("<%=txtBatchDate.ClientID%>").focus();
                //"");
                return false;
            }







            //txtInvoiceDate---------------------------------------------------------------------------
            var len = document.getElementById("<%=txtInvoiceDate.ClientID%>").value
            var RegExPattern = /^((((0?[1-9]|[12]\d|3[01])[\.\-\/](0?[13578]|1[02])[\.\-\/]((1[6-9]|[2-9]\d)?\d{2}))|((0?[1-9]|[12]\d|30)[\.\-\/](0?[13456789]|1[012])[\.\-\/]((1[6-9]|[2-9]\d)?\d{2}))|((0?[1-9]|1\d|2[0-8])[\.\-\/]0?2[\.\-\/]((1[6-9]|[2-9]\d)?\d{2}))|(29[\.\-\/]0?2[\.\-\/]((1[6-9]|[2-9]\d)?(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00)|00)))|(((0[1-9]|[12]\d|3[01])(0[13578]|1[02])((1[6-9]|[2-9]\d)?\d{2}))|((0[1-9]|[12]\d|30)(0[13456789]|1[012])((1[6-9]|[2-9]\d)?\d{2}))|((0[1-9]|1\d|2[0-8])02((1[6-9]|[2-9]\d)?\d{2}))|(2902((1[6-9]|[2-9]\d)?(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00)|00))))$/;
            var errorMessage = 'Please enter Valid Transaction Date in dd/mm/yyyy format.';

            if (document.getElementById("<%=txtInvoiceDate.ClientID%>").value.match(RegExPattern)) {
                if (len.length == 8) {
                    alert(errorMessage);
                    document.getElementById("<%=txtInvoiceDate.ClientID%>").value = "";
                    document.getElementById("<%=txtInvoiceDate.ClientID%>").focus();
                    return false;
                }
            }
            else {
                alert(errorMessage);
                document.getElementById("<%=txtInvoiceDate.ClientID%>").value = "";
                document.getElementById("<%=txtInvoiceDate.ClientID%>").focus();
                return false;
            }

            var str1 = document.getElementById("<%=txtInvoiceDate.ClientID %>").value;
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
                document.getElementById("<%=txtInvoiceDate.ClientID%>").value = "";
                document.getElementById("<%=txtInvoiceDate.ClientID%>").focus();
                //"");
                return false;
            }
            var str1 = document.getElementById("<%=txtInvoiceDate.ClientID %>").value;
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
                alert("Invoice Date Cannot be Lesser than Batch Date");
                document.getElementById("<%=txtInvoiceDate.ClientID%>").value = "";
                document.getElementById("<%=txtInvoiceDate.ClientID%>").focus();
                //"");
                return false;
            }


            return true;
        }
        function checkValue() {

            if (((event.keyCode < 48) || (event.keyCode > 57)) && (event.keyCode != 13) && (event.keyCode != 46)) {
                alert("Enter Valid Total Amount");
                event.keyCode = 0;
            }
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
        function getibtnBDate() {
            popUpCalendar(document.getElementById("<%=ibtnBDate.ClientID%>"), document.getElementById("<%=txtBatchDate.ClientID%>"), 'dd/mm/yyyy')

        }
        function getDate1from() {
            popUpCalendar(document.getElementById("<%=ibtnInDate.ClientID%>"), document.getElementById("<%=txtInvoiceDate.ClientID%>"), 'dd/mm/yyyy')

        }

        function CheckBatchDate() {
            var digits = "0123456789/";
            var temp;
            for (var i = 0; i < document.getElementById("<%=txtBatchDate.ClientID %>").value.length; i++) {
                temp = document.getElementById("<%=txtBatchDate.ClientID%>").value.substring(i, i + 1);
                if (digits.indexOf(temp) == -1) {
                    alert("Enter Valid Batch Date (dd/mm/yyyy)");
                    document.getElementById("<%=txtBatchDate.ClientID%>").value = "";
                    document.getElementById("<%=txtBatchDate.ClientID%>").focus();
                    return false;
                }
            }
            return true;
        }

        function CheckInvDate() {
            var digits = "0123456789/";
            var temp;
            for (var i = 0; i < document.getElementById("<%=txtInvoiceDate.ClientID %>").value.length; i++) {
                temp = document.getElementById("<%=txtInvoiceDate.ClientID%>").value.substring(i, i + 1);
                if (digits.indexOf(temp) == -1) {
                    alert("Enter Valid Transaction Date (dd/mm/yyyy)");
                    document.getElementById("<%=txtInvoiceDate.ClientID%>").value = "";
                    document.getElementById("<%=txtInvoiceDate.ClientID%>").focus();
                    return false;
                }
            }
            return true;
        }
        function CheckDescription() {
            var digits = "0123456789abcdefghijklmnopqrstuvwxyz ABCDEFGHIJKLMNOPQRSTUVWXYZ ";
            var temp;
            for (var i = 0; i < document.getElementById("<%=txtDesc.ClientID %>").value.length; i++) {
                temp = document.getElementById("<%=txtDesc.ClientID%>").value.substring(i, i + 1);
                if (digits.indexOf(temp) == -1) {
                    alert("Enter Valid Description Field");
                    document.getElementById("<%=txtDesc.ClientID%>").value = "";
                    document.getElementById("<%=txtDesc.ClientID%>").focus();
                    return false;
                }
            }
            return true;
        }


 
    </script>
    <%-- <atlas:ScriptManager ID="scriptmanager1" EnablePartialRendering="true" runat="Server" />
   <atlas:UpdatePanel ID="up1" runat="server">
<ContentTemplate>--%>
    <asp:Panel ID="pnlToolbar" runat="server" Width="100%">
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
                                <asp:ImageButton ID="ImageButton1" runat="server" ImageUrl="~/images/gothers.png"
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
                    <asp:TextBox ID="txtRecNo" runat="server" AutoPostBack="True" Style="text-align: right"
                        Width="52px" MaxLength="7" ReadOnly="true" CssClass="text_box" disabled="disabled"
                        TabIndex="1" dir="ltr"></asp:TextBox>
                </td>
                <td style="width: 2%; height: 14px">
                    <asp:Label ID="Label47" runat="server">Of</asp:Label>
                </td>
                <td style="width: 2%; height: 14px">
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
        <table style="width: 100%">
            <tr>
                <td class="vline" style="width: 746px; height: 1px">
                </td>
            </tr>
        </table>
        <table style="width: 100%">
            <tr>
                <td style="width: 400px">
                    <asp:SiteMapPath ID="SiteMapPath1" runat="server">
                    </asp:SiteMapPath>
                    /Debit Note
                </td>
                <td align="right"  class="pagetext">
                    <asp:Label ID="lblMenuName" runat="server" Width="350px"></asp:Label>
                </td>
            </tr>
        </table>
    </asp:Panel>
    <%--</ContentTemplate>     
</atlas:UpdatePanel>
<atlas:UpdateProgress ID="ProgressIndicator" runat="server">
    <ProgressTemplate>
        Loading the data, please wait... 
        <asp:Image ID="LoadingImage" ImageAlign="AbsMiddle" runat="server" ImageUrl="~/Images/spinner.gif" />        
    </ProgressTemplate>
 </atlas:UpdateProgress>
<atlas:UpdatePanel ID="up2" runat="server">
<ContentTemplate>--%>
    <table style="width: 100%;">
        <tr>
            <td style="width: 95px;">
                <table  cellspacing="0" cellspacing="0">
                    <tr>
                        <td>
                        </td>
                        <td colspan="4">
                            <asp:Label ID="lblMsg" runat="server" CssClass="lblError" Style="text-align: center"
                                Width="448px"></asp:Label>
                        </td>
                        <td rowspan="2">
                           
                        </td>
                    </tr>
                    <tr>
                        <td>
                        </td>
                        <td>
                            <!--<asp:Image ID="imgLeft1" runat="server" ImageUrl="images/b_orange_left.gif" ImageAlign="AbsBottom" />-->
                            <asp:Button ID="btnBatchInvoice" runat="server" Height="24px" Text="Credit Note"
                                Width="108px" CssClass="TabButton" OnClick="btnBatchInvoice_Click" /><!--<asp:Image ID="imgRight1" runat="server" ImageUrl="images/b_orange_right.gif" ImageAlign="AbsBottom" />-->
                        </td>
                        <td>
                            <!--<asp:Image ID="imgLeft2" runat="server" ImageUrl="images/b_orange_left.gif" ImageAlign="AbsBottom" />-->
                            <asp:Button ID="btnSelection" runat="server" Height="25px" Text="Selection Criteria"
                                Width="108px" CssClass="TabButton" OnClick="btnSelection_Click" /><!--<asp:Image ID="imgRight2" runat="server" ImageUrl="images/b_orange_right.gif" ImageAlign="AbsBottom" />-->
                        </td>
                        <td>
                            <!--<asp:Image ID="imgLeft3" runat="server" ImageUrl="images/b_orange_left.gif" ImageAlign="AbsBottom" />-->
                            <asp:Button ID="btnViewStu" runat="server" Height="25px" Text="View Sponsors" Width="108px"
                                CssClass="TabButton" OnClick="btnViewStu_Click" /><!--<asp:Image ID="imgRight3" runat="server" ImageUrl="images/b_orange_right.gif" ImageAlign="AbsBottom" />-->
                        </td>
                        <td>
                            <!--<asp:Image ID="imgLeft4" runat="server" ImageUrl="images/b_orange_left.gif" ImageAlign="AbsBottom" />-->
                            <asp:Button ID="btnViewBalanceSponsor" runat="server" Height="25px" Text="Sponsor Balance"
                                Width="108px" CssClass="TabButton" /><!--<asp:Image ID="imgRight4" runat="server" ImageUrl="images/b_orange_right.gif" ImageAlign="AbsBottom" />-->
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
   
    <asp:MultiView ID="MultiView1" runat="server" ActiveViewIndex="0">
        <asp:View ID="View1" runat="server">
            <table style="width: 100%">
                <tr>
                    <td style="width: 100%">
                      <div  style="border: thin solid #A6D9F4; width: 100%">
                            <asp:Panel ID="pnlBatch" runat="server" Height="100%" Width="100%">
                                <table style="width: 100%">
                                    <tr>
                                        <td style="width: 100px; height: 138px;">
                                            <table style="width: 100%">
                                                <tr>
                                                    <td style="height: 25px">
                                                       
                                                    </td>
                                                    <td style="width: 178px; height: 25px">
                                                        <asp:Label ID="lblTypeNote" runat="server" Text="Note Type" Width="150px"></asp:Label>
                                                    </td>
                                                    <td colspan="4" style="height: 25px">
                                                        <asp:DropDownList ID="ddlNoteType" runat="server" AutoPostBack="True">
                                                            <asp:ListItem Selected="True" Value="-1">--------Select--------</asp:ListItem>
                                                            <asp:ListItem>Debit Note</asp:ListItem>
                                                            <asp:ListItem>Credit Note</asp:ListItem>
                                                        </asp:DropDownList>
                                                    </td>
                                                    <td style="width: 100px;" rowspan="5">
                                                        <asp:ImageButton ID="ibtnStatus" runat="server" ImageUrl="~/images/NotReady.gif"
                                                            Enabled="False" CssClass="cursor" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="height: 25px">
                                                    </td>
                                                    <td style="width: 178px; height: 25px">
                                                        <asp:Label ID="Label1" runat="server" Text="Batch No" Width="150px"></asp:Label>
                                                    </td>
                                                    <td style="height: 25px" colspan="4">
                                                        <asp:TextBox ID="txtBatchNo" runat="server" Width="142px" ReadOnly="True"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="height: 16px">
                                                    </td>
                                                    <td style="width: 178px; height: 16px">
                                                        <asp:Label ID="Label3" runat="server" Text="Batch Date" Width="150px"></asp:Label>
                                                    </td>
                                                    <td style="width: 40px; height: 16px">
                                                        <asp:TextBox ID="txtBatchDate" runat="server" Width="95px" MaxLength="10"></asp:TextBox>
                                                    </td>
                                                    <td style="width: 9px; height: 16px">
                                                        <asp:Image ID="ibtnBDate" runat="server" ImageUrl="~/images/cal.gif" />
                                                    </td>
                                                    <td style="width: 868px; height: 16px">
                                                        <span style="color: red">*</span>
                                                    </td>
                                                    <td style="height: 16px;">
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="height: 22px">
                                                    </td>
                                                    <td style="width: 178px; height: 22px">
                                                        <asp:Label ID="Label7" runat="server" Text="Transaction Date" Width="99px"></asp:Label>
                                                    </td>
                                                    <td style="height: 22px; width: 40px;">
                                                        <asp:TextBox ID="txtInvoiceDate" runat="server" MaxLength="10" Width="95px"></asp:TextBox>
                                                    </td>
                                                    <td style="width: 9px; height: 22px">
                                                        <asp:Image ID="ibtnInDate" runat="server" ImageUrl="~/images/cal.gif" />
                                                    </td>
                                                    <td style="width: 868px; height: 22px">
                                                        <span style="color: #ff0000">*</span>
                                                    </td>
                                                    <td style="text-align: right; height: 22px;" rowspan="1">
                                                       
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="height: 22px">
                                                       
                                                    </td>
                                                    <td style="width: 178px; height: 22px">
                                                        <asp:Label ID="lblCheckGL" runat="server" Text="GL Code" Width="99px"></asp:Label>
                                                    </td>
                                                    <td style="height: 22px; width: 40px;">
                                                        <asp:TextBox ID="txtGLCode" runat="server" MaxLength="10" Width="95px"></asp:TextBox>
                                                    </td>
                                                    <td style="width: 9px; height: 22px">
                                                        <asp:Button ID="ibtnCheckGL" runat="server" Text="Check" />
                                                    </td>
                                                    <td style="height: 22px; color: Red; font-size: smaller;" colspan="2">
                                                        <asp:Label ID="lblResultGlCode" runat="server"></asp:Label>
                                                        <asp:Image ID="imgGL" runat="server" Height="18px" ImageAlign="Baseline" ImageUrl="~/images/check.png"
                                                            Visible="False" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="height: 22px">
                                                    </td>
                                                    <td style="width: 178px; height: 22px">
                                                        <asp:Label ID="Label23" runat="server" Text="Description" Width="150px"></asp:Label>
                                                    </td>
                                                    <td style="height: 22px" colspan="5">
                                                        <asp:TextBox ID="txtDesc" runat="server" Height="20px" MaxLength="50" Width="297px"></asp:TextBox>
                                                        <span style="color: #ff0000">*</span>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="height: 22px">
                                                    </td>
                                                    <td style="width: 178px; height: 22px">
                                                        <asp:Label ID="lblTotal" runat="server" Text="Total Amount" Width="65px"></asp:Label>
                                                    </td>
                                                    <td style="width: 40px; height: 22px">
                                                        <asp:TextBox ID="txtTotal" runat="server" AutoPostBack="True" Height="15px" MaxLength="10"
                                                            OnTextChanged="txtTotal_TextChanged" Style="text-align: right" Width="100px"></asp:TextBox>
                                                    </td>
                                                    <td style="width: 9px; height: 22px">
                                                        <span style="color: #ff0000">*</span>
                                                    </td>
                                                    <td style="width: 868px; height: 22px">
                                                    </td>
                                                    <td rowspan="1" style="height: 22px; text-align: right">
                                                    </td>
                                                    <td rowspan="1" style="vertical-align: middle; width: 100px">
                                                        <asp:TextBox ID="lblStatus1" runat="server" Height="15px" MaxLength="10" OnTextChanged="txtTotal_TextChanged"
                                                            Style="text-align: right" Visible="False" Width="100px">New</asp:TextBox>
                                                    </td>
                                                </tr>
                                            </table>
                                            <asp:Button ID="ibtnafterpost" runat="server" OnClick="ibtnafterpost_Click" Text="Button"
                                                Visible="False" />
                                            <asp:HiddenField ID="today" runat="server" />
                                           
                                        </td>
                                    </tr>
                                </table>
                            </asp:Panel>
                       </div>
                    </td>
                </tr>
            </table>
        </asp:View>
        <asp:View ID="View2" runat="server">
            <table style="width: 100%">
                <tr>
                    <td style="width: 100%">
                        <fieldset style="width: 98%">
                            <legend><strong><span style="color: #000000;"></span></strong></legend>
                            <asp:Panel ID="pnlSelection" runat="server" Width="100%" Visible="False">
                                <table style="width: 100%; height: 150px">
                                    <tr>
                                        <td style="width: 3px; height: 21px">
                                            <asp:Label ID="Label4" runat="server" Text="Sponsor Code" Width="91px"></asp:Label>
                                        </td>
                                        <td style="width: 246px; height: 21px">
                                            <asp:TextBox ID="txtSpnCode" runat="server"></asp:TextBox>
                                        </td>
                                        <td style="width: 330270px; height: 21px">
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 3px; height: 18px">
                                            <asp:Label ID="Label8" runat="server" Text="Sponsor Name" Width="83px"></asp:Label>
                                        </td>
                                        <td style="width: 246px; height: 18px">
                                            <asp:TextBox ID="txtSpnName" runat="server"></asp:TextBox>
                                        </td>
                                        <td style="width: 330270px; height: 18px">
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 3px; height: 16px">
                                            <asp:Label ID="Label9" runat="server" Text="Sponsor Type" Width="82px" Visible="False"></asp:Label>
                                        </td>
                                        <td style="width: 330270px; height: 16px">
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 3px; height: 16px">
                                        </td>
                                        <td style="width: 246px; height: 16px">
                                            <asp:TextBox ID="txtSpnType" runat="server" Visible="False"></asp:TextBox>
                                        </td>
                                        <td style="width: 330270px; height: 16px">
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 3px; height: 16px">
                                            <asp:CheckBox ID="chkSelectSponsor" runat="server" AutoPostBack="True" OnCheckedChanged="chkSelectSponsor_CheckedChanged"
                                                Text="Select All" Width="83px" Visible="False" />
                                        </td>
                                        <td style="width: 246px; height: 16px">
                                        </td>
                                        <td style="width: 330270px; height: 16px">
                                        </td>
                                    </tr>
                                </table>
                                <table style="width: 100%">
                                    <tr>
                                        <td style="width: 100px">
                                        </td>
                                        <td style="width: 95%">
                                            <asp:DataGrid ID="dgView" runat="server" AutoGenerateColumns="False" DataKeyField="SponserCode"
                                                Width="60%">
                                                <FooterStyle CssClass="dgFooterStyle" Height="20px" />
                                                <SelectedItemStyle CssClass="dgSelectedItemStyle" />
                                                <AlternatingItemStyle BackColor="Beige" CssClass="dgAlternatingItemStyle" Font-Bold="False"
                                                    Font-Italic="False" Font-Overline="False" Font-Strikeout="False" Font-Underline="False" />
                                                <ItemStyle CssClass="dgItemStyle" />
                                                <HeaderStyle BackColor="#CDD7EE" CssClass="dgHeaderStyle" Font-Bold="True" Font-Italic="False"
                                                    Font-Overline="False" Font-Size="Medium" Font-Strikeout="False" Font-Underline="False" />
                                                <Columns>
                                                    <asp:TemplateColumn HeaderText="Select ">
                                                        <ItemTemplate>
                                                            <asp:CheckBox ID="chk" runat="server" />
                                                        </ItemTemplate>
                                                    </asp:TemplateColumn>
                                                    <asp:ButtonColumn CommandName="Select" DataTextField="SponserCode" HeaderText="Sponsor Code"
                                                        Text="SponserCode"></asp:ButtonColumn>
                                                    <asp:BoundColumn DataField="Name" HeaderText="Sponsor Name"></asp:BoundColumn>
                                                </Columns>
                                            </asp:DataGrid>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 1%">
                                        </td>
                                        <td style="width: 162px">
                                            <asp:Button ID="btnUpdateCri" runat="server" OnClick="btnUpdateCri_Click1" Text="Update Criteria" />
                                        </td>
                                    </tr>
                                </table>
                            </asp:Panel>
                        </fieldset>
                    </td>
                </tr>
            </table>
        </asp:View>
        <asp:View ID="View3" runat="server">
            <table style="width: 100%">
                <tr>
                    <td style="width: 100%">
                       <div  style="border: thin solid #A6D9F4; width: 100%">
                            <asp:Panel ID="pnlView" runat="server" Width="100%" Visible="False">
                                <table style="width: 100%;">
                                    <tr>
                                        <td style="width: 100%; vertical-align: top;">
                                            <br />
                                            <asp:Label ID="Label2" runat="server" Text="Add Sponsor" Width="68px"></asp:Label>
                                            <asp:ImageButton ID="ibtnStudent" runat="server" Height="16px" Width="16px" ImageUrl="~/images/find_img.png"
                                                ToolTip="Select Sponsor" /><br />
                                            <asp:CheckBox ID="chkSponsor" runat="server" AutoPostBack="True" OnCheckedChanged="chkSponsor_CheckedChanged1"
                                                Visible="False" Text="Select All" /><br />
                                            <br />
                                            <asp:DataGrid ID="dgSponsor" runat="server" AutoGenerateColumns="False" DataKeyField="SponserCode"
                                                Width="420px">
                                                <FooterStyle CssClass="dgFooterStyle" Height="20px" />
                                                <SelectedItemStyle CssClass="dgSelectedItemStyle" />
                                                <AlternatingItemStyle BackColor="Beige" CssClass="dgAlternatingItemStyle" Font-Bold="False"
                                                    Font-Italic="False" Font-Overline="False" Font-Strikeout="False" Font-Underline="False" />
                                                <ItemStyle CssClass="dgItemStyle" />
                                                <HeaderStyle BackColor="#CDD7EE" CssClass="dgHeaderStyle" Font-Bold="True" Font-Italic="False"
                                                    Font-Overline="False" Font-Size="Medium" Font-Strikeout="False" Font-Underline="False" />
                                                <Columns>
                                                    <asp:TemplateColumn HeaderText="Select ">
                                                        <ItemTemplate>
                                                            <asp:CheckBox ID="chk" runat="server" />
                                                        </ItemTemplate>
                                                    </asp:TemplateColumn>
                                                    <asp:ButtonColumn CommandName="Select" DataTextField="SponserCode" HeaderText="Sponsor Code"
                                                        Text="SponserCode"></asp:ButtonColumn>
                                                    <asp:BoundColumn DataField="Name" HeaderText="Sponsor"></asp:BoundColumn>
                                                </Columns>
                                            </asp:DataGrid>
                                        </td>
                                    </tr>
                                </table>
                            </asp:Panel>
                       </div>
                    </td>
                </tr>
            </table>
        </asp:View>
        <asp:View ID="View4" runat="server">
            <asp:Panel ID="pnlSponsorBalance" runat="server" Width="100%" Visible="False">
                <fieldset style="width: 98%">
                    <legend><strong><span style="color: #000000;"></span></strong></legend>
                    <table width="100%">
                        <tr>
                            <td style="width: 13%">
                                <asp:Label ID="Label10" runat="server" Text="Sponsor Code" Width="97px"></asp:Label>
                            </td>
                            <td style="width: 14%">
                                <asp:TextBox ID="txtStudentCode" runat="server" MaxLength="20" Width="142px" ReadOnly="True"></asp:TextBox>
                            </td>
                            <td style="width: 101px; text-align: left">
                                <asp:Image ID="ibtnSpn1" runat="server" Height="16px" ImageUrl="~/images/find_img.png"
                                    Width="16px" />
                            </td>
                            <td style="width: 136px; text-align: left">
                            </td>
                            <td style="width: 41px">
                            </td>
                            <td style="width: 378px">
                               
                            </td>
                            <td style="width: 378px">
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 13%; height: 25px;">
                                <asp:Label ID="Label12" runat="server" Text="Sponsor Name" Width="97px"></asp:Label>
                            </td>
                            <td style="width: 14%; height: 25px;">
                                <asp:TextBox ID="txtStuName" runat="server" MaxLength="20" Width="142px" ReadOnly="True"></asp:TextBox>
                            </td>
                            <td style="width: 101px; text-align: right; height: 25px;">
                            </td>
                            <td style="width: 136px; text-align: right; height: 25px;">
                            </td>
                            <td style="width: 41px; height: 25px;">
                               
                            </td>
                            <td style="width: 378px; height: 25px;">
                            </td>
                            <td style="width: 378px; height: 25px;">
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 13%; height: 25px;">
                                <asp:Label ID="Label15" runat="server" Text="Date" Width="37px" Visible="False"></asp:Label>
                            </td>
                            <td colspan="2" style="height: 25px">
                                <asp:TextBox ID="txtDate" runat="server" MaxLength="20" Width="142px" ReadOnly="True"
                                    Visible="False"></asp:TextBox>
                            </td>
                            <td colspan="1" style="height: 25px; text-align: right; width: 136px;">
                            </td>
                            <td style="width: 41px; height: 25px;">
                            </td>
                            <td style="width: 378px; height: 25px;">
                            </td>
                            <td style="width: 378px; height: 25px;">
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 13%; height: 25px;">
                                <asp:Label ID="Label16" runat="server" Text="Status" Width="97px" Visible="False"></asp:Label>
                            </td>
                            <td style="width: 14%; height: 25px;">
                                <asp:TextBox ID="TextBox8" runat="server" MaxLength="20" Width="142px" Visible="False"></asp:TextBox>
                            </td>
                            <td style="width: 101px; text-align: right; height: 25px;">
                            </td>
                            <td style="width: 136px; height: 25px; text-align: right">
                            </td>
                            <td style="width: 41px; height: 25px;">
                            </td>
                            <td style="width: 378px; height: 25px;">
                            </td>
                            <td style="width: 378px; height: 25px;">
                            </td>
                        </tr>
                    </table>
                </fieldset>
                <br />
                <table width="100%">
                    <tr>
                        <td style="height: 16px" width="1%">
                        </td>
                        <td style="width: 72%; height: 16px">
                            <asp:Label ID="Label19" runat="server" Text="Sponsor Ledger" Width="97px" Visible="False"></asp:Label>
                            <span style="color: Red;">
                                <asp:Label ID="lblWarn" runat="server" Text="Sponsor Out Of Balance"></asp:Label></span>
                        </td>
                        <td style="height: 16px" width="30%">
                        </td>
                        <td style="height: 16px" width="30%">
                        </td>
                        <td style="height: 16px" width="1%">
                        </td>
                    </tr>
                    <tr>
                        <td style="height: 16px" width="1%">
                        </td>
                        <td colspan="3" style="height: 16px">
                            <asp:DataGrid ID="dgInvoices" runat="server" AutoGenerateColumns="False" DataKeyField="TransactionCode"
                                Width="100%">
                                <FooterStyle CssClass="dgFooterStyle" Height="20px" />
                                <SelectedItemStyle CssClass="dgSelectedItemStyle" />
                                <AlternatingItemStyle BackColor="Beige" CssClass="dgAlternatingItemStyle" Font-Bold="False"
                                    Font-Italic="False" Font-Overline="False" Font-Strikeout="False" Font-Underline="False" />
                                <ItemStyle CssClass="dgItemStyle" />
                                <HeaderStyle BackColor="#CDD7EE" CssClass="dgHeaderStyle" Font-Bold="True" Font-Italic="False"
                                    Font-Overline="False" Font-Size="Medium" Font-Strikeout="False" Font-Underline="False" />
                                <Columns>
                                    <asp:BoundColumn DataField="TransDate" DataFormatString="{0:dd/MM/yyyy}" HeaderText="Date">
                                    </asp:BoundColumn>
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
                                    <asp:TemplateColumn HeaderText="View">
                                        <ItemTemplate>
                                            <asp:HyperLink ID="btnview" runat="server">View</asp:HyperLink>
                                        </ItemTemplate>
                                    </asp:TemplateColumn>
                                    <asp:BoundColumn DataField="Category" HeaderText="Category" Visible="False"></asp:BoundColumn>
                                    <asp:BoundColumn DataField="BatchCode" HeaderText="BatchCode" Visible="False"></asp:BoundColumn>
                                    <asp:BoundColumn DataField="TransType" HeaderText="TransType" Visible="False"></asp:BoundColumn>
                                </Columns>
                            </asp:DataGrid>
                        </td>
                        <td style="height: 16px" width="1%">
                        </td>
                    </tr>
                    <tr>
                        <td style="height: 16px" width="1%">
                        </td>
                        <td style="width: 72%; height: 16px; text-align: right">
                           &nbsp;
                        </td>
                        <td style="height: 16px; text-align: right" width="30%">
                            <asp:Label ID="lblDebit" runat="server" Text="Total Debit Amount" Visible="False"></asp:Label>
                        </td>
                        <td style="height: 16px; text-align: right" width="30%">
                            <asp:TextBox ID="txtDebitAmt" runat="server" Style="text-align: right" Width="101px"
                                Visible="False" Font-Bold="True" ReadOnly="True"></asp:TextBox>
                        </td>
                        <td style="height: 16px" width="1%">
                        </td>
                    </tr>
                    <tr>
                        <td style="height: 16px" width="1%">
                        </td>
                        <td style="width: 72%; height: 16px; text-align: right">
                            <asp:TextBox ID="txtPg" runat="server" MaxLength="20" Width="240px" Visible="False"></asp:TextBox>
                        </td>
                        <td style="height: 16px; text-align: right" width="30%">
                            <asp:Label ID="lblCredit" runat="server" Text="Total Credit Amount" Visible="False"></asp:Label>
                        </td>
                        <td style="height: 16px; text-align: right" width="30%">
                            <asp:TextBox ID="txtCreditAmt" runat="server" Style="text-align: right" Width="102px"
                                Visible="False" Font-Bold="True" ReadOnly="True"></asp:TextBox>
                        </td>
                        <td style="height: 16px" width="1%">
                        </td>
                    </tr>
                    <tr>
                        <td style="height: 16px" width="1%">
                        </td>
                        <td style="width: 72%; height: 16px; text-align: right">
                        </td>
                        <td style="height: 16px; text-align: right" width="30%">
                            <asp:Label ID="lblOut" runat="server" Text="Outstanding Amount"></asp:Label>
                        </td>
                        <td style="height: 16px; text-align: right" width="30%">
                            <asp:TextBox ID="txtOutAmt" runat="server" Font-Bold="True" Style="text-align: right"
                                Width="102px" ReadOnly="True"></asp:TextBox>
                        </td>
                        <td style="height: 16px" width="1%">
                        </td>
                    </tr>
                </table>
            </asp:Panel>
            </fieldset> </td> </tr> </table>
        </asp:View>
    </asp:MultiView>
    <asp:HiddenField ID="scriptid" runat="server" />
    <asp:HiddenField ID="lblStatus" runat="server" Value="New" />
     <asp:Button ID="btnHidden" runat="Server" OnClick="btnHidden_Click"  style="display:none" />
    <%--  </ContentTemplate>     
</atlas:UpdatePanel>--%>
    <br />
    <br />
    <br />
</asp:Content>
