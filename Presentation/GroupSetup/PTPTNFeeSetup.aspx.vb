Imports HTS.SAS.Entities
Imports HTS.SAS.BusinessObjects
Imports System.Data
Imports System.Collections.Generic

Partial Class PTPTNFeeSetup
    Inherits System.Web.UI.Page
    '
    Dim objBE As New BusinessEntities.PTPTNFeeSetupEn
    Dim objSQLQuery As New SQLPowerQueryManager.PowerQueryManager.PTPTNFessSetupDL
    Dim GlobalSQLConnString As String = ConfigurationManager.ConnectionStrings("SASNEWConnectionString").ToString
    Dim DSReturn As New DataSet
    Dim strRetrunErrorMsg As String = String.Empty
    Dim blnReturnValue As Boolean
    Dim strMode As String

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Not IsPostBack() Then

            txtProgFee.Attributes.Add("onKeypress", "isNumberKey(event)")
            ibtnSave.Attributes.Add("onclick", "return validate()")
            ibtnSearch.Attributes.Add("onclick", "return validateProgramName()")

            PageFunctional("Default")
            Menuname(CInt(Request.QueryString("Menuid")))
            FillDataGrid()
            LoadProgramDropDown()
            LoadProgramEditDropDown()
        End If
        lblMsg.Text = ""
    End Sub

