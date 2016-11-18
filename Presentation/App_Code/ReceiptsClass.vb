#Region "NameSpaces "

Imports MaxGeneric
Imports HTS.SAS.Entities
Imports System.Web.HttpContext
Imports Microsoft.VisualBasic
Imports HTS.SAS.BusinessObjects
Imports System.Web.UI.WebControls
Imports System.Collections.Generic
Imports HTS.SAS.DataAccessObjects

#End Region

Public Class ReceiptsClass

#Region "Global Declarations "

    Private _StudentBAL As New StudentBAL()

#End Region

#Region "Constants "

    Public Const StatusNew As String = "New"
    Public Const Debit As String = "Debit"
    Public Const Credit As String = "Credit"
    Public Const Student As String = "Student"
    Public Const Sponsor As String = "Sponsor"
    Public Const Loan As String = "Loan"
    Public Const ReceiptStudent As String = "St"
    Public Const ReceiptSponsor As String = "Sp"
    Public Const StudentLoan As String = "Sl"
    Public Const AllocateAuto As String = "Auto"
    Public Const StatusOpen As String = "Open"
    Public Const StatusReady As String = "Ready"
    Public Const StatusEdit As String = "Edit"
    Public Const StatusView As String = "View"
    Public Const StatusDelete As String = "Delete"
    Public Const StatusPosted As String = "Posted"
    Public Const AllocateManual As String = "Manual"
    Public Const CategoryReceipt As String = "Receipt"
    Public Const CategoryLoan As String = "Loan"
    Public Const PaymentModeCash As String = "CSH"
    Private Const InvoiceCategory As String = "'Invoice','Debit Note','AFC'"
    Public Const CategoryCimbClicks As String = "CIMBCLICKS"
    Public Const FeeCtgyAdmission As String = "A"
    Public Const FeeCtgyTuition As String = "T"
    Public Const FeeCtgyHostel As String = "H"
    Public Const Student_BUKAN_WARGANEGARA As String = "BW"
    Public Const Student_WARGANEGARA As String = "W"
    Public Const student_Local As String = "Local"
    Public Const student_International As String = "International"

    'Session Constants - Start
    Public Const SessionRecordNo As String = "Recno"
    Public Const SessionEditFlag As String = "EditFlag"
    Public Const SessionPayMode As String = "paymode"
    Public Const SessionBankCode As String = "bankcode"
    Public Const SessionPageMode As String = "PageMode"
    Public Const SessionLoadData As String = "loaddata"
    Public Const SessionListView As String = "listview"
    Public Const SessionListObject As String = "ListObj"
    Public Const SessionReceiptList As String = "RecptList"
    Public Const SessionscHostel As String = "scHostel"
    Public Const SessionscProgram As String = "scProgram"
    Public Const SessionscSponsor As String = "scSponsor"
    Public Const SessionStuChange As String = "stuChange"
    Public Const SessionStuChgMatricNo As String = "StuChgMatricNo"
    Public Const SessionStuToSave As String = "StuToSave"
    Public Const SessionTrackingNotes As String = "TrackingNotes"
    Public Const SessionSponsorList As String = "SponsorList"
    'Session Constants - Stop

#End Region

#Region "Load Student List To Grid "

    'Purpose			: Load Student Grid
    'Author			    : Sujith Sharatchandran - T-Melmax Sdn Bhd
    'Created Date		: 03/06/2015
    'Modified by Hafiz @ 30/3/2016

    Public Function LoadStudentListToGrid(ByRef ListStudent As List(Of StudentEn),
        ByRef ListStudentView As List(Of StudentEn), ByRef StudentList As DataGrid, ByRef msg As String) As Boolean

        'Create Instances - Start
        Dim ListStudentDetails As New StudentEn()
        Dim UpdatedStudentDetails As New StudentEn()
        'Create Instances - Stop

        'VAriable Declarations - Start
        Dim IsStudentInList As Boolean = False, Index_1 As Integer = 0
        Dim Index As Integer = 0, TotalSelectedStudents As Integer = 0, StudentMatricNo As String = Nothing
        'VAriable Declarations - Stop

        Try

            'Get Total Selected Students
            TotalSelectedStudents = ListStudent.Count

            'If Student List Available - Start
            If TotalSelectedStudents > 0 Then

                'Loop thro the selected Students - Start
                While Index < TotalSelectedStudents

                    'Get Student Details from List
                    ListStudentDetails = ListStudent(Index)

                    'Get Matric No
                    StudentMatricNo = ListStudentDetails.MatricNo

                    'Get Updated Student Details
                    UpdatedStudentDetails = _StudentBAL.GetItem(StudentMatricNo)

                    'Update Student Info - Start
                    ListStudentDetails.ICNo = UpdatedStudentDetails.ICNo
                    ListStudentDetails.ProgramID = UpdatedStudentDetails.ProgramID
                    ListStudentDetails.StudentName = UpdatedStudentDetails.StudentName
                    ListStudentDetails.CurrentSemester = UpdatedStudentDetails.CurrentSemester
                    'Update Student Info - Stop

                    'Set Flag to False
                    IsStudentInList = False
                    msg = Nothing

                    'Loop thro the list to check if student exist - Start
                    While Index_1 < ListStudentView.Count

                        'if student exist - Start
                        If ListStudentView(Index_1).MatricNo = ListStudentDetails.MatricNo And
                            ListStudentView(Index_1).noAkaun = ListStudentDetails.noAkaun Then

                            'Set Flag to true
                            IsStudentInList = True

                            msg = "Matric No Already Exists"

                            Exit While

                        End If
                        'if student exist - Stop

                        'Increment Index
                        Index_1 = Index_1 + 1

                    End While
                    'Loop thro the list to check if student exist - Stop

                    'if Student not in list - Start
                    If Not IsStudentInList Then
                        'Add Student to List
                        ListStudentView.Add(ListStudentDetails)
                    End If
                    'if Student not in list - Stop

                    'Increment Index
                    Index = Index + 1

                End While
                'Loop thro the selected Students - Stop

            End If
            'If Student List Available - Stop

            'if Populate Student Grid Failed - Start
            If Not PopulateStudentGrid(ListStudentView, StudentList) Then
                Return False
            End If
            'if Populate Student Grid Failed - Stop

            Return True

        Catch ex As Exception

            Call MaxModule.Helper.LogError(ex.Message)
            Return False

        End Try

    End Function

