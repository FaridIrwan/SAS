<%@ Page Language="VB" MasterPageFile="~/MasterPage3.master" AutoEventWireup="false"
    CodeFile="RptStudentOverPaid.aspx.vb" Inherits="RptStudentOverPaid" Title="Student Over Paid Balance Report" %>

<asp:content ID="Content2" contentplaceholderid="head" runat="server">
    <script src="../Scripts/functions.js" type="text/javascript"></script>
    <script src="../Scripts/popcalendar.js" type="text/javascript"></script>
    <link href="../style.css" rel="stylesheet" />
    <title ></title>
</asp:content>

<%@ Register Assembly="CrystalDecisions.Web, Version=13.0.2000.0, Culture=neutral, PublicKeyToken=692fbea5521e1304"
    Namespace="CrystalDecisions.Web" TagPrefix="CR" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <script language="javascript" type="text/javascript">

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
            var curenttempdate = dateString()

            var todate = new Date(document.getElementById("<%=txtTodate.ClientID%>").value)
            var currentdate = new Date(curenttempdate)

            //Check dllStatus
            if (document.getElementById("<%=ddlStuStatus.ClientID%>").selectedIndex == 0) {
                alert("Please select a status")
                return false;
            }

            //Checking Date should not be empty
            if (document.getElementById("<%=txtTodate.ClientID%>").value.length == 0 || document.getElementById("<%=txtToDate.ClientID%>").value.length == 0) {
                alert("Date Field Cannot Be Blank")
                return false;
            }

            //Checking correct date format
            var len = document.getElementById("<%=txtTodate.ClientID%>").value
            var RegExPattern = /^((((0?[1-9]|[12]\d|3[01])[\.\-\/](0?[13578]|1[02])[\.\-\/]((1[6-9]|[2-9]\d)?\d{2}))|((0?[1-9]|[12]\d|30)[\.\-\/](0?[13456789]|1[012])[\.\-\/]((1[6-9]|[2-9]\d)?\d{2}))|((0?[1-9]|1\d|2[0-8])[\.\-\/]0?2[\.\-\/]((1[6-9]|[2-9]\d)?\d{2}))|(29[\.\-\/]0?2[\.\-\/]((1[6-9]|[2-9]\d)?(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00)|00)))|(((0[1-9]|[12]\d|3[01])(0[13578]|1[02])((1[6-9]|[2-9]\d)?\d{2}))|((0[1-9]|[12]\d|30)(0[13456789]|1[012])((1[6-9]|[2-9]\d)?\d{2}))|((0[1-9]|1\d|2[0-8])02((1[6-9]|[2-9]\d)?\d{2}))|(2902((1[6-9]|[2-9]\d)?(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00)|00))))$/;
            var errorMessage = 'Enter Valid Date (dd/mm/yyyy)';

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
            var str2 = document.getElementById("<%=txtTodate.ClientID %>").value;
            var dt2 = parseInt(str2.substring(0, 2), 10);
            var mon2 = parseInt(str2.substring(3, 5), 10);
            var yr2 = parseInt(str2.substring(6, 10), 10);
            var date2 = new Date(yr2, mon2, dt2);

            if (document.getElementById("<%=txtTodate.ClientID%>").value.length != 10) {
                alert("Enter Valid Date (dd/mm/yyyy)")
                document.getElementById("<%=txtTodate.ClientID%>").focus()
                return false
            }

            dllValues()
        }
        //Finish GetDate()

        function dllValues() {
            var fromdate1 = "0"
            var todate1 = "0"
            var lbl = false
            if ((document.getElementById("<%=ddlStuStatus.ClientID%>").value == "-1") && (document.getElementById("<%=ddlFaculty.ClientID%>").value == "-1") && (document.getElementById("<%=ddlProgram.ClientID%>").value == "-1")) {
                if (confirm("Do you Want to Show All Records?")) {
                    todate1 = document.getElementById("<%=txtTodate.ClientID%>").value
                    lbl = true;
                    OpenPopup('../GroupReport/RptStudentOverPaidViewer.aspx?tdate=' + todate1, 'Over Paid Report', '700', '650')
                    //new_window = window.open('RptOverChargeFromTo.aspx?tdate=' + todate1, 'SAS', 'width=700,height=500,resizable=1'); new_window.focus();
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
                todate1 = document.getElementById("<%=txtTodate.ClientID%>").value
                OpenPopup('../GroupReport/RptStudentOverPaidViewer.aspx?tdate=' + todate1, 'Over Paid Report', '700', '650')
                //new_window = window.open('RptOverChargeFromTo.aspx?tdate=' + todate1, 'SAS', 'width=700,height=500,resizable=1'); new_window.focus();
                return true;
            }
        }


        function dllValues1() {

            var lbl = false
            if ((document.getElementById("<%=ddlStuStatus.ClientID%>").value == "-1") && (document.getElementById("<%=ddlFaculty.ClientID%>").value == "-1") && (document.getElementById("<%=ddlProgram.ClientID%>").value == "-1")) {
                if (confirm("Do you Want to Show All Records?")) {
                    lbl = true;
                    OpenPopup('../GroupReport/RptStudentOverPaidViewer.aspx', 'Over Paid Report', '700', '650')
                    //new_window = window.open('RptOverChargeFromTo.aspx', 'SAS', 'width=700,height=500,resizable=1'); new_window.focus();
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
                OpenPopup('../GroupReport/RptStudentOverPaidViewer.aspx', 'Over Paid Report', '700', '650')
                //new_window = window.open('RptOverChargeFromTo.aspx', 'SAS', 'width=700,height=500,resizable=1'); new_window.focus();
                return true;
            }
        }

        function OpenPopup(pageURL, title, w, h) {
            var left = (screen.width - w) / 2;
            var top = (screen.height - h) / 4;  // for 25% - devide by 4  |  for 33% - devide by 3
            var targetWin = window.open(pageURL, title, 'toolbar=no, location=no, directories=no, status=no, menubar=no, scrollbars=yes, resizable=yes, copyhistory=no, width=' + w + ', height=' + h + ', top=' + top + ', left=' + left);
            targetWin.focus();
            return true;
        }
    </script>

    <asp:Panel ID="Panel1" runat="server" Width="100%">
        <table style="background-image: url(../images/Sample.png);">
            <tr>
                <td style="width: 4px; height: 14px"></td>
                <td style="width: 14px; height: 14px"></td>
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
                                <asp:ImageButton ID="ibtnCancel" runat="server" ImageUrl="~/images/cancel.png" ToolTip="Cancel" />
                            </td>
                            <td style="width: 3%; height: 14px">
                                <asp:Label ID="Label18" runat="server" Text="Cancel"></asp:Label>
                            </td>
                        </tr>
                    </table>
                </td>
                <td style="width: 100%; height: 14px"></td>
                <td style="width: 100%; height: 14px"></td>
            </tr>
        </table>
        <table class="mainbg" style="width: 100%;">
            <tr>
                <td class="vline" style="width: 98%; height: 1px"></td>
            </tr>
        </table>
        <table class="mainbg" style="width: 100%;">
            <tr>
                <td style="width: 494px; height: 39px;">
                    <asp:SiteMapPath ID="SiteMapPath1" runat="server" Width="250px">
                    </asp:SiteMapPath>
                </td>
                <td align="right" class="pagetext" style="height: 39px">
                    <asp:Label ID="lblMenuName" runat="server" Text="Over Paid Balance" Width="450px"></asp:Label>
                </td>
            </tr>
        </table>
    </asp:Panel>

    <asp:Panel ID="Panel2" runat="server" Width="100%">
        <table style="width: 100%">
            <tr>
                <td style="width: 1px; height: 16px"></td>
                <td style="width: 1005px; height: 16px">&nbsp;<fieldset style="width: 100%; border: thin solid #A6D9F4;">
                    <legend><strong><span style="color: #000000">Selection Criteria</span></strong></legend>
                    <table style="width: 100%">
                        <tr>
                            <td style="width: 10%; height: 16px">
                                <asp:Label ID="Label3" runat="server" Text="Status"></asp:Label>
                            </td>
                            <td style="height: 16px">
                                <asp:DropDownList ID="ddlStuStatus" runat="server" Width="135px" AutoPostBack="true">
                                    <asp:ListItem>-- Select --</asp:ListItem>
                                    <asp:ListItem>Active</asp:ListItem>
                                    <asp:ListItem>Inactive</asp:ListItem>
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td style="height: 16px">
                                <asp:Label ID="Label9" runat="server" Text="By Faculty"></asp:Label>
                            </td>
                            <td style="height: 16px">
                                <asp:DropDownList ID="ddlFaculty" runat="server" Width="400px" AutoPostBack="true">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td style="height: 16px">
                                <asp:Label ID="Label10" runat="server" Text="By Program"></asp:Label>
                            </td>
                            <td style="height: 16px">
                                <asp:DropDownList ID="ddlProgram" runat="server" Width="400px" AutoPostBack="true">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td style="height: 16px">
                                <asp:Label ID="lblByDate" runat="server" Text="By Date"></asp:Label>
                            </td>
                            <td>
                                <table>
                                    <tr>
                                        <td style="width: 9px; height: 16px">
                                            <asp:Label ID="Label2" runat="server" Text="To"></asp:Label>
                                        </td>
                                        <td style="width: 63px; height: 16px">
                                            <asp:TextBox ID="txtTodate" runat="server"></asp:TextBox>
                                        </td>
                                        <td style="width: 50px; height: 16px">
                                            <asp:Image ID="ibtnTodate" runat="server" ImageUrl="~/images/cal.gif" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </fieldset>
                </td>
                <td style="width: 113px; height: 16px"></td>
            </tr>
            <tr>
                <td style="height: 16px"></td>
                <td style="width: 1005px; height: 16px">
                    <fieldset style="width: 100%; border: thin solid #A6D9F4;">
                        <legend><strong><span style="color: #000000;">Record Sort By</span></strong></legend>
                        <table style="width: 100%">
                            <tr>
                                <td style="height: 16px; width: 224px;">&nbsp;<asp:RadioButton ID="rdbtnstudid" runat="server" Text="Matric Id" GroupName="RecordSort"
                                    AutoPostBack="True" OnCheckedChanged="rdbtnstudid_CheckedChanged" />
                                </td>
                                <td style="width: 458px; height: 16px"></td>
                                <td style="width: 180px; height: 16px"></td>
                                <td style="width: 10px; height: 16px"></td>
                            </tr>
                            <tr>
                                <td style="height: 16px; width: 224px;">&nbsp;<asp:RadioButton ID="rdbtnstudname" runat="server" Text="Student Name" Width="123px"
                                    GroupName="RecordSort" AutoPostBack="True" OnCheckedChanged="rdbtnstudname_CheckedChanged" />
                                </td>
                                <td style="width: 458px; height: 16px"></td>
                                <td style="width: 180px; height: 16px"></td>
                                <td style="width: 10px; height: 16px"></td>
                            </tr>
                        </table>
                    </fieldset>
                </td>
                <td style="height: 16px;"></td>
            </tr>
        </table>
    </asp:Panel>
    <%-- 
</ContentTemplate>     
</atlas:UpdatePanel>--%>
</asp:Content>
