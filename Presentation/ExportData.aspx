<%@ Page Language="VB" MasterPageFile="~/MasterPage3.master" AutoEventWireup="false" MaintainScrollPositionOnPostback="true"
    CodeFile="ExportData.aspx.vb" Inherits="ExportData" Title="ExportData" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <script language="javascript" type="Scripts/popcalendar.js"></script>
    <script language="javascript" type="Scripts/functions.js"></script>
    <script language="javascript" type="text/javascript">
        function getFromDate() {
            popUpCalendar(document.getElementById("<%=ibtnFromDate.ClientID%>"), document.getElementById("<%=txtFromDate.ClientID%>"), 'dd/mm/yyyy')

        }
        function getToDate() {
            popUpCalendar(document.getElementById("<%=ibtnToDate.ClientID%>"), document.getElementById("<%=txtToDate.ClientID%>"), 'dd/mm/yyyy')

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
        function showDialog() {
            document.execCommand('SaveAs', '1', '*.csv');
        }
        function CheckFromDate() {
            var digits = "0123456789/";
            var temp;
            for (var i = 0; i < document.getElementById("<%=txtFromDate.ClientID %>").value.length; i++) {
                temp = document.getElementById("<%=txtFromDate.ClientID%>").value.substring(i, i + 1);
                if (digits.indexOf(temp) == -1) {
                    alert("Enter Valid Date (dd/mm/yyyy)");
                    document.getElementById("<%=txtFromDate.ClientID%>").value = "";
                    document.getElementById("<%=txtFromDate.ClientID%>").focus();
                    return false;
                }
            }
            return true;
        }
        function CheckTdate() {
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
        function getconfirm() {

            if (document.getElementById("<%=interfaceid.ClientID%>").value == "") {
                alert("Select a Record");
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
        function Validate() {
            var re = /\s*((\S+\s*)*)/;

            if (document.getElementById("<%=txtFilePath.ClientID%>").value.replace(re, "$1").length == 0) {
                alert("File Path Field Cannot Be Blank");
                document.getElementById("<%=txtFilePath.ClientID%>").focus();
                return false;
            }

            if (document.getElementById("<%=txtFileName.ClientID%>").value.replace(re, "$1").length == 0) {
                alert("File Name Field Cannot Be Blank");
                document.getElementById("<%=txtFileName.ClientID%>").focus();
                return false;
            }
            var digits = "ABCDEFGHIJKLMNOPQRSTUVWXYZ abcdefghijklmnopqrstuvwxyz0123456789.";
            var temp;
            for (var i = 0; i < document.getElementById("<%=txtFileName.ClientID %>").value.length; i++) {
                temp = document.getElementById("<%=txtFileName.ClientID%>").value.substring(i, i + 1);
                if (digits.indexOf(temp) == -1) {
                    alert("Enter Valid File Name");
                    document.getElementById("<%=txtFileName.ClientID%>").focus();
                    return false;
                }
            }

            if (document.getElementById("<%=txtFromDate.ClientID%>").value.replace(re, "$1").length == 0) {
                alert("From Date Field Cannot Be Blank");
                document.getElementById("<%=txtFromDate.ClientID%>").focus();
                return false;
            }
            var len = document.getElementById("<%=txtFromDate.ClientID%>").value
            var RegExPattern = /^((((0?[1-9]|[12]\d|3[01])[\.\-\/](0?[13578]|1[02])[\.\-\/]((1[6-9]|[2-9]\d)?\d{2}))|((0?[1-9]|[12]\d|30)[\.\-\/](0?[13456789]|1[012])[\.\-\/]((1[6-9]|[2-9]\d)?\d{2}))|((0?[1-9]|1\d|2[0-8])[\.\-\/]0?2[\.\-\/]((1[6-9]|[2-9]\d)?\d{2}))|(29[\.\-\/]0?2[\.\-\/]((1[6-9]|[2-9]\d)?(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00)|00)))|(((0[1-9]|[12]\d|3[01])(0[13578]|1[02])((1[6-9]|[2-9]\d)?\d{2}))|((0[1-9]|[12]\d|30)(0[13456789]|1[012])((1[6-9]|[2-9]\d)?\d{2}))|((0[1-9]|1\d|2[0-8])02((1[6-9]|[2-9]\d)?\d{2}))|(2902((1[6-9]|[2-9]\d)?(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00)|00))))$/;
            var errorMessage = 'Enter Valid Date dd/mm/yyyy';

            if (document.getElementById("<%=txtFromDate.ClientID%>").value.match(RegExPattern)) {
                if (len.length == 8) {
                    alert(errorMessage);
                    document.getElementById("<%=txtFromDate.ClientID%>").value = "";
                    document.getElementById("<%=txtFromDate.ClientID%>").focus();
                    return false;
                }
            }
            else {
                alert(errorMessage);
                document.getElementById("<%=txtFromDate.ClientID%>").value = "";
                document.getElementById("<%=txtFromDate.ClientID%>").focus();
                return false;
            }



            if (document.getElementById("<%=txtToDate.ClientID%>").value == "") {
                alert("To Date Field Cannot Be Blank");
                document.getElementById("<%=txtToDate.ClientID%>").focus();
                return false;
            }


            var len = document.getElementById("<%=txtToDate.ClientID%>").value
            var RegExPattern = /^((((0?[1-9]|[12]\d|3[01])[\.\-\/](0?[13578]|1[02])[\.\-\/]((1[6-9]|[2-9]\d)?\d{2}))|((0?[1-9]|[12]\d|30)[\.\-\/](0?[13456789]|1[012])[\.\-\/]((1[6-9]|[2-9]\d)?\d{2}))|((0?[1-9]|1\d|2[0-8])[\.\-\/]0?2[\.\-\/]((1[6-9]|[2-9]\d)?\d{2}))|(29[\.\-\/]0?2[\.\-\/]((1[6-9]|[2-9]\d)?(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00)|00)))|(((0[1-9]|[12]\d|3[01])(0[13578]|1[02])((1[6-9]|[2-9]\d)?\d{2}))|((0[1-9]|[12]\d|30)(0[13456789]|1[012])((1[6-9]|[2-9]\d)?\d{2}))|((0[1-9]|1\d|2[0-8])02((1[6-9]|[2-9]\d)?\d{2}))|(2902((1[6-9]|[2-9]\d)?(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00)|00))))$/;
            var errorMessage = 'Enter Valid Date dd/mm/yyyy';

            if (document.getElementById("<%=txtToDate.ClientID%>").value.match(RegExPattern)) {
                if (len.length == 8) {
                    alert(errorMessage);
                    document.getElementById("<%=txtToDate.ClientID%>").value = "";
                    document.getElementById("<%=txtToDate.ClientID%>").focus();
                    return false;
                }
            }
            else {
                alert(errorMessage);
                document.getElementById("<%=txtToDate.ClientID%>").value = "";
                document.getElementById("<%=txtToDate.ClientID%>").focus();
                return false;
            }


            var str1 = document.getElementById("<%=txtFromDate.ClientID %>").value;
            var str2 = document.getElementById("<%=txtToDate.ClientID %>").value;
            var dt1 = parseInt(str1.substring(0, 2), 10);
            var mon1 = parseInt(str1.substring(3, 5), 10);
            var yr1 = parseInt(str1.substring(6, 10), 10);
            var dt2 = parseInt(str2.substring(0, 2), 10);
            var mon2 = parseInt(str2.substring(3, 5), 10);
            var yr2 = parseInt(str2.substring(6, 10), 10);
            var date1 = new Date(yr1, mon1, dt1);
            var date2 = new Date(yr2, mon2, dt2);




            if (date2 < date1) {
                alert("To Date Cannot be Lesser than From Date");
                document.getElementById("<%=txtToDate.ClientID%>").value = "";
                document.getElementById("<%=txtToDate.ClientID%>").focus();
                //"");
                return false;
            }

            return true;
        }
    </script>
    <%-- <atlas:ScriptManager ID="scriptmanager1" runat="Server" EnablePartialRendering="True">
    </atlas:ScriptManager>
    <atlas:UpdatePanel ID="up1" runat="server">
        <ContentTemplate>--%>
    <asp:Panel ID="Panel1" runat="server" Width="100%" meta:resourcekey="Panel1Resource1">
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
                                <asp:ImageButton ID="ibtnNew" runat="server" ImageUrl="~/images/add.png" ToolTip="New"
                                    meta:resourcekey="ibtnNewResource1" OnClick="ibtnNew_Click" />
                            </td>
                            <td style="width: 3%; height: 14px">
                                <asp:Label ID="Label11" runat="server" Text="New" meta:resourcekey="Label11Resource1"></asp:Label>
                            </td>
                        </tr>
                    </table>
                </td>
                <td>
                    <table style="border-collapse: collapse;" class="menuoff" onmouseover="className='menuon';"
                        onmouseout="className='menuoff';">
                        <tr>
                            <td style="width: 3%; height: 14px">
                                <asp:ImageButton ID="ibtnSave" runat="server" ImageUrl="~/images/save.png" ToolTip="Save"
                                    OnClick="ibtnSave_Click" meta:resourcekey="ibtnSaveResource1" />
                            </td>
                            <td style="width: 3%; height: 14px">
                                <asp:Label ID="Label14" runat="server" Text="Save" meta:resourcekey="Label14Resource1"></asp:Label>
                            </td>
                        </tr>
                    </table>
                </td>
                <td>
                    <table style="border-collapse: collapse;" class="menuoff" onmouseover="className='menuon';"
                        onmouseout="className='menuoff';">
                        <tr>
                            <td style="width: 3%; height: 14px">
                                <asp:ImageButton ID="ibtnDelete" runat="server" ImageUrl="~/images/delete.png" meta:resourcekey="ibtnDeleteResource1"
                                    OnClick="ibtnDelete_Click" />
                            </td>
                            <td style="width: 3%; height: 14px">
                                <asp:Label ID="Label13" runat="server" Text="Delete" meta:resourcekey="Label13Resource1"></asp:Label>
                            </td>
                        </tr>
                    </table>
                </td>
                <td>
                    <table style="border-collapse: collapse;" class="menuoff" onmouseover="className='menuon';"
                        onmouseout="className='menuoff';">
                        <tr>
                            <td style="width: 3%; height: 14px">
                                <asp:ImageButton ID="ibtnView" runat="server" ImageUrl="~/images/find.png" ToolTip="View"
                                    meta:resourcekey="ibtnViewResource1"  />
                            </td>
                            <td style="width: 3%; height: 14px">
                                <asp:Label ID="Label16" runat="server" Text="Search" meta:resourcekey="Label16Resource1"></asp:Label>
                            </td>
                        </tr>
                    </table>
                </td>
                <td>
                    <table style="border-collapse: collapse;" class="menuoff" onmouseover="className='menuon';"
                        onmouseout="className='menuoff';">
                        <tr>
                            <td style="width: 3%; height: 14px">
                                <asp:ImageButton ID="ibtnPrint" runat="server" ImageUrl="~/images/print.png" ToolTip="Print"
                                    meta:resourcekey="ibtnPrintResource1" />
                            </td>
                            <td style="width: 3%; height: 14px">
                                <asp:Label ID="Label17" runat="server" Text="Print" meta:resourcekey="Label17Resource1"></asp:Label>
                            </td>
                        </tr>
                    </table>
                </td>
                <td>
                    <table style="border-collapse: collapse;" class="menuoff" onmouseover="className='menuon';"
                        onmouseout="className='menuoff';">
                        <tr>
                            <td style="width: 3%; height: 14px">
                                <asp:ImageButton ID="ibtnPosting" runat="server" ImageUrl="~/images/Export.png"
                                    ToolTip="Export" meta:resourcekey="ibtnPostingResource1" />
                            </td>
                            <td style="width: 3%; height: 14px">
                                <asp:Label ID="Label6" runat="server" Text="Export" ></asp:Label>
                            </td>
                        </tr>
                    </table>
                </td>
                <td>
                    <table style="border-collapse: collapse;" class="menuoff" onmouseover="className='menuon';"
                        onmouseout="className='menuoff';">
                        <tr>
                            <td style="width: 3%; height: 14px">
                                <asp:ImageButton ID="ibtnOthers" runat="server" ImageUrl="~/images/gothers.png" ToolTip="Cancel"
                                    meta:resourcekey="ibtnOthersResource1" />
                            </td>
                            <td style="width: 3%; height: 14px">
                                <asp:Label ID="Label5" runat="server" Text="Others" meta:resourcekey="Label5Resource1"></asp:Label>
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
                                    meta:resourcekey="ibtnCancelResource1" OnClick="ibtnCancel_Click" />
                            </td>
                            <td style="width: 3%; height: 14px">
                                <asp:Label ID="Label18" runat="server" Text="Cancel" meta:resourcekey="Label18Resource1"></asp:Label>
                            </td>
                        </tr>
                    </table>
                </td>
                <td style="width: 2%; height: 14px">
                    <asp:ImageButton ID="ibtnFirst" runat="server" ImageUrl="~/images/new_last.png" meta:resourcekey="ibtnFirstResource1"
                        OnClick="ibtnFirst_Click" />
                </td>
                <td style="width: 2%; height: 14px">
                    <asp:ImageButton ID="ibtnPrevs" runat="server" ImageUrl="~/images/new_prev.png" meta:resourcekey="ibtnPrevsResource1"
                        OnClick="ibtnPrevs_Click" />
                </td>
                <td style="width: 2%; height: 14px">
                    <asp:TextBox ID="txtRecNo" runat="server" AutoPostBack="True" ReadOnly="true" CssClass="text_box"
                        MaxLength="7" Style="text-align: right" Width="52px" meta:resourcekey="txtRecNoResource1"
                        disabled="disabled" TabIndex="1" dir="ltr"></asp:TextBox>
                </td>
                <td style="width: 2%; height: 14px">
                    <asp:Label ID="Label47" runat="server" meta:resourcekey="Label47Resource1">Of</asp:Label>
                </td>
                <td style="width: 2%; height: 14px">
                    <asp:Label ID="lblCount" runat="server" Width="41px" meta:resourcekey="lblCountResource1"></asp:Label>
                </td>
                <td style="width: 2%; height: 14px">
                    <asp:ImageButton ID="ibtnNext" runat="server" ImageUrl="~/images/new_next.png" meta:resourcekey="ibtnNextResource1"
                        OnClick="ibtnNext_Click" />
                </td>
                <td style="width: 2%; height: 14px">
                    <asp:ImageButton ID="ibtnLast" runat="server" ImageUrl="~/images/new_first.png" meta:resourcekey="ibtnLastResource1"
                        OnClick="ibtnLast_Click" />
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
                <td class="vline" style="width: 100%; height: 1px">
                </td>
            </tr>
        </table>
        <table style="width: 100%">
            <tr>
                <td style="width: 400px">
                    <asp:SiteMapPath ID="SiteMapPath1" runat="server" meta:resourcekey="SiteMapPath1Resource1">
                    </asp:SiteMapPath>
                </td>
                <td class="pagetext" style="text-align: right">
                    <asp:Label ID="lblMenuName" runat="server" Width="422px" meta:resourcekey="lblMenuNameResource1"></asp:Label>
                </td>
            </tr>
        </table>
        <table>
            <tr>
                <td style="width: 100px">
                    <asp:Label ID="lblMsg" runat="server" CssClass="lblError" Style="text-align: center"
                        Width="301px" meta:resourcekey="lblMsgResource1"></asp:Label>
                </td>
            </tr>
        </table>
    </asp:Panel>
    <%--  </ContentTemplate>
    </atlas:UpdatePanel>
    <atlas:UpdateProgress ID="ProgressIndicator" runat="server">
        <ProgressTemplate>
            Loading the data, please wait...
            <asp:Image ID="LoadingImage" runat="server" ImageAlign="AbsMiddle" ImageUrl="~/Images/spinner.gif" meta:resourcekey="LoadingImageResource1" />
        </ProgressTemplate>
    </atlas:UpdateProgress>
    <atlas:UpdatePanel ID="up2" runat="server">
        <ContentTemplate>
            &nbsp;--%>
    <div style="border: thin solid #A6D9F4; width: 100%">
        <br />
        <table style="width: 61%">
            <tr>
                <td style="width: 85px; height: 16px">
                    <asp:CheckBox ID="ChkDateRange" runat="server" Text="Date Range" Width="77px" AutoPostBack="True"
                        OnCheckedChanged="ChkDateRange_CheckedChanged" meta:resourcekey="ChkDateRangeResource1" />
                </td>
                <td style="width: 15px; height: 16px">
                    &nbsp;
                </td>
                <td style="width: 30px; height: 16px; text-align: right;">
                    <asp:Label ID="Label7" runat="server" ForeColor="ControlText" Text="From" meta:resourcekey="Label7Resource1"></asp:Label>
                </td>
                <td colspan="2" style="height: 16px; width: 194px;">
                    <asp:TextBox ID="txtFromDate" runat="server" MaxLength="10" Style="text-align: left"
                        Width="132px" meta:resourcekey="txtFromDateResource1"></asp:TextBox>
                </td>
                <td style="width: 10px; height: 16px">
                    <asp:Image ID="ibtnFromDate" runat="server" ImageUrl="~/images/cal.gif" meta:resourcekey="ibtnFromDateResource1" />
                </td>
                <td style="width: 10px; height: 16px">
                </td>
                <td style="width: 10px; height: 16px">
                </td>
                <td style="width: 10px; height: 16px">
                </td>
                <td style="width: 10px; height: 16px">
                </td>
                <td style="width: 10px; height: 16px">
                </td>
                <td style="width: 10px; height: 16px">
                </td>
                <td style="width: 10px; height: 16px">
                </td>
                <td style="width: 10px; height: 16px; text-align: right;">
                    <asp:Label ID="Label12" runat="server" Text="To" meta:resourcekey="Label12Resource1"></asp:Label>
                </td>
                <td style="width: 57px; height: 16px">
                    <asp:TextBox ID="txtToDate" runat="server" MaxLength="10" Width="128px" meta:resourcekey="txtToDateResource1"></asp:TextBox>
                </td>
                <td style="width: 57px; height: 16px">
                </td>
                <td style="width: 648px; height: 16px">
                    <asp:Image ID="ibtnToDate" runat="server" ImageUrl="~/images/cal.gif" meta:resourcekey="ibtnToDateResource1" />
                </td>
                <td style="width: 1px;" rowspan="4">
                    <asp:ImageButton ID="ibtnStatus" runat="server" Enabled="False" ImageUrl="~/images/NotReady.gif"
                        CssClass="cursor" />
                </td>
                <td style="width: 1px; height: 16px">
                </td>
                <td style="width: 1px; height: 16px">
                </td>
                <td style="width: 1px; height: 16px">
                </td>
            </tr>
            <tr>
                <td colspan="2" style="height: 16px">
                    <asp:Label ID="Label8" runat="server" Text="FileFormat"></asp:Label>
                </td>
                <td colspan="1" style="width: 30px; height: 16px">
                </td>
                <td colspan="2" style="height: 16px; width: 194px;">
                    <span style="color: #ff0066"></span><span style="color: #ff0066; text-align: left;">
                        <asp:DropDownList ID="ddlFileFormat" runat="server" Width="138px">
                            <asp:ListItem Value=",">Comma</asp:ListItem>
                            <asp:ListItem Value=";">Semicolon</asp:ListItem>
                            <asp:ListItem Value="|">Pipe</asp:ListItem>
                        </asp:DropDownList>
                        *</span>
                </td>
                <td style="width: 10px; height: 16px">
                </td>
                <td style="width: 10px; height: 16px">
                </td>
                <td style="width: 10px; height: 16px">
                </td>
                <td style="width: 10px; height: 16px">
                </td>
                <td style="width: 10px; height: 16px">
                </td>
                <td style="width: 10px; height: 16px">
                </td>
                <td style="width: 10px; height: 16px">
                </td>
                <td style="width: 10px; height: 16px">
                </td>
                <td style="width: 10px; height: 16px">
                    <asp:Label ID="Label9" runat="server" Text="Interface"></asp:Label>
                </td>
                <td style="width: 57px; height: 16px">
                    <asp:DropDownList ID="ddlinterface" runat="server" Width="135px">
                        <asp:ListItem Value="All">--All--</asp:ListItem>
                        <asp:ListItem>GL Journal</asp:ListItem>
                        <asp:ListItem>CB Payment</asp:ListItem>
                        <asp:ListItem>CB Receipt</asp:ListItem>
                    </asp:DropDownList>
                    <span style="color: #ff0066">*</span>
                </td>
                <td style="width: 57px; height: 16px">
                </td>
                <td style="width: 57px; height: 16px">
                </td>
                <td style="width: 57px; height: 16px">
                </td>
                <td style="width: 57px; height: 16px">
                </td>
            </tr>
            <tr>
                <td colspan="2" style="height: 16px">
                    <asp:Label ID="Label10" runat="server" Text="Frequency"></asp:Label>
                </td>
                <td colspan="1" style="width: 30px; height: 16px">
                </td>
                <td colspan="2" style="height: 16px; width: 194px;">
                    <span style="color: #ff0066"></span><span style="color: #ff0066; text-align: left;">
                        <asp:DropDownList ID="ddlFrequency" runat="server" Width="139px">
                            <asp:ListItem>daily</asp:ListItem>
                            <asp:ListItem>weekly</asp:ListItem>
                            <asp:ListItem>monthly</asp:ListItem>
                        </asp:DropDownList>
                        *</span>
                </td>
                <td style="width: 10px; height: 16px">
                </td>
                <td style="width: 10px; height: 16px">
                </td>
                <td style="width: 10px; height: 16px">
                </td>
                <td style="width: 10px; height: 16px">
                </td>
                <td style="width: 10px; height: 16px">
                </td>
                <td style="width: 10px; height: 16px">
                </td>
                <td style="width: 10px; height: 16px">
                </td>
                <td style="width: 10px; height: 16px">
                </td>
                <td style="width: 10px; height: 16px">
                    <asp:Label ID="Label15" runat="server" Text="Time"></asp:Label>
                </td>
                <td colspan="3" style="width: 195px; height: 17px">
                    <asp:DropDownList ID="ddlhour" runat="server" Width="43px">
                        <asp:ListItem>00</asp:ListItem>
                        <asp:ListItem>01</asp:ListItem>
                        <asp:ListItem>02</asp:ListItem>
                        <asp:ListItem>03</asp:ListItem>
                        <asp:ListItem>04</asp:ListItem>
                        <asp:ListItem>05</asp:ListItem>
                        <asp:ListItem>06</asp:ListItem>
                        <asp:ListItem>07</asp:ListItem>
                        <asp:ListItem>08</asp:ListItem>
                        <asp:ListItem>09</asp:ListItem>
                        <asp:ListItem>10</asp:ListItem>
                        <asp:ListItem>11</asp:ListItem>
                        <asp:ListItem>12</asp:ListItem>
                        <asp:ListItem>13</asp:ListItem>
                        <asp:ListItem>14</asp:ListItem>
                        <asp:ListItem>15</asp:ListItem>
                        <asp:ListItem>16</asp:ListItem>
                        <asp:ListItem>17</asp:ListItem>
                        <asp:ListItem>18</asp:ListItem>
                        <asp:ListItem>19</asp:ListItem>
                        <asp:ListItem>20</asp:ListItem>
                        <asp:ListItem>21</asp:ListItem>
                        <asp:ListItem>22</asp:ListItem>
                        <asp:ListItem>23</asp:ListItem>
                    </asp:DropDownList>
                    <asp:DropDownList ID="ddlMin" runat="server" Width="38px">
                        <asp:ListItem>00</asp:ListItem>
                        <asp:ListItem>01</asp:ListItem>
                        <asp:ListItem>02</asp:ListItem>
                        <asp:ListItem>03</asp:ListItem>
                        <asp:ListItem>04</asp:ListItem>
                        <asp:ListItem>05</asp:ListItem>
                        <asp:ListItem>06</asp:ListItem>
                        <asp:ListItem>07</asp:ListItem>
                        <asp:ListItem>08</asp:ListItem>
                        <asp:ListItem>09</asp:ListItem>
                        <asp:ListItem>10</asp:ListItem>
                        <asp:ListItem>11</asp:ListItem>
                        <asp:ListItem>12</asp:ListItem>
                        <asp:ListItem>13</asp:ListItem>
                        <asp:ListItem>14</asp:ListItem>
                        <asp:ListItem>15</asp:ListItem>
                        <asp:ListItem>16</asp:ListItem>
                        <asp:ListItem>17</asp:ListItem>
                        <asp:ListItem>18</asp:ListItem>
                        <asp:ListItem>19</asp:ListItem>
                        <asp:ListItem>20</asp:ListItem>
                        <asp:ListItem>21</asp:ListItem>
                        <asp:ListItem>22</asp:ListItem>
                        <asp:ListItem>23</asp:ListItem>
                        <asp:ListItem>24</asp:ListItem>
                        <asp:ListItem>25</asp:ListItem>
                        <asp:ListItem>26</asp:ListItem>
                        <asp:ListItem>27</asp:ListItem>
                        <asp:ListItem>28</asp:ListItem>
                        <asp:ListItem>29</asp:ListItem>
                        <asp:ListItem>30</asp:ListItem>
                        <asp:ListItem>31</asp:ListItem>
                        <asp:ListItem>32</asp:ListItem>
                        <asp:ListItem>33</asp:ListItem>
                        <asp:ListItem>34</asp:ListItem>
                        <asp:ListItem>35</asp:ListItem>
                        <asp:ListItem>36</asp:ListItem>
                        <asp:ListItem>37</asp:ListItem>
                        <asp:ListItem>38</asp:ListItem>
                        <asp:ListItem>39</asp:ListItem>
                        <asp:ListItem>40</asp:ListItem>
                        <asp:ListItem>41</asp:ListItem>
                        <asp:ListItem>42</asp:ListItem>
                        <asp:ListItem>43</asp:ListItem>
                        <asp:ListItem>44</asp:ListItem>
                        <asp:ListItem>45</asp:ListItem>
                        <asp:ListItem>46</asp:ListItem>
                        <asp:ListItem>47</asp:ListItem>
                        <asp:ListItem>48</asp:ListItem>
                        <asp:ListItem>49</asp:ListItem>
                        <asp:ListItem>50</asp:ListItem>
                        <asp:ListItem>51</asp:ListItem>
                        <asp:ListItem>52</asp:ListItem>
                        <asp:ListItem>53</asp:ListItem>
                        <asp:ListItem>54</asp:ListItem>
                        <asp:ListItem>55</asp:ListItem>
                        <asp:ListItem>56</asp:ListItem>
                        <asp:ListItem>57</asp:ListItem>
                        <asp:ListItem>58</asp:ListItem>
                        <asp:ListItem>59</asp:ListItem>
                    </asp:DropDownList>
                    <span style="color: #ff0066">*</span>
                </td>
                <td colspan="1" style="width: 195px; height: 17px">
                </td>
                <td colspan="1" style="width: 195px; height: 17px">
                </td>
                <td colspan="1" style="width: 195px; height: 17px">
                </td>
                <td colspan="1" style="width: 195px; height: 17px">
                </td>
                <td colspan="1" style="width: 195px; height: 17px">
                </td>
                <td colspan="1" style="width: 195px; height: 17px">
                </td>
                <td colspan="1" style="width: 195px; height: 17px">
                </td>
                <td colspan="1" style="width: 195px; height: 17px">
                </td>
                <td colspan="1" style="width: 195px; height: 17px">
                </td>
                <td colspan="1" style="width: 195px; height: 17px">
                </td>
                <td colspan="1" style="width: 195px; height: 17px">
                </td>
                <td colspan="1" style="width: 195px; height: 17px">
                </td>
                <td colspan="1" style="width: 195px; height: 17px">
                </td>
                <td colspan="1" style="width: 195px; height: 17px">
                </td>
                <td colspan="1" style="width: 195px; height: 17px">
                </td>
                <td colspan="1" style="width: 195px; height: 17px">
                </td>
                <td colspan="1" style="width: 195px; height: 17px">
                </td>
                <td colspan="1" style="width: 195px; height: 17px">
                </td>
                <td colspan="1" style="width: 195px; height: 17px">
                </td>
                <td colspan="1" style="width: 195px; height: 17px">
                </td>
                <td colspan="1" style="width: 195px; height: 17px">
                </td>
                <td colspan="1" style="width: 195px; height: 17px">
                </td>
                <td colspan="1" style="width: 195px; height: 17px">
                </td>
                <td colspan="1" style="width: 195px; height: 17px">
                </td>
                <td colspan="1" style="width: 195px; height: 17px">
                </td>
                <td colspan="1" style="width: 195px; height: 17px">
                </td>
                <td colspan="1" style="width: 195px; height: 17px">
                </td>
                <td colspan="1" style="width: 195px; height: 17px">
                </td>
            </tr>
            <tr>
                <td colspan="2" style="height: 16px">
                    <asp:Label ID="Label19" runat="server" Text="File Path"></asp:Label>
                </td>
                <td colspan="1" style="width: 30px; height: 16px">
                </td>
                <td colspan="2" style="width: 194px; height: 16px">
                    <asp:TextBox ID="txtFilePath" runat="server" Width="132px"></asp:TextBox><span style="color: #ff0066">*</span>
                </td>
                <td style="width: 10px; height: 16px">
                </td>
                <td style="width: 10px; height: 16px">
                </td>
                <td style="width: 10px; height: 16px">
                </td>
                <td style="width: 10px; height: 16px">
                </td>
                <td style="width: 10px; height: 16px">
                </td>
                <td style="width: 10px; height: 16px">
                </td>
                <td style="width: 10px; height: 16px">
                </td>
                <td style="width: 10px; height: 16px">
                </td>
                <td style="width: 10px; height: 16px">
                    <asp:Label ID="Label3" runat="server" Text="File Name" Width="51px" meta:resourcekey="Label3Resource1"></asp:Label>
                </td>
                <td colspan="3" style="width: 195px; height: 17px">
                    <asp:TextBox ID="txtFileName" runat="server" Width="127px" meta:resourcekey="TextBox1Resource1"
                        AutoCompleteType="Disabled"></asp:TextBox><span style="color: #ff0066">*</span>
                </td>
            </tr>
            </table>
        <table style="width: 100%">
            <tr>
                <td style="width: 100px">
                    <asp:CheckBox ID="CheckBox1" runat="server" Text="Include Previous Transaction Data"
                        Width="210px" AutoPostBack="True" meta:resourcekey="CheckBox1Resource1" />
                </td>
                <td style="width: 100px">
                </td>
            </tr>
        </table>
        &nbsp;<asp:HiddenField ID="lblStatus" runat="server" />
        <asp:HiddenField ID="interfaceid" runat="server" />
        &nbsp;&nbsp;
        <asp:Label ID="Label2" runat="server" Text="Payer" Width="33px" meta:resourcekey="Label2Resource1"
            Visible="False"></asp:Label>
        <asp:DropDownList ID="ddlPayer" runat="server" Width="137px" AutoPostBack="True"
            OnSelectedIndexChanged="ddlPayer_SelectedIndexChanged" meta:resourcekey="ddlPayerResource1"
            Visible="False">
            <asp:ListItem meta:resourcekey="ListItemResource1" Text="--All--"></asp:ListItem>
            <asp:ListItem meta:resourcekey="ListItemResource2" Text="Student"></asp:ListItem>
            <asp:ListItem meta:resourcekey="ListItemResource3" Text="Sponsor"></asp:ListItem>
        </asp:DropDownList>
        <asp:Label ID="Label1" runat="server" Text="Transaction Type" Width="95px" meta:resourcekey="Label1Resource1"
            Visible="False"></asp:Label>
        <asp:DropDownList ID="ddlTransactionType" runat="server" Width="105px" meta:resourcekey="ddlTransactionTypeResource1"
            Visible="False">
            <asp:ListItem Value="All" meta:resourcekey="ListItemResource4" Text="--All--"></asp:ListItem>
            <asp:ListItem meta:resourcekey="ListItemResource5" Text="Invoice"></asp:ListItem>
            <asp:ListItem meta:resourcekey="ListItemResource6" Text="Allocation"></asp:ListItem>
            <asp:ListItem meta:resourcekey="ListItemResource7" Text="Credit Note"></asp:ListItem>
            <asp:ListItem meta:resourcekey="ListItemResource8" Text="Debit Note"></asp:ListItem>
            <asp:ListItem meta:resourcekey="ListItemResource9" Text="Receipts"></asp:ListItem>
            <asp:ListItem meta:resourcekey="ListItemResource10" Text="Payments"></asp:ListItem>
        </asp:DropDownList>
        <asp:Label ID="Label4" runat="server" Text="File Location" meta:resourcekey="Label4Resource1"
            Visible="False"></asp:Label>
        <asp:DropDownList ID="DropDownList1" runat="server" Width="41px" meta:resourcekey="DropDownList1Resource1"
            Visible="False">
        </asp:DropDownList>
    </div>
    <%--  </ContentTemplate>
    </atlas:UpdatePanel>--%>
</asp:Content>
