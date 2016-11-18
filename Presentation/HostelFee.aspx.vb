Imports System.Data
Imports HTS.SAS.BusinessObjects
Imports HTS.SAS.Entities
Imports System.Collections.Generic
Imports System.Linq

Partial Class HostelFee
    Inherits System.Web.UI.Page
    Private ListHFeeStrDet As List(Of HostelStructEn) 'Details of hostel struct
    Private ListHFeeStrAmt As List(Of HostelStrAmountEn)
    Private ListObjects As List(Of HostelStructEn)
    Private _GstSetupDal As New HTS.SAS.DataAccessObjects.GSTSetupDAL
    Dim DFlag As String
    Private ErrorDescription As String
    ''Private LogErrors As LogError

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack() Then
            'Adding validation for save button
            ibtnSave.Attributes.Add("onclick", "return validate()")
            ibtnDelete.Attributes.Add("onclick", "return getconfirm()")
            txtRecNo.Attributes.Add("OnKeyUp", "return geterr()")
            'Loading User Rights
            Session("PageMode") = ""
            Session("AddFee") = Nothing
            LoadUserRights()
            'While loading the page make the CFlag as null
            Session("Block") = ddlBloackCode.DataSourceID
            'Session("Room") = ddlRoomType.DataSourceID
            'while loading list object make it nothing
            Session("ListObj") = Nothing
            FillDropDownList()
            DisableRecordNavigator()
            'load PageName
            Menuname(CInt(Request.QueryString("Menuid")))
            'ddlfeetypefill()
            ibtnAddFeeType.Attributes.Add("onclick", "new_window=window.open('StudentFeetype.aspx?Category=H','Hanodale','width=450,height=600,resizable=0');new_window.focus();")
            'lnkAddKolej.Attributes.Add("onclick", "new_window=window.open('AddKolej.aspx?cat= "+','Hanodale','width=520,height=400,resizable=0');new_window.focus();")
            Session("StudentCategory") = Nothing
            GetStudentCategory()
        End If
        If Not Session("eobj") Is Nothing Then
            addFeeType()
        End If
        lblMsg.Visible = False
        lblMsg.Text = ""
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
    Protected Sub ibtnNext_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ibtnNext.Click
        OnMoveNext()
    End Sub

    Protected Sub ibtnLast_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ibtnLast.Click
        OnMoveLast()
    End Sub

    Protected Sub ibtnPrevs_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ibtnPrevs.Click
        OnMovePrevious()
    End Sub

    Protected Sub ibtnFirst_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ibtnFirst.Click
        OnMoveFirst()
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
        Try
            eobj = bobj.GetMenus(eobj)
        Catch ex As Exception
            LogError.Log("HostelFee", "Menuname", ex.Message)
            lblMsg.Text = ex.Message.ToString
        End Try
        lblMenuName.Text = eobj.MenuName
    End Sub
    ''' <summary>
    ''' Method to Add FeeTypes
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub addFeeType()

        Dim newHostelCode As New HostelStructEn
        Dim listFee As New List(Of FeeTypesEn)
        ListHFeeStrDet = New List(Of HostelStructEn)
        Dim listStuCtgy As New List(Of StudentCategoryEn)
        Dim localCtgyCode As String = ""
        Dim nonLocalCtgyCode As String = ""
        If Not Session("AddFee") Is Nothing Then
            ListHFeeStrDet = Session("AddFee")
        End If

        If Not Session("StudentCategory") Is Nothing Then
            listStuCtgy = Session("StudentCategory")
        End If

        localCtgyCode = listStuCtgy.Where(Function(x) x.StudentCategoryCode = "W" Or x.StudentCategoryCode = "Local").FirstOrDefault().StudentCategoryCode
        nonLocalCtgyCode = listStuCtgy.Where(Function(y) y.StudentCategoryCode = "BW" Or y.StudentCategoryCode = "International").FirstOrDefault().StudentCategoryCode

        listFee = Session("eobj")
        If listFee.Count <> 0 Then
            For Each eobjFt In listFee
                If Not ListHFeeStrDet.Any(Function(x) x.FTCode = eobjFt.FeeTypeCode) Then
                    newHostelCode = New HostelStructEn
                    newHostelCode.FTCode = eobjFt.FeeTypeCode
                    newHostelCode.Description = eobjFt.Description
                    newHostelCode.HostelStructureCode = txtHostelFeeCode.Text
                    newHostelCode.Priority = CInt(eobjFt.Priority)
                    newHostelCode.FeeCategory = "H"
                    newHostelCode.LocalAmount = eobjFt.LocalAmount
                    newHostelCode.LocalGSTAmount = eobjFt.LocalGSTAmount
                    newHostelCode.LocalTempAmount = newHostelCode.LocalAmount - newHostelCode.LocalGSTAmount
                    newHostelCode.LocalCategory = localCtgyCode
                    newHostelCode.NonLocalAmount = eobjFt.NonLocalAmount
                    newHostelCode.NonLocalGSTAmount = eobjFt.NonLocalGSTAmount
                    newHostelCode.NonLocalTempAmount = newHostelCode.NonLocalAmount - newHostelCode.NonLocalGSTAmount
                    newHostelCode.NonLocalCategory = nonLocalCtgyCode
                    newHostelCode.TaxId = eobjFt.TaxId
                    ListHFeeStrDet.Add(newHostelCode)
                End If
            Next
        End If

        Session("AddFee") = ListHFeeStrDet

        If ListHFeeStrDet.Count > 0 Then
            dgViewHostelFee.DataSource = ListHFeeStrDet
            dgViewHostelFee.DataBind()
            If dgViewHostelFee.Items.Count > 0 Then
                dgViewHostelFee.SelectedIndex = dgViewHostelFee.Items.Count - 1
            End If
        End If
        Session("eobj") = Nothing
    End Sub
    'Private Function LoadFeeDetailAmount(ByVal code As String)
    '    Session("Amount") = Nothing
    '    Dim bobj As New FeeTypesBAL
    '    Dim obj As FeeTypesEn
    '    Dim eobjHFeeStrDet As HostelStrAmountEn
    '    Dim Lst As New List(Of FeeChargesEn)
    '    Dim LstFeeTypeObject As List(Of FeeTypesEn)
    '    Dim i As Integer = 0
    '    Dim dcode As String = ""
    '    obj = New FeeTypesEn()
    '    obj.FeeTypeCode = code
    '    obj.FeeType = ""
    '    obj.Description = ""
    '    obj.Status = 1
    '    Try
    '        LstFeeTypeObject = bobj.GetFeeList(obj)
    '        obj = LstFeeTypeObject(0)
    '    Catch ex As Exception
    '        LogError.Log("HostelFee", "LoadFeeDetailAmount", ex.Message)
    '        lblMsg.Text = ex.Message.ToString
    '    End Try

    '    Lst = obj.ListFeeCharges

    '    ListHFeeStrAmt = New List(Of HostelStrAmountEn)
    '    While i < Lst.Count
    '        eobjHFeeStrDet = New HostelStrAmountEn
    '        eobjHFeeStrDet.FTCode = Lst(i).FTCode
    '        eobjHFeeStrDet.SCCode = Lst(i).SCCode
    '        eobjHFeeStrDet.HAAmount = Lst(i).FSAmount
    '        eobjHFeeStrDet.SCDesc = Lst(i).SCDesc
    '        ListHFeeStrAmt.Add(eobjHFeeStrDet)
    '        eobjHFeeStrDet = Nothing
    '        i = i + 1
    '    End While
    '    Session("Amount") = ListHFeeStrAmt
    '    dgViewType.DataSource = ListHFeeStrAmt
    '    dgViewType.DataBind()
    '    ListHFeeStrAmt = Nothing
    '    Session("eobj") = Nothing
    '    Return dcode
    'End Function
    ''' <summary>
    ''' Method to Load All the Dropdowns
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub FillDropDownList()
        Dim eKolej As New KolejEn
        Dim bKolej As New KolejBAL
        ddlHostelCOde.Items.Clear()
        ddlHostelCOde.Items.Add(New ListItem("---Select---", "-1"))
        ddlHostelCOde.DataTextField = "SAKO_Description"
        ddlHostelCOde.DataValueField = "SAKO_Code"

        Try
            ddlHostelCOde.DataSource = bKolej.GetList(eKolej)
        Catch ex As Exception
            LogError.Log("HostelFee", "FillDropDownList", ex.Message)
            lblMsg.Text = ex.Message.ToString
        End Try

        ddlHostelCOde.DataBind()
    End Sub

    ''' <summary>
    ''' Method to Save HostelFee
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub onsave()
        Dim eobj As New HostelStructEn
        Dim bsobj As New HostelStructBAL
        Dim LstHostel As New List(Of HostelStructEn)
        Dim LstHostelDetailsToSave As New List(Of HostelStrDetailsEn)
      
        Dim RecAff As Integer
        lblMsg.Text = ""
        lblMsg.Visible = True
        If dgViewHostelFee.Items.Count <> 0 Then
            eobj.HostelStructureCode = txtHostelFeeCode.Text
            eobj.Code = ddlHostelCOde.SelectedValue
            eobj.Block = ddlBloackCode.SelectedValue
            eobj.RoomTYpe = "-1" 'ddlRoomType.SelectedValue
            eobj.EffectFm = ""
            eobj.UpdatedUser = Session("User")
            If ddlStatus.SelectedValue = 0 Then
                eobj.Status = False
            Else
                eobj.Status = True
            End If
            If Session("AddFee") Is Nothing Then
                LstHostel = New List(Of HostelStructEn)
            Else
                LstHostel = Session("AddFee")
            End If
            LstHostelDetailsToSave = New List(Of HostelStrDetailsEn)
            eobj.lstHFeeSD = LstHostelDetailsToSave
            If LstHostel.Count > 0 Then
                LstHostelDetailsToSave = New List(Of HostelStrDetailsEn)
                For Each obj In LstHostel
                    Dim objHostelDetail As New HostelStrDetailsEn 'to store detail for hostel struct list
                    Dim LstHostelAmtToSave As New List(Of HostelStrAmountEn) 'to store the amount for local and nonlocal 
                    Dim objHostelAmt As New HostelStrAmountEn
                    '------Clear existing data before insert--------'
                    ' obj.lstHFeeSD = LstHostelDetailsToSave
                    objHostelDetail.ListFeeAmount = New List(Of HostelStrAmountEn)
                    '-----------------------------------------------'
                    objHostelDetail = New HostelStrDetailsEn
                    objHostelDetail.HSCode = txtHostelFeeCode.Text
                    objHostelDetail.FTCode = obj.FTCode
                    objHostelDetail.HDPriority = obj.Priority
                    objHostelDetail.TaxId = obj.TaxId
                    objHostelDetail.HDType = "H"
                    '-----------------------------------------------'
                    objHostelAmt = New HostelStrAmountEn
                    objHostelAmt.FTCode = obj.FTCode
                    objHostelAmt.SCCode = obj.LocalCategory
                    objHostelAmt.HAAmount = obj.LocalAmount
                    'Added by Zoya @15/04/2016
                    If objHostelAmt.HAAmount = 0 Then
                        lblMsg.Visible = True
                        lblMsg.Text = "Amount Cannot be Zero"
                        Exit Sub
                    End If
                    'End Added by Zoya @15/04/2016
                    objHostelAmt.GstAmount = obj.LocalGSTAmount
                    objHostelAmt.HSCode = obj.HostelStructureCode
                    LstHostelAmtToSave.Add(objHostelAmt)
                    '-----------------------------------------------'
                    objHostelAmt = New HostelStrAmountEn
                    objHostelAmt.FTCode = obj.FTCode
                    objHostelAmt.SCCode = obj.NonLocalCategory
                    objHostelAmt.HAAmount = obj.NonLocalAmount
                    'Added by Zoya @15/04/2016
                    If objHostelAmt.HAAmount = 0 Then
                        lblMsg.Visible = True
                        lblMsg.Text = "Amount Cannot be Zero"
                        Exit Sub
                    End If
                    'End Added by Zoya @15/04/2016
                    objHostelAmt.GstAmount = obj.NonLocalGSTAmount
                    objHostelAmt.HSCode = obj.HostelStructureCode
                    LstHostelAmtToSave.Add(objHostelAmt)
                    '-----------------------------------------------'
                    objHostelDetail.ListFeeAmount = LstHostelAmtToSave 'Bind for fee amount
                    LstHostelDetailsToSave.Add(objHostelDetail)
                    '-----------------------------------------------'
                    'obj.lstHFeeSD = LstHostelDetailsToSave.ToList() 'Bind for details 
                Next
                eobj.lstHFeeSD = LstHostelDetailsToSave
            End If
            eobj.lstHFeeWithAmt = LstHostel
            If Session("PageMode") = "Add" Then
                Try
                    eobj.HostelStructureCode = String.Empty
                    RecAff = bsobj.Insert(eobj)
                    ErrorDescription = "Record Saved Successfully "
                    lblMsg.Text = ErrorDescription

                Catch ex As Exception
                    lblMsg.Text = ex.Message.ToString()
                    LogError.Log("HostelFee", "onsave", ex.Message)
                End Try
            ElseIf Session("PageMode") = "Edit" Then
                Try
                    RecAff = bsobj.Update(eobj)
                    ListObjects = Session("ListObj")
                    ListObjects(CInt(txtRecNo.Text) - 1) = eobj
                    Session("ListObj") = ListObjects
                    ErrorDescription = "Record Updated Successfully "
                    lblMsg.Text = ErrorDescription
                Catch ex As Exception
                    lblMsg.Text = ex.Message.ToString()
                    LogError.Log("HostelFee", "onsave", ex.Message)
                End Try
            End If
        Else
            lblMsg.Text = "Enter At Least One Fee Type"
        End If
    End Sub
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
            LogError.Log("HostelFee", "LoadUserRights", ex.Message)
            lblMsg.Text = ex.Message.ToString
        End Try
        'Rights for Add

        If eobj.IsAdd = True Then
            'ibtnSave.Enabled = True
            onadd()
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
                Dim obj As HostelStructEn
                ListObjects = Session("ListObj")
                pnlView.Visible = True
                pnlAdd.Visible = True
                obj = ListObjects(RecNo)
                txtHostelFeeCode.Text = obj.HostelStructureCode
                txtBlockCode.Text = obj.Code
                txtBlock.Text = obj.Block

                ddlHostelCOde.SelectedValue = obj.Code
                LoadBlock()
                ddlBloackCode.SelectedValue = obj.Block
                'LoadRoom()
                ddlRoomType.SelectedValue = "-1" ' obj.RoomTYpe
                txtRoomType.Text = obj.RoomTYpe

                If obj.Status = True Then
                    ddlStatus.SelectedValue = 1
                Else
                    ddlStatus.SelectedValue = 0
                End If

                'dgView.DataSource = obj.lstHFeeSD
                'dgView.DataBind()
                dgViewHostelFee.DataSource = obj.lstHFeeWithAmt
                dgViewHostelFee.DataBind()
                If dgViewHostelFee.Items.Count > 0 Then
                    dgViewHostelFee.SelectedIndex = 0
                End If
                Session("AddFee") = obj.lstHFeeWithAmt
            End If
        End If
        hfValidateAmt.Value = False
    End Sub
    ''' <summary>
    ''' Method to get the List Of HostelFees
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub LoadListObjects()
        Dim obj As New HostelStructBAL
        Dim eobj As New HostelStructEn
        Dim recStu As Integer

        If ddlStatus.SelectedValue = -1 Then
            recStu = -1
        Else
            recStu = ddlStatus.SelectedValue
        End If
        If ddlHostelCOde.SelectedValue <> "-1" Then
            eobj.Code = ddlHostelCOde.SelectedValue
        Else
            eobj.Code = ""
        End If
        If ddlRoomType.SelectedValue <> "-1" Then
            eobj.RoomTYpe = ddlRoomType.SelectedValue
        Else
            eobj.RoomTYpe = ""
        End If

        If ddlBloackCode.SelectedValue <> "-1" Then
            eobj.Block = ddlBloackCode.SelectedValue
        Else
            eobj.Block = ""
        End If
        eobj.Status = recStu

        Try
            ListObjects = obj.GetHostelStructList(eobj)

        Catch ex As Exception
            LogError.Log("HostelFee", "LoadListObjects", ex.Message)
            lblMsg.Text = ex.Message.ToString
        End Try

        Session("ListObj") = ListObjects
        lblCount.Text = ListObjects.Count.ToString()
        If ListObjects.Count <> 0 Then
            DisableRecordNavigator()
            txtRecNo.Text = "1"
            pnlView.Visible = False
            pnlAdd.Visible = True
            OnMoveFirst()
            If Session("EditFlag") = True Then
                Session("PageMode") = "Edit"
                txtHostelFeeCode.Enabled = False
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
            'OnClearData()

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
    ''' Method to Clear the Field Values
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub OnClearData()
        txtHostelFeeCode.Enabled = True
        Session("ListObj") = Nothing
        DisableRecordNavigator()
        Session("AddFee") = Nothing
        txtHostelFeeCode.Text = ""
        txtBlockCode.Text = ""
        txtBlock.Text = ""
        txtRoomType.Text = ""
        ddlHostelCOde.SelectedValue = "-1"

        ddlBloackCode.Items.Clear()
        ddlBloackCode.Items.Add(New ListItem("--Select--", "-1"))
        ddlRoomType.Items.Clear()
        ddlRoomType.Items.Add(New ListItem("--Select--", "-1"))
        dgViewHostelFee.DataSource = Nothing
        dgViewHostelFee.DataBind()
        
        ddlStatus.SelectedValue = "1"
        lblMsg.Text = ""

        Session("PageMode") = "Add"
    End Sub
    ''' <summary>
    ''' Method to Load Fields in New Mode
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub onadd()
        Session("PageMode") = "Add"
        ibtnSave.Enabled = True
        ibtnSave.ImageUrl = "images/save.png"
        OnClearData()

    End Sub
    ''' <summary>
    ''' Method to Change  the Mode to Edit
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub OnEdit()

        Session("PageMode") = "Edit"

    End Sub
    ''' <summary>
    ''' Method to Delete HostelFee
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub onDelete()
        lblMsg.Visible = True
        If txtHostelFeeCode.Text <> "" Then
            If lblCount.Text = "" Then lblCount.Text = 0
            If lblCount.Text > 0 Then

                Dim bsobj As New HostelStructBAL
                Dim eobj As New HostelStructEn
                Dim RecAff As Integer
                lblMsg.Visible = True
                eobj.HostelStructureCode = txtHostelFeeCode.Text
                Try
                    RecAff = bsobj.Delete(eobj)
                    ListObjects = Session("ListObj")
                    ListObjects.RemoveAt(CInt(txtRecNo.Text) - 1)
                    lblCount.Text = lblCount.Text - 1
                    Session("ListObj") = ListObjects
                    onadd()
                    lblMsg.Visible = True
                    ErrorDescription = "Record Deleted Successfully "
                    lblMsg.Text = ErrorDescription

                Catch ex As Exception
                    lblMsg.Text = ex.Message.ToString()
                    LogError.Log("HostelFee", "onDelete", ex.Message)


                End Try
                txtHostelFeeCode.Text = ""
                txtBlockCode.Text = ""
                txtBlock.Text = ""
                txtRoomType.Text = ""
                ddlStatus.SelectedValue = "1"
                DFlag = "Delete"
                'LoadListObjects()
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
    ''' Method to Load Room Dropdown
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub LoadRoom(Optional ByVal defaultVal As String = "")
        Dim bRoomType As New RoomTypeBAL
        ddlRoomType.Items.Clear()
        ddlRoomType.Items.Add(New ListItem("---Select---", "-1"))
        ddlRoomType.DataTextField = "SART_Description"
        ddlRoomType.DataValueField = "SART_Code"
        'ddlRoomType.SelectedValue = defaultVal
        Try
            ddlRoomType.DataSource = bRoomType.GetRoomTypeList(ddlBloackCode.SelectedValue)
        Catch ex As Exception
            LogError.Log("HostelFee", "LoadRoom", ex.Message)
            lblMsg.Text = ex.Message.ToString
        End Try
        ddlRoomType.DataBind()
    End Sub
    ''' <summary>
    ''' Method to Load Block Dropdown
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub LoadBlock()
        Dim eBlock As New BlockEn
        Dim bBlock As New BlockBAL
        ddlBloackCode.Items.Clear()
        ddlBloackCode.Items.Add(New ListItem("---Select---", "-1"))
        ddlBloackCode.DataTextField = "SABK_Description"
        ddlBloackCode.DataValueField = "SABK_Code"
        Try
            ddlBloackCode.DataSource = bBlock.GetBlockList(ddlHostelCOde.SelectedValue)

        Catch ex As Exception
            LogError.Log("HostelFee", "LoadBlock", ex.Message)
            lblMsg.Text = ex.Message.ToString
        End Try
        ddlBloackCode.DataBind()
        ddlRoomType.Items.Clear()
        ddlRoomType.Items.Add(New ListItem("---Select---", "-1"))
    End Sub
