#Region "NameSpaces "

Imports MaxGeneric
Imports System.Data
Imports DataObjects
Imports System.Data.Common
Imports System.Data.SqlClient
Imports System.Collections.Generic

#End Region

Namespace PowerQueryManager
    Public Class UserDL

#Region "Global Declarations "

        Private SqlStatement As String = Nothing

        Private _DbParameterCollection As DbParameterCollection = Nothing

        Private _DatabaseFactory As New MaxModule.DatabaseProvider()

        Private DataBaseConnectionString As String = Helper.GetConnectionString()

#End Region

#Region "Insert "

        Public Function Insert(ByVal UserEn As BusinessEntities.UserEn) As Boolean

            Try
                Dim userid As Integer = 0
                Dim SqlStatement1 As String = String.Empty

                With UserEn

                    'get userid details - Start
                    SqlStatement1 = "select max(userid) from ur_users;"

                    userid = clsGeneric.NullToInteger(_DatabaseFactory.ExecuteScalar(
                        Helper.GetDataBaseType, DataBaseConnectionString, SqlStatement1))
                    userid = clsGeneric.ConvertToInt(userid) + 1
                    'get userid details - Stop

                    'Build Sql Statement - Start
                    SqlStatement = "INSERT INTO UR_Users"
                    SqlStatement += "(userid, "
                    SqlStatement += "UserName, "
                    SqlStatement += "Password, "
                    SqlStatement += "UserGroupId, "
                    SqlStatement += "Email, "
                    SqlStatement += "Department, "
                    SqlStatement += "RecStatus, "
                    SqlStatement += "UserStatus, "
                    SqlStatement += "LastUpdatedBy, "
                    SqlStatement += "LastUpdatedDtTm"
                    SqlStatement += ") VALUES ("
                    SqlStatement += userid & ", "
                    SqlStatement += clsGeneric.AddQuotes(.UserName) & ", "
                    SqlStatement += clsGeneric.AddQuotes(.Password) & ", "
                    SqlStatement += .UserGroupId & ", "
                    SqlStatement += clsGeneric.AddQuotes(.Email) & ", "
                    SqlStatement += clsGeneric.AddQuotes(.Department) & ", "
                    SqlStatement += clsGeneric.AddQuotes(.RecStatus) & ", "
                    SqlStatement += clsGeneric.AddQuotes(.UserStatus) & ", "
                    SqlStatement += clsGeneric.AddQuotes(.LastUpdatedBy) & ", "
                    SqlStatement += "'" & Format(.LastUpdatedDtTm, "yyyy-MM-dd") & "' "
                    SqlStatement += ")"
                    'Build Sql Statement - Stop

                End With


                'Execute Sql Statement - Start
                If _DatabaseFactory.ExecuteSqlStatement(Helper.GetDataBaseType,
                    DataBaseConnectionString, SqlStatement) > -1 Then
                    Return True
                End If
                'Execute Sql Statement - Stop

                Return False

            Catch ex As Exception
                Throw ex
            End Try

        End Function

#End Region

#Region "Update "

        Public Function Update(ByVal UserEn As BusinessEntities.UserEn) As Boolean

            Try

                'Build Sql Statement
                SqlStatement = "UPDATE UR_Users Set "

                With UserEn

                    'Build Sql Statement - Start
                    SqlStatement += "UserName = " & clsGeneric.AddQuotes(.UserName) & ", "
                    SqlStatement += "Password = " & clsGeneric.AddQuotes(.Password) & ", "
                    SqlStatement += "UserGroupId = " & .UserGroupId & ", "
                    SqlStatement += "Email = " & clsGeneric.AddQuotes(.Email) & ", "
                    SqlStatement += "Department = " & clsGeneric.AddQuotes(.Department) & ", "
                    SqlStatement += "RecStatus = " & clsGeneric.AddQuotes(.RecStatus) & ", "
                    SqlStatement += "UserStatus = " & clsGeneric.AddQuotes(.UserStatus) & ", "
                    SqlStatement += "LastUpdatedBy = " & clsGeneric.AddQuotes(.LastUpdatedBy) & ", "
                    SqlStatement += "LastUpdatedDtTm = '" & Format(.LastUpdatedDtTm, "yyyy-MM-dd") & "' "
                    SqlStatement += "WHERE UserName = " & clsGeneric.AddQuotes(.UserName)
                    'Build Sql Statement - Stop

                End With

                'Execute Sql Statement - Start
                If _DatabaseFactory.ExecuteSqlStatement(Helper.GetDataBaseType,
                    DataBaseConnectionString, SqlStatement) > -1 Then
                    Return True
                End If
                'Execute Sql Statement - Stop

                Return False

            Catch ex As Exception
                Throw ex
            End Try

        End Function

#End Region

#Region "Delete "

        Public Function Delete(ByVal UserEn As BusinessEntities.UserEn) As Boolean

            Try

                With UserEn
                    'Build Sql Statement
                    SqlStatement = "DELETE From UR_Users WHERE UserID = " & .UserID
                End With


                'Execute Sql Statement - Start
                If _DatabaseFactory.ExecuteSqlStatement(Helper.GetDataBaseType,
                    DataBaseConnectionString, SqlStatement) > -1 Then
                    Return True
                End If
                'Execute Sql Statement - Stop

                Return False

            Catch ex As Exception
                Throw ex
            End Try

        End Function

#End Region

#Region "Get User "

        Public Function GetUser(ByVal UserEn As BusinessEntities.UserEn) As DataSet

            Try

                With UserEn

                    'Build Sql Statement
                    SqlStatement = "SELECT * FROM UR_Users WHERE UserID = " & .UserID

                End With

                'Return Data Set
                Return _DatabaseFactory.ExecuteDataSet(Helper.GetDataBaseType,
                    DataBaseConnectionString, SqlStatement)

            Catch ex As Exception
                Throw ex
            End Try

        End Function

#End Region

#Region "Data Grid "

        Public Function DataGrid(ByVal UserEn As BusinessEntities.UserEn) As DataSet

            Try

                With UserEn

                    'Build Sql Statement - Start
                    SqlStatement = "SELECT US.UserID, US.UserName, SD.Department, UUG.Description, "
                    SqlStatement += "CASE WHEN US.UserStatus = 'True' THEN 'Active' ELSE 'Inactive' END UserStatus "
                    SqlStatement += "FROM UR_Users US "
                    SqlStatement += "LEFT JOIN SAS_Department SD ON US.Department = SD.DepartmentID "
                    SqlStatement += "LEFT JOIN UR_UserGroups UUG ON US.UserGroupId = UUG.UserGroupId "
                    If Not FormHelp.IsBlank(.SearchCriteria) Then
                        SqlStatement += " WHERE US.UserName = '" & .SearchCriteria & "' OR SD.Department LIKE '%" & .SearchCriteria & "%' "
                        SqlStatement += "OR UUG.Description LIKE '%" & .SearchCriteria & "%' "
                    End If
                    SqlStatement += " ORDER BY US.UserName"
                    'Build Sql Statement - Stop

                End With

                'Return Data Set
                Return _DatabaseFactory.ExecuteDataSet(Helper.GetDataBaseType,
                    DataBaseConnectionString, SqlStatement)

            Catch ex As Exception
                Throw ex
            End Try

        End Function

#End Region

    End Class
End Namespace

