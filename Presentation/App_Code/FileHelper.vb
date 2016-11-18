#Region "NameSpaces "

Imports System
Imports HTS.SAS
Imports System.IO
Imports MaxGeneric
Imports System.Text
Imports System.Data
Imports HTS.SAS.Entities
Imports System.Configuration
Imports Microsoft.VisualBasic
Imports HTS.SAS.BusinessObjects
Imports System.Collections.Generic
Imports System.Web
Imports HTS.SAS.DataAccessObjects
Imports System.Linq

#End Region

Public Class FileHelper
    Inherits System.Web.UI.Page

#Region "Cimb Clicks Entity "

    Private Class CimbClicksEntity

        Private _STUDENT_IC_NO As String
        Public Property IC_NO() As String
            Get
                Return _STUDENT_IC_NO
            End Get
            Set(ByVal value As String)
                _STUDENT_IC_NO = value
            End Set
        End Property

        Private _STUDENT_MATRIC_NO As String
        Public Property MATRIC_NO() As String
            Get
                Return _STUDENT_MATRIC_NO
            End Get
            Set(ByVal value As String)
                _STUDENT_MATRIC_NO = value
            End Set
        End Property

        Private _STUDENT_NAME As String
        Public Property STUDENT_NAME() As String
            Get
                Return _STUDENT_NAME
            End Get
            Set(ByVal value As String)
                _STUDENT_NAME = value
            End Set
        End Property

        Private _STUDENT_RECEIPT_NO As String
        Public Property RECEIPT_NO() As String
            Get
                Return _STUDENT_RECEIPT_NO
            End Get
            Set(ByVal value As String)
                _STUDENT_RECEIPT_NO = value
            End Set
        End Property

        Private _STUDENT_PAID_AMOUNT As Decimal
        Public Property PAID_AMOUNT() As Decimal
            Get
                Return _STUDENT_PAID_AMOUNT
            End Get
            Set(ByVal value As Decimal)
                _STUDENT_PAID_AMOUNT = value
            End Set
        End Property

        Private _BATCH_CODE As String
        Public Property BATCH_CODE() As String
            Get
                Return _BATCH_CODE
            End Get
            Set(ByVal value As String)
                _BATCH_CODE = value
            End Set
        End Property

        Private _STUDENT_RECEIPT_DATE As String
        Public Property RECEIPT_DATE() As String
            Get
                Return _STUDENT_RECEIPT_DATE
            End Get
            Set(ByVal value As String)
                _STUDENT_RECEIPT_DATE = value
            End Set
        End Property

    End Class

#End Region

#Region "Statement Matching Entity "

    Private Class StatementMatchingEntity

        Private _STATEMENT_RECEIPT_NO As String
        Public Property RECEIPT_NO() As String
            Get
                Return _STATEMENT_RECEIPT_NO
            End Get
            Set(ByVal value As String)
                _STATEMENT_RECEIPT_NO = value
            End Set
        End Property

        Private _STATEMENT_PAID_AMOUNT As Decimal
        Public Property PAID_AMOUNT() As Decimal
            Get
                Return _STATEMENT_PAID_AMOUNT
            End Get
            Set(ByVal value As Decimal)
                _STATEMENT_PAID_AMOUNT = value
            End Set
        End Property

        Private _STATEMENT_RECEIPT_DATE As String
        Public Property RECEIPT_DATE() As String
            Get
                Return _STATEMENT_RECEIPT_DATE
            End Get
            Set(ByVal value As String)
                _STATEMENT_RECEIPT_DATE = value
            End Set
        End Property

    End Class

#End Region

#Region "Statement Un Matching Entity "

    Private Class StatementUnMatchingEntity

        Private _STATEMENT_RECEIPT_NO As String
        Public Property RECEIPT_NO() As String
            Get
                Return _STATEMENT_RECEIPT_NO
            End Get
            Set(ByVal value As String)
                _STATEMENT_RECEIPT_NO = value
            End Set
        End Property

        Private _STATEMENT_PAID_AMOUNT As Decimal
        Public Property PAID_AMOUNT() As Decimal
            Get
                Return _STATEMENT_PAID_AMOUNT
            End Get
            Set(ByVal value As Decimal)
                _STATEMENT_PAID_AMOUNT = value
            End Set
        End Property

        Private _STATEMENT_RECEIPT_DATE As String
        Public Property RECEIPT_DATE() As String
            Get
                Return _STATEMENT_RECEIPT_DATE
            End Get
            Set(ByVal value As String)
                _STATEMENT_RECEIPT_DATE = value
            End Set
        End Property

    End Class

#End Region

#Region "Student Entity PTPTN"

    'Added by Hafiz @ 14/3/2016
    'Create PTPTN File related Entities

    Private Class StudentEntityPTPTN

        'IC NO
        Private _STUDENT_IC_NO As String
        Public Property ICNo() As String
            Get
                Return _STUDENT_IC_NO
            End Get
            Set(ByVal value As String)
                _STUDENT_IC_NO = value
            End Set
        End Property

        'WARRANT AMOUNT/ALLOCATED AMOUNT
        Private _STUDENT_WARRANT_AMOUNT As Decimal
        Public Property WarrantAmt() As Decimal
            Get
                Return _STUDENT_WARRANT_AMOUNT
            End Get
            Set(ByVal value As Decimal)
                _STUDENT_WARRANT_AMOUNT = value
            End Set
        End Property

        'MATRIC NO
        Private _STUDENT_MATRIC_NO As String
        Public Property MatricNO() As String
            Get
                Return _STUDENT_MATRIC_NO
            End Get
            Set(ByVal value As String)
                _STUDENT_MATRIC_NO = value
            End Set
        End Property

        'STUDENT NAME
        Private _STUDENT_NAME As String
        Public Property StudentName() As String
            Get
                Return _STUDENT_NAME
            End Get
            Set(ByVal value As String)
                _STUDENT_NAME = value
            End Set
        End Property

        'PROGRAM ID
        Private _STUDENT_PROGRAM_ID As String
        Public Property ProgramID() As String
            Get
                Return _STUDENT_PROGRAM_ID
            End Get
            Set(ByVal value As String)
                _STUDENT_PROGRAM_ID = value
            End Set
        End Property

        'CURRENT SEM
        Private _STUDENT_CURR_SEM As Integer
        Public Property CurrentSemester() As Integer
            Get
                Return _STUDENT_CURR_SEM
            End Get
            Set(ByVal value As Integer)
                _STUDENT_CURR_SEM = value
            End Set
        End Property

        'NO ACC
        Private _STUDENT_NO_ACC As String
        Public Property AccountNo() As String
            Get
                Return _STUDENT_NO_ACC
            End Get
            Set(ByVal value As String)
                _STUDENT_NO_ACC = value
            End Set
        End Property

        'STATUS CODE
        Private _STUDENT_STATS_CD As String
        Public Property StatsCode() As String
            Get
                Return _STUDENT_STATS_CD
            End Get
            Set(ByVal value As String)
                _STUDENT_STATS_CD = value
            End Set
        End Property

        'TEMP AMT
        Private _STUDENT_TEMP_AMT As Decimal
        Public Property TempAmt() As Decimal
            Get
                Return _STUDENT_TEMP_AMT
            End Get
            Set(ByVal value As Decimal)
                _STUDENT_TEMP_AMT = value
            End Set
        End Property

        'ALLOC AMT
        Private _STUDENT_ALLOC_AMT As Decimal
        Public Property AllocAmt() As Decimal
            Get
                Return _STUDENT_ALLOC_AMT
            End Get
            Set(ByVal value As Decimal)
                _STUDENT_ALLOC_AMT = value
            End Set
        End Property

    End Class

#End Region

#Region "Global Declarations "

    'Constants - Start
    Private Const StudentFeeCol As String = "Student_Fee"
    Private Const StudentNameCol As String = "Student_Name"
    Private Const StudentAccountCol As String = "Student_Account"
    Private Const StudentMatricNoCol As String = "Student_MatricNo"
    'Constants - Stop

    'Create Instances - Start
    Private _DatabaseFactory As New MaxModule.DatabaseProvider()
    Private DataBaseType As Short = DataAccessObjects.Helper.GetDataBaseType()
    Private DataBaseConnectionString As String = DataAccessObjects.Helper.GetConnectionString()
    'Create Instances - Stop

    'constant added by Hafiz @ 14/3/2016 - start
    Private Const SASI_MatricNo As String = "SASI_MatricNo"
    Private Const SASI_Name As String = "SASI_Name"
    Private Const SASI_PgId As String = "SASI_PgId"
    Private Const SASI_ICNo As String = "SASI_ICNo"
    Private Const SASI_CurSem As String = "SASI_CurSem"
    Private Const SASI_AccNO As String = "SASI_AccNO"
    Private Const SASS_Code As String = "SASS_Code"
    'constant added by Hafiz @ 14/3/2016 - end

    'added by Hafiz @ 27/3/2016
    Private Const spons_code As String = "Sponsor_Code"
    'Private Const MIN_POCKET_AMOUNT As Decimal = 100.0

    'added by Hafiz @ 21/4/2016 - use by Update Student Account process
    Private Const stud_matricno As String = "SASI_MatricNo"
    Private Const stud_bank As String = "SASI_Bank"
    Private Const stud_accno As String = "SASI_AccNO"

#End Region

