using System;
using CP.FastConsig.DAL;
using CP.FastConsig.Facade;
using CP.FastConsig.WebApplication.Auxiliar;
using CP.FastConsig.Util;
using CP.FastConsig.Common;
using CP.FastConsig.WebApplication.FastConsigCenterService;

namespace CP.FastConsig.WebApplication.WebUserControls
{

    public partial class WebUserControlAlterarMinhaSenha : CustomUserControl
    {

        protected void Page_Load(object sender, EventArgs e)
        {

            if (EhPostBack || ControleCarregado) return;

            EhPostBack = true;

            ConfiguraExibicaoUsuario();

        }

        private void ConfiguraExibicaoUsuario()
        {

            int idusuario = Sessao.UsuarioLogado.IDUsuario;
            
            CP.FastConsig.DAL.Usuario usuario = FachadaUsuario.ObtemUsuario(idusuario);

            if (usuario != null)
            {

                string pathImagemPerfil = Util.ObtemPathImagemPerfil(usuario.IDUsuario, Sessao.PastaUpload);
                if (pathImagemPerfil != null) ImagemFloat.ImageUrl = pathImagemPerfil;

                LabelNomePerfil.Text = usuario.NomeCompleto;
                LabelPerfil.Text = usuario.Perfil;

            }

        }

        protected void ButtonAlterarSenha_Click(object sender, EventArgs e)
        {

            if (!ValidaInformacoes()) return;

            FachadaUsuario.AtualizaSenha(Sessao.UsuarioLogado.IDUsuario, TextBoxSenhaNova.Text);

            string senhanova = Seguranca.getMd5Hash(TextBoxSenhaNova.Text);

            using (ServicoUsuarioClient servicoUsuario = new ServicoUsuarioClient()) servicoUsuario.AlterarSenhaUsuario(Sessao.UsuarioLogado.CPF, senhanova, Sessao.IdModulo.Equals((int)Enums.Modulos.Consignante) ? FachadaUsuariosPermissoesEdicao.ObtemIdConsignanteCenter() : Sessao.IdBanco, Sessao.IdModulo.Equals((int)Enums.Modulos.Consignante) ? Enums.TipoCadastradorCenter.C.ToString() : Enums.TipoCadastradorCenter.B.ToString());

        }

        private bool ValidaInformacoes()
        {

            if (Utilidades.ExisteItemVazio(TextBoxSenhaAntiga.Text, TextBoxSenhaNova.Text, TextBoxSenhaNova2.Text))
            {
                PageMaster.ExibeMensagem(ResourceMensagens.MensagemTodosCamposObrigatorios);
                return false;
            }

            if (TextBoxSenhaNova.Text != TextBoxSenhaNova2.Text)
            {
                PageMaster.ExibeMensagem(ResourceMensagens.MensagemSenhasDiferentes);
                return false;
            }

            if (TextBoxSenhaNova.Text.Length < 5)
            {
                PageMaster.ExibeMensagem(ResourceMensagens.MensagemMinimo5Caracteres);
                return false;
            }

            int idusuario = Sessao.UsuarioLogado.IDUsuario;

            CP.FastConsig.DAL.Usuario usuario = FachadaUsuario.ObtemUsuario(idusuario);

            if (usuario != null)
            {

                if (!usuario.Senha.Equals(Seguranca.getMd5Hash(TextBoxSenhaAntiga.Text)))
                {
                    PageMaster.ExibeMensagem(ResourceMensagens.MensagemSenhaNaoConfere);
                    return false;
                }

            }
            else
            {
                PageMaster.ExibeMensagem(ResourceMensagens.MensagemUsuarioNaoEncontrado);
                return false;
            }

            PageMaster.ExibeMensagem(ResourceMensagens.MensagemSucessoOperacao);

            return true;

        }

    }

}