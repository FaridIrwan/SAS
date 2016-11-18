#Region "NameSpaces "

Imports MaxGeneric
Imports System.Data
Imports DataObjects
Imports System.Data.Common
Imports System.Data.SqlClient
Imports System.Collections.Generic

#End Region

Namespace PowerQueryManager
    Public Class StudentLedgerDL

#Region "Global Declarations "

        Private SqlStatement As String = Nothing

        Private _DbParameterCollection As DbParameterCollection = Nothing

        Private _DatabaseFactory As New MaxModule.DatabaseProvider()

        Private DataBaseConnectionString As String = Helper.GetConnectionString()

#End Region

#Region "GetStudentLedger "

        Public Function GetStudentLedger(ByVal AccountEn As BusinessEntities.AccountsEn) As DataSet

            Try
                With AccountEn
                    SqlStatement = "SELECT TransDate, "
                    SqlStatement += " TransCode, "
                    SqlStatement += " Description, "
                    SqlStatement += " Category, "
                    SqlStatement += " CASE WHEN TransType = 'Credit' THEN TransAmount ELSE 0 END Debit, "
                    SqlStatement += " CASE WHEN TransType = 'Debit' THEN TransAmount ELSE 0 END Credit, "
                    SqlStatement += " TransAmount,"
                    SqlStatement += " TransType,"
                    SqlStatement += " BatchCode"
                    SqlStatement += " FROM SAS_Accounts"
                    SqlStatement += " WHERE CreditRef = '" & .CreditRef & "' AND SubType = '" & .SubType & "' AND PostStatus ='Posted' "
                    SqlStatement += " ORDER BY TransDate"
                End With

                Return _DatabaseFactory.ExecuteDataSet(Helper.GetDataBaseType,
                    DataBaseConnectionString, SqlStatement)

            Catch ex As Exception
                Throw ex
            End Try

        End Function

#End Region

#Region "GetStudentData "

        Public Function GetStudentData(ByVal StudentEn As BusinessEntities.StudentEn) As DataSet

            Try

                With StudentEn
                    SqlStatement = "SELECT SASI_Name, "
                    SqlStatement += " SASI_ICNo, "
                    SqlStatement += " ST.SASS_Code + ' - ' + SS.SASS_Description Status, "
                    SqlStatement += " SASI_CurSem Semester, "
                    SqlStatement += " SASI_CurSemYr CurrentSemester, "
                    SqlStatement += " ST.SASI_PgId + ' - ' + SP.SAPG_Program Program, "
                    SqlStatement += " ST.SASI_Faculty + ' - ' + SF.SAFC_Desc Faculty, "
                    SqlStatement += " SK.SAKO_Description Kolej, "
                    SqlStatement += " SB.SABK_Description Block, "
                    SqlStatement += " ST.SASI_FloorNo FloorNo, "
                    SqlStatement += " RT.SART_Description RoomType "
                    SqlStatement += " FROM SAS_STUDENT ST LEFT JOIN SAS_StudentStatus SS ON ST.SASS_Code=SS.SASS_Code "
                    SqlStatement += " LEFT JOIN SAS_Program SP ON ST.SASI_PgId = SP.SAPG_Code "
                    SqlStatement += " LEFT JOIN SAS_Faculty SF ON ST.SASI_Faculty = SF.SAFC_Code "
                    SqlStatement += " LEFT JOIN SAS_Kolej SK ON ST.SAKO_Code = SK.SAKO_Code "
                    SqlStatement += " LEFT JOIN SAS_Block SB ON ST.SABK_Code = SB.SABK_Code "
                    SqlStatement += " LEFT JOIN SAS_RoomType RT ON SB.SABK_Code = RT.SABK_Code AND SK.SAKO_Code = RT.SART_Code "
                    SqlStatement += " WHERE SASI_MatricNo = '" & .SASI_MatricNo & "' "
                End With

                Return _DatabaseFactory.ExecuteDataSet(Helper.GetDataBaseType,
                    DataBaseConnectionString, SqlStatement)

            Catch ex As Exception
                Throw ex
            End Try

        End Function

#End Region

#Region "GetStudentOutstanding "

        Public Function GetStudentOutstanding(ByVal StudentEn As BusinessEntities.StudentEn) As DataSet

            Try
                With StudentEn
                    SqlStatement = "SELECT "
                    SqlStatement += " SAS_StudentOutstanding.SASI_MatricNo AS MatricNo, "
                    SqlStatement += " SAS_StudentOutstanding.SASO_DueAmount AS OutstandingAmt, "
                    SqlStatement += " SAS_StudentOutstanding.SASO_IsReleased AS StatusReleased "
                    SqlStatement += " FROM SAS_StudentOutstanding "
                    SqlStatement += " WHERE SAS_StudentOutstanding.SASI_MatricNo = '" & .SASI_MatricNo & "' "
                    SqlStatement += " GROUP BY SAS_StudentOutstanding.SASI_MatricNo, "
                    SqlStatement += " SAS_StudentOutstanding.SASO_DueAmount, "
                    SqlStatement += " SAS_StudentOutstanding.SASO_IsReleased "
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

