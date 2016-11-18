#Region "Name Spaces "

Imports MaxGeneric
Imports System.Reflection
Imports System.Data.Common

#End Region

Public Class CfPosting

#Region "Global Declarations "

    'Create Instances - Start
    Private _CfCommon As New CfCommon()
    Private _CfGeneric As New CfGeneric()
    Private _DataBaseProvider As New DatabaseProvider()
    'Create Instances - Stop

#End Region

#Region "Execute Sql Statement "

    'Purpose			: To Execute the Sql Statement(s)
    'Author			    : Sujith Sharatchandran - T-Melmax Sdn Bhd
    'Created Date		: 31/05/2015
    Private Function ExecuteSqlStatement(ByVal SqlStatement As String) As Boolean

        Try

            If _DataBaseProvider.ExecuteSqlStatement(Helper.
                GetFinancialsDataBaseType, Helper.FinancialsConnectionString,
                SqlStatement) > -1 Then
                Return True
            End If

            Return False

        Catch ex As Exception

            'log error
            Call Helper.LogError(ex.Message)

            Return False

        End Try

    End Function

#End Region

#Region "Get Data Reader "

    'Purpose			: To Get Sql Statement as Data Reader
    'Author			    : Sujith Sharatchandran - T-Melmax Sdn Bhd
    'Created Date		: 31/05/2015
    Private Function GetDataReader(ByVal SqlStatement As String) As IDataReader

        Try

            Return _DataBaseProvider.ExecuteReader(Helper.GetDataBaseType,
                Helper.GetConnectionString, SqlStatement).CreateDataReader()

        Catch ex As Exception

            'log error
            Call Helper.LogError(ex.Message)

            Return Nothing

        End Try

    End Function

#End Region

#Region "Cash Book Payment "

    'Purpose			: To Post Cash Book Payment
    'Author			    : Sujith Sharatchandran - T-Melmax Sdn Bhd
    'Created Date		: 29/05/2015
    Public Function CashBookPayment(ByVal TransactionId As Integer, ByVal BatchCode As String, ByRef CashBookNo As String) As Boolean

        'Variable Declarations - Start
        'Dim CashBookNo As String = Nothing
        Dim CfCbPayBatchSql As String = Nothing, CompanyCode As String = Nothing
        'Variable Declarations - Stop

        Try

            'Get Company Code
            'CompanyCode = _CfCommon.GetCompanyCode()
            CompanyCode = "UPM"

            'Get Auto Number
            CashBookNo = _CfCommon.GetAutoNumber(CfGeneric.PostingType.CashBookPayment)

            'Post Cash Book Payment Batch - Start
            If Not CashBookPaymentBatch(TransactionId, BatchCode, CompanyCode, CashBookNo) Then
                'Roll Back Transactions
                Call RollBackCashBookPayment(BatchCode, CashBookNo)

                Return False

            Else

                'Increment Auto No
                Call _CfCommon.UpdateAutoNumber(CfGeneric.PostingType.CashBookPayment)

                Return True

            End If
            'Post Cash Book Payment Batch - Stop

        Catch ex As Exception

            'log error
            Call Helper.LogError(ex.Message)

            Return False

        End Try

    End Function

#End Region

#Region "Cash Book Payment Batch "

    'Purpose			: To Post Cash Book Payment Batch
    'Author			    : Sujith Sharatchandran - T-Melmax Sdn Bhd
    'Created Date		: 29/05/2015
    Private Function CashBookPaymentBatch(ByVal TransactionId As Integer, ByVal BatchCode As String,
        ByVal CompanyCode As String, ByVal CashBookNo As String) As Boolean

        'Create Instances - Start
        Dim BatchDetails As IDataReader = Nothing
        Dim _CfCbPayBatchEn As New CfCbPayBatchEn()
        'Create Instances - Stop

        'Variable Declarations
        Dim SqlStatement As String = Nothing, CfCbPayBatchSql As String = Nothing

        Try

            'Buils Sql Statement - Start
            SqlStatement = "SELECT SUM(SAS_Accounts.transamount) AS Batch_Amount, SUM(SAS_Accounts.batchtotal) AS Batch_Total,"
            SqlStatement &= "SAS_Accounts.paymentmode AS Payment_Mode,SAS_Accounts.BankCode AS Bank_Code,sas_workflow.user_id AS Posted_By, sas_workflow.date_time AS Posted_Date"
            SqlStatement &= " FROM  SAS_Accounts INNER JOIN sas_workflow ON SAS_Accounts.batchcode = sas_workflow.batch_code "
            SqlStatement &= " WHERE SAS_Accounts.batchcode = " & clsGeneric.AddQuotes(BatchCode)
            SqlStatement &= " GROUP BY SAS_Accounts.paymentmode,SAS_Accounts.BankCode,sas_workflow.user_id,sas_workflow.date_time"
            'Buils Sql Statement - Stop

            'get Batch Details
            BatchDetails = GetDataReader(SqlStatement)

            'if batch details available - Start
            If BatchDetails.Read Then

                'Set Values - Start
                _CfCbPayBatchEn.cbpb_batchid = CashBookNo
                _CfCbPayBatchEn.cbpb_company = CompanyCode
                _CfCbPayBatchEn.cbpb_who = BatchDetails(Helper.PostedByCol)
                _CfCbPayBatchEn.cbpb_bank = BatchDetails(Helper.BankCodeCol)
                _CfCbPayBatchEn.cbpb_type = BatchDetails(Helper.PaymentModeCol)
                _CfCbPayBatchEn.cbpb_usrctltot = BatchDetails(Helper.BatchTotalCol)
                _CfCbPayBatchEn.cbpb_batchtot = BatchDetails(Helper.BatchTotalCol)
                _CfCbPayBatchEn.cbpb_batchdate = CfGeneric.DateConversion(BatchDetails(Helper.PostedDateCol))
                'Set Values - Stop

                'Build Sql Statement - Start
                Call _CfGeneric.BuildSqlStatement(_CfCbPayBatchEn,
                    CfCbPayBatchSql, CfGeneric.CfCbPayBatchTbl)
                'Build Sql Statement - Stop

                'if posted successfully - Start
                If ExecuteSqlStatement(CfCbPayBatchSql) Then

                    'Build Cash Book Payment Header - Start
                    Return CashBookPaymentHeader(TransactionId, BatchCode,
                         CompanyCode, CashBookNo)
                    'Build Cash Book Payment Header - Stop

                End If
                'if posted successfully - Stop

            End If
            'if batch details available - Stop

            Return False

        Catch ex As Exception

            'log error
            Call Helper.LogError(ex.Message)

            Return False

        End Try

    End Function

