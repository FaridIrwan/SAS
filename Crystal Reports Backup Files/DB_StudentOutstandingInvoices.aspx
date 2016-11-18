<%@ Page Title="" Language="VB" MasterPageFile="~/MasterPage3.master" AutoEventWireup="false" MaintainScrollPositionOnPostback="true"
    CodeFile="DB_StudentOutstandingInvoices.aspx.vb" Inherits="Dashboard" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div style="border: thin solid #A6D9F4; width: 99.9%">
        <br />
               <center>
            <asp:Chart ID="ChartOutstanding" Width="1000px" Height="600px" runat="server"
                BorderlineWidth="2" Palette="Pastel" BorderlineColor="ControlDark">
                <Series>
                    <asp:Series Name="Categories" XValueMember="AgingId" YValueMembers="Amount" IsValueShownAsLabel="true"
                        Palette="BrightPastel" ChartArea="MainChartArea" ChartType="StackedColumn" CustomProperties="DrawingStyle=LightToDark"
                        Font="Microsoft Sans Serif, 12pt, style=Bold">
                    </asp:Series>
                </Series>
                <ChartAreas>
                    <asp:ChartArea BackColor="Lavender" BackGradientStyle="LeftRight" ShadowOffset="5"
                        Name="MainChartArea" BorderWidth="2" IsSameFontSizeForAllAxes="True">
                        <AxisY Title="Amount" TextOrientation="Rotated90" TitleFont="Microsoft Sans Serif, 12pt"
                            IsLabelAutoFit="False">
                            <LabelStyle Font="Microsoft Sans Serif, 8.25pt, style=Bold" />
                        </AxisY>
                        <AxisX Title="Days" IsLabelAutoFit="True" TitleFont="Microsoft Sans Serif, 12pt">
                            <LabelStyle Angle="-90" Interval="Auto" Font="Microsoft Sans Serif, 8.25pt, style=Bold" />
                        </AxisX>
                        <Area3DStyle Enable3D="True" LightStyle="Realistic" PointDepth="30" PointGapDepth="30"
                            WallWidth="5" />
                    </asp:ChartArea>
                </ChartAreas>
                <Titles>
                    <asp:Title Font="Microsoft Sans Serif, 18pt" Name="Title1" Text="Student Outstanding Invoices">
                    </asp:Title>
                </Titles>
                <BorderSkin BackColor="SkyBlue" SkinStyle="FrameTitle4" BackGradientStyle="HorizontalCenter" />
            </asp:Chart>
        </center>

 <br />
        <br />
        <br />
         <br />
        <br />
        <br />
          <br />
        <br />
        <br />
    </div>
</asp:Content>
