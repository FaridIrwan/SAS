
'Author			: Anil Kumar - T-Melmax Sdn Bhd
'Created Date	: 28/05/2015

#Region "NameSpaces "
Imports MaxGeneric
Imports System.Data
Imports HTS.SAS.BusinessObjects
Imports HTS.SAS.Entities
#End Region

Partial Class GST_Setup
    Inherits System.Web.UI.Page

#Region "Global Declarations "

    Private _GstSetupDal As New HTS.SAS.DataAccessObjects.GSTSetupDAL
    Private _WorkflowDAL As New HTS.SAS.DataAccessObjects.WorkflowDAL
    Private _MaxModule As New MaxModule.CfCommon

#End Region

#Region "Get Tax Id "

    Public Function GetTaxId() As Short

        Return MaxGeneric.clsGeneric.NullToShort(
            Request.QueryString(Helper.QueryTaxId))

    End Function

#End Region

#Region "Set Message "

    Private Sub SetMessage(ByVal MessageDetails As String)

        lblMsg.Text = String.Empty
        lblMsg.Text = MessageDetails

    End Sub

#End Region

#Region "Page_Load "

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Try

            If Not Page.IsPostBack Then

                'Adding validation for save button
                ibtnSave.Attributes.Add("onclick", "return validate()")
                _TaxPercentage.Attributes.Add("onKeyPress", "isNumberKey(event);")

                'Populate Drop Down Lists - Start
                Call FormHelp.EnumToDropDown(GetType(Generic._TaxCode), ddlTaxCode, True)
                ddlTaxCode.Items.Add(New ListItem("--Select--", "-1"))
                ddlTaxCode.SelectedValue = -1
                Call FormHelp.EnumToDropDown(GetType(Generic._TaxMode), ddlTaxMode, True)
                ddlTaxMode.Items.Add(New ListItem("--Select--", "-1"))
                ddlTaxMode.SelectedValue = -1
                'Populate Drop Down Lists - Stop

                'If tax id available - Start
                If GetTaxId() > 0 Then
                    Call Modify()
                End If
                'If tax id available - Stop

                'Call Bind Grid
                Call BindGrid()

                'While loading the page make the CFlag as null
                Session("PageMode") = ""
                'Loading User Rights
                LoadUserRights()

                'while loading list object make it nothing
                Session("ListObj") = Nothing
                DisableRecordNavigator()
                'load PageName
                ' Menuname(CInt(Request.QueryString("Menuid")))
            End If

        Catch ex As Exception

            Call MaxModule.Helper.LogError(ex.Message)

        End Try

    End Sub

#End Region

#Region "Save GST Tax Records "

    Protected Sub ibtnSave_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ibtnSave.Click
        'Validation 
        SpaceValidation()

        'Call Bind Grid
        Call BindGrid()
    End Sub

#End Region

#Region "Modify "

    Private Sub Modify()

        'Create Instances
        Dim GstDetails As DataSet = Nothing

        Try

            'get Gst Details
            GstDetails = _GstSetupDal.GetGstDetails(GetTaxId())

            If GstDetails.Tables(0).Rows.Count > 0 Then

                With GstDetails.Tables(0)

                    Tax_Id.Value = GetTaxId()
                    ddlTaxCode.SelectedValue = .Rows(0)("")


                End With

            End If

        Catch ex As Exception
            Call MaxModule.Helper.LogError(ex.Message)
        End Try

    End Sub

#End Region

#Region "Validation Controls "

    ''' <summary>
    ''' Method to Validate Before Save
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub SpaceValidation()

        If Trim(_TaxType.Text).Length = 0 Then
            _TaxType.Text = Trim(_TaxType.Text)
            lblMsg.Text = "Enter Valid Description"
            lblMsg.Visible = True
            _TaxType.Focus()
            Exit Sub
        End If

        Call InsertUpdate()

    End Sub

#End Region

#Region "Bind Grid "

    Private Sub BindGrid()

        'Create Instances
        Dim GstDetails As DataSet = Nothing

        Try

            'get Gst Details
            GstDetails = _GstSetupDal.GetGstDetails(0)

            'Bind Data grid - Start
            dgGSTView.DataSource = GstDetails
            dgGSTView.DataBind()
            'Bind Data grid - Stop
        Catch ex As Exception

            MaxModule.Helper.LogError(ex.Message)

        End Try

    End Sub

#End Region

