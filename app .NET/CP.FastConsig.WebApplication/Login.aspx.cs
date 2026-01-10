using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using CP.FastConsig.Common;
using CP.FastConsig.DAL;
using CP.FastConsig.Facade;
using CP.FastConsig.Util;
using CP.FastConsig.WebApplication.Auxiliar;
using CP.FastConsig.WebApplication.FastConsigCenterService;
using Usuario = CP.FastConsig.DAL.Usuario;

namespace CP.FastConsig.WebApplication
{

    public partial class Login : CustomPage
    {

        #region Constantes

        private const string CaminhaPaginaInicial = "~/Portal";
        private const string ParametroWww = "WWW";
        private const string ParametroBanco = "Banco";
        private const string ParametroFastConsig = "FASTCONSIG";
        private const string ParametroLocalhost = "LOCALHOST";
        private const string ParametroCaseServer = "CASESERVER";
        private const string ParametroPrimeiraSenha = "PrimeiraSenha";
        private const string ParametroPerfilUsuario = "PerfilUsuario";
        private const string PrefixoUrl = "HTTP://";
        private const string ParametroServerName = "SERVER_NAME";
        private const string ParametroUsuarioBanco = "UsuarioBanco";
        private const string ParametroAtualizouUsuarioPrimeiraVisita = "AtualizouUsuarioPrimeiraVisita";
        private const string ParametroStringConexaoSemEntity = "{0}_SEM_ENTITY";
        private const string PrefixoImagemLogoGrande = "BIG_{0}";
        private const string ParametroNomeStringConexao = "NomeStringConexao";
        private const string ParametroPastaUpload = "PastaUpload";
        private const string ParametroNomeStringConexaoSemEntity = "NomeStringConexaoSemEntity";
        private const string ParametroPerfisFastConsig = "PerfisFastConsig";
        private const string AtributoOnClick = "onclick";
        private const string TagBrowswerIe = "IE6  IE  6";
        private const string TagBrowserFirefox2 = "Firefox2";
        private const string TagBrowswerFirefox3 = "Firefox3";

        #endregion Constantes

        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack) return;

            Sessao.NomeStringConexao = NomeStringConexao = NomeCliente;
            Sessao.PastaUpload = PastaUpload = NomeCliente;
            Sessao.NomeStringConexaoSemEntity = NomeStringConexaoSemEntity = string.Format(ParametroStringConexaoSemEntity, NomeCliente);

            DivLoginConteudo2.Visible = false;

