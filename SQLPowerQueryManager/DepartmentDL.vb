#Region "NameSpaces "

Imports MaxGeneric
Imports System.Data
Imports DataObjects
Imports System.Data.Common
Imports System.Data.SqlClient
Imports System.Collections.Generic

#End Region

Namespace PowerQueryManager
    Public Class DepartmentDL

#Region "Global Declarations "

        Private SqlStatement As String = Nothing

        Private _DbParameterCollection As DbParameterCollection = Nothing

        Private _DatabaseFactory As New MaxModule.DatabaseProvider()

        Private DataBaseConnectionString As String = Helper.GetConnectionString()

#End Region

#Region "Insert Data "

        Public Function InsertData(ByVal AppEntity As BusinessEntities.DepartmentEn,
            ByRef strRetrunErrorMsg As String, ByVal DbSQLConnString As String) As Boolean

            Try
                Dim autoid As Integer = 0
                Dim SqlStatement1 As String = String.Empty
                Dim SqlStatement As String = String.Empty

                With AppEntity

                    'get autoid details - Start
                    SqlStatement1 = "select max(autoid) from SAS_Department;"
                    autoid = clsGeneric.NullToInteger(_DatabaseFactory.ExecuteScalar(
                        Helper.GetDataBaseType, DataBaseConnectionString, SqlStatement1))
                    autoid = clsGeneric.ConvertToInt(autoid) + 1
                    'get autoid details - Stop

                    'Build SqlStatement - Start
                    SqlStatement = "INSERT INTO SAS_Department ("
                    SqlStatement &= "autoid, "
                    SqlStatement &= "DepartmentID, "
                    SqlStatement &= "Department, "
                    SqlStatement &= "Status, "
                    SqlStatement &= "createdby, "
                    SqlStatement &= "createdate)"
                    SqlStatement &= " VALUES ("                    '
                    SqlStatement &= autoid & ", "
                    SqlStatement &= clsGeneric.AddQuotes(.DepartmentID) & ", "
                    SqlStatement &= clsGeneric.AddQuotes(.Department) & ", "
                    SqlStatement &= clsGeneric.NullToBoolean(.Status) & ", "
                    SqlStatement &= clsGeneric.AddQuotes(.CreatedBy) & ", "
                    SqlStatement &= "'" & Format(.CreateDate, "yyyy-MM-dd") & "'); "
                    'Build SqlStatement - Stop

                End With
               
                'Execute Sql Statement - Start
                If _DatabaseFactory.ExecuteSqlStatement(Helper.GetDataBaseType,
                    DataBaseConnectionString, SqlStatement) > -1 Then
                    Return True
                End If
                'Execute Sql Statement - Stop

                Return False

            Catch ex As Exception

                'Set Error Message
                strRetrunErrorMsg = ex.Message

                Return False

            End Try

        End Function

#End Region

#Region "Update Data "

        ' Edit Statement 
        Public Function UpdateData(ByVal AppEntity As BusinessEntities.DepartmentEn,
            ByRef strRetrunErrorMsg As String, ByVal DbSQLConnString As String) As Boolean

            Try
                'Build SqlStatement
                SqlStatement = "UPDATE SAS_Department Set "

                With AppEntity
                    Select Case .SQLCase
                        Case 1
                            SqlStatement += "DepartmentID = " & clsGeneric.AddQuotes(.DepartmentID)
                            SqlStatement += ", Department = " & clsGeneric.AddQuotes(.Department)
                            SqlStatement += ", Status = " & clsGeneric.NullToBoolean(.Status)
                            SqlStatement += ", ModifiedBy = " & clsGeneric.AddQuotes(.ModifiedBy)
                            'SqlStatement += ", ModifiedDate = " & Helper.DateConversion(.ModifiedDate)
                            SqlStatement &= ", ModifiedDate = '" & Format(.ModifiedDate, "yyyy-MM-dd") & "'"
                            'SqlStatement += ", ModifiedDate = " & clsGeneric.AddQuotes(.ModifiedDate)
                            SqlStatement += " WHERE AutoID = " & clsGeneric.NullToInteger(.AutoID) & " ;"
                        Case Else
                    End Select
                End With

                'Execute Sql Statement - Start
                If _DatabaseFactory.ExecuteSqlStatement(Helper.GetDataBaseType,
                   DataBaseConnectionString, SqlStatement) > -1 Then
                    Return True
                End If
                'Execute Sql Statement - Stop

                Return False

            Catch ex As Exception

                'Set Error Message
                strRetrunErrorMsg = ex.Message

                Return False

            End Try

        End Function

#End Region

