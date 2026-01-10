using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using CP.FastConsig.Common;
using CP.FastConsig.Facade;
using CP.FastConsig.DAL;
using CP.FastConsig.WebApplication.Auxiliar;
using CP.FastConsig.WebApplication.FastConsigCenterService;
using DevExpress.Web.ASPxPopupControl;
using DevExpress.Web.ASPxTabControl;
using DevExpress.Web.ASPxEditors;
using Phantom;
using DevExpress.Web.ASPxGridView.Export;

namespace CP.FastConsig.WebApplication
{

    public partial class NewMasterPrincipal : MasterPage
    {

        #region Constantes

        private const string PathWebUserControls = "~/WebUserControls/{0}.ascx";
        private const string PathImagensIcone = "~/Imagens/{0}";
        private const string PathPaginaLogin = "~/Login";
        private const string TagBr = "<br />";
        private const string AtributoBackGround = "background";
        private const string EstiloBackgroundBotaoMenuNormal = "transparent url(/Imagens/Menu/MenuBgBtn.png) repeat-x;";
        private const string EstiloBackgroundBotaoMenuSelecionado = "transparent url(/Imagens/Menu/MenuBtnActive.png) repeat-x;";
        private const string PathImagensMenu = @"{0}Imagens/Menu/{1}";
        private const string IdControleMenuI = "menui";
        private const string PathImagensLogosCompletos = "~/Imagens/Logos/FULL_{0}";
        private const string AtributoValorPro = "ValorPro";
        private const string AtributoValorNeutro = "ValorNeutro";
        private const string AtributoValorContra = "ValorContra";
        private const string AtributoOnclick = "onclick";
        private const string MensagemErroComRedirecionamento1 = "THE STATUS CODE RETURNED FROM THE SERVER WAS";
        private const string MensagemErroComRedirecionamento2 = "COULD NOT LOAD FILE OR ASSEMBLY";
        private const string ParametroIndiceControleAtual = "IndiceControleAtual";

        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {

                Page.ClientScript.GetPostBackEventReference(this, string.Empty);

                DevExpress.Web.ASPxClasses.ASPxWebControl.RegisterBaseScript(Page);

                string nomerecurso = Sessao.IdModulo == (int)Enums.Modulos.Funcionario ? ResourceAuxiliar.NomeWebUserControlDashBoard : Sessao.IdModulo == (int)Enums.Modulos.Consignante ? ResourceAuxiliar.NomeWebUserControlDashBoardConsignante : ResourceAuxiliar.NomeWebUserControlDashBoardConsignataria;
                int idrecurso = FachadaMaster.ObtemRecursoPorNome(nomerecurso, Sessao.IdModulo);

                CarregaControle(nomerecurso, idrecurso, 1);

                ConfiguraPerfil();
                CarregaMenu();
                AtualizaNotificacoes();
                if (Session["NomeStringConexao"] != "LOCALHOST")
                    logoorgao.Src = @"Imagens/logo" + Session["NomeStringConexao"] + ".png";
            }

        }

        public void RegistraControlePostBackAssincrono(Control control)
        {
            ScriptManagerPrincipal.RegisterAsyncPostBackControl(control);
        }
        
        protected void EditarPerfil_Click(object sender, EventArgs e)
        {
            //int id = Convert.ToInt32(((LinkButton)sender).CommandArgument);
            //int idRecurso = (int)Enums.Recursos.

            int usuario = Sessao.UsuarioLogado.IDUsuario;

            CarregaControle(ResourceAuxiliar.NomeWebUserControlUsuariosPermissoesEdicao, usuario);
        }

        public void F5(int idRecursoParaRecarregar)
        {
            FechaJanela(0);
            CarregaControle(idRecursoParaRecarregar, 1, new List<Object>().ToArray());
        }
        
        public void RecarregaControle()
        {

            try
            {

                if (IndiceControleAtual.Equals(0)) return;

                if (ControleAtual == null) return;

                string controle = ControleAtual.GetType().Name;

                if (controle.Equals("WebUserControlSubMenu")) return;

                FechaJanela(IndiceControleAtual);
                CarregaControle(controle);

            }
            catch
            {
                
            }

        }