            if (NomeCliente.Equals(ParametroWww) || NomeCliente.Equals(ParametroFastConsig)) Banco = null;
            else ProcessaLoginBanco();
        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            DivZopimChat.Visible = !NomeCliente.ToUpper().Equals(ResourceAuxiliar.NomeBancoBmg.ToUpper());
            ImageBanco.Visible = !string.IsNullOrEmpty(ImageBanco.Src);          

        }

        private string NomeStringConexao
        {
            get
            {
                if (ViewState[ParametroNomeStringConexao] == null) ViewState[ParametroNomeStringConexao] = string.Empty;
                return (string)ViewState[ParametroNomeStringConexao];
            }
            set
            {
                ViewState[ParametroNomeStringConexao] = value;
            }
        }

        private string PastaUpload
        {
            get
            {
                if (ViewState[ParametroPastaUpload] == null) ViewState[ParametroPastaUpload] = string.Empty;
                return (string)ViewState[ParametroPastaUpload];
            }
            set
            {
                ViewState[ParametroPastaUpload] = value;
            }
        }

        private string NomeStringConexaoSemEntity
        {
            get
            {
                if (ViewState[ParametroNomeStringConexaoSemEntity] == null) ViewState[ParametroNomeStringConexaoSemEntity] = string.Empty;
                return (string)ViewState[ParametroNomeStringConexaoSemEntity];
            }
            set
            {
                ViewState[ParametroNomeStringConexaoSemEntity] = value;
            }
        }

        private void ProcessaLoginBanco()
        {
            try
            {
                using (ServicoUsuarioClient servicoUsuario = new ServicoUsuarioClient())
                {
                    Banco = servicoUsuario.ListarConsignatarias().SingleOrDefault(x => x.Nome.ToUpper().Equals(NomeCliente));

                    if (Banco == null) return;

                    string caminhoLogo = Banco.Logo;

                    string diretorio = Path.GetDirectoryName(caminhoLogo) + "//";
                    string imagem = string.Format(PrefixoImagemLogoGrande, Path.GetFileName(caminhoLogo));

                    caminhoLogo = string.Concat(diretorio, imagem);

                    ImageBanco.Src = caminhoLogo;
                    ImageBanco.Visible = true;
                }
            }
            catch (Exception ex)
            {
                Sessao.Finaliza();
            }
        }

        private Consignataria Banco
        {
            get
            {
                return Session[ParametroBanco] as Consignataria;
            }
            set
            {
                Session[ParametroBanco] = value;
            }
        }

        private string NomeCliente
        {
            get
            {
                return ObtemNomeCliente(Request.ServerVariables[ParametroServerName]);
            }
        }

        private string ObtemNomeCliente(string url)
        {

            string nomeServidor = url;

            if (nomeServidor.ToUpper().Equals(ParametroLocalhost)) return ParametroLocalhost;
            if (nomeServidor.ToUpper().Contains(ParametroCaseServer)) return ParametroCaseServer;

            string nomeCliente = nomeServidor.Substring(0, nomeServidor.IndexOf('.'));
            
            return nomeCliente.ToUpper();

        }

        private string PrimeiraSenha
        {
            get
            {
                if (ViewState[ParametroPrimeiraSenha] == null) ViewState[ParametroPrimeiraSenha] = string.Empty;
                return (string)ViewState[ParametroPrimeiraSenha];
            }
            set
            {
                ViewState[ParametroPrimeiraSenha] = value;
            }
        }

        protected void ButtonEntrar_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(Sessao.NomeStringConexao))
                {
                    Sessao.NomeStringConexao = NomeStringConexao;
                    Sessao.PastaUpload = PastaUpload;
                    Sessao.NomeStringConexaoSemEntity = NomeStringConexaoSemEntity;
                }

                PrimeiraSenha = TextBoxLogin.Text.Trim();

                if (Banco == null) TrataLoginPadrao();
                else TrataLoginBanco();
            }
            catch (Exception ex)
            {
                LabelErro.Text = string.Format("{0} {1}", ex.Message, ex.StackTrace);
            }
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
            if (pathImagemPerfil != null) ImageFotoUsuario.ImageUrl = pathImagemPerfil;
        }

        private void TrataLoginBanco()
        {
            if (!VerificaBrowser())
                return;

            using (ServicoUsuarioClient servicoUsuario = new ServicoUsuarioClient()) UsuarioBanco = servicoUsuario.ObtemUsuarioPorLogin(TextBoxLogin.Text.Trim());

            if (!DadosUsuarioValidos(UsuarioBanco)) return;

            TrataLoginBanco(UsuarioBanco);
        }

        private FastConsigCenterService.Usuario UsuarioBanco
        {
            get
            {
                return Session[ParametroUsuarioBanco] as FastConsigCenterService.Usuario;
            }
            set
            {
                Session[ParametroUsuarioBanco] = value;
            }
        }

        private void TrataLoginBanco(FastConsigCenterService.Usuario usuario)
        {
            if (!VerificaBrowser())
                return;

            if (usuario == null)
                ExibeMensagem(ResourceMensagens.MensagemUsuarioNaoEncontrado);

            if (!DadosUsuarioBancoValidos(usuario)) return;

            IEnumerable<Consignante> consignantesDoUsuario;

            using (ServicoUsuarioClient servicoUsuario = new ServicoUsuarioClient()) consignantesDoUsuario = servicoUsuario.ConsignantesDoUsuario(usuario.CPF).OrderBy(x => x.Tipo).ToList();

            ASPxDataViewEstados.DataSource = consignantesDoUsuario;
            ASPxDataViewEstados.DataBind();

            LabelNomeUsuarioLogado.Text = usuario.Nome;

            ExibeLogin(false);
            LoginUsuarioVisivel(false);
        }

        private bool VerificaBrowser()
        {
            string browser = FachadaGeral.ObtemBrowser(Request);

            if (browser.Contains(TagBrowswerIe) || browser.Contains(TagBrowserFirefox2))
            {
                ExibeMensagem(ResourceMensagens.MensagemBrowswerNaoSuportado);
                return false;
            }

            return true;
        }

        private bool DadosUsuarioBancoValidos(FastConsigCenterService.Usuario usuario)
        {
            if (Banco == null)
            {
                ExibeMensagem(ResourceMensagens.MensagemUsuarioNaoPermitidoBanco);
                return false;
            }

            if (!usuario.UsuarioVinculos.Any(x => x.Vinculo.IDConsignataria.Equals(Banco.IDConsignataria)))
            {
                ExibeMensagem(ResourceMensagens.MensagemUsuarioNaoPermitidoBanco);
                return false;
            }
            else if (false)
            {
                ExibeMensagem(ResourceMensagens.MensagemBancoNaoCadastradoParaUsuario);
                return false;
            }

            return true;
        }

        private void LoginUsuarioVisivel(bool visivel)
        {
            DivLoginNormal.Visible = visivel;
            DivLoginBanco.Visible = !visivel;
        }

        private void TrataLoginPadrao()
        {
            if (!VerificaBrowser()) return;

            Usuario usuario = FachadaLogin.ObtemUsuarioPorLogin(TextBoxLogin.Text.Trim());

            if (!DadosUsuarioValidos(usuario)) return;

            LoginUsuarioVisivel(true);
            TrataLoginUsuario(usuario);
        }

        private void TrataLoginUsuario(Usuario usuario)
        {
            if (usuario.Situacao.Equals(Enums.TipoBloqueioUsuario.B.ToString()))
            {
                ExibeMensagem(ResourceMensagens.MensagemUsuarioBloqueado);
                return;
            }

            Sessao.UsuarioLogado = usuario;
            Sessao.IdUsuario = usuario.IDUsuario;

            List<UsuarioPerfil> perfisAux = usuario.UsuarioPerfil.OrderBy(x => x.Empresa.Nome).OrderBy(x => x.Perfil.Nome).ToList();
            List<Perfil> perfis = ObtemListaPerfis(perfisAux);

            if (perfis.Count.Equals(1) && !usuario.ApelidoLogin.Equals(ResourceAuxiliar.LoginUsuarioFastConsig))
            {
                LogaUsuario(usuario.UsuarioPerfil.First(), string.Empty, perfis[0].IDEmpresa);
            }
            else
            {
                if (usuario.ApelidoLogin.Equals(ResourceAuxiliar.LoginUsuarioFastConsig))
                {
                    PerfisFastConsig = ObtemListaPerfisFastConsig();

                    DropDownListBancosLoginFastConsig.DataSource = FachadaLogin.ListaConsignatarias().ToList();
                    DropDownListBancosLoginFastConsig.DataBind();

                    ASPxPopupControlLoginFastConsig.ShowOnPageLoad = true;

                    return;
                }

                ExibeLogin(false);
                AtualizaGridPerfis(perfis);
                ConfiguraPerfil();

                DivLoginConteudo2.Visible = true;

                LabelNomeUsuarioLogado.Text = usuario.NomeCompleto;
            }
        }

        private List<Perfil> PerfisFastConsig
        {
            get
            {
                if (Session[ParametroPerfisFastConsig] == null) Session[ParametroPerfisFastConsig] = new List<Perfil>();
                return (List<Perfil>)Session[ParametroPerfisFastConsig];
            }
            set
            {
                Session[ParametroPerfisFastConsig] = value;
            }
        }

        private void AtualizaGridPerfis(IEnumerable<Perfil> perfis)
        {
            GridViewPerfis.DataSource = perfis;
            GridViewPerfis.DataBind();
        }

        private List<Perfil> ObtemListaPerfisFastConsig()
        {
            List<DAL.Perfil> todosPerfis = FachadaLogin.ObtemTodosPerfis().ToList();
            return todosPerfis.Select(usuarioPerfil => new Perfil { IDUsuarioPerfil = 0, IDPerfil = usuarioPerfil.IDPerfil, Empresa = String.Empty, Nome = usuarioPerfil.Nome, Modulo = ((Enums.Modulos)usuarioPerfil.IDModulo).ToString(), Matricula = "---" }).ToList();
        }

        private List<Perfil> ObtemListaPerfis(IEnumerable<UsuarioPerfil> perfis)
        {
            List<Perfil> retorno = new List<Perfil>();

            List<Perfil> perfisNaoFuncionarios = perfis.Where(x => !x.Perfil.IDModulo.Equals((int)Enums.Modulos.Funcionario)).Select(x => new Perfil { IDUsuarioPerfil = x.IDUsuarioPerfil, Empresa = x.Empresa.Nome, Nome = x.Perfil.Nome, Modulo = x.Modulo, Matricula = "---" }).ToList();
            List<Perfil> perfisFuncionarios = new List<Perfil>();

            List<UsuarioPerfil> perfisFuncionariosAux = perfis.Where(x => x.Perfil.IDModulo.Equals((int)Enums.Modulos.Funcionario)).ToList();

            foreach (UsuarioPerfil usuarioPerfil in perfisFuncionariosAux)
            {
                IEnumerable<Funcionario> funcionariosUsuario = FachadaLogin.ObtemFuncionariosUsuario(usuarioPerfil.IDUsuario);
                perfisFuncionarios.AddRange(funcionariosUsuario.Select(funcionario => new Perfil { IDUsuarioPerfil = usuarioPerfil.IDUsuarioPerfil, Empresa = usuarioPerfil.Empresa.Nome, Nome = usuarioPerfil.Perfil.Nome, Modulo = usuarioPerfil.Modulo, Matricula = funcionario.Matricula }));
            }

            foreach (UsuarioPerfil usuarioPerfil in perfis)
            {
                if (usuarioPerfil.Empresa.IDEmpresaTipo.Equals((int)Enums.EmpresaTipo.Agente))
                {
                    perfisNaoFuncionarios.Clear();

                    List<Empresa> consignatariasAgente = FachadaLogin.ObtemConsignatariasAgente(usuarioPerfil.IDEmpresa).ToList();

                    perfisNaoFuncionarios.AddRange(consignatariasAgente.Select(consignataria => new Perfil { IDEmpresa = consignataria.IDEmpresa, IDUsuarioPerfil = usuarioPerfil.IDUsuarioPerfil, Empresa = consignataria.Nome, Nome = usuarioPerfil.Perfil.Nome, Modulo = usuarioPerfil.Modulo, Matricula = "---" }));
                }
            }

            GridViewPerfis.Columns[(int)ColunasGridViewPerfis.Matricula].Visible = perfisFuncionarios.Count > 1;

            retorno.AddRange(perfisNaoFuncionarios);
            retorno.AddRange(perfisFuncionarios);

            return retorno;
        }

        private enum ColunasGridViewPerfis
        {
            Matricula = 1
        }

        private class Perfil
        {
            public int IDUsuarioPerfil { get; set; }

            public int IDEmpresa { get; set; }

            public int IDPerfil { get; set; }

            public string Empresa { get; set; }

            public string Matricula { get; set; }

            public string Nome { get; set; }

            public string Modulo { get; set; }
        }

        private bool DadosUsuarioValidos(dynamic usuario)
        {
            bool existeSenhaProvisoria = true;

            try
            {
                if (!usuario.SenhaProvisoria.Equals(TextBoxSenha.Text.Trim()))
                {
                    ExibeMensagem(ResourceMensagens.MensagemSenhaNaoConfere);
                    return false;
                }
            }
            catch
            {
                existeSenhaProvisoria = false;
            }

            if (usuario == null)
            {
                ExibeMensagem(ResourceMensagens.MensagemUsuarioNaoEncontrado);

                Sessao.Finaliza();

                return false;
            }

            if (!existeSenhaProvisoria && !Seguranca.verifyMd5Hash(TextBoxSenha.Text.Trim(), usuario.Senha))
            {
                ExibeMensagem(ResourceMensagens.MensagemSenhaNaoConfere);
                return false;
            }

            return true;
        }

        private void ExibeMensagem(string mensagem)
        {

            StringBuilder stringBuilder = new StringBuilder();

            stringBuilder.Append("$('#dialogMensagem').dialog({ autoOpen: false, modal: true, width: 400, show: 'blind', hide: 'puff', buttons: { 'OK': function () { $(this).dialog('close'); } } });");
            stringBuilder.Append(string.Format("$('#dialogMensagem').text('{0}');", mensagem));
            stringBuilder.Append("$('#dialogMensagem').dialog('open');");

            ScriptManager.RegisterStartupScript(this, GetType(), "MensagemDialogJQuery", stringBuilder.ToString(), true);

        }

        private UsuarioPerfil PerfilUsuario
        {
            get { return Session[ParametroPerfilUsuario] as UsuarioPerfil; }
            set { Session[ParametroPerfilUsuario] = value; }
        }

        private void LogaUsuario(UsuarioPerfil usuarioPerfil, string matricula, int idEmpresa)
        {
            List<Funcionario> funcionariosUsuario = FachadaLogin.ObtemFuncionariosUsuario(usuarioPerfil.IDUsuario).ToList();

            Funcionario funcionario = funcionariosUsuario.Any() && string.IsNullOrEmpty(matricula) ? funcionariosUsuario[0] : funcionariosUsuario.SingleOrDefault(x => x.Matricula.Equals(matricula));

            if (funcionario != null && FachadaLogin.FuncionarioBloqueado(funcionario.IDFuncionario))
            {
                ExibeMensagem(ResourceMensagens.MensagemFuncionarioBloqueado);
                return;
            }

            if (FachadaLogin.EmpresaBloqueada(usuarioPerfil.IDEmpresa))
            {
                EmpresaSuspensao empresaSuspensao = FachadaLogin.ObtemSuspensao(usuarioPerfil.IDEmpresa);

                string dataInicial = empresaSuspensao == null ? ResourceMensagens.LabelIndefinido : (empresaSuspensao.DataInicial == null ? ResourceMensagens.LabelIndefinido : empresaSuspensao.DataInicial.Value.ToString(ResourceAuxiliar.FormatoDataBrasil));
                string dataFinal = empresaSuspensao == null ? ResourceMensagens.LabelIndefinido : (empresaSuspensao.DataFinal == null ? ResourceMensagens.LabelIndefinido : empresaSuspensao.DataFinal.Value.ToString(ResourceAuxiliar.FormatoDataBrasil));
                string motivo = empresaSuspensao == null ? "---" : empresaSuspensao.Motivo;

                string periodo = string.Format("{0} a {1}", dataInicial, dataFinal);

                if ((empresaSuspensao != null && empresaSuspensao.IDEmpresaSituacaoSuspensao > (int)Enums.EmpresaSituacao.SuspensoCompra) || empresaSuspensao == null)
                {
                    ExibeMensagem(string.Format(ResourceMensagens.MensagemBancoBloqueado, periodo, motivo));
                    return;
                }
            }

            Sessao.Matricula = matricula;

            if (!string.IsNullOrEmpty(Sessao.UsuarioLogado.SenhaProvisoria) && !AtualizouUsuarioPrimeiraVisita && !Sessao.UsuarioLogado.ApelidoLogin.Equals(ResourceAuxiliar.LoginUsuarioFastConsig))
            {
                PerfilUsuario = usuarioPerfil;

                ExibePainelPrimeiroAcesso();

                return;
            }

            if (idEmpresa.Equals(0) && usuarioPerfil.IDPerfil.Equals((int)Enums.Perfil.Agente))
            {
                Empresa empresaVinculada = FachadaLogin.ObtemConsignatariasAgente(usuarioPerfil.IDEmpresa).ToList().FirstOrDefault();
                idEmpresa = empresaVinculada == null ? 0 : empresaVinculada.IDEmpresa;
            }

            if (idEmpresa > 0 && FachadaLogin.EmpresaBloqueada(idEmpresa))
            {
                EmpresaSuspensao empresaSuspensao = FachadaLogin.ObtemSuspensao(idEmpresa);

                string dataInicial = empresaSuspensao == null ? ResourceMensagens.LabelIndefinido : (empresaSuspensao.DataInicial == null ? ResourceMensagens.LabelIndefinido : empresaSuspensao.DataInicial.Value.ToString(ResourceAuxiliar.FormatoDataBrasil));
                string dataFinal = empresaSuspensao == null ? ResourceMensagens.LabelIndefinido : (empresaSuspensao.DataFinal == null ? ResourceMensagens.LabelIndefinido : empresaSuspensao.DataFinal.Value.ToString(ResourceAuxiliar.FormatoDataBrasil));
                string motivo = empresaSuspensao == null ? "---" : empresaSuspensao.Motivo;

                string periodo = string.Format("{0} a {1}", dataInicial, dataFinal);

                if ((empresaSuspensao != null && empresaSuspensao.IDEmpresaSituacaoSuspensao > (int)Enums.EmpresaSituacao.SuspensoCompra) || empresaSuspensao == null)
                {
                    ExibeMensagem(string.Format(ResourceMensagens.MensagemBancoBloqueado, periodo, motivo));
                    return;
                }
            }

            FachadaLogin.IncrementaQuantidadeAcessos(Sessao.UsuarioLogado.IDUsuario);

            Sessao.IdBanco = idEmpresa > 0 ? idEmpresa : usuarioPerfil.IDEmpresa;
            Sessao.IdAgente = idEmpresa > 0 ? usuarioPerfil.IDEmpresa : 0;
            Sessao.IdPerfil = usuarioPerfil.IDPerfil;
            Sessao.IdModulo = usuarioPerfil.Perfil == null ? FachadaLogin.ObtemIdModulo(usuarioPerfil.IDPerfil) : usuarioPerfil.Perfil.IDModulo;

            Empresa agente = FachadaLogin.ObtemEmpresa(Sessao.IdBanco);
            Empresa banco = FachadaLogin.ObtemConsignatariasAgente(agente.IDEmpresa).FirstOrDefault();

            if (banco != null) Sessao.IdBanco = banco.IDEmpresa;

            string browser = FachadaGeral.ObtemBrowser(Request);
            string ip = FachadaGeral.ObtemIP(Request);
            FachadaMaster.RegistrarAcesso(browser, ip, Sessao.IdModulo, Sessao.IdBanco, Sessao.IdPerfil, Sessao.UsuarioLogado == null ? 0 : Sessao.UsuarioLogado.IDUsuario, Sessao.IdRecurso, Sessao.NomeRecurso, "Login");

            Response.Redirect(CaminhaPaginaInicial);
        }

        private void ExibePainelPrimeiroAcesso()
        {
            ASPxPopupControlPrimeiroAcesso.ShowOnPageLoad = true;
            DivLoginConteudo2.Visible = GridViewPerfis.Rows.Count > 1;
        }

        protected void ButtonTrocarUsuario_Click(object sender, EventArgs e)
        {
            CancelaLogin();
        }

        private void CancelaLogin()
        {
            ExibeLogin(true);

            DivLoginConteudo2.Visible = false;
            DivLoginNormal.Visible = false;
            DivLoginBanco.Visible = false;
        }

        private void ExibeLogin(bool visivel)
        {
            DivLogin.Visible = visivel;
            ImageBanco.Visible = visivel;

            DivLogado.Visible = !visivel;
            DivLoginNormal.Visible = !visivel;

            TextBoxLogin.Text = string.Empty;

            AtualizaGridPerfis(new List<Perfil>());
        }

        protected void GridViewPerfis_SelectedIndexChanged(object sender, EventArgs e)
        {

            int idUsuarioPerfil = Convert.ToInt32(GridViewPerfis.SelectedDataKey.Values[0]);
            int idEmpresa = Convert.ToInt32(GridViewPerfis.SelectedDataKey.Values[2]);
            int idPerfil = Convert.ToInt32(GridViewPerfis.SelectedDataKey.Values[3]);

            string matricula = GridViewPerfis.SelectedDataKey.Values[1].ToString();

            UsuarioPerfil usuarioPerfil = idUsuarioPerfil.Equals(0) ? new UsuarioPerfil { IDPerfil = idPerfil, IDEmpresa = idEmpresa, IDUsuario = Sessao.UsuarioLogado.IDUsuario } : FachadaLogin.ObtemUsuarioPerfil(idUsuarioPerfil);
            
            LogaUsuario(usuarioPerfil, matricula, idEmpresa);

        }

        protected void GridViewPerfis_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow) e.Row.Attributes.Add(AtributoOnClick, Page.ClientScript.GetPostBackEventReference(GridViewPerfis, string.Format("Select${0}", e.Row.RowIndex.ToString())));
        }

        protected void ButtonEntrarPrimeiroAcesso_Click(object sender, EventArgs e)
        {
            Usuario usuarioAux = FachadaLogin.ObtemUsuarioPorLogin(PrimeiraSenha);

            if (usuarioAux == null)
            {
                ASPxPopupControlPrimeiroAcesso.ShowOnPageLoad = false;

                ExibeMensagem(ResourceMensagens.MensagemTrocaDeSenhaProvavelmenteRealizada);

                return;
            }

            FastConsigCenterService.Usuario usuario;

            ServicoUsuarioClient servicoUsuario = new ServicoUsuarioClient();

            usuario = servicoUsuario.ObtemUsuarioPorLogin(TextBoxLogin.Text.Trim());

            if (!string.IsNullOrEmpty(TextBoxApelidoLoginPrimeiroAcesso.Text.Trim()) && FachadaLogin.ObtemUsuarioPorLogin(TextBoxApelidoLoginPrimeiroAcesso.Text.Trim()) != null)
            {
                ASPxPopupControlPrimeiroAcesso.ShowOnPageLoad = false;

                ExibeMensagem(ResourceMensagens.MensagemExisteUsuarioCadastradoComLogin);

                return;
            }

            if (!string.IsNullOrEmpty(TextBoxApelidoLoginPrimeiroAcesso.Text.Trim()) && servicoUsuario.ObtemUsuarioPorLogin(TextBoxApelidoLoginPrimeiroAcesso.Text.Trim()) != null)
            {
                ASPxPopupControlPrimeiroAcesso.ShowOnPageLoad = false;

                ExibeMensagem(ResourceMensagens.MensagemExisteUsuarioCadastradoComLogin);

                return;
            }

            if (!TextBoxSenhaPrimeiroAcesso.Text.Trim().Equals(TextBoxSenhaPrimeiroAcessoaRepeticao.Text.Trim()))
            {
                ASPxPopupControlPrimeiroAcesso.ShowOnPageLoad = false;

                ExibeMensagem(ResourceMensagens.MensagemSenhasDiferentes);

                return;
            }

            if (string.IsNullOrEmpty(TextBoxSenhaPrimeiroAcesso.Text.Trim()))
            {
                ASPxPopupControlPrimeiroAcesso.ShowOnPageLoad = false;

                ExibeMensagem(ResourceMensagens.MensagemPreencherSenha);

                return;
            }

            if (TextBoxSenhaPrimeiroAcesso.Text.Trim().Equals(Sessao.UsuarioLogado.SenhaProvisoria))
            {
                ASPxPopupControlPrimeiroAcesso.ShowOnPageLoad = false;

                ExibeMensagem(ResourceMensagens.MensagemSenhaDeveSerModificada);

                return;
            }

            if (usuario != null)
            {
                if (!string.IsNullOrEmpty(TextBoxApelidoLoginPrimeiroAcesso.Text.Trim())) usuario.Login = TextBoxApelidoLoginPrimeiroAcesso.Text.Trim();

                usuario.Senha = TextBoxSenhaPrimeiroAcesso.Text.Trim();

                servicoUsuario.AlterarUsuario(usuario.CPF, usuario.Nome, usuario.Email, usuario.Telefone, usuario.Login, Seguranca.getMd5Hash(usuario.Senha), string.Empty, usuario.IDUsuario, Enums.TipoCadastradorCenter.U.ToString());
            }
            else if (UsuarioBanco != null)
            {
                if (!string.IsNullOrEmpty(TextBoxApelidoLoginPrimeiroAcesso.Text.Trim())) UsuarioBanco.Login = TextBoxApelidoLoginPrimeiroAcesso.Text.Trim();

                UsuarioBanco.Senha = TextBoxSenhaPrimeiroAcesso.Text.Trim();

                servicoUsuario.AlterarUsuario(UsuarioBanco.CPF, UsuarioBanco.Nome, UsuarioBanco.Email, UsuarioBanco.Telefone, UsuarioBanco.Login, Seguranca.getMd5Hash(UsuarioBanco.Senha), string.Empty, usuario.IDUsuario, Enums.TipoCadastradorCenter.U.ToString());
            }

            FachadaLogin.AtualizaUsuarioPrimeiroAcesso(Sessao.UsuarioLogado.IDUsuario, TextBoxApelidoLoginPrimeiroAcesso.Text.Trim(), TextBoxSenhaPrimeiroAcesso.Text.Trim());

            AtualizouUsuarioPrimeiraVisita = true;

            LogaUsuario(PerfilUsuario, Sessao.Matricula, 0);
        }

        private bool AtualizouUsuarioPrimeiraVisita
        {
            get
            {
                if (ViewState[ParametroAtualizouUsuarioPrimeiraVisita] == null) ViewState[ParametroAtualizouUsuarioPrimeiraVisita] = false;
                return (bool)ViewState[ParametroAtualizouUsuarioPrimeiraVisita];
            }
            set
            {
                ViewState[ParametroAtualizouUsuarioPrimeiraVisita] = value;
            }
        }

        private void RedirecionaCliente(string url)
        {
            try
            {
                url = url.ToUpper().Replace(PrefixoUrl, string.Empty);

                Sessao.NomeStringConexao = NomeCliente.Equals(ParametroLocalhost) ? ParametroLocalhost : ObtemNomeCliente(url);
                Sessao.PastaUpload = Sessao.NomeStringConexao;
                Sessao.NomeStringConexaoSemEntity = string.Format(ParametroStringConexaoSemEntity, Sessao.NomeStringConexao);

                Usuario usuario = FachadaLogin.ObtemUsuarioPorLogin(UsuarioBanco.Login);

                TrataLoginUsuario(usuario);
            }
            catch (Exception ex)
            {
                CancelaLogin();
                ExibeMensagem(ResourceMensagens.MensagemNaoFoiPossivelRealizarLogin);
            }
        }

        protected void ImageButtonCliente_Click(object sender, ImageClickEventArgs e)
        {
            RedirecionaCliente(((ImageButton)sender).CommandArgument);
        }

        protected void ScriptManagerLogin_AsyncPostBackError(object sender, AsyncPostBackErrorEventArgs e)
        {
            string TagBr = "<br />";
            string erro = e.Exception.Message + TagBr;
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

            FachadaMaster.RegistrarErro(browser, ip, Sessao.IdModulo, Sessao.IdBanco, Sessao.IdPerfil, Sessao.UsuarioLogado == null ? 0 : Sessao.UsuarioLogado.IDUsuario, 0, "Login", erro);

            ScriptManagerLogin.AsyncPostBackErrorMessage = erro;
        }

        protected void ButtonLoginFastConsig_Click(object sender, EventArgs e)
        {

            ASPxPopupControlLoginFastConsig.ShowOnPageLoad = false;

            ExibeLogin(false);

            foreach (Perfil perfil in PerfisFastConsig)
            {

                if (perfil.Modulo.Equals(Enums.Modulos.Consignataria.ToString()) || perfil.Modulo.Equals(Enums.Modulos.Agente.ToString()))
                {

                    perfil.Empresa = DropDownListBancosLoginFastConsig.SelectedItem.Text;
                    perfil.IDEmpresa = Convert.ToInt32(DropDownListBancosLoginFastConsig.SelectedValue);

                }
                else
                {

                    string matricula = string.IsNullOrEmpty(TextBoxMatriculaLoginFastConsig.Text) ? "0" : TextBoxMatriculaLoginFastConsig.Text;

                    if (perfil.Modulo.Equals(Enums.Modulos.Funcionario.ToString()))
                    {
                        perfil.Matricula = matricula;
                        perfil.IDEmpresa = Convert.ToInt32(FachadaGeral.IdEmpresaConsignante());
                    }
                    else if (perfil.Modulo.Equals(Enums.Modulos.Consignante.ToString()))
                    {
                        perfil.IDEmpresa = Convert.ToInt32(FachadaGeral.IdEmpresaConsignante());
                    }

                    perfil.Empresa = FachadaLogin.ObtemNomeEmpresaConsignante();

                }

            }

            List<int> perfisDaEmpresa = FachadaLogin.ListaPerfisEmpresa(Convert.ToInt32(DropDownListBancosLoginFastConsig.SelectedValue)).Select(x => x.IDPerfil).ToList();

            var perfisParaRemover = PerfisFastConsig.Where(x => x.Modulo.Equals(Enums.Modulos.Consignataria.ToString()) && !perfisDaEmpresa.Contains(x.IDPerfil)).ToList();

            PerfisFastConsig.RemoveAll(x => perfisParaRemover.Contains(x));

            GridViewPerfis.Columns[(int)ColunasGridViewPerfis.Matricula].Visible = true;

            AtualizaGridPerfis(PerfisFastConsig);
            ConfiguraPerfil();

            DivLoginConteudo2.Visible = true;

            LabelNomeUsuarioLogado.Text = Sessao.UsuarioLogado.NomeCompleto;
        }

    }

}