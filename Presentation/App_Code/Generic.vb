#Region "NameSpaces "

Imports System
Imports MaxGeneric
Imports System.Reflection
Imports Microsoft.VisualBasic

#End Region

Public Class Generic

#Region "Log Error "

    'Purpose			: To Track Error in the Applications
    'Author			    : Sujith Sharatchandran - T-Melmax Sdn Bhd
    'Created Date		: 20/04/2015
    Public Shared Sub LogError(ByVal ErrorSource As String, ByVal ErrorMessage As String)

        Call clsGeneric.prcLogError(ErrorMessage, ErrorSource)

    End Sub

#End Region

#Region "Enumerators "

    Public Enum _Menu
        Setup = 2
        Process = 3
        Reports = 4
        Dashboard = 1
        Home = 0
    End Enum

    Public Enum _TaxCode
        SR = 1
        ES = 2
        OS = 3
        DS = 4
        GS = 5
        RS = 6
        AJS = 7
        ES43 = 8
        ZRL = 9
        ZRE = 10

    End Enum

    Public Enum _TaxMode
        Exclusive = 1
        Inclusive = 2
    End Enum

    Public Enum _ProcessType
        Post = 1
        Reject = 2
        Success = 3
    End Enum

    Public Enum _WorkflowType
        Received = 1
        Posted = 2
        Rejected = 3
        Approved = 4
        Reviewed = 5
    End Enum

#End Region

End Class
