﻿Imports System
Imports System.Data
Imports System.IO
Imports System.Collections.Generic

Partial Class Department
    Inherits System.Web.UI.Page

    Dim objBE As New BusinessEntities.DepartmentEn
    Dim objSQLQuery As New SQLPowerQueryManager.PowerQueryManager.DepartmentDL
    'Dim GlobalSQLConnString As String = ConfigurationManager.ConnectionStrings("SASNEWConnectionString").ToString
    Private DataBaseConnectionString As String = SQLPowerQueryManager.Helper.GetConnectionString()

    Dim DSReturn As New DataSet
    Dim strRetrunErrorMsg As String = String.Empty
    Dim blnReturnValue As Boolean
    Dim strMode As String

    Protected Sub Department_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Not IsPostBack() Then
            PageFunctional("Default")
            FillDataGrid()
        End If
        lblMsg.Text = ""
    End Sub

    Private Sub FillDataGrid()

        Try

            If Not IsNothing(DSReturn) Then DSReturn.Clear()

            blnReturnValue = objSQLQuery.DataGrid(objBE, strRetrunErrorMsg, DataBaseConnectionString, DSReturn)

            If blnReturnValue Then
                DataGridDataBinding(DSReturn, blnReturnValue)
                '
                If DSReturn.Tables(0).Rows.Count > 0 Then
                    lblDataGridMsg.Text = ""
                    lblDataGridMsg.Visible = False
                Else
                    lblDataGridMsg.Text = "Record Did Not Exist"
                    lblDataGridMsg.Visible = True
                End If
                '
            Else
                LogError.Log("Department", "FillDataGrid", strRetrunErrorMsg)
                lblMsg.Text = strRetrunErrorMsg
            End If
        Catch ex As Exception
            LogError.Log("Department", "FillDataGrid", ex.Message)
            lblMsg.Text = ex.Message
        End Try
        '
    End Sub

    Private Sub DataGridDataBinding(ByVal DSReturn As DataSet, ByVal blnValue As Boolean)
        '
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
            LogError.Log("Department", "DataGridDataBinding", ex.Message)
            lblMsg.Text = ex.Message
        End Try
        '
    End Sub

    Protected Sub dgDataGrid_PageIndexChanged(source As Object, e As DataGridPageChangedEventArgs) Handles dgDataGrid.PageIndexChanged
        dgDataGrid.CurrentPageIndex = e.NewPageIndex
        FillDataGrid()
    End Sub

    Private Sub MoveDBToTextBox()
        Try
            objBE.AutoID = hdnAutoID.Value
            objBE.SQLCase = 2
            If Not IsNothing(DSReturn) Then DSReturn.Clear()

            blnReturnValue = objSQLQuery.RetriveData(objBE, strRetrunErrorMsg, DataBaseConnectionString, DSReturn)
            If blnReturnValue Then
                If DSReturn.Tables(0).Rows.Count > 0 Then
                    With DSReturn.Tables(0).Rows(0)
                        If IsDBNull(.Item("DepartmentID")) Then
                            txtDepartmentID.Text = ""
                        Else
                            txtDepartmentID.Text = .Item("DepartmentID")
                            'Changed by Zoya @ 19/02/2016
                            txtDepartmentID.Enabled = False
                            'End changed 
                        End If

                        If IsDBNull(.Item("Department")) Then
                            txtDepartmentName.Text = ""
                        Else
                            txtDepartmentName.Text = .Item("Department")
                        End If

                        If IsDBNull(.Item("Department")) Then
                            rblStatus.SelectedIndex = -1
                        Else
                            If .Item("DepartmentID").Equals("master") Then
                                rblStatus.Enabled = False
                            Else
                                rblStatus.Enabled = True
                            End If

                            If .Item("Status") Then
                                rblStatus.SelectedValue = 1
                            Else
                                rblStatus.SelectedValue = 0
                            End If
                            
                        End If
                    End With
                Else
                    ClearData()
                    lblMsg.Text = "Record doesn't exits!"
                End If
            Else
                LogError.Log("Department", "MoveDBToTextBox", strRetrunErrorMsg)
                lblMsg.Text = strRetrunErrorMsg
            End If

            '
        Catch ex As Exception
            LogError.Log("Department", "MoveDBToTextBox", ex.Message)
            lblMsg.Text = ex.Message
        End Try
    End Sub

    Private Sub Insert(ByVal strMode As String)

        Try
            With objBE
                .DepartmentID = clsEmbeddedQuote(txtDepartmentID.Text)
                .Department = clsEmbeddedQuote(txtDepartmentName.Text)
                .Status = rblStatus.SelectedValue
                If strMode = "New" Then
                    .CreatedBy = Session("User")
                    .CreateDate = Format(Date.Now(), "dd-MM-yyyy")
                Else
                    .AutoID = hdnAutoID.Value
                    .ModifiedBy = Session("User")
                    .ModifiedDate = Format(Date.Now(), "dd-MM-yyyy")
                End If
                .SQLCase = 1
            End With

            If strMode = "New" Then
                blnReturnValue = objSQLQuery.RetriveData(objBE, strRetrunErrorMsg, DataBaseConnectionString, DSReturn)

                If blnReturnValue = True Then
                    blnReturnValue = objSQLQuery.InsertData(objBE, strRetrunErrorMsg, DataBaseConnectionString)

                    If blnReturnValue Then
                        PageFunctional("Default")
                        ClearData()
                        FillDataGrid()
                        lblMsg.Text = "Record Saved Successfully "
                    Else
                        LogError.Log("Department", "Insert", strRetrunErrorMsg)
                        lblMsg.Text = strRetrunErrorMsg
                    End If
                Else
                    lblMsg.Text = "Record already exist."
                    Exit Sub
                End If
            Else
                blnReturnValue = objSQLQuery.UpdateData(objBE, strRetrunErrorMsg, DataBaseConnectionString)

                If blnReturnValue Then
                    PageFunctional("Default")
                    ClearData()
                    FillDataGrid()
                    lblMsg.Text = "Record Updated Successfully "
                Else
                    LogError.Log("Department", "Update", strRetrunErrorMsg)
                    lblMsg.Text = strRetrunErrorMsg
                End If
            End If

        Catch ex As Exception
            LogError.Log("Department", "Insert", ex.Message)
            lblMsg.Text = ex.Message
        End Try

    End Sub

    Private Sub Delete(strDepartmentID As String)
        '
        Try
            objBE.DepartmentID = strDepartmentID
            objBE.SQLCase = 1

            blnReturnValue = objSQLQuery.DeleteData(objBE, strRetrunErrorMsg, DataBaseConnectionString)

            If blnReturnValue Then
                PageFunctional("Default")
                FillDataGrid()
                lblMsg.Text = "Record Deleted Successfully "
            Else
                LogError.Log("Department", "Delete", strRetrunErrorMsg)
                lblMsg.Text = strRetrunErrorMsg
            End If
        Catch ex As Exception
            LogError.Log("Department", "Delete", ex.Message)
            lblMsg.Text = ex.Message
        End Try
    End Sub

    Private Sub ClearData()
        txtDepartmentID.Text = ""
        txtDepartmentName.Text = ""
        txtSearch.Text = ""
        rblStatus.Enabled = True
        rblStatus.SelectedIndex = -1
        dgDataGrid.CurrentPageIndex = 0
        dgDataGrid.Controls.Clear()
        dgDataGrid.Visible = False
        txtDepartmentID.Enabled = True

    End Sub

    Protected Sub ibtnNew_Click(sender As Object, e As ImageClickEventArgs) Handles ibtnNew.Click
        PageFunctional("Edit")
        ClearData()
        ViewState("strMode") = "New"
    End Sub

    Protected Sub ibtnCancel_Click(sender As Object, e As ImageClickEventArgs) Handles ibtnCancel.Click
        PageFunctional("Default")
        ClearData()
        FillDataGrid()
    End Sub

    Protected Sub ibtnRefresh_Click(sender As Object, e As ImageClickEventArgs) Handles ibtnRefresh.Click
        ClearData()
        FillDataGrid()
    End Sub

    Protected Sub ibtnSearch_Click(sender As Object, e As ImageClickEventArgs) Handles ibtnSearch.Click

        If Not String.IsNullOrEmpty(txtSearch.Text) = True Then
            objBE.SearchCriteria = clsEmbeddedSpace(txtSearch.Text)
        Else
            objBE.SearchCriteria = ""
        End If

        FillDataGrid()
    End Sub

    Protected Sub ibtnOpen_Click(sender As Object, e As ImageClickEventArgs) Handles ibtnOpen.Click
        Dim atLeastOneSelected As Boolean = True
        Dim cb As CheckBox
        '
        For Each dgitem In dgDataGrid.Items
            cb = dgitem.Cells(0).Controls(1)
            If cb IsNot Nothing AndAlso cb.Checked Then
                atLeastOneSelected = False
                hdnAutoID.Value = dgitem.Cells(1).Text.Trim
            End If
        Next
        '
        If atLeastOneSelected = False Then
            PageFunctional("Edit")
            MoveDBToTextBox()
            ViewState("strMode") = "Edit"
        Else
            lblMsg.Text = "Please select atleast 1 Department."
        End If
    End Sub

    Protected Sub ibtnSave_Click(sender As Object, e As ImageClickEventArgs) Handles ibtnSave.Click
        strMode = ViewState("strMode")

        If txtDepartmentID.Text.Length = 0 Or txtDepartmentName.Text.Length = 0 Or rblStatus.SelectedValue = "-1" Then
            lblMsg.Text = "Enter All Required Fields "
            lblMsg.Visible = True
            Exit Sub
        End If

        If txtDepartmentID.Text = "" Then
            lblMsg.Text = "Enter valid Department ID."
            txtDepartmentID.Focus()
            Exit Sub
        End If

        If txtDepartmentName.Text = "" Then
            lblMsg.Text = "Enter valid Department Name."
            txtDepartmentName.Focus()
            Exit Sub
        End If
        If rblStatus.SelectedIndex = -1 Then
            lblMsg.Text = "Please Select Status."
            Exit Sub
        End If

        Insert(strMode)
    End Sub

    Protected Sub ibtnDelete_Click(sender As Object, e As ImageClickEventArgs) Handles ibtnDelete.Click
        Dim atLeastOneSelected As Boolean = True
        Dim strDepartmentID As String = ""
        Dim cb As CheckBox
        '
        For Each dgitem In dgDataGrid.Items
            cb = dgitem.Cells(0).Controls(1)
            If cb IsNot Nothing AndAlso cb.Checked Then
                atLeastOneSelected = False
                strDepartmentID = dgitem.Cells(2).Text.Trim
            End If
        Next
        '
        If atLeastOneSelected = False Then
            Delete(strDepartmentID)
        Else
            lblMsg.Text = "Please Select Atleast 1 Department."
        End If
    End Sub

