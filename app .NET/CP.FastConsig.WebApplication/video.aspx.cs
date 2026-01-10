using System;
using System.Drawing;
using System.Linq;
using System.Net.Mail;
using Bewise.Web.UI.WebControls;
using CP.FastConsig.DAL;
using CP.FastConsig.WebApplication.Auxiliar;

namespace CP.FastConsig.WebApplication
{

    public partial class video : CustomPage
    {

        #region Constantes

        private const string ParametroNomeStringConexao = "NomeStringConexao";
        private const string ParametroNomeStringConexaoSemEntity = "NomeStringConexaoSemEntity";
        private const string ParametroStringConexaoSemEntity = "{0}_SEM_ENTITY";
        private const string ParametroServerName = "SERVER_NAME";
        private const string ParametroLocalhost = "LOCALHOST";
        private const string ParametroCaseServer = "CASESERVER";
        private const string MensagemPreencherEmail = "* Todos os campos são obrigatórios! Exceto o campo de senha.";
        private const string MensagemPreencherTodosOsCampos = "* Preencha todos os campos e verifique se a senha está correta!";
        private const string TituloEmailSolicitacao = "[FASTCONSIG] Solicitação de senha para visualização de Vídeo";
        private const string TituloEmail = "[FASTCONSIG] Visualização de Vídeo";
        private const string ConteudoEmail = "<b>NOME:</b> {0}<br/><b>E-MAIL:</b> {1}<br/><b>EMPRESA/ORGAO:</b> {2}<br/><b>ESTADO:</b> {3}<br/><b>TELEFONE:</b> {4}";
        private const string ServidorEmail = "smtp.gmail.com";
        private const string EmailOrigem = "fastconsig@gmail.com";
        private const string LogiEmail = "fastconsig";
        private const string SenhaEmail = "vertrigo";
        private const string MensagemSucesso = "* Solicitação enviada com sucesso! Em breve você receberá uma resposta por e-mail.";
        private const string CorDourada = "#EBCB17";
        private const string CorBranca = "#FFFFFF";
        private const string MensagemVerificarEmail = "Verifique se o e-mail está no formato correto!";

        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {

            LabelPreencherCampo.Visible = false;
            Labelerro.Visible = false;

            LabelPreencherCampo.ForeColor = ColorTranslator.FromHtml(CorDourada);

            Sessao.NomeStringConexao = NomeStringConexao = NomeCliente;
            Sessao.NomeStringConexaoSemEntity = NomeStringConexaoSemEntity = string.Format(ParametroStringConexaoSemEntity, NomeCliente);
            if (!IsPostBack)
            {
                cmbEstado.DataSource = new Repositorio<Estado>().Listar().ToList();
                cmbEstado.DataBind();
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

        private string NomeCliente
        {
            get
            {
                return ObtemNomeCliente(Request.ServerVariables[ParametroServerName]);
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

        protected void ButtonVerVideo_Click(object sender, EventArgs e)
        {

            try
            {

                if (!DadosPreenchidos(TextBoxEmail.Text, TextBoxEmpresa.Text, TextBoxNome.Text)) // || !TextBoxSenha.Text.Equals("case-fc" + DateTime.Today.Month))
                {

                    LabelPreencherCampo.Text = MensagemPreencherTodosOsCampos;
                    LabelPreencherCampo.Visible = true;
                    LabelPreencherCampo.ForeColor = ColorTranslator.FromHtml(CorBranca);

                    return;

                }

                AcessoVideo acessoVideo = new AcessoVideo();

                acessoVideo.Nome = TextBoxNome.Text;
                acessoVideo.Email = TextBoxEmail.Text;
                acessoVideo.Empresa = TextBoxEmpresa.Text;
                acessoVideo.Estado = cmbEstado.SelectedValue;
                acessoVideo.Telefone = TextBoxTelefone.Text;

                acessoVideo.DataAcesso = DateTime.Now;

                new Repositorio<AcessoVideo>().Incluir(acessoVideo);
                EnviarEmail(acessoVideo.Estado);

                DivVideo.Visible = true;
                DivDados.Visible = false;

            }
            catch (Exception ex)
            {

                Labelerro.Text = ex.StackTrace;

                Labelerro.Visible = true;
                DivVideo.Visible = false;
                DivDados.Visible = true;

            }

        }

        private bool DadosPreenchidos(params string[] parametros)
        {
            return !parametros.Any(x => x.Equals(string.Empty));
        }

        protected void ButtonTamanhoOriginal_Click(object sender, EventArgs e)
        {

            FlashControlVideo.Width = 1024;
            FlashControlVideo.Height = 788;

            FlashControlVideo.Scale = FlashControl.ScaleEnum.Showall;

        }

        protected void EnviarEmail(string estado)
        {
            try
            {
                if (!DadosPreenchidos(TextBoxEmail.Text, TextBoxEmpresa.Text, TextBoxNome.Text))
                {

                    LabelPreencherCampo.Text = MensagemPreencherEmail;
                    LabelPreencherCampo.Visible = true;
                    LabelPreencherCampo.ForeColor = ColorTranslator.FromHtml(CorBranca);

                    return;

                }

                var mail = new MailMessage();

                mail.From = new MailAddress(EmailOrigem);

                mail.To.Add("rilton.campos@casepartners.com.br  ");
                mail.To.Add("helio.pereira@casepartners.com.br");
                //mail.To.Add("adrianormp@gmail.com");
                mail.Subject = TituloEmail;
                mail.ReplyToList.Add(TextBoxEmail.Text);
                mail.IsBodyHtml = true;
                mail.Body = string.Format(ConteudoEmail, TextBoxNome.Text, TextBoxEmail.Text, TextBoxEmpresa.Text, estado, TextBoxTelefone.Text);

                var mailclient = new SmtpClient
                                     {
                                         Host = ServidorEmail,
                                         Port = 587,
                                         EnableSsl = true,
                                         Credentials = new System.Net.NetworkCredential(LogiEmail, SenhaEmail)
                                     };

                mailclient.Send(mail);

                LabelPreencherCampo.Text = MensagemSucesso;
                LabelPreencherCampo.Visible = true;
                LabelPreencherCampo.ForeColor = ColorTranslator.FromHtml(CorBranca);

            }
            catch (FormatException)
            {
                Labelerro.Text = MensagemVerificarEmail;
                Labelerro.Visible = true;
            }
            catch (Exception ex)
            {
                Labelerro.Text = ex.Message;
                Labelerro.Visible = true;
            }

        }

        protected void ButtonTamanhoPequeno_Click(object sender, EventArgs e)
        {

            FlashControlVideo.Width = 500;
            FlashControlVideo.Height = 395;

            FlashControlVideo.Scale = FlashControl.ScaleEnum.Exactfit;

        }

        protected void ButtonSolicitarSenha_Click(object sender, EventArgs e)
        {

            try
            {

                if (!DadosPreenchidos(TextBoxEmail.Text, TextBoxEmpresa.Text, TextBoxNome.Text))
                {

                    LabelPreencherCampo.Text = MensagemPreencherEmail;
                    LabelPreencherCampo.Visible = true;
                    LabelPreencherCampo.ForeColor = ColorTranslator.FromHtml(CorBranca);

                    return;

                }

                var mail = new MailMessage();

                mail.From = new MailAddress(EmailOrigem);

                mail.To.Add("rilton.campos@casepartners.com.br  ");
                mail.To.Add("helio.pereira@casepartners.com.br");

                mail.Subject = TituloEmailSolicitacao;
                mail.ReplyToList.Add(TextBoxEmail.Text);
                mail.IsBodyHtml = true;
                mail.Body = string.Format(ConteudoEmail, TextBoxNome.Text, TextBoxEmail.Text, TextBoxEmpresa.Text);

                var mailclient = new SmtpClient
                                     {
                                         Host = ServidorEmail,
                                         Port = 587,
                                         EnableSsl = true,
                                         Credentials = new System.Net.NetworkCredential(LogiEmail, SenhaEmail)
                                     };

                mailclient.Send(mail);

                LabelPreencherCampo.Text = MensagemSucesso;
                LabelPreencherCampo.Visible = true;
                LabelPreencherCampo.ForeColor = ColorTranslator.FromHtml(CorBranca);

            }
            catch (FormatException)
            {
                Labelerro.Text = MensagemVerificarEmail;
                Labelerro.Visible = true;
            }
            catch (Exception ex)
            {
                Labelerro.Text = ex.Message;
                Labelerro.Visible = true;
            }

        }

    }

}