#Region "Config Properties "

    Private Function GetCimbOrgCode() As String
        Return ConfigurationManager.AppSettings("CIMB_ORG_CODE")
    End Function

    Private Function GetCimbOrgName() As String
        Return ConfigurationManager.AppSettings("CIMB_ORG_NAME")
    End Function

    Private Function GetSecretKey() As String
        Return ConfigurationManager.AppSettings("CIMB_ORG_KEY")
    End Function

    Private Function GetDownloadFilePath() As String
        Return ConfigurationManager.AppSettings("DIRECT_DEBIT_FILE_DOWNLOAD_PATH")
    End Function

    Private Function GetUploadFileHeaderIdentifier() As String
        Return ConfigurationManager.AppSettings("PTPTN_UPLOAD_FILE_HEADER_IDENTIFIER")
    End Function

    Private Function GetUploadFileHeaderIdentifierLen() As Short
        Return clsGeneric.NullToShort(ConfigurationManager.AppSettings("PTPTN_UPLOAD_FILE_HEADER_IDENTIFIER_LEN"))
    End Function

    Private Function GetUploadFileFooterIdentifier() As String
        Return ConfigurationManager.AppSettings("PTPTN_UPLOAD_FILE_FOOTER_IDENTIFIER")
    End Function

    Private Function GetUploadFileFooterIdentifierLen() As Short
        Return clsGeneric.NullToShort(ConfigurationManager.AppSettings("PTPTN_UPLOAD_FILE_FOOTER_IDENTIFIER_LEN"))
    End Function

    Private Function GetUploadFileIcNoStartPosition() As Short
        Return clsGeneric.NullToShort(ConfigurationManager.AppSettings("PTPTN_UPLOAD_FILE_IC_NO_START_POS"))
    End Function

    Private Function GetUploadFileIcNoEndPosition() As Short
        Return clsGeneric.NullToShort(ConfigurationManager.AppSettings("PTPTN_UPLOAD_FILE_IC_NO_END_POS"))
    End Function

    Private Function GetUploadFileWarrantAmountStartPosition() As Short
        Return clsGeneric.NullToShort(ConfigurationManager.AppSettings("PTPTN_UPLOAD_FILE_WARRANT_AMOUNT_START_POS"))
    End Function

    Private Function GetUploadFileWarrantAmountEndPosition() As Short
        Return clsGeneric.NullToShort(ConfigurationManager.AppSettings("PTPTN_UPLOAD_FILE_WARRANT_AMOUNT_END_POS"))
    End Function

    Private ReadOnly Property StudentMinimumRetainAmount As Decimal
        Get
            Return clsGeneric.NullToDecimal(ConfigurationManager.AppSettings("STUDENT_MINIMUM_RETAIN_AMOUNT"))
        End Get
    End Property

    Private Function GetCimbClicksFileHeaderIdentifier() As String
        Return ConfigurationManager.AppSettings("CIMB_CLICKS_FILE_HEADER_IDENTIFIER")
    End Function

    Private Function GetCimbClicksFileHeaderIdentifierLen() As Short
        Return clsGeneric.NullToShort(ConfigurationManager.AppSettings("CIMB_CLICKS_FILE_HEADER_IDENTIFIER_LEN"))
    End Function

    Private Function GetCimbClicksFileFooterIdentifier() As String
        Return ConfigurationManager.AppSettings("CIMB_CLICKS_FILE_FOOTER_IDENTIFIER")
    End Function

    Private Function GetCimbClicksFileFooterIdentifierLen() As Short
        Return clsGeneric.NullToShort(ConfigurationManager.AppSettings("CIMB_CLICKS_FILE_FOOTER_IDENTIFIER_LEN"))
    End Function

    Private Function GetCimbClicksFileIcNoStartPosition() As Short
        Return clsGeneric.NullToShort(ConfigurationManager.AppSettings("CIMB_CLICKS_FILE_IC_NO_START_POS"))
    End Function

    Private Function GetCimbClicksFileIcNoEndPosition() As Short
        Return clsGeneric.NullToShort(ConfigurationManager.AppSettings("CIMB_CLICKS_FILE_IC_NO_END_POS"))
    End Function

    Private Function GetCimbClicksFilePaidAmountStartPosition() As Short
        Return clsGeneric.NullToShort(ConfigurationManager.AppSettings("CIMB_CLICKS_FILE_PAID_AMOUNT_START_POS"))
    End Function

    Private Function GetCimbClicksFilePaidAmountEndPosition() As Short
        Return clsGeneric.NullToShort(ConfigurationManager.AppSettings("CIMB_CLICKS_FILE_PAID_AMOUNT_END_POS"))
    End Function

    Private Function GetCimbClicksFileMatricNoStartPosition() As Short
        Return clsGeneric.NullToShort(ConfigurationManager.AppSettings("CIMB_CLICKS_FILE_MATRIC_NO_START_POS"))
    End Function

    Private Function GetCimbClicksFileMatricNoEndPosition() As Short
        Return clsGeneric.NullToShort(ConfigurationManager.AppSettings("CIMB_CLICKS_FILE_MATRIC_NO_END_POS"))
    End Function

    Private Function GetCimbClicksFileReceiptNoStartPosition() As Short
        Return clsGeneric.NullToShort(ConfigurationManager.AppSettings("CIMB_CLICKS_FILE_RECEIPT_NO_START_POS"))
    End Function

    Private Function GetCimbClicksFileReceiptNoEndPosition() As Short
        Return clsGeneric.NullToShort(ConfigurationManager.AppSettings("CIMB_CLICKS_FILE_RECEIPT_NO_END_POS"))
    End Function

    Private Function GetCimbClicksFileReceiptDateStartPosition() As Short
        Return clsGeneric.NullToShort(ConfigurationManager.AppSettings("CIMB_CLICKS_FILE_RECEIPT_DATE_START_POS"))
    End Function

    Private Function GetCimbClicksFileReceiptDateEndPosition() As Short
        Return clsGeneric.NullToShort(ConfigurationManager.AppSettings("CIMB_CLICKS_FILE_RECEIPT_DATE_END_POS"))
    End Function

    Private Function GetBankStatementAccountNoCol() As Short
        Return clsGeneric.NullToShort(ConfigurationManager.AppSettings("BANK_STATEMENT_ACCOUNT_NO_COL"))
    End Function

    Private Function GetBankStatementReceiptNoCol() As Short
        Return clsGeneric.NullToShort(ConfigurationManager.AppSettings("BANK_STATEMENT_RECEIPT_NO_COL"))
    End Function

    Private Function GetBankStatementReceiptDateCol() As Short
        Return clsGeneric.NullToShort(ConfigurationManager.AppSettings("BANK_STATEMENT_RECEIPT_DATE_COL"))
    End Function

    Private Function GetBankStatementPaidAmountCol() As Short
        Return clsGeneric.NullToShort(ConfigurationManager.AppSettings("BANK_STATEMENT_PAID_AMOUNT_COL"))
    End Function

    Private Function GetStudentAccountNoCol() As Short
        Return clsGeneric.NullToShort(ConfigurationManager.AppSettings("STUDENT_ACCOUNT_NO_COL"))
    End Function

    Private Function GetStudentBankCodeCol() As Short
        Return clsGeneric.NullToShort(ConfigurationManager.AppSettings("STUDENT_BANK_CODE_COL"))
    End Function

    Private Function GetStudentMatricNoCol() As Short
        Return clsGeneric.NullToShort(ConfigurationManager.AppSettings("STUDENT_MATRIC_NO_COL"))
    End Function

#End Region

#Region "Get Position Value "

    'Author         : Sujith Sharatchandran - T-Melmax Sdn Bhd
    'Purpose        : Get Position value from text file
    'Created        : 14/06/2015
    Private Function GetPositionValue(ByVal LineContent As String, _
        ByVal StartPosition As Short, ByVal EndPosition As Short) As String

        Dim LineContentLength As Integer = Len(LineContent)
        Dim MidLength As Integer = (EndPosition - StartPosition) + 1

        If LineContentLength > MidLength Then
            Return Mid(LineContent, StartPosition,
                (EndPosition - StartPosition) + 1)
        End If

        Return String.Empty

    End Function

#End Region

#Region "Generate Direct Debit File "

    'Purpose			: Generate Ptptn File 
    'Author			    : Sujith Sharatchandran - T-Melmax Sdn Bhd
    'Created Date		: 13/06/2015
    'Modified by Hafiz @ 24/5/2016

    Public Function GenerateDirectDebitFile(ByVal UploadFile As String, ByRef TotalRecords As Integer, ByRef TotalAmount As Decimal,
                                            ByRef DirectDebitFileName As String, Optional ByVal dt_result As DateTime = Nothing) As Boolean

        'Create Instances - Start
        Dim _AccountsBAL As New AccountsBAL
        Dim _StudentStatusDAL As New StudentStatusDAL
        Dim _StreamReader As StreamReader = Nothing
        Dim StudentDetails As IDataReader = Nothing
        Dim ListStudentEntity As New List(Of StudentEntityPTPTN)
        Dim ListStudentEntityFail As New List(Of StudentEntityPTPTN)
        Dim _StudentEntityPTPTN As StudentEntityPTPTN = Nothing
        'Create Instances - Stop

        'Variable Declarations - Start
        Dim WarrantAmountText As String = Nothing, IdentityNo As String = Nothing, WarrantAmount As Decimal = 0
        Dim PtptnFileName As String = Nothing, LineContent As String = Nothing
        'Variable Declarations - Stop

        Try

            'Open File in Default Format
            _StreamReader = New StreamReader(UploadFile, Encoding.Default)

            'loop thro the file contents - Start
            While _StreamReader.Peek <> -1

                'get Line Contents
                LineContent = _StreamReader.ReadLine()

                'Check Line Type - Start
                If Left(LineContent, GetUploadFileHeaderIdentifierLen) = GetUploadFileHeaderIdentifier() Then 'Header Line
                    'Do Nothing
                ElseIf Left(LineContent, GetUploadFileFooterIdentifierLen) = GetUploadFileFooterIdentifier() Then ' Footer Line
                    'Do nothing
                Else

                    'Get Identity No - Start
                    IdentityNo = GetPositionValue(LineContent, GetUploadFileIcNoStartPosition, GetUploadFileIcNoEndPosition)
                    'Get Identity No - Stop

                    'Get Warrant Amount - Start
                    WarrantAmountText = GetPositionValue(LineContent, GetUploadFileWarrantAmountStartPosition, GetUploadFileWarrantAmountEndPosition)
                    WarrantAmount = clsGeneric.NullToInteger(WarrantAmountText) / 100
                    WarrantAmount = Math.Round(WarrantAmount, 2)
                    'Get Warrant Amount - Stop

                    StudentDetails = GetStudentDetailsFromPTPTNFile(IdentityNo)

                    While StudentDetails.Read()

                        _StudentEntityPTPTN = New StudentEntityPTPTN()

                        _StudentEntityPTPTN.ICNo = IdentityNo
                        _StudentEntityPTPTN.WarrantAmt = WarrantAmount
                        _StudentEntityPTPTN.MatricNO = clsGeneric.NullToString(StudentDetails(SASI_MatricNo))
                        _StudentEntityPTPTN.StudentName = clsGeneric.NullToString(StudentDetails(SASI_Name))
                        _StudentEntityPTPTN.ProgramID = clsGeneric.NullToString(StudentDetails(SASI_PgId))
                        _StudentEntityPTPTN.CurrentSemester = clsGeneric.NullToInteger(StudentDetails(SASI_CurSem))
                        _StudentEntityPTPTN.AccountNo = clsGeneric.NullToString(StudentDetails(SASI_AccNO))
                        _StudentEntityPTPTN.StatsCode = clsGeneric.NullToString(StudentDetails(SASS_Code))

                        Dim FndStats As StudentStatusEn = _StudentStatusDAL.GetStudentBlStatus(_StudentEntityPTPTN.StatsCode)

                        If FndStats.BlStatus = True Then
                            'calculate the alloc. amt - start
                            Dim stud_obj As New StudentEn
                            Dim amt As Double = 0, tamt As Double = 0, outamt As Double = 0, allocamt As Double = 0

                            amt = _StudentEntityPTPTN.WarrantAmt  'warran amount
                            tamt = _StudentEntityPTPTN.TempAmt

                            stud_obj.MatricNo = _StudentEntityPTPTN.MatricNO
                            outamt = _AccountsBAL.GetStudentOutstandingAmtInSponsorAllocation(stud_obj)

                            If outamt <= 0 Then

                                tamt = amt
                                allocamt = 0

                            Else

                                If amt > 100 Then
                                    If outamt > 100 Then
                                        'tamt = MIN_POCKET_AMOUNT
                                        tamt = GetMinBalance()
                                        allocamt = amt - tamt

                                        If allocamt >= outamt Then
                                            allocamt = outamt
                                            tamt = amt - allocamt
                                        End If

                                    Else
                                        tamt = amt - outamt
                                        allocamt = amt - tamt

                                        If allocamt >= outamt Then
                                            allocamt = outamt
                                            tamt = amt - allocamt
                                        End If

                                    End If
                                Else
                                    tamt = amt
                                    allocamt = 0
                                End If

                            End If

                            'Add to Student Entity List - Succeed list
                            _StudentEntityPTPTN.AllocAmt = Format(allocamt, "0.00") 'allocated/allocamt
                            ListStudentEntity.Add(_StudentEntityPTPTN)

                            'calculate the alloc. amt - end
                        Else
                            'Add to Student Entity List - Failed list
                            _StudentEntityPTPTN.AllocAmt = 0.0
                            ListStudentEntityFail.Add(_StudentEntityPTPTN)

                        End If

                    End While

                End If
                'Check Line Type - Stop

            End While
            'loop thro the file contents - Stop

            'if student list available - Start
            If Not ListStudentEntity Is Nothing Then

                If CreateDirectDebitFile(UploadFile, ListStudentEntity, TotalRecords, TotalAmount, DirectDebitFileName, dt_result) Then

                    'Create fail`s DD File
                    If Not ListStudentEntityFail Is Nothing Then
                        Dim FailTotalRecords As Integer = 0, ContentDirectDebitFileFail As String = Nothing

                        ContentDirectDebitFileFail = MandateGenerateToTextFile(ListStudentEntityFail, FailTotalRecords)
                        Session("DirectDebitFileFail") = ContentDirectDebitFileFail
                        Session("FailTotalRecords") = FailTotalRecords
                    End If

                    Session("lststudent") = ListStudentEntity
                    Return True
                End If

            End If
            'if student list available - Stop

            Return False

        Catch ex As Exception

            'Log Error 
            Call MaxModule.Helper.LogError(ex.Message)
            Return False

        Finally

            ListStudentEntity = Nothing
            ListStudentEntityFail = Nothing

            'Destroy Stream reader instance - Start
            If Not _StreamReader Is Nothing Then
                _StreamReader.Close()
                _StreamReader = Nothing
            End If
            'Destroy Stream reader instance - Stop

        End Try

    End Function

