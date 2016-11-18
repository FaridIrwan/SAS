
#Region "NameSpaces "

Imports MaxModule
Imports MaxGeneric
Imports System.Data
Imports System.Configuration
Imports Microsoft.VisualBasic

#End Region

Public Class ReportHelper

#Region "Global Declarations "

    Private _DatabaseFactory As New MaxModule.DatabaseProvider

#End Region

#Region "Get Data Set "

    Public Function GetDataSet(ByVal ParamArray SqlStatement As String()) As DataSet
        Try

            Return _DatabaseFactory.ExecuteMultiSqlDataSet(HTS.SAS.DataAccessObjects.Helper.GetDataBaseType,
                HTS.SAS.DataAccessObjects.Helper.GetConnectionString, SqlStatement)

        Catch ex As Exception

            Return Nothing

        End Try

    End Function

#End Region

#Region "Get Data from Execute Scalar "

    Public Function GetExecuteScalar(ByVal SqlSclarStatement As String) As Object

        Try


            Return _DatabaseFactory.ExecuteScalar(HTS.SAS.DataAccessObjects.Helper.GetDataBaseType,
                     HTS.SAS.DataAccessObjects.Helper.GetConnectionString, SqlSclarStatement)

        Catch ex As Exception

            Return Nothing

        End Try

    End Function

#End Region

#Region "Report Status Filter"

    'added by Hafiz @ 26/4/2016

    Public Function Status(ByVal blstatus As Boolean) As String

        Dim sqlstats As String = Nothing

        sqlstats = "(select sass_code from sas_studentstatus where sass_blstatus='" & blstatus & "')"

        Return sqlstats

    End Function

#End Region

End Class

