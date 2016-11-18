Option Strict On
Option Explicit On

Public Class CfCbRecHeaderEn

    Private _cbrh_company As String
    Public Property cbrh_company() As String
        Get
            Return _cbrh_company
        End Get
        Set(ByVal value As String)
            _cbrh_company = value
        End Set
    End Property

    Private _cbrh_batchid As String
    Public Property cbrh_batchid() As String
        Get
            Return _cbrh_batchid
        End Get
        Set(ByVal value As String)
            _cbrh_batchid = value
        End Set
    End Property

    Private _cbrh_lineno As Integer
    Public Property cbrh_lineno() As Integer
        Get
            Return _cbrh_lineno
        End Get
        Set(ByVal value As Integer)
            _cbrh_lineno = value
        End Set
    End Property

    Private _cbrh_rcptid As String
    Public Property cbrh_rcptid() As String
        Get
            Return _cbrh_rcptid
        End Get
        Set(ByVal value As String)
            _cbrh_rcptid = value
        End Set
    End Property

    Private _cbrh_refno As String
    Public Property cbrh_refno() As String
        Get
            Return _cbrh_refno
        End Get
        Set(ByVal value As String)
            _cbrh_refno = value
        End Set
    End Property

    Private _cbrh_desc As String
    Public Property cbrh_desc() As String
        Get
            Return _cbrh_desc
        End Get
        Set(ByVal value As String)
            _cbrh_desc = value
        End Set
    End Property

    Private _cbrh_payer As String
    Public Property cbrh_payer() As String
        Get
            Return _cbrh_payer
        End Get
        Set(ByVal value As String)
            _cbrh_payer = value
        End Set
    End Property

    Private _cbrh_payeraddr1 As String = Helper.NullValue
    Public ReadOnly Property cbrh_payeraddr1() As String
        Get
            Return _cbrh_payeraddr1
        End Get
    End Property

    Private _cbrh_payeraddr2 As String = Helper.NullValue
    Public ReadOnly Property cbrh_payeraddr2() As String
        Get
            Return _cbrh_payeraddr2
        End Get
     End Property

    Private _cbrh_payeraddr3 As String = Helper.NullValue
    Public ReadOnly Property cbrh_payeraddr3() As String
        Get
            Return _cbrh_payeraddr3
        End Get
     End Property

    Private _cbrh_payeraddr4 As String = Helper.NullValue
    Public ReadOnly Property cbrh_payeraddr4() As String
        Get
            Return _cbrh_payeraddr4
        End Get
     End Property

    Private _cbrh_cashtype As String
    Public Property cbrh_cashtype() As String
        Get
            Return _cbrh_cashtype
        End Get
        Set(ByVal value As String)
            _cbrh_cashtype = value
        End Set
    End Property

    Private _cbrh_frombank As String
    Public Property cbrh_frombank() As String
        Get
            Return _cbrh_frombank
        End Get
        Set(ByVal value As String)
            _cbrh_frombank = value
        End Set
    End Property

    Private _cbrh_frombranch As String = Helper.NullValue
    Public ReadOnly Property cbrh_frombranch() As String
        Get
            Return _cbrh_frombranch
        End Get
    End Property

    Private _cbrh_amount As Decimal
    Public Property cbrh_amount() As Decimal
        Get
            Return _cbrh_amount
        End Get
        Set(ByVal value As Decimal)
            _cbrh_amount = value
        End Set
    End Property

    Private _cbrh_vatamount As String = Helper.NullValue
    Public ReadOnly Property cbrh_vatamount() As String
        Get
            Return _cbrh_vatamount
        End Get
    End Property

    Private _cbrh_lclamount As Decimal
    Public Property cbrh_lclamount() As Decimal
        Get
            Return _cbrh_lclamount
        End Get
        Set(ByVal value As Decimal)
            _cbrh_lclamount = value
        End Set
    End Property

    Private _cbrh_magic As Integer
    Public Property cbrh_magic() As Integer
        Get
            Return _cbrh_magic
        End Get
        Set(ByVal value As Integer)
            _cbrh_magic = value
        End Set
    End Property

End Class
