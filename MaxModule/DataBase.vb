'****************************************************************************************************
'Class Name     : DataBase
'ProgId         : MaxModule.DataBase
'Purpose        : All Data Base Insert/Update Functions
'Author         : Sujith Sharatchandran - T-Melmax Sdn Bhd
'Created        : 18/02/2006
'*****************************************************************************************************

#Region "Imports "

Imports System
Imports MaxGeneric
Imports System.Data
Imports System.Data.Common
Imports System.Data.SqlClient
Imports System.Configuration
Imports System.Text.RegularExpressions

#End Region

Public Class DataBase

#Region "Global Declarations "

    Private _Helper As New MaxModule.Helper

#End Region

#Region "Properties "

    Private ReadOnly Property GetExecuteSQL() As String
        Get
            Return ConfigurationManager.AppSettings("EXEC_SQL")
        End Get
    End Property

#End Region

#Region "Database Connection Properties "

    Private ReadOnly Property SERVER() As String
        Get
            Return ConfigurationManager.AppSettings("SERVER")
        End Get
    End Property

    Private ReadOnly Property DATABASE() As String
        Get
            Return ConfigurationManager.AppSettings("DATABASE")
        End Get
    End Property

    Private ReadOnly Property USERNAME() As String
        Get
            Return ConfigurationManager.AppSettings("USERNAME")
        End Get
    End Property

    Private ReadOnly Property PASSWORD() As String
        Get
            Return ConfigurationManager.AppSettings("PASSWORD")
        End Get
    End Property

    Private ReadOnly Property SqlConnectionTimeOut() As Short
        Get
            If clsGeneric.NullToShort(ConfigurationManager.AppSettings("TIMEOUT")) = 0 Then
                Return 30
            Else
                clsGeneric.NullToShort(ConfigurationManager.AppSettings("TIMEOUT"))
            End If
        End Get
    End Property

    Private ReadOnly Property ConnectionStringEncrypted() As Short
        Get
            Return clsGeneric.NullToShort(ConfigurationManager.AppSettings("KEY"))
        End Get
    End Property

#End Region

#Region "Get SQL Connection "

    'Purpose			: Get Sql Connection
    'Author			    : Sujith Sharatchandran - T-Melmax Sdn Bhd
    'Created Date		: 09/03/2010
    Private Function GetSqlConnection() As SqlConnection

        'Create Instances
        Dim _SqlConnection As SqlConnection = Nothing

        'Variable Declarations
        Dim SqlConenctionString As String = Nothing

        Try

            If ConnectionStringEncrypted = EnumHelp.YesNo.No Then
                SqlConenctionString = "SERVER=" & SERVER & ";DATABASE=" & DATABASE & _
                    ";UID=" & USERNAME & ";PWD=" & PASSWORD
            ElseIf ConnectionStringEncrypted = EnumHelp.YesNo.Yes Then
                SqlConenctionString = "SERVER=" & clsGeneric.Cryption(SERVER) & ";DATABASE=" _
                    & clsGeneric.Cryption(DATABASE) & ";UID=" & clsGeneric.Cryption(USERNAME) & ";PWD=" _
                        & clsGeneric.Cryption(PASSWORD)
            End If

            'Create Sql Connection
            _SqlConnection = New SqlConnection(SqlConenctionString)

            'Open Sql Connection
            Call _SqlConnection.Open()

            Return _SqlConnection

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
    Private Sub TerminateSqlInstances(ByVal _SqlConnection As SqlConnection, _
        ByVal _SqlTransaction As SqlTransaction)

        Try

            If Not _SqlConnection Is Nothing Then
                Call _SqlConnection.Close()
                Call _SqlConnection.Dispose()
                _SqlConnection = Nothing
                _SqlTransaction = Nothing
            End If

        Finally

            'force Garbage Collection
            Call GC.Collect(0)

        End Try

    End Sub

#End Region

#Region "Clear SQL Connection Pool "

    'Purpose			: Clear Connection Pool
    'Author			    : Sujith Sharatchandran - T-Melmax Sdn Bhd
    'Created Date		: 09/03/2010
    Private Sub ClearConnectionPool(ByVal _SqlConnection As SqlConnection)

        Try

            If Not _SqlConnection Is Nothing Then
                SqlConnection.ClearPool(_SqlConnection)
            End If

        Finally

            'force Garbage Collection
            Call GC.Collect(0)

        End Try

    End Sub

#End Region

