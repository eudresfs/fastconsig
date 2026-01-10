using System.Collections.Generic;
using System.Linq;
using CP.FastConsig.Common;
using CP.FastConsig.DAL;
using System;

namespace CP.FastConsig.BLL
{

    public static class Perfis
    {

        public static List<Perfil> ObtemPerfis(int idModulo)
        {
            Repositorio<Perfil> repositorioPerfil = new Repositorio<Perfil>();
            return repositorioPerfil.Listar().Where(x => x.IDModulo.Equals(idModulo)).ToList();
        }

        public static List<Perfil> ObtemPerfis(int idModulo, int idConsignataria)
        {
            return new Repositorio<Perfil>().Listar().Where(x => x.IDModulo.Equals(idModulo) && (x.IDEmpresa == null || x.IDEmpresa.Value.Equals(idConsignataria))).OrderBy(x => x.Nome).ToList();
        }

        public static Perfil ObtemPerfil(int idPerfil)
        {
            return new Repositorio<Perfil>().ObterPorId(idPerfil);
        }


        public static IQueryable<Perfil> Listar(int IdEmpresa, int IdModulo)
        {
            var dados = new Repositorio<Perfil>().Listar();

            if (IdEmpresa == Convert.ToInt32(Geral.IdEmpresaConsignante()))
            {
                dados = dados.Where(x => x.IDEmpresa.Equals(null) && x.IDModulo == IdModulo);
            }
            else
            {
                dados = dados.Where(x => (x.IDEmpresa == IdEmpresa || x.IDEmpresa.Equals(null)) && x.IDModulo == IdModulo);
            }
            return dados;
        }

        public static void CopiarPerfil(int IdEmpresa, int De, int Para)
        {
            // excluindo definição atual
            Repositorio<PermissaoUsuario> repusuarioPara = new Repositorio<PermissaoUsuario>();
            repusuarioPara.Excluir("IDEmpresa = " + IdEmpresa.ToString() + " and IDPerfil = " + Para.ToString());

            Repositorio<PermissaoUsuario> repusuarioDe = new Repositorio<PermissaoUsuario>();
            var permissoesDe = repusuarioDe.Listar().Where(x => x.IDEmpresa == IdEmpresa && x.IDPerfil == De);


            Repositorio<PermissaoUsuario> reppu = new Repositorio<PermissaoUsuario>();
            // incluindo novos
            foreach (var item in permissoesDe)
            {
                PermissaoUsuario pu = new PermissaoUsuario();
                pu.IDEmpresa = IdEmpresa;
                pu.IDPerfil = Para;
                pu.IDPermissao = item.IDPermissao;
                pu.IDRecurso = item.IDRecurso;

                reppu.Incluir(pu);
            }
        }

        public static void RemovePerfilUsuario(int idUsuarioPerfil)
        {
            new Repositorio<UsuarioPerfil>().Excluir(idUsuarioPerfil);
        }

        public static IEnumerable<UsuarioPerfil> ObtemPerfisUsuario(int idUsuarioEdicao)
        {
            return new Repositorio<UsuarioPerfil>().Listar().Where(x => x.IDUsuario.Equals(idUsuarioEdicao));
        }

        public static IEnumerable<Perfil> Listar()
        {
            return new Repositorio<Perfil>().Listar();
        }


        public static IEnumerable<Perfil> ObtemPefisAgente()
        {
            return new Repositorio<Perfil>().Listar().Where(x => x.Sigla.ToUpper().Equals("AG")).ToList();
        }
    }

}