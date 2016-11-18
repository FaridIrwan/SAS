'****************************************************************************************************
'Class Name     : Cryption
'ProgId         : MaxModule.Cryption
'Purpose        : All Encryption/Decryption Methods
'Author         : Sujith Sharatchandran - T-Melmax Sdn Bhd
'Created        : 15/03/2006
'*****************************************************************************************************

Imports System.Text
Imports System.Security.Cryptography

Public Class Cryption

#Region "Global Variables "

    Dim arrKey(255), arrBox(255), intPwdLen As Int32
    Dim strCryptText As String, strCryptKey As String

#End Region

#Region "Property Methods "

    Public Property CryptText() As String
        Get
            Return strCryptText
        End Get

        Set(ByVal Value As String)
            strCryptText = Value
        End Set
    End Property

    Public Property CryptKey() As String
        Get
            Return strCryptKey
        End Get

        Set(ByVal Value As String)
            strCryptKey = Value
        End Set
    End Property

#End Region

#Region "Cryptographic Methods "

    '****************************************************************************************************
    'Procedure Name : Crypt()
    'Purpose        : Encrypt/Decrypt String
    'Arguments      : 
    'Return Value   : Encrypted/Decrypted String
    'Author         : Sujith Sharatchandran - T-Melmax Sdn Bhd
    'Created        : 26/08/2003
    '*****************************************************************************************************
    Public Function Crypt() As String

        'variable declarations
        Dim strCryptograph As String = String.Empty, strConstant As String
        Dim intCounter As Int32, strCryptographBy As String = Nothing
        Dim intModResult As Int32, intModResult1 As Int32, strTemp As String

        Try

            intModResult = 0
            intModResult1 = 0

            SecretKey(strCryptKey)

            For intCounter = 1 To Len(strCryptText)

                intModResult = (intModResult + 1) Mod 256
                intModResult1 = (intModResult1 + arrBox(intModResult)) Mod 256
                strTemp = arrBox(intModResult)
                arrBox(intModResult) = arrBox(intModResult1)
                arrBox(intModResult1) = strTemp

                strConstant = arrBox((arrBox(intModResult) + arrBox(intModResult1)) Mod 256)

                strCryptographBy = Asc(Mid(strCryptText, intCounter, 1)) Xor strConstant
                strCryptograph = strCryptograph & Chr(strCryptographBy)

            Next

            Return strCryptograph

        Finally

        End Try

    End Function

    '****************************************************************************************************
    'Procedure Name : PwdInitialize()
    'Purpose        : Password Initializer
    'Arguments      : Encrypt Password
    'Return Value   : N/A
    'Author         : Sujith Sharatchandran - T-Melmax Sdn Bhd
    'Created        : 30/08/2003
    '*****************************************************************************************************
    Private Sub SecretKey(ByVal strEncryptPwd As String)

        'variable declarations
        Dim strSwap As String, intCounter As Int32, intModResult As Int32

        Try

            intPwdLen = Len(strEncryptPwd)
            For intCounter = 0 To 255
                arrKey(intCounter) = Asc(Mid(strEncryptPwd, (intCounter Mod intPwdLen) + 1, 1))
                arrBox(intCounter) = intCounter
            Next

            intModResult = 0
            For intCounter = 0 To 255
                intModResult = (intModResult + arrBox(intCounter) + arrKey(intCounter)) Mod 256
                strSwap = arrBox(intCounter)
                arrBox(intCounter) = arrBox(intModResult)
                arrBox(intModResult) = strSwap
            Next

        Catch ex As Exception

        End Try

    End Sub

#End Region

#Region "Generate MD5 Checksum "

    'Author         : Sujith Sharatchandran - T-Melmax Sdn Bhd
    'Purpose        : To Generate MD5 Check Sum
    'Created        : 20/02/2009
    Public Function GenerateMD5CheckSum(ByVal MD5RequiredText As String) As String

        ' Create a new Stringbuilder
        Dim _StringBuilder As New StringBuilder()

        'Variable
        Dim Counter As Integer = 0

        Try

            'Create a new instance of the MD5CryptoServiceProvider object.
            Dim _MD5CryptoServiceProvider As New MD5CryptoServiceProvider()

            'Convert the input string to a byte array and compute the hash. - Start
            Dim ByteData As Byte() = _MD5CryptoServiceProvider.ComputeHash( _
                Encoding.Default.GetBytes(MD5RequiredText))
            'Convert the input string to a byte array and compute the hash. - Stop

            'Loop through each byte of the hashed data and  - Start
            For Counter = 0 To ByteData.Length - 1
                'format each one as a hexadecimal string.
                _StringBuilder.Append(ByteData(Counter).ToString("x2"))
            Next Counter
            'Loop through each byte of the hashed data and  - Stop

            'Return the hexadecimal string.
            Return _StringBuilder.ToString()

        Finally

        End Try

    End Function

#End Region

#Region "Verify MD5 Checksum "

    'Author         : Sujith Sharatchandran - T-Melmax Sdn Bhd
    'Purpose        : To Verify MD5 Check Sum
    'Created        : 20/02/2009
    Public Function VerifyMD5CheckSum(ByVal MD5Text As String, ByVal MD5Value As String) As Boolean

        Try

            'Hash the input.
            Dim MD5CheckSum As String = GenerateMD5CheckSum(MD5Text)

            ' Create a StringComparer an compare the hashes.
            Dim _StringComparer As StringComparer = StringComparer.OrdinalIgnoreCase

            'Check if MD5 Checksum is Correct - Start
            If 0 = _StringComparer.Compare(MD5CheckSum, MD5Value) Then
                'Return True if Valid
                Return True
            Else
                'Return False if Invalid
                Return False
            End If
            'Check if MD5 Checksum is Correct - Stop

        Finally

        End Try

    End Function

#End Region

End Class
