Imports HTS.SAS.BusinessObjects
Imports HTS.SAS.Entities
Imports System.Data
Imports System.Collections.Generic
Imports System.Linq
Partial Class FeeStructure
    Inherits System.Web.UI.Page
#Region "Global Declarations "
    'Instant Declaration
    Private _GstSetupDal As New HTS.SAS.DataAccessObjects.GSTSetupDAL
    Private ListFeeStrDet As List(Of FeeStructEn)
    Private ListFeeStrAmt As List(Of FeeStrAmountEn)
    Private ListObjects As List(Of FeeStructEn)
    Public GlistStud As List(Of StudentEn)
    'Added Mona 3/3/2016
    Public PlistStud As List(Of StudentEn)
    Dim listStudFee As List(Of StudentEn)

    Dim DFlag As String
    Dim feeflag As String
    Private ErrorDescription As String

    Private _StudentDAL As New HTS.SAS.DataAccessObjects.StudentDAL
    Dim listStud As List(Of StudentEn)

#End Region

    ''Private LogErrors As LogError
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack() Then
            'Adding Validation for save button
            ibtnSave.Attributes.Add("onclick", "return Validate()")
            ibtnDelete.Attributes.Add("onclick", "return getconfirm()")
            txtRecNo.Attributes.Add("OnKeyUp", "return geterr()")
            txtSemster.Attributes.Add("OnKeyPress", "return checkSemValue()")
            txtTutPoint.Attributes.Add("OnKeyPress", "return checknValue()")
            txtTutAmt.Attributes.Add("OnKeyPress", "return checknValue()")
            'Loading User Rights
            LoadUserRights()
            Session("PageMode") = "Add"
            If Session("PageMode") = "Edit" Then
                ddlBidang.Enabled = False
                ddlEFrom.Enabled = False
            Else
                ddlBidang.Enabled = True
                ddlEFrom.Enabled = True
            End If
            Session("AddFee") = Nothing
            'added Mona 3/3/2016
            Session("StudentList") = Nothing
            pnlSemester.Visible = False
            pnlTution.Visible = False
            'addSemester()
            'While loading the page make the CFlag as null
            ddlSession.Items.Clear()
            ddlSession.Items.Add(New ListItem("---Select---", "-1"))
            'while loading list object make it nothing
            Session("ListObj") = Nothing
            DisableRecordNavigator()
            'load PageName
            Menuname(CInt(Request.QueryString("Menuid")))

            'ibtnAddFeeType.Attributes.Add("onclick", "new_window=window.open('StudentFeetype.aspx','Hanodale','width=600,height=500,resizable=0,center');new_window.focus();")
            resetTabSelection()

            If CInt(Request.QueryString("IsFeePosting")).Equals(1) Then
                tblMenu.Visible = False
            End If
            If Request.QueryString("Formid") = "FS" Then
                Try
                    LoadListObjects()
                    DirectCast(Master.FindControl("Panel1"), System.Web.UI.WebControls.Panel).Visible = False
                    DirectCast(Master.FindControl("td"), System.Web.UI.HtmlControls.HtmlTableCell).Visible = False
                    SiteMapPath1.Visible = False
                    Label26.Visible = False
                    Label27.Visible = False
                    ibtnAddFeeType.Visible = False
                    ibtnRemoveFee.Visible = False
                    dgViewStudent.DataSource = Nothing
                    dgViewStudent.DataBind()
                    Dim loEnStud As New StudentEn
                    Dim loStud As New StudentBAL
                    'Dim listStud As List(Of StudentEn)

                    loEnStud.ProgramID = Request.QueryString("ProgramId")
                    loEnStud.Faculty = Request.QueryString("Faculty")
                    loEnStud.BatchCode = Request.QueryString("BatchCode")
                    loEnStud.MatricNo = Request.QueryString("MatricNo")
                    loEnStud.CurretSemesterYear = Request.QueryString("CurrSem")
                    loEnStud.Intake = Request.QueryString("Semester")
                    loEnStud.PostStatus = Request.QueryString("PostStatus")
                    If Request.QueryString("PostStatus") = 0 Then
                        listStud = loStud.GetListByProgram(loEnStud)
                        Session("StudentList") = listStud
                    Else
                        listStud = loStud.GetListByProgramForFee(loEnStud)

                    End If

                    'dgViewStudent.DataSource = listStud
                    'dgViewStudent.DataBind()
                    lblstucount.Text = listStud.Count
                    GlistStud = loStud.GetListGroupedByProgram(loEnStud)
                    tblMenu.Visible = False
                    ibtnAddFeeType.Visible = False
                    ibtnRemoveFee.Visible = False
                    ddlSem.Enabled = False
                    ddlBidang.Enabled = False
                    ddlEFrom.Enabled = False
                    ddlSession.Enabled = False
                    ChkClone.Visible = False
                    lblstucount.Visible = True
                    lblbatch.Visible = True
                    Label49.Visible = True
                    Label48.Visible = True
                    ddlProgram.Enabled = False
                    ddlStatus.Enabled = False
                    Dim batchTotal As Decimal
                    batchTotal = CalculateTotalBatch()
                    lblbatch.Text = batchTotal
                    lblSemester.Visible = False
                    txtSemster.Visible = False

                    dgViewStudent.DataSource = listStud
                    dgViewStudent.DataBind()

                Catch ex As Exception
                    Throw ex
                End Try
            End If

        End If
        If Request.QueryString("Formid") <> "FS" Then
            lblMsg.Visible = False
        End If

        If Not Session("eobj") Is Nothing Then
            addFeeType()
            'addFeeType_dgViewFee()
        End If
   
    End Sub