#End Region

#Region "Cash Book Payment Header "

    'Purpose			: To Post Cash Book Payment Header
    'Author			    : Sujith Sharatchandran - T-Melmax Sdn Bhd
    'Created Date		: 31/05/2015
    Private Function CashBookPaymentHeader(ByVal TransactionId As Integer, ByVal BatchCode As String,
        ByVal CompanyCode As String, ByVal CashBookNo As String) As Boolean

        'Create Instances
        Dim HeaderDetails As IDataReader = Nothing

        'Variable Declarations - Start
        Dim CfCbPayDetailsSql As String = Nothing
        Dim LineNo As Integer = 0, TransDate As String = Nothing, TransAmount As Decimal = 0
        Dim TransCode As String = Nothing, PayeeName As String = Nothing, BankCode As String = Nothing, MatricNo As String = Nothing
        Dim SqlStatement As String = Nothing, CfCbPayHeaderSql As String = Nothing, MagicNo As Integer = 0
        'Dim SqlVoucherNo As String = Nothing
        'Variable Declarations - Stop

        Try

            'SqlVoucherNo = "SELECT length(SAS_Accounts.voucherno) as lenVN FROM SAS_Accounts where batchcode = " & clsGeneric.AddQuotes(BatchCode)

            'Buils Sql Statement - Start
            SqlStatement = "SELECT ROW_NUMBER() OVER (ORDER BY SAS_Accounts.paymentmode) AS Row_No, SUM(SAS_Accounts.transamount) AS Trans_Amount,"
            SqlStatement &= "SUM(SAS_Accounts.batchtotal) AS Batch_Total,rtrim(SAS_Accounts.voucherno) AS Trans_Code, SAS_Accounts.paymentmode AS Payment_Mode,SAS_Accounts.PayeeName AS Payee_Name,"
            'SqlStatement &= "SUM(SAS_Accounts.batchtotal) AS Batch_Total,(left(rtrim(SAS_Accounts.voucherno), 2) || right(rtrim(SAS_Accounts.voucherno), 10)) AS Trans_Code, SAS_Accounts.paymentmode AS Payment_Mode,SAS_Accounts.PayeeName AS Payee_Name,"
            SqlStatement &= "sas_workflow.user_id AS Posted_By,sas_workflow.date_time AS Posted_Date,SAS_Accounts.bankcode AS Bank_Code,SAS_Accounts.CreditRef AS Matric_No FROM  SAS_Accounts INNER JOIN sas_workflow "
            SqlStatement &= "ON SAS_Accounts.batchcode = sas_workflow.batch_code WHERE SAS_Accounts.batchcode = " & clsGeneric.AddQuotes(BatchCode)
            SqlStatement &= " GROUP BY SAS_Accounts.voucherno,SAS_Accounts.paymentmode,SAS_Accounts.PayeeName,sas_workflow.user_id,sas_workflow.date_time,SAS_Accounts.bankcode,SAS_Accounts.CreditRef"
            'Buils Sql Statement - Stop

            'get Header Details
            HeaderDetails = GetDataReader(SqlStatement)

            'if batch details available - Start
            While HeaderDetails.Read

                'Initialise Cash Book Header Entity
                Dim _CfCbPayHeaderEn As New CfCbPayHeaderEn()

                'Get Values - Start
                LineNo = HeaderDetails(Helper.LineNoCol)
                BankCode = HeaderDetails(Helper.BankCodeCol)
                TransCode = HeaderDetails(Helper.TransCodeCol)
                TransDate = CfGeneric.DateConversion(HeaderDetails(Helper.PostedDateCol))
                PayeeName = HeaderDetails(Helper.PayeeNameCol)
                TransAmount = HeaderDetails(Helper.TransAmountCol)
                MatricNo = HeaderDetails(Helper.MatricNoCol)
                MagicNo = _CfCommon.GetPaymentHeaderMagicNo()
                'Get Values - Stop

                'Set Values - Start
                _CfCbPayHeaderEn.cbph_lineno = LineNo
                _CfCbPayHeaderEn.cbph_payee = PayeeName
                _CfCbPayHeaderEn.cbph_bankcode = BankCode
                _CfCbPayHeaderEn.cbph_voucher = TransCode
                _CfCbPayHeaderEn.cbph_amount = TransAmount
                _CfCbPayHeaderEn.cbph_batchid = CashBookNo
                _CfCbPayHeaderEn.cbph_company = CompanyCode
                _CfCbPayHeaderEn.cbph_magic = MagicNo
                'Set Values - Stop

                'Build Sql Statement - Start
                If _CfGeneric.BuildSqlStatement(_CfCbPayHeaderEn,
                    CfCbPayHeaderSql, CfGeneric.CfCbPayHeaderTbl) Then

                    'if record successfully posted - Start
                    If ExecuteSqlStatement(CfCbPayHeaderSql) Then

                        'Post Cash Book Payment Details - Start
                        If CashBookPaymentDetail(BatchCode, CompanyCode, CashBookNo,
                            TransCode, TransDate, TransAmount, LineNo, MagicNo) Then

                            'Cash Book Payment Gl Distribution - Start
                            'If Not CashBookGlDist(BatchCode, CompanyCode, CashBookNo,
                            '    TransCode, TransDate, TransAmount, LineNo, MagicNo, "") Then
                            If Not CashBookGlDist(TransactionId, LineNo, CashBookNo, CompanyCode,
                                TransCode, TransAmount, BankCode, MatricNo) Then
                                Return False
                            End If
                            'Cash Book Payment Gl Distribution - Stop

                        Else
                            Return False
                        End If
                        'Post Cash Book Payment Details - Stop

                    Else
                        Return False
                    End If
                    'if record successfully posted - Stop

                End If
                'Build Sql Statement - Stop

            End While
            'if batch details available - Stop

            Return True

        Catch ex As Exception

            'log error
            Call Helper.LogError(ex.Message)

            Return False

        End Try

    End Function