        private void CarregaMenu()
        {
            menuPrinc.DataSource = FachadaMaster.ListaMenuOpcoes(0, Sessao.IdModulo).ToList();
            menuPrinc.DataBind();
        }

        public void ExibeAlerta(string mensagem, string tipo = "message")
        {
            WebUserControlAlertaSistema.ExibeAlerta(mensagem, tipo);
        }

        private Auxiliar.Auxiliar Util
        {
            get
            {
                return new Auxiliar.Auxiliar(Page);
            }
        }

        private void ConfiguraPerfil()
        {

            string pathImagemPerfil = Util.ObtemPathImagemPerfil(Sessao.UsuarioLogado.IDUsuario, Sessao.PastaUpload);
            if (pathImagemPerfil != null) ImagemFloat.ImageUrl = pathImagemPerfil;

            LabelNomePerfil.Text = string.Join(" ", Sessao.UsuarioLogado.NomeCompleto.Split(new[] { " " }, StringSplitOptions.RemoveEmptyEntries).Where(x => x.Length > 3).Take(2).ToArray());
            LabelPerfil.Text = FachadaMaster.ObtemPerfil(Sessao.IdPerfil).Nome;
            LabelModulo.Text = ((Enums.Modulos)Sessao.IdModulo).ToString().ToUpper();
            LabelBoasVindas.Text = string.Format("{0} {1}!", BoasVindas, LabelNomePerfil.Text);

            string caminhoLogo = string.Empty;

            if (Sessao.IdModulo == (int)Enums.Modulos.Consignataria)
            {

                int idconsignataria = FachadaMaster.ObtemEmpresa(Sessao.IdBanco).IDContribuinteFastConsig ?? 0;
                
                using (ServicoUsuarioClient servico = new ServicoUsuarioClient())
                {

                    Consignataria consignataria = servico.ObtemConsignataria(idconsignataria);

                    if (consignataria != null)
                    {

                        caminhoLogo = consignataria.Logo;

                        string diretorio = Path.GetDirectoryName(caminhoLogo) + "//";
                        string imagem = string.Format("FULL_{0}", Path.GetFileName(caminhoLogo));

                        caminhoLogo = string.Concat(diretorio, imagem);
                        
                    }

                }

            }

            ImgLogoBanco.Visible = (Sessao.IdModulo.Equals((int)Enums.Modulos.Consignataria) || Sessao.IdModulo.Equals((int)Enums.Modulos.Sindicato)) && !string.IsNullOrEmpty(caminhoLogo);
            
            if (!string.IsNullOrEmpty(caminhoLogo)) ImgLogoBanco.Src = caminhoLogo;

        }

        private string BoasVindas
        {

            get
            {

                DateTime tempo = DateTime.Now;

                if (tempo.Hour > 6 && tempo.Hour < 12) return ResourceMensagens.MensagemBomDia;
                if (tempo.Hour >= 12 && tempo.Hour < 18) return ResourceMensagens.MensagemBoaTarde;

                return ResourceMensagens.MensagemBoaNoite;

            }

        }

        private string ObtemCaminhoLogoConsignataria(int idConsignataria)
        {

            using (ServicoUsuarioClient servicoUsuario = new ServicoUsuarioClient())
            {
                Consignataria consignataria = servicoUsuario.ListarConsignatarias().SingleOrDefault(x => x.IDConsignataria.Equals(idConsignataria));
                return consignataria == null ? string.Empty : string.Format(PathImagensLogosCompletos, consignataria.Logo);
            }

        }

        private GerenciadorSessao Sessao
        {
            get
            {
                return new GerenciadorSessao(Page);
            }
        }

        public void ExibeMensagem(string mensagem, string titulo = "")
        {
            MsgBox.Title = "Atenção";
            if (!string.IsNullOrEmpty(titulo))
                MsgBox.Title = titulo;

            //MessageBoxControl1.NameBtn = "Open Message";
            MsgBox.NameBtnMessageBox = "Ok";
            //MessageBoxControl1.tipo = WebUserControl.MessageBoxControl.typeMessageBox.growlMessageBox;
            MsgBox.tipo = jQueryMessageBox.WebUserControl.MessageBoxControl.typeMessageBox.centerMessageBox;
            MsgBox.Text = mensagem;

            MsgBox.btn_Click(this, new EventArgs());
            //pcAviso.HeaderText = string.IsNullOrEmpty(titulo) ? ResourceMensagens.TituloAviso : titulo;
            //LabelMensagemPopup.Text = mensagem;

            //pcAviso.ShowOnPageLoad = true;
            upPrincipal.Update();

        }

