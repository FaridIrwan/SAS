<%@ Page Language="VB" MasterPageFile="~/MasterPage3.master" AutoEventWireup="false" CodeFile="AutoNumber.aspx.vb" MaintainScrollPositionOnPostback="true" Inherits="AutoNumber" title="Untitled Page" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<script language="javascript" type="text/javascript">
function geterr()
{        
var digits="0123456789";
     var temp;
     for (var i=0;i<document.getElementById("<%=txtRecNo.ClientID %>").value.length;i++)
     {
               temp=document.getElementById("<%=txtRecNo.ClientID%>").value.substring(i,i+1);
               if (digits.indexOf(temp)==-1)
               {
                        alert("Enter Valid Record No");
                        document.getElementById("<%=txtRecNo.ClientID%>").value=1;
                        document.getElementById("<%=txtRecNo.ClientID%>").focus();
                        return false;
               }
     }
     return true;
}
function getconfirm()
{        
    if(document.getElementById("<%=txtANDesc.ClientID%>").value == "")
    {
          
             alert("Select a Record to Delete");
        return false;
     }
     else
     {
         if (confirm("Do you want to Delete Record?"))
            {
                return true;
            }
            else
            {
                return false;
            }
     }
     return true;
}
function validate()
{
      if (document.getElementById("<%=txtANDesc.ClientID%>").value=="")
      {
                 alert("Description Field Cannot Be Blank");
                 document.getElementById("<%=txtANDesc.ClientID%>").focus();
                 return false;
      }
       var digits="ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
     var temp;
     for (var i=0;i<document.getElementById("<%=txtPrefix.ClientID %>").value.length;i++)
     {
               temp=document.getElementById("<%=txtPrefix.ClientID%>").value.substring(i,i+1);
               if (digits.indexOf(temp)==-1)
               {
                        alert("Enter valid Prefix");
                        document.getElementById("<%=txtPrefix.ClientID%>").focus();
                        return false;
               }
     }
     var nodigits="0123456789";
     var temp1;
     for (var i=0;i<document.getElementById("<%=txtNoDigits.ClientID %>").value.length;i++)
     {
               temp1=document.getElementById("<%=txtNoDigits.ClientID%>").value.substring(i,i+1);
               if (nodigits.indexOf(temp1)==-1)
               {
                        alert("Enter valid number");
                        document.getElementById("<%=txtNoDigits.ClientID%>").focus();
                        return false;
               }
     }
      if (document.getElementById("<%=txtNoDigits.ClientID%>").value=="")
      {
                 alert("No of digits Field Cannot Be Blank");
                 document.getElementById("<%=txtNoDigits.ClientID%>").focus();
                 return false;
      }
      var startno="0123456789";
     var temp2;
     for (var i=0;i<document.getElementById("<%=txtStartNo.ClientID %>").value.length;i++)
     {
               temp2=document.getElementById("<%=txtStartNo.ClientID%>").value.substring(i,i+1);
               if (startno.indexOf(temp2)==-1)
               {
                        alert("Enter valid number");
                        document.getElementById("<%=txtStartNo.ClientID%>").focus();
                        return false;
               }
     }
      if (document.getElementById("<%=txtStartNo.ClientID%>").value=="")
      {
                 alert("Start Number Field Cannot be blank");
                 document.getElementById("<%=txtStartNo.ClientID%>").focus();
                 return false;
      }
      
      return true;
}

