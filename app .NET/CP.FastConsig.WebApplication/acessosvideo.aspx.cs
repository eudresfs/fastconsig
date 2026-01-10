using System;
using System.Drawing;
using System.Linq;
using System.Net.Mail;
using Bewise.Web.UI.WebControls;
using CP.FastConsig.DAL;
using CP.FastConsig.WebApplication.Auxiliar;

namespace CP.FastConsig.WebApplication
{

    public partial class acessosvideo : CustomPage
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
            DevExpress.Web.ASPxGridView.ASPxGridView.RegisterBaseScript(Page);

            Sessao.NomeStringConexao = NomeStringConexao = NomeCliente;
            Sessao.NomeStringConexaoSemEntity = NomeStringConexaoSemEntity = string.Format(ParametroStringConexaoSemEntity, NomeCliente);

            var dados = new Repositorio<AcessoVideo>().Listar().OrderByDescending(x=> x.DataAcesso).ToList();
            grid.DataSource = dados;
            grid.DataBind();
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

        protected void ButtonAcessar_Click(object sender, EventArgs e)
        {
            if (TextBoxNome.Text.Equals("Admin") && TextBoxSenha.Text.Equals("case-fc"))
            {
                DivDadosAcesso.Visible = false;
                DivGrid.Visible = true;
            }
        }

    }

}