        public void ExibeMensagemComRedirecionamento(string mensagem, string titulo = "", string url = "")
        {
            ExibeMensagem(mensagem, titulo);
        }

        public string Titulo
        {
            set 
            { 
                LabelTitulo.Text = value;
                upTitulo.Update();
            }
            get
            {
                return LabelTitulo.Text;
            }
        }

        public string SubTitulo
        {
            set
            {
                LabelSubTitulo.Text = (value != string.Empty ? " - " + value : string.Empty);
                upTitulo.Update();
            }
            get
            {
                return LabelSubTitulo.Text;
            }
        }

        public string Icone
        {
            set
            {
                IconeDash.Src = string.Format(PathImagensIcone, value);
                upTitulo.Update();
            }
        }

        public CustomUserControl CarregaControle(int idrecurso, params object[] parametros)
        {
            return CarregaControle(string.Empty, idrecurso, 1, parametros);
        }

        public CustomUserControl CarregaControle(int idrecurso, int idpermissao, params object[] parametros)
        {
            return CarregaControle(string.Empty, idrecurso, idpermissao, parametros);
        }

        public CustomUserControl CarregaControle(string nomeControle, params object[] parametros)
        {
            return CarregaControle(nomeControle, FachadaMaster.ObtemRecursoPorNome(nomeControle, Sessao.IdModulo), 1, parametros);
        }

        public CustomUserControl ControleAnterior
        {
            get
            {
                if (ControleAtual == null) return null;
                return ObtemControle(ControleAtual.IndiceControleAnterior);
            }
        }

        public CustomUserControl CarregaControle(string nomeControle, int idrecurso, int idpermissao, params object[] parametros)
        {

            if (idrecurso == 0) idrecurso = FachadaMaster.ObtemRecursoPorNome(nomeControle, Sessao.IdModulo);

            Sessao.IdRecurso = idrecurso;
            if (string.IsNullOrEmpty(nomeControle)) nomeControle = FachadaMaster.ObtemRecurso(idrecurso).Arquivo;

            Sessao.IdRecurso = idrecurso;
            Sessao.NomeRecurso = nomeControle;

            if (idrecurso > 0 && !FachadaPermissoesAcesso.CheckPermissao(idrecurso, Sessao.IdBanco, Sessao.IdPerfil, idpermissao))
            {
                ExibeMensagem(ResourceMensagens.MensagemAcessoNegado, ResourceMensagens.TituloPermissaoAcesso);
                return null;
            }

            ConfiguraVisibilidadeControles(null);

            int indiceControleAnterior = IndiceControleAtual;

            Sessao.HistoricoNavegacaoRecursos.Push(idrecurso);

            CustomUserControl controleParaCarregar = (CustomUserControl)LoadControl(string.Format(PathWebUserControls, nomeControle));

            controleParaCarregar.Titulo = string.Empty;
            controleParaCarregar.SubTitulo = string.Empty;
            controleParaCarregar.ID = Guid.NewGuid().ToString();
            controleParaCarregar.IdRecurso = idrecurso;
            controleParaCarregar.IdPermissao = idpermissao;

            controleParaCarregar.ConfiguraParametros(indiceControleAnterior, parametros);
            controleParaCarregar.ConfiguraTopo();

            if (string.IsNullOrEmpty(controleParaCarregar.Titulo) && !idrecurso.Equals(-1)) controleParaCarregar.Titulo = FachadaMaster.ObtemRecurso(idrecurso).Nome;

            phPrincipal.Controls.Add(controleParaCarregar);
            IndiceControleAtual = phPrincipal.Controls.Count - 1;

            controleParaCarregar.ExecutaAcoes();

            upPrincipal.Update();
            upRodape.Update();
            upTitulo.Update();

            string browser = FachadaGeral.ObtemBrowser(Request);
            string ip = FachadaGeral.ObtemIP(Request);

            FachadaMaster.RegistrarAcesso(browser, ip, Sessao.IdModulo, Sessao.IdBanco, Sessao.IdPerfil, Sessao.UsuarioLogado == null ? 0 : Sessao.UsuarioLogado.IDUsuario, Sessao.IdRecurso, Sessao.NomeRecurso, "Acesso");

            return controleParaCarregar;

        }

