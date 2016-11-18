Imports HTS.SAS.BusinessObjects
Imports HTS.SAS.Entities
Imports System.Data
Imports System.Collections.Generic
Imports Microsoft.VisualBasic
Imports System.Linq

Partial Class Kokorikulum
    Inherits System.Web.UI.Page
    Private _GstSetupDal As New HTS.SAS.DataAccessObjects.GSTSetupDAL
#Region "Global Declaration "
    'declare instant
    Private _MaxModule As New MaxModule.CfCommon

    Private ErrorDescription As String
    Dim DFlag As String
    Dim ListObjects As List(Of FeeTypesEn)
    Private ListHFeeStrDet As List(Of KokoEn) 'Details of hostel stru
    Private ListHFee As List(Of KokoEn)
#End Region

    ''Private LogErrors As LogError
    Protected Sub ibtnNew_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ibtnNew.Click
        OnAdd()
    End Sub
    Protected Sub ibtnSave_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ibtnSave.Click
        SpaceValidation()
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack() Then
            'Adding validation for save button
            ibtnSave.Attributes.Add("onclick", "return validate()")
            ibtnDelete.Attributes.Add("onclick", "return getconfirm()")
            txtRecNo.Attributes.Add("OnKeyUp", "return geterr()")
            txtCreditHours.Attributes.Add("onKeypress", "isNumberKey(event)")
            'Loading User Rights
            dgView.Visible = False
            dgViewKokoFee.Visible = True
            'DataGridView.DataBindingComplete()
            Session("PageMode") = "Add"
            Session("AddFee") = Nothing
            Session("getkoko") = Nothing
            Session("getkokorikulum") = Nothing
            LoadUserRights()
            FillDropDownList()
            'while loading list object make it nothing
            Session("ListObj") = Nothing
            DisableRecordNavigator()
            'load PageName
            Menuname(CInt(Request.QueryString("Menuid")))
            ibtnAddFeeType.Attributes.Add("onclick", "new_window=window.open('StudentFeetype.aspx?Category=H','Hanodale','width=450,height=600,resizable=0');new_window.focus();")
            imgGL.Visible = False
            Session("StudentCategory") = Nothing
            GetStudentCategory()
            'ibtnDelete.Enabled = False
        End If
        If Not Session("eobj") Is Nothing Then
            addFeeType()
        End If
        lblMsg.Text = ""
        lblMsg.Visible = False
        
    End Sub
    Protected Sub ibtnDelete_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ibtnDelete.Click
        onDelete()
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
    Protected Sub dgView_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dgView.ItemDataBound
        Dim txtAmount As TextBox


        Select Case e.Item.ItemType
            Case ListItemType.Item, ListItemType.AlternatingItem
                txtAmount = CType(e.Item.FindControl("txtFeeAmount"), TextBox)
                txtAmount.Attributes.Add("onKeyPress", "isNumberKey(event)")
                Dim amount As Double
                amount = e.Item.Cells(3).Text
                txtAmount.Text = String.Format("{0:F}", amount)

        End Select

    End Sub
    'Protected Sub dgViewKokoFee_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dgViewKokoFee.ItemDataBound
    '    Dim txtFeeAmountLocalOut As TextBox
    '    Dim txtFeeAmountLocalIn As TextBox
    '    Dim txtFeeAmountInterIn As TextBox
    '    Dim txtFeeAmountInterOut As TextBox
    '    Dim amount As Double = 0
    '    Dim amount1 As Double = 0
    '    Dim amount2 As Double = 0
    '    Dim amount3 As Double = 0
    '    Dim TaxId As Integer = 0
    '    Dim TaxMode As Integer = 0

    '    TaxId = Session("TaxId")
    '    Select Case e.Item.ItemType
    '        Case ListItemType.Item, ListItemType.AlternatingItem, ListItemType.SelectedItem
    '            txtFeeAmountLocalIn = CType(e.Item.FindControl("txtFeeAmountLocalIn"), TextBox)
    '            txtFeeAmountLocalIn.Attributes.Add("onKeyPress", "checkValue();")
    '            'txtFeeAmountLocalIn.Text = String.Format("{0:F}", CDbl(txtFeeAmountLocalIn.Text))
    '            'txtFeeAmountLocalIn.Text = String.Format("{0:F}", amount)
    '            'e.Item.Cells(2).Text = String.Format("{0:F}", amount)

    '            txtFeeAmountLocalOut = CType(e.Item.FindControl("txtFeeAmountLocalOut"), TextBox)
    '            txtFeeAmountLocalOut.Attributes.Add("onKeyPress", "checkValue();")
    '            'txtFeeAmountLocalOut.Text = String.Format("{0:F}", CDbl(txtFeeAmountLocalOut.Text))
    '            'txtFeeAmountLocalOut.Text = String.Format("{0:F}", amount1)
    '            'e.Item.Cells(3).Text = String.Format("{0:F}", amount1)

    '            txtFeeAmountInterIn = CType(e.Item.FindControl("txtFeeAmountInterIn"), TextBox)
    '            txtFeeAmountInterIn.Attributes.Add("onKeyPress", "checkValue();")
    '            'txtFeeAmountInterIn.Text = String.Format("{0:F}", CDbl(txtFeeAmountInterIn.Text))
    '            'txtFeeAmountInterIn.Text = String.Format("{0:F}", amount2)
    '            'e.Item.Cells(9).Text = String.Format("{0:F}", amount2)

    '            txtFeeAmountInterOut = CType(e.Item.FindControl("txtFeeAmountInterOut"), TextBox)
    '            txtFeeAmountInterOut.Attributes.Add("onKeyPress", "checkValue();")
    '            'txtFeeAmountInterOut.Text = String.Format("{0:F}", CDbl(txtFeeAmountInterOut.Text))
    '            'txtFeeAmountInterOut.Text = String.Format("{0:F}", amount3)
    '            'e.Item.Cells(10).Text = String.Format("{0:F}", amount3)
    '    End Select
    'End Sub
    Protected Sub ibtnCancel_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ibtnCancel.Click
        LoadUserRights()
        OnClearData()
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
#Region "Methods"
    ''' <summary>
    ''' Method to Fill the Field Values
    ''' </summary>
    ''' <param name="RecNo"></param>
    ''' <remarks></remarks>
    Private Sub FillData(ByVal RecNo As Integer)
        lblMsg.Visible = False
        Dim dbListFeeCharges As New List(Of FeeChargesEn)
        Dim getkoko As New List(Of KokoEn)
        Dim objacc As New StudentBAL
        Dim eobacc As New KokoEn
        Dim stlist As New List(Of KokoEn)
        Dim getkoku As New List(Of KokoEn)
        Dim bsstu As New AccountsBAL
        Dim stuen As New KokoEn
        Dim eobjDetails As New AccountsDetailsEn
        Dim newKokoCode As New KokoEn
        Dim eobjFt As New KokoEn
        Dim j As Integer = 0
        Dim dgitem As DataGridItem
        Dim amtin As Double = 0
        Dim amtout As Double = 0
        Dim amtinterin As Double = 0
        Dim amtinterout As Double = 0
        Dim txtfeeamountin As TextBox
        Dim txtfeeamountout As TextBox
        Dim txtfeeamountinterin As TextBox
        Dim txtfeeamountinterout As TextBox
        Dim stu As String
        Dim stk As String
        Dim localCtgyCode As String = ""
        Dim nonLocalCtgyCode As String = ""
        If Not Session("ListFeeCharge") Is Nothing Then
            dbListFeeCharges = Session("ListFeeCharge")
        End If
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
        pnlView.Visible = True
        PnlAdd.Visible = True
        If txtRecNo.Text = 0 Then
            txtRecNo.Text = 1
        Else
            If lblCount.Text = 0 Then
                txtRecNo.Text = 0
            Else
                Dim obj As FeeTypesEn
                Dim Lst As New List(Of FeeChargesEn)
                ListObjects = New List(Of FeeTypesEn)
                ListObjects = Session("ListObj")
                obj = ListObjects(RecNo)
                'eobacc.Code = Trim(txtKokoCode.Text)
                ddlKokoCode.SelectedValue = obj.FeeTypeCode

                'Try
                '    getkoko = objacc.GetKokoDetails(eobacc)
                'Catch ex As Exception
                '    LogError.Log("kokorikulum", "LoadGrid()", ex.Message)
                'End Try
                'Session("getkoko") = getkoko
                ''Dim saveAllStu As New List(Of KokoEn)
                ' ''save all the student to session by default
                ''If getkoko.Count > 0 Then
                ''    saveAllStu.AddRange(getkoko.Where(Function(y) Not saveAllStu.Any(Function(z) z.Saftcode = y.Saftcode)).Select(Function(x) New KokoEn With {.Saftcode = x.Saftcode, .LocalCategory = x.LocalCategory, .NonLocalCategory = x.NonLocalCategory,
                ''                                                                             .sakodfeeamountlocalin = x.sakodfeeamountlocalin, .sakodfeeamountlocalout = x.sakodfeeamountlocalout,
                ''                                                                             .sakodgstamountlocalin = x.sakodgstamountlocalin, .sakodfeegstamountlocalout = x.sakodfeegstamountlocalout,
                ''                                                                             .sakodfeeamountinterin = x.sakodfeeamountinterin, .sakodfeeamountinterout = x.sakodfeeamountinterout,
                ''                                                                              .sakodgstamountinterin = x.sakodgstamountinterin, .sakodgstamountinterout = x.sakodgstamountinterout}))
                ''End If
                ''Session("getkoko") = saveAllStu
                'If getkoko.Count <> 0 Then
                '    '    Dim lstcategory As List(Of String)
                '    '    lstcategory = getkoko.Select(Function(x) x.Saftcode).Distinct().ToList()
                '    'Dim categry As List(Of String)
                '    'categry = getkoko.Select(Function(x) x.Category).Distinct().ToList()

                '    For Each eobjFt In getkoko
                '        If Not eobjFt.sakodfeeamountlocalin = Nothing Then
                '            'While j < getkoko.Count
                '            'For Each eobjFt In getkoko.Where(Function(x) x.Saftcode = stk).ToList()
                '            'For Each eobjFt In getkoko
                '            'For Each newKokoCode In getkoko
                '            '    If Not getkoku.Any(Function(x) x.Category = newKokoCode.Category) Then
                '            '        getkoku.Add(New KokoEn With {.Saftcode = newKokoCode.Saftcode, .LocalCategory = newKokoCode.LocalCategory, .sakodfeeamountlocalin = newKokoCode.sakodfeeamountlocalin,
                '            '                                                  .sakodfeeamountlocalout = newKokoCode.sakodfeeamountlocalout, .sakodgstamountlocalin = newKokoCode.sakodgstamountlocalin, .sakodfeegstamountlocalout = newKokoCode.sakodfeegstamountlocalout,
                '            '                                     .NonLocalCategory = newKokoCode.NonLocalCategory, .sakodfeeamountinterin = newKokoCode.sakodfeeamountinterin
                '            '                                                 })
                '            '    Else
                '            '        getkoku.Add(New KokoEn With {
                '            '                                     .NonLocalCategory = newKokoCode.NonLocalCategory, .sakodfeeamountinterin = newKokoCode.sakodfeeamountinterin
                '            '                                                 })
                '            '        'Dim assignNewCategory As KokoEn = getkoku.Where(Function(x) x.NonLocalCategory = newKokoCode.NonLocalCategory).FirstOrDefault()
                '            '        'assignNewCategory.sakodfeeamountinterin = newKokoCode.sakodfeeamountinterin
                '            '        'assignNewCategory.sakodfeeamountinterout = newKokoCode.sakodfeeamountinterout
                '            '        'getkoku.Add(New KokoEn With {.Saftcode = newKokoCode.Saftcode, .NonLocalCategory = newKokoCode.NonLocalCategory, .sakodfeeamountinterin = newKokoCode.sakodfeeamountinterin,
                '            '        '                                          .sakodfeeamountinterout = newKokoCode.sakodfeeamountinterout, .sakodgstamountinterin = newKokoCode.sakodgstamountinterin, .sakodgstamountinterout = newKokoCode.sakodgstamountinterout
                '            '        '                                         })
                '            '    End If

                '            'Next
                '            If Not getkoku.Any(Function(x) x.Saftcode = eobjFt.Saftcode) Then
                '                newKokoCode = New KokoEn
                '                newKokoCode.Saftcode = eobjFt.Saftcode
                '                'If eobjFt.LocalCategory = "W" Then

                '                'newKokoCode = New KokoEn
                '                newKokoCode.LocalCategory = eobjFt.LocalCategory
                '                newKokoCode.sakodfeeamountlocalin = eobjFt.sakodfeeamountlocalin
                '                newKokoCode.sakodfeeamountlocalout = eobjFt.sakodfeeamountlocalout
                '                newKokoCode.sakodgstamountlocalin = eobjFt.sakodgstamountlocalin
                '                newKokoCode.sakodfeegstamountlocalout = eobjFt.sakodfeegstamountlocalout
                '                'newKokoCode.Local_TempAmount = newKokoCode.sakodfeeamountlocalin - newKokoCode.sakodgstamountlocalin
                '                'getkoko.Add(newKokoCode)
                '                'stlist.Add(newKokoCode)
                '                'ElseIf eobjFt.NonLocalCategory = "BW" Then
                '                'newKokoCode = New KokoEn

                '                'newKokoCode.Saftcode = eobjFt.Saftcode
                '                newKokoCode.NonLocalCategory = eobjFt.NonLocalCategory
                '                newKokoCode.sakodfeeamountinterin = eobjFt.sakodfeeamountinterin
                '                newKokoCode.sakodfeeamountinterout = eobjFt.sakodfeeamountinterout
                '                newKokoCode.sakodgstamountinterin = eobjFt.sakodgstamountinterin
                '                newKokoCode.sakodgstamountinterout = eobjFt.sakodgstamountinterout
                '                'newKokoCode.Inter_TempAmount = newKokoCode.sakodfeeamountinterin - newKokoCode.sakodgstamountinterin
                '                'stlist.Add(newKokoCode)
                '                'End If
                '                'newKokoCode = New KokoEn
                '                'newKokoCode.Saftcode = eobjFt.Saftcode
                '                'newKokoCode.Category = eobjFt.Category
                '                'newKokoCode.NonLocalCategory = eobjFt.NonLocalCategory
                '                'newKokoCode.sakodfeeamountlocalin = eobjFt.sakodfeeamountlocalin
                '                'newKokoCode.sakodfeeamountlocalout = eobjFt.sakodfeeamountlocalout
                '                'newKokoCode.sakodgstamountlocalin = eobjFt.sakodgstamountlocalin
                '                'newKokoCode.sakodfeegstamountlocalout = eobjFt.sakodfeegstamountlocalout
                '                'newKokoCode.Local_TempAmount = newKokoCode.sakodfeeamountlocalin - newKokoCode.sakodgstamountlocalin
                '                'newKokoCode.sakodfeeamountinterin = eobjFt.sakodfeeamountinterin
                '                'newKokoCode.sakodfeeamountinterout = eobjFt.sakodfeeamountinterout
                '                'newKokoCode.sakodgstamountinterin = eobjFt.sakodgstamountinterin
                '                'newKokoCode.sakodgstamountinterout = eobjFt.sakodgstamountinterout
                '                'newKokoCode.Inter_TempAmount = newKokoCode.sakodfeeamountinterin - newKokoCode.sakodgstamountinterin
                '                getkoku.Add(newKokoCode)

                '            End If

                '        End If

                '    Next

                '    '    If dgViewKokoFee.Items.Count > 0 Then
                '    '        dgViewKokoFee.SelectedIndex = dgViewKokoFee.Items.Count - 1
                '    'End If
                'End If
                Session("getkokorikulum") = obj.ListKokoCharges
                'Session("AddFee") = Nothing
                dgViewKokoFee.DataSource = obj.ListKokoCharges
                dgViewKokoFee.DataBind()
                'While j < getkoko.Count

                '    For Each dgitem In dgViewKokoFee.Items
                '        If Not dgitem.Cells(2).Text = dgitem.Cells(2).Text Then
                '            dgitem.Cells(2).Text = getkoko(j).Saftcode
                '            If (getkoko(j).LocalCategory = "W") Then
                '                dgitem.Cells(3).Text = getkoko(j).LocalCategory
                '                txtfeeamountin = dgitem.Cells(4).Controls(1)
                '                txtfeeamountout = dgitem.Cells(5).Controls(1)
                '                amtin = getkoko(j).sakodfeeamountlocalin
                '                amtout = getkoko(j).sakodfeeamountlocalout
                '                txtfeeamountin.Text = String.Format("{0:F}", amtin)
                '                txtfeeamountout.Text = String.Format("{0:F}", amtout)
                '            ElseIf (getkoko(j).NonLocalCategory = "BW") Then
                '                dgitem.Cells(10).Text = getkoko(j).NonLocalCategory
                '                'dgitem.Cells(10).Visible = False
                '                txtfeeamountinterin = dgitem.Cells(11).Controls(1)
                '                txtfeeamountinterout = dgitem.Cells(12).Controls(1)

                '                amtinterin = getkoko(j).sakodfeeamountinterin
                '                amtinterout = getkoko(j).sakodfeeamountinterout

                '                txtfeeamountinterin.Text = String.Format("{0:F}", amtinterin)
                '                txtfeeamountinterout.Text = String.Format("{0:F}", amtinterout)
                '            Else

                '            End If
                '            Exit For

                '        End If
                '        'End If
                '    Next
                '    j = j + 1
                '    'LoadTotals()
                'End While



                'End If
                'getkoku = getkoko


                pnlView.Visible = True
                PnlAdd.Visible = True
                dgViewKokoFee.Visible = True

                'dgViewKokoFee.DataSource = ListHFeeStrDet
                'dgViewKokoFee.DataBind()
                'While j < getkoko.Count
                '    'If getkoko.Count > j Then
                '    For Each dgitem In dgViewKokoFee.Items
                '        'If Not getkoko.Any(Function(x) x.Saftcode = dgitem.Cells(0).Text) Then
                '        'If getkoko(j).Category = "W" Then
                '        '    eobDetails = New KokoEn
                '        '    eobDetails.Saftcode = getkoko(j).Saftcode
                '        '    eobDetails.Category = "W"
                '        '    eobDetails.sakodfeeamountlocalin = getkoko(j).sakodfeeamountlocalin
                '        '    eobDetails.sakodfeeamountlocalout = getkoko(j).sakodfeeamountlocalout
                '        '    eobDetails.sakodgstamountlocalin = getkoko(j).sakodgstamountlocalin
                '        '    eobDetails.sakodfeegstamountlocalout = getkoko(j).sakodfeegstamountlocalout
                '        '    eobDetails.Local_TempAmount = getkoko(j).sakodfeeamountlocalin - getkoko(j).sakodgstamountlocalin
                '        'Else
                '        '    eobDetails.NonLocalCategory = "BW"
                '        '    eobDetails.sakodfeeamountinterin = getkoko(j).sakodfeeamountinterin
                '        '    eobDetails.sakodfeeamountinterout = getkoko(j).sakodfeeamountinterout
                '        '    eobDetails.sakodgstamountinterin = getkoko(j).sakodgstamountinterin
                '        '    eobDetails.sakodgstamountinterout = getkoko(j).sakodgstamountinterout
                '        '    eobDetails.Inter_TempAmount = getkoko(j).sakodfeeamountinterin - getkoko(j).sakodgstamountinterintxtAmount = dgItem1.Cells(7).Controls(1)
                '        'amt = liststuAll(j).TransactionAmount

                '        If getkoko(j).Category = "W" Then
                '            dgitem.Cells(0).Text = getkoko(j).Saftcode
                '            dgitem.Cells(1).Text = "W"
                '            amtin = getkoko(j).sakodfeeamountlocalin
                '            txtfeeamountin = dgitem.Cells(2).Controls(1)
                '            txtfeeamountin.Text = String.Format("{0:F}", amtin)
                '            amtout = getkoko(j).sakodfeeamountlocalout
                '            txtfeeamountout = dgitem.Cells(3).Controls(1)
                '            txtfeeamountout.Text = String.Format("{0:F}", amtout)
                '            dgitem.Cells(4).Text = getkoko(j).sakodfeeamountlocalin - getkoko(j).sakodgstamountlocalin
                '            dgitem.Cells(5).Text = getkoko(j).sakodgstamountlocalin
                '            dgitem.Cells(6).Text = getkoko(j).sakodfeegstamountlocalout
                '            'eobDetails.sakodfeeamountlocalin = eobDetails.sakodfeeamountlocalin
                '            'eobDetails.sakodfeeamountlocalout = eobDetails.sakodfeeamountlocalout
                '            'eobDetails.sakodgstamountlocalin = eobDetails.sakodgstamountlocalin
                '            'eobDetails.sakodfeegstamountlocalout = eobDetails.sakodgstamountlocalin
                '            'eobDetails.Local_TempAmount = eobDetails.sakodfeeamountlocalin - eobDetails.sakodgstamountlocalin

                '        Else
                '            dgitem.Cells(8).Text = "BW"
                '            amtinterin = getkoko(j).sakodfeeamountlocalin
                '            txtfeeamountinterin = dgitem.Cells(9).Controls(1)
                '            txtfeeamountinterin.Text = String.Format("{0:F}", amtinterin)
                '            amtinterout = getkoko(j).sakodfeeamountlocalout
                '            txtfeeamountinterout = dgitem.Cells(10).Controls(1)
                '            txtfeeamountinterout.Text = String.Format("{0:F}", amtinterout)
                '            'dgitem.Cells(9).Text = getkoko(j).sakodfeeamountlocalin
                '            'dgitem.Cells(10).Text = getkoko(j).sakodfeeamountlocalout
                '            dgitem.Cells(11).Text = getkoko(j).sakodfeeamountlocalin - getkoko(j).sakodgstamountlocalin
                '            dgitem.Cells(12).Text = getkoko(j).sakodgstamountlocalin
                '            dgitem.Cells(13).Text = getkoko(j).sakodfeegstamountlocalout
                '            'End If
                '        End If
                '        'getkoko.Add(eobDetails)
                '    Next
                '    j = j + 1
                'End While
                'pnlView.Visible = True
                'PnlAdd.Visible = True
                'dgViewKokoFee.Visible = True
                'Session("getkoko") = getkoko

                'If getkoko.Count > 0 Then
                '    dgViewKokoFee.DataSource = getkoko
                '    dgViewKokoFee.DataBind()
                '    If dgViewKokoFee.Items.Count > 0 Then
                '        dgViewKokoFee.SelectedIndex = dgViewKokoFee.Items.Count - 1
                '    End If
                'End If
                'Lst = obj.ListFeeCharges.ToList()

                ''Dim getNewSCCode As List(Of String)
                ''getNewSCCode = dbListFeeCharges.Where(Function(x) Not Lst.Any(Function(y) x.SCCode = y.SCCode)).Select(Function(z) z.SCCode).ToList()
                ''If getNewSCCode.Count > 0 Then
                ''    lblMsg.Text = String.Join(",", getNewSCCode) + " - New Category Code Which Not Exist For Current Record"
                ''    lblMsg.Visible = True
                ''End If
                ''Lst.AddRange(dbListFeeCharges.Where(Function(x) Not Lst.Any(Function(y) x.SCCode = y.SCCode)).ToList())

                'Dim getNewSCCode As List(Of String)
                'getNewSCCode = dbListFeeCharges.Where(Function(x) Not Lst.Any(Function(y) x.SCCode = y.SCCode)).Select(Function(z) z.SCCode).ToList()
                'If getNewSCCode.Count > 0 Then
                '    lblMsg.Text = String.Join(",", getNewSCCode) + " - New Category Code Which Not Exist For Current Record"
                '    lblMsg.Visible = True
                'End If
                'Lst.AddRange(dbListFeeCharges.Where(Function(x) Not Lst.Any(Function(y) x.SCCode = y.SCCode)).ToList())

                ''dgViewKokoFee.DataSource = Lst
                ''dgViewKokoFee.DataBind()
                'dgView.DataSource = Lst
                'dgView.DataBind()
                txtKokoCode.Text = obj.FeeTypeCode
                txtKokoDesc.Text = obj.Description
                txtCreditHours.Text = obj.CreditHours
                txtGlAmount.Text = obj.GLCode

                If obj.Status = True Then
                    ddlStatus.SelectedValue = 1
                Else
                    ddlStatus.SelectedValue = 0
                End If
                
            End If
        End If
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
    ''' <summary>
    ''' Method to Add FeeTypes
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub addFeeType()
        dgViewKokoFee.Visible = True
        Dim newKokoCode As New KokoEn
        Dim listFee As New List(Of FeeTypesEn)
        Dim ListHFeeStrDet = New List(Of KokoEn)
        Dim listStuCtgy As New List(Of StudentCategoryEn)
        Dim localCtgyCode As String = ""
        Dim nonLocalCtgyCode As String = ""
        Dim GetGST As DataSet
        Dim TaxMode As String
        'Dim txtfeeamountin As TextBox
        'Dim txtfeeamountout As TextBox
        'Dim txtgstamountin As TextBox
        'Dim txtgstamountout As TextBox
        'Dim feeamountin As Double
        'Dim gstamount As Double
        If Not Session("getkokorikulum") Is Nothing Then
            ListHFeeStrDet = Session("getkokorikulum")
        End If

        If Not Session("StudentCategory") Is Nothing Then
            listStuCtgy = Session("StudentCategory")
        End If

        localCtgyCode = listStuCtgy.Where(Function(x) x.StudentCategoryCode = "W" Or x.StudentCategoryCode = "Local").FirstOrDefault().StudentCategoryCode
        nonLocalCtgyCode = listStuCtgy.Where(Function(y) y.StudentCategoryCode = "BW" Or y.StudentCategoryCode = "International").FirstOrDefault().StudentCategoryCode
        'listFee.Find(localCtgyCode)
        listFee = Session("eobj")
        If listFee.Count <> 0 Then
            For Each eobjFt In listFee
                If Not ListHFeeStrDet.Any(Function(x) x.Saftcode = eobjFt.FeeTypeCode) Then
                    newKokoCode = New KokoEn
                    newKokoCode.Saftcode = eobjFt.FeeTypeCode
                    newKokoCode.LocalCategory = eobjFt.LocalCategory
                    newKokoCode.NonLocalCategory = eobjFt.NonLocalCategory
                    newKokoCode.sakod_idkoko = eobjFt.TaxId
                    If Not eobjFt.TaxId = 0 Then
                        GetGST = _GstSetupDal.GetGstDetails(MaxGeneric.clsGeneric.NullToInteger(eobjFt.TaxId))
                    Else
                        Throw New Exception("TaxCode Missing")
                    End If
                    TaxMode = GetGST.Tables(0).Rows(0)(3).ToString()

                    If (TaxMode = Generic._TaxMode.Inclusive) Then
                        newKokoCode.sakodfeeamountlocalin = eobjFt.LocalAmount
                        newKokoCode.sakodfeeamountlocalout = eobjFt.LocalAmount
                        newKokoCode.sakodgstamountlocalin = eobjFt.LocalGSTAmount
                        newKokoCode.sakodfeegstamountlocalout = eobjFt.LocalGSTAmount
                        newKokoCode.sakodfeeamountinterin = eobjFt.NonLocalAmount
                        newKokoCode.sakodfeeamountinterout = eobjFt.NonLocalAmount
                        newKokoCode.sakodgstamountinterin = eobjFt.NonLocalGSTAmount
                        newKokoCode.sakodgstamountinterout = eobjFt.NonLocalGSTAmount
                        newKokoCode.Local_TempAmount = newKokoCode.sakodfeeamountlocalin - newKokoCode.sakodgstamountlocalin
                        newKokoCode.Inter_TempAmount = newKokoCode.sakodfeeamountinterin - newKokoCode.sakodgstamountinterin
                        newKokoCode.LocalTempAmount = newKokoCode.sakodfeeamountlocalout - newKokoCode.sakodfeegstamountlocalout
                        newKokoCode.NonLocalTempAmount = newKokoCode.sakodfeeamountinterout - newKokoCode.sakodgstamountinterout
                        newKokoCode.totalfeelocalin = newKokoCode.Local_TempAmount + newKokoCode.sakodgstamountlocalin
                        newKokoCode.totalfeelocalout = newKokoCode.LocalTempAmount + newKokoCode.sakodfeegstamountlocalout
                        newKokoCode.totalfeeinterin = newKokoCode.Inter_TempAmount + newKokoCode.sakodgstamountinterin
                        newKokoCode.totalfeeinterout = newKokoCode.NonLocalTempAmount + newKokoCode.sakodgstamountinterout
                    ElseIf (TaxMode = Generic._TaxMode.Exclusive) Then
                        newKokoCode.sakodfeeamountlocalin = eobjFt.LocalAmount - eobjFt.LocalGSTAmount
                        newKokoCode.sakodfeeamountlocalout = eobjFt.LocalAmount - eobjFt.LocalGSTAmount
                        newKokoCode.sakodgstamountlocalin = eobjFt.LocalGSTAmount
                        newKokoCode.sakodfeegstamountlocalout = eobjFt.LocalGSTAmount
                        newKokoCode.sakodfeeamountinterin = eobjFt.NonLocalAmount - eobjFt.NonLocalGSTAmount
                        newKokoCode.sakodfeeamountinterout = eobjFt.NonLocalAmount - eobjFt.NonLocalGSTAmount
                        newKokoCode.sakodgstamountinterin = eobjFt.NonLocalGSTAmount
                        newKokoCode.sakodgstamountinterout = eobjFt.NonLocalGSTAmount
                        newKokoCode.Local_TempAmount = newKokoCode.sakodfeeamountlocalin
                        newKokoCode.Inter_TempAmount = newKokoCode.sakodfeeamountinterin
                        newKokoCode.LocalTempAmount = newKokoCode.sakodfeeamountlocalout
                        newKokoCode.NonLocalTempAmount = newKokoCode.sakodfeeamountinterout
                        newKokoCode.totalfeelocalin = newKokoCode.Local_TempAmount + newKokoCode.sakodgstamountlocalin
                        newKokoCode.totalfeelocalout = newKokoCode.LocalTempAmount + newKokoCode.sakodfeegstamountlocalout
                        newKokoCode.totalfeeinterin = newKokoCode.Inter_TempAmount + newKokoCode.sakodgstamountinterin
                        newKokoCode.totalfeeinterout = newKokoCode.NonLocalTempAmount + newKokoCode.sakodgstamountinterout
                    End If
                    'If (eobjFt.TaxId = 14) Then
                    '    newKokoCode.sakodfeeamountlocalin = eobjFt.LocalAmount - eobjFt.LocalGSTAmount
                    '    newKokoCode.sakodfeeamountlocalout = eobjFt.LocalAmount - eobjFt.LocalGSTAmount
                    '    newKokoCode.sakodgstamountlocalin = eobjFt.LocalGSTAmount
                    '    newKokoCode.sakodfeegstamountlocalout = eobjFt.LocalGSTAmount
                    '    newKokoCode.sakodfeeamountinterin = eobjFt.NonLocalAmount - eobjFt.NonLocalGSTAmount
                    '    newKokoCode.sakodfeeamountinterout = eobjFt.NonLocalAmount - eobjFt.NonLocalGSTAmount
                    '    newKokoCode.sakodgstamountinterin = eobjFt.NonLocalGSTAmount
                    '    newKokoCode.sakodgstamountinterout = eobjFt.NonLocalGSTAmount
                    '    newKokoCode.Local_TempAmount = newKokoCode.sakodfeeamountlocalin
                    '    newKokoCode.Inter_TempAmount = newKokoCode.sakodfeeamountinterin
                    '    newKokoCode.LocalTempAmount = newKokoCode.sakodfeeamountlocalout
                    '    newKokoCode.NonLocalTempAmount = newKokoCode.sakodfeeamountinterout
                    '    newKokoCode.totalfeelocalin = newKokoCode.Local_TempAmount + newKokoCode.sakodgstamountlocalin
                    '    newKokoCode.totalfeelocalout = newKokoCode.LocalTempAmount + newKokoCode.sakodfeegstamountlocalout
                    '    newKokoCode.totalfeeinterin = newKokoCode.Inter_TempAmount + newKokoCode.sakodgstamountinterin
                    '    newKokoCode.totalfeeinterout = newKokoCode.NonLocalTempAmount + newKokoCode.sakodgstamountinterout

                    'ElseIf (eobjFt.TaxId = 15) Then
                    '    newKokoCode.sakodfeeamountlocalin = eobjFt.LocalAmount
                    '    newKokoCode.sakodfeeamountlocalout = eobjFt.LocalAmount
                    '    newKokoCode.sakodgstamountlocalin = eobjFt.LocalGSTAmount
                    '    newKokoCode.sakodfeegstamountlocalout = eobjFt.LocalGSTAmount
                    '    newKokoCode.sakodfeeamountinterin = eobjFt.NonLocalAmount
                    '    newKokoCode.sakodfeeamountinterout = eobjFt.NonLocalAmount
                    '    newKokoCode.sakodgstamountinterin = eobjFt.NonLocalGSTAmount
                    '    newKokoCode.sakodgstamountinterout = eobjFt.NonLocalGSTAmount
                    '    newKokoCode.Local_TempAmount = newKokoCode.sakodfeeamountlocalin - newKokoCode.sakodgstamountlocalin
                    '    newKokoCode.Inter_TempAmount = newKokoCode.sakodfeeamountinterin - newKokoCode.sakodgstamountinterin
                    '    newKokoCode.LocalTempAmount = newKokoCode.sakodfeeamountlocalout - newKokoCode.sakodfeegstamountlocalout
                    '    newKokoCode.NonLocalTempAmount = newKokoCode.sakodfeeamountinterout - newKokoCode.sakodgstamountinterout
                    '    newKokoCode.totalfeelocalin = newKokoCode.Local_TempAmount + newKokoCode.sakodgstamountlocalin
                    '    newKokoCode.totalfeelocalout = newKokoCode.LocalTempAmount + newKokoCode.sakodfeegstamountlocalout
                    '    newKokoCode.totalfeeinterin = newKokoCode.Inter_TempAmount + newKokoCode.sakodgstamountinterin
                    '    newKokoCode.totalfeeinterout = newKokoCode.NonLocalTempAmount + newKokoCode.sakodgstamountinterout
                    'End If
                    ListHFeeStrDet.Add(newKokoCode)
                End If
            Next
        End If

        Session("AddFee") = ListHFeeStrDet

        If ListHFeeStrDet.Count > 0 Then
            dgViewKokoFee.DataSource = ListHFeeStrDet
            dgViewKokoFee.DataBind()
            If dgViewKokoFee.Items.Count > 0 Then
                dgViewKokoFee.SelectedIndex = dgViewKokoFee.Items.Count - 1
            End If
        End If
        Session("eobj") = Nothing
        Session("getkokorikulum") = Nothing
        'End If
        'Session("AddListStud") = liststuAll
        'dgViewKokoFee.DataSource = liststuAll
        'dgViewKokoFee.DataBind()
        'If dgViewKokoFee.Items.Count > 0 Then
        '    dgViewKokoFee.SelectedIndex =
        '    dgViewKokoFee.SelectedIndex = vbHide
        'End If
        'While j < liststuAll.Count


        '    For Each dgItem1 In dgViewKokoFee.Items
        '        'If dgItem1.Cells(0).Text < 1 Then
        '        'If liststuAll(j).Saftcode = liststuAll(j).Saftcode = 1 Then
        '        If liststuAll(j).Category = "W" Then
        '            'dgItem1.Cells(0).Text = liststuAll(j).Saftcode
        '            dgItem1.Cells(1).Text = "W"
        '            txtfeeamountin = dgItem1.Cells(2).Controls(1)
        '            txtfeeamountout = dgItem1.Cells(3).Controls(1)
        '            txtgstamountout = dgItem1.Cells(6).Controls(1)
        '            txtgstamountin = dgItem1.Cells(5).Controls(1)
        '            gstamount = liststuAll(j).
        '            feeamountin = liststuAll(j).sakodfee
        '            txtfeeamountin.Text = String.Format("{0:F}", feeamountin)
        '            txtfeeamountout.Text = String.Format("{0:F}", feeamountin)
        '            txtgstamountout.Text = String.Format("{0:F}", gstamount)
        '            txtgstamountin.Text = String.Format("{0:F}", gstamount)
        '            dgItem1.Cells(4).Text = feeamountin - gstamount
        '        ElseIf liststuAll(j).Category = "BW" Then
        '            'dgItem1.Cells(0).Text = liststuAll(j).Saftcode
        '            dgItem1.Cells(8).Text = "BW"
        '            txtfeeamountin = dgItem1.Cells(9).Controls(1)
        '            txtfeeamountout = dgItem1.Cells(10).Controls(1)
        '            txtgstamountout = dgItem1.Cells(12).Controls(1)
        '            txtgstamountin = dgItem1.Cells(13).Controls(1)
        '            gstamount = liststuAll(j).sakodgst
        '            feeamountin = liststuAll(j).sakodfee
        '            txtfeeamountin.Text = String.Format("{0:F}", feeamountin)
        '            txtfeeamountout.Text = String.Format("{0:F}", feeamountin)
        '            txtgstamountout.Text = String.Format("{0:F}", gstamount)
        '            txtgstamountin.Text = String.Format("{0:F}", gstamount)
        '            dgItem1.Cells(11).Text = feeamountin - gstamount
        '        End If
        '        Exit For

        '        'Else
        '        'End If
        '    Next
        '    j = j + 1
        '    'LoadTotals()
        'End While
        'Session("AddFee") = ListHFeeStrDet

        'If ListHFeeStrDet.Count > 0 Then
        '    dgViewHostelFee.DataSource = ListHFeeStrDet
        '    dgViewHostelFee.DataBind()
        '    If dgViewHostelFee.Items.Count > 0 Then
        '        dgViewHostelFee.SelectedIndex = dgViewHostelFee.Items.Count - 1
        '    End If
        'End If
        'Session("eobj") = Nothing
    End Sub
    Private Sub FillDropDownList()
        Dim eKolej As New KolejEn
        Dim bKolej As New KolejBAL
        ddlKokoCode.Items.Clear()
        ddlKokoCode.Items.Add(New ListItem("---Select---", "-1"))
        'ddlKokoCode.DataTextField = "SAKO_Description"
        ddlKokoCode.DataValueField = "SAKO_Code"

        Try
            ddlKokoCode.DataSource = bKolej.GetList(eKolej)
        Catch ex As Exception
            LogError.Log("HostelFee", "FillDropDownList", ex.Message)
            lblMsg.Text = ex.Message.ToString
        End Try
        txtKokoDesc.Text = eKolej.SAKO_Description
        ddlKokoCode.DataBind()
    End Sub
    Protected Sub ibtnRemoveFee_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ibtnRemoveFee.Click
        ListHFeeStrDet = Session("AddFee")
        'Session("getkokorikulum") = ListHFeeStrDet
        'Dim ListHFeeStrDet As New List(Of KokoEn)
        If Not ListHFeeStrDet Is Nothing Then
            If dgViewKokoFee.SelectedIndex <> -1 Then
                Dim getFTcode As String = dgViewKokoFee.DataKeys(dgViewKokoFee.SelectedIndex)
                ListHFeeStrDet.RemoveAll(Function(x) x.Saftcode = getFTcode)
                Dim i As Integer = 0
                While i < ListHFeeStrDet.Count
                    '  ListHFeeStrDet(i).ObjectID = i
                    i = i + 1
                End While
                dgViewKokoFee.DataSource = ListHFeeStrDet
                dgViewKokoFee.DataBind()
                Session("getkokorikulum") = ListHFeeStrDet
                dgViewKokoFee.SelectedIndex = -1

            Else
                'lblMsg.Visible = True
                'ErrorDescription = "Select a Fee code from the Fee list"
                'lblMsg.Text = ErrorDescription
            End If
            If dgViewKokoFee.Items.Count = 0 Then
                dgViewKokoFee.DataSource = Nothing
                dgViewKokoFee.DataBind()
                Session("getkokorikulum") = Nothing
            End If
        End If
        ListHFee = Session("getkokorikulum")
        If Not ListHFee Is Nothing Then
            If dgViewKokoFee.SelectedIndex <> -1 Then
                Dim getFTcode As String = dgViewKokoFee.DataKeys(dgViewKokoFee.SelectedIndex)
                ListHFee.RemoveAll(Function(x) x.Saftcode = getFTcode)
                Dim i As Integer = 0
                While i < ListHFee.Count
                    '  ListHFeeStrDet(i).ObjectID = i
                    i = i + 1
                End While
                dgViewKokoFee.DataSource = ListHFee
                dgViewKokoFee.DataBind()
                Session("getkokorikulum") = ListHFee
                dgViewKokoFee.SelectedIndex = -1

            Else
                'lblMsg.Visible = True
                'ErrorDescription = "Select a Fee code from the Fee list"
                'lblMsg.Text = ErrorDescription
            End If
            If dgViewKokoFee.Items.Count = 0 Then
                dgViewKokoFee.DataSource = Nothing
                dgViewKokoFee.DataBind()
                Session("getkokorikulum") = Nothing
            End If
        End If
        'Session("AddFee") = Nothing
    End Sub
    Protected Sub btnHidden_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnHidden.Click

    End Sub
    ''' <summary>
    ''' Method to Validate Before Save
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub SpaceValidation()
        'If Trim(txtKokoDesc.Text).Length = 0 Then

        '    txtKokoDesc.Text = Trim(txtKokoDesc.Text)
        '    lblMsg.Text = "Enter valid Kokorikulum Description "
        '    lblMsg.Visible = True
        '    txtKokoDesc.Focus()
        '    Exit Sub
        'End If
        'If Trim(txtCreditHours.Text).Length = 0 Then

        '    txtCreditHours.Text = Trim(txtCreditHours.Text)
        '    lblMsg.Text = "Enter valid Credit Hours "
        '    lblMsg.Visible = True
        '    txtCreditHours.Focus()
        '    Exit Sub
        'End If
        'If Trim(txtGlAmount.Text).Length = 0 Then
        '    lblMsg.Text = "Enter valid GL Account"
        '    lblMsg.Visible = True
        '    txtGlAmount.Text = Trim(txtGlAmount.Text)

        '    txtGlAmount.Focus()
        '    Exit Sub
        'End If
        onSave(Session("PageMode"))
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
            LogError.Log("Kokorikulum", "Menuname", ex.Message)
        End Try
        lblMenuName.Text = eobj.MenuName
    End Sub
    ''' <summary>
    ''' Method to Load Fields in New Mode
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub OnAdd()
        Session("PageMode") = "Add"
        PnlAdd.Visible = True
        OnClearData()
        'LoadGrid()
    End Sub
    ''' <summary>
    ''' Method to Save and Update FeeTypes
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub onSave(ByVal pageMode As String)
        lblMsg.Visible = True
        Dim txtAmount As TextBox
        Dim dgItem1 As DataGridItem
        Dim eobj As New FeeTypesEn
        Dim objfee As FeeChargesEn
        Dim bsobj As New FeeTypesBAL
        Dim LstFee As New List(Of FeeChargesEn)
        Dim RecAff As Integer
        'added by farid 23032016
        Dim LstKoko As New List(Of KokoEn)
        Dim LstKokoDetailsToSave As New List(Of KokoEn)
        Dim objacc As New StudentBAL
        Dim eobacc As New KokoEn
        Dim stlist As New List(Of KokoEn)
        Dim bsstu As New AccountsBAL
        Dim koko As New KokoEn
        Dim eobjDetails As New AccountsDetailsEn
        Dim eobDetails As New StudentEn
        Dim txtfeeamountin As TextBox
        Dim txtfeeamountout As TextBox
        Dim txtfeeinterin As TextBox
        Dim txtfeeinterout As TextBox
        Dim feeamountin As Double
        Dim gstamount As Double
        'Dim i As Integer = 0
        Dim j As Integer = 0
        If Trim(txtCreditHours.Text) = "" Then txtCreditHours.Text = 0
        eobj.FeeTypeCode = Trim(txtKokoCode.Text)
        eobj.Description = Trim(txtKokoDesc.Text)
        eobj.CreditHours = Trim(txtCreditHours.Text)

        'eobj.RecStatus = ChKStatus.Checked
        If ddlStatus.SelectedValue = 0 Then
            eobj.Status = False
        Else
            eobj.Status = True
        End If
        eobj.GLCode = Trim(txtGlAmount.Text)
        eobj.UpdatedBy = Session("User")
        'eobj.TaxId = "15"
        eobj.SCCode = "K"
        'Loop the Data Grid
        'Dim i As Integer = 0
        'For Each dgItem1 In dgView.Items
        '    txtAmount = dgItem1.FindControl("txtFeeAmount")
        '    objfee = New FeeChargesEn()
        '    objfee.FTCode = txtKokoCode.Text
        '    objfee.SCCode = dgItem1.Cells(0).Text
        '    objfee.SCDesc = dgItem1.Cells(1).Text
        '    If txtAmount.Text = "" Then
        '        objfee.FSAmount = "0"
        '    Else
        '        objfee.FSAmount = txtAmount.Text
        '    End If
        '    LstFee.Add(objfee)
        '    objfee = Nothing
        '    txtAmount = Nothing
        'Next

        'eobj.ListFeeCharges = LstFee
        If Session("AddFee") Is Nothing Then
            LstKoko = New List(Of KokoEn)
        Else
            'Session("getkokorikulum") = Nothing
            LstKoko = Session("AddFee")
            If LstKoko.Count > 0 Then

                'Dim SaftCode As String


                'lblMsg.Text = ""
                'lblMsg.Visible = True
                For Each dgItem1 In dgViewKokoFee.Items
                    'For Each eobacc In LstKoko
                    Dim liststuAll As New KokoEn
                    'Dim SaftCode As String
                    '----------------------------------------------'
                    'SaftCode = dgItem1.Cells(0).Controls
                    liststuAll = New KokoEn
                    liststuAll.Code = Trim(txtKokoCode.Text)
                    liststuAll.Saftcode = dgItem1.Cells(2).Text
                    liststuAll.FTCode = "K"
                    liststuAll.Category = dgItem1.Cells(3).Text
                    liststuAll.categoryname = "WARGANEGARA MALAYSIA"
                    'txtfeeamountin = dgItem1.Cells(4).Controls(1)
                    'txtfeeamountout = dgItem1.Cells(5).Controls(1)
                    'liststuAll.FSAmount = CDbl(txtfeeamountin.Text.Trim)
                    'liststuAll.sakodfeeamountlocalout = CDbl(txtfeeamountout.Text.Trim)
                    liststuAll.FSAmount = dgItem1.Cells(7).Text
                    liststuAll.sakodfeeamountlocalout = dgItem1.Cells(11).Text
                    liststuAll.sakodgstamountlocalin = dgItem1.Cells(6).Text
                    liststuAll.sakodfeegstamountlocalout = dgItem1.Cells(10).Text
                    liststuAll.sakod_idkoko = dgItem1.Cells(22).Text
                    LstKokoDetailsToSave.Add(liststuAll)

                    '-----------------------------------------------'
                    liststuAll = New KokoEn
                    liststuAll.Code = Trim(txtKokoCode.Text)
                    liststuAll.Saftcode = dgItem1.Cells(2).Text
                    liststuAll.FTCode = "K"
                    liststuAll.Category = dgItem1.Cells(13).Text
                    liststuAll.categoryname = "BUKAN WARGANEGARA"
                    'txtfeeinterin = dgItem1.Cells(14).Controls(1)
                    'txtfeeinterout = dgItem1.Cells(15).Controls(1)
                    'liststuAll.FSAmount = CDbl(txtfeeinterin.Text.Trim)
                    'liststuAll.sakodfeeamountlocalout = CDbl(txtfeeinterout.Text.Trim)
                    liststuAll.FSAmount = dgItem1.Cells(17).Text
                    liststuAll.sakodfeeamountlocalout = dgItem1.Cells(21).Text
                    liststuAll.sakodgstamountlocalin = dgItem1.Cells(16).Text
                    liststuAll.sakodfeegstamountlocalout = dgItem1.Cells(20).Text
                    liststuAll.sakod_idkoko = dgItem1.Cells(22).Text
                    LstKokoDetailsToSave.Add(liststuAll)
                Next
                'Next
            End If
            eobj.ListKokoCharges = LstKokoDetailsToSave
        End If
        If Session("getkokorikulum") Is Nothing Then
            stlist = New List(Of KokoEn)
        Else
            stlist = Session("getkokorikulum")
            If stlist.Count > 0 Then

                'Dim SaftCode As String


                'lblMsg.Text = ""
                'lblMsg.Visible = True
                For Each dgItem1 In dgViewKokoFee.Items
                    'For Each eobacc In LstKoko
                    Dim liststuAll As New KokoEn
                    'Dim SaftCode As String

                    '----------------------------------------------'
                    'SaftCode = dgItem1.Cells(0).Controls
                    liststuAll = New KokoEn
                    liststuAll.Code = Trim(txtKokoCode.Text)
                    liststuAll.Saftcode = dgItem1.Cells(2).Text
                    liststuAll.FTCode = "K"
                    liststuAll.Category = dgItem1.Cells(3).Text
                    liststuAll.categoryname = "WARGANEGARA MALAYSIA"
                    'txtfeeamountin = dgItem1.Cells(4).Controls(1)
                    'txtfeeamountout = dgItem1.Cells(5).Controls(1)
                    'liststuAll.FSAmount = CDbl(txtfeeamountin.Text.Trim)
                    'liststuAll.sakodfeeamountlocalout = CDbl(txtfeeamountout.Text.Trim)
                    liststuAll.FSAmount = dgItem1.Cells(7).Text
                    liststuAll.sakodfeeamountlocalout = dgItem1.Cells(11).Text
                    liststuAll.sakodgstamountlocalin = dgItem1.Cells(6).Text
                    liststuAll.sakodfeegstamountlocalout = dgItem1.Cells(10).Text
                    liststuAll.sakod_idkoko = dgItem1.Cells(22).Text
                    LstKokoDetailsToSave.Add(liststuAll)

                    '-----------------------------------------------'
                    liststuAll = New KokoEn
                    liststuAll.Code = Trim(txtKokoCode.Text)
                    liststuAll.Saftcode = dgItem1.Cells(2).Text
                    liststuAll.FTCode = "K"
                    liststuAll.Category = dgItem1.Cells(13).Text
                    liststuAll.categoryname = "BUKAN WARGANEGARA"
                    'txtfeeinterin = dgItem1.Cells(14).Controls(1)
                    'txtfeeinterout = dgItem1.Cells(15).Controls(1)
                    'liststuAll.FSAmount = CDbl(txtfeeinterin.Text.Trim)
                    'liststuAll.sakodfeeamountlocalout = CDbl(txtfeeinterout.Text.Trim)
                    liststuAll.FSAmount = dgItem1.Cells(17).Text
                    liststuAll.sakodfeeamountlocalout = dgItem1.Cells(21).Text
                    liststuAll.sakodgstamountlocalin = dgItem1.Cells(16).Text
                    liststuAll.sakodfeegstamountlocalout = dgItem1.Cells(20).Text
                    liststuAll.sakod_idkoko = dgItem1.Cells(22).Text
                    LstKokoDetailsToSave.Add(liststuAll)
                Next
                'Next
            End If
            eobj.ListKokoCharges = LstKokoDetailsToSave
        End If
        
        'If Session("getkokorikulum") Is Nothing Then
        '    stlist = New List(Of KokoEn)
        'Else
        '    stlist = Session("getkokorikulum")
        '    If stlist.Count > 0 Then

        '        'Dim SaftCode As String


        '        'lblMsg.Text = ""
        '        'lblMsg.Visible = True
        '        For Each dgItem1 In dgViewKokoFee.Items
        '            'For Each eobacc In LstKoko
        '            Dim liststuAll As New KokoEn
        '            'Dim SaftCode As String

        '            '----------------------------------------------'
        '            'SaftCode = dgItem1.Cells(0).Controls
        '            liststuAll = New KokoEn
        '            liststuAll.Code = Trim(txtKokoCode.Text)
        '            liststuAll.Saftcode = dgItem1.Cells(0).Text
        '            liststuAll.FTCode = "K"
        '            liststuAll.Category = dgItem1.Cells(1).Text
        '            liststuAll.categoryname = "WARGANEGARA MALAYSIA"
        '            txtfeeamountin = dgItem1.Cells(2).Controls(1)
        '            txtfeeamountout = dgItem1.Cells(3).Controls(1)
        '            liststuAll.FSAmount = CDbl(txtfeeamountin.Text.Trim)
        '            liststuAll.sakodfeeamountlocalout = CDbl(txtfeeamountout.Text.Trim)
        '            liststuAll.sakodgstamountlocalin = dgItem1.Cells(5).Text
        '            liststuAll.sakodfeegstamountlocalout = dgItem1.Cells(6).Text
        '            LstKokoDetailsToSave.Add(liststuAll)

        '            '-----------------------------------------------'
        '            liststuAll = New KokoEn
        '            liststuAll.Code = Trim(txtKokoCode.Text)
        '            liststuAll.Saftcode = dgItem1.Cells(0).Text
        '            liststuAll.FTCode = "K"
        '            liststuAll.Category = dgItem1.Cells(8).Text
        '            liststuAll.categoryname = "BUKAN WARGANEGARA"
        '            txtfeeinterin = dgItem1.Cells(9).Controls(1)
        '            txtfeeinterout = dgItem1.Cells(10).Controls(1)
        '            liststuAll.FSAmount = CDbl(txtfeeinterin.Text.Trim)
        '            liststuAll.sakodfeeamountlocalout = CDbl(txtfeeinterout.Text.Trim)
        '            liststuAll.sakodgstamountlocalin = dgItem1.Cells(12).Text
        '            liststuAll.sakodfeegstamountlocalout = dgItem1.Cells(13).Text
        '            LstKokoDetailsToSave.Add(liststuAll)
        '        Next
        '        'Next
        '    End If
        '    eobj.ListKokoCharges = LstKokoDetailsToSave
        'End If
        
        '        For Each obj In LstHostel
        '            Dim objHostelDetail As New HostelStrDetailsEn 'to store detail for hostel struct list
        '            Dim LstHostelAmtToSave As New List(Of HostelStrAmountEn) 'to store the amount for local and nonlocal 
        '            Dim objHostelAmt As New HostelStrAmountEn
        '            '------Clear existing data before insert--------'
        '            ' obj.lstHFeeSD = LstHostelDetailsToSave
        '            objHostelDetail.ListFeeAmount = New List(Of HostelStrAmountEn)
        '            '-----------------------------------------------'
        '            objHostelDetail = New HostelStrDetailsEn
        '            objHostelDetail.HSCode = txtHostelFeeCode.Text
        '            objHostelDetail.FTCode = obj.FTCode
        '            objHostelDetail.HDPriority = obj.Priority
        '            objHostelDetail.TaxId = obj.TaxId
        '            objHostelDetail.HDType = "H"
        '            '-----------------------------------------------'
        '            objHostelAmt = New HostelStrAmountEn
        '            objHostelAmt.FTCode = obj.FTCode
        '            objHostelAmt.SCCode = obj.LocalCategory
        '            objHostelAmt.HAAmount = obj.LocalAmount
        '            objHostelAmt.GstAmount = obj.LocalGSTAmount
        '            objHostelAmt.HSCode = obj.HostelStructureCode
        '            LstHostelAmtToSave.Add(objHostelAmt)
        '            '-----------------------------------------------'
        '            objHostelAmt = New HostelStrAmountEn
        '            objHostelAmt.FTCode = obj.FTCode
        '            objHostelAmt.SCCode = obj.NonLocalCategory
        '            objHostelAmt.HAAmount = obj.NonLocalAmount
        '            objHostelAmt.GstAmount = obj.NonLocalGSTAmount
        '            objHostelAmt.HSCode = obj.HostelStructureCode
        '            LstHostelAmtToSave.Add(objHostelAmt)
        '            '-----------------------------------------------'
        '            objHostelDetail.ListFeeAmount = LstHostelAmtToSave 'Bind for fee amount
        '            LstHostelDetailsToSave.Add(objHostelDetail)
        '            '-----------------------------------------------'
        '            'obj.lstHFeeSD = LstHostelDetailsToSave.ToList() 'Bind for details 
        '        Next
        '        eobj.lstHFeeSD = LstHostelDetailsToSave
        '    End If
        '    eobj.lstHFeeWithAmt = LstHostel
        If pageMode = "Add" Then
            Try

                RecAff = bsobj.InsertKoko(eobj)
                ErrorDescription = "Record Saved Successfully "
                lblMsg.Text = ErrorDescription

            Catch ex As Exception

                lblMsg.Text = ex.Message.ToString()
                LogError.Log("FeeType", "onSave", ex.Message)

            End Try
        ElseIf pageMode = "Edit" Then
            Try
                RecAff = bsobj.UpdateKokoList(eobj)
                ListObjects = Session("ListObj")
                ListObjects(CInt(txtRecNo.Text) - 1) = eobj
                Session("ListObj") = ListObjects
                ErrorDescription = "Record Updated Successfully "
                lblMsg.Text = ErrorDescription
            Catch ex As Exception
                lblMsg.Text = ex.Message.ToString()
                LogError.Log("FeeType", "onSave", ex.Message)
            End Try
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
    Public Sub ddlkokocodeselectedchanged()

        Dim eKolej As New KolejEn
        Dim returnKolej As New KolejEn
        Dim bKolej As New KolejBAL
        'ddlKokoCode.DataValueField = "SAKO_Code"
        'ddlKokoCode.DataValueField = Trim(ddlKokoCode.SelectedValue)
        eKolej.SAKO_Code = Trim(ddlKokoCode.SelectedValue)
        Try
            returnKolej = bKolej.GetItem(eKolej)
        Catch ex As Exception
            LogError.Log("HostelFee", "FillDropDownList", ex.Message)
            lblMsg.Text = ex.Message.ToString
        End Try
        txtKokoCode.Text = Trim(ddlKokoCode.SelectedValue)
        txtKokoDesc.Text = returnKolej.SAKO_Description

    End Sub
    ''' <summary>
    ''' Method to get the List Of FeeTypes
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub LoadListObjects()
        Dim ds As New DataSet
        Dim bobj As New FeeTypesBAL
        Dim eobj As New FeeTypesEn
        Dim recStu As Integer

        If ddlStatus.SelectedValue = -1 Then
            recStu = -1
        Else
            recStu = ddlStatus.SelectedValue
        End If
        If ddlKokoCode.SelectedValue <> "-1" Then
            eobj.FeeTypeCode = ddlKokoCode.SelectedValue
        Else
            eobj.FeeTypeCode = ""
        End If
        'eobj.FeeTypeCode = Trim(txtKokoCode.Text)

        'If txtCreditHours.Text.Length <> 0 Then
        '    eobj.CreditHours = CInt(txtCreditHours.Text)
        'End If
        'eobj.Description = Trim(txtKokoDesc.Text)
        'eobj.GLCode = Trim(txtGlAmount.Text)
        'eobj.Status = ddlStatus.SelectedValue
        'Try
        '    ListObjects = bobj.GetKokoList(eobj)
        'Catch ex As Exception
        '    LogError.Log("FeeType", "LoadListObjects", ex.Message)
        'End Try

        'If txtCreditHours.Text.Length <> 0 Then
        '    eobj.CreditHours = CInt(txtCreditHours.Text)
        'End If
        'eobj.FeeTypeCode = Trim(txtKokoCode.Text)
        'eobj.Description = Trim(txtKokoDesc.Text)
        'eobj.GLCode = Trim(txtGlAmount.Text)
        eobj.Status = recStu
        Try
            ListObjects = bobj.GetKokokurikulumList(eobj)
        Catch ex As Exception
            LogError.Log("FeeType", "LoadListObjects", ex.Message)
        End Try
        
        Session("ListObj") = ListObjects
        lblCount.Text = ListObjects.Count.ToString()
        If ListObjects.Count <> 0 Then
            DisableRecordNavigator()
            txtRecNo.Text = "1"
            pnlView.Visible = True
            PnlAdd.Visible = True
            OnMoveFirst()
            If Session("EditFlag") = True Then
                Session("PageMode") = "Edit"
                txtKokoCode.Enabled = False
                ibtnSave.Enabled = True
                ibtnSave.ImageUrl = "images/save.png"
                lblMsg.Visible = True

            Else
                ibtnSave.Enabled = False
                ibtnSave.ImageUrl = "images/gsave.png"
                Session("PageMode") = ""

            End If
        Else
            txtRecNo.Text = ""
            lblCount.Text = ""
            Session("PageMode") = "Add"
            txtKokoCode.Enabled = True
            'Clear Text Box values
            Session("ListObj") = Nothing
            DisableRecordNavigator()
            'txtKokoCode.Text = ""
            'txtKokoDesc.Text = ""
            txtCreditHours.Text = ""
            txtGlAmount.Text = ""
            LoadGrid()
            ddlStatus.SelectedValue = "1"
            If DFlag = "Delete" Then
            Else
                lblMsg.Visible = True
                ErrorDescription = "Record did not Exist"
                lblMsg.Text = ErrorDescription
                DFlag = ""
                dgViewKokoFee.Visible = False
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
    ''' Method to Delete the FeeTypes
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub onDelete()
        lblMsg.Visible = True
        If txtKokoCode.Text <> "" Then
            If lblCount.Text = "" Then lblCount.Text = 0
            If lblCount.Text > 0 Then

                Dim bsobj As New FeeTypesBAL
                Dim eobj As New FeeTypesEn
                Dim bsobb As New FeeTypesBAL
                Dim liststuAll As New List(Of KokoEn)
                lblMsg.Visible = True
                eobj.FeeTypeCode = txtKokoCode.Text
                'bsobb.GetKokoStudent(eobj)
                liststuAll = bsobb.GetKokoStudent(eobj)

                If liststuAll.Count = 0 Then
                    Try
                        bsobj.DeleteKoko(eobj)
                        ListObjects = Session("ListObj")
                        ListObjects.RemoveAt(CInt(txtRecNo.Text) - 1)
                        lblCount.Text = lblCount.Text - 1
                        Session("ListObj") = ListObjects
                        lblMsg.Visible = True
                        ErrorDescription = "Record Deleted Successfully "
                        lblMsg.Text = ErrorDescription
                    Catch ex As Exception
                        lblMsg.Text = ex.Message.ToString()
                        LogError.Log("FeeType", "onDelete", ex.Message)

                    End Try
                Else
                    ErrorDescription = "Record Already In Use"
                    lblMsg.Text = ErrorDescription
                    Exit Sub
                End If
                txtKokoCode.Text = ""
                txtKokoDesc.Text = ""
                txtCreditHours.Text = ""
                txtGlAmount.Text = ""
                ddlStatus.SelectedValue = "1"
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
            LogError.Log("FeeType", "LoadUserRights", ex.Message)
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
        ibnPrint.Enabled = eobj.IsPrint
        If eobj.IsPrint = True Then
            ibnPrint.Enabled = True
            ibnPrint.ImageUrl = "images/print.png"
            ibnPrint.ToolTip = "Print"
        Else
            ibnPrint.Enabled = False
            ibnPrint.ImageUrl = "images/gprint.png"
            ibnPrint.ToolTip = "Access Denied"
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
    ''' Method to Load Grid With FeeCharges
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub LoadGrid()
        'Dim ds As New DataSet
        'Dim bobj As New StudentCategoryBAL
        'Dim eobj As New StudentCategoryEn
        'Dim eobjStuCat As StudentCategoryEn
        'Dim eobjFeeCharge As FeeChargesEn

        'Dim ListStudentCategory As New List(Of StudentCategoryEn)
        'Dim ListFeeCharge As New List(Of FeeChargesEn)
        'Dim i As Integer = 0
        'Dim j As Integer = 0
        ''eobj.Code = Trim(txtKokoCode.Text)
        'eobj.StudentCategoryCode = ""
        'eobj.Description = ""
        'eobj.Status = True
        'Try
        '    ListStudentCategory = bobj.GetStudentCategoryList(eobj)
        'Catch ex As Exception
        '    LogError.Log("FeeType", "LoadGrid", ex.Message)
        'End Try

        ''Loading feeCharges
        'While i < ListStudentCategory.Count
        '    eobjStuCat = ListStudentCategory(i)
        '    eobjFeeCharge = New FeeChargesEn
        '    eobjFeeCharge.SCCode = eobjStuCat.StudentCategoryCode
        '    eobjFeeCharge.SCDesc = eobjStuCat.Description
        '    eobjFeeCharge.FSAmount = 0
        '    ListFeeCharge.Add(eobjFeeCharge)
        '    eobjFeeCharge = Nothing
        '    eobjStuCat = Nothing
        '    i = i + 1
        'End While
        'pnlView.Visible = True
        'PnlAdd.Visible = True
        'Session("ListFeeCharge") = ListFeeCharge
        ''dgViewKokoFee.DataSource = ListFeeCharge
        ''dgViewKokoFee.DataBind()
        'dgView.DataSource = ListFeeCharge
        'dgView.DataBind()
        'Dim getkoko As New List(Of KokoEn)
        'Dim objacc As New StudentBAL
        'Dim eobacc As New KokoEn
        'Dim stlist As New List(Of KokoEn)
        'Dim bsstu As New AccountsBAL
        'Dim stuen As New StudentEn
        'Dim eobjDetails As New AccountsDetailsEn
        'Dim eobDetails As New KokoEn
        'Dim txtfeeamountin As TextBox
        'Dim txtfeeamountout As TextBox
        'Dim txtgstamountin As TextBox
        'Dim txtgstamountout As TextBox
        'Dim feeamountin As Double
        'Dim gstamount As Double
        'Dim feeamountout As Double
        'Dim gstamountout As Double
        'Dim i As Integer = 0
        'Dim j As Integer = 0
        'Session("ListObj") = ListObjects
        'eobacc.Code = Trim(txtKokoCode.Text)
        'Try
        '    getkoko = objacc.GetKokoDetails(eobacc)
        'Catch ex As Exception
        '    LogError.Log("kokorikulum", "LoadGrid()", ex.Message)
        'End Try
        'pnlView.Visible = True
        'PnlAdd.Visible = True
        'dgViewKokoFee.Visible = True
        'dgViewKokoFee.DataSource = getkoko
        'dgViewKokoFee.DataBind()

        'If getkoko.Count > j Then
        '    For Each eobjFt In getkoko
        '        If Not getkoko.Any(Function(x) x.Saftcode = eobjFt.Saftcode) Then
        '            If eobjFt.Category = "W" Then
        '                eobDetails = New KokoEn
        '                eobDetails.Saftcode = eobjFt.Saftcode
        '                eobDetails.Category = "W"

        '                eobDetails.sakodfeeamountlocalin = eobjFt.sakodfeeamountlocalin
        '                eobDetails.sakodfeeamountlocalout = eobjFt.sakodfeeamountlocalout
        '                eobDetails.sakodgstamountlocalin = eobjFt.sakodgstamountlocalin
        '                eobDetails.sakodfeegstamountlocalout = eobjFt.sakodgstamountlocalin
        '                eobDetails.Local_TempAmount = eobjFt.sakodfeeamountlocalin - eobjFt.sakodgstamountlocalin

        '            Else
        '                'eobDetails = New KokoEn
        '                eobDetails.NonLocalCategory = "BW"
        '                eobDetails.sakodfeeamountinterin = eobjFt.sakodfeeamountinterin
        '                eobDetails.sakodfeeamountinterout = eobjFt.sakodfeeamountinterout
        '                eobDetails.sakodgstamountinterin = eobjFt.sakodgstamountinterin
        '                eobDetails.sakodgstamountinterout = eobjFt.sakodgstamountinterout
        '                eobDetails.Inter_TempAmount = eobjFt.sakodfeeamountinterin - eobjFt.sakodfeeamountinterout
        '                'ListHFeeStrDet.Add(eobDetails)
        '            End If
        '        End If
        '        'getkoko.Add(eobDetails)
        '    Next
        '    j = j + 1
        'End If

        'Session("getkoko") = getkoko

        'If getkoko.Count > 0 Then
        '    dgViewKokoFee.DataSource = getkoko
        '    dgViewKokoFee.DataBind()
        '    If dgViewKokoFee.Items.Count > 0 Then
        '        dgViewKokoFee.SelectedIndex = dgViewKokoFee.Items.Count - 1
        '    End If
        'End If
        'Session("eobj") = Nothing
        'dgViewKokoFee.DataSource = getkoko
        'dgViewKokoFee.DataBind()

        'While j < getkoko.Count


        '    For Each dgItem1 In dgViewKokoFee.Items
        '        'If dgItem1.Cells(0).Text < 1 Then
        '        'If liststuAll(j).Saftcode = liststuAll(j).Saftcode = 1 Then
        '        If getkoko(j).Category = "W" Then
        '            'dgItem1.Cells(0).Text = liststuAll(j).Saftcode
        '            dgItem1.Cells(1).Text = "W"
        '            txtfeeamountin = dgItem1.Cells(2).Controls(1)
        '            txtfeeamountout = dgItem1.Cells(3).Controls(1)
        '            dgItem1.Cells(6).Text = getkoko(j).sakodgstamountlocalin
        '            dgItem1.Cells(5).Text = getkoko(j).sakodgstamountlocalin
        '            'gstamount = getkoko(j).sakodgstamountlocalin
        '            feeamountin = getkoko(j).sakodfeeamountlocalin
        '            feeamountout = getkoko(j).sakodfeeamountlocalout
        '            'gstamountout = getkoko(j).sakodfeegstamountlocalout
        '            txtfeeamountin.Text = String.Format("{0:F}", feeamountin)
        '            txtfeeamountout.Text = String.Format("{0:F}", feeamountin)
        '            'txtgstamountout.Text = String.Format("{0:F}", gstamount)
        '            'txtgstamountin.Text = String.Format("{0:F}", gstamount)
        '            dgItem1.Cells(4).Text = feeamountin - dgItem1.Cells(6).Text
        '        ElseIf getkoko(j).Category = "BW" Then
        '            'dgItem1.Cells(0).Text = liststuAll(j).Saftcode
        '            dgItem1.Cells(8).Text = "BW"
        '            txtfeeamountin = dgItem1.Cells(9).Controls(1)
        '            txtfeeamountout = dgItem1.Cells(10).Controls(1)
        '            dgItem1.Cells(13).Text = getkoko(j).sakodfeegstamountlocalout
        '            dgItem1.Cells(12).Text = getkoko(j).sakodfeegstamountlocalout
        '            'gstamount = getkoko(j).sakodgstamountlocalin
        '            feeamountin = getkoko(j).sakodfeeamountlocalin
        '            feeamountout = getkoko(j).sakodfeeamountlocalout
        '            'gstamountout = getkoko(j).sakodfeegstamountlocalout
        '            txtfeeamountin.Text = String.Format("{0:F}", feeamountin)
        '            txtfeeamountout.Text = String.Format("{0:F}", feeamountin)
        '            'txtgstamountout.Text = String.Format("{0:F}", gstamount)
        '            'txtgstamountin.Text = String.Format("{0:F}", gstamount)
        '            dgItem1.Cells(11).Text = feeamountin - dgItem1.Cells(12).Text
        '        End If
        '        Exit For

        '        'Else
        '        'End If
        '    Next
        '    j = j + 1
        '    'LoadTotals()
        'End While
    End Sub
    ''' <summary>
    ''' Method to Clear the Field Values
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub OnClearData()
        Session("AddFee") = Nothing
        Session("PageMode") = "Add"
        txtKokoCode.Enabled = True
        'Clear Text Box values
        Session("ListObj") = Nothing
        DisableRecordNavigator()
        txtKokoCode.Text = ""
        txtKokoDesc.Text = ""
        txtCreditHours.Text = ""
        txtGlAmount.Text = ""
        LoadGrid()
        lblMsg.Text = ""
        ddlStatus.SelectedValue = "1"
        ddlKokoCode.SelectedValue = "-1"
        dgViewKokoFee.Visible = False
        lblDesc.Text = ""
        lblDesc.Visible = False
        imgGL.Visible = False
    End Sub
#End Region
    Protected Sub Check_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Check.Click
        'varaible declaration
        Dim result As Boolean = False
        Dim _GLCode As String = txtGlAmount.Text
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
                    End If
                Else
                    lblMsg.Text = "Invalid GLCode"
                    imgGL.Visible = True
                    imgGL.ImageUrl = "~/images/cross.png"
                End If
                'Check GLCODE in CF - Ended
            End If
            'Check Empty GLCode - Starting

            'imgGL.Visible = False
            'Dim oleCF As New OleDb.OleDbConnection
            'Dim dsCF As New DataSet
            'Dim strsql As String
            'Dim olCFEn As New ConnectionEn
            'Dim olCFBal As New ConnectionBAL

            'olCFEn.Code = "I"
            'olCFEn = olCFBal.GetConnectionString(olCFEn)
            'oleCF.ConnectionString = olCFEn.ConnectionStrings
            'strsql = "select glac_account, glac_desc from gl_account where  glac_account = '" & txtGlAmount.Text & "'"
            'Dim daCF As New OleDb.OleDbDataAdapter(strsql, oleCF)
            'daCF.Fill(dsCF, "gl_account")

            'If dsCF.Tables("gl_account").Rows.Count > 0 Then
            '    imgGL.ImageUrl = "~/images/check.png"

            'Else
            '    imgGL.ImageUrl = "~/images/cross.png"

            'End If
            'imgGL.Visible = True

        Catch ex As Exception
            Throw ex
        End Try

    End Sub
    Protected Sub Chk_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)

        Dim chk As CheckBox
        Dim dgItem1 As DataGridItem
        Dim getkoku As New List(Of KokoEn)
        Dim koku As New KokoEn
        If Not Session("getkokorikulum") Is Nothing Then
            'Session("getkokorikulum") = getkoku
            For Each dgItem1 In dgViewKokoFee.Items
                chk = dgItem1.Cells(0).Controls(1)
                If chk.Checked = True Then
                    'If ibtnRemoveFee.OnClientClick = True Then
                    Dim Saftcode As String = dgViewKokoFee.DataKeys(dgViewKokoFee.SelectedIndex).ToString()
                    getkoku.RemoveAll(Function(x) x.Saftcode = Saftcode)
                    koku.Saftcode = dgItem1.Cells(1).Text
                    'Try
                    '    getkoko = objacc.GetKokoDetails(eobacc)
                    'Catch ex As Exception
                    '    LogError.Log("kokorikulum", "LoadGrid()", ex.Message)
                    'End Try
                    'Session("getkoko") = getkoko
                End If
            Next
        End If

        dgViewKokoFee.DataSource = getkoku
        dgViewKokoFee.DataBind()
    End Sub
    Protected Sub txtGlAmount_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtGlAmount.TextChanged
        If imgGL.Visible Then
            imgGL.Visible = False
        End If
    End Sub
#Region "GST Calculation - Starting "

    Public Function GSTActAmt(ByVal Amt As Double, ByVal gst As Double, ByVal TaxId As Integer, ByVal ReferenceCode As String) As String
        Dim ActAmout As Double = 0
        Dim TaxMode As Integer = _GstSetupDal.GetGstDetails(MaxGeneric.clsGeneric.NullToInteger(TaxId)).Tables(0).Rows(0)(3).ToString()
        If (TaxMode = Generic._TaxMode.Inclusive) Then
            ActAmout = MaxGeneric.clsGeneric.NullToDecimal(Amt) - gst
        ElseIf (TaxMode = Generic._TaxMode.Exclusive) Then
            ActAmout = MaxGeneric.clsGeneric.NullToDecimal(Amt) - gst
        End If
        Return String.Format("{0:F}", ActAmout)
    End Function

    Public Function GSTFeeAmt(ByVal FeeAmt As Double, ByVal gst As Double, ByVal TaxId As Integer) As String
        Dim TaxMode As Integer = 0
        Try
            TaxMode = _GstSetupDal.GetGstDetails(MaxGeneric.clsGeneric.NullToInteger(TaxId)).Tables(0).Rows(0)(3).ToString()
        Catch

        End Try
        Dim FeeAmout As Double = 0
        If (TaxMode = Generic._TaxMode.Inclusive) Then
            FeeAmout = MaxGeneric.clsGeneric.NullToDecimal(FeeAmt)
        ElseIf (TaxMode = Generic._TaxMode.Exclusive) Then
            FeeAmout = MaxGeneric.clsGeneric.NullToDecimal(FeeAmt) - gst
        End If
        Return FeeAmout
    End Function

    'Protected Sub txtFeeAmount_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs)
    '    Dim txtFeeAmount As TextBox
    '    txtFeeAmount = sender
    '    txtFeeAmount.Text = String.Format("{0:F}", CDbl(txtFeeAmount.Text))
    'End Sub
    Protected Sub txtFeeAmountLocalIn_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim dgitem As DataGridItem
        Dim txtFeeAmount As TextBox
        Dim addfee As New List(Of KokoEn)
        Dim GetGST As DataSet
        Dim TaxMode As String
        txtFeeAmount = sender
        txtFeeAmount.Text = String.Format("{0:F}", CDbl(txtFeeAmount.Text))
        Dim gstamountin As Double, GSTAmt As Double, totalamt As Double
        'Dim dgitem As DataGridItem
        Dim i As Integer = 0
        'Dim dgitem As DataGridItem
        Dim _txtFeeAmount As TextBox
        Dim actual As Double, Gst As Double, total As Double
        'varaible declaration
        Dim FeeAmount As Double, ActAmout As Double
        'Dim TaxMode As String
        Try

            TaxMode = _GstSetupDal.GetGstDetails(MaxGeneric.clsGeneric.NullToInteger(0)).Tables(0).Rows(0)(3).ToString()

            'GST Calculation - Stating
            For Each dgitem In dgViewKokoFee.Items
                _txtFeeAmount = dgitem.FindControl("txtFeeAmountLocalIn")
                '_txtGSTAmount = dgitem.FindControl("sakodgstamountlocalin")
                '_txtTotFeeAmount = dgitem.FindControl("txtTotFeeAmount")
                'gstamountin = dgitem.Cells(8).Text
                Dim TaxId As Integer = MaxGeneric.clsGeneric.NullToInteger(dgitem.Cells(22).Text)
                FeeAmount = _txtFeeAmount.Text
                'FeeAmount = _GstSetupDal.GetGstAmount(0, MaxGeneric.clsGeneric.NullToDecimal(TaxId))
                If Not TaxId = 0 Then
                    GetGST = _GstSetupDal.GetGstDetails(MaxGeneric.clsGeneric.NullToInteger(TaxId))
                Else
                    Throw New Exception("TaxCode Missing")
                End If
                TaxMode = GetGST.Tables(0).Rows(0)(3).ToString()

                If Not TaxId = 0 Then
                    GSTAmt = _GstSetupDal.GetGstAmount(TaxId, MaxGeneric.clsGeneric.NullToDecimal(FeeAmount))
                Else
                    Throw New Exception("TaxCode Missing")
                End If
                If (TaxMode = Generic._TaxMode.Inclusive) Then
                    ActAmout = MaxGeneric.clsGeneric.NullToDecimal(FeeAmount) - GSTAmt
                    actual = String.Format("{0:F}", CDbl(ActAmout))
                    Gst = String.Format("{0:F}", CDbl(GSTAmt))
                    totalamt = actual + Gst
                    total = String.Format("{0:F}", CDbl(totalamt))
                    dgitem.Cells(5).Text = actual
                    dgitem.Cells(6).Text = Gst
                    dgitem.Cells(7).Text = total
                ElseIf (TaxMode = Generic._TaxMode.Exclusive) Then
                    ActAmout = FeeAmount
                    'ActAmout = MaxGeneric.clsGeneric.NullToDecimal(FeeAmount) + GSTAmt
                    actual = String.Format("{0:F}", CDbl(ActAmout))
                    Gst = String.Format("{0:F}", CDbl(GSTAmt))
                    totalamt = actual + Gst
                    total = String.Format("{0:F}", CDbl(totalamt))
                    dgitem.Cells(5).Text = actual
                    dgitem.Cells(6).Text = Gst
                    dgitem.Cells(7).Text = total
                End If
                
                '_txtActFeeAmount.Text = String.Format("{0:F}", ActAmout)
                '_txtGSTAmount.Text = String.Format("{0:F}", GSTAmt)
                '_txtTotFeeAmount.Text = String.Format("{0:F}", FeeAmount)
                'sumAmt = sumAmt + FeeAmount
            Next
            'txtTotal.Text = sumAmt

            'GST Calculation - Ended
        Catch ex As Exception
            If ex.Message = "There is no row at position 0." Then
                lblMsg.Visible = True
                lblMsg.Text = "Required Tax Type"
            End If
            Call MaxModule.Helper.LogError(ex.Message)
        End Try
    End Sub
    Protected Sub txtFeeAmountLocalOut_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim dgitem As DataGridItem
        Dim txtFeeAmount As TextBox
        Dim addfee As New List(Of KokoEn)
        Dim GetGST As DataSet
        Dim TaxMode As String
        txtFeeAmount = sender
        txtFeeAmount.Text = String.Format("{0:F}", CDbl(txtFeeAmount.Text))
        Dim gstamountout As Double, GSTAmt As Double, totalamt As Double
        'Dim dgitem As DataGridItem
        Dim i As Integer = 0
        'Dim dgitem As DataGridItem
        Dim _txtFeeAmount As TextBox
        Dim actual As Double, Gst As Double, total As Double
        'varaible declaration
        Dim FeeAmount As Double, ActAmout As Double
        'Dim TaxMode As String
        Try

            TaxMode = _GstSetupDal.GetGstDetails(MaxGeneric.clsGeneric.NullToInteger(0)).Tables(0).Rows(0)(3).ToString()

            'GST Calculation - Stating
            For Each dgitem In dgViewKokoFee.Items
                _txtFeeAmount = dgitem.FindControl("txtFeeAmountLocalOut")
                gstamountout = dgitem.Cells(9).Text
                Dim TaxId As Integer = MaxGeneric.clsGeneric.NullToInteger(dgitem.Cells(22).Text)
                FeeAmount = _txtFeeAmount.Text
                'FeeAmount = _GstSetupDal.GetGstAmount(0, MaxGeneric.clsGeneric.NullToDecimal(TaxId))
                If Not TaxId = 0 Then
                    GetGST = _GstSetupDal.GetGstDetails(MaxGeneric.clsGeneric.NullToInteger(TaxId))
                Else
                    Throw New Exception("TaxCode Missing")
                End If
                TaxMode = GetGST.Tables(0).Rows(0)(3).ToString()

                If Not TaxId = 0 Then
                    GSTAmt = _GstSetupDal.GetGstAmount(TaxId, MaxGeneric.clsGeneric.NullToDecimal(FeeAmount))
                Else
                    Throw New Exception("TaxCode Missing")
                End If
                If (TaxMode = Generic._TaxMode.Inclusive) Then
                    ActAmout = MaxGeneric.clsGeneric.NullToDecimal(FeeAmount) - GSTAmt
                    actual = String.Format("{0:F}", CDbl(ActAmout))
                    Gst = String.Format("{0:F}", CDbl(GSTAmt))
                    totalamt = actual + Gst
                    total = String.Format("{0:F}", CDbl(totalamt))
                    dgitem.Cells(9).Text = actual
                    dgitem.Cells(10).Text = Gst
                    dgitem.Cells(11).Text = total
                ElseIf (TaxMode = Generic._TaxMode.Exclusive) Then
                    ActAmout = FeeAmount
                    'ActAmout = MaxGeneric.clsGeneric.NullToDecimal(FeeAmount) + GSTAmt
                    actual = String.Format("{0:F}", CDbl(ActAmout))
                    Gst = String.Format("{0:F}", CDbl(GSTAmt))
                    totalamt = actual + Gst
                    total = String.Format("{0:F}", CDbl(totalamt))
                    dgitem.Cells(9).Text = actual
                    dgitem.Cells(10).Text = Gst
                    dgitem.Cells(11).Text = total
                End If
                
            Next
            'txtTotal.Text = sumAmt

            'GST Calculation - Ended
        Catch ex As Exception
            If ex.Message = "There is no row at position 0." Then
                lblMsg.Visible = True
                lblMsg.Text = "Required Tax Type"
            End If
            Call MaxModule.Helper.LogError(ex.Message)
        End Try
    End Sub
    Protected Sub txtFeeAmountInterIn_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim dgitem As DataGridItem
        Dim txtFeeAmount As TextBox
        Dim addfee As New List(Of KokoEn)
        Dim GetGST As DataSet
        Dim TaxMode As String
        txtFeeAmount = sender
        txtFeeAmount.Text = String.Format("{0:F}", CDbl(txtFeeAmount.Text))
        Dim gstamountin As Double, GSTAmt As Double, totalamt As Double
        'Dim dgitem As DataGridItem
        Dim i As Integer = 0
        'Dim dgitem As DataGridItem
        Dim _txtFeeAmount As TextBox
        Dim actual As Double, Gst As Double, total As Double
        'varaible declaration
        Dim FeeAmount As Double, ActAmout As Double
        'Dim TaxMode As String
        Try

            TaxMode = _GstSetupDal.GetGstDetails(MaxGeneric.clsGeneric.NullToInteger(0)).Tables(0).Rows(0)(3).ToString()

            'GST Calculation - Stating
            For Each dgitem In dgViewKokoFee.Items
                _txtFeeAmount = dgitem.FindControl("txtFeeAmountInterIn")
                gstamountin = dgitem.Cells(16).Text
                Dim TaxId As Integer = MaxGeneric.clsGeneric.NullToInteger(dgitem.Cells(22).Text)
                FeeAmount = _txtFeeAmount.Text
                'FeeAmount = _GstSetupDal.GetGstAmount(0, MaxGeneric.clsGeneric.NullToDecimal(TaxId))
                If Not TaxId = 0 Then
                    GetGST = _GstSetupDal.GetGstDetails(MaxGeneric.clsGeneric.NullToInteger(TaxId))
                Else
                    Throw New Exception("TaxCode Missing")
                End If
                TaxMode = GetGST.Tables(0).Rows(0)(3).ToString()

                If Not TaxId = 0 Then
                    GSTAmt = _GstSetupDal.GetGstAmount(TaxId, MaxGeneric.clsGeneric.NullToDecimal(FeeAmount))
                Else
                    Throw New Exception("TaxCode Missing")
                End If
                If (TaxMode = Generic._TaxMode.Inclusive) Then
                    ActAmout = MaxGeneric.clsGeneric.NullToDecimal(FeeAmount) - GSTAmt
                    actual = String.Format("{0:F}", CDbl(ActAmout))
                    Gst = String.Format("{0:F}", CDbl(GSTAmt))
                    totalamt = actual + Gst
                    total = String.Format("{0:F}", CDbl(totalamt))
                    dgitem.Cells(15).Text = actual
                    dgitem.Cells(16).Text = Gst
                    dgitem.Cells(17).Text = total
                ElseIf (TaxMode = Generic._TaxMode.Exclusive) Then
                    ActAmout = FeeAmount
                    'ActAmout = MaxGeneric.clsGeneric.NullToDecimal(FeeAmount) + GSTAmt
                    actual = String.Format("{0:F}", CDbl(ActAmout))
                    Gst = String.Format("{0:F}", CDbl(GSTAmt))
                    totalamt = actual + Gst
                    total = String.Format("{0:F}", CDbl(totalamt))
                    dgitem.Cells(15).Text = actual
                    dgitem.Cells(16).Text = Gst
                    dgitem.Cells(17).Text = total
                End If
                
            Next
            'txtTotal.Text = sumAmt

            'GST Calculation - Ended
        Catch ex As Exception
            If ex.Message = "There is no row at position 0." Then
                lblMsg.Visible = True
                lblMsg.Text = "Required Tax Type"
            End If
            Call MaxModule.Helper.LogError(ex.Message)
        End Try
    End Sub
    Protected Sub txtFeeAmountInterOut_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs)
       Dim dgitem As DataGridItem
        Dim txtFeeAmount As TextBox
        Dim addfee As New List(Of KokoEn)
        Dim GetGST As DataSet
        Dim TaxMode As String
        txtFeeAmount = sender
        txtFeeAmount.Text = String.Format("{0:F}", CDbl(txtFeeAmount.Text))
        Dim gstamountout As Double, GSTAmt As Double, totalamt As Double
        'Dim dgitem As DataGridItem
        Dim i As Integer = 0
        'Dim dgitem As DataGridItem
        Dim _txtFeeAmount As TextBox
        Dim actual As Double, Gst As Double, total As Double
        'varaible declaration
        Dim FeeAmount As Double, ActAmout As Double
        'Dim TaxMode As String
        Try

            TaxMode = _GstSetupDal.GetGstDetails(MaxGeneric.clsGeneric.NullToInteger(0)).Tables(0).Rows(0)(3).ToString()

            'GST Calculation - Stating
            For Each dgitem In dgViewKokoFee.Items
                _txtFeeAmount = dgitem.FindControl("txtFeeAmountInterOut")
                gstamountout = dgitem.Cells(17).Text
                Dim TaxId As Integer = MaxGeneric.clsGeneric.NullToInteger(dgitem.Cells(22).Text)
                FeeAmount = _txtFeeAmount.Text
                'FeeAmount = _GstSetupDal.GetGstAmount(0, MaxGeneric.clsGeneric.NullToDecimal(TaxId))
                If Not TaxId = 0 Then
                    GetGST = _GstSetupDal.GetGstDetails(MaxGeneric.clsGeneric.NullToInteger(TaxId))
                Else
                    Throw New Exception("TaxCode Missing")
                End If
                TaxMode = GetGST.Tables(0).Rows(0)(3).ToString()

                If Not TaxId = 0 Then
                    GSTAmt = _GstSetupDal.GetGstAmount(TaxId, MaxGeneric.clsGeneric.NullToDecimal(FeeAmount))
                Else
                    Throw New Exception("TaxCode Missing")
                End If
                If (TaxMode = Generic._TaxMode.Inclusive) Then
                    ActAmout = MaxGeneric.clsGeneric.NullToDecimal(FeeAmount) - GSTAmt
                    actual = String.Format("{0:F}", CDbl(ActAmout))
                    Gst = String.Format("{0:F}", CDbl(GSTAmt))
                    totalamt = actual + Gst
                    total = String.Format("{0:F}", CDbl(totalamt))
                    dgitem.Cells(19).Text = actual
                    dgitem.Cells(20).Text = Gst
                    dgitem.Cells(21).Text = total
                ElseIf (TaxMode = Generic._TaxMode.Exclusive) Then
                    ActAmout = FeeAmount
                    'ActAmout = MaxGeneric.clsGeneric.NullToDecimal(FeeAmount) + GSTAmt
                    actual = String.Format("{0:F}", CDbl(ActAmout))
                    Gst = String.Format("{0:F}", CDbl(GSTAmt))
                    totalamt = actual + Gst
                    total = String.Format("{0:F}", CDbl(totalamt))
                    dgitem.Cells(19).Text = actual
                    dgitem.Cells(20).Text = Gst
                    dgitem.Cells(21).Text = total
                End If
                
            Next
            'txtTotal.Text = sumAmt

            'GST Calculation - Ended
        Catch ex As Exception
            If ex.Message = "There is no row at position 0." Then
                lblMsg.Visible = True
                lblMsg.Text = "Required Tax Type"
            End If
            Call MaxModule.Helper.LogError(ex.Message)
        End Try
    End Sub
#End Region
End Class