#Region "Execute Scalar "

    'Purpose			: To Insert/Update with scalar value
    'Author			    : Sujith Sharatchandran - T-Melmax Sdn Bhd
    'Created Date		: 18/02/2006
    Public Function ExecuteScalar(ByVal DataToSave As DataTable, _
                ByVal SQLStatement As String) As Integer

        'Create Instances - Start
        Dim cmdDataStore As New SqlCommand
        Dim _SqlConnection As SqlConnection = Nothing
        Dim _SqlTransaction As SqlTransaction = Nothing
        'Create Instances - Stop

        'Variable declarations
        Dim intScalarId As Integer = 0

        Try

            'Get sql connection
            _SqlConnection = GetSqlConnection()

            'begin transaction
            _SqlTransaction = _SqlConnection.BeginTransaction()

            With cmdDataStore

                .Connection = _SqlConnection
                .Transaction = _SqlTransaction
                .CommandType = CommandType.StoredProcedure
                .CommandTimeout = SqlConnectionTimeOut
                .CommandText = SQLStatement

                'Add Command Parameters
                Call DataHelp.DataToParam(DataToSave, cmdDataStore, 0)

                'get scalar id
                intScalarId = .ExecuteScalar()

            End With

            'commit transaction
            Call _SqlTransaction.Commit()

            Return intScalarId

        Catch ex As Exception

            'roll back transaction
            Call _SqlTransaction.Rollback()

            'log error
            Call clsGeneric.prcLogError(ex.Message, "MaxModule.DataBase - ExecuteScalar")

            Return 0

        Finally

            'close sql connection
            Call TerminateSqlInstances(_SqlConnection, _SqlTransaction)

            'destroy instance of command object
            cmdDataStore = Nothing

            'force garbage collection
            Call GC.Collect(0)

        End Try

    End Function

#End Region

#Region "Execute Non Query "

    'Purpose			: To Insert/Update
    'Author			    : Sujith Sharatchandran - T-Melmax Sdn Bhd
    'Created Date		: 18/02/2006
    Public Function ExecuteNonQuery(ByVal DataToSave As DataTable, _
                ByVal SQLStatement As String) As Boolean

        'Create Instances - Start
        Dim drDataStore As DataRow = Nothing
        Dim cmdDataStore As SqlCommand = Nothing
        Dim _SqlConnection As SqlConnection = Nothing
        Dim _SqlTransaction As SqlTransaction = Nothing
        'Create Instances - Stop

        'Variable declarations
        Dim intCounter As Short = 0

        Try

            'initialise counter
            intCounter = 0

            'Open sql connection
            _SqlConnection = GetSqlConnection()

            'begin transaction
            _SqlTransaction = _SqlConnection.BeginTransaction()

            'loop thro the data table - start
            For Each drDataStore In DataToSave.Rows

                cmdDataStore = New SqlCommand
                With cmdDataStore

                    .Connection = _SqlConnection
                    .Transaction = _SqlTransaction
                    .CommandType = CommandType.StoredProcedure
                    .CommandTimeout = SqlConnectionTimeOut
                    .CommandText = StripHtmlTags(SQLStatement)

                    'Add Command Parameters
                    Call DataHelp.DataToParam(DataToSave, cmdDataStore, intCounter)

                    .ExecuteNonQuery()

                End With

                'increment counter
                intCounter = intCounter + 1

            Next
            'loop thro the data table - stop

            'commit transaction
            Call _SqlTransaction.Commit()

            Return True

        Catch ex As Exception

            'roll back transaction
            Call _SqlTransaction.Rollback()

            'log error
            Call clsGeneric.prcLogError(ex.Message, "MaxModule.DataBase - ExecuteNonQuery")

            Return False

        Finally

            'close sql connection
            Call TerminateSqlInstances(_SqlConnection, _SqlTransaction)

            'create instance of data row
            drDataStore = Nothing

            'destroy instance of command object
            cmdDataStore = Nothing

            'force garbage collection
            Call GC.Collect(0)

        End Try

    End Function

#End Region

#Region "Execute SQL Statement "

    'Purpose			: To Delete Record 
    'Author			    : Sujith Sharatchandran - T-Melmax Sdn Bhd
    'Created Date		: 18/02/2006
    Public Function ExecuteSQL(ByVal ParamArray SQLStatement As String()) As Boolean

        'create instances - Start
        Dim cmdExecuteData As SqlCommand = Nothing
        Dim _SqlConnection As SqlConnection = Nothing
        Dim _SqlTransaction As SqlTransaction = Nothing
        'create instances - Stop

        'variable
        Dim Index As Short = 0

        Try

            'open sql connection
            _SqlConnection = GetSqlConnection()

            'begin transaction
            _SqlTransaction = _SqlConnection.BeginTransaction()

            'loop thro the sql Statements arrays - start
            For Index = 0 To SQLStatement.GetUpperBound(0)

                'if Not blank - Start
                If Not FormHelp.IsBlank(clsGeneric.NullToString(SQLStatement(Index))) Then

                    'execute sql command
                    cmdExecuteData = New SqlCommand
                    With cmdExecuteData
                        .Connection = _SqlConnection
                        .Transaction = _SqlTransaction
                        .CommandType = CommandType.Text
                        .CommandTimeout = SqlConnectionTimeOut
                        .CommandText = clsGeneric.NullToString(SQLStatement(Index))
                        .ExecuteNonQuery()
                    End With

                End If
                'if Not blank - Stop

            Next
            'loop thro the sql Statements arrays - stop

            'commit transaction
            Call _SqlTransaction.Commit()

            Return True

        Catch ex As Exception

            'roll back transaction
            Call _SqlTransaction.Rollback()

            'log error
            Call clsGeneric.prcLogError(ex.Message, "MaxModule.DataBase - ExecuteSQL")

            Return False

        Finally

            'close sql connection
            Call TerminateSqlInstances(_SqlConnection, _SqlTransaction)

            'destroy instance of sql command
            cmdExecuteData = Nothing

            'force garbage collection
            Call GC.Collect(0)

        End Try

    End Function