#Region "Methods"
    ''' <summary>
    ''' Method to Load Programs Dropdown
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub addSemester()
        Dim eobjF As New SemesterSetupEn
        Dim bsobj As New SemesterSetupBAL
        Dim list As New List(Of SemesterSetupEn)
        eobjF.Semester = ""
        eobjF.SemisterSetupCode = ""
        eobjF.Status = True
        ddlProgram.Items.Clear()
        ddlProgram.Items.Add(New ListItem("---Select---", "-1"))
        ddlProgram.DataTextField = "Semester"
        ddlProgram.DataValueField = "Semester"
        If Session("PageMode") = "Add" Then
            Try
                'list = bsobj.GetProgramInfoList(eobjF)
                list = bsobj.GetSemesterSetupList(eobjF)
            Catch ex As Exception
                LogError.Log("FeeStructure", "addSemester", ex.Message)
            End Try

        Else

            Try
                'list = bsobj.GetProgramInfoAll(eobjF)
                list = bsobj.GetListSemesterCode(eobjF)
            Catch ex As Exception
                LogError.Log("FeeStructure", "addSemester", ex.Message)
            End Try
        End If
        Session("Semester") = list
        ddlProgram.DataSource = list
        ddlProgram.DataBind()
    End Sub
    ''' <summary>
    ''' Method to Load Programs Dropdown
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub addProgramCode()
        'Dim eobjF As New ProgramInfoEn
        'Dim bsobj As New ProgramInfoBAL
        'Dim list As New List(Of ProgramInfoEn)
        'eobjF.ProgramType = ""
        'eobjF.ProgramCode = ""
        'eobjF.Program = ""
        'eobjF.ProgramBM = ""
        'eobjF.Status = True
        'eobjF.Faculty = ""
        'ddlProgram.Items.Clear()
        'ddlProgram.Items.Add(New ListItem("---Select---", "-1"))
        'ddlProgram.DataTextField = "CodeProgram"
        'ddlProgram.DataValueField = "ProgramCode"
        'If Session("PageMode") = "Add" Then
        '    Try
        '        list = bsobj.GetProgramInfoList(eobjF)
        '    Catch ex As Exception
        '        LogError.Log("FeeStructure", "addProgramCode", ex.Message)
        '    End Try

        'Else

        '    Try
        '        list = bsobj.GetProgramInfoAll(eobjF)
        '    Catch ex As Exception
        '        LogError.Log("FeeStructure", "addProgramCode", ex.Message)
        '    End Try
        'End If

        Dim eobjF As New ProgramInfoEn
        Dim bsobj As New ProgramInfoBAL
        Dim list As New List(Of ProgramInfoEn)
        eobjF.ProgramType = ""
        eobjF.ProgramCode = ""
        eobjF.Program = ""
        eobjF.ProgramBM = ""
        eobjF.Status = True
        eobjF.Faculty = ""
        ddlProgram.Items.Clear()
        ddlProgram.Items.Add(New ListItem("---Select---", "-1"))
        ddlProgram.DataTextField = "CodeProgram"
        ddlProgram.DataValueField = "ProgramCode"
        If Session("PageMode") = "Add" Then
            Try
                list = bsobj.GetProgramInfoList(eobjF)
            Catch ex As Exception
                LogError.Log("FeeStructure", "addProgramCode", ex.Message)
            End Try

        Else

            Try
                list = bsobj.GetProgramInfoAll(eobjF)
            Catch ex As Exception
                LogError.Log("FeeStructure", "addProgramCode", ex.Message)
            End Try
        End If

        Session("Programcode") = list
        ddlProgram.DataSource = list
        ddlProgram.DataBind()
    End Sub

    ''' Method to Load Bidang From Dropdown
    Private Sub addBidangCode()
        'GetBidangList()
        Dim eobjS As New BidangEn
        Dim bsobj As New BidangBAL
        Dim list As New List(Of BidangEn)
        eobjS.BidangCode = ""
        eobjS.Description = ""
        eobjS.Status = True
        ddlBidang.Items.Clear()
        ddlBidang.Items.Add(New ListItem("---Select---", "-1"))
        ddlBidang.DataTextField = "Description"
        ddlBidang.DataValueField = "BidangCode"
        If Session("PageMode") = "Add" Then
            Try
                list = bsobj.GetBidangList(eobjS)
            Catch ex As Exception
                LogError.Log("FeeStructure", "addBidangCode", ex.Message)
            End Try

        Else

            Try
                list = bsobj.GetBidangList(eobjS)
            Catch ex As Exception
                LogError.Log("FeeStructure", "addBidangCode", ex.Message)
            End Try
        End If

        Session("Bidang") = list
        ddlBidang.DataSource = list
        ddlBidang.DataBind()

    End Sub
    ''' <summary>
    ''' Method to Load Grid byFeeType
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub LoadGridByFeeType(Optional firstLoad As [Boolean] = False)
        Dim Mylist As New List(Of FeeStrDetailsEn)
        Dim Myamt As New List(Of FeeStrAmountEn)
        Dim eobjFSD As New FeeStrDetailsEn
        Dim eobjFSA As New FeeStrAmountEn
        Dim listamt As New List(Of FeeStrAmountEn)

        dgViewFee.Visible = True
        Dim getSelectedCtgyList As New List(Of FeeStructEn)

        If Not Session("AddFee") Is Nothing Then
            ListFeeStrDet = Session("AddFee")
        Else
            ListFeeStrDet = New List(Of FeeStructEn)
        End If

        ''Set the Tab Selection 
        If firstLoad = True Then
            If ListFeeStrDet.Any(Function(x) x.FeeType = "A") And ListFeeStrDet.Count > 0 Then
                ddlFeeCategory.SelectedValue = "A"
                TabSelection("A")
            Else
                If ListFeeStrDet.Any(Function(x) x.FeeType = "S") Then
                    ddlFeeCategory.SelectedValue = "S"
                    TabSelection("S")
                Else
                    If ListFeeStrDet.Any(Function(x) x.FeeType = "T") Then
                        ddlFeeCategory.SelectedValue = "T"
                        TabSelection("T")
                    Else
                        ddlFeeCategory.SelectedValue = "A"
                        TabSelection("A")
                    End If
                End If
            End If
        End If
        If ddlFeeCategory.SelectedValue = "T" Then
            ResetGrid("T")
        Else
            ResetGrid()
        End If


        If ddlFeeCategory.SelectedValue <> "T" Then
            pnlTution.Visible = False
            If ddlFeeCategory.SelectedValue = "S" Then
                pnlSemester.Visible = True
            Else
                pnlSemester.Visible = False
            End If

            'Created by Jessica - 26/02/2016
            If ListFeeStrDet.Count > 0 Then
                If ddlFeeCategory.SelectedValue <> "-1" Then
                    getSelectedCtgyList = ListFeeStrDet.Where(Function(x) x.FeeType = ddlFeeCategory.SelectedValue).OrderBy(Function(p) p.FeeFor).OrderBy(Function(q) q.FeeDetailSem).OrderBy(Function(y) y.FTCode).ToList()
                End If
                If getSelectedCtgyList.Count > 0 Then
                    dgViewFee.DataSource = getSelectedCtgyList
                    dgViewFee.DataBind()
                Else
                    dgViewFee.DataSource = Nothing
                    dgViewFee.DataBind()
                End If
            End If
        Else
            pnlTution.Visible = True
            pnlSemester.Visible = False

            dgViewFee.Visible = True
            'Created by Jessica - 26/02/2016
            If ListFeeStrDet.Count > 0 Then
                If ddlFeeCategory.SelectedValue <> "-1" Then
                    getSelectedCtgyList = ListFeeStrDet.Where(Function(x) x.FeeType = ddlFeeCategory.SelectedValue).OrderBy(Function(p) p.FeeFor).OrderBy(Function(q) q.FeeDetailSem).OrderBy(Function(y) y.FTCode).ToList()
                End If
                If getSelectedCtgyList.Count > 0 Then
                    dgViewFee.DataSource = getSelectedCtgyList
                    dgViewFee.DataBind()
                Else
                    dgViewFee.DataSource = Nothing
                    dgViewFee.DataBind()
                End If
            End If
        End If

    End Sub
    ''' <summary>
    ''' Method to Load Feetypes Grid
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub addFeeType()
        Dim eobjFt As FeeTypesEn
        Dim listFee As New List(Of FeeTypesEn)
        Dim efeechar As New FeeChargesEn
        If Not Session("AddFee") Is Nothing Then
            ListFeeStrDet = Session("AddFee")
        Else
            ListFeeStrDet = New List(Of FeeStructEn)
        End If

        listFee = Session("eobj")
        If listFee.Count <> 0 Then
            If ddlFeeCategory.SelectedValue = "A" Then
                For Each eobjFt In listFee
                    If Not ListFeeStrDet.Any(Function(x) x.FTCode = eobjFt.FeeTypeCode And x.FeeType = ddlFeeCategory.SelectedValue) Then
                        If Not ListFeeStrDet.Any(Function(x) x.FTCode = eobjFt.FeeTypeCode And (x.FeeType = "A" Or x.FeeType = "S")) Then
                            Dim eobjFS As New FeeStructEn
                            eobjFS.LocalAmount = eobjFt.LocalAmount
                            eobjFS.LocalGSTAmount = eobjFt.LocalGSTAmount
                            eobjFS.LocalTempAmount = eobjFS.LocalAmount - eobjFS.LocalGSTAmount
                            eobjFS.LocalCategory = eobjFt.LocalCategory
                            eobjFS.NonLocalAmount = eobjFt.NonLocalAmount
                            eobjFS.NonLocalGSTAmount = eobjFt.NonLocalGSTAmount
                            eobjFS.NonLocalTempAmount = eobjFS.NonLocalAmount - eobjFS.NonLocalGSTAmount
                            eobjFS.NonLocalCategory = eobjFt.NonLocalCategory
                            eobjFS.TaxId = eobjFt.TaxId
                            eobjFS.FTCode = eobjFt.FeeTypeCode
                            eobjFS.FeeStructureCode = txtFSCode.Text
                            eobjFS.Description = eobjFt.Description
                            eobjFS.Priority = eobjFt.Priority
                            'efeechar.FTCode = eobjFt.FeeTypeCode

                            'Try
                            '    listfeecharges = bofeechar.GetFeeCharges(efeechar)
                            'Catch ex As Exception
                            '    LogError.Log("FeeStructure", "addFeeType", ex.Message)
                            'End Try

                            eobjFS.FeeType = "A"
                            eobjFS.FeeFor = "0"
                            eobjFS.FeeDetailSem = 0
                            If ddlFeeCategory.SelectedValue = "S" Then
                                eobjFS.FeeType = "S"
                                ' eobjFSD.FeeFor = ddlFeeFor.SelectedValue
                                If ddlFeeFor.SelectedValue = "0" Then 'Annual
                                    eobjFS.FeeFor = "0"
                                    eobjFS.FeeDetailSem = 0
                                ElseIf ddlFeeFor.SelectedValue = "1" Then 'Semester
                                    'If ddlSemester.SelectedValue = "1" Then
                                    '    eobjFS.FeeFor = "1"
                                    '    eobjFS.FeeDetailSem = txtSemster.Text
                                    'Else
                                    '    eobjFS.FeeFor = "0"
                                    'End If
                                    eobjFS.FeeFor = "1"
                                    eobjFS.FeeDetailSem = txtSemster.Text
                                End If
                            End If
                            If ddlFeeCategory.SelectedValue = "T" Then
                                eobjFS.FeeType = "T"
                            End If
                            ListFeeStrDet.Add(eobjFS)
                            eobjFS = Nothing
                        Else
                            lblMsg.Text = "Selected Fee Code Already Added For Semester"
                            lblMsg.Visible = True
                        End If
                    Else
                        lblMsg.Text = "Selected Fee Code Already Added For Admission"
                        lblMsg.Visible = True

                    End If
                Next
            ElseIf ddlFeeCategory.SelectedValue = "S" Then
                Dim ExistingFTCode As String = ""
                For Each eobjFt In listFee
                    Dim FeeFor As String
                    If ddlFeeFor.SelectedValue = "-1" Then
                        FeeFor = "0"
                    Else
                        FeeFor = ddlFeeFor.SelectedValue
                    End If
                    Dim Sem As Integer
                    Sem = MaxGeneric.clsGeneric.NullToInteger(txtSemster.Text)
                    If ddlFeeFor.SelectedValue <> "1" Then
                        Sem = 0
                    End If
                    ''Avoid the data duplicate for all sem and individual sem - start 01/03/2016
                    If Not ListFeeStrDet.Any(Function(x) x.FTCode = eobjFt.FeeTypeCode And x.FeeType = ddlFeeCategory.SelectedValue) Then
                        ''Fee Code never exist then add.
                        If Not ListFeeStrDet.Any(Function(x) x.FTCode = eobjFt.FeeTypeCode And x.FeeType = ddlFeeCategory.SelectedValue And x.FeeFor = FeeFor And x.FeeDetailSem = Sem) Then
                            If Not ListFeeStrDet.Any(Function(x) x.FTCode = eobjFt.FeeTypeCode And (x.FeeType = "A" Or x.FeeType = "S")) Then
                                Dim eobjFS As New FeeStructEn
                                eobjFS.LocalAmount = eobjFt.LocalAmount
                                eobjFS.LocalGSTAmount = eobjFt.LocalGSTAmount
                                eobjFS.LocalTempAmount = eobjFS.LocalAmount - eobjFS.LocalGSTAmount
                                eobjFS.LocalCategory = eobjFt.LocalCategory
                                eobjFS.NonLocalAmount = eobjFt.NonLocalAmount
                                eobjFS.NonLocalGSTAmount = eobjFt.NonLocalGSTAmount
                                eobjFS.NonLocalTempAmount = eobjFS.NonLocalAmount - eobjFS.NonLocalGSTAmount
                                eobjFS.NonLocalCategory = eobjFt.NonLocalCategory
                                eobjFS.TaxId = eobjFt.TaxId
                                eobjFS.FTCode = eobjFt.FeeTypeCode
                                eobjFS.FeeStructureCode = txtFSCode.Text
                                eobjFS.Description = eobjFt.Description
                                eobjFS.Priority = eobjFt.Priority
                                'efeechar.FTCode = eobjFt.FeeTypeCode

                                'Try
                                '    listfeecharges = bofeechar.GetFeeCharges(efeechar)
                                'Catch ex As Exception
                                '    LogError.Log("FeeStructure", "addFeeType", ex.Message)
                                'End Try

                                eobjFS.FeeType = "A"
                                eobjFS.FeeFor = "0"
                                eobjFS.FeeDetailSem = 0
                                If ddlFeeCategory.SelectedValue = "S" Then
                                    eobjFS.FeeType = "S"
                                    ' eobjFSD.FeeFor = ddlFeeFor.SelectedValue
                                    If ddlFeeFor.SelectedValue = "0" Then 'Annual
                                        eobjFS.FeeFor = "0"
                                        eobjFS.FeeDetailSem = 0
                                    ElseIf ddlFeeFor.SelectedValue = "1" Then 'Semester
                                        'If ddlSemester.SelectedValue = "1" Then
                                        '    eobjFS.FeeFor = "1"
                                        '    eobjFS.FeeDetailSem = txtSemster.Text
                                        'Else
                                        '    eobjFS.FeeFor = "0"
                                        'End If
                                        eobjFS.FeeFor = "1"
                                        eobjFS.FeeDetailSem = txtSemster.Text
                                    Else
                                        eobjFS.FeeFor = "0"
                                        eobjFS.FeeDetailSem = 0
                                        ddlFeeFor.SelectedValue = "0"
                                    End If
                                Else
                                    eobjFS.FeeFor = String.Empty
                                    eobjFS.FeeDetailSem = 0
                                End If
                                If ddlFeeCategory.SelectedValue = "T" Then
                                    eobjFS.FeeType = "T"
                                End If
                                ListFeeStrDet.Add(eobjFS)
                                eobjFS = Nothing
                            Else
                                lblMsg.Visible = True
                                lblMsg.Text = "Selected Fee Code Already Added For Admission"
                            End If
                        End If

                    Else
                        ''Fee Code already exist
                        ''Check if add for all sem then need to remove all the same fee for individual
                        ''if add for individual, need to check already exist for all sem; if yes, prompt msg which is not added

                        If ddlFeeFor.SelectedValue = "0" Then
                            ''add fee for all sem
                            If Not ListFeeStrDet.Any(Function(x) x.FTCode = eobjFt.FeeTypeCode And x.FeeType = ddlFeeCategory.SelectedValue) Then
                                addfee(eobjFt)
                            Else
                                ''Prompt msg and delete the individual fee
                                ListFeeStrDet.RemoveAll(Function(x) x.FTCode = eobjFt.FeeTypeCode And x.FeeType = ddlFeeCategory.SelectedValue)
                                addfee(eobjFt)
                                lblMsg.Text = "Selected Fee Code Removed from Individual Semester"
                                lblMsg.Visible = True
                            End If
                        Else
                            ''add fee for individual sem
                            If Not ListFeeStrDet.Any(Function(x) x.FTCode = eobjFt.FeeTypeCode And x.FeeType = "S" And x.FeeFor = "0") Then
                                If Not ListFeeStrDet.Any(Function(x) x.FTCode = eobjFt.FeeTypeCode And x.FeeType = ddlFeeCategory.SelectedValue And x.FeeFor = FeeFor And x.FeeDetailSem = Sem) Then
                                    addfee(eobjFt)
                                End If
                            Else
                                lblMsg.Text = "Selected Fee Code Already Added For All Semester"
                                lblMsg.Visible = True
                            End If
                        End If

                    End If
                    ''Avoid the data duplicate for all sem and individual sem - end 01/03/2016

                Next
            ElseIf ddlFeeCategory.SelectedValue = "T" Then
                For Each eobjFt In listFee
                    ListFeeStrDet.RemoveAll(Function(x) x.FeeType = "T")
                    addfee(eobjFt)
                Next
            Else
                lblMsg.Text = "Please Select Tab To Add Fee"
                lblMsg.Visible = True
            End If
        End If

        Dim getSelectedFeeCtgy As New List(Of FeeStructEn)
        getSelectedFeeCtgy = ListFeeStrDet.Where(Function(x) x.FeeType = ddlFeeCategory.SelectedValue).OrderBy(Function(p) p.FeeFor).OrderBy(Function(q) q.FeeDetailSem).OrderBy(Function(y) y.FTCode).ToList()
        If getSelectedFeeCtgy.Count > 0 Then
            dgViewFee.DataSource = getSelectedFeeCtgy
            dgViewFee.DataBind()
        Else
            dgViewFee.DataSource = Nothing
            dgViewFee.DataBind()
        End If


        feeflag = "ADDFEE"
        Session("AddFee") = ListFeeStrDet
        LoadGridByFeeType()
        Session("eobj") = Nothing
    End Sub

    ''' <summary>
    ''' Method to Load Session Dropdown
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub LoadSession()
        Dim eSemSetup As New SemesterSetupEn
        Dim objBAL As New SemesterSetupBAL
        Dim listObj As New List(Of SemesterSetupEn)
        Try
            'listObj = objBAL.GetSessionList(txtSemister.Text)
            listObj = objBAL.GetListSemesterCur(eSemSetup)
        Catch ex As Exception
            LogError.Log("FeeStructure", "LoadSession", ex.Message)
        End Try

        ddlEFrom.Items.Clear()
        ddlEFrom.Items.Add(New ListItem("--Select--", "-1"))
        ddlEFrom.DataTextField = "SemisterCode2"
        ddlEFrom.DataValueField = "Semester"
        ddlEFrom.DataSource = listObj
        ddlEFrom.DataBind()

    End Sub

    Private Sub addfee(ByVal eobjFt As FeeTypesEn)
        Dim eobjFS As New FeeStructEn
        eobjFS.LocalAmount = eobjFt.LocalAmount
        eobjFS.LocalGSTAmount = eobjFt.LocalGSTAmount
        eobjFS.LocalTempAmount = eobjFS.LocalAmount - eobjFS.LocalGSTAmount
        eobjFS.LocalCategory = eobjFt.LocalCategory
        eobjFS.NonLocalAmount = eobjFt.NonLocalAmount
        eobjFS.NonLocalGSTAmount = eobjFt.NonLocalGSTAmount
        eobjFS.NonLocalTempAmount = eobjFS.NonLocalAmount - eobjFS.NonLocalGSTAmount
        eobjFS.NonLocalCategory = eobjFt.NonLocalCategory
        eobjFS.TaxId = eobjFt.TaxId
        eobjFS.FTCode = eobjFt.FeeTypeCode
        eobjFS.FeeStructureCode = txtFSCode.Text
        eobjFS.Description = eobjFt.Description
        eobjFS.Priority = eobjFt.Priority
        'efeechar.FTCode = eobjFt.FeeTypeCode

        'Try
        '    listfeecharges = bofeechar.GetFeeCharges(efeechar)
        'Catch ex As Exception
        '    LogError.Log("FeeStructure", "addFeeType", ex.Message)
        'End Try

        eobjFS.FeeType = "A"
        eobjFS.FeeFor = "0"
        eobjFS.FeeDetailSem = 0
        If ddlFeeCategory.SelectedValue = "S" Then
            eobjFS.FeeType = "S"
            ' eobjFSD.FeeFor = ddlFeeFor.SelectedValue
            If ddlFeeFor.SelectedValue = "0" Then 'Annual
                eobjFS.FeeFor = "0"
                eobjFS.FeeDetailSem = 0
            ElseIf ddlFeeFor.SelectedValue = "1" Then 'Semester
                'If ddlSemester.SelectedValue = "1" Then
                '    eobjFS.FeeFor = "1"
                '    eobjFS.FeeDetailSem = txtSemster.Text
                'Else
                '    eobjFS.FeeFor = "0"
                'End If
                eobjFS.FeeFor = "1"
                eobjFS.FeeDetailSem = txtSemster.Text
            Else
                eobjFS.FeeFor = "0"
                eobjFS.FeeDetailSem = 0
                ddlFeeFor.SelectedValue = "0"
            End If
        Else
            eobjFS.FeeFor = String.Empty
            eobjFS.FeeDetailSem = 0
        End If
        If ddlFeeCategory.SelectedValue = "T" Then
            eobjFS.FeeType = "T"
        End If
        ListFeeStrDet.Add(eobjFS)
    End Sub

    ' <summary>
    ' Method to Load FeeDetailsAmount
    ' </summary>
    ' <param name="code">FeeTypeCode is an Input.</param>
    ' <returns></returns>
    ' <remarks></remarks>
    'Private Function LoadFeeDetailAmount(ByVal code As String)
    '    'Dim bobj As New FeeTypesBAL
    '    'Dim obj As New FeeTypesEn
    '    'Dim eobjFeeStrDet As FeeStrAmountEn
    '    'Dim Lst As New List(Of FeeChargesEn)
    '    'Dim LstFeeTypeObject As List(Of FeeTypesEn)
    '    'Dim i As Integer = 0
    '    'Session("Amount") = Nothing
    '    'obj = New FeeTypesEn()
    '    'obj.FeeTypeCode = code
    '    'obj.Description = ""
    '    'obj.FeeType = ""
    '    'obj.Status = True

    '    'Try
    '    '    LstFeeTypeObject = bobj.GetFeeList(obj)
    '    '    obj = LstFeeTypeObject(0)
    '    'Catch ex As Exception
    '    '    LogError.Log("FeeStructure", "LoadFeeDetailAmount", ex.Message)
    '    'End Try

    '    'Lst = obj.ListFeeCharges
    '    'ListFeeStrAmt = New List(Of FeeStrAmountEn)
    '    'While i < Lst.Count
    '    '    eobjFeeStrDet = New FeeStrAmountEn
    '    '    eobjFeeStrDet.FTCode = Lst(i).FTCode
    '    '    eobjFeeStrDet.SCCode = Lst(i).SCCode
    '    '    eobjFeeStrDet.FeeAmount = Lst(i).FSAmount
    '    '    eobjFeeStrDet.SCDesc = Lst(i).SCDesc
    '    '    ListFeeStrAmt.Add(eobjFeeStrDet)
    '    '    eobjFeeStrDet = Nothing
    '    '    i = i + 1
    '    'End While
    '    'Session("Amount") = ListFeeStrAmt
    '    'dgViewType.DataSource = ListFeeStrAmt
    '    'dgViewType.DataBind()

    '    'ListFeeStrAmt = Nothing
    '    'Session("eobj") = Nothing
    'End Function
    ' <summary>
    ' Method to Load Values of FeeAmount TemplateControl
    ' </summary>
    ' <remarks></remarks>
    'Private Sub listvalues()
    '    Dim txtAmount As TextBox
    '    Dim eobjFSD As New FeeStrDetailsEn
    '    Dim eobjFS As New FeeStructEn
    '    Dim listFSD As New List(Of FeeStrDetailsEn)
    '    Dim ListFeeAmount As New List(Of FeeStrAmountEn)
    '    Dim dgItem1 As DataGridItem
    '    Dim objfeeAM As New FeeStrAmountEn
    '    Dim i As Integer = 0
    '    For Each dgItem1 In dgView.Items
    '        txtAmount = dgItem1.FindControl("txtFeeAmount")
    '        objfeeAM.SCCode = dgItem1.Cells(0).Text
    '        If txtAmount.Text = "" Then
    '            objfeeAM.FeeAmount = "0"
    '        Else
    '            objfeeAM.FeeAmount = txtAmount.Text
    '        End If
    '        ListFeeAmount.Add(objfeeAM)
    '        objfeeAM = Nothing
    '        txtAmount = Nothing
    '    Next
    '    eobjFSD.ListFeeAmount = ListFeeAmount
    '    listFSD.Add(eobjFSD)
    'End Sub
    ''' <summary>
    ''' Method to Save and Update FeeStructures
    ''' </summary>
    ''' <remarks></remarks>
    ''' 
    Private Sub onsave()
        Dim eobj As New FeeStructEn
        Dim bsobj As New FeeStructBAL
        Dim LstFeesd As New List(Of FeeStructEn)
        Dim lstfeeAm As New List(Of FeeStrAmountEn)
        Dim eob As New SemesterSetupEn
        Dim bob As New SemesterSetupBAL
        Dim RecAff As Integer
        lblMsg.Text = ""
        lblMsg.Visible = True

        'LoadFeeStructureDetails()
        LstFeesd = Session("AddFee")
        If Not Session("AddFee") Is Nothing Then
            eobj.FeeStructureCode = txtFSCode.Text
            'eobj.PGCode = txtPgCode.Text
            eobj.BidangCode = txtBidang.Text
            eobj.FeeCategory = ddlFeeCategory.SelectedValue

            'eob.Semester = ddlSem.SelectedValue
            'Intake Sem
            eob.SemisterSetupCode = ddlEFrom.SelectedValue
            'eob.Description = ddlSession.SelectedItem.ToString()
            'eob = bob.GetItem(eob)
            eob = bob.GetItemBySession(eob)
            eobj.STCode = eob.SemisterSetupCode
            'eobj.Semester = ddlSem.SelectedItem.ToString
            'Semester Code
            eobj.STCode = ddlEFrom.SelectedValue.ToString
            If ddlStatus.SelectedValue = 0 Then
                eobj.Status = False
            Else
                eobj.Status = True
            End If
            eobj.UpdatedUser = Session("User")
            eobj.UpdatedDtTm = Date.Now

            If ddlTution.SelectedValue = -1 Then
                txtTutPoint.Text = "0"
            End If
            If txtTutPoint.Text.Length <> 0 Then eobj.CrPoint = CDbl(txtTutPoint.Text)
            'If txtTutAmt.Text.Length <> 0 Then eobj.TutAmt = CDbl(txtTutAmt.Text)
            eobj.FeeBaseOn = ddlTution.SelectedValue
            'If Session("AddFee") Is Nothing Then
            'Else
            '    eobj.ListFeeStrDetails = Session("AddFee")
            'End If
            'eobj.TutAmt = 
            Dim lstFeeStrDetailsToSave As New List(Of FeeStrDetailsEn)
            eobj.ListFeeStrDetails = lstFeeStrDetailsToSave

            If LstFeesd.Count > 0 Then
                For Each obj In LstFeesd
                    Dim objFeeStrDetails As New FeeStrDetailsEn 'to store details for fee struct list
                    Dim lstFeeAmtToSave As New List(Of FeeStrAmountEn) 'to store the amount for local and nonlocal
                    Dim objFeeAmt As New FeeStrAmountEn
                    Dim FeeFor As String
                    If obj.FeeFor = Nothing Or obj.FeeFor = "" Then
                        FeeFor = "0"
                    Else
                        FeeFor = obj.FeeFor
                    End If
                    '----------------------------------------------------'
                    objFeeStrDetails.ListFeeAmount = lstFeeAmtToSave
                    '----------------------------------------------------'
                    objFeeStrDetails.FSCode = txtFSCode.Text
                    objFeeStrDetails.FTCode = obj.FTCode
                    objFeeStrDetails.Type = obj.FeeType
                    objFeeStrDetails.Priority = obj.Priority
                    objFeeStrDetails.FeeFor = FeeFor
                    objFeeStrDetails.Sem = obj.FeeDetailSem
                    objFeeStrDetails._TaxId = obj.TaxId
                    '----------------------------------------------------'
                    objFeeAmt = New FeeStrAmountEn
                    objFeeAmt.FSCode = txtFSCode.Text
                    objFeeAmt.FTCode = obj.FTCode
                    objFeeAmt.SCCode = obj.LocalCategory
                    objFeeAmt.Type = obj.FeeType
                    objFeeAmt.FeeAmount = obj.LocalAmount

                    If obj.FeeType = "T" Then
                        eobj.TutAmt = obj.LocalTempAmount
                    End If

                    'Added by Zoya @15/04/2016
                    'If objFeeAmt.FeeAmount = 0 Then
                    '    lblMsg.Visible = True
                    '    lblMsg.Text = "Amount Cannot be Zero"
                    '    Exit Sub
                    'End If
                    'End Added by Zoya @15/04/2016
                    objFeeAmt.GSTAmount = obj.LocalGSTAmount
                    objFeeAmt.FeeFor = FeeFor
                    objFeeAmt.FeeDetailSem = obj.FeeDetailSem
                    lstFeeAmtToSave.Add(objFeeAmt)
                    '----------------------------------------------------'
                    objFeeAmt = New FeeStrAmountEn
                    objFeeAmt.FSCode = txtFSCode.Text
                    objFeeAmt.FTCode = obj.FTCode
                    objFeeAmt.SCCode = obj.NonLocalCategory
                    objFeeAmt.Type = obj.FeeType
                    objFeeAmt.FeeAmount = obj.NonLocalAmount

                    If obj.FeeType = "T" Then
                        eobj.NonTutAmt = obj.NonLocalTempAmount
                    End If
                    'Added by Zoya @15/04/2016
                    'If objFeeAmt.FeeAmount = 0 Then
                    '    lblMsg.Visible = True
                    '    lblMsg.Text = "Amount Cannot be Zero"
                    '    Exit Sub
                    'End If
                    'End Added by Zoya @15/04/2016

                    objFeeAmt.GSTAmount = obj.NonLocalGSTAmount
                    objFeeAmt.FeeFor = FeeFor
                    objFeeAmt.FeeDetailSem = obj.FeeDetailSem
                    lstFeeAmtToSave.Add(objFeeAmt)
                    '----------------------------------------------------'
                    objFeeStrDetails.ListFeeAmount = lstFeeAmtToSave
                    lstFeeStrDetailsToSave.Add(objFeeStrDetails)
                Next
                eobj.ListFeeStrDetails = lstFeeStrDetailsToSave
            End If
            eobj.lstFeeStrWithAmt = LstFeesd
            If Session("PageMode") = "Add" Then
                Try
                    eobj.FeeStructureCode = ""
                    RecAff = bsobj.Insert(eobj)
                    ErrorDescription = "Record Saved Successfully "
                    lblMsg.Text = ErrorDescription
                    LoadListObjects()
                    If Session("PageMode") = "Edit" Then
                        ddlBidang.Enabled = False
                        ddlEFrom.Enabled = False
                    Else
                        ddlBidang.Enabled = True
                        ddlEFrom.Enabled = True
                    End If
                Catch ex As Exception
                    lblMsg.Text = ex.Message.ToString()
                    LogError.Log("FeeStructure", "onsave", ex.Message)
                End Try
            ElseIf Session("PageMode") = "Edit" Then
                Try
                    RecAff = bsobj.Update(eobj)

                    Dim ListObjAddFee As New List(Of FeeStructEn)
                    ListObjAddFee = Session("ListObj")
                    ListObjAddFee(CInt(txtRecNo.Text) - 1) = eobj
                    Session("ListObj") = ListObjAddFee
                    ErrorDescription = "Record Updated Successfully "
                    lblMsg.Text = ErrorDescription
                Catch ex As Exception
                    lblMsg.Text = ex.Message.ToString()
                    LogError.Log("FeeStructure", "onsave", ex.Message)

                End Try
            End If
        Else
            lblMsg.Text = "Enter At least One Fee type"
        End If
    End Sub

    ''' <summary>
    ''' Method to Delete FeeStuctures
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub onDelete()
        lblMsg.Visible = True
        If txtFSCode.Text <> "" Then
            If lblCount.Text = "" Then lblCount.Text = 0
            If lblCount.Text > 0 Then

                Dim bsobj As New FeeStructBAL
                Dim eobj As New FeeStructEn
                Dim RecAff As Integer
                lblMsg.Visible = True
                eobj.FeeStructureCode = txtFSCode.Text
                Try
                    RecAff = bsobj.Delete(eobj)
                    ListObjects = Session("ListObj")
                    ListObjects.RemoveAt(CInt(txtRecNo.Text) - 1)
                    lblCount.Text = lblCount.Text - 1
                    Session("ListObj") = ListObjects
                    lblMsg.Visible = True
                    ErrorDescription = "Record Deleted Successfully "
                    lblMsg.Text = ErrorDescription

                Catch ex As Exception
                    lblMsg.Text = ex.Message.ToString()
                    LogError.Log("FeeStructure", "onDelete", ex.Message)
                End Try
                txtFSCode.Text = ""
                txtPgCode.Text = ""
                ddlStatus.SelectedValue = "1"
                DFlag = "Delete"
                ddlStatus.SelectedValue = "1"
                ddlFeeFor.SelectedValue = "-1"
                ddlSemester.SelectedValue = "-1"
                ddlTution.SelectedValue = "-1"
                ddlSem.SelectedValue = "-1"
                ddlSession.SelectedValue = "-1"
                ddlEFrom.SelectedValue = "-1"
                txtTutPoint.Text = ""
                txtTutAmt.Text = ""
                dgViewFee.DataSource = Nothing
                dgViewFee.DataBind()
                LoadListObjects()
                pnlTution.Visible = False
                pnlSemester.Visible = False
            Else
                ErrorDescription = "Select a Record to Delete"
                lblMsg.Text = ErrorDescription
            End If

        Else
            ErrorDescription = "Select a Record to Delete"
            lblMsg.Text = ErrorDescription
        End If
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
            LogError.Log("FeeStructure", "LoadUserRights", ex.Message)
        End Try
        'Rights for Add

        If eobj.IsAdd = True Then
            'ibtnSave.Enabled = True
            onAdd()
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
            LogError.Log("FeeStructure", "Menuname", ex.Message)
        End Try
        lblMenuName.Text = eobj.MenuName
    End Sub
    ''' <summary>
    ''' Method to Load Fields in New Mode
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub onAdd()
        Session("PageMode") = "Add"
        'addProgramCode()
        'addBidangCode()
        ibtnSave.Enabled = True
        ibtnSave.ImageUrl = "images/save.png"
        ChkClone.Visible = False
        OnClearData()
        resetTabSelection()
    End Sub
    ''' <summary>
    ''' Method to Load Admission Tab Fields
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub OnClickAdmission()
        imgLeft1.ImageUrl = "images/b_white_left.png"
        imgRight1.ImageUrl = "images/b_white_right.png"
        btnAdmission.CssClass = "TabButtonClick"


        imgLeft2.ImageUrl = "images/b_orange_left.png"
        imgRight2.ImageUrl = "images/b_orange_right.png"
        btnSemester.CssClass = "TabButton"

        imgLeft3.ImageUrl = "images/b_orange_left.png"
        imgRight3.ImageUrl = "images/b_orange_right.png"
        btnTution.CssClass = "TabButton"

        ddlFeeCategory.SelectedValue = "A"
        ibtnAddFeeType.Attributes.Add("onclick", "new_window=window.open('StudentFeetype.aspx?Category=" & ddlFeeCategory.SelectedValue & "','Hanodale','width=600,height=500,resizable=0,center');new_window.focus();")
        LoadGridByFeeType(True)
        ibtnAddFeeType.Visible = True
        ibtnRemoveFee.Visible = True
    End Sub
    ''' <summary>
    ''' Method to get the List Of FeeStuctures
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub LoadListObjects()
        Dim obj As New FeeStructBAL
        Dim eobj As New FeeStructEn
        Dim recStu As Integer
        Dim sem As String
        Dim ses As String
        Dim eFS As New SemesterSetupEn
        Dim bFs As New SemesterSetupBAL

        If ddlStatus.SelectedValue = "-1" Then
            recStu = -1
        Else
            recStu = ddlStatus.SelectedValue
        End If

        If ddlSem.SelectedValue = "-1" Then
            sem = ""
        Else
            sem = ddlSem.SelectedValue

        End If
        If ddlSession.SelectedValue = "-1" Then
            ses = ""
        Else
            ses = ddlSession.SelectedValue

        End If

        If ddlEFrom.SelectedValue = "-1" Then
            sem = ""
        Else
            sem = ddlEFrom.SelectedValue
        End If

        If ddlBidang.SelectedValue = "-1" Then
            eobj.BidangCode = ""
        Else
            eobj.BidangCode = ddlBidang.SelectedValue
        End If

        eobj.semestersetup = New SemesterSetupEn

        eobj.FeeStructureCode = txtFSCode.Text
        eobj.PGCode = txtPgCode.Text
        eobj.semestersetup.SemisterSetupCode = sem
        eobj.semestersetup.Description = ses
        eobj.Status = recStu

        If Request.QueryString("Formid") = "FS" Then
            eFS.SemisterSetupCode = Request.QueryString("Semester")
            eFS = bFs.GetItemBySession(eFS)
            eobj.semestersetup.Semester = eFS.Semester
            eobj.semestersetup.Description = eFS.Description
            eobj.PGCode = Request.QueryString("ProgramId")
            eobj.BidangCode = Request.QueryString("BidangCode")
            eobj.STCode = Request.QueryString("Semester")
        End If
        Dim lstFeeAmt As New List(Of FeeStructEn)
        Try
            ListObjects = obj.GetFeeStructureDetailList(eobj)
        Catch ex As Exception
            LogError.Log("FeeStructure", "LoadListObjects", ex.Message)
        End Try
        Session("ListObj") = ListObjects
        lblCount.Text = ListObjects.Count.ToString()
        If ListObjects.Count <> 0 Then
            DisableRecordNavigator()
            txtRecNo.Text = "1"

            OnMoveFirst()
            If Session("EditFlag") = True Then
                Session("PageMode") = "Edit"
                txtFSCode.Enabled = False
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
            OnClearData()

            If DFlag = "Delete" Then
            Else
                lblMsg.Visible = True
                ErrorDescription = "Fee Structure did not Exist"
                lblMsg.Text = ErrorDescription
                DFlag = ""
            End If
        End If
    End Sub
    ''' <summary>
    ''' Method to Clear all the Field Values
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub OnClearData()
        txtFSCode.Enabled = True
        Session("ListObj") = Nothing
        DisableRecordNavigator()
        Session("AddFee") = Nothing
        txtFSCode.Text = ""
        txtPgCode.Text = ""
        ddlBidang.SelectedIndex = "-1"
        ddlEFrom.SelectedIndex = "-1"
        ddlProgram.SelectedIndex = "-1"
        txtSemster.Text = "1"
        txtTutPoint.Text = ""
        txtTutAmt.Text = ""
        ddlStatus.SelectedValue = "1"
        ddlFeeFor.SelectedValue = "-1"
        ddlSemester.SelectedValue = "-1"
        ddlTution.SelectedValue = "-1"
        ddlSem.SelectedIndex = "-1"
        Session("PageMode") = "Add"
        'ddlSession.Items.Clear()
        ddlSession.SelectedValue = "-1"
        txtTutPoint.Text = ""
        txtTutAmt.Text = ""
        ChkClone.Checked = False
        lblSemester.Visible = False
        txtSemster.Visible = False
        dgViewFee.DataSource = Nothing
        dgViewFee.DataBind()
        ddlFeeCategory.SelectedValue = "A"
        ibtnAddFeeType.Attributes.Add("onclick", "new_window=window.open('StudentFeetype.aspx?Category=" & ddlFeeCategory.SelectedValue & "','Hanodale','width=600,height=500,resizable=0,center');new_window.focus();")
        If Session("PageMode") = "Edit" Then
            ddlBidang.Enabled = False
            ddlEFrom.Enabled = False
        Else
            ddlBidang.Enabled = True
            ddlEFrom.Enabled = True
        End If
        pnlTution.Visible = False
        pnlSemester.Visible = False

    End Sub
    ''' <summary>
    ''' Method to Fill the Fields
    ''' </summary>
    ''' <param name="RecNo"></param>
    ''' <remarks></remarks>
    Private Sub FillData(ByVal recno As Integer)
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
                Dim obj As FeeStructEn
                Dim eob As New SemesterSetupEn
                Dim stcode As String
                Dim bob As New SemesterSetupBAL
                ListObjects = Session("ListObj")
                obj = ListObjects(recno)
                txtFSCode.Text = obj.FeeStructureCode
                txtPgCode.Text = obj.PGCode
                'addProgramCode()
                'ddlProgram.Items.Clear()
                'ddlProgram.Items.Add(New ListItem("---Select---", "-1"))
                'ddlProgram.DataSource = Session("Programcode")
                'ddlProgram.DataBind()
                'ddlProgram.SelectedValue = obj.PGCode
                stcode = obj.STCode

                txtBidang.Text = obj.BidangCode
                ddlBidang.SelectedValue = obj.BidangCode

                Try
                    eob = bob.GetSessionItem(stcode)
                Catch ex As Exception
                    LogError.Log("FeeStructure", "Filldata", ex.Message)
                End Try
                'ddlSem.SelectedValue = eob.Semester
                'ddlEFrom.SelectedValue = eob.SemisterCode2

                txtSemister.Text = eob.Semester

                txtEffSem.Text = eob.SemisterSetupCode
                ddlEFrom.SelectedValue = eob.SemisterSetupCode
                'LoadSession()
                'ddlSession.SelectedValue = eob.Description
                ddlFeeCategory.SelectedValue = obj.FeeCategory
                txtTutPoint.Text = obj.CrPoint.ToString
                txtTutAmt.Text = obj.TutAmt.ToString
                ''Edited by Jessica 02/03/2016 - start
                If obj.FeeBaseOn <> "-1" Then
                    ddlTution.SelectedValue = obj.FeeBaseOn
                    'lblPoint.Visible = True
                    'txtTutPoint.Visible = True
                Else
                    ddlTution.SelectedValue = "-1"
                    'lblPoint.Visible = True
                    'txtTutPoint.Visible = True
                End If
                ''Edited by Jessica  02/03/2016 - start
                If obj.Status = True Then
                    ddlStatus.SelectedValue = 1
                Else
                    ddlStatus.SelectedValue = 0
                End If

                ''Created by Jessica 24/02/2016
                Dim lstFeeAmt As New List(Of FeeStructEn)
                lstFeeAmt = Session("ListObj")
                Dim newObj As New FeeStructEn
                newObj = lstFeeAmt(recno)
                Dim getSelectedCtgy As New List(Of FeeStructEn)
                getSelectedCtgy = newObj.lstFeeStrWithAmt.Where(Function(x) x.FeeType = ddlFeeCategory.SelectedValue).OrderBy(Function(p) p.FeeFor).OrderBy(Function(q) q.FeeDetailSem).OrderBy(Function(y) y.FTCode).ToList()
                dgViewFee.DataSource = getSelectedCtgy
                dgViewFee.DataBind()
                Session("AddFee") = Nothing
                Session("AddFee") = newObj.lstFeeStrWithAmt

                OnClickAdmission()
            End If
        End If
        ChkClone.Visible = True
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
    ''' Method to Load Grid Values in a Session
    ''' </summary>
    ''' <remarks></remarks>
    '''  
    Dim listfee As New List(Of FeeStrAmountEn)

#End Region

    Protected Sub ibtnSave_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ibtnSave.Click
        If ddlBidang.SelectedValue = "-1" Then
            lblMsg.Text = "Please Select Bidang"
            lblMsg.Visible = True
            Exit Sub
        End If
        If ddlEFrom.SelectedValue = "-1" Then
            lblMsg.Text = "Please Select Effective From"
            lblMsg.Visible = True
            Exit Sub
        End If
        Dim LstFeesd As New List(Of FeeStructEn)
        If Not Session("AddFee") Is Nothing Then
            LstFeesd = Session("AddFee")
        End If

        If LstFeesd.Count = 0 Then
            lblMsg.Text = "Please Add At Least One Fee"
            lblMsg.Visible = True
            Exit Sub
        End If
        If ddlTution.SelectedValue <> "-1" Then
            Dim getTutFee As Boolean = LstFeesd.Any(Function(x) x.FeeType = "T")
            If getTutFee = False Then
                lblMsg.Text = "Please Select Tuition Fee"
                lblMsg.Visible = True
                Exit Sub
            End If
        End If
        onsave()
    End Sub

    'Protected Sub dgView_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgView.SelectedIndexChanged
    '    Dim eobj As New FeeStrDetailsEn
    '    Dim i As Integer = 0
    '    ListFeeStrDet = Session("AddFee")
    '    If dgView.SelectedIndex <> -1 Then
    '        While i < ListFeeStrDet.Count
    '            If ListFeeStrDet(i).FTCode = dgView.Items(dgView.SelectedIndex).Cells(5).Text Then
    '                eobj = ListFeeStrDet(i)
    '                Session("TaxId") = eobj._TaxId
    '                Session("FTCode") = eobj.FTCode
    '                dgViewType.DataSource = eobj.ListFeeAmount
    '                dgViewType.DataBind()
    '            End If
    '            i = i + 1
    '        End While

    '        Dim poststs As String = Request.QueryString("PostStatus")
    '        Dim Mn As String = Request.QueryString("Menuid")

    '        If Mn <> 11 And poststs = 0 Then
    '            'Declare instance 
    '            Dim loEnStud As New StudentEn
    '            Dim loStud As New HTS.SAS.DataAccessObjects.StudentDAL   'StudentDAL 'StudentBAL
    '            Dim listStud As List(Of StudentEn)

    '            loEnStud.ProgramID = Request.QueryString("ProgramId")
    '            loEnStud.Faculty = Request.QueryString("Faculty")
    '            loEnStud.BatchCode = Request.QueryString("BatchCode")
    '            loEnStud.MatricNo = Request.QueryString("MatricNo")
    '            loEnStud.CurretSemesterYear = Request.QueryString("Semester")
    '            loEnStud.PostStatus = Request.QueryString("PostStatus")
    '            listStud = loStud.GetListByProgramFeeAmount(loEnStud)

    '            '----------------------

    '            ' While i < dgViewType.RowCount 

    '            Dim ind As Integer = 0
    '            Dim BtAmt As Double = 0
    '            'For ind = 0 To listStud.Count

    '            For Each dataGridItem As DataGridItem In dgViewType.Items
    '                If ind < listStud.Count Then
    '                    Dim ltr As TextBox = DirectCast(dataGridItem.FindControl("txtTotFeeAmount"), TextBox)
    '                    Dim ltr2 As String = dataGridItem.Cells(0).Text
    '                    Dim strName As String = listStud(ind).SASI_Add1
    '                    Dim strName2 As Integer = listStud(ind).SASI_Add2
    '                    'DirectCast(dataGridItem.FindControl("SCCode"), Literal)

    '                    If ltr2 = strName Then
    '                        BtAmt = BtAmt + (ltr.Text * strName2)
    '                    End If

    '                End If
    '                ind = ind + 1
    '            Next
    '            ' Next
    '            lblbatch.Visible = True
    '            'lblbatch.Text = BtAmt
    '            '-------------------------

    '        Else
    '            'listStud = loStud.GetListByProgramForFee(loEnStud)

    '        End If

    '    End If

    'End Sub


    Protected Sub ibtnRemoveFee_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ibtnRemoveFee.Click

        ListFeeStrDet = Session("AddFee")
        Dim K As Integer = 0
        If Not ListFeeStrDet Is Nothing Then
            If dgViewFee.SelectedIndex <> -1 Then
                Try
                    If ddlFeeCategory.SelectedValue <> "S" Then
                        If ListFeeStrDet.Any(Function(x) x.FTCode = dgViewFee.Items(dgViewFee.SelectedIndex).Cells(dgViewFeeCell.FTCode).Text And x.FeeType = ddlFeeCategory.SelectedValue) Then
                            ListFeeStrDet.Remove(ListFeeStrDet.Where(Function(x) x.FTCode = dgViewFee.Items(dgViewFee.SelectedIndex).Cells(dgViewFeeCell.FTCode).Text And x.FeeType = ddlFeeCategory.SelectedValue).FirstOrDefault())
                        End If
                    Else
                        Dim feeFor As String
                        If dgViewFee.Items(dgViewFee.SelectedIndex).Cells(dgViewFeeCell.FeeFor).Text = "All Semester" Then
                            feeFor = "0"
                        ElseIf dgViewFee.Items(dgViewFee.SelectedIndex).Cells(dgViewFeeCell.FeeFor).Text = "Semester" Then
                            feeFor = "1"
                        ElseIf dgViewFee.Items(dgViewFee.SelectedIndex).Cells(dgViewFeeCell.FeeFor).Text = "&nbsp;" Then
                            feeFor = String.Empty
                        Else
                            feeFor = dgViewFee.Items(dgViewFee.SelectedIndex).Cells(dgViewFeeCell.FeeFor).Text
                        End If
                        If ListFeeStrDet.Any(Function(x) x.FTCode = dgViewFee.Items(dgViewFee.SelectedIndex).Cells(dgViewFeeCell.FTCode).Text And x.FeeType = ddlFeeCategory.SelectedValue And
                                                 x.FeeFor = feeFor And x.FeeDetailSem = dgViewFee.Items(dgViewFee.SelectedIndex).Cells(dgViewFeeCell.FeeDetailSem).Text) Then
                            ListFeeStrDet.Remove(ListFeeStrDet.Where(Function(x) x.FTCode = dgViewFee.Items(dgViewFee.SelectedIndex).Cells(dgViewFeeCell.FTCode).Text And x.FeeType = ddlFeeCategory.SelectedValue And
                                                 x.FeeFor = feeFor And x.FeeDetailSem = dgViewFee.Items(dgViewFee.SelectedIndex).Cells(dgViewFeeCell.FeeDetailSem).Text).FirstOrDefault())
                        End If
                    End If
                Catch ex As Exception
                    lblMsg.Visible = True
                    ErrorDescription = "Select a Fee code from the Fee list"
                    lblMsg.Text = ErrorDescription
                End Try

                Session("AddFee") = ListFeeStrDet
                Dim getSelectedFeeCtgy As New List(Of FeeStructEn)
                getSelectedFeeCtgy = ListFeeStrDet.Where(Function(x) x.FeeType = ddlFeeCategory.SelectedValue).OrderBy(Function(y) y.FTCode).ToList()
                dgViewFee.SelectedIndex = -1
                If getSelectedFeeCtgy.Count > 0 Then
                    dgViewFee.SelectedIndex = 0
                    dgViewFee.DataSource = getSelectedFeeCtgy
                    dgViewFee.DataBind()
                Else
                    dgViewFee.DataSource = Nothing
                    dgViewFee.DataBind()
                End If


            Else
                lblMsg.Visible = True
                ErrorDescription = "Select a Fee code from the Fee list"
                lblMsg.Text = ErrorDescription
            End If
        End If
        LoadGridByFeeType()
    End Sub
    Protected Sub ddlFeeCategory_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlFeeCategory.SelectedIndexChanged
        LoadGridByFeeType()
    End Sub

    Protected Sub txtRecNo_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtRecNo.TextChanged
        If Trim(txtRecNo.Text).Length = 0 Then
            txtRecNo.Text = 0
            If lblCount.Text <> Nothing Then
                If CInt(txtRecNo.Text) > CInt(lblCount.Text) Then
                    txtRecNo.Text = lblCount.Text
                End If
                FillData(CInt(txtRecNo.Text) - 1)
            Else
                txtRecNo.Text = ""
            End If
        Else
            If lblCount.Text <> Nothing Then
                If CInt(txtRecNo.Text) > CInt(lblCount.Text) Then
                    txtRecNo.Text = lblCount.Text
                End If
                FillData(CInt(txtRecNo.Text) - 1)
            Else
                txtRecNo.Text = ""
            End If
        End If
    End Sub

    Protected Sub ddlSemester_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlSemester.SelectedIndexChanged
        If ddlSemester.SelectedValue = "1" Then
            txtSemster.Visible = True
        Else
            txtSemster.Visible = False
        End If
    End Sub

    'Private Function CalculateTotalBatch() As Decimal
    '    Dim eobj As New FeeStrDetailsEn
    '    Dim eFeeStruct As New FeeStructEn
    '    Dim obj As New SemesterSetupBAL()
    '    Dim loStud As New StudentBAL
    '    Dim i As Integer = 0
    '    Dim j As Integer = 0, k As Integer = 0
    '    Dim amt As Decimal = 0
    '    Dim Wamt As Decimal = 0
    '    Dim Bamt As Decimal = 0
    '    Dim WPamt As Decimal = 0
    '    Dim BWamt As Decimal = 0
    '    Dim batch As String, progId As String
    '    Dim AdminAmtW As Decimal = 0, AdminAmtBW As Decimal = 0, TuitionAmt As Decimal = 0
    '    Dim SemAmtW As Decimal = 0, SemAmtBW As Decimal = 0, CurrSemAmtW As Decimal = 0, CurrSemAmtBW As Decimal = 0
    '    Dim HostelFee As Decimal = 0, KokoFee As Decimal = 0, AmtStd As Decimal = 0

    '    Dim cnt As Integer = 0
    '    Dim eStudentFee As New StudentEn
    '    Dim loHostelFee As New List(Of HostelStrAmountEn)

    '    'modified by Hafiz @ 10/6/2016
    '    batch = Request.QueryString("BatchCode")
    '    progId = Request.QueryString("ProgramId")

    '    If Not String.IsNullOrEmpty(batch) AndAlso Not String.IsNullOrEmpty(progId) Then
    '        amt = obj.FetchTotalBatchAmount(batch, progId)
    '    Else
    '        Session("StudentFee") = Nothing
    '        ListFeeStrDet = Session("AddFee")
    '        PlistStud = Session("StudentList")
    '        If ListFeeStrDet IsNot Nothing Then

    '            'GetFeeDetails based on Fee Type (A/S/T)
    '            While i < ListFeeStrDet.Count
    '                eFeeStruct = ListFeeStrDet(i)

    '                If eFeeStruct.FeeType = "A" Then
    '                    'Get Admission Fee
    '                    If eFeeStruct.LocalCategory = "W" Or eFeeStruct.LocalCategory = "Local" Then
    '                        AdminAmtW += Convert.ToDecimal(eFeeStruct.LocalAmount)
    '                    End If
    '                    If eFeeStruct.NonLocalCategory = "BW" Or eFeeStruct.NonLocalCategory = "International" Then
    '                        AdminAmtBW += Convert.ToDecimal(eFeeStruct.NonLocalAmount)
    '                    End If

    '                ElseIf eFeeStruct.FeeType = "S" Then
    '                    'Get Semester Fee for all
    '                    If eFeeStruct.FeeDetailSem = 0 Then
    '                        If eFeeStruct.LocalCategory = "W" Or eFeeStruct.LocalCategory = "Local" Then
    '                            SemAmtW += Convert.ToDecimal(eFeeStruct.LocalAmount)
    '                        End If
    '                        If eFeeStruct.NonLocalCategory = "BW" Or eFeeStruct.NonLocalCategory = "International" Then
    '                            SemAmtBW += Convert.ToDecimal(eFeeStruct.NonLocalAmount)
    '                        End If
    '                    End If

    '                End If

    '                i = i + 1
    '            End While

    '            i = 0
    '            j = 0

    '            'If GlistStud.Count > 0 Then

    '            'While i < GlistStud.Count

    '            While j < PlistStud.Count

    '                'If GlistStud(i).FeeCat = "W" Or GlistStud(i).FeeCat = "Local" Then
    '                If PlistStud(j).CategoryCode = "W" Or PlistStud(j).CategoryCode = "Local" Then

    '                    'End If
    '                    Dim CurrentSem As Integer = PlistStud(j).CurrentSemester
    '                    Wamt = 0

    '                    While cnt < ListFeeStrDet.Count

    '                        If ListFeeStrDet(cnt).FeeType = "S" Then
    '                            If ListFeeStrDet(cnt).FeeDetailSem = CurrentSem Then
    '                                If ListFeeStrDet(cnt).LocalCategory = "W" Or ListFeeStrDet(cnt).LocalCategory = "Local" Then
    '                                    CurrSemAmtW += ListFeeStrDet(cnt).LocalAmount
    '                                End If
    '                            End If
    '                        End If

    '                        cnt = cnt + 1
    '                    End While

    '                    cnt = 0

    '                    'Get Tuition Fee
    '                    TuitionAmt = _StudentDAL.GetTuitionFee(PlistStud(j).Intake, PlistStud(j).ProgramID, PlistStud(j).SASI_CrditHrs)
    '                    'Get Hostel Fee
    '                    HostelFee = _StudentDAL.GetStudentHostelFee(PlistStud(j).MatricNo)
    '                    'Get Koko Fee
    '                    KokoFee = _StudentDAL.GetStudentKokoFee(PlistStud(j).MatricNo)

    '                    If CurrentSem = 1 Then
    '                        Wamt = AdminAmtW + SemAmtW + CurrSemAmtW + TuitionAmt + HostelFee + KokoFee
    '                    End If

    '                    If CurrentSem > 1 Then
    '                        Wamt = SemAmtW + CurrSemAmtW + TuitionAmt + HostelFee + KokoFee
    '                    End If

    '                    'Add total fee for each student
    '                    PlistStud(j).LocalAmount = Wamt
    '                    PlistStud(j).TransactionAmount = PlistStud(j).LocalAmount

    '                    CurrSemAmtW = 0
    '                    TuitionAmt = 0
    '                    HostelFee = 0
    '                    KokoFee = 0

    '                    'Get total amount
    '                    amt += Wamt

    '                End If

    '                'If GlistStud(i).FeeCat = "BW" Or GlistStud(i).FeeCat = "International" Then
    '                If PlistStud(j).CategoryCode = "BW" Or PlistStud(j).CategoryCode = "International" Then

    '                    Dim CurrentSemBW As Integer = PlistStud(j).CurrentSemester

    '                    While cnt < ListFeeStrDet.Count

    '                        If ListFeeStrDet(cnt).FeeType = "S" Then
    '                            If ListFeeStrDet(cnt).FeeDetailSem = CurrentSemBW Then
    '                                If ListFeeStrDet(cnt).NonLocalCategory = "BW" Or ListFeeStrDet(cnt).NonLocalCategory = "International" Then
    '                                    CurrSemAmtBW += ListFeeStrDet(cnt).NonLocalAmount
    '                                End If
    '                            End If
    '                        End If

    '                        cnt = cnt + 1
    '                    End While

    '                    cnt = 0

    '                    'Get Tuition Fee
    '                    TuitionAmt = _StudentDAL.GetTuitionFee(PlistStud(j).Intake, PlistStud(j).ProgramID, PlistStud(j).SASI_CrditHrs)
    '                    'Get Hostel Fee
    '                    HostelFee = _StudentDAL.GetStudentHostelFee(PlistStud(j).MatricNo)
    '                    'Get Koko Fee
    '                    KokoFee = _StudentDAL.GetStudentKokoFee(PlistStud(j).MatricNo)

    '                    If CurrentSemBW = 1 Then
    '                        Bamt = AdminAmtBW + SemAmtBW + CurrSemAmtBW + TuitionAmt + HostelFee + KokoFee
    '                    End If

    '                    If CurrentSemBW > 1 Then
    '                        Bamt = SemAmtBW + CurrSemAmtBW + TuitionAmt + HostelFee + KokoFee
    '                    End If

    '                    'Add total fee for each student
    '                    'PlistStud(j).LocalAmount = Bamt
    '                    'PlistStud(j).TransactionAmount = PlistStud(j).LocalAmount
    '                    PlistStud(j).NonLocalAmount = Bamt
    '                    PlistStud(j).TransactionAmount = PlistStud(j).NonLocalAmount

    '                    CurrSemAmtBW = 0
    '                    TuitionAmt = 0
    '                    HostelFee = 0
    '                    KokoFee = 0

    '                    'Get total amount
    '                    amt += Bamt

    '                End If

    '                j = j + 1
    '            End While

    '            '    i = i + 1
    '            'End While
    '            'End If
    '        Else
    '            lblMsg.Visible = True
    '            ErrorDescription = "Fee Structure did not Exist"
    '            lblMsg.Text = ErrorDescription

    '        End If

    '        Session("StudentFee") = PlistStud
    '        listStud = Session("StudentFee")

    '    End If

    '    Return amt

    'End Function

    Private Function CalculateTotalBatch() As Decimal
        Dim eobj As New FeeStrDetailsEn
        Dim eFeeStruct As New FeeStructEn
        Dim obj As New SemesterSetupBAL()
        Dim loStud As New StudentBAL
        Dim i As Integer = 0
        Dim j As Integer = 0, k As Integer = 0
        Dim amt As Decimal = 0
        Dim Wamt As Decimal = 0
        Dim Bamt As Decimal = 0
        Dim WPamt As Decimal = 0
        Dim BWamt As Decimal = 0
        Dim batch As String, progId As String
        Dim AdminAmtW As Decimal = 0, AdminAmtBW As Decimal = 0, TuitionAmt As Decimal = 0
        Dim SemAmtW As Decimal = 0, SemAmtBW As Decimal = 0, CurrSemAmtW As Decimal = 0, CurrSemAmtBW As Decimal = 0
        Dim HostelFee As Decimal = 0, KokoFee As Decimal = 0, AmtStd As Decimal = 0

        Dim cnt As Integer = 0
        Dim eStudentFee As New StudentEn
        Dim loHostelFee As New List(Of HostelStrAmountEn)

        'modified by Hafiz @ 10/6/2016
        batch = Request.QueryString("BatchCode")
        progId = Request.QueryString("ProgramId")

        If Not String.IsNullOrEmpty(batch) AndAlso Not String.IsNullOrEmpty(progId) Then
            amt = MaxGeneric.clsGeneric.NullToDecimal(obj.FetchTotalBatchAmount(batch, progId))
        Else
            Session("StudentFee") = Nothing
            ListFeeStrDet = Session("AddFee")
            PlistStud = Session("StudentList")
            If ListFeeStrDet IsNot Nothing Then

                'GetFeeDetails based on Fee Type (A/S/T)
                While i < ListFeeStrDet.Count
                    eFeeStruct = ListFeeStrDet(i)

                    If eFeeStruct.FeeType = "A" Then
                        'Get Admission Fee
                        If eFeeStruct.LocalCategory = "W" Or eFeeStruct.LocalCategory = "Local" Then
                            AdminAmtW += Convert.ToDecimal(eFeeStruct.LocalAmount)
                        End If
                        If eFeeStruct.NonLocalCategory = "BW" Or eFeeStruct.NonLocalCategory = "International" Then
                            AdminAmtBW += Convert.ToDecimal(eFeeStruct.NonLocalAmount)
                        End If

                    ElseIf eFeeStruct.FeeType = "S" Then
                        'Get Semester Fee for all
                        If eFeeStruct.FeeDetailSem = 0 Then
                            If eFeeStruct.LocalCategory = "W" Or eFeeStruct.LocalCategory = "Local" Then
                                SemAmtW += Convert.ToDecimal(eFeeStruct.LocalAmount)
                            End If
                            If eFeeStruct.NonLocalCategory = "BW" Or eFeeStruct.NonLocalCategory = "International" Then
                                SemAmtBW += Convert.ToDecimal(eFeeStruct.NonLocalAmount)
                            End If
                        End If

                    End If

                    i = i + 1
                End While

                i = 0
                j = 0

                'If GlistStud.Count > 0 Then

                'While i < GlistStud.Count

                While j < PlistStud.Count

                    'If GlistStud(i).FeeCat = "W" Or GlistStud(i).FeeCat = "Local" Then
                    If PlistStud(j).CategoryCode = "W" Or PlistStud(j).CategoryCode = "Local" Then

                        'End If
                        Dim CurrentSem As Integer = PlistStud(j).CurrentSemester
                        Wamt = 0

                        While cnt < ListFeeStrDet.Count

                            If ListFeeStrDet(cnt).FeeType = "S" Then
                                If ListFeeStrDet(cnt).FeeDetailSem = CurrentSem Then
                                    If ListFeeStrDet(cnt).LocalCategory = "W" Or ListFeeStrDet(cnt).LocalCategory = "Local" Then
                                        CurrSemAmtW += ListFeeStrDet(cnt).LocalAmount
                                    End If
                                End If
                            End If

                            cnt = cnt + 1
                        End While

                        cnt = 0

                        'Get Tuition Fee
                        TuitionAmt = _StudentDAL.GetTuitionFee(PlistStud(j).Intake, PlistStud(j).ProgramID, PlistStud(j).SASI_CrditHrs, PlistStud(j).MatricNo)
                        'Get Hostel Fee
                        HostelFee = _StudentDAL.GetStudentHostelFee(PlistStud(j).MatricNo)
                        'Get Koko Fee
                        KokoFee = _StudentDAL.GetStudentKokoFee(PlistStud(j).MatricNo)

                        If CurrentSem = 1 Then
                            Wamt = AdminAmtW + SemAmtW + CurrSemAmtW + TuitionAmt + HostelFee + KokoFee
                        End If

                        If CurrentSem > 1 Then
                            Wamt = SemAmtW + CurrSemAmtW + TuitionAmt + HostelFee + KokoFee
                        End If

                        'Add total fee for each student
                        PlistStud(j).LocalAmount = Wamt
                        PlistStud(j).TransactionAmount = PlistStud(j).LocalAmount

                        CurrSemAmtW = 0
                        TuitionAmt = 0
                        HostelFee = 0
                        KokoFee = 0

                        'Get total amount
                        amt += Wamt

                    End If

                    'If GlistStud(i).FeeCat = "BW" Or GlistStud(i).FeeCat = "International" Then
                    If PlistStud(j).CategoryCode = "BW" Or PlistStud(j).CategoryCode = "International" Then

                        Dim CurrentSemBW As Integer = PlistStud(j).CurrentSemester

                        While cnt < ListFeeStrDet.Count

                            If ListFeeStrDet(cnt).FeeType = "S" Then
                                If ListFeeStrDet(cnt).FeeDetailSem = CurrentSemBW Then
                                    If ListFeeStrDet(cnt).NonLocalCategory = "BW" Or ListFeeStrDet(cnt).NonLocalCategory = "International" Then
                                        CurrSemAmtBW += ListFeeStrDet(cnt).NonLocalAmount
                                    End If
                                End If
                            End If

                            cnt = cnt + 1
                        End While

                        cnt = 0

                        'Get Tuition Fee
                        TuitionAmt = _StudentDAL.GetTuitionFee(PlistStud(j).Intake, PlistStud(j).ProgramID, PlistStud(j).SASI_CrditHrs, PlistStud(j).MatricNo)
                        'Get Hostel Fee
                        HostelFee = _StudentDAL.GetStudentHostelFee(PlistStud(j).MatricNo)
                        'Get Koko Fee
                        KokoFee = _StudentDAL.GetStudentKokoFee(PlistStud(j).MatricNo)

                        If CurrentSemBW = 1 Then
                            Bamt = AdminAmtBW + SemAmtBW + CurrSemAmtBW + TuitionAmt + HostelFee + KokoFee
                        End If

                        If CurrentSemBW > 1 Then
                            Bamt = SemAmtBW + CurrSemAmtBW + TuitionAmt + HostelFee + KokoFee
                        End If

                        'Add total fee for each student
                        'PlistStud(j).LocalAmount = Bamt
                        'PlistStud(j).TransactionAmount = PlistStud(j).LocalAmount
                        PlistStud(j).NonLocalAmount = Bamt
                        PlistStud(j).TransactionAmount = PlistStud(j).NonLocalAmount

                        CurrSemAmtBW = 0
                        TuitionAmt = 0
                        HostelFee = 0
                        KokoFee = 0

                        'Get total amount
                        amt += Bamt

                    End If

                    j = j + 1
                End While

                '    i = i + 1
                'End While
                'End If
            Else
                lblMsg.Visible = True
                ErrorDescription = "Fee Structure did not Exist"
                lblMsg.Text = ErrorDescription

            End If

            Session("StudentFee") = PlistStud
            listStud = Session("StudentFee")

        End If

        Return amt

    End Function

    Protected Sub ibtnView_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ibtnView.Click
        If lblCount.Text <> "" Then
            If CInt(lblCount.Text) > 0 Then
                OnClearData()
                If ibtnNew.Enabled = False Then
                    ibtnSave.Enabled = False
                    ibtnSave.ImageUrl = "images/gsave.png"
                    ibtnSave.ToolTip = "Access Denied"
                End If
            Else
                Session("PageMode") = "Edit"
                'addProgramCode()
                LoadListObjects()
            End If
        Else
            Session("PageMode") = "Edit"
            'addProgramCode()
            LoadListObjects()
        End If
        If lblCount.Text.Length = 0 Then
            Session("PageMode") = "Add"
        End If
        If Session("PageMode") = "Edit" Then
            ddlBidang.Enabled = False
            ddlEFrom.Enabled = False
        Else
            ddlBidang.Enabled = True
            ddlEFrom.Enabled = True
        End If
    End Sub

    Protected Sub ibtnFirst_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ibtnFirst.Click
        OnMoveFirst()
    End Sub

    Protected Sub ibtnNext_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ibtnNext.Click
        OnMoveNext()
    End Sub

    Protected Sub ibtnPrevs_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ibtnPrevs.Click
        OnMovePrevious()
    End Sub

    Protected Sub ibtnLast_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ibtnLast.Click
        OnMoveLast()
    End Sub

    Protected Sub ibtnDelete_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ibtnDelete.Click
        onDelete()
    End Sub

    Protected Sub btnAdmission_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        OnClickAdmission()
        If Request.QueryString("Formid") = "FS" Then
            ibtnAddFeeType.Visible = False
            ibtnRemoveFee.Visible = False
        Else
            ibtnAddFeeType.Visible = True
            ibtnRemoveFee.Visible = True
        End If
    End Sub

    Protected Sub btnSemester_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        MultiView1.SetActiveView(View2)
        imgLeft2.ImageUrl = "images/b_white_left.png"
        imgRight2.ImageUrl = "images/b_white_right.png"
        btnSemester.CssClass = "TabButtonClick"

        imgLeft1.ImageUrl = "images/b_orange_left.png"
        imgRight1.ImageUrl = "images/b_orange_right.png"
        btnAdmission.CssClass = "TabButton"

        imgLeft3.ImageUrl = "images/b_orange_left.png"
        imgRight3.ImageUrl = "images/b_orange_right.png"
        btnTution.CssClass = "TabButton"
        ddlFeeCategory.SelectedValue = "S"
        ibtnAddFeeType.Attributes.Add("onclick", "new_window=window.open('StudentFeetype.aspx?Category=" & ddlFeeCategory.SelectedValue & "','Hanodale','width=600,height=500,resizable=0,center');new_window.focus();")

        If Not Session("AddFee") Is Nothing Then
            dgViewFee.DataSource = Nothing
            dgViewFee.DataBind()
            LoadGridByFeeType()
        End If
        If Request.QueryString("Formid") = "FS" Then
            ibtnAddFeeType.Visible = False
            ibtnRemoveFee.Visible = False
        Else
            ibtnAddFeeType.Visible = True
            ibtnRemoveFee.Visible = True
            dgFeeBySem.Visible = True
        End If

    End Sub

    Protected Sub btnTution_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        MultiView1.SetActiveView(View3)
        imgLeft3.ImageUrl = "images/b_white_left.png"
        imgRight3.ImageUrl = "images/b_white_right.png"
        btnTution.CssClass = "TabButtonClick"

        imgLeft1.ImageUrl = "images/b_orange_left.png"
        imgRight1.ImageUrl = "images/b_orange_right.png"
        btnAdmission.CssClass = "TabButton"

        imgLeft2.ImageUrl = "images/b_orange_left.png"
        imgRight2.ImageUrl = "images/b_orange_right.png"
        btnSemester.CssClass = "TabButton"
        ddlFeeCategory.SelectedValue = "T"
        ibtnAddFeeType.Attributes.Add("onclick", "new_window=window.open('StudentFeetype.aspx?Category=" & ddlFeeCategory.SelectedValue & "&SelectionType=S','Hanodale','width=600,height=500,resizable=0,center');new_window.focus();")
        'LoadGridByFeeType()
        'ibtnAddFeeType.Visible = True
        'ibtnRemoveFee.Visible = True
        If Not Session("AddFee") Is Nothing Then
            dgViewFee.DataSource = Nothing
            dgViewFee.DataBind()
            LoadGridByFeeType()
        End If
        If Request.QueryString("Formid") = "FS" Then
            ibtnAddFeeType.Visible = False
            ibtnRemoveFee.Visible = False
        Else
            ibtnAddFeeType.Visible = True
            ibtnRemoveFee.Visible = True
            dgFeeBySem.Visible = True
        End If
    End Sub

    Protected Sub ddlTution_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        'If ddlTution.SelectedValue = 1 Then
        '    lblPoint.Visible = True
        '    txtTutPoint.Visible = True
        'Else
        '    lblPoint.Visible = False
        '    txtTutPoint.Visible = False
        'End If
        If ddlTution.SelectedValue = 1 Then
            lblCredit.Visible = True
        Else
            lblCredit.Visible = False
        End If
    End Sub

    Protected Sub ddlProgram_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlProgram.SelectedIndexChanged
        If ddlProgram.SelectedValue = "-1" Then
            txtPgCode.Text = ""
        Else
            txtPgCode.Text = ddlProgram.SelectedValue
        End If

    End Sub

    Protected Sub ibtnCancel_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ibtnCancel.Click
        LoadUserRights()
        OnClearData()
        resetTabSelection()
    End Sub

    Protected Sub ibtnNew_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ibtnNew.Click
        onAdd()
    End Sub

    Protected Sub txtTutAmt_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtTutAmt.TextChanged
        'Added by Zoya @15/04/2016
        If txtTutAmt.Text = "" Then txtTutAmt.Text = 0
        'End Added by Zoya @15/04/2016

        Dim amount As Double = txtTutAmt.Text
        txtTutAmt.Text = String.Format("{0:F}", amount)
    End Sub

    Protected Sub ddlSem_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlSem.SelectedIndexChanged
        'txtSemister.Text = ddlSem.SelectedValue
        'LoadSession()
    End Sub


    Protected Sub ibtnAddFeeType_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)

    End Sub

    Protected Sub ChkClone_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ChkClone.CheckedChanged
        If ChkClone.Checked = True Then
            Session("PageMode") = "Add"
        ElseIf ChkClone.Checked = False Then
            Session("PageMode") = "Edit"
        End If

        If Session("PageMode") = "Edit" Then
            ddlBidang.Enabled = False
            ddlEFrom.Enabled = False
        Else
            ddlBidang.Enabled = True
            ddlEFrom.Enabled = True
            'clear ddl bidang and eff.from for new entry
            ddlBidang.SelectedValue = "-1"
            ddlEFrom.SelectedValue = "-1"
            'disable next and last button
            txtFSCode.Text = ""
            txtRecNo.Text = ""
            lblCount.Text = ""
            ibtnNext.Enabled = False
            ibtnNext.ImageUrl = "images/gnew_next.png"
            ibtnLast.Enabled = False
            ibtnLast.ImageUrl = "images/gnew_last.png"
            ibtnPrevs.Enabled = False
            ibtnPrevs.ImageUrl = "images/gnew_Prev.png"
            ibtnFirst.Enabled = False
            ibtnFirst.ImageUrl = "images/gnew_first.png"
            'Dim myScript As String = "window.alert('Cloning in progress');"
            'ClientScript.RegisterStartupScript(Me.GetType(), "myScript", myScript, True)
        End If
    End Sub

    Protected Sub btnHidden_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnHidden.Click

    End Sub

#Region "GST Function "
    Public Function GSTFunc(ByVal Amt As Double, ByVal gst As Double) As String
        Dim TaxId As Integer = 0
        TaxId = Session("TaxId")
        Try
            TaxId = _GstSetupDal.GetGstDetails(MaxGeneric.clsGeneric.NullToInteger(TaxId)).Tables(0).Rows(0)(3).ToString()
        Catch

        End Try
        Dim ActAmout As Double = 0
        If (TaxId = Generic._TaxMode.Inclusive) Then
            ActAmout = MaxGeneric.clsGeneric.NullToDecimal(Amt) - gst
        ElseIf (TaxId = Generic._TaxMode.Exclusive) Then
            ActAmout = MaxGeneric.clsGeneric.NullToDecimal(Amt) - gst
        End If
        Return ActAmout
    End Function
#End Region

#Region "GST Function2 "
    Public Function GSTFunc2(ByVal Amt As Double, ByVal gst As Double) As String
        Dim TaxId As Integer = 0
        TaxId = Session("TaxId")
        Try
            TaxId = _GstSetupDal.GetGstDetails(MaxGeneric.clsGeneric.NullToInteger(TaxId)).Tables(0).Rows(0)(3).ToString()
        Catch

        End Try
        Dim ActAmout As Double = 0
        If (TaxId = Generic._TaxMode.Inclusive) Then
            ActAmout = MaxGeneric.clsGeneric.NullToDecimal(Amt)
        ElseIf (TaxId = Generic._TaxMode.Exclusive) Then
            ActAmout = MaxGeneric.clsGeneric.NullToDecimal(Amt) - gst
        End If
        Return ActAmout
    End Function
#End Region

#Region "GST Calculation - Starting "
    'Protected Sub txtFeeAmount_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs)

    '    'control declaration
    '    Dim dgitem As DataGridItem
    '    Dim _txtFeeAmount As TextBox
    '    Dim _txtGSTAmount As TextBox
    '    Dim _txtTotFeeAmount As TextBox
    '    Dim _txtActFeeAmount As TextBox

    '    'varaible declaration
    '    Dim FeeAmount As Double, GSTAmt As Double, ActAmout As Double
    '    Dim TaxMode As String
    '    Dim TaxId As Integer = 0
    '    Try
    '        lblMsg.Text = String.Empty

    '        'Assign TaxId
    '        TaxId = Session("TaxId")
    '        TaxMode = _GstSetupDal.GetGstDetails(MaxGeneric.clsGeneric.NullToInteger(TaxId)).Tables(0).Rows(0)(3).ToString()

    '        'GST Calculation - Stating
    '        For Each dgitem In dgViewType.Items

    '            _txtFeeAmount = dgitem.FindControl("txtFeeAmount")
    '            _txtGSTAmount = dgitem.FindControl("txtGSTAmount")
    '            _txtTotFeeAmount = dgitem.FindControl("txtTotFeeAmount")
    '            _txtActFeeAmount = dgitem.FindControl("txtActFeeAmount")

    '            'Validate Fee Amount - Start
    '            Dim intFee As Double
    '            If Not Double.TryParse(_txtFeeAmount.Text, intFee) Then
    '                lblMsg.Visible = True
    '                lblMsg.Text = "Please Enter Valid Fee Amount"
    '            End If
    '            'Validate Fee Amount - Stop

    '            FeeAmount = _txtFeeAmount.Text
    '            If Not TaxId = 0 Then
    '                GSTAmt = _GstSetupDal.GetGstAmount(TaxId, MaxGeneric.clsGeneric.NullToDecimal(FeeAmount)) 'TAX id
    '            Else
    '                Throw New Exception("TaxCode Missing")
    '                'lblMsg.Text = "TaxCode Note Available"

    '            End If

    '            If (TaxMode = Generic._TaxMode.Inclusive) Then
    '                ActAmout = MaxGeneric.clsGeneric.NullToDecimal(FeeAmount) - GSTAmt
    '            ElseIf (TaxMode = Generic._TaxMode.Exclusive) Then
    '                ActAmout = FeeAmount
    '                FeeAmount = MaxGeneric.clsGeneric.NullToDecimal(FeeAmount) + GSTAmt
    '            End If

    '            _txtActFeeAmount.Text = String.Format("{0:F}", ActAmout)
    '            _txtGSTAmount.Text = String.Format("{0:F}", GSTAmt)
    '            _txtTotFeeAmount.Text = String.Format("{0:F}", FeeAmount)

    '        Next
    '        'GST Calculation - Ended
    '    Catch ex As Exception
    '        If ex.Message = "TaxCode Missing" Then
    '            lblMsg.Visible = True
    '            lblMsg.Text = "Required Tax Type"
    '        End If
    '        Call MaxModule.Helper.LogError(ex.Message)
    '    End Try
    'End Sub
#End Region

    'Protected Sub txtFeeAmount_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs)

    '    Dim txtamt As TextBox
    '    Dim amount As Double
    '    ListFeeStrDet = Session("AddFee")

    '    Dim dgitem As DataGridItem
    '    Dim i As Integer = 0
    '    Dim temp As Integer = 0
    '    For Each dgitem In dgViewType.Items
    '        txtamt = dgitem.FindControl("txtFeeAmount")
    '        If dgView.SelectedIndex <> -1 Then
    '            ListFeeStrDet(CInt(dgView.SelectedIndex)).ListFeeAmount(i).FeeAmount = String.Format("{0:F}", CDbl(txtamt.Text))
    '        End If
    '        i = i + 1
    '        amount = txtamt.Text

    '        txtamt.Text = String.Format("{0:F}", amount)
    '    Next
    '    Session("AddFee") = ListFeeStrDet
    'End Sub

    Protected Sub ddlEFrom_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlEFrom.SelectedIndexChanged
        txtEffSem.Text = ddlEFrom.SelectedValue
        'LoadSession()
    End Sub

    Protected Sub ddlBidang_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlBidang.SelectedIndexChanged
        Dim obj As New FeeStructBAL
        Dim eobj As New FeeStructEn

        If ddlBidang.SelectedValue = "-1" Then
            txtBidang.Text = ""
        Else
            txtBidang.Text = ddlBidang.SelectedValue
        End If

    End Sub

#Region "Grid Item "

    Protected Sub dgFeeBySem_ItemCommand(source As Object, e As DataGridCommandEventArgs)
        'GridItem declaration
        Dim gditem As DataGridItem
        'LinkButton delclaration
        Dim _LinkButton As New LinkButton
        Dim TransId As Integer = 0
        Dim BatchId As String

        Try

            gditem = DirectCast(DirectCast(e.CommandSource, LinkButton).NamingContainer, DataGridItem)

        Catch ex As Exception
            Call MaxModule.Helper.LogError(ex.Message)
        End Try
    End Sub

#End Region

#Region "addFeeType_dgViewFee"

    Private Sub addFeeType_dgViewFee()

        Dim eobj As New List(Of FeeTypesEn)
        Dim bobj As New FeeTypesBAL
        Dim listobj As New List(Of FeeTypesEn)
        Dim eob As New FeeTypesEn
        'Dim feeType As String
        'Dim feeCat As String

        Try
            listobj = bobj.GetFeeDetails(eob)

        Catch ex As Exception
            LogError.Log("FeeStructure", "addFeeType_dgViewFee", ex.Message)
        End Try
        If listobj.Count <> 0 Then
            dgViewFee.DataSource = listobj
            'If eob.SCCode = "" Then
            '    dgView.Columns(3).Visible = False
            '    dgView.Columns(4).Visible = False
            '    'dgView.Columns(9).Visible = False
            '    dgView.Columns(5).Visible = True
            '    dgView.Columns(6).Visible = True
            '    'dgView.Columns(10).Visible = True
            '    'dgView.Columns(11).Visible = True
            'Else
            '    dgView.Columns(3).Visible = True
            '    dgView.Columns(4).Visible = True
            '    'dgView.Columns(9).Visible = True
            '    dgView.Columns(5).Visible = False
            '    dgView.Columns(6).Visible = False
            '    'dgView.Columns(10).Visible = False
            '    'dgView.Columns(11).Visible = False
            'End If
            dgViewFee.DataBind()
            'Else
            '    Response.Write("No Fee types are Available")
        End If

        'LoadGridByFeeType
        If ddlFeeCategory.SelectedValue <> "T" Then
            pnlTution.Visible = False
            If ddlFeeCategory.SelectedValue = "S" Then
                pnlSemester.Visible = True
            Else
                pnlSemester.Visible = False
            End If
        Else
            pnlTution.Visible = True
            pnlSemester.Visible = False
        End If

        feeflag = "ADDFEE"
        Session("eobj") = Nothing

    End Sub

#End Region

    Private Enum dgViewFeeCell As Integer
        ButtonFeeCode = 0
        Description = 1
        FeeType = 2
        FeeFor = 3
        FeeDetailSem = 4
        safs_code = 5
        Priority = 6
        FTCode = 7
        LocalFeeAmt = 8
        LocalActFeeAmt = 9
        LocalGSTAmt = 10
        LocalTotalFeeAmt = 11
        Separator = 12
        NonLocalFeeAmt = 13
        NonLocalActFeeAmt = 14
        NonLocalGSTAmt = 15
        NonLocalTotalFeeAmt = 16
        TaxId = 17
    End Enum

    Protected Sub txtFeeAmountLocal_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim txtamt As TextBox
        Dim amount As Double, GSTAmt As Double, ActAmount As Double
        Dim GetGST As DataSet
        Dim TaxMode As String
        Dim listFeeStrDet = New List(Of FeeStructEn)
        listFeeStrDet = Session("AddFee")
        Dim dgitem As DataGridItem

        Try
            For Each dgitem In dgViewFee.Items
                Dim CurrReferencCode As String
                Dim getFeeDetail As New FeeStructEn
                CurrReferencCode = dgViewFee.DataKeys(dgitem.ItemIndex).ToString()
                txtamt = dgitem.Cells(dgViewFeeCell.LocalFeeAmt).Controls(1)
                If txtamt.Text = "" Then txtamt.Text = 0

                'Validate Fee Amount - Start
                Dim intFee As Double
                If Not Double.TryParse(txtamt.Text, intFee) Then
                    lblMsg.Visible = True
                    lblMsg.Text = "Please Enter Valid Fee Amount"
                    'hfValidateAmt.Value = True
                    Exit Sub
                End If
                'Validate Fee Amount - Stop

                txtamt.Text = String.Format("{0:F}", CDbl(txtamt.Text))
                Dim feeFor As String
                If dgitem.Cells(dgViewFeeCell.FeeFor).Text = "All Semester" Then
                    feeFor = "0"
                ElseIf dgitem.Cells(dgViewFeeCell.FeeFor).Text = "Semester" Then
                    feeFor = "1"
                ElseIf dgitem.Cells(dgViewFeeCell.FeeFor).Text = "&nbsp;" Then
                    feeFor = String.Empty
                Else
                    feeFor = dgitem.Cells(dgViewFeeCell.FeeFor).Text
                End If

                getFeeDetail = listFeeStrDet.Where(Function(x) x.FTCode = CurrReferencCode And x.FeeType = dgitem.Cells(dgViewFeeCell.FeeType).Text And x.FeeDetailSem = dgitem.Cells(dgViewFeeCell.FeeDetailSem).Text And x.FeeFor = feeFor).FirstOrDefault()

                'GST, Actual Fee, Fee Amount Calculation - Start
                amount = String.Format("{0:F}", CDbl(txtamt.Text))

                Dim TaxId As Integer = MaxGeneric.clsGeneric.NullToInteger(dgitem.Cells(dgViewFeeCell.TaxId).Text)
                If Not TaxId = 0 Then
                    GetGST = _GstSetupDal.GetGstDetails(MaxGeneric.clsGeneric.NullToInteger(TaxId))
                Else
                    Throw New Exception("TaxCode Missing")
                End If
                TaxMode = GetGST.Tables(0).Rows(0)(3).ToString()

                If Not TaxId = 0 Then
                    GSTAmt = _GstSetupDal.GetGstAmount(TaxId, MaxGeneric.clsGeneric.NullToDecimal(txtamt.Text))
                Else
                    Throw New Exception("TaxCode Missing")
                End If

                GSTAmt = String.Format("{0:F}", GSTAmt)
                If (TaxMode = Generic._TaxMode.Inclusive) Then
                    ActAmount = MaxGeneric.clsGeneric.NullToDecimal(amount) - GSTAmt
                ElseIf (TaxMode = Generic._TaxMode.Exclusive) Then
                    ActAmount = amount
                    amount = MaxGeneric.clsGeneric.NullToDecimal(amount) + GSTAmt
                End If
                'GST, Actual Fee, Fee Amount Calculation - end
                dgitem.Cells(dgViewFeeCell.LocalGSTAmt).Text = GSTAmt

                getFeeDetail.LocalAmount = String.Format("{0:F}", CDbl(amount))
                getFeeDetail.LocalGSTAmount = String.Format("{0:F}", CDbl(GSTAmt))
                Dim ActFeeAmt As Double = amount - GSTAmt
                getFeeDetail.LocalTempAmount = String.Format("{0:F}", CDbl(ActFeeAmt))

            Next

            Session("AddFee") = listFeeStrDet
            Dim getSelectedFeeCtgy As New List(Of FeeStructEn)
            getSelectedFeeCtgy = listFeeStrDet.Where(Function(x) x.FeeType = ddlFeeCategory.SelectedValue).OrderBy(Function(p) p.FeeFor).OrderBy(Function(q) q.FeeDetailSem).OrderBy(Function(y) y.FTCode).ToList()
            dgViewFee.DataSource = getSelectedFeeCtgy
            dgViewFee.DataBind()
        Catch ex As Exception
            If ex.Message = "TaxCode Missing" Then
                lblMsg.Visible = True
                lblMsg.Text = "Required Tax Type"
            End If
            Call MaxModule.Helper.LogError(ex.Message)
        End Try
    End Sub

    Protected Sub txtFeeAmountInter_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim txtamt As TextBox
        Dim amount As Double, GSTAmt As Double, ActAmount As Double
        Dim GetGST As DataSet
        Dim TaxMode As String
        Dim listFeeStrDet = New List(Of FeeStructEn)
        listFeeStrDet = Session("AddFee")
        Dim dgitem As DataGridItem

        Try
            For Each dgitem In dgViewFee.Items
                Dim CurrReferencCode As String
                Dim getFeeDetail As New FeeStructEn
                CurrReferencCode = dgViewFee.DataKeys(dgitem.ItemIndex).ToString()
                txtamt = dgitem.Cells(dgViewFeeCell.NonLocalFeeAmt).Controls(1)
                If txtamt.Text = "" Then txtamt.Text = 0

                'Validate Fee Amount - Start
                Dim intFee As Double
                If Not Double.TryParse(txtamt.Text, intFee) Then
                    lblMsg.Visible = True
                    lblMsg.Text = "Please Enter Valid Fee Amount"
                    'hfValidateAmt.Value = True
                    Exit Sub
                End If
                'Validate Fee Amount - Stop

                txtamt.Text = String.Format("{0:F}", CDbl(txtamt.Text))
                Dim feeFor As String
                If dgitem.Cells(dgViewFeeCell.FeeFor).Text = "All Semester" Then
                    feeFor = "0"
                ElseIf dgitem.Cells(dgViewFeeCell.FeeFor).Text = "Semester" Then
                    feeFor = "1"
                ElseIf dgitem.Cells(dgViewFeeCell.FeeFor).Text = "&nbsp;" Then
                    feeFor = String.Empty
                Else
                    feeFor = dgitem.Cells(dgViewFeeCell.FeeFor).Text
                End If

                getFeeDetail = listFeeStrDet.Where(Function(x) x.FTCode = CurrReferencCode And x.FeeType = dgitem.Cells(dgViewFeeCell.FeeType).Text And x.FeeDetailSem = dgitem.Cells(dgViewFeeCell.FeeDetailSem).Text And x.FeeFor = feeFor).FirstOrDefault()

                'GST, Actual Fee, Fee Amount Calculation - Start
                amount = String.Format("{0:F}", CDbl(txtamt.Text))

                Dim TaxId As Integer = MaxGeneric.clsGeneric.NullToInteger(dgitem.Cells(dgViewFeeCell.TaxId).Text)
                If Not TaxId = 0 Then
                    GetGST = _GstSetupDal.GetGstDetails(MaxGeneric.clsGeneric.NullToInteger(TaxId))
                Else
                    Throw New Exception("TaxCode Missing")
                End If
                TaxMode = GetGST.Tables(0).Rows(0)(3).ToString()

                If Not TaxId = 0 Then
                    GSTAmt = _GstSetupDal.GetGstAmount(TaxId, MaxGeneric.clsGeneric.NullToDecimal(txtamt.Text))
                Else
                    Throw New Exception("TaxCode Missing")
                End If

                GSTAmt = String.Format("{0:F}", GSTAmt)
                If (TaxMode = Generic._TaxMode.Inclusive) Then
                    ActAmount = MaxGeneric.clsGeneric.NullToDecimal(amount) - GSTAmt
                ElseIf (TaxMode = Generic._TaxMode.Exclusive) Then
                    ActAmount = amount
                    amount = MaxGeneric.clsGeneric.NullToDecimal(amount) + GSTAmt
                End If
                'GST, Actual Fee, Fee Amount Calculation - end
                dgitem.Cells(dgViewFeeCell.NonLocalGSTAmt).Text = GSTAmt

                getFeeDetail.NonLocalAmount = String.Format("{0:F}", amount)
                getFeeDetail.NonLocalGSTAmount = String.Format("{0:F}", GSTAmt)
                Dim ActFeeAmt As Double = amount - GSTAmt
                getFeeDetail.NonLocalTempAmount = String.Format("{0:F}", ActFeeAmt)
            Next
            Session("AddFee") = listFeeStrDet
            Dim getSelectedFeeCtgy As New List(Of FeeStructEn)
            getSelectedFeeCtgy = listFeeStrDet.Where(Function(x) x.FeeType = ddlFeeCategory.SelectedValue).OrderBy(Function(p) p.FeeFor).OrderBy(Function(q) q.FeeDetailSem).OrderBy(Function(y) y.FTCode).ToList()
            dgViewFee.DataSource = getSelectedFeeCtgy
            dgViewFee.DataBind()
        Catch ex As Exception
            If ex.Message = "TaxCode Missing" Then
                lblMsg.Visible = True
                lblMsg.Text = "Required Tax Type"
            End If
            Call MaxModule.Helper.LogError(ex.Message)
        End Try
    End Sub

#Region "GST Function3 (Fee Amount)"
    Public Function GSTFunc3(ByVal Amt As Double, ByVal gst As Double, ByVal tax As Integer) As String
        Dim TaxId As Integer = tax

        Try
            TaxId = _GstSetupDal.GetGstDetails(MaxGeneric.clsGeneric.NullToInteger(TaxId)).Tables(0).Rows(0)(3).ToString()
        Catch

        End Try
        Dim ActAmout As Double = 0
        If (TaxId = Generic._TaxMode.Inclusive) Then
            ActAmout = MaxGeneric.clsGeneric.NullToDecimal(Amt)
        ElseIf (TaxId = Generic._TaxMode.Exclusive) Then
            ActAmout = MaxGeneric.clsGeneric.NullToDecimal(Amt) - gst
        End If
        Return String.Format("{0:F}", ActAmout)
    End Function
#End Region

    ''' <summary>
    ''' Method to reset the tab
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub resetTabSelection()

        If Request.QueryString("Formid") = "FS" Then
            ibtnAddFeeType.Visible = False
            ibtnRemoveFee.Visible = False
        Else
            imgLeft1.ImageUrl = "images/b_white_left.png"
            imgRight1.ImageUrl = "images/b_white_right.png"
            btnAdmission.CssClass = "TabButtonClick"


            imgLeft2.ImageUrl = "images/b_orange_left.png"
            imgRight2.ImageUrl = "images/b_orange_right.png"
            btnSemester.CssClass = "TabButton"

            imgLeft3.ImageUrl = "images/b_orange_left.png"
            imgRight3.ImageUrl = "images/b_orange_right.png"
            btnTution.CssClass = "TabButton"

            ddlFeeCategory.SelectedValue = "A"
            ibtnAddFeeType.Attributes.Add("onclick", "new_window=window.open('StudentFeetype.aspx?Category=" & ddlFeeCategory.SelectedValue & "','Hanodale','width=600,height=500,resizable=0,center');new_window.focus();")
            ibtnAddFeeType.Visible = True
            ibtnRemoveFee.Visible = True
        End If
    End Sub

    Protected Sub dgViewFee_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dgViewFee.ItemDataBound
        Dim txtFeeAmountLocal As TextBox
        Dim txtFeeAmountInter As TextBox
        Dim GSTAmt As Double = 0
        Dim TaxId As Integer = 0
        Dim TaxMode As Integer = 0
        Dim FeeFor As String
        'Dim Sem As Integer
        Select Case e.Item.ItemType
            Case ListItemType.Item, ListItemType.AlternatingItem, ListItemType.SelectedItem
                txtFeeAmountLocal = CType(e.Item.FindControl("txtFeeAmountLocal"), TextBox)
                txtFeeAmountLocal.Attributes.Add("onKeyPress", "checkValue();")
                txtFeeAmountLocal.Text = String.Format("{0:F}", CDbl(txtFeeAmountLocal.Text))
                txtFeeAmountInter = CType(e.Item.FindControl("txtFeeAmountInter"), TextBox)
                txtFeeAmountInter.Attributes.Add("onKeyPress", "checkValue();")
                txtFeeAmountInter.Text = String.Format("{0:F}", CDbl(txtFeeAmountInter.Text))

                FeeFor = e.Item.Cells(dgViewFeeCell.FeeFor).Text
                If FeeFor = "0" Then
                    e.Item.Cells(dgViewFeeCell.FeeFor).Text = "All Semester"
                ElseIf FeeFor = "1" Then
                    e.Item.Cells(dgViewFeeCell.FeeFor).Text = "Semester"
                Else
                    e.Item.Cells(dgViewFeeCell.FeeFor).Text = FeeFor
                End If

                'Added by Mona 4/3/2016 -Disable fee amount changes
                If Request.QueryString("Formid") = "FS" Then
                    txtFeeAmountLocal.Enabled = False
                    txtFeeAmountInter.Enabled = False
                    txtTutAmt.Enabled = False
                End If
        End Select

    End Sub

    Protected Sub ddlFeeFor_SelectedIndexChanged(sender As Object, e As EventArgs)

        If ddlFeeFor.SelectedValue = "1" Then
            lblSemester.Visible = True
            txtSemster.Visible = True
        Else
            lblSemester.Visible = False
            txtSemster.Visible = False
        End If
    End Sub

    Private Sub ResetGrid(Optional Type As String = "A")
        If Type = "T" Then
            'dgViewFee.Columns(dgViewFeeCell.LocalFeeAmt).Visible = False
            'dgViewFee.Columns(dgViewFeeCell.LocalGSTAmt).Visible = False
            'dgViewFee.Columns(dgViewFeeCell.LocalActFeeAmt).Visible = False
            'dgViewFee.Columns(dgViewFeeCell.LocalTotalFeeAmt).Visible = False
            'dgViewFee.Columns(dgViewFeeCell.NonLocalActFeeAmt).Visible = False
            'dgViewFee.Columns(dgViewFeeCell.NonLocalFeeAmt).Visible = False
            'dgViewFee.Columns(dgViewFeeCell.NonLocalGSTAmt).Visible = False
            'dgViewFee.Columns(dgViewFeeCell.NonLocalTotalFeeAmt).Visible = False
            'dgViewFee.Columns(dgViewFeeCell.Separator).Visible = False
        Else
            dgViewFee.Columns(dgViewFeeCell.LocalFeeAmt).Visible = True
            dgViewFee.Columns(dgViewFeeCell.LocalGSTAmt).Visible = True
            dgViewFee.Columns(dgViewFeeCell.LocalActFeeAmt).Visible = True
            dgViewFee.Columns(dgViewFeeCell.LocalTotalFeeAmt).Visible = True
            dgViewFee.Columns(dgViewFeeCell.NonLocalActFeeAmt).Visible = True
            dgViewFee.Columns(dgViewFeeCell.NonLocalFeeAmt).Visible = True
            dgViewFee.Columns(dgViewFeeCell.NonLocalGSTAmt).Visible = True
            dgViewFee.Columns(dgViewFeeCell.NonLocalTotalFeeAmt).Visible = True
            dgViewFee.Columns(dgViewFeeCell.Separator).Visible = True
        End If
    End Sub

    Private Sub TabSelection(Optional selection As String = "A")
        If selection = "S" Then
            imgLeft1.ImageUrl = "images/b_white_left.png"
            imgRight1.ImageUrl = "images/b_white_right.png"
            btnAdmission.CssClass = "TabButton"


            imgLeft2.ImageUrl = "images/b_orange_left.png"
            imgRight2.ImageUrl = "images/b_orange_right.png"
            btnSemester.CssClass = "TabButtonClick"

            imgLeft3.ImageUrl = "images/b_orange_left.png"
            imgRight3.ImageUrl = "images/b_orange_right.png"
            btnTution.CssClass = "TabButton"

            ddlFeeCategory.SelectedValue = "S"
            ibtnAddFeeType.Attributes.Add("onclick", "new_window=window.open('StudentFeetype.aspx?Category=" & ddlFeeCategory.SelectedValue & "','Hanodale','width=600,height=500,resizable=0,center');new_window.focus();")

        ElseIf selection = "T" Then
            imgLeft1.ImageUrl = "images/b_white_left.png"
            imgRight1.ImageUrl = "images/b_white_right.png"
            btnAdmission.CssClass = "TabButton"


            imgLeft2.ImageUrl = "images/b_orange_left.png"
            imgRight2.ImageUrl = "images/b_orange_right.png"
            btnSemester.CssClass = "TabButton"

            imgLeft3.ImageUrl = "images/b_orange_left.png"
            imgRight3.ImageUrl = "images/b_orange_right.png"
            btnTution.CssClass = "TabButtonClick"

            ddlFeeCategory.SelectedValue = "T"
            ibtnAddFeeType.Attributes.Add("onclick", "new_window=window.open('StudentFeetype.aspx?Category=" & ddlFeeCategory.SelectedValue & "&SelectionType=S','Hanodale','width=600,height=500,resizable=0,center');new_window.focus();")

        Else

            imgLeft1.ImageUrl = "images/b_white_left.png"
            imgRight1.ImageUrl = "images/b_white_right.png"
            btnAdmission.CssClass = "TabButtonClick"


            imgLeft2.ImageUrl = "images/b_orange_left.png"
            imgRight2.ImageUrl = "images/b_orange_right.png"
            btnSemester.CssClass = "TabButton"

            imgLeft3.ImageUrl = "images/b_orange_left.png"
            imgRight3.ImageUrl = "images/b_orange_right.png"
            btnTution.CssClass = "TabButton"

            ddlFeeCategory.SelectedValue = "A"
            ibtnAddFeeType.Attributes.Add("onclick", "new_window=window.open('StudentFeetype.aspx?Category=" & ddlFeeCategory.SelectedValue & "','Hanodale','width=600,height=500,resizable=0,center');new_window.focus();")
        End If
    End Sub
End Class
