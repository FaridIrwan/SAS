<%@ Page Language="VB" MasterPageFile="~/MasterPage3.master" AutoEventWireup="false" MaintainScrollPositionOnPostback="true"
    CodeFile="PTPTNFileGeneration.aspx.vb" Inherits="PTPTNFileGeneration" Title="Welcome To SAS" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <script language="javascript" type="Scripts/popcalendar.js"></script>
    <script language="javascript" type="Scripts/functions.js"></script>
    <script language="javascript" type="text/javascript">

        function CheckDbtDate() {
            var digits = "0123456789/";
            var temp;
            for (var i = 0; i < document.getElementById("<%=txtDebitingDate.ClientID%>").value.length; i++) {
                temp = document.getElementById("<%=txtDebitingDate.ClientID%>").value.substring(i, i + 1);
                if (digits.indexOf(temp) == -1) {
                    alert("Enter Valid Date (dd/mm/yyyy)");
                    document.getElementById("<%=txtDebitingDate.ClientID%>").value = "";
                    document.getElementById("<%=txtDebitingDate.ClientID%>").focus();
                    return false;
                }
            }
            return true;
        }

        function getDbtDate() {
            popUpCalendar(document.getElementById("<%=ibtnDbtDate.ClientID%>"), document.getElementById("<%=txtDebitingDate.ClientID%>"), 'dd/mm/yyyy')
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
                            <td style="height: 14px">
                                <asp:ImageButton ID="ibtnNew" runat="server" ImageUrl="~/images/add.png" ToolTip="New" />
                            </td>
                            <td style="width: 3%; height: 14px">
                                <asp:Label ID="lblNew" runat="server" Text="New" meta:resourcekey="Label11Resource1"></asp:Label>
                            </td>
                        </tr>
                    </table>
                </td>
                <%-- Editted By Zoya @27/02/2016--%>
                <td style="float: left;display:None; visibility:collapse;">
                    <table style="border-collapse: collapse;" class="menuoff" onmouseover="className='menuon';"
                        onmouseout="className='menuoff';">
                        <tr>
                            <td style="width: 24px; height: 14px">
                                <asp:ImageButton ID="ibtnSave" runat="server" ImageUrl="~/images/save.png" ToolTip="Save" ValidationGroup="1" Visible="false"/>
                            </td>
                            <td style="width: 3%; height: 14px">
                                <asp:Label ID="lblSave" runat="server" Text="Save" meta:resourcekey="Label14Resource1" Visible="false"></asp:Label>
                            </td>
                        </tr>
                    </table>
                </td>                
                <td style="float: left;display:None; visibility:collapse;">
                    <table style="border-collapse: collapse;" class="menuoff" onmouseover="className='menuon';"
                        onmouseout="className='menuoff';">
                        <tr>
                            <td style="width: 24px; height: 14px">
                                <asp:ImageButton ID="ibtnOpen" runat="server" ImageUrl="~/images/edit.gif" ToolTip="Open" Visible="false"/>
                            </td>
                            <td style="width: 3%; height: 14px">
                                <asp:Label ID="lblOpen" runat="server" Text="Open" meta:resourcekey="Label14Resource1" Visible="false"></asp:Label>
                            </td>
                        </tr>
                    </table>
                </td>
                <td style="float: left;display:None; visibility:collapse;">
                    <table style="border-collapse: collapse;" class="menuoff" onmouseover="className='menuon';"
                        onmouseout="className='menuoff';">
                        <tr>
                            <td style="width: 3%; height: 14px">
                                <asp:ImageButton ID="ibtnDelete" runat="server" ImageUrl="~/images/delete.png" Visible="false"/>
                            </td>
                            <td style="width: 3%; height: 14px">
                                <asp:Label ID="lblDelete" runat="server" Text="Delete" meta:resourcekey="Label13Resource1" Visible="false"></asp:Label>
                            </td>
                        </tr>
                    </table>
                </td>
                <td style="float: left;display:None; visibility:collapse;">
                    <table style="border-collapse: collapse;" class="menuoff" onmouseover="className='menuon';"
                        onmouseout="className='menuoff';">
                        <tr>
                            <td style="width: 3%; height: 14px">
                                <asp:ImageButton ID="ibtnSearch" runat="server" ImageUrl="~/images/find.png" ToolTip="View" Visible="false"/>
                            </td>
                            <td style="width: 3%; height: 14px">
                                <asp:Label ID="lblSearch" runat="server" Text="Search" meta:resourcekey="Label16Resource1" Visible="false"></asp:Label>
                            </td>
                        </tr>
                    </table>
                </td>
                <%--Done Editted By Zoya @27/02/2016--%>

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
        <div style="border: thin solid #A6D9F4; margin: 5px; width: 99%">
            <br />
            <table width="100%">
                <tr>
                    <td>
                        <table style="text-align:left;margin-bottom:20px;">
                            <tr>
                                <td style="width: 100px;">
                                    <asp:Label ID="lblDebitDate" runat="server" Text="Debiting Date"></asp:Label>
                                </td>
                                <td style="width:5px"></td>
                                <td style="width: 69px;">
                                    <asp:TextBox ID="txtDebitingDate" runat="server" MaxLength="10"></asp:TextBox>
                                </td>
                                <td style="width: 88px; height: 25px">
                                    <asp:Image ID="ibtnDbtDate" runat="server" ImageUrl="~/images/cal.gif" />
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
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
                <tr id="trDbtDate" runat="server" visible="false" >
                    <td>
                        <asp:Label ID="Label21" runat="server" Text="Debiting Date" Font-Bold="true"></asp:Label>
                    </td>
                    <td>
                        <asp:Label ID="lblDbtDate" runat="server"></asp:Label>
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
                <tr>
                    <td>
                        <asp:Label ID="Label7" runat="server" Text="Total Active Student" Font-Bold="true"></asp:Label>
                    </td>
                    <td>
                        <asp:Label ID="lblTotalStudentActive" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="Label9" runat="server" Text="Total Inactive Student" Font-Bold="true"></asp:Label>
                    </td>
                    <td>
                        <asp:Label ID="lblTotalStudentInactive" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="Label4" runat="server" Text="Direct Debit File<br />(Active Student)" Font-Bold="true"></asp:Label>
                    </td>
                    <td>
                        <asp:Button ID="btnDDFileDownload" runat="server" Text="Download"></asp:Button>
                        <br /> 
                        <asp:HiddenField ID="hdnDirectDebitFile" runat="server" Value="0" />
                    </td>
                </tr>
                <tr id="tblDDFail" runat="server" visible="false">
                    <td>
                        <asp:Label ID="Label5" runat="server" Text="Failed Direct Debit File<br />(Inactive Student)" Font-Bold="true"></asp:Label>
                    </td>
                    <td>
                        <asp:Button ID="btnDDFileDownloadFail" runat="server" Text="Download"></asp:Button>
                        <br /> 
                        <asp:HiddenField ID="hdnDDFail" runat="server" Value="0" />
                    </td>
                </tr>
                <tr id="tblMandateFile" runat="server" visible="false">
                    <td>
                        <asp:Label ID="Label6" runat="server" Text="Mandate File" Font-Bold="true"></asp:Label>
                    </td>
                    <td>
                        <asp:Button ID="btnDDMandateFile" runat="server" Text="Download"></asp:Button>
                        <br /> 
                        <asp:HiddenField ID="hdnMandateFile" runat="server" Value="0" />
                    </td>
                </tr>
            </table>
        </fieldset>
    </asp:Panel>

</asp:Content>
<asp:Content ID="Content2" runat="server" contentplaceholderid="head">
    <style type="text/css">
        .auto-style2 {
            width: 19px;
        }
    </style>
</asp:Content>