#End Region

#Region "Cash Book Payment Details "

    'Purpose			: To Post Cash Book Payment Details
    'Author			    : Sujith Sharatchandran - T-Melmax Sdn Bhd
    'Created Date		: 31/05/2015
    Private Function CashBookPaymentDetail(ByVal BatchCode As String,
        ByVal CompanyCode As String, ByVal CashBookNo As String, ByVal TransCode As String,
        ByVal TransDate As String, ByVal TransAmount As Decimal, ByVal LineNo As Integer,
        ByVal MagicNo As Integer) As Boolean

        'Create Instances
        Dim _CfCbPayDetailsEn As New CfCbPayDetailsEn()

        'Variable Declarations
        Dim CfCbPayDetailsSql As String = Nothing

        Try

            'Set Values - Start
            _CfCbPayDetailsEn.cbpr_magic = MagicNo
            _CfCbPayDetailsEn.cbpr_lineno = LineNo
            _CfCbPayDetailsEn.cbpr_itemdate = TransDate
            _CfCbPayDetailsEn.cbpr_itemrefno = TransCode
            _CfCbPayDetailsEn.cbpr_rmtamount = TransAmount
            _CfCbPayDetailsEn.cbpr_itemamount = TransAmount
            'Set Values - Stop

            'Build Sql Statement - Start
            If _CfGeneric.BuildSqlStatement(_CfCbPayDetailsEn,
                CfCbPayDetailsSql, CfGeneric.CfCbPayDetailsTbl) Then
                Return ExecuteSqlStatement(CfCbPayDetailsSql)
            End If
            'Build Sql Statement - Stop

        Catch ex As Exception

            'log error
            Call Helper.LogError(ex.Message)

            Return False

        End Try

    End Function

#End Region

#Region "Roll Back Cash Book Payment "

    'Purpose			: To Roll Back Cash Book Payment 
    'Author			    : Sujith Sharatchandran - T-Melmax Sdn Bhd
    'Created Date		: 31/05/2015
    Private Sub RollBackCashBookPayment(ByVal BatchCode As String, ByVal CashBookNo As String)

        'Variable Declarations
        Dim SqlStatement As String = Nothing

        Try

            'Build Delete Sql Statement for Cash Book Payment - Start
            SqlStatement = "DELETE FROM " & CfGeneric.CfCbGlDistTbl & " WHERE "
            SqlStatement &= "gldi_batchid = " & clsGeneric.AddQuotes(CashBookNo) & ";"
            SqlStatement &= " DELETE FROM " & CfGeneric.CfCbPayDetailsTbl & " WHERE cbpr_magic "
            SqlStatement &= " IN (SELECT cbph_magic FROM " & CfGeneric.CfCbPayHeaderTbl & " WHERE "
            SqlStatement &= " cbph_batchid = " & clsGeneric.AddQuotes(CashBookNo) & ");"
            SqlStatement &= " DELETE FROM " & CfGeneric.CfCbPayHeaderTbl & " WHERE "
            SqlStatement &= " cbph_batchid = " & clsGeneric.AddQuotes(CashBookNo) & ";"
            SqlStatement &= " DELETE FROM " & CfGeneric.CfCbPayBatchTbl & " WHERE "
            SqlStatement &= " cbpb_batchid = " & clsGeneric.AddQuotes(CashBookNo) & ";"
            'Build Delete Sql Statement for Cash Book Payment - Stop

            'Execute Sql Statement
            Call ExecuteSqlStatement(SqlStatement)

        Catch ex As Exception

            'log error
            Call Helper.LogError(ex.Message)

        End Try

    End Sub

#End Region

#Region "Cash Book Receipt "

    'Purpose			: To Post Cash Book Receipt
    'Author			    : Sujith Sharatchandran - T-Melmax Sdn Bhd
    'Created Date		: 31/05/2015
    Public Function CashBookReceipt(ByVal TransactionId As Integer,
        ByVal BatchCode As String, ByRef CashBookNo As String) As Boolean

        'Variable Declarations - Start
        'Dim CashBookNo As String = Nothing
        Dim CfCbPayBatchSql As String = Nothing, CompanyCode As String = Nothing
        'Variable Declarations - Stop

        Try

            'Get Company Code
            CompanyCode = _CfCommon.GetCompanyCode()

            'Get Auto Number
            CashBookNo = _CfCommon.GetAutoNumber(CfGeneric.PostingType.CashBookReceipt)

            'Post Cash Book Receipt Batch - Start
            If Not CashBookReceiptBatch(TransactionId, BatchCode, CompanyCode, CashBookNo) Then
                'Roll Back Transactions
                Call RollBackCashBookReceipt(BatchCode, CashBookNo)

                Return False

            Else

                'Increment Auto No
                Call _CfCommon.UpdateAutoNumber(CfGeneric.PostingType.CashBookReceipt)

                Return True

            End If
            'Post Cash Book Receipt Batch - Stop

        Catch ex As Exception

            'log error
            Call Helper.LogError(ex.Message)

            Return False

        End Try

    End Function

#End Region

