using System.Collections.Generic;
using System.Linq;
using CP.FastConsig.BLL;
using CP.FastConsig.DAL;

namespace CP.FastConsig.Facade
{

    public static class FachadaUsuariosPermissoes
    {

        public static void RemoveUsuario(int idUsuario)
        {
            Usuarios.RemoveUsuario(idUsuario);
        }

        public static List<Modulo> ObtemModulos()
        {
            return Modulos.ObtemModulos();
        }

        public static List<Empresa> ObtemConsignatarias()
        {
            return Consignatarias.ObtemConsignatarias().ToList();
        }

        public static List<Empresa> ObtemAgentesConsignataria(int idBanco)
        {
            return Empresas.ListaAgentesEmpresa(idBanco).ToList();
        }

        public static List<Empresa> ObtemAgentesConsignatarias()
        {
            return Empresas.ListaAgentes().ToList();
        }

        public static List<Averbacao> ObtemAverbacoesUsuario(int idUsuario)
        {

            List<Averbacao> averbacoes = new List<Averbacao>();

            Pessoa pessoa = Pessoas.ObtemPessoaPorIdUsuario(idUsuario);

            if (pessoa == null) return averbacoes;

            List<Funcionario> funcionarios = Funcionarios.ObtemFuncionariosPorPessoa(pessoa.IDUsuario ?? 0);

            foreach (Funcionario funcionario in funcionarios) averbacoes.AddRange(Averbacoes.ListaAverbacao().Where(x => x.IDFuncionario.Equals(funcionario.IDFuncionario)).ToList());
            
            return averbacoes;

        }

        public static Usuario ObtemUsuario(int idUsuario)
        {
            return Usuarios.ObtemUsuario(idUsuario);
        }

    }

}