#End Region

#Region "Populate Student Data Grid "

    'Purpose			: Populate Student Data Grid
    'Author			    : Sujith Sharatchandran - T-Melmax Sdn Bhd
    'Created Date		: 03/06/2015
    Private Function PopulateStudentGrid(ByVal ListStudentView As List(Of StudentEn),
        ByRef StudentList As DataGrid) As Boolean

        'Create Instances - Start
        Dim stud As New StudentEn
        Dim _AccountsDal As New AccountsDAL
        Dim ManualSelected As CheckBox = Nothing
        Dim StudentSelected As CheckBox = Nothing
        Dim OutstandingAmountText As TextBox = Nothing
        'Create Instances - Stop

        'Variable Declarations
        Dim Index As Integer = 0, StudentMatricNo As String = Nothing, TransactionAmount As String = Nothing
        Dim GetStudOutstndAmt As String = Nothing, txt_transdate As Date = Nothing

        Try

            'If Student List Available - Start
            If Not ListStudentView Is Nothing Then

                'Add empty Data Table at Below
                ListStudentView.Add(stud)

                'Bind Data Grid - Start
                StudentList.DataSource = ListStudentView
                StudentList.DataBind()
                'Bind Data Grid - Stop

                'Loop thro the Selected Student List - Start
                While Index < ListStudentView.Count - 1

                    'Loop thro the grid items - Start
                    For Each _DataGridItems In StudentList.Items

                        'Get Student Matric No from Grid
                        StudentMatricNo = _DataGridItems.Cells(1).Text

                        'if matric no matches List Student Matric no - Start
                        If ListStudentView(Index).MatricNo = StudentMatricNo Then

                            'Get Check Boxes - Start
                            ManualSelected = _DataGridItems.Cells(8).Controls(1)
                            StudentSelected = _DataGridItems.Cells(0).Controls(1)
                            'Get Check Boxes - Stop

                            'Set Student Selected
                            StudentSelected.Checked = True

                            'get Outstanding amount text box
                            OutstandingAmountText = _DataGridItems.Cells(9).Controls(1)

                            'Get Transaction Amount
                            TransactionAmount = clsGeneric.NullToDecimal(_DataGridItems.Cells(10).Text)

                            'if Student has Paid details - Start
                            If Not ListStudentView(Index).AccountDetailsList Is Nothing Then
                                If ListStudentView(Index).AccountDetailsList.Count > 0 Then

                                    'Set Paid Amount
                                    _DataGridItems.Cells(12).Text = ListStudentView(Index).AmountPaid

                                    'Set Manual as Checked
                                    ManualSelected.Checked = True
                                    _DataGridItems.Cells(13).Text = False

                                End If
                            Else
                                'Set Manual as Checked
                                ManualSelected.Checked = False
                                _DataGridItems.Cells(13).Text = True

                            End If
                            'if Student has Paid details - Stop

                            'Set Outstanding Amount
                            OutstandingAmountText.Text = String.Format("{0:F}", TransactionAmount)

                            'Set Index value as Id
                            _DataGridItems.Cells(11).Text = Index

                            'Set Outstanding Amount - Start
                            '_DataGridItems.Cells(18).Text = clsGeneric.SetCurrencyFormat(_AccountsDal.GetStudentOutstandingAmount(StudentMatricNo))
                            'GetStudOutstndAmt = clsGeneric.SetCurrencyFormat(_AccountsDal.GetStudentOutstandingAmount(StudentMatricNo))

                            'Modified by Hafiz @ 04/3/2016
                            'Outstanding amount calculation to tally Student Ledger - start
                            Dim _stud As New StudentEn
                            _stud = _AccountsDal.GetStudentOutstanding(StudentMatricNo)

                            GetStudOutstndAmt = (_stud.OutstandingAmount) + (_stud.LoanAmount)
                            'Outstanding amount calculation to tally Student Ledger - end

                            'cast
                            DirectCast(_DataGridItems.FindControl("Outstanding_Amount"), Label).Text = String.Format("{0:F}", CDbl(GetStudOutstndAmt))
                            'assign to list Outstanding_Amount
                            _DataGridItems.Cells(19).Text = String.Format("{0:F}", CDbl(GetStudOutstndAmt))
                            ListStudentView(Index).Outstanding_Amount = GetStudOutstndAmt
                            'Set Outstanding Amount - Stop

                            DirectCast(_DataGridItems.FindControl("BankSlipID"), TextBox).Text = ListStudentView(Index).BankSlipID

                            'Set Today's Date - Start
                            txt_transdate = ListStudentView(Index).ReceiptDate.ToString("yyyy-MM-dd")

                            If Not txt_transdate = "0001-01-01" Then
                                DirectCast(_DataGridItems.FindControl("txtTransDate"), TextBox).Text = ListStudentView(Index).ReceiptDate.ToString("yyyy-MM-dd")
                            Else
                                DirectCast(_DataGridItems.FindControl("txtTransDate"), TextBox).Text = DateTime.Now.ToString("yyyy-MM-dd")
                            End If
                            'Set Today's Date - End

                            'Set MatricNo Textbox
                            DirectCast(_DataGridItems.FindControl("MatricNo"), TextBox).Text = StudentMatricNo

                            Exit For

                        End If
                        'if matric no matches List Student Matric no - Stop

                    Next
                    'Loop thro the grid items - Stop

                    'Increment Index
                    Index = Index + 1

                End While
                'Loop thro the Selected Student List - Stop

            End If
            'If Student List Available - Stop

            Return True

        Catch ex As Exception

            Call MaxModule.Helper.LogError(ex.Message)
            Return False

        End Try

    End Function