#End Region

    Protected Sub ibtnSave_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ibtnSave.Click
        onsave()
    End Sub


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
                LoadListObjects()
            End If
        Else
            LoadListObjects()
        End If
    End Sub

  
    Protected Sub ibtnCancel_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ibtnCancel.Click
        LoadUserRights()
        OnClearData()
    End Sub

    Protected Sub ibtnNew_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ibtnNew.Click
        onadd()
    End Sub

    Protected Sub ibtnDelete_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ibtnDelete.Click
        onDelete()
    End Sub
    Protected Sub ddlHostelCOde_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlHostelCOde.SelectedIndexChanged
        LoadBlock()
    End Sub


    Protected Sub ddlBloackCode_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlBloackCode.SelectedIndexChanged
        'LoadRoom()
    End Sub


    Protected Sub ddlRoomType_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlRoomType.SelectedIndexChanged
        txtRoomType.Text = ddlRoomType.SelectedValue
    End Sub


    Protected Sub ibtnRemoveFee_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ibtnRemoveFee.Click
        ListHFeeStrDet = Session("AddFee")
        If Not ListHFeeStrDet Is Nothing Then
            If dgViewHostelFee.SelectedIndex <> -1 Then
                Dim getFTcode As String = dgViewHostelFee.DataKeys(dgViewHostelFee.SelectedIndex)
                ListHFeeStrDet.RemoveAll(Function(x) x.FTCode = getFTcode)
                Dim i As Integer = 0
                While i < ListHFeeStrDet.Count
                    '  ListHFeeStrDet(i).ObjectID = i
                    i = i + 1
                End While
                dgViewHostelFee.DataSource = ListHFeeStrDet
                dgViewHostelFee.DataBind()
                Session("AddFee") = ListHFeeStrDet
                dgViewHostelFee.SelectedIndex = -1

            Else
                lblMsg.Visible = True
                ErrorDescription = "Select a Fee code from the Fee list"
                lblMsg.Text = ErrorDescription
                End If
                If dgViewHostelFee.Items.Count = 0 Then
                    dgViewHostelFee.DataSource = Nothing
                    dgViewHostelFee.DataBind()
                End If
        End If
    End Sub

    Protected Sub btnHidden_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnHidden.Click

    End Sub


