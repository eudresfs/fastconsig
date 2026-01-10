using System;
using System.Linq;
using System.Collections.Generic;
using CP.FastConsig.Common;
using CP.FastConsig.DAL;
using CP.FastConsig.Facade;
using CP.FastConsig.WebApplication.Auxiliar;


namespace CP.FastConsig.WebApplication.WebUserControls
{

    public partial class WebUserControlFuncionariosAutorizacoesEdicao : CustomUserControl
    {

        #region

        private const string ParametroIdFuncionarioEmEdicao = "IdFuncionarioEmEdicao";
        private const string ParametroIdAutorizacaoEmEdicao = "IdAutorizacaoEmEdicao";

        #endregion

        private int IdFunc
        {
            get
            {
                if (ViewState[ParametroIdFuncionarioEmEdicao] == null) ViewState[ParametroIdFuncionarioEmEdicao] = 0;
                return (int)ViewState[ParametroIdFuncionarioEmEdicao];
            }
            set
            {
                ViewState[ParametroIdFuncionarioEmEdicao] = value;
            }
        }

        private int IdAutorizacaoEdicao
        {
            get
            {
                if (ViewState[ParametroIdAutorizacaoEmEdicao] == null) ViewState[ParametroIdAutorizacaoEmEdicao] = 0;
                return (int)ViewState[ParametroIdAutorizacaoEmEdicao];
            }
            set
            {
                ViewState[ParametroIdAutorizacaoEmEdicao] = value;
                PopulaDados();

            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {

            if (EhPostBack || ControleCarregado) return;

            RegistrarBlockScript(this, "$(document).ready(function(){ $('#TextBoxValidade').priceFormat({ prefix: '', centsSeparator: '', thousandsSeparator: '', limit : 2, centsLimit: 0 }); });", true);

            IdAutorizacaoEdicao = Id.HasValue ? Id.Value : 0;
            IdFunc = Convert.ToInt32(ParametrosConfiguracao[1]);

            EhPostBack = true;

            Novo();
            PopulaDadosFunc();
        }

        private void PopulaDadosFunc()
        {
            Funcionario func = FachadaFuncionariosConsulta.ObtemFuncionario(IdFunc);

            LabelMatriculaFuncionario.Text = func.Matricula;
            LabelNomeFuncionario.Text = func.Pessoa.Nome;
            LabelCpfFuncionario.Text = func.Pessoa.CPFMascara;
        }

        protected void ButtonNovo_Click(object sender, EventArgs e)
        {
            Novo();
        }

        private void Novo()
        {

            LimpaCampos();
            
            IdAutorizacaoEdicao = 0;

        }

        public override void ConfiguraTopo()
        {
            PageMaster.Titulo = "Gerenciar Funcionários";
            PageMaster.SubTitulo = "Autorização Especial";
        }

        protected void ButtonSalvarClick(object sender, EventArgs e)
        {

            FuncionarioAutorizacao dado = new FuncionarioAutorizacao();

            dado = PopulaAutorizacaoObjeto(dado);
            FachadaFuncionariosAutorizacoesEdicao.SalvarAutorizacao(dado);

            //if (ControleAnterior is WebUserControlFuncionariosAutorizacoes) ((WebUserControlFuncionariosAutorizacoes)ControleAnterior).Atualizar();

            PageMaster.ExibeMensagem(ResourceMensagens.MensagemSucessoOperacao);
            PageMaster.Voltar();

        }

        private void PopulaTipos()
        {

            List<FuncionarioAutorizacaoTipo> lista = FachadaFuncionariosAutorizacoesEdicao.ListaAutorizacoesTipo().ToList();

            DropDownListTipo.DataSource = lista;
            DropDownListTipo.DataBind();

        }

        private FuncionarioAutorizacao PopulaAutorizacaoObjeto(FuncionarioAutorizacao item)
        {

            item.IDFuncionarioAutorizacao = IdAutorizacaoEdicao;
            item.IDFuncionario = IdFunc;
            item.AutorizacaoData = ASPxDateEditData.Date;
            item.IDFuncionarioAutorizacaoTipo = Convert.ToInt32(DropDownListTipo.SelectedValue);
            item.AutorizacaoValidade = Convert.ToInt32(TextBoxValidade.Text);
            item.Motivo = TextBoxMotivo.Text;

            return item;

        }

        private void LimpaCampos()
        {

            ASPxDateEditData.Date = DateTime.Today;
            TextBoxValidade.Text = string.Empty;
            TextBoxMotivo.Text = string.Empty;
            DropDownListTipo.SelectedIndex = -1;

            PopulaTipos();

        }

        protected void PopulaDados()
        {

            if (IdAutorizacaoEdicao == 0) return;

            FuncionarioAutorizacao item = FachadaFuncionariosAutorizacoesEdicao.ObtemAutorizacao(IdAutorizacaoEdicao);

            ASPxDateEditData.Text = item.AutorizacaoData.Date.ToString();
            TextBoxValidade.Text = item.AutorizacaoValidade.ToString();
            TextBoxMotivo.Text = item.Motivo;
            DropDownListTipo.SelectedValue = item.IDFuncionarioAutorizacaoTipo.ToString();

            PopulaTipos();

            PageMaster.SubTitulo = ResourceMensagens.TituloEditar;
       
        }

    }

}