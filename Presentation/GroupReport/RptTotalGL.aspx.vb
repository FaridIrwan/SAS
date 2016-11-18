Imports HTS.SAS.BusinessObjects
Imports HTS.SAS.Entities
Imports System.Data
Imports System.Collections.Generic
Imports System.Data.SqlClient
Imports System.Web.Configuration

Partial Class RptTotalGL
    Inherits System.Web.UI.Page

    Dim strFromDate As String = ""
    Dim strTodate As String = ""

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
            LoadUserRights()
            ibtnFDate.Attributes.Add("onClick", "return getibtnFDate()")
            ibtnTodate.Attributes.Add("onClick", "return getDateto()")

            'modified by Hafiz @ 02/3/2016

            'UnCommented By Zoya @26/02/2016
            ibtnPrint.Attributes.Add("onclick", "return getDate()")

            'Added By Zoya @26/02/2016
            'ibtnPrint.Attributes.Add("onclick", "new_window=window.open('../GroupReport/RptTotalGLViewer.aspx','Hanodale','width=520,height=400,resize=0,scrollbars=1');new_window.focus();")

            'Commented By Zoya @26/02/2016
            'ibtnBMReport.Attributes.Add("onclick", "return getDate()")
            'ibtnBMReport.Attributes.Add("onclick", "new_window=window.open('../GroupReport/RptTotalGLViewer.aspx','Hanodale','width=520,height=400,resize=0,scrollbars=1');new_window.focus();")
            'ibtnEnReport.Attributes.Add("onclick", "return getDate()")
            'ibtnEnReport.Attributes.Add("onclick", "new_window=window.open('../GroupReport/RptTotalGLViewerEn.aspx','Hanodale','width=520,height=400,resize=0,scrollbars=1');new_window.focus();")

            ibtnView.Attributes.Add("onclick", "return getDate()")
            txtFrom.Attributes.Add("OnKeyup", "return CheckFromDate()")
            txtTodate.Attributes.Add("OnKeyup", "return CheckToDate()")
            dates()
            Session("sortby") = Nothing
            Session("fromdate") = Nothing
            Session("todate") = Nothing
        End If

        'ibtnPrint.Attributes.Add("onclick", "new_window=window.open('RptTotalGLViewer.aspx?fdate=" & strFromDate & "&tdate=" & strTodate & "','Hanodale','width=520,height=400,resizable=0');new_window.focus();")
        'ibtnView.Attributes.Add("onclick", "new_window=window.open('RptTotalGLViewer.aspx?fdate=" & strFromDate & "&tdate=" & strTodate & "','Hanodale','width=520,height=400,resizable=0');new_window.focus();")

    End Sub
    Private Sub dates()
        txtFrom.Text = Format(Date.Now, "dd/MM/yyyy")
        txtTodate.Text = Format(Date.Now, "dd/MM/yyyy")
    End Sub
    Private Sub LoadUserRights()
        Dim obj As New UsersBAL
        Dim eobj As UserRightsEn

        'eobj = obj.GetUserRights(5, 1)
        eobj = obj.GetUserRights(CInt(Request.QueryString("Menuid")), CInt(Session("UserGroup")))
        'Rights for Add
        If eobj.IsAdd = True Then
            ibtnSave.Enabled = False
            ibtnSave.ImageUrl = "../images/gsave.png"
            ibtnSave.ToolTip = "Access Denied"
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

        'Commenetd and uncommenetd by Zoya @ 26/02/2016
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
        'End Commenetd and uncommenetd by Zoya @ 26/02/2016

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

    'modified by Hafiz @ 03/3/2016
    Protected Sub ibtnCancel_Click1(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        dates()
    End Sub

    'Protected Sub ibtnPrint_Click(sender As Object, e As ImageClickEventArgs) Handles ibtnPrint.Click
    '    strFromDate = txtFrom.Text
    '    strTodate = txtTodate.Text
    'End Sub

    'Protected Sub ibtnView_Click(sender As Object, e As ImageClickEventArgs) Handles ibtnView.Click
    '    strFromDate = txtFrom.Text
    '    strTodate = txtTodate.Text
    'End Sub

    'Commented by Zoya @ 26/02/2016

    'Protected Sub EnablePrintUserRights()
    '    ibtnBMReport.Enabled = True
    '    ibtnEnReport.Enabled = True
    '    'ibtnPrint.ImageUrl = "../images/print.png"
    '    ibtnBMReport.ImageUrl = ""
    '    ibtnEnReport.ImageUrl = ""
    '    ibtnBMReport.ToolTip = "Print"
    '    ibtnEnReport.ToolTip = "Print"
    '    'ibtnPrint.ToolTip = "Print"
    'End Sub

    'Protected Sub DisablePrintUserRights()
    '    ibtnBMReport.Enabled = False
    '    ibtnEnReport.Enabled = False
    '    'ibtnPrint.ImageUrl = "../images/print.png"
    '    ibtnBMReport.ImageUrl = ""
    '    ibtnEnReport.ImageUrl = ""
    '    ibtnBMReport.ToolTip = "Access Denied"
    '    ibtnEnReport.ToolTip = "Access Denied"
    '    'ibtnPrint.ToolTip = "Access Denied"
    'End Sub

#Region "ibtnNew Click"

    'added by Hafiz @ 01/3/2016
    'Button new function

    Protected Sub ibtnNew_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ibtnNew.Click
        dates()
    End Sub

#End Region

End Class
