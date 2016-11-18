<%@ Page Language="VB" MasterPageFile="~/MasterPage3.master" AutoEventWireup="false" MaintainScrollPositionOnPostback="true"
    CodeFile="HostelFee.aspx.vb" Inherits="HostelFee" Title="Welcome To SAS" %>

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
       
        function checkValue() {

            if (((event.keyCode < 48) || (event.keyCode > 57)) && (event.keyCode != 13) && (event.keyCode != 46)) {
                alert("Enter Valid Amount");
                event.preventDefault();
                return false;
            }
        }
        function getconfirm() {

            if (document.getElementById("<%=ddlHostelCOde.ClientID%>").value == "-1") {
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
        function validate() {
            var re = /\s*((\S+\s*)*)/;
            if (document.getElementById("<%=ddlHostelCOde.ClientID%>").value == "-1") {
                alert("Select a Kolej");
                document.getElementById("<%=ddlHostelCOde.ClientID%>").focus();
                return false;
            }
            if (document.getElementById("<%=ddlBloackCode.ClientID%>").value == "-1") {
                alert("Select a Block");
                document.getElementById("<%=ddlBloackCode.ClientID%>").focus();
                return false;
            }
            var hfValidateAmt = document.getElementById("<%=hfValidateAmt.ClientID%>").value
            if (hfValidateAmt == "True") {
                alert("Invalid Fee Amount not Accepted");
                return false;
            }

            // if (document.getElementById("<%=ddlRoomType.ClientID%>").value == "-1") {
            //    alert("Select a Room Type");
            //    document.getElementById("<%=ddlRoomType.ClientID%>").focus();
            //     return false;
            //}
            return true;
        }
    </script>
    <%--<atlas:ScriptManager ID="scriptmanager1" EnablePartialRendering="true" runat="Server" />
   <atlas:UpdatePanel ID="up1" runat="server">
<ContentTemplate>--%>
    <asp:Panel ID="Panel1" runat="server" Width="100%">
        <table style="background-image: url(images/Sample.png);">
            <tr>
                <td style="width: 4px; height: 14px"></td>
                <td style="width: 14px; height: 14px"></td>
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
                                <asp:ImageButton ID="ibtnDelete" runat="server" ImageUrl="~/images/delete.png" />
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
                <%--Done Editted By Zoya @25/02/2016--%>
                <%-- Editted By Zoya @24/02/2016--%>
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
                <td style="float: left;display:None; visibility:collapse;">
                    <table style="border-collapse: collapse;" class="menuoff" onmouseover="className='menuon';"
                        onmouseout="className='menuoff';">
                        <tr>
                            <td>
                                <asp:ImageButton ID="ibtnOthers" runat="server" ImageUrl="~/images/gothers.gif" ToolTip="Cancel" Visible="false"/>
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
                    <asp:TextBox ID="txtRecNo" runat="server" disabled="disabled" TabIndex="1" dir="ltr"
                        Width="52px" ReadOnly="true" CssClass="text_box" Style="text-align: right" AutoPostBack="True"
                        MaxLength="7"></asp:TextBox>
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
                <td style="width: 100%; height: 14px"></td>
                <td style="width: 100%; height: 14px"></td>
            </tr>
        </table>
    </asp:Panel>
    <table style="width: 100%">
        <tr>
            <td class="vline" style="width: 746px; height: 1px"></td>
        </tr>
    </table>
    <table style="width: 100%">
        <tr>
            <td style="width: 400px">
                <asp:SiteMapPath ID="SiteMapPath1" runat="server">
                </asp:SiteMapPath>
            </td>
            <td class="pagetext" style="text-align: right">
                <asp:Label ID="lblMenuName" runat="server" Width="350px"></asp:Label>
            </td>
        </tr>
    </table>
    <%--  </ContentTemplate>     
</atlas:UpdatePanel>
<atlas:UpdateProgress ID="ProgressIndicator" runat="server">
    <ProgressTemplate>
        Loading the data, please wait... 
        <asp:Image ID="LoadingImage" ImageAlign="AbsMiddle" runat="server" ImageUrl="~/Images/spinner.gif" />        
    </ProgressTemplate>
 </atlas:UpdateProgress>
<atlas:UpdatePanel ID="up2" runat="server">
<ContentTemplate>--%>
    <asp:Panel ID="pnlAdd" runat="server" Width="100%" Visible="True">
        <table style="width: 100%">
            <tr>
                <td style="width: 100%">
                    <div style="border: thin solid #A6D9F4; width: 100%">
                        <table class="fields" style="width: 99%; height: 77%;">
                            <tr>
                                <td style="width: 2px; height: 26px"></td>
                                <td class="fields" style="width: 156px; height: 26px"></td>
                                <td style="height: 26px; text-align: center" colspan="2">
                                    <asp:Label ID="lblMsg" runat="server" CssClass="lblError" Width="348px"></asp:Label>
                                </td>
                                <td style="width: 131px; height: 26px">
                                    <asp:HiddenField ID="hfValidateAmt" runat="server" />
                                </td>
                            </tr>
                            <tr style="height: 26px">
                                <td style="width: 2px;" valign="middle">
                                    <span style="font-size: 11pt; color: #ff0066">*</span></td>
                                <td class="fields" style="width: 156px;">
                                    <asp:Label ID="Label1" runat="server" Text="Kolej"></asp:Label>
                                </td>
                                <td style="width: 18px;">
                                    <asp:DropDownList ID="ddlHostelCOde" runat="server" Width="353px" AutoPostBack="True"
                                        AppendDataBoundItems="True">
                                        <asp:ListItem Value="-1">--Select--</asp:ListItem>
                                    </asp:DropDownList>

                                </td>
                                <td style="width: 252px; vertical-align: middle;">
                                    <span style="font-size: 11pt; color: #ff0066">
                                        <asp:TextBox ID="txtBlockCode" runat="server" Width="142px" Enabled="False" Visible="False"></asp:TextBox></span>
                                </td>
                                <td style="width: 131px;">&nbsp;
                                </td>
                            </tr>
                            <tr style="height: 26px">
                                <td style="width: 2px; height: 15px" valign="middle">
                                    <span style="font-size: 11pt; color: #ff0066">*</span></td>
                                <td class="fields" style="width: 156px; height: 15px">
                                    <asp:Label ID="Label2" runat="server" Text="Block"></asp:Label>
                                </td>
                                <td style="width: 18px; height: 15px">
                                    <asp:DropDownList ID="ddlBloackCode" runat="server" Width="353px" AutoPostBack="True"
                                        AppendDataBoundItems="True">
                                        <asp:ListItem Value="-1">--Select--</asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                                <td style="width: 252px; height: 15px">
                                    <span style="font-size: 11pt; color: #ff0066">
                                        <asp:TextBox ID="txtBlock" runat="server" Width="142px" Enabled="False" Visible="False"></asp:TextBox></span>
                                </td>
                                <td style="width: 131px; height: 15px">&nbsp;
                                </td>
                            </tr>
                            <tr style="display: none; visibility: collapse;">
                                <td style="width: 2px; height: 15px" valign="middle">
                                    <span style="font-size: 11pt; color: #ff0066">*</span></td>
                                <td class="fields" style="width: 156px; height: 15px">
                                    <asp:Label ID="Label4" runat="server" Text="Room Type" Width="83px"></asp:Label>
                                </td>
                                <td style="width: 18px; height: 15px">
                                    <asp:DropDownList ID="ddlRoomType" runat="server" Width="353px" AutoPostBack="True"
                                        AppendDataBoundItems="True">
                                        <asp:ListItem Value="-1">--Select--</asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                                <td style="width: 252px; height: 15px">
                                    <span style="font-size: 11pt; color: #ff0066">
                                        <asp:TextBox ID="txtRoomType" runat="server" Width="142px" Enabled="False" Visible="False"></asp:TextBox></span>
                                </td>
                                <td style="width: 131px; height: 15px">&nbsp;
                                </td>
                            </tr>
                            <%--  <tr style="height: 26px">
                           <td style="width: 2px; height: 15px;">
                            </td>
                            <td class="fields" style="width: 156px; height: 15px;">
                                <asp:Label ID="Label3" runat="server" Text="Effective From" Width="101px"></asp:Label></td>
                            <td style="width: 18px; height: 15px;">
                                <asp:DropDownList ID="ddlEFrom" runat="server" Width="147px" DataSourceID="oDsEffFrom" DataTextField="SAST_Semester" DataValueField="SAST_Code" AutoPostBack="True" AppendDataBoundItems="True">
                                    <asp:ListItem Value="-1">--Select--</asp:ListItem>
                                </asp:DropDownList><span style="font-size: 11pt; color: #ff0066">*</span></td>
                            <td style="width: 250px; height: 15px;">
                                <span style="font-size: 11pt; color: #ff0066">&nbsp;</span></td>
                            <td style="width: 131px; height: 15px;">
                                &nbsp;</td>
                        </tr>--%>
                            <tr>
                                <td style="width: 2px; height: 15px"></td>
                                <td class="fields" style="width: 156px; height: 15px">
                                    <asp:Label ID="Label8" runat="server" Style="vertical-align: middle" Text="Status"
                                        Width="78px"></asp:Label>
                                </td>
                                <td style="width: 18px; height: 15px">
                                    <asp:DropDownList ID="ddlStatus" runat="server" Width="146px">
                                        <asp:ListItem Value="1">Active</asp:ListItem>
                                        <asp:ListItem Value="0">Inactive</asp:ListItem>
                                    </asp:DropDownList>
                                    <asp:TextBox ID="txtHostelFeeCode" runat="server" Width="142px" Visible="False"></asp:TextBox>
                                </td>
                                <td style="width: 252px; height: 15px">
                                    <asp:ImageButton ID="ibtnAddFeeType" runat="server" ToolTip="Add" ImageUrl="~/images/addrec.gif"
                                        Height="26px" Width="28px" />
                                    <asp:ImageButton ID="ibtnRemoveFee" runat="server" Height="26px" ImageUrl="~/images/removey.gif"
                                        ToolTip="Remove" Width="28px" />
                                </td>
                                <td style="width: 131px; height: 15px"></td>
                            </tr>
                        </table>
                    </div>
                </td>
            </tr>
        </table>
    </asp:Panel>
    <br />
    <asp:Panel ID="pnlView" runat="server" Width="100%">
        <table style="width: 100%">
            <tr>
                <td style="width: 71%; height: 149px; vertical-align: top;">
                    <asp:DataGrid ID="dgViewHostelFee" runat="server" AutoGenerateColumns="False" Width="100%"
                        Height="123px" Style="vertical-align: text-top" DataKeyField="FTCode">
                        <FooterStyle CssClass="dgFooterStyle" Height="20px" />
                        <SelectedItemStyle CssClass="dgSelectedItemStyle" />
                        <AlternatingItemStyle BackColor="Beige" CssClass="dgAlternatingItemStyle" Font-Bold="False"
                            Font-Italic="False" Font-Overline="False" Font-Strikeout="False" Font-Underline="False" />
                        <ItemStyle CssClass="dgItemStyle" />
                        <HeaderStyle BackColor="#CDD7EE" CssClass="dgHeaderStyle" Font-Bold="True" Font-Italic="False"
                            Font-Overline="False" Font-Size="Medium" Font-Strikeout="False" Font-Underline="False" />
                        <Columns>
                            <asp:ButtonColumn CommandName="Select" DataTextField="FTCode" HeaderText="Fee Code"
                                Text="FTCode"></asp:ButtonColumn>
                            <asp:BoundColumn DataField="Description" HeaderText="Fee Desc"></asp:BoundColumn>
                            <asp:BoundColumn DataField="Priority" HeaderText="Priority" Visible="false"></asp:BoundColumn>
                            <asp:BoundColumn DataField="HostelStructureCode" HeaderText="sahs_code" Visible="false"></asp:BoundColumn>                            
                            <asp:TemplateColumn HeaderText="Fee Amount (Local)">
                                <ItemTemplate>
                                    <asp:TextBox ID="txtFeeAmountLocal" runat="server" Width="67px" AutoPostBack="true" OnTextChanged="txtFeeAmountLocal_TextChanged" Text='<%# GSTFunc2(Eval("LocalAmount"), Eval("LocalGSTAmount"), Eval("TaxId"))%>'></asp:TextBox>
                                </ItemTemplate>
                            </asp:TemplateColumn>
                            <asp:TemplateColumn HeaderText="Actual Fee Amount (Local)">
                                <ItemTemplate>
                                   <%# Eval("LocalTempAmount")%>
                                </ItemTemplate>
                            </asp:TemplateColumn>
                            <asp:BoundColumn DataField="LocalGSTAmount" HeaderText="GST Amount (Local)"></asp:BoundColumn>
                            <asp:BoundColumn DataField="LocalAmount" HeaderText="Total Fee Amount (Local)"></asp:BoundColumn>
                            <asp:TemplateColumn ItemStyle-BackColor="DarkBlue"></asp:TemplateColumn>
                            <asp:TemplateColumn HeaderText="Fee Amount (International)">
                                <ItemTemplate>
                                    <asp:TextBox ID="txtFeeAmountInter" runat="server" Width="67px" AutoPostBack="true" OnTextChanged="txtFeeAmountInter_TextChanged" Text='<%# GSTFunc2(Eval("NonLocalAmount"), Eval("NonLocalGSTAmount"), Eval("TaxId"))%>'></asp:TextBox>
                                </ItemTemplate>
                            </asp:TemplateColumn>
                            <asp:TemplateColumn HeaderText="Actual Fee Amount (International)">
                                <ItemTemplate>
                                    <%# Eval("NonLocalTempAmount")%>
                                </ItemTemplate>
                            </asp:TemplateColumn>
                            <asp:BoundColumn DataField="NonLocalGSTAmount" HeaderText="GstAmount (International)"></asp:BoundColumn>
                            <asp:BoundColumn DataField="NonLocalAmount" HeaderText="Total Fee Amount (International)"></asp:BoundColumn>
                            <asp:BoundColumn DataField="TaxId" HeaderText="TaxId" Visible="false"></asp:BoundColumn>
                        </Columns>
                    </asp:DataGrid>
                    &nbsp;&nbsp;
                    <asp:Label ID="Label3" runat="server" Text="Hostel Fee Code:" Width="103px" Visible="False"></asp:Label>
                    &nbsp;
                </td>
            </tr>

        </table>
    </asp:Panel>
    <asp:Button ID="btnHidden" runat="Server" OnClick="btnHidden_Click" Style="display: none" />
    <%--  </ContentTemplate>     
</atlas:UpdatePanel>--%>
</asp:Content>
