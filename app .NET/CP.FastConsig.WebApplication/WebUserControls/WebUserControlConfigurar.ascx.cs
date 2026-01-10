using System;
using CP.FastConsig.Common;
using CP.FastConsig.WebApplication.Auxiliar;

namespace CP.FastConsig.WebApplication.WebUserControls
{

    public partial class WebUserControlConfigurar : CustomUserControl
    {
        
        protected void LinkGerenciarUsuarios_Click(object sender, EventArgs e)
        {
            PageMaster.CarregaControle(ResourceAuxiliar.NomeWebUserControlUsuariosPermissoes);
        }

    }

}