#End Region

#Region "Load Student Invoices to Grid "

    'Purpose			: Populate Student Invoices
    'Author			    : Sujith Sharatchandran - T-Melmax Sdn Bhd
    'Created Date		: 03/06/2015
    Public Function LoadStudentInvoicesToGrid(ByVal _DataGridItem As DataGridItem,
        ByVal StudentStatus As String, ByVal StudentList As DataGrid,
        ByRef StudentInvoices As DataGrid, ByRef TotalAllocatedAmount As String) As Boolean

        'Create Instances - Start
        Dim _AccountsEn As New AccountsEn
        Dim _AccountsBAL As New AccountsBAL
        Dim ListStudentEn As New List(Of StudentEn)
        Dim StudentInvoicesList As New List(Of AccountsEn)
        'Create Instances - Stop

        'Variable Declarations - Start
        Dim AllocatedAmountText As TextBox = Nothing, _DataGridItem_1 As DataGridItem
        Dim AllocatedAmount As Decimal = 0, TotalAmount As Decimal = 0, PaidAmount As Decimal = 0
        Dim StudentMatricNo As String = Nothing, GridItemIndex As Integer = 0, Index As Integer = 0
        Dim TransactionAmount As Decimal = 0, OutstandingAmount As Decimal = 0, SelectCheckBox As CheckBox = Nothing
        'Variable Declarations - Stop

        Try

            'Get Grid Itemindex
            GridItemIndex = _DataGridItem.ItemIndex

            'Get Student Matric No
            StudentMatricNo = StudentList.Items(GridItemIndex).Cells(2).Text

            'Set Where Condition Values - Start
            _AccountsEn.SubType = Student
            _AccountsEn.PostStatus = StatusPosted
            _AccountsEn.Category = InvoiceCategory
            _AccountsEn.CreditRef = StudentMatricNo
            'Set Where Condition Values - Stop

            Select Case StudentStatus

                Case StatusNew

                    'Get Student Invoice List - Start
                    StudentInvoicesList = _AccountsBAL.
                        GetStudentAutoAllocation(_AccountsEn)
                    'Get Student Invoice List - Stop

                Case StatusReady, StatusPosted

                    'Get Student List from Session
                    ListStudentEn = Current.Session(SessionListView)

                    'Loop thro the Student List - Start
                    While Index < ListStudentEn.Count

                        'if Student Matric No Matches - Start
                        If ListStudentEn(Index).MatricNo = StudentMatricNo Then

                            'Get Student Invoices
                            StudentInvoicesList = ListStudentEn(Index).Listtaccounts
                            Exit While

                        End If
                        'if Student Matric No Matches - Stop

                        'Incerment Index
                        Index = Index + 1

                    End While
                    'Loop thro the Student List - Stop

                    'if Student Invoice List not available - Start
                    If StudentInvoicesList Is Nothing Then

                        'Get Student Invoice List - Start
                        StudentInvoicesList = _AccountsBAL.
                            GetStudentAutoAllocation(_AccountsEn)
                        'Get Student Invoice List - Stop

                    End If
                    'if Student Invoice List not available - Stop

            End Select

            'Populate Invoices Data grid - Start
            StudentInvoices.DataSource = StudentInvoicesList
            StudentInvoices.DataBind()
            'Populate Invoices Data grid - Stop

            'Loop thro the Student Invoices Data Grid - Start
            For Each _DataGridItem_1 In StudentInvoices.Items

                'Get Allocated Amount Text Box
                AllocatedAmountText = _DataGridItem_1.Cells(5).Controls(1)

                'Set Allocated Amount
                AllocatedAmountText.Text = String.Format("{0:F}", AllocatedAmount)

                'Add Total Allocated Amount
                TotalAmount += TotalAmount + AllocatedAmount

                'Get/Set Transaction Amount - Start
                TransactionAmount = clsGeneric.NullToDecimal(_DataGridItem_1.Cells(4).Text)
                _DataGridItem_1.Cells(4).Text = clsGeneric.SetCurrencyFormat(TransactionAmount)
                'Get/Set Transaction Amount - Stop

                'Get/Set Paid Amount - Start
                PaidAmount = clsGeneric.NullToDecimal(_DataGridItem_1.Cells(8).Text)
                _DataGridItem_1.Cells(8).Text = clsGeneric.SetCurrencyFormat(PaidAmount)
                'Get/Set Paid Amount - Stop

                'Get Outstanding Amount
                OutstandingAmount = TransactionAmount - PaidAmount

                'Set Outstanding Amount
                _DataGridItem_1.Cells(6).Text = String.Format("{0:F}", OutstandingAmount)

                'Get Grid Row Select Check Box
                SelectCheckBox = _DataGridItem_1.Cells(0).Controls(1)

                'if Student Status is Ready or Posted - Start
                If StudentStatus = StatusReady Or StudentStatus = StatusPosted Then

                    'Set Checkbox as Selected
                    SelectCheckBox.Checked = True

                End If
                'if Student Status is Ready or Posted - Stop

            Next
            'Loop thro the Student Invoices Data Grid - Stop

            'Set Total Allocated Amount
            TotalAllocatedAmount = String.Format("{0:F}", TotalAmount)

            Return True

        Catch ex As Exception

            Call MaxModule.Helper.LogError(ex.Message)
            Return False

        End Try

    End Function

#End Region