#Region "Methods"
    ''' <summary>
    ''' Method to get the MenuName
    ''' </summary>
    ''' <param name="MenuId">Parameter is MenuId</param>
    ''' <remarks></remarks>
    Private Sub Menuname(ByVal MenuId As Integer)
        Dim eobj As New MenuEn
        Dim bobj As New MenuBAL
        eobj.MenuId = MenuId
        eobj = bobj.GetMenus(eobj)
        lblMenuName.Text = eobj.MenuName
    End Sub

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
            ibtnSave.Visible = False
            lblSave.Visible = False
            ibtnCancel.Visible = False
            lblCancel.Visible = False
            lblDataGridMsg.Visible = False

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
            ibtnSave.Visible = True
            lblSave.Visible = True
            ibtnCancel.Visible = True
            lblCancel.Visible = True

            'Panel
            pnlSearch.Visible = False
            pnlEdit.Visible = True
        End If
    End Sub

#End Region

    Private Sub FillDropDown()

    End Sub

    ''' <summary>
    ''' Fill Program to ddlProgram DropdownList
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub LoadProgramDropDown()
        Dim eProgram As New ProgramAccountEn
        Dim bProgram As New ProgramAccountBAL
        Dim listProgram As New List(Of ProgramAccountEn)
        ddlProgram.Items.Clear()
        ddlProgram.DataTextField = "CodeProgram"
        ddlProgram.DataValueField = "ProgramCode"
        eProgram.ProgramCode = "%"
        '
        Try
            listProgram = bProgram.GetListProgram(eProgram)
        Catch ex As Exception
            LogError.Log("PTPTN Fee Setup", "LoadProgramDropDown", ex.Message)
        End Try
        ddlProgram.DataSource = listProgram
        ddlProgram.DataBind()
        ddlProgram.Items.Insert(0, New ListItem("-Please Select-", "0"))
    End Sub

    ''' <summary>
    ''' Fill Program to ddlProgramEdit DropdownList
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub LoadProgramEditDropDown()
        Dim eProgram As New ProgramAccountEn
        Dim bProgram As New ProgramAccountBAL
        Dim listProgram As New List(Of ProgramAccountEn)
        ddlProgramEdit.Items.Clear()
        ddlProgramEdit.DataTextField = "CodeProgram"
        ddlProgramEdit.DataValueField = "ProgramCode"
        eProgram.ProgramCode = "%"
        '
        Try
            listProgram = bProgram.GetListProgram(eProgram)
        Catch ex As Exception
            LogError.Log("PTPTN Fee Setup", "LoadProgramEditDropDown", ex.Message)
        End Try
        ddlProgramEdit.DataSource = listProgram
        ddlProgramEdit.DataBind()
        ddlProgramEdit.Items.Insert(0, New ListItem("-Please Select-", "0"))
    End Sub

    ''' <summary>
    ''' Fill Data to dgPTPTN DataGrid
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub FillDataGrid()
        '
        Try
            If ddlProgram.SelectedIndex = 0 Then
                objBE.ProgCode = String.Empty
            Else
                objBE.ProgCode = ddlProgram.SelectedValue
            End If
            '
            If Not IsNothing(DSReturn) Then DSReturn.Clear()
            '
            blnReturnValue = objSQLQuery.DataGrid(objBE, strRetrunErrorMsg, GlobalSQLConnString, DSReturn)
            '
            If blnReturnValue Then
                DataGridDataBinding(DSReturn, blnReturnValue)
                '
                If DSReturn.Tables(0).Rows.Count > 0 Then
                    lblDataGridMsg.Text = ""
                    lblDataGridMsg.Visible = False
                Else
                    lblDataGridMsg.Text = "Record did not Exist"
                    lblDataGridMsg.Visible = True
                End If
                '
            Else
                LogError.Log("PTPTN Fee Setup", "FillDataGrid", strRetrunErrorMsg)
                lblMsg.Text = strRetrunErrorMsg
            End If
        Catch ex As Exception
            LogError.Log("PTPTN Fee Setup", "FillDataGrid", ex.Message)
            lblMsg.Text = ex.Message
        End Try
        '
    End Sub

    ''' <summary>
    ''' DataGrid Data Binding
    ''' </summary>
    ''' <param name="DSReturn"></param>
    ''' <remarks></remarks>
    Private Sub DataGridDataBinding(ByVal DSReturn As DataSet, ByVal blnValue As Boolean)
        '
        Try
            If DSReturn IsNot Nothing Then
                If DSReturn.Tables.Count <> 0 Then
                    If DSReturn.Tables(0).Rows.Count > 0 Then
                        dgPTPTN.DataSource = DSReturn
                        dgPTPTN.DataBind()
                        dgPTPTN.Visible = True
                    Else
                        dgPTPTN.Controls.Clear()
                        dgPTPTN.Visible = False
                    End If
                End If
            End If
        Catch ex As Exception
            LogError.Log("PTPTN Fee Setup", "DataGridDataBinding", ex.Message)
            lblMsg.Text = ex.Message
        End Try
        '
    End Sub

    'Protected Sub cbPTPTN_CheckedChanged(sender As Object, e As EventArgs)
    '    Dim strKeyId_Parent As String = String.Empty
    '    Dim strKeyId_Child As String = String.Empty
    '    '
    '    For Each dgitem In dgPTPTN.Items
    '        Dim cb As CheckBox = dgitem.Cells(0).Controls(1)
    '        If cb IsNot Nothing AndAlso cb.Checked Then
    '            strKeyId_Parent = dgitem.Cells(1).Text.Trim
    '        End If
    '    Next
    '    GetCurrentCheckBoxChecked(strKeyId_Parent)
    '    '        
    'End Sub

    'Private Sub GetCurrentCheckBoxChecked(strKeyId_Parent As String)
    '    Dim intSelIndex = 0
    '    Dim intRowCount = 0
    '    '
    '    For Each dgItem As DataGridItem In dgPTPTN.Items
    '        Dim cb As CheckBox = dgItem.Cells(0).Controls(1)
    '        If strKeyId_Parent = CType(dgItem.Cells(1).Text, String) Then
    '            cb.Checked = True
    '            intSelIndex = intRowCount
    '        Else
    '            cb.Checked = False
    '        End If
    '        intRowCount += 1
    '    Next
    'End Sub

    ''' <summary>
    ''' Retrive Data From DB To Textbox
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub MoveDataEdit()
        Try
            objBE.AutoID = hdnAutoID.Value
            objBE.ProgCode = ddlProgramEdit.SelectedValue
            If Not IsNothing(DSReturn) Then DSReturn.Clear()

            blnReturnValue = objSQLQuery.RetriveSearchData(objBE, strRetrunErrorMsg, GlobalSQLConnString, DSReturn)

            If blnReturnValue Then
                If DSReturn.Tables(0).Rows.Count > 0 Then
                    With DSReturn.Tables(0).Rows(0)
                        If IsDBNull(.Item("ProgFee")) Then
                            txtProgFee.Text = "0.00"
                        Else
                            txtProgFee.Text = Format(CDbl(.Item("ProgFee")), "##,###,##0.00")
                        End If
                    End With
                Else
                    ClearData()
                    lblMsg.Text = "Record doesn't exits!"
                End If
            Else
                LogError.Log("PTPTN Fee Setup", "MoveDataEdit", strRetrunErrorMsg)
                lblMsg.Text = strRetrunErrorMsg
            End If
            '
        Catch ex As Exception
            LogError.Log("PTPTN Fee Setup", "MoveDataEdit", ex.Message)
            lblMsg.Text = ex.Message
        End Try

    End Sub

    ''' <summary>
    ''' Insert Data From Textbox to DB
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub Insert(ByVal strMode As String)

        Try
            objBE.ProgCode = ddlProgramEdit.SelectedValue
            objBE.ProgName = ddlProgramEdit.SelectedItem.ToString()
            objBE.ProgFee = txtProgFee.Text
            objBE.CreatedBy = Session("User")

            If strMode = "New" Then
                objBE.ProgCode = ddlProgramEdit.SelectedValue
                blnReturnValue = objSQLQuery.RetriveSearchData(objBE, strRetrunErrorMsg, GlobalSQLConnString, DSReturn)

                'Editted By Zoya @24/02/2016
                If DSReturn.Tables(0).Rows.Count = 0 Then
                    blnReturnValue = objSQLQuery.InsertData(objBE, strRetrunErrorMsg, GlobalSQLConnString)

                    If blnReturnValue Then
                        PageFunctional("Default")
                        ClearData()
                        FillDataGrid()
                        lblMsg.Text = "Record Saved Successfully"
                    Else
                        LogError.Log("PTPTN Fee Setup", "Insert", strRetrunErrorMsg)
                        lblMsg.Text = strRetrunErrorMsg
                    End If

                Else
                    lblMsg.Text = "Record already exist."
                    Exit Sub
                End If
                'Done Editted By Zoya @24/02/2016

            Else
                objBE.AutoID = hdnAutoID.Value
                blnReturnValue = objSQLQuery.UpdateData(objBE, strRetrunErrorMsg, GlobalSQLConnString)

                If blnReturnValue Then
                    PageFunctional("Default")
                    ClearData()
                    FillDataGrid()
                    lblMsg.Text = "Record Updated Successfully"
                Else
                    'Editted By zoya @24/02/2016
                    LogError.Log("PTPTN Fee Setup", "Update", strRetrunErrorMsg)
                    'Done Editted By Zoya @24/02/2016
                    lblMsg.Text = strRetrunErrorMsg
                End If
            End If

        Catch ex As Exception
            LogError.Log("PTPTN Fee Setup", "Insert", ex.Message)
            lblMsg.Text = ex.Message
        End Try

    End Sub

    Private Sub Delete(strProgCode As String)
        '
        Try
            objBE.ProgCode = strProgCode
            blnReturnValue = objSQLQuery.DeleteData(objBE, strRetrunErrorMsg, GlobalSQLConnString)

            If blnReturnValue Then
                PageFunctional("Default")
                FillDataGrid()
                lblMsg.Text = "Record Deleted Successfully"
            Else
                LogError.Log("PTPTN Fee Setup", "DeleteData", strRetrunErrorMsg)
                lblMsg.Text = strRetrunErrorMsg
            End If
        Catch ex As Exception
            LogError.Log("PTPTN Fee Setup", "DeleteData", ex.Message)
            lblMsg.Text = ex.Message
        End Try
    End Sub

    Private Sub ClearData()
        ddlProgram.SelectedIndex = 0
        ddlProgramEdit.SelectedIndex = 0
        txtProgFee.Text = ""
        dgPTPTN.Controls.Clear()
        dgPTPTN.Visible = False
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

    Protected Sub ibtnSearch_Click(sender As Object, e As ImageClickEventArgs) Handles ibtnSearch.Click
        FillDataGrid()
    End Sub

    Protected Sub ibtnOpen_Click(sender As Object, e As ImageClickEventArgs) Handles ibtnOpen.Click
        Dim atLeastOneSelected As Boolean = True
        Dim cb As CheckBox
        '
        For Each dgitem In dgPTPTN.Items
            cb = dgitem.Cells(0).Controls(1)
            If cb IsNot Nothing AndAlso cb.Checked Then
                atLeastOneSelected = False
                hdnAutoID.Value = dgitem.Cells(1).Text.Trim
                ddlProgramEdit.SelectedValue = dgitem.Cells(2).Text.Trim
            End If
        Next
        '
        If atLeastOneSelected = False Then
            PageFunctional("Edit")
            MoveDataEdit()
            ViewState("strMode") = "Edit"
        Else
            lblMsg.Text = "Please Select Atleast 1 Program."
        End If
    End Sub

    Protected Sub ibtnSave_Click(sender As Object, e As ImageClickEventArgs) Handles ibtnSave.Click
        strMode = ViewState("strMode")
        Insert(strMode)
    End Sub

    Protected Sub ibtnDelete_Click(sender As Object, e As ImageClickEventArgs) Handles ibtnDelete.Click
        Dim atLeastOneSelected As Boolean = True
        Dim strProgCode As String = ""
        Dim cb As CheckBox
        '
        For Each dgitem In dgPTPTN.Items
            cb = dgitem.Cells(0).Controls(1)
            If cb IsNot Nothing AndAlso cb.Checked Then
                atLeastOneSelected = False
                strProgCode = dgitem.Cells(2).Text.Trim
            End If
        Next
        '
        If atLeastOneSelected = False Then
            Delete(strProgCode)
        Else
            lblMsg.Text = "Please Select Atleast 1 Program."
        End If
    End Sub
End Class
