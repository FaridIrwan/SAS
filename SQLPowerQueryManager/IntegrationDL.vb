#Region "NameSpaces "

Imports MaxGeneric
Imports System.Data
Imports DataObjects
Imports System.Data.Common
Imports System.Data.SqlClient
Imports System.Collections.Generic

#End Region

Namespace PowerQueryManager
    Public Class IntegrationDL

#Region "Global Declarations "

        Private SqlStatement As String = Nothing

        Private _DataSet As DataSet = Nothing

        Private _DbParameterCollection As DbParameterCollection = Nothing

        Private _DatabaseFactory As New MaxModule.DatabaseProvider()

        Private DataBaseConnectionString As String = Helper.GetConnectionString()

#End Region

#Region "Get Batch Details "

        Public Function GetBatchDetails(ByVal BatchCode As String) As DataSet

            Try

                SqlStatement = "SELECT a.BatchCode batch_code, a.PostedDateTime posted_date, a.UpdatedBy userid FROM SAS_Accounts a, SAS_AccountsDetails b"
                SqlStatement += " WHERE a.TransID = b.TransID and a.BatchCode in ('" + BatchCode + "')"
                SqlStatement += " group by a.BatchCode, a.PostedDateTime, a.UpdatedBy"

                Return _DatabaseFactory.ExecuteDataSet(Helper.GetDataBaseType, DataBaseConnectionString, SqlStatement)


            Catch ex As Exception
                Throw ex
            End Try

        End Function

#End Region

#Region "GetLine Details DR "

        Public Function GetLineDetailsDR(ByVal strBatchCode As String) As DataSet

            Try

                SqlStatement = "SELECT ROW_NUMBER() OVER (ORDER BY a.BatchCode) line_no, a.Batchcode batch_code, c.SAPG_TI GLCode, SUM(a.TransAmount) TotalAmount "
                SqlStatement += " FROM SAS_Accounts a, SAS_Student b, SAS_Program c, SAS_Faculty d "
                SqlStatement += " WHERE a.CreditRef = b.SASI_MatricNo And b.SASI_PgId = C.SAPG_Code And b.SASI_Faculty = d.SAFC_Code "
                SqlStatement += " AND a.BatchCode in ('" + strBatchCode + "') GROUP BY a.Batchcode, c.SAPG_TI "

                Return _DatabaseFactory.ExecuteDataSet(Helper.GetDataBaseType, DataBaseConnectionString, SqlStatement)


            Catch ex As Exception
                Throw ex
            End Try

        End Function

#End Region

#Region "Get Line Details CR "

        Public Function GetLineDetailsCR(ByVal strBatchCode As String) As DataSet

            Try

                SqlStatement = "SELECT ROW_NUMBER() OVER (ORDER BY a.batchcode) line_no, a.BatchCode batch_code, c.SAFT_GLCode GLCode,sum(b.TransAmount) TotalAmount "
                SqlStatement += "FROM SAS_Accounts a, SAS_AccountsDetails b, SAS_FeeTypes c "
                SqlStatement += "WHERE a.TransID = b.TransID AND b.RefCode = c.SAFT_Code AND a.BatchCode in ('" + strBatchCode + "') "
                SqlStatement += "GROUP BY a.BatchCode, c.SAFT_GLCode"

                Return _DatabaseFactory.ExecuteDataSet(Helper.GetDataBaseType, DataBaseConnectionString, SqlStatement)


            Catch ex As Exception
                Throw ex
            End Try

        End Function

#End Region

#Region "Get Line Details CNDR "

        Public Function GetLineDetailsCNDR(ByVal strBatchCode As String) As DataSet

            Try

                SqlStatement = "SELECT ROW_NUMBER() OVER (ORDER BY a.batchcode) line_no, a.BatchCode batch_code, c.SAFT_GLCode GLCode,sum(b.TransAmount) TotalAmount "
                SqlStatement += "FROM SAS_Accounts a, SAS_AccountsDetails b, SAS_FeeTypes c "
                SqlStatement += "WHERE a.TransID = b.TransID AND b.RefCode = c.SAFT_Code AND a.BatchCode in ('" + strBatchCode + "') "
                SqlStatement += "GROUP BY a.BatchCode, c.SAFT_GLCode"

                Return _DatabaseFactory.ExecuteDataSet(Helper.GetDataBaseType, DataBaseConnectionString, SqlStatement)

            Catch ex As Exception
                Throw ex
            End Try

        End Function

#End Region

#Region "Get Line Details CNCR "

        Public Function GetLineDetailsCNCR(ByVal strBatchCode As String) As DataSet

            Try

                SqlStatement = "SELECT ROW_NUMBER() OVER (ORDER BY a.BatchCode) line_no, a.Batchcode batch_code, c.SAPG_TI GLCode, SUM(a.TransAmount) TotalAmount "
                SqlStatement += " FROM SAS_Accounts a, SAS_Student b, SAS_Program c, SAS_Faculty d "
                SqlStatement += " WHERE a.CreditRef = b.SASI_MatricNo And b.SASI_PgId = C.SAPG_Code And b.SASI_Faculty = d.SAFC_Code "
                SqlStatement += " AND a.BatchCode in ('" + strBatchCode + "') GROUP BY a.Batchcode, c.SAPG_TI "

                Return _DatabaseFactory.ExecuteDataSet(Helper.GetDataBaseType, DataBaseConnectionString, SqlStatement)

            Catch ex As Exception
                Throw ex
            End Try

        End Function

#End Region

