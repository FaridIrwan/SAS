#Region "NameSpaces "

Imports MaxGeneric
Imports System.Data
Imports DataObjects
Imports System.Data.Common
Imports System.Data.SqlClient
Imports System.Collections.Generic

#End Region

Namespace PowerQueryManager
    Public Class SemesterSetup

#Region "Global Declarations "

        Private SqlStatement As String = Nothing

        Private _DbParameterCollection As DbParameterCollection = Nothing

        Private _DatabaseFactory As New MaxModule.DatabaseProvider()

        Private DataBaseConnectionString As String = Helper.GetConnectionString()

#End Region

#Region "SemesterSetup_Retreive_Details "

        ' Retrieve Statement 
        Public Function SemesterSetup_Retreive_Details(ByVal AppEntity As BusinessEntities.SemesterSetup,
        ByVal DbSQLConnString As String, ByRef ReturnErrorMsg As String,
        ByRef _DataSet As DataSet) As Boolean

            Try

                With AppEntity
                    Select Case .SQLCase
                        Case 1
                            SqlStatement = "Select * From SAS_SemesterSetup ORDER BY SAST_Description, SAST_Semester "
                        Case Else
                    End Select
                End With

                _DataSet = _DatabaseFactory.ExecuteDataSet(Helper.GetDataBaseType,
                    DataBaseConnectionString, SqlStatement)

                Return True

            Catch ex As Exception

                ReturnErrorMsg = ex.Message
                Return False

            End Try
        End Function

#End Region

    End Class
End Namespace
