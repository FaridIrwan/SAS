Imports HTS.SAS.Entities
Imports HTS.SAS.BusinessObjects
Imports System.Data
Imports System.Collections.Generic
Partial Class RptSponsorAgeing
    Inherits System.Web.UI.Page

    Private Sub Menuname(ByVal MenuId As Integer)
        Dim eobj As New MenuEn
        Dim bobj As New MenuBAL
        eobj.MenuId = MenuId
        eobj = bobj.GetMenus(eobj)
        lblMenuName.Text = eobj.MenuName
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack() Then
            Menuname(CInt(Request.QueryString("Menuid")))
            'ibtnPrint.Attributes.Add("onclick", "new_window=window.open('RptSpAgeingCRForm.aspx','Hanodale','width=700,height=500,resizable=1');new_window.focus();")

            'ibtnView.Attributes.Add("onclick", "new_window=window.open('RptSpAgeingCRForm.aspx','Hanodale','width=700,height=500,resizable=1');new_window.focus();")
            ibtnFDate.Attributes.Add("onClick", "return getibtnFDate()")
            ibtnTodate.Attributes.Add("onClick", "return getDateto()")

            'Modified by Hafiz @ 09/3/2016

            'Uncommented By Zoya @ 26/02/2016
            ibtnPrint.Attributes.Add("onclick", "return getDate()")

            'Added By Zoya @ 26/02/2016
            'ibtnPrint.Attributes.Add("onclick", "new_window=window.open('../GroupReport/RptSponsorAgeingViewer.aspx','Hanodale','width=520,height=400,resize=0,scrollbars=1');new_window.focus();")

            'Commented By Zoya @26/02/2016
            'ibtnBMReport.Attributes.Add("onclick", "return getDate()")
            'ibtnBMReport.Attributes.Add("onclick", "new_window=window.open('../GroupReport/RptSponsorAgeingViewer.aspx','Hanodale','width=520,height=400,resize=0,scrollbars=1');new_window.focus();")
            'ibtnEnReport.Attributes.Add("onclick", "return getDate()")
            'ibtnEnReport.Attributes.Add("onclick", "new_window=window.open('../GroupReport/RptSponsorAgeingViewerEn.aspx','Hanodale','width=520,height=400,resize=0,scrollbars=1');new_window.focus();")


            ibtnView.Attributes.Add("onclick", "return dllValues()")
            txtFrom.Attributes.Add("OnKeyup", "return CheckFromDate()")
            txtTodate.Attributes.Add("OnKeyup", "return CheckToDate()")
            'txtperiod.Attributes.Add("OnKeyup", "return number()")
            LoadUserRights()
            Sponsor()
            dates()

        End If
    End Sub

    Private Sub dates()
        txtFrom.Text = Format(Date.Now, "dd/MM/yyyy")
        txtTodate.Text = Format(Date.Now, "dd/MM/yyyy")
    End Sub
    Private Sub Sponsor()
        Dim eobjSponsorEn As New SponsorEn
        Dim objSponsorBs As New SponsorBAL
        Dim listSponsor As New List(Of SponsorEn)
        listSponsor = objSponsorBs.GetList(eobjSponsorEn)
        ddlSponsor.Items.Clear()
        ddlSponsor.AppendDataBoundItems = True
        ddlSponsor.Items.Add(New ListItem("-- Select --", "-1"))
        ddlSponsor.DataSource = listSponsor
        ddlSponsor.DataTextField = "Name"
        ddlSponsor.DataValueField = "SponserCode"
        ddlSponsor.DataBind()
    End Sub

    Private Sub LoadUserRights()
        Dim obj As New UsersBAL
        Dim eobj As UserRightsEn

        'eobj = obj.GetUserRights(5, 1)
        eobj = obj.GetUserRights(CInt(Request.QueryString("Menuid")), CInt(Session("UserGroup")))
        'Rights for Add
        If eobj.IsAdd = True Then
            ibtnSave.Enabled = True
            'OnAdd()
        Else
            ibtnNew.Enabled = False
            ibtnNew.ImageUrl = "../images/gAdd.png"
            ibtnNew.ToolTip = "Access Denied"
            ibtnDelete.Enabled = False
            ibtnDelete.ImageUrl = "../images/gdelete.png"
            ibtnDelete.ToolTip = "Access Denied"
            '-----------------------------------------------
            ibtnFirst.Enabled = False
            ibtnLast.Enabled = False
            ibtnPrevs.Enabled = False
            ibtnNext.Enabled = False
            ibtnFirst.ToolTip = "Access Denied"
            ibtnLast.ToolTip = "Access Denied"
            ibtnPrevs.ToolTip = "Access Denied"
            ibtnNext.ToolTip = "Access Denied"
            ibtnFirst.ImageUrl = "../images/gnew_first.png"
            ibtnLast.ImageUrl = "../images/gnew_last.png"
            ibtnPrevs.ImageUrl = "../images/gnew_Prev.png"
            ibtnNext.ImageUrl = "../images/gnew_next.png"
            '------------------------------------------------
            ibtnSave.Enabled = False
            ibtnSave.ImageUrl = "../images/gsave.png"
            ibtnSave.ToolTip = "Access Denied"
        End If
        'Rights for Edit
        If eobj.IsEdit = True Then
            'ibtnSave.ToolTip = "Access Denied"
            Session("EditFlag") = True
        Else
            Session("EditFlag") = False
        End If
        'Rights for View
        ibtnView.Enabled = eobj.IsView
        If eobj.IsView = True Then
            ibtnView.ImageUrl = "../images/find.png"
            ibtnView.Enabled = True
        Else
            ibtnView.ImageUrl = "../images/gfind.png"
            ibtnView.ToolTip = "Access Denied"
        End If

        'Commented and uncommented by Zoya @ 26/02/2016
        'Rights for Print
        ibtnPrint.Enabled = eobj.IsPrint
        If eobj.IsPrint = True Then
            'EnablePrintUserRights()
            ibtnPrint.Enabled = True
            ibtnPrint.ImageUrl = "../images/print.png"
            ibtnPrint.ToolTip = "Print"
        Else
            'DisablePrintUserRights()
            ibtnPrint.Enabled = False
            ibtnPrint.ImageUrl = "../images/gprint.png"
            ibtnPrint.ToolTip = "Access Denied"
        End If
        'End Commented and uncommented by Zoya @ 26/02/2016

        'Checking Default mode
        If eobj.IsAddModeDefault = True Then
            ' pnlView.Visible = False
            'pnlAdd.Visible = True
        Else
            'pnlAdd.Visible = False
            ' pnlView.Visible = True
            'LoadGrid()
        End If
        If eobj.IsOthers = True Then
            ibtnOthers.Enabled = True
            ibtnOthers.ImageUrl = "../images/others.png"
            ibtnOthers.ToolTip = "Others"
        Else
            ibtnOthers.Enabled = False
            ibtnOthers.ImageUrl = "../images/gothers.png"
            ibtnOthers.ToolTip = "Access Denied"
        End If
        If eobj.IsPost = True Then
            ibtnPosting.Enabled = True
            ibtnPosting.ImageUrl = "../images/posting.png"
            ibtnPosting.ToolTip = "Posting"
        Else
            ibtnPosting.Enabled = False
            ibtnPosting.ImageUrl = "../images/gposting.png"
            ibtnPosting.ToolTip = "Access Denied"
        End If
    End Sub



    Protected Sub ibtnNew_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        ddlSponsor.SelectedValue = -1
        ChkBoxDate.Checked = True
        CheckBoxMonth.Checked = False
        CheckBoxQuarter.Checked = False
        CheckBoxYear.Checked = False
        txtFrom.ReadOnly = False
        txtTodate.ReadOnly = False
        ibtnFDate.Visible = True
        ibtnTodate.Visible = True
        dates()

        Session("sponsor") = Nothing
    End Sub

    Protected Sub ibtnCancel_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        ddlSponsor.SelectedValue = -1
        ChkBoxDate.Checked = True
        CheckBoxMonth.Checked = False
        CheckBoxQuarter.Checked = False
        CheckBoxYear.Checked = False
        txtFrom.ReadOnly = False
        txtTodate.ReadOnly = False
        ibtnFDate.Visible = True
        ibtnTodate.Visible = True
        dates()

        Session("sponsor") = Nothing
    End Sub

    Protected Sub ibtnView_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ibtnView.Click
    End Sub

    Protected Sub ddlSponsor_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim sponsor As String
        sponsor = ddlSponsor.SelectedValue
        If sponsor = "-1" Then
            sponsor = "%"
            Session("sponsor") = sponsor
        Else
            Session("sponsor") = sponsor
        End If
    End Sub

    Protected Sub ChkBoxDate_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ChkBoxDate.CheckedChanged
        If ChkBoxDate.Checked = True Then
            CheckBoxMonth.Checked = False
            CheckBoxQuarter.Checked = False
            CheckBoxYear.Checked = False
            txtFrom.ReadOnly = False
            txtTodate.ReadOnly = False
            ibtnFDate.Visible = True
            ibtnTodate.Visible = True
        Else
            txtFrom.ReadOnly = True
            txtTodate.ReadOnly = True
            ibtnFDate.Visible = False
            ibtnTodate.Visible = False
            dates()
        End If
    End Sub

    Protected Sub CheckBoxMonth_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles CheckBoxMonth.CheckedChanged
        If CheckBoxMonth.Checked = True Then
            ChkBoxDate.Checked = False
            CheckBoxQuarter.Checked = False
            CheckBoxYear.Checked = False
        End If
    End Sub

    Protected Sub CheckBoxQuarter_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles CheckBoxQuarter.CheckedChanged
        If CheckBoxQuarter.Checked = True Then
            ChkBoxDate.Checked = False
            CheckBoxMonth.Checked = False
            CheckBoxYear.Checked = False
        End If
    End Sub

    Protected Sub CheckBoxYear_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles CheckBoxYear.CheckedChanged
        If CheckBoxYear.Checked = True Then
            ChkBoxDate.Checked = False
            CheckBoxMonth.Checked = False
            CheckBoxQuarter.Checked = False
        End If
    End Sub

End Class
