<%@ Page Language="VB" AutoEventWireup="false" CodeFile="Default2.aspx.vb" Inherits="Default2" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register Assembly="System.Web.DataVisualization, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI.DataVisualization.Charting" TagPrefix="asp" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Charting" TagPrefix="telerik" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:Chart ID="chtCategoriesProductCount" runat="server" Width="550px" Height="350px"
            DataSourceID="SqlDataSource1">
            <Series>
                <asp:Series Name="Categories" XValueMember="AgingId" YValueMembers="Amount" Palette="Chocolate"
                    IsValueShownAsLabel="true" ChartArea="MainChartArea">
                </asp:Series>
            </Series>
            <ChartAreas>
                <asp:ChartArea BackColor="NavajoWhite" BackGradientStyle="LeftRight" ShadowOffset="5"
                    Name="MainChartArea">
                      <AxisY Title="Amount" TextOrientation="Rotated90" 
                         TitleFont="Verdana, 14.25pt">
                    </AxisY>
                    <AxisX Title="Days" IsLabelAutoFit="True" 
                         TitleFont="Verdana, 14.25pt">
                          <LabelStyle Angle="-90" Interval="1" />
                    </AxisX>
                </asp:ChartArea>
            </ChartAreas>
             <Titles>
                <asp:Title Font="Microsoft Sans Serif, 18pt" Name="Title1" 
                    Text="Student Outstanding Invoices">
                </asp:Title>
            </Titles>
        </asp:Chart>
        <asp:Chart ID="Chart1" runat="server" Width="500" Height="400" DataSourceID="SqlDataSource1">
            <Series>
                <asp:Series Name="Series1" XValueMember="AgingId" YValueMembers="Amount" IsValueShownAsLabel="true"
                    ChartType="Column">
                </asp:Series>
            </Series>
            <ChartAreas>
                <asp:ChartArea BackColor="NavajoWhite" BackGradientStyle="LeftRight" Name="ChartArea1"
                    ShadowOffset="5">
                    <AxisY Title="Amount">
                    </AxisY>
                    <AxisX Title="Days" IsLabelAutoFit="True">
                        <LabelStyle Angle="-90" Interval="1" />
                    </AxisX>
                    <Area3DStyle Enable3D="True" />
                </asp:ChartArea>
            </ChartAreas>
            <Titles>
                <asp:Title Name="OutstandingInvoices" Text="Outstanding Invoices">
                </asp:Title>
            </Titles>
        </asp:Chart>
        <br />
        <br />
           <asp:Chart ID="Chart2" runat="server" Width="648px" Height="350px" DataSourceID="SqlDataSource2">
            <Series>
                <asp:Series Name="Series1" Legend="Legend1" ChartArea="MainChartArea1">
                </asp:Series>
            </Series>
            <ChartAreas>
                 <asp:ChartArea BackColor="NavajoWhite" BackGradientStyle="LeftRight" ShadowOffset="5"
                    Name="MainChartArea1">
                    <AxisY Title="Amount" TextOrientation="Rotated90" 
                         TitleFont="Verdana, 14.25pt">
                    </AxisY>
                    <AxisX Title="Days" IsLabelAutoFit="True" 
                         TitleFont="Verdana, 14.25pt">
                          <LabelStyle Angle="-90" Interval="1" />
                    </AxisX>
                   <%--   <Area3DStyle Enable3D="True" />--%>
                </asp:ChartArea>
            </ChartAreas>
            <Legends>
                <asp:Legend Name="Legend1">
                </asp:Legend>
            </Legends>
            <Titles>
                <asp:Title Font="Microsoft Sans Serif, 18pt" Name="Title1" 
                    Text="Student Ageing by Faculty">
                </asp:Title>
            </Titles>
        </asp:Chart>


     
        <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:SASNEWConnectionString %>"
            SelectCommand="SELECT  A.AgingId ,
        SUM(A.Amount) AS Amount
FROM    ( SELECT    '30 days' AS AgingId ,
                    1 AS OrderNo ,
                    CASE WHEN DATEDIFF(d, duedate, GETDATE()) &lt; 30
                         THEN ( dbo.SAS_Accounts.TransAmount
                                - dbo.SAS_Accounts.PaidAmount )
                         ELSE '0.00'
                    END AS 'Amount'
          FROM      dbo.SAS_Accounts
                    INNER JOIN dbo.SAS_Student ON ( dbo.SAS_Accounts.CreditRef = dbo.SAS_Student.SASI_MatricNo )
          WHERE     Transstatus = 'Open'
                    AND poststatus = 'Posted'
                    AND transtype = 'Credit'
                    AND subtype = 'Student'
          UNION	 ALL
          SELECT    '60 days' AS AgingId ,
                    2 AS OrderNo ,
                    CASE WHEN DATEDIFF(d, duedate, GETDATE()) &gt; 30
                              AND DATEDIFF(d, duedate, GETDATE()) &lt; 61
                         THEN ( dbo.SAS_Accounts.TransAmount
                                - dbo.SAS_Accounts.PaidAmount )
                         ELSE '0.00'
                    END AS 'Amount'
          FROM      dbo.SAS_Accounts
                    INNER JOIN dbo.SAS_Student ON ( dbo.SAS_Accounts.CreditRef = dbo.SAS_Student.SASI_MatricNo )
          WHERE     Transstatus = 'Open'
                    AND poststatus = 'Posted'
                    AND transtype = 'Credit'
                    AND subtype = 'Student'
          UNION ALL
          SELECT    '90 days' AS AgingId ,
                    3 AS OrderNo ,
                    CASE WHEN DATEDIFF(d, duedate, GETDATE()) &gt; 30
                              AND DATEDIFF(d, duedate, GETDATE()) &lt; 61
                         THEN ( dbo.SAS_Accounts.TransAmount
                                - dbo.SAS_Accounts.PaidAmount )
                         ELSE '0.00'
                    END AS 'Amount'
          FROM      dbo.SAS_Accounts
                    INNER JOIN dbo.SAS_Student ON ( dbo.SAS_Accounts.CreditRef = dbo.SAS_Student.SASI_MatricNo )
          WHERE     Transstatus = 'Open'
                    AND poststatus = 'Posted'
                    AND transtype = 'Credit'
                    AND subtype = 'Student'
          UNION ALL
          SELECT    '90 days above' AS AgingId ,
                    4 AS OrderNo ,
                    CASE WHEN DATEDIFF(d, duedate, GETDATE()) &gt; 90
                         THEN ( dbo.SAS_Accounts.TransAmount
                                - dbo.SAS_Accounts.PaidAmount )
                         ELSE '0.00'
                    END AS 'Amount'
          FROM      dbo.SAS_Accounts
                    INNER JOIN dbo.SAS_Student ON ( dbo.SAS_Accounts.CreditRef = dbo.SAS_Student.SASI_MatricNo )
          WHERE     Transstatus = 'Open'
                    AND poststatus = 'Posted'
                    AND transtype = 'Credit'
                    AND subtype = 'Student'
        ) AS A
