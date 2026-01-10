using System;
using System.Web.UI.WebControls;
using CP.FastConsig.Common;
using CP.FastConsig.Facade;
using CP.FastConsig.WebApplication.Auxiliar;

namespace CP.FastConsig.WebApplication.WebUserControls
{

    public partial class WebUserControlAgentes : CustomUserControl
    {

        #region Constantes

        private const string ParametroDirecaoOrdenacao = "DirecaoOrdenacao";
        private const string ControleDropDownPagina = "DropDownPagina";
        private const string ControleLabelPaginas = "LabelPaginas";
        private const string ParametroIdConsignataria = "idConsignataria";
        private const string ParametroCadastroAgente = "CadastroAgente";

        #endregion

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

        private bool CadastroAgente
        {
            set { Session[ParametroCadastroAgente] = value; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            
            if (EhPostBack || ControleCarregado) return;

            CadastroAgente = true;

            EhPostBack = true;

            PopulaGrid();
        
        }

        private void PopulaGrid()
        {
            grid.DataBind();
        }

        protected void ImageButtonRemover_Click(object sender, EventArgs e)
        {

            LinkButton linkButtonRemover = (LinkButton)sender;

            FachadaConsignatarias.RemoveEmpresa(Convert.ToInt32(linkButtonRemover.CommandArgument));

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

        protected void grid_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            
        }

        protected void ButtonNovo_Click(object sender, EventArgs e)
        {
            PageMaster.CarregaControle(ResourceAuxiliar.NomeWebUserControlAgentesEdicao, 0, 1);
        }

        protected void ImageButtonEditar_Click(object sender, EventArgs e)
        {

            LinkButton linkButtonEditar = (LinkButton)sender;

            int id = Convert.ToInt32(linkButtonEditar.CommandArgument);

            PageMaster.CarregaControle(ResourceAuxiliar.NomeWebUserControlAgentesEdicao, 0, 1, id);

        }

        protected void grid_DataBound(Object sender, EventArgs e)
        {

            GridViewRow gvrPager = grid.BottomPagerRow;

            if (gvrPager == null) return;

            DropDownList dropDownPagina = (DropDownList)gvrPager.Cells[0].FindControl(ControleDropDownPagina);
            Label labelPaginas = (Label)gvrPager.Cells[0].FindControl(ControleLabelPaginas);

            if (dropDownPagina != null)
            {
                
                for (int i = 0; i < grid.PageCount; i++)
                {

                    int intPageNumber = i + 1;
                    ListItem lstItem = new ListItem(intPageNumber.ToString());

                    if (i == grid.PageIndex) lstItem.Selected = true;

                    dropDownPagina.Items.Add(lstItem);

                }

            }

            if (labelPaginas != null) labelPaginas.Text = grid.PageCount.ToString();
        
        }

        protected void dropwdown_SelectedIndexChangend(Object sender, EventArgs e)
        {

            GridViewRow gvrPager = grid.BottomPagerRow;
            DropDownList dropDownPagina = (DropDownList)gvrPager.Cells[0].FindControl(ControleDropDownPagina);

            grid.PageIndex = dropDownPagina.SelectedIndex;
            grid.DataBind();

        }

        protected void ODS_Consignatarias_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
        {
            e.InputParameters[ParametroIdConsignataria] = Sessao.IdModulo.Equals((int)Enums.Modulos.Consignante) ? 0 : Sessao.IdBanco;
        }

    }

}