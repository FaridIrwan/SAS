<%@ Page Language="VB" MasterPageFile="~/MasterPage3.master" AutoEventWireup="false" MaintainScrollPositionOnPostback="true" CodeFile="CimbClicksTrans.aspx.vb" Inherits="CimbClicksTrans" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

<script language="javascript" type="text/javascript">
    function Reload()
    {
        var getFileName = document.getElementById("<%=UploadFile.ClientID%>").value.split('\\').pop();

        document.getElementById("<%=lblFileUpload.ClientID%>").innerHTML = getFileName
    }
    function validate() {

        if (document.getElementById("<%=ddlBankCode.ClientID%>").value == "-1") {
            alert("Select a Bank Code");
            document.getElementById("<%=ddlBankCode.ClientID%>").focus();
             return false;
         }

         var uploadcontrol = document.getElementById('<%=UploadFile.ClientID%>').value;
        if (uploadcontrol.length === 0) {
            alert("Choose File");
            return false;
        }

    }
</script>

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
                            
                <td>
                <table style="border-collapse:collapse;" class="menuoff" onmouseover="className='menuon';" onmouseout="className='menuoff';">
                <tr>
                <td><asp:ImageButton ID="ibtnSave" runat="server" ImageUrl="~/images/save.png" ToolTip="Save" /></td>
                <td><asp:Label ID="Label14" runat="server" Text="Save"></asp:Label></td>
                </tr>
                </table>
                </td>
                <%-- Editted By Zoya @27/02/2016--%>                                     
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
                           
                <td>
                <table  style="border-collapse:collapse;" class="menuoff" onmouseover="className='menuon';" onmouseout="className='menuoff';">
                <tr>
                <td><asp:ImageButton ID="ibtnPosting" runat="server" ImageUrl="~/images/gposting.png" ToolTip="Access Denied" /></td>
                <td><asp:Label ID="Label6" runat="server" Text="Posting"></asp:Label></td>
                </tr></table></td>
                         
                    <td style="float: left;display:None; visibility:collapse;">
                <table  style="border-collapse:collapse;" class="menuoff" onmouseover="className='menuon';" onmouseout="className='menuoff';">
                <tr>
                <td ><asp:ImageButton ID="ibtnOthers" runat="server" ImageUrl="~/images/gothers.gif" ToolTip="Cancel" Visible="false"/></td>
                <td ><asp:Label ID="Label5" runat="server" Text="Others" Visible="false"></asp:Label></td>
                </tr></table></td>
                           
                <td style="float: left" >
                <table style="border-collapse:collapse;" class="menuoff" onmouseover="className='menuon';" onmouseout="className='menuoff';">
                <tr>
                <td ><asp:ImageButton ID="ibtnCancel" runat="server" ImageUrl="~/images/cancel.png" ToolTip="Cancel" /></td>
                <td><asp:Label ID="Label18" runat="server" Text="Cancel"></asp:Label></td>
                </tr></table></td>
             <%-- Done Editted By Zoya @25/02/2016--%>

             <%-- Editted By Zoya @27/02/2016--%>      
            <td style=" float: left;display:None; visibility:collapse; width: 2%; height: 14px">
                <asp:ImageButton ID="ibtnFirst" runat="server" ImageUrl="~/images/new_last.png" Visible="false"/></td>
                      
            <td style="float: left;display:None; visibility:collapse;width: 2%; height: 14px">
                <asp:ImageButton ID="ibtnPrevs" runat="server" ImageUrl="~/images/new_prev.png" visible="false"/></td>
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
            <td style="width: 2%; height: 14px">
            </td>
            <%-- Done Editted By Zoya @27/02/2016--%>

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
           <%-- Editted By Zoya @27/02/2016--%>
             <%-- <td align="right" class="pagetext" style="height: 39px">
                <asp:Label ID="lblMenuName" runat="server" Text="CIMB Clicks Upload" Width="253px"></asp:Label></td>--%>
             <td align="right" class="pagetext" style="height: 39px;width:300px" >
                <asp:Label ID="lblMenuName" runat="server" Text="CIMB Clicks File Upload" Width="300px"></asp:Label></td>
           <%-- Done Editted By Zoya @27/02/2016--%>
        </tr>
    </table>
</asp:Panel>
    <asp:Panel ID="PnlAdd" runat="server"  Width="100%">
<div  style="border: thin solid #A6D9F4; width: 100%;   margin-left:5px;">
<table class="fields" style="width: 100%; height: 100%; text-align: left;">
    <tr>
        <td class="fields" style="width: 67px; height: 17px">
        </td>
        <td style="width: 43px; height: 17px">
            <asp:Label ID="lblMsg" runat="server" CssClass="lblError" Style="text-align: center"
                Width="348px"></asp:Label></td>
        <td style="width: 100px; height: 17px">
        </td>
    </tr>
    <tr>
    <td class="fields" style="width: 67px; height: 17px">
        &nbsp;<span style="font-size: 11pt; color: #ff0066">*</span>
            <asp:Label ID="Label10" runat="server" Text="Bank Code" Width="97px"></asp:Label></td>
    <td style="width: 43px; height: 17px">
        <span style="font-size: 11pt; color: #ff0066">
          <asp:DropDownList ID="ddlBankCode" runat="server" AppendDataBoundItems="True" TabIndex="11"
                Width="149px">
                <asp:ListItem Value="-1">--Select--</asp:ListItem>
            </asp:DropDownList>&nbsp;</span></td>
    <td style="width: 100px; height: 17px">
    </td>
