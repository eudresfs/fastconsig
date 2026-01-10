using System;
using CP.FastConsig.WebApplication.Auxiliar;
using CP.FastConsig.Facade;
using CP.FastConsig.DAL;
using CP.FastConsig.Common;

namespace CP.FastConsig.WebApplication.WebUserControls
{

    public partial class WebUserControlAverbacaoCancelar : CustomUserControl
    {
        #region Constantes

        private const string TituloPagina = "Cancelar Averbação";

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
            Enums.CancelamentoIndevido tipo;
            bool bcancelou = FachadaGerenciarAverbacao.Cancelar(Id.Value, txtMotivo.Text, Sessao.UsuarioLogado.IDUsuario, out tipo);

            if (!bcancelou)
            {
                if (tipo == Enums.CancelamentoIndevido.ParticipandoCompra)
                    PageMaster.ExibeMensagem("Não pode ser cancelada! Esta averbação esta participando de processo de compra/renegociação!");
                else if (tipo == Enums.CancelamentoIndevido.ExisteSolicitacoesProcessadas)
                    PageMaster.ExibeMensagem("Não pode ser cancelada! Existem solicitações do processo de compra já processadas!");
                return;
            }

            PageMaster.ExibeMensagem(ResourceMensagens.MensagemSucessoOperacao);
            PageMaster.Voltar();

        }

    }

}