#Region "NameSpaces "

Imports System
Imports MaxGeneric
Imports System.Data
Imports System.Data.Common
Imports System.Configuration
Imports System.Collections.Generic
Imports EntLibContrib.Data.PostgreSql
Imports Microsoft.Practices.EnterpriseLibrary.Data
Imports Microsoft.Practices.EnterpriseLibrary.Data.Sql

#End Region

Public Class CfCommon

#Region "Constants "


    Public Const GlSagaAccount As String = "G"
    Public Const GlPcAccount As String = "P"

    'Table\Columns - Start
    Public Const ColJournelNo As String = "mjjh_jnl"
    Public Const ColJournelLine As String = "mjjl_jnl"
    Public Const ColJournelLineNo As String = "mjjl_lineno"
    Public Const TblJournelHeader As String = "mj_jnlhdr"
    Public Const TblJournelLine As String = "mj_jnlline"
    Public Const ColJournelCompany As String = "mjjh_company"
    Public Const ColJournelLineCompany As String = "mjjl_company"
    Public Const ColJournelLedger As String = "mjjl_ledger"
    Public Const ColJournelAccount As String = "mjjl_account"
    Public Const ColJournelAmount As String = "mjjl_amount"
    Public Const ColJournelLcAmount As String = "mjjl_lclamount"
    Public Const ColJournelReference As String = "mjjl_reference"
    Public Const ColJournelUnits As String = "mjjl_units"
    Public Const ColJournelEntryDate As String = "mjjh_entdate"
    Public Const ColJournelDesc As String = "mjjh_desc"
    Public Const ColJournelLineDesc As String = "mjjl_desc"
    Public Const ColJournelDate As String = "mjjh_jnldate"
    Public Const ColJournelStatus As String = "mjjh_status"
    Public Const ColJournelPostDate As String = "mjjh_postdate"
    Public Const ColJournelPrefixId As String = "mjjh_nprefid"
    Public Const ColJournelCurrencyCode As String = "mjjh_curcode"
    Public Const ColJournelCurrencyRate As String = "mjjh_currate"
    Public Const ColJournelBaseUnit As String = "mjjh_baseunit"
    Public Const ColJournelOperator As String = "mjjh_operator"
    Public Const ColJournelDoneBy As String = "mjjh_who"
    Public Const ColJournelDateTime As String = "mjjh_dtime"
    Public Const ColJournelLedgerDateTime As String = "mjjh_ldtime"
    'Table\Columns - Stop


#End Region

#Region "Global Declarations "

    Private _DataBaseFactory As New DatabaseProvider

#End Region

#Region "Check GL Code "

    'Purpose			: To Check GL Code
    'Author			    : Sujith Sharatchandran - T-Melmax Sdn Bhd
    'Created Date		: 06/05/2015
    Public Function GLCodeValid(ByVal GLCodeToCheck As String) As Boolean

        'Create Instances - Start
        Dim _DbCommand As DbCommand = Nothing
        Dim _DbDataReader As DbDataReader = Nothing
        'Create Instances - Stop

        'Variable Declarations
        Dim SqlStatement As String = Nothing

        Try

            'Build Sql Statement - Start
            SqlStatement = "SELECT glac_desc AS GL_Desc FROM gl_account WHERE "
            SqlStatement &= " glac_account = " & clsGeneric.AddQuotes(GLCodeToCheck)
            SqlStatement &= " UNION "
            SqlStatement &= "SELECT pcac_desc AS GL_Desc from pc_account WHERE "
            SqlStatement &= " pcac_account = " & clsGeneric.AddQuotes(GLCodeToCheck)
            'Build Sql Statement - Stop

            Select Case Helper.GetFinancialsDataBaseType

                Case Helper.DataBaseType.PostGres
                    Dim _NpgsqlDatabase As NpgsqlDatabase = New NpgsqlDatabase(Helper.FinancialsConnectionString)
                    _DbCommand = _NpgsqlDatabase.GetSqlStringCommand(SqlStatement)
                    _DbDataReader = _NpgsqlDatabase.ExecuteReader(_DbCommand)
                    If _DbDataReader.HasRows Then
                        Return True
                    End If

                Case Helper.DataBaseType.SqlServer
                    Dim _SqlDataBase As SqlDatabase = New SqlDatabase(Helper.FinancialsConnectionString)
                    _DbCommand = _SqlDataBase.GetSqlStringCommand(SqlStatement)
                    _DbDataReader = _SqlDataBase.ExecuteReader(_DbCommand)
                    If _DbDataReader.HasRows Then
                        Return True
                    End If

                Case Else
                    Return False

            End Select

        Catch ex As Exception

            'log error
            Call Helper.LogError(ex.Message)

        End Try

    End Function

