using System;
using System.Linq;
using System.Collections.Generic;
using CP.FastConsig.Common;
using CP.FastConsig.DAL;
using CP.FastConsig.Facade;
using CP.FastConsig.WebApplication.Auxiliar;
using System.Web.UI.WebControls;

namespace CP.FastConsig.WebApplication.WebUserControls
{

    public partial class WebUserControlPerfisEdicao : CustomUserControl
    {

        #region Constantes

        private const string ParametroIdEmpresaEmEdicao = "IdEmpresaEmEdicao";
        private const string ParametroIdPerfilEmEdicao = "IdPerfilEmEdicao";

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

        private int IdPerfilEdicao
        {
            get
            {
                if (ViewState[ParametroIdPerfilEmEdicao] == null) ViewState[ParametroIdPerfilEmEdicao] = 0;
                return (int)ViewState[ParametroIdPerfilEmEdicao];
            }
            set
            {
                ViewState[ParametroIdPerfilEmEdicao] = value;
                PopulaDados();
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (EhPostBack || ControleCarregado) return;

            IdPerfilEdicao = Id.HasValue ? Id.Value : 0;

            if (IdPerfilEdicao > 0)
                ButtonCopiar.Visible = true;

            if (Sessao.IdModulo == (int)Enums.Modulos.Consignataria)
            {
                if (DropDownListModulo.Items.FindByValue(((int)Enums.Modulos.Consignataria).ToString()) != null) DropDownListModulo.SelectedValue = ((int)Enums.Modulos.Consignataria).ToString();
                DropDownListModulo.Visible = false;
                DropDownListConsignataria.Visible = false;
                tr_consignataria.Visible = false;
            }
            else
            {
                if (DropDownListModulo.Items.FindByValue(ParametrosConfiguracao[1].ToString()) != null) DropDownListModulo.SelectedValue = ParametrosConfiguracao[1].ToString();
                DropDownListModulo_SelectedIndexChanged(DropDownListModulo, new EventArgs());
            }
            PopularCombos();

            EhPostBack = true;
        }

        protected void ButtonNovo_Click(object sender, EventArgs e)
        {
            Novo();
        }

        protected void ButtonCopiar_Click(object sender, EventArgs e)
        {
            if (Convert.ToInt32(DropDownListConsignataria.SelectedValue) == 0)
            {
                PageMaster.ExibeMensagem(ResourceMensagens.MensagemSelecioneEmpresa);
            }

            if (Convert.ToInt32(DropDownListCopiarPerfil.SelectedValue) == 0)
            {
                PageMaster.ExibeMensagem(ResourceMensagens.MensagemSelecionePerfilACopiar);
            }

            FachadaPerfisEdicao.CopiarPerfil(Convert.ToInt32(DropDownListModulo.SelectedValue) == (int)Enums.Modulos.Consignataria ? Convert.ToInt32(DropDownListConsignataria.SelectedValue) : Sessao.IdBanco, Convert.ToInt32(DropDownListCopiarPerfil.SelectedValue), IdPerfilEdicao);
            PageMaster.ExibeMensagem(ResourceMensagens.MensagemSucessoOperacao);
        }


        private void Novo()
        {

            LimpaCampos();
            PopularCombos();

            IdPerfilEdicao = 0;

            ConfiguraTopo();

        }

        private void PopularCombos()
        {
            DropDownListModulo.DataSource = FachadaPermissoesAcesso.ListarModulos().Where(x => x.IDModulo != (int)Enums.Modulos.Agente && x.IDModulo != (int)Enums.Modulos.Funcionario).ToList();
            DropDownListConsignataria.DataSource = FachadaConciliacao.ListarConsignatarias().ToList();
            DropDownListConsignataria.DataBind();
            DropDownListModulo.DataBind();

            DropDownListCopiarPerfil.DataSource = FachadaPerfisEdicao.Listar(Convert.ToInt32(DropDownListModulo.SelectedValue) == (int)Enums.Modulos.Consignataria ? Convert.ToInt32(DropDownListConsignataria.SelectedValue) : Sessao.IdBanco, Convert.ToInt32(DropDownListModulo.SelectedValue)).ToList();
            DropDownListCopiarPerfil.DataBind();
            DropDownListCopiarPerfil.Items.Insert(0, new ListItem(ResourceMensagens.LabelSelecione, "0"));

            DropDownListConsignataria.Items.Insert(0, new ListItem(ResourceMensagens.LabelSelecione, "0"));
            if (Sessao.IdModulo == (int)Enums.Modulos.Consignataria)
                DropDownListConsignataria.SelectedValue = Sessao.IdBanco.ToString();


            if (DropDownListModulo.Items.FindByValue(Sessao.IdModulo.ToString()) != null) DropDownListModulo.SelectedValue = Sessao.IdModulo.ToString();
            DropDownListModulo.Visible = Sessao.IdModulo == (int)Enums.Modulos.Consignante;
            LabelModulo.Visible = DropDownListModulo.Visible;
        }

        protected void ButtonSalvarClick(object sender, EventArgs e)
        {

            Perfil dado = new Perfil();

            dado = PopulaObjeto(dado);

            FachadaPerfisEdicao.Salvar(dado);

            if (Convert.ToInt32(DropDownListCopiarPerfil.SelectedValue) > 0 && (Convert.ToInt32(DropDownListModulo.SelectedValue) != (int)Enums.Modulos.Consignataria || Convert.ToInt32(DropDownListConsignataria.SelectedValue) > 0))
                FachadaPerfisEdicao.CopiarPerfil(Convert.ToInt32(DropDownListModulo.SelectedValue) == (int)Enums.Modulos.Consignataria ? Convert.ToInt32(DropDownListConsignataria.SelectedValue) : Sessao.IdBanco, Convert.ToInt32(DropDownListCopiarPerfil.SelectedValue), dado.IDPerfil);

            if (ControleAnterior is WebUserControlPerfis) ((WebUserControlPerfis)ControleAnterior).AtualizarGrid();

            PageMaster.Voltar();

        }


        private Perfil PopulaObjeto(Perfil perfil)
        {

            perfil.IDPerfil = IdPerfilEdicao;
            if (Convert.ToInt32(DropDownListConsignataria.SelectedValue) > 0)
                perfil.IDEmpresa = Convert.ToInt32(DropDownListConsignataria.SelectedValue);
            perfil.Nome = TextBoxNome.Text;
            perfil.IDModulo = Convert.ToInt32(DropDownListModulo.SelectedValue);

            return perfil;

        }

        private void LimpaCampos()
        {

            PopularCombos();

            TextBoxNome.Text = string.Empty;
            DropDownListModulo.SelectedIndex = -1;
            DropDownListConsignataria.SelectedIndex = -1;
        }

        protected void PopulaDados()
        {

            if (IdPerfilEdicao == 0) return;

            Perfil perfil = FachadaPerfisEdicao.ObtemPerfil(IdPerfilEdicao);

            TextBoxNome.Text = perfil.Nome;
            if (DropDownListModulo.Items.FindByValue(perfil.IDModulo.ToString()) != null) DropDownListModulo.SelectedValue = perfil.IDModulo.ToString();
            DropDownListModulo_SelectedIndexChanged(DropDownListModulo, new EventArgs());

            if (perfil.IDEmpresa != null)
                DropDownListConsignataria.SelectedValue = perfil.IDEmpresa.ToString();
            else
                DropDownListConsignataria.SelectedIndex = -1;

            PageMaster.SubTitulo = ResourceMensagens.TituloEditar;

        }

        protected void DropDownListModulo_SelectedIndexChanged(object sender, EventArgs e)
        {

            if (DropDownListModulo.SelectedValue == ((int)Enums.Modulos.Consignataria).ToString())
            {
                tr_consignataria.Visible = true;
                DropDownListConsignataria.Visible = true;
            }
            else
            {
                DropDownListConsignataria.Visible = false;
                tr_consignataria.Visible = false;
            }

        }
    }

}