#End Region

#Region "Get Student Details "

    'Purpose			: Get Student Details 
    'Author			    : Sujith Sharatchandran - T-Melmax Sdn Bhd
    'Created Date		: 13/06/2015
    Private Function GetStudentDetails(ByVal IdentityNo As String) As IDataReader

        'Variable Declarations
        Dim SqlStatement As String = Nothing

        Try

            'Build Sql Statement Start
            SqlStatement = "SELECT SAS_Student.SASI_MatricNo AS Student_MatricNo,"
            SqlStatement &= "SAS_Student.SASI_Name AS Student_Name,"
            SqlStatement &= "SAS_Student.SASI_AccNo AS Student_Account,"
            SqlStatement &= "SAS_PTPTNFeeSetup.ProgFee AS Student_Fee "
            SqlStatement &= "FROM SAS_Student INNER JOIN SAS_PTPTNFeeSetup ON "
            SqlStatement &= "SAS_Student.SASI_PgId = SAS_PTPTNFeeSetup.ProgCode "
            SqlStatement &= " WHERE SAS_Student.SASI_ICNo = " & clsGeneric.AddQuotes(IdentityNo)
            'Build Sql Statement Stop

            'Return Results as Datatable - Start
            Return _DatabaseFactory.ExecuteDataTable(DataBaseType,
                DataBaseConnectionString, SqlStatement).CreateDataReader()
            'Return Results as Datatable - Stop

        Catch ex As Exception

            'Log Error 
            Call MaxModule.Helper.LogError(ex.Message)
            Return Nothing

        End Try

    End Function

#End Region

#Region "Create Direct Debit File "

    'Purpose			: Create Bank Direct Debit File
    'Author			    : Sujith Sharatchandran - T-Melmax Sdn Bhd
    'Created Date		: 12/06/2015
    'Modified by Hafiz @ 24/5/2016

    Private Function CreateDirectDebitFile(ByVal UploadFileName As String, ByVal ListStudentEntity As List(Of StudentEntityPTPTN),
        ByRef TotalRecords As Integer, ByRef TotalAmount As Decimal, ByRef DirectDebitFileName As String,
        Optional ByVal dt_result As DateTime = Nothing) As Boolean

        'Create Instances
        Dim _StringBuilder As New StringBuilder

        'Variable Declarations - Start
        Dim HeaderContent As String = Nothing, DetailContent As String = Nothing
        Dim HashTotal As String = Nothing, BatchNumber As String = Nothing, FooterContent As String = Nothing
        'Variable Declarations - Stop

        Try
            'Create File Header Content
            HeaderContent = DirectDebitFileHeader(BatchNumber, dt_result)

            'Create Details Content - Start
            DetailContent = DirectDebitFileDetails(BatchNumber, ListStudentEntity, TotalAmount, TotalRecords, HashTotal)
            'Create Details Content - Stop

            'Create Footer Content - Start
            FooterContent = DirectDebitFileFooter(BatchNumber, TotalRecords, TotalAmount, HashTotal)
            'Create Footer Content - Stop

            'Build Direct Debit File Name - Start
            DirectDebitFileName = Path.GetFileNameWithoutExtension(UploadFileName)
            DirectDebitFileName &= "_" & Format(CDate(Now), "yyyyMMdd") & "_PTPTN.txt"
            DirectDebitFileName = GetDownloadFilePath() & DirectDebitFileName
            'Build Direct Debit File Name - Start

            'Concate Header/Details/Footer Contents - Start
            _StringBuilder.AppendLine(HeaderContent)
            _StringBuilder.Append(DetailContent)
            _StringBuilder.Append(FooterContent)
            'Concate Header/Details/Footer Contents - Stop

            'Create File - Start
            If TotalRecords > 0 Then
                Call clsGeneric.CreateFile(DirectDebitFileName, False, _StringBuilder.ToString())
            End If
            'Create File - Stop

            If File.Exists(DirectDebitFileName) Then
                Return True
            End If

            Return False

        Catch ex As Exception

            'Log Error 
            Call MaxModule.Helper.LogError(ex.Message)
            Return False

        End Try

    End Function

#End Region

#Region "Create Direct Debit File Details "

    'Purpose			: Direct Debit File Details
    'Author			    : Sujith Sharatchandran - T-Melmax Sdn Bhd
    'Created Date		: 12/06/2015
    'modified by Hafiz @ 24/5/2016

    Private Function DirectDebitFileDetails(ByVal BatchNumber As String, ByVal ListStudentEntity As List(Of StudentEntityPTPTN),
        ByRef TotalAmount As Decimal, ByRef TotalRecords As Integer, ByRef HashTotal As String) As String

        'Create Instances - Start
        Dim _StringBuilder As New StringBuilder
        Dim StudentDetails As IDataReader = Nothing
        'Create Instances - Stop

        'Variable Declarations - Start
        Dim StudentDebitAmount As Decimal = 0, Index As Integer = 0, LineContent As String = Nothing
        Dim StudentAccountNo As String = Nothing, StudentFeeAmount As Decimal = 0, StudentName As String = Nothing
        Dim StudentIdentityNo As String = Nothing, StudentWarrantAmount As Decimal = 0, StudentMatricNo As String = Nothing
        'Variable Declarations - Stop

        Try

            'Loop thro the Student Entity List - Start
            While Index < ListStudentEntity.Count

                StudentIdentityNo = clsGeneric.NullToString(ListStudentEntity(Index).ICNo)
                StudentName = clsGeneric.NullToString(ListStudentEntity(Index).StudentName)
                StudentMatricNo = clsGeneric.NullToString(ListStudentEntity(Index).MatricNO)
                StudentAccountNo = clsGeneric.NullToString(ListStudentEntity(Index).AccountNo)

                'Increment total records
                TotalRecords = TotalRecords + 1

                'Add Total Amount
                TotalAmount = TotalAmount + clsGeneric.NullToString(ListStudentEntity(Index).AllocAmt)

                'Build Hash Total - Start
                HashTotal = clsGeneric.NullToLong(HashTotal) + clsGeneric.NullToLong(StudentAccountNo) +
                    clsGeneric.NullToLong(clsGeneric.RemoveCurrencyFormat(ListStudentEntity(Index).AllocAmt))
                'Build Hash Total - Stop

                'Build Line Content - Start
                LineContent = "02"
                LineContent &= BatchNumber
                LineContent &= clsGeneric.StringFiller(StudentMatricNo, 30, EnumHelp.FillerType.Suffix, " ")
                LineContent &= clsGeneric.StringFiller(StudentIdentityNo, 16, EnumHelp.FillerType.Suffix, " ")
                LineContent &= clsGeneric.StringFiller(StudentName, 40, EnumHelp.FillerType.Suffix, " ")
                LineContent &= clsGeneric.StringFiller(StudentAccountNo, 14, EnumHelp.FillerType.Suffix, " ")
                LineContent &= clsGeneric.StringFiller(clsGeneric.RemoveCurrencyFormat(ListStudentEntity(Index).AllocAmt), 13,
                                                        EnumHelp.FillerType.Prefix, "0")
                LineContent &= clsGeneric.StringFiller(String.Empty, 77, EnumHelp.FillerType.Suffix, " ")
                'Build Line Content - Stop

                'Append to String Builder
                _StringBuilder.AppendLine(LineContent)

                'Inrcement Index
                Index = Index + 1

            End While
            'Loop thro the Student Entity List - Stop

            Return _StringBuilder.ToString()

        Catch ex As Exception

            'Log Error 
            Call MaxModule.Helper.LogError(ex.Message)
            Return String.Empty

        End Try

    End Function

#End Region

#Region "Create Direct Debit File Header "

    'Purpose			: Create Direct Debit File Header
    'Author			    : Sujith Sharatchandran - T-Melmax Sdn Bhd
    'Created Date		: 12/06/2015
    Private Function DirectDebitFileHeader(ByRef BatchNumber As String, Optional ByVal dt_result As DateTime = Nothing) As String

        'Variable Declarations
        Dim HeaderDetails As String = Nothing, SecretKey As Integer = 0, FileCounter As Short = 0

        Try

            'Get File Counter
            FileCounter = GetDirectDebitFileCount()

            'Build Batch Number - Start
            BatchNumber = Format(DateTime.Now.Day, "00") & Format(DateTime.Now.Month, "00")
            BatchNumber &= Right(DateTime.Now.Year, 2) & Format(FileCounter, "00")
            'Build Batch Number - Stop

            'Build Secret Key - Start
            SecretKey = clsGeneric.NullToInteger(BatchNumber) + clsGeneric.NullToInteger(GetSecretKey)
            'Build Secret Key - Stop

            Dim str_header As String = Nothing
            If dt_result <> Nothing Then
                str_header = Format(dt_result.Day, "00") & Format(dt_result.Month, "00")
                str_header &= dt_result.Year
            Else
                str_header &= Format(DateTime.Now.Day, "00") & Format(DateTime.Now.Month, "00")
                str_header &= DateTime.Now.Year
            End If

            'Build Header Record details - Start
            HeaderDetails = "01"
            HeaderDetails &= BatchNumber
            HeaderDetails &= GetCimbOrgCode()
            HeaderDetails &= clsGeneric.StringFiller(GetCimbOrgName(), 40, EnumHelp.FillerType.Suffix, " ")
            HeaderDetails &= str_header
            'HeaderDetails &= Format(DateTime.Now.Day, "00") & Format(DateTime.Now.Month, "00")
            'HeaderDetails &= DateTime.Now.Year
            HeaderDetails &= clsGeneric.StringFiller(SecretKey, 10, EnumHelp.FillerType.Prefix, "0")
            HeaderDetails &= clsGeneric.StringFiller("", 56, EnumHelp.FillerType.Suffix, " ")
            'Build Header Record details - Stop

            Return HeaderDetails

        Catch ex As Exception

            'Log Error 
            Call MaxModule.Helper.LogError(ex.Message)
            Return String.Empty

        End Try

    End Function

