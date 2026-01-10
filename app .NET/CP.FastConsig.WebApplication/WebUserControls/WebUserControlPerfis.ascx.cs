using System;
using System.Web.UI.WebControls;
using CP.FastConsig.Common;
using CP.FastConsig.Facade;
using CP.FastConsig.WebApplication.Auxiliar;
using System.Linq;


namespace CP.FastConsig.WebApplication.WebUserControls
{

    public partial class WebUserControlPerfis : CustomUserControl
    {

        #region Constantes

        private const string ParametroIdEmpresaEmEdicao = "IdEmpresaEmEdicao";
        private const string ParametroDirecaoOrdenacao = "DirecaoOrdenacao";
        #endregion

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

        protected void Page_Load(object sender, EventArgs e)
        {

            if (EhPostBack || ControleCarregado) return;

            EhPostBack = true;
            PopularCombos();
            PopulaGrid();

        }

        private void PopularCombos()
        {

            DropDownListModulo.DataSource = FachadaPermissoesAcesso.ListarModulos().Where(x => x.IDModulo != (int)Enums.Modulos.Agente && x.IDModulo != (int)Enums.Modulos.Funcionario).ToList();
            DropDownListConsignataria.DataSource = FachadaConciliacao.ListarConsignatarias().ToList();
            DropDownListConsignataria.DataBind();
            DropDownListModulo.DataBind();

            DropDownListConsignataria.Items.Insert(0, new ListItem(ResourceMensagens.LabelSelecione, "0"));
            DropDownListConsignataria.SelectedValue = Sessao.IdBanco.ToString();

            DropDownListModulo.SelectedValue = Sessao.IdModulo.ToString();
            DropDownListModulo.Visible = Sessao.IdModulo == (int)Enums.Modulos.Consignante;
            LabelModulo.Visible = DropDownListModulo.Visible;
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

        protected void ButtonNovo_Click(object sender, EventArgs e)
        {
            PageMaster.CarregaControle(ResourceAuxiliar.NomeWebUserControlPerfisEdicao, this.IdRecurso, 1, 0, DropDownListModulo.SelectedValue);
        }

        protected void PerfisRemover_Click(object sender, EventArgs e)
        {

            LinkButton linkButtonRemover = (LinkButton)sender;

            FachadaPerfis.RemovePerfil(Convert.ToInt32(linkButtonRemover.CommandArgument));

            PopulaGrid();

            PageMaster.ExibeMensagem(ResourceMensagens.MensagemSucessoOperacao);

        }

        protected void grid_Sorting(object sender, GridViewSortEventArgs e)
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
                e.Row.Attributes.Add("onclick", Page.ClientScript.GetPostBackEventReference(grid, "Select$" +
                e.Row.RowIndex.ToString()));
                e.Row.Style.Add("cursor", "pointer");
            }

        }

        protected void ImageButtonEditar_Click(object sender, EventArgs e)
        {
            LinkButton linkButtonEditar = (LinkButton)sender;

            int id = Convert.ToInt32(linkButtonEditar.CommandArgument);

            PageMaster.CarregaControle(ResourceAuxiliar.NomeWebUserControlPerfisEdicao, this.IdRecurso, 1, id, IdEmpresa);
        }

        protected void grid_SelectedIndexChanged(object sender, EventArgs e)
        {
            int id = Convert.ToInt32(grid.SelectedDataKey.Value);
            PageMaster.CarregaControle(ResourceAuxiliar.NomeWebUserControlPerfisEdicao, this.IdRecurso, 1, id, IdEmpresa);
        }

        protected void ODS_Perfis_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
        {
            e.InputParameters[0] = Convert.ToInt32(DropDownListModulo.SelectedValue) == (int)Enums.Modulos.Consignataria ? Convert.ToInt32(DropDownListConsignataria.SelectedValue) : Sessao.IdBanco;
            e.InputParameters[1] = Convert.ToInt32(DropDownListModulo.SelectedValue);
        }

        protected void DropDownListConsignataria_SelectedIndexChanged(object sender, EventArgs e)
        {
            grid.DataBind();
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

            grid.DataBind();
        }

        public void AtualizarGrid()
        {
            PopulaGrid();
        }
    }

}