#End Region

#Region "Get Batch Details "

    'Purpose			: To Get Batch Details
    'Author			    : Sujith Sharatchandran - T-Melmax Sdn Bhd
    'Created Date		: 19/05/2015
    Private Function GetBatchDetails(ByVal BatchCode As String) As DataTable

        'Create Instances
        Dim _DataSet As DataSet = Nothing

        'Variable Declarations
        Dim SqlStatement As String = Nothing

        Try

            'Build Sql Statement - Start
            SqlStatement = "SELECT distinct a.BatchCode batch_code, a.PostedTimeStamp posted_date, "
            SqlStatement &= " a.UpdatedBy userid FROM SAS_Accounts a, SAS_AccountsDetails b"
            SqlStatement &= " WHERE a.TransID = b.TransID and a.BatchCode in (" + clsGeneric.AddQuotes(BatchCode) + ")"
            'Build Sql Statement - Stop

            'Get Details - Start
            _DataSet = _DataBaseFactory.ExecuteDataSet(Helper.GetDataBaseType,
                Helper.GetConnectionString, SqlStatement)
            'Get Details - Stop

            Return _DataSet.Tables(0)

        Catch ex As Exception

            'log error
            Call Helper.LogError(ex.Message)

            Return Nothing

        End Try

    End Function


#End Region

#Region "Get Company Code "

    'Purpose			: To Get Company Code
    'Author			    : Sujith Sharatchandran - T-Melmax Sdn Bhd
    'Created Date		: 19/05/2015
    Public Function GetCompanyCode() As String

        'Variable Declarations
        Dim SqlStatement As String = Nothing, CompanyCode As String = Nothing

        Try

            'Build Sql Statement
            SqlStatement = "SELECT SAUP_Code As SAUP_Code FROM SAS_UniversityProfile"

            'Execute Data Reader - Start
            Dim _IDataReader As IDataReader = _DataBaseFactory.ExecuteReader(Helper.GetDataBaseType,
                 Helper.GetConnectionString, SqlStatement).CreateDataReader()
            'Execute Data Reader - Stop

            If _IDataReader.Read() Then
                CompanyCode = clsGeneric.NullToString(_IDataReader(0))
            End If

            Return CompanyCode

        Catch ex As Exception

            'log error
            Call clsGeneric.prcLogError(ex.Message, "MaxModule.Financials - GetBatchDetails")

            Return String.Empty

        End Try

    End Function


#End Region

#Region "Get Auto Number "

    'Purpose			: To Generate Running Number for Posting to CF
    'Author			    : Sujith Sharatchandran - T-Melmax Sdn Bhd
    'Created Date		: 19/05/2015
    Public Function GetAutoNumber(ByVal _PostingType As CfGeneric.PostingType) As String

        'Create Instances
        Dim _DataSet As DataSet = Nothing

        'Variable Declarations - Start
        Dim AutoNoLen As Short = 0, NextAutoNumber As String = Nothing
        Dim SqlStatement As String = Nothing, AutoNumber As Integer = 0, AutoNoPrefix As String = Nothing
        'Variable Declarations - Stop

        Try

            'Build Sql Statement
            SqlStatement = "SELECT * FROM sas_saga WHERE Posting_Type = " & _PostingType

            'Get Details - Start
            _DataSet = _DataBaseFactory.ExecuteDataSet(Helper.GetDataBaseType,
                Helper.GetConnectionString, SqlStatement)
            'Get Details - Stop

            'If Records Available - Start
            If _DataSet.Tables(0).Rows.Count > 0 Then

                'Get Values - Start
                With _DataSet.Tables(0)
                    AutoNumber = .Rows(0)("Auto_No")
                    AutoNoLen = .Rows(0)("Auto_Length")
                    AutoNoPrefix = .Rows(0)("Auto_Prefix")
                End With
                'Get Values - Stop

                'Get Next Auto Number - Start
                NextAutoNumber = clsGeneric.StringFiller(
                    AutoNumber, AutoNoLen, EnumHelp.FillerType.Prefix, "0")
                NextAutoNumber = AutoNoPrefix & NextAutoNumber
                'Get Next Auto Number - Stop

            End If
            'If Records Available - Stop

            Return NextAutoNumber

        Catch ex As Exception

            'log error
            Call Helper.LogError(ex.Message)

            Return String.Empty

        End Try

    End Function


