<%@ Page Language="VB" MasterPageFile="~/MasterPage3.master" AutoEventWireup="false" MaintainScrollPositionOnPostback="true"
    CodeFile="WorkflowSetup.aspx.vb" Inherits="WorkflowSetup" Title="Welcome To SAS" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
     <script language="javascript" type="text/javascript">

         function validate() {

             if (document.getElementById("<%=ddlProcess.ClientID%>").value == "-1") {
                 alert("Select Process Required");
                 document.getElementById("<%=ddlProcess.ClientID%>").focus();
                 return false;
             }
         }
       
         function getconfirm()
         {
             if (confirm("Do you want to Delete Record?")) {
                 return true;
             }
             else {
                 return false;
             }
         }

         function CheckAll(oCheckbox) {
             var gv = document.getElementById("<%=gvUser.ClientID %>");
              for (i = 1; i < gv.rows.length; i++) {
                  gv.rows[i].cells[0].getElementsByTagName("INPUT")[0].checked = oCheckbox.checked;
              }
         }

         function WorkflowExisted()
         {
             var ans = "";

             if (confirm("Workflow Setup Already Existed. Continue the process will overwrite the process. Proceed?"))
             {
                 ans = "Yes";
             }
             else
             {
                 ans = "No";
             }

             __doPostBack('CustomPostBack', ans);
         }

    </script>
    <asp:Panel ID="Panel1" runat="server" Width="100%">
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
                                <asp:ImageButton ID="ibtnNew" runat="server" ImageUrl="images/add.png" ToolTip="New" />
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
                <td>
                    <table style="border-collapse: collapse;" class="menuoff" onmouseover="className='menuon';"
                        onmouseout="className='menuoff';">
                        <tr>
                            <td>
                                <asp:ImageButton ID="ibtnDelete" runat="server" ImageUrl="~/images/gdelete.png" ToolTip="Delete" />
                            </td>
                            <td>
                                <asp:Label ID="Label13" runat="server" Text="Delete" ></asp:Label>
                            </td>
                        </tr>
                    </table>
                </td>
                <td>
                    <table style="border-collapse: collapse;" class="menuoff" onmouseover="className='menuon';"
                        onmouseout="className='menuoff';">
                        <tr>
                            <td>
                                <asp:ImageButton ID="ibtnView" runat="server" ImageUrl="~/images/find.png" ToolTip="View" />
                            </td>
                            <td class="auto-style3">
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
                                <asp:ImageButton ID="ibtnPrint" runat="server" ImageUrl="~/images/gprint.png" ToolTip="Print" Visible="false"/>
                            </td>
                            <td>
                                <asp:Label ID="Label17" runat="server" Text="Print" Visible="false"></asp:Label>
                            </td>
                        </tr>
                    </table>
                </td>
                <td style="float: left;display:None; visibility:collapse;">
                    <table style="border-collapse: collapse;" class="menuoff" onmouseover="className='menuon';"
                        onmouseout="className='menuoff';">
                        <tr>
                            <td>
                                <asp:ImageButton ID="ibtnPosting" runat="server" ImageUrl="~/images/gposting.png"
                                    ToolTip="Cancel" Visible="false"/>
                            </td>
                            <td>
                                <asp:Label ID="Label6" runat="server" Text="Posting" Visible="false"></asp:Label>
                            </td>
                        </tr>
                    </table>
                </td>
                <%--Done Editted By Zoya @25/02/2016--%>

                <%-- Editted By Zoya @24/02/2016--%>
                <td style="float: left;display:None; visibility:collapse;">
                    <table style="border-collapse: collapse;" class="menuoff" onmouseover="className='menuon';"
                        onmouseout="className='menuoff';">
                        <tr>
                            <td>
                                <asp:ImageButton ID="ibtnOthers" runat="server" ImageUrl="~/images/others.png" ToolTip="Cancel" Visible="false"/>
                            </td>
                            <td>
                                <asp:Label ID="Label5" runat="server" Text="Others" Visible="false"></asp:Label>
                            </td>
                        </tr>
                    </table>
                </td>
                 <%-- Done Editted By Zoya @24/02/2016--%>
                <td>
                    <table style="border-collapse: collapse;" class="menuoff" onmouseover="className='menuon';"
                        onmouseout="className='menuoff';">
                        <tr>
                            <td>
                                <asp:ImageButton ID="ibtnCancel" runat="server" ImageUrl="~/images/cancel.png" ToolTip="Cancel"  />
                            </td>
                            <td>
                                <asp:Label ID="Label18" runat="server" Text="Cancel" ></asp:Label>
                            </td>
                        </tr>
                    </table>
                </td>
                <td style="width: 3%; height: 14px">
                </td>
                <td style="width: 2%; height: 14px">
                    <asp:ImageButton ID="ibtnFirst" runat="server" ImageUrl="~/images/new_last.png" Visible="false" />
                </td>
                <td style="width: 2%; height: 14px">
                    <asp:ImageButton ID="ibtnPrevs" runat="server" ImageUrl="~/images/new_prev.png" Visible="false" />
                </td>
                <td style="width: 2%; height: 14px">
                    <asp:TextBox ID="txtRecNo" runat="server" disabled="disabled" TabIndex="1" dir="ltr"
                        Width="52px" AutoPostBack="True" Style="text-align: right" MaxLength="7" ReadOnly="true"
                        CssClass="text_box" Visible="false"></asp:TextBox>
                </td>
                <td style="width: 2%; height: 14px">
                    <asp:Label ID="Label47" runat="server" Visible="false">Of</asp:Label>
                </td>
                <td style="width: 2%; height: 14px">
                    <asp:Label ID="lblCount" runat="server" Width="20px" Visible="false"></asp:Label>
                </td>
                <td style="width: 2%; height: 14px">
                    <asp:ImageButton ID="ibtnNext" runat="server" ImageUrl="~/images/new_next.png" Visible="false" />
                </td>
                <td style="width: 2%; height: 14px">
                    <asp:ImageButton ID="ibtnLast" runat="server" ImageUrl="~/images/new_first.png" Visible="false" />
                </td>
                <td style="width: 8%; height: 14px">
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
                <td style="width: 589px; height: 39px;">
                    <asp:SiteMapPath ID="SiteMapPath1" runat="server">
                    </asp:SiteMapPath>
                </td>
                <td class="pagetext" style="height: 39px" align="right">
                    <asp:Label ID="lblMenuName" runat="server" Width="350px"></asp:Label>
                </td>
            </tr>
        </table>
    </asp:Panel>

    <asp:Panel ID="PnlAdd" runat="server" Width="100%" Visible="true">
        <table style="width: 100%">
            <tr>
                <td style="width: 100%">
                  <div  style="border: thin solid #A6D9F4; width: 100%">
                        <table class="fields" style="width: 100%; height: 100%;">
                            <tr>
                                <td style="width: 3px; height: 26px">
                                </td>
                                <td class="auto-style4">
                                </td>
                                <td class="auto-style5">
                                    <asp:Label ID="lblMsg" runat="server" CssClass="lblError" Style="text-align: center"
                                        Width="348px"></asp:Label>
                                </td>
                                <td style="width: 100px; height: 26px">
                                </td>
                            </tr>

                             <tr>
                                <td style="width: 3px; height: 26px"><span style="font-size: 11pt; color: #ff0066">*</span></td>
                                <td class="fields" style="width: 131px; height: 26px">
                                    <asp:Label ID="Label3" runat="server" Text="Process" Width="128px"></asp:Label>
                                </td>
                                <td class="auto-style5">
                                    <asp:DropDownList ID="ddlProcess" runat="server" Width="266px" OnSelectedIndexChanged="ddlProcess_SelectedIndexChanged" AutoPostBack="true">
                                    </asp:DropDownList>
                                </td>
                                <td style="width: 100px; height: 26px"></td>
                            </tr>

                            <tr>
                                <td style="width: 3px; height: 26px"><span style="font-size: 11pt; color: #ff0066">*</span></td>
                                <td class="fields" style="width: 131px; height: 21px">                                                                     
                                    <asp:Label ID="Label7" runat="server" Style="vertical-align: middle" Text="Status" Width="78px"></asp:Label>
                                </td>
                                <td class="auto-style6">
                                    <asp:DropDownList ID="ddlStatus" runat="server" Width="133px">
                                        <asp:ListItem Value="1">Active</asp:ListItem>
                                        <asp:ListItem Value="0">Inactive</asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                                <td style="width: 100px; height: 21px">
                                </td>
                            </tr>

                            <tr>
                                <td style="width: 3px; height: 21px">
                                </td>
                                <td class="fields" style="width: 131px; height: 21px">
                                </td>
                                <td class="auto-style6">
                                </td>
                                <td style="width: 10px; height: 21px">
                                </td>
                            </tr>
                        </table>
                   </div>
                </td>
            </tr>
        </table>
    </asp:Panel>
    
    <asp:Panel ID="PnlUser" runat="server" Width="100%">
        <table style="width: 100%;">
            <tr>
                <td style="width: 1px; height: 21px"></td>
                <td class="fields" style="width: 100%; height: 21px">

                    <asp:GridView ID="gvUser" runat="server" AutoGenerateColumns="false" OnRowDataBound="gvUser_RowDataBound" 
                        Width="100%" Style="vertical-align: text-top">

                        <FooterStyle CssClass="dgFooterStyle" Height="20px" />
                        <SelectedRowStyle CssClass="dgSelectedItemStyle" />
                        <AlternatingRowStyle BackColor="Beige" CssClass="dgAlternatingItemStyle" Font-Bold="False"
                            Font-Italic="False" Font-Overline="False" Font-Strikeout="False" Font-Underline="False" />
                        <RowStyle CssClass="dgItemStyle" HorizontalAlign="Center" />
                        <HeaderStyle BackColor="#00699b" CssClass="dgHeaderStyle" ForeColor="#ffffff" Font-Bold="False" Font-Italic="False"
                            Font-Overline="False" Font-Size="8pt" Font-Strikeout="False" Font-Underline="False" />
                        <Columns>
                            <asp:TemplateField HeaderText="Select">
                                <HeaderTemplate>
                                    <input id="cbSelectAll" type="checkbox" onclick="CheckAll(this);" />
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:CheckBox ID="cbSelect" runat="server" AutoPostBack="true" OnCheckedChanged="cbSelect_CheckedChanged"></asp:CheckBox>
                                </ItemTemplate>
                                <HeaderStyle Width="5%" />
                            </asp:TemplateField>
                            <asp:BoundField DataField="UsersEn.UserId" HeaderText="UserId" />
                            <asp:BoundField DataField="UsersEn.UserName" HeaderText="Username" />
                            <asp:BoundField DataField="UsersEn.StaffName" HeaderText="Staff Name" />
                            <asp:BoundField DataField="UsersEn.StaffId" HeaderText="Staff No" />
                            <asp:BoundField DataField="UserGroupsEn.UserGroupName" HeaderText="Usergroup" />
                            <asp:TemplateField>
                                <HeaderTemplate>
                                    <asp:Label ID="lblPreparer" runat="server" Font-Bold="True" Text="Preparer" Width="52px"></asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:CheckBox name="chkPreparer" ID="Preparer" runat="server" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <HeaderTemplate>
                                    <asp:Label ID="lblApprover" runat="server" Font-Bold="True" Text="Approver" Width="52px"></asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:CheckBox name="chkApprover" ID="Approver" runat="server" OnCheckedChanged="chkApprover_CheckedChanged" AutoPostBack="true" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Transaction Limit" ItemStyle-Width="120px">
                            <ItemTemplate>
                                <table>
                                    <tr>
                                        <td class="WFuser">Lower Limit</td>
                                        <td class="WFuser"><asp:TextBox ID="txtLowerLim" runat="server" Width="30px" Enabled="false" Text='<%#(Eval("WorkflowApproverEn.LowerLimit"))%>' 
                                            AutoPostBack="true" OnTextChanged="LowerLimit_TextChanged" /></td>
                                    </tr>
                                    <tr>
                                        <td>Upper Limit</td>
                                        <td><asp:TextBox ID="txtUpperLim" runat="server" Width="30px" Enabled="false" Text='<%#(Eval("WorkflowApproverEn.UpperLimit"))%>' 
                                            AutoPostBack="true" OnTextChanged="UpperLimit_TextChanged" /></td>
                                    </tr>
                                </table>

                                <%--<asp:TextBox ID="Slider1" runat="server" AutoPostBack="true" />
                                <asp:TextBox ID="SliderValue" runat="server" AutoPostBack="true" />
                                <ajaxToolkit:SliderExtender ID="se1" runat="server" TargetControlId="Slider1" BoundControlID="SliderValue" />--%>

                            </ItemTemplate>
                        </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </td>
                <td style="width: 100px; height: 21px"></td>
            </tr>
        </table>
    </asp:Panel>

    <asp:Panel ID="PnlView" runat="server" Width="100%">
        <table style="width:100%;">
            <tr>
                <td style="width: 100%">
                    <%--<div  style="border: thin solid #A6D9F4; width: 100%">--%>
                    <fieldset style="border: thin solid #A6D9F4;">
                        <legend><strong>Filter</strong></legend><br />
                        <table class="fields" style="width: 100%; height: 100%;">
                            <tr>
                                <td style="width: 3px; height: 26px"></td>
                                <td class="fields" style="width: 131px; height: 21px">                                                                     
                                    <asp:Label ID="Label4" runat="server" Style="vertical-align: middle" Text="Status" Width="78px"></asp:Label>
                                </td>
                                <td class="auto-style6">
                                    <asp:DropDownList ID="ddlSearchStats" runat="server" Width="133px" 
                                        OnSelectedIndexChanged="ddlSearchStats_SelectedIndexChanged" AutoPostBack="true">
                                        <asp:ListItem Value="1">Active</asp:ListItem>
                                        <asp:ListItem Value="0">Inactive</asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                                <td style="width: 100px; height: 21px">
                                </td>
                            </tr>

                            <tr>
                                <td style="width: 3px; height: 21px">
                                </td>
                                <td class="fields" style="width: 131px; height: 21px">
                                </td>
                                <td class="auto-style6">
                                </td>
                                <td style="width: 10px; height: 21px">
                                </td>
                            </tr>
                        </table>
                    </fieldset>
                    <%--</div>--%>
                </td>
            </tr>
        </table>
        <table style="width: 100%;">
            <tr>
                <td style="width: 1px; height: 21px"></td>
                <td class="fields" style="width: 100%; height: 21px">
                    <asp:GridView ID="gvView" runat="server" AutoGenerateColumns="false" Width="100%" OnItemCommand="gvView_ItemCommand"
                        Style="vertical-align: text-top">
                         <FooterStyle CssClass="dgFooterStyle" Height="20px" />
                        <SelectedRowStyle CssClass="dgSelectedItemStyle" />
                        <AlternatingRowStyle BackColor="Beige" CssClass="dgAlternatingItemStyle" Font-Bold="False"
                            Font-Italic="False" Font-Overline="False" Font-Strikeout="False" Font-Underline="False" />
                        <RowStyle CssClass="dgItemStyle" HorizontalAlign="Center" />
                        <HeaderStyle BackColor="#00699b" CssClass="dgHeaderStyle" ForeColor="#ffffff" Font-Bold="False" Font-Italic="False"
                            Font-Overline="False" Font-Size="8pt" Font-Strikeout="False" Font-Underline="False" />
                        <Columns>
                            <asp:TemplateField HeaderText="Page Name">
                                <ItemTemplate>
                                    <asp:LinkButton ID="imgBtn1" CssClass="SelectRow" runat="server"
                                        Text='<%#(Eval("MenuMasterEn.PageName"))%>' CommandName="view" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <%--<asp:BoundField DataField="ProcessName" HeaderText="Process Name" />--%>
                            <asp:BoundField DataField="Status" HeaderText="Status" />
                            <asp:BoundField DataField="TotalPreparer" HeaderText="Total Preparer" />
                            <asp:BoundField DataField="TotalApprover" HeaderText="Total Approver" />
                            <asp:BoundField DataField="LastUpdatedBy" HeaderText="Last Updated By" />
                            <asp:BoundField DataField="LastUpdatedDtTm" HeaderText="Last Updated Date & Time" />
                        </Columns>
                    </asp:GridView>
                </td>
                <td style="width: 100px; height: 21px"></td>
            </tr>
        </table>
    </asp:Panel>
</asp:Content>


<asp:Content ID="Content2" runat="server" contentplaceholderid="head">
    <style type="text/css">
        .auto-style3
        {
            width: 36px;
        }
        .auto-style4 {
            height: 26px;
            width: 131px;
        }
        .auto-style5 {
            height: 26px;
            width: 658px;
        }
        .auto-style6 {
            height: 21px;
            width: 658px;
        }

        td.WFuser {
            text-align: left;
            border-bottom: 1px solid #ddd;
        }
    </style>
</asp:Content>



