#Region "NameSpaces "

Imports System
Imports MaxGeneric
Imports System.Data
Imports System.Data.Common
Imports System.Configuration
Imports System.Collections.Generic

#End Region

Public Class Workflow

#Region "Enumerators "

    Public Enum CategoryType

        Loan = 1
        Refund = 2
        Allocation = 3

    End Enum

    Public Enum TransactionType

        Debit = 1
        Credit = 2

    End Enum

#End Region

#Region "Global Decarations "

    'Create instances
    Private _DataBaseProvider As New MaxModule.DatabaseProvider()

#End Region

#Region "Post to Cf "

    'Purpose			: To Post to Century Financials
    'Author			    : Sujith Sharatchandran - T-Melmax Sdn Bhd
    'Created Date		: 01/06/2015
    Public Function PostToCenturyFinancials(ByVal WorkflowId As Integer) As Boolean

        'Create Instances - Start
        Dim _CfPosting As New CfPosting()
        Dim PostingDetails As IDataReader = Nothing
        'Create Instances - Stop

        'Variable Declarations - Start
        Dim BatchCode As String = Nothing, PostingType As CfGeneric.PostingType
        Dim SqlStatement As String = Nothing, TransactionDate As String = Nothing
        Dim TransactionCode As String = Nothing, CategoryType As String = Nothing
        Dim TransactionType As String = Nothing, CategorySubType As String = Nothing
        Dim PostedBy As String = Nothing, TransactionId As Integer = 0, ReferenceNo As String = Nothing
        'Variable Declarations - Stop

        Try

            'Buils Sql Statemnent - Start
            SqlStatement = "SELECT sas_accounts.transid AS Trans_Id,sas_accounts.batchcode AS Batch_Code,sas_accounts.category AS Category_Type,"
            SqlStatement &= "sas_accounts.subtype AS Sub_Type,sas_accounts.transtype AS Trans_Type,"
            SqlStatement &= "sas_accounts.transcode AS Trans_Code,sas_workflow.date_time AS Trans_Date,"
            SqlStatement &= "sas_workflow.user_id AS Posted_By FROM SAS_Accounts"
            SqlStatement &= " INNER JOIN sas_workflow ON SAS_Accounts.batchcode = sas_workflow.batch_code"
            SqlStatement &= " WHERE sas_accounts.category NOT IN ('SPA','STA') AND sas_workflow.workflow_id = " & WorkflowId
            'Buils Sql Statemnent - Stop

            'Get Posting Details - Start
            PostingDetails = _DataBaseProvider.ExecuteReader(Helper.GetDataBaseType,
                Helper.GetConnectionString, SqlStatement).CreateDataReader()
            'Get Posting Details - Stop

            'if Records Exist - Start
            If PostingDetails.Read Then

                'Get Values - Start
                PostedBy = PostingDetails(Helper.PostedByCol)
                BatchCode = PostingDetails(Helper.BatchCodeCol)
                TransactionId = PostingDetails(Helper.TransIdCol)
                CategoryType = PostingDetails(Helper.CategoryTypeCol)
                CategorySubType = PostingDetails(Helper.CategorySubTypeCol)
                TransactionType = PostingDetails(Helper.TransactionTypeCol)
                TransactionCode = PostingDetails(Helper.TransactionCodeCol)
                TransactionDate = CfGeneric.DateConversion(PostingDetails(Helper.TransactionDateCol))
                'Get Values - Stop

                'Get Posting Type - Start
                PostingType = GetPostingType(CategoryType,
                    CategorySubType, TransactionType)
                'Get Posting Type - Stop

                'Posting Type - Start
                Select Case PostingType

                    Case CfGeneric.PostingType.MultipleJournel

                        'Post to Multiple Journel - Start
                        If _CfPosting.MultipleJournel(CategoryType, BatchCode,
                            TransactionCode, TransactionType, PostedBy, ReferenceNo) Then

                            'Track Posting - Start
                            Call TrackPosting(BatchCode, ReferenceNo,
                                CfGeneric.PostingType.MultipleJournel)
                            'Track Posting - Stop

                            Return True

                        End If
                        'Post to Multiple Journel - Stop

                    Case CfGeneric.PostingType.CashBookPayment

                        'Post to Multiple Journel - Start
                        If _CfPosting.CashBookPayment(TransactionId, BatchCode, ReferenceNo) Then

                            'Track Posting - Start
                            Call TrackPosting(BatchCode, ReferenceNo,
                                CfGeneric.PostingType.CashBookPayment)
                            'Track Posting - Stop

                            Return True

                        End If
                        'Post to Multiple Journel - Stop

                    Case CfGeneric.PostingType.CashBookReceipt

                        'Post to Multiple Journel - Start
                        If _CfPosting.CashBookReceipt(TransactionId, BatchCode, ReferenceNo) Then

                            'Track Posting - Start
                            Call TrackPosting(BatchCode, ReferenceNo,
                                CfGeneric.PostingType.CashBookReceipt)
                            'Track Posting - Stop

                            Return True

                        End If
                        'Post to Multiple Journel - Stop

                End Select
                'Posting Type - Stop

            End If
            'if Records Exist - 

            Return False

        Catch ex As Exception

            'log error
            Call Helper.LogError(ex.Message)

            Return False

        End Try

    End Function

