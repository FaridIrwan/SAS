Imports HTS.SAS.Entities
Imports HTS.SAS.BusinessObjects
Imports System.Data
Imports System.Collections.Generic
Imports System.IO
Imports System.Xml

Partial Class ErrorLog
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack() Then
            Session("Menuid") = Request.QueryString("Menuid")
            'Loading User Rights
            LoadUserRights()
            'load PageName
            Menuname(CInt(Request.QueryString("Menuid")))
            dates()
            ibtnDate.Attributes.Add("onClick", "return getDate()")
            txtDate.Attributes.Add("OnKeyup", "return CheckDate()")
        End If
    End Sub
    ''' <summary>
    ''' Method to Change the Date Format
    ''' </summary>
    ''' <remarks>Date in ddd/mm/yyyy Format</remarks>
    Private Sub dates()
        'Dim GBFormat As System.Globalization.CultureInfo
        'GBFormat = New System.Globalization.CultureInfo("en-GB")
        txtDate.Text = Format(Date.Now, "dd/MM/yyyy")
    End Sub
    ''' <summary>
    ''' Method to Load UserRights
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub LoadUserRights()
        Dim obj As New UsersBAL
        Dim eobj As New UserRightsEn
        Try
            eobj = obj.GetUserRights(CInt(Request.QueryString("Menuid")), CInt(Session("UserGroup")))

        Catch ex As Exception
            LogError.Log("ErrorLog", "LoadUserRights", ex.Message)
        End Try
        'Rights for Add

        If eobj.IsAdd = True Then
            'ibtnSave.Enabled = True
            'OnAdd()
            ibtnNew.ImageUrl = "images/add.png"
            ibtnNew.Enabled = True
        Else
            ibtnNew.ImageUrl = "images/gadd.png"
            ibtnNew.Enabled = False
            ibtnNew.ToolTip = "Access Denied"

        End If
        'Rights for Edit
        If eobj.IsEdit = True Then
            ibtnSave.Enabled = True
            ibtnSave.ImageUrl = "images/save.png"
            ibtnSave.ToolTip = "Edit"
            If eobj.IsAdd = False Then
                ibtnSave.Enabled = False
                ibtnSave.ImageUrl = "images/gsave.png"
                ibtnSave.ToolTip = "Access Denied"
            End If

            Session("EditFlag") = True

        Else
            Session("EditFlag") = False
            ibtnSave.Enabled = False
            ibtnSave.ImageUrl = "images/gsave.png"
        End If
        'Rights for View
        ibtnView.Enabled = eobj.IsView
        If eobj.IsView = True Then
            ibtnView.ImageUrl = "images/find.png"
            ibtnView.Enabled = True
        Else
            ibtnView.ImageUrl = "images/gfind.png"
            ibtnView.ToolTip = "Access Denied"
        End If
        'Rights for Delete
        If eobj.IsDelete = True Then
            ibtnDelete.ImageUrl = "images/delete.png"
            ibtnDelete.Enabled = True
        Else
            ibtnDelete.ImageUrl = "images/gdelete.png"
            ibtnDelete.ToolTip = "Access Denied"
            ibtnDelete.Enabled = False
        End If
        'Rights for Print
        ibtnPrint.Enabled = eobj.IsPrint
        If eobj.IsPrint = True Then
            ibtnPrint.Enabled = True
            ibtnPrint.ImageUrl = "images/print.png"
            ibtnPrint.ToolTip = "Print"
        Else
            ibtnPrint.Enabled = False
            ibtnPrint.ImageUrl = "images/gprint.png"
            ibtnPrint.ToolTip = "Access Denied"
        End If
        If eobj.IsOthers = True Then
            ibtnOthers.Enabled = True
            ibtnOthers.ImageUrl = "images/others.png"
            ibtnOthers.ToolTip = "Others"
        Else
            ibtnOthers.Enabled = False
            ibtnOthers.ImageUrl = "images/gothers.png"
            ibtnOthers.ToolTip = "Access Denied"
        End If
        If eobj.IsPost = True Then
            ibtnPosting.Enabled = True
            ibtnPosting.ImageUrl = "images/posting.png"
            ibtnPosting.ToolTip = "Posting"
        Else
            ibtnPosting.Enabled = False
            ibtnPosting.ImageUrl = "images/gposting.png"
            ibtnPosting.ToolTip = "Access Denied"
        End If
        ibtnPrevs.Enabled = False
        ibtnPrevs.ImageUrl = "images/gnew_Prev.png"
        ibtnFirst.Enabled = False
        ibtnFirst.ImageUrl = "images/gnew_first.png"
        ibtnNext.Enabled = False
        ibtnNext.ImageUrl = "images/gnew_next.png"
        ibtnLast.Enabled = False
        ibtnLast.ImageUrl = "images/gnew_last.png"
    End Sub
    ''' <summary>
    ''' Method to get the MenuName
    ''' </summary>
    ''' <param name="MenuId">Parameter is MenuId</param>
    ''' <remarks></remarks>
    Private Sub Menuname(ByVal MenuId As Integer)
        Dim eobj As New MenuEn
        Dim bobj As New MenuBAL
        eobj.MenuId = MenuId
        Try
            eobj = bobj.GetMenus(eobj)
        Catch ex As Exception
            LogError.Log("ErrorLog", "Menuname", ex.Message)
        End Try
        lblMenuName.Text = eobj.MenuName
    End Sub


    Protected Sub Search_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        lblMsg.Text = ""
        LoadGrid()
    End Sub
    ''' <summary>
    ''' Method to Load the Grid
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub LoadGrid()
        Dim lolist As New List(Of ErrorLogEn)
        Dim loerror As ErrorLogEn
        Dim lsDelimeter As String = ","
        Dim strDate As String = Trim(txtDate.Text)
        strDate = strDate.Substring(0, 2) & "-" & strDate.Substring(3, 2) & "-" & strDate.Substring(6, 4)
        Dim path As String = Server.MapPath("LOGFILE_PATH") & "\" & strDate & ".xml"

        Try

            Dim _doc As New XmlDocument
            _doc.Load(path)

            Dim Snames As XmlNodeList = _doc.GetElementsByTagName("ScreenName")
            Dim Mnames As XmlNodeList = _doc.GetElementsByTagName("MethodName")
            Dim Emessage As XmlNodeList = _doc.GetElementsByTagName("ErrorMessage")
            Dim Datet As XmlNodeList = _doc.GetElementsByTagName("DateTime")
            Dim i As Integer = 0
            While i < Snames.Count
                loerror = New ErrorLogEn
                loerror.ScreenName = Snames(i).InnerText
                loerror.MethodName = Mnames(i).InnerText
                loerror.ErrorMessage = Emessage(i).InnerText
                loerror.DateTime = DateTime.Parse(Datet(i).InnerText.ToString())
                lolist.Add(loerror)
                i = i + 1
            End While
            dgInvoices.DataSource = lolist
            dgInvoices.DataBind()
        Catch ex As Exception
            dgInvoices.DataSource = Nothing
            dgInvoices.DataBind()
            lblMsg.Text = "No ErrorLog Present"
            LogError.Log("ErrorLog", "LoadGrid", ex.Message)
        End Try

    End Sub

    Protected Sub ibtnCancel_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        oncleatData()
    End Sub
    ''' <summary>
    ''' Method to Clear the Field Values
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub oncleatData()
        dates()
        dgInvoices.DataSource = Nothing
        dgInvoices.DataBind()
        lblMsg.Text = ""
    End Sub
End Class
