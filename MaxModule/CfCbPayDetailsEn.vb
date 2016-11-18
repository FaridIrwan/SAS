Option Strict On
Option Explicit On

Public Class CfCbPayDetailsEn

    Private _cbpr_magic As Integer
    Public Property cbpr_magic() As Integer
        Get
            Return _cbpr_magic
        End Get
        Set(ByVal value As Integer)
            _cbpr_magic = value
        End Set
    End Property

    Private _cbpr_lineno As Integer
    Public Property cbpr_lineno() As Integer
        Get
            Return _cbpr_lineno
        End Get
        Set(ByVal value As Integer)
            _cbpr_lineno = value
        End Set
    End Property

    Private _cbpr_itemdate As String
    Public Property cbpr_itemdate() As String
        Get
            Return _cbpr_itemdate
        End Get
        Set(ByVal value As String)
            _cbpr_itemdate = value
        End Set
    End Property

    Private _cbpr_itemrefno As String
    Public Property cbpr_itemrefno() As String
        Get
            Return _cbpr_itemrefno
        End Get
        Set(ByVal value As String)
            _cbpr_itemrefno = value
        End Set
    End Property

    Private _cbpr_itemamount As Decimal
    Public Property cbpr_itemamount() As Decimal
        Get
            Return _cbpr_itemamount
        End Get
        Set(ByVal value As Decimal)
            _cbpr_itemamount = value
        End Set
    End Property

    Private _cbpr_secondref As String = Helper.NullValue
    Public ReadOnly Property cbpr_secondref() As String
        Get
            Return _cbpr_secondref
        End Get
    End Property

    Private _cbpr_comment As String = Helper.NullValue
    Public ReadOnly Property cbpr_comment() As String
        Get
            Return _cbpr_comment
        End Get
    End Property

    Private _cbpr_rmtamount As Decimal
    Public Property cbpr_rmtamount() As Decimal
        Get
            Return _cbpr_rmtamount
        End Get
        Set(ByVal value As Decimal)
            _cbpr_rmtamount = value
        End Set
    End Property

    Private _cbpr_discount As Decimal = 0
    Public ReadOnly Property cbpr_discount() As Decimal
        Get
            Return _cbpr_discount
        End Get
    End Property

    Private _cbpr_ppsamount As Decimal = 0
    Public ReadOnly Property cbpr_ppsamount() As Decimal
        Get
            Return _cbpr_ppsamount
        End Get
    End Property

    Private _cbpr_ppsnumber As String = Helper.NullValue
    Public ReadOnly Property cbpr_ppsnumber() As String
        Get
            Return _cbpr_ppsnumber
        End Get
    End Property

End Class
