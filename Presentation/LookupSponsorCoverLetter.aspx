<%@ Page Language="VB" AutoEventWireup="false" CodeFile="LookupSponsorCoverLetter.aspx.vb" MaintainScrollPositionOnPostback="true"
    Inherits="LookupSponsorCoverLetter" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <link href="style.css" rel="stylesheet" type="text/css" />
    <script src="Scripts/popcalendar.js" type="text/javascript"></script>
</head>
<body topmargin="0" leftmargin="0" alink="#00ff66">
    <form id="form1" runat="server">
        <script type="text/javascript" language="javascript">
            function getDateto() {
                popUpCalendar(document.getElementById("<%=ibtnTodate.ClientID%>"), document.getElementById("<%=txtToDate.ClientID%>"), 'dd/mm/yyyy')
            }
            function getibtnFDate() {
                popUpCalendar(document.getElementById("<%=ibtnFdate.ClientID%>"), document.getElementById("<%=txtFrom.ClientID%>"), 'dd/mm/yyyy')
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
            function validate() {
                var re = /\s*((\S+\s*)*)/;
                if (document.getElementById("<%=ddlCode.ClientID%>").selectedIndex == 0) {
                    alert("Code Field Cannot Be Blank");
                    document.getElementById("<%=ddlCode.ClientID%>").focus();
                    return false;
                }
                var digits = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
                var temp;

                if (document.getElementById("<%=txtTitle.ClientID%>").value.replace(re, "$1").length == 0) {
                    alert("Title Field Cannot Be Blank");
                    document.getElementById("<%=txtTitle.ClientID%>").focus();
                    return false;
                }
                if (document.getElementById("<%=txtOurRef.ClientID%>").value.replace(re, "$1").length == 0) {
                    alert("Our Ref Field Cannot Be Blank");
                    document.getElementById("<%=txtOurRef.ClientID%>").focus();
                    return false;
                }
                if (document.getElementById("<%=txtYourRef.ClientID%>").value.replace(re, "$1").length == 0) {
                    alert("Your Ref Field Cannot Be Blank");
                    document.getElementById("<%=txtYourRef.ClientID%>").focus();
                    return false;
                }
                if (document.getElementById("<%=txtMsg.ClientID%>").value.replace(re, "$1").length == 0) {
                    alert("Message Field Cannot Be Blank");
                    document.getElementById("<%=txtMsg.ClientID%>").focus();
                    return false;
                }
                //if (document.getElementById("<%=txtAddress.ClientID%>").value.replace(re, "$1").length == 0) {
                 //   alert("Address Field Cannot Be Blank");
                //    document.getElementById("<%=txtAddress.ClientID%>").focus();
                //    return false;
                //}
                if (document.getElementById("<%=txtSignBy.ClientID%>").value.replace(re, "$1").length == 0) {
                    alert("Sign By Field Cannot Be Blank");
                    document.getElementById("<%=txtSignBy.ClientID%>").focus();
                    return false;
                }
                if (document.getElementById("<%=txtName.ClientID%>").value.replace(re, "$1").length == 0) {
                    alert("Name Field Cannot Be Blank");
                    document.getElementById("<%=txtName.ClientID%>").focus();
                    return false;
                }
                //txtFrom---------------------------------------------------------------------------

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


                //txtTodate---------------------------------------------------------------------------
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

                return true;
            }
            function PreviewReport() {
                var new_window = window.open('GroupReport/RptSponsorCoverLetterViewer.aspx', 'SAS', 'width=700,height=500,resizable=1,scrollbars=1, top=100');
                new_window.focus();
                window.opener.document.getElementById('ctl00_ContentPlaceHolder1_btnHidden').click();
                self.close();
            }

        </script>
        <div style="text-align: left">
            <table style="width: 400px">
                <tr>
                    <td style="width: 100px; height: 16px"></td>
                    <td style="width: 100px; height: 16px">
                        <table style="width: 100%">
                            <tr>
                                <td colspan="2" class="auto-style2"></td>
                                <td colspan="2" style="height: 16px">
                                    <asp:Label ID="Label15" runat="server" ForeColor="Red" Visible="False" CssClass=" lblError"></asp:Label>
                                </td>
                                <td style="width: 1650px; height: 16px"></td>
                            </tr>
                            <tr>
                                <td colspan="2" style="height: 16px; width: 283px;">
                                    <asp:Label ID="Label36" runat="server" Text="Code"></asp:Label>
                                </td>
                                <td colspan="2" style="height: 16px">
                                    <asp:DropDownList ID="ddlCode" AppendDataBoundItems="true" runat="server" Width="128px"
                                        Height="20px" AutoPostBack="True">
                                    </asp:DropDownList>
                                </td>
                                <td style="width: 1650px; height: 16px"></td>
                            </tr>
                            <tr>
                                <td colspan="2" class="auto-style2">
                                    <span
                                        style="color: #ff0000">*</span>
                                    <asp:Label ID="Label27" runat="server" Text="Title"></asp:Label>
                                </td>
                                <td colspan="2" style="height: 16px">
                                    <asp:TextBox ID="txtTitle" runat="server" Width="519px" MaxLength="50"></asp:TextBox>
                                </td>
                                <td style="width: 1650px; height: 16px">
                                    <span style="color: #ff0000"></span>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2" class="auto-style2">
                                    <span style="color: #ff0000">*</span>
                                    <asp:Label ID="Label28" runat="server" Text="Our Ref"></asp:Label>
                                </td>
                                <td colspan="2" style="height: 16px">
                                    <asp:TextBox ID="txtOurRef" runat="server"></asp:TextBox>
                                </td>
                                <td style="width: 1650px; height: 16px"></td>
                            </tr>
                            <tr>
                                <td colspan="2" class="auto-style2">
                                    <span style="color: #ff0000">*</span>
                                    <asp:Label ID="Label29" runat="server" Text="Your Ref"></asp:Label>
                                </td>
                                <td colspan="2" style="height: 16px">
                                    <asp:TextBox ID="txtYourRef" runat="server"></asp:TextBox>
                                </td>
                                <td style="width: 1650px; height: 16px"></td>
                            </tr>
                            <tr style="display: none">
                                <td colspan="2" class="auto-style2">
                                    <span style="color: #ff0000">*</span>
                                    <asp:Label ID="Label30" runat="server" Text="Date"></asp:Label>
                                </td>
                                <td colspan="1" style="width: 71px; height: 16px">
                                    <asp:TextBox ID="txtToDate" runat="server"></asp:TextBox>
                                </td>
                                <td colspan="1" style="width: 444px; height: 16px">
                                    <asp:Image ID="ibtnTodate" runat="server" ImageUrl="~/images/cal.gif" /><span style="color: #ff0000">*</span>
                                </td>
                                <td colspan="2" style="height: 16px"></td>
                                <td colspan="1" style="height: 16px"></td>
                                <td style="width: 10px; height: 16px"></td>
                            </tr>
                            <tr style="display: none">
                                <td colspan="2" style="vertical-align: top;" class="auto-style4">
                                    <span style="color: #ff0000">*</span>
                                    <asp:Label ID="Label31" runat="server" Text="Address"></asp:Label>
                                </td>
                                <td colspan="2" style="height: 88px">
                                    <asp:TextBox ID="txtAddress" runat="server" EnableTheming="False" Height="108px"
                                        TextMode="MultiLine" Width="519px"></asp:TextBox>
                                </td>
                                <td style="width: 1650px; height: 88px">
                                    <span style="color: #ff0000"></span>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2" style="vertical-align: top;" class="auto-style4">
                                    <span style="color: #ff0000">*</span>
                                    <asp:Label ID="Label32" runat="server" Text="Message"></asp:Label>
                                </td>
                                <td colspan="2" style="height: 88px">
                                    <asp:TextBox ID="txtMsg" runat="server" EnableTheming="False" Height="108px"
                                        TextMode="MultiLine" Width="519px"></asp:TextBox>
                                </td>
                                <td style="width: 1650px; height: 88px">
                                    <span style="color: #ff0000"></span>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2" class="auto-style2">
                                    <span
                                        style="color: #ff0000">*</span>
                                    <asp:Label ID="Label33" runat="server" Text="Sign By"></asp:Label>
                                </td>
                                <td colspan="2" style="height: 16px">
                                    <asp:TextBox ID="txtSignBy" runat="server" Width="519px" MaxLength="50"></asp:TextBox>
                                </td>
                                <td style="width: 1650px; height: 16px">
                                    <span style="color: #ff0000"></span>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2" class="auto-style4" style="vertical-align: top;">
                                    <span style="color: #ff0000">*</span>
                                    <asp:Label ID="Label34" runat="server" Text="Name, Designation & Address"></asp:Label>
                                </td>
                                <td colspan="2" style="height: 88px">
                                    <asp:TextBox ID="txtName" runat="server" EnableTheming="false" Height="108px" TextMode="MultiLine" Width="519px" MaxLength="1000"></asp:TextBox>
                                </td>
                                <td style="width: 1650px; height: 88px">
                                    <span style="color: #ff0000"></span>
                                </td>
                            </tr>
                            <tr style="display: none">
                                <td colspan="2" class="auto-style2">
                                    <span style="color: #ff0000">*</span>
                                    <asp:Label ID="Label35" runat="server" Text="Date"></asp:Label>
                                </td>
                                <td colspan="1" style="height: 16px">
                                    <asp:TextBox ID="txtFrom" runat="server"></asp:TextBox>
                                </td>
                                <td colspan="2" style="height: 16px">
                                    <asp:Image ID="ibtnFDate" runat="server" ImageUrl="~/images/cal.gif" /><span style="color: #ff0000">*</span>
                                </td>
                                <td style="width: 1833px; height: 16px"></td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td style="width: 100px; height: 16px; text-align: center"></td>
                    <td style="width: 100px; height: 16px; text-align: center">
                        <table>
                            <tr>
                                <td style="width: 250px">
                                    <asp:RadioButtonList ID="rblSCLType" runat="server" RepeatDirection="Horizontal" RepeatLayout="Flow">
                                        <asp:ListItem Value="0" Selected="True">Batch</asp:ListItem>
                                        <asp:ListItem Value="1">Individual</asp:ListItem>
                                    </asp:RadioButtonList>
                                </td>
                                <td style="width: 150px">
                                    <asp:RadioButtonList ID="rblSCLLang" runat="server" RepeatDirection="Horizontal" RepeatLayout="table">
                                        <asp:ListItem Value="0" Selected="True">BM</asp:ListItem>
                                        <asp:ListItem Value="1">EN</asp:ListItem>
                                    </asp:RadioButtonList>
                                </td>

                              <%--  <td style="width: 8px; height: 30px; text-align: right">
                                    <asp:ImageButton ID="ibtnOK" runat="server" ImageUrl="~/images/add_list.gif" Width="24px"
                                        Height="24px" />
                                </td>
                                <td style="width: 49px; height: 30px; text-align: left">
                                    <asp:Label ID="Label8" runat="server" Text="Select" Width="21px"></asp:Label>
                                </td>
                                <td style="width: 34px; height: 30px; text-align: left">
                                    <asp:ImageButton ID="ibtnClose" runat="server" Height="24px" ImageUrl="~/images/ok_cancel.jpg"
                                        Width="24px" />
                                </td>
                                <td style="width: 158px; height: 30px; text-align: left">
                                    <asp:Label ID="Label10" runat="server" Text="Close" Width="21px"></asp:Label>
                                </td>
                                <td style="width: 142px; height: 30px; text-align: left"></td>
                                <td style="width: 100px; height: 30px; text-align: left"></td>--%>
                                <td>
                                    <table>
                                        <tr>
                                            <td>
                                                <asp:Button ID="btnPrintSCL" runat="server" Text="Print" />
                                            </td>
                                            <td>
                                                <asp:Button ID="btnEmailSCL" runat="server" Text="Email" />
                                            </td>
                                            <td>
                                                <asp:Button ID="ibtnClose" runat="server" Text="Close" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <%-- <tr>
                                <td style="width: 100px; text-align: right"></td>
                                <td style="width: 100px; text-align: left">&nbsp;
                                    <table style="width: 100%; height: 100%">
                                        <tr>
                                            <td style="width: 8px; height: 30px; text-align: right">
                                                <asp:ImageButton ID="ibtnOK" runat="server" ImageUrl="~/images/add_list.gif" Width="24px"
                                                    Height="24px" />
                                            </td>
                                            <td style="width: 49px; height: 30px; text-align: left">
                                                <asp:Label ID="Label8" runat="server" Text="Select" Width="21px"></asp:Label>
                                            </td>
                                            <td style="width: 34px; height: 30px; text-align: left">
                                                <asp:ImageButton ID="ibtnClose" runat="server" Height="24px" ImageUrl="~/images/ok_cancel.jpg"
                                                    Width="24px" />
                                            </td>
                                            <td style="width: 158px; height: 30px; text-align: left">
                                                <asp:Label ID="Label10" runat="server" Text="Close" Width="21px"></asp:Label>
                                            </td>
                                            <td style="width: 142px; height: 30px; text-align: left"></td>
                                            <td style="width: 100px; height: 30px; text-align: left"></td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>--%>
                        </table>
                    </td>
                </tr>
                <tr style="width:100%">
                     <td colspan="3" style="height: 12px" align="left">
                            <asp:Label ID="lblMsg" runat="server" CssClass="lblError" Style="text-align: center"></asp:Label>
                        </td>
                </tr>
            </table>
            <br />

        </div>
    </form>
</body>
</html>
