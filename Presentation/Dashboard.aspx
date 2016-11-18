<%@ Page Title="" Language="VB" MasterPageFile="~/MasterPage3.master" AutoEventWireup="false"
    CodeFile="Dashboard.aspx.vb" Inherits="Dashboard" MaintainScrollPositionOnPostback="true" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div>
        <table width="100%" style="border: 1px solid #23B2D0" cellpadding="0px" cellspacing="0px">
            <tr>
                <td colspan="2" align="center" style="border: 1px solid #23B2D0">
                    <h1>
                        Dash Board
                    </h1>
                </td>
            </tr>
            <tr>
                <td width="50%" style="border: 1px solid #23B2D0">
                    <asp:Chart ID="ChartOutstanding" Width="600px" Height="400px" runat="server" DataSourceID="SqlDataSource10">
                        <Series>
                            <asp:Series Name="Categories" XValueMember="AgingId" YValueMembers="Amount" IsValueShownAsLabel="true"
                                Palette="Chocolate" ChartArea="MainChartArea">
                            </asp:Series>
                        </Series>
                        <ChartAreas>
                            <asp:ChartArea BackColor="NavajoWhite" BackGradientStyle="LeftRight" ShadowOffset="5"
                                Name="MainChartArea">
                                <AxisY Title="Amount" TextOrientation="Rotated90" TitleFont="Verdana, 14.25pt">
                                </AxisY>
                                <AxisX Title="Days" IsLabelAutoFit="True" TitleFont="Verdana, 14.25pt">
                                    <LabelStyle Angle="-90" Interval="1" />
                                </AxisX>
                            </asp:ChartArea>
                        </ChartAreas>
                        <Titles>
                            <asp:Title Font="Microsoft Sans Serif, 18pt" Name="Title1" Text="Student Outstanding Invoices">
                            </asp:Title>
                        </Titles>
                    </asp:Chart>
                </td>
                <td width="50%" style="border: 1px solid #23B2D0">
                    <asp:Chart ID="Chart2" runat="server" Width="600px" Height="400px" DataSourceID="SqlDataSource20">
                        <Series>
                            <asp:Series Name="Series1" Legend="Legend1" ChartArea="MainChartArea1">
                            </asp:Series>
                        </Series>
                        <ChartAreas>
                            <asp:ChartArea BackColor="NavajoWhite" BackGradientStyle="LeftRight" ShadowOffset="5"
                                Name="MainChartArea1">
                                <AxisY Title="Amount" TextOrientation="Rotated90" TitleFont="Verdana, 14.25pt">
                                </AxisY>
                                <AxisX Title="Days" IsLabelAutoFit="True" TitleFont="Verdana, 14.25pt">
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
                            <asp:Title Font="Microsoft Sans Serif, 18pt" Name="Title1" Text="Student Ageing by Faculty">
                            </asp:Title>
                        </Titles>
                    </asp:Chart>
                </td>
            </tr>
            <tr>
                <td width="50%" style="border: 1px solid #23B2D0">
                    <asp:Chart ID="ChartSponsor" runat="server" Width="600px" Height="400px" DataSourceID="SqlDataSource2">
                        <Series>
                        </Series>
                        <ChartAreas>
                            <asp:ChartArea BackColor="NavajoWhite" BackGradientStyle="LeftRight" ShadowOffset="5"
                                Name="MainChartArea1">
                                <AxisY Title="Amount" TextOrientation="Rotated90" TitleFont="Verdana, 14.25pt">
                                </AxisY>
                                <AxisX Title="Current Year" IsLabelAutoFit="True" TitleFont="Verdana, 14.25pt">
                                    <LabelStyle Angle="-90" Interval="1" />
                                </AxisX>
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
                    </asp:Chart>
                </td>
                <td width="50%" style="border: 1px solid #23B2D0">
                    <asp:Chart ID="ChartCollectionAnalysis" runat="server" Width="600px" Height="400px"
                        DataSourceID="SqlDataSource1">
                        <Series>
                            <asp:Series Name="Series1" Legend="Legend1" ChartArea="MainChartArea">
                            </asp:Series>
                        </Series>
                        <ChartAreas>
                            <asp:ChartArea BackColor="NavajoWhite" BackGradientStyle="LeftRight" ShadowOffset="5"
                                Name="MainChartArea">
                                <AxisY Title="Amount" TextOrientation="Rotated90" TitleFont="Verdana, 14.25pt">
                                </AxisY>
                                <AxisX Title="Semester" IsLabelAutoFit="True" TitleFont="Verdana, 14.25pt" Minimum="0"
                                    Maximum="8">
                                    <LabelStyle Angle="-90" Interval="1" />
                                </AxisX>
                            </asp:ChartArea>
                        </ChartAreas>
                        <Legends>
                            <asp:Legend Name="Legend1">
                            </asp:Legend>
                        </Legends>
                        <Titles>
                            <asp:Title Font="Microsoft Sans Serif, 18pt" Name="Title1" Text="Collection By Semester">
                            </asp:Title>
                        </Titles>
                    </asp:Chart>
                </td>
            </tr>
            <tr>
                <td width="50%" style="border: 1px solid #23B2D0">
                    <asp:Chart ID="ChartComparison" runat="server" Width="600px" Height="400px" DataSourceID="SqlDataSource3">
                        <Series>
                            <asp:Series Name="NetInvoice" Legend="Legend1" ChartArea="MainChartArea2" XValueMember="SASI_CurSem"
                                YValueMembers="Invoice" ChartType="Line" BorderWidth="5" IsValueShownAsLabel="true">
                            </asp:Series>
                            <asp:Series Name="Payment" Legend="Legend1" ChartArea="MainChartArea2" XValueMember="SASI_CurSem"
                                YValueMembers="Payment" ChartType="Spline" BorderWidth="5" IsValueShownAsLabel="true">
                            </asp:Series>
                        </Series>
                        <ChartAreas>
                            <asp:ChartArea BackColor="NavajoWhite" BackGradientStyle="LeftRight" ShadowOffset="5"
                                Name="MainChartArea2">
                                <AxisY Title="Amount" TextOrientation="Rotated90" TitleFont="Verdana, 14.25pt">
                                </AxisY>
                                <AxisX Title="Semester" IsLabelAutoFit="True" TitleFont="Verdana, 14.25pt" Minimum="0"
                                    Maximum="8">
                                    <LabelStyle Angle="-90" Interval="1" />
                                </AxisX>
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
                    </asp:Chart>
                </td>
                <td width="50%" style="border: 1px solid #23B2D0">
                </td>
            </tr>
        </table>
        <asp:SqlDataSource ID="SqlDataSource10" runat="server" ConnectionString="<%$ ConnectionStrings:SASNEWConnectionString %>"
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
                    CASE WHEN DATEDIFF(d, duedate, GETDATE()) &gt; 60
                              AND DATEDIFF(d, duedate, GETDATE()) &lt; 91
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
        <asp:SqlDataSource ID="SqlDataSource20" runat="server" ConnectionString="<%$ ConnectionStrings:SASNEWConnectionString %>"
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
                      
                        SELECT    '90 days' AS AgingId ,
                                dbo.SAS_Student.SASI_Faculty ,
                                3 AS OrderNo ,
                                CASE WHEN DATEDIFF(d, duedate, GETDATE()) &gt; 60
                                            AND DATEDIFF(d, duedate, GETDATE()) &lt; 91
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
        <asp:SqlDataSource ID="SqlDataSource2" runat="server" ConnectionString="<%$ ConnectionStrings:SASNEWConnectionString %>"
            SelectCommand="SELECT  SASR_Code AS SASR_Name ,
        SUM(Jan) AS Jan  ,
        SUM(Feb) AS Feb ,
        SUM(Mar) AS Mar ,
        SUM(Apr) AS Apr ,
        SUM(May) AS May ,
        SUM(Jun) AS Jun ,
        SUM(Jul) AS Jul ,
        SUM(Aug) AS Aug ,
        SUM(Sep) AS Sep ,
        SUM(Oct) AS Oct ,
        SUM(Nov) AS Nov ,
        SUM(Dec) AS Dec