#End Region

#Region "Create Direct Debit File Footer "

    'Purpose			: Create Ptptn File Footer
    'Author			    : Sujith Sharatchandran - T-Melmax Sdn Bhd
    'Created Date		: 12/06/2015
    Private Function DirectDebitFileFooter(ByVal BatchNumber As String,
    ByVal TotalRecords As Integer, ByVal TotalAmount As Decimal,
    ByVal HashTotal As String) As String

        'Variable Declarations - Start
        Dim FooterDetails As String = Nothing
        'Variable Declarations - Stop

        Try

            'Build Header Record details - Start
            FooterDetails = "03"
            FooterDetails &= BatchNumber
            FooterDetails &= clsGeneric.StringFiller(TotalRecords, 6, EnumHelp.FillerType.Prefix, "0")
            FooterDetails &= clsGeneric.StringFiller(clsGeneric.RemoveCurrencyFormat(TotalAmount), 15, EnumHelp.FillerType.Prefix, "0")
            FooterDetails &= clsGeneric.StringFiller("", 42, EnumHelp.FillerType.Suffix, " ")
            FooterDetails &= clsGeneric.StringFiller(Right(HashTotal, 15), 15, EnumHelp.FillerType.Prefix, "0")
            FooterDetails &= clsGeneric.StringFiller("", 10, EnumHelp.FillerType.Suffix, " ")
            'Build Header Record details - Stop

            Return FooterDetails

        Catch ex As Exception

            'Log Error 
            Call MaxModule.Helper.LogError(ex.Message)
            Return String.Empty

        End Try

    End Function

#End Region

#Region "Track Ptptn File Details "

    'Purpose			: Track Ptptn File Details
    'Author			    : Sujith Sharatchandran - T-Melmax Sdn Bhd
    'Created Date		: 16/06/2015
    Public Sub TrackPtptnFileDetails(ByVal UploadFileName As String,
        ByVal DirectDebitFileName As String, ByVal TotalAmount As Decimal,
        ByVal TotalRecords As Integer, Optional ByVal dt_result As DateTime = Nothing)

        'Variable Declarations
        Dim SqlStatement As String = Nothing

        Try

            'if transactions available - Start
            If TotalRecords > 0 Then

                Dim dt As DateTime = IIf(dt_result <> Nothing, dt_result, Now)

                'Build Sql Statement - Start
                SqlStatement = "INSERT INTO sas_ptptn_filedetails(File_In_Name"
                SqlStatement &= ",File_Out_Name,File_Amount,File_Transactions,Date_Time,Debiting_Date)"
                SqlStatement &= " VALUES(" & clsGeneric.AddQuotes(Path.GetFileName(UploadFileName))
                SqlStatement &= clsGeneric.AddComma() & clsGeneric.AddQuotes(Path.GetFileName(DirectDebitFileName))
                SqlStatement &= clsGeneric.AddComma() & clsGeneric.AddQuotes(TotalAmount)
                SqlStatement &= clsGeneric.AddComma() & TotalRecords
                SqlStatement &= clsGeneric.AddComma() & clsGeneric.AddQuotes(DataAccessObjects.Helper.DateConversion(Now))
                SqlStatement &= clsGeneric.AddComma() & clsGeneric.AddQuotes(DataAccessObjects.Helper.DateConversion(dt))
                SqlStatement &= ")"
                'Build Sql Statement - Stop

                'Execute Sql Statement - Start
                _DatabaseFactory.ExecuteSqlStatement(DataBaseType, DataBaseConnectionString, SqlStatement)
                'Execute Sql Statement - Stop

            End If
            'if transactions available - Stop

        Catch ex As Exception

            'Log Error 
            Call MaxModule.Helper.LogError(ex.Message)

        End Try

    End Sub

#End Region

#Region "Direct Debit File Count "

    'Purpose			: To Direct Debit File Count for the same day to get the next running number for Batch Number
    'Author			    : Sujith Sharatchandran - T-Melmax Sdn Bhd
    'Created Date		: 16/06/2015
    Public Function GetDirectDebitFileCount() As Short

        'Variable Declarations
        Dim SqlStatement As String = Nothing, FileCounter As Short = 0

        Try

            'Build Sql Statement - Start
            SqlStatement = "SELECT COUNT(*) AS Total_Files FROM sas_ptptn_filedetails WHERE "
            SqlStatement &= "date_part('day',DATE(Date_Time)) = date_part('day',DATE(NOW())) AND "
            SqlStatement &= "date_part('month',DATE(Date_Time)) = date_part('month',DATE(NOW())) AND "
            SqlStatement &= "date_part('year',DATE(Date_Time)) = date_part('year',DATE(NOW()));"
            'Build Sql Statement - Stop

            'get file counter - Start
            FileCounter = clsGeneric.NullToShort(_DatabaseFactory.ExecuteScalar(DataBaseType,
                DataBaseConnectionString, SqlStatement))
            'get file counter - Stop

            Return FileCounter + 1

        Catch ex As Exception

            'Log Error 
            Call MaxModule.Helper.LogError(ex.Message)

            Return 0

        End Try

    End Function

#End Region

#Region "Check if Ptptn File Uploaded "

    'Purpose			: To Check if Ptptn File Uploaded
    'Author			    : Sujith Sharatchandran - T-Melmax Sdn Bhd
    'Created Date		: 16/06/2015
    Public Function IsPtptnFileUploaded(ByVal UploadedPtptnFile As String) As Boolean

        'Variable Declarations
        Dim SqlStatement As String = Nothing, FileCounter As Short = 0

        Try

            'Build Sql Statement - Start
            SqlStatement = "SELECT COUNT(*) AS Total_Files FROM sas_ptptn_filedetails WHERE "
            SqlStatement &= " File_In_Name = " & clsGeneric.AddQuotes(Path.GetFileName(UploadedPtptnFile))
            'Build Sql Statement - Stop

            'get file counter - Start
            FileCounter = clsGeneric.NullToShort(_DatabaseFactory.ExecuteScalar(DataBaseType,
                DataBaseConnectionString, SqlStatement))
            'get file counter - Stop

            If FileCounter > 0 Then
                Return True
            End If

            Return False

        Catch ex As Exception

            'Log Error 
            Call MaxModule.Helper.LogError(ex.Message)

            Return 0

        End Try

    End Function

#End Region

#Region "Upload Cimb Clicks File "

    'Purpose			: To Upload Cimb Clicks File
    'Author			    : Sujith Sharatchandran - T-Melmax Sdn Bhd
    'Created Date		: 16/06/2015
    Public Function UploadCimbClicksFile(ByVal CimbClicksFile As String,
        ByRef dgClicksTransactions As DataGrid, ByRef TotalAmount As Decimal,
        ByRef TotalRecords As Integer, ByVal BankCode As String) As Boolean

        Try

            'if cimb clicks file uploaded successfully - Start
            If ReadCimbClicksFile(CimbClicksFile,
                dgClicksTransactions, TotalAmount, TotalRecords) Then
                Return True
            End If
            'if cimb clicks file uploaded successfully - Stop

            Return False

        Catch ex As Exception

            'Log Error 
            Call MaxModule.Helper.LogError(ex.Message)
            Return False

        End Try

    End Function

#End Region