        private void ConfiguraVisibilidadeControles(int? indiceVisivel)
        {

            foreach (CustomUserControl controle in phPrincipal.Controls)
            {
                controle.Visible = false;
                controle.DadosControleCarregado = false;
            }

            if (indiceVisivel != null && indiceVisivel >= 0)
            {

                CustomUserControl controle = (CustomUserControl)phPrincipal.Controls[indiceVisivel.Value];

                controle.Titulo = string.Empty;
                controle.SubTitulo = string.Empty;
                controle.Visible = true;

                IndiceControleAtual = indiceVisivel.Value;

                if (Sessao.HistoricoNavegacaoRecursos.Count > 1) Sessao.HistoricoNavegacaoRecursos.Pop();

                controle.ConfiguraTopo();
                controle.ExecutaAcoes();

                upPrincipal.Update();
                upTitulo.Update();
                upRodape.Update();

            }

        }

        public CustomUserControl ControleAtual
        {
            get
            {
                if (IndiceControleAtual >= 0) return (CustomUserControl)phPrincipal.Controls[IndiceControleAtual];
                return null;
            }
        }

        private int IndiceControleAtual
        {
            get 
            { 
                if (ViewState[ParametroIndiceControleAtual] == null) ViewState[ParametroIndiceControleAtual] = 0;
                return (int) ViewState[ParametroIndiceControleAtual];
            }
            set
            {
                ViewState[ParametroIndiceControleAtual] = value;
            }
        }

        public CustomUserControl ObtemControle(int posicao)
        {
            return (CustomUserControl)phPrincipal.Controls[posicao];
        }

        public void Voltar()
        {

            Sessao.Voltou = true;

            if (ControleAtual == null)
            {
                FechaJanela(0);
                return;
            }

            if (FachadaMaster.AtivaAbasSistema()) ConfiguraVisibilidadeControles(ControleAtual.IndiceControleAnterior);
            else FechaJanela(0);

        }

        public void FechaControleVoltaChamada()
        {

            Sessao.Voltou = true;

            if (!FachadaMaster.AtivaAbasSistema()) FechaJanela(0);

            if (ControleAtual == null)
            {
                FechaJanela(0);
                return;
            }

            int indiceParaVoltar = ControleAtual.IndiceControleAnterior;

            FechaJanela(IndiceControleAtual);

            if (!FachadaMaster.AtivaAbasSistema()) ConfiguraVisibilidadeControles(indiceParaVoltar);

        }

        protected void menuPrinc_Click(object sender, EventArgs e)
        {

            ASPxButton link = (ASPxButton)sender;

            int idrecurso = Convert.ToInt32(link.CommandArgument);

            ZeraBackgroundBotoesMenu();

            link.Attributes.CssStyle.Add(AtributoBackGround, EstiloBackgroundBotaoMenuSelecionado);
            link.Width = Unit.Parse("101%");

            for (int i = 0; i < phPrincipal.Controls.Count; i++)
            {

                CustomUserControl controle = (CustomUserControl) phPrincipal.Controls[i];

                if (controle.IdRecurso.ToString().Equals(link.CommandArgument))
                {
                    ConfiguraVisibilidadeControles(i);
                    return;
                }

            }

           CarregaControle(idrecurso, 1);

        }

        private void ZeraBackgroundBotoesMenu()
        {

            foreach (ASPxButton botao in menuPrinc.Controls.Cast<RepeaterItem>().SelectMany(item => item.Controls.OfType<ASPxButton>()))
            {
                botao.Width = Unit.Parse("100%");
                botao.Attributes.CssStyle.Add(AtributoBackGround, EstiloBackgroundBotaoMenuNormal);
            }

        }

        protected void menuPrinc_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {

            RepeaterItem item = e.Item;

            Recurso recurso = (Recurso)item.DataItem;
            ASPxButton botao = (ASPxButton)item.FindControl(IdControleMenuI);

            botao.Image.Url = string.Format(PathImagensMenu, Request.ApplicationPath, recurso.Imagem);

        }

