#Region "Name Spaces "

Imports System
Imports MaxGeneric
Imports System.Reflection

#End Region

Public Class CfGeneric

#Region "Constants "

    'Table Names - Start
    Public Const CfCbGlDistTbl As String = "gl_dist"
    Public Const CfMjLineTbl As String = "mj_jnlline"
    Public Const CfMjHeaderTbl As String = "mj_jnlhdr"
    Public Const CfCbPayHeaderTbl As String = "cb_payhdr"
    Public Const CfCbRecHeaderTbl As String = "cb_rcpthdr"
    Public Const CfCbPayBatchTbl As String = "cb_paybatch"
    Public Const CfCbRecBatchTbl As String = "cb_rcptbatch"
    Public Const CfCbPayDetailsTbl As String = "cb_payrmt"
    'Table Names - Stop

    'Generic Constants - Start
    Public Const CategoryTypeAfc As String = "AFC"
    Public Const CategoryTypeDebitNote As String = "Debit Note"
    Public Const CategoryTypeInvoice As String = "Invoice"
    Public Const CategoryTypeReceipts As String = "Receipt"
    Public Const CategoryTypePayments As String = "Payment"
    Public Const TransactionTypeDebit As String = "Debit"
    Public Const TransactionTypeCredit As String = "Credit"
    Public Const CategorySubTypeStudent As String = "Student"
    Public Const CategoryTypeCreditNote As String = "Credit Note"
    Public Const CategoryTypeRefund As String = "Refund"
    Public Const CategoryTypeAllocation As String = "Allocation"
    Public Const CategoryTypePoketAmount As String = "SPA"
    Public Const CategoryTypeLoan As String = "Loan"
    'Generic Constants - Stop

#End Region

#Region "Enumerators "

    Public Enum CategoryType
        Invoice = 1
        Debit_Note = 2
        Credit_Note = 3
    End Enum

    Public Enum PostingType
        CashBookPayment = 1
        CashBookReceipt = 2
        MultipleJournel = 3
    End Enum

    Public Enum CategorySubType
        Afc = 1
        Student = 2
    End Enum

    Public Enum TransType
        Debit = 1
        Credit = 2
    End Enum

    Public Enum GlType

        BankCode = 1
        StudentProgram = 2

    End Enum

#End Region

#Region "Date Conversion "

    Public Shared Function DateConversion(ByVal _DateTime As DateTime) As String
        Return String.Format("{0:u}", _DateTime)
    End Function

#End Region

#Region "Build Sql Statement "

    'Purpose			: To Build Sql Statement from Entity Class
    'Author			    : Sujith Sharatchandran - T-Melmax Sdn Bhd
    'Created Date		: 29/05/2015
    Public Function BuildSqlStatement(ByVal EntityClass As Object,
        ByRef SqlStatement As String, ByVal TableName As String) As Boolean

        'Variable Declarations - Start
        Dim TableColSql As String = Nothing, TableColValueSql As String = Nothing
        Dim ColumnValue As String = Nothing, Counter As Integer = 0, ColumnName As String = Nothing
        'Variable Declarations - Stop

        Try

            'Build Sql Statement
            SqlStatement &= "INSERT INTO " & TableName & "("

            'loop thro the Properties to the property name/value as column name/value - Start
            For Each _PropertyInfo As PropertyInfo In EntityClass.GetType().GetProperties()

                'if property info can be read - Start
                If _PropertyInfo.CanRead Then

                    'Get Column Name 
                    ColumnName = _PropertyInfo.Name

                    'Get Column Value - Start
                    ColumnValue = clsGeneric.NullToString(
                        _PropertyInfo.GetValue(EntityClass, Nothing))
                    'Get Column Value - Stop

                    'Add Quotes if applicable - Start
                    If Not ColumnValue = Helper.NullValue And
                        Not ColumnValue = Helper.BaseUnit Then
                        ColumnValue = clsGeneric.AddQuotes(ColumnValue)
                    End If
                    'Add Quotes if applicable - Stop

                    'if first column - Start
                    If Counter = 0 Then

                        'Build Sql Statement - Start
                        TableColSql = ColumnName
                        TableColValueSql = ColumnValue
                        'Build Sql Statement - Stop

                    Else

                        'Build Sql Statement - Start
                        TableColSql &= clsGeneric.AddComma() & ColumnName
                        TableColValueSql &= clsGeneric.AddComma() & ColumnValue
                        'Build Sql Statement - Stop

                    End If
                    'if first column - Stop

                End If
                'if property info can be read - Start

                'Increment Counter
                Counter = Counter + 1

            Next
            'loop thro the Properties to the property name/value as column name/value - Start

            'Build Sql Statement - Start
            SqlStatement &= TableColSql & ")"
            SqlStatement &= " VALUES (" & TableColValueSql & ");"
            'Build Sql Statement - Stop

            Return True

        Catch ex As Exception

            'log error
            Call Helper.LogError("Entity Class: " & EntityClass.ToString() & vbCrLf & ex.Message)

            Return False

        End Try

    End Function

#End Region

End Class
