Imports System.Data
Imports System.Data.SqlClient
Imports System.Web.Configuration
Imports CrystalDecisions.CrystalReports.Engine

Partial Class RptTotalGLViewerEn
    Inherits System.Web.UI.Page
    Private MyReportDocument As New ReportDocument

#Region "Global Declarations "
    'Author			: Anil Kumar - T-Melmax Sdn Bhd
    'Created Date	: 20/05/2015

    Private _ReportHelper As New ReportHelper

#End Region

#Region "Page Load Starting  "

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        'Author			: Anil Kumar - T-Melmax Sdn Bhd
        'Created Date	: 20/05/2015

        Try

            If Not IsPostBack Then
                Dim sortbyvalue As String = Nothing
                Dim datecondition As String = Nothing
                Session("reportobject") = Nothing
                Session("reportobject1") = Nothing
                Dim datef As String = Request.QueryString("fdate")
                Dim datet As String = Request.QueryString("tdate")
                Dim datecond As String = Nothing
                Dim str As String = Nothing

                If Request.QueryString("fdate") = "0" Or Request.QueryString("tdate") = "0" Or Request.QueryString("fdate") Is Nothing Or Request.QueryString("tdate") Is Nothing Then

                    datecondition = ""

                Else

                    Dim datelen As Integer = Len(datef)
                    Dim d1, m1, y1, d2, m2, y2 As String
                    d1 = Mid(datef, 1, 2)
                    m1 = Mid(datef, 4, 2)
                    y1 = Mid(datef, 7, 4)
                    Dim datefrom As String = y1 + "/" + m1 + "/" + d1
                    d2 = Mid(datet, 1, 2)
                    m2 = Mid(datet, 4, 2)
                    y2 = Mid(datet, 7, 4)
                    Dim dateto As String = y2 + "/" + m2 + "/" + d2

                    datecondition = " where sa.transdate between '" + datefrom + "' and '" + dateto + "'"

                    datecond = " and transdate between '" + datefrom + "' and '" + dateto + "'"

                End If

                str = " select '" + datef + " - " + datet + "' dt,st.SAFT_Code,st.SAFT_Desc,st.SAFT_GLCode,"
                str += " isnull(totalAmount,0) transAmount,sg.SAFT_FeeType from SAS_FeeTypes st left join (SELECT SAFT_Code,SAFT_GLCode,"
                str += " SAFT_Desc,SAFT_FeeType,SAFT_Hostel,(isnull(Credit,0)-isnull(Debit,0)) totalAmount"
                str += " FROM ( select sf.SAFT_Code,"
                str += " sf.SAFT_GLCode, sf.SAFT_Desc, sf.SAFT_FeeType, sf.SAFT_Hostel, isnull(sum(sd.transamount), 0)"
                str += " transAmount,sa.TransType from   SAS_FeeTypes  sf left join SAS_AccountsDetails sd on sd.RefCode = sf.SAFT_Code "
                str += " left join SAS_Accounts sa on  sa.TransCode = sd.TransCode  " + datecondition + " group by sf.SAFT_Code,"
                str += " sf.SAFT_GLCode, sf.SAFT_Desc, sf.SAFT_FeeType, sf.SAFT_Hostel,sa.TransType) AS SourceTable"
                str += " PIVOT (SUM(transamount) FOR TransType IN ([Debit], [Credit])"
                str += " ) AS PivotTable) sg on sg.SAFT_Code = st.SAFT_Code"


                'Author			: Anil Kumar - T-Melmax Sdn Bhd
                'Created Date	: 20/05/2015

                'DataSet Strating
                Dim _DataSet As DataSet = _ReportHelper.GetDataSet(str)

                'Report XML Loading

                Dim s As String = Server.MapPath("~/xml/totalGL.xml")
                _DataSet.WriteXml(s)

                'Report XML Ended

                'Records Checking

                If _DataSet.Tables(0).Rows.Count = 0 Then
                    Response.Write("No Record Found")

                Else

                    'Report Loading
                    'Dim subRpt As New ReportDocument
                    'subRpt.Load(Server.MapPath("RptBayaranPelajar.rpt"))
                    'subRpt.SetDataSource(ds1)
                    'MyReportDocument.Subreports("subRpt").Load(Server.MapPath("RptBayaranPelajar.rpt"))
                    'MyReportDocument.Subreports("subRpt").SetDataSource(ds1)

                    MyReportDocument.Load(Server.MapPath("~/GroupReport/RptTotalGLEn.rpt"))
                    MyReportDocument.SetDataSource(_DataSet)
                    Session("reportobject") = MyReportDocument
                    CrystalReportViewer1.ReportSource = MyReportDocument
                    CrystalReportViewer1.DataBind()
                    MyReportDocument.Refresh()

                    'Report Ended

                End If

            Else

                'Report Loading

                MyReportDocument = Session("reportobject")
                CrystalReportViewer1.ReportSource = MyReportDocument
                CrystalReportViewer1.DataBind()
                MyReportDocument.Refresh()

                'Report Ended

            End If

        Catch ex As Exception

            Response.Write(ex.Message)

        End Try
    End Sub
#End Region

    Protected Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Unload
        'MyReportDocument.Close()
        'MyReportDocument.Dispose()
    End Sub
End Class
