<%@ Page Title="" Language="VB" MasterPageFile="~/MasterPage3.master" AutoEventWireup="false" CodeFile="StudentAcctsUpd.aspx.vb" Inherits="StudentAcctsUpd" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

    <asp:Panel ID="Panel1" runat="server" Width="100%">
    <table style="background-image:url(images/Sample.png);">
        <tr>
            <td style="width: 4px; height: 14px">

            </td>
            <td style="width: 14px; height: 14px">
            </td>
                <td >
                <table style="border-collapse:collapse;" class="menuoff" onmouseover="className='menuon';" onmouseout="className='menuoff';">
                <tr>
                <td ><asp:ImageButton ID="ibtnNew" runat="server" ImageUrl="images/add.png" ToolTip="New" /></td>
                <td ><asp:Label ID="Label11" runat="server" Text="New"></asp:Label></td>
                </tr>
                </table>
                </td>
                 
                <%-- Editted By Zoya @27/02/2016--%>           
                <td style="float: left;display:None; visibility:collapse;">
                <table style="border-collapse:collapse;" class="menuoff" onmouseover="className='menuon';" onmouseout="className='menuoff';">
                <tr>
                <td><asp:ImageButton ID="ibtnSave" runat="server" ImageUrl="~/images/save.png" ToolTip="Save" Visible="false"/></td>
                <td><asp:Label ID="Label14" runat="server" Text="Save" Visible="false"></asp:Label></td>
                </tr>
                </table>
                </td>
                <td style="float: left;display:None; visibility:collapse;">
                <table style="border-collapse:collapse;" class="menuoff" onmouseover="className='menuon';" onmouseout="className='menuoff';">
                <tr>
                <td><asp:ImageButton ID="ibtnDelete" runat="server" ImageUrl="~/images/delete.png" Visible="false"/></td>
                    <td><asp:Label ID="Label13" runat="server" Text="Delete" Visible="false"></asp:Label></td>
                </tr></table>
                    </td>
                             
                <td style="float: left;display:None; visibility:collapse;">
                <table style="border-collapse:collapse;" class="menuoff" onmouseover="className='menuon';" onmouseout="className='menuoff';">
                <tr>
                <td><asp:ImageButton ID="ibtnView" runat="server" ImageUrl="~/images/find.png" ToolTip="View" Visible="false"/></td>
                <td ><asp:Label ID="Label16" runat="server" Text="Search" Visible="false"></asp:Label></td>
                </tr></table>
                </td>
                <%--Done Editted By Zoya @27/02/2016--%> 

                <%-- Editted By Zoya @25/02/2016--%>            
                <td style="float: left;display:None; visibility:collapse;">
                <table style="border-collapse:collapse;" class="menuoff" onmouseover="className='menuon';" onmouseout="className='menuoff';">
                <tr>
                <td><asp:ImageButton ID="ibtnPrint" runat="server" ImageUrl="~/images/print.png" ToolTip="Print" Visible="false"/></td>
                <td><asp:Label ID="Label17" runat="server" Text="Print" Visible="false"></asp:Label></td>
                </tr>
                </table>
                </td>
               
                   
               <%-- <td>
                <table  style="border-collapse:collapse;" class="menuoff" onmouseover="className='menuon';" onmouseout="className='menuoff';">
                    </table></td>--%>
                           
                <td style="float: left;display:None; visibility:collapse;">
                <table  style="border-collapse:collapse;" class="menuoff" onmouseover="className='menuon';" onmouseout="className='menuoff';">
                <tr>
                <td ><asp:ImageButton ID="ibtnOthers" runat="server" ImageUrl="~/images/gothers.gif" ToolTip="Cancel" Visible="false"/></td>
                <td ><asp:Label ID="Label5" runat="server" Text="Others" Visible="false"></asp:Label></td>
                </tr></table></td>
                           
                <td style="float: left">
                <table style="border-collapse:collapse;" class="menuoff" onmouseover="className='menuon';" onmouseout="className='menuoff';">
                <tr>
                <td ><asp:ImageButton ID="ibtnCancel" runat="server" ImageUrl="~/images/cancel.png" ToolTip="Cancel" /></td>
                <td><asp:Label ID="Label18" runat="server" Text="Cancel"></asp:Label></td>
                </tr></table></td>
             <%-- Done Editted By Zoya @25/02/2016--%>

             <%--Editted By Zoya @27/02/2016--%>
            <td style="float: left;display:None; visibility:collapse;width: 2%; height: 14px">
                <asp:ImageButton ID="ibtnFirst" runat="server" ImageUrl="~/images/new_last.png" Visible="false"/></td>
                      
            <td style="float: left;display:None; visibility:collapse;width: 2%; height: 14px">
                <asp:ImageButton ID="ibtnPrevs" runat="server" ImageUrl="~/images/new_prev.png" Visible="false"/></td>
            <td style="float: left;display:None; visibility:collapse;width: 2%; height: 14px">
                <asp:TextBox ID="txtRecNo" runat="server" readonly="true" disabled="disabled" tabindex="1" dir="ltr" CssClass="text_box" AutoPostBack="True" MaxLength="7" Style="text-align: right"
                    Width="52px" Visible="false"></asp:TextBox></td>
            <td style="float: left;display:None; visibility:collapse;width: 2%; height: 14px">
                <asp:Label ID="Label47" runat="server" Visible="false">Of</asp:Label></td>
                    <td style="width: 2%; height: 14px"><asp:Label ID="lblCount" runat="server" Width="20px" Visible="false"></asp:Label></td>
                       
            <td style="float: left;display:None; visibility:collapse;width: 2%; height: 14px">
                <asp:ImageButton ID="ibtnNext" runat="server" ImageUrl="~/images/next.png" Visible="false"/></td>
                      
            <td style="float: left;display:None; visibility:collapse;width: 2%; height: 14px">
                <asp:ImageButton ID="ibtnLast" runat="server" ImageUrl="~/images/new_first.png" Visible="false"/></td>
            <%-- Done Editted By Zoya @27/02/2016--%>

            <td style="width: 2%; height: 14px">
            </td>
            <td style="width: 100%; height: 14px">
            </td>
            <td style="width: 100%; height: 14px">
            </td>
        </tr>
    </table>
    <table class="mainbg" style="width: 100%">
        <tr>
            <td class="vline" style="width: 98%; height: 1px">
            </td>
        </tr>
    </table>
    <table class="mainbg" style="width: 100%">
        <tr>
            <td style="width: 494px; height: 39px">
                <asp:SiteMapPath ID="SiteMapPath1" runat="server">
                </asp:SiteMapPath>
            </td>
            <td align="right" class="pagetext" style="height: 39px">
                <asp:Label ID="lblMenuName" Text="Update Student Account" runat="server" Width="290px"></asp:Label></td>
        </tr>
    </table>
