<%@ Page Language="VB" MasterPageFile="~/MasterPage3.master" AutoEventWireup="false"
    CodeFile="FeePosting.aspx.vb" Inherits="FeePosting" Title="Automated Fee Charging" MaintainScrollPositionOnPostback="true" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <script language="javascript" src="Scripts/popcalendar.js" type="text/javascript"></script>
    <script language="javascript" src="Scripts/functions.js" type="text/javascript"></script>
    <script language="javascript" type="text/javascript">
        function chkdate(objName) {
            alert('lllll');
            var strDatestyle = "EU"; //United States date style
            //var strDatestyle = "EU";  //European date style
            var strDate;
            var strDateArray;
            var strDay;
            var strMonth;
            var strYear;
            var intday;
            var intMonth;
            var intYear;
            var booFound = false;
            var datefield = objName;
            var strSeparatorArray = new Array("-", " ", "/", ".");
            var intElementNr;
            var err = 0;
            var strMonthArray = new Array(12);
            strMonthArray[0] = "01";
            strMonthArray[1] = "02";
            strMonthArray[2] = "03";
            strMonthArray[3] = "04";
            strMonthArray[4] = "05";
            strMonthArray[5] = "06";
            strMonthArray[6] = "07";
            strMonthArray[7] = "08";
            strMonthArray[8] = "09";
            strMonthArray[9] = "10";
            strMonthArray[10] = "11";
            strMonthArray[11] = "12";
            strDate = datefield.value;
            if (strDate.length < 1) {
                return true;
            }
            for (intElementNr = 0; intElementNr < strSeparatorArray.length; intElementNr++) {
                if (strDate.indexOf(strSeparatorArray[intElementNr]) != -1) {
                    strDateArray = strDate.split(strSeparatorArray[intElementNr]);
                    if (strDateArray.length != 3) {
                        err = 1;
                        return false;
                    }
                    else {
                        strDay = strDateArray[0];
                        strMonth = strDateArray[1];
                        strYear = strDateArray[2];
                    }
                    booFound = true;
                }
            }
            if (booFound == false) {
                if (strDate.length > 5) {
                    strDay = strDate.substr(0, 2);
                    strMonth = strDate.substr(2, 2);
                    strYear = strDate.substr(4);
                }
            }
            if (strYear.length == 2) {
                strYear = '20' + strYear;
            }
            // US style
            if (strDatestyle == "US") {
                strTemp = strDay;
                strDay = strMonth;
                strMonth = strTemp;
            }
            intday = parseInt(strDay, 10);
            if (isNaN(intday)) {
                err = 2;
                return false;
            }
            intMonth = parseInt(strMonth, 10);
            if (isNaN(intMonth)) {
                for (i = 0; i < 12; i++) {
                    if (strMonth.toUpperCase() == strMonthArray[i].toUpperCase()) {
                        intMonth = i + 1;
                        strMonth = strMonthArray[i];
                        i = 12;
                    }
                }
                if (isNaN(intMonth)) {
                    err = 3;
                    return false;
                }
            }
            intYear = parseInt(strYear, 10);
            if (isNaN(intYear)) {
                err = 4;
                return false;
            }
            if (intMonth > 12 || intMonth < 1) {
                err = 5;
                return false;
            }
            if ((intMonth == 1 || intMonth == 3 || intMonth == 5 || intMonth == 7 || intMonth == 8 || intMonth == 10 || intMonth == 12) && (intday > 31 || intday < 1)) {
                err = 6;
                return false;
            }
            if ((intMonth == 4 || intMonth == 6 || intMonth == 9 || intMonth == 11) && (intday > 30 || intday < 1)) {
                err = 7;
                return false;
            }
            if (intMonth == 2) {
                if (intday < 1) {
                    err = 8;
                    return false;
                }
                if (LeapYear(intYear) == true) {
                    if (intday > 29) {
                        err = 9;
                        return false;
                    }
                }
                else {
                    if (intday > 28) {
                        err = 10;
                        return false;
                    }
                }
            }
            if (intday < 10) {
                intday = "0" + intday
            }
            if (strDatestyle == "US") {
                datefield.value = strMonthArray[intMonth - 1] + "/" + intday + "/" + strYear;
            }
            else {
                datefield.value = intday + "/" + strMonthArray[intMonth - 1] + "/" + strYear;
            }
            return true;
        }
        function LeapYear(intYear) {
            if (intYear % 100 == 0) {
                if (intYear % 400 == 0) { return true; }
            }
            else {
                if ((intYear % 4) == 0) { return true; }
            }
            return false;
        }

        function do_totals1() {
            document.all.pleasewaitScreen.style.visibility = "visible";
            window.setTimeout('do_totals2()', 1)
        }
        function WaitingProcess() {
            document.all.pleasewaitScreen.style.visibility = "visible";
            var Semester = document.getElementById("<%=ddlSemester.ClientID%>").value;
            var Program = document.getElementById("<%=ddlProgramInfo.ClientID%>").value;
            var Faculty = document.getElementById("<%=ddlFaculty.ClientID%>").value;
            PageMethods.GetData(CallSuccess, CallError);
            window.setTimeout('do_totals2()', 1)
        }
        function CallSuccess(res) {
            alert(res);
        }

        function CallError() {
            alert('Error');
        }

        function do_totals2() {
            calc_totals();
            document.all.pleasewaitScreen.style.visibility = "hidden";
        }

        function formatDate(date, format) {
            format = format + "";
            var result = "";
            var i_format = 0;
            var c = "";
            var token = "";
            var y = date.getYear() + "";
            var M = date.getMonth() + 1;
            var d = date.getDate();
            var E = date.getDay();
            var H = date.getHours();
            var m = date.getMinutes();
            var s = date.getSeconds();
            var yyyy, yy, MMM, MM, dd, hh, h, mm, ss, ampm, HH, H, KK, K, kk, k;
            // Convert real date parts into formatted versions
            var value = new Object();
            if (y.length < 4) { y = "" + (y - 0 + 1900); }
            value["y"] = "" + y;
            value["yyyy"] = y;
            value["yy"] = y.substring(2, 4);
            value["M"] = M;
            value["MM"] = LZ(M);
            value["MMM"] = MONTH_NAMES[M - 1];
            value["NNN"] = MONTH_NAMES[M + 11];
            value["d"] = d;
            value["dd"] = LZ(d);
            value["E"] = DAY_NAMES[E + 7];
            value["EE"] = DAY_NAMES[E];
            value["H"] = H;
            value["HH"] = LZ(H);
            if (H == 0) { value["h"] = 12; }
            else if (H > 12) { value["h"] = H - 12; }
            else { value["h"] = H; }
            value["hh"] = LZ(value["h"]);
            if (H > 11) { value["K"] = H - 12; } else { value["K"] = H; }
            value["k"] = H + 1;
            value["KK"] = LZ(value["K"]);
            value["kk"] = LZ(value["k"]);
            if (H > 11) { value["a"] = "PM"; }
            else { value["a"] = "AM"; }
            value["m"] = m;
            value["mm"] = LZ(m);
            value["s"] = s;
            value["ss"] = LZ(s);
            while (i_format < format.length) {
                c = format.charAt(i_format);
                token = "";
                while ((format.charAt(i_format) == c) && (i_format < format.length)) {
                    token += format.charAt(i_format++);
                }
                if (value[token] != null) { result = result + value[token]; }
                else { result = result + token; }
            }
            return result;
        }
        function compareDates(date1, dateformat1, date2, dateformat2) {
            var d1 = getDateFromFormat(date1, dateformat1);
            var d2 = getDateFromFormat(date2, dateformat2);
            if (d1 == 0 || d2 == 0) {
                return -1;
            }
            else if (d1 > d2) {
                return 1;
            }
            return 0;
        }
        function CheckInvDate() {
            var digits = "0123456789/";
            var temp;
            for (var i = 0; i < document.getElementById("<%=txtInvoiceDate.ClientID %>").value.length; i++) {
                temp = document.getElementById("<%=txtInvoiceDate.ClientID%>").value.substring(i, i + 1);
                if (digits.indexOf(temp) == -1) {
                    alert("Enter Valid Date (dd/mm/yyyy)");
                    document.getElementById("<%=txtInvoiceDate.ClientID%>").value = "";
                    document.getElementById("<%=txtInvoiceDate.ClientID%>").focus();
                    return false;
                }
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
        function CheckDueDate() {
            var digits = "0123456789/";
            var temp;
            for (var i = 0; i < document.getElementById("<%=txtDuedate.ClientID %>").value.length; i++) {
                temp = document.getElementById("<%=txtDuedate.ClientID%>").value.substring(i, i + 1);
                if (digits.indexOf(temp) == -1) {
                    alert("Enter Valid Date (dd/mm/yyyy)");
                    document.getElementById("<%=txtDuedate.ClientID%>").value = "";
                    document.getElementById("<%=txtDuedate.ClientID%>").focus();
                    return false;
                }
            }
            return true;
        }

        function getconfirm() {
            if (document.getElementById("<%=lblStatus.ClientID%>").value == "Posted") {
                alert("Posted Record Cannot be Deleted");
                return false;
            }

            if (confirm("Do you want to Delete Record?")) {
                return true;
            }
            else {
                return false;
            }
        }
        function getControlFields() {
            if (document.getElementById("<%=rbSemester.ClientID%>").checked == true) {
                document.getElementById("<%=ddlSemester.ClientID%>").enabled = true
            }
            else {
                document.getElementById("<%=ddlSemester.ClientID%>").enabled = false
            }

        }
        function OpenWindow1(URL) {
            var WindowName = "MyPopup";
            var Features = "location=no,toolbar=no,menubar=no,height =600,scrollbars=yes";
            window.open(URL, WindowName, Features);
        }
        function OpenWindow() {
            var batch = document.getElementById("<%=hvbatch.ClientID%>").value;

            new_window = window.open('AFCReportForm.aspx?bat=' + batch, 'Hanodale', 'width=520,height=500,resizable=1,srcollable =yes'); new_window.focus();
        }
        function getCallUrl() {
            new_window = window.open('http://172.17.2.74:46908/SAS', 'Integration', 'width=520,height=500,resizable=1,srcollable =yes'); new_window.focus();
        }
        function getcheck() {
            var digits = "0123456789.";
            var temp;
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
            var ans = validate()
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
        function validate() {
            var re = /\s*((\S+\s*)*)/;
            if (document.getElementById("<%=lblStatus.ClientID%>").value == "Posted") {
                alert("Posted Record Cannot be Edited.");
                return false;
            }

            var digits = "ABCDEFGHIJKLMNOPQRSTUVWXYZ abcdefghijklmnopqrstuvwxyz0123456789";
            var temp;
            for (var i = 0; i < document.getElementById("<%=txtBatchNo.ClientID %>").value.length; i++) {
                temp = document.getElementById("<%=txtBatchNo.ClientID%>").value.substring(i, i + 1);
                if (digits.indexOf(temp) == -1) {
                    alert("Please Enter Correct Code");
                    document.getElementById("<%=txtBatchNo.ClientID%>").focus();
                    return false;
                }
            }
            if (document.getElementById("<%=txtBatchDate.ClientID%>").value.replace(re, "$1").length == 0) {
                alert("Batch Date Field Cannot Be Blank");
                document.getElementById("<%=txtBatchDate.ClientID%>").focus();
                return false;
            }
            if (document.getElementById("<%=txtInvoiceDate.ClientID%>").value.replace(re, "$1").length == 0) {
                alert("Trnasaction Date Field Cannot Be Blank");
                document.getElementById("<%=txtInvoiceDate.ClientID%>").focus();
                return false;
            }
            if (document.getElementById("<%=txtDueDate.ClientID%>").value.replace(re, "$1").length == 0) {
                alert("Due Date Field Cannot Be Blank");
                document.getElementById("<%=txtDueDate.ClientID%>").focus();
                return false;
            }
            //txtBatchDate---------------------------------------------------------------------------
            var len = document.getElementById("<%=txtBatchDate.ClientID%>").value
            var RegExPattern = /^((((0?[1-9]|[12]\d|3[01])[\.\-\/](0?[13578]|1[02])[\.\-\/]((1[6-9]|[2-9]\d)?\d{2}))|((0?[1-9]|[12]\d|30)[\.\-\/](0?[13456789]|1[012])[\.\-\/]((1[6-9]|[2-9]\d)?\d{2}))|((0?[1-9]|1\d|2[0-8])[\.\-\/]0?2[\.\-\/]((1[6-9]|[2-9]\d)?\d{2}))|(29[\.\-\/]0?2[\.\-\/]((1[6-9]|[2-9]\d)?(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00)|00)))|(((0[1-9]|[12]\d|3[01])(0[13578]|1[02])((1[6-9]|[2-9]\d)?\d{2}))|((0[1-9]|[12]\d|30)(0[13456789]|1[012])((1[6-9]|[2-9]\d)?\d{2}))|((0[1-9]|1\d|2[0-8])02((1[6-9]|[2-9]\d)?\d{2}))|(2902((1[6-9]|[2-9]\d)?(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00)|00))))$/;
            var errorMessage = 'Enter Valid Date (dd/mm/yyyy)';

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
            //txtInvoiceDate---------------------------------------------------------------------------
            var len = document.getElementById("<%=txtInvoiceDate.ClientID%>").value
            var RegExPattern = /^((((0?[1-9]|[12]\d|3[01])[\.\-\/](0?[13578]|1[02])[\.\-\/]((1[6-9]|[2-9]\d)?\d{2}))|((0?[1-9]|[12]\d|30)[\.\-\/](0?[13456789]|1[012])[\.\-\/]((1[6-9]|[2-9]\d)?\d{2}))|((0?[1-9]|1\d|2[0-8])[\.\-\/]0?2[\.\-\/]((1[6-9]|[2-9]\d)?\d{2}))|(29[\.\-\/]0?2[\.\-\/]((1[6-9]|[2-9]\d)?(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00)|00)))|(((0[1-9]|[12]\d|3[01])(0[13578]|1[02])((1[6-9]|[2-9]\d)?\d{2}))|((0[1-9]|[12]\d|30)(0[13456789]|1[012])((1[6-9]|[2-9]\d)?\d{2}))|((0[1-9]|1\d|2[0-8])02((1[6-9]|[2-9]\d)?\d{2}))|(2902((1[6-9]|[2-9]\d)?(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00)|00))))$/;
            var errorMessage = 'Enter Valid Date (dd/mm/yyyy)';

            if (document.getElementById("<%=txtInvoiceDate.ClientID%>").value.match(RegExPattern)) {
                if (len.length == 8) {
                    alert(errorMessage);
                    document.getElementById("<%=txtInvoiceDate.ClientID%>").value = "";
                    document.getElementById("<%=txtInvoiceDate.ClientID%>").focus();
                    return false;
                }
            }
            else {
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


            //txtDueDate---------------------------------------------------------------------------
            var len = document.getElementById("<%=txtDuedate.ClientID%>").value
            var RegExPattern = /^((((0?[1-9]|[12]\d|3[01])[\.\-\/](0?[13578]|1[02])[\.\-\/]((1[6-9]|[2-9]\d)?\d{2}))|((0?[1-9]|[12]\d|30)[\.\-\/](0?[13456789]|1[012])[\.\-\/]((1[6-9]|[2-9]\d)?\d{2}))|((0?[1-9]|1\d|2[0-8])[\.\-\/]0?2[\.\-\/]((1[6-9]|[2-9]\d)?\d{2}))|(29[\.\-\/]0?2[\.\-\/]((1[6-9]|[2-9]\d)?(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00)|00)))|(((0[1-9]|[12]\d|3[01])(0[13578]|1[02])((1[6-9]|[2-9]\d)?\d{2}))|((0[1-9]|[12]\d|30)(0[13456789]|1[012])((1[6-9]|[2-9]\d)?\d{2}))|((0[1-9]|1\d|2[0-8])02((1[6-9]|[2-9]\d)?\d{2}))|(2902((1[6-9]|[2-9]\d)?(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00)|00))))$/;
            var errorMessage = 'Enter Valid Date (dd/mm/yyyy)';

            if (document.getElementById("<%=txtDuedate.ClientID%>").value.match(RegExPattern)) {
                if (len.length == 8) {
                    alert(errorMessage);
                    document.getElementById("<%=txtDuedate.ClientID%>").value = "";
                    document.getElementById("<%=txtDuedate.ClientID%>").focus();
                    return false;
                }
            }
            else {
                alert(errorMessage);
                document.getElementById("<%=txtDuedate.ClientID%>").value = "";
                document.getElementById("<%=txtDuedate.ClientID%>").focus();
                return false;
            }


            //Compare Dates----------------------------------------------
            var str1 = document.getElementById("<%=txtInvoiceDate.ClientID %>").value;
            var str2 = document.getElementById("<%=txtDuedate.ClientID %>").value;
            var dt1 = parseInt(str1.substring(0, 2), 10);
            var mon1 = parseInt(str1.substring(3, 5), 10);
            var yr1 = parseInt(str1.substring(6, 10), 10);
            var dt2 = parseInt(str2.substring(0, 2), 10);
            var mon2 = parseInt(str2.substring(3, 5), 10);
            var yr2 = parseInt(str2.substring(6, 10), 10);
            var date1 = new Date(yr1, mon1, dt1);
            var date2 = new Date(yr2, mon2, dt2);




            if (date2 < date1) {
                alert("Due Date Should be Greater than Transaction Date");
                document.getElementById("<%=txtDuedate.ClientID%>").value = "";
                document.getElementById("<%=txtDuedate.ClientID%>").focus();
                return false;
            }

            //Compare Dates----------------------------------------------
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
                return false;
            }
            return true;
        }



        function getibtnBDate() {
            popUpCalendar(document.getElementById("<%=ibtnBDate.ClientID%>"), document.getElementById("<%=txtBatchDate.ClientID%>"), 'dd/mm/yyyy')

        }
        function getDate1from() {
            popUpCalendar(document.getElementById("<%=ibtnInDate.ClientID%>"), document.getElementById("<%=txtInvoiceDate.ClientID%>"), 'dd/mm/yyyy')

        }
        function getDate2from() {
            popUpCalendar(document.getElementById("<%=ibtnDueDate.ClientID%>"), document.getElementById("<%=txtDueDate.ClientID%>"), 'dd/mm/yyyy')

        }

        function SelectAllCheckboxes(spanChk) {

            // Added as ASPX uses SPAN for checkbox
            var oItem = spanChk.children;
            var theBox = (spanChk.type == "checkbox") ?
                spanChk : spanChk.children.item[0];
            xState = theBox.checked;
            elm = theBox.form.elements;

            for (i = 0; i < elm.length; i++)
                if (elm[i].type == "checkbox" &&
              elm[i].id != theBox.id) {
                    //elm[i].click();
                    if (elm[i].checked != xState)
                        elm[i].click();
                    //elm[i].checked=xState;
                }
        }
        function SelectAll() {

            var frm = document.forms['aspnetForm'];

            for (var i = 0; i < document.forms[0].length; i++) {
                if (document.forms[0].elements[i].id.indexOf('chk') != -1) {

                    document.forms[0].elements[i].checked = true

                }
            }
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
                            <td style="width: 3%; height: 14px; margin-left: 40px;">
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
                    </table>
                </td>
                <td>
                    <table style="border-collapse: collapse;" class="menuoff" onmouseover="className='menuon';"
                        onmouseout="className='menuoff';">
                        <tr>
                            <td style="width: 3%; height: 14px">
                                <asp:ImageButton ID="ibtnPrint" runat="server" ImageUrl="~/images/print.png" ToolTip="Print"
                                    OnClick="ibtnPrint_Click1" />
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
                            <td style="width: 3%; height: 14px">&nbsp;<asp:Label ID="Label6" runat="server" Text="Posting"></asp:Label>
                            </td>
                        </tr>
                    </table>
                </td>
                <td>
                    <table style="border-collapse: collapse;" class="menuoff" onmouseover="className='menuon';"
                        onmouseout="className='menuoff';">
                        <tr>
                            <%--<td style="width: 3%; height: 14px">
                                <asp:ImageButton ID="ImageButton1" runat="server" ImageUrl="~/images/gothers.png"
                                    ToolTip="Cancel" OnClick="ibtnOthers_Click" />
                            </td>--%>
                            <%--<td style="width: 3%; height: 14px">
                                <asp:Label ID="Label5" runat="server" Text="Others"></asp:Label>
                            </td>--%>
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
                <td>
                    <table style="border-collapse: collapse;" class="menuoff" onmouseover="className='menuon';"
                        onmouseout="className='menuoff';">
                        <tr>
                            <td>
                                <asp:ImageButton ID="ibtnView" runat="server" ImageUrl="~/images/find.png" ToolTip="View" />
                            </td>
                            <td style="width: 3%; height: 14px">
                                <asp:Label ID="Label2" runat="server" Text="Search"></asp:Label>
                            </td>
                        </tr>
                    </table>
                </td>
                <td style="width: 2%; height: 14px"></td>
                <td style="width: 100%; height: 14px"></td>
                <td style="width: 100%; height: 14px"></td>
            </tr>
        </table>
        <table class="mainbg" style="width: 100%">
            <tr>
                <td class="vline" style="width: 98%; height: 1px"></td>
            </tr>
        </table>
        <table class="mainbg" style="width: 100%">
            <tr>
                <td style="width: 494px; height: 39px">
                    <asp:SiteMapPath ID="SiteMapPath1" runat="server">
                    </asp:SiteMapPath>
                </td>
                <td class="pagetext" style="height: 39px; text-align: right">
                    <asp:Label ID="lblMenuName" runat="server" Text="Automated Fee Charging" Width="496px" Height="33px"></asp:Label>
                </td>
            </tr>
        </table>
        <asp:Label ID="lblMsg" runat="server" CssClass="lblError" Style="text-align: center"
            Width="276px"></asp:Label>
    </asp:Panel>

    <asp:Panel ID="Panel2" runat="server" Height="100%" Width="100%">
        <table>
            <tr>
                <td style="width: 70%; height: 215px;">
                    <fieldset>
                        <legend><strong><span style="color: #000000"></span></strong></legend>
                        <table>
                            <tr>
                                <td style="width: 3px"></td>
                                <td style="width: 11%">
                                    <asp:HiddenField ID="hvbatch" runat="server" />
                                </td>
                                <td style="width: 387px; text-align: left;"></td>
                                <td>&nbsp;
                                </td>
                                <td></td>
                                <td colspan="1"></td>
                                <td style="width: 46px"></td>
                                <td style="width: 130px"></td>
                            </tr>
                            <tr>
                                <td style="height: 27px; width: 3px;">
                                    <span style="color: #ff0033">*</span>
                                </td>
                                <td style="width: 11%; height: 27px">
                                    <asp:Label ID="Label3" runat="server" Text="Faculty" Width="48px"></asp:Label>
                                </td>
                                <td style="width: 387px; height: 27px; text-align: left;">
                                    <asp:DropDownList ID="ddlFaculty" runat="server" AppendDataBoundItems="True" DataValueField=" "
                                        Height="20px" Width="302px" AutoPostBack="True">
                                        <asp:ListItem Value="-1">--Select--</asp:ListItem>
                                        <asp:ListItem Value="1">All</asp:ListItem>
                                    </asp:DropDownList>
                                    &nbsp;
                                </td>
                                <td rowspan="2">

                                </td>
                                <td rowspan="2">
                                    <asp:Label ID="Label1" runat="server" Style="text-align: left" Text="Batch Id" Width="60px" Visible="true"></asp:Label>
                                </td>
                                <td colspan="2" rowspan="2">
                                    <asp:TextBox ID="txtBatchNo" runat="server" MaxLength="20" Width="154px" Visible="true" AutoPostBack="true"></asp:TextBox>&nbsp;
                                </td>
                                <td rowspan="6" style="width: 149px">
                                    <asp:ImageButton ID="ibtnStatus" runat="server" Enabled="False" ImageUrl="~/images/NotReady.gif"
                                        CssClass="cursor" />
                                </td>
                                <td style="width: 151px; height: 27px"></td>
                            </tr>
                            <tr>
                                <td style="height: 13px; width: 3px;">&nbsp;
                                </td>
                                <td style="width: 11%; height: 13px">
                                    <asp:Label ID="lblProgram" runat="server" Text="Program" Width="48px"></asp:Label>
                                </td>
                                <td style="width: 387px; height: 13px">
                                    <asp:DropDownList ID="ddlProgramInfo" AppendDataBoundItems="true" runat="server"
                                        AutoPostBack="True" Width="300px">
                                    </asp:DropDownList>
                                </td>
                                <td colspan="1" style="width: 151px; height: 13px">&nbsp;
                                </td>
                            </tr>
                            <tr>
                                <td style="height: 13px; width: 3px;">
                                    <span style="color: #ff0000">*</span>
                                </td>
                                <td style="width: 11%; height: 13px">
                                    <asp:Label ID="Label4" runat="server" Text="Fee Posted For" Width="79px"></asp:Label>
                                </td>
                                <td style="width: 387px; height: 13px">
                                    <span style="color: #ff0000">
                                        <table>
                                            <tr>
                                                <td style="width: 124px; height: 22px; display: inline;">
                                                    <asp:RadioButton ID="rbSemester"
                                                        runat="server" AutoPostBack="true" ForeColor="Black" GroupName="PostedFor" Height="22px"
                                                        Text="Semester" Width="115px" />
                                                </td>
                                                <td style="width: 144px; height: 22px; text-align: left;">
                                                    <asp:RadioButton ID="rbAnnual" runat="server" AutoPostBack="true" ForeColor="Black"
                                                        GroupName="PostedFor" Style="margin-left: 0px" Text="Annual" Width="62px" />
                                                    &nbsp;
                                                </td>
                                            </tr>
                                        </table>
                                    </span>
                                </td>
                                <td style="height: 13px;">
                                    <span style="color: #ff0000">*</span>
                                </td>
                                <td colspan="1" style="height: 13px;">
                                    <asp:Label ID="Label22" runat="server" Style="text-align: left" Text="Batch Date"
                                        Width="60px"></asp:Label>
                                </td>
                                <td colspan="1" style="width: 10px; height: 13px">
                                    <asp:TextBox ID="txtBatchDate" runat="server" MaxLength="20" Style="text-align: left"
                                        Width="104px" ReadOnly="True" TabIndex="-1"></asp:TextBox>
                                    &nbsp;
                                </td>
                                <td colspan="1" style="width: 46px; height: 13px">
                                    <asp:Image ID="ibtnBDate" runat="server" ImageUrl="~/images/cal.gif" Visible="false" TabIndex="-1" />
                                </td>
                                <td colspan="1" style="width: 151px; height: 13px"></td>
                            </tr>
                            <tr>
                                <td style="height: 25px; width: 3px;">
                                    <span style="color: #ff0000">*</span>
                                </td>
                                <td style="width: 11%; height: 25px">
                                    <asp:Label ID="lblSemester" runat="server" Text="Semester" Width="67px"></asp:Label>
                                </td>
                                <td style="width: 387px; height: 25%; text-align: left;">
                                    <span style="color: #ff0000">
                                        <asp:DropDownList ID="ddlSemester" AppendDataBoundItems="true" runat="server" AutoPostBack="True">
                                        </asp:DropDownList>
                                    </span>&nbsp;
                                </td>
                                <td style="height: 25px;">
                                    <span style="color: #ff0000">*</span>
                                </td>
                                <td style="height: 25px;">
                                    <asp:Label ID="Label9" runat="server" Style="text-align: left" Text="Transaction Date"
                                        Width="97px"></asp:Label>
                                </td>
                                <td style="width: 10px; height: 25px">
                                    <span style="color: #ff0000">
                                        <asp:TextBox ID="txtInvoiceDate" runat="server" ReadOnly="true" MaxLength="20" Width="103px" TabIndex="-1"></asp:TextBox></span>
                                    &nbsp;
                                </td>
                                <td style="width: 46px; height: 25px">
                                    <asp:Image ID="ibtnInDate" runat="server" ImageUrl="~/images/cal.gif" Visible="false" TabIndex="-1" />
                                </td>
                                <td style="width: 151px; height: 25px"></td>
                            </tr>
                            <tr>
                                <td style="height: 25px; width: 3px;">
                                    <%-- <span style="color: #ff0000" >*</span>--%></td>
                                <td style="width: 11%; height: 25px">Status</td>
                                <td style="width: 387px; height: 25px; text-align: left;">&nbsp;
                                    <asp:DropDownList ID="ddlstatus" runat="server" AutoPostBack="True" Visible="true" Width="300px">
                                        <%--<asp:ListItem Value="-1">--Select--</asp:ListItem>--%>
                                        <asp:ListItem Value="-1">---Select---</asp:ListItem>
                                        <asp:ListItem Value="0">Held</asp:ListItem>
                                        <asp:ListItem Value="1">Ready</asp:ListItem>
                                        <asp:ListItem Value="2">Posted</asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                                <td style="height: 25px;">
                                    <span style="color: #ff0000">*</span>
                                </td>
                                <td style="height: 25px;">
                                    <asp:Label ID="Label7" runat="server" Style="text-align: left" Text="Due Date" Width="97px"></asp:Label>
                                </td>
                                <td style="width: 10px; height: 25px">
                                    <span style="color: #ff0000"></span><span style="color: #ff0000">
                                        <asp:TextBox ID="txtDueDate" runat="server" MaxLength="20" Width="103px"></asp:TextBox></span>
                                    &nbsp;
                                </td>
                                <td style="width: 46px; height: 25px">
                                    <asp:Image ID="ibtnDueDate" runat="server" ImageUrl="~/images/cal.gif" />
                                </td>
                                <td style="width: 151px; height: 25px"></td>
                            </tr>
                            <tr>
                                <td style="height: 25px; width: 3px;">&nbsp;</td>
                                <td style="width: 11%; height: 25px"></td>
                                <td text-align: left>&nbsp;</td>
                                <td style="height: 25px;">&nbsp;</td>
                                <td style="height: 25px;">&nbsp;</td>
                                <td style="width: 10px; height: 25px">&nbsp;</td>
                                <td style="width: 46px; height: 25px">&nbsp;</td>
                                <td style="width: 151px; height: 25px">&nbsp;</td>

                            </tr>
                        </table>
                        <asp:HiddenField ID="lblStatus" runat="server" />
                        &nbsp;&nbsp;
                        <asp:HiddenField ID="today" runat="server" />
                    </fieldset>
                </td>
            </tr>
        </table>
        <table style="width: 91%">
            <tr>
                <td colspan="4" style="vertical-align: top; height: 206px">
                    <asp:DataGrid ID="dgView" runat="server" AutoGenerateColumns="False"
                        Width="100%" AllowPaging="True">
                        <FooterStyle CssClass="dgFooterStyle" Height="20px" />
                        <PagerStyle HorizontalAlign="Center" Mode="NumericPages" />
                        <SelectedItemStyle CssClass="dgSelectedItemStyle" />
                        <AlternatingItemStyle BackColor="Beige" CssClass="dgAlternatingItemStyle" Font-Bold="False"
                            Font-Italic="False" Font-Overline="False" Font-Strikeout="False" Font-Underline="False" />
                        <ItemStyle CssClass="dgItemStyle" />
                        <HeaderStyle BackColor="#CDD7EE" CssClass="dgHeaderStyle" Font-Bold="True" Font-Italic="False"
                            Font-Overline="False" Font-Size="Medium" Font-Strikeout="False" Font-Underline="False" />
                        <Columns>
                            <asp:TemplateColumn HeaderText="Select">
                                <HeaderTemplate>
                                    <input id="chkall" type="checkbox" onclick="javascript: SelectAllCheckboxes(this);" />
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:CheckBox ID="chk" runat="server" AutoPostBack="false" />
                                </ItemTemplate>
                                <HeaderStyle Width="5%" />
                            </asp:TemplateColumn>
                            <asp:BoundColumn DataField="ProgramId" HeaderText="Program Code">
                                <HeaderStyle Width="12%" />
                            </asp:BoundColumn>
                            <asp:BoundColumn DataField="Program" HeaderText="Program Name"></asp:BoundColumn>
                            <asp:BoundColumn HeaderText="Post Status" Visible="False">
                                <HeaderStyle Width="10%" />
                            </asp:BoundColumn>
                            <asp:BoundColumn DataField="Faculty" HeaderText="Faculty" Visible="False"></asp:BoundColumn>
                            <asp:BoundColumn DataField="Semester" HeaderText="Semester" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center"></asp:BoundColumn>
                            <asp:BoundColumn DataField="BatchNo" HeaderText="Batch Number"></asp:BoundColumn>
                            <asp:BoundColumn DataField="TransStatus" HeaderText="Status" Visible="False"></asp:BoundColumn>
                            <asp:TemplateColumn HeaderText="View">
                                <HeaderStyle HorizontalAlign="Center" Width="12%" />
                                <ItemTemplate>
                                    <center>
                                        <asp:HyperLink ID="View" runat="server">View Student</asp:HyperLink>
                                    </center>
                                </ItemTemplate>
                            </asp:TemplateColumn>
                        </Columns>
                    </asp:DataGrid>&nbsp;
                </td>
            </tr>
            <tr>
                <td style="vertical-align: top; width: 100%; height: 30px; text-align: right">
                    <asp:Label ID="lblTotal" runat="server" Text="Total   " Width="142px" Visible="False"></asp:Label>&nbsp;
                </td>
                <td style="vertical-align: top; height: 30px" width="8%"></td>
                <td style="vertical-align: top; height: 30px" width="8%"></td>
                <td style="vertical-align: top; height: 30px; width: 12%;">
                    <asp:TextBox ID="txtAfterBalance" runat="server" Height="15px" MaxLength="20" ReadOnly="True"
                        Style="text-align: right" Width="98px" Visible="False">0.00</asp:TextBox>
                </td>
            </tr>
        </table>

        <br />
        &nbsp;<br />
        <asp:Button ID="btnCallUrl" runat="server" Text="Call Url" Visible="False" />
        <br />
        <br />
        <br />
        <br />
        <br />
        <br />
        <div id="pleasewaitScreen" style="z-index: 5; left: 40%; visibility: hidden; position: absolute; top: 30%">
            <table bordercolor="#5ba789" height="100" cellspacing="0" cellpadding="0" width="300" bgcolor="#5ba789" border="1">
                <tr>
                    <td valign="middle" align="center" width="100%" bgcolor="#e4f0db" height="100%">
                        <br>
                        <br />
                        <img src="Images/spinner.gif" align="middle">
                        <font face="Lucida Grande, Verdana, Arial, sans-serif" color="#000066" size="2">
                            <b>Processing. Please wait...</b></font>
                        <br />
                        <br />
                    </td>
                </tr>
            </table>
        </div>
    </asp:Panel>
    <br />
    <br />
    <br />
    <br />
    <br />
    <br />
</asp:Content>
