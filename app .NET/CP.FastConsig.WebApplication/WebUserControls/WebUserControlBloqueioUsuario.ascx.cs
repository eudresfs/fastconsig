using System;
using System.Collections.Generic;
using System.Linq;
using CP.FastConsig.Common;
using CP.FastConsig.DAL;
using CP.FastConsig.Facade;
using CP.FastConsig.WebApplication.Auxiliar;
using DevExpress.Web.ASPxEditors;

namespace CP.FastConsig.WebApplication.WebUserControls
{

    public partial class WebUserControlBloqueioUsuario : CustomUserControl
    {

        #region Constantes
        
        private const string ParametroProdutos = "Produtos";
        private const string ColunaProdutoGrupo = "ProdutoGrupo";

        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {

            if (EhPostBack || ControleCarregado) return;
            
            if (Id == null) return;

            Funcionario funcionario = FachadaBloqueioUsuario.PesquisarFuncionarioPorId(Id.Value);

            LabelCpfFuncionario.Text = funcionario.Pessoa.CPFMascara;
            LabelMatriculaFuncionario.Text = funcionario.Matricula;
            LabelNomeFuncionario.Text = funcionario.Pessoa.Nome;

            EhPostBack = true;

            ASPxListBoxEmpresasBloqueio.DataSource = FachadaBloqueioUsuario.ListaConsignatarias().ToList();
            ASPxListBoxEmpresasBloqueio.DataBind();
       
            CarregaBloqueios();
            CarregaProdutos();

        }

        private void CarregaProdutos()
        {

            ASPxGridViewProdutos.DataSource = Produtos;
            ASPxGridViewProdutos.DataBind();

            ASPxGridViewProdutos.GroupBy(ASPxGridViewProdutos.Columns[ColunaProdutoGrupo]);
            ASPxGridViewProdutos.ExpandAll();

        }

        private List<ProdutoAux> Produtos
        {
            get
            {
                if (Session[ParametroProdutos] == null) Session[ParametroProdutos] = FachadaBloqueioUsuario.ListaProdutos(0).ToList().Select(x => new ProdutoAux { IdProduto = x.IDProduto, ProdutoGrupo = x.ProdutoGrupo.Nome, Produto = x.Nome, Verba = x.Verba, Banco = x.Empresa.Nome }).ToList();
                return (List<ProdutoAux>)Session[ParametroProdutos];
            }
        }

        class ProdutoAux
        {

            public int IdProduto { get; set; }

            public string ProdutoGrupo { get; set; }
            public string Produto { get; set; }
            public string Verba { get; set; }
            public string Banco { get; set; }

        }

        private void CarregaBloqueios()
        {
            var dados = FachadaBloqueioUsuario.ObtemBloqueios(Id.Value).ToList().Select(x => new { Autor = x.CreatedBy == null ? "---" : FachadaBloqueioUsuario.ObtemUsuario(x.CreatedBy.Value).NomeCompleto, IDFuncionarioBloqueio = x.IDFuncionarioBloqueio, Motivo = x.Motivo, Produto = x.Chaves.Equals("0") ? "Todos" : string.Format("{0} - {1}", FachadaBloqueioUsuario.ObtemProduto(Convert.ToInt32(x.Chaves)).Nome, FachadaBloqueioUsuario.ObtemProduto(Convert.ToInt32(x.Chaves)).Empresa.Nome), DataBloqueio = x.DataBloqueio, TipoBloqueio = ((Enums.BloqueioTipo)Convert.ToInt32(x.TipoBloqueio)).ToString() });           

            if (dados.Count() > 0)
            {
                ASPxGridViewBloqueio.DataSource = dados.ToList();
                ASPxGridViewBloqueio.DataBind();
            }
            else
            {
                LimpaCampos();
                EntraEmModoInsercao(true);
            }


        }

        protected void ButtonVoltar_Click(object sender, EventArgs e)
        {
            EntraEmModoInsercao(false);
        }

        private void LimpaCampos()
        {

            TextBoxMotivo.Text = string.Empty;

            DropDownListTipoBloqueio.SelectedIndex = (int) Enums.BloqueioTipo.Completo;

            MultiViewBloqueio.Visible = false;

            ASPxListBoxEmpresasBloqueio.UnselectAll();

            ASPxGridViewBloqueio.Selection.UnselectAll();

        }

