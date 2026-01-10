using System;
using CP.FastConsig.WebApplication.Auxiliar;

namespace CP.FastConsig.WebApplication.WebUserControls
{

    public partial class WebUserControlImportacaoFiltros : CustomUserControl
    {

        protected void Page_Load(object sender, EventArgs e)
        {
            LinkButtonAdicionar.OnClientClick = string.Format("$('#{0}').clone().appendTo('#{1}'); return false;", pFiltros.ClientID, DivFiltroColuna.ClientID);
            LinkButtonRemover.OnClientClick = string.Format("$('#{0}').children().last().remove(); return false;", DivFiltroColuna.ClientID);
        }

        public void ConfiguraCampoFiltro(string campo)
        {
            InputColunaFiltro.Value = campo;
        }

    }

}