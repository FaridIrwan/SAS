<%@ Page Language="VB" AutoEventWireup="false" CodeFile="StudentNotes.aspx.vb" Inherits="StudentNotes" MaintainScrollPositionOnPostback="true" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Untitled Page</title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        &nbsp;</div>
        <asp:Repeater ID="StuNotesList" runat="server">        
        <ItemTemplate>          
            <ItemTemplate> 
            <%#DataBinder.Eval(Container.DataItem, "Remarks")%>         
            </ItemTemplate>    
            <SeparatorTemplate>                         
             <hr color="black" size="1"/> 
            </SeparatorTemplate>               
        </ItemTemplate>   
        </asp:Repeater>
        <asp:TextBox ID="txtRemarks" runat="server" Height="99px" MaxLength="250" TextMode="MultiLine" Width="333px"></asp:TextBox>
        <asp:Button ID="btnAdd" runat="server" Text="Save" /><br />
        <br />
        &nbsp;
        
        
    </form>
</body>
</html>