#Region "Cash Book Receipt Batch "

    'Purpose			: To Post Cash Book Receipt
    'Author			    : Sujith Sharatchandran - T-Melmax Sdn Bhd
    'Created Date		: 29/05/2015
    Private Function CashBookReceiptBatch(ByVal TransactionId As Integer, ByVal BatchCode As String,
        ByVal CompanyCode As String, ByVal CashBookNo As String) As Boolean

        'Create Instances - Start
        Dim BatchDetails As IDataReader = Nothing
        Dim _CfCbRecBatchEn As New CfCbRecBatchEn()
        'Create Instances - Stop

        'Variable Declarations
        Dim SqlStatement As String = Nothing, CfCbRecBatchSql As String = Nothing

        Try

            'Buils Sql Statement - Start
            SqlStatement &= "SELECT SAS_Accounts.TransID AS Trans_Id,SAS_Accounts.BankCode AS Bank_Code,"
            SqlStatement &= "SAS_Accounts.BatchTotal AS Batch_Total,SAS_Accounts.TransDate AS Batch_Date,"
            SqlStatement &= "sas_workflow.date_time AS Posted_Date,sas_workflow.user_id AS Posted_By"
            SqlStatement &= " FROM SAS_Accounts INNER JOIN sas_workflow ON SAS_Accounts.batchcode = sas_workflow.batch_code"
            SqlStatement &= " WHERE SAS_Accounts.TransID = " & TransactionId
            'Buils Sql Statement - Stop

            'get Batch Details
            BatchDetails = GetDataReader(SqlStatement)

            'if batch details available - Start
            If BatchDetails.Read Then

                'Set Values - Start
                _CfCbRecBatchEn.cbrb_batchid = CashBookNo
                _CfCbRecBatchEn.cbrb_company = CompanyCode
                _CfCbRecBatchEn.cbrb_who = BatchDetails(Helper.PostedByCol)
                _CfCbRecBatchEn.cbrb_bank = BatchDetails(Helper.BankCodeCol)
                _CfCbRecBatchEn.cbrb_batchtot = BatchDetails(Helper.BatchTotalCol)
                _CfCbRecBatchEn.cbrb_batchdate = CfGeneric.DateConversion(BatchDetails(Helper.BatchDateCol))
                _CfCbRecBatchEn.cbrb_postdate = CfGeneric.DateConversion(BatchDetails(Helper.PostedDateCol))
                _CfCbRecBatchEn.cbrb_usrctltot = BatchDetails(Helper.BatchTotalCol)
                'Set Values - Stop

                'Build Sql Statement - Start
                Call _CfGeneric.BuildSqlStatement(_CfCbRecBatchEn,
                    CfCbRecBatchSql, CfGeneric.CfCbRecBatchTbl)
                'Build Sql Statement - Stop

                'if posted successfully - Start
                If ExecuteSqlStatement(CfCbRecBatchSql) Then

                    'Build Cash Book Payment Header - Start
                    Return CashBookReceiptHeader(TransactionId, BatchCode,
                         CompanyCode, CashBookNo)
                    'Build Cash Book Payment Header - Stop

                End If
                'if posted successfully - Stop

            End If
            'if batch details available - Stop

            Return False

        Catch ex As Exception

            'log error
            Call Helper.LogError(ex.Message)

            Return False

        End Try

    End Function

#End Region

#Region "Cash Book Receipt Header "

    'Purpose			: To Post Cash Book Receipt Header
    'Author			    : Sujith Sharatchandran - T-Melmax Sdn Bhd
    'Created Date		: 01/06/2015
    Private Function CashBookReceiptHeader(ByVal TransactionId As Integer, ByVal BatchCode As String,
        ByVal CompanyCode As String, ByVal CashBookNo As String) As Boolean

        'Create Instances
        Dim HeaderDetails As IDataReader = Nothing

        'Variable Declarations - Start
        Dim PaymentMode As String = Nothing, MatricNo As String = Nothing
        Dim LineNo As Integer = 0, TransAmount As Decimal = 0, Reference1 As String = Nothing
        Dim TransCode As String = Nothing, PayeeName As String = Nothing, BankCode As String = Nothing
        Dim SqlStatement As String = Nothing, CfCbRecHeaderSql As String = Nothing, MagicNo As Integer = 0
        'Variable Declarations - Stop

        Try

            'Buils Sql Statement - Start
            SqlStatement = "SELECT ROW_NUMBER() OVER (ORDER BY SAS_Accounts.TransID) AS Row_No,SAS_Accounts.TransID As Trans_Id,"
            SqlStatement &= "SAS_AccountsDetails.TransCode AS Trans_Code,SAS_Student.SASI_Name AS Payee_Name,"
            SqlStatement &= "SAS_AccountsDetails.TransAmount AS Trans_Amount, SAS_Accounts.BankCode AS Bank_Code,"
            SqlStatement &= "SAS_AccountsDetails.Ref1 AS Reference_1, SUBSTRING(SAS_Accounts.PaymentMode,1,3) AS Payment_Mode "
            SqlStatement &= ",SAS_Accounts.CreditRef AS Matric_No FROM SAS_Accounts "
            SqlStatement &= "INNER JOIN SAS_AccountsDetails ON SAS_Accounts.TransID = SAS_AccountsDetails.TransID "
            SqlStatement &= "INNER JOIN SAS_Student ON SAS_Accounts.CreditRef = SAS_Student.SASI_MatricNo "
            SqlStatement &= "WHERE SAS_AccountsDetails.Transid = " & TransactionId
            'Buils Sql Statement - Stop

            'get Header Details
            HeaderDetails = GetDataReader(SqlStatement)

            'if batch details available - Start
            While HeaderDetails.Read

                'Initialise Cash Book Header Entity
                Dim _CfCbRecHeaderEn As New CfCbRecHeaderEn()

                'Get Values - Start
                LineNo = HeaderDetails(Helper.LineNoCol)
                MatricNo = HeaderDetails(Helper.MatricNoCol)
                BankCode = HeaderDetails(Helper.BankCodeCol)
                TransCode = HeaderDetails(Helper.TransCodeCol)
                PayeeName = HeaderDetails(Helper.PayeeNameCol)
                Reference1 = HeaderDetails(Helper.Reference1Col)
                TransAmount = HeaderDetails(Helper.TransAmountCol)
                PaymentMode = HeaderDetails(Helper.PaymentModeCol)
                'Get Values - Stop

                'Set Values - Start
                _CfCbRecHeaderEn.cbrh_lineno = LineNo
                _CfCbRecHeaderEn.cbrh_refno = TransCode
                '_CfCbRecHeaderEn.cbrh_payer = PayeeName
                _CfCbRecHeaderEn.cbrh_payer = PayeeName.Replace("'", "''")
                _CfCbRecHeaderEn.cbrh_rcptid = Reference1
                _CfCbRecHeaderEn.cbrh_frombank = BankCode
                _CfCbRecHeaderEn.cbrh_amount = TransAmount
                _CfCbRecHeaderEn.cbrh_batchid = CashBookNo
                _CfCbRecHeaderEn.cbrh_company = CompanyCode
                _CfCbRecHeaderEn.cbrh_cashtype = PaymentMode
                _CfCbRecHeaderEn.cbrh_lclamount = TransAmount
                _CfCbRecHeaderEn.cbrh_desc = "SAS Batch Id " & BatchCode
                _CfCbRecHeaderEn.cbrh_magic = _CfCommon.GetReceiptHeaderMagicNo()
                'Set Values - Stop

                'Build Sql Statement - Start
                If _CfGeneric.BuildSqlStatement(_CfCbRecHeaderEn,
                    CfCbRecHeaderSql, CfGeneric.CfCbRecHeaderTbl) Then

                    'if record successfully posted - Start
                    If ExecuteSqlStatement(CfCbRecHeaderSql) Then

                        'Clear Sql Statement
                        CfCbRecHeaderSql = String.Empty

                        'Cash Book Payment Gl Distribution - Start
                        If Not CashBookGlDist(TransactionId, LineNo, CashBookNo,
                            CompanyCode, TransCode, TransAmount, BankCode, MatricNo) Then
                            Return False
                        End If
                        'Cash Book Payment Gl Distribution - Stop

                    Else
                        Return False
                    End If
                    'if record successfully posted - Stop

                End If
                'Build Sql Statement - Stop

            End While
            'if batch details available - Stop

            Return True

        Catch ex As Exception

            'log error
            Call Helper.LogError(ex.Message)

            Return False

        End Try

    End Function