        private void EntraEmModoInsercao(bool visivel)
        {

            DivGridConfiguracao.Visible = !visivel;
            ButtonRemoverTodos.Visible = !visivel && ASPxGridViewBloqueio.VisibleRowCount > 0;
            DivBloqueio.Visible = visivel;
            
        }

        protected void DropDownListTipoBloqueio_SelectedIndexChanged(object sender, EventArgs e)
        {

            if (DropDownListTipoBloqueio.SelectedIndex.Equals(0))
            {
                MultiViewBloqueio.Visible = false;
                return;
            }

            MultiViewBloqueio.Visible = true;

            int indiceParaExibir = DropDownListTipoBloqueio.SelectedIndex - 1;

            MultiViewBloqueio.ActiveViewIndex = indiceParaExibir;

            if (indiceParaExibir.Equals(0)) CarregaProdutos();

        }

        protected void ButtonNovo_Click(object sender, EventArgs e)
        {
            LimpaCampos();
            EntraEmModoInsercao(true);
        }

        protected void ButtonSalvarBloqueio_Click(object sender, EventArgs e)
        {

            List<int> idsProdutosBloqueio = new List<int>();

            if (string.IsNullOrEmpty(TextBoxMotivo.Text))
            {
                PageMaster.ExibeMensagem(ResourceMensagens.MensagemFaltaPreencherMotivo);
                return;
            }

            switch (DropDownListTipoBloqueio.SelectedIndex)
            {

                case ((int)Enums.BloqueioTipo.Completo):

                    idsProdutosBloqueio = null;
                    break;

                case ((int)Enums.BloqueioTipo.TipoProduto):

                    CarregaProdutos();
                    idsProdutosBloqueio = HiddenFieldItensSelecionados.Value.Split(new[] { ',' }).Select(x => Convert.ToInt32(x)).ToList();

                    break;

                case ((int)Enums.BloqueioTipo.TipoEmpresa):

                    idsProdutosBloqueio = FachadaBloqueioUsuario.ListaProdutos(ASPxListBoxEmpresasBloqueio.SelectedValues.Cast<int>().ToList()).Select(x => x.IDProduto).ToList();
                    break;

            }

            if (idsProdutosBloqueio != null && !idsProdutosBloqueio.Any())
            {
                PageMaster.ExibeMensagem(ResourceMensagens.MensagemSelecionarProdutos);
                return;
            }

            List<int> ProdutosBloqueadosAnteriormente = FachadaBloqueioUsuario.ObtemBloqueios(Id.Value).ToList().Select(x => Convert.ToInt32(x.Chaves)).ToList();

            bool jaExisteBloqueioCompleto = ProdutosBloqueadosAnteriormente.Contains(0);

            FachadaBloqueioUsuario.SalvaBloqueios(Id.Value, DropDownListTipoBloqueio.SelectedIndex, idsProdutosBloqueio, TextBoxMotivo.Text, Sessao.UsuarioLogado.IDUsuario);

            CarregaBloqueios();
            EntraEmModoInsercao(false);

            if (idsProdutosBloqueio == null && ProdutosBloqueadosAnteriormente.Any()) PageMaster.ExibeMensagem(ResourceMensagens.MensagemSucessoSalvarBloqueiosAplicacaoCompletaRemocaoAnterior);
            else if (idsProdutosBloqueio != null && jaExisteBloqueioCompleto) PageMaster.ExibeMensagem(ResourceMensagens.MensagemSucessoJaExisteBloqueioCompleto);
            else if (idsProdutosBloqueio != null && idsProdutosBloqueio.Any(x => ProdutosBloqueadosAnteriormente.Contains(x))) PageMaster.ExibeMensagem(ResourceMensagens.MensagemSucessoSalvarBloqueiosAplicacaoParcialComRepeticao);
            else PageMaster.ExibeMensagem(ResourceMensagens.MensagemSucessoOperacao);

            LimpaCampos();

        }

        protected void ASPxButtonRemoverBloqueio_Click(object sender, EventArgs e)
        {

            FachadaBloqueioUsuario.RemoveBloqueio(Convert.ToInt32(((ASPxButton)sender).CommandArgument), Sessao.UsuarioLogado.IDUsuario);

            CarregaBloqueios();

            PageMaster.ExibeMensagem(ResourceMensagens.MensagemSucessoOperacao);

        }

        protected void ButtonRemoverTodos_Click(object sender, EventArgs e)
        {
            FachadaBloqueioUsuario.RemoverBloqueios(Id.Value);
            PageMaster.ExibeMensagem(ResourceMensagens.MensagemSucessoOperacao);
        }

    }

}