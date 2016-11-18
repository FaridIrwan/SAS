Public Class UserEn

    Private _UserID As Integer
    Public Property UserID() As Integer
        Get
            Return _UserID
        End Get
        Set(ByVal value As Integer)
            _UserID = value
        End Set
    End Property

    Private _UserName As String
    Public Property UserName() As String
        Get
            Return _UserName
        End Get
        Set(ByVal value As String)
            _UserName = value
        End Set
    End Property

    Private _Password As String
    Public Property Password() As String
        Get
            Return _Password
        End Get
        Set(ByVal value As String)
            _Password = value
        End Set
    End Property

    Private _UserGroupId As Integer
    Public Property UserGroupId() As Integer
        Get
            Return _UserGroupId
        End Get
        Set(ByVal value As Integer)
            _UserGroupId = value
        End Set
    End Property

    Private _Email As String
    Public Property Email() As String
        Get
            Return _Email
        End Get
        Set(ByVal value As String)
            _Email = value
        End Set
    End Property

    Private _Designation As String
    Public Property Designation() As String
        Get
            Return _Designation
        End Get
        Set(ByVal value As String)
            _Designation = value
        End Set
    End Property

    Private _Department As String
    Public Property Department() As String
        Get
            Return _Department
        End Get
        Set(ByVal value As String)
            _Department = value
        End Set
    End Property

    Private _RecStatus As Boolean
    Public Property RecStatus() As Boolean
        Get
            Return _RecStatus
        End Get
        Set(ByVal value As Boolean)
            _RecStatus = value
        End Set
    End Property

    Private _UserStatus As String
    Public Property UserStatus() As String
        Get
            Return _UserStatus
        End Get
        Set(ByVal value As String)
            _UserStatus = value
        End Set
    End Property

    Private _LastUpdatedBy As String
    Public Property LastUpdatedBy() As String
        Get
            Return _LastUpdatedBy
        End Get
        Set(ByVal value As String)
            _LastUpdatedBy = value
        End Set
    End Property

    Private _LastUpdatedDtTm As DateTime
    Public Property LastUpdatedDtTm() As DateTime
        Get
            Return _LastUpdatedDtTm
        End Get
        Set(ByVal value As DateTime)
            _LastUpdatedDtTm = value
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
