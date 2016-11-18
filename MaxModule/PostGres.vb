#Region "Imports "

Imports System
Imports Npgsql
Imports MaxGeneric
Imports System.Data
Imports System.Data.Common
Imports System.Configuration
Imports System.Collections.Generic
Imports System.Text.RegularExpressions

#End Region

Public Class PostGres

#Region "Config Properties "

    Private ReadOnly Property SqlConnectionTimeOut() As Short
        Get
            If clsGeneric.NullToShort(ConfigurationManager.AppSettings("TIMEOUT")) = 0 Then
                Return 30
            Else
                clsGeneric.NullToShort(ConfigurationManager.AppSettings("TIMEOUT"))
            End If
        End Get
    End Property

#End Region

#Region "Strip HTML Tags "

    'Purpose			: To Strip HTML Tags
    'Author			    : Sujith Sharatchandran - T-Melmax Sdn Bhd
    'Created Date		: 13/10/2011
    Public Function StripHtmlTags(ByVal TextToStrip As String) As String

        Try

            Return Regex.Replace(TextToStrip, "<(?<tag>\w*)>(?<text>.*)</\k<tag>>", String.Empty)

        Catch ex As Exception

            'log error
            Call clsGeneric.prcLogError(ex.Message, "MaxModule.PostGres - StripHtmlTags")

            Return String.Empty

        End Try

    End Function

#End Region

#Region "Get Database Connection String "

    'Purpose			: To Get the connection String for the given Database
    'Author			    : Sujith Sharatchandran - T-Melmax Sdn Bhd
    'Created Date		: 15/04/2015
    Private Function GetConnectionString() As String
        Return Helper.NpgSqlConnectionString()
    End Function

#End Region

#Region "Get SQL Connection "

    'Purpose			: Get Post Gres Connection
    'Author			    : Sujith Sharatchandran - T-Melmax Sdn Bhd
    'Created Date		: 16/04/2015
    Private Function GetSqlConnection() As NpgsqlConnection

        'Create Instances
        Dim _NpgsqlConnection As NpgsqlConnection = Nothing

        Try

            'Create Post Gres Connection
            _NpgsqlConnection = New NpgsqlConnection(GetConnectionString())

            'Open Post Gres Connection
            Call _NpgsqlConnection.Open()

            Return _NpgsqlConnection

        Finally

            'force Garbage Collection
            Call GC.Collect(0)

        End Try

    End Function

#End Region

#Region "Terminate SQL Connection "

    'Purpose			: Destroy Sql Connection
    'Author			    : Sujith Sharatchandran - T-Melmax Sdn Bhd
    'Created Date		: 09/03/2010
    Private Sub TerminateSqlInstances(ByVal _NpgsqlConnection As NpgsqlConnection, _
        ByVal _NpgsqlTransaction As NpgsqlTransaction)

        If Not _NpgsqlConnection Is Nothing Then
            Call _NpgsqlConnection.Close()
            Call _NpgsqlConnection.Dispose()
            _NpgsqlConnection = Nothing
            _NpgsqlTransaction = Nothing
        End If

    End Sub

#End Region

#Region "Get Data Reader "

    'Purpose			: To Return Data Reader for given Post Gres SQL Statement
    'Author			    : Sujith Sharatchandran - T-Melmax Sdn Bhd
    'Created Date		: 15/04/2015
    Public Function GetDataReader(ByVal SqlStatement As String) As DataTable

        'Create Instances - Start
        Dim _DataTable As New DataTable
        Dim _NpgsqlConnection As NpgsqlConnection = Nothing
        Dim _NpgsqlDataAdapter As NpgsqlDataAdapter = Nothing
        'Create Instances - Stop

        Try

            'Open Post Gres connection
            _NpgsqlConnection = GetSqlConnection()

            'Execute Post Gres adapter - Start
            _NpgsqlDataAdapter = New NpgsqlDataAdapter(
                SqlStatement, _NpgsqlConnection)
            'Execute Post Gres adapter - Stop

            'Fill Data Table
            _NpgsqlDataAdapter.Fill(_DataTable)

            Return _DataTable

        Catch ex As Exception

            'log error
            Call clsGeneric.prcLogError(ex.Message, "MaxModule.PostGres.GetDataReader")

            Return Nothing

        Finally

            'Terminate Sql Instances
            Call TerminateSqlInstances(_NpgsqlConnection, Nothing)

        End Try

    End Function

#End Region

