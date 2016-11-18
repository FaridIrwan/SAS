Option Strict On
Option Explicit On

Public Class CfMjLineEn

    Private _mjjl_company As String
    Public Property mjjl_company() As String
        Get
            Return _mjjl_company
        End Get
        Set(ByVal value As String)
            _mjjl_company = value
        End Set
    End Property

    Private _mjjl_jnl As String
    Public Property mjjl_jnl() As String
        Get
            Return _mjjl_jnl
        End Get
        Set(ByVal value As String)
            _mjjl_jnl = value
        End Set
    End Property

    Private _mjjl_lineno As String
    Public Property mjjl_lineno() As String
        Get
            Return _mjjl_lineno
        End Get
        Set(ByVal value As String)
            _mjjl_lineno = value
        End Set
    End Property

    Private _mjjl_ledger As String
    Public Property mjjl_ledger() As String
        Get
            Return _mjjl_ledger
        End Get
        Set(ByVal value As String)
            _mjjl_ledger = value
        End Set
    End Property

    Private _mjjl_account As String
    Public Property mjjl_account() As String
        Get
            Return _mjjl_account
        End Get
        Set(ByVal value As String)
            _mjjl_account = value
        End Set
    End Property

    Private _mjjl_units As String = Helper.NullValue
    Public ReadOnly Property mjjl_units() As String
        Get
            Return _mjjl_units
        End Get
    End Property

    Private _mjjl_desc As String
    Public Property mjjl_desc() As String
        Get
            Return _mjjl_desc
        End Get
        Set(ByVal value As String)
            _mjjl_desc = value
        End Set
    End Property

    Private _mjjl_amount As Decimal
    Public Property mjjl_amount() As Decimal
        Get
            Return _mjjl_amount
        End Get
        Set(ByVal value As Decimal)
            _mjjl_amount = value
        End Set
    End Property

    Private _mjjl_reference As String
    Public Property mjjl_reference() As String
        Get
            Return _mjjl_reference
        End Get
        Set(ByVal value As String)
            _mjjl_reference = value
        End Set
    End Property

    Private _mjjl_lclamount As Decimal
    Public Property mjjl_lclamount() As Decimal
        Get
            Return _mjjl_lclamount
        End Get
        Set(ByVal value As Decimal)
            _mjjl_lclamount = value
        End Set
    End Property

End Class
