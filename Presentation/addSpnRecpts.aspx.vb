Imports HTS.SAS.Entities
Imports HTS.SAS.BusinessObjects
Imports System.Data
Imports System.Collections.Generic
Partial Class addSpnRecpts
    Inherits System.Web.UI.Page
    ''Private LogErrors As LogError
    ''' <summary>
    ''' Method to Load Sponsors in Grid
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub LoadGrid()
        Dim ds As New DataSet
        Dim eobj As New SponsorEn
        Dim eobjlist As New List(Of SponsorEn)
        Dim bobj As New AccountsBAL
        Dim cat As String
        Dim lblmsg As String = ""
        Dim j As Integer = 0

        Dim totalavailable As Double = 0
        Dim bospn As New AccountsBAL
        If Not eobjlist Is Nothing Then
            eobj.Name = txtSponsorName.Text
            eobj.SponserCode = txtSponsorCode.Text
            'modified by Hafiz @ 22/2/2016
            'commented - start
            'eobj.TransStatus = "Open" 
            'commented - end
            eobj.PostStatus = "Posted"
            eobj.Category = "Receipt"
            If Session("ReceiptFrom") Is Nothing Then
                cat = Request.QueryString("Cat")
            Else

                cat = Session("ReceiptFrom")
            End If
            If cat = "SA" Then
                Try
                    eobjlist = bobj.GetReciptSpAll(eobj)

                Catch ex As Exception
                    LogError.Log("addSpnRecpts", "LoadGrid", ex.Message)
                End Try

                'modified by Hafiz @ 22/2/2016
                'change allocated amount column to available amount & hide account balance - START
                dgView.Columns(8).HeaderText = "Available Amount"
                dgView.Columns(9).Visible = False
                'change allocated amount column to available amount & hide account balance - END

                dgView.DataSource = eobjlist
                dgView.DataBind()
                While j < eobjlist.Count

                    For Each dgItem1 In dgView.Items
                        If dgItem1.Cells(1).Text = eobjlist(j).TempTransCode Then
                            Dim espn1 As Double = 0
                            Dim available As Double = 0
                            Dim espn2 As New AccountsEn

                            espn2.TransactionCode = eobjlist(j).TransactionCode
                            espn2.PostStatus = "Posted"
                            espn2.TransStatus = "Closed"
                            espn2.Category = "Allocation"
                            espn2.SubType = "Sponsor"
                            Try
                                espn1 = bospn.GetTotalAllocatedAmount(espn2)
                            Catch ex As Exception
                                LogError.Log("SponsorAllocation", "addSpnCode", ex.Message)
                            End Try
                            available = eobjlist(j).AllocatedAmount
                            totalavailable = espn1 + available
                            dgItem1.Cells(6).Text = String.Format("{0:F}", totalavailable)
                            Exit For
                        End If
                    Next
                    j = j + 1

                End While

            ElseIf cat = "SP" Then
                Try
                    eobjlist = bobj.GetReceiptSpAll(eobj)

                Catch ex As Exception
                    LogError.Log("addSpnRecpts", "LoadGrid", ex.Message)
                End Try
                dgView.Columns(8).Visible = False
                dgView.DataSource = eobjlist
                dgView.DataBind()
            End If
            While j < eobjlist.Count

                For Each dgItem1 In dgView.Items
                    If dgItem1.Cells(1).Text = eobjlist(j).TempTransCode Then
                        Dim espn1 As Double = 0
                        Dim available As Double = 0
                        Dim espn2 As New AccountsEn

                        espn2.TransactionCode = eobjlist(j).TransactionCode
                        espn2.PostStatus = "Posted"
                        espn2.TransStatus = "Closed"
                        espn2.Category = "Allocation"
                        espn2.SubType = "Sponsor"
                        Try
                            espn1 = bospn.GetTotalAllocatedAmount(espn2)
                        Catch ex As Exception
                            LogError.Log("SponsorAllocation", "addSpnCode", ex.Message)
                        End Try
                        available = eobjlist(j).TempAmount
                        totalavailable = espn1 + available
                        dgItem1.Cells(6).Text = String.Format("{0:F}", totalavailable)
                        Exit For
                    End If
                Next
                j = j + 1

            End While

            
        Else
            Response.Write("No Fee types are Available")
        End If
    End Sub

    Private Sub UpdateGrid()
        Dim cScript As String = ""
        cScript = "<SCRIPT LANGUAGE=""JavaScript"">window.opener.document.getElementById('ctl00_ContentPlaceHolder1_btnHidden').click();self.close();</SCRIPT>"
        ClientScript.RegisterStartupScript(Me.GetType(), "ClosePopUp", cScript)
    End Sub
    Protected Sub ibtnOK_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ibtnOK.Click
        Dim cat As String

        If Session("ReceiptFrom") Is Nothing Then
            cat = Request.QueryString("Cat")
        Else

            cat = Session("ReceiptFrom")
        End If

        If dgView.SelectedIndex <> -1 Then
            If cat = "SP" Then
                Dim eob As New SponsorEn
                eob.TransactionCode = dgView.DataKeys(dgView.SelectedIndex)
                'added by Hafiz @ 17/2/2016
                eob.BatchCode = dgView.DataKeys(dgView.SelectedIndex)
                eob.TempAmount = dgView.Items(dgView.SelectedIndex).Cells(9).Text
                'eobj.TempTransCode = dgView.Items(dgView.SelectedIndex).Cells(1).Text
                eob.TempTransCode = dgView.Items(dgView.SelectedIndex).Cells(1).Text()
                eob.CreditRef = dgView.Items(dgView.SelectedIndex).Cells(2).Text
                eob.Name = dgView.Items(dgView.SelectedIndex).Cells(3).Text
                eob.TransactionAmount = dgView.Items(dgView.SelectedIndex).Cells(5).Text
                eob.PaidAmount = dgView.Items(dgView.SelectedIndex).Cells(6).Text
                eob.Category = dgView.Items(dgView.SelectedIndex).Cells(7).Text
                eob.AllocatedAmount = dgView.Items(dgView.SelectedIndex).Cells(8).Text
                dgView.SelectedIndex = -1
                Session("spnobj") = eob

            ElseIf cat = "SA" Then

                Dim eobj1 As New AccountsDetailsEn
                eobj1.TransactionCode = dgView.DataKeys(dgView.SelectedIndex)
                'eobj.TempTransCode = dgView.Items(dgView.SelectedIndex).Cells(1).Text
                eobj1.TransTempCode = dgView.Items(dgView.SelectedIndex).Cells(1).Text()
                eobj1.CreditRef = dgView.Items(dgView.SelectedIndex).Cells(2).Text
                eobj1.Description = dgView.Items(dgView.SelectedIndex).Cells(3).Text
                eobj1.TransactionAmount = dgView.Items(dgView.SelectedIndex).Cells(5).Text
                eobj1.PaidAmount = dgView.Items(dgView.SelectedIndex).Cells(6).Text
                eobj1.ReferenceOne = dgView.Items(dgView.SelectedIndex).Cells(7).Text
                eobj1.TempAmount = dgView.Items(dgView.SelectedIndex).Cells(8).Text
                dgView.SelectedIndex = -1
                Session("spnobj") = eobj1
            End If
        End If
        UpdateGrid()

    End Sub

    Protected Sub ibtnClose_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ibtnClose.Click
        UpdateGrid()
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        LoadGrid()
    End Sub

    Protected Sub ibtnLoad_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ibtnLoad.Click
        LoadGrid()
    End Sub
    Protected Sub txtAllAmount1_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs)

        Dim txtamt As TextBox
        Dim amount As Double = 0
        Dim dgitem As DataGridItem
        Dim i As Integer = 0
        For Each dgitem In dgView.Items
            txtamt = dgitem.FindControl("txtAllAmount1")

            amount = txtamt.Text

            txtamt.Text = String.Format("{0:F}", amount)
        Next
        LoadTotals()
    End Sub

    Protected Sub txtpamont_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs)

        Dim txtamt As TextBox
        Dim amount As Double = 0
        Dim dgitem As DataGridItem
        Dim i As Integer = 0
        For Each dgitem In dgView.Items
            txtamt = dgitem.FindControl("txtpamont")

            amount = MaxGeneric.clsGeneric.NullToDecimal(txtamt.Text)

            txtamt.Text = String.Format("{0:F}", amount)
        Next
        LoadTotals()
    End Sub
    Protected Sub Chk_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)

        Dim chk As CheckBox
        Dim dgItem1 As DataGridItem
        For Each dgItem1 In dgView.Items
            chk = dgItem1.Cells(0).Controls(1)
            If chk.Checked = True Then
                LoadTotals()
            Else

                LoadTotals()
            End If
        Next

    End Sub
    Private Sub LoadTotals()

        'varaible declaration
        Dim ActSpAmount As Double = 0.0, SpStuAllAmount As Double = 0.0, RspStuAllAmount As Double = 0.0, StAllAmount = 0.0, AvalAllAmount As Double = 0.0

        Dim chk As CheckBox
        Dim txtAmount As TextBox
        Dim txtPocket As TextBox
        Dim dgItem1 As DataGridItem
        Dim totalAmt1 As Double = 0
        Dim BalAmt As Double = 0

        For Each dgItem1 In dgView.Items
            Dim totalAmt As Double = 0
            chk = dgItem1.Cells(0).Controls(1)
            If chk.Checked = True Then
                Dim AllAmt As Double = 0
                Dim Allpck As Double = 0

                txtAmount = dgItem1.Cells(7).Controls(1)
                If txtAmount.Text <> "" Then
                    AllAmt = CDbl(txtAmount.Text)
                End If
                txtPocket = dgItem1.Cells(9).Controls(1)
                If txtPocket.Text <> "" Then
                    Allpck = CDbl(txtPocket.Text)
                End If
                totalAmt = AllAmt + Allpck
                totalAmt1 += totalAmt
            End If
        Next



    End Sub
    Protected Sub dgView_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs)

    End Sub
End Class
