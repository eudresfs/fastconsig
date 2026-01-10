using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;
using CP.FastConsig.Common;
using CP.FastConsig.DAL;
using CP.FastConsig.Facade;
using CP.FastConsig.WebApplication.Auxiliar;
using CP.FastConsig.WebApplication.FastConsigCenterService;
using DevExpress.Web.ASPxEditors;
using Usuario = CP.FastConsig.DAL.Usuario;

namespace CP.FastConsig.WebApplication.WebUserControls
{

    public partial class WebUserControlUsuariosPermissoes : CustomUserControl
    {

        #region Constantes

        private const string ParametroIdEmpresaEmEdicao = "IdEmpresaEmEdicao";
        private const string ParametroExibirTitulo = "ExibirTitulo";
        private const string ParametroQuantidadeTotalResultados = "QuantidadeTotalResultados";
        private const string ParametroIdModulo = "IdModulo";
        private const string IdControleDropDownPagina = "DropDownPagina";
        private const string IdControleLabelPaginas = "LabelPaginas";
        private const string IdControleLabelTotal = "LabelTotal";
        private const string IdControleLabelPerfil = "LabelPerfil";
        private const string TegTextoWaterMark = "#textoWaterMark";
        private const string TagClientIdTextBox = "#clientIdTextBox";
        private const string ParametroCadastroAgente = "CadastroAgente";

        #endregion

        private bool CadastroAgente
        {

            get
            {
                if (Session[ParametroCadastroAgente] == null) Session[ParametroCadastroAgente] = false;
                return (bool)Session[ParametroCadastroAgente];
            }
            set
            {
                Session[ParametroCadastroAgente] = value;
            }

        }

        public bool ExibirTitulo
        {
            get
            {
                if (ViewState[ParametroExibirTitulo] == null) ViewState[ParametroExibirTitulo] = true;
                return (bool)ViewState[ParametroExibirTitulo];
            }
            set
            {
                ViewState[ParametroExibirTitulo] = value;
            }
        }

        public int IdRecursoWebUserControl
        {
            get
            {
                return IdRecurso;
            }
            set
            {
                IdRecurso = value;
            }
        }

        public int IdEmpresa
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

        public void AtualizaUsuarios()
        {            
            PopulaGridUsuarios();
        }

        private void PopulaGridUsuarios()
        {
            GridViewUsuarios.DataBind();
        }

        protected void ButtonPesquisarUsuarios_Click(object sender, EventArgs e)
        {
            
        }

        protected void Page_Load(object sender, EventArgs e)
        {

            if (EhPostBack || ControleCarregado) return;

            if (IdEmpresa == 0) CadastroAgente = false;

            EhPostBack = true;

            if ((IdEmpresa == 0) || (IdEmpresa == null)) IdEmpresa = Sessao.IdBanco;

            IdModulo = Sessao.IdModulo;

            ConfiguraComponentes();

            if (ParametrosConfiguracao != null && ParametrosConfiguracao.Count > 1)
            {

                string pesquisa = ParametrosConfiguracao[1].ToString();

                PageMaster.Titulo = ResourceMensagens.TituloResultadoBusca;

                TextBoxPesquisarUsuarios.Text = pesquisa;

            }

        }

        private void ConfiguraComponentes()
        {

            ConfiguraWaterMark();

            if(Sessao.IdModulo.Equals((int) Enums.Modulos.Consignante))
            {
                ConfiguraModulos();
            }
            else
            {

                ASPxComboBoxModulos.Visible = false;
                ASPxComboBoxBancos.Visible = true;

                var listaConsignatarias = FachadaUsuariosPermissoes.ObtemAgentesConsignataria(Sessao.IdBanco);

                ASPxComboBoxBancos.DataSource = listaConsignatarias;
                ASPxComboBoxBancos.DataBind();

                ASPxComboBoxBancos.Items.Insert(0, new ListEditItem(ResourceMensagens.LabelAgente, 0));

                ASPxComboBoxBancos.SelectedIndex = 0;

                if (CadastroAgente)
                {
                    ASPxComboBoxBancos.Enabled = false;
                    if (ASPxComboBoxBancos.Items.Count > 1) ASPxComboBoxBancos.SelectedIndex = 1;
                }
                
            }

        }

        private void ConfiguraWaterMark()
        {

            Dictionary<string, string> tagsValores = new Dictionary<string, string>();

            tagsValores.Add(TagClientIdTextBox, string.Format("#{0}",TextBoxPesquisarUsuarios.ClientID));
            tagsValores.Add(TegTextoWaterMark, ResourceMensagens.WaterMarkTextBoxPesquisaUsuario);

            LimpaScripts();

            AdicionaArquivoScriptParaExecucao("WaterMarkTextBox", tagsValores);

        }

