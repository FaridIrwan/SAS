Imports System.Web.UI.DataVisualization.Charting
Imports System.Data
Partial Class Dashboard
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack() Then
            Dim dal As New HTS.SAS.DataAccessObjects.DashboardDAL()
            Dim dtSet As DataSet = dal.GetData(HTS.SAS.DataAccessObjects.DashboardDAL.DashboardType.Student_Ageing_By_Faculty)
            Me.ChartStudentAgeing.DataSource = dtSet
            Me.ChartStudentAgeing.DataBind()

            If dtSet.Tables.Count > 0 Then
                ' Set series chart type
                Dim dv As System.Data.DataView = dtSet.Tables(0).AsDataView

                Me.ChartStudentAgeing.Series.Clear()
                Me.ChartStudentAgeing.DataBindCrossTable(dv.ToTable().Rows, "SASI_Faculty", "AgingId", "Amount", "Label=Amount") '"Label=SASI_Faculty"   


                For intLoopIndex = 0 To (ChartStudentAgeing.Series.Count - 1)
                    ChartStudentAgeing.Series(intLoopIndex).CustomProperties = "DrawingStyle=LightToDark"
                Next intLoopIndex
            End If
        End If
    End Sub
End Class