#Region "Insert/Update GST Tax Records "

    Public Sub InsertUpdate()

        'variable declarations - Start
        Dim GstTaxId As Integer = 0, TaxCode As Integer = 0, Result As Short = 0
        Dim TaxType As String = Nothing, TaxPercentage As Decimal = 0, TaxMode As Short = 0
        Dim GLAccount As String = Nothing
        'variable declarations - Stop

        Try

            'Get values - Start
            TaxType = clsGeneric.NullToString(_TaxType.Text)
            GstTaxId = clsGeneric.NullToInteger(Tax_Id.Value)
            TaxMode = clsGeneric.NullToShort(ddlTaxMode.SelectedValue)
            TaxCode = clsGeneric.NullToInteger(ddlTaxCode.SelectedValue)
            TaxPercentage = clsGeneric.NullToDecimal(_TaxPercentage.Text)
            GLAccount = clsGeneric.NullToString(txtGLAccount.Text)
            'Get values - Stop

            'Insert Update Data
            Result = _GstSetupDal.InsertUpdate(GstTaxId, TaxCode, TaxType, TaxPercentage, TaxMode, GLAccount)

            'Check Result
            If Result = -1 Then
                lblMsg.Text = "Insert\Update Failed"
            ElseIf Result = -2 Then
                lblMsg.Text = "Record Already exist"
            Else
                If clsGeneric.NullToInteger(Tax_Id.Value) > 0 Then
                    lblMsg.Text = "Record Updated Successfully!"
                    _TaxType.Text = ""
                    ddlTaxCode.SelectedValue = -1
                    _TaxPercentage.Text = ""
                    ddlTaxMode.SelectedValue = -1

                    txtGLAccount.Text = ""
                    lblDesc.Text = ""
                    lblDesc.Visible = False
                    imgGL.Visible = False
                    'bind the grid again
                    BindGrid()
                Else
                    lblMsg.Text = "Record Saved Successfully!"
                    _TaxType.Text = ""
                    ddlTaxCode.SelectedValue = -1
                    _TaxPercentage.Text = ""
                    ddlTaxMode.SelectedValue = -1

                    txtGLAccount.Text = ""
                    lblDesc.Text = ""
                    lblDesc.Visible = False
                    imgGL.Visible = False
                    'bind the grid again
                    BindGrid()
                End If
            End If

        Catch ex As Exception

            Call MaxModule.Helper.LogError(ex.Message)

        End Try

    End Sub
#End Region

#Region "Grid Item "


    Protected Sub dgGSTView_ItemCommand(source As Object, e As DataGridCommandEventArgs) Handles dgGSTView.ItemCommand

        'GridItem declaration
        Dim gditem As DataGridItem
        'varaible declaration
        Dim rowindex As Integer
        'Create Instances
        Dim GstDetails As DataSet = Nothing

        Try

            gditem = DirectCast(DirectCast(e.CommandSource, LinkButton).NamingContainer, DataGridItem)
            rowindex = DirectCast(dgGSTView.DataKeys(gditem.ItemIndex), Integer)

            'get Gst Details
            GstDetails = _GstSetupDal.GetGstDetails(rowindex)

            'load controls - starting
            _TaxType.Text = GstDetails.Tables(0).Rows(0)("sas_taxtype").ToString()
            _TaxPercentage.Text = GstDetails.Tables(0).Rows(0)("sas_taxpercentage").ToString()
            ddlTaxCode.SelectedValue = clsGeneric.NullToInteger(GstDetails.Tables(0).Rows(0)("sas_taxcode").ToString())
            ddlTaxMode.SelectedValue = clsGeneric.NullToInteger(GstDetails.Tables(0).Rows(0)("sas_taxmod").ToString())
            Tax_Id.Value = clsGeneric.NullToInteger(GstDetails.Tables(0).Rows(0)("sas_taxid").ToString())
            txtGLAccount.Text = clsGeneric.NullToString(GstDetails.Tables(0).Rows(0)("sas_glaccount").ToString())
            'load controls - ended


        Catch ex As Exception
            Call MaxModule.Helper.LogError(ex.Message)
        End Try

    End Sub
#End Region

#Region "Add New Record "
    Protected Sub ibtnNew_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ibtnNew.Click
        OnAdd()
    End Sub
#End Region

#Region "ADD New GST Record "
    Private Sub OnAdd()
        'clear controls - stating
        _TaxType.Text = ""
        _TaxPercentage.Text = ""
        Tax_Id.Value = ""
        lblMsg.Text = ""
        ddlTaxCode.SelectedValue = -1
        ddlTaxMode.SelectedValue = -1

        txtGLAccount.Text = ""
        lblDesc.Text = ""
        lblDesc.Visible = False
        imgGL.Visible = False
        'clear controls - ended
    End Sub

#End Region

#Region "Cancel Record "
    Protected Sub ibtnCancel_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ibtnCancel.Click
        OnCancel()
    End Sub
#End Region

#Region "Cancel GST Record "
    Private Sub OnCancel()
        'clear controls - stating
        _TaxType.Text = ""
        _TaxPercentage.Text = ""
        Tax_Id.Value = ""
        lblMsg.Text = ""
        ddlTaxCode.SelectedValue = -1
        ddlTaxMode.SelectedValue = -1

        txtGLAccount.Text = ""
        lblDesc.Text = ""
        lblDesc.Visible = False
        imgGL.Visible = False
        'clear controls - ended
    End Sub

