Public Class CB_PayrmtEn

    Private _cbpr_magic As String
    Public Property cbpr_magic() As String
        Get
            Return _cbpr_magic
        End Get
        Set(ByVal value As String)
            _cbpr_magic = value
        End Set
    End Property

    Private _cbpr_lineno As String
    Public Property cbpr_lineno() As String
        Get
            Return _cbpr_lineno
        End Get
        Set(ByVal value As String)
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

    Private _cbpr_itemamount As String
    Public Property cbpr_itemamount() As String
        Get
            Return _cbpr_itemamount
        End Get
        Set(ByVal value As String)
            _cbpr_itemamount = value
        End Set
    End Property

    Private _cbpr_secondref As String
    Public Property cbpr_secondref() As String
        Get
            Return _cbpr_secondref
        End Get
        Set(ByVal value As String)
            _cbpr_secondref = value
        End Set
    End Property

    Private _cbpr_comment As String
    Public Property cbpr_comment() As String
        Get
            Return _cbpr_comment
        End Get
        Set(ByVal value As String)
            _cbpr_comment = value
        End Set
    End Property

    Private _cbpr_rmtamount As String
    Public Property cbpr_rmtamount() As String
        Get
            Return _cbpr_rmtamount
        End Get
        Set(ByVal value As String)
            _cbpr_rmtamount = value
        End Set
    End Property

    Private _cbpr_discount As String
    Public Property cbpr_discount() As String
        Get
            Return _cbpr_discount
        End Get
        Set(ByVal value As String)
            _cbpr_discount = value
        End Set
    End Property

    Private _cbpr_ppsamount As String
    Public Property cbpr_ppsamount() As String
        Get
            Return _cbpr_ppsamount
        End Get
        Set(ByVal value As String)
            _cbpr_ppsamount = value
        End Set
    End Property

    Private _cbpr_ppsnumber As String
    Public Property cbpr_ppsnumber() As String
        Get
            Return _cbpr_ppsnumber
        End Get
        Set(ByVal value As String)
            _cbpr_ppsnumber = value
        End Set
    End Property

End Class
