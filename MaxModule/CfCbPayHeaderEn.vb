﻿Option Strict On
Option Explicit On

Public Class CfCbPayHeaderEn

    Private _cbph_company As String
    Public Property cbph_company() As String
        Get
            Return _cbph_company
        End Get
        Set(ByVal value As String)
            _cbph_company = value
        End Set
    End Property

    Private _cbph_batchid As String
    Public Property cbph_batchid() As String
        Get
            Return _cbph_batchid
        End Get
        Set(ByVal value As String)
            _cbph_batchid = value
        End Set
    End Property

    Private _cbph_lineno As Integer
    Public Property cbph_lineno() As Integer
        Get
            Return _cbph_lineno
        End Get
        Set(ByVal value As Integer)
            _cbph_lineno = value
        End Set
    End Property

    Private _cbph_pmtid As String = Helper.NullValue
    Public ReadOnly Property cbph_pmtid() As String
        Get
            Return _cbph_pmtid
        End Get
    End Property

    Private _cbph_voucher As String
    Public Property cbph_voucher() As String
        Get
            Return _cbph_voucher
        End Get
        Set(ByVal value As String)
            _cbph_voucher = value
        End Set
    End Property

    Private _cbph_payee As String
    Public Property cbph_payee() As String
        Get
            Return _cbph_payee
        End Get
        Set(ByVal value As String)
            _cbph_payee = value
        End Set
    End Property

    Private _cbph_payeeaddr1 As String = Helper.NullValue
    Public ReadOnly Property cbph_payeeaddr1() As String
        Get
            Return _cbph_payeeaddr1
        End Get
    End Property

    Private _cbph_payeeaddr2 As String = Helper.NullValue
    Public ReadOnly Property cbph_payeeaddr2() As String
        Get
            Return _cbph_payeeaddr2
        End Get
    End Property

    Private _cbph_payeeaddr3 As String = Helper.NullValue
    Public ReadOnly Property cbph_payeeaddr3() As String
        Get
            Return _cbph_payeeaddr3
        End Get
    End Property

    Private _cbph_payeeaddr4 As String = Helper.NullValue
    Public ReadOnly Property cbph_payeeaddr4() As String
        Get
            Return _cbph_payeeaddr4
        End Get
    End Property

    Private _cbph_amount As Decimal
    Public Property cbph_amount() As Decimal
        Get
            Return _cbph_amount
        End Get
        Set(ByVal value As Decimal)
            _cbph_amount = value
        End Set
    End Property

    Private _cbph_vatamount As Decimal = 0
    Public ReadOnly Property cbph_vatamount() As Decimal
        Get
            Return _cbph_vatamount
        End Get
    End Property

    Private _cbph_lclamount As Decimal
    Public Property cbph_lclamount() As Decimal
        Get
            Return _cbph_lclamount
        End Get
        Set(ByVal value As Decimal)
            _cbph_lclamount = value
        End Set
    End Property

    Private _cbph_bankname As String = Helper.NullValue
    Public ReadOnly Property cbph_bankname() As String
        Get
            Return _cbph_bankname
        End Get
    End Property

    Private _cbph_bankbranch As String = Helper.NullValue
    Public ReadOnly Property cbph_bankbranch() As String
        Get
            Return _cbph_bankbranch
        End Get
    End Property

    Private _cbph_bankcode As String
    Public Property cbph_bankcode() As String
        Get
            Return _cbph_bankcode
        End Get
        Set(ByVal value As String)
            _cbph_bankcode = value
        End Set
    End Property

    Private _cbph_bankacct As String = Helper.NullValue
    Public ReadOnly Property cbph_bankacct() As String
        Get
            Return _cbph_bankacct
        End Get
    End Property

    Private _cbph_magic As Integer
    Public Property cbph_magic() As Integer
        Get
            Return _cbph_magic
        End Get
        Set(ByVal value As Integer)
            _cbph_magic = value
        End Set
    End Property


End Class
