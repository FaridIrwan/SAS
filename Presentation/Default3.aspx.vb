Imports System.Web.UI.DataVisualization.Charting

Partial Class Default3
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack() Then
            ' Set series chart type
            Dim dv As System.Data.DataView
            dv = SqlDataSource1.Select(DataSourceSelectArguments.Empty)
            Me.chtCategoriesProductCount.Series.Clear()
            Me.chtCategoriesProductCount.DataBindCrossTable(dv.ToTable().Rows, "SASI_Faculty", "AgingId", "Amount", "Label=Amount") '"Label=SASI_Faculty"

        End If
    End Sub
End Class
