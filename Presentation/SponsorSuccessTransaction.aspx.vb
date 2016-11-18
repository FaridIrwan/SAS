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
Partial Class SponsorSuccessTransaction
    Inherits System.Web.UI.Page
    Dim ListObjects As List(Of AccountsEn)
    Dim ListObjectsStudent As List(Of StudentEn)
    Dim GBFormat As System.Globalization.CultureInfo

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack() Then
            ''Loading User Rights
            LoadUserRights()
            LoadButton("notpostback")
            ''while loading list object make it nothing
            Session("ListObj") = Nothing
            ''load PageName
            Menuname(CInt(Request.QueryString("Menuid")))
            'Clear the Sessions
            Session("eobjstu") = Nothing
            Session("liststu") = Nothing
            Session("eobjspn") = Nothing
            Session("fileSponsor") = Nothing
            Session("fileType") = Nothing
            Session("StudentList") = Nothing
            Session("Menuid") = Request.QueryString("Menuid")
            Session("autonum") = Nothing
            tabStudStatus.Visible = False
            txtNoLaporan.Visible = False
            lblNoLaporan.Visible = False
            ibtnPrint.Attributes.Add("onclick", "new_window=window.open('RptLaporanPengkreditan.aspx?cat=" + txtHeaderNo.Value + "','Hanodale','width=520,height=400,resizable=0');new_window.focus();")
            btnupload.Attributes.Add("onclick", "new_window=window.open('FileSponsor.aspx','Hanodale','width=470,height=200,resizable=0');new_window.focus();")
        End If

        If Not Session("fileSponsor") Is Nothing And Session("fileType") = "excel" Then
            Dim importobj As New ImportData
            ListObjectsStudent = importobj.GetImportedSponsorData(Session("fileSponsor").ToString())
            LoadStudentsTemplates(ListObjectsStudent)
            System.IO.File.Delete(Session("fileSponsor"))
            Session("fileSponsor") = Nothing
            Session("fileType") = Nothing
        ElseIf Not Session("fileSponsor") Is Nothing And Session("fileType") = "text" Then
            ListObjectsStudent = readTextFile(Session("fileSponsor").ToString())
            If Session("Err") = Nothing Then
                LoadStudentsTemplates(ListObjectsStudent)
            End If
            System.IO.File.Delete(Session("fileSponsor"))
            Session("fileSponsor") = Nothing
            Session("fileType") = Nothing
        End If
    End Sub

