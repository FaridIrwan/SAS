Imports HTS.SAS.Entities
Imports HTS.SAS.BusinessObjects
Imports System.Data
Imports System.Collections.Generic
Partial Class StudentFeetype
    Inherits System.Web.UI.Page
    Dim listfee As New List(Of FeeTypesEn)
    ''Private LogErrors As LogError
    Dim Category As String
    Dim SelectionType As String ''S - Single selection (hide checkbox), M - Multiple selection with checkbox
    Dim StudentCategory As String

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Category = Request.QueryString("Category")
        SelectionType = Request.QueryString("SelectionType")
        'StudentCategory = Request.QueryString("Student")

        If Not Me.IsPostBack Then
            StudentCategory = Request.QueryString("Student")
            'LoadGrid()
            ibtnOK.Visible = False
            Label2.Visible = False
            ibtnClose.Visible = False
            Label3.Visible = False
            LoadStudentCategory()
            If Category <> String.Empty And Category <> Nothing Then
                If Category = "H" Then
                    ddlFeeCategory.SelectedValue = "H"
                    ddlFeeCategory.Enabled = False
                ElseIf Category = "A" Then
                    ddlFeeCategory.SelectedValue = "A"
                    ddlFeeCategory.Enabled = False
                ElseIf Category = "T" Then
                    ddlFeeCategory.SelectedValue = "T"
                    ddlFeeCategory.Enabled = False
                ElseIf Category = "S" Then
                    'ddlFeeCategory.Items.Remove(New ListItem("Tuition Fee", "T"))
                    'ddlFeeCategory.DataBind()
                    ddlFeeCategory.SelectedValue = "A"
                    ddlFeeCategory.Enabled = False
                Else
                    ddlFeeCategory.SelectedValue = "-1"
                    ddlFeeCategory.Enabled = True
                End If
            Else
                ddlFeeCategory.SelectedValue = "-1"
                ddlFeeCategory.Enabled = True
            End If

        End If
    End Sub
    Protected Sub ibtnOK_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ibtnOK.Click
        laodvalues()
        UpdateGrid()

    End Sub

    Protected Sub ibtnClose_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ibtnClose.Click
        UpdateGrid()
    End Sub

    Protected Sub ibtnView_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ibtnView.Click
        LoadGrid()
    End Sub

