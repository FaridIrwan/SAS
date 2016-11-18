<%@ Page Language="VB" MasterPageFile="~/MasterPage3.master" AutoEventWireup="false"
    CodeFile="RptStudentAgeing.aspx.vb" Inherits="RptStudentAgeing" Title="Welcome To SAS" %>

<asp:content ID="Content2" contentplaceholderid="head" runat="server">
    <script src="../Scripts/functions.js" type="text/javascript"></script>
    <script src="../Scripts/popcalendar.js" type="text/javascript"></script>
    <link href="../style.css" rel="stylesheet" />
    <style type="text/css">
        .auto-style1 {
            height: 16px;
        }
    </style>
</asp:content>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <script language="javascript" type="text/javascript">
        //whole JS modified by Hafiz @ 08/11/2016 for the NEW RA

        function PopupCalendar(type)
        {
            if (type == "ibtnDatePanelType1")
            {
                popUpCalendar(document.getElementById("<%=ibtnDatePanelType1.ClientID%>"), document.getElementById("<%=txtDatePanelType1.ClientID%>"), 'dd/mm/yyyy')
            }
            else if (type == "ibtnDatePanelType2")
            {
                popUpCalendar(document.getElementById("<%=ibtnDatePanelType2.ClientID%>"), document.getElementById("<%=txtDatePanelType2.ClientID%>"), 'dd/mm/yyyy')
            }
        }

        function CheckToDate(type) {
            var digits = "0123456789/";
            var temp, _type;

            if (type == "txtDatePanelType1")
            {
                _type = document.getElementById("<%=txtDatePanelType1.ClientID%>")
            }
            else if (type == "txtDatePanelType2")
            {
                _type = document.getElementById("<%=txtDatePanelType2.ClientID%>")
            }

            for (var i = 0; i < _type.value.length; i++)
            {
                temp = _type.value.substring(i, i + 1);
                if (digits.indexOf(temp) == -1)
                {
                    alert("Enter Valid Date (dd/mm/yyyy)");
                    _type.value = "";
                    _type.focus();
                    return false;
                }
            }
            return true;
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
            var SelectedValue, ChekedRB;

            //Check Type Of Record 
            if (document.getElementById("<%=ddlReportType.ClientID%>").value != "-1") {
                if (document.getElementById("<%=ddlReportType.ClientID%>").value == "1") {
                    SelectedValue = document.getElementById("<%=txtDatePanelType1.ClientID%>")
                }
                else if (document.getElementById("<%=ddlReportType.ClientID%>").value == "2") {
                    SelectedValue = document.getElementById("<%=txtDatePanelType2.ClientID%>")
                }
                else if (document.getElementById("<%=ddlReportType.ClientID%>").value == "3") {
                    //need to create new function 
                }
            }
            else
            {
                alert("Choose Type of Report To Proceed")
                return false;
            }

            //Check Radio-button Ageing By
            if ((document.getElementById("<%=rbYearly.ClientID%>").checked == false) && (document.getElementById("<%=rbVariousMonths.ClientID%>").checked == false) &&
                (document.getElementById("<%=rbQuaterly.ClientID%>").checked == false) && (document.getElementById("<%=rbMonthly.ClientID%>").checked == false)) {
                alert("Choose Ageing By To Proceed")
                return false;
            }
            else
            {
                if (document.getElementById("<%=rbYearly.ClientID%>").checked == true) {
                    ChekedRB = document.getElementById("<%=rbYearly.ClientID%>").value;
                }
                else if (document.getElementById("<%=rbVariousMonths.ClientID%>").checked == true) {
                    ChekedRB = document.getElementById("<%=rbVariousMonths.ClientID%>").value;
                }
                else if (document.getElementById("<%=rbQuaterly.ClientID%>").checked == true) {
                    ChekedRB = document.getElementById("<%=rbQuaterly.ClientID%>").value;
                }
                else if (document.getElementById("<%=rbMonthly.ClientID%>").checked == true) {
                    ChekedRB = document.getElementById("<%=rbMonthly.ClientID%>").value;
                }
            }

            //Check Date Fields
            var todate = new Date(SelectedValue.value)
            var currentdate = new Date(curenttempdate)

            if (SelectedValue.value.length == 0) {
                alert("Date Field Cannot Be Blank")
                return false;
            }

            //Checking correct date format
            var len = SelectedValue.value
            var RegExPattern = /^((((0?[1-9]|[12]\d|3[01])[\.\-\/](0?[13578]|1[02])[\.\-\/]((1[6-9]|[2-9]\d)?\d{2}))|((0?[1-9]|[12]\d|30)[\.\-\/](0?[13456789]|1[012])[\.\-\/]((1[6-9]|[2-9]\d)?\d{2}))|((0?[1-9]|1\d|2[0-8])[\.\-\/]0?2[\.\-\/]((1[6-9]|[2-9]\d)?\d{2}))|(29[\.\-\/]0?2[\.\-\/]((1[6-9]|[2-9]\d)?(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00)|00)))|(((0[1-9]|[12]\d|3[01])(0[13578]|1[02])((1[6-9]|[2-9]\d)?\d{2}))|((0[1-9]|[12]\d|30)(0[13456789]|1[012])((1[6-9]|[2-9]\d)?\d{2}))|((0[1-9]|1\d|2[0-8])02((1[6-9]|[2-9]\d)?\d{2}))|(2902((1[6-9]|[2-9]\d)?(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00)|00))))$/;
            var errorMessage = 'Enter Valid Date (dd/mm/yyyy)';

            if (SelectedValue.value.match(RegExPattern)) {
                if (len.length == 8) {
                    alert(errorMessage);
                    SelectedValue.value = "";
                    SelectedValue.focus();
                    return false;
                }
            }
            else {
                alert(errorMessage);
                SelectedValue.value = "";
                SelectedValue.focus();
                return false;
            }

            //Checking date between from and to
            var str2 = SelectedValue.value;

            var dt2 = parseInt(str2.substring(0, 2), 10);
            var mon2 = parseInt(str2.substring(3, 5), 10);
            var yr2 = parseInt(str2.substring(6, 10), 10);
            var date2 = new Date(yr2, mon2, dt2);

            if (SelectedValue.value.length != 10) {
                alert("Enter Valid Date (dd/mm/yyyy)")
                SelectedValue.focus();
                return false;
            }

            dllValues1(SelectedValue, ChekedRB);

        }

        function dllValues1(SelectedValue, ChekedRB) {

            if (document.getElementById("<%=ddlReportType.ClientID%>").value == "1")
            {
                var Info = "", FeeType = "";

                if ((document.getElementById("<%=ddlFeeType.ClientID%>").value == "-1") && (document.getElementById("<%=cbMatricId.ClientID%>").checked == false)
                    && (document.getElementById("<%=cbProgram.ClientID%>").checked == false))
                {
                    if (confirm("Do You Want To Show All Records?"))
                    {
                        OpenPopup('../GroupReport/RptStudentAgeingViewer.aspx?Report=1&FeeType=' + FeeType + '&Info=' + Info + '&ByDate=' + SelectedValue.value +
                        '&AgeingBy=' + ChekedRB, 'Ageing Report Based On Student Matric ID', '700', '650');

                        return true;
                    }
                    else { return false; }
                }
                else
                {
                    //Check Fee Type
                    FeeType = document.getElementById("<%=ddlFeeType.ClientID%>").value;
                    if (FeeType == "-1")
                    {
                        FeeType = ""
                    }

                    //Check Checkbox Information to Display 
                    if (document.getElementById("<%=cbMatricId.ClientID%>").checked == true) {
                        Info += 'matricid;';
                    }
                    if (document.getElementById("<%=cbProgram.ClientID%>").checked == true) {
                        Info += 'program;';
                    }

                    OpenPopup('../GroupReport/RptStudentAgeingViewer.aspx?Report=1&FeeType=' + FeeType + '&Info=' + Info + '&ByDate=' + SelectedValue.value +
                        '&AgeingBy=' + ChekedRB, 'Ageing Report Based On Student Matric ID', '700', '650');

                    return true;
                }
            }
            else if (document.getElementById("<%=ddlReportType.ClientID%>").value == "2")
            {

            }
            else if (document.getElementById("<%=ddlReportType.ClientID%>").value == "3")
            {

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
                            <td style="width: 3%; height: 14px">
                                <asp:ImageButton ID="ibtnCancel" runat="server" OnClick="ibtnCancel_Click" ImageUrl="~/images/cancel.png" ToolTip="Cancel" />
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
                    <asp:SiteMapPath ID="SiteMapPath1" runat="server" Width="346px">
                    </asp:SiteMapPath>
                </td>
                <td align="right" class="pagetext" style="height: 39px">
                    <asp:Label ID="lblMenuName" runat="server" Text="Student Ageing Report" Width="400px"></asp:Label>
                </td>
            </tr>
        </table>
    </asp:Panel>

    <table width="100%">
        <tr>
            <td width="1%"></td>
            <td width="98%">
                <fieldset style="width: 100%; border: thin solid #A6D9F4;">
                    <legend><strong><span style="color: #000000;">Selection Criteria</span></strong></legend>
                    <table style="width: 100%">
                        <tr>
                            <td style="height: 16px">
                                <asp:Label ID="lblReportType" runat="server" Text="Type of Report"></asp:Label>
                            </td>
                             <%-- modified by Hafiz @ 03/3/2016 --%>
                            <td class="auto-style1">
                                <asp:DropDownList ID="ddlReportType" runat="server" Width="400px" AutoPostBack="true" OnSelectedIndexChanged="ddlReportType_SelectedIndexChanged" >
                                    <asp:ListItem Value="-1">-- Please Select --</asp:ListItem>
                                    <asp:ListItem Value="1">Ageing Report Based On Student Matric ID</asp:ListItem>
                                    <asp:ListItem Value="2">Details Ageing Report</asp:ListItem>
                                    <asp:ListItem Value="3">Ageing Report Based On KPT Requirement</asp:ListItem>
                                </asp:DropDownList>
                            </td>
                        </tr>

                        <!-- Panel Ageing Report Based On Student Matric ID - START -->
                        <asp:Panel ID="PanelType1" runat="server" Visible="false" >
                            <tr>
                                <td style="height: 16px">
                                    <asp:Label ID="lblFeeType" runat="server" Text="Fee Type"></asp:Label>
                                </td>
                                <td class="auto-style1">
                                    <asp:DropDownList ID="ddlFeeType" runat="server" Width="400px">
                                    </asp:DropDownList>
                                </td>
                            </tr>

                            <tr>
                                <td style="height: 16px">
                                    <asp:Label ID="lblDisplayInfo" runat="server" Text="Information to display"></asp:Label>
                                </td>
                                <td class="auto-style1">
                                    <table>
                                        <tr>
                                            <td>
                                                 <asp:CheckBox ID="cbMatricId" runat="server" Text="Student Matric ID" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:CheckBox ID="cbProgram" runat="server" Text="Program" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>

                            <tr>
                                <td style="height: 16px">
                                    <asp:Label ID="lblDatePanelType1" runat="server" Text="By Date"></asp:Label>
                                </td>
                                <td>
                                    <table>
                                        <tr>
                                            <td style="width: 9px; height: 16px">
                                                <asp:Label ID="Label4" runat="server" Text="To"></asp:Label>
                                            </td>
                                            <td style="width: 63px; height: 16px">
                                                <asp:TextBox ID="txtDatePanelType1" runat="server"></asp:TextBox>
                                            </td>
                                            <td style="width: 50px; height: 16px">
                                                <asp:Image ID="ibtnDatePanelType1" runat="server" ImageUrl="~/images/cal.gif" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>

                        </asp:Panel>
                        <!-- Panel Ageing Report Based On Student Matric ID - END -->

                        <!-- Panel Details Ageing Report - START -->
                        <asp:Panel ID="PanelType2" runat="server" Visible="false" >
                            <tr>
                               <td>
                                    <asp:Label ID="lblStatus2" runat="server" Text="Status"></asp:Label>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlStatus2" runat="server" Width="150px" AutoPostBack="true">
                                    </asp:DropDownList>
                                </td>
                            </tr>

                            <tr>
                                <td>
                                    <asp:Label ID="lblNationality" runat="server" Text="Nationality"></asp:Label>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlNationality" runat="server" Width="150px" AutoPostBack="true">
                                        <asp:ListItem Value="-1">All</asp:ListItem>
                                        <asp:ListItem Value="1">Local</asp:ListItem>
                                        <asp:ListItem Value="0">Foreigner</asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                            </tr>

                            <tr>
                                <td style="height: 16px">
                                    <asp:Label ID="Label9" runat="server" Text="By Faculty"></asp:Label>
                                </td>
                                <td class="auto-style1">
                                    <asp:DropDownList ID="ddlFaculty" runat="server" Width="400px" AutoPostBack="true">
                                    </asp:DropDownList>
                                </td>
                            </tr>

                            <tr>
                                <td style="height: 16px">
                                    <asp:Label ID="lblSponsor" runat="server" Text="Sponsor"></asp:Label>
                                </td>
                                <td class="auto-style1">
                                    <asp:DropDownList ID="ddlSponsor" runat="server" Width="400px" AutoPostBack="true">
                                    </asp:DropDownList>
                                </td>
                            </tr>

                            <tr>
                                <td style="height: 16px">
                                    <asp:Label ID="lblDatePanelType2" runat="server" Text="By Date"></asp:Label>
                                </td>
                                <td>
                                    <table>
                                        <tr>
                                            <td style="width: 9px; height: 16px">
                                                <asp:Label ID="Label7" runat="server" Text="To"></asp:Label>
                                            </td>
                                            <td style="width: 63px; height: 16px">
                                                <asp:TextBox ID="txtDatePanelType2" runat="server"></asp:TextBox>
                                            </td>
                                            <td style="width: 50px; height: 16px">
                                                <asp:Image ID="ibtnDatePanelType2" runat="server" ImageUrl="~/images/cal.gif" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>

                        </asp:Panel>
                        <!-- Panel Details Ageing Report - END -->


                        <!-- Panel Ageing Report Based On KPT Requirement - START -->
                        <asp:Panel ID="PanelType3" runat="server" Visible="false" >

                             <tr>
                                <td style="height: 16px">
                                    <asp:Label ID="Label1" runat="server" Text="Current Ageing Date"></asp:Label>
                                </td>
                                <td>
                                    <table>
                                        <tr>
                                            <td style="width: 80px; height: 16px">
                                                <asp:TextBox ID="txtCurrAgeingDt" runat="server"></asp:TextBox>
                                            </td>
                                            <td style="width: 50px; height: 16px">
                                                <asp:Image ID="ibtnCurrAgeingDt" runat="server" ImageUrl="~/images/cal.gif" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>

                             <tr>
                                <td style="height: 16px">
                                    <asp:Label ID="Label5" runat="server" Text="Last Ageing Date"></asp:Label>
                                </td>
                                <td>
                                    <table>
                                        <tr>
                                            <td style="width: 80px; height: 16px">
                                                <asp:TextBox ID="txtLastAgeingDt" runat="server"></asp:TextBox>
                                            </td>
                                            <td style="width: 50px; height: 16px">
                                                <asp:Image ID="ibtnLastAgeingDt" runat="server" ImageUrl="~/images/cal.gif" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>

                        </asp:Panel>
                        <!-- Panel Ageing Report Based On KPT Requirement - END -->

                        <tr>
                            <td style="width: 10%; height: 16px">
                                <asp:Label ID="lblAgeingBy" runat="server" Text="Ageing By"></asp:Label>
                            </td>
                            <td style="height: 16px">
                                <table width="60%">
                                    <tr>
                                        <td style="width: 10%; height: 16px">
                                            <asp:RadioButton ID="rbYearly" runat="server" AutoPostBack="True" GroupName="ageingby" Text="Yearly" />
                                        </td>
                                        <td style="width: 25%; height: 16px">
                                            <asp:RadioButton ID="rbVariousMonths" runat="server" AutoPostBack="True" GroupName="ageingby" Text="6 months, 12 months & 36 months" />
                                        </td>
                                         <td style="width: 10%; height: 16px">
                                            <asp:RadioButton ID="rbQuaterly" runat="server" AutoPostBack="True" GroupName="ageingby" Text="Quarterly" />
                                        </td>
                                        <td style="width: 10%; height: 16px">
                                            <asp:RadioButton ID="rbMonthly" runat="server" AutoPostBack="True" GroupName="ageingby" Text="Monthly" />
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
    </table>

</asp:Content>