FROM    ( SELECT    SS.SASR_Code ,
                    CASE WHEN MONTH(SA.transdate) = 1
                         THEN ( ISNULL(SA.TransAmount, 0)
                                - ISNULL(SA.PaidAmount, 0) )
                         ELSE 0
                    END AS 'Jan' ,
                    CASE WHEN MONTH(SA.transdate) = 2
                         THEN ( ISNULL(SA.TransAmount, 0)
                                - ISNULL(SA.PaidAmount, 0) )
                         ELSE 0
                    END AS 'Feb' ,
                    CASE WHEN MONTH(SA.transdate) = 3
                         THEN ( ISNULL(SA.TransAmount, 0)
                                - ISNULL(SA.PaidAmount, 0) )
                         ELSE 0
                    END AS 'Mar' ,
                    CASE WHEN MONTH(SA.transdate) = 4
                         THEN ( ISNULL(SA.TransAmount, 0)
                                - ISNULL(SA.PaidAmount, 0) )
                         ELSE 0
                    END AS 'Apr' ,
                    CASE WHEN MONTH(SA.transdate) = 5
                         THEN ( ISNULL(SA.TransAmount, 0)
                                - ISNULL(SA.PaidAmount, 0) )
                         ELSE 0
                    END AS 'May' ,
                    CASE WHEN MONTH(SA.transdate) = 6
                         THEN ( ISNULL(SA.TransAmount, 0)
                                - ISNULL(SA.PaidAmount, 0) )
                         ELSE 0
                    END AS 'Jun' ,
                    CASE WHEN MONTH(SA.transdate) = 7
                         THEN ( ISNULL(SA.TransAmount, 0)
                                - ISNULL(SA.PaidAmount, 0) )
                         ELSE 0
                    END AS 'Jul' ,
                    CASE WHEN MONTH(SA.transdate) = 8
                         THEN ( ISNULL(SA.TransAmount, 0)
                                - ISNULL(SA.PaidAmount, 0) )
                         ELSE 0
                    END AS 'Aug' ,
                    CASE WHEN MONTH(SA.transdate) = 9
                         THEN ( ISNULL(SA.TransAmount, 0)
                                - ISNULL(SA.PaidAmount, 0) )
                         ELSE 0
                    END AS 'Sep' ,
                    CASE WHEN MONTH(SA.transdate) = 10
                         THEN ( ISNULL(SA.TransAmount, 0)
                                - ISNULL(SA.PaidAmount, 0) )
                         ELSE 0
                    END AS 'Oct' ,
                    CASE WHEN MONTH(SA.transdate) = 11
                         THEN ( ISNULL(SA.TransAmount, 0)
                                - ISNULL(SA.PaidAmount, 0) )
                         ELSE 0
                    END AS 'Nov' ,
                    CASE WHEN MONTH(SA.transdate) = 12
                         THEN ( ISNULL(SA.TransAmount, 0)
                                - ISNULL(SA.PaidAmount, 0) )
                         ELSE 0
                    END AS 'Dec'
          FROM      dbo.SAS_Accounts SA
                    INNER JOIN dbo.SAS_Sponsor SS ON SA.CreditRef = SS.SASR_Code
          WHERE     SA.Transstatus = 'Open'
                    AND SA.poststatus = 'Posted'
                    AND SA.Transtype = 'Credit'
                    AND SA.subtype = 'Sponsor'
                    AND YEAR(SA.transdate) = YEAR(GETDATE())
        ) A