</script>
<%--<atlas:ScriptManager ID="scriptmanager1" EnablePartialRendering="true" runat="Server" />
   <atlas:UpdatePanel ID="up1" runat="server">
	<ContentTemplate>--%>
 <asp:Panel ID="Panel1" runat="server" Width="100%">
                      <table >
                        <tr>
                            <td style="width: 4px; height: 14px">
                            </td>
                            <td style="width: 14px; height: 14px">
                            </td>
                            <td style="height: 14px">
                                <asp:ImageButton ID="ibtnNew" runat="server" ImageUrl="~/images/add.png" 
                                    ToolTip="New" OnClick="ibtnNew_Click" /></td>
                            <td style="width: 39px; height: 14px">
                                <asp:Label ID="Label11" runat="server" Text="New"></asp:Label></td>
                            <td style="width: 34px; height: 14px">
                                <asp:ImageButton ID="ibtnSave" runat="server" ImageUrl="~/images/save.gif" ToolTip="Save" OnClick="ibtnSave_Click" /></td>
                            <td style="width: 35px; height: 14px">
                                <asp:Label ID="Label14" runat="server" Text="Save"></asp:Label></td>
                            <td style="height: 14px; width: 24px;">
                                <asp:ImageButton ID="ibtnDelete" runat="server" ImageUrl="~/images/delete.jpg" OnClick="ibtnDelete_Click" /></td>
                            <td style="width: 42px; height: 14px">
                                <asp:Label ID="Label13" runat="server" Text="Delete"></asp:Label></td>
                            <td style="width: 3%; height: 14px">
                                <asp:ImageButton ID="ibtnView" runat="server" ImageUrl="~/images/find.gif" ToolTip="View" OnClick="ibtnView_Click" /></td>
                            <td style="width: 6%; height: 14px">
                                <asp:Label ID="Label16" runat="server" Text="Search"></asp:Label></td>
                            <td style="width: 5%; height: 14px">
                                <asp:ImageButton ID="ibtnPrint" runat="server" ImageUrl="~/images/print.gif" ToolTip="Print" /></td>
                            <td style="width: 6%; height: 14px">
                                <asp:Label ID="Label17" runat="server" Text="Print"></asp:Label></td>
                            <td style="width: 2%; height: 14px">
                                <asp:ImageButton ID="ibtnPosting" runat="server" ImageUrl="~/images/gposting.gif"
                                    ToolTip="Cancel" /></td>
                            <td style="width: 2%; height: 14px">
                                &nbsp;<asp:Label ID="Label6" runat="server" Text="Posting"></asp:Label></td>
                            <td style="width: 4%; height: 14px"><asp:ImageButton ID="ibtnOthers" runat="server" ImageUrl="~/images/gothers.gif"
                                    ToolTip="Cancel" /></td>
                            <td style="width: 3%; height: 14px">
                                <asp:Label ID="Label5" runat="server" Text="Others"></asp:Label></td>
                            <td style="width: 3%; height: 14px">
                                <asp:ImageButton ID="ibtnCancel" runat="server" ImageUrl="~/images/cancel.png"
                                    ToolTip="Cancel" OnClick="ibtnCancel_Click" /></td>
                            <td style="width: 3%; height: 14px">
                                <asp:Label ID="Label18" runat="server" Text="Cancel"></asp:Label></td>
                            <td style="width: 3%; height: 14px">
                            </td>
                            <td style="width: 4%; height: 14px">
                                <asp:ImageButton ID="ibtnFirst" runat="server" ImageUrl="~/images/new_last.gif" OnClick="ibtnFirst_Click" /></td>
                            <td style="height: 14px; width: 4px;">
                            </td>
                            <td style="width: 4%; height: 14px">
                                <asp:ImageButton ID="ibtnPrevs" runat="server" ImageUrl="~/images/new_prev.gif" OnClick="ibtnPrevs_Click" /></td>
                            <td style="width: 2%; height: 14px">
                                <asp:TextBox ID="txtRecNo" runat="server" Width="52px" AutoPostBack="True" style="text-align: right" MaxLength="7"></asp:TextBox></td>
                            <td style="width: 5%; height: 14px">
                                <asp:Label ID="Label47" runat="server">Of</asp:Label></td>
                            <td style="width: 14%; height: 14px">
                                <asp:Label ID="lblCount" runat="server" Width="41px"></asp:Label></td>
                            <td style="width: 5%; height: 14px">
                                <asp:ImageButton ID="ibtnNext" runat="server" ImageUrl="~/images/next.gif" OnClick="ibtnNext_Click" /></td>
                            <td style="height: 14px">
                            </td>
                            <td style="width: 5%; height: 14px">
                                <asp:ImageButton ID="ibtnLast" runat="server" ImageUrl="~/images/new_first.gif" OnClick="ibtnLast_Click" /></td>
                            <td style="width: 8%; height: 14px">
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
                        <td class="pagetext" style="height: 39px">
                <asp:Label ID="lblMenuName" runat="server" Text="Auto Number" Width="253px"></asp:Label></td>
                        </tr>
                    </table>
                </asp:Panel>
 <%-- </ContentTemplate>     