#Region "GetAFCBatchDetails "

        Public Function GetAFCBatchDetails(ByVal BatchCode As String) As DataSet

            Try

                SqlStatement = "SELECT b.BatchCode batch_code, b.PostedDateTime posted_date, a.UpdatedBy userid FROM SAS_AFC a, SAS_Accounts b"
                SqlStatement += " WHERE a.BatchCode = b.BatchCode AND b.BatchCode in ('" + BatchCode + "') AND b.Category = 'AFC'"
                SqlStatement += " GROUP BY b.BatchCode, b.PostedDateTime, a.UpdatedBy"

                Return _DatabaseFactory.ExecuteDataSet(Helper.GetDataBaseType, DataBaseConnectionString, SqlStatement)

            Catch ex As Exception
                Throw ex
            End Try

        End Function

#End Region

#Region "GetAFCLineDetailsDR "

        Public Function GetAFCLineDetailsDR(ByVal strBatchCode As String) As DataSet

            Try

                SqlStatement = "SELECT ROW_NUMBER() OVER (ORDER BY b.BatchCode) line_no, b.BatchCode batch_code,"
                SqlStatement += "d.SAPG_TI GLCode,"
                SqlStatement += "SUM(b.TransAmount) TotalAmount FROM SAS_AFC a, SAS_Accounts b, SAS_Student c, SAS_Program d, SAS_Faculty e "
                SqlStatement += "WHERE  a.BatchCode = b.BatchCode "
                SqlStatement += "AND b.CreditRef = c.SASI_MatricNo  "
                SqlStatement += "AND c.SASI_PgId = d.SAPG_Code  "
                SqlStatement += "AND c.SASI_Faculty = e.SAFC_Code "
                SqlStatement += "AND b.BatchCode in ('" + strBatchCode + "') "
                SqlStatement += " GROUP BY b.BatchCode,d.SAPG_TI "

                Return _DatabaseFactory.ExecuteDataSet(Helper.GetDataBaseType, DataBaseConnectionString, SqlStatement)

            Catch ex As Exception
                Throw ex
            End Try

        End Function

#End Region

#Region "GetAFCLineDetailsCR "

        Public Function GetAFCLineDetailsCR(ByVal strBatchCode As String) As DataSet

            Try

                SqlStatement = "SELECT ROW_NUMBER() OVER (ORDER BY b.batchcode) line_no, b.BatchCode batch_code, d.SAFT_GLCode GLCode, SUM(c.TransAmount) TotalAmount "
                SqlStatement += "FROM SAS_AFC a, SAS_Accounts b, SAS_AccountsDetails c, SAS_FeeTypes d "
                SqlStatement += "WHERE a.BatchCode = b.BatchCode "
                SqlStatement += "AND b.TransID = c.TransID "
                SqlStatement += "AND c.RefCode = d.SAFT_Code "
                SqlStatement += "AND b.BatchCode in ('" + strBatchCode + "') and c.TransAmount > 0 "
                SqlStatement += "GROUP BY b.BatchCode, d.SAFT_GLCode"

                Return _DatabaseFactory.ExecuteDataSet(Helper.GetDataBaseType, DataBaseConnectionString, SqlStatement)

            Catch ex As Exception
                Throw ex
            End Try

        End Function

#End Region

#Region "GetPayment_CBPayBatch "

        Public Function GetPayment_CBPayBatch(ByVal strBatchCode As String) As DataSet

            Try

                SqlStatement = "SELECT Substring(BatchCode, 9,8) BatchCode, BankCode, PostedDateTime, SUM(TransAmount) TotalAmount, UpdatedBy UserID "
                SqlStatement += "FROM SAS_Accounts "
                SqlStatement += "WHERE BatchCode in ('" + strBatchCode + "') "
                SqlStatement += "AND Category = 'Refund' "
                SqlStatement += "GROUP BY BatchCode, BankCode, PostedDateTime, UpdatedBy "

                Return _DatabaseFactory.ExecuteDataSet(Helper.GetDataBaseType, DataBaseConnectionString, SqlStatement)

            Catch ex As Exception
                Throw ex
            End Try

        End Function

#End Region

#Region "GetPayment_GLDist "

        Public Function GetPayment_GLDist(ByVal strBatchCode As String) As DataSet

            Try

                SqlStatement = "SELECT Row_number() OVER(ORDER BY GLCode) LineNum, * "
                SqlStatement += "FROM (SELECT c.SAPG_TI GLCode,Substring(a.BatchCode, 9, 8) BatchCode, Sum(a.TransAmount) TotalAmount "
                SqlStatement += "FROM SAS_Accounts a, "
                SqlStatement += "SAS_Student b, "
                SqlStatement += "SAS_Program c "
                SqlStatement += "WHERE a.CreditRef = b.SASI_MatricNo "
                SqlStatement += "AND b.SASI_PgId = c.SAPG_Code "
                SqlStatement += "AND a.BatchCode IN ('" + strBatchCode + "') "
                SqlStatement += "GROUP  BY c.SAPG_TI, "
                SqlStatement += "a.BatchCode "
                SqlStatement += "UNION ALL "
                SqlStatement += "SELECT b.SABD_GLCode GLCode, Substring(a.BatchCode, 9, 8) BatchCode, Sum(a.TransAmount) TotalAmount "
                SqlStatement += "FROM   SAS_Accounts a, "
                SqlStatement += "SAS_BankDetails b "
                SqlStatement += "WHERE a.BankCode = b.SABD_Code "
                SqlStatement += "AND a.BatchCode IN ('" + strBatchCode + "') "
                SqlStatement += "GROUP  BY b.SABD_GLCode, "
                SqlStatement += "a.BatchCode) GL_Dist "

                Return _DatabaseFactory.ExecuteDataSet(Helper.GetDataBaseType, DataBaseConnectionString, SqlStatement)

            Catch ex As Exception
                Throw ex
            End Try

        End Function

