using System;
using System.Web.UI.WebControls;
using CP.FastConsig.Common;
using CP.FastConsig.Facade;
using CP.FastConsig.WebApplication.Auxiliar;

namespace CP.FastConsig.WebApplication.WebUserControls
{

    public partial class WebUserControlConsignatarias : CustomUserControl
    {

        #region Constantes

        private const string ParametroModoInsercao = "ModoInsercao";
        private const string ParametroIdEmpresaEmEdicao = "IdEmpresaEmEdicao";
        private const string ParametroDirecaoOrdenacao = "DirecaoOrdenacao";

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
        
        protected void Page_Load(object sender, EventArgs e)
        {
            
            if (EhPostBack || ControleCarregado) return;

            EhPostBack = true;

            if (Sessao.IdModulo == (int)Enums.Modulos.Consignataria) PageMaster.CarregaControle(ResourceAuxiliar.NomeWebUserControlConsignatariasEdicao, this.IdRecurso, 1, Sessao.IdBanco);
            else PopulaGrid();

        }

        public void ConfiguraTela()
        {
            if (Sessao.IdModulo == (int)Enums.Modulos.Consignante) PopulaGrid();
        }


        private void PopulaGrid(int pagina = 0)
        {
            grid.DataBind();
        }


        protected void ImageButtonRemover_Click(object sender, EventArgs e)
        {

            LinkButton linkButtonRemover = (LinkButton)sender;

            FachadaConsignatarias.RemoveEmpresa(Convert.ToInt32(linkButtonRemover.CommandArgument));

            PopulaGrid(grid.PageIndex);

            PageMaster.ExibeMensagem(ResourceMensagens.MensagemSucessoOperacao);

        }

        protected void grid_Sorting(object sender, GridViewSortEventArgs e)
        {

            switch (DirecaoOrdenacao)
            {

                case (SortDirection.Ascending):

                    DirecaoOrdenacao = SortDirection.Descending;
                    PopulaGrid(grid.PageIndex);

                    break;

                case (SortDirection.Descending):

                    DirecaoOrdenacao = SortDirection.Ascending;
                    PopulaGrid(grid.PageIndex);

                    break;

            }

        }

        protected void grid_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            
        }



        protected void ButtonPesquisar_Click(object sender, EventArgs e)
        {

            
        }

        protected void ButtonNovo_Click(object sender, EventArgs e)
        {
            PageMaster.CarregaControle(ResourceAuxiliar.NomeWebUserControlConsignatariasEdicao, this.IdRecurso, 1);
        }

        //protected void grid_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    int id = Convert.ToInt32(grid.SelectedDataKey.Value);

        //    PageMaster.CarregaControle(ResourceAuxiliar.NomeWebUserControlConsignatariasEdicao, this.IdRecurso, 1, id);
        //}

        protected void ImageButtonEditar_Click(object sender, EventArgs e)
        {
            LinkButton linkButtonEditar = (LinkButton)sender;

            int id = Convert.ToInt32(linkButtonEditar.CommandArgument);

            PageMaster.CarregaControle(ResourceAuxiliar.NomeWebUserControlConsignatariasEdicao, IdRecurso, 1, id);
        }

        protected void grid_DataBound(Object sender, EventArgs e)
        {

            GridViewRow gvrPager = grid.BottomPagerRow;

            if (gvrPager == null) return;

            DropDownList DropDownPagina = (DropDownList)gvrPager.Cells[0].FindControl("DropDownPagina");
            Label LabelPaginas = (Label)gvrPager.Cells[0].FindControl("LabelPaginas");

            if (DropDownPagina != null)
            {
                // Popula Paginador
                for (int i = 0; i < grid.PageCount; i++)
                {

                    int intPageNumber = i + 1;
                    ListItem lstItem = new ListItem(intPageNumber.ToString());

                    if (i == grid.PageIndex)
                        lstItem.Selected = true;

                    DropDownPagina.Items.Add(lstItem);
                }
            }

            // Popula Contador de Páginas
            if (LabelPaginas != null)
                LabelPaginas.Text = grid.PageCount.ToString();
        
        
        
        }

        protected void grid_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (((Label)e.Row.Cells[6].Controls[1]).Text != "Ativo")
                    e.Row.Cells[6].ForeColor = System.Drawing.Color.Red;
            }
        }

        protected void dropwdown_SelectedIndexChangend(Object sender, EventArgs e)
        {
            GridViewRow gvrPager = grid.BottomPagerRow;
            DropDownList DropDownPagina = (DropDownList)gvrPager.Cells[0].FindControl("DropDownPagina");

            grid.PageIndex = DropDownPagina.SelectedIndex;

            // Popular o Grid
            grid.DataBind();

        }

        //protected void gridConsignatarias_RowDataBound(object sender, GridViewRowEventArgs e)
        //{

        //    if (e.Row.RowType == DataControlRowType.DataRow)
        //    {
        //        e.Row.Attributes.Add("onclick", Page.ClientScript.GetPostBackEventReference(grid, "Select$" +e.Row.RowIndex.ToString()));
        //        //e.Row.Style.Add("cursor", "pointer");
        //    }

        //}

    }

}