Public Class DepartmentEn

    Private _AutoID As Integer
    Public Property AutoID() As Integer
        Get
            Return _AutoID
        End Get
        Set(ByVal value As Integer)
            _AutoID = value
        End Set
    End Property

    Private _DepartmentID As String
    Public Property DepartmentID() As String
        Get
            Return _DepartmentID
        End Get
        Set(ByVal value As String)
            _DepartmentID = value
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

    Private _Status As Boolean
    Public Property Status() As Boolean
        Get
            Return _Status
        End Get
        Set(ByVal value As Boolean)
            _Status = value
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

    Private _CreateDate As DateTime
    Public Property CreateDate() As DateTime
        Get
            Return _CreateDate
        End Get
        Set(ByVal value As DateTime)
            _CreateDate = value
        End Set
    End Property

    Private _ModifiedBy As String
    Public Property ModifiedBy() As String
        Get
            Return _ModifiedBy
        End Get
        Set(ByVal value As String)
            _ModifiedBy = value
        End Set
    End Property

    Private _ModifiedDate As DateTime
    Public Property ModifiedDate() As DateTime
        Get
            Return _ModifiedDate
        End Get
        Set(ByVal value As DateTime)
            _ModifiedDate = value
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

End Class