        protected void ImageButtonSair_Click(object sender, ImageClickEventArgs e)
        {
            string browser = FachadaGeral.ObtemBrowser(Request);
            string ip = FachadaGeral.ObtemIP(Request);

            FachadaMaster.RegistrarAcesso(browser, ip, Sessao.IdModulo, Sessao.IdBanco, Sessao.IdPerfil, Sessao.UsuarioLogado == null ? 0 : Sessao.UsuarioLogado.IDUsuario, Sessao.IdRecurso, Sessao.NomeRecurso, "Logout");

            Sessao.Finaliza();
            Response.Redirect(PathPaginaLogin);
        }

        protected void ImageButtonBusca_Click(object sender, ImageClickEventArgs e)
        {

            if (string.IsNullOrEmpty(textfield.Text))
            {
                ExibeMensagem(ResourceMensagens.MensagemDigitarTextoPesquisa);
                return;
            }

            WebUserControlPainelResultadoBusca.ConfiguraResultadoBusca(textfield.Text);

            PopupControlBusca.ShowOnPageLoad = true;

            textfield.Text = string.Empty;

        }

        protected void ButtonFechar_Click(object sender, EventArgs e)
        {
            //pcAviso.ShowOnPageLoad = false;
        }

        public void EscondeBusca()
        {
            PopupControlBusca.ShowOnPageLoad = false;
        }

        private void FechaJanela(int posicaoRemocao)
        {

            if (!FachadaMaster.AtivaAbasSistema())
            {

                if (Sessao.HistoricoNavegacaoRecursos.Count > 1)
                {

                    phPrincipal.Controls.Clear();

                    Sessao.HistoricoNavegacaoRecursos.Pop();

                    int idRecurso = Sessao.HistoricoNavegacaoRecursos.Pop();
                    while (idRecurso < 0) idRecurso = Sessao.HistoricoNavegacaoRecursos.Pop();

                    CarregaControle(idRecurso);

                    upPrincipal.Update();

                }

            }
            else
            {

                if (phPrincipal.Controls.Count.Equals(1)) return;

                if (ControleAtual == null) return;

                int indiceAnterior = ControleAtual.IndiceControleAnterior;

                int i = 0;

                foreach (CustomUserControl controle in phPrincipal.Controls) if (controle.IndiceControleAnterior > 0 && i++ >= posicaoRemocao - 2) controle.IndiceControleAnterior--;

                ConfiguraVisibilidadeControles(indiceAnterior);

            }
            

        }

        protected void ImageButtonVoltar_Click(object sender, EventArgs e)
        {
            Voltar();
        }

        protected void ImageButtonFechar_Click(object sender, EventArgs e)
        {
            Voltar();
        }

        protected void ASPxPageControlJanelas_Click(object source, TabControlCancelEventArgs e)
        {
            AbreJanela(e.Tab.Index);
        }

        private void AbreJanela(int posicao)
        {
            ConfiguraVisibilidadeControles(posicao);
        }

        protected void ScriptManagerPrincipal_AsyncPostBackError(object sender, AsyncPostBackErrorEventArgs e)
        {

            string erro = e.Exception.Message + TagBr;

            if (erro.ToUpper().Contains(MensagemErroComRedirecionamento1)) Response.Redirect(PathPaginaLogin);
            else if (erro.ToUpper().Contains(MensagemErroComRedirecionamento2)) Response.Redirect("http://www.fastconsig.com.br");

            string browser = FachadaGeral.ObtemBrowser(Request);
            string ip = FachadaGeral.ObtemIP(Request);
            if (e.Exception.InnerException != null)
            {

                erro += e.Exception.InnerException.Message;

                if (e.Exception.InnerException.InnerException != null)
                {
                    
                    erro += e.Exception.InnerException.InnerException.Message;
                    
                    if (e.Exception.InnerException.InnerException.InnerException != null) erro += e.Exception.InnerException.InnerException.InnerException.Message;
                
                }
                
            }

            erro += TagBr;
            erro += e.Exception.StackTrace;

            FachadaMaster.RegistrarErro(browser, ip, Sessao.IdModulo, Sessao.IdBanco, Sessao.IdPerfil, Sessao.UsuarioLogado == null ? 0 : Sessao.UsuarioLogado.IDUsuario, Sessao.IdRecurso, Sessao.NomeRecurso, erro);

            if (!erro.Contains("The client disconnected"))
                ScriptManagerPrincipal.AsyncPostBackErrorMessage = erro;
            else
                ScriptManagerPrincipal.AsyncPostBackErrorMessage = "";
        }