#Region "Read Cimb Clicks File "

    'Purpose			: To Read Cimb Clicks File
    'Author			    : Sujith Sharatchandran - T-Melmax Sdn Bhd
    'Created Date		: 16/06/2015
    Private Function ReadCimbClicksFile(ByVal CimbClicksFile As String,
        ByRef dgClicksTransactions As DataGrid, ByRef TotalAmount As Decimal,
        ByRef TotalRecords As Integer) As Boolean

        'Create Instances - Start
        Dim _StreamReader As StreamReader = Nothing
        Dim _CimbClicksEntity As CimbClicksEntity = Nothing
        Dim ListCimbClicksEntity As List(Of CimbClicksEntity)
        'Create Instances - Stop

        'Variable Declarations - Start
        Dim StudentName As String = Nothing, ReceiptDate As String = Nothing
        Dim PaidAmount As Decimal = 0, PaidAmountText As String = Nothing, ReceiptNo As String = Nothing
        Dim LineContent As String = Nothing, IdentityNo As String = Nothing, MatricNo As String = Nothing
        'Variable Declarations - Stop

        Try

            'Set Values
            TotalAmount = 0

            'Initialize
            ListCimbClicksEntity = New List(Of CimbClicksEntity)

            'Open File in Default Format
            _StreamReader = New StreamReader(CimbClicksFile, Encoding.Default)

            'loop thro the file contents - Start
            While _StreamReader.Peek <> -1

                'get Line Contents
                LineContent = _StreamReader.ReadLine()

                'Check Line Type - Start
                If Left(LineContent, GetCimbClicksFileHeaderIdentifierLen) =
                    GetCimbClicksFileHeaderIdentifier() Then 'Header Line
                    'Do Nothing
                ElseIf Left(LineContent, GetCimbClicksFileFooterIdentifierLen) =
                    GetCimbClicksFileFooterIdentifier() Then ' Footer Line
                    'Do nothing
                Else

                    'Get Identity No - Start
                    IdentityNo = GetPositionValue(LineContent,
                        GetCimbClicksFileIcNoStartPosition,
                        GetCimbClicksFileIcNoEndPosition)
                    IdentityNo = clsGeneric.NullToString(IdentityNo)
                    'Get Identity No - Stop

                    'Change by Hafiz Roslan on 07/01/2016
                    'Commented Math.Round to enable decimal point (SetCurrencyFormat)
                    'Get Paid Amount - Start
                    PaidAmountText = GetPositionValue(LineContent,
                        GetCimbClicksFilePaidAmountStartPosition,
                        GetCimbClicksFilePaidAmountEndPosition)
                    PaidAmount = clsGeneric.NullToInteger(PaidAmountText) / 100
                    'PaidAmount = Math.Round(PaidAmount, 2)
                    PaidAmount = clsGeneric.SetCurrencyFormat(PaidAmount)
                    'Get Paid Amount - Stop

                    'Get Matric No - Start
                    MatricNo = GetPositionValue(LineContent,
                        GetCimbClicksFileMatricNoStartPosition,
                        GetCimbClicksFileMatricNoEndPosition)
                    MatricNo = clsGeneric.NullToString(MatricNo)
                    'Get Matric No - Stop

                    'Get Receipt No - Start
                    ReceiptNo = GetPositionValue(LineContent,
                        GetCimbClicksFileReceiptNoStartPosition,
                        GetCimbClicksFileReceiptNoEndPosition)

                    ReceiptNo = clsGeneric.NullToString(ReceiptNo)
                    'Get Receipt No - Stop

                    'Get Receipt Date - Start
                    ReceiptDate = GetPositionValue(LineContent,
                        GetCimbClicksFileReceiptDateStartPosition,
                        GetCimbClicksFileReceiptDateEndPosition)
                    'Get Receipt Date - Stop

                    'Set Date Format - Start
                    ReceiptDate = Left(ReceiptDate, 2) & "/" &
                        ReceiptDate.Substring(2, 2) & "/" & Right(ReceiptDate, 4)
                    'Set Date Format - Stop

                    'Get Student Name
                    StudentName = GetStudentName(MatricNo)

                    'Set Entity Values - Start
                    _CimbClicksEntity = New CimbClicksEntity()
                    _CimbClicksEntity.IC_NO = IdentityNo
                    _CimbClicksEntity.MATRIC_NO = MatricNo
                    _CimbClicksEntity.RECEIPT_NO = ReceiptNo
                    _CimbClicksEntity.PAID_AMOUNT = PaidAmount
                    _CimbClicksEntity.STUDENT_NAME = StudentName
                    _CimbClicksEntity.RECEIPT_DATE = ReceiptDate
                    'Set Entity Values - Stop

                    'Check Receipt No - Start
                    'Variable Declarations
                    Dim SqlStatement As String = Nothing, ReceiptCounter As Short = 0

                    Try

                        'Build Sql Statement - Start
                        SqlStatement = "SELECT COUNT(*) AS Total_Files FROM sas_clicks_receiptdetails WHERE "
                        SqlStatement &= " receipt_no = " & clsGeneric.AddQuotes(ReceiptNo)
                        'Build Sql Statement - Stop

                        'get file counter - Start
                        ReceiptCounter = clsGeneric.NullToShort(_DatabaseFactory.ExecuteScalar(DataBaseType,
                            DataBaseConnectionString, SqlStatement))
                        'get file counter - Stop

                        'comment by Hafiz @ 06/6/2016
                        'Allow duplicate Receipt No
                        'If ReceiptCounter > 0 Then
                        '    Return False
                        'End If

                    Catch ex As Exception

                        'Log Error 
                        Call MaxModule.Helper.LogError(ex.Message)

                        Return False

                    End Try
                    'Check Receipt No - Stop

                    'Add Total Amount
                    TotalAmount = TotalAmount + PaidAmount

                    'Add to List 
                    ListCimbClicksEntity.Add(_CimbClicksEntity)

                End If
                'Check Line Type - Stop

            End While
            'loop thro the file contents - Stop

            'if clicks trans list not null - Start
            If Not ListCimbClicksEntity Is Nothing Then

                'if clicks transaction available - Start
                If ListCimbClicksEntity.Count > 0 Then

                    'Set Values
                    TotalRecords = ListCimbClicksEntity.Count

                    'Populate Data Grid - Start
                    dgClicksTransactions.DataSource = ListCimbClicksEntity
                    dgClicksTransactions.DataBind()
                    'Populate Data Grid - Stop
                End If
                'if clicks transaction available - Stop

            End If
            'if clicks trans list not null - Stop

            Return True

        Catch ex As Exception

            'Log Error 
            Call MaxModule.Helper.LogError(ex.Message)
            Return False

        Finally

            'Destroy Stream reader instance - Start
            If Not _StreamReader Is Nothing Then
                _StreamReader.Close()
                _StreamReader = Nothing
            End If
            'Destroy Stream reader instance - Stop

        End Try

    End Function

#End Region

#Region "Check if Clicks File Uploaded "

    'Purpose			: To Check if Clicks File Uploaded
    'Author			    : Sujith Sharatchandran - T-Melmax Sdn Bhd
    'Created Date		: 16/06/2015
    Public Function IsClicksFileUploaded(ByVal UploadedClicksFile As String, ByRef HeaderNo As String) As Boolean

        'Variable Declarations
        Dim SqlStatement As String = Nothing, FileCounter As Short = 0, iRecCount As Integer = 0
        Dim _StreamReader As StreamReader = Nothing, LineContent As String = Nothing

        'Open File in Default Format
        _StreamReader = New StreamReader(UploadedClicksFile, Encoding.Default)

        iRecCount = 0
        Try
            'loop thro the file contents - Start (Read only first line to find header)
            Do While _StreamReader.Peek >= 0

                iRecCount += 1

                'get Line Contents
                LineContent = _StreamReader.ReadLine()

                If iRecCount = 1 Then
                    If Left(LineContent, GetCimbClicksFileHeaderIdentifierLen) =
                    GetCimbClicksFileHeaderIdentifier() Then 'Header Line
                        HeaderNo = Trim(LineContent)
                    End If

                    Try
                        'Build Sql Statement - Start
                        SqlStatement = "SELECT COUNT(*) AS Total_Files FROM sas_clicks_filedetails WHERE "
                        SqlStatement &= " header_no = " & clsGeneric.AddQuotes(HeaderNo)
                        'Build Sql Statement - Stop

                        'get file counter - Start
                        FileCounter = clsGeneric.NullToShort(_DatabaseFactory.ExecuteScalar(DataBaseType,
                            DataBaseConnectionString, SqlStatement))
                        'get file counter - Stop

                        If FileCounter > 0 Then
                            Return True
                        End If

                    Catch ex As Exception
                        Throw ex
                        Return False
                    End Try

                Else
                    Exit Do
                End If

            Loop
            'loop thro the file contents - Stop

            Return False

        Catch ex As Exception

            'Log Error 
            Call MaxModule.Helper.LogError(ex.Message)
            Return False

        Finally
            _StreamReader.Close()

        End Try

    End Function

#End Region

#Region "Get Student Name "

    'Purpose			: To Check if Ptptn File Uploaded
    'Author			    : Sujith Sharatchandran - T-Melmax Sdn Bhd
    'Created Date		: 16/06/2015
    Public Function GetStudentName(ByVal MatricNo As String) As String

        'Create Instances
        Dim _IDataReader As IDataReader

        'Variable Declarations
        Dim SqlStatement As String = Nothing, StudentName As String = String.Empty

        Try

            'Build Sql Statement - Start
            SqlStatement = "SELECT sasi_name AS Student_Name FROM sas_student WHERE "
            SqlStatement &= "sasi_matricno = " & clsGeneric.AddQuotes(MatricNo)
            'Build Sql Statement - Stop

            'Execute Reader - Start
            _IDataReader = _DatabaseFactory.ExecuteReader(DataBaseType,
                DataBaseConnectionString, SqlStatement).CreateDataReader()
            'Execute Reader - Stop

            'if record available - Start
            If _IDataReader.Read() Then
                StudentName = clsGeneric.NullToString(_IDataReader("Student_Name"))
            End If
            'if record available - Stop

            'Close Data Reader
            _IDataReader.Close()

            Return StudentName

        Catch ex As Exception

            'Log Error 
            Call MaxModule.Helper.LogError(ex.Message)

            Return String.Empty

        End Try

    End Function

#End Region

#Region "Track Clicks File Details "

    'Purpose			: Track Clicks File Details
    'Author			    : Sujith Sharatchandran - T-Melmax Sdn Bhd
    'Created Date		: 16/06/2015
    Public Sub TrackClicksFileDetails(ByVal UploadFileName As String,
        ByVal TotalAmount As Decimal, ByVal TotalRecords As Integer,
        ByVal BankCode As String, ByVal HeaderNo As String)

        'Variable Declarations
        Dim SqlStatement As String = Nothing

        Try

            'if transactions available - Start
            If TotalRecords > 0 Then

                'Build Sql Statement - Start
                SqlStatement = "INSERT INTO sas_clicks_filedetails(File_In_Name"
                SqlStatement &= ",File_Amount,File_Transactions,Date_Time,Bank_Code,Header_No)"
                SqlStatement &= " VALUES(" & clsGeneric.AddQuotes(Path.GetFileName(UploadFileName))
                SqlStatement &= clsGeneric.AddComma() & clsGeneric.AddQuotes(TotalAmount)
                SqlStatement &= clsGeneric.AddComma() & TotalRecords
                SqlStatement &= clsGeneric.AddComma() & clsGeneric.AddQuotes(DataAccessObjects.Helper.DateConversion(Now))
                SqlStatement &= clsGeneric.AddComma() & clsGeneric.AddQuotes(BankCode)
                SqlStatement &= clsGeneric.AddComma() & clsGeneric.AddQuotes(HeaderNo)
                SqlStatement &= ")"
                'Build Sql Statement - Stop

                'Execute Sql Statement - Start
                _DatabaseFactory.ExecuteSqlStatement(DataBaseType,
                    DataBaseConnectionString, SqlStatement)
                'Execute Sql Statement - Stop

            End If
            'if transactions available - Stop

        Catch ex As Exception

            'Log Error 
            Call MaxModule.Helper.LogError(ex.Message)

        End Try

    End Sub

#End Region

