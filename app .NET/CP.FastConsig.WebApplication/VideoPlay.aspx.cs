using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CP.FastConsig.WebApplication
{
    public partial class VideoPlay : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if(Session["PodeVerVideo"] == null || !((bool) Session["PodeVerVideo"])) Response.Redirect("~/video.aspx");
        }
    }
}