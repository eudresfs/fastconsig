using System;
using System.Web.UI.WebControls;
using CP.FastConsig.Common;
using CP.FastConsig.DAL;
using CP.FastConsig.Facade;
using CP.FastConsig.WebApplication.Auxiliar;
using CP.FastConsig.WebApplication.FastConsigCenterService;
using CP.FastConsig.Util;
using System.Linq;

namespace CP.FastConsig.WebApplication.WebUserControls
{

    public partial class WebUserControlConsignatariasEdicao : CustomUserControl
    {

        private const string ParametroIdEmpresaEmEdicao = "IdEmpresaEmEdicao";

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

            if (EhPostBack || ControleCarregado) return;

            LimpaCampos();

            if (Sessao.IdModulo == (int)Enums.Modulos.Consignataria)
                IdEmpresaEdicao = Sessao.IdBanco;
            else
                IdEmpresaEdicao = Id.HasValue ? Id.Value : 0;

            WebUserControlProdutos1.IdEmpresa = IdEmpresaEdicao;
            WebUserControlContatos1.IdEmpresa = IdEmpresaEdicao;
            WebUserControlUsuarios1.IdEmpresa = IdEmpresaEdicao;
            WebUserControlEmpresaSuspensoes1.IdEmpresa = IdEmpresaEdicao;

            //if (IdEmpresaEdicao > 0)
            //    DivMaisDetalhes.Attributes.Add("class", "toggle_container");

            if (Sessao.IdModulo == (int)Enums.Modulos.Consignataria)
            {
                int aba_suspensoes = tabcontrol.TabPages.IndexOfText("Suspender/Ativar");

                tabcontrol.TabPages[aba_suspensoes].Visible = false;
            }

            EhPostBack = true;

            AdicionaArquivoScriptParaExecucao(ResourceAuxiliar.ArquivoScriptExpandCollapse3);
            ConfiguraCamposEditaveis();

        }

        public override void ExecutaAcoes()
        {
            ConfiguraCamposEditaveis();
        }


        private void ConfiguraCamposEditaveis()
        {
            if (Sessao.IdModulo == (int)Enums.Modulos.Consignataria)
            {
                DropDownListTipo.Enabled = false;
                DropDownListContribuinte.Enabled = false;
                TextBoxFantasia.Enabled = false;
                TextBoxRazao.Enabled = false;
                TextBoxCnpj.Enabled = false;
                TextBoxSigla.Enabled = false;
                DropDownListTipoTarifa.Enabled = false;
                dfValorTarifa.Enabled = false;

            }
        }

        private void PopulaEstados()
        {
            DropDownListEstado.DataSource = FachadaConsignatariasEdicao.ObtemEstados();
            DropDownListEstado.DataBind();
        }

        private void PopulaTipos()
        {
            DropDownListTipo.DataSource = FachadaConsignatariasEdicao.ObtemEmpresaTipos();
            DropDownListTipo.DataBind();
        }

        private void PopulaTarifas()
        {
            DropDownListTipoTarifa.DataSource = FachadaConsignatariasEdicao.ObtemTipoTarifas().ToList();
            DropDownListTipoTarifa.DataBind();
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
            if (Sessao.IdModulo == (int)Enums.Modulos.Consignante)
                PageMaster.FechaControleVoltaChamada();
            else
                PageMaster.CarregaControle( 138, 1 );

        }

        protected void ButtonRestaurarClick(object sender, EventArgs e)
        {
            FachadaConsignatariasEdicao.RestaurarPermissoes(IdEmpresaEdicao);

            PageMaster.ExibeMensagem(ResourceMensagens.MensagemSucessoOperacao);
        }

