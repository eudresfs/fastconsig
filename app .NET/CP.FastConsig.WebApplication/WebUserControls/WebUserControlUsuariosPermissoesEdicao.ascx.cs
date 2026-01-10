using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.UI.WebControls;
using CP.FastConsig.Common;
using CP.FastConsig.DAL;
using CP.FastConsig.Facade;
using CP.FastConsig.Util;
using CP.FastConsig.WebApplication.Auxiliar;
using CP.FastConsig.WebApplication.FastConsigCenterService;
using DevExpress.Web.ASPxUploadControl;
using Usuario = CP.FastConsig.DAL.Usuario;

namespace CP.FastConsig.WebApplication.WebUserControls
{

    public partial class WebUserControlUsuariosPermissoesEdicao : CustomUserControl
    {

        #region Constantes

        private const string ParametroIdUsuarioEmEdicao = "IdUsuarioEmEdicao";
        private const string ParametroIdEmpresaEmEdicao = "IdEmpresaEmEdicao";
        private const string PathImagensWebCamjpg = @"{0}Imagens\WebCam\{1}\{2}.jpg";
        private const string PathImagemTempWebCam = @"{0}Temp\webcam{1}.jpg";
        private const string LabelAgente = "Correspondente:";
        private const string LabelConsignataria = "Consignatária:";
        private const string ParametroCadastroAgente = "CadastroAgente";
        private const string ParametroSenhaCadastradaNoCenter = "SenhaCadastradaNoCenter";

        #endregion

