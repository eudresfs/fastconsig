using System.Collections.Generic;
using System.Linq;
using CP.FastConsig.BLL;
using CP.FastConsig.DAL;


namespace CP.FastConsig.Facade
{

    public static class FachadaPermissoesAcesso
    {

        public static IEnumerable<MenuPermissaoAcesso> MenuOpcoes()
        {
            return Permissoes.MenuOpcoes();
        }

        public static IEnumerable<MenuPermissaoAcesso> MenuArvore(int idempresa, int idmodulo)
        {
            return Permissoes.MenuArvore(idempresa, idmodulo);
        }

        public static void SalvarPermissaoAcesso(int id, bool permitir, int idrecurso, int idempresa, int idperfil, int idpermissao)
        {
            Permissoes.SalvarPermissaoAcesso(id, permitir, idrecurso, idempresa, idperfil, idpermissao);    
        }

        public static IQueryable<Modulo> ListarModulos()
        {
            return new Repositorio<Modulo>().Listar();
        }

        public static bool CheckPermissao(int IDRecurso, int IDEmpresa, int IDPerfil, int IDPermissao)
        {
            return Permissoes.CheckPermissao(IDRecurso, IDEmpresa, IDPerfil, IDPermissao);
        }
    }
}