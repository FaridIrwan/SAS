<%@ Page Language="VB" MasterPageFile="~/MasterPage3.master" AutoEventWireup="false" MaintainScrollPositionOnPostback="true"
    CodeFile="Semester.aspx.vb" Inherits="semester" Title="Welcome to SAS" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <script language="javascript" type="text/javascript">
        function geterr() {
            var digits = "0123456789";
            var temp;
            for (var i = 0; i < document.getElementById("<%=txtRecNo.ClientID %>").value.length; i++) {
                temp = document.getElementById("<%=txtRecNo.ClientID%>").value.substring(i, i + 1);
                if (digits.indexOf(temp) == -1) {
                    alert("Please Enter correct Record No");
                    document.getElementById("<%=txtRecNo.ClientID%>").value = 1;
                    document.getElementById("<%=txtRecNo.ClientID%>").focus();
                    return false;
                }
            }
            return true;
        }
        
    </script>
    <%--  </ContentTemplate> </atlas:UpdatePanel>--%>
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
                <%-- Number1- Editted By Zoya @27/02/2016--%>
                <td style="float: left;display:None; visibility:collapse;">
                    <table style="border-collapse: collapse;" class="menuoff" onmouseover="className='menuon';"
                        onmouseout="className='menuoff';">
                        <tr>
                            <td>
                                <asp:ImageButton ID="ibtnDelete" runat="server" ImageUrl="~/images/delete.png" Visible="false"/>
                            </td>
                            <td>
                                <asp:Label ID="Label13" runat="server" Text="Delete" Visible="false"></asp:Label>
                            </td>
                        </tr>
                    </table>
                </td>
                <td style="float: left;display:None; visibility:collapse;">
                    <table style="border-collapse: collapse;" class="menuoff" onmouseover="className='menuon';"
                        onmouseout="className='menuoff';">
                        <tr>
                            <td>
                                <asp:ImageButton ID="ibtnView" runat="server" ImageUrl="~/images/find.png" ToolTip="View" Visible="false"/>
                            </td>
                            <td>
                                <asp:Label ID="Label16" runat="server" Text="Search" Visible="false"></asp:Label>
                            </td>
                        </tr>
                    </table>
                </td>
                <%--Number 1- Done Editted By Zoya @27/02/2016--%>

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

                 <%-- Number2- Editted By Zoya @27/02/2016--%>
                <td style="float: left;display:None; visibility:collapse;width: 2%; height: 14px">
                    <asp:ImageButton ID="ibtnFirst" runat="server" ImageUrl="~/images/new_last.png" Visible="false"/>
                </td>
                <td style="float: left;display:None; visibility:collapse;width: 2%; height: 14px">
                    <asp:ImageButton ID="ibtnPrevs" runat="server" ImageUrl="~/images/new_prev.png" Visible="false"/>
                </td>
                <td style="float: left;display:None; visibility:collapse;width: 2%; height: 14px">
                    <asp:TextBox ID="txtRecNo" runat="server" Width="52px" AutoPostBack="True" disabled="disabled"
                        TabIndex="1" dir="ltr" Style="text-align: right" MaxLength="7" ReadOnly="true"
                        CssClass="text_box" Visible="false"></asp:TextBox>
                </td>
                <td style="float: left;display:None; visibility:collapse;width: 2%; height: 14px">
                    <asp:Label ID="Label47" runat="server" Visible="false">Of</asp:Label>
                </td>
                <td style="float: left;display:None; visibility:collapse;width: 2%; height: 14px">
                    <asp:Label ID="lblCount" runat="server" Width="20px" Visible="false"></asp:Label>
                </td>
                <td style="float: left;display:None; visibility:collapse;width: 2%; height: 14px">
                    <asp:ImageButton ID="ibtnNext" runat="server" ImageUrl="~/images/next.png" Visible="false"/>
                </td>
                <td style="float: left;display:None; visibility:collapse;width: 2%; height: 14px">
                    <asp:ImageButton ID="ibtnLast" runat="server" ImageUrl="~/images/new_first.png" Visible="false"/>
                </td>
                <%-- Number2- Editted By Zoya @27/02/2016--%>
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
        </table>
        <table class="mainbg" style="width: 100%;">
            <tr>
                <td style="width: 494px; height: 39px;">
                    <asp:SiteMapPath ID="SiteMapPath1" runat="server">
                    </asp:SiteMapPath>
                </td>
                <td class="pagetext" style="height: 39px" align="right">
                    <asp:Label ID="lblMenuName" runat="server" Width="253px"></asp:Label>
                </td>
            </tr>
        </table>
    </asp:Panel>
    <%--  </ContentTemplate> </atlas:UpdatePanel>--%>
    <asp:Panel ID="PnlAdd" runat="server" Width="100%" Visible="true">
        <table style="width: 100%">
            <tr>
                <td style="width: 100%">
                   <div  style="border: thin solid #A6D9F4; width: 100%">
                        <table class="fields" style="width: 100%; height: 100%">
                            <tr>
                                <td style="width: 3px; height: 26px">
                                </td>
                                <td class="fields" style="width: 146px" >
                                </td>
                                <td style="width: 43px; height: 26px; text-align: center">
                                    <asp:Label ID="lblMsg" runat="server" Width="312px" CssClass="lblError" Visible="False"></asp:Label>&nbsp;
                                </td>
                                <td style="width: 100px; height: 26px">
                                </td>
                            </tr>
                            <tr runat="server" id="trCurrSem">
                                <td style="width: 3px; height: 26px"><span style="font-size: 11pt; color: #ff0066">*</span>
                                </td>
                                <td class="fields" style="width: 146px; height: 26px">                                    
                                    <asp:Label ID="Label1" runat="server" Text="Current Semester" Width="129px"></asp:Label>
                                </td>
                                <td style="width: 43px; height: 26px">
                                    <asp:DropDownList ID="ddlCurSem" runat="server" AutoPostBack="TRUE"  AppendDataBoundItems="true" Width="146px">
                                    </asp:DropDownList>
                                    </td>
                                <td style="width: 100px; height: 26px">
                                </td>
                            </tr>
                            <tr runat="server" id="trProgramType">
                                <td style="width: 3px; height: 21px">
                                </td>
                                <td class="fields" style="width: 146px; height: 21px">
                                    <span style="font-size: 11pt; color: #ff0066"></span>
                                    <asp:Label ID="Label48" runat="server" Height="16px" Text="Program Type" 
                                        Width="153px"></asp:Label>
                                </td>
                                <td class="Alerts" style="width: 43px; height: 21px">
                                    <asp:DropDownList ID="ddlProgramType" AppendDataBoundItems="true" AutoPostBack="true" runat="server" Width="146px">
                                    </asp:DropDownList>
                                    </td>
                                <td style="width: 100px; height: 21px">
                                </td>
                            </tr>
                            <tr runat="server" id="trProgram">
                                <td style="width: 3px; height: 21px">
                                    &nbsp;</td>
                                <td class="fields" style="width: 146px; height: 21px">
                                    <span style="font-size: 11pt; color: #ff0066"></span>
                                    <asp:Label ID="Label49" runat="server" Height="16px" Text="Program" 
                                        Width="153px"></asp:Label>
                                </td>
                                <td class="Alerts"  style="width: 43px; height: 21px">
                                    <asp:DropDownList ID="ddlProgram" AppendDataBoundItems="true" runat="server" Width="146px" AutoPostBack="true">
                                    </asp:DropDownList>
                                    </td>
                                <td style="width: 100px; height: 21px">
                                    &nbsp;</td>
                            </tr>
                            <tr runat="server" id="trNewSem">
                                <td style="width: 3px; height: 21px"><span style="font-size: 11pt; color: #ff0066">*</span>
                                </td>
                                <td class="fields" style="width: 146px; height: 21px">                                    
                                    <asp:Label ID="Label50" runat="server" Text="New Semester" Width="153px"></asp:Label>
                                </td>
                                <td style="width: 43px; height: 21px">
                                    
                                    <asp:DropDownList ID="ddlNewSem" runat="server" AppendDataBoundItems="true" 
                                        Width="146px">
                                    </asp:DropDownList>
                                    </td>
                                <td style="width: 100px; height: 21px">
                                </td>
                            </tr>
                        </table>
                  </div>
                </td>
            </tr>
        </table>
    </asp:Panel>
    <asp:Panel ID="PnlView" runat="server" Height="100%" Width="100%">
        <asp:DataGrid ID="dgView" runat="server" AutoGenerateColumns="False" 
            DataKeyField="MatricNo" Width="100%">
            <FooterStyle CssClass="dgFooterStyle" Height="20px" />
            <SelectedItemStyle CssClass="dgSelectedItemStyle" />
            <AlternatingItemStyle BackColor="Beige" CssClass="dgAlternatingItemStyle" 
                Font-Bold="False" Font-Italic="False" Font-Overline="False" 
                Font-Strikeout="False" Font-Underline="False" />
            <ItemStyle CssClass="dgItemStyle" />
            <HeaderStyle BackColor="#CDD7EE" CssClass="dgHeaderStyle" Font-Bold="True" 
                Font-Italic="False" Font-Overline="False" Font-Size="Medium" 
                Font-Strikeout="False" Font-Underline="False" />
            <Columns>
                <asp:ButtonColumn DataTextField="MatricNo" HeaderText="Matric No" 
                    ItemStyle-Width="10%"></asp:ButtonColumn>
                <asp:BoundColumn DataField="StudentName" HeaderText="Student Name">
                </asp:BoundColumn>
                <asp:BoundColumn DataField="CurretSemesterYear" HeaderText="Current Semester" 
                    ItemStyle-Width="10%"></asp:BoundColumn>
                <asp:BoundColumn DataField="Intake" HeaderText="Intake Session" 
                    ItemStyle-Width="10%"></asp:BoundColumn>
            </Columns>
        </asp:DataGrid>
        <asp:HiddenField ID="txtCurrSem" runat="server" />
    </asp:Panel>
  <%--  </ContentTemplate> </atlas:UpdatePanel>--%>
</asp:Content>
