Public Class UserGroupEn
    Private _UserGroupId As Integer
    Public Property UserGroupId() As Integer
        Get
            Return _UserGroupId
        End Get
        Set(ByVal value As Integer)
            _UserGroupId = value
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

    Private _UserGroupName As String
    Public Property UserGroupName() As String
        Get
            Return _UserGroupName
        End Get
        Set(ByVal value As String)
            _UserGroupName = value
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

    Private _Description As String
    Public Property Description() As String
        Get
            Return _Description
        End Get
        Set(ByVal value As String)
            _Description = value
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