#End Region

#Region "GetReceipt_CBRcptBatch "

        Public Function GetReceipt_CBRcptBatch(ByVal strBatchCode As String) As DataSet

            Try

                SqlStatement = "SELECT BankCode, PostedDateTime, Sum(TransAmount) TotalAmount, UpdatedBy UserID "
                SqlStatement += "FROM SAS_Accounts "
                SqlStatement += "WHERE BatchCode IN ( '" & strBatchCode & "' ) "
                SqlStatement += "GROUP  BY BankCode, PostedDateTime, UpdatedBy "

                Return _DatabaseFactory.ExecuteDataSet(Helper.GetDataBaseType, DataBaseConnectionString, SqlStatement)

            Catch ex As Exception
                Throw ex
            End Try

        End Function

#End Region

#Region "GetReceipt_CBRcptHdr "

        Public Function GetReceipt_CBRcptHdr(ByVal strBatchCode As String) As DataSet

            Try
                SqlStatement = "SELECT Row_number() OVER(ORDER BY a.Transcode) LineNum, Substring(a.Transcode, 8, 12) Transcode, "
                SqlStatement += "a.SubRef1, a.BatchCode, b.SASI_Name, b.SASI_Add1, b.SASI_Add2, b.SASI_Add3, "
                SqlStatement += "a.PaymentMode, a.BankCode, a.TransAmount "
                SqlStatement += "FROM SAS_Accounts a, "
                SqlStatement += "SAS_Student b "
                SqlStatement += "WHERE a.CreditRef = b.SASI_MatricNo "
                SqlStatement += "AND a.BatchCode IN ( '" & strBatchCode & "' ) "

                Return _DatabaseFactory.ExecuteDataSet(Helper.GetDataBaseType, DataBaseConnectionString, SqlStatement)

            Catch ex As Exception
                Throw ex
            End Try

        End Function

#End Region

#Region "GetPaymentBatchDetails "

        Public Function GetPaymentBatchDetails(ByVal strBatchCode As String) As DataSet

            Try

                SqlStatement = "SELECT Substring(a.BatchCode, 9, 8) BatchCode, Row_number() OVER(ORDER BY a.BatchCode) LineNum, "
                SqlStatement += "a.VoucherNo VoucherNo,c.SABD_BankName Payee, a.PaymentMode PaymentMode, "
                SqlStatement += "a.BankCode BankCode, Sum(a.TransAmount) TotalAmount, a.PostedDateTime PostedDateTime, a.Description "
                SqlStatement += "FROM   SAS_Accounts a, "
                SqlStatement += "SAS_Student b, "
                SqlStatement += "SAS_BankDetails c "
                SqlStatement += "WHERE a.CreditRef = b.SASI_MatricNo "
                SqlStatement += "AND a.BankCode = c.SABD_Code "
                SqlStatement += "AND a.BatchCode IN ('" & strBatchCode & "') "
                SqlStatement += "AND a.Category = 'Refund' "
                SqlStatement += "GROUP  BY a.BatchCode, "
                SqlStatement += "a.VoucherNo, "
                SqlStatement += "c.SABD_BankName, "
                SqlStatement += "a.PaymentMode, "
                SqlStatement += "a.BankCode, "
                SqlStatement += "a.PostedDateTime, "
                SqlStatement += "a.Description"

                Return _DatabaseFactory.ExecuteDataSet(Helper.GetDataBaseType, DataBaseConnectionString, SqlStatement)

            Catch ex As Exception
                Throw ex
            End Try

        End Function

#End Region

#Region "GetIntegrationStatus "

        Public Function GetIntegrationStatus() As DataSet

            Try

                SqlStatement = "SELECT * FROM UR_Config where CON_Code = '100' and CON_CodeID = '101' "

                Return _DatabaseFactory.ExecuteDataSet(Helper.GetDataBaseType, DataBaseConnectionString, SqlStatement)

            Catch ex As Exception
                Throw ex
            End Try

        End Function

#End Region

#Region "UpdateIntegrationStatus "

        Public Function UpdateIntegrationStatus(ByVal ConfigEn As BusinessEntities.UR_Config) As Boolean

            Try

                SqlStatement = "UPDATE [UR_Config] Set "

                With ConfigEn
                    SqlStatement += "CON_Value1 = N'" & .CON_Value1 & "', "
                    SqlStatement += "CON_Value2 = N'" & .CON_Value2 & "', "
                    SqlStatement += "CON_Value3 = N'" & .CON_Value3 & "', "
                    SqlStatement += "CON_UpdatedDate = N'" & .CON_UpdatedDate & "', "
                    SqlStatement += "CON_UpdatedBy = N'" & .CON_UpdatedBy & "' "
                    SqlStatement += "WHERE CON_CodeID = '101' "
                End With

                If _DatabaseFactory.ExecuteSqlStatement(Helper.GetDataBaseType, DataBaseConnectionString, SqlStatement) > -1 Then
                    Return True
                End If

                Return False

            Catch ex As Exception
                Throw ex
            End Try

        End Function

#End Region

#Region "GetUnPostedData "

        Public Function GetUnPostedData() As DataSet

            Try

                SqlStatement = "SELECT BatchCode, Description, Category, TransDate FROM   SAS_Accounts "
                SqlStatement += "WHERE PostToCF = 0 "
                SqlStatement += "AND Category IN ('AFC', 'Invoice', 'Receipt', 'Credit Note', 'Debit Note', 'Refund') "
                SqlStatement += "AND PostStatus = 'POSTED' "
                SqlStatement += "GROUP BY BatchCode, Description, Category, TransDate "
                SqlStatement += "ORDER BY BatchCode "

                Return _DatabaseFactory.ExecuteDataSet(Helper.GetDataBaseType, DataBaseConnectionString, SqlStatement)

            Catch ex As Exception
                Throw ex
            End Try

        End Function

