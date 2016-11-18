<%@ Page Language="VB" MasterPageFile="~/MasterPage3.master" AutoEventWireup="false" MaintainScrollPositionOnPostback="true"
    CodeFile="RptStudentLedger.aspx.vb" Inherits="RptStudentLedger" Title="Student Ledger" %>

<%@ Register Assembly="AtlasControlToolkit" Namespace="AtlasControlToolkit" TagPrefix="atlasToolkit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <script language="javascript" type="text/javascript">

        //function onloadview(url)
        //{
        //    var iff = window.document.getElementById("id");
        //    iff.src=url;
        //    $object('modalp1')._show();
        //}

        function OpenPopup(pageURL, title, w, h) {
            var left = (screen.width - w) / 2;
            var top = (screen.height - h) / 4;  // for 25% - devide by 4  |  for 33% - devide by 3
            var targetWin = window.open(pageURL, title, 'toolbar=no, location=no, directories=no, status=no, menubar=no, scrollbars=yes, resizable=yes, copyhistory=no, width=' + w + ', height=' + h + ', top=' + top + ', left=' + left);
            targetWin.focus();
        }

        function OpenWindow(URL) {
            var WindowName = "MyPopup";
            var Features = "location=no,toolbar=no,menubar=no,height =600,scrollbars=yes";
            window.open(URL, WindowName, Features);
        }

        function refreshParentPage() {
            window.document.forms(0).submit();
        }

        function dllValues() {
            if (document.getElementById("<%=txtStudentCode.ClientID %>").value == "") {
                alert("Please Choose Matric No.");
                return false
            }
            else {
                window.open('../GroupReport/RptStudentLedgerViewer.aspx', 'SAS', 'width=700,height=500,resizable=1');
                return true
            }
        }
    </script>

    <asp:Panel ID="Panel1" runat="server" Width="100%">
        <table style="background-image: url(../images/Sample.png);">
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
                                <asp:ImageButton ID="ibtnOthers" runat="server" ImageUrl="~/images/gothers.png" ToolTip="Cancel" />
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
                    <asp:ImageButton ID="ibtnFirst" runat="server" ImageUrl="~/images/new_last.png" />
                </td>
                <td style="width: 2%; height: 14px">
                    <asp:ImageButton ID="ibtnPrevs" runat="server" ImageUrl="~/images/new_prev.png" />
                </td>
                <td style="width: 2%; height: 14px">
                    <asp:TextBox ID="txtRecNo" runat="server" AutoPostBack="True" MaxLength="5" Style="text-align: right"
                        Width="52px" ReadOnly="true" CssClass="text_box" disabled="disabled" TabIndex="1"
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
                <td style="width: 2%; height: 14px">
                    <asp:ImageButton ID="ibtnLast" runat="server" ImageUrl="~/images/new_first.png" />
                </td>
                <td style="width: 2%; height: 14px"></td>
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
                    <asp:Label ID="lblMenuName" runat="server" Width="273px"></asp:Label>
                </td>
            </tr>
        </table>

        <asp:Label ID="lblMsg" runat="server" CssClass="lblError" Style="text-align: center" Width="359px"></asp:Label>
    </asp:Panel>

    <asp:Panel ID="pnlBody" runat="server">
        <table style="width: 98%; margin-left: auto; margin-right: auto;">
            <tr>
                <td style="width: 100%">
                    <div style="border: thin solid #A6D9F4; width: 100%">
                        <table style="width:100%; margin-left:5px;">
                            <tr>
                                <td style="width: 13%"></td>
                                <td style="width: 13%"></td>
                                <td style="width: 183px; text-align: left"></td>
                                <td style="width: 100px; text-align: right"></td>
                                <td style="width: 271px"></td>
                                <td style="width: 378px">
                                    <asp:Button ID="btnHidden" runat="Server" Style="display: none" OnClick="btnHidden_Click" />
                                </td>
                                <td style="width: 378px"></td>
                            </tr>
                            <tr>
                                <td style="width: 13%">
                                    <strong>
                                        <asp:Label ID="Label1" runat="server" Text="Matric ID" Width="97px"></asp:Label></strong>
                                </td>
                                <td style="width: 13%">
                                    <asp:TextBox ID="txtStudentCode" runat="server" MaxLength="20" Width="216px"></asp:TextBox>
                                </td>
                                <td style="width: 183px; text-align: left">
                                    <asp:Image ID="ibtnSpn1" runat="server" Height="16px" ImageAlign="Left" ImageUrl="~/images/find_img.png"
                                        Style="margin-left: 0px" />
                                </td>
                                <td style="width: 42px;">
                                    <strong>
                                        <asp:Label ID="Label4" runat="server" Text="IC No" Width="42px" Style="text-align: left"></asp:Label></strong>
                                </td>
                                <td style="width: 271px">
                                    <asp:Label ID="txtICNO" runat="server" Width="142px"></asp:Label>
                                </td>
                                <td style="width: 378px">&nbsp;
                                </td>
                                <td style="width: 378px"></td>
                            </tr>
                            <tr>
                                <td style="width: 13%">
                                    <strong>
                                        <asp:Label ID="Label3" runat="server" Text="Student Name " Width="97px"></asp:Label></strong>
                                </td>
                                <td style="width: 13%">
                                    <asp:Label ID="txtStuName" runat="server" Width="218px" Text=""></asp:Label>
                                </td>
                                <td style="width: 183px; text-align: right"></td>
                                <td style="width: 42px;">
                                    <strong>
                                        <asp:Label ID="Label7" runat="server" Text="Semester " Width="51px" Style="text-align: left"></asp:Label></strong>
                                </td>
                                <td style="width: 271px; padding-left: 5px">
                                    <asp:DropDownList ID="ddlSponser" runat="server" AppendDataBoundItems="True" Width="82px"
                                        AutoPostBack="True">
                                        <asp:ListItem Value="-1">All</asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                                <td style="width: 378px">&nbsp;</td>
                                <td style="width: 378px"></td>
                            </tr>
                            <tr>
                                <td colspan="1" style="height: 25px; width: 42px;">
                                    <strong>
                                        <asp:Label ID="Label12" runat="server" Text="Status" Width="97px"></asp:Label></strong>
                                </td>
                                <td style="width: 271px; height: 25px;">
                                    <asp:Label ID="TextBox8" runat="server" Width="142px" Text=""
                                        ReadOnly="True"></asp:Label>
                                </td>
                                <td style="width: 378px; height: 25px;">&nbsp;</td>
                            </tr>
                            <tr>
                                <td style="width: 13%; height: 25px;">
                                    <strong>Current Semester</strong>
                                </td>
                                <td style="width: 13%; height: 25px;">
                                    <asp:Label ID="lblCurSem" runat="server" Text=""></asp:Label>
                                </td>
                                <td style="width: 183px; text-align: right; height: 25px;"></td>
                                <td align="left">
                                    <strong>Program</strong>
                                </td>
                                <td style="width: 271px; height: 25px;" colspan="3">
                                    <asp:Label ID="lblProgram" runat="server" Text=""></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 13%; height: 25px;">
                                    <strong>Kolej</strong>
                                </td>
                                <td style="width: 13%; height: 25px;">
                                    <asp:Label ID="lblkolej" runat="server" Text=""></asp:Label>
                                </td>
                                <td style="width: 183px; text-align: right; height: 25px;">&nbsp;</td>
                                <td align="left">
                                    <strong>Block</strong>
                                </td>
                                <td style="width: 271px; height: 25px;">
                                    <asp:Label ID="lblblock" runat="server" Text=""></asp:Label>
                                </td>
                                <td style="width: 378px; height: 25px;">&nbsp;</td>
                                <td style="width: 378px; height: 25px;">&nbsp;</td>
                            </tr>
                            <tr>
                                <td style="width: 13%; height: 25px;">
                                    <strong>Floor No</strong>
                                </td>
                                <td style="width: 13%; height: 25px;">
                                    <asp:Label ID="lblfloor" runat="server" Text=""></asp:Label>
                                </td>
                                <td style="width: 183px; text-align: right; height: 25px;">&nbsp;</td>
                                <td align="left"><strong>Room Type</strong></td>
                                <td>
                                    <asp:Label ID="lblroom" runat="server" Text=""></asp:Label>
                                </td>
                                <td style="width: 378px; height: 25px;">&nbsp;</td>
                                <td style="width: 378px; height: 25px;">&nbsp;</td>
                            </tr>
                        </table>
                    </div>
                </td>
            </tr>
        </table>
        <table width="100%">
            <tr>
                <td style="height: 16px; width: 1px;">&nbsp;
                </td>
                <td style="width: 72%; height: 16px">
                    <asp:RadioButton ID="rdbStudentLeddger" AutoPostBack="true" runat="server" Checked="true"
                        GroupName="Ledger" Text="Student Ledger" />
                    <asp:RadioButton ID="rdbStudentLoanLedger" AutoPostBack="true" runat="server" Text="Loan Ledger"
                        GroupName="Ledger" />
                </td>
                <td style="height: 16px" width="30%"></td>
                <td style="height: 16px" width="30%"></td>
                <td style="height: 16px" width="1%"></td>
            </tr>
            <tr>
                <td style="height: 16px" width="1%"></td>
                <td colspan="3" style="height: 16px">
                    <asp:DataGrid ID="dgInvoices" runat="server" AutoGenerateColumns="False" DataKeyField="TransactionCode"
                        Width="100%" OnSelectedIndexChanged="dgInvoices_SelectedIndexChanged">
                        <FooterStyle CssClass="dgFooterStyle" Height="20px" />
                        <SelectedItemStyle CssClass="dgSelectedItemStyle" />
                        <AlternatingItemStyle BackColor="Beige" CssClass="dgAlternatingItemStyle" Font-Bold="False"
                            Font-Italic="False" Font-Overline="False" Font-Strikeout="False" Font-Underline="False" />
                        <ItemStyle CssClass="dgItemStyle" />
                        <HeaderStyle BackColor="#CDD7EE" CssClass="dgHeaderStyle" Font-Bold="True" Font-Italic="False"
                            Font-Overline="False" Font-Size="Medium" Font-Strikeout="False" Font-Underline="False" />
                        <Columns>
                            <asp:BoundColumn DataField="TransDate" HeaderText="Date" DataFormatString="{0:dd/MM/yyyy}"></asp:BoundColumn>
                            <asp:BoundColumn DataField="TransactionCode" HeaderText="Document No"></asp:BoundColumn>
                            <asp:BoundColumn HeaderText="Description" DataField="Description">
                                <HeaderStyle Width="30%" />
                            </asp:BoundColumn>
                            <asp:BoundColumn DataField="Category" HeaderText="Category"></asp:BoundColumn>
                            <asp:BoundColumn HeaderText="Debit" DataField="Debit" DataFormatString="{0:F}">
                                <HeaderStyle HorizontalAlign="Right" />
                                <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                    Font-Underline="False" HorizontalAlign="Right" />
                            </asp:BoundColumn>
                            <asp:BoundColumn HeaderText="Credit" DataField="Credit" DataFormatString="{0:F}">
                                <HeaderStyle HorizontalAlign="Right" />
                                <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                    Font-Underline="False" HorizontalAlign="Right" />
                            </asp:BoundColumn>
                            <asp:BoundColumn DataField="TransType" HeaderText="TransType" Visible="False"></asp:BoundColumn>
                            <asp:BoundColumn HeaderText="Transaction Amount" DataField="TransactionAmount" Visible="false">
                                <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                    Font-Underline="False" HorizontalAlign="Right" />
                            </asp:BoundColumn>
                            <asp:BoundColumn HeaderText="Statement Balance">
                                <HeaderStyle HorizontalAlign="Right" />
                                <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                    Font-Underline="False" HorizontalAlign="Right" />
                            </asp:BoundColumn>
                            <asp:TemplateColumn HeaderText="View">
                                <HeaderStyle HorizontalAlign="Center" />
                                <ItemTemplate>
                                    <center>
                                        <asp:HyperLink ID="View" runat="server">View</asp:HyperLink></center>
                                </ItemTemplate>
                            </asp:TemplateColumn>
                            <asp:BoundColumn DataField="BatchCode" HeaderText="BatchCode" Visible="False"></asp:BoundColumn>
                        </Columns>
                    </asp:DataGrid>
                </td>
                <td style="height: 16px" width="1%"></td>
            </tr>
            <tr>
                <td style="height: 16px" width="1%"></td>
                <td style="width: 72%; height: 16px; text-align: right">
                    <asp:TextBox ID="txtPg" runat="server" MaxLength="20" Width="240px" Visible="False"></asp:TextBox>&nbsp;
                </td>
                <td style="height: 16px; text-align: right" width="30%">
                    <asp:Label ID="lblDebit" runat="server" Text="Total Debit Amount" Visible="False"></asp:Label>
                </td>
                <td style="height: 16px; text-align: right" width="30%">
                    <asp:TextBox ID="txtDebitAmt" runat="server" Style="text-align: right" Width="101px"
                        Visible="False" Font-Bold="True" ReadOnly="True"></asp:TextBox>
                </td>
                <td style="height: 16px" width="1%"></td>
            </tr>
            <tr>
                <td style="height: 16px" width="1%"></td>
                <td style="width: 72%; height: 16px; text-align: right">
                    <asp:ObjectDataSource ID="odbSem" runat="server" SelectMethod="GetSemesters" TypeName="HTS.SAS.BS.bsTransaction">
                        <SelectParameters>
                            <asp:ControlParameter ControlID="txtStudentCode" Name="StudentMno" PropertyName="Text"
                                Type="String" />
                        </SelectParameters>
                    </asp:ObjectDataSource>
                </td>
                <td style="height: 16px; text-align: right" width="30%">
                    <asp:Label ID="lblCredit" runat="server" Text="Total Credit Amount" Visible="False"></asp:Label>
                </td>
                <td style="height: 16px; text-align: right" width="30%">
                    <asp:TextBox ID="txtCreditAmt" runat="server" Style="text-align: right" Width="102px"
                        Visible="False" Font-Bold="True" ReadOnly="True"></asp:TextBox>
                </td>
                <td style="height: 16px" width="1%"></td>
            </tr>
            <tr>
                <td style="height: 16px" width="1%"></td>
                <td style="width: 72%; height: 16px; text-align: right"></td>
                <td style="height: 16px; text-align: right" width="30%">
                    <asp:Label ID="lblOut" runat="server" Text="Outstanding Amount"></asp:Label>
                </td>
                <td style="height: 16px; text-align: right" width="30%">
                    <asp:TextBox ID="txtOutAmt" runat="server" Font-Bold="True" Style="text-align: right"
                        Width="102px" ReadOnly="True"></asp:TextBox>
                </td>
                <td style="height: 16px" width="1%"></td>
            </tr>
        </table>
    </asp:Panel>

</asp:Content>
