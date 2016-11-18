
#Region "NameSpaces "

Imports MaxGeneric
Imports System.Data
Imports DataObjects
Imports System.Data.Common
Imports System.Data.SqlClient
Imports System.Collections.Generic

#End Region

Namespace PowerQueryManager
    Public Class UserRightsDL

#Region "Global Declarations "

        Private SqlStatement As String = Nothing

        Private _DbParameterCollection As DbParameterCollection = Nothing

        Private _DatabaseFactory As New MaxModule.DatabaseProvider()

        Private DataBaseConnectionString As String = Helper.GetConnectionString()

#End Region

#Region "Retrive Data "

        Public Function RetriveData(ByVal AppEntity As BusinessEntities.UserRightsEn,
            ByRef strRetrunErrorMsg As String, ByVal DbSQLConnString As String,
            ByRef _DataSet As Object) As Boolean

            Try

                With AppEntity
                    Select Case .SQLCase
                        Case 1
                            SqlStatement = "SELECT * FROM UR_UserRights "
                        Case Else
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

    End Class
End Namespace
