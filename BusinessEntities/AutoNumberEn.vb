Public Class AutoNumberEn

        Private _SAAN_Code As Integer
        Public Property SAAN_Code() As Integer
            Get
                Return _SAAN_Code
            End Get
            Set(ByVal value As Integer)
                _SAAN_Code = value
            End Set
        End Property

        Private _SAAN_Des As String
        Public Property SAAN_Des() As String
            Get
                Return _SAAN_Des
            End Get
            Set(ByVal value As String)
                _SAAN_Des = value
            End Set
        End Property

        Private _SAAN_Prefix As String
        Public Property SAAN_Prefix() As String
            Get
                Return _SAAN_Prefix
            End Get
            Set(ByVal value As String)
                _SAAN_Prefix = value
            End Set
        End Property

        Private _SAAN_NoDigit As Integer
        Public Property SAAN_NoDigit() As Integer
            Get
                Return _SAAN_NoDigit
            End Get
            Set(ByVal value As Integer)
                _SAAN_NoDigit = value
            End Set
        End Property

        Private _SAAN_StartNo As Integer
        Public Property SAAN_StartNo() As Integer
            Get
                Return _SAAN_StartNo
            End Get
            Set(ByVal value As Integer)
                _SAAN_StartNo = value
            End Set
        End Property

        Private _SAAN_CurNo As Integer
        Public Property SAAN_CurNo() As Integer
            Get
                Return _SAAN_CurNo
            End Get
            Set(ByVal value As Integer)
                _SAAN_CurNo = value
            End Set
        End Property

        Private _SAAN_AutoNo As String
        Public Property SAAN_AutoNo() As String
            Get
                Return _SAAN_AutoNo
            End Get
            Set(ByVal value As String)
                _SAAN_AutoNo = value
            End Set
        End Property

End Class
