<%@ Page Language="VB" MasterPageFile="~/MasterPage3.master" AutoEventWireup="false" MaintainScrollPositionOnPostback="true" 
    CodeFile="ChequeManagement.aspx.vb" Inherits="ChequeManagement" title="Cheque Management" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
   <script language="javascript" type="Scripts/popcalendar.js"></script>
		<script language="javascript" type="Scripts/functions.js"></script>
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
                        alert("Please Enter Correct Record No");
                        document.getElementById("<%=txtRecNo.ClientID%>").value=1;
                        document.getElementById("<%=txtRecNo.ClientID%>").focus();
                        return false;
               }
     }
     return true;
}
function CheckSearch()
{
    if (document.getElementById("<%=ddlChequefor.ClientID%>").value=="-1")
     {
                alert("Select a Cheque For to Search");
                document.getElementById("<%=ddlChequefor.ClientID%>").focus();
                return false;
     }
     return true;
}

function OpenWindow()
{
var batch = document.getElementById("<%=txtBatchid.ClientID%>").value;
var type = document.getElementById("<%=ddlChequefor.ClientID%>").value;
new_window=window.open('RptChequeManagementForm.aspx?bat='+batch+'&type='+type,'Hanodale','width=520,height=500,resizable=1,srcollable =yes');new_window.focus();
}

function CheckChequeDate()
{        
var digits="0123456789/";
     var temp;
     for (var i=0;i<document.getElementById("<%=txtChequeDate.ClientID %>").value.length;i++)
     {
               temp=document.getElementById("<%=txtChequeDate.ClientID%>").value.substring(i,i+1);
               if (digits.indexOf(temp)==-1)
               {
                        alert("Enter Valid Date (dd/mm/yyyy)");
                        document.getElementById("<%=txtChequeDate.ClientID%>").value="";
                        document.getElementById("<%=txtChequeDate.ClientID%>").focus();
                        return false;
                        
               }
     }
     return true;
}


