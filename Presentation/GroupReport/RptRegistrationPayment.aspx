<%@ Page Language="VB" MasterPageFile="~/MasterPage3.master" AutoEventWireup="false" 
    CodeFile="RptRegistrationPayment.aspx.vb" Inherits="RptRegistrationPayment" Title="Welcome To SAS" %>

<asp:content ID="Content2" contentplaceholderid="head" runat="server">
    <script src="../Scripts/functions.js" type="text/javascript"></script>
    <script src="../Scripts/popcalendar.js" type="text/javascript"></script>
    <link href="../style.css" rel="stylesheet" />
</asp:content>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <script language="javascript" type="text/javascript">

        function getDate() {

            //Check ddlStuStatus
            if (document.getElementById("<%=ddlStuStatus.ClientID%>").selectedIndex == 0) {
                alert("Please select a status.")
                return false;
            }

            //Check ddlStuStatus
            if (document.getElementById("<%=ddlSemester.ClientID%>").selectedIndex == 0) {
                alert("Please select semester.")
                return false;
            }
            dllValues1()
        }

        function dllValues1() {
            var fromdate1 = "0"
            var todate1 = "0"
            var lbl = false
            if ((document.getElementById("<%=ddlStuStatus.ClientID%>").value == "-1") && (document.getElementById("<%=ddlFaculty.ClientID%>").value == "-1") && (document.getElementById("<%=ddlProgram.ClientID%>").value == "-1")) {
                if (confirm("Do You Want To Show All Records?")) {

                    lbl = true;
                    OpenPopup('../GroupReport/RptRegistrationPaymentViewer.aspx?tdate=' + todate1, 'Registration Payment Report', '700', '650')
                    //window.open('RptRegistrationQuery.aspx', 'SAS', 'width=700,height=500,resizable=1');
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
                OpenPopup('../GroupReport/RptRegistrationPaymentViewer.aspx', 'Registration Payment Report', '700', '650')
                //new_window = window.open('RptRegistrationQuery.aspx', 'SAS', 'width=700,height=500,resizable=1'); new_window.focus();
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

                <%--Commented by Zoya @26/02/2016--%>
                <%--<td>
                    <table style="border-collapse: collapse;" class="menuoff" onmouseover="className='menuon';"
                        onmouseout="className='menuoff';">
                         <tr>
                            <td style="width: 3%; height: 14px">
                                <div id="wrap">
                                    <ul id="navbar">
                                        <li><a href="#">
                                            <asp:Image id="ibtnPrint" ImageUrl="~/images/print.png" width="24" height="24" runat="server" />
                                             <asp:Label ID="Label4" runat="server" height="24" cssClass="lblPrintCss">Print</asp:Label>
                                            <img src="../images/down.png" width="16" height="16" border="0" align="middle" /></a>
                                            <ul>
                                                <li><a href="#">
                                                    <asp:ImageButton ID="ibtnBMReport" runat="server" ImageUrl="~/images/print-bm.png"/>
                                                    </a></li>
                                                <li><a href="#">
                                                    <asp:ImageButton ID="ibtnEnReport" runat="server" ImageUrl="~/images/print-eng.png"/>
                                                    </a></li>
                                            </ul>
                                        </li>
                                        <%-- <li><a href="#">
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
                                        </li>--%>
                                  <%-- </ul>
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
                    <asp:Label ID="lblMenuName" runat="server" Text="Registration Payment Report" Width="400px"></asp:Label>
                </td>
            </tr>
        </table>
    </asp:Panel>
    <table width="100%">
        <tr>
            <td>
                <asp:Label ID="lblMsg" runat="server" Text="" ForeColor="Red" Font-Bold="true"></asp:Label>
            </td>
        </tr>
    </table>
    <table width="100%">
        <tr>
            <td width="1%"></td>
            <td width="98%">
                <fieldset style="width: 100%; border: thin solid #A6D9F4;">
                    <legend><strong><span style="color: #000000;">Selection Criteria</span></strong></legend>
                    <table style="width: 100%">
                        <tr>
                            <td style="width: 10%; height: 16px">
                                <asp:Label ID="Label3" runat="server" Text="Status"></asp:Label>
                            </td>
                            <td style="height: 16px">
                                <asp:DropDownList ID="ddlStuStatus" runat="server" Width="135px" AutoPostBack="true">
                                    <asp:ListItem>-- Please Select --</asp:ListItem>
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
                                <asp:Label ID="Label1" runat="server" Text="Semester"></asp:Label>
                            </td>
                            <td style="height: 16px">
                                <asp:DropDownList ID="ddlSemester" runat="server" Width="150px" AutoPostBack="true">
                                </asp:DropDownList>
                            </td>
                        </tr>
                    </table>
                </fieldset>
            </td>
            <td style="width: 113px; height: 16px"></td>
        </tr>
    </table>

</asp:Content>
