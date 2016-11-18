Imports System.Web.UI.DataVisualization.Charting
Imports System.Data
Imports HTS.SAS.Entities
Imports HTS.SAS.DataAccessObjects

Partial Class Dashboard
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Dim dal As New HTS.SAS.DataAccessObjects.DashboardDAL()
        Dim dtSet As DataSet = dal.GetData(DashboardDAL.DashboardType.Student_Outstanding_Invoice)
        Me.ChartOutstanding.DataSource = dtSet
        Me.ChartOutstanding.DataBind()

    End Sub
End Class
