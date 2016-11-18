Option Strict On
Option Explicit On

Public Class CfMjHeaderEn

    Private _mjjh_company As String
    Public Property mjjh_company() As String
        Get
            Return _mjjh_company
        End Get
        Set(ByVal value As String)
            _mjjh_company = value
        End Set
    End Property

    Private _mjjh_jnl As String
    Public Property mjjh_jnl() As String
        Get
            Return _mjjh_jnl
        End Get
        Set(ByVal value As String)
            _mjjh_jnl = value
        End Set
    End Property

    Private _mjjh_entdate As String
    Public Property mjjh_entdate() As String
        Get
            Return _mjjh_entdate
        End Get
        Set(ByVal value As String)
            _mjjh_entdate = value
        End Set
    End Property

    Private _mjjh_desc As String
    Public Property mjjh_desc() As String
        Get
            Return _mjjh_desc
        End Get
        Set(ByVal value As String)
            _mjjh_desc = value
        End Set
    End Property

    Private _mjjh_jnldate As String
    Public Property mjjh_jnldate() As String
        Get
            Return _mjjh_jnldate
        End Get
        Set(ByVal value As String)
            _mjjh_jnldate = value
        End Set
    End Property

    Private _mjjh_status As String = "R"
    Public ReadOnly Property mjjh_status() As String
        Get
            Return _mjjh_status
        End Get
    End Property

    Private _mjjh_postdate As String = Helper.NullValue
    Public ReadOnly Property mjjh_postdate() As String
        Get
            Return _mjjh_postdate
        End Get
     End Property

    Private _mjjh_nprefid As Integer = 0
    Public ReadOnly Property mjjh_nprefid() As Integer
        Get
            Return _mjjh_nprefid
        End Get
    End Property

    Private _mjjh_curcode As String = Helper.CurrencyCode
    Public ReadOnly Property mjjh_curcode() As String
        Get
            Return _mjjh_curcode
        End Get
    End Property

    Private _mjjh_currate As String = Helper.CurrencyRate
    Public ReadOnly Property mjjh_currate() As String
        Get
            Return _mjjh_currate
        End Get
    End Property

    Private _mjjh_baseunit As String = Helper.BaseUnit
    Public ReadOnly Property mjjh_baseunit() As String
        Get
            Return _mjjh_baseunit
        End Get
    End Property

    Private _mjjh_operator As String = Helper.Operators
    Public ReadOnly Property mjjh_operator() As String
        Get
            Return _mjjh_operator
        End Get
     End Property

    Private _mjjh_who As String
    Public Property mjjh_who() As String
        Get
            Return _mjjh_who
        End Get
        Set(ByVal value As String)
            _mjjh_who = value
        End Set
    End Property

    Private _mjjh_dtime As String = Helper.NullValue
    Public ReadOnly Property mjjh_dtime() As String
        Get
            Return _mjjh_dtime
        End Get
  End Property

    Private _mjjh_ldtime As String = Helper.NullValue
    Public ReadOnly Property mjjh_ldtime() As String
        Get
            Return _mjjh_ldtime
        End Get
    End Property

End Class
