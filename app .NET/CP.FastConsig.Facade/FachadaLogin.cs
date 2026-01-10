using System;
using System.Collections.Generic;
using System.Linq;
using CP.FastConsig.BLL;
using CP.FastConsig.Common;
using CP.FastConsig.DAL;

namespace CP.FastConsig.Facade
{

    public static class FachadaLogin
    {

        public static Usuario ObtemUsuarioPorLogin(string login)
        {
            return Usuarios.ObtemUsuarioPorLogin(login);
        }

        public static UsuarioPerfil ObtemUsuarioPerfil(int idUsuarioPerfil)
        {
            return Usuarios.ObtemUsuarioPerfil(idUsuarioPerfil);
        }

        public static void IncrementaQuantidadeAcessos(int idUsuario)
        {
            Usuarios.IncrementaQuantidadeAcessos(idUsuario);
        }

        public static void AtualizaUsuarioPrimeiroAcesso(int idUsuario, string apelidoLogin, string senha)
        {
            Usuarios.AtualizaUsuarioPrimeiroAcesso(idUsuario, apelidoLogin, senha);
        }

        public static bool EmpresaBloqueada(int idEmpresa)
        {
            return Consignatarias.EmpresaBloqueada(idEmpresa);
        }

        public static EmpresaSuspensao ObtemSuspensao(int idEmpresa)
        {
            return Consignatarias.ObtemSuspensoes(idEmpresa).ToList().FirstOrDefault();
        }

        public static IEnumerable<Funcionario> ObtemFuncionariosUsuario(int idUsuario)
        {

            Pessoa pessoa = Pessoas.ObtemPessoaPorIdUsuario(idUsuario);

            if (pessoa == null) return new List<Funcionario>();

            return Funcionarios.ObtemFuncionariosPorPessoa(pessoa.IDPessoa);

        }

        public static bool FuncionarioBloqueado(int idFuncionario)
        {
            return Averbacoes.FuncionarioBloqueado(idFuncionario);
        }

        public static IEnumerable<Empresa> ObtemConsignatariasAgente(int idAgente)
        {
            return Empresas.ObtemConsignatariasAgente(idAgente);
        }

        public static IEnumerable<Perfil> ObtemTodosPerfis()
        {
            return Perfis.Listar();
        }

        public static IQueryable<Empresa> ListaConsignatarias()
        {
            return Empresas.ListaConsignatarias();
        }

        public static string ObtemNomeEmpresaConsignante()
        {
            return Empresas.ObtemEmpresa(Convert.ToInt32(FachadaGeral.IdEmpresaConsignante())).Fantasia;
        }

        public static int ObtemIdModulo(int idPerfil)
        {
            return Perfis.ObtemPerfil(idPerfil).IDModulo;
        }

        public static List<Perfil> ListaPerfisEmpresa(int idEmpresa)
        {
            return Perfis.ObtemPerfis((int) Enums.Modulos.Consignataria, idEmpresa);
        }

        public static Empresa ObtemEmpresa(int idBanco)
        {
            return Empresas.ObtemEmpresa(idBanco);
        }

    }

}