#Region "Imports "

Imports System
Imports MaxGeneric
Imports System.Data
Imports System.Data.Common
Imports System.Configuration
Imports System.Collections.Generic
Imports EntLibContrib.Data.PostgreSql
Imports Microsoft.Practices.EnterpriseLibrary.Data
Imports Microsoft.Practices.EnterpriseLibrary.Data.Sql
Imports HTS.SAS.Entities

#End Region

Public Class DatabaseProvider

#Region "Global Declarations "

    Private _PostGres As New PostGres
    Private _SqlServer As New DataBase

#End Region

#Region "Return Database Command "

    'Purpose			: To Return Db Command for given Database Type
    'Author			    : Sujith Sharatchandran - T-Melmax Sdn Bhd
    'Created Date		: 28/04/2015
    Public Function GetDbCommand(ByVal DataBaseType As Short, ByVal SqlStatement As String,
        ByVal ConnectionString As String) As DbCommand

        Try

            Select Case DataBaseType

                Case Helper.DataBaseType.SqlServer

                    Dim _SqlDataBase As SqlDatabase = New SqlDatabase(ConnectionString)
                    Return _SqlDataBase.GetSqlStringCommand(SqlStatement)

                Case Helper.DataBaseType.PostGres

                    Dim _NpgsqlDatabase As NpgsqlDatabase = New NpgsqlDatabase(ConnectionString)
                    Return _NpgsqlDatabase.GetSqlStringCommand(SqlStatement)

                Case Else

                    Return Nothing

            End Select

        Catch ex As Exception

            'log error
            Call Helper.LogError(ex.Message)

            Return Nothing

        End Try

    End Function


#End Region

#Region "Build Command Parameters "

    'Purpose			: To Reset Command Parameters due to Data Type Issues
    'Author			    : Sujith Sharatchandran - T-Melmax Sdn Bhd
    'Created Date		: 27/04/2015
    Public Function BuildCommandParams(ByVal DataBaseType As Short,
        ByVal _DbParameterCollection As DbParameterCollection,
        ByVal _DbCommand As DbCommand) As DbCommand

        'Variable Declarations
        Dim ParameterName As String = Nothing, ParameterValue As String = Nothing

        Try

            'Check Database Type - Start
            Select Case DataBaseType

                Case Helper.DataBaseType.SqlServer
                    Return _SqlServer.BuildCommandParams(
                        _DbParameterCollection, _DbCommand)

                Case Helper.DataBaseType.PostGres
                    Return _PostGres.BuildCommandParams(
                        _DbParameterCollection, _DbCommand)

                Case Else
                    Return Nothing

            End Select
            'Check Database Type - Stop

        Catch ex As Exception

            'log error
            Call Helper.LogError(ex.Message)

            Return Nothing

        End Try

    End Function

#End Region

#Region "Add In Parameter "

    'Purpose			: To Add In Parameter to DbCommand Object
    'Author			    : Sujith Sharatchandran - T-Melmax Sdn Bhd
    'Created Date		: 28/04/2015
    Public Sub AddInParameter(ByRef _DbCommand As DbCommand,
        ByVal ParamName As String, ByVal _DbType As DbType,
        ByVal ParamValue As Object)

        'Create Instances
        Dim _DbParameter As DbParameter

        Try

            'Set Parameter Properties - Start
            _DbParameter = _DbCommand.CreateParameter()
            _DbParameter.DbType = _DbType
            'Set Parameter Properties - Stop

            Select Case _DbParameter.DbType

                Case DbType.Decimal, DbType.Double
                    ParamValue = clsGeneric.NullToDecimal(ParamValue)

                Case DbType.String
                    ParamValue = clsGeneric.NullToString(ParamValue)

                Case DbType.Int16, DbType.Int32, DbType.Int64
                    ParamValue = clsGeneric.NullToInteger(ParamValue)

                Case Else
                    ParamValue = clsGeneric.NullToString(ParamValue)

            End Select

            'Set Parameter Properties - Start
            _DbParameter.Value = ParamValue
            _DbParameter.ParameterName = ParamName
            'Set Parameter Properties - Stop

            'Add Parameter to Command
            _DbCommand.Parameters.Add(_DbParameter)

        Catch ex As Exception

            'log error
            Call Helper.LogError(ex.Message)

        End Try

    End Sub

