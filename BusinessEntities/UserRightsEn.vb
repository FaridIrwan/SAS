Public Class UserRightsEn

    Private _UserGroupId As Integer
    Public Property UserGroupId() As Integer
        Get
            Return _UserGroupId
        End Get
        Set(ByVal value As Integer)
            _UserGroupId = value
        End Set
    End Property

    Private _MenuID As Integer
    Public Property MenuID() As Integer
        Get
            Return _MenuID
        End Get
        Set(ByVal value As Integer)
            _MenuID = value
        End Set
    End Property

    Private _IsAdd As Boolean
    Public Property IsAdd() As Boolean
        Get
            Return _IsAdd
        End Get
        Set(ByVal value As Boolean)
            _IsAdd = value
        End Set
    End Property

    Private _IsEdit As Boolean
    Public Property IsEdit() As Boolean
        Get
            Return _IsEdit
        End Get
        Set(ByVal value As Boolean)
            _IsEdit = value
        End Set
    End Property

    Private _IsDelete As Boolean
    Public Property IsDelete() As Boolean
        Get
            Return _IsDelete
        End Get
        Set(ByVal value As Boolean)
            _IsDelete = value
        End Set
    End Property

    Private _IsView As Boolean
    Public Property IsView() As Boolean
        Get
            Return _IsView
        End Get
        Set(ByVal value As Boolean)
            _IsView = value
        End Set
    End Property

    Private _IsPrint As Boolean
    Public Property IsPrint() As Boolean
        Get
            Return _IsPrint
        End Get
        Set(ByVal value As Boolean)
            _IsPrint = value
        End Set
    End Property

    Private _IsPost As Boolean
    Public Property IsPost() As Boolean
        Get
            Return _IsPost
        End Get
        Set(ByVal value As Boolean)
            _IsPost = value
        End Set
    End Property

    Private _IsOthers As Boolean
    Public Property IsOthers() As Boolean
        Get
            Return _IsOthers
        End Get
        Set(ByVal value As Boolean)
            _IsOthers = value
        End Set
    End Property

    Private _DefaultMode As Boolean
    Public Property DefaultMode() As Boolean
        Get
            Return _DefaultMode
        End Get
        Set(ByVal value As Boolean)
            _DefaultMode = value
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
