<%@ Page Title="Integration" Language="VB" MasterPageFile="~/MasterPage3.master" AutoEventWireup="false" CodeFile="Integration.aspx.vb" Inherits="Integration" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">       
    <script language="javascript" src="Scripts/functions.js" type="text/javascript"></script>
    <script language="javascript" type="text/javascript">

        function do_totals1() {
            document.all.pleasewaitScreen.style.visibility = "visible";
            window.setTimeout('do_totals2()', 1)
        }
        function WaitingProcess() {
            document.all.pleasewaitScreen.style.visibility = "visible";
            PageMethods.FillDataGrid(CallSuccess, CallError);
            window.setTimeout('do_totals2()', 1)
        }
        function CallSuccess(res) {
            alert(res);
        }

        function CallError() {
            alert('Error');
        }

        function do_totals2() {
            calc_totals();
            document.all.pleasewaitScreen.style.visibility = "hidden";
        }
    </script>
    

    <asp:Panel ID="pnlHeader" runat="server" Width="100%">
        <table style="width: 100%; background-image: url(../images/Sample.png);">
            <tr>
                <td style="width: 1%; height: 14px"></td>
                <td style="width: 6%;">
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
                <td style="width: 10%;">
                    <table style="width: 100%; border-collapse: collapse;" onmouseover="className='menuon';"
                        onmouseout="className='menuoff';">
                        <tr>
                            <td style="width: 3%; height: 14px">
                                <asp:ImageButton ID="ibtnPost" runat="server" ImageUrl="~/images/posting.png" ToolTip="Post To CF" OnClientClick="do_totals1()" />
                            </td>
                            <td>
                                <asp:Label ID="lblPost" runat="server" Text="Post To CF" meta:resourcekey="Label13Resource1"></asp:Label>
                            </td>
                        </tr>
                    </table>
                </td>
                <td style="width: 80%; height: 14px"></td>
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
                    <asp:Label ID="lblMenuName" runat="server" Width="422px" Text="Integration"></asp:Label>
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
                                    <asp:Label ID="Label9" runat="server" Text="Invoice Status" style="margin-left: 7px;"></asp:Label>
                                </td>
                                <td align="left">
                                    <asp:RadioButtonList ID="rblInvoice" runat="server" RepeatDirection="Horizontal"  >
                                        <asp:ListItem Value="1" Text="ON" ></asp:ListItem>
                                        <asp:ListItem Value="0" Text="OFF"></asp:ListItem>
                                    </asp:RadioButtonList>  
                                </td>
                            </tr>
                            <tr>
                                <td width="10%">
                                    <asp:Label ID="Label1" runat="server" Text="Reciept Status" style="margin-left: 7px;"></asp:Label>
                                </td>
                                <td align="left">
                                    <asp:RadioButtonList ID="rblReciept" runat="server" RepeatDirection="Horizontal" >
                                        <asp:ListItem Value="1" Text="ON" ></asp:ListItem>
                                        <asp:ListItem Value="0" Text="OFF"></asp:ListItem>
                                    </asp:RadioButtonList>  
                                </td>
                            </tr>
                            <tr>
                                <td width="10%">
                                    <asp:Label ID="Label2" runat="server" Text="Payment Status" style="margin-left: 7px;"></asp:Label>
                                </td>
                                <td align="left">
                                    <asp:RadioButtonList ID="rblPayment" runat="server" RepeatDirection="Horizontal" >
                                        <asp:ListItem Value="1" Text="ON" ></asp:ListItem>
                                        <asp:ListItem Value="0" Text="OFF"></asp:ListItem>
                                    </asp:RadioButtonList>  
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
                                <asp:BoundColumn DataField="Batchcode" HeaderText="Batch Code">
                                    <HeaderStyle Width="15%" HorizontalAlign="Center" />
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:BoundColumn>
                                <asp:BoundColumn DataField="Description" HeaderText="Description">
                                    <HeaderStyle HorizontalAlign="left" />
                                    <ItemStyle HorizontalAlign="left" />
                                </asp:BoundColumn>
                                <asp:BoundColumn DataField="Category" HeaderText="Processes">
                                    <HeaderStyle HorizontalAlign="Left" />
                                    <ItemStyle HorizontalAlign="Left" />
                                </asp:BoundColumn>
                                <asp:BoundColumn DataField="TransDate" HeaderText="Date">
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

        <br />
        <br />
        <br />

        <div id="pleasewaitScreen" style="z-index: 5; left: 40%; visibility: hidden; position: absolute; top: 30%">
            <table bordercolor="#5ba789" height="100" cellspacing="0" cellpadding="0" width="300" bgcolor="#5ba789" border="1">
                <tr>
                    <td valign="middle" align="center" width="100%" bgcolor="#e4f0db" height="100%">
                        <br>
                        <br />
                        <img alt="" src="../images/spinner.gif" align="middle" />
                        <font face="Lucida Grande, Verdana, Arial, sans-serif" color="#000066" size="2">
                            <b>Processing. Please wait...</b></font>
                        <br />
                        <br />
                    </td>
                </tr>
            </table>
        </div>
    </asp:Panel>

</asp:Content>