#Region "Insert Clicks Trans to Accounts "

    'Purpose			: Insert Clicks Trans to Accounts
    'Author			    : Sujith Sharatchandran - T-Melmax Sdn Bhd
    'Created Date		: 19/06/2015
    'modified by Hafiz @ 29/6/2016 - CIMB Clicks BatchCode`s related
    'modified by Hafiz @ 25/7/2016 - GetHeaderDate()

    Public Function InsertClicksTransToAccounts(ByRef dgClicksTransactions As DataGrid,
        ByVal DoneBy As String, ByVal TotalAmount As Decimal,
        ByVal ClicksFileName As String, ByVal BankCode As String, ByVal HeaderNo As String,
        ByRef BatchCode As String) As Boolean

        'Create Instances - Start
        Dim _AccountsEn As AccountsEn
        Dim _AccountsBAL As New AccountsBAL
        Dim ListStudentEn As New List(Of StudentEn)
        Dim StudentReceiptsList As New List(Of AccountsEn)
        Dim ListAccountsDetails As New List(Of AccountsDetailsEn)
        'Create Instances - Stop

        'Variable Declarations - Start
        Dim StudentName As String = Nothing
        Dim PaidAmount As Decimal = 0, IdentityNo As String = Nothing, ReceiptDate As String = Nothing
        Dim _DataGridItem As DataGridItem = Nothing, MatricNo As String = Nothing, ReceiptNo As String = Nothing
        Dim NoFiles As Integer = 0
        'Variable Declarations - Stop

        Session("Temp_BatchCode") = Nothing

        Try

            NoFiles = dgClicksTransactions.Items.Count
            'Insert File Details - Start
            'Dim _AccountDalFileDts As New HTS.SAS.DataAccessObjects.AccountsDAL
            'If _AccountDalFileDts.InsertFileDetails(ClicksFileName, TotalAmount, NoFiles, DateTime.Now, BankCode) = False Then
            '    Return False
            'End If
            'Insert File Details - Ended

            'Loop thro the Data grid items - Start
            For Each _DataGridItem In dgClicksTransactions.Items

                'Get Values - Start
                MatricNo = clsGeneric.NullToString(_DataGridItem.Cells(1).Text)
                IdentityNo = clsGeneric.NullToString(_DataGridItem.Cells(2).Text)
                ReceiptNo = clsGeneric.NullToString(_DataGridItem.Cells(3).Text)
                ReceiptDate = clsGeneric.NullToString(_DataGridItem.Cells(4).Text)
                PaidAmount = clsGeneric.NullToString(_DataGridItem.Cells(5).Text)
                StudentName = clsGeneric.NullToString(_DataGridItem.Cells(0).Text)
                'Get Values - Stop

                'initialize instance
                _AccountsEn = New AccountsEn()

                'Set Account Entities Values - Start
                _AccountsEn.SubType = ReceiptsClass.Student
                _AccountsEn.CreatedBy = DoneBy
                _AccountsEn.UpdatedBy = DoneBy
                _AccountsEn.TransType = ReceiptsClass.Credit
                _AccountsEn.BankCode = BankCode
                _AccountsEn.BatchDate = DateTime.Now
                _AccountsEn.TransDate = ReceiptDate
                _AccountsEn.TransStatus = ReceiptsClass.StatusOpen
                _AccountsEn.Description = "CIMB CLICKS - " + GetHeaderDate(HeaderNo)
                _AccountsEn.PostStatus = ReceiptsClass.StatusReady
                _AccountsEn.Category = ReceiptsClass.CategoryCimbClicks
                _AccountsEn.SubReferenceOne = ReceiptNo
                _AccountsEn.PaymentMode = ReceiptsClass.PaymentModeCash
                _AccountsEn.PostedDateTime = DateTime.Now
                _AccountsEn.DueDate = DateTime.Now
                _AccountsEn.UpdatedTime = DateTime.Now
                _AccountsEn.ChequeDate = DateTime.Now
                _AccountsEn.CreatedDateTime = DateTime.Now
                _AccountsEn.TransactionAmount = TotalAmount
                'Set Account Entities Values - Stop

                'Create Instance of Student Entity
                Dim _StudentEn As New StudentEn

                'Set Values - Start
                _StudentEn.MatricNo = MatricNo
                _StudentEn.StudentName = StudentName
                _StudentEn.ICNo = IdentityNo
                _StudentEn.TransactionAmount = PaidAmount
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
                'Set Values - Stop

                'Create Instance of Accounts Details Entities
                Dim _AccountDetailsEn As New AccountsDetailsEn

                'Set Values - Start
                _AccountDetailsEn.PaidAmount = 0
                _AccountDetailsEn.PostStatus = ReceiptsClass.StatusReady
                _AccountDetailsEn.TransStatus = ReceiptsClass.StatusOpen
                _AccountDetailsEn.ReferenceTwo = ReceiptsClass.AllocateAuto
                _AccountDetailsEn.ReferenceOne = MatricNo
                _AccountDetailsEn.TransactionAmount = PaidAmount
                'Set Values - Stop

                'Add to Accounts Details Entities List
                ListAccountsDetails.Add(_AccountDetailsEn)

                'Add Accounts Details List to Accounts Entities
                _StudentEn.AccountDetailsList = ListAccountsDetails

                'Add Student Entities to Student List
                ListStudentEn.Add(_StudentEn)

                'Save Details

                'Added by Hafiz Roslan @ 29/6/2016
                If Not Session("Temp_BatchCode") Is Nothing Then
                    _AccountsEn.BatchCode = Session("Temp_BatchCode")
                End If

                BatchCode = _AccountsBAL.StudentBatchInsert(_AccountsEn, ListStudentEn)
                Session("Temp_BatchCode") = BatchCode

                'Added by Hafiz Roslan @ 11/01/2016
                'Reason:Save/Update Header No details in the Receipt Details Table
                _AccountsBAL.InsertHeaderNoDetails(_AccountsEn, HeaderNo)

                'Set Batch Code
                '_DataGridItem.Cells(6).Text = BatchCode

                'Clear Data - Start
                ListStudentEn.Clear()
                ListAccountsDetails.Clear()
                'Clear Data - Stop

            Next
            'Loop thro the Data grid items - Stop

            Return True

        Catch ex As Exception

            'Log Error 
            Call MaxModule.Helper.LogError(ex.Message)

            Return False

        End Try

    End Function

#End Region

#Region "GetHeaderDate()"
    'added by Hafiz @ 25/7/2017
    'to get header date to populate at stud ledger

    Public Function GetHeaderDate(ByRef HeaderNo As String) As String

        Dim result As String = Nothing

        If Left(HeaderNo, GetCimbClicksFileHeaderIdentifierLen) = GetCimbClicksFileHeaderIdentifier() Then

            result = GetPositionValue(HeaderNo, 2, 9)

            result = Left(result, 2) & "/" & result.Substring(2, 2) & "/" & Right(result, 4)

        End If

        Return result

    End Function

#End Region

#Region "Upload Bank Statement "

    'Purpose			: To Upload Bank Statement 
    'Author			    : Sujith Sharatchandran - T-Melmax Sdn Bhd
    'Created Date		: 22/06/2015
    Public Function UploadBankStatement(ByVal BankStatement As String,
        ByRef dgMatchingRecords As DataGrid, ByRef dgUnMatchingRecords As DataGrid,
        ByRef MatchingTotalAmount As Decimal, ByRef MatchingTotalRecords As Integer,
        ByRef UnMatchingTotalAmount As Decimal, ByRef UnMatchingTotalRecords As Integer) As Boolean

        'Create Instances - Start
        Dim WorkSheets As DataTable = Nothing
        Dim FileContents As New DataTable
        Dim _OleDbCommand As OleDb.OleDbCommand
        Dim _OleDbConnection As OleDb.OleDbConnection
        Dim _OleDbDataAdapter As OleDb.OleDbDataAdapter
        'Create Instances - Stop

        'Variable Declarations
        Dim ExcelConnectionString As String = Nothing, SQLStatement As String = Nothing

        Try

            'build connection string - Start
            ExcelConnectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" & BankStatement
            ExcelConnectionString += ";Extended Properties='Excel 12.0;HDR=YES;IMEX=1';"
            'build connection string - Stop

            'create instance of OleDbConnection
            _OleDbConnection = New OleDb.OleDbConnection(ExcelConnectionString)

            'open connection
            _OleDbConnection.Open()

            'Get the data table containg the schema guid. - Start
            WorkSheets = _OleDbConnection.GetOleDbSchemaTable(
                OleDb.OleDbSchemaGuid.Tables, Nothing)
            'Get the data table containg the schema guid. - Stop

            'if excel file contains worksheets - Start
            If WorkSheets.Rows.Count > 0 Then

                'build sql statement
                SQLStatement = "SELECT * FROM [" + WorkSheets.Rows(0)(2).ToString() + "]"

                'execute sql statment as OleDb Command
                _OleDbCommand = New OleDb.OleDbCommand(SQLStatement, _OleDbConnection)

                'get work sheet data
                _OleDbDataAdapter = New OleDb.OleDbDataAdapter(_OleDbCommand)

                'fill data set
                _OleDbDataAdapter.Fill(FileContents)

            End If
            'if excel file contains worksheets - Stop

            _OleDbConnection.Close()
            _OleDbConnection.Dispose()



            Return ReadBankStatementMatching(FileContents, dgMatchingRecords,
                dgUnMatchingRecords, MatchingTotalAmount, MatchingTotalRecords,
                UnMatchingTotalAmount, UnMatchingTotalRecords)


        Catch ex As Exception

            'Log Error
            Call MaxModule.Helper.LogError(ex.Message)
            Return False

        End Try

    End Function

#End Region

#Region "Read Bank Statement & Matching "

    'Purpose			: To Read Bank Statement & Matching
    'Author			    : Sujith Sharatchandran - T-Melmax Sdn Bhd
    'Created Date		: 22/06/2015
    Private Function ReadBankStatementMatching(ByVal FileContents As DataTable,
        ByRef dgMatchingRecords As DataGrid, ByRef dgUnMatchingRecords As DataGrid,
        ByRef MatchingTotalAmount As Decimal, ByRef MatchingTotalRecords As Integer,
        ByRef UnMatchingTotalAmount As Decimal, ByRef UnMatchingTotalRecords As Integer) As Boolean

        'Create Instances - Start
        Dim _DataRow As DataRow = Nothing
        Dim _StatementMatchingEntity As StatementMatchingEntity = Nothing
        Dim _StatementUnMatchingEntity As StatementUnMatchingEntity = Nothing
        Dim ListStatementMatchingEntity As New List(Of StatementMatchingEntity)
        Dim ListStatementUnMatchingEntity As New List(Of StatementUnMatchingEntity)
        'Create Instances - Stop

        ' Session("FileData") = FileContents

        'Variable Declarations - Start
        Dim ReceiptDate As String = Nothing, PaidAmount As Decimal = 0
        Dim BankAccountNo As String = Nothing, BankReceiptNo As String = Nothing
        'Variable Declarations - Stop

        Try

            Dim ind As Integer = 0
            'Loop thro the Data Table - Start


            For Each _DataRow In FileContents.Rows
                If ind > 5 Then
                    'Get Values - Start
                    ' If FileContents.Rows.Count > 6 Then

                    BankAccountNo = clsGeneric.NullToString(
                 _DataRow(GetBankStatementAccountNoCol))

                    ' BankAccountNo = "12150007457053"

                    BankReceiptNo = clsGeneric.NullToString(
                        _DataRow(GetBankStatementReceiptNoCol))
                    ReceiptDate = clsGeneric.NullToString(
                        _DataRow(GetBankStatementReceiptDateCol))
                    'PaidAmount = clsGeneric.NullToString(
                    '    _DataRow(GetBankStatementPaidAmountCol))

                    PaidAmount = clsGeneric.NullToDecimal(
                   _DataRow(GetBankStatementPaidAmountCol))

                    ' End If

                    'Get Values - Stop

                    'Check if matching or not matching - Start
                    '  If IsReceiptExists(BankAccountNo, BankAccountNo) Then

                    If IsReceiptExists(BankAccountNo, BankReceiptNo) Then

                        'Set Values - Start
                        _StatementMatchingEntity = New StatementMatchingEntity
                        _StatementMatchingEntity.RECEIPT_NO = BankReceiptNo
                        _StatementMatchingEntity.PAID_AMOUNT = PaidAmount
                        _StatementMatchingEntity.RECEIPT_DATE = ReceiptDate
                        'Set Values - Stop

                        'Add to List
                        ListStatementMatchingEntity.Add(_StatementMatchingEntity)

                        'Add Matching Total Amount
                        MatchingTotalAmount = MatchingTotalAmount + PaidAmount

                        'Add Matching Total Reoords
                        MatchingTotalRecords = MatchingTotalRecords + 1

                    Else

                        'Set Values - Start
                        _StatementUnMatchingEntity = New StatementUnMatchingEntity
                        _StatementUnMatchingEntity.RECEIPT_NO = BankReceiptNo
                        _StatementUnMatchingEntity.PAID_AMOUNT = PaidAmount
                        _StatementUnMatchingEntity.RECEIPT_DATE = ReceiptDate
                        'Set Values - Stop

                        'Add to List
                        ListStatementUnMatchingEntity.Add(_StatementUnMatchingEntity)

                        'Add Unmatching Total Amount
                        UnMatchingTotalAmount = UnMatchingTotalAmount + PaidAmount

                        'Add UnMatching Total Reoords
                        UnMatchingTotalRecords = UnMatchingTotalRecords + 1

                    End If
                    'Check if matching or not matching - Stop

                End If
                ind = ind + 1
            Next

            'Loop thro the Data Table - Stop

            'Populate Matching Data grid - Start
            If ListStatementMatchingEntity.Count > 0 Then
                dgMatchingRecords.DataSource = ListStatementMatchingEntity
                dgMatchingRecords.DataBind()
            End If
            'Populate Matching Data grid - Stop

            'Populate Un Matching Data grid - Start
            If ListStatementUnMatchingEntity.Count > 0 Then
                dgUnMatchingRecords.DataSource = ListStatementUnMatchingEntity
                dgUnMatchingRecords.DataBind()
            End If
            'Populate Un Matching Data grid - Stop

            Return True

        Catch ex As Exception

            'Log Error
            Call MaxModule.Helper.LogError(ex.Message)
            Return False

        End Try

    End Function

