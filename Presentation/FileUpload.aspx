<%@ Page Language="VB" MasterPageFile="~/MasterPage3.master" AutoEventWireup="false" MaintainScrollPositionOnPostback="true"
    CodeFile="FileUpload.aspx.vb" Inherits="FileUpload" Title="Welcome To SAS" Culture="auto"
    meta:resourcekey="PageResource1" UICulture="auto" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <script language="javascript" src="Scripts/popcalendar.js"></script>
    <script language="javascript" src="Scripts/functions.js"></script>
    <script language="javascript" type="text/javascript">

        function Validation() {
            if (document.getElementById("<%=txtColumns.ClientID%>").value == "") {
                alert("Code Cannot Be Blank");
                document.getElementById("<%=txtColumns.ClientID%>").focus();
                return false;
            }
            if (document.getElementById("<%=txtMatrixNo.ClientID%>").value == "") {
                alert("Matrix Id Field Cannot Be Blank");
                document.getElementById("<%=txtMatrixNo.ClientID%>").focus();
                return false;
            }
            var digits = "0123456789";
            var temp;
            for (var i = 0; i < document.getElementById("<%=txtMatrixNo.ClientID %>").value.length; i++) {
                temp = document.getElementById("<%=txtMatrixNo.ClientID%>").value.substring(i, i + 1);
                if (digits.indexOf(temp) == -1) {
                    alert("Enter valid Matrix Id");
                    document.getElementById("<%=txtMatrixNo.ClientID%>").value = "";
                    document.getElementById("<%=txtMatrixNo.ClientID%>").focus();
                    return false;
                }
            }
            if (document.getElementById("<%=txtICNo.ClientID%>").value == "") {
                alert("ICNo Id Cannot Be Blank");
                document.getElementById("<%=txtICNo.ClientID%>").focus();
                return false;
            }
            var digits = "0123456789";
            var temp;
            for (var i = 0; i < document.getElementById("<%=txtICNo.ClientID %>").value.length; i++) {
                temp = document.getElementById("<%=txtICNo.ClientID%>").value.substring(i, i + 1);
                if (digits.indexOf(temp) == -1) {
                    alert("Enter valid IcNo Id");
                    document.getElementById("<%=txtICNo.ClientID%>").value = "";
                    document.getElementById("<%=txtICNo.ClientID%>").focus();
                    return false;
                }
            }
            if (document.getElementById("<%=txtName.ClientID%>").value == "") {
                alert("Name id Field Cannot Be Blank");
                document.getElementById("<%=txtName.ClientID%>").focus();
                return false;
            }
            var digits = "0123456789";
            var temp;
            for (var i = 0; i < document.getElementById("<%=txtName.ClientID %>").value.length; i++) {
                temp = document.getElementById("<%=txtName.ClientID%>").value.substring(i, i + 1);
                if (digits.indexOf(temp) == -1) {
                    alert(temp);
                    alert("Enter valid Name Id");
                    document.getElementById("<%=txtName.ClientID%>").value = "";
                    document.getElementById("<%=txtName.ClientID%>").focus();
                    return false;
                }
            }

            if (document.getElementById("<%=txtAmount.ClientID%>").value == "") {
                alert("Amount Id Field Cannot Be Blank");
                document.getElementById("<%=txtAmount.ClientID%>").focus();
                return false;
            }
            var digits = "0123456789";
            var temp;
            for (var i = 0; i < document.getElementById("<%=txtAmount.ClientID %>").value.length; i++) {
                temp = document.getElementById("<%=txtAmount.ClientID%>").value.substring(i, i + 1);
                if (digits.indexOf(temp) == -1) {
                    alert("Enter valid Amount Id");
                    document.getElementById("<%=txtAmount.ClientID%>").value = "";
                    document.getElementById("<%=txtAmount.ClientID%>").focus();
                    return false;
                }
            }

            if (document.getElementById("<%=txtDelimeter.ClientID%>").value == "") {
                alert("Delimeter Field Cannot Be Blank");
                document.getElementById("<%=txtDelimeter.ClientID%>").focus();
                return false;
            }

            if (document.getElementById("<%=txtStuName.ClientID%>").value == "") {
                alert("File Name Field Cannot Be Blank");
                document.getElementById("<%=txtStuName.ClientID%>").focus();
                return false;
            }


            if (document.getElementById("<%=txtRef.ClientID%>").value == "") {
                alert("Ref Field Cannot Be Blank");
                document.getElementById("<%=txtRef.ClientID%>").focus();
                return false;
            }
            var digits = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            var temp;
            for (var i = 0; i < document.getElementById("<%=txtRef.ClientID %>").value.length; i++) {
                temp = document.getElementById("<%=txtRef.ClientID%>").value.substring(i, i + 1);
                if (digits.indexOf(temp) == -1) {
                    alert("Enter valid Ref");
                    document.getElementById("<%=txtRef.ClientID%>").value = "";
                    document.getElementById("<%=txtRef.ClientID%>").focus();
                    return false;
                }
            }




            if (document.getElementById("<%=txtReference.ClientID%>").value == "") {
                alert("Reference Field Cannot Be Blank");

                document.getElementById("<%=txtReference.ClientID%>").focus();
                return false;
            }
            var digits = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            var temp;
            for (var i = 0; i < document.getElementById("<%=txtReference.ClientID %>").value.length; i++) {
                temp = document.getElementById("<%=txtReference.ClientID%>").value.substring(i, i + 1);
                if (digits.indexOf(temp) == -1) {
                    alert("Enter valid Reference");
                    document.getElementById("<%=txtReference.ClientID%>").value = "";
                    document.getElementById("<%=txtReference.ClientID%>").focus();
                    return false;
                }
            }


            return true;
        }
    </script>
    <asp:Panel ID="Panel1" runat="server" Width="100%">
        <table style="background-image: url(images/Sample.png);">
            <tr>
                <td style="width: 4px; height: 14px">
                </td>
                <td style="width: 14px; height: 14px">
                </td>
                <td>
                    <table style="border-collapse: collapse;" class="menuoff" onmouseover="className='menuon';"
                        onmouseout="className='menuoff';">
                        <tr>
                            <td>
                                <asp:ImageButton ID="ibtnNew" runat="server" ImageUrl="~/images/add.png" ToolTip="New" />
                            </td>
                            <td>
                                <asp:Label ID="Label4" runat="server" Text="New"></asp:Label>
                            </td>
                        </tr>
                    </table>
                </td>
                <td>
                    <table style="border-collapse: collapse;" class="menuoff" onmouseover="className='menuon';"
                        onmouseout="className='menuoff';">
                        <tr>
                            <td>
                                <asp:ImageButton ID="ImageButton1" runat="server" ImageUrl="~/images/save.png" />
                            </td>
                            <td>
                                <asp:Label ID="Label14" runat="server" Text="Save"></asp:Label>
                            </td>
                        </tr>
                    </table>
                </td>
                <td>
                    <table style="border-collapse: collapse;" class="menuoff" onmouseover="className='menuon';"
                        onmouseout="className='menuoff';">
                        <tr>
                            <td>
                                <asp:ImageButton ID="ibtnDelete" runat="server" ImageUrl="~/images/delete.png" />
                            </td>
                            <td>
                                <asp:Label ID="Label13" runat="server" Text="Delete"></asp:Label>
                            </td>
                        </tr>
                    </table>
                </td>
                <td>
                    <table style="border-collapse: collapse;" class="menuoff" onmouseover="className='menuon';"
                        onmouseout="className='menuoff';">
                        <tr>
                            <td>
                                <asp:ImageButton ID="ibtnV" runat="server" ImageUrl="~/images/find.png" />
                            </td>
                            <td>
                                <asp:Label ID="Label16" runat="server" Text="Search"></asp:Label>
                            </td>
                        </tr>
                    </table>
                </td>
                <td>
                    <table style="border-collapse: collapse;" class="menuoff" onmouseover="className='menuon';"
                        onmouseout="className='menuoff';">
                        <tr>
                            <td>
                                <asp:ImageButton ID="ibtnPrint" runat="server" ImageUrl="~/images/print.png" ToolTip="Print" />
                            </td>
                            <td>
                                <asp:Label ID="Label17" runat="server" Text="Print"></asp:Label>
                            </td>
                        </tr>
                    </table>
                </td>
                <td>
                    <table style="border-collapse: collapse;" class="menuoff" onmouseover="className='menuon';"
                        onmouseout="className='menuoff';">
                        <tr>
                            <td>
                                <asp:ImageButton ID="ibtnPosting" runat="server" ImageUrl="~/images/gposting.png"
                                    ToolTip="Cancel" Visible="false"/>
                            </td>
                            <td>
                                <asp:Label ID="Label8" runat="server" Text="Posting" Visible="false"></asp:Label>
                            </td>
                        </tr>
                    </table>
                </td>
                <td>
                    <table style="border-collapse: collapse;" class="menuoff" onmouseover="className='menuon';"
                        onmouseout="className='menuoff';">
                        <tr>
                            <td>
                                <asp:ImageButton ID="ibtnOthers" runat="server" ImageUrl="~/images/gothers.png" ToolTip="Others" />
                            </td>
                            <td>
                                <asp:Label ID="Label12" runat="server" Text="Others"></asp:Label>
                            </td>
                        </tr>
                    </table>
                </td>
                <td>
                    <table style="border-collapse: collapse;" class="menuoff" onmouseover="className='menuon';"
                        onmouseout="className='menuoff';">
                        <tr>
                            <td>
                                <asp:ImageButton ID="ibtnCancel" runat="server" ImageUrl="~/images/cancel.png" ToolTip="Cancel" />
                            </td>
                            <td>
                                <asp:Label ID="Label18" runat="server" Text="Cancel"></asp:Label>
                            </td>
                        </tr>
                    </table>
                </td>
                <td style="width: 2%; height: 14px">
                    <asp:ImageButton ID="ibtnFirst" runat="server" ImageUrl="~/images/new_last.png" />
                </td>
                <td style="width: 2%; height: 14px">
                    <asp:ImageButton ID="ibtnPrevs" runat="server" ImageUrl="~/images/new_prev.png" />
                </td>
                <td style="width: 2%; height: 14px">
                    <asp:TextBox ID="txtRecNo" runat="server" disabled="disabled" TabIndex="1" dir="ltr"
                        ReadOnly="true" CssClass="text_box" Width="52px" AutoPostBack="True" Style="text-align: right"
                        MaxLength="7" OnTextChanged="txtRecNo_TextChanged"></asp:TextBox>
                </td>
                <td style="width: 2%; height: 14px">
                    <asp:Label ID="Label47" runat="server">Of</asp:Label>
                </td>
                <td style="width: 2%; height: 14px">
                    <asp:Label ID="lblCount" runat="server" Width="20px"></asp:Label>
                </td>
                <td style="width: 2%; height: 14px">
                    <asp:ImageButton ID="ibtnNext" runat="server" ImageUrl="~/images/next.png" />
                </td>
                <td style="width: 2%; height: 14px">
                    <asp:ImageButton ID="ibtnLast" runat="server" ImageUrl="~/images/new_first.png" />
                </td>
                <td style="width: 2%; height: 14px">
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
                <td class="pagetext" style="height: 39px; text-align: right;">
                    <asp:Label ID="lblMenuName" runat="server" Text="File Upload" Width="354px"></asp:Label>
                </td>
            </tr>
        </table>
        <asp:Label ID="lblMsg" runat="server" CssClass="lblError" Style="text-align: center"
            Width="348px"></asp:Label></asp:Panel>
    <asp:Panel ID="Panel2" runat="server" Height="100%" Width="100%">
        <table style="width: 100%">
            <tr>
                <td style="width: 100%">
                    <div style="border: thin solid #A6D9F4; width: 100%">
                        <table width="100%">
                            <tr>
                                <td style="width: 6px">
                                </td>
                                <td style="width: 100px; text-align: left;">
                                </td>
                            </tr>
                        </table>
                        <table>
                            <tr>
                                <td style="width: 100px">
                                    <span style="color: #ff3366">*</span>
                                    <asp:Label ID="Label2" runat="server" Text="Code"></asp:Label>
                                </td>
                                <td style="width: 142px">
                                    <asp:TextBox ID="txtColumns" runat="server" Width="80px"></asp:TextBox>
                                </td>
                                <td style="width: 100px">
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 100px; height: 25px;">
                                    <span
                                        style="color: #ff3366">*</span>
                                    <asp:Label ID="Label3" runat="server" Text="Matrix Id"></asp:Label>
                                </td>
                                <td style="width: 142px; height: 25px;">
                                    <asp:TextBox ID="txtMatrixNo" runat="server" MaxLength="2" Width="80px"></asp:TextBox>
                                </td>
                                <td style="width: 100px; height: 25px;">
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 100px; height: 25px;">
                                    <span
                                        style="color: #ff3366">*</span>
                                    <asp:Label ID="Label5" runat="server" Text="ICNo Id"></asp:Label>
                                </td>
                                <td style="width: 142px; height: 25px;">
                                    <asp:TextBox ID="txtICNo" runat="server" MaxLength="2" Width="80px"></asp:TextBox>
                                </td>
                                <td style="width: 100px; height: 25px;">
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 100px; height: 25px;">
                                    <span
                                        style="color: #ff3366">*</span>
                                    <asp:Label ID="Label6" runat="server" Text="Name Id"></asp:Label>
                                </td>
                                <td style="width: 142px; height: 25px;">
                                    <asp:TextBox ID="txtName" runat="server" MaxLength="2" Width="80px"></asp:TextBox>
                                </td>
                                <td style="width: 100px; height: 25px;">
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 100px; height: 22px;">
                                    <span style="color: #ff3366">*</span>
                                    <asp:Label ID="Label7" runat="server" Text="Amount Id"></asp:Label>
                                </td>
                                <td style="width: 142px; height: 22px;">
                                    
                                        <asp:TextBox ID="txtAmount" runat="server" MaxLength="2" Width="80px"></asp:TextBox>
                                </td>
                                <td style="width: 100px; height: 22px;">
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 100px">
                                    <span style="color: #ff3366">*</span>
                                    <asp:Label ID="Label10" runat="server" Text="Delimeter"></asp:Label>
                                </td>
                                <td style="width: 142px">
                                    
                                        <asp:TextBox ID="txtDelimeter" runat="server" MaxLength="2" Width="80px"></asp:TextBox>
                                </td>
                                <td style="width: 100px">
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 100px; height: 24px">
                                    <span style="color: #ff3366">*</span>
                                    <asp:Label ID="Label11" runat="server" Text="File Name"></asp:Label>
                                </td>
                                <td style="width: 142px; height: 24px">
                                    <asp:TextBox ID="txtStuName" runat="server" Width="80px"></asp:TextBox>
                                </td>
                                <td style="width: 100px; height: 24px">
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 100px">
                                    <span style="color: #ff3366">*</span>
                                    <asp:Label ID="lblRef" runat="server" Text="Ref"></asp:Label>
                                </td>
                                <td style="width: 142px">
                                    <asp:TextBox ID="txtRef" runat="server" Width="80px"></asp:TextBox>
                                </td>
                                <td style="width: 100px">
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 100px">
                                    <span
                                        style="color: #cc3366">*</span>
                                    <asp:Label ID="Label9" runat="server" Text="Reference"></asp:Label>
                                </td>
                                <td style="width: 142px">
                                    <asp:TextBox ID="txtReference" runat="server" MaxLength="2" Width="80px"></asp:TextBox>
                                </td>
                                <td style="width: 100px">
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 100px">
                                    <asp:Label ID="Label1" runat="server" Text="Select File" Width="83px"></asp:Label>
                                </td>
                                <td style="width: 142px">
                                    <asp:FileUpload ID="FileUpload1" runat="server" />
                                </td>
                                <td style="width: 100px">
                                    <asp:Button ID="Button1" runat="server" Text="Create File" />
                                </td>
                            </tr>
                        </table>
                        <asp:HiddenField ID="HiddenField1" runat="server" Value="fuCode" />
                    </div>
                    <br />
                    &nbsp;<br />
                    <br />
                    <br />
                    <br />
                    <br />
                    &nbsp;&nbsp;<br />
                    <br />
                    &nbsp;<asp:Button ID="btnSave" runat="server" Style="left: 198px; position: relative;
                        top: -138px" Text="Save" Width="70px" Visible="False" />
    </asp:Panel>
</asp:Content>