</asp:Panel>
    <asp:Panel ID="PnlAdd" runat="server"  Width="100%">
<div  style="border: thin solid #A6D9F4; width: 100%">
<table class="fields" style="width: 100%; height: 100%; text-align: left;">
    <tr>
        <td class="fields" style="width: 67px; height: 17px">
        </td>
        <td style="width: 43px; height: 17px">
            <asp:Label ID="lblMsg" runat="server" CssClass="lblError" Style="text-align: center"
                Width="348px"></asp:Label>
        </td>
        <td style="width: 100px; height: 20px">
        </td>
    </tr>
<tr>
    <td class="fields" style="width: 67px; height: 17px">
        &nbsp;
           <span
                style="font-size: 11pt; color: #ff0066">*</span>
            <asp:Label ID="lblUpload" runat="server" Text="Upload File"></asp:Label></td>
    <td style="width: 43px; height: 17px">
           <asp:FileUpload ID="UploadFile" runat="server" Width="300px" /></td>          
    <td style="width: 100px; height: 17px">
    </td>
</tr>
    <tr>
        <td class="fields" style="width: 67px; height: 26px" colspan="2">
             <asp:Button ID="btnUpload" runat="server" Text="Upload File"></asp:Button>
        </td>
    </tr>
    
</table>
<br />
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
                        <asp:Label ID="Label1" runat="server" Text="Total Student" Font-Bold="true"></asp:Label>
                    </td>
                    <td>
                        <asp:Label ID="lblTotalStudent" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="Label2" runat="server" Text="Stud Acc. Update" Font-Bold="true"></asp:Label>
                    </td>
                    <td>
                        <asp:Label ID="lblStudAccUpd" runat="server"></asp:Label>
                    </td>
                </tr>
            </table>
        </fieldset>
</asp:Panel>
</asp:Content>