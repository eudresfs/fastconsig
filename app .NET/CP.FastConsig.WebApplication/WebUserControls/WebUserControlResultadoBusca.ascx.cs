using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;
using CP.FastConsig.Common;
using CP.FastConsig.DAL;
using CP.FastConsig.Facade;
using CP.FastConsig.Util;
using CP.FastConsig.WebApplication.Auxiliar;

namespace CP.FastConsig.WebApplication.WebUserControls
{

    public partial class WebUserControlResultadoBusca : CustomUserControl
    {

        #region Constantes
        
        private const string ParametroResultadoBuscaUsuarios = "ResultadoBuscaUsuarios";
        private const string ControleImagemFloat = "ImagemFloat";
        private const string ParametroRealizaBusca = "RealizaBusca";
        private const string ParametrosResultadoBuscaAverbacaos = "ResultadoBuscaAverbacaos";
        private const string AtributoOnclick = "onclick";

        #endregion

        private bool RealizaBusca
        {

            get
            {
                if (Session[ParametroRealizaBusca] == null) Session[ParametroRealizaBusca] = false;
                return (bool)Session[ParametroRealizaBusca];
            }
            set
            {
                Session[ParametroRealizaBusca] = value;
            }

        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (RealizaBusca) ExibeResultados();
        }

        private void ExibeResultados()
        {

            ConfiguraResultadoFuncionarios();
            ConfiguraResultadoAverbacaos();

            RealizaBusca = false;
            
        }

        private void ConfiguraResultadoAverbacaos()
        {

            GridViewResultadoBuscaAverbacaos.DataSource = ResultadoBuscaAverbacaos;
            GridViewResultadoBuscaAverbacaos.DataBind();

            if (!ResultadoBuscaAverbacaos.Any()) GridViewResultadoBuscaAverbacaos.Visible = false;

        }

        private void ConfiguraResultadoFuncionarios()
        {

            GridViewResultadoBuscaFuncionarios.DataSource = ResultadoBuscaUsuarios;
            GridViewResultadoBuscaFuncionarios.DataBind();

            if (!ResultadoBuscaUsuarios.Any()) GridViewResultadoBuscaFuncionarios.Visible = false;

        }

        public void ConfiguraResultadoBusca(string busca)
        {

            ResultadoBuscaUsuarios = FachadaResultadoBusca.ObtemResultadosPesquisa(busca, 4);
            ResultadoBuscaAverbacaos = FachadaResultadoBusca.ObtemAverbacaosPesquisa(busca, 4);

            RealizaBusca = true;

        }

        private List<ResultadoBusca> ResultadoBuscaUsuarios
        {
            get
            {
                if (Session[ParametroResultadoBuscaUsuarios] == null) Session[ParametroResultadoBuscaUsuarios] = new List<ResultadoBusca>();
                return (List<ResultadoBusca>)Session[ParametroResultadoBuscaUsuarios];
            }
            set
            {
                Session[ParametroResultadoBuscaUsuarios] = value;
            }
        }

        private List<Averbacao> ResultadoBuscaAverbacaos
        {
            get
            {
                if (Session[ParametrosResultadoBuscaAverbacaos] == null) Session[ParametrosResultadoBuscaAverbacaos] = new List<Averbacao>();
                return (List<Averbacao>)Session[ParametrosResultadoBuscaAverbacaos];
            }
            set
            {
                Session[ParametrosResultadoBuscaAverbacaos] = value;
            }
        }

        protected void GridViewResultadoBuscaFuncionarios_RowDataBound(object sender, GridViewRowEventArgs e)
        {

            GridViewRow linha = e.Row;

            if (linha.RowType == DataControlRowType.DataRow)
            {

                Image imagem = (Image)linha.FindControl(ControleImagemFloat);
                ResultadoBusca usuario = (ResultadoBusca)linha.DataItem;

                string pathImagemPerfil = Util.ObtemPathImagemPerfil(usuario.IDUsuario, Sessao.PastaUpload);
                if (pathImagemPerfil != null) imagem.ImageUrl = pathImagemPerfil;

                linha.Attributes.Add(AtributoOnclick, Page.ClientScript.GetPostBackEventReference(GridViewResultadoBuscaFuncionarios, string.Format("Select${0}", linha.RowIndex.ToString())));

            }

        }

        protected void GridViewResultadoBuscaAverbacaos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow) e.Row.Attributes.Add(AtributoOnclick, Page.ClientScript.GetPostBackEventReference(GridViewResultadoBuscaAverbacaos, string.Format("Select${0}", e.Row.RowIndex.ToString())));
        }

        protected void GridViewResultadoBuscaAverbacaos_SelectedIndexChanged(object sender, EventArgs e)
        {
            ConfiguraResultadoAverbacaos();

            if (GridViewResultadoBuscaAverbacaos.SelectedDataKey != null)
            {
                PageMaster.CarregaControle(ResourceAuxiliar.NomeWebUserControlGerenciarAverbacaoConsulta, 0, 1, Convert.ToInt32(GridViewResultadoBuscaAverbacaos.SelectedDataKey.Value));
                PageMaster.EscondeBusca();
            }
            else
                FachadaMaster.RegistrarErro(Request, "Prevenção de Erro --> Ocorreu alguma falha, pois o SelectedDataKey estava nulo");

        }

        protected void GridViewResultadoBuscaFuncionarios_SelectedIndexChanged(object sender, EventArgs e)
        {

            ConfiguraResultadoFuncionarios();

            if (GridViewResultadoBuscaFuncionarios.SelectedDataKey != null)
            {
                int idUsuario = Convert.ToInt32(GridViewResultadoBuscaFuncionarios.SelectedDataKey.Value);
                int idFuncionario = FachadaResultadoBusca.ObtemIdsFuncionarios(idUsuario).FirstOrDefault();

                PageMaster.CarregaControle(ResourceAuxiliar.NomeWebUserControlFuncionariosConsulta, FachadaMaster.ObtemRecursoPorNome(ResourceAuxiliar.NomeWebUserControlFuncionarios, Sessao.IdModulo), 1, idFuncionario);
                PageMaster.EscondeBusca();
            }
            else
                FachadaMaster.RegistrarErro(Request, "Prevenção de Erro --> Ocorreu alguma falha, pois o SelectedDataKey estava nulo");

        }

    }

}