#Region "Delete Data "

        ' Delete Statement 
        Public Function DeleteData(ByVal AppEntity As BusinessEntities.DepartmentEn,
            ByRef strRetrunErrorMsg As String, ByVal DbSQLConnString As String) As Boolean

            Try
                'Added by Zoya @ 20/02/2016
                Dim DSReturn As Object
                SqlStatement = "Select * from UR_UserGroups WHERE DepartmentID = "
                SqlStatement &= clsGeneric.AddQuotes(AppEntity.DepartmentID)

                DSReturn = _DatabaseFactory.ExecuteDataSet(Helper.GetDataBaseType,
                       DataBaseConnectionString, SqlStatement)

                If (DSReturn.Tables(0).Rows.Count > 0) Then
                    strRetrunErrorMsg = "Record Already In Use"
                    Return False
                    'Else
                    '    Return True
                End If
                'End Added by Zoya

                'Build Sql Statement - Start
                SqlStatement = "DELETE From SAS_Department WHERE DepartmentID = "
                SqlStatement &= clsGeneric.AddQuotes(AppEntity.DepartmentID)
                'Build Sql Statement - Stop

                'Execute Sql Statement - Start
                If _DatabaseFactory.ExecuteSqlStatement(Helper.GetDataBaseType,
                   DataBaseConnectionString, SqlStatement) > -1 Then
                    Return True
                End If
                'Execute Sql Statement - Stop

                Return False

            Catch ex As Exception

                'Set Error Message
                strRetrunErrorMsg = ex.Message

                Return False

            End Try

        End Function

#End Region

#Region "Retrive Data "

        ' Retrieve Statement 
        Public Function RetriveData(ByVal AppEntity As BusinessEntities.DepartmentEn,
            ByRef strRetrunErrorMsg As String, ByVal DbSQLConnString As String,
            ByRef DSReturn As Object) As Boolean

            Try
                Dim blnResult As Integer = 0

                Select Case AppEntity.SQLCase
                    Case 1

                        'Build Sql Statement - Start
                        SqlStatement = "SELECT * FROM SAS_Department WHERE DepartmentID = "
                        SqlStatement &= clsGeneric.AddQuotes(AppEntity.DepartmentID)
                        'Build Sql Statement - Stop

                        'Added by Zoya-20/02/2016
                        DSReturn = _DatabaseFactory.ExecuteDataSet(Helper.GetDataBaseType,
                        DataBaseConnectionString, SqlStatement)

                        If (DSReturn.Tables(0).Rows.Count > 0) Then
                            Return False
                        Else
                            Return True
                        End If
                        'End Added by Zoya-20/02/2016
                    Case 2
                        'Build Sql Statement - Start
                        SqlStatement = "SELECT * FROM SAS_Department WHERE AutoID = "
                        'SqlStatement &= clsGeneric.AddQuotes(AppEntity.AutoID)
                        SqlStatement &= clsGeneric.NullToInteger(AppEntity.AutoID)
                        'Build Sql Statement - Stop
                    Case 3
                        'Build Sql Statement - Start
                        SqlStatement = "SELECT * FROM SAS_Department"
                        'Build Sql Statement - Stop
                    Case 4
                        'Build Sql Statement - Start
                        SqlStatement = "SELECT * FROM SAS_Department WHERE status=true"
                        'Build Sql Statement - Stop

                End Select
                'Build Sql Statement - Stop

                'Get Data Set - Start
                DSReturn = _DatabaseFactory.ExecuteDataSet(Helper.GetDataBaseType,
                   DataBaseConnectionString, SqlStatement)
                'Get Data Set - Stop

                'If _DatabaseFactory.ExecuteSqlStatement(Helper.GetDataBaseType,
                '   DataBaseConnectionString, SqlStatement) > -1 Then
                '    Return True
                'End If

                Return True

            Catch ex As Exception

                'Set Error Message
                strRetrunErrorMsg = ex.Message

                Return False

            End Try

        End Function

#End Region

#Region "Data Grid "

        'Grid View DS Statement 
        Public Function DataGrid(ByVal AppEntity As BusinessEntities.DepartmentEn,
            ByRef strRetrunErrorMsg As String, ByVal DbSQLConnString As String,
            ByRef _DataSet As DataSet) As Boolean

            Try
                With AppEntity

                    'Build Sqlstatement - Start
                    SqlStatement = "SELECT * FROM SAS_Department "
                    If Not String.IsNullOrEmpty(.SearchCriteria) Then
                        SqlStatement &= " WHERE DepartmentID = '" & .SearchCriteria & "' OR Department LIKE '%" & .SearchCriteria & "%' "
                    End If
                    SqlStatement &= " ORDER BY DepartmentID"
                    'Build Sqlstatement - Stop

                End With

                'Get Data Set
                _DataSet = _DatabaseFactory.ExecuteDataSet(Helper.
                    GetDataBaseType, DataBaseConnectionString, SqlStatement)


                Return True

            Catch ex As Exception

                'Set Error Message
                strRetrunErrorMsg = ex.Message

                Return False

            End Try

        End Function

#End Region

#Region "Get Department List "

        Public Function GetDepartmentList(ByVal DepartmentEn As BusinessEntities.DepartmentEn) As DataSet

            Try

                'Build Sql Statement
                SqlStatement = "SELECT * FROM SAS_Department WHERE Status = 'true' "

                'Return Data Set
                Return _DatabaseFactory.ExecuteDataSet(Helper.GetDataBaseType,
                    DataBaseConnectionString, SqlStatement)

            Catch ex As Exception

                Return Nothing

            End Try

        End Function

#End Region

    End Class
End Namespace