#End Region

#Region "Get Data To Data Table "

    'Purpose			: To Get Data for given Store Proc
    'Author			    : Sujith Sharatchandran - T-Melmax Sdn Bhd
    'Created Date		: 18/02/2006
    Public Function GetData(ByVal SQLStatement As String) As DataTable

        'create instances - Start
        Dim dtGetData As New DataTable
        Dim _SqlConnection As SqlConnection = Nothing
        'create instances - Stop

        'create instance of sql data adapter
        Dim sdaGetData As SqlDataAdapter = Nothing

        Try

            'open sql connection
            _SqlConnection = GetSqlConnection()

            'execute sql data adapter
            sdaGetData = New SqlDataAdapter(SQLStatement, _SqlConnection)

            'fill data table
            sdaGetData.Fill(dtGetData)

            Return dtGetData

        Finally

            'close sql connection
            Call TerminateSqlInstances(_SqlConnection, Nothing)

            'destroy instance of sql data adapter
            sdaGetData = Nothing

            'force garbage collection
            Call GC.Collect(0)

        End Try

    End Function

#End Region

#Region "Get Data To Data Set "

    'Function Name		: GetDataSet()
    'Parameters		    : SQL Store Proc
    'Return Value		: Data Table
    'Purpose			: To Get Data for given Store Proc
    'Author			    : Sujith Sharatchandran - T-Melmax Sdn Bhd
    'Created Date		: 18/02/2006
    Public Function GetDataSet(ByVal SQLStatement As String) As DataSet

        'create instances - Start
        Dim dsGetData As New DataSet
        Dim _SqlConnection As SqlConnection = Nothing
        'create instances - Stop

        'create instance of sql data adapter
        Dim sdaGetData As SqlDataAdapter = Nothing

        Try

            'open sql connection
            _SqlConnection = GetSqlConnection()

            'execute sql data adapter
            sdaGetData = New SqlDataAdapter(SQLStatement, _SqlConnection)

            'fill data table
            sdaGetData.Fill(dsGetData)

            Return dsGetData

        Finally

            'close sql connection
            Call TerminateSqlInstances(_SqlConnection, Nothing)

            'destroy instance of sql data adapter
            sdaGetData = Nothing

            'destroy instance of data set
            dsGetData = Nothing

            'force garbage collection
            Call GC.Collect(0)

        End Try

    End Function

#End Region

#Region "Get Single Data "

    'Purpose			: To Get a result for the given store proc
    'Author			    : Sujith Sharatchandran - T-Melmax Sdn Bhd
    'Created Date		: 07/03/2006
    Public Function GetValue(ByVal SQLStatement As String) As String

        'Create Instances - Start
        Dim cmdGetValue As New SqlCommand
        Dim _SqlConnection As SqlConnection = Nothing
        'Create Instances - Stop

        'variable declarations
        Dim strResult As String = Nothing

        Try

            'Open sql connection
            _SqlConnection = GetSqlConnection()

            With cmdGetValue

                .Connection = _SqlConnection
                .CommandType = CommandType.Text
                .CommandText = SQLStatement

                'get scalar id
                strResult = .ExecuteScalar()

            End With

            Return strResult

        Finally

            'close sql connection
            Call TerminateSqlInstances(_SqlConnection, Nothing)

            'destroy instance of command object
            cmdGetValue = Nothing

            'force garbage collection
            Call GC.Collect(0)

        End Try

    End Function

#End Region

