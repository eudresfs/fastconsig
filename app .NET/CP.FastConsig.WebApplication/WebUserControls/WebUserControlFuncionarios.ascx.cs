using System;
using CP.FastConsig.Common;
using CP.FastConsig.WebApplication.Auxiliar;
using System.Web.UI.WebControls;
using System.Linq;
using CP.FastConsig.DAL;

namespace CP.FastConsig.WebApplication.WebUserControls
{

    public partial class WebUserControlFuncionarios : CustomUserControl
    {

        protected void Page_Load(object sender, EventArgs e)
        {

            if (EhPostBack || ControleCarregado) return;

            GridViewListaFunc.DataBind();
            EhPostBack = true;
                TextoAncora.Visible = false;

        }
        
        protected void Buscar_Click(object sender, EventArgs e)
        {

            GridViewListaFunc.DataBind();

            TextoAncora.Visible = true;

        }

        protected void grid_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {

        }
        
        protected void GridViewListaFuncSelect_Click(object sender, EventArgs e)
        {
            int id = Convert.ToInt32(GridViewListaFunc.SelectedDataKey.Value);
            PageMaster.CarregaControle(ResourceAuxiliar.NomeWebUserControlFuncionariosConsulta, this.IdRecurso, 1, id);
        }

        protected void ODS_Funcionarios_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
        {
            if (string.IsNullOrEmpty(ASPxTextBoxBusca.Text))
            {
                if(EhPostBack) PageMaster.ExibeMensagem(ResourceMensagens.MensagemDigitarTextoPesquisa);
                e.Cancel = true;
            }
            e.InputParameters[0] = ASPxTextBoxBusca.Text;
            e.InputParameters[1] = Sessao.IdBanco;
        }

        protected void grid_DataBound(Object sender, EventArgs e)
        {

            GridViewRow gvrPager = GridViewListaFunc.BottomPagerRow;

            if (gvrPager == null) return;

            DropDownList DropDownPagina = (DropDownList)gvrPager.Cells[0].FindControl("DropDownPagina");
            Label LabelPaginas = (Label)gvrPager.Cells[0].FindControl("LabelPaginas");
            Label LabelCountReg = (Label)gvrPager.Cells[0].FindControl("LabelCountReg");
            Label LabelRegistros = (Label)gvrPager.Cells[0].FindControl("LabelRegistros");
            LabelRegistros.Text = " (" + new CP.FastConsig.BLL.ODS_Funcionario().SelectGridCount(ASPxTextBoxBusca.Text,Sessao.IdBanco).ToString() + " registros)";

            if (DropDownPagina != null)
            {
                // Popula Paginador
                for (int i = 0; i < GridViewListaFunc.PageCount; i++)
                {

                    int intPageNumber = i + 1;
                    ListItem lstItem = new ListItem(intPageNumber.ToString());

                    if (i == GridViewListaFunc.PageIndex)
                        lstItem.Selected = true;

                    DropDownPagina.Items.Add(lstItem);
                }
            }

            // Popula Contador de Páginas
            if (LabelPaginas != null)
                LabelPaginas.Text = GridViewListaFunc.PageCount.ToString();

            if (LabelCountReg != null)
            {
                //LabelCountReg.Text = String.Format("({0}) item(s)", 0);
            }

            if (GridViewListaFunc.Rows.Count.Equals(0)) PageMaster.ExibeMensagem(ResourceMensagens.MensagemNaoExiste);

        }

        protected void dropwdown_SelectedIndexChangend(Object sender, EventArgs e)
        {
            GridViewRow gvrPager = GridViewListaFunc.BottomPagerRow;
            DropDownList DropDownPagina = (DropDownList)gvrPager.Cells[0].FindControl("DropDownPagina");

            GridViewListaFunc.PageIndex = DropDownPagina.SelectedIndex;

            // Metodo para popular o Grid
            GridViewListaFunc.DataBind();
            TextoAncora.Visible = true;

        }

        protected void ODS_Funcionarios_Selected(object sender, ObjectDataSourceStatusEventArgs e)
        {
            IQueryable<Funcionario> results = e.ReturnValue as IQueryable<Funcionario>;
            bool bNaoExiste = results != null && results.Count() == 0;
            if (bNaoExiste)
                PageMaster.ExibeMensagem(ResourceMensagens.MensagemNaoExiste);
        }

    }

}