#End Region

#Region "Get IData Reader "

    'Purpose			: Execute IData Reader
    'Author			    : Sujith Sharatchandran - T-Melmax Sdn Bhd
    'Created Date		: 28/04/2015
    Public Function GetIDataReader(ByVal DataBaseType As Short,
        ByVal _DbCommand As DbCommand, ByVal ConnectionString As String,
        ByVal SqlStatement As String, ByVal _DbParameterCollection As DbParameterCollection) As DataTable

        'Create Instances - Start
        Dim _DataTable As New DataTable
        Dim _DbCommand_1 As DbCommand = Nothing
        Dim _IDataReader As IDataReader = Nothing
        'Create Instances - Stop

        Try

            Select Case DataBaseType

                Case Helper.DataBaseType.SqlServer

                    'Create Instance of Sql Data Base
                    Dim _SqlDataBase As SqlDatabase = New SqlDatabase(ConnectionString)
                    _DbCommand_1 = _SqlDataBase.GetSqlStringCommand(SqlStatement)
                    _DbCommand_1 = BuildCommandParams(DataBaseType, _DbParameterCollection, _DbCommand_1)
                    _DbCommand_1.CommandTimeout = Helper.CommandTimeOut
                    _IDataReader = _SqlDataBase.ExecuteReader(_DbCommand_1)

                Case Helper.DataBaseType.PostGres

                    Dim _NpgsqlDatabase As NpgsqlDatabase = New NpgsqlDatabase(ConnectionString)
                    _DbCommand_1 = _NpgsqlDatabase.GetSqlStringCommand(SqlStatement)
                    _DbCommand_1 = BuildCommandParams(DataBaseType, _DbParameterCollection, _DbCommand_1)
                    _DbCommand_1.CommandTimeout = Helper.CommandTimeOut
                    _IDataReader = _NpgsqlDatabase.ExecuteReader(_DbCommand_1)

                Case Else

                    Return Nothing

            End Select

            'Load Data Table
            _DataTable.Load(_IDataReader)

            Return _DataTable

        Catch ex As Exception

            'log error
            Call Helper.LogError(ex.Message)

            Return Nothing

        End Try

    End Function

#End Region

#Region "Execute Non Query "

    'Purpose			: Execute IData Reader
    'Author			    : Sujith Sharatchandran - T-Melmax Sdn Bhd
    'Created Date		: 28/04/2015
    Public Function ExecuteNonQuery(ByVal DataBaseType As Short,
        ByVal _DbCommand As DbCommand, ByVal ConnectionString As String,
        ByVal SqlStatement As String, ByVal _DbParameterCollection As DbParameterCollection) As Integer

        'Create Instances - Start
        Dim _DbCommand_1 As DbCommand = Nothing
        'Create Instances - Stop

        Try

            Select Case DataBaseType

                Case Helper.DataBaseType.SqlServer

                    'Create Instance of Sql Data Base
                    Dim _SqlDataBase As SqlDatabase = New SqlDatabase(ConnectionString)
                    _DbCommand_1 = _SqlDataBase.GetSqlStringCommand(SqlStatement)
                    _DbCommand_1 = BuildCommandParams(DataBaseType, _DbParameterCollection, _DbCommand_1)
                    _DbCommand_1.CommandTimeout = Helper.CommandTimeOut
                    Return _SqlDataBase.ExecuteNonQuery(_DbCommand_1)

                Case Helper.DataBaseType.PostGres

                    Dim _NpgsqlDatabase As NpgsqlDatabase = New NpgsqlDatabase(ConnectionString)
                    _DbCommand_1 = _NpgsqlDatabase.GetSqlStringCommand(SqlStatement)
                    _DbCommand_1 = BuildCommandParams(DataBaseType, _DbParameterCollection, _DbCommand_1)
                    _DbCommand_1.CommandTimeout = Helper.CommandTimeOut
                    Return _NpgsqlDatabase.ExecuteNonQuery(_DbCommand_1)

                Case Else

                    Return -1

            End Select

        Catch ex As Exception

            'log error
            Call Helper.LogError(ex.Message)

            Return -1

        End Try

    End Function

#End Region