GROUP BY A.AgingId "></asp:SqlDataSource>


        <asp:SqlDataSource ID="SqlDataSource2" runat="server" ConnectionString="<%$ ConnectionStrings:SASNEWConnectionString %>"
       SelectCommand="SELECT  A.AgingId ,
                        A.SASI_Faculty ,
                        SUM(A.Amount) AS Amount
            FROM    ( SELECT    '30 days' AS AgingId ,
                                dbo.SAS_Student.SASI_Faculty ,
                                1 AS OrderNo ,
                                CASE WHEN DATEDIFF(d, duedate, GETDATE()) &lt; 30
                                        THEN ( dbo.SAS_Accounts.TransAmount
                                            - dbo.SAS_Accounts.PaidAmount )
                                        ELSE '0.00'
                                END AS 'Amount'
                        FROM      dbo.SAS_Accounts
                                INNER JOIN dbo.SAS_Student ON ( dbo.SAS_Accounts.CreditRef = dbo.SAS_Student.SASI_MatricNo )
                        WHERE     Transstatus = 'Open'
                                AND poststatus = 'Posted'
                                AND transtype = 'Credit'
                                AND subtype = 'Student'
                        UNION ALL
                        SELECT    '30 days' AS AgingId ,
                                'ABC' AS SASI_Faculty ,
                                1 AS OrderNo ,
                                10 AS 'Amount'
                        UNION ALL
                        SELECT    '60 days' AS AgingId ,
                                dbo.SAS_Student.SASI_Faculty ,
                                2 AS OrderNo ,
                                CASE WHEN DATEDIFF(d, duedate, GETDATE()) &gt; 30
                                            AND DATEDIFF(d, duedate, GETDATE()) &lt; 61
                                        THEN ( dbo.SAS_Accounts.TransAmount
                                            - dbo.SAS_Accounts.PaidAmount )
                                        ELSE '0.00'
                                END AS 'Amount'
                        FROM      dbo.SAS_Accounts
                                INNER JOIN dbo.SAS_Student ON ( dbo.SAS_Accounts.CreditRef = dbo.SAS_Student.SASI_MatricNo )
                        WHERE     Transstatus = 'Open'
                                AND poststatus = 'Posted'
                                AND transtype = 'Credit'
                                AND subtype = 'Student'
                        UNION ALL
                        SELECT    '60 days' AS AgingId ,
                                'BAC' AS SASI_Faculty ,
                                1 AS OrderNo ,
                                2000 AS 'Amount'
                        UNION ALL
                        SELECT    '90 days' AS AgingId ,
                                dbo.SAS_Student.SASI_Faculty ,
                                3 AS OrderNo ,
                                CASE WHEN DATEDIFF(d, duedate, GETDATE()) &gt; 30
                                            AND DATEDIFF(d, duedate, GETDATE()) &lt; 61
                                        THEN ( dbo.SAS_Accounts.TransAmount
                                            - dbo.SAS_Accounts.PaidAmount )
                                        ELSE '0.00'
                                END AS 'Amount'
                        FROM      dbo.SAS_Accounts
                                INNER JOIN dbo.SAS_Student ON ( dbo.SAS_Accounts.CreditRef = dbo.SAS_Student.SASI_MatricNo )
                        WHERE     Transstatus = 'Open'
                                AND poststatus = 'Posted'
                                AND transtype = 'Credit'
                                AND subtype = 'Student'
                        UNION ALL
                        SELECT    '90 days above' AS AgingId ,
                                dbo.SAS_Student.SASI_Faculty ,
                                4 AS OrderNo ,
                                CASE WHEN DATEDIFF(d, duedate, GETDATE()) &gt; 90
                                        THEN ( dbo.SAS_Accounts.TransAmount
                                            - dbo.SAS_Accounts.PaidAmount )
                                        ELSE '0.00'
                                END AS 'Amount'
                        FROM      dbo.SAS_Accounts
                                INNER JOIN dbo.SAS_Student ON ( dbo.SAS_Accounts.CreditRef = dbo.SAS_Student.SASI_MatricNo )
                        WHERE     Transstatus = 'Open'
                                AND poststatus = 'Posted'
                                AND transtype = 'Credit'
                                AND subtype = 'Student'
                    ) A
            GROUP BY A.AgingId ,
                    A.SASI_Faculty
            ORDER BY A.AgingId ,
                    A.SASI_Faculty"></asp:SqlDataSource>
    </div>
    </form>
</body>
</html>
