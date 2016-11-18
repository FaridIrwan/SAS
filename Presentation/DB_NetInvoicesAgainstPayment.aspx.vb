Imports System.Web.UI.DataVisualization.Charting
Imports System.Data
Partial Class Dashboard
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Dim dal As New HTS.SAS.DataAccessObjects.DashboardDAL()
        Dim dtSet As DataSet = dal.GetData(HTS.SAS.DataAccessObjects.DashboardDAL.DashboardType.Net_Invoices_Against_Payment)
        Me.ChartComparison.DataSource = dtSet
        Me.ChartComparison.DataBind()

    End Sub
End Class