</atlas:UpdatePanel>
<atlas:UpdateProgress ID="ProgressIndicator" runat="server">
    <ProgressTemplate>
        Loading the data, please wait... 
        <asp:Image ID="LoadingImage" ImageAlign="AbsMiddle" runat="server" ImageUrl="~/Images/spinner.gif" />        
    </ProgressTemplate>
 </atlas:UpdateProgress>
<atlas:UpdatePanel ID="up2" runat="server">
<ContentTemplate>
--%>
    <asp:Panel ID="PnlAdd" runat="server"  Width="100%">
      <table style="width: 100%">
            <tr>              
                <td style="width: 100%">
                <div  style="border: thin solid #A6D9F4; width: 100%">              
                    <table class="fields" style="width: 100%; height: 100%;">
                        <tr>
                            <td style="width: 3px; height: 26px">
                            </td>
                            <td class="fields" style="width: 67px; height: 26px">
                            </td>
                            <td style="width: 12px; height: 26px; text-align: center">
                                <asp:Label ID="lblMsg" runat="server" CssClass="lblError" Width="348px"></asp:Label></td>
                            <td style="width: 218px; height: 26px">
                            </td>
                            <td style="width: 100px; height: 26px">
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 3px; height: 26px">
                            </td>
                            <td class="fields" style="width: 67px; height: 26px">
                                <asp:Label ID="Label1" runat="server" Text="Description" Width="79px"></asp:Label></td>
                            <td style="width: 12px; height: 26px">
                                <span
                                    style="font-size: 11pt; color: #ff0066">
                                <asp:TextBox ID="txtANDesc" runat="server" Width="346px" MaxLength="50"></asp:TextBox>*</span></td>
                            <td style="width: 218px; height: 26px">
                            </td>
                            <td style="width: 100px; height: 26px">
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 3px; height: 21px">
                            </td>
                            <td class="fields" style="width: 67px; height: 21px">
                                <asp:Label ID="Label2" runat="server" Text="Prefix" Width="99px"></asp:Label></td>
                            <td style="width: 12px; height: 21px">
                                <span style="font-size: 11pt;
                                    color: #ff0066">
                                <asp:TextBox ID="txtPrefix" runat="server" Width="142px" style="text-align: left" MaxLength="4"></asp:TextBox>*</span></td>
                            <td style="width: 218px; height: 21px">
                            </td>
                            <td style="width: 100px; height: 21px">
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 3px">
                            </td>
                            <td class="fields" style="width: 67px">
                                <asp:Label ID="Label4" runat="server" Text="No Of Digits" Width="91px"></asp:Label></td>
                            <td style="width: 12px">
                                <asp:TextBox ID="txtNoDigits" runat="server" Width="142px" MaxLength="4"></asp:TextBox><span
                                    style="font-size: 11pt; color: #ff0066">*</span></td>
                            <td style="width: 218px">
                            </td>
                            <td style="width: 100px">
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 3px">
                            </td>
                            <td class="fields" style="width: 67px">
                                <asp:Label ID="Label3" runat="server" Text="Start Number" Width="78px"></asp:Label></td>
                            <td class="fields" style="width: 12px">
                                <asp:TextBox ID="txtStartNo" runat="server" Width="142px" MaxLength="4"></asp:TextBox><span style="font-size: 11pt"><span
                                    style="color: #ff0066; font-size: 8pt;">*</span><span style="color: #ff0066"></span></span></td>
                            <td style="width: 218px">
                                </td>
                            <td style="width: 100px">
                                </td>
                        </tr>
                        <tr>
                            <td style="width: 3px; height: 21px">
                            </td>
                            <td class="fields" style="width: 67px; height: 21px">
                                </td>
                            <td class="fields" style="width: 12px; height: 21px">
                                <asp:TextBox ID="txtANCode" runat="server" MaxLength="4" Visible="False" Width="142px"></asp:TextBox>
                                <asp:TextBox ID="txtCurNo" runat="server" MaxLength="4" Visible="False" Width="142px"></asp:TextBox></td>
                            <td style="width: 218px; height: 21px">
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
               
                <asp:Panel ID="pnlView" runat="server" Height="100%" Width="100%">
                </asp:Panel>
   <%--  </ContentTemplate>     
</atlas:UpdatePanel>--%>
</asp:Content>

