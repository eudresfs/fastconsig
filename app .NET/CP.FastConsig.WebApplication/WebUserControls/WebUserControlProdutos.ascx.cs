using System;
using System.Web.UI.WebControls;
using CP.FastConsig.Common;
using CP.FastConsig.Facade;
using CP.FastConsig.WebApplication.Auxiliar;

namespace CP.FastConsig.WebApplication.WebUserControls
{

    public partial class WebUserControlProdutos : CustomUserControl
    {

        #region Constantes

        private const string ParametroIdEmpresaEmEdicao = "IdEmpresaEmEdicao";
        private const string ParametroDirecaoOrdenacao = "DirecaoOrdenacao";
        private const string ParametroExibirTitulo = "ExibirTitulo";
        private const string AtributoOnclick = "onclick";
        private const string FlagSelect = "Select${0}";
        private const string EstiloCursor = "cursor";
        private const string OpcaoCursorPointer = "pointer";

        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            ButtonNovoProduto.Visible = (Sessao.IdModulo == (int)Enums.Modulos.Consignante); 
            gridProdutos.Columns[0].Visible = (Sessao.IdModulo == (int)Enums.Modulos.Consignante);
            gridProdutos.Columns[1].Visible = (Sessao.IdModulo == (int)Enums.Modulos.Consignante);

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

        public void AtualizaProdutos()
        {
            if (IdEmpresa == 0) return;
            PopulaGridProdutos();
        }

        private SortDirection DirecaoOrdenacao
        {
            get
            {
                if (ViewState[ParametroDirecaoOrdenacao] == null) ViewState[ParametroDirecaoOrdenacao] = SortDirection.Ascending;
                return (SortDirection)ViewState[ParametroDirecaoOrdenacao];
            }
            set
            {
                ViewState[ParametroDirecaoOrdenacao] = value;
            }
        }

        protected void ButtonNovoProduto_Click(object sender, EventArgs e)
        {
            PageMaster.CarregaControle(ResourceAuxiliar.NomeWebUserControlProdutosEdicao, this.IdRecurso, 1, 0, IdEmpresa);
        }

        protected void ImageButtonEditar_Click(object sender, EventArgs e)
        {
            LinkButton linkButtonEditar = (LinkButton)sender;

            int id = Convert.ToInt32(linkButtonEditar.CommandArgument);

            PageMaster.CarregaControle(ResourceAuxiliar.NomeWebUserControlProdutosEdicao, this.IdRecurso, 1, id, IdEmpresa);

        }

        protected void ProdutosRemover_Click(object sender, EventArgs e)
        {

            LinkButton linkButtonRemover = (LinkButton)sender;

            FachadaProdutos.RemoveProduto(Convert.ToInt32(linkButtonRemover.CommandArgument));

            PopulaGridProdutos();

            PageMaster.ExibeMensagem(ResourceMensagens.MensagemSucessoOperacao);

        }

        protected void gridProdutos_Sorting(object sender, GridViewSortEventArgs e)
        {

            switch (DirecaoOrdenacao)
            {

                case (SortDirection.Ascending):

                    DirecaoOrdenacao = SortDirection.Descending;
                    PopulaGridProdutos();

                    break;

                case (SortDirection.Descending):

                    DirecaoOrdenacao = SortDirection.Ascending;
                    PopulaGridProdutos();

                    break;

            }

        }

        private void PopulaGridProdutos()
        {
            gridProdutos.DataBind();
        }

        protected void ButtonPesquisar_Click(object sender, EventArgs e)
        {
            PopulaGridProdutos();
        }

        protected void grid_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gridProdutos.PageIndex = e.NewPageIndex;
        }

        protected void gridProdutos_RowDataBound(object sender, GridViewRowEventArgs e)
        {

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Attributes.Add(AtributoOnclick,Page.ClientScript.GetPostBackEventReference(gridProdutos, string.Format(FlagSelect, e.Row.RowIndex.ToString())));
                e.Row.Style.Add(EstiloCursor, OpcaoCursorPointer);
            }

        }

        protected void gridProdutos_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Sessao.IdModulo == (int)(Enums.Modulos.Consignante))
            {
                int id = Convert.ToInt32(gridProdutos.SelectedDataKey.Value);
                PageMaster.CarregaControle(ResourceAuxiliar.NomeWebUserControlProdutosEdicao, this.IdRecurso, 1, id, IdEmpresa);
            }
        }

        protected void ODS_Produtos_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
        {
            e.InputParameters[0] = IdEmpresa;
        }

    }

}