#End Region

#Region "Roll Back Cash Book Receipt "

    'Purpose			: To Roll Back Cash Book Receipt 
    'Author			    : Sujith Sharatchandran - T-Melmax Sdn Bhd
    'Created Date		: 01/06/2015
    Private Sub RollBackCashBookReceipt(ByVal BatchCode As String, ByVal CashBookNo As String)

        'Variable Declarations
        Dim SqlStatement As String = Nothing

        Try

            'Build Delete Sql Statement for Cash Book Payment - Start
            SqlStatement = "DELETE FROM " & CfGeneric.CfCbGlDistTbl & " WHERE "
            SqlStatement &= "gldi_batchid = " & clsGeneric.AddQuotes(CashBookNo) & ";"
            SqlStatement &= " DELETE FROM " & CfGeneric.CfCbRecHeaderTbl & " WHERE "
            SqlStatement &= " cbrh_batchid = " & clsGeneric.AddQuotes(CashBookNo) & ";"
            SqlStatement &= " DELETE FROM " & CfGeneric.CfCbRecBatchTbl & " WHERE "
            SqlStatement &= " cbrb_batchid = " & clsGeneric.AddQuotes(CashBookNo) & ";"
            'Build Delete Sql Statement for Cash Book Payment - Stop

            'Execute Sql Statement
            Call ExecuteSqlStatement(SqlStatement)

        Catch ex As Exception

            'log error
            Call Helper.LogError(ex.Message)

        End Try

    End Sub

#End Region