        private void ConfiguraModulos()
        {

            List<Modulo> modulos = FachadaUsuariosPermissoes.ObtemModulos().Where(x => !x.IDModulo.Equals((int)Enums.Modulos.Funcionario)).ToList();

            if (Sessao.IdModulo.Equals((int)Enums.Modulos.Consignante)) modulos.Remove(modulos.Single(x => x.IDModulo.Equals((int)Enums.Modulos.Agente)));
            if (Sessao.IdModulo.Equals((int)Enums.Modulos.Consignataria)) modulos.Remove(modulos.Single(x => x.IDModulo.Equals((int)Enums.Modulos.Consignante)));

            ASPxComboBoxModulos.DataSource = modulos;
            ASPxComboBoxModulos.DataBind();

            ASPxComboBoxModulos.Items.Insert(0, new ListEditItem(ResourceMensagens.LabelTodosUsuarios, 0));

            ASPxComboBoxModulos.SelectedIndex = 0;

        }

        protected void ButtonNovoUsuario_Click(object sender, EventArgs e)
        {
            PageMaster.CarregaControle(ResourceAuxiliar.NomeWebUserControlUsuariosPermissoesEdicao, this.IdRecurso, 1, 0, IdEmpresa);
        }

        protected void ImageButtonRemover_Click(object sender, EventArgs e)
        {

            LinkButton linkButtonRemover = (LinkButton)sender;

            int idUsuario = Convert.ToInt32(linkButtonRemover.CommandArgument);

            FachadaUsuariosPermissoes.RemoveUsuario(idUsuario);

            Usuario usuario = FachadaUsuariosPermissoes.ObtemUsuario(idUsuario);

            using (ServicoUsuarioClient servicoUsuario = new ServicoUsuarioClient()) servicoUsuario.ExcluirUsuarioPorCpf(usuario.CPF, Sessao.IdModulo.Equals((int)Enums.Modulos.Consignante) ? FachadaUsuariosPermissoesEdicao.ObtemIdConsignanteCenter() : Sessao.IdBanco, Sessao.IdModulo.Equals((int)Enums.Modulos.Consignante) ? Enums.TipoCadastradorCenter.C.ToString() : Enums.TipoCadastradorCenter.B.ToString());

            AtualizaUsuarios();

            PageMaster.ExibeMensagem(ResourceMensagens.MensagemSucessoOperacao);

        }

        protected void GridViewUsuarios_Sorting(object sender, GridViewSortEventArgs e)
        {
            
        }

        protected void GridViewUsuarios_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GridViewUsuarios.PageIndex = e.NewPageIndex;
        }
        
        protected void ODS_Usuarios_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
        {
            e.InputParameters[0] = IdEmpresa;
            e.InputParameters[1] = IdModulo;
        }

        private int IdModulo
        {
            get
            {
                if (ViewState[ParametroIdModulo] == null) ViewState[ParametroIdModulo] = 0;
                return (int) ViewState[ParametroIdModulo];
            }
            set
            {
                ViewState[ParametroIdModulo] = value;
            }
        }

        protected void ASPxComboBoxModulos_SelectedIndexChanged(object sender, EventArgs e)
        {

            if (ASPxComboBoxModulos.SelectedIndex.Equals(0))
            {

                IdModulo = 0;
                IdEmpresa = 0;

                ASPxComboBoxBancos.Visible = false;
                GridViewUsuarios.DataBind();

                return;

            }

            if (((int)ASPxComboBoxModulos.Value).Equals((int)Enums.Modulos.Consignataria) || ((int)ASPxComboBoxModulos.Value).Equals((int)Enums.Modulos.Agente))
            {

                IdModulo = 0;
                
                ASPxComboBoxBancos.Visible = true;

                List<Empresa> listaConsignatarias = new List<Empresa>();

                if (((int)ASPxComboBoxModulos.Value).Equals((int)Enums.Modulos.Agente)) listaConsignatarias = FachadaUsuariosPermissoes.ObtemAgentesConsignatarias();
                else listaConsignatarias = Sessao.IdModulo == (int)Enums.Modulos.Consignataria ? FachadaUsuariosPermissoes.ObtemAgentesConsignataria(Sessao.IdBanco) : FachadaUsuariosPermissoes.ObtemConsignatarias();

                ASPxComboBoxBancos.DataSource = listaConsignatarias;
                ASPxComboBoxBancos.DataBind();

                ASPxComboBoxBancos.Items.Insert(0, new ListEditItem(ResourceMensagens.LabelConsignataria, 0));

                if (listaConsignatarias.Count > 0)
                {
                    ASPxComboBoxBancos.SelectedIndex = 0;
                    IdEmpresa = (int)ASPxComboBoxBancos.Value;
                }

            }
            else
            {
                ASPxComboBoxBancos.Visible = false;
            }

            IdModulo = (int) ASPxComboBoxModulos.Value;

            GridViewUsuarios.DataBind();

        }

