using System;
using System.Linq;
using System.Web.UI.WebControls;
using CP.FastConsig.Common;
using CP.FastConsig.DAL;
using CP.FastConsig.Facade;
using CP.FastConsig.WebApplication.Auxiliar;
using CP.FastConsig.WebApplication.FastConsigCenterService;
using CP.FastConsig.Util;
using DevExpress.Web.ASPxClasses;

namespace CP.FastConsig.WebApplication.WebUserControls
{

    public partial class WebUserControlAgentesEdicao : CustomUserControl
    {

        #region Constantes

        private const string ParametroIdEmpresaEmEdicao = "IdEmpresaEmEdicao";
        private const string LabelSuspenderAtivar = "Suspender/Ativar";
        private const string NomeAbaUsuarios = "aba_usuarios";
        private const string NomeAbaSuspensoes = "aba_suspensoes";
        private const string NomeAbaContatos = "aba_contatos";
        private const string ParametroJaExisteAgente = "JaExisteAgente";

        #endregion

        private int IdEmpresaEdicao
        {

            get
            {
                if (ViewState[ParametroIdEmpresaEmEdicao] == null) ViewState[ParametroIdEmpresaEmEdicao] = 0;
                return (int)ViewState[ParametroIdEmpresaEmEdicao];
            }
            set
            {
                ViewState[ParametroIdEmpresaEmEdicao] = value;
                PopulaDados();
            }

        }

        protected void Page_Load(object sender, EventArgs e)
        {

            ASPxWebControl.RegisterBaseScript(Page);

            if (EhPostBack || ControleCarregado) return;

            LimpaCampos();

            IdEmpresaEdicao = Id.HasValue ? Id.Value : 0;

            ConfiguraUserControls();

            if (Sessao.IdModulo == (int)Enums.Modulos.Consignataria) tabcontrol.TabPages[tabcontrol.TabPages.IndexOfText(LabelSuspenderAtivar)].Visible = false;
            
            EhPostBack = true;

            AdicionaArquivoScriptParaExecucao(ResourceAuxiliar.ArquivoScriptExpandCollapse3);

        }
        
        private void PopulaEstados()
        {
            DropDownListEstado.DataSource = FachadaConsignatariasEdicao.ObtemEstados();
            DropDownListEstado.DataBind();
        }

        private void PopulaTipos()
        {

            DropDownListTipo.DataSource = FachadaConsignatariasEdicao.ObtemTodasEmpresaTipos().Where(x => x.IDEmpresaTipo.Equals((int)Enums.EmpresaTipo.Agente));
            DropDownListTipo.DataBind();

            DropDownListTipo.Enabled = false;

        }

        private void PopulaContribuintes()
        {

            ServicoUsuarioClient svc = new ServicoUsuarioClient();

            DropDownListContribuinte.DataSource = svc.ListarConsignatarias();
            DropDownListContribuinte.DataBind();

            DropDownListContribuinte.Items.Insert(0, new ListItem(ResourceMensagens.LabelSelecione, "0"));
            
        }

        protected void DropDownListTipo_SelectedIndexChanged(object sender, EventArgs e)
        {
            DropDownListContribuinte.Visible = (Convert.ToInt32(DropDownListTipo.SelectedValue) == (int)Enums.EmpresaTipo.Banco);
        }

        protected void ButtonCancelarClick(object sender, EventArgs e)
        {
            PageMaster.FechaControleVoltaChamada();
        }
        
        protected void ButtonSalvarClick(object sender, EventArgs e)
        {

            if (!ValidaInformacoes()) return;

            Empresa dado = new Empresa();

            dado = PopulaObjeto(dado);

            if(!JaExisteAgente) FachadaConsignatariasEdicao.SalvarEmpresa(dado);
            FachadaConsignatariasEdicao.SalvarVinculo(Sessao.IdModulo.Equals((int)Enums.Modulos.Consignante) ? Convert.ToInt32(DropDownListConsignataria.SelectedValue) : Sessao.IdBanco, dado.IDEmpresa);

            LimpaCampos();

            PageMaster.ExibeMensagem(ResourceMensagens.MensagemSucessoOperacao);
            PageMaster.FechaControleVoltaChamada();

        }