#End Region

#Region "Track Posting Details "

    'Purpose			: To Track Posting Details
    'Author			    : Sujith Sharatchandran - T-Melmax Sdn Bhd
    'Created Date		: 20/05/2015
    Private Sub TrackPostingDetails(ByVal BatchCode As String,
        ByVal PostingType As CfGeneric.PostingType, ByVal ReferenceNo As String)

        'Variable Declarations
        Dim SqlStatement As String = Nothing

        Try

            'Build Sql Statement - Start
            SqlStatement = "INSERT INTO sas_saga_posting (posting_type,batch_code,reference_no,posting_date)"
            SqlStatement &= " VALUES (" & PostingType
            SqlStatement &= clsGeneric.AddComma() & clsGeneric.AddQuotes(BatchCode)
            SqlStatement &= clsGeneric.AddComma() & clsGeneric.AddQuotes(ReferenceNo)
            SqlStatement &= clsGeneric.AddComma() & clsGeneric.AddQuotes(Format(Now, "yyyy-MM-dd hh:mm:ss")) & ")"
            'Build Sql Statement - Stop

            'Execute Sql Statement - Start
            Call _DataBaseFactory.ExecuteSqlStatement(
                Helper.GetDataBaseType, Helper.GetConnectionString, SqlStatement)
            'Execute Sql Statement - Stop

        Catch ex As Exception

            'log error
            Call Helper.LogError(ex.Message)

        End Try

    End Sub

#End Region

#Region "Update Auto Number "

    'Purpose			: To Track Posting Details
    'Author			    : Sujith Sharatchandran - T-Melmax Sdn Bhd
    'Created Date		: 20/05/2015
    Public Sub UpdateAutoNumber(ByVal PostingType As Short)

        'Variable Declarations
        Dim SqlStatement As String = Nothing

        Try

            'Build Sql Statement - Start
            SqlStatement = "UPDATE sas_saga set auto_no = auto_no + 1"
            SqlStatement &= " WHERE posting_type = " & PostingType
            'Build Sql Statement - Stop

            'Execute Sql Statement - Start
            Call _DataBaseFactory.ExecuteSqlStatement(
                Helper.GetDataBaseType, Helper.GetConnectionString, SqlStatement)
            'Execute Sql Statement - Stop

        Catch ex As Exception

            'log error
            Call Helper.LogError(ex.Message)

        End Try

    End Sub

#End Region

#Region "Get Ledger Type "

    'Purpose			: To Get the ledger type
    'Author			    : Sujith Sharatchandran - T-Melmax Sdn Bhd
    'Created Date		: 19/05/2015
    Public Function GetLedgerType(ByVal GlAccount As String) As String

        'Create Instances
        Dim _DataSet As DataSet = Nothing

        'variable declarations
        Dim LedgerType As String = Nothing, SqlStatement As String = Nothing

        Try

            'Build SqlStatement - Start
            SqlStatement = "select glac_account, glac_desc from gl_account "
            SqlStatement &= "where glac_account = " & clsGeneric.AddQuotes(GlAccount)
            'Build SqlStatement - Stop

            'get Data - Start
            _DataSet = _DataBaseFactory.ExecuteDataSet(Helper.GetFinancialsDataBaseType,
                Helper.FinancialsConnectionString, SqlStatement)
            'get Data - Stoo

            If _DataSet.Tables(0).Rows.Count > 0 Then
                LedgerType = "G" ' exist in SAGA GL Account
            Else
                LedgerType = "P" ' pc account
            End If

            Return LedgerType

        Catch ex As Exception

            'log error
            Call Helper.LogError(ex.Message)

            Return String.Empty

        End Try

    End Function

