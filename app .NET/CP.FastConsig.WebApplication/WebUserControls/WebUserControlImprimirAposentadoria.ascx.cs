using System;
using CP.FastConsig.DAL;
using CP.FastConsig.Facade;
using CP.FastConsig.WebApplication.Auxiliar;
using System.Linq;

namespace CP.FastConsig.WebApplication.WebUserControls
{
    public partial class WebUserControlImprimirAposentadoria : CustomUserControl
    {
        public void Configura(Funcionario func)
        {
            // Dados do Termo
            impData.Text = DateTime.Today.ToString("dd/MM/yyyy");

            // Dados Pessoasis.
            impNome.Text = func.Pessoa.Nome;
            impCPF.Text = func.Pessoa.CPF;
            impRG.Text = func.Pessoa.RG;
            impDataNasc.Text = func.Pessoa.DataNascimento == null ? "---" : func.Pessoa.DataNascimento.Value.ToString("dd/MM/yyyy");
            impEndereco.Text = func.Pessoa.Endereco;
            impBairro.Text = func.Pessoa.Bairro;
            impCidade.Text = func.Pessoa.Cidade;
            impUF.Text = func.Pessoa.Estado;
            impCep.Text = func.Pessoa.CEP;
            impEmail.Text = func.Pessoa.Email;
            impTelefone.Text = func.Pessoa.Fone;
            impCelular.Text = func.Pessoa.Celular;

            // Dados Funcionais
            impMatricula.Text = func.Matricula;
            impLocal.Text = func.NomeLocalFolha;
            impSetor.Text = func.NomeSetorFolha;
            impCargo.Text = func.NomeCargoFolha;
            impCategoria.Text = func.FuncionarioCategoria.Nome;
            impRegime.Text = func.NomeRegimeFolha;
            impDataAdm.Text = func.DataAdmissao == null ? "---" : func.DataAdmissao.Value.ToString("dd/MM/yyyy");
            impSituacao.Text = func.NomeSituacao;

            GridViewAverbacoes.DataSource = func.Averbacao.Where(x => x.Ativo == 1 && x.AverbacaoSituacao.DeduzMargem);
            GridViewAverbacoes.DataBind();
        }
    }
}