#End Region

#Region "Check Receipt Exists "

    'Purpose			: To Read Bank Statement & Matching
    'Author			    : Sujith Sharatchandran - T-Melmax Sdn Bhd
    'Created Date		: 22/06/2015
    Private Function IsReceiptExists(ByVal BankAccountNo As String,
        ByVal BankReceiptNo As String) As Boolean

        'Variable Declarations
        Dim SqlStatement As String = Nothing, RowCounter As Short = 0

        Try

            'Build Sql Statement - Start
            SqlStatement = "SELECT COUNT(*) AS Total_Records FROM SAS_Accounts INNER JOIN "
            SqlStatement &= "SAS_BankDetails ON SAS_Accounts.BankCode = SAS_BankDetails.SABD_Code "
            SqlStatement &= " WHERE SAS_Accounts.SubRef1 = " & clsGeneric.AddQuotes(BankReceiptNo)
            SqlStatement &= " AND sabd_accode = " & clsGeneric.AddQuotes(BankAccountNo)
            'Build Sql Statement - Stop

            'get file counter - Start
            RowCounter = clsGeneric.NullToShort(_DatabaseFactory.ExecuteScalar(DataBaseType,
                DataBaseConnectionString, SqlStatement))
            'get file counter - Stop

            If RowCounter > 0 Then
                Return True
            End If

            Return False

        Catch ex As Exception

            'Log Error
            Call MaxModule.Helper.LogError(ex.Message)
            Return False

        End Try

    End Function

#End Region

#Region "Check if Recon File Uploaded "

    'Purpose			: To Check if Recon File Uploaded
    'Author			    : Sujith Sharatchandran - T-Melmax Sdn Bhd
    'Created Date		: 24/06/2015
    Public Function IsReconFileUploaded(ByVal UploadedReconFile As String) As Boolean

        'Variable Declarations
        Dim SqlStatement As String = Nothing, FileCounter As Short = 0

        Try

            'Build Sql Statement - Start
            SqlStatement = "SELECT COUNT(*) AS Total_Files FROM sas_bankrecon_filedetails WHERE "
            SqlStatement &= " File_In_Name = " & clsGeneric.AddQuotes(Path.GetFileName(UploadedReconFile))
            'Build Sql Statement - Stop

            'get file counter - Start
            FileCounter = clsGeneric.NullToShort(_DatabaseFactory.ExecuteScalar(DataBaseType,
                DataBaseConnectionString, SqlStatement))
            'get file counter - Stop

            If FileCounter > 0 Then
                Return True
            End If

            Return False

        Catch ex As Exception

            'Log Error 
            Call MaxModule.Helper.LogError(ex.Message)

            Return False

        End Try

    End Function

#End Region

#Region "Track Recon File Details "

    'Purpose			: Track Recon File Details
    'Author			    : Sujith Sharatchandran - T-Melmax Sdn Bhd
    'Created Date		: 16/06/2015
    Public Sub TrackReconFileDetails(ByVal UploadFileName As String,
        ByVal MatchingTotalAmount As Decimal, ByVal MatchingTotalRecords As Integer,
        ByVal UnMatchingTotalAmount As Decimal, ByVal UnMatchingTotalRecords As Integer,
        ByVal BankCode As String)

        'Variable Declarations
        Dim SqlStatement As String = Nothing

        Try

            'Build Sql Statement - Start
            SqlStatement = "INSERT INTO sas_bankrecon_filedetails(File_In_Name"
            SqlStatement &= ",file_match_amount,file_match_transactions,file_unmatch_amount,"
            SqlStatement &= "file_unmatch_transactions,Date_Time,Bank_Code)"
            SqlStatement &= " VALUES(" & clsGeneric.AddQuotes(Path.GetFileName(UploadFileName))
            SqlStatement &= clsGeneric.AddComma() & clsGeneric.AddQuotes(MatchingTotalAmount)
            SqlStatement &= clsGeneric.AddComma() & MatchingTotalRecords
            SqlStatement &= clsGeneric.AddComma() & clsGeneric.AddQuotes(UnMatchingTotalAmount)
            SqlStatement &= clsGeneric.AddComma() & UnMatchingTotalRecords
            SqlStatement &= clsGeneric.AddComma() & clsGeneric.AddQuotes(DataAccessObjects.Helper.DateConversion(Now))
            SqlStatement &= clsGeneric.AddComma() & clsGeneric.AddQuotes(BankCode)
            SqlStatement &= ")"
            'Build Sql Statement - Stop

            'Execute Sql Statement - Start
            _DatabaseFactory.ExecuteSqlStatement(DataBaseType,
                DataBaseConnectionString, SqlStatement)
            'Execute Sql Statement - Stop

        Catch ex As Exception

            'Log Error 
            Call MaxModule.Helper.LogError(ex.Message)

        End Try

    End Sub

#End Region

#Region "Update Student Accounts "

    'Purpose			: Update Student Accounts
    'Author			    : Sujith Sharatchandran - T-Melmax Sdn Bhd
    'Created Date		: 16/06/2015
    Public Function UpdateStudentAccounts(ByVal UploadFile As String,
        ByRef TotalRecords As Integer, ByRef CountUpdated As Integer) As Boolean

        'Create Instances - Start
        Dim WorkSheets As DataTable = Nothing
        Dim FileContents As New DataTable
        Dim _OleDbCommand As OleDb.OleDbCommand
        Dim _OleDbConnection As OleDb.OleDbConnection
        Dim _OleDbDataAdapter As OleDb.OleDbDataAdapter
        'Create Instances - Stop

        'Variable Declarations
        Dim ExcelConnectionString As String = Nothing, SQLStatement As String = Nothing

        Try

            'build connection string - Start
            ExcelConnectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" & UploadFile
            ExcelConnectionString += ";Extended Properties='Excel 12.0;HDR=YES;IMEX=1';"
            'build connection string - Stop

            'create instance of OleDbConnection
            _OleDbConnection = New OleDb.OleDbConnection(ExcelConnectionString)

            'open connection
            _OleDbConnection.Open()

            'Get the data table containg the schema guid. - Start
            WorkSheets = _OleDbConnection.GetOleDbSchemaTable(
                OleDb.OleDbSchemaGuid.Tables, Nothing)
            'Get the data table containg the schema guid. - Stop

            'if excel file contains worksheets - Start
            If WorkSheets.Rows.Count > 0 Then

                'build sql statement
                SQLStatement = "SELECT * FROM [" + WorkSheets.Rows(0)(2).ToString() + "]"

                'execute sql statment as OleDb Command
                _OleDbCommand = New OleDb.OleDbCommand(SQLStatement, _OleDbConnection)

                'get work sheet data
                _OleDbDataAdapter = New OleDb.OleDbDataAdapter(_OleDbCommand)

                'fill data set
                _OleDbDataAdapter.Fill(FileContents)

            End If
            'if excel file contains worksheets - Stop

            _OleDbConnection.Close()
            _OleDbConnection.Dispose()

            Return ReadStudentAccounts(FileContents, TotalRecords, CountUpdated)

        Catch ex As Exception
            'Log Error
            Call MaxModule.Helper.LogError(ex.Message)

            'Added By Zoya@1/03/2016
            If ex.Message = "Student Bank Not Matched" Then
                Throw New Exception(ex.Message)
            End If
            'End Added By Zoya@1/03/2016 

            Return False

        End Try

    End Function

#End Region

#Region "Read & Update Student Accounts "

    'Purpose			: To Read Student Accounts
    'Author			    : Sujith Sharatchandran - T-Melmax Sdn Bhd
    'Created Date		: 22/06/2015
    Public Function ReadStudentAccounts(ByVal FileContents As DataTable,
        ByRef TotalStudents As Integer, ByRef CountUpdated As Integer) As Boolean

        'Create Instances
        Dim _DataRow As DataRow = Nothing
        Dim StudDetails As IDataReader = Nothing

        'Variable Declarations - Start
        Dim SqlStatement As String = Nothing, MatricNo As String = Nothing
        Dim BankAccountNo As String = Nothing, BankCode As String = Nothing
        Dim CountSame As Integer = 0
        'Variable Declarations - Stop

        Try

            'Loop thro the Data Table - Start
            For Each _DataRow In FileContents.Rows

                'Get Values - Start
                BankAccountNo = clsGeneric.NullToString(_DataRow(GetStudentAccountNoCol))
                BankCode = clsGeneric.NullToString(_DataRow(GetStudentBankCodeCol))
                MatricNo = clsGeneric.NullToString(_DataRow(GetStudentMatricNoCol))
                'Get Values - Stop

                'Added by Zoya @1/03/2016

                SqlStatement = "select COUNT(*) AS cnt from sas_studentbank where sasb_code = " & clsGeneric.AddQuotes(BankCode) & ";"

                If _DatabaseFactory.ExecuteScalar(DataBaseType,
                    DataBaseConnectionString, SqlStatement) > 0 Then

                    'modified by Hafiz @ 21/4/2016
                    'check acc number; available/not - start
                    StudDetails = GetStudAccDetails(MatricNo)

                    If StudDetails.Read() Then

                        Dim obj As New StudentEn

                        obj.MatricNo = clsGeneric.NullToString(StudDetails(stud_matricno))
                        obj.SASI_Bank = clsGeneric.NullToString(StudDetails(stud_bank))
                        obj.SASI_AccNo = clsGeneric.NullToString(StudDetails(stud_accno))

                        If obj.SASI_AccNo = BankAccountNo Then

                            Session("return") = "one_!same"

                            CountSame = CountSame + 1
                        Else

                            'Build Sql Statement - Start
                            SqlStatement = "UPDATE sas_student SET sasi_accno = " & clsGeneric.AddQuotes(BankAccountNo)
                            SqlStatement &= clsGeneric.AddComma() & " sasi_bank = " & clsGeneric.AddQuotes(BankCode)
                            SqlStatement &= " WHERE sasi_matricno = " & clsGeneric.AddQuotes(MatricNo) & ";"
                            'Build Sql Statement - Stop

                            'Execute Update Sql Statement - Start
                            _DatabaseFactory.ExecuteSqlStatement(DataBaseType,
                                DataBaseConnectionString, SqlStatement)
                            'Execute Update Sql Statement - Stop

                            CountUpdated = CountUpdated + 1
                        End If

                        'Increment Counter
                        TotalStudents = TotalStudents + 1

                    Else
                        Session("matricno_exist") = "yes"
                    End If
                    'check acc number; available/not - end

                Else
                    Throw New Exception("Student Bank Not Matched")
                End If

                'End Added by Zoya @1/03/2016
            Next
            'Loop thro the Data Table - Stop


            If CountSame = TotalStudents Then
                Session("return") = "all_same"
            End If

            Return True

        Catch ex As Exception
            'Added by Zoya @1/03/2016
            Throw New Exception(ex.Message)
            'End Added by Zoya @1/03/2016

            'Log Error
            Call MaxModule.Helper.LogError(ex.Message)
            Return False

        End Try

    End Function

