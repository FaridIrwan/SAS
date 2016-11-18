
Public Class UR_Config

    Private _CON_Code As Integer
    Public Property CON_Code() As Integer
        Get
            Return _CON_Code
        End Get
        Set(ByVal value As Integer)
            _CON_Code = value
        End Set
    End Property

    Private _CON_CodeID As String
    Public Property CON_CodeID() As String
        Get
            Return _CON_CodeID
        End Get
        Set(ByVal value As String)
            _CON_CodeID = value
        End Set
    End Property

    Private _CON_Category As String
    Public Property CON_Category() As String
        Get
            Return _CON_Category
        End Get
        Set(ByVal value As String)
            _CON_Category = value
        End Set
    End Property

    Private _CON_Value1 As String
    Public Property CON_Value1() As String
        Get
            Return _CON_Value1
        End Get
        Set(ByVal value As String)
            _CON_Value1 = value
        End Set
    End Property

    Private _CON_Value2 As String
    Public Property CON_Value2() As String
        Get
            Return _CON_Value2
        End Get
        Set(ByVal value As String)
            _CON_Value2 = value
        End Set
    End Property

    Private _CON_Value3 As String
    Public Property CON_Value3() As String
        Get
            Return _CON_Value3
        End Get
        Set(ByVal value As String)
            _CON_Value3 = value
        End Set
    End Property

    Private _CON_UpdatedDate As Date
    Public Property CON_UpdatedDate() As Date
        Get
            Return _CON_UpdatedDate
        End Get
        Set(ByVal value As Date)
            _CON_UpdatedDate = value
        End Set
    End Property

    Private _CON_UpdatedBy As String
    Public Property CON_UpdatedBy() As String
        Get
            Return _CON_UpdatedBy
        End Get
        Set(ByVal value As String)
            _CON_UpdatedBy = value
        End Set
    End Property

End Class
