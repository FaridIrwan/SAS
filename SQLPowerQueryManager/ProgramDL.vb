#Region "NameSpaces "

Imports MaxGeneric
Imports System.Data
Imports DataObjects
Imports System.Data.Common
Imports System.Data.SqlClient
Imports System.Collections.Generic

#End Region

Namespace PowerQueryManager
    Public Class ProgramDL

#Region "Global Declarations "

        Private SqlStatement As String = Nothing

        Private _DbParameterCollection As DbParameterCollection = Nothing

        Private _DatabaseFactory As New MaxModule.DatabaseProvider()

        Private DataBaseConnectionString As String = Helper.GetConnectionString()

#End Region

#Region "Get Program By Faculty "

        Public Function GetProgramByFaculty(ByVal ProgramEn As BusinessEntities.ProgramEn,
            ByRef strRetrunErrorMsg As String, ByVal DbSQLConnString As String,
            ByRef _DataSet As DataSet) As Boolean

            Try

                With ProgramEn
                    SqlStatement = "SELECT * FROM SAS_Program "
                    SqlStatement += "WHERE SAFC_Code = '" & .SAFC_Code & "' "
                    SqlStatement += "ORDER BY SAPG_Code"
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

