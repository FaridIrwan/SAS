Option Strict On
Option Explicit On

Public Class CfCbPayBatchEn

    Private _cbpb_batchid As String
    Public Property cbpb_batchid() As String
        Get
            Return _cbpb_batchid
        End Get
        Set(ByVal value As String)
            _cbpb_batchid = value
        End Set
    End Property

    Private _cbpb_bank As String
    Public Property cbpb_bank() As String
        Get
            Return _cbpb_bank
        End Get
        Set(ByVal value As String)
            _cbpb_bank = value
        End Set
    End Property

    Private _cbpb_company As String
    Public Property cbpb_company() As String
        Get
            Return _cbpb_company
        End Get
        Set(ByVal value As String)
            _cbpb_company = value
        End Set
    End Property

    Private _cbpb_type As String
    Public Property cbpb_type() As String
        Get
            Return _cbpb_type
        End Get
        Set(ByVal value As String)
            _cbpb_type = value
        End Set
    End Property

    Private _cbpb_batchtot As Integer
    Public Property cbpb_batchtot() As Integer
        Get
            Return _cbpb_batchtot
        End Get
        Set(ByVal value As Integer)
            _cbpb_batchtot = value
        End Set
    End Property

    Private _cbpb_curcode As String = Helper.CurrencyCode
    Public ReadOnly Property cbpb_curcode() As String
        Get
            Return _cbpb_curcode
        End Get
    End Property

    Private _cbpb_currate As String = Helper.CurrencyRate
    Public ReadOnly Property cbpb_currate() As String
        Get
            Return _cbpb_currate
        End Get
    End Property

    Private _cbpb_baseunit As String = Helper.BaseUnit
    Public ReadOnly Property cbpb_baseunit() As String
        Get
            Return _cbpb_baseunit
        End Get
    End Property

    Private _cbpb_operator As String = Helper.Operators
    Public ReadOnly Property cbpb_operator() As String
        Get
            Return _cbpb_operator
        End Get
    End Property

    Private _cbpb_status As String = "READY"
    Public ReadOnly Property cbpb_status() As String
        Get
            Return _cbpb_status
        End Get
    End Property

    Private _cbpb_batchdate As String
    Public Property cbpb_batchdate() As String
        Get
            Return _cbpb_batchdate
        End Get
        Set(ByVal value As String)
            _cbpb_batchdate = value
        End Set
    End Property

    Private _cbpb_usrctltot As String
    Public Property cbpb_usrctltot() As String
        Get
            Return _cbpb_usrctltot
        End Get
        Set(ByVal value As String)
            _cbpb_usrctltot = value
        End Set
    End Property

    Private _cbpb_postdate As String = Helper.NullValue
    Public ReadOnly Property cbpb_postdate() As String
        Get
            Return _cbpb_postdate
        End Get
    End Property

    Private _cbpb_source As String = Helper.Manual
    Public ReadOnly Property cbpb_source() As String
        Get
            Return _cbpb_source
        End Get
    End Property

    Private _cbpb_nprefid As Integer = 1
    Public ReadOnly Property cbpb_nprefid() As Integer
        Get
            Return _cbpb_nprefid
        End Get
    End Property

    Private _cbpb_who As String
    Public Property cbpb_who() As String
        Get
            Return _cbpb_who
        End Get
        Set(ByVal value As String)
            _cbpb_who = value
        End Set
    End Property

    Private _cbpb_dtime As String = Helper.NullValue
    Public Property cbpb_dtime() As String
        Get
            Return _cbpb_dtime
        End Get
        Set(ByVal value As String)
            _cbpb_dtime = value
        End Set
    End Property

    Private _cbpb_ldtime As String = Helper.NullValue
    Public Property cbpb_ldtime() As String
        Get
            Return _cbpb_ldtime
        End Get
        Set(ByVal value As String)
            _cbpb_ldtime = value
        End Set
    End Property

End Class
