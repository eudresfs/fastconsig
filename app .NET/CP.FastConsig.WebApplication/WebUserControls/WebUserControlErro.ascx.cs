using System;
using CP.FastConsig.WebApplication.Auxiliar;
using System.Data.SqlClient;

namespace CP.FastConsig.WebApplication.WebUserControls
{

    public partial class WebUserControlErro : CustomUserControl
    {
        
        protected void Page_Load(object sender, EventArgs e)
        {

            if (EhPostBack || ControleCarregado) return;

            EhPostBack = true;

            Exception ex = (Exception) ParametrosConfiguracao[1];

            string textoerro;

            if (ex.InnerException != null) textoerro = ex.InnerException.Message;
            else if (ex is SqlException) textoerro = ex.Message;
            else textoerro = ex.Message;

            TextBoxErro.Text = textoerro;

        }

    }

}