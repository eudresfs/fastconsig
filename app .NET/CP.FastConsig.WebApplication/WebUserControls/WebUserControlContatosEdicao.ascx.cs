using System;
using System.Linq;
using System.Collections.Generic;
using CP.FastConsig.Common;
using CP.FastConsig.DAL;
using CP.FastConsig.Facade;
using CP.FastConsig.WebApplication.Auxiliar;

namespace CP.FastConsig.WebApplication.WebUserControls
{

    public partial class WebUserControlContatosEdicao : CustomUserControl
    {

        #region Constantes

        private const string ParametroIdEmpresaEmEdicao = "IdEmpresaEmEdicao";
        private const string ParametroIdContatoEmEdicao = "IdContatoEmEdicao";

        #endregion

        private int IdEmpresa
        {
            get
            {
                if (ViewState[ParametroIdEmpresaEmEdicao] == null) ViewState[ParametroIdEmpresaEmEdicao] = 0;
                return (int)ViewState[ParametroIdEmpresaEmEdicao];
            }
            set
            {
                ViewState[ParametroIdEmpresaEmEdicao] = value;
            }
        }

        private int IdContatoEdicao
        {
            get
            {
                if (ViewState[ParametroIdContatoEmEdicao] == null) ViewState[ParametroIdContatoEmEdicao] = 0;
                return (int)ViewState[ParametroIdContatoEmEdicao];
            }
            set
            {
                ViewState[ParametroIdContatoEmEdicao] = value;
                PopulaDados();
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            
            
            if (EhPostBack || ControleCarregado) return;

            Novo();

            IdContatoEdicao = Id.HasValue ? Id.Value : 0;
            IdEmpresa = Convert.ToInt32(ParametrosConfiguracao[1]);

            EhPostBack = true;
        }

        protected void ButtonNovoContato_Click(object sender, EventArgs e)
        {
            Novo();
        }

        private void Novo()
        {

            LimpaCampos();
            PopulaPerfils();
            PopulaTipos();

            IdContatoEdicao = 0;

            ConfiguraTopo();

        }

        protected void ButtonSalvarContatoClick(object sender, EventArgs e)
        {

            EmpresaContato dado = new EmpresaContato();

            dado = PopulaContatoObjeto(dado);

            FachadaContatosEdicao.SalvarContato(dado);

            PageMaster.ExibeMensagem(ResourceMensagens.MensagemSucessoOperacao);

            if (ControleAnterior is WebUserControlConsignatariasEdicao) ((WebUserControlConsignatariasEdicao)ControleAnterior).AtualizarContatos();
            
            PageMaster.Voltar();

        }

        private void PopulaPerfils()
        {

            List<EmpresaContatoPerfil> lista = FachadaContatosEdicao.ListaContatoPerfis().ToList();

            DropDownListPerfil.DataSource = lista;
            DropDownListPerfil.DataBind();

        }

        private void PopulaTipos()
        {

            List<ContatoTipo> lista = FachadaContatosEdicao.ListaContatoTipos().ToList();

            DropDownListTipo.DataSource = lista;
            DropDownListTipo.DataBind();

        }

        private EmpresaContato PopulaContatoObjeto(EmpresaContato contato)
        {

            contato.IDEmpresaContato = IdContatoEdicao;
            contato.IDEmpresa = IdEmpresa;
            contato.Nome = TextBoxNome.Text;
            contato.Titulo = TextBoxTitulo.Text;
            contato.Conteudo = TextBoxConteudo.Text;
            contato.IDEmpresaContatoPerfil = Convert.ToInt32(DropDownListPerfil.SelectedValue);
            contato.IDContatoTipo = Convert.ToInt32(DropDownListTipo.SelectedValue); 

            return contato;

        }

        private void LimpaCampos()
        {

            TextBoxNome.Text = string.Empty;
            TextBoxTitulo.Text = string.Empty;
            TextBoxConteudo.Text = string.Empty;
            DropDownListPerfil.SelectedIndex = -1;
            DropDownListTipo.SelectedIndex = -1;

            PopulaPerfils();
            PopulaTipos();

        }

        protected void PopulaDados()
        {

            if (IdContatoEdicao == 0) return;

            EmpresaContato contato = FachadaContatosEdicao.ObtemContato(IdContatoEdicao);

            TextBoxNome.Text = contato.Nome;
            TextBoxTitulo.Text = contato.Titulo;
            TextBoxConteudo.Text = contato.Conteudo;
            DropDownListPerfil.SelectedValue = contato.IDEmpresaContatoPerfil.HasValue ? contato.IDEmpresaContatoPerfil.Value.ToString() : "0";
            DropDownListTipo.SelectedValue = contato.IDContatoTipo.ToString();

            PopulaPerfils();
            PopulaTipos();

            PageMaster.SubTitulo = ResourceMensagens.TituloEditar;

        }
        
    }

}