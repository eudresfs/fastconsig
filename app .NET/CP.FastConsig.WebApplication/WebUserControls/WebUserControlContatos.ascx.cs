using System;
using System.Web.UI.WebControls;
using CP.FastConsig.Common;
using CP.FastConsig.Facade;
using CP.FastConsig.WebApplication.Auxiliar;


namespace CP.FastConsig.WebApplication.WebUserControls
{

    public partial class WebUserControlContatos : CustomUserControl
    {

        #region Constantes

        private const string ParametroIdContatoEmEdicao = "IdContatoEmEdicao";
        private const string ParametroDirecaoOrdenacao = "DirecaoOrdenacao";
        private const string ParametroExibirTitulo = "ExibirTitulo";
        #endregion

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
                if (ViewState[ParametroIdContatoEmEdicao] == null) ViewState[ParametroIdContatoEmEdicao] = 0;
                return (int)ViewState[ParametroIdContatoEmEdicao];
            }
            set
            {
                ViewState[ParametroIdContatoEmEdicao] = value;
            }
        }


        public void AtualizaContatos()
        {
            if (IdEmpresa == 0) return;
            PopulaGridContatos();
        }

        protected void Page_Load(object sender, EventArgs e)
        {

            if (EhPostBack || ControleCarregado) return;

            EhPostBack = true;
            AtualizaContatos();

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

        protected void ButtonNovoContato_Click(object sender, EventArgs e)
        {
            PageMaster.CarregaControle(ResourceAuxiliar.NomeWebUserControlContatosEdicao, 280, 1, 0, IdEmpresa);
        }

        protected void ContatosRemover_Click(object sender, EventArgs e)
        {

            LinkButton linkButtonRemover = (LinkButton)sender;

            FachadaContatos.RemoveContato(Convert.ToInt32(linkButtonRemover.CommandArgument));

            PopulaGridContatos();

            PageMaster.ExibeMensagem(ResourceMensagens.MensagemSucessoOperacao);

        }

        protected void gridContatos_Sorting(object sender, GridViewSortEventArgs e)
        {

            switch (DirecaoOrdenacao)
            {

                case (SortDirection.Ascending):

                    DirecaoOrdenacao = SortDirection.Descending;
                    PopulaGridContatos();

                    break;

                case (SortDirection.Descending):

                    DirecaoOrdenacao = SortDirection.Ascending;
                    PopulaGridContatos();

                    break;

            }

        }

        private void PopulaGridContatos()
        {
            gridContatos.DataBind();
        }

        protected void ButtonPesquisar_Click(object sender, EventArgs e)
        {
            PopulaGridContatos();
        }

        protected void grid_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gridContatos.PageIndex = e.NewPageIndex;
        }

        protected void gridContatos_RowDataBound(object sender, GridViewRowEventArgs e)
        {

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Attributes.Add("onclick", Page.ClientScript.GetPostBackEventReference(gridContatos, "Select$" +
                e.Row.RowIndex.ToString()));
                e.Row.Style.Add("cursor", "pointer");
            }

        }

        protected void ImageButtonEditar_Click(object sender, EventArgs e)
        {
            LinkButton linkButtonEditar = (LinkButton)sender;

            int id = Convert.ToInt32(linkButtonEditar.CommandArgument);

            PageMaster.CarregaControle(ResourceAuxiliar.NomeWebUserControlContatosEdicao, this.IdRecurso, 1, id, IdEmpresa);

        }

        protected void gridContatos_SelectedIndexChanged(object sender, EventArgs e)
        {
            //if (gridContatos.SelectedDataKey != null)
            //{
            //    int id = Convert.ToInt32(gridContatos.SelectedDataKey.Value);
            //    PageMaster.CarregaControle(ResourceAuxiliar.NomeWebUserControlContatosEdicao, this.IdRecurso, 1, id, IdEmpresa);
            //}
        }

        protected void ODS_Contatos_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
        {
            e.InputParameters[0] = IdEmpresa;
        }

    }

}