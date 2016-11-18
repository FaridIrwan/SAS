#Region "NameSpaces "

Imports MaxGeneric
Imports System.Data
Imports DataObjects
Imports System.Data.Common
Imports System.Data.SqlClient
Imports System.Collections.Generic

#End Region

Namespace PowerQueryManager
    Public Class PTPTNFessSetupDL

#Region "Global Declarations "

        Private SqlStatement As String = Nothing

        Private _DbParameterCollection As DbParameterCollection = Nothing

        Private _DatabaseFactory As New MaxModule.DatabaseProvider()

        Private DataBaseConnectionString As String = Helper.GetConnectionString()

#End Region

#Region "Data Grid "

        Public Function DataGrid(ByVal PTPTNFeeSetupEn As BusinessEntities.PTPTNFeeSetupEn,
            ByRef strRetrunErrorMsg As String, ByVal DbSQLConnString As String,
            ByRef _DataSet As DataSet) As Boolean

            Try

                With PTPTNFeeSetupEn
                    SqlStatement = "SELECT * FROM SAS_PTPTNFeeSetup "
                    If Not FormHelp.IsBlank(.ProgCode) Then
                        SqlStatement += "WHERE ProgCode = '" & .ProgCode & "' "
                    End If
                    SqlStatement += "ORDER BY ProgCode"
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

#Region "Retrive Search Data "

        Public Function RetriveSearchData(ByVal PTPTNFeeSetupEn As BusinessEntities.PTPTNFeeSetupEn,
            ByRef strRetrunErrorMsg As String, ByVal DbSQLConnString As String,
            ByRef _DataSet As DataSet) As Boolean

            Try

                With PTPTNFeeSetupEn
                    SqlStatement = "SELECT ProgFee FROM SAS_PTPTNFeeSetup WHERE ProgCode = '" & .ProgCode & "' "
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

        Public Function InsertData(ByVal PTPTNFeeSetupEn As BusinessEntities.PTPTNFeeSetupEn,
            ByRef strRetrunErrorMsg As String, ByVal DbSQLConnString As String) As Boolean

            Try

                With PTPTNFeeSetupEn
                    SqlStatement = "INSERT INTO SAS_PTPTNFeeSetup "
                    SqlStatement += "(ProgCode, ProgName, ProgFee, CreatedBy) VALUES "
                    SqlStatement += "('" & .ProgCode.ToString().Trim & "', '" & .ProgName.ToString().Trim & "', '" & CDbl(.ProgFee) & "', '" & .CreatedBy.ToString() & "')"
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

        Public Function UpdateData(ByVal PTPTNFeeSetupEn As BusinessEntities.PTPTNFeeSetupEn,
            ByRef strRetrunErrorMsg As String, ByVal DbSQLConnString As String) As Boolean

            Try

                With PTPTNFeeSetupEn
                    SqlStatement = "UPDATE SAS_PTPTNFeeSetup SET "
                    SqlStatement += "ProgCode = '" & .ProgCode.ToString().Trim & "', ProgName = '" & .ProgName.ToString().Trim & "', "
                    SqlStatement += "ProgFee = '" & CDbl(.ProgFee) & "', CreatedBy = '" & .CreatedBy.ToString() & "' "
                    SqlStatement += "WHERE AutoID = '" & .AutoID & "' "
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

        Public Function DeleteData(ByVal PTPTNFeeSetupEn As BusinessEntities.PTPTNFeeSetupEn,
            ByRef strRetrunErrorMsg As String, ByVal DbSQLConnString As String) As Boolean

            Try

                With PTPTNFeeSetupEn
                    SqlStatement = "DELETE FROM SAS_PTPTNFeeSetup WHERE ProgCode = '" & .ProgCode & "' "
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