#End Region

#Region "InsertHeader "

        Public Function InsertHeader(ByVal MJ_JnlHdr As BusinessEntities.MJ_JnlHdr) As Boolean

            Try

                With MJ_JnlHdr
                    SqlStatement = "INSERT INTO mj_jnlhdr "
                    SqlStatement += "(mjjh_company, "
                    SqlStatement += "mjjh_jnl, "
                    SqlStatement += "mjjh_entdate, "
                    SqlStatement += "mjjh_desc, "
                    SqlStatement += "mjjh_jnldate, "
                    SqlStatement += "mjjh_status, "
                    SqlStatement += "mjjh_postdate, "
                    SqlStatement += "mjjh_nprefid, "
                    SqlStatement += "mjjh_curcode, "
                    SqlStatement += "mjjh_currate, "
                    SqlStatement += "mjjh_baseunit, "
                    SqlStatement += "mjjh_operator, "
                    SqlStatement += "mjjh_who, "
                    SqlStatement += "mjjh_dtime, "
                    SqlStatement += "mjjh_ldtime) "
                    SqlStatement += "VALUES "
                    SqlStatement += "('" & .mjjh_company & "', "
                    SqlStatement += "'" & .mjjl_jnl & "', "
                    SqlStatement += "DateTime(" & .mjjh_entdate & ") YEAR TO SECOND, "
                    SqlStatement += "'" & .mjjh_desc & "', "
                    SqlStatement += "DateTime(" & .mjjh_jnldate & ") YEAR TO SECOND, "
                    SqlStatement += "'" & .mjjh_status & "', "
                    SqlStatement += "" & .mjjh_postdate & ", "
                    SqlStatement += "" & .mjjh_nprefid & ", "
                    SqlStatement += "'" & .mjjh_curcode & "', "
                    SqlStatement += "'" & .mjjh_currate & "', "
                    SqlStatement += "'" & .mjjh_baseunit & "', "
                    SqlStatement += "'" & .mjjh_operator & "', "
                    SqlStatement += "'" & .mjjh_who & "', "
                    SqlStatement += "" & .mjjh_dtime & ", "
                    SqlStatement += "" & .mjjh_ldtime & ")"
                End With

                If _DatabaseFactory.ExecuteSqlStatement(Helper.GetDataBaseType, DataBaseConnectionString, SqlStatement) > -1 Then
                    Return True
                End If

                Return False

            Catch ex As Exception
                Throw ex
            End Try

        End Function

#End Region

#Region "InsertLine "

        Public Function InsertLine(ByVal MJ_JnlLine As BusinessEntities.MJ_JnlLine) As Boolean

            Try

                With MJ_JnlLine
                    SqlStatement = "INSERT INTO mj_jnlline "
                    SqlStatement += "(mjjl_company, "
                    SqlStatement += "mjjl_jnl, "
                    SqlStatement += "mjjl_lineno, "
                    SqlStatement += "mjjl_ledger, "
                    SqlStatement += "mjjl_account, "
                    SqlStatement += "mjjl_units, "
                    SqlStatement += "mjjl_desc, "
                    SqlStatement += "mjjl_amount, "
                    SqlStatement += "mjjl_reference, "
                    SqlStatement += "mjjl_lclamount) "
                    SqlStatement += "VALUES "
                    SqlStatement += "('" & .mjjl_Company & "', "
                    SqlStatement += "'" & .mjjl_jnl & "', "
                    SqlStatement += "'" & .mjjl_lineno & "', "
                    SqlStatement += "'" & .mjjl_ledger & "', "
                    SqlStatement += "'" & .mjjl_account & "', "
                    SqlStatement += "" & .mjjl_units & ", "
                    SqlStatement += "'" & .mjjl_desc & "', "
                    SqlStatement += "'" & .mjjl_amount & "', "
                    SqlStatement += "'" & .mjjl_reference & "', "
                    SqlStatement += "'" & .mjjl_lclamount & "')"
                End With

                If _DatabaseFactory.ExecuteSqlStatement(Helper.GetDataBaseType, DataBaseConnectionString, SqlStatement) > -1 Then
                    Return True
                End If

                Return False

            Catch ex As Exception
                Throw ex
            End Try

        End Function

#End Region