#Region "Multiple Insert/Update "

    'Purpose			: To Insert/Update
    'Author			    : Sujith Sharatchandran - T-Melmax Sdn Bhd
    'Created Date		: 18/02/2006
    Public Function MultipleExecute(ByVal DataToSave As DataSet, _
                ByVal ParamArray SQLStatement As String()) As Boolean

        'Create Instances - Start
        Dim drDataStore As DataRow = Nothing
        Dim cmdDataStore As SqlCommand = Nothing
        Dim _SqlConnection As SqlConnection = Nothing
        Dim _SqlTransaction As SqlTransaction = Nothing
        'Create Instances - Stop

        'variable declarations - Start
        Dim intCounter As Short = 0, intScalarId As Integer = 0
        Dim intTotalTable As Short = 0, intTableNum As Short = 0
        'variable declarations - Stop

        Try

            'open sql connection
            _SqlConnection = GetSqlConnection()

            'begin transaction
            _SqlTransaction = _SqlConnection.BeginTransaction()

            'execute scalar 
            For Each drDataStore In DataToSave.Tables(0).Rows

                cmdDataStore = New SqlCommand

                With cmdDataStore

                    .Connection = _SqlConnection
                    .Transaction = _SqlTransaction
                    .CommandType = CommandType.StoredProcedure
                    .CommandTimeout = SqlConnectionTimeOut
                    .CommandText = SQLStatement(0).ToString()

                    'Add Command Parameters
                    Call DataHelp.DataToParam(DataToSave.Tables(0), cmdDataStore, 0)

                    'get scalar id
                    intScalarId = .ExecuteScalar()

                End With
            Next

            If intScalarId > 0 Then

                'initialise table count
                intTotalTable = (DataToSave.Tables.Count - 1)

                'loop thro the total number of tables
                For intTableNum = 1 To intTotalTable

                    'initialise counter 
                    intCounter = 0

                    'loop thro the rows of the data table
                    For Each drDataStore In DataToSave.Tables(intTableNum).Rows

                        cmdDataStore = New SqlCommand
                        With cmdDataStore
                            .Connection = _SqlConnection
                            .Transaction = _SqlTransaction
                            .CommandType = CommandType.StoredProcedure
                            .CommandText = SQLStatement(intTableNum).ToString()
                            .CommandTimeout = SqlConnectionTimeOut
                            .Parameters.Add(New SqlParameter("@in_ScalarId", intScalarId))

                            'Add Command Parameters
                            Call DataHelp.DataToParam(DataToSave.Tables(intTableNum), cmdDataStore, intCounter)

                            .ExecuteNonQuery()

                        End With

                        'increment counter
                        intCounter = intCounter + 1

                    Next

                Next

                'commit transaction
                Call _SqlTransaction.Commit()

            Else

                'roll back transaction
                Call _SqlTransaction.Rollback()

            End If

            Return True

        Catch ex As Exception

            'roll back transaction
            Call _SqlTransaction.Rollback()

            'log error
            Call clsGeneric.prcLogError(ex.Message, "MaxModule.DataBase - MultipleExecute")

            Return False

        Finally

            'close sql connection
            Call TerminateSqlInstances(_SqlConnection, _SqlTransaction)

            'force garbage collection
            Call GC.Collect(0)

        End Try

    End Function

#End Region

#Region "Scalar Multiple Insert/Update "

    'Purpose			: To Insert/Update
    'Author			    : Sujith Sharatchandran - T-Melmax Sdn Bhd
    'Created Date		: 18/02/2006
    Public Function MultiScalarExecute(ByVal DataToSave As DataSet, _
                ByVal ParamArray SQLStatement As String()) As Integer

        'Create Instances - Start
        Dim drDataStore As DataRow = Nothing
        Dim cmdDataStore As SqlCommand = Nothing
        Dim _SqlConnection As SqlConnection = Nothing
        Dim _SqlTransaction As SqlTransaction = Nothing
        'Create Instances - Stop

        'Variable declarations - Start
        Dim intCounter As Short = 0, intScalarId As Integer = 0
        Dim intTableNum As Short = 0, intTotalTable As Short = 0
        'variable declarations - Stop

        Try

            'open sql connection
            _SqlConnection = GetSqlConnection()

            'begin transaction
            _SqlTransaction = _SqlConnection.BeginTransaction()

            'execute scalar 
            For Each drDataStore In DataToSave.Tables(0).Rows

                cmdDataStore = New SqlCommand

                With cmdDataStore

                    .Connection = _SqlConnection
                    .Transaction = _SqlTransaction
                    .CommandType = CommandType.StoredProcedure
                    .CommandTimeout = SqlConnectionTimeOut
                    .CommandText = SQLStatement(0).ToString()

                    'Add Command Parameters
                    Call DataHelp.DataToParam(DataToSave.Tables(0), cmdDataStore, 0)

                    'get scalar id
                    intScalarId = .ExecuteScalar()

                End With
            Next

            If intScalarId > 0 Then

                'initialise table count
                intTotalTable = (DataToSave.Tables.Count - 1)

                'loop thro the total number of tables
                For intTableNum = 1 To intTotalTable

                    'initialise counter 
                    intCounter = 0

                    'loop thro the rows of the data table
                    For Each drDataStore In DataToSave.Tables(intTableNum).Rows

                        cmdDataStore = New SqlCommand

                        With cmdDataStore
                            .Connection = _SqlConnection
                            .Transaction = _SqlTransaction
                            .CommandType = CommandType.StoredProcedure
                            .CommandText = SQLStatement(intTableNum).ToString()
                            .CommandTimeout = SqlConnectionTimeOut
                            .Parameters.Add(New SqlParameter("@in_ScalarId", intScalarId))

                            'Add Command Parameters
                            Call DataHelp.DataToParam(DataToSave.Tables(intTableNum), cmdDataStore, intCounter)

                            .ExecuteNonQuery()

                        End With

                        'increment counter
                        intCounter = intCounter + 1

                    Next

                Next

                'commit transaction
                Call _SqlTransaction.Commit()

            Else

                'roll back transaction
                Call _SqlTransaction.Rollback()

            End If

            Return intScalarId

        Catch ex As Exception

            'roll back transaction
            Call _SqlTransaction.Rollback()

            'log error
            Call clsGeneric.prcLogError(ex.Message, "MaxModule.DataBase - MultiScalarExecute")

            Return 0

        Finally

            'close sql connection
            Call TerminateSqlInstances(_SqlConnection, _SqlTransaction)

            'force garbage collection
            Call GC.Collect(0)

        End Try

    End Function

