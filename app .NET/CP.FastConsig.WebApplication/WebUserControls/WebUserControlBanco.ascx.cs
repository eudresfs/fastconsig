using System;
using CP.FastConsig.Common;
using CP.FastConsig.WebApplication.Auxiliar;

namespace CP.FastConsig.WebApplication.WebUserControls
{

    public partial class WebUserControlBanco : CustomUserControl
    {

        private enum ViewsBancos
        {
            Estados,
            Cidades,
            Autarquias,
            Favoritos
        }

        protected void ButtonEstados_Click(object sender, EventArgs e)
        {
            MultiViewOpcoesBancos.ActiveViewIndex = (int) ViewsBancos.Estados;
        }

        protected void ButtonCidades_Click(object sender, EventArgs e)
        {
            MultiViewOpcoesBancos.ActiveViewIndex = (int)ViewsBancos.Cidades;
        }

        protected void ButtonAutarquias_Click(object sender, EventArgs e)
        {
            MultiViewOpcoesBancos.ActiveViewIndex = (int)ViewsBancos.Autarquias;
        }

        protected void ButtonFavoritos_Click(object sender, EventArgs e)
        {
            MultiViewOpcoesBancos.ActiveViewIndex = (int)ViewsBancos.Favoritos;
        }
        
    }

}