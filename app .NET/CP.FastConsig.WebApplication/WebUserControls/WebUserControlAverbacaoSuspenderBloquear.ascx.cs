using System;
using CP.FastConsig.WebApplication.Auxiliar;
using CP.FastConsig.Facade;
using CP.FastConsig.DAL;
using CP.FastConsig.Common;

namespace CP.FastConsig.WebApplication.WebUserControls
{

    public partial class WebUserControlAverbacaoSuspenderBloquear : CustomUserControl
    {

        protected void Page_Load(object sender, EventArgs e)
        {

            if (EhPostBack || ControleCarregado) return;

            PopularDados();
            EhPostBack = true;

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

            bool AverbacaoParaSuspender = Convert.ToBoolean(ParametrosConfiguracao[1]);

            if (!AverbacaoParaSuspender) fsSuspender.Visible = false;

        }

        protected void SalvarSuspensaoClick(Object sender, EventArgs e)
        {

            if (fsSuspender.Visible)
            {
                bool suspender = ASPxRadioButtonSuspender.Checked;
                FachadaGerenciarAverbacao.SuspenderBloquear(Id.Value, suspender, txtMotivo.Text, Sessao.IdBanco);
            }
            else
            {
                FachadaGerenciarAverbacao.AtivarAverbacaoSuspenso(Id.Value, txtMotivo.Text, Sessao.IdBanco);
            }

            PageMaster.Voltar();

        }

    }

}