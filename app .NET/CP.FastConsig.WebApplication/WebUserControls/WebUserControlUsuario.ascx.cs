using System;
using CP.FastConsig.DAL;
using CP.FastConsig.Facade;
using CP.FastConsig.WebApplication.Auxiliar;

namespace CP.FastConsig.WebApplication.WebUserControls
{

    public partial class WebUserControlUsuario : CustomUserControl
    {

        protected void Page_Load(object sender, EventArgs e)
        {

            if (EhPostBack || ControleCarregado) return;

            EhPostBack = true;

            ConfiguraExibicaoUsuario();

        }

        private void ConfiguraExibicaoUsuario()
        {

            Usuario usuario = FachadaUsuario.ObtemUsuario(Id ?? 0);

            if (usuario != null)
            {

                string pathImagemPerfil = Util.ObtemPathImagemPerfil(usuario.IDUsuario, Sessao.PastaUpload);
                if (pathImagemPerfil != null) ImagemFloat.ImageUrl = pathImagemPerfil;

                LabelNomePerfil.Text = usuario.NomeCompleto;
                LabelPerfil.Text = usuario.Perfil;

            }

        }

    }

}