#Region "InsertPayment_CBPaybatch "

        Public Function InsertPayment_CBPaybatch(ByVal CB_PayBatchEn As BusinessEntities.CB_PayBatchEn) As Boolean

            Try

                With CB_PayBatchEn
                    SqlStatement = "INSERT INTO cb_paybatch "
                    SqlStatement += "(cbpb_batchid, "
                    SqlStatement += "cbpb_bank, "
                    SqlStatement += "cbpb_company, "
                    SqlStatement += "cbpb_type, "
                    SqlStatement += "cbpb_batchtot, "
                    SqlStatement += "cbpb_curcode, "
                    SqlStatement += "cbpb_currate, "
                    SqlStatement += "cbpb_baseunit, "
                    SqlStatement += "cbpb_operator, "
                    SqlStatement += "cbpb_status, "
                    SqlStatement += "cbpb_batchdate, "
                    SqlStatement += "cbpb_usrctltot, "
                    SqlStatement += "cbpb_postdate, "
                    SqlStatement += "cbpb_source, "
                    SqlStatement += "cbpb_nprefid, "
                    SqlStatement += "cbpb_who, "
                    SqlStatement += "cbpb_dtime, "
                    SqlStatement += "cbpb_ldtime) "
                    SqlStatement += "VALUES "
                    SqlStatement += "('" & .cbpb_batchid & "', "
                    SqlStatement += "'" & .cbpb_bank & "', "
                    SqlStatement += "'" & .cbpb_company & "', "
                    SqlStatement += "'" & .cbpb_type & "', "
                    SqlStatement += "'" & .cbpb_batchtot & "', "
                    SqlStatement += "'" & .cbpb_curcode & "', "
                    SqlStatement += "" & .cbpb_currate & ", "
                    SqlStatement += "" & .cbpb_baseunit & ", "
                    SqlStatement += "'" & .cbpb_operator & "', "
                    SqlStatement += "'" & .cbpb_status & "', "
                    SqlStatement += "DateTime(" & .cbpb_batchdate & ") YEAR TO SECOND, "
                    SqlStatement += "'" & .cbpb_usrctltot & "', "
                    SqlStatement += "" & .cbpb_postdate & ", "
                    SqlStatement += "'" & .cbpb_source & "', "
                    SqlStatement += "" & .cbpb_nprefid & ", "
                    SqlStatement += "'" & .cbpb_who & "', "
                    SqlStatement += "" & .cbpb_dtime & ", "
                    SqlStatement += "" & .cbpb_ldtime & ")"
                End With

                If _DatabaseFactory.ExecuteSqlStatement(Helper.GetDataBaseType, DataBaseConnectionString, SqlStatement) > -1 Then
                    Return True
                End If

                Return False

            Catch ex As Exception
                Throw ex
            End Try

        End Function

#End Region

#Region "InsertPayment_CBPayHdr "

        Public Function InsertPayment_CBPayHdr(ByVal CB_PayHdrEn As BusinessEntities.CB_PayHdrEn) As Boolean

            Try

                With CB_PayHdrEn
                    SqlStatement = "INSERT INTO cb_payhdr "
                    SqlStatement += "(cbph_company, "
                    SqlStatement += "cbph_batchid, "
                    SqlStatement += "cbph_lineno, "
                    SqlStatement += "cbph_pmtid, "
                    SqlStatement += "cbph_voucher, "
                    SqlStatement += "cbph_payee, "
                    SqlStatement += "cbph_payeeaddr1, "
                    SqlStatement += "cbph_payeeaddr2, "
                    SqlStatement += "cbph_payeeaddr3, "
                    SqlStatement += "cbph_payeeaddr4, "
                    SqlStatement += "cbph_amount, "
                    SqlStatement += "cbph_lclamount, "
                    SqlStatement += "cbph_bankname, "
                    SqlStatement += "cbph_bankbranch, "
                    SqlStatement += "cbph_bankcode, "
                    SqlStatement += "cbph_bankacct, "
                    SqlStatement += "cbph_magic) "
                    SqlStatement += "VALUES "
                    SqlStatement += "('" & .cbph_company & "', "
                    SqlStatement += "'" & .cbph_batchid & "', "
                    SqlStatement += "'" & .cbph_lineno & "', "
                    SqlStatement += "'" & .cbph_pmtid & "', "
                    SqlStatement += "'" & .cbph_voucher & "', "
                    SqlStatement += "'" & .cbph_payee & "', "
                    SqlStatement += "" & .cbph_payeeaddr1 & ", "
                    SqlStatement += "" & .cbph_payeeaddr2 & ", "
                    SqlStatement += "" & .cbph_payeeaddr3 & ", "
                    SqlStatement += "" & .cbph_payeeaddr4 & ", "
                    SqlStatement += "'" & .cbph_amount & "', "
                    SqlStatement += "'" & .cbph_lclamount & "', "
                    SqlStatement += "" & .cbph_bankname & ", "
                    SqlStatement += "" & .cbph_bankbranch & ", "
                    SqlStatement += "'" & .cbph_bankcode & "', "
                    SqlStatement += "" & .cbph_bankacct & ", "
                    SqlStatement += "" & .cbph_magic & ")"
                End With

                If _DatabaseFactory.ExecuteSqlStatement(Helper.GetDataBaseType, DataBaseConnectionString, SqlStatement) > -1 Then
                    Return True
                End If

                Return False

            Catch ex As Exception
                Throw ex
            End Try

        End Function

#End Region

#Region "InsertPayment_CBPayrmt "

        Public Function InsertPayment_CBPayrmt(ByVal CB_PayrmtEn As BusinessEntities.CB_PayrmtEn) As Boolean

            Try

                With CB_PayrmtEn
                    SqlStatement = "INSERT INTO cb_payrmt "
                    SqlStatement += "(cbpr_magic, "
                    SqlStatement += "cbpr_lineno, "
                    SqlStatement += "cbpr_itemdate, "
                    SqlStatement += "cbpr_itemrefno, "
                    SqlStatement += "cbpr_itemamount, "
                    SqlStatement += "cbpr_secondref, "
                    SqlStatement += "cbpr_comment, "
                    SqlStatement += "cbpr_rmtamount, "
                    SqlStatement += "cbpr_discount, "
                    SqlStatement += "cbpr_ppsamount, "
                    SqlStatement += "cbpr_ppsnumber) "
                    SqlStatement += "VALUES "
                    SqlStatement += "(" & .cbpr_magic & ", "
                    SqlStatement += "'" & .cbpr_lineno & "', "
                    SqlStatement += "DateTime(" & .cbpr_itemdate & ") YEAR TO SECOND, "
                    SqlStatement += "'" & .cbpr_itemrefno & "', "
                    SqlStatement += "" & .cbpr_itemamount & ", "
                    SqlStatement += "" & .cbpr_secondref & ", "
                    SqlStatement += "'" & .cbpr_comment & "', "
                    SqlStatement += "'" & .cbpr_rmtamount & "', "
                    SqlStatement += "" & .cbpr_discount & ", "
                    SqlStatement += "" & .cbpr_ppsamount & ", "
                    SqlStatement += "" & .cbpr_ppsnumber & ")"
                End With

                If _DatabaseFactory.ExecuteSqlStatement(Helper.GetDataBaseType, DataBaseConnectionString, SqlStatement) > -1 Then
                    Return True
                End If

                Return False

            Catch ex As Exception
                Throw ex
            End Try

        End Function

