#Region "NameSpaces "

Imports MaxGeneric
Imports System.Data
Imports DataObjects
Imports System.Data.Common
Imports System.Data.SqlClient
Imports System.Collections.Generic

#End Region

Namespace PowerQueryManager

    Public Class UserGroupDL

#Region "Global Declarations "

        Private SqlStatement As String = Nothing

        Private _DbParameterCollection As DbParameterCollection = Nothing

        Private _DatabaseFactory As New MaxModule.DatabaseProvider()

        Private DataBaseConnectionString As String = Helper.GetConnectionString()

#End Region

#Region "Retrive Data "

        Public Function RetriveData(ByVal AppEntity As BusinessEntities.UserGroupEn,
            ByRef strRetrunErrorMsg As String, ByVal DbSQLConnString As String,
            ByRef _DataSet As Object) As Boolean

            Try

                With AppEntity

                    Select Case .SQLCase
                        Case 1
                            'SqlStatement = ""
                            SqlStatement = "SELECT usergroupid FROM UR_UserGroups WHERE Status = 'true' "
                            If Not String.IsNullOrEmpty(.DepartmentID) Then
                                SqlStatement += "AND DepartmentID = '" & .DepartmentID & "' "
                            End If
                        Case 2
                            SqlStatement = "SELECT * FROM UR_UserGroups WHERE Status = 'true' "
                            If Not String.IsNullOrEmpty(.DepartmentID) Then
                                SqlStatement += "AND DepartmentID = '" & .DepartmentID & "' "
                            End If
                    End Select

                End With

                'Get Data Set
                _DataSet = _DatabaseFactory.ExecuteDataSet(Helper.GetDataBaseType,
                    DataBaseConnectionString, SqlStatement)

                Return True

            Catch ex As Exception

                'Set Error Message
                strRetrunErrorMsg = ex.Message

                Return False

            End Try

        End Function

#End Region

#Region "Get User Group Lisr "

        Public Function GetUserGroupList(ByVal UserGroupEn As BusinessEntities.UserGroupEn) As DataSet

            Try

                With UserGroupEn
                    SqlStatement = "SELECT * FROM UR_UserGroups WHERE Status = 'true' "
                    If Not String.IsNullOrEmpty(.DepartmentID) Then
                        SqlStatement += "AND DepartmentID = '" & .DepartmentID & "' "
                    End If
                End With

                Return _DatabaseFactory.ExecuteDataSet(Helper.GetDataBaseType,
                    DataBaseConnectionString, SqlStatement)

            Catch ex As Exception
                Throw ex
            End Try

        End Function

#End Region

    End Class
End Namespace