function getChequeDate()
{        
popUpCalendar(document.getElementById("<%=ibtnChequetDate.ClientID%>"),document.getElementById("<%=txtChequeDate.ClientID%>"), 'dd/mm/yyyy')
    
}
function getpostconfirm()
{   
    
     if(document.getElementById("<%=lblStatus.ClientID%>").value == "New")
     {
        alert("Select a Record to Post");
        return false;
     }   
     if(document.getElementById("<%=lblStatus.ClientID%>").value == "Posted")
     {
        alert("Record Already Posted");
        return false;
     } 
     var ans = Validate()
if(ans == false)
{
   return false;
   }
   else
   {  
        if (confirm("Posted Record Cannot Be Altered, Do You Want To Proceed?"))
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}

function getconfirm()
{        
    if(document.getElementById("<%=lblStatus.ClientID%>").value == "Posted")
     {
        alert("Posted Record Cannot be Deleted");
        return false;
     } 
    if(document.getElementById("<%=lblStatus.ClientID%>").value == "New")
     {
        alert("Select a Record to Delete");
        return false;
     } 
        
    if(document.getElementById("<%=txtBatchid.ClientID%>").value == "")
    {
        alert("Select a Record");
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
function checknValue()	
{	
  
if (((event.keyCode < 48) || (event.keyCode > 57)) && (event.keyCode != 13)  && (event.keyCode != 46) )
	{
	alert("Enter Valid Amount");
	event.keyCode=0;
	}
}

function Chequealidate()
{
var re = /\s*((\S+\s*)*)/;
           if (document.getElementById("<%=txtCStart.ClientID%>").value.replace(re, "$1").length == 0)
      {
                 alert("Enter a Cheque Starting No");
                 document.getElementById("<%=txtCStart.ClientID%>").focus();
                 
                 return false;
      } 
             if (document.getElementById("<%=txtCEND.ClientID%>").value.replace(re, "$1").length == 0)
      {
                 alert("Enter a Cheque End No");
                 document.getElementById("<%=txtCEND.ClientID%>").focus();
                 return false;
      } 
   var str1  = document.getElementById("<%=txtCStart.ClientID %>").value;
   var str2  = document.getElementById("<%=txtCEND.ClientID %>").value;
   

 var str8 = isBigger(str2, str1)
     if(str8 ==false)
     {
     document.getElementById("<%=txtCEND.ClientID%>").focus();
     return false;
     }
return true;
 }

 function isBigger(a, b)
    {

    // Make sure they are integers
    var re = new RegExp('\\D','g');
    if (re.test(a) || re.test(b))
    {
    return false;
    }

    // Trim leading zeros
    a = a.replace(/^0+/g,'');
    b = b.replace(/^0+/g,'');

    // Check lengths
    if (b.length > a.length)
    {
    alert("End No Cannot be Lesser than Start No");
    document.getElementById("<%=txtCEND.ClientID%>").focus();
    
    return false;
    }
    // Compare digits
    var a = a.split('');
    var b = b.split('');
    
    for (var i=0, j=a.length; i<j; i++)
    {
    if (+a[i], b[i])return true;
    }
    return false;
    }
function Validate()
{
var re = /\s*((\S+\s*)*)/;
     if(document.getElementById("<%=lblStatus.ClientID%>").value == "Posted")
     {
        alert("Posted record cannot be edited.");
        return false;
     } 
        if (document.getElementById("<%=ddlChequefor.ClientID%>").value=="-1")
      {
                 alert("Select a Cheque For");
                 document.getElementById("<%=ddlChequefor.ClientID%>").focus();
                 return false;
      }
       if (document.getElementById("<%= txtBatchid.ClientID%>").value.replace(re, "$1").length == 0)
      {
                 alert("Batch Id Field Cannot Be Blank");
                 document.getElementById("<%= txtBatchid.ClientID%>").focus();
                 return false;
      }
           if (document.getElementById("<%=txtAllCode.ClientID%>").value.replace(re, "$1").length == 0)
      {
                 alert("Select a Payment No");
                 document.getElementById("<%=txtAllCode.ClientID%>").focus();
                 return false;
      } 

           if (document.getElementById("<%=txtBankCode.ClientID%>").value.replace(re, "$1").length == 0)
      {
                 alert("Select a Bank Code");
                 document.getElementById("<%=txtBankCode.ClientID%>").focus();
                 return false;
      } 
             if (document.getElementById("<%=txtDescri.ClientID%>").value.replace(re, "$1").length == 0)
      {
                 alert("Description Field Cannot Be Blank");
                 document.getElementById("<%=txtDescri.ClientID%>").focus();
                 return false;
      } 
  
          if (document.getElementById("<%=txtChequeDate.ClientID%>").value.replace(re, "$1").length == 0)
      {
                 alert("Cheque Date Field Cannot Be Blank");
                 document.getElementById("<%=txtChequeDate.ClientID%>").focus();
                 return false;
      }        
     
       var len = document.getElementById("<%=txtChequeDate.ClientID%>").value
      var RegExPattern = /^((((0?[1-9]|[12]\d|3[01])[\.\-\/](0?[13578]|1[02])[\.\-\/]((1[6-9]|[2-9]\d)?\d{2}))|((0?[1-9]|[12]\d|30)[\.\-\/](0?[13456789]|1[012])[\.\-\/]((1[6-9]|[2-9]\d)?\d{2}))|((0?[1-9]|1\d|2[0-8])[\.\-\/]0?2[\.\-\/]((1[6-9]|[2-9]\d)?\d{2}))|(29[\.\-\/]0?2[\.\-\/]((1[6-9]|[2-9]\d)?(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00)|00)))|(((0[1-9]|[12]\d|3[01])(0[13578]|1[02])((1[6-9]|[2-9]\d)?\d{2}))|((0[1-9]|[12]\d|30)(0[13456789]|1[012])((1[6-9]|[2-9]\d)?\d{2}))|((0[1-9]|1\d|2[0-8])02((1[6-9]|[2-9]\d)?\d{2}))|(2902((1[6-9]|[2-9]\d)?(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00)|00))))$/;
    var errorMessage = 'Enter Valid Cheque date in dd/mm/yyyy format.';
    if (document.getElementById("<%=txtChequeDate.ClientID%>").value == "") 
    { 
     
    // return false;
    } 
    else 
    {
          if (document.getElementById("<%=txtChequeDate.ClientID%>").value.match(RegExPattern)) 
           {
              if (len.length == 8 )
                   {
                       alert(errorMessage);
                       document.getElementById("<%=txtChequeDate.ClientID%>").value ="";
                       document.getElementById("<%=txtChequeDate.ClientID%>").focus();
                       return false;
                   }
           } 
           else 
           {
                  alert(errorMessage);
                   document.getElementById("<%=txtChequeDate.ClientID%>").value ="";
                        document.getElementById("<%=txtChequeDate.ClientID%>").focus();
                      return false;
            } 
      }    
      var str1  = document.getElementById("<%=txtChequeDate.ClientID %>").value;
   var str2  = document.getElementById("<%=today.ClientID %>").value;
   var dt1   = parseInt(str1.substring(0,2),10); 
   var mon1  = parseInt(str1.substring(3,5),10);
   var yr1   = parseInt(str1.substring(6,10),10); 
   var dt2   = parseInt(str2.substring(0,2),10); 
   var mon2  = parseInt(str2.substring(3,5),10); 
   var yr2   = parseInt(str2.substring(6,10),10); 
   var date1 = new Date(yr1, mon1, dt1); 
   var date2 = new Date(yr2, mon2, dt2);

   if (yr1 == yr2 & mon1 == mon2)
{
}
else
{
      alert("Cheque Date Must be in the Current Month");
      document.getElementById("<%=txtChequeDate.ClientID%>").value ="";
      document.getElementById("<%=txtChequeDate.ClientID%>").focus();

      return false; 
}
  
                   
      if (document.getElementById("<%=txtPaymentDate.ClientID%>").value=="")
      {
                 alert("Transaction Date Field Cannot Be Blank");
                 document.getElementById("<%=txtPaymentDate.ClientID%>").focus();
                 return false;
      }
      
  
       var len = document.getElementById("<%=txtPaymentDate.ClientID%>").value
      var RegExPattern = /^((((0?[1-9]|[12]\d|3[01])[\.\-\/](0?[13578]|1[02])[\.\-\/]((1[6-9]|[2-9]\d)?\d{2}))|((0?[1-9]|[12]\d|30)[\.\-\/](0?[13456789]|1[012])[\.\-\/]((1[6-9]|[2-9]\d)?\d{2}))|((0?[1-9]|1\d|2[0-8])[\.\-\/]0?2[\.\-\/]((1[6-9]|[2-9]\d)?\d{2}))|(29[\.\-\/]0?2[\.\-\/]((1[6-9]|[2-9]\d)?(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00)|00)))|(((0[1-9]|[12]\d|3[01])(0[13578]|1[02])((1[6-9]|[2-9]\d)?\d{2}))|((0[1-9]|[12]\d|30)(0[13456789]|1[012])((1[6-9]|[2-9]\d)?\d{2}))|((0[1-9]|1\d|2[0-8])02((1[6-9]|[2-9]\d)?\d{2}))|(2902((1[6-9]|[2-9]\d)?(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00)|00))))$/;
    var errorMessage = 'Enter Valid Transaction Date in dd/mm/yyyy format.';
    if (document.getElementById("<%=txtPaymentDate.ClientID%>").value == "") 
    { 
     
    // return false;
    } 
    else 
    {
          if (document.getElementById("<%=txtPaymentDate.ClientID%>").value.match(RegExPattern)) 
           {
              if (len.length == 8 )
                   {
                       alert(errorMessage);
                       document.getElementById("<%=txtPaymentDate.ClientID%>").value ="";
                       document.getElementById("<%=txtPaymentDate.ClientID%>").focus();
                       return false;
                   }
           } 
           else 
           {
                  alert(errorMessage);
                   document.getElementById("<%=txtPaymentDate.ClientID%>").value ="";
                        document.getElementById("<%=txtPaymentDate.ClientID%>").focus();
                      return false;
            } 
      }
      var str1  = document.getElementById("<%=txtPaymentDate.ClientID %>").value;
   var str2  = document.getElementById("<%=today.ClientID %>").value;
   var dt1   = parseInt(str1.substring(0,2),10); 
   var mon1  = parseInt(str1.substring(3,5),10);
   var yr1   = parseInt(str1.substring(6,10),10); 
   var dt2   = parseInt(str2.substring(0,2),10); 
   var mon2  = parseInt(str2.substring(3,5),10); 
   var yr2   = parseInt(str2.substring(6,10),10); 
   var date1 = new Date(yr1, mon1, dt1); 
   var date2 = new Date(yr2, mon2, dt2); 

   if(date2 < date1)
   {
      alert("Payment Date Cannot be Greater than Current Date");
      document.getElementById("<%=txtPaymentDate.ClientID%>").value ="";
      document.getElementById("<%=txtPaymentDate.ClientID%>").focus();
      //"");
      return false; 
   }    
   

      return true;
}	
</script>
<%--<atlas:ScriptManager ID="scriptmanager1" EnablePartialRendering="true" runat="Server" />
   <atlas:UpdatePanel ID="up1" runat="server">
	<ContentTemplate>--%>
    <asp:Panel ID="Panel1" runat="server" Width="100%">
                <table style="background-image:url(images/Sample.png);">
            <tr>
                <td style="width: 4px; height: 14px">
                </td>
                <td style="width: 14px; height: 14px">
                </td>
                
                <td>
                 <table  style="border-collapse:collapse;" class="menuoff" onmouseover="className='menuon';" onmouseout="className='menuoff';">
                 <tr>
                <td style="height: 14px">
                    <asp:ImageButton ID="ibtnNew" runat="server" ImageUrl="~/images/add.png" ToolTip="New" OnClick="ibtnNew_Click" /></td>
                <td style="width: 3%; height: 14px">
                    <asp:Label ID="Label11" runat="server" Text="New"></asp:Label></td>
                    </tr></table></td>
                    
                    <td>
                 <table  style="border-collapse:collapse;" class="menuoff" onmouseover="className='menuon';" onmouseout="className='menuoff';">
                 <tr>
                <td style="width: 3%; height: 14px">
                    <asp:ImageButton ID="ibtnSave" runat="server" ImageUrl="~/images/save.png" ToolTip="Save" OnClick="ibtnSave_Click" /></td>
                <td style="width: 3%; height: 14px">
                    <asp:Label ID="Label14" runat="server" Text="Save"></asp:Label></td>
                    </tr></table></td>
                    
                    <td>
                 <table  style="border-collapse:collapse;" class="menuoff" onmouseover="className='menuon';" onmouseout="className='menuoff';">
                 <tr>
                <td style="width: 3%; height: 14px">
                    <asp:ImageButton ID="ibtnDelete" runat="server" ImageUrl="~/images/delete.png" OnClick="ibtnDelete_Click1"  /></td>
                <td style="width: 3%; height: 14px">
                    <asp:Label ID="Label13" runat="server" Text="Delete"></asp:Label></td>
                    </tr></table></td>
                    
                    <td>
                 <table  style="border-collapse:collapse;" class="menuoff" onmouseover="className='menuon';" onmouseout="className='menuoff';">
                 <tr>
                 <td style="width: 3%; height: 14px">
                               <div id="wrap">
		                            <ul id="navbar">
			                                <li><a href="#"><img src="images/find.png" width="24" height="24"  border="0" align="middle" />&nbsp;Search <img src="images/down.png" width="16" height="16"  border="0" align="middle"  /> </a>
				                                <ul>
					                                <li><a href="#"><asp:ImageButton ID="ibtnView" runat="server" ImageUrl="~/images/ready.png" OnClick="ibtnView_Click" /></a></li>
					                                <li><a href="#"><asp:ImageButton ID="ibtnOthers" runat="server" ImageUrl="~/images/post.png" OnClick="ibtnOthers_Click" /></li>
				                                </ul>			
			                                </li>
			                                </ul>
		
                                </div></td>
                    </tr></table></td>
                    
                    <td>
                 <table  style="border-collapse:collapse;" class="menuoff" onmouseover="className='menuon';" onmouseout="className='menuoff';">
                 <tr>
                <td style="width: 3%; height: 14px">
                    <asp:ImageButton ID="ibtnPrint" runat="server" ImageUrl="~/images/print.gif" ToolTip="Print" OnClick="ibtnPrint_Click1" /></td>
                <td style="width: 3%; height: 14px">
                    <asp:Label ID="Label17" runat="server" Text="Print"></asp:Label></td>
                    </tr></table></td>
                    
                    <td>
                 <table  style="border-collapse:collapse;" class="menuoff" onmouseover="className='menuon';" onmouseout="className='menuoff';">
                 <tr>
                <td style="width: 3%; height: 14px">
                    <asp:ImageButton ID="ibtnPosting" runat="server" ImageUrl="~/images/gposting.png"
                        ToolTip="Cancel" OnClick="ibtnPosting_Click" /></td>
                <td style="width: 3%; height: 14px"><asp:Label ID="Label6" runat="server" Text="Posting"></asp:Label></td>
                    </tr></table></td>
                    
                    <td>
                 <table  style="border-collapse:collapse;" class="menuoff" onmouseover="className='menuon';" onmouseout="className='menuoff';">
                 <tr>
                <td style="width: 3%; height: 14px">
                    <asp:ImageButton ID="ImageButton1" runat="server" ImageUrl="~/images/gothers.png" ToolTip="Cancel" OnClick="ibtnOthers_Click"  /></td>
                <td style="width: 3%; height: 14px">
                    <asp:Label ID="Label5" runat="server" Text="Others"></asp:Label></td>
                    </tr></table></td>
                    
                    <td>
                 <table  style="border-collapse:collapse;" class="menuoff" onmouseover="className='menuon';" onmouseout="className='menuoff';">
                 <tr>
                <td style="width: 3%; height: 14px">
                    <asp:ImageButton ID="ibtnCancel" runat="server" ImageUrl="~/images/cancel.png" ToolTip="Cancel" OnClick="ibtnCancel_Click"  /></td>
                <td style="width: 3%; height: 14px">
                    <asp:Label ID="Label18" runat="server" Text="Cancel"></asp:Label></td>
                    </tr></table></td>
              
                <td style="width: 2%; height: 14px">
                    <asp:ImageButton ID="ibtnFirst" runat="server" ImageUrl="~/images/new_last.png" OnClick="ibtnFirst_Click" /></td>
               
                <td style="width: 2%; height: 14px">
                    <asp:ImageButton ID="ibtnPrevs" runat="server" ImageUrl="~/images/new_prev.png" OnClick="ibtnPrevs_Click" /></td>
                <td style="width: 2%; height: 14px">
                    <asp:TextBox ID="txtRecNo" runat="server" AutoPostBack="True" MaxLength="7" Style="text-align: right" Width="52px" readonly="true"  CssClass="text_box" disabled="disabled" tabindex="1" dir="ltr"></asp:TextBox></td>
                <td style="width: 2%; height: 14px">
                    <asp:Label ID="Label47" runat="server">Of</asp:Label></td>
                    <td style="width: 2%; height: 14px"><asp:Label ID="lblCount" runat="server" Width="20px"></asp:Label></td>
              
                <td style="width: 2%; height: 14px">
                    <asp:ImageButton ID="ibtnNext" runat="server" ImageUrl="~/images/next.png" OnClick="ibtnNext_Click" /></td>
              
                <td style="width: 2%; height: 14px">
                    <asp:ImageButton ID="ibtnLast" runat="server" ImageUrl="~/images/new_first.png" OnClick="ibtnLast_Click" /></td>
                <td style="width: 2%; height: 14px">
                </td>
                <td style="width: 100%; height: 14px">
                </td>
                <td style="width: 100%; height: 14px">
                </td>
            </tr>
        </table>
        <table style="width :100%">
     <tr>
            <td class="vline" style="width: 100%; height: 1px">
                </td>
        </tr>
    </table>
    <table style="width: 100%">
        <tr>
            <td style="width: 400px">
                <asp:SiteMapPath ID="SiteMapPath1" runat="server">
                </asp:SiteMapPath>
            </td>
            <td class="pagetext" style="text-align: right">
                <asp:Label ID="lblMenuName" runat="server" Width="422px"></asp:Label></td>
        </tr>
    </table>
        <table>
            <tr>
                <td style="width: 100px">
                    <asp:Label ID="lblMsg" runat="server" CssClass="lblError" Style="text-align: center"
                        Width="301px"></asp:Label></td>
            </tr>
        </table>
    </asp:Panel>
     <%--</ContentTemplate>     
</atlas:UpdatePanel>
<atlas:UpdateProgress ID="ProgressIndicator" runat="server">
    <ProgressTemplate>
        Loading the data, please wait... 
        <asp:Image ID="LoadingImage" ImageAlign="AbsMiddle" runat="server" ImageUrl="~/Images/spinner.gif" />        
    </ProgressTemplate>
 </atlas:UpdateProgress>
<atlas:UpdatePanel ID="up2" runat="server">
<ContentTemplate>--%>
    <asp:Panel ID="Panel2" runat="server" Width="100%">
       <table style="width: 100%">
            <tr>              
                <td style="width: 100%">
               <div  style="border: thin solid #A6D9F4; width: 100%">
        <table width="500">
            <tr>
                <td style="width: 2%; height: 16px;">
                </td>
                <td style="width: 13%; height: 16px;">
                </td>
                <td colspan="4" style="text-align: left; height: 16px;">
                    </td>
                <td colspan="2" style="height: 16px">
                </td>
                <td style="width: 378px; height: 16px;">
                </td>
                <td style="width: 125px; height: 16px;">
                </td>
                <td style="height: 16px">
                </td>
            </tr>
            <tr>
                <td style="width: 2%">
                </td>
                <td style="width: 13%">
                </td>
                <td style="width: 156px">
                    </td>
                <td style="width: 42%">
                </td>
                <td style="width: 1px">
                </td>
                <td style="text-align: right; width: 138px;">
                    </td>
                <td colspan="2">
                </td>
                <td style="width: 378px">
                </td>
                <td style="width: 125px">
                </td>
                <td>
                </td>
            </tr>
            <tr>
                <td style="width: 2%; height: 13px;">
                </td>
                <td style="width: 13%; height: 13px;">
                    <asp:Label ID="Label3" runat="server" Text="Cheque For" Width="97px"></asp:Label></td>
                <td style="width: 156px; height: 13px;">
                    <span style="color: #ff0000">
                        <asp:DropDownList ID="ddlChequefor" runat="server" Width="149px" AutoPostBack="True" OnSelectedIndexChanged="ddlChequefor_SelectedIndexChanged" >
                            <asp:ListItem Value="-1">--Select--</asp:ListItem>
                            <asp:ListItem Value="1">Allocation</asp:ListItem>
                            <asp:ListItem Value="St">Refund</asp:ListItem>
                            <asp:ListItem Value="2">Sponsor Payments</asp:ListItem>
                        </asp:DropDownList>*</span></td>
                <td style="width: 10%; height: 13px;">
                    <asp:Image ID="ibtnAll" runat="server" Height="16px" ImageUrl="~/images/find_img.png"
                        Visible="False" Width="16px" ImageAlign=Left />
                    <asp:Image ID="ibtnRef" runat="server" Height="16px" ImageUrl="~/images/find_img.png"
                        Visible="False" Width="16px" />
                    <asp:Image ID="ibtnSpPay" runat="server" Height="16px" ImageUrl="~/images/find_img.png"
                        Visible="False" Width="16px" /></td>
                <td style="height: 13px; width: 1px;">
                </td>
                <td style="text-align: left; height: 13px; width: 138px;">
                    <asp:Label ID="Label4" runat="server" Style="text-align: left" Text="Process Id" Width="97px"></asp:Label></td>
                <td colspan="2" style="height: 13px">
                    <span
                        style="color: #ff0000">
                        <asp:TextBox ID="txtBatchid" runat="server" MaxLength="20" Style="text-align: left"
                            Width="115px" ReadOnly="True">Auto Number</asp:TextBox>*</span></td>
                <td style="width: 378px; height: 13px;">
                </td>
                <td style="width: 125px; height: 13px;">
                        </td>
                <td style="height: 13px;">
                </td>
            </tr>
            <tr>
                <td style="width: 2%; height: 27px;">
                </td>
                <td style="width: 13%; height: 27px;">
                    <asp:Label ID="Label15" runat="server" Text="Payment No" Width="97px"></asp:Label></td>
                <td style="width: 156px; height: 27px;">
                    <span
                        style="color: #ff0000">
                    <asp:TextBox ID="txtAllCode" runat="server" MaxLength="20" Width="143px" ReadOnly="True"></asp:TextBox>*</span></td>
                <td style="width: 42%; height: 27px">
                </td>
                <td style="height: 27px; width: 1px;">
                </td>
                <td style="text-align: left; height: 27px; width: 138px;">
                    <asp:Label ID="Label7" runat="server" Text="Batch Date  " Width="97px" style="text-align: left"></asp:Label></td>
                <td style="height: 27px; width: 117px;">
                    <asp:TextBox ID="txtBDate" runat="server" MaxLength="20" Style="text-align: left"
                        Width="115px"></asp:TextBox><span style="color: #ff0000"></span></td>
                <td style="width: 67px; height: 27px;">
                    </td>
                <td rowspan="3" style="width: 378px">
                </td>
                <td style="width: 125px" rowspan="3">
                    <asp:ImageButton ID="ibtnStatus" runat="server" Enabled="False" ImageUrl="~/images/NotReady.gif" CssClass="cursor" /></td>
                <td rowspan="3">
                </td>
            </tr>
            <tr>
                <td style="width: 2%; height: 16px">
                </td>
                <td style="width: 13%; height: 16px;">
                    <asp:Label ID="Label10" runat="server" Text="Bank Code" Width="97px"></asp:Label></td>
                <td style="width: 156px; height: 16px;">
                    <span
                        style="color: #ff0000">
                        <asp:TextBox ID="txtBankCode" runat="server" Width="142px"></asp:TextBox>*</span></td>
                <td style="width: 42%; height: 16px">
                </td>
                <td style="height: 16px; width: 1px;">
                </td>
                <td style="text-align: left; height: 16px; width: 138px;">
                    <asp:Label ID="Label9" runat="server" Text="Transaction Date  " Width="97px" style="text-align: left"></asp:Label></td>
                <td style="height: 16px; width: 117px;">
                    <asp:TextBox ID="txtPaymentDate" runat="server" MaxLength="20" Width="115px"></asp:TextBox><span
                        style="color: #ff0000"></span></td>
                <td style="height: 16px; width: 67px;">
                    </td>
            </tr>
            <tr>
                <td style="width: 2%; height: 23px">
                </td>
                <td style="width: 13%; height: 23px;">
                    <asp:Label ID="Label20" runat="server" Text="Description" Width="97px"></asp:Label></td>
                <td colspan="2" style="height: 23px">
                    <span
                        style="color: #ff0000">
                    <asp:TextBox ID="txtDescri" runat="server" MaxLength="50" Width="241px"></asp:TextBox>*</span></td>
                <td style="height: 23px; width: 1px;">
                </td>
                 <td style="text-align: left; height: 23px; width: 138px;">
                     <asp:Label ID="Label1" runat="server" Style="text-align: left" Text="Cheque Date"
                         Width="97px"></asp:Label></td>
                <td style="height: 23px; width: 117px;">
                    <span
                        style="color: #ff0000">
                        <asp:TextBox ID="txtChequeDate" runat="server" Width="116px" MaxLength="10"></asp:TextBox></span></td>
                <td style="width: 117px; height: 23px">
                    <asp:Image ID="ibtnChequetDate" runat="server" ImageUrl="~/images/cal.gif" /></td>
            </tr>
            <tr>
                <td style="width: 2%; height: 23px">
                </td>
                <td style="width: 13%; height: 23px">
                    <asp:Label ID="Label2" runat="server" Text="Cheque Starting No"></asp:Label></td>
                <td colspan="1" style="height: 23px">
                    <asp:TextBox ID="txtCStart" runat="server" Width="142px"></asp:TextBox></td>
                <td colspan="2" style="height: 23px">
                    <asp:Button ID="ChequeNo" runat="server" Text="Add Cheques" Width="106px"  OnClick="ChequeNo_Click" /></td>
                <td colspan="1" style="width: 138px; height: 23px">
                </td>
                <td style="width: 3px; height: 23px">
                </td>
                <td style="width: 138px; height: 23px; text-align: left">
                    </td>
                <td style="width: 117px; height: 23px">
                    </td>
                <td rowspan="1" style="width: 378px">
                </td>
                <td rowspan="1" style="width: 125px">
                </td>
                <td rowspan="1">
                    </td>
            </tr>
            <tr>
                <td style="width: 2%; height: 25px">
                </td>
                <td style="width: 13%; height: 25px">
                    <asp:Label ID="Label8" runat="server" Text="Cheque End No"></asp:Label></td>
                <td colspan="1" style="height: 25px">
                    <asp:TextBox ID="txtCEND" runat="server" Width="142px"></asp:TextBox></td>
                <td colspan="2" style="height: 25px">
                    <asp:Button ID="Remove" runat="server" Text="Remove Cheques" Width="108px" OnClick="Remove_Click" /></td>
                <td colspan="1" style="width: 138px; height: 25px">
                </td>
                <td style="width: 3px; height: 25px">
                </td>
                <td style="width: 138px; height: 25px; text-align: left">
                </td>
                <td style="width: 117px; height: 25px">
                </td>
                <td rowspan="1" style="width: 378px; height: 25px">
                </td>
                <td rowspan="1" style="width: 125px; height: 25px">
                </td>
                <td rowspan="1" style="height: 25px">
                </td>
            </tr>
        </table>
        <asp:Panel ID="pnlStudentGrid" runat="server" Width="100%">
            <table width="100%">
                <tr>
                    <td style="height: 16px"; width="1%">
                    </td>
                    <td style="width: 100%;">
                        &nbsp;&nbsp;&nbsp;
                        <asp:Panel ID="Panel4" runat="server"  Width="100%">
                        <asp:DataGrid ID="dgCheque" runat="server" AutoGenerateColumns="False"
                            Width="50%" OnSelectedIndexChanged="dgCheque_SelectedIndexChanged">
                            <FooterStyle CssClass="dgFooterStyle" Height="20px" />
                            <SelectedItemStyle CssClass="dgSelectedItemStyle" />
                            <AlternatingItemStyle BackColor="Beige" CssClass="dgAlternatingItemStyle" Font-Bold="False"
                                Font-Italic="False" Font-Overline="False" Font-Strikeout="False" Font-Underline="False" />
                            <ItemStyle CssClass="dgItemStyle" />
                            <HeaderStyle BackColor="#CDD7EE" CssClass="dgHeaderStyle" Font-Bold="True" Font-Italic="False"
                                Font-Overline="False" Font-Size="Medium" Font-Strikeout="False" Font-Underline="False" />
                            <Columns>
                                <asp:ButtonColumn CommandName="Select" HeaderText="Start No" DataTextField="ChequeStartNo"></asp:ButtonColumn>
                                <asp:BoundColumn DataField="ChequeEndNo" HeaderText="End No"></asp:BoundColumn>
                                <asp:BoundColumn DataField="Number" HeaderText="No.Of Cheques"></asp:BoundColumn>
                            </Columns>
                        </asp:DataGrid></asp:Panel>
                        &nbsp; &nbsp;
                        <asp:Panel ID="Panel3" runat="server" Width="100%">
                        <asp:DataGrid ID="dgView" runat="server" AutoGenerateColumns="False"
                            Width="100%" >
                            <FooterStyle CssClass="dgFooterStyle" Height="20px" />
                            <SelectedItemStyle CssClass="dgSelectedItemStyle" />
                            <AlternatingItemStyle BackColor="Beige" CssClass="dgAlternatingItemStyle" Font-Bold="False"
                                Font-Italic="False" Font-Overline="False" Font-Strikeout="False" Font-Underline="False" />
                            <ItemStyle CssClass="dgItemStyle" />
                            <HeaderStyle BackColor="#CDD7EE" CssClass="dgHeaderStyle" Font-Bold="True" Font-Italic="False"
                                Font-Overline="False" Font-Size="Medium" Font-Strikeout="False" Font-Underline="False" />
                            <Columns>
                                <asp:TemplateColumn HeaderText="Select" Visible="False">
                                    <ItemTemplate>
                                        <asp:CheckBox ID="chk" runat="server"  AutoPostBack="True" />
                                    </ItemTemplate>
                                </asp:TemplateColumn>
                                <asp:BoundColumn DataField="MatricNO" HeaderText="Matric Id"></asp:BoundColumn>
                                <asp:BoundColumn DataField="StudentName" HeaderText="Name"></asp:BoundColumn>
                                <asp:BoundColumn DataField="ProgramId" HeaderText="Program" Visible="False"></asp:BoundColumn>
                                <asp:BoundColumn DataField="CurrentSemester" HeaderText="CurSem" Visible="False"></asp:BoundColumn>
                                <asp:BoundColumn HeaderText="OutStanding Amount" Visible="False">
                                    <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                        Font-Underline="False" HorizontalAlign="Right" />
                                    <HeaderStyle Font-Bold="True" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                        Font-Underline="False" HorizontalAlign="Center" />
                                </asp:BoundColumn>
                                <asp:TemplateColumn HeaderText="Amount" Visible="False">
                                    <ItemTemplate>
                                        <asp:TextBox ID="TxtAmt" runat="server" Style="text-align: right"  AutoPostBack="True"></asp:TextBox>
                                    </ItemTemplate>
                                    <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                        Font-Underline="False" HorizontalAlign="Center" />
                                    <HeaderStyle Font-Bold="True" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                        Font-Underline="False" HorizontalAlign="Center" />
                                </asp:TemplateColumn>
                                <asp:BoundColumn DataField="TransactionAmount" HeaderText="Amount" DataFormatString="{0:F}">
                                    <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                        Font-Underline="False" HorizontalAlign="Right" />
                                    <HeaderStyle Font-Bold="True" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                        Font-Underline="False" HorizontalAlign="Center" />
                                </asp:BoundColumn>
                                <asp:TemplateColumn HeaderText="Voucher" Visible="False">
                                    <ItemTemplate>
                                        <asp:TextBox ID="Voucher" runat="server"></asp:TextBox>
                                    </ItemTemplate>
                                    <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                        Font-Underline="False" HorizontalAlign="Center" />
                                    <HeaderStyle Font-Bold="True" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                        Font-Underline="False" HorizontalAlign="Center" />
                                </asp:TemplateColumn>
                                <asp:BoundColumn HeaderText="Voucher No" DataField="VoucherNo"></asp:BoundColumn>
                                <asp:BoundColumn HeaderText="Cheque No" DataField="ChequeNo"></asp:BoundColumn>
                                <asp:BoundColumn DataField="TransactionCode" HeaderText="TransactionCode" Visible="False">
                                </asp:BoundColumn>
                            </Columns>
                        </asp:DataGrid></asp:Panel>
                        
                                                <asp:Panel ID="Panel5" runat="server" Width="100%">
                        <asp:DataGrid ID="dgsponsor" runat="server" AutoGenerateColumns="False"
                            Width="100%" >
                            <FooterStyle CssClass="dgFooterStyle" Height="20px" />
                            <SelectedItemStyle CssClass="dgSelectedItemStyle" />
                            <AlternatingItemStyle BackColor="Beige" CssClass="dgAlternatingItemStyle" Font-Bold="False"
                                Font-Italic="False" Font-Overline="False" Font-Strikeout="False" Font-Underline="False" />
                            <ItemStyle CssClass="dgItemStyle" />
                            <HeaderStyle BackColor="#CDD7EE" CssClass="dgHeaderStyle" Font-Bold="True" Font-Italic="False"
                                Font-Overline="False" Font-Size="Medium" Font-Strikeout="False" Font-Underline="False" />
                            <Columns>
                                <asp:TemplateColumn HeaderText="Select" Visible="False">
                                    <ItemTemplate>
                                        <asp:CheckBox ID="chk" runat="server"  AutoPostBack="True" />
                                    </ItemTemplate>
                                </asp:TemplateColumn>
                                <asp:BoundColumn DataField="CreditRef" HeaderText="Sponsor Code"></asp:BoundColumn>
                                <asp:BoundColumn DataField="CrRefOne" HeaderText="Name"></asp:BoundColumn>
                                <asp:TemplateColumn HeaderText="Amount" Visible="False">
                                    <ItemTemplate>
                                        <asp:TextBox ID="TxtAmt" runat="server" Style="text-align: right"  AutoPostBack="True"></asp:TextBox>
                                    </ItemTemplate>
                                    <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                        Font-Underline="False" HorizontalAlign="Center" />
                                    <HeaderStyle Font-Bold="True" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                        Font-Underline="False" HorizontalAlign="Center" />
                                </asp:TemplateColumn>
                                <asp:BoundColumn DataField="TransactionAmount" HeaderText="Amount" DataFormatString="{0:F}">
                                    <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                        Font-Underline="False" HorizontalAlign="Right" />
                                    <HeaderStyle Font-Bold="True" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                        Font-Underline="False" HorizontalAlign="Center" />
                                </asp:BoundColumn>
                                <asp:TemplateColumn HeaderText="Voucher" Visible="False">
                                    <ItemTemplate>
                                        <asp:TextBox ID="Voucher" runat="server"></asp:TextBox>
                                    </ItemTemplate>
                                    <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                        Font-Underline="False" HorizontalAlign="Center" />
                                    <HeaderStyle Font-Bold="True" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                        Font-Underline="False" HorizontalAlign="Center" />
                                </asp:TemplateColumn>
                                <asp:BoundColumn HeaderText="Voucher No" DataField="VoucherNo" Visible="False"></asp:BoundColumn>
                                <asp:BoundColumn HeaderText="Cheque No"></asp:BoundColumn>
                                <asp:BoundColumn DataField="TransactionCode" HeaderText="TransactionCode" Visible="False">
                                </asp:BoundColumn>
                            </Columns>
                        </asp:DataGrid></asp:Panel>
                    <asp:HiddenField ID="lblStatus" runat="server" />
                        <asp:HiddenField ID="today" runat="server" />
                        &nbsp;
                    </td>
                    <td style="height: 16px" width="1%">
                    </td>
                </tr>
            </table>
        </asp:Panel>
       </div>
                      </td>
            </tr>
        </table>
    </asp:Panel>
     <asp:Button ID="btnHidden" runat="Server" OnClick="btnHidden_Click"  style="display:none" />
<%--</ContentTemplate>     
</atlas:UpdatePanel>--%>
</asp:Content>

