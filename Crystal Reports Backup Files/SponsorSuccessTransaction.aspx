<%@ Page Title="File Generate" MaintainScrollPositionOnPostback="true" Language="VB" MasterPageFile="~/MasterPage3.master" AutoEventWireup="false" CodeFile="SponsorSuccessTransaction.aspx.vb" Inherits="SponsorSuccessTransaction" %>

<%@ Register Assembly="AtlasControlToolkit" Namespace="AtlasControlToolkit" TagPrefix="atlasToolkit" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
<script language="javascript" type="text/javascript">
    function OpenWindow(URL) {
        var WindowName = "MyPopup";
        var Features = "location=no,toolbar=no,menubar=no,height=600,scrollbars=yes";
        window.open(URL, WindowName, Features);
    }
    </script>
    <%--  </ContentTemplate>     
</atlas:UpdatePanel>
<atlas:UpdateProgress ID="ProgressIndicator" runat="server">
    <ProgressTemplate>
        Loading the data, please wait... 
        <asp:Image ID="LoadingImage" ImageAlign="AbsMiddle" runat="server" ImageUrl="~/Images/spinner.gif" />        
    </ProgressTemplate>
 </atlas:UpdateProgress>
<atlas:UpdatePanel ID="up2" runat="server">
<ContentTemplate>--%><%--  </ContentTemplate>     
</atlas:UpdatePanel>--%>
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
                            <td style="height: 14px">
                                <asp:ImageButton ID="ibtnNew" runat="server" ImageUrl="~/images/add.png" ToolTip="New" />
                            </td>
                            <td style="width: 3%; height: 14px">
                                <asp:Label ID="Label11" runat="server" Text="New"></asp:Label>
                            </td>
                        </tr>
                    </table>
                </td>
                <td>
                    <table style="border-collapse: collapse;" class="menuoff" onmouseover="className='menuon';"
                        onmouseout="className='menuoff';">
                        <tr>
                            <td style="width: 3%; height: 14px">
                                <asp:ImageButton ID="ibtnSave" runat="server" ImageUrl="~/images/save.png" ToolTip="Save" />
                            </td>
                            <td style="width: 35px; height: 14px">
                                <asp:Label ID="Label14" runat="server" Text="Save"></asp:Label>
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
                                <asp:Label ID="Label13" runat="server" Text="Delete"></asp:Label>
                            </td>
                        </tr>
                    </table>
                </td>
                <td>
                    <table style="border-collapse: collapse;" class="menuoff" onmouseover="className='menuon';"
                        onmouseout="className='menuoff';">
                        <tr>
                            <td style="width: 3%; height: 14px">
                                <asp:ImageButton ID="ibtnView" runat="server" ImageUrl="~/images/find.png" ToolTip="View" />
                            </td>
                            <td style="width: 3%; height: 14px">
                                <asp:Label ID="Label16" runat="server" Text="Search"></asp:Label>
                            </td>
                        </tr>
                    </table>
                </td>
                <td>
                    <table style="border-collapse: collapse;" class="menuoff" onmouseover="className='menuon';"
                        onmouseout="className='menuoff';">
                        <tr>
                            <td style="width: 3%; height: 14px">
                                <asp:ImageButton ID="ibtnPrint" runat="server" ImageUrl="~/images/print.png" ToolTip="Print" />
                            </td>
                            <td style="width: 3%; height: 14px">
                                <asp:Label ID="Label17" runat="server" Text="Print"></asp:Label>
                            </td>
                        </tr>
                    </table>
                </td>
                <td>
                    <table style="border-collapse: collapse;" class="menuoff" onmouseover="className='menuon';"
                        onmouseout="className='menuoff';">
                        <tr>
                            <td style="width: 3%; height: 14px">
                                <asp:ImageButton ID="ibtnPosting" runat="server" ImageUrl="~/images/gposting.png"
                                    ToolTip="Cancel" />
                            </td>
                            <td style="width: 3%; height: 14px">
                                &nbsp;<asp:Label ID="Label6" runat="server" Text="Posting"></asp:Label>
                            </td>
                        </tr>
                    </table>
                </td>
                <td>
                    <table style="border-collapse: collapse;" class="menuoff" onmouseover="className='menuon';"
                        onmouseout="className='menuoff';">
                        <tr>
                            <td style="width: 3%; height: 14px">
                                <asp:ImageButton ID="ibtnOthers" runat="server" ImageUrl="~/images/gothers.png" ToolTip="Cancel" />
                            </td>
                            <td style="width: 3%; height: 14px">
                                <asp:Label ID="Label5" runat="server" Text="Others"></asp:Label>
                            </td>
                        </tr>
                    </table>
                </td>
                <td>
                    <table style="border-collapse: collapse;" class="menuoff" onmouseover="className='menuon';"
                        onmouseout="className='menuoff';">
                        <tr>
                            <td style="width: 3%; height: 14px">
                                <asp:ImageButton ID="ibtnCancel" runat="server" ImageUrl="~/images/cancel.png" ToolTip="Cancel" />
                            </td>
                            <td style="width: 3%; height: 14px">
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
                    <asp:TextBox ID="txtRecNo" runat="server" AutoPostBack="True" MaxLength="5" Style="text-align: right"
                        Width="52px" ReadOnly="true" CssClass="text_box" disabled="disabled" TabIndex="1"
                        dir="ltr"></asp:TextBox>
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
        <table style="width: 100%">
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
                    <asp:Label ID="lblMenuName" runat="server" Width="273px"></asp:Label>
                </td>
            </tr>
        </table>
        <asp:Label ID="lblMsg" runat="server" CssClass="lblError" Style="text-align: center"
            Width="359px"></asp:Label></asp:Panel>
    <%--  </ContentTemplate>     
