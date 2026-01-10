using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;

namespace CP.FastConsig.WebApplication.Arquivos
{
    public partial class DownloadFile : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string FileResponse = Request.QueryString[0];
            string arquivo = Path.Combine(Path.Combine(Request.PhysicalApplicationPath, "Arquivos"), FileResponse); // Path.Combine(@"c:\temp", "xxx.pdf"); // Request.QueryString["arquivo"];

            // Code here to fill FileResponse with the 
            //  appropriate data based on the selected Region.

            Response.AddHeader("Content-disposition", "attachment; filename="+FileResponse);
            Response.ContentType = "application/octet-stream";
            //Response.Write("Some,Csv,Data\r\nPlease,Download,Me");
            Response.WriteFile(arquivo);
            Response.End();
        }
    }
}