#Region "Excute Non Query "

    'Purpose			: Execute Non Query
    'Author			    : Sujith Sharatchandran - T-Melmax Sdn Bhd
    'Created Date		: 20/04/2015
    Public Function ExecuteNonQuery(ByVal SqlColumns As String,
        ByVal ParamArray SqlValues As String()) As Integer

        'Create Instances - Start
        Dim _NpgsqlCommand As New NpgsqlCommand
        Dim _NpgsqlConnection As NpgsqlConnection = Nothing
        Dim _NpgsqlTransaction As NpgsqlTransaction = Nothing
        'Create Instances - Stop

        'Variable Declarations - Start
        Dim Index As Short = 0, Counter As Short = 0
        Dim SqlStatement As String = Nothing, Result As Integer = 0
        'Variable Declarations - Stop

        Try

            'Sql Statement Start
            SqlStatement = SqlColumns & " VALUES ("

            'loop thro the column values arrays - Start
            For Index = 0 To SqlValues.GetUpperBound(0)

                'Check If first column value - Start
                If Counter = 0 Then
                    'Continue Build Sql Statement
                    SqlStatement &= clsGeneric.AddQuotes(SqlValues(Index))
                Else
                    'Continue Build Sql Statement
                    SqlStatement &= clsGeneric.AddComma() & clsGeneric.AddQuotes(SqlValues(Index))
                End If
                'Check If first column value - Stop

                'Increment Counter
                Counter = Counter + 1

            Next
            'loop thro the column values arrays - Stop

            'Sql Statement End
            SqlStatement = SqlStatement & ")"

            'Open Post Gres connection
            _NpgsqlConnection = GetSqlConnection()

            'begin transaction
            _NpgsqlTransaction = _NpgsqlConnection.BeginTransaction()

            'Command Values - Start
            With _NpgsqlCommand

                .Connection = _NpgsqlConnection
                .Transaction = _NpgsqlTransaction
                .CommandType = CommandType.Text
                .CommandText = SqlStatement

            End With
            'Command Values - Stop

            'Execute Sql Statement
            Result = _NpgsqlCommand.ExecuteNonQuery()

            'commit transaction
            Call _NpgsqlTransaction.Commit()

            Return Result

        Catch ex As Exception

            'log error
            Call clsGeneric.prcLogError(ex.Message, "MaxModule.PostGres.ExecuteNonQuery")

            Return -1

        Finally

            'Terminate Sql Instances
            Call TerminateSqlInstances(_NpgsqlConnection, Nothing)

        End Try

    End Function

#End Region

#Region "Excute Sql Statement "

    'Purpose			: Execute Non Query
    'Author			    : Sujith Sharatchandran - T-Melmax Sdn Bhd
    'Created Date		: 20/04/2015
    Public Function ExecuteSqlStatement(ByVal SqlStatement As String) As Integer

        'Create Instances - Start
        Dim _NpgsqlCommand As New NpgsqlCommand
        Dim _NpgsqlConnection As NpgsqlConnection = Nothing
        Dim _NpgsqlTransaction As NpgsqlTransaction = Nothing
        'Create Instances - Stop

        'Variable Declarations
        Dim Result As Integer = 0

        Try

            'Open Post Gres connection
            _NpgsqlConnection = GetSqlConnection()

            'begin transaction
            _NpgsqlTransaction = _NpgsqlConnection.BeginTransaction()

            'Command Values - Start
            With _NpgsqlCommand

                .Connection = _NpgsqlConnection
                .Transaction = _NpgsqlTransaction
                .CommandType = CommandType.Text
                .CommandText = SqlStatement

            End With
            'Command Values - Stop

            'Execute Sql Statement
            Result = _NpgsqlCommand.ExecuteNonQuery()

            'commit transaction
            Call _NpgsqlTransaction.Commit()

            Return Result

        Catch ex As Exception

            'log error
            Call clsGeneric.prcLogError(ex.Message, "MaxModule.PostGres.ExecuteSqlStatement")

            Return -1

        Finally

            'Terminate Sql Instances
            Call TerminateSqlInstances(_NpgsqlConnection, Nothing)

        End Try

    End Function

#End Region

