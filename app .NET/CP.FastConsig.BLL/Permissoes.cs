using System.Collections.Generic;
using System.Linq;
using CP.FastConsig.DAL;
using System;
using CP.FastConsig.Common;
using CP.FastConsig.Util;
using System.Data.Objects;
using System.Data;
using System.Data.SqlClient;
using Microsoft.Data.Extensions;

namespace CP.FastConsig.BLL
{

    public static class Permissoes
    {

        public static bool CheckPermissao(int IDRecurso, int IDEmpresa, int IDPerfil, int IDPermissao)
        {
            return new Repositorio<PermissaoUsuario>().Listar().Any(x => x.IDEmpresa == IDEmpresa && x.IDRecurso == IDRecurso && x.IDPerfil == IDPerfil && x.IDPermissao == IDPermissao);
        }

        public static IEnumerable<MenuPermissaoAcesso> MenuOpcoes()
        {
            var recursos = new Repositorio<Recurso>().Listar();
            var perfis = new Repositorio<Perfil>().Listar();
            var permissoes = new Repositorio<PermissaoRecurso>().Listar();

            var dados = from p in permissoes
                        select new MenuPermissaoAcesso { IDRecurso = p.Recurso.IDRecurso, IDRecursoPai = p.Recurso.IDRecursoPai, Nome = p.Recurso.Nome, idpermissao = p.IDPermissao, Permissao = p.Permissao.Nome };
            return dados;
        }

        public static IEnumerable<MenuPermissaoAcesso> MenuArvore(int idempresa, int idmodulo)
        {
            var perfis = new Repositorio<Perfil>().Listar();

            ObjectContext ctx = new Repositorio<Empresa>().ObterObjectContext();

            var comando = ctx.CreateStoreCommand("SP_ExecutarGerarMenuArvore", CommandType.StoredProcedure, new SqlParameter("IdEmpresa", idempresa), new SqlParameter("IdModulo", idmodulo));
            var dados = comando.Materialize<MenuPermissaoAcesso>();

            return dados;
        }

        public static void SalvarPermissaoAcesso(int id, bool permitir, int idrecurso, int idempresa, int idperfil, int idpermissao)
        {
            Repositorio<PermissaoRecurso> rep = new Repositorio<PermissaoRecurso>();
            PermissaoRecurso pr = rep.ObterPorId(id);

            Repositorio<PermissaoUsuario> reppu = new Repositorio<PermissaoUsuario>();
            var pu = reppu.Listar().FirstOrDefault(x => x.IDEmpresa == idempresa && x.IDRecurso == idrecurso && x.IDPerfil == idperfil && x.IDPermissao == idpermissao);
            if (pu != null)
                reppu.Excluir(pu);

            if (permitir)
                reppu.Incluir(new PermissaoUsuario() { IDEmpresa = idempresa, IDPerfil = idperfil, IDPermissao = idpermissao, IDRecurso = idrecurso });
        }
    }

}
