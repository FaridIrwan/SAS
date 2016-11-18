Imports HTS.SAS.Entities
Imports HTS.SAS.BusinessObjects
Imports System.Data
Imports System.Collections.Generic
Imports csvFile
Imports System.IO
Imports System.Text
Imports System.Reflection


Partial Class ExportData
    Inherits System.Web.UI.Page
    Private Const separator As String = "|"
    Dim CFlag As String
    Dim DFlag As String
    Dim ListObjects As List(Of ExportDataEN)
    Private ErrorDescription As String
    Private SaveLocation As String = Server.MapPath("data")

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack() Then
            Session("Menuid") = Request.QueryString("Menuid")
            'load PageName
            Menuname(CInt(Request.QueryString("Menuid")))
            LoadUserRights()
            dates()
            DisableRecordNavigator()
            Session("PageMode") = "Add"
            txtFromDate.ReadOnly = True
            txtToDate.ReadOnly = True
            ibtnFromDate.Visible = False
            ibtnToDate.Visible = False
            ibtnFromDate.Attributes.Add("onClick", "return getFromDate()")
            ibtnToDate.Attributes.Add("onClick", "return getToDate()")
            ibtnSave.Attributes.Add("onClick", "return Validate()")
            'ibtnPosting.Attributes.Add("onClick", "return ValidatePost()")
            'Button1.Attributes.Add("onclick", "return showDialog()")
            txtFromDate.Attributes.Add("OnKeyup", "return CheckFromDate()")
            txtToDate.Attributes.Add("OnKeyup", "return CheckTdate()")
            ibtnDelete.Attributes.Add("onclick", "return getconfirm()")
            txtRecNo.Attributes.Add("OnKeyup", "return geterr()")
            Dim drive As String() = Environment.GetLogicalDrives()
            For Each tdrive As String In drive
                DropDownList1.Items.Add(tdrive.ToString())
            Next


        End If
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
    ''' Method To Change the Date Format(dd/MM/yyyy)
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub setDateFormat()

        'Dim GBFormat As System.Globalization.CultureInfo
        'GBFormat = New System.Globalization.CultureInfo("en-GB")
        Dim myPaymentDate As Date = CDate(CStr(txtFromDate.Text))
        Dim myFormat As String = "dd/MM/yyyy"
        Dim myFormattedDate As String = Format(myPaymentDate, myFormat)
        txtFromDate.Text = myFormattedDate
        Dim myBatchDate As Date = CDate(CStr(txtToDate.Text))
        Dim myFormattedDate2 As String = Format(myBatchDate, myFormat)
        txtToDate.Text = myFormattedDate2

    End Sub
    ''' <summary>
    ''' Method to Save and Update University Funds
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub OnSave()
        Dim bsobj As New ExportDataBAL
        Dim eobj As New ExportDataEN
        Dim RecAff As Integer
        eobj.FileFormat = ddlFileFormat.SelectedValue
        eobj.Interface = ddlinterface.SelectedValue
        eobj.Frequency = ddlFrequency.SelectedValue
        If CheckBox1.Checked = True Then
            eobj.PreviousData = True
        Else
            eobj.PreviousData = False
        End If
        eobj.TimeofExport = ddlhour.SelectedValue.ToString() + ":" + ddlMin.SelectedValue.ToString()
        Dim FSO As Object = CreateObject("scripting.filesystemobject")
        If FSO.FolderExists(txtFilePath.Text) = True Then
            eobj.Filepath = txtFilePath.Text + "\" + txtFileName.Text + ".txt"
        Else
            lblMsg.Text = "File Path is not Valid"
            Exit Sub
        End If

        eobj.DateFrom = Trim(txtFromDate.Text)
        eobj.DateTo = Trim(txtToDate.Text)
        If ChkDateRange.Checked = True Then
            eobj.DateRange = True
        Else
            eobj.DateRange = False
        End If
        eobj.LastUpdatedBy = Session("User")
        eobj.LastUpdatedDateTime = Date.Now.ToString()
        lblMsg.Visible = True

        If Session("PageMode") = "Add" Then
            Try
                RecAff = bsobj.Insert(eobj)
                ErrorDescription = "Record Saved Successfully "
                ibtnStatus.ImageUrl = "images/ready.gif"
                lblStatus.Value = "Ready"
                lblMsg.Text = ErrorDescription
            Catch ex As Exception
                lblMsg.Text = ex.Message.ToString()
                LogError.Log("ExportData", "OnSave", ex.Message)
            End Try
        ElseIf Session("PageMode") = "Edit" Then
            Try
                RecAff = bsobj.Update(eobj)
                ListObjects = Session("ListObj")
                ListObjects(CInt(txtRecNo.Text) - 1) = eobj
                Session("ListObj") = ListObjects

                ErrorDescription = "Record Updated Successfully "
                ibtnStatus.ImageUrl = "images/ready.gif"
                lblStatus.Value = "Ready"
                lblMsg.Text = ErrorDescription
            Catch ex As Exception
                lblMsg.Text = ex.Message.ToString()
                LogError.Log("ExportData", "OnSave", ex.Message)
            End Try

        End If

    End Sub
    ''' <summary>
    ''' Method to get the List Of University Funds
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub LoadListObjects()
        Dim ds As New DataSet
        Dim bobj As New ExportDataBAL
        Dim eobj As New ExportDataEN
        Dim recStu As Integer

        eobj.InterfaceID = ""


        Try
            ListObjects = bobj.GetList(eobj)
        Catch ex As Exception
            LogError.Log("ExportData", "LoadListObjects", ex.Message)
            lblMsg.Text = ex.Message
        End Try
        Session("ListObj") = ListObjects
        lblCount.Text = ListObjects.Count.ToString()

        If ListObjects.Count <> 0 Then
            DisableRecordNavigator()

            If lblCount.Text = 0 Then
                txtRecNo.Text = 0
            Else
                txtRecNo.Text = 1
            End If

            OnMoveFirst()

            If Session("EditFlag") = True Then
                Session("PageMode") = "Edit"
                ibtnSave.Enabled = True
                ibtnSave.ImageUrl = "images/save.png"
            Else
                Session("PageMode") = ""
                ibtnSave.Enabled = False
                ibtnSave.ImageUrl = "images/gsave.png"
            End If
        Else
            txtRecNo.Text = ""
            lblCount.Text = ""
            ibtnStatus.ImageUrl = "images/notready.gif"
            lblStatus.Value = "New"
            OnClearData()
            dates()
            If DFlag = "Delete" Then
            Else
                lblMsg.Visible = True
                ErrorDescription = "Record did not Exist"
                lblMsg.Text = ErrorDescription
                DFlag = ""
            End If
        End If
    End Sub
    ''' <summary>
    ''' Method to Move to First Record
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub OnMoveFirst()
        txtRecNo.Text = "1"
        FillData(0)
    End Sub
    ''' <summary>
    ''' Method to Move to Next Record
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub OnMoveNext()
        txtRecNo.Text = CInt(txtRecNo.Text) + 1
        FillData(CInt(txtRecNo.Text) - 1)
    End Sub
    ''' <summary>
    ''' Method to Move to Previous Record
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub OnMovePrevious()
        txtRecNo.Text = CInt(txtRecNo.Text) - 1
        FillData(CInt(txtRecNo.Text) - 1)
    End Sub
    ''' <summary>
    ''' Method to Move to Last Record
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub OnMoveLast()
        txtRecNo.Text = lblCount.Text
        FillData(CInt(lblCount.Text) - 1)
    End Sub
    ''' <summary>
    ''' Method to Enable or Disable Navigation Buttons
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub DisableRecordNavigator()
        Dim flag As Boolean
        If Session("ListObj") Is Nothing Then
            flag = False
            txtRecNo.Text = ""
            lblCount.Text = ""
        Else
            flag = True
        End If
        ibtnFirst.Enabled = flag
        ibtnLast.Enabled = flag
        ibtnPrevs.Enabled = flag
        ibtnNext.Enabled = flag
        If flag = False Then
            ibtnFirst.ImageUrl = "images/gnew_first.png"
            ibtnLast.ImageUrl = "images/gnew_last.png"
            ibtnPrevs.ImageUrl = "images/gnew_Prev.png"
            ibtnNext.ImageUrl = "images/gnew_next.png"
        Else
            ibtnFirst.ImageUrl = "images/new_last.png"
            ibtnLast.ImageUrl = "images/new_first.png"
            ibtnPrevs.ImageUrl = "images/new_Prev.png"
            ibtnNext.ImageUrl = "images/new_next.png"

        End If
    End Sub
    ''' <summary>
    ''' Method to Fill the Field Values
    ''' </summary>
    ''' <param name="RecNo"></param>
    ''' <remarks></remarks>
    Private Sub FillData(ByVal RecNo As Integer)
        'Conditions for Button Enable & Disable

        If txtRecNo.Text = lblCount.Text Then
            ibtnNext.Enabled = False
            ibtnNext.ImageUrl = "images/gnew_next.png"
            ibtnLast.Enabled = False
            ibtnLast.ImageUrl = "images/gnew_last.png"
        Else
            ibtnNext.Enabled = True
            ibtnNext.ImageUrl = "images/new_next.png"
            ibtnLast.Enabled = True
            ibtnLast.ImageUrl = "images/new_last.png"
        End If
        If txtRecNo.Text = "1" Then
            ibtnPrevs.Enabled = False
            ibtnPrevs.ImageUrl = "images/gnew_Prev.png"
            ibtnFirst.Enabled = False
            ibtnFirst.ImageUrl = "images/gnew_first.png"
        Else
            ibtnPrevs.Enabled = True
            ibtnPrevs.ImageUrl = "images/new_prev.png"
            ibtnFirst.Enabled = True
            ibtnFirst.ImageUrl = "images/new_first.png"
        End If
        If txtRecNo.Text = 0 Then
            txtRecNo.Text = 1
        Else
            If lblCount.Text = 0 Then
                txtRecNo.Text = 0
            Else

                Dim obj As ExportDataEN
                Dim myFolderPath As String
                Dim myFileName As String
                ListObjects = Session("ListObj")
                obj = ListObjects(RecNo)

                interfaceid.Value = obj.InterfaceID
                ddlinterface.SelectedValue = obj.Interface
                ddlFrequency.SelectedValue = obj.Frequency
                ddlFileFormat.SelectedValue = obj.FileFormat
                Dim time As String = obj.TimeofExport
                Dim arr As String() = time.Split(":")
                ddlhour.SelectedValue = arr(0)
                ddlMin.SelectedValue = arr(1)
                Dim filepat As String = obj.Filepath
                Dim filearr As String() = filepat.Split("*")
                myFileName = Right(filepat, Len(filepat) - InStrRev(filepat, "\"))
                myFolderPath = Left(filepat, InStrRev(filepat, "\") - 1)
                txtFilePath.Text = myFolderPath
                txtFileName.Text = myFileName
                txtFromDate.Text = obj.DateFrom
                txtToDate.Text = obj.DateTo
                ibtnToDate.Visible = True
                ibtnFromDate.Visible = True
                ChkDateRange.Checked = True
                ibtnStatus.ImageUrl = "images/ready.gif"
                lblStatus.Value = "Ready"
                If obj.PreviousData = True Then
                    CheckBox1.Checked = True
                Else
                    CheckBox1.Checked = False
                End If

            End If
        End If
        setDateFormat()
    End Sub
    ''' <summary>
    ''' Method to Clear the Field Values
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub OnClearData()
        lblStatus.Value = "New"
        ibtnStatus.ImageUrl = "images/notready.gif"
        'Clear Text Box values
        Session("ListObj") = Nothing
        DisableRecordNavigator()
        interfaceid.Value = ""
        ddlinterface.SelectedValue = "All"
        ddlFrequency.SelectedValue = "daily"
        ddlFileFormat.SelectedValue = ","
        ddlhour.SelectedValue = "00"
        ddlMin.SelectedValue = "00"
        txtFilePath.Text = ""
        txtFileName.Text = ""
        ChkDateRange.Checked = False
        CheckBox1.Checked = False
        ibtnToDate.Visible = False
        ibtnFromDate.Visible = False
        lblMsg.Text = ""
        Session("PageMode") = "Add"
    End Sub
    ''' <summary>
    ''' Method to get the ExportData
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub GetExportData()
        Dim loAccountsEn As New AccountsEn
        Dim loAccountsBAL As New AccountsBAL
        Dim lolist As New List(Of AccountsEn)
        loAccountsEn.SubType = ddlPayer.SelectedValue.ToString()
        loAccountsEn.Category = ddlTransactionType.SelectedValue.ToString()
        loAccountsEn.TransDate = Trim(txtFromDate.Text)
        loAccountsEn.BatchDate = Trim(txtToDate.Text)

        Dim Str As String = DropDownList1.SelectedItem.Text + "\" + txtFileName.Text
        If ddlTransactionType.SelectedValue = "Payments" Then
            If ddlPayer.SelectedValue = "Student" Then
                Dim lolist1 As New List(Of AccountsEn)
                Dim lolist2 As New List(Of AccountsDetailsEn)
                loAccountsEn.Category = "Payment"
                lolist1 = loAccountsBAL.GetExportData(loAccountsEn)
                'lo()
                'CreateCSVFromGenericListReceipt(lolist, Str)
                CreateCSVFromGenericListPayments(lolist, Str)
            ElseIf ddlPayer.SelectedValue = "Sponsor" Then

            End If
        Else
            lolist = loAccountsBAL.GetExportData(loAccountsEn)
            CreateCSVFromGenericListGL(lolist, Str)
        End If
    End Sub
    ''' <summary>
    ''' Method to Validate
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub SpaceValidation()
        ibtnSave.Attributes.Add("onclick", "return Validate()")
        Dim GBFormat As System.Globalization.CultureInfo
        GBFormat = New System.Globalization.CultureInfo("en-GB")
        'Batch date
        If Trim(txtFromDate.Text).Length < 10 Then
            lblMsg.Text = "Enter Valid From Date"
            lblMsg.Visible = True
            txtFromDate.Focus()
            Exit Sub
        Else
            Try
                txtFromDate.Text = DateTime.Parse(txtFromDate.Text, GBFormat)
            Catch ex As Exception
                lblMsg.Text = "Enter Valid From Date"
                lblMsg.Visible = True
                txtFromDate.Focus()
                Exit Sub
            End Try
        End If
        'Invoice date
        If Trim(txtToDate.Text).Length < 10 Then
            lblMsg.Text = "Enter Valid To Date"
            lblMsg.Visible = True
            txtToDate.Focus()
            Exit Sub
        Else
            Try
                txtToDate.Text = DateTime.Parse(txtToDate.Text, GBFormat)
            Catch ex As Exception
                lblMsg.Text = "Enter Valid To Date"
                lblMsg.Visible = True
                txtToDate.Focus()
                Exit Sub
            End Try
        End If

    End Sub
    Public Shared Sub CreateCSVFromGenericListPayments(ByVal list As List(Of AccountsEn), ByVal Str As String)
        Dim csvString As New StringBuilder()
        Dim accountdetaillist As New List(Of AccountsDetailsEn)
        Dim accountlist As New List(Of AccountsEn)

        Dim filePath As String = Str
        Dim i As Integer = 0

        accountlist = list
        For Each loen As AccountsEn In accountlist
            csvString.Append("HTS")
            csvString.Append(separator)
            csvString.Append(loen.BatchCode.ToString())
            csvString.Append(separator)
            csvString.Append("CIMB")
            csvString.Append(separator)
            csvString.Append(loen.BatchDate.ToString())
            csvString.Append(separator)
            csvString.Append(loen.CreditRef.ToString())
            csvString.Append(separator)
            csvString.Append(loen.TransactionCode.ToString())
            csvString.Append(separator)
            csvString.Append(loen.CreditRef.ToString())
            csvString.Append(separator)
            csvString.Append(loen.TransDate.ToString())
            csvString.Append(separator)
            csvString.Append(loen.DueDate.ToString())
            csvString.Append(separator)
            csvString.Append(loen.TransactionAmount.ToString())
            csvString.Append(separator)
            csvString.Append(loen.TransactionAmount.ToString())
            csvString.Append(separator)
            csvString.Append("")
            csvString.Append(separator)
            csvString.Append(loen.Description.ToString())
            csvString.Append(separator)
            If loen.AccountDetailsList.Count > 0 Then
                accountdetaillist = loen.AccountDetailsList
                For Each lodetailen As AccountsDetailsEn In accountdetaillist
                    csvString.Append(lodetailen.ReferenceCode.ToString())
                    csvString.Append(separator)
                    csvString.Append(lodetailen.TransactionAmount.ToString())
                    csvString.Append(separator)

                Next

            End If
            csvString.Append("R")
            csvString.Append(separator)

            'Next Line
            csvString.AppendLine()


        Next

        Try
            Dim streamWriter As New StreamWriter(filePath)
            streamWriter.Write(csvString.ToString())
            streamWriter.Close()
        Catch ex As Exception


        End Try
    End Sub
    Public Shared Sub CreateCSVFromGenericListGL(ByVal list As List(Of AccountsEn), ByVal Str As String)

        Dim csvString As New StringBuilder()
        Dim accountdetaillist As New List(Of AccountsDetailsEn)
        Dim accountlist As New List(Of AccountsEn)


        Dim filePath As String = Str
        Dim i As Integer = 0

        accountlist = list
        For Each loen As AccountsEn In accountlist
            csvString.Append("HTS")
            csvString.Append(separator)
            csvString.Append(loen.BatchCode.ToString())
            csvString.Append(separator)
            csvString.Append("cimb")
            csvString.Append(separator)
            csvString.Append(loen.BatchDate.ToString())
            csvString.Append(separator)
            csvString.Append(loen.CreditRef.ToString())
            csvString.Append(separator)
            csvString.Append(loen.TransactionCode.ToString())
            csvString.Append(separator)
            csvString.Append(loen.CreditRef.ToString())
            csvString.Append(separator)
            csvString.Append(loen.TransDate.ToString())
            csvString.Append(separator)
            csvString.Append(loen.DueDate.ToString())
            csvString.Append(separator)
            csvString.Append(loen.TransactionAmount.ToString())
            csvString.Append(separator)
            csvString.Append(loen.TransactionAmount.ToString())
            csvString.Append(separator)
            csvString.Append("")
            csvString.Append(separator)
            csvString.Append(loen.Description.ToString())
            csvString.Append(separator)
            If loen.AccountDetailsList.Count > 0 Then
                accountdetaillist = loen.AccountDetailsList
                For Each lodetailen As AccountsDetailsEn In accountdetaillist
                    csvString.Append(lodetailen.ReferenceCode.ToString())
                    csvString.Append(separator)
                    csvString.Append(lodetailen.TransactionAmount.ToString())
                    csvString.Append(separator)

                Next

            End If
            csvString.Append("R")
            csvString.Append(separator)

            'Next Line
            csvString.AppendLine()


        Next

        Try
            Dim streamWriter As New StreamWriter(filePath)
            streamWriter.Write(csvString.ToString())
            streamWriter.Close()
        Catch ex As Exception

        End Try
    End Sub
    ''' <summary>
    ''' Method to Change the Date Format
    ''' </summary>
    ''' <remarks>Date in ddd/mm/yyyy Format</remarks>
    Private Sub dates()
        'Dim GBFormat As System.Globalization.CultureInfo
        'GBFormat = New System.Globalization.CultureInfo("en-GB")
        txtToDate.Text = Format(Date.Now, "dd/MM/yyyy")
        txtFromDate.Text = Format(Date.Now, "dd/MM/yyyy")
    End Sub
    ''' <summary>
    ''' Method to Load the UserRights
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub LoadUserRights()
        Dim obj As New UsersBAL
        Dim eobj As New UserRightsEn


        Try
            eobj = obj.GetUserRights(CInt(Request.QueryString("Menuid")), CInt(Session("UserGroup")))
        Catch ex As Exception
            LogError.Log("ExportData", "LoadUserRights", ex.Message)
        End Try
        'Rights for Add

        If eobj.IsAdd = True Then
            'ibtnSave.Enabled = True
            'onAdd()
            ibtnNew.ImageUrl = "images/add.png"
            ibtnNew.Enabled = True
        Else
            ibtnNew.ImageUrl = "images/gadd.png"
            ibtnNew.Enabled = False
            ibtnNew.ToolTip = "Access Denied"

        End If
        'Rights for Edit
        If eobj.IsEdit = True Then
            ibtnSave.Enabled = True
            ibtnSave.ImageUrl = "images/save.png"
            ibtnSave.ToolTip = "Edit"
            If eobj.IsAdd = False Then
                ibtnSave.Enabled = False
                ibtnSave.ImageUrl = "images/gsave.png"
                ibtnSave.ToolTip = "Access Denied"
            End If

            Session("EditFlag") = True

        Else
            Session("EditFlag") = False
            ibtnSave.Enabled = False
            ibtnSave.ImageUrl = "images/gsave.png"
        End If
        'Rights for View
        ibtnView.Enabled = eobj.IsView
        If eobj.IsView = True Then
            ibtnView.ImageUrl = "images/find.png"
            ibtnView.Enabled = True
        Else
            ibtnView.ImageUrl = "images/gfind.png"
            ibtnView.ToolTip = "Access Denied"
        End If
        'Rights for Delete
        If eobj.IsDelete = True Then
            ibtnDelete.ImageUrl = "images/delete.png"
            ibtnDelete.Enabled = True
        Else
            ibtnDelete.ImageUrl = "images/gdelete.png"
            ibtnDelete.ToolTip = "Access Denied"
            ibtnDelete.Enabled = False
        End If
        'Rights for Print
        ibtnPrint.Enabled = eobj.IsPrint
        If eobj.IsPrint = True Then
            ibtnPrint.Enabled = True
            ibtnPrint.ImageUrl = "images/print.png"
            ibtnPrint.ToolTip = "Print"
        Else
            ibtnPrint.Enabled = False
            ibtnPrint.ImageUrl = "images/gprint.png"
            ibtnPrint.ToolTip = "Access Denied"
        End If
        If eobj.IsOthers = True Then
            ibtnOthers.Enabled = True
            ibtnOthers.ImageUrl = "images/others.png"
            ibtnOthers.ToolTip = "Others"
        Else
            ibtnOthers.Enabled = False
            ibtnOthers.ImageUrl = "images/gothers.png"
            ibtnOthers.ToolTip = "Access Denied"
        End If
        If eobj.IsPost = True Then
            ibtnPosting.Enabled = True
            ibtnPosting.ImageUrl = "images/Export.png"
            ibtnPosting.ToolTip = "Export"
        Else
            ibtnPosting.Enabled = False
            ibtnPosting.ImageUrl = "images/Export.png"
            ibtnPosting.ToolTip = "Access Denied"
        End If
    End Sub
    ''' <summary>
    ''' Method to Load Fields in New Mode
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub OnAdd()
        Session("PageMode") = "Add"
        ibtnSave.Enabled = True
        ibtnSave.ImageUrl = "images/save.png"
        OnClearData()

    End Sub
    ''' <summary>
    ''' Method to Delete ExportData
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub OnDelete()
        lblMsg.Visible = True
        If interfaceid.Value <> "" Then
            If lblCount.Text = "" Then lblCount.Text = 0
            If lblCount.Text > 0 Then
                Dim bsobj As New ExportDataBAL
                Dim eobj As New ExportDataEN

                Dim RecAff As Integer
                lblMsg.Visible = True
                eobj.InterfaceID = interfaceid.Value
                Try
                    RecAff = bsobj.Delete(eobj)
                    ListObjects = Session("ListObj")
                    ListObjects.RemoveAt(CInt(txtRecNo.Text) - 1)
                    lblCount.Text = lblCount.Text - 1
                    Session("ListObj") = ListObjects
                    ErrorDescription = "Record Deleted Successfully "
                    lblMsg.Text = ErrorDescription
                Catch ex As Exception
                    lblMsg.Text = ex.Message.ToString()
                    LogError.Log("UniversityFund", "OnDelete", ex.Message)
                End Try
                interfaceid.Value = ""
                ddlFrequency.SelectedValue = "daily"
                ddlinterface.SelectedValue = "All"
                ddlFileFormat.SelectedValue = ","
                ddlhour.SelectedValue = "00"
                ddlMin.SelectedValue = "00"
                DFlag = "Delete"
                LoadListObjects()
            Else
                ErrorDescription = "Select a Record to Delete"
                lblMsg.Text = ErrorDescription
            End If
        Else
            ErrorDescription = "Select a Record to Delete"
            lblMsg.Text = ErrorDescription
        End If

    End Sub
#End Region


    Protected Sub ddlPayer_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        If ddlPayer.SelectedValue = "Student" Then
            ddlTransactionType.Items.Item(2).Enabled = False
            ddlTransactionType.Items.Item(1).Enabled = True
        ElseIf ddlPayer.SelectedValue = "Sponsor" Then
            ddlTransactionType.Items.Item(1).Enabled = False
            ddlTransactionType.Items.Item(2).Enabled = True
        ElseIf ddlPayer.SelectedValue = "All" Then
            ddlTransactionType.Items.Item(1).Enabled = True
            ddlTransactionType.Items.Item(2).Enabled = True
        End If
    End Sub

    Protected Sub ibtnSave_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ibtnSave.Click
        SpaceValidation()
        GetExportData()
        OnSave()
        setDateFormat()
    End Sub

    Protected Sub Button1_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        'Dim path As String = Server.MapPath("") 'get file object as FileInfo
    End Sub

    Protected Sub ChkDateRange_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        If ChkDateRange.Checked = True Then
            txtFromDate.ReadOnly = False
            txtToDate.ReadOnly = False
            ibtnFromDate.Visible = True
            ibtnToDate.Visible = True
        Else
            txtFromDate.ReadOnly = True
            txtToDate.ReadOnly = True
            ibtnFromDate.Visible = False
            ibtnToDate.Visible = False
            dates()
        End If
    End Sub

    Protected Sub ibtnCancel_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        LoadUserRights()
        OnClearData()
        dates()
    End Sub

    Protected Sub ibtnView_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ibtnView.Click
        Session("loaddata") = "View"
        If lblCount.Text <> "" Then
            If CInt(lblCount.Text) > 0 Then
                OnAdd()
            Else

                Session("PageMode") = "Edit"
                LoadListObjects()
            End If
        Else
            Session("PageMode") = "Edit"
            LoadListObjects()

        End If
        If lblCount.Text.Length = 0 Then
            Session("PageMode") = "Add"
        End If
    End Sub

    Protected Sub ibtnDelete_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        OnDelete()
    End Sub

    Protected Sub ibtnNew_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        OnAdd()
    End Sub

    Protected Sub ibtnNext_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        OnMoveNext()
    End Sub

    Protected Sub ibtnLast_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        OnMoveLast()
    End Sub

    Protected Sub ibtnPrevs_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        OnMovePrevious()
    End Sub

    Protected Sub ibtnFirst_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        OnMoveFirst()
    End Sub

    Protected Sub ibtnPosting_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ibtnPosting.Click
        SpaceValidation()
        Dim eobj As New ExportDataEN
        eobj.FileFormat = ddlFileFormat.SelectedValue
        eobj.Interface = ddlinterface.SelectedValue
        eobj.Frequency = ddlFrequency.SelectedValue
        If CheckBox1.Checked = True Then
            eobj.PreviousData = True
        Else
            eobj.PreviousData = False
        End If
        eobj.TimeofExport = ddlhour.SelectedValue.ToString() + ":" + ddlMin.SelectedValue.ToString()
        If ChkDateRange.Checked = True Then
            eobj.DateFrom = Trim(txtFromDate.Text)
            eobj.DateTo = Trim(txtToDate.Text)
        Else
            eobj.DateFrom = Trim(txtFromDate.Text)
            eobj.DateTo = Trim(txtToDate.Text)
        End If
        If ChkDateRange.Checked = True Then
            eobj.DateRange = True
        Else
            eobj.DateRange = False
        End If
        Dim FSO As Object = CreateObject("scripting.filesystemobject")
        Dim FILE_NAME As String
        If FSO.FolderExists(txtFilePath.Text) = True Then
            eobj.Filepath = txtFilePath.Text + "\" + txtFileName.Text
        Else
            lblMsg.Text = "File Path is not Valid"
            Exit Sub
        End If

        
        Try
            GetExportData()
            FILE_NAME = eobj.Filepath
            Response.ContentType = "text/plain"
            Response.AddHeader("content-disposition", "attachment; filename=" & FILE_NAME & "")
            Response.TransmitFile(FILE_NAME)
            Response.End()

            eobj.LastUpdatedBy = Session("User")
            eobj.LastUpdatedDateTime = Date.Now
            ErrorDescription = "Record Exported Successfully "
            lblMsg.Text = ErrorDescription
        Catch ex As Exception
            LogError.Log("ExportData", "ibtnPosting_Click", ex.Message)
        End Try
        setDateFormat()
    End Sub
End Class