        protected void ASPxComboBoxBancos_SelectedIndexChanged(object sender, EventArgs e)
        {
            IdEmpresa = (int)ASPxComboBoxBancos.Value;
            GridViewUsuarios.DataBind();
        }

        protected void grid_DataBound(Object sender, EventArgs e)
        {

            GridViewRow gvrPager = GridViewUsuarios.BottomPagerRow;

            if (gvrPager == null) return;

            DropDownList DropDownPagina = (DropDownList)gvrPager.Cells[0].FindControl(IdControleDropDownPagina);
            Label LabelPaginas = (Label)gvrPager.Cells[0].FindControl(IdControleLabelPaginas);

            if (DropDownPagina != null)
            {
                for (int i = 0; i < GridViewUsuarios.PageCount; i++)
                {

                    int intPageNumber = i + 1;
                    ListItem lstItem = new ListItem(intPageNumber.ToString());

                    if (i == GridViewUsuarios.PageIndex)
                        lstItem.Selected = true;

                    DropDownPagina.Items.Add(lstItem);
                }
            }

            int paginaAtual = GridViewUsuarios.PageIndex + 1;
            int quantidadeDeItensNaPagina = GridViewUsuarios.Rows.Count;
            int quantidadeDePaginas = GridViewUsuarios.PageCount;
            
            if (paginaAtual > 1) quantidadeDeItensNaPagina = ((paginaAtual - 1)*GridViewUsuarios.PageSize) + quantidadeDeItensNaPagina;
            if (LabelPaginas != null) LabelPaginas.Text = quantidadeDePaginas.ToString();

            Label labelTotal = (Label)GridViewUsuarios.FooterRow.FindControl(IdControleLabelTotal);

            labelTotal.Text = String.Format("Total: {0}/{1}", quantidadeDeItensNaPagina, QuantidadeTotalResultados);

            GridViewUsuarios.FooterRow.Cells[0].ColumnSpan = GridViewUsuarios.FooterRow.Cells.Count;

            for (int i = 1; i < GridViewUsuarios.FooterRow.Cells.Count; i++) GridViewUsuarios.FooterRow.Cells[i].Visible = false;
            
        }

        protected void dropwdown_SelectedIndexChangend(Object sender, EventArgs e)
        {

            GridViewRow gvrPager = GridViewUsuarios.BottomPagerRow;
            DropDownList dropDownPagina = (DropDownList)gvrPager.Cells[0].FindControl(IdControleDropDownPagina);

            GridViewUsuarios.PageIndex = dropDownPagina.SelectedIndex;

            GridViewUsuarios.DataBind();

        }

        protected void ImageButtonEditar_Click(object sender, EventArgs e)
        {

            ImageButton linkButtonEditar = (ImageButton)sender;

            int id = Convert.ToInt32(linkButtonEditar.CommandArgument);

            PageMaster.CarregaControle(ResourceAuxiliar.NomeWebUserControlUsuariosPermissoesEdicao, IdRecurso, 1, id, IdEmpresa);

        }

        protected void ODS_Usuarios_Selected(object sender, ObjectDataSourceStatusEventArgs e)
        {

            int quantidadeResultados;
            int.TryParse(e.ReturnValue.ToString(), out quantidadeResultados);

            QuantidadeTotalResultados = quantidadeResultados;

        }

        private int QuantidadeTotalResultados
        {

            get
            {
                if (ViewState[ParametroQuantidadeTotalResultados] == null) ViewState[ParametroQuantidadeTotalResultados] = 0;
                return (int)ViewState[ParametroQuantidadeTotalResultados];
            }
            set
            {
                ViewState[ParametroQuantidadeTotalResultados] = value;
            }

        }

        protected void GridViewUsuarios_RowDataBound(object sender, GridViewRowEventArgs e)
        {

            GridViewRow linha = e.Row;

            if (linha.RowType != DataControlRowType.DataRow) return;

            Label labelPerfil = (Label)linha.FindControl(IdControleLabelPerfil);

            Usuario usuario = (Usuario) linha.DataItem;

            int idModuloSelecionado = Sessao.IdModulo;
            int? idBancoSelecionado = null;

            if(ASPxComboBoxModulos.SelectedIndex > 0) idModuloSelecionado = Convert.ToInt32(ASPxComboBoxModulos.Value);
            if (ASPxComboBoxBancos.SelectedIndex > 0) idBancoSelecionado = Convert.ToInt32(ASPxComboBoxBancos.Value);

            List<UsuarioPerfil> perfisNoModulo = usuario.UsuarioPerfil.Where(x => x.Perfil.IDModulo.Equals(idModuloSelecionado)).ToList();

            if (idBancoSelecionado != null) perfisNoModulo = perfisNoModulo.Where(x => x.IDEmpresa.Equals(idBancoSelecionado.Value)).ToList();

            labelPerfil.Text = perfisNoModulo.Count > 0 ? perfisNoModulo.First().Perfil.Nome : "---";

        }

    }

}