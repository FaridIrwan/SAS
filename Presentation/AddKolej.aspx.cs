using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using HTS.SAS.Entities;
using HTS.SAS.BusinessObjects;

public partial class AddKolej : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    protected void ImageButton1_Click(object sender, ImageClickEventArgs e)
    {
        KolejEn lokolej = new KolejEn();
        lokolej.SAKO_Description = txtName.Text;
        KolejBAL LOBO = new KolejBAL();
        LOBO.Insert(lokolej);
        Response.Redirect("HostelFee.aspx?Menuid=12");

    }
}