#Region "Methods"

    ''' <summary>
    ''' Method Button Configurations
    ''' </summary>
    ''' <param name="strMode"></param>
    ''' <remarks></remarks>
    Private Sub PageFunctional(ByVal strMode As String)

        If strMode = "Default" Then
            ibtnNew.Visible = True
            lblNew.Visible = True
            ibtnOpen.Visible = True
            lblOpen.Visible = True
            ibtnDelete.Visible = True
            lblDelete.Visible = True
            ibtnSearch.Visible = True
            lblSearch.Visible = True
            ibtnRefresh.Visible = True
            lblRefresh.Visible = True
            ibtnSave.Visible = False
            lblSave.Visible = False
            ibtnCancel.Visible = False
            lblCancel.Visible = False

            'Panel
            pnlSearch.Visible = True
            pnlEdit.Visible = False

        ElseIf strMode = "Edit" Then
            'Buttons
            ibtnNew.Visible = False
            lblNew.Visible = False
            ibtnOpen.Visible = False
            lblOpen.Visible = False
            ibtnDelete.Visible = False
            lblDelete.Visible = False
            ibtnSearch.Visible = False
            lblSearch.Visible = False
            ibtnRefresh.Visible = False
            lblRefresh.Visible = False
            ibtnSave.Visible = True
            lblSave.Visible = True
            ibtnCancel.Visible = True
            lblCancel.Visible = True

            'Panel
            pnlSearch.Visible = False
            pnlEdit.Visible = True
        End If
    End Sub

    Private Function clsEmbeddedQuote(ByVal strText As String) As String
        clsEmbeddedQuote = Replace(strText, "'", "''")
        If String.IsNullOrEmpty(clsEmbeddedQuote) = True Then
            clsEmbeddedQuote = ""
        End If
        Return clsEmbeddedQuote
    End Function
    ' 
    Private Function clsEmbeddedSpace(ByVal strText As String) As String
        clsEmbeddedSpace = Replace(strText, "'", "")
        If String.IsNullOrEmpty(clsEmbeddedSpace) = True Then
            clsEmbeddedSpace = ""
        End If
        Return clsEmbeddedSpace
    End Function

#End Region

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load

    End Sub
End Class