#Region "Get IData Reader "

    'Purpose			: Execute IData Reader
    'Author			    : Sujith Sharatchandran - T-Melmax Sdn Bhd
    'Created Date		: 28/04/2015
    Public Function ExecuteReader(ByVal DataBaseType As Short,
        ByVal ConnectionString As String, ByVal SqlStatement As String) As DataTable

        'Create Instances - Start
        Dim _DataTable As New DataTable
        Dim _DbCommand As DbCommand = Nothing
        Dim _IDataReader As IDataReader = Nothing
        'Create Instances - Stop

        Try

            Select Case DataBaseType

                Case Helper.DataBaseType.SqlServer

                    'Create Instance of Sql Data Base
                    Dim _SqlDataBase As SqlDatabase = New SqlDatabase(ConnectionString)
                    _DbCommand = _SqlDataBase.GetSqlStringCommand(SqlStatement)
                    _DbCommand.CommandTimeout = Helper.CommandTimeOut
                    _IDataReader = _SqlDataBase.ExecuteReader(_DbCommand)

                Case Helper.DataBaseType.PostGres

                    Dim _NpgsqlDatabase As NpgsqlDatabase = New NpgsqlDatabase(ConnectionString)
                    _DbCommand = _NpgsqlDatabase.GetSqlStringCommand(SqlStatement)
                    _DbCommand.CommandTimeout = Helper.CommandTimeOut
                    _IDataReader = _NpgsqlDatabase.ExecuteReader(_DbCommand)

                Case Else

                    Return Nothing

            End Select

            'Load Data Table
            _DataTable.Load(_IDataReader)

            Return _DataTable

        Catch ex As Exception

            'log error
            Call Helper.LogError(ex.Message)

            Return Nothing

        End Try

    End Function

#End Region

#Region "Execute Sql Statement "

    'Purpose			: Execute Sql Statement
    'Author			    : Sujith Sharatchandran - T-Melmax Sdn Bhd
    'Created Date		: 28/04/2015
    Public Function ExecuteSqlStatement(ByVal DataBaseType As Short,
        ByVal ConnectionString As String, ByVal SqlStatement As String) As Integer

        'Create Instances
        Dim _DbCommand As DbCommand = Nothing

        Try

            Select Case DataBaseType

                Case Helper.DataBaseType.SqlServer

                    'Create Instance of Sql Data Base
                    Dim _SqlDataBase As SqlDatabase = New SqlDatabase(ConnectionString)
                    _DbCommand = _SqlDataBase.GetSqlStringCommand(SqlStatement)
                    _DbCommand.CommandTimeout = Helper.CommandTimeOut
                    Return _SqlDataBase.ExecuteNonQuery(_DbCommand)

                Case Helper.DataBaseType.PostGres

                    Dim _NpgsqlDatabase As NpgsqlDatabase = New NpgsqlDatabase(ConnectionString)
                    _DbCommand = _NpgsqlDatabase.GetSqlStringCommand(SqlStatement)
                    _DbCommand.CommandTimeout = Helper.CommandTimeOut
                    Return _NpgsqlDatabase.ExecuteNonQuery(_DbCommand)

                Case Else

                    Return -1

            End Select

        Catch ex As Exception

            'log error
            Call Helper.LogError(ex.Message)

            Return -1

        End Try

    End Function

#End Region

