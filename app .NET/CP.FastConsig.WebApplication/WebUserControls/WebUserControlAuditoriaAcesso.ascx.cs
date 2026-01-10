using System;
using CP.FastConsig.Common;
using CP.FastConsig.WebApplication.Auxiliar;
using System.Web.UI.WebControls;
using System.Linq;
using CP.FastConsig.DAL;
using CP.FastConsig.Facade;

namespace CP.FastConsig.WebApplication.WebUserControls
{

    public partial class WebUserControlAuditoriaAcesso : CustomUserControl
    {

        protected void Page_Load(object sender, EventArgs e)
        {

            if (EhPostBack || ControleCarregado) return;

            PopularCombos();
            GridViewLista.DataBind();
            EhPostBack = true;
                TextoAncora.Visible = false;

        }

        private void PopularCombos()
        {

            DropDownListModulo.DataSource = FachadaPermissoesAcesso.ListarModulos().Where(x => x.IDModulo != (int)Enums.Modulos.Agente).ToList();
            DropDownListConsignataria.DataSource = FachadaConsignatarias.ListaConsignatarias().ToList();
            DropDownListConsignataria.DataBind();
            DropDownListModulo.DataBind();

            DropDownListConsignataria.SelectedValue = Sessao.IdBanco.ToString();

            DropDownListModulo.SelectedValue = Sessao.IdModulo.ToString();
            DropDownListModulo.Visible = Sessao.IdModulo == (int)Enums.Modulos.Consignante;
            LabelModulo.Visible = DropDownListModulo.Visible;
            btnBuscar.Visible = DropDownListModulo.Visible;
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
        
        protected void Buscar_Click(object sender, EventArgs e)
        {

            GridViewLista.DataBind();

            TextoAncora.Visible = true;

        }

        protected void grid_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {

        }
        
        protected void GridViewListaFuncSelect_Click(object sender, EventArgs e)
        {
        }

        protected void ODS_AuditoriaAcesso_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
        {           
            //if (string.IsNullOrEmpty(ASPxTextBoxBusca.Text))
            //{
            //    if(EhPostBack) PageMaster.ExibeMensagem(ResourceMensagens.MensagemDigitarTextoPesquisa);
            //    e.Cancel = true;
            //}
            e.InputParameters[0] = ASPxTextBoxBusca.Text;

            if (Convert.ToInt32(DropDownListModulo.SelectedValue) == (int)Enums.Modulos.Consignante)
                e.InputParameters[1] = Sessao.IdBanco;
            else
                e.InputParameters[1] = Convert.ToInt32(DropDownListConsignataria.SelectedValue);
        }

        protected void grid_DataBound(Object sender, EventArgs e)
        {

            GridViewRow gvrPager = GridViewLista.BottomPagerRow;

            if (gvrPager == null) return;

            DropDownList DropDownPagina = (DropDownList)gvrPager.Cells[0].FindControl("DropDownPagina");
            Label LabelPaginas = (Label)gvrPager.Cells[0].FindControl("LabelPaginas");

            if (DropDownPagina != null)
            {
                // Popula Paginador
                for (int i = 0; i < GridViewLista.PageCount; i++)
                {

                    int intPageNumber = i + 1;
                    ListItem lstItem = new ListItem(intPageNumber.ToString());

                    if (i == GridViewLista.PageIndex)
                        lstItem.Selected = true;

                    DropDownPagina.Items.Add(lstItem);
                }
            }

            // Popula Contador de Páginas
            if (LabelPaginas != null)
                LabelPaginas.Text = GridViewLista.PageCount.ToString();

            if (GridViewLista.Rows.Count.Equals(0)) PageMaster.ExibeMensagem(ResourceMensagens.MensagemNaoExiste);

        }

        protected void dropwdown_SelectedIndexChangend(Object sender, EventArgs e)
        {
            GridViewRow gvrPager = GridViewLista.BottomPagerRow;
            DropDownList DropDownPagina = (DropDownList)gvrPager.Cells[0].FindControl("DropDownPagina");

            GridViewLista.PageIndex = DropDownPagina.SelectedIndex;

            // Metodo para popular o Grid
            GridViewLista.DataBind();
            TextoAncora.Visible = true;

        }

        protected void ODS_AuditoriaAcesso_Selected(object sender, ObjectDataSourceStatusEventArgs e)
        {
            IQueryable<Funcionario> results = e.ReturnValue as IQueryable<Funcionario>;
            bool bNaoExiste = results != null && results.Count() == 0;
            if (bNaoExiste)
                PageMaster.ExibeMensagem(ResourceMensagens.MensagemNaoExiste);
        }

    }

}