        private bool CadastroAgente
        {
            get
            {
                if (Session[ParametroCadastroAgente] == null) Session[ParametroCadastroAgente] = false;
                return (bool)Session[ParametroCadastroAgente];
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            
            if (!EhPostBack)
            {

                ConfiguraModulos();
                ConfiguraConsignatarias();

                IdUsuarioEdicao = Id.HasValue ? Id.Value : 0;
                IdEmpresa = ParametrosConfiguracao.Count > 1 ? Convert.ToInt32(ParametrosConfiguracao[1]) : 0;
                EhPostBack = true;

                DivPerfisCadastrados.Visible = !IdUsuarioEdicao.Equals(0) && Sessao.IdModulo.Equals((int)Enums.Modulos.Consignante);
                TrPerfisCadastrados.Visible = !IdUsuarioEdicao.Equals(0) && Sessao.IdModulo.Equals((int)Enums.Modulos.Consignante);

                if (IdUsuarioEdicao.Equals(0))
                {

                    LimpaCampos();
                    GeraSenhaProvisoria();

                    if (IdEmpresa != Convert.ToInt32(FachadaGeral.IdEmpresaConsignante()))
                    {

                        DropDownListModulo.SelectedValue = CadastroAgente ?  ((int)Enums.Modulos.Agente).ToString() : ((int)Enums.Modulos.Consignataria).ToString();
                        DropDownListConsignataria.SelectedValue = IdEmpresa.ToString();

                        DropDownListModulo_SelectedIndexChanged(DropDownListModulo, new EventArgs());

                        DropDownListModulo.Enabled = false;
                        
                        DropDownListConsignataria.Enabled = false;

                    }
                }
                else
                {

                    if (CadastroAgente)
                    {
                        DropDownListConsignataria.SelectedValue = IdEmpresa.ToString();
                        DropDownListModulo_SelectedIndexChanged(DropDownListModulo, new EventArgs());
                    }

                    AtualizaListaPerfisUsuario();

                    TableResponsabilidades.Visible = true;

                }

                ConfiguraTopo();
                ConfiguraComponentes();

            }

        }

        public override void ExecutaAcoes()
        {
            AdicionaArquivoScriptParaExecucao(ResourceAuxiliar.ArquivoScriptExpandCollapsePerfis);
        }

        private void ConfiguraComponentes()
        {

            ConfiguraOpcaoFoto();
            ConfigurarTiposSolicitacoes();
            ConfiguraResponsabilidades();
            ConfiguraBloqueio();

        }

        private void ConfiguraBloqueio()
        {

            ButtonBloquear.Visible = !IdUsuarioEmEdicao.Equals(0);

            Usuario usuario = FachadaUsuariosPermissoesEdicao.ObtemUsuario(IdUsuarioEmEdicao);

            if (usuario == null) return;

            ButtonBloquear.Text = usuario.Situacao.Equals(Enums.TipoBloqueioUsuario.A.ToString()) ? ResourceMensagens.LabelBloquear : ResourceMensagens.LabelAtivar;

        }



        private void ConfiguraResponsabilidades()
        {

            if (IdUsuarioEmEdicao.Equals(0)) return;

            ListBoxTipoSolicitacoes.DataSource = FachadaUsuariosPermissoesEdicao.ListaResponsabilidades(IdUsuarioEmEdicao, Sessao.IdBanco).ToList().Select(x => new { Nome = x.EmpresaSolicitacaoTipo.Nome, IDEmpresaSolicitacaoTipo = x.IDEmpresaSolicitacaoTipo }).ToList();
            ListBoxTipoSolicitacoes.DataBind();

        }

        private void ConfigurarTiposSolicitacoes()
        {
            DropDownListTipoSolicitacoes.DataSource = FachadaUsuariosPermissoesEdicao.ObterTiposSolicitacoes(Sessao.IdModulo);
            DropDownListTipoSolicitacoes.DataBind();
        }

        private void ConfiguraOpcaoFoto()
        {

            string pathImagemPerfil = Util.ObtemPathImagemPerfil(IdUsuarioEdicao, Sessao.PastaUpload);

            if (!IdUsuarioEdicao.Equals(0) && !string.IsNullOrEmpty(pathImagemPerfil))
            {

                ImageFotoAtual.ImageUrl = pathImagemPerfil;

                DivFotoAtual.Visible = true;
                DivFotoUpload.Visible = false;
                DivFotoFlash.Visible = false;
                DivOpcaoFoto.Visible = false;
                DivCancelarOpcaoFoto.Visible = false;

            }
            else
            {

                DivFotoAtual.Visible = false;
                DivFotoUpload.Visible = false;
                DivFotoFlash.Visible = false;
                DivOpcaoFoto.Visible = true;
                DivCancelarOpcaoFoto.Visible = false;

            }

        }

        private int IdUsuarioEdicao
        {
            get
            {
                if (ViewState[ParametroIdUsuarioEmEdicao] == null) ViewState[ParametroIdUsuarioEmEdicao] = 0;
                return (int)ViewState[ParametroIdUsuarioEmEdicao];
            }
            set
            {
                ViewState[ParametroIdUsuarioEmEdicao] = value;
                PopulaDados();
            }
        }

        private int IdEmpresa
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

        private void ConfiguraConsignatarias()
        {

            DropDownListConsignataria.DataSource = CadastroAgente ? (Sessao.IdModulo.Equals((int)Enums.Modulos.Consignante) ? FachadaUsuariosPermissoesEdicao.ListaAgentes().ToList() : FachadaUsuariosPermissoesEdicao.ListaAgentesEmpresa(Sessao.IdBanco).ToList()) : FachadaUsuariosPermissoesEdicao.ListaEmpresas().ToList();
            DropDownListConsignataria.DataBind();

            DropDownListConsignataria.Items.Insert(0, new ListItem(ResourceMensagens.LabelSelecione, string.Empty));

            DropDownListConsignataria.Enabled = !Sessao.IdModulo.Equals((int)Enums.Modulos.Consignataria);

        }

        private void ConfiguraModulos()
        {

            List<Modulo> modulos = FachadaUsuariosPermissoesEdicao.ObtemModulos().Where(x => !x.IDModulo.Equals((int)Enums.Modulos.Funcionario)).ToList();

            if (Sessao.IdModulo.Equals((int)Enums.Modulos.Consignante))
            {
                if (!CadastroAgente) modulos.Remove(modulos.Single(x => x.IDModulo.Equals((int)Enums.Modulos.Agente)));
                TrConsignataria.Visible = false;
            }

            if (Sessao.IdModulo.Equals((int)Enums.Modulos.Consignataria))
            {

                modulos.Remove(modulos.Single(x => x.IDModulo.Equals((int)Enums.Modulos.Consignante)));
                modulos.Remove(CadastroAgente ? modulos.Single(x => x.IDModulo.Equals((int)Enums.Modulos.Consignataria)) : modulos.Single(x => x.IDModulo.Equals((int)Enums.Modulos.Agente)));

                TrConsignataria.Visible = true;

            }

            DropDownListModulo.DataSource = modulos;
            DropDownListModulo.DataBind();

            DropDownListModulo.Items.Insert(0, new ListItem(ResourceMensagens.LabelSelecione, string.Empty));

            DropDownListModulo.Enabled = !Sessao.IdModulo.Equals((int)Enums.Modulos.Consignataria);

            if (Sessao.IdModulo.Equals((int)Enums.Modulos.Consignataria) && DropDownListModulo.Items.Count > 1) DropDownListModulo.SelectedIndex = 1;

        }

        private string CPF
        {
            get { return TextBoxCpf.Text.Replace(".", string.Empty).Replace("-", string.Empty); }
        }

        protected void ButtonSalvarClick(object sender, EventArgs e)
        {

            if (!ValidaInformacoesUsuario()) return;

            // PROCESSANDO DADOS DO FASTCONSIGCENTER
            using (ServicoUsuarioClient servicoUsuario = new ServicoUsuarioClient())
            {

                FastConsigCenterService.Usuario usuarioPorLogin = null;

                if (!string.IsNullOrEmpty(TextBoxLogin.Text)) usuarioPorLogin = servicoUsuario.ObtemUsuarioPorLogin(TextBoxLogin.Text);

                if (usuarioPorLogin != null && IdUsuarioEmEdicao.Equals(0) && string.IsNullOrEmpty(SenhaCadastradaNoCenter))
                {
                    PageMaster.ExibeMensagem(ResourceMensagens.MensagemExisteUsuarioCadastradoComLogin);
                    return;
                }

                FastConsigCenterService.Usuario usuarioPorCpf = usuarioPorLogin ?? servicoUsuario.ObtemUsuarioPorCpf(CPF);

                bool tipoConsignataria = DropDownListModulo.SelectedValue.Equals(((int)Enums.Modulos.Consignataria).ToString());

                int idConsignataria = 0;
                int idConsignante = FachadaUsuariosPermissoesEdicao.ObtemIdConsignanteCenter();


                if (tipoConsignataria)
                {
                    idConsignataria = FachadaUsuariosPermissoesEdicao.ObtemEmpresa(Convert.ToInt32(DropDownListConsignataria.SelectedValue)).IDContribuinteFastConsig ?? 0;
                    
                    if (usuarioPorCpf != null)
                    {

                        if (!string.IsNullOrEmpty(TextBoxLogin.Text)) usuarioPorCpf.Login = TextBoxLogin.Text;
                        if (!string.IsNullOrEmpty(TextBoxSenhaProvisoria.Text)) usuarioPorCpf.Senha = Seguranca.getMd5Hash(TextBoxSenhaProvisoria.Text);

                        try
                        {
                            servicoUsuario.AtualizaUsuario(usuarioPorCpf.CPF, usuarioPorCpf.Nome, usuarioPorCpf.Email, usuarioPorCpf.Telefone, usuarioPorCpf.Login, usuarioPorCpf.Senha, idConsignataria, CPF, idConsignante, Sessao.IdModulo.Equals((int)Enums.Modulos.Consignante) ? idConsignante : idConsignataria, Sessao.IdModulo.Equals((int)Enums.Modulos.Consignante) ? Enums.TipoCadastradorCenter.C.ToString() : Enums.TipoCadastradorCenter.B.ToString());
                        }
                        catch (Exception ex)
                        {
                            FachadaMaster.RegistrarErro(Request, string.Format(ResourceMensagens.MensagemPrevencaoErroAlteracaoUsuarioFastConsigCenter, ex.Message));
                        }

                    }
                    else
                    {
                        try
                        {
                            servicoUsuario.IncluirUsuario(idConsignataria, FachadaUsuariosPermissoesEdicao.ObtemIdConsignanteCenter(), CPF, TextBoxNome.Text, TextBoxEmail.Text, TextBoxTelefone.Text, TextBoxLogin.Text, Seguranca.getMd5Hash(TextBoxSenhaProvisoria.Text), Sessao.IdModulo.Equals((int)Enums.Modulos.Consignante) ? idConsignante : idConsignataria, Sessao.IdModulo.Equals((int)Enums.Modulos.Consignante) ? Enums.TipoCadastradorCenter.C.ToString() : Enums.TipoCadastradorCenter.B.ToString());
                        }
                        catch (Exception ex)
                        {
                            FachadaMaster.RegistrarErro(Request, string.Format(ResourceMensagens.MensagemPrevencaoErroInclusaoUsuarioFastConsigCenter, ex.Message));
                        }
                    }

                }
                else if (usuarioPorCpf != null && !string.IsNullOrEmpty(TextBoxSenhaProvisoria.Text))
                {
                    servicoUsuario.AlterarSenhaUsuario(usuarioPorCpf.CPF, Seguranca.getMd5Hash(TextBoxSenhaProvisoria.Text), Sessao.IdModulo.Equals((int)Enums.Modulos.Consignante) ? idConsignante : idConsignataria, Sessao.IdModulo.Equals((int)Enums.Modulos.Consignante) ? Enums.TipoCadastradorCenter.C.ToString() : Enums.TipoCadastradorCenter.B.ToString());
                }

            }

            int idUsuario;

            if (IdUsuarioEmEdicao == 0)
            {
                idUsuario = FachadaUsuariosPermissoesEdicao.AdicionaUsuario(TextBoxNome.Text, CPF, TextBoxLogin.Text, TextBoxEmail.Text, TextBoxTelefone.Text, TextBoxSenhaProvisoria.Text, Convert.ToInt32(DropDownListPerfil.SelectedValue), Convert.ToInt32(!TrConsignataria.Visible ? FachadaGeral.IdEmpresaConsignante() : DropDownListConsignataria.SelectedValue), Convert.ToInt32(DropDownListModulo.SelectedValue), SenhaCadastradaNoCenter);
            }
            else
            {
                idUsuario = IdUsuarioEmEdicao;
                FachadaUsuariosPermissoesEdicao.AlteraUsuario(IdUsuarioEmEdicao, TextBoxNome.Text, CPF, TextBoxLogin.Text, TextBoxEmail.Text, TextBoxTelefone.Text, TextBoxSenhaProvisoria.Text, Convert.ToInt32(DropDownListPerfil.SelectedValue), Convert.ToInt32(!TrConsignataria.Visible ? FachadaGeral.IdEmpresaConsignante() : DropDownListConsignataria.SelectedValue), Convert.ToInt32(DropDownListModulo.SelectedValue));
            }

            if (!string.IsNullOrEmpty(Sessao.PathWebCamImagemTemp))
            {

                try
                {

                    string arquivo = string.Format(PathImagensWebCamjpg, Request.PhysicalApplicationPath, Sessao.PastaUpload, idUsuario);

                    if (File.Exists(arquivo)) File.Delete(arquivo);

                    File.Move(Sessao.PathWebCamImagemTemp, arquivo);

                }
                catch (Exception ex)
                {
                    Sessao.PathWebCamImagemTemp = null;
                }

            }

            LimpaCampos();

            PageMaster.ExibeMensagem(ResourceMensagens.MensagemSucessoOperacao);

            if (ControleAnterior is WebUserControlConsignatariasEdicao) ((WebUserControlConsignatariasEdicao)ControleAnterior).AtualizarUsuarios();
            if (ControleAnterior is WebUserControlAgentesEdicao) ((WebUserControlAgentesEdicao)ControleAnterior).AtualizarUsuarios();
            if (ControleAnterior is WebUserControlUsuariosPermissoes) ((WebUserControlUsuariosPermissoes)ControleAnterior).AtualizaUsuarios();

            PageMaster.Voltar();

        }

        private void LimpaCampos()
        {

            TextBoxNome.Text = string.Empty;
            TextBoxCpf.Text = string.Empty;
            TextBoxEmail.Text = string.Empty;
            TextBoxTelefone.Text = string.Empty;
            TextBoxSenhaProvisoria.Text = string.Empty;
            DropDownListModulo.Enabled = true;
            DropDownListConsignataria.Enabled = true;

            if (IdUsuarioEdicao == 0 && IdEmpresa > 0 && Sessao.IdModulo != (int)Enums.Modulos.Consignante)
            {

                DropDownListModulo.SelectedIndex = DropDownListModulo.Items.Count > 1 ? 1 : 0;

                ConfiguraPerfis();
                AtualizaConsignatarias();

                if (DropDownListConsignataria.Items.FindByValue(IdEmpresa.ToString()) != null) DropDownListConsignataria.SelectedValue = IdEmpresa.ToString();
                else DropDownListConsignataria.SelectedIndex = 0;

            }
            else
            {
                DropDownListModulo.SelectedIndex = 0;
            }

        }

        private bool ValidaInformacoesUsuario()
        {

            if (Utilidades.ExisteItemVazio(TextBoxNome.Text, CPF, TextBoxEmail.Text, TextBoxTelefone.Text, DropDownListModulo.SelectedValue, DropDownListPerfil.SelectedValue))
            {
                PageMaster.ExibeMensagem(ResourceMensagens.MensagemTodosCamposObrigatoriosCadastroAgenteConsignataria);
                return false;
            }

            if (FachadaUsuariosPermissoesEdicao.ObtemUsuarioPorLogin(TextBoxLogin.Text) != null && IdUsuarioEmEdicao.Equals(0))
            {
                PageMaster.ExibeMensagem(ResourceMensagens.MensagemExisteUsuarioCadastradoComLogin);
                return false;
            }

            if (DropDownListModulo.SelectedValue == ((int)Enums.Modulos.Consignataria).ToString() && DropDownListConsignataria.SelectedIndex.Equals(0))
            {
                PageMaster.ExibeMensagem(ResourceMensagens.MensagemSelecionarConsignataria);
                return false;
            }

            if (IdUsuarioEmEdicao.Equals(0) && string.IsNullOrEmpty(TextBoxSenhaProvisoria.Text) && string.IsNullOrEmpty(SenhaCadastradaNoCenter))
            {
                PageMaster.ExibeMensagem(ResourceMensagens.MensagemPrecisaPreencherSenha);
                return false;
            }

            if (!string.IsNullOrEmpty(TextBoxSenhaProvisoria.Text) && TextBoxSenhaProvisoria.Text.Length < 6 && string.IsNullOrEmpty(SenhaCadastradaNoCenter))
            {
                PageMaster.ExibeMensagem(ResourceMensagens.MensagemSenhaProvisoriaSeisDigitos);
                return false;
            }

            if (!Utilidades.ValidaCPF(CPF))
            {
                PageMaster.ExibeMensagem(ResourceMensagens.MensagemCpfInvalido);
                return false;
            }

            if (!Utilidades.ValidaEmail(TextBoxEmail.Text))
            {
                PageMaster.ExibeMensagem(ResourceMensagens.MensagemEmailInvalido);
                return false;
            }

            if (IdUsuarioEmEdicao == 0 && FachadaUsuariosPermissoesEdicao.ObtemUsuario(CPF) != null)
            {
                PageMaster.ExibeMensagem(ResourceMensagens.MensagemUsuarioExistente);
                return false;
            }

            Usuario usuario = FachadaUsuariosPermissoesEdicao.ObtemUsuario(IdUsuarioEdicao);

            if (usuario != null && !usuario.ApelidoLogin.ToUpper().Equals(TextBoxLogin.Text.ToUpper()) && FachadaUsuariosPermissoesEdicao.ObtemUsuarioPorLogin(TextBoxLogin.Text) != null)
            {
                PageMaster.ExibeMensagem(ResourceMensagens.MensagemExisteUsuarioCadastradoComLogin);
                return false;
            }

            return true;

        }

        protected void DropDownListModulo_SelectedIndexChanged(object sender, EventArgs e)
        {
            ConfiguraPerfis();
            AtualizaConsignatarias();
        }

        private void AtualizaConsignatarias()
        {

            if (!string.IsNullOrEmpty(DropDownListModulo.SelectedValue))
            {

                switch (Convert.ToInt32(DropDownListModulo.SelectedValue))
                {

                    case ((int)Enums.Modulos.Consignataria):

                        TrConsignataria.Visible = true;
                        LabelAgenteConsignataria.Text = LabelConsignataria;

                        break;

                    case ((int)Enums.Modulos.Agente):

                        TrConsignataria.Visible = true;
                        LabelAgenteConsignataria.Text = LabelAgente;

                        break;

                    default:

                        TrConsignataria.Visible = false;
                        break;

                }

            }

        }

        private void ConfiguraPerfis()
        {

            if (!string.IsNullOrEmpty(DropDownListModulo.SelectedValue))
            {
                DropDownListPerfil.DataSource = CadastroAgente || DropDownListModulo.SelectedValue.Equals(((int)Enums.Modulos.Agente).ToString()) ? FachadaUsuariosPermissoesEdicao.ObtemPefisAgente().ToList() : FachadaUsuariosPermissoesEdicao.ObtemPefis(Convert.ToInt32(DropDownListModulo.SelectedValue)).Where(x => !x.Nome.ToUpper().Equals("AGENTE")).ToList();
                DropDownListPerfil.DataBind();
            }

        }

        protected void ButtonGerarSenhaProvisoria_Click(object sender, EventArgs e)
        {
            GeraSenhaProvisoria();
        }

        private void GeraSenhaProvisoria()
        {

            Random random = new Random();

            TextBoxSenhaProvisoria.Text = random.Next(100000, 999999).ToString();
            LabelSituacaoSenhaProvisoria.Text = String.Empty;

        }

        private void PopulaDados()
        {

            if (IdUsuarioEdicao == 0) return;

            Usuario usuario = FachadaUsuariosPermissoesEdicao.ObtemUsuario(IdUsuarioEdicao);

            TextBoxNome.Text = usuario.NomeCompleto;
            TextBoxLogin.Text = usuario.ApelidoLogin;
            TextBoxEmail.Text = usuario.Email;
            TextBoxTelefone.Text = usuario.Celular;
            TextBoxSenhaProvisoria.Text = usuario.SenhaProvisoria;

            if (!EdicaoEmCadastro) TextBoxCpf.Text = usuario.CPF;

            LabelSituacaoSenhaProvisoria.Text = usuario.SenhaProvisoria == null ? ResourceMensagens.MensagemSenhaPersonalizadaUsuario : string.Empty;

            PageMaster.SubTitulo = ResourceMensagens.TituloEditar;

            IdUsuarioEmEdicao = usuario.IDUsuario;

            if (IdUsuarioEdicao == 0 && IdEmpresa > 0 && Sessao.IdModulo != (int)Enums.Modulos.Consignante)
            {

                DropDownListModulo.SelectedIndex = DropDownListModulo.Items.Count > 1 ? 1 : 0;

                ConfiguraPerfis();
                AtualizaConsignatarias();

                if (DropDownListConsignataria.Items.FindByValue(IdEmpresa.ToString()) != null) DropDownListConsignataria.SelectedValue = IdEmpresa.ToString();
                else DropDownListConsignataria.SelectedIndex = 0;

            }

            UsuarioPerfil usuarioPerfil = usuario.UsuarioPerfil.FirstOrDefault(x => x.Perfil.IDModulo.Equals(Sessao.IdModulo));

            if (EdicaoEmCadastro && Sessao.IdModulo.Equals((int)Enums.Modulos.Consignante))
            {

                DropDownListModulo.SelectedIndex = 0;
                DropDownListPerfil.Items.Clear();

                TrConsignataria.Visible = false;

            }
            else if (usuarioPerfil != null)
            {

                if (!CadastroAgente) DropDownListModulo.SelectedIndex = DropDownListModulo.Items.IndexOf(DropDownListModulo.Items.FindByValue(usuarioPerfil.Perfil.IDModulo.ToString()));

                DropDownListModulo_SelectedIndexChanged(this, new EventArgs());

                DropDownListPerfil.SelectedIndex = DropDownListPerfil.Items.IndexOf(DropDownListPerfil.Items.FindByValue(usuarioPerfil.IDPerfil.ToString()));

                ListItem itemParaEncontrar = DropDownListConsignataria.Items.FindByValue(EdicaoEmCadastro ? Sessao.IdBanco.ToString() : usuarioPerfil.IDEmpresa.ToString());

                int indiceParaSelecionar = 0;

                if (itemParaEncontrar != null) indiceParaSelecionar = DropDownListConsignataria.Items.IndexOf(itemParaEncontrar);

                DropDownListConsignataria.SelectedIndex = indiceParaSelecionar >= 0 ? indiceParaSelecionar : 0;

            }

            EdicaoEmCadastro = false;

        }

        private int IdUsuarioEmEdicao
        {
            get
            {
                if (ViewState[ParametroIdUsuarioEmEdicao] == null) ViewState[ParametroIdUsuarioEmEdicao] = 0;
                return (int)ViewState[ParametroIdUsuarioEmEdicao];
            }
            set
            {
                ViewState[ParametroIdUsuarioEmEdicao] = value;
            }
        }

        protected void ButtonCancelarOpcaoFoto_Click(object sender, EventArgs e)
        {
            ConfiguraOpcaoFoto();
        }

        protected void ButtonExibirFotoFlash_Click(object sender, EventArgs e)
        {

            DivFotoUpload.Visible = false;
            DivFotoFlash.Visible = true;
            DivOpcaoFoto.Visible = false;
            DivCancelarOpcaoFoto.Visible = true;

        }

        protected void ButtonExibirFotoUpload_Click(object sender, EventArgs e)
        {

            DivFotoUpload.Visible = true;
            DivFotoFlash.Visible = false;
            DivOpcaoFoto.Visible = false;
            DivCancelarOpcaoFoto.Visible = true;

        }

        protected void ASPxUploadControlFoto_FileUploadComplete(object sender, FileUploadCompleteEventArgs e)
        {
            e.CallbackData = SavePostedFile(e.UploadedFile);
        }

        private string SavePostedFile(UploadedFile uploadedFile)
        {

            if (!uploadedFile.IsValid)
            {
                PageMaster.ExibeMensagem(ResourceMensagens.MensagemSelecionarArquivoUpload);
                return string.Empty;
            }

            Sessao.PathWebCamImagemTemp = string.Format(PathImagemTempWebCam, Request.PhysicalApplicationPath, Sessao.IdSessao);

            uploadedFile.SaveAs(Sessao.PathWebCamImagemTemp);

            ASPxUploadControlFoto.Enabled = true;
            ASPxButtonUploadFoto.Enabled = true;

            LabelUpload.Text = ResourceMensagens.MensagemSucessoOperacao;

            return Sessao.PathWebCamImagemTemp;

        }

        private string SenhaCadastradaNoCenter
        {
            get
            {
                if (ViewState[ParametroSenhaCadastradaNoCenter] == null) ViewState[ParametroSenhaCadastradaNoCenter] = string.Empty;
                return (string)ViewState[ParametroSenhaCadastradaNoCenter];
            }
            set
            {
                ViewState[ParametroSenhaCadastradaNoCenter] = value;
            }
        }

        protected void TextBoxCpf_TextChanged(object sender, EventArgs e)
        {

            bool prossegue = true;

            Usuario usuario = FachadaUsuariosPermissoesEdicao.ObtemUsuario(CPF);

            if (usuario == null || usuario.IDUsuario == IdUsuarioEdicao) prossegue = false;

            using (ServicoUsuarioClient servicoUsuario = new ServicoUsuarioClient())
            {

                var usuarioCenter = !IdUsuarioEmEdicao.Equals(0) || usuario != null ? null : servicoUsuario.ObtemUsuarioPorCpf(CPF);

                if (usuarioCenter != null)
                {

                    prossegue = false;

                    TextBoxNome.Text = usuarioCenter.Nome;
                    TextBoxEmail.Text = usuarioCenter.Email;
                    TextBoxTelefone.Text = usuarioCenter.Telefone;
                    TextBoxLogin.Text = usuarioCenter.Login;

                    LabelSituacaoSenhaProvisoria.Visible = true;
                    LabelSituacaoSenhaProvisoria.Text = ResourceMensagens.MensagemSenhaJaCadastrada;

                    TextBoxSenhaProvisoria.Text = string.Empty;

                    SenhaCadastradaNoCenter = usuarioCenter.Senha;

                }

            }

            if (!prossegue) return;

            EdicaoEmCadastro = true;

            IdUsuarioEdicao = usuario.IDUsuario;
            TableResponsabilidades.Visible = true;

            DropDownListModulo.Enabled = Sessao.IdModulo.Equals((int) Enums.Modulos.Consignante);
            DropDownListModulo.SelectedValue = Sessao.IdModulo.ToString();

            DropDownListModulo_SelectedIndexChanged(DropDownListModulo, new EventArgs());

            ConfiguraResponsabilidades();

            TextBoxNome.Focus();

        }

        private bool EdicaoEmCadastro
        {

            get
            {
                if (Session["EdicaoEmCadastro"] == null) Session["EdicaoEmCadastro"] = false;
                return (bool)Session["EdicaoEmCadastro"];
            }
            set
            {
                Session["EdicaoEmCadastro"] = value;
            }

        }

        protected void ButtonTrocarFoto_Click(object sender, EventArgs e)
        {

            DivFotoAtual.Visible = false;
            DivFotoUpload.Visible = false;
            DivFotoFlash.Visible = false;
            DivOpcaoFoto.Visible = true;
            DivCancelarOpcaoFoto.Visible = false;

        }

        protected void DropDownListConsignataria_SelectedIndexChanged(object sender, EventArgs e)
        {

            List<Perfil> listaPerfis = new List<Perfil>();

            listaPerfis = DropDownListConsignataria.SelectedIndex.Equals(0) ? FachadaUsuariosPermissoesEdicao.ObtemPefis(Convert.ToInt32(DropDownListModulo.SelectedValue)) : FachadaUsuariosPermissoesEdicao.ObtemPefis(Convert.ToInt32(DropDownListModulo.SelectedValue), Convert.ToInt32(DropDownListConsignataria.SelectedValue));

            DropDownListPerfil.DataSource = listaPerfis.Where(x=> !x.Nome.ToUpper().Equals("AGENTE")).ToList();
            DropDownListPerfil.DataBind();

        }

        protected void ButtonRemoverPerfil_Click(object sender, EventArgs e)
        {

            if (string.IsNullOrEmpty(ListBoxPerfisCadastrados.SelectedValue))
            {
                PageMaster.ExibeMensagem(ResourceMensagens.MensagemMarcarUmaOpcao);
                return;
            }

            int idUsuarioPerfil = Convert.ToInt32(ListBoxPerfisCadastrados.SelectedValue);

            FachadaUsuariosPermissoesEdicao.RemovePerfilUsuario(idUsuarioPerfil);

            AtualizaListaPerfisUsuario();

            PageMaster.ExibeMensagem(ResourceMensagens.MensagemSucessoOperacao);

        }

        private void AtualizaListaPerfisUsuario()
        {

            var perfis = Sessao.IdModulo.Equals((int)Enums.Modulos.Consignante) ? FachadaUsuariosPermissoesEdicao.ObtemPefisUsuario(IdUsuarioEdicao).Select(x => new { IDUsuarioPerfil = x.IDUsuarioPerfil, Descricao = string.Format("{0} - {1}", x.Perfil.Nome, x.Empresa.Nome) }).ToList() : FachadaUsuariosPermissoesEdicao.ObtemPefisUsuario(IdUsuarioEdicao).Where(x => x.Perfil.IDModulo.Equals(Sessao.IdModulo)).Select(x => new { IDUsuarioPerfil = x.IDUsuarioPerfil, Descricao = string.Format("{0} - {1}", x.Perfil.Nome, x.Empresa.Nome) }).ToList();

            ListBoxPerfisCadastrados.DataSource = perfis;
            ListBoxPerfisCadastrados.DataBind();

        }

        protected void ButtonExcluir_Click(object sender, EventArgs e)
        {

            Usuario usuario = FachadaUsuariosPermissoesEdicao.ObtemUsuario(IdUsuarioEmEdicao);

            FachadaUsuariosPermissoesEdicao.RemoveUsuario(IdUsuarioEmEdicao);

            using (ServicoUsuarioClient servicoUsuario = new ServicoUsuarioClient()) servicoUsuario.ExcluirUsuarioPorCpf(usuario.CPF, Sessao.IdModulo.Equals((int)Enums.Modulos.Consignante) ? FachadaUsuariosPermissoesEdicao.ObtemIdConsignanteCenter() : Sessao.IdBanco, Sessao.IdModulo.Equals((int)Enums.Modulos.Consignante) ? Enums.TipoCadastradorCenter.C.ToString() : Enums.TipoCadastradorCenter.B.ToString());

            WebUserControlUsuariosPermissoes controleAnterior = (WebUserControlUsuariosPermissoes)PageMaster.ControleAnterior;

            if (controleAnterior != null) controleAnterior.AtualizaUsuarios();

            PageMaster.ExibeMensagem(ResourceMensagens.MensagemSucessoOperacao);

            PageMaster.FechaControleVoltaChamada();

        }

        protected void ButtonAdicionarSolicitacaoTipo_Click(object sender, EventArgs e)
        {

            if (!ListBoxTipoSolicitacoes.Items.Contains(DropDownListTipoSolicitacoes.SelectedItem))
            {
                ListBoxTipoSolicitacoes.Items.Add(DropDownListTipoSolicitacoes.SelectedItem);
                FachadaUsuariosPermissoesEdicao.AdicionaResponsabilidade(IdUsuarioEmEdicao, Sessao.IdBanco, Convert.ToInt32(DropDownListTipoSolicitacoes.SelectedItem.Value));
            }
            else
            {
                PageMaster.ExibeMensagem(ResourceMensagens.MensagemItemJaAdicionado);
            }

            ListBoxTipoSolicitacoes.SelectedIndex = 0;

        }

        protected void ButtonRemoverSolicitacaoTipo_Click(object sender, EventArgs e)
        {

            if (ListBoxTipoSolicitacoes.SelectedIndex >= 0)
            {
                FachadaUsuariosPermissoesEdicao.RemoveResponsabilidade(IdUsuarioEmEdicao, Sessao.IdBanco, Convert.ToInt32(ListBoxTipoSolicitacoes.SelectedItem.Value));
                ListBoxTipoSolicitacoes.Items.RemoveAt(ListBoxTipoSolicitacoes.SelectedIndex);
            }
            else
            {
                PageMaster.ExibeMensagem(ResourceMensagens.MensagemSelecionarItemRemocao);
            }

        }

        protected void ButtonBloquear_Click(object sender, EventArgs e)
        {

            FachadaUsuariosPermissoesEdicao.BloqueiaUsuario(IdUsuarioEmEdicao);

            PageMaster.ExibeMensagem(ResourceMensagens.MensagemSucessoOperacao);

            ConfiguraBloqueio();

        }

    }

}