#End Region

#Region "InsertReceipt_CBRcptBatch "

        Public Function InsertReceipt_CBRcptBatch(ByVal CB_RcptBatchEn As BusinessEntities.CB_RcptBatchEn) As Boolean

            Try

                With CB_RcptBatchEn
                    SqlStatement = "INSERT INTO cb_rcptbatch "
                    SqlStatement += "(cbrb_batchid, "
                    SqlStatement += "cbrb_bank, "
                    SqlStatement += "cbrb_company, "
                    SqlStatement += "cbrb_batchtot, "
                    SqlStatement += "cbrb_curcode, "
                    SqlStatement += "cbrb_currate, "
                    SqlStatement += "cbrb_baseunit, "
                    SqlStatement += "cbrb_operator, "
                    SqlStatement += "cbrb_status, "
                    SqlStatement += "cbrb_batchdate, "
                    SqlStatement += "cbrb_usrctltot, "
                    SqlStatement += "cbrb_postdate, "
                    SqlStatement += "cbrb_source, "
                    SqlStatement += "cbrb_nprefid, "
                    SqlStatement += "cbrb_who, "
                    SqlStatement += "cbrb_dtime, "
                    SqlStatement += "cbrb_ldtime) "
                    SqlStatement += "VALUES "
                    SqlStatement += "('" & .cbrb_batchid & "', "
                    SqlStatement += "'" & .cbrb_bank & "', "
                    SqlStatement += "'" & .cbrb_company & "', "
                    SqlStatement += "'" & .cbrb_batchtot & "', "
                    SqlStatement += "'" & .cbrb_curcode & "', "
                    SqlStatement += "" & .cbrb_currate & ", "
                    SqlStatement += "" & .cbrb_baseunit & ", "
                    SqlStatement += "'" & .cbrb_operator & "', "
                    SqlStatement += "'" & .cbrb_status & "', "
                    SqlStatement += "DateTime(" & .cbrb_batchdate & ") YEAR TO SECOND, "
                    SqlStatement += "'" & .cbrb_usrctltot & "', "
                    SqlStatement += "" & .cbrb_postdate & ", "
                    SqlStatement += "'" & .cbrb_source & "', "
                    SqlStatement += "" & .cbrb_nprefid & ", "
                    SqlStatement += "'" & .cbrb_who & "', "
                    SqlStatement += "" & .cbrb_dtime & ", "
                    SqlStatement += "" & .cbrb_ldtime & ")"
                End With

                If _DatabaseFactory.ExecuteSqlStatement(Helper.GetDataBaseType, DataBaseConnectionString, SqlStatement) > -1 Then
                    Return True
                End If

                Return False

            Catch ex As Exception
                Throw ex
            End Try

        End Function

#End Region

#Region "InsertReceipt_CBRcptHdr "

        Public Function InsertReceipt_CBRcptHdr(ByVal CB_RcptHdrEn As BusinessEntities.CB_RcptHdrEn) As Boolean

            Try

                With CB_RcptHdrEn
                    SqlStatement = "INSERT INTO cb_rcpthdr "
                    SqlStatement += "(cbrh_company, "
                    SqlStatement += "cbrh_batchid, "
                    SqlStatement += "cbrh_lineno, "
                    SqlStatement += "cbrh_rcptid, "
                    SqlStatement += "cbrh_refno, "
                    SqlStatement += "cbrh_desc, "
                    SqlStatement += "cbrh_payer, "
                    SqlStatement += "cbrh_payeraddr1, "
                    SqlStatement += "cbrh_payeraddr2, "
                    SqlStatement += "cbrh_payeraddr3, "
                    SqlStatement += "cbrh_payeraddr4, "
                    SqlStatement += "cbrh_cashtype, "
                    SqlStatement += "cbrh_frombank, "
                    SqlStatement += "cbrh_frombranch, "
                    SqlStatement += "cbrh_amount, "
                    SqlStatement += "cbrh_lclamount, "
                    SqlStatement += "cbrh_magic) "
                    SqlStatement += "VALUES "
                    SqlStatement += "('" & .cbrh_company & "', "
                    SqlStatement += "'" & .cbrh_batchid & "', "
                    SqlStatement += "" & .cbrh_lineno & ", "
                    SqlStatement += "'" & .cbrh_rcptid & "', "
                    SqlStatement += "'" & .cbrh_refno & "', "
                    SqlStatement += "'" & .cbrh_desc & "', "
                    SqlStatement += "'" & .cbrh_payer & "', "
                    SqlStatement += "'" & .cbrh_payeraddr1 & "', "
                    SqlStatement += "'" & .cbrh_payeraddr2 & "', "
                    SqlStatement += "'" & .cbrh_payeraddr3 & "', "
                    SqlStatement += "" & .cbrh_payeraddr4 & ", "
                    SqlStatement += "'" & .cbrh_cashtype & "', "
                    SqlStatement += "'" & .cbrh_frombank & "', "
                    SqlStatement += "" & .cbrh_frombranch & ", "
                    SqlStatement += "'" & .cbrh_amount & "', "
                    SqlStatement += "'" & .cbrh_lclamount & "', "
                    SqlStatement += "" & .cbrh_magic & ")"
                End With

                If _DatabaseFactory.ExecuteSqlStatement(Helper.GetDataBaseType, DataBaseConnectionString, SqlStatement) > -1 Then
                    Return True
                End If

                Return False

            Catch ex As Exception
                Throw ex
            End Try

        End Function

