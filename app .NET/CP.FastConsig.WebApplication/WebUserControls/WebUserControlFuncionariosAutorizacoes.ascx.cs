using System;
using System.Web.UI.WebControls;
using CP.FastConsig.Common;
using CP.FastConsig.Facade;
using CP.FastConsig.WebApplication.Auxiliar;
using CP.FastConsig.DAL;


namespace CP.FastConsig.WebApplication.WebUserControls
{

    public partial class WebUserControlFuncionariosAutorizacoes : CustomUserControl
    {

        #region Constantes

        private const string ParametroIdFuncionarioEmEdicao = "IdFuncionarioEmEdicao";
        private const string ParametroDirecaoOrdenacao = "DirecaoOrdenacao";

        #endregion

        private int IdFunc
        {
            get
            {
                if (ViewState[ParametroIdFuncionarioEmEdicao] == null) ViewState[ParametroIdFuncionarioEmEdicao] = 0;
                return (int)ViewState[ParametroIdFuncionarioEmEdicao];
            }
            set
            {
                ViewState[ParametroIdFuncionarioEmEdicao] = value;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {

            if (EhPostBack || ControleCarregado) return;

            IdFunc = Id.Value;
            EhPostBack = true;

            PopulaDadosFunc();

            if (!FachadaFuncionariosAutorizacoes.ExisteAutorizacoes(IdFunc)) PageMaster.CarregaControle(ResourceAuxiliar.NomeWebUserControlFuncionariosAutorizacoesEdicao, FachadaMaster.ObtemRecursoPorNome(ResourceAuxiliar.NomeWebUserControlFuncionarios, Sessao.IdModulo), 26, 0, IdFunc);

        }

        private void PopulaDadosFunc()
        {
            Funcionario func = FachadaFuncionariosConsulta.ObtemFuncionario(IdFunc);

            LabelMatriculaFuncionario.Text = func.Matricula;
            LabelNomeFuncionario.Text = func.Pessoa.Nome;
            LabelCpfFuncionario.Text = func.Pessoa.CPFMascara;
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
            PageMaster.CarregaControle(ResourceAuxiliar.NomeWebUserControlFuncionariosAutorizacoesEdicao, FachadaMaster.ObtemRecursoPorNome(ResourceAuxiliar.NomeWebUserControlFuncionarios, Sessao.IdModulo), 26, 0, IdFunc);
        }
        
        protected void Remover_Click(object sender, EventArgs e)
        {

            LinkButton linkButtonRemover = (LinkButton)sender;

            FachadaFuncionariosAutorizacoes.RemoveAutorizacao(Convert.ToInt32(linkButtonRemover.CommandArgument));

            PopulaGrid();

            PageMaster.ExibeMensagem(ResourceMensagens.MensagemSucessoOperacao);

        }

        protected void Grid_Sorting(object sender, GridViewSortEventArgs e)
        {

            switch (DirecaoOrdenacao)
            {

                case (SortDirection.Ascending):

                    DirecaoOrdenacao = SortDirection.Descending;
                    PopulaGrid();

                    break;

                case (SortDirection.Descending):

                    DirecaoOrdenacao = SortDirection.Ascending;
                    PopulaGrid();

                    break;

            }

        }

        private void PopulaGrid()
        {
            grid.DataBind();
        }

        protected void ButtonPesquisar_Click(object sender, EventArgs e)
        {
            PopulaGrid();
        }

        protected void grid_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grid.PageIndex = e.NewPageIndex;
        }

        protected void grid_RowDataBound(object sender, GridViewRowEventArgs e)
        {

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Attributes.Add("onclick",Page.ClientScript.GetPostBackEventReference(grid, "Select$" +
                e.Row.RowIndex.ToString()));
                e.Row.Style.Add("cursor", "pointer");
            }

        }

        protected void grid_SelectedIndexChanged(object sender, EventArgs e)
        {
            int id = Convert.ToInt32(grid.SelectedDataKey.Value);
            PageMaster.CarregaControle("WebUserControlFuncionarios_Autorizacoes_Edicao", FachadaMaster.ObtemRecursoPorNome(ResourceAuxiliar.NomeWebUserControlFuncionarios, Sessao.IdModulo), 26, id, IdFunc);
        }
        
        protected void ODS_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
        {
            e.InputParameters[0] = IdFunc;
        }

        public void Atualizar()
        {
            PopulaGrid();
        }

    }

}