#End Region

#Region "Get GL Desc "

    'Purpose			: To Get GL Description
    'Author			    : Sujith Sharatchandran - T-Melmax Sdn Bhd
    'Created Date		: 20/05/2015
    Public Function GetGLDescription(ByVal GlAccount As String, ByVal LedgerType As String) As String

        'Variable Declarations
        Dim SqlStatement As String = Nothing, GlDescription As String = Nothing

        Try

            Select Case LedgerType

                Case GlSagaAccount
                    SqlStatement = "SELECT glac_desc FROM gl_account WHERE glac_account = " & clsGeneric.AddQuotes(GlAccount)

                Case GlPcAccount
                    SqlStatement = "SELECT pcac_desc FROM pc_account WHERE pcac_account = " & clsGeneric.AddQuotes(GlAccount)

            End Select

            Dim _IDataReader As IDataReader = _DataBaseFactory.ExecuteReader(Helper.GetFinancialsDataBaseType,
                Helper.FinancialsConnectionString, SqlStatement).CreateDataReader()

            If _IDataReader.Read() Then
                GlDescription = clsGeneric.NullToString(_IDataReader(0))
            End If

            Return GlDescription

        Catch ex As Exception

            'log error
            Call Helper.LogError(ex.Message)

            Return String.Empty

        End Try

    End Function

#End Region

#Region "Get Magic No Receipt Header "

    'Purpose			: To Get the magic no from Receipt Header
    'Author			    : Sujith Sharatchandran - T-Melmax Sdn Bhd
    'Created Date		: 07/06/2015
    Public Function GetReceiptHeaderMagicNo() As Integer

        'Variable Declarations
        Dim SqlStatement As String = Nothing, MagicNo As Integer = 0

        Try

            'Build Sql Statement
            SqlStatement = "SELECT MAX(cbrh_magic) AS Magio_No FROM cb_rcpthdr"

            MagicNo = _DataBaseFactory.ExecuteScalar(Helper.GetFinancialsDataBaseType,
                Helper.FinancialsConnectionString, SqlStatement)

            Return MagicNo + 1

        Catch ex As Exception

            'log error
            Call MaxModule.Helper.LogError(ex.Message)

            Return 0

        End Try

    End Function


#End Region

#Region "Get the Gl Code "

    'Purpose			: To Get the Gl Code
    'Author			    : Sujith Sharatchandran - T-Melmax Sdn Bhd
    'Created Date		: 07/06/2015
    Public Function GetGlCode(ByVal MatchingField As String,
        ByVal _GlCodeType As CfGeneric.GlType) As String

        'Variable Declarations
        Dim SqlStatement As String = Nothing, GlCode As String = Nothing

        Try

            Select Case _GlCodeType

                Case CfGeneric.GlType.BankCode

                    'Build Sql Statement - Start
                    SqlStatement = "SELECT  SAS_BankDetails.SABD_GLCode AS Gl_Code FROM SAS_Accounts "
                    SqlStatement &= "INNER JOIN SAS_BankDetails ON SAS_Accounts.BankCode = SAS_BankDetails.SABD_Code "
                    SqlStatement &= "WHERE SAS_Accounts.BankCode = " & clsGeneric.AddQuotes(MatchingField)
                    'Build Sql Statement - Stop

                Case CfGeneric.GlType.StudentProgram

                    SqlStatement = "SELECT DISTINCT SAS_Program.SAPG_TI AS Gl_Code FROM SAS_Program "
                    SqlStatement &= "INNER JOIN SAS_Student ON SAS_Program.SAPG_Code = SAS_Student.SASI_PgId "
                    SqlStatement &= "INNER JOIN SAS_Accounts ON SAS_Student.SASI_MatricNo = SAS_Accounts.CreditRef "
                    SqlStatement &= "WHERE SAS_Accounts.CreditRef = " & clsGeneric.AddQuotes(MatchingField)

            End Select

            Return clsGeneric.NullToString(_DataBaseFactory.ExecuteScalar(Helper.GetDataBaseType,
               Helper.GetConnectionString, SqlStatement))

        Catch ex As Exception

            'log error
            Call MaxModule.Helper.LogError(ex.Message)
            Return String.Empty

        End Try

    End Function


