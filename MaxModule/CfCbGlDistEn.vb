Option Strict On
Option Explicit On

Public Class CfCbGlDistEn

    Private _gldi_company As String
    Public Property gldi_company() As String
        Get
            Return _gldi_company
        End Get
        Set(ByVal value As String)
            _gldi_company = value
        End Set
    End Property

    Private _gldi_module As String = "CB"
    Public ReadOnly Property gldi_module() As String
        Get
            Return _gldi_module
        End Get
    End Property

    Private _gldi_itemtype As String = "PAY"
    Public ReadOnly Property gldi_itemtype() As String
        Get
            Return _gldi_itemtype
        End Get
    End Property

    Private _gldi_batchid As String
    Public Property gldi_batchid() As String
        Get
            Return _gldi_batchid
        End Get
        Set(ByVal value As String)
            _gldi_batchid = value
        End Set
    End Property

    Private _gldi_code As String
    Public Property gldi_code() As String
        Get
            Return _gldi_code
        End Get
        Set(ByVal value As String)
            _gldi_code = value
        End Set
    End Property

    Private _gldi_itemref As String
    Public Property gldi_itemref() As String
        Get
            Return _gldi_itemref
        End Get
        Set(ByVal value As String)
            _gldi_itemref = value
        End Set
    End Property

    Private _gldi_itemrefline As Integer
    Public Property gldi_itemrefline() As Integer
        Get
            Return _gldi_itemrefline
        End Get
        Set(ByVal value As Integer)
            _gldi_itemrefline = value
        End Set
    End Property

    Private _gldi_seqno As Integer
    Public Property gldi_seqno() As Integer
        Get
            Return _gldi_seqno
        End Get
        Set(ByVal value As Integer)
            _gldi_seqno = value
        End Set
    End Property

    Private _gldi_glac As String
    Public Property gldi_glac() As String
        Get
            Return _gldi_glac
        End Get
        Set(ByVal value As String)
            _gldi_glac = value
        End Set
    End Property

    Private _gldi_desc As String
    Public Property gldi_desc() As String
        Get
            Return _gldi_desc
        End Get
        Set(ByVal value As String)
            _gldi_desc = value
        End Set
    End Property

    Private _gldi_qty As Integer = 0
    Public ReadOnly Property gldi_qty() As Integer
        Get
            Return _gldi_qty
        End Get
    End Property

    Private _gldi_amount As Decimal
    Public Property gldi_amount() As Decimal
        Get
            Return _gldi_amount
        End Get
        Set(ByVal value As Decimal)
            _gldi_amount = value
        End Set
    End Property

    Private _gldi_posted As String = "N"
    Public ReadOnly Property gldi_posted() As String
        Get
            Return _gldi_posted
        End Get
    End Property

    Private _gldi_modify As String = "C"
    Public ReadOnly Property gldi_modify() As String
        Get
            Return _gldi_modify
        End Get
    End Property

    Private _gldi_serial As Integer
    Public Property gldi_serial() As Integer
        Get
            Return _gldi_serial
        End Get
        Set(ByVal value As Integer)
            _gldi_serial = value
        End Set
    End Property

    Private _gldi_curcode As String = Helper.CurrencyCode
    Public ReadOnly Property gldi_curcode() As String
        Get
            Return _gldi_curcode
        End Get
    End Property

    Private _gldi_leqfor As String = "000"
    Public Property gldi_leqfor() As String
        Get
            Return _gldi_leqfor
        End Get
        Set(ByVal value As String)
            _gldi_leqfor = value
        End Set
    End Property

    Private _gldi_currate As String = Helper.CurrencyRate
    Public ReadOnly Property gldi_currate() As String
        Get
            Return _gldi_currate
        End Get
    End Property

    Private _gldi_baseunit As String = Helper.BaseUnit
    Public ReadOnly Property gldi_baseunit() As String
        Get
            Return _gldi_baseunit
        End Get
    End Property

    Private _gldi_operator As String = Helper.Operators
    Public Property gldi_operator() As String
        Get
            Return _gldi_operator
        End Get
        Set(ByVal value As String)
            _gldi_operator = value
        End Set
    End Property

    Private _gldi_lclamount As Decimal
    Public Property gldi_lclamount() As Decimal
        Get
            Return _gldi_lclamount
        End Get
        Set(ByVal value As Decimal)
            _gldi_lclamount = value
        End Set
    End Property

End Class