#Region "On Save Student Receipt "

    'Purpose			: On Save Student Receipt
    'Author			    : Sujith Sharatchandran - T-Melmax Sdn Bhd
    'Created Date		: 03/06/2015
    'Modified by Hafiz @ 24/3/2016

    Public Function OnSaveStudentReceipt(ByVal StudentList As DataGrid,
                ByVal SubType As String, ByVal DoneBy As String, ByVal BatchDate As String, ByVal Description As String, ByVal ReceiptDate As String,
                ByVal ReferenceNo As String, ByVal PaymentMode As String, ByVal BankCode As String, ByVal TotalAllocatedAmount As Decimal,
                ByVal ControlAmt As Decimal, ByRef BatchCode As String, ByRef err_msg As String) As Boolean

        'Create Instances - Start
        Dim _AccountsEn As New AccountsEn
        Dim _AccountsBAL As New AccountsBAL
        Dim _AccountsDAL As New AccountsDAL
        Dim _AccountsDetailsDAL As New AccountsDetailsDAL
        Dim _AccountsEn_1 As New AccountsEn
        Dim ListStudentEn As New List(Of StudentEn)
        Dim StudentInvoicesList As New List(Of AccountsEn)
        Dim ListAccountsDetails As New List(Of AccountsDetailsEn)
        'Create Instances - Stop

        'Variable Declarations - Start
        Dim TransAmountText As TextBox = Nothing, TransactionAmount As Decimal = 0, Index As Integer = 0, PaidAmount As Decimal = 0
        Dim ManualSelected As CheckBox = Nothing, _DataGridItem As DataGridItem = Nothing, OutStandingAmount As Decimal = 0
        Dim StudentMatricNo As String = Nothing, StudentSelected As CheckBox = Nothing, DetailTransactionAmount As Decimal = 0
        Dim txt_matricno As TextBox = Nothing
        'Variable Declarations - Stop

        Try

            'Set Account Entities Values - Start
            _AccountsEn.SubType = SubType
            _AccountsEn.CreatedBy = DoneBy
            _AccountsEn.UpdatedBy = DoneBy
            _AccountsEn.TransType = Credit
            _AccountsEn.BankCode = BankCode
            _AccountsEn.BatchDate = BatchDate
            _AccountsEn.TransDate = ReceiptDate
            _AccountsEn.TransStatus = StatusOpen
            _AccountsEn.Description = Description
            _AccountsEn.PostStatus = StatusReady
            _AccountsEn.Category = CategoryReceipt
            _AccountsEn.SubReferenceOne = ReferenceNo
            _AccountsEn.PaymentMode = PaymentMode
            _AccountsEn.PostedDateTime = DateTime.Now
            _AccountsEn.DueDate = DateTime.Now
            _AccountsEn.UpdatedTime = DateTime.Now
            _AccountsEn.ChequeDate = DateTime.Now
            _AccountsEn.CreatedDateTime = DateTime.Now
            _AccountsEn.TransactionAmount = TotalAllocatedAmount
            _AccountsEn.ControlAmt = ControlAmt
            'Set Account Entities Values - Stop

            'Loop thro the Data Grid Item - Start
            For Each _DataGridItem In StudentList.Items
                'original part/code deleted by Hafiz Roslan @ 20/1/2016

                'If Student Selected - Start
                'If StudentSelected.Checked Then
                txt_matricno = _DataGridItem.Cells(2).Controls(1)

                If Not txt_matricno.Text = "" Then

                    'Get Values - Start
                    TransAmountText = _DataGridItem.Cells(9).Controls(1)
                    StudentMatricNo = txt_matricno.Text

                    _AccountsEn.ReceiptDate = DirectCast(_DataGridItem.FindControl("txtTransDate"), TextBox).Text

                    TransactionAmount = clsGeneric.NullToDecimal(TransAmountText.Text)
                    'Get Values - Stop

                    'Create Instance of Student Entity
                    Dim _StudentEn As New StudentEn

                    'Set Values - Start
                    _StudentEn.MatricNo = _DataGridItem.Cells(1).Text
                    _StudentEn.StudentName = _DataGridItem.Cells(3).Text
                    _StudentEn.ICNo = _DataGridItem.Cells(4).Text
                    _StudentEn.CurrentSemester = _DataGridItem.Cells(7).Text
                    _StudentEn.TransactionAmount = String.Format("{0:F}", TransactionAmount)
                    _StudentEn.PaidAmount = 0.0
                    _StudentEn.TransDate = DateTime.Now
                    _StudentEn.BatchDate = DateTime.Now
                    _StudentEn.PostedDateTime = DateTime.Now
                    _StudentEn.CreatedDateTime = DateTime.Now
                    _StudentEn.ChequeDate = DateTime.Now
                    _StudentEn.UpdatedTime = DateTime.Now
                    _StudentEn.CurrencyUsed = "0.0"
                    _StudentEn.BatchTotal = 0.0
                    _StudentEn.TaxPercentage = 0.0
                    _StudentEn.DiscountPercentage = 0.0
                    _StudentEn.TaxAmount = 0.0
                    _StudentEn.DiscountAmount = 0.0
                    _StudentEn.TempAmount = 0.0
                    _StudentEn.TempPaidAmount = 0.0
                    _StudentEn.ReceiptDate = DirectCast(_DataGridItem.FindControl("txtTransDate"), TextBox).Text
                    _StudentEn.BankSlipID = DirectCast(_DataGridItem.FindControl("BankSlipID"), TextBox).Text
                    'Set Values - Stop

                    'added by Hafiz @ 23/3/2016
                    'add outstanding amount at sas_accounts - start
                    _StudentEn.Outstanding_Amount = DirectCast(_DataGridItem.FindControl("Outstanding_Amount"), Label).Text
                    'add outstanding amount at sas_accounts - end

                    'updated by Hafiz Roslan @ 2/2/2016
                    'why? check BSN first before inserting - START
                    _AccountsDAL.CheckDuplicateBankSlipNo(_StudentEn.BankSlipID, _StudentEn.MatricNo, BatchCode)
                    'why? check BSN first before inserting - END

                    'if manual allocation not checked - Start
                    'If Not ManualSelected.Checked Then

                    'Set Where Condition Values - Start
                    _AccountsEn_1.SubType = Student
                    _AccountsEn_1.PostStatus = StatusPosted
                    _AccountsEn_1.Category = InvoiceCategory
                    _AccountsEn_1.CreditRef = StudentMatricNo
                    'Set Where Condition Values - Stop

                    'Get Student Invoice List - Start
                    StudentInvoicesList = _AccountsBAL.GetStudentAutoAllocation(_AccountsEn_1)
                    'Get Student Invoice List - Stop

                    'Set Values - Start
                    _StudentEn.SubReferenceTwo = AllocateAuto
                    'Set Values - Stop

                    While Index < StudentInvoicesList.Count

                        'Get Values - Start 
                        PaidAmount = StudentInvoicesList(Index).PaidAmount
                        OutStandingAmount = StudentInvoicesList(Index).TransactionAmount
                        'Get Values - Stop

                        'Get Outstanding Amount - Start
                        If OutStandingAmount > PaidAmount Then
                            OutStandingAmount = OutStandingAmount - PaidAmount
                        Else
                            OutStandingAmount = PaidAmount - OutStandingAmount
                        End If
                        'Get Outstanding Amount - Stop

                        'if Outstanding Amount Less than Transaction Amount - Start
                        If OutStandingAmount <= TransactionAmount Then
                            DetailTransactionAmount = OutStandingAmount
                        End If
                        'if Outstanding Amount Less than Transaction Amount - Stop

                        'Create Instance of Accounts Details Entities
                        Dim _AccountDetailsEn As New AccountsDetailsEn

                        'Set Values - Start
                        _AccountDetailsEn.PaidAmount = 0
                        _AccountDetailsEn.PostStatus = StatusReady
                        _AccountDetailsEn.TransStatus = StatusOpen
                        _AccountDetailsEn.ReferenceTwo = AllocateAuto
                        _AccountDetailsEn.ReferenceOne = StudentMatricNo
                        _AccountDetailsEn.TransactionAmount = DetailTransactionAmount
                        'Set Values - Stop

                        'if Transaction Amount Greater Than Zero - Start
                        If TransactionAmount > 0 Then
                            'Add to Accounts Details Entities List
                            ListAccountsDetails.Add(_AccountDetailsEn)
                        End If
                        'if Transaction Amount Greater Than Zero - Stop

                        'Reduce Transaction Amount
                        TransactionAmount = TransactionAmount - DetailTransactionAmount

                        'Increment Counter
                        Index = Index + 1

                    End While
                    'Loop thro the Student Invoice List - Stop

                    'Add Accounts Details List to Accounts Entities
                    _StudentEn.AccountDetailsList = ListAccountsDetails

                    'Add Student Entities to Student List
                    ListStudentEn.Add(_StudentEn)

                End If

            Next
            'Loop thro the Data Grid Item - Stop

            'Save Details
            BatchCode = _AccountsBAL.StudentBatchInsert(_AccountsEn, ListStudentEn)

            StudentList.DataSource = ListStudentEn

            Return True

        Catch ex As Exception

            Call MaxModule.Helper.LogError(ex.Message)
            err_msg = ex.Message

            Return False

        End Try

    End Function