#Region "Methods"
    ''' <summary>
    ''' Load button at top
    ''' </summary>
    ''' <param name="mode">Mode to load button</param>
    ''' <remarks></remarks>
    Private Sub LoadButton(ByVal mode As String)
        If mode = "notpostback" Then
            ibtnPrint.Enabled = False
            ibtnPrint.ImageUrl = "images/gprint.png"
            ibtnPrint.ToolTip = "Access Denied"

            ibtnOthers.Enabled = False
            ibtnOthers.ImageUrl = "images/gothers.png"
            ibtnOthers.ToolTip = "Access Denied"

            ibtnDelete.ImageUrl = "images/gdelete.png"
            ibtnDelete.ToolTip = "Access Denied"
            ibtnDelete.Enabled = False

            ibtnSave.Enabled = True
            ibtnSave.ImageUrl = "images/save.png"
            ibtnSave.ToolTip = "Edit"
        ElseIf mode = "aftersave" Then
            ibtnPrint.Enabled = True
            ibtnPrint.ImageUrl = "images/print.png"
            ibtnPrint.ToolTip = "Print"

            ibtnDelete.ImageUrl = "images/delete.png"
            ibtnDelete.Enabled = True

            ibtnSave.Enabled = False
            ibtnSave.ImageUrl = "images/gsave.png"
            ibtnSave.ToolTip = "Access Denied"
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
            LogError.Log("SponsorSuccessTransaction", "Menuname", ex.Message)
        End Try
        lblMenuName.Text = eobj.MenuName
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
            LogError.Log("SponsorSuccessTransaction", "LoadUserRights", ex.Message)
        End Try
        'Rights for Add

        If eobj.IsAdd = True Then
            'ibtnSave.Enabled = True
            'OnAdd()
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
        ibtnPrevs.Enabled = False
        ibtnPrevs.ImageUrl = "images/gnew_Prev.png"
        ibtnFirst.Enabled = False
        ibtnFirst.ImageUrl = "images/gnew_first.png"
        ibtnNext.Enabled = False
        ibtnNext.ImageUrl = "images/gnew_next.png"
        ibtnLast.Enabled = False
        ibtnLast.ImageUrl = "images/gnew_last.png"
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
    ''' Method to Reset All Fields
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub Reset()
        lblMsg.Text = ""
        txtKodUni.Text = ""
        txtKodBank.Text = ""
        txtKumpPelajar.Text = ""
        txtTarikhProcess.Text = ""
        txtNoLaporan.Text = ""
        txtNoLaporan.Visible = False
        chkSelectAll.Visible = False
        Session("ListStudent") = Nothing
        tabStudStatus.Visible = False
        dgView.DataSource = Nothing
        dgView.DataBind()
        lblNoLaporan.Visible = False
        ibtnSave.Enabled = True
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
        Dim amaunWarran As HiddenField = Nothing
        'Dim noAkaunPelajar As HiddenField = Nothing
        'Dim statusBayaran As HiddenField = Nothing
        'dgView.PageSize = mylst.Count

        For Each stuItem As StudentEn In studentList
            eobj = New StudentEn

            eobj.NoPelajar = stuItem.NoPelajar
            eobj.NoIC = stuItem.ICNo
            eobj.NamaPelajar = stuItem.NamaPelajar
            eobj.TransactionAmount = 0.0
            eobj.ProgramID = String.Empty
            eobj.Faculty = String.Empty
            eobj.CurrentSemester = 0
            eobj.SASI_StatusRec = True
            eobj.NoKelompok = stuItem.NoKelompok
            eobj.NoWarran = stuItem.NoWarran
            eobj.AmaunWarran = stuItem.AmaunWarran
            eobj.noAkaun = stuItem.noAkaun
            eobj.StatusBayaran = stuItem.StatusBayaran
            eobj.NilaiBersih = stuItem.NilaiBersih
            eobj.TarikhTransaksi = stuItem.TarikhTransaksi
            eobj.TarikhLupusWarran = stuItem.TarikhLupusWarran
            eobj.TarikhProses = stuItem.TarikhProses
            eobj.AmaunPotongan = stuItem.AmaunPotongan

            'Try
            '    list = objStu.CheckStudentList(eobj)
            'Catch ex As Exception
            '    LogError.Log("SponsorAllocation", "UploadData", ex.Message)
            '    Exit Sub
            'End Try
            'If list.Count = 0 Then
            '    lblMsg.Text = "Invalid Matric No exists in uploaded file."
            '    lblMsg.Visible = True
            '    Session("fileSponsor") = Nothing
            '    Exit Sub
            'End If
        Next
        Session("StudentList") = studentList
        dgView.DataSource = studentList
        dgView.DataBind()

        'For Each dgItem1 In dgView.Items
        '    stuen.MatricNo = dgItem1.Cells(1).Text
        '    dgItem1.Cells(6).Text = String.Format("{0:F}", eobj.NilaiBersih)
        'Next

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
        tabStudStatus.Visible = True
        MultiView1.SetActiveView(View1)
        ibtnSave.Enabled = True

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
            Response.Write("<script>javascript:alert(The File`s is empty. Error message: " & ex.Message & ")</script>")
        End Try
        Try
            For Each line As String In fileEntries
                Dim checkCol As String = line.Substring(0, 10)
                Dim _studentEN As New StudentEn
                Dim _studEnFromDB As New StudentEn
                Dim stud As New StudentBAL
                Dim _studAccFromDB As New AccountsEn
                Dim studAcc As New AccountsBAL
                Dim dtTrans As Date
                Dim dtLuput As Date

                If checkCol = "0000000000" Then
                    'Check Line for header
                    txtKodUni.Text = line.Substring(10, 2)
                    txtKumpPelajar.Text = line.Substring(13, 2)
                    txtTarikhProcess.Text = line.Substring(15, 8)
                    txtKodBank.Text = line.Substring(23, 2)
                ElseIf checkCol = "9999999999" Then
                    'Check Line for footer
                Else
                    _studentEN.NoIC = line.Substring(43, 12)
                    Dim strEnPelajar As New StudentEn
                    Dim strBalPelajar As New StudentBAL
                    strEnPelajar = strBalPelajar.GetStudInfo(_studentEN.NoIC)
                    _studentEN.NamaPelajar = line.Substring(55, 80)
                    _studentEN.NoPelajar = strEnPelajar.MatricNo
                    _studentEN.NoKelompok = line.Substring(0, 10)
                    _studentEN.NoWarran = line.Substring(15, 14)
                    _studentEN.AmaunWarran = line.Substring(135, 8)
                    _studentEN.AmaunPotongan = line.Substring(143, 8)
                    _studentEN.NilaiBersih = line.Substring(151, 8)
                    _studentEN.TarikhTransaksi = line.Substring(159, 8)
                    _studentEN.TarikhLupusWarran = line.Substring(167, 8)
                    _studentEN.noAkaun = line.Substring(175, 14)
                    _studentEN.StatusBayaran = line.Substring(191, 1)
                    _studentEN.AmaunWarran = String.Format("{0:000000.00}", _studentEN.AmaunWarran)
                    _studentEN.AmaunWarran = (_studentEN.AmaunWarran * 0.01).ToString("N2")
                    _studentEN.AmaunPotongan = String.Format("{0:000000.00}", _studentEN.AmaunPotongan)
                    _studentEN.AmaunPotongan = (_studentEN.AmaunPotongan * 0.01).ToString("N2")
                    _studentEN.NilaiBersih = String.Format("{0:000000.00}", _studentEN.NilaiBersih)
                    _studentEN.NilaiBersih = (_studentEN.NilaiBersih * 0.01).ToString("N2")
                    lstStudents.Add(_studentEN)
                End If
            Next
            readTextFile = lstStudents
        Catch ex As Exception
            Response.Write("<script>javascript:alert(Hint: Make Sure Fee Structure Exist For Students and valid for current Semester.)</script>")
            'MsgBox("Error message: " & ex.Message & "  *Hint: Make Sure Fee Structure Exist For Students and valid for current Semester.", MsgBoxStyle.Critical, "Error SAS")
            Session("Err") = "Error"
        End Try
        Return readTextFile
    End Function
    ''' <summary>
    ''' Method to Save and Update Sponsor Allocations 
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub onSave()
        Dim eobj As New AccountsEn
        Dim list As New List(Of AccountsDetailsEn)
        Dim bsobj As New AccountsBAL
        Dim dgItem1 As DataGridItem
        Dim chkselect As CheckBox

        GBFormat = New System.Globalization.CultureInfo("en-GB")
        eobj.NoKelompok_H = "0000000000"
        eobj.KumpulanPelajar = txtKumpPelajar.Text
        eobj.KodUniversiti = txtKodUni.Text
        eobj.TarikhProses = txtTarikhProcess.Text
        eobj.KodBank = txtKodBank.Text
        eobj.NoKelompok_F = "9999999999"
        For Each dgItem1 In dgView.Items
            Dim eobjDetails As New AccountsDetailsEn
            chkselect = dgItem1.Cells(0).Controls(1)
            If chkselect.Checked = True Then
                eobjDetails.NoPelajar = dgItem1.Cells(1).Text.Trim
                eobjDetails.NamaPelajar = dgItem1.Cells(2).Text.Trim
                eobjDetails.NoIC = dgItem1.Cells(3).Text.Trim
                eobjDetails.noAkaun = dgItem1.Cells(4).Text.Trim
                eobjDetails.StatusBayaran = dgItem1.Cells(5).Text.Trim
                eobjDetails.AmaunWarran = dgItem1.Cells(6).Text.Trim
                eobjDetails.AmaunPotongan = dgItem1.Cells(7).Text.Trim
                eobjDetails.NilaiBersih = dgItem1.Cells(8).Text.Trim
                eobjDetails.Filler = ""
                list.Add(eobjDetails)
                eobjDetails = Nothing
            End If
        Next
        eobj.AccountDetailsList = list
        If list.Count = 0 Then
            lblMsg.Text = "Select At least One Student"
            Exit Sub
        End If

        lblMsg.Visible = True
        'If Session("PageMode") = "Add" Then
        Try
            Dim result As Boolean
            result = bsobj.SucceedTransaction(eobj)
            If result = True Then
                lblMsg.Text = "Record Saved Successfully "
            Else
                lblMsg.Text = "Failed To Save Record "
            End If
            LoadButton("aftersave")
        Catch ex As Exception
            lblMsg.Text = ex.Message.ToString()
            LogError.Log("SponsorSuccessTransaction", "Onsave", ex.Message)
        End Try
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
                Dim obj As New AccountsEn
                ListObjects = Session("ListObj")
                obj = ListObjects(RecNo)
                txtHeaderNo.Value = obj.AutoNum
                txtKodUni.Text = obj.KodUniversiti
                txtKumpPelajar.Text = obj.KumpulanPelajar
                txtTarikhProcess.Text = obj.TarikhProses
                txtKodBank.Text = obj.KodBank
                txtNoLaporan.Text = obj.AutoNum
                Dim espn As New List(Of AccountsDetailsEn)
                Dim bospn As New AccountsDetailsBAL

                Try
                    espn = bospn.GetItemDetails(obj)
                    'espn = obj.AccountDetailsList
                Catch ex As Exception
                    LogError.Log("SponsorSuccedTransaction", "FillData", ex.Message)
                End Try
                Session("ListStudDetails") = Nothing
                Session("ListStudDetails") = espn
                dgView.DataSource = espn
                dgView.DataBind()
                MultiView1.SetActiveView(View1)

                Dim chk As CheckBox
                Dim dgItem1 As DataGridItem
                Dim j As Integer = 0

                While j < espn.Count
                    For Each dgItem1 In dgView.Items
                        'chk.Checked = True
                        'chk.Enabled = False
                        dgItem1.Cells(1).Text = espn(j).NoPelajar
                        dgItem1.Cells(2).Text = espn(j).NamaPelajar
                        dgItem1.Cells(3).Text = espn(j).NoIC
                        dgItem1.Cells(4).Text = espn(j).noAkaun
                        dgItem1.Cells(5).Text = espn(j).StatusBayaran
                        dgItem1.Cells(6).Text = espn(j).AmaunWarran
                        dgItem1.Cells(7).Text = espn(j).AmaunPotongan
                        dgItem1.Cells(8).Text = espn(j).NilaiBersih
                        j = j + 1
                        Exit For
                    Next
                End While
            End If
        End If
        'setDateFormat()
    End Sub
    ''' <summary>
    ''' Method to get the List Of Sponsor Allocations
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub LoadListObjects()
        Dim obj As New AccountsBAL
        Dim eob As New AccountsEn
        Try
            ListObjects = obj.GetListSucceedHeader()
        Catch ex As Exception
            LogError.Log("SponsorAllocation", "LoadListObjects", ex.Message)
        End Try
        LoadButton("aftersave")
        txtNoLaporan.Visible = True
        lblNoLaporan.Visible = True
        Session("ListObj") = ListObjects
        lblCount.Text = ListObjects.Count.ToString()
        If ListObjects.Count <> 0 Then
            DisableRecordNavigator()
            txtRecNo.Text = "1"
            OnMoveFirst()
            'If Session("EditFlag") = True Then
            '    ibtnSave.Enabled = False
            '    ibtnSave.ImageUrl = "images/save.png"
            '    lblMsg.Visible = True

            'Else
            '    ibtnSave.Enabled = False
            '    ibtnSave.ImageUrl = "images/gsave.png"
            '    Session("PageMode") = ""
            'End If
            ibtnSave.Enabled = False
            ibtnSave.ImageUrl = "images/save.png"
            lblMsg.Visible = True

        Else
            txtRecNo.Text = ""
            lblCount.Text = ""

            'If DFlag = "Delete" Then
            'Else
            '    lblMsg.Visible = True
            '    ErrorDescription = "Record did not Exist"
            '    lblMsg.Text = ErrorDescription
            '    DFlag = ""
            '    Session("PageMode") = "Add"
            'End If
        End If
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
    ''' Method to Move to Clear Record
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub OnClear()
        txtKodBank.Text = ""
        txtKodUni.Text = ""
        txtKumpPelajar.Text = ""
        txtTarikhProcess.Text = ""
        txtNoLaporan.Text = ""
        chkSelectAll.Visible = False
        dgView.DataSource = Nothing
        dgView.DataBind()
    End Sub
    ''' <summary>
    ''' Method to Delete Record
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub OnDelete(ByVal noLaporan As String)
        Dim loDel As New AccountsBAL
        Dim argEn As New AccountsEn
        Dim result As Boolean

        argEn.AutoNum = noLaporan
        Try
            result = loDel.DeleteSucceedTrans(argEn)
            If result = True Then
                lblMsg.Text = "Successfully Delete Succeed Transaction"
                OnClear()
                LoadListObjects()
            End If
        Catch ex As Exception
            Throw ex
        End Try

    End Sub
#End Region

    Protected Sub ibtnCancel_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ibtnCancel.Click
        Reset()
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
        'LoadTotals()

    End Sub
    Protected Sub dgView_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgView.SelectedIndexChanged

    End Sub
    Protected Sub dgView_PageIndexChanged(ByVal sender As Object, ByVal e As DataGridPageChangedEventArgs) Handles dgView.PageIndexChanged
        dgView.CurrentPageIndex = e.NewPageIndex
        LoadStudentsTemplates(Session("StudentList"))
    End Sub
    Protected Sub Chk_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)

        Dim chk As CheckBox
        Dim dgItem1 As DataGridItem
        For Each dgItem1 In dgView.Items
            chk = dgItem1.Cells(0).Controls(1)
            If chk.Checked = True Then
                'LoadTotals()
            Else
                'txtAllocateAmount.Text = ""
                'LoadTotals()
            End If
        Next

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
        'LoadTotals()
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
        'LoadTotals()
    End Sub
    Protected Sub btnHidden_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnHidden.Click

    End Sub

    Protected Sub ibtnNew_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ibtnNew.Click
        Reset()
    End Sub

    Protected Sub ibtnSave_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ibtnSave.Click
        onSave()
    End Sub

    Protected Sub ibtnView_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ibtnView.Click
        LoadListObjects()
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

    Protected Sub ibtnDelete_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ibtnDelete.Click
        If txtNoLaporan.Text <> "" Then
            OnDelete(txtNoLaporan.Text)
        End If
    End Sub

    Protected Sub ibtnPrint_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ibtnPrint.Click
        Try
            Session("autonum") = txtHeaderNo.Value
        Catch ex As Exception
            Throw ex
        Finally

        End Try
    End Sub
End Class
