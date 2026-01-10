using System;
using System.Web.UI.WebControls;
using CP.FastConsig.Common;
using CP.FastConsig.DAL;
using CP.FastConsig.Facade;
using CP.FastConsig.WebApplication.Auxiliar;


namespace CP.FastConsig.WebApplication.WebUserControls
{

    public partial class WebUserControlEmpresaSuspensoes : CustomUserControl
    {

        #region Constantes

        private const string ParametroIdEmpresaEmEdicao = "IdEmpresaEmEdicao";
        private const string ParametroDirecaoOrdenacao = "DirecaoOrdenacao";
        private const string ParametroExibirTitulo = "ExibirTitulo";
        private const string TituloPagina = "Suspensão da Consignatária";
        #endregion

        public override void ConfiguraTopo()
        {
            PageMaster.Titulo = TituloPagina;
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


        public void AtualizaSuspensoes()
        {

            if (IdEmpresa == 0) return;

            EmpresaSuspensao ultimaSuspensao = FachadaSuspensoes.ObtemUltimaSuspensao(IdEmpresa);

            if (ultimaSuspensao != null && ultimaSuspensao.IDEmpresaSituacaoSuspensao.Equals((int)Enums.EmpresaSituacao.Bloqueado) && ultimaSuspensao.TipoPeriodo.Equals(Enums.BloqueioPeriodo.I.ToString())) ButtonNovo.Visible = false;

            PopulaGridSuspensoes();

        }

        protected void Page_Load(object sender, EventArgs e)
        {

            if (EhPostBack || ControleCarregado) return;

            EhPostBack = true;
            AtualizaSuspensoes();

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
            PageMaster.CarregaControle(ResourceAuxiliar.NomeWebUserControlSuspensoesEdicao, this.IdRecurso, 1, 0, IdEmpresa);
        }

        protected void SuspensoesRemover_Click(object sender, EventArgs e)
        {

            LinkButton linkButtonRemover = (LinkButton)sender;

            FachadaSuspensoes.RemoveSuspensao(Convert.ToInt32(linkButtonRemover.CommandArgument));

            AtualizaSuspensoes();

            PageMaster.ExibeMensagem(ResourceMensagens.MensagemSucessoOperacao);

        }

        protected void gridSuspensoes_Sorting(object sender, GridViewSortEventArgs e)
        {

            switch (DirecaoOrdenacao)
            {

                case (SortDirection.Ascending):

                    DirecaoOrdenacao = SortDirection.Descending;
                    PopulaGridSuspensoes();

                    break;

                case (SortDirection.Descending):

                    DirecaoOrdenacao = SortDirection.Ascending;
                    PopulaGridSuspensoes();

                    break;

            }

        }

        private void PopulaGridSuspensoes()
        {
            gridSuspensoes.DataBind();
        }

        protected void ButtonPesquisar_Click(object sender, EventArgs e)
        {
            PopulaGridSuspensoes();
        }

        protected void grid_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gridSuspensoes.PageIndex = e.NewPageIndex;
        }

        protected void gridSuspensoes_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Attributes.Add("onclick", Page.ClientScript.GetPostBackEventReference(gridSuspensoes, "Select$" +
                e.Row.RowIndex.ToString()));
                e.Row.Style.Add("cursor", "pointer");
            }

        }

        protected void ImageButtonEditar_Click(object sender, EventArgs e)
        {
            LinkButton linkButtonEditar = (LinkButton)sender;

            int id = Convert.ToInt32(linkButtonEditar.CommandArgument);

            PageMaster.CarregaControle(ResourceAuxiliar.NomeWebUserControlSuspensoesEdicao, this.IdRecurso, 1, id, IdEmpresa);

        }

        protected void gridSuspensoes_SelectedIndexChanged(object sender, EventArgs e)
        {
            int id = Convert.ToInt32(gridSuspensoes.SelectedDataKey.Value);
            PageMaster.CarregaControle(ResourceAuxiliar.NomeWebUserControlSuspensoesEdicao, this.IdRecurso, 1, id, IdEmpresa);
        }

        protected void ODS_Suspensoes_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
        {
            e.InputParameters[0] = IdEmpresa;
        }

    }

}