GROUP BY SASR_Code
ORDER BY SASR_Code
"></asp:SqlDataSource>
        <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:SASNEWConnectionString %>"
            SelectCommand=" SELECT A.SASI_CurSem ,
        A.Year ,
        SUM(A.TransAmount) AS TransAmount
 FROM   ( SELECT    SAS_Student.SASI_CurSem ,
                    YEAR(GETDATE()) AS Year ,
                    SUM(SAS_Accounts.TransAmount) AS TransAmount
          FROM      dbo.SAS_Program ,
                    SAS_Accounts
                    INNER JOIN SAS_Student ON SAS_Accounts.CreditRef = SAS_Student.SASI_MatricNo
                    INNER JOIN SAS_BankDetails ON SAS_Accounts.BankCode = SAS_BankDetails.SABD_Code
          WHERE     dbo.SAS_Program.SAPG_Code = dbo.SAS_Student.SASI_PgId
                    AND SAS_Accounts.TransType = 'Debit'
                    AND poststatus = 'Posted'
                    AND YEAR(SAS_Accounts.TransDate) = ( YEAR(GETDATE()) )
          GROUP BY  SAS_Student.SASI_CurSem
          UNION ALL
          SELECT    SAS_Student.SASI_CurSem ,
                    YEAR(GETDATE()) - 1 AS YEAR ,
                    SUM(SAS_Accounts.TransAmount) AS TransAmount
          FROM      dbo.SAS_Program ,
                    SAS_Accounts
                    INNER JOIN SAS_Student ON SAS_Accounts.CreditRef = SAS_Student.SASI_MatricNo
                    INNER JOIN SAS_BankDetails ON SAS_Accounts.BankCode = SAS_BankDetails.SABD_Code
          WHERE     dbo.SAS_Program.SAPG_Code = dbo.SAS_Student.SASI_PgId
                    AND SAS_Accounts.TransType = 'Debit'
                    AND poststatus = 'Posted'
                    AND YEAR(SAS_Accounts.TransDate) = ( YEAR(GETDATE()) - 1 )
          GROUP BY  SAS_Student.SASI_CurSem
        ) A
 GROUP BY A.SASI_CurSem ,
        A.Year
 ORDER BY A.Year ,
        A.SASI_CurSem"></asp:SqlDataSource>
        <asp:SqlDataSource ID="SqlDataSource3" runat="server" ConnectionString="<%$ ConnectionStrings:SASNEWConnectionString %>"
            SelectCommand="SELECT  SASI_CurSem ,
        SASI_CurSemYr ,
        SUM(Invoice) AS Invoice ,
        SUM(Payment) AS Payment