#Region "Excute Sql Statement 1"

    'Purpose			: Execute Non Query
    'Author			    : Sujith Sharatchandran - T-Melmax Sdn Bhd
    'Created Date		: 20/04/2015
    Public Function ExecuteSqlStatement1(ByVal SqlStatement As String) As Integer

        'Create Instances - Start
        Dim _NpgsqlCommand As New NpgsqlCommand
        Dim _NpgsqlConnection As NpgsqlConnection = Nothing
        Dim _NpgsqlTransaction As NpgsqlTransaction = Nothing
        'Create Instances - Stop

        'Variable Declarations
        Dim Result As Integer = 0

        Try

            'Open Post Gres connection
            _NpgsqlConnection = GetSqlConnection()

            'begin transaction
            _NpgsqlTransaction = _NpgsqlConnection.BeginTransaction()

            'Command Values - Start
            With _NpgsqlCommand

                .Connection = _NpgsqlConnection
                .Transaction = _NpgsqlTransaction
                .CommandType = CommandType.Text
                .CommandText = SqlStatement

            End With
            'Command Values - Stop

            'Execute Sql Statement
            Dim rd As IDataReader = Nothing


            rd = _NpgsqlCommand.ExecuteReader()
            While rd.Read()
                Result = rd.Item(0)
            End While
            'commit transaction
            'Call _NpgsqlTransaction.Commit()

            Return Result

        Catch ex As Exception

            'log error
            Call clsGeneric.prcLogError(ex.Message, "MaxModule.PostGres.ExecuteSqlStatement")

            Return -1

        Finally

            'Terminate Sql Instances
            Call TerminateSqlInstances(_NpgsqlConnection, Nothing)

        End Try

    End Function

#End Region

#Region "Get Data Set "

    'Purpose			: To Return Data Reader for given Post Gres SQL Statement
    'Author			    : Sujith Sharatchandran - T-Melmax Sdn Bhd
    'Created Date		: 15/04/2015
    Public Function GetDataSet(ByVal SqlStatement As String) As DataSet

        'Create Instances - Start
        Dim _DataSet As New DataSet
        Dim _DataTable As New DataTable
        Dim _NpgsqlConnection As NpgsqlConnection = Nothing
        Dim _NpgsqlDataAdapter As NpgsqlDataAdapter = Nothing
        'Create Instances - Stop

        Try

            'Open Post Gres connection
            _NpgsqlConnection = GetSqlConnection()

            'Execute Post Gres adapter - Start
            _NpgsqlDataAdapter = New NpgsqlDataAdapter(
                SqlStatement, _NpgsqlConnection)
            'Execute Post Gres adapter - Stop

            'Fill Data Table
            _NpgsqlDataAdapter.Fill(_DataTable)

            'Add To Data Set
            _DataSet.Tables.Add(_DataTable)

            Return _DataSet

        Catch ex As Exception

            'log error
            Call clsGeneric.prcLogError(ex.Message, "MaxModule.PostGres.GetDataSet")

            Return Nothing

        Finally

            'Terminate Sql Instances
            Call TerminateSqlInstances(_NpgsqlConnection, Nothing)

        End Try

    End Function

#End Region

#Region "Reset Command Parameters "

    'Purpose			: To Reset Command Parameters due to Data Type Issues
    'Author			    : Sujith Sharatchandran - T-Melmax Sdn Bhd
    'Created Date		: 27/04/2015
    Public Function BuildCommandParams(ByVal _DbParameterCollection As DbParameterCollection,
       ByVal _DbCommand As DbCommand) As DbCommand

        'Create Instances - Start
        Dim _DbParameter As DbParameter = Nothing
        Dim _NpgsqlParameter As NpgsqlParameter = Nothing
        'Create Instances - Stop

        'Variable Declarations
        Dim ParameterName As String = Nothing, ParameterValue As String = Nothing

        Try

            'Loop thro the Parameter Collections - Start
            For Each _DbParameter In _DbParameterCollection

                'Get Data - Start
                ParameterValue = _DbParameter.Value
                ParameterName = _DbParameter.ParameterName
                ParameterName = ParameterName.Replace("@", "")
                'Get Data - Stop

                'Create Parameter for command
                _NpgsqlParameter = New NpgsqlParameter()

                'Set Param Name
                _NpgsqlParameter.ParameterName = ParameterName

                'Set Param Value
                _NpgsqlParameter.Value = clsGeneric.NullToString(ParameterValue)

                'Add Parameter
                _DbCommand.Parameters.Add(_NpgsqlParameter)

            Next
            'Loop thro the Parameter Collections - Stop\

            Return _DbCommand

        Catch ex As Exception

            'log error
            Call Helper.LogError(ex.Message)

            Return Nothing

        End Try

    End Function

#End Region

End Class