#End Region

#Region "On Edit Student Receipt"
    'modified by Hafiz @ 11/4/2016

    Public Function OnEditStudentReceipt(ByVal StudentList As DataGrid,
        ByVal ReceiptFor As String, ByVal BatchId As String, ByVal PaymentMode As String, ByVal BatchDate As String,
        ByVal BankCode As String, ByVal ReceiptDate As String, ByVal ReferenceNo As String, ByVal TotalAllocatedAmount As Decimal,
        ByVal Description As String, ByVal ControlAmt As Decimal, ByVal DoneBy As String, ByRef err_msg As String) As Boolean

        'Create Instances - Start
        Dim _AccountsEn As New AccountsEn
        Dim _AccountsBAL As New AccountsBAL
        Dim _AccountsDAL As New AccountsDAL
        Dim _AccountsDetailsDAL As New AccountsDetailsDAL
        Dim ListStudentEn As New List(Of StudentEn)
        Dim ListAccountsDetails As New List(Of AccountsDetailsEn)
        'Create Instances - Stop

        'Variable Declarations - Start
        Dim txt_matricno As TextBox = Nothing, AmountTextBox As TextBox = Nothing, TransAmount As Double = 0
        Dim PaidAmount As Decimal = 0, TotAmount As Decimal = 0, TransactionAmount As Decimal = 0
        'Variable Declarations - Stop

        Try

            'Retrive Account Entities - Start
            _AccountsEn.SubType = ReceiptFor
            _AccountsEn.BatchCode = BatchId
            _AccountsEn.PaymentMode = PaymentMode
            _AccountsEn.BatchDate = BatchDate
            _AccountsEn.BankCode = BankCode
            _AccountsEn.TransDate = ReceiptDate
            _AccountsEn.SubReferenceOne = ReferenceNo
            _AccountsEn.TransactionAmount = TotalAllocatedAmount
            _AccountsEn.Description = Description
            _AccountsEn.UpdatedBy = DoneBy
            _AccountsEn.CreatedBy = DoneBy
            _AccountsEn.PostStatus = StatusReady
            _AccountsEn.TransStatus = StatusOpen
            _AccountsEn.Category = CategoryReceipt
            _AccountsEn.TransType = Credit
            _AccountsEn.DueDate = DateTime.Now
            _AccountsEn.ChequeDate = DateTime.Now
            _AccountsEn.CreatedDateTime = DateTime.Now
            _AccountsEn.PostedDateTime = DateTime.Now
            _AccountsEn.UpdatedTime = DateTime.Now
            _AccountsEn.ControlAmt = ControlAmt
            'Retrive Account Entities - Stop

            'Get data from DataGrid - Start
            For Each _DataGridItem In StudentList.Items

                'Get Student Selected
                txt_matricno = _DataGridItem.Cells(2).Controls(1)

                'If Student Selected - Start
                If Not txt_matricno.Text = "" Then

                    _AccountsEn.ReceiptDate = DirectCast(_DataGridItem.FindControl("txtTransDate"), TextBox).Text
                    AmountTextBox = _DataGridItem.Cells(9).Controls(1)

                    'Create Instance of Student Entity
                    Dim _StudentEn As New StudentEn

                    _StudentEn.MatricNo = txt_matricno.Text
                    _StudentEn.StudentName = _DataGridItem.Cells(3).Text
                    _StudentEn.ICNo = _DataGridItem.Cells(4).Text
                    _StudentEn.CurrentSemester = _DataGridItem.Cells(7).Text
                    TransAmount = _DataGridItem.Cells(10).Text
                    AmountTextBox.Text = String.Format("{0:F}", TransAmount)
                    _StudentEn.TransactionAmount = String.Format("{0:F}", TransAmount)
                    _StudentEn.TransDate = DateTime.Now
                    _StudentEn.UpdatedTime = DateTime.Now
                    _StudentEn.BankSlipID = DirectCast(_DataGridItem.FindControl("BankSlipID"), TextBox).Text
                    _StudentEn.ReceiptDate = DirectCast(_DataGridItem.FindControl("txtTransDate"), TextBox).Text

                    _StudentEn.Outstanding_Amount = DirectCast(_DataGridItem.FindControl("Outstanding_Amount"), Label).Text

                    'updated by Hafiz Roslan @ 2/2/2016
                    'why? check BSN first before inserting - START
                    _AccountsDAL.CheckDuplicateBankSlipNo(_StudentEn.BankSlipID, _StudentEn.MatricNo, _AccountsEn.BatchCode)
                    'why? check BSN first before inserting - END

                    Dim _AccountDetailsEn As New AccountsDetailsEn

                    _AccountDetailsEn.PaidAmount = 0
                    _AccountDetailsEn.ReferenceOne = txt_matricno.Text
                    _AccountDetailsEn.TransactionAmount = _StudentEn.TransactionAmount

                    ListAccountsDetails.Add(_AccountDetailsEn)

                    'Add Accounts Details List to Accounts Entities
                    _StudentEn.AccountDetailsList = ListAccountsDetails

                    'Add Student Entities to Student List
                    ListStudentEn.Add(_StudentEn)

                End If
                'If Student Selected - Stop

            Next
            'Loop thro the Data Grid Item - Stop

            'Update Details
            _AccountsBAL.ReceiptUpdate(_AccountsEn, ListStudentEn)

            StudentList.DataSource = ListStudentEn

            Return True

        Catch ex As Exception

            Call MaxModule.Helper.LogError(ex.Message)
            err_msg = ex.Message

            Return False

        End Try

    End Function

