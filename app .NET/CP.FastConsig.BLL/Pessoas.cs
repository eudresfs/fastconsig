using System;
using System.Linq;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CP.FastConsig.Common;
using CP.FastConsig.DAL;

namespace CP.FastConsig.BLL
{

    public static class Pessoas
    {

        public static Pessoa ObtemPessoa(int idPessoa)
        {
            return new Repositorio<Pessoa>().ObterPorId(idPessoa);
        }

        public static Pessoa ObtemPessoa(string cpf)
        {
            return new Repositorio<Pessoa>().Listar().SingleOrDefault(x => x.CPF.Equals(cpf));
        }

        public static int AdicionaPessoa(string nome, string cpf, string email, string telefone, int idConsignataria)
        {
            return SalvaPessoa(null, nome, cpf, email, telefone, idConsignataria);
        }

        private static int SalvaPessoa(int? idPessoa, string nome, string cpf, string email, string telefone, int idConsignataria)
        {

            Repositorio<Pessoa> repositorioPessoa = new Repositorio<Pessoa>();

            bool inclusao = (idPessoa == null);

            Pessoa pessoa = inclusao ? new Pessoa() : repositorioPessoa.ObterPorId(idPessoa.Value);
            Funcionario funcionario = new Funcionario();

            if (pessoa == null) return 0;

            pessoa.Nome = nome;
            pessoa.CPF = cpf;
            pessoa.Email = email;
            pessoa.Celular = telefone;
            pessoa.Ativo = 1;
            pessoa.RG = "12345";

            funcionario.IDConsignante = Convert.ToInt32(Geral.IdEmpresaConsignante());
            funcionario.Pessoa = pessoa;
            funcionario.Matricula = Guid.NewGuid().ToString().Substring(0, 15);
            funcionario.IDPessoa = pessoa.IDPessoa;
            funcionario.IDFuncionarioCategoria = 1;
            funcionario.IDFuncionarioRegime = 1;
            funcionario.IDFuncionarioSituacao = 1;

            if (inclusao) repositorioPessoa.Incluir(pessoa);
            else repositorioPessoa.Alterar(pessoa);

            return pessoa.IDPessoa;

        }

        public static void AtualizaPessoa(int idPessoa, string nome, string cpf, string email, string telefone, int idConsignataria)
        {
            SalvaPessoa(idPessoa, nome, cpf, email, telefone, idConsignataria);
        }

        public static Pessoa ObtemPessoaPorIdUsuario(int idUsuario)
        {
            return new Repositorio<Pessoa>().Listar().Where(x => x.IDUsuario != null).FirstOrDefault(x => x.IDUsuario.Value.Equals(idUsuario));
        }

    }

}