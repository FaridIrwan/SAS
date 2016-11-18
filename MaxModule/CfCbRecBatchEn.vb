Option Strict On
Option Explicit On

Public Class CfCbRecBatchEn

    Private _cbrb_batchid As String
    Public Property cbrb_batchid() As String
        Get
            Return _cbrb_batchid
        End Get
        Set(ByVal value As String)
            _cbrb_batchid = value
        End Set
    End Property

    Private _cbrb_bank As String
    Public Property cbrb_bank() As String
        Get
            Return _cbrb_bank
        End Get
        Set(ByVal value As String)
            _cbrb_bank = value
        End Set
    End Property

    Private _cbrb_company As String
    Public Property cbrb_company() As String
        Get
            Return _cbrb_company
        End Get
        Set(ByVal value As String)
            _cbrb_company = value
        End Set
    End Property

    Private _cbrb_batchtot As Integer
    Public Property cbrb_batchtot() As Integer
        Get
            Return _cbrb_batchtot
        End Get
        Set(ByVal value As Integer)
            _cbrb_batchtot = value
        End Set
    End Property

    Private _cbrb_curcode As String = Helper.CurrencyCode
    Public ReadOnly Property cbrb_curcode() As String
        Get
            Return _cbrb_curcode
        End Get
    End Property

    Private _cbrb_currate As String = Helper.CurrencyRate
    Public ReadOnly Property cbrb_currate() As String
        Get
            Return _cbrb_currate
        End Get
    End Property

    Private _cbrb_baseunit As String = Helper.BaseUnit
    Public ReadOnly Property cbrb_baseunit() As String
        Get
            Return _cbrb_baseunit
        End Get
    End Property

    Private _cbrb_operator As String = Helper.Operators
    Public ReadOnly Property cbrb_operator() As String
        Get
            Return _cbrb_operator
        End Get
    End Property

    Private _cbrb_status As String = "READY"
    Public ReadOnly Property cbrb_status() As String
        Get
            Return _cbrb_status
        End Get
    End Property

    Private _cbrb_batchdate As String
    Public Property cbrb_batchdate() As String
        Get
            Return _cbrb_batchdate
        End Get
        Set(ByVal value As String)
            _cbrb_batchdate = value
        End Set
    End Property

    Private _cbrb_usrctltot As String
    Public Property cbrb_usrctltot() As String
        Get
            Return _cbrb_usrctltot
        End Get
        Set(ByVal value As String)
            _cbrb_usrctltot = value
        End Set
    End Property

    Private _cbrb_postdate As String
    Public Property cbrb_postdate() As String
        Get
            Return _cbrb_postdate
        End Get
        Set(ByVal value As String)
            _cbrb_postdate = value
        End Set
    End Property

    Private _cbrb_source As String = Helper.Manual
    Public ReadOnly Property cbrb_source() As String
        Get
            Return _cbrb_source
        End Get
    End Property

    Private _cbrb_nprefid As String = Helper.NullValue
    Public ReadOnly Property cbrb_nprefid() As String
        Get
            Return _cbrb_nprefid
        End Get
    End Property

    Private _cbrb_who As String
    Public Property cbrb_who() As String
        Get
            Return _cbrb_who
        End Get
        Set(ByVal value As String)
            _cbrb_who = value
        End Set
    End Property

    Private _cbrb_dtime As String = Helper.NullValue
    Public ReadOnly Property cbrb_dtime() As String
        Get
            Return _cbrb_dtime
        End Get
    End Property

    Private _cbrb_ldtime As String = Helper.NullValue
    Public ReadOnly Property cbrb_ldtime() As String
        Get
            Return _cbrb_ldtime
        End Get
    End Property

End Class
