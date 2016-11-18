Public Class SemesterSetup

    Private _SAST_Code As String
    Public Property SAST_Code() As String
        Get
            Return _SAST_Code
        End Get
        Set(ByVal value As String)
            _SAST_Code = value
        End Set
    End Property

    Private _SAST_Semester As String
    Public Property SAST_Semester() As String
        Get
            Return _SAST_Semester
        End Get
        Set(ByVal value As String)
            _SAST_Semester = value
        End Set
    End Property

    Private _SAST_Description As String
    Public Property SAST_Description() As String
        Get
            Return _SAST_Description
        End Get
        Set(ByVal value As String)
            _SAST_Description = value
        End Set
    End Property

    Private _SAST_Status As Boolean
    Public Property SAST_Status() As Boolean
        Get
            Return _SAST_Status
        End Get
        Set(ByVal value As Boolean)
            _SAST_Status = value
        End Set
    End Property

    Private _SABR_Code As Integer
    Public Property SABR_Code() As Integer
        Get
            Return _SABR_Code
        End Get
        Set(ByVal value As Integer)
            _SABR_Code = value
        End Set
    End Property

    Private _SAST_UpdatedUser As String
    Public Property SAST_UpdatedUser() As String
        Get
            Return _SAST_UpdatedUser
        End Get
        Set(ByVal value As String)
            _SAST_UpdatedUser = value
        End Set
    End Property

    Private _SAST_UpdatedDtTm As String
    Public Property SAST_UpdatedDtTm() As String
        Get
            Return _SAST_UpdatedDtTm
        End Get
        Set(ByVal value As String)
            _SAST_UpdatedDtTm = value
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
