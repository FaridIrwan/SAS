<%@ Page Title="" Language="VB" MasterPageFile="~/MasterPage3.master" AutoEventWireup="false" MaintainScrollPositionOnPostback="true"
    CodeFile="DB_NetInvoicesAgainstPayment.aspx.vb" Inherits="Dashboard" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
<br />
    <div>
    <center>
     <asp:Chart ID="ChartComparison" runat="server"  Width="1000px" Height="600px" >
        <Series>
            <asp:Series Name="NetInvoice" Legend="Legend1" ChartArea="MainChartArea2" XValueMember="SASI_CurSemYr"
                YValueMembers="Invoice" ChartType="Line" BorderWidth="5" IsValueShownAsLabel="true">
            </asp:Series>
            <asp:Series Name="Payment" Legend="Legend1" ChartArea="MainChartArea2" XValueMember="SASI_CurSemYr"
                YValueMembers="Payment" ChartType="Spline" BorderWidth="5" IsValueShownAsLabel="true">
            </asp:Series>
        </Series>
        <ChartAreas>
            <asp:ChartArea BackColor="Lavender" BackGradientStyle="LeftRight" ShadowOffset="5"
                Name="MainChartArea2">
                <AxisY Title="Amount" TextOrientation="Rotated90" TitleFont="Verdana, 14.25pt">
                </AxisY>
                <AxisX Title="Semester" IsLabelAutoFit="True" TitleFont="Verdana, 14.25pt" Minimum="0"
                    Maximum="8">
                    <LabelStyle Angle="-90" Interval="1" />
                </AxisX>
                    <Area3DStyle Enable3D="True" LightStyle="Realistic" PointDepth="30" PointGapDepth="30"
            WallWidth="5" />
            </asp:ChartArea>
        </ChartAreas>
        <Legends>
            <asp:Legend Name="Legend1" BorderWidth="10">
            </asp:Legend>
        </Legends>
        <Titles>
            <asp:Title Font="Microsoft Sans Serif, 18pt" Name="Title1" Text="Net Invoices vs Payment">
            </asp:Title>
        </Titles>
            <BorderSkin BackColor="SkyBlue" SkinStyle="FrameTitle4" BackGradientStyle="HorizontalCenter" />
    </asp:Chart>
    </center>
       
        
    </div>
</asp:Content>
