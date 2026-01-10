using System;
using CP.FastConsig.DAL;
using CP.FastConsig.Facade;
using CP.FastConsig.WebApplication.Auxiliar;

namespace CP.FastConsig.WebApplication.WebUserControls
{
    public partial class WebUserControlTermoAverbacao : CustomUserControl
    {
        public void ConfiguraTermo(Averbacao averbacao)
        {
            // Dados do Termo
            impNumero.Text = averbacao.Numero;
            impData.Text = averbacao.Data.ToString("dd/MM/yyyy");
            impBanco.Text = averbacao.Empresa1.Nome;
            impCNPJ.Text = averbacao.Empresa1.CNPJ;

            // Dados Pessoasis.
            impNome.Text = averbacao.Funcionario.Pessoa.Nome;
            impNome2.Text = averbacao.Funcionario.Pessoa.Nome;
            impCPF.Text = averbacao.Funcionario.Pessoa.CPF;
            impRG.Text = averbacao.Funcionario.Pessoa.RG;
            impDataNasc.Text = averbacao.Funcionario.Pessoa.DataNascimento == null ? "---" : averbacao.Funcionario.Pessoa.DataNascimento.Value.ToString("dd/MM/yyyy");
            impEndereco.Text = averbacao.Funcionario.Pessoa.Endereco;
            impBairro.Text = averbacao.Funcionario.Pessoa.Bairro;
            impCidade.Text = averbacao.Funcionario.Pessoa.Cidade;
            impUF.Text = averbacao.Funcionario.Pessoa.Estado;
            impCep.Text = averbacao.Funcionario.Pessoa.CEP;
            impEmail.Text = averbacao.Funcionario.Pessoa.Email;
            impTelefone.Text = averbacao.Funcionario.Pessoa.Fone;
            impCelular.Text = averbacao.Funcionario.Pessoa.Celular;

            // Dados Funcionais
            impMatricula.Text = averbacao.Funcionario.Matricula;
            impLocal.Text = averbacao.Funcionario.NomeLocalFolha;
            impSetor.Text = averbacao.Funcionario.NomeSetorFolha;
            impCargo.Text = averbacao.Funcionario.NomeCargoFolha;
            impCategoria.Text = averbacao.Funcionario.FuncionarioCategoria.Nome;
            impRegime.Text = averbacao.Funcionario.NomeRegimeFolha;
            impDataAdm.Text = averbacao.Funcionario.DataAdmissao == null ? "---" : averbacao.Funcionario.DataAdmissao.Value.ToString("dd/MM/yyyy");
            impSituacao.Text = averbacao.Funcionario.NomeSituacao;

            //  Dados da Averbação
            impProduto.Text = averbacao.Produto.Nome;
            impValorContrato.Text = averbacao.ValorContratado.ToString("C");
            impPrazo.Text = averbacao.Prazo == null ? "---" : averbacao.Prazo.Value.ToString();
            impValorParcela.Text = averbacao.ValorParcela.ToString("C");
            impValorConsignado.Text = averbacao.ValorDevidoTotal == null ? "---" : averbacao.ValorDevidoTotal.Value.ToString("C");
            impObs.Text = averbacao.Obs;
            impMesInicio.Text = averbacao.CompetenciaInicial;
            impMesFim.Text = averbacao.CompetenciaFinal;
            impFator.Text = averbacao.Coeficiente == null ? "---" : averbacao.Coeficiente.Value.ToString();
            impCET.Text = averbacao.CET == null ? "---" : averbacao.CET.Value.ToString();
            impDataAverbacao.Text = string.Format("{0}, {1} de {2} de {3} {4}", FachadaTermoAverbacao.ObtemEmpresa(Convert.ToInt32(FachadaGeral.IdEmpresaConsignante())).Cidade, averbacao.CreatedOn == null ? "---" : averbacao.CreatedOn.Value.ToString("dd"), averbacao.CreatedOn == null ? "---" : averbacao.CreatedOn.Value.ToString("MMMM"), averbacao.CreatedOn == null ? "---" : averbacao.CreatedOn.Value.ToString("yyyy"), averbacao.CreatedOn == null ? "---" : averbacao.CreatedOn.Value.ToString("HH:mm"));
            impDataAverbacao2.Text = string.Format("{0}, {1} de {2} de {3} {4}", FachadaTermoAverbacao.ObtemEmpresa(Convert.ToInt32(FachadaGeral.IdEmpresaConsignante())).Cidade, DateTime.Today.ToString("dd"), DateTime.Today.ToString("MMMM"), DateTime.Today.ToString("yyyy"), DateTime.Now.ToString("HH:mm"));
        }
    }
}