<%@ Page Language="VB" MasterPageFile="~/MasterPage3.master" AutoEventWireup="false" MaintainScrollPositionOnPostback="true"
    CodeFile="UniversityProfile.aspx.vb" Inherits="universityProfile" Title="Welcome To SAS" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">   
    <script language="javascript" type="text/javascript">
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
        function validate() {
            var re = /\s*((\S+\s*)*)/;
            if (document.getElementById("<%=txtCode.ClientID%>").value.replace(re, "$1").length == 0) {
                alert("University Profile Code Field Cannot Be Blank");
                document.getElementById("<%=txtCode.ClientID%>").focus();
                return false;
            }
            var digits = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            var temp;
            for (var i = 0; i < document.getElementById("<%=txtCode.ClientID %>").value.length; i++) {
                temp = document.getElementById("<%=txtCode.ClientID%>").value.substring(i, i + 1);
                if (digits.indexOf(temp) == -1) {
                    alert("Enter Valid University Profile Code");
                    document.getElementById("<%=txtCode.ClientID%>").focus();
                    return false;
                }
            }
            if (document.getElementById("<%=txtName.ClientID%>").value.replace(re, "$1").length == 0) {
                alert("University Profile Description Field Cannot Be Blank");
                document.getElementById("<%=txtName.ClientID%>").focus();
                return false;
            }
            if (document.getElementById("<%=txtSName.ClientID%>").value.replace(re, "$1").length == 0) {
                alert("University Profile Short Description Field Cannot Be Blank");
                document.getElementById("<%=txtSName.ClientID%>").focus();
                return false;
            }
            return true;
        }
        function getconfirm() {
            if (document.getElementById("<%=txtName.ClientID%>").value == "") {

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

        function isNumberKey(evt) {
            var charCode = (evt.which) ? evt.which : event.keyCode

            if (charCode > 31 && (charCode < 48 || charCode > 57)) {
                alert("Enter Only Digits");
                evt.preventDefault();
                return false;
            }
        }

    </script>
    <%--  <atlas:ScriptManager ID="scriptmanager1" EnablePartialRendering="true" runat="Server" />
   <atlas:UpdatePanel ID="up1" runat="server">
	<ContentTemplate>--%>
    <asp:Panel ID="Panel1" runat="server" Width="100%">
        <table style="background-image: url(images/Sample.png);">
            <tr valign="middle">
                <td style="width: 4px; height: 14px">
                </td>
                <td style="width: 14px; height: 14px">
                </td>
                 <%-- Editted By Zoya @29/04/2016--%>
                <td style="float: left;display:None; visibility:collapse;">
                    <table style="border-collapse: collapse;" class="menuoff" onmouseover="className='menuon';"
                        onmouseout="className='menuoff';">
                        <tr>
                            <td>
                                <asp:ImageButton ID="ibtnNew" runat="server" ImageUrl="images/add.png" ToolTip="New" visible="false"/>
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
                                <asp:ImageButton ID="ibtnSave" runat="server" ImageUrl="~/images/save.png" ToolTip="Save" />
                            </td>
                            <td>
                                <asp:Label ID="Label14" runat="server" Text="Save"></asp:Label>
                            </td>
                        </tr>
                    </table>
                </td>
                <td style="float: left;display:None; visibility:collapse;">
                    <table style="border-collapse: collapse;" class="menuoff" onmouseover="className='menuon';"
                        onmouseout="className='menuoff';">
                        <tr>
                            <td>
                                <asp:ImageButton ID="ibtnDelete" runat="server" ImageUrl="~/images/delete.png" visible="false"/>
                            </td>
                            <td>
                                <asp:Label ID="Label13" runat="server" Text="Delete"></asp:Label>
                            </td>
                        </tr>
                    </table>
                </td>
                 <%--Done Editted By Zoya @29/04/2016--%>
                <td>
                    <table style="border-collapse: collapse;" class="menuoff" onmouseover="className='menuon';"
                        onmouseout="className='menuoff';">
                        <tr>
                            <td>
                                <asp:ImageButton ID="ibtnView" runat="server" ImageUrl="~/images/find.png" ToolTip="View" />
                            </td>
                            <td>
                                <asp:Label ID="Label16" runat="server" Text="Search"></asp:Label>
                            </td>
                        </tr>
                    </table>
                </td>
                <%-- Editted By Zoya @25/02/2016--%>
                <td style="float: left;display:None; visibility:collapse;">
                    <table style="border-collapse: collapse;" class="menuoff" onmouseover="className='menuon';"
                        onmouseout="className='menuoff';">
                        <tr>
                            <td>
                                <asp:ImageButton ID="ibtnPrint" runat="server" ImageUrl="~/images/print.png" ToolTip="Print" Visible="false"/>
                            </td>
                            <td>
                                <asp:Label ID="Label17" runat="server" Text="Print" Visible="false"></asp:Label>
                            </td>
                        </tr>
                    </table>
                </td>
                <%-- Done Editted By Zoya @25/02/2016--%>
                 <%-- Editted By Zoya @24/02/2016--%>
                <td style="float: left;display:None; visibility:collapse;">
                    <table style="border-collapse: collapse;" class="menuoff" onmouseover="className='menuon';"
                        onmouseout="className='menuoff';">
                        <tr>
                            <td>
                                <asp:ImageButton ID="ibtnPosting" runat="server" ImageUrl="~/images/gposting.png"
                                    ToolTip="Cancel" Visible="false" />
                            </td>
                            <td>
                                <asp:Label ID="Label6" runat="server" Text="Posting" Visible="false"></asp:Label>
                            </td>
                        </tr>
                    </table>
                </td>
                <td style="float: left;display:None; visibility:collapse;">
                    <table style="border-collapse: collapse;" class="menuoff" onmouseover="className='menuon';"
                        onmouseout="className='menuoff';">
                        <tr>
                            <td>
                                <asp:ImageButton ID="ibtnOthers" runat="server" ImageUrl="~/images/gothers.gif" ToolTip="Cancel" Visible="false" Width="23px" />
                            </td>
                            <td>
                                <asp:Label ID="Label5" runat="server" Text="Others" Visible="false"></asp:Label>
                            </td>
                        </tr>
                    </table>
                </td>
                <td style="float: left">
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
                <%-- Done Editted By Zoya @24/02/2016--%>

                <td style="width: 2%; height: 14px">
                    <asp:ImageButton ID="ibtnFirst" runat="server" ImageUrl="~/images/new_last.png" />
                </td>
                <td style="width: 2%; height: 14px">
                    <asp:ImageButton ID="ibtnPrevs" runat="server" ImageUrl="~/images/new_prev.png" />
                </td>
                <td style="width: 2%; height: 14px">
                    <asp:TextBox ID="txtRecNo" runat="server" Width="52px" AutoPostBack="True" Style="text-align: right"
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
                    <asp:ImageButton ID="ibtnNext" runat="server" ImageUrl="~/images/next.png" />
                </td>
                <td style="width: 2%; height: 14px">
                    <asp:ImageButton ID="ibtnLast" runat="server" ImageUrl="~/images/new_first.png" />
                </td>
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
            <tr>
                <td>
                </td>
            </tr>
        </table>
        <table class="mainbg" style="width: 100%;">
            <tr>
                <td style="width: 494px; height: 39px;">
                    <asp:SiteMapPath ID="SiteMapPath1" runat="server">
                    </asp:SiteMapPath>
                </td>
                <td class="pagetext" style="height: 39px" align="right">
                    <asp:Label ID="lblMenuName" runat="server" Text="University Fund" Width="326px"></asp:Label>
                </td>
            </tr>
        </table>
    </asp:Panel>
    <%--  </ContentTemplate>     
</atlas:UpdatePanel>
<atlas:UpdateProgress ID="ProgressIndicator" runat="server">
    <ProgressTemplate>
        Loading the data, please wait... 
        <asp:Image ID="LoadingImage" ImageAlign=AbsMiddle runat="server" ImageUrl="~/Images/spinner.gif" />        
    </ProgressTemplate>
 </atlas:UpdateProgress>
<atlas:UpdatePanel ID="up2" runat="server">
<ContentTemplate>
    --%>
    <asp:Panel ID="PnlAdd" runat="server" Width="100%">
        <table style="width: 100%">
            <tr>
                <td style="width: 100%">
                   <div  style="border: thin solid #A6D9F4; width: 100%">
                        <table style="width: 100%">
                            <tr>
                                <td style="width: 1px">
                                </td>
                                <td class="fields" style="width: 312px; height: 24px">
                                </td>
                                <td class="fields" style="width: 37%; height: 24px; text-align: center">
                                    <asp:Label ID="lblMsg" runat="server" CssClass="lblError" Style="text-align: center"
                                        Width="348px"></asp:Label>
                                </td>
                                <td style="width: 4823px; height: 24px">
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 1px; height: 25px">
                                </td>
                                <td class="fields" style="width: 312px; height: 25px">
                                </td>
                                <td class="fields" style="width: 37%; height: 25px; text-align: left">
                                    <span style="font-size: 11pt; color: #ff0066"></span>
                                </td>
                                <td style="width: 4823px; height: 25px">
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 1px">
                                </td>
                                <td class="fields" style="width: 312px; height: 24px">
                                    <span
                                        style="font-size: 11pt; color: #ff0066; font-family: Arial">*</span>
                                    <asp:Label ID="Label19" runat="server" Text="Code"></asp:Label>
                                </td>
                                <td class="fields" style="width: 37%; height: 24px">
                                    <asp:TextBox ID="txtCode" runat="server" MaxLength="20" Width="142px"></asp:TextBox>
                                </td>
                                <td style="font-size: 8pt; width: 4823px; height: 24px">
                                </td>
                            </tr>
                            <tr style="font-size: 8pt">
                                <td style="width: 1px">
                                </td>
                                <td class="fields" style="width: 312px; height: 24px;">
                                    <span style="font-size: 11pt; color: #ff0066">*</span>
                                    <asp:Label ID="Label1" runat="server" Text="Description "></asp:Label>
                                </td>
                                <td class="fields" style="width: 37%; height: 24px;">
                                    <asp:TextBox ID="txtName" runat="server" Width="517px" MaxLength="50"></asp:TextBox>
                                </td>
                                <td style="width: 4823px; height: 24px;">
                                    <span style="color: #ff0066"></span>
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 1px; height: 29px;">
                                </td>
                                <td class="fields" style="width: 312px; height: 29px">
                                    <span
                                        style="font-size: 11pt; color: #ff0066; font-family: Arial">*</span>
                                    <asp:Label ID="Label20" runat="server" Text="Short Description " Width="108px"></asp:Label>
                                </td>
                                <td class="fields" style="width: 37%; height: 29px">
                                    <asp:TextBox ID="txtSName" runat="server" Width="142px" MaxLength="50"></asp:TextBox><span style="color: #ff0066"></span>
                                </td>
                                <td style="width: 4823px; height: 29px">
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 1px; height: 24px;">
                                </td>
                                <td class="fields" style="width: 312px; height: 24px;">
                                    <asp:Label ID="Label2" runat="server" Text="Address"></asp:Label>
                                </td>
                                <td class="fields" style="width: 37%; height: 24px;">
                                    <asp:TextBox ID="txtAddress" runat="server" Width="346px" MaxLength="50"></asp:TextBox>
                                </td>
                                <td style="width: 4823px; height: 24px">
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 1px; height: 21px">
                                </td>
                                <td style="width: 312px; height: 24px">
                                </td>
                                <td class="fields" style="width: 37%; height: 24px">
                                    <asp:TextBox ID="txtAddres1" runat="server" Width="346px" MaxLength="50"></asp:TextBox>
                                </td>
                                <td class="fields" style="width: 4823px; height: 24px">
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 1px; height: 24px">
                                </td>
                                <td style="width: 312px; height: 24px">
                                </td>
                                <td class="fields" style="width: 37%; height: 24px">
                                    <asp:TextBox ID="txtAddres2" runat="server" Width="346px" MaxLength="50"></asp:TextBox>
                                </td>
                                <td class="fields" style="width: 4823px; height: 24px">
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 1px; height: 21px">
                                </td>
                                <td class="fields" style="width: 312px; height: 24px">
                                    <asp:Label ID="Label3" runat="server" Text="City"></asp:Label>
                                </td>
                                <td class="fields" style="width: 37%; height: 24px">
                                    <asp:TextBox ID="TxtCity" runat="server" Width="346px" MaxLength="50"></asp:TextBox>
                                </td>
                                <td style="width: 4823px; height: 24px">
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 1px; height: 26px;">
                                </td>
                                <td class="fields" style="width: 312px; height: 26px;">
                                    <asp:Label ID="Label4" runat="server" Text="State"></asp:Label>
                                </td>
                                <td style="width: 37%; height: 26px;">
                                    <asp:TextBox ID="txtState" runat="server" MaxLength="50" Width="277px"></asp:TextBox>
                                </td>
                                <td class="fields" style="width: 4823px; height: 26px;">
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 1px; height: 25px;">
                                </td>
                                <td class="fields" style="width: 312px; height: 25px;">
                                    <asp:Label ID="Label7" runat="server" Text="Country"></asp:Label>
                                </td>
                                <td class="fields" style="width: 37%; height: 25px;">
                                    <asp:DropDownList ID="ddlCountry" runat="server" Width="205px">
                                        <asp:ListItem Value="-">-</asp:ListItem>
                                        <asp:ListItem Value="us">U.S.</asp:ListItem>
                                        <asp:ListItem Value="cn">China</asp:ListItem>
                                        <asp:ListItem Value="uk">United Kingdom</asp:ListItem>
                                        <asp:ListItem Value="ar">Argentina</asp:ListItem>
                                        <asp:ListItem Value="aa">Asia</asp:ListItem>
                                        <asp:ListItem Value="au">Australia</asp:ListItem>
                                        <asp:ListItem Value="br">Brazil</asp:ListItem>
                                        <asp:ListItem Value="ca">Canada in English</asp:ListItem>
                                        <asp:ListItem Value="cf">Canada in French</asp:ListItem>
                                        <asp:ListItem Value="dk">Denmark</asp:ListItem>
                                        <asp:ListItem Value="fr">France </asp:ListItem>
                                        <asp:ListItem Value="de">Germany</asp:ListItem>
                                        <asp:ListItem Value="gr">Greece</asp:ListItem>
                                        <asp:ListItem Value="hk">Hong Kong</asp:ListItem>
                                        <asp:ListItem Value="in">India</asp:ListItem>
                                        <asp:ListItem Value="id">Indonesia</asp:ListItem>
                                        <asp:ListItem Value="ie">Ireland</asp:ListItem>
                                        <asp:ListItem Value="it">Italy</asp:ListItem>
                                        <asp:ListItem Value="kr">Korea</asp:ListItem>
                                        <asp:ListItem Value="my">Malaysia</asp:ListItem>
                                        <asp:ListItem Value="mx">Mexico</asp:ListItem>
                                        <asp:ListItem Value="nz">New Zealand</asp:ListItem>
                                        <asp:ListItem Value="no">Norway</asp:ListItem>
                                        <asp:ListItem Value="ph">Philippines</asp:ListItem>
                                        <asp:ListItem Value="pl">Poland</asp:ListItem>
                                        <asp:ListItem Value="sg">Singapore</asp:ListItem>
                                        <asp:ListItem Value="es">Spain</asp:ListItem>
                                        <asp:ListItem Value="se">Sweden</asp:ListItem>
                                        <asp:ListItem Value="tw">Taiwan</asp:ListItem>
                                        <asp:ListItem Value="th">Thailand</asp:ListItem>
                                        <asp:ListItem Value="tr">Turkey</asp:ListItem>
                                        <asp:ListItem Value="vn">Vietnam</asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                                <td style="width: 4823px; height: 25px;">
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 1px; height: 26px;">
                                </td>
                                <td class="fields" style="width: 312px; height: 26px;">
                                    <asp:Label ID="Label8" runat="server" Text="Post Code" Width="83px"></asp:Label>
                                </td>
                                <td class="fields" style="width: 37%; height: 26px;">
                                    <asp:TextBox ID="txtPostalCode" runat="server" Width="142px" MaxLength="10"></asp:TextBox>
                                </td>
                                <td style="width: 4823px; height: 26px;">
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 1px; height: 15px;">
                                </td>
                                <td class="fields" style="width: 312px; height: 24px;">
                                    <asp:Label ID="Label9" runat="server" Text="Phone"></asp:Label>
                                </td>
                                <td class="fields" style="width: 37%; height: 24px;">
                                    <asp:TextBox ID="txtPhone" runat="server" Width="114px" MaxLength="20"></asp:TextBox>
                                </td>
                                <td style="width: 4823px; height: 24px;">
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 1px">
                                </td>
                                <td class="fields" style="width: 312px; height: 24px;">
                                    <asp:Label ID="Label10" runat="server" Text="Fax"></asp:Label>
                                </td>
                                <td class="fields" style="width: 37%; height: 24px;">
                                    <asp:TextBox ID="txtFax" runat="server" Width="114px" MaxLength="20"></asp:TextBox>
                                </td>
                                <td style="width: 4823px; height: 24px;">
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 1px; height: 23px;">
                                </td>
                                <td class="fields" style="width: 312px; height: 24px;">
                                    <asp:Label ID="Label12" runat="server" Text="E-Mail" Width="47px"></asp:Label>
                                </td>
                                <td class="fields" style="width: 37%; height: 24px;">
                                    <asp:TextBox ID="txtEmail" runat="server" Width="346px" MaxLength="50"></asp:TextBox>
                                </td>
                                <td style="width: 4823px; height: 24px;">
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 1px">
                                </td>
                                <td class="fields" style="width: 312px; height: 24px;">
                                    <asp:Label ID="Label15" runat="server" Text="Web Site" Width="62px"></asp:Label>
                                </td>
                                <td class="fields" style="width: 37%; height: 24px;">
                                    <asp:TextBox ID="txtWebSite" runat="server" Width="346px" MaxLength="50"></asp:TextBox>
                                </td>
                                <td style="width: 4823px; height: 24px;">
                                </td>
                            </tr>
                        </table>
                   </div>
                </td>
            </tr>
        </table>
    </asp:Panel>
    <%--             
     </ContentTemplate>     
</atlas:UpdatePanel>--%>
</asp:Content>
