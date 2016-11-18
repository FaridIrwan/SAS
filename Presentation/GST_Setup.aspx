<%@ Page Title="" Language="VB" MasterPageFile="~/MasterPage3.master" AutoEventWireup="false" CodeFile="GST_Setup.aspx.vb" Inherits="GST_Setup" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <style type="text/css">
        .auto-style1 {
            width: 3px;
            height: 28px;
        }

        .auto-style2 {
            height: 28px;
        }

        .auto-style3 {
            width: 100px;
            height: 28px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <script language="javascript" type="text/javascript">
        function validate() {
            var re = /\s*((\S+\s*)*)/;
            var rexp = /^\d*\.?\d*$/;
            if (document.getElementById("<%=_TaxType.ClientID%>").value.replace(re, "$1").length == 0) {
                alert("Tax Type Field Cannot Be Blank");
                document.getElementById("<%=_TaxType.ClientID%>").focus();
                return false;
            }
            if (document.getElementById("<%=ddlTaxCode.ClientID%>").value == "-1") {
                alert("Select TaxCode");
                document.getElementById("<%=ddlTaxCode.ClientID%>").focus();
                return false;
            }
            
            if (document.getElementById("<%=_TaxPercentage.ClientID%>").value.replace(re, "$1").length == 0) {
                alert("GST Tax Percentage Field Cannot Be Blank");
                document.getElementById("<%=_TaxPercentage.ClientID%>").focus();
                return false;
            } else {
                
                if (document.getElementById("<%=_TaxPercentage.ClientID%>").value.match(rexp)) {
                } else {
                    alert("Please Enter Valid Value");
                    document.getElementById("<%=_TaxPercentage.ClientID%>").focus();
                    return false;
                }
            }

            if (document.getElementById("<%=ddlTaxMode.ClientID%>").value == "-1") {
                alert("Select TaxMode");
                document.getElementById("<%=ddlTaxMode.ClientID%>").focus();
                return false;
            }

            if (document.getElementById("<%=txtGLAccount.ClientID%>").value.replace(re, "$1").length == 0) {
                alert("GLAccount Field Cannot Be Blank");
                document.getElementById("<%=txtGLAccount.ClientID%>").focus();
                return false;
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
                <%-- Done Editted By Zoya @25/02/2016--%>

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
                                <asp:ImageButton ID="ibtnOthers" runat="server" ImageUrl="~/images/gothers.gif" ToolTip="Cancel" Visible="false" />
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
                <%--<td style="width: 3%; height: 14px"></td>--%>

                <%-- Done Editted By Zoya @24/02/2016--%>

                <td style="width: 2%; height: 14px">
                    <asp:ImageButton ID="ibtnFirst" runat="server" ImageUrl="~/images/new_last.png" />
                </td>
                <td style="height: 14px; width: 4px;"></td>
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
                    <asp:ImageButton ID="ibtnNext" runat="server" ImageUrl="~/images/new_next.png" />
                </td>
                <td style="height: 14px"></td>
                <td style="width: 2%; height: 14px">
                    <asp:ImageButton ID="ibtnLast" runat="server" ImageUrl="~/images/new_first.png" />
                </td>
                <td style="width: 2%; height: 14px"></td>
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
                    <asp:Label ID="lblMenuName" runat="server" Text="GST Setup" Width="253px"></asp:Label>
                </td>
            </tr>
        </table>
    </asp:Panel>

    <asp:Panel ID="PnlAdd" runat="server" Width="100%">
        <table style="width: 100%">
            <tr>
                <td style="width: 100%">
                    <div style="border: thin solid #A6D9F4; width: 100%">
                        <table class="fields" style="width: 100%;">
                            <tr>
                                <td style="width: 3px; height: 26px"></td>
                                <td class="fields" style="width: 107px; height: 26px"></td>
                                <td style="width: 134px; height: 26px; text-align: center">
                                    <asp:Label ID="lblMsg" runat="server" CssClass="lblError" Style="text-align: center"
                                        Width="348px"></asp:Label>
                                </td>
                                <td style="width: 178px; height: 26px"></td>
                                <td style="width: 100px; height: 26px"></td>
                            </tr>
                            <tr>
                                <td class="auto-style1"></td>
                                <td class="fields" style="width: 107px; height: 28px">
                                    <span
                                        style="font-size: 11pt; color: #ff0066">*</span>
                                    <asp:Label ID="lblTaxType" runat="server" Text="Tax Type"></asp:Label>
                                </td>
                                <td colspan="2" class="auto-style2">
                                    <asp:TextBox ID="_TaxType" runat="server" Width="346px" MaxLength="50" Height="21px"></asp:TextBox>
                                </td>
                                <td class="auto-style3">
                                   

                                </td>
                            </tr>
                            <tr>
                                <td style="width: 3px; height: 26px"></td>
                                <td class="fields" style="width: 107px; height: 26px">
                                    <span
                                        style="font-size: 11pt; color: #ff0066">*</span>
                                    <asp:Label ID="lblTaxcode" runat="server" Text="Tax Code"></asp:Label>
                                </td>
                                <td style="height: 26px" colspan="2">
                                    <asp:DropDownList ID="ddlTaxCode" runat="server" Width="146px">
                                    </asp:DropDownList>
                                    
                                </td>
                                <td style="width: 100px; height: 26px"></td>
                            </tr>

                            <tr>
                                <td style="width: 3px; height: 25px;"></td>
                                <td class="fields" style="width: 107px; height: 25px;">
                                    <span
                                        style="font-size: 11pt; color: #ff0066">*</span>
                                    <asp:Label ID="lblTaxPercentage" runat="server" Text="Tax Percentage (%)"></asp:Label>
                                </td>
                                <td style="height: 25px;" colspan="2">
                                    <asp:TextBox ID="_TaxPercentage" runat="server" Width="142px" MaxLength="20"></asp:TextBox>
                                    
                                </td>
                                <td style="width: 100px; height: 25px;"></td>
                            </tr>
                            <tr>
                                <td style="width: 3px"></td>
                                <td class="fields" style="vertical-align: text-top; width: 107px">
                                    <span style="font-size: 11pt; color: #ff0066">*</span>
                                    <asp:Label ID="lblTaxMode" runat="server" Style="vertical-align: middle" Text="Tax Mode"
                                        Width="78px"></asp:Label>
                                </td>
                                <td class="fields" style="height: 26px" colspan="2">
                                    <asp:DropDownList ID="ddlTaxMode" runat="server" Width="146px">
                                    </asp:DropDownList>
                                </td>
                                <td style="width: 178px"></td>
                                <td style="width: 100px"></td>
                            </tr>
                            <tr>
                                <td style="width: 3px"></td>
                                <td class="fields" style="vertical-align: text-top; width: 107px">
                                    <span style="font-size: 11pt; color: #ff0066">*</span>
                                    <asp:Label ID="lblGlAcc" runat="server" Style="vertical-align: middle" Text="GL Account" Width="78px"></asp:Label>
                                </td>
                                <td class="fields" style="height: 26px" colspan="2">
                                    <asp:TextBox ID="txtGLAccount" runat="server"></asp:TextBox>
                                    <asp:Button ID="Check" runat="server" Text="Check" />
                                    &nbsp;
                                    <br />
                                    <asp:Label ID="lblDesc" runat="server"></asp:Label>
                                    <br />
                                    <asp:Image ID="imgGL" runat="server" Height="18px" ImageAlign="Baseline" ImageUrl="~/images/check.png" Visible="False" />
                                    &nbsp;
                                </td>
                                <td style="width: 178px"></td>
                                <td style="width: 100px"></td>
                            </tr>
                        </table>
                        <br />
                        <br />
                    </div>
                </td>
            </tr>
        </table>
    </asp:Panel>

    <asp:Panel ID="PnlView" runat="server" Visible="" Width="100%">
        <table style="width: 100%;">
            <tr>
                <td style="width: 1px; height: 21px"></td>
                <td class="fields" style="width: 100%; height: 21px">
                    <asp:DataGrid ID="dgGSTView" runat="server" AutoGenerateColumns="False" DataKeyField="sas_taxid"
                        Width="100%" OnItemCommand="dgGSTView_ItemCommand">
                        <FooterStyle CssClass="dgFooterStyle" Height="20px" />
                        <SelectedItemStyle CssClass="dgSelectedItemStyle" />
                        <AlternatingItemStyle BackColor="Beige" CssClass="dgAlternatingItemStyle" Font-Bold="False"
                            Font-Italic="False" Font-Overline="False" Font-Strikeout="False" Font-Underline="False" />
                        <ItemStyle CssClass="dgItemStyle" />
                        <HeaderStyle BackColor="#CDD7EE" CssClass="dgHeaderStyle" Font-Bold="True" Font-Italic="False"
                            Font-Overline="False" Font-Size="Medium" Font-Strikeout="False" Font-Underline="False" />
                        <Columns>

                            <asp:TemplateColumn HeaderText="Tax Type">
                                <ItemTemplate>
                                    <asp:LinkButton ID="imgBtn1" CssClass="SelectRow" runat="server"
                                        Text='<%#(Eval("sas_taxtype"))%>' CommandName="ViewFactors" />
                                </ItemTemplate>
                            </asp:TemplateColumn>
                            <asp:BoundColumn DataField="taxcode" HeaderText="Tax Code"></asp:BoundColumn>
                            <asp:BoundColumn DataField="taxmode" HeaderText="Tax Mode"></asp:BoundColumn>
                            <asp:BoundColumn DataField="sas_taxpercentage" HeaderText="Tax Percentage"></asp:BoundColumn>
                            <asp:BoundColumn DataField="glaccount" HeaderText="GL Account"></asp:BoundColumn>
                        </Columns>
                    </asp:DataGrid>
                </td>
                <td style="width: 100px; height: 21px"></td>
            </tr>
        </table>
        <input type="hidden" id="Tax_Id" name="Tax_Id" runat="server" />
    </asp:Panel>
</asp:Content>



