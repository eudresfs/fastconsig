using System;
using System.Linq;
using System.Collections.Generic;
using CP.FastConsig.Common;
using CP.FastConsig.DAL;
using CP.FastConsig.Facade;
using CP.FastConsig.WebApplication.Auxiliar;
using CP.FastConsig.Util;


namespace CP.FastConsig.WebApplication.WebUserControls
{

    public partial class WebUserControlProdutosEdicao : CustomUserControl
    {

        #region Constantes

        private const string ParametroIdEmpresaEmEdicao = "IdEmpresaEmEdicao";
        private const string ParametroIdProdutoEmEdicao = "IdProdutoEmEdicao";

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

        private int IdProdutoEdicao
        {
            get
            {
                if (ViewState[ParametroIdProdutoEmEdicao] == null) ViewState[ParametroIdProdutoEmEdicao] = 0;
                return (int)ViewState[ParametroIdProdutoEmEdicao];
            }
            set
            {
                ViewState[ParametroIdProdutoEmEdicao] = value;
                PopulaDados();

            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {

            if (EhPostBack || ControleCarregado) return;

            PopulaConsignantes();
            PopulaGrupos();

            IdProdutoEdicao = Id.HasValue ? Id.Value : 0;
            IdEmpresa = Convert.ToInt32(ParametrosConfiguracao[1]);

            EhPostBack = true;

            ConfiguraCamposEditaveis();

        }

        private void ConfiguraCamposEditaveis()
        {
            if (Sessao.IdModulo == (int)Enums.Modulos.Consignataria)
            {
                TextBoxVerba.Enabled = false;
                TextBoxVerbaFolha.Enabled = false;
                DropDownListConsiganante.Enabled = false;
                DropDownListGrupo.Enabled = false;
            }
        }

        protected void ButtonNovoProduto_Click(object sender, EventArgs e)
        {
            Novo();
        }

        private void Novo()
        {
            LimpaProdutoCampos();

            IdProdutoEdicao = 0;
            PageMaster.SubTitulo = ResourceMensagens.TituloNovo;

        }

        private bool ValidaInformacoes()
        {

            if (Utilidades.ExisteItemVazio(TextBoxProdutoNome.Text, TextBoxVerba.Text, TextBoxVerbaFolha.Text))
            {
                PageMaster.ExibeMensagem(ResourceMensagens.MensagemTodosCamposObrigatorios);
                return false;
            }
            return true;
        }

        protected void ButtonSalvarProdutoClick(object sender, EventArgs e)
        {
            if (!ValidaInformacoes()) return;

            Produto dado = new Produto();

            dado = PopulaProdutoObjeto(dado);

            FachadaProdutosEdicao.SalvarProduto(dado);

            if (ControleAnterior is WebUserControlConsignatariasEdicao) ((WebUserControlConsignatariasEdicao)ControleAnterior).AtualizarProdutos();
            
            PageMaster.Voltar();

        }

        private void PopulaConsignantes()
        {

            List<Empresa> lista = FachadaProdutosEdicao.ListaConsignantes().Where(x => x.IDEmpresaTipo == (int)Enums.EmpresaTipo.Consignante).ToList();

            //if (!string.IsNullOrEmpty(DropDownListConsiganante.SelectedValue) && !lista.Any(x => x.IDEmpresa == Convert.ToInt32(DropDownListConsiganante.SelectedValue)))
            //    DropDownListConsiganante.SelectedValue = null;
            DropDownListConsiganante.DataSource = lista;
            DropDownListConsiganante.DataBind();

        }

        private void PopulaGrupos()
        {

            List<ProdutoGrupo> lista = FachadaProdutosEdicao.ListaProdutoGrupos().ToList();

            DropDownListGrupo.DataSource = lista;
            DropDownListGrupo.DataBind();

        }

        private Produto PopulaProdutoObjeto(Produto serv)
        {

            serv.IDProduto = IdProdutoEdicao;

            serv.Nome = TextBoxProdutoNome.Text;
            serv.Verba = TextBoxVerba.Text;
            serv.VerbaFolha = TextBoxVerbaFolha.Text;
            serv.IDConsignataria = IdEmpresa;
            serv.IDConsignante = Convert.ToInt32(DropDownListConsiganante.SelectedValue);
            serv.IDProdutoGrupo = Convert.ToInt32(DropDownListGrupo.SelectedValue);
            serv.CarenciaMaxima = Convert.ToInt32(string.IsNullOrEmpty(TextBoxCarenciaMaxima.Text) ? "0" : TextBoxCarenciaMaxima.Text);
            serv.DesativadoConsignante = false;

            return serv;

        }

        private void LimpaProdutoCampos()
        {

            TextBoxProdutoNome.Text = string.Empty;
            TextBoxVerba.Text = string.Empty;
            TextBoxVerbaFolha.Text = string.Empty;
//            DropDownListConsiganante.SelectedIndex = -1;
//            DropDownListGrupo.SelectedIndex = -1;
            TextBoxCarenciaMaxima.Text = "0";

            PopulaConsignantes();
            PopulaGrupos();

        }

        protected void PopulaDados()
        {

            if (IdProdutoEdicao == 0) return;

            Produto serv = FachadaProdutosEdicao.ObtemProduto(IdProdutoEdicao);

            TextBoxProdutoNome.Text = serv.Nome;
            TextBoxVerba.Text = serv.Verba;
            TextBoxVerbaFolha.Text = serv.VerbaFolha;
            DropDownListConsiganante.SelectedValue = serv.IDConsignante.HasValue ? serv.IDConsignante.Value.ToString() : "0";
            TextBoxCarenciaMaxima.Text = (serv.CarenciaMaxima ?? 0).ToString();
            DropDownListGrupo.SelectedValue = serv.IDProdutoGrupo.ToString();

            PopulaConsignantes();
            PopulaGrupos();

            PageMaster.SubTitulo = ResourceMensagens.TituloEditar;            
        }

    }

}