        protected void ScriptManagerPrincipal_Navigate(object sender, EventArgs e)
        {
            if (ScriptManagerPrincipal.IsNavigating) Voltar();
        }

        protected void TimerNotificacao_Tick(object sender, EventArgs e)
        {

            try
            {

                AtualizaNotificacoes();
                // AtualizaMensagens();

                //upTopo.Update();

            }
            catch (Exception ex)
            {
                Sessao.Finaliza();
                Response.Redirect(PathPaginaLogin);
            }

        }

        //private void AtualizaMensagens()
        //{

        //    List<object> mensagens = new List<object>();

        //    mensagens.Add(new { Descricao = "Marcos Mezenga foi exonerado do governo." });
        //    mensagens.Add(new { Descricao = "894 novos contratos foram feitos hoje." });
        //    mensagens.Add(new { Descricao = "Existem pendências na lista de notificações." });
        //    mensagens.Add(new { Descricao = "Confira o novo gráfico de margens." });

        //    EmQuantidadeMensagens.InnerText = mensagens.Count.ToString();
        //    GridViewMensagens.DataSource = mensagens;
        //    GridViewMensagens.DataBind();

        //    upTopo.Update();

        //}

        protected void PopupControlNotificacoes_WindowCallback(object source, PopupWindowCallbackArgs e)
        {
            AtualizaNotificacoes();
        }

        private void AtualizaNotificacoes()
        {

            List<object> solicitacoes = FachadaMaster.ObtemSolicitacoes(Sessao.IdModulo, Sessao.IdBanco, Sessao.UsuarioLogado.IDUsuario);

            int quantidade = solicitacoes.Sum(x => Convert.ToInt32(x.Get(AtributoValorPro))) + solicitacoes.Sum(x => Convert.ToInt32(x.Get(AtributoValorNeutro))) + solicitacoes.Sum(x => Convert.ToInt32(x.Get(AtributoValorContra)));

            EmQuantidadeNotificacoes.InnerText = quantidade.ToString();

            GridViewNotificacoes.DataSource = solicitacoes;
            GridViewNotificacoes.DataBind();

        }

        protected void PopupControlMensagens_WindowCallback(object source, PopupWindowCallbackArgs e)
        {
            //AtualizaMensagens();
        }

        protected override void OnPreRender(EventArgs e)
        {

            base.OnPreRender(e);

            Empresa empresa = FachadaMaster.ObtemEmpresa(Sessao.IdBanco);

            using (ServicoUsuarioClient servico = new ServicoUsuarioClient())
            {

                var empresaCenter = servico.ObtemConsignataria(empresa.IDContribuinteFastConsig ?? 0);

                if (empresaCenter == null) return;

                DivZopimChat.Visible = !empresaCenter.Nome.Equals(ResourceAuxiliar.NomeBancoBmg);

            }            

        }

        protected void GridViewNotificacoes_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            GridViewRow linha = e.Row;
            if (linha.RowType == DataControlRowType.DataRow) linha.Attributes.Add(AtributoOnclick, Page.ClientScript.GetPostBackEventReference(GridViewNotificacoes, string.Format("Select${0}", linha.RowIndex.ToString())));
        }

        protected void GridViewNotificacoes_SelectedIndexChanged(object sender, EventArgs e)
        {

            int idEmpresaSolicitacaoTipo = Convert.ToInt32(GridViewNotificacoes.SelectedDataKey.Value);

            PopupControlNotificacoes.ShowOnPageLoad = false;

            CarregaControle(ResourceAuxiliar.NomeWebUserControlGerenciarAverbacao, idEmpresaSolicitacaoTipo);

        }

        protected void LabelTitulo_Click(object sender, EventArgs e)
        {
            RecarregaControle();
        }

        public void AtualizaPagina()
        {
            upPrincipal.Update();
        }

        public ASPxGridViewExporter getExportarGrid(string IDGrid)
        {
            gridExportar.GridViewID = IDGrid;
            return gridExportar;
        }

    }

}