#End Region

#Region "Post to Workflow "

    'Purpose			: Post To Workflow
    'Author			    : Sujith Sharatchandran - T-Melmax Sdn Bhd
    'Created Date		: 04/06/2015
    'Public Function PostToWorkflow(ByVal BatchCode As String,
    '    ByVal DoneBy As String, ByVal PageName As String) As Boolean
    'Modified by Hafiz @ 16/2/2016
    Public Function PostToWorkflow(ByVal BatchCode As String,
        ByVal DoneBy As String, ByVal ReceiptFor As String, ByVal PageName As String, ByRef msg As String) As Boolean

        'Create Instances - Start
        Dim Workflow As New WorkflowDAL()
        Dim _AccountsDAL As New AccountsDAL()
        'Create Instances - Stop

        Try

            'Post To workflow for apporval - Start
            'If Not ReceiptFor = "St" Then

            'Update Accounts Details - Start
            'If _AccountsDAL.UpdatePostingStatus(BatchCode, DoneBy) Then
            '    msg = "Records Posted Successfully"
            '    Return True
            'End If
            'Update Accounts Details - Stop
            'Else

            'Add to workflow if Receipt=St - Start
            If Workflow.Workflow(BatchCode, DoneBy, PageName) Then
                msg = "Record Posted Successfully for Approval"
                Return True
            Else
                msg = "Record Already Posted"
                Return False
            End If
            'Add to workflow if Receipt=St - End
            'End If

            Return False
            'Post To workflow for apporval - Stop

        Catch ex As Exception

            Call MaxModule.Helper.LogError(ex.Message)
            Return False

        End Try

    End Function

#End Region