</tr>
<tr>
    <td class="fields" style="width: 67px; height: 17px">
        &nbsp;<span style="font-size: 11pt; color: #ff0066">*</span>
            <asp:Label ID="lblUpload" runat="server" Text="Upload File"></asp:Label></td>
    <td style="width: 43px; height: 17px">
        <span style="font-size: 9pt; color: #ff0066">
            <asp:FileUpload ID="UploadFile" runat="server" Width="90px"  style="color: transparent" onchange="Reload()"/>
            <asp:Label ID="lblFileUpload" Text="" runat="server" />
        </span>
    </td>
    <td style="width: 100px; height: 17px">
    </td>
</tr>
    <tr>
        <td class="fields" style="width: 67px; height: 26px" colspan="2">
             <asp:Button ID="btnUpload" runat="server" Text="Upload Clicks File"></asp:Button>
        </td>
    </tr>
    
</table>
<br />
</div>
<!-- hidden field -->
<!-- added by Hafiz Roslan @ 12/01/2016-->
<input runat="server" type="hidden" id="hidHeaderNo" name="hidHeaderNo" />
<!-- end hidden field -->

</asp:Panel>
     <asp:Panel ID="pnlDisplay" runat="server" Width="100%" Visible="false">
        <fieldset style="width: 98%; padding: 2px; margin-left:5px; border: thin solid #A6D9F4">
            <legend style="margin-left: 10px">
                <asp:Label ID="lblSummary" runat="server" Text="File Summary" Font-Bold="true"></asp:Label>
            </legend>
            <table id="_summry" width="100%" runat="server">
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
                <tr id="batch_code" runat="server" visible="false">
                    <td style="width: 10%;">
                        <asp:Label ID="Label4" runat="server" Text="Batch Code" Font-Bold="true"></asp:Label>
                    </td>
                    <td>
                        <asp:Label ID="lblBatchCode" runat="server" Text=""></asp:Label>
                    </td>
                </tr>
            </table>
        </fieldset>
    </asp:Panel>

<asp:Panel ID="GridPanel" runat="server"  Width="100%">
    <table class="fields" style="width: 100%; height: 100%; text-align: left;">
        <%--Added By Zoya @8/03/2016--%>
        <tr>
         <td style="width: 43px; height: 17px">
            <asp:Label ID="StudentNameMsg" runat="server" CssClass="lblError" Style="text-align: right"
                Width="348px"></asp:Label>
         </td>
        </tr>
        <%--End Added By Zoya @8/03/2016--%>
    <tr>
        <td>   <asp:DataGrid ID="dgClicksTransactions" runat="server" AutoGenerateColumns="False" Width="100%">
                    <FooterStyle CssClass="dgFooterStyle" Height="20px" />
                <SelectedItemStyle CssClass="dgSelectedItemStyle" />
                <AlternatingItemStyle BackColor="Beige" CssClass="dgAlternatingItemStyle" Font-Bold="False"
                    Font-Italic="False" Font-Overline="False" Font-Strikeout="False" Font-Underline="False" />
                <ItemStyle CssClass="dgItemStyle" />
                <HeaderStyle BackColor="#CDD7EE" CssClass="dgHeaderStyle" Font-Bold="True" Font-Italic="False"
                    Font-Overline="False" Font-Size="Medium" Font-Strikeout="False" Font-Underline="False" />
                <Columns>
                    <asp:BoundColumn DataField="STUDENT_NAME" HeaderText="Student Name" ></asp:BoundColumn>
                    <asp:BoundColumn DataField="MATRIC_NO" HeaderText="Matric No" ></asp:BoundColumn>
                    <%--Edited by Zoya @7/03/2016--%>
                    <asp:BoundColumn DataField="IC_NO" HeaderText="Identity No" ></asp:BoundColumn>
                    <%--End Edited by Zoya @7/03/2016--%>
                    <asp:BoundColumn DataField="RECEIPT_NO" HeaderText="Receipt No"></asp:BoundColumn>
                    <asp:BoundColumn DataField="RECEIPT_DATE" HeaderText="Receipt Date"></asp:BoundColumn>
                    <asp:BoundColumn DataField="PAID_AMOUNT" HeaderText="Paid Amount"></asp:BoundColumn>
                    <%--<asp:BoundColumn DataField="BATCH_CODE" HeaderText="Batch Code"></asp:BoundColumn>--%>
               </Columns>
            </asp:DataGrid>
        </td>
    </tr>
     
    </table>
</asp:Panel>
</asp:Content>

