<%@ Page Language="VB" MasterPageFile="~/MasterPage3.master" AutoEventWireup="false" MaintainScrollPositionOnPostback="true"
    CodeFile="Student.aspx.vb" Inherits="Student" Title="Setup - Student Info" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <script  type="text/javascript" language="javascript" src="Scripts/popcalendar.js"></script>
    <script language="javascript" type="Scripts/functions"></script>
    <script language="javascript" type="text/javascript">
        function getconfirm() {
            if (document.getElementById("<%=txtMatricNo.ClientID%>").value == "") {

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
        function CheckVFromDate() {
            var digits = "0123456789/";
            var temp;
            for (var i = 0; i < document.getElementById("<%=txtVFrom1.ClientID %>").value.length; i++) {
                temp = document.getElementById("<%=txtVFrom1.ClientID%>").value.substring(i, i + 1);
                if (digits.indexOf(temp) == -1) {
                    alert("Enter Valid Date (dd/mm/yyyy)");
                    document.getElementById("<%=txtVFrom1.ClientID%>").value = "";
                    document.getElementById("<%=txtVFrom1.ClientID%>").focus();
                    return false;
                }
            }
            return true;
        }
        function CheckT0date() {
            var digits = "0123456789/";
            var temp;
            for (var i = 0; i < document.getElementById("<%=txtspTo1.ClientID %>").value.length; i++) {
                temp = document.getElementById("<%=txtspTo1.ClientID%>").value.substring(i, i + 1);
                if (digits.indexOf(temp) == -1) {
                    alert("Enter Valid Date (dd/mm/yyyy)");
                    document.getElementById("<%=txtspTo1.ClientID%>").value = "";
                    document.getElementById("<%=txtspTo1.ClientID%>").focus();
                    return false;
                }
            }
            return true;
        }

        function getconfirm() {
            if (document.getElementById("<%=txtMatricNo.ClientID%>").value == "") {

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

        function getDate1from() {
            popUpCalendar(document.getElementById("<%=ibtnFDate1.ClientID%>"), document.getElementById("<%=txtVFrom1.ClientID%>"), 'dd/mm/yyyy')

        }

        function getDate1to() {
            popUpCalendar(document.getElementById("<%=ibtnTDate1.ClientID%>"), document.getElementById("<%=txtspTo1.ClientID%>"), 'dd/mm/yyyy')

        }

        function validate() {
            var re = /\s*((\S+\s*)*)/;
            if (document.getElementById("<%=txtMatricNo.ClientID%>").value.replace(re, "$1").length == 0) {
                alert("Student Matric No Field Cannot Be Blank");
                document.getElementById("<%=txtMatricNo.ClientID%>").focus();
                return false;
            }
            var digits = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            var temp;
            for (var i = 0; i < document.getElementById("<%=txtMatricNo.ClientID %>").value.length; i++) {
                temp = document.getElementById("<%=txtMatricNo.ClientID%>").value.substring(i, i + 1);
                if (digits.indexOf(temp) == -1) {
                    alert("Enter valid MatricNo");
                    document.getElementById("<%=txtMatricNo.ClientID%>").focus();
                    return false;
                }
            }
            if (document.getElementById("<%=txtName.ClientID%>").value.replace(re, "$1").length == 0) {
                alert("Student Name Field Cannot Be Blank");
                document.getElementById("<%=txtName.ClientID%>").focus();
                return false;
            }
            if (document.getElementById("<%=ddlstudentStatus.ClientID%>").value == "-1") {
                alert("Select a Student Status");
                document.getElementById("<%=ddlstudentStatus.ClientID%>").focus();
                return false;
            }
            if (document.getElementById("<%=ddlIcNo.ClientID%>").value == "-1") {
                alert("Select a Matric ID Type");
                document.getElementById("<%=ddlIcNo.ClientID%>").focus();
                return false;
            }
            if (document.getElementById("<%=ddlRegistrationStatus.ClientID%>").value == "-1") {
                alert("Select the Registration Status");
                document.getElementById("<%=ddlRegistrationStatus.ClientID%>").focus();
                return false;
            }
            if (document.getElementById("<%=txtIcNo.ClientID%>").value.replace(re, "$1").length == 0) {
                alert("Matric ID No Field Cannot Be Blank");
                document.getElementById("<%=txtIcNo.ClientID%>").focus();
                return false;
            }
            if (document.getElementById("<%=ddlFaculty.ClientID%>").value == "-1") {
                alert("Select a Student Faculty");
                document.getElementById("<%=ddlFaculty.ClientID%>").focus();
                return false;
            }
            if (document.getElementById("<%=ddlProgram.ClientID%>").value == "-1") {
                alert("Select a Student Program");
                document.getElementById("<%=ddlProgram.ClientID%>").focus();
                return false;
            }

            if (document.getElementById("<%=ddlIntkSemester.ClientID%>").value == "-1") {
                alert("Select a Student Intake Semester");
                document.getElementById("<%=ddlIntkSemester.ClientID%>").focus();
                return false;
            }

            if (document.getElementById("<%=ddlIntakeSession.ClientID%>").value == "-1") {
                alert("Select a Student Intake Session ");
                document.getElementById("<%=ddlIntakeSession.ClientID%>").focus();
                return false;
            }
            if (document.getElementById("<%=ddlSem.ClientID%>").value == "-1") {
                alert("Select a Student Current Semester");
                document.getElementById("<%=ddlSem.ClientID%>").focus();
                return false;
            }

            if (document.getElementById("<%=ddlCurSession.ClientID%>").value == "-1") {
                alert("Select a Student Current Session ");
                document.getElementById("<%=ddlCurSession.ClientID%>").focus();
                return false;
            }
            if (document.getElementById("<%=ddlStudyType.ClientID%>").value == "-1") {
                alert("Select a Student Study Type");
                document.getElementById("<%=ddlStudyType.ClientID%>").focus();
                return false;
            }
            if (document.getElementById("<%=ddlStudentCategory.ClientID%>").value == "-1") {
                alert("Select a Student Category");
                document.getElementById("<%=ddlStudentCategory.ClientID%>").focus();
                return false;
            }
            if (document.getElementById("<%=ddlBank.ClientID%>").value == "-1") {
                alert("Select a Student Bank Name");
                document.getElementById("<%=ddlBank.ClientID%>").focus();
                return false;
            }
            if (document.getElementById("<%=txtAccNo.ClientID%>").value.replace(re, "$1").length == 0) {
                alert("Student Bank Account Field Cannot Be Blank");
                document.getElementById("<%=txtAccNo.ClientID%>").focus();
                return false;
            }
            if (document.getElementById("<%=ddlBank.ClientID%>").value == "-1") {
                alert("Select a Student Bank Name");
                document.getElementById("<%=ddlBank.ClientID%>").focus();
                return false;
            }


            if (document.getElementById("<%=ddlFeeCat.ClientID%>").value == "-1") {
                alert("Select a Student Fee Category");
                document.getElementById("<%=ddlFeeCat.ClientID%>").focus();
                return false;
            }

            if (document.getElementById("<%=ddlHostel.ClientID%>").value == "-1") {
                alert("Select a Hostel");
                document.getElementById("<%=ddlHostel.ClientID%>").focus();
                return false;
            }
            if (document.getElementById("<%=ddlHostel.ClientID%>").value == "1") {
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
                if (document.getElementById("<%=ddlRoomType.ClientID%>").value == "-1") {
                    alert("Select a Room Type");
                    document.getElementById("<%=ddlRoomType.ClientID%>").focus();
                    return false;
                }
                if (document.getElementById("<%=txtFloorNo.ClientID%>").value == "-1") {
                    alert("Floor No Field Cannot Be Blank");
                    document.getElementById("<%=txtFloorNo.ClientID%>").focus();
                    return false;
                }
            }
            var digits = "0123456789.";
            var temp;
            for (var i = 0; i < document.getElementById("<%=txtGpa.ClientID %>").value.length; i++) {
                temp = document.getElementById("<%=txtGpa.ClientID%>").value.substring(i, i + 1);
                if (digits.indexOf(temp) == -1) {
                    alert("Enter Valid Correct GPA");
                    document.getElementById("<%=txtGpa.ClientID%>").focus();
                    return false;
                }
            }
            var digits = "0123456789.";
            var temp;
            for (var i = 0; i < document.getElementById("<%=txtcgpa.ClientID %>").value.length; i++) {
                temp = document.getElementById("<%=txtcgpa.ClientID%>").value.substring(i, i + 1);
                if (digits.indexOf(temp) == -1) {
                    alert("Enter Valid Correct CPGA");
                    document.getElementById("<%=txtcgpa.ClientID%>").focus();
                    return false;
                }
            }
            var digits = "0123456789";
            var temp;
            for (var i = 0; i < document.getElementById("<%=txtPostcode.ClientID %>").value.length; i++) {
                temp = document.getElementById("<%=txtPostcode.ClientID%>").value.substring(i, i + 1);
                if (digits.indexOf(temp) == -1) {
                    alert("Enter Valid Correct PostalCode");
                    document.getElementById("<%=txtPostcode.ClientID%>").focus();
                    return false;
                }
            }

            var digits = "0123456789";
            var temp;
            for (var i = 0; i < document.getElementById("<%=txtPhone.ClientID %>").value.length; i++) {
                temp = document.getElementById("<%=txtPhone.ClientID%>").value.substring(i, i + 1);
                if (digits.indexOf(temp) == -1) {
                    alert("Enter Valid Correct PhoneNo");
                    document.getElementById("<%=txtPhone.ClientID%>").focus();
                    return false;
                }
            }

            var digits = "0123456789";
            var temp;
            for (var i = 0; i < document.getElementById("<%=txtmoblieno.ClientID %>").value.length; i++) {
                temp = document.getElementById("<%=txtmoblieno.ClientID%>").value.substring(i, i + 1);
                if (digits.indexOf(temp) == -1) {
                    alert("Enter Valid Correct MobileNo");
                    document.getElementById("<%=txtmoblieno.ClientID%>").focus();
                    return false;
                }
            }

            var digits = "0123456789";
            var temp;
            for (var i = 0; i < document.getElementById("<%=txtmPostcode.ClientID %>").value.length; i++) {
                temp = document.getElementById("<%=txtmPostcode.ClientID%>").value.substring(i, i + 1);
                if (digits.indexOf(temp) == -1) {
                    alert("Enter Valid Correct PostalCode");
                    document.getElementById("<%=txtmPostcode.ClientID%>").value == "";
                    document.getElementById("<%=txtmPostcode.ClientID%>").focus();
                    return false;
                }
            }

            var digits = "0123456789";
            var temp;
            for (var i = 0; i < document.getElementById("<%=txtCreditHrs.ClientID %>").value.length; i++) {
                temp = document.getElementById("<%=txtCreditHrs.ClientID%>").value.substring(i, i + 1);
                if (digits.indexOf(temp) == -1) {
                    alert("Enter Valid Correct CreditHrs");
                    document.getElementById("<%=txtCreditHrs.ClientID%>").focus();
                    return false;
                }
            }





            return true;


        }
        function DateCopare() {

            if (document.getElementById("<%=txtSpn1.ClientID%>").value == "") {
                alert("Sponsor Field Cannot Be Blank");
                document.getElementById("<%=txtSpn1.ClientID%>").focus();
                return false;
            }
            if (document.getElementById("<%=txtVFrom1.ClientID%>").value == "") {
                alert("From Date Field Cannot Be Blank");
                document.getElementById("<%=txtVFrom1.ClientID%>").focus();
                return false;
            }
            if (document.getElementById("<%=txtspTo1.ClientID%>").value == "") {
                alert("To Date Field Cannot Be Blank");
                document.getElementById("<%=txtspTo1.ClientID%>").focus();
                return false;
            }


            var len = document.getElementById("<%=txtVFrom1.ClientID%>").value
            var RegExPattern = /^((((0?[1-9]|[12]\d|3[01])[\.\-\/](0?[13578]|1[02])[\.\-\/]((1[6-9]|[2-9]\d)?\d{2}))|((0?[1-9]|[12]\d|30)[\.\-\/](0?[13456789]|1[012])[\.\-\/]((1[6-9]|[2-9]\d)?\d{2}))|((0?[1-9]|1\d|2[0-8])[\.\-\/]0?2[\.\-\/]((1[6-9]|[2-9]\d)?\d{2}))|(29[\.\-\/]0?2[\.\-\/]((1[6-9]|[2-9]\d)?(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00)|00)))|(((0[1-9]|[12]\d|3[01])(0[13578]|1[02])((1[6-9]|[2-9]\d)?\d{2}))|((0[1-9]|[12]\d|30)(0[13456789]|1[012])((1[6-9]|[2-9]\d)?\d{2}))|((0[1-9]|1\d|2[0-8])02((1[6-9]|[2-9]\d)?\d{2}))|(2902((1[6-9]|[2-9]\d)?(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00)|00))))$/;
            var errorMessage = 'Enter Valid From Date in dd/mm/yyyy format.';
            if (document.getElementById("<%=txtVFrom1.ClientID%>").value.match(RegExPattern)) {
                if (len.length == 8) {
                    alert(errorMessage);
                    document.getElementById("<%=txtVFrom1.ClientID%>").value = "";
                    document.getElementById("<%=txtVFrom1.ClientID%>").focus();
                    return false;
                }
            }
            else {
                alert(errorMessage);
                document.getElementById("<%=txtVFrom1.ClientID%>").value = "";
                document.getElementById("<%=txtVFrom1.ClientID%>").focus();
                return false;
            }

            if (document.getElementById("<%=txtspTo1.ClientID%>").value.match(RegExPattern)) {
                if (len.length == 8) {
                    alert(errorMessage);
                    document.getElementById("<%=txtspTo1.ClientID%>").value = "";
                    document.getElementById("<%=txtspTo1.ClientID%>").focus();
                    return false;
                }
            }
            else {
                alert(errorMessage);
                document.getElementById("<%=txtspTo1.ClientID%>").value = "";
                document.getElementById("<%=txtspTo1.ClientID%>").focus();
                return false;
            }






            var str1 = document.getElementById("<%=txtVFrom1.ClientID %>").value;
            var str2 = document.getElementById("<%=txtspTo1.ClientID %>").value;
            var dt1 = parseInt(str1.substring(0, 2), 10);
            var mon1 = parseInt(str1.substring(3, 5), 10);
            var yr1 = parseInt(str1.substring(6, 10), 10);
            var dt2 = parseInt(str2.substring(0, 2), 10);
            var mon2 = parseInt(str2.substring(3, 5), 10);
            var yr2 = parseInt(str2.substring(6, 10), 10);
            var date1 = new Date(yr1, mon1, dt1);
            var date2 = new Date(yr2, mon2, dt2);




            if (date2 < date1) {
                alert("To Date Should be greater than From Date");
                document.getElementById("<%=txtspTo1.ClientID%>").value = "";
                document.getElementById("<%=txtspTo1.ClientID%>").focus();
                return false;
            }
//            validate()
            return true;


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
                                <asp:ImageButton ID="ibtnPrint" runat="server" ImageUrl="~/images/print.png" ToolTip="Print"
                                    Target="_blank" />
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
                        Width="52px" ReadOnly="true" CssClass="text_box" AutoPostBack="True" Style="text-align: right"
                        MaxLength="7"></asp:TextBox>
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
                <td align="right" class="pagetext" style="height: 39px">
                    <asp:Label ID="lblMenuName" runat="server" Text="Student Info" Width="253px"></asp:Label>
                </td>
            </tr>
        </table>
        <asp:Label ID="lblMsg" runat="server" CssClass="lblError" Style="text-align: center"
            Width="348px"></asp:Label></asp:Panel>
 
    <asp:Panel ID="PnlAdd" runat="server" Width="100%" Visible="true">
        <table>
            <tbody>
                <tr>
                    <td style="width: 105px">
                        <!--<asp:Image id="imgLeft1" runat="server" ImageUrl="images/b_orange_left.gif" ImageAlign="AbsBottom"></asp:Image>-->
                        <asp:Button ID="btnBatchInvoice" OnClick="btnBatchInvoice_Click" runat="server" Text="Student Info"
                            Width="108px" CssClass="TabButton" Height="24px"></asp:Button><!--<asp:Image id="imgRight1" runat="server" ImageUrl="images/b_orange_right.gif" ImageAlign="AbsBottom"></asp:Image>-->
                    </td>
                    <td style="width: 137px">
                        <!--<asp:Image id="imgLeft2" runat="server" ImageUrl="images/b_orange_left.gif" ImageAlign="AbsBottom"></asp:Image>-->
                        <asp:Button ID="btnSelection" OnClick="btnSelection_Click" runat="server" Text="Sponsor"
                            Width="108px" CssClass="TabButton" Height="25px"></asp:Button><!--<asp:Image id="imgRight2" runat="server" ImageUrl="images/b_orange_right.gif" ImageAlign="AbsBottom"></asp:Image>-->
                    </td>
                </tr>
            </tbody>
        </table>
        <asp:MultiView ID="MultiView1" runat="server">
            <asp:View ID="View1" runat="server">
                <table style="width: 100%">
                    <tr>
                        <td style="width: 100%">
                           <div  style="border: thin solid #A6D9F4; width: 100%">
                                <table class="fields" style="width: 100%; height: 100%;">
                                    <tr>
                                        <td style="height: 21px; width: 2px;">
                                        </td>
                                        <td class="fields" style="height: 21px; text-align: left;" colspan="8">
                                            <asp:Button ID="btnStuNotes" runat="server" Text="Notes" Enabled="False" OnClick="btnStuNotes_Click1" />
                                                       
                                                       
                                                      
                                                
                                        </td>
                                        <td style="height: 21px; width: 17px;">
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="fields" colspan="10" style="height: 21px">
                                            <table class="mainbg" style="width: 100%;">
                                                <tr>
                                                    <td class="vline" style="width: 100%; height: 1px">
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="height: 21px; width: 2px;">
                                            <span style="font-size: 11pt; color: #ff0066; font-family: Arial">*</span></td>
                                        <td class="fields" style="width: 8px; height: 21px">
                                            <asp:Label ID="Label2" runat="server" Font-Bold="False" Text=" Matric No" Width="113px"></asp:Label>
                                        </td>
                                        <td style="width: 32px; height: 21px">
                                            <asp:TextBox ID="txtMatricNo" runat="server" MaxLength="20" Width="146px"></asp:TextBox>
                                        </td>
                                        <td class="fields" style="width: 72px; height: 21px">
                                            </td>
                                        <td class="fields" style="height: 21px; width: 163px;">
                                        </td>
                                        <td style="width: 28px; height: 21px; text-align: right;">
                                            <span style="font-size: 11pt; color: #ff0066">*</span></td>
                                        <td class="fields" style="width: 28px; height: 21px">
                                            <asp:Label ID="Label7" runat="server" Font-Bold="False" Text="Name" 
                                                Width="113px"></asp:Label>
                                        </td>
                                        <td style="width: 56px; height: 21px">
                                            <asp:TextBox ID="txtName" runat="server" Width="346px" MaxLength="50" TabIndex="1"></asp:TextBox>
                                        </td>
                                        <td style="height: 21px; width: 3px;">
                                        </td>
                                        <td style="height: 21px; width: 17px;">
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 28px; height: 14px; text-align: right;">
                                            <span style="font-size: 11pt; color: #ff0066">*</span></td>
                                        <td class="fields" style="width: 28px; height: 14px">
                                            <asp:Label ID="Label66" runat="server" Font-Bold="False" Text="Student Status" 
                                                Width="113px"></asp:Label>
                                        </td>
                                        <td style="width: 56px; height: 14px">
                                            <asp:DropDownList ID="ddlstudentStatus" runat="server" Width="153px" AppendDataBoundItems="True"
                                                TabIndex="2">
                                                <asp:ListItem Value="-1">--Select--</asp:ListItem>
                                            </asp:DropDownList>
                                            </td>

                                                <td class="fields" style="width: 72px; height: 14px"></td>
                                        <td class="fields" style="height: 14px; width: 163px;"></td>

                                                <td style="width: 28px; height: 14px; text-align: right;">
                                            <span style="font-size: 11pt; color: #ff0066">*</span></td>
                                                <td class="fields" style="width: 28px; height: 14px">
                                            <asp:Label ID="Label67" runat="server" Font-Bold="False" Text="Registration Status" Width="113px"></asp:Label>
                                        </td>
                                        <td style="width: 56px; height: 14px">
                                            <asp:DropDownList ID="ddlRegistrationStatus" runat="server" Width="153px" AppendDataBoundItems="True"
                                                TabIndex="2">
                                                <asp:ListItem Value="-1">--Select--</asp:ListItem>
                                                <asp:ListItem Value="1">Not Registered</asp:ListItem>
                                                <asp:ListItem Value="2">Registered</asp:ListItem>
                                            </asp:DropDownList>
                                            </td>                                        
                                        <td style="width: 28px; height: 14px; text-align: right;">&nbsp;</td>
                                        <td class="fields" style="width: 28px; height: 14px">&nbsp;</td>                                        
                                    </tr>
                                    <tr>
                                        <td style="height: 21px; width: 2px;">
                                        </td>
                                        <td class="fields" colspan="9" style="height: 21px; text-align: right;">
                                            <table class="mainbg" style="width: 100%;">
                                                <tr>
                                                    <td class="vline" style="width: 99%; height: 1px">
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="height: 15px; width: 2px;">
                                            <span style="font-size: 11pt; color: #ff0066">*</span></td>
                                        <td class="fields" style="width: 8px; height: 15px">
                                            <asp:Label ID="Label3" runat="server" Font-Bold="False" Text="ID Type" Width="113px"></asp:Label>
                                        </td>
                                        <td style="width: 32px; height: 15px">
                                            <asp:DropDownList ID="ddlIcNo" runat="server" TabIndex="3" Width="153px">
                                                <asp:ListItem Value="-1">--Select--</asp:ListItem>
                                                <asp:ListItem Value="1">IC No</asp:ListItem>
                                                <asp:ListItem Value="2">Passport No</asp:ListItem>
                                            </asp:DropDownList>
                                            </td>
                                        <td class="fields" style="width: 72px; height: 15px">
                                        </td>
                                        <td class="fields" style="height: 15px; width: 163px;">
                                        </td>
                                        <td style="width: 28px; height: 15px; text-align: right;">
                                            <span style="font-size: 11pt; color: #ff0066">*</span></td>
                                        <td class="fields" style="width: 28px; height: 15px">
                                            <asp:Label ID="Label52" runat="server" Font-Bold="False" Text="ID No" 
                                                Width="113px"></asp:Label>
                                        </td>
                                        <td style="width: 56px; height: 15px">
                                            <asp:TextBox ID="txtIcNo" runat="server" MaxLength="20" TabIndex="4" Width="146px"></asp:TextBox>
                                            </td>
                                        <td style="height: 15px; width: 3px;">
                                        </td>
                                        <td style="height: 15px; width: 17px;">
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="height: 4px; width: 2px;">
                                        </td>
                                        <td class="fields" style="width: 8px; height: 4px">
                                        </td>
                                        <td style="width: 32px; height: 4px">
                                        </td>
                                        <td class="fields" style="width: 72px; height: 4px">
                                        </td>
                                        <td class="fields" style="height: 4px; width: 163px;">
                                        </td>
                                        <td class="fields" style="width: 28px; height: 4px; text-align: right;">
                                            </td>
                                        <td class="fields" style="width: 28px; height: 4px">
                                            <asp:Label ID="Label29" runat="server" Font-Bold="False" Text="Other Ref Id" 
                                                Width="113px"></asp:Label>
                                        </td>
                                        <td style="width: 56px; height: 4px">
                                            <asp:TextBox ID="txtkolejgit" runat="server" Width="146px" TabIndex="6"></asp:TextBox>
                                        </td>
                                        <td style="height: 4px; width: 3px;">
                                        </td>
                                        <td style="height: 4px; width: 17px;">
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 2px; height: 15px">
                                            <span style="font-size: 11pt; color: #ff0066">*</span></td>
                                        <td class="fields" style="width: 8px; height: 15px">
                                            <asp:Label ID="Label8" runat="server" Font-Bold="False" Text="Faculty" Width="113px"></asp:Label>
                                        </td>
                                        <td style="width: 32px; height: 15px">
                                            <asp:DropDownList ID="ddlFaculty" runat="server" AppendDataBoundItems="True" AutoPostBack="True"
                                                TabIndex="7" Width="153px">
                                                <asp:ListItem Value="-1">--Select--</asp:ListItem>
                                            </asp:DropDownList>
                                            </td>
                                        <td class="fields" style="width: 72px; height: 15px">
                                            </td>
                                        <td class="fields" style="height: 15px; width: 163px;">
                                        </td>
                                        <td style="width: 28px; height: 11px; text-align: right;">
                                            <span style="font-size: 11pt; color: #ff0066; text-align: center;">*</span></td>
                                        <td class="fields" style="width: 28px; height: 15px">
                                            <asp:Label ID="Label4" runat="server" Font-Bold="False" Text="Program" 
                                                Width="113px"></asp:Label>
                                        </td>
                                        <td style="width: 56px; height: 15px">
                                            <asp:DropDownList ID="ddlProgram" runat="server" AppendDataBoundItems="True" AutoPostBack="True"
                                                OnSelectedIndexChanged="ddlProgram_SelectedIndexChanged" TabIndex="8" Width="153px">
                                                <asp:ListItem Value="-1">--Select--</asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                        <td style="width: 3px; height: 15px">
                                        </td>
                                        <td style="width: 17px; height: 15px">
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="height: 11px; width: 2px;">
                                            <span style="font-size: 11pt; color: #ff0066">* </span>
                                        </td>
                                        <td class="fields" style="width: 8px; height: 11px;">
                                            <asp:Label ID="Label64" runat="server" Font-Bold="False" Text="Intake Semester" Width="113px"></asp:Label>
                                        </td>
                                        <td style="width: 32px; height: 11px;">
                                            <asp:DropDownList ID="ddlIntkSemester" runat="server" Width="153px" AppendDataBoundItems="True"
                                                AutoPostBack="True" TabIndex="15">
                                                <asp:ListItem Value="-1">--Select--</asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                        <td class="fields" style="width: 72px; height: 11px">
                                        </td>
                                        <td class="fields" style="height: 11px; width: 163px;">
                                        </td>
                                        <td style="width: 28px; height: 11px; text-align: right;">
                                            <span style="font-size: 11pt; color: #ff0066">*</span></td>
                                        <td class="fields" style="width: 28px; height: 11px;">
                                            <asp:Label ID="Label15" runat="server" Font-Bold="False" Text="Intake Session" 
                                                Width="113px"></asp:Label>
                                        </td>
                                        <td style="width: 56px; height: 11px;">
                                            <asp:DropDownList ID="ddlIntakeSession" runat="server" AppendDataBoundItems="True"
                                                Width="153px" TabIndex="15">
                                                <asp:ListItem Value="-1">--Select--</asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                        <td style="height: 11px; width: 3px;">
                                        </td>
                                        <td style="height: 11px; width: 17px;">
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 2px; height: 11px">
                                            <span style="font-size: 11pt; color: #ff0066">*</span></td>
                                        <td class="fields" style="width: 8px; height: 11px">
                                            <asp:Label ID="Label10" runat="server" Font-Bold="False" Text="Current Semester"
                                                Width="113px"></asp:Label>
                                        </td>
                                        <td style="width: 32px; height: 11px">
                                            <asp:DropDownList ID="ddlSem" runat="server" AppendDataBoundItems="True" Width="153px"
                                                TabIndex="9" AutoPostBack="True">
                                                <asp:ListItem Value="-1">--Select--</asp:ListItem>
                                            </asp:DropDownList>
                                            </td>
                                        <td class="fields" style="width: 72px; height: 11px">
                                        </td>
                                        <td class="fields" style="height: 11px; width: 163px;">
                                        </td>
                                        <td style="width: 28px; height: 11px; text-align: right;">
                                            <span style="font-size: 11pt; color: #ff0066">*</span></td>
                                        <td class="fields" style="width: 28px; height: 11px">
                                            <asp:Label ID="Label51" runat="server" Font-Bold="False" 
                                                Text="Current  Session" Width="113px"></asp:Label>
                                        </td>
                                        <td style="width: 56px; height: 11px">
                                            <span style="font-size: 11pt; color: #ff0066">
                                                <asp:DropDownList ID="ddlCurSession" runat="server" AppendDataBoundItems="True" TabIndex="15"
                                                    Width="153px">
                                                    <asp:ListItem Value="-1">--Select--</asp:ListItem>
                                                </asp:DropDownList>
                                                </span>
                                        </td>
                                        <td style="width: 3px; height: 11px">
                                        </td>
                                        <td style="width: 17px; height: 11px">
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 2px;">
                                            <span style="font-size: 11pt; color: #ff0066">*</span></td>
                                        <td class="fields" style="width: 8px;">
                                            <asp:Label ID="Label20" runat="server" Font-Bold="False" Text="Study Type" Width="113px"></asp:Label>
                                        </td>
                                        <td style="width: 32px;">
                                            <asp:DropDownList ID="ddlStudyType" runat="server" Width="153px" TabIndex="11">
                                                <asp:ListItem Value="-1">--Select--</asp:ListItem>
                                                <asp:ListItem Value="1">Full Time</asp:ListItem>
                                                <asp:ListItem Value="2">Part Time</asp:ListItem>
                                            </asp:DropDownList>
                                            </td>
                                        <td class="fields" style="width: 72px;">
                                        </td>
                                        <td class="fields" style="width: 163px">
                                        </td>
                                        <td style="width: 28px; text-align: right;">
                                            <span style="font-size: 11pt; color: #ff0066">*</span></td>
                                        <td class="fields" style="width: 28px;">
                                            <asp:Label ID="Label22" runat="server" Font-Bold="False" 
                                                Style="vertical-align: middle" Text="Student Category" Width="110px"></asp:Label>
                                        </td>
                                        <td style="width: 56px;">
                                            <asp:DropDownList ID="ddlStudentCategory" runat="server" AppendDataBoundItems="True"
                                                AutoPostBack="True" OnSelectedIndexChanged="ddlStudentCategory_SelectedIndexChanged"
                                                TabIndex="12" Width="153px">
                                                <asp:ListItem Value="-1">--Select--</asp:ListItem>
                                            </asp:DropDownList>
                                            
                                        </td>
                                        <td style="width: 3px;">
                                        </td>
                                        <td style="width: 17px;">
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 2px; height: 14px;">
                                            <span style="font-size: 11pt; color: #ff0066">*</span></td>
                                        <td class="fields" style="width: 8px; height: 14px;">
                                            <asp:Label ID="Label9" runat="server" Font-Bold="False" Text="Bank " Width="113px"></asp:Label>
                                        </td>
                                        <td style="width: 32px; height: 14px;">
                                            <asp:DropDownList ID="ddlBank" runat="server" Width="153px" TabIndex="13" AppendDataBoundItems="True">
                                                <asp:ListItem Value="-1">--Select--</asp:ListItem>
                                            </asp:DropDownList>
                                            </td>
                                        <td class="fields" style="width: 72px; height: 14px;">
                                        </td>
                                        <td class="fields" style="height: 14px; width: 163px;">
                                        </td>
                                        <td style="width: 28px; height: 14px; text-align: right;">
                                            <span style="font-size: 11pt; color: #ff0066">*</span></td>
                                        <td class="fields" style="width: 28px; height: 14px;">
                                            <asp:Label ID="Label12" runat="server" Font-Bold="False" Text="Acc.No" 
                                                Width="54px"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtAccNo" runat="server" MaxLength="20" TabIndex="14" Width="146px"></asp:TextBox>
                                            </td>
                                        <td style="width: 3px; height: 14px;">
                                        </td>
                                        <td style="width: 17px; height: 14px;">
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="height: 15px; width: 2px;">
                                            <span style="font-size: 11pt; color: #ff0066">*</span></td>
                                        <td class="fields" style="width: 8px; height: 15px">
                                            <asp:Label ID="Label65" runat="server" Font-Bold="False" Text="Fee Category" Width="110px"
                                                Style="vertical-align: middle"></asp:Label>
                                        </td>
                                        <td style="width: 32px; height: 15px">
                                            <asp:DropDownList ID="ddlFeeCat" runat="server" Width="153px" AppendDataBoundItems="True"
                                                TabIndex="12">
                                                <asp:ListItem Value="-1">--Select--</asp:ListItem>
                                            </asp:DropDownList>
                                            </td>
                                        <td class="fields" style="width: 72px; height: 15px">
                                        </td>
                                        <td class="fields" style="height: 15px; width: 163px;">
                                        </td>
                                        <td class="fields" style="width: 28px; height: 15px; text-align: right;">
                                            </td>
                                        <td class="fields" style="width: 28px; height: 15px">
                                            <asp:Label ID="lblKoko" runat="server" Font-Bold="False" 
                                                Style="vertical-align: middle" Text="Kokurikulum" Width="110px"></asp:Label>
                                        </td>
                                        <td style="width: 56px; height: 15px">
                                            <asp:DropDownList ID="ddlKokoList" runat="server" AppendDataBoundItems="True" 
                                                AutoPostBack="True" 
                                                OnSelectedIndexChanged="ddlStudentCategory_SelectedIndexChanged" TabIndex="12" 
                                                Width="153px">
                                                <asp:ListItem Value="-1">--Select--</asp:ListItem>
                                            </asp:DropDownList>
                                            </td>
                                        <td style="height: 15px; width: 3px;">
                                        </td>
                                        <td style="height: 15px; width: 17px;">
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="height: 16px; width: 2px;">
                                        </td>
                                        <td class="fields" style="height: 16px; text-align: right;" colspan="9">
                                            <span style="font-size: 11pt;
                                                color: #ff0066">
                                                <table class="mainbg" style="width: 100%;">
                                                    <tr>
                                                        <td class="vline" style="width: 100%; height: 1px">
                                                        </td>
                                                    </tr>
                                                </table>
                                            </span>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="height: 14px; width: 2px;">
                                            <span style="font-size: 11pt; color: #ff0066">*</span></td>
                                        <td class="fields" style="height: 14px; width: 8px;">
                                            <asp:Label ID="Label23" runat="server" Font-Bold="False" Text="Hostel" Width="113px"></asp:Label>
                                        </td>
                                        <td style="width: 32px; height: 14px;">
                                            <asp:DropDownList ID="ddlHostel" runat="server" AppendDataBoundItems="True" AutoPostBack="True"
                                                TabIndex="16" Width="153px">
                                                <asp:ListItem Value="-1">--Select--</asp:ListItem>
                                                <asp:ListItem Value="1">Yes</asp:ListItem>
                                                <asp:ListItem Value="0">No</asp:ListItem>
                                            </asp:DropDownList>
                                            </td>
                                        <td class="fields" style="width: 72px; height: 14px">
                                            </td>
                                        <td class="fields" style="height: 14px; width: 163px;">
                                        </td>
                                        <td class="fields" style="width: 28px; height: 14px; text-align: right;">
                                            </td>
                                        <td class="fields" style="width: 28px; height: 14px;">
                                            <asp:Label ID="Label19" runat="server" Font-Bold="False" Text="Credit Hrs" 
                                                Width="113px"></asp:Label>
                                        </td>
                                        <td style="width: 56px; height: 14px;">
                                            <span style="font-size: 11pt; color: #ff0066">
                                                <asp:TextBox ID="txtCreditHrs" runat="server" TabIndex="21" Width="146px"></asp:TextBox>
                                            </span>
                                        </td>
                                        <td style="height: 14px; width: 3px;">
                                        </td>
                                        <td style="height: 14px; width: 17px;">
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="height: 19px; width: 2px;">
                                        </td>
                                        <td class="fields" style="width: 8px; height: 19px;">
                                            <asp:Label ID="Label28" runat="server" Font-Bold="False" Text="Kolej" Width="113px"></asp:Label>
                                        </td>
                                        <td style="width: 32px; height: 19px;" class="fields">
                                            <asp:DropDownList ID="ddlKolej" runat="server" AppendDataBoundItems="True" Width="153px"
                                                TabIndex="17" AutoPostBack="True">
                                                <asp:ListItem Value="-1">--Select--</asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                        <td class="fields" style="width: 72px; height: 19px">
                                            </td>
                                        <td class="fields" style="height: 19px; width: 163px;">
                                        </td>
                                        <td class="fields" style="width: 28px; height: 19px; text-align: right;">
                                            </td>
                                        <td class="fields" style="width: 28px; height: 19px;">
                                            <asp:Label ID="Label26" runat="server" Font-Bold="False" Text="GPA" 
                                                Width="113px"></asp:Label>
                                        </td>
                                        <td style="width: 56px; height: 19px;">
                                            <span style="font-size: 11pt; color: #ff0066">
                                                <asp:TextBox ID="txtGpa" runat="server" Width="146px" TabIndex="22"></asp:TextBox>
                                            </span>
                                        </td>
                                        <td style="height: 19px; width: 3px;">
                                        </td>
                                        <td style="height: 19px; width: 17px;">
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="height: 15px; width: 2px;">
                                        </td>
                                        <td class="fields" style="width: 8px; height: 15px;">
                                            <asp:Label ID="Label25" runat="server" Font-Bold="False" Text="Block" Width="113px"></asp:Label>
                                        </td>
                                        <td class="fields" style="width: 32px; height: 15px;">
                                            <asp:DropDownList ID="ddlblock" runat="server" AppendDataBoundItems="True" AutoPostBack="True"
                                                Width="153px" TabIndex="18">
                                                <asp:ListItem Value="-1">--Select--</asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                        <td class="fields" style="width: 72px; height: 15px">
                                            </td>
                                        <td class="fields" style="height: 15px; width: 163px;">
                                        </td>
                                        <td class="fields" style="width: 28px; height: 15px; text-align: right;">
                                            </td>
                                        <td class="fields" style="width: 28px; height: 15px">
                                            <asp:Label ID="Label24" runat="server" Font-Bold="False" Text="Session Intake" 
                                                Width="113px"></asp:Label>
                                        </td>
                                        <td style="width: 56px; height: 15px;">
                                            <asp:TextBox ID="txtcgpa" runat="server" TabIndex="23" Width="146px"></asp:TextBox>
                                        </td>
                                        <td style="height: 15px; width: 3px;">
                                        </td>
                                        <td style="height: 15px; width: 17px;">
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="height: 17px; width: 2px;">
                                        </td>
                                        <td class="fields" style="width: 8px; height: 17px;">
                                            <asp:Label ID="Label58" runat="server" Font-Bold="False" Text="Room Type" Width="113px"></asp:Label>
                                        </td>
                                        <td class="fields" style="width: 32px; height: 17px;">
                                            <asp:DropDownList ID="ddlRoomType" runat="server" AppendDataBoundItems="True" Width="153px"
                                                AutoPostBack="True" TabIndex="19">
                                                <asp:ListItem Value="-1">--Select--</asp:ListItem>
                                            </asp:DropDownList>
                                            
                                        </td>
                                        <td class="fields" style="width: 72px; height: 17px">
                                            <span style="font-size: 11pt; color: #ff0066; font-family: Arial"></span>
                                        </td>
                                        <td class="fields" style="height: 17px; width: 163px;">
                                        </td>
                                        <td class="fields" style="width: 28px; height: 17px">
                                            </td>
                                        <td class="fields" style="width: 28px; height: 17px">
                                        </td>
                                        <td style="width: 56px; height: 17px;" class="fields">
                                            <span style="font-size: 11pt; color: #ff0066; font-family: Arial"></span>
                                        </td>
                                        <td style="height: 17px; width: 3px;" class="fields">
                                        </td>
                                        <td style="height: 17px; width: 17px;" class="fields">
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="height: 10px; width: 2px;">
                                        </td>
                                        <td class="fields" style="width: 8px; height: 10px; vertical-align: middle;">
                                            <asp:Label ID="Label27" runat="server" Font-Bold="False" Text="Room Number" Width="113px"></asp:Label>
                                        </td>
                                        <td class="fields" style="width: 32px; height: 10px;">
                                            <span style="font-size: 11pt; color: #ff0066; font-family: Arial">
                                                <asp:TextBox ID="txtFloorNo" runat="server" TabIndex="20" Width="146px"></asp:TextBox>
                                            </span>
                                        </td>
                                        <td class="fields" style="width: 72px; height: 10px">
                                            <span style="font-size: 11pt; color: #ff0066; font-family: Arial"></span>
                                        </td>
                                        <td class="fields" style="height: 10px; width: 163px;">
                                        </td>
                                        <td class="fields" style="width: 28px; height: 10px;">
                                            </td>
                                        <td class="fields" style="width: 28px; height: 10px;">
                                            <asp:Label ID="Label48" runat="server" Style="vertical-align: middle" 
                                                Text="Status" Visible="False" Width="78px"></asp:Label>
                                        </td>
                                        <td style="width: 56px; height: 10px; vertical-align: middle;">
                                            <asp:DropDownList ID="ddlStatus" runat="server" Visible="False" Width="153px">
                                                <asp:ListItem Value="-1">--Select--</asp:ListItem>
                                                <asp:ListItem Value="1">Active</asp:ListItem>
                                                <asp:ListItem Value="0">Inactive</asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                        <td style="height: 10px; width: 3px;">
                                        </td>
                                        <td style="height: 10px; width: 17px;">
                                        </td>
                                    </tr>
                                </table>
                                <table class="mainbg" style="width: 100%;">
                                    <tr>
                                        <td class="vline" style="width: 100%; height: 1px">
                                        </td>
                                    </tr>
                                </table>
                                <table width="100%">
                                    <tr>
                                        <td>
                                        </td>
                                        <td class="fields" width="12%">
                                        </td>
                                        <td class="fields" width="40%">
                                        </td>
                                        <td class="fields" >
                                        </td>
                                        <td class="fields" width="11%">
                                        </td>
                                        <td class="fields" >
                                        </td>
                                        <td >
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 1px; height: 16px">
                                        </td>
                                        <td class="fields">
                                        </td>
                                        <td class="fields">
                                            <asp:Label ID="Label56" runat="server" Font-Bold="False" Text="Address" Width="113px"></asp:Label>
                                            <asp:CheckBox ID="chkmadd" runat="server" AutoPostBack="True" Text="Copy" TabIndex="50" />
                                        </td>
                                        <td class="fields" >
                                        </td>
                                        <td class="fields" >
                                        </td>
                                        <td class="fields" >
                                            <asp:Label ID="Label57" runat="server" Font-Bold="False" Text="Mail Address" Width="79px"></asp:Label>
                                                 
                                        </td>
                                        <td >
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 1px; height: 16px">
                                        </td>
                                        <td class="fields">
                                            <asp:Label ID="Label46" runat="server" Font-Bold="False" Text="Address" Width="113px"></asp:Label>
                                        </td>
                                        <td class="fields">
                                            <asp:TextBox ID="txtAdd1" runat="server" Width="300px" MaxLength="50" TabIndex="33"></asp:TextBox>
                                        </td>
                                        <td class="fields" >
                                        </td>
                                        <td class="fields" >
                                            <asp:Label ID="Label49" runat="server" Font-Bold="False" Text="Address" Width="63px"
                                                Height="15px"></asp:Label>
                                        </td>
                                        <td class="fields" >
                                            <asp:TextBox ID="txtmAddress1" runat="server" MaxLength="50" Width="300px" TabIndex="43"></asp:TextBox>
                                        </td>
                                        <td >
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 1px; height: 16px">
                                        </td>
                                        <td>
                                        </td>
                                        <td class="fields">
                                            <asp:TextBox ID="txtAdd2" runat="server" Width="300px" MaxLength="50" TabIndex="34"></asp:TextBox>
                                        </td>
                                        <td >
                                        </td>
                                        <td >
                                        </td>
                                        <td >
                                            <asp:TextBox ID="txtmAddress2" runat="server" MaxLength="50" Width="300px" TabIndex="44"></asp:TextBox>
                                        </td>
                                        <td >
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 1px; height: 16px">
                                        </td>
                                        <td>
                                        </td>
                                        <td class="fields">
                                            <asp:TextBox ID="txtAdd3" runat="server" MaxLength="50" Width="300px" TabIndex="35"></asp:TextBox>
                                        </td>
                                        <td >
                                        </td>
                                        <td >
                                        </td>
                                        <td >
                                            <asp:TextBox ID="txtmAddress3" runat="server" MaxLength="50" Width="300px" TabIndex="45"></asp:TextBox>
                                        </td>
                                        <td >
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 1px; height: 16px">
                                        </td>
                                        <td class="fields">
                                            <asp:Label ID="Label39" runat="server" Font-Bold="False" Text="City" Width="113px"></asp:Label>
                                        </td>
                                        <td class="fields">
                                            <asp:TextBox ID="txtCity" runat="server" Width="300px" MaxLength="50" TabIndex="36"></asp:TextBox>
                                        </td>
                                        <td class="fields" >
                                        </td>
                                        <td class="fields" >
                                            <asp:Label ID="Label50" runat="server" Font-Bold="False" Text="City" Width="62px"
                                                Height="16px"></asp:Label>
                                        </td>
                                        <td class="fields" >
                                            <asp:TextBox ID="txtmCity" runat="server" MaxLength="50" Width="300px" TabIndex="46"></asp:TextBox>
                                        </td>
                                        <td class="fields" >
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 1px; height: 16px">
                                        </td>
                                        <td class="fields">
                                            <asp:Label ID="Label40" runat="server" Font-Bold="False" Text="State" Width="113px"></asp:Label>
                                        </td>
                                        <td class="fields">
                                            <asp:TextBox ID="txtState" runat="server" Width="300px" MaxLength="50" TabIndex="37"></asp:TextBox>
                                        </td>
                                        <td class="fields" >
                                        </td>
                                        <td class="fields" >
                                            <asp:Label ID="Label53" runat="server" Font-Bold="False" Text="State" Width="60px"
                                                Height="8px"></asp:Label>
                                        </td>
                                        <td class="fields" >
                                            <asp:TextBox ID="txtmState" runat="server" MaxLength="50" Width="300px" TabIndex="47"></asp:TextBox>
                                        </td>
                                        <td >
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 1px; height: 16px">
                                        </td>
                                        <td class="fields">
                                            <asp:Label ID="Label41" runat="server" Font-Bold="False" Text="Country" Width="113px"></asp:Label>
                                        </td>
                                        <td class="fields">
                                            <asp:TextBox ID="txtCountry" runat="server" Width="300px" MaxLength="50" Visible="False"></asp:TextBox>
                                            <asp:DropDownList ID="ddlCountry" runat="server" Width="205px" TabIndex="38">
                                                <asp:ListItem Value="-1">--Select--</asp:ListItem>
                                                <asp:ListItem Value="us">U.S.</asp:ListItem>
                                                <asp:ListItem Value="cn">China</asp:ListItem>
                                                <asp:ListItem Value="uk">United Kingdom</asp:ListItem>
                                                <asp:ListItem Value="ar">Argentina</asp:ListItem>
                                                <asp:ListItem Value="aa">Asia</asp:ListItem>
                                                <asp:ListItem Value="au">Australia</asp:ListItem>
                                                <asp:ListItem Value="br">Brazil</asp:ListItem>
                                                <asp:ListItem Value="ca">Canada in English</asp:ListItem>
                                                <asp:ListItem Value="cf">Canada in French</asp:ListItem>
                                                <asp:ListItem Value="dk">Denmark</asp:ListItem>
                                                <asp:ListItem Value="fr">France </asp:ListItem>
                                                <asp:ListItem Value="de">Germany</asp:ListItem>
                                                <asp:ListItem Value="gr">Greece</asp:ListItem>
                                                <asp:ListItem Value="hk">Hong Kong</asp:ListItem>
                                                <asp:ListItem Value="in">India</asp:ListItem>
                                                <asp:ListItem Value="id">Indonesia</asp:ListItem>
                                                <asp:ListItem Value="ie">Ireland</asp:ListItem>
                                                <asp:ListItem Value="it">Italy</asp:ListItem>
                                                <asp:ListItem Value="kr">Korea</asp:ListItem>
                                                <asp:ListItem Value="my">Malaysia</asp:ListItem>
                                                <asp:ListItem Value="mx">Mexico</asp:ListItem>
                                                <asp:ListItem Value="nz">New Zealand</asp:ListItem>
                                                <asp:ListItem Value="no">Norway</asp:ListItem>
                                                <asp:ListItem Value="ph">Philippines</asp:ListItem>
                                                <asp:ListItem Value="pl">Poland</asp:ListItem>
                                                <asp:ListItem Value="sg">Singapore</asp:ListItem>
                                                <asp:ListItem Value="es">Spain</asp:ListItem>
                                                <asp:ListItem Value="se">Sweden</asp:ListItem>
                                                <asp:ListItem Value="tw">Taiwan</asp:ListItem>
                                                <asp:ListItem Value="th">Thailand</asp:ListItem>
                                                <asp:ListItem Value="tr">Turkey</asp:ListItem>
                                                <asp:ListItem Value="vn">Vietnam</asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                        <td >
                                        </td>
                                        <td class="fields" >
                                            <asp:Label ID="Label54" runat="server" Font-Bold="False" Text="Country" Width="61px"
                                                Height="15px"></asp:Label>
                                        </td>
                                        <td >
                                            <asp:DropDownList ID="ddlmCountry" runat="server" Width="205px" TabIndex="48">
                                                <asp:ListItem Value="-1">--Select--</asp:ListItem>
                                                <asp:ListItem Value="us">U.S.</asp:ListItem>
                                                <asp:ListItem Value="cn">China</asp:ListItem>
                                                <asp:ListItem Value="uk">United Kingdom</asp:ListItem>
                                                <asp:ListItem Value="ar">Argentina</asp:ListItem>
                                                <asp:ListItem Value="aa">Asia</asp:ListItem>
                                                <asp:ListItem Value="au">Australia</asp:ListItem>
                                                <asp:ListItem Value="br">Brazil</asp:ListItem>
                                                <asp:ListItem Value="ca">Canada in English</asp:ListItem>
                                                <asp:ListItem Value="cf">Canada in French</asp:ListItem>
                                                <asp:ListItem Value="dk">Denmark</asp:ListItem>
                                                <asp:ListItem Value="fr">France </asp:ListItem>
                                                <asp:ListItem Value="de">Germany</asp:ListItem>
                                                <asp:ListItem Value="gr">Greece</asp:ListItem>
                                                <asp:ListItem Value="hk">Hong Kong</asp:ListItem>
                                                <asp:ListItem Value="in">India</asp:ListItem>
                                                <asp:ListItem Value="id">Indonesia</asp:ListItem>
                                                <asp:ListItem Value="ie">Ireland</asp:ListItem>
                                                <asp:ListItem Value="it">Italy</asp:ListItem>
                                                <asp:ListItem Value="kr">Korea</asp:ListItem>
                                                <asp:ListItem Value="my">Malaysia</asp:ListItem>
                                                <asp:ListItem Value="mx">Mexico</asp:ListItem>
                                                <asp:ListItem Value="nz">New Zealand</asp:ListItem>
                                                <asp:ListItem Value="no">Norway</asp:ListItem>
                                                <asp:ListItem Value="ph">Philippines</asp:ListItem>
                                                <asp:ListItem Value="pl">Poland</asp:ListItem>
                                                <asp:ListItem Value="sg">Singapore</asp:ListItem>
                                                <asp:ListItem Value="es">Spain</asp:ListItem>
                                                <asp:ListItem Value="se">Sweden</asp:ListItem>
                                                <asp:ListItem Value="tw">Taiwan</asp:ListItem>
                                                <asp:ListItem Value="th">Thailand</asp:ListItem>
                                                <asp:ListItem Value="tr">Turkey</asp:ListItem>
                                                <asp:ListItem Value="vn">Vietnam</asp:ListItem>
                                            </asp:DropDownList>
                                            <asp:TextBox ID="txtmCountry" runat="server" MaxLength="50" Width="341px" Visible="False"></asp:TextBox>
                                        </td>
                                        <td class="fields" >
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="fields" style="width: 1px; height: 16px">
                                        </td>
                                        <td class="fields">
                                            <asp:Label ID="Label42" runat="server" Font-Bold="False" Text="Postal Code" Width="113px"></asp:Label>
                                        </td>
                                        <td class="fields">
                                            <asp:TextBox ID="txtPostcode" runat="server" Width="146px" MaxLength="20" TabIndex="39"></asp:TextBox>
                                        </td>
                                        <td >
                                        </td>
                                        <td class="fields" >
                                            <asp:Label ID="Label55" runat="server" Font-Bold="False" Text="Postal Code" Width="80px"
                                                Height="13px"></asp:Label>
                                        </td>
                                        <td class="fields" >
                                            <asp:TextBox ID="txtmPostcode" runat="server" MaxLength="20" Width="146px" TabIndex="49"></asp:TextBox>
                                        </td>
                                        <td >
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 1px; height: 16px">
                                        </td>
                                        <td class="fields">
                                            <asp:Label ID="Label43" runat="server" Font-Bold="False" Text="Email" Width="113px"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtEmail" runat="server" Width="300px" MaxLength="50" TabIndex="40"></asp:TextBox>
                                        </td>
                                        <td >
                                        </td>
                                        <td >
                                        </td>
                                        <td >
                                        </td>
                                        <td >
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 1px; height: 16px">
                                        </td>
                                        <td class="fields">
                                            <asp:Label ID="Label44" runat="server" Font-Bold="False" Text="Phone" Width="113px"></asp:Label>
                                        </td>
                                        <td class="fields">
                                            <asp:TextBox ID="txtPhone" runat="server" Width="146px" MaxLength="20" TabIndex="41"></asp:TextBox>
                                        </td>
                                        <td >
                                        </td>
                                        <td >
                                        </td>
                                        <td >
                                        </td>
                                        <td >
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 1px; height: 16px">
                                        </td>
                                        <td class="fields">
                                            <asp:Label ID="Label45" runat="server" Font-Bold="False" Text="Mobile No" Width="113px"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtmoblieno" runat="server" Width="146px" MaxLength="20" TabIndex="42"></asp:TextBox>
                                        </td>
                                        <td >
                                        </td>
                                        <td >
                                        </td>
                                        <td >
                                        </td>
                                        <td >
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 1px; height: 16px">
                                        </td>
                                        <td class="fields">
                                        </td>
                                        <td>
                                        </td>
                                        <td >
                                        </td>
                                        <td >
                                        </td>
                                        <td >
                                        </td>
                                        <td >
                                        </td>
                                    </tr>
                                </table>
                           </div>
                        </td>
                    </tr>
                </table>
            </asp:View>
            <asp:View ID="View2" runat="server">
                <table width="100%">
                    <tr>
                        <td style="height: 25px">
                        </td>
                        <td class="fields" colspan="11" style="height: 25px">
                            <asp:DataGrid ID="dgStuSponser" runat="server" AutoGenerateColumns="False" DataKeyField="Sponsor"
                                OnSelectedIndexChanged="dgStuSponser_SelectedIndexChanged" Width="420px">
                                <FooterStyle CssClass="dgFooterStyle" Height="20px" />
                                <SelectedItemStyle CssClass="dgSelectedItemStyle" />
                                <AlternatingItemStyle BackColor="Beige" CssClass="dgAlternatingItemStyle" Font-Bold="False"
                                    Font-Italic="False" Font-Overline="False" Font-Strikeout="False" Font-Underline="False" />
                                <ItemStyle CssClass="dgItemStyle" />
                                <Columns>
                                    <asp:ButtonColumn CommandName="Select" DataTextField="Sponsor" HeaderText="Sponsor Code"
                                        Text="Sponsor"></asp:ButtonColumn>
                                    <asp:BoundColumn DataField="Name" HeaderText="Sponsor Name"></asp:BoundColumn>
                                    <asp:BoundColumn DataField="SDate" DataFormatString="{0:dd/MM/yyyy}" HeaderText="Valid From">
                                    </asp:BoundColumn>
                                    <asp:BoundColumn HeaderText="Valid To" DataField="EDate" DataFormatString="{0:dd/MM/yyyy}">
                                    </asp:BoundColumn>
                                    <asp:BoundColumn HeaderText="Fully Sponsored" DataField="FullySponsered"></asp:BoundColumn>
                                    <asp:BoundColumn DataField="Sponsor" HeaderText="Sponsor" Visible="False"></asp:BoundColumn>
                                </Columns>
                                <HeaderStyle BackColor="#CDD7EE" CssClass="dgHeaderStyle" Font-Bold="True" Font-Italic="False"
                                    Font-Overline="False" Font-Size="Medium" Font-Strikeout="False" Font-Underline="False" />
                            </asp:DataGrid>
                        </td>
                    </tr>
                    <tr>
                        <td>
                        </td>
                        <td class="fields" style="width: 107px;">
                            <asp:Button ID="btnAddSponser" runat="server" Text="Add Sponsor" Width="111px" OnClick="btnAddSponser_Click" />
                        </td>
                        <td class="fields" style="width: 107px">
                            <asp:Button ID="btnDelSponser" runat="server" Text="Remove Sponsor" Width="118px"
                                OnClick="btnDelSponser_Click" />
                        </td>
                        <td class="fields" style="width: 63px;">
                        </td>
                        <td style="width: 24px;">
                        </td>
                        <td class="fields" style="width: 128px;">
                        </td>
                        <td class="fields" style="width: 92px;">
                        </td>
                        <td class="fields" style="width: 16px; text-align: left">
                        </td>
                        <td class="fields" style="width: 26px;">
                        </td>
                        <td class="fields" style="width: 22px;">
                        </td>
                        <td style="width: 58px;">
                        </td>
                        <td style="width: 186px;">
                        </td>
                        <td style="width: 100px;">
                        </td>
                    </tr>
                    <tr>
                        <td style="height: 25px">
                        </td>
                        <td class="fields" style="width: 107px; height: 25px">
                            <asp:Label ID="Label30" runat="server" Font-Bold="False" Text="Sponsor" Width="113px"></asp:Label>
                        </td>
                        <td class="fields" style="width: 63px; height: 25px">
                            <asp:TextBox ID="txtSpn1" runat="server" Enabled="true" Width="146px" TabIndex="24"
                                ReadOnly="True"></asp:TextBox>
                        </td>
                        <td style="width: 24px; height: 25px">
                            <asp:Image ID="ibtnSpn1" runat="server" Height="16px" ImageUrl="~/images/find_img.png"
                                Width="16px" ImageAlign="Left" />
                        </td>
                        <td class="fields" style="width: 128px; height: 25px">
                        </td>
                        <td class="fields" style="width: 92px; height: 25px">
                            <asp:Label ID="Label31" runat="server" Font-Bold="False" Text="Valid From" Width="114px"></asp:Label>
                        </td>
                        <td class="fields" style="width: 16px; height: 25px; text-align: left;">
                            <asp:TextBox ID="txtVFrom1" runat="server" Width="90px" onBlur="checkdate(this)"
                                TabIndex="25" MaxLength="10"></asp:TextBox>
                        </td>
                        <td class="fields" style="width: 26px; height: 25px">
                            <asp:Image ID="ibtnFDate1" runat="server" ImageUrl="~/images/cal.gif" />
                        </td>
                        <td class="fields" style="width: 22px; height: 25px">
                            <asp:Label ID="Label32" runat="server" Font-Bold="False" Text="To" Width="18px"></asp:Label>
                        </td>
                        <td style="width: 58px; height: 25px">
                            <asp:TextBox ID="txtspTo1" runat="server" Width="90px" onBlur="checkdate(this)" TabIndex="26"
                                MaxLength="10"></asp:TextBox>
                        </td>
                        <td style="width: 186px; height: 25px">
                            <asp:Image ID="ibtnTDate1" runat="server" ImageUrl="~/images/cal.gif" />
                        </td>
                        <td style="width: 100px; height: 25px">
                        </td>
                    </tr>
                    <tr>
                        <td class="fields" style="height: 16px">
                        </td>
                        <td class="fields" style="width: 107px; height: 16px; vertical-align: top;">
                        </td>
                        <td class="fields" style="width: 63px; height: 16px">
                            <asp:TextBox ID="txtSpCode" runat="server" Visible="False"></asp:TextBox>
                        </td>
                        <td style="width: 24px; height: 16px">
                        </td>
                        <td class="fields" style="width: 128px; height: 16px">
                        </td>
                        <td class="fields" style="width: 92px; height: 16px">
                        </td>
                        <td style="width: 16px; height: 16px">
                        </td>
                        <td class="fields" style="width: 26px; height: 16px">
                        </td>
                        <td class="fields" style="width: 22px; height: 16px">
                        </td>
                        <td style="width: 58px; height: 16px">
                        </td>
                        <td style="width: 186px; height: 16px">
                        </td>
                        <td style="width: 100px; height: 16px">
                        </td>
                    </tr>
                    <tr>
                        <td class="fields" style="height: 16px">
                        </td>
                        <td class="fields" style="width: 107px; height: 16px; vertical-align: top;">
                            <asp:Label ID="Label1" runat="server" Font-Bold="False" Style="vertical-align: top"
                                Text="Fee Type" Width="113px"></asp:Label>
                        </td>
                        <td class="fields" style="width: 63px; height: 16px">
                            <asp:ListBox ID="lstStudentFeetype1" runat="server" Height="121px" Width="148px"
                                DataTextField="Description" DataValueField="FeeTypeCode"></asp:ListBox>
                        </td>
                        <td style="width: 24px; height: 16px; vertical-align: top;">
                            <asp:ImageButton ID="ibtn_spn1_feetype" runat="server" Height="16px" ImageUrl="~/images/find_img.png"
                                Width="16px" ImageAlign="Left" Visible="false" />
                        </td>
                        <td class="fields" style="width: 128px; height: 16px">
                        </td>
                        <td class="fields" style="width: 92px; height: 16px; vertical-align: top;">
                            <asp:Label ID="Label59" runat="server" Font-Bold="False" Height="17px" Text="Fully Sponsored"
                                Width="113px"></asp:Label>
                        </td>
                        <td style="width: 16px; height: 16px; vertical-align: top;">
                            <asp:CheckBox ID="chkSpnFeetypes1" runat="server" Height="22px" Width="85px" OnCheckedChanged="chkSpnFeetypes1_CheckedChanged"
                                AutoPostBack="True" />
                        </td>
                        <td class="fields" style="width: 26px; height: 16px">
                        </td>
                        <td class="fields" style="width: 22px; height: 16px">
                        </td>
                        <td style="width: 58px; height: 16px">
                        </td>
                        <td style="width: 186px; height: 16px">
                        </td>
                        <td style="width: 100px; height: 16px">
                        </td>
                    </tr>
                </table>
            </asp:View>
        </asp:MultiView>
    </asp:Panel>
    <asp:Panel ID="PnlView" runat="server" Visible="False" Width="100%">
        <table style="width: 100%;">
            <tr>
                <td style="width: 1px; height: 21px">
                </td>
                <td class="fields" style="width: 100%; height: 21px">
                    <asp:DataGrid ID="dgView" runat="server" AutoGenerateColumns="False" DataKeyField="SAUF_Code"
                        Width="100%">
                        <FooterStyle CssClass="dgFooterStyle" Height="20px" />
                        <SelectedItemStyle CssClass="dgSelectedItemStyle" />
                        <AlternatingItemStyle BackColor="Beige" CssClass="dgAlternatingItemStyle" Font-Bold="False"
                            Font-Italic="False" Font-Overline="False" Font-Strikeout="False" Font-Underline="False" />
                        <ItemStyle CssClass="dgItemStyle" />
                        <HeaderStyle BackColor="#CDD7EE" CssClass="dgHeaderStyle" Font-Bold="True" Font-Italic="False"
                            Font-Overline="False" Font-Size="Medium" Font-Strikeout="False" Font-Underline="False" />
                        <Columns>
                            <asp:ButtonColumn CommandName="Select" DataTextField="SAUF_Code" HeaderText="Fund Code"
                                Text="SAUF_Code"></asp:ButtonColumn>
                            <asp:BoundColumn DataField="SAUF_Desc" HeaderText="Fund Name"></asp:BoundColumn>
                            <asp:BoundColumn DataField="SAUF_GLCode" HeaderText="GLAccount"></asp:BoundColumn>
                        </Columns>
                    </asp:DataGrid>
                </td>
                <td style="width: 100px; height: 21px">
                </td>
            </tr>
        </table>
    </asp:Panel>
   <asp:Button ID="btnHidden" runat="Server" OnClick="btnHidden_Click"  style="display:none" />
</asp:Content>
