<%@ Page Language="VB" MasterPageFile="~/MasterPage3.master" AutoEventWireup="false" MaintainScrollPositionOnPostback="true" CodeFile="Setup.aspx.vb" Inherits="Default4" title="Setup-SAS" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <table border="0" cellpadding="0" cellspacing="0" width="85%">
        <tr>
            <td style="height: 43px">
                &nbsp;<asp:SiteMapPath ID="SiteMapPath1" runat="server">
                </asp:SiteMapPath>
                <asp:SiteMapDataSource ID="SiteMapDataSource1" runat="server" />
            </td>
        </tr>
        <tr>
            <td>
                <div align="center">
                   <table border="0" cellpadding="0" cellspacing="0" width="500" >
                        <tr>
                        
                       
                            <td style="height: 14px">
                                 <br />
                                <br />
                                <br />
                                <br />
                                <br />
                                <br />
                                <!--<asp:ImageButton ID="ibtnSetup" runat="server" ImageUrl="~/images/setup.png"
                                    onmouseout="this.src='images/setup.png';" onmouseover="this.src='images/setup.png';" />-->
                                 
                                    <img src="images/setup.png" />   <br />
                                 
                                <br />
                                <br />
                                <br />
                            </td>
                        </tr>
                    </table>
                </div>
            </td>
        </tr>
        <tr>
            <td>
                &nbsp;</td>
        </tr>
    </table>
</asp:Content>

