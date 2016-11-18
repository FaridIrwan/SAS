<%@ Page Title="Welcome To SAS" Language="VB" MasterPageFile="~/MasterPage3.master" AutoEventWireup="false" MaintainScrollPositionOnPostback="true"
    CodeFile="SponsorCoverLetter.aspx.vb" Inherits="SponsorCoverLetter" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <script language="javascript" type="text/javascript">
        function getibtnFDate() {
            popUpCalendar(document.getElementById("<%=ibtnFdate.ClientID%>"), document.getElementById("<%=txtFrom.ClientID%>"), 'dd/mm/yyyy')
        }
        function getDateto() {
            popUpCalendar(document.getElementById("<%=ibtnTodate.ClientID%>"), document.getElementById("<%=txtToDate.ClientID%>"), 'dd/mm/yyyy')
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
            if (document.getElementById("<%=ddlCode.ClientID%>").selectedIndex == 0) {

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
            if (document.getElementById("<%=txtAddress.ClientID%>").value.replace(re, "$1").length == 0) {
                alert("Address Field Cannot Be Blank");
                document.getElementById("<%=txtAddress.ClientID%>").focus();
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
        <div style="position: absolute; width: 83.2%; height: 100%; background-color: Gray; filter: alpha(opacity=70); opacity: 0.7;">
        </div>
        <table style="position: absolute; width: 80%; height: 100%; z-index: 10003;">
            <tr>
                <td align="center" valign="middle">
                    <div style="color: Black; font-weight: bolder; background-color: White; padding: 15px; width: 200px;">
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
                <td style="width: 4px; height: 14px"></td>
                <td style="width: 14px; height: 14px"></td>
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
                                <asp:ImageButton ID="ibtnView" runat="server" Enabled="false" ImageUrl="~/images/gfind.png"
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
                <td style="width: 2%; height: 14px"></td>
                <td style="width: 100%; height: 14px"></td>
                <td style="width: 100%; height: 14px"></td>
            </tr>
        </table>
        <table style="width: 100%">
            <tr>
                <td class="vline" style="width: 746px; height: 1px"></td>
            </tr>
        </table>
        <table width="100%">
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
                <td></td>
                <td>&nbsp;
                </td>
                <td></td>
            </tr>
            <tr>
                <td colspan="2" style="padding-left: 20px">
                    <asp:Label ID="lblMesg" runat="server" ForeColor="Red" Visible="False" CssClass="lblError1"></asp:Label>
                </td>
            </tr>
            <tr>
                <td style="width: 1px; height: 16px"></td>
                <td style="width: 100%; height: 16px">
                    <div style="width: 100%">
                        <asp:Panel ID="pnlExisting" Width="100%" runat="server">
                            <fieldset style="width: 99%; border: thin solid #A6D9F4;">
                                <legend><strong><span style="color: #000000;">Selection Criteria</span></strong></legend>
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
                                    <tr>
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
                                    <tr>
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
                                    <tr>
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
                            </fieldset>
                        </asp:Panel>
                    </div>
                </td>
                <td style="width: 113px; height: 16px"></td>
            </tr>
            <tr>
                <td></td>
                <td align="right">
                    <asp:Button ID="btnSend" runat="server" Width="100px" Text="Send" OnClientClick="showhide();" />
                </td>
                <td style="height: 16px;"></td>
            </tr>
        </table>
        <fieldset style="width: 99.5%">
            <legend><strong><span style="color: #000000;">Select Sponsor(s)</span></strong></legend>
            <br />
            <table width="60%">
                <tr>
                    <td style="width: 103px;">
                        <span style="color: #ff0000">*</span>
                        <asp:Label ID="lblSponsor" runat="server" Text="Sponsor"></asp:Label>
                    </td>
                    <td style="width: 404px;">
                        <asp:DropDownList ID="ddl_Sponsor" AppendDataBoundItems="true" runat="server" AutoPostBack="True"
                            Width="400px">
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td style="width: 103px">
                        <asp:Label ID="lblProgram" runat="server" Text="Program"></asp:Label>
                    </td>
                    <td style="width: 404px">
                        <asp:DropDownList ID="ddlProgram" AppendDataBoundItems="true" runat="server" AutoPostBack="True"
                            Width="400px" Enabled="false">
                        </asp:DropDownList>
                    </td>
                    <td></td>
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
                    <td></td>
                </tr>
                <tr>
                    <td></td>
                    <td style="width: 404px"></td>
                    <td></td>
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
                <asp:BoundColumn DataField="ICNo" HeaderText="IC No"></asp:BoundColumn>
                <asp:BoundColumn DataField="ProgramID" HeaderText="Program"></asp:BoundColumn>
                <asp:BoundColumn DataField="CurretSemesterYear" HeaderText="Semester"></asp:BoundColumn>
                <asp:BoundColumn DataField="ProgramName" HeaderText="Program Name" Visible="false"></asp:BoundColumn>
            </Columns>
        </asp:DataGrid>
        <br />
        <br />
         <br />
        <table style="width:100%">
            <tr>
                <td>Rujukan Kami	
                </td>
                <td>:
                </td>
                <td>{0} ({4})
                </td>
            </tr>
            <tr>
                <td>Rujukan Tuan		
                </td>
                <td>:
                </td>
                <td>{1}{5}
                </td>
            </tr>
            <tr>
                <td>Tarikh			
                </td>
                <td>:
                </td>
                <td>{2}
                </td>
            </tr>
            <tr>
            </tr>
            <tr>
                <td>Ketua Pengarah Perkhidmatan Awam<br/>Jabatan Perkhidmatan Awam Malaysia<br/>Bahagian Pembangunan Modal Insan<br/>Aras 4-6, Blok C1, Kompleks C <br/>Pusat Pentadbiran Kerajaan Persekutuan<br/>62510 PUTRAJAYA
                </td>
            </tr>
        </table>
        <p>Tuan/Puan,</p>
        <h2>TUNTUTAN YURAN PENGAJIAN PELAJAR IJAZAH LANJUTAN DI UNIVERSITI PUTRA MALAYSIA</h2>
        <p>Adalah dimaklumkan pelajar tersebut  adalah pelajar ijazah lanjutan yang berdaftar di Universiti Putra Malaysia. Berikut adalah maklumat pembayaran yuran pelajar.</p>
        <table style="width:100%">
            <tr>
                <td>Nama Pelajar
                </td>
                <td>:
                </td>
                <td>{3}
                </td>
            </tr>
            <tr>
                <td>No. Matrik	
                </td>
                <td>:</td>
                <td>{4}</td>
            </tr>
            <tr>
                <td>No. Kad Pengenalan	
                </td>
                <td>:</td>
                <td>{5}</td>
            </tr>
            <tr>
                <td>Program Pengajian  </td>
                <td>:</td>
                <td>{6}</td>
            </tr>
            <tr>
                <td>Semester</td>
                <td>:</td>
                <td>SEMESTER {7}</td>
            </tr>
        </table>
        <br />
        <table style="width:50%;" class="tableWithBorder">
            <thead style="background-color:lightgrey; ">
                <tr>
                    <th>KETERANGAN
                    </th>
                    <th>JUMLAH (RM)
                    </th>
                </tr>
            </thead>
            <tr>
                <td>Yuran xxx</td>
                <td>100</td>
            </tr>
            <tr>
                <td>Yuran xxx</td>
                <td>100</td>
            </tr>
            <tr>
                <td>Yuran xxx</td>
                <td>100</td>
            </tr>
            <tfoot style="background-color:lightgrey">
                <tr>
                    <td>JUMLAH KESELURUHAN
                    </td>
                    <td>300</td>
                </tr>
            </tfoot>
        </table>
        <br />
        <p>Sila sertakan senarai nama pelajar yang dibayar dan maklumat pembayaran adalah seperti berikut :</p>
        <table style="width:100%">
            <tr>
                <td>Nama Bank	
                </td>
                <td>:
                </td>
                <td>CIMB BANK BERHAD
                </td>
            </tr>
            <tr>
                <td>No.  Akaun
                </td>
                <td>:
                </td>
                <td>8002155042
                </td>
            </tr>
            <tr>
                <td>Nama Akaun
                </td>
                <td>:</td>
                <td>UPM COLLECTION</td>
            </tr>
            <tr>
                <td>Nama Penerima
                </td>
                <td>:</td>
                <td>BURSAR, KEWANGAN PELAJAR II</td>
            </tr>
            <tr>
                <td>Alamat
                </td>
                <td>:
                </td>
                <td>SEKSYEN KEWANGAN PELAJAR II
				PEJABAT BURSAR, UNIVERSITI PUTRA MALAYSIA
				43400 SERDANG, SELANGOR
                </td>
            </tr>
            <tr>
                <td>Alamat Emel
                </td>
                <td>:</td>
                <td>bursar.student_pg@upm.edu.my
                </td>
            </tr>
        </table>
        <p>
            Sekian, terima kasih.            
        </p>
        <p>
            “ BERLIMU, BERBAKTI “
        </p>
        <p>
            Yang menjalankan tugas,
        </p>
        <p>
            ( MAZITAH BINTI AHMAD ) <br />
            Penolong Bendahari Kanan <br />
            Pejabat Bursar	<br />
            Universiti Putra Malaysia
        </p>


        
    </asp:Panel>
</asp:Content>