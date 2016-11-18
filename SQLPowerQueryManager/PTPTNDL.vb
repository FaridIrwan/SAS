#Region "NameSpaces "

Imports MaxGeneric
Imports System.Data
Imports DataObjects
Imports System.Data.Common
Imports System.Data.SqlClient
Imports System.Collections.Generic

#End Region

Namespace PowerQueryManager
    Public Class PTPTNDL

#Region "Global Declarations "

        Private SqlStatement As String = Nothing

        Private _DbParameterCollection As DbParameterCollection = Nothing

        Private _DatabaseFactory As New MaxModule.DatabaseProvider()

        Private DataBaseConnectionString As String = Helper.GetConnectionString()

#End Region

#Region "Retrive Prog Fee "

        Public Function RetriveProgFee(ByVal PTPTNFee As BusinessEntities.PTPTN,
            ByRef strRetrunErrorMsg As String, ByVal DbSQLConnString As String,
            ByRef _DataSet As DataSet) As Boolean

            Try

                With PTPTNFee
                    SqlStatement = "SELECT B.ProgFee FROM SAS_Student A "
                    SqlStatement += "INNER JOIN SAS_PTPTNFeeSetup B ON A.SASI_PgId = B.ProgCode "
                    SqlStatement += "WHERE A.SASI_ICNo = '" & .NoKP.Trim & "' AND SASS_Code = 'PA' "
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

#Region "Insert Data "

        Public Function InsertData(ByVal PTPTNFeeSetupEn As BusinessEntities.PTPTN,
            ByRef strRetrunErrorMsg As String, ByVal DbSQLConnString As String) As Boolean

            Try

                With PTPTNFeeSetupEn
                    SqlStatement = "INSERT INTO SAS_PTPTNFeeSetup "
                    SqlStatement += "(ProgCode, ProgName, ProgFee, CreatedBy) VALUES "
                    SqlStatement += "('')"
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

    End Class
End Namespace

