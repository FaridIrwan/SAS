<%@ Page Language="VB" MasterPageFile="~/MasterPage3.master" AutoEventWireup="false" MaintainScrollPositionOnPostback="true"
    CodeFile="Users.aspx.vb" Inherits="Users" Title="Welcome To SAS" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <script language="javascript" type="Scripts/popcalendar.js"></script>
    <script language="javascript" type="Scripts/functions.js"></script>

    <script language="javascript" type="text/javascript">

//          function CheckBoxCheck(rb) {
//            debugger;
//            var gv = document.getElementById("<%=dgDataGrid.ClientID%>");
//           var chk = gv.getElementsByTagName("input");
//            var row = rb.parentNode.parentNode;
//           for (var i = 0; i < chk.length; i++) {
//               if (chk[i].type == "checkbox") {
//                    if (chk[i].checked && chk[i] != rb) {
//                        chk[i].checked = false;
//                        break;
//                    }
//                }
//            }
       

        function validate() {
            var re = /\s*((\S+\s*)*)/;
            if (document.getElementById("<%=txtUserName.ClientID%>").value.replace(re, "$1").length == 0) {
                alert("User Name Field Cannot Be Blank");
                document.getElementById("<%=txtUserName.ClientID%>").focus();
                return false;
            }

            var digits = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            var temp;
            for (var i = 0; i < document.getElementById("<%=txtUserName.ClientID %>").value.length; i++) {
                temp = document.getElementById("<%=txtUserName.ClientID%>").value.substring(i, i + 1);
                if (digits.indexOf(temp) == -1) {
                    alert("Enter Valid User Name");
                    document.getElementById("<%=txtUserName.ClientID%>").focus();
                    return false;
                }
            }


            if (document.getElementById("<%=txtPassword.ClientID%>").value.replace(re, "$1").length == 0) {
                alert("Password Field Cannot Be Blank");
                document.getElementById("<%=txtPassword.ClientID%>").focus();
                return false;
            }
            var digits = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            var temp;
            for (var i = 0; i < document.getElementById("<%=txtPassword.ClientID %>").value.length; i++) {
                temp = document.getElementById("<%=txtPassword.ClientID%>").value.substring(i, i + 1);
                if (digits.indexOf(temp) == -1) {
                    alert("Enter Valid Password");
                    document.getElementById("<%=txtPassword.ClientID%>").focus();
                    return false;
                }
            }

           //Commented By Zoya @23/02/2016
          //  var password = document.getElementById("<%=txtPassword.ClientID%>").value;

        //    if (password.length < 6) {
          //      alert("Password Must Contain Atleast 6 Characters.");
          //      document.getElementById("<%=txtPassword.ClientID%>").focus();
           //     return false;
            //  }

            if (document.getElementById("<%=txtStaffNo.ClientID%>").value.replace(re, "$1").length == 0) {
                alert("Staff No Field Cannot Be Blank");
                document.getElementById("<%=txtStaffNo.ClientID%>").focus();
                return false;
            }
            if (document.getElementById("<%=txtStaffName.ClientID%>").value.replace(re, "$1").length == 0) {
                alert("Staff Name Field Cannot Be Blank");
                document.getElementById("<%=txtStaffName.ClientID%>").focus();
                return false;
            }
            if (document.getElementById("<%=txtDesignation.ClientID%>").value.replace(re, "$1").length == 0) {
                alert("Designation Field Cannot Be Blank");
                document.getElementById("<%=txtDesignation.ClientID%>").focus();
                return false;
            }
            if (document.getElementById("<%=ddlDepartment.ClientID%>").value == "-1") {
                alert("Select a Department");
                document.getElementById("<%=ddlDepartment.ClientID%>").focus();
                return false;
            }
            if (document.getElementById("<%=ddlUserGroup.ClientID%>").value == "-1") {
                alert("Select a Valid User Group");
                document.getElementById("<%=ddlUserGroup.ClientID%>").focus();
                return false;
            }
            if (document.getElementById("<%=txtEmail.ClientID%>").value.replace(re, "$1").length == 0) {
                alert("Email Field Cannot Be Blank");
                document.getElementById("<%=txtEmail.ClientID%>").focus();
                return false;
            }
            var emailPat = /^(\".*\"|[A-Za-z]\w*)@(\[\d{1,3}(\.\d{1,3}){3}]|[A-Za-z]\w*(\.[A-Za-z]\w*)+)$/;
            var emailid = document.getElementById("<%=txtEmail.ClientID %>").value;
            var matchArray = emailid.match(emailPat);
            if (matchArray == null) {
                alert("Enter Valid Email");
                document.getElementById("<%=txtEmail.ClientID %>").focus();
                return false;
            }
            if (document.getElementById("<%=txtExpiryDate.ClientID%>").value.replace(re, "$1").length == 0) {
                alert("Staff Expiry Field Cannot Be Blank");
                document.getElementById("<%=txtExpiryDate.ClientID%>").focus();
                return false;
            }
            return true;
        }

        function CheckDueDate() {
            var digits = "0123456789/";
            var temp;
            for (var i = 0; i < document.getElementById("<%=txtExpiryDate.ClientID %>").value.length; i++) {
                temp = document.getElementById("<%=txtExpiryDate.ClientID%>").value.substring(i, i + 1);
                if (digits.indexOf(temp) == -1) {
                    alert("Enter Valid Date (dd/mm/yyyy)");
                    document.getElementById("<%=txtExpiryDate.ClientID%>").value = "";
                    document.getElementById("<%=txtExpiryDate.ClientID%>").focus();
                    return false;
                }
            }
            return true;
        }

        function getDate2from() {
            popUpCalendar(document.getElementById("<%=ibtnExpiryDate.ClientID%>"), document.getElementById("<%=txtExpiryDate.ClientID%>"), 'dd/mm/yyyy')

        }              

    </script>

    <asp:Panel ID="pnlHeader" runat="server" Width="100%">
        <table style="background-image: url(../images/Sample.png);">
            <tr>
                <td style="width: 4px; height: 14px"></td>
                <td style="width: 14px; height: 14px"></td>
                <td>
                    <table style="border-collapse: collapse;" class="menuoff" onmouseover="className='menuon';"
                        onmouseout="className='menuoff';">
                        <tr>
                            <td style="width: 3%; height: 14px">
                                <asp:ImageButton ID="ibtnNew" runat="server" ImageUrl="~/images/add.png" ToolTip="New" />
                            </td>
                            <td style="width: 3%; height: 14px">
                                <asp:Label ID="lblNew" runat="server" Text="New" meta:resourcekey="Label14Resource1"></asp:Label>
                            </td>
                        </tr>
                    </table>
                </td>
                <td>
                    <table style="border-collapse: collapse;" class="menuoff" onmouseover="className='menuon';"
                        onmouseout="className='menuoff';">
                        <tr>
                            <td style="width: 3%; height: 14px">
                                <asp:ImageButton ID="ibtnSave" runat="server" ImageUrl="~/images/save.png" ToolTip="Save" ValidationGroup="1" />
                            </td>
                            <td style="width: 3%; height: 14px">
                                <asp:Label ID="lblSave" runat="server" Text="Save" meta:resourcekey="Label14Resource1"></asp:Label>
                            </td>
                        </tr>
                    </table>
                </td>
                <td>
                    <table style="border-collapse: collapse;" class="menuoff" onmouseover="className='menuon';"
                        onmouseout="className='menuoff';">
                        <tr>
                            <td style="width: 3%; height: 14px">
                                <asp:ImageButton ID="ibtnOpen" runat="server" ImageUrl="~/images/edit.gif" ToolTip="Open" />
                            </td>
                            <td style="width: 3%; height: 14px">
                                <asp:Label ID="lblOpen" runat="server" Text="Open" meta:resourcekey="Label14Resource1"></asp:Label>
                            </td>
                        </tr>
                    </table>
                </td>
                <td>
                    <table style="border-collapse: collapse;" class="menuoff" onmouseover="className='menuon';"
                        onmouseout="className='menuoff';">
                        <tr>
                            <td style="width: 3%; height: 14px">
                                <asp:ImageButton ID="ibtnDelete" runat="server" ImageUrl="~/images/delete.png" ToolTip="Delete"/>
                            </td>
                            <td style="width: 3%; height: 14px">
                                <asp:Label ID="lblDelete" runat="server" Text="Delete" meta:resourcekey="Label13Resource1"></asp:Label>
                            </td>
                        </tr>
                    </table>
                </td>
                <td>
                    <table style="border-collapse: collapse;" class="menuoff" onmouseover="className='menuon';"
                        onmouseout="className='menuoff';">
                        <tr>
                            <td style="width: 3%; height: 14px">
                                <asp:ImageButton ID="ibtnSearch" runat="server" ImageUrl="~/images/find.png" ToolTip="Search" />
                            </td>
                            <td style="width: 3%; height: 14px">
                                <asp:Label ID="lblSearch" runat="server" Text="Search" meta:resourcekey="Label16Resource1"></asp:Label>
                            </td>
                        </tr>
                    </table>
                </td>
                <td>
                    <table style="border-collapse: collapse;" class="menuoff" onmouseover="className='menuon';"
                        onmouseout="className='menuoff';">
                        <tr>
                            <td style="width: 3%; height: 14px">
                                <asp:ImageButton ID="ibtnRefresh" runat="server" ImageUrl="~/images/refresh.png" ToolTip="Refresh" />
                            </td>
                            <td style="width: 3%; height: 14px">
                                <asp:Label ID="lblRefresh" runat="server" Text="Refresh" meta:resourcekey="Label16Resource1"></asp:Label>
                            </td>
                        </tr>
                    </table>
                </td>
                <td>
                    <table style="border-collapse: collapse;" class="menuoff" onmouseover="className='menuon';"
                        onmouseout="className='menuoff';">
                        <tr>
                            <td style="width: 3%; height: 14px">
                                <asp:ImageButton ID="ibtnCancel" runat="server" ImageUrl="~/images/cancel.png" ToolTip="Cancel" Width="24px" />
                            </td>
                            <td style="width: 3%; height: 14px">
                                <asp:Label ID="lblCancel" runat="server" Text="Cancel" meta:resourcekey="Label18Resource1"></asp:Label>
                            </td>
                        </tr>
                    </table>
                </td>
                <td style="width: 100%; height: 14px"></td>
                <td style="width: 100%; height: 14px"></td>
            </tr>
        </table>
        <table style="width: 100%">
            <tr>
                <td class="vline" style="width: 100%; height: 1px"></td>
            </tr>
        </table>
        <table style="width: 100%">
            <tr>
                <td style="width: 400px">
                    <asp:SiteMapPath ID="SiteMapPath1" runat="server">
                    </asp:SiteMapPath>
                </td>
                <td class="pagetext" style="text-align: right">
                    <asp:Label ID="lblMenuName" runat="server" Width="422px" Text="User"></asp:Label>
                </td>
            </tr>
        </table>
        <table>
            <tr>
                <td style="width: 100px">
                    <asp:Label ID="lblMsg" runat="server" CssClass="lblError" Style="text-align: left" Width="301px"></asp:Label>
                </td>
            </tr>
        </table>
    </asp:Panel>

    <asp:Panel ID="pnlSearch" runat="server" Width="100%">
        <div align="center" style="border: thin solid #A6D9F4; margin: 5px; width: 99%">
            <br />
            <table width="100%" align="center">
                <tr>
                    <td style="height: 43px">
                        <table width="100%">
                            <tr>
                                <td width="10%">
                                    <asp:Label ID="Label9" runat="server" Text="Search"></asp:Label>
                                </td>
                                <td align="left">
                                    <asp:TextBox ID="txtSearch" runat="server" Width="400px"></asp:TextBox>
                                </td>
                            </tr>
                        </table>
                        <br />
                    </td>
                </tr>
            </table>
        </div>
        <div align="center" style="margin: 5px; width: 99%">
            <table width="100%" align="center">
                <tr>
                    <td align="center">
                        <asp:Label ID="lblDataGridMsg" runat="server" CssClass="userMsg"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:DataGrid ID="dgDataGrid" runat="server" AutoGenerateColumns="False"
                            Width="99%" HorizontalAlign="Center" AllowPaging="True">
                            <FooterStyle CssClass="dgFooterStyle" Height="20px" />
                            <PagerStyle HorizontalAlign="Center" Mode="NumericPages" />
                            <SelectedItemStyle CssClass="dgSelectedItemStyle" />
                            <AlternatingItemStyle BackColor="Beige" CssClass="dgAlternatingItemStyle" Font-Bold="False"
                                Font-Italic="False" Font-Overline="False" Font-Strikeout="False" Font-Underline="False" />
                            <ItemStyle CssClass="dgItemStyle" />
                            <HeaderStyle BackColor="#CDD7EE" CssClass="dgHeaderStyle" Font-Bold="True" Font-Italic="False"
                                Font-Overline="False" Font-Size="Medium" Font-Strikeout="False" Font-Underline="False" />
                            <Columns>
                                <asp:TemplateColumn HeaderText="Select">
                                    <ItemTemplate>
                                       <%-- <asp:CheckBox ID="cbSelect" runat="server" AutoPostBack="true" onclick="CheckBoxCheck(this);" />--%>
                                        <asp:CheckBox ID="cbSelect" runat="server" AutoPostBack="true"/>
                                    </ItemTemplate>
                                    <HeaderStyle Width="5%" HorizontalAlign="Center" />
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:TemplateColumn>
                                <asp:BoundColumn DataField="UserID" HeaderText="UserID" Visible="false">
                                    <HeaderStyle Width="5%" HorizontalAlign="Center" />
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:BoundColumn>
                                <asp:BoundColumn DataField="UserName" HeaderText="User Name">
                                    <HeaderStyle Width="15%" HorizontalAlign="Center" />
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:BoundColumn>
                                <asp:BoundColumn DataField="Department" HeaderText="Department">
                                    <HeaderStyle HorizontalAlign="Left" />
                                    <ItemStyle HorizontalAlign="Left" />
                                </asp:BoundColumn>

                               <%-- Edited by Zoya @3/03/2016--%>

                                <%--<asp:BoundColumn DataField="Description" HeaderText="User Group">
                                    <HeaderStyle HorizontalAlign="Left" />
                                    <ItemStyle HorizontalAlign="Left" />
                                </asp:BoundColumn>--%>                              
                                 <asp:BoundColumn DataField="UserGroupName" HeaderText="User Group">
                                    <HeaderStyle HorizontalAlign="Left" />
                                    <ItemStyle HorizontalAlign="Left" />
                                </asp:BoundColumn>

                                <%-- Edited by Zoya @3/03/2016--%>

                                <asp:BoundColumn DataField="UserStatus" HeaderText="Status">
                                    <HeaderStyle HorizontalAlign="Center" />
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:BoundColumn>
                            </Columns>
                        </asp:DataGrid>
                    </td>
                </tr>
            </table>
            <asp:HiddenField ID="hdnUserID" runat="server"></asp:HiddenField>
        </div>
    </asp:Panel>

    <asp:Panel ID="pnlEdit" runat="server" Width="100%" Visible="true">
        <table style="width: 100%">
            <tr>
                <td style="width: 100%">
                    <div style="border: thin solid #A6D9F4; width: 100%">
                        <table class="fields" style="width: 100%; height: 100%">
                            <tr>
                                <td style="width: 6px; height: 26px">
                                    <span style="font-size: 11pt; color: #ff0066">*</span></td>
                                <td class="auto-style9">
                                    <asp:Label ID="Label1" runat="server" Text="User Name" Width="76px"></asp:Label>
                                </td>
                                <td class="auto-style14">
                                    <asp:TextBox ID="txtUserName" runat="server" Width="142px" MaxLength="20"></asp:TextBox>
                                </td>
                                <td class="auto-style7"></td>
                            </tr>
                            <tr>
                                <td style="width: 6px; height: 26px">
                                    <span style="font-size: 11pt; color: #ff0066">*</span></td>
                                <td class="fields" style="width: 166px; height: 26px">
                                    <asp:Label ID="Label4" runat="server" Text="Password" Width="76px"></asp:Label>
                                </td>
                                <td class="auto-style14">
                                    <asp:TextBox ID="txtPassword" runat="server" MaxLength="20" Width="142px" OnTextChanged="txtPassword_TextChanged"></asp:TextBox>
                                </td>
                                <td class="auto-style7"></td>
                            </tr>
                            <tr>
                                <td style="width: 6px; height: 26px">
                                    <span style="font-size: 11pt; color: #ff0066">*</span></td>
                                <td class="fields" style="width: 166px; height: 26px">
                                    <asp:Label ID="Label5" runat="server" Text="Staff No" Width="76px"></asp:Label>
                                </td>
                                <td class="auto-style14">
                                    <asp:TextBox ID="txtStaffNo" runat="server" MaxLength="20" Width="400px"></asp:TextBox>
                                </td>
                                <td class="auto-style7"></td>
                            </tr>
                            <tr>
                                <td style="width: 6px; height: 26px">
                                    <span style="font-size: 11pt; color: #ff0066">*</span></td>
                                <td class="fields" style="width: 166px; height: 26px">
                                    <asp:Label ID="Label6" runat="server" Text="Staff Name" Width="76px"></asp:Label>
                                </td>
                                <td class="auto-style14">
                                    <asp:TextBox ID="txtStaffName" runat="server" MaxLength="300" Width="400px"></asp:TextBox>
                                </td>
                                <td class="auto-style7"></td>
                            </tr>
                            <tr>
                                <td style="width: 6px; height: 26px">
                                    <span style="font-size: 11pt; color: #ff0066">*</span></td>
                                <td class="fields" style="width: 166px; height: 26px">
                                    <asp:Label ID="Label10" runat="server" Text="Designation" Width="76px"></asp:Label>
                                </td>
                                <td class="auto-style14">
                                    <asp:TextBox ID="txtDesignation" runat="server" MaxLength="20" Width="400px"></asp:TextBox>
                                </td>
                                <td class="auto-style7"></td>
                            </tr>
                            <tr>
                                <td style="width: 6px; height: 26px">
                                    <span style="font-size: 11pt; color: #ff0066">*</span></td>
                                <td class="fields" style="width: 166px; height: 26px">
                                    <asp:Label ID="Label12" runat="server" Text="Department" Width="76px"></asp:Label>
                                </td>
                                <td class="auto-style14">
                                    <asp:DropDownList ID="ddlDepartment" runat="server" Width="147px" AutoPostBack="true">
                                    </asp:DropDownList>
                                </td>
                                <td class="auto-style7"></td>
                            </tr>
                            <tr>
                                <td style="width: 6px; height: 26px">
                                    <span style="font-size: 11pt; color: #ff0066">*</span></td>
                                <td class="fields" style="width: 166px; height: 26px">
                                    <asp:Label ID="Label8" runat="server" Text="User Group" Width="76px"></asp:Label>
                                </td>
                                <td class="auto-style14">
                                    <asp:DropDownList ID="ddlUserGroup" runat="server" Width="147px">
                                    </asp:DropDownList>
                                </td>
                                <td class="auto-style7"></td>
                            </tr>
                            <%--edited by Hafiz @ 21/9/2016--%>
                            <%--<tr>
                                <td style="width: 6px; height: 21px">
                                    <span style="font-size: 11pt; color: #ff0066">*</span></td>
                                <td class="fields" style="width: 166px; height: 26px">
                                    <asp:Label ID="Label3" runat="server" Text="Aprroval Group" Width="80px"></asp:Label>
                                </td>
                                <td class="auto-style15">
                                   <table>
                                       <tr>
                                           <td>
                                            <asp:DropDownList ID="ddlApprovalGroup" runat="server" Width="146px" OnSelectedIndexChanged ="ddlApprovalGroup_SelectedIndexChanged" AutoPostBack="true">
                                            <asp:ListItem Value="0">No</asp:ListItem>
                                            <asp:ListItem Value="1">Yes</asp:ListItem>
                                        </asp:DropDownList>  
                                           </td>
                                           <caption>
                                               &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                               <tr>
                                                   <td>
                                                       <asp:RadioButtonList ID="rbApproval" runat="server" RepeatDirection="Horizontal" Visible="false" Width="200px">
                                                           <asp:ListItem Text="Reviewer" Value="R"></asp:ListItem>
                                                           <asp:ListItem Text="Approver" Value="A"></asp:ListItem>
                                                       </asp:RadioButtonList>
                                                   </td>
                                               </tr>
                                           </caption>
                                       </tr>
                                   </table>                   
                                </td>
                                <td class="auto-style8">  
                                </td>
                            </tr>--%>                           
                            
                            <tr>
                                <td style="width: 6px; height: 21px">
                                    <span style="font-size: 11pt; color: #ff0066">*</span></td>
                                <td class="fields" style="width: 166px; height: 21px">
                                    <asp:Label ID="Label2" runat="server" Text="Email" Width="117px"></asp:Label>
                                </td>
                                <td class="auto-style15">
                                    <asp:TextBox ID="txtEmail" runat="server" MaxLength="50" Width="400px"></asp:TextBox>
                                </td>
                                <td class="auto-style8">&nbsp;</td>
                            </tr>     
                            <tr>
                                <td style="width: 6px; height: 21px">
                                    <span style="font-size: 11pt; color: #ff0066">*</span></td>
                                <td class="fields" style="width: 166px; height: 21px">
                                    <asp:Label ID="Label11" runat="server" Style="vertical-align: middle" Text="Staff Expiry"
                                        Width="78px"></asp:Label>
                                </td>
                                <td class="auto-style15">
                                    <span style="font-size: 11pt; color: #ff0066">
                                        <asp:TextBox ID="txtExpiryDate" runat="server" MaxLength="10" Width="102px"></asp:TextBox>
                                    &nbsp;&nbsp;&nbsp;<asp:Image ID="ibtnExpiryDate" runat="server" ImageUrl="~/images/cal.gif" />
                                    </span>
                                </td>
                                <td class="auto-style8"></td>
                            </tr>                       
                            <tr>
                                <td style="width: 6px; height: 21px">
                                    <span style="font-size: 11pt; color: #ff0066">*</span></td>
                                <td class="fields" style="width: 166px; height: 21px">
                                    <asp:Label ID="Label7" runat="server" Style="vertical-align: middle" Text="Status"
                                        Width="78px"></asp:Label>
                                </td>
                                <td class="auto-style15">
                                    <span style="font-size: 11pt; color: #ff0066">
                                        <asp:DropDownList ID="ddlStatus" runat="server" Width="146px">
                                            <asp:ListItem Value="1" Text="Active">Active</asp:ListItem>
                                            <asp:ListItem Value="0" Text="Inactive">Inactive</asp:ListItem>
                                        </asp:DropDownList>
                                    </span>
                                </td>
                                <td class="auto-style8"></td>
                            </tr>                            
                        </table>
                    </div>
                </td>
            </tr>
        </table>
    </asp:Panel>

    <%--<asp:Panel ID="PnlView" runat="server" Width="100%">
        <table style="width: 100%;">            
            <tr>
                <td style="width: 3px;"></td>
                <td>
                    <asp:CheckBox ID="cbSelectAll" runat="server" Text="Select All" onclick="checkAll(this);" Visible="false" />
                </td>
                <td>
                    &nbsp;</td>
            </tr>
            <tr>
                <td style="width: 3px;"></td>
                <td style="vertical-align: text-top;">                  
                    <asp:DataGrid ID="dgRoles" runat="server" AllowSorting="True" AutoGenerateColumns="False" Font-Names="verdana" Font-Size="8pt" 
                        Height="17px" Width="50%" Visible="false">
                        <FooterStyle CssClass="dgFooterStyle" />
                        <ItemStyle CssClass="dgItemStyle" Height="10px" />
                        <SelectedItemStyle CssClass="dgSelectedItemStyle" />
                        <Columns>
                            <asp:BoundColumn DataField="MenuID" HeaderText="Menu ID" Visible="False">
                                <HeaderStyle Font-Bold="True" Font-Italic="False" Font-Overline="False" Font-Strikeout="False" Font-Underline="False" />
                            </asp:BoundColumn>
                            <asp:BoundColumn DataField="MenuName" HeaderText="Menu Name" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="100px" Visible="false">
                                <HeaderStyle Font-Bold="True" Font-Italic="False" Font-Overline="False" Font-Strikeout="False" Font-Underline="False" Width="100px" HorizontalAlign="Center" />
                            </asp:BoundColumn>
                            <asp:BoundColumn DataField="PageName" HeaderStyle-Width="100px" HeaderText="Process Page Name" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="100px" HeaderStyle-HorizontalAlign="Center"></asp:BoundColumn>
                            <asp:BoundColumn DataField="IsAdd" HeaderText="IsAdd" Visible="False"></asp:BoundColumn>                            
                            <asp:TemplateColumn HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="20px" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="20px">
                                <HeaderTemplate>
                                    <asp:Label ID="lblAdd" runat="server" Font-Bold="True" Text="Select"></asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:CheckBox ID="Add" runat="server" classname="classAdd" name="chkAdd" />
                                </ItemTemplate>
                            </asp:TemplateColumn>                            
                        </Columns>
                        <AlternatingItemStyle BackColor="Beige" CssClass="dgAlternatingItemStyle" Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False" Font-Underline="False" />
                        <HeaderStyle BackColor="#CDD7EE" CssClass="dgHeaderStyle" Font-Bold="True" Font-Italic="False" Font-Overline="False" Font-Size="Medium" Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Center" />
                    </asp:DataGrid>
                </td>
                <td style="vertical-align: text-top;">&nbsp;</td>
            </tr>
        </table>
        <asp:Label ID="userGroupID" runat="server"></asp:Label>
    </asp:Panel>--%>

</asp:Content>
<asp:Content ID="Content2" runat="server" contentplaceholderid="head">
    <style type="text/css">
        .auto-style7 {
            height: 26px;
        }
        .auto-style8 {
            height: 21px;
        }
        .auto-style9 {
            height: 26px;
            width: 166px;
        }
        .auto-style14 {
            height: 26px;
            width: 685px;
        }
        .auto-style15 {
            height: 21px;
            width: 685px;
        }
    </style>
</asp:Content>

