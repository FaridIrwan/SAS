#Region "NameSpaces "

Imports System
Imports MaxGeneric
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports System.Threading.Tasks
Imports System.Configuration

#End Region

Public Class Helper

#Region "Get Database Type "

    Public Shared ReadOnly Property GetDataBaseType() As Short

        Get
            Return MaxModule.Helper.GetDataBaseType
        End Get

    End Property

#End Region

#Region "Get Connection String "

    Public Shared Function GetConnectionString() As String

        If GetDataBaseType = MaxModule.Helper.DataBaseType.PostGres Then
            Return clsGeneric.NullToString(
                ConfigurationManager.AppSettings("PG_CONNNECTION_STR"))
        ElseIf GetDataBaseType = MaxModule.Helper.DataBaseType.SqlServer Then
            Return clsGeneric.NullToString(
                ConfigurationManager.AppSettings("SQL_CONFIG_KEY"))
        End If

        Return String.Empty

    End Function

#End Region

#Region "Date Conversion "

    Public Shared Function DateConversion(ByVal _DateTime As DateTime) As String
        Return String.Format("{0:u}", _DateTime)
    End Function

#End Region

End Class