        private Empresa PopulaObjeto(Empresa emp)
        {

            emp.IDEmpresa = IdEmpresaEdicao;
            emp.CNPJ = TextBoxCnpj.Text;
            emp.RazaoSocial = TextBoxRazao.Text;
            emp.Fantasia = TextBoxFantasia.Text;
            emp.Fone1 = TextBoxTelefone.Text;
            emp.Fax = TextBoxFax.Text;
            emp.Email = TextBoxEmail.Text;
            emp.Endereco = TextBoxEndereco.Text;
            emp.Bairro = TextBoxBairro.Text;
            emp.Sigla = TextBoxSigla.Text;
            emp.CEP = TextBoxCep.Text;
            emp.Complemento = TextBoxComplemento.Text;
            emp.Cidade = TextBoxCidade.Text;
            emp.Estado = DropDownListEstado.SelectedValue;
            emp.IDEmpresaTipo = Convert.ToInt32(DropDownListTipo.SelectedValue);
            emp.IDContribuinteFastConsig = DropDownListContribuinte.SelectedItem == null ? 0 : Convert.ToInt32(DropDownListContribuinte.SelectedValue);
            emp.IDEmpresaSituacao = 1;
            emp.Ativo = 1;
            emp.IDEmpresaTarifada = 1;

            return emp;

        }

        private void LimpaCampos()
        {

            TextBoxFantasia.Text = string.Empty;
            TextBoxRazao.Text = string.Empty;
            TextBoxCnpj.Text = string.Empty;
            TextBoxEmail.Text = string.Empty;
            TextBoxTelefone.Text = string.Empty;
            TextBoxSigla.Text = string.Empty;
            TextBoxFax.Text = string.Empty;
            TextBoxEndereco.Text = string.Empty;
            TextBoxBairro.Text = string.Empty;
            TextBoxComplemento.Text = string.Empty;
            TextBoxCidade.Text = string.Empty;
            TextBoxCep.Text = string.Empty;

            JaExisteAgente = false;

            PopulaEstados();
            PopulaTipos();
            PopulaContribuintes();
            PopulaConsignatarias();

            tabcontrol.Visible = false;

        }

        private void PopulaConsignatarias()
        {

            if (!Sessao.IdModulo.Equals((int)Enums.Modulos.Consignante)) return;

            DropDownListConsignataria.Visible = true;

            DropDownListConsignataria.DataSource = FachadaUsuariosPermissoesEdicao.ListaEmpresas().ToList();
            DropDownListConsignataria.DataBind();

        }

        private bool ValidaInformacoes()
        {

            if (Utilidades.ExisteItemVazio(TextBoxRazao.Text, TextBoxFantasia.Text, TextBoxCnpj.Text, TextBoxEmail.Text, TextBoxTelefone.Text))
            {
                PageMaster.ExibeMensagem(ResourceMensagens.MensagemTodosCamposObrigatoriosCadastroAgenteConsignataria);
                return false;
            }

            if (!Utilidades.ValidaCNPJ(TextBoxCnpj.Text))
            {
                PageMaster.ExibeMensagem(ResourceMensagens.MensagemCnpjInvalido);
                return false;
            }

            if (!Utilidades.ValidaEmail(TextBoxEmail.Text))
            {
                PageMaster.ExibeMensagem(ResourceMensagens.MensagemEmailInvalido);
                return false;
            }

            if (IdEmpresaEdicao == 0 && FachadaConsignatariasEdicao.ObtemEmpresaPorCnpj(TextBoxCnpj.Text) != null)
            {
                PageMaster.ExibeMensagem(ResourceMensagens.MensagemConsignatariaExistente);
                return false;
            }

            return true;

        }

