<%@ Page Language="VB" AutoEventWireup="false" CodeFile="RptRegistrationPaymentViewerEn.aspx.vb" Inherits="RptRegistrationPaymentViewerEn" %>

<%@ Register Assembly="CrystalDecisions.Web, Version=13.0.2000.0, Culture=neutral, PublicKeyToken=692fbea5521e1304"
    Namespace="CrystalDecisions.Web" TagPrefix="CR" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>Registration Payment Report</title>
    <link href="../crystalreportviewers13/css/default.css"
        rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
        <asp:Panel ID="Panel1" runat="server" ScrollBars="Both">
            <CR:CrystalReportViewer ID="CrystalReportViewer1" runat="server" AutoDataBind="true" EnableDrillDown="True" ToolPanelView="None" HasRefreshButton="True" />
        </asp:Panel>
    </form>
</body>
</html>
