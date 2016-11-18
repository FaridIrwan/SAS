<%@ Page Language="VB" MasterPageFile="~/MasterPage3.master" AutoEventWireup="false" MaintainScrollPositionOnPostback="true"
    CodeFile="PTPTNFileGeneration.aspx.vb" Inherits="PTPTNFileGeneration" Title="PTPTN File Generation" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <script language="javascript" type="Scripts/popcalendar.js"></script>
    <script language="javascript" type="Scripts/functions.js"></script>

    <asp:Panel ID="pnlHeader" runat="server" Width="100%">
        <table style="background-image: url(../images/Sample.png);">
            <tr>
                <td style="width: 4px; height: 14px"></td>
                <td style="width: 14px; height: 14px"></td>
                <td>
                    <table style="border-collapse: collapse;" class="menuoff" onmouseover="className='menuon';"
                        onmouseout="className='menuoff';">
                        <tr>
                            <td style="height: 14px">
                                <asp:ImageButton ID="ibtnNew" runat="server" ImageUrl="~/images/add.png" ToolTip="New" />
                            </td>
                            <td style="width: 3%; height: 14px">
                                <asp:Label ID="lblNew" runat="server" Text="New" meta:resourcekey="Label11Resource1"></asp:Label>
                            </td>
                        </tr>
                    </table>
                </td>
                <td>
                    <table style="border-collapse: collapse;" class="menuoff" onmouseover="className='menuon';"
                        onmouseout="className='menuoff';">
                        <tr>
                            <td style="width: 24px; height: 14px">
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
                            <td style="width: 24px; height: 14px">
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
                                <asp:ImageButton ID="ibtnDelete" runat="server" ImageUrl="~/images/delete.png" />
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
                                <asp:ImageButton ID="ibtnSearch" runat="server" ImageUrl="~/images/find.png" ToolTip="View" />
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
                                <asp:ImageButton ID="ibtnCancel" runat="server" ImageUrl="~/images/cancel.png" ToolTip="Cancel" Width="24px" />
                            </td>
                            <td style="width: 3%; height: 14px">
                                <asp:Label ID="lblCancel" runat="server" Text="Cancel" meta:resourcekey="Label18Resource1"></asp:Label>
                            </td>
                        </tr>
                    </table>
                </td>
                <td style="width: 2%; height: 14px"></td>
                <td style="width: 2%; height: 14px"></td>
                <td style="width: 2%; height: 14px"></td>
                <td style="width: 2%; height: 14px"></td>
                <td style="width: 2%; height: 14px"></td>
                <td style="width: 2%; height: 14px"></td>
                <td style="width: 2%; height: 14px"></td>
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
                    <asp:Label ID="lblMenuName" runat="server" Width="422px" meta:resourcekey="lblMenuNameResource1"></asp:Label>
                </td>
            </tr>
        </table>
        <table>
            <tr>
                <td style="width: 100px">
                    <asp:Label ID="lblMsg" runat="server" CssClass="lblError" Style="text-align: center"
                        Width="301px" meta:resourcekey="lblMsgResource1"></asp:Label>
                </td>
            </tr>
        </table>
    </asp:Panel>

    <asp:Panel ID="pnlSearch" runat="server" Width="100%">
        <div align="center" style="border: thin solid #A6D9F4; margin: 5px; width: 99%">
            <br />
            <table width="100%" align="center">
                <tr>
                    <td>
                        <table width="100%">
                            <tr>
                                <td width="10%" align="left">
                                    <asp:FileUpload ID="FileUpload1" runat="server" Width="300px" />
                                </td>
                            </tr>
                            <tr>
                                <td width="10%" align="left">
                                    <asp:Button ID="btnGenerate" runat="server" Text="Generate Text File"></asp:Button>
                                    <br />
                                    <asp:HiddenField ID="hdnFileNoCount" runat="server" Value="0" />
                                </td>
                            </tr>
                        </table>
                        <br />
                    </td>
                </tr>
            </table>
        </div>
    </asp:Panel>
    <asp:Panel ID="pnlDisplay" runat="server" Width="100%" Visible="false">
        <fieldset style="width: 98%; padding: 2px; margin-left:5px; border: thin solid #A6D9F4">
            <legend style="margin-left: 10px">
                <asp:Label ID="lblSummary" runat="server" Text="File Summary" Font-Bold="true"></asp:Label>
            </legend>
            <table width="100%">
                <tr>
                    <td style="width: 10%;">
                        <asp:Label ID="Label3" runat="server" Text="File Name" Font-Bold="true"></asp:Label>
                    </td>
                    <td>
                        <asp:Label ID="lblFileName" runat="server" Text=""></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="Label2" runat="server" Text="Total Amount" Font-Bold="true"></asp:Label>
                    </td>
                    <td>
                        <asp:Label ID="lblTotalAmount" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="Label1" runat="server" Text="Total Student" Font-Bold="true"></asp:Label>
                    </td>
                    <td>
                        <asp:Label ID="lblTotalStudent" runat="server"></asp:Label>
                    </td>
                </tr>
            </table>
        </fieldset>
    </asp:Panel>

</asp:Content>