</atlas:UpdatePanel>--%>
    <table style="width: 100%">
        <tr>
            <td style="width: 100%">
              <div  style="border: thin solid #A6D9F4; width: 100%">
                    <table width="100%">
                        <tr>
                            <td style="width: 13%">
                                    <asp:Label ID="Label12" runat="server" Text="File Upload" Width="97px"></asp:Label>
                            </td>
                            <td colspan="2">
                                    <asp:Button ID="btnupload" runat="server" 
                                        Text="Select student details template" Width="200px" />
                            </td>
                            <td style="width: 111px">
                                &nbsp;
                            </td>
                            <td style="width: 378px" rowspan="2">
                                &nbsp;</td>
                        </tr>
                        <tr>
                            <td style="width: 13%; height: 25px;">
                                &nbsp;</td>
                            <td style="height: 25px">
                                &nbsp;</td>
                            <td colspan="1" style="height: 25px; text-align: right; ">
                            </td>
                            <td style="width: 111px; height: 25px;">
                            </td>
                        </tr>
                       
                        <tr>
                            <td style="width: 13%; height: 25px;">
                                <asp:Label ID="lblUniKod" runat="server" Text="Universiti Code" Width="97px"></asp:Label>
                            </td>
                            <td style="height: 25px" colspan="2">
                                <asp:TextBox ID="txtKodUni" runat="server" MaxLength="20" Width="100px" 
                                    Style="text-align: right" ReadOnly="True"></asp:TextBox></td>
                            <td style="width: 111px; height: 25px;">
                                <asp:Label ID="lblKumpPelajar" runat="server" Text="Kumpulan Pelajar" Width="97px"></asp:Label>
                            </td>
                            <td style="width: 378px; height: 25px;">
                                <asp:TextBox ID="txtKumpPelajar" runat="server" MaxLength="20" Width="100px" 
                                    Style="text-align: right" ReadOnly="True"></asp:TextBox></td>
                        </tr>
                       
                        <tr>
                            <td style="width: 13%; height: 25px;">
                                <asp:Label ID="lblTarikhProses" runat="server" Text="Tarikh Proses" Width="97px"></asp:Label>
                            </td>
                            <td style="height: 25px" colspan="2">
                                <asp:TextBox ID="txtTarikhProcess" runat="server" MaxLength="20" Width="100px" 
                                    Style="text-align: right" ReadOnly="True"></asp:TextBox></td>
                            <td style="width: 111px; height: 25px;">
                                <asp:Label ID="lblKodBank" runat="server" Text="Kod Bank" Width="97px"></asp:Label>
                            </td>
                            <td style="width: 378px; height: 25px;">
                                <asp:TextBox ID="txtKodBank" runat="server" MaxLength="20" Width="100px" 
                                    Style="text-align: right" ReadOnly="True"></asp:TextBox></td>
                        </tr>
                       
                        <tr>
                            <td style="width: 13%; height: 25px;">
                                <asp:Label ID="lblNoLaporan" runat="server" Text="Nombor Laporan" 
                                    Width="97px"></asp:Label>
                            </td>
                            <td style="height: 25px">
                                <asp:TextBox ID="txtNoLaporan" runat="server" MaxLength="50" Width="150px" 
                                    Style="text-align: right" ReadOnly="True"></asp:TextBox></td>
                            <td colspan="1" style="height: 25px; text-align: right; ">
                                &nbsp;</td>
                            <td style="width: 111px; height: 25px;">
                                <asp:HiddenField ID="txtHeaderNo" runat="server" />
                            </td>
                            <td style="width: 378px; height: 25px;">
                                &nbsp;</td>
                        </tr>
                       
                        </table>
                    <br />
               </div>
               <table >
                        <tr runat="server" id="tabStudStatus">
                            <td style="width: 113px">
                                <!--<asp:Image ID="imgLeft1" runat="server" ImageUrl="images/b_orange_left.gif" ImageAlign="AbsBottom" />-->
                                <asp:Button ID="btnBatchInvoice" runat="server" CssClass="TabButton" Height="24px"
                                    OnClick="btnBatchInvoice_Click" Text="Active" Width="108px" /><!--<asp:Image ID="imgRight1" runat="server" ImageUrl="images/b_orange_right.gif" ImageAlign="AbsBottom" />-->
                            </td>
                            <td style="width: 139px">
                                <!--<asp:Image ID="imgLeft2" runat="server" ImageUrl="images/b_orange_left.gif" ImageAlign="AbsBottom" />-->
                                <asp:Button ID="btnSelection" runat="server" CssClass="TabButton" Height="25px" Text="InActive"
                                    Width="108px" OnClick="btnSelection_Click" /><!--<asp:Image ID="imgRight2" runat="server" ImageUrl="images/b_orange_right.gif" ImageAlign="AbsBottom" />-->
                            </td>
                        </tr>
                    </table>
               <div>
                <asp:MultiView ID="MultiView1" runat="server">
                        <asp:View ID="View1" runat="server">
                            <asp:CheckBox ID="chkSelectAll" runat="server" AutoPostBack="True" OnCheckedChanged="chkSelectAll_CheckedChanged"
                                Text="Confirm Students" Checked="true" />
                            <asp:DataGrid ID="dgView" runat="server" AutoGenerateColumns="False" DataKeyField="NoIC"
                                Width="100%" OnSelectedIndexChanged="dgView_SelectedIndexChanged" 
                                OnPageIndexChanged="dgView_PageIndexChanged" 
                                AllowSorting="True" Font-Bold="False" Font-Italic="False" Font-Overline="False" 
                                Font-Size="Larger" Font-Strikeout="False" Font-Underline="False" 
                                PageSize="15">
                                <FooterStyle CssClass="dgFooterStyle" Height="20px" Font-Bold="True" 
                                    Font-Italic="False" Font-Overline="False" Font-Size="Larger" 
                                    Font-Strikeout="False" Font-Underline="False" />
                                <PagerStyle Mode="NumericPages" />
                                <SelectedItemStyle CssClass="dgSelectedItemStyle" />
                                <AlternatingItemStyle BackColor="Beige" CssClass="dgAlternatingItemStyle" Font-Bold="False"
                                    Font-Italic="False" Font-Overline="False" Font-Strikeout="False" Font-Underline="False" />
                                <ItemStyle CssClass="dgItemStyle" />
                                <HeaderStyle BackColor="#CDD7EE" CssClass="dgHeaderStyle" Font-Bold="True" Font-Italic="False"
                                    Font-Overline="False" Font-Size="Medium" Font-Strikeout="False" Font-Underline="False" />
                                <Columns>
                                    <asp:TemplateColumn HeaderText="Select">
                                        <ItemTemplate>
                                            &nbsp;<asp:CheckBox ID="Chk" runat="server" Checked="true" AutoPostBack="True" OnCheckedChanged="Chk_CheckedChanged" />
                                        </ItemTemplate>
                                        <HeaderStyle Width="5%" />
                                    </asp:TemplateColumn>
                                    <asp:BoundColumn DataField="noPelajar" HeaderText="No Pelajar">
                                        <HeaderStyle Width="8%" />
                                    </asp:BoundColumn>
                                    <asp:BoundColumn DataField="NamaPelajar" HeaderText="NamaPelajar">
                                        <HeaderStyle />
                                    </asp:BoundColumn>
                                    <asp:BoundColumn DataField="NoIC" HeaderText="IC No">
                                        <HeaderStyle Width="5%" />
                                    </asp:BoundColumn>
                                    <asp:BoundColumn DataField="noAkaun" HeaderText="NoAkaun">
                                        <HeaderStyle Width="10%" />
                                    </asp:BoundColumn>
                                    <asp:BoundColumn DataField="statusBayaran"  HeaderText="Status Bayaran">
                                    <HeaderStyle Width="3%" />
                                    </asp:BoundColumn>
                                    <asp:BoundColumn DataField="AmaunWarran" HeaderText="Jumlah Warran">
                                        <HeaderStyle Width="10%" />
                                    </asp:BoundColumn>
                                    <asp:BoundColumn DataField="AmaunPotongan" HeaderText="AmaunPotongan">
                                        <HeaderStyle Width="10%" />
                                    </asp:BoundColumn>
                                    <asp:BoundColumn DataField="NilaiBersih" HeaderText="Nilai Bersih">
                                        <HeaderStyle Width="10%" />
                                    </asp:BoundColumn>
                                </Columns>
                            </asp:DataGrid></asp:View>
                        <asp:View ID="View2" runat="server">
                            <asp:DataGrid ID="dgUnView" runat="server" AutoGenerateColumns="False" DataKeyField="MatricNo"
                                Width="100%" PageSize="1">
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
                                            <asp:CheckBox ID="chk" runat="server" />
                                        </ItemTemplate>
                                        <HeaderStyle Width="5%" />
                                    </asp:TemplateColumn>
                                    <asp:BoundColumn DataField="MatricNO" HeaderText="Student MatricNo">
                                        <HeaderStyle Width="12%" />
                                    </asp:BoundColumn>
                                    <asp:BoundColumn DataField="StudentName" HeaderText="StudentName"></asp:BoundColumn>
                                    <asp:BoundColumn DataField="ProgramID" HeaderText="Program">
                                        <HeaderStyle Width="10%" />
                                    </asp:BoundColumn>
                                    <asp:BoundColumn HeaderText="IC No" DataField="ICNo">
                                        <HeaderStyle Width="10%" />
                                    </asp:BoundColumn>
                                    <asp:BoundColumn HeaderText="Semester" DataField="CurrentSemester">
                                        <HeaderStyle Width="10%" />
                                    </asp:BoundColumn>
                                    <asp:BoundColumn HeaderText="OutStanding Amount" DataField="TransactionAmount">
                                        <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                            Font-Underline="False" HorizontalAlign="Right" />
                                        <HeaderStyle Font-Bold="True" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                            Font-Underline="False" VerticalAlign="Middle" Width="10%" />
                                    </asp:BoundColumn>
                                    <asp:TemplateColumn HeaderText="Allocated Amount">
                                        <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                            Font-Underline="False" HorizontalAlign="Right" />
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtAllAmount1" runat="server" AutoPostBack="True" OnTextChanged="txtAllAmount1_TextChanged"
                                                Style="text-align: right" Width="97px"></asp:TextBox>
                                        </ItemTemplate>
                                        <HeaderStyle Font-Bold="True" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                            Font-Underline="False" VerticalAlign="Middle" Width="10%" />
                                    </asp:TemplateColumn>
                                    <asp:BoundColumn HeaderText="Balance Amount" Visible="False">
                                        <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                            Font-Underline="False" HorizontalAlign="Right" />
                                        <HeaderStyle Font-Bold="True" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                            Font-Underline="False" VerticalAlign="Middle" Width="10%" />
                                    </asp:BoundColumn>
                                    <asp:TemplateColumn HeaderText="Pocket Amount">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtpamont" runat="server" OnTextChanged="txtpamont_TextChanged"
                                                Style="text-align: right" Width="86px"></asp:TextBox>
                                        </ItemTemplate>
                                    </asp:TemplateColumn>
                                    <asp:BoundColumn DataField="TransactionAmount" HeaderText="Amt" Visible="False">
                                    </asp:BoundColumn>
                                </Columns>
                            </asp:DataGrid>
                            <br />
                        </asp:View>
                    </asp:MultiView>
               </div>
               <asp:Button ID="btnHidden" runat="Server" OnClick="btnHidden_Click"  style="display:none" />
            </td>
        </tr>
    </table>
    <%--  </ContentTemplate>     
</atlas:UpdatePanel>--%>
</asp:Content>