#Region "Get Data Set "

    'Purpose			: Get Data Set
    'Author			    : Sujith Sharatchandran - T-Melmax Sdn Bhd
    'Created Date		: 28/04/2015
    Public Function ExecuteDataSet(ByVal DataBaseType As Short,
        ByVal ConnectionString As String, ByVal SqlStatement As String,
        Optional ByVal IsStroredProc As Boolean = False) As DataSet

        'Create Instances - Start
        Dim _DbCommand As DbCommand = Nothing
        'Create Instances - Stop

        Try

            Select Case DataBaseType

                Case Helper.DataBaseType.SqlServer

                    'Create Instance of Sql Data Base
                    Dim _SqlDataBase As SqlDatabase = New SqlDatabase(ConnectionString)

                    'Set Db Command with Stored proc/Sql Statement - Start
                    If IsStroredProc Then
                        _DbCommand = _SqlDataBase.GetStoredProcCommand(SqlStatement)
                    Else
                        _DbCommand = _SqlDataBase.GetSqlStringCommand(SqlStatement)
                    End If
                    'Set Db Command with Stored proc/Sql Statement - Stop

                    'Set Time Out 
                    _DbCommand.CommandTimeout = Helper.CommandTimeOut

                    'Return Data Set
                    Return _SqlDataBase.ExecuteDataSet(_DbCommand)

                Case Helper.DataBaseType.PostGres

                    'Create Instance of Posgres Sql Data Base
                    Dim _NpgsqlDatabase As NpgsqlDatabase = New NpgsqlDatabase(ConnectionString)

                    'Set Db Command with Stored proc/Sql Statement - Start
                    If IsStroredProc Then
                        _DbCommand = _NpgsqlDatabase.GetStoredProcCommand(SqlStatement)
                    Else
                        _DbCommand = _NpgsqlDatabase.GetSqlStringCommand(SqlStatement)
                    End If
                    'Set Db Command with Stored proc/Sql Statement - Stop

                    'Set Time Out 
                    _DbCommand.CommandTimeout = Helper.CommandTimeOut

                    'Return Data Set
                    Return _NpgsqlDatabase.ExecuteDataSet(_DbCommand)

                Case Else

                    Return Nothing

            End Select

        Catch ex As Exception

            'log error
            Call Helper.LogError(ex.Message)

            Return Nothing

        End Try

    End Function

#End Region

#Region "Execute Scalar "

    'Purpose			: Execute Scalar
    'Author			    : Sujith Sharatchandran - T-Melmax Sdn Bhd
    'Created Date		: 28/04/2015
    Public Function ExecuteScalar(ByVal DataBaseType As Short,
        ByVal ConnectionString As String, ByVal SqlStatement As String) As Object

        'Create Instances
        Dim _DbCommand As DbCommand = Nothing

        Try

            Select Case Helper.GetDataBaseType

                Case Helper.DataBaseType.PostGres
                    Dim _NpgsqlDatabase As NpgsqlDatabase = New NpgsqlDatabase(ConnectionString)
                    _DbCommand = _NpgsqlDatabase.GetSqlStringCommand(SqlStatement)
                    _DbCommand.CommandTimeout = Helper.CommandTimeOut
                    Return _NpgsqlDatabase.ExecuteScalar(_DbCommand)

                Case Helper.DataBaseType.SqlServer
                    Dim _SqlDataBase As SqlDatabase = New SqlDatabase(ConnectionString)
                    _DbCommand = _SqlDataBase.GetSqlStringCommand(SqlStatement)
                    _DbCommand.CommandTimeout = Helper.CommandTimeOut
                    Return _SqlDataBase.ExecuteScalar(_DbCommand)

                Case Else
                    Return Nothing

            End Select

        Catch ex As Exception

            'log error
            Call Helper.LogError(ex.Message)

            Return Nothing

        End Try

    End Function

#End Region

#Region "Execute Scalar Command "

    'Purpose			: Execute Scalar Command
    'Author			    : Sujith Sharatchandran - T-Melmax Sdn Bhd
    'Created Date		: 28/04/2015
    Public Function ExecuteScalarCommand(ByVal DataBaseType As Short,
        ByVal _DbCommand As DbCommand, ByVal ConnectionString As String,
        ByVal SqlStatement As String, ByVal _DbParameterCollection As DbParameterCollection) As Object

        'Create Instances - Start
        Dim _DbCommand_1 As DbCommand = Nothing
        Dim _DbConnection As DbConnection = Nothing
        'Create Instances - Stop

        Try

            Select Case Helper.GetDataBaseType

                Case Helper.DataBaseType.PostGres
                    Dim _NpgsqlDatabase As NpgsqlDatabase = New NpgsqlDatabase(Helper.GetConnectionString)
                    _DbCommand_1 = _NpgsqlDatabase.GetSqlStringCommand(SqlStatement)
                    _DbCommand_1 = BuildCommandParams(DataBaseType, _DbParameterCollection, _DbCommand_1)
                    _DbCommand_1.CommandTimeout = Helper.CommandTimeOut
                    _DbConnection = _NpgsqlDatabase.CreateConnection()
                    _DbConnection.Open()
                    _DbCommand_1.Connection = _DbConnection
                    Return _DbCommand_1.ExecuteScalar()

                Case Helper.DataBaseType.SqlServer
                    Dim _SqlDataBase As SqlDatabase = New SqlDatabase(Helper.GetConnectionString)
                    _DbCommand_1 = _SqlDataBase.GetSqlStringCommand(SqlStatement)
                    _DbCommand_1 = BuildCommandParams(DataBaseType, _DbParameterCollection, _DbCommand_1)
                    _DbCommand_1.CommandTimeout = Helper.CommandTimeOut
                    _DbConnection = _SqlDataBase.CreateConnection()
                    _DbConnection.Open()
                    _DbCommand_1.Connection = _DbConnection
                    Return _DbCommand_1.ExecuteScalar()

                Case Else
                    Return Nothing

            End Select

        Catch ex As Exception

            'log error
            Call Helper.LogError(ex.Message)

            Return Nothing

        Finally

            If Not _DbConnection Is Nothing Then
                _DbConnection.Close()
                _DbConnection.Dispose()
            End If

        End Try

    End Function

