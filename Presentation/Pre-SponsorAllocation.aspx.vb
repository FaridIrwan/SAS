Imports HTS.SAS.Entities
Imports HTS.SAS.BusinessObjects
Imports System.Data
Imports System.Collections.Generic
Imports System.IO
Imports System.IO.FileSystemEventArgs
Imports System.Diagnostics
Imports AutoPayModule
Imports System.Globalization
Imports System.Web.Services

Partial Class SponsorAllocation
    Inherits System.Web.UI.Page
    Dim ListObjects As List(Of AccountsEn)
    Dim ListObjectsStudent As List(Of StudentEn)

    Dim CFlag As String
    Dim DFlag As String
    Dim Aflag As String
    Dim tFlag As String
    Dim AutoNo As Boolean
    Dim PAidAmount As Double
    Private dalc As Object
    Private StudentMNo As String
    Private SemNo As String
    Private totalStuamt As Double = 0
    Dim GBFormat As System.Globalization.CultureInfo
    Private SaveLocation As String = Server.MapPath("data")
    Private FILE_NAME As String = "\BIMB_" + Format(Date.Today.ToLocalTime, "dd_MM_yyyy") + ".txt"
    Private ErrorDescription As String
    ''Private LogErrors As LogError

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        lblMsg.Text = ""
        If Not IsPostBack() Then
            Session("selectStu") = Nothing
            Session("Menuid") = Request.QueryString("Menuid")
            'Adding Validation for all button
            ibtnSave.Attributes.Add("onclick", "return Validate()")
            ibtnDelete.Attributes.Add("onclick", "return getconfirm()")
            ibtnPosting.Attributes.Add("onclick", "return getpostconfirm()")
            txtAllocateAmount.Attributes.Add("onKeypress", "return checknValue()")
            txtBDate.Attributes.Add("OnKeyup", "return CheckBatchDate()")
            txtPaymentDate.Attributes.Add("OnKeyup", "return CheckTransDate()")
            txtchequeDate.Attributes.Add("OnKeyup", "return CheckChequeDate()")
            ibtnBDate.Attributes.Add("onClick", "return BDate()")
            btnAllocate.Attributes.Add("onClick", "return CheckAllocate()")
            ibtnPaymentDate.Attributes.Add("onClick", "return getpaymentDate()")
            ibtnChequeDate.Attributes.Add("onClick", "return getChequeDate()")
            'Loading User Rights
            LoadUserRights()
            'Clear all Sessions
            Session("PageMode") = ""
            Session("AddBank") = Nothing
            Session("liststu") = Nothing
            Session("spnObj") = Nothing
            Session("stualloc") = Nothing
            Session("stuupload") = Nothing
            Session("ReceiptFor") = "St"
            Session("Scode") = Nothing
            Session("eobjspn") = Nothing

            Session("PageMode") = "Add"

            DisableRecordNavigator()
            txtRecNo.Attributes.Add("OnKeyup", "return geterr()")
            'load PageName
            Menuname(CInt(Request.QueryString("Menuid")))
            OnLoadItem()
            'Date Formatting
            dates()

            lblMsg.Text = ""
            btnupload.Attributes.Add("onclick", "new_window=window.open('FileSponsor.aspx','Hanodale','width=470,height=200,resizable=0');new_window.focus();")
            IdtnStud.Attributes.Add("onclick", "new_window=window.open('AddMulStudents.aspx','Hanodale','width=600,height=580,resizable=0');new_window.focus();")
            'addPayMode()
            lblMsg.Text = ""
            Session("KodUniversiti") = Nothing
            Session("KumpulanPelajar") = Nothing
            Session("TarikhProses") = Nothing
            Session("KodBank") = Nothing
            Session("fileSponsor") = Nothing
            Session("fileType") = Nothing
            Session("Err") = Nothing
            trFileGen.Visible = False
            ibtnYesNo.Visible = False
        End If
        ' Import Sponsor Data from Excel 
        If  Not Session("fileSponsor") Is Nothing And Session("fileType") = "text" Then
            ListObjectsStudent = readTextFile(Session("fileSponsor").ToString())
            LoadStudentsTemplates(ListObjectsStudent)
            System.IO.File.Delete(Session("fileSponsor"))
            Session("fileSponsor") = Nothing
            Session("fileType") = Nothing
            If Session("errStulist") <> Nothing Then
                lblMsg.Visible = True
                lblMsg.Text = "Senarai No. Kad Pengenalan Pelajar Yang Tiada Didalam Simpanan SAS:" & Session("errStulist")
            End If
        End If
        If Not Session("spnObj") Is Nothing Then
            addSpnCode()
            btnupload.Enabled = True
        End If
        If Not Session("liststu") Is Nothing Then
            addSelectStudent()
        End If
        If Not Session("File1") Is Nothing Then
            uploadData()
        End If

        If Not Request.QueryString("BatchCode") Is Nothing Then
            Session("Menuid") = Request.QueryString("Menuid")
            txtRecNo.Text = Request.QueryString("BatchCode")
            DirectCast(Master.FindControl("Panel1"), System.Web.UI.WebControls.Panel).Visible = False
            DirectCast(Master.FindControl("td"), System.Web.UI.HtmlControls.HtmlTableCell).Visible = False
            Panel1.Visible = False
            OnSearchOthers()
        End If
    End Sub
    Protected Sub ibtnSave_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ibtnSave.Click
        If Trim(txtDesc.Text).Length = 0 Then
            lblMsg.Text = "Enter Valid Description"
            lblMsg.Visible = True
            Exit Sub
        End If
        SpaceValidation()
        onSave()
        setDateFormat()
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
    Protected Sub ibtnView_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ibtnView.Click
        LoadUserRights()
        Session("loaddata") = "View"
        If lblCount.Text <> "" Then
            If CInt(lblCount.Text) > 0 Then
                onAdd()
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
    Protected Sub ibtnPosting_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ibtnPosting.Click
        If lblStatus.Value = "Ready" Then
            totalall()
            If CDbl(txtAllocateAmount.Text) = CDbl(txtAfterBalance.Text) Then
                SpaceValidation()
                onPost()
                setDateFormat()
            Else
                lblMsg.Text = "Check the Total Allocate Amount"
                lblMsg.Visible = True
            End If

        ElseIf lblStatus.Value = "New" Then
            lblMsg.Text = "Record not Ready for Posting"
            lblMsg.Visible = True
        ElseIf lblStatus.Value = "Posted" Then
            'lblMsg.Text = "Record Already Posted"
            lblMsg.Visible = True
        End If
    End Sub
    Protected Sub ibtnCancel_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ibtnCancel.Click
        LoadUserRights()
        onAdd()
    End Sub
    Protected Sub ibtnNew_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ibtnNew.Click
        onAdd()
    End Sub
    Protected Sub ibtnYesNo_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ibtnYesNo.Click
        System.IO.File.Delete(SaveLocation & FILE_NAME)
        generateFileToBank(SaveLocation & FILE_NAME, sender, e)
        ibtnYesNo.Visible = False
    End Sub
    Protected Sub dgView_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dgView.ItemDataBound
        Dim txtAmount As New TextBox
        Dim txtpamount As New TextBox
        Dim amount As Double = 0
        Dim pamount As Double = 0

        Select Case e.Item.ItemType
            Case ListItemType.Item, ListItemType.AlternatingItem
                txtAmount = CType(e.Item.FindControl("txtAllAmount1"), TextBox)
                txtAmount.Attributes.Add("onKeyPress", "checkValue();")
                txtpamount = CType(e.Item.FindControl("txtpamont"), TextBox)
                txtpamount.Attributes.Add("onKeyPress", "checkValue();")
                StudentMNo = e.Item.Cells(1).Text
                SemNo = e.Item.Cells(5).Text
                LoadInvoiceGrid(StudentMNo, SemNo)
                e.Item.Cells(6).Text = String.Format("{0:F}", totalStuamt)
                txtAmount.Text = String.Format("{0:F}", amount)
                amount = e.Item.Cells(6).Text - txtAmount.Text
                e.Item.Cells(8).Text = String.Format("{0:F}", amount)
                If txtpamount.Text = "" Then
                    txtpamount.Text = 0
                    pamount = txtpamount.Text
                    txtpamount.Text = String.Format("{0:F}", pamount)
                Else
                    pamount = txtpamount.Text
                    txtpamount.Text = String.Format("{0:F}", pamount)
                End If
        End Select
    End Sub
    Protected Sub btnAllocate_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAllocate.Click
        'If Trim(txtSpnName.Text).Length <> 0 Then
        Dim bamount As Double
        If txtAllocateAmount.Text = "" Then txtAllocateAmount.Text = 0
        bamount = txtAllocateAmount.Text
        txtAllocateAmount.Text = String.Format("{0:F}", bamount)
        LoadPaidInvoices()
        'End If
    End Sub
    Protected Sub txtAllAmount1_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim txtamt As TextBox
        Dim amount As Double = 0
        Dim dgitem As DataGridItem
        Dim i As Integer = 0
        For Each dgitem In dgView.Items
            txtamt = dgitem.FindControl("txtAllAmount1")

            amount = txtamt.Text

            txtamt.Text = String.Format("{0:F}", amount)
        Next
        LoadTotals()
    End Sub
    Protected Sub txtpamont_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim txtamt As TextBox
        Dim amount As Double = 0
        Dim dgitem As DataGridItem
        Dim i As Integer = 0
        For Each dgitem In dgView.Items
            txtamt = dgitem.FindControl("txtpamont")

            amount = txtamt.Text

            txtamt.Text = String.Format("{0:F}", amount)
        Next
        LoadTotals()
    End Sub
    Protected Sub ibtnDelete_Click1(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        ondelete()
    End Sub
    Protected Sub txtAllocateAmount_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim totalAmt As Double = 0
        Dim allamount As Double = 0
        If txtspnAmount.Text = "" Or txtAllAmount.Text = "" Then
        Else
            totalAmt = CDbl(txtspnAmount.Text) - CDbl(txtAllAmount.Text)
            If totalAmt < CDbl(txtAllocateAmount.Text) Then
                lblMsg.Visible = True
                lblMsg.Text = "Allocated Amount Exceeds the Amount Received"
                txtAllocateAmount.Text = ""
            End If
        End If
        If txtAllocateAmount.Text = "" Then
            txtAllocateAmount.Text = 0
        Else
            allamount = CDbl(txtAllocateAmount.Text)
            txtAllocateAmount.Text = String.Format("{0:F}", allamount)
        End If
    End Sub
    Protected Sub btnBatchInvoice_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        imgLeft1.ImageUrl = "images/b_white_left.gif"
        imgRight1.ImageUrl = "images/b_white_right.gif"
        btnBatchInvoice.CssClass = "TabButtonClick"
        imgLeft2.ImageUrl = "images/b_orange_left.gif"
        imgRight2.ImageUrl = "images/b_orange_right.gif"
        btnSelection.CssClass = "TabButton"

        MultiView1.SetActiveView(View1)

    End Sub
    Protected Sub btnSelection_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        imgLeft2.ImageUrl = "images/b_white_left.gif"
        imgRight2.ImageUrl = "images/b_white_right.gif"
        btnSelection.CssClass = "TabButtonClick"
        imgLeft1.ImageUrl = "images/b_orange_left.gif"
        imgRight1.ImageUrl = "images/b_orange_right.gif"
        btnBatchInvoice.CssClass = "TabButton"
        MultiView1.SetActiveView(View2)
    End Sub
    Protected Sub dgView_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs)

    End Sub
    Protected Sub ibtnOthers_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        LoadUserRights()
        OnSearchOthers()
    End Sub
    Protected Sub Btnselect_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        uploadData()
        imgLeft1.ImageUrl = "images/b_white_left.gif"
        imgRight1.ImageUrl = "images/b_white_right.gif"
        btnBatchInvoice.CssClass = "TabButtonClick"
        imgLeft2.ImageUrl = "images/b_orange_left.gif"
        imgRight2.ImageUrl = "images/b_orange_right.gif"
        btnSelection.CssClass = "TabButton"

        MultiView1.SetActiveView(View1)
    End Sub
    Protected Sub txtRecNo_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs)
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
    Protected Sub chkSelectAll_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkSelectAll.CheckedChanged
        Dim dgItem1 As DataGridItem
        Dim chkselect As CheckBox
        For Each dgItem1 In dgView.Items
            chkselect = dgItem1.Cells(0).Controls(1)
            If chkSelectAll.Checked = False Then
                chkselect.Checked = False
            Else
                chkselect.Checked = True
            End If
        Next
        LoadTotals()

    End Sub
    Protected Sub Chk_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)

        Dim chk As CheckBox
        Dim dgItem1 As DataGridItem
        For Each dgItem1 In dgView.Items
            chk = dgItem1.Cells(0).Controls(1)
            If chk.Checked = True Then
                LoadTotals()
            Else
                txtAllocateAmount.Text = ""
                LoadTotals()
            End If
        Next

    End Sub
    Protected Sub txtauto_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        tFlag = "Changed"
    End Sub
    Protected Sub btnHidden_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnHidden.Click

    End Sub
    Protected Sub btnGenerate_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnGenerate.Click
        
        If System.IO.File.Exists(SaveLocation & FILE_NAME) = True Then
            ibtnYesNo.Visible = True
            ibtnYesNo_Click(sender, e)
        Else
            generateFileToBank(SaveLocation & FILE_NAME, sender, e)
        End If
    End Sub
