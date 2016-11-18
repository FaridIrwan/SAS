#Region "Imports "

Imports System
Imports MaxGeneric
Imports System.Reflection
Imports System.Diagnostics
Imports System.Configuration

#End Region

Public Class Helper

#Region "Constants "

    Public Const Manual As String = "MAN"
    Public Const Operators As String = "M"
    Public Const BaseUnit As String = "1.0"
    Public Const NullValue As String = "NULL"
    Public Const JournelStatus As String = "R"
    Public Const CurrencyCode As String = "MYR"
    Public Const CurrencyRate As String = "1.0000"
    Public Const InputParam As String = "@in_Param_"

#End Region

#Region "Column Constants "

    Public Const LineNoCol As String = "Row_No"
    Public Const GlCodeCol As String = "GL_Code"
    Public Const TransIdCol As String = "Trans_Id"
    Public Const BankCodeCol As String = "Bank_Code"
    Public Const BatchCodeCol As String = "Batch_Code"
    Public Const PostedByCol As String = "Posted_By"
    Public Const TransDateCol As String = "Trans_Date"
    Public Const TransCodeCol As String = "Trans_Code"
    Public Const MatricNoCol As String = "Matric_No"
    Public Const PayeeNameCol As String = "Payee_Name"
    Public Const BatchDateCol As String = "Batch_Date"
    Public Const PostedDateCol As String = "Posted_Date"
    Public Const BatchTotalCol As String = "Batch_Total"
    Public Const BatchAmountCol As String = "Batch_Amount"
    Public Const PaymentModeCol As String = "Payment_Mode"
    Public Const TransAmountCol As String = "Trans_Amount"
    Public Const Reference1Col As String = "Reference_1"
    Public Const CategoryTypeCol As String = "Category_Type"
    Public Const CategorySubTypeCol As String = "Sub_Type"
    Public Const TransactionTypeCol As String = "Trans_Type"
    Public Const TransactionCodeCol As String = "Trans_Code"
    Public Const TransactionDateCol As String = "Trans_Date"

#End Region

#Region "SQL Properties "

    Public ReadOnly Property SQLErrorLog() As String
        Get
            Return ConfigurationManager.AppSettings("ERROR_LOG")
        End Get
    End Property

#End Region

#Region "Config Properties "

    Private Shared ReadOnly Property ConnectionStringEncrypted() As Short
        Get
            Return clsGeneric.NullToShort(ConfigurationManager.AppSettings("KEY"))
        End Get
    End Property

    Public Shared ReadOnly Property NpgSqlConnectionString() As String
        Get
            If ConnectionStringEncrypted = EnumHelp.YesNo.Yes Then
                Return clsGeneric.Cryption(ConfigurationManager.AppSettings("PG_CONNNECTION_STR"))
            Else
                Return ConfigurationManager.AppSettings("PG_CONNNECTION_STR")
            End If
        End Get
    End Property

    Public Shared ReadOnly Property SqlConnectionString() As String
        Get
            If ConnectionStringEncrypted = EnumHelp.YesNo.Yes Then
                Return clsGeneric.Cryption(ConfigurationManager.AppSettings("SQL_CONNNECTION_STR"))
            Else
                Return ConfigurationManager.AppSettings("SQL_CONNNECTION_STR")
            End If
        End Get
    End Property

    Public Shared ReadOnly Property GetDataBaseType() As Short
        Get
            Return clsGeneric.NullToShort(ConfigurationManager.AppSettings("DATABASE_TYPE"))
        End Get
    End Property

    Public Shared ReadOnly Property GetFinancialsDataBaseType() As Short
        Get
            Return clsGeneric.NullToShort(ConfigurationManager.AppSettings("CF_DATABASE_TYPE"))
        End Get
    End Property

    Public Shared ReadOnly Property FinancialsConnectionString() As String
        Get
            If ConnectionStringEncrypted = EnumHelp.YesNo.Yes Then
                Return clsGeneric.Cryption(ConfigurationManager.AppSettings("CF_CONNNECTION_STR"))
            Else
                Return ConfigurationManager.AppSettings("CF_CONNNECTION_STR")
            End If
        End Get
    End Property

    Public Shared ReadOnly Property CommandTimeOut() As Short
        Get
            Return clsGeneric.NullToShort(ConfigurationManager.AppSettings("COMMAND_TIMEOUT"))
        End Get
    End Property

