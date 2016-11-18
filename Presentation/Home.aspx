<%@ Page Language="VB" MasterPageFile="~/MasterPage3.master" MaintainScrollPositionOnPostback="true" AutoEventWireup="false" CodeFile="Home.aspx.vb" Inherits="Home" title="Welcome To SAS" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

    <script language="javascript" type="text/javascript">
// <!CDATA[

function IMG1_onMouseOver() {
IMG1.src="images/flowimages/index03.gif";
}

function IMG1_onMouseOut() {
IMG1.src="images/flowimages/index_21.gif";
}
// ]]>
</script>

	<table border="0" width="80%" cellspacing="0" cellpadding="0">
		<!--<tr>
			<td style="height: 15px">&nbsp;<asp:SiteMapPath ID="SiteMapPath1" runat="server">
                </asp:SiteMapPath>
                <asp:SiteMapDataSource ID="SiteMapDataSource1" runat="server" />
            </td>
		</tr>-->
        <br />
        <br />
        <br />
        <br />
		<tr align="center">
			<td style="height: 20px"><h2>Welcome</h2></td>
		</tr>
		<tr align="center">
		<td style="height: 20px">You are logged into the Student Accounting System</td>
		</tr>
		
		<tr>
			<td>
			<div align="center">
				<table border="0" width="458" cellspacing="10" cellpadding="0">
					<tr>
						<td>