#End Region

#Region "Execute Sql Statements "

    'Purpose			: To Execute Multiple Sql Statements
    'Author			    : Sujith Sharatchandran - T-Melmax Sdn Bhd
    'Created Date		: 20/05/2015
    Public Function ExecuteSqlStatements(ByVal DataBaseType As Short,
        ByVal ConnectionString As String, ByVal ParamArray SQLStatementArr As String()) As Boolean

        'Create Instances - Start
        Dim _DbCommand As DbCommand = Nothing
        Dim _DbConnection As DbConnection = Nothing
        Dim _DbTransaction As DbTransaction = Nothing
        'Create Instances - Stop

        'Variable Declarations
        Dim Index As Short = 0, SQLStatement As String = Nothing

        Try

            Select Case DataBaseType

                Case Helper.DataBaseType.SqlServer

                    'Create Sql Database Instance
                    Dim _SqlDataBase As SqlDatabase = New SqlDatabase(ConnectionString)

                    'Create Connection
                    _DbConnection = _SqlDataBase.CreateConnection()

                    'Open Connection
                    _DbConnection.Open()

                    'Begin Transaction
                    _DbTransaction = _DbConnection.BeginTransaction()

                    'loop thro the sql Statements arrays - start
                    For Index = 0 To SQLStatementArr.GetUpperBound(0)

                        'Get Sql Statement
                        SQLStatement = SQLStatementArr(Index)

                        'Create Command
                        _DbCommand = _SqlDataBase.DbProviderFactory.CreateCommand()

                        With _DbCommand
                            .Connection = _DbConnection
                            .Transaction = _DbTransaction
                            .CommandType = CommandType.Text
                            .CommandText = SQLStatement
                            .CommandTimeout = Helper.CommandTimeOut
                            .ExecuteNonQuery()
                        End With

                    Next
                    'loop thro the sql Statements arrays - stop

                    'Commit Transaction
                    _DbTransaction.Commit()

                    Return True

                Case Helper.DataBaseType.PostGres

                    'Create Sql Database Instance
                    Dim _NpgsqlDatabase As NpgsqlDatabase = New NpgsqlDatabase(ConnectionString)

                    'Create Connection
                    _DbConnection = _NpgsqlDatabase.CreateConnection()

                    'Open Connection
                    _DbConnection.Open()

                    'Begin Transaction
                    _DbTransaction = _DbConnection.BeginTransaction()

                    'loop thro the sql Statements arrays - start
                    For Index = 0 To SQLStatementArr.GetUpperBound(0)

                        'Get Sql Statement
                        SQLStatement = SQLStatementArr(Index)

                        'Create Command
                        _DbCommand = _NpgsqlDatabase.DbProviderFactory.CreateCommand()

                        With _DbCommand
                            .Connection = _DbConnection
                            .Transaction = _DbTransaction
                            .CommandType = CommandType.Text
                            .CommandText = SQLStatement
                            .CommandTimeout = Helper.CommandTimeOut
                            .ExecuteNonQuery()
                        End With

                    Next
                    'loop thro the sql Statements arrays - stop

                    'Commit Transaction
                    _DbTransaction.Commit()

                    Return True

                Case Else
                    Return False

            End Select

            Return False

        Catch ex As Exception

            'log error
            Call Helper.LogError(ex.Message)

            'Roll Back Transaction - Start
            If Not _DbTransaction Is Nothing Then
                _DbTransaction.Rollback()
            End If
            'Roll Back Transaction - Stop

            Return False

        Finally

            'close sql connection/transaction
            Call TerminateInstances(_DbConnection, _DbTransaction)

        End Try

    End Function