#Region "Cash Book Gl Disturbtion "

    'Purpose			: To Post Cash Book Payment GL Distribution
    'Author			    : Sujith Sharatchandran - T-Melmax Sdn Bhd
    'Created Date		: 31/05/2015
    Private Function CashBookGlDist(ByVal TransactionId As Integer,
        ByVal LineNo As Integer, ByVal CashBookNo As String,
        ByVal CompanyCode As String, ByVal TransCode As String,
        ByVal TransAmount As Decimal, ByVal BankCode As String,
        ByVal MatricNo As String) As Boolean

        'Create Instances
        Dim _CfCbGlDistEn As CfCbGlDistEn

        'Variable Declarations - Start
        Dim SqlStatement As String = Nothing, Glcode As String = Nothing
        Dim CfCbGlDistSql As String = Nothing, LedgerType As String = Nothing
        'Variable Declarations - Stop

        Try

            'Get Gl Code - Start
            Glcode = _CfCommon.GetGlCode(BankCode,
                CfGeneric.GlType.BankCode)
            'Get Gl Code - Stop

            'intialize
            _CfCbGlDistEn = New CfCbGlDistEn

            'Set Values - Start
            _CfCbGlDistEn.gldi_glac = Glcode
            _CfCbGlDistEn.gldi_seqno = LineNo
            _CfCbGlDistEn.gldi_itemref = TransCode
            _CfCbGlDistEn.gldi_itemrefline = LineNo
            _CfCbGlDistEn.gldi_batchid = CashBookNo
            _CfCbGlDistEn.gldi_amount = TransAmount
            _CfCbGlDistEn.gldi_company = CompanyCode
            _CfCbGlDistEn.gldi_lclamount = TransAmount
            LedgerType = _CfCommon.GetLedgerType(Glcode)
            _CfCbGlDistEn.gldi_serial = _CfCommon.GetGlDistNextSerialNo()
            _CfCbGlDistEn.gldi_desc = _CfCommon.GetGLDescription(Glcode, LedgerType)
            'Set Values - Stop

            'Build Sql Statement - Start
            If _CfGeneric.BuildSqlStatement(_CfCbGlDistEn,
                CfCbGlDistSql, CfGeneric.CfCbGlDistTbl) Then

                'if Successful - Start
                If ExecuteSqlStatement(CfCbGlDistSql) Then

                    'Clear Sql Statement
                    CfCbGlDistSql = String.Empty

                    'intialize
                    _CfCbGlDistEn = New CfCbGlDistEn

                    'Get Gl Code - Start
                    Glcode = _CfCommon.GetGlCode(MatricNo,
                        CfGeneric.GlType.StudentProgram)
                    'Get Gl Code - Stop

                    'Set Values - Start
                    _CfCbGlDistEn.gldi_glac = Glcode
                    _CfCbGlDistEn.gldi_seqno = LineNo + 1
                    _CfCbGlDistEn.gldi_itemref = TransCode
                    _CfCbGlDistEn.gldi_itemrefline = LineNo + 1
                    _CfCbGlDistEn.gldi_batchid = CashBookNo
                    _CfCbGlDistEn.gldi_company = CompanyCode
                    LedgerType = _CfCommon.GetLedgerType(Glcode)
                    _CfCbGlDistEn.gldi_amount = TransAmount * -1
                    _CfCbGlDistEn.gldi_lclamount = TransAmount * -1
                    _CfCbGlDistEn.gldi_serial = _CfCommon.GetGlDistNextSerialNo()
                    _CfCbGlDistEn.gldi_desc = _CfCommon.GetGLDescription(Glcode, LedgerType)
                    'Set Values - Stop

                    'Build Sql Statement - Start
                    If _CfGeneric.BuildSqlStatement(_CfCbGlDistEn,
                        CfCbGlDistSql, CfGeneric.CfCbGlDistTbl) Then

                        Return ExecuteSqlStatement(CfCbGlDistSql)

                    End If
                    'Build Sql Statement - Stop

                End If
                'if Successful - Stop

            End If
            'Build Sql Statement - Stop

        Catch ex As Exception

            'log error
            Call Helper.LogError(ex.Message)

            Return False

        End Try

    End Function

#End Region

#Region "Multiple Journel "

    'Purpose			: To Post Mulitple Journel
    'Author			    : Sujith Sharatchandran - T-Melmax Sdn Bhd
    'Created Date		: 01/06/2015
    Public Function MultipleJournel(ByVal CategoryType As String,
        ByVal BatchCode As String, ByVal TransactionCode As String,
        ByVal TransactionType As String, ByVal PostedBy As String,
        ByRef JournelNo As String) As Boolean

        'Variable Declarations - Start
        Dim CfMjHeaderSql As String = Nothing, CompanyCode As String = Nothing
        'Variable Declarations - Stop

        Try

            'Get Company Code
            CompanyCode = _CfCommon.GetCompanyCode()

            'Get Auto Number
            JournelNo = _CfCommon.GetAutoNumber(CfGeneric.PostingType.MultipleJournel)

            'Post to Multiple Journel Header - Start
            If MultipleJournelHeader(BatchCode, CompanyCode,
                JournelNo, CategoryType, PostedBy, TransactionType) Then

                'Increment Auto No
                Call _CfCommon.UpdateAutoNumber(CfGeneric.PostingType.MultipleJournel)

                Return True

            Else

                'Roll Back Transactions
                Call RollBackMultipleJournal(BatchCode, JournelNo)

                Return False

            End If
            'Post to Multiple Journel Header - Stop

            Return False

        Catch ex As Exception

            'log error
            Call Helper.LogError(ex.Message)

            Return False

        End Try

    End Function

#End Region

#Region "Multiple Journel Header "

    'Purpose			: To Post Mulitple Journel Header
    'Author			    : Sujith Sharatchandran - T-Melmax Sdn Bhd
    'Created Date		: 01/06/2015
    Private Function MultipleJournelHeader(ByVal BatchCode As String,
        ByVal CompanyCode As String, ByVal JournalNo As String,
        ByVal CategoryType As String, ByVal PostedBy As String,
        ByVal TransactionType As String) As Boolean

        'Create Instances - Start
        Dim _CfMjHeaderEn As New CfMjHeaderEn()
        'Create Instances - Stop

        'Variable Declarations - Start
        Dim TotalRecords As Integer = 0
        Dim CfMjHeaderSql As String = Nothing, HeaderDescription As String = Nothing
        'Variable Declarations - Stop

        Try

            'Set Header Description
            HeaderDescription = CategoryType & " Batch " & BatchCode & " From SAS"

            'Set Values - Start
            _CfMjHeaderEn.mjjh_who = PostedBy
            _CfMjHeaderEn.mjjh_jnl = JournalNo
            _CfMjHeaderEn.mjjh_company = CompanyCode
            _CfMjHeaderEn.mjjh_desc = HeaderDescription
            _CfMjHeaderEn.mjjh_entdate = CfGeneric.DateConversion(Now)
            _CfMjHeaderEn.mjjh_jnldate = CfGeneric.DateConversion(Now)
            'Set Values - Stop

            'Build Sql Statement - Start
            If _CfGeneric.BuildSqlStatement(_CfMjHeaderEn,
                CfMjHeaderSql, CfGeneric.CfMjHeaderTbl) Then

                'if header successfully posted - Start
                If ExecuteSqlStatement(CfMjHeaderSql) Then

                    'If Line Debit Details Successful - Start
                    If MultipleJournelLineDebit(BatchCode, CompanyCode,
                        JournalNo, TotalRecords, TransactionType) Then

                        'If Line Credit Details Successful - Start
                        If Not MultipleJournelLineCredit(BatchCode, CompanyCode,
                            JournalNo, TotalRecords, TransactionType) Then
                            Return False
                        End If
                        'If Line Credit Details Successful - Stop

                    End If
                    'If Line Debit Details Successful - Stop

                End If
                'if header successfully posted - Stop

            End If
            'Build Sql Statement - Stop

            Return True

        Catch ex As Exception

            'log error
            Call Helper.LogError(ex.Message)

            Return False

        End Try

    End Function