#End Region

#Region "InsertPayment_GLDist "

        Public Function InsertPayment_GLDist(ByVal GL_DistEn As BusinessEntities.GL_DistEn) As Boolean

            Try

                With GL_DistEn
                    SqlStatement = "INSERT INTO gl_dist  "
                    SqlStatement += "(gldi_company, "
                    SqlStatement += "gldi_module, "
                    SqlStatement += "gldi_itemtype, "
                    SqlStatement += "gldi_batchid, "
                    SqlStatement += "gldi_code, "
                    SqlStatement += "gldi_itemref, "
                    SqlStatement += "gldi_itemrefline, "
                    SqlStatement += "gldi_seqno, "
                    SqlStatement += "gldi_glac, "
                    SqlStatement += "gldi_desc, "
                    SqlStatement += "gldi_qty, "
                    SqlStatement += "gldi_amount, "
                    SqlStatement += "gldi_posted, "
                    SqlStatement += "gldi_modify, "
                    SqlStatement += "gldi_serial, "
                    SqlStatement += "gldi_curcode, "
                    SqlStatement += "gldi_leqfor, "
                    SqlStatement += "gldi_currate, "
                    SqlStatement += "gldi_baseunit, "
                    SqlStatement += "gldi_operator, "
                    SqlStatement += "gldi_lclamount) "
                    SqlStatement += "VALUES "
                    SqlStatement += "('" & .gldi_company & "', "
                    SqlStatement += "'" & .gldi_module & "', "
                    SqlStatement += "'" & .gldi_itemtype & "', "
                    SqlStatement += "'" & .gldi_batchid & "', "
                    SqlStatement += "'" & .gldi_code & "', "
                    SqlStatement += "'" & .gldi_itemref & "', "
                    SqlStatement += "" & .gldi_itemrefline & ", "
                    SqlStatement += "" & .gldi_seqno & ", "
                    SqlStatement += "'" & .gldi_glac & "', "
                    SqlStatement += "'" & .gldi_desc & "', "
                    SqlStatement += "" & .gldi_qty & ", "
                    SqlStatement += "'" & .gldi_amount & "', "
                    SqlStatement += "'" & .gldi_posted & "', "
                    SqlStatement += "" & .gldi_modify & ", "
                    SqlStatement += "" & .gldi_serial & ", "
                    SqlStatement += "'" & .gldi_curcode & "', "
                    SqlStatement += "'" & .gldi_leqfor & "', "
                    SqlStatement += "" & .gldi_currate & ", "
                    SqlStatement += "" & .gldi_baseunit & ", "
                    SqlStatement += "'" & .gldi_operator & "', "
                    SqlStatement += "'" & .gldi_lclamount & "')"
                End With

                If _DatabaseFactory.ExecuteSqlStatement(Helper.GetDataBaseType, DataBaseConnectionString, SqlStatement) > -1 Then
                    Return True
                End If

                Return False

            Catch ex As Exception
                Throw ex
            End Try

        End Function

#End Region

#Region "UpdatePostedData "

        Public Function UpdatePostedData(ByVal AccountsEn As BusinessEntities.AccountsEn) As Boolean

            Try

                With AccountsEn
                    SqlStatement = "UPDATE SAS_Accounts SET "
                    SqlStatement += "PostToCF = '1' "
                    SqlStatement += "WHERE BatchCode = '" & .BatchCode & "' "
                    SqlStatement += "AND PostStatus = 'POSTED' "
                End With

                Dim cmd As New SqlCommand(SqlStatement)
                'cmd.Parameters.Add(New SqlParameter("@SearchCriteria", .SearchCriteria))

                If _DatabaseFactory.ExecuteSqlStatement(Helper.GetDataBaseType, DataBaseConnectionString, SqlStatement) > -1 Then
                    Return True
                End If

                Return False

            Catch ex As Exception
                Throw ex
            End Try

        End Function

#End Region

#Region "GetCompanyCode "

        Public Function GetCompanyCode() As String

            Dim strCompanyCode As String = String.Empty

            Try

                SqlStatement = "select SAUP_Code from SAS_UniversityProfile "

                _DataSet = _DatabaseFactory.ExecuteDataSet(Helper.GetDataBaseType, DataBaseConnectionString, SqlStatement)

                strCompanyCode = clsGeneric.NullToString(_DataSet.Tables(0).Rows(0).Item("SAUP_Code"))

                If FormHelp.IsBlank(strCompanyCode) Then
                    Throw New System.Exception("No Company Code Defined")
                Else

                End If

                Return strCompanyCode

            Catch ex As Exception
                Throw ex
            End Try



        End Function

#End Region

#Region "GetGLDesc "

        Public Function GetGLDesc(ByVal strGLCode As String) As String

            Dim strGLDesc As String = String.Empty

            Try

                SqlStatement = "SELECT glac_desc FROM gl_account WHERE glac_account = '" & strGLCode & "'"

                _DataSet = _DatabaseFactory.ExecuteDataSet(Helper.GetDataBaseType, DataBaseConnectionString, SqlStatement)

                If _DataSet.Tables(0).Rows.Count > 0 Then
                    strGLDesc = clsGeneric.NullToString(_DataSet.Tables(0).Rows(0).Item("glac_desc"))
                Else
                    SqlStatement = "SELECT pcac_desc FROM pc_account WHERE pcac_account = '" & strGLCode & "'"

                    _DataSet = _DatabaseFactory.ExecuteDataSet(Helper.GetDataBaseType, DataBaseConnectionString, SqlStatement)

                    If _DataSet.Tables(0).Rows.Count > 0 Then
                        strGLDesc = clsGeneric.NullToString(_DataSet.Tables(0).Rows(0).Item("pcac_desc"))
                    End If
                End If

                Return strGLDesc

            Catch ex As Exception
                Throw ex
            End Try

        End Function

