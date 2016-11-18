Imports System.Collections.Generic
Imports HTS.SAS.Entities
Imports HTS.SAS.BusinessObjects
Imports System.Data
Imports System.Data.SqlClient
Imports System.Globalization

Partial Class Integration
    Inherits System.Web.UI.Page

    Dim dsReturn As New DataSet
    Dim dsReturn_II As New DataSet
    Dim MJ_JnlLineEn As New BusinessEntities.MJ_JnlLine
    Dim MJ_JnlHdrEn As New BusinessEntities.MJ_JnlHdr
    Dim AutoNumberEn As New BusinessEntities.AutoNumberEn
    Dim ConfigEn As New BusinessEntities.UR_Config
    Dim AccountEn As New BusinessEntities.AccountsEn
    Dim objIntegrationDL As New SQLPowerQueryManager.PowerQueryManager.IntegrationDL
    Dim objIntegration As New IntegrationModule.IntegrationNameSpace.Integration

    Protected Sub Integration_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Not IsPostBack() Then
            LoadData()
            FillDataGrid()
        End If

    End Sub

    Private Sub FillDataGrid()

        Try
            dsReturn = objIntegrationDL.GetUnPostedData()

            DataGridDataBinding(dsReturn)

            If dsReturn.Tables(0).Rows.Count > 0 Then
                lblDataGridMsg.Text = ""
                lblDataGridMsg.Visible = False
            Else
                lblDataGridMsg.Text = "No Record Found..."
                lblDataGridMsg.Visible = True
            End If

        Catch ex As Exception
            LogError.Log("Integration", "FillDataGrid", ex.Message)
            lblMsg.Text = ex.Message
        End Try

    End Sub

    Private Sub DataGridDataBinding(ByVal DSReturn As DataSet)

        Try
            If DSReturn IsNot Nothing Then
                If DSReturn.Tables.Count <> 0 Then
                    If DSReturn.Tables(0).Rows.Count > 0 Then
                        dgDataGrid.DataSource = DSReturn
                        dgDataGrid.DataBind()
                        dgDataGrid.Visible = True
                    Else
                        dgDataGrid.Controls.Clear()
                        dgDataGrid.Visible = False
                    End If
                End If
            End If
        Catch ex As Exception
            LogError.Log("User", "DataGridDataBinding", ex.Message)
            lblMsg.Text = ex.Message
        End Try

    End Sub

    Private Sub LoadData()

        Try
            dsReturn = objIntegrationDL.GetIntegrationStatus()

            If dsReturn.Tables(0).Rows(0).Item("CON_Value1") = "1" Then
                rblInvoice.SelectedIndex = "0"
            Else
                rblInvoice.SelectedIndex = "1"
            End If

            If dsReturn.Tables(0).Rows(0).Item("CON_Value2") = "1" Then
                rblReciept.SelectedIndex = "0"
            Else
                rblReciept.SelectedIndex = "1"
            End If

            If dsReturn.Tables(0).Rows(0).Item("CON_Value3") = "1" Then
                rblPayment.SelectedIndex = "0"
            Else
                rblPayment.SelectedIndex = "1"
            End If

        Catch ex As Exception
            LogError.Log("Integration", "LoadData", ex.Message)
            lblMsg.Text = ex.Message
        End Try

    End Sub

    Private Sub OnSave()

        Try
            With ConfigEn
                If rblInvoice.SelectedIndex = "0" Then
                    .CON_Value1 = "1"
                Else
                    .CON_Value1 = "0"
                End If

                If rblReciept.SelectedIndex = "0" Then
                    .CON_Value2 = "1"
                Else
                    .CON_Value2 = "0"
                End If

                If rblPayment.SelectedIndex = "0" Then
                    .CON_Value3 = "1"
                Else
                    .CON_Value3 = "0"
                End If

                .CON_UpdatedDate = Format(Date.Now, "yyyy-MM-dd")
                .CON_UpdatedBy = Session("User")
            End With

            objIntegrationDL.UpdateIntegrationStatus(ConfigEn)

        Catch ex As Exception
            Throw ex
        End Try

    End Sub

    Protected Sub ibtnPost_Click(sender As Object, e As ImageClickEventArgs) Handles ibtnPost.Click
        Try
            Dim strBatchCode As String = String.Empty
            Dim strCategory As String = String.Empty

            dsReturn = objIntegrationDL.GetUnPostedData()

            For Each dr As DataRow In dsReturn.Tables(0).Rows

                strBatchCode = dr("BatchCode").ToString
                strCategory = dr("Category").ToString

                If strCategory = "Invoice" Or strCategory = "Debit Note" Or strCategory = "Credit Note" Then
                    objIntegration.InvoiceDebitCredit(strBatchCode, strCategory)
                ElseIf strCategory = "AFC" Then
                    objIntegration.InvoiceDebitCredit(strBatchCode, strCategory)
                ElseIf strCategory = "Receipt" Then
                    objIntegration.Receipt(strBatchCode)
                ElseIf strCategory = "Refund" Then
                    objIntegration.Payment(strBatchCode)
                End If
            Next

            FillDataGrid()

            lblMsg.Text = "Integration to CF was successful"

        Catch ex As Exception
            LogError.Log("Integration", "PostToCF", ex.Message)
            lblMsg.Text = ex.Message
        End Try

    End Sub

    Protected Sub ibtnSave_Click(sender As Object, e As ImageClickEventArgs) Handles ibtnSave.Click
        Try
            OnSave()

            lblMsg.Text = "Record sucessfully saved."
        Catch ex As Exception
            LogError.Log("Integration", "OnSave", ex.Message)
            lblMsg.Text = ex.Message
        End Try

    End Sub

    Protected Sub dgDataGrid_PageIndexChanged(source As Object, e As DataGridPageChangedEventArgs) Handles dgDataGrid.PageIndexChanged
        dgDataGrid.CurrentPageIndex = e.NewPageIndex
        FillDataGrid()
    End Sub
End Class

