<%@ Page Language="VB" AutoEventWireup="false" CodeFile="RptDunningLetterWarningViewer.aspx.vb" Inherits="RptDunningLetterWarningViewer" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="style.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:Button ID="btnPrint" runat="server" Text="Print Dunning Letter" OnClientClick="return confirm('Are you want to Print This Report? Process cannot Be Undone.');" />
        &nbsp;<br />
        <asp:DataGrid ID="dgView" runat="server" AutoGenerateColumns="False" Width="100%" style="vertical-align: top">
           <FooterStyle CssClass="dgFooterStyle" Height="20px" />
           <SelectedItemStyle CssClass="dgSelectedItemStyle" />
           <AlternatingItemStyle BackColor="Beige" CssClass="dgAlternatingItemStyle" Font-Bold="False"
              Font-Italic="False" Font-Overline="False" Font-Strikeout="False" Font-Underline="False" />
           <ItemStyle CssClass="dgItemStyle" />
              <HeaderStyle BackColor="#CDD7EE" CssClass="dgHeaderStyle" Font-Bold="True" Font-Italic="False"
              Font-Overline="False" Font-Size="Medium" Font-Strikeout="False" Font-Underline="False" />
              <Columns>
                 <asp:BoundColumn DataField="MatricNo" HeaderText="Matric No."></asp:BoundColumn>
                 <asp:BoundColumn DataField="Name" HeaderText="Student Name"></asp:BoundColumn>
                 <asp:BoundColumn DataField="ICNo" HeaderText="Student IC No."></asp:BoundColumn>
                 <asp:BoundColumn DataField="PgID" HeaderText="Program Name"></asp:BoundColumn>
                 <asp:BoundColumn DataField="Semester" HeaderText="Semester"></asp:BoundColumn>
                 <asp:BoundColumn DataField="Code" HeaderText="Warning Code"></asp:BoundColumn>
                 <asp:BoundColumn DataField="Warning" HeaderText="Warning"></asp:BoundColumn>
              </Columns>
         </asp:DataGrid>
    </div>
    <p>
      <asp:Label ID="lblCountwarn" runat="server"></asp:Label>
    </p>
    </form>
</body>
</html>