FROM    ( SELECT  DISTINCT
                    SS.SASI_MatricNo ,
                    SS.SASI_Name ,
                    SS.SASI_PgId ,
                    SS.SASI_CurSem ,
                    SS.SASI_CurSemYr ,
                    ISNULL(( SELECT SUM(TransAmount) AS Amount
                             FROM   SAS_Accounts
                             WHERE  CreditRef = SS.SASI_MatricNo
                                    AND PostStatus = 'Posted'
                                    AND TransType = 'Credit'
                           ), 0) AS Invoice ,
                    ISNULL(( SELECT SUM(TransAmount) AS Amount
                             FROM   SAS_Accounts
                             WHERE  CreditRef = SS.SASI_MatricNo
                                    AND PostStatus = 'Posted'
                                    AND TransType = 'Debit'
                           ), 0) AS Payment ,
                    NULL AS SASO_LoanAmount ,
                    0 AS SASO_IsReleased
          FROM      SAS_Accounts SA
                    INNER JOIN SAS_Student SS ON SA.CreditRef = SS.SASI_MatricNo
          WHERE    SA.PostStatus = 'Posted'
        ) A
GROUP BY SASI_CurSem ,
        SASI_CurSemYr
    ORDER BY SASI_CurSem"></asp:SqlDataSource>
    </div>
</asp:Content>
