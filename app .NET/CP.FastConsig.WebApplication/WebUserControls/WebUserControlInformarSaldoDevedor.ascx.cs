using System;
using System.Linq;
using CP.FastConsig.Common;
using CP.FastConsig.DAL;
using CP.FastConsig.Facade;
using CP.FastConsig.Util;
using CP.FastConsig.WebApplication.Auxiliar;

namespace CP.FastConsig.WebApplication.WebUserControls
{
    public partial class WebUserControlInformarSaldoDevedor : CustomUserControl
    {
        private const string ParametroPrazoQuitacao = "Quitacao";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (EhPostBack || ControleCarregado) return;

            PopularDados();
            ConfiguraTopo();

            EhPostBack = true;
        }

        public override void ConfiguraTopo()
        {
            PageMaster.Titulo = "Informar Saldo Devedor";
        }

        private void PopularDados()
        {
            if (Id == null || Id.Value <= 0) return;

            Averbacao con = FachadaConciliacao.ObtemAverbacao(Id.Value);

            DateTime dataMinima = Utilidades.ObtemProximoDiaUtil(2 + Convert.ToInt32(FachadaGeral.obtemParametro(ParametroPrazoQuitacao).Valor));

            DateEditValidade.MinDate = dataMinima;
            DateEditValidade.Date = dataMinima;

            LabelMatriculaFuncionario.Text = con.Funcionario.Matricula;
            LabelNomeFuncionario.Text = con.Funcionario.Pessoa.Nome;
            LabelCpfFuncionario.Text = Utilidades.MascaraCPF(con.Funcionario.Pessoa.CPF);

            LabelNumeroAverbacao.Text = con.Numero;
            LabelConsignataria.Text = con.Empresa1.Nome;
            LabelPrazo.Text = con.Prazo == null ? "0" : con.Prazo.Value.ToString();
            LabelSituacaoAtual.Text = con.AverbacaoSituacao.Nome;
            LabelValorParcela.Text = String.Format("{0:N}", con.ValorParcela);
            LabelSaldo.Text = String.Format("{0:N}", FachadaInformarQuitacao.CalculaSaldoRestante(con.IDAverbacao));

            DropDownListFormaPagamento.DataSource = FachadaConsignatarias.ListaTiposPagamentos().ToList();
            DropDownListFormaPagamento.DataBind();
        }

        private bool ValidaInformacoes()
        {
            if (DateEditValidade.Date.DayOfYear <= 1)
            {
                PageMaster.ExibeMensagem("Data de Validade Inválida!");
                return false;
            }

            if (Convert.ToInt32(DropDownListFormaPagamento.SelectedValue) == (int)Enums.TipoPagamento.TED && Utilidades.ExisteItemVazio(ASPxTextBoxIdentificado.Text, ASPxTextBoxBanco.Text, ASPxTextBoxAgencia.Text, ASPxTextBoxContaCredito.Text, ASPxTextBoxNomeFavorecido.Text))
            {
                PageMaster.ExibeMensagem(ResourceMensagens.MensagemTodosCamposObrigatorios);
                return false;
            }

            if (Convert.ToDecimal(ASPxTextBoxValor.Value.ToString().Replace(",", ".")) <= 0)
            {
                PageMaster.ExibeMensagem("Valor Inválido!");
                return false;
            }

            return true;
        }

        protected void SalvarSaldoDevedor_Click(Object sender, EventArgs e)
        {
            if (!ValidaInformacoes()) return;

            EmpresaSolicitacao es = FachadaInformarSaldoDevedor.ObtemSolicitacaoFuncOrigem(Id.Value);
            if (es == null)
                es = FachadaInformarSaldoDevedor.ObtemSolicitacaoOrigem(Id.Value);

            if (es != null)
            {
                FachadaAverbacoes.salvarSaldoDevedor(es.IDEmpresaSolicitacao, Id.Value, Sessao.IdBanco, Sessao.UsuarioLogado.IDUsuario, DateTime.Now, DateEditValidade.Date, Convert.ToDecimal(ASPxTextBoxValor.Value.ToString()), Convert.ToInt32(DropDownListFormaPagamento.SelectedValue), ASPxTextBoxIdentificado.Text, ASPxTextBoxBanco.Text, ASPxTextBoxAgencia.Text, ASPxTextBoxContaCredito.Text, ASPxTextBoxNomeFavorecido.Text, ASPxTextBoxObs.Text);

                PageMaster.ExibeMensagem(ResourceMensagens.MensagemSucessoOperacao);
                PageMaster.FechaControleVoltaChamada();
            }
            else
            {
                PageMaster.ExibeMensagem(ResourceMensagens.MensagemFalhaOperacao);
            }

        }

        protected void DropDownListFormaPagamento_SelectIndexChanged(Object sender, EventArgs e)
        {
            DadosTED.Visible = (DropDownListFormaPagamento.SelectedValue != null && Convert.ToInt32(DropDownListFormaPagamento.SelectedValue) == (int)Enums.TipoPagamento.TED);
        }
    }
}