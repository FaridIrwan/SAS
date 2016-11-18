<%@ Page Language="VB" MasterPageFile="~/MasterPage3.master" AutoEventWireup="false" MaintainScrollPositionOnPostback="true"
    CodeFile="PTPTNFeeSetup.aspx.vb" Inherits="PTPTNFeeSetup" Title="Welcome To SAS" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <script language="javascript" type="Scripts/popcalendar.js"></script>
    <script language="javascript" type="Scripts/functions.js"></script>

     <script language="javascript" type="text/javascript">

         function isNumberKey(evt) {
             var charCode = (evt.which) ? evt.which : event.keyCode

             if (charCode > 31 && (charCode < 48 || charCode > 57)) {
                 alert("Enter Only Digits");
                 evt.preventDefault();
                 return false;
             }
         }

         function validate() {
             var re = /\s*((\S+\s*)*)/;

             if (document.getElementById("<%=ddlProgramEdit.ClientID%>").value == "0") {
                  alert("Select a Program Name");
                  document.getElementById("<%=ddlProgramEdit.ClientID%>").focus();
                 return false;
             }

             if (document.getElementById("<%=txtProgFee.ClientID%>").value.replace(re, "$1").length == 0) {
                 alert("Program Fee Field Cannot Be Blank");
                 document.getElementById("<%=txtProgFee.ClientID%>").focus();
                return false;
             }

         }

         function validateProgramName() {
             
             if (document.getElementById("<%=ddlProgram.ClientID%>").value == "0") {
                 alert("Select a Program Name");
                 document.getElementById("<%=ddlProgram.ClientID%>").focus();
                  return false;
              }
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
                <td>
                    <table style="border-collapse: collapse;" class="menuoff" onmouseover="className='menuon';"
                        onmouseout="className='menuoff';">
                        <tr>
                            <td style="width: 24px; height: 14px">
                                <asp:ImageButton ID="ibtnSave" runat="server" ImageUrl="~/images/save.png" ToolTip="Save" ValidationGroup="1" />
                            </td>
                            <td style="width: 3%; height: 14px">
                                <asp:Label ID="lblSave" runat="server" Text="Save" meta:resourcekey="Label14Resource1"></asp:Label>
                            </td>
                        </tr>
                    </table>
                </td>
                <td>
                    <table style="border-collapse: collapse;" class="menuoff" onmouseover="className='menuon';"
                        onmouseout="className='menuoff';">
                        <tr>
                            <td style="width: 24px; height: 14px">
                                <asp:ImageButton ID="ibtnOpen" runat="server" ImageUrl="~/images/edit.gif" ToolTip="Open" />
                            </td>
                            <td style="width: 3%; height: 14px">
                                <asp:Label ID="lblOpen" runat="server" Text="Open" meta:resourcekey="Label14Resource1"></asp:Label>
                            </td>
                        </tr>
                    </table>
                </td>
                <td>
                    <table style="border-collapse: collapse;" class="menuoff" onmouseover="className='menuon';"
                        onmouseout="className='menuoff';">
                        <tr>
                            <td style="width: 3%; height: 14px">
                                <asp:ImageButton ID="ibtnDelete" runat="server" ImageUrl="~/images/delete.png" />
                            </td>
                            <td style="width: 3%; height: 14px">
                                <asp:Label ID="lblDelete" runat="server" Text="Delete" meta:resourcekey="Label13Resource1"></asp:Label>
                            </td>
                        </tr>
                    </table>
                </td>
                <td>
                    <table style="border-collapse: collapse;" class="menuoff" onmouseover="className='menuon';"
                        onmouseout="className='menuoff';">
                        <tr>
                            <td style="width: 3%; height: 14px">
                                <asp:ImageButton ID="ibtnSearch" runat="server" ImageUrl="~/images/find.png" ToolTip="View" />
                            </td>
                            <td style="width: 3%; height: 14px">
                                <asp:Label ID="lblSearch" runat="server" Text="Search" meta:resourcekey="Label16Resource1"></asp:Label>
                            </td>
                        </tr>
                    </table>
                </td>
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
        <div align="center" style="border: thin solid #A6D9F4; margin: 5px; width: 99%">
            <br />
            <table width="100%" align="center">
                <tr>
                    <td>
                        <table width="100%">
                            <tr>
                                <td width="10%">
                                    <asp:Label ID="Label2" runat="server" Text="Program Name"></asp:Label>
                                </td>
                                <td align="left">
                                    <asp:DropDownList ID="ddlProgram" runat="server" Width="400px"></asp:DropDownList>
                                </td>
                            </tr>
                        </table>
                        <br />
                    </td>
                </tr>
            </table>
        </div>
        <div align="center" style="margin: 5px; width: 99%">
            <table width="100%" align="center">
                <tr>
                    <td align="center">
                        <asp:Label ID="lblDataGridMsg" runat="server" CssClass="userMsg"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:DataGrid ID="dgPTPTN" runat="server" AutoGenerateColumns="False" PageSize="4"
                            Width="99%" HorizontalAlign="Center">
                            <FooterStyle CssClass="dgFooterStyle" Height="20px" />
                            <SelectedItemStyle CssClass="dgSelectedItemStyle" />
                            <AlternatingItemStyle BackColor="Beige" CssClass="dgAlternatingItemStyle" Font-Bold="False"
                                Font-Italic="False" Font-Overline="False" Font-Strikeout="False" Font-Underline="False" />
                            <ItemStyle CssClass="dgItemStyle" />
                            <HeaderStyle BackColor="#CDD7EE" CssClass="dgHeaderStyle" Font-Bold="True" Font-Italic="False"
                                Font-Overline="False" Font-Size="Medium" Font-Strikeout="False" Font-Underline="False" />
                            <Columns>
                                <asp:TemplateColumn HeaderText="Select">
                                    <ItemTemplate>
                                        <asp:CheckBox ID="cbPTPTN" runat="server"/>
                                    </ItemTemplate>
                                    <HeaderStyle Width="5%" HorizontalAlign="Center" />
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:TemplateColumn>
                                <asp:BoundColumn DataField="AutoID" HeaderText="AutoID" Visible ="false">
                                    <HeaderStyle Width="5%" HorizontalAlign="Center" />
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:BoundColumn>
                                <asp:BoundColumn DataField="ProgCode" HeaderText="Program Code">
                                    <HeaderStyle Width="10%" HorizontalAlign="Center" />
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:BoundColumn>
                                <asp:BoundColumn DataField="ProgName" HeaderText="Program Name"></asp:BoundColumn>
                                <asp:BoundColumn HeaderText="Post Status" Visible="False">
                                    <HeaderStyle Width="45%" />
                                </asp:BoundColumn>
                                <asp:BoundColumn DataField="ProgFee" HeaderText="Fee">
                                    <HeaderStyle HorizontalAlign="Center" />
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:BoundColumn>
                            </Columns>
                        </asp:DataGrid>
                    </td>
                </tr>
            </table>
            <asp:HiddenField ID="hdnAutoID" runat="server"></asp:HiddenField>
        </div>
    </asp:Panel>

    <asp:Panel ID="pnlEdit" runat="server" Width="100%">
        <div align="center" style="margin: 5px; width: 99%">
            <table width="100%" align="center">
                <tr>
                    <td>
                        <fieldset style="width: 98%; padding: 5px; margin-left: 2px;">
                            <legend>
                                <asp:Label ID="Label1" runat="server" style="padding: 5px;"> PTPTN Fee Details </asp:Label>
                            </legend>
                            <table border="0" cellpadding="2" cellspacing="0" align="left" style="width: 100%; margin-left: 3px;">
                                <tr>
                                    <td align="left" width="10%">
                                        <asp:Label ID="lblProgName" runat="server" Text="Program Name"></asp:Label>
                                        &nbsp;<font color="red">*</font>
                                    </td>
                                    <td align="left" width="85%">
                                        <asp:DropDownList ID="ddlProgramEdit" runat="server" Width="400px">
                                        </asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="rfvddlProgramEdit" runat="server" ControlToValidate="ddlProgramEdit"
                                            Display="None" ErrorMessage="Please select" InitialValue="-1" ValidationGroup="1"/>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left">
                                        <asp:Label ID="lblProgFee" runat="server" Text="Program Fee"></asp:Label>
                                        &nbsp;<font color="red">*</font>
                                    </td>
                                    <td >
                                        <asp:TextBox ID="txtProgFee" runat="server" MaxLength="7" Width="100px" style="text-align:right"></asp:TextBox>
                                       <%-- <asp:RequiredFieldValidator ID="rfvProgFee" runat="server" ErrorMessage=" Program Fee Can not be Empty"
                                            Display="None" ControlToValidate="txtProgFee" ValidationGroup="1" />--%>
                                        <%--<asp:validationsummary id="Validationsummary1" runat="server" headertext="Validation Errors: "/>--%>
                                    </td>
                                </tr>
                               <%-- <tr>
                                    <td align="center" colspan="2">
                                        <asp:validationsummary id="valSummary" ShowMessageBox="true" runat="server" 
                                            headertext="Required Field Can't Empty." ValidationGroup="1" EnableClientScript="true"
                                            ShowSummary="false" />
                                    </td>
                                </tr>--%>
                                
                            </table>
                            &nbsp;
                        </fieldset>
                    </td>
                </tr>
            </table>
        </div>
    </asp:Panel>

</asp:Content>