#Region "GST Function (Actual Fee Amount)"
    Public Function GSTFunc(ByVal Amt As Double, ByVal gst As Double, ByVal tax As Integer) As String
         Dim TaxId As Integer = tax
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


#Region "GST Function2 (Fee Amount)"
    Public Function GSTFunc2(ByVal Amt As Double, ByVal gst As Double, ByVal tax As Integer) As String
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

    Private Enum dgViewHostelCell As Integer
        ButtonFeeCode = 0
        Description = 1
        Priority = 2
        sahs_code = 3
        LocalFeeAmt = 4
        LocalActFeeAmt = 5
        LocalGSTAmt = 6
        LocalTotalFeeAmt = 7
        Separator = 8
        NonLocalFeeAmt = 9
        NonLocalActFeeAmt = 10
        NonLocalGSTAmt = 11
        NonLocalTotalFeeAmt = 12
        TaxId = 13
    End Enum

    Protected Sub txtFeeAmountLocal_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        hfValidateAmt.Value = False
        Dim txtamt As TextBox
        Dim amount As Double, GSTAmt As Double, ActAmount As Double
        Dim GetGST As DataSet
        Dim TaxMode As String
        ListHFeeStrDet = New List(Of HostelStructEn)
        ListHFeeStrDet = Session("AddFee")
        Dim dgitem As DataGridItem
        Try
            For Each dgitem In dgViewHostelFee.Items
                Dim CurrReferencCode As String
                Dim getHostelDetail As New HostelStructEn
                CurrReferencCode = dgViewHostelFee.DataKeys(dgitem.ItemIndex).ToString()
                txtamt = dgitem.Cells(dgViewHostelCell.LocalFeeAmt).Controls(1)
                If txtamt.Text = "" Then txtamt.Text = 0

                'Validate Fee Amount - Start
                Dim intFee As Double
                If Not Double.TryParse(txtamt.Text, intFee) Then
                    lblMsg.Visible = True
                    lblMsg.Text = "Please Enter Valid Fee Amount"
                    hfValidateAmt.Value = True
                    Exit Sub
                End If
                'Validate Fee Amount - Stop

                txtamt.Text = String.Format("{0:F}", CDbl(txtamt.Text))
                getHostelDetail = ListHFeeStrDet.Where(Function(x) x.FTCode = CurrReferencCode).FirstOrDefault()

                'GST, Actual Fee, Fee Amount Calculation - Start
                amount = String.Format("{0:F}", CDbl(txtamt.Text))

                Dim TaxId As Integer = MaxGeneric.clsGeneric.NullToInteger(dgitem.Cells(dgViewHostelCell.TaxId).Text)
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

                dgitem.Cells(dgViewHostelCell.LocalGSTAmt).Text = GSTAmt
                getHostelDetail.LocalAmount = amount
                getHostelDetail.LocalGSTAmount = GSTAmt
                getHostelDetail.LocalTempAmount = amount - GSTAmt
            Next
            Session("AddFee") = ListHFeeStrDet
            dgViewHostelFee.DataSource = ListHFeeStrDet
            dgViewHostelFee.DataBind()
        Catch ex As Exception
            If ex.Message = "TaxCode Missing" Then
                lblMsg.Visible = True
                lblMsg.Text = "Required Tax Type"
            End If
            Call MaxModule.Helper.LogError(ex.Message)
        End Try
    End Sub

    Protected Sub txtFeeAmountInter_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        hfValidateAmt.Value = False
        Dim txtamt As TextBox
        Dim amount As Double, GSTAmt As Double, ActAmount As Double
        Dim GetGST As DataSet
        Dim TaxMode As String
        ListHFeeStrDet = New List(Of HostelStructEn)
        ListHFeeStrDet = Session("AddFee")
        Dim dgitem As DataGridItem
        Try
            For Each dgitem In dgViewHostelFee.Items
                Dim CurrReferencCode As String
                Dim getHostelDetail As New HostelStructEn
                CurrReferencCode = dgViewHostelFee.DataKeys(dgitem.ItemIndex).ToString()
                txtamt = dgitem.Cells(dgViewHostelCell.NonLocalFeeAmt).Controls(1)
                If txtamt.Text = "" Then txtamt.Text = 0
                'Validate Fee Amount - Start
                Dim intFee As Double
                If Not Double.TryParse(txtamt.Text, intFee) Then
                    lblMsg.Visible = True
                    lblMsg.Text = "Please Enter Valid Fee Amount"
                    hfValidateAmt.Value = True
                    Exit Sub
                End If
                'Validate Fee Amount - Stop
                txtamt.Text = String.Format("{0:F}", CDbl(txtamt.Text))
                getHostelDetail = ListHFeeStrDet.Where(Function(x) x.FTCode = CurrReferencCode).FirstOrDefault()

                'GST, Actual Fee, Fee Amount Calculation - Start
                amount = String.Format("{0:F}", CDbl(txtamt.Text))
                Dim TaxId As Integer = MaxGeneric.clsGeneric.NullToInteger(dgitem.Cells(dgViewHostelCell.TaxId).Text)
                If Not TaxId = 0 Then
                    GetGST = _GstSetupDal.GetGstDetails(TaxId)
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

                dgitem.Cells(dgViewHostelCell.NonLocalGSTAmt).Text = GSTAmt
                getHostelDetail.NonLocalAmount = amount
                getHostelDetail.NonLocalGSTAmount = GSTAmt
                getHostelDetail.NonLocalTempAmount = amount - GSTAmt
            Next
            Session("AddFee") = ListHFeeStrDet
            dgViewHostelFee.DataSource = ListHFeeStrDet
            dgViewHostelFee.DataBind()
        Catch ex As Exception
            If ex.Message = "TaxCode Missing" Then
                lblMsg.Visible = True
                lblMsg.Text = "Required Tax Type"
            End If
            Call MaxModule.Helper.LogError(ex.Message)
        End Try
    End Sub

    Private Sub GetStudentCategory()
        Dim eStuCtgy As New StudentCategoryEn
        Dim eListStuCtgy As New List(Of StudentCategoryEn)
        Dim bStuCtgy As New StudentCategoryBAL

        Try
            eListStuCtgy = bStuCtgy.GetList(eStuCtgy)
            Session("StudentCategory") = eListStuCtgy
        Catch ex As Exception
            LogError.Log("HostelFee", "GetStudentCategory", ex.Message)
            lblMsg.Text = ex.Message.ToString
        End Try
    End Sub
    Protected Sub dgViewHostelFee_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dgViewHostelFee.ItemDataBound
        Dim txtFeeAmountLocal As TextBox
        Dim txtFeeAmountInter As TextBox
        Dim GSTAmt As Double = 0
        Dim TaxId As Integer = 0
        Dim TaxMode As Integer = 0

        TaxId = Session("TaxId")

        Select Case e.Item.ItemType
            Case ListItemType.Item, ListItemType.AlternatingItem, ListItemType.SelectedItem
                txtFeeAmountLocal = CType(e.Item.FindControl("txtFeeAmountLocal"), TextBox)
                txtFeeAmountLocal.Attributes.Add("onKeyPress", "checkValue();")
                txtFeeAmountLocal.Text = String.Format("{0:F}", CDbl(txtFeeAmountLocal.Text))
                txtFeeAmountInter = CType(e.Item.FindControl("txtFeeAmountInter"), TextBox)
                txtFeeAmountInter.Attributes.Add("onKeyPress", "checkValue();")
                txtFeeAmountInter.Text = String.Format("{0:F}", CDbl(txtFeeAmountInter.Text))
        End Select

    End Sub
End Class
