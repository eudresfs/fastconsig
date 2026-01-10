using System;
using CP.FastConsig.Common;
using CP.FastConsig.WebApplication.Auxiliar;

namespace CP.FastConsig.WebApplication.WebUserControls
{

    public partial class WebUserControlImportarExportar : CustomUserControl
    {

        protected void LinkImportacao_Click(object sender, EventArgs e)
        {
            PageMaster.CarregaControle(ResourceAuxiliar.NomeWebUserControlImportacao);
        }
        
    }

}