#Region "Methods"
    ''' <summary>
    ''' Method to read text file
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub generateFileToBank(ByVal FILEName As String, ByVal sender As Object, ByVal e As EventArgs)
        Dim obj As New List(Of AccountsDetailsEn)
        Dim fGenerate As New AccountsDetailsBAL
        Dim header As String = ""
        Dim footer As String = ""
        Dim i As Integer = 0
        Dim index As Integer = 0
        Dim aryText As String = ""
        Dim totalBatch, amt As Double
        Dim studentIC As String = ""
        Dim amtTrans As String = ""
        Dim noKelompokDet As String = ""
        Dim uniKod As String = ""
        Dim kumpulanPelajar As String = ""
        Dim kumpulanPelajarHeader As String = ""
        Dim namaPelajar As String = ""
        Dim noWarran As String = ""
        Dim noPelajar As String = ""
        Dim amaunPotongan As String = ""
        Dim nilaiBersih As String = ""
        Dim tarikhTrans As String = ""
        Dim tarikhlupusWarran As String = ""
        Dim noAkuanPeljr As String = ""
        Dim filler As String = ""
        Dim statusBayaran As String = ""
        Dim objWriter As New System.IO.StreamWriter(FILEName, True)
        Dim jumlahAmaun, jumlahRekod, kodBank As String
        Dim eobj As New AccountsEn
        Dim headerPTPTN As New AccountsBAL
        Dim fileGen As New AutoPayModule.PayFileGeneration
        Try
            If txtAutoNum.Text.Trim() <> "Auto Number" Then
                obj = fGenerate.GetListStudentSponsorAlloc(txtAutoNum.Text.Trim())
                eobj = headerPTPTN.GetHeaderPTPTN(txtAutoNum.Text.Trim())

                uniKod = eobj.KodUniversiti
                kumpulanPelajarHeader = "00"
                kodBank = eobj.KodBank
                'header format for text file
                header = fileGen.CreateHeader(uniKod, kumpulanPelajarHeader, "00000000", kodBank)
                objWriter.WriteLine(header)
                While index < obj.Count
                    studentIC = obj(i).Sudentacc.ICNo.Replace("-", "")
                    namaPelajar = obj(i).StudentName
                    noKelompokDet = obj(i).NoKelompok
                    amaunPotongan = fileGen.PrepareAmount(obj(i).TransactionAmount, "amaunPotongan")
                    aryText = fileGen.CreateDetails(noKelompokDet, uniKod, kumpulanPelajar, noWarran, noAkuanPeljr, studentIC, namaPelajar,
                                            amtTrans, amaunPotongan, nilaiBersih, tarikhTrans, tarikhlupusWarran, noAkuanPeljr, filler, statusBayaran)
                    amt = obj(i).TransactionAmount
                    totalBatch += amt
                    index += 1
                    i += 1
                    objWriter.WriteLine(aryText)
                End While
                jumlahAmaun = fileGen.PrepareAmount(totalBatch, "jumlahAmaun")
                jumlahRekod = fileGen.PrepareAmount(obj.Count, "jumlahRekod")
                footer = fileGen.CreateFooter(uniKod, kumpulanPelajarHeader, jumlahAmaun, jumlahRekod)
                objWriter.WriteLine(footer)
                objWriter.Close()
                'fileGen(FILE_NAME)
                Response.ContentType = "text/plain"
                Response.AddHeader("content-disposition", "attachment; filename=" & FILE_NAME & "")
                Response.TransmitFile(SaveLocation & FILE_NAME)
                Response.End()
            Else
                'MsgBox("Please select a record", MsgBoxStyle.Critical, "SAS Warning")
                Response.Write("<script>javascript:alert('Please select a record')</script>")
            End If
        Catch ex As Exception
            Throw ex
            objWriter.Close()

        End Try
    End Sub
    ''' <summary>
    ''' Method to read text file
    ''' </summary>
    ''' <remarks></remarks>
    Private Function readTextFile(ByVal filepath As String) As List(Of StudentEn)
        Dim lstStudents As New List(Of StudentEn)
        Dim fileEntries As New List(Of String)

        Try
            ' Read the file into a list...
            Dim reader As StreamReader = New StreamReader(filepath)
            fileEntries.Clear()

            Do Until reader.Peek = -1 'Until eof
                fileEntries.Add(reader.ReadLine)
            Loop
            reader.Close()
        Catch ex As Exception
            ' The file's empty.
            lblMsg.Visible = True
            lblMsg.Text = "The File`s is empty. Error message: " & ex.Message
        End Try
        Try
            For Each line As String In fileEntries
                Dim checkCol As String = line.Substring(0, 10)
                Dim _studentEN As New StudentEn
                Dim _studEnFromDB As New StudentEn
                Dim stud As New StudentBAL
                Dim _studAccFromDB As New AccountsEn
                Dim studAcc As New AccountsBAL
                Dim tempAmount As Double
                If checkCol = "0000000000" Then
                    'Check Line for header
                    Session("KodUniversiti") = line.Substring(10, 2)
                    Session("KumpulanPelajar") = line.Substring(13, 2)
                    Session("TarikhProses") = line.Substring(15, 8)
                    Session("KodBank") = line.Substring(23, 2)
                ElseIf checkCol = "9999999999" Then
                    'Check Line for footer
                Else
                    _studentEN.ICNo = line.Substring(43, 12)
                    _studEnFromDB = stud.GetStudInfo(_studentEN.ICNo)
                    If _studEnFromDB.MatricNo <> "null" Then
                        _studentEN.ProgramID = _studEnFromDB.ProgramID
                        _studentEN.StudentName = line.Substring(55, 80)
                        _studentEN.CurrentSemester = _studEnFromDB.CurrentSemester
                        _studentEN.MatricNo = _studEnFromDB.MatricNo
                        _studAccFromDB = studAcc.GetItemTrans(_studentEN)
                        If _studAccFromDB.AllocatedAmount > 0 Then
                            _studentEN.TransactionAmount = _studAccFromDB.AllocatedAmount
                        Else
                            _studentEN.TransactionAmount = 0.0
                        End If
                        tempAmount = 0.0
                        tempAmount = String.Format("{0:000000.00}", line.Substring(135, 8))
                        tempAmount = (tempAmount * 0.01).ToString("N2")
                        tempAmount = tempAmount - _studAccFromDB.AllocatedAmount
                        If tempAmount <= _studAccFromDB.AllocatedAmount Then
                            _studentEN.TempAmount = 100.0
                        Else
                            _studentEN.TempAmount = tempAmount
                        End If
                        _studentEN.NoKelompok = line.Substring(0, 10)
                        _studentEN.NoWarran = line.Substring(15, 14)
                        _studentEN.AmaunWarran = line.Substring(135, 8)
                        _studentEN.noAkaun = line.Substring(175, 14)
                        _studentEN.StatusBayaran = ""
                        lstStudents.Add(_studentEN)
                        _studentEN = Nothing
                    Else
                        If Session("errStulist") = Nothing Then
                            Session("errStulist") = _studentEN.ICNo
                        Else
                            Session("errStulist") = _studentEN.ICNo & "," & Session("errStulist")
                        End If

                        _studentEN = Nothing
                    End If
                End If
            Next
            readTextFile = lstStudents
        Catch ex As Exception
            lblMsg.Visible = True
            lblMsg.Text = "Error message: " & ex.Message
            'MsgBox("Error message: " & ex.Message & "  *Hint: Make Sure Fee Structure Exist For Students and valid for current Semester.", MsgBoxStyle.Critical, "Error SAS")
            Session("Err") = "Error"
        End Try
        Return readTextFile
    End Function
    'Public Sub Filegen(ByVal FilePath As String)

    '    '======================================================================
    '    'Purpose       :- To show the generated file in notepad.exe after file is created
    '    'Inputs        :- No arguments
    '    'Return Values :- Nothing
    '    'Modified      :-
    '    '======================================================================

    '    Try
    '        Dim pr As Process
    '        pr = New Process
    '        pr.StartInfo.FileName = "NOTEPAD.EXE"
    '        pr.StartInfo.Arguments = FilePath
    '        pr.Start()
    '        pr = Nothing

    '    Catch ex As Exception
    '        Throw ex
    '    End Try
    'End Sub
    ''' <summary>
    ''' Method to get the List Of Sponsor Allocations
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub LoadListObjects()
        Dim obj As New AccountsBAL
        Dim eob As New AccountsEn
        If txtAutoNum.Text <> "Auto Number" Then
            eob.BatchCode = txtAutoNum.Text
        ElseIf Request.QueryString("Batchcode") <> Nothing Then
            eob.BatchCode = Request.QueryString("Batchcode")
        Else
            eob.BatchCode = ""
        End If
        If Session("loaddata") = "View" Then

            eob.Category = "Pre-Allocation"
            eob.SubType = "Sponsor"
            eob.PostStatus = "Ready"
            Try
                ListObjects = obj.GetTransactions(eob)
            Catch ex As Exception
                LogError.Log("SponsorAllocation", "LoadListObjects", ex.Message)
            End Try

        ElseIf Session("loaddata") = "others" Then

            eob.Category = "Pre-Allocation"
            eob.SubType = "Sponsor"
            eob.PostStatus = "Posted"
            eob.TransStatus = ""

            Try
                ListObjects = obj.GetSPAllocationTransactions(eob)
            Catch ex As Exception
                LogError.Log("SponsorAllocation", "LoadListObjects", ex.Message)
            End Try
        End If

        Session("ListObj") = ListObjects
        lblCount.Text = ListObjects.Count.ToString()
        If ListObjects.Count <> 0 Then
            DisableRecordNavigator()
            txtRecNo.Text = "1"
            OnMoveFirst()
            If Session("EditFlag") = True Then
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

            If DFlag = "Delete" Then
            Else
                lblMsg.Visible = True
                ErrorDescription = "Record did not Exist"
                lblMsg.Text = ErrorDescription
                DFlag = ""
                Session("PageMode") = "Add"
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
    ''' Method to Fill the Field Values
    ''' </summary>
    ''' <param name="RecNo"></param>
    ''' <remarks></remarks>
    Private Sub FillData(ByVal RecNo As Integer)
        Dim amount As Double
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
                Dim obj As AccountsEn
                ListObjects = Session("ListObj")
                obj = ListObjects(RecNo)
                txtAutoNum.Text = obj.BatchCode
                ibtnStatus.ImageUrl = "images/ready.gif"
                lblStatus.Value = "Ready"
                amount = obj.TransactionAmount
                Session("PAidAmount") = amount

                txtPaymentDate.Text = obj.TransDate
                txtDesc.Text = obj.Description
                txtBDate.Text = obj.BatchDate
                txtCheque.Text = obj.ChequeNo
                'txtAllocationCode.Text = obj.CreditRefOne
                txtchequeDate.Text = obj.ChequeDate
                txtAllocateAmount.Text = String.Format("{0:F}", obj.TransactionAmount)


                If obj.PostStatus = "Ready" Then
                    lblStatus.Value = "Ready"
                    ibtnStatus.ImageUrl = "images/Ready.gif"
                End If
                If obj.PostStatus = "Posted" Then
                    lblStatus.Value = "Posted"
                    ibtnStatus.ImageUrl = "images/Posted.gif"
                    trFileGen.Visible = True
                End If
                Dim espn As New AccountsEn
                Dim bospn As New AccountsBAL
                Dim listsp As New List(Of SponsorEn)
                espn.TransactionCode = obj.CreditRefOne
                Try
                    espn = bospn.GetItemReceipt(espn)
                Catch ex As Exception
                    LogError.Log("SponsorAllocation", "FillData", ex.Message)
                End Try


                txtspnAmount.Text = obj.SubReferenceOne
                txtAllAmount.Text = obj.SubReferenceTwo
                txtspcode.Text = obj.CreditRef

                Session("KodUniversiti") = obj.KodUniversiti
                Session("KumpulanPelajar") = obj.KumpulanPelajar
                Session("KodBank") = obj.KodBank
                Session("loaddata") = Nothing
                Dim liststuAll As New List(Of AccountsDetailsEn)
                Dim objstu As New AccountsDetailsBAL
                Dim eobstu As New AccountsDetailsEn
                Dim stlist As New List(Of StudentEn)
                Dim stuen As New StudentEn
                Dim bsstu As New AccountsBAL
                eobstu.TransactionID = obj.TranssactionID

                Try
                    liststuAll = objstu.GetStuDentAllocation(eobstu)
                Catch ex As Exception
                    LogError.Log("SponsorAllocation", "FillData", ex.Message)
                End Try

                Session("spt") = obj.CreditRef
                Session("AddFee") = liststuAll
                dgView.DataSource = liststuAll
                dgView.DataBind()
                MultiView1.SetActiveView(View1)

                Dim chk As CheckBox
                Dim txtAmount As TextBox
                Dim txtpamont As TextBox
                Dim dgItem1 As DataGridItem
                Dim j As Integer = 0
                Dim amt As Double = 0
                Dim outamt As Double = 0
                Dim tamt As Double = 0

                While j < liststuAll.Count

                    For Each dgItem1 In dgView.Items
                        If dgItem1.Cells(1).Text = liststuAll(j).Sudentacc.MatricNo Then
                            chk = dgItem1.Cells(0).Controls(1)
                            chk.Checked = True
                            txtAmount = dgItem1.Cells(7).Controls(1)
                            amt = liststuAll(j).TransactionAmount
                            txtpamont = dgItem1.Cells(9).Controls(1)
                            tamt = liststuAll(j).TempAmount
                            txtpamont.Text = String.Format("{0:F}", tamt)
                            txtAmount.Text = String.Format("{0:F}", amt)
                            stuen.MatricNo = dgItem1.Cells(1).Text
                            outamt = bsstu.GetStudentOutstandingAmt(stuen)
                            ' Added by JK
                            dgItem1.Cells(6).Text = String.Format("{0:F}", outamt)
                            amt = (CDbl(dgItem1.Cells(6).Text) - amt)
                            dgItem1.Cells(8).Text = String.Format("{0:F}", amt)
                            dgItem1.Cells(13).Text = liststuAll(j).NoKelompok
                            dgItem1.Cells(14).Text = liststuAll(j).NoWarran
                            dgItem1.Cells(15).Text = liststuAll(j).AmaunWarran
                            dgItem1.Cells(16).Text = liststuAll(j).noAkaun
                            'statusBayaran.Value = liststuAll(j).StatusBayaran
                            Exit For
                        End If
                    Next
                    j = j + 1
                End While
            End If
        End If
        setDateFormat()
    End Sub

    ''' <summary>
    ''' Method to Load Totals in the Grid
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub LoadTotals()
        Dim chk As CheckBox
        Dim txtAmount As TextBox
        Dim txtPocket As TextBox
        Dim dgItem1 As DataGridItem
        Dim totalAmt1 As Double = 0
        Dim BalAmt As Double = 0
        For Each dgItem1 In dgView.Items
            Dim totalAmt As Double = 0
            chk = dgItem1.Cells(0).Controls(1)
            If chk.Checked = True Then
                Dim AllAmt As Double = 0
                Dim Allpck As Double = 0

                txtAmount = dgItem1.Cells(7).Controls(1)
                If txtAmount.Text <> "" Then
                    AllAmt = CDbl(txtAmount.Text)
                End If
                txtPocket = dgItem1.Cells(9).Controls(1)
                If txtPocket.Text <> "" Then
                    Allpck = CDbl(txtPocket.Text)
                End If
                totalAmt = AllAmt + Allpck
                totalAmt1 += totalAmt
            End If
        Next

        If txtspnAmount.Text = "" Then
            Exit Sub
        End If
        BalAmt = CDbl(txtspnAmount.Text) - CDbl(txtAllAmount.Text)


        If totalAmt1 > BalAmt Then
            lblMsg.Visible = True
            lblMsg.Text = "Allocated Amount Exceeds the Amount Received"
            txtAllocateAmount.Text = String.Format("{0:F}", "0.0")
            Aflag = "Exit"
        Else
            txtAllocateAmount.Text = String.Format("{0:F}", totalAmt1)
            txtAllocateAmount.ReadOnly = True
        End If


    End Sub

    ''' <summary>
    ''' Method to Load Fields in New Mode
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub onAdd()
        today.Value = Now.Date
        today.Value = Format(CDate(today.Value), "dd/MM/yyyy")
        If ibtnNew.Enabled = False Then
            ibtnSave.Enabled = False
            ibtnSave.ImageUrl = "images/gsave.png"
            ibtnSave.ToolTip = "Access Denied"
        End If
        Session("ListObj") = Nothing
        OnClearData()
        If ibtnNew.Enabled = False Then
            ibtnSave.Enabled = False
            ibtnSave.ImageUrl = "images/gsave.png"
            ibtnSave.ToolTip = "Access Denied"
        End If
        Session("PageMode") = "Add"
        OnLoadItem()
        lblStatus.Value = "New"
        ibtnStatus.ImageUrl = "images/notready.gif"
    End Sub

    ''' <summary>
    ''' Method to Load DateFields
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub OnLoadItem()
        If Session("PageMode") = "Add" Then
            txtAutoNum.Text = "Auto Number"
            txtAutoNum.ReadOnly = True
            today.Value = Now.Date
            today.Value = Format(CDate(today.Value), "dd/MM/yyyy")
            txtPaymentDate.Text = Format(Date.Now, "dd/MM/yyyy")
            txtchequeDate.Text = Format(Date.Now, "dd/MM/yyyy")
            txtBDate.Text = Format(Date.Now, "dd/MM/yyyy")
        End If
    End Sub

    ''' <summary>
    ''' Method to Save and Update Sponsor Allocations 
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub onSave()
        Dim eobj As New AccountsEn
        Dim eobjDetails As New AccountsDetailsEn
        Dim list As New List(Of AccountsDetailsEn)
        Dim splist As New List(Of SponsorEn)
        Dim eospn As New SponsorEn
        Dim bsobj As New AccountsBAL
        GBFormat = New System.Globalization.CultureInfo("en-GB")
        eobj.BatchCode = Trim(txtAutoNum.Text)
        eobj.CreditRef = Session("spt")
        eobj.TempAmount = Trim(txtAllocateAmount.Text)
        eobj.TempPaidAmount = Trim(txtAllocateAmount.Text)
        eobj.PaidAmount = Trim(txtAllocateAmount.Text)
        eobj.TransactionAmount = Trim(txtAllocateAmount.Text)
        eobj.TransType = "Credit"
        'eobj.PaymentMode = ddlPaymentMode.SelectedValue
        eobj.SubType = "Sponsor"
        eobj.TransDate = Convert.ToDateTime(txtPaymentDate.Text)
        eobj.Description = Trim(txtDesc.Text)
        eobj.BatchDate = Trim(txtBDate.Text)
        eobj.ChequeDate = Trim(txtchequeDate.Text)
        eobj.SubReferenceOne = Trim(txtspnAmount.Text)
        eobj.SubReferenceTwo = Trim(txtAllAmount.Text)
        eobj.Category = "Pre-Allocation"
        eobj.TransStatus = "Open"
        eobj.PostStatus = "Ready"
        eobj.PostedDateTime = DateTime.Now
        eobj.UpdatedTime = DateTime.Now
        eobj.UpdatedBy = Session("User")
        eobj.DueDate = DateTime.Now
        eobj.CreatedDateTime = DateTime.Now

        eobj.KodUniversiti = Session("KodUniversiti")
        eobj.KumpulanPelajar = Session("KumpulanPelajar")
        eobj.TarikhProses = Session("TarikhProses")
        eobj.KodBank = Session("txtKodBank")
        Dim dgItem1 As DataGridItem
        Dim amount As TextBox
        Dim tempAmount As TextBox
        Dim chkselect As CheckBox
        'Dim NoKelompok As HiddenField = Nothing
        'Dim NoWarran As HiddenField = Nothing
        'Dim amaunWarran As HiddenField = Nothing
        'Dim noAkaunPelajar As HiddenField = Nothing
        'Dim statusBayaran As HiddenField = Nothing

        For Each dgItem1 In dgView.Items
            chkselect = dgItem1.Cells(0).Controls(1)
            If chkselect.Checked = True Then
                Dim NoKelompok As String = ""
                Dim NoWarran As String = ""
                Dim AmaunWarran As Double = 0.0
                Dim noAkaun As String = ""
                amount = dgItem1.Cells(7).Controls(1)
                'NoKelompok = dgItem1.Cells(13).Controls(1)
                'NoWarran = dgItem1.Cells(14).Controls(1)
                'amaunWarran = dgItem1.Cells(15).Controls(1)
                'noAkaunPelajar = dgItem1.Cells(16).Controls(1)
                tempAmount = dgItem1.Cells(9).Controls(1)
                eobjDetails = New AccountsDetailsEn
                eobjDetails.ReferenceCode = dgItem1.Cells(1).Text.Trim
                eobjDetails.PaidAmount = dgItem1.Cells(6).Text.Trim
                eobjDetails.TransactionAmount = CDbl(amount.Text.Trim)
                eobjDetails.TempAmount = CDbl(tempAmount.Text.Trim)
                eobjDetails.TransStatus = "Open"
                If dgItem1.Cells(13).Text.Trim = "&nbsp;" Then
                    NoKelompok = ""
                End If
                If dgItem1.Cells(14).Text.Trim = "&nbsp;" Then
                    NoWarran = ""
                End If
                If dgItem1.Cells(15).Text.Trim = "&nbsp;" Then
                    AmaunWarran = ""
                End If
                If dgItem1.Cells(16).Text.Trim = "&nbsp;" Then
                    noAkaun = ""
                End If
                eobjDetails.NoKelompok = NoKelompok
                eobjDetails.NoWarran = NoWarran
                eobjDetails.AmaunWarran = AmaunWarran
                eobjDetails.noAkaun = noAkaun
                'eobjDetails.StatusBayaran = statusBayaran.Value
                list.Add(eobjDetails)
                eobjDetails = Nothing
            End If

        Next
        eobj.AccountDetailsList = list
        If list.Count = 0 Then
            ErrorDescription = "Select At least One Student"
            lblMsg.Text = ErrorDescription
            Exit Sub

        End If
        If txtAllocateAmount.Text = 0 Then
            ErrorDescription = "Enter Valid Amount"
            lblMsg.Text = ErrorDescription
            Exit Sub
        End If
        If Not Session("spt") Is Nothing Then
            eospn.SponserCode = Session("spt")
        Else
            eospn.SponserCode = txtspcode.Text

        End If
        LoadTotals()
        If Aflag = "Exit" Then
            Exit Sub
        End If

        splist.Add(eospn)
        lblMsg.Visible = True
        If Session("PageMode") = "Add" Then
            Try
                txtAutoNum.Text = bsobj.SponsorBatchInsert(eobj, splist)
                ErrorDescription = "Record Saved Successfully "
                lblMsg.Text = ErrorDescription
                ibtnStatus.ImageUrl = "images/ready.gif"
                lblStatus.Value = "Ready"
                txtAutoNum.ReadOnly = False
                txtAutoNum.Text = eobj.BatchCode
                txtAutoNum.ReadOnly = True
                'Display error message saying that Duplicate Record
            Catch ex As Exception
                lblMsg.Text = ex.Message.ToString()
                LogError.Log("SponsorAllocation", "Onsave", ex.Message)
            End Try
        ElseIf Session("PageMode") = "Edit" Then
            Try

                txtAutoNum.Text = bsobj.SponsorBatchUpdate(eobj, splist)
                ListObjects = Session("ListObj")
                ListObjects(CInt(txtRecNo.Text) - 1) = eobj
                Session("ListObj") = ListObjects
                ErrorDescription = "Record Updated Successfully "
                lblMsg.Text = ErrorDescription
            Catch ex As Exception
                lblMsg.Text = ex.Message.ToString()
                LogError.Log("SponsorAllocation", "Onsave", ex.Message)
            End Try
        End If
        'setDateFormat()
    End Sub

    ''' <summary>
    ''' Method to Clear the Field Values
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub OnClearData()
        txtspnAmount.ReadOnly = True
        txtAllAmount.ReadOnly = True
        Session("ListObj") = Nothing
        Session("stualloc") = Nothing
        Session("stuupload") = Nothing
        Session("fileSponsor") = Nothing
        Session("fileType") = Nothing
        Session("Err") = Nothing
        DisableRecordNavigator()
        txtAutoNum.Text = ""
        txtAllocateAmount.Text = ""
        lblMsg.Text = ""
        'ddlPaymentMode.SelectedValue = "-1"
        txtPaymentDate.Text = ""
        txtDesc.Text = ""
        txtBDate.Text = ""
        txtCheque.Text = ""
        txtchequeDate.Text = ""
        txtspnAmount.Text = ""
        txtAllAmount.Text = ""
        trFileGen.Visible = False
        chkSelectAll.Visible = False
        dgView.DataSource = Nothing
        dgView.DataBind()
        dgUnView.DataSource = Nothing
        dgUnView.DataBind()
        dgInvoices.DataSource = Nothing
        dgInvoices.DataBind()

        Session("PageMode") = "Add"
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
            LogError.Log("SponsorAllocation", "Menuname", ex.Message)
        End Try
        lblMenuName.Text = eobj.MenuName
    End Sub

    ''' <summary>
    ''' Method to Change the Date Format
    ''' </summary>
    ''' <remarks>Date in ddd/mm/yyyy Format</remarks>
    Private Sub dates()
        Dim GBFormat As System.Globalization.CultureInfo
        GBFormat = New System.Globalization.CultureInfo("en-GB")

        txtPaymentDate.Text = Format(Date.Now, "dd/MM/yyyy")
        txtchequeDate.Text = Format(Date.Now, "dd/MM/yyyy")
        txtBDate.Text = Format(Date.Now, "dd/MM/yyyy")
    End Sub

    ''' <summary>
    ''' Method To Change the Date Format(dd/MM/yyyy)
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub setDateFormat()
        'Dim GBFormat As System.Globalization.CultureInfo
        'GBFormat = New System.Globalization.CultureInfo("en-GB")
        'txtPaymentDate.Text = Format(DateTime.Parse(txtPaymentDate.Text.Trim(), GBFormat), "dd/MM/yyyy")
        'txtchequeDate.Text = Format(DateTime.Parse(txtchequeDate.Text, GBFormat), "dd/MM/yyyy")
        'txtBDate.Text = Format(DateTime.Parse(txtBDate.Text, GBFormat), "dd/MM/yyyy")

        Dim myPaymentDate As Date = CDate(CStr(txtPaymentDate.Text))
        Dim myFormat As String = "dd/MM/yyyy"
        Dim myFormattedDate As String = Format(myPaymentDate, myFormat)
        txtPaymentDate.Text = myFormattedDate
        Dim mychequeDate As Date = CDate(CStr(txtchequeDate.Text))
        Dim myFormattedDate1 As String = Format(mychequeDate, myFormat)
        txtchequeDate.Text = myFormattedDate1
        Dim myBatchDate As Date = CDate(CStr(txtBDate.Text))
        Dim myFormattedDate2 As String = Format(myBatchDate, myFormat)
        txtBDate.Text = myFormattedDate2
    End Sub
    
    ''' <summary>
    ''' Method to Validate
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub SpaceValidation()
        Dim GBFormat As System.Globalization.CultureInfo
        GBFormat = New System.Globalization.CultureInfo("en-GB")

        'Description
        If Trim(txtDesc.Text).Length = 0 Then
            txtDesc.Text = Trim(txtDesc.Text)
            lblMsg.Text = "Enter Valid Description "
            lblMsg.Visible = True
            txtDesc.Focus()
            Exit Sub
        End If

        'Batch date
        If Trim(txtBDate.Text).Length < 10 Then
            lblMsg.Text = "Enter Valid Batch Date"
            lblMsg.Visible = True
            txtBDate.Focus()
            Exit Sub
        Else
            Try
                txtBDate.Text = DateTime.Parse(txtBDate.Text.Trim(), GBFormat)
            Catch ex As Exception
                lblMsg.Text = "Enter Valid Batch Date"
                lblMsg.Visible = True
                txtBDate.Focus()
                Exit Sub
            End Try
        End If
        'Invoice date
        If Trim(txtPaymentDate.Text).Length < 10 Then
            lblMsg.Text = "Enter Valid Invoice Date"
            lblMsg.Visible = True
            txtPaymentDate.Focus()
            Exit Sub
        Else
            Try
                txtPaymentDate.Text = DateTime.Parse(txtPaymentDate.Text.Trim(), GBFormat)

            Catch ex As Exception
                lblMsg.Text = "Enter Valid Invoice Date"
                lblMsg.Visible = True
                txtPaymentDate.Focus()
                Exit Sub
            End Try
        End If

        'Due date
        If Trim(txtchequeDate.Text).Length < 10 Then
            lblMsg.Text = "Enter Valid Due Date"
            lblMsg.Visible = True
            txtchequeDate.Focus()
            Exit Sub
        Else
            Try

                txtchequeDate.Text = DateTime.Parse(txtchequeDate.Text.Trim(), GBFormat)
            Catch ex As Exception
                lblMsg.Text = "Enter Valid Due Date"
                lblMsg.Visible = True
                txtchequeDate.Focus()
                Exit Sub
            End Try
        End If

    End Sub

    ''' <summary>
    ''' Method to LoadSponsor Receipts
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub addSpnCode()
        Dim obj As New AccountsBAL
        Session("SPncode") = Nothing
        txtspnAmount.ReadOnly = True
        txtAllAmount.ReadOnly = True
        Dim eobj As New SponsorEn
        eobj = Session("spnObj")
        Dim amount As Double
        txtspcode.Text = eobj.CreditRef
        amount = eobj.TransactionAmount - eobj.PaidAmount
        txtspnAmount.Text = String.Format("{0:F}", eobj.TransactionAmount)
        txtAllAmount.Text = String.Format("{0:F}", eobj.PaidAmount)
        Session("SPncode") = eobj.CreditRef
        Session("PAidAmount") = amount
        Session("Scode") = eobj.CreditRef
        Session("spt") = eobj.CreditRef
        Session("spnObj") = Nothing

    End Sub

    ''' <summary>
    ''' Method to Load Students to Grid
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub addSelectStudent()
        dgView.DataSource = Nothing
        dgView.DataBind()
        Dim mylst As List(Of StudentEn)
        Dim all As List(Of StudentEn)
        Dim eobj As New StudentEn
        Dim i As Integer = 0
        mylst = Session("liststu")
        If Not Session("stuupload") Is Nothing Then
            all = Session("stuupload")
        Else
            all = New List(Of StudentEn)
        End If

        If mylst.Count <> 0 Then
            While i < mylst.Count
                eobj = mylst(i)
                Dim k As Integer = 0
                Dim Flag As Boolean = False
                While k < all.Count
                    If all(k).MatricNo = eobj.MatricNo Then
                        Flag = True
                        Exit While
                    End If
                    k = k + 1
                End While
                If Flag = False Then
                    all.Add(eobj)
                End If
                i = i + 1
            End While
        End If

        If all Is Nothing Then
            dgView.DataSource = Nothing
            dgView.DataBind()
        Else
            Dim dgItem1 As DataGridItem
            Dim txtAmount As TextBox
            Dim amt As Double = 0.0
            Dim j As Integer = 0
            Dim stuen As New StudentEn
            Dim bsstu As New AccountsBAL
            Dim outamt As Double = 0.0
            'dgView.PageSize = mylst.Count
            Session("stualloc") = all
            dgView.DataSource = all
            dgView.DataBind()
            While j < all.Count
                For Each dgItem1 In dgView.Items
                    If dgItem1.Cells(1).Text = all(j).MatricNo Then
                        txtAmount = dgItem1.Cells(7).Controls(1)
                        amt = CDbl(dgItem1.Cells(10).Text)
                        txtAmount.Text = String.Format("{0:F}", amt)
                        stuen.MatricNo = dgItem1.Cells(1).Text
                        outamt = bsstu.GetStudentOutstandingAmt(stuen)
                        dgItem1.Cells(6).Text = String.Format("{0:F}", outamt)
                        Exit For
                    End If
                Next
                j = j + 1
            End While

        End If


        Session("spt") = Session("SPncode")
        Session("spnObj") = Nothing
        Session("liststu") = Nothing
        Session("SPncode") = Nothing
        Session("paidInvoices") = Nothing
        imgLeft1.ImageUrl = "images/b_white_left.gif"
        imgRight1.ImageUrl = "images/b_white_right.gif"
        btnBatchInvoice.CssClass = "TabButtonClick"
        imgLeft2.ImageUrl = "images/b_orange_left.gif"
        imgRight2.ImageUrl = "images/b_orange_right.gif"
        btnSelection.CssClass = "TabButton"

        MultiView1.SetActiveView(View1)
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
            LogError.Log("SponsorAllocation", "LoadUserRights", ex.Message)
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
            ibtnView.ImageUrl = "images/ready.png"
            ibtnView.Enabled = True
        Else
            ibtnView.ImageUrl = "images/ready.png"
            ibtnView.Enabled = True
            'ibtnView.ToolTip = "Access Denied"
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
            ibtnOthers.ImageUrl = "images/post.png"
            ibtnOthers.ToolTip = "Others"
        Else
            ibtnOthers.Enabled = False
            ibtnOthers.ImageUrl = "images/post.png"
            'ibtnOthers.ToolTip = "Access Denied"
            ibtnOthers.ToolTip = "Others"
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
    ''' Method to Load Student Invoices in Grid
    ''' </summary>
    ''' <param name="StuMNo"></param>
    ''' <param name="semNo"></param>
    ''' <remarks></remarks>
    Private Sub LoadInvoiceGrid(ByVal StuMNo As String, ByVal semNo As String)
        Dim ListInvObjects As New List(Of AccountsEn)
        Dim eob As New AccountsEn
        Dim obj As New AccountsBAL
        Dim TotalAmount As Double
        Dim amount As Double
        Dim CreditAmt As Double
        Dim DreditAmt As Double
        Dim OutSAmt As Double
        Dim dr As Double = 0
        Dim cr As Double = 0
        eob.CreditRef = StuMNo
        eob.PostStatus = "Posted"
        eob.SubType = "Student"
        eob.TransType = ""
        eob.TransStatus = ""
        Try
            ListInvObjects = obj.GetStudentLedgerList(eob)

        Catch ex As Exception
            LogError.Log("SponsorAllocation", "LoadInvoiceGrid", ex.Message)
        End Try

        If ListInvObjects.Count = 0 Then
        Else
            dgInvoices.DataSource = ListInvObjects
            dgInvoices.DataBind()

            Dim dgItem1 As DataGridItem
            CreditAmt = String.Format("{0:F}", 0)
            DreditAmt = String.Format("{0:F}", 0)
            OutSAmt = String.Format("{0:F}", 0)
            For Each dgItem1 In dgInvoices.Items
                If dgItem1.Cells(5).Text = "Cr" Then
                    TotalAmount = TotalAmount - CDbl(dgItem1.Cells(3).Text)
                    dgItem1.Cells(4).Text = String.Format("{0:F}", TotalAmount)
                    amount = dgItem1.Cells(3).Text
                    dgItem1.Cells(3).Text = String.Format("{0:F}", amount) & "-"
                    cr = cr + amount
                    CreditAmt = String.Format("{0:F}", cr)
                Else
                    TotalAmount = TotalAmount + CDbl(dgItem1.Cells(3).Text)
                    dgItem1.Cells(4).Text = String.Format("{0:F}", TotalAmount)
                    amount = dgItem1.Cells(3).Text
                    dgItem1.Cells(3).Text = String.Format("{0:F}", amount) & "+"
                    dr = dr + amount
                    DreditAmt = String.Format("{0:F}", dr)
                End If
            Next
            totalStuamt = String.Format("{0:F}", DreditAmt - CreditAmt)
        End If
    End Sub

    ''' <summary>
    ''' Method to Load Paid Invoices
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub LoadPaidInvoices()
        Dim dgItem1 As DataGridItem
        Dim listInvoices As New List(Of StudentEn)
        Dim eTTRDetails As New StudentEn
        Dim tempamount As Double = 0.0
        Dim InvoiceAmount As Double = 0.0
        Dim txtAmount As TextBox
        Dim txtpdAmount As TextBox
        Dim chk As CheckBox
        Dim alamount As Double
        Dim pamount As Double
        Dim Tamount As Double
        'txtAddedAmount.Text = 0
        If Session("paidInvoices") Is Nothing Then
            listInvoices = Session("stualloc")
            dgView.DataSource = listInvoices
            dgView.DataBind()

            Dim tamt As Double = 0.0
            Dim tpamt As Double = 0.0
            Dim j As Integer = 0
            Dim stuen As New StudentEn
            While j < listInvoices.Count

                For Each dgItem1 In dgView.Items
                    If dgItem1.Cells(1).Text = listInvoices(j).MatricNo Then
                        chk = dgItem1.Cells(0).Controls(1)
                        txtAmount = dgItem1.Cells(7).Controls(1)
                        'txtAmount.Text = "0.00"

                        If chk.Checked = True Then
                            chk.Checked = False
                        End If

                        InvoiceAmount = InvoiceAmount + dgItem1.Cells(6).Text
                        If CDbl(txtauto.Text) >= InvoiceAmount Then
                            eTTRDetails = New StudentEn
                            eTTRDetails.PaidAmount = dgItem1.Cells(6).Text
                            Tamount = eTTRDetails.PaidAmount
                            txtAmount.Text = String.Format("{0:F}", Tamount)
                            eTTRDetails.TempAmount = txtAmount.Text

                            txtpdAmount = dgItem1.Cells(9).Controls(1)
                            pamount = dgItem1.Cells(6).Text - txtAmount.Text
                            txtpdAmount.Text = String.Format("{0:F}", pamount)
                            eTTRDetails.TempPaidAmount = txtpdAmount.Text

                            eTTRDetails.TransactionAmount = dgItem1.Cells(6).Text
                            If chk.Checked = False Then
                                chk.Checked = True
                            End If

                        Else
                            If txtAllAmount.Text = InvoiceAmount Then
                            Else


                                tempamount = InvoiceAmount - txtauto.Text
                                alamount = dgItem1.Cells(6).Text - tempamount

                                If alamount > 0 Then
                                    If chk.Checked = False Then
                                        chk.Checked = True
                                    End If
                                    eTTRDetails = New StudentEn
                                    eTTRDetails.PaidAmount = alamount
                                    Tamount = eTTRDetails.PaidAmount
                                    txtpdAmount = dgItem1.Cells(9).Controls(1)
                                    txtAmount.Text = String.Format("{0:F}", alamount)
                                    eTTRDetails.TempAmount = txtAmount.Text
                                    pamount = dgItem1.Cells(6).Text - txtAmount.Text
                                    txtpdAmount.Text = String.Format("{0:F}", pamount)
                                    eTTRDetails.TempPaidAmount = txtpdAmount.Text
                                    eTTRDetails.TransactionAmount = dgItem1.Cells(6).Text
                                End If

                            End If
                        End If
                    End If
                Next

                j = j + 1
            End While
        Else
            listInvoices = Session("paidInvoices")
            dgView.DataSource = Nothing
            dgView.DataSource = listInvoices
            dgView.DataBind()
        End If
        Session("stualloc") = Nothing
        Session("paidInvoices") = listInvoices
        MultiView1.SetActiveView(View1)
    End Sub

    ''' <summary>
    ''' Method to Post Sponsor Allocations
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub onPost()
        Dim eobj As New AccountsEn
        Dim eobjDetails As New AccountsDetailsEn
        Dim list As New List(Of AccountsDetailsEn)
        Dim splist As New List(Of SponsorEn)
        Dim eospn As New SponsorEn
        Dim bsobj As New AccountsBAL
        eobj.BatchCode = Trim(txtAutoNum.Text)
        eobj.CreditRef = Session("spt")
        eobj.TempAmount = Trim(txtAllocateAmount.Text)
        eobj.TempPaidAmount = Trim(txtAllocateAmount.Text)
        eobj.PaidAmount = Trim(txtAllocateAmount.Text)
        eobj.TransactionAmount = Trim(txtAllocateAmount.Text)
        eobj.TransType = "Credit"
        eobj.BankCode = ""
        'eobj.PaymentMode = ddlPaymentMode.SelectedValue
        eobj.SubType = "Sponsor"
        eobj.TransDate = Trim(txtPaymentDate.Text)
        eobj.Description = Trim(txtDesc.Text)
        eobj.BatchDate = Trim(txtBDate.Text)
        eobj.CreditRefOne = ""
        eobj.ChequeDate = Trim(txtchequeDate.Text)
        eobj.SubReferenceOne = Trim(txtspnAmount.Text)
        eobj.SubReferenceTwo = Trim(txtAllAmount.Text)
        eobj.Category = "Pre-Allocation"
        eobj.TransStatus = "Open"
        eobj.PostStatus = "Posted"
        eobj.PostedDateTime = DateTime.Now
        eobj.UpdatedTime = DateTime.Now
        eobj.UpdatedBy = Session("User")
        eobj.DueDate = DateTime.Now
        eobj.CreatedDateTime = DateTime.Now
        eobj.KodUniversiti = Session("KodUniversiti")
        eobj.KumpulanPelajar = Session("KumpulanPelajar")
        eobj.TarikhProses = Session("TarikhProses")
        eobj.KodBank = Session("txtKodBank")
        Dim dgItem1 As DataGridItem
        Dim amount As TextBox
        Dim tempAmount As TextBox
        Dim chkselect As CheckBox
        'Dim NoKelompok As HiddenField = Nothing
        'Dim NoWarran As HiddenField = Nothing
        'Dim amaunWarran As HiddenField = Nothing
        'Dim noAkaunPelajar As HiddenField = Nothing
        'Dim statusBayaran As HiddenField = Nothing
        For Each dgItem1 In dgView.Items
            chkselect = dgItem1.Cells(0).Controls(1)
            If chkselect.Checked = True Then
                Dim NoKelompok As String = ""
                Dim NoWarran As String = ""
                Dim AmaunWarran As Double = 0.0
                Dim noAkaun As String = ""
                amount = dgItem1.Cells(7).Controls(1)
                tempAmount = dgItem1.Cells(9).Controls(1)
                'NoKelompok = dgItem1.Cells(13).Controls(1)
                'NoWarran = dgItem1.Cells(14).Controls(1)
                'amaunWarran = dgItem1.Cells(15).Controls(1)
                'noAkaunPelajar = dgItem1.Cells(16).Controls(1)
                eobjDetails = New AccountsDetailsEn
                eobjDetails.ReferenceCode = dgItem1.Cells(1).Text
                eobjDetails.PaidAmount = dgItem1.Cells(6).Text
                eobjDetails.TransactionAmount = CDbl(amount.Text)
                eobjDetails.TempAmount = CDbl(tempAmount.Text)
                eobjDetails.TransStatus = "Open"
                If dgItem1.Cells(13).Text.Trim = "&nbsp;" Then
                    NoKelompok = ""
                End If
                If dgItem1.Cells(14).Text.Trim = "&nbsp;" Then
                    NoWarran = ""
                End If
                If dgItem1.Cells(15).Text.Trim = "&nbsp;" Then
                    AmaunWarran = ""
                End If
                If dgItem1.Cells(16).Text.Trim = "&nbsp;" Then
                    noAkaun = ""
                End If
                eobjDetails.NoKelompok = NoKelompok
                eobjDetails.NoWarran = NoWarran
                eobjDetails.AmaunWarran = AmaunWarran
                eobjDetails.noAkaun = noAkaun
                'eobjDetails.StatusBayaran = statusBayaran.Value
                list.Add(eobjDetails)
                eobjDetails = Nothing
            End If
        Next
        eobj.AccountDetailsList = list
        If list.Count = 0 Then
            ErrorDescription = "Select At least One Student"
            lblMsg.Text = ErrorDescription
            Exit Sub

        End If
        If txtAllocateAmount.Text = 0 Then
            ErrorDescription = "Enter Valid Amount"
            lblMsg.Text = ErrorDescription
            Exit Sub
        End If
        If Not Session("spt") Is Nothing Then
            eospn.SponserCode = Session("spt")
        Else
            eospn.CreditRef = txtspcode.Text
        End If
        If Aflag = "Exit" Then
            Exit Sub
        End If

        splist.Add(eospn)
        lblMsg.Visible = True

        Try
            txtAutoNum.Text = bsobj.SponsorBatchUpdate(eobj, splist)
            ErrorDescription = "Record Posted Successfully "
            ibtnStatus.ImageUrl = "images/posted.gif"
            lblStatus.Value = "Posted"
            lblMsg.Text = ErrorDescription
            trFileGen.Visible = True
            eobj.TransStatus = "Posted"
            txtAutoNum.ReadOnly = False
            txtAutoNum.Text = eobj.BatchCode
            txtAutoNum.ReadOnly = True
            trFileGen.Visible = True

            'Remove item from List 
            If Not Session("ListObj") Is Nothing Then
                ListObjects = Session("ListObj")
                Session("ListObj") = ListObjects
                If lblStatus.Value = "Posted" Then
                    ibtnStatus.ImageUrl = "images/posted.gif"
                    lblStatus.Value = "Posted"
                    trFileGen.Visible = True
                End If
            End If

        Catch ex As Exception
            lblMsg.Text = ex.Message.ToString()
            LogError.Log("SponsorAllocation", "OnPost", ex.Message)

        End Try

    End Sub

    ''' <summary>
    ''' Method to Delete the Sponsor Allocations
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub ondelete()
        Dim RecAff As Boolean
        Dim eob As New AccountsEn
        Dim bsobj As New AccountsBAL
        If lblStatus.Value = "Ready" Then
            Try
                eob.BatchCode = Trim(txtAutoNum.Text)
                RecAff = bsobj.BatchDelete(eob)
                onAdd()
                DFlag = "Delete"
                Session("loaddata") = "View"
                lblMsg.Text = "Record Deleted Successfully "
                lblMsg.Visible = True
                LoadListObjects()
                'Session("ListObj") = ListObjects
            Catch ex As Exception
                lblMsg.Text = ex.Message.ToString()
                LogError.Log("SponsorAllocation", "OnDelete", ex.Message)
            End Try
        End If
    End Sub

    ''' <summary>
    ''' Method to Upload Files
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub uploadData()
        Dim lsDelimeter As String = ","
        Dim path As String = Session("file1")
        lblMsg.Text = ""
        'Dim spncode As String
        'FLBankFile.PostedFile.FileName
        'Dim loReader As New StreamReader("C:/Documents and Settings/Vijay/My Documents/test2.txt")
        Dim loReader As New StreamReader(path)
        ' Dim loWriter As New StreamWriter(Server.MapPath("Uploadfiles") & "\" & txtFName.Text)
        'Response.Write(FileUpload1.PostedFile.FileName)
        Dim eTstudent As StudentEn
        Dim listStudent As New List(Of StudentEn)
        Dim listUnStudent As New List(Of StudentEn)
        Dim list As New List(Of StudentEn)
        Dim alllist As New List(Of StudentEn)
        Dim i As Integer
        Dim objStu As New StudentBAL
        While loReader.Read() > 0

            Dim lsRow As String = loReader.ReadLine()
            Dim lsArr As String() = lsRow.Split(lsDelimeter.ToCharArray())
            eTstudent = New StudentEn
            Try

                eTstudent.MatricNo = lsArr(0)
                eTstudent.ICNo = lsArr(2).Trim()
                eTstudent.StudentName = lsArr(1).Trim()
                eTstudent.TransactionAmount = CDbl(lsArr(3).Trim())
                eTstudent.ProgramID = ""
                eTstudent.Faculty = ""
                eTstudent.CurrentSemester = 0
                eTstudent.SASI_StatusRec = True
                eTstudent.STsponsercode = New StudentSponEn()
                eTstudent.STsponsercode.Sponsor = Session("Scode")
            Catch ex As Exception
                lblMsg.Text = "File Cannot be Read"
            End Try

            If Session("Scode") Is Nothing Then
                lblMsg.Text = "Select At Least One Sponsor"
                Exit Sub
            End If
            'Check Student
            Try
                list = objStu.CheckStudentList(eTstudent)
            Catch ex As Exception
                LogError.Log("SponsorAllocation", "UploadData", ex.Message)
                Exit Sub
            End Try
            If list.Count = 0 Then
                eTstudent = New StudentEn '
                Try
                    eTstudent.StudentName = lsArr(1).Trim()
                    eTstudent.MatricNo = lsArr(0)
                    eTstudent.ICNo = lsArr(2).Trim()
                    eTstudent.StuIndex = i
                    eTstudent.TransactionAmount = CDbl(lsArr(3).Trim())
                    listUnStudent.Add(eTstudent)
                Catch ex As Exception
                    lblMsg.Text = "File Cannot be Read"
                    Exit Sub
                End Try
                'eTstudent = Nothing
            Else

                eTstudent.StudentName = list(0).StudentName
                eTstudent.MatricNo = list(0).MatricNo
                eTstudent.ICNo = list(0).ICNo
                eTstudent.ProgramID = list(0).ProgramID
                eTstudent.Faculty = list(0).Faculty
                eTstudent.CurrentSemester = list(0).CurrentSemester
                eTstudent.StuIndex = i
                Try
                    eTstudent.TransactionAmount = CDbl(lsArr(3).Trim())
                Catch ex As Exception
                    lblMsg.Text = "File Cannot be Read"
                    Exit Sub
                End Try

                listStudent.Add(eTstudent)
                eTstudent = Nothing
            End If
            i = i + 1

            '   loWriter.WriteLine(lsArr(CInt(txtMatrix.Text)).Trim() + "," + lsArr(CInt(txtICNo.Text)).Trim() + "," + lsArr(CInt(txtName.Text)).Trim() + "," + lsArr(CInt(txtAmount.Text)).Trim())
        End While
        loReader.Close()
        'loWriter.Close()
        Dim totalAmt As Double = 0
        Dim totalPCAmt As Double = 0
        Dim stuen As New StudentEn
        Dim bsstu As New AccountsBAL
        Dim outamt As Double = 0.0
        Dim eobj As New StudentEn
        Dim k As Integer
        If Not Session("stualloc") Is Nothing Then
            alllist = Session("stualloc")
        Else
            alllist = New List(Of StudentEn)
        End If
        If listStudent.Count <> 0 Then
            While k < listStudent.Count
                eobj = listStudent(k)
                Dim j As Integer = 0
                Dim Flag As Boolean = False
                While j < alllist.Count
                    If alllist(j).MatricNo = eobj.MatricNo Then
                        Flag = True
                        Exit While
                    End If
                    j = j + 1
                End While
                If Flag = False Then
                    alllist.Add(eobj)
                End If
                k = k + 1
            End While
        End If
        If alllist Is Nothing Then
            dgView.DataSource = Nothing
            dgView.DataBind()
        Else
            dgView.DataSource = alllist
            Session("stuupload") = alllist
            dgView.DataBind()
            Dim dgItem1 As DataGridItem
            Dim amt As Double
            Dim txtAmount As TextBox
            Dim txtpamount As TextBox
            For Each dgItem1 In dgView.Items
                stuen.MatricNo = dgItem1.Cells(1).Text

                Try
                    outamt = bsstu.GetStudentOutstandingAmt(stuen)
                Catch ex As Exception
                    LogError.Log("SponsorAllocation", "uploadData", ex.Message)
                End Try
                dgItem1.Cells(6).Text = String.Format("{0:F}", outamt)
                txtAmount = dgItem1.Cells(7).Controls(1)
                txtpamount = dgItem1.Cells(9).Controls(1)
                amt = CDbl(dgItem1.Cells(10).Text)
                txtAmount.Text = String.Format("{0:F}", amt)
                totalAmt = totalAmt + txtAmount.Text

            Next
        End If
        Dim totalAmt1 As Double = 0
        Dim totalPCAmt1 As Double = 0
        dgUnView.DataSource = listUnStudent
        dgUnView.DataBind()
        Dim dgItem2 As DataGridItem
        Dim amt1 As Double
        Dim txtAmount1 As TextBox


        For Each dgItem2 In dgUnView.Items
            txtAmount1 = dgItem2.Cells(7).Controls(1)
            amt1 = CDbl(dgItem2.Cells(10).Text)
            txtAmount1.Text = String.Format("{0:F}", amt1)

        Next
        'Else
        'End If
        Session("file1") = Nothing
    End Sub

    ''' <summary>
    ''' Method to Get a Total Amount of all Students in Grid
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub totalall()
        Dim totalAmt As Double = 0
        Dim totalPCAmt As Double = 0
        Dim dgItem1 As DataGridItem
        Dim txtAmount As TextBox
        Dim txtpamount As TextBox
        For Each dgItem1 In dgView.Items
            txtAmount = dgItem1.Cells(7).Controls(1)
            txtpamount = dgItem1.Cells(9).Controls(1)

            totalAmt = totalAmt + txtAmount.Text
            txtTotalPenAmt.Text = String.Format("{0:F}", totalAmt)
            totalPCAmt = totalPCAmt + txtpamount.Text
            txtAddedAmount.Text = String.Format("{0:F}", totalPCAmt)
        Next
        txtAfterBalance.Text = CDbl(txtTotalPenAmt.Text) + CDbl(txtAddedAmount.Text)
    End Sub

    ''' <summary>
    ''' Method to get Outstanding Total of Students
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub OutTotal()
        Dim dgItem1 As DataGridItem
        Dim txtAmount As Double = 0
        Dim totalAmt As Double = 0
        For Each dgItem1 In dgView.Items
            txtAmount = dgItem1.Cells(6).Text
            totalAmt = totalAmt + txtAmount
        Next
    End Sub

    ''' <summary>
    ''' Method to Disable Options After Posting
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub PostEnFalse()
        ibtnNew.Enabled = False
        ibtnNew.ImageUrl = "images/gadd.png"
        ibtnNew.ToolTip = "Access Denied"
        ibtnSave.Enabled = False
        ibtnSave.ImageUrl = "images/gsave.png"
        ibtnSave.ToolTip = "Access Denied"
        ibtnDelete.Enabled = False
        ibtnDelete.ImageUrl = "images/gdelete.png"
        ibtnDelete.ToolTip = "Access Denied"
        'ibtnView.Enabled = False
        'ibtnView.ImageUrl = "images/ready.png"
        'ibtnView.ToolTip = "Access Denied"
        ibtnPrint.Enabled = False
        ibtnPrint.ImageUrl = "images/gprint.png"
        ibtnPrint.ToolTip = "Access denied"
        'ibtnPosting.Enabled = False
        'ibtnPosting.ImageUrl = "images/gposting.png"
        'ibtnPosting.ToolTip = "Access denied"
        'ibtnOthers.Enabled = False
        'ibtnOthers.ImageUrl = "images/post.png"
        'ibtnOthers.ToolTip = "Access denied"

    End Sub

    ''' <summary>
    ''' Method to Search for Posted Records
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub OnSearchOthers()
        Session("loaddata") = "others"
        If lblCount.Text <> "" Then
            If CInt(lblCount.Text) > 0 Then
                onAdd()
            Else
                Session("PageMode") = "Edit"
                LoadListObjects()

            End If
        Else
            Session("PageMode") = "Edit"
            LoadListObjects()

            PostEnFalse()
        End If
        If lblCount.Text.Length = 0 Then
            Session("PageMode") = "Add"
        End If
    End Sub

    ''' <summary>
    ''' Method to Load Students Template
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub LoadStudentsTemplates(ByVal studentList As List(Of StudentEn))
        dgView.DataSource = Nothing
        dgView.DataBind()

        Dim list As New List(Of StudentEn)
        Dim eobj As New StudentEn
        Dim i As Integer = 0

        Dim dgItem1 As DataGridItem
        Dim txtAmount As TextBox
        Dim txtPocket As TextBox
        Dim amt As Double = 0.0
        Dim pocAmt As Double = 0.0
        Dim j As Integer = 0
        Dim stuen As New StudentEn
        Dim bsstu As New AccountsBAL
        Dim objStu As New StudentBAL
        Dim outamt As Double = 0.0
        'Dim NoKelompok As HiddenField = Nothing
        'Dim NoWarran As HiddenField = Nothing
        'Dim amaunWarran As HiddenField = Nothing
        'Dim noAkaunPelajar As HiddenField = Nothing
        'Dim statusBayaran As HiddenField = Nothing
        'dgView.PageSize = mylst.Count

        For Each stuItem As StudentEn In studentList
            eobj = New StudentEn

            eobj.MatricNo = stuItem.MatricNo
            eobj.ICNo = String.Empty
            eobj.StudentName = String.Empty
            eobj.TransactionAmount = 0.0
            eobj.ProgramID = String.Empty
            eobj.Faculty = String.Empty
            eobj.CurrentSemester = 0
            eobj.SASI_StatusRec = True
            eobj.STsponsercode = New StudentSponEn()
            eobj.STsponsercode.Sponsor = String.Empty
            eobj.NoKelompok = stuItem.NoKelompok
            eobj.NoWarran = stuItem.NoWarran
            eobj.AmaunWarran = stuItem.AmaunWarran
            eobj.noAkaun = stuItem.noAkaun
            eobj.StatusBayaran = stuItem.StatusBayaran
            Try
                list = objStu.CheckStudentList(eobj)
            Catch ex As Exception
                LogError.Log("SponsorAllocation", "UploadData", ex.Message)
                Exit Sub
            End Try
            If list.Count = 0 Then
                lblMsg.Text = "Invalid Matric No exists in uploaded file."
                lblMsg.Visible = True
                Session("fileSponsor") = Nothing
                Exit Sub
            End If
        Next

        dgView.DataSource = studentList
        dgView.DataBind()

        For Each dgItem1 In dgView.Items
            txtAmount = dgItem1.Cells(7).Controls(1)
            amt = CDbl(dgItem1.Cells(11).Text)
            txtPocket = dgItem1.Cells(9).Controls(1)
            pocAmt = CDbl(dgItem1.Cells(12).Text)
            txtAmount.Text = String.Format("{0:F}", amt)
            txtPocket.Text = String.Format("{0:F}", pocAmt)
            stuen.MatricNo = dgItem1.Cells(1).Text
            outamt = bsstu.GetStudentOutstandingAmt(stuen)
            dgItem1.Cells(6).Text = String.Format("{0:F}", outamt)
            'NoKelompok = dgItem1.Cells(13).Controls(1)
            'NoWarran = dgItem1.Cells(14).Controls(1)
            'amaunWarran = dgItem1.Cells(15).Controls(1)
            'noAkaunPelajar = dgItem1.Cells(16).Controls(1)

            'dgItem1.Cells(13).Text = eobj.NoKelompok
            'dgItem1.Cells(14).Text = eobj.NoWarran
            'dgItem1.Cells(15).Text = eobj.AmaunWarran
            'dgItem1.Cells(16).Text = eobj.noAkaun
        Next

        Session("spt") = Session("SPncode")
        Session("spnObj") = Nothing
        Session("liststu") = Nothing
        Session("SPncode") = Nothing
        Session("paidInvoices") = Nothing
        imgLeft1.ImageUrl = "images/b_white_left.gif"
        imgRight1.ImageUrl = "images/b_white_right.gif"
        btnBatchInvoice.CssClass = "TabButtonClick"
        imgLeft2.ImageUrl = "images/b_orange_left.gif"
        imgRight2.ImageUrl = "images/b_orange_right.gif"
        btnSelection.CssClass = "TabButton"
        chkSelectAll.Visible = True
        MultiView1.SetActiveView(View1)

    End Sub

#End Region

End Class
