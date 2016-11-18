<%@ Page Title="Student Dunning Letter" Language="VB" MasterPageFile="~/MasterPage3.master" AutoEventWireup="false" MaintainScrollPositionOnPostback="true"
    CodeFile="StudentDunningLetter.aspx.vb" Inherits="StudentDunningLetter" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <script language="javascript" type="text/javascript">
        function getibtnFDate() {
            popUpCalendar(document.getElementById("<%=ibtnFdate.ClientID%>"), document.getElementById("<%=txtFrom.ClientID%>"), 'dd/mm/yyyy')
        }
        function getDateto() {
            popUpCalendar(document.getElementById("<%=ibtnTodate.ClientID%>"), document.getElementById("<%=txtToDate.ClientID%>"), 'dd/mm/yyyy')
        }

        function getDunningFDate() {
            popUpCalendar(document.getElementById("<%=ibtnDunningFDate.ClientID%>"), document.getElementById("<%=txt2Date.ClientID%>"), 'dd/mm/yyyy')
        }
        function getDunningTDate() {
            popUpCalendar(document.getElementById("<%=ibtnDunningTDate.ClientID%>"), document.getElementById("<%=txt2SignDate.ClientID%>"), 'dd/mm/yyyy')
        }

        function geterr() {
            var digits = "0123456789";
            var temp;
            for (var i = 0; i < document.getElementById("<%=txtRecNo.ClientID %>").value.length; i++) {
                temp = document.getElementById("<%=txtRecNo.ClientID%>").value.substring(i, i + 1);
                if (digits.indexOf(temp) == -1) {
                    alert("Enter Valid Record No");
                    document.getElementById("<%=txtRecNo.ClientID%>").value = 1;
                    document.getElementById("<%=txtRecNo.ClientID%>").focus();
                    return false;
                }
            }
            return true;
        }
        function getconfirm() {
            var re = /\s*((\S+\s*)*)/;
            if (document.getElementById("<%=txtCode.ClientID%>").value.replace(re, "$1").length == 0) {

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
            if (document.getElementById("<%=txtCode.ClientID%>").value.replace(re, "$1").length == 0) {
                alert("Code Field Cannot Be Blank");
                document.getElementById("<%=txtCode.ClientID%>").focus();
                return false;
            }
            var digits = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            var temp;
            for (var i = 0; i < document.getElementById("<%=txtCode.ClientID %>").value.length; i++) {
                temp = document.getElementById("<%=txtCode.ClientID%>").value.substring(i, i + 1);
                if (digits.indexOf(temp) == -1) {
                    alert("Enter Valid Code");
                    document.getElementById("<%=txtCode.ClientID%>").focus();
                    return false;
                }
            }
            if (document.getElementById("<%=txtTitle.ClientID%>").value.replace(re, "$1").length == 0) {
                alert("Title Field Cannot Be Blank");
                document.getElementById("<%=txtTitle.ClientID%>").focus();
                return false;
            }
            if (document.getElementById("<%=txtRef.ClientID%>").value.replace(re, "$1").length == 0) {
                alert("Ref Field Cannot Be Blank");
                document.getElementById("<%=txtTitle.ClientID%>").focus();
                return false;
            }
            if (document.getElementById("<%=txtMsg.ClientID%>").value.replace(re, "$1").length == 0) {
                alert("Message Field Cannot Be Blank");
                document.getElementById("<%=txtMsg.ClientID%>").focus();
                return false;
            }
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

        function showhide() {
            document.getElementById("ModalPopup").style.visibility = "visible";
        }

     
    </script>
    <div id="ModalPopup" style="visibility: hidden; width: 90%">
        <div style="position: absolute; width: 83.2%; height: 100%; background-color: Gray;
            filter: alpha(opacity=70); opacity: 0.7;">
        </div>
        <table style="position: absolute; width: 80%; height: 100%; z-index: 10003;">
            <tr>
                <td align="center" valign="middle">
                    <div style="color: Black; font-weight: bolder; background-color: White; padding: 15px;
                        width: 200px;">
                        <asp:Image ID="Image3" runat="server" ImageUrl="~/images/roller.gif" />
                        <br />
                        Please Wait....
                    </div>
                </td>
            </tr>
        </table>
    </div>
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
                                <asp:ImageButton ID="ibtnSave" runat="server" Enabled="false" ImageUrl="~/images/gsave.png" ToolTip="Save" />
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
                                <asp:ImageButton ID="ibtnDelete" Enabled="false" runat="server" ImageUrl="~/images/gdelete.png" ToolTip="Delete" />
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
                                <asp:ImageButton ID="ibtnView"  runat="server" Enabled="false" ImageUrl="~/images/gfind.png"
                                    ToolTip="View" />
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
                        <tr id="trPrint" runat="server">
                            <td>
                                <asp:ImageButton ID="ibtnPrint" Enabled="false" runat="server" ImageUrl="~/images/gprint.png" ToolTip="Print" />
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
                                <%--OnClick="ibtnPosting_Click"--%>
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
                                <%--  OnClick="ibtnOthers_Click"--%>
                                <asp:ImageButton ID="ImageButton1" runat="server" ImageUrl="~/images/gothers.png"
                                    ToolTip="Cancel" />
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
                    <%-- OnTextChanged="txtRecNo_TextChanged"--%>
                    <asp:TextBox ID="txtRecNo" runat="server" AutoPostBack="True" Width="50px" MaxLength="7"
                        ReadOnly="true" CssClass="text_box" disabled="disabled" TabIndex="1" dir="ltr"></asp:TextBox>
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
        <table width="100%" >
            <tr>
                <td width="50%">
                    <asp:SiteMapPath ID="SiteMapPath1" runat="server">
                    </asp:SiteMapPath>
                </td>
                <td width="50%" align="right" class="pagetext">
                    <asp:Label ID="lblMenuName" runat="server" Width="350px"></asp:Label>
                </td>
            </tr>
        </table>
    </asp:Panel>
    <asp:Panel ID="pnlBody" runat="server">
        <table style="width: 100%">
            <tr>
                <td>
                </td>
                <td>
                    &nbsp;
                </td>
                <td>
                </td>
            </tr>
            <tr>
                <td colspan="2" style="padding-left: 20px">
                    <asp:Label ID="lblMesg" runat="server" ForeColor="Red" Visible="False" CssClass="lblError1"></asp:Label>
                </td>
            </tr>
            <tr>
                <td style="width: 1px; height: 16px">
                </td>
                <td style="width: 100%; height: 16px">
                    <div style="width: 100%">
                        <asp:Panel ID="pnlNew" Width="100%" runat="server" Visible="false">
                            <table style="width: 100%">
                                <tr>
                                    <td colspan="5" style="height: 16px;">
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2" style="height: 16px; width: 283px;">
                                        <asp:Label ID="Label2" runat="server" Text="Code"></asp:Label>
                                    </td>
                                    <td colspan="2" style="height: 16px">
                                        <asp:TextBox ID="txtCode" runat="server"></asp:TextBox><span style="color: #ff0000">*</span>
                                    </td>
                                    <td style="width: 1650px; height: 16px">
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2" style="height: 16px; width: 283px;">
                                        <asp:Label ID="Label3" runat="server" Text="Title"></asp:Label>
                                    </td>
                                    <td colspan="2" style="height: 16px">
                                        <asp:TextBox ID="txtTitle" runat="server" Width="519px" MaxLength="50"></asp:TextBox><span
                                            style="color: #ff0000">*</span>
                                    </td>
                                    <td style="width: 1650px; height: 16px">
                                        <span style="color: #ff0000"></span>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2" style="height: 16px; width: 283px;">
                                        <asp:Label ID="Label4" runat="server" Text="Ref"></asp:Label>
                                    </td>
                                    <td colspan="2" style="height: 16px">
                                        <asp:TextBox ID="txtRef" runat="server"></asp:TextBox><span style="color: #ff0000">*</span>
                                    </td>
                                    <td style="width: 1650px; height: 16px">
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2" style="height: 16px; width: 283px;">
                                        <asp:Label ID="Label7" runat="server" Text="Date"></asp:Label>
                                    </td>
                                    <td colspan="1" style="width: 71px; height: 16px">
                                        <asp:TextBox ID="txtFrom" runat="server"></asp:TextBox>
                                    </td>
                                    <td colspan="1" style="width: 444px; height: 16px">
                                        <asp:Image ID="ibtnFDate" runat="server" ImageUrl="~/images/cal.gif" /><span style="color: #ff0000">*</span>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2" style="height: 88px; vertical-align: top; width: 283px;">
                                        <asp:Label ID="Label8" runat="server" Text="Message"></asp:Label>
                                    </td>
                                    <td colspan="2" style="height: 88px">
                                        <asp:TextBox ID="txtMsg" runat="server" Height="108px" EnableTheming="false" TextMode="MultiLine"
                                            Width="519px"></asp:TextBox><span style="color: #ff0000">*</span>
                                    </td>
                                    <td style="width: 1650px; height: 88px">
                                        <span style="color: #ff0000"></span>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2" style="height: 16px; width: 283px;">
                                        <asp:Label ID="Label9" runat="server" Text="Sign By"></asp:Label>
                                    </td>
                                    <td colspan="2" style="height: 16px">
                                        <asp:TextBox ID="txtSignBy" runat="server" Width="519px" MaxLength="60"></asp:TextBox><span
                                            style="color: #ff0000">*</span>
                                    </td>
                                    <td style="width: 1650px; height: 16px">
                                        <span style="color: #ff0000"></span>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2" style="height: 16px; width: 283px;">
                                        <asp:Label ID="Label10" runat="server" Text="Name"></asp:Label>
                                    </td>
                                    <td colspan="2" style="height: 16px">
                                        <asp:TextBox ID="txtName" runat="server" Width="519px" MaxLength="50"></asp:TextBox><span
                                            style="color: #ff0000">*</span>
                                    </td>
                                    <td style="width: 1650px; height: 16px">
                                        <span style="color: #ff0000"></span>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2" style="height: 16px; width: 283px;">
                                        <asp:Label ID="Label12" runat="server" Text="Date"></asp:Label>
                                    </td>
                                    <td colspan="1" style="height: 16px">
                                        <asp:TextBox ID="txtTodate" runat="server"></asp:TextBox>
                                    </td>
                                    <td colspan="2" style="height: 16px">
                                        <asp:Image ID="ibtnTodate" runat="server" ImageUrl="~/images/cal.gif" /><span style="color: #ff0000">*</span>
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                        <asp:Panel ID="pnlExisting" Width="100%" runat="server">
                            <fieldset style="border: thin solid #A6D9F4; width: 99.5%">
                                <legend><strong><span style="color: #000000;">Choose Dunning Letter</span></strong></legend>
                                <table style="width: 100%; padding-left: 100px">
                                    <tr>
                                        <td colspan="5" style="height: 16px;">
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2" style="height: 16px; width: 283px;">
                                            <asp:Label ID="Label17" runat="server" Text="Code"></asp:Label>
                                        </td>
                                        <td colspan="2" style="height: 16px">
                                            <asp:DropDownList ID="ddlCode" AppendDataBoundItems="true" runat="server" Width="128px"
                                                Height="20px" AutoPostBack="True">
                                            </asp:DropDownList>
                                        </td>
                                        <td style="width: 1650px; height: 16px">
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2" style="height: 16px; width: 283px;">
                                            <asp:Label ID="Label19" runat="server" Text="Title"></asp:Label>
                                        </td>
                                        <td colspan="2" style="height: 16px">
                                            <asp:TextBox ID="txt2Tittle" runat="server" Width="519px" MaxLength="50"></asp:TextBox>
                                        </td>
                                        <td style="width: 1650px; height: 16px">
                                            <span style="color: #ff0000"></span>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2" style="height: 16px; width: 283px;">
                                            <asp:Label ID="Label20" runat="server" Text="Ref"></asp:Label>
                                        </td>
                                        <td colspan="2" style="height: 16px">
                                            <asp:TextBox ID="txt2Ref" runat="server"></asp:TextBox>
                                        </td>
                                        <td style="width: 1650px; height: 16px">
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2" style="height: 16px; width: 283px;">
                                            <asp:Label ID="Label21" runat="server" Text="Date"></asp:Label>
                                        </td>
                                        <td colspan="1" style="width: 71px; height: 16px">
                                            <asp:TextBox ID="txt2Date" runat="server"></asp:TextBox>
                                        </td>
                                        <td colspan="1" style="width: 444px; height: 16px">
                                            <asp:Image ID="ibtnDunningFDate" runat="server" ImageUrl="~/images/cal.gif" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2" style="height: 88px; vertical-align: top; width: 283px;">
                                            <asp:Label ID="Label22" runat="server" Text="Message"></asp:Label>
                                        </td>
                                        <td colspan="2" style="height: 88px">
                                            <asp:TextBox ID="txt2Msg" runat="server" Height="108px" TextMode="MultiLine" EnableTheming="false"
                                                Width="519px"></asp:TextBox>
                                        </td>
                                        <td style="width: 1650px; height: 88px">
                                            <span style="color: #ff0000"></span>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2" style="height: 16px; width: 283px;">
                                            <asp:Label ID="Label23" runat="server" Text="Sign By"></asp:Label>
                                        </td>
                                        <td colspan="2" style="height: 16px">
                                            <asp:TextBox ID="txt2Signby" runat="server" Width="519px" MaxLength="60"></asp:TextBox>
                                        </td>
                                        <td style="width: 1650px; height: 16px">
                                            <span style="color: #ff0000"></span>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2" style="height: 16px; width: 283px;">
                                            <asp:Label ID="Label24" runat="server" Text="Name"></asp:Label>
                                        </td>
                                        <td colspan="2" style="height: 16px">
                                            <asp:TextBox ID="txt2Name" runat="server" Width="519px" MaxLength="50"></asp:TextBox>
                                        </td>
                                        <td style="width: 1650px; height: 16px">
                                            <span style="color: #ff0000"></span>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2" style="height: 16px; width: 283px;">
                                            <asp:Label ID="Label25" runat="server" Text="Date"></asp:Label>
                                        </td>
                                        <td colspan="1" style="height: 16px">
                                            <asp:TextBox ID="txt2SignDate" runat="server"></asp:TextBox>
                                        </td>
                                        <td colspan="2" style="height: 16px">
                                            <asp:Image ID="ibtnDunningTDate" runat="server" ImageUrl="~/images/cal.gif" />
                                        </td>
                                    </tr>
                                </table>
                            </fieldset>
                        </asp:Panel>
                    </div>
                </td>
                <td style="width: 113px; height: 16px">
                </td>
            </tr>
            <tr>
                <td>
                </td>
                <td align="right">
                    <asp:Button ID="btnSend" runat="server" Width="100px" Text="Send" OnClientClick="showhide();" />
                </td>
                <td style="height: 16px;">
                </td>
            </tr>
        </table>
        <fieldset style="width: 99.5%">
            <legend><strong><span style="color: #000000;">Select Student(s)</span></strong></legend>
            <br />
            <table width="60%">
                <tr>
                    <td style="width: 103px">
                        <asp:Label ID="lblProgram" runat="server" Text="Program"></asp:Label>
                    </td>
                    <td style="width: 404px">
                        <asp:DropDownList ID="ddlProgram" AppendDataBoundItems="true" runat="server" AutoPostBack="True"
                            Width="400px">
                        </asp:DropDownList>
                    </td>
                    <td>
                    </td>
                </tr>
                <tr>
                    <td style="width: 103px">
                        <asp:Label ID="Label1" runat="server" Text="Semester"></asp:Label>
                    </td>
                    <td style="width: 404px">
                        <asp:DropDownList ID="ddlSemester" AppendDataBoundItems="true" runat="server" Enabled="False">
                        </asp:DropDownList>
                        &nbsp;&nbsp;<asp:Button ID="btnFind" runat="server" Text="Find" Width="57px" />
                    </td>
                    <td>
                    </td>
                </tr>
                <tr>
                    <td>
                    </td>
                    <td style="width: 404px">
                    </td>
                    <td>
                    </td>
                </tr>
            </table>
        </fieldset>
        <br />
        <asp:DataGrid ID="dgStudent" runat="server" AutoGenerateColumns="False" Width="100%">
            <FooterStyle CssClass="dgFooterStyle" Height="20px" />
            <SelectedItemStyle CssClass="dgSelectedItemStyle" />
            <AlternatingItemStyle BackColor="Beige" CssClass="dgAlternatingItemStyle" Font-Bold="False"
                Font-Italic="False" Font-Overline="False" Font-Strikeout="False" Font-Underline="False" />
            <ItemStyle CssClass="dgItemStyle" />
            <HeaderStyle BackColor="#CDD7EE" CssClass="dgHeaderStyle" Font-Bold="True" Font-Italic="False"
                Font-Overline="False" Font-Size="Medium" Font-Strikeout="False" Font-Underline="False" />
            <Columns>
                <asp:TemplateColumn HeaderText="Select">
                    <HeaderTemplate>
                        <asp:CheckBox ID="chkSelectAll" runat="server" AutoPostBack="True" OnCheckedChanged="chkSelectAll_CheckedChanged" />
                    </HeaderTemplate>
                    <ItemTemplate>
                        <asp:CheckBox ID="chkSelect" runat="server" />
                    </ItemTemplate>
                </asp:TemplateColumn>
                <asp:BoundColumn DataField="MatricNo" HeaderText="Matric No"></asp:BoundColumn>
                <asp:BoundColumn DataField="StudentName" HeaderText="Student Name"></asp:BoundColumn>
                <asp:BoundColumn DataField="SASI_Email" HeaderText="Email"></asp:BoundColumn>
                <asp:BoundColumn DataField="ProgramID" HeaderStyle-HorizontalAlign="Left" HeaderText="Program">
                    <HeaderStyle HorizontalAlign="Left" />
                </asp:BoundColumn>
                <asp:BoundColumn DataField="CurrentSemester" HeaderText="Semester"></asp:BoundColumn>
                <asp:BoundColumn DataField="OutStandingAmount" HeaderText="Outstanding Amount" DataFormatString="{0:F}">
                    <HeaderStyle HorizontalAlign="Right" />
                    <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                        Font-Underline="False" HorizontalAlign="Right" />
                </asp:BoundColumn>
                <asp:BoundColumn DataField="SASI_Add1" Visible="false"></asp:BoundColumn>
                <asp:BoundColumn DataField="SASI_Add2" Visible="false"></asp:BoundColumn>
                <asp:BoundColumn DataField="SASI_City" Visible="false"></asp:BoundColumn>
                <asp:BoundColumn DataField="SASI_State" Visible="false"></asp:BoundColumn>
                <asp:BoundColumn DataField="SASI_Postcode" Visible="false"></asp:BoundColumn>
            </Columns>
        </asp:DataGrid>
        <br />
        <br />
    </asp:Panel>
</asp:Content>