<table id="Table_01" width="400" height="348" border="0" cellpadding="0" cellspacing="0">
	<tr>
		<td colspan="20">
			&nbsp;</td>
		<td>
			<img src="images/flowimages/spacer.gif" width="1" height="12" alt=""></td>
	</tr>
	<tr>
		<td rowspan="14">
			&nbsp;</td>
		<td colspan="3" rowspan="4">
            <asp:ImageButton ID="ibtnHome" runat="server" onmouseover="this.src='images/flowimages/main_03.gif';" onmouseout="this.src='images/flowimages/index_03.gif';" ImageUrl="~/images/flowimages/index_03.gif" /></td>
		<td colspan="22" style="height: 29px">
			&nbsp;</td>
		<td style="height: 29px">
			<img src="images/flowimages/spacer.gif" width="1" height="29" alt=""></td>
	</tr>
	<tr>
		<td colspan="19" style="height: 4px">
			<img src="images/flowimages/index_05.gif" width="331" height="4" alt=""></td>
		<td colspan="3" rowspan="10">
			&nbsp;</td>
		<td style="height: 4px">
			<img src="images/flowimages/spacer.gif" width="1" height="4" alt=""></td>
	</tr>
	<tr>
		<td colspan="2" rowspan="5">
			&nbsp;</td>
		<td style="height: 2px">
			<img src="images/flowimages/index_08.gif" width="1" height="2" alt=""></td>
		<td colspan="2" rowspan="7">
			<img src="images/flowimages/index_09.gif" width="8" height="63" alt=""></td>
		<td colspan="14" style="height: 2px">
			<img src="images/flowimages/index_10.gif" width="234" height="2" alt=""></td>
		<td style="height: 2px">
			<img src="images/flowimages/spacer.gif" width="1" height="2" alt=""></td>
	</tr>
	<tr>
		<td rowspan="7">
			<img src="images/flowimages/index_11.gif" width="1" height="62" alt=""></td>
		<td colspan="13" rowspan="5">
			&nbsp;</td>
		<td rowspan="8" style="width: 4px">
			<img src="images/flowimages/index_13.gif" width="3" height="112" alt=""></td>
		<td>
			<img src="images/flowimages/spacer.gif" width="1" height="38" alt=""></td>
	</tr>
	<tr>
		<td>
			<img src="images/flowimages/index_14.gif" width="33" height="1" alt=""></td>
		<td rowspan="5" style="width: 9px">
			<img src="images/flowimages/index_15.gif" width="8" height="23" alt=""></td>
		<td rowspan="5" style="width: 36px">
			<img src="images/flowimages/index_16.gif" width="35" height="23" alt=""></td>
		<td>
			<img src="images/flowimages/spacer.gif" width="1" height="1" alt=""></td>
	</tr>
	<tr>
		<td style="height: 5px">
			<img src="images/flowimages/index_17.gif" width="33" height="5" alt=""></td>
		<td style="height: 5px">
			<img src="images/flowimages/spacer.gif" width="1" height="5" alt=""></td>
	</tr>
	<tr>
		<td rowspan="3">
			<img src="images/flowimages/index_18.gif" width="33" height="17" alt=""></td>
		<td>
			<img src="images/flowimages/spacer.gif" width="1" height="13" alt=""></td>
	</tr>
	<tr>
		<td colspan="2" rowspan="3">
			<img src="images/flowimages/index_19.gif" width="88" height="5" alt=""></td>
		<td>
			<img src="images/flowimages/spacer.gif" width="1" height="1" alt=""></td>
	</tr>
	<tr>
		<td colspan="13">
			<img src="images/flowimages/index_20.gif" width="231" height="3" alt=""></td>
		<td>
			<img src="images/flowimages/spacer.gif" width="1" height="3" alt=""></td>
	</tr>
	<tr>
		<td colspan="3" rowspan="5">
            <asp:ImageButton ID="ibtnSetup" runat="server" onmouseover="this.src='images/flowimages/main_21.gif';" onmouseout="this.src='images/flowimages/index_21.gif';" ImageUrl="~/images/flowimages/index_21.gif" /></td>
		<td colspan="4">
			<img src="images/flowimages/index_22.gif" width="79" height="1" alt=""></td>
		<td colspan="6" rowspan="4">
            <asp:ImageButton ID="ibtnStudent" runat="server" onmouseover="this.src='images/flowimages/main_23.gif';" onmouseout="this.src='images/flowimages/index_23.gif';"  ImageUrl="~/images/flowimages/index_23.gif" /></td>
		<td colspan="5" rowspan="2">
			&nbsp;</td>
		<td>
			<img src="images/flowimages/spacer.gif" width="1" height="1" alt=""></td>
	</tr>
	<tr>
		<td rowspan="4">
			<img src="images/flowimages/index_25.gif" width="54" height="77" alt=""></td>
		<td colspan="5" rowspan="2">
            <asp:ImageButton ID="ibtnProcess" runat="server" onmouseover="this.src='images/flowimages/main_26.gif';" onmouseout="this.src='images/flowimages/index_26.gif';"  ImageUrl="~/images/flowimages/index_26.gif" /></td>
		<td rowspan="2">
			<img src="images/flowimages/index_27.gif" width="40" height="75" alt=""></td>
		<td>
			<img src="images/flowimages/spacer.gif" width="1" height="50" alt=""></td>
	</tr>
	<tr>
		<td colspan="3" rowspan="2">
			&nbsp;</td>
		<td colspan="5" rowspan="7">
            <asp:ImageButton ID="IbtnReports" onmouseover="this.src='images/flowimages/main_54.gif';" onmouseout="this.src='images/flowimages/index_54.gif';" runat="server" ImageUrl="~/images/flowimages/index_54.gif" /></td>
		<td rowspan="14">
			&nbsp;</td>
		<td>
			<img src="images/flowimages/spacer.gif" width="1" height="25" alt=""></td>
	</tr>
	<tr>
		<td colspan="4" rowspan="2">
			<img src="images/flowimages/index_31.gif" width="43" height="2" alt=""></td>
		<td colspan="2" rowspan="8">
			&nbsp;</td>
		<td>
			<img src="images/flowimages/spacer.gif" width="1" height="1" alt=""></td>
	</tr>
	<tr>
		<td colspan="2" rowspan="3">
			&nbsp;</td>
		<td rowspan="4" style="width: 4px">
			<img src="images/flowimages/index_34.gif" width="3" height="28" alt=""></td>
		<td rowspan="4" style="width: 6px">
			<img src="images/flowimages/index_35.gif" width="5" height="28" alt=""></td>
		<td rowspan="4" style="width: 10px">
			<img src="images/flowimages/index_36.gif" width="9" height="28" alt=""></td>
		<td colspan="3" rowspan="2">
			<img src="images/flowimages/index_37.gif" width="64" height="11" alt=""></td>
		<td rowspan="2">
			<img src="images/flowimages/index_38.gif" width="5" height="11" alt=""></td>
		<td>
			<img src="images/flowimages/spacer.gif" width="1" height="1" alt=""></td>
	</tr>
	<tr>
		<td colspan="7" rowspan="9">
			&nbsp;</td>
		<td colspan="2" rowspan="6">
			<img src="images/flowimages/index_40.gif" width="8" height="59" alt=""></td>
		<td>
			<img src="images/flowimages/spacer.gif" width="1" height="10" alt=""></td>
	</tr>
	<tr>
		<td colspan="4" style="height: 8px">
			<img src="images/flowimages/index_41.gif" width="69" height="8" alt=""></td>
		<td style="height: 8px">
			<img src="images/flowimages/spacer.gif" width="1" height="8" alt=""></td>
	</tr>
	<tr>
		<td colspan="2">
			<img src="images/flowimages/index_42.gif" width="37" height="9" alt=""></td>
		<td colspan="4">
			<img src="images/flowimages/index_43.gif" width="69" height="9" alt=""></td>
		<td>
			<img src="images/flowimages/spacer.gif" width="1" height="9" alt=""></td>
	</tr>
	<tr>
		<td rowspan="6">
			<img src="images/flowimages/index_44.gif" width="2" height="75" alt=""></td>
		<td colspan="6" rowspan="6">
            <asp:ImageButton ID="ibtnSpn" runat="server" onmouseover="this.src='images/flowimages/main_45.gif';" onmouseout="this.src='images/flowimages/index_45.gif';"  ImageUrl="~/images/flowimages/index_45.gif" /></td>
		<td colspan="2" rowspan="6">
			&nbsp;</td>
		<td>
			<img src="images/flowimages/spacer.gif" width="1" height="19" alt=""></td>
	</tr>
	<tr>
		<td colspan="5">
			<img src="images/flowimages/index_47.gif" width="78" height="2" alt=""></td>
		<td>
			<img src="images/flowimages/spacer.gif" width="1" height="2" alt=""></td>
	</tr>
	<tr>
		<td rowspan="3">
			&nbsp;</td>
		<td colspan="3" rowspan="3">
			&nbsp;</td>
		<td rowspan="3">
			&nbsp;</td>
		<td>
			<img src="images/flowimages/spacer.gif" width="1" height="11" alt=""></td>
	</tr>
	<tr>
		<td style="height: 7px">
			<img src="images/flowimages/index_51.gif" width="7" height="6" alt=""></td>
		<td colspan="3" style="height: 7px">
			<img src="images/flowimages/index_52.gif" width="72" height="6" alt=""></td>
		<td style="height: 7px">
			<img src="images/flowimages/spacer.gif" width="1" height="6" alt=""></td>
	</tr>
	<tr>
		<td colspan="4" rowspan="2">
			&nbsp;</td>
		<td>
			<img src="images/flowimages/spacer.gif" width="1" height="11" alt=""></td>
	</tr>
	<tr>
		<td colspan="5" rowspan="2">
            </td>
		<td>
			<img src="images/flowimages/spacer.gif" width="1" height="26" alt=""></td>
	</tr>
	<tr>
		<td colspan="20" rowspan="2">
			&nbsp;</td>
		<td>
			<img src="images/flowimages/spacer.gif" width="1" height="48" alt=""></td>
	</tr>
	<tr>
		<td colspan="5">
			&nbsp;</td>
		<td>
			<img src="images/flowimages/spacer.gif" width="1" height="11" alt=""></td>
	</tr>
	<tr>
		<td>
			<img src="images/flowimages/spacer.gif" width="20" height="1" alt=""></td>
		<td>
			<img src="images/flowimages/spacer.gif" width="33" height="1" alt=""></td>
		<td style="width: 9px">
			<img src="images/flowimages/spacer.gif" width="8" height="1" alt=""></td>
		<td style="width: 36px">
			<img src="images/flowimages/spacer.gif" width="35" height="1" alt=""></td>
		<td>
			<img src="images/flowimages/spacer.gif" width="54" height="1" alt=""></td>
		<td>
			<img src="images/flowimages/spacer.gif" width="34" height="1" alt=""></td>
		<td>
			<img src="images/flowimages/spacer.gif" width="1" height="1" alt=""></td>
		<td>
			<img src="images/flowimages/spacer.gif" width="7" height="1" alt=""></td>
		<td style="width: 2px">
			<img src="images/flowimages/spacer.gif" width="1" height="1" alt=""></td>
		<td>
			<img src="images/flowimages/spacer.gif" width="31" height="1" alt=""></td>
		<td>
			<img src="images/flowimages/spacer.gif" width="40" height="1" alt=""></td>
		<td>
			<img src="images/flowimages/spacer.gif" width="2" height="1" alt=""></td>
		<td style="width: 36px">
			<img src="images/flowimages/spacer.gif" width="35" height="1" alt=""></td>
		<td style="width: 4px">
			<img src="images/flowimages/spacer.gif" width="3" height="1" alt=""></td>
		<td style="width: 6px">
			<img src="images/flowimages/spacer.gif" width="5" height="1" alt=""></td>
		<td style="width: 10px">
			<img src="images/flowimages/spacer.gif" width="9" height="1" alt=""></td>
		<td>
			<img src="images/flowimages/spacer.gif" width="23" height="1" alt=""></td>
		<td>
			<img src="images/flowimages/spacer.gif" width="2" height="1" alt=""></td>
		<td style="width: 8px">
			<img src="images/flowimages/spacer.gif" width="39" height="1" alt=""></td>
		<td>
			<img src="images/flowimages/spacer.gif" width="5" height="1" alt=""></td>
		<td>
			<img src="images/flowimages/spacer.gif" width="34" height="1" alt=""></td>
		<td>
			<img src="images/flowimages/spacer.gif" width="3" height="1" alt=""></td>
		<td style="width: 4px">
			<img src="images/flowimages/spacer.gif" width="3" height="1" alt=""></td>
		<td>
			<img src="images/flowimages/spacer.gif" width="2" height="1" alt=""></td>
		<td>
			<img src="images/flowimages/spacer.gif" width="36" height="1" alt=""></td>
		<td>
			<img src="images/flowimages/spacer.gif" width="12" height="1" alt=""></td>
		<td></td>
	</tr>
</table>
						</td>
					</tr>
				</table>
			</div>
			</td>
		</tr>
		<tr>
			<td>&nbsp;</td>
		</tr>
	</table>

</asp:Content>

