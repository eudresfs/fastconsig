using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CP.FastConsig.BLL;
using CP.FastConsig.DAL;

namespace CP.FastConsig.Facade
{
    public static class FachadaCentralSimulacao
    {
        public static Pessoa obtemPessoa(string parametro)
        {
            return Funcionarios.ObtemPessoa(parametro);
        }

        public static List<Funcionario> obtemFuncionariosPorPessoa(int idPessoa)
        {
            return Funcionarios.ObtemFuncionariosPorPessoa(idPessoa);
        }

        public static IEnumerable<GrupoMargem> ListarMargens(int id)
        {
            return Funcionarios.ListarMargens(id);
        }

        public static Funcionario obtemFuncionario(string matricula)
        {
            return Funcionarios.ObtemFuncionario(matricula);
        }

        public static Averbacao obtemAverbacao(int idAverbacao)
        {
            return Averbacoes.ObtemAverbacao(idAverbacao);
        }

        public static int obtemParcelaAtual(int idAverbacao)
        {
            return Averbacoes.ObtemParcelaAtual(idAverbacao);
        }

        public static int obtemParcelasRestantes(int idAverbacao)
        {
            return Averbacoes.ObtemParcelasRestantes(idAverbacao);
        }

        public static decimal obtemSaldoAberto(int idAverbacao)
        {
            return Averbacoes.CalculaSaldoRestante(idAverbacao);
            //return Averbacoes.obtemSaldoAberto(idAverbacao);
        }

        public static List<Averbacao> obtemAverbacoesAtivas(int idFuncionario)
        {
            return Funcionarios.ObtemAverbacoesAtivas(idFuncionario);
        }

        public static Recurso obtemRecurso(int idRecurso)
        {
            return Geral.ObtemRecurso(idRecurso);
        }

        public static List<EmpresaCoeficienteDetalhe> ObtemCoeficientesBanco(int idBanco)
        {
            return Empresas.ObtemCoeficientesBanco(idBanco);
        }
    }
}