#End Region

#Region "Exceute String Scalar "

    'Function Name		: ScalarString()
    'Parameters		    : Data Table
    'Return Value		: String
    'Purpose			: To Insert/Update 
    'Author			    : Sujith Sharatchandran - T-Melmax Sdn Bhd
    'Created Date		: 17/03/2006
    Public Function ScalarString(ByVal DataCondition As DataTable, _
                ByVal SQLStatement As String) As String

        'Create Instances - Start
        Dim cmdDataStore As New SqlCommand
        Dim _SqlConnection As SqlConnection = Nothing
        'Create Instances - Stop

        'Variable declarations
        Dim strScalar As String = Nothing

        Try

            'Open sql connection
            _SqlConnection = GetSqlConnection()

            With cmdDataStore

                .Connection = _SqlConnection
                .CommandType = CommandType.StoredProcedure
                .CommandTimeout = SqlConnectionTimeOut
                .CommandText = SQLStatement

                'Add Command Parameters
                Call DataHelp.DataToParam(DataCondition, cmdDataStore, 0)

                'get scalar id
                Dim objScalar As Object = .ExecuteScalar()

                If Not IsDBNull(objScalar) Then
                    strScalar = objScalar.GetType.ToString()
                Else
                    strScalar = ""
                End If

            End With

            Return strScalar

        Finally

            'close sql connection
            Call TerminateSqlInstances(_SqlConnection, Nothing)

            'destroy instance of command object
            cmdDataStore = Nothing

            'force garbage collection
            Call GC.Collect(0)

        End Try

    End Function

#End Region

#Region "Scalar Delete - Insert/Update "

    'Purpose			: To Delete and then Insert/Update 
    'Author			    : Sujith Sharatchandran - T-Melmax Sdn Bhd
    'Created Date		: 13/08/2006
    Public Function DeleteInsert(ByVal DeleteSQLStatement As String, _
        ByVal DataToSave As DataSet, ByVal ParamArray SQLStatement As String()) As Boolean

        'Create instances - Start
        Dim dtDataStore As New DataTable
        Dim drDataStore As DataRow = Nothing
        Dim cmdDataStore As SqlCommand = Nothing
        Dim _SqlConnection As SqlConnection = Nothing
        Dim _SqlTransaction As SqlTransaction = Nothing
        'Create instances - Stop

        'variable
        Dim intArrayIndex As Short = 0, intRowIndex As Short = 0

        Try

            'open sql connection
            _SqlConnection = GetSqlConnection()

            'begin transaction
            _SqlTransaction = _SqlConnection.BeginTransaction()

            'execute sql command
            cmdDataStore = New SqlCommand

            With cmdDataStore
                .Connection = _SqlConnection
                .Transaction = _SqlTransaction
                .CommandType = CommandType.Text
                .CommandTimeout = SqlConnectionTimeOut
                .CommandText = DeleteSQLStatement
                .ExecuteNonQuery()
            End With

            'initialise counter 
            intArrayIndex = 0

            'loop thro the tables of the data set - start
            For Each dtDataStore In DataToSave.Tables

                'initialise Row Index
                intRowIndex = 0

                'loop thro the rows of the data table - start
                For Each drDataStore In dtDataStore.Rows

                    cmdDataStore = New SqlCommand

                    With cmdDataStore
                        .Connection = _SqlConnection
                        .Transaction = _SqlTransaction
                        .CommandType = CommandType.StoredProcedure
                        .CommandTimeout = SqlConnectionTimeOut
                        .CommandText = SQLStatement(intArrayIndex).ToString()

                        'Add Command Parameters
                        Call DataHelp.DataToParam(dtDataStore, cmdDataStore, intRowIndex)

                        .ExecuteNonQuery()

                    End With

                    'increment Row Index
                    intRowIndex = intRowIndex + 1

                Next
                'loop thro the rows of the data table - stop

                'increment array Index
                intArrayIndex = intArrayIndex + 1

            Next
            'loop thro the tables of the data set - stop

            'commit transaction
            Call _SqlTransaction.Commit()

            Return True

        Catch ex As Exception

            'roll back transaction
            Call _SqlTransaction.Rollback()

            'log error
            Call clsGeneric.prcLogError(ex.Message, "Database - DeleteInsert")

        Finally

            'close sql connection
            Call TerminateSqlInstances(_SqlConnection, _SqlTransaction)

            'force garbage collection
            Call GC.Collect(0)

        End Try

    End Function

