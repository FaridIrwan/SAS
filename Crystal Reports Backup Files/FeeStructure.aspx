<%@ Page Language="VB" MasterPageFile="~/MasterPage3.master" AutoEventWireup="false" MaintainScrollPositionOnPostback="true"
    CodeFile="FeeStructure.aspx.vb" Inherits="FeeStructure" Title="Setup - Fee Structure" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <script language="javascript" type="text/javascript">
        function geterr() {
            var digits = "0123456789";
            var temp;
            for (var i = 0; i < document.getElementById("<%=txtRecNo.ClientID %>").value.length; i++) {
                temp = document.getElementById("<%=txtRecNo.ClientID%>").value.substring(i, i + 1);
                if (digits.indexOf(temp) == -1) {
                    alert("Enter Valid Record No");
                    document.getElementById("<%=txtRecNo.ClientID%>").value = 1;
                    document.getElementById("<%=txtRecNo.ClientID%>").focus();
                    return false;
                }
            }
            return true;
        }
        function checknValue() {
            if (((event.keyCode < 48) || (event.keyCode > 57)) && (event.keyCode != 13) && (event.keyCode != 46)) {
                alert("Enter Valid Values");
                event.keyCode = 0;
            }
        }
        function getconfirm() {
            if (document.getElementById("<%=ddlProgram.ClientID%>").value == "-1") {
                alert("Select a Record to Delete");
                return false;
            }
            else {
                if (confirm("Do you want to Delete Record?")) {
                    return true;
                }
                else {
                    return false;
                }
            }
            return true;
        }

        function checkValue() {

            if (((event.keyCode < 48) || (event.keyCode > 57)) && (event.keyCode != 13) && (event.keyCode != 46)) {
                alert("Enter Correct Amount");
                event.keyCode = 0;
            }
        }
        function Validate() {
            var re = /\s*((\S+\s*)*)/;
            if (document.getElementById("<%=ddlProgram.ClientID%>").value == "-1") {
                alert("Select a Program");
                document.getElementById("<%=ddlProgram.ClientID%>").focus();
                return false;
            }
            if (document.getElementById("<%=ddlSem.ClientID%>").value == "-1") {
                alert("Select a Semester");
                document.getElementById("<%=ddlSem.ClientID%>").focus();
                return false;
            }
            if (document.getElementById("<%=ddlSession.ClientID%>").value == "-1") {
                alert("Select a Session");
                document.getElementById("<%=ddlSession.ClientID%>").focus();
                return false;
            }
            return true;
        }
    </script>

    <table style="background-image: url(images/Sample.png);" runat="server" id="tblMenu">
        <tr>
            <td style="width: 4px; height: 14px"></td>
            <td style="width: 14px; height: 14px"></td>
            <td>
                <table style="border-collapse: collapse;" class="menuoff" onmouseover="className='menuon';"
                    onmouseout="className='menuoff';">
                    <tr>
                        <td>
                            <asp:ImageButton ID="ibtnNew" runat="server" ImageUrl="images/add.png" ToolTip="New" />
                        </td>
                        <td>
                            <asp:Label ID="Label11" runat="server" Text="New"></asp:Label>
                        </td>
                    </tr>
                </table>
            </td>
            <td>
                <table style="border-collapse: collapse;" class="menuoff" onmouseover="className='menuon';"
                    onmouseout="className='menuoff';">
                    <tr>
                        <td>
                            <asp:ImageButton ID="ibtnSave" runat="server" ImageUrl="~/images/save.png" ToolTip="Save" />
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
                            <asp:ImageButton ID="ibtnView" runat="server" ImageUrl="~/images/find.png" ToolTip="View" />
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
                                ToolTip="Cancel" />
                        </td>
                        <td>
                            <asp:Label ID="Label6" runat="server" Text="Posting"></asp:Label>
                        </td>
                    </tr>
                </table>
            </td>
            <td>
                <table style="border-collapse: collapse;" class="menuoff" onmouseover="className='menuon';"
                    onmouseout="className='menuoff';">
                    <tr>
                        <td>
                            <asp:ImageButton ID="ibtnOthers" runat="server" ImageUrl="~/images/gothers.png"
                                ToolTip="Cancel" />
                        </td>
                        <td>
                            <asp:Label ID="Label5" runat="server" Text="Others"></asp:Label>
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
                    Width="52px" ReadOnly="true" CssClass="text_box" Style="text-align: right" AutoPostBack="True"
                    MaxLength="7"></asp:TextBox>
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
            <td style="width: 2%; height: 14px"></td>
            <td style="width: 100%; height: 14px"></td>
            <td style="width: 100%; height: 14px"></td>
        </tr>
    </table>
    <table style="width: 100%">
        <tr>
            <td class="vline" style="width: 746px; height: 1px"></td>
        </tr>
    </table>
    <table style="width: 100%">
        <tr>
            <td style="width: 400px">
                <asp:SiteMapPath ID="SiteMapPath1" runat="server">
                </asp:SiteMapPath>
            </td>
            <td class="pagetext" style="text-align: right; width: 100px;">
                <asp:Label ID="lblMenuName" runat="server" Width="350px"></asp:Label>
            </td>
        </tr>
    </table>

    <table style="width: 100%">
        <tr>
            <td style="width: 100%;">
                <div style="border: thin solid #A6D9F4; width: 100%">
                    <table class="fields" style="width: 100%; height: 52%;">
                        <tr>
                            <td style="width: 2px; height: 26px"></td>
                            <td class="fields" style="height: 26px; text-align: center;" colspan="4">
                                <asp:Label ID="lblMsg" runat="server" CssClass="lblError" Width="348px"></asp:Label>
                            </td>
                            <td style="width: 111px; height: 26px"></td>
                        </tr>
                        <tr>
                            <td style="width: 2px; height: 9px">
                                <span style="font-size: 11pt; color: #ff0066">*</span>
                            </td>
                            <td class="fields" style="width: 196px; height: 9px">
                                <asp:Label ID="Label1" runat="server" Text="Program"></asp:Label>
                            </td>
                            <td style="width: 21px; height: 9px">
                                <asp:DropDownList ID="ddlProgram" runat="server" Width="339px" AppendDataBoundItems="True"
                                    AutoPostBack="True" OnSelectedIndexChanged="ddlProgram_SelectedIndexChanged">
                                    <asp:ListItem Value="-1">--Select--</asp:ListItem>
                                </asp:DropDownList>
                            </td>
                            <td style="width: 21px; height: 9px">
                                <asp:CheckBox ID="ChkClone" runat="server" Text="Clone This" Width="150px" OnCheckedChanged="ChkClone_CheckedChanged"
                                    Visible="False" AutoPostBack="True" />
                            </td>
                            <td style="width: 250px; height: 9px">&nbsp;<asp:Label ID="Label9" runat="server" Text="Fee Category:" Visible="False"></asp:Label>
                                <asp:DropDownList ID="ddlFeeCategory" runat="server" Width="147px" AutoPostBack="True"
                                    Visible="False">
                                    <asp:ListItem Value="A">Admission Fee</asp:ListItem>
                                    <asp:ListItem Value="S">Semester Fee</asp:ListItem>
                                    <asp:ListItem Value="T">Tuition Fee</asp:ListItem>
                                </asp:DropDownList>
                            </td>
                            <td style="width: 111px; height: 9px"></td>
                        </tr>
                        <tr>
                            <td style="width: 2px; height: 15px">
                                <span style="font-size: 11pt; color: #ff0066">*</span>
                            </td>
                            <td class="fields" style="width: 196px; height: 15px">
                                <asp:Label ID="Label2" runat="server" Text="Semester"></asp:Label>
                            </td>
                            <td style="width: 21px; height: 15px">
                                <asp:DropDownList ID="ddlSem" runat="server" AppendDataBoundItems="True"
                                    AutoPostBack="True" DataTextField="Semester" DataValueField="Semester"
                                    Width="147px" DataSourceID="oDsEffFrom">
                                    <asp:ListItem Value="-1">--Select--</asp:ListItem>
                                </asp:DropDownList>
                                &nbsp;
                            </td>
                            <td style="width: 21px; height: 15px"></td>
                            <td style="width: 250px; height: 15px">
                                <span style="font-size: 11pt; color: #ff0066">
                                    <asp:TextBox ID="txtSemister" runat="server" Width="142px" Enabled="False" Visible="False"></asp:TextBox></span>
                            </td>
                            <td style="width: 250px; height: 15px"></td>
                            <td style="width: 111px; height: 15px"></td>
                        </tr>
                        <tr>
                            <td style="width: 2px; height: 1px;">
                                <span style="font-size: 11pt; color: #ff0066">*</span>
                            </td>
                            <td class="fields" style="width: 196px; height: 1px;">
                                <asp:Label ID="Label4" runat="server" Text="Session"></asp:Label>
                            </td>
                            <td style="width: 21px; height: 1px;">
                                <span style="font-size: 11pt; color: #ff0066">
                                    <asp:DropDownList ID="ddlSession" runat="server" AppendDataBoundItems="True" AutoPostBack="True"
                                        Width="147px">
                                    </asp:DropDownList>
                                </span>&nbsp;
                            </td>
                            <td style="width: 21px; height: 1px"></td>
                            <td style="width: 250px; height: 1px;">
                                <asp:TextBox ID="txtPgCode" runat="server" Width="142px" ReadOnly="True" Visible="False"></asp:TextBox>
                            </td>
                            <td style="width: 111px; height: 1px;"></td>
                        </tr>
                        <tr>
                            <td style="width: 2px; height: 1px"></td>
                            <td class="fields" style="width: 196px; height: 1px">
                                <asp:Label ID="Label3" runat="server" Text="Effective From" Visible="False"></asp:Label>
                            </td>
                            <td style="width: 21px; height: 1px">
                                <asp:DropDownList ID="ddlEFrom" runat="server" AppendDataBoundItems="True" AutoPostBack="True"
                                    DataSourceID="oDsEffFrom" DataTextField="SAST_Semester" DataValueField="SAST_Code"
                                    Width="147px" Visible="False">
                                    <asp:ListItem Value="-1">--Select--</asp:ListItem>
                                </asp:DropDownList>
                                <span style="font-size: 11pt; color: #ff0066"></span>
                            </td>
                            <td style="width: 21px; height: 1px"></td>
                            <td style="width: 250px; height: 1px">
                                <asp:TextBox ID="txtSession" runat="server" Enabled="False" Visible="False" Width="142px"></asp:TextBox>
                            </td>
                            <td style="width: 111px; height: 1px"></td>
                        </tr>
                        <tr>
                            <td style="width: 2px; height: 6px"></td>
                            <td class="fields" style="width: 196px; height: 6px; vertical-align: top;">
                                <asp:Label ID="Label10" runat="server" Style="vertical-align: middle" Text="Status"
                                    Width="78px"></asp:Label>
                            </td>
                            <td class="fields" style="width: 21px; height: 6px">
                                <asp:DropDownList ID="ddlStatus" runat="server" Width="146px">
                                    <asp:ListItem Value="1">Active</asp:ListItem>
                                    <asp:ListItem Value="0">Inactive</asp:ListItem>
                                </asp:DropDownList>
                                &nbsp;
                                <asp:ObjectDataSource ID="odsProgram" runat="server" SelectMethod="GetStudentProgram"
                                    TypeName="HTS.SAS.BS.bsProgram">
                                    <SelectParameters>
                                        <asp:Parameter DefaultValue="" Name="pG_Code" Type="String" />
                                        <asp:Parameter DefaultValue="1" Name="Status" Type="Int32" />
                                    </SelectParameters>
                                </asp:ObjectDataSource>
                                <asp:ObjectDataSource ID="oDsEffFrom" runat="server" SelectMethod="GetList" TypeName="HTS.SAS.BusinessObjects.SemesterSetupBAL"
                                    OldValuesParameterFormatString="original_{0}">
                                    <SelectParameters>
                                        <asp:Parameter Name="argEn" Type="Object" />
                                    </SelectParameters>
                                </asp:ObjectDataSource>
                                &nbsp;
                            </td>
                            <td style="width: 250px; height: 6px">
                                <asp:ImageButton ID="ibtnAddFeeType" runat="server" ToolTip="Add" ImageUrl="~/images/addrec.gif"
                                    Height="24px" Width="24px" />
                                <asp:ImageButton ID="ibtnRemoveFee" runat="server" Height="24px" ImageUrl="~/images/removey.gif"
                                    ToolTip="Remove" Width="24px" />
                            </td>
                            <td class="fields" style="width: 21px; height: 6px"></td>
                            <td style="width: 111px; height: 6px"></td>
                        </tr>
                    </table>
                </div>
            </td>
        </tr>
    </table>
    <table style="width: 80%; height: 9px">
        <tr>
            <td style="width: 1%; height: 12px"></td>
            <td style="width: 10%; height: 12px">
                <!--<asp:Image ID="imgLeft1" runat="server" ImageAlign="AbsBottom" ImageUrl="images/b_orange_left.gif" />-->
                <asp:Button ID="btnAdmission" runat="server" CssClass="TabButton" Height="24px" Text="Admission"
                    Width="108px" OnClick="btnAdmission_Click" /><!--<asp:Image ID="imgRight1" runat="server" ImageAlign="AbsBottom" ImageUrl="images/b_orange_right.gif" />-->
            </td>
            <td style="width: 10%; height: 12px">
                <!--<asp:Image ID="imgLeft2" runat="server" ImageAlign="AbsBottom" ImageUrl="images/b_orange_left.gif" />-->
                <asp:Button ID="btnSemester" runat="server" CssClass="TabButton" Height="25px" Text="Semester"
                    Width="108px" OnClick="btnSemester_Click" /><!--<asp:Image ID="imgRight2" runat="server" ImageAlign="AbsBottom" ImageUrl="images/b_orange_right.gif" />-->
            </td>
            <td style="width: 10%; height: 12px">
                <!--<asp:Image ID="imgLeft3" runat="server" ImageAlign="AbsBottom" ImageUrl="images/b_orange_left.gif" />-->
                <asp:Button ID="btnTution" runat="server" CssClass="TabButton" Height="25px" Text="Tuition"
                    Width="108px" OnClick="btnTution_Click" Visible="false" /><!--<asp:Image ID="imgRight3" runat="server" ImageAlign="AbsBottom" ImageUrl="images/b_orange_right.gif" />-->
            </td>
            <td style="width: 20%; height: 12px">
                <%-- </ContentTemplate> 
                </atlas:UpdatePanel>--%>
            </td>
            <td style="width: 20%; height: 12px"></td>
            <td style="width: 19%; height: 12px"></td>
        </tr>
    </table>
    <asp:MultiView ID="MultiView1" runat="server" ActiveViewIndex="0">
        <asp:View ID="View2" runat="server">
            <table style="width: 100%">
                <tr>
                    <td style="width: 100%">
                        <div style="border: thin solid #A6D9F4; width: 100%">
                            <asp:Panel Width="100%" ID="pnlSemester" runat="server">
                                <table style="width: 100%; height: 25%;">
                                    <tr>
                                        <td style="width: 31824px; height: 25px;"></td>
                                        <td class="fields" style="width: 256px; height: 25px">
                                            <asp:Label ID="Label7" runat="server" Text="Faculty Fee Type:" Width="102px"></asp:Label>
                                        </td>
                                        <td class="fields" style="width: 19%; height: 25px">
                                            <asp:DropDownList ID="ddlFeeFor" runat="server" Width="189px">
                                                <asp:ListItem Value="-1">--Select--</asp:ListItem>
                                                <asp:ListItem Value="1">Semester</asp:ListItem>
                                                <asp:ListItem Value="0">Annual</asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                        <td class="fields" style="width: 250px; height: 25px;"></td>
                                        <td style="width: 100px; height: 25px;"></td>
                                    </tr>
                                    <tr>
                                        <td style="width: 31824px; height: 26px"></td>
                                        <td class="fields" style="width: 256px; height: 26px">
                                            <asp:Label ID="Label19" runat="server" Text="Semester:" Width="102px" Visible="False"></asp:Label>
                                        </td>
                                        <td class="fields" style="width: 19%; height: 26px">
                                            <asp:DropDownList ID="ddlSemester" runat="server" Width="189px" AutoPostBack="True"
                                                Visible="False">
                                                <asp:ListItem Value="-1">--Select--</asp:ListItem>
                                                <asp:ListItem Value="0">For All Semester</asp:ListItem>
                                                <asp:ListItem Value="1">For individual Semester </asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                        <td class="fields" style="width: 84%; height: 26px">&nbsp;<asp:TextBox ID="txtSemster" runat="server" Width="42px" Visible="False"></asp:TextBox>
                                        </td>
                                        <td style="width: 100px; height: 26px"></td>
                                    </tr>
                                </table>
                            </asp:Panel>
                        </div>
                    </td>
                </tr>
            </table>
        </asp:View>
        <asp:View ID="View3" runat="server">
            <table style="width: 100%">
                <tr>
                    <td style="width: 100%">
                        <div style="border: thin solid #A6D9F4; width: 100%">
                            <asp:Panel Width="100%" ID="pnlTution" runat="server">
                                <table style="width: 100%; height: 38px;">
                                    <tr>
                                        <td style="width: 2px; height: 11px"></td>
                                        <td class="fields" style="width: 135px; height: 11px; vertical-align: bottom;">
                                            <asp:Label ID="Label8" runat="server" Text="Fee Based On" Width="112px"></asp:Label>
                                        </td>
                                        <td style="width: 279px; height: 11px; vertical-align: bottom;">
                                            <asp:DropDownList ID="ddlTution" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlTution_SelectedIndexChanged">
                                                <asp:ListItem Value="-1">--Select--</asp:ListItem>
                                                <asp:ListItem Value="0">Fixed for all subject</asp:ListItem>
                                                <asp:ListItem Value="1">Credit Point</asp:ListItem>
                                                <asp:ListItem Value="2">Credit Hours</asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                        <td style="width: 337px;"></td>
                                        <td style="width: 100px; height: 11px">&nbsp; &nbsp; &nbsp; &nbsp;
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 2px; height: 16px"></td>
                                        <td class="fields" style="width: 135px; height: 16px">
                                            <asp:Label ID="lblPoint" runat="server" Text="Point" Width="112px"></asp:Label>
                                        </td>
                                        <td style="width: 279px; height: 16px">
                                            <asp:TextBox ID="txtTutPoint" runat="server" Style="text-align: right"></asp:TextBox>
                                        </td>
                                        <td style="width: 337px; height: 16px"></td>
                                        <td style="width: 100px; height: 16px"></td>
                                    </tr>
                                    <tr>
                                        <td style="width: 2px; height: 16px"></td>
                                        <td class="fields" style="width: 135px; height: 16px">
                                            <asp:Label ID="Label12" runat="server" Text="Amount" Width="112px"></asp:Label>
                                        </td>
                                        <td class="fields" style="width: 279px; height: 16px">
                                            <asp:TextBox ID="txtTutAmt" runat="server" Style="text-align: right" AutoPostBack="True"
                                                OnTextChanged="txtTutAmt_TextChanged"></asp:TextBox>
                                            &nbsp;<asp:Label ID="lblCredit" runat="server" Text="/ Credit Hours" Visible="false" Width="110px"></asp:Label>
                                        </td>
                                        <td style="width: 337px; height: 16px"></td>
                                        <td style="width: 100px; height: 16px"></td>
                                    </tr>
                                </table>
                            </asp:Panel>
                        </div>
                    </td>
                </tr>
            </table>
        </asp:View>
        <asp:View ID="View4" runat="server">
            <fieldset style="width: 98%">
                <legend><strong><span style="color: #000000;"></span></strong></legend>
                <br />
                <br />
                <table width="50%">
                    <tr>
                        <td class="fields" width="15%">Amount
                        </td>
                        <td class="fields">
                            <asp:TextBox ID="txtCreditAmount" runat="server" Width="126px"></asp:TextBox>
                            / Credit Hour
                        </td>
                    </tr>
                </table>
                <br />
                <br />
            </fieldset>
        </asp:View>
    </asp:MultiView>
    <table style="width: 100%">
        <tr>
            <td style="width: 71%; height: 149px; vertical-align: top;">
                <asp:DataGrid ID="dgView" runat="server" AutoGenerateColumns="False" Width="100%"
                    Height="123px" Style="vertical-align: text-top">
                    <FooterStyle CssClass="dgFooterStyle" Height="20px" />
                    <SelectedItemStyle CssClass="dgSelectedItemStyle" />
                    <AlternatingItemStyle BackColor="Beige" CssClass="dgAlternatingItemStyle" Font-Bold="False"
                        Font-Italic="False" Font-Overline="False" Font-Strikeout="False" Font-Underline="False" />
                    <ItemStyle CssClass="dgItemStyle" />
                    <HeaderStyle BackColor="#CDD7EE" CssClass="dgHeaderStyle" Font-Bold="True" Font-Italic="False"
                        Font-Overline="False" Font-Size="Medium" Font-Strikeout="False" Font-Underline="False" />
                    <Columns>
                        <asp:ButtonColumn CommandName="Select" DataTextField="FTCode" HeaderText="Fee Code"
                            Text="Feecode"></asp:ButtonColumn>
                        <asp:BoundColumn DataField="FeeDesc" HeaderText="Fee Desc"></asp:BoundColumn>
                        <asp:BoundColumn DataField="Type" HeaderText="Fee Type"></asp:BoundColumn>
                        <asp:BoundColumn DataField="Priority" HeaderText="Priority"></asp:BoundColumn>
                        <asp:BoundColumn HeaderText="Object id" Visible="False"></asp:BoundColumn>
                        <asp:BoundColumn DataField="FTCode" HeaderText="FTCode" Visible="False"></asp:BoundColumn>
                    </Columns>
                </asp:DataGrid>
                <asp:TextBox ID="txtFSCode" runat="server" Width="142px" Visible="False"></asp:TextBox>
            </td>
            <td style="width: 104%; height: 149px; vertical-align: top;">
                <asp:DataGrid ID="dgViewType" runat="server" AutoGenerateColumns="False" Width="98%"
                    Height="53px">
                    <FooterStyle CssClass="dgFooterStyle" Height="20px" />
                    <SelectedItemStyle CssClass="dgSelectedItemStyle" />
                    <AlternatingItemStyle BackColor="Beige" CssClass="dgAlternatingItemStyle" Font-Bold="False"
                        Font-Italic="False" Font-Overline="False" Font-Strikeout="False" Font-Underline="False" />
                    <ItemStyle CssClass="dgItemStyle" />
                    <HeaderStyle BackColor="#CDD7EE" CssClass="dgHeaderStyle" Font-Bold="True" Font-Italic="False"
                        Font-Overline="False" Font-Size="Medium" Font-Strikeout="False" Font-Underline="False" />
                    <Columns>
                        <asp:BoundColumn DataField="SCCode" HeaderText="Category Code" Visible="False"></asp:BoundColumn>
                        <asp:BoundColumn DataField="SCDesc" HeaderText="Category Description"></asp:BoundColumn>
                        <asp:TemplateColumn HeaderText="Fee Amount">
                            <ItemTemplate>
                                <asp:TextBox ID="txtFeeAmount" runat="server" Width="67px" OnTextChanged="txtFeeAmount_TextChanged"
                                    AutoPostBack="True" Style="text-align: right"></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateColumn>
                        <asp:BoundColumn DataField="FeeAmount" HeaderText="FeeAmount" Visible="False"></asp:BoundColumn>
                    </Columns>
                </asp:DataGrid>
            </td>
        </tr>
        <tr>
            <td class="fields">
                <br />
                <asp:Label ID="Label48" runat="server" Text="Student Count : " Font-Bold="True" Visible="False" ></asp:Label>
                <asp:Label ID="lblstucount" Font-Bold="True" runat="server" ></asp:Label>
                <br />
                <br />
                <asp:Label ID="Label49" runat="server" Style="font-weight: 700" Text="Total Batch Amount : RM " Visible="False" ></asp:Label>
                <asp:Label ID="lblbatch" runat="server" Style="font-weight: 700" Visible="False" ></asp:Label>
                <br />
                &nbsp;</td>
            <td>&nbsp;</td>
        </tr>
    </table>

    <asp:Button ID="btnHidden" runat="Server" OnClick="btnHidden_Click" Style="display: none" />

    <br />
    <asp:DataGrid ID="dgViewStudent" runat="server" AutoGenerateColumns="False"
        PageSize="1" Width="100%">
        <FooterStyle CssClass="dgFooterStyle" Height="20px" />
        <SelectedItemStyle CssClass="dgSelectedItemStyle" />
        <AlternatingItemStyle BackColor="Beige" CssClass="dgAlternatingItemStyle"
            Font-Bold="False" Font-Italic="False" Font-Overline="False"
            Font-Strikeout="False" Font-Underline="False" />
        <ItemStyle CssClass="dgItemStyle" />
        <HeaderStyle BackColor="#CDD7EE" CssClass="dgHeaderStyle" Font-Bold="True"
            Font-Italic="False" Font-Overline="False" Font-Size="Medium"
            Font-Strikeout="False" Font-Underline="False" />
        <Columns>
            <asp:BoundColumn DataField="MatricNo" HeaderText="Matric No">
                <HeaderStyle Width="12%" />
            </asp:BoundColumn>
            <asp:BoundColumn DataField="StudentName" HeaderText="Student Name"></asp:BoundColumn>
        </Columns>
    </asp:DataGrid>
</asp:Content>
