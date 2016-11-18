Public Class AccountsEn

    Private _TransID As Integer
    Public Property TransID() As Integer
        Get
            Return _TransID
        End Get
        Set(ByVal value As Integer)
            _TransID = value
        End Set
    End Property

    Private _TransTempCode As String
    Public Property TransTempCode() As String
        Get
            Return _TransTempCode
        End Get
        Set(ByVal value As String)
            _TransTempCode = value
        End Set
    End Property

    Private _TransCode As String
    Public Property TransCode() As String
        Get
            Return _TransCode
        End Get
        Set(ByVal value As String)
            _TransCode = value
        End Set
    End Property

    Private _CreditRef As String
    Public Property CreditRef() As String
        Get
            Return _CreditRef
        End Get
        Set(ByVal value As String)
            _CreditRef = value
        End Set
    End Property

    Private _CreditRef1 As String
    Public Property CreditRef1() As String
        Get
            Return _CreditRef1
        End Get
        Set(ByVal value As String)
            _CreditRef1 = value
        End Set
    End Property

    Private _DebitRef As String
    Public Property DebitRef() As String
        Get
            Return _DebitRef
        End Get
        Set(ByVal value As String)
            _DebitRef = value
        End Set
    End Property

    Private _DebitRef1 As String
    Public Property DebitRef1() As String
        Get
            Return _DebitRef1
        End Get
        Set(ByVal value As String)
            _DebitRef1 = value
        End Set
    End Property

    Private _Category As String
    Public Property Category() As String
        Get
            Return _Category
        End Get
        Set(ByVal value As String)
            _Category = value
        End Set
    End Property

    Private _SubCategory As String
    Public Property SubCategory() As String
        Get
            Return _SubCategory
        End Get
        Set(ByVal value As String)
            _SubCategory = value
        End Set
    End Property

    Private _TransType As String
    Public Property TransType() As String
        Get
            Return _TransType
        End Get
        Set(ByVal value As String)
            _TransType = value
        End Set
    End Property

    Private _SubType As String
    Public Property SubType() As String
        Get
            Return _SubType
        End Get
        Set(ByVal value As String)
            _SubType = value
        End Set
    End Property

    Private _SourceType As String
    Public Property SourceType() As String
        Get
            Return _SourceType
        End Get
        Set(ByVal value As String)
            _SourceType = value
        End Set
    End Property

    Private _TransDate As DateTime
    Public Property TransDate() As DateTime
        Get
            Return _TransDate
        End Get
        Set(ByVal value As DateTime)
            _TransDate = value
        End Set
    End Property

    Private _DueDate As DateTime
    Public Property DueDate() As DateTime
        Get
            Return _DueDate
        End Get
        Set(ByVal value As DateTime)
            _DueDate = value
        End Set
    End Property

    Private _BatchCode As String
    Public Property BatchCode() As String
        Get
            Return _BatchCode
        End Get
        Set(ByVal value As String)
            _BatchCode = value
        End Set
    End Property

    Private _BatchIntake As String
    Public Property BatchIntake() As String
        Get
            Return _BatchIntake
        End Get
        Set(ByVal value As String)
            _BatchIntake = value
        End Set
    End Property

    Private _BatchDate As DateTime
    Public Property BatchDate() As DateTime
        Get
            Return _BatchDate
        End Get
        Set(ByVal value As DateTime)
            _BatchDate = value
        End Set
    End Property

    Private _CrRef1 As String
    Public Property CrRef1() As String
        Get
            Return _CrRef1
        End Get
        Set(ByVal value As String)
            _CrRef1 = value
        End Set
    End Property

    Private _CrRef2 As String
    Public Property CrRef2() As String
        Get
            Return _CrRef2
        End Get
        Set(ByVal value As String)
            _CrRef2 = value
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

    Private _Currency As String
    Public Property Currency() As String
        Get
            Return _Currency
        End Get
        Set(ByVal value As String)
            _Currency = value
        End Set
    End Property

    Private _BatchTotal As Double
    Public Property BatchTotal() As Double
        Get
            Return _BatchTotal
        End Get
        Set(ByVal value As Double)
            _BatchTotal = value
        End Set
    End Property

    Private _Tax As Double
    Public Property Tax() As Double
        Get
            Return _Tax
        End Get
        Set(ByVal value As Double)
            _Tax = value
        End Set
    End Property

    Private _Discount As Double
    Public Property Discount() As Double
        Get
            Return _Discount
        End Get
        Set(ByVal value As Double)
            _Discount = value
        End Set
    End Property

    Private _TaxAmount As Double
    Public Property TaxAmount() As Double
        Get
            Return _TaxAmount
        End Get
        Set(ByVal value As Double)
            _TaxAmount = value
        End Set
    End Property

    Private _DiscountAmount As Double
    Public Property DiscountAmount() As Double
        Get
            Return _DiscountAmount
        End Get
        Set(ByVal value As Double)
            _DiscountAmount = value
        End Set
    End Property

    Private _TransAmount As Double
    Public Property TransAmount() As Double
        Get
            Return _TransAmount
        End Get
        Set(ByVal value As Double)
            _TransAmount = value
        End Set
    End Property

    Private _PaidAmount As Double
    Public Property PaidAmount() As Double
        Get
            Return _PaidAmount
        End Get
        Set(ByVal value As Double)
            _PaidAmount = value
        End Set
    End Property

    Private _TransStatus As String
    Public Property TransStatus() As String
        Get
            Return _TransStatus
        End Get
        Set(ByVal value As String)
            _TransStatus = value
        End Set
    End Property

    Private _TempAmount As Double
    Public Property TempAmount() As Double
        Get
            Return _TempAmount
        End Get
        Set(ByVal value As Double)
            _TempAmount = value
        End Set
    End Property

    Private _TempPaidAmount As Double
    Public Property TempPaidAmount() As Double
        Get
            Return _TempPaidAmount
        End Get
        Set(ByVal value As Double)
            _TempPaidAmount = value
        End Set
    End Property

    Private _PaymentMode As String
    Public Property PaymentMode() As String
        Get
            Return _PaymentMode
        End Get
        Set(ByVal value As String)
            _PaymentMode = value
        End Set
    End Property

    Private _BankCode As String
    Public Property BankCode() As String
        Get
            Return _BankCode
        End Get
        Set(ByVal value As String)
            _BankCode = value
        End Set
    End Property

    Private _PayeeName As String
    Public Property PayeeName() As String
        Get
            Return _PayeeName
        End Get
        Set(ByVal value As String)
            _PayeeName = value
        End Set
    End Property

    Private _ChequeDate As DateTime
    Public Property ChequeDate() As DateTime
        Get
            Return _ChequeDate
        End Get
        Set(ByVal value As DateTime)
            _ChequeDate = value
        End Set
    End Property

    Private _ChequeNo As String
    Public Property ChequeNo() As String
        Get
            Return _ChequeNo
        End Get
        Set(ByVal value As String)
            _ChequeNo = value
        End Set
    End Property

    Private _VoucherNo As String
    Public Property VoucherNo() As String
        Get
            Return _VoucherNo
        End Get
        Set(ByVal value As String)
            _VoucherNo = value
        End Set
    End Property

    Private _PocketAmount As String
    Public Property PocketAmount() As String
        Get
            Return _PocketAmount
        End Get
        Set(ByVal value As String)
            _PocketAmount = value
        End Set
    End Property

    Private _SubRef1 As String
    Public Property SubRef1() As String
        Get
            Return _SubRef1
        End Get
        Set(ByVal value As String)
            _SubRef1 = value
        End Set
    End Property

    Private _SubRef2 As String
    Public Property SubRef2() As String
        Get
            Return _SubRef2
        End Get
        Set(ByVal value As String)
            _SubRef2 = value
        End Set
    End Property

    Private _SubRef3 As String
    Public Property SubRef3() As String
        Get
            Return _SubRef3
        End Get
        Set(ByVal value As String)
            _SubRef3 = value
        End Set
    End Property

    Private _PostStatus As String
    Public Property PostStatus() As String
        Get
            Return _PostStatus
        End Get
        Set(ByVal value As String)
            _PostStatus = value
        End Set
    End Property

    Private _IntStatus As String
    Public Property IntStatus() As String
        Get
            Return _IntStatus
        End Get
        Set(ByVal value As String)
            _IntStatus = value
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

    Private _CreatedDateTime As DateTime
    Public Property CreatedDateTime() As DateTime
        Get
            Return _CreatedDateTime
        End Get
        Set(ByVal value As DateTime)
            _CreatedDateTime = value
        End Set
    End Property

    Private _PostedBy As String
    Public Property PostedBy() As String
        Get
            Return _PostedBy
        End Get
        Set(ByVal value As String)
            _PostedBy = value
        End Set
    End Property

    Private _PostedDateTime As DateTime
    Public Property PostedDateTime() As DateTime
        Get
            Return _PostedDateTime
        End Get
        Set(ByVal value As DateTime)
            _PostedDateTime = value
        End Set
    End Property

    Private _IntCode As String
    Public Property IntCode() As String
        Get
            Return _IntCode
        End Get
        Set(ByVal value As String)
            _IntCode = value
        End Set
    End Property

    Private _GLCode As String
    Public Property GLCode() As String
        Get
            Return _GLCode
        End Get
        Set(ByVal value As String)
            _GLCode = value
        End Set
    End Property

    Private _UpdatedBy As String
    Public Property UpdatedBy() As String
        Get
            Return _UpdatedBy
        End Get
        Set(ByVal value As String)
            _UpdatedBy = value
        End Set
    End Property

    Private _UpdatedTime As DateTime
    Public Property UpdatedTime() As DateTime
        Get
            Return _UpdatedTime
        End Get
        Set(ByVal value As DateTime)
            _UpdatedTime = value
        End Set
    End Property

    Private _KodUniversiti As String
    Public Property KodUniversiti() As String
        Get
            Return _KodUniversiti
        End Get
        Set(ByVal value As String)
            _KodUniversiti = value
        End Set
    End Property

    Private _KumpulanPelajar As String
    Public Property KumpulanPelajar() As String
        Get
            Return _KumpulanPelajar
        End Get
        Set(ByVal value As String)
            _KumpulanPelajar = value
        End Set
    End Property

    Private _TarikhProses As String
    Public Property TarikhProses() As String
        Get
            Return _TarikhProses
        End Get
        Set(ByVal value As String)
            _TarikhProses = value
        End Set
    End Property

    Private _KodBank As String
    Public Property KodBank() As String
        Get
            Return _KodBank
        End Get
        Set(ByVal value As String)
            _KodBank = value
        End Set
    End Property

    Private _NoKelompok As String
    Public Property NoKelompok() As String
        Get
            Return _NoKelompok
        End Get
        Set(ByVal value As String)
            _NoKelompok = value
        End Set
    End Property

    Private _NoWarran As String
    Public Property NoWarran() As String
        Get
            Return _NoWarran
        End Get
        Set(ByVal value As String)
            _NoWarran = value
        End Set
    End Property

    Private _AmaunWarran As String
    Public Property AmaunWarran() As String
        Get
            Return _AmaunWarran
        End Get
        Set(ByVal value As String)
            _AmaunWarran = value
        End Set
    End Property

    Private _noAkaun As String
    Public Property noAkaun() As String
        Get
            Return _noAkaun
        End Get
        Set(ByVal value As String)
            _noAkaun = value
        End Set
    End Property

    Private _StatusBayaran As String
    Public Property StatusBayaran() As String
        Get
            Return _StatusBayaran
        End Get
        Set(ByVal value As String)
            _StatusBayaran = value
        End Set
    End Property

    Private _PostToCF As Integer
    Public Property PostToCF() As Integer
        Get
            Return _PostToCF
        End Get
        Set(ByVal value As Integer)
            _PostToCF = value
        End Set
    End Property

    Private _sb_flag As String
    Public Property sb_flag() As String
        Get
            Return _sb_flag
        End Get
        Set(ByVal value As String)
            _sb_flag = value
        End Set
    End Property

End Class