#End Region

#Region "Scalar Delete Multiple Insert/Update "

    'Function Name		: MultiScalarExecute()
    'Parameters		    : Data Set, Param Array
    'Return Value		: Boolean
    'Purpose			: To Insert/Update
    'Author			    : Sujith Sharatchandran - T-Melmax Sdn Bhd
    'Created Date		: 18/02/2006
    Public Function DeleteScalarInsert(ByVal DeleteSQLStatement As String, _
        ByVal DataToSave As DataSet, ByVal ParamArray SQLStatement As String()) As Integer

        'Create Instances - Start
        Dim drDataStore As DataRow = Nothing
        Dim cmdDataStore As SqlCommand = Nothing
        Dim _SqlConnection As SqlConnection = Nothing
        Dim _SqlTransaction As SqlTransaction = Nothing
        'Create Instances - Stop

        'variable declarations - Start
        Dim intCounter As Short = 0, intTableNum As Short = 0
        Dim ScalarId As Integer = 0, intTotalTable As Short = 0
        'variable declarations - Stop

        Try

            'open sql connection
            _SqlConnection = GetSqlConnection()

            'begin transaction
            _SqlTransaction = _SqlConnection.BeginTransaction()

            'execute sql command
            cmdDataStore = New SqlCommand

            With cmdDataStore
                .Connection = _SqlConnection
                .Transaction = _SqlTransaction
                .CommandType = CommandType.Text
                .CommandTimeout = SqlConnectionTimeOut
                .CommandText = DeleteSQLStatement
                .ExecuteNonQuery()
            End With

            'execute scalar 
            For Each drDataStore In DataToSave.Tables(0).Rows
                cmdDataStore = New SqlCommand
                With cmdDataStore

                    .Connection = _SqlConnection
                    .Transaction = _SqlTransaction
                    .CommandType = CommandType.StoredProcedure
                    .CommandTimeout = SqlConnectionTimeOut
                    .CommandText = SQLStatement(0).ToString()

                    'Add Command Parameters
                    Call DataHelp.DataToParam(DataToSave.Tables(0), cmdDataStore, 0)

                    'get scalar id
                    ScalarId = .ExecuteScalar()

                End With
            Next

            If ScalarId > 0 Then

                'initialise table count
                intTotalTable = (DataToSave.Tables.Count - 1)

                'loop thro the total number of tables
                For intTableNum = 1 To intTotalTable

                    'initialise counter 
                    intCounter = 0

                    'loop thro the rows of the data table
                    For Each drDataStore In DataToSave.Tables(intTableNum).Rows

                        cmdDataStore = New SqlCommand
                        With cmdDataStore
                            .Connection = _SqlConnection
                            .Transaction = _SqlTransaction
                            .CommandType = CommandType.StoredProcedure
                            .CommandText = SQLStatement(intTableNum).ToString()
                            .CommandTimeout = SqlConnectionTimeOut
                            .Parameters.Add(New SqlParameter("@in_ScalarId", ScalarId))

                            'Add Command Parameters
                            Call DataHelp.DataToParam(DataToSave.Tables(intTableNum), cmdDataStore, intCounter)

                            .ExecuteNonQuery()
                        End With

                        'increment counter
                        intCounter = intCounter + 1

                    Next

                Next

                'commit transaction
                Call _SqlTransaction.Commit()

            Else

                'roll back transaction
                Call _SqlTransaction.Rollback()

            End If

            Return ScalarId

        Catch ex As Exception

            'roll back transaction
            Call _SqlTransaction.Rollback()

            'log error
            Call clsGeneric.prcLogError(ex.Message, "MaxModule.DataBase - DeleteScalarInsert")

            Return 0

        Finally

            'close sql connection
            Call TerminateSqlInstances(_SqlConnection, _SqlTransaction)

            'force garbage collection
            Call GC.Collect(0)

        End Try

    End Function

#End Region

