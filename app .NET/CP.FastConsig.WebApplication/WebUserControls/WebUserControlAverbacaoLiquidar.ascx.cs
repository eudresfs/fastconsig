using System;
using CP.FastConsig.WebApplication.Auxiliar;
using CP.FastConsig.Facade;
using CP.FastConsig.DAL;
using CP.FastConsig.Common;

namespace CP.FastConsig.WebApplication.WebUserControls
{

    public partial class WebUserControlAverbacaoLiquidar : CustomUserControl
    {
        #region Constantes

        private const string TituloPagina = "Liquidar Averbação";

        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {

            if (EhPostBack || ControleCarregado) return;

            PopularDados();
            EhPostBack = true;

        }

        public override void ConfiguraTopo()
        {
            PageMaster.Titulo = TituloPagina;
        }

        private void PopularDados()
        {
            Averbacao con = FachadaConciliacao.ObtemAverbacao(Id.Value);

            LabelMatriculaFuncionario.Text = con.Funcionario.Matricula;
            LabelNomeFuncionario.Text = con.Funcionario.Pessoa.Nome;
            LabelCpfFuncionario.Text = con.Funcionario.Pessoa.CPFMascara;

            LabelNumeroAverbacao.Text = con.Numero;
            LabelConsignataria.Text = con.Empresa1.Nome;
            LabelPrazo.Text = con.Prazo.Value.ToString();
            LabelSituacaoAtual.Text = con.AverbacaoSituacao.Nome;
            LabelValorParcela.Text = con.ValorParcela.ToString();
        }

        protected void SalvarClick(Object sender, EventArgs e)
        {
            
            FachadaGerenciarAverbacao.Liquidar(Id.Value, txtMotivo.Text, Sessao.UsuarioLogado.IDUsuario);

            WebUserControlGerenciarAverbacaoConsulta controleAnterior = (WebUserControlGerenciarAverbacaoConsulta) ControleAnterior;

            controleAnterior.AtualizaGrid();

            PageMaster.ExibeMensagem(ResourceMensagens.MensagemSucessoOperacao);

            PageMaster.Voltar();

        }

    }

}