#End Region

#Region "Cancel Record "
    Protected Sub ibtnDelete_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ibtnDelete.Click
        OnDelete()
    End Sub
#End Region

#Region "Delete GST Record "
    Private Sub OnDelete()

        'variable declarations - Start
        Dim GstTaxId As Integer = 0, Result As Short = 0
        Try

            'Get values - Start
            GstTaxId = clsGeneric.NullToInteger(Tax_Id.Value)

            'Delete GST Record - Starting
            Result = _GstSetupDal.Delete(GstTaxId)
            'Delete GST Record - Ended

            'Check Result
            If Result = -1 Then
                lblMsg.Text = "Delete Failed"
            ElseIf Result = -2 Then
                lblMsg.Text = "Record Already In Use"
            Else
                If clsGeneric.NullToInteger(Tax_Id.Value) > 0 Then
                    lblMsg.Text = "Record Deleted Successfully!"

                    'Call Bind Grid
                    Call BindGrid()

                    'clear controls - stating
                    _TaxType.Text = ""
                    _TaxPercentage.Text = ""
                    Tax_Id.Value = ""
                    ddlTaxCode.SelectedValue = -1
                    ddlTaxMode.SelectedValue = -1

                    txtGLAccount.Text = ""
                    lblDesc.Text = ""
                    lblDesc.Visible = False
                    imgGL.Visible = False
                    'clear controls - ended
                End If
            End If

        Catch ex As Exception

            Call MaxModule.Helper.LogError(ex.Message)

        End Try
    End Sub

#End Region

#Region "Methods"
    ''' <summary>
    ''' Method to Load UserRights
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub LoadUserRights()
        Dim obj As New UsersBAL
        Dim eobj As New UserRightsEn

        Try
            eobj = obj.GetUserRights(CInt(Request.QueryString("Menuid")), CInt(Session("UserGroup")))
        Catch ex As Exception
            LogError.Log("UniversityFund", "LoadUserRights", ex.Message)
        End Try
        'Rights for Add

        If eobj.IsAdd = True Then
            'ibtnSave.Enabled = True
            OnAdd()
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
            ibtnPosting.ImageUrl = "images/posting.png"
            ibtnPosting.ToolTip = "Posting"
        Else
            ibtnPosting.Enabled = False
            ibtnPosting.ImageUrl = "images/gposting.png"
            ibtnPosting.ToolTip = "Access Denied"
        End If
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
    ''' Method to get the MenuName
    ''' </summary>
    ''' <param name="MenuId">Parameter is MenuId</param>
    ''' <remarks></remarks>
    Private Sub Menuname(ByVal MenuId As Integer)
        Dim eobj As New MenuEn
        Dim bobj As New MenuBAL
        eobj.MenuId = MenuId
        Try
            eobj = bobj.GetMenus(eobj)
        Catch ex As Exception
            LogError.Log("UniversityFund", "Menuname", ex.Message)
        End Try
        lblMenuName.Text = eobj.MenuName
    End Sub

#End Region

#Region "GL Account"
    'added by Hafiz @ 28/7/2016

    Protected Sub txtGlAccount_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtGLAccount.TextChanged
        If imgGL.Visible Then
            imgGL.Visible = False
        End If
    End Sub

    Protected Sub Check_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Check.Click

        'varaible declaration
        Dim result As Boolean = False
        Dim _GLCode As String = txtGLAccount.Text
        Dim _GLLedgerType As String = Nothing
        Dim _GLDescription As String = Nothing

        Try
            imgGL.Visible = False
            'Adding validation for Check button
            Check.Attributes.Add("onclick", "return validate()")
            'Check Empty GLCode - Starting
            If Not _GLCode = "" Then
                'Check GLCODE in CF - Starting
                result = _MaxModule.GLCodeValid(_GLCode)

                If result Then
                    'Check GLLedgerType in CF
                    _GLLedgerType = _MaxModule.GetLedgerType(_GLCode)

                    If Not _GLLedgerType = "" Then
                        'Retrive GLDescription in CF 
                        _GLDescription = _MaxModule.GetGLDescription(_GLCode, _GLLedgerType)
                        lblDesc.Text = _GLDescription
                        imgGL.Visible = True
                        imgGL.ImageUrl = "~/images/check.png"
                        lblDesc.Visible = True
                    End If
                Else
                    lblMsg.Text = "Invalid GLCode"
                    imgGL.Visible = True
                    imgGL.ImageUrl = "~/images/cross.png"
                End If
                'Check GLCODE in CF - Ended
            End If

        Catch ex As Exception
            'Log & Show Error - Start
            Call MaxModule.Helper.LogError(ex.Message)
            Call SetMessage(ex.Message)
            'Log & Show Error - Stop
        End Try

    End Sub

#End Region


End Class

