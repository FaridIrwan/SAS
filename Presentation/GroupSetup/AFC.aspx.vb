Imports System
Imports System.Data
Imports System.IO
Imports System.Collections.Generic

Partial Class AFC
    Inherits System.Web.UI.Page

    'Dim objBE As New BusinessEntities.DepartmentEn
    Dim objSQLQuery As New SQLPowerQueryManager.PowerQueryManager.DepartmentDL
    Dim GlobalSQLConnString As String = ConfigurationManager.ConnectionStrings("SASNEWConnectionString").ToString

    Dim DSReturn As New DataSet
    Dim strRetrunErrorMsg As String = String.Empty
    Dim blnReturnValue As Boolean
    Dim strMode As String


    Protected Sub AFC_Load(sender As Object, e As EventArgs) Handles Me.Load

    End Sub


End Class