#End Region

#Region "GetMagicNum "

        Public Function GetMagicNum(ByVal strProcess As String) As String

            Dim intMagicNum As Integer

            Try
                Select Case strProcess
                    Case "Payment"
                        SqlStatement = "SELECT MAX(cbph_magic) magicNum from cb_payhdr"
                    Case "Receipt"
                        SqlStatement = "SELECT MAX(cbrh_magic) magicNum FROM CB_RcptHdr"
                End Select

                _DataSet = _DatabaseFactory.ExecuteDataSet(Helper.GetDataBaseType, DataBaseConnectionString, SqlStatement)

                If _DataSet.Tables(0).Rows.Count > 0 Then
                    intMagicNum = clsGeneric.NullToInteger(_DataSet.Tables(0).Rows(0).Item("magicNum")) + 1
                End If

                Return intMagicNum

            Catch ex As Exception
                Throw ex
            End Try

        End Function

#End Region

#Region "GetSerialNum "

        Public Function GetSerialNum() As String

            Dim strSerialNum As String = String.Empty

            Try

                SqlStatement = "SELECT MAX(gldi_serial) SerialNum FROM gl_dist"

                _DataSet = _DatabaseFactory.ExecuteDataSet(Helper.GetDataBaseType, DataBaseConnectionString, SqlStatement)
                If _DataSet.Tables(0).Rows.Count > 0 Then
                    strSerialNum = clsGeneric.NullToInteger(_DataSet.Tables(0).Rows(0).Item("SerialNum")) + 1
                End If

                Return strSerialNum

            Catch ex As Exception
                Throw ex
            End Try

        End Function

#End Region

#Region "GetLedgerType "

        Public Function GetLedgerType(ByVal strGLAccount As String) As String

            Dim strLedgerType As String = String.Empty

            Try

                SqlStatement = "select glac_account, glac_desc from gl_account where  glac_account = '" & strGLAccount & "'"

                _DataSet = _DatabaseFactory.ExecuteDataSet(Helper.GetDataBaseType, DataBaseConnectionString, SqlStatement)

                If _DataSet.Tables(0).Rows.Count > 0 Then
                    strLedgerType = "G"
                Else
                    strLedgerType = "P"
                End If

                Return strLedgerType

            Catch ex As Exception
                Throw ex
            End Try



        End Function

#End Region

#Region "GetAutoNumber "

        Public Function GetAutoNumber(ByVal strAutoCode As String) As String

            Dim strAutoNo As String = ""
            Dim CurNo As Integer = 0
            Dim NoDigit As Integer = 0
            Dim AutoCode As Integer = 0
            Dim i As Integer = 0

            Try

                SqlStatement = "SELECT * FROM SAS_AutoNumber WHERE SAAN_Code = '" & strAutoCode & "' "

                _DataSet = _DatabaseFactory.ExecuteDataSet(Helper.GetDataBaseType, DataBaseConnectionString, SqlStatement)

                With _DataSet.Tables(0).Rows(0)
                    AutoCode = clsGeneric.NullToInteger(.Item("SAAN_Code"))
                    If IsDBNull(.Item("SAAN_CurNo")) Then
                        CurNo = 1
                    Else
                        CurNo = clsGeneric.NullToInteger(.Item("SAAN_CurNo")) + 1
                    End If
                    NoDigit = clsGeneric.NullToInteger(.Item("SAAN_NoDigit"))
                    strAutoNo = .Item("SAAN_Prefix").ToString()
                End With

                If CurNo.ToString().Length < NoDigit Then
                    While i < NoDigit - CurNo.ToString().Length
                        strAutoNo = strAutoNo & "0"
                        i = i + 1
                    End While
                    strAutoNo = strAutoNo + CurNo.ToString
                End If

                Dim AutoNumberEn As New BusinessEntities.AutoNumberEn

                AutoNumberEn.SAAN_Code = AutoCode
                AutoNumberEn.SAAN_CurNo = CurNo
                AutoNumberEn.SAAN_AutoNo = strAutoNo

                UpdateAutoNumber(AutoNumberEn, strAutoCode)

                Return strAutoNo

            Catch ex As Exception
                Throw ex
            End Try

        End Function

#End Region

#Region "UpdateAutoNumber "

        Public Function UpdateAutoNumber(ByVal AutoNumber As BusinessEntities.AutoNumberEn, ByVal strAutoCode As String) As Boolean

            Try

                With AutoNumber
                    SqlStatement = "UPDATE SAS_AutoNumber SET "
                    SqlStatement += "SAAN_CurNo = '" & .SAAN_CurNo & "', "
                    SqlStatement += "SAAN_AutoNo = '" & .SAAN_AutoNo & "' "
                    SqlStatement += "WHERE SAAN_Code = '" & strAutoCode & "' "
                End With

                If _DatabaseFactory.ExecuteSqlStatement(Helper.GetDataBaseType, DataBaseConnectionString, SqlStatement) > -1 Then
                    Return True
                End If

                Return False

            Catch ex As Exception
                Throw ex
            End Try

        End Function

#End Region

    End Class
End Namespace

