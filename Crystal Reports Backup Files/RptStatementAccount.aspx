<%@ Page Language="VB" MasterPageFile="~/MasterPage3.master" AutoEventWireup="false"
    CodeFile="RptStatementAccount.aspx.vb" Inherits="RptStatementAccount" Title="Statement of Account Report" %>

<asp:content ID="Content2" contentplaceholderid="head" runat="server">
    <script src="../Scripts/functions.js" type="text/javascript"></script>
    <script src="../Scripts/popcalendar.js" type="text/javascript"></script>
    <link href="../style.css" rel="stylesheet" />
</asp:content>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <script language="javascript" type="text/javascript">


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
        function getibtnFDate() {
            popUpCalendar(document.getElementById("<%=ibtnFdate.ClientID%>"), document.getElementById("<%=txtFrom.ClientID%>"), 'dd/mm/yyyy')

        }
        function getDateto() {
            popUpCalendar(document.getElementById("<%=ibtnTodate.ClientID%>"), document.getElementById("<%=txtToDate.ClientID%>"), 'dd/mm/yyyy')

        }
        function dateString() {
            var date, text = " "
            date = new Date()

            text += date.getDate() + "/"
            text += (date.getMonth() + 1) + "/"
            if (navigator.appName == "Netscape") {
                text += date.getYear() + 1900
            }
            if (navigator.appName == "Microsoft Internet Explorer") {
                text += date.getYear()
            }
            return (text)
        }

        function getDate() {
            var fromdate1 = "0"
            var todate1 = "0"
            var dunno;
            dunno = document.getElementById("<%=ddlDunningNo.ClientID %>").value


            var str1 = document.getElementById("<%=txtFrom.ClientID %>").value; //
            var str2 = document.getElementById("<%=txtTodate.ClientID %>").value; //
            var dt1 = parseInt(str1.substring(0, 2), 10); //
            var mon1 = parseInt(str1.substring(3, 5), 10); //
            var yr1 = parseInt(str1.substring(6, 10), 10); //
            var dt2 = parseInt(str2.substring(0, 2), 10); //
            var mon2 = parseInt(str2.substring(3, 5), 10); //
            var yr2 = parseInt(str2.substring(6, 10), 10); //
            var date1 = new Date(yr1, mon1, dt1); //
            var date2 = new Date(yr2, mon2, dt2); //

            if ((document.getElementById("<%=ChkBoxStatementAc.ClientID%>").checked == true) && (document.getElementById("<%=ChkBoxAcInfo.ClientID%>").checked == true) && (document.getElementById("<%=ChkBoxDunningLetter.ClientID%>").checked == true)) {
                alert("Do not Checked Three Checkbox")
                return;
            }


            else if ((document.getElementById("<%=ChkBoxStatementAc.ClientID%>").checked == true) && (document.getElementById("<%=ChkBoxAcInfo.ClientID%>").checked == true)) {
                if ((document.getElementById("<%=ddlStuStatus.ClientID%>").value == "-1") && (document.getElementById("<%=ddlFaculty.ClientID%>").value == "-1") && (document.getElementById("<%=ddlProgram.ClientID%>").value == "-1") && (document.getElementById("<%=ddlSponsor.ClientID%>").value == "-1") && (document.getElementById("<%=ChkDateRange.ClientID%>").checked == false)) {
                    if (confirm("Do you Want to Show All Records?")) {
                        new_window = window.open('../GroupReport/RptStatementAccountLetterViewer.aspx?dunno1=' + dunno, 'SAS', 'width=700,height=500,resizable=1'); new_window.focus();
                    }
                    else {
                        return false
                    }
                }
                else {
                    if (document.getElementById("<%=ChkDateRange.ClientID%>").checked) {
                        if (document.getElementById("<%=txtFrom.ClientID %>").value.length < 10) {
                            alert("Enter Valid Date (dd/mm/yyyy)")
                            document.getElementById("<%=txtFrom.ClientID %>").value = ""
                            document.getElementById("<%=txtFrom.ClientID%>").focus()
                            return false
                        }
                        if (document.getElementById("<%=txtToDate.ClientID %>").value.length < 10) {
                            alert("Enter Valid Date (dd/mm/yyyy)")
                            document.getElementById("<%=txtToDate.ClientID %>").value = ""
                            document.getElementById("<%=txtToDate.ClientID%>").focus()
                            return false
                        }

                        if (document.getElementById("<%=txtFrom.ClientID%>").value.length == 0 || document.getElementById("<%=txtToDate.ClientID%>").value.length == 0) {
                            alert("Date Field Cannot Be Blank")
                            return false;
                        }
                        if (date2 < date1) //
                        { //
                            alert("From Date Cannot Be Greater Than To Date"); //
                            return false; //
                        }   //
                        fromdate1 = document.getElementById("<%=txtFrom.ClientID%>").value
                        todate1 = document.getElementById("<%=txtTodate.ClientID%>").value
                        new_window = window.open('../GroupReport/RptStatementAccountLetterViewer.aspx?fdate=' + fromdate1 + '&tdate=' + todate1 + '&dunno1=' + dunno, 'SAS', 'width=700,height=500,resizable=1'); new_window.focus();
                    }
                    else {
                        new_window = window.open('../GroupReport/RptStatementAccountLetterViewer.aspx?dunno1=' + dunno, 'SAS', 'width=700,height=500,resizable=1'); new_window.focus();
                        return true
                    }
                }
            }




            else if ((document.getElementById("<%=ChkBoxStatementAc.ClientID%>").checked) && (document.getElementById("<%=ChkBoxDunningLetter.ClientID%>").checked == true)) {
                alert("Do not select StatementAccount and Dunning checkbox")
                return;
            }
            else if ((document.getElementById("<%=ChkBoxAcInfo.ClientID%>").checked) && (document.getElementById("<%=ChkBoxDunningLetter.ClientID%>").checked == true)) {
                alert("Do not select Info and Dunning checkbox")
                return;
            }
            else if (document.getElementById("<%=ChkBoxAcInfo.ClientID%>").checked == true) {
                alert("select with SatementAC")
            }
            else if (document.getElementById("<%=ChkBoxStatementAc.ClientID%>").checked == true) {
                if ((document.getElementById("<%=ddlStuStatus.ClientID%>").value == "-1") && (document.getElementById("<%=ddlFaculty.ClientID%>").value == "-1") && (document.getElementById("<%=ddlProgram.ClientID%>").value == "-1") && (document.getElementById("<%=ddlSponsor.ClientID%>").value == "-1") && (document.getElementById("<%=ChkDateRange.ClientID%>").checked == false)) {
                    if (confirm("Do you Want to Show All Records?")) {
                        new_window = window.open('../GroupReport/RptStatementAccountViewer.aspx', 'SAS', 'width=700,height=500,resizable=1'); new_window.focus();
                    }
                    else {
                        return false
                    }
                }
                else {
                    if (document.getElementById("<%=ChkDateRange.ClientID%>").checked == true) {
                        if (document.getElementById("<%=txtFrom.ClientID %>").value.length < 10) {
                            alert("Enter Valid Date (dd/mm/yyyy)")
                            document.getElementById("<%=txtFrom.ClientID %>").value = ""
                            document.getElementById("<%=txtFrom.ClientID%>").focus()
                            return false
                        }
                        if (document.getElementById("<%=txtToDate.ClientID %>").value.length < 10) {
                            alert("Enter Valid Date (dd/mm/yyyy)")
                            document.getElementById("<%=txtToDate.ClientID %>").value = ""
                            document.getElementById("<%=txtToDate.ClientID%>").focus()
                            return false
                        }


                        if (document.getElementById("<%=txtFrom.ClientID%>").value.length == 0 || document.getElementById("<%=txtToDate.ClientID%>").value.length == 0) {
                            alert("Date Field Cannot Be Blank")
                            return false;
                        }
                        if (date2 < date1) //
                        { //
                            alert("From Date Cannot Be Greater Than To Date"); //
                            return false; //
                        }   // 

                        fromdate1 = document.getElementById("<%=txtFrom.ClientID%>").value
                        todate1 = document.getElementById("<%=txtTodate.ClientID%>").value
                        new_window = window.open('../GroupReport/RptStatementAccountViewer.aspx?fdate=' + fromdate1 + '&tdate=' + todate1, 'SAS', 'width=700,height=500,resizable=1'); new_window.focus();
                    }
                    else {
                        new_window = window.open('../GroupReport/RptStatementAccountViewer.aspx', 'SAS', 'width=700,height=500,resizable=1'); new_window.focus();
                    }

                }
            }
            else if (document.getElementById("<%=ChkBoxDunningLetter.ClientID%>").checked == true) {

                //  Dunnining Letter

                if ((document.getElementById("<%=ddlStuStatus.ClientID%>").value == "-1") && (document.getElementById("<%=ddlFaculty.ClientID%>").value == "-1") && (document.getElementById("<%=ddlProgram.ClientID%>").value == "-1") && (document.getElementById("<%=ddlSponsor.ClientID%>").value == "-1") && (document.getElementById("<%=ChkDateRange.ClientID%>").checked == false)) {
                    if (confirm("Do you Want to Show All Records?")) {
                        new_window = window.open('RptDunningLetterWarningViewer.aspx?dunno1=' + dunno, 'SAS', 'width=700,height=500,resizable=1'); new_window.focus();
                    }
                    else {
                        return false
                    }
                }
                else {
                    if (document.getElementById("<%=ChkDateRange.ClientID%>").checked) {
                        if (document.getElementById("<%=txtFrom.ClientID %>").value.length < 10) {
                            alert("Enter Valid Date (dd/mm/yyyy)")
                            document.getElementById("<%=txtFrom.ClientID %>").value = ""
                            document.getElementById("<%=txtFrom.ClientID%>").focus()
                            return false
                        }
                        if (document.getElementById("<%=txtToDate.ClientID %>").value.length < 10) {
                            alert("Enter Valid Date (dd/mm/yyyy)")
                            document.getElementById("<%=txtToDate.ClientID %>").value = ""
                            document.getElementById("<%=txtToDate.ClientID%>").focus()
                            return false
                        }

                        if (document.getElementById("<%=txtFrom.ClientID%>").value.length == 0 || document.getElementById("<%=txtToDate.ClientID%>").value.length == 0) {
                            alert("Date Field Cannot Be Blank")
                            return false;
                        }
                        if (date2 < date1) {
                            alert("From Date Cannot Be Greater Than To Date");
                            return false;
                        }
                        fromdate1 = document.getElementById("<%=txtFrom.ClientID%>").value
                        todate1 = document.getElementById("<%=txtTodate.ClientID%>").value
                        new_window = window.open('RptDunninglattersCRFormFinal.aspx?fdate=' + fromdate1 + '&tdate=' + todate1 + '&dunno1=' + dunno, 'SAS', 'width=700,height=500,resizable=1'); new_window.focus();
                    }
                    else {
                        //new_window= window.open('RptDunninglattersCRFormFinal.aspx?dunno1='+dunno,'SAS','width=700,height=500,resizable=1');  new_window.focus();
                        new_window = window.open('RptDunningLetterWarningViewer.aspx?dunno1=' + dunno, 'SAS', 'width=700,height=500,resizable=1'); new_window.focus();
                        return true
                    }
                }
            }
            else {
                alert("Select Account Statement or Dunning Letter")
            }

        }

        function datevalide() {
            var fromdate1 = "0"
            var todate1 = "0"
            if ((document.getElementById("<%=ddlStuStatus.ClientID%>").value == "-1") && (document.getElementById("<%=ddlFaculty.ClientID%>").value == "-1") && (document.getElementById("<%=ddlProgram.ClientID%>").value == "-1") && (document.getElementById("<%=ddlSponsor.ClientID%>").value == "-1") && (document.getElementById("<%=ChkDateRange.ClientID%>").checked == false)) {
                if (confirm("Do you Want to Show All Records?")) {
                    if (document.getElementById("<%=ChkDateRange.ClientID%>").checked) {
                        fromdate1 = document.getElementById("<%=txtFrom.ClientID%>").value
                        todate1 = document.getElementById("<%=txtTodate.ClientID%>").value
                        return true
                    }
                    return false
                }

            }
        }

        function getDate1() {
            var curenttempdate = dateString()

            var fromdate = new Date(document.getElementById("<%=txtFrom.ClientID%>").value)
            var todate = new Date(document.getElementById("<%=txtTodate.ClientID%>").value)
            var currentdate = new Date(curenttempdate)

            if (document.getElementById("<%=ChkDateRange.ClientID%>").checked) {
                if (document.getElementById("<%=txtFrom.ClientID%>").value.length == 0 || document.getElementById("<%=txtToDate.ClientID%>").value.length == 0) {
                    alert("Date Field Cannot Be Blank")
                    return false;
                }
            }

            //Checking correct date format
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




            //Checking date between from and to
            var str1 = document.getElementById("<%=txtFrom.ClientID %>").value;
            var str2 = document.getElementById("<%=txtTodate.ClientID %>").value;
            var dt1 = parseInt(str1.substring(0, 2), 10);
            var mon1 = parseInt(str1.substring(3, 5), 10);
            var yr1 = parseInt(str1.substring(6, 10), 10);
            var dt2 = parseInt(str2.substring(0, 2), 10);
            var mon2 = parseInt(str2.substring(3, 5), 10);
            var yr2 = parseInt(str2.substring(6, 10), 10);
            var date1 = new Date(yr1, mon1, dt1);
            var date2 = new Date(yr2, mon2, dt2);


            if (date2 < date1) {
                alert("From Date Cannot Be Greater Than To Date");
                return false;
            }

            else {
                dllValues()
            }
        }

        function dllValues() {
            var fromdate1 = "0"
            var todate1 = "0"
            var lbl = false
            if ((document.getElementById("<%=ddlStuStatus.ClientID%>").value == "-1") && (document.getElementById("<%=ddlFaculty.ClientID%>").value == "-1") && (document.getElementById("<%=ddlProgram.ClientID%>").value == "-1") && (document.getElementById("<%=ddlSponsor.ClientID%>").value == "-1") && (document.getElementById("<%=ChkDateRange.ClientID%>").checked == false)) {
                if (confirm("Do you Want to Show All Records?")) {
                    if (document.getElementById("<%=ChkDateRange.ClientID%>").checked)   //test
                    {
                        fromdate1 = document.getElementById("<%=txtFrom.ClientID%>").value   //test
                        todate1 = document.getElementById("<%=txtTodate.ClientID%>").value   //test 
                    }            //test
                    lbl = true;
                    new_window = window.open('../GroupReport/RptStatementAccountViewer.aspx?fdate=' + fromdate1 + '&tdate=' + todate1, 'SAS', 'width=700,height=500,resizable=1'); new_window.focus();
                    return lbl;
                }
                else {
                    if (document.getElementById("<%=ddlStuStatus.ClientID%>").value == "-1") {
                        document.getElementById("<%=ddlStuStatus.ClientID%>").focus();
                        return lbl;
                    }
                }
            }
            else {
                if (document.getElementById("<%=ChkDateRange.ClientID%>").checked)   //test
                {                                                            //test
                    fromdate1 = document.getElementById("<%=txtFrom.ClientID%>").value   //test
                    todate1 = document.getElementById("<%=txtTodate.ClientID%>").value   //test 
                }            //test
                new_window = window.open('../GroupReport/RptStatementAccountViewer.aspx?fdate=' + fromdate1 + '&tdate=' + todate1, 'SAS', 'width=700,height=500,resizable=1'); new_window.focus();
                return true;
            }
        }

        function dllValues1() {
            var lbl = false
            if ((document.getElementById("<%=ddlStuStatus.ClientID%>").value == "-1") && (document.getElementById("<%=ddlFaculty.ClientID%>").value == "-1") && (document.getElementById("<%=ddlProgram.ClientID%>").value == "-1") && (document.getElementById("<%=ddlSponsor.ClientID%>").value == "-1")) {
                if (confirm("Do you Want to Show All Records?")) {
                    lbl = true;
                    new_window = window.open('../GroupReport/RptTransactionDetailViewer.aspx', 'SAS', 'width=700,height=500,resizable=1'); new_window.focus();
                    return lbl;
                }
                else {
                    if (document.getElementById("<%=ddlStuStatus.ClientID%>").value == "-1") {
                        document.getElementById("<%=ddlStuStatus.ClientID%>").focus();
                        return lbl;
                    }
                }
            }
            else {
                new_window = window.open('../GroupReport/RptTransactionDetailViewer.aspx', 'SAS', 'width=700,height=500,resizable=1'); new_window.focus();
                return true;
            }
        }

    </script>

    <asp:Panel ID="Panel1" runat="server" Width="100%">
        <table style="background-image: url(../images/Sample.png);">
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
                                <asp:ImageButton ID="ibtnView" runat="server" ImageUrl="~/images/find.png" ToolTip="View" />
                            </td>
                            <td style="width: 3%; height: 14px">
                                <asp:Label ID="Label16" runat="server" Text="Search"></asp:Label>
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
                                    ToolTip="Cancel" />
                            </td>
                            <td style="width: 3%; height: 14px">
                                &nbsp;<asp:Label ID="Label6" runat="server" Text="Posting"></asp:Label>
                            </td>
                        </tr>
                    </table>
                </td>
                <td>
                    <table style="border-collapse: collapse;" class="menuoff" onmouseover="className='menuon';"
                        onmouseout="className='menuoff';">
                        <tr>
                            <td style="width: 3%; height: 14px">
                                <asp:ImageButton ID="ibtnOthers" runat="server" ImageUrl="~/images/gothers.png" ToolTip="Cancel" />
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
                                <asp:ImageButton ID="ibtnCancel" runat="server" ImageUrl="~/images/cancel.png" ToolTip="Cancel"
                                    OnClick="ibtnCancel_Click" />
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
                    <asp:SiteMapPath ID="SiteMapPath1" runat="server" Width="273px">
                    </asp:SiteMapPath>
                </td>
                <td class="pagetext" style="height: 39px; width: 217px;">
                    <asp:Label ID="lblMenuName" runat="server" Text="University Fund" Width="470px"></asp:Label>
                </td>
            </tr>
        </table>
    </asp:Panel>

    <table style="width: 100%">
        <tr>
            <td style="width: 1px; height: 16px">
            </td>
            <td style="width: 1005px; height: 16px">
                &nbsp;<fieldset style="width: 100%;border: thin solid #A6D9F4;">
                    <legend><strong><span style="color: #000000">Selection Criteria</span></strong></legend>
                    <table style="width: 100%">
                        <tr>
                            <td style="height: 16px">
                                <asp:Label ID="Label3" runat="server" Text="Status"></asp:Label>
                            </td>
                            <td style="height: 16px; width: 34px;">
                                &nbsp;
                            </td>
                            <td style="width: 137px">
                                <asp:DropDownList ID="ddlStuStatus" runat="server" Width="135px" OnSelectedIndexChanged="ddlStuStatus_SelectedIndexChanged"
                                    AutoPostBack="True">
                                    <asp:ListItem Value="-1">-- Select --</asp:ListItem>
                                    <asp:ListItem Value="1">Active</asp:ListItem>
                                    <asp:ListItem Value="0">Inactive</asp:ListItem>
                                </asp:DropDownList>
                            </td>
                            <td style="height: 16px">
                            </td>
                            <td style="width: 10px; height: 16px">
                            </td>
                        </tr>
                        <tr>
                            <td style="height: 16px">
                                <asp:Label ID="Label9" runat="server" Text="By Faculty"></asp:Label>
                            </td>
                            <td style="height: 16px; width: 34px;">
                                &nbsp;
                            </td>
                            <td colspan="3">
                                <asp:DropDownList ID="ddlFaculty" runat="server" Width="135px" onmousedown="this.style.width=''"
                                    AppendDataBoundItems="True" OnSelectedIndexChanged="ddlFaculty_SelectedIndexChanged"
                                    AutoPostBack="True">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td style="height: 16px">
                                <asp:Label ID="Label10" runat="server" Text="By Program"></asp:Label>
                            </td>
                            <td style="height: 16px; width: 34px;">
                                &nbsp;
                            </td>
                            <td colspan="3">
                                <asp:DropDownList ID="ddlProgram" runat="server" Width="135px" onmousedown="this.style.width=''"
                                    AppendDataBoundItems="True" OnSelectedIndexChanged="ddlProgram_SelectedIndexChanged"
                                    AutoPostBack="True">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td style="height: 16px">
                                <asp:Label ID="Label4" runat="server" Text="Sponsor" Style="position: static"></asp:Label>
                            </td>
                            <td style="height: 16px; width: 34px;">
                                &nbsp;
                            </td>
                            <td colspan="3">
                                <asp:DropDownList ID="ddlSponsor" runat="server" Width="135px" onmousedown="this.style.width=''"
                                    Style="position: static" AppendDataBoundItems="True" OnSelectedIndexChanged="ddlSponsor_SelectedIndexChanged"
                                    AutoPostBack="True">
                                    <asp:ListItem Value="0">By Sponsor</asp:ListItem>
                                    <asp:ListItem Value="-1">-- Select --</asp:ListItem>
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 129px; height: 16px">
                                <asp:CheckBox ID="ChkDateRange" runat="server" Text="Date Range" Width="90px" OnCheckedChanged="ChkDateRange_CheckedChanged"
                                    AutoPostBack="True" />
                            </td>
                            <td style="width: 34px; height: 16px">
                                <asp:Label ID="Label1" runat="server" Text="From"></asp:Label>
                            </td>
                            <td style="width: 137px; height: 16px; text-align: left">
                                <asp:TextBox ID="txtFrom" runat="server"></asp:TextBox>
                            </td>
                            <td style="width: 10px; height: 16px">
                                <asp:Image ID="ibtnFDate" runat="server" ImageUrl="~/images/cal.gif" />
                            </td>
                            <td style="width: 34px; height: 16px">
                                <asp:Label ID="Label2" runat="server" Text="To"></asp:Label>
                            </td>
                            <td style="width: 137px; height: 16px; text-align: left">
                                <asp:TextBox ID="txtTodate" runat="server"></asp:TextBox>
                            </td>
                            <td style="width: 10px; height: 16px">   
                                <asp:Image ID="ibtnTodate" runat="server" ImageUrl="~/images/cal.gif" />
                            </td>
                            <td style="width: 10px; height: 16px">
                            </td>
                            <td style="width: 9px; height: 16px; text-align: left">
                                &nbsp;
                            </td>
                            <td style="width: 63px; height: 16px">
                            </td>
                            <td style="width: 648px; height: 16px">
                            </td>
                            <td style="width: 1px; height: 16px">
                            </td>
                            <td style="width: 1px; height: 16px">
                            </td>
                            <td style="width: 1px; height: 16px">
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 129px; height: 15px">
                                <asp:CheckBox ID="ChkBoxStatementAc" runat="server" Text="Include Accounts Statement"
                                    Width="180px" AutoPostBack="True" OnCheckedChanged="ChkBoxStatementAc_CheckedChanged" />
                            </td>
                            <td style="width: 34px; height: 15px">
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 129px; height: 16px">
                                <asp:CheckBox ID="ChkBoxAcInfo" runat="server" Text="Include Accounting Info" Width="171px"
                                    AutoPostBack="True" OnCheckedChanged="ChkBoxAcInfo_CheckedChanged" />
                            </td>
                            <td style="width: 34px; height: 16px">
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 129px; height: 16px">
                                <asp:CheckBox ID="ChkBoxDunningLetter" runat="server" Text="Dunning Letter" Width="172px"
                                    AutoPostBack="True" OnCheckedChanged="ChkBoxDunningLetter_CheckedChanged" />
                            </td>
                            <td style="width: 34px; height: 16px">
                                &nbsp;
                            </td>
                            <td style="width: 137px; height: 16px; text-align: right">
                                <asp:Label ID="Label7" runat="server" Text="Dunning Letter Code" Width="101px"></asp:Label>
                            </td>
                            <td style="height: 16px">
                                <asp:DropDownList ID="ddlDunningNo" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlDunningNo_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                        </tr>
                    </table>
                </fieldset>
            </td>
            <td style="width: 113px; height: 16px">
            </td>
        </tr>
        <tr>
            <td style="height: 16px">
            </td>
            <td style="width: 1005px; height: 16px">
                &nbsp;<fieldset style="width: 100%;border: thin solid #A6D9F4;">
                    <legend><strong><span style="color: #000000">Record Sort By</span></strong></legend>
                    <table style="width: 100%">
                        <tr>
                            <td style="width: 224px; height: 16px">
                                &nbsp;<asp:RadioButton ID="rdbtnstudentid" runat="server" Text="Matric Id" GroupName="RecordSort"
                                    AutoPostBack="True" OnCheckedChanged="rdbtnstudentid_CheckedChanged" />
                            </td>
                            <td style="width: 458px; height: 16px">
                            </td>
                            <td style="width: 180px; height: 16px">
                            </td>
                            <td style="width: 10px; height: 16px">
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 224px; height: 16px">
                                &nbsp;<asp:RadioButton ID="rdbtnstudentname" runat="server" Text="Student Name" Width="123px"
                                    GroupName="RecordSort" AutoPostBack="True" OnCheckedChanged="rdbtnstudentname_CheckedChanged" />
                            </td>
                            <td style="width: 458px; height: 16px">
                            </td>
                            <td style="width: 180px; height: 16px">
                            </td>
                            <td style="width: 10px; height: 16px">
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 224px; height: 16px">
                                &nbsp;<asp:RadioButton ID="rdbtnfaculty" runat="server" Text="Faculty" GroupName="RecordSort"
                                    AutoPostBack="True" OnCheckedChanged="rdbtnfaculty_CheckedChanged" />
                            </td>
                            <td style="width: 458px; height: 16px">
                            </td>
                            <td style="width: 180px; height: 16px">
                            </td>
                            <td style="width: 10px; height: 16px">
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 224px; height: 16px">
                                &nbsp;<asp:RadioButton ID="rdbtnpostcod" runat="server" Text="Post Code" GroupName="RecordSort"
                                    AutoPostBack="True" OnCheckedChanged="rdbtnpostcod_CheckedChanged" />
                            </td>
                            <td style="width: 458px; height: 16px">
                            </td>
                            <td style="width: 180px; height: 16px">
                            </td>
                            <td style="width: 10px; height: 16px">
                            </td>
                        </tr>
                    </table>
                </fieldset>
            </td>
            <td style="height: 16px;">
            </td>
        </tr>
    </table>

</asp:Content>
