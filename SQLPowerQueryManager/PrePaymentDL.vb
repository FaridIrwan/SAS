#Region "NameSpaces "

Imports MaxGeneric
Imports System.Data
Imports DataObjects
Imports System.Data.Common
Imports System.Data.SqlClient
Imports System.Collections.Generic

#End Region

Namespace PowerQueryManager

    Public Class PrePaymentDL

#Region "Global Declarations "

        Private SqlStatement As String = Nothing

        Private _DbParameterCollection As DbParameterCollection = Nothing

        Private _DatabaseFactory As New MaxModule.DatabaseProvider()

        Private DataBaseConnectionString As String = Helper.GetConnectionString()

#End Region

#Region "GetPrePayment "

        Public Function GetPrePayment(ByVal StudentEn As BusinessEntities.StudentEn) As DataSet

            Try

                With StudentEn
                    SqlStatement = " SELECT FS.SAPG_Code ProgramCode, "
                    SqlStatement += " SP.SAPG_Program Program, "
                    SqlStatement += " FS.SAFS_Code, "
                    SqlStatement += " FS.SAFS_Semester, "
                    SqlStatement += " FS.SAST_Code Semester, "
                    SqlStatement += " FSD.SAFT_Code FeeCode, "
                    SqlStatement += " CONVERT(Decimal (8,2), FSA.SAFA_Amount) FeeAmount, "
                    SqlStatement += " FT.SAFT_Desc FeeDesc "
                    SqlStatement += " FROM SAS_FeeStruct FS "
                    SqlStatement += " LEFT JOIN SAS_Program SP ON FS.SAPG_Code = SP.SAPG_Code "
                    SqlStatement += " LEFT JOIN SAS_FeeStrDetails FSD ON FS.SAFS_Code = FSD.SAFS_Code "
                    SqlStatement += " LEFT JOIN SAS_FeeStrAmount FSA ON FS.SAFS_Code = FSA.SAFS_Code AND FSA.SAFT_Code = FSD.SAFT_Code "
                    SqlStatement += " LEFT JOIN SAS_FeeTypes FT ON FSD.SAFT_Code = FT.SAFT_Code "
                    SqlStatement += " LEFT JOIN SAS_Student SS ON SS.SASI_PgId = SP.SAPG_Code AND SS.SASC_Code = FSA.SASC_Code "
                    SqlStatement += " AND SS.SASI_CurSem + 1 = FS.SAFS_Semester "
                    SqlStatement += " AND SS.SASI_Faculty = SP.SAFC_Code "
                    SqlStatement += " WHERE "
                    SqlStatement += " SS.SASI_MatricNo = '" & .SASI_MatricNo & "' "
                    SqlStatement += " AND FS.SAST_Code = (SELECT CONVERT(INT,SASI_CurSemYr) + 0100010001 FROM SAS_Student WHERE SASI_MatricNo = '" & .SASI_MatricNo & "' ) "
                    SqlStatement += " GROUP BY FS.SAPG_Code, SP.SAPG_Program, "
                    SqlStatement += " FS.SAFS_Code, "
                    SqlStatement += " FS.SAFS_Semester, "
                    SqlStatement += " FS.SAST_Code, "
                    SqlStatement += " FSD.SAFT_Code, "
                    SqlStatement += " FSA.SAFA_Amount, "
                    SqlStatement += " FT.SAFT_Desc"
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