#Region "Paging Data "

    'Purpose			: To Fetch Data
    'Author			    : Sujith Sharatchandran - T-Melmax Sdn Bhd
    'Created Date		: 20/03/2006
    Public Function PagingData(ByVal StoreProc As String, _
        ByVal Fields As String, ByVal SQLSource As String, _
        ByVal PageIndex As Integer, ByVal PageSize As Short, _
        ByVal TableName As String, ByVal WhereClause As String, _
        ByRef TotalRecords As Short) As DataTable

        'Create Instances - Start
        Dim dsPagingData As New DataSet
        Dim cmdPagingData As New SqlCommand
        Dim sdaPagingData As SqlDataAdapter = Nothing
        Dim _SqlConnection As SqlConnection = Nothing
        'Create Instances - Stop

        Try

            'open sql connection
            _SqlConnection = GetSqlConnection()

            With cmdPagingData
                .Connection = _SqlConnection
                .CommandType = CommandType.StoredProcedure
                .CommandText = StoreProc
                With .Parameters
                    .Add(New SqlParameter("@in_PageIndex", PageIndex))
                    .Add(New SqlParameter("@in_PageSize", PageSize))
                    .Add(New SqlParameter("@in_GetFields", Fields))
                    .Add(New SqlParameter("@in_SQLSource", SQLSource))
                    .Add(New SqlParameter("@in_TableName", TableName))
                    .Add(New SqlParameter("@in_Condition", WhereClause))
                End With
            End With

            sdaPagingData = New SqlDataAdapter(cmdPagingData)
            sdaPagingData.Fill(dsPagingData)

            'get total records returned
            TotalRecords = dsPagingData.Tables(1).Rows(0)(0)

            Return dsPagingData.Tables(0)

        Finally

            'terminate sql connection
            Call TerminateSqlInstances(_SqlConnection, Nothing)

            'force garbage collection
            Call GC.Collect(0)

        End Try

    End Function

#End Region

#Region "Execute Data Set "

    'Function Name		: ExecuteNonQuery()
    'Parameters		    : Data Table
    'Return Value		: Boolean
    'Purpose			: To Insert/Update
    'Author			    : Sujith Sharatchandran - T-Melmax Sdn Bhd
    'Created Date		: 18/02/2006
    Public Function ExecuteDataSet(ByVal DataToSave As DataSet, _
                ByVal ParamArray SQLStatement As String()) As Boolean

        'Create Instances - Start
        Dim drDataStore As DataRow = Nothing
        Dim dtDataStore As DataTable = Nothing
        Dim cmdDataStore As SqlCommand = Nothing
        Dim _SqlConnection As SqlConnection = Nothing
        Dim _SqlTransaction As SqlTransaction = Nothing
        'Create Instances - Stop

        'Variable declarations 
        Dim intCounter As Int16 = 0, intTable As Int16 = 0

        Try

            'Open sql connection
            _SqlConnection = GetSqlConnection()

            'begin transaction
            _SqlTransaction = _SqlConnection.BeginTransaction()

            'loop thro the data set - start
            For Each dtDataStore In DataToSave.Tables

                'initialise Counter
                intCounter = 0

                'loop thro the data table - start
                For Each drDataStore In dtDataStore.Rows

                    cmdDataStore = New SqlCommand
                    With cmdDataStore

                        .Connection = _SqlConnection
                        .Transaction = _SqlTransaction
                        .CommandType = CommandType.StoredProcedure
                        .CommandTimeout = SqlConnectionTimeOut
                        .CommandText = SQLStatement(intTable)

                        'Add Command Parameters
                        Call DataHelp.DataToParam(dtDataStore, cmdDataStore, intCounter)

                        .ExecuteNonQuery()

                    End With

                    'increment counter
                    intCounter = intCounter + 1

                Next
                'loop thro the data table - stop

                'increment counter
                intTable = intTable + 1

            Next
            'loop thro the data set - stop

            'commit transaction
            Call _SqlTransaction.Commit()

            Return True

        Catch ex As Exception

            'roll back transaction
            Call _SqlTransaction.Rollback()

            'log error
            Call clsGeneric.prcLogError(ex.Message, "MaxModule.DataBase.ExecuteDataSet")

            Return False

        Finally

            'close sql connection
            Call TerminateSqlInstances(_SqlConnection, _SqlTransaction)

            'destroy instance of data row
            drDataStore = Nothing

            'destroy instance of data table
            dtDataStore = Nothing

            'destroy instance of command object
            cmdDataStore = Nothing

            'force garbage collection
            Call GC.Collect(0)

        End Try

    End Function

#End Region

