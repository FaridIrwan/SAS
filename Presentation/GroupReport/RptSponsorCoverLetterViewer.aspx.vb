Imports System.Collections.Generic
Imports HTS.SAS.Entities
Imports HTS.SAS.BusinessObjects
Imports HTS.SAS.DataAccessObjects
Imports System.Data
Imports System.Data.SqlClient
Imports System.Web.Configuration
Imports CrystalDecisions.CrystalReports.Engine
Imports CrystalDecisions.Shared
Imports System.Linq
Imports System.Xml.Serialization
Imports System.IO
Imports System.Reflection
Imports System.Reflection.Emit

Partial Class GroupReport_RptSponsorCoverLetterViewer
    Inherits System.Web.UI.Page
    Private MyReportDocument As New ReportDocument
    Private _ReportHelper As New ReportHelper

    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        Try

            If Not IsPostBack Then
                If Session("batchcode") Is Nothing Then
                    Response.Write("Load Report Failed. No Batchcode Selected")
                    Return
                End If
                Dim str As String = Nothing

                str = "select si.batchcode, stu.sasi_matricno, stu.sasi_name, stu.sasi_cursemyr, stu.sasi_icno, prog.sapg_programbm, ft.saft_code, ft.saft_desc, ft.saft_feetype," +
                " ft.saft_taxmode, sid.transamount, sid.taxamount, sid.tax, ft.SAFT_Hostel, ft.SAFT_Priority, ft.SAFT_Remarks, ft.SAFT_Status, sid.Transid," +
                " sid.RefCode, sid.TransTempCode, sid.TransCode, sid.internal_use, si.creditref1, stu.sasc_code" +
                    " from sas_sponsorInvoice si" +
                    " inner join sas_sponsorInvoiceDetails sid on si.transcode = sid.transcode and si.transtempcode = sid.transtempcode" +
                    " inner join sas_feetypes ft on ft.saft_code = sid.refcode" +
                    " inner join sas_student stu on stu.sasi_matricno = si.creditref" +
                    " left join sas_program prog on prog.sapg_code = stu.sasi_pgid"
                If Not Session("batchcode") Is Nothing Then
                    str += " where si.batchcode = '" + Session("batchcode").ToString() + "'"
                End If

                'DataSet Strating
                Dim _DataSet As DataSet = _ReportHelper.GetDataSet(str)
                _DataSet.Tables(0).TableName = "StudentEn"
                'Report XML Loading

                'Dim SponCoverLetterSQL As String = Nothing
                'SponCoverLetterSQL = "select * from sas_sponsorcoverletter where sascl_code = '" + Session("SCLCode").ToString() + "'"

                'Dim _SponCoverLetterDataSet As DataSet = _ReportHelper.GetDataSet(SponCoverLetterSQL)
                '_SponCoverLetterDataSet.Tables(0).TableName = "SponsorCoverLetter"

                Dim bobj As New SponsorCoverLetterBAL
                Dim eobj As New SponsorCoverLetterEn
                Dim ListObjects As New List(Of SponsorCoverLetterEn)
                eobj.Code = String.Empty
                eobj.Title = String.Empty
                If Not Session("SCLCode") Is Nothing Then
                    eobj = Session("SCLCode")
                    ListObjects.Add(eobj)
                Else
                    ListObjects = bobj.GetList(eobj)
                End If

                Dim objSpon As New SponsorEn
                If Not Session("SCLSponsor") Is Nothing Then
                    objSpon = Session("SCLSponsor")
                End If

                Dim _SponCoverLetterDataSet As DataSet = GetDataSetSponsorCoverLetter(ListObjects)
                Dim _Sponsor As DataSet = GetDataSetSponsor(objSpon)

                _DataSet.Tables.Add(_SponCoverLetterDataSet.Tables(0).Copy())
                _DataSet.Tables.Add(_Sponsor.Tables(0).Copy())
                Dim s As String = Server.MapPath("~/xml/SponsorCoverLetter.xml")


                _DataSet.WriteXml(s)

                'Report XML Ended

                'Records Checking

                If _DataSet.Tables(0).Rows.Count = 0 Then
                    Response.Write("No Record Found")

                Else
                    ''Default will take BM version -  0 = BM, 1 = EN
                    Dim sclReportBatch As String = "~/GroupReport/RptSponsorCoverLetterBatch.rpt"
                    If Not Session("SCLLang") Is Nothing Then
                        If Session("SCLLang").ToString() = "1" Then
                            sclReportBatch = "~/GroupReport/RptSponsorCoverLetterBatch_EN.rpt"
                        Else
                            sclReportBatch = "~/GroupReport/RptSponsorCoverLetterBatch.rpt"
                        End If
                    End If
                    ''Default will take BM version
                    Dim sclReportIndividual As String = "~/GroupReport/RptSponsorCoverLetter.rpt"
                    If Not Session("SCLLang") Is Nothing Then
                        If Session("SCLLang").ToString() = "1" Then
                            sclReportIndividual = "~/GroupReport/RptSponsorCoverLetter_EN.rpt"
                        Else
                            sclReportIndividual = "~/GroupReport/RptSponsorCoverLetter.rpt"
                        End If
                    End If

                    'Report Loading 
                    ''Default will take Batch Type - 0 = Batch, 1 =Individual
                    If Not Session("SCLType") Is Nothing Then
                        If Session("SCLType").ToString() = "1" Then
                            MyReportDocument.Load(Server.MapPath(sclReportIndividual))
                        Else
                            MyReportDocument.Load(Server.MapPath(sclReportBatch))
                        End If
                    Else
                        MyReportDocument.Load(Server.MapPath(sclReportBatch))
                    End If

                    MyReportDocument.SetDataSource(_DataSet)
                    Session("reportobject") = MyReportDocument
                    CrystalReportViewer1.ReportSource = MyReportDocument
                    CrystalReportViewer1.DataBind()
                    MyReportDocument.Refresh()

                    'Report Ended
                End If
            Else

                'Report Loading

                MyReportDocument = Session("reportobject")
                CrystalReportViewer1.ReportSource = MyReportDocument
                CrystalReportViewer1.DataBind()
                MyReportDocument.Refresh()

                'Report Ended

            End If

        Catch ex As Exception

            Response.Write(ex.Message)

        End Try
    End Sub

    ''http://www.codeproject.com/Articles/16252/Generic-List-Of-to-DataSet-Three-Approaches
    Private Function GetDataSetSponsorCoverLetter(ByVal list As  _
          List(Of SponsorCoverLetterEn)) As DataSet
        Dim _result As New DataSet()
        _result.Tables.Add("SponsorCoverLetter")
        _result.Tables("SponsorCoverLetter").Columns.Add("sascl_code")
        _result.Tables("SponsorCoverLetter").Columns.Add("sascl_title")
        _result.Tables("SponsorCoverLetter").Columns.Add("sascl_ourref")
        _result.Tables("SponsorCoverLetter").Columns.Add("sascl_yourref")
        _result.Tables("SponsorCoverLetter").Columns.Add("sascl_address")
        _result.Tables("SponsorCoverLetter").Columns.Add("sascl_message")
        _result.Tables("SponsorCoverLetter").Columns.Add("sascl_signby")
        _result.Tables("SponsorCoverLetter").Columns.Add("sascl_name")
        _result.Tables("SponsorCoverLetter").Columns.Add("sascl_frdate")
        _result.Tables("SponsorCoverLetter").Columns.Add("sascl_todate")
        _result.Tables("SponsorCoverLetter").Columns.Add("sascl_updatedby")
        _result.Tables("SponsorCoverLetter").Columns.Add("sascl_updatedtime")

        For Each item As SponsorCoverLetterEn In list
            Dim newRow As DataRow = _
                _result.Tables("SponsorCoverLetter").NewRow()
            newRow("sascl_code") = item.Code
            newRow("sascl_address") = item.Address
            newRow("sascl_frdate") = item.FromDate
            newRow("sascl_todate") = item.ToDate
            newRow("sascl_title") = item.Title
            newRow("sascl_ourref") = item.OurRef
            newRow("sascl_yourref") = item.YourRef
            newRow("sascl_message") = item.Message
            newRow("sascl_signby") = item.SignBy
            newRow("sascl_name") = item.Name
            newRow("sascl_updatedby") = item.UpdatedBy
            newRow("sascl_updatedtime") = item.UpdatedTime
            _result.Tables("SponsorCoverLetter").Rows.Add(newRow)
        Next
        Return _result
    End Function

    Private Function GetDataSetSponsor(ByVal obj As SponsorEn) As DataSet
        Dim _result As New DataSet()
        _result.Tables.Add("Sponsor")
        _result.Tables("Sponsor").Columns.Add("sasr_code")
        _result.Tables("Sponsor").Columns.Add("sasr_name")
        _result.Tables("Sponsor").Columns.Add("sassr_sname")
        _result.Tables("Sponsor").Columns.Add("sasr_address")
        _result.Tables("Sponsor").Columns.Add("sasr_address1")
        _result.Tables("Sponsor").Columns.Add("sasr_address2")
        _result.Tables("Sponsor").Columns.Add("sasr_contact")
        _result.Tables("Sponsor").Columns.Add("sasr_phone")

        Dim newRow As DataRow = _result.Tables("Sponsor").NewRow()
        newRow("sasr_code") = obj.SponserCode
        newRow("sasr_name") = obj.SponsorName
        newRow("sassr_sname") = obj.SName
        newRow("sasr_address") = obj.Address
        newRow("sasr_address1") = obj.Address1
        newRow("sasr_address2") = obj.Address2
        newRow("sasr_contact") = obj.Contact
        newRow("sasr_phone") = obj.Phone
        _result.Tables("Sponsor").Rows.Add(newRow)
       Return _result
    End Function

End Class
