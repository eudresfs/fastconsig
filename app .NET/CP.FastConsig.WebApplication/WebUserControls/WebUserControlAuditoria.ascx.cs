using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CP.FastConsig.WebApplication.Auxiliar;
using CP.FastConsig.Facade;
using CP.FastConsig.DAL;

namespace CP.FastConsig.WebApplication.WebUserControls
{
    public partial class WebUserControlAuditoria : CustomUserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (EhPostBack) return;

            EhPostBack = true;
        }

        protected void ButtonPesquisar_Click(object sender, EventArgs e)
        {
            List<Auditoria> a = FachadaAuditoria.localizaAuditoria(TextBoxPesquisar.Text).ToList();

            gridAuditoria.DataSource = a;
            gridAuditoria.DataBind();
        }
    }
}