#End Region

#Region "Terminate Instances "

    'Purpose			: Destroy SQL Connection/Transaction
    'Author			    : Sujith Sharatchandran - T-Melmax Sdn Bhd
    'Created Date		: 20/05/2015
    Private Sub TerminateInstances(ByVal _DbConnection As DbConnection, _
        ByVal _DbTransaction As DbTransaction)

        If Not _DbConnection Is Nothing Then
            Call _DbConnection.Close()
            Call _DbConnection.Dispose()
            _DbConnection = Nothing
            _DbTransaction = Nothing
        End If

    End Sub

#End Region

#Region "Get Multi Sql Data Set "

    'Purpose			: Get Multi Sql Data Set
    'Author			    : Sujith Sharatchandran - T-Melmax Sdn Bhd
    'Created Date		: 28/04/2015
    Public Function ExecuteMultiSqlDataSet(ByVal DataBaseType As Short,
        ByVal ConnectionString As String, ByVal ParamArray SqlStatementArr As String()) As DataSet

        'Create Instances - Start
        Dim _DataSet As New DataSet
        Dim _DbCommand As DbCommand = Nothing
        Dim _DataTable As DataTable = Nothing
        Dim _DataTable_1 As DataTable = Nothing
        'Create Instances - Stop

        'Variable Declarations
        Dim Index As Short = 0, SQLStatement As String = Nothing

        Try

            Select Case DataBaseType

                Case Helper.DataBaseType.SqlServer

                    'Create Sql Database Instance
                    Dim _SqlDataBase As SqlDatabase = New SqlDatabase(ConnectionString)

                    'loop thro the sql Statements arrays - Start
                    For Index = 0 To SqlStatementArr.GetUpperBound(0)

                        'Get Sql Statement
                        SQLStatement = SqlStatementArr(Index)

                        'if sql statement not blank - Start
                        If Not FormHelp.IsBlank(SQLStatement) Then

                            'Exceute Sql Statement
                            _DbCommand = _SqlDataBase.GetSqlStringCommand(SQLStatement)

                            'Set Time Out
                            _DbCommand.CommandTimeout = Helper.CommandTimeOut

                            'get Data Table
                            _DataTable = _SqlDataBase.ExecuteDataSet(_DbCommand).Tables(0)

                            'Copy Data Table
                            _DataTable_1 = _DataTable.Copy()

                            'Add Table Name
                            _DataTable_1.TableName = Index

                            'Add Table to Data Set
                            _DataSet.Tables.Add(_DataTable_1)

                        End If
                        'if sql statement not blank - Stop

                    Next
                    'loop thro the sql Statements arrays - Stop

                Case Helper.DataBaseType.PostGres

                    'Create Sql Database Instance
                    Dim _NpgsqlDatabase As NpgsqlDatabase = New NpgsqlDatabase(ConnectionString)

                    'loop thro the sql Statements arrays - Start
                    For Index = 0 To SqlStatementArr.GetUpperBound(0)

                        'Get Sql Statement
                        SQLStatement = SqlStatementArr(Index)

                        'if sql statement not blank - Start
                        If Not FormHelp.IsBlank(SQLStatement) Then

                            'Exceute Sql Statement
                            _DbCommand = _NpgsqlDatabase.GetSqlStringCommand(SQLStatement)

                            'Set Time Out
                            _DbCommand.CommandTimeout = Helper.CommandTimeOut

                            'get Data Table
                            _DataTable = _NpgsqlDatabase.ExecuteDataSet(_DbCommand).Tables(0)

                            'Copy Data Table
                            _DataTable_1 = _DataTable.Copy()

                            'Add Table Name
                            _DataTable_1.TableName = Index

                            'Add Table to Data Set
                            _DataSet.Tables.Add(_DataTable_1)

                        End If
                        'if sql statement not blank - Stop

                    Next
                    'loop thro the sql Statements arrays - Stop

            End Select

            Return _DataSet

        Catch ex As Exception

            'log error
            Call Helper.LogError(ex.Message)

            Return Nothing

        End Try

    End Function

