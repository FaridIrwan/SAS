Imports System.Web
Imports System.Web.Services
Imports System.Web.Services.Protocols
Imports System.Data
Imports BusinessEntities


' To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line.
' <System.Web.Script.Services.ScriptService()> _
<WebService(Namespace:="http://tempuri.org/")> _
<WebServiceBinding(ConformsTo:=WsiProfiles.BasicProfile1_1)> _
<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Public Class wsLedger
    Inherits System.Web.Services.WebService

    Dim StudentLedgerDL As New SQLPowerQueryManager.PowerQueryManager.StudentLedgerDL
    Dim PrePaymentDL As New SQLPowerQueryManager.PowerQueryManager.PrePaymentDL
    Dim dsReturn As New DataSet

    <System.Web.Services.WebMethod()> _
    Public Function GetStudentLedger(ByVal strMatricNo As String) As DataSet

        Dim AccountsEn As New AccountsEn
        AccountsEn.CreditRef = strMatricNo
        AccountsEn.PostStatus = "Posted"
        AccountsEn.SubType = "Student"

        Try
            dsReturn = StudentLedgerDL.GetStudentLedger(AccountsEn)
        Catch ex As Exception
            Throw ex
        End Try

        Return dsReturn

    End Function

    <System.Web.Services.WebMethod()> _
    Public Function GetStudentData(ByVal strMatricNo As String) As DataSet

        Dim StudentEn As New StudentEn
        Dim strStudentName As String = String.Empty

        StudentEn.SASI_MatricNo = strMatricNo

        Try
            dsReturn = StudentLedgerDL.GetStudentData(StudentEn)

        Catch ex As Exception
            Throw ex
        End Try

        Return dsReturn

    End Function

    <System.Web.Services.WebMethod()> _
    Public Function GetPrepayment(ByVal strMatricNo As String) As DataSet

        Dim StudentEn As New StudentEn
        Dim strStudentName As String = String.Empty
        StudentEn.SASI_MatricNo = strMatricNo

        Try
            dsReturn = PrePaymentDL.GetPrePayment(StudentEn)
        Catch ex As Exception
            Throw ex
        End Try
        Return dsReturn

    End Function

    <System.Web.Services.WebMethod()> _
    Public Function GetStudentOutstanding(ByVal strMatricNo As String) As DataSet

        Dim StudentEn As New StudentEn
        Dim strStudentName As String = String.Empty
        StudentEn.SASI_MatricNo = strMatricNo

        Try
            dsReturn = StudentLedgerDL.GetStudentOutstanding(StudentEn)
        Catch ex As Exception
            Throw ex
        End Try
        Return dsReturn

    End Function

End Class