#End Region

#Region "Get Posting Type "

    'Purpose			: Get Posting Type
    'Author			    : Sujith Sharatchandran - T-Melmax Sdn Bhd
    'Created Date		: 01/06/2015
    Private Function GetPostingType(ByVal CategoryType As String,
        ByVal CategorySubType As String, ByVal TransactionType As String) As CfGeneric.PostingType

        Select Case CategoryType

            Case CfGeneric.CategoryTypeInvoice, CfGeneric.CategoryTypeAfc,
                CfGeneric.CategoryTypeCreditNote, CfGeneric.CategoryTypeDebitNote,
                CfGeneric.CategoryTypeLoan, CfGeneric.CategoryTypeAllocation

                'If CategorySubType = CfGeneric.CategorySubTypeStudent Then
                Return CfGeneric.PostingType.MultipleJournel
                'End If

            Case CfGeneric.CategoryTypeReceipts

                Return CfGeneric.PostingType.CashBookReceipt

            Case CfGeneric.CategoryTypePayments, CfGeneric.CategoryTypeRefund

                Return CfGeneric.PostingType.CashBookPayment

        End Select

    End Function

#End Region

#Region "Track Posting "

    'Purpose			: Track Posting
    'Author			    : Sujith Sharatchandran - T-Melmax Sdn Bhd
    'Created Date		: 16/06/2015
    Private Sub TrackPosting(ByVal BatchCode As String,
        ByVal ReferenceNo As String, ByVal PostingType As Short)

        'Variable Declarations
        Dim SqlStatement As String = Nothing

        Try

            'Build Sql Statement - Start
            SqlStatement = "INSERT INTO sas_saga_posting("
            SqlStatement &= "batch_code, reference_no, posting_type, posting_date)"
            SqlStatement &= " VALUES(" & clsGeneric.AddQuotes(BatchCode)
            SqlStatement &= clsGeneric.AddComma() & clsGeneric.AddQuotes(ReferenceNo)
            SqlStatement &= clsGeneric.AddComma() & PostingType
            SqlStatement &= clsGeneric.AddComma() & clsGeneric.AddQuotes(Helper.DateConversion(Now))
            SqlStatement &= ")"
            'Build Sql Statement - Stop

            'Execute Sql Statement - Start
            _DataBaseProvider.ExecuteSqlStatement(Helper.GetDataBaseType,
                Helper.GetConnectionString, SqlStatement)
            'Execute Sql Statement - Stop

        Catch ex As Exception

            'log error
            Call Helper.LogError(ex.Message)

        End Try

    End Sub

#End Region

