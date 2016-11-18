Imports System.Web.UI.DataVisualization.Charting
Imports System.Data
Partial Class Dashboard
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack() Then

            Dim dal As New HTS.SAS.DataAccessObjects.DashboardDAL()
            Dim dtSet As DataSet = dal.GetData(HTS.SAS.DataAccessObjects.DashboardDAL.DashboardType.Sponsor_Ageing_Monthly)
            Me.ChartSponsor.DataSource = dtSet
            Me.ChartSponsor.DataBind()

            If dtSet.Tables.Count > 0 Then
                ' Set series chart type
                Dim dv As System.Data.DataView = dtSet.Tables(0).AsDataView

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
            End If

        End If
    End Sub
    
End Class
