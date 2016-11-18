Imports System.Web.UI.DataVisualization.Charting
Imports System.Data
Partial Class Dashboard
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack() Then

            Dim dal As New HTS.SAS.DataAccessObjects.DashboardDAL()
            Dim dtSet As DataSet = dal.GetData(HTS.SAS.DataAccessObjects.DashboardDAL.DashboardType.Collection_By_Semester)
            Me.ChartCollectionAnalysis.DataSource = dtSet
            Me.ChartCollectionAnalysis.DataBind()

            If dtSet.Tables.Count > 0 Then
                ' Set series chart type
                Dim dv As System.Data.DataView = dtSet.Tables(0).AsDataView
                Dim intLoopIndex As Integer

                ' Collectin By Semester
                'dv = sdsCollection.Select(DataSourceSelectArguments.Empty)
                ChartCollectionAnalysis.Series.Clear()
                ChartCollectionAnalysis.DataBindCrossTable(dv.ToTable().Rows, "Year", "SASI_CurSem", "TransAmount", "Label=TransAmount") '"Label=SASI_Faculty"

                For intLoopIndex = 0 To (ChartCollectionAnalysis.Series.Count - 1)
                    ChartCollectionAnalysis.Series(intLoopIndex).IsValueShownAsLabel = True
                    ChartCollectionAnalysis.Series(intLoopIndex).CustomProperties = "DrawingStyle=LightToDark"
                Next intLoopIndex
            End If
        End If
    End Sub
End Class
