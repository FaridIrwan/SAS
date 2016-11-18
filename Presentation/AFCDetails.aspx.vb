#Region "NameSpaces "
Imports MaxGeneric
Imports System.Data
Imports HTS.SAS.BusinessObjects
Imports HTS.SAS.Entities
#End Region

Partial Class AFCDetails
    Inherits System.Web.UI.Page

#Region "Global Declarations "
    Private _WorkflowDAL As New HTS.SAS.DataAccessObjects.WorkflowDAL
    Private _AccountsDAL As New HTS.SAS.DataAccessObjects.AccountsDAL
    Private _Helper As New Helper
#End Region

#Region "Page_Load "

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Try

            If Not Page.IsPostBack Then

                DirectCast(Master.FindControl("Panel1"), System.Web.UI.WebControls.Panel).Visible = False
                DirectCast(Master.FindControl("td"), System.Web.UI.HtmlControls.HtmlTableCell).Visible = False

                Dim TransID As String = MaxGeneric.clsGeneric.NullToString(Request.QueryString("TransID"))
                'Dim Category As String = MaxGeneric.clsGeneric.NullToString(Request.QueryString("Category"))
                'Dim Type As String = MaxGeneric.clsGeneric.NullToString(Request.QueryString("SubType"))
                'added by Hafiz @ 31/3/2016
                Dim MatricNo As String = MaxGeneric.clsGeneric.NullToString(Request.QueryString("MatricNo"))
                'added by Mona @ 30/6/2016
                Dim StudentCurrentSem As String = MaxGeneric.clsGeneric.NullToString(Request.QueryString("CurSem"))

                'Call Bind Grid
                'Call BindGrid(TransID, Category, Type)
                Call BindGrid(TransID, MatricNo, StudentCurrentSem)

                'While loading the page make the CFlag as null
                Session("PageMode") = ""
                'Loading User Rights
                ' LoadUserRights()

                'while loading list object make it nothing
                'Session("ListObj") = Nothing
                ' DisableRecordNavigator()
                'load PageName
                '  Menuname(CInt(Request.QueryString("Menuid")))
            End If

        Catch ex As Exception

            Call MaxModule.Helper.LogError(ex.Message)

        End Try

    End Sub
#End Region

#Region "Bind Grid "

    'Private Sub BindGrid(ByVal TransID As String, Optional ByVal Category As String = "", Optional ByVal SubType As String = "")
    'mofified by Hafiz @ 31/3/2016 - adding MatricNo params
    'Private Sub BindGrid(ByVal TransID As String, Optional ByVal Category As String = "", Optional ByVal SubType As String = "", Optional ByVal MatricNo As String = "",
    '                     Optional ByVal CurSem As String = "")
    'mofified by Mona @ 30/6/2016
    Private Sub BindGrid(ByVal TransID As String, Optional ByVal MatricNo As String = "", Optional ByVal CurSem As String = "")

        'Create Instances
        Dim WorkFlowAccountDetails As DataSet = Nothing
        Dim ProgStudentDetails As DataSet = Nothing
        Dim StudentFeeDetails As DataSet = Nothing
        'varaible declearation
        'Dim TransID As Integer = Nothing
        Dim StudentCount As Integer = 0
        Dim BatchAmount As Decimal = 0

        Try

            StudentFeeDetails = _AccountsDAL.GetAFCDetails(MatricNo, CurSem, TransID)
            If StudentFeeDetails.Tables(0).Rows.Count = 0 Then
                lblMsg.Text = "No Records"
            Else
                dgFeeType.DataSource = StudentFeeDetails
                dgFeeType.DataBind()
            End If

        Catch ex As Exception

            MaxModule.Helper.LogError(ex.Message)

        End Try

    End Sub

#End Region

End Class