#End Region

#Region "Get Data Set "

    'Purpose			: Get Data Set
    'Author			    : Sujith Sharatchandran - T-Melmax Sdn Bhd
    'Created Date		: 28/04/2015
    Public Function ExecuteDataTable(ByVal DataBaseType As Short,
        ByVal ConnectionString As String, ByVal SqlStatement As String,
        Optional ByVal IsStroredProc As Boolean = False) As DataTable

        'Create Instances
        Dim _DbCommand As DbCommand = Nothing

        Try

            Select Case DataBaseType

                Case Helper.DataBaseType.SqlServer

                    'Create Instance of Sql Data Base
                    Dim _SqlDataBase As SqlDatabase = New SqlDatabase(ConnectionString)

                    'Set Db Command with Stored proc/Sql Statement - Start
                    If IsStroredProc Then
                        _DbCommand = _SqlDataBase.GetStoredProcCommand(SqlStatement)
                    Else
                        _DbCommand = _SqlDataBase.GetSqlStringCommand(SqlStatement)
                    End If
                    'Set Db Command with Stored proc/Sql Statement - Stop

                    'Set Time Out 
                    _DbCommand.CommandTimeout = Helper.CommandTimeOut

                    'Return Data Table
                    Return _SqlDataBase.ExecuteDataSet(_DbCommand).Tables(0)

                Case Helper.DataBaseType.PostGres

                    'Create Instance of Posgres Sql Data Base
                    Dim _NpgsqlDatabase As NpgsqlDatabase = New NpgsqlDatabase(ConnectionString)

                    'Set Db Command with Stored proc/Sql Statement - Start
                    If IsStroredProc Then
                        _DbCommand = _NpgsqlDatabase.GetStoredProcCommand(SqlStatement)
                    Else
                        _DbCommand = _NpgsqlDatabase.GetSqlStringCommand(SqlStatement)
                    End If
                    'Set Db Command with Stored proc/Sql Statement - Stop

                    'Set Time Out 
                    _DbCommand.CommandTimeout = Helper.CommandTimeOut

                    'Return Data Table
                    Return _NpgsqlDatabase.ExecuteDataSet(_DbCommand).Tables(0)

                Case Else

                    Return Nothing

            End Select

        Catch ex As Exception

            'log error
            Call Helper.LogError(ex.Message)

            Return Nothing

        End Try

    End Function

#End Region