#End Region

#Region "Check if Student Accounts Uploaded "

    'Purpose			: To Check if Stundet Accounts Uploaded
    'Author			    : Sujith Sharatchandran - T-Melmax Sdn Bhd
    'Created Date		: 16/06/2015
    Public Function IsStudentsFileUploaded(ByVal UploadedStudentFile As String) As Boolean

        'Variable Declarations
        Dim SqlStatement As String = Nothing, FileCounter As Short = 0

        Try

            'Build Sql Statement - Start
            SqlStatement = "SELECT COUNT(*) AS Total_Files FROM sas_studAccs_filedetails WHERE "
            SqlStatement &= " File_In_Name = " & clsGeneric.AddQuotes(Path.GetFileName(UploadedStudentFile))
            'Build Sql Statement - Stop

            'get file counter - Start
            FileCounter = clsGeneric.NullToShort(_DatabaseFactory.ExecuteScalar(DataBaseType,
                DataBaseConnectionString, SqlStatement))
            'get file counter - Stop

            If FileCounter > 0 Then
                Return True
            End If

            Return False

        Catch ex As Exception

            'Log Error 
            Call MaxModule.Helper.LogError(ex.Message)
            Return False

        End Try

    End Function

#End Region

#Region "Track Student Accounts File Details "

    'Purpose			: Track Clicks File Details
    'Author			    : Sujith Sharatchandran - T-Melmax Sdn Bhd
    'Created Date		: 16/06/2015
    Public Sub TrackStudentFileDetails(ByVal UploadFileName As String,
        ByVal TotalRecords As Integer)

        'Variable Declarations
        Dim SqlStatement As String = Nothing

        Try

            'if transactions available - Start
            If TotalRecords > 0 Then

                'Build Sql Statement - Start
                SqlStatement = "INSERT INTO sas_studaccs_filedetails(File_In_Name"
                SqlStatement &= ",File_Transactions,Date_Time)"
                SqlStatement &= " VALUES(" & clsGeneric.AddQuotes(Path.GetFileName(UploadFileName))
                SqlStatement &= clsGeneric.AddComma() & TotalRecords
                SqlStatement &= clsGeneric.AddComma() & clsGeneric.AddQuotes(DataAccessObjects.Helper.DateConversion(Now))
                SqlStatement &= ")"
                'Build Sql Statement - Stop

                'Execute Sql Statement - Start
                _DatabaseFactory.ExecuteSqlStatement(DataBaseType,
                    DataBaseConnectionString, SqlStatement)
                'Execute Sql Statement - Stop

            End If
            'if transactions available - Stop

        Catch ex As Exception

            'Log Error 
            Call MaxModule.Helper.LogError(ex.Message)

        End Try

    End Sub

#End Region

#Region "GetStudentDetailsFromPTPTNFile"

    'Added By Hafiz @ 14/3/2016
    'To get student details used uploaded PTPTN File. Used at the Sponsor Allocation
    'Modified by Hafiz @ 22/3/2016 - change to RIGHT JOIN

    Private Function GetStudentDetailsFromPTPTNFile(ByVal IdentityNo As String) As IDataReader

        Dim SqlStatement As String = Nothing

        Try

            'Build Sql Statement Start
            SqlStatement = "SELECT SAS_Student.SASI_MatricNo, SAS_Student.SASI_Name, SAS_Student.SASI_PgId, SAS_Student.SASI_ICNo,"
            SqlStatement &= " SAS_Student.SASI_CurSem, SAS_Student.SASI_AccNo, SAS_Student.SASS_Code FROM SAS_Student"
            SqlStatement &= " INNER JOIN SAS_StudentStatus ON SAS_StudentStatus.SASS_code = SAS_Student.SASS_code"
            SqlStatement &= " WHERE SAS_Student.SASI_ICNo = " & clsGeneric.AddQuotes(IdentityNo)
            'Build Sql Statement Stop

            'Return Results as Datatable - Start
            Return _DatabaseFactory.ExecuteDataTable(DataBaseType, DataBaseConnectionString, SqlStatement).CreateDataReader()
            'Return Results as Datatable - Stop

        Catch ex As Exception

            Call MaxModule.Helper.LogError(ex.Message)
            Return Nothing

        End Try

    End Function

#End Region

#Region "GetStudentSponsorDetails"
    'added by Hafiz @ 27/3/2016
    'modified by Hafiz @ 06/4/2016
    'details content for the sponsor alloc generating text file

    Private Function GetStudentSponsorDetails(ByVal IdentityNo As String) As IDataReader

        'Variable Declarations
        Dim SqlStatement As String = Nothing

        Try

            'Build Sql Statement Start
            SqlStatement = "SELECT SAS_Student.SASI_Name AS Student_Name, "
            SqlStatement &= "SAS_Student.SASI_MatricNo AS Student_MatricNo, "
            SqlStatement &= "SAS_Student.SASI_AccNo AS Student_Account, "
            SqlStatement &= "SAS_Studentspon.SASS_Sponsor AS Sponsor_Code "
            SqlStatement &= "FROM SAS_Student "
            SqlStatement &= "INNER JOIN SAS_Studentspon ON SAS_Student.SASI_Matricno = SAS_Studentspon.SASI_Matricno "
            SqlStatement &= "WHERE SAS_Student.SASI_ICNo = " & clsGeneric.AddQuotes(IdentityNo)
            'Build Sql Statement Stop

            'Return Results as Datatable - Start
            Return _DatabaseFactory.ExecuteDataTable(DataBaseType,
                DataBaseConnectionString, SqlStatement).CreateDataReader()
            'Return Results as Datatable - Stop

        Catch ex As Exception

            'Log Error 
            Call MaxModule.Helper.LogError(ex.Message)
            Return Nothing

        End Try

    End Function

#End Region

#Region "Mandate Generate To Text File"

    'added by Hafiz @ 06/4/2016
    'modified @ 25/5/2016
    'methods for Mandate Generate To Text File

    Public Function MandateGenerateToTextFile(ByVal obj As Object, ByRef TotalRecords As Integer) As String

        Dim _StringBuilder As New StringBuilder
        Dim _StudentDAL As New StudentDAL

        Dim Index As Integer = 0, LineContent As String = Nothing, stud_icno As String = Nothing

        Try
            While Index < obj.Count

                stud_icno = obj(Index).ICNo

                Dim argEn As StudentEn = _StudentDAL.GetStudInfo(stud_icno)

                TotalRecords = TotalRecords + 1

                'account no - start
                Dim str_accno As String = Nothing

                If argEn.ICNo.Length > 12 Then
                    str_accno = clsGeneric.StringFiller(argEn.ICNo, 14, EnumHelp.FillerType.Suffix, " ")
                Else
                    str_accno = clsGeneric.StringFiller("", 14 - argEn.ICNo.Length, EnumHelp.FillerType.Suffix, " ")
                    str_accno &= clsGeneric.StringFiller(argEn.ICNo, argEn.ICNo.Length, EnumHelp.FillerType.Suffix, " ")
                End If
                'account no - end

                'student name - start
                Dim studname As String = Nothing

                If argEn.StudentName.Length > 20 Then
                    studname = argEn.StudentName.Substring(0, 20)
                Else
                    studname = argEn.StudentName
                End If
                'student name - end

                'Build Line Content - Start
                LineContent = "50"                                                                          'record tye
                LineContent &= str_accno                                                                    'acc no
                LineContent &= GetCimbOrgCode()                                                             'org code
                LineContent &= clsGeneric.StringFiller(studname, 20, EnumHelp.FillerType.Suffix, " ")       'name
                LineContent &= clsGeneric.StringFiller("", 14, EnumHelp.FillerType.Prefix, "9")             'limit amount
                LineContent &= clsGeneric.StringFiller("", 14, EnumHelp.FillerType.Prefix, "0")             'fixed amount
                LineContent &= clsGeneric.StringFiller(argEn.MatricNo, 30, EnumHelp.FillerType.Suffix, " ") 'refno
                LineContent &= clsGeneric.StringFiller(stud_icno, 12, EnumHelp.FillerType.Suffix, " ")      'icno
                LineContent &= clsGeneric.StringFiller("", 2, EnumHelp.FillerType.Prefix, "0")              'frequency
                LineContent &= clsGeneric.StringFiller("", 3, EnumHelp.FillerType.Prefix, "0")              'frequency limit
                LineContent &= clsGeneric.StringFiller("", 85, EnumHelp.FillerType.Suffix, " ")             'filler
                'Build Line Content - Stop

                _StringBuilder.AppendLine(LineContent)

                Index = Index + 1

            End While

            Return _StringBuilder.ToString()

        Catch ex As Exception

            'Log Error 
            Call MaxModule.Helper.LogError(ex.Message)
            Return String.Empty

        End Try

    End Function

#End Region

#Region "GetStudAccDetails"
    'added by Hafiz @ 21/4/2016
    'modified @ 25/5/2016
    'get student accounts details from SAS_Student

    Private Function GetStudAccDetails(ByVal MatricNo As String) As IDataReader

        'Variable Declarations
        Dim SqlStatement As String = Nothing

        Try

            'Build Sql Statement Start
            SqlStatement = "SELECT SASI_MatricNo,SASI_Bank,SASI_AccNO "
            SqlStatement &= "FROM SAS_Student "
            SqlStatement &= "WHERE SASI_MatricNo = " & clsGeneric.AddQuotes(MatricNo)
            'Build Sql Statement Stop

            'Return Results as Datatable - Start
            Return _DatabaseFactory.ExecuteDataTable(DataBaseType,
                DataBaseConnectionString, SqlStatement).CreateDataReader()
            'Return Results as Datatable - Stop

        Catch ex As Exception

            'Log Error 
            Call MaxModule.Helper.LogError(ex.Message)
            Return Nothing

        End Try

    End Function

#End Region

#Region "GetMinBalance"

    'added by Hafiz @ 09/9/2016
    Function GetMinBalance() As Double

        Dim lst As New List(Of PTPTNSetupEn)
        Dim objDAL As New PTPTNSetupDAL

        lst = objDAL.GetList(New PTPTNSetupEn)

        If lst.Count = 1 Then

            For Each x As PTPTNSetupEn In lst
                Return x.min_balance
            Next

        End If

        Return 0.0

    End Function


#End Region

End Class
