<%@ Page Title="" Language="VB" MasterPageFile="~/MasterPage3.master" AutoEventWireup="false" CodeFile="WorkFlowDetails.aspx.vb" Inherits="WorkFlowDetails" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <script language="javascript" type="text/javascript">

        function SelectAllCheckboxes(spanChk) {
            //alert("hai");
            // Added as ASPX uses SPAN for checkbox
            var oItem = spanChk.children;
            var theBox = (spanChk.type == "checkbox") ?
                spanChk : spanChk.children.item[0];
            xState = theBox.checked;
            elm = theBox.form.elements;

            for (i = 0; i < elm.length; i++)
                if (elm[i].type == "checkbox" &&
              elm[i].id != theBox.id) {
                    //elm[i].click();
                    if (elm[i].checked != xState)
                        elm[i].click();
                    //elm[i].checked=xState;
                }
        }

        function OpenWindow(URL) {
            var WindowName = "MyPopup";
            var Features = "location=no,toolbar=no,menubar=no,height =600,scrollbars=yes";
            window.open(URL, WindowName, Features);
        }

    </script>
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
                                <asp:ImageButton ID="ibtnView" runat="server" ImageUrl="~/images/find.png" ToolTip="View" />
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
                        <tr>
                            <td>
                                <asp:ImageButton ID="ibtnPrint" runat="server" ImageUrl="~/images/print.png" ToolTip="Print" />
                            </td>
                            <td>
                                <asp:Label ID="Label17" runat="server" Text="Print"></asp:Label>
                            </td>
                        </tr>
                    </table>
                </td>
                <td>
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
                <td>
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
                <td>
                    <table style="border-collapse: collapse;" class="menuoff" onmouseover="className='menuon';"
                        onmouseout="className='menuoff';">
                        <tr>
                            <td>
                                <asp:ImageButton ID="ibtnCancel" runat="server" ImageUrl="~/images/cancel.png" ToolTip="Cancel" Visible="false"/>
                            </td>
                            <td>
                                <asp:Label ID="Label18" runat="server" Text="Cancel" Visible="false"></asp:Label>
                            </td>
                        </tr>
                    </table>
                </td>
                <td style="width: 3%; height: 14px"></td>
                <td style="width: 2%; height: 14px">
                    <asp:ImageButton ID="ibtnFirst" runat="server" ImageUrl="~/images/new_last.png" Visible="false"/>
                </td>
                <td style="height: 14px; width: 4px;"></td>
                <td style="width: 2%; height: 14px">
                    <asp:ImageButton ID="ibtnPrevs" runat="server" ImageUrl="~/images/new_prev.png" Visible="false"/>
                </td>
                <td style="width: 2%; height: 14px">
                    <asp:TextBox ID="txtRecNo" runat="server" Width="52px" AutoPostBack="True" Style="text-align: right"
                        MaxLength="7" ReadOnly="true" CssClass="text_box" disabled="disabled" TabIndex="1"
                        dir="ltr" Visible="false"></asp:TextBox>
                </td>
                <td style="width: 2%; height: 14px">
                    <asp:Label ID="Label47" runat="server" Visible="false">Of</asp:Label>
                </td>
                <td style="width: 2%; height: 14px">
                    <asp:Label ID="lblCount" runat="server" Width="20px" Visible="false"></asp:Label>
                </td>
                <td style="width: 2%; height: 14px">
                    <asp:ImageButton ID="ibtnNext" runat="server" ImageUrl="~/images/new_next.png" Visible="false"/>
                </td>
                <td style="height: 14px"></td>
                <td style="width: 2%; height: 14px">
                    <asp:ImageButton ID="ibtnLast" runat="server" ImageUrl="~/images/new_first.png" Visible="false"/>
                </td>
                <td style="width: 2%; height: 14px"></td>
                 <td>
                    <table style="border-collapse: collapse;" class="menuoff" onmouseover="className='menuon';"
                        onmouseout="className='menuoff';">
                        <tr>
                            <td>
                                <asp:ImageButton ID="ibtnNew" runat="server" ImageUrl="images/add.png" ToolTip="New" Visible="false" />
                            </td>
                            <td>
                                <asp:Label ID="Label11" runat="server" Text="New" Visible="false"></asp:Label>
                            </td>
                        </tr>
                    </table>
                </td>
                <td>
                    <table style="border-collapse: collapse;" class="menuoff" onmouseover="className='menuon';"
                        onmouseout="className='menuoff';">
                        <tr>
                            <td>
                                <asp:ImageButton ID="ibtnSave" runat="server" ImageUrl="~/images/save.png" ToolTip="Save" Visible="false" />
                            </td>
                            <td>
                                <asp:Label ID="Label14" runat="server" Text="Save" Visible="false"></asp:Label>
                            </td>
                        </tr>
                    </table>
                </td>
                <td>
                    <table style="border-collapse: collapse;" class="menuoff" onmouseover="className='menuon';"
                        onmouseout="className='menuoff';">
                        <tr>
                            <td>
                                <asp:ImageButton ID="ibtnDelete" runat="server" ImageUrl="~/images/delete.png" Visible="false" />
                            </td>
                            <td>
                                <asp:Label ID="Label13" runat="server" Text="Delete" Visible="false"></asp:Label>
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
                    <asp:SiteMapPath ID="SiteMapPath1" runat="server">
                    </asp:SiteMapPath>
                </td>
                <td class="pagetext" style="height: 39px" align="right">
                    <asp:Label ID="lblMenuName" runat="server" Text="Account WorkFlow Details" Width="" Style="margin-left: 0px"></asp:Label>
                </td>
            </tr>
        </table>
    </asp:Panel>

    <asp:Panel ID="PnlAdd" runat="server" Width="100%"> 
        <div style="border: thin solid #A6D9F4; width: 100%">
            <table style="width: 100%">
                <tr>
                    <td style="width: 100%">

                        <table class="fields" style="width: 100%;">
                            <tr>
                                <td style="width: 3px; height: 26px"></td>
                                <td class="fields" style="width: 107px; height: 26px"></td>
                                <td style="width: 134px; height: 26px; text-align: center;">
                                    <asp:Label ID="lblMsg" runat="server" CssClass="lblError" Style="text-align: center" 
                                        Width="348px"></asp:Label>
                                </td>
                                <td style="width: 178px; height: 26px">
                                    <asp:HiddenField ID="hidUserRole" runat="server" Visible="false" />
                                </td>

                            </tr>

                        </table>


                    </td>
                </tr>
            </table>
        </div>
    </asp:Panel>
    <form id="workflow">
        <asp:Panel ID="PnlView" runat="server" Width="100%">
            <div style="border: thin solid #A6D9F4; width: 100%">
                <table style="width: 100%;">
                    <tr>
                        <td style="width: 1px; height: 21px">&nbsp;</td>
                        <td class="fields" style="width: 100%; height: 21px">
                            <asp:gridview ID="dgWofkFlowView" runat="server" AutoGenerateColumns="False" AllowPaging="true"
                                Width="100%" PageSize="10">
                                <FooterStyle CssClass="dgFooterStyle" Height="20px" />
                                <SelectedRowStyle CssClass="dgSelectedItemStyle" />
                                <AlternatingRowStyle BackColor="Beige" CssClass="dgAlternatingItemStyle" Font-Bold="False"
                                    Font-Italic="False" Font-Overline="False" Font-Strikeout="False" Font-Underline="False" />
                                <RowStyle CssClass="dgItemStyle" HorizontalAlign="Center" />
                                <HeaderStyle BackColor="#00699b" CssClass="dgHeaderStyle" ForeColor="#ffffff" Font-Bold="False" Font-Italic="False"
                                    Font-Overline="False" Font-Size="8pt" Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Center" />
                                <Columns>

                                    <asp:TemplateField HeaderText="Select">
                                        <HeaderTemplate>
                                            <input id="chkall" type="checkbox" onclick="SelectAllCheckboxes(this)" />
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:CheckBox ID="chk" runat="server" AutoPostBack="false" />
                                        </ItemTemplate>
                                        <HeaderStyle Width="5%" />
                                    </asp:TemplateField>

                                    <asp:BoundField DataField="AccountsEn.Category" HeaderText="Trans Type"></asp:BoundField>
                                    <asp:BoundField DataField="AccountsEn.BatchCode" HeaderText="Batch Code"></asp:BoundField>
                                    <asp:BoundField DataField="AccountsEn.TransactionAmount" HeaderText="Amount" DataFormatString="{0:F}"></asp:BoundField>
                                    <asp:BoundField DataField="UserId" HeaderText="Posted By" ></asp:BoundField>
                                    <asp:BoundField DataField="AccountsEn.BatchDate" HeaderText="Batch Date" DataFormatString="{0:dd/MM/yyyy}"></asp:BoundField>
                                    <asp:TemplateField HeaderText="View Details" Visible="false">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="studentlink" runat="server" CssClass="SelectRow" Text='<%#Eval("AccountsEn.Description")%>' 
                                                CommandArgument='<%#Eval("AccountsEn.BatchCode")%>' />  
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="PageName" HeaderText="Page Name" Visible="false"></asp:BoundField>
                                    <asp:BoundField DataField="AccountsEn.SubType" HeaderText="Sub Type" Visible="true"></asp:BoundField>
                                    <asp:TemplateField HeaderText="View Details">
                                        <HeaderStyle HorizontalAlign="Center" Width="12%" />
                                        <ItemTemplate>
                                        <center>
                                            <asp:HyperLink ID="View" runat="server" Text ="View"></asp:HyperLink>                                            
                                        </center>
                                            <input type="hidden" id="Hidden_WorkFlowId" name="H_WId"  value='<%#Eval("WorkflowId")%>' runat="server" />
                                            <input type="hidden" id="Hidden_TransId" name="H_TId" value='<%#Eval("AccountsEn.BatchCode")%>' runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:gridview>
                        </td>
                        <td style="width: 100px; height: 21px"></td>
                    </tr>
                    <tr id="viewbuttons" runat="server" visible="false">
                        <td style="width: 1px; height: 21px"></td>
                        <td class="fields" style="width: 100%; height: 21px">
                            <asp:Button ID="btnApproved" runat="server" Width="108px" />
                            <asp:Button ID="btnReject" runat="server" Text="Reject" Width="108px"  />&nbsp;&nbsp;
                            <asp:Label ID="lblRemarks" runat="server" Text="Remarks:" Width="60px" Height="20px"  />                           
                            <asp:TextBox ID="txtRemarks" runat="server" Width="450px"/>
                        </td>
                        <td style="width: 100px; height: 21px"></td>

                    </tr>
                </table>
                <input type="hidden" id="Tax_Id" name="Tax_Id" runat="server" />
            </div>
        </asp:Panel>
    </form>
</asp:Content>