#End Region

#Region "Get Gl Distribution Serial No "

    'Purpose			: To Get Gl Distribution Serial No 
    'Author			    : Sujith Sharatchandran - T-Melmax Sdn Bhd
    'Created Date		: 07/06/2015
    Public Function GetGlDistNextSerialNo() As Integer

        'Variable Declarations
        Dim SqlStatement As String = Nothing, SerialNo As Integer = 0

        Try

            'Build Sql Statement
            SqlStatement = "SELECT MAX(gldi_serial) AS Serial_No FROM gl_dist"

            SerialNo = _DataBaseFactory.ExecuteScalar(Helper.GetFinancialsDataBaseType,
                Helper.FinancialsConnectionString, SqlStatement)

            Return SerialNo + 1

        Catch ex As Exception

            'log error
            Call MaxModule.Helper.LogError(ex.Message)

            Return 0

        End Try

    End Function


#End Region

#Region "Get Magic No Payment Header "

    'Purpose			: To Get the magic no from Receipt Header
    'Author			    : Sujith Sharatchandran - T-Melmax Sdn Bhd
    'Created Date		: 07/06/2015
    Public Function GetPaymentHeaderMagicNo() As Integer

        'Variable Declarations
        Dim SqlStatement As String = Nothing, MagicNo As Integer = 0

        Try

            'Build Sql Statement
            SqlStatement = "SELECT MAX(cbph_magic) AS Magic_No FROM cb_payhdr"

            MagicNo = _DataBaseFactory.ExecuteScalar(Helper.GetFinancialsDataBaseType,
                Helper.FinancialsConnectionString, SqlStatement)

            Return MagicNo + 1

        Catch ex As Exception

            'log error
            Call MaxModule.Helper.LogError(ex.Message)

            Return 0

        End Try

    End Function


#End Region

#Region "Check GL Code-R "

    'Purpose			: To Check GL Code
    'Author			    : Sujith Sharatchandran - T-Melmax Sdn Bhd
    'Created Date		: 06/05/2015
    Public Function GLCodeValidR(ByVal GLCodeToCheck As String) As Boolean

        'Create Instances - Start
        Dim _DbCommand As DbCommand = Nothing
        Dim _DbDataReader As DbDataReader = Nothing
        'Create Instances - Stop

        'Variable Declarations
        Dim SqlStatement As String = Nothing

        Try

            'Build Sql Statement - Start
            SqlStatement = "SELECT glac_desc AS GL_Desc FROM gl_account WHERE "
            SqlStatement &= " glac_account = " & clsGeneric.AddQuotes(GLCodeToCheck)
            SqlStatement &= " AND substring(glac_account from 15 for 1) = 'R' UNION "
            SqlStatement &= "SELECT pcac_desc AS GL_Desc from pc_account WHERE "
            SqlStatement &= " pcac_account = " & clsGeneric.AddQuotes(GLCodeToCheck)
            SqlStatement &= " AND substring(pcac_account from 15 for 1) = 'R' "
            'Build Sql Statement - Stop

            Select Case Helper.GetFinancialsDataBaseType

                Case Helper.DataBaseType.PostGres
                    Dim _NpgsqlDatabase As NpgsqlDatabase = New NpgsqlDatabase(Helper.FinancialsConnectionString)
                    _DbCommand = _NpgsqlDatabase.GetSqlStringCommand(SqlStatement)
                    _DbDataReader = _NpgsqlDatabase.ExecuteReader(_DbCommand)
                    If _DbDataReader.HasRows Then
                        Return True
                    End If

                Case Helper.DataBaseType.SqlServer
                    Dim _SqlDataBase As SqlDatabase = New SqlDatabase(Helper.FinancialsConnectionString)
                    _DbCommand = _SqlDataBase.GetSqlStringCommand(SqlStatement)
                    _DbDataReader = _SqlDataBase.ExecuteReader(_DbCommand)
                    If _DbDataReader.HasRows Then
                        Return True
                    End If

                Case Else
                    Return False

            End Select

        Catch ex As Exception

            'log error
            Call Helper.LogError(ex.Message)

        End Try

    End Function

#End Region

End Class