#End Region

#Region "Enumerators "

    Public Enum DataBaseType
        None = 0
        SqlServer = 1
        PostGres = 2
    End Enum

#End Region

#Region "Get Connection String "

    'Purpose			: To Get the Connection String
    'Author			    : Sujith Sharatchandran - T-Melmax Sdn Bhd
    'Created Date		: 19/05/2015
    Public Shared Function GetConnectionString() As String

        Select Case GetDataBaseType

            Case DataBaseType.PostGres
                Return NpgSqlConnectionString
            Case DataBaseType.SqlServer
                Return SqlConnectionString
            Case Else
                Return String.Empty

        End Select

    End Function

#End Region

#Region "Log Error "

    'Purpose			: To Log Error
    'Author			    : Sujith Sharatchandran - T-Melmax Sdn Bhd
    'Created Date		: 21/05/2015
    Public Shared Sub LogError(ByVal ErrorMessage As String)

        'Create Instances - Start
        Dim _StackTrace As New StackTrace()
        Dim _StackFrame As StackFrame = Nothing
        Dim _MethodBase As MethodBase = Nothing
        'Create Instances - Stop

        'Variable Declarations - Start
        Dim MethodChild As String = Nothing, MethodChild_1 As String = Nothing
        Dim ErrorDetails As String = Nothing, MethodParent As String = Nothing
        'Variable Declarations - Stop

        Try

            If Not ErrorMessage = "Thread was being aborted." Then

                'Get Parent Method - Start
                _StackFrame = _StackTrace.GetFrame(3)
                _MethodBase = _StackFrame.GetMethod()
                MethodParent = _MethodBase.Name
                'Get Parent Method - Stop

                'Get Child Method - Start
                _StackFrame = _StackTrace.GetFrame(2)
                _MethodBase = _StackFrame.GetMethod()
                MethodChild = _MethodBase.Name
                'Get Child Method - Stop

                'Get Sub Child Method - Start
                _StackFrame = _StackTrace.GetFrame(1)
                _MethodBase = _StackFrame.GetMethod()
                MethodChild_1 = _MethodBase.Name
                'Get Sub Child Method - Stop

                'Build Error Details - Start
                ErrorDetails = Environment.NewLine()
                ErrorDetails &= "========================================================================================" & vbCrLf
                ErrorDetails &= "Message Details : " & ErrorMessage & vbCrLf
                ErrorDetails &= "Message Date Time : " & Now.ToString() & vbCrLf
                ErrorDetails &= "Method Parent : " & MethodParent & vbCrLf
                ErrorDetails &= "Method Child : " & MethodChild & vbCrLf
                ErrorDetails &= "Method Sub Child : " & MethodChild_1 & vbCrLf
                ErrorDetails &= "========================================================================================"
                'Build Error Details - Stop

                'Log Error
                Call clsGeneric.prcLogError(ErrorDetails, String.Empty)

            End If

        Catch ex As Exception

            'log error
            Call clsGeneric.prcLogError(ex.Message, "MaxModule.Helper - LogError")

        End Try

    End Sub

#End Region

#Region "Date Conversion "

    'Purpose			: To Date Conversion
    'Author			    : Sujith Sharatchandran - T-Melmax Sdn Bhd
    'Created Date		: 16/06/2015
    Public Shared Function DateConversion(_DateTime As DateTime) As String
        Return String.Format("{0:u}", _DateTime)
    End Function

#End Region

End Class