#Region "Reset Posting Status to Ready "

    'Purpose			: Reset Posting Status to Ready
    'Author			    : Sujith Sharatchandran - T-Melmax Sdn Bhd
    'Created Date		: 16/06/2015
    'Public Sub ResetPostingStatusToReady(ByVal BatchCode As String)
    Public Function ResetPostingStatusToReady(ByVal BatchCode As String) As Boolean

        'Create Instances
        Dim _IDataReader As IDataReader = Nothing

        'Variable Declarations
        Dim SqlStatement As String = Nothing, TransId As Integer = 0, UpdateStatement As String = Nothing

        Try

            'Build Sql Statement - Start
            SqlStatement = "SELECT transid from sas_accounts WHERE batchcode = " &
                clsGeneric.AddQuotes(BatchCode)
            'Build Sql Statement - Stop

            'Get Batch Details - Start
            _IDataReader = _DataBaseProvider.ExecuteReader(Helper.GetDataBaseType,
                Helper.GetConnectionString, SqlStatement).CreateDataReader()
            'Get Batch Details - Stop

            'if record available - Start
            If _IDataReader.Read() Then

                'Get Transaction Id
                TransId = clsGeneric.NullToInteger(_IDataReader(0))

                'Build Update Statement - Start
                UpdateStatement = "UPDATE sas_accountsdetails SET transtempcode = 'T' || transcode,transcode = '',poststatus = 'Ready',transstatus='Open' WHERE transid = " + TransId + ";"
                UpdateStatement += "UPDATE sas_accounts SET transtempcode = 'T' || transcode,transcode = '',poststatus = 'Ready',transstatus='Open' WHERE transid = " + TransId + ";"
                'Build Update Statement - Stop

                'if sql statement not blank - Start
                If Not FormHelp.IsBlank(UpdateStatement) Then

                    'Execute Sql Statement - Start
                    _DataBaseProvider.ExecuteSqlStatement(Helper.GetDataBaseType,
                          Helper.GetConnectionString, UpdateStatement)
                    'Execute Sql Statement - Stop

                    Return True

                    'If (_DataBaseProvider.ExecuteSqlStatement(Helper.GetDataBaseType,
                    '      Helper.GetConnectionString, UpdateStatement) > -1) Then
                    '    Return True
                    'End If
                    'if sql statement not blank - Stop

                End If
                'if record available - Stop

            End If

        Catch ex As Exception

            'log error
            Call Helper.LogError(ex.Message)
            Return False

        End Try

    End Function

#End Region

#Region "Reset Posting Status to Ready StudentLoan"

    'Purpose			: Reset Posting Status to Ready
    'Author			    : Noor Aslinamona - T-Melmax Sdn Bhd
    'Created Date		: 17/03/2016

    Public Function ResetPostingStatusToReadyStudentLoan(ByVal BatchCode As String) As Boolean

        'Variable Declarations
        Dim UpdateStatement As String = Nothing

        Try

            'Build Update Statement - Start                
            UpdateStatement = "UPDATE sas_studentloan SET transtempcode = 'T' || transcode,transcode = '',poststatus = 'Ready',transstatus='Open',"
            UpdateStatement += "postedby = '', postedtimestamp = '' WHERE batchcode = " + clsGeneric.AddQuotes(BatchCode) + ";"
            'Build Update Statement - Stop

            'if sql statement not blank - Start
            If Not FormHelp.IsBlank(UpdateStatement) Then

                'Execute Sql Statement - Start
                'If (_DataBaseProvider.ExecuteSqlStatement(Helper.GetDataBaseType,
                '     Helper.GetConnectionString, UpdateStatement) > -1) Then
                '    Return True
                'End If
                _DataBaseProvider.ExecuteSqlStatement(Helper.GetDataBaseType,
                     Helper.GetConnectionString, UpdateStatement)
                Return True
                'Execute Sql Statement - Stop

                'if sql statement not blank - Stop

            End If
            'if record available - Stop

        Catch ex As Exception

            'log error
            Call Helper.LogError(ex.Message)
            Return False

        End Try

    End Function

#End Region

End Class