#Region "Methods"
    ''' <summary>
    ''' Method to Load Student Category Dropdown
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub LoadStudentCategory()
        Dim bStuCategory As New StudentCategoryBAL
        Dim eStuCategory As New StudentCategoryEn

        'Assign Student Category - 28/4/2016 Start
        If StudentCategory <> String.Empty And StudentCategory <> Nothing Then
            eStuCategory.StudentCategoryCode = StudentCategory
        Else
            eStuCategory.StudentCategoryCode = ""
        End If
        'Assign Student Category - 28/4/2016 End

        eStuCategory.Description = ""
        eStuCategory.Status = True
        ddlStudentCategory.Items.Clear()
        Dim ListStuCat As New List(Of StudentCategoryEn)
        ddlStudentCategory.DataTextField = "Description"
        ddlStudentCategory.DataValueField = "StudentCategoryCode"
        
        Try
            ddlStudentCategory.DataSource = bStuCategory.GetStudentCategoryListAll(eStuCategory)
            ddlStudentCategory.DataBind()
            ddlStudentCategory.Items.Add(New ListItem("--Select--", "-1"))
            ddlStudentCategory.SelectedValue = -1
        Catch ex As Exception
            LogError.Log("StudentFeeType", "LoadStudentCategory", ex.Message)
        End Try

    End Sub
    ''' <summary>
    ''' Method to Load the Grid With FeeTypes
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub LoadGrid()

        Dim eobj As New List(Of FeeTypesEn)
        Dim bobj As New FeeTypesBAL
        Dim listobj As New List(Of FeeTypesEn)
        Dim eob As New FeeTypesEn
        Dim feeType As String
        Dim feeCat As String

        'clear grid
        dgView.DataSource = Nothing
        dgView.DataBind()

        If ddlFeeType.SelectedValue = -1 Then
            feeType = ""
        Else
            feeType = ddlFeeType.SelectedValue

        End If
        If ddlFeeCategory.SelectedValue = "-1" Then
            feeCat = ""
        Else
            feeCat = ddlFeeCategory.SelectedValue

        End If
        eob.SCCode = ddlStudentCategory.SelectedValue
        If ddlStudentCategory.SelectedValue = "-1" Then
            eob.SCCode = ""
        Else
            eob.SCCode = ddlStudentCategory.SelectedValue
        End If
        'eob.SCCode = "BW"
        eob.FeeTypeCode = txtFeeCode.Text
        eob.Description = txtFeeDesc.Text
        eob.FeeType = feeType
        eob.Hostel = feeCat
        eob.Status = True

        Try
            'listobj = bobj.GetStudentFee(eob)
            listobj = bobj.GetFeeDetails(eob)

        Catch ex As Exception
            LogError.Log("StudentFeeType", "LoadGrid", ex.Message)
        End Try
        If listobj.Count <> 0 Then
            dgView.DataSource = listobj
            If eob.SCCode = "" Then
                dgView.Columns(3).Visible = False
                dgView.Columns(4).Visible = False
                'dgView.Columns(9).Visible = False
                dgView.Columns(5).Visible = True
                dgView.Columns(6).Visible = True
                'dgView.Columns(10).Visible = True
                'dgView.Columns(11).Visible = True
            Else
                dgView.Columns(3).Visible = True
                dgView.Columns(4).Visible = True
                'dgView.Columns(9).Visible = True
                dgView.Columns(5).Visible = False
                dgView.Columns(6).Visible = False
                'dgView.Columns(10).Visible = False
                'dgView.Columns(11).Visible = False
            End If
            SelectionType = Request.QueryString("SelectionType")
            If SelectionType = "S" Then
                dgView.Columns(0).Visible = False
            Else
                dgView.Columns(0).Visible = True
            End If
            dgView.DataBind()
            ibtnOK.Visible = True
            Label2.Visible = True
            ibtnClose.Visible = True
            Label3.Visible = True

            'Dim chk As CheckBox
            'Dim dgitem As DataGridItem

            'For Each dgitem In dgView.Items
            '    chk = dgitem.Cells(0).Controls(1)
            '    chk.Checked = True
            'Next

        Else
            Response.Write("No Fee types are Available")
        End If
    End Sub
    ''' <summary>
    ''' Method to Update Grid
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub UpdateGrid()
        'Dim cScript As String = ""
        'cScript &= "<script language='javascript'>"
        'cScript &= "window.opener.location.href = window.opener.location.href;"
        'cScript &= "window.close();"
        'cScript &= "</script>"
        Dim cScript As String = ""
        cScript = "<SCRIPT LANGUAGE=""JavaScript"">window.opener.document.getElementById('ctl00_ContentPlaceHolder1_btnHidden').click();self.close();</SCRIPT>"
        ClientScript.RegisterStartupScript(Me.GetType(), "ClosePopUp", cScript)
        'Response.Write(cScript)
    End Sub
    ''' <summary>
    ''' Method to Load Grid Values in a Session
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub laodvalues()
        Dim eobj As New FeeTypesEn
        SelectionType = Request.QueryString("SelectionType")
        If SelectionType = "S" Then ''Single Selection
            If dgView.SelectedIndex <> -1 Then
                eobj = New FeeTypesEn
                eobj.FeeTypeCode = dgView.DataKeys(dgView.SelectedIndex).ToString()
                eobj.Description = dgView.Items(dgView.SelectedIndex).Cells(2).Text
                eobj.Priority = dgView.Items(dgView.SelectedIndex).Cells(7).Text
                eobj.tempAmount = dgView.Items(dgView.SelectedIndex).Cells(4).Text
                eobj.LocalAmount = dgView.Items(dgView.SelectedIndex).Cells(5).Text
                eobj.LocalGSTAmount = dgView.Items(dgView.SelectedIndex).Cells(10).Text
                eobj.NonLocalAmount = dgView.Items(dgView.SelectedIndex).Cells(6).Text
                eobj.NonLocalGSTAmount = dgView.Items(dgView.SelectedIndex).Cells(11).Text
                eobj.GSTAmount = dgView.Items(dgView.SelectedIndex).Cells(9).Text
                eobj.TaxId = dgView.Items(dgView.SelectedIndex).Cells(8).Text
                eobj.LocalCategory = dgView.Items(dgView.SelectedIndex).Cells(13).Text
                eobj.NonLocalCategory = dgView.Items(dgView.SelectedIndex).Cells(12).Text
                listfee.Add(eobj)
                eobj = Nothing
            End If
        Else ''Multiple Selection
            Dim chkBox As CheckBox
            For Each dgi As DataGridItem In dgView.Items
                chkBox = dgi.Cells(0).Controls(1)
                'If chkBox.Checked = True Then
                '    eobj = New FeeTypesEn
                '    eobj.FeeTypeCode = dgView.DataKeys(dgi.ItemIndex).ToString
                '    eobj.Description = dgi.Cells(2).Text
                '    eobj.Priority = dgi.Cells(4).Text
                '    eobj.tempAmount = dgi.Cells(3).Text
                '    eobj.GSTAmount = dgi.Cells(5).Text
                '    eobj.TaxId = dgi.Cells(6).Text
                '    listfee.Add(eobj)
                '    eobj = Nothing
                'End If
                If chkBox.Checked = True Then
                    eobj = New FeeTypesEn
                    eobj.FeeTypeCode = dgView.DataKeys(dgi.ItemIndex).ToString
                    eobj.Description = dgi.Cells(2).Text
                    eobj.Priority = dgi.Cells(7).Text
                    eobj.tempAmount = dgi.Cells(4).Text
                    eobj.LocalAmount = dgi.Cells(5).Text
                    eobj.LocalGSTAmount = dgi.Cells(10).Text
                    eobj.NonLocalAmount = dgi.Cells(6).Text
                    eobj.NonLocalGSTAmount = dgi.Cells(11).Text
                    eobj.GSTAmount = dgi.Cells(9).Text
                    eobj.TaxId = dgi.Cells(8).Text
                    eobj.LocalCategory = dgi.Cells(13).Text
                    eobj.NonLocalCategory = dgi.Cells(12).Text
                    listfee.Add(eobj)
                    eobj = Nothing
                End If
            Next
        End If
        Session("eobj") = listfee
        'End If
    End Sub
#End Region
End Class