#Region "Execute SQL Statement -  Stored Proc "

    'Purpose			: To execute sql statement using Stored Proc.
    'Author			    : Sujith Sharatchandran - T-Melmax Sdn Bhd
    'Created Date		: 18/02/2006
    Public Function ExecuteSQLsp(ByVal ParamArray SQLStatement As String()) As Boolean

        'create instances - Start
        Dim cmdExecuteData As SqlCommand = Nothing
        Dim _SqlConnection As SqlConnection = Nothing
        Dim _SqlTransaction As SqlTransaction = Nothing
        'create instances - Stop

        'variable
        Dim Index As Short = 0

        Try

            'open sql connection
            _SqlConnection = GetSqlConnection()

            'begin transaction
            _SqlTransaction = _SqlConnection.BeginTransaction()

            'loop thro the sql Statements arrays - start
            For Index = 0 To SQLStatement.GetUpperBound(0)

                'execute sql command
                cmdExecuteData = New SqlCommand
                With cmdExecuteData
                    .Connection = _SqlConnection
                    .Transaction = _SqlTransaction
                    .CommandType = CommandType.StoredProcedure
                    .CommandTimeout = SqlConnectionTimeOut
                    .CommandText = GetExecuteSQL & clsGeneric.NullToString(SQLStatement(Index))
                    .ExecuteNonQuery()
                End With

            Next
            'loop thro the sql Statements arrays - stop

            'commit transaction
            Call _SqlTransaction.Commit()

            Return True

        Catch ex As Exception

            'roll back transaction
            Call _SqlTransaction.Rollback()

            'log error
            Call clsGeneric.prcLogError(ex.Message, "MaxModule.DataBase - ExecuteSQLsp")

            Return False

        Finally

            'close sql connection
            Call TerminateSqlInstances(_SqlConnection, _SqlTransaction)

            'destroy instance of sql command
            cmdExecuteData = Nothing

            'force garbage collection
            Call GC.Collect(0)

        End Try

    End Function

#End Region

#Region "Error Log "

    Public Sub ErrorLog(ByVal ErrorSource As String, ByVal CatchMessage As String)

        'Track Application Error Logs - Start
        Call ExecuteSQL(_Helper.SQLErrorLog & clsGeneric.AddQuotes( _
            clsGeneric.ReplaceInvalidSQLChars(CatchMessage)) & clsGeneric.AddComma & _
                clsGeneric.AddQuotes(clsGeneric.ReplaceInvalidSQLChars(ErrorSource)))
        'Track Application Error Logs - Stop

    End Sub

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
            Call clsGeneric.prcLogError(ex.Message, "MaxModule.DataBase - StripHtmlTags")

            Return String.Empty

        End Try

    End Function

#End Region

#Region "Execute SQL - Parameter "

    'Purpose			: To execute sql statement using Param Stored Proc.
    'Author			    : Sujith Sharatchandran - T-Melmax Sdn Bhd
    'Created Date		: 08/02/2012
    Public Function ExecuteSQLParam(ByVal StoredProcName As String, _
       ByVal ParamArray SqlParamValues As String()) As DataTable

        'create instances - Start
        Dim _DataTable As New DataTable
        Dim _SqlCommand As New SqlCommand
        Dim _SqlConnection As SqlConnection = Nothing
        Dim _SqlDataReader As SqlDataReader = Nothing
        'create instances - Stop

        'variable
        Dim Index As Short = 0, ParamName As String = Nothing

        Try

            'open sql connection
            _SqlConnection = GetSqlConnection()

            With _SqlCommand

                .Connection = _SqlConnection
                .CommandType = CommandType.StoredProcedure
                .CommandText = StoredProcName

                'loop thro the sql Param value arrays - start
                For Index = 0 To SqlParamValues.GetUpperBound(0)

                    'Build Param Name - Start
                    ParamName = Index + 1
                    ParamName = clsGeneric.StringFiller(ParamName, 2, EnumHelp.FillerType.Prefix, "0")
                    ParamName = Helper.InputParam & ParamName
                    'Build Param Name - Stop

                    'Add Parameter with values
                    .Parameters.AddWithValue(ParamName, SqlParamValues(Index))

                Next
                'loop thro the sql Param value arrays - stop

                'Execute Reder to Data Reader
                _SqlDataReader = .ExecuteReader

                'Load Data reader to Data Table
                _DataTable.Load(_SqlDataReader)

            End With

            Return _DataTable

        Catch ex As Exception

            'log error
            Call clsGeneric.prcLogError(ex.Message, "MaxModule.DataBase.ExecuteSQLParam")

            Return Nothing

        Finally

            'close sql connection
            Call TerminateSqlInstances(_SqlConnection, Nothing)

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
        Dim _SqlParameter As SqlParameter = Nothing
        'Create Instances - Stop

        'Variable Declarations
        Dim ParameterName As String = Nothing, ParameterValue As String = Nothing

        Try

            'Loop thro the Parameter Collections - Start
            For Each _DbParameter In _DbParameterCollection

                'Get Data - Start
                ParameterValue = _DbParameter.Value
                ParameterName = _DbParameter.ParameterName
                'Get Data - Stop

                'Create Parameter for command
                _SqlParameter = New SqlParameter()

                'Set Param Name
                _SqlParameter.ParameterName = ParameterName

                'Set Param Value
                _SqlParameter.Value = clsGeneric.NullToString(ParameterValue)

                'Add Parameter
                _DbCommand.Parameters.Add(_SqlParameter)

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
