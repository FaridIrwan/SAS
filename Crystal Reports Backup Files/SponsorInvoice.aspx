<%@ Page Language="VB" MasterPageFile="~/MasterPage3.master" AutoEventWireup="false" MaintainScrollPositionOnPostback="true"
    CodeFile="SponsorInvoice.aspx.vb" Inherits="SponsorInvoice" Title="Sponsor Invoice" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <script language="javascript" src="Scripts/popcalendar.js" type="text/javascript"></script>
    <script language="javascript" src="Scripts/functions.js" type="text/javascript"></script>
    <script language="javascript" type="text/javascript">
        function sample(ProgramId, Semester) {
            debugger;
            window.open("FeeStructure.aspx?Menuid=16&Formid=FS&IsStudentLedger=1&ProgramId=" + ProgramId + "&Semester=" + Semester + "'","Sample","width=520,height=600,resizable=0");
        }
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
                datefield.value = strMonthArray[intMonth - 1] + "-" + intday + "-" + strYear;
            }
            else {
                datefield.value = intday + "-" + strMonthArray[intMonth - 1] + "-" + strYear;
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

        function checkValue() {

            if (((event.keyCode < 48) || (event.keyCode > 57)) && (event.keyCode != 13) && (event.keyCode != 46)) {
                alert("Enter Valid Amount");
                event.keyCode = 0;
            }
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

        function getconfirm() {
            if (document.getElementById("<%=lblStatus.ClientID%>").value == "Posted") {
                alert("Posted Record Cannot be Deleted");
                return false;
            }
            if (document.getElementById("<%=lblStatus.ClientID%>").value == "New") {
                alert("Select a Record to Delete");
                return false;
            }

            if (document.getElementById("<%=txtBatchNo.ClientID%>").value == "") {
                alert("Select a Record");
                return false;
            }
            else {
                if (confirm("Do You Want to Delete Record?")) {
                    return true;
                }
                else {
                    return false;
                }
            }
            return true;
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
                alert("Record already Posted");
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

            var digits = "ABCDEFGHIJKLMNOPQRSTUVWXYZ abcdefghijklmnopqrstuvwxyz0123456789";
            var temp;
            if (document.getElementById("<%=txtBatchNo.ClientID %>") != null) {
                for (var i = 0; i < document.getElementById("<%=txtBatchNo.ClientID %>").value.length; i++) {
                    temp = document.getElementById("<%=txtBatchNo.ClientID%>").value.substring(i, i + 1);
                    if (digits.indexOf(temp) == -1) {
                        alert("Please Enter Correct Code");
                        document.getElementById("<%=txtBatchNo.ClientID%>").focus();
                        return false;
                    }
                }
            }

            if (document.getElementById("<%=ddlIntake.ClientID%>").value == "-1") {
                alert("Select Batch Intake");
                document.getElementById("<%=ddlIntake.ClientID%>").focus();
                return false;
            }

            if (document.getElementById("<%=txtBatchDate.ClientID%>") != null) {

                if (document.getElementById("<%=txtBatchDate.ClientID%>").value.replace(re, "$1").length == 0) {
                    alert("Batch Date Field Cannot Be Blank");
                    document.getElementById("<%=txtBatchDate.ClientID%>").focus();
                    return false;
                }
            }
            if (document.getElementById("<%=txtInvoiceDate.ClientID%>") != null) {
                if (document.getElementById("<%=txtInvoiceDate.ClientID%>").value == "") {
                    alert("Invoice Date Field Cannot Be Blank");
                    document.getElementById("<%=txtInvoiceDate.ClientID%>").focus();
                    return false;
                }
            }
            if (document.getElementById("<%=txtDesc.ClientID%>") != null) {
                if (document.getElementById("<%=txtDesc.ClientID%>").value.replace(re, "$1").length == 0) {
                    alert("Description Field Cannot Be Blank");
                    document.getElementById("<%=txtDesc.ClientID%>").focus();
                    return false;
                }
            }
            //txtBatchDate---------------------------------------------------------------------------
            if (document.getElementById("<%=txtBatchDate.ClientID%>") != null) {
                var len = document.getElementById("<%=txtBatchDate.ClientID%>").value
                var RegExPattern = /^((((0?[1-9]|[12]\d|3[01])[\.\-\/](0?[13578]|1[02])[\.\-\/]((1[6-9]|[2-9]\d)?\d{2}))|((0?[1-9]|[12]\d|30)[\.\-\/](0?[13456789]|1[012])[\.\-\/]((1[6-9]|[2-9]\d)?\d{2}))|((0?[1-9]|1\d|2[0-8])[\.\-\/]0?2[\.\-\/]((1[6-9]|[2-9]\d)?\d{2}))|(29[\.\-\/]0?2[\.\-\/]((1[6-9]|[2-9]\d)?(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00)|00)))|(((0[1-9]|[12]\d|3[01])(0[13578]|1[02])((1[6-9]|[2-9]\d)?\d{2}))|((0[1-9]|[12]\d|30)(0[13456789]|1[012])((1[6-9]|[2-9]\d)?\d{2}))|((0[1-9]|1\d|2[0-8])02((1[6-9]|[2-9]\d)?\d{2}))|(2902((1[6-9]|[2-9]\d)?(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00)|00))))$/;
                var errorMessage = 'Enter Valid Date (dd/mm/yyyy)';

                if (document.getElementById("<%=txtBatchDate.ClientID%>").value.match(RegExPattern)) {
                    if (len.length == 8) {
                        alert("ddd");
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
            }
            //txtInvoiceDate---------------------------------------------------------------------------
            if (document.getElementById("<%=txtInvoiceDate.ClientID%>") != null) {
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
                    alert("Invoice Date Cannot be Greater than Current Date");
                    document.getElementById("<%=txtInvoiceDate.ClientID%>").value = "";
                    document.getElementById("<%=txtInvoiceDate.ClientID%>").focus();
                    //"");
                    return false;
                }
            }

            if (document.getElementById("<%=txtDuedate.ClientID%>") != null) {
                if (document.getElementById("<%=txtDuedate.ClientID%>").value.replace(re, "$1").length == 0) {
                    alert("Due Date Field Cannot Be Blank");
                    document.getElementById("<%=txtDuedate.ClientID%>").focus();
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

                //txtDesc----------------------------------------------------
                if (document.getElementById("<%=txtDesc.ClientID%>").value == "") {
                    alert("Description Field Cannot Be Blank");
                    document.getElementById("<%=txtDesc.ClientID%>").focus();
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
                    alert("Due Date Should be Greater than Invoice Date");
                    document.getElementById("<%=txtDuedate.ClientID%>").value = "";
                    document.getElementById("<%=txtDuedate.ClientID%>").focus();
                    return false;
                }
            }
            if (document.getElementById("<%=txtInvoiceDate.ClientID %>") != null & document.getElementById("<%=txtBatchDate.ClientID %>")) {
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
        function ValidateObjects() {
            var jsFromDate = document.getElementById("<%=txtInvoiceDate.ClientID %>").value;
            var jsToDate = document.getElementById("<%=txtDuedate.ClientID %>").value;
            var TempFromDate = new Date(jsFromDate);
            var TempToDate = new Date(jsToDate);
            if (jsFromDate == '' && jsToDate != '') {
                alert("Please Enter Correct Record No");
                return false;
            }
            if (jsToDate == '' && jsFromDate != '') {
                alert("Please Enter Correct Record No");
                return false;
            }
            if (jsToDate == '' && jsFromDate == '') {
                alert("Please Enter Correct Record No");
                return false;
            }
            if (TempFromDate > TempToDate) {
                alert("Please Enter Correct Record No");

                return false;
            }
            else if (TempFromDate == TempToDate) {
                return true;
            }
            else if (TempFromDate <= TempToDate) {
                return true;
            }
        }
        //Print Record
        function getPrint() 
        {            
            var str1 =    document.getElementById("<%=txtBatchNo.ClientID %>").value;
            window.open('/GroupReport/RptSponsorCoverLetter.aspx?batchNo=' + str1, 'SAS', 'width=700,height=500,resizable=1');
        }

    </script>
    <%--</ContentTemplate>     
</atlas:UpdatePanel>--%>
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
                                <asp:ImageButton ID="ibtnDelete" runat="server" ImageUrl="~/images/delete.png" ToolTip="Delete" />
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
                        <tr id="trPrint" runat="server">
                            <td>
                                <asp:ImageButton ID="ibtnPrint" runat="server" ImageUrl="~/images/print.png" ToolTip="Print" />
                            </td>
                            <td>
                                <asp:Label ID="lblPrint" runat="server" Text="Print"></asp:Label>
                            </td>
                        </tr>
                    </table>
                </td>
                <td>
                    <table style="border-collapse: collapse;" class="menuoff" onmouseover="className='menuon';"
                        onmouseout="className='menuoff';">
                        <tr>
                            <td>
                                <asp:ImageButton ID="ibtnPosting" runat="server" ImageUrl="~/images/gposting.png" ToolTip="Cancel"  />
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
                    <asp:ImageButton ID="ibtnFirst" runat="server" ImageUrl="~/images/new_last.png" ToolTip="First" />
                </td>
                <td style="width: 2%; height: 14px">
                    <asp:ImageButton ID="ibtnPrevs" runat="server" ImageUrl="~/images/new_prev.png" ToolTip="Previous" />
                </td>
                <td style="width: 2%; height: 14px">
                    <asp:TextBox ID="txtRecNo" runat="server" AutoPostBack="True" Width="50px" OnTextChanged="txtRecNo_TextChanged"
                        MaxLength="7" ReadOnly="true" CssClass="text_box" disabled="disabled" TabIndex="1"
                        dir="ltr"></asp:TextBox>
                </td>
                <td style="width: 2%; height: 14px">
                    <asp:Label ID="Label47" runat="server">Of</asp:Label>
                </td>
                <td style="width: 2%; height: 14px">
                    <asp:Label ID="lblCount" runat="server" Width="20px"></asp:Label>
                </td>
                <td style="width: 2%; height: 14px">
                    <asp:ImageButton ID="ibtnNext" runat="server" ImageUrl="~/images/next.png" ToolTip="Next" />
                </td>
                <td style="width: 2%; height: 14px">
                    <asp:ImageButton ID="ibtnLast" runat="server" ImageUrl="~/images/new_first.png" ToolTip="Last" />
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
                <td width="50%">
                    <asp:SiteMapPath ID="SiteMapPath1" runat="server">
                    </asp:SiteMapPath>
                </td>
                <td width="50%" class="pagetext" align="right">
                    <asp:Label ID="lblMenuName" runat="server" Width="350px"></asp:Label>
                </td>
            </tr>
        </table>
    </asp:Panel>
    <%--<atlas:UpdateProgress ID="ProgressIndicator" runat="server">
    <ProgressTemplate>
        Loading the data, please wait... 
        <asp:Image ID="LoadingImage" ImageAlign="AbsMiddle" runat="server" ImageUrl="~/Images/spinner.gif" />        
    </ProgressTemplate>
 </atlas:UpdateProgress>
<atlas:UpdatePanel ID="up2" runat="server">
<ContentTemplate>--%>
    <%--   </ContentTemplate>     
</atlas:UpdatePanel>
    --%>
    <table>
        <tr>
            <td style="width: 98px; height: 1px">
                <table>
                    <tr>
                        <td>
                        </td>
                        <td colspan="3" style="height: 12px" align="left">
                            <asp:Label ID="lblMsg" runat="server" CssClass="lblError" Style="text-align: center"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>
                        </td>
                        <td>
                            <asp:Button ID="btnBatchInvoice" runat="server" Height="24px" Text="Invoice" Width="108px"
                                CssClass="TabButton" />
                        </td>
                        <td>
                            <asp:Button ID="btnViewStu" runat="server" Height="25px" Text="View Students" Width="108px"
                                CssClass="TabButton" />
                        </td>
                        <td>
                            <asp:Button ID="btnSelection" runat="server" Height="25px" Text="Selection Criteria" Visible="false"
                                Width="108px" CssClass="TabButton" />
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
                        <div style="border: thin solid #A6D9F4; width: 100%">
                            <asp:Panel ID="pnlBatch" runat="server" Height="100%" Width="100%">
                                <table style="width: 100%">
                                    <tr>
                                        <td style="height: 138px;">
                                            <table style="width: 50%">
                                                <tr>
                                                    <td style="height: 23px">
                                                        <span style="color: #ff0000">*</span>
                                                    </td>
                                                    <td style="width: 120px; height: 23px">
                                                        <asp:Label ID="Label1" runat="server" Text="Batch No" Width="59px"></asp:Label>
                                                    </td>
                                                    <td style="height: 23px">
                                                    </td>
                                                    <td colspan="3" style="height: 23px">
                                                        <asp:TextBox ID="txtBatchNo" runat="server" Width="142px" ></asp:TextBox>&nbsp;
                                                    </td>
                                                    <td style="width: 81px; height: 23px">
                                                        <span style="color: #ff0000"></span>
                                                    </td>
                                                    <td style="width: 133px; height: 23px">
                                                        &nbsp;</td>
                                                    <td style="width: 100px;" rowspan="6">
                                                        &nbsp;</td>
                                                </tr>
                                                <tr>
                                                    <td style="height: 25px">
                                                        <span style="color: #ff0000">*</span>
                                                    </td>
                                                    <td style="width: 120px; height: 25px">
                                                        <asp:Label ID="lblBatchIntake" runat="server" Text="Batch Intake" Width="59px"></asp:Label>
                                                    </td>
                                                    <td style="height: 25px">
                                                    </td>
                                                    <td style="height: 25px" >
                                                        <asp:DropDownList ID="ddlIntake" runat="server" AppendDataBoundItems="true">
                                                        </asp:DropDownList>
                                                        &nbsp;
                                                    </td>
                                                    <td></td>
                                                    <td rowspan="4" valign="top">
                                                        <asp:ImageButton ID="ibtnStatus" runat="server" CssClass="cursor" 
                                                            Enabled="False" ImageUrl="~/images/NotReady.gif" />
                                                    </td>
                                                    <td></td>
                                                    <td style="width: 133px; height: 25px">
                                                    </td>
                                                    
                                                </tr>
                                                <tr>
                                                    <td style="height: 25px">
                                                        <span style="color: #ff0000">*</span>
                                                    </td>
                                                    <td style="width: 120px; height: 25px">
                                                        <asp:Label ID="Label3" runat="server" Text="Batch Date" Width="61px"></asp:Label>
                                                    </td>
                                                    <td style="height: 25px">
                                                    </td>
                                                    <td style="width: 69px; height: 25px">
                                                        <asp:TextBox ID="txtBatchDate" runat="server" MaxLength="10" Width="73px"></asp:TextBox>
                                                        &nbsp;
                                                    </td>
                                                    <td style="width: 88px; height: 25px">
                                                        <asp:Image ID="ibtnBDate" runat="server" ImageUrl="~/images/cal.gif" />
                                                    </td>
                                                    <td style="width: 81px; height: 25px">
                                                        <asp:HiddenField ID="today" runat="server" />
                                                    </td>
                                                    <td style="width: 133px; height: 25px">
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="height: 25px">
                                                        <span style="color: #ff0000">*</span>
                                                    </td>
                                                    <td style="width: 120px; height: 25px">
                                                        <asp:Label ID="Label7" runat="server" Text="Invoice Date" Width="64px"></asp:Label>
                                                    </td>
                                                    <td style="height: 25px">
                                                    </td>
                                                    <td style="width: 69px; height: 25px">
                                                        <asp:TextBox ID="txtInvoiceDate" runat="server" Width="73px" MaxLength="10"></asp:TextBox>&nbsp;
                                                    </td>
                                                    <td style="width: 88px; height: 25px">
                                                        <asp:Image ID="ibtnInDate" runat="server" ImageUrl="~/images/cal.gif" />
                                                    </td>
                                                    <td style="width: 81px; height: 25px">
                                                    </td>
                                                    <td style="width: 133px; text-align: right;" rowspan="3">
                                                        <asp:Label ID="todate" runat="server" Text="                                         "
                                                            Width="64px" Visible="False"></asp:Label>&nbsp;&nbsp;
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="height: 25px" valign="top">
                                                        <span style="color: #ff0000">*</span>
                                                    </td>
                                                    <td style="width: 120px; height: 25px; vertical-align: top;">
                                                        <asp:Label ID="Label9" runat="server" Text="Due Date" Width="64px"></asp:Label>
                                                    </td>
                                                    <td style="vertical-align: top; height: 25px">
                                                    </td>
                                                    <td style="width: 69px; height: 25px; vertical-align: top;">
                                                        <asp:TextBox ID="txtDuedate" runat="server" onBlur="checkdate(this)" Width="73px"
                                                            MaxLength="10"></asp:TextBox>&nbsp;
                                                    </td>
                                                    <td style="vertical-align: top; width: 88px; height: 25px">
                                                        <asp:Image ID="ibtnDueDate" runat="server" ImageUrl="~/images/cal.gif" />
                                                    </td>
                                                    <td style="width: 81px; height: 25px">
                                                        <asp:Label ID="Label25" runat="server" Text="welcome to sas invoice screen" Visible="False"
                                                            Width="64px"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="height: 25px" valign="top">
                                                        <span style="color: #ff0000">*</span></td>
                                                    <td style="width: 120px; height: 25px; vertical-align: top;">
                                                        <asp:Label ID="Label23" runat="server" Text="Description "></asp:Label>
                                                    </td>
                                                    <td style="vertical-align: top; height: 25px">
                                                        &nbsp;</td>
                                                    <td style="height: 25px; vertical-align: top;" colspan="4">
                                                        <asp:TextBox ID="txtDesc" runat="server" Height="20px" MaxLength="50" 
                                                            Width="300px"></asp:TextBox>
                                                    </td>
                                                </tr>
                                            </table>
                                            <table width="60%">
                                                <tr>
                                                    <td>
                                                        &nbsp;</td>
                                                    <td style="width: 229px; padding-left: 5px">
                                                        &nbsp;</td>
                                                    <td>
                                                        &nbsp;</td>
                                                    <td>
                                                        &nbsp;
                                                    </td>
                                                    <td style="width: 497px">
                                                        &nbsp;
                                                    </td>
                                                    <td style="width: 100px">
                                                    </td>
                                                    <td style="width: 100px">
                                                    </td>
                                                </tr>
                                            </table>
                                            <table>
                                                <tr>
                                                    <td>
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="Label26" runat="server" Text="Add Fee Item" Width="64px"></asp:Label>
                                                    </td>
                                                    <td style="width: 4px; height: 23px;">
                                                        <asp:ImageButton ID="ibtnAddFeeType" runat="server" Height="21px" ImageUrl="~/images/addrec.gif"
                                                            ToolTip="Add" />
                                                    </td>
                                                    <td style="width: 135px; text-align: right; height: 23px;">
                                                        <asp:Label ID="Label27" runat="server" Text="Remove Fee Item" Width="90px"></asp:Label>
                                                    </td>
                                                    <td style="width: 26px; height: 23px;">
                                                        <asp:ImageButton ID="ibtnRemoveFee" runat="server" Height="21px" ImageUrl="~/images/removey.gif"
                                                            ToolTip="Remove" Width="20px" OnClick="ibtnRemoveFee_Click" />
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                </table>
                                <table style="width: 100%">
                                    <tr>
                                        <td width="100%" style="height: 261px">
                                            <asp:DataGrid ID="dgView" runat="server" AutoGenerateColumns="False" Width="100%"
                                                ShowFooter="True" DataKeyField="ReferenceCode" OnSelectedIndexChanged="dgView_SelectedIndexChanged"
                                                Style="vertical-align: top">
                                                <FooterStyle CssClass="dgFooterStyle" Height="20px" />
                                                <SelectedItemStyle CssClass="dgSelectedItemStyle" />
                                                <AlternatingItemStyle BackColor="Beige" CssClass="dgAlternatingItemStyle" Font-Bold="False"
                                                    Font-Italic="False" Font-Overline="False" Font-Strikeout="False" Font-Underline="False" />
                                                <ItemStyle CssClass="dgItemStyle" />
                                                <HeaderStyle BackColor="#CDD7EE" CssClass="dgHeaderStyle" Font-Bold="True" Font-Italic="False"
                                                    Font-Overline="False" Font-Size="Medium" Font-Strikeout="False" Font-Underline="False" />
                                                <Columns>
                                                    <asp:ButtonColumn CommandName="Select" DataTextField="ReferenceCode" HeaderText="Fee Code"
                                                        Text="ReferenceCode"></asp:ButtonColumn>
                                                    <asp:BoundColumn DataField="Description" HeaderText="Fee Desc" FooterText="Total">
                                                    </asp:BoundColumn>
                                                    <asp:TemplateColumn HeaderText="Fee Amount">
                                                        <ItemTemplate>
                                                            <asp:TextBox ID="txtFeeAmt" runat="server" Width="98px" AutoPostBack="True" OnTextChanged="txtFeeAmt_TextChanged"
                                                                Style="text-align: right" Height="18px" MaxLength="10"></asp:TextBox>
                                                        </ItemTemplate>
                                                        <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                                            Font-Underline="False" HorizontalAlign="Right" />
                                                        <FooterStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                                            Font-Underline="False" HorizontalAlign="Right" />
                                                        <HeaderStyle Width="15%" />
                                                    </asp:TemplateColumn>
                                                    <asp:BoundColumn DataField="TransactionAmount" HeaderText="FeeAmount" Visible="False">
                                                        <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                                            Font-Underline="False" HorizontalAlign="Right" />
                                                        <FooterStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                                            Font-Underline="False" HorizontalAlign="Right" />
                                                    </asp:BoundColumn>
                                                    <asp:BoundColumn DataField="Priority" HeaderText="Priority">
                                                        <HeaderStyle Width="15%" />
                                                    </asp:BoundColumn>
                                                    <asp:BoundColumn HeaderText="Fee Code" Visible="False"></asp:BoundColumn>
                                                </Columns>
                                            </asp:DataGrid>
                                        </td>
                                    </tr>
                                </table>
                                <table style="width: 100%; text-align: right;">
                                    <tr>
                                        <td style="width: 76%">
                                            <asp:Label ID="lblTotal" runat="server" Text="Total Amount" Width="65px" Visible="False"></asp:Label>
                                        </td>
                                        <td style="text-align: left">
                                            <asp:TextBox ID="txtTotal" runat="server" Width="106px" Visible="False" Style="text-align: right"></asp:TextBox>
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
                        <div style="border: thin solid #A6D9F4; width: 100%">
                            <asp:Panel ID="pnlSelection" runat="server" Height="100%" Width="100%" Visible="False">
                                <table style="width: 100%; height: 100%">
                                    <tr>
                                        <td style="width: 35%; height: 16px">
                                            <table style="width: 100%; height: 100%">
                                                <tr>
                                                    <td colspan="1" style="text-align: left">
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="1" style="text-align: left">
                                                        <asp:Label ID="Label19" runat="server" Text="Sponsor" Width="45px"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="width: 100px; height: 12px">
                                                        <asp:CheckBox ID="chkSelectSponsor" runat="server" Text="Select All" AutoPostBack="True" />
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 35%; vertical-align: top;">
                                            <asp:DataGrid ID="DgSponsor" runat="server" AutoGenerateColumns="False" Width="100%">
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
                                                            &nbsp;<asp:CheckBox ID="chk" runat="server" />
                                                        </ItemTemplate>
                                                    </asp:TemplateColumn>
                                                    <asp:BoundColumn DataField="SponserCode" HeaderText="Code"></asp:BoundColumn>
                                                    <asp:BoundColumn DataField="Name" HeaderText="Sponsor "></asp:BoundColumn>
                                                </Columns>
                                            </asp:DataGrid>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 35%;">
                                            <table style="width: 100%;">
                                                <tr>
                                                    <td style="width: 3px; text-align: right">
                                                    </td>
                                                    <td style="width: 100px">
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="width: 3px; text-align: right; height: 29px;">
                                                    </td>
                                                    <td align="right">
                                                        <asp:Button ID="btnUpdateCri" runat="server" Height="27px" Text="Update Criteria"
                                                            Width="177px" OnClick="btnUpdateCri_Click" />
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                </table>
                            </asp:Panel>
                        </div>
                    </td>
                </tr>
            </table>
        </asp:View>
        <asp:View ID="View3" runat="server">
        <div style="border: thin solid #A6D9F4; width: 99%">
        <br />
        <table style="width: 100%">
                <tr>
                    <td>
                        <table>
                            <tr>
                                <td>
                                    <asp:Label ID="Label48" runat="server" Text="Sponser" Width="104px"></asp:Label>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlSponsor" runat="server" Height="20px" Width="314px" AppendDataBoundItems="true">
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    <asp:ImageButton ID="ibtnLoad" runat="server" ImageUrl="~/images/find.gif" TabIndex="5" />
                                </td>
                                <td>
                                    <asp:Label ID="Label12" runat="server" Text="Search" Width="55px"></asp:Label>
                                </td>
                            </tr>

                        
                        </table>
                    </td>
                </tr>
                <tr>
                    <td style="width: 100%">
                        <fieldset style="width: 98%">
                            <legend><strong><span style="color: #000000;"></span></strong></legend>
                            <asp:Panel ID="pnlView" runat="server" Height="100%" Width="100%" Visible="False">
                                <table style="width: 100%;">
                                    <tr>
                                        <td style="width: 100%; vertical-align: top;">
                                            <br />
                                            <asp:Label ID="Label2" runat="server" Text="Add Student" Width="68px" Visible="False"></asp:Label>
                                            <asp:ImageButton ID="ibtnStudent" runat="server" Height="16px" Width="16px" ImageUrl="~/images/find_img.png"
                                                ToolTip="Select Student" Visible="False" /><br />
                                            <asp:CheckBox ID="chkStudent" runat="server" OnCheckedChanged="chkStudent_CheckedChanged"
                                                AutoPostBack="True" Text="Select All" />
                                            <asp:DataGrid ID="dgStudent" runat="server" AutoGenerateColumns="False" Width="100%" OnItemDataBound="dgStudent_ItemDataBound">
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
                                                            &nbsp;<asp:CheckBox ID="chk" runat="server" AutoPostBack="True" />
                                                        </ItemTemplate>
                                                    </asp:TemplateColumn>
                                                    <asp:BoundColumn DataField="MatricNo" HeaderText="Matric No" Visible ="false"></asp:BoundColumn>
                                                    <asp:BoundColumn DataField="StudentName" HeaderText="Student Name" Visible ="false"></asp:BoundColumn>
                                                    
                                                    <asp:BoundColumn DataField="ProgramID" HeaderText="Program Id"></asp:BoundColumn>
                                                    <asp:BoundColumn DataField="CurrentSemester" HeaderText="Semester" Visible="false"></asp:BoundColumn>
                                                    <asp:BoundColumn DataField="SponsorCode" HeaderText="Sponsor Code" Visible="false"></asp:BoundColumn>
                                                       <asp:BoundColumn DataField="ProgramType" HeaderText="Program Name"></asp:BoundColumn>
                                                    
                                                     <asp:TemplateColumn HeaderText="View">
                                                     <HeaderStyle HorizontalAlign="Center" Width="12%" />
                                                        <ItemTemplate>
                                                        <center>
                                                           <asp:LinkButton ID="lnkView" runat="server" >View Student</asp:LinkButton>
                                                        </center>
                                                       </ItemTemplate>
                                                        </asp:TemplateColumn>



                                                </Columns>
                                            </asp:DataGrid><br />
                                            <br />
                                        </td>
                                    </tr>
                                </table>
                            </asp:Panel>
                        </fieldset>
                    </td>
                </tr>
            </table>
            </div> 
        </asp:View>
    </asp:MultiView>
    <asp:Button ID="btnHidden" runat="Server" OnClick="btnHidden_Click" Style="display: none" />
     <asp:HiddenField ID="lblStatus" runat="server" />
    <%--   </ContentTemplate>     
</atlas:UpdatePanel>
    --%>
</asp:Content>
