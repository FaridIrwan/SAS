<%@ Page Title="" Language="VB" MasterPageFile="~/MasterPage3.master" AutoEventWireup="false" MaintainScrollPositionOnPostback="true"
    CodeFile="DB_StudentAgeingByFaculty.aspx.vb" Inherits="Dashboard" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <br />
        <div>
        <center>
            <asp:Chart ID="ChartStudentAgeing" runat="server" Width="1000px" Height="600px">
                <Series>
                    <asp:Series Name="Series1" Legend="Legend1" ChartArea="MainChartArea"  IsValueShownAsLabel="true"
                        Palette="BrightPastel"  ChartType="StackedColumn" CustomProperties="DrawingStyle=LightToDark"
                        Font="Microsoft Sans Serif, 12pt, style=Bold" >
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
                <Legends>
                    <asp:Legend Name="Legend1">
                    </asp:Legend>
                </Legends>
                <Titles>
                    <asp:Title Font="Microsoft Sans Serif, 18pt" Name="Title1" Text="Student Ageing by Faculty">
                    </asp:Title>
                </Titles>
                <BorderSkin BackColor="SkyBlue" SkinStyle="FrameTitle4" BackGradientStyle="HorizontalCenter" />
            </asp:Chart>
        </center>
        
    </div>
</asp:Content>