#End Region

#Region "Multiple Journel Line - Debit "

    'Purpose			: To Post Mulitple Journel Line - Debit
    'Author			    : Sujith Sharatchandran - T-Melmax Sdn Bhd
    'Created Date		: 01/06/2015
    Private Function MultipleJournelLineDebit(ByVal BatchCode As String,
        ByVal CompanyCode As String, ByVal JournalNo As String,
        ByRef TotalRecords As Integer, ByVal TransactionType As String) As Boolean

        'Create Instances - Start
        Dim _CfMJLineEn As New CfMjLineEn()
        Dim LineDetails As IDataReader = Nothing
        'Create Instances - Stop

        'Variable Declarations - Start
        Dim LineNo As Integer = 0
        Dim SqlStatement As String = Nothing, GlCode As String = Nothing, GlDescription As String = Nothing
        Dim TransactionAmount As Decimal = 0, LedgerType As String = Nothing, CfMjLineSql As String = Nothing
        'Variable Declarations - Stop

        Try

            If TransactionType = CfGeneric.TransactionTypeDebit Then

                'Build Sql Statement - Start
                SqlStatement = "Select ROW_NUMBER() OVER (ORDER BY SAS_Program.SAPG_TI) AS Row_No,"
                SqlStatement &= "SAS_Program.SAPG_TI AS GL_Code,SUM(SAS_AccountsDetails.TransAmount) AS Trans_Amount"
                SqlStatement &= " FROM SAS_Accounts INNER JOIN SAS_AccountsDetails ON"
                SqlStatement &= " SAS_Accounts.TransCode = SAS_AccountsDetails.TransCode INNER JOIN"
                SqlStatement &= " SAS_Student ON SAS_Accounts.CreditRef = SAS_Student.SASI_MatricNo"
                SqlStatement &= " INNER JOIN SAS_Program ON SAS_Student.SASI_PgId = SAS_Program.SAPG_Code"
                SqlStatement &= " WHERE SAS_Accounts.batchcode = " & clsGeneric.AddQuotes(BatchCode)
                SqlStatement &= " GROUP BY SAS_Program.SAPG_TI"
                'Build Sql Statement - Stop

            ElseIf TransactionType = CfGeneric.TransactionTypeCredit Then

                'Build Sql Statement - Start
                SqlStatement = "Select ROW_NUMBER() OVER (ORDER BY SAS_FeeTypes.SAFT_GLCode) AS Row_No,"
                SqlStatement &= "SAS_FeeTypes.SAFT_GLCode AS GL_Code,SUM(SAS_AccountsDetails.TransAmount) AS Trans_Amount"
                SqlStatement &= " FROM SAS_Accounts INNER JOIN SAS_AccountsDetails ON"
                SqlStatement &= " SAS_Accounts.TransCode = SAS_AccountsDetails.TransCode INNER JOIN"
                SqlStatement &= " SAS_FeeTypes ON SAS_AccountsDetails.RefCode = SAS_FeeTypes.SAFT_Code"
                SqlStatement &= " WHERE SAS_Accounts.batchcode = " & clsGeneric.AddQuotes(BatchCode)
                SqlStatement &= " GROUP BY SAS_FeeTypes.SAFT_GLCode"
                'Build Sql Statement - Stop

            End If

            'Get Line Details - Start
            LineDetails = _DataBaseProvider.ExecuteReader(Helper.GetDataBaseType,
                Helper.GetConnectionString, SqlStatement).CreateDataReader()
            'Get Line Details - Stop

            'Loop thro the Data Reader - Start
            While LineDetails.Read

                'Get Values - Start
                GlCode = LineDetails(Helper.GlCodeCol)
                LedgerType = _CfCommon.GetLedgerType(GlCode)
                LineNo = LineDetails(Helper.LineNoCol)
                TransactionAmount = LineDetails(Helper.TransAmountCol)
                GlDescription = _CfCommon.GetGLDescription(GlCode, LedgerType)
                'Get Values - Stop

                'Set Values - Start
                _CfMJLineEn.mjjl_lineno = LineNo
                _CfMJLineEn.mjjl_jnl = JournalNo
                _CfMJLineEn.mjjl_account = GlCode
                _CfMJLineEn.mjjl_ledger = LedgerType
                _CfMJLineEn.mjjl_desc = GlDescription
                _CfMJLineEn.mjjl_company = CompanyCode
                _CfMJLineEn.mjjl_amount = TransactionAmount
                _CfMJLineEn.mjjl_lclamount = TransactionAmount
                _CfMJLineEn.mjjl_reference = BatchCode.Substring(4, 12)
                'Set Values - Stop

                'Build Sql Statement - Start
                If _CfGeneric.BuildSqlStatement(_CfMJLineEn,
                    CfMjLineSql, CfGeneric.CfMjLineTbl) Then

                    'if posting not successful - Start
                    If Not ExecuteSqlStatement(CfMjLineSql) Then
                        Return False
                    End If
                    'if posting not successful - Stop

                End If
                'Build Sql Statement - Stop

                'Increment Row Count
                TotalRecords = TotalRecords + 1

            End While
            'Loop thro the Data Reader - Stop

            Return True

        Catch ex As Exception

            'log error
            Call Helper.LogError(ex.Message)

            Return False

        End Try

    End Function

#End Region