#Region "Stored Procedure"
    'added by Hafiz @ 26/10/2016

    'Public Function SimpleAdd(ByVal val As Integer) As DataSet

    '    Dim cmd As DbCommand = Nothing

    '    Dim _NpgsqlDatabase As NpgsqlDatabase = New NpgsqlDatabase(Helper.NpgSqlConnectionString())
    '    cmd = _NpgsqlDatabase.GetStoredProcCommand("simpleadd")
    '    _NpgsqlDatabase.AddInParameter(cmd, "val", DbType.Int16, val)

    '    cmd.CommandTimeout = Helper.CommandTimeOut

    '    Return _NpgsqlDatabase.ExecuteDataSet(cmd)

    'End Function

    Public Function GetStudentDetails(ByVal loen As AccountsEn, ByVal programid As String, ByVal safc_code As String, ByVal poststatus As String, ByVal currentsemester As String,
                                       ByVal semester As String, ByVal creditref As String, ByVal recstatus As String, ByVal batch As String) As DataTable

        'Create Instances - Start
        Dim _DataTable As New DataTable
        Dim _DbCommand As DbCommand = Nothing
        Dim _IDataReader As IDataReader = Nothing
        'Create Instances - Stop

        Try
            Dim _NpgsqlDatabase As NpgsqlDatabase = New NpgsqlDatabase(Helper.NpgSqlConnectionString())
            _DbCommand = _NpgsqlDatabase.GetStoredProcCommand("student_details")
            _NpgsqlDatabase.AddInParameter(_DbCommand, "Category", DbType.String, loen.Category)
            _NpgsqlDatabase.AddInParameter(_DbCommand, "TransType", DbType.String, loen.TransType)
            _NpgsqlDatabase.AddInParameter(_DbCommand, "SubType", DbType.String, loen.SubType)
            _NpgsqlDatabase.AddInParameter(_DbCommand, "PostStatus", DbType.String, loen.PostStatus)
            _NpgsqlDatabase.AddInParameter(_DbCommand, "TransStatus", DbType.String, loen.TransStatus)
            _NpgsqlDatabase.AddInParameter(_DbCommand, "TransDate", DbType.DateTime, loen.TransDate)
            _NpgsqlDatabase.AddInParameter(_DbCommand, "DueDate", DbType.DateTime, loen.DueDate)
            _NpgsqlDatabase.AddInParameter(_DbCommand, "BatchDate", DbType.DateTime, loen.BatchDate)
            _NpgsqlDatabase.AddInParameter(_DbCommand, "ChequeDate", DbType.DateTime, loen.ChequeDate)
            _NpgsqlDatabase.AddInParameter(_DbCommand, "PostedDateTime", DbType.DateTime, loen.PostedDateTime)
            _NpgsqlDatabase.AddInParameter(_DbCommand, "UpdatedTime", DbType.DateTime, loen.UpdatedTime)
            _NpgsqlDatabase.AddInParameter(_DbCommand, "CreatedDateTime", DbType.DateTime, loen.CreatedDateTime)
            _NpgsqlDatabase.AddInParameter(_DbCommand, "Description", DbType.String, loen.Description)
            _NpgsqlDatabase.AddInParameter(_DbCommand, "CreatedBy", DbType.String, loen.CreatedBy)
            _NpgsqlDatabase.AddInParameter(_DbCommand, "UpdatedBy", DbType.String, loen.UpdatedBy)
            _NpgsqlDatabase.AddInParameter(_DbCommand, "programid", DbType.String, programid)
            _NpgsqlDatabase.AddInParameter(_DbCommand, "safc_code", DbType.String, safc_code)
            _NpgsqlDatabase.AddInParameter(_DbCommand, "poststatus", DbType.String, poststatus)
            _NpgsqlDatabase.AddInParameter(_DbCommand, "currentsemester", DbType.String, currentsemester)
            _NpgsqlDatabase.AddInParameter(_DbCommand, "semester", DbType.String, semester)
            _NpgsqlDatabase.AddInParameter(_DbCommand, "creditref", DbType.String, creditref)
            _NpgsqlDatabase.AddInParameter(_DbCommand, "recstatus", DbType.String, recstatus)
            _NpgsqlDatabase.AddInParameter(_DbCommand, "Batch", DbType.String, batch)

            _DbCommand.CommandTimeout = Helper.CommandTimeOut

            _IDataReader = _NpgsqlDatabase.ExecuteReader(_DbCommand)
            _DataTable.Load(_IDataReader)

            Return _DataTable

        Catch ex As Exception

            'log error
            Call Helper.LogError(ex.Message)

            Return Nothing
        End Try

    End Function

#End Region

#Region "Move Function to PLPGSQL"
    'added by Hafiz @ 21/11/2016 

    Public Function Update_OutstandingAmount(ByVal BatchCode As String) As DataTable

        Dim _DataTable As New DataTable
        Dim _DbCommand As DbCommand = Nothing
        Dim _IDataReader As IDataReader = Nothing

        Try
            Dim _NpgsqlDatabase As NpgsqlDatabase = New NpgsqlDatabase(Helper.NpgSqlConnectionString())
            _DbCommand = _NpgsqlDatabase.GetStoredProcCommand("Update_OutstandingAmount")
            _NpgsqlDatabase.AddInParameter(_DbCommand, "BatchCode", DbType.String, BatchCode)
            _DbCommand.CommandTimeout = Helper.CommandTimeOut

            _IDataReader = _NpgsqlDatabase.ExecuteReader(_DbCommand)
            _DataTable.Load(_IDataReader)

            Return _DataTable

        Catch ex As Exception

            'log error
            Call Helper.LogError(ex.Message)

            Return Nothing

        End Try

    End Function


#End Region


End Class
