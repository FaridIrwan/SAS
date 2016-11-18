Public Class PTPTNFeeSetupEn
    Private _AutoID As Integer
    Public Property AutoID() As Integer
        Get
            Return _AutoID
        End Get
        Set(ByVal value As Integer)
            _AutoID = value
        End Set
    End Property

    Private _ProgCode As String
    Public Property ProgCode() As String
        Get
            Return _ProgCode
        End Get
        Set(ByVal value As String)
            _ProgCode = value
        End Set
    End Property

    Private _ProgName As String
    Public Property ProgName() As String
        Get
            Return _ProgName
        End Get
        Set(ByVal value As String)
            _ProgName = value
        End Set
    End Property

    Private _ProgFee As Double
    Public Property ProgFee() As Double
        Get
            Return _ProgFee
        End Get
        Set(ByVal value As Double)
            _ProgFee = value
        End Set
    End Property

    Private _CreatedBy As String
    Public Property CreatedBy() As String
        Get
            Return _CreatedBy
        End Get
        Set(ByVal value As String)
            _CreatedBy = value
        End Set
    End Property

    Private _CreatedDate As DateTime
    Public Property CreatedDate() As DateTime
        Get
            Return _CreatedDate
        End Get
        Set(ByVal value As DateTime)
            _CreatedDate = value
        End Set
    End Property

    Private _SQLCase As String
    Public Property SQLCase() As String
        Get
            Return _SQLCase
        End Get
        Set(ByVal value As String)
            _SQLCase = value
        End Set
    End Property

    Private _SearchCriteria As String
    Public Property SearchCriteria() As String
        Get
            Return _SearchCriteria
        End Get
        Set(ByVal value As String)
            _SearchCriteria = value
        End Set
    End Property

    Private _Sortexp As String
    Public Property Sortexp() As String
        Get
            Return _Sortexp
        End Get
        Set(ByVal value As String)
            _Sortexp = value
        End Set
    End Property

    Private _Colname As String
    Public Property Colname() As String
        Get
            Return _Colname
        End Get
        Set(ByVal value As String)
            _Colname = value
        End Set
    End Property
End Class
