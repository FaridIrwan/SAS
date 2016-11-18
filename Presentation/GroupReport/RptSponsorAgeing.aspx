<%@ Page Language="VB" MasterPageFile="~/MasterPage3.master" AutoEventWireup="false"
    CodeFile="RptSponsorAgeing.aspx.vb" Inherits="RptSponsorAgeing" Title="Welcome To SAS" %>

<asp:content ID="Content2" contentplaceholderid="head" runat="server">
    <script src="../Scripts/functions.js" type="text/javascript"></script>
    <script src="../Scripts/popcalendar.js" type="text/javascript"></script>
    <link href="../style.css" rel="stylesheet" />
</asp:content>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <style type="text/css">
        #Day
        {
            display: none;
        }
        #Quarter
        {
            display: none;
        }
    </style>

    <script language="javascript" type="text/javascript">

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

        //modified by Hafiz @ 09/3/2016

        function getDate() {
            var curenttempdate = dateString()

            var fromdate = new Date(document.getElementById("<%=txtFrom.ClientID%>").value)
            var todate = new Date(document.getElementById("<%=txtTodate.ClientID%>").value)
            var currentdate = new Date(curenttempdate)

            if (document.getElementById("<%=ChkBoxDate.ClientID%>").checked) {
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
            var timeDiff = date2 - date1;
            daysDiff = Math.floor(timeDiff / (24 * 3600 * 1000));

            if (document.getElementById("<%=txtFrom.ClientID%>").value.length != 10) {
                alert("Enter Valid Date (dd/mm/yyyy)")
                document.getElementById("<%=txtFrom.ClientID%>").focus();
                return false
            }

            if (document.getElementById("<%=txtTodate.ClientID%>").value.length != 10) {
                alert("Enter Valid Date (dd/mm/yyyy)")
                document.getElementById("<%=txtTodate.ClientID%>").focus()
                return false
            }
            if (daysDiff > 31) {
                alert("Date Range must 30 days");
                return false;
            }

            if (date2 < date1) {
                alert("From Date Cannot Be Greater Than To Date");
                return false;
            }
            if (date2 < date1) {
                alert("From Date Cannot Be Greater Than To Date");
                return false;
            }
            else
            {
                //DATE RANGE
                dllValues1()
            }

            //CHECK FOR DATE RANGE - START
            function dllValues1()
            {
                var fromdate1 = "0"
                var todate1 = "0"

                if (document.getElementById("<%=ChkBoxDate.ClientID%>").checked) {
                    var lbl = false
                    if ((document.getElementById("<%=ddlSponsor.ClientID%>").value == "-1") && (document.getElementById("<%=ChkBoxDate.ClientID%>").checked == true)) {
                        if (confirm("Do You Want To Show All Records?")) {
                            if (document.getElementById("<%=ChkBoxDate.ClientID%>").checked) {
                            fromdate1 = document.getElementById("<%=txtFrom.ClientID%>").value
                            todate1 = document.getElementById("<%=txtTodate.ClientID%>").value
                        }

                        lbl = true;
                        window.open('../GroupReport/RptSpAgeingDateCRFormFinal.aspx?fdate=' + fromdate1 + '&tdate=' + todate1, 'SAS', 'width=700,height=500,resizable=1');
                        return lbl;
                    }
                    else {
                        if (document.getElementById("<%=ddlSponsor.ClientID%>").value == "-1") {
                            document.getElementById("<%=ddlSponsor.ClientID%>").focus();
                            return lbl;
                        }
                    }
                }
                else {
                    if (document.getElementById("<%=ChkBoxDate.ClientID%>").checked) {
                        fromdate1 = document.getElementById("<%=txtFrom.ClientID%>").value
                        todate1 = document.getElementById("<%=txtTodate.ClientID%>").value
                    }

                    new_window = window.open('../GroupReport/RptSpAgeingDateCRFormFinal.aspx?fdate=' + fromdate1 + '&tdate=' + todate1, 'SAS', 'width=700,height=500,resizable=1'); new_window.focus();
                    return true;
                    }
                } 
            }
            //CHECK FOR DATE RANGE - END

            //CHECK FOR YEARLY - START
            if (document.getElementById("<%=CheckBoxYear.ClientID%>").checked) {

                var lbl = false
                if ((document.getElementById("<%=ddlsponsor.ClientID%>").value == "-1") && (document.getElementById("<%=CheckBoxYear.ClientID%>").checked == true)) {
                    if (confirm("Do You Want To Show All Records?")) {
                        if (document.getElementById("<%=ChkBoxDate.ClientID%>").checked) {
                            new_window = window.open('../GroupReport/RptSponsorAgeingViewer.aspx', 'SAS', 'width=700,height=500,resizable=1'); new_window.focus();
                        }
                        lbl = true;
                        new_window = window.open('../GroupReport/RptSponsorAgeingViewer.aspx', 'SAS', 'width=700,height=500,resizable=1'); new_window.focus();
                        return lbl;
                    }
                    else {
                        if (document.getElementById("<%=ddlsponsor.ClientID%>").value == "-1") {
                            document.getElementById("<%=ddlsponsor.ClientID%>").focus();
                            return lbl;
                        }
                    }
                }
                else {
                    new_window = window.open('../GroupReport/RptSponsorAgeingViewer.aspx', 'SAS', 'width=700,height=500,resizable=1'); new_window.focus();
                    return true;


                }
            }
            //CHECK FOR YEARLY - END

            //CHECK FOR MONTHLY - START
            if (document.getElementById("<%=CheckBoxMonth.ClientID%>").checked) {
                var lbl = false
                if ((document.getElementById("<%=ddlsponsor.ClientID%>").value == "-1") && (document.getElementById("<%=CheckBoxMonth.ClientID%>").checked == true)) {

                    if (confirm("Do You Want To Show All Records?")) {
                        if (document.getElementById("<%=CheckBoxMonth.ClientID%>").checked) {
                            new_window = window.open('../GroupReport/RptSpAgeingMonthCRFormFinal.aspx', 'SAS', 'width=700,height=500,resizable=1'); new_window.focus();
                        }

                        lbl = true;
                        new_window = window.open('../GroupReport/RptSpAgeingMonthCRFormFinal.aspx', 'SAS', 'width=700,height=500,resizable=1'); new_window.focus();
                        return lbl;
                    }
                    else {
                        if (document.getElementById("<%=ddlsponsor.ClientID%>").value == "-1") {
                            document.getElementById("<%=ddlsponsor.ClientID%>").focus();
                            return lbl;
                        }
                    }
                }
                else {
                    new_window = window.open('../GroupReport/RptSpAgeingMonthCRFormFinal.aspx', 'SAS', 'width=700,height=500,resizable=1'); new_window.focus();
                    return true;


                }
            }
            //CHECK FOR MONTHLY - END

            //CHECK FOR QUATERLY - START
            var curenttempdate = dateString()
            if (document.getElementById("<%=CheckBoxQuarter.ClientID%>").checked) {

                if (len != 0) {
                    if (len < 7 || len > 12) {
                        //alert("Give values 30,60,90,120 format")
                        return false
                    }
                    else {
                        var lbl = true
                        if ((document.getElementById("<%=ddlsponsor.ClientID%>").value == "-1") && (document.getElementById("<%=CheckBoxQuarter.ClientID%>").checked == true)) {
                            if (confirm("Do You Want To Show All Records?")) {
                                if (document.getElementById("<%=CheckBoxQuarter.ClientID%>").checked) {
                                    new_window = window.open('../GroupReport/RptSpAgeingCRFormFinal.aspx', 'SAS', 'width=700,height=500,resizable=1'); new_window.focus();
                                }
                                lbl = false;
                                new_window = window.open('../GroupReport/RptSpAgeingCRFormFinal.aspx', 'SAS', 'width=700,height=500,resizable=1'); new_window.focus();
                                return lbl;
                            }
                            else {
                                if (document.getElementById("<%=ddlsponsor.ClientID%>").value == "-1") {
                                    document.getElementById("<%=ddlsponsor.ClientID%>").focus();
                                    return lbl;
                                }
                            }
                        }
                        else {
                            new_window = window.open('../GroupReport/RptSpAgeingCRFormFinal.aspx', 'SAS', 'width=700,height=500,resizable=1'); new_window.focus();
                            return true;
                        }
                    }
                }
            }
            //CHECK FOR QUATERLY - END
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
                                <asp:ImageButton ID="ibtnNew" runat="server" ImageUrl="~/images/add.png" ToolTip="New"
                                    OnClick="ibtnNew_Click" />
                            </td>
                            <td style="width: 3%; height: 14px">
                                <asp:Label ID="Label11" runat="server" Text="New"></asp:Label>
                            </td>
                        </tr>
                    </table>
                </td>
                <%--Eddited by Zoya @26/02/2016--%>
                <td style="float: left;display:None; visibility:collapse;">
                    <table style="border-collapse: collapse;" class="menuoff" onmouseover="className='menuon';"
                        onmouseout="className='menuoff';">
                        <tr>
                            <td style="width: 3%; height: 14px">
                                <asp:ImageButton ID="ibtnSave" runat="server" ImageUrl="~/images/save.png" ToolTip="Save" Visible="False"/>
                            </td>
                            <td style="width: 3%; height: 14px">
                                <asp:Label ID="Label14" runat="server" Text="Save" Visible="False"></asp:Label>
                            </td>
                        </tr>
                    </table>
                </td>
                <td style="float: left;display:None; visibility:collapse;">
                    <table style="border-collapse: collapse;" class="menuoff" onmouseover="className='menuon';"
                        onmouseout="className='menuoff';">
                        <tr>
                            <td style="height: 14px; width: 3%;">
                                <asp:ImageButton ID="ibtnDelete" runat="server" ImageUrl="~/images/delete.png" Visible="False"/>
                            </td>
                            <td style="width: 3%; height: 14px">
                                <asp:Label ID="Label13" runat="server" Text="Delete" Visible="False"></asp:Label>
                            </td>
                        </tr>
                    </table>
                </td>
                <td style="float: left;display:None; visibility:collapse;">
                    <table style="border-collapse: collapse;" class="menuoff" onmouseover="className='menuon';"
                        onmouseout="className='menuoff';">
                        <tr>
                            <td style="width: 3%; height: 14px">
                                <asp:ImageButton ID="ibtnView" runat="server" ImageUrl="~/images/find.png" ToolTip="View"
                                    OnClick="ibtnView_Click" Visible="False"/>
                            </td>
                            <td style="width: 3%; height: 14px">
                                <asp:Label ID="Label16" runat="server" Text="Search" Visible="False"></asp:Label>
                            </td>
                        </tr>
                    </table>
                </td>
                <%--Done Eddited by Zoya @26/02/2016--%>

               <%--Commented by Zoya @26/02/2016--%>
               <%-- <td>
                    <table style="border-collapse: collapse;" class="menuoff" onmouseover="className='menuon';"
                        onmouseout="className='menuoff';">
                         <tr>
                            <td style="width: 3%; height: 14px">
                                <div id="wrap">
                                    <ul id="navbar">
                                        <li><a href="#">
                                            <img id="ibtnPrint" src="/images/print.png"  width="24" height="24"  border="0" align="middle" />&nbsp;Print                                   
                                            <img src="/images/down.png" width="16" height="16" align="middle" border="0" /></a>
                                            <ul>
                                                <li><a href="#">
                                                    <asp:ImageButton ID="ibtnBMReport" runat="server" ImageUrl="~/images/print-bm.png"/>
                                                    </a></li>
                                                <li><a href="#">
                                                    <asp:ImageButton ID="ibtnEnReport" runat="server" ImageUrl="~/images/print-eng.png"/>
                                                    </a></li>
                                            </ul>
                                        </li>
                                    </ul>
                                </div>
                            </td>
                        </tr>
                    </table>
                </td>--%>
                <%--End Commented by Zoya @26/02/2016--%>

                 <%--Added by Zoya @26/02/2016 -  link to Malay Report--%>
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
                 <%--Done Added by Zoya @26/02/2016 - link to Malay Report--%>

                <%-- Editted By Zoya @25/02/2016--%>
                <td style="float: left;display:None; visibility:collapse;">
                    <table style="border-collapse: collapse;" class="menuoff" onmouseover="className='menuon';"
                        onmouseout="className='menuoff';">
                        <tr>
                            <td style="width: 3%; height: 14px">
                                <asp:ImageButton ID="ibtnPosting" runat="server" ImageUrl="~/images/gposting.png"
                                    ToolTip="Cancel" Visible="false"/>
                            </td>
                            <td style="width: 3%; height: 14px">
                                &nbsp;<asp:Label ID="Label6" runat="server" Text="Posting" Visible="false"></asp:Label>
                            </td>
                        </tr>
                    </table>
                </td>
               
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
                                    OnClick="ibtnCancel_Click" />
                            </td>
                            <td style="width: 3%; height: 14px">
                                <asp:Label ID="Label18" runat="server" Text="Cancel"></asp:Label>
                            </td>
                        </tr>
                    </table>
                </td>
                <%-- Done Editted By Zoya @25/02/2016--%>
                <%--Editted By Zoya @2/03/2016--%>
                <td style="float: left;display:None; visibility:collapse;width: 2%; height: 14px">
                    <asp:ImageButton ID="ibtnFirst" runat="server" ImageUrl="~/images/new_last.png" Visible="false"/>
                </td>
                <td style="float: left;display:None; visibility:collapse;width: 2%; height: 14px">
                    <asp:ImageButton ID="ibtnPrevs" runat="server" ImageUrl="~/images/new_prev.png" Visible="false"/>
                </td>
                <td style="float: left;display:None; visibility:collapse;width: 2%; height: 14px">
                    <asp:TextBox ID="txtRecNo" runat="server" ReadOnly="true" CssClass="text_box" Width="52px"
                        AutoPostBack="True" Style="text-align: right" disabled="disabled" TabIndex="1"
                        dir="ltr" Visible="false"></asp:TextBox>
                </td>
                <td style="float: left;display:None; visibility:collapse;width: 2%; height: 14px">
                    <asp:Label ID="Label47" runat="server" Visible="false">Of</asp:Label>
                </td>
                <td style="float: left;display:None; visibility:collapse;width: 2%; height: 14px">
                    <asp:Label ID="lblCount" runat="server" Width="20px" Visible="false"></asp:Label>
                </td>
                <td style="float: left;display:None; visibility:collapse;width: 2%; height: 14px">
                    <asp:ImageButton ID="ibtnNext" runat="server" ImageUrl="~/images/new_next.png" Visible="false"/>
                </td>
                <td style="float: left;display:None; visibility:collapse;width: 2%; height: 14px">
                    <asp:ImageButton ID="ibtnLast" runat="server" ImageUrl="~/images/new_first.png" Visible="false"/>
                </td>
                <%--Done Editted By Zoya @2/03/2016--%>
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
                    <asp:SiteMapPath ID="SiteMapPath1" runat="server" Width="369px">
                    </asp:SiteMapPath>
                </td>
                <td align="right" class="pagetext" style="height: 39px">
                    <asp:Label ID="lblMenuName" runat="server" Text="University Fund" Width="400px"></asp:Label>
                </td>
            </tr>
        </table>
    </asp:Panel>

    <table style="width: 99%">
        <tr>
            <td width="1%">
            </td>
            <td width="98%">
                <fieldset style="border: thin solid #A6D9F4; width:100%">
                    <legend><strong><span style="color: #000000;">Selection Criteria</span></strong></legend>
                    <table style="width: 50%; text-align: right;">
                        <tr>
                            <td style="text-align: left; width: 77px;" width="50">
                                <asp:Label ID="Label2" runat="server" Text="By Sponsor" Width="65px" Height="16px"></asp:Label>
                            </td>
                            <td style="height: 14px; text-align: left;" colspan="5">
                                <asp:DropDownList ID="ddlSponsor" runat="server" Style="width: 300px;" onmousedown="this.style.width=''"
                                    onblur="this.style.width='139px'" AutoPostBack="True" AppendDataBoundItems="True"
                                    OnSelectedIndexChanged="ddlSponsor_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: left;" class="style2">
                                <asp:Label ID="Label3" runat="server" Text="Ageing Period" Width="99px"></asp:Label>
                            </td>
                            <td style="text-align: left;" class="style3" colspan="2">
                                <asp:CheckBox ID="ChkBoxDate" runat="server" Text="Date Range" Width="169px" AutoPostBack="True" Checked="true"
                                    OnCheckedChanged="ChkBoxDate_CheckedChanged" />
                            </td>
                            <td style="width: 34px; height: 16px">
                                &nbsp;
                            </td>
                            <td style="text-align: left" colspan="2">
                            </td>
                            <td style="height: 16px; text-align: left">
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: left;">
                            </td>
                            <td style="text-align: right;">
                                <asp:Label ID="lblfrom" runat="server" Text="From"></asp:Label>
                            </td>
                            <td style="text-align: left; width: 20px;">
                                <asp:TextBox ID="txtFrom" runat="server"></asp:TextBox>
                            </td>
                            <td style="width: 34px; text-align: left; height: 16px">
                                <asp:Image ID="ibtnFDate" runat="server" ImageUrl="~/images/cal.gif" />
                            </td>
                            <td style="text-align: right">
                                <asp:Label ID="lblto" runat="server" Text="To"></asp:Label>
                            </td>
                            <td style="text-align: left">
                                <asp:TextBox ID="txtTodate" runat="server"></asp:TextBox>
                            </td>
                            <td style="width: 34px; text-align: left; height: 16px">
                                <asp:Image ID="ibtnTodate" runat="server" ImageUrl="~/images/cal.gif" />
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: left;" class="style2">
                            </td>
                            <td style="text-align: left;" class="style3" colspan="2">
                                <asp:CheckBox ID="CheckBoxMonth" runat="server" Text="Monthly" Width="169px" AutoPostBack="True" OnCheckedChanged="CheckBoxMonth_CheckedChanged" />
                            </td>
                            <td style="width: 10px; height: 15px">
                            </td>
                            <td colspan="2">
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: left;" class="style2">
                            </td>
                            <td style="text-align: left;" class="style3" colspan="2">
                                <asp:CheckBox ID="CheckBoxQuarter" runat="server" Text="Quarterly (30,60,90,120)"
                                    Width="169px" AutoPostBack="True" OnCheckedChanged="CheckBoxQuarter_CheckedChanged" />
                            </td>
                            <td style="width: 10px; height: 15px">
                                &nbsp;
                            </td>
                            <td colspan="2">
                            </td>
                            <td>
                            </td>
                        </tr>
                        <%-- <tr>
                                <td style="text-align: left; "></td>
                                <td style="text-align: left;"> &nbsp;</td>
                                <td style="text-align: left;">
                                    <asp:TextBox ID="txtperiod" runat="server" Width="135px" />
                                </td>
                                <td style="height: 15px; text-align:left;" colspan="2"> <asp:Label ID="lblperiod" Text="(30,60,90,120)" runat="server" >(30,60,90,120)</asp:Label></td>
                                 <td colspan="2" style="height: 15px">
                                     &nbsp;</td>
                                 <td> </td>
                            </tr>--%>
                        <tr>
                            <td style="text-align: left;" class="style2">
                            </td>
                            <td style="text-align: left;" class="style3" colspan="2">
                                <asp:CheckBox ID="CheckBoxYear" runat="server" Text="Yearly" Width="169px" AutoPostBack="True"
                                    OnCheckedChanged="CheckBoxYear_CheckedChanged" />
                            </td>
                            <td style="width: 10px; height: 15px">
                            </td>
                            <td colspan="2">
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: left;" class="style2">
                            </td>
                            <td class="style3" colspan="2">
                            </td>
                            <td style="width: 10px; height: 15px">
                            </td>
                            <td class="style4" colspan="2">
                            </td>
                        </tr>
                    </table>
                </fieldset>
            </td>
            <td style="width: 5760px; height: 16px">
            </td>
        </tr>
    </table>

</asp:Content>
