using System;
using System.Collections.Generic;
using System.Linq;
using CP.FastConsig.BLL;
using CP.FastConsig.DAL;

namespace CP.FastConsig.Facade
{

    public static class FachadaUsuariosPermissoesEdicao
    {

        public static Usuario ObtemUsuario(string cpf)
        {
            return Usuarios.ObtemUsuario(cpf);
        }

        public static List<Perfil> ObtemPefis(int idModulo)
        {
            return Perfis.ObtemPerfis(idModulo);
        }

        public static List<Perfil> ObtemPefis(int idModulo, int idConsignataria)
        {
            return Perfis.ObtemPerfis(idModulo, idConsignataria);
        }

        public static void AlteraUsuario(int idUsuario, string nome, string cpf, string login, string email, string telefone, string senhaProvisoria, int idPerfil, int idConsignataria, int idmodulo)
        {
            Usuarios.AlteraUsuario(idUsuario, nome, cpf, login, email, telefone, senhaProvisoria, idPerfil, idConsignataria, idmodulo);
        }

        public static Usuario ObtemUsuario(int idUsuario)
        {
            return Usuarios.ObtemUsuario(idUsuario);
        }

        public static IQueryable<Empresa> ListaEmpresas()
        {
            return Empresas.ListaEmpresas();
        }

        public static List<Modulo> ObtemModulos()
        {
            return Modulos.ObtemModulos();
        }

        public static int AdicionaUsuario(string nome, string cpf, string login, string email, string telefone, string senhaProvisoria, int idPerfil, int idConsignataria, int idmodulo, string senhaCadastradaNoCenter)
        {
            return Usuarios.AdicionaUsuario(nome, cpf, login, email, telefone, senhaProvisoria, idPerfil, idConsignataria, idmodulo, senhaCadastradaNoCenter);
        }

        public static void RemovePerfilUsuario(int idUsuarioPerfil)
        {
            Perfis.RemovePerfilUsuario(idUsuarioPerfil);
        }

        public static IEnumerable<UsuarioPerfil> ObtemPefisUsuario(int idUsuarioEdicao)
        {
            return Perfis.ObtemPerfisUsuario(idUsuarioEdicao);
        }

        public static void RemoveUsuario(int idUsuarioEmEdicao)
        {
            Usuarios.RemoveUsuario(idUsuarioEmEdicao);
        }

        public static Empresa ObtemEmpresa(int idEmpresa)
        {
            return Empresas.ObtemEmpresa(idEmpresa);
        }

        public static Usuario ObtemUsuarioPorLogin(string login)
        {
            return Usuarios.ObtemUsuarioPorLogin(login);
        }

        public static int ObtemIdConsignanteCenter()
        {
            return Convert.ToInt32(Geral.IdEmpresaConsignanteCenter());
        }

        public static IEnumerable<EmpresaSolicitacaoTipo> ObterTiposSolicitacoes(int idModulo)
        {
            return Solicitacoes.ObterTiposSolicitacoes(idModulo);
        }

        public static void AdicionaResponsabilidade(int idUsuarioEmEdicao, int idBanco, int idSolicitacaoTipo)
        {
            Solicitacoes.AdicionaResponsabilidade(idUsuarioEmEdicao, idBanco, idSolicitacaoTipo);
        }

        public static void RemoveResponsabilidade(int idUsuarioEmEdicao, int idBanco, int idSolicitacaoTipo)
        {
            Solicitacoes.RemoveResponsabilidade(idUsuarioEmEdicao, idBanco, idSolicitacaoTipo);
        }

        public static IQueryable<UsuarioResponsabilidade> ListaResponsabilidades(int idUsuarioEmEdicao, int idBanco)
        {
            return Solicitacoes.ListaResponsabilidades(idUsuarioEmEdicao, idBanco);
        }

        public static IEnumerable<Empresa> ListaAgentesEmpresa(int idBanco)
        {
            return Empresas.ListaAgentesEmpresa(idBanco);
        }


        public static void BloqueiaUsuario(int idUsuario)
        {
            Usuarios.BloqueiaUsuario(idUsuario);
        }

        public static IEnumerable<Empresa> ListaAgentes()
        {
            return Empresas.ListaAgentes();
        }

        public static IEnumerable<Perfil> ObtemPefisAgente()
        {
            return Perfis.ObtemPefisAgente();
        }

    }

}