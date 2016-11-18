#Region "NameSpaces "
Imports MaxGeneric
Imports System.Data
Imports HTS.SAS.BusinessObjects
Imports HTS.SAS.Entities
#End Region
Partial Class WorkFlowStudentAccountView
    Inherits System.Web.UI.Page
#Region "Global Declarations "
    Private _WorkflowDAL As New HTS.SAS.DataAccessObjects.WorkflowDAL
    Private _Helper As New Helper
#End Region

#Region "Page_Load "

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Try

            If Not Page.IsPostBack Then

                DirectCast(Master.FindControl("Panel1"), System.Web.UI.WebControls.Panel).Visible = False
                DirectCast(Master.FindControl("td"), System.Web.UI.HtmlControls.HtmlTableCell).Visible = False

                Dim TransID As String = MaxGeneric.clsGeneric.NullToString(Request.QueryString("TransID"))
                Dim Category As String = MaxGeneric.clsGeneric.NullToString(Request.QueryString("Category"))
                Dim Type As String = MaxGeneric.clsGeneric.NullToString(Request.QueryString("SubType"))
                'added by Hafiz @ 31/3/2016
                Dim MatricNo As String = MaxGeneric.clsGeneric.NullToString(Request.QueryString("MatricNo"))

                'Call Bind Grid
                'Call BindGrid(TransID, Category, Type)
                Call BindGrid(TransID, Category, Type, MatricNo)

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
    'modified by Hafiz @ 27/7/2016 - edit AFC related

    Private Sub BindGrid(ByVal TransID As String, Optional ByVal Category As String = "", Optional ByVal SubType As String = "", Optional ByVal MatricNo As String = "")

        'Create Instances
        Dim WorkFlowAccountDetails As DataSet = Nothing
        Dim ProgStudentDetails As DataSet = Nothing
        'varaible declearation
        'Dim TransID As Integer = Nothing
        Dim StudentCount As Integer = 0
        Dim BatchAmount As Decimal = 0

        Try

            'get WorkFlowAccount Details
            'WorkFlowAccountDetails = _WorkflowDAL.WorkFlowSASAcountStudentDetails(TransID)
            'WorkFlowAccountDetails = _WorkflowDAL.WorkFlowSASAcountStudentDetails(TransID, Category, SubType)
            WorkFlowAccountDetails = _WorkflowDAL.WorkFlowSASAcountStudentDetails(TransID, Category, SubType, MatricNo)

            'if data available - Start
            If WorkFlowAccountDetails.Tables(0).Rows.Count = 0 Then
                lblMsg.Text = "No Records"
            Else

                If Not Category = "AFC" Then

                    'enable labels
                    lblCount.Visible = True
                    lblstucount.Visible = True
                    lblAmount.Visible = True
                    lblbatch.Visible = True

                    StudentCount = WorkFlowAccountDetails.Tables(0).Rows.Count
                    BatchAmount = WorkFlowAccountDetails.Tables(0).Compute("SUM(transamount)", "")

                    'asssing values
                    lblstucount.Text = StudentCount

                    lblbatch.Text = _Helper.DecimalFormat(BatchAmount)

                End If

            End If
            'if data available - Ended

            If WorkFlowAccountDetails.Tables(0).Rows.Count > 0 Then

                'Bind Data grid - Start
                dgWorkFlowStudentAccount.DataSource = WorkFlowAccountDetails
                dgWorkFlowStudentAccount.DataBind()

                If SubType = "Sponsor" Then

                    If Category = "Invoice" Then

                        'Hide Label
                        fsDetails.Visible = False
                        lblCount.Visible = False
                        lblstucount.Visible = False
                        lblAmount.Visible = False
                        lblbatch.Visible = False
                        lblSponsorCode.Visible = False
                        lblCode.Visible = False
                        lblSponsorName.Visible = False
                        lblName.Visible = False
                        lblSpnAmt.Visible = False
                        lblAmt.Visible = False
                        lblAmt.Text = _Helper.DecimalFormat(BatchAmount)

                        dgWorkFlowStudentAccount.Columns(0).Visible = True
                        dgWorkFlowStudentAccount.Columns(1).Visible = True
                        dgWorkFlowStudentAccount.Columns(2).Visible = False
                        dgWorkFlowStudentAccount.Columns(3).Visible = False
                        dgWorkFlowStudentAccount.Columns(4).Visible = False
                        dgWorkFlowStudentAccount.Columns(5).Visible = False
                        dgWorkFlowStudentAccount.Columns(6).Visible = False

                    Else

                        'hide Label
                        fsDetails.Visible = False
                        lblCount.Visible = False
                        lblstucount.Visible = False
                        lblAmount.Visible = False
                        lblbatch.Visible = False
                        lblSponsorCode.Visible = False
                        lblCode.Visible = False
                        lblSponsorName.Visible = False
                        lblName.Visible = False
                        lblSpnAmt.Visible = False
                        lblAmt.Visible = False

                        dgWorkFlowStudentAccount.Columns(0).Visible = False
                        dgWorkFlowStudentAccount.Columns(1).Visible = False
                        dgWorkFlowStudentAccount.Columns(2).Visible = False
                        dgWorkFlowStudentAccount.Columns(3).Visible = False
                        dgWorkFlowStudentAccount.Columns(4).Visible = True
                        dgWorkFlowStudentAccount.Columns(5).Visible = True
                        dgWorkFlowStudentAccount.Columns(6).Visible = False

                    End If

                Else

                    If Not Category = "AFC" Then
                        'hide Label
                        lblSponsorCode.Visible = False
                        lblCode.Visible = False
                        lblSponsorName.Visible = False
                        lblName.Visible = False
                        lblSpnAmt.Visible = False
                        lblAmt.Visible = False

                        dgWorkFlowStudentAccount.Columns(0).Visible = False
                        dgWorkFlowStudentAccount.Columns(1).Visible = False
                        dgWorkFlowStudentAccount.Columns(2).Visible = True
                        dgWorkFlowStudentAccount.Columns(3).Visible = True
                        dgWorkFlowStudentAccount.Columns(4).Visible = False
                        dgWorkFlowStudentAccount.Columns(5).Visible = False

                        'If Category = "AFC" Then
                        '    dgWorkFlowStudentAccount.Columns(6).Visible = True
                        'End If
                    Else

                        tblStudAcc.Visible = False

                        dgWorkFlowStudentAccount.DataSource = Nothing
                        dgWorkFlowStudentAccount.DataBind()

                    End If

                End If
                'Bind Data grid - Stop

                If Request.QueryString("Formid") = "FS" Then
                    fsDetails.Visible = False
                    lblCount.Visible = False
                    lblstucount.Visible = False
                    lblAmount.Visible = False
                    lblbatch.Visible = False
                    lblSponsorCode.Visible = False
                    lblCode.Visible = False
                    lblSponsorName.Visible = False
                    lblName.Visible = False
                    lblSpnAmt.Visible = False
                    lblAmt.Visible = False
                End If

                If WorkFlowAccountDetails.Tables(0).Rows.Count < 10 Then
                    dgWorkFlowStudentAccount.PagerStyle.Visible = False
                End If

            End If

            If Category = "AFC" Then

                ProgStudentDetails = _WorkflowDAL.GetProgrambySemIntake(TransID)

                If ProgStudentDetails.Tables(0).Rows.Count > 0 Then
                    dgProgram.Visible = True
                    dgProgram.DataSource = ProgStudentDetails
                    dgProgram.DataBind()
                End If

            End If

        Catch ex As Exception

            MaxModule.Helper.LogError(ex.Message)

        End Try

    End Sub

#End Region

#Region "Pagination Grid "
    Protected Sub dgWorkFlowStudentAccount_PageIndexChanged(source As Object, e As DataGridPageChangedEventArgs) Handles dgWorkFlowStudentAccount.PageIndexChanged

        'Grid Index - Start
        dgWorkFlowStudentAccount.CurrentPageIndex = e.NewPageIndex
        dgWorkFlowStudentAccount.DataBind()

        'Bind Data grid - Start
        Call BindGrid(0)
        'Bind Data grid - Stop

    End Sub
#End Region

End Class
