<%@ Page Title="" Language="VB" MasterPageFile="~/MasterPage3.master" AutoEventWireup="false" MaintainScrollPositionOnPostback="true"
    CodeFile="DB_SponsorAgeingMonthly.aspx.vb" Inherits="Dashboard" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <br />
        <div>
        <center>
            <asp:Chart ID="ChartSponsor" runat="server" Width="1000px" Height="600px">
                <Series>
                </Series>
                <ChartAreas>
                     <asp:ChartArea BackColor="Lavender" BackGradientStyle="LeftRight" ShadowOffset="5" 
                        Name="MainChartArea" BorderWidth="2" IsSameFontSizeForAllAxes="True">
                        <AxisY Title="Amount" TextOrientation="Rotated90" TitleFont="Microsoft Sans Serif, 12pt"
                            IsLabelAutoFit="False">
                            <LabelStyle Font="Microsoft Sans Serif, 8.25pt, style=Bold" />
                        </AxisY>
                        <AxisX Title="Current Year" IsLabelAutoFit="True" TitleFont="Microsoft Sans Serif, 12pt" Minimum="0" Interval="1"
                            Maximum="13">
                            <LabelStyle Angle="-90" Interval="1" Font="Microsoft Sans Serif, 8.25pt, style=Bold" />
                        </AxisX>
                        <Area3DStyle Enable3D="True" LightStyle="Realistic" PointDepth="30" PointGapDepth="30"
                            WallWidth="5" />
                    </asp:ChartArea>

                </ChartAreas>
                <Legends>
                    <asp:Legend Name="Legend1">
                    </asp:Legend>
                </Legends>
                <Titles>
                    <asp:Title Font="Microsoft Sans Serif, 18pt" Name="Title1" Text="Monthly Sponsor Ageing">
                    </asp:Title>                    
                </Titles>
                 <BorderSkin BackColor="SkyBlue" SkinStyle="FrameTitle4" BackGradientStyle="HorizontalCenter" />                
            </asp:Chart>
        </center>        
    </div>
</asp:Content>
