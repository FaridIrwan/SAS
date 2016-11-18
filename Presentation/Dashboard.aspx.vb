Imports System.Web.UI.DataVisualization.Charting
Imports System.Data
Partial Class Dashboard
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack() Then
            ' Set series chart type
            Dim dv As System.Data.DataView
            dv = SqlDataSource20.Select(DataSourceSelectArguments.Empty)
            Me.Chart2.Series.Clear()
            Me.Chart2.DataBindCrossTable(dv.ToTable().Rows, "SASI_Faculty", "AgingId", "Amount", "Label=Amount") '"Label=SASI_Faculty"


            dv = SqlDataSource2.Select(DataSourceSelectArguments.Empty)
            Dim row As DataRow
            For Each row In dv.ToTable().Rows
                ' for each Row, add a new series
                Dim seriesName As String = row("SASR_Name").ToString()
                ChartSponsor.Series.Add(seriesName)
                ChartSponsor.Series(seriesName).ChartType = SeriesChartType.Column
                Dim colIndex As Integer
                For colIndex = 1 To (dv.ToTable().Columns.Count) - 1
                    ' for each column (column 1 and onward), add the value as a point
                    Dim columnName As String = dv.ToTable().Columns(colIndex).ColumnName
                    Dim YVal As Integer = CInt(row(columnName))
                    ChartSponsor.Series(seriesName).Points.AddXY(columnName, YVal)
                    ChartSponsor.Series(seriesName).IsValueShownAsLabel = True
                Next colIndex
            Next row

            ' Collectin By Semester
            dv = SqlDataSource1.Select(DataSourceSelectArguments.Empty)
            ChartCollectionAnalysis.Series.Clear()
            ChartCollectionAnalysis.DataBindCrossTable(dv.ToTable().Rows, "Year", "SASI_CurSem", "TransAmount", "Label=TransAmount") '"Label=SASI_Faculty"
        End If
    End Sub
End Class