#Region "Multiple Journel Line - Credit "

    'Purpose			: To Post Mulitple Journel Line - Debit
    'Author			    : Sujith Sharatchandran - T-Melmax Sdn Bhd
    'Created Date		: 01/06/2015
    Private Function MultipleJournelLineCredit(ByVal BatchCode As String,
        ByVal CompanyCode As String, ByVal JournalNo As String,
        ByVal TotalRecords As Integer, ByVal TransactionType As String) As Boolean

        'Create Instances - Start
        Dim _CfMJLineEn As New CfMjLineEn()
        Dim LineDetails As IDataReader = Nothing
        'Create Instances - Stop

        'Variable Declarations - Start
        Dim LineNo As Integer = 0
        Dim SqlStatement As String = Nothing, GlCode As String = Nothing, GlDescription As String = Nothing
        Dim TransactionAmount As Decimal = 0, LedgerType As String = Nothing, CfMjLineSql As String = Nothing
        'Variable Declarations - Stop

        Try

            If TransactionType = CfGeneric.TransactionTypeDebit Then

                'Build Sql Statement - Start
                SqlStatement = "Select ROW_NUMBER() OVER (ORDER BY SAS_FeeTypes.SAFT_GLCode) AS Row_No,"
                SqlStatement &= "SAS_FeeTypes.SAFT_GLCode AS GL_Code,SUM(SAS_AccountsDetails.TransAmount) AS Trans_Amount"
                SqlStatement &= " FROM SAS_Accounts INNER JOIN SAS_AccountsDetails ON"
                SqlStatement &= " SAS_Accounts.TransCode = SAS_AccountsDetails.TransCode INNER JOIN"
                SqlStatement &= " SAS_FeeTypes ON SAS_AccountsDetails.RefCode = SAS_FeeTypes.SAFT_Code"
                SqlStatement &= " WHERE SAS_Accounts.batchcode = " & clsGeneric.AddQuotes(BatchCode)
                SqlStatement &= " GROUP BY SAS_FeeTypes.SAFT_GLCode"
                'Build Sql Statement - Stop

            ElseIf TransactionType = CfGeneric.TransactionTypeCredit Then

                'Build Sql Statement - Start
                SqlStatement = "Select ROW_NUMBER() OVER (ORDER BY SAS_Program.SAPG_TI) AS Row_No,"
                SqlStatement &= "SAS_Program.SAPG_TI AS GL_Code,SUM(SAS_AccountsDetails.TransAmount) AS Trans_Amount"
                SqlStatement &= " FROM SAS_Accounts INNER JOIN SAS_AccountsDetails ON"
                SqlStatement &= " SAS_Accounts.TransCode = SAS_AccountsDetails.TransCode INNER JOIN"
                SqlStatement &= " SAS_Student ON SAS_Accounts.CreditRef = SAS_Student.SASI_MatricNo"
                SqlStatement &= " INNER JOIN SAS_Program ON SAS_Student.SASI_PgId = SAS_Program.SAPG_Code"
                SqlStatement &= " WHERE SAS_Accounts.batchcode = " & clsGeneric.AddQuotes(BatchCode)
                SqlStatement &= " GROUP BY SAS_Program.SAPG_TI"
                'Build Sql Statement - Stop

            End If

            'Get Line Details - Start
            LineDetails = _DataBaseProvider.ExecuteReader(Helper.GetDataBaseType,
                Helper.GetConnectionString, SqlStatement).CreateDataReader()
            'Get Line Details - Stop

            'Loop thro the Data Reader - Start
            While LineDetails.Read

                'Get Values - Start
                LineNo = LineDetails(Helper.LineNoCol)
                GlCode = LineDetails(Helper.GlCodeCol)
                LedgerType = _CfCommon.GetLedgerType(GlCode)
                TransactionAmount = LineDetails(Helper.TransAmountCol) * -1
                GlDescription = _CfCommon.GetGLDescription(GlCode, LedgerType)
                'Get Values - Stop

                'Set Values - Start
                _CfMJLineEn.mjjl_jnl = JournalNo
                _CfMJLineEn.mjjl_account = GlCode
                _CfMJLineEn.mjjl_ledger = LedgerType
                _CfMJLineEn.mjjl_desc = GlDescription
                _CfMJLineEn.mjjl_company = CompanyCode
                _CfMJLineEn.mjjl_amount = TransactionAmount
                _CfMJLineEn.mjjl_lclamount = TransactionAmount
                _CfMJLineEn.mjjl_lineno = LineNo + TotalRecords
                _CfMJLineEn.mjjl_reference = BatchCode.Substring(4, 12)
                'Set Values - Stop

                'Build Sql Statement - Start
                If _CfGeneric.BuildSqlStatement(_CfMJLineEn,
                    CfMjLineSql, CfGeneric.CfMjLineTbl) Then

                    'if posting not successful - Start
                    If Not ExecuteSqlStatement(CfMjLineSql) Then
                        Return False
                    End If
                    'if posting not successful - Stop

                End If
                'Build Sql Statement - Stop

            End While
            'Loop thro the Data Reader - Stop

            Return True

        Catch ex As Exception

            'log error
            Call Helper.LogError(ex.Message)

            Return False

        End Try

    End Function

#End Region

#Region "Roll Back Multiple Journel "

    'Purpose			: To Roll Back Multiple Journal 
    'Author			    : Sujith Sharatchandran - T-Melmax Sdn Bhd
    'Created Date		: 01/06/2015
    Private Sub RollBackMultipleJournal(ByVal BatchCode As String, ByVal JournalNo As String)

        'Variable Declarations
        Dim SqlStatement As String = Nothing

        Try

            'Build Delete Sql Statement for Multiple Journal - Start
            SqlStatement = "DELETE FROM " & CfGeneric.CfMjLineTbl & " WHERE "
            SqlStatement &= "mjjl_jnl = " & clsGeneric.AddQuotes(JournalNo) & ";"
            SqlStatement &= " DELETE FROM " & CfGeneric.CfMjHeaderTbl & " WHERE "
            SqlStatement &= " mjjh_jnl = " & clsGeneric.AddQuotes(JournalNo) & ";"
            'Build Delete Sql Statement for Multiple Journal - Stop

            'Execute Sql Statement
            Call ExecuteSqlStatement(SqlStatement)

        Catch ex As Exception

            'log error
            Call Helper.LogError(ex.Message)

        End Try

    End Sub

#End Region

End Class