#Region "On Save Sponsor Receipt "

    'Purpose			: On Save Sponsor Receipt 
    'Author			    : Sujith Sharatchandran - T-Melmax Sdn Bhd
    'Created Date		: 10/06/2015
    Public Function OnSaveSponsorReceipt(ByVal SponsorCode As String,
        ByVal SubType As String, ByVal DoneBy As String,
        ByVal BatchDate As String, ByVal Description As String,
        ByVal ReceiptDate As String, ByVal ReferenceNo As String,
        ByVal PaymentMode As String, ByVal BankCode As String,
        ByVal TransactionAmount As Decimal, ByRef BatchCode As String,
        ByVal InvoiceId As String) As Boolean

        'Create Instances - Start
        Dim _SponsorEn As New SponsorEn
        Dim _AccountsEn As New AccountsEn
        Dim _AccountsBAL As New AccountsBAL
        Dim _AccountsDAL As New AccountsDAL
        Dim ListSponsorEn As New List(Of SponsorEn)
        Dim _AccountsDetailsEn As New AccountsDetailsEn
        Dim ListAccountsDetails As New List(Of AccountsDetailsEn)
        'Create Instances - Stop

        Try

            'Set Account Entities Values - Start
            _AccountsEn.SubType = SubType
            _AccountsEn.CreatedBy = DoneBy
            _AccountsEn.UpdatedBy = DoneBy
            _AccountsEn.TransType = Credit
            _AccountsEn.BankCode = BankCode
            _AccountsEn.BatchDate = BatchDate
            _AccountsEn.TransDate = ReceiptDate
            _AccountsEn.TransStatus = StatusOpen
            _AccountsEn.Description = Description
            _AccountsEn.PostStatus = StatusReady
            _AccountsEn.Category = CategoryReceipt
            _AccountsEn.SubReferenceOne = ReferenceNo
            _AccountsEn.PaymentMode = PaymentMode
            _AccountsEn.PostedDateTime = DateTime.Now
            _AccountsEn.DueDate = DateTime.Now
            _AccountsEn.UpdatedTime = DateTime.Now
            _AccountsEn.ChequeDate = DateTime.Now
            _AccountsEn.CreatedDateTime = DateTime.Now
            _AccountsEn.TransactionAmount = TransactionAmount
            'Set Account Entities Values - Stop

            'Create Instance of Accounts Details Entities
            Dim _AccountDetailsEn As New AccountsDetailsEn

            'Set Values - Start
            _AccountDetailsEn.PaidAmount = 0
            _AccountDetailsEn.PostStatus = StatusReady
            _AccountDetailsEn.TransStatus = StatusOpen
            _AccountDetailsEn.ReferenceTwo = AllocateAuto
            _AccountDetailsEn.ReferenceOne = SponsorCode
            _AccountDetailsEn.TransactionAmount = TransactionAmount
            'Set Values - Stop

            'if Transaction Amount Greater Than Zero - Start
            If TransactionAmount > 0 Then
                'Add to Accounts Details Entities List
                ListAccountsDetails.Add(_AccountDetailsEn)
            End If
            'if Transaction Amount Greater Than Zero - Stop

            'Set Account Entities Values - Start
            _SponsorEn.SponserCode = SponsorCode
            'Set Account Entities Values - Stop

            'Add to List
            Call ListSponsorEn.Add(_SponsorEn)

            'Add Accounts Details List
            _AccountsEn.AccountDetailsList = ListAccountsDetails

            'Save Details
            BatchCode = _AccountsBAL.SponsorBatchInsert(_AccountsEn, ListSponsorEn)

            'Track Receipt & Invoice
            _AccountsDAL.TrackSponsorInvoiceReceipt(InvoiceId, BatchCode)

            Return True

        Catch ex As Exception

            Call MaxModule.Helper.LogError(ex.Message)
            Return False

        End Try

    End Function

#End Region

#Region "On Edit Sponsor Receipt"

    Public Function OnEditSponsorReceipt(ByVal SubType As String, ByVal BatchId As String, ByVal PaymentMode As String,
        ByVal BatchDate As String, ByVal BankCode As String, ByVal ReceiptDate As String, ByVal ReferenceNo As String,
        ByVal TransactionAmount As Decimal, ByVal Description As String, ByVal SponsorCode As String, ByVal InvoiceId As String,
        ByVal DoneBy As String) As Boolean

        'Create Instances - Start
        Dim _SponsorEn As New SponsorEn
        Dim _AccountsEn As New AccountsEn
        Dim _AccountsBAL As New AccountsBAL
        Dim _AccountsDAL As New AccountsDAL
        Dim ListSponsorEn As New List(Of SponsorEn)
        'Create Instances - Stop

        Try

            'Set Account Entities Values - Start
            _AccountsEn.SubType = SubType
            _AccountsEn.BatchCode = BatchId
            _AccountsEn.PaymentMode = PaymentMode
            _AccountsEn.BatchDate = BatchDate
            _AccountsEn.BankCode = BankCode
            _AccountsEn.TransDate = ReceiptDate
            _AccountsEn.SubReferenceOne = ReferenceNo
            _AccountsEn.TransactionAmount = TransactionAmount
            _AccountsEn.Description = Description
            _AccountsEn.UpdatedBy = DoneBy
            'Set Account Entities Values - Stop

            'Set Account Entities Values - Start
            _SponsorEn.SponserCode = SponsorCode
            'Set Account Entities Values - Stop

            'Add to List
            Call ListSponsorEn.Add(_SponsorEn)

            'Save Details
            _AccountsBAL.ReceiptUpdate2(_AccountsEn, ListSponsorEn)

            Return True

        Catch ex As Exception

            Call MaxModule.Helper.LogError(ex.Message)
            Return False

        End Try

    End Function

#End Region

#Region "On Save Stundent Loan Receipt "

    'Purpose			: On Save Stundent Loan Receipt
    'Author			    : Sujith Sharatchandran - T-Melmax Sdn Bhd
    'Created Date		: 10/06/2015
    'Updated by Hafiz Roslan @ 4/2/2016
    'Public Function OnSaveStudentLoanReceipt(ByVal StudentMatricNo As String,
    '    ByVal SubType As String, ByVal DoneBy As String,
    '    ByVal BatchDate As String, ByVal Description As String,
    '    ByVal ReceiptDate As String, ByVal ReferenceNo As String,
    '    ByVal PaymentMode As String, ByVal BankCode As String, ByVal BatchTotal As Double,
    '    ByVal TransactionAmount As Decimal, ByRef BatchCode As String) As Boolean

    'Updated Hafiz 15/2/2016
    Public Function OnSaveStudentLoanReceipt(ByVal StudentMatricNo As String,
    ByVal SubType As String, ByVal DoneBy As String,
    ByVal BatchDate As String, ByVal Description As String,
    ByVal ReceiptDate As String, ByVal ReferenceNo As String,
    ByVal PaymentMode As String, ByVal BankCode As String, ByVal LoanAmountToPay As Double,
    ByVal TransactionAmount As Decimal, ByRef BatchCode As String) As Boolean

        'Create Instances - Start
        Dim _AccountsEn As New AccountsEn
        Dim _AccountsDAL As New AccountsDAL
        'Create Instances - Stop

        Try

            'Set Account Entities Values - Start
            _AccountsEn.SubType = SubType
            _AccountsEn.CreatedBy = DoneBy
            _AccountsEn.UpdatedBy = DoneBy
            _AccountsEn.TransType = Credit
            _AccountsEn.BankCode = BankCode
            _AccountsEn.BatchDate = BatchDate
            _AccountsEn.TransDate = ReceiptDate
            _AccountsEn.TransStatus = StatusOpen
            _AccountsEn.Description = Description
            _AccountsEn.PostStatus = StatusReady
            _AccountsEn.Category = CategoryReceipt             'change category to Loan on 4/2/2016 by Hafiz Roslan
            _AccountsEn.SubReferenceOne = ReferenceNo          'change back to receipt on 10/2/2016 
            _AccountsEn.PaymentMode = PaymentMode
            _AccountsEn.PostedDateTime = DateTime.Now
            _AccountsEn.DueDate = DateTime.Now
            _AccountsEn.UpdatedTime = DateTime.Now
            _AccountsEn.ChequeDate = DateTime.Now
            _AccountsEn.CreatedDateTime = DateTime.Now
            _AccountsEn.TransactionAmount = TransactionAmount
            _AccountsEn.CreditRef = StudentMatricNo
            '_AccountsEn.BatchTotal = BatchTotal
            'added by Hafiz Roslan @ 10/2/2016
            'add subcategory to identify the receipt for the loan - START
            _AccountsEn.SubCategory = CategoryLoan
            'add subcategory to identify the receipt for the loan - END
            'Set Account Entities Values - Stop

            'updated by Hafiz Roslan @ 10/2/2016
            'calculated the paid amount - START
            _AccountsEn.BatchTotal = LoanAmountToPay - TransactionAmount
            '_AccountsEn.PaidAmount = LoanAmountToPay - TransactionAmount
            'calculated the paid amount - END

            'Save Details
            'updated by Hafiz Roslan @ 4/2/2016
            BatchCode = _AccountsDAL.StudentLoanInsert(_AccountsEn)

            Return True

        Catch ex As Exception

            Call MaxModule.Helper.LogError(ex.Message)
            Return False

        End Try

    End Function