        protected void ButtonSalvarClick(object sender, EventArgs e)
        {

            if (!ValidaInformacoes()) return;

            Empresa dado = new Empresa();

            dado = PopulaObjeto(dado);

            FachadaConsignatariasEdicao.SalvarEmpresa(dado);

            if (IdEmpresaEdicao == 0)
            {
                tabcontrol.Visible = true;
                IdEmpresaEdicao = dado.IDEmpresa;
                FachadaConsignatariasEdicao.RestaurarPermissoes(IdEmpresaEdicao);
            }

            PageMaster.ExibeMensagem(ResourceMensagens.MensagemSucessoOperacao);

            if (ControleAnterior is WebUserControlConsignatarias) ((WebUserControlConsignatarias)ControleAnterior).ConfiguraTela();

            PageMaster.Voltar();

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
            emp.IDEmpresaTarifada = Convert.ToInt32(DropDownListTipoTarifa.SelectedValue);
            emp.ValorTarifa = Convert.ToDecimal(dfValorTarifa.Text);
            emp.DiaCorte = Convert.ToInt32(dfDiaCorte.Text);
            emp.RepasseDia = Convert.ToInt32(dfDiaRepasse.Text);
            emp.Ativo = 1;

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
            dfDiaCorte.Text = "0";
            dfDiaRepasse.Text = "0";
            dfValorTarifa.Text = "0";

            PopulaEstados();
            PopulaTipos();
            PopulaContribuintes();
            PopulaTarifas();

            tabcontrol.Visible = false;

        }

        private bool ValidaInformacoes()
        {

            if (Utilidades.ExisteItemVazio(TextBoxRazao.Text, TextBoxFantasia.Text, TextBoxCnpj.Text, TextBoxEmail.Text, TextBoxTelefone.Text))
            {
                PageMaster.ExibeMensagem(ResourceMensagens.MensagemTodosCamposObrigatorios);
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

        protected void ButtonNovo_Click(object sender, EventArgs e)
        {

            LimpaCampos();

            IdEmpresaEdicao = 0;

            PageMaster.SubTitulo = ResourceMensagens.TituloNovo; 

        }

        protected void PopulaDados()
        {

            if (IdEmpresaEdicao == 0) return;

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

            if (empresa.DiaCorte.ToString().Equals(string.Empty))
            {
                Parametro p = FachadaGeral.obtemParametro( "DiaCorte" );
                dfDiaCorte.Text = p.Valor;
            }
            else
            {
                dfDiaCorte.Text = empresa.DiaCorte.ToString();
            }
            if (empresa.RepasseDia.ToString().Equals(string.Empty))
            {
                Parametro p = FachadaGeral.obtemParametro("DiaRepasse");
                dfDiaRepasse.Text = p.Valor;
            }
            else
            {
                dfDiaRepasse.Text = empresa.RepasseDia.ToString();
            }
            DropDownListTipoTarifa.SelectedValue = empresa.IDEmpresaTarifada.ToString();
            dfValorTarifa.Text = (empresa.ValorTarifa.HasValue ? empresa.ValorTarifa.Value : 0).ToString();

            WebUserControlProdutos1.IdEmpresa = empresa.IDEmpresa;
            WebUserControlUsuarios1.IdEmpresa = empresa.IDEmpresa;            
            WebUserControlContatos1.IdEmpresa = empresa.IDEmpresa;
            WebUserControlEmpresaSuspensoes1.IdEmpresa = empresa.IDEmpresa;

            tabcontrol.Visible = true;

            PageMaster.SubTitulo = ResourceMensagens.TituloEditar;

            DropDownListContribuinte.SelectedValue = empresa.IDContribuinteFastConsig == null ? "0" : empresa.IDContribuinteFastConsig.ToString();

            DropDownListTipo_SelectedIndexChanged(this, new EventArgs());
        }

        public void AtualizarContatos()
        {
            WebUserControlContatos1.AtualizaContatos();
        }

        public void AtualizarProdutos()
        {
            WebUserControlProdutos1.AtualizaProdutos();
        }

        public void AtualizarUsuarios()
        {
            
            WebUserControlUsuarios1.AtualizaUsuarios();
        }

        public void AtualizarSuspensoes()
        {
            WebUserControlEmpresaSuspensoes1.AtualizaSuspensoes();
        }

        protected void tabcontrol_ActiveTabChanged(object source, DevExpress.Web.ASPxTabControl.TabControlEventArgs e)
        {
            if (e.Tab.Name == "aba_Produtos") AtualizarProdutos();
            else if (e.Tab.Name == "aba_contatos") AtualizarContatos();
            else if (e.Tab.Name == "aba_suspensoes") AtualizarSuspensoes();
            else if (e.Tab.Name == "aba_usuarios") AtualizarUsuarios();
        }

    }

}