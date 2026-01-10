using System;
using System.Linq;
using System.Collections.Generic;
using CP.FastConsig.Common;
using CP.FastConsig.DAL;
using CP.FastConsig.Facade;
using CP.FastConsig.WebApplication.Auxiliar;

namespace CP.FastConsig.WebApplication.WebUserControls
{

    public partial class WebUserControlEmpresaSuspensoesEdicao : CustomUserControl
    {

        #region Constantes

        private const string ParametroIdEmpresaEmEdicao = "IdEmpresaEmEdicao";
        private const string ParametroIdSuspensaoEmEdicao = "IdSuspensaoEmEdicao";
        private const string TituloPagina = "Suspensão da Consignatária";
        private const string FormatoDataPadrao = "dd/MM/yyyy";

        #endregion

        public override void ConfiguraTopo()
        {
            PageMaster.Titulo = TituloPagina;
            PageMaster.SubTitulo = "Inclusão";
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

        private int IdSuspensaoEdicao
        {
            get
            {
                if (ViewState[ParametroIdSuspensaoEmEdicao] == null) ViewState[ParametroIdSuspensaoEmEdicao] = 0;
                return (int)ViewState[ParametroIdSuspensaoEmEdicao];
            }
            set
            {
                ViewState[ParametroIdSuspensaoEmEdicao] = value;
                PopulaDados();
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            
            if (EhPostBack || ControleCarregado) return;

            EhPostBack = true;

            Novo();
            
            IdSuspensaoEdicao = Id.HasValue ? Id.Value : 0;

            if (ParametrosConfiguracao != null)
            {
                IdEmpresa = Convert.ToInt32(ParametrosConfiguracao[1]);
                NomeConsignataria.Text = FachadaConsignatarias.ObtemEmpresa(IdEmpresa).Nome;
            }

            dfDataInicio.MinDate = DateTime.Today;
            dfDataFim.MinDate = DateTime.Today.AddDays(1);

        }


        protected void ButtonNovo_Click(object sender, EventArgs e)
        {
            Novo();
        }

        private void Novo()
        {

            LimpaCampos();
            PopulaSituacao();            

            IdSuspensaoEdicao = 0;

            ConfiguraTopo();

        }

        protected void ButtonSalvarClick(object sender, EventArgs e)
        {

            if (string.IsNullOrEmpty(dfMotivo.Text))
            {
                PageMaster.ExibeMensagem(ResourceMensagens.MensagemCamposVazios);
                return;
            }

            List<EmpresaSuspensao> suspensoes = FachadaSuspensoes.ObtemSuspensoes(IdEmpresa).Where(x => x.TipoPeriodo.Equals(Enums.BloqueioPeriodo.D.ToString())).ToList();

            if (!cmbSituacao.SelectedValue.Equals(((int)Enums.EmpresaSituacao.Normal).ToString()) && suspensoes.Any(suspensao => suspensao.DataInicial.Value.ToString(FormatoDataPadrao).Equals(dfDataInicio.Date.ToString(FormatoDataPadrao)) && suspensao.DataFinal.Value.ToString(FormatoDataPadrao).Equals(dfDataFim.Date.ToString(FormatoDataPadrao))))
            {
                PageMaster.ExibeMensagem(ResourceMensagens.MensagemJaExisteBloqueioParaEstaData);
                return;
            }

            EmpresaSuspensao dado = new EmpresaSuspensao();

            dado = PopulaObjeto(dado);

            FachadaSuspensoes.SalvarSuspensao(dado);

            PageMaster.ExibeMensagem(ResourceMensagens.MensagemSucessoOperacao);

            if (PageMaster.ControleAnterior is WebUserControlConsignatariasEdicao)
                ((WebUserControlConsignatariasEdicao)PageMaster.ControleAnterior).AtualizarSuspensoes();

            if (PageMaster.ControleAnterior is WebUserControlDashBoardConsignante)
                ((WebUserControlDashBoardConsignante)PageMaster.ControleAnterior).AtualizarSuspensoes();

            PageMaster.Voltar();

        }

        private void PopulaSituacao()
        {
            cmbSituacao.DataSource = FachadaSuspensoes.ListaSituacao().Where(x => !x.IDEmpresaSituacao.Equals((int)Enums.EmpresaSituacao.BloqueioPersonalizado)).ToList();
            cmbSituacao.DataBind();
        }

        protected override void OnPreRender(EventArgs e)
        {

            base.OnPreRender(e);

            Empresa empresa = FachadaSuspensoes.ObtemEmpresa(IdEmpresa);

            if (empresa == null || empresa.IDEmpresaSituacao.Equals((int)Enums.EmpresaSituacao.Normal))
            {
                var item = cmbSituacao.Items.FindByValue(((int) Enums.EmpresaSituacao.Normal).ToString());
                if(item != null) cmbSituacao.Items.Remove(item);
            }

            if (empresa != null && empresa.IDEmpresaSituacao.Equals((int)Enums.EmpresaSituacao.SuspensoAverbacoes))
            {
                var item = cmbSituacao.Items.FindByValue(((int)Enums.EmpresaSituacao.SuspensoCompra).ToString());
                if (item != null) cmbSituacao.Items.Remove(item);
            }

            cmbSituacao_SelectedIndexChanged(cmbSituacao, e);

        }

        private EmpresaSuspensao PopulaObjeto(EmpresaSuspensao suspensao)
        {

            suspensao.IDEmpresaSuspensao = IdSuspensaoEdicao;
            suspensao.IDEmpresa = IdEmpresa;
            suspensao.Data = DateTime.Now;
            suspensao.Motivo = dfMotivo.Text;
            suspensao.TipoPeriodo = cmbTipoPeriodo.SelectedValue;
            suspensao.IDEmpresaSituacaoAnterior = suspensao.IDEmpresaSituacaoSuspensao == 0 ? (int)Enums.EmpresaSituacao.Normal : suspensao.IDEmpresaSituacaoSuspensao;
            suspensao.IDEmpresaSituacaoSuspensao = Convert.ToInt32(cmbSituacao.SelectedValue);

            if (suspensao.TipoPeriodo.Equals(Enums.BloqueioPeriodo.D.ToString()))
            {
                suspensao.DataInicial = dfDataInicio.Date;
                suspensao.DataFinal = dfDataFim.Date;
            }

            return suspensao;

        }

        private void LimpaCampos()
        {

            dfMotivo.Text = string.Empty;
            
            cmbTipoPeriodo.SelectedIndex = 1;
            
            dfDataInicio.Date = DateTime.Today;
            dfDataFim.Date = DateTime.Today.AddDays(1);

            PopulaSituacao();

        }

        private void PopulaDados()
        {

            if (IdSuspensaoEdicao == 0) return;

            EmpresaSuspensao suspensao = FachadaSuspensoes.ObtemSuspensao(IdSuspensaoEdicao);

            dfMotivo.Text = suspensao.Motivo;
            
            cmbSituacao.SelectedValue = suspensao.IDEmpresaSituacaoSuspensao.ToString();
            cmbTipoPeriodo.SelectedValue = suspensao.TipoPeriodo;
            
            dfDataInicio.Date = suspensao.DataInicial.HasValue ? suspensao.DataInicial.Value : DateTime.Today;
            dfDataFim.Date = suspensao.DataFinal.HasValue ? suspensao.DataFinal.Value : DateTime.Today.AddDays(1);

            TrPeriodo.Visible = !cmbTipoPeriodo.SelectedValue.Equals(Enums.BloqueioPeriodo.I.ToString());

            PopulaSituacao();

            PageMaster.SubTitulo = ResourceMensagens.TituloEditar;

        }

        protected void cmbTipoPeriodo_SelectedIndexChanged(object sender, EventArgs e)
        {
            TrPeriodo.Visible = cmbTipoPeriodo.SelectedValue.Equals(Enums.BloqueioPeriodo.D.ToString());
        }

        protected void cmbSituacao_SelectedIndexChanged(object sender, EventArgs e)
        {
            TrTipoPeriodo .Visible = !cmbSituacao.SelectedValue.Equals(((int)Enums.EmpresaSituacao.Normal).ToString());
            TrPeriodo.Visible = cmbTipoPeriodo.SelectedValue.Equals(Enums.BloqueioPeriodo.D.ToString()) && !cmbSituacao.SelectedValue.Equals(((int)Enums.EmpresaSituacao.Normal).ToString());
        }

    }

}