#End Region

#Region "On Edit Stundent Loan Receipt "

    'Purpose			: On Update Stundent Loan Receipt
    'Author			    : Hafiz Roslan
    'Created Date		: 06/01/2016
    'Updated on 4/2/2016
    'Public Function OnEditStudentLoanReceipt(ByVal SubType As String, ByVal BatchId As String,
    '    ByVal PaymentMode As String, ByVal BatchDate As String, ByVal BankCode As String, ByVal BatchTotal As Double,
    '    ByVal ReceiptDate As String, ByVal ReferenceNo As String, ByVal TransactionAmount As Decimal,
    '    ByVal Description As String, ByVal StudentMatricNo As String, ByVal DoneBy As String) As Boolean
    Public Function OnEditStudentLoanReceipt(ByVal SubType As String, ByVal BatchId As String,
        ByVal PaymentMode As String, ByVal BatchDate As String, ByVal BankCode As String, ByVal LoanAmountToPay As Double,
        ByVal ReceiptDate As String, ByVal ReferenceNo As String, ByVal TransactionAmount As Decimal,
        ByVal Description As String, ByVal StudentMatricNo As String, ByVal DoneBy As String) As Boolean

        'Create Instances - Start
        Dim _AccountsEn As New AccountsEn
        Dim _StudentEn As New StudentEn
        Dim _AccountsDAL As New AccountsDAL
        'Create Instances - Stop

        Dim BatchCode As String = Nothing

        Try

            'Set Account Entities Values - Start
            _AccountsEn.SubType = SubType
            _AccountsEn.BatchCode = BatchId
            _AccountsEn.PaymentMode = PaymentMode
            _AccountsEn.BatchDate = BatchDate
            _AccountsEn.BankCode = BankCode
            _AccountsEn.PostStatus = StatusReady
            _AccountsEn.TransDate = ReceiptDate
            _AccountsEn.SubReferenceOne = ReferenceNo
            _AccountsEn.TransactionAmount = TransactionAmount
            _AccountsEn.Description = Description
            _AccountsEn.CreditRef = StudentMatricNo
            _AccountsEn.UpdatedBy = DoneBy
            _AccountsEn.TransType = Credit
            _AccountsEn.TransStatus = StatusOpen
            _AccountsEn.Category = CategoryReceipt             'change category to Loan on 4/2/2016 by Hafiz Roslan
            _AccountsEn.DueDate = DateTime.Now                 'change back to receipt on 10/2/2016 
            _AccountsEn.UpdatedTime = DateTime.Now
            '_AccountsEn.BatchTotal = BatchTotal

            'added by Hafiz Roslan @ 10/2/2016
            'add subcategory to identify the receipt for the loan - START
            _AccountsEn.SubCategory = CategoryLoan
            'add subcategory to identify the receipt for the loan - END
            'Set Account Entities Values - Stop

            _StudentEn.MatricNo = _AccountsEn.CreditRef
            _StudentEn.TransactionAmount = _AccountsEn.TransactionAmount

            'updated by Hafiz Roslan @ 11/2/2016
            'calculated the paid amount - START
            _AccountsEn.BatchTotal = LoanAmountToPay - TransactionAmount
            '_AccountsEn.PaidAmount = LoanAmountToPay - TransactionAmount
            'calculated the paid amount - END

            'Save Details
            'Updated by Hafiz Roslan @ 4/2/2016
            BatchCode = _AccountsDAL.StudentLoanUpdate(_AccountsEn, _StudentEn)

            Return True

        Catch ex As Exception

            Call MaxModule.Helper.LogError(ex.Message)
            Return False

        End Try

    End Function

#End Region

#Region "OnDeleteGridList"

    Public Function OnDeleteGridList(ByVal matricno As String, ByVal batchid As String) As Boolean
        Dim _AccountsDal As New AccountsDAL

        Dim index As Integer = 0

        Try
            If Not _AccountsDal.CheckStudAvailableOrNot(matricno, batchid) Then
                Return False
            Else
                'delete from DB
                If Not _AccountsDal.DeleteStudAvailableForDG(matricno, batchid) Then
                Else
                    Return True
                End If
            End If

            Return True

        Catch ex As Exception
            Call MaxModule.Helper.LogError(ex.Message)
            Return False
        End Try

    End Function

#End Region

End Class