        private void PopulaDados()
        {

            if (IdEmpresaEdicao == 0)
            {

                Empresa empresaAux = FachadaConsignatariasEdicao.ObtemEmpresa(Sessao.IdBanco);
                
                DropDownListContribuinte.SelectedValue = empresaAux.IDContribuinteFastConsig == null ? "0" : empresaAux.IDContribuinteFastConsig.ToString();    
                
                return;
            
            }

            PageMaster.SubTitulo = ResourceMensagens.TituloEditar;

            Empresa empresa = FachadaConsignatariasEdicao.ObtemEmpresa(IdEmpresaEdicao);

            TextBoxFantasia.Text = empresa.Fantasia;
            TextBoxRazao.Text = empresa.RazaoSocial;
            TextBoxCnpj.Text = empresa.CNPJ;
            TextBoxEmail.Text = empresa.Email;
            TextBoxTelefone.Text = empresa.Fone1;
            TextBoxSigla.Text = empresa.Sigla;
            TextBoxFax.Text = empresa.Fax;
            TextBoxEndereco.Text = empresa.Endereco;
            TextBoxBairro.Text = empresa.Bairro;
            TextBoxComplemento.Text = empresa.Complemento;
            TextBoxCidade.Text = empresa.Cidade;
            TextBoxCep.Text = empresa.CEP;
            
            WebUserControlUsuarios1.IdEmpresa = empresa.IDEmpresa;            
            WebUserControlContatos1.IdEmpresa = empresa.IDEmpresa;
            WebUserControlEmpresaSuspensoes1.IdEmpresa = empresa.IDEmpresa;

            tabcontrol.Visible = true;

            PageMaster.SubTitulo = ResourceMensagens.TituloEditar;

            DropDownListContribuinte.SelectedValue = empresa.IDContribuinteFastConsig == null ? "0" : empresa.IDContribuinteFastConsig.ToString();
            DropDownListConsignataria.SelectedValue = FachadaAgentesEdicao.ObtemIdConsignatariaVinculadaComAgente(empresa.IDEmpresa).ToString();

            DropDownListTipo_SelectedIndexChanged(this, new EventArgs());

        }

        private void AtualizarContatos()
        {
            WebUserControlContatos1.AtualizaContatos();
        }

        public void AtualizarUsuarios()
        {
            
            WebUserControlUsuarios1.AtualizaUsuarios();
        }

        private void AtualizarSuspensoes()
        {
            WebUserControlEmpresaSuspensoes1.AtualizaSuspensoes();
        }

        protected void tabcontrol_ActiveTabChanged(object source, DevExpress.Web.ASPxTabControl.TabControlEventArgs e)
        {

            if (e.Tab.Name == NomeAbaContatos) AtualizarContatos();
            else if (e.Tab.Name == NomeAbaSuspensoes) AtualizarSuspensoes();
            else if (e.Tab.Name == NomeAbaUsuarios) AtualizarUsuarios();

        }

        protected void TextBoxCnpj_TextChanged(object sender, EventArgs e)
        {

            if (string.IsNullOrEmpty(TextBoxCnpj.Text) || IdEmpresaEdicao > 0) return;

            Empresa empresa = FachadaAgentesEdicao.ObtemEmpresaPorCnpj(TextBoxCnpj.Text);

            if (empresa == null) return;

            IdEmpresaEdicao = empresa.IDEmpresa;
            JaExisteAgente = true;

            ConfiguraUserControls();
            RecuperaDadosEmpresa(empresa);

        }

        private void ConfiguraUserControls()
        {

            WebUserControlContatos1.IdEmpresa = IdEmpresaEdicao;
            WebUserControlUsuarios1.IdEmpresa = IdEmpresaEdicao;
            WebUserControlEmpresaSuspensoes1.IdEmpresa = IdEmpresaEdicao;

        }

        private bool JaExisteAgente
        {

            get
            {
                if (ViewState[ParametroJaExisteAgente] == null) ViewState[ParametroJaExisteAgente] = false;
                return (bool)ViewState[ParametroJaExisteAgente];
            }
            set
            {
                ViewState[ParametroJaExisteAgente] = value;
            }

        }

        private void RecuperaDadosEmpresa(Empresa empresa)
        {

            TextBoxRazao.Text = empresa.RazaoSocial;
            TextBoxFantasia.Text = empresa.Fantasia;
            TextBoxSigla.Text = empresa.Sigla;
            TextBoxTelefone.Text = empresa.Fone1;
            TextBoxFax.Text = empresa.Fax;
            TextBoxEmail.Text = empresa.Email;
            TextBoxEndereco.Text = empresa.Endereco;
            TextBoxBairro.Text = empresa.Bairro;
            TextBoxComplemento.Text = empresa.Complemento;
            TextBoxCidade.Text = empresa.Cidade;
            TextBoxCep.Text = empresa.CEP;

            DropDownListEstado.